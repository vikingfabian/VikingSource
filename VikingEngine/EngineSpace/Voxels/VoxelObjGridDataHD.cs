using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Map.HDvoxel;
using Microsoft.Xna.Framework;

namespace VikingEngine.Voxels
{
    class VoxelObjGridDataHD
    {
        public int Rotation;
        public IntVector3 Size
        {
            get
            {
                if (MaterialGrid == null)
                    return IntVector3.Zero;
                return new IntVector3(MaterialGrid.GetLength(0), MaterialGrid.GetLength(1), MaterialGrid.GetLength(2));
            }
        }
        public IntVector3 Limits
        {
            get
            {
                return Size - 1;
            }
        }

        public ushort[, ,] MaterialGrid;
        public List<ushort> special = null;
        
        public VoxelObjGridDataHD(IntVector3 size, List<VoxelHD> voxels)
        {
            newGrid(size);
            SafeAddVoxels(voxels);
        }

        public VoxelObjGridDataHD(IntVector3 size, List<VoxelHD> voxels, IntVector3 offset)
        {
            newGrid(size);
            VoxelHD v;

            for (int i = 0; i < voxels.Count; ++i)
            {
                v = voxels[i];
                MaterialGrid[v.Position.X + offset.X, v.Position.Y + offset.Y, v.Position.Z + offset.Z] = v.Material;
            }
        }

        public void ReplaceMaterial(ushort from1, ushort to1, IntervalIntV3 inVolume)
        {
            if (from1 == to1) return;

            IntVector3 pos = IntVector3.Zero;
            IntVector3 sz = Size;

            for (pos.Y = inVolume.Min.Y; pos.Y <= inVolume.Max.Y; ++pos.Y)
            {
                for (pos.Z = inVolume.Min.Z; pos.Z <= inVolume.Max.Z; ++pos.Z)
                {
                    for (pos.X = inVolume.Min.X; pos.X <= inVolume.Max.X; ++pos.X)
                    {
                        if (MaterialGrid[pos.X, pos.Y, pos.Z] == from1)
                            MaterialGrid[pos.X, pos.Y, pos.Z] = to1;
                    }
                }
            }
        }

