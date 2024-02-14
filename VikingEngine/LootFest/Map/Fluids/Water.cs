//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

//namespace VikingEngine.LootFest.Map.Fluids
//{
//    class Water
//    {
//        public Water(WorldPosition wp)
//        {
//            wp.Y += 5;


//            Graphics.ImageFile2 imgfileTop =  LootFest.LfRef.Images.TileIxToImgeFile[VikingEngine.DataLib.Images.imagesNames[SpriteName.TextureWaterMap]];
//           // Graphics.ImageFile2 imgfileSide = LootFest.LootfestLib.Images.TileIxToImgeFile[Data.MaterialBuilder.Materials[(byte)Data.MaterialType.dark_blue].TopTiles.startTile];
//            Graphics.PolygonsAndTrianglesColor polygons = new Graphics.PolygonsAndTrianglesColor(new List<Graphics.PolygonColor>());

//            const int Width = 64;
//            IntVector3 pos = IntVector3.Zero;

//            for (pos.Z = 0; pos.Z < Width; ++pos.Z)
//            {
//                for (pos.X = 0; pos.X < Width; ++pos.X)
//                {
//                    Color faceCol = new Color((byte)pos.X, (byte)pos.Z, byte.MaxValue);
//                    Color rightSideCol = new Color((byte)(pos.X + 1), (byte)pos.Z, byte.MaxValue);
//                    Color lowSideCol = new Color((byte)pos.X, (byte)(pos.Z + 1), byte.MaxValue);

//                    Vector3 topLeft = pos.Vec;
//                    Vector3 topRight = topLeft; topRight.X += 1;
//                    Vector3 lowLeft = topLeft; lowLeft.Z += 1;
//                    Vector3 lowRight = lowLeft; lowRight.X += 1;
//                    //Top
//                    Graphics.PolygonColor top = new Graphics.PolygonColor();
//                    top.Vertex1 = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(topLeft, faceCol, imgfileTop.SourcePolygonTopLeft);
//                    top.Vertex0 = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(lowLeft, faceCol, imgfileTop.SourcePolygonLowLeft);
//                    top.Vertex3 = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(topRight, faceCol, imgfileTop.SourcePolygonTopRight);
//                    top.Vertex2 = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(lowRight, faceCol, imgfileTop.SourcePolygonLowRight);
//                    polygons.Polygons.Add(top);

//                    //right
//                    Graphics.PolygonColor right = new Graphics.PolygonColor();
//                    right.Vertex1 = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(topRight, faceCol, imgfileTop.SourcePolygonTopLeft);
//                    right.Vertex0 = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(lowRight, faceCol, imgfileTop.SourcePolygonLowLeft);
//                    right.Vertex3 = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(topRight, rightSideCol, imgfileTop.SourcePolygonTopRight);
//                    right.Vertex2 = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(lowRight, rightSideCol, imgfileTop.SourcePolygonLowRight);
//                    polygons.Polygons.Add(right);

//                    //low
//                    Graphics.PolygonColor low = new Graphics.PolygonColor();
//                    low.Vertex1 = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(lowLeft, faceCol, imgfileTop.SourcePolygonTopLeft);
//                    low.Vertex0 = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(lowLeft, lowSideCol, imgfileTop.SourcePolygonLowLeft);
//                    low.Vertex3 = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(lowRight, faceCol, imgfileTop.SourcePolygonTopRight);
//                    low.Vertex2 = new Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture(lowRight, lowSideCol, imgfileTop.SourcePolygonLowRight);
//                    polygons.Polygons.Add(low);

//                }
//            }

//            Ref.draw.CurrentRenderLayer = 1;
//            Graphics.VoxelObj model = new Graphics.VoxelObj(true);
//            model.BuildFromPolygons(polygons, LoadedTexture.NO_TEXTURE);
//            model.Effect = Graphics.VoxelObjEffectWater.Instance;
//            model.Position = wp.PositionV3;
//            model.AlwaysInCameraCulling = true;
//            Ref.draw.CurrentRenderLayer = 0;
//        }
//    }
//}
