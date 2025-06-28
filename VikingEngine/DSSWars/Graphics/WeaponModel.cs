using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;

using VikingEngine.Graphics;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Map;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars
{
    class WeaponModel
    {
        const int IdleFrame = 0;
        const int AttackFrame = 1;
        const int MoveFrame = 2;

        List<VoxelHD> idle, attack, move;
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

                    idle = grids[IdleFrame].GetVoxelArray(out ushort swordForward_jointResult, out forward_jointPos);
                    if (grids.Count > AttackFrame)
                    {
                        attack = grids[AttackFrame].GetVoxelArray(out ushort swordAttack_jointResult, out attack_jointPos);
                    }
                    if (grids.Count > MoveFrame)
                    {
                        move = grids[MoveFrame].GetVoxelArray(out ushort swordAttack_jointResult, out attack_jointPos);
                    }
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
                grid.AddVoxels(idle, armJointPos - forward_jointPos);
            }
        }
    }
}
