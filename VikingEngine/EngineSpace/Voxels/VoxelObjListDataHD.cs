using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.Voxels
{
    class VoxelObjListDataHD
    {
        public IntVector3 Size;
        public List<VoxelHD> Voxels;
        public VoxelObjListDataHD(VoxelObjGridDataHD grid)
            :this(grid.GetVoxelArray())
        {
            Size = grid.Limits + 1;
        }
        public VoxelObjListDataHD(List<VoxelHD> voxels)
        {
            //System.Diagnostics.Debug.WriteLine("VoxelObjListData Init");
            Voxels = voxels;
        }

        //public VoxelObjListData(byte[] compressedData)
        //{
        //    //List<byte> list = new List<byte>(compressedData);
        //    VoxelObjGridData grid = VoxelObjGridData.FromByteArray(compressedData);
        //    //grid.FromCompressedData(list);
        //    Voxels = grid.GetVoxelArray();
        //    Size = grid.Limits + 1;
        //}

        public IntervalIntV3 getMinMax()
        {
            IntervalIntV3 blockRange = IntervalIntV3.Zero;

            if (Voxels.Count > 0)
            {
                blockRange.Min = Voxels[0].Position;
                blockRange.Max = Voxels[0].Position;

                for (int i = 1; i < Voxels.Count; ++i)
                {
                    IntVector3 pos = Voxels[i].Position;

                    if (pos.X < blockRange.Min.X)
                    {
                        blockRange.Min.X = pos.X;
                    }
                    else if (pos.X > blockRange.Max.X)
                    {
                        blockRange.Max.X = pos.X;
                    }

                    if (pos.Y < blockRange.Min.Y)
                    {
                        blockRange.Min.Y = pos.Y;
                    }
                    else if (pos.Y > blockRange.Max.Y)
                    {
                        blockRange.Max.Y = pos.Y;
                    }

                    if (pos.Z < blockRange.Min.Z)
                    {
                        blockRange.Min.Z = pos.Z;
                    }
                    else if (pos.Z > blockRange.Max.Z)
                    {
                        blockRange.Max.Z = pos.Z;
                    }
                }
            }

            return blockRange;
        }
        


        public bool IsNull
        { get { return Voxels == null; } }
        public void Remove(IntVector3 pos)
        {
            for (int i = 0; i < Voxels.Count; ++i)
            {
                if (Voxels[i].Position.Equals(pos))
                {
                    Voxels.Remove(Voxels[i]);
                    break;
                }
            }
        }
        public ushort GetValue(IntVector3 pos)
        {
            for (int i = 0; i < Voxels.Count; ++i)
            {
                if (Voxels[i].Position.Equals(pos))
                {
                    return Voxels[i].Material;
                }
            }
            return BlockHD.EmptyBlock;
        }
        public int selectedVoxel(IntVector3 pos)
        {
            for (int i = 0; i < Voxels.Count; ++i)
            {
                if (Voxels[i].Position == pos)
                {
                    return i;
                }
            }
            return -1;
        }
        public void ReplaceMaterial(ushort from1, ushort to1)
        {
            //byte f1 = from1;
            //byte t1 = to1;

            for (int i = 0; i < Voxels.Count; ++i)
            {
                if (Voxels[i].Material == from1)
                {
                    Voxels[i] = Voxels[i].ChangeMaterial(to1);
                }
            }
        }
        public void ReplaceMaterial(List<BlockHDPair> findReplace)
        {
            if (findReplace.Count > 0)
            {
                for (int i = 0; i < Voxels.Count; ++i)
                {
                    for (int f = 0; f < findReplace.Count; f++)
                    {
                        if (Voxels[i].Material == findReplace[f].block1)
                        {
                            Voxels[i] = Voxels[i].ChangeMaterial(findReplace[f].block2);
                            break;
                        }
                    }
                }
            }
        }
        public void RemoveVoxelsOutsideLimits(IntervalIntV3 limits)
        {
            for (int i = 0; i < Voxels.Count; ++i)
            {
                if (!limits.pointInBounds(Voxels[i].Position))
                {
                    Voxels.Remove(Voxels[i]);
                    i--;
                }
            }
        }

        public void Rotate(int rotationSteps, IntVector3 limits)
        {
            this.Rotate(rotationSteps, new IntervalIntV3(IntVector3.Zero, limits));
        }
        /// <param name="rotationSteps">a value between 1-3, representing 90degree steps</param>
        /// <param name="limits"></param>
        public void Rotate(int rotationSteps, IntervalIntV3 limits)
        {
            switch (rotationSteps)
            {
                case 1:
                    SwapXZ(limits);
                    FlipDir(Dimensions.X, limits);
                    break;
                case 2://180D
                    FlipDir(Dimensions.X, limits);
                    FlipDir(Dimensions.Z, limits);
                    break;
                case 3:
                    SwapXZ(limits);
                    FlipDir(Dimensions.Z, limits);
                    break;
            }
        }

        public VoxelObjListDataHD(VoxelObjGridDataHD grid, int rotationSteps, IntVector3 offset)
        {
            IntVector3 pos = IntVector3.Zero;
            IntVector3 sz = grid.Size;
            IntVector3 max = grid.Size - 1;

            Voxels = new List<VoxelHD>((sz.X * sz.Y * sz.Z) / 4);

            //int index = 0;
            for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
            {
                for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        if (grid.MaterialGrid[pos.X, pos.Y, pos.Z] != BlockHD.EmptyBlock)
                        {

                            Voxels.Add(new VoxelHD(pos + offset, grid.MaterialGrid[pos.X, pos.Y, pos.Z]));
                        }
                    }
                }
            }

            Rotate(rotationSteps, new IntervalIntV3(offset, offset + max)); 
        }

        public void Combine(VoxelObjListDataHD otherOverriding)
        {
            List<VoxelHD> add = new List<VoxelHD>();
            for (int o = 0; o < otherOverriding.Voxels.Count; o++)
            {
                bool found = false;
                for (int i = 0; i < Voxels.Count; ++i)
                {
                    if (otherOverriding.Voxels[o].Position.Equals(Voxels[i].Position))
                    {
                        found = true;
                        Voxels[i] = otherOverriding.Voxels[o];
                        break;
                    }

                }
                if (!found)
                {
                    add.Add(otherOverriding.Voxels[o]);
                }
            }

            this.Voxels.AddRange(add);
        }
        //public bool[] ContainMaterials()
        //{
        //    bool[] result = new bool[PublicConstants.ByteSize];
        //    foreach (VoxelHD v in Voxels)
        //    {
        //        result[v.Material] = true;
        //    }
        //    return result;
        //}
        public VoxelObjListDataHD Clone()
        {
            var clone = new List<VoxelHD>(Voxels);
           // clone.InsertRange(0, Voxels);
            return new VoxelObjListDataHD(clone);
        }
        void SwapXZ(IntervalIntV3 limits)
        {
            var newVoxels = new List<VoxelHD>(Voxels.Count);
            foreach (var v in Voxels)
            {
                VoxelHD clone = v;
                clone.Position.X = (v.Position.Z - limits.Min.Z) +limits.Min.X;
                clone.Position.Z = (v.Position.X - limits.Min.X) + limits.Min.Z;// v.Position.X;
                newVoxels.Add(clone);
            }
            Voxels = newVoxels;
        }
        void SwapZY(IntervalIntV3 limits)
        {
            var newVoxels = new List<VoxelHD>(Voxels.Count);
            foreach (var v in Voxels)
            {
                VoxelHD clone = v;
                clone.Position.Z = (v.Position.Y - limits.Min.Y) + limits.Min.Z;
                clone.Position.Y = (v.Position.Z - limits.Min.Z) + limits.Min.Y;
                newVoxels.Add(clone);
            }
            Voxels = newVoxels;
        }
        public void CheckBounds(IntervalIntV3 limits)
        {
            for (int i = Voxels.Count -1; i >= 0; --i)
            {
                if (!limits.pointInBounds(Voxels[i].Position))
                {
                    Voxels.RemoveAt(i);
                }
            }
        }

        public void Move(IntVector3 dir)
        {
            for (int i = Voxels.Count -1; i >= 0; --i)
            {
                VoxelHD v = Voxels[i];
                v.Position.Add(dir);
                Voxels[i] = v;
            }
        }

        public void Move(IntVector3 dir, IntervalIntV3 limits)
        {
            for (int i = Voxels.Count -1; i >= 0; --i)
            {
                VoxelHD v = Voxels[i];
                v.Position.Add(dir);
                //v.Position = limits.SetBounds(v.Position, true);
                if (!limits.pointInBounds(v.Position))
                {
                    Voxels.RemoveAt(i);
                }
                else
                    Voxels[i] = v;
            }
        }
        public void MoveWithRollOver(IntVector3 dir, IntervalIntV3 limits)
        {
            for (int i = 0; i < Voxels.Count; ++i)
            {
                VoxelHD v = Voxels[i];
                v.Position.Add(dir);
                v.Position = limits.keepValueInMyBounds(v.Position, true);
                Voxels[i] = v;
            }
        }
        public void FlipDir(Dimensions dimention, IntervalIntV3 area)
        {
            Range limits;
            //List<Voxel> newVoxels = new List<Voxel>();
            switch (dimention)
            {
                case Dimensions.X:
                    limits = new Range(area.Min.X, area.Max.X);
                    for (int i = 0; i < Voxels.Count; ++i)
                    {
                        VoxelHD clone = Voxels[i];
                        clone.Position.X = limits.Max - (clone.Position.X - limits.Min);
                        Voxels[i] = clone;
                    }
                    break;
                case Dimensions.Y:
                    limits = new Range(area.Min.Y, area.Max.Y);
                    for (int i = 0; i < Voxels.Count; ++i)
                    {
                        VoxelHD clone = Voxels[i];
                        clone.Position.Y = limits.Max - (clone.Position.Y - limits.Min);
                        Voxels[i] = clone;
                    }
                    break;
                case Dimensions.Z:
                    limits = new Range(area.Min.Z, area.Max.Z);
                    for (int i = 0; i < Voxels.Count; ++i)
                    {
                        VoxelHD clone = Voxels[i];
                        clone.Position.Z = limits.Max - (clone.Position.Z - limits.Min);
                        Voxels[i] = clone;
                    }
                    break;
            }
        }

        

        public void FlipLyingToStanding(IntervalIntV3 area)
        {
            SwapZY(area);
        }
        

        public void BuildOnTerrain(LootFest.Map.WorldPosition origo)
        {
            if (!origo.CorrectPos)
                return;
            foreach (var v in Voxels)
            {
                origo.GetNeighborPos(v.Position).SetBlock_IfOpen(v.Material);//LootFest.LfRef.chunks.SetIfOpen(origo.GetNeighborPos(v.Position), v.Material);
            }
        }

        public void BuildOnTerrainWithAnitBlocks(LootFest.Map.WorldPosition origo)
        {
            if (!origo.CorrectPos)
                return;
            foreach (var v in Voxels)
            {
                if (v.Material == BlockHD.AntiMaterial)//VoxelLib.AntiBlock)
                {
                    origo.GetNeighborPos(v.Position).SetBlock_IfOpen(BlockHD.EmptyBlock);//LootFest.LfRef.chunks.SetIfOpen(origo.GetNeighborPos(v.Position), BlockHD.Empty);
                }
                else
                {
                    origo.GetNeighborPos(v.Position).SetBlock_IfOpen(v.Material);//LootFest.LfRef.chunks.SetIfOpen(origo.GetNeighborPos(v.Position), v.Material);
                }
            }
        }

        public void BuildOnTerrain_Safe(LootFest.Map.WorldPosition origo)
        {
            if (!origo.CorrectPos)
                return;
            foreach (var v in Voxels)
            {
                origo.GetNeighborPos(v.Position).SetBlock_IfOpen(v.Material);//LootFest.LfRef.chunks.SetIfOpen(origo.GetNeighborPos(v.Position), v.Material);
            }
        }
    }
}
