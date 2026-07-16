using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Network;
using VikingEngine.SteamWrapping;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Net
{
    class FactionHandover
    {
        public AbsNetworkPeer peer; 
        Faction faction;
        HandoverPart part = HandoverPart.Cities;
        
        SpottedArrayCounter<Army> armyCounter;
        SpottedPointerArrayCounter citiesC;
        SteamLargePacketWriter largeWriter = null;

        bool fullHandover;
        public FactionHandover(AbsNetworkPeer peer, Faction faction, bool firstEnterSetup, bool fullHandover) 
        {
            this.fullHandover = fullHandover;
            this.peer = peer;
            this.faction = faction;

            armyCounter = faction.armies.counter();
            citiesC = new SpottedPointerArrayCounter();

            if (fullHandover)
            {
                var remote = DssRef.state.GetOrCreateRemotePlayer(peer, 0);
                if (remote.pfaction.GetFaction() != faction)
                {
                    remote.AssignFaction(faction);
                }

                {
                    var w = Ref.netSession.BeginWritingPacket_Asynch(PacketType.DssAssignFaction, PacketReliability.Reliable, out var packet);
                    ((PlayState)DssRef.state).NetWritePlayer(w, remote);
                    w.Write((ushort)faction.myIndex);
                    w.Write(DssRef.time.TotalIngameTime().Ticks);
                    w.Write(remote.timePlayed.Ticks);
                    DssRef.world.metaData.worldId.write(w);
                    w.Write(firstEnterSetup);

                    packet.EndWrite_Asynch();
                }
            }
            else
            {
                part = HandoverPart.CityStatus;
            }

            {
                var w = Ref.netSession.BeginWritingPacket_Asynch(PacketType.DssFactionStatus, PacketReliability.Reliable, out var packet);
                w.Write((ushort)faction.myIndex);
                faction.writeNet_Status(w);

                packet.EndWrite_Asynch();
            }
        }

        public bool Next()
        {
            if (peer.highLoad())
            {
                return true;
            }

            if (largeWriter != null)
            {
                if (!largeWriter.Complete)
                {
                    return true;
                }
                else if (largeWriter.TimeOut)
                { //Cancel the handover
                    Ref.update.AddSyncAction(new SyncAction(() =>
                    {
                        Ref.NetUpdateReciever().NetEvent_ErrorMessage("Faction handover timeout", peer, false);
                    }));
                    part = HandoverPart.DONE;
                    return false;
                }
            }

            switch (part)
            {
                case HandoverPart.Cities:
                    {
                        var w = Ref.netSession.BeginWritingPacket_Asynch(PacketType.DssAssignFactionCities, PacketReliability.Reliable, out var packet);
                        w.Write((ushort)faction.myIndex);
                        IntVector2 centerCamera = IntVector2.Zero;
                        if (faction.mainCity != null)
                        {
                            centerCamera = faction.mainCity.tilePos;
                        }
                        centerCamera.writeUshort(w);
                        faction.cities.write_ushort_compressed(w);
                        part++;
                        

                        packet.EndWrite_Asynch();
                    }
                    break;
                case HandoverPart.CityStatus:
                    {
                        if (citiesC.Next(ref faction.cities, DssRef.world.cities, out City city))
                        {
                            largeWriter = City.NetWriteHandoverPacket(peer, city);
                            if (fullHandover)
                            {
                                city.IsNetHosted = false;
                            }
                        }
                        else
                        {
                            citiesC.Reset();
                            part++;
                        }
                    }
                    break;
               
                case HandoverPart.Armies:
                    int maxArmies = 2;
                    while (--maxArmies > 0 && armyCounter.Next())
                    {
                        if (fullHandover)
                        {
                            armyCounter.sel.IsNetHosted = false;
                        }
                        
                        Army.NetFullArmyStatus(armyCounter.sel, PacketReliability.Reliable, true);
                    }

                    if (!armyCounter.HasMore())
                    {
                        if (fullHandover)
                        {
                            part++;
                        }
                        else
                        {
                            part = HandoverPart.DONE;
                            Ref.netSession.BeginWritingPacket_Asynch(PacketType.DssClientHandoverComplete, PacketReliability.Reliable, out var packet);
                            packet.EndWrite_Asynch();
                        }
                    }
                    break;

                case HandoverPart.Diplomacy:
                    {
                        DataStream.MemoryStreamHandler diplomacyData = new DataStream.MemoryStreamHandler();
                        var w = diplomacyData.GetWriter();
                        DssRef.world.diplomacy.writeRelations(w);

                        largeWriter = new SteamLargePacketWriter(diplomacyData, SendPacketTo.OneSpecific, peer.fullId, PacketType.DssWorldDiplomacy);
                        largeWriter.begin();
                        part++;
                    }
                    break;

                case HandoverPart.HandOverComplete:
                    {
                        citiesC.Reset();
                        while (citiesC.Next(ref faction.cities, DssRef.world.cities, out City city))
                        {
                            city.IsNetHosted = false;
                        }
                        Debug.Log("Write handover complete");
                        var w = Ref.netSession.BeginWritingPacket_Asynch(PacketType.DssAssignFactionComplete, PacketReliability.Reliable, out var packet);
                        w.Write((ushort)faction.myIndex);
                        packet.EndWrite_Asynch();
                        part++;
                    }
                    break;
            }

            return part < HandoverPart.DONE;
        }

        enum HandoverPart
        { 
            Cities,
            
            CityStatus,
            //CityGuard,
            Armies,
            Diplomacy,
            HandOverComplete,
            DONE
        }
    }
}
