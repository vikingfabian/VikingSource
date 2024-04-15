﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD;
using VikingEngine.ToGG.HeroQuest;
using VikingEngine.ToGG;
using Microsoft.Xna.Framework;

namespace VikingEngine.DSSWars.Display
{
    class GameMenuSystem:MenuSystem
    {
        bool gameWasPaused;
        bool localHost;
        Graphics.Image blackFade;
        public GameMenuSystem(InputMap input, bool localHost)
            :base (input, MenuType.InGame)
        { 
            this.localHost = localHost;
        }

        public bool update()
        {
            if (localHost)
            {
                if (Open)
                {
                    menuUpdate();
                    return true;
                }
                else
                {
                    if (input.Menu.DownEvent)
                    {
                        pauseMenu();
                    }
                }
            }
            return false;
        }

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

        void pauseMenu()
        { 
            openMenu();
            GuiLayout layout = new GuiLayout(DssRef.lang.GameMenu_Title, menu);
            {  
                new GuiTextButton(DssRef.lang.GameMenu_Resume, null, closeMenu, false, layout);
                
                Ref.gamesett.soundOptions(layout);
                new GuiSectionSeparator(layout);

                new GuiDialogButton(DssRef.lang.GameMenu_ExitGame, null, new GuiAction(DssRef.state.exit), false, layout);
            }
            layout.End();
        }

    }
}
