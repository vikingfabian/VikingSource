using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.GO.Characters.AI;

namespace VikingEngine.LootFest.GO.Characters
{
    class OrcArcher : AbsOrc
    {
        static readonly IntervalF ArcherScaleRange = OrcScaleRange * 0.9f;

        public OrcArcher(GoArgs args)
            : base(args, VoxelModelName.orc_archer, ArcherScaleRange)
        {
            aggresivity = HumanoidEnemyAgressivity.Careful_2;
            hasRangedWeapon = true;
            hasHandWeapon = true;
            preRangeAttackTime = 1000;
            projectileRange = new IntervalF(8, 32);
            tryKeepRangedDistance = true;
            projectileRate.Seconds = 2;

            handWeapon = new Gadgets.HumanoidEnemyHandWeapon(
                VoxelModelName.orc_sword1,
                new HandWeaponAttackSettings(
                    GameObjectType.OrcArcherSwordAttack, 0.8f, 0.12f,
                    new Vector3(
                        HandWeaponAttackSettings.SwordBoundScaleW,
                        HandWeaponAttackSettings.SwordBoundScaleW,
                        HandWeaponAttackSettings.SwordBoundScaleL),
                    new Vector3(4.5f, 8.4f, 4f),
                    500,
                    HandWeaponAttackSettings.SwordSwingStartAngle,
                    HandWeaponAttackSettings.SwordSwingEndAngle,
                    HandWeaponAttackSettings.SwordSwingPercTime,
                    new WeaponAttack.DamageData(1, WeaponAttack.WeaponUserType.Enemy, NetworkId.Empty)
                    ),
                new Vector3(0, -0.7f, -1f),
                new Effects.BouncingBlockColors(Data.MaterialType.dark_red, Data.MaterialType.darker_red)
            );

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }
       

        protected override void createPreRangedAttackEffect()
        {
            preRangedAttackWeaponEffect = new Effects.HumanoidEnemyPreBowEffect(VoxelModelName.orcbow,
                new Vector3(0.1f, 0.3f, 0.3f) * modelScale, this);
        }
        protected override void createRangedAttack()
        {
            Vector3 targetPos = target.Position;
            targetPos.Y += 1.5f;
            new WeaponAttack.OrcArrow(new GoArgs(preRangedAttackWeaponEffect.Position), targetPos);
            base.createRangedAttack();
        }

        public override GameObjectType Type
        { get { return GameObjectType.OrcArcher; } }
        override public CardType CardCaptureType
        {
            get { return CardType.OrcArcher; }
        }

        public override float GivesBravery
        { get { return 1.5f; } }

        protected const float WalkingSpeed = 0.007f;
        protected const float CasualWalkSpeed = WalkingSpeed * 0.5f;

        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        override protected float walkingSpeed
        {
            get { return WalkingSpeed; }
        }

        new static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.darker_green_cyan,
            Data.MaterialType.light_pea_green,
            Data.MaterialType.darker_yellow_orange
            );

        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }
    }
}
