using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest
{
    class RecordedDamageEvent
    {
        public static RecordedDamageEvent Empty()
        {
            return new RecordedDamageEvent(null, Damage.Zero, DamageAnimationType.None, IntVector2.Zero);
        }

        public Unit reciever;
        public Damage damage;
        public DamageAnimationType animationType;
        public IntVector2 damageSource;

        public int startHealth, endhealth;

        public RecordedDamageEvent(Unit reciever, Damage damage, DamageAnimationType animationType, IntVector2 damageSource)
        {
            this.reciever = reciever;
            this.damageSource = damageSource;
            if (reciever != null)
            {
                this.startHealth = reciever.health.Value;
            }
            this.damage = damage;
            this.animationType = animationType;
        }

        public RecordedDamageEvent(System.IO.BinaryReader r)
        {
            read(r);
        }

        public void apply()
        {
            if (reciever != null)
            {
                reciever.TakeDamage(this);
            }
        }

        public void sendDamageEventToAttacker(Unit attacker)
        {
            if (DamageRecieved > 0)
            {
                if (reciever.Dead)
                {
                    attacker.PlayerHQ.onDestroyedUnit(attacker, reciever);
                }
                else
                {
                    attacker.PlayerHQ.onDamagedUnit(attacker, reciever);
                }
            }
        }

        public void NetSend()
        {
            if (!IsEmpty())
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqSendDamage, Network.PacketReliability.Reliable);
                write(w);
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            reciever.netWriteUnitId(w);
            w.Write((byte)animationType);
            toggRef.board.WritePosition(w, damageSource);
            damage.write(w);
            NetManager.WriteHealth(w, startHealth);
            NetManager.WriteHealth(w, endhealth);
        }

        public void read(System.IO.BinaryReader r)
        {
            reciever = Unit.NetReadUnitId(r);
            animationType = (DamageAnimationType)r.ReadByte();
            damageSource = toggRef.board.ReadPosition(r);
            damage.read(r);
            startHealth = NetManager.ReadHealth(r);
            endhealth = NetManager.ReadHealth(r);
        }

        public int DamageRecieved { get { return startHealth - endhealth; } }

        public bool IsEmpty()
        {
            return damage.IsEmpty;
        }
    }
}
