using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Map.HDvoxel;


namespace VikingEngine.Voxels
{
    static class VoxelLib
    {
        public const string VoxelObjByteArrayEnding = ".vox";
        public static readonly byte AntiBlock = (byte)VikingEngine.LootFest.Data.MaterialType.AntiBlock;
        public const int FaceTopDir = 1;
        public const int FaceFrontDir = 1;
        public const int FaceRightDir = -1;
        public const byte JointStart = (byte)VikingEngine.LootFest.Data.MaterialType.Joint0;
        public const byte JointEnd = (byte)VikingEngine.LootFest.Data.MaterialType.Joint9;

        public static byte[, ,] MaterialGrid(IntervalIntV3 drawLimits, List<Voxel> voxels)
        {
            byte[, ,] result = new byte[drawLimits.Max.X + 1, drawLimits.Max.Y + 1, drawLimits.Max.Z + 1];
            foreach (Voxel v in voxels)
            { //create grid
                if (drawLimits.pointInBounds(v.Position))
                    result[v.Position.X, v.Position.Y, v.Position.Z] = v.Material;
            }
            return result;
        }
        public static bool emptyBlock(byte[, ,] materialGrid, IntervalIntV3 drawLimits, IntVector3 pos)
        {
            if (!drawLimits.pointInBounds(pos))
                return true;
            return materialGrid[pos.X, pos.Y, pos.Z] == 0;

        }

        const int ByteShiftLenght = 8;


        public static void GridToMaterialAndReps(byte[, ,] grid, List<IntVector2> materialAndReps, byte[] usedMaterials)
        {
            IntVector3 size = new IntVector3(grid.GetLength(0), grid.GetLength(1), grid.GetLength(2));
            IntVector3 pos = IntVector3.Zero;

            //Go through the grid and look for repetitions
            byte lastVal = grid[0, 0, 0];
            int numRepetitions = 0;
            for (pos.Y = 0; pos.Y < size.Y; pos.Y++)
            {
                for (pos.Z = 0; pos.Z < size.Z; pos.Z++)
                {
                    for (pos.X = 0; pos.X < size.X; pos.X++)
                    {
                        byte value = grid[pos.X, pos.Y, pos.Z];
                        if (value == lastVal)
                        {
                            numRepetitions++;
                        }
                        else if (numRepetitions > 0)
                        {
                            materialAndReps.Add(new IntVector2(lastVal, numRepetitions));
                            usedMaterials[lastVal] = 1;
                            lastVal = value;
                            numRepetitions = 1;
                        }
                    }
                }
            }
            materialAndReps.Add(new IntVector2(lastVal, numRepetitions));
            usedMaterials[lastVal] = 1;
        }

        public static List<byte> GetUsedMaterialsList(byte[] usedMaterials)
        {
            List<byte> result = new List<byte>();
            usedMaterials[0] = 0; //Empty has value zero
            for (int i = 1; i < usedMaterials.Length; i++)
            {
                if (usedMaterials[i] != 0)
                {
                    //use a header to view the materials used
                    result.Add((byte)i);
                    usedMaterials[i] = (byte)result.Count;
                }
            }
            return result;
        }

        public static void WriteUsedMaterials(System.IO.BinaryWriter w, List<byte> usedMaterialsList)
        {
            w.Write((byte)usedMaterialsList.Count); //how many materials included
            foreach (byte m in usedMaterialsList)
            {
                w.Write(m);
            }
        }

        public static byte[] ReadUsedMaterials(System.IO.BinaryReader r)
        {
            int numUsedMaterials = r.ReadByte();
            byte[] result = r.ReadBytes(numUsedMaterials);
            return result;
        }

