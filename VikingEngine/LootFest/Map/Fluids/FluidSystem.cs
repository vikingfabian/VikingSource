//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using VikingEngine.Graphics;

//namespace VikingEngine.LootFest.Map.Fluids
//{
//    class FluidSystem
//    {
//        const int Empty = 0;
//        const int FullValue = 100;
//        const int ParticleSpawner = -1;


//        public FluidType type;
//        public int[, ,] DataGrid = null;
//        public Chunk chunk;

//        VoxelModel model;

//        public FluidSystem(FluidType type, Chunk chunk)
//        {
//            this.type = type;
//            DataGrid = new int[WorldPosition.ChunkWidth, WorldPosition.ChunkHeight, WorldPosition.ChunkWidth];
//            this.chunk = chunk;
//        }

//        public void fillUpChunk(int height)
//        {
//            IntVector3 pos = IntVector3.Zero;

//            //for (pos.Z = 0; pos.Z < Map.WorldPosition.ChunkWidth; ++pos.Z)
//            //{
//            //    for (pos.X = 0; pos.X < Map.WorldPosition.ChunkWidth; ++pos.X)
//            //    {
//            //        for (pos.Y = height; pos.Y >= 0; --pos.Y)
//            //        {
//            //            if (chunk.DataGrid[pos.X, pos.Y, pos.Z] == byte.MinValue)
//            //            {
//            //                DataGrid[pos.X, pos.Y, pos.Z] = FullValue;
//            //            }
//            //        }
//            //    }
//            //}
//        }

//        public void generateMesh()
//        {
//            Graphics.Sprite imgfileTop = LootFest.LfRef.Images.TileIxToImgeFile[VikingEngine.DataLib.SpriteCollection.imagesNames[SpriteName.LfGranpaHead1]];
//            List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>(64);

//            IntVector3 pos = IntVector3.Zero;
//            bool hasValueBelow = false;

//            for (pos.Z = 0; pos.Z < Map.WorldPosition.ChunkWidth; ++pos.Z)
//            {
//                for (pos.X = 0; pos.X < Map.WorldPosition.ChunkWidth; ++pos.X)
//                {
//                    for (pos.Y = 0; pos.Y <  Map.WorldPosition.ChunkHeight; ++pos.Y)
//                    {
//                        if (DataGrid[pos.X, pos.Y, pos.Z] > 0)
//                        {
//                            hasValueBelow = true;
//                        }
//                        else
//                        {
//                            if (hasValueBelow)
//                            {
//                                Vector3 topLeft = pos.Vec;
//                                Vector3 topRight = topLeft; topRight.X += 1;
//                                Vector3 lowLeft = topLeft; lowLeft.Z += 1;
//                                Vector3 lowRight = lowLeft; lowRight.X += 1;

//                                Graphics.PolygonColor top = new Graphics.PolygonColor();
//                                top.Vertex1nw = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(topLeft, Color.White, imgfileTop.SourcePolygonTopLeft);
//                                top.Vertex0sw = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(lowLeft, Color.White, imgfileTop.SourcePolygonLowLeft);
//                                top.Vertex3ne = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(topRight, Color.White, imgfileTop.SourcePolygonTopRight);
//                                top.Vertex2se = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(lowRight, Color.White, imgfileTop.SourcePolygonLowRight);
//                                polygons.Add(top);
//                            }
//                            hasValueBelow = false;
//                        }
//                    }
//                }
//            }

//            WorldPosition wp = new WorldPosition(chunk.Index);

//            model = new VoxelModel(false);
//            model.BuildFromPolygons(new PolygonsAndTrianglesColor( polygons, null), new List<int> { polygons.Count }, LoadedTexture.LF3Tiles);
//            //model.Effect = Graphics.VoxelObjEffectWater.Instance;
//            model.position = wp.PositionV3;

//            new Timer.Action0ArgTrigger(addModel);
//        }

//        void addModel()
//        {
//            model.AddToRender();
//        }
//    }
//}
