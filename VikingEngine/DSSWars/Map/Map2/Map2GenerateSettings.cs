using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.MapLib;

namespace VikingEngine.DSSWars.Map.Map2
{
    class Map2GenerateSettings
    {
        public bool useGenerate = false;

        public MapStartAs StartAs = MapStartAs.Water;

        public MapScale mapScale = new MapScale();

        public int nodeFillPerc = 15;
        public int nodeConnectPerc = 70;

        public float percentageUnclaimed = 0.25f;

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
                mapScale.bCustomSize = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return mapScale.bCustomSize;
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
                mapScale.customMapSize.X = value;
            }
            return mapScale.customMapSize.X;
        }
        public int MapYProperty(object tag, bool set, int value)
        {
            if (set)
            {
                mapScale.customMapSize.Y = value;
            }
            return mapScale.customMapSize.Y;
        }

        public void setCustomSize(IntVector2 customMapSize)
        {
            this.mapScale.customMapSize = customMapSize;

            mapScale.bCustomSize = true;
        }

    }
}
