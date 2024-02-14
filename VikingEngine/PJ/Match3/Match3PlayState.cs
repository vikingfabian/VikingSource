using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using VikingEngine.Input;

namespace VikingEngine.PJ.Match3
{
    class Match3PlayState : AbsPJGameState
    {
        static readonly ObjectCommonessPair<BrickColor> Purple = new ObjectCommonessPair<BrickColor>(BrickColor.Purple, 25);

        RandomObjects<BrickColor> BrickCommoness = new RandomObjects<BrickColor>
        (
            new ObjectCommonessPair<BrickColor>(BrickColor.Blue, 100),
            new ObjectCommonessPair<BrickColor>(BrickColor.Green, 100),
            new ObjectCommonessPair<BrickColor>(BrickColor.Red, 100),
            new ObjectCommonessPair<BrickColor>(BrickColor.Yellow, 100),

            new ObjectCommonessPair<BrickColor>(BrickColor.Orange, 50)
            //new ObjectCommonessPair<BrickColor>(BrickColor.Purple, 25)
        );        

        LevelLayout layout;
        TimeManager timeManager;
        EndGameAnimation endAnimation = null;
        Display.MenuButton exitButton = null;
        public List<Gamer> gamers;
        public ushort seed;
        public bool blockMoveUpdate = false;
        float topSlideSpeed;
        float nextTopSlidePos = 0;
        public bool topMoveHappened;

        public float fallSpeedMultiplier;
        public int speedLevel = 0;
       
        public Match3PlayState(List2<GamerData> joinedGamers, Texture2D bgTexture)
            : base(true)
        {
            m3Ref.gamestate = this;

            seed = Ref.rnd.Ushort();
            this.joinedLocalGamers = joinedGamers;

            layout = new LevelLayout(joinedGamers.Count);

            gamers = new List<Gamer>(joinedGamers.Count);
            for (int i = 0; i < joinedGamers.Count; ++i)
            {
                gamers.Add(new Gamer(joinedGamers[i], layout.boxes[i]));
            }

            VectorRect bgArea = Engine.Screen.Area;
            bgArea.AddRadius(1f);
            Graphics.ImageAdvanced bg = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                Vector2.Zero, Vector2.One, ImageLayers.Bottom6, false);
            bg.Texture = bgTexture;
            bg.SetFullTextureSource();

            bg.fillAreaWithoutStreching(bgArea);

            Graphics.Image bgColOverlay = new Graphics.Image(SpriteName.WhiteArea,
                bgArea.Position, bgArea.Size, ImageLayers.Bottom6_Front);
            bgColOverlay.Color = Color.DarkBlue;
            bgColOverlay.Opacity = 0.6f;

            timeManager = new TimeManager();
            refreshSpeedLevel();

#if XBOX
            Ref.xbox.presence.Set("match3");
#endif
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            topMoveHappened = false;

            if (PlatformSettings.DevBuild)
            {
                if (Input.Keyboard.KeyDownEvent(Keys.D3))
                {
                    addSpeedLevel();
                }
            }

            if (exitButton != null)
            {
                if (HudLib.ExitEndScoreInput() ||
                    exitButton.update())
                {
                    this.exitToLobby();
                }
            }
            else if (baseClassUpdate())
            {
                return;
            }

            if (endAnimation != null)
            {
                endAnimation.update();

                foreach (var m in gamers)
                {
                    m.endGameUpdate();
                }
            }
            else
            {
                timeManager.Update();
                updateBlockMove();

                foreach (var m in gamers)
                {
                    m.update();
                }
            }

            if (topMoveHappened)
            {
                m3Ref.sounds.topMove.PlayFlat();
            }
        }

        public void onTimeUp()
        {
            PjRef.achievements.m3Timeout.Unlock();
            endAnimation = new EndGameAnimation(true);
        }

        public void onGamerDeath()
        {
            int aliveCount = 0;

            foreach (var m in gamers)
            {
                if (m.alive)
                {
                    aliveCount++;
                }
            }

            if (aliveCount <= 1)
            {
                endAnimation = new EndGameAnimation(false);
            }
        }
       
        public void createExitButton()
        {
            exitButton = Display.MenuButton.ExitToLobbyButton();

            Input.Mouse.Visible = true;
        }

        void updateBlockMove()
        {
            nextTopSlidePos += topSlideSpeed * Ref.DeltaGameTimeSec;
            if (nextTopSlidePos >= 1f)
            {
                nextTopSlidePos -= 1f;
                blockMoveUpdate = true;
            }
            else
            {
                blockMoveUpdate = false;
            }
        }

        public void addSpeedLevel()
        {
            if (speedLevel + 1 < m3Lib.SpeedUpLevels)
            {
                speedLevel++;

                timeManager.animateSpeedUp();
                refreshSpeedLevel();
            }
        }

        void refreshSpeedLevel()
        {
            switch (speedLevel)
            {
                case 0:
                    fallSpeedMultiplier = 1f;
                    topSlideSpeed = 2f;
                    break;

                case 1:
                    BrickCommoness.AddItem(Purple);
                    fallSpeedMultiplier = 1.3f;
                    topSlideSpeed = 3f;
                    break;

                case 2:
                    fallSpeedMultiplier = 1.5f;
                    topSlideSpeed = 4f;
                    break;

                default:
                    throw new System.Exception("Speed level error " + speedLevel.ToString());
            }
            
        }

        public BrickColor RndColor(PcgRandom rnd)
        {
            return BrickCommoness.GetRandom(rnd);
        }
    }
}
