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
    }

    class CardActionTrigger : AbsCardAction
    {
        public CardContent cardContent = new CardContent();

        AbsAction action = null;

        public override void toEditor(RichBoxContent content, RichMenu menu)
        {
            
        }
        public override CardContent CardContent => cardContent;

    }
    class CardActionFieldUnit : AbsCardAction
    {
        public Id unitId = Id.Empty;

        public override void toEditor(RichBoxContent content, RichMenu menu)
        {
            var unit = GameDb.Current.unitTypes[unitId];

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
                unit.eventTriggers[i].ToMenu(content);
                content.space();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconSettings) },
                    new RbAction1Arg<int>((int index) => { editTriggerIndex = index; menu.menuStack.Add(Menu_Trigger); }, i)));
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("X") },
                    new RbAction1Arg<int>((int index) => { card.eventTriggers.RemoveAt(index); }, i)));

                content.newLine();
            }
        }
        public override CardContent CardContent => GameDb.Current.unitTypes[unitId].cardContent;
    }


    enum CardActionType
    {
        ActionTrigger,
        FieldUnit,
        NUM
    }
}
