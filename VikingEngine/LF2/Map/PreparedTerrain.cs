//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace Game1.LootFest.Map
//{
//    //ska ta ut dom block som ska renderas
//    //räkna ut vilka sidor som blocket har
//    //beräkna skuggor och ljus
   

//    class PreparedTerrain
//    {
//        static readonly IPreparedBlock Empty = new EmptyBlock();
//        IPreparedBlock[, ,] blockGrid;
//        public List<PreparedBlock> FaceBlocks = new List<PreparedBlock>();

//        public PreparedTerrain(byte[, ,] data, Range xrange, Range yrange, Range zrange)
//        {
//            const byte EmptyBlock = 0;
//            blockGrid = new IPreparedBlock[xrange.Difference, yrange.Difference, zrange.Difference];

//            IntVecor3 blockGridPointer = IntVecor3.Zero;

//            for (int z = zrange.Min; z < zrange.Max; z++)
//            {
//                for (int y = yrange.Min; y < yrange.Max; y++)
//                {
//                    for (int x = zrange.Min; x < zrange.Max; x++)
//                    {

//                        if (data[x, y, z] != EmptyBlock)
//                        {
//                            bool shadowSide = false;
//                            List<PreparedFace> faces = new List<PreparedFace>();
//                            byte material = data[x, y, z];

//                            //check top
//                            if (data[x, y + 1, z] == EmptyBlock)
//                            {
//                                faces.Add(new PreparedFace(Data.CubeFace.Top, material));
//                            }
//                            //check bottom
//                            if (data[x, y - 1, z] == EmptyBlock)
//                            {
//                                shadowSide = true;
//                            }
//                            //check front
//                            if (data[x, y, z + 1] == EmptyBlock)
//                            {
//                                faces.Add(new PreparedFace(Data.CubeFace.Front, material));
//                            }
//                            //check back
//                            if (data[x, y, z - 1] == EmptyBlock)
//                            {
//                                faces.Add(new PreparedFace(Data.CubeFace.Back, material));
//                                shadowSide = true;
//                            }
//                            //check right
//                            if (data[x - 1, y, z] == EmptyBlock)
//                            {
//                                faces.Add(new PreparedFace(Data.CubeFace.Right, material));
//                            }
//                            //check left
//                            if (data[x + 1, y, z] == EmptyBlock)
//                            {
//                                faces.Add(new PreparedFace(Data.CubeFace.Left, material));
//                                shadowSide = true;
//                            }
//                            if (faces.Count > 0)
//                            {
//                                PreparedBlock block = new PreparedBlock(faces, shadowSide, x, y, z);
//                                FaceBlocks.Add(block);
//                                blockGrid[blockGridPointer.X, blockGridPointer.Y, blockGridPointer.Z] = block;
//                            }
//                            else
//                            {
//                                //all faces hidden
//                                blockGrid[blockGridPointer.X, blockGridPointer.Y, blockGridPointer.Z] = Empty;
//                            }

//                        }
//                        else
//                        {
//                            //empty
//                            blockGrid[blockGridPointer.X, blockGridPointer.Y, blockGridPointer.Z] = Empty;
//                        }
//                        blockGridPointer.X++;
//                    }
//                    blockGridPointer.Y++;
//                    blockGridPointer.X = 0;
//                }
//                blockGridPointer.Z++;
//                blockGridPointer.X = 0;
//                blockGridPointer.Y = 0;
//            }
            
//        }

//        void CalculateShadows()
//        {
//            foreach (PreparedBlock block in FaceBlocks)
//            {
//                if (block.ShadowSide)
//                { //this block will cast a shadow

//                }
//            }
//        }
//    }
//}
