using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Display
{
    class PlayerHud_InputHelp
    {
        public RichMenu menu;

        public PlayerHud_InputHelp(LocalPlayer player)
        {
            createMenu(player);
        }

        public void createMenu(LocalPlayer player)
        {
            if (menu == null)
            {
                var objectMenuArea = new VectorRect(0, 0,
                    HudLib.HeadDisplayWidth * 0.6f, HudLib.HeadDisplayWidth * 0.8f);
                objectMenuArea.X = player.playerData.view.safeScreenArea.Right - objectMenuArea.Width;
                objectMenuArea.Y = player.playerData.view.safeScreenArea.Bottom - objectMenuArea.Height;

                menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(0), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
                var bgTex = menu.addBackground_Flat(new Color(20, 37, 65), 0.4f);
            }
        }

        public void refreshUpdate(LocalPlayer player)
        {
            var content = new RichBoxContent();
            InputMap map = player.gameControls.input;
            bool ct = map.inputSource.IsController;
            bool mouse = map.inputSource.HasMouse;
           
            switch (player.gameControls.inputHelpState)
            {
                case InputHelpState.Map:
                    input(ct? map.mouseSelect.Icon : map.mouseSelect.Icon, DssRef.lang.InputActionName_ControllerSelect);
                    input(ct ? SpriteName.RightStick_UD : SpriteName.MouseScroll, DssRef.lang.Tutorial_ZoomInput);
                    break;

                case InputHelpState.Army:
                    input(ct ? map.mouseSelect.Icon : map.mouseSelect.Icon, DssRef.lang.Hud_Cancel);
                    input(map.mouseOrder.Icon, DssRef.lang.Tutorial_MoveInput);
                    if (ct)
                    {
                        input(map.ControllerFocus.Icon, DssRef.lang.InputActionName_ControllerFocus);
                    }
                    break;

                case InputHelpState.Menu:
                    input(ct ? map.mouseSelect.Icon : map.mouseSelect.Icon, DssRef.lang.InputActionName_ControllerSelect);
                    if (ct)
                    {
                        input(map.ControllerFocus.Icon, DssRef.lang.InputActionName_ControllerFocus);
                    }
                    break;
            }
            

            menu.Refresh(content, player.gameControls.controllerPointer);

            void input(SpriteName button, string text)
            {
                content.newLine();
                content.Add(new RbImage(button));
                content.space();
                content.Add(new RbText(text, HudLib.TitleColor_Action));
            }
        }
    }

    enum InputHelpState
    { 
        Map,
        Army,
        Menu,
        Build,

    }
}
