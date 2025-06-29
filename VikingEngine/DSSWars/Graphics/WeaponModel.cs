using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;

using VikingEngine.Graphics;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Map;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars
{
    struct VoxelJoint
    {
        public static readonly VoxelJoint Empty = new VoxelJoint(IntVector3.NegativeOne, BlockHD.EmptyBlock);

        public IntVector3 pos;
        public ushort value;

        public VoxelJoint(IntVector3 pos, ushort value)
        {
            this.pos = pos;
            this.value = value;
        }
    }

    class WeaponModel
    {
        public const int IdleFrame = 0;
        public const int AttackFrame = 1;
        public const int MoveFrame = 2;

        List<VoxelHD> idle, attack, move;
        public VoxelJoint idle_jointPos, attack_jointPos, move_jointPos;
        //ushort idleJoint, attack

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

                    idle = grids[IdleFrame].GetVoxelArray(out idle_jointPos);
                    if (grids.Count > AttackFrame)
                    {
                        attack = grids[AttackFrame].GetVoxelArray(out attack_jointPos);
                    }
                    if (grids.Count > MoveFrame)
                    {
                        move = grids[MoveFrame].GetVoxelArray(out move_jointPos);
                    }

                    //if (modelName == VoxelModelName.modweapon_shortbow)
                    //{ 
                    //    lib.DoNothing();
                    //}
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
            });

        }

        public void addToGrid(VoxelObjGridDataHD grid, IntVector3 armJointPos, int state)
        {
            switch (state)
            {
                default:
                    grid.SafeAddVoxels(idle, armJointPos - idle_jointPos.pos);
                    break;
                case AttackFrame:
                    grid.SafeAddVoxels(attack, armJointPos - attack_jointPos.pos);
                    break;
                case MoveFrame:
                    grid.SafeAddVoxels(move, armJointPos - move_jointPos.pos);
                    break;

            }
        }
    }
}
