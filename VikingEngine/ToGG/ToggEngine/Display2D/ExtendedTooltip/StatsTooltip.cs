using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    abstract class AbsStatsTooltip : AbsExtToolTip
    {

    }

    class MovementTooltip : AbsStatsTooltip
    {
        public override SpriteName Icon => SpriteName.cmdStatsMove;
        public override string Title => "Movement";
        public override string Text => "How many squares the unit can move. Diagonal movement is allowed.";
    }

    class MeleePowerTooltip : AbsStatsTooltip
    {
        public override SpriteName Icon => SpriteName.cmdStatsMelee;
        public override string Title => "Melee attack power";
        public override string Text => "The number of " + LanguageLib.BattleDie + " the unit will roll during a melee attack";
    }

    class RangedPowerTooltip : AbsStatsTooltip
    {
        public override SpriteName Icon => SpriteName.cmdStatsRanged;
        public override string Title => "Ranged attack power";
        public override string Text => "The number of " + LanguageLib.BattleDie + " the unit will roll during a ranged attack";
    }

    class RangedLengthTooltip : AbsStatsTooltip
    {
        public override SpriteName Icon => SpriteName.cmdUnitCardRangeBg;
        public override string Title => "Ranged attack length";
        public override string Text => "Max distance for the ranged attack";
    }
}
