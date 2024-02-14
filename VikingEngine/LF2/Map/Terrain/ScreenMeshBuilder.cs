using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using VikingEngine.Voxels;

namespace VikingEngine.LF2.Map.Terrain
{
    abstract class AbsScreenMeshBuilder
    {
       // protected static PreparedFaceCorners[, , ,] faceCorners; 

        //protected static Graphics.PolygonColor[,,,,,,,,,,,,] preCalculatedFaces_Face_Above1to8_Side1to4; 
        const int AddCheckWidth = 2;
        protected static byte[, ,] preparedChunk = new byte[
            WorldPosition.ChunkWidth + AddCheckWidth, 
            WorldPosition.ChunkHeight + AddCheckWidth, 
            WorldPosition.ChunkWidth + AddCheckWidth];

        public static void Init()
        {
            //NEW
            //prepare all situations a block can be sourrounded in
            //look at each face and calc all 512 variations it can be in
            //const int NumBlockSitutations = 2;
            //preCalculatedFaces_Face_Above1to8_Side1to4 =
            //    new Graphics.PolygonColor[(int)CubeFace.NUM,
            //        NumBlockSitutations, NumBlockSitutations, NumBlockSitutations, NumBlockSitutations, NumBlockSitutations, NumBlockSitutations, NumBlockSitutations, NumBlockSitutations,
            //        NumBlockSitutations, NumBlockSitutations, NumBlockSitutations, NumBlockSitutations];

            //for (int face = 0; face < (int)CubeFace.NUM; face++)
            //{
                
            //    for (int above1 = 0; above1 < NumBlockSitutations; above1++)
            //    {
            //        for (int above2 = 0; above2 < NumBlockSitutations; above2++)
            //        {
            //            for (int above3 = 0; above3 < NumBlockSitutations; above3++)
            //            {
            //                for (int above4 = 0; above4 < NumBlockSitutations; above4++)
            //                {
            //                    for (int above5 = 0; above5 < NumBlockSitutations; above5++)
            //                    {
            //                        for (int above6 = 0; above6 < NumBlockSitutations; above6++)
            //                        {
            //                            for (int above7 = 0; above7 < NumBlockSitutations; above7++)
            //                            {
            //                                for (int above8 = 0; above8 < NumBlockSitutations; above8++)
            //                                {

            //for (int side1 = 0; side1 < NumBlockSitutations; side1++)
            //{
            //    for (int side2 = 0; side2 < NumBlockSitutations; side2++)
            //    {
            //        for (int side3 = 0; side3 < NumBlockSitutations; side3++)
            //        {
            //            for (int side4 = 0; side4 < NumBlockSitutations; side4++)
            //            {
            //                PreparedFaceCorners ssao = new PreparedFaceCorners();
            //                if (above2 == 1)
            //                {//north
            //                    ssao.Corner0++;
            //                    ssao.Corner1++;
            //                }
            //                if (above4 == 1)
            //                {//west
            //                    ssao.Corner3++;
            //                    ssao.Corner0++;
            //                }
            //                if (above5 == 1)
            //                {//east
            //                    ssao.Corner1++;
            //                    ssao.Corner2++;
            //                }
            //                if (above7 == 1)
            //                {//south
            //                    ssao.Corner1++;
            //                    ssao.Corner2++;
            //                }

            //                //CORNERS
            //                if (above1 == 1)
            //                {//north
            //                    ssao.Corner0++;
            //                }
            //                if (above3 == 1)
            //                {//north
            //                    ssao.Corner1++;
            //                }
            //                if (above6 == 1)
            //                {//north
            //                    ssao.Corner2++;
            //                }
            //                if (above8 == 1)
            //                {//north
            //                    ssao.Corner3++;
            //                }

            //                Graphics.PolygonColor polygon = new Graphics.PolygonColor(
            //                    new FaceCornerColorYS(TopSideBrightness, false, ssao.C0, ssao.C1, ssao.C2, ssao.C3));

            //                preCalculatedFaces_Face_Above1to8_Side1to4[face, above1, above2, above3, above4, above5, above6, above7, above8,
            //                    side1, side2, side3, side4] = polygon;
            //            }
            //        }
            //    }
            //}
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
                
            //}


                //OLD
               // faceCorners = new PreparedFaceCorners[2, 2, 2, 2];
            for (byte side4 = 0; side4< 2; side4++)
            {
                for (byte side3 = 0; side3 < 2; side3++)
                {
                    for (byte side2 = 0; side2 < 2; side2++)
                    {
                        for (byte side1 = 0; side1 < 2; side1++)
                        {
                            PreparedFaceCorners result = new PreparedFaceCorners();
                            
                            result.Corner2+= side1;
                            result.Corner3+= side1;
                            
                            
                            result.Corner3+= side2;
                            result.Corner1+= side2;
                            
                            result.Corner0+= side3;
                            result.Corner1+= side3;
                           
                            result.Corner2+= side4;
                            result.Corner0+= side4;
                            
                           // faceCorners[side1, side2, side3, side4] = result;
                        }
                    }
                }
            }
        }

