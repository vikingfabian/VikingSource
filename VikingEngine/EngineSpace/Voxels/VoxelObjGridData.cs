using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.Map;


namespace VikingEngine.Voxels
{
    class VoxelObjGridData
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

        public byte[, ,] MaterialGrid;
        public List<Voxel> special = null;
        
        public VoxelObjGridData(IntVector3 limits, List<Voxel> voxels)
        {
            newGrid(limits + 1);
            AddVoxels(voxels);
        }

        public void ReplaceMaterial(byte from1, byte to1)
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
        public void ReplaceMaterial(List<ByteVector2> findReplace)
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
                            foreach (ByteVector2 fromTo in findReplace)
                            {
                                if (MaterialGrid[pos.X, pos.Y, pos.Z] == fromTo.X)
                                {
                                    MaterialGrid[pos.X, pos.Y, pos.Z] = fromTo.Y;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Combine(List<ByteVector2> findReplace, List<VoxelObjGridData> addItemsData)
        {
            IntVector3 pos = IntVector3.Zero;
            IntVector3 sz = Size;

            for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
            {
                for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        foreach (ByteVector2 fromTo in findReplace)
                        {
                            if (MaterialGrid[pos.X, pos.Y, pos.Z] == fromTo.X)
                            {
                                MaterialGrid[pos.X, pos.Y, pos.Z] = fromTo.Y;
                                break;
                            }
                        }

                        if (addItemsData != null)
                        {
                            foreach (VoxelObjGridData grid in addItemsData)
                            {
                                if (grid.InBounds(pos) && grid.MaterialGrid[pos.X, pos.Y, pos.Z] != byte.MinValue)
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

                byte[, ,] old = MaterialGrid;
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
            
            MaterialGrid = new byte[size.X, size.Y, size.Z];
        }

        public VoxelObjGridData(byte[, ,] materialGrid)
        {
            MaterialGrid = materialGrid;
        }
        //public VoxelObjGridData()
        //{ }
        public VoxelObjGridData(IntVector3 limits)
        {
            IntVector3 size = limits + 1;
            MaterialGrid = new byte[size.X, size.Y, size.Z];
        }
        public VoxelObjGridData Clone()
        {
            byte[, ,] cloneGrid = (byte[, ,])MaterialGrid.Clone();
            return new VoxelObjGridData(cloneGrid);
        }

        public static VoxelObjGridData FromByteArray(byte[] array)
        {
            VoxelObjGridDataAnim anim = new VoxelObjGridDataAnim();
            System.IO.MemoryStream s = new System.IO.MemoryStream(array);
            System.IO.BinaryReader w = new System.IO.BinaryReader(s);
            anim.ReadBinaryStream(w);
            return anim.Frames[0];
        }

        public byte Get(IntVector3 pos)
        {
           return MaterialGrid[pos.X, pos.Y, pos.Z];
            //return 0;
        }
        public void Set(IntVector3 pos, byte value)
        {
            MaterialGrid[pos.X, pos.Y, pos.Z] = value;
        }
        public void SetSafe(IntVector3 pos, byte value)
        {
            IntVector3 sz = Size;
            if (pos.X >= 0 && pos.X < sz.X &&
                pos.Y >= 0 && pos.Y < sz.Y &&
                pos.Z >= 0 && pos.Z < sz.Z)
            {
                MaterialGrid[pos.X, pos.Y, pos.Z] = value;
            }
        }
        public byte GetSafe(IntVector3 pos)
        {
            //RangeIntV3 range = new RangeIntV3(IntVector3.Zero, Limits);
            if (InBounds(pos))
            { return MaterialGrid[pos.X, pos.Y, pos.Z]; }
            else
            { return 0; }
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

        public void AddVoxels(List<Voxel> voxels)
        {
            foreach (Voxel v in voxels)
            {
                MaterialGrid[v.Position.X, v.Position.Y, v.Position.Z] = v.Material;
            }
        }
        public void SafeAddVoxels(List<Voxel> voxels)
        {
            foreach (Voxel v in voxels)
            {
                SetSafe(v.Position, v.Material);
            }
        }

        public List<Voxel> GetVoxelArray()
        {
            List<Voxel> result = new List<Voxel>();
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
                            if (MaterialGrid[pos.X, pos.Y, pos.Z] != 0)
                                result.Add(new Voxel(pos, MaterialGrid[pos.X, pos.Y, pos.Z]));

                        }
                    }
                }
            }
            return result;
        }

        public bool[] UsingMaterials(int numMaterials)
        {
            bool[] use = new bool[numMaterials + 1];
            IntVector3 pos = IntVector3.Zero;
            IntVector3 sz = Size;
            for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
            {
                for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        use[MaterialGrid[pos.X, pos.Y, pos.Z]] = true;
                    }
                }
            }
            return use;
        }

