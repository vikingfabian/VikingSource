using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardEditor;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.CardDesign.CardData
{

    abstract class AbsAction
    {
        public Target target;

        public Number Xamount = new Number(1);

        virtual public void ToMenu(RichBoxContent content)
        {
            content.Add(new RbText(Xamount.ToString() + " " + Type.ToString() + " to " + target.Description()));
        }
        virtual public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu, bool fromUnit)
        {
            AmountToEditor(content, menu);
            
            content.newLine();
            if (target != null)
            {
                target.ToEditor(content, menu, fromUnit);
            }
            DSSWars.HudLib.Label(content, "Preview");
            content.space();
            ToMenu(content);
        }
        protected void AmountToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            new NumberEditor().DragButton(content, menu, "Amount", Number.EndlessPositiveBounds, AmountProperty);
        }

        int AmountProperty(object tag, bool set, int value)
        {
            if (set)
            {
                Xamount.value = value;
            }
            return Xamount.value;
        }
        //    content.newLine();
        //    DSSWars.HudLib.Label(content, "Amount");
        //    content.space();
        //    NumberDragButton.RbDragButtonGroup(content, new List<float> { 1f }, new DragButtonSettings(Number.Bounds, 1),
        //       Xamount.UiProperty, false);
        //}

        public abstract ActionType Type { get; }
    }

    class KeyWordAction : AbsAction, IHasId
    {
        Id id;

        string name;
        AbsAction action;
        //public Number x;
        public override ActionType Type => action.Type;
        public Id Id { get { return id; } }
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
        Resource resource;
        //DefaultResourceType resourceType = DefaultResourceType.Coin;
        public CollectAction()
        {

        }

        override public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu, bool fromUnit)
        {
            AmountToEditor(content, menu);
            content.newLine();

            //DropDownBuilder dropdown = new DropDownBuilder("resource");
            //{
            //    for (DefaultResourceType res = 0; res < DefaultResourceType.NUM_NONE; res++)
            //    {
            //        IconName.Resource(res, out SpriteName icon, out string name);
            //        dropdown.AddOption(icon, name, res == resourceType, false,
            //            new RbAction1Arg<DefaultResourceType>((DefaultResourceType type) => { resourceType = type; menu.CloseDropDown(); }, res), null);
            //    }

            //    dropdown.Build(content, SpriteName.NO_IMAGE, "Resource", menu);
            //}
            EditorLib.SelectGameTagMenu(content, menu, false, resource.id, 
                (id) => { resource.id = id; menu.CloseDropDown(); });

            content.newParagraph();

            DSSWars.HudLib.Label(content, "Preview");
            content.space();
            ToMenu(content);
        }

        public override void ToMenu(RichBoxContent content)
        {
            content.Add(new RbText("Collect " + resource.ToAmountNameString() /*+ Xamount.ToString() + " " + resourceType.ToString()*/));
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

        override public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu, bool fromUnit)
        {
            AmountToEditor(content, menu);
            content.newLine();

            ToEditorBase(content, menu, fromUnit);
        }

        protected void ToEditorBase(RichBoxContent content, HUD.RichMenu.RichMenu menu, bool fromUnit)
        {

            DropDownBuilder dropdown = new DropDownBuilder("property");
            {
                for (UnitPropertyType res = 0; res < UnitPropertyType.NUM_NONE; res++)
                {
                    dropdown.AddOption(res.ToString(), res == unitPropertyType, false,
                        new RbAction1Arg<UnitPropertyType>((type) => { unitPropertyType = type; menu.CloseDropDown(); }, res), null);
                }

                dropdown.Build(content, SpriteName.NO_IMAGE, "Property", menu);
            }

            content.newLine();
            target.ToEditor(content, menu, fromUnit);

            content.newParagraph();

            DSSWars.HudLib.Label(content, "Preview");
            content.space();
            ToMenu(content);
        }

        public override void ToMenu(RichBoxContent content)
        {
            content.Add(new RbText(Xamount.PlusMinusString() + " " + unitPropertyType.ToString() + " to " + target.Description()));
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

        override public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu, bool fromUnit)
        {
            ToEditorBase(content, menu, fromUnit);
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
        Keyword,
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
