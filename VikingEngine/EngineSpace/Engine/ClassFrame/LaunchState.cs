using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.Graphics;
using VikingEngine.LootFest;
using VikingEngine.Sound;
using VikingEngine.SteamWrapping;
using VikingEngine.ToGG.HeroQuest.Display;

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
            Config,
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
        bool dataProcessComplete = false;
        WaitForCloudSynch waitForCloudSynch = new WaitForCloudSynch();

        protected int mainPart = 0;
        protected int contentPart = 0;
        protected int storagePart = 0;
        protected int dataProcessPart = 0;
        int updateCounter = 0;

        string exceptionString;
        Texture2D bgTex = null;
        TextS progressString = null;
        Graphics.ImageAdvanced bgImage = null;
        protected SoundContainerSingle introSound = null;

        public LaunchState(bool isReset)
            :base()
        {
            Ref.draw.ClrColor = new Color(33, 37, 41);
            
            if (isReset)
            {
                
                load = LoadState.COMPLETE;

                new Timer.AsynchActionTrigger(() =>
                {
                    try
                    {
                        asyncLoading_OnRestart(ref contentPart);
                        loadingDataComplete = true;
                        loadingContentComplete = true;
                    }
                    catch (Exception ex)
                    {
                        exceptionString = ex.Message + " :: " + Environment.NewLine + ex.StackTrace;
                    }
                });       
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
                mainPart = 0;
                LoadContent.LoadConsoleFont();
                mainPart++;
                load = LoadState.Config;

                Config.OnStartUp();
                mainPart++;

                load = LoadState.Screen;
                Ref.gamesett.Load();
                mainPart++;
#if PCGAME
                Engine.Screen.ApplyScreenSettings(false);
                mainPart++;
#endif
                progressString = new TextS(LoadedFont.Console, VectorExt.AddY( Engine.Screen.SafeArea.LeftBottom, -20), new Vector2(1), Align.CenterHeight,
                    string.Empty, Color.Gray, ImageLayers.Top8);
                mainPart++;

                new Timer.AsynchActionTrigger(() =>
                {
                    try
                    {
                        load = LoadState.Splash;
                        mainPart = 10;
                        asyncLoadIntro();
                        mainPart++;
                        bgTex = Ref.main.Content.Load<Texture2D>(LoadContent.TexturePath + "monogame_splash");
                        mainPart++;
                        load = LoadState.DefaltContent;
                        Ref.main.baseContentLoad(ref contentPart);
                        mainPart++;
                        Engine.Screen.RefreshUiSize();
                        mainPart++;
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
                        asyncContentLoading(ref contentPart);
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
                        asyncStorageLoading(ref storagePart);
                        loadingDataComplete = true;
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
                        asyncDataProcessLoading();
                        dataProcessComplete = true;
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

            introSound?.Play();
        }

        virtual protected void asyncLoadIntro() { }

        /// <summary>
        /// Before asych loading
        /// </summary>
        abstract protected void preLoading();
        abstract protected void asyncContentLoading(ref int part);
        abstract protected void asyncStorageLoading(ref int part);
        virtual protected async void asyncDataProcessLoading() { throw new NotImplementedException(); }
        abstract protected void asyncLoading_OnRestart(ref int part);
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
                        if (loadingContentComplete && loadingDataComplete && dataProcessComplete)
                        {
                            launch();
                        }
                    }
                }

                if (progressString != null)
                {
                    updateCounter++;
                    if (updateCounter >= 100)
                    { 
                        updateCounter = 0;
                    }
                    progressString.TextString = $"State{(int)load}, m{mainPart}, c{contentPart}, s{storagePart}, d{dataProcessPart}, u{updateCounter}";
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
                    new Graphics.TextBoxSimple(LoadedFont.Console, new Vector2(20), Vector2.One, Graphics.Align.Zero, exceptionString,
                        Color.White, ImageLayers.AbsoluteTopLayer, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.8f);
                }
            }
        }
    }
}
