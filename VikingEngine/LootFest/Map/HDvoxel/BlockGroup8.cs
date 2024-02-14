//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.LootFest.Map.HDvoxel
//{
//    abstract class AbsBlockGroup8
//    {
//        public const int BlockBroupWidth = 8;
//        //public static readonly BlockGroup8_Empty Empty = new BlockGroup8_Empty();
//        abstract public BlockHD Get(int x, int y, int z);
//        abstract public void Set(int x, int y, int z, BlockHD value);
//        abstract public bool IsFill { get; }

//        abstract public BlockHD FillBlock { get; }
//        abstract public void FillWidth(BlockHD value);
//        abstract public bool isFilledCheck();
//    }

//    //class BlockGroup8_Empty : AbsBlockGroup8
//    //{

//    //}

//    class BlockGroup8_Filled : AbsBlockGroup8
//    {
//        BlockHD block;

//        public BlockGroup8_Filled(BlockHD block)
//        {
//            this.block = block;
//        }

//        public override BlockHD Get(int x, int y, int z)
//        {
//            return block;
//        }
//        public override void Set(int x, int y, int z, BlockHD value)
//        {
//            throw new NotImplementedException();
//        }
//        public override bool IsFill
//        {
//            get { return true; }
//        }
//        public override BlockHD FillBlock
//        {
//            get { return block; }
//        }
//        public override void FillWidth(BlockHD value)
//        {
//            block = value;
//        }
//        public override bool isFilledCheck()
//        {
//            return true;
//        }
//    }

//    class BlockGroup8 : AbsBlockGroup8
//    {
//        const int MaxCount = BlockBroupWidth * BlockBroupWidth * BlockBroupWidth;
//        public int count = 0;
//        public BlockHD[,,] grid;

//        public BlockGroup8()
//        {
//            grid = new BlockHD[BlockBroupWidth, BlockBroupWidth, BlockBroupWidth];
//        }

//        public override BlockHD Get(int x, int y, int z)
//        {
//            return grid[x, y, z];
//        }
//        public override void Set(int x, int y, int z, BlockHD value)
//        {
//            if (value.material == 0)
//            {
//                if (grid[x, y, z].material != 0)
//                {
//                    ++count;
//                }
//            }
//            else
//            {
//                if (grid[x, y, z].material == 0)
//                {
//                    --count;
//                }
//            }
//            grid[x, y, z] = value;
//        }

//        public override bool isFilledCheck()
//        {
//            if (count == MaxCount)
//            {
//                BlockHD compare = grid[0, 0, 0];
//                for (int z = 0; z < BlockBroupWidth; ++z)
//                {
//                    for (int y = 0; y < BlockBroupWidth; ++y)
//                    {
//                        for (int x = 0; x < BlockBroupWidth; ++x)
//                        {
//                            if (grid[x, y, z] != compare)
//                            {
//                                return false;
//                            }
//                        }
//                    }
//                }
//                return true;
//            }
//            else if (count == 0)
//            {
//                return true;
//            }
//            return false;
//        }

//        public override void FillWidth(BlockHD value)
//        {
//            for (int z = 0; z < BlockBroupWidth; ++z)
//            {
//                for (int y = 0; y < BlockBroupWidth; ++y)
//                {
//                    for (int x = 0; x < BlockBroupWidth; ++x)
//                    {
//                        grid[x, y, z] = value;
//                    }
//                }
//            }

//            count = MaxCount;
//        }

//        public override bool IsFill
//        {
//            get { return false; }
//        }
//        public override BlockHD FillBlock
//        {
//            get { throw new NotImplementedException(); }
//        }
//    }


//    //class BlockLayer
//    //{

//    //}

//}
