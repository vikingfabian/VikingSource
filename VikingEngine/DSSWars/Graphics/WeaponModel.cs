using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    abstract class AbsWeaponModel
    {
        protected List<VoxelHD> idle;
        public VoxelJoint idle_jointPos;

        protected List<VoxelHD> cloneVoxels(Dictionary<ushort, ushort> findReplace, List<VoxelHD> voxels)
        {
            if (voxels != null)
            {
                List<VoxelHD> result = new List<VoxelHD>(voxels.Count);
                foreach (VoxelHD v in voxels)
                {
                    VoxelHD copy = v;
                    if (findReplace.TryGetValue(v.Material, out ushort toColor))
                    {
                        copy.Material = toColor;
                    }
                    result.Add(copy);
                }

                return result;
            }
            return null;
        }

        abstract public void addToGrid(VoxelObjGridDataHD grid, IntVector3 armJointPos, int state);
    }

    class ShieldModel : AbsWeaponModel
    {
        public ShieldModel()
        {
        }
        public ShieldModel(VoxelModelName modelName, int frame)
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

                    idle = grids[frame].GetVoxelArray(out idle_jointPos);
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
            });

        }

        override public void addToGrid(VoxelObjGridDataHD grid, IntVector3 armJointPos, int state)
        {
            grid.SafeAddVoxels(idle, armJointPos - idle_jointPos.pos);
        }

        public ShieldModel recolor(Dictionary<ushort, ushort> findReplace)
        {
            ShieldModel clone = new ShieldModel();

            clone.idle = cloneVoxels(findReplace, idle);
            clone.idle_jointPos = idle_jointPos;
           
            return clone;


        }
    }

    class WeaponModel : AbsWeaponModel
    {
        public const int IdleFrame = 0;
        public const int AttackFrame = 1;
        public const int MoveFrame = 2;

        List<VoxelHD> attack, move;
        public VoxelJoint attack_jointPos, move_jointPos;
        //ushort idleJoint, attack
        public WeaponModel()
        { 
        }
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
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
            });

        }

        override public void addToGrid(VoxelObjGridDataHD grid, IntVector3 armJointPos, int state)
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

        public WeaponModel recolor(Dictionary<ushort, ushort> findReplace)
        {
            WeaponModel clone = new WeaponModel();

            clone.idle = cloneVoxels(findReplace, idle);
            clone.attack = cloneVoxels(findReplace, attack);
            clone.move = cloneVoxels(findReplace, move);

            clone.idle_jointPos = idle_jointPos;
            clone.attack_jointPos = attack_jointPos;
            clone.move_jointPos = move_jointPos;

            return clone;

            
        }
    }
}
