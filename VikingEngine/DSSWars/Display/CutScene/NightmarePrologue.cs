using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Timer;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars.Display.CutScene
{
    class NightmarePrologue : AbsCutScene
    {
        const float TextFadeSpeed = 1.0f;

        Graphics.Image blackout;
        Texture2D bgTex = null;
        Graphics.ImageAdvanced bgImage = null;
        Image eyeGlow = null;
        Vector2 eyePos = Vector2.Zero;
        StateType state = 0;
        float stateTime = float.MaxValue;
        Graphics.TextG title;
        Graphics.TextBoxSimple text1, text2;
        List<string> strings = new List<string>();
        float imageGoalOpacity = 0f;
        float glowGoalOpacity = 0f;
        Time emitTimer = new Time(1);

        public NightmarePrologue() 
            :base()
        {
            VectorRect area = Screen.Area;
            area.AddRadius(5);
            blackout = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, HudLib.CutSceneBgLayer + 1);
            blackout.Color = new Color(16, 16, 32);

            Ref.music.stop(false);
            new Timer.AsynchActionTrigger(load_asynch, true);

            strings = DssRef.lang.Prologue_TextLines;
            title = new Graphics.TextG(LoadedFont.Bold, VectorExt.AddY(Engine.Screen.CenterScreen, -Engine.Screen.IconSize * 2f), Engine.Screen.TextSizeV2 * 2f, Graphics.Align.CenterWidth,
                DssRef.lang.Prologue_Title, Color.Yellow, HudLib.StoryContentLayer);
            text1 = new Graphics.TextBoxSimple(LoadedFont.Regular, VectorExt.AddY(Engine.Screen.CenterScreen, -Engine.Screen.IconSize), Engine.Screen.TextSizeV2 * 2f, Graphics.Align.CenterWidth,
               "XXX",
               Color.White, HudLib.StoryContentLayer, Engine.Screen.Width * 0.3f);

            text2 = new Graphics.TextBoxSimple(LoadedFont.Regular, VectorExt.AddY(Engine.Screen.CenterScreen, Engine.Screen.IconSize), Engine.Screen.TextSizeV2 * 2f, Graphics.Align.CenterWidth,
               "XXX",
               Color.White, HudLib.StoryContentLayer, Engine.Screen.Width * 0.3f);

            title.Opacity = 0;
            title.Visible = false;
            text1.Opacity = 0;
            text1.Visible = false;
            text2.Opacity = 0;
            text2.Visible = false;
        }

        void load_asynch()
        {
            Ref.music.PlaySong(Data.Music.Nightmare, true);
            bgTex = Ref.main.Content.Load<Texture2D>(DssLib.StoryContentDir + "nighmare");
        }

        public override void Time_Update(float time)
        {
            if (bgImage == null)
            {
                //loading image 
                if (bgTex != null)
                {
                    float w = Screen.SafeArea.Width;
                    float h = w / bgTex.Width * bgTex.Height;

                    if (h > Screen.SafeArea.Height)
                    {
                        h = Screen.SafeArea.Height;
                        w = h / bgTex.Height * bgTex.Width;
                    }

                    bgImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                        Screen.CenterScreen - new Vector2(w, h) * 0.5f, new Vector2(w, h), HudLib.CutSceneBgLayer - 1, false);
                    bgImage.Texture = bgTex;
                    bgImage.SetFullTextureSource();
                    bgImage.Opacity = 0f;

                    eyePos = bgImage.Area.PercentToPosition(0.4f, 0.4f);


                    float glowSize = 0.0466f * h;
                    eyeGlow = new Image(SpriteName.WarsNightmareEyeGlow, bgImage.position + new Vector2(0.62f, 0.39f) * h,
                        new Vector2(glowSize), HudLib.CutSceneBgLayer - 2);
                    eyeGlow.Opacity = 0f;
                }

            }
            
            if (bgImage != null)
            {
                if (bgImage.Opacity < imageGoalOpacity)
                {
                    float speed = 0.02f;

                    if (state >= StateType.DisplayName)
                    {
                        speed = 0.2f;
                    }
                    bgImage.Opacity += speed * Ref.DeltaTimeSec;
                }

                if (eyeGlow.Opacity < glowGoalOpacity)
                {
                    eyeGlow.Opacity += 0.1f * Ref.DeltaTimeSec;
                }
                else if (eyeGlow.Opacity > glowGoalOpacity)
                {
                    eyeGlow.Opacity -= 0.1f * Ref.DeltaTimeSec;
                }

                //if (imageGoalOpacity > 0f)
                //{
                //    //emitGlow();
                //}
            }
            

            //if (state == StateType.LoadingSong &&
            //    Ref.music.PlaySongState == Sound.PlaySongState.Playing)
            //{
            //    state++;

            //    //title.Visible = true;
            //    stateTime = 0;
            //}

            stateTime -= Ref.DeltaTimeMs;
            if (stateTime <= 0)
            {
                state++;
                stateTime = float.MaxValue;

                switch (state)
                {
                    default:
                        
                        break;
                    case StateType.FadeInTitle:
                        title.Visible = true;
                        break;

                    case StateType.DisplayTitle:
                        stateTime = 4000;
                        break;

                    case StateType.FadeInText1:
                        glowGoalOpacity = 0.5f;
                        text1.TextString = strings[0];
                        text1.Visible = true;
                        break;

                    case StateType.DisplayText1:
                        stateTime = 2000;
                        break;

                    case StateType.FadeInText2:
                        imageGoalOpacity = 0.02f;
                        text2.TextString = strings[1];
                        text2.Visible = true;
                        break;

                    case StateType.DisplayText2:
                        stateTime = 4000;
                        break;

                    case StateType.FadeInText3:
                        imageGoalOpacity = 0.04f;
                        text1.TextString = strings[2];
                        text1.Visible = true;
                        break;

                    case StateType.DisplayText3:
                        stateTime = 2000;
                        break;

                    case StateType.FadeInName:
                        imageGoalOpacity = 1f;
                        glowGoalOpacity = 0f;
                        text2.TextString = DssRef.lang.FactionName_DarkLord;
                        text2.Visible = true;
                        break;

                    case StateType.DisplayName:
                        stateTime = 3000;
                        break;

                    case StateType.DisplayImageOnly:
                        stateTime = 3000;
                        break;

                    case StateType.End:
                        Close();
                        break;
                }
            }


            switch (state)
            {
                case StateType.LoadingSong:
                    if (Ref.music.PlaySongState == Sound.PlaySongState.Playing)
                    {
                        stateTime = 0;
                    }
                    break;

                case StateType.FadeInTitle:
                    title.Opacity += TextFadeSpeed * Ref.DeltaTimeSec;
                    if (title.Opacity >= 1f)
                    { 
                        stateTime = 0;
                    }
                    break;
                case StateType.FadeOutTitle:
                    title.Opacity -= TextFadeSpeed * Ref.DeltaTimeSec;
                    if (title.Opacity <= 0f)
                    {
                        stateTime = 0;
                    }
                    break;

                case StateType.FadeInText1:
                    text1.Opacity += TextFadeSpeed * Ref.DeltaTimeSec;
                    if (text1.Opacity >= 1f)
                    {
                        stateTime = 0;
                    }
                    break;

                case StateType.FadeInText2:
                    text2.Opacity += TextFadeSpeed * Ref.DeltaTimeSec;
                    if (text2.Opacity >= 1f)
                    {
                        stateTime = 0;
                    }
                    break;

                case StateType.FadeOutText1And2:
                    
                    text1.Opacity -= TextFadeSpeed * Ref.DeltaTimeSec;
                    text2.Opacity = text1.Opacity;
                    if (text1.Opacity <= 0f)
                    {
                        stateTime = 0;
                    }
                    break;

                case StateType.FadeInText3:
                    text1.Opacity += TextFadeSpeed * Ref.DeltaTimeSec;
                    if (text1.Opacity >= 1f)
                    {
                        stateTime = 0;
                    }
                    break;

                case StateType.FadeInName:
                    text1.Opacity -= TextFadeSpeed * Ref.DeltaTimeSec;
                    text2.Opacity += TextFadeSpeed * Ref.DeltaTimeSec;
                    if (text2.Opacity >= 1f)
                    {
                        stateTime = 0;
                    }
                    break;
                case StateType.FadeOutName:

                    text2.Opacity -= TextFadeSpeed * 0.25f * Ref.DeltaTimeSec;
                    if (text2.Opacity <= 0f)
                    {
                        stateTime = 0;
                    }
                    break;
            }

            if (DssRef.state.closeMenuInput_AnyPlayer())
            {
                Close();
            }

        }

        void emitGlow()
        {
            if (emitTimer.CountDown())
            {
                emitTimer = new Time(Ref.rnd.Float(0.01f, 0.2f), TimeUnit.Seconds);
                
                //if (bgImage != null)
                {
                    //Ref.draw.CurrentRenderLayer = 1;
                    float maxSpeed = bgImage.Ypos * 0.0001f;
                    Vector2 speed = Ref.rnd.vector2_cirkle(maxSpeed);
                    speed.Y -= maxSpeed * 0.5f;
                    var particle = new ParticleImage(SpriteName.WhiteArea, eyePos, VectorExt.V2(bgImage.Height * 0.01f), HudLib.CutContentLayer - 2, speed);
                    particle.Color = Color.LightYellow;
                    particle.Opacity = 0.2f;
                    particle.particleData.setFadeout(400, 200);
                    //Ref.draw.CurrentRenderLayer = 0;
                }
            }
        }

        public override void Close()
        {
            blackout.DeleteMe();
            bgImage?.DeleteMe();
            eyeGlow?.DeleteMe();
            
            title.DeleteMe();
            text1.DeleteMe();
            text2.DeleteMe();

            base.Close();
        }

        private enum StateType
        { 
            LoadingSong,

            FadeInTitle,
            DisplayTitle,
            FadeOutTitle,

            FadeInText1,
            DisplayText1,
            FadeInText2,
            DisplayText2,
            FadeOutText1And2,

            FadeInText3,
            DisplayText3,

            FadeInName,
            DisplayName,
            FadeOutName,

            DisplayImageOnly,
            End,
        }
    }
}
