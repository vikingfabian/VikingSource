using System;
using System.Drawing;

namespace VikingEngine.DSSWars.Map
{
    class TerrainContent
    {
        public const int SproutMaxSize = 5;
        public const int TreeMaxSize = 100;

        public void asyncFoilGroth(IntVector2 pos, SubTile subtile)
        {
            switch ((Map.TerrainSubFoilType)subtile.subTerrain)
            {

                case Map.TerrainSubFoilType.TreeHard:
                    {
                        if (subtile.terrainValue < TreeMaxSize)
                        {
                            subtile.terrainValue++;
                            DssRef.world.subTileGrid.Set(pos, subtile);
                        }

                        if (Ref.rnd.Chance(0.2) && subtile.terrainValue > 20 && subtile.terrainValue < 90)
                        {
                            IntVector2 rndDir = arraylib.RandomListMember(IntVector2.Dir8Array);
                            if (Ref.rnd.Chance(0.2))
                            {
                                rndDir *= 2;
                            }
                            Map.SubTile ntile;
                            var npos = pos + rndDir;
                            if (DssRef.world.subTileGrid.TryGet(npos, out ntile))
                            {
                                if (ntile.mainTerrain == Map.TerrainMainType.DefaultLand)
                                {
                                    ntile.SetType(Map.TerrainMainType.Foil, (int)Map.TerrainSubFoilType.TreeHardSprout, 1);

                                    DssRef.world.subTileGrid.Set(npos, ntile);
                                }
                            }

                        }
                    }
                    break;

                case Map.TerrainSubFoilType.TreeHardSprout:
                    {
                        if (++subtile.terrainValue > SproutMaxSize)
                        {
                            subtile.SetType(Map.TerrainMainType.Foil, (int)Map.TerrainSubFoilType.TreeHard, 1);
                        }

                        DssRef.world.subTileGrid.Set(pos, subtile);
                    }
                    break;
            }
        }

        public static void createSubTileContent(int x, int y, 
            float distanceToCity,
            Tile tile, 
            ref IntervalF mudRadius,
            ref SubTile subTile, 
            WorldData world, 
            VikingEngine.EngineSpace.Maths.SimplexNoise2D noiseMap)
        {

            var percTree = tile.heightSett().percTree;
            if (percTree > 0)
            {
                
                if (distanceToCity <= mudRadius.Max)
                    
                {
                    if (distanceToCity <= mudRadius.Min || world.rnd.Chance(0.5))
                    {
                        subTile.SetType(TerrainMainType.Destroyed, 0, 1);
                        return;
                    }
                }

                float noise = noiseMap.OctaveNoise2D_Normal(4, 0.75f, 1, x, y);

                if (noise < percTree && noise < world.rnd.Double(percTree * 2f))
                {
                    int size = (int)((1.0 - Math.Min(noise, world.rnd.Double())) * TreeMaxSize);

                    subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.TreeHard, size);
                }

            }
        }
    }

    enum TerrainMainType
    {
        DefaultLand,
        DefaultSea,
        Destroyed,

        Foil,
        Terrain,
        Building,
        NUM
    }

    enum TerrainSubFoilType
    {
        TreeHardSprout,
        TreeSoftSprout,
        TreeHard,
        TreeSoft,
        Bush,
        Stones,
        NUM
    }
}
