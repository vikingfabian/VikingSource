using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map.Generate
{
    class MapGenerateSettings
    {
        public float LandChainMinRadius = 2;
        public float LandChainMaxRadius = 30;
        public IntervalF linkPosDiffRange = new IntervalF(0.5f, 3);
        public Range landSpotSzRange = new Range(2, 24);
        public IntervalF startRadiusRange;
        public Range chainLengthRange = new Range(2, 20);


        public float BuildChainsCount_per100Tiles = 1 / 20f; //Per 100 tiles 
        public float DigChainsCount_per100Tiles = 1 / 18f; //Per 100 tiles 

        public int repeatBuildDigCount = 3;
        public MapStartAs StartAs = MapStartAs.Water;

        public MapGenerateSettings()
        {
            startRadiusRange = new IntervalF(LandChainMinRadius, LandChainMaxRadius * 0.5f);
        }

        
    }
}
