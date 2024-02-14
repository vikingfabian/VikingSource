using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data;

namespace VikingEngine.ToGG.HeroQuest.Data.UnitAction
{
    class RemoveTrap : AbsUnitAction
    {
        public RemoveTrap()
        {
            useCount = new SkillUseCounter(1);
        }

        public override bool InstantAction => true;

        public override List<IntVector2> unitActionTargets(Unit unit)
        {
            var traps = collectTraps(unit);

            List<IntVector2> result = new List<IntVector2>(traps.Count);

            foreach (var m in traps)
            {
                result.Add(m.position);
            }

            return result;
        }

        List<ToggEngine.GO.AbsTileObject> collectTraps(Unit unit)
        {
            List<ToggEngine.GO.AbsTileObject> result = new List<ToggEngine.GO.AbsTileObject>(8);
            foreach (var dir in IntVector2.Dir8Array)
            {
                var sq = toggRef.Square(unit.squarePos + dir);
                if (sq != null)
                {
                    var trap = sq.tileObjects.GetObject(ToggEngine.TileObjCategory.Trap);
                    if (trap != null)
                    {
                        result.Add(trap);
                    }
                }
            }

            return result;
        }

        public override bool Use(Unit unit, IntVector2 pos)
        {
            var traps = collectTraps(unit);
            if (traps.Count > 0)
            {
                foreach (var m in traps)
                {
                    if (m.Type == ToggEngine.TileObjectType.RougeTrap)
                    {
                        unit.PlayerHQ.add(new Gadgets.RougeTrapItem(), true, true);
                    }

                    m.takeDamage();
                }
                return true;
            }
            return false;
        }

        public override string Name => "Remove traps";

        public override SpriteName Icon => SpriteName.cmdRemoveTrap;

        public override UnitPropertyType Type => UnitPropertyType.Num_Non;

        public override string Desc => "Remove all adjacent traps";
    }
}
