using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.GameObjects.Characters.EnemyState
{
    //class FireState : AbsEnemyState
    //{
    //    float fireTime;
    //    bool aim;
    //    public FireState()
    //        : base() { }
    //    public FireState(Data.Characters.StartNewEnemyStateArguments arg)
    //        : base(arg)
    //    {
    //        if (!arg.Empty)
    //        {
    //            if (arg.StateData.Stop)
    //            {
    //                enemyStateData.Parent.Speed = Vector2.Zero;
    //            }
    //            fireTime = arg.StateData.LifeTime.GetRandom();
    //            lifeTime = fireTime + arg.StateData.TimeAfter.GetRandom();
    //            aim = arg.StateData.Aim;
    //        }
    //    }
    //    public override void Time_Update(float time)
    //    {

    //        if (aim)
    //        {//kan bugga om gubben rör på sig
    //            enemyStateData.Parent.SetRotation(enemyStateData.Parent.AngleDirToObject(target));
    //        }

    //        if (fireTime > 0)
    //        {
    //            fireTime -= time;
    //            if (fireTime <= 0)
    //            {
    //                //fire
    //                float angle;
                   
    //                angle = enemyStateData.Parent.Rotation.Radians+ Ref.rnd.Plus_MinusF(0.3f);
                    
    //                enemyStateData.Parent.SharedData.Weapon.Fire(angle, enemyStateData.Parent.Position);
    //                //share to network
    //                enemyStateData.Parent.NetworkSendAttack();
    //            }
    //        }
    //        base.Time_Update(time);
    //    }
    //    public override void OsticleCollition()//Physics.CollisionIntersection collData)
    //    {
    //        base.OsticleCollition();
    //        lifeTime = 0;
    //    }
    //    public override AbsEnemyState CreateInstance(Data.Characters.StartNewEnemyStateArguments arg)
    //    {
    //        return new FireState(arg);
    //    }
    //}
}
