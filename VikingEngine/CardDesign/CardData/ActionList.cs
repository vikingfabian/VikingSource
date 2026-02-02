using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardEditor;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.CardDesign.CardData
{
    class ActionList: List<AbsAction>
    {
        public ActionList(): base(1) 
        { 
        }

        public void StaticTriggerTitle(RichBoxContent content, string trigger)
        {
            DSSWars.HudLib.Label(content, "Trigger: " + trigger);
        }

        public void ToMenu(RichBoxContent content)
        {
            foreach (var action in this)
            {
                content.newLine();
                DSSWars.HudLib.BulletSeperationPoint(content);
                action.ToMenu(content);
            }
        }

        public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            DSSWars.HudLib.Label(content, "Add action");
            content.newLine();
            for (ActionType actionType = 0; actionType < ActionType.NUM; actionType++)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+" + actionType.ToString()) },
                    new RbAction1Arg<ActionType>(addAction, actionType)));
                content.newLine();
            }
            for (int i = 0; i < Count; i++)
            {
                this[i].ToMenu(content);
                content.space();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconSettings) },
                    new RbAction1Arg<int>((index) => { cref.current.editAction = this[index]; menu.menuStack.Add(EditorMenu.Menu_Action); }, i)));
                //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("X") },
                cHud.DeleteButton(content,
                    new RbAction1Arg<int>((index) => { RemoveAt(index); }, i));

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

            Add(newAction);
        }
    }
}
