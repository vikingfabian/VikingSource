using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.GameObject
{
    class FlyingHat :AbsUpdateable
    {
        Graphics.Image image;
        Vector2 velocity;
        float rotationSpeed;
        float gravity;

        public FlyingHat(HatImage original, int facingDir, float gravity)
            : base(true)
        {
            image = original.image.CloneMe() as Graphics.Image;

            velocity.X = Ref.rnd.Float(0.2f, 0.7f) * facingDir;
            velocity.Y = Ref.rnd.Float(-0.7f, -0.3f);
            this.gravity = gravity * 0.8f;

            rotationSpeed = Ref.rnd.Plus_MinusF(0.05f);
        }

        public override void Time_Update(float time_ms)
        {
            velocity.Y += gravity; //* Ref.DeltaTimeMs;

            image.Position += velocity * Ref.DeltaTimeMs;

            image.Rotation += rotationSpeed * Ref.DeltaTimeMs;

            if (image.Ypos - image.Height > Engine.Screen.Height)
            {
                this.DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            image.DeleteMe();
        }
    }
}
