//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;


//namespace VikingEngine.LootFest.GameObjects.Characters.EnemyState
//{
//    class WaitingState : AbsEnemyState
//    {
//        public WaitingState() 
//            :base()
//        { }
//        public WaitingState(Data.Characters.StartNewEnemyStateArguments arg)
//            : base(arg)
//        {
//            lifeTime = arg.StateData.LifeTime.GetRandom();
//            enemyStateData.Parent.Speed = Vector2.Zero;
//        }
//        public override AbsEnemyState CreateInstance(Data.Characters.StartNewEnemyStateArguments arg)
//        {
//            return new WaitingState(arg);
//        }
//    }
//}
