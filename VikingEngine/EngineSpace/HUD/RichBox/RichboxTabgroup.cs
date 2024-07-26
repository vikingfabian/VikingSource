using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD.RichBox
{
    class RichboxTabgroup : AbsRichBoxMember
    {
        List<RichboxTabMember> members;

        public RichboxTabgroup(List<RichboxTabMember> members, int selected, Action<int> click, Action<int> enter = null, Color? overrideBgColor = null)
        {
            this.members = members;
            for (int i = 0; i < members.Count; i++)
            {   
                members[i].initGroup(i, selected, click, enter, overrideBgColor);
            }
        }

        public override void Create(RichBoxGroup group)
        {
            foreach (var m in members)
            {
                m.Create(group);
            }        
        }

        public override void getButtons(List<RichboxButton> buttons)
        {
           buttons.AddRange(members);
        }
    }

    class RichboxTabMember: RichboxButton
    {
        bool selected;
        //int index;

        public RichboxTabMember(List<AbsRichBoxMember> content)
        {
            this.content = content;
            this.enabled = true;
        }

        public void initGroup(int index, int selectedIx, Action<int> click, Action<int> enter, Color? overrideBgColor)
        { 
            this.selected = index == selectedIx;
            this.overrideBgColor = overrideBgColor;

            if (click != null)
            {
                this.click = new RbAction1Arg<int>(click, index);
            }
            if (enter != null)
            { 
                this.enter = new RbAction1Arg<int>(enter, index); 
            }
        }

        public override void Create(RichBoxGroup group)
        {
            base.Create(group);
            if (!this.selected)
            {
                bgPointer.Color = group.settings.buttonSecondary.BgColor;
            }
        }

        
    }


}
