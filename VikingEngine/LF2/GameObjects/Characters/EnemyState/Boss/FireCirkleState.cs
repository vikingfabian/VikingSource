//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;

//namespace VikingEngine.LootFest.GameObjects.Characters.EnemyState.Boss
//{
//    class FireCirkleState : AbsEnemyState
//    {
//        float fireTime;
//        bool aim;
//        public FireCirkleState()
//            : base() { }
//        public FireCirkleState(Data.Characters.StartNewEnemyStateArguments arg)
//            : base(arg)
//        {
//            if (!arg.Empty)
//            {
//                //if (arg.StateData.Stop)
//                //{
//                    enemyStateData.Parent.Speed = Vector2.Zero;
//                //}
//                fireTime = arg.StateData.LifeTime.GetRandom();
//                lifeTime = fireTime + arg.StateData.TimeAfter.GetRandom();
//                aim = arg.StateData.Aim;
//            }
//        }
//        public override void Time_Update(float time)
//        {

//            if (aim)
//            {//kan bugga om gubben rör på sig
//                enemyStateData.Parent.SetRotation(enemyStateData.Parent.AngleDirToObject(target));
//            }

//            if (fireTime > 0)
//            {
//                fireTime -= time;
//                if (fireTime <= 0)
//                {
                    
//                    const int NumBullets = 8;
//                    float angleStep = MathHelper.TwoPi / NumBullets;
//                    Rotation1D angle = enemyStateData.Parent.Rotation;
//                    for (int i = 0; i < NumBullets; i++)
//                    {
//                        enemyStateData.Parent.SharedData.Weapon.Fire(angle.Radians, enemyStateData.Parent.Position);
//                        angle.Add(angleStep);
//                    }

                        
//                }
//            }
//            base.Time_Update(time);
//        }
//        public override void OsticleCollition()//Physics.CollisionIntersection collData)
//        {
//            base.OsticleCollition();
//            lifeTime = 0;
//        }
//        public override AbsEnemyState CreateInstance(Data.Characters.StartNewEnemyStateArguments arg)
//        {
//            return new FireCirkleState(arg);
//        }
//        public override bool WeaponAttack { get { return true; } }
//    }
//}
