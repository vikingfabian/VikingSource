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
        public Image linePointer;
        public ArtTabgroup(List<ArtTabMember> members, int selected, Action<int> click, Action<int> enter = null, RbSoundProfile clickSound = null, RbSoundProfile hoverSound = null)
        {
            this.members = members;
            for (int i = 0; i < members.Count; i++)
            {
                members[i].initGroup(i, selected, click, enter, clickSound, hoverSound);
            }
        }

        public override void Create(RichBoxGroup group)
        {
            foreach (var m in members)
            {
                m.Create(group);
            }

            Vector2 pos = new Vector2(group.area.X - 2, group.position.Y + group.lineSpacingHalf - 2);
            linePointer = new Image(SpriteName.WhiteArea, pos,
                new Vector2(group.boxWidth + 4, 4), group.layer, false, group.addToRender);
            linePointer.Color = group.settings.tabSelected.BgColor;
            group.Add(linePointer);

            group.newLine(true, 0.5f);
        }

        public override void getButtons(List<AbsRbButton> buttons)
        {
            buttons.AddRange(members);
        }
    }

    class ArtTabMember : ArtButton
    {
        bool selected;

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

        }

        public override void onEnter()
        {
            enter?.actionTrigger();
        }
    }
}
