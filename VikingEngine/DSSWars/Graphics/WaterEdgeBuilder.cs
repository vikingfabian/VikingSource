using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars
{
    static class WaterEdgeBuilder
    {
        public const float Opacity = 0.6f;
        static PolygonColor[][] WaterEdgeOrtho_Dir_List;

        public static PolygonColor[] Get(Dir4 dir)
        {
            return WaterEdgeOrtho_Dir_List[(int)dir];
        }

        public static void Init()
        {
            Color color = Color.White;//ColorExt.GrayScale(0.4f);
            WaterEdgeOrtho_Dir_List = new PolygonColor[(int)Dir4.NUM_NON][];

            const float LargeTileHalfWidth = 0.5f;

            //north
            {
                PolygonColor[] north = new PolygonColor[Map.MapData.MapTile1_1.ModelScale_Inv];
                Vector2 topLeft = new Vector2(-LargeTileHalfWidth, -LargeTileHalfWidth - Map.MapData.MapTile1_1.ModelScale);
                for (int i = 0; i < Map.MapData.MapTile1_1.ModelScale_Inv; i++)
                {
                    north[i] = Graphics.PolygonColor.QuadXZ(
                           topLeft,
                           Map.MapData.MapTile1_1.ModelScaleV2, false, 0,
                           SpriteName.WaterEdgeMask_coast,
                           Dir4.E,
                           color);
                    topLeft.X += Map.MapData.MapTile1_1.ModelScale;
                }
                WaterEdgeOrtho_Dir_List[(int)Dir4.N] = north;
            }
            //south
            {
                PolygonColor[] south = new PolygonColor[Map.MapData.MapTile1_1.ModelScale_Inv];
                Vector2 topLeft = new Vector2(-LargeTileHalfWidth, LargeTileHalfWidth);
                for (int i = 0; i < Map.MapData.MapTile1_1.ModelScale_Inv; i++)
                {
                    south[i] = Graphics.PolygonColor.QuadXZ(
                           topLeft,
                           Map.MapData.MapTile1_1.ModelScaleV2, false, 0,
                           SpriteName.WaterEdgeMask_coast,
                           Dir4.W,
                           color);
                    topLeft.X += Map.MapData.MapTile1_1.ModelScale;
                }
                WaterEdgeOrtho_Dir_List[(int)Dir4.S] = south;
            }

            //west
            {
                PolygonColor[] west = new PolygonColor[Map.MapData.MapTile1_1.ModelScale_Inv];
                Vector2 topLeft = new Vector2(-LargeTileHalfWidth - Map.MapData.MapTile1_1.ModelScale, -LargeTileHalfWidth);
                for (int i = 0; i < Map.MapData.MapTile1_1.ModelScale_Inv; i++)
                {
                    west[i] = Graphics.PolygonColor.QuadXZ(
                           topLeft,
                           Map.MapData.MapTile1_1.ModelScaleV2, false, 0,
                           SpriteName.WaterEdgeMask_coast,
                           Dir4.S,
                           color);
                    topLeft.Y += Map.MapData.MapTile1_1.ModelScale;
                }
                WaterEdgeOrtho_Dir_List[(int)Dir4.W] = west;
            }

            //east
            {
                PolygonColor[] east = new PolygonColor[Map.MapData.MapTile1_1.ModelScale_Inv];
                Vector2 topLeft = new Vector2(LargeTileHalfWidth, -LargeTileHalfWidth);
                for (int i = 0; i < Map.MapData.MapTile1_1.ModelScale_Inv; i++)
                {
                    east[i] = Graphics.PolygonColor.QuadXZ(
                           topLeft,
                           Map.MapData.MapTile1_1.ModelScaleV2, false, 0,
                           SpriteName.WaterEdgeMask_coast,
                           Dir4.N,
                           color);
                    topLeft.Y += Map.MapData.MapTile1_1.ModelScale;
                }
                WaterEdgeOrtho_Dir_List[(int)Dir4.E] = east;
            }
        }


    }
}
