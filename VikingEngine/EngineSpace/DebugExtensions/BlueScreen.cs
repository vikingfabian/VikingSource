using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Input;
using VikingEngine.HUD;

using VikingEngine.SteamWrapping;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Linq;

#if PCGAME
//using System.Windows.Forms;
#endif


namespace VikingEngine.DebugExtensions
{
    class BlueScreen : Engine.GameState
    {
        string detailedText;
        protected string logFullPath;

        protected Gui menu;
        public static Exception ThreadException = null;
        public static string AttachMessage = null;
        Time flashTimer = Time.Zero;
        bool redFlash = true;
        

        public BlueScreen()
        {
            cleanUp();
        }

        public BlueScreen(string errorMessageDetailed)
        {
            cleanUp();

            logError(Engine.LoadContent.EngineVersion + (AttachMessage== null? string.Empty : AttachMessage + Environment.NewLine) + errorMessageDetailed);
            
            errorMessageDetailed = Engine.LoadContent.EngineVersion + errorMessageDetailed;


            Engine.StateHandler.ReplaceGamestate(this);

            if (Ref.main.criticalContentIsLoaded)
            {
                detailedText = errorMessageDetailed;

               
                GuiLayout layout = createMenu("Game Crashed!");
                {
                    if (PlatformSettings.PC_platform)
                    {
                        new GuiLabel("A file with the crash details is created, see " + logFullPath, layout);
                        new GuiLabel("F12-Print screen. (share it on Steam and we will see it)", layout);
                        new GuiTextButton("Restart game", null, restart, false, layout);
                        new GuiTextButton("Exit to desktop", null, exitToDash, false, layout);
                    }
                    else if (PlatformSettings.TargetPlatform == ReleasePlatform.Xbox)
                    {
                        new GuiIconTextButton(SpriteName.ButtonA, "RESTART", null, restart, false, layout);
                    }
                    if (AttachMessage != null)
                    {
                        new GuiLabel(Engine.LoadContent.CheckCharsSafety(AttachMessage, menu.style.textFormat.Font), true, menu.style.textFormat, layout);
                    }
                    new GuiLabel(Engine.LoadContent.CheckCharsSafety(detailedText, menu.style.textFormat.Font), true, menu.style.textFormat, layout);
                }
                layout.End();
            }
            else
            {
                Engine.Draw.graphicsDeviceManager.ApplyChanges();
                Ref.main.Exit();
            }

            if (PlatformSettings.RunProgram == StartProgram.DSS)
            {  
                if (Ref.steam != null && Ref.steam.statsInitialized)
                {
                    DssRef.stats?.blueScreen.addOne();
                    Ref.steam.stats.upload();
                }
            }

            AttachMessage = null;
        }

        protected void cleanUp()
        {
            Ref.draw.CurrentRenderLayer = 0;
            TaskExt.ClearStorageQue();

            if (Ref.netSession != null)
                Ref.netSession.Disconnect("Blue screen");
        }

