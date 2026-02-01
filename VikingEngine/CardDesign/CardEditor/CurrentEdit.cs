using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;
using VikingEngine.CardDesign.Entity;

namespace VikingEngine.CardDesign.CardEditor
{
    class CurrentEdit
    {
        public GameDb game = null;
        public CardEntity card = null;
        public int editTriggerIndex = -1;
        public int editActionIndex = -1;
        public bool editIsTag = false;
    }
}
