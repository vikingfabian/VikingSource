////#define WHITE_EDGES

//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;

//using Game1.Voxels;

//namespace Game1.LootFest.Map.Terrain
//{
//    class ScreenMeshBuilder5 : AbsScreenMeshBuilder
//    {
//        public const int MaxPart = 2;
       
//        static Graphics.PolygonColor[, , ,] preparedPolygons_PosXYZ_FaceType; //400 000st
//#if WHITE_EDGES
//        static readonly PreparedFaceCorners StartSSAO = PreparedFaceCorners.Two;
//#else
//        static readonly PreparedFaceCorners StartSSAO = PreparedFaceCorners.Zero;
//#endif
//        public ScreenMeshBuilder5()
//        {
//            //init
//            preparedPolygons_PosXYZ_FaceType = new Graphics.PolygonColor[
//                WorldPosition.ChunkWith, WorldPosition.ChunkHeight, WorldPosition.ChunkWith,
//                (int)CubeFace.NUM];
//            IntVector3 pos = IntVector3.Zero;
//            for (pos.X = 0; pos.X < WorldPosition.ChunkWith; pos.X++)
//            {
//                for (pos.Z = 0; pos.Z < WorldPosition.ChunkWith; pos.Z++)
//                {
//                    for (pos.Y = 0; pos.Y < WorldPosition.ChunkHeight; pos.Y++)
//                    {
//                        for (int faceIx = 0; faceIx < (int)CubeFace.NUM; faceIx++)
//                        {
//                            preparedPolygons_PosXYZ_FaceType[pos.X, pos.Y, pos.Z, faceIx] = 
//                                new Graphics.PolygonColor(
//                                    faceIx,
//                                    0,
//                                    1,1,1,1,
//                                    pos);
//                        }
//                    }
//                }
//            }

//            LootFest.Data.Block.FreeMemory();
//        }

        
//        PreparedFaceCorners ssao = new PreparedFaceCorners();
//        Graphics.PolygonColor polygon;

//        override public Graphics.VertexAndIndexBuffer BuildScreen(ref byte[, ,] dataGrid,
//            WorldPosition wp,
//            Graphics.LFHeightMap heightMap,
//            List<Graphics.PolygonColor> polygons, int part)
//        {
//            IntVector3 gridPos = IntVector3.Zero;
//            IntVector3 preChunkPos = IntVector3.Zero;
//            Voxels.FaceCornerColorYS.Init();
//            //Go through the whole data grid of the screen
//            //Fill the prepared chunk
//            if (part == 0)
//            {
//                for (gridPos.Y = 0; gridPos.Y < WorldPosition.ChunkHeight; gridPos.Y++)
//                {
//                    preChunkPos.Y = gridPos.Y + 1;
//                    for (gridPos.Z = 0; gridPos.Z < WorldPosition.ChunkWith; gridPos.Z++)
//                    {
//                        preChunkPos.Z = gridPos.Z + 1;
//                        for (gridPos.X = 0; gridPos.X < WorldPosition.ChunkWith; gridPos.X++)
//                        {
//                            preChunkPos.X = gridPos.X + 1;

//                            preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z] =
//                                dataGrid[gridPos.X, gridPos.Y, gridPos.Z];// == Empty ? Empty : Filled;
//                        }
//                    }
//                }
//                //Fill the prepared chunk with its sourrounding blocks
//                for (Facing8Dir dir = (Facing8Dir)0; dir < Facing8Dir.NUM; dir++)
//                {
//                    WorldPosition nScreen = wp;
//                    nScreen.ScreenIndex.Add(IntVector2.FromFacing8Dir(dir));
//                    if (nScreen.CorrectScreenPos)
//                    {
//                        LfRef.chunks.GetScreen(nScreen).AddToPreparedChunk(dir, ref preparedChunk);
//                    }

//                }
//            }
//            else
//            {

//                //The chunk is now prepared
//                //Go through the prepared chunk and generate the mesh
//                byte material;
//                int start;
//                int end;
//                const int MidHeight = WorldPosition.ChunkHeight / PublicConstants.TWICE;

//                if (part == 1)
//                {
//                    start = 0;
//                    end = MidHeight;
//                }
//                else
//                {
//                    start = MidHeight;
//                    end = WorldPosition.ChunkHeight;
//                }

//                for (gridPos.Y = start; gridPos.Y < end; gridPos.Y++)
//                {
//                    preChunkPos.Y = gridPos.Y + 1;
//                    for (gridPos.Z = 0; gridPos.Z < WorldPosition.ChunkWith; gridPos.Z++)
//                    {
//                        preChunkPos.Z = gridPos.Z + 1;
//                        for (gridPos.X = 0; gridPos.X < WorldPosition.ChunkWith; gridPos.X++)
//                        {
//                            material = dataGrid[gridPos.X, gridPos.Y, gridPos.Z];

