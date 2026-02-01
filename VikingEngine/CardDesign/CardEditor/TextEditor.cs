using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VikingEngine.CardDesign.CardData;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;

namespace VikingEngine.CardDesign.CardEditor
{
    class TextEditor
    {
        RichMenu menu;
        IHasText textEntity;
        TextType type;

        public TextEditor(IHasText entity, TextType type)
        {
            this.textEntity = entity;
            this.type = type;
        }

        public void ToEditor(RichBoxContent content, RichMenu menu, string defaultName)
        {
            this.menu = menu;
            Text name = textEntity.GetText(type);
            RbText text;
            if (name.IsEmpty && !string.IsNullOrEmpty(defaultName))
            {
                text = new RbText(defaultName, DSSWars.HudLib.SecondaryTextColor);
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
            new TextInputState(
                textEntity.GetText(type).ToString(), nameEditEvent, null);
        }
        void nameEditEvent(string result, object tag)
        {
            textEntity.SetText(type, new Text(result));
           
            menu.needRefresh = true;
        }
    }
}
