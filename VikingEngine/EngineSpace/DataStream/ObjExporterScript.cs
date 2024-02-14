//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System.Text;
//using VikingEngine.Graphics;

//namespace VikingEngine.DataStream
//{
//    class ObjExporterScript
//    {
//        //private static int StartIndex = 0;

//        //public static void Start()
//        //{
//        //    StartIndex = 0;
//        //}
//        //public static void End()
//        //{
//        //    StartIndex = 0;
//        //}
//        public static void Export(List<PolygonNormal> polygons, string name)
//        {
//            StringBuilder sbVertices = new StringBuilder();
//            StringBuilder sbNormals = new StringBuilder();
//            StringBuilder sbUVs = new StringBuilder();
//            StringBuilder sbFaces = new StringBuilder();

//            int index = 1;

//            foreach (PolygonNormal poly in polygons)
//            {
//                sbFaces.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
//                    index + 0, index + 1, index + 2));
//                sbFaces.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
//                    index + 2, index + 1, index + 3));

//                index += 4;


//                foreach (var v in poly.VerticeData)
//                {
//                    sbVertices.Append(string.Format("v {0} {1} {2}\n", v.Position.X, v.Position.Y, -v.Position.Z));

//                    sbNormals.Append(string.Format("vn {0} {1} {2}\n", v.Normal.X, v.Normal.Y, v.Normal.Z));

//                    sbUVs.Append(string.Format("vt {0} {1}\n", v.TextureCoordinate.X, 1 - v.TextureCoordinate.Y));
//                }
//            }

//            string file = 
//                sbVertices.ToString() + Environment.NewLine +
//                sbUVs.ToString() + Environment.NewLine + 
//                sbNormals.ToString() + Environment.NewLine + 
//                "s off" +  Environment.NewLine +
//                sbFaces.ToString();

//            //Ta bort kommas
//            StringBuilder commaFreeText = new StringBuilder();
//            foreach (char c in file)
//            {
//                if (c == ',')
//                {
//                    commaFreeText.Append('.');
//                }
//                else
//                {
//                    commaFreeText.Append(c);
//                }
//            }


//            DataStream.FilePath path = new FilePath(null, name, ".obj", true, false);
//            new DataLib.TextFileToStorage(true, path.CompletePath(true),
//                commaFreeText.ToString(), 
//                null, DataLib.ThreadType.SaveOnly);
//        }


//        //static string MeshToString(List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs)
//        //{
//        //    //Vector3 s = t.localScale;
//        //    //Vector3 p = t.localPosition;
//        //    //Quaternion r = t.localRotation;


//        //    //int numVertices = 0;
//        //    //Mesh m = mf.sharedMesh;
//        //    //if (!m)
//        //    //{
//        //    //    return "####Error####";
//        //    //}
//        //    //Material[] mats = mf.renderer.sharedMaterials;

//        //    StringBuilder sb = new StringBuilder();

//        //    foreach (Vector3 vv in vertices)
//        //    {
//        //        //Vector3 v = t.TransformPoint(vv);
//        //        //numVertices++;
//        //        sb.Append(string.Format("v {0} {1} {2}\n", vv.X, vv.Y, -vv.Z));
//        //    }
//        //    sb.Append("\n");
//        //    foreach (Vector3 nn in normals)
//        //    {
//        //        //Vector3 v = r * nn;
//        //        sb.Append(string.Format("vn {0} {1} {2}\n", -nn.X, -nn.Y, nn.Z));
//        //    }
//        //    sb.Append("\n");
//        //    foreach (Vector2 v in uvs)
//        //    {
//        //        sb.Append(string.Format("vt {0} {1}\n", v.X, v.Y));
//        //    }
//        //    //for (int material = 0; material < m.subMeshCount; material++)
//        //    //{
//        //    //    sb.Append("\n");
//        //    //    sb.Append("usemtl ").Append(mats[material].name).Append("\n");
//        //    //    sb.Append("usemap ").Append(mats[material].name).Append("\n");

//        //    //    int[] triangles = m.GetTriangles(material);
//        //    //    for (int i = 0; i < triangles.Length; i += 3)
//        //    //    {
//        //    //        sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
//        //    //            triangles[i] + 1 + StartIndex, triangles[i + 1] + 1 + StartIndex, triangles[i + 2] + 1 + StartIndex));
//        //    //    }
//        //    //}

//        //    //StartIndex += numVertices;
//        //    return sb.ToString();
//        //}
//    }
//}
