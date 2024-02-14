//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;


//namespace VikingEngine.LootFest.GameObjects.Characters.EnemyState
//{
//    class RandomWalkingState : AbsEnemyState
//    {
//        public RandomWalkingState()
//            : base() { }
//        public RandomWalkingState(Data.Characters.StartNewEnemyStateArguments arg)
//            : base(arg)
//        {
//            if (!arg.Empty)
//            {
//                arg.Parent.Speed = lib.AngleToV2(lib.RandomRotation(), arg.StateData.Speed.GetRandom());
//                lifeTime = arg.StateData.LifeTime.GetRandom();
//            }
//        }
//        public override AbsEnemyState CreateInstance(Data.Characters.StartNewEnemyStateArguments arg)
//        {
//            return new RandomWalkingState(arg);
//        }
//    }
//}