        abstract public Graphics.VertexAndIndexBuffer BuildScreen(ref byte[,,] dataGrid,
            WorldPosition wp,
            Graphics.LFHeightMap heightMap,
            List<Graphics.PolygonColor> polygons, int part);

        protected static readonly List<bool> MoveShadowSearchSideWays = new List<bool> { false, true, false, true, false, true, false, true, false, true, false, true };
        protected const float ShadowSideBrightness = 0.5f;
        protected const float LightSideBrightness = 0.85f;
        protected const float TopSideBrightness = 1f;
        protected static readonly FaceCornerColorYS FaceCornersTop = new FaceCornerColorYS(TopSideBrightness, false);
        protected static readonly FaceCornerColorYS FaceCornersFront = new FaceCornerColorYS(LightSideBrightness, false);
        protected static readonly FaceCornerColorYS FaceCornersLeft = new FaceCornerColorYS(ShadowSideBrightness, true);
        protected static readonly FaceCornerColorYS FaceCornersRight = new FaceCornerColorYS(LightSideBrightness, false);
        protected static readonly FaceCornerColorYS FaceCornersBack = new FaceCornerColorYS(ShadowSideBrightness, false);
        protected static readonly int FaceTypeTop = (int)CubeFace.Ypositive;
        protected static readonly int FaceTypeBottom = (int)CubeFace.Ynegative;
        protected static readonly int FaceTypeFront = (int)CubeFace.Zpositive;
        protected static readonly int FaceTypeBack = (int)CubeFace.Znegative;
        protected static readonly int FaceTypeLeft = (int)CubeFace.Xpositive;
        protected static readonly int FaceTypeRight = (int)CubeFace.Xnegative;


