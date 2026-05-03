using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Network;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Net
{
    class FactionHandover
    {
        AbsNetworkPeer peer; 
        Faction faction;
        HandoverPart part = HandoverPart.Cities;
        
        SpottedArrayCounter<Army> armyCounter;
        SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
        public FactionHandover(AbsNetworkPeer peer, Faction faction) 
        {
            this.peer = peer;
            this.faction = faction;

            var remote = DssRef.state.GetOrCreateRemotePlayer(peer, 0);
            if (remote.faction != faction)
            {
                remote.AssignFaction(faction);
            }

            {
                var w = Ref.netSession.BeginWritingPacket_Asynch(PacketType.DssFactionStatus, PacketReliability.Reliable, out var packet);
                w.Write((ushort)faction.myIndex);
                faction.writeNet_Status(w);

                packet.EndWrite_Asynch();
            }
            {
                var w = Ref.netSession.BeginWritingPacket_Asynch(PacketType.DssAssignFaction, PacketReliability.Reliable, out var packet);
                ((PlayState)DssRef.state).NetWritePlayer(w, remote);
                w.Write((ushort)faction.myIndex);

                packet.EndWrite_Asynch();
            }

        }

        public bool Next()
        {
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
                        armyCounter = faction.armies.counter();

                        //SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                        //while (citiesC.Next(ref cities, DssRef.world.cities, out City city))
                            //faction.cities

                        packet.EndWrite_Asynch();
                    }
                    break;
                case HandoverPart.CityStatus:
                    {
                        if (citiesC.Next(ref faction.cities, DssRef.world.cities, out City city))
                        {
                            city.net_handover();
                        }
                        else
                        {
                            part++;
                        }
                    }
                    break;
                case HandoverPart.Armies:
                    int maxArmies = 2;
                    while (--maxArmies > 0 && armyCounter.Next())
                    {
                        armyCounter.sel.IsNetHosted = false;
                        Army.NetFullArmyStatus(armyCounter.sel, PacketReliability.Reliable);
                    }

                    if (!armyCounter.HasMore())
                    {
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
            Armies,
            HandOverComplete,
            DONE
        }
    }
}
