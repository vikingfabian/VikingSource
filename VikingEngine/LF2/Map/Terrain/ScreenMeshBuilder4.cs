using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using VikingEngine.Voxels;

namespace VikingEngine.LF2.Map.Terrain
{
    class ScreenMeshBuilder4 : AbsScreenMeshBuilder
    {
        override public Graphics.VertexAndIndexBuffer BuildScreen(ref byte[, ,] dataGrid,
            WorldPosition wp,
            Graphics.LFHeightMap heightMap,
            List<Graphics.PolygonColor> polygons, int part)
        {
            Voxels.FaceCornerColorYS.Init();
            //Go through the whole data grid of the screen
            //Fill the prepared chunk
            const byte Empty = 0;
            const byte Filled = 1;
            IntVector3 gridPos = IntVector3.Zero;
            IntVector3 preChunkPos = IntVector3.Zero;
            for (gridPos.Y = 0; gridPos.Y < WorldPosition.ChunkHeight; gridPos.Y++)
            {
                preChunkPos.Y = gridPos.Y + 1;
                for (gridPos.Z = 0; gridPos.Z < WorldPosition.ChunkWidth; gridPos.Z++)
                {
                    preChunkPos.Z = gridPos.Z + 1;
                    for (gridPos.X = 0; gridPos.X < WorldPosition.ChunkWidth; gridPos.X++)
                    {
                        preChunkPos.X = gridPos.X + 1;

                        preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z] =
                            dataGrid[gridPos.X, gridPos.Y, gridPos.Z] == Empty ? Empty : Filled;
                    }
                }
            }
            //Fill the prepared chunk with its sourrounding blocks
            for (Facing8Dir dir = (Facing8Dir)0; dir < Facing8Dir.NUM; dir++)
            {
                WorldPosition nScreen = wp;
                nScreen.ChunkGrindex.Add(IntVector2.FromFacing8Dir(dir));
                if (nScreen.CorrectPos)
                {
                    LfRef.chunks.GetScreen(nScreen).AddToPreparedChunk(dir, ref preparedChunk);
                }

            }
            PreparedFaceCorners ssao;
            //The chunk is now prepared
            //Go through the prepared chunk and generate the mesh
            byte material;
            for (gridPos.Y = 0; gridPos.Y < WorldPosition.ChunkHeight; gridPos.Y++)
            {
                preChunkPos.Y = gridPos.Y + 1;
                for (gridPos.Z = 0; gridPos.Z < WorldPosition.ChunkWidth; gridPos.Z++)
                {
                    preChunkPos.Z = gridPos.Z + 1;
                    for (gridPos.X = 0; gridPos.X < WorldPosition.ChunkWidth; gridPos.X++)
                    {
                        material = dataGrid[gridPos.X, gridPos.Y, gridPos.Z];

                        if (material != 0)
                        {


                            preChunkPos.X = gridPos.X + 1;
                            //Go through all faces

                            //TOP
                            if (preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z] == 0)
                            {
                                ssao = calcSSAO(
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z + 1],

                                    preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z + 1]
                                    );
                                
                                polygons.Add(new Graphics.PolygonColor(
                                    FaceTypeTop, 
                                    Data.MaterialBuilder.Materials[material].TopTiles.GetRandomTile(gridPos), 
                                    ssao.Corner0, ssao.Corner1, ssao.Corner2, ssao.Corner3, 
                                    gridPos));


                            }

                            //FRONT
                            if (preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z + 1] == 0)
                            {
                                ssao = calcSSAO(
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y - 1, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y - 1, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y - 1, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z + 1],

                                    preparedChunk[preChunkPos.X, preChunkPos.Y - 1, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z]
                                );

                                polygons.Add(new Graphics.PolygonColor(
                                    FaceTypeFront,
                                    Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos),
                                    ssao.Corner0, ssao.Corner1, ssao.Corner2, ssao.Corner3,
                                    gridPos));
                            }
                            //BACK
                            if (preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z - 1] == 0)
                            {
                                #region CalcSSAO
                                ssao = calcSSAO(
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y-1, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y-1, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y-1, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X -1, preChunkPos.Y, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X +1, preChunkPos.Y, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X-1, preChunkPos.Y+1, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y+1, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X+1, preChunkPos.Y+1, preChunkPos.Z - 1],
                                    
                                    preparedChunk[preChunkPos.X, preChunkPos.Y-1, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X-1, preChunkPos.Y, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X+1, preChunkPos.Y, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y+1, preChunkPos.Z]
                                );
                                #endregion
                                polygons.Add(new Graphics.PolygonColor(
                                    FaceTypeBack,
                                    Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos),
                                    ssao.Corner0, ssao.Corner1, ssao.Corner2, ssao.Corner3,
                                    gridPos));
                            }
                            //LEFT
                            if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y, preChunkPos.Z] == 0)
                            {
                                ssao = calcSSAO(
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y - 1, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y - 1, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y - 1, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z + 1],

                                    preparedChunk[preChunkPos.X, preChunkPos.Y - 1, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z]
                                );

                                polygons.Add(new Graphics.PolygonColor(
                                    FaceTypeLeft,
                                    Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos),
                                    ssao.Corner0, ssao.Corner1, ssao.Corner2, ssao.Corner3,
                                    gridPos));
                            }
                            //RIGHT
                            if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y, preChunkPos.Z] == 0)
                            {
                                ssao = calcSSAO(
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y - 1, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y - 1, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y - 1, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z - 1],

                                    preparedChunk[preChunkPos.X, preChunkPos.Y - 1, preChunkPos.Z],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z + 1],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z - 1],
                                    preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z]
                                );

                                polygons.Add(new Graphics.PolygonColor(
                                    FaceTypeRight,
                                    Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos),
                                    ssao.Corner0, ssao.Corner1, ssao.Corner2, ssao.Corner3,
                                    gridPos));
                            }
                        }
                    }
                }
            }

            wp.ClearLocalPos(); //.LocalBlockGrindex = IntVector3.Zero;
            return null;
        }


        static PreparedFaceCorners calcSSAO(int above1, int above2, int above3, int above4, int above5, int above6, int above7, int above8,
            int below1, int below2, int below3, int below4)
        {
            PreparedFaceCorners ssao = PreparedFaceCorners.Two;
            if (above2 == 1)
            {//north
                ssao.Corner0++;
                ssao.Corner1++;
            }
            else if (below1 == 0)
            {//north //White edges
                ssao.Corner0--;
                ssao.Corner1--;
            }
            if (above4 == 1)
            {//west

                ssao.Corner0++;
                ssao.Corner2++;
            }
            else if (below2 == 0)
            {//west //White edges

                ssao.Corner0--;
                ssao.Corner2--;
            }
            if (above5 == 1)
            {//east
                ssao.Corner1++;
                ssao.Corner3++;
            }
            else if (below3 == 0)
            {//east //White edges
                ssao.Corner1--;
                ssao.Corner3--;
            }
            if (above7 == 1)
            {//south
                ssao.Corner2++;
                ssao.Corner3++;
            }
            else if (below4 == 0)
            {//south //White edges
                ssao.Corner2--;
                ssao.Corner3--;
            }
            ////CORNERS
            if (above1 == 1)
            {//north
                ssao.Corner0++;
            }
            if (above3 == 1)
            {//north
                ssao.Corner1++;
            }
            if (above6 == 1)
            {//SW
                ssao.Corner2++;
            }
            if (above8 == 1)
            {//SE
                ssao.Corner3++;
            }
            return ssao;
        }

    }
}
