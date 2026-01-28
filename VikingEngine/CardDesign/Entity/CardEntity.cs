using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;

namespace VikingEngine.CardDesign.Entity
{
    class CardEntity : AbsEntity
    {
        public ResourceList cost;
        public AbsCardAction action;
    }
}
