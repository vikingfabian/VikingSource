using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.Players
{

    partial class RemotePlayer
    {
        static List<Army> netCollArmies = new List<Army>(16);

        public const int OverviewSendChunkSize = 8;
        static HashSet<int> CitiesInView = new HashSet<int>();
        static HashSet<int> FactionsInView = new HashSet<int>();

        public Grid2D<RemoteTile> remoteTileGrid;
        public bool[] citiesRecieved;
        public bool[] factionsRecieved;

        public void InitData()
        {
            remoteTileGrid = new Grid2D<RemoteTile>(DssRef.world.Size);
            citiesRecieved = new bool[DssRef.world.cities.Count];
            factionsRecieved = new bool[DssRef.world.factions.Count];
        }

        public bool Net_HostMapUpdate_async()
        {           
            bool sentMap = true;

            if (playerCulling.enterArea.size.HasValue())
            {
                CitiesInView.Clear();
                FactionsInView.Clear();

                if (findMissingTile(out IntVector2 tilePos, false))
                {
                    var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssWorldTiles, Network.PacketReliability.Reliable, out var packet);
                    {
                        DssRef.world.writeNet_Tile(w, tilePos);
                    }
                    packet.EndWrite_Asynch();
                }
                else if (FactionsInView.Count > 0)
                {
                    var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssFactions, Network.PacketReliability.Reliable, out var packet);
                    {
                        DssRef.world.writeNet_Factions(w, FactionsInView);
                    }
                    packet.EndWrite_Asynch();
                }
                else if (CitiesInView.Count > 0)
                {
                    var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssCities, Network.PacketReliability.Reliable, out var packet);
                    {
                        DssRef.world.writeNet_Cities(w, CitiesInView);
                    }
                    packet.EndWrite_Asynch();
                }
                else if (playerCulling.detailLayer && findMissingTile(out IntVector2 subtilePos, true))
                {
                    var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssWorldSubTiles, Network.PacketReliability.Reliable, out var packet);
                    {
                        DssRef.world.writeNet_SubTile(w, subtilePos);
                    }
                    packet.EndWrite_Asynch();
                }
                else
                {
                    sentMap = false;
                }

                return sentMap;

                //todo hitta alla städer och armeer i sight
            }

            return false;

            bool findMissingTile(out IntVector2 tilePos, bool subTile)
            {
                Rectangle2 area;

                if (subTile)
                {
                    area = playerCulling.enterArea;
                }
                else
                {
                    area = playerCulling.screenAreaRaw;
                }

                
                ForXYLoop loop = new ForXYLoop(area);
                while (loop.Next())
                {
                    if (remoteTileGrid.InBounds(loop.Position))
                    {
                        if (!remoteTileGrid.Get(loop.Position).HasTile(subTile))
                        {
                            tilePos = loop.Position;
                            return true;
                        }
                        if (!subTile)
                        {
                            var tile = DssRef.world.tileGrid.Get(loop.Position);
                            if (!citiesRecieved[tile.CityIndex])
                            {
                                CitiesInView.Add(tile.CityIndex);
                            }

                            int faction = tile.City().factionIndex;
                            if (!factionsRecieved[faction])
                            {
                                FactionsInView.Add(faction);
                            }
                        }
                    }
                }

                tilePos = IntVector2.NegativeOne;
                return false;
            }

            
        }

        public void Net_UpdateArmies(ref int maxPackets)
        {
            const int GroupsPerPacket = 10;

            if (playerCulling.farLayer == false)
            {
                DssRef.world.unitCollAreaGrid.net_collectArmies(playerCulling.screenAreaRaw, netCollArmies);

                int waitSeconds;
                if ( netCollArmies.Count <= 2)
                {
                    waitSeconds = 5;
                }
                else if (netCollArmies.Count <= 10)
                {
                    waitSeconds = 10;
                }
                else 
                {
                    waitSeconds = 20;
                }

                foreach (Army army in netCollArmies)
                {
                    if (army.lastNetUpdate.secPassed(waitSeconds))
                    {
                        {
                            var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssArmyStatus, Network.PacketReliability.Unrelyable, out var packet);
                            {
                                Army.NetWriteArmy(w, army);
                                army.lastNetUpdate.setNow();
                            }
                            packet.EndWrite_Asynch();
                        }

                        if (army.groups.Count > 0)
                        {
                            var groupC = army.groups.counter();

                            int count = 0;

                            while (groupC.HasMore())
                            {                                
                                var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssSoldierGroupStatus, Network.PacketReliability.Unrelyable, out var packet);
                                {
                                    w.Write((ushort)army.factionIndex);
                                    w.Write((ushort)army.myIndex);

                                    while (--count < GroupsPerPacket && groupC.Next())
                                    {
                                        Army.NetWriteGroup(w, groupC.sel);
                                        army.lastNetUpdate.setNow();
                                    }

                                    w.Write(ushort.MaxValue);
                                }
                                packet.EndWrite_Asynch();
                                
                            }
                        }
                    }
                }
            }
        }

        public HashSet<int> GetAllCitiesInView()
        {
            CitiesInView.Clear();

            ForXYLoop loop = new ForXYLoop(playerCulling.enterArea);
            while (loop.Next())
            {  
                
                var tile = DssRef.world.tileGrid.Get(loop.Position);
                
                CitiesInView.Add(tile.CityIndex);
                
                
            }

            return CitiesInView;
        }

        public void Net_HostObjectsUpdate_async()
        {

        }
    }

    struct RemoteTile
    {
        public bool overview, detail;

        public bool HasTile(bool subTile)
        { 
            return subTile? detail : overview;
        }
    }
}