//                            if (material != 0)
//                            {
//                                preChunkPos.X = gridPos.X + 1;
//                                //Go through all faces

//                                //TOP
//                                if (preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z] == 0)
//                                {
//                                    #region CalcSSAO

//                                    ssao = PreparedFaceCorners.Two;
//                                    if (preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z - 1] != 0)
//                                    {//north, above2
//                                        ssao.Corner0++;
//                                        ssao.Corner1++;
//                                    }
////#if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z - 1] == 0)
//                                    {//north //White edges, below1
//                                        ssao.Corner0--;
//                                        ssao.Corner1--;
//                                    }
////#endif
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z] != 0)
//                                    {//west, above4

//                                        ssao.Corner0++;
//                                        ssao.Corner2++;
//                                    }
//// #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y, preChunkPos.Z] == 0)
//                                    {//west //White edges, below2

//                                        ssao.Corner0--;
//                                        ssao.Corner2--;
//                                    }
////#endif
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z] != 0)
//                                    {//east, above5
//                                        ssao.Corner1++;
//                                        ssao.Corner3++;
//                                    }
////  #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y, preChunkPos.Z] == 0)
//                                    {//east //White edges, below3
//                                        ssao.Corner1--;
//                                        ssao.Corner3--;
//                                    }
////#endif
//                                    if (preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z + 1] != 0)
//                                    {//south, above7
//                                        ssao.Corner2++;
//                                        ssao.Corner3++;
//                                    }
////#if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z + 1] == 0)
//                                    {//south //White edges, below4
//                                        ssao.Corner2--;
//                                        ssao.Corner3--;
//                                    }
//// #endif
//                                    ////CORNERS
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z - 1] != 0)
//                                    {//north, above1
//                                        ssao.Corner0++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z - 1] != 0)
//                                    {//north, above3
//                                        ssao.Corner1++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z + 1] != 0)
//                                    {//SW, above6
//                                        ssao.Corner2++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z + 1] != 0)
//                                    {//SE, above8
//                                        ssao.Corner3++;
//                                    }
//                                    #endregion

//                                    polygon = preparedPolygons_PosXYZ_FaceType[gridPos.X, gridPos.Y, gridPos.Z, FaceTypeTop];
//                                    polygon.SetTileAndCorners(
//                                        FaceTypeTop,
//                                        Data.MaterialBuilder.Materials[material].TopTiles.GetRandomTile(gridPos),
//                                        ssao.Corner0, ssao.Corner1, ssao.Corner2, ssao.Corner3);
//                                    polygons.Add(polygon);

//                                }

//                                //BOTTOM
//                                if (preparedChunk[preChunkPos.X, preChunkPos.Y - 1, preChunkPos.Z] == 0)
//                                {
//                                    polygon = preparedPolygons_PosXYZ_FaceType[gridPos.X, gridPos.Y, gridPos.Z, FaceTypeBottom];
//                                    polygon.SetTileAndCorners(
//                                        FaceTypeBottom,
//                                        Data.MaterialBuilder.Materials[material].TopTiles.GetRandomTile(gridPos),
//                                        StartSSAO.Corner0, StartSSAO.Corner1, StartSSAO.Corner2, StartSSAO.Corner3);
//                                    polygons.Add(polygon);
//                                }

//                                //FRONT
//                                if (preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z + 1] == 0)
//                                {
//                                    #region CalcSSAO

//                                    ssao = StartSSAO;
//                                    if (preparedChunk[preChunkPos.X, preChunkPos.Y - 1, preChunkPos.Z + 1] != 0)
//                                    {//north, above2
//                                        ssao.Corner0++;
//                                        ssao.Corner1++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X, preChunkPos.Y - 1, preChunkPos.Z] == 0)
//                                    {//north //White edges, below1
//                                        ssao.Corner0--;
//                                        ssao.Corner1--;
//                                    }
//                                    #endif
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y, preChunkPos.Z + 1] != 0)
//                                    {//west, above4

//                                        ssao.Corner0++;
//                                        ssao.Corner2++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y, preChunkPos.Z] == 0)
//                                    {//west //White edges, below2

