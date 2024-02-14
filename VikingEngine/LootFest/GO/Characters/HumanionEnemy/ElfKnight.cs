using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters.HumanionEnemy
{
    class ElfKnight : AbsElf
     {
        public ElfKnight(GoArgs args)
            : this(args, VoxelModelName.elf_knight, ElfScaleRange, 0.22f, VoxelModelName.elf_knight_shield, 2.6f,
                new Effects.BouncingBlockColors(
                    Data.MaterialType.gray_75,
                    Data.MaterialType.gray_85))
        {
        }
        public ElfKnight(GoArgs args, VoxelModelName model, IntervalF scale, float weaponScale, VoxelModelName shieldModel, float shieldScale, 
            Effects.BouncingBlockColors shieldDamCols)
            : base(args, model, scale)
        {
            shieldWalkDist = 8;
            aggresivity = HumanoidEnemyAgressivity.Agressive_3;
            hasRangedWeapon = false;
            hasHandWeapon = true;

            handWeapon = new Gadgets.HumanoidEnemyHandWeapon(
                VoxelModelName.elf_long_sword1,
                new HandWeaponAttackSettings(
                    GameObjectType.ElfLongSword, 0.8f, weaponScale,
                    new Vector3(
                        HandWeaponAttackSettings.SwordBoundScaleW,
                        HandWeaponAttackSettings.SwordBoundScaleW,
                        HandWeaponAttackSettings.SwordBoundScaleL),
                    new Vector3(3.6f, 6f, 4f),
                    500,
                    HandWeaponAttackSettings.SwordSwingStartAngle,
                    HandWeaponAttackSettings.SwordSwingEndAngle,
                    HandWeaponAttackSettings.SwordSwingPercTime,
                    new WeaponAttack.DamageData(1, WeaponAttack.WeaponUserType.Enemy, NetworkId.Empty)
                    ),
                new Vector3(0, -0.7f, -1f),
                new Effects.BouncingBlockColors(
                    Data.MaterialType.darker_cyan,
                    Data.MaterialType.gray_20,
                    Data.MaterialType.light_cyan));

            shield = new Gadgets.HumanoidEnemyShield(shieldModel, shieldScale,
                new Vector3(-0.16f, 0.32f, 0.18f) * modelScale,
                this, new Effects.BouncingBlockColors(
                    Data.MaterialType.gray_75,
                    Data.MaterialType.gray_85));
            shield.indestructable = true;

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

       

        public override GameObjectType Type
        { get { return GameObjectType.ElfKnight; } }
        override public CardType CardCaptureType
        {
            get { return CardType.OrcKnight; }
        }

        public override float GivesBravery
        { get { return 3; } }

        new protected const float WalkingSpeed = StandardWalkingSpeed * 0.8f;
        new protected const float CasualWalkSpeed = WalkingSpeed * 0.6f;

        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        override protected float walkingSpeed
        {
            get { return WalkingSpeed; }
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.dark_cyan,
            Data.MaterialType.pastel_blue,
            Data.MaterialType.pale_skin
            );

        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }
    }
}
