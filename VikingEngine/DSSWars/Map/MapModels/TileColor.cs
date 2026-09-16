using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Map2;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.DSSWars.Map.MapModels
{
    /// <summary>
    /// Combines data from different tile data types
    /// </summary>
    struct CombinedTile
    {
        public MapTile1_1 mapTile;
        public SumTile4_4 sumTile;

        public CombinedTile() 
        { 
            
        }

        public CombinedTile(GenTile genTile)
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
        public static Color FactionAndTerrainColor(CombinedTile tile)
        {
            return TerrainColor(tile);
        }
        public static Color MinimapColor(CombinedTile tile)
        {
            return TerrainColor(tile);
        }
        public static Color TerrainColor(CombinedTile tile)
        {
            if (tile.mapTile.heightValue <= MapLib.MapHeight2.WaterPlaneHeight)
            {
                float depth = /*1f - */tile.mapTile.heightValue / (float)MapLib.MapHeight2.WaterPlaneHeight;//1f - tile.groundY / MapLib.MapHeight2.WaterBottomY;
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
                    color = ColorExt.Mix(col2, color, tile.sumTile.secondBiomWeight / (float)byte.MaxValue);
                }

                return color;
                //depth *= 0.75f;
                //tile.color = ColorExt.MultiplyRGB(col, 0.5f + 0.9f * height);//new Color(depth, depth + 0.2f, depth);
            }
        }

        public static Color factionColor(CombinedTile tile)
        {
            if (tile.sumTile.CityIndex != ushort.MaxValue)
            {   
                if (DssRef.world.cities[tile.sumTile.CityIndex].pfaction.TryGetPlayer(out var p) && 
                    p.profile.flag != null)
                {
                    return p.profile.flag.col0_Main;
                }
            }
            return Color.Gray;
            
        }

        public static Color heightAndMinimapCol(Faction playerFaction, CombinedTile tile)
        {
            float brightness = 1f - tile.mapTile.heightValue * 0.05f;

            float red = 0;
            float green = 0;

            if (tile.sumTile.CityIndex == ushort.MaxValue)
            {
                return ColorExt.VeryDarkGray;
            }

            var pfaction = DssRef.world.cities[tile.sumTile.CityIndex].pfaction;
            if (pfaction == playerFaction.pfaction)
            {
                brightness *= 0.5f;
            }
            else
            {
                var rel = DssRef.world.diplomacy.GetRelation(playerFaction.pfaction, pfaction).Relation;

                if (rel <= RelationType.RelationTypeN2_Truce)
                {
                    red = 0.2f;
                }
                else if (rel >= RelationType.RelationType3_Ally)
                {
                    green = 0.2f;
                }

                brightness *= 0.2f;
            }

            //int distance = city.tilePos.SideLength(pos);

            //if (distance == 1)
            //{
            //    brightness *= 1.5f;
            //}
            //else if (hasBorder(out bool sameFaction))
            //{
            //    if (sameFaction)
            //    {
            //        brightness *= 1.25f;
            //    }
            //    else
            //    {
            //        brightness *= 0.6f;
            //    }
            //}

            return new Color(brightness + red, brightness + green, brightness);
        }
        static Color biomCol(Settings.BiomType biom, int biomColorheight, float percNextHeight)
        {
            Color col1 = DssRef.map.bioms.bioms[(int)biom].colors_height[biomColorheight].Color;
            if (percNextHeight > 0)
            {
                Color col2 = DssRef.map.bioms.bioms[(int)biom].colors_height[biomColorheight + 1].Color;
                return ColorExt.Mix(col2, col1, percNextHeight);
            }
            else
            {
                return col1;
            }
        }
    }
}
