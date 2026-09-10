using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map.Map2;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.DSSWars.Map.MapModels
{
    /// <summary>
    /// Combines data from different tile data types
    /// </summary>
    struct SummaryTile
    {
        public MapTile1_1 mapTile;
        public SumTile4_4 sumTile;

        public SummaryTile() 
        { 
            
        }

        public SummaryTile(GenTile genTile)
        {
            mapTile = new MapTile1_1()
            {
                heightValue = MapLib.MapHeight2.HeightY_Interval.GetValueBytePercentPos(genTile.groundY),
            };

            sumTile = new SumTile4_4()
            {
                biom1 = genTile.biom1,
                biom2 = genTile.biom2,
                secondBiomWeight = (byte)(genTile.secondBiomWeight * byte.MaxValue),
            };
        }
    }


    static class TileColor
    {
       
        static Color tileColor(SummaryTile tile)
        {
            //if (tile.groundY > Height_WaterBottom)
            //{
            //    lib.DoNothing();
            //}
            

            if (tile.mapTile.heightValue <= MapLib.MapHeight2.WaterPlaneHeight)
            {
                float depth = 1f - tile.mapTile.heightValue / (float)MapLib.MapHeight2.WaterPlaneHeight;//1f - tile.groundY / MapLib.MapHeight2.WaterBottomY;
                return new Color(depth * 0.5f, depth * 0.5f, depth * 0.5f + 0.2f);
            }
            else
            {
                //float height = tile.groundY / MapLib.MapHeight2.MountainPeekY;
                //int biomColorheight = Bound.Set(MapLib.MapHeight2.MinLandHeight +  Convert.ToInt32( (ColorHeight.MaxHeight - ColorHeight.MinLandHeight) * height), 0, 9);

                
                MapLib.MapHeight2.ToColorHeight(tile.mapTile.heightValue, out int biomColorheight, out float percNextHeight);
                //var col = DssRef.map.bioms.bioms[(int)tile.biom1].colors_height[biomColorheight].Color;
                Color color = biomCol(tile.sumTile.biom1, biomColorheight, percNextHeight);

                if (tile.sumTile.biom2 != tile.sumTile.biom1 && tile.sumTile.secondBiomWeight > 0)
                {
                    var col2 = biomCol(tile.sumTile.biom2, biomColorheight, percNextHeight);//DssRef.map.bioms.bioms[(int)tile.biom2].colors_height[biomColorheight].Color;
                    color = ColorExt.Mix(col2, color, tile.sumTile.secondBiomWeight);
                }

                return color;
                //depth *= 0.75f;
                //tile.color = ColorExt.MultiplyRGB(col, 0.5f + 0.9f * height);//new Color(depth, depth + 0.2f, depth);
            }
        }
        static Color biomCol(Settings.BiomType biom, int biomColorheight, float percNextHeight)
        {
            Color col1 = DssRef.map.bioms.bioms[(int)biom].colors_height[biomColorheight].Color;
            Color col2 = DssRef.map.bioms.bioms[(int)biom].colors_height[biomColorheight + 1].Color;
            return ColorExt.Mix(col2, col1, percNextHeight);
        }
    }
}
