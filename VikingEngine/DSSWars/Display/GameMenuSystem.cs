using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD;
using VikingEngine.ToGG.HeroQuest;
using VikingEngine.ToGG;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Display.CutScene;

namespace VikingEngine.DSSWars.Display
{
    class GameMenuSystem:MenuSystem
    {
        bool gameWasPaused;
        //bool localHost;
        Graphics.Image blackFade;
        public GameMenuSystem()//, bool localHost)
            :base (new InputMap(Engine.XGuide.LocalHostIndex), MenuType.InGame)
        { 
            //this.localHost = localHost;
        }

        //public bool update()
        //{
        //    //if (localHost)
        //    //{
        //        if (Open)
        //        {
        //            menuUpdate();
        //            return true;
        //        }
        //        else
        //        {
        //            if (input.Menu.DownEvent)
        //            {
        //                pauseMenu();
        //            }
        //        }
        //    //}
        //    return false;
        //}

        public override void openMenu()
        {
            gameWasPaused = Ref.isPaused;
            Ref.SetPause(true);

            if (blackFade == null)
            {
                VectorRect area = Engine.Screen.Area;
                area.AddRadius(4);
                blackFade = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, layer + 5);
                blackFade.ColorAndAlpha(Color.Black, 0.4f);

            }
            base.openMenu();
        }

        public override void closeMenu()
        {   
            Ref.SetPause(gameWasPaused);
            blackFade?.DeleteMe();
            blackFade = null;
            base.closeMenu();
        }

        void watchEpilogue()
        {
            closeMenu();
            new CutScene.NightmarePrologue();
        }

        void saveGameState()
        {
            closeMenu();

            if (DssRef.state.cutScene == null)
            {
                new SaveScene(false);
            }
        }

        void saveAndExit()
        {
            closeMenu();

            if (DssRef.state.cutScene == null)
            {
                new SaveScene(true).ExitGame = true;
            }
            else
            {
                DssRef.state.exit();
            }
        }

        public void pauseMenu()
        { 
            openMenu();
            GuiLayout layout = new GuiLayout(DssRef.lang.GameMenu_Title, menu);
            {  
                new GuiTextButton(DssRef.lang.GameMenu_Resume, null, closeMenu, false, layout);
                new GuiTextButton(DssRef.lang.GameMenu_SaveState, DssRef.lang.GameMenu_SaveStateWarnings, saveGameState, false, layout);
                new GuiTextButton(DssRef.lang.GameMenu_WatchPrologue, null, watchEpilogue, false, layout);


                if (DssRef.state.IsLocalMultiplayer())
                {
                    multiplayerGameSpeedToMenu(layout);
                }
                
                Ref.gamesett.soundOptions(layout);
                new GuiSectionSeparator(layout);

                new GuiDialogButton(DssRef.lang.GameMenu_ExitGame, null, new GuiAction(saveAndExit), false, layout);
            }
            layout.End();
        }

        public void controllerLost()
        {
            pauseMenu();

        }


    }
}
