using Microsoft.Xna.Framework;
using System.Collections.Generic;
using VikingEngine.DSSWars;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.Voxels
{
    
        class VoxelObjGridDataHD : Grid3D_L<ushort>
        {
            public int Rotation;
            public List<ushort> special = null;

            public VoxelObjGridDataHD() : base()
            { }

            public VoxelObjGridDataHD(IntVector3 size) : base(size)
            { }

            // Helper constructor for Clone()
            public VoxelObjGridDataHD(ushort[] array, IntVector3 size) : base(array, size)
            { }

            public VoxelObjGridDataHD(IntVector3 size, List<VoxelHD> voxels) : base(size)
            {
                SafeAddVoxels(voxels);
            }

            public VoxelObjGridDataHD(IntVector3 size, List<VoxelHD> voxels, IntVector3 offset) : base(size)
            {
                VoxelHD v;
                for (int i = 0; i < voxels.Count; ++i)
                {
                    v = voxels[i];

                    IntVector3 vpos = v.Position + offset;
                    if (InBounds(vpos))
                    {
                        Set(vpos, v.Material);
                    }
                }
            }

            public VoxelObjGridDataHD(ushort[,,] materialGrid)
                : base(new IntVector3(materialGrid.GetLength(0), materialGrid.GetLength(1), materialGrid.GetLength(2)))
            {
                IntVector3 sz = Size;
                for (int z = 0; z < sz.Z; ++z)
                {
                    for (int y = 0; y < sz.Y; ++y)
                    {
                        for (int x = 0; x < sz.X; ++x)
                        {
                            Set(x, y, z, materialGrid[x, y, z]);
                        }
                    }
                }
            }

            public void ReplaceMaterial(ushort from1, ushort to1, IntervalIntV3 inVolume)
            {
                if (from1 == to1) return;

                IntVector3 pos = IntVector3.Zero;

                for (pos.Y = inVolume.Min.Y; pos.Y <= inVolume.Max.Y; ++pos.Y)
                {
                    for (pos.Z = inVolume.Min.Z; pos.Z <= inVolume.Max.Z; ++pos.Z)
                    {
                        for (pos.X = inVolume.Min.X; pos.X <= inVolume.Max.X; ++pos.X)
                        {
                            if (Get(pos) == from1)
                                Set(pos, to1);
                        }
                    }
                }
            }

            public void SetMaterialProperty(MaterialProperty toMaterial)
            {
                IntVector3 pos = IntVector3.Zero;
                IntVector3 sz = Size;

                for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                {
                    for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                    {
                        for (pos.X = 0; pos.X < sz.X; ++pos.X)
                        {
                            ushort value = Get(pos);
                            if (value != BlockHD.EmptyBlock)
                            {
                                Set(pos, BlockHD.SetMaterialProperty(value, toMaterial));
                            }
                        }
                    }
                }
            }

            public void ReplaceMaterial(Dictionary<ushort, ushort> findReplace, IntervalIntV3 inVolume)
            {
                IntVector3 pos = IntVector3.Zero;

                for (pos.Y = inVolume.Min.Y; pos.Y <= inVolume.Max.Y; ++pos.Y)
                {
                    for (pos.Z = inVolume.Min.Z; pos.Z <= inVolume.Max.Z; ++pos.Z)
                    {
                        for (pos.X = inVolume.Min.X; pos.X <= inVolume.Max.X; ++pos.X)
                        {
                            if (findReplace.TryGetValue(Get(pos), out ushort toColor))
                            {
                                Set(pos, toColor);
                            }
                        }
                    }
                }
            }

            public void BucketFill(IntVector3 pos, ushort find, ushort replace, bool continious)
            {
                if (continious)
                {
                    ReplaceMaterial(find, replace);
                }
                else
                {
                    if (!InBounds(pos)) return;
                    if (Get(pos) != find || find == replace) return;

                    Queue<IntVector3> queue = new Queue<IntVector3>();
                    queue.Enqueue(pos);

                    while (queue.Count > 0)
                    {
                        IntVector3 current = queue.Dequeue();

                        if (!InBounds(current)) continue;
                        if (Get(current) != find) continue;

                        Set(current, replace);

                        for (CubeFace face = 0; face < CubeFace.NUM; ++face)
                        {
                            IntVector3 adjacent = new IntVector3(face) + current;
                            queue.Enqueue(adjacent);
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
                            if (Get(pos) == from1)
                                Set(pos, to1);
                        }
                    }
                }
            }

            public void ReplaceMaterial(List<BlockHDPair> findReplace)
            {
                if (findReplace != null && findReplace.Count > 0)
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
                                    if (Get(pos) == fromTo.block1)
                                    {
                                        Set(pos, fromTo.block2);
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
                                    if (fromTo.mat1.replaceMaterial(Get(pos), fromTo.mat2, out replace))
                                    {
                                        Set(pos, replace);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            public void ReplaceMaterial(Dictionary<ushort, ushort> findReplace)
            {
                IntVector3 pos = IntVector3.Zero;
                IntVector3 sz = Size;

                for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                {
                    for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                    {
                        for (pos.X = 0; pos.X < sz.X; ++pos.X)
                        {
                            if (findReplace.TryGetValue(Get(pos), out ushort toColor))
                            {
                                Set(pos, toColor);
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
                                if (Get(pos) == fromTo.block1)
                                {
                                    Set(pos, fromTo.block2);
                                    break;
                                }
                            }

                            if (addItemsData != null)
                            {
                                foreach (VoxelObjGridDataHD grid in addItemsData)
                                {
                                    if (grid.InBounds(pos) && grid.Get(pos) != BlockHD.EmptyBlock)
                                    {
                                        Set(pos, grid.Get(pos));
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
                                if (fromTo.mat1.replaceMaterial(Get(pos), fromTo.mat2, out replace))
                                {
                                    Set(pos, replace);
                                    break;
                                }
                            }

                            if (addItemsData != null)
                            {
                                foreach (VoxelObjGridDataHD grid in addItemsData)
                                {
                                    if (grid.InBounds(pos) && grid.Get(pos) != BlockHD.EmptyBlock)
                                    {
                                        Set(pos, grid.Get(pos));
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
                    ReSize(newSize, null, null);
                }
            }

            public new VoxelObjGridDataHD Clone()
            {
                return new VoxelObjGridDataHD((ushort[])array.Clone(), Size);
            }

            public void SetSafe(IntVector3 pos, ushort value)
            {
                TrySet(pos, value);
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
                    Set(v.Position, v.Material);
                }
            }

            public void AddVoxels(List<VoxelHD> voxels, IntVector3 offset)
            {
                foreach (var v in voxels)
                {
                    Set(v.Position.X + offset.X, v.Position.Y + offset.Y, v.Position.Z + offset.Z, v.Material);
                }
            }

            public void SafeAddVoxels(List<VoxelHD> voxels)
            {
                foreach (var v in voxels)
                {
                    SetSafe(v.Position, v.Material);
                }
            }

            public void SafeAddVoxels(List<VoxelHD> voxels, IntVector3 offset)
            {
                foreach (var v in voxels)
                {
                    var pos = v.Position + offset;
                    SetSafe(pos, v.Material);
                }
            }

            public List<VoxelHD> GetVoxelArray()
            {
                List<VoxelHD> result = new List<VoxelHD>();
                if (array != null)
                {
                    IntVector3 pos = IntVector3.Zero;
                    IntVector3 sz = Size;

                    for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                    {
                        for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                        {
                            for (pos.X = 0; pos.X < sz.X; ++pos.X)
                            {
                                ushort value = Get(pos);
                                if (value != BlockHD.EmptyBlock)
                                {
                                    result.Add(new VoxelHD(pos, value));
                                }
                            }
                        }
                    }
                }
                return result;
            }

            public List<VoxelHD> GetVoxelArray(IntVector3 offset, Dictionary<ushort, ushort> findReplace, IntVector3 bounds)
            {
                List<VoxelHD> result = new List<VoxelHD>();
                if (array != null)
                {
                    IntVector3 pos = IntVector3.Zero;
                    IntVector3 sz = Size;

                    for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                    {
                        for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                        {
                            for (pos.X = 0; pos.X < sz.X; ++pos.X)
                            {
                                ushort value = Get(pos);
                                if (value != BlockHD.EmptyBlock)
                                {
                                    IntVector3 vpos = pos + offset;
                                    if (vpos.X >= 0 && vpos.X < bounds.X &&
                                        vpos.Y >= 0 && vpos.Y < bounds.Y &&
                                        vpos.Z >= 0 && vpos.Z < bounds.Z)
                                    {
                                        if (findReplace.TryGetValue(value, out ushort toColor))
                                        {
                                            value = toColor;
                                        }
                                        result.Add(new VoxelHD(vpos, value));
                                    }
                                }
                            }
                        }
                    }
                }
                return result;
            }

            public List<VoxelHD> GetVoxelArray(IntVector3 offset, Dictionary<ushort, ushort> findReplace, out ushort jointResult, out IntVector3 jointPos)
            {
                jointResult = BlockHD.EmptyBlock;
                jointPos = IntVector3.NegativeOne;

                List<VoxelHD> result = new List<VoxelHD>();
                if (array != null)
                {
                    IntVector3 pos = IntVector3.Zero;
                    IntVector3 sz = Size;

                    for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                    {
                        for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                        {
                            for (pos.X = 0; pos.X < sz.X; ++pos.X)
                            {
                                ushort value = Get(pos);
                                if (value != BlockHD.EmptyBlock)
                                {
                                    if (findReplace.TryGetValue(value, out ushort toColor))
                                    {
                                        result.Add(new VoxelHD(pos + offset, toColor));
                                    }
                                    else if (value == BlockHD.JointForward || value == BlockHD.JointUp)
                                    {
                                        jointResult = value;
                                        jointPos = pos + offset;
                                    }
                                    else
                                    {
                                        result.Add(new VoxelHD(pos + offset, value));
                                    }
                                }
                            }
                        }
                    }
                }
                return result;
            }

            public List<VoxelHD> GetVoxelArray(IntVector3 offset, Dictionary<ushort, ushort> findReplace, ushort findjoint, out IntVector3 jointPos)
            {
                jointPos = IntVector3.NegativeOne;

                List<VoxelHD> result = new List<VoxelHD>();
                if (array != null)
                {
                    IntVector3 pos = IntVector3.Zero;
                    IntVector3 sz = Size;

                    for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                    {
                        for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                        {
                            for (pos.X = 0; pos.X < sz.X; ++pos.X)
                            {
                                ushort value = Get(pos);
                                if (value != BlockHD.EmptyBlock)
                                {
                                    if (findReplace.TryGetValue(value, out ushort toColor))
                                    {
                                        result.Add(new VoxelHD(pos + offset, toColor));
                                    }
                                    else if (value == BlockHD.JointForward || value == BlockHD.JointUp)
                                    {
                                        if (value == findjoint)
                                        {
                                            jointPos = pos + offset;
                                        }
                                    }
                                    else
                                    {
                                        result.Add(new VoxelHD(pos + offset, value));
                                    }
                                }
                            }
                        }
                    }
                }
                return result;
            }

            public List<VoxelHD> GetVoxelArray(out VoxelJoint joint)
            {
                joint = VoxelJoint.Empty;

                List<VoxelHD> result = new List<VoxelHD>();
                if (array != null)
                {
                    IntVector3 pos = IntVector3.Zero;
                    IntVector3 sz = Size;

                    for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                    {
                        for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                        {
                            for (pos.X = 0; pos.X < sz.X; ++pos.X)
                            {
                                ushort value = Get(pos);
                                if (value != BlockHD.EmptyBlock)
                                {
                                    if (value == BlockHD.JointForward || value == BlockHD.JointUp)
                                    {
                                        joint = new VoxelJoint(pos, value);
                                    }
                                    else
                                    {
                                        result.Add(new VoxelHD(pos, value));
                                    }
                                }
                            }
                        }
                    }
                }

                return result;
            }

            public List<VoxelHD> GetVoxelArrayCentered()
            {
                IntVector3 limits = this.Limits;
                IntVector3 adjustPos = new IntVector3(limits.X / PublicConstants.Twice, 0, limits.Z / PublicConstants.Twice);
                IntVector3 voxelPos = IntVector3.Zero;
                List<VoxelHD> result = new List<VoxelHD>();
                IntVector3 pos = IntVector3.Zero;

                for (pos.Z = 0; pos.Z < limits.Z; ++pos.Z)
                {
                    voxelPos.Z = pos.Z + adjustPos.Z;
                    for (pos.Y = 0; pos.Y < limits.Y; ++pos.Y)
                    {
                        voxelPos.Y = pos.Y;
                        for (pos.X = 0; pos.X < limits.X; ++pos.X)
                        {
                            ushort val = Get(pos);
                            if (val != BlockHD.EmptyBlock)
                            {
                                voxelPos.X = pos.X + adjustPos.X;
                                result.Add(new VoxelHD(voxelPos, val));
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
                    case 2:
                        clone = FlipDir(Dimensions.X, limits, false);
                        clone = clone.FlipDir(Dimensions.Z, limits, false);
                        break;
                    case 3:
                        limits = new IntervalIntV3(IntVector3.Zero, new IntVector3(limits.AddZ, limits.AddY, limits.AddX));
                        clone = SwapXZ(limits);
                        clone = clone.FlipDir(Dimensions.Z, limits, false);
                        break;
                }

                if (replaceOriginalData && clone != null)
                {
                    array = clone.array;
                    initGrid(clone.Size);
                }
                return clone;
            }

            VoxelObjGridDataHD SwapXZ(IntervalIntV3 limits)
            {
                VoxelObjGridDataHD clone = new VoxelObjGridDataHD(limits.Add + 1);

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

                            clone.Set(pos, Get(fromPos));
                        }
                    }
                }

                if (replaceOriginalData)
                {
                    array = clone.array;
                }
                return clone;
            }

            public void Move(IntVector3 dir, IntervalIntV3 limits)
            {
                VoxelObjGridDataHD clone = this.Clone();
                initGrid(limits.Add + 1);
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

            //public void BuildOnTerrain(LootFest.Map.WorldPosition origo)
            //{
            //    if (!origo.CorrectPos)
            //        return;

            //    IntVector3 pos = IntVector3.Zero;
            //    IntVector3 size = Size;

            //    ushort material;
            //    for (pos.Z = 0; pos.Z < size.Z; ++pos.Z)
            //    {
            //        for (pos.X = 0; pos.X < size.X; ++pos.X)
            //        {
            //            pos.Y = 0;
            //            material = Get(pos);

            //            if (material != BlockHD.EmptyBlock)
            //            {
            //                VikingEngine.LootFest.Map.WorldPosition wp = origo.GetNeighborPos(pos);
            //                wp.SetFromHeightMap(0);
            //                ushort topMaterial = wp.GetBlock();

            //                while (wp.Y > origo.Y)
            //                {
            //                    wp.Screen.Set(wp, LootFest.Map.HDvoxel.BlockHD.EmptyBlock);
            //                    --wp.Y;
            //                }

            //                while (wp.Y <= origo.Y)
            //                {
            //                    wp.SetBlock(topMaterial);
            //                    ++wp.Y;
            //                }
            //            }

            //            for (pos.Y = 0; pos.Y < size.Y; ++pos.Y)
            //            {
            //                material = Get(pos);
            //                if (material != BlockHD.EmptyBlock)
            //                {
            //                    origo.GetNeighborPos(pos).SetBlock_IfOpen(material);
            //                }
            //            }
            //        }
            //    }
            //}

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

                // Updated to pass the 1D array inherited from Grid3D_L.
                // Adjust to your actual VoxelLib method signature appropriately.
                VoxelLib.CompressGridHD(this, w);
            }

            public void read(System.IO.BinaryReader r)
            {
                byte version = r.ReadByte();
                if (version > 0)
                {
                    IntVector3 sz = IntVector3.FromByteSzStream(r);

                    Resize(sz);
                    // Updated to read into the 1D array inherited from Grid3D_L.
                    // Adjust to your actual VoxelLib method signature appropriately.
                    VoxelLib.DeCompressGridHD(this, r);
                }
            }
        }
    }


