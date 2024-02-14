using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Characters
{
    abstract class AbsGameObject : AbsVoxelObj
    {
        protected int areaLevel = 0;
        float lastDamageLevel = 0;
        protected Time immortalityTime = Time.Zero;
        public float ImmortalityTime
        {
            set { immortalityTime.MilliSeconds = value; }
        }
        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            if (immortalityTime.TimeOut || damage.Damage > lastDamageLevel)
            {
                lastDamageLevel = damage.Damage;
                immortalityTime.MilliSeconds = 500;
                return true;
            }
            return false;
        }

        public AbsGameObject()
            : base()
        {

        }
        
        public AbsGameObject(System.IO.BinaryReader r)
            :base(r)
        {

        }

        public override void Time_Update(UpdateArgs args)
        {
            immortalityTime.CountDown();
            base.Time_Update(args);
        }
    }
}
