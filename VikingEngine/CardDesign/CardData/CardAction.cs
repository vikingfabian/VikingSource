using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign.CardData
{
    abstract class AbsCardAction
    {
        //abstract public Name GetName();
        //abstract public void SetName(Name name);
    }

    class CardActionTrigger : AbsCardAction
    {
        Text name = Text.Empty;
        AbsAction action = null;

    }
    class CardActionFieldUnit : AbsCardAction
    {
        public Id unitId = Id.Empty;
    }


    enum CardActionType
    {
        ActionTrigger,
        FieldUnit,
        NUM
    }
}
