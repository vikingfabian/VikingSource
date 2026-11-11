using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map.Map2;

namespace VikingEngine.DSSWars.Map.MapData
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

        public GenTile GenTile()
        {
            return new GenTile()
            {
                biom1 = sumTile.biom1,
                groundY = mapTile.GroundY_NoRamp(),
                heightValue = mapTile.heightValue
            };
        }
    }
}
