using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.LootFest.Map;

namespace VikingEngine.DSSWars.Map
{
    class TerrainContent
    {
        public const int SproutMaxSize = 5;
        public const int TreeMaxSize = 100;

        public void asyncFoilGroth(IntVector2 pos, SubTile subtile)
        {
            switch ((Map.SubTileFoilType)subtile.undertype)
            {

                case Map.SubTileFoilType.TreeHard:
                    {
                        if (subtile.typeValue < TreeMaxSize)
                        {
                            subtile.typeValue++;
                            DssRef.world.subTileGrid.Set(pos, subtile);
                        }

                        if (Ref.rnd.Chance(0.2) && subtile.typeValue > 20 && subtile.typeValue < 90)
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
                                if (ntile.maintype == Map.SubTileMainType.DefaultLand)
                                {
                                    ntile.SetType(Map.SubTileMainType.Foil, (int)Map.SubTileFoilType.TreeHardSprout, 1);

                                    DssRef.world.subTileGrid.Set(npos, ntile);
                                }
                            }

                        }
                    }
                    break;

                case Map.SubTileFoilType.TreeHardSprout:
                    {
                        if (++subtile.typeValue > SproutMaxSize)
                        {
                            subtile.SetType(Map.SubTileMainType.Foil, (int)Map.SubTileFoilType.TreeHard, 1);
                        }

                        DssRef.world.subTileGrid.Set(pos, subtile);
                    }
                    break;
            }
        }

        public static void createSubTileContent(int x, int y, Tile tile, ref SubTile subTile, 
            WorldData world, VikingEngine.EngineSpace.Maths.SimplexNoise2D noiseMap)
        {
            var percTree = tile.heightSett().percTree;
            if (percTree > 0)
            {
                float noise = noiseMap.OctaveNoise2D_Normal(4, 0.75f, 1, x, y);

                if (noise < percTree && noise < world.rnd.Double(percTree * 2f))
                {
                    int size = (int)((1.0 - Math.Min(noise, world.rnd.Double())) * TreeMaxSize);

                    subTile.SetType(SubTileMainType.Foil, (int)SubTileFoilType.TreeHard, size);
                }

            }
        }
    }
}
