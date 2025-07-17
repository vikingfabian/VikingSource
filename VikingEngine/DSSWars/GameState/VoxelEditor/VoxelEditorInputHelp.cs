using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Interface;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.HeroQuest.Display;

namespace VikingEngine.DSSWars.GameState.VoxelEditor
{
    class VoxelEditorInputHelp
    {
        public RichMenu menu;
        Graphics.Image bgTex;


        public void createMenu()
        {
            if (menu == null)
            {
                var objectMenuArea = new VectorRect(0, Screen.SafeArea.Y,
                    HudLib.HeadDisplayWidth * 0.6f, Screen.SafeArea.Height);
                objectMenuArea.X = Screen.SafeArea.Right - objectMenuArea.Width;
                //objectMenuArea.Y = Screen.SafeArea.Bottom - objectMenuArea.Height;

                menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(0), RichMenu.DefaultRenderEdge, HudLib.IngameUiLayer, XGuide.LocalHost);
                bgTex = menu.addBackground_Flat(new Color(20, 37, 65), 0.4f);
            }
        }


        public void deleteMenu()
        {
            menu?.DeleteMe();
            bgTex?.DeleteMe();
            menu = null;
        }

        public void refreshUpdate(VoxelDesigner designer, VoxelEditorInputState inputState, InputMap map)
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
                        input(map.Menu.Icon, SpriteName.WarsHudHeadBarMenuIcon);
                        input(map.editorInput.draw.Icon, SpriteName.IconBuildAdd);
                        input(map.editorInput.erase.Icon, SpriteName.IconBuildRemove);
                        input(map.editorInput.select.Icon, SpriteName.IconBuildSelection);
                        input(map.editorInput.colorPick.Icon, SpriteName.IconColorPick);
                        input(map.editorInput.undo.Icon, SpriteName.Undo);
                        input(map.editorInput.toggleCameraMode.Icon, SpriteName.InterfaceIconCamera);

                        if (designer.voxelProject.HaveAnimation)
                        {
                            input(map.editorInput.next.Icon, SpriteName.VoxelEditorFrameNext);
                            input(map.editorInput.previous.Icon, SpriteName.VoxelEditorFramePrevious);
                        }
                        break;
                    case VoxelEditorInputState.Selection:
                        content.newLine();
                        content.Add(new RbImage(map.Menu.Icon));
                        content.space();
                        content.Add(new RbText(DssRef.lang.Editor_SelectOptionsMenu));
                            
                        input(map.editorInput.draw.Icon, SpriteName.IconBuildStamp);
                        input(map.editorInput.mirrorX.Icon, SpriteName.FlipHori);
                        input(map.editorInput.mirrorY.Icon, SpriteName.FlipVerti);
                        input(map.editorInput.previous.Icon, SpriteName.RotateCCW);
                        input(map.editorInput.next.Icon, SpriteName.RotateCW);
                        input(map.inputSource.HasMouse ? SpriteName.MouseButtonRight : map.editorInput.cancel.Icon, SpriteName.WarsHudIconExit);
                        break;

                    case VoxelEditorInputState.Camera:
                        input(map.inputSource.HasMouse ? SpriteName.MouseAllDir : map.editorInput.cameraXMoveY.Icon, SpriteName.CamAngleY);
                        input(map.inputSource.HasMouse ? SpriteName.MouseScroll : map.editorInput.moveXZ.Icon, SpriteName.CamZoom);
                        break;

                    case VoxelEditorInputState.Menu:
                        input(map.Menu.Icon, SpriteName.WarsHudIconExit);
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
        NONE,
        HideHud,
        Editor,
        Camera,
        Selection,
        Menu,
    }
}
