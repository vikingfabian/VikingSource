using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Physics
{
    struct Velocity2D
    {
        public Vector2 value;

        public Velocity2D(Vector2 startSpeed, LeftRight leftRight)
        {
            value.X = startSpeed.X * leftRight;
            value.Y = startSpeed.Y;
        }

        public void SetZero()
        {
            value = Vector2.Zero;
        }

        public void accelerateY(float acc, float maxSpeed)
        {
            maxSpeed = Math.Abs(maxSpeed);
            value.Y += acc;

            if (acc < 0)
            {
                if (value.Y < -maxSpeed)
                {
                    value.Y = -maxSpeed;
                }
            }
            else
            {
                if (value.Y > maxSpeed)
                {
                    value.Y = maxSpeed;
                }
            }
        }

        public void set(Rotation1D angle, LeftRight leftRight, float speed)
        {
            value = leftRight.applyToX(angle.Direction(speed));
        }

        public void setX(float speed, LeftRight leftRight)
        {
            value.X = speed * leftRight;
        }

        public void setXDir(LeftRight leftRight)
        {
            value = leftRight.applyToX(value);
        }

        public bool HasValue
        {
            get { return value.X != 0 || value.Y != 0; }
        }

        public Vector2 moveDistance(int divideLength = 1)
        {
            return Ref.DeltaGameTimeMs / divideLength * value;
        }

        public void flipX(float scale = 1f)
        {
            value.X = -value.X * Math.Abs(scale);
        }

        public void flipY(float scale = 1f)
        {
            value.Y = -value.Y * Math.Abs(scale);
        }
    }
}
