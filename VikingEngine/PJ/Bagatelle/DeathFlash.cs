using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Bagatelle
{
    class DeathFlash : AbsUpdateable
    {
        Graphics.Image image;
        const SpriteName StartFrame = SpriteName.bagBallExitFire1;
        SpriteName tile = StartFrame;
        Timer.Basic frameTime = new Timer.Basic(30, true);
        //int lifeTime = 0;
        VectorRect area;
        BagatellePlayState state;

        public DeathFlash(Ball ball, BagatellePlayState state)
            : base(true)
        {
            this.state = state;
            area = new VectorRect(new Vector2(ball.image.Xpos, state.activeScreenArea.Bottom), new Vector2(2, 3) * state.BallScale * 1.4f);
            area.X -= area.Width * 0.5f;
            area.Y -= area.Height - 1;

            image = new Graphics.Image(StartFrame, area.Position, area.Size, ImageLayers.Background4, false);
            //image.Opacity = 0.7f;

            area.AddPercentRadius(-0.3f);
        }

        public override void Time_Update(float time_ms)
        {
            for (int i = 0; i < 2; ++i)
            {
                new DeathFlashParticle(area.RandomPos(), state);
            }

            if (frameTime.Update())
            {
                tile++;
                if (tile > SpriteName.bagBallExitFire6)
                {
                    DeleteMe();
                }
                else
                {
                    image.SetSpriteName(tile);
                }
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            image.DeleteMe();
        }

        class DeathFlashParticle : Graphics.AbsUpdateableImage
        {
            int frameCount = 0;
            SpriteName startTile;
            Timer.Basic frameTime = new Timer.Basic(30, true);
            float speedY;

            public DeathFlashParticle(Vector2 startPos, BagatellePlayState state)
                :base(SpriteName.NO_IMAGE, startPos, new Vector2(state.BallScale * Ref.rnd.Float(0.4f, 1.6f)), ImageLayers.Background5, true, true, true)
            {
                speedY = -state.BallScale * Ref.rnd.Float(0.01f, 0.015f);
                switch (Ref.rnd.Int(3))
                {
                    default:
                        startTile = SpriteName.bagBallExitFireParticleA1;
                        break;
                    case 1:
                        startTile = SpriteName.bagBallExitFireParticleB1;
                        break;
                    case 2:
                        startTile = SpriteName.bagBallExitFireParticleC1;
                        break;

                }

                SetSpriteName(startTile);
            }

            public override void Time_Update(float time_ms)
            {
                Ypos += speedY * time_ms;

                if (frameTime.Update())
                {
                    frameCount++;
                    if (frameCount >= 3)
                    {
                        DeleteMe();
                    }
                    else
                    {
                        SetSpriteName(startTile, frameCount);
                    }
                }
            }
        }
    }


}
