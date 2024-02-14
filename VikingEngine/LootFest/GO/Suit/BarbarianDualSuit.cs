using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO
{
    
    class BarbarianDualSuit : AbsBarbarian
    {
        /// <summary>
        /// Will attack left, right, both and repeate
        /// </summary>
        CirkleCounterUp attackIndex = new CirkleCounterUp(2, 2);
        Time timeSinceLastAttack = 0;

        const float AttackTime = 300;
        const float ReloadTime = 180, DoubleReloadTime = 450;
        const float AxeScale = 0.24f;

        const float WhirlAttackTime = 2000;

        protected GO.WeaponAttack.HandWeaponAttackSettings 
            leftAttack, rightAttack, 
            leftDoubleAttack, rightDoubleAttack,
            leftWhirlAttack, rightWhirlAttack;
        public const float WhirrWindMoveSpeed = 0.018f;

        public BarbarianDualSuit(Players.AbsPlayer user)
            : base(user, VoxelModelName.barbariandualaxe_r_base)
        {
            const float SwingStartAngle = -1.0f;
            Vector3 ScaleToPosDiff = new Vector3(1.2f, 0.4f, 2.0f);
            Vector3 ScaleToBound = new Vector3(4.4f, HandWeaponAttackSettings.SwordBoundScaleH, 6.6f);
            WeaponAttack.DamageData attDamage = new WeaponAttack.DamageData(LfLib.HeroNormalAttack, WeaponAttack.WeaponUserType.Player,
                     user.hero.ObjOwnerAndId);
            
            loadWeaponModel(1, VoxelModelName.barbariandualaxe_l_base);

            rightAttack = new WeaponAttack.HandWeaponAttackSettings(
               GameObjectType.DualAxeAttack, HandWeaponAttackSettings.SwordStartScalePerc,
               AxeScale,
               ScaleToBound,
               ScaleToPosDiff,
               AttackTime,
               SwingStartAngle,
               HandWeaponAttackSettings.SwordSwingEndAngle,
               0.4f, attDamage);

            leftAttack = rightAttack;
            leftAttack.swingStartAngle *= -1;
            leftAttack.swingEndAngle *= -1;
            leftAttack.weaponPosDiff.X *= -1;

            const float DoubleSwingOffSetX = 1.4f;
            const float DoubleSwingStartAngle = -0.6f;
            const float DoubleSwingEndAngle = 1.2f;

            rightDoubleAttack = rightAttack;
            rightDoubleAttack.weaponPosDiff.X *= DoubleSwingOffSetX;
            rightDoubleAttack.swingStartAngle = DoubleSwingStartAngle;
            rightDoubleAttack.swingEndAngle = DoubleSwingEndAngle;

            leftDoubleAttack = leftAttack;
            leftDoubleAttack.weaponPosDiff.Y += 0.2f;
            leftDoubleAttack.weaponPosDiff.X *= DoubleSwingOffSetX;
            leftDoubleAttack.swingStartAngle = -DoubleSwingStartAngle;
            leftDoubleAttack.swingEndAngle = -DoubleSwingEndAngle;

            const float WhirlAngle = MathHelper.PiOver2;
            rightWhirlAttack = new WeaponAttack.HandWeaponAttackSettings(
               GameObjectType.DualAxeAttack, HandWeaponAttackSettings.SwordStartScalePerc,
               AxeScale,
               ScaleToBound,
               ScaleToPosDiff,
               WhirlAttackTime,
               WhirlAngle,
               WhirlAngle,
               0.4f, attDamage);

            leftWhirlAttack = rightWhirlAttack;
            leftWhirlAttack.weaponPosDiff.X *= -1;
            leftWhirlAttack.swingStartAngle *= -1;
            leftWhirlAttack.swingEndAngle *= -1;

        }

        override protected Time PrimaryAttack(out float attackAnimFrameTime, bool localUse)
        {
            attackIndex.Next();

            if (attackIndex.Value == 0)
            {
                new WeaponAttack.HandWeaponAttack2(rightAttack, originalWeaponMesh1, player.hero, localUse);
            }
            else if (attackIndex.Value == 1)
            {
                new WeaponAttack.HandWeaponAttack2(leftAttack, originalWeaponMesh2, player.hero, localUse);
            }
            else
            {
                new WeaponAttack.HandWeaponAttack2(rightDoubleAttack, originalWeaponMesh1, player.hero, localUse);
                new WeaponAttack.HandWeaponAttack2(leftDoubleAttack, originalWeaponMesh2, player.hero, localUse);
            }
            
            attackAnimFrameTime = primaryAttackTime + 120;
            return new Time(primaryAttackTime + primaryReloadTime);
        }

        protected override void UseSpecial()
        {
            if (player.hero.isMounted)
            {
                new WeaponAttack.ItemThrow.ThrowAxe(player.hero);
            }
            else
            {
                forwardDir = player.hero.Rotation;
                //specialAttackTimer.MilliSeconds = WhirlAttackTime;

                new WeaponAttack.HandWeaponAttack2(rightWhirlAttack, originalWeaponMesh1, player.hero, true);
                new WeaponAttack.HandWeaponAttack2(leftWhirlAttack, originalWeaponMesh1, player.hero, true);

                player.hero.mainAction = new Players.DualAxeWhirrWind(new Time(WhirlAttackTime), player.hero);
            }
        }

        //public override void Update(Players.InputMap controller)
        //{
        //    base.Update(controller);
        //    specialAttackTimer.CountDown();
        //}

        override protected bool canUseSpecial() { return player.hero.canPerformAction(true); }//specialAttackTimer.TimeOut && primaryAttackReloadTime.TimeOut; }

        override protected float primaryAttackTime { get { return AttackTime; } }
        override protected float primaryReloadTime { get { return attackIndex.Value ==2? DoubleReloadTime : ReloadTime; } }

        public override SuitType Type
        {
            get { return SuitType.BarbarianDual; }
        }
        public override SpriteName PrimaryIcon
        {
            get { return SpriteName.LFDualIcon1; }
        }
        public override SpriteName SpecialAttackIcon
        {
            get { return SpriteName.LFDualIcon2; }
        }

        override protected float primaryAttackMovementPerc { get { return attackIndex.Value ==2? 0.8f : 0.2f; } }
    }
}
