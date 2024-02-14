//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;


//namespace VikingEngine.LootFest.GameObjects.Characters.EnemyState
//{
//    class FollowingState : AbsEnemyState
//    {
//        //Hero target;
//        float speed;
//        public FollowingState()
//            : base() { }
//        public FollowingState(Data.Characters.StartNewEnemyStateArguments arg)
//            : base(arg)
//        {
//            Music.SoundManager.PlaySound(LoadedSound.EnemySound1, enemyStateData.Parent.Position, 0);
//            speed = arg.StateData.Speed.GetRandom();
//        }
//        public override void Time_Update(float time)
//        {
//            enemyStateData.Parent.Speed = lib.AngleToV2(enemyStateData.Parent.AngleDirToObject(target), speed);
//            base.Time_Update(time);
//        }
//        public override void OsticleCollition()//Physics.CollisionIntersection collData)
//        {
//            base.OsticleCollition();
//            DeleteMe();
//        }
//        public override AbsEnemyState CreateInstance(Data.Characters.StartNewEnemyStateArguments arg)
//        {
//            return new FollowingState(arg);
//        }
//    }
//}
