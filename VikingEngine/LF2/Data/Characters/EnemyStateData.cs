using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using VikingEngine.LF2.GameObjects.Characters.EnemyState;
using VikingEngine.LF2.GameObjects.Characters.EnemyState.Boss;

namespace VikingEngine.LF2.Data.Characters
{
    //struct EmptyArg
    //{
    //    public static readonly StartNewEnemyStateArguments Arg = new StartNewEnemyStateArguments();
    //}
    //struct EnemyStateData
    //{
    //    static readonly RangeF WalkingSpeed = RangeF.FromRadius(0.008f, 0.002f);
    //    static readonly RangeF RunningSpeed = RangeF.FromRadius(0.012f, 0.003f);
    //    //static readonly StartNewEnemyStateArguments EmptyArg = EmptyArg.EmptyArg;
    //    EnemyStateType enemyStateType;
    //    public EnemyStateType Type
    //    {
    //        get { return enemyStateType; }
    //    }
    //    public RangeF Speed;
    //    public RangeF LifeTime;
    //    public RangeF TimeAfter;
    //    public bool Stop;
    //    public bool Aim;
    //    private AbsEnemyState stateClass; //Instace from this
    //    public bool IsNull
    //    { get { return stateClass == null; } }

    //    public void Init(EnemyStateType type)
    //    {
    //        this.enemyStateType = type;
    //        Speed = WalkingSpeed;
    //        switch (type)
    //        {
    //            case EnemyStateType.FreezeByHit:
    //                LifeTime = new RangeF(200, 250);
    //                stateClass = new FreezeByHit();
    //                break;
    //            case EnemyStateType.BullRush:
    //                Speed = RunningSpeed;
    //                LifeTime = new RangeF(800, 2000);
    //                stateClass = new BullRushState();
    //                break;
    //            case EnemyStateType.SmoothBullRush:
    //                Speed = RunningSpeed;
    //                LifeTime = new RangeF(1200, 2000);
    //                stateClass = new SmoothBullRushState();
    //                break;
    //            case EnemyStateType.Fire:
    //                LifeTime = new RangeF(0, 400);
    //                TimeAfter = new RangeF(0, 1000);
    //                Stop = Data.RandomSeed.Instance.BytePercent(160);
    //                Aim = Data.RandomSeed.Instance.BytePercent(160);
    //                stateClass = new FireState();
    //                break;
    //            case EnemyStateType.SprayFire:
    //                LifeTime = new RangeF(0, 400);
    //                TimeAfter = new RangeF(300, 800);
    //                Stop = Data.RandomSeed.Instance.BytePercent(160);
    //                //Aim = Data.RandomSeed.Instance.BytePercent(160);
    //                stateClass = new SprayFireState();
    //                break;
    //            case EnemyStateType.CirkleFire:
    //                LifeTime = new RangeF(0, 400);
    //                TimeAfter = new RangeF(200, 1000);
    //                //Stop = Data.RandomSeed.Instance.BytePercent(160);
    //                Aim = Data.RandomSeed.Instance.BytePercent(160);
    //                stateClass = new FireCirkleState();
    //                break;
    //            case EnemyStateType.Following:
    //                stateClass = new FollowingState();
    //                break;
    //            case EnemyStateType.Random90:
    //                LifeTime = new RangeF(500, 2000);
    //                stateClass = new Random90TurningState();
    //                break;
    //            case EnemyStateType.RandomTurning:
    //                stateClass = new RandomTurningState();

    //                break;
    //            case EnemyStateType.RandomWalking:
    //                LifeTime = new RangeF(800, 3000);
    //                stateClass = new RandomWalkingState();
    //                break;
    //            case EnemyStateType.TrapBackNforward:

    //                stateClass = new TrapBackNForward();
    //                break;
    //            case EnemyStateType.TrapCirkular:
    //                stateClass = new TrapCirkular();
    //                break;
    //            case EnemyStateType.TrapSlide:
    //                stateClass = new TrapSlide();
    //                break;
    //            case EnemyStateType.Waiting:
    //                LifeTime = new RangeF(1000, 3000);
    //                stateClass = new WaitingState();
    //                break;
    //            //case EnemyStateType.HumanoidSwordsMan:
    //            //    stateClass = new GameObjects.Characters.EnemyState.Humanoid.SwordsManState();
    //            //    break;
    //        }

    //    }
    //    public AbsEnemyState StartNewState(StartNewEnemyStateArguments arg)
    //    {
    //        arg.StateData = this;
    //        return stateClass.CreateInstance(arg);
    //    }
    //}
}
