using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.CardDesign
{

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
            content.Add(new RbText("Collect " + amount.ToString() + " " + resourceType.ToString()));
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

    class LoseAction : AbsAction
    {
        public LoseAction()
        { }

        public override void ToMenu(RichBoxContent content)
        {
            content.Add(new RbText("lose the game"));
        }

        public override ActionType Type => ActionType.LoseGame;
    }

    class WinAction : AbsAction
    {
        public WinAction()
        { }

        public override void ToMenu(RichBoxContent content)
        {
            content.Add(new RbText("win the game"));
        }

        public override ActionType Type => ActionType.WinGame;
    }



    enum ActionType
    {
        Damage,
        Heal,
        Collect,
        ChangeProperty,
        ResetProperty,
        Spawn,
        LoseGame,
        WinGame,
        NUM
    }
}
