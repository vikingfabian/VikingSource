using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Display3D
{
    abstract class AbsUnitMessage
    {
        protected Time viewTime = new Time(1600);
        protected Graphics.RenderTargetBillboard model;
        protected Vector3 basePos;
        Vector3 goalPos;
        int updateCount = 0;
        protected AbsUnit unit;

        public AbsUnitMessage(AbsUnit unit)
        {
            this.unit = unit;    
        }

        virtual public void start()
        {
            basePos = toggRef.board.toWorldPos_adjustForUnitZ(unit.visualPos, 0f);
            basePos.Z += 0.05f;
            basePos.Y += 0.7f;
        }

        protected void completeInit(AbsUnit unit)
        {
            refreshGoalPos(0);
            Handler.add(unit, this);
        }

        public void refreshGoalPos(float stackHeight)
        {
            if (stackHeight > 0 && updateCount == 0)
            {
                model.Position = goalPos;
            }

            goalPos = basePos + toggLib.UpVector * (stackHeight + MessageHeight * 0.5f);
        }

        public void update()
        {
            ++updateCount;

            if (Ref.TimePassed16ms)
            {
                if (model.Y < goalPos.Y + 0.05f)
                {
                    model.Position += (goalPos - model.Position) * 0.3f;
                }
                else
                {
                    model.Position = goalPos;
                }
            }

            if (viewTime.CountDown())
            {
                model.Opacity -= 2f * Ref.DeltaTimeSec;
                if (model.Opacity <= 0)
                {
                    DeleteMe();
                }
            }
        }

        public void DeleteMe()
        {
            model.DeleteMe();
        }

        UnitMessagesHandler Handler => toggRef.gamestate.unitMessages;

        public bool TimedOut => model.Opacity <= 0;

        abstract public float MessageHeight { get; }
    }
}
