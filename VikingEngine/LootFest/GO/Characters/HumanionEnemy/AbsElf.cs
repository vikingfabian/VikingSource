using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.GO.WeaponAttack.Monster;

namespace VikingEngine.LootFest.GO.Characters.HumanionEnemy
{
    abstract class AbsElf: AbsHumanoidEnemy
    {
        protected static readonly IntervalF ElfScaleRange = new IntervalF(3.6f, 4f);
        //static readonly IntervalF ScaleRange = new IntervalF(2.5f, 3f);

        public AbsElf(GoArgs args, VoxelModelName modelName, IntervalF scaleRange)
            : base(args)
        {
            this.WorldPos = args.startWp;
            modelScale = scaleRange.GetRandom();
            createImage(modelName, modelScale, new Graphics.AnimationsSettings(11, 0.6f, 6));
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBoundFromFeetPos(modelScale * 0.22f, modelScale * 0.45f, 0f);

            if (args.LocalMember)
            {
                createAiPhys();
            }
        }

       

        protected void elfSword()
        {
            handWeapon = new Gadgets.HumanoidEnemyHandWeapon(
                VoxelModelName.elf_sword1,
                new HandWeaponAttackSettings(
                    GameObjectType.ElfSword, 0.8f, 0.21f,
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
                    Data.MaterialType.light_cyan,
                    Data.MaterialType.gray_10,
                    Data.MaterialType.dark_cyan_blue)
            );
        }

        //virtual protected float getGoblinScale()
        //{
        //    return ScaleRange.GetRandom();
        //}

        protected override void createPreRangedAttackEffect()
        {
            preRangedAttackWeaponEffect = new Effects.PreRangedAttackWeaponEffect(VoxelModelName.goblin_spear,
                GoblinJavelin.Scale * 0.1f, GoblinJavelin.Scale * 0.7f, aiStateTimer.MilliSeconds * 0.6f,
                new Vector3(5f, 9f, 1f) * image.Scale1D, this);
        }

        protected override void createRangedAttack()
        {
            //Vector3 targetPos = target.Position;
            //targetPos.Y += 1.5f;
            //new WeaponAttack.Monster.GoblinJavelin(preRangedAttackWeaponEffect.Position, targetPos);
            base.createRangedAttack();
        }

        //public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
        //    Data.MaterialType.pure_pea_green, 
        //    Data.MaterialType.pale_skin, 
        //    Data.MaterialType.light_yellow);
        //public override Effects.BouncingBlockColors DamageColors
        //{
        //    get
        //    {
        //        return DamageColorsLvl1;
        //    }
        //}

        public override MountType MountType
        {
            get { return MountType.Rider; }
        }

        protected const float WalkingSpeed = 0.008f;
        protected const float CasualWalkSpeed = StandardWalkingSpeed * 0.5f;
        protected const float ShieldWalkSpeed = StandardCasualWalkSpeed;
        protected const float RushSpeed = 0.015f;

        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        override protected float shieldWalkSpeed
        {
            get { return ShieldWalkSpeed; }
        }
        override protected float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        override protected float rushSpeed
        {
            get { return RushSpeed; }
        }
    }
}

