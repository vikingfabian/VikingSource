using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VikingEngine.SteamWrapping;
using VikingEngine.Input;

namespace VikingEngine.PJ.Display
{
    class PauseMenu
    {
        Graphics.Image iconBg, icon;
        Display.MenuButton exitGame, resume;
        VikingEngine.PJ.AbsPJGameState gamestate;
        Graphics.Image darkBg;
        public float buttonsY;
        public LostControllerDisplay lostControllerDisplay = null;

        public PauseMenu(int pIx, AbsPJGameState gamestate, Input.InputSource inputType)
        {
            this.gamestate = gamestate;

            darkBg = HudLib.DarkBgOverlay(HudLib.LayPopupBg);

            iconBg = new Graphics.Image(SpriteName.birdWhiteSoftBox, Engine.Screen.CenterScreen, 
                new Vector2(Engine.Screen.IconSize * 3f), HudLib.LayPopup, true);
            icon = new Graphics.Image(SpriteName.pauseSymbol, iconBg.Position, iconBg.Size * 0.8f, ImageLayers.AbsoluteBottomLayer, true);
            icon.LayerAbove(iconBg);

            Vector2 buttonsCenter = Engine.Screen.CenterScreen;
            buttonsCenter.Y += Engine.Screen.IconSize * 3f;

            Vector2 bigButtonsSize = new Vector2(Engine.Screen.IconSize * 2.4f);
            float buttonSideAdj = bigButtonsSize.X * 1.6f;

            float border = Engine.Screen.IconSize * 0.1f;

            exitGame = new MenuButton(VectorRect.FromCenterSize(VectorExt.AddX(buttonsCenter, -buttonSideAdj), bigButtonsSize), 
                false, SpriteName.pjForwardIcon, HudLib.LayPopup, HudLib.LargeButtonSettings);
            exitGame.iconImg.spriteEffects = SpriteEffects.FlipHorizontally;
              
            resume = new MenuButton(VectorRect.FromCenterSize(VectorExt.AddX(buttonsCenter, buttonSideAdj), bigButtonsSize), 
                true, SpriteName.pjPlayIcon, HudLib.LayPopup, HudLib.LargeButtonSettings);

            SpriteName exitButtonImage = inputType.sourceType == InputSourceType.XController? SpriteName.ButtonVIEW : SpriteName.KeyBack;
            SpriteName resumeButtonImage = inputType.sourceType == InputSourceType.XController ? SpriteName.ButtonMENU : SpriteName.KeyEsc;

            exitGame.createInputIcon(Dir4.W, exitButtonImage);
            resume.createInputIcon(Dir4.E, resumeButtonImage);

            buttonsY = exitGame.area.Y;
        }

        public bool Update()
        {
            int input_1Exit_2Resume = 0;

            if (lostControllerDisplay != null)
            {
                if (lostControllerDisplay.update())
                {
                    resume.Visible = true;
                    lostControllerDisplay.DelteMe();
                    lostControllerDisplay = null;
                }
            }

            foreach (var controller in Input.XInput.controllers)
            {
                if (controller.Connected)
                {
                    if (controller.KeyDownEvent(Buttons.Back))
                    {
                        input_1Exit_2Resume = 1;
                    }
                    if (controller.KeyDownEvent(Buttons.Start))
                    {
                        input_1Exit_2Resume = 2;
                    }
                }
            }
            

            if (exitGame.update() ||
                Input.Keyboard.KeyDownEvent(Keys.Back))
            {
                input_1Exit_2Resume = 1;
            }


            if (resume.update() ||
                    Input.Keyboard.KeyDownEvent(Keys.Enter) ||
                    Input.Keyboard.KeyDownEvent(Keys.Escape))
            {
                input_1Exit_2Resume = 2;
            }

            if (input_1Exit_2Resume == 1)
            {
                gamestate.exitToLobby();
            }
            else if (input_1Exit_2Resume == 2 &&
                resume.Visible)
            {
                return true;
            }

            return false;
        }

        public void createLostCtrlDispay()
        {
            lostControllerDisplay = new Display.LostControllerDisplay(this);
            resume.Visible = false;
        }

        public void hidePauseIcon()
        {
            iconBg.Visible = false;
            icon.Visible = false;
        }

        public void DeleteMe()
        {
            iconBg.DeleteMe();
            icon.DeleteMe();
            exitGame.DeleteMe();
            resume.DeleteMe();
            darkBg.DeleteMe();

            lostControllerDisplay?.DelteMe();
        }
    }
}
