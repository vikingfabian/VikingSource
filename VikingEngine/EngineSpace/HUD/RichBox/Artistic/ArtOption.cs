using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD.RichBox.Artistic
{
    class ArtOption : ArtButton
    {
        public ArtOption(bool selected, List<AbsRichBoxMember> content, AbsRbAction click, AbsRbAction enter = null, bool enabled = true)
            :base(selected? RbButtonStyle.OptionSelected : RbButtonStyle.OptionNotSelected, content, click, enter, enabled)
        {
        }

        protected override void createPreContent(RichBoxGroup group)
        {   
            var checkImage = new RbImage(buttonStyle == RbButtonStyle.OptionNotSelected ? group.settings.optionOff : group.settings.optionOn, 0.76f);
            checkImage.Create(group);
            group.position.X += 4;            
        }
    }

    class ArtToggle : ArtButton
    {
        public ArtToggle(bool selected, List<AbsRichBoxMember> content, AbsRbAction click, AbsRbAction enter = null, bool enabled = true)
            : base(selected ? RbButtonStyle.ToggleSelected : RbButtonStyle.ToggleNotSelected, content, click, enter, enabled)
        {
        }
        //protected override void createBackground(RichBoxGroup group, VectorRect area, ImageLayers layer)
        //{
        //    base.createBackground(group, area, layer);
        //    texture.SetColor(Color.DarkGray);
        //}
        //protected override void createPreContent(RichBoxGroup group)
        //{
        //    var checkImage = new RbImage(buttonStyle == RbButtonStyle.OptionNotSelected ? group.settings.optionOff : group.settings.optionOn, 0.76f);
        //    checkImage.Create(group);
        //    group.position.X += 4;
        //}
    }
}
