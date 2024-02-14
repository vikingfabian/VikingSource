using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Characters.EnemyState
{
    //class RandomTurningState : AbsEnemyState
    //{
    //    float speed;
    //    float turningSpeed = 0;
    //    Rotation1D dir;

    //    public RandomTurningState()
    //        : base() { }
    //    public RandomTurningState(Data.Characters.StartNewEnemyStateArguments arg)
    //        : base(arg)
    //    {
    //        speed = arg.StateData.Speed.GetRandom();
    //        dir = Rotation1D.Random();
    //    }
    //    public override void Time_Update(float time)
    //    {    
    //        const float MaxTurningSpeed = 0.002f;
    //        if (Ref.rnd.RandomChance(5))
    //        {
    //            turningSpeed = Bound.Set(turningSpeed + Ref.rnd.Plus_MinusF(MaxTurningSpeed * 0.5f), -MaxTurningSpeed, MaxTurningSpeed);
    //        }
    //        dir.Radians += turningSpeed * time;
    //        enemyStateData.Parent.Speed = dir.Direction(speed);
    //        base.Time_Update(time);
    //    }
    //    public override void OsticleCollition()//Physics.CollisionIntersection collData)
    //    {
    //        base.OsticleCollition();
    //        turningSpeed = 0;
    //        dir = enemyStateData.Parent.Rotation;
    //    }
    //    public override AbsEnemyState CreateInstance(Data.Characters.StartNewEnemyStateArguments arg)
    //    {
    //        return new RandomTurningState(arg);
    //    }
    //}
}
