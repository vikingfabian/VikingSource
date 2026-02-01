using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.CardDesign.Entity
{
    

    class CardEntity : AbsEntity, ICardContent
    {
        public ResourceList cost = new ResourceList();
        public AbsCardAction action;

        public CardEntity(CardActionType actionType)
            :base(true)
        {
            switch (actionType)
            {
                case CardActionType.FieldUnit:
                    CardActionFieldUnit cardActionFieldUnit = new CardActionFieldUnit();
                    FieldUnit unit = new FieldUnit(true);
                    cardActionFieldUnit.unitId = unit.id;
                    cref.current.game.unitTypes.Add(unit.id, unit);
                    action = cardActionFieldUnit;
                    break;
                case CardActionType.ActionTrigger:
                    action = new CardActionTrigger();
                    break;
            }

            cref.current.game.cards.Add(id, this);
        }
        public void toEditButton(RichBoxContent content)
        {

        }

        public CardContent CardContent => action.CardContent;
    }
}
