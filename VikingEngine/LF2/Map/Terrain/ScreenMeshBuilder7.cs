using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Map.Terrain
{
    class ScreenMeshBuilder7
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

        Vector4 normLength;
        StaticCountingList<VertexPositionNormalTexture> vertices;
        StaticCountingList<int> indexDrawOrder;
        UInt16 totalVerticeIx = 0;
        float[,] shadowValue;
        public const int MaxPart = 1;
        ChunkGroup chunkGroup = new ChunkGroup();
        const int MaxArraySize = 92000;
        public ScreenMeshBuilder7()
        {
            
            vertices = new StaticCountingList<VertexPositionNormalTexture>(MaxArraySize);
            vertices.FillArrayWith(new VertexPositionNormalTexture());
            indexDrawOrder = new StaticCountingList<int>(MaxArraySize);
        }

        public Graphics.VertexAndIndexBuffer BuildScreen(ref byte[, ,] dataGrid,
            WorldPosition wp,
            Graphics.LFHeightMap heightMap, bool reload)
        {
            chunkGroup.Init(dataGrid, wp);
            int tileIx;
            totalVerticeIx = 0;
            vertices.ResetCounting();//vertices = new StaticCountingList<VertexPositionNormalTexture>(MaxArraySize);//vertices.ResetCounting();
            indexDrawOrder.ResetCounting();
            shadowValue = new float[WorldPosition.ChunkWidth, WorldPosition.ChunkWidth];

            byte material;

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
                            if (!chunkGroup.Get(gridPos.X, gridPos.Y + 1, gridPos.Z))//FACE, set dir and limit
                            {
                                tileIx = Data.MaterialBuilder.Materials[material].TopTiles.GetRandomTile(gridPos);
                                Graphics.ImageFile2 file = LF2.LootfestLib.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LF2.Data.Block.GetTerrainFace(gridPos,
                                    //FACE
                                    TopFaceIndex);

                                normLength = calcSSAO(
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
                                    normLength *= 1 - (0.2f * shadowValue[gridPos.X, gridPos.Z]);
                                }

                                addVertices(data, file, normLength);

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }
                            //BOTTOM
                            if (!chunkGroup.Get(gridPos.X, gridPos.Y - 1, gridPos.Z))//FACE, set dir and limit
                            {
                                tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.ImageFile2 file = LF2.LootfestLib.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LF2.Data.Block.GetTerrainFace(gridPos,
                                    //FACE
                                    BottomFaceIndex);

                                addVertices(data, file, Vector4.One);

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }

                            if (!chunkGroup.Get(gridPos.X, gridPos.Y, gridPos.Z + 1))//FACE, set dir and limit
                            {
                                tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.ImageFile2 file = LF2.LootfestLib.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LF2.Data.Block.GetTerrainFace(gridPos,
                                    //FACE
                                    FrontFaceIndex);
                                normLength = calcTopBottomSSAO(
                                    gridPos.X, gridPos.Y - 1, gridPos.Z + 1,
                                    gridPos.X, gridPos.Y + 1, gridPos.Z + 1
                                    );
                                addVertices(data, file, normLength);

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }

                            if (!chunkGroup.Get(gridPos.X, gridPos.Y, gridPos.Z - 1))//FACE, set dir and limit
                            {
                                tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.ImageFile2 file = LF2.LootfestLib.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LF2.Data.Block.GetTerrainFace(gridPos,
                                    //FACE
                                    BackFaceIndex);

                                normLength = calcTopBottomSSAO(
                                    gridPos.X, gridPos.Y - 1, gridPos.Z - 1,
                                    gridPos.X, gridPos.Y + 1, gridPos.Z - 1
                                    );
                                addVertices(data, file, normLength);

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }

                            if (!chunkGroup.Get(gridPos.X - 1, gridPos.Y, gridPos.Z))//FACE, set dir and limit
                            {
                                tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.ImageFile2 file = LF2.LootfestLib.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LF2.Data.Block.GetTerrainFace(gridPos,
                                    //FACE
                                    RightFaceIndex);

                                normLength = calcTopBottomSSAO(
                                    gridPos.X - 1, gridPos.Y - 1, gridPos.Z,
                                    gridPos.X - 1, gridPos.Y + 1, gridPos.Z
                                    );
                                addVertices(data, file, normLength);


                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }

                            if (!chunkGroup.Get(gridPos.X + 1, gridPos.Y, gridPos.Z))//FACE, set dir and limit
                            {
                                tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.ImageFile2 file = LF2.LootfestLib.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LF2.Data.Block.GetTerrainFace(gridPos,
                                    //FACE
                                    LeftFaceIndex);

                                normLength = calcTopBottomSSAO(
                                    gridPos.X + 1, gridPos.Y - 1, gridPos.Z,
                                    gridPos.X + 1, gridPos.Y + 1, gridPos.Z
                                    );
                                addVertices(data, file, normLength);

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
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

        void addVertices(Graphics.Face data, Graphics.ImageFile2 file, Vector4 normalLength)
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
        byte[, ,] north;
        byte[, ,] south;
        byte[, ,] west;
        byte[, ,] east;

        public void Init(byte[, ,] Center, Map.WorldPosition wp)
        {
            this.Center = Center;
            IntVector2 centerScreen = wp.ChunkGrindex;
            //Map.WorldPosition neighbor = wp;
            IntVector2 neighbor = centerScreen;
            --neighbor.Y;
            north = LfRef.chunks.GetScreen(neighbor).DataGrid;

            neighbor = centerScreen;
            ++neighbor.Y;
            south = LfRef.chunks.GetScreen(neighbor).DataGrid;

            neighbor = centerScreen;
            --neighbor.X;
            west = LfRef.chunks.GetScreen(neighbor).DataGrid;

            neighbor = centerScreen;
            ++neighbor.X;
            east = LfRef.chunks.GetScreen(neighbor).DataGrid;
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

