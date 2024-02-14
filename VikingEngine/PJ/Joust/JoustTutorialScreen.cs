using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace VikingEngine.PJ.Joust
{
    class JoustTutorialScreen : AbsPJGameState
    {
        static int TutorialViewTimes = 0;
        //Texture2D texture;
        Time viewTime;
        bool loadComplete = false;
        bool skipInput = false;

        public JoustTutorialScreen(List2<GamerData> joinedGamers)
            : base(false)
        {
            Microsoft.Xna.Framework.Media.MediaPlayer.Stop();
            Input.Mouse.Visible = false;

            this.joinedLocalGamers = joinedGamers;
            Ref.draw.ClrColor = PjLib.ClearColor;
            JoustLib.Init();

            //Graphics.Image bgTex = new Graphics.Image(SpriteName.TutorialBgTex, VectorExt.V2NegOne, 
            //    Engine.Screen.ResolutionVec + new Vector2(2), ImageLayers.Background4);



          
            new PjEngine.Display.BlueprintTexture();

            Vector2 iconSz = new Vector2(Engine.Screen.SafeArea.Height * 0.06f);
            Vector2 quarterScreenArea = new Vector2(Engine.Screen.SafeArea.Height / 4f);
            Vector2 animalSz = iconSz * 2.1f;

            {//Jumping sheep
                Vector2 jumpCurveSz = new Vector2(quarterScreenArea.X * 0.8f);
                jumpCurveSz.Y *= 0.5f;
                jumpCurveSz = VectorExt.Round(jumpCurveSz);

                Vector2 center = Engine.Screen.SafeArea.PercentToPosition(0.5f, 0.2f);
                Vector2 start = center;
                start.X -= jumpCurveSz.X * 1.5f;
                Vector2 curvePos = start;

                for (int i = 0; i < 3; ++i)
                {
                    Graphics.Image curve = new Graphics.Image(SpriteName.PjJumpCurve,
                        curvePos, jumpCurveSz, ImageLayers.Lay9, true);

                    Vector2 inputPos = curve.RealArea().PercentToPosition(0.5f, 1.2f);

                    Graphics.Image upButton = new Graphics.Image(SpriteName.birdButtonPressUp,
                        inputPos, iconSz * 0.9f, ImageLayers.Lay4, true);

                    Graphics.Image downButton = new Graphics.Image(SpriteName.birdButtonPressDown,
                        new Vector2(curve.RealRight, inputPos.Y), 
                        iconSz * 1.2f, ImageLayers.Lay4, true);

                    curvePos.X += jumpCurveSz.X;
                }

                Graphics.ImageAdvanced halfcurve = new Graphics.ImageAdvanced(SpriteName.PjJumpCurve,
                        curvePos, jumpCurveSz, ImageLayers.Lay9, true);
                halfcurve.Xpos -= halfcurve.Width * 0.25f;
                halfcurve.SourceWidth /= 2;
                halfcurve.Width /= 2;

                Vector2 sheepPos = halfcurve.RealArea().PercentToPosition(new Vector2(1.4f, 0.2f));
                Graphics.Image sheep = new Graphics.Image(SpriteName.sheepWhiteWingDown,
                    sheepPos, animalSz * 1.1f, ImageLayers.Lay1, true);
                sheep.Rotation = -0.14f;
            }

            {//Damage
                Vector2 center = Engine.Screen.SafeArea.PercentToPosition(0.26f, 0.6f);

                Graphics.Image defender = new Graphics.Image(SpriteName.birdP1Dead,
                    center, animalSz, ImageLayers.Lay7, true);
                defender.spriteEffects = SpriteEffects.FlipHorizontally;
                defender.RotationDegrees = 10;

                Vector2 smackPos = defender.RealArea().PercentToPosition(0.3f, 0.24f);
                Graphics.Image smack = new Graphics.Image(SpriteName.PjSmackIcon,
                    smackPos, animalSz * 0.6f, ImageLayers.Lay6, true);

                Vector2 attackerPos = defender.RealArea().PercentToPosition(0.1f, -0.05f);
                Graphics.Image attacker = new Graphics.Image(SpriteName.pigP1WingUp,
                    attackerPos, animalSz, ImageLayers.Lay7, true);
                attacker.RotationDegrees = -5;

                Vector2 heartPos = defender.RealArea().PercentToPosition(0.9f, 0.5f);
                Graphics.Image minus = new Graphics.Image(SpriteName.pjNumMinus, heartPos,
                    iconSz * 0.7f, ImageLayers.Lay0, true);
                Graphics.Image heart = new Graphics.Image(SpriteName.PjHeartBroken, 
                    minus.position, iconSz * 1.5f, ImageLayers.Lay0, true);
                heart.Xpos += iconSz.X * 0.7f;

            }

            {//Ground kill
                Vector2 center = Engine.Screen.SafeArea.PercentToPosition(0.4f, 0.85f);

                Graphics.Image defender = new Graphics.Image(SpriteName.birdP1Dead,
                    center, animalSz, ImageLayers.Lay7, true);
                //defender.spriteEffects = SpriteEffects.FlipHorizontally;
                defender.RotationDegrees = 200;

                Graphics.Image ground = new Graphics.Image(SpriteName.birdGroundTex,
                    defender.RealArea().PercentToPosition(0.4f, 0.6f),
                    animalSz * 2f, ImageLayers.Lay9_Back);
                ground.Height *= 3f / 4f;
                ground.OrigoAtCenterWidth();

                Vector2 smackPos = defender.RealArea().PercentToPosition(0.4f, 0.24f);
                Graphics.Image smack = new Graphics.Image(SpriteName.PjSmackIcon,
                    smackPos, animalSz * 0.6f, ImageLayers.Lay6, true);

                Vector2 attackerPos = defender.RealArea().PercentToPosition(0.25f, -0.05f);
                Graphics.Image attacker = new Graphics.Image(SpriteName.pigP1WingUp,
                    attackerPos, animalSz, ImageLayers.Lay7, true);
                attacker.RotationDegrees = -8;

                Vector2 speechPos = defender.RealArea().PercentToPosition(1.1f, 0.0f);
                Graphics.Image minus = new Graphics.Image(SpriteName.PjDeathSpeechBobble, speechPos,
                    iconSz * 1.6f, ImageLayers.Lay0, true);
                
            }

            {//Coins
                Vector2 start = Engine.Screen.SafeArea.PercentToPosition(0.7f, 0.45f);

                Vector2 pos = start;
                for (int i = 0; i < 3; ++i)
                {
                    Graphics.Image coin = new Graphics.Image(SpriteName.birdCoin1, pos, 
                        iconSz, ImageLayers.Lay0, true);

                    pos.X += iconSz.X;
                }

                Graphics.Image arrow = new Graphics.Image(SpriteName.cmdConvertArrow, pos,
                        iconSz * 0.9f, ImageLayers.Lay0, true);
                pos.X += iconSz.X;
                Graphics.Image heart = new Graphics.Image(SpriteName.PjHeartFull, pos,
                        iconSz *1.5f, ImageLayers.Lay0, true);
            }

            {//Air trick
                Vector2 start = Engine.Screen.SafeArea.PercentToPosition(0.8f, 0.8f);

                Graphics.Image animal = new Graphics.Image(SpriteName.sheepWhiteWingDown,
                    start, animalSz, ImageLayers.Lay7, true);

                animal.RotationDegrees = 150;
                Graphics.Image wave1 = new Graphics.Image(SpriteName.joustAirTrickWave, 
                    VectorExt.AddX(start, animalSz.X * 0.3f), animalSz * 0.9f, ImageLayers.Lay7, true);
                wave1.Opacity = 0.3f;

                Graphics.Image wave2 = new Graphics.Image(SpriteName.joustAirTrickWave, 
                    wave1.position, wave1.size,  ImageLayers.Lay7, true);
                wave2.Xpos += wave2.Width * 0.25f;
                wave2.Opacity = 0.6f;

                Graphics.Image input = new Graphics.Image(SpriteName.birdButtonPressDown, 
                    VectorExt.AddY(animal.position, animalSz.Y * 0.7f), 
                    iconSz * 1.4f,  ImageLayers.Lay6, true);
                Graphics.Image timebar = new Graphics.Image(SpriteName.PjHoldButtonBar,
                     input.RealArea().PercentToPosition(0.8f, 0.6f), 
                     new Vector2(3, 1) * input.Height * 0.6f, ImageLayers.Lay7_Back);
                timebar.OrigoAtCenterHeight();
            }

            new StorageTask(loadContent_Asynch, true, onLoadingComplete);

#if XBOX
            Ref.xbox.presence.Set("joust");
#endif
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            //return;

            if (tutorialSkipInput())
            {
                skipInput = true;
            }

            if (loadComplete &&
                (skipInput || viewTime.CountDown()))
            {
                if (Ref.gamestate == this)
                {
                    new Joust.JoustGameState(joinedLocalGamers, 0);
                }
            }
        }

        void loadContent_Asynch(MultiThreadType type)
        {
            new Sounds();
            //texture = Engine.LoadContent.LoadTexture(PjLib.ContentFolder + "joust tut2");
        }

        void onLoadingComplete()
        {
            loadComplete = true;
            //float width = Engine.Screen.ResolutionVec.Y / texture.Height * texture.Width;
            //Graphics.ImageAdvanced img = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE, new Vector2(Engine.Screen.CenterScreen.X -(width / 2), 0), new Vector2(width, Engine.Screen.Height), ImageLayers.Foreground1, false);
            //img.Texture = texture;
            //img.SetFullTextureSource();
            //img.Visible = false;

            float time;
            
            if (TutorialViewTimes == 0)
            {
                time = 6;
            }
            else
            {
                time = 3;
            }

            viewTime = new Time(time, TimeUnit.Seconds);
            TutorialViewTimes++;
        }

    }
}
