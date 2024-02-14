using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Characters.EnemyState.Boss
{
    //class SmoothBullRushState:  AbsEnemyState
    //{
    //    float speed;
    //    public SmoothBullRushState()
    //        : base() { }
    //    public SmoothBullRushState(Data.Characters.StartNewEnemyStateArguments arg)
    //        : base(arg)
    //    {
    //        lifeTime = arg.StateData.LifeTime.GetRandom();
    //        speed = arg.StateData.Speed.GetRandom();
    //    }
    //    public override int SetTarget(Hero hero)
    //    {
    //        Music.SoundManager.PlaySound(LoadedSound.EnemySound1, enemyStateData.Parent.Position, (int)hero.Player.Index);
    //        base.SetTarget(hero);
    //        enemyStateData.Parent.Speed = enemyStateData.Parent.Rotation.Direction(speed); //lib.AngleToV2(enemyStateData.Parent.AngleDirToObject(target), speed);
    //        return (int)lifeTime;
    //    }
    //    public override void Time_Update(float time)
    //    {
    //        if (target != null)
    //        {
    //            float angleDiff = enemyStateData.Parent.AngleDirToObject(target);

    //            const float AngleAddSpeed = 0.02f;
    //            if (Math.Abs(angleDiff) >= AngleAddSpeed)
    //            {
    //                enemyStateData.Parent.Rotation.Add(AngleAddSpeed * lib.FloatToDir(angleDiff));
    //                enemyStateData.Parent.Speed = enemyStateData.Parent.Rotation.Direction(speed);
    //            }
    //        }
    //    }
    //    public override void OsticleCollition()//Physics.CollisionIntersection collData)
    //    {
    //        base.OsticleCollition();//collData);
    //        enemyStateData.Parent.NewStateEvent();
    //    }
    //    public override AbsEnemyState CreateInstance(Data.Characters.StartNewEnemyStateArguments arg)
    //    {
    //        return new SmoothBullRushState(arg);
    //    }
    //}
}
