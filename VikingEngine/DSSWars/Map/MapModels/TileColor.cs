using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Map.Map2;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.DSSWars.Map.MapModels
{
    


    static class TileColor
    {
        static readonly Color MapCol_HeadCity = new Color(255, 174, 184);
        static readonly Color MapCol_LargeCity = new Color(253, 0, 30);
        static readonly Color MapCol_SmallCity = new Color(148, 0, 17);
        static readonly Color MapCol_CampsiteCity = new Color(148, 0, 17);
        static readonly Color MapCol_UnclaimedCity = Color.Blue;

        static readonly Color MiniMapCol_HeadCity = new Color(251, 37, 114);
        static readonly Color MiniMapCol_LargeCity = new Color(226, 11, 88);
        static readonly Color MiniMapCol_SmallCity = new Color(194, 4, 72);
        static readonly Color MiniMapCol_CampsiteCity = new Color(148, 0, 17);
        static readonly Color MiniMapCol_UnclaimedCity = Color.Blue;
        public static void cityColor(City city, out Color center, out Color outline, out Color factionCol)
        {
            switch (city.cityType)
            {
                default: center = MapCol_HeadCity; break;
                case CityType.Town: center = MapCol_LargeCity; break;
                case CityType.Village: center = MapCol_SmallCity; break;
                case CityType.Campsite: center = MapCol_CampsiteCity; break;
                case CityType.UnClaimed: center = MapCol_UnclaimedCity; break;

            }
            outline = Color.LightPink;
            factionCol = city.pfaction.GetPlayer().profile.flag.col0_Main;
        }
        public static Color FactionAndTerrainColor(CombinedTile tile, int leanY)
        {
            
            Color color = TerrainColor(tile, leanY);

            if (tile.mapTile.IsLandOrWaterPlane() && tile.sumTile.pcity.HasValue())
            {
                //Color factionCol = 
                var pf = tile.sumTile.pcity.City().pfaction;
                if (pf.TryGetPlayer(out var player))
                {
                    if (tile.sumTile.IsBorderTile)
                    {
                        color = player.profile.flag.col0_Main;
                    }
                    else
                    {
                        color = ColorExt.Mix(color, player.profile.flag.col0_Main, 0.5f);
                    }
                }
            }

            return color;
        }

        public static Color BorderAndTerrainColor(CombinedTile tile, int leanY)
        {

            Color color = TerrainColor(tile, leanY);

            if (tile.sumTile.IsBorderTile && tile.mapTile.IsLandOrWaterPlane() && tile.sumTile.pcity.HasValue())
            {
                //Color factionCol = 
                var pf = tile.sumTile.pcity.City().pfaction;
                if (pf.TryGetPlayer(out var player))
                {
                    color = player.profile.flag.col0_Main;                    
                }
            }

            return color;
        }

        public static Color MinimapColor(CombinedTile tile, int leanY)
        {
            if (tile.mapTile.IsWater())
            {
                return WaterColor(tile.mapTile.heightValue);
                //if (tile.mapTile.heightValue >= MapHeight2.ShallowWaterHeight)
                //{
                //    return WorldData.WaterEdgeColorBright;
                //}
                //else
                //{
                //    return WorldData.WaterDarkCol1;
                //}
            }
            return FactionAndTerrainColor(tile, leanY);
        }

        public static Color IconmapColor(CombinedTile tile)
        {
            if (tile.mapTile.IsWater())
            {
                return WaterColor(tile.mapTile.heightValue);
                //if (tile.mapTile.heightValue >= MapHeight2.ShallowWaterHeight)
                //{
                //    return WorldData.WaterEdgeColorBright;
                //}
                //else
                //{
                //    return WorldData.WaterDarkCol1;
                //}
            }
            return TerrainColor(tile, 0);
        }

        public static Color WaterColor(byte height)
        {
            if (height >= MapHeight2.ShallowWaterHeight)
            {
                return WorldData.WaterEdgeColorBright;
            }
            else
            {
                return ColorExt.MultiplyRGB( WorldData.WaterDarkCol1,  0.7f + 0.3f * height / MapHeight2.ShallowWaterHeight);
            }
        }
        public static Color TerrainColor(CombinedTile tile, int leanY)
        {
            //if (tile.mapTile.heightValue <= MapLib.MapHeight2.WaterPlaneHeight)
            //{
            //    float depth = /*1f - */tile.mapTile.heightValue / (float)MapLib.MapHeight2.WaterPlaneHeight;//1f - tile.groundY / MapLib.MapHeight2.WaterBottomY;
            //    return new Color(depth * 0.7f, depth * 0.7f, depth * 0.7f + 0.2f);
            //}
            //else
            //{
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

            if (leanY < -1)
            {
                color = ColorExt.MultiplyRGB(color, tile.mapTile.heightValue < MapHeight2.MountainStarHeight? 0.97f : 0.92f);
            }

                return color;
                //depth *= 0.75f;
                //tile.color = ColorExt.MultiplyRGB(col, 0.5f + 0.9f * height);//new Color(depth, depth + 0.2f, depth);
            //}
        }

        public static Color factionColor(CombinedTile tile)
        {
            if (tile.sumTile.pcity.HasValue())
            {   
                if (tile.sumTile.pcity.City().pfaction.TryGetPlayer(out var p) && 
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

            if (tile.sumTile.pcity.IsEmpty())
            {
                return ColorExt.VeryDarkGray;
            }

            var pfaction = tile.sumTile.pcity.City().pfaction;
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
                if (biomColorheight == 7)
                {
                    lib.DoNothing();
                }
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
