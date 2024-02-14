//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.LootFest.Map.HDvoxel
//{
//    class VoxelsChunk
//    {
//        public const int BlockGroupWidth = 16;

//        public int blockLayersStartY = 0;
//        List<AbsBlockGroup8[,]> groupLayers = new List<AbsBlockGroup8[,]>(24);

//        public VoxelsChunk()
//        {
//            //Temp, fyll 92 block höjd
//            for (int i = 0; i < 24; ++i)
//            {
//                groupLayers.Add(new AbsBlockGroup8[BlockGroupWidth,BlockGroupWidth]);
//            }
//        }

//        //public BlockHD Get(WorldPosition pos)
//        //{
//        //    int blockGroupLocalY;
//        //    AbsBlockGroup8[,] layer = GetLayer(pos.WorldGrindex.Y, out blockGroupLocalY);

//        //    if (layer != null)
//        //    {
//        //        int blockGroupX, blockGroupZ, blockGroupLocalX, blockGroupLocalZ;

//        //        toBlockGroupXZ(pos.WorldGrindex.X, pos.WorldGrindex.Z, out blockGroupX, out blockGroupZ, out blockGroupLocalX, out blockGroupLocalZ);
//        //        AbsBlockGroup8 group = layer[blockGroupX, blockGroupZ];

//        //        if (group != null)
//        //        {
//        //            return group.Get(blockGroupLocalX, blockGroupLocalY, blockGroupLocalZ);
//        //        }
//        //    }

//        //    return BlockHD.Empty;
//        //}
//        //public void Set(WorldPosition pos, BlockHD value)
//        //{
//        //    int blockGroupLocalY;
//        //    AbsBlockGroup8[,] layer = GetLayer(pos.WorldGrindex.Y, out blockGroupLocalY);

//        //    if (layer != null)
//        //    {
//        //        int blockGroupX, blockGroupZ, blockGroupLocalX, blockGroupLocalZ;

//        //        toBlockGroupXZ(pos.WorldGrindex.X, pos.WorldGrindex.Z, 
//        //            out blockGroupX, out blockGroupZ, out blockGroupLocalX, out blockGroupLocalZ);
//        //        AbsBlockGroup8 group = layer[blockGroupX, blockGroupZ];

//        //        if (group == null)
//        //        {
//        //            if (value.HasMaterial)
//        //            {
//        //                group = new BlockGroup8();
//        //                group.Set(blockGroupLocalX, blockGroupLocalY, blockGroupLocalZ, value);
//        //                layer[blockGroupX, blockGroupZ] = group;
//        //            }
//        //        }
//        //        else if (group.IsFill)
//        //        {
//        //            if (value != group.FillBlock)
//        //            {
//        //                var newGroup = new BlockGroup8();
//        //                newGroup.FillWidth(group.FillBlock);
//        //                newGroup.Set(blockGroupLocalX, blockGroupLocalY, blockGroupLocalZ, value);
//        //                layer[blockGroupX, blockGroupZ] = newGroup;
//        //            }
//        //        }
//        //        else
//        //        {
//        //            group.Set(blockGroupLocalX, blockGroupLocalY, blockGroupLocalZ, value);
//        //            if (group.isFilledCheck())
//        //            {
//        //                if (value.IsEmpty)
//        //                {
//        //                    layer[blockGroupX, blockGroupZ] = null;
//        //                }
//        //                else
//        //                {
//        //                    group = new BlockGroup8_Filled(value);
//        //                    layer[blockGroupX, blockGroupZ] = group;
//        //                }
//        //            }
//        //        }
//        //    }
//        //}

//        void toBlockGroupXZ(int worldX, int worldZ, out int blockGroupX, out int blockGroupZ, out int blockGroupLocalX, out int blockGroupLocalZ)
//        {
//            worldX = worldX & Map.WorldPosition.ChunkBitsZero;
//            blockGroupX = worldX / AbsBlockGroup8.BlockBroupWidth;
//            blockGroupLocalX = worldX - blockGroupX * AbsBlockGroup8.BlockBroupWidth;

//            worldZ = worldZ & Map.WorldPosition.ChunkBitsZero;
//            blockGroupZ = worldZ / AbsBlockGroup8.BlockBroupWidth;
//            blockGroupLocalZ = worldZ - blockGroupZ * AbsBlockGroup8.BlockBroupWidth;
//        }

        

//        HDvoxel.AbsBlockGroup8[,] GetLayer(int y, out int blockGroupLocalY)
//        {
//            y -= blockLayersStartY;

//            int layerIndex = y / AbsBlockGroup8.BlockBroupWidth;

//            blockGroupLocalY = y - layerIndex * AbsBlockGroup8.BlockBroupWidth;

//            if (0 <= layerIndex && layerIndex < groupLayers.Count)
//            {
//                return groupLayers[layerIndex];
//            }

//            return null;
//        }

//    }
//}