//                                        ssao.Corner0--;
//                                        ssao.Corner2--;
//                                    }
//                                    #endif
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y, preChunkPos.Z + 1] != 0)
//                                    {//east, above5
//                                        ssao.Corner1++;
//                                        ssao.Corner3++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y, preChunkPos.Z] == 0)
//                                    {//east //White edges, below3
//                                        ssao.Corner1--;
//                                        ssao.Corner3--;
//                                    }
//                                    #endif
//                                    if (preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z + 1] != 0)
//                                    {//south, above7
//                                        ssao.Corner2++;
//                                        ssao.Corner3++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z] == 0)
//                                    {//south //White edges, below4
//                                        ssao.Corner2--;
//                                        ssao.Corner3--;
//                                    }
//                                    #endif
//                                    ////CORNERS
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y - 1, preChunkPos.Z + 1] != 0)
//                                    {//north, above1
//                                        ssao.Corner0++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y - 1, preChunkPos.Z + 1] != 0)
//                                    {//north, above3
//                                        ssao.Corner1++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z + 1] != 0)
//                                    {//SW, above6
//                                        ssao.Corner2++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z + 1] != 0)
//                                    {//SE, above8
//                                        ssao.Corner3++;
//                                    }
//                                    #endregion

//                                    polygon = preparedPolygons_PosXYZ_FaceType[gridPos.X, gridPos.Y, gridPos.Z, FaceTypeFront];
//                                    polygon.SetTileAndCorners(
//                                        FaceTypeFront,
//                                        Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos),
//                                        ssao.Corner3, ssao.Corner2, ssao.Corner1, ssao.Corner0);
//                                    polygons.Add(polygon);
//                                }
//                                //BACK
//                                if (preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z - 1] == 0)
//                                {

//                                    #region CalcSSAO

//                                    ssao = StartSSAO;
//                                    if (preparedChunk[preChunkPos.X, preChunkPos.Y - 1, preChunkPos.Z - 1] != 0)
//                                    {//north, above2
//                                        ssao.Corner0++;
//                                        ssao.Corner1++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X, preChunkPos.Y - 1, preChunkPos.Z] == 0)
//                                    {//north //White edges, below1
//                                        ssao.Corner0--;
//                                        ssao.Corner1--;
//                                    }
//                                    #endif
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y, preChunkPos.Z - 1] != 0)
//                                    {//west, above4

//                                        ssao.Corner0++;
//                                        ssao.Corner2++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y, preChunkPos.Z] == 0)
//                                    {//west //White edges, below2

//                                        ssao.Corner0--;
//                                        ssao.Corner2--;
//                                    }
//                                    #endif
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y, preChunkPos.Z - 1] != 0)
//                                    {//east, above5
//                                        ssao.Corner1++;
//                                        ssao.Corner3++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y, preChunkPos.Z] == 0)
//                                    {//east //White edges, below3
//                                        ssao.Corner1--;
//                                        ssao.Corner3--;
//                                    }
//                                    #endif
//                                    if (preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z - 1] != 0)
//                                    {//south, above7
//                                        ssao.Corner2++;
//                                        ssao.Corner3++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z] == 0)
//                                    {//south //White edges, below4
//                                        ssao.Corner2--;
//                                        ssao.Corner3--;
//                                    }
//                                    #endif
//                                    ////CORNERS
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y - 1, preChunkPos.Z - 1] != 0)
//                                    {//north, above1
//                                        ssao.Corner0++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y - 1, preChunkPos.Z - 1] != 0)
//                                    {//north, above3
//                                        ssao.Corner1++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z - 1] != 0)
//                                    {//SW, above6
//                                        ssao.Corner2++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z - 1] != 0)
//                                    {//SE, above8
//                                        ssao.Corner3++;
//                                    }
//                                    #endregion
//                                    polygon = preparedPolygons_PosXYZ_FaceType[gridPos.X, gridPos.Y, gridPos.Z, FaceTypeBack];
//                                    polygon.SetTileAndCorners(
//                                        FaceTypeBack,
//                                        Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos),
//                                        ssao.Corner3, ssao.Corner2, ssao.Corner1, ssao.Corner0);
//                                    polygons.Add(polygon);
//                                }
//                                //LEFT
//                                 if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y, preChunkPos.Z] == 0)
//                                {
//                                    #region CalcSSAO

//#if WHITE_EDGES
//                                    ssao = StartSSAO;
//#else
//                                    ssao = PreparedFaceCorners.Zero;
//#endif
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y - 1, preChunkPos.Z] != 0)
//                                    {//north, above2
//                                        ssao.Corner0++;
//                                        ssao.Corner1++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X, preChunkPos.Y - 1, preChunkPos.Z] == 0)
//                                    {//north //White edges, below1
//                                        ssao.Corner0--;
//                                        ssao.Corner1--;
//                                    }
//                                    #endif
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y, preChunkPos.Z - 1] != 0)
//                                    {//west, above4

//                                        ssao.Corner0++;
//                                        ssao.Corner2++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z - 1] == 0)
//                                    {//west //White edges, below2

