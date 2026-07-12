using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.Players
{

    struct PlayerMapHistory
    {
        public bool local;
        public int localScreenIndex;
        public ulong id;
        public int faction;
        public TimeSpan timePlayed;
        public Color? recolor;

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(local);
            w.Write((byte)localScreenIndex);
            w.Write(id);
            w.Write((ushort)faction);
            w.Write((int)timePlayed.TotalSeconds);

            w.Write(recolor.HasValue);
            if (recolor.HasValue) 
            { 
                StreamLib.WriteColorStream_3B(w, recolor.Value);
            }
        }
        public void read(System.IO.BinaryReader r, int subVersion)
        {
            local = r.ReadBoolean();
            localScreenIndex = r.ReadByte();
            id = r.ReadUInt64();
            faction = r.ReadUInt16();
            timePlayed = TimeSpan.FromSeconds(r.ReadInt32());

            if (subVersion >= 118)
            {
                if (r.ReadBoolean())
                {
                    recolor = StreamLib.ReadColorStream_3B(r);
                }
                else
                {
                    recolor = null;
                }
            }
        }

        public override int GetHashCode()
        {
            return GetGamerHash(local, id, localScreenIndex);
        }

        public static int GetGamerHash(bool local, ulong peerid, int localScreenIndex)
        {
            if (local)
            {
                return localScreenIndex;
            }
            return HashCode.Combine(peerid, localScreenIndex);
        }
    }

    partial class RemotePlayer
    {
        static List<Army> netCollArmies = new List<Army>(16);

        public const int OverviewSendChunkSize = 8;
        static HashSet<int> CitiesInView = new HashSet<int>();
        static HashSet<int> FactionsInView = new HashSet<int>();

        public Grid2D<RemoteTile> remoteTileGrid;
        public bool[] citiesRecieved;
        public bool[] factionsRecieved;

        ForXYLoop fullMapSendPosition;

        public void InitData()
        {
            remoteTileGrid = new Grid2D<RemoteTile>(DssRef.world.Size);
            fullMapSendPosition = new ForXYLoop(DssRef.world.Size);
            citiesRecieved = new bool[DssRef.world.cities.Count];
            factionsRecieved = new bool[DssRef.world.factions.Count];
        }

        public PlayerMapHistory GetMapHistory()
        {
            return new PlayerMapHistory()
            {
                id = networkPeer.peer.fullId,
                faction = assignedFaction,
                timePlayed = timePlayed,
                recolor = profile.flag == null ? null : profile.flag.col0_Main,
            };
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
                else if (playerCulling.detailLayer && findMissingTile(out IntVector2 tilePosForSubtiles, true))
                {
                    var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssWorldSubTiles, Network.PacketReliability.Reliable, out var packet);
                    {
                        DssRef.world.writeNet_SubTile(w, tilePosForSubtiles);
                    }
                    packet.EndWrite_Asynch();
                }//TODO make sure owned cities are map ready
                else if (Net_SendCityTiles_async())
                { 
                    //no code
                }
                else
                {
                    sentMap = false;
                }

                return sentMap;

                //todo hitta alla städer och armeer i sight
            }

            return false;

            bool findMissingTile(out IntVector2 tilePos, bool isSubTile)
            {
                Rectangle2 area;

                if (isSubTile)
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
                        if (!remoteTileGrid.Get(loop.Position).HasTile(isSubTile))
                        {
                            tilePos = loop.Position;
                            return true;
                        }
                        if (!isSubTile)
                        {
                            var tile = DssRef.world.tileGrid.Get(loop.Position);
                            if (!citiesRecieved[tile.CityIndex])
                            {
                                CitiesInView.Add(tile.CityIndex);
                            }

                            int faction = tile.City().factionIndex;
                            if (faction >= 0 && DssRef.world.factions.Array[faction].player.IsLocal)
                            {
                                if (!factionsRecieved[faction])
                                {
                                    FactionsInView.Add(faction);
                                }
                            }
                        }
                    }
                }

                tilePos = IntVector2.NegativeOne;
                return false;
            }
        }

        public bool Net_SendCityTiles_async()
        {
            if (pfaction.GetFaction() != null && pfaction.GetFaction().cities.Count > 0)
            {
                int chunkSize = 4;

                int cityIx = pfaction.GetFaction().cities.GetRandom(Ref.rnd);
                var city = DssRef.world.cities[cityIx];
                ForXYLoop loop = new ForXYLoop(city.cityTileArea);
                while (loop.Next() && chunkSize > 0)
                {
                    var hasRecieved = remoteTileGrid.Get(loop.Position);
                    if (!hasRecieved.overview)
                    {
                        var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssWorldTiles, Network.PacketReliability.Reliable, out var packet);
                        {
                            DssRef.world.writeNet_Tile(w, loop.Position);
                        }
                        packet.EndWrite_Asynch();
                        chunkSize--;
                    }

                    if (!hasRecieved.detail)
                    {
                        var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssWorldSubTiles, Network.PacketReliability.Reliable, out var packet);
                        {
#if DEBUG
                            if (w.BaseStream.Length > 6)
                            {
                                throw new Exception();
                            }
#endif
                            DssRef.world.writeNet_SubTile(w, loop.Position);
                        }
                        packet.EndWrite_Asynch();
                        chunkSize--;
                    }
                }

                return chunkSize <= 0;
            }

            return false;
        }

        public bool Net_FullMapSend_async()
        {
            if (!fullMapSendPosition.Done)
            {
                int sendChunkSize = 8;

                while (fullMapSendPosition.Next())
                {
                    if (!remoteTileGrid.Get(fullMapSendPosition.Position).HasTile(false))
                    {
                        var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssWorldTiles, Network.PacketReliability.Reliable, out var packet);
                        {
                            DssRef.world.writeNet_Tile(w, fullMapSendPosition.Position);
                        }
                        packet.EndWrite_Asynch();

                        sendChunkSize--;
                        if (sendChunkSize <= 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void Net_UpdateArmies(ref int maxPackets)
        {
            

            if (playerCulling.farLayer == false)
            {
                DssRef.world.unitCollAreaGrid.net_collectArmies(playerCulling.screenAreaRaw, netCollArmies);

                int waitSeconds;
                if ( netCollArmies.Count <= 2)
                {
                    waitSeconds = 2;
                }
                else if (netCollArmies.Count <= 10)
                {
                    waitSeconds = 4;
                }
                else 
                {
                    waitSeconds = 10;
                }

                foreach (Army army in netCollArmies)
                {
                    if (army.IsNetHosted && army.lastNetUpdate.secPassed(waitSeconds))
                    {
                        Army.NetFullArmyStatus(army, Network.PacketReliability.Unrelyable);
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

        public bool HasTile(bool isSubTile)
        { 
            return isSubTile? detail : overview;
        }
    }
}
