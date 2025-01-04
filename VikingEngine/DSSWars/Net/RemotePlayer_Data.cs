using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.Players
{

    partial class RemotePlayer
    {
        public const int OverviewSendChunkSize = 8;
        static HashSet<int> CitiesInView = new HashSet<int>();
        static HashSet<int> FactionsInView = new HashSet<int>();

        public Grid2D<RemoteTile> remoteTileGrid;
        bool[] citiesRecieved;
        bool[] factionsRecieved;

        public void InitData()
        {
            remoteTileGrid = new Grid2D<RemoteTile>(DssRef.world.Size);
            citiesRecieved = new bool[DssRef.world.cities.Count];
            factionsRecieved = new bool[DssRef.world.factions.Count];
        }

        public void Net_HostMapUpdate_async()
        {            
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
                else if (findMissingTile(out IntVector2 subtilePos, true))
                {
                    var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssWorldSubTiles, Network.PacketReliability.Reliable, out var packet);
                    {
                        DssRef.world.writeNet_SubTile(w, subtilePos);
                    }
                    packet.EndWrite_Asynch();
                }
            }

            bool findMissingTile(out IntVector2 tilePos, bool subTile)
            {
                ForXYLoop loop = new ForXYLoop(playerCulling.enterArea);
                while (loop.Next())
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

                        int faction = tile.City().faction.parentArrayIndex;
                        if (!factionsRecieved[faction])
                        {
                            FactionsInView.Add(faction);
                        }
                    }
                }

                tilePos = IntVector2.NegativeOne;
                return false;
            }

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
