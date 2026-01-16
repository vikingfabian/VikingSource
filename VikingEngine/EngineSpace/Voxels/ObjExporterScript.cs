using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.Voxels;
using VikingEngine.Timer;
using System.IO;
using VikingEngine.DataStream;

namespace VikingEngine.Voxels
{
    class ObjExporterScript
    {
        //private static int StartIndex = 0;

        //public static void Start()
        //{
        //    StartIndex = 0;
        //}
        //public static void End()
        //{
        //    StartIndex = 0;
        //}

        public static FilePath ExportPath(string name)
        {
            return new FilePath(null, name, ".obj", true, false);
        }

        public static void Export(VoxelObjGridDataHD gridData, string name)
        {
            StringBuilder sbVertices = new StringBuilder();
            StringBuilder sbNormals = new StringBuilder();
            StringBuilder sbUVs = new StringBuilder();
            StringBuilder sbFaces = new StringBuilder();
            StringBuilder commaFreeText = new StringBuilder();

            int index = 1; // .obj indices are 1-based
            IntVector3 size = gridData.Size;

            // Directions and normals
            (IntVector3 offset, Vector3 normal)[] directions = new (IntVector3, Vector3)[]
            {
                (new IntVector3(0, 1, 0), new Vector3(0, 1, 0)),     // +Y top
                (new IntVector3(0, -1, 0), new Vector3(0, -1, 0)),   // -Y bottom
                (new IntVector3(0, 0, 1), new Vector3(0, 0, 1)),     // +Z front
                (new IntVector3(0, 0, -1), new Vector3(0, 0, -1)),   // -Z back
                (new IntVector3(1, 0, 0), new Vector3(1, 0, 0)),     // +X right
                (new IntVector3(-1, 0, 0), new Vector3(-1, 0, 0)),   // -X left
            };

            Vector2[] faceUVs = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
            };

            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    for (int z = 0; z < size.Z; z++)
                    {
                        ushort block = gridData.MaterialGrid[x, y, z];
                        if (block == BlockHD.EmptyBlock)
                            continue;

                        Vector3 basePos = new Vector3(x, y, -z); // Z is flipped for OBJ

                        foreach (var (offset, normal) in directions)
                        {
                            IntVector3 neighbor = new IntVector3(x + offset.X, y + offset.Y, z + offset.Z);

                            bool isExposed = !gridData.InBounds(neighbor) ||
                                gridData.MaterialGrid[neighbor.X, neighbor.Y, neighbor.Z] == BlockHD.EmptyBlock;

                            if (isExposed)
                            {
                                // Define quad (2 triangles) for the face
                                Vector3[] faceVerts = VoxelFaceVertices(basePos, offset);

                                foreach (var v in faceVerts)
                                {
                                    sbVertices.AppendFormat("v {0} {1} {2}\n", v.X, v.Y, v.Z);
                                    sbNormals.AppendFormat("vn {0} {1} {2}\n", normal.X, normal.Y, normal.Z);
                                }

                                foreach (var uv in faceUVs)
                                {
                                    sbUVs.AppendFormat("vt {0} {1}\n", uv.X, 1 - uv.Y);
                                }

                                sbFaces.AppendFormat("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", index + 0, index + 1, index + 2);
                                sbFaces.AppendFormat("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", index + 2, index + 1, index + 3);
                                index += 4;
                            }
                        }
                    }
                }
            }

            string file =
                sbVertices.ToString() + Environment.NewLine +
                sbUVs.ToString() + Environment.NewLine +
                sbNormals.ToString() + Environment.NewLine +
                "s off" + Environment.NewLine +
                sbFaces.ToString();

            foreach (char c in file)
            {
                commaFreeText.Append(c == ',' ? '.' : c);
            }

            FilePath path = ExportPath(name);//new FilePath(null, name, ".obj", true, false);

            new AsynchActionTrigger(() =>
            {
                // Write the string array to a new file named "WriteLines.txt".
                using (StreamWriter outputFile = new StreamWriter(path.CompletePath(true)))
                {   
                        outputFile.WriteLine(commaFreeText.ToString());
                }
            }, true);
            

            static Vector3[] VoxelFaceVertices(Vector3 basePos, IntVector3 dir)
            {
                float size = 1f;

                Vector3[] face = new Vector3[4];

                // Depending on the direction, choose a quad face
                if (dir.X == 1) // +X
                {
                    face[0] = basePos + new Vector3(size, 0, 0);
                    face[1] = basePos + new Vector3(size, 1, 0);
                    face[2] = basePos + new Vector3(size, 0, -1);
                    face[3] = basePos + new Vector3(size, 1, -1);
                }
                else if (dir.X == -1) // -X
                {
                    face[0] = basePos + new Vector3(0, 0, -1);
                    face[1] = basePos + new Vector3(0, 1, -1);
                    face[2] = basePos + new Vector3(0, 0, 0);
                    face[3] = basePos + new Vector3(0, 1, 0);
                }
                else if (dir.Y == 1) // +Y
                {
                    face[0] = basePos + new Vector3(0, 1, 0);
                    face[1] = basePos + new Vector3(1, 1, 0);
                    face[2] = basePos + new Vector3(0, 1, -1);
                    face[3] = basePos + new Vector3(1, 1, -1);
                }
                else if (dir.Y == -1) // -Y
                {
                    face[0] = basePos + new Vector3(0, 0, -1);
                    face[1] = basePos + new Vector3(1, 0, -1);
                    face[2] = basePos + new Vector3(0, 0, 0);
                    face[3] = basePos + new Vector3(1, 0, 0);
                }
                else if (dir.Z == 1) // +Z (front face)
                {
                    face[0] = basePos + new Vector3(0, 0, -1);
                    face[1] = basePos + new Vector3(0, 1, -1);
                    face[2] = basePos + new Vector3(1, 0, -1);
                    face[3] = basePos + new Vector3(1, 1, -1);
                }
                else if (dir.Z == -1) // -Z (back face)
                {
                    face[0] = basePos + new Vector3(1, 0, 0);
                    face[1] = basePos + new Vector3(1, 1, 0);
                    face[2] = basePos + new Vector3(0, 0, 0);
                    face[3] = basePos + new Vector3(0, 1, 0);
                }

                return face;
            }
        }


        //public static void Export(List<PolygonNormal> polygons, string name)
        //{
        //    //This method is outdated
        //    //replace List<PolygonNormal> polygons with VoxelObjGridDataHD
        //    //calculate faces by checking is the adjacent block is empty

        //    StringBuilder sbVertices = new StringBuilder();
        //    StringBuilder sbNormals = new StringBuilder();
        //    StringBuilder sbUVs = new StringBuilder();
        //    StringBuilder sbFaces = new StringBuilder();

        //    int index = 1;

        //    foreach (PolygonNormal poly in polygons)
        //    {
        //        sbFaces.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
        //            index + 0, index + 1, index + 2));
        //        sbFaces.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
        //            index + 2, index + 1, index + 3));

        //        index += 4;


        //        foreach (var v in poly.VerticeData)
        //        {
        //            sbVertices.Append(string.Format("v {0} {1} {2}\n", v.Position.X, v.Position.Y, -v.Position.Z));

        //            sbNormals.Append(string.Format("vn {0} {1} {2}\n", v.Normal.X, v.Normal.Y, v.Normal.Z));

        //            sbUVs.Append(string.Format("vt {0} {1}\n", v.TextureCoordinate.X, 1 - v.TextureCoordinate.Y));
        //        }
        //    }

        //    string file =
        //        sbVertices.ToString() + Environment.NewLine +
        //        sbUVs.ToString() + Environment.NewLine +
        //        sbNormals.ToString() + Environment.NewLine +
        //        "s off" + Environment.NewLine +
        //        sbFaces.ToString();

        //    //Ta bort kommas
        //    //StringBuilder commaFreeText = new StringBuilder();
        //    foreach (char c in file)
        //    {
        //        if (c == ',')
        //        {
        //            commaFreeText.Append('.');
        //        }
        //        else
        //        {
        //            commaFreeText.Append(c);
        //        }
        //    }


        //    DataStream.FilePath path = new FilePath(null, name, ".obj", true, false);
        //    new DataLib.TextFileToStorage(true, path.CompletePath(true),
        //        commaFreeText.ToString(),
        //        null, DataLib.ThreadType.SaveOnly);
        //}


        //static string MeshToString(List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs)
        //{
            

        //    StringBuilder sb = new StringBuilder();

        //    foreach (Vector3 vv in vertices)
        //    {
        //        sb.Append(string.Format("v {0} {1} {2}\n", vv.X, vv.Y, -vv.Z));
        //    }
        //    sb.Append("\n");
        //    foreach (Vector3 nn in normals)
        //    {
        //        //Vector3 v = r * nn;
        //        sb.Append(string.Format("vn {0} {1} {2}\n", -nn.X, -nn.Y, nn.Z));
        //    }
        //    sb.Append("\n");
        //    foreach (Vector2 v in uvs)
        //    {
        //        sb.Append(string.Format("vt {0} {1}\n", v.X, v.Y));
        //    }
          
        //    return sb.ToString();
        //}
    }
}
