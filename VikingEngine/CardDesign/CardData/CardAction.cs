using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardEditor;
using VikingEngine.CardDesign.Entity;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.PJ.CarBall;

namespace VikingEngine.CardDesign.CardData
{
    interface ICardContent
    {
        CardContent CardContent { get; }
    }

    abstract class AbsCardAction : ICardContent
    {
        //abstract public Name GetName();
        //abstract public void SetName(Name name);
        abstract public void toEditor(RichBoxContent content, RichMenu menu);
        
        abstract public CardContent CardContent { get; }

        abstract public CardActionType ActionType { get; }

        virtual public FieldUnit GetUnit() { return null; }
    }

    class CardActionTrigger : AbsCardAction
    {
        public CardContent cardContent = new CardContent();

        AbsAction action = null;

        public override void toEditor(RichBoxContent content, RichMenu menu)
        {
            
        }
        public override CardContent CardContent => cardContent;

        public override CardActionType ActionType => CardActionType.ActionTrigger;

    }
    class CardActionFieldUnit : AbsCardAction
    {
        public Id unitId = Id.Empty;

        public override void toEditor(RichBoxContent content, RichMenu menu)
        {
            var unit = GetUnit();

            DSSWars.HudLib.Label(content, "Properties");
            content.space();
            unit.unitProperties.ToMenu(content);
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconSettings) },
                new RbAction(() => { menu.menuStack.Add(EditorMenu.Menu_UnitProperties); })));

            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Add trigger") },
               new RbAction(() => { unit.eventTriggers.Add(new Trigger()); })));
            content.newLine();

            for (int i = 0; i < unit.eventTriggers.Count; i++)
            {
                var trigger = unit.eventTriggers[i];
                trigger.EventTriggerToMenu(content);
                content.space();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconSettings) },
                    new RbAction1Arg<int>((int index) => { cref.current.editTrigger = unit.eventTriggers[index]; menu.menuStack.Add(EditorMenu.Menu_Trigger); }, i)));
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("X") },
                    new RbAction1Arg<int>((int index) => { cref.current.card.action.GetUnit().eventTriggers.RemoveAt(index); }, i)));

                trigger.ActionsToMenu(content);
                content.Add(new RbSeperationLine());
                content.newLine();
            }
        }
        public override FieldUnit GetUnit()
        {
            return cref.current.game.unitTypes[unitId];
        }
        public override CardContent CardContent => cref.current.game.unitTypes[unitId].cardContent;

        public override CardActionType ActionType => CardActionType.FieldUnit;

        
    }


    enum CardActionType
    {
        ActionTrigger,
        FieldUnit,
        NUM
    }
}
