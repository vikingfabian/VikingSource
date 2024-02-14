using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.GO.PlayerCharacter;

namespace VikingEngine.LootFest.GO.Characters.Condition
{
    abstract class AbsHeroCondition
    {
        protected Hero hero;

        public AbsHeroCondition(Hero h)
        { this.hero = h; }
        /// <returns>Is Alive</returns>
        virtual public bool Update(GO.UpdateArgs args) { return true; }
        virtual public WeaponAttack.DamageData SpecialsToAttack(WeaponAttack.DamageData damage) { return damage; }

        //protected void DeleteMe()
        //{
        //    hero.
        //}
    }



    class FirstBlood : AbsHeroCondition
    {
        static readonly float RestTime = TimeExt.MinutesToMS(1);
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
        static readonly WeaponAttack.DamageData FriendlyDamage = new WeaponAttack.DamageData(LfLib.EvilAuraFriendlyDamage, WeaponAttack.WeaponUserType.NON, NetworkId.Empty, Magic.MagicElement.Evil);
        static readonly WeaponAttack.DamageData FoeDamage = new WeaponAttack.DamageData(LfLib.EvilAuraDamage, WeaponAttack.WeaponUserType.NON, NetworkId.Empty, Magic.MagicElement.Evil);

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
                    if (args.allMembersCounter.GetSelection is GO.Characters.AbsCharacter && args.allMembersCounter.GetSelection != hero && hero.distanceToObject(args.allMembersCounter.GetSelection) <= AuraRadius)
                    {
                        bool foe = WeaponAttack.WeaponLib.IsFoeTarget(hero, args.allMembersCounter.GetSelection, false);
                        WeaponAttack.DamageData damage = foe ? FoeDamage : FriendlyDamage;
                        if (args.allMembersCounter.GetSelection.TakeDamage(damage, true) && !foe)
                        {
                            hero.TakeDamage(damage, true);
                        }
                    }
                }

                //foreach (GO.AbsUpdateObj obj in active)
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