        /// <summary>
        /// writing a byte[,,] grid with a very high compression
        /// </summary>
        public static void WriteMaterialAndReps(System.IO.BinaryWriter w, List<IntVector2> materialAndReps, byte[] usedMaterials, List<byte> usedMaterialsList)
        {
            w.Write((ushort)materialAndReps.Count); //how many repeating parts
            foreach (IntVector2 val in materialAndReps)
            {
                DataStream.DataStreamLib.WriteGrowingBitShiftValue(w, val.Y); //write repetitions
            }
            //Divitions
            int numDivitions;
            int bitShiftLeght;
            int divLength;
            ByteGridDivitions(usedMaterialsList.Count, out numDivitions, out bitShiftLeght, out divLength);

            int materialAndRepsIndex = 0;
            EightBit materialIndexBits = new EightBit(0);
            int eightBitPos = 0;
            int bits = 0;
            int bitsIndex = 0;

            while (true)
            {
                eightBitPos--;
                if (eightBitPos < 0)
                {
                    if (materialAndRepsIndex < materialAndReps.Count)
                    {
                        eightBitPos = Bound.Min(bitShiftLeght - 1, 0);
                        materialIndexBits.bitArray = usedMaterials[materialAndReps[materialAndRepsIndex].X];
                        materialAndRepsIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
                bool bit = materialIndexBits.Get(eightBitPos);
                if (bit)
                {
                    bits = bits | 1;
                }
                
                bitsIndex++;
                if (bitsIndex >= ByteShiftLenght)
                {
                    w.Write((byte)bits);
                    bits = 0;
                    bitsIndex = 0;
                }
                else
                {
                    bits = bits << 1; 
                }
            }
            //make sure the last bits are shifted to leftmost pos
            if (bitsIndex > 0)
            {
                w.Write((byte)(bits << ByteShiftLenght - 1 - bitsIndex));
                int lastByte = (byte)(bits << ByteShiftLenght - 1 - bitsIndex);
            }
        }

        

        public static byte[, ,] ReadMaterialAndReps(System.IO.BinaryReader r, IntVector3 gridSize, byte[] usedMaterialsList, 
            bool checkSpecialBlocks, out List<Voxel> special)
        {
            special = null;

            byte[, ,] grid = new byte[gridSize.X, gridSize.Y, gridSize.Z];
            int numRepetitionParts = r.ReadUInt16();

            int[] repetionsList = new int[numRepetitionParts];

            for (int i = 0; i < numRepetitionParts; i++)
            {
                repetionsList[i]= DataStream.DataStreamLib.ReadGrowingBitShiftValue(r);
            }

            int numDivitions;
            int bitShiftLeght;
            int maxDivValue;
            ByteGridDivitions(usedMaterialsList.Length, out numDivitions, out bitShiftLeght, out maxDivValue);

            
            IntVector3 pos = IntVector3.Zero;

            EightBit readBits = new EightBit(0);
            EightBit materialIndexBits = new EightBit(0);
            int readBitPos = 0;

            for (int partIx = 0; partIx < numRepetitionParts; partIx++)
            {
                //pick the next range of bits
                materialIndexBits.bitArray = 0;
                for (int bitIndex = bitShiftLeght - 1; bitIndex >= 0; bitIndex--)
                {
                    readBitPos--;
                    if (readBitPos < 0)
                    {
                        readBits.bitArray = r.ReadByte();
                        readBitPos = ByteShiftLenght - 1;
                    }
                    if (readBits.Get(readBitPos))
                    {
                        materialIndexBits.Set(bitIndex, true);
                    }
                }

                //place in grid
                if (materialIndexBits.bitArray == byte.MinValue)
                {
                    NextBlock(repetionsList[partIx], ref pos, gridSize);
                }
                else
                {
                    byte material = usedMaterialsList[materialIndexBits.bitArray - 1];
                    if (checkSpecialBlocks && material >= JointStart && material <= JointEnd)
                    {
                        if (special == null)
                        {
                            special = new List<Voxel>(8);
                        }

                        special.Add(new Voxel(pos, material));
                        NextBlock(repetionsList[partIx], ref pos, gridSize);
                    }
                    else
                    {
                        for (int rep = 0; rep < repetionsList[partIx]; rep++)
                        {
                            grid[pos.X, pos.Y, pos.Z] = material;
                            NextBlock(ref pos, gridSize);
                        }
                    }
                }
                
            }
            return grid;
        }

        static void NextBlock(int repetitions, ref IntVector3 position, IntVector3 size)
        {
            position.X += repetitions;
            if (position.X >= size.X)
            {
                int addZ = position.X / size.X;
                position.Z += addZ;
                position.X -= size.X * addZ;
                if (position.Z >= size.Z)
                {
                    int addY = position.Z / size.Z;
                    position.Y += addY;
                    position.Z -= size.Z * addY;
                }
            }
        }
        static void NextBlock(ref IntVector3 position, IntVector3 size)
        {
            position.X++;
            if (position.X >= size.X)
            {
                position.X = 0;
                position.Z++;
                if (position.Z >= size.Z)
                {
                    position.Z = 0;
                    position.Y++;
                }
            }
        }

        
        public static void WriteVoxelObj(System.IO.BinaryWriter w, byte[, ,] grid)
        {
            writeVoxelObjPrefix(w, new IntVector3(grid.GetLength(0), grid.GetLength(1), grid.GetLength(2)), 1);

            List<IntVector2> materialAndReps = new List<IntVector2>();
            byte[] usedMaterials = new byte[PublicConstants.ByteSize];
            
            GridToMaterialAndReps(grid, materialAndReps, usedMaterials);

            List<byte> usedMaterialsList = GetUsedMaterialsList(usedMaterials);
            WriteUsedMaterials(w, usedMaterialsList);
            WriteMaterialAndReps(w, materialAndReps, usedMaterials, usedMaterialsList);
            //Voxels.VoxelLib.WriteGrid(grid, w);
        }

        public static void WriteVoxelObjAnim(System.IO.BinaryWriter w, List<byte[,,]> grids)
        {
            writeVoxelObjPrefix(w, new IntVector3(grids[0].GetLength(0), grids[0].GetLength(1), grids[0].GetLength(2)), grids.Count);
            List<IntVector2>[] materialAndReps = new List<IntVector2>[grids.Count];
            byte[] usedMaterials = new byte[PublicConstants.ByteSize];

            for (int frame = 0; frame < grids.Count; frame++)
            {
                materialAndReps[frame] = new List<IntVector2>();
                GridToMaterialAndReps(grids[frame], materialAndReps[frame], usedMaterials);
            }
            List<byte> usedMaterialsList = GetUsedMaterialsList(usedMaterials);
            WriteUsedMaterials(w, usedMaterialsList);
            for (int frame = 0; frame < grids.Count; frame++)
            {
                WriteMaterialAndReps(w, materialAndReps[frame], usedMaterials, usedMaterialsList);
            }
        }

        public static void WriteVoxelObjAnimHD(System.IO.BinaryWriter w, ushort[,,] grid)
        {
            writeVoxelObjPrefix(w, new IntVector3(grid.GetLength(0), grid.GetLength(1), grid.GetLength(2)), 1);
            CompressGridHD(grid, w);
        }
        public static void WriteVoxelObjAnimHD(System.IO.BinaryWriter w, List<VoxelObjGridDataHD> grids)
        {
            writeVoxelObjPrefix(w, grids[0].Size, grids.Count);

            for (int frame = 0; frame < grids.Count; frame++)
            {
                CompressGridHD(grids[frame].MaterialGrid, w);
            }
        }
        public static List<VoxelObjGridDataHD> ReadVoxelObjectAnimHD(System.IO.BinaryReader r)
        {
            int startPos = (int)r.BaseStream.Position;

            List<VoxelObjGridDataHD> grids;
            byte saveVersion;
            int numFrames;
            IntVector3 size;

            ReadVoxelObjPrefix(r, out saveVersion, out numFrames, out size);
            grids = new List<VoxelObjGridDataHD>(numFrames);

            if (saveVersion < 100)
            {
                r.BaseStream.Position = startPos;
                var oldGrids = ReadVoxelObjectAnim(r);
                if (oldGrids == null)
                {
                    grids.Add(new VoxelObjGridDataHD(new IntVector3(5)));
                }
                else
                {
                    foreach (var og in oldGrids)
                    {
                        grids.Add(new VoxelObjGridDataHD(ConvertToHD(og)));
                    }
                }
            }
            else
            {
                for (int frame = 0; frame < numFrames; frame++)
                {
                    ushort[, ,] grid = new ushort[size.X, size.Y, size.Z];
                    DeCompressGridHD(grid, r);

                    grids.Add(new VoxelObjGridDataHD(grid));
                }
            }

            return grids;
        }

        //static List<byte[, ,]> ReadVoxelObjectAnimOld(int numFrames, IntVector3 size, System.IO.BinaryReader r)
        //{
        //    List<Voxel> non;
        //    var grids = new List<byte[, ,]>(numFrames);
        //    byte[] usedMaterials = ReadUsedMaterials(r);
        //    for (int frame = 0; frame < numFrames; frame++)
        //    {
        //        grids.Add(Voxels.VoxelLib.ReadMaterialAndReps(r, size, usedMaterials, false, out non));
        //    }

        //    return grids;
        //}

        //static Color[] colors = null;
        static readonly ushort[] OldToNewBlock = new ushort[] {
63937,
61441,
65281,
3841,
4081,
241,
61681,
57633,
65281,
2641,
2801,
9105,
57473,
65521,
61153,
61153,
56785,
52417,
52417,
48049,
43681,
43681,
39313,
34945,
30577,
30577,
26209,
21841,
17473,
13105,
4369,
1,
63857,
64129,
64641,
65425,
44433,
52625,
35985,
31937,
28145,
31441,
35265,
34993,
43185,
47281,
63937,
63889,
63041,
63569,
64081,
65377,
44401,
31857,
15217,
7089,
3057,
18625,
22449,
26017,
34465,
42657,
63137,
63089,
57633,
63281,
63761,
65281,
35889,
15169,
2641,
2705,
2801,
1969,
1441,
9105,
25233,
37505,
57473,
57681,
36865,
41985,
42497,
43521,
22561,
5937,
1841,
1889,
1393,
593,
881,
44705,
12353,
16449,
28737,
28705,
28673,
29185,
29697,
34561,
17937,
1313,
1313,
1361,
1393,
865,
593,
65,
12353,
16449,
28737,
28705,
52113,
39025,
30289,
21569,
12833,
51553,
42833,
34353,
29729,
25361,
64929,
65105,
62897,
44705,
35889,
30993,
42257,
48177,
60273,
60241,
46641,
39009,
43649,
25361,
26177,
21265,
42545,
51809,
43105,
56449,
21265,
37905,
34657,
48065,
65505,
22561,
17665,
25345,
30497,
38721,
30545,
21265,
43697,
37905,
56177,
39809,
30545,
65521,
65249,
39009,
52161,
42257,
60545,
29729,
50993,
56209,
43089,
30337,
38705,
12833,
44337,
59425,
60081,
52353,
34769,
64049,
65521,
34081,
59457,
56161,
22385,
38177,
17473,
21345,
6449,
57633,
41985,
2801,
60273,
60561,
33553,
33553,
50977,
55345,
12561,
16913,
39233,
43841,
60833,
34337,
56449,
56465,
34337,
34337,
60833,
34337,
34337,
60561,
34337,
60817,
34337,
34337,
56449,
60817,
60833,
65217,
34337,
34337,
56449,
65217,
34337,
34337,
34337,
34337,
56449,
34337,
34337,
34337,
38705,
61105,
34337,
34337,
34337,
34337,
56449,
60833,
34337,
34337,
12833,
12833,
51873,
17201,
63937,
56449,
34337,
34337,
34337,
38705,
61105,
34337,
34337,
34337,
34337,
4081 };

        static ushort[, ,] ConvertToHD(byte[, ,] grid)
        {
            //if (colors == null)
            //{
            //    Texture2D tex = Engine.LoadContent.Texture(LoadedTexture.BlockTextures);//
            //    colors = new Color[tex.Width * tex.Height];
                
            //    tex.GetData<Color>(colors);
            //}
            ushort[, ,] result = new ushort[grid.GetLength(0), grid.GetLength(1), grid.GetLength(2)];

            IntVector3 pos = IntVector3.Zero;

            for (pos.Z = 0; pos.Z < grid.GetLength(2); ++pos.Z)
            {
                for (pos.Y = 0; pos.Y < grid.GetLength(1); ++pos.Y)
                {
                    for (pos.X = 0; pos.X < grid.GetLength(0); ++pos.X)
                    {
                        if (grid[pos.X, pos.Y, pos.Z] != byte.MinValue)
                        {
                            result[pos.X, pos.Y, pos.Z] = OldToNewBlock[grid[pos.X, pos.Y, pos.Z]];
                        }
                    }
                }
            }

            return result;
        }

        //static ushort ConvertToBlockHD(byte oldBlock)//, Color[] colors, int texW)
        //{
        //    var tileIx = VikingEngine.LootFest.Data.BlockTextures.Materials[oldBlock].TopTiles.startTile;
        //    Graphics.ImageFile2 file = LootFest.LfRef.Images.TileIxToImgeFile[tileIx];
        //    Point texpos = file.Source.Center;

        //    var col = colors[texpos.X + texpos.Y * texW];
        //    return BlockHD.ToBlockValue(col, BlockHD.UnknownMaterial);//new BlockHD(col);
        //}
        

        public static void CompressGridHD(ushort[, ,] grid, System.IO.BinaryWriter w)
        {
            ushort currentColor = 0;
            int repeatCount = 0;

            IntVector3 pos = IntVector3.Zero;
            var sz = new IntVector3(grid.GetLength(0), grid.GetLength(1), grid.GetLength(2));

            for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
            {
                for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        if (repeatCount < byte.MaxValue &&
                            grid[pos.X, pos.Y, pos.Z] == currentColor)
                        {
                            ++repeatCount;
                        }
                        else
                        {
                            if (repeatCount > 0)
                            {
                                w.Write(currentColor);//currentColor.write(w);
                                w.Write((byte)repeatCount);
                            }

                            currentColor = grid[pos.X, pos.Y, pos.Z];
                            repeatCount = 1;

                        }
                    }
                }
            }

            if (repeatCount > 0)
            {
                w.Write(currentColor);//currentColor.write(w);
                w.Write((byte)repeatCount);
            }

            w.Write(BlockHD.EmptyBlock);
            w.Write(byte.MinValue);
            //BlockHD.EndBlock.write(w);
        }

