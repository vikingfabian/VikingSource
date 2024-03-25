using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Input;
using VikingEngine.Network;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.DSSWars.Display
{

    class HSLColorArea
    {
        Vector3 hsl = Vector3.Zero;
        ColorPickArea huePick;
        ColorPickArea darkPick;

        Vector2 previewOffset;
        Graphics.Image preview, previewOutline;

        public HSLColorArea(InputMap input, PaintFlagState state) 
        {
            Vector2 pos = state.paintArea.RightTop;
            pos.X += Engine.Screen.SmallIconSize;
            float w = Math.Min(Engine.Screen.SafeArea.Right - pos.X, Engine.Screen.IconSize * 5f);

            VectorRect hueArea = new VectorRect(pos, new Vector2(w));
            VectorRect darkArea = hueArea;
            darkArea.nextAreaY(1, Engine.Screen.SmallIconSize);
            darkArea.Height = Engine.Screen.IconSize;

            huePick = new ColorPickArea(hueArea, true);
            darkPick = new ColorPickArea(darkArea, false);

            float previewOutlineW = 2;

            previewOutline = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Engine.Screen.SmallIconSizeV2, 
                ImageLayers.Foreground9_Back, true);
            preview = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Engine.Screen.SmallIconSizeV2 - new Vector2(previewOutlineW * 2),
                ImageLayers.Foreground9_Back, true);

            previewOffset = new Vector2(Engine.Screen.SmallIconSize * 0.7f);

            updatePreview(false, Vector2.Zero);
        }

        public void setColor(Color color)
        { 
            hsl = lib.RGB2HSL(color);

            huePick.SetPercPosition(new Vector2(hsl.X, 1f - hsl.Y));
            darkPick.SetPercPosition(new Vector2(hsl.Z, 0.5f));

        }

        void updatePreview(bool view, Vector2 pos)
        {
            preview.Visible = view;
            previewOutline.Visible = view;

            if (view)
            {
                preview.position = pos + previewOffset;
                previewOutline.position = preview.position;

                preview.Color = getColor();
            }
        }

        public Color getColor()
        {
            return lib.HSL2RGB(hsl.X, hsl.Y, hsl.Z);
        }

        public bool updateInput()
        {
            if (huePick.updateInput())
            {
                hsl.X = huePick.percPosition.X;
                hsl.Y = 1- huePick.percPosition.Y;

                updatePreview(true, huePick.PointerPos);
                return true;
            }
            if (darkPick.updateInput())
            {
                hsl.Z = darkPick.percPosition.X;
                updatePreview(true, darkPick.PointerPos);

                return true;
            }
            updatePreview(false, Vector2.Zero);

            return false;
        }

        class ColorPickArea
        {
            VectorRect area;
            bool keyDown = false;
            //Vector2 pos;
            Graphics.Image pointer;
            bool hueType;

            public Vector2 percPosition=Vector2.Zero;

            public ColorPickArea(VectorRect area, bool hueType)
            {
                this.area = area;
                this.hueType = hueType;
                
                Graphics.Image hue = new Graphics.Image(
                    hueType? SpriteName.TextureHueSaturation : SpriteName.TextureDarknessGradient,
                    area.Center, area.Size, ImageLayers.Lay1, true);

                pointer = new Graphics.Image(SpriteName.ColorPickerCircle,
                    area.Position, Engine.Screen.SmallIconSizeV2, ImageLayers.Lay1_Front, true);
            }

            public bool updateInput()
            {
                bool inPaintArea = area.IntersectPoint(Input.Mouse.Position);

                if (keyDown)
                {
                    if (Input.Mouse.ButtonUpEvent(MouseButton.Left))
                    {
                        keyDown = false;
                        return keyDown;
                    }
                }
                else if (inPaintArea && Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    keyDown = true;
                }

                if (keyDown)
                {
                    pointer.position = area.KeepPointInsideBound_Position(Input.Mouse.Position);

                    percPosition = area.PositionToPercent(pointer.position);
                }

                return keyDown;
            }

            public void SetPercPosition(Vector2 pos)
            {
                percPosition = pos;
                pointer.position = area.PercentToPosition(percPosition);
            }

            public Vector2 PointerPos => pointer.position;
        }
    }
}
