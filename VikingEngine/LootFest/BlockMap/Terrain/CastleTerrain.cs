using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.BlockMap
{
    class CastleTerrain : AbsLevelTerrain
    {
        public CastleTerrain(AbsLevel level)
            : base(level)
        {
            wallMaterials = HeightMapMaterials.Test;//new HeightMapMaterials(Data.MaterialType.black, Data.MaterialType.bricks_gray, 6, Data.MaterialType.block_gray);
            groundMaterials = HeightMapMaterials.Test;//new HeightMapMaterials(Data.MaterialType.medium_cool_brown, Data.MaterialType.bricks_gray, 4, Data.MaterialType.dirt_brown);
            roadMaterials = HeightMapMaterials.Test; //new HeightMapMaterials(Data.MaterialType.dark_magenta_red, Data.MaterialType.bricks_gray, 4, Data.MaterialType.dirt_brown);

            backgroundScenery = new Map.BackgroundSceneryData(true);
            wallEdge = false;
        }

        public override int mapHeight(IntVector2 worldXZ, MapBlockType mapType)
        {
            int result = Map.WorldPosition.ChunkStandardHeight;
            if (mapType == MapBlockType.Occupied)
            {
                result += WallHeight;
            }
            return result;
        }

        override protected void generateRoad(IntVector2 gridPos, VikingEngine.LootFest.Map.Chunk chunk, PcgRandom rnd)
        {
            var sq = level.squares.Get(gridPos);
            if (sq.type == MapBlockType.Road)
            {
                IntVector2 worldPos = level.toWorldXZ(gridPos);

                IntVector2 roadPos = IntVector2.Zero;
                
                for (roadPos.Y = 0; roadPos.Y < BlockMapLib.SquareBlockWidth; ++roadPos.Y)
                {
                
                    for (roadPos.X = 0; roadPos.X < BlockMapLib.SquareBlockWidth; ++roadPos.X)
                    {   
                        IntVector2 pos = worldPos + roadPos;
                        var wp = new Map.WorldPosition(pos, mapHeight(pos, MapBlockType.Open) - 1);

                        chunk.Set(wp, roadMaterials.topMaterial);
                    }
                }
            }
        }
    }
}
