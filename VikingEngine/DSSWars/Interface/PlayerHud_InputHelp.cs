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
using VikingEngine.SteamWrapping;
using VikingEngine.ToGG.HeroQuest.Display;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Interface
{
    class PlayerHud_InputHelp
    {
        List<SpriteName> iconsBuffer = new List<SpriteName>(3);
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
                    HudLib.HeadDisplayWidth * 0.6f, HudLib.HeadDisplayWidth * (player.gameControls.input.inputSource.HasControllerInput? 0.76f : 0.54f));
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
            bool controllerMode = map.inputSource.ControllerMode;
            bool hasController = map.inputSource.HasControllerInput;
            bool mouse = map.inputSource.HasMouse;
            bool casual = player.profile.casualControls;
           
            switch (player.gameControls.inputHelpState)
            {
                case InputHelpState.Map:
                    input_buttonmap(map.mouseSelect, DssRef.lang.InputActionName_ControllerSelect);
                    if (controllerMode)
                    {
                        input_buttonmap(map.mouseOrder, DssRef.lang.Tutorial_MoveInput);
                        input_buttonmap(map.Controller_ObjectMenuToggle, DssRef.lang.InputActionName_ToggleMenu);
                        input_buttonmap(map.Controller_Faction, DssRef.lang.FactionSettings_Titel);
                    }
                    //input(controllerMode ? SpriteName.RightStick_UD : SpriteName.MouseScroll, DssRef.lang.Tutorial_ZoomInput);
                    input_directionmap(map.RbScroll(), DssRef.lang.Tutorial_ZoomInput);

                    if (!casual)
                    {
                        input_buttonmap(map.Build, DssRef.lang.InputActionName_Build);
                    }

                    if (hasController)
                    {
                        input_directionmap(map.cameraTiltUpSmooth, DssRef.lang.InputActionName_CameraTiltUp);
                        //content.newLine();
                        //content.Add(new RbImage(SpriteName.ButtonLT));
                        //content.space();
                        //content.Add(new RbText("+"));
                        //content.Add(new RbImage(SpriteName.RightStick));
                        //content.Add(new RbText(DssRef.lang.InputActionName_CameraTiltUp, HudLib.TitleColor_Action));

                        //input(map.Controller_SubTabRight.Icon, DssRef.lang.InputActionName_CameraTiltUp);
                    }
                    break;

                case InputHelpState.Army:
                    input_buttonmap(map.mouseSelect, DssRef.lang.Hud_Cancel);
                    input_buttonmap(map.mouseOrder, DssRef.lang.Tutorial_MoveInput);
                    if (controllerMode)
                    {
                        input_buttonmap(map.Controller_ObjectMenuToggle, DssRef.lang.InputActionName_ToggleMenu);
                    }
                    break;

                case InputHelpState.Menu:
                    input_buttonmap(map.mouseSelect, DssRef.lang.InputActionName_ControllerSelect);
                    if (controllerMode)
                    {
                        input_buttonmap(map.Controller_ObjectMenuToggle, DssRef.lang.InputActionName_ToggleMenu);
                    }
                    break;
                case InputHelpState.Build:
                    input_buttonmap(map.mouseSelect, DssRef.lang.Build_PlaceBuilding);
                    if (controllerMode)
                    {
                        input_buttonmap(map.Controller_ObjectMenuToggle, DssRef.lang.InputActionName_ToggleMenu);
                    }
                    break;

                case InputHelpState.CommandTarget:
                    input_buttonmap(map.mouseSelect, DssRef.lang.InputActionName_PlaceTarget);
                    input(map.cancelIcons().First(), DssRef.lang.Hud_Cancel);
                    break;
            }
            input_buttonmap(map.ToggleHudDetail, DssRef.lang.InputActionName_ToggleHudDetail);


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
                if (button.IsActive)
                {
                    content.newLine();
                    button.ToRichContent(content);
                    content.space();
                    content.Add(new RbText(text, HudLib.TitleColor_Action));
                }
            }

            void input_directionmap(IDirectionalMap dir, string text)
            {
                //if (dir)
                {
                    iconsBuffer.Clear();

                    content.newLine();
                    dir.ListIcons(iconsBuffer, out SpriteName plusKey, false);//.ToRichContent(content);

                    if (iconsBuffer.Count > 0)
                    {
                        if (iconsBuffer[0] == SpriteName.NO_IMAGE)
                        {
                            SteamInputManager.UnusedLayerToRichContent(content);
                        }
                        else
                        {
                            content.Add(new RbImage(iconsBuffer[0]));
                            if (plusKey != SpriteName.NO_IMAGE)
                            {
                                content.space();
                                content.Add(new RbText("+"));
                                content.Add(new RbImage(iconsBuffer[1]));
                            }
                        }
                    }
                    
                        content.space();
                        content.Add(new RbText(text, HudLib.TitleColor_Action));
                    
                }
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
