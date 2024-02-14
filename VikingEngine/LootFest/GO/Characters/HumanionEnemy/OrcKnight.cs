using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters
{
    class OrcKnight: AbsOrc
    {
        public OrcKnight(GoArgs args)
            : base(args, VoxelModelName.orc_knight, OrcScaleRange)
        {
            shieldWalkDist = 8;
            aggresivity = HumanoidEnemyAgressivity.Agressive_3;
            hasRangedWeapon = false;
            hasHandWeapon = true;

            const float Angle = -0.2f;
            handWeapon = new Gadgets.HumanoidEnemyHandWeapon(
                VoxelModelName.orc_handspear,
                new HandWeaponAttackSettings(
                    GameObjectType.OrcKnightAttack, 0.8f, 0.3f,
                    new Vector3(2, 2, 8),//bounds
                    new Vector3(3f, 3f, 3f),//offset
                    500,
                    Angle,
                    Angle,
                    HandWeaponAttackSettings.SwordSwingPercTime,
                    new WeaponAttack.DamageData(1, WeaponAttack.WeaponUserType.Enemy, NetworkId.Empty)
                    ),
                new Vector3(0, -0.4f, -2f),
                new Effects.BouncingBlockColors(Data.MaterialType.gray_60, Data.MaterialType.gray_80)
                );

            shield = new Gadgets.HumanoidEnemyShield(VoxelModelName.orc_steel_shield, 3f,
                new Vector3(-0.16f, 0.32f, 0.18f) * modelScale, //posOffset
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
        { get { return GameObjectType.OrcKnight; } }
        override public CardType CardCaptureType
        {
            get { return CardType.OrcKnight; }
        }

        public override float GivesBravery
        { get { return 3; } }

        protected const float WalkingSpeed = StandardWalkingSpeed * 0.8f;
        protected const float CasualWalkSpeed = WalkingSpeed * 0.6f;

        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        override protected float walkingSpeed
        {
            get { return WalkingSpeed; }
        }

        new static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.dark_cool_brown,
            Data.MaterialType.light_blue,
            Data.MaterialType.light_pea_green
            );

        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }
    }
}
