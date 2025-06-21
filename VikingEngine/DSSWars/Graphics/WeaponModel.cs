using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.Graphics;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Editor;
using VikingEngine.LootFest.Map;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars
{
    class WeaponModel
    {
        const int ForwardFrame = 0;
        const int AttackFrame = 1;
        
        List<VoxelHD> forward, attack;
        IntVector3 forward_jointPos, attack_jointPos;

        public WeaponModel(VoxelModelName modelName)
        {
            DataStream.FilePath path = VoxelObjDataLoader.ContentPath(modelName);
            byte[] data = DataStream.FileToDiskManager.Read(path);
            
            Task.Run(() =>
            {
                try
                {
                    System.IO.MemoryStream s = new System.IO.MemoryStream(data);
                    System.IO.BinaryReader r = new System.IO.BinaryReader(s);

                    var grids = VoxelObjDataLoader.LoadVoxelObjGridHD(r);

                    forward = grids[ForwardFrame].GetVoxelArray(out ushort swordForward_jointResult, out forward_jointPos);
                    attack = grids[AttackFrame].GetVoxelArray(out ushort swordAttack_jointResult, out attack_jointPos);
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
            });
            
        }

        public void addToGrid(VoxelObjGridDataHD grid, ref IntVector3 armJointPos, bool bAttack)
        {
            if (bAttack)
            {
                grid.AddVoxels(attack, armJointPos - attack_jointPos);
            }
            else
            {
                grid.AddVoxels(forward, armJointPos - forward_jointPos);
            }
        }
    }
}
