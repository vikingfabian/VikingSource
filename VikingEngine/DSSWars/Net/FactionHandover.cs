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
                var w = Ref.netSession.BeginWritingPacket(PacketType.DssFactionStatus, PacketReliability.Reliable);
                w.Write((ushort)faction.myIndex);
                faction.writeNet_Status(w);
            }
            {
                var w = Ref.netSession.BeginWritingPacket(PacketType.DssAssignFaction, PacketReliability.Reliable);
                ((PlayState)DssRef.state).NetWritePlayer(w, remote);
                w.Write((ushort)faction.myIndex);
            }

        }

        public bool Next()
        {
            switch (part)
            {
                case HandoverPart.Cities:
                    var w = Ref.netSession.BeginWritingPacket(PacketType.DssAssignFactionCities, PacketReliability.Reliable);
                    w.Write((ushort)faction.myIndex);
                    faction.cities.write_ushort_compressed(w);
                    part++;
                    armyCounter = faction.armies.counter();
                    break;
                case HandoverPart.Armies:
                    int maxArmies = 2;
                    while (--maxArmies > 0 && armyCounter.Next())
                    {
                        armyCounter.sel.IsNetHosted = false;
                        Army.NetFullArmyStatus(armyCounter.sel);
                    }

                    if (!armyCounter.HasMore())
                    {
                        part++;
                    }
                    break;

            }

            return part < HandoverPart.DONE;
        }

        enum HandoverPart
        { 
            Cities,
            Armies,
            DONE
        }
    }
}