        protected static IPreparedFaceCorners runCornerCheck(WorldPosition wp, CubeFace face)
        {
            WorldPosition c0 = wp;
            WorldPosition c1 = wp;
            WorldPosition c2 = wp;
            WorldPosition c3 = wp;

            switch (face)
            {
                case CubeFace.Zpositive:
                    c0.WorldGrindex.Y++;
                    c1.WorldGrindex.X--;
                    c2.WorldGrindex.Y--;
                    c3.WorldGrindex.X++;
                    break;
                case CubeFace.Xpositive:
                    c0.WorldGrindex.Y++;
                    c1.WorldGrindex.Z++;
                    c2.WorldGrindex.Y--;
                    c3.WorldGrindex.Z--;
                    break;
                case CubeFace.Xnegative:
                    c0.WorldGrindex.Y++;
                    c1.WorldGrindex.Z--;
                    c2.WorldGrindex.Y--;
                    c3.WorldGrindex.Z++;

                    break;
                case CubeFace.Ypositive:
                    c0.WorldGrindex.Z++;
                    c1.WorldGrindex.X++;
                    c2.WorldGrindex.Z--;
                    c3.WorldGrindex.X--;

                    break;
                
            }
            return null;
           //return faceCorners[
           //     cornerCheck2(ref c0),
           //     cornerCheck2(ref c1),
           //     cornerCheck2(ref c2),
           //     cornerCheck2(ref c3)];
                
        }
        protected static float SearchDropShadow(WorldPosition wp)
        {
            ++wp.WorldGrindex.X;
            ++wp.WorldGrindex.Y;
            ++wp.WorldGrindex.Z;

            foreach (bool sideWaysMove in MoveShadowSearchSideWays)
            {
                if (!wp.CorrectPos)
                { return 1; }
                //if (WorldPosition.ChunkHeight <= wp.BlockPos.Y)
                //{ return 1; }
                if (LfRef.chunks.Get(wp) != WorldPosition.EmptyBlock)
                {
                    return 0.7f;
                }
                else
                {
                    wp.WorldGrindex.Y+=1;
                    if (sideWaysMove)
                    {
                        wp.WorldGrindex.X+=1;
                        wp.WorldGrindex.Z+=1;
                    }
                }
            }
            return 1;
        }
        protected static IPreparedFaceCorners runCornerCheck2(WorldPosition wp, CubeFace face)
        {//bara top front left, än så länge
            //PreparedFaceCorners result = new PreparedFaceCorners();
            WorldPosition c0 = wp;
            WorldPosition c1 = wp;
            WorldPosition c2 = wp;
            WorldPosition c3 = wp;

            WorldPosition c4 = WorldPosition.EmptyPos;
            WorldPosition c5 = WorldPosition.EmptyPos;
            WorldPosition c6 = WorldPosition.EmptyPos;
            WorldPosition c7 = WorldPosition.EmptyPos;

            switch (face)
            {
                case CubeFace.Ypositive:
                    c0.WorldGrindex.Z++;
                    c1.WorldGrindex.X++;
                    c2.WorldGrindex.Z--;
                    c3.WorldGrindex.X--;

                    c4 = c0;
                    c4.WorldGrindex.X--;
                    c5 = c0;
                    c5.WorldGrindex.X++;
                    c6 = c2;
                    c6.WorldGrindex.X--;
                    c7 = c2;
                    c7.WorldGrindex.X++;
                    break;
                case CubeFace.Zpositive:
                    c0.WorldGrindex.Y++;
                    c1.WorldGrindex.X--;
                    c2.WorldGrindex.Y--;
                    c3.WorldGrindex.X++;
                    break;
                case CubeFace.Xpositive:
                    c0.WorldGrindex.Y++;
                    c1.WorldGrindex.Z++;
                    c2.WorldGrindex.Y--;
                    c3.WorldGrindex.Z--;
                    break;
                case CubeFace.Xnegative:
                    c0.WorldGrindex.Y++;
                    c1.WorldGrindex.Z--;
                    c2.WorldGrindex.Y--;
                    c3.WorldGrindex.Z++;

                    break;
            }

            PreparedFaceCorners result = new PreparedFaceCorners();
            //faceCorners[
            //    cornerCheck2(ref c0), cornerCheck2(ref c1), cornerCheck2(ref c2), cornerCheck2(ref c3)];
            
            if (cornerCheck(ref c4))
            {
                result.Corner2++;
            }
            if (cornerCheck(ref c5))
            {
                result.Corner3++;
            }
            if (cornerCheck(ref c6))
            {
                result.Corner0++;
            }
            if (cornerCheck(ref c7))
            {
                result.Corner1++;
            }
            return result;
        }
       
       
        protected static bool cornerCheck(ref WorldPosition wp)
        {
            if (LfRef.chunks.Get(wp) == WorldPosition.EmptyBlock)
            {
                return false;
            }
            //numCorners++;
            return true;
        }
        protected static byte cornerCheck2(ref WorldPosition wp)
        {
            if (LfRef.chunks.Get(wp) == WorldPosition.EmptyBlock)
            {
                return 0;
            }
            return 1;
        }
    }


    

    
    /*
     * SKulle kunna skippa hela checken genom att istället stega igenom varje block som ett scenario
     * förbered då en array på 3*3*3 block som man plockar en förbered situation ifrån
     */

    

    //class ScreenMeshBuilderHeavy : AbsScreenMeshBuilder
    //{
    //    override public Graphics.VertexAndIndexBuffer BuildScreen(ref byte[,,] dataGrid,
    //        WorldPosition wp,
    //        Graphics.LFHeightMap heightMap,
    //        List<Graphics.PolygonColor> polygons, int part)
    //    {
    //        //Go through the whole data grid of the screen
    //        //If there is a block, it will check if it is a surface block
    //        //List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>();

    //        wp.Y = part;
    //        //wp.UpdateGridPosY();
    //        for (wp.LocalBlockZ = 0; wp.LocalBlockZ < WorldPosition.ChunkWidth; wp.LocalBlockZ++)
    //        {
    //            //wp.UpdateGridPosZ();
    //            for (wp.LocalBlockX = 0; wp.LocalBlockX < WorldPosition.ChunkWidth; wp.LocalBlockX++)
    //            {

    //                byte material = dataGrid[wp.LocalBlockX,wp.WorldGrindex.Y, wp.LocalBlockZ];
    //                if (material != WorldPosition.EmptyBlock)
    //                {
    //                    //Check which faces the block includes
    //                    //public const int FaceTopDir = 1;
    //                    //public const int FaceFrontDir = 1;
    //                    //public const int FaceRightDir = -1;

