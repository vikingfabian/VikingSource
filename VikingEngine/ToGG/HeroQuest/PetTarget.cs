using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.Data;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest
{
    class PetTarget
    {
        public List<AbsUnit> targets = null;
        Unit unit;
        AbsUnitProperty targetProperty;

        public PetTarget(Unit unit)
        {
            this.unit = unit;
            targetProperty = ((AbsPet)unit.data).petTargetProperty;
        }

        public void collectAllTargets(IntVector2 pos)
        {
            if (targetProperty != null)
            {
                targets = targetProperty.collectTargetUnits(pos, unit);
            }
        }

        public bool hasTarget(AbsUnit target)
        {
            collectAllTargets(unit.squarePos);
            return targets.Contains(target);
        }
    }
}
