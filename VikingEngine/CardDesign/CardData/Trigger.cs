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
        public ActionList actionList = new ActionList();

        public void ToMenu(RichBoxContent content)
        {
            //content.Add(new RbText(eventTrigger.ToString() + ":"));
            EventTriggerToMenu(content);
            content.space();
            actionList.ToMenu(content);
            //foreach (var action in actionList)
            //{
            //    content.newLine();
            //    DSSWars.HudLib.BulletSeperationPoint(content);
            //    action.ToMenu(content);
            //}
        }

        public void EventTriggerToMenu(RichBoxContent content)
        {
            content.Add(new RbText(eventTrigger.ToString() + ":"));
            
        }
        //public void ActionsToMenu(RichBoxContent content)
        //{   
        //    foreach (var action in actionList)
        //    {
        //        content.newLine();
        //        DSSWars.HudLib.BulletSeperationPoint(content);
        //        action.ToMenu(content);
        //    }
        //}
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
            actionList.ToEditor(content, menu);

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
