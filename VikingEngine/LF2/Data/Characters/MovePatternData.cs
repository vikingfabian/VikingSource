using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using VikingEngine.LF2.GameObjects.Characters.EnemyState;

namespace VikingEngine.LF2.Data.Characters
{
    //abstract class AbsMovePatternData
    //{
    //    public bool UsesWeapon;
    //    abstract public bool SeeksHero { get; }
    //    virtual public float HeroSeekRadius { get { throw new NotImplementedException(); } }

    //    abstract public AbsEnemyState TakeDamage(StartNewEnemyStateArguments arg);
    //    abstract public AbsEnemyState FoundHero(StartNewEnemyStateArguments arg);
    //    abstract public AbsEnemyState Init(StartNewEnemyStateArguments arg);
    //    abstract public AbsEnemyState Next(StartNewEnemyStateArguments arg, ref int currentIx);

    //    protected static EnemyStateData GetStateData(EnemyStateType type)
    //    {
    //        EnemyStateData result = new EnemyStateData();
    //        result.Init(type);
    //        return result;
    //    }
    //}


    ///// <summary>
    ///// Information about the monsters reactions in different situations
    ///// </summary>
    //class MovePatternData : AbsMovePatternData
    //{
    //    static readonly List<EnemyStateType> BossStates = new List<EnemyStateType> {
    //        EnemyStateType.RandomWalking, EnemyStateType.RandomTurning, EnemyStateType.Random90, 
    //        EnemyStateType.SmoothBullRush, EnemyStateType.CirkleFire, EnemyStateType.SprayFire };
    //    static readonly List<EnemyStateType> IdleStates = new List<EnemyStateType> { 
    //        EnemyStateType.RandomWalking, EnemyStateType.Waiting, EnemyStateType.RandomTurning, EnemyStateType.Random90 };
    //    static readonly List<EnemyStateType> FoundTargetState = new List<EnemyStateType> { 
    //        EnemyStateType.BullRush, EnemyStateType.Following };
    //    static readonly RangeF SeekHero = new RangeF(14, 30);

    //    static readonly List<EnemyStateType> TrapTypes = new List<EnemyStateType> { 
    //        EnemyStateType.TrapSlide, EnemyStateType.TrapBackNforward, EnemyStateType.TrapCirkular };
    //    public float heroSeekRadius;
    //    public EnemyStateData damageAction;
    //    public EnemyStateData heroFoundAction;
    //    public List<EnemyStateData> actionList;
    //    static int numWeaponUsingEnemiesLeft = 2 + Ref.rnd.Int(2);
    //    bool seeksHero = false;
    //    public override bool SeeksHero
    //    {
    //        get { return seeksHero; }
    //    }
    //    //Targeting
    //    public MovePatternData(EnemyType type)
    //    {
    //        if (type == EnemyType.Trap)
    //        {
    //            UsesWeapon = false;

    //            switch (TrapTypes[Ref.rnd.Int(TrapTypes.Count)])
    //            {
    //                default: //case EnemyStateType.TrapSlide:
    //                    seeksHero = true;
    //                    actionList = new List<EnemyStateData> { GetStateData(EnemyStateType.Waiting) };
    //                    heroSeekRadius = 20;
    //                    heroFoundAction = GetStateData(EnemyStateType.TrapSlide);
    //                    break;
    //                case EnemyStateType.TrapBackNforward:
    //                    actionList = new List<EnemyStateData> { GetStateData(EnemyStateType.TrapBackNforward) };
    //                    break;
    //                case EnemyStateType.TrapCirkular:
    //                    actionList = new List<EnemyStateData> { GetStateData(EnemyStateType.TrapCirkular) };
    //                    break;
    //            }