//                                        ssao.Corner0--;
//                                        ssao.Corner2--;
//                                    }
//                                    #endif
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y, preChunkPos.Z + 1] != 0)
//                                    {//east, above5
//                                        ssao.Corner1++;
//                                        ssao.Corner3++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z + 1] == 0)
//                                    {//east //White edges, below3
//                                        ssao.Corner1--;
//                                        ssao.Corner3--;
//                                    }
//                                    #endif
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z] != 0)
//                                    {//south, above7
//                                        ssao.Corner2++;
//                                        ssao.Corner3++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z] == 0)
//                                    {//south //White edges, below4
//                                        ssao.Corner2--;
//                                        ssao.Corner3--;
//                                    }
//                                    #endif
//                                    ////CORNERS
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y - 1, preChunkPos.Z - 1] != 0)
//                                    {//north, above1
//                                        ssao.Corner0++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y - 1, preChunkPos.Z + 1] != 0)
//                                    {//north, above3
//                                        ssao.Corner1++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z - 1] != 0)
//                                    {//SW, above6
//                                        ssao.Corner2++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X + 1, preChunkPos.Y + 1, preChunkPos.Z + 1] != 0)
//                                    {//SE, above8
//                                        ssao.Corner3++;
//                                    }
//                                    #endregion
//                                    polygon = preparedPolygons_PosXYZ_FaceType[gridPos.X, gridPos.Y, gridPos.Z, FaceTypeLeft];
//                                    polygon.SetTileAndCorners(
//                                        FaceTypeLeft,
//                                        Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos),
//                                        ssao.Corner3, ssao.Corner2, ssao.Corner1, ssao.Corner0);
//                                    polygons.Add(polygon);
//                                }
//                                //RIGHT
//                                if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y, preChunkPos.Z] == 0)
//                                {
//                                    #region CalcSSAO

//                                    ssao = StartSSAO;
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y - 1, preChunkPos.Z] != 0)
//                                    {//north, above2
//                                        ssao.Corner0++;
//                                        ssao.Corner1++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X, preChunkPos.Y - 1, preChunkPos.Z] == 0)
//                                    {//north //White edges, below1
//                                        ssao.Corner0--;
//                                        ssao.Corner1--;
//                                    }
//                                    #endif
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y, preChunkPos.Z + 1] != 0)
//                                    {//west, above4

//                                        ssao.Corner0++;
//                                        ssao.Corner2++;
//                                    }
//#if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z + 1] == 0)
//                                    {//west //White edges, below2

//                                        ssao.Corner0--;
//                                        ssao.Corner2--;
//                                    }
//#endif
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y, preChunkPos.Z - 1] != 0)
//                                    {//east, above5
//                                        ssao.Corner1++;
//                                        ssao.Corner3++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z - 1] == 0)
//                                    {//east //White edges, below3
//                                        ssao.Corner1--;
//                                        ssao.Corner3--;
//                                    }
//#endif
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z] != 0)
//                                    {//south, above7
//                                        ssao.Corner2++;
//                                        ssao.Corner3++;
//                                    }
//                                        #if WHITE_EDGES
//                                    else if (preparedChunk[preChunkPos.X, preChunkPos.Y + 1, preChunkPos.Z] == 0)
//                                    {//south //White edges, below4
//                                        ssao.Corner2--;
//                                        ssao.Corner3--;
//                                    }
//                                    #endif
//                                    ////CORNERS
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y - 1, preChunkPos.Z + 1] != 0)
//                                    {//north, above1
//                                        ssao.Corner0++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y - 1, preChunkPos.Z - 1] != 0)
//                                    {//north, above3
//                                        ssao.Corner1++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z + 1] != 0)
//                                    {//SW, above6
//                                        ssao.Corner2++;
//                                    }
//                                    if (preparedChunk[preChunkPos.X - 1, preChunkPos.Y + 1, preChunkPos.Z - 1] != 0)
//                                    {//SE, above8 
//                                        ssao.Corner3++;
//                                    }
//                                    #endregion

//                                    polygon = preparedPolygons_PosXYZ_FaceType[gridPos.X, gridPos.Y, gridPos.Z, FaceTypeRight];
//                                    polygon.SetTileAndCorners(
//                                        FaceTypeRight,
//                                        Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos),
//                                        ssao.Corner3, ssao.Corner2, ssao.Corner1, ssao.Corner0);
//                                    polygons.Add(polygon);
//                                }     
//                            }
//                        }
//                    }

//                }

//                if (part == MaxPart)
//                {
//                    wp.BlockPos = IntVector3.Zero;
//                    //wp.UpdateWorldGridPos();
//                    return heightMap.BuildFromPolygons(new Graphics.PolygonsAndTrianglesColor(polygons,
//                        new List<Graphics.TriangleColor>()), LoadedTexture.NO_TEXTURE, wp.WorldGridPos);
//                }
//            }
//            return null;

//        }
    
//    }
//}
