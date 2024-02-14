using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    abstract class AbsToolTip : Graphics.ImageGroupParent2D
    {
        protected const float BgOpacity = 0.7f;
        protected const ImageLayers Layer = ImageLayers.Lay3;
        protected Vector2 StandardOffset = new Vector2(Engine.Screen.IconSize * 0.5f, Engine.Screen.IconSize * 0.2f);
        Vector2 offset;
        protected MapControls mapControls;
        protected VectorRect area;

        public AbsToolTip(MapControls mapControls)
        {
            this.mapControls = mapControls;
        }

        protected void AddRichBox(List<AbsRichBoxMember> members,
            bool frame = true, bool complete = true)
        {
            RichBoxGroup richBox = new RichBoxGroup(Vector2.Zero, HudLib.ToolTipWidth, Layer,
                HudLib.MouseTipRichBoxSett, members);

            Add(richBox);

            area = richBox.maxArea;

            if (frame)
            {
                createFrame(area);

                if (complete)
                {
                    completeSetup(area.Size);
                }
            }
        }

        protected void completeSetup(Vector2 size)
        {
            area.Size = size;
            mapControls.removeToolTip();
            mapControls.tooltip = this;
            offset = StandardOffset;
            update();
        }

        public void update()
        {
            if (Input.Mouse.Position.X + StandardOffset.X + area.Size.X > Engine.Screen.Width)
            {
                offset.X = -(StandardOffset.X + area.Width);
            }
            else
            {
                offset.X = StandardOffset.X;
            }

            ParentPosition = Input.Mouse.Position + offset;
        }

        protected void createFrame(VectorRect area)
        {
            const float Opacity = 0.6f;

            area.AddRadius(Engine.Screen.BorderWidth);
            Graphics.RectangleLines frame = new Graphics.RectangleLines(area, 2, 1, Layer - 1);
            
            Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, Layer +1);

            if (Available)
            {
                frame.setColor(new Color(205, 193, 175), Opacity);
                bg.ColorAndAlpha(new Color(12, 18, 23), Opacity);
            }
            else
            {
                frame.setColor(new Color(201,141,141), Opacity);
                bg.ColorAndAlpha(new Color(71,13,13), Opacity);
            }

            Add(frame);
            Add(bg);
        }

        public void remove()
        {
            mapControls.removeToolTip();
        }

        virtual protected bool Available => true;
    }
}
