using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Characters.EnemyState
{
    
    //class BullRushState :  AbsEnemyState
    //{
    //    float speed;
    //    public BullRushState()
    //        : base() { }
    //    public BullRushState(Data.Characters.StartNewEnemyStateArguments arg)
    //        : base(arg)
    //    {
    //        lifeTime = arg.StateData.LifeTime.GetRandom();
    //        speed = arg.StateData.Speed.GetRandom();
    //    }
    //    public override int SetTarget(Hero hero)
    //    {
    //        Music.SoundManager.PlaySound(LoadedSound.EnemySound1, enemyStateData.Parent.Position, (int)hero.Player.Index);
    //        base.SetTarget(hero);
    //        enemyStateData.Parent.Speed = lib.AngleToV2(enemyStateData.Parent.AngleDirToObject(target), speed);
    //        return (int)lifeTime;
    //    }
    //    public override void OsticleCollition()//Physics.CollisionIntersection collData)
    //    {
    //        base.OsticleCollition();
    //        enemyStateData.Parent.NewStateEvent();
    //    }
    //    public override AbsEnemyState CreateInstance(Data.Characters.StartNewEnemyStateArguments arg)
    //    {
    //        return new BullRushState(arg);
    //    }
    //}
}
