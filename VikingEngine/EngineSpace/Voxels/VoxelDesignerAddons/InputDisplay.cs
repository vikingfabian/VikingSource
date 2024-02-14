using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Voxels
{
    class InputDisplay
    {
        const ImageLayers Layer = ImageLayers.Background4;

        ToolBar mainBar, selectionBar, cameraBar;
        InputDisplayMember undo, colorpick;
        InputDisplayMember mouseTool;


        public InputDisplay(EditorInputMap input, HUD.MenuInputMap menuInput)
        {
            Vector2 toolIconSz = new Vector2((int)(Engine.Screen.IconSize * 0.8f));
            Vector2 startPos = Engine.Screen.SafeArea.RightTop;

            float totalW = toolIconSz.X * 2 + Engine.Screen.BorderWidth * 3;
            startPos.X -= totalW;
            startPos.Y += Engine.Screen.IconSize * 2f;

            SpriteName menuInputImage = input.useMouseInput ? menuInput.OpenCloseKeyBoard.Icon : menuInput.OpenCloseController.Icon;
            SpriteName menuToolImage = SpriteName.birdLobbyMenuButton;

            { //MAIN BAR
                Vector2 position = startPos;

                mainBar = new ToolBar(ref position, totalW);

                mainBar.members.Add(new InputDisplayMember(ref position, toolIconSz, menuInputImage, menuToolImage));

                if (input.useMouseInput)
                {
                    mouseTool = new InputDisplayMember(ref position, toolIconSz, input.mouseUseButton.Icon, SpriteName.IconBuildAdd);
                    mainBar.members.Add(mouseTool);
                    mainBar.members.Add(new InputDisplayMember(ref position, toolIconSz, SpriteName.KeyShift, SpriteName.Ydir));
                    mainBar.members.Add(new InputDisplayMember(ref position, toolIconSz, input.mouseToolMenu.Icon, SpriteName.EditorMouseToolsIcon));
                }
                else
                {
                    mainBar.members.Add(new InputDisplayMember(ref position, toolIconSz, input.draw.Icon, SpriteName.IconBuildAdd));
                    mainBar.members.Add(new InputDisplayMember(ref position, toolIconSz, input.erase.Icon, SpriteName.IconBuildRemove));
                    mainBar.members.Add(new InputDisplayMember(ref position, toolIconSz, input.select.Icon, SpriteName.IconBuildSelection));
                }
                colorpick = new InputDisplayMember(ref position, toolIconSz, input.colorPick.Icon, SpriteName.IconColorPick);
                mainBar.members.Add(colorpick);
                undo = new InputDisplayMember(ref position, toolIconSz, input.undo.Icon, SpriteName.Undo);
                mainBar.members.Add(undo);
                mainBar.members.Add(new InputDisplayMember(ref position, toolIconSz, input.toggleCameraMode.Icon, SpriteName.InterfaceIconCamera));

                mainBar.EndInit(position);
            }

            startPos.X -= totalW + Engine.Screen.BorderWidth * 2;
            { //SELECTION BAR
                Vector2 position = startPos;

                selectionBar = new ToolBar(ref position, totalW);

                selectionBar.members.Add(new InputDisplayMember(ref position, toolIconSz, menuInputImage, menuToolImage));
                selectionBar.members.Add(new InputDisplayMember(ref position, toolIconSz, input.useMouseInput? SpriteName.MouseButtonLeft : input.draw.Icon, SpriteName.IconBuildStamp));
                selectionBar.members.Add(new InputDisplayMember(ref position, toolIconSz, input.mirrorX.Icon, SpriteName.FlipHori));
                selectionBar.members.Add(new InputDisplayMember(ref position, toolIconSz, input.mirrorY.Icon, SpriteName.FlipVerti));

                selectionBar.members.Add(new InputDisplayMember(ref position, toolIconSz, input.previous.Icon, SpriteName.RotateCCW));
                selectionBar.members.Add(new InputDisplayMember(ref position, toolIconSz, input.next.Icon, SpriteName.RotateCW));

                selectionBar.members.Add(new InputDisplayMember(ref position, toolIconSz, input.useMouseInput ? SpriteName.MouseButtonRight : input.cancel.Icon, SpriteName.MissingImage));

                selectionBar.EndInit(position);
            }

            { //CAMERA BAR
                Vector2 position = startPos;

                cameraBar = new ToolBar(ref position, totalW);

                cameraBar.members.Add(new InputDisplayMember(ref position, toolIconSz, input.useMouseInput ? SpriteName.MouseAllDir : input.cameraXMoveY.Icon, SpriteName.CamAngleY));
                cameraBar.members.Add(new InputDisplayMember(ref position, toolIconSz, input.useMouseInput ? SpriteName.MouseScroll : input.moveXZ.Icon, SpriteName.CamZoom));

                cameraBar.EndInit(position);
            }

            selectionBar.setVisible(false);
            cameraBar.setVisible(false);
        }


        public void update(bool hasSelection, int undoCount, bool canPickColor, EditorInputMap input)
        {
            selectionBar.setVisible(hasSelection);
            mainBar.setEnabled(!hasSelection);

            cameraBar.setVisible(!hasSelection && input.toggleCameraMode.IsDown);

            if (mainBar.enabled)
            {
                undo.setEnabled(undoCount > 0);
                colorpick.setEnabled(canPickColor);
            }

            if (input.useMouseInput)
            {
                mouseTool.toolImage.SetSpriteName(MouseToolHUD.ToolIcon(input.mouseTool));
            }
        }

        public void DeleteMe()
        {
            mainBar.DeleteMe();
            selectionBar.DeleteMe();
        }

        class ToolBar
        {
            public bool visible = true;
            public bool enabled = true;
            Graphics.Image topBar, bg;
            public List<InputDisplayMember> members = new List<InputDisplayMember>();

            public ToolBar(ref Vector2 position, float totalW)
            {
                topBar = new Graphics.Image(SpriteName.WhiteArea, position, new Vector2(totalW, Engine.Screen.IconSize * 0.5f), Layer);
                topBar.Color = ColorExt.VeryDarkGray;

                position.Y = topBar.Bottom - 1;
                bg = new Graphics.Image(SpriteName.WhiteArea, position, new Vector2(totalW), Layer + 1);
                bg.Color = new Color(148, 144, 176);

                position += new Vector2(Engine.Screen.BorderWidth);
            }

            public void EndInit(Vector2 startPos)
            {
                bg.Height = startPos.Y - topBar.Bottom;
            }

            public void setEnabled(bool enabled)
            {
                if (this.enabled != enabled)
                {
                    this.enabled = enabled;
                    topBar.Opacity = enabled ? 1f : 0.3f;
                    bg.Opacity = enabled ? 1f : 0.6f;
                    foreach (var m in members)
                    {
                        m.setEnabled(enabled);
                    }
                }
            }

            public void setVisible(bool visible)
            {
                if (this.visible != visible)
                {
                    this.visible = visible;

                    topBar.Visible = visible;
                    bg.Visible = visible;
                    foreach (var m in members)
                    {
                        m.setVisible(visible);
                    }
                }
            }

            public void DeleteMe()
            {
                topBar.DeleteMe();
                bg.DeleteMe();
                foreach (var m in members)
                {
                    m.DeleteMe();
                }
            }
        }

        class InputDisplayMember
        {
            static readonly Color ToolBgCol = Color.LightGray;
            static readonly Color ToolBgDisabledCol = Color.Gray;

            const float DisableOpacity = 0.3f;

            public Graphics.Image toolImage, toolBg, inputImage;

            public InputDisplayMember(ref Vector2 position, Vector2 toolIconSz, SpriteName input, SpriteName tool)
            {
                inputImage = new Graphics.Image(input, position, toolIconSz, Layer);
                toolImage = new Graphics.Image(tool, position, toolIconSz, Layer);
                toolImage.Xpos += toolIconSz.X + Engine.Screen.BorderWidth;
                toolBg = new Graphics.Image(SpriteName.WhiteArea, toolImage.Position, toolImage.Size, Layer);
                toolBg.Color = ToolBgCol;
                toolBg.LayerBelow(toolImage);

                position.Y += toolIconSz.Y + Engine.Screen.BorderWidth * 2f;
            }

            public void setVisible(bool visible)
            {
                toolImage.Visible = visible;
                toolBg.Visible = visible;
                inputImage.Visible = visible;
            }

            public void setEnabled(bool enabled)
            {
                float op = enabled ? 1f : DisableOpacity;
                Color col = enabled ? Color.White : Color.DarkGray;

                inputImage.Color = col;
                inputImage.Opacity = op;

                toolImage.Color = col;
                toolImage.Opacity = op;

                toolBg.Color = enabled ? ToolBgCol : ToolBgDisabledCol;
            }

            public void DeleteMe()
            {
                toolImage.DeleteMe();
                toolBg.DeleteMe();
                inputImage.DeleteMe();
            }
        }
    }
}
