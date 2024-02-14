using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters.Condition
{
    class Poision : AbsCharCondition
    {
        static readonly WeaponAttack.DamageData PoisionDamage = new WeaponAttack.DamageData(
            LootfestLib.PoisionConditionmDamage, WeaponAttack.WeaponUserType.Player, 
            ByteVector2.Zero, Gadgets.GoodsType.NONE, Magic.MagicElement.Poision, WeaponAttack.SpecialDamage.NONE, true);
        int attacks = 3;

        public Poision(AbsCharacter parent, WeaponAttack.DamageData damage)
            : base(parent, DamageRate)
        {
            if (damage.Special == WeaponAttack.SpecialDamage.TinyBoost)
            {
                attacks += 1;
            }
            else if (damage.Special == WeaponAttack.SpecialDamage.SmallBoost)
            {
                attacks += 2;
            }
        }
        public override void Time_Update(UpdateArgs args)
        {

            lifeTime -= args.time;
            emitter.Area.Min = parent.Position;
            //emitter.Area.Min.Y += 8;

            if (lifeTime <= 0)
            {
                lifeTime = DamageRate;
                parent.TakeDamage(PoisionDamage, true);
                attacks--;
                if (attacks <= 0 || !parent.Alive)
                    DeleteMe();

            }
        }
        protected override ParticleSystemType particles
        {
#if CMODE
            get { return ParticleSystemType.NUM; }
#else

            get { return ParticleSystemType.Poision; }
#endif
        }
        override protected ConditionType conditionType
        { get { return ConditionType.Poisioned; } }
        protected override bool setCharacterCondition
        {
            get { return true; }
        }
    }
}