        public static void DeCompressGridHD(ushort[, ,] grid, System.IO.BinaryReader r)
        {
            var sz = new IntVector3(grid.GetLength(0), grid.GetLength(1), grid.GetLength(2));
            IntVector3 pos = IntVector3.Zero;
            ushort block = ushort.MinValue;//BlockHD.EndBlock;
            int repeatCount = 0;

            while (true)
            {
                //block.read(r);
                //if (block.material == BlockHD.EndBlockMaterial) return;
                block = r.ReadUInt16();
                repeatCount = r.ReadByte();

                if (block == BlockHD.EmptyBlock)
                {
                    if (repeatCount == byte.MinValue)
                    {
                        return;
                    }
                    NextBlock(repeatCount, ref pos, sz);
                }
                else
                {
                    for (int i = 0; i < repeatCount; ++i)
                    {
                        //if (pos.Y < sz.Y)
                        //{
                            grid[pos.X, pos.Y, pos.Z] = block;
                        //}
                        NextBlock(ref pos, sz);
                    }
                }
            }
        }


        public static void CompressToList(ushort[, ,] grid, List<ushort> list)
        {
            ushort currentColor = 0;
            int repeatCount = 0;

            IntVector3 pos = IntVector3.Zero;
            var sz = new IntVector3(grid.GetLength(0), grid.GetLength(1), grid.GetLength(2));

            for (pos.Y = 0; pos.Y < sz.Y; ++pos.Y)
            {
                for (pos.Z = 0; pos.Z < sz.Z; ++pos.Z)
                {
                    for (pos.X = 0; pos.X < sz.X; ++pos.X)
                    {
                        if (repeatCount < ushort.MaxValue &&
                            grid[pos.X, pos.Y, pos.Z] == currentColor)
                        {
                            ++repeatCount;
                        }
                        else
                        {
                            if (repeatCount > 0)
                            {
                                list.Add(currentColor);
                                list.Add((ushort)repeatCount);
                            }

                            currentColor = grid[pos.X, pos.Y, pos.Z];
                            repeatCount = 1;

                        }
                    }
                }
            }

            if (repeatCount > 0)
            {
                list.Add(currentColor);
                list.Add((ushort)repeatCount);
            }
        }

