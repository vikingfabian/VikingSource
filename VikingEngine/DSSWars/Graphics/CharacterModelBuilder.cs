using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.LootFest;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars
{
    class CharacterModelBuilder : Voxels.ModelBuilder
    {
        static readonly IntVector3 GridSize = new IntVector3(26, 48, 78);
        const int FrameCount = 7;

        //26*48*78

        public Graphics.VoxelModel buildModel(Faction faction, SoldierModelData modelData)
        {
            VoxelObjGridDataAnimHD grid = new VoxelObjGridDataAnimHD(GridSize, FrameCount);
            

            var debug = DssRef.models.rawModels[VoxelModelName.modsoldier_debug];

            var firstFrame = debug.Frames[0].GetVoxelArray();
            for (int frame = 0; frame < 2; frame++)
            {                
                grid.Frames[frame].AddVoxels(firstFrame);
            }
            for (int frame = 2; frame < FrameCount; frame++)
            {
                var voxels = debug.Frames[frame-1].GetVoxelArray();
                grid.Frames[frame].AddVoxels(voxels);
            }

            var centerAdjust = grid.Frames[0].BottomCenterAdj();
            buildVerticeDataHD_ColorNormal(grid.Frames, centerAdjust);
            Graphics.VoxelModel model = modelFromVertices();

            return model;
        }
    }
}
