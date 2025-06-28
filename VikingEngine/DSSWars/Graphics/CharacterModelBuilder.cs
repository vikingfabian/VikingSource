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
        //modsoldier_hat_allclasses
        /*Hats
         * 0 soldier
         * 1 archer
         * 2 slinger
         * 3 javelin
         * 4 bannerman
         * 5 hirdman
         * 6 hammer
         * 7 pike
         * 8 longsword
         * 9 viking
         * 10 crossbow
         * 11 gun far
         * 12 gun near
         * 13 farmer
         */


        static readonly IntVector3 GridSize = new IntVector3(26, 48, 78);
        const int FrameCount = 7;
        //const int SwordAttackFrame = 1;
        //const int SwordForwardFrame = 0;

        //26*48*78

        public Graphics.VoxelModel buildModel(Faction faction, SoldierModelData modelData)
        {
            IntVector3 legOffSet = new IntVector3(5, 0, 32);
            IntVector3 rArmOffSet = new IntVector3(4, 0, 30);
            IntVector3 lArmOffset = VectorExt.AddX(rArmOffSet, 10);

            VoxelObjGridDataAnimHD grid = new VoxelObjGridDataAnimHD(GridSize, FrameCount);

            //var debug = DssRef.models.rawModels[VoxelModelName.modsoldier_debug];
            var face = DssRef.models.rawModels[VoxelModelName.modsoldier_face1];
            var body = DssRef.models.rawModels[VoxelModelName.modsoldier_body1];
            var leg = DssRef.models.rawModels[VoxelModelName.modsoldier_leg1];

            var leftArm = DssRef.models.rawModels[VoxelModelName.modsoldier_larm_empty1];
            var rightArm = DssRef.models.rawModels[VoxelModelName.modsoldier_rarm_sword1];

            var sword = DssRef.models.weaponModels[VoxelModelName.modweapon_sword1];
            //var swordForward = sword.Frames[SwordForwardFrame].GetVoxelArray(out ushort swordForward_jointResult, out IntVector3 swordForward_jointPos);
            //var swordAttack = sword.Frames[SwordForwardFrame].GetVoxelArray(out ushort swordAttack_jointResult, out IntVector3 swordAttack_jointPos);

            var profileColors = faction.flagProfile.GetColorReplaceTable();

            //var firstFrame = debug.Frames[0].GetVoxelArray();
            var legsIdle = leg.Frames[0].GetVoxelArray(legOffSet, profileColors);
            for (int frame = 0; frame < 2; frame++)
            {                
                //grid.Frames[frame].AddVoxels(firstFrame);
                grid.Frames[frame].AddVoxels(legsIdle);

            }
            for (int frame = 2; frame < FrameCount; frame++)
            {
                //var voxels = debug.Frames[frame-1].GetVoxelArray();
                //grid.Frames[frame].AddVoxels(voxels);
                grid.Frames[frame].AddVoxels(leg.Frames[frame - 1].GetVoxelArray(legOffSet, profileColors));
            }


            var bodyVoxels = body.Frames[0].GetVoxelArray(new IntVector3(6, 0, 33), profileColors);
            var faceVoxels = face.Frames[0].GetVoxelArray(new IntVector3(2, 5, 30), profileColors);
            for (int frame = 0; frame < FrameCount; frame++)
            {                
                grid.Frames[frame].AddVoxels(bodyVoxels);
                grid.Frames[frame].AddVoxels(faceVoxels);   
            }

            var larmIdle = leftArm.Frames[0].GetVoxelArray(lArmOffset, profileColors);
            var rarmIdle = rightArm.Frames[0].GetVoxelArray(rArmOffSet, profileColors, out ushort rarm_jointResult, out IntVector3 rarm_jointPos);
            rarm_jointPos.Z -= 1;

            for (int frame = 0; frame < 2; frame++)
            {
                grid.Frames[frame].AddVoxels(larmIdle);
                grid.Frames[frame].AddVoxels(rarmIdle);

                sword.addToGrid(grid.Frames[frame], ref rarm_jointPos, false);
                //grid.Frames[frame].AddVoxels(swordForward, rarm_jointPos - swordForward_jointPos);
            }
            for (int frame = 2; frame < FrameCount; frame++)
            {
                grid.Frames[frame].AddVoxels(leftArm.Frames[frame - 1].GetVoxelArray(lArmOffset, profileColors));
                grid.Frames[frame].AddVoxels(rightArm.Frames[frame - 1].GetVoxelArray(rArmOffSet, profileColors, out rarm_jointResult, out rarm_jointPos));
                rarm_jointPos.Z -= 1;
                               
                sword.addToGrid(grid.Frames[frame], ref rarm_jointPos, frame == 2);
                //    //grid.Frames[frame].AddVoxels(swordAttack, rarm_jointPos - swordAttack_jointPos);
                //}
                //else
                //{
                //    sword.addToGrid(grid.Frames[frame], ref rarm_jointPos, false);
                //    //grid.Frames[frame].AddVoxels(swordForward, rarm_jointPos - swordForward_jointPos);
                //}
            }

            var centerAdjust = grid.Frames[0].BottomCenterAdj();
            buildVerticeDataHD_ColorNormal(grid.Frames, centerAdjust);
            Graphics.VoxelModel model = modelFromVertices();

            return model;
        }
    }
}
