using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
////xna
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VikingEngine.LootFest.Editor;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;

namespace VikingEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        /* Static Properties */
        public static bool GameIsActive { get { return gameIsActive; } }
        public static int NextThreadIx { get { threadIndex++; return threadIndex; } }
#if PCGAME
        public static bool IsMainThread { get { return System.Threading.Thread.CurrentThread.ManagedThreadId == mainThreadID; } }
#endif
        /* Static Fields */
        static bool gameIsActive = false;
        static int threadIndex = 0;
        static int mainThreadID;
        public bool criticalContentIsLoaded = false;

        /* Static Methods */
        static void AbortAllThreads()
        {
            TaskExt.CheckStorageQue();

            Ref.update.AbortThreads();
            //Ref.asynchUpdate.AbortThreads();
            //Engine.Storage.AbortSaveThread(true);
        }

        /* Fields */
        object stateObj = (object)"ERROR";
        bool halfUpdate = true;
        GameTime gameTime;
        DateTime start;
        //public TaskScheduler taskScheduler;

        /* Constructors */
        public MainGame()
        {
            Ref.main = this;

#if DEBUG
            //"en-US",  // English (United States)
            //"de-DE",  // German (Germany)
            //"fr-FR",  // French (France) /
            //"de-CH",  // German (Switzerland)
            //"pt-BR",  // Portuguese (Brazil)
            //"it-IT",  // Italian (Italy)
            //"es-ES",  // Spanish (Spain)
            //"nl-NL",  // Dutch (Netherlands)
            //"sv-SE",  // Swedish (Sweden)
            //"da-DK",  // Danish (Denmark)
            //"fi-FI",  // Finnish (Finland)
            //"ru-RU",  // Russian (Russia)/
            //"zh-CN",  // Chinese (Simplified, China)/
            //"ja-JP",  // Japanese (Japan)/
            //"ko-KR",  // Korean (Korea)
            //"ar-SA",  // Arabic (Saudi Arabia)/
            //"hi-IN",  // Hindi (India)
            //"th-TH",  // Thai (Thailand)
            //"he-IL",  // Hebrew (Israel)

            //CultureInfo culture = new CultureInfo("ar-SA");
            //CultureInfo.DefaultThreadCurrentCulture = culture;
            //CultureInfo.DefaultThreadCurrentUICulture = culture;

#endif 

            DebugExtensions.BlueScreen.TryCatch(init1_Construct, DebugExtensions.TryMethodType.Init1);
        }

        /* Family Methods */
        protected override void Initialize()
        {
            DebugExtensions.BlueScreen.TryCatch(init2_Initialize, DebugExtensions.TryMethodType.Init2);
        }
        protected override void LoadContent()
        {
            DebugExtensions.BlueScreen.TryCatch(init3_LoadContent, DebugExtensions.TryMethodType.Init3);
        }



        //private static string inputBuffer = "";

        //public static void RegisterFocusedButtonForTextInput(System.EventHandler<TextInputEventArgs> method)
        //{
        //    // Example `gw` reference; this must be your actual game window or framework's input object
        //    Ref.main.Window.TextInput += method;
        //}

        //private static void OnTextInput(object sender, TextInputEventArgs e)
        //{
        //    // Handle backspace
        //    if (e.Character == '\b' && inputBuffer.Length > 0)
        //    {
        //        inputBuffer = inputBuffer.Substring(0, inputBuffer.Length - 1);
        //    }
        //    else
        //    {
        //        // Append input to the buffer
        //        inputBuffer += e.Character;
        //    }

        //    Console.WriteLine($"Current input: {inputBuffer}");
        //}

        //// Usage in initialization
        //public static void InitializeTextInput()
        //{
        //    RegisterFocusedButtonForTextInput(OnTextInput);
        //}


        protected override void Update(GameTime gameTime)
        {
            //if (PlatformSettings.RunProgram == StartProgram.LootFest3 && Input.Keyboard.KeyDownEvent(Keys.D5))
            //{ PlatformSettings.DebugWindow = !PlatformSettings.DebugWindow; }
            
            if (PlatformSettings.DebugPerformanceText) start = DateTime.Now;

            this.gameTime = gameTime;
            gameIsActive = IsActive;
            halfUpdate = !halfUpdate;

            Engine.Update.IsRunningSlow = gameTime.IsRunningSlowly;
            DebugExtensions.BlueScreen.TryCatch(updateLoop, DebugExtensions.TryMethodType.U);
            DebugExtensions.BlueScreen.CatchThreadExeception();


            if (PlatformSettings.DebugPerformanceText) Engine.StateHandler.UpdateTimePass(DateTime.Now.Subtract(start).TotalMilliseconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            DebugExtensions.BlueScreen.TryCatch(Ref.draw.MainDrawLoop, DebugExtensions.TryMethodType.D);
            
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            //Ref.analytics.onExit();
            Ref.update.exitApplication = true;
            AbortAllThreads();

            //System.Threading.Thread.Sleep(16);

            Engine.Sound.StopMusic();
            Engine.XGuide.OnSuspend(true);
            //Ref.gamestate.onClosingApplication();
            base.UnloadContent();

            //Input.PlayerInputMap.StopAllVibration();
            Input.Mouse.Visible = true;
        }

        /* Novelty Methods */
        void init1_Construct()
        {   
            Debug.Init();
            Engine.Draw.graphicsDeviceManager = new GraphicsDeviceManager(this);
            Engine.Draw.graphicsDeviceManager.IsFullScreen = false;
                     
            Content.RootDirectory = "Content";            
        }
        
        void init2_Initialize()
        {
            int targetFrameRate = 60;
#if PCGAME
            mainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
            var sett = new GameSettings();
            if (PlatformSettings.PC_platform)
            {
                targetFrameRate = sett.FrameRate;
            }

            Engine.Update.SetFrameRate(targetFrameRate);


            //taskScheduler = new TaskScheduler(,);
            this.IsMouseVisible = true;
            base.Initialize();

            int loops = 0;
            while (Engine.Draw.graphicsDeviceManager.GraphicsDevice == null)
            {
                Thread.Sleep(100);
                if (loops++ > 20)
                {
                    throw new Exception("Empty GraphicsDevice");
                }
            }

        }

        void init3_LoadContent()
        {
            Engine.LoadContent.Init(Content);
            DataLib.SpriteCollection.Init();
            Engine.Draw.Init();

            Input.InputLib.Init(this);

#if XBOX
            Engine.Screen.ApplyScreenSettings();
            new XboxWrapping.XboxManager();
#endif
            Engine.XGuide.Init(this);

            DataLib.SaveLoad.Init();

            bootUp();

        }

        void bootUp()
        {
            GameIntroState(false);
        }

        public void GameIntroState(bool isReset)
        {
#if PJ
            new PJ.StartScreen(isReset);
#elif DSS
            new DSSWars.IntroState(isReset);
#elif TOGG
            new ToGG.IntroState(isReset);
#elif SPECIAL
            new Voxels.CreateTexState();
#else
            new LootFest.GameState.IntroScene(isReset);
#endif

        }
        
        void updateLoop()
        {
            if (Ref.update.MainUpdate(gameTime))
            {
                AbortAllThreads();
                this.ExitGame();
            }
        }

        public void beginExit()
        {
            Ref.update.exitApplication = true;
        }

        public void ExitGame()
        {
            this.Exit();
        }
        public void FormTitle(string text)
        {
            Window.Title = text;
        }
    }
}
