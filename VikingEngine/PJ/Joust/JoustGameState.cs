using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VikingEngine.Engine;

namespace VikingEngine.PJ.Joust
{
    class JoustGameState : AbsPJGameState
    {
        public static int MultiKillCount;
        protected State state = 0;
        
        protected List<Gamer> gamers;

        //int matchCount;
        
        int prevAliveCount = int.MinValue;
        protected int aliveCount;
        Time announceWinnerTime = new Time(3, TimeUnit.Seconds);
        Time levelTimeOut = new Time(2, TimeUnit.Minutes);
        Time timeoutCountDown = new Time(5, TimeUnit.Seconds);

        Display.CountDownNumber countDownNumber;

        Graphics.ImageGroup controllerIxImages = new Graphics.ImageGroup();

        Graphics.ImageGroup round;
        VikingEngine.PJ.Display.LeadingPlayerCrown leadingPlayerCrown = null;
        
        VikingEngine.PJ.Display.TimesUpDisplay timesUpDisplay = null;
        public List<PlatformService.AchievementSettings> endLevelAchievements = new List<PlatformService.AchievementSettings>();


        public JoustGameState(List2<GamerData> joinedGamers, int matchCount)
            : base(true)
        {
            JoustRef.gamestate = this;
            Ref.draw.ClrColor = PjLib.ClearColor;
            //Engine.Update.SetFrameRate(Ref.gamesett.FrameRate);
            this.matchCount = matchCount + 1;
            this.joinedLocalGamers = joinedGamers;

            PjRef.JoustSong.PlayStored();

            Gamer.Init();

            Engine.XGuide.UnjoinAll();

            new Level();
            gamers = new List<Gamer>();
            
            foreach (var gData in joinedLocalGamers)
            {
                Gamer gamer = new Gamer(gData);
                gamers.Add(gamer);

                gamer.pressButtonIcon.Xpos += gamer.pressButtonIcon.Width * 0.3f;

                if (gamer.gamerData.button.ControllerIndex >= 0)
                {
                    Graphics.Image controllerNumber = new Graphics.Image((SpriteName)((int)SpriteName.ControllerIconP1 + Bound.Set(gamer.gamerData.button.ControllerIndex, 0, 3)),
                        gamer.pressButtonIcon.Position, gamer.pressButtonIcon.Size, ImageLayers.Foreground5, true);
                    controllerNumber.Ypos -= controllerNumber.Height;

                    controllerIxImages.Add(controllerNumber);
                }
            }

            aliveCount = PjRef.storage.joinedGamersSetup.Count;
            prevAliveCount = aliveCount;

            countDownNumber = new Display.CountDownNumber();

            MultiKillCount = 3;
            if (gamers.Count > 5)
            {
                MultiKillCount = gamers.Count - 2;
            }

            //Round
            Graphics.Image roundFlag = new Graphics.Image(SpriteName.BirdRoundFlag, new Vector2(Engine.Screen.CenterScreen.X, Engine.Screen.Height * 0.7f),
                new Vector2(Engine.Screen.IconSize), ImageLayers.Foreground5, true);
            roundFlag.Xpos -= roundFlag.Width * 0.3f;
            Display.SpriteText roundNumber = new Display.SpriteText(this.matchCount.ToString(),
                roundFlag.RightTop + new Vector2(-roundFlag.Width * 0.4f, 0f), 
                roundFlag.Height * 0.8f, ImageLayers.Foreground5,
                new Vector2(0, 0.5f), Color.White, true);


            round = new Graphics.ImageGroup(roundFlag);
            roundNumber.AddTo(round.images);
            VikingEngine.PJ.GameObject.EggBallChick.ChickCount = 0;
            
            List<GamerData> order = GamerData.OrderPlayers(PjRef.storage.joinedGamersSetup);
            if (order[0].Score() > order[1].Score())
            {
                leadingPlayerCrown = new Display.LeadingPlayerCrown(order[0].gamer);
            }
        }

        protected override void createDrawManager()
        {
            draw = new Draw2D();
        }


        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (baseClassUpdate()) return;

