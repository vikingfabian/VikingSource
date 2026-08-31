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

        public int nodeFillPerc = 15;
        public int nodeConnectPerc = 70;

        public int nodeFillPercProperty(object tag, bool set, int value)
        {
            if (set)
            {
                nodeFillPerc = value;
            }
            return nodeFillPerc;
        }
        public int nodeConnectPercProperty(object tag, bool set, int value)
        {
            if (set)
            {
                nodeConnectPerc = value;
            }
            return nodeConnectPerc;
        }

        /// <summary>
        /// Compared to medium map 
        /// </summary>
        public float scale = 1.0f;

        public int minCitySpacing = 32;

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

        public int loopCount(PcgRandom rnd, int mediumCount)
        {
            double count = mediumCount * scale;

            double rndAdd = Bound.Min(count * 0.2, 1);

            return Convert.ToInt32(count + rnd.Plus_MinusD(rndAdd));
        }
       
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