        public static void DeCompressList(List<ushort> list, ushort[, ,] grid)
        {
            IntVector3 pos = IntVector3.Zero;
            ushort block = ushort.MinValue;
            int repeatCount = 0;

            var sz = new IntVector3(grid.GetLength(0), grid.GetLength(1), grid.GetLength(2));

            for (int listIx = 0; listIx < list.Count; listIx += 2)
            {
                block = list[listIx];
                repeatCount = list[listIx + 1];

                if (block == BlockHD.EmptyBlock)
                {
                    NextBlock(repeatCount, ref pos, sz);
                }
                else
                {
                    for (int repeatIx = 0; repeatIx < repeatCount; ++repeatIx)
                    {
                        grid[pos.X, pos.Y, pos.Z] = block;
                        NextBlock(ref pos, sz);
                    }
                }
            }
        }


        
        public static byte[, ,] ReadVoxelObject(System.IO.BinaryReader r, bool checkSpecialBlocks, out List<Voxel> special)
        {
            byte[, ,] grid;// = new List<byte[, ,]>();
            byte saveVersion;
            int numFrames;
            IntVector3 size;

            
            if (ReadVoxelObjPrefix(r, out saveVersion, out numFrames, out size))
            {
                byte[] usedMaterials = ReadUsedMaterials(r);
                grid = Voxels.VoxelLib.ReadMaterialAndReps(r, size, usedMaterials, checkSpecialBlocks, out special);
            }
            else
            {
                //to old version, backwards working
                special = null;
                return null;
            }
            return grid;
        }
        public static List<byte[,,]> ReadVoxelObjectAnim(System.IO.BinaryReader r)
        {
            List<byte[, ,]> grids;
            List<Voxel> non;
            byte saveVersion;
            int numFrames;
            IntVector3 size;

            bool correctVersion = ReadVoxelObjPrefix(r, out saveVersion, out numFrames, out size);
            if (correctVersion)
            {
                grids = new List<byte[, ,]>(numFrames);
                byte[] usedMaterials = ReadUsedMaterials(r);
                for (int frame = 0; frame < numFrames; frame++)
                {
                    grids.Add(Voxels.VoxelLib.ReadMaterialAndReps(r, size, usedMaterials, false, out non));
                }
            }
            else
            {
                //to old version, backwards working
                return null;
            }
            return grids;
        }

