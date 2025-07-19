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
            4. load the rest of the content
        */

        enum LoadState
        { 
            Font,
            Screen,
            Splash,
            GameContent,
            COMPLETE,            
        }

        bool failState = false;
        LoadState load = 0;

        bool loadingContentComplete = false;
        bool loadingDataComplete = false;
        WaitForCloudSynch waitForCloudSynch = new WaitForCloudSynch();

        protected int contentPart = 0;
        protected int storagePart = 0;

        string exceptionString;

        public LaunchState(bool isReset)
            :base()
        {
            Ref.draw.ClrColor = Color.Black;

            try
            {
                if (isReset)
                {
                    loadingDataComplete = true;
                    loadingContentComplete = true;
                }
                else
                {
                    //1.
                    LoadContent.Font(LoadedFont.Console);


                    preLoading();

                    new Timer.AsynchActionTrigger(() =>
                    {
                        try
                        {
                            asynchContentLoading();
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
                            asynchStorageLoading();
                            loadingDataComplete = true;
                        }
                        catch (Exception ex)
                        {
                            exceptionString = ex.Message + " :: " + Environment.NewLine + ex.StackTrace;
                        }
                    });

                }
            }
            catch (Exception ex)
            {
                //failState = true;
                exceptionString = ex.Message + " :: " + Environment.NewLine + ex.StackTrace;
                
            }
        }

        /// <summary>
        /// Before asych loading
        /// </summary>
        abstract protected void preLoading();
        abstract protected void asynchContentLoading();
        abstract protected void asynchStorageLoading();
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
                if (waitForCloudSynch.update())
                {
                    if (loadingContentComplete && loadingDataComplete)
                    {
                        launch();
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionString = ex.Message + " :: " + Environment.NewLine + ex.StackTrace;
            }

            if (exceptionString != null)
            {
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
