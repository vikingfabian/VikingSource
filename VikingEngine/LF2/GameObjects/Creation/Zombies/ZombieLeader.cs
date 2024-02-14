using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.Creation.Zombies
{
    class ZombieLeader : AbsZombie
    {
        public ZombieLeader()
            : base(0)
        { }
        public ZombieLeader(System.IO.BinaryReader packetReader)
            : base(packetReader)
        {}
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.zombieleader; }
        }
        
        static readonly IntervalF Scale = CharacterScale * 1.2f;
        protected override IntervalF ScaleRange
        {
            get { return Scale; }
        }

        static readonly IntervalF MySpeedRange = new IntervalF(0.005f, 0.007f);
        override protected IntervalF SpeedRange { get { return MySpeedRange; } }
        protected override IntervalF RunningSpeedRange { get { return MySpeedRange; } }
        
        static readonly IntervalF MyBasicStateTime = new IntervalF(600, 1500);
        override protected IntervalF BasicStateTime { get { return MyBasicStateTime; } } 
        
        const int MyChanceToMove = 80;
        override protected int ChanceToMove { get { return MyChanceToMove; } }
        
        const float TargetRndAngleAdd = MathHelper.Pi * 1.2f;
        override protected float WalkingToTargetRndAngleAdd { get { return TargetRndAngleAdd; } }

        protected override float StartHealth { get { return 3; } }


        override protected AttackType attackType { get { return AttackType.NONE; } }
        
        //public override Data.Characters.EnemyType EnemyType
        //{
        //    get { return Data.Characters.EnemyType.ZombieLeader; }
        //}
        protected override bool isLeader
        {
            get
            {
                return true;
            }
        }
        protected override LoadedSound MoanSound
        {
            get
            {
                return LoadedSound.zombie_3_a;
            }
        }
        protected override LoadedSound HurtSound
        {
            get
            {
                return LoadedSound.zombie_3_h;
            }
        }
        Graphics.AnimationsSettings AnimationsSettings = new Graphics.AnimationsSettings(3, 1.2f);
        override protected Graphics.AnimationsSettings animationsSettings { get { return AnimationsSettings; } }
        public override GameObjects.Characters.CharacterUtype CharacterType
        {
            get { return GameObjects.Characters.CharacterUtype.LeaderZombie; }
        }
    }
}