        static void writeVoxelObjPrefix(System.IO.BinaryWriter w, IntVector3 size, int numFrames)
        {
            const byte SaveVersion = 100;//92;
            //IntVector3 size = new IntVector3(grid.GetLength(0), grid.GetLength(1), grid.GetLength(2));

            w.Write(SaveVersion);
            w.Write((byte)numFrames);
            size.WriteByteStream(w);
        }
        /// <returns>Load succeded</returns>
        static bool ReadVoxelObjPrefix(System.IO.BinaryReader r, out byte saveVersion, out int numFrames, out IntVector3 size)
        {
            int startPos = (int)r.BaseStream.Position;
            size = IntVector3.Zero;

            saveVersion = r.ReadByte();
            numFrames = r.ReadByte();
            size.ReadByteStream(r);

            if (saveVersion < 92)
            {
                r.BaseStream.Position = startPos;
                return false;
            }
            return true;
        }

        static void ByteGridDivitions(int numUsedMaterials, out int numDivitions, out int bitShiftLeght, out  int maxDivValue)
        {
            numUsedMaterials++; //include empty
            numDivitions = 0;
            bitShiftLeght = ByteShiftLenght;
            maxDivValue = PublicConstants.ByteSize;

            while (maxDivValue / PublicConstants.Twice >= numUsedMaterials)
            {
                maxDivValue /= PublicConstants.Twice;
                //128, 64, 32, 16, 8
                numDivitions++;
#if PCGAME
                if (bitShiftLeght < 1)
                {
                    throw new Exception();
                }
#endif
                bitShiftLeght--;
                //7, 6, 5, 4, 3
            }
        }

    }



