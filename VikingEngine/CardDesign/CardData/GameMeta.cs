using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.CardDesign.CardData
{
    class GameMeta : IHasText
    {
        public Id id;

        public Text name = Text.Empty;
        public Text description = Text.Empty;

        public int cardCount = 0;

        public GameMeta()
        {
            id = Id.CreateNew(false);
        }
        

        public Text GetText(TextType type) { return type == TextType.Name? name : description; }
        public void SetText(TextType type, Text name) 
        {
            if (type == TextType.Name)
            {
                this.name = name;
            }
            else
            {
                this.description = name;
            }
        }
    }
}
