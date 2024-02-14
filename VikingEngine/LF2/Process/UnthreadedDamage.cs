using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Process
{
    class UnthreadedDamage : OneTimeTrigger
    {
        GameObjects.WeaponAttack.DamageData damage; GameObjects.AbsUpdateObj target;
        public UnthreadedDamage(GameObjects.WeaponAttack.DamageData damage, GameObjects.AbsUpdateObj target)
            :base(false)
        {
            this.damage = damage; this.target = target;
            AddToUpdateList(true);
        }
        public override void Time_Update(float time)
        {
            target.TakeDamage(damage, true);
        }
    }
}
