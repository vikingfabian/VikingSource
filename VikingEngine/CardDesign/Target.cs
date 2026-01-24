using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.ToGG;

namespace VikingEngine.CardDesign
{
    //class TargetOptions
    //{
    //    public List<TargetOptions>
    //}

    class Target
    {
        public TargetType type = TargetType.Default;
        public TargetSide side = TargetSide.Enemy;
        public bool includeSelf = false;

        public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            DSSWars.HudLib.Label(content, "Target");
            content.newLine();
            DropDownBuilder dropdown = new DropDownBuilder("target type");
            {
                for (TargetType val = 0; val < TargetType.NUM; val++)
                {
                    dropdown.AddOption(val.ToString(), val == type, false,
                        new RbAction1Arg<TargetType>((TargetType value) => { type = value; menu.CloseDropDown(); }, val), null);
                }

                dropdown.Build(content, SpriteName.NO_IMAGE, "Type", menu);
            }

            content.newLine();
            DropDownBuilder sidedropdown = new DropDownBuilder("target side");
            {
                for (TargetSide val = 0; val < TargetSide.NUM; val++)
                {
                    sidedropdown.AddOption(val.ToString(), val == side, false,
                        new RbAction1Arg<TargetSide>((TargetSide value) => { side = value; menu.CloseDropDown(); }, val), null);
                }

                sidedropdown.Build(content, SpriteName.NO_IMAGE, "Type", menu);
            }
            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("including self") },
                selfProperty));

        }

        public bool selfProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                includeSelf = value;
            }
            return includeSelf;
        }

        public void ToAttackMenu(RichBoxContent content)
        {
            if (type != TargetType.Default &&
                side !=  TargetSide.Enemy)
            {
                content.Add(new RbText("Attack " + Description()));
                content.newLine();
            }
        }

        public string Description()
        {
            string desc =  "target: " + type.ToString() + " " + side.ToString();
            if (includeSelf)
            {
                desc += ", including self";
            }
            return desc;
        }
    }

    enum TargetType
    { 
        Default,
        Self,
        Any,
        Adjacent,
        Creature,
        Hero,
        Player,
        NUM
    }

    enum TargetPlacementType
    { 
        Selected,
        All,
        Random,
    }

    enum TargetSide
    { 
        Any,
        Friendly,
        Enemy,
        NUM
    }
}
