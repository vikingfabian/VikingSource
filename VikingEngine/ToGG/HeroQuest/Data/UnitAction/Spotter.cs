using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest.Display;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.Commander;

namespace VikingEngine.ToGG.HeroQuest.Data.UnitAction
{
    class Spotter : AbsUnitAction
    {
        public const int PetSpotterDistance = 2;

        public Spotter()
        {
            useCount = new SkillUseCounter(1);
        }

        public override bool InstantAction => true;

        public override List<IntVector2> unitActionTargets(Unit unit)
        {   
            var tunits = collectTargetUnits(unit.squarePos, unit);

            List<IntVector2> result = new List<IntVector2>(tunits.Count);
            foreach (var m in tunits)
            {
                result.Add(m.squarePos);
            }

            return result;
        }

        public override List<AbsUnit> collectTargetUnits(IntVector2 pos, AbsUnit parentUnit)
        {
            var targets = hqRef.players.opponentsInLOS(parentUnit, pos, PetSpotterDistance, true);
            return targets;
        }

        public override bool Use(Unit unit, IntVector2 pos)
        {
            var tunits = collectTargetUnits(unit.squarePos, unit);

            foreach (var m in tunits)
            {
                m.hq().condition.Set(Data.Condition.ConditionType.Spotted, 1,
                    false, true, true);
                //new Data.Condition.Spotted().apply(m, true);
            }

            return true;
        }

        public override void toDoList(HeroQuest.Unit parentUnit, List<AbsToDoAction> list)
        {
            list.Add(new ToDoUseSkill(parentUnit, this));
        }

        public override SpriteName Icon => SpriteName.cmdPetTargetGui;

        public override UnitPropertyType Type => UnitPropertyType.Spotter;

        public override string Desc => "Spots opponents within " + PetSpotterDistance.ToString() + " squares.";

        public override AbsExtToolTip[] DescriptionKeyWords()
        {
            return new AbsExtToolTip[] { new StatusEffectTooltip(new Condition.Spotted()) };
        }
    }
}
