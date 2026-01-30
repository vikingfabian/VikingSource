using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardEditor;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;
using VikingEngine.PJ.CarBall;

namespace VikingEngine.CardDesign.CardData
{
    class CardContent : IHasText
    {
        public Text name = Text.Empty;
        public Text flavor = Text.Empty;
        public SpriteName image = SpriteName.MissingImage;

        public void toEditor(RichBoxContent content, RichMenu menu)
        {
            new TextEditor(this, TextType.Name).ToEditor(content, menu, "Name");

            content.newLine();
            new TextEditor(this, TextType.Flavor).ToEditor(content, menu, "Flavor text");
        }
        public Text GetName(TextType type) {
            switch (type)
            { 
                case TextType.Name:
                    return name;
                case TextType.Flavor:
                    return flavor;
                default:
                    throw new NotImplementedException();
            }
        }
        public void SetName(TextType type, Text name) 
        {
            switch (type)
            {
                case TextType.Name:
                    this.name = name;
                    break;
                case TextType.Flavor:
                    this.flavor = name;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

