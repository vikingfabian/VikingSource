using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map.MapData;

namespace VikingEngine.DSSWars.Map.MapLib
{
    struct SetTerrainChance
    {
        public double chance;
        public TerrainMainType mainTerrain;
        public int subTerrrain;

        public Range sizeRange;

        public SetTerrainChance(double chance, TerrainMainType mainTerrain, int subTerrrain)
        {
            this.chance = chance;
            this.mainTerrain = mainTerrain;
            this.subTerrrain = subTerrrain;
            sizeRange = new Range(1, 1);
        }

        /// <returns>check next</returns>
        public bool set(PcgRandom rnd, ref CombinedTile cTile)
        {
            if (chance > 0)
            {
                if (rnd.ChanceWithCheck(chance))
                {
                    cTile.mapTile.SetType(mainTerrain, subTerrrain, sizeRange.GetRandom_WithCheck(rnd));
                    return false;
                }
                return true;
            }
            return false;
        }
    }

    struct BiomGroundType
    {
        public IntervalF noiseValue;
        public GroundType groundType;

        public SetTerrainChance setTerrain1;
        public SetTerrainChance setTerrain2;

        public bool animalSpawn;
        public bool mineSpawn;


        public void set(PcgRandom rnd, ref CombinedTile cTile)
        {
            if (setTerrain1.set(rnd, ref cTile))
            {
                setTerrain2.set(rnd, ref cTile);
            }
            cTile.mapTile.groundType = groundType;
        }
    }
}
