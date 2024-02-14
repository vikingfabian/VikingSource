using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Creation.Zombies
{
    abstract class AbsBoostedZombie : AbsZombie
    {
        
        //public static void AddLeader(AbsZombie obj, bool add)
        //{
        //    if (add)
        //        leaders.Add(obj);
        //    else
        //        leaders.Remove(obj);
        //}

        protected bool boosted = false;

        public AbsBoostedZombie()
            : base(0)
        { }
        public AbsBoostedZombie(System.IO.BinaryReader packetReader)
            : base(packetReader)
        {}

        //public override void ReceiveDamageCollision(GameObjects.Weapons.DamageData damage, bool local)
        //{
        //    if (boosted)
        //        damage.Damage *= 0.6f;
        //    base.ReceiveDamageCollision(damage, local);
        //}
        protected override void handleDamage(GameObjects.WeaponAttack.DamageData damage, bool local)
        {
            if (boosted)
                damage.Damage *= 0.6f;
            base.handleDamage(damage, local);
        }

        public override void Time_LasyUpdate(ref float time)
        {
            base.Time_LasyUpdate(ref time);

            if (lib.PercentChance(5))
            {
                boosted = false;
                foreach (AbsZombie l in leaders)
                {
                    const float LeaderDist = 10;
                    if (PositionDiff(l).Length() <= LeaderDist)
                    {
                        boosted = true;
                        break;
                    }
                }
                
            }
            
        }
    }
}
