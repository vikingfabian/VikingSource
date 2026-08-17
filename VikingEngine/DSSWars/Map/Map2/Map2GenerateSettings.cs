using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using VikingEngine.DSSWars.Map.Generate;

namespace VikingEngine.DSSWars.Map.Map2
{
    class Map2GenerateSettings
    {
        public bool useGenerate = false;

        public MapStartAs StartAs = MapStartAs.Water;

        public bool bCustomSize = false;
        public IntVector2 customMapSize = new IntVector2(WorldData.CustomMapSize_Min);
        public MapSize mapSize = MapSize.Medium;

        public bool CustomSizeProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                bCustomSize = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return bCustomSize;
        }

        public IntVector2 IconSize()
        {
            return bCustomSize ? customMapSize : WorldData.SizeDimentions(mapSize);
        }

        //public bool CleanUpProperty(object tag, bool set, bool value)
        //{
        //    if (set)
        //    {
        //        cleanUpSingleTiles = value;
        //        (value ? SoundLib.click : SoundLib.back).Play();
        //    }
        //    return cleanUpSingleTiles;
        //}

        public int MapXProperty(object tag, bool set, int value)
        {
            if (set)
            {
                customMapSize.X = value;
            }
            return customMapSize.X;
        }
        public int MapYProperty(object tag, bool set, int value)
        {
            if (set)
            {
                customMapSize.Y = value;
            }
            return customMapSize.Y;
        }

        public void setCustomSize(IntVector2 customMapSize)
        {
            this.customMapSize = customMapSize;

            bCustomSize = true;
        }

    }
}
