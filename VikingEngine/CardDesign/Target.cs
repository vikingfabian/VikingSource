using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;

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
            string desc =  "target: " + side.ToString() + " " + type.ToString();
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
    }
}
