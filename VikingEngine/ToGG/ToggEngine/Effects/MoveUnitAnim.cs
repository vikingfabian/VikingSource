using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Effects
{
    abstract class AbsMoveUnitAnim : AbsUpdateable
    {
        protected const float TilePerSecSpeed = 4f;

        public AbsUnit unit;
        public Action onComplete;
        protected Vector3 speed;
        protected Time time;

        public AbsMoveUnitAnim(AbsUnit unit, Action onComplete, bool addToUpdate)
            :base(addToUpdate)
        {
            this.unit = unit;
            this.onComplete = onComplete;
        }

        virtual protected void moveComplete()
        {
            onComplete?.Invoke();
            this.DeleteMe();    
        }
    }

    class MoveUnitAnim : AbsMoveUnitAnim
    {   
        IntVector2 toSquare;
        public MoveUnitAnim(AbsUnit unit, IntVector2 toSquare, Action onComplete, bool addToUpdate = true)
            : base(unit, onComplete, addToUpdate)
        {
            this.toSquare = toSquare;
            Vector3 goal = toggRef.board.toWorldPos_Center(toSquare, 0);

            Vector3 diff = goal - unit.soldierModel.Position;
            float l;
            speed = VectorExt.Normalize(diff, out l);

            speed *= TilePerSecSpeed;

            time = new Time(1f / TilePerSecSpeed * l, TimeUnit.Seconds);
        }

        public override void Time_Update(float time_ms)
        {
            unit.soldierModel.Position += speed * Ref.DeltaTimeSec;

            if (time.CountDown())
            {
                unit.SetPosition(toSquare);
                moveComplete();
            }
        }
    }

    class BounceUnitAnim : AbsMoveUnitAnim
    {
        float setTime;
        bool outWards = true;

        public BounceUnitAnim(AbsUnit unit, IntVector2 towardsSquare, float bounceLength, 
            Action onComplete = null, bool addToUpdate = true)
           : base(unit, onComplete, addToUpdate)
        {
            if (towardsSquare.IsZero() || towardsSquare == unit.visualPos)
            {
                towardsSquare = unit.squarePos + new IntVector2(-1, 1);
            }

            Vector3 goal = toggRef.board.toWorldPos_Center(towardsSquare, 0);

            Vector3 diff = goal - unit.soldierModel.Position;
            float l;
            diff = VectorExt.Normalize(diff, out l);

            speed = diff * TilePerSecSpeed;

            setTime = 1f / TilePerSecSpeed * bounceLength;
            time = new Time(setTime, TimeUnit.Seconds);
        }

        public override void Time_Update(float time_ms)
        {
            unit.soldierModel.Position += speed * Ref.DeltaTimeSec;

            if (time.CountDown())
            {
                if (outWards)
                {
                    time = new Time(setTime, TimeUnit.Seconds);
                    speed = -speed;
                    outWards = false;
                }
                else
                {
                    moveComplete();
                }
            }
        }
    }
}
