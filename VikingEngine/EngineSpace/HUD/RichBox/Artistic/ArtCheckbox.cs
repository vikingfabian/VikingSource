using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD.RichBox.Artistic
{
    class ArtCheckbox : ArtButton
    {
        BoolGetSet property;
        RbImage checkImage = null;
        SpriteName checkOn, checkOff;
        public ArtCheckbox(List<AbsRichBoxMember> content, BoolGetSet property)
            :base(RbButtonStyle.CheckBox, content, null)
        {
            this.property = property;
            this.enabled = true;
        }

        public override void onClick()
        {
            bool value = !property.Invoke(0, false, false);
            property.Invoke(0, true, value);

            if (checkImage != null)
            {
                checkImage.pointer.SetSpriteName(value ? checkOn : checkOff);
            }
        }

        protected override void createPreContent(RichBoxGroup group)
        {
            this.checkOn = group.settings.checkOn;
            this.checkOff = group.settings.checkOff;

            if (this.property != null)
            {
                bool value = property.Invoke(0, false, false);

                checkImage = new RbImage(value ? checkOn : checkOff);
                checkImage.Create(group);
                group.position.X += 4;
            }
        }


    }
}
