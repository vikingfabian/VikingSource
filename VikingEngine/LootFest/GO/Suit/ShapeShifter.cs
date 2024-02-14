using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO
{
    class ShapeShifter : AbsSuit
    {
        const int ShiftAmmoCost = 3;
        const float SpearScale = 0.23f;
        const float AttackAngle = -0.1f;
        const float SpearAttackTime = 340;
        const float ReloadTime = 180;
        //const int ThrowJavelinAttackIndex = 2;

        CirkleCounterUp attackIndex = new CirkleCounterUp(1, 1);
        WeaponAttack.HandWeaponAttackSettings spearStab, spearSwing;



        public ShapeShifter(Players.AbsPlayer user)
            : base(user, VoxelModelName.shapeshifter_spear)
        {
            Vector3 spearBound =  new Vector3(1.8f, HandWeaponAttackSettings.SwordBoundScaleH, 8.5f);
            Vector3 spearOffset = new Vector3(3f, 1.4f, 4f);
            WeaponAttack.DamageData damage = new WeaponAttack.DamageData(LfLib.HeroNormalAttack, WeaponAttack.WeaponUserType.Player,
                     user.hero.ObjOwnerAndId);

            spearStab = new WeaponAttack.HandWeaponAttackSettings(
                 GameObjectType.DualAxeAttack, HandWeaponAttackSettings.SwordStartScalePerc,
                SpearScale,
                spearBound,//bounds
                spearOffset, //offset
                SpearAttackTime, //att time
                AttackAngle,
                AttackAngle,
                0, 
                damage
                 );

            spearSwing = new WeaponAttack.HandWeaponAttackSettings(
                GameObjectType.DualAxeAttack, HandWeaponAttackSettings.SwordStartScalePerc,
                SpearScale,
                 spearBound,//bounds
                spearOffset, //offset
                SpearAttackTime, //att time
                -1.5f, //start angle
                0.8f, //end angle
                0.7f, 
                damage
                 );
        }

        //public void onJavelinDeleted()
        //{
        //    primaryAttackReloadTime = 300;
        //}

        protected override Time PrimaryAttack(out float attackAnimFrameTime, bool localUse)
        {
            attackIndex.Next();

            if (attackIndex.Value == 0)
            {
                new WeaponAttack.HandWeaponAttack2(spearSwing, originalWeaponMesh1, player.hero, localUse);
            }
            else if (attackIndex.Value == 1)
            {
                new WeaponAttack.HandWeaponAttack2(spearStab, originalWeaponMesh1, player.hero, localUse);
            }
            //else
            //{
            //    if (localUse)
            //    {
            //        new WeaponAttack.ItemThrow.ShapeShifterJavelin(player.hero, this);
            //    }
            //}

            attackAnimFrameTime = primaryAttackTime + 120;
            return new  Time( primaryAttackTime + primaryReloadTime);
        }

        override protected float primaryAttackTime
        {
            get
            {
                return SpearAttackTime;}}

            //attackIndex.Value == ThrowJavelinAttackIndex?  
            //    WeaponAttack.ItemThrow.ShapeShifterJavelin.LifeTime + 200 : 
            //    SpearAttackTime; } }

        override protected float primaryReloadTime { get { return ReloadTime; } }


        //protected override void onActionComplete(bool main)
        //{
        //    if (main && attackIndex.Value == 2)
        //    {
        //        player.hero.weaponReadyEffeckt();
        //    }
        //}

        protected override void UseSpecial()
        {
            if (player.hero.isMounted)
            {
                new WeaponAttack.ItemThrow.ShapeShifterJavelin(player.hero, this);
            }
            else
            {
                Players.Player p = (Players.Player)player;
               p.setNewHero(new PlayerCharacter.WolfHero(p), true);
                attackAnimationTime = 0;
            }
        }

        public override Players.HatType[] availableHatTypes()
        {
            return new Players.HatType[] { Players.HatType.WolfHead };
        }
        override public Players.BeardType[] availableBeardTypes()
        {
            return new Players.BeardType[] { Players.BeardType.BeardLarge, Players.BeardType.Barbarian1, Players.BeardType.Barbarian2, 
                Players.BeardType.Barbarian3, Players.BeardType.Barbarian4, Players.BeardType.Barbarian5, };
        }

        public override SuitType Type
        {
            get { return SuitType.ShapeShifter; }
        }
        public override SpriteName PrimaryIcon
        {
            get { return SpriteName.LfShapeshifterIcon1; }
        }
        public override SpriteName SpecialAttackIcon
        {
            get { return SpriteName.LfShapeshifterIcon2; }
        }
        public override int MaxSpecialAmmo
        {
            get
            {
                return ShiftAmmoCost;
            }
        }
        protected override int SpecialAttackCost
        {
            get
            {
                return ShiftAmmoCost;
            }
        }

        override public bool SecondaryAttackWorksFromMounts
        {
            get { return true; }
        }

        override protected float primaryAttackMovementPerc { get { return 0f; } }//attackIndex.Value == ThrowJavelinAttackIndex? 1f : 0f; } }
    }
}
