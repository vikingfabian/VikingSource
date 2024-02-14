using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.MiniGolf
{
    class LocalGamer
    {
        public GamerData gamerdata;
        public AnimalSetup animalSetup;
        Ball ball;
        AbsBallPower power = null;
        Club club = null;
        public Effect.Stunned stunned = null;
        float idleTimer = 0;

        protected Graphics.Image border, animal, controllerIcon;
        protected Graphics.ImageAdvanced button;
        Display.SpriteText scoreText;

        public int score = 0;

        public LocalGamer(GamerData gamerdata, VectorRect hudArea)
        {
            this.gamerdata = gamerdata;
            animalSetup = AnimalSetup.Get(gamerdata.joustAnimal);
            VectorRect iconArea = hudArea;
            iconArea.Width = iconArea.Height;
            iconArea.AddRadius(-iconArea.Height * 0.05f);

            LobbyAvatar.GamerAvatarFrameAndJoustAnimal(iconArea, GolfLib.BorderLayer, gamerdata,
               out border, out animal, out button, out controllerIcon);
            if (gamerdata.hat != Hat.NoHat)
            {
                new HatImage(gamerdata.hat, animal, animalSetup);
            }

            Vector2 pos = border.RightCenter;
            pos.X += Engine.Screen.BorderWidth;
            Graphics.Image coin = new Graphics.Image(SpriteName.birdCoin1, pos, 
                Engine.Screen.SmallIconSizeV2 * 0.9f, GolfLib.BorderLayer);
            coin.OrigoAtCenterHeight();

            pos.X += coin.Width * 1.1f;

            scoreText = new Display.SpriteText("", pos,
               Engine.Screen.SmallIconSize, GolfLib.BorderLayer, VectorExt.V2HalfY, Color.White, true);
            addScore(null, null, 0);
        }

        public void update(ref bool inCannon)
        {
            if (ball == null)
            {
                inCannon = true;

                if (gamerdata.button.DownEvent)
                {
                    ball = new Ball(this);
                    GolfRef.sounds.launchSound.Play(ball.image.position);
                    onFireBall();
                    Input.InputLib.Vibrate(gamerdata.button, 0.15f, 0.1f, 300);
                }
            }
            else
            {
                
                ball.update();

                if (stunned != null)
                {
                    updateStunn();
                }
                else if (club != null)
                {
                    updateClub();
                }
                else
                {
                    if (ball.isIdle)
                    {
                        updateIdle();
                    }
                    else
                    {
                        updateRolling();
                    }
                }

                if (PlatformSettings.DevBuild)
                {
                    updateDebug();
                }
            }
        }

        void updateStunn()
        {
            if (stunned.update())
            {
                stunned.DeleteMe();
                stunned = null;

                ball.onStunnEnd();
            }
        }

        void updateClub()
        {
            if (club.update(gamerdata.button))
            {
                addScore(null, null, -GolfLib.ClubStrikeCost);

                removeClub();
                onFireBall();
            }
        }

        public bool takeDamage()
        {
            if (stunned == null)
            {
                removeClub();
                stunned = new Effect.Stunned(ball);

                //var coinDirs = VectorExt.CircleOfDirections(GolfLib.DamageCost, 0, 1f);
                Rotation1D dir = Rotation1D.D45;
                for (int i = 0; i < GolfLib.DamageCost; ++i)
                {
                    new DropCoinAnimation(ball, dir, true);
                    dir.Add(MathExt.Tau / GolfLib.DamageCost);
                }

                addScore(null, null, -GolfLib.DamageCost);
                Input.InputLib.Vibrate(gamerdata.button, 0.3f, 0.2f, 300);

                return true;
            }

            return false;
        }


        void removeClub()
        {
            if (club != null)
            {
                club.DeleteMe();
                club = null;
            }
        }

        void onFireBall()
        {
            Input.InputLib.Vibrate(gamerdata.button, 0.2f, 0.1f, 300);
            ball.onFire();
            power = new BumpPower();
        }

        void updateIdle()
        {
            idleTimer += Ref.DeltaTimeSec;
            power = null;

            if (idleTimer >= 1f)
            {
                club = new Club(ball);
            }
        }

        void updateRolling()
        {
            idleTimer = 0;
            if (power != null && gamerdata.button.DownEvent)
            {
                if (power.activate(ball))
                {
                    power = null;
                }
            }
        }

        void updateDebug()
        {
            if (gamerdata.GamerIndex == 0)
            {
                if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    ball.image.Position = Input.Mouse.Position;
                    ball.velocity = Vector2.Zero;
                }
                if (Input.Mouse.ButtonDownEvent(MouseButton.Right))
                {
                    Vector2 diff = Input.Mouse.Position - ball.image.Position;
                    if (VectorExt.HasValue(diff))
                    {
                        diff.Normalize();

                        ball.addForce(diff * GolfRef.gamestate.ClubForce.Max, null);
                    }
                }
            }
        }

        public void update_asynch()
        {
            if (ball != null)
            {
                ball.update_asynch();
            }
        }

        public void addScore(Ball withBall, AbsItem scoreGivingItem, int value)
        {
            if (scoreGivingItem != null && value > 0)
            {
                new PjEngine.Effect.GainCoinsEffect(scoreGivingItem.bound.Area.CenterTop, value, GolfLib.PointEffectsLayer);
            }

            score += value;
            scoreText.Text(score.ToString());

            Color col;
            if (score < 0)
            {
                col = PjLib.CoinMinusColor;
            }
            else if (score == 0)
            {
                col = Color.Gray;
            }
            else
            {
                col = PjLib.CoinPlusColor;
            }

            scoreText.SetColor(col);
        }

        public void onEndScore()
        {
            gamerdata.coins += score;
        }
    }
}
