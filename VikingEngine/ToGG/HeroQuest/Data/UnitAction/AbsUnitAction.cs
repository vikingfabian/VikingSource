using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data.Property;

namespace VikingEngine.ToGG.HeroQuest.Data.UnitAction
{
    abstract class AbsUnitAction : AbsUnitProperty
    {

        abstract public List<IntVector2> unitActionTargets(Unit unit);

        abstract public bool InstantAction { get; }

        abstract public bool Use(Unit unit, IntVector2 pos);

        virtual public void onUse(Unit unit)
        {
            unit.HqLocalPlayer.onUnitSkillUse();
            useCount?.Use();
        }

        virtual public bool Enabled(Unit unit)
        {
            return UseActionCountLeft(unit) > 0 && unitActionTargets(unit).Count > 0;
        }

        virtual public bool IsValidActionTarget(Unit unit, IntVector2 pos, 
            out List<IntVector2> groupSelection)
        {
            groupSelection = null;
            return false;
        }

        virtual public List<HUD.RichBox.AbsRichBoxMember> actionTargetToolTip()
        {
            return null;
        }

        virtual public int UseActionCountLeft(Unit unit)
        {
            return useCount.count;
        }
    }

    
}
