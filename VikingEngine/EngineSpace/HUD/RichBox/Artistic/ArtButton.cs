using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.HUD.RichBox.Artistic
{
    enum RbButtonStyle
    { 
        Primary,
        Secondary,
        OptionSelected,
        OptionNotSelected,
        TabSelected,
        TabNotSelected,
    }

    class ArtButton : AbsRbButton
    {
        public static readonly Color MouseDownCol = Color.LightGray;
        RbButtonStyle buttonStyle;
        HUD.NineSplitAreaTexture texture;
        public ArtButton()
        { }

        public ArtButton(RbButtonStyle buttonStyle, List<AbsRichBoxMember> content, AbsRbAction click, AbsRbAction enter = null, bool enabled = true)
        {
            this.buttonStyle = buttonStyle;
            this.content = content;
            this.click = click;
            this.enter = enter;
            if (this.click != null)
            {
                this.click.enabled = enabled;
            }
            this.enabled = enabled;
        }

        override protected void createBackground(RichBoxGroup group, VectorRect area, ImageLayers layer)
        {
            NineSplitSettings textureSett;
            switch (buttonStyle)
            { 
                default:
                    textureSett = group.settings.artButtonTex;
                    break;
                case RbButtonStyle.Secondary:
                    textureSett = group.settings.artButtonTex.Selected(false);
                    break;
                case RbButtonStyle.OptionSelected:
                    textureSett = group.settings.artOptionButtonTex;
                    break;
                case RbButtonStyle.OptionNotSelected:
                    textureSett = group.settings.artOptionButtonTex.Selected(false);
                    break;
                case RbButtonStyle.TabSelected:
                    textureSett = group.settings.artTabTex;
                    break;
                case RbButtonStyle.TabNotSelected:
                    textureSett = group.settings.artTabTex.Selected(false);
                    break;
            }
            texture = new HUD.NineSplitAreaTexture(textureSett, area, layer);

            group.images.AddRange(texture.images);
        }

        override public void setGroupSelectionColor(RichBoxSettings settings, bool selected)
        {
            if (!selected)
            {
                //overrideBgColor = settings.buttonSecondary.BgColor;
            }
        }
        override public VectorRect area()
        {
            return texture.GetAreaAdjusted();
        }

        public override void clickAnimation(bool keyDown)
        {
            base.clickAnimation(keyDown);

            Color toCol = keyDown? MouseDownCol : Color.White;
            foreach (var img in texture.images)
            { 
                img.Color = toCol;
            }
        }
    }
}
