using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardEditor;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.PJ.CarBall;

namespace VikingEngine.CardDesign.CardData
{
    class Trigger
    {
        public EventType eventTrigger = EventType.TimeNever;
        public List<AbsAction> actionList = new List<AbsAction>(1);

        public void ToMenu(RichBoxContent content)
        {
            content.Add(new RbText(eventTrigger.ToString() + ":"));
            content.space();
            foreach (var action in actionList)
            {
                DSSWars.HudLib.BulletSeperationPoint(content);
                action.ToMenu(content);
            }
        }
        public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            DropDownBuilder dropdown = new DropDownBuilder("event");
            {
                for (EventType ev = 0; ev < EventType.NUM; ev++)
                {
                    dropdown.AddOption(ev.ToString(), eventTrigger == ev, false,
                        new RbAction1Arg<EventType>((eventType)=> {
                            menu.CloseDropDown();
                            eventTrigger = eventType; }, ev), null);
                }

                dropdown.Build(content, SpriteName.NO_IMAGE, "Event", menu);
            }

            content.newLine();
            DSSWars.HudLib.Label(content, "Add action");
            content.newLine();
            for (ActionType actionType = 0; actionType < ActionType.NUM; actionType++)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+" + actionType.ToString()) },
                    new RbAction1Arg<ActionType>(addAction, actionType)));
                content.newLine();
            }
            for (int i = 0; i < actionList.Count; i++)
            {
                actionList[i].ToMenu(content);
                content.space();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconSettings) },
                    new RbAction1Arg<int>((index) => { cref.current.editActionIndex = index; menu.menuStack.Add(EditorMenu.Menu_Action); }, i)));
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("X") },
                    new RbAction1Arg<int>((index) => { actionList.RemoveAt(index); }, i)));

                content.newLine();
            }
            content.newLine();

        }

        void addAction(ActionType actionType)
        {
            AbsAction newAction = null;
            switch (actionType)
            {
                case ActionType.Spawn:
                    newAction = new SpawnAction();
                    break;
                case ActionType.Heal:
                    newAction = new HealAction();
                    break;
                case ActionType.Collect:
                    newAction = new CollectAction();
                    break;
                case ActionType.Damage:
                    newAction = new DamageAction();
                    break;
                case ActionType.ChangeProperty:
                    newAction = new ChangePropertyAction();
                    break;
                case ActionType.ResetProperty:
                    newAction = new ResetPropertyAction();
                    break;
                case ActionType.LoseGame:
                    newAction = new LoseAction();
                    break;
                case ActionType.WinGame:
                    newAction = new WinAction();
                    break;

            }

            actionList.Add(newAction);
        }

    }


    enum EventType
    {
        TimeNever,
        TimeGameStart,

        TimeStartOfTurn,
        TimeEndOfTurn,
        TimeStartOfOpponentTurn,
        TimeEndOfOpponentTurn,

        FieldEnter,
        FieldDestroyed,

        BeforeAttack,
        AfterAttack,
        BeforeActivation,
        AfterActivation,
        AfterAction,

        DamageGiven,
        DamageRecieved,
        BattleDamageGiven,
        BattleDamageRecieved,
        HealRecieved,

        BeforeCollect,
        Collect,
        AfterCollect,

        NUM
    }

}
