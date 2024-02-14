using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Match3
{
    class Gamer
    {
        public DeathAnimation deathAnimation = null;
        public BrickBox box;
        public GamerData gamerdata;
        public AnimalSetup animalSetup;
        public FallingBlock block = null;

        public HatImage hatimage = null;
        public Graphics.Image animal;
        Display.SpriteText scoreText;
        public int score = 0;
        PunishMeter punishMeter;
        public bool hasDroppedBlock = false;
        PointScoring mostRecentCombo = null;
        public bool alive = true;

        public int viewRotationInput = 4;

        public Gamer(GamerData gamerdata, BrickBox box)
        {
            this.box = box;
            box.gamer = this;
            this.gamerdata = gamerdata;
            animalSetup = AnimalSetup.Get(gamerdata.joustAnimal);

            punishMeter = new PunishMeter(box);

            animal = new Graphics.Image(animalSetup.wingUpSprite,
                box.area.LeftBottom, new Vector2(m3Ref.TileWidth * 2f), ImageLayers.Lay5, true);
            animal.position += animal.size * 0.5f;

            if (gamerdata.hat != Hat.NoHat)
            {
                hatimage = new HatImage(gamerdata.hat, animal, animalSetup);
                hatimage.update();
            }

            scoreText = new Display.SpriteText("0", VectorExt.AddX(animal.position, animal.Width * 0.6f), m3Ref.TileWidth * 1f, ImageLayers.Lay4,
                VectorExt.V2HalfY, Color.Yellow, true);

        }

        public void update()
        {
            deathAnimation?.update();
            if (alive)
            {
                if (PlatformSettings.DevBuild)
                {
                    if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.D1) &&
                        box.index == 1)
                    {
                        punishMeter.addPunish(1);
                    }
                    if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.D2))
                    {
                        box.debug_fill();
                    }
                }

                if (box.IsActive)
                {
                    box.update();
                }
                else if (block != null)
                {
                    if (block.update(this))
                    {
                        block.DeleteMe();
                        block = null;
                    }
                }
                else if (mostRecentCombo != null)
                {
                    punishMeter.removePunish(mostRecentCombo.combos + 1);

                    if (mostRecentCombo.combos > 0)
                    {
                        foreach (var m in m3Ref.gamestate.gamers)
                        {
                            if (m != this)
                            {
                                m.punishMeter.addPunish(mostRecentCombo.combos);
                            }
                        }

                        if (mostRecentCombo.combos >= 2)
                        {
                            PjRef.achievements.m3Combo3.Unlock();
                        }
                    }

                    box.comboCount = 0;
                    mostRecentCombo = null;
                }
                else if (punishMeter.HasPunishment)
                {
                    punishMeter.dropPunishBlocks(box);
                }
                else
                {
                    block = new FallingBlock(box);
                }

                if (gamerdata.button.DownEvent)
                {
                    animal.SetSpriteName(animalSetup.wingDownSprite);
                }
                else if (gamerdata.button.UpEvent)
                {
                    animal.SetSpriteName(animalSetup.wingUpSprite);
                }
            }
        }

        public void endGameUpdate()
        {
            deathAnimation?.update();
        }

        public void onBlockMiss()
        {
            punishMeter.addMiss();
        }

        public void onScore(PointScoring add)
        {
            score += add.totalScore;
            scoreText.Text(score.ToString());
            
            mostRecentCombo = add;
        }

        public void onDeath(Brick b)
        {
            alive = false;
            b.deathFlash();
            if (deathAnimation == null)
            {
                deathAnimation = new DeathAnimation(this);
            }

            m3Ref.gamestate.onGamerDeath();
        }

        
    }
}
