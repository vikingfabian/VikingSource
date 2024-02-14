using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.BlockMap
{
    abstract class AbsLevelTerrain
    {
        protected AbsLevel level;
        public VikingEngine.LootFest.Map.BackgroundSceneryData backgroundScenery;
        //protected bool jaggedRoadEdge = true, 
        protected bool  wallEdge = true;

        public AbsLevelTerrain(AbsLevel level)
        {
            this.level = level;
        }


        //static readonly HeightMapMaterials DebugMaterial = new HeightMapMaterials(Data.MaterialType.pure_magenta, Data.MaterialType.pure_cyan, 1, Data.MaterialType.pure_magenta);
        HeightMapMaterials waterMaterial = HeightMapMaterials.Test;//new HeightMapMaterials(Data.MaterialType.light_blue, Data.MaterialType.dark_blue, 4, Data.MaterialType.dirt_sand);

        public HeightMapMaterials wallMaterials, groundMaterials, roadMaterials;

        public void heightmap(IntVector2 chunk, int x, int z, BlockMapSquare sq, ref int height, out HeightMapMaterials material)
        {
            IntVector2 worldXZ = new IntVector2(chunk.X * Map.WorldPosition.ChunkWidth + x, chunk.Y * Map.WorldPosition.ChunkWidth + z);

            //int squareX = (chunk.X - level.topLeftChunk.X) * BlockMapLib.SquaresPerChunkW + x / BlockMapLib.SquareBlockWidth;
            //int squareY = (chunk.Y - level.topLeftChunk.Y) * BlockMapLib.SquaresPerChunkW + z / BlockMapLib.SquareBlockWidth;

            //var sq = level.squares.Get(squareX, squareY);

            height = mapHeight(worldXZ, sq.type);

            switch (sq.type)
            {
                case MapBlockType.Occupied:
                    material = wallMaterials;
                    break;
                case MapBlockType.Water:
                    material = waterMaterial;
                    break;
                default://Open
                    material = groundMaterials;
                    break;
            }

            //if (sq.special == MapBlockSpecialType.SpawnPos)
            //{
            //    material = DebugMaterial;
            //}
        }

        protected const int WallHeight = 32;
        const int GroundStartHeight = 40;

        virtual public int mapHeight(IntVector2 worldXZ, MapBlockType mapType)
        {
            if (level == null)
                return 3;

            if (mapType == MapBlockType.Water)
            {
                return GroundStartHeight - 6;
            }
            else
            {
                float heightNoise = level.heightMap.OctaveNoise2D(5, 0.5f, 2, worldXZ.X, worldXZ.Y);

                int height = GroundStartHeight;
                height += (int)(8 * heightNoise);
                height = (int)MathHelper.Clamp(height, 0, Map.WorldPosition.MaxChunkY);

                if (mapType == MapBlockType.Occupied)
                {
                    height += WallHeight;
                }

                return height;
            }
            
        }


        public void generateChunkDetail(VikingEngine.LootFest.Map.Chunk chunk, IntVector2 gridPos, PcgRandom rnd)//)
        {
            //PcgRandom rnd = new PcgRandom(level.mySeed + chunk.Index.X * 3 + chunk.Index.Y * 11);

            //IntVector2 localChunkPos = chunk.Index - level.topLeftChunk;

            //IntVector2 gridPosStart = localChunkPos * BlockMapLib.SquaresPerChunkW;
            //IntVector2 gridPosEnd = gridPosStart + BlockMapLib.SquaresPerChunkW;

            //IntVector2 gridPos = IntVector2.Zero;
            //for (gridPos.Y = gridPosStart.Y; gridPos.Y < gridPosEnd.Y; ++gridPos.Y)
            //{
            //    for (gridPos.X = gridPosStart.X; gridPos.X < gridPosEnd.X; ++gridPos.X)
            //    {
            if (wallEdge)
            {
                generateWallEdges(gridPos, chunk, rnd);
            }
            generateRoad(gridPos, chunk, rnd);
            //    }
            //}
        }

        public void fillUpGround(Map.WorldPosition wp, int fromY)
        {
            for (wp.WorldGrindex.Y = fromY; wp.WorldGrindex.Y >= 0; --wp.WorldGrindex.Y)
            {
                if (wp.BlockIsEmpty())
                {
                    wp.SetBlock(groundMaterials.topMaterial);
                }
                else
                {
                    return;
                }
            }
        }

        const float FullRoadChance = 1f;
        static readonly float[] EdgeRoadChance = new float[]
        {
             0.4f,
             0.9f,
             FullRoadChance,
             FullRoadChance,
             FullRoadChance,
        };

        static bool[] AdjacentRoads = new bool[4];

        virtual protected void generateRoad(IntVector2 gridPos, VikingEngine.LootFest.Map.Chunk chunk, PcgRandom rnd)
        {
            //var sq = level.squares.Get(gridPos);
            //if (sq.type == MapBlockType.Road)
            //{
            //    for (int i = 0; i < IntVector2.Dir4Array.Length; ++i)
            //    {
            //        AdjacentRoads[i] = level.squares.Get(gridPos + IntVector2.Dir4Array[i]).type == MapBlockType.Road;
            //    }

            //    IntVector2 worldPos = level.toWorldXZ(gridPos);

            //    IntVector2 roadPos = IntVector2.Zero;
            //    int distanceFromEdgeX = 0;
            //    int distanceFromEdgeY = 0;

            //    bool roadNeighborX = false, roadNeighborY = false;

            //    for (roadPos.Y = 0; roadPos.Y < BlockMapLib.SquareBlockWidth; ++roadPos.Y)
            //    {
            //        if (roadPos.Y < BlockMapLib.SquareHalfWidth)
            //        {
            //            distanceFromEdgeY = roadPos.Y;
            //            roadNeighborY = AdjacentRoads[0];
            //        }
            //        else
            //        {
            //            distanceFromEdgeY = BlockMapLib.SquareBlockWidth - roadPos.Y - 1;
            //            roadNeighborY = AdjacentRoads[2];
            //        }
                    

            //        for (roadPos.X = 0; roadPos.X < BlockMapLib.SquareBlockWidth; ++roadPos.X)
            //        {
            //            //bool placeRoad = true;

                        
            //            if (roadPos.X < BlockMapLib.SquareHalfWidth)
            //            {
            //                distanceFromEdgeX = roadPos.X;
            //                roadNeighborX = AdjacentRoads[3];
            //            }
            //            else
            //            {
            //                distanceFromEdgeX = BlockMapLib.SquareBlockWidth - roadPos.X - 1;
            //                roadNeighborX = AdjacentRoads[1];
            //            }

            //            float roadChance = (roadNeighborX ? FullRoadChance : EdgeRoadChance[distanceFromEdgeX]) * (roadNeighborY ? FullRoadChance : EdgeRoadChance[distanceFromEdgeY]);
            //            //placeRoad = rnd.RandomChance(roadChance);

            //            if (rnd.RandomChance(roadChance))
            //            {
            //                IntVector2 pos = worldPos + roadPos;
            //                var wp = new Map.WorldPosition(pos, mapHeight(pos, MapBlockType.Open) - 1);

            //                chunk.voxels.Set(wp, Map.HDvoxel.Block.Empty);
            //                wp.Y--;
            //                chunk.voxels.Set(wp, new Map.HDvoxel.Block( roadMaterials.topMaterial));
            //            }
            //        }
            //    }
            //}
        }

        void generateWallEdges(IntVector2 gridPos, VikingEngine.LootFest.Map.Chunk chunk, PcgRandom rnd)
        {
            var sq = level.squares.Get(gridPos);
            if (sq.type == MapBlockType.Occupied)
            {
                for (int i = 0; i < IntVector2.Dir4Array.Length; ++i)
                {
                    IntVector2 nPos = gridPos + IntVector2.Dir4Array[i];

                    if (level.squares.InBounds(nPos))
                    {
                        if (level.squares.Get(nPos).type != MapBlockType.Occupied)
                        {
                            //Edge to open area
                            IntVector2 edgeFrom, edgeTo, outwardsDir;
                            switch (i)
                            {
                                default:
                                    edgeFrom = IntVector2.Zero;
                                    edgeTo = IntVector2.Right;
                                    break;
                                case 1:
                                    edgeFrom = IntVector2.Right;
                                    edgeTo = IntVector2.One;
                                    break;
                                case 2:
                                    edgeFrom = IntVector2.One;
                                    edgeTo = IntVector2.PositiveY;
                                    break;
                                case 3:
                                    edgeFrom = IntVector2.PositiveY;
                                    edgeTo = IntVector2.Zero;
                                    break;

                            }
                            outwardsDir = IntVector2.Dir4Array[i];

                            IntVector2 worldXZ = level.toWorldXZ(gridPos);
                            edgeFrom = worldXZ + edgeFrom * BlockMapLib.SquareBlockWidth;
                            edgeTo = worldXZ + edgeTo * BlockMapLib.SquareBlockWidth;

                            ForXYLoop loop = new ForXYLoop(Rectangle2.FromTwoPoints(edgeFrom, edgeTo));
                            while (loop.Next())
                            {
                                int height = WallHeight + 1;

                                IntVector2 xz = loop.Position;
                                //move inward first

                                xz += outwardsDir * -4;
                                if (rnd.Chance(0.3))
                                {
                                    if (rnd.Chance(0.05))
                                    { height -= 1; }
                                    generateWallDetail(xz, height, chunk, rnd);
                                }
                                xz += outwardsDir;
                                if (rnd.Chance(0.8))
                                {
                                    if (rnd.Chance(0.05))
                                    { height -= 1; }
                                    generateWallDetail(xz, height, chunk, rnd);
                                }
                                xz += outwardsDir;
                                if (rnd.Chance(0.95))
                                {
                                    if (rnd.Chance(0.05))
                                    { height -= 1; }
                                    generateWallDetail(xz, height, chunk, rnd);
                                }

                                if (rnd.Chance(0.9))
                                { height -= rnd.Int(1, 3); }
                                xz += outwardsDir;
                                generateWallDetail(xz, height, chunk, rnd);

                                if (rnd.Chance(0.95))
                                { height -= rnd.Int(1, 5); }
                                xz += outwardsDir;
                                generateWallDetail(xz, height, chunk, rnd);

                                if (rnd.Chance(0.2))
                                {
                                    height -= rnd.Int(0, 5);
                                    xz += outwardsDir;
                                    generateWallDetail(xz, height, chunk, rnd);
                                }
                            }
                        }
                    }
                }
            }
        }

        void generateWallDetail(IntVector2 worldXZ, int height, VikingEngine.LootFest.Map.Chunk chunk, PcgRandom rnd)
        {
            Map.WorldPosition wp = new Map.WorldPosition(worldXZ, mapHeight(worldXZ, MapBlockType.Open));

            int secMaterialHeight = WallHeight - wallMaterials.firstLayerHeight + rnd.Int(-1, 1);

            for (int y = 1; y < WallHeight + 1; ++y)
            {
                if (y < height)
                {
                    wp.SetBlockPattern(y < secMaterialHeight ? wallMaterials.bottomMaterial : wallMaterials.firstLayerMaterial);
                }
                else
                {
                    wp.SetBlockPattern(VikingEngine.LootFest.Map.HDvoxel.BlockHD.EmptyBlock);
                }
                ++wp.WorldGrindex.Y;
            }
        }
    }
     
    
}
