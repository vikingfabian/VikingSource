using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    static class AreaFilter
    {
        public static List<AbsUnit> AdjacentUnits(IntVector2 center,
            ToggEngine.Data.PlayerFilter filter)
        {
            return Units(Rectangle2.FromCenterTileAndRadius(center, 1),
                center,
                filter);
        }

        public static List<AbsUnit> Units(
            IntVector2 center,
            int radius,
            bool includeCenter,
            ToggEngine.Data.PlayerFilter filter)
        {
            return Units(Rectangle2.FromCenterTileAndRadius(center, radius),
                includeCenter ? IntVector2.NegativeOne : center,
                filter);
        }

        public static List<AbsUnit> Units(
            Rectangle2 area,
            IntVector2 ignorePos,
            ToggEngine.Data.PlayerFilter filter)
        {
            List<AbsUnit> result = new List<AbsUnit>(2);

            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                if (loop.Position != ignorePos)
                {
                    var u = toggRef.board.getUnit(loop.Position);
                    if (u != null && filter.unit(u))
                    {
                        result.Add(u);
                    }
                }
            }

            return result;
        }

        public static List<AbsUnit> Units_InRangeAndSight(
            IntVector2 center,
            int radius,
            bool unitsAreObsticles,
            ToggEngine.Data.PlayerFilter filter)
        {
            var result = Units(Rectangle2.FromCenterTileAndRadius(center, radius),
                IntVector2.NegativeOne,
                filter);

            IntVector2 non;
            for (int i = result.Count - 1; i >= 0; --i)
            {
                if (toggRef.board.InLineOfSight(center, result[i].squarePos, out non, null,
                    unitsAreObsticles, false) == false)
                {
                    result.RemoveAt(i);
                }
            }

            return result;
        }

    }

    enum AreaFilterType
    {
        HeroUnit,
    }
}
