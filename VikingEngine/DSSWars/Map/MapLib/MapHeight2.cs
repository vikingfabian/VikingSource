using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.DSSWars.Map.MapLib
{
    static class MapHeight2
    {
        public const byte ColorLayerHeight = 32;
        public const int WaterColorLayersCount = 2;
        public const float HeightY = 0.004f;

        public const byte WaterPlaneHeight = ColorLayerHeight * WaterColorLayersCount; //64
        public const float WaterSurfaceY = -0.1f;

        public const byte ShallowWaterHeight = WaterPlaneHeight - 16;
        public const float WaterBottomY = WaterSurfaceY - WaterPlaneHeight * HeightY - 0.5f * HeightY;
        public const float LowGroundY = WaterSurfaceY + 20 * HeightY;
        public const float DefaultGroundY = WaterSurfaceY + 40 * HeightY;
        public const float MountainStartY = DefaultGroundY + 60 * HeightY;
        
        public const float MaxY = WaterBottomY + byte.MaxValue * HeightY;

        public const float MountainPeekY = MaxY - HeightY * 4f;
        public const float WaterFoamY = WaterSurfaceY + 0.01f;
        public const float UnitMinY = WaterSurfaceY;

        public const float UnitQuadMinY = WaterSurfaceY + 0.07f;

        public static readonly IntervalF HeightY_Interval = new IntervalF(WaterBottomY, MaxY);

        public static void ToColorHeight(byte heightValue, out int colorheight, out float percNextHeight)
        {
            colorheight = heightValue / ColorLayerHeight;
            percNextHeight = (heightValue % ColorLayerHeight) / (float)ColorLayerHeight;
        }
    }
}