        public void ReplaceMaterial(ushort from1, ushort to1)
        {
            
            IntVector3 pos = IntVector3.Zero;
            IntVector3 sz = Size;

            for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
            {
                for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        if (MaterialGrid[pos.X, pos.Y, pos.Z] == from1)
                            MaterialGrid[pos.X, pos.Y, pos.Z] = to1;
                    }
                }
            }
        }
        public void ReplaceMaterial(List<BlockHDPair> findReplace)
        {
            if (findReplace.Count > 0)
            {
                IntVector3 pos = IntVector3.Zero;
                IntVector3 sz = Size;

                for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                {
                    for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                    {
                        for (pos.X = 0; pos.X < sz.X; ++pos.X)
                        {
                            foreach (BlockHDPair fromTo in findReplace)
                            {
                                if (MaterialGrid[pos.X, pos.Y, pos.Z] == fromTo.block1)
                                {
                                    MaterialGrid[pos.X, pos.Y, pos.Z] = fromTo.block2;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void ReplaceMaterial(List<TwoAppearanceMaterials> findReplace)
        {
            if (findReplace.Count > 0)
            {
                IntVector3 pos = IntVector3.Zero;
                IntVector3 sz = Size;
                ushort replace;

                for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                {
                    for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                    {
                        for (pos.X = 0; pos.X < sz.X; ++pos.X)
                        {
                            foreach (TwoAppearanceMaterials fromTo in findReplace)
                            {
                                if (fromTo.mat1.replaceMaterial(MaterialGrid[pos.X, pos.Y, pos.Z], fromTo.mat2, out replace))
                                {
                                    MaterialGrid[pos.X, pos.Y, pos.Z] = replace;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Combine(List<BlockHDPair> findReplace, List<VoxelObjGridDataHD> addItemsData)
        {
            IntVector3 pos = IntVector3.Zero;
            IntVector3 sz = Size;

            for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
            {
                for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        foreach (BlockHDPair fromTo in findReplace)
                        {
                            if (MaterialGrid[pos.X, pos.Y, pos.Z] == fromTo.block1)
                            {
                                MaterialGrid[pos.X, pos.Y, pos.Z] = fromTo.block2;
                                break;
                            }
                        }

                        if (addItemsData != null)
                        {
                            foreach (VoxelObjGridDataHD grid in addItemsData)
                            {
                                if (grid.InBounds(pos) && grid.MaterialGrid[pos.X, pos.Y, pos.Z] != BlockHD.EmptyBlock)
                                {
                                    MaterialGrid[pos.X, pos.Y, pos.Z] = grid.MaterialGrid[pos.X, pos.Y, pos.Z];
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Combine(List<TwoAppearanceMaterials> findReplace, List<VoxelObjGridDataHD> addItemsData)
        {
            IntVector3 pos = IntVector3.Zero;
            IntVector3 sz = Size;
            ushort replace;

            for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
            {
                for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        foreach (TwoAppearanceMaterials fromTo in findReplace)
                        {
                            if (fromTo.mat1.replaceMaterial(MaterialGrid[pos.X, pos.Y, pos.Z], fromTo.mat2, out replace))//MaterialGrid[pos.X, pos.Y, pos.Z] == fromTo.block1)
                            {
                                MaterialGrid[pos.X, pos.Y, pos.Z] = replace;//fromTo.block2;
                                break;
                            }
                        }

                        if (addItemsData != null)
                        {
                            foreach (VoxelObjGridDataHD grid in addItemsData)
                            {
                                if (grid.InBounds(pos) && grid.MaterialGrid[pos.X, pos.Y, pos.Z] != BlockHD.EmptyBlock)
                                {
                                    MaterialGrid[pos.X, pos.Y, pos.Z] = grid.MaterialGrid[pos.X, pos.Y, pos.Z];
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Resize(IntVector3 newSize)
        {
            if (newSize != Size)
            {
                IntVector3 copyArea = new IntVector3
                    (
                    lib.SmallestValue(newSize.X, Size.X),
                    lib.SmallestValue(newSize.Y, Size.Y),
                    lib.SmallestValue(newSize.Z, Size.Z));

                ushort[, ,] old = MaterialGrid;
                newGrid(newSize);

                IntVector3 pos = IntVector3.Zero;
                for (pos.Y = 0; pos.Y < copyArea.Y; ++pos.Y)
                {
                    for (pos.Z = 0; pos.Z < copyArea.Z; ++pos.Z)
                    {
                        for (pos.X = 0; pos.X < copyArea.X; ++pos.X)
                        {
                            Set(pos, old[pos.X, pos.Y, pos.Z]);
                        }
                    }
                }
            }
        }

        void newGrid(IntVector3 size)
        {
            MaterialGrid = new ushort[size.X, size.Y, size.Z];
        }

        public VoxelObjGridDataHD(ushort[, ,] materialGrid)
        {
            MaterialGrid = materialGrid;
        }
        public VoxelObjGridDataHD()
        { }
        public VoxelObjGridDataHD(IntVector3 size)
        {
            //IntVector3 size = limits + 1;
            MaterialGrid = new ushort[size.X, size.Y, size.Z];
        }
        public VoxelObjGridDataHD Clone()
        {
            ushort[, ,] cloneGrid = (ushort[, ,])MaterialGrid.Clone();
            return new VoxelObjGridDataHD(cloneGrid);
        }

        public ushort Get(IntVector3 pos)
        {
           return MaterialGrid[pos.X, pos.Y, pos.Z];
            //return 0;
        }
        public void Set(IntVector3 pos, ushort value)
        {
             MaterialGrid[pos.X, pos.Y, pos.Z] = value;
        }
        public void SetSafe(IntVector3 pos, ushort value)
        {
            IntVector3 sz = Size;
            if (pos.X >= 0 && pos.X < sz.X &&
                pos.Y >= 0 && pos.Y < sz.Y &&
                pos.Z >= 0 && pos.Z < sz.Z)
            {
                MaterialGrid[pos.X, pos.Y, pos.Z] = value;
            }
        }
        public ushort GetSafe(IntVector3 pos)
        {
            if (InBounds(pos))
            { return MaterialGrid[pos.X, pos.Y, pos.Z]; }
            else
            { return BlockHD.EmptyBlock; }
        }

        public bool InBounds(IntVector3 pos)
        {
            return 0 <= pos.X && pos.X < MaterialGrid.GetLength(0) &&
                0 <= pos.Y && pos.Y < MaterialGrid.GetLength(1) &&
                0 <= pos.Z && pos.Z < MaterialGrid.GetLength(2);
        }

        public Vector3 BottomCenterAdj()
        {
            return new Vector3(
                -(Size.X - 1) * PublicConstants.Half,
                0,
                -(Size.Z - 1) * PublicConstants.Half);
        }
        public Vector3 CenterAdj()
        {
            return -(Size.Vec - Vector3.One) * PublicConstants.Half;
        }

        public void AddVoxels(List<VoxelHD> voxels)
        {
            foreach (var v in voxels)
            {
                MaterialGrid[v.Position.X, v.Position.Y, v.Position.Z] = v.Material;
            }
        }
        public void SafeAddVoxels(List<VoxelHD> voxels)
        {
            foreach (var v in voxels)
            {
                SetSafe(v.Position, v.Material);
            }
        }

        public List<VoxelHD> GetVoxelArray()
        {
            List<VoxelHD> result = new List<VoxelHD>();
            if (MaterialGrid != null)
            {
                IntVector3 pos = IntVector3.Zero;
                IntVector3 sz = Size;
                //int index = 0;
                for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                {
                    for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                    {
                        for (pos.X = 0; pos.X < sz.X; ++pos.X)
                        {
                            if (MaterialGrid[pos.X, pos.Y, pos.Z] != BlockHD.EmptyBlock)
                            {
                                result.Add(new VoxelHD(pos, MaterialGrid[pos.X, pos.Y, pos.Z]));
                            }
                        }
                    }
                }
            }
            return result;
        }
        
        public List<VoxelHD> GetVoxelArrayCentered() //only centers X and Z
        {
            IntVector3 limits = this.Limits;
            IntVector3 adjustPos = new IntVector3(limits.X / PublicConstants.Twice, 0, limits.Z / PublicConstants.Twice);
            IntVector3 voxelPos = IntVector3.Zero;
            List<VoxelHD> result = new List<VoxelHD>();
            IntVector3 pos = IntVector3.Zero;
            // Unused var
            //int index = 0;
            for (pos.Z = 0; pos.Z < limits.Z; ++pos.Z)
            {
                voxelPos.Z = pos.Z + adjustPos.Z;
                for (pos.Y = 0; pos.Y < limits.Y; ++pos.Y)
                {
                    voxelPos.Y = pos.Y;
                    for (pos.X = 0; pos.X < limits.X; ++pos.X)
                    {
                        if (MaterialGrid[pos.X, pos.Y, pos.Z] != BlockHD.EmptyBlock)
                        {
                            voxelPos.X = pos.X + adjustPos.X;
                            result.Add(new VoxelHD(voxelPos, MaterialGrid[pos.X, pos.Y, pos.Z]));
                        }
                    }
                }
            }
            return result;
        }
        

        byte currentValueReplace(byte material, List<byte> find, List<byte> replace)
        {
            for (int i = 0; i < find.Count; i++)
            {
                if (material == find[i])
                    return replace[i];
            }
            return material;
        }

        
        public VoxelObjGridDataHD Rotate(int rotationSteps, bool replaceOriginalData)
        {
            return this.Rotate(rotationSteps, new IntervalIntV3(IntVector3.Zero, this.Limits), replaceOriginalData);
        }

        public VoxelObjGridDataHD Rotate(int rotationSteps, IntVector3 limits, bool replaceOriginalData)
        {
            return this.Rotate(rotationSteps, new IntervalIntV3(IntVector3.Zero, limits), replaceOriginalData);
        }
        /// <param name="rotationSteps">a value between 1-3, representing 90degree steps</param>
        /// <param name="limits"></param>
        public VoxelObjGridDataHD Rotate(int rotationSteps, IntervalIntV3 limits, bool replaceOriginalData)
        {
            VoxelObjGridDataHD clone = null;

            Rotation += rotationSteps;
            switch (rotationSteps)
            {
                case 1:
                    limits = new IntervalIntV3(IntVector3.Zero, new IntVector3(limits.AddZ, limits.AddY, limits.AddX));
                    clone = SwapXZ(limits);
                    clone = clone.FlipDir(Dimensions.X, limits, false);
                    break;
                case 2://180D
                    clone = FlipDir(Dimensions.X, limits, false);
                    clone = clone.FlipDir(Dimensions.Z, limits, false);
                    break;
                case 3:
                    limits = new IntervalIntV3(IntVector3.Zero, new IntVector3(limits.AddZ, limits.AddY, limits.AddX));
                    clone = SwapXZ(limits);
                    clone = clone.FlipDir(Dimensions.Z, limits, false);
                    break;
            }

            if (replaceOriginalData)
            {
                MaterialGrid = clone.MaterialGrid;
            }
            return clone;
        }
        VoxelObjGridDataHD SwapXZ(IntervalIntV3 limits)
        {
            VoxelObjGridDataHD clone = new VoxelObjGridDataHD(limits.Add + 1);//this.Clone();
            //newGrid(limits.Add + 1);
            IntVector3 pos = IntVector3.Zero;
            IntVector3 invPos = IntVector3.Zero;

            for (pos.Z = 0; pos.Z <= limits.AddZ; ++pos.Z)
            {
                invPos.X = pos.Z;

                for (pos.X = 0; pos.X <= limits.AddX; ++pos.X)
                {
                    invPos.Z = pos.X;

                    for (pos.Y = 0; pos.Y <= limits.AddY; ++pos.Y)
                    {
                        invPos.Y = pos.Y;

                        //Set(pos, clone.Get(invPos));
                        clone.Set(pos, Get(invPos));
                    }
                }
            }

            return clone;
        }
        public VoxelObjGridDataHD FlipDir(Dimensions dimention, IntervalIntV3 limits, bool replaceOriginalData)
        {
            VoxelObjGridDataHD clone = new VoxelObjGridDataHD(Size);
            IntVector3 pos = IntVector3.Zero;
            IntVector3 fromPos = IntVector3.Zero;
            for (pos.Z = limits.Min.Z; pos.Z <= limits.Max.Z; ++pos.Z)
            {
                if (dimention == Dimensions.Z)
                    fromPos.Z = limits.Max.Z - (pos.Z - limits.Min.Z);
                else
                    fromPos.Z = pos.Z;
                for (pos.Y = limits.Min.Y; pos.Y <= limits.Max.Y; ++pos.Y)
                {
                    if (dimention == Dimensions.Y)
                        fromPos.Y = limits.Max.Y - (pos.Y - limits.Min.Y);
                    else
                        fromPos.Y = pos.Y;

                    for (pos.X = limits.Min.X; pos.X <= limits.Max.X; ++pos.X)
                    {
                        if (dimention == Dimensions.X)
                            fromPos.X = limits.Max.X - (pos.X - limits.Min.X);
                        else
                            fromPos.X = pos.X;

                        //Set(pos, clone.Get(fromPos));
                        clone.Set(pos, Get(fromPos));
                    }
                }
            }

            if (replaceOriginalData)
            {
                MaterialGrid = clone.MaterialGrid;
            }
            return clone;
        }
        public void Move(IntVector3 dir, IntervalIntV3 limits)
        {
            VoxelObjGridDataHD clone = this.Clone();
            newGrid(limits.Add + 1);
            IntVector3 pos = IntVector3.Zero;
            IntVector3 toPos = IntVector3.Zero;
            for (pos.Z = limits.Min.Z; pos.Z <= limits.Max.Z; ++pos.Z)
            {
                toPos.Z = pos.Z + dir.Z;
                for (pos.Y = limits.Min.Y; pos.Y <= limits.Max.Y; ++pos.Y)
                {
                    toPos.Y = pos.Y + dir.Y;
                    for (pos.X = limits.Min.X; pos.X <= limits.Max.X; ++pos.X)
                    {
                        toPos.X = pos.X + dir.X;
                        SetSafe(toPos, clone.Get(pos));
                    }
                }
            }
        }
        public void BuildOnTerrain(LootFest.Map.WorldPosition origo)
        {
            if (!origo.CorrectPos)
                return;

            IntVector3 pos = IntVector3.Zero;
            IntVector3 size = Size;

            ushort material;
            for (pos.Z = 0; pos.Z < size.Z; ++pos.Z)
            {
                for (pos.X = 0; pos.X < size.X; ++pos.X)
                {
                    pos.Y = 0;
                    material = Get(pos);

                    if (material != BlockHD.EmptyBlock)
                    {
                        VikingEngine.LootFest.Map.WorldPosition wp = origo.GetNeighborPos(pos);
                        wp.SetFromHeightMap(0);
                        ushort topMaterial = wp.GetBlock();

                        while (wp.Y > origo.Y)
                        {
                            wp.Screen.Set(wp, LootFest.Map.HDvoxel.BlockHD.EmptyBlock);
                            --wp.Y;
                        }

                        while (wp.Y <= origo.Y)
                        {
                            wp.SetBlock(topMaterial);//.Screen.voxels.Set(wp, new LootFest.Map.HDvoxel.BlockHD(topMaterial));
                            ++wp.Y;
                        }
                    }

                    for (pos.Y = 0; pos.Y < size.Y; ++pos.Y)
                    {
                        material = Get(pos);
                        if (material != BlockHD.EmptyBlock)
                        {
                            if (BlockHD.ToMaterialValue(material) == BlockHD.AntiMaterial)
                            {
                                material = BlockHD.EmptyBlock;
                            }

                            origo.GetNeighborPos(pos).SetBlock_IfOpen(material);//LootFest.LfRef.chunks.SetIfOpen(origo.GetNeighborPos(pos), material);
                        }
                    }
                }
            }   
        }

        /// <summary>
        /// The position of the top-most block
        /// </summary>
        /// <returns>Y index</returns>
        public int TopBlock()
        {
            IntVector3 pos = IntVector3.Zero;
            IntVector3 size = Size;

            for (pos.Y = size.Y - 1; pos.Y >= 1; --pos.Y)
            {
                for (pos.Z = 0; pos.Z < size.Z; ++pos.Z)
                {
                    for (pos.X = 0; pos.X < size.X; ++pos.X)
                    {
                        if (Get(pos) != BlockHD.EmptyBlock)
                        {
                            return pos.Y;
                        }
                    }
                }
            }

            return 0;
        }

        public void Merge(VoxelObjGridDataHD other, bool keepOldGridSize, bool newBlocksReplaceOld, IntVector3 offset)
        {
            IntVector3 loopSize;
            IntVector3 mySz = Size;
            IntVector3 otherSz = other.Size;

            if (keepOldGridSize)
            {
                loopSize = new IntVector3(
                    lib.SmallestValue(mySz.X, otherSz.X),
                    lib.SmallestValue(mySz.Y, otherSz.Y),
                    lib.SmallestValue(mySz.Z, otherSz.Z));
            }
            else
            {
                
                loopSize = new IntVector3(
                    lib.LargestValue(mySz.X, otherSz.X),
                    lib.LargestValue(mySz.Y, otherSz.Y),
                    lib.LargestValue(mySz.Z, otherSz.Z));
                Resize(loopSize);
            }

            IntVector3 pos = IntVector3.Zero;
            ushort oldMaterial;
            ushort newMaterial;
            for (pos.Y = 0; pos.Y < loopSize.Y; ++pos.Y)
            {
                for (pos.Z = 0; pos.Z < loopSize.Z; ++pos.Z)
                {
                    for (pos.X = 0; pos.X < loopSize.X; ++pos.X)
                    {
                        oldMaterial = Get(pos);
                        newMaterial = other.Get(pos);

                        if (newBlocksReplaceOld)
                        {
                            if (newMaterial != BlockHD.EmptyBlock)
                            {
                                SetSafe(pos + offset, newMaterial);
                            }
                        }
                        else
                        {
                            if (oldMaterial == BlockHD.EmptyBlock)
                            {
                                SetSafe(pos + offset, newMaterial);
                            }
                        }
                        
                    }
                }
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            const byte Version = 1;

            w.Write(Version);
            Size.WriteByteStream(w);

            VoxelLib.CompressGridHD(MaterialGrid, w);
        }
        public void read(System.IO.BinaryReader r)
        {
            byte version = r.ReadByte();
            if (version > 0)
            {
                IntVector3 sz = IntVector3.FromByteSzStream(r);

                Resize(sz);
                VoxelLib.DeCompressGridHD(MaterialGrid, r);
            }
        }
        
    }
}

