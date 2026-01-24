using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;

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

        }
    }

    abstract class AbsAction
    {
        public Target target;

        public int amount = 1;

        virtual public void ToMenu(RichBoxContent content)
        {
            content.Add(new RbText(Type.ToString() + " " + amount.ToString() + " to " + target.Description()));
        }
        virtual public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {

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
        public override ActionType Type => ActionType.Collect;
    }
    class ChangePropertyAction : AbsAction
    {
        protected UnitPropertyType unitPropertyType = UnitPropertyType.Attack;
        public ChangePropertyAction()
        {
            target = new Target();
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
        public override ActionType Type => ActionType.ResetProperty;
    }

    class SpawnAction : ChangePropertyAction
    {

        public SpawnAction()
        { }

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
