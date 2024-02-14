using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters.HumanionEnemy
{
    class ElfArcher : AbsElf
    {
        public ElfArcher(GoArgs args)
            : base(args, VoxelModelName.elf_archer, ElfScaleRange)
        {
            aggresivity = HumanoidEnemyAgressivity.Careful_2;
            hasRangedWeapon = true;
            hasHandWeapon = true;
            preRangeAttackTime = 800;
            projectileRange = new IntervalF(8, 32);
            tryKeepRangedDistance = true;
            projectileRate.Seconds = 1;

            elfSword();

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        protected override void createPreRangedAttackEffect()
        {
            preRangedAttackWeaponEffect = new Effects.HumanoidEnemyPreBowEffect(VoxelModelName.elfbow,
                new Vector3(0.1f, 0.3f, 0.3f) * modelScale, this);
        }
        protected override void createRangedAttack()
        {
            Vector3 targetPos = target.Position;
            targetPos.Y += 1.5f;
            new WeaponAttack.ElfArrow(new GoArgs(preRangedAttackWeaponEffect.Position), targetPos);
            base.createRangedAttack();
        }

        public override GameObjectType Type
        { get { return GameObjectType.ElfArcher; } }
        override public CardType CardCaptureType
        {
            get { return CardType.ElfArcher; }
        }

        public override float GivesBravery
        { get { return 1.5f; } }

        new protected const float WalkingSpeed = 0.009f;
        new protected const float CasualWalkSpeed = WalkingSpeed * 0.5f;

        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        override protected float walkingSpeed
        {
            get { return WalkingSpeed; }
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.darker_green_cyan,
            Data.MaterialType.light_pea_green,
            Data.MaterialType.darker_yellow_orange
            );

        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }
    }
}
