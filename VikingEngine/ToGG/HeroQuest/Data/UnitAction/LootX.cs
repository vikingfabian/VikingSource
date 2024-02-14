using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data;

namespace VikingEngine.ToGG.HeroQuest.Data.UnitAction
{
    class LootX : AbsUnitAction
    {
        int lootRange;

        public LootX(int lootRange)
        {
            this.lootRange = lootRange;
            useCount = new SkillUseCounter(1);
        }

        public override bool InstantAction => true;

        ForXYLoop areaLoop(Unit unit)
        {
            Rectangle2 area = Rectangle2.FromCenterTileAndRadius(unit.squarePos, lootRange);
            area.SetTileBounds(toggRef.board.tileGrid.Area);
            return new ForXYLoop(area);
        }

        public override List<IntVector2> unitActionTargets(Unit unit)
        {
            List<IntVector2> result = new List<IntVector2>(16);
            var loop = areaLoop(unit);

            while (loop.Next())
            {
                var sq = toggRef.Square(loop.Position);
                if (sq != null)
                {
                    var objects = sq.objLoop();

                    while (objects.next())
                    {
                        if (objects.sel.IsLootable())
                        {
                            result.Add(loop.Position);
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public override bool Use(Unit unit, IntVector2 pos)
        {
            //unit.HqLocalPlayer.onStrategyAction();
            //useCount.Use();

            var loop = areaLoop(unit);

            while (loop.Next())
            {
                var sq = toggRef.Square(loop.Position);
                if (sq != null)
                {
                    var objects = sq.objLoop();

                    while (objects.next())
                    {
                        objects.sel.Loot(unit);
                    }
                }
            }

            return true;
        }

        public override string Name => "Loot" + lootRange.ToString();

        public override SpriteName Icon => SpriteName.cmdLootAction;

        public override UnitPropertyType Type => UnitPropertyType.Num_Non;

        public override string Desc => "Collect all loot within " + lootRange.ToString() + " squares";
    }
}
