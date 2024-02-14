using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.Creation.Zombies
{
    class Skeleton : AbsBoostedZombie
    {
        public Skeleton()
            : base()
        { }
        public Skeleton(System.IO.BinaryReader packetReader)
            : base(packetReader)
        {}
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.Skeleton; }
        }
        protected override IntervalF ScaleRange
        {
            get { return CharacterScale; }
        }

        static readonly IntervalF MySpeedRange = new IntervalF(0.006f, 0.0075f);
        override protected IntervalF SpeedRange { get { return MySpeedRange; } }

        static readonly IntervalF MyRunningSpeedRange = new IntervalF(0.01f, 0.014f);
        override protected IntervalF RunningSpeedRange { get { return MyRunningSpeedRange; } }

        static readonly IntervalF MyBasicStateTime = new IntervalF(500, 1000);
        override protected IntervalF BasicStateTime { get { return MyBasicStateTime; } }

        const int MyChanceToMove = 60;
        override protected int ChanceToMove { get { return MyChanceToMove; } }

        const float TargetRndAngleAdd = MathHelper.Pi * 0.6f;
        override protected float WalkingToTargetRndAngleAdd { get { return TargetRndAngleAdd; } }

        protected override float StartHealth { get { return 1; } }

        //public override Data.Characters.EnemyType EnemyType
        //{
        //    get { return Data.Characters.EnemyType.Skeleton; }
        //}

        override protected AttackType attackType { get { return AttackType.Projectile; } }

        static readonly IntervalF FireDist = new IntervalF(6, 12);
        static readonly IntervalF FireDistBoosted = new IntervalF(4, 18);
        override protected IntervalF attackDistance { get { return boosted? FireDistBoosted : FireDist; } }

        static readonly Range FireReloadTime = new Range(2, 4);
        override protected Range attackReloadTimeInStates { get { return FireReloadTime; } }

        override protected void LootDrop()
        {
#if CMODE
            if (lib.PercentChance(40))
            {
                new GameObjects.PickUp.ThrowingSpear(image.Position);
            }
            else
            {
                new GameObjects.PickUp.Coin(image.Position);
            }
#endif
        }

        override protected void fireProjectile() 
        {
            Weapon.ZombieProjectile projectile = new Weapon.SkeletonBone(image.Position, targetHero.Position, boosted);
            Rotation = projectile.Velocity.Rotation1D();//Rotation1D.FromDirection(projectile.Speed);
            this.setImageDirFromRotation();
            Velocity.SetZeroPlaneSpeed();
            
        }
        public override GameObjects.Characters.CharacterUtype CharacterType
        {
            get { return GameObjects.Characters.CharacterUtype.Skeleton; }
        }
        Graphics.AnimationsSettings AnimationsSettings = new Graphics.AnimationsSettings(5, 0.5f);
        override protected Graphics.AnimationsSettings animationsSettings { get { return AnimationsSettings; } }
    }
}