    //interface IVoxelGridIOPortal
    //{
    //    ushort GetVoxelFromPortal(LootFest.Map.WorldPosition wp);
    //    void SetVoxelToPortal(LootFest.Map.WorldPosition wp, ushort material);
    //}
    interface IPreparedFaceCorners
    {
        bool Empty { get; }
        float C0 { get; }
        float C1 { get; }
        float C2 { get; }
        float C3 { get; }

    }
    struct PreparedFaceEmptyCorners : IPreparedFaceCorners
    {
        public static readonly PreparedFaceEmptyCorners GetEmpty = new PreparedFaceEmptyCorners();
        public bool Empty { get { return true; } }
        public float C0 { get { return 1; } }
        public float C1 { get { return 1; } }
        public float C2 { get { return 1; } }
        public float C3 { get { return 1; } }

    }
    struct PreparedFaceCorners : IPreparedFaceCorners
    {
        public static readonly PreparedFaceCorners Zero = new PreparedFaceCorners(0,0,0,0);
        public static readonly PreparedFaceCorners Two = new PreparedFaceCorners(2, 2, 2, 2);
        static readonly List<float> CornerValueToDarkness = new List<float> { 1, 1, 0.8f, 0.6f, 0.5f, 0.5f, 0.1f };//new List<float> { 1, 0.8f, 0.6f, 0.6f };
        public byte Corner0;
        public byte Corner1;
        public byte Corner2;
        public byte Corner3;
        public bool Empty { get { return false; } }
        public float C0 { get { return CornerValueToDarkness[Corner0]; } }
        public float C1 { get { return CornerValueToDarkness[Corner1]; } }
        public float C2 { get { return CornerValueToDarkness[Corner2]; } }
        public float C3 { get { return CornerValueToDarkness[Corner3]; } }

