using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Bagatelle
{
    class BagatelleTutorialScreen: AbsPJGameState
    {
        static int ViewTimes = 0;
        Texture2D bgTexture;
        Graphics.Image cannon, ball, bumpCount, buttonPress;

        public BagatelleTutorialScreen(List2<GamerData> joinedGamers)
            :base(false)
        {
            if (PjRef.host)
            {
                Ref.netSession.BeginWritingPacket(Network.PacketType.birdBeginLoadScreen, Network.PacketReliability.Reliable);
            }
            Input.Mouse.Visible = false;

            this.joinedLocalGamers = joinedGamers;
           

            
           

            const float MusicVol = 0.6f;

            Microsoft.Xna.Framework.Media.MediaPlayer.Stop();
            Ref.music.SetPlaylist(new List<Sound.SongData>
                {
                    new Sound.SongData(PjLib.MusicFolder + "AgriculturalHuts", "Agricultural Huts", true, MusicVol),
                    new Sound.SongData(PjLib.MusicFolder + "BunnyMassacre", "Bunny Massacre", true, MusicVol),
                    new Sound.SongData(PjLib.MusicFolder + "Endless Plains", "Endless Plains", true, MusicVol),
                    new Sound.SongData(PjLib.MusicFolder + "Mysterious Grotto","Mysterious Grotto", false, 1.6f * MusicVol),
                }, false);

            if (ViewTimes == 0)
            {
                new PjEngine.Display.BlueprintTexture();
                tutorial();
            }
            else
            {
                Ref.draw.ClrColor = Color.Black;
            }


            new Timer.AsynchActionTrigger(loadContent_Asynch, true);

            PjLib.NetWriteJoinedGamers(joinedGamers);

#if XBOX
            Ref.xbox.presence.Set("bagatelle");
#endif
        }

        void tutorial()
        {
            cannon = new Graphics.Image(SpriteName.bagCannon, Engine.Screen.Area.PercentToPosition(new Vector2(0.5f, 0.2f)),
                Engine.Screen.IconSize * 1f * new Vector2(2f, 3f), ImageLayers.Lay4, true);
            buttonPress = new Graphics.Image(SpriteName.birdButtonPressUp,
                cannon.Position + VectorExt.V2FromX(Engine.Screen.IconSize * 5f),
                new Vector2(Engine.Screen.IconSize * 2f), ImageLayers.Lay3, true);

            const float FireTime = 500;
            animateButtonPress(FireTime);
            new Timer.TimedAction0ArgTrigger(createBall, FireTime);
        }

        void animateButtonPress(float time)
        {
            new Graphics.SetFrame(buttonPress, SpriteName.birdButtonPressDown, time);
            time += 500;
            new Graphics.SetFrame(buttonPress, SpriteName.birdButtonPressUp, time);
        }

        void createBall()
        {
            if (IsActiveGameState)
            {

                ball = new Graphics.Image(SpriteName.pigP1WingUp,
                    new Vector2(cannon.Xpos, cannon.Ypos + cannon.Height * 0.4f),
                    new Vector2(Engine.Screen.IconSize * 3f), ImageLayers.Lay4, true);

                Graphics.Motion2d rotate = new Graphics.Motion2d(Graphics.MotionType.ROTATE,
                    ball, new Vector2(2f), Graphics.MotionRepeate.Loop, 1000, true);

                float FallTime = 600;

                Graphics.Motion2d fall = new Graphics.Motion2d(Graphics.MotionType.MOVE,
                    ball, VectorExt.V2FromY(Engine.Screen.Height * 0.4f), Graphics.MotionRepeate.NO_REPEAT, FallTime, true);

                new Timer.TimedAction0ArgTrigger(doneFalling, FallTime + 200);
            }
        }

        void doneFalling()
        {
            if (IsActiveGameState)
            {
                bumpCount = new Graphics.Image(SpriteName.birdBumpCount0, ball.Position,
                ball.Size * 0.6f, ImageLayers.Lay3, true);
                bumpCount.Ypos -= ball.Height * 0.5f;

                buttonPress.Ypos = ball.Ypos;

                float time = 400;
                for (int i = 0; i < Ball.BumpCount; ++i)
                {
                    new Timer.TimedAction0ArgTrigger(bump, time);
                    time += 900;
                }
            }
        }

        int animatedBumps = 0;

        void bump()
        {
            if (IsActiveGameState)
            {
                animatedBumps++;
                animateButtonPress(0);
                Graphics.Motion2d scaleBump = new Graphics.Motion2d(Graphics.MotionType.SCALE,
                    ball, ball.Size * 0.7f, Graphics.MotionRepeate.BackNForwardOnce, 300, true);
                bumpCount.SetSpriteName(SpriteName.birdBumpCount0, animatedBumps);

                const float WaveTime = 400;
                Graphics.Image wave = new Graphics.Image(SpriteName.birdBumpWave, ball.Position,
                    ball.Size * 0.3f, ImageLayers.Lay6, true);
                new Graphics.Motion2d(Graphics.MotionType.SCALE, wave, ball.Size * 3f, Graphics.MotionRepeate.NO_REPEAT,
                    WaveTime, true);
                new Timer.Terminator(WaveTime, wave);
            }
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            
            if (tutorialSkipInput())
            {
                endTutorial();
            }
        }

        void loadContent_Asynch()
        {
            bgTexture = Engine.LoadContent.LoadTexture(PjLib.ContentFolder + "bagatelle_bg");
            new Timer.Action0ArgTrigger(onLoadingComplete);
        }

        public override void NetworkReadPacket(Network.ReceivedPacket packet)
        {
            base.NetworkReadPacket(packet);
            if (packet.type == Network.PacketType.birdGameStart)
            {
                endTutorial();
                if (Ref.gamestate is Bagatelle.BagatellePlayState)
                {
                    ((Bagatelle.BagatellePlayState)Ref.gamestate).readGameStart(packet.r);
                }
            }
        }

        void onLoadingComplete()
        {
            float timeSec;

            if (ViewTimes == 0)
            {
                timeSec = 5;

                if (PjRef.host)
                {
                    new Timer.ActionEventTimedTrigger(endTutorial, new Time(timeSec, TimeUnit.Seconds));
                }
            }
            else
            {
                endTutorial();
            }

            ViewTimes++;

        }

        void endTutorial()
        {
            if (Ref.gamestate == this)
            {
                new BagatellePlayState(joinedLocalGamers, bgTexture, 0);
            }
        }

    }
}
