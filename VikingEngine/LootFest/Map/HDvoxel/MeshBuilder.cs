using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.Map.HDvoxel
{
    class MeshBuilder
    {
        const int BlueTintAddR = -6;
        const int BlueTintAddG = -6;
        const int BlueTintAddB = 12;

        const int RedTintAddR = 8;
        const int RedTintAddG = 4;
        const int RedTintAddB = -8;

        static readonly int TopFaceIndex = (int)CubeFace.Ypositive;
        static readonly int BottomFaceIndex = (int)CubeFace.Ynegative;
        static readonly int FrontFaceIndex = (int)CubeFace.Zpositive;
        static readonly int BackFaceIndex = (int)CubeFace.Znegative;
        static readonly int LeftFaceIndex = (int)CubeFace.Xnegative;
        static readonly int RightFaceIndex = (int)CubeFace.Xpositive;

        public const int MaxArraySize = 1000000;

        Map.WorldPosition gridPos = Map.WorldPosition.EmptyPos;
        Map.WorldPosition adjgridPos = Map.WorldPosition.EmptyPos;
        Map.WorldPosition ssaoCheckPos = Map.WorldPosition.EmptyPos;

        StaticCountingList<VertexPositionColor> vertices;
        StaticCountingList<int> indexDrawOrder;
        UInt16 totalVerticeIx = 0;

        VikingEngine.Graphics.Face[] faces;
        VikingEngine.Graphics.Face[] lowresFaces;

        //VikingEngine.Graphics.Face[] facesWidth2;
        //VikingEngine.Graphics.Face[] facesWidth4;
        const byte SsaoColAdd = 3;

        IntVector2 preparedSize;
        ushort[,][, ,] preparedGrids;

        public MeshBuilder()
        {
            vertices = new StaticCountingList<VertexPositionColor>(MaxArraySize);
            vertices.FillArrayWith(new VertexPositionColor());
            indexDrawOrder = new StaticCountingList<int>(MaxArraySize + MaxArraySize / 2);

            faces = VikingEngine.Graphics.PolygonLib.createFaces(1f);
            //facesWidth2 = VikingEngine.Graphics.PolygonLib.createFaces(2);
            //facesWidth4 = VikingEngine.Graphics.PolygonLib.createFaces(4);
            lowresFaces = VikingEngine.Graphics.PolygonLib.createFaces(LowResChunk.BlockSteps);
        }

        public bool prepareArea(IntVector2 startChunk, IntVector2 endChunk)
        {
            IntVector2 topleft = startChunk - 1;
            IntVector2 bottomRight = endChunk + 1;

            var tlChunk = LfRef.chunks.GetScreenUnsafe(topleft);
            var brChunk = LfRef.chunks.GetScreenUnsafe(bottomRight);

            if (tlChunk == null ||
                brChunk == null ||
                tlChunk.openstatus < ScreenOpenStatus.Detail_2 ||
                brChunk.openstatus < ScreenOpenStatus.Detail_2)
            {
                return false;
            }


            var size = (bottomRight - topleft) + 1;
            if (size != preparedSize)
            {
                preparedSize = size;
                preparedGrids = new ushort[preparedSize.X, preparedSize.Y][, ,];
            }

            IntVector2 pos = IntVector2.Zero;
            for (pos.Y = topleft.Y; pos.Y <= bottomRight.Y; ++pos.Y)
            {
                for (pos.X = topleft.X; pos.X <= bottomRight.X; ++pos.X)
                {
                    var chunk = LfRef.chunks.GetScreenUnsafe(pos);
                    if (chunk == null || chunk.openstatus < ScreenOpenStatus.Detail_2)
                    {
                        return false;
                    }
                    else
                    {
                        ushort[, ,] grid = chunk.DataGrid;
                        //if (grid == null)
                        //{
                        //    grid = chunk.unCompressToGrid();
                        //    if (grid == null)
                        //    {
                        //        return false;
                        //    }
                        //}

                        

                        var preparedGridsPos = pos - topleft;
                        preparedGrids[preparedGridsPos.X, preparedGridsPos.Y] = grid;
                    }
                }
            }

            return true;
        }

        public void resetCounting()
        {
            totalVerticeIx = 0;
            vertices.ResetCounting();
            indexDrawOrder.ResetCounting();
        }


        public Graphics.VertexAndIndexBuffer BuildScreen(
            WorldPosition wp,
            Graphics.LFHeightMap heightMap)
        {
            Graphics.Face face;
            int brightness;
            ushort material, adjMaterial;
            resetCounting();

            IntVector3 endPos = wp.WorldGrindex;
            endPos.X += WorldPosition.ChunkWidth;
            endPos.Y += WorldPosition.ChunkHeight -1;
            endPos.Z += WorldPosition.ChunkWidth;

            
            for (gridPos.WorldGrindex.Z = wp.WorldGrindex.Z; gridPos.WorldGrindex.Z < endPos.Z; ++gridPos.WorldGrindex.Z)
            {
                for (gridPos.WorldGrindex.X = wp.WorldGrindex.X; gridPos.WorldGrindex.X < endPos.X; ++gridPos.WorldGrindex.X)
                {
                    int fallShadow = 0;
                    for (gridPos.WorldGrindex.Y = endPos.Y; gridPos.WorldGrindex.Y > 0; --gridPos.WorldGrindex.Y)
                    {
                        material = gridPos.GetBlock_Unsafe();

                        if (material == BlockHD.EmptyBlock)
                        {
                            fallShadow -= 2;
                        }
                        else
                        {
                            {//TOP
                                brightness = 0;

                                if (gridPos.WorldGrindex.Y < Map.WorldPosition.MaxChunkY)
                                {
                                    adjgridPos = gridPos;
                                    ++adjgridPos.WorldGrindex.Y;
                                    adjMaterial = adjgridPos.GetBlock_Unsafe();
                                }
                                else
                                {
                                    adjMaterial = 0;
                                }
                                
                                if (adjMaterial == 0)
                                {
                                    {//SSAO -X
                                        ssaoCheckPos = adjgridPos;
                                        --ssaoCheckPos.X;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            --ssaoCheckPos.Y;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO +X
                                        ssaoCheckPos = adjgridPos;
                                        ++ssaoCheckPos.X;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            --ssaoCheckPos.Y;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO -Z
                                        ssaoCheckPos = adjgridPos;
                                        --ssaoCheckPos.Z;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            --ssaoCheckPos.Y;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO +Z
                                        ssaoCheckPos = adjgridPos;
                                        ++ssaoCheckPos.Z;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            --ssaoCheckPos.Y;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    if (fallShadow > 0)
                                    {
                                        brightness -= fallShadow;
                                    }
                                    
                                    face = faces[TopFaceIndex];
                                    face.Move(gridPos.LocalBlockGrindex);

                                    addVertices(ref face, BlockHD.FaceColorTinted(material, brightness, brightness, brightness));//material.faceColorTinted(brightness, brightness, brightness, gridPos.WorldGrindex));
                                }
                                
                            }//end top

                            {//BOTTOM
                                brightness = 0;
                                adjgridPos = gridPos;
                                --adjgridPos.WorldGrindex.Y;

                                adjMaterial = adjgridPos.GetBlock_Unsafe();
                                if (adjMaterial == 0)
                                {

                                    {//SSAO -X
                                        ssaoCheckPos = adjgridPos;
                                        --ssaoCheckPos.X;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            ++ssaoCheckPos.Y;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO +X
                                        ssaoCheckPos = adjgridPos;
                                        ++ssaoCheckPos.X;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            ++ssaoCheckPos.Y;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO -Z
                                        ssaoCheckPos = adjgridPos;
                                        --ssaoCheckPos.Z;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            ++ssaoCheckPos.Y;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO +Z
                                        ssaoCheckPos = adjgridPos;
                                        ++ssaoCheckPos.Z;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            ++ssaoCheckPos.Y;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }
                                    
                                    face = faces[BottomFaceIndex];
                                    face.Move(gridPos.LocalBlockGrindex);

                                    addVertices(ref face, BlockHD.FaceColorTinted(material, brightness, brightness, brightness));
                                }

                            }//end bottom

                            {//FRONT
                                brightness = 0;
                                adjgridPos = gridPos;
                                ++adjgridPos.WorldGrindex.Z;

                                adjMaterial = adjgridPos.GetBlock_Unsafe();
                                if (adjMaterial == 0)
                                {
                                    {//SSAO -Y
                                        ssaoCheckPos = adjgridPos;
                                        --ssaoCheckPos.Y;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            --ssaoCheckPos.Z;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO +Y
                                        ssaoCheckPos = adjgridPos;
                                        ++ssaoCheckPos.Y;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            --ssaoCheckPos.Z;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO -X
                                        ssaoCheckPos = adjgridPos;
                                        --ssaoCheckPos.X;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            --ssaoCheckPos.Z;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO +X
                                        ssaoCheckPos = adjgridPos;
                                        ++ssaoCheckPos.X;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            --ssaoCheckPos.Z;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    face = faces[FrontFaceIndex];
                                    face.Move(gridPos.LocalBlockGrindex);

                                    addVertices(ref face, BlockHD.FaceColorTinted(material, brightness + RedTintAddR, brightness + RedTintAddG, brightness + RedTintAddB));
                                }

                            }//end front

                            {//BACK
                                brightness = 0;
                                adjgridPos = gridPos;
                                --adjgridPos.WorldGrindex.Z;

                                adjMaterial = adjgridPos.GetBlock_Unsafe();
                                if (adjMaterial == 0)
                                {
                                    {//SSAO -Y
                                        ssaoCheckPos = adjgridPos;
                                        --ssaoCheckPos.Y;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            ++ssaoCheckPos.Z;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO +Y
                                        ssaoCheckPos = adjgridPos;
                                        ++ssaoCheckPos.Y;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            ++ssaoCheckPos.Z;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO -X
                                        ssaoCheckPos = adjgridPos;
                                        --ssaoCheckPos.X;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            ++ssaoCheckPos.Z;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO +X
                                        ssaoCheckPos = adjgridPos;
                                        ++ssaoCheckPos.X;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            ++ssaoCheckPos.Z;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    face = faces[BackFaceIndex];
                                    face.Move(gridPos.LocalBlockGrindex);

                                    addVertices(ref face, BlockHD.FaceColorTinted(material, brightness + RedTintAddR, brightness + RedTintAddG, brightness + RedTintAddB));
                                }

                            }//end back

                            {//LEFT
                                brightness = 0;
                                adjgridPos = gridPos;
                                --adjgridPos.WorldGrindex.X;

                                adjMaterial = adjgridPos.GetBlock_Unsafe();
                                if (adjMaterial == 0)
                                {
                                    {//SSAO -Y
                                        ssaoCheckPos = adjgridPos;
                                        --ssaoCheckPos.Y;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            ++ssaoCheckPos.X;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO +Y
                                        ssaoCheckPos = adjgridPos;
                                        ++ssaoCheckPos.Y;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            ++ssaoCheckPos.X;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO -Z
                                        ssaoCheckPos = adjgridPos;
                                        --ssaoCheckPos.Z;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            ++ssaoCheckPos.X;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO +Z
                                        ssaoCheckPos = adjgridPos;
                                        ++ssaoCheckPos.Z;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            ++ssaoCheckPos.X;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    face = faces[LeftFaceIndex];
                                    face.Move(gridPos.LocalBlockGrindex);

                                    addVertices(ref face, BlockHD.FaceColorTinted(material, brightness + BlueTintAddR, brightness + BlueTintAddG, brightness + BlueTintAddB));
                                }

                            }//end left

                            {//RIGHT
                                brightness = 0;
                                adjgridPos = gridPos;
                                ++adjgridPos.WorldGrindex.X;

                                adjMaterial = adjgridPos.GetBlock_Unsafe();
                                if (adjMaterial == 0)
                                {
                                    {//SSAO -Y
                                        ssaoCheckPos = adjgridPos;
                                        --ssaoCheckPos.Y;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            --ssaoCheckPos.X;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO +Y
                                        ssaoCheckPos = adjgridPos;
                                        ++ssaoCheckPos.Y;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            --ssaoCheckPos.X;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO -Z
                                        ssaoCheckPos = adjgridPos;
                                        --ssaoCheckPos.Z;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            --ssaoCheckPos.X;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    {//SSAO +Z
                                        ssaoCheckPos = adjgridPos;
                                        ++ssaoCheckPos.Z;
                                        if (ssaoCheckPos.BlockIsEmpty())
                                        {
                                            --ssaoCheckPos.X;
                                            if (ssaoCheckPos.BlockIsEmpty())
                                            {
                                                brightness += SsaoColAdd;
                                            }
                                        }
                                        else
                                        {
                                            brightness -= SsaoColAdd;
                                        }
                                    }

                                    face = faces[RightFaceIndex];
                                    face.Move(gridPos.LocalBlockGrindex);

                                    addVertices(ref face, BlockHD.FaceColorTinted(material, brightness + BlueTintAddR, brightness + BlueTintAddG, brightness + BlueTintAddB));
                                }

                            }//end right

                            fallShadow = 40;
                        }
                    }
                }
            }//End Loop

            wp.ClearLocalPos();
            if (heightMap == null)
            {
                return null;
            }

            return heightMap.BuildFromPolygons(vertices, indexDrawOrder, LoadedTexture.NO_TEXTURE, wp.ChunkGrindex, wp.WorldGrindex);
        }

        

        void addVertices(ref Graphics.Face data, Color color)
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
            vertices.CurrentMember.Color = color;

            vertices.NextIndex();

            vertices.CurrentMember.Position = data.Corner1;
            vertices.CurrentMember.Color = color;

            vertices.NextIndex();

            vertices.CurrentMember.Position = data.Corner4;
            vertices.CurrentMember.Color = color;

            vertices.NextIndex();

            vertices.CurrentMember.Position = data.Corner2;
            vertices.CurrentMember.Color = color;

            vertices.NextIndex();

            totalVerticeIx += BlockLib.NumVerticesPerFace;
        }

        

        //public void BuildLowDetailEdge(LowDetailLevel level,
        //    Dir4 side,
        //    IntVector2 startChunk, IntVector2 endChunk,
        //    Graphics.LFHeightMap heightMap)
        //{
        //    ushort material, mostOutwardMaterial, adjMaterial;
        //    resetCounting();

        //    Map.WorldPosition startPos = new Map.WorldPosition(IntVector2.One);
        //    Map.WorldPosition endPos = new Map.WorldPosition(preparedSize -2);

        //    endPos.WorldGrindex.X += WorldPosition.ChunkWidth;
        //    endPos.WorldGrindex.Y += WorldPosition.ChunkHeight - 1;
        //    endPos.WorldGrindex.Z += WorldPosition.ChunkWidth;

        //    int blockWidth;
        //    Graphics.Face[] faces;

        //    switch (level)
        //    {
        //        default:
        //            faces = facesWidth2;
        //            blockWidth = 2;
        //            break;
        //        case LowDetailLevel.Level3_Width4:
        //            faces = facesWidth4;
        //            blockWidth = 4;
        //            break;
        //    }

        //    int surfaceStepsWidth = blockWidth - 1;

        //    for (gridPos.WorldGrindex.Z = startPos.WorldGrindex.Z; gridPos.WorldGrindex.Z < endPos.WorldGrindex.Z; gridPos.WorldGrindex.Z += blockWidth)
        //    {
        //        for (gridPos.WorldGrindex.X = startPos.WorldGrindex.X; gridPos.WorldGrindex.X < endPos.WorldGrindex.X; gridPos.WorldGrindex.X += blockWidth)
        //        {
        //            for (gridPos.WorldGrindex.Y = endPos.WorldGrindex.Y; gridPos.WorldGrindex.Y > 0; gridPos.WorldGrindex.Y -= blockWidth)
        //            {
        //                material = GetBlock(ref gridPos);

                        
        //                if (material != BlockHD.Empty)
        //                {
        //                    {//TOP
        //                        //brightness = 0;
        //                        adjgridPos = gridPos;
        //                        adjgridPos.WorldGrindex.Y += blockWidth;

        //                        adjMaterial = GetBlock(ref adjgridPos);
        //                        if (adjMaterial == 0)
        //                        {
        //                            Graphics.Face face = faces[TopFaceIndex];
        //                            face.Move(gridPos.WorldGrindex - startPos.WorldGrindex);

        //                            mostOutwardMaterial = material;
        //                            adjgridPos = gridPos;
        //                            for (int i = 0; i < surfaceStepsWidth; ++i)
        //                            {
        //                                adjgridPos.WorldGrindex.Y++;

        //                                adjMaterial = GetBlock(ref adjgridPos);
        //                                if (adjMaterial != 0)
        //                                {
        //                                    mostOutwardMaterial = adjMaterial;
        //                                }
        //                            }

        //                            addVertices(ref face, BlockHD.ToColor(mostOutwardMaterial));
        //                        }

        //                    }//end top

        //                    {//BOTTOM
        //                       // brightness = 0;
        //                        adjgridPos = gridPos;
        //                        adjgridPos.WorldGrindex.Y -= blockWidth;

        //                        if (adjgridPos.WorldGrindex.Y >= 0)
        //                        {
        //                            adjMaterial = GetBlock(ref adjgridPos);
        //                        }
        //                        else
        //                        {
        //                            adjMaterial = 1;
        //                        }

        //                        if (adjMaterial == 0)
        //                        {
        //                            Graphics.Face face = faces[BottomFaceIndex];
        //                            face.Move(gridPos.WorldGrindex - startPos.WorldGrindex);

        //                            addVertices(ref face, BlockHD.ToColor(material));
        //                        }

        //                    }//end bottom

        //                    {//FRONT
        //                        //brightness = 0;
        //                        adjgridPos = gridPos;
        //                        adjgridPos.WorldGrindex.Z += blockWidth;

        //                        adjMaterial = GetBlock(ref adjgridPos);
        //                        if (adjMaterial == 0)
        //                        {
        //                            Graphics.Face face = faces[FrontFaceIndex];
        //                            face.Move(gridPos.WorldGrindex - startPos.WorldGrindex);

        //                            mostOutwardMaterial = material;
        //                            adjgridPos = gridPos;
        //                            for (int i = 0; i < surfaceStepsWidth; ++i)
        //                            {
        //                                adjgridPos.WorldGrindex.Z++;

        //                                adjMaterial = GetBlock(ref adjgridPos);
        //                                if (adjMaterial != 0)
        //                                {
        //                                    mostOutwardMaterial = adjMaterial;
        //                                }
        //                            }

        //                            addVertices(ref face, BlockHD.ToColor(material));
        //                        }

        //                    }//end front

        //                    {//BACK
        //                        //brightness = 0;
        //                        adjgridPos = gridPos;
        //                        adjgridPos.WorldGrindex.Z -= blockWidth;

        //                        adjMaterial = GetBlock(ref adjgridPos);
        //                        if (adjMaterial == 0)
        //                        {
        //                            Graphics.Face face = faces[BackFaceIndex];
        //                            face.Move(gridPos.WorldGrindex - startPos.WorldGrindex);

        //                            mostOutwardMaterial = material;
        //                            adjgridPos = gridPos;
        //                            for (int i = 0; i < surfaceStepsWidth; ++i)
        //                            {
        //                                adjgridPos.WorldGrindex.Z--;

        //                                adjMaterial = GetBlock(ref adjgridPos);
        //                                if (adjMaterial != 0)
        //                                {
        //                                    mostOutwardMaterial = adjMaterial;
        //                                }
        //                            }

        //                            addVertices(ref face, BlockHD.ToColor(material));
        //                        }

        //                    }//end back

        //                    {//LEFT
        //                        //brightness = 0;
        //                        adjgridPos = gridPos;
        //                        adjgridPos.WorldGrindex.X -= blockWidth;

        //                        adjMaterial = GetBlock(ref adjgridPos);
        //                        if (adjMaterial == 0)
        //                        {
        //                            Graphics.Face face = faces[LeftFaceIndex];
        //                            face.Move(gridPos.WorldGrindex - startPos.WorldGrindex);

        //                            mostOutwardMaterial = material;
        //                            adjgridPos = gridPos;
        //                            for (int i = 0; i < surfaceStepsWidth; ++i)
        //                            {
        //                                adjgridPos.WorldGrindex.X--;

        //                                adjMaterial = GetBlock(ref adjgridPos);
        //                                if (adjMaterial != 0)
        //                                {
        //                                    mostOutwardMaterial = adjMaterial;
        //                                }
        //                            }

        //                            addVertices(ref face, BlockHD.ToColor(material));
        //                        }

        //                    }//end left

        //                    {//RIGHT
        //                        //brightness = 0;
        //                        adjgridPos = gridPos;
        //                        adjgridPos.WorldGrindex.X += blockWidth;

        //                        adjMaterial = GetBlock(ref adjgridPos);
        //                        if (adjMaterial == 0)
        //                        {
                                    
        //                            Graphics.Face face = faces[RightFaceIndex];
        //                            face.Move(gridPos.WorldGrindex - startPos.WorldGrindex);

        //                            mostOutwardMaterial = material;
        //                            adjgridPos = gridPos;
        //                            for (int i = 0; i < surfaceStepsWidth; ++i)
        //                            {
        //                                adjgridPos.WorldGrindex.X++;

        //                                adjMaterial = GetBlock(ref adjgridPos);
        //                                if (adjMaterial != 0)
        //                                {
        //                                    mostOutwardMaterial = adjMaterial;
        //                                }
        //                            }

        //                            addVertices(ref face, BlockHD.ToColor(material));
        //                        }

        //                    }//end right

        //                }
        //            }
        //        }
        //    }//End Loop

        //    heightMap.LowDetailEdgeFromPolygons(vertices, indexDrawOrder, LoadedTexture.NO_TEXTURE, startPos.WorldGrindex, side, blockWidth);
        //}

        public void addLowResChunk(ushort[,][, ,] preparedGrids, IntVector2 chunk)
        {
            IntVector3 pos = IntVector3.Zero;
            IntVector3 adjpos = IntVector3.Zero;
            ushort material, adjMaterial;

            for (pos.Z = 0; pos.Z < LowResChunk.Width; ++pos.Z)
            {
                for (pos.X = 0; pos.X < LowResChunk.Width; ++pos.X)
                {
                    for (pos.Y = 0; pos.Y < LowResChunk.Height; ++pos.Y)
                    {
                        material = GetLowResBlock(ref preparedGrids, ref pos);

                        if (material != BlockHD.EmptyBlock)
                        {
                            {//TOP
                                adjpos = pos;
                                ++adjpos.Y;

                                adjMaterial = GetLowResBlock(ref preparedGrids, ref adjpos);
                                if (adjMaterial == 0)
                                {
                                    Graphics.Face face = lowresFaces[TopFaceIndex];
                                    face.Move(GetLowResBlockPos(ref pos, ref chunk));

                                    addVertices(ref face, BlockHD.ToColor(material));
                                }
                            }//end top

                            {//BOTTOM
                                adjpos = pos;
                                --adjpos.Y;

                                adjMaterial = GetLowResBlock(ref preparedGrids, ref adjpos);
                                if (adjMaterial == 0)
                                {
                                    Graphics.Face face = lowresFaces[BottomFaceIndex];
                                    face.Move(GetLowResBlockPos(ref pos, ref chunk));

                                    addVertices(ref face, BlockHD.ToColor(material));
                                }
                            }//end bottom

                            {//FRONT
                                adjpos = pos;
                                ++adjpos.Z;

                                adjMaterial = GetLowResBlock(ref preparedGrids, ref adjpos);
                                if (adjMaterial == 0)
                                {
                                    Graphics.Face face = lowresFaces[FrontFaceIndex];
                                    face.Move(GetLowResBlockPos(ref pos, ref chunk));

                                    addVertices(ref face, BlockHD.FaceColorTinted(material, RedTintAddR, RedTintAddG, RedTintAddB));
                                }
                            }//end front

                            {//BACK
                                adjpos = pos;
                                --adjpos.Z;

                                adjMaterial = GetLowResBlock(ref preparedGrids, ref adjpos);
                                if (adjMaterial == 0)
                                {
                                    Graphics.Face face = lowresFaces[BackFaceIndex];
                                    face.Move(GetLowResBlockPos(ref pos, ref chunk));

                                    addVertices(ref face, BlockHD.FaceColorTinted(material, RedTintAddR, RedTintAddG, RedTintAddB));
                                }
                            }//end back

                            {//LEFT
                                adjpos = pos;
                                --adjpos.X;

                                adjMaterial = GetLowResBlock(ref preparedGrids, ref adjpos);
                                if (adjMaterial == 0)
                                {
                                    Graphics.Face face = lowresFaces[LeftFaceIndex];
                                    face.Move(GetLowResBlockPos(ref pos, ref chunk));

                                    addVertices(ref face, BlockHD.FaceColorTinted(material, BlueTintAddR, BlueTintAddG, BlueTintAddB));
                                }
                            }//end left

                            {//RIGHT
                                adjpos = pos;
                                ++adjpos.X;

                                adjMaterial = GetLowResBlock(ref preparedGrids, ref adjpos);
                                if (adjMaterial == 0)
                                {
                                    Graphics.Face face = lowresFaces[RightFaceIndex];
                                    face.Move(GetLowResBlockPos(ref pos, ref chunk));

                                    addVertices(ref face, BlockHD.FaceColorTinted(material, BlueTintAddR, BlueTintAddG, BlueTintAddB));
                                }
                            }//end right
                        }
                    }
                }
            }
        }

        public void endLowResMesh()
        {
            if (Ref.draw.heightmap != null)
            {
                Ref.draw.heightmap.LowDetailFromPolygons(vertices, indexDrawOrder, LoadedTexture.NO_TEXTURE);
            }
        }

        ushort GetLowResBlock(ref ushort[,][, ,] preparedGrids, ref IntVector3 pos)
        {
            int chunkX = 1, chunkY = 1;

            if (pos.Y < 0 || pos.Y >= LowResChunk.Height)
            {
                return 0;
            }

            if (pos.X < 0)
            {
                pos.X = LowResChunk.Width - 1;
                chunkX = 0;
            }
            else if (pos.X >= LowResChunk.Width)
            {
                pos.X = 0;
                chunkX = 2;
            }

            if (pos.Z < 0)
            {
                pos.Z = LowResChunk.Width - 1;
                chunkY = 0;
            }
            else if (pos.Z >= LowResChunk.Width)
            {
                pos.Z = 0;
                chunkY = 2;
            }

            ushort[, ,] grid = preparedGrids[chunkX, chunkY];
            if (grid != null)
            {
                return grid[pos.X, pos.Y, pos.Z];
            }
            return 0;
        }

        Vector3 GetLowResBlockPos(ref IntVector3 pos, ref IntVector2 chunk)
        {
            return new Vector3(
                chunk.X * Map.WorldPosition.ChunkWidth + pos.X * LowResChunk.BlockSteps,
                pos.Y * LowResChunk.BlockSteps,
                chunk.Y * Map.WorldPosition.ChunkWidth + pos.Z * LowResChunk.BlockSteps);

        }

        public ushort GetBlock(ref Map.WorldPosition wp)
        {
            ushort[,,] chunk = preparedGrids[wp.WorldGrindex.X >> Map.WorldPosition.ChunkBitsWidth, wp.WorldGrindex.Z >> Map.WorldPosition.ChunkBitsWidth];

            return chunk[wp.WorldGrindex.X & Map.WorldPosition.ChunkBitsZero, wp.WorldGrindex.Y, wp.WorldGrindex.Z & Map.WorldPosition.ChunkBitsZero];
        }

    }

    enum LowDetailLevel
    {
        Level2_Width2,
        Level3_Width4,
    }
}
