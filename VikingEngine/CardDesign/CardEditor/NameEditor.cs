using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.PJ.CarBall;

namespace VikingEngine.CardDesign.CardEditor
{
    class NameEditor
    {
        RichMenu menu;
        IHasName entity;

        public NameEditor(IHasName entity)
        {
            this.entity = entity;
        }
        public void ToEditor(RichBoxContent content, RichMenu menu, string defaultName)
        {
            this.menu = menu;
            Name name = entity.GetName();
            RbText text;
            if (name.IsEmpty)
            {
                text = new RbText(defaultName, Color.Gray);
            }
            else
            {
                text = new RbText(name.ToString());
            }
            content.Add(new RbButton(new List<AbsRichBoxMember> {
                new RbImage(SpriteName.InterfaceTextInput),
                new RbSpace(),
                text }, new RbAction(beginEditName), null, true, Color.White));
        }

        public void beginEditName()
        {
            new TextInputState(entity.GetName().ToString(), nameEditEvent, null);
        }
        void nameEditEvent(string result, object tag)
        {
            entity.SetName(new Name(result));
            menu.needRefresh = true;
        }
    }
}
