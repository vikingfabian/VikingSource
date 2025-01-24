using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD;

namespace VikingEngine.SteamWrapping
{
    class SteamBlueScreen : VikingEngine.DebugExtensions.BlueScreen
    {
        public SteamBlueScreen(string errorMessageDetailed) 
            : base()
        {
            logError(errorMessageDetailed); errorMessageDetailed = Engine.LoadContent.SteamVersion + errorMessageDetailed;


            Engine.StateHandler.ReplaceGamestate(this);

            if (Ref.main.criticalContentIsLoaded)
            {                
                GuiLayout layout = createMenu("Steam API error");
                {                    
                    if (PlatformSettings.PC_platform)
                    {
                        new GuiTextButton("Restart in OFFLINE mode", null, restartOffline, false, layout);
                        new GuiTextButton("Restart (try again)", null, restart, false, layout);
                        new GuiTextButton("Exit to desktop", null, exitToDash, false, layout);
                    }
                    else if (PlatformSettings.TargetPlatform == ReleasePlatform.Xbox)
                    {
                        new GuiIconTextButton(SpriteName.ButtonA, "RESTART", null, restart, false, layout);
                    }
                    new GuiLabel(Engine.LoadContent.CheckCharsSafety(errorMessageDetailed, menu.style.textFormat.Font), true, menu.style.textFormat, layout);
                }
                layout.End();
            }
            else
            {
                Engine.Draw.graphicsDeviceManager.ApplyChanges();
                Ref.main.Exit();
            }
        }

        void restartOffline()
        {
            Ref.steam.GoOffline();
            restart();
        }
    }

    abstract class AbsSteamException : System.Exception
    {
        public AbsSteamException(string message)
            : base(message)
        { }
    }

    class EmptyUser_SteamException : AbsSteamException
    {
        public EmptyUser_SteamException()
            : base("Steam user is empty")
        { }
    }
}
