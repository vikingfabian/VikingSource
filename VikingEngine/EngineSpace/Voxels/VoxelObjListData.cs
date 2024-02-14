using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Voxels
{
    class VoxelObjListData
    {
        public IntVector3 Size;
        public List<Voxel> Voxels;
        public VoxelObjListData(VoxelObjGridData grid)
            :this(grid.GetVoxelArray())
        {
            Size = grid.Limits + 1;
        }
        public VoxelObjListData(List<Voxel> voxels)
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
        public byte GetValue(IntVector3 pos)
        {
            for (int i = 0; i < Voxels.Count; ++i)
            {
                if (Voxels[i].Position.Equals(pos))
                {
                    return Voxels[i].Material;
                }
            }
            return 0;
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
        public void ReplaceMaterial(byte from1, byte to1)
        {
            byte f1 = from1;
            byte t1 = to1;

            for (int i = 0; i < Voxels.Count; ++i)
            {
                if (Voxels[i].Material == f1)
                {
                    Voxels[i] = Voxels[i].ChangeMaterial(t1);
                }
            }
        }
        public void ReplaceMaterial(List<ByteVector2> findReplace)
        {
            if (findReplace.Count > 0)
            {
                for (int i = 0; i < Voxels.Count; ++i)
                {
                    for (int f = 0; f < findReplace.Count; f++)
                    {
                        if (Voxels[i].Material == findReplace[f].X)
                        {
                            Voxels[i] = Voxels[i].ChangeMaterial(findReplace[f].Y);
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

        public VoxelObjListData(VoxelObjGridData grid, int rotationSteps, IntVector3 offset)
        {
            IntVector3 pos = IntVector3.Zero;
            IntVector3 sz = grid.Size;
            IntVector3 max = grid.Size - 1;

            Voxels = new List<Voxel>((sz.X * sz.Y * sz.Z) / 4);

            //int index = 0;
            for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
            {
                for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        if (grid.MaterialGrid[pos.X, pos.Y, pos.Z] != 0)
                        {

                            Voxels.Add(new Voxel(pos + offset, grid.MaterialGrid[pos.X, pos.Y, pos.Z]));
                        }
                    }
                }
            }

            Rotate(rotationSteps, new IntervalIntV3(offset, offset + max)); 
        }

        public void Combine(VoxelObjListData otherOverriding)
        {
            List<Voxel> add = new List<Voxel>();
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
        public bool[] ContainMaterials()
        {
            bool[] result = new bool[PublicConstants.ByteSize];
            foreach (Voxel v in Voxels)
            {
                result[v.Material] = true;
            }
            return result;
        }
        public VoxelObjListData Clone()
        {
            List<Voxel> clone = new List<Voxel>(Voxels);
           // clone.InsertRange(0, Voxels);
            return new VoxelObjListData(clone);
        }
        void SwapXZ(IntervalIntV3 limits)
        {
            List<Voxel> newVoxels = new List<Voxel>(Voxels.Count);
            foreach (Voxel v in Voxels)
            {
                Voxel clone = v;
                clone.Position.X = (v.Position.Z - limits.Min.Z) +limits.Min.X;
                clone.Position.Z = (v.Position.X - limits.Min.X) + limits.Min.Z;// v.Position.X;
                newVoxels.Add(clone);
            }
            Voxels = newVoxels;
        }
        void SwapZY(IntervalIntV3 limits)
        {
            List<Voxel> newVoxels = new List<Voxel>(Voxels.Count);
            foreach (Voxel v in Voxels)
            {
                Voxel clone = v;
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
                Voxel v = Voxels[i];
                v.Position.Add(dir);
                Voxels[i] = v;
            }
        }

        public void Move(IntVector3 dir, IntervalIntV3 limits)
        {
            for (int i = Voxels.Count -1; i >= 0; --i)
            {
                Voxel v = Voxels[i];
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
                Voxel v = Voxels[i];
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
                        Voxel clone = Voxels[i];
                        clone.Position.X = limits.Max - (clone.Position.X - limits.Min);
                        Voxels[i] = clone;
                    }
                    break;
                case Dimensions.Y:
                    limits = new Range(area.Min.Y, area.Max.Y);
                    for (int i = 0; i < Voxels.Count; ++i)
                    {
                        Voxel clone = Voxels[i];
                        clone.Position.Y = limits.Max - (clone.Position.Y - limits.Min);
                        Voxels[i] = clone;
                    }
                    break;
                case Dimensions.Z:
                    limits = new Range(area.Min.Z, area.Max.Z);
                    for (int i = 0; i < Voxels.Count; ++i)
                    {
                        Voxel clone = Voxels[i];
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
            foreach (Voxel v in Voxels)
            {
                //LootFest.LfRef.chunks.SetIfOpen(origo.GetNeighborPos(v.Position), v.Material);
            }
        }

        public void BuildOnTerrainWithAnitBlocks(LootFest.Map.WorldPosition origo)
        {
            if (!origo.CorrectPos)
                return;
            foreach (Voxel v in Voxels)
            {
                if (v.Material == VoxelLib.AntiBlock)
                {
                    //LootFest.LfRef.chunks.SetIfOpen(origo.GetNeighborPos(v.Position), byte.MinValue);
                }
                else
                {
                    //LootFest.LfRef.chunks.SetIfOpen(origo.GetNeighborPos(v.Position), v.Material);
                }
            }
        }

        public void BuildOnTerrain_Safe(LootFest.Map.WorldPosition origo)
        {
            if (!origo.CorrectPos)
                return;
            foreach (Voxel v in Voxels)
            {
                //LootFest.LfRef.chunks.SetIfOpen(origo.GetNeighborPos(v.Position), v.Material);
            }
        }
    }
}
