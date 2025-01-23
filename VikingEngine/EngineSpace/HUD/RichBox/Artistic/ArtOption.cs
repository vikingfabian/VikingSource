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
}
