using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Map.Map2
{
    static class CityMapClaim2
    {
        const int DefaultHallClaimSummaryRadius = 4;

        public static void CityClaim(City city)
        {
            var area = Rectangle2.FromCenterTileAndRadius(WP.MaptileToSumTile(city.maptilePos), DefaultHallClaimSummaryRadius);
            ForXYLoop loop = new ForXYLoop(area);

            while (loop.Next())
            {
                if (DssRef.world.tileGrid.TryGet(loop.Position, out var tile))
                {
                    tile.CityIndex = (ushort)city.myIndex;
                    DssRef.world.tileGrid.Set(loop.Position, tile);
                }
            }
        }
    }
}