        public bool UsingMaterial(int material)
        {
            IntVector3 pos = IntVector3.Zero;
            IntVector3 sz = Size;
            for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
            {
                for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        if (MaterialGrid[pos.X, pos.Y, pos.Z] == material)
                            return true;
                    }
                }
            }
            return false;
        }

        public void MaterialShift(int removeIx)
        {
            IntVector3 pos = IntVector3.Zero;
            IntVector3 sz = Size;
            for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
            {
                for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        if (MaterialGrid[pos.X, pos.Y, pos.Z] > removeIx)
                            MaterialGrid[pos.X, pos.Y, pos.Z]--;

                    }
                }
            }
        }

        public List<Voxel> GetVoxelArrayCentered() //only centers X and Z
        {
            IntVector3 limits = this.Limits;
            IntVector3 adjustPos = new IntVector3(limits.X / PublicConstants.Twice, 0, limits.Z / PublicConstants.Twice);
            IntVector3 voxelPos = IntVector3.Zero;
            List<Voxel> result = new List<Voxel>();
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
                        if (MaterialGrid[pos.X, pos.Y, pos.Z] != 0)
                        {
                            voxelPos.X = pos.X + adjustPos.X;
                            result.Add(new Voxel(voxelPos, MaterialGrid[pos.X, pos.Y, pos.Z]));
                        }
                    }
                }
            }
            return result;
        }
        /// <returns>end position in the data array</returns>
        public int FromCompressedData(List<byte> data)
        {
            //add two values in the end to avoid crashing
            data.Add(0); data.Add(1); //data.Add(0); data.Add(1);

            IntVector3 pos = IntVector3.Zero;
            byte currentVal = data[0];
            int dataArrayPos = 2;
            int repetitionsLeft = data[1];
            IntVector3 sz = Size;

            for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
            {
                for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        if (repetitionsLeft <= 0)
                        {

                            if (dataArrayPos + 2 >= data.Count)
                            {
                                break;
                            }
                            else
                            {
                                currentVal = data[dataArrayPos];
                                dataArrayPos++;
                                repetitionsLeft = data[dataArrayPos];
                                dataArrayPos++;

                            }
                        }
                        repetitionsLeft--;
                        MaterialGrid[pos.X, pos.Y, pos.Z] = currentVal;
                    }
                }
            }
            return dataArrayPos;
        }


        public int FromCompressedDataWithColorReplace(List<byte> data, List<byte> find, List<byte> replace)
        {
            //add two values in the end to avoid crashing
            data.Add(0); data.Add(1); //data.Add(0); data.Add(1);

            IntVector3 pos = IntVector3.Zero;
            byte currentVal = currentValueReplace(data[0], find, replace);
            int dataArrayPos = 2;
            int repetitionsLeft = data[1];
            IntVector3 sz = Size;

            for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
            {
                for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        if (repetitionsLeft <= 0)
                        {

                            if (dataArrayPos + 2 >= data.Count)
                            {
                                break;
                            }
                            else
                            {
                                currentVal = currentValueReplace(data[dataArrayPos], find, replace);//data[dataArrayPos];
                                dataArrayPos++;
                                repetitionsLeft = data[dataArrayPos];
                                dataArrayPos++;

                            }
                        }
                        repetitionsLeft--;
                        MaterialGrid[pos.X, pos.Y, pos.Z] = currentVal;
                    }
                }
            }
            return dataArrayPos;
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

        public List<byte> ToCompressedData()
        {

            List<byte> result = new List<byte>();
            IntVector3 pos = IntVector3.Zero;
            byte lastVal = MaterialGrid[0, 0, 0];
            byte numReperitions = 0;

            IntVector3 sz = Size;

            for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
            {
                for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        if (lastVal == MaterialGrid[pos.X, pos.Y, pos.Z] && numReperitions < byte.MaxValue)
                        {
                            numReperitions++;
                        }
                        else
                        {
                            result.Add(lastVal); result.Add(numReperitions);
                            numReperitions = 1;
                            lastVal = MaterialGrid[pos.X, pos.Y, pos.Z];
                        }
                    }
                }
            }
            result.Add(lastVal); result.Add(numReperitions);

            return result;
        }

        const string NumberLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public string ToMessage()
        {

            string result = "|";
            List<byte> usingMaterials = new List<byte>();
            List<byte> compressedData = ToCompressedData();
            for (int i = 0; i < compressedData.Count; i += 2)
            {
                byte material = compressedData[i];
                int usingPos;
                if (usingMaterials.Contains(material))
                {
                    for (usingPos = 0; usingPos < usingMaterials.Count; usingPos++)
                    {
                        if (usingMaterials[usingPos] == material)
                            break;
                    }
                }
                else
                {
                    usingPos = usingMaterials.Count;
                    usingMaterials.Add(material);
                    if (usingMaterials.Count > NumberLetters.Length)
                        return "ERR";
                }
                result += NumberLetters[usingPos] + compressedData[i + 1].ToString();
            }

            foreach (byte m in usingMaterials)
            {
                result = m.ToString() + SaveData.Dimension + result;
            }


            return lib.IntV3Text(Limits) + SaveData.Dimension + result;
        }

        public VoxelObjGridData(string fromMessage)
        {
            string part1 = TextLib.EmptyString;
            foreach (char c in fromMessage)
            {
                if (c == '|')
                {
                    break;
                }
                else
                {
                    part1 += c;
                }
            }
            List<byte> compressedData = new List<byte>();
            List<int> materials = lib.StingIntDimentions(part1);
            IntVector3 readLimits = new IntVector3(materials[0], materials[1], materials[2]);
            materials.RemoveRange(0, 3);

            const bool BackWardMaterials = true;
            if (BackWardMaterials)
            {
                List<int> clone = new List<int>(materials.Capacity);
                materials.Clear();
                for (int i = clone.Count - 1; i >= 0; i--)
                {
                    materials.Add(clone[i]);
                }
            }

            if (materials.Count == 0)
            {
                return;
            }

            //materials.Insert(0, 0);

            fromMessage.Remove(0, part1.Length + 1);

            for (int i = 0; i < fromMessage.Length; i++)
            {
                char mLetter = fromMessage[i];
                for (int letterIx = 0; letterIx < NumberLetters.Length; letterIx++)
                {
                    if (NumberLetters[letterIx] == mLetter)
                    {
                        i++;
                        string num = TextLib.EmptyString;
                        while (i < fromMessage.Length && TextLib.Numbers.Contains(fromMessage[i]))
                        {
                            num += fromMessage[i];
                            i++;
                        }
                        compressedData.Add((byte)materials[letterIx]); compressedData.Add((byte)lib.SafeStringToInt(num));
                        i--;
                    }
                }
            }

            IntVector3 size = readLimits + 1;
            MaterialGrid = new byte[size.X, size.Y, size.Z];
            FromCompressedData(compressedData);
        }

        public VoxelObjGridData Rotate(int rotationSteps, bool replaceOriginalData)
        {
            return this.Rotate(rotationSteps, new IntervalIntV3(IntVector3.Zero, this.Limits), replaceOriginalData);
        }

        public VoxelObjGridData Rotate(int rotationSteps, IntVector3 limits, bool replaceOriginalData)
        {
            return this.Rotate(rotationSteps, new IntervalIntV3(IntVector3.Zero, limits), replaceOriginalData);
        }
        /// <param name="rotationSteps">a value between 1-3, representing 90degree steps</param>
        /// <param name="limits"></param>
        public VoxelObjGridData Rotate(int rotationSteps, IntervalIntV3 limits, bool replaceOriginalData)
        {
            VoxelObjGridData clone = null;

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
        VoxelObjGridData SwapXZ(IntervalIntV3 limits)
        {
            VoxelObjGridData clone = new VoxelObjGridData(limits.Add + 1);//this.Clone();
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
        public VoxelObjGridData FlipDir(Dimensions dimention, IntervalIntV3 limits, bool replaceOriginalData)
        {
            VoxelObjGridData clone = new VoxelObjGridData(limits.Add + 1);//this.Clone();
            //newGrid(limits.Add + 1);
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
            VoxelObjGridData clone = this.Clone();
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

            byte material;
            for (pos.Z = 0; pos.Z < size.Z; ++pos.Z)
            {
                for (pos.X = 0; pos.X < size.X; ++pos.X)
                {
                    pos.Y = 0;
                    material = Get(pos);

                    if (material != byte.MinValue)
                    {
                        WorldPosition wp = origo.GetNeighborPos(pos);
                        wp.SetFromHeightMap(0);
                        ushort topMaterial = wp.GetBlock();//wp.Screen.voxels.Get(wp).material;

                        while (wp.Y > origo.Y)
                        {
                            wp.SetBlock(LootFest.Map.HDvoxel.BlockHD.EmptyBlock);//wp.Screen.voxels.Set(wp, LootFest.Map.HDvoxel.BlockHD.Empty);
                            --wp.Y;
                        }

                        while (wp.Y <= origo.Y)
                        {
                            wp.SetBlock(topMaterial);//wp.Screen.voxels.Set(wp, new LootFest.Map.HDvoxel.BlockHD(topMaterial));
                            ++wp.Y;
                        }
                    }

                    for (pos.Y = 0; pos.Y < size.Y; ++pos.Y)
                    {
                        material = Get(pos);
                        if (material != byte.MinValue)
                        {
                            if (material == VoxelLib.AntiBlock)
                            {
                                material = byte.MinValue;
                            }

                            //LootFest.LfRef.chunks.SetIfOpen(origo.GetNeighborPos(pos), material);
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
                        if (Get(pos) != byte.MinValue)
                        {
                            return pos.Y;
                        }
                    }
                }
            }

            return 0;
        }

        public void Merge(VoxelObjGridData other, bool keepOldGridSize, bool newBlocksReplaceOld, IntVector3 offset)
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
            byte oldMaterial;
            byte newMaterial;
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
                            if (newMaterial != 0)
                            {
                                Set(pos + offset, newMaterial);
                            }
                        }
                        else
                        {
                            if (oldMaterial == 0)
                            {
                                Set(pos + offset, newMaterial);
                            }
                        }
                        
                    }
                }
            }
        }


        public void write(System.IO.BinaryWriter w)
        {
            const byte Version = 0;
            IntVector3 pos = IntVector3.Zero;
            IntVector3 sz = Size;

            w.Write(Version);
            sz.WriteByteStream(w);

            for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
            {
                for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        w.Write(MaterialGrid[pos.X, pos.Y, pos.Z]);
                    }
                }
            }
        }
        public void read(System.IO.BinaryReader r)
        {
            byte version = r.ReadByte();
            IntVector3 pos = IntVector3.Zero;
            IntVector3 sz = IntVector3.FromByteSzStream(r);

            MaterialGrid = new byte[sz.X, sz.Y, sz.Z];

            for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
            {
                for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        MaterialGrid[pos.X, pos.Y, pos.Z] = r.ReadByte();
                    }
                }
            }
        }
    }
}
