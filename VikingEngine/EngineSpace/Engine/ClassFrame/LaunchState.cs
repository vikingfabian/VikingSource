using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars;
using VikingEngine.SteamWrapping;
using VikingEngine.DSSWars.Map.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Engine
{
    abstract class LaunchState : GameState
    {
        /*
            --should run in four steps--
            1. load font
            2. load screen size
            3. load splash image
            4. defalt content
            5. load the rest of the content
        */

        enum LoadState
        { 
            Font,
            Screen,
            Splash,
            DefaltContent,
            GameContent,
            COMPLETE,            
        }

        bool failState = false;
        LoadState load = 0;

        bool loadingDefaltContentComplete = false;
        bool loadingContentComplete = false;
        bool loadingDataComplete = false;
        WaitForCloudSynch waitForCloudSynch = new WaitForCloudSynch();

        protected int contentPart = 0;
        protected int storagePart = 0;

        string exceptionString;
        Texture2D bgTex = null;
        Graphics.ImageAdvanced bgImage = null;
        public LaunchState(bool isReset)
            :base()
        {
            Ref.draw.ClrColor = new Color(33, 37, 41);
            
            if (isReset)
            {
                loadingDataComplete = true;
                loadingContentComplete = true;
                load = LoadState.COMPLETE;
            }
            else
            {
                initPart1();
            }           
        }

        void initPart1()
        {
            try
            {
                //1.
                LoadContent.LoadConsoleFont();

                load = LoadState.Screen;

                Ref.gamesett.Load();

#if PCGAME
                Engine.Screen.ApplyScreenSettings(false);
#endif
                
                //todo splash

                new Timer.AsynchActionTrigger(() =>
                {
                    try
                    {
                        load = LoadState.Splash;

                        bgTex = Ref.main.Content.Load<Texture2D>(LoadContent.TexturePath + "monogame_splash");

                        load = LoadState.DefaltContent;
                        Ref.main.baseContentLoad(ref contentPart);
                        Engine.Screen.RefreshUiSize();
                        loadingDefaltContentComplete = true;
                    }
                    catch (Exception ex)
                    {
                        exceptionString = ex.Message + " :: " + Environment.NewLine + ex.StackTrace;
                    }
                });     
            }
            catch (Exception ex)
            {
                //failState = true;
                exceptionString = ex.Message + " :: " + Environment.NewLine + ex.StackTrace;

            }
        }

        void initPart2()
        {
            try
            {
                preLoading();

                new Timer.AsynchActionTrigger(() =>
                {
                    try
                    {
                        contentPart = 0;
                        asynchContentLoading(ref contentPart);
                        loadingContentComplete = true;
                    }
                    catch (Exception ex)
                    {
                        exceptionString = ex.Message + " :: " + Environment.NewLine + ex.StackTrace;
                    }
                });
                new Timer.AsynchActionTrigger(() =>
                {
                    try
                    {
                        asynchStorageLoading(ref storagePart);
                        loadingDataComplete = true;
                    }
                    catch (Exception ex)
                    {
                        exceptionString = ex.Message + " :: " + Environment.NewLine + ex.StackTrace;
                    }
                });                
            }
            catch (Exception ex)
            {
                //failState = true;
                exceptionString = ex.Message + " :: " + Environment.NewLine + ex.StackTrace;
            }
        }

        void createSplash()
        {
            float w = bgTex.Width;
            float h = bgTex.Height;
            float x = Screen.CenterScreen.X - w * 0.5f;
            float y = Screen.CenterScreen.Y - h * 0.5f;

            bgImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                new Vector2(x, y), new Vector2(w, h), ImageLayers.Background5, false);
            bgImage.Texture = bgTex;
            bgImage.SetFullTextureSource();
            //bgImage.Color = ColorExt.GrayScale(0.8f);
            //bgImage.Opacity = 0.8f;

            //Vector2 promoworkerSz = new Vector2(9, 6) * new Vector2(h * 0.02f);

            //var worker1 = new Graphics.Image(SpriteName.warsWorkerPromoCannon, VectorExt.AddY(Engine.Screen.Area.PercentToPosition(0.7f, 1f), -promoworkerSz.Y * 0.9f), promoworkerSz, ImageLayers.Background5);
            //worker1.LayerAbove(bgImage);
        }

        /// <summary>
        /// Before asych loading
        /// </summary>
        abstract protected void preLoading();
        abstract protected void asynchContentLoading(ref int part);
        abstract protected void asynchStorageLoading(ref int part);
        abstract protected void launch();

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (failState)
            {
                return;
            }

            

            try
            {
                if (bgTex != null)
                {
                    createSplash();
                    bgTex = null;
                }


                if (load <= LoadState.DefaltContent)
                {
                    if (loadingDefaltContentComplete)
                    {
                        load++;
                        initPart2();
                    }
                }
                else
                {
                    if (waitForCloudSynch.update())
                    {
                        if (loadingContentComplete && loadingDataComplete)
                        {
                            launch();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionString = ex.Message + " :: " + Environment.NewLine + ex.StackTrace;
            }

            if (exceptionString != null)
            {
                if (bgImage != null)
                {
                    bgImage.Visible = false;
                }

                failState = true;
                switch (load)
                {
                    case LoadState.Font:
                        draw.ClrColor = Color.Red;
                        break;
                    case LoadState.Screen:
                        draw.ClrColor = Color.DarkOrange;
                        break;
                    case LoadState.Splash:
                        draw.ClrColor = Color.DarkMagenta;
                        break;
                    case LoadState.DefaltContent:
                        draw.ClrColor = Color.DarkGreen;
                        break;
                    case LoadState.GameContent:
                        draw.ClrColor = Color.DarkBlue;
                        break;
                }

                if (load > LoadState.Font)
                {
                    new Graphics.TextBoxSimple(LoadedFont.Console, Vector2.Zero, Vector2.One, Graphics.Align.Zero, exceptionString,
                        Color.White, ImageLayers.AbsoluteTopLayer, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
                }
            }
        }
    }
}
