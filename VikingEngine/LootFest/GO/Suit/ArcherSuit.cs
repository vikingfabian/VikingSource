using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.GO.WeaponAttack;

namespace VikingEngine.LootFest.GO
{
    class ArcherSuit : AbsSuit
    {
        const float SwordScale = 0.16f;

        const float BowReloadTime = 200;

        WeaponAttack.DamageData arrowDamage;

        public ArcherSuit(Players.AbsPlayer user)
            : base(user, VoxelModelName.sword_base)
        {
            primaryWeaponAttack = new WeaponAttack.HandWeaponAttackSettings(
                GameObjectType.ArcherAttack, HandWeaponAttackSettings.SwordStartScalePerc,
                SwordScale,
                new Vector3(GO.WeaponAttack.HandWeaponAttackSettings.SwordBoundScaleW,
                HandWeaponAttackSettings.SwordBoundScaleH,
                HandWeaponAttackSettings.SwordBoundScaleL),
                HandWeaponAttackSettings.StandardScaleToPosDiff,
                SwordAttackTime,
                HandWeaponAttackSettings.SwordSwingStartAngle,
                HandWeaponAttackSettings.SwordSwingEndAngle,
                HandWeaponAttackSettings.SwordSwingPercTime,
                 new WeaponAttack.DamageData(LfLib.HeroNormalAttack, WeaponAttack.WeaponUserType.Player,
                     user.hero.ObjOwnerAndId)
                 );

            arrowDamage = new WeaponAttack.DamageData(LfLib.HeroNormalAttack, WeaponUserType.Player, NetworkId.Empty);
        }

        public override void Update(Players.InputMap controller)
        {
            base.Update(controller);
            updateVisualBow();
        }

        GO.Bounds.ObjectBound arrowBound = WeaponAttack.GravityArrow.ArrowBound(Rotation1D.D0);
        protected override void UseSpecial()
        {
            new GO.WeaponAttack.GravityArrow(arrowDamage, player.hero.BowFirePos(), player.hero.GetBowTarget(),
                            arrowBound, player.hero.player);

            ViewVisualBow(BowReloadTime, VoxelModelName.ironbow);
        }

        public override SuitType Type
        {
            get { return SuitType.Archer; }
        }
        public override SpriteName PrimaryIcon
        {
            get { return SpriteName.LFSwordsmanIcon1; }
        }
        public override SpriteName SpecialAttackIcon
        {
            get { return SpriteName.LFArcherIcon2; }
        }

        override public float initialJumpForce { get { return 1.5f; } }
        override public float holdJumpMaxTime { get { return 200f; } }
        override public float holdJumpForcePerSec { get { return 0.108f; } }

        override public float MovementAccPerc { get { return StandardMovementAccPerc * 1.2f; } }
        override public float RunLengthDecrease { get { return 0.95f; } }
        override public float RunningSpeed { get { return StandardRunningSpeed * 0.9f; } }

        override public Players.BeardType[] availableBeardTypes()
        {

            return new Players.BeardType[] { 
                Players.BeardType.BeardSmall,
                Players.BeardType.Gentle1,
                Players.BeardType.Gentle2,
                Players.BeardType.Gentle3,
                Players.BeardType.Gentle4,
                Players.BeardType.Robin,
            };
        }

        override public Players.HatType[] availableHatTypes()
        {
            return new Players.HatType[] { 
                Players.HatType.Archer
            };
        }
    }
}
