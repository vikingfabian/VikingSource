using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.DSSWars.Data.GameSetup;
using VikingEngine.Core.DSSWars.Map.MapData;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;

namespace VikingEngine.Core.DSSWars.Map.MapProcess
{
    static class CityMapClaim2
    {
        const double OutsideRegionRadiusMultiplier = 2.0;
        const float HeightDifferenceMaxEffect = MapHeight2.HeightY * 40;

        public static void CityClaim(City city)
        {
            //var pcity = new PCity(city.myIndex);

           // IntVector2 center = WP.MaptileToSumTile(city.maptilePos);
            
            int radius = DssRef.world.CitySizeSetup[(int)city.cityType].claimRadius_sum;

            city.buildOutsideRegionRadius = MathExt.MultiplyInt(radius, OutsideRegionRadiusMultiplier) * SumTile4_4.TileWidth;

            AreaClaim(city.maptilePos, radius, city);
        }

        public static void CityExpansionClaim(City city, TerrainBuildingType build, IntVector2 expansionMapTilePos)
        {
            int radius = CityHallSetup.ExpansionRadius(build);
            AreaClaim(expansionMapTilePos, radius, city);
        }

        static void AreaClaim(IntVector2 mapTileCenter, int sumTileRadius, City city)
        {
            var pcity = city.Pointer();
            IntVector2 center = WP.MaptileToSumTile(mapTileCenter);
            float cityHeight = DssRef.world.subTileGrid.Get(mapTileCenter).groundY;

            int maxRadius = sumTileRadius + 4;
            Rectangle2 maxArea = Rectangle2.FromCenterTileAndRadius(center, maxRadius);
            Grid2D_L<float> inflenceMap = new Grid2D_L<float>(maxArea);

            ForXYLoop setuploop = new ForXYLoop(Rectangle2.FromCenterTileAndRadius(center, 1));
            while (setuploop.Next())
            {
                if (DssRef.world.sumtileGrid.TryGet(setuploop.Position, out var tile))
                {
                    tile.pcity = pcity;
                    DssRef.world.sumtileGrid.Set(setuploop.Position, tile);
                    inflenceMap.Set_offset(setuploop.Position, sumTileRadius - 1);
                }
            }

            for (int r = 1; r < maxRadius; r++)
            {
                ForXYEdgeLoop loop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(center, r));
                while (loop.Next())
                {
                    var influence = inflenceMap.Get_offset(loop.Position) - 1;
                    if (influence > 0)
                    {
                        for (int i = 0; i < IntVector2.Dir8Array.Length; ++i)//foreach (var dir in IntVector2.Dir4Array)
                        {
                            IntVector2 npos = loop.Position + IntVector2.Dir8Array[i];
                            if (DssRef.world.sumtileGrid.TryGet(npos, out var ntile))
                            {
                                if (ntile.pcity.IsEmpty() || ntile.pcity == pcity)
                                {
                                    ntile.pcity = pcity;
                                    DssRef.world.sumtileGrid.Set(npos, ntile);

                                    float height = WP.GroundY_SumTile(npos);
                                    float heightDiff = height - cityHeight;
                                    float diffEffect = Bound.Set(heightDiff / HeightDifferenceMaxEffect, -1, 1);

                                    float totalInflence = influence + diffEffect * -0.5f;
                                    if (lib.IsOdd(i))
                                    {
                                        totalInflence -= 0.2f; //diagonal is weaker
                                    }

                                    var prevInf = inflenceMap.Get_offset(npos);
                                    inflenceMap.Set_offset(npos, Math.Max(totalInflence, prevInf));
                                }
                            }
                        }
                    }
                }
            }

            {//Finalize with edge check
                Rectangle2 sumArea = Rectangle2.FromCenterTileAndRadius(center, 2);

                ForXYLoop loop = new ForXYLoop(maxArea);
                while (loop.Next())
                {
                    if (DssRef.world.sumtileGrid.TryGet(loop.Position, out var tile))
                    {
                        if (tile.pcity == pcity)
                        {
                            sumArea.includeTile(loop.Position);

                            if (tile.biomColorHeight > 1)
                            {
                                tile.IsBorderTile = false;
                                foreach (var dir in IntVector2.Dir8Array)
                                {
                                    if ((DssRef.world.sumtileGrid.TryGet(loop.Position + dir, out var ntile) &&
                                        ntile.pcity == pcity &&
                                        ntile.biomColorHeight > 1) == false)
                                    {
                                        tile.IsBorderTile = true;
                                        break;
                                    }
                                }
                                DssRef.world.sumtileGrid.Set(loop.Position, tile);

                            }
                        }
                    }
                }

                city.citySumTileArea.includeArea(sumArea);
            }

        }

    }
}
