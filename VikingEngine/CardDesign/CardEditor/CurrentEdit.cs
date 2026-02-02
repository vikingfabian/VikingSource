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
        public ResourcePool resourcePool = null;
        public CardPile cardPile = null;
        public PlayerSupply supply = null;
        public Trigger editTrigger = null;
        public AbsAction editAction = null;
        public bool editIsTag = false;
    }
}
