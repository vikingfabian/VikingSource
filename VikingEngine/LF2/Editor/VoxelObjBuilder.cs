using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using VikingEngine.Voxels;

namespace VikingEngine.LF2.Editor
{
    static class VoxelObjBuilder
    {

        public static Graphics.VoxelObj BuildFromVoxels(IntVector3 drawLimits, List<Voxel> voxels, Vector3 posAdjust)
        {
            return BuildFromVoxels(new RangeIntV3(IntVector3.Zero, drawLimits), voxels, posAdjust);
        }

        const int MinPolyCount = 400;
        const int MinVertexCount = MinPolyCount * 4;
        const int MinDrawOrderLenght = MinPolyCount * 6;


        public static Graphics.VoxelObj BuildFromVoxels(RangeIntV3 drawLimits, List<Voxel> voxels, Vector3 posAdjust)
        {
            if (voxels.Count == 0)
            {
                throw new Exception("Empty voxel array");
            }
            Graphics.VoxelObj result = new Graphics.VoxelObj(false);
            List<Graphics.PolygonNormal> polygons = VoxelsToPolygons(voxels,
                drawLimits, posAdjust);
            if (polygons.Count == 0)
            { return null; }
            result.BuildFromPolygons(new Graphics.PolygonsAndTrianglesNormal(polygons,
                new List<Graphics.TriangleNormal>()), LoadedTexture.LF_TargetSheet);
            return result;
        }

        public static Graphics.VoxelObj BuildFromVoxelGrid2(VoxelObjGridData grid, Vector3 posAdjust)
        {
            Graphics.VoxelObj result = new Graphics.VoxelObj(false);
            //old
            //List<PolygonNormal> polygons = VoxelGridToPolygons(grid, posAdjust);
            //result.BuildFromPolygons(new Graphics.PolygonsAndTrianglesNormal(polygons,
            //    new List<Graphics.TriangleNormal>()), LoadedTexture.LF_TargetSheet);
            
            //new
            List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>(MinVertexCount);
            List<int> indexDrawOrder = new List<int>();
            VoxelGridToVertices(grid, posAdjust, ref vertices, ref indexDrawOrder);
            VerticeDataNormal verticeData = new VerticeDataNormal(vertices, indexDrawOrder);
            result.BuildFromVerticeData(verticeData, LoadedTexture.LF_TargetSheet);

            result.SetOneScale(grid.Limits);
            return result;
        }

        public static Graphics.VoxelObjAnimated BuildAnimatedFromVoxelGrid2(List<VoxelObjGridData> grids, Vector3 posAdjust)
        {
            Graphics.VoxelObjAnimated result = new Graphics.VoxelObjAnimated(false);


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
            result.BuildFromVerticeData(verticeData, framesData, LoadedTexture.LF_TargetSheet);
            result.SetOneScale(grids[0].Limits);
            return result;
        }

        public static Graphics.VoxelObjAnimated BuildAnimatedFromVoxels(RangeIntV3 drawLimits, List<List<Voxel>> framesVoxels, Vector3 posAdjust)
        {
            Graphics.VoxelObjAnimated result = new Graphics.VoxelObjAnimated(false);
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
                    new List<Graphics.TriangleNormal>()), numPolys, LoadedTexture.LF_TargetSheet);
            result.SetOneScale(drawLimits.Add);
            return result;
        }

        static Frame VoxelGridToVertices(VoxelObjGridData materialGrid, Vector3 posAdjust,
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
                                tileIx = Data.MaterialBuilder.Materials[material].TopTiles.GetRandomTile(gridPos);
                                Graphics.ImageFile2 file = LF2.LootfestLib.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LF2.Data.Block.GetVoxelObjFace(position,
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
                                tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.ImageFile2 file = LF2.LootfestLib.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LF2.Data.Block.GetVoxelObjFace(position,
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
                               
                                tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.ImageFile2 file = LF2.LootfestLib.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LF2.Data.Block.GetVoxelObjFace(position,
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
                               
                                tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.ImageFile2 file = LF2.LootfestLib.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LF2.Data.Block.GetVoxelObjFace(position,
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
                               
                                tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.ImageFile2 file = LF2.LootfestLib.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LF2.Data.Block.GetVoxelObjFace(position,
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
                               
                                tileIx = Data.MaterialBuilder.Materials[material].SideTiles.GetRandomTile(gridPos);
                                Graphics.ImageFile2 file = LF2.LootfestLib.Images.TileIxToImgeFile[tileIx];
                                Graphics.Face data = LF2.Data.Block.GetVoxelObjFace(position,
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

        static List<Graphics.PolygonNormal> VoxelGridToPolygons(VoxelObjGridData materialGrid, Vector3 posAdjust)
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
            RangeIntV3 drawLimits, Vector3 posAdjust)
        {
            byte[, ,] materialGrid = Voxels.VoxelLib.MaterialGrid(drawLimits, voxels);

            List<Graphics.PolygonNormal> polygons = new List<Graphics.PolygonNormal>();
            foreach (Voxel v in voxels)
            {//build polygons
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

        public static Vector3 CenterAdjust(IntVector3 drawLimits)
        {
            Vector3 result = drawLimits.Vec * -PublicConstants.Half;
            result.Y = 0;
            return result;
        }


    }
}
