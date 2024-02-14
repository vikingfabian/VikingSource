using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using VikingEngine.Voxels;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.LootFest.Editor
{
    static class VoxelObjBuilder
    {

        //public static Graphics.VoxelObj BuildFromVoxels(IntVector3 drawLimits, List<Voxel> voxels, Vector3 posAdjust)
        //{
        //    return BuildFromVoxels(new RangeIntV3(IntVector3.Zero, drawLimits), voxels, posAdjust);
        //}

        const int MinPolyCount = 1024;
        const int MinVertexCount = MinPolyCount * 4;
        const int MinDrawOrderLenght = MinPolyCount * 6;


        //public static Graphics.VoxelObj BuildFromVoxels(RangeIntV3 drawLimits, List<Voxel> voxels, Vector3 posAdjust)
        //{
        //    if (voxels.Count == 0)
        //    {
        //        throw new Exception("Empty voxel array");
        //    }
        //    Graphics.VoxelObj result = new Graphics.VoxelObj(false);
        //    List<Graphics.PolygonNormal> polygons = VoxelsToPolygons(voxels,
        //        drawLimits, posAdjust);
        //    if (polygons.Count == 0)
        //    { return null; }
        //    result.BuildFromPolygons(new Graphics.PolygonsAndTrianglesNormal(polygons,
        //        new List<Graphics.TriangleNormal>()), LfLib.BlockTexture);
        //    return result;
        //}

        //public static Graphics.VoxelModel BuildFromVoxelGrid2(VoxelObjGridData grid, Vector3 posAdjust)
        //{
        //    Graphics.VoxelModel result = new Graphics.VoxelModel(false);
            
        //    //new
        //    List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>(MinVertexCount);
        //    List<int> indexDrawOrder = new List<int>();
        //    VoxelGridToVertices(grid, posAdjust, ref vertices, ref indexDrawOrder);
        //    VerticeDataNormal verticeData = new VerticeDataNormal(vertices, indexDrawOrder);
        //    result.BuildFromVerticeData(verticeData, LfLib.BlockTexture);

        //    result.SetOneScale(grid.Limits);
        //    return result;
        //}

        public static Graphics.VoxelModel BuildAnimatedFromVoxelGrid2(List<VoxelObjGridData> grids, Vector3 posAdjust)
        {
            Graphics.VoxelModel result = new Graphics.VoxelModel(false);


            List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>(MinVertexCount * grids.Count);
            List<int> indexDrawOrder = new List<int>();
            List<Frame> framesData = new List<Frame>();
            foreach (VoxelObjGridData grid in grids)
            {
                Frame frameData = VoxelGridToVertices(grid, posAdjust, ref vertices, ref indexDrawOrder);
                framesData.Add(frameData);
            }
            if (vertices.Count == 0)
            { return null; }


            VerticeDataNormal verticeData = new VerticeDataNormal(vertices, indexDrawOrder);
            result.BuildFromVerticeData(verticeData, framesData, LfLib.BlockTexture);
            result.SetOneScale(grids[0].Size);
            return result;
        }

        public static Graphics.VoxelModel BuildModelHD(List<VoxelHD> voxels, IntVector3 size, Vector3 posAdjust)
        {
            return BuildModelHD(
                new List<VoxelObjGridDataHD>
                {
                    new VoxelObjGridDataHD(size, voxels)
                }, posAdjust);
        }

        public static Graphics.VoxelModel BuildModelHD(List<List<VoxelHD>> framesVoxels, IntVector3 size, Vector3 posAdjust)
        {
            List<VoxelObjGridDataHD> grids = new List<VoxelObjGridDataHD>(framesVoxels.Count);
            foreach (var m in framesVoxels)
            {
                grids.Add(new VoxelObjGridDataHD(size, m));
            }

            return BuildModelHD(grids, posAdjust);
        }


        public static Graphics.VoxelModel BuildModelHD(List<VoxelObjGridDataHD> grids, Vector3 posAdjust)
        {
            List<Frame> framesData;
            var verticeData = BuildVerticesHD(grids, posAdjust, out framesData);

            return BuildModelHD(verticeData, grids[0].Size, framesData);
        }

        public static IVerticeData BuildVerticesHD(List<VoxelObjGridDataHD> grids, Vector3 posAdjust, 
            out List<Frame> framesData)
        {
            List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>(MinVertexCount * grids.Count);
            List<int> indexDrawOrder = new List<int>(MinDrawOrderLenght);
            framesData = new List<Frame>(grids.Count);

            foreach (VoxelObjGridDataHD grid in grids)
            {
                Frame frameData = VoxelGridToVerticesHD(grid, posAdjust, ref vertices, ref indexDrawOrder);
                framesData.Add(frameData);
            }

            if (vertices.Count == 0)
            { return null; }

            VerticeDataColorTexture verticeData = new VerticeDataColorTexture(vertices, indexDrawOrder);
            return verticeData;
        }

        public static Graphics.VoxelModel BuildModelHD(IVerticeData verticeData, IntVector3 gridSz, List<Frame> framesData)
        {
            Graphics.VoxelModel result = new Graphics.VoxelModel(false);
            result.BuildFromVerticeData(verticeData, framesData, LfLib.BlockTexture);
            result.SetOneScale(gridSz);
            return result;
        }

            //public static Graphics.VoxelModel BuildModelHD(List<VoxelObjGridDataHD> grids, Vector3 posAdjust)
            //{

        //    List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>(MinVertexCount * grids.Count);
        //    List<int> indexDrawOrder = new List<int>(MinDrawOrderLenght);
        //    List<Frame> framesData = new List<Frame>(grids.Count);

        //    foreach (VoxelObjGridDataHD grid in grids)
        //    {
        //        Frame frameData = VoxelGridToVerticesHD(grid, posAdjust, ref vertices, ref indexDrawOrder);
        //        framesData.Add(frameData);
        //    }

        //    if (vertices.Count == 0)
        //    { return null; }

        //    VerticeDataColorTexture verticeData = new VerticeDataColorTexture(vertices, indexDrawOrder);

        //    Graphics.VoxelModel result = new Graphics.VoxelModel(false);
        //    result.BuildFromVerticeData(verticeData, framesData, LfLib.BlockTexture);
        //    result.SetOneScale(grids[0].Size);
        //    return result;
        //}



        public static Graphics.VoxelModel BuildAnimatedFromVoxels(IntervalIntV3 drawLimits, List<List<Voxel>> framesVoxels, Vector3 posAdjust)
        {
            Graphics.VoxelModel result = new Graphics.VoxelModel(false);
            List<Graphics.PolygonNormal> polygons = null;
            List<int> numPolys = new List<int>();
            foreach (List<Voxel> voxels in framesVoxels)
            {
                List<Graphics.PolygonNormal> addpolygons = VoxelsToPolygons(voxels,
                    drawLimits, posAdjust);
                numPolys.Add(addpolygons.Count);
                if (polygons == null)
                {
                    polygons = addpolygons;
                }
                else
                {
                    polygons.AddRange(addpolygons);
                }
                if (polygons.Count == 0)
                { return null; }

            }
            result.BuildFromPolygons(new Graphics.PolygonsAndTrianglesNormal(polygons,
                    new List<Graphics.TriangleNormal>()), numPolys, LfLib.BlockTexture);
            result.SetOneScale(drawLimits.Size);
            return result;
        }

        public static Frame VoxelGridToVertices(VoxelObjGridData materialGrid, Vector3 posAdjust,
            ref List<VertexPositionNormalTexture> vertices, ref List<int> indexDrawOrder)
        {
            byte material;
            IntVector3 size = materialGrid.Size;
            IntVector3 limits = materialGrid.Limits;
            IntVector3 gridPos = IntVector3.Zero;
            UInt16 totalVerticeIx = 0;
            int startVerticeIndex = vertices.Count;
            int tileIx;

            Vector3 position = Vector3.Zero;

            //bool drawFace;
            #region LOOP_TILES
            for (gridPos.Y = 0; gridPos.Y < size.Y; gridPos.Y++)
            {
                bool notCheckTop = gridPos.Y == 0;
                bool notCheckBottom = gridPos.Y == limits.Y;
                position.Y = gridPos.Y + posAdjust.Y;

                for (gridPos.Z = 0; gridPos.Z < size.Z; gridPos.Z++)
                {
                    bool notCheckFront = gridPos.Z == limits.Z;
                    bool notCheckBack = gridPos.Z == 0;
                    position.Z = gridPos.Z + posAdjust.Z;

                    for (gridPos.X = 0; gridPos.X < size.X; gridPos.X++)
                    {
                        material = materialGrid.MaterialGrid[gridPos.X, gridPos.Y, gridPos.Z];

                        if (material != 0)
                        {
                            bool notCheckLeft = gridPos.X == limits.X;
                            bool notCheckRight = gridPos.X == 0;
                            position.X = gridPos.X + posAdjust.X;

                            //BOTTOM
                            if (notCheckBottom || materialGrid.MaterialGrid[gridPos.X, gridPos.Y + 1, gridPos.Z] == 0)//FACE, set dir and limit
                            {
                                tileIx = Data.BlockTextures.Materials[material].TopTiles.GetRandomTile(gridPos);
                                Graphics.Sprite file = LootFest.LfRef.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(position,
                                    //FACE
                                    CubeFace.Ypositive);

                                indexDrawOrder.Add(vertices.Count);
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 3));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner3, data.Normal, file.SourcePolygonLowLeft));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner1, data.Normal, file.SourcePolygonTopLeft));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner4, data.Normal, file.SourcePolygonLowRight));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner2, data.Normal, file.SourcePolygonTopRight));

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }
                            //TOP
                            if (notCheckTop || materialGrid.MaterialGrid[gridPos.X, gridPos.Y - 1, gridPos.Z] == byte.MinValue)//FACE, set dir and limit
                            {
                                tileIx = Data.BlockTextures.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.Sprite file = LootFest.LfRef.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(position,
                                    //FACE
                                    CubeFace.Ynegative);
                               
                                indexDrawOrder.Add(vertices.Count);
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 3));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner3, data.Normal, file.SourcePolygonLowLeft));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner1, data.Normal, file.SourcePolygonTopLeft));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner4, data.Normal, file.SourcePolygonLowRight));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner2, data.Normal, file.SourcePolygonTopRight));

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }
                            //FRONT
                            if (notCheckFront || materialGrid.MaterialGrid[gridPos.X, gridPos.Y, gridPos.Z + 1] == byte.MinValue)
                            {
                               
                                tileIx = Data.BlockTextures.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.Sprite file = LootFest.LfRef.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(position,
                                    //FACE
                                    CubeFace.Zpositive);
                                
                                indexDrawOrder.Add(vertices.Count);
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 3));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner3, data.Normal, file.SourcePolygonLowLeft));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner1, data.Normal, file.SourcePolygonTopLeft));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner4, data.Normal, file.SourcePolygonLowRight));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner2, data.Normal, file.SourcePolygonTopRight));

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }
                            //BACK
                            if (notCheckBack || materialGrid.MaterialGrid[gridPos.X, gridPos.Y, gridPos.Z - 1] == byte.MinValue)
                            {
                               
                                tileIx = Data.BlockTextures.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.Sprite file = LootFest.LfRef.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(position,
                                    //FACE
                                    CubeFace.Znegative);
                                //static readonly int[] BasicIndexDrawOrder = new int[] { 0, 1, 2, 2, 1, 3 };
                                indexDrawOrder.Add(vertices.Count);
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 3));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner3, data.Normal, file.SourcePolygonLowLeft));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner1, data.Normal, file.SourcePolygonTopLeft));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner4, data.Normal, file.SourcePolygonLowRight));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner2, data.Normal, file.SourcePolygonTopRight));

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }
                            //RIGHT
                            if (notCheckRight || materialGrid.MaterialGrid[gridPos.X - 1, gridPos.Y, gridPos.Z] == byte.MinValue)
                            {
                               
                                tileIx = Data.BlockTextures.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.Sprite file = LootFest.LfRef.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(position,
                                    //FACE
                                    CubeFace.Xnegative);
                                //static readonly int[] BasicIndexDrawOrder = new int[] { 0, 1, 2, 2, 1, 3 };
                                indexDrawOrder.Add(vertices.Count);
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 3));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner3, data.Normal, file.SourcePolygonLowLeft));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner1, data.Normal, file.SourcePolygonTopLeft));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner4, data.Normal, file.SourcePolygonLowRight));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner2, data.Normal, file.SourcePolygonTopRight));

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }
                            //LEFT
                            if (notCheckLeft || materialGrid.MaterialGrid[gridPos.X + 1, gridPos.Y, gridPos.Z] == byte.MinValue)
                            {
                               
                                tileIx = Data.BlockTextures.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.Sprite file = LootFest.LfRef.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(position,
                                    //FACE
                                    CubeFace.Xpositive);
                                //static readonly int[] BasicIndexDrawOrder = new int[] { 0, 1, 2, 2, 1, 3 };
                                indexDrawOrder.Add(vertices.Count);
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 3));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner3, data.Normal, file.SourcePolygonLowLeft));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner1, data.Normal, file.SourcePolygonTopLeft));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner4, data.Normal, file.SourcePolygonLowRight));
                                vertices.Add(new VertexPositionNormalTexture(data.Corner2, data.Normal, file.SourcePolygonTopRight));

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }
                        }
                    }
                }


            }
            #endregion

            Frame result = new Frame();
            result.FromVerticesCount(startVerticeIndex, vertices.Count - startVerticeIndex);
            return result;
        }

        public static Frame VoxelGridToVerticesHD(VoxelObjGridDataHD materialGrid, Vector3 posAdjust,
            ref List<VertexPositionColorTexture> vertices, ref List<int> indexDrawOrder)
        {
            ushort material;
            IntVector3 size = materialGrid.Size;
            IntVector3 limits = materialGrid.Limits;
            IntVector3 gridPos = IntVector3.Zero;
            UInt16 totalVerticeIx = 0;
            int startVerticeIndex = vertices.Count;
            //int tileIx;

            Vector3 position = Vector3.Zero;

            Graphics.Sprite file = DataLib.SpriteCollection.Get(SpriteName.WhiteArea);//Graphics.ImageFile2
            Color faceColor;

            //bool drawFace;
            #region LOOP_TILES
            for (gridPos.Y = 0; gridPos.Y < size.Y; gridPos.Y++)
            {
                bool notCheckTop = gridPos.Y == 0;
                bool notCheckBottom = gridPos.Y == limits.Y;
                position.Y = gridPos.Y + posAdjust.Y;

                for (gridPos.Z = 0; gridPos.Z < size.Z; gridPos.Z++)
                {
                    bool notCheckFront = gridPos.Z == limits.Z;
                    bool notCheckBack = gridPos.Z == 0;
                    position.Z = gridPos.Z + posAdjust.Z;

                    for (gridPos.X = 0; gridPos.X < size.X; gridPos.X++)
                    {
                        material = materialGrid.MaterialGrid[gridPos.X, gridPos.Y, gridPos.Z];

                        if (material != BlockHD.EmptyBlock)
                        {
                            bool notCheckLeft = gridPos.X == limits.X;
                            bool notCheckRight = gridPos.X == 0;
                            position.X = gridPos.X + posAdjust.X;

                            //TOP
                            if (notCheckBottom || materialGrid.MaterialGrid[gridPos.X, gridPos.Y + 1, gridPos.Z] == BlockHD.EmptyBlock)
                            {
                                Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(position,
                                    //FACE
                                    CubeFace.Ypositive);

                                faceColor = BlockHD.YellowTintCol(material);//material.YellowTintCol();

                                indexDrawOrder.Add(vertices.Count);
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 3));
                                vertices.Add(new VertexPositionColorTexture(data.Corner3, faceColor, file.SourcePolygonLowLeft));
                                vertices.Add(new VertexPositionColorTexture(data.Corner1, faceColor, file.SourcePolygonTopLeft));
                                vertices.Add(new VertexPositionColorTexture(data.Corner4, faceColor, file.SourcePolygonLowRight));
                                vertices.Add(new VertexPositionColorTexture(data.Corner2, faceColor, file.SourcePolygonTopRight));

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }
                            //BOTTOM
                            if (notCheckTop || materialGrid.MaterialGrid[gridPos.X, gridPos.Y - 1, gridPos.Z] == BlockHD.EmptyBlock)
                            {
                                Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(position,
                                    //FACE
                                    CubeFace.Ynegative);

                                faceColor = BlockHD.DarkTintCol(material);// material.DarkTintCol();

                                indexDrawOrder.Add(vertices.Count);
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 3));
                                vertices.Add(new VertexPositionColorTexture(data.Corner3, faceColor, file.SourcePolygonLowLeft));
                                vertices.Add(new VertexPositionColorTexture(data.Corner1, faceColor, file.SourcePolygonTopLeft));
                                vertices.Add(new VertexPositionColorTexture(data.Corner4, faceColor, file.SourcePolygonLowRight));
                                vertices.Add(new VertexPositionColorTexture(data.Corner2, faceColor, file.SourcePolygonTopRight));

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }
                            //FRONT
                            if (notCheckFront || materialGrid.MaterialGrid[gridPos.X, gridPos.Y, gridPos.Z + 1] == BlockHD.EmptyBlock)
                            {
                                Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(position,
                                    //FACE
                                    CubeFace.Zpositive);

                                faceColor = BlockHD.ToColor(material);//material.GetColor();

                                indexDrawOrder.Add(vertices.Count);
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 3));
                                vertices.Add(new VertexPositionColorTexture(data.Corner3, faceColor, file.SourcePolygonLowLeft));
                                vertices.Add(new VertexPositionColorTexture(data.Corner1, faceColor, file.SourcePolygonTopLeft));
                                vertices.Add(new VertexPositionColorTexture(data.Corner4, faceColor, file.SourcePolygonLowRight));
                                vertices.Add(new VertexPositionColorTexture(data.Corner2, faceColor, file.SourcePolygonTopRight));

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }
                            //BACK
                            if (notCheckBack || materialGrid.MaterialGrid[gridPos.X, gridPos.Y, gridPos.Z - 1] == BlockHD.EmptyBlock)
                            {
                                Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(position,
                                    //FACE
                                    CubeFace.Znegative);

                                faceColor = BlockHD.ToColor(material);//material.GetColor();

                                indexDrawOrder.Add(vertices.Count);
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 3));
                                vertices.Add(new VertexPositionColorTexture(data.Corner3, faceColor, file.SourcePolygonLowLeft));
                                vertices.Add(new VertexPositionColorTexture(data.Corner1, faceColor, file.SourcePolygonTopLeft));
                                vertices.Add(new VertexPositionColorTexture(data.Corner4, faceColor, file.SourcePolygonLowRight));
                                vertices.Add(new VertexPositionColorTexture(data.Corner2, faceColor, file.SourcePolygonTopRight));

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }
                            //RIGHT
                            if (notCheckRight || materialGrid.MaterialGrid[gridPos.X - 1, gridPos.Y, gridPos.Z] == BlockHD.EmptyBlock)
                            {
                                Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(position,
                                    //FACE
                                    CubeFace.Xnegative);

                                faceColor = BlockHD.BlueTintCol(material);//material.BlueTintCol();

                                indexDrawOrder.Add(vertices.Count);
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 3));
                                vertices.Add(new VertexPositionColorTexture(data.Corner3, faceColor, file.SourcePolygonLowLeft));
                                vertices.Add(new VertexPositionColorTexture(data.Corner1, faceColor, file.SourcePolygonTopLeft));
                                vertices.Add(new VertexPositionColorTexture(data.Corner4, faceColor, file.SourcePolygonLowRight));
                                vertices.Add(new VertexPositionColorTexture(data.Corner2, faceColor, file.SourcePolygonTopRight));

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }
                            //LEFT
                            if (notCheckLeft || materialGrid.MaterialGrid[gridPos.X + 1, gridPos.Y, gridPos.Z] == BlockHD.EmptyBlock)
                            {
                                Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(position,
                                    //FACE
                                    CubeFace.Xpositive);

                                faceColor = BlockHD.BlueTintCol(material);//material.BlueTintCol();

                                indexDrawOrder.Add(vertices.Count);
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 2));
                                indexDrawOrder.Add((vertices.Count + 1));
                                indexDrawOrder.Add((vertices.Count + 3));
                                vertices.Add(new VertexPositionColorTexture(data.Corner3, faceColor, file.SourcePolygonLowLeft));
                                vertices.Add(new VertexPositionColorTexture(data.Corner1, faceColor, file.SourcePolygonTopLeft));
                                vertices.Add(new VertexPositionColorTexture(data.Corner4, faceColor, file.SourcePolygonLowRight));
                                vertices.Add(new VertexPositionColorTexture(data.Corner2, faceColor, file.SourcePolygonTopRight));

                                totalVerticeIx += BlockLib.NumVerticesPerFace;
                            }
                        }
                    }
                }


            }
            #endregion

            Frame result = new Frame();
            result.FromVerticesCount(startVerticeIndex, vertices.Count - startVerticeIndex);
            return result;
        }



        public static List<Graphics.PolygonNormal> VoxelGridToPolygons(VoxelObjGridData materialGrid, Vector3 posAdjust)
        {
            List<Graphics.PolygonNormal> polygons = new List<Graphics.PolygonNormal>();
            IntVector3 pos = IntVector3.Zero;
            for (pos.X = 0; pos.X <= materialGrid.Limits.X; pos.X++)
            {
                for (pos.Y = 0; pos.Y <= materialGrid.Limits.Y; pos.Y++)
                {
                    for (pos.Z = 0; pos.Z <= materialGrid.Limits.Z; pos.Z++)
                    {
                        byte material = materialGrid.Get(pos);
                        if (material != 0)
                        {
                            List<Map.PreparedFace> faces = new List<Map.PreparedFace>();
                            //TOP
                            IntVector3 checkPos = pos;
                            checkPos.Y += Data.Block.FaceTopDir;
                            if (materialGrid.GetSafe(checkPos) == 0)
                            {
                                faces.Add(new Map.PreparedFace(CubeFace.Ypositive, material, pos));
                            }
                            //BOTTOM
                            checkPos = pos;
                            checkPos.Y -= Data.Block.FaceTopDir;
                            if (materialGrid.GetSafe(checkPos) == 0)
                            {
                                faces.Add(new Map.PreparedFace(CubeFace.Ynegative, material, pos));
                            }

                            //FRONT
                            checkPos = pos;
                            checkPos.Z += Data.Block.FaceFrontDir;
                            if (materialGrid.GetSafe(checkPos) == 0)
                            {
                                faces.Add(new Map.PreparedFace(CubeFace.Zpositive, material, pos));
                            }
                            //BACK
                            checkPos = pos;
                            checkPos.Z -= Data.Block.FaceFrontDir;
                            if (materialGrid.GetSafe(checkPos) == 0)
                            {
                                faces.Add(new Map.PreparedFace(CubeFace.Znegative, material, pos));
                            }
                            //RIGHT
                            checkPos = pos;
                            checkPos.X += Data.Block.FaceRightDir;
                            if (materialGrid.GetSafe(checkPos) == 0)
                            {
                                faces.Add(new Map.PreparedFace(CubeFace.Xnegative, material, pos));
                            }
                            //LEFT
                            checkPos = pos;
                            checkPos.X -= Data.Block.FaceRightDir;
                            if (materialGrid.GetSafe(checkPos) == 0)
                            {
                                faces.Add(new Map.PreparedFace(CubeFace.Xpositive, material, pos));
                            }
                            if (faces.Count > 0)
                            {
                                foreach (Map.PreparedFace face in faces)
                                {
                                    Vector3 setpos = posAdjust;
                                    setpos.X += pos.X;
                                    setpos.Y += pos.Y;
                                    setpos.Z += pos.Z;

                                    polygons.Add(new Graphics.PolygonNormal(face, setpos));
                                }
                            }
                        }
                    }
                }
            }

            return polygons;
        }

        static List<Graphics.PolygonNormal> VoxelsToPolygons(List<Voxel> voxels,
            IntervalIntV3 drawLimits, Vector3 posAdjust)
        {
            byte[, ,] materialGrid = Voxels.VoxelLib.MaterialGrid(drawLimits, voxels);

            List<Graphics.PolygonNormal> polygons = new List<Graphics.PolygonNormal>();
            //foreach (Voxel v in voxels)
            for (int i = 0; i < voxels.Count; ++i)
            {//build polygons
                Voxel v = voxels[i];

                List<Map.PreparedFace> faces = new List<Map.PreparedFace>();
                //TOP
                IntVector3 checkPos = v.Position;
                checkPos.Y += Data.Block.FaceTopDir;
                if (VoxelLib.emptyBlock(materialGrid, drawLimits, checkPos))
                {
                    faces.Add(new Map.PreparedFace(CubeFace.Ypositive, v.Material, v.Position));
                }

                //BOTTOM
                checkPos = v.Position;
                checkPos.Y -= Data.Block.FaceTopDir;
                if (VoxelLib.emptyBlock(materialGrid, drawLimits, checkPos))
                {
                    faces.Add(new Map.PreparedFace(CubeFace.Ynegative, v.Material, v.Position));
                }

                //FRONT
                checkPos = v.Position;
                checkPos.Z += Data.Block.FaceFrontDir;
                if (VoxelLib.emptyBlock(materialGrid, drawLimits, checkPos))
                {
                    faces.Add(new Map.PreparedFace(CubeFace.Zpositive, v.Material, v.Position));
                }
                //BACK
                checkPos = v.Position;
                checkPos.Z -= Data.Block.FaceFrontDir;
                if (VoxelLib.emptyBlock(materialGrid, drawLimits, checkPos))
                {
                    faces.Add(new Map.PreparedFace(CubeFace.Znegative, v.Material, v.Position));
                }
                //RIGHT
                checkPos = v.Position;
                checkPos.X += Data.Block.FaceRightDir;
                if (VoxelLib.emptyBlock(materialGrid, drawLimits, checkPos))
                {
                    faces.Add(new Map.PreparedFace(CubeFace.Xnegative, v.Material, v.Position));
                }
                //LEFT
                checkPos = v.Position;
                checkPos.X -= Data.Block.FaceRightDir;
                if (VoxelLib.emptyBlock(materialGrid, drawLimits, checkPos))
                {
                    faces.Add(new Map.PreparedFace(CubeFace.Xpositive, v.Material, v.Position));
                }
                if (faces.Count > 0)
                {
                    foreach (Map.PreparedFace face in faces)
                    {
                        Vector3 pos = posAdjust;
                        pos.X += v.Position.X;
                        pos.Y += v.Position.Y;
                        pos.Z += v.Position.Z;

                        polygons.Add(new Graphics.PolygonNormal(face, pos));
                    }
                }
            }
            return polygons;
        }

        //public static Vector3 CenterAdjust(IntVector3 drawLimits)
        //{
        //    Vector3 result = drawLimits.Vec * -PublicConstants.Half;
        //    result.Y = 0;
        //    return result;
        //}


    }
}
