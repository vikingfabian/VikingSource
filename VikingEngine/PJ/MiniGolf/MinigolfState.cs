using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf
{
    class MinigolfState : AbsPJGameState
    {
        public float HoleScale;
        public float BallScale;
        public float FitInHoleRadius;
        public float CoinScale;
        public float MaxClubForce;
        public float MaxSpeed;

        public IntervalF ClubForce;
        public float LaunchForce;
        public float MinBumpForce;

        public float FieldEdgeThickness;
        public float FieldEdgeOutlineWidth;


        public float MinSpeed;
        public float LowSpeed;
        public float HoleDropSpeed;

        public Rotation1D prevClubAngle;
        public Rotation1D clubAngle;
        List<LocalGamer> gamers;
        GamePhase phase = GamePhase.Loading;
        public SideBorder sideborder;
        Time endTimer = new Time(4, TimeUnit.Seconds);
        Time successSound = new Time(0.5f, TimeUnit.Seconds);

        Graphics.Motion2d fadeEffect;

        int flyCountsCount;
        Time nextFlyCoins;
        //Timer.Basic testFlyCoins = new Timer.Basic(10, false);

        public MinigolfState(List2<GamerData> joinedGamers, int matchCount)
            : base(true)
        {
            this.joinedLocalGamers = joinedGamers;

            if (PlatformSettings.PlayMusic)
            {
                Ref.music.nextRandomSong();
            }
            fadeEffect = PjLib.BlackFade(true, false);

            GolfRef.gamestate = this;
            new GO.ObjectCollection();
            new Field(matchCount);
            sideborder = new SideBorder(GolfRef.field.outerBound, Engine.Screen.SafeArea);

            BallScale = GolfRef.field.squareSize.X * 1.4f;
            FitInHoleRadius = BallScale * 0.05f;
            CoinScale = BallScale * 0.44f;
            CoinTailPart.CoinsSpacing = CoinScale * 1.5f;
            HoleScale = BallScale * 1.4f;
            FieldEdgeThickness = (int)BallScale * 0.2f;
            FieldEdgeOutlineWidth = (int)(FieldEdgeThickness * 0.15f);

            MaxClubForce = 16f * BallScale;
            LowSpeed = 0.16f * MaxClubForce;
            MinSpeed = MaxClubForce * 0.01f;
            HoleDropSpeed = MaxClubForce * 0.5f;
            ClubForce = new IntervalF(0.07f, 1f) * MaxClubForce;
            LaunchForce = 0.3f * MaxClubForce;
            MinBumpForce = 0.1f * MaxClubForce;

            MaxSpeed = MaxClubForce * 1.2f;

            gamers = new List<LocalGamer>(joinedGamers.Count);
            for (int i = 0; i < joinedGamers.Count; ++i)
            {
                var gamer = new LocalGamer(joinedGamers[i], sideborder.getPlayerHudArea(i));
                gamers.Add(gamer);
            }

            GolfRef.field.storage.loadLevel(GolfRef.NextRandomLevel());
            ((Draw)draw).initShadow();

            if (matchCount == 0)
            {
                flyCountsCount = 0;
            }
            else
            {
                flyCountsCount = Ref.rnd.Int(0, 4);
            }
            nextFlyCoins = new Time(Ref.rnd.Float(0.5f, 5f), TimeUnit.Seconds);

            this.matchCount = matchCount + 1;
        }

        public void onMapLoaded()
        {
            fadeEffect.AddToUpdateList();

            if (phase == GamePhase.Loading)
            {
                phase = GamePhase.Play;
                new AsynchUpdateable(update_asynch, "MiniGolf asynch update", 0);
            }

            if (GolfRef.editor != null)
            {
                GolfRef.editor.onMapLoaded();
            }
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (GolfRef.editor == null)
            {
                if (baseClassUpdate()) return;
            }

            switch (phase)
            {
                case GamePhase.Play:
                    toggleEditorInput();

                    if (GolfRef.editor != null)
                    {
                        GolfRef.editor.update();
                    }
                    else
                    {                        
                        updatePlay();
                    }
                    break;

                case GamePhase.Ending:
                    if (successSound.CountDown_IfActive())
                    {
                        GolfRef.sounds.success.PlayFlat();
                    }

                    if (endTimer.CountDown())
                    {
                        if (matchCount >= GolfLib.MatchCount)
                        {
                            new FinalScoreState();
                        }
                        else
                        {
                            new MinigolfState(joinedLocalGamers, matchCount);
                        }
                    }
                    break;
            }

            Ref.music.Update();

            if (PlatformSettings.DevBuild && Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                Ref.music.nextRandomSong();
                Debug.Log(Ref.music.nextSongData.name);
            }
        }

        void toggleEditorInput()
        {
            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Tab))
            {
                if (GolfRef.editor == null)
                {
                    new Editor();
                    if (GolfRef.objects.cannon != null)
                    {
                        GolfRef.objects.cannon.image.Visible = false;
                    }
                }
                else
                {
                    GolfRef.editor.DeleteMe();
                    GolfRef.editor = null;
                }
            }
        }

        bool update_asynch(int id, float time)
        {
            foreach (var m in gamers)
            {
                m.update_asynch();
            }

            GolfRef.objects.update_asynch();

            return false;
        }

        public void onHoleDrop()
        {
            Ref.music.stop(true);
            phase = GamePhase.Ending;

            foreach (var m in gamers)
            {
                m.onEndScore();
            }
        }



        void updatePlay()
        {
            if (PlatformSettings.DevBuild)
            {
                //if (testFlyCoins.Update())
                //{
                //    new FlyingCoins();
                //}

                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.D1))
                {
                    GolfRef.objects.debugHole();
                }

                if (Input.Keyboard.KeyUpEvent(Microsoft.Xna.Framework.Input.Keys.PageDown))
                {
                    onHoleDrop();
                }
            }

            if (GolfRef.field.storage.isLoaded)
            {
                prevClubAngle = clubAngle;
                clubAngle.Add(Ref.DeltaTimeSec * GolfLib.ClubAngleSpeed);

                bool inCannon = false;
                foreach (var m in gamers)
                {
                    m.update(ref inCannon);
                }
                
                GolfRef.objects.update(inCannon);

                updateFlyCoinsSpawn();
                
            }
        }

        void updateFlyCoinsSpawn()
        {
            if (flyCountsCount > 0)
            {
                if (nextFlyCoins.CountDown())
                {
                    new FlyingCoins();

                    flyCountsCount--;
                    nextFlyCoins = new Time(Ref.rnd.Float(5f, 20f), TimeUnit.Seconds);
                }
            }
        }

        protected override void createDrawManager()
        {
            draw = new Draw();
        }

        enum GamePhase
        {
            Loading,
            Countdown,
            Play,
            Ending,
        }
    }
}
