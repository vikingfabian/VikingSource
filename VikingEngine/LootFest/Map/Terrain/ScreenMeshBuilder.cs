using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Map.Terrain
{
    class ScreenMeshBuilder
    {
        static readonly int TopFaceIndex = (int)CubeFace.Ypositive;
        static readonly int BottomFaceIndex = (int)CubeFace.Ynegative;
        static readonly int FrontFaceIndex = (int)CubeFace.Zpositive;
        static readonly int BackFaceIndex = (int)CubeFace.Znegative;
        static readonly int LeftFaceIndex = (int)CubeFace.Xpositive;
        static readonly int RightFaceIndex = (int)CubeFace.Xnegative;

        IntVector3 gridPos = IntVector3.Zero;
        //bool drawFace;
        const byte NoBlock = byte.MinValue;

        StaticCountingList<VertexPositionNormalTexture> vertices;
        StaticCountingList<int> indexDrawOrder;
        UInt16 totalVerticeIx = 0;
        float[,] shadowValue;
        public const int MaxPart = 1;
        ChunkGroup chunkGroup = new ChunkGroup();
        public const int MaxArraySize = 70000;

        public ScreenMeshBuilder()
        {
            vertices = new StaticCountingList<VertexPositionNormalTexture>(MaxArraySize);
            vertices.FillArrayWith(new VertexPositionNormalTexture());
            indexDrawOrder = new StaticCountingList<int>(MaxArraySize + MaxArraySize / 2);
        }

        public Graphics.VertexAndIndexBuffer BuildScreen(byte[, ,] dataGrid,
            WorldPosition wp,
            Graphics.LFHeightMap heightMap, bool reload)
        {
            chunkGroup.Init(dataGrid, wp);
            totalVerticeIx = 0;
            vertices.ResetCounting();//vertices = new StaticCountingList<VertexPositionNormalTexture>(MaxArraySize);//vertices.ResetCounting();
            indexDrawOrder.ResetCounting();
            shadowValue = new float[WorldPosition.ChunkWidth, WorldPosition.ChunkWidth];

            byte material;
            byte[, ,] grid;
            int sideVar;

            for (gridPos.Y = WorldPosition.ChunkHeight - 1; gridPos.Y >= 0; --gridPos.Y)
            {
                for (gridPos.Z = 0; gridPos.Z < WorldPosition.ChunkWidth; ++gridPos.Z)
                {
                    for (gridPos.X = 0; gridPos.X < WorldPosition.ChunkWidth; ++gridPos.X)
                    {
                        material = dataGrid[gridPos.X, gridPos.Y, gridPos.Z];

                        if (material != 0)
                        {
                            //TOP
                            grid = chunkGroup.Center;
                            sideVar = gridPos.Y + 1;
                            if (sideVar >= WorldPosition.ChunkHeight || grid[gridPos.X, sideVar, gridPos.Z] == 0)
                            {
                                Vector4 normal = calcSSAO(
                                    gridPos.X - 1, gridPos.Y + 1, gridPos.Z,
                                    gridPos.X, gridPos.Y + 1, gridPos.Z - 1,
                                    gridPos.X + 1, gridPos.Y + 1, gridPos.Z,
                                    gridPos.X, gridPos.Y + 1, gridPos.Z + 1,

                                    gridPos.X - 1, gridPos.Y, gridPos.Z,
                                    gridPos.X, gridPos.Y, gridPos.Z - 1,
                                    gridPos.X + 1, gridPos.Y, gridPos.Z,
                                    gridPos.X, gridPos.Y, gridPos.Z + 1);

                                if (shadowValue[gridPos.X, gridPos.Z] > 0)
                                {
                                    //Special shadow value
                                    normal *= 1 - (0.2f * shadowValue[gridPos.X, gridPos.Z]);
                                }

                                int tileIndex = Data.BlockTextures.Materials[material].TopTiles.GetRandomTile(gridPos);

                                AddFace(TopFaceIndex, tileIndex, normal);
                            }
                            //BOTTOM
                            sideVar = gridPos.Y - 1;
                            if (sideVar >= 0 && grid[gridPos.X, sideVar, gridPos.Z] == 0)
                            {
                                int tileIndex = Data.BlockTextures.Materials[material].SideTiles.GetRandomTile(gridPos);
                                AddFace(BottomFaceIndex, tileIndex, Vector4.One);
                            }

                            // Front
                            sideVar = gridPos.Z + 1;
                            if (sideVar >= Map.WorldPosition.ChunkWidth)
                            {
                                grid = chunkGroup.south;
                                sideVar = 0;
                            }
                            if (grid[gridPos.X, gridPos.Y, sideVar] == 0)
                            {
                                int tileIndex = Data.BlockTextures.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Vector4 normal = CalculateSSAONormal(gridPos.X, gridPos.Y, gridPos.Z + 1);
                                AddFace(FrontFaceIndex, tileIndex, normal);
                            }

                            // Back
                            sideVar = gridPos.Z - 1;
                            if (sideVar < 0)
                            {
                                grid = chunkGroup.north;
                                sideVar = WorldPosition.MaxChunkXZ;
                            }
                            else
                            {
                                grid = chunkGroup.Center;
                            }

                            if (grid[gridPos.X, gridPos.Y, sideVar] == 0)
                            {
                                int tileIndex = Data.BlockTextures.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Vector4 normal = CalculateSSAONormal(gridPos.X, gridPos.Y, gridPos.Z - 1);
                                AddFace(BackFaceIndex, tileIndex, normal);
                            }

                            // Right
                            sideVar = gridPos.X - 1;
                            if (sideVar < 0)
                            {
                                grid = chunkGroup.west;
                                sideVar = WorldPosition.MaxChunkXZ;
                            }
                            else
                            {
                                grid = chunkGroup.Center;
                            }

                            if (grid[sideVar, gridPos.Y, gridPos.Z] == 0)
                            {
                                int tileIndex = Data.BlockTextures.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Vector4 normal = CalculateSSAONormal(gridPos.X - 1, gridPos.Y, gridPos.Z);
                                AddFace(RightFaceIndex, tileIndex, normal);
                            }

                            // Left
                            sideVar = gridPos.X + 1;
                            if (sideVar >= WorldPosition.ChunkWidth)
                            {
                                grid = chunkGroup.east;
                                sideVar = 0;
                            }
                            else
                            {
                                grid = chunkGroup.Center;
                            }

                            if (grid[sideVar, gridPos.Y, gridPos.Z] == 0)
                            {
                                int tileIndex = Data.BlockTextures.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Vector4 normal = CalculateSSAONormal(gridPos.X + 1, gridPos.Y, gridPos.Z);
                                AddFace(LeftFaceIndex, tileIndex, normal);
                            }

                            shadowValue[gridPos.X, gridPos.Z] = 1;
                        }
                        else
                        {
                            shadowValue[gridPos.X, gridPos.Z] -= 0.024f;
                        }
                    }
                }

            }
            wp.ClearLocalPos();

            return heightMap.BuildFromPolygons(vertices, indexDrawOrder, LoadedTexture.NO_TEXTURE, wp.ChunkGrindex, wp.WorldGrindex, reload);

        }

        void AddFace(int faceIndex, int tileIndex, Vector4 normal)
        {
            Graphics.Sprite file = LootFest.LfRef.Images.TileIxToImgeFile[tileIndex];
            Graphics.Face data = LootFest.Data.Block.GetTerrainFace(gridPos, faceIndex);

            addVertices(data, file, normal);

            totalVerticeIx += BlockLib.NumVerticesPerFace;
        }

        Vector4 CalculateSSAONormal(int x, int y, int z)
        {
            return calcTopBottomSSAO(x, y - 1, z,
                                     x, y + 1, z);
        }

        Vector4 calcSSAO(
            int shadowSide1X, int shadowSide1Y, int shadowSide1Z,
            int shadowSide2X, int shadowSide2Y, int shadowSide2Z,
            int shadowSide3X, int shadowSide3Y, int shadowSide3Z,
            int shadowSide4X, int shadowSide4Y, int shadowSide4Z)
        {
            const float ShadowLengthReduce = 0.2f;
            Vector4 cornerNormLength = Vector4.One;

            if (chunkGroup.Get(shadowSide1X, shadowSide1Y, shadowSide1Z))
            {
                cornerNormLength.X -= ShadowLengthReduce;
                cornerNormLength.Z -= ShadowLengthReduce;
            }
            if (chunkGroup.Get(shadowSide2X, shadowSide2Y, shadowSide2Z))
            {
                cornerNormLength.X -= ShadowLengthReduce;
                cornerNormLength.Y -= ShadowLengthReduce;
            }
            if (chunkGroup.Get(shadowSide3X, shadowSide3Y, shadowSide3Z))
            {
                cornerNormLength.Y -= ShadowLengthReduce;
                cornerNormLength.W -= ShadowLengthReduce;
            }
            if (chunkGroup.Get(shadowSide4X, shadowSide4Y, shadowSide4Z))
            {
                cornerNormLength.W -= ShadowLengthReduce;
                cornerNormLength.Z -= ShadowLengthReduce;
            }

            return cornerNormLength;
        }

        Vector4 calcSSAO(
            int shadowSide1X, int shadowSide1Y, int shadowSide1Z,
            int shadowSide2X, int shadowSide2Y, int shadowSide2Z,
            int shadowSide3X, int shadowSide3Y, int shadowSide3Z,
            int shadowSide4X, int shadowSide4Y, int shadowSide4Z,

            int brightSide1X, int brightSide1Y, int brightSide1Z,
            int brightSide2X, int brightSide2Y, int brightSide2Z,
            int brightSide3X, int brightSide3Y, int brightSide3Z,
            int brightSide4X, int brightSide4Y, int brightSide4Z)
        {
            const float ShadowLengthReduce = 0.24f;
            const float BrightLengthAdd = 0.08f;
            Vector4 cornerNormLength = Vector4.One;

            if (chunkGroup.Get(shadowSide1X, shadowSide1Y, shadowSide1Z))
            {
                cornerNormLength.X -= ShadowLengthReduce;
                cornerNormLength.Z -= ShadowLengthReduce;
            }
            else if (!chunkGroup.Get(brightSide1X, brightSide1Y, brightSide1Z))
            {
                cornerNormLength.X += BrightLengthAdd;
                cornerNormLength.Z += BrightLengthAdd;
            }

            if (chunkGroup.Get(shadowSide2X, shadowSide2Y, shadowSide2Z))
            {
                cornerNormLength.X -= ShadowLengthReduce;
                cornerNormLength.Y -= ShadowLengthReduce;
            }
            else if (!chunkGroup.Get(brightSide2X, brightSide2Y, brightSide2Z))
            {
                cornerNormLength.X += BrightLengthAdd;
                cornerNormLength.Y += BrightLengthAdd;
            }

            if (chunkGroup.Get(shadowSide3X, shadowSide3Y, shadowSide3Z))
            {
                cornerNormLength.Y -= ShadowLengthReduce;
                cornerNormLength.W -= ShadowLengthReduce;
            }
            else if (!chunkGroup.Get(brightSide3X, brightSide3Y, brightSide3Z))
            {
                cornerNormLength.Y += BrightLengthAdd;
                cornerNormLength.W += BrightLengthAdd;
            }

            if (chunkGroup.Get(shadowSide4X, shadowSide4Y, shadowSide4Z))
            {
                cornerNormLength.W -= ShadowLengthReduce;
                cornerNormLength.Z -= ShadowLengthReduce;
            }
            else if (!chunkGroup.Get(brightSide4X, brightSide4Y, brightSide4Z))
            {
                cornerNormLength.W += BrightLengthAdd;
                cornerNormLength.Z += BrightLengthAdd;
            }


            return cornerNormLength;
        }


        Vector4 calcBottomSSAO(
            int shadowSide1X, int shadowSide1Y, int shadowSide1Z)
        {
            const float ShadowLengthReduce = 0.25f;
            Vector4 cornerNormLength = Vector4.One;

            if (chunkGroup.Get(shadowSide1X, shadowSide1Y, shadowSide1Z))
            {
                cornerNormLength.W -= ShadowLengthReduce;
                cornerNormLength.Z -= ShadowLengthReduce;
            }

            return cornerNormLength;
        }

        Vector4 calcTopBottomSSAO(
            int shadowSide1X, int shadowSide1Y, int shadowSide1Z,
            int shadowSide2X, int shadowSide2Y, int shadowSide2Z)
        {
            const float ShadowLengthReduce = 0.2f;
            Vector4 cornerNormLength = Vector4.One;

            if (chunkGroup.Get(shadowSide1X, shadowSide1Y, shadowSide1Z)) //below
            {
                cornerNormLength.W -= ShadowLengthReduce;
                cornerNormLength.Z -= ShadowLengthReduce;
            }
            if (chunkGroup.Get(shadowSide2X, shadowSide2Y, shadowSide2Z)) //above
            {
                cornerNormLength.X -= ShadowLengthReduce;
                cornerNormLength.Y -= ShadowLengthReduce;
            }
            return cornerNormLength;
        }

        void addVertices(Graphics.Face data, Graphics.Sprite file, Vector4 normalLength)
        {
            int verticeCount = vertices.CountingLength;
            int verticeCountPlusOne = (verticeCount + 1);
            int verticeCountPlusTwo = (verticeCount + 2);
            int verticeCountPlusThree = (verticeCount + 3);

            indexDrawOrder.SetCurrentAndMoveNext(verticeCount);
            indexDrawOrder.SetCurrentAndMoveNext(verticeCountPlusOne);
            indexDrawOrder.SetCurrentAndMoveNext(verticeCountPlusTwo);
            indexDrawOrder.SetCurrentAndMoveNext(verticeCountPlusTwo);
            indexDrawOrder.SetCurrentAndMoveNext(verticeCountPlusOne);
            indexDrawOrder.SetCurrentAndMoveNext(verticeCountPlusThree);

            vertices.CurrentMember.Position = data.Corner3;
            vertices.CurrentMember.Normal = data.Normal * normalLength.Z;
            vertices.CurrentMember.TextureCoordinate = file.SourcePolygonLowLeft;
            vertices.NextIndex();

            vertices.CurrentMember.Position = data.Corner1;
            vertices.CurrentMember.Normal = data.Normal * normalLength.X;
            vertices.CurrentMember.TextureCoordinate = file.SourcePolygonTopLeft;
            vertices.NextIndex();

            vertices.CurrentMember.Position = data.Corner4;
            vertices.CurrentMember.Normal = data.Normal * normalLength.W;
            vertices.CurrentMember.TextureCoordinate = file.SourcePolygonLowRight;
            vertices.NextIndex();

            vertices.CurrentMember.Position = data.Corner2;
            vertices.CurrentMember.Normal = data.Normal * normalLength.Y;
            vertices.CurrentMember.TextureCoordinate = file.SourcePolygonTopRight;
            vertices.NextIndex();

            totalVerticeIx += BlockLib.NumVerticesPerFace;
        }

    }

    class ChunkGroup
    {
        const byte NoBlock = byte.MinValue;

        public byte[, ,] Center;
        public byte[, ,] north;
        public byte[, ,] south;
        public byte[, ,] west;
        public byte[, ,] east;

        public void Init(byte[, ,] Center, Map.WorldPosition wp)
        {
            //this.Center = Center;
            //IntVector2 centerScreen = wp.ChunkGrindex;
            ////Map.WorldPosition neighbor = wp;
            //IntVector2 neighbor = centerScreen;
            //--neighbor.Y;
            //north = LfRef.chunks.GetScreen(neighbor).DataGrid;

            //neighbor = centerScreen;
            //++neighbor.Y;
            //south = LfRef.chunks.GetScreen(neighbor).DataGrid;

            //neighbor = centerScreen;
            //--neighbor.X;
            //west = LfRef.chunks.GetScreen(neighbor).DataGrid;

            //neighbor = centerScreen;
            //++neighbor.X;
            //east = LfRef.chunks.GetScreen(neighbor).DataGrid;
        }

        public bool GetUnsafe(int x, int y, int z)
        {
            return Center[x, y, z] != 0;
        }

        public bool Get(int x, int y, int z)
        {
            //inside the center chunk
            if (x >= 0 && x < Map.WorldPosition.ChunkWidth &&
                y >= 0 && y < Map.WorldPosition.ChunkHeight &&
                z >= 0 && z < Map.WorldPosition.ChunkWidth)
            {
                return Center[x, y, z] != 0;
            }

            //Outside the chunk
            if (y < 0) return true;
            if (y >= WorldPosition.ChunkHeight) return false;
            if (x < 0)
                return west[Map.WorldPosition.MaxChunkXZ, y, z] != NoBlock;
            if (x >= Map.WorldPosition.ChunkWidth)
                return east[0, y, z] != NoBlock;

            if (z < 0)
                return north[x, y, Map.WorldPosition.MaxChunkXZ] != NoBlock;
            if (z >= Map.WorldPosition.ChunkWidth)
                return south[x, y, 0] != NoBlock;

            return false;
        }

    }

}

