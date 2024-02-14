using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Map.Terrain
{
    //class ScreenMeshBuilder6
    //{
    //    IntVector3 gridPos = IntVector3.Zero;
    //    bool drawFace;
    //    const byte NoBlock = byte.MinValue;

    //    StaticCountingList<VertexPositionNormalTexture> vertices;// = new List<VertexPositionNormalTexture>(StandardArraySize);
    //    StaticCountingList<UInt16> indexDrawOrder;// = new List<ushort>(StandardArraySize);
    //    UInt16 totalVerticeIx = 0;
    //    float[,] shadowValue;
    //    public const int MaxPart = 1;


    //    public ScreenMeshBuilder6()
    //    {
    //        const int MaxArraySize = 64000;
    //        vertices = new StaticCountingList<VertexPositionNormalTexture>(MaxArraySize);
    //        vertices.FillArrayWith(new VertexPositionNormalTexture());
    //        indexDrawOrder = new StaticCountingList<ushort>(MaxArraySize);
    //    }

    //    public Graphics.VertexAndIndexBuffer BuildScreen(ref byte[, ,] dataGrid,
    //        WorldPosition wp,
    //        Graphics.LFHeightMap heightMap, bool reload) //, int part)
    //    {
    //        int tileIx;
    //        totalVerticeIx = 0;
    //        vertices.ResetCounting();
    //        indexDrawOrder.ResetCounting();
    //        shadowValue = new float[WorldPosition.ChunkWidth, WorldPosition.ChunkWidth];
            
    //        byte material;
    //        WorldPosition checkPos;

    //        for (gridPos.Y = WorldPosition.ChunkHeight - 1; gridPos.Y >= 0; --gridPos.Y)
    //        {
    //            bool checkTop = gridPos.Y < WorldPosition.MaxChunkY;
    //            bool checkBottom = gridPos.Y > 0;

    //            for (gridPos.Z = 0; gridPos.Z < WorldPosition.ChunkWidth; ++gridPos.Z)
    //            {
    //                bool checkFront = gridPos.Z < WorldPosition.MaxChunkXZ;
    //                bool checkBack = gridPos.Z > 0;

    //                for (gridPos.X = 0; gridPos.X < WorldPosition.ChunkWidth; ++gridPos.X)
    //                {
    //                    material = dataGrid[gridPos.X, gridPos.Y, gridPos.Z];

    //                    if (material != 0)
    //                    {
    //                        bool checkLeft = gridPos.X < WorldPosition.MaxChunkXZ;
    //                        bool checkRight = gridPos.X > 0;
    //                        //TOP
    //                        if (!checkTop || dataGrid[gridPos.X, gridPos.Y + 1, gridPos.Z] == 0)//FACE, set dir and limit
    //                        {
    //                            tileIx = Data.MaterialBuilder.Materials[material].TopTiles.GetRandomTile(gridPos);
    //                            Graphics.ImageFile2 file = LootFest.LootfestLib.Images.TileIxToImgeFile[tileIx];
    //                            Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(gridPos,
    //                                //FACE
    //                                CubeFace.Top);

    //                            //Special shadow value
    //                            if (shadowValue[gridPos.X, gridPos.Z] > 0)
    //                            {
    //                                data.Normal.Y += shadowValue[gridPos.X, gridPos.Z];
    //                            }

    //                            addVertices(data, file);

    //                            totalVerticeIx += BlockLib.NumVerticesPerFace;
    //                        }
    //                        //BOTTOM
    //                        if (checkBottom && dataGrid[gridPos.X, gridPos.Y - 1, gridPos.Z] == 0)//FACE, set dir and limit
    //                        {
    //                            tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
    //                            Graphics.ImageFile2 file = LootFest.LootfestLib.Images.TileIxToImgeFile[tileIx];
    //                            Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(gridPos,
    //                                //FACE
    //                                CubeFace.Bottom);

    //                            addVertices(data, file);

    //                            totalVerticeIx += BlockLib.NumVerticesPerFace;
    //                        }
    //                        //FRONT
    //                        if (checkFront)
    //                        {
    //                            drawFace = dataGrid[gridPos.X, gridPos.Y, gridPos.Z + 1] == NoBlock;
    //                        }
    //                        else
    //                        {
    //                            checkPos = wp;
    //                            checkPos.LocalBlockGrindex = gridPos;
    //                            checkPos.WorldGrindex.Z++;
    //                            drawFace = LfRef.chunks.Get(checkPos) == NoBlock;
    //                        }
    //                        if (drawFace)//FACE, set dir and limit
    //                        {
    //                            tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
    //                            Graphics.ImageFile2 file = LootFest.LootfestLib.Images.TileIxToImgeFile[tileIx];
    //                            Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(gridPos,
    //                                //FACE
    //                                CubeFace.Front);

    //                            addVertices(data, file);

    //                            totalVerticeIx += BlockLib.NumVerticesPerFace;
    //                        }
    //                        //BACK
    //                        if (checkBack)
    //                        {
    //                            drawFace = dataGrid[gridPos.X, gridPos.Y, gridPos.Z - 1] == NoBlock;
    //                        }
    //                        else
    //                        {
    //                            checkPos = wp;
    //                            checkPos.LocalBlockGrindex = gridPos;
    //                            checkPos.WorldGrindex.Z--;
    //                            drawFace = LfRef.chunks.Get(checkPos) == NoBlock;
    //                        }
    //                        if (drawFace)//FACE, set dir and limit
    //                        {
    //                            tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
    //                            Graphics.ImageFile2 file = LootFest.LootfestLib.Images.TileIxToImgeFile[tileIx];
    //                            Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(gridPos,
    //                                //FACE
    //                                CubeFace.Back);

    //                            addVertices(data, file);

    //                            totalVerticeIx += BlockLib.NumVerticesPerFace;
    //                        }
    //                        //RIGHT
    //                        if (checkRight)
    //                        {
    //                            drawFace = dataGrid[gridPos.X - 1, gridPos.Y, gridPos.Z] == NoBlock;
    //                        }
    //                        else
    //                        {
    //                            checkPos = wp;
    //                            checkPos.LocalBlockGrindex = gridPos;
    //                            checkPos.WorldGrindex.X--;
    //                            drawFace = LfRef.chunks.Get(checkPos) == NoBlock;
    //                        }
    //                        if (drawFace)//FACE, set dir and limit
    //                        {
    //                            tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
    //                            Graphics.ImageFile2 file = LootFest.LootfestLib.Images.TileIxToImgeFile[tileIx];
    //                            Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(gridPos,
    //                                //FACE
    //                                CubeFace.Right);

    //                            addVertices(data, file);

    //                            totalVerticeIx += BlockLib.NumVerticesPerFace;
    //                        }
    //                        //LEFT
    //                        if (checkLeft)
    //                        {
    //                            drawFace = dataGrid[gridPos.X + 1, gridPos.Y, gridPos.Z] == NoBlock;
    //                        }
    //                        else
    //                        {
    //                            checkPos = wp;
    //                            checkPos.LocalBlockGrindex = gridPos;
    //                            checkPos.WorldGrindex.X++;
    //                            drawFace = LfRef.chunks.Get(checkPos) == NoBlock;
    //                        }
    //                        if (drawFace)//FACE, set dir and limit
    //                        {
    //                            tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
    //                            Graphics.ImageFile2 file = LootFest.LootfestLib.Images.TileIxToImgeFile[tileIx];
    //                            Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(gridPos,
    //                                //FACE
    //                                CubeFace.Left);

    //                            addVertices(data, file);

    //                            totalVerticeIx += BlockLib.NumVerticesPerFace;
    //                        }

    //                        shadowValue[gridPos.X, gridPos.Z] = 1;
    //                    }
    //                    else
    //                    {
    //                        shadowValue[gridPos.X, gridPos.Z] -= 0.024f;
    //                    }
    //                }
    //            }
                    
    //        }


            

    //        wp.LocalBlockGrindex = IntVector3.Zero;
    //        //wp.UpdateWorldGridPos();
            
    //        return heightMap.BuildFromPolygons(vertices, indexDrawOrder,  LoadedTexture.NO_TEXTURE, wp.WorldGrindex, reload);
             
    //    }

    //    void addVertices(Graphics.Face data, Graphics.ImageFile2 file)
    //    {
    //        ushort verticeCount = (ushort)vertices.CountingLength;
    //        ushort verticeCountPlusOne = (ushort)(verticeCount + 1);
    //        ushort verticeCountPlusTwo = (ushort)(verticeCount + 2);
    //        ushort verticeCountPlusThree = (ushort)(verticeCount + 3);


    //        indexDrawOrder.SetCurrentAndMoveNext(verticeCount);
    //        indexDrawOrder.SetCurrentAndMoveNext(verticeCountPlusOne);
    //        indexDrawOrder.SetCurrentAndMoveNext(verticeCountPlusTwo);
    //        indexDrawOrder.SetCurrentAndMoveNext(verticeCountPlusTwo);
    //        indexDrawOrder.SetCurrentAndMoveNext(verticeCountPlusOne);
    //        indexDrawOrder.SetCurrentAndMoveNext(verticeCountPlusThree);
            
    //        vertices.CurrentMember.Position = data.Corner3;
    //        vertices.CurrentMember.Normal = data.Normal;
    //        vertices.CurrentMember.TextureCoordinate = file.SourcePolygonLowLeft;
    //        vertices.NextIndex();

    //        vertices.CurrentMember.Position = data.Corner1;
    //        vertices.CurrentMember.Normal = data.Normal;
    //        vertices.CurrentMember.TextureCoordinate = file.SourcePolygonTopLeft;
    //        vertices.NextIndex();

    //        vertices.CurrentMember.Position = data.Corner4;
    //        vertices.CurrentMember.Normal = data.Normal;
    //        vertices.CurrentMember.TextureCoordinate = file.SourcePolygonLowRight;
    //        vertices.NextIndex();

    //        vertices.CurrentMember.Position = data.Corner2;
    //        vertices.CurrentMember.Normal = data.Normal;
    //        vertices.CurrentMember.TextureCoordinate = file.SourcePolygonTopRight;
    //        vertices.NextIndex();

    //        totalVerticeIx += BlockLib.NumVerticesPerFace;
    //    }

    //}
}

