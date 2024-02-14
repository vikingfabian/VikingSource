using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest
{
    class HealUnit
    {
        public Unit reciever;
        public HealSettings heal;

        public HealUnit(Unit reciever, int heal, HealType healType, bool netShare, bool applyNow = true)
            :this(reciever, new HealSettings(heal, healType), netShare, applyNow)
        { }

        public HealUnit(Unit reciever, HealSettings heal, bool netShare, bool applyNow = true)
        {
            this.reciever = reciever;
            this.heal = heal;

            if (applyNow)
            {
                apply();
            }

            if (netShare && HasValue)
            {
                NetSend();
            }
        }

        public HealUnit(System.IO.BinaryReader r)
        {
            read(r);
        }

        public void apply()
        {
            if (HasValue)
            {
                if (heal.heal > reciever.health.ValueRemoved)
                {
                    heal.heal = reciever.health.ValueRemoved;
                }

                new ToggEngine.Display3D.UnitMessageValueChange(
                    reciever, ValueType.Health, heal.heal);

                reciever.health.add(heal.heal);
                reciever.OnEvent(ToGG.Data.EventType.Heal, true, null);
                reciever.statusGui?.refresh();
            }
        }

        bool HasValue => heal.heal > 0 && reciever != null;

        void NetSend()
        {           
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqHealEvent, 
                Network.PacketReliability.Reliable);
            write(w);            
        }

        public void write(System.IO.BinaryWriter w)
        {
            reciever.netWriteUnitId(w);
            heal.write(w);
        }

        void read(System.IO.BinaryReader r)
        {
            reciever = Unit.NetReadUnitId(r);
            heal.read(r);
        }
    }

    
}