    //                    //TOP
    //                    WorldPosition neighbor = wp;
    //                    neighbor.WorldGrindex.Y++;
    //                    if (LfRef.chunks.Get(neighbor) == WorldPosition.EmptyBlock)
    //                    {
    //                        //doneDropShadowSearch = true;
    //                        //dropShadowAdd = SearchDropShadow(wp);
    //                        IPreparedFaceCorners ssao = runCornerCheck2(neighbor, CubeFace.Top);
    //                        polygons.Add(new Graphics.PolygonColor(
    //                            FaceTypeTop,
    //                            Data.MaterialBuilder.Materials[material].TopTiles.GetRandomTile(wp.LocalBlockGrindex),
    //                            //TopSideBrightness * dropShadowAdd
    //                            new FaceCornerColorYS(TopSideBrightness, false, ssao.C0, ssao.C1, ssao.C2, ssao.C3),
    //                            wp.LocalBlockGrindex));
    //                    }
    //                    //LEFT
    //                    neighbor = wp;
    //                    neighbor.WorldGrindex.X++;
    //                    if (LfRef.chunks.Get(neighbor) == WorldPosition.EmptyBlock)
    //                    {
    //                        IPreparedFaceCorners ssao = runCornerCheck(neighbor, CubeFace.Left);
    //                        polygons.Add(new Graphics.PolygonColor(
    //                            FaceTypeLeft,
    //                            Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(wp.LocalBlockGrindex),
    //                            new FaceCornerColorYS(ShadowSideBrightness, true, ssao.C0, ssao.C1, ssao.C2, ssao.C3),
    //                            wp.LocalBlockGrindex));
    //                    }
    //                    //BACK
    //                    neighbor = wp;
    //                    neighbor.WorldGrindex.Z--;
    //                    if (LfRef.chunks.Get(neighbor) == WorldPosition.EmptyBlock)
    //                    {
    //                        polygons.Add(new Graphics.PolygonColor(
    //                            FaceTypeBack,
    //                            Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(wp.LocalBlockGrindex),
    //                            FaceCornersBack,
    //                            wp.LocalBlockGrindex));
    //                    }
    //                    //FRONT
    //                    neighbor = wp;
    //                    neighbor.WorldGrindex.Z++;
    //                    if (LfRef.chunks.Get(neighbor) == WorldPosition.EmptyBlock)
    //                    {
    //                        IPreparedFaceCorners ssao = runCornerCheck(neighbor, CubeFace.Front);
    //                        polygons.Add(new Graphics.PolygonColor(
    //                            FaceTypeFront,
    //                            Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(wp.LocalBlockGrindex),
    //                            new FaceCornerColorYS(LightSideBrightness, false, ssao.C0, ssao.C1, ssao.C2, ssao.C3),
    //                            wp.LocalBlockGrindex));
    //                    }

    //                    //RIGHT
    //                    neighbor = wp;
    //                    neighbor.WorldGrindex.X--;
    //                    if (LfRef.chunks.Get(neighbor) == WorldPosition.EmptyBlock)
    //                    {
    //                        IPreparedFaceCorners ssao = runCornerCheck(neighbor, CubeFace.Right);
    //                        polygons.Add(new Graphics.PolygonColor(
    //                            FaceTypeRight,
    //                            Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(wp.LocalBlockGrindex),
    //                            new FaceCornerColorYS(LightSideBrightness, false, ssao.C0, ssao.C1, ssao.C2, ssao.C3),
    //                            wp.LocalBlockGrindex));
    //                    }

    //                }
    //            }
    //        }

    //        if (part == WorldPosition.ChunkHeight - 1)
    //        {
    //            wp.LocalBlockGrindex = IntVector3.Zero;
    //            //wp.UpdateWorldGridPos();
    //            //return heightMap.BuildFromPolygons(new Graphics.PolygonsAndTrianglesColor(polygons,
    //            //    new List<Graphics.TriangleColor>()), LoadedTexture.NO_TEXTURE, wp.WorldGridPos);
    //            return null;
    //        }

    //        return null;
    //    }
    //}
    //    class ScreenMeshBuilderLight : AbsScreenMeshBuilder
    //    {
    //        override public Graphics.VertexAndIndexBuffer BuildScreen(ref byte[,,] dataGrid,
    //            WorldPosition wp,
    //            Graphics.LFHeightMap heightMap,
    //            List<Graphics.PolygonColor> polygons, int part)
    //        {
    //            //Go through the whole data grid of the screen
    //            //If there is a block, it will check if it is a surface block
    //            //List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>();