        protected void logError(string errorMessageDetailed)
        {
            Ref.sentry?.sendReport(errorMessageDetailed);

            if (PlatformSettings.PC_platform)
            {
                try
                {
                    var now = DateTime.Now;

                    var logFilePath = new DataStream.FilePath(
                         "Logs",
                         string.Format("{0}_{1}_{2}__{3}_{4}", now.Year, now.Month, now.Day, now.Hour, now.Minute),
                         ".txt", true, false);

                    System.IO.Directory.CreateDirectory(logFilePath.CompleteDirectory);

                    //create a log file
                    logFullPath = logFilePath.CompletePath(true);
                    DataLib.SaveLoad.CreateTextFile(logFullPath, new List<string>
                    {
                        PlatformSettings.SteamVersion,
                        errorMessageDetailed,
                    });
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }

        protected GuiLayout createMenu(string title)
        {
            Ref.draw.ClrColor = Color.DarkBlue;

            float t = 0.075f;
            float t_w = (1 - 2 * t);
            VectorRect rect = new VectorRect(t * Ref.draw.ScreenWidth, t * Ref.draw.ScreenHeight, t_w * Ref.draw.ScreenWidth, t_w * Ref.draw.ScreenHeight);
            var style = new GuiStyle(rect.Width, 5, SpriteName.WhiteArea);
            style.headBar = false;
            menu = new Gui(style, rect, 0, ImageLayers.AbsoluteBottomLayer, Input.InputSource.DefaultPC);
            GuiLayout layout = new GuiLayout("Game Crashed!", menu);

            return layout;
        }

        string compressText(string error)
        {
            //error = error.Replace("VikingEngine.LootFest.", "");
            //error = error.Replace("VikingEngine.", "");
            //error = error.Replace("VikingEngine.", "");
            //error = error.Replace("..ctor", "");
            //error = error.Replace(" at", " ");

            string result = "";
            bool addChar = true;
            foreach (char c in error)
            {
                if (addChar)
                {
                    result += c;
                    if (c == '(')
                        addChar = false;
                }
                else if (c == ')')
                {
                    addChar = true;
                }
            }

            return result;
        }

        public override void Time_Update(float time)
        {
            if (Ref.main.criticalContentIsLoaded)
            {
                if (menu.Update())
                {
                    Ref.update.exitApplication = true;
                }

                if (Input.XInput.KeyDownEvent(Buttons.A) ||
                    Input.XInput.KeyDownEvent(Buttons.X) ||
                    Input.XInput.KeyDownEvent(Buttons.Start))
                {
                    restart();
                }

                base.Time_Update(time);
                Ref.draw.Set2DTranslation(0, Vector2.Zero);
                Ref.draw.Set2DScale(0, 1f);
            }
            else 
            {
                if (flashTimer.CountDown())
                {
                    flashTimer = new Time(1, TimeUnit.Seconds);
                    redFlash = !redFlash;
                    Ref.draw.ClrColor = redFlash? Color.Red : Color.Purple;
                }

                if (Input.XInput.KeyDownEvent(Buttons.A) ||
                    Input.XInput.KeyDownEvent(Buttons.X) ||
                    Input.XInput.KeyDownEvent(Buttons.Start) ||
                    
                    Input.Keyboard.KeyDownEvent(Keys.Space) ||
                    Input.Keyboard.KeyDownEvent(Keys.Escape))
                {
                    Ref.update.exitApplication = true;
                }
            }
        }

        protected void exitToDash()
        {
            Ref.update.exitApplication = true;
        }
        protected void restart()
        {
            Ref.main.GameIntroState(true);
        }


        public static void TryCatch(Action method, TryMethodType methodType)
        {
            if (PlatformSettings.BlueScreen || Engine.Screen.PcDisplayMode == WindowDisplayMode.HardwareFullscreen)
            {
                try
                {
                    method();
                }
                //catch (AbsSteamException e) 
                //{
                //    new SteamBlueScreen(ErrorMessage(e, methodType));
                //}
                catch (Exception e)
                {
                    new BlueScreen(ErrorMessage(e, methodType));
#if PCGAME
                    SteamCrashReport.uploadException(e, methodType);
#endif                    
                }
            }
            else
            {
                method();
            }
        }

        public static void CatchThreadExeception()
        {
            if (ThreadException != null)
            {
#if DEBUG
                if (!PlatformSettings.BlueScreen)
                {
                    throw new Exception();
                }
#endif
                if (Ref.gamestate is BlueScreen == false)
                {
                    new BlueScreen(ErrorMessage(ThreadException, TryMethodType.A));
                }
                ThreadException = null;
            }
        }

        public static string ErrorMessage(Exception e, TryMethodType methodType)
        {
            string gametypeCode = "-";
            switch (PlatformSettings.RunProgram)
            {
                case StartProgram.LootFest3:
                    gametypeCode = "L3";
                    break;
                case StartProgram.PartyJousting:
                    gametypeCode = "PJ";
                    break;
                case StartProgram.DSS:
                    gametypeCode = "DS";
                    break;
                case StartProgram.ToGG:
                    gametypeCode = "CM";
                    break;
            }

            string type = gametypeCode + methodType.ToString();


            if (methodType == TryMethodType.U)
            {
                type += " N" + ((int)Network.NetLib.PacketType).ToString();
            }
            if (!Ref.steam.isInitialized)
            {
                type += "-P";
            }

            string stacktrace = string.Empty;
            var stackFrames = new StackTrace(e, fNeedFileInfo: true).GetFrames();
            foreach (var frame in stackFrames)
            {
                string fileName = frame.GetFileName();
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = TextLib.Unknown;
                }
                else
                {
                    fileName = fileName.Split("VikingEngine").Last();
                }
                stacktrace += $"{fileName} ::line {frame.GetFileLineNumber()}, col {frame.GetFileColumnNumber()}" + Environment.NewLine;
                var m = frame.GetMethod();
                if (m != null)
                {
                    stacktrace += m + Environment.NewLine;
                }

                stacktrace += Environment.NewLine;
            }

            return type + "; " + e.Message + " @" + stacktrace;
        }


    }

    class TheadedCrash : OneTimeTrigger
    {
        string text;
        public TheadedCrash(Exception e)
            :base(false)
        {
            text = BlueScreen.ErrorMessage(e, TryMethodType.A);
            AddToUpdateList();
        }

        public override void Time_Update(float time)
        {
            new BlueScreen(text);
        }
    }

    enum TryMethodType
    {
        Init1,
        Init2,
        Init3,

        /// <summary>
        /// Draw loop
        /// </summary>
        D,

        /// <summary>
        /// Update loop
        /// </summary>
        U,

        /// <summary>
        /// Asynch update
        /// </summary>
        A,
    }
}
