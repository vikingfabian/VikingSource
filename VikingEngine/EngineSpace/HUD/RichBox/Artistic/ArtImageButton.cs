using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD.RichBox.Artistic
{
    class ArtImageButton : AbsRbButton
    {
        VectorRect bgArea;
        public ArtImageButton(List<AbsRichBoxMember> content, AbsRbAction click, AbsRbAction enter = null, bool enabled = true)
        {
            this.content = content;
            this.click = click;
            this.enter = enter;
            if (this.click != null)
            {
                this.click.enabled = enabled;
            }
            this.enabled = enabled;
        }

        protected override void createBackground(RichBoxGroup group, VectorRect area, ImageLayers layer)
        {
            //no background
            this.bgArea = area;
        }
        override public VectorRect area()
        {
            return bgArea;
        }
        override public void setGroupSelectionColor(RichBoxSettings settings, bool selected)
        {
            if (!selected)
            {
                //overrideBgColor = settings.buttonSecondary.BgColor;
            }
        }
    }
}
