using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Voxels;


namespace VikingEngine.LootFest.Map.HDvoxel
{
    static class BlockPatternMaterialsLib
    {
        public static ColorGrid[] MaterialColors;

        public static void Init()
        {
            MaterialColors = new ColorGrid[(int)BlockPatternMaterial.NUM];

            LoadPattern(VoxelModelName.NUM_NON, BlockPatternMaterial.Grass);
            LoadPattern(VoxelModelName.NUM_NON, BlockPatternMaterial.Sand);
            LoadPattern(VoxelModelName.NUM_NON, BlockPatternMaterial.Dirt);
        }

        static void LoadPattern(VoxelModelName modelName, BlockPatternMaterial material)
        {
            var grid = VoxelObjDataLoader.LoadVoxelObjGrid(modelName)[0];
            MaterialColors[(int)material] = new ColorGrid(grid);
        }

        public static ushort ToBlock(ushort blockValue, IntVector3 wp)
        {
            int r = blockValue >> 12;
            
            var grid = BlockPatternMaterialsLib.MaterialColors[r];
            IntVector3 sz = grid.size;

            wp.X %= sz.X;
            wp.Y %= sz.Y;
            wp.Z %= sz.Z;

            return grid.colors[wp.X, wp.Y, wp.Z];
        }
    }

    class ColorGrid
    {
        public ushort[, ,] colors;
        public IntVector3 size;
        
        public ColorGrid(VikingEngine.Voxels.VoxelObjGridDataHD data)
        {
            this.size = data.Size;
            colors = new ushort[size.X, size.Y, size.Z];

            ForXYZLoop loop = new ForXYZLoop(size);
            while (loop.Next())
            {
                colors[loop.Position.X, loop.Position.Y, loop.Position.Z] = data.Get(loop.Position);
            }
        }


    }

    enum BlockPatternMaterial
    {
        Grass,
        Sand,
        Dirt,
        NUM
    }
}
