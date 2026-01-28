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

        public int cardCount = 0;

        

        public Text GetName(TextType type) { return name; }
        public void SetName(TextType type, Text name) { this.name = name; }
    }
}