        public PreparedFaceCorners(byte c0,byte c1, byte c2, byte c3)
        {
            Corner0 = c0;
            Corner1 = c1;
            Corner2 = c2;
            Corner3 = c3;
        }
        public void Reset()
        {
            Corner0 = 1;
            Corner1 = 1;
            Corner2 = 1;
            Corner3 = 1;
        }
    }
    struct FaceCornerColorYS
    {
        const float LeftSideR = 1.1f;
        const float LeftSideG = 0.9f;
        const float LeftSideB = 0.8f;

        const float RightSideR = 0.9f;
        const float RightSideG = 0.9f;
        const float RightSideB = 1.1f;

        public static readonly FaceCornerColorYS TestWhite = new FaceCornerColorYS(1, false);

        public Color Col0;
        public Color Col1;
        public Color Col2;
        public Color Col3;

        static readonly List<float> CornerValueToDarkness = new List<float> { 1.2f, 1.2f, 0.9f, 0.6f, 0.5f, 0.5f, 0.1f };
        public static Color[,] PreCalculatedColor_Face_CornerVal;

        public static void Init()
        {
            PreCalculatedColor_Face_CornerVal = new Color[(int)CubeFace.NUM, CornerValueToDarkness.Count];
            for (int face = 0; face < (int)CubeFace.NUM; face++)
            {
                const float BottomSideBrightness = 0.5f;
                const float ShadowSideBrightness = 0.72f;
                const float LightSideBrightness = 0.85f;
                const float TopSideBrightness = 1f;
                Vector3 faceColor = Vector3.One;
                float faceDarkness;
                switch ((CubeFace)face)
                {
                    case CubeFace.Ypositive:
                        faceDarkness = TopSideBrightness;
                        break;
                    case CubeFace.Zpositive:
                        faceDarkness = LightSideBrightness;
                        break;
                    case CubeFace.Xnegative:
                        faceDarkness = LightSideBrightness;
                        faceColor = new Vector3(RightSideR, RightSideG, RightSideB);
                        break;
                    case CubeFace.Xpositive:
                        faceDarkness = ShadowSideBrightness;
                        faceColor = new Vector3(LeftSideR, LeftSideG, LeftSideB);
                        break;
                    case CubeFace.Ynegative:
                        faceDarkness = BottomSideBrightness;
                        break;
                    default:
                        faceDarkness = ShadowSideBrightness;
                        break;
                }
                for (int corner = 0; corner < CornerValueToDarkness.Count; corner++)
                {
                    PreCalculatedColor_Face_CornerVal[face, corner] =
                        new Color(
                            faceDarkness * faceColor.X * CornerValueToDarkness[corner],
                            faceDarkness * faceColor.Y * CornerValueToDarkness[corner],
                            faceDarkness * faceColor.Z * CornerValueToDarkness[corner]);
                }
            }
            
        }

