using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.HUD.RichBox;
using VikingEngine.Graphics;

namespace VikingEngine.HUD.RichBox.Artistic
{
    class ArtTabgroup : AbsRichBoxMember
    {
        List<ArtTabMember> members;
        public Image pointer;
        public ArtTabgroup(List<ArtTabMember> members, int selected, Action<int> click, Action<int> enter = null, RbSoundProfile clickSound = null, RbSoundProfile hoverSound = null, Color? overrideBgColor = null)
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

            Vector2 pos = new Vector2(group.area.X - 2, group.position.Y + group.lineSpacingHalf - 2);
            pointer = new Image(SpriteName.WhiteArea, pos,
                new Vector2(group.boxWidth + 4, 4), group.layer, false, group.addToRender);
            pointer.Color = group.settings.tabSelected.BgColor;
            group.Add(pointer);
        }



        public override void getButtons(List<AbsRbButton> buttons)
        {
            buttons.AddRange(members);
            //foreach (var m in members)
            //{
            //    buttons.Add(m);
            //}
        }
    }

    class ArtTabMember : ArtButton
    {
        bool selected;
        //int index;

        public ArtTabMember(List<AbsRichBoxMember> content, AbsRbAction enterAction = null)
            :base()
        {
            this.content = content;
            this.enabled = true;
            this.enter = enterAction;
        }

        public void initGroup(int index, int selectedIx, Action<int> click, Action<int> enter, RbSoundProfile clickSound, RbSoundProfile hoverSound)
        {
            this.selected = index == selectedIx;
            buttonStyle = selected? RbButtonStyle.TabSelected : RbButtonStyle.TabNotSelected;
          
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

            group.position.X += group.imageHeight * 0.3f;
        }

        public override void onEnter()
        {
            enter?.actionTrigger();
        }
    }
}
