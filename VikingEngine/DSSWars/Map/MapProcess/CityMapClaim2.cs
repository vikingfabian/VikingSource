using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.DSSWars.Map.MapData;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;

namespace VikingEngine.Core.DSSWars.Map.MapProcess
{
    static class CityMapClaim2
    {
        
        const float HeightDifferenceMaxEffect = MapHeight2.HeightY * 40;

        public static void CityClaim(City city)
        {
            var pcity = new VikingEngine.DSSWars.GameObject.ObjectPointer.PCity(city.myIndex);

            IntVector2 center = WP.MaptileToSumTile(city.maptilePos);
            float cityHeight = DssRef.world.subTileGrid.Get(city.maptilePos).groundY;
            int radius = DssRef.storage.ruleset_instance.CitySizeSetup[(int)city.cityType].claimRadius;
            int maxRadius = radius + 4;
            Rectangle2 maxArea = Rectangle2.FromCenterTileAndRadius(center, maxRadius);
            Grid2D_L<float> inflenceMap = new Grid2D_L<float>(maxArea);

            ForXYLoop setuploop = new ForXYLoop(Rectangle2.FromCenterTileAndRadius(center, 1));
            while (setuploop.Next())
            {
                if (DssRef.world.tileGrid.TryGet(setuploop.Position, out var tile))
                {
                    tile.pcity = pcity;
                    DssRef.world.tileGrid.Set(setuploop.Position, tile);
                    inflenceMap.Set_offset(setuploop.Position, radius -1);
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
                            if (DssRef.world.tileGrid.TryGet(npos, out var ntile))
                            {
                                if (ntile.pcity.IsEmpty() || ntile.pcity == pcity)
                                {
                                    ntile.pcity = pcity;
                                    DssRef.world.tileGrid.Set(npos, ntile);

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
                    if (DssRef.world.tileGrid.TryGet(loop.Position, out var tile))
                    {
                        if (tile.pcity == pcity)
                        {
                            sumArea.includeTile(loop.Position);

                            if (tile.biomColorHeight > 1)
                            {
                                tile.IsBorderTile = false;
                                foreach (var dir in IntVector2.Dir8Array)
                                {
                                    if ((DssRef.world.tileGrid.TryGet(loop.Position + dir, out var ntile) &&
                                        ntile.pcity == pcity &&
                                        ntile.biomColorHeight > 1) == false)
                                    {
                                        tile.IsBorderTile = true;
                                        break;
                                    }
                                }
                                DssRef.world.tileGrid.Set(loop.Position, tile);

                            }
                        }
                    }
                }

                city.citySumTileArea.includeArea(sumArea);
            }

            //ForXYLoop loop = new ForXYLoop(area);

            //while (loop.Next())
            //{
            //    if (DssRef.world.tileGrid.TryGet(loop.Position, out var tile))
            //    {
            //        if (tile.pcity.IsEmpty())
            //        {
            //            tile.pcity = new GameObject.ObjectPointer.PCity(city.myIndex);
            //            DssRef.world.tileGrid.Set(loop.Position, tile);
            //        }
            //    }
            //}

            //ForXYEdgeLoop outerEdge = new ForXYEdgeLoop(area);
            //while (outerEdge.Next())
            //{
            //    if (DssRef.world.tileGrid.TryGet(outerEdge.Position, out var tile))
            //    {
            //        tile.IsBorderTile = true;
            //        DssRef.world.tileGrid.Set(outerEdge.Position, tile);
            //    }
            //}
        }

        //static int ClaimRadius(CityType cityType)
        //{
        //    switch (cityType)
        //    {
        //        default:
        //            return ClaimRadius_Campsite;
        //        case CityType.Village:
        //            return ClaimRadius_Village;
        //        case CityType.Town:
        //            return ClaimRadius_Town;
        //        case CityType.Capital:
        //            return ClaimRadius_Capital;

        //    }
        //}
    }
}
