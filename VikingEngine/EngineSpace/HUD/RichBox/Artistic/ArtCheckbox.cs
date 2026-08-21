using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD.RichBox.Artistic
{
    class ArtCheckbox : ArtButton
    {
        BoolGetSet_Tag property;
        RbImage checkImage = null;
        SpriteName checkOn, checkOff;
        public object propertyTag = null;
        public ArtCheckbox(List<AbsRichBoxMember> buttonContent, BoolGetSet_Tag property, AbsRbAction enter=null)
            :base(RbButtonStyle.CheckBox, buttonContent, null, enter)
        {
            this.property = property;
            this.enabled = true;
        }

        public override void onClick(RichMenu.RichMenu menu)
        {
            bool value = !property.Invoke(propertyTag, false, false);
            property.Invoke(propertyTag, true, value);

#if DSS
            (value? DSSWars.SoundLib.option_select : DSSWars.SoundLib.option_deselect).Play();
#else
            (value ? PJ.SoundManager.option_select : PJ.SoundManager.option_deselect).Play();
#endif
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
                bool value = property.Invoke(propertyTag, false, false);

                checkImage = new RbImage(value ? checkOn : checkOff, 0.76f);
                checkImage.Create(group);
                group.carriage.position.X += 6;
            }
        }


    }
}
