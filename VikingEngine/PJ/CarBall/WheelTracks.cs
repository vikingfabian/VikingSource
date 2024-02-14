using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.CarBall
{
    class WheelTracks
    {
        static readonly Color Color = new Color(117, 87, 22, 50);

        CarImage car;
        Vector2 prevLeft, prevRight;
        float trackSize;
        IntervalF trackinterval;
        float nextInterval;

        public WheelTracks(CarImage car)
        {
            this.car = car;
            resetPosition();

            trackSize = cballRef.ballScale * 0.6f;
            trackinterval = new IntervalF(0.2f, 2f) * trackSize;
        }

        public void resetPosition()
        {

        }

        public void update()
        {
            Vector2 offset = new Vector2(car.carSize.X * 0.2f, car.carSize.X * -0.1f);
            Vector2 left = VectorExt.RotateVector(offset, car.rotation.radians) + car.car.position;
            offset.X *= -1f;
            Vector2 right = VectorExt.RotateVector(offset, car.rotation.radians) + car.car.position;

            wheelMoveCheck(ref prevLeft, left);
            wheelMoveCheck(ref prevRight, right);

        }

        void wheelMoveCheck(ref Vector2 prev, Vector2 current)
        {
            float l = (current - prev).Length();

            if (l >= nextInterval)
            {
                prev = current;
                nextInterval = Ref.rnd.Float(trackinterval.Min, trackinterval.Min * 1.5f);

                if (l < trackinterval.Max)
                {
                    Graphics.ParticleImage track = new Graphics.ParticleImage(SpriteName.cballTrack1,
                        current, new Vector2(trackSize), cballLib.LayerTrackMin, Vector2.Zero);
                    track.SetSpriteName(SpriteName.cballTrack1, Ref.rnd.Int(8));

                    track.Rotation = car.rotation.radians;
                    track.Color = Color;

                    track.particleData.setFadeout(600, 3000);
                }
            }
        }
    }
}
