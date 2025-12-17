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
using VikingEngine.ToGG.HeroQuest.Display;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Interface
{
    class PlayerHud_InputHelp
    {
        public RichMenu menu;
        Graphics.Image bgTex;
        public PlayerHud_InputHelp(LocalPlayer player, float bottom)
        {
            createMenu(player, bottom);
        }

        public void createMenu(LocalPlayer player, float bottom)
        {
            if (menu == null)
            {
                var objectMenuArea = new VectorRect(0, 0,
                    HudLib.HeadDisplayWidth * 0.6f, HudLib.HeadDisplayWidth * 0.54f);
                objectMenuArea.X = player.playerData.view.safeScreenArea.Right - objectMenuArea.Width;
                objectMenuArea.Y = bottom - objectMenuArea.Height;

                menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(0), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
                bgTex = menu.addBackground_Flat(new Color(20, 37, 65), 0.4f);
            }
        }

        //public void refreshPosition(LocalPlayer player)
        //{
        //    deleteMenu();
        //}

        public void deleteMenu()
        {
            menu?.DeleteMe();
            bgTex?.DeleteMe();
            menu = null;
        }

        public void refreshUpdate(LocalPlayer player)
        {
            if (!player.hud.maximizedHud)
            {
                deleteMenu();
                return;
            }

            createMenu(player, player.hud.inputHelpBottom());

            var content = new RichBoxContent();
            InputMap map = player.gameControls.input;
            bool ct = map.inputSource.IsController;
            bool mouse = map.inputSource.HasMouse;
            bool casual = player.profile.casualControls;
           
            switch (player.gameControls.inputHelpState)
            {
                case InputHelpState.Map:
                    input(map.mouseSelect.Icon, DssRef.lang.InputActionName_ControllerSelect);
                    if (ct)
                    {
                        input(map.mouseOrder.Icon, DssRef.lang.Tutorial_MoveInput);
                        input(map.ControllerFocus.Icon, DssRef.lang.InputActionName_ToggleMenu);
                    }
                    input(ct ? SpriteName.RightStick_UD : SpriteName.MouseScroll, DssRef.lang.Tutorial_ZoomInput);

                    if (!casual)
                    {
                        input_buttonmap(map.Build, DssRef.lang.InputActionName_Build);
                    }

                    if (ct)
                    {
                        content.newLine();
                        content.Add(new RbImage(SpriteName.ButtonLT));
                        content.space();
                        content.Add(new RbText("+"));
                        content.Add(new RbImage(SpriteName.RightStick));
                        content.Add(new RbText(DssRef.lang.InputActionName_CameraTiltUp, HudLib.TitleColor_Action));

                        //input(map.Controller_SubTabRight.Icon, DssRef.lang.InputActionName_CameraTiltUp);
                    }
                    break;

                case InputHelpState.Army:
                    input(map.mouseSelect.Icon, DssRef.lang.Hud_Cancel);
                    input(map.mouseOrder.Icon, DssRef.lang.Tutorial_MoveInput);
                    if (ct)
                    {
                        input(map.ControllerFocus.Icon, DssRef.lang.InputActionName_ToggleMenu);
                    }
                    break;

                case InputHelpState.Menu:
                    input(map.mouseSelect.Icon, DssRef.lang.InputActionName_ControllerSelect);
                    if (ct)
                    {
                        input(map.ControllerFocus.Icon, DssRef.lang.InputActionName_ToggleMenu);
                    }
                    break;
                case InputHelpState.Build:
                    input(map.mouseSelect.Icon, DssRef.lang.Build_PlaceBuilding);
                    if (ct)
                    {
                        input(map.ControllerFocus.Icon, DssRef.lang.InputActionName_ToggleMenu);
                    }
                    break;

                case InputHelpState.CommandTarget:
                    input(map.mouseSelect.Icon, DssRef.lang.InputActionName_PlaceTarget);
                    input(map.cancelIcons().First(), DssRef.lang.Hud_Cancel);
                    break;
            }
            input(map.ToggleHudDetail.Icon, DssRef.lang.InputActionName_ToggleHudDetail);


            menu.Refresh(content, player.gameControls.controllerPointer);

            void input(SpriteName button, string text)
            {
                content.newLine();
                content.Add(new RbImage(button));
                content.space();
                content.Add(new RbText(text, HudLib.TitleColor_Action));
            }

            void input_buttonmap(IButtonMap button, string text)
            {
                content.newLine();
                button.ToRichContent(content);
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
        CommandTarget,

    }
}
