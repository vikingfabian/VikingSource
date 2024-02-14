using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters.Condition
{
    abstract class AbsHeroCondition
    {
        protected Hero hero;

        public AbsHeroCondition(Hero h)
        { this.hero = h; }
        /// <returns>Is Alive</returns>
        virtual public bool Update(GameObjects.UpdateArgs args) { return true; }
        virtual public WeaponAttack.DamageData SpecialsToAttack(WeaponAttack.DamageData damage) { return damage; }

        //protected void DeleteMe()
        //{
        //    hero.
        //}
    }



    class FirstBlood : AbsHeroCondition
    {
        static readonly float RestTime = lib.MinutesToMS(1);
        const float DamageBoost = 2;

        bool boost = false;
        bool handWeapon;
        public FirstBlood(Hero h, bool handWeapon)
            : base(h)
        {
            this.handWeapon = handWeapon;
        }

        public override bool  Update(UpdateArgs args)
        {
 	 
            if (!boost)
            {
                float timePass = handWeapon ? hero.TimeSinceMelee : hero.TimeSinceProjectile;
                if (timePass >= RestTime)
                {
                    boost = true;
                    //visual effect
                    Vector3 pos = hero.Position;
                    pos.Y += 2;
                    Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.GoldenSparkle, pos, 1, 8);
                }
                
            }
            return true; 
        }
        override public WeaponAttack.DamageData SpecialsToAttack(WeaponAttack.DamageData damage) 
        {
            damage.Damage *= DamageBoost;
            return damage; 
        }
    }
    class EvilAura: AbsHeroCondition
    {
        const float AuraRadius = 8;
        Timer.Basic updateRate = new Timer.Basic(2000, true);
        static readonly WeaponAttack.DamageData FriendlyDamage = new WeaponAttack.DamageData(LootfestLib.EvilAuraFriendlyDamage, WeaponAttack.WeaponUserType.NON, ByteVector2.Zero, Gadgets.GoodsType.NONE, Magic.MagicElement.Evil);
        static readonly WeaponAttack.DamageData FoeDamage = new WeaponAttack.DamageData(LootfestLib.EvilAuraDamage, WeaponAttack.WeaponUserType.NON, ByteVector2.Zero, Gadgets.GoodsType.NONE, Magic.MagicElement.Evil);

        public EvilAura(Hero h)
            : base(h)
        {

        }
        public override bool Update(UpdateArgs args)
        {
            if (updateRate.Update(args.time))
            {
                args.allMembersCounter.Reset();
                while (args.allMembersCounter.Next())
                {
                    if (args.allMembersCounter.GetMember.Type == ObjectType.Character && args.allMembersCounter.GetMember != hero && hero.distanceToObject(args.allMembersCounter.GetMember) <= AuraRadius)
                    {
                        bool foe = WeaponAttack.WeaponLib.IsFoeTarget(hero, args.allMembersCounter.GetMember, false);
                        WeaponAttack.DamageData damage = foe ? FoeDamage : FriendlyDamage;
                        if (args.allMembersCounter.GetMember.TakeDamage(damage, true) && !foe)
                        {
                            hero.TakeDamage(damage, true);
                        }
                    }
                }

                //foreach (GameObjects.AbsUpdateObj obj in active)
                //{
                //    if (obj.Type == ObjectType.Character && obj != hero && hero.distanceToObject(obj) <= AuraRadius)
                //    {
                //        bool foe = Weapons.WeaponLib.IsFoeTarget(hero, obj);
                //        Weapons.DamageData damage = foe ? FoeDamage : FriendlyDamage;
                //        if (obj.TakeDamage(damage, true) && !foe)
                //        {
                //            hero.TakeDamage(damage, true);
                //        }
                //    }
                //}
            }
            return true; 
        }
    }
}
