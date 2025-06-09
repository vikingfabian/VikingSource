using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.VoxelEditor
{
    class VoxelEditorInputHelp
    {
        public RichMenu menu;
        Graphics.Image bgTex;


        public void createMenu()
        {
            if (menu == null)
            {
                var objectMenuArea = new VectorRect(0, 0,
                    HudLib.HeadDisplayWidth * 0.6f, HudLib.HeadDisplayWidth * 0.5f);
                objectMenuArea.X = Engine.Screen.SafeArea.Right - objectMenuArea.Width;
                objectMenuArea.Y = Engine.Screen.SafeArea.Bottom - objectMenuArea.Height;

                menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(0), RichMenu.DefaultRenderEdge, HudLib.GUILayer, XGuide.LocalHost);
                bgTex = menu.addBackground_Flat(new Color(20, 37, 65), 0.4f);
            }
        }


        public void deleteMenu()
        {
            menu?.DeleteMe();
            bgTex?.DeleteMe();
            menu = null;
        }

        public void refreshUpdate(VoxelEditorInputState inputState, InputMap map)
        {
            if (inputState == VoxelEditorInputState.HideHud)
            {
                deleteMenu();
            }
            else
            {
                createMenu();

                var content = new RichBoxContent();

                switch (inputState)
                {
                    case VoxelEditorInputState.Editor:

                        break;
                }



                void input(SpriteName button, SpriteName action)
                {
                    content.newLine();
                    content.Add(new RbImage(button));
                    content.space();
                    content.Add(new RbImage(action));
                }

                menu.Refresh(content);


            }

            
            //bool ct = map.inputSource.IsController;
            //bool mouse = map.inputSource.HasMouse;

            //switch (player.gameControls.inputHelpState)
            //{
            //    case InputHelpState.Map:
            //        input(map.mouseSelect.Icon, DssRef.lang.InputActionName_ControllerSelect);
            //        input(ct ? SpriteName.RightStick_UD : SpriteName.MouseScroll, DssRef.lang.Tutorial_ZoomInput);
            //        input(map.Build.Icon, DssRef.lang.InputActionName_Build);
            //        break;

            //    case InputHelpState.Army:
            //        input(ct ? map.mouseSelect.Icon : map.mouseSelect.Icon, DssRef.lang.Hud_Cancel);
            //        input(map.mouseOrder.Icon, DssRef.lang.Tutorial_MoveInput);
            //        if (ct)
            //        {
            //            input(map.ControllerFocus.Icon, DssRef.lang.InputActionName_ToggleMenu);
            //        }
            //        break;

            //    case InputHelpState.Menu:
            //        input(ct ? map.mouseSelect.Icon : map.mouseSelect.Icon, DssRef.lang.InputActionName_ControllerSelect);
            //        if (ct)
            //        {
            //            input(map.ControllerFocus.Icon, DssRef.lang.InputActionName_ToggleMenu);
            //        }
            //        break;
            //    case InputHelpState.Build:
            //        input(map.mouseSelect.Icon, DssRef.lang.Build_PlaceBuilding);
            //        if (ct)
            //        {
            //            input(map.ControllerFocus.Icon, DssRef.lang.InputActionName_ToggleMenu);
            //        }
            //        break;
            //}
            //input(map.ToggleHudDetail.Icon, DssRef.lang.InputActionName_ToggleHudDetail);


            //menu.Refresh(content, player.gameControls.controllerPointer);

            //void input(SpriteName button, string text)
            //{
            //    content.newLine();
            //    content.Add(new RbImage(button));
            //    content.space();
            //    content.Add(new RbText(text, HudLib.TitleColor_Action));
            //}
        }
    }

    enum VoxelEditorInputState
    { 
        HideHud,
        Editor,
        Selection,
        Menu,
    }
}
