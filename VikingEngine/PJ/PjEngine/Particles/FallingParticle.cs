using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ
{
    class FallingParticle : AbsUpdateable
    {
        Graphics.Image image;
        public Vector2 velocity;
        float rotationSpeed;
        static int ParticleLayer = 0;
        float gravity;
        float airResistance;

        public FallingParticle(Vector2 pos, SpriteName tile, Color color, float scale, 
            float maxRotSpeed, float maxSpeedX, float gravity, float airResistance, ImageLayers layer = ImageLayers.Lay3)
            :base(true)
        {
            this.airResistance = airResistance;
            this.gravity = gravity;
            image = new Graphics.Image(tile, pos, new Vector2(scale), layer, true);
            image.Rotation = Ref.rnd.Rotation();
            image.Color = color;
            image.PaintLayer += ParticleLayer * PublicConstants.LayerMinDiff;
            if (ParticleLayer++ > 100)
            {
                ParticleLayer = 0;
            }

            rotationSpeed = Ref.rnd.Plus_MinusF(maxRotSpeed);
            velocity = new Vector2(Ref.rnd.Plus_MinusF(maxSpeedX), 0f);
 
        }

        public override void Time_Update(float time_ms)
        {
            velocity.Y += gravity;
            velocity *= airResistance;

            image.Position += velocity;
            image.Rotation += rotationSpeed;

            if (image.Ypos > Engine.Screen.Height * 1.2f)
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
