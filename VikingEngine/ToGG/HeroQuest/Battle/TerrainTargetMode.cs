using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG;

namespace VikingEngine.ToGG.HeroQuest
{
    class TerrainTargetMode
    {
        public AttackTargetCollection targets;

        public TerrainTargetMode(Unit attacker)
        {
            targets = new AttackTargetCollection(attacker, attacker.squarePos, false, true);
        }
    }

    class UnitActionTargetMode
    {
        Data.UnitAction.AbsUnitAction unitAction;


    }
}