            switch (state)
            {
                case State.CountDown:
                    updateCountDown();
                    levelTimeOut.CountDown();
                    break;

                case State.Playing:
                    JoustRef.level.Update();
                    aliveCount = 0;
                    for (int i = 0; i < gamers.Count; ++i)
                    {
                        gamers[i].update(gamers, i);

                        if (gamers[i].Alive)
                            aliveCount++;
                    }
                    checkWinningCondition();

                    if (levelTimeOut.CountDown())
                    {
                        if (timesUpDisplay == null)
                        {
                            timesUpDisplay = new Display.TimesUpDisplay(Convert.ToInt32(timeoutCountDown.Seconds));
                        }
                        else
                        {
                            timesUpDisplay.updateTimeLeft(Convert.ToInt32(timeoutCountDown.Seconds));
                        }

                        if (timeoutCountDown.CountDown())
                        {
                            winnerAnnouncedState();
                        }
                    }
                    break;
                case State.AnnounceWinner:
                    if (announceWinnerTime.CountDown())
                    {
                        nextState();
                    }
                    break;
            }            
        }        

        void updateCountDown()
        {
            for (int i = 0; i < gamers.Count; ++i)
            {
                gamers[i].updateCountDown();
            }

            if (countDownNumber.update())
            {
                //startSong();
                state++;
                
                controllerIxImages.DeleteAll();
                round.DeleteAll();
                for (int i = 0; i < gamers.Count; ++i)
                {
                    gamers[i].OnLevelStart();
                }

                if (leadingPlayerCrown != null)
                {
                    leadingPlayerCrown.endState = true;
                }
            }
        }

        protected void checkWinningCondition()
        {
            if (aliveCount <= 1)
            {
                //Only one alive
                int winnerIx = 0;
                for (int i = 0; i < gamers.Count; ++i)
                {
                    if (gamers[i].Alive)
                    {
                        winnerIx = i;
                        break;
                    }
                }

                Vector2 throphyPos;
                SpriteName throphyIcon;
                float scale = Joust.Gamer.ImageScale;
                if (aliveCount == 0)
                {
                    throphyPos = Engine.Screen.CenterScreen;
                    throphyIcon = SpriteName.BirdNoThrophy;
                    scale *= 2.2f;
                }
                else
                {
                    gamers[winnerIx].gamerData.Victories++;
                    throphyPos = gamers[winnerIx].Position;
                    throphyPos.Y -= Joust.Gamer.ImageScale * 0.2f;
                    throphyIcon = SpriteName.BirdThrophy;

                    new WinnerParticleEmitter(gamers[winnerIx].image);
                }

                Rectangle source = DataLib.SpriteCollection.Get(throphyIcon).Source;

                Graphics.Image winnerIcon = new Graphics.Image(throphyIcon, throphyPos, new Vector2(scale), ImageLayers.Top9, true);

                if (aliveCount == 0)
                {
                    new Graphics.Motion2d(Graphics.MotionType.SCALE, winnerIcon, winnerIcon.Size * 0.8f, Graphics.MotionRepeate.BackNForwardOnce, 99, true);
                }
                else
                {
                    winnerIcon.Opacity = 0f;

                    const float FadeTime = 200;
                    new Graphics.Motion2d(Graphics.MotionType.MOVE, winnerIcon, new Vector2(0, -Joust.Gamer.ImageScale * 0.8f), 
                        Graphics.MotionRepeate.NO_REPEAT, FadeTime, true);
                    new Graphics.Motion2d(Graphics.MotionType.OPACITY, winnerIcon, Vector2.One, 
                        Graphics.MotionRepeate.NO_REPEAT, FadeTime, true);
                }

                winnerAnnouncedState();

                SoundManager.PlaySound(LoadedSound.SmackEchoes);
            }
            else
            {
                if (prevAliveCount > aliveCount)
                {
                    prevAliveCount = aliveCount;
                    SoundManager.PlaySound(LoadedSound.smack);
                }
            }
        }

        void winnerAnnouncedState()
        {
            state = State.AnnounceWinner;

            Engine.Sound.StopMusic();

            foreach (var m in endLevelAchievements)
            {
                m.Unlock();
            }
        }

        void nextState()
        {
            if (matchCount >= JoustLib.MatchCount)
            {
                new FinalScoreState();
            }
            else
            {
                new JoustGameState(joinedLocalGamers, matchCount);
            }
        }
        

        protected enum State
        {
            CountDown,
            Playing,
            AnnounceWinner,
            Next,
        }
    }
}
