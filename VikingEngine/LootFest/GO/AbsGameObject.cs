using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.GO
{
    abstract class AbsGameObject : AbsVoxelObj
    {
        
        protected float lastDamageLevel = float.MinValue;
        
        public float ImmortalityTime
        {
            set { immortalityTime.MilliSeconds = value; }
        }
        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            if (immortalityTime.TimeOut || damage.Damage > lastDamageLevel || damage.Special == WeaponAttack.SpecialDamage.CardThrow)
            {
                lastDamageLevel = damage.Damage;
                immortalityTime.MilliSeconds = 500;
                return true;
            }
            return false;
        }

        public AbsGameObject(GoArgs args)
            :base(args)
        {

        }

        public override void Time_Update(UpdateArgs args)
        {
            if (immortalityTime.CountDown())
            {
                lastDamageLevel = float.MinValue;
            }
            base.Time_Update(args);
        }
    }
}
