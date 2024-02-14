using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.GameObjects.Characters.EnemyState
{
    //abstract class AbsEnemyState : Update2
    //{
    //    protected Hero target;
    //    protected float lifeTime = float.MaxValue;
    //    protected float ignoreEnemyTime = 800;
    //    protected Data.Characters.StartNewEnemyStateArguments enemyStateData;

    //    public AbsEnemyState()
    //        :base(false)
    //    { }
    //    public AbsEnemyState(Data.Characters.StartNewEnemyStateArguments arg)
    //        :base(false)
    //    {
    //        enemyStateData = arg;

    //    }
    //    /// <returns>The time before a new hero seach should start</returns>
    //    virtual public int SetTarget(Hero hero)
    //    {
    //        target = hero;
    //        return 1000;
    //    }
    //    virtual public void OsticleCollition()//Physics.CollisionIntersection collData)
    //    {
    //        if ( enemyStateData.Parent.SpeedX > enemyStateData.Parent.SpeedY) //collData.XbiggerThanY)
    //        {//X
    //            enemyStateData.Parent.SpeedX *= -1;
    //        }
    //        else
    //        {//Y
    //            enemyStateData.Parent.SpeedY *= -1;
    //        }
    //        enemyStateData.Parent.Speed = lib.ChangeV2Angle(enemyStateData.Parent.Speed, new Rotation1D(Ref.rnd.Plus_MinusF(1f)));
    //    }
    //    public override void Time_Update(float time)
    //    {
    //    }
    //    public override void Time_LasyUpdate(float time)
    //    {
    //        lifeTime -= time;
    //        if (lifeTime <= 0)
    //            enemyStateData.Parent.NewStateEvent();

    //        if (enemyStateData.Parent.SeekHero)
    //        {
    //            ignoreEnemyTime -= time;
    //            if (ignoreEnemyTime <= 0)
    //            {
    //                foreach (GameObjects.Characters.Hero h in LfRef.Heroes)
    //                {
    //                    if (enemyStateData.Parent.PositionDiff(h).Length() <= enemyStateData.MovepatternData.HeroSeekRadius)
    //                    {
    //                        enemyStateData.Parent.HeroFoundEvent(h);
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    abstract public AbsEnemyState CreateInstance(Data.Characters.StartNewEnemyStateArguments arg);
    //    public virtual bool WeaponAttack { get { return false; } }
    //}
    //enum EnemyStateType
    //{
    //    NoState = -1, //used as a null
    //    Wait = 0,
    //    WalkInRandomDir,
    //    FollowHero,
    //    NUM
    //}
}
