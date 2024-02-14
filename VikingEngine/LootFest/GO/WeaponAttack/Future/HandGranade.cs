using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack.ItemThrow;

namespace VikingEngine.LootFest.GO.WeaponAttack.Future
{
    class HandGranade : AbsBomb
    {
        Time flashTimer;
        
        public HandGranade(GO.Characters.AbsCharacter h)
            : base(GoArgs.Empty,h, 0.008f)
        {
            NetworkShareObject();
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.HandGranade; }
        }

        static readonly IntervalF FireExplosionRadius = new IntervalF(6, 7);

        static readonly GO.WeaponAttack.DamageData FireExplosionDamage = new GO.WeaponAttack.DamageData(LfLib.HeroNormalAttack, GO.WeaponAttack.WeaponUserType.NON, NetworkId.Empty, 
            Magic.MagicElement.NoMagic, SpecialDamage.NONE, WeaponPush.Normal, Rotation1D.D0);
        

        protected override void Explode(UpdateArgs args)
        {
            if (localMember)
                new GO.WeaponAttack.Explosion(new GoArgs(image.position) ,args.localMembersCounter, FireExplosionDamage, 
                    FireExplosionRadius.GetRandom(), Data.MaterialType.gray_85, true, true, callBackObj);
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);

            if (flashTimer.CountDown())
            {
                if (image.Frame == 0)
                {
                    image.Frame = 1;
                    flashTimer.MilliSeconds = 60;
                }
                else
                {
                    image.Frame = 0;
                    flashTimer.MilliSeconds = 160;
                }
            }
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            //const bool EndSearchLoop = false;
            //if (type == Gadgets.GoodsType.Poision_bomb)
            //{
            //    character.TakeDamage(PoisionDamage, true);
            //}
            //else
            //    throw new NotImplementedException("bomb coll damage");
            return false;
        }
    }
}