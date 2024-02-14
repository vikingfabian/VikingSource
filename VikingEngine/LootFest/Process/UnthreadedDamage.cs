using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Process
{
    class UnthreadedDamage : OneTimeTrigger
    {
        GO.WeaponAttack.DamageData damage; GO.AbsUpdateObj target;
        public UnthreadedDamage(GO.WeaponAttack.DamageData damage, GO.AbsUpdateObj target)
            :base(false)
        {
            this.damage = damage; this.target = target;
            AddToOrRemoveFromUpdateList(true);
        }
        public override void Time_Update(float time)
        {
            target.TakeDamage(damage, true);
        }
    }
}
