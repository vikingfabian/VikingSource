using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;

namespace VikingEngine.DSSWars.Map.MapLib
{
    struct MapScale
    {
        public const int IconToWorldScale = 16;
        public bool bCustomSize = false;
        public IntVector2 customMapSize = new IntVector2(WorldData.CustomMapSize_Min);
        public MapSize mapSize = MapSize.Small;

        public IntVector2 Size(bool icon)
        {
            var size = bCustomSize ? customMapSize : WorldData.SizeDimentions(mapSize);
            if (icon)
            {
                size /= IconToWorldScale;
            }
            return size;
        }

        public MapScale()
        { }

        public void setTextureSize(IntVector2 size)
        {
            customMapSize = size * IconToWorldScale;
            bCustomSize = true;
        }
    }
}
