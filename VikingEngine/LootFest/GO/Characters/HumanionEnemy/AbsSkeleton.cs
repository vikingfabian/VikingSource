using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.GO.WeaponAttack.Monster;

namespace VikingEngine.LootFest.GO.Characters
{
    class SkeletonBoneThrower : AbsSkeleton
    {
        public SkeletonBoneThrower(GoArgs args)
            : base(args, VoxelModelName.Skeleton)
        {
            preRangeAttackTime = 400;
            projectileRate.Seconds = 4;
            tryKeepRangedDistance = true;

            aggresivity = HumanoidEnemyAgressivity.Agressive_3;
            hasRangedWeapon = true;
            hasRangedWeaponAsRider = true;    

            goblinBoneSword();

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.Skeleton; }
        }

        override public CardType CardCaptureType
        {
            get { return CardType.UnderConstruction; }
        }

    }

    abstract class AbsSkeleton: AbsHumanoidEnemy
    {
        static readonly IntervalF ScaleRange = new IntervalF(3.6f, 4f);

        public AbsSkeleton(GoArgs args, VoxelModelName modelName)
            : base(args)
        {
            modelScale = getGoblinScale();
            createImage(modelName, modelScale, new Graphics.AnimationsSettings(8, 0.6f, 4));
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBoundFromFeetPos(modelScale * 0.22f, modelScale * 0.45f, 0f);

            if (args.LocalMember)
            {
                createAiPhys();
            }
        }

        protected void goblinBoneSword()
        {
            handWeapon = new Gadgets.HumanoidEnemyHandWeapon(
                VoxelModelName.goblin_sword,
                new HandWeaponAttackSettings(
                    GameObjectType.GoblinBoneAttack, 0.8f, 0.16f,
                    new Vector3(2, 4, 6.1f),
                    new Vector3(4f, 7f, 4.2f),
                    500,
                    HandWeaponAttackSettings.SwordSwingStartAngle,
                    HandWeaponAttackSettings.SwordSwingEndAngle,
                    HandWeaponAttackSettings.SwordSwingPercTime,
                    new WeaponAttack.DamageData(1, WeaponAttack.WeaponUserType.Enemy, NetworkId.Empty)
                    ),
                new Vector3(-0.1f, -0.2f, -0.6f),
                new Effects.BouncingBlockColors(Data.MaterialType.gray_20, Data.MaterialType.gray_75)
                );
        }

        virtual protected float getGoblinScale()
        {
            return ScaleRange.GetRandom();
        }

        protected override void createPreRangedAttackEffect()
        {
            //preRangedAttackWeaponEffect = new Effects.PreRangedAttackWeaponEffect(VoxelModelName.goblin_spear,
            //    GoblinJavelin.Scale * 0.1f, GoblinJavelin.Scale * 0.7f, aiStateTimer.MilliSeconds * 0.6f,
            //    new Vector3(5f, 9f, 1f) * image.Scale1D, this);

            if (!isMounted && physics != null)
            {
                physics.Jump(0.4f, image);
            }
        }

        protected override void createRangedAttack()
        {
            //Vector3 targetPos = target.Position;
            //targetPos.Y += 1.5f;
            //new WeaponAttack.Monster.GoblinJavelin( GoArgs.Empty, preRangedAttackWeaponEffect.Position, targetPos);
            Vector3 firePos = image.position + VectorExt.V2toV3XZ(rotation.Direction(image.scale.X * 0.2f), 1);

            int boneCount = Ref.rnd.Int(2, 4);

            float spread = Ref.rnd.Float(0.2f, 0.26f);

            Rotation1D dir = rotation;
            dir.Add(-((boneCount - 1) * 0.5f * spread));

            for (int i = 0; i < boneCount; ++i)
            {
                new WeaponAttack.Monster.SkeletonBone(GoArgs.Empty, firePos, dir);
                dir.Add(spread);
            }
            base.createRangedAttack();
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.gray_20, 
            Data.MaterialType.gray_30, 
            Data.MaterialType.light_pea_green);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        public override MountType MountType
        {
            get { return MountType.Rider; }
        }

        public override bool canBeStunned
        {
            get
            {
                return false;
            }
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
