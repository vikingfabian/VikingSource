using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.HUD.RichBox;
using VikingEngine.Graphics;
using VikingEngine.ToGG.HeroQuest.Data.Condition;

namespace VikingEngine.HUD.RichBox
{
    class RbTabgroup : AbsRichBoxMember
    {
        List<RbTabMember> members;
        public Image pointer;
        public RbTabgroup(List<RbTabMember> members, int selected, Action<int> click, Action<int> enter = null, RbSoundProfile clickSound = null, RbSoundProfile hoverSound = null, Color? overrideBgColor = null)
        {
            this.members = members;
            for (int i = 0; i < members.Count; i++)
            {   
                members[i].initGroup(i, selected, click, enter, clickSound, hoverSound, overrideBgColor);
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

        

        public override void getButtons(List<RbButton> buttons)
        {
           buttons.AddRange(members);
        }
    }

    class RbTabMember: RbButton
    {
        bool selected;
        //int index;

        public RbTabMember(List<AbsRichBoxMember> content, AbsRbAction enterAction = null)
        {
            this.content = content;
            this.enabled = true;
            this.enter = enterAction;
        }

        public void initGroup(int index, int selectedIx, Action<int> click, Action<int> enter, RbSoundProfile clickSound, RbSoundProfile hoverSound, Color? overrideBgColor)
        { 
            this.selected = index == selectedIx;
            this.overrideBgColor = overrideBgColor;

            if (click != null)
            {
                this.click = new RbAction1Arg<int>(click, index, clickSound);
            }
            if (enter != null)
            {
                this.enter = new RbAction1Arg<int>(enter, index, hoverSound);
            }
            else
            { 
                this.enter = new RbSoundAction(hoverSound);
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

        public override void onEnter()
        {
            enter?.actionTrigger();
        }
    }


}
