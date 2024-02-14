using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest
{
    //class AttackRollSpeed
    //{
    //    static readonly int? DebugLevel = null;

    //    const float DefaultRollTime = 300;
    //    const float DefaultIntroTime = 100;

    //    const int Level2Count = 3;
    //    const int Level3Count = Level2Count + 4;

    //    const float Level2Speed = 0.7f;
    //    const float Level3Speed = 0.3f;

    //    const float Level2Intro = DefaultIntroTime * 0.7f;
    //    const float Level3Intro = DefaultIntroTime * 0.4f;

    //    int speedLevel = 1;
    //    int rollCount = 0;

    //    public void resultTimer(int defenderCount, 
    //        out float resultIntroTime, 
    //        out float damageExtendTime, 
    //        out float finalViewTime)
    //    {
    //        if (DebugLevel != null)
    //        {
    //            speedLevel = DebugLevel.Value;
    //        }

    //        switch (speedLevel)
    //        {
    //            default:
    //                resultIntroTime = 1200;
    //                damageExtendTime = 2000;
    //                finalViewTime = 1500;
    //                break;
    //            case 2:
    //                resultIntroTime = 1000;
    //                damageExtendTime = 1200;
    //                finalViewTime = 1200;
    //                break;
    //            case 3:
    //                resultIntroTime = 16;
    //                damageExtendTime = 600;
    //                finalViewTime = 1000;
    //                break;
    //        }

    //        if (defenderCount >= 5)
    //        {
    //            resultIntroTime *= 0.4f;
    //            damageExtendTime *= 0.4f;
    //        }
    //        else if (defenderCount >= 3)
    //        {
    //            //high speed
    //            resultIntroTime *= 0.7f;
    //            damageExtendTime *= 0.7f;
    //        }
    //    }

    //    public void rollTimer(int attackDice, 
    //        out bool simultaniousDefence, 
    //        out float introTime, 
    //        out float time)
    //    {
    //        const int MinCountAdjust = 3;
    //        const int MinTime = 30;

    //        if (DebugLevel != null)
    //        {
    //            speedLevel = DebugLevel.Value;
    //        }

    //        time = DefaultRollTime;

    //        if (attackDice > MinCountAdjust)
    //        {
    //            int over = attackDice - MinCountAdjust;
    //            time = Bound.Min(time - over * 30, MinTime);
    //        }

    //        float speed;

    //        switch (speedLevel)
    //        {
    //            default:
    //                introTime = DefaultIntroTime;
    //                speed = 1f;                    
    //                break;
    //            case 2:
    //                introTime = Level2Intro;
    //                speed = Level2Speed;
    //                break;
    //            case 3:
    //                introTime = Level3Intro;
    //                speed = Level3Speed;
    //                break;
    //        }

    //        time = Bound.Min(time * speed, MinTime);
    //        simultaniousDefence = true; 
    //    }

    //    public void onAttackRoll()
    //    {
    //        rollCount++;

    //        if (rollCount >= Level3Count)
    //        {
    //            speedLevel = 3;
    //        }
    //        else if (rollCount >= Level2Count)
    //        {
    //            speedLevel = 2;
    //        }
    //    }
    //}
}
