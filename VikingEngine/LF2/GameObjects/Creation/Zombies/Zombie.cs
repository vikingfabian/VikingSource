using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.Creation.Zombies
{
    class Zombie : AbsBoostedZombie
    {
        //bool boosted = false;

        public Zombie()
            : base()
        { }
        public Zombie(System.IO.BinaryReader packetReader)
            : base(packetReader)
        {}
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.zombie1; }
        }
        protected override IntervalF ScaleRange
        {
            get { return CharacterScale; }
        }

        static readonly IntervalF MySpeedRange = new IntervalF(0.005f, 0.007f);
        static readonly IntervalF MySpeedRangeBoosted = new IntervalF(0.007f, 0.008f);
        override protected IntervalF SpeedRange { get { return boosted? MySpeedRangeBoosted : MySpeedRange; } } 
        
        static readonly IntervalF MyRunningSpeedRange = new IntervalF(0.01f, 0.014f);
        override protected IntervalF RunningSpeedRange { get { return MyRunningSpeedRange; } } 
        
        static readonly IntervalF MyBasicStateTime = new IntervalF(1000, 3000);
        override protected IntervalF BasicStateTime { get { return MyBasicStateTime; } } 
        
        const int MyChanceToMove = 50;
        override protected int ChanceToMove { get { return MyChanceToMove; } }
        
        const float TargetRndAngleAdd = MathHelper.Pi * 0.8f;
        override protected float WalkingToTargetRndAngleAdd { get { return TargetRndAngleAdd; } }

        protected override float StartHealth { get { return 2; } }

      

        static readonly IntervalF FireDist = new IntervalF(2, 8);
        static readonly IntervalF FireDistBoosted = new IntervalF(2, 16);
        override protected IntervalF attackDistance { get { return boosted? FireDistBoosted : FireDist; } }
        override protected AttackType attackType { get { return AttackType.Charge; } }
        static readonly Range FireReloadTime = new Range(0, 1);
        override protected Range attackReloadTimeInStates { get { return FireReloadTime; } }

        //public override Data.Characters.EnemyType EnemyType
        //{
        //    get { return Data.Characters.EnemyType.Zombie; }
        //}
        public override GameObjects.Characters.CharacterUtype CharacterType
        {
            get { return GameObjects.Characters.CharacterUtype.Zombie; }
        }
        Graphics.AnimationsSettings AnimationsSettings = new Graphics.AnimationsSettings(5, 0.5f);
        override protected Graphics.AnimationsSettings animationsSettings { get { return AnimationsSettings; } }
    }
}
