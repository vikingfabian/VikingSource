using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.LootFest.Editor.Display
{
    class ColorPicker
    {
        const int Width = 10;
        Graphics.PixelImage pickSurface;
        Graphics.PixelImage hueBar;
        Graphics.Image hueBarSlider, pickSlider;
        Graphics.Image currentColor, prevColor;
        Vector3 hls;

        VectorRect pickArea, barArea;
        HUD.IconButton okButton, cancelButton;
        bool closeMe = false;

        public BlockHD result;

        public ColorPicker(BlockHD current, int player)
        {
            result = current;
            Input.Mouse.Visible = true;
            pickSurface = new Graphics.PixelImage(Engine.Screen.CenterScreen, new Vector2(Engine.Screen.Height * 0.24f), ImageLayers.Foreground7, false, new IntVector2(Width), true);
            pickSurface.Position -= pickSurface.Size * 0.5f;
            hls = lib.RGB2HSL(current.color);
            pickArea = pickSurface.Area;

            pickSlider = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, new Vector2(3), ImageLayers.AbsoluteBottomLayer, true);
            pickSlider.LayerAbove(pickSurface);

            const int HueTexHeight = 20;
            float barW = Engine.Screen.IconSize * 0.5f;
            hueBar = new Graphics.PixelImage(VectorExt.AddX(pickSurface.RightTop, barW), new Vector2(barW, pickSurface.Height), ImageLayers.Foreground8,
                false, new IntVector2(1, HueTexHeight), true);

            for (int y = 0; y < HueTexHeight; ++y)
            {
                double hue = 1.0 - (double)y / HueTexHeight;
                hueBar.pixelTexture.SetPixel(new IntVector2(0, y), lib.HSL2RGB(hue, 1.0, 0.5));
            }
            hueBar.pixelTexture.ApplyPixelsToTexture();

            hueBarSlider = new Graphics.Image(SpriteName.WhiteArea, hueBar.Center, new Vector2(hueBar.Width, 3), ImageLayers.AbsoluteBottomLayer, true);
            hueBarSlider.LayerAbove(hueBar);
            barArea = hueBar.Area;

            currentColor = new Graphics.Image(SpriteName.WhiteArea, VectorExt.AddX(hueBar.RightTop, barW), new Vector2(2, 1) * Engine.Screen.IconSize * 1.5f, ImageLayers.Foreground8);
            prevColor = currentColor.CloneMe() as Graphics.Image;
            prevColor.Ypos += currentColor.Height;

            refresh();
            HlsToSurfacePos();

            currentColor.Color = current.color;
            prevColor.Color = currentColor.Color;

            VectorRect buttonArea =  new VectorRect(VectorExt.AddX(currentColor.RightTop, barW), new Vector2(Engine.Screen.IconSize * 1.2f));
            okButton = new HUD.IconButton(SpriteName.LfCheckYes, 
                buttonArea,
                ImageLayers.Foreground8,
                LootFest.LfLib.ButtonGuiSettings);
            
            buttonArea.Y = prevColor.Ypos;
            cancelButton = new HUD.IconButton(SpriteName.LfCheckNo, 
                buttonArea,
                ImageLayers.Foreground8,
                LootFest.LfLib.ButtonGuiSettings);

            var map = Engine.XGuide.GetPlayer(player).inputMap as LootFest.Players.InputMap;
            Input.IButtonMap okMap;
            if (map.inputSource.HasKeyBoard)
            {
                okMap = new Input.KeyboardButtonMap(Microsoft.Xna.Framework.Input.Keys.Enter);
            }
            else
            {
                okMap = map.menuInput.click;//map.GetButton(Input.ButtonActionType.MenuClick);
            }

            okButton.addInputIcon(Dir4.E, okMap);
            okButton.clickAction = ok;
            cancelButton.addInputIcon(Dir4.E, map.menuInput.back);//.GetButton(Input.ButtonActionType.MenuBack));
            cancelButton.clickAction = cancel;
        }

        void ok()
        {
            result.SetColor(currentColor.Color);//.SetColor(currentColor.Color);
            closeMe = true;
        }
        void cancel()
        {
            closeMe = true;
        }

        public bool update()
        {
            if (Input.Mouse.IsButtonDown(MouseButton.Left))
            {
                if (pickArea.IntersectPoint(Input.Mouse.Position))
                {
                    Vector2 percPos = pickArea.PositionToPercent(Input.Mouse.Position);
                    double light, saturation;

                    surfacePosToLS(percPos.X, percPos.Y, out light, out saturation);
                    hls.Y = (float)light;
                    hls.Z = (float)saturation;

                    refresh();
                    pickSlider.Position = Input.Mouse.Position;
                }

                if (barArea.IntersectPoint(Input.Mouse.Position))
                {
                    hls.X = 1f - barArea.PositionToPercent(Input.Mouse.Position).Y;
                    refresh();
                }
            }

            okButton.update();
            cancelButton.update();

            return closeMe;
        }

        void refresh()
        {
            ForXYLoop loop = new ForXYLoop(new IntVector2(Width));

            while (loop.Next())
            {
                double percX = (double)loop.Position.X / Width;
                double percY = (double)loop.Position.Y / Width;

                double light, saturation;
                surfacePosToLS(percX, percY, out light, out saturation);
                
                Color col = lib.HSL2RGB(hls.X, saturation, light);
                pickSurface.pixelTexture.SetPixel(loop.Position, col);
            }

            pickSurface.pixelTexture.ApplyPixelsToTexture();

            hueBarSlider.Ypos = hueBar.Ypos + hueBar.Height * (1f - hls.X);

            currentColor.Color = lib.HSL2RGB(hls.X, hls.Z, hls.Y);
        }

        void surfacePosToLS(double percX, double percY, out double light, out double saturation)
        {
            light = 1.0 - percY;
            saturation = percX;

            light = light * (1.0 - percX) + 0.5 * light * percX;
        }

        void HlsToSurfacePos()
        {
            float percX = hls.Z;

            float percY = 1f - (hls.Y * 2f * percX + hls.Y * (1 - percX));

            percY = Bound.Set(percY, 0, 1f);

            pickSlider.Position = pickArea.PercentToPosition(new Vector2(percX, percY));
        }

        public void DeleteMe()
        {
            pickSurface.DeleteMe();
            hueBar.DeleteMe();
            hueBarSlider.DeleteMe(); 
            pickSlider.DeleteMe();
            currentColor.DeleteMe();
            prevColor.DeleteMe();
            okButton.DeleteMe();
            cancelButton.DeleteMe();
        }
    }
}
