using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Match3
{
    class DeathAnimation
    {
        Gamer gamer;
        int row;
        Timer.Basic nextRowTimer = new Timer.Basic(120, true);
        int state_0rows_1deathanim = 0;

        public DeathAnimation(Gamer gamer)
        {
            this.gamer = gamer;
        }

        public void update()
        {
            switch (state_0rows_1deathanim)
            {
                case 0:
                    if (nextRowTimer.Update())
                    {
                        grayBrickRow();
                        
                        if (++row >= BrickBox.BrickCountSz.Y)
                        {
                            state_0rows_1deathanim++;
                        }
                    }
                    break;
                case 1:
                    fallingImage(gamer.animal, gamer.animalSetup.deadSprite);
                    state_0rows_1deathanim++;
                    break;
            }
        }

        void fallingImage(Graphics.Image original, SpriteName sprite)
        {
            Graphics.ParticleImage fallingImg = new Graphics.ParticleImage(sprite,
                        original.position, original.size, ImageLayers.AbsoluteBottomLayer,
                        Vector2.Zero);
            fallingImg.LayerAbove(original);
            original.Visible = false;

            Rotation1D dir = Rotation1D.D0;
            dir.Add(Ref.rnd.Plus_MinusF(1.2f));
            fallingImg.particleData.Velocity2D = dir.Direction(Ref.rnd.Plus_MinusPercent(m3Ref.ParticleSpeed * 1.6f, 0.2f));
            fallingImg.particleData.setFalling(m3Ref.Gravity);
            fallingImg.particleData.setRoundsPerSecond(lib.ToLeftRight(fallingImg.particleData.velocity.X) * Ref.rnd.Float(5f));
            fallingImg.particleData.setLifeTime(2000);
        }

        void grayBrickRow()
        {
            IntervalF pitch = new IntervalF(0.5f, -0.5f);
            float percPos = (float)row / BrickBox.BrickCountSz.Y;
            m3Ref.sounds.bassExplosion.pitchAdd = pitch.GetFromPercent(percPos);
            m3Ref.sounds.bassExplosion.Play(gamer.box.area.Center);

            Input.InputLib.Vibrate(gamer.gamerdata.button, 0.6f * percPos, 1f - (0.6f * percPos), 200);

            IntVector2 pos = new IntVector2(0, row);
            for (pos.X = 0; pos.X < BrickBox.BrickCountSz.X; ++pos.X)
            {
                var b = gamer.box.grid.Get(pos);
                if (b != null)
                {
                    b.turnGray();
                    
                    int count = Ref.rnd.Int(4);
                    for (int i = 0; i < count; ++i)
                    {
                        Graphics.ParticleImage p = new Graphics.ParticleImage(SpriteName.m3BrickStone,
                            b.images.images[0].image.position,
                            new Vector2(m3Ref.TileWidth * 0.3f), m3Lib.LayerParticle,
                            Ref.rnd.vector2_cirkle(Ref.rnd.Plus_MinusPercent(m3Ref.ParticleSpeed, 0.1f)));

                        p.particleData.setRoundsPerSecond(lib.ToLeftRight(p.particleData.velocity.X) * Ref.rnd.Float(50f));

                        p.particleData.setFalling(m3Ref.Gravity);
                        p.particleData.setFadeout(200, 90);
                    }
                }
            }
        }
    }
}
