using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using VikingEngine.DSSWars.GameState.MapEditor2.IconEditor;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.MapLib;

namespace VikingEngine.DSSWars.Map.Map2
{
    class Map2GenerateSettings
    {
        public bool useGenerate = false;

        public MapStartAs StartAs = MapStartAs.Water;

        public MapScale mapScale = new MapScale();

        public int nodeFillPerc = 25;
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

        public int secondaryCitySpacing = 8;

        public bool CustomSizeProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                mapScale.bCustomSize = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return mapScale.bCustomSize;
        }

        public int SpaceBetweenCities = 6;

        public int cityPlacementPerc = 25;

        public int SpaceBetweenCitiesProperty(object tag, bool set, int value)
        {
            if (set)
            {
                SpaceBetweenCities = value;
            }
            return SpaceBetweenCities;
        }
        public int CityPlacementPercProperty(object tag, bool set, int value)
        {
            if (set)
            {
                cityPlacementPerc = value;
            }
            return cityPlacementPerc;
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

        public void ResetToDefault(Map2GeneratorTab tab)
        {
            Map2GenerateSettings defaultSett = new Map2GenerateSettings();

            switch (tab)
            {
                case Map2GeneratorTab.Nodes:
                    this.nodeFillPerc = defaultSett.nodeFillPerc;
                    this.nodeConnectPerc = defaultSett.nodeConnectPerc;
                    break;
                case Map2GeneratorTab.CityPlacements:
                    this.cityPlacementPerc = defaultSett.cityPlacementPerc;
                    this.SpaceBetweenCities = defaultSett.SpaceBetweenCities;
                    break;
        } }

    }
}
