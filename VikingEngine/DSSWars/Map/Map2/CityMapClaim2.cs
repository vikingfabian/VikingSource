using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Map.Map2
{
    static class CityMapClaim2
    {
        const int DefaultHallClaimSummaryRadius = 8;

        public static void CityClaim(City city)
        {
            var area = Rectangle2.FromCenterTileAndRadius(WP.MaptileToSumTile(city.maptilePos), DefaultHallClaimSummaryRadius);
            ForXYLoop loop = new ForXYLoop(area);

            while (loop.Next())
            {
                if (DssRef.world.tileGrid.TryGet(loop.Position, out var tile))
                {
                    tile.pcity = new GameObject.ObjectPointer.PCity(city.myIndex);
                    DssRef.world.tileGrid.Set(loop.Position, tile);
                }
            }

            ForXYEdgeLoop outerEdge = new ForXYEdgeLoop(area);
            while (outerEdge.Next())
            {
                if (DssRef.world.tileGrid.TryGet(outerEdge.Position, out var tile))
                {
                    tile.IsBorderTile = true;
                    DssRef.world.tileGrid.Set(outerEdge.Position, tile);
                }
            }
        }
    }
}
