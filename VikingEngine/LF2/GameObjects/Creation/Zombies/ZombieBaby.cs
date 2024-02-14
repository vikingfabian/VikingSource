using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.Creation.Zombies
{
    class ZombieBaby: AbsZombie
    {
        public ZombieBaby()
            : base(0)
        { }
        public ZombieBaby(System.IO.BinaryReader packetReader)
            : base(packetReader)
        {}
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.zombiebaby; }
        }

        static readonly IntervalF Scale = CharacterScale * 0.5f;
        protected override IntervalF ScaleRange
        {
            get { return Scale; }
        }

        static readonly IntervalF MySpeedRange = new IntervalF(0.007f, 0.008f);
        override protected IntervalF SpeedRange { get { return MySpeedRange; } }

        static readonly IntervalF MyRunningSpeedRange = new IntervalF(0.01f, 0.014f);
        override protected IntervalF RunningSpeedRange { get { return MyRunningSpeedRange; } }

        static readonly IntervalF MyBasicStateTime = new IntervalF(500, 1000);
        override protected IntervalF BasicStateTime { get { return MyBasicStateTime; } }

        const int MyChanceToMove = 90;
        override protected int ChanceToMove { get { return MyChanceToMove; } }

        const float TargetRndAngleAdd = MathHelper.Pi * 0.6f;
        override protected float WalkingToTargetRndAngleAdd { get { return TargetRndAngleAdd; } }

        protected override float StartHealth { get { return 1; } }

        //public override Data.Characters.EnemyType EnemyType
        //{
        //    get { return Data.Characters.EnemyType.ZombieBaby; }
        //}

        override protected AttackType attackType { get { return AttackType.Projectile; } }

        static readonly IntervalF FireDist = new IntervalF(8, 16);
        override protected IntervalF attackDistance { get { return FireDist; } }

        static readonly Range FireReloadTime = new Range(0, 3);
        override protected Range attackReloadTimeInStates { get { return FireReloadTime; } }

        
        
        
        override protected void fireProjectile() 
        {
            Vector3 start = image.Position;
            start.Y += 1f;
            Vector3 goal = targetHero.Position;
            goal.Y += 2f;
           Weapon.ZombieProjectile projectile = new Weapon.BabyAxe(
                start, goal);

           Rotation = projectile.Velocity.Rotation1D();//Rotation1D.FromDirection(projectile.Speed);
           this.setImageDirFromRotation();
           Velocity.SetZeroPlaneSpeed();
        }
        protected override bool willFlee
        { get { return true; } }
        override protected float fleeRange { get { return 8; } }

        protected override LoadedSound MoanSound
        {
            get
            {
                return LoadedSound.zombie_6_a;
            }
        }
        protected override LoadedSound HurtSound
        {
            get
            {
                return LoadedSound.zombie_6_e;
            }
        }
        Graphics.AnimationsSettings AnimationsSettings = new Graphics.AnimationsSettings(3, 0.8f);
        override protected Graphics.AnimationsSettings animationsSettings { get { return AnimationsSettings; } }
        public override GameObjects.Characters.CharacterUtype CharacterType
        {
            get { return GameObjects.Characters.CharacterUtype.BabyZombie; }
        }
    }
}
