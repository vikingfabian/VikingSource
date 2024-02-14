using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.Creation.Zombies
{
    class ZombieDog : AbsBoostedZombie
    {
        public ZombieDog()
            : base()
        { }
        public ZombieDog(System.IO.BinaryReader packetReader)
            : base(packetReader)
        {}
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.EnemyDog; }
        }
        protected override IntervalF ScaleRange
        {
            get { return CharacterScale; }
        }

        static readonly IntervalF MySpeedRange = new IntervalF(0.005f, 0.007f);
        override protected IntervalF SpeedRange { get { return MySpeedRange; } } 
        
        static readonly IntervalF MyRunningSpeedRange = new IntervalF(0.012f, 0.016f);
        static readonly IntervalF MyRunningSpeedRangeBoosted = new IntervalF(0.016f, 0.017f);
        override protected IntervalF RunningSpeedRange { get { return (boosted? MyRunningSpeedRangeBoosted : MyRunningSpeedRange); } } 
        
        static readonly IntervalF MyBasicStateTime = new IntervalF(1500, 3000);
        override protected IntervalF BasicStateTime { get { return MyBasicStateTime; } } 
        
        const int MyChanceToMove = 70;
        override protected int ChanceToMove { get { return MyChanceToMove; } }
        
        const float TargetRndAngleAdd = MathHelper.Pi * 0.1f;
        override protected float WalkingToTargetRndAngleAdd { get { return TargetRndAngleAdd; } }

        protected override float StartHealth { get { return 2; } }

        static readonly IntervalF FireDist = new IntervalF(2, 24);
        static readonly IntervalF FireDistBoosted = new IntervalF(2, 35);
        override protected IntervalF attackDistance { get { return boosted? FireDistBoosted : FireDist; } }
        override protected AttackType attackType { get { return AttackType.Charge; } }
        static readonly Range FireReloadTime = new Range(0, 2);
        override protected Range attackReloadTimeInStates { get { return FireReloadTime; } }

        //public override Data.Characters.EnemyType EnemyType
        //{
        //    get { return Data.Characters.EnemyType.ZombieDog; }
        //}
        protected override LoadedSound MoanSound
        {
            get
            {
                return LoadedSound.NON;
            }
        }
        protected override LoadedSound HurtSound
        {
            get
            {
                return LoadedSound.zombie_2_e;
            }
        }
        Graphics.AnimationsSettings AnimationsSettings = new Graphics.AnimationsSettings(5, 0.6f);
        override protected Graphics.AnimationsSettings animationsSettings { get { return AnimationsSettings; } }
        public override GameObjects.Characters.CharacterUtype CharacterType
        {
            get { return GameObjects.Characters.CharacterUtype.DogZombie; }
        }
    }
}