    //        }
    //        else
    //        {
    //            damageAction = GetStateData(EnemyStateType.FreezeByHit);
    //            UsesWeapon = false;
    //            EnemyStateType idle = randomStateType(IdleStates);
    //            seeksHero = idle == EnemyStateType.Waiting ||
    //                type == EnemyType.MiddleBoss || type == EnemyType.FinalBoss || Data.RandomSeed.Instance.BytePercent(200);
    //            if (seeksHero)
    //            {
    //                heroSeekRadius = SeekHero.GetRandom();
    //                EnemyStateType found;
    //                if (numWeaponUsingEnemiesLeft > 0)
    //                {
    //                    UsesWeapon = true;
    //                    numWeaponUsingEnemiesLeft--;
    //                    found = EnemyStateType.Fire;
    //                }
    //                else
    //                {
    //                    found = randomStateType(FoundTargetState);

    //                }
    //                //EnemyStateType 
    //                // UsesWeapon = found == EnemyStateType.Fire; //här sätts om sen använder vapen
    //                heroFoundAction = GetStateData(found);
    //                if (UsesWeapon && idle == EnemyStateType.Waiting)
    //                {
    //                    ////need to aim
    //                    heroFoundAction.Aim = true;
    //                }
    //            }
    //            if (type == EnemyType.Level1)
    //            {
    //                actionList = new List<EnemyStateData> { GetStateData(idle) };

    //            }
    //            else
    //            {
    //                actionList = new List<EnemyStateData> { randomStateData(BossStates), 
    //                    randomStateData(BossStates), randomStateData(BossStates), randomStateData(BossStates) };
    //                foreach (EnemyStateData bstate in actionList)
    //                {
    //                    if (bstate.Type == EnemyStateType.CirkleFire || bstate.Type == EnemyStateType.SprayFire)
    //                    {
    //                        UsesWeapon = true;
    //                        break;
    //                    }
    //                }

    //            }

    //        }
    //    }
    //    public override AbsEnemyState Init(StartNewEnemyStateArguments arg)
    //    {
    //        return actionList[0].StartNewState(arg);
    //    }
    //    public override AbsEnemyState Next(StartNewEnemyStateArguments arg, ref int currentIx)
    //    {
    //        lib.SetIntMaxBoundsRollover(currentIx + 1, actionList.Count);
    //        return actionList[currentIx].StartNewState(arg);
    //    }
    //    public override AbsEnemyState TakeDamage(StartNewEnemyStateArguments arg)
    //    {
    //        return damageAction.StartNewState(arg);
    //    }
    //    public override AbsEnemyState FoundHero(StartNewEnemyStateArguments arg)
    //    {
    //        return heroFoundAction.StartNewState(arg);
    //    }
    //    public override float HeroSeekRadius
    //    {
    //        get
    //        {
    //            return heroSeekRadius;
    //        }
    //    }
    //    static EnemyStateData randomStateData(List<EnemyStateType> types)
    //    {
    //        return GetStateData(randomStateType(types));
    //    }
    //    static EnemyStateType randomStateType(List<EnemyStateType> types)
    //    {
    //        return types[Data.RandomSeed.Instance.Next(types.Count)];
    //    }

        
    //}


    //struct StartNewEnemyStateArguments
    //{
    //    public Game1.LootFest.GameObjects.Characters.Monster Parent;
    //    public AbsMovePatternData MovepatternData;
    //    public Data.Characters.EnemyStateData StateData;
    //    public bool Empty
    //    { get { return Parent == null; } }

    //    public StartNewEnemyStateArguments(Game1.LootFest.GameObjects.Characters.Monster parent,
    //        AbsMovePatternData movepatternData, Data.Characters.EnemyStateData stateData)
    //    {
    //        Parent = parent;
    //        MovepatternData = movepatternData;
    //        StateData = stateData;
    //    }
    //}
   
    //enum EnemyStateType
    //{
    //    RandomWalking,
    //    Waiting,
    //    FreezeByHit,
    //    Following,
    //    Fire,
    //    BullRush,
    //    TrapSlide,
    //    TrapBackNforward,
    //    TrapCirkular,
    //    Random90,
    //    RandomTurning,
    //    //bosses only
    //    SmoothBullRush,
    //    CirkleFire,
    //    SprayFire,
    //   // HumanoidSwordsMan,
    //}
}
