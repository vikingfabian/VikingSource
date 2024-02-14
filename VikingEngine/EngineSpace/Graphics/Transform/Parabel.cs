using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class Parabel2D : AbsUpdateable
    {
        Graphics.AbsDraw2D image;
        Vector2 speed;
        float gravity;
        Percent bounciness;
        int numBouncesLeft;
        float floorLevel;

        public Parabel2D(Graphics.AbsDraw2D image, bool toUpdate, Vector2 startSpeed, float gravity, Percent bounciness, int maxBounces)
            :base(toUpdate)
        {
            this.image = image;
            this.speed = startSpeed;
            this.gravity = gravity;
            this.bounciness = bounciness;
            this.numBouncesLeft = maxBounces;
            this.floorLevel = image.Ypos;
        }

        public override void Time_Update(float time)
        {
            if (image.IsDeleted) DeleteMe();

            speed.Y += gravity * time;
            image.Position += speed * time;
            if (image.Ypos > floorLevel)
            {
                image.Ypos = floorLevel;
                speed.Y = -speed.Y * bounciness.Value;
                if (--numBouncesLeft <= 0 || Math.Abs(speed.Y) < 0.001f)
                {
                    DeleteMe();
                }
            }
        }
    }

}
