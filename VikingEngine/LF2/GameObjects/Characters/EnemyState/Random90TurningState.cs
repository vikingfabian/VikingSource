//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;


//namespace VikingEngine.LootFest.GameObjects.Characters.EnemyState
//{
//    class Random90TurningState :  AbsEnemyState
//    {
//        float speed;
//        public Random90TurningState()
//            : base() { }
//        public Random90TurningState(Data.Characters.StartNewEnemyStateArguments arg)
//            : base(arg)
//        {
//            Rotation1D rot = new Rotation1D(enemyStateData.Parent.Rotation.Radians + lib.RandomDirection() * MathHelper.PiOver2);
//            enemyStateData.Parent.Speed = rot.Direction(arg.StateData.Speed.GetRandom());
//            lifeTime = arg.StateData.LifeTime.GetRandom();
//        }
//        public override AbsEnemyState CreateInstance(Data.Characters.StartNewEnemyStateArguments arg)
//        {
//            return new Random90TurningState(arg);
//        }
//    }
//}
