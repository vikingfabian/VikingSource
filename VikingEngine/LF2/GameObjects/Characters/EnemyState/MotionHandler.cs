using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2.Data.Characters;

namespace VikingEngine.LF2.GameObjects.Characters.EnemyState
{
    
    //struct MotionHandler
    //{
    //    bool seeksHero; //?
    //    public bool SeekHero
    //    {
    //        get { return seeksHero && !seekHeroBlock; }
    //    }
    //    bool seekHeroBlock;
    //    Data.Characters.StartNewEnemyStateArguments arg;
    //    int currentActionIndex;
    //    public AbsEnemyState currentState;
        
    //    public MotionHandler(Data.Characters.StartNewEnemyStateArguments arg)
    //    {
    //        this.arg = arg;
    //        currentActionIndex = 0;
    //        currentState = arg.MovepatternData.Init(arg); //.actionList[0].StartNewState(arg);
    //        seeksHero = arg.MovepatternData.SeeksHero;
    //        seekHeroBlock = false;
    //    }
       
    //    public void NewStateEvent()
    //    {
    //        seekHeroBlock = false;
    //        currentState.DeleteMe();
    //        //currentActionIndex.Next();
    //        currentState = arg.MovepatternData.Next(arg, ref currentActionIndex); //actionList[currentActionIndex.Value].StartNewState(arg);
    //    }
    //    public void FoundHero(GameObjects.Characters.Hero h)
    //    {
    //       // if (arg.Parent.EnemyType != EnemyType.Trap && )
    //        seekHeroBlock = arg.Parent.EnemyType == EnemyType.Trap;
    //        currentState.DeleteMe();
    //        currentState = arg.MovepatternData.FoundHero(arg); //heroFoundAction.StartNewState(arg);
    //        currentState.SetTarget(h);
    //    }
    //    public void TakeOrGiveDamage(bool take)
    //    {
    //        //if (!arg.MovepatternData.damageAction.IsNull)
    //        //{
    //            currentState.DeleteMe();
    //            currentState = arg.MovepatternData.TakeDamage(arg);
    //        //}
    //    }
    //    public void ObsticleCollision()//Physics.CollisionIntersection collData)
    //    {
    //        currentState.OsticleCollition();
    //    }
    //    public void DeleteMe()
    //    {
    //        currentState.DeleteMe();
    //    }
    //}
    
}
