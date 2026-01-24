using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.PJ.CarBall;

namespace VikingEngine.CardDesign
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
                        new RbAction1Arg<EventType>((EventType eventType)=> {
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
                    new RbAction1Arg<int>((int index) => { CardRef.playState.editActionIndex = index; menu.menuStack.Add(CardDesignPlayState.Menu_Action); }, i)));
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("X") },
                    new RbAction1Arg<int>((int index) => { actionList.RemoveAt(index); }, i)));

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
            }

            actionList.Add(newAction);
        }

    }

    abstract class AbsAction
    {
        public Target target;

        public int amount = 1;

        virtual public void ToMenu(RichBoxContent content)
        {
            content.Add(new RbText(amount.ToString() + " " + Type.ToString() + " to " + target.Description()));
        }
        virtual public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            AmountToEditor(content);
            content.newLine();
            target.ToEditor(content, menu);

            DSSWars.HudLib.Label(content, "Preview");
            content.space();
            ToMenu(content);
        }
        protected void AmountToEditor(RichBoxContent content)
        {
            content.newLine();
            DSSWars.HudLib.Label(content, "Amount");
            content.space();
            RbDragButton.RbDragButtonGroup(content, new List<float> { 1f }, new DragButtonSettings(Const.Bounds, 1),
                AmountProperty, false);
        }
        public int AmountProperty(object tag, bool set, int value)
        {
            if (set)
            {
                this.amount = value;
            }
            return this.amount;
        }
        public abstract ActionType Type { get; }
    }
    class DamageAction : AbsAction
    {
        public DamageAction() 
        { 
            target = new Target();
        }
        public override ActionType Type => ActionType.Damage;
    }
    class HealAction : AbsAction
    {
        public HealAction()
        {
            target = new Target();
        }
        public override ActionType Type => ActionType.Heal;
    }
    class CollectAction : AbsAction
    {
        ResourceType resourceType = ResourceType.Coin;
        public CollectAction()
        {
            
        }

        override public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            AmountToEditor(content);
            content.newLine();

            DropDownBuilder dropdown = new DropDownBuilder("resource");
            {
                for (ResourceType res = 0; res < ResourceType.NUM_NONE; res++)
                {
                    IconName.Resource(res, out SpriteName icon, out string name);
                    dropdown.AddOption(icon, name, res == resourceType, false,
                        new RbAction1Arg<ResourceType>((ResourceType type) => { resourceType = type; menu.CloseDropDown(); }, res), null);
                }

                dropdown.Build(content, SpriteName.NO_IMAGE, "Resource", menu);
            }

            content.newParagraph();

            DSSWars.HudLib.Label(content, "Preview");
            content.space();
            ToMenu(content);
        }

        public override void ToMenu(RichBoxContent content)
        {
            content.Add(new RbText( "Collect " + amount.ToString() + " " + resourceType.ToString()));
        }
        public override ActionType Type => ActionType.Collect;
    }
    class ChangePropertyAction : AbsAction
    {
        protected UnitPropertyType unitPropertyType = UnitPropertyType.Attack;
        public ChangePropertyAction()
        {
            target = new Target();
        }

        override public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            AmountToEditor(content);
            content.newLine();

            ToEditorBase(content, menu);
        }

        protected void ToEditorBase(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            
            DropDownBuilder dropdown = new DropDownBuilder("property");
            {
                for (UnitPropertyType res = 0; res < UnitPropertyType.NUM_NONE; res++)
                {
                    dropdown.AddOption(res.ToString(), res == unitPropertyType, false,
                        new RbAction1Arg<UnitPropertyType>((UnitPropertyType type) => { unitPropertyType = type; menu.CloseDropDown(); }, res), null);
                }

                dropdown.Build(content, SpriteName.NO_IMAGE, "Property", menu);
            }

            content.newLine();
            target.ToEditor(content, menu);

            content.newParagraph();

            DSSWars.HudLib.Label(content, "Preview");
            content.space();
            ToMenu(content);
        }

        public override void ToMenu(RichBoxContent content)
        {
            content.Add(new RbText(TextLib.PlusMinus(amount) + " " + unitPropertyType.ToString() + " to " + target.Description()));
        }
        public override ActionType Type => ActionType.ChangeProperty;
    }

    class ResetPropertyAction : ChangePropertyAction
    {
        public ResetPropertyAction()
        {
            target = new Target();
            unitPropertyType = UnitPropertyType.Health;
        }

        override public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            ToEditorBase(content, menu);
        }

        public override void ToMenu(RichBoxContent content)
        {
            content.Add(new RbText("Reset" + " " + unitPropertyType.ToString() + " on " + target.Description()));
        }
        public override ActionType Type => ActionType.ResetProperty;
    }

    class SpawnAction : AbsAction
    {
        public SpawnAction()
        { }

        public override void ToMenu(RichBoxContent content)
        {
            content.Add(new RbText("Spawn X"));
        }

        public override ActionType Type => ActionType.Spawn;
    }

    enum ActionType
    { 
        Damage,
        Heal,
        Collect,
        ChangeProperty,
        ResetProperty,
        Spawn,
        NUM
    }

    enum EventType
    {
        TimeNever,

        TimeStartOfTurn,
        TimeEndOfTurn,
        TimeStartOfOpponentTurn,
        TimeEndOfOpponentTurn,

        FieldEnter,
        FieldDestroyed,

        BeforeAttack,
        AfterAttack,
        AfterActivation,
        AfterAction,

        DamageGiven,
        DamageRecieved,
        HealRecieved,

        BeforeCollect,
        Collect,
        AfterCollect,

        NUM
    }

}
