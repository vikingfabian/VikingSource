//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;

//namespace VikingEngine.LootFest.GameObjects.Characters.EnemyState.Boss
//{
//    class SprayFireState: AbsEnemyState
//    {
//        int bulletsLeft = 6;
//        float fireTime;
//        float dir;
//        static readonly RangeF AngleStep = new RangeF(0.1f, 0.4f);
//        Rotation1D angle;// = enemyStateData.Parent.Rotation.Radians + Ref.rnd.Plus_MinusF(0.3f);
//        static readonly RangeF StepTime = new RangeF(30, 200);
//        float stepTime;// = StepTime.GetRandom();
//        //bool aim;
//        public SprayFireState()
//            : base() { }
//        public SprayFireState(Data.Characters.StartNewEnemyStateArguments arg)
//            : base(arg)
//        {
//            if (!arg.Empty)
//            {
//                if (arg.StateData.Stop)
//                {
//                    enemyStateData.Parent.Speed = Vector2.Zero;
//                }
//                fireTime = arg.StateData.LifeTime.GetRandom();
//                stepTime = StepTime.GetRandom();
//                lifeTime = fireTime + stepTime * bulletsLeft + arg.StateData.TimeAfter.GetRandom();
//                //aim = arg.StateData.Aim;

//                angle = enemyStateData.Parent.Rotation;
//                dir = lib.RandomDirection();
//                angle.Add((1 + Ref.rnd.Plus_MinusF(0.2f)) * dir);
//                //const float AngleStep = 0.4f;
//                dir *= AngleStep.GetRandom();
                
//            }
//        }
//        public override void Time_Update(float time)
//        {

//            //if (aim)
//            //{//kan bugga om gubben rör på sig
//            //    enemyStateData.Parent.SetRotation(enemyStateData.Parent.AngleDirToObject(target));
//            //}


//            //if (fireTime > 0)
//            //{
//                if (bulletsLeft > 0)
//                {


//                    fireTime -= time;
//                    if (fireTime <= 0)
//                    {
//                        bulletsLeft--;
//                        fireTime = stepTime;
//                        //fire
//                        enemyStateData.Parent.SharedData.Weapon.Fire(angle.Radians, enemyStateData.Parent.Position);
//                        angle.Add(dir);
//                    }
//                }
//                else
//                {
//                    fireTime = float.MaxValue;
//                }
//            //}
//            base.Time_Update(time);
//        }
//        public override void OsticleCollition()//Physics.CollisionIntersection collData)
//        {
//            base.OsticleCollition();
//            lifeTime = 0;
//        }
//        public override AbsEnemyState CreateInstance(Data.Characters.StartNewEnemyStateArguments arg)
//        {
//            return new SprayFireState(arg);
//        }
//        public override bool WeaponAttack { get { return true; } }
//    }
//}