    //            wp.Y = part;
    //            //wp.UpdateGridPosY();
    //            for (wp.LocalBlockZ = 0; wp.LocalBlockZ < WorldPosition.ChunkWidth; wp.LocalBlockZ++)
    //            {
    //                //wp.UpdateGridPosZ();
    //                for (wp.LocalBlockX = 0; wp.LocalBlockX < WorldPosition.ChunkWidth; wp.LocalBlockX++)
    //                {

    //                    byte material = dataGrid[wp.LocalBlockX,wp.WorldGrindex.Y, wp.LocalBlockZ];
    //                    if (material != WorldPosition.EmptyBlock)
    //                    {
    //                        //wp.UpdateGridPosX();
    //                        //Check which faces the block includes
    //                        //gör beräkningen direkt i if satsen
    //                        //ha en bool som vet om man har sökt efter skuggande föremål

    //                        //only top, left and back does the search, but it will spread onto the whole block
    //                        //bool doneDropShadowSearch = false;

    //                        //float dropShadowAdd = 0;

    //                        //TOP
    //                        WorldPosition neighbor = wp;
    //                        neighbor.WorldGrindex.Y++;
    //                        if (LfRef.chunks.Get(neighbor) == WorldPosition.EmptyBlock)
    //                        {
    //                            //
    //                            //IPreparedFaceCorners ssao = runCornerCheck2(neighbor, CubeFace.Top);
    //                            polygons.Add(new Graphics.PolygonColor(
    //                                FaceTypeTop,
    //                                Data.MaterialBuilder.Materials[material].TopTiles.GetRandomTile(wp.LocalBlockGrindex),
    //                                FaceCornersTop,
    //                                 wp.LocalBlockGrindex));
    //                        }
    //                        //LEFT
    //                        neighbor = wp;
    //                        neighbor.WorldGrindex.X++;
    //                        if (LfRef.chunks.Get(neighbor) == WorldPosition.EmptyBlock)
    //                        {
    //                            //IPreparedFaceCorners ssao = runCornerCheck(neighbor, CubeFace.Left);
    //                            polygons.Add(new Graphics.PolygonColor(
    //                                FaceTypeLeft,
    //                                Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(wp.LocalBlockGrindex),
    //                                FaceCornersLeft,
    //                                 wp.LocalBlockGrindex));
    //                        }
    //                        //BACK
    //                        //neighbor = wp;
    //                        //neighbor.WorldGrindex.Z+=-Data.Block.FaceFrontDir);
    //                        //if (LfRef.chunks.Get(neighbor) == WorldPosition.EmptyBlock)
    //                        //{
    //                        //    polygons.Add(new Graphics.PolygonColor(
    //                        //        CubeFace.Back,
    //                        //        Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(wp.BlockPos),
    //                        //        new FaceCornerColors(ShadowSideBrightness, false),
    //                        //        wp.GridPos));
    //                        //}
    //                        //FRONT
    //                        neighbor = wp;
    //                        neighbor.WorldGrindex.Z++;
    //                        if (LfRef.chunks.Get(neighbor) == WorldPosition.EmptyBlock)
    //                        {
    //                            //IPreparedFaceCorners ssao = runCornerCheck(neighbor, CubeFace.Front);
    //                            polygons.Add(new Graphics.PolygonColor(
    //                                FaceTypeFront,
    //                                Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(wp.LocalBlockGrindex),
    //                                FaceCornersFront,
    //                                 wp.LocalBlockGrindex));
    //                        }

    //                        //RIGHT
    //                        neighbor = wp;
    //                        neighbor.WorldGrindex.X--;
    //                        if (LfRef.chunks.Get(neighbor) == WorldPosition.EmptyBlock)
    //                        {
    //                            //IPreparedFaceCorners ssao = runCornerCheck(neighbor, CubeFace.Right);
    //                            polygons.Add(new Graphics.PolygonColor(
    //                                FaceTypeRight,
    //                                Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(wp.LocalBlockGrindex),
    //                                FaceCornersRight,
    //                                 wp.LocalBlockGrindex));
    //                        }

    //                    }
    //                }
    //            }

    //            if (part == WorldPosition.ChunkHeight - 1)
    //            {
    //                wp.LocalBlockGrindex = IntVector3.Zero;
    //                //wp.UpdateWorldGridPos();
    //                //return heightMap.BuildFromPolygons(new Graphics.PolygonsAndTrianglesColor(polygons,
    //                //    new List<Graphics.TriangleColor>()), LoadedTexture.NO_TEXTURE, wp.WorldGridPos);
    //            }

    //            return null;
    //        }
    //    }
    }
