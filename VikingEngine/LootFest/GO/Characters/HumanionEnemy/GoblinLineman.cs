using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.GO.WeaponAttack.Monster;

namespace VikingEngine.LootFest.GO.Characters
{
    class GoblinLineman : AbsGoblin
    {
        public GoblinLineman(GoArgs args)
            : this(args, VoxelModelName.goblin_lineman, 0.22f, VoxelModelName.goblin_shield, 1.2f, new Effects.BouncingBlockColors(
                    Data.MaterialType.darker_yellow_orange,
                    Data.MaterialType.gray_85,
                    Data.MaterialType.gray_70))
        { }

        public GoblinLineman(GoArgs args, VoxelModelName model, float spearScale, VoxelModelName shieldModel, float shieldScale, 
            Effects.BouncingBlockColors shieldDamCols)
            : base(args, model)
        {
            const float Angle = -0.2f;
            handWeapon = new Gadgets.HumanoidEnemyHandWeapon(
                VoxelModelName.goblin_spear,
                new HandWeaponAttackSettings(
                    GameObjectType.GoblinLinemanSpearAttack, 0.8f, spearScale,
                    new Vector3(2, 2, 4.8f),//bounds
                    new Vector3(2f, 4.7f, 10f),//offset
                    500,
                    Angle,
                    Angle,
                    HandWeaponAttackSettings.SwordSwingPercTime,
                    new WeaponAttack.DamageData(1, WeaponAttack.WeaponUserType.Enemy, NetworkId.Empty)
                    ),
                new Vector3(0, -0.4f, -1f),
                new Effects.BouncingBlockColors(
                    Data.MaterialType.dark_yellow_orange,
                    Data.MaterialType.dark_red,
                    Data.MaterialType.gray_85)
                );
            aggresivity = HumanoidEnemyAgressivity.Dodgy_1;
            shieldWalkDist = 7;

            shield = new Gadgets.HumanoidEnemyShield(shieldModel, shieldScale,
                new Vector3(-0.176f, 0.35f, 0.20f) * modelScale, //posOffset
                this, shieldDamCols);

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
            
        }

     
       

        protected override void shieldHit(WeaponAttack.DamageData damage, bool local)
        {
            base.shieldHit(damage, local);
            aggresivity = HumanoidEnemyAgressivity.ChickenShit_0;
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.GoblinLineman; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.GoblinLineman; }
        }
        new static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.dark_warm_brown,
            Data.MaterialType.light_yellow_green,
            Data.MaterialType.darker_warm_brown
            );

        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }
    }
}