        public FaceCornerColorYS(float whiteness, bool leftSide)
        {
            if (leftSide)
            {//lägga in gul sida
                Col0 = new Color(whiteness * LeftSideR, whiteness * LeftSideG, whiteness * LeftSideB);
            }
            else
            { Col0 = new Color(whiteness, whiteness, whiteness); }
            Col1 = Col0;
            Col2 = Col0;
            Col3 = Col0;
        }



        public FaceCornerColorYS(float whiteness, bool leftSide, float corner0, float corner1, float corner2, float corner3)
        {
            if (leftSide)
            {//lägga in gul sida
                float r = whiteness * LeftSideR;
                float g = whiteness * LeftSideG;
                float b = whiteness * LeftSideB;

                Col0 = new Color(corner0 * r, corner0 * g, corner0 * b);
                Col1 = new Color(corner1 * r, corner1 * g, corner1 * b);
                Col2 = new Color(corner2 * r, corner2 * g, corner2 * b);
                Col3 = new Color(corner3 * r, corner3 * g, corner3 * b);
            }
            else
            {
                float w = whiteness * corner0;
                Col0 = new Color(w, w, w);

                w = whiteness * corner1;
                Col1 = new Color(w, w, w);

                w = whiteness * corner2;
                Col2 = new Color(w, w, w);

                w = whiteness * corner3;
                Col3 = new Color(w, w, w);
            }
        }
    }
    struct FaceCornerColor
    {
        public Color Col0;
        public Color Col1;
        public Color Col2;
        public Color Col3;

        public FaceCornerColor(float whiteness, Vector3 color, float corner0, float corner1, float corner2, float corner3)
        {
            
                float w = whiteness * corner0;
                Col0 = new Color(w * color.X, w * color.Y, w * color.Z);

                w = whiteness * corner1;
                Col1 = new Color(w * color.X, w * color.Y, w * color.Z);

                w = whiteness * corner2;
                Col2 = new Color(w * color.X, w * color.Y, w * color.Z);

                w = whiteness * corner3;
                Col3 = new Color(w * color.X, w * color.Y, w * color.Z);
        }

    }
    struct MergeModelsOption
    {
        public bool KeepOldGridSize;
        public bool NewBlocksReplaceOld;
        public MergeFramesOptions MergeFramesOptions;

        public MergeModelsOption StandardInit()
        {
            KeepOldGridSize = true;
            NewBlocksReplaceOld = true;
            MergeFramesOptions = Voxels.MergeFramesOptions.NewFirstOnOldFrames;
            return this;
        }
    }

    enum MergeFramesOptions
    {
        NewFirstOnOldFrames,
        FrameByFrame,
        OldFirstOnNewFrames,
        NUM
    }
}
