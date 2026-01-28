using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign.CardData
{
    abstract class AbsCardAction
    {

    }

    class CardActionTrigger : AbsCardAction
    {
        Name name = Name.Empty;
        AbsAction action = null;
    }
    class CardActionFieldUnit : AbsCardAction
    {
        Id unit = Id.Empty;
    }


    enum CardActionType
    {
        ActionTrigger,
        FieldUnit,
        NUM
    }
}
