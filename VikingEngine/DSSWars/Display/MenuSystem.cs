using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;
using Microsoft.Xna.Framework;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars.Display
{
    class MenuSystem
    {
        public AbsDeleteable menuAttachment = null;

        public HUD.Gui menu = null;
        protected InputMap input;
        MenuType menuType;
        protected ImageLayers layer = ImageLayers.Foreground7;
        //public SelectionMenuState selectionMenuState = new SelectionMenuState();

        public MenuSystem(InputMap input, MenuType menuType)
        {
            this.menuType = menuType;
            this.input = input;
            // this.playerData = playerData;
        }

        public bool menuUpdate()
        {
            if (menu != null)
            {
                if (menu.Update() || input.Menu.DownEvent)//input.DownEvent(ButtonActionType.MenuReturnToGame))
                {
                    closeMenu();
                }

                return true;
            }

            return false;
        }

        virtual public void openMenu()
        {
            if (menu == null)
            {
                VectorRect area = Engine.Screen.SafeArea;//XGuide.GetPlayer(input.playerIndex).view.safeScreenArea;
                //if (area.Width <= 0)
                //{
                //    area = Engine.Screen.SafeArea;
                //}

                menu = new Gui(GuiStyle(menuType == MenuType.Lobby), 
                    area, 
                    menuType == MenuType.InGame? 0.5f : 0f,
                    layer, 
                    Input.InputSource.DefaultPC);
                menu.useAnyControllerInput = true;//menuType != MenuType.InGame;
                Input.Mouse.LockToScreenArea = false;
            }
        }

        public static GuiStyle GuiStyle(bool lobby)
        {
            var style = new GuiStyle(
                (Screen.PortraitOrientation ? Screen.Width : Screen.Height) * 0.61f, 5, SpriteName.LFMenuRectangleSelection);
            style.headBar = !lobby;
            style.openSound = SoundLib.click;
            style.closeSound = SoundLib.back;


            return style;
        }

        virtual public void closeMenu()
        {
            if (menu != null)
            {
                menu.DeleteMe();
                menu = null;

                menuAttachment?.DeleteMe();
                menuAttachment = null;
            }
        }

        public bool Open { get { return menu != null; } }

        public void multiplayerGameSpeedToMenu(GuiLayout layout)
        {
            var options = new List<GuiOption<float>>
            {
                new GuiOption<float>(1.0f),
                new GuiOption<float>(1.5f),
                new GuiOption<float>(2f),
                new GuiOption<float>(3f),
                new GuiOption<float>(4f),
            };

            new GuiOptionsList<float>(SpriteName.NO_IMAGE, DssRef.lang.Input_GameSpeed, options, multiplayerGameSpeedProperty, layout);
        }

        float multiplayerGameSpeedProperty(bool set, float value)
        {
            if (set)
            {
                DssRef.storage.multiplayerGameSpeed = value;
                Ref.SetGameSpeed(value);
                DssRef.storage.Save(null);
            }
            return DssRef.storage.multiplayerGameSpeed;
        }

        void debugMenu()
        {
            GuiLayout layout = new GuiLayout("Army options", menu);
            {
                new GuiTextButton("Pause Ai", null, debugPauseAi, false, layout);
                new GuiTextButton("Crash game", null, debugCrash, false, layout);

            }
            layout.End();

        }

        void debugCrash()
        {
            throw new Exception("Test crash");
        }
        void debugPauseAi()
        {
            StartupSettings.RunAI = !StartupSettings.RunAI;
        }

        void exitToMain()
        {
            new GameState.ExitGamePlay();
        }
        void exitGame()
        {
            Ref.update.exitApplication = true;
        }
    }

    enum MenuType
    { 
        Lobby,
        Editor,
        InGame,
    }
}
