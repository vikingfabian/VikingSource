//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.EngineSpace.Engine;
//using Game1.EngineSpace.Graphics;
//using Game1.EngineSpace.Graphics.In3D.Generated;
//using Game1.GameSpace;

//namespace Game1.LF2.Map
//{
//    class World
//    {
//        EngineSpace.Graphics.In3D.Generated.LFHeightMap mesh;
//        TerrainData terrainData;

//        public World()
//        {
//            //generate a simple map
//            Data.Block.Init();
//            PreparedFace.Init();

//            mesh = new EngineSpace.Graphics.In3D.Generated.LFHeightMap();
//            //createCube();
//            terrainData = new TerrainData();
            
//            Range xrange =new Range(1, TerrainData.SizeX -1);
//            Range yrange =new Range(1, TerrainData.SizeY -1);
//            Range zrange =new Range(1, TerrainData.SizeZ -1);
//            generateTerrain(new PreparedTerrain(terrainData.Blocks, xrange, yrange, zrange));
            
//        }
//        void createCube()
//        {
//            //PolygonNormal testplane = new PolygonNormal(Data.Block.GetTerrainFace(Vector3.Zero, Data.CubeFace.Top), 0);
//            //PolygonNormal testplane2 = new PolygonNormal(Data.Block.GetTerrainFace(Vector3.Zero, Data.CubeFace.Bottom), 0);
//            //PolygonsAndTrianglesNormal polys = new PolygonsAndTrianglesNormal(new List<PolygonNormal>{ testplane, testplane2 }, new List<TriangleNormal>());
//            //mesh.BuildFromPolygons(polys, LoadedTexture.LF_TileSheet);
//        }
//        int randomMaterialTile(byte type, bool side)
//        {
//            if (side)
//                return Data.MaterialBuilder.Materials[type].SideTiles.GetRandomTile();
//            else
//                return Data.MaterialBuilder.Materials[type].TopTiles.GetRandomTile();
//        }
//        void generateTerrain(PreparedTerrain preparedTerrain)
//        {
//            PolygonsAndTrianglesColor polys = new PolygonsAndTrianglesColor(new List<PolygonColor>(), new List<TriangleColor>());

//            foreach (PreparedBlock block in preparedTerrain.FaceBlocks)
//            {
//                block.GenerateMesh(polys);
//            }
//            mesh.BuildFromPolygons(polys, LoadedTexture.LF_TileSheet);



//            //const byte EmptyBlock = 0;
//            //Range xrange = new Range(1, TerrainData.SizeX - 1);
//            //Range yrange = new Range(1, TerrainData.SizeY - 1);
//            //Range zrange = new Range(1, TerrainData.SizeZ - 1);


//            //for (int z = zrange.Min; z < zrange.Max; z++)
//            //{
//            //    for (int y = yrange.Min; y < yrange.Max; y++)
//            //    {
//            //        for (int x = zrange.Min; x < zrange.Max; x++)
//            //        {
//            //            if (terrainData.Blocks[x, y, z] != EmptyBlock)
//            //            {
//            //                byte material = terrainData.Blocks[x, y, z];
//            //                Vector3 pos = new Vector3(x, y, z);
//            //                //check top
//            //                if (terrainData.Blocks[x, y + 1, z] == EmptyBlock)
//            //                {
//            //                    polys.Polygons.Add(new PolygonColor(Data.Block.GetTerrainFace(pos, Data.CubeFace.Top),
//            //                        randomMaterialTile(material, false), Color.White));
//            //                }
//            //                //check front
//            //                if (terrainData.Blocks[x, y, z + 1] == EmptyBlock)
//            //                {
//            //                    polys.Polygons.Add(new PolygonColor(Data.Block.GetTerrainFace(pos, Data.CubeFace.Front),
//            //                        randomMaterialTile(material, true), Color.White));
//            //                }
//            //                //check back
//            //                if (terrainData.Blocks[x, y, z - 1] == EmptyBlock)
//            //                {
//            //                    polys.Polygons.Add(new PolygonColor(Data.Block.GetTerrainFace(pos, Data.CubeFace.Back),
//            //                        randomMaterialTile(material, true), Color.DarkGray));
//            //                }
//            //                //check right
//            //                if (terrainData.Blocks[x - 1, y, z] == EmptyBlock)
//            //                {
//            //                    polys.Polygons.Add(new PolygonColor(Data.Block.GetTerrainFace(pos, Data.CubeFace.Right),
//            //                        randomMaterialTile(material, true), Color.DarkGray));
//            //                }
//            //                //check left
//            //                if (terrainData.Blocks[x + 1, y, z] == EmptyBlock)
//            //                {
//            //                    polys.Polygons.Add(new PolygonColor(Data.Block.GetTerrainFace(pos, Data.CubeFace.Left),
//            //                        randomMaterialTile(material, true), Color.White));
//            //                }
//            //            }
//            //        }
//            //    }
//            //}
//            //mesh.BuildFromPolygons(polys, LoadedTexture.LF_TileSheet);
            
//        }
//    }
//}
