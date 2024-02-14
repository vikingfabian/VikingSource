using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters.EnemyState
{
    //class TrapCirkular : AbsEnemyState
    //{
    //    float swingAngularSpeed = 0.002f;
    //    float rotationSpeed = 0.004f;
    //    Rotation1D swingPosision;
    //    Vector2 startPos;
    //    float swingRadius = 8;

    //    public TrapCirkular()
    //        : base() { }
    //    public TrapCirkular(Data.Characters.StartNewEnemyStateArguments arg)
    //        : base(arg)
    //    {
    //        startPos = enemyStateData.Parent.PlanePos;
    //    }
    //    public override void Time_Update(float time)
    //    {
    //        swingPosision.Radians += swingAngularSpeed * time;
    //        Vector2 pos = swingPosision.Direction(swingRadius);
    //        pos.X += startPos.X;
    //        pos.Y += startPos.Y;

    //        enemyStateData.Parent.PlanePos = pos;
    //        enemyStateData.Parent.RotateImage(rotationSpeed * time);
    //    }
    //    public override AbsEnemyState CreateInstance(Data.Characters.StartNewEnemyStateArguments arg)
    //    {
    //        return new TrapCirkular(arg);
    //    }
    //}

    //class TrapBackNForward : AbsEnemyState
    //{
    //    float swingAngularSpeed = 0.002f;
    //    Rotation1D swingPosision;
    //    float startPos;
    //    bool xDir = lib.RandomBool();
    //    float swingRadius = 8;

    //    public TrapBackNForward()
    //        : base() { }
    //    public TrapBackNForward(Data.Characters.StartNewEnemyStateArguments arg)
    //        : base(arg)
    //    {
    //        startPos = xDir ? enemyStateData.Parent.PlanePosX : enemyStateData.Parent.PlanePosY;
    //    }
    //    public override void Time_Update(float time)
    //    {
    //        swingPosision.Radians += swingAngularSpeed * time;
    //        float pos = swingPosision.Direction(swingRadius).X + startPos;
    //        if (xDir)
    //        { enemyStateData.Parent.PlanePosX = pos; }
    //        else
    //        { enemyStateData.Parent.PlanePosY = pos; }
    //    }
    //    public override AbsEnemyState CreateInstance(Data.Characters.StartNewEnemyStateArguments arg)
    //    {
    //        return new TrapBackNForward(arg);
    //    }
    //}

    //class TrapSlide : AbsEnemyState
    //{
    //    float attackSpeed = 0.01f;
    //    float reverseSpeed = 0.004f;
    //    const int HorizontalState = 1;
    //    int slideState = 0; //0=in center, 1=hori sliding, 2=verti
    //    float slideLength = 10;
    //    Vector2 startPos;

    //    public TrapSlide()
    //        : base() { }
    //    public TrapSlide(Data.Characters.StartNewEnemyStateArguments arg)
    //        : base(arg)
    //    {
    //        startPos = enemyStateData.Parent.PlanePos;
    //        //lifeTime = data.Time.GetRandom();
    //        //speed = data.Speed.GetRandom();
    //    }
    //    //public override int SetTarget(Hero hero)
    //    //{
    //    //    base.SetTarget(hero);
    //    //    //parent.Speed = lib.AngleToV2(parent.AngleDirToObject(target), speed);

    //    //}
    //    public override void Time_Update(float time)
    //    {
    //        if (target != null)
    //        {
    //            //ta reda på om hjälten är närmast i x eller yled
    //            Vector2 posDiff = enemyStateData.Parent.PositionDiff(target);
    //            if (posDiff.Length() < slideLength)
    //            {
    //                if (slideState == 0)
    //                {
    //                    slideState = (Math.Abs(posDiff.X) > Math.Abs(posDiff.Y)) ? HorizontalState : 2;
    //                }
    //                bool attack = false;
    //                const float AttackRadius = 2;
    //                if (slideState == HorizontalState)
    //                {
    //                    if (Math.Abs(posDiff.Y) < AttackRadius)
    //                    {//attack
    //                        attack = true;
    //                        enemyStateData.Parent.SpeedX = lib.FloatToDir(posDiff.X) * attackSpeed;
    //                    }

    //                }
    //                else
    //                {
    //                    if (Math.Abs(posDiff.X) < AttackRadius)
    //                    {//attack
    //                        attack = true;
    //                        enemyStateData.Parent.SpeedY = lib.FloatToDir(posDiff.Y) * attackSpeed;
    //                    }
    //                }
    //                if (!attack)
    //                {
    //                    slideState = 0;
    //                }
    //            }
    //            else
    //            {
    //                //target = null;
    //                //slideState = 0;
    //                DeleteMe();
    //            }
    //            if (slideState == 0)
    //            {//reverse action
    //                Vector2 pos = enemyStateData.Parent.PlanePos;
    //                const float SnapRadius = 0.3f;
    //                if (pos.X != startPos.X)
    //                {
    //                    float diff = pos.X - startPos.X;
    //                    if (Math.Abs( diff) < SnapRadius)
    //                    {
    //                        enemyStateData.Parent.PlanePos = startPos;
    //                        enemyStateData.Parent.Speed = Vector2.Zero;
    //                    }
    //                    else
    //                        enemyStateData.Parent.SpeedX = lib.FloatToDir(diff) * reverseSpeed;
    //                }
    //                else if (pos.Y != startPos.Y)
    //                {
    //                    float diff = pos.Y - startPos.Y;
    //                    if (Math.Abs(diff) < SnapRadius)
    //                    {
    //                        enemyStateData.Parent.PlanePos = startPos;
    //                        enemyStateData.Parent.Speed = Vector2.Zero;
    //                    }
    //                    else
    //                        enemyStateData.Parent.SpeedY = lib.FloatToDir(diff) * reverseSpeed;
    //                }
    //            }

    //            //dubbel checka speed
    //            //if (enemyStateData.Parent.SpeedX != 0 && enemyStateData.Parent.SpeedY != 0)
    //            //{
    //            //    if (enemyStateData.Parent.SpeedX > enemyStateData.Parent.SpeedY)
    //            //        enemyStateData.Parent.SpeedY = 0;
    //            //    else
    //            //        enemyStateData.Parent.SpeedX = 0;
    //            //}
    //        }
    //    }
    //    public override AbsEnemyState CreateInstance(Data.Characters.StartNewEnemyStateArguments arg)
    //    {
    //        return new TrapSlide(arg);
    //    }
    //}
    
}
