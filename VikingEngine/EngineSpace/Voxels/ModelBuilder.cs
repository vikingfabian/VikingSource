using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.Voxels
{
    class ModelBuilder
    {
        const int MinPolyCount = 1024;
        const int MinVertexCount = MinPolyCount * 4;
        const int MinDrawOrderLenght = MinPolyCount * 6;

        IVerticeData verticeData;
        List<Frame> framesData;
        protected IntVector3 gridSz;

        public ModelBuilder()
        { }

        public void buildVerticeDataHD(List<VoxelObjGridDataHD> grids, Vector3 posAdjust)
        {

            List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>(MinVertexCount * grids.Count);
            List<int> indexDrawOrder = new List<int>(MinDrawOrderLenght);
            framesData = new List<Frame>(grids.Count);

            foreach (VoxelObjGridDataHD grid in grids)
            {
                Frame frameData = LootFest.Editor.VoxelObjBuilder.VoxelGridToVerticesHD(
                    grid, posAdjust, ref vertices, ref indexDrawOrder);
                framesData.Add(frameData);
            }

            if (vertices.Count > 0)
            {
                gridSz = grids[0].Size;
                verticeData = new VerticeDataColorTexture(vertices, indexDrawOrder);
            }
        }

        public Graphics.VoxelModel modelFromVertices()
        {
            if (verticeData == null)
            {
                return null;
            }
            
            Graphics.VoxelModel result = new Graphics.VoxelModel(false);
            result.BuildFromVerticeData(verticeData, framesData, LootFest.LfLib.BlockTexture);
            result.SetOneScale(gridSz);

            return result;
        }
    }

}
