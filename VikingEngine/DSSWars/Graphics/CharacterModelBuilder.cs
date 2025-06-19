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
            IntVector3 legOffSet = new IntVector3(5, 0, 32);

            VoxelObjGridDataAnimHD grid = new VoxelObjGridDataAnimHD(GridSize, FrameCount);

            var debug = DssRef.models.rawModels[VoxelModelName.modsoldier_debug];
            var face = DssRef.models.rawModels[VoxelModelName.modsoldier_face1];
            var body = DssRef.models.rawModels[VoxelModelName.modsoldier_body1];
            var leg = DssRef.models.rawModels[VoxelModelName.modsoldier_leg1];


            var firstFrame = debug.Frames[0].GetVoxelArray();
            var legsIdle = leg.Frames[0].GetVoxelArray(legOffSet);
            for (int frame = 0; frame < 2; frame++)
            {                
                grid.Frames[frame].AddVoxels(firstFrame);
                grid.Frames[frame].AddVoxels(legsIdle);

            }
            for (int frame = 2; frame < FrameCount; frame++)
            {
                var voxels = debug.Frames[frame-1].GetVoxelArray();
                grid.Frames[frame].AddVoxels(voxels);
                grid.Frames[frame].AddVoxels(leg.Frames[frame - 1].GetVoxelArray(legOffSet));
            }


            var bodyVoxels = body.Frames[0].GetVoxelArray(new IntVector3(6, 0, 33));
            var faceVoxels = face.Frames[0].GetVoxelArray(new IntVector3(2, 5, 30));
            for (int frame = 0; frame < FrameCount; frame++)
            {
                
                grid.Frames[frame].AddVoxels(bodyVoxels);
                grid.Frames[frame].AddVoxels(faceVoxels);
                
            }

            var centerAdjust = grid.Frames[0].BottomCenterAdj();
            buildVerticeDataHD_ColorNormal(grid.Frames, centerAdjust);
            Graphics.VoxelModel model = modelFromVertices();

            return model;
        }
    }
}
