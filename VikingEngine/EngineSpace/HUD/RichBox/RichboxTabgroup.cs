using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.ToGG.HeroQuest.Data.Condition;

namespace VikingEngine.HUD.RichBox
{
    class RichboxTabgroup : AbsRichBoxMember
    {
        List<RichboxTabMember> members;
        public Image pointer;
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

            Vector2 pos = new Vector2(group.area.X -2, group.position.Y + group.lineSpacingHalf -2);
            pointer = new Image(SpriteName.WhiteArea, pos,
                new Vector2(group.boxWidth +4, 4), group.layer, false, group.addToRender);
            pointer.Color = group.settings.tabSelected.BgColor;
            group.Add(pointer);
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

        public RichboxTabMember(List<AbsRichBoxMember> content, AbsRbAction enterAction = null)
        {
            this.content = content;
            this.enabled = true;
            this.enter = enterAction;
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
            if (this.selected)
            {
                bgPointer.Color = group.settings.tabSelected.BgColor;
            }
            else
            {
                bgPointer.Color = group.settings.tabNotSelected.BgColor;
            }

            group.position.X += group.imageHeight * 0.3f;
        }

        
    }


}
