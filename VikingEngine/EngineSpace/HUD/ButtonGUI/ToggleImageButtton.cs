using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.HUD
{
    class ToggleImageButtton : AbsButtonGui
    {
        public Graphics.Image inputIcon;

        public ToggleImageButtton(SpriteName image1, SpriteName image2, ImageLayers layer, VectorRect area)
        {
            this.area = area;
            toggleHighLight = true;

            baseImage = new Graphics.Image(image1,
                area.Position, area.Size, layer, false);

            highlight = new Graphics.Image(image2,
                area.Position, area.Size, layer, false);

            highlight.Visible = false;
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            if (inputIcon != null)
            {
                inputIcon.DeleteMe();
            }
        }

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;

                if (inputIcon != null)
                {
                    inputIcon.Visible = value;
                }
            }
        }
    }
}
