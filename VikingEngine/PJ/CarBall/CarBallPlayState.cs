using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.CarBall
{
    /*
     * Tre fill pluppar -> slump vapen
     *  -Missiler, miner, tagg-aura
     * 
     */

    class CarBallPlayState : AbsPJGameState
    {
        public Field field;
        public List2<Gamer> gamers;
        Ball ball;
        
        bool forwardCarUpdate = true;
        int state_0intro_1play_2ending = 0;
        Time slowmoTimer = 0;
        BallRespawn ballRespawn;
        Display.CountDownNumber countDownNumber;

        public CarBallPlayState(List2<GamerData> joinedGamers, Texture2D fieldTexture)
            : base(true)
        {
            cballRef.state = this;
            field = new Field(fieldTexture);
            float test = Engine.Screen.IconSize;

            this.joinedLocalGamers = joinedGamers;
            GamerData.SetLeftRightTeams(joinedGamers);
            gamers = new List2<Gamer>(joinedGamers.Count);
            
            for (int i = 0; i < joinedGamers.Count; ++i)
            {
                Gamer g = new Gamer(joinedGamers[i]);
                gamers.Add(g);
            }
            ball = new Ball();

            field.initGoals();
            countDownNumber = new Display.CountDownNumber();

#if XBOX
            Ref.xbox.presence.Set("carball");
#endif
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (baseClassUpdate()) return;

            switch (state_0intro_1play_2ending)
            {
                case 0:
                    updateIntro();
                    break;
                case 1:
                    updatePlay();
                    if (!ball.image.Visible)
                    {
                        updateGoalReset();
                    }
                    break;
                case 2: updateEnding(); break;
            }

            if (PlatformSettings.DevBuild)
            {
                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.D1))
                {
                    //onGoal(field.rightField);
                    debugDestroyAllCars();
                }

                if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    ball.image.position = Input.Mouse.Position;
                }
            }
        }

        void debugDestroyAllCars()
        {
            foreach (var m in gamers)
            {
                m.car.takeHit();
            }
        }

        void updateIntro()
        {
            gamers.loopBegin();
            while (gamers.loopNext())
            {
                gamers.sel.updateIntro();
            }

            if (countDownNumber.update())
            {
                gamers.loopBegin();
                while (gamers.loopNext())
                {
                    gamers.sel.car.onPlayStart();//.car.resetSize();
                }
                state_0intro_1play_2ending++;
            }
        }

        void updatePlay()
        {
            for (int updatePart = 0; updatePart < cballLib.CollisionUpdatesPerFrame; ++updatePart)
            {
                gamers.loopBegin(forwardCarUpdate);
                while (gamers.loopNext())
                {
                    gamers.sel.update(gamers.selIndex, updatePart);
                }
                
                lib.Invert(ref forwardCarUpdate);

                ball.update(updatePart);
            }
        }

        Time endCountDown = new Time(6, TimeUnit.Seconds);
        void updateEnding()
        {
            if (endCountDown.CountDown())
            {
                new ExitToLobbyState();
            }
        }

        void updateGoalReset()
        {
            if (ballRespawn != null)
            {
                if (ballRespawn.update())
                {
                    ballRespawn = null;
                }
            }
            else
            {
                const float SloMoSpeed = 0.1f;
                float sloMoAdd = 10f * Ref.DeltaTimeSec;

                if (slowmoTimer.HasTime)
                {
                    slowmoTimer.CountDown();
                    Ref.GameTimeSpeed = Bound.Min(Ref.GameTimeSpeed - sloMoAdd, SloMoSpeed);
                }
                else
                {
                    Ref.GameTimeSpeed += sloMoAdd;
                    if (Ref.GameTimeSpeed >= 1f)
                    {
                        Ref.GameTimeSpeed = 1f;
                        ballRespawn = new BallRespawn(ball);
                    }
                }
            }
        }

        public void onGoal(FieldHalf field)
        {
            field.opposite.scorePoint();
            new GoalFieldBeemEffect(field.leftHalf);
            new BounceEffect(field.goalImg);
            new BallExplosionEffect(ball.image);

            slowmoTimer.Seconds = 0.4f;
            ball.onGoal();

            foreach (var m in gamers)
            {
                Input.InputLib.Vibrate(m.gamerdata.button, 0.8f, 0.8f, 500);
            }
        }

        public void onFinalGoal(bool leftTeamWinner)
        {
            state_0intro_1play_2ending = 2;

            cballRef.sounds.winner.PlayFlat();

            foreach (var m in gamers)
            {
                if (m.leftTeam == leftTeamWinner)
                {
                    m.car.respawnIfDestroyed();

                    new WinnerParticleEmitter(m.car.image.car);

                    float scale = Engine.Screen.IconSize * 1.5f;
                    Graphics.Image winnerIcon = new Graphics.Image(SpriteName.BirdThrophy, 
                       VectorExt.AddY(m.Center(), -scale * 0.25f), new Vector2(scale), ImageLayers.AbsoluteTopLayer, true);

                    winnerIcon.Opacity = 0f;

                    const float FadeTime = 200;
                    new Graphics.Motion2d(Graphics.MotionType.MOVE, winnerIcon, new Vector2(0, -scale * 1.0f),
                        Graphics.MotionRepeate.NO_REPEAT, FadeTime, true);
                    new Graphics.Motion2d(Graphics.MotionType.OPACITY, winnerIcon, Vector2.One,
                        Graphics.MotionRepeate.NO_REPEAT, FadeTime, true);

                    if (m.goals > 0)
                    {
                        Vector2 goalCountCenter = VectorExt.AddY(m.Center(), -scale * 0.4f);
                        float goalCountScale = Engine.Screen.IconSize * 0.5f;

                        for (int i = 0; i < m.goals; ++i)
                        {
                            var area = Table.CellPlacement(goalCountCenter, true, i, m.goals, new Vector2(goalCountScale), Vector2.Zero);
                            Graphics.Image goalCountIcon = new Graphics.Image(SpriteName.cballBall, area.Position, area.Size, ImageLayers.Top0);
                        }
                    }
                }
            }
            
        }
    }

    
}
