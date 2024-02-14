using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Voxels;

namespace VikingEngine.LF2.Editor
{
    static class VoxelObjDataLoader
    {
        public const int StandardBlockWitdh = 16;
        public const int StandardHalfBlockWitdh = StandardBlockWitdh / PublicConstants.Twice;
        public static readonly IntVector3 StandardLimits = new IntVector3(StandardBlockWitdh - 1, 23, StandardBlockWitdh -1);

        public static Graphics.VoxelObjAnimated GetVoxelObjAnim(VoxelObjName name)
        {
            return GetVoxelObjAnim(name, Vector3.Zero);
        }
        public static Graphics.VoxelObjAnimated GetVoxelObjAnimWithColReplace(VoxelObjName name, Vector3 centerAdjust, List<byte> find, List<byte> replace)
        {
            //skapa ett voxelObj som kan hantera frames
            List<VoxelObjGridData> loadedFrames = LoadObj(name, find, replace);
            List<List<Voxel>> voxels = new List<List<Voxel>>();
            foreach (VoxelObjGridData data in loadedFrames)
            {
                voxels.Add(data.GetVoxelArray());
            }
            return VoxelObjBuilder.BuildAnimatedFromVoxels(new RangeIntV3(IntVector3.Zero, loadedFrames[0].Limits), voxels, loadedFrames[0].BottomCenterAdj() + centerAdjust);
        }

        //public static Graphics.VoxelModel GetVoxelObjMaster(VoxelObjName name, Vector3 centerAdjust)
        //{
        //    //skapa ett voxelObj som kan hantera frames
        //    List<VoxelObjGridData> loadedFrames = LoadVoxelObjGrid(name);
        //    List<List<Voxel>> voxels = new List<List<Voxel>>();
        //    foreach (VoxelObjGridData data in loadedFrames)
        //    {
        //        voxels.Add(data.GetVoxelArray());
        //    }

        //    centerAdjust += loadedFrames[0].BottomCenterAdj();

        //    if (loadedFrames.Count == 1)
        //    {
        //        return VoxelObjBuilder.BuildFromVoxels(loadedFrames[0].Limits, voxels[0], centerAdjust);
        //    }
        //    return VoxelObjBuilder.BuildAnimatedFromVoxels(new RangeIntV3(IntVector3.Zero, loadedFrames[0].Limits), voxels, centerAdjust);
        //}

        public static Graphics.VoxelObjAnimated GetVoxelObjAnim(VoxelObjName name, Vector3 centerAdjust)
        {
            //skapa ett voxelObj som kan hantera frames
            List<VoxelObjGridData> loadedFrames = LoadVoxelObjGrid(name);
            List<List<Voxel>> voxels = new List<List<Voxel>>();
            foreach (VoxelObjGridData data in loadedFrames)
            {
                voxels.Add(data.GetVoxelArray());
            }
            Graphics.VoxelObjAnimated result = VoxelObjBuilder.BuildAnimatedFromVoxels(new RangeIntV3(IntVector3.Zero, 
                loadedFrames[0].Limits), voxels, loadedFrames[0].BottomCenterAdj() + centerAdjust);
            result.SetOneScale(loadedFrames[0].Limits);
            return result;
        }
        public static Graphics.VoxelObj GetVoxelObj(VoxelObjName name, bool addToRender)
        {
            return GetVoxelObj(name, addToRender, CenterAdjType.BottomCenter, Vector3.Zero);
        }
        public static Graphics.VoxelObj GetVoxelObj(VoxelObjName name, bool addToRender, CenterAdjType centerType, Vector3 centerAdjust)
        {
            //skapa ett voxelObj som kan hantera frames
            VoxelObjGridData data = LoadVoxelObjGrid(name)[0];

            switch (centerType)
            {
                case CenterAdjType.BottomCenter:
                    centerAdjust += data.BottomCenterAdj();
                    break;
                case CenterAdjType.CenterAll:
                    centerAdjust += data.CenterAdj();
                    break;
            }

            Graphics.VoxelObj result =
                VoxelObjBuilder.BuildFromVoxelGrid2(data, centerAdjust);
            if (addToRender)
            {
                Ref.draw.AddToRenderList(result, true);
            }
            result.SetOneScale(data.Limits);
            return result;
        }

        //public static Graphics.VoxelObj[] GetVoxelObjMirrored(VoxelObjName name, bool addToRender, CenterAdjType centerType, Vector3 centerAdjust)
        //{
        //    VoxelObjGridData data = LoadVoxelObjGrid(name)[0];

        //    RangeIntV3 limit = new RangeIntV3(IntVector3.Zero, data.Limits);

        //    switch (centerType)
        //    {
        //        case CenterAdjType.BottomCenter:
        //            centerAdjust += data.BottomCenterAdj();
        //            break;
        //        case CenterAdjType.CenterAll:
        //            centerAdjust += data.CenterAdj();
        //            break;
        //    }

        //    VoxelObjGridData mirrored = data.Clone();
        //    mirrored.FlipDir(Dimentions.X, limit);

        //    Graphics.VoxelObj result1 =
        //        VoxelObjBuilder.BuildFromVoxels(limit, data.GetVoxelArray(), centerAdjust);
        //    Graphics.VoxelObj result2 =
        //        VoxelObjBuilder.BuildFromVoxels(limit, mirrored.GetVoxelArray(), centerAdjust);
        //    if (!addToRender)
        //    {
        //        result1.DeleteMe();
        //        result2.DeleteMe();

        //    }
        //    result1.SetOneScale(data.Limits);
        //    result2.SetOneScale(data.Limits);
        //    return new Graphics.VoxelObj[] {  result1, result2 };

        //}

        public static VoxelObjListData VoxelObjListData(VoxelObjName name)
        {
            return new VoxelObjListData(Editor.VoxelObjDataLoader.LoadVoxelObjGrid(name)[0].GetVoxelArray());

        }
        //public static VoxelObjListDataAnim VoxelObjListDataAnimated(VoxelObjName name)
        //{
        //    List<VoxelObjGridData> grids = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(name);
        //    return new VoxelObjListDataAnim(grids);
        //}
        //public static VoxelObjListData VoxelObjListDataCentered(VoxelObjName name)
        //{
        //    return new VoxelObjListData(Editor.VoxelObjDataLoader.LoadVoxelObjGrid(name)[0].GetVoxelArrayCentered());
        
        //}
        //public static Graphics.VoxelObjAnimated GetVoxelObjAnimWithMReplace(VoxelObjName name,
        //    Data.MaterialType find, Data.MaterialType replace)
        //{

        //    List<VoxelObjGridData> loadedFrames = LoadVoxelObjGrid(name);
        //    List<List<Voxel>> voxels = new List<List<Voxel>>();
        //    foreach (VoxelObjGridData data in loadedFrames)
        //    {
        //        List<Voxel> f = data.GetVoxelArray();
        //        ReplaceMaterial(ref f, find, replace);
        //        voxels.Add(f);
        //    }

        //    return VoxelObjBuilder.BuildAnimatedFromVoxels(new RangeIntV3(IntVector3.Zero, loadedFrames[0].Limits), voxels, loadedFrames[0].BottomCenterAdj());
        //}
        //public static Graphics.VoxelObj GetVoxelObjWithMReplace(VoxelObjName name,
        //    Data.MaterialType find, Data.MaterialType replace)
        //{
        //    VoxelObjGridData data = LoadVoxelObjGrid(name)[0];
        //    List<Voxel> voxels = data.GetVoxelArray();
        //    ReplaceMaterial(ref voxels, find, replace);
        //    return VoxelObjBuilder.BuildFromVoxels(new RangeIntV3(IntVector3.Zero, data.Limits), voxels, data.BottomCenterAdj());
        //}
        //public static void ReplaceMaterial(ref List<Voxel> voxels, Data.MaterialType find, Data.MaterialType replaceWith)
        //{
        //    byte bfind = (byte)find;

        //    for (int i = 0; i < voxels.Count; i++)
        //    {
        //        if (voxels[i].Material == bfind)
        //        {

        //            voxels[i] = new Voxel(voxels[i].Position, (byte)replaceWith);
        //        }
        //    }
        //}

        public static List<VoxelObjGridData> LoadObjFromFile(List<string> textFile, IntVector3 limits, SaveVersion version)
        {
            List<VoxelObjGridData> result = new List<VoxelObjGridData>();
            

            for (int frame = 0; frame < textFile.Count; frame++)
            {
                byte[, ,] materialGrid = new byte[limits.X + 1, limits.Y + 1, limits.Z + 1];
                List<int> materialArray;
                switch (version)
                {
                    default:
                        materialArray = lib.StingIntDimentions(textFile[frame]);
                        break;
                    case SaveVersion.Ver2:
                        materialArray = lib.UncompressStingIntDimentionsVer2(textFile[frame]);
                        break;
                }

                IntVector3 pos = IntVector3.Zero;
                int index = 0;
                for (pos.Z = 0; pos.Z <= limits.Z; pos.Z++)
                {
                    for (pos.Y = 0; pos.Y <= limits.Y; pos.Y++)
                    {
                        for (pos.X = 0; pos.X <= limits.X; pos.X++)
                        {
                            materialGrid[pos.X, pos.Y, pos.Z] = (byte)materialArray[index];
                            index++;
                        }
                    }
                }
                result.Add(new VoxelObjGridData(materialGrid));
            }
            return result;
        }
        public static DataStream.FilePath ContentPath(VoxelObjName name)
        {
            //const string ContentFolder = "Lootfest\\Data\\VoxelObj";
            return new DataStream.FilePath(LootfestLib.VoxelModelFolder, name.ToString(), 
                Data.Images.VoxelObjByteArrayEnding, false);
        }

        public static List<VoxelObjGridData> LoadObj(VoxelObjName name, List<byte> find, List<byte> replace)
        {
            VoxelObjGridDataAnimWithColReplace result = new VoxelObjGridDataAnimWithColReplace();
            result.Save(false, ContentPath(name), false, null, find, replace);
            return result.Frames;
        }

        /// <summary>
        /// Load a player custom made animated obj 
        /// </summary>
        public static List<VoxelObjGridData> LoadVoxelObjGrid(string name)
        {
            VoxelObjGridDataAnim result = new VoxelObjGridDataAnim();
            result.Save(false, Editor.VoxelDesigner.CustomVoxelObjPath(name));

            return result.Frames;
        }
        public static List<VoxelObjGridData> LoadVoxelObjGrid(System.IO.BinaryReader r)
        {
            VoxelObjGridDataAnim result = new VoxelObjGridDataAnim();
            result.ReadBinaryStream(r);//Path(name), false);
            return result.Frames;
        }

        //public static Graphics.VoxelObjAnimated GetVoxelObjAnim(string name, Vector3 centerAdjust)
        //{
        //    //skapa ett voxelObj som kan hantera frames
        //    List<VoxelObjGridData> loadedFrames = LoadVoxelObjGrid(name);
        //    if (loadedFrames == null)
        //        return null;
        //    List<List<Voxel>> voxels = new List<List<Voxel>>();
        //    foreach (VoxelObjGridData data in loadedFrames)
        //    {
        //        voxels.Add(data.GetVoxelArray());
        //    }
        //    Graphics.VoxelObjAnimated result = VoxelObjBuilder.BuildAnimatedFromVoxels(
        //        new RangeIntV3(IntVector3.Zero, loadedFrames[0].Limits), voxels, loadedFrames[0].BottomCenterAdj() + centerAdjust);
        //    result.SetOneScale(loadedFrames[0].Limits);
        //    return result;
        //}
        public static Graphics.VoxelObjAnimated GetVoxelObjAnim(System.IO.BinaryReader r, Vector3 centerAdjust)
        {
            List<VoxelObjGridData> loadedFrames = LoadVoxelObjGrid(r);
            if (loadedFrames == null) return null; //Err corrupt file

            Graphics.VoxelObjAnimated result = VoxelObjBuilder.BuildAnimatedFromVoxelGrid2(
                loadedFrames, loadedFrames[0].BottomCenterAdj() + centerAdjust);

            result.SetOneScale(loadedFrames[0].Limits);
            return result;
        }

        public static Graphics.VoxelObj GetVoxelObj(System.IO.BinaryReader r, Vector3 centerAdjust)
        {
            VoxelObjGridData loadedFrame = LoadVoxelObjGrid(r)[0];
            if (loadedFrame == null) return null; //Err corrupt file

            //List<Voxel> voxels = loadedFrame.GetVoxelArray(); 

            Graphics.VoxelObj result = VoxelObjBuilder.BuildFromVoxelGrid2(loadedFrame, loadedFrame.BottomCenterAdj() + centerAdjust);
                //VoxelObjBuilder.BuildFromVoxels(
               // new RangeIntV3(IntVector3.Zero, loadedFrame.Limits), voxels, loadedFrame.BottomCenterAdj() + centerAdjust);
            result.SetOneScale(loadedFrame.Limits);
            return result;
        }


        public static Graphics.VoxelModel GetVoxelObjMaster(System.IO.BinaryReader r, Vector3 centerAdjust)
        {
            //skapa ett voxelObj som kan hantera frames
            List<VoxelObjGridData> loadedFrames = LoadVoxelObjGrid(r);
           
            centerAdjust += loadedFrames[0].BottomCenterAdj();
            Graphics.VoxelModel result;
            if (loadedFrames.Count == 1)
            {
                result = VoxelObjBuilder.BuildFromVoxelGrid2(loadedFrames[0], centerAdjust);//BuildFromVoxels(loadedFrames[0].Limits, voxels[0], centerAdjust);
            }
            else
            {
                result = VoxelObjBuilder.BuildAnimatedFromVoxelGrid2(loadedFrames, centerAdjust);//BuildAnimatedFromVoxels(new RangeIntV3(IntVector3.Zero, loadedFrames[0].Limits), voxels, centerAdjust);
            }
            result.SetOneScale(loadedFrames[0].Limits);
            return result;
        }


       
        public static List<VoxelObjGridData> LoadVoxelObjGrid(VoxelObjName name)
        {
            //List<string> data;
            //IntVector3 limits = new IntVector3(15, 23, 15);
            //SaveVersion version = SaveVersion.Ver1;

            VoxelObjGridDataAnim result = new VoxelObjGridDataAnim();
            result.Save(false, ContentPath(name));
            return result.Frames;



            //switch (name)
            //{
            //    default:
            //        VoxelObjGridDataAnim result = new VoxelObjGridDataAnim();
            //        result.Save(false, ContentPath(name));
            //        return result.Frames;

            //    case VoxelObjName.NUM_Empty:
            //        throw new Exception("Empty object load");
                
            //    //case VoxelObjName.EnemyBlock:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{
            //    //        "0_1172_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_200_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_1_6_2_0_5_6_8_0_1_6_2_0_5_6_10_0_6_6_9_0_7_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_1_6_2_0_2_6_2_0_1_6_8_0_1_6_2_0_3_6_12_0_5_6_10_0_7_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_5_6_2_0_1_6_8_0_6_6_10_0_7_6_9_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_216_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_17_1_6_4_17_1_6_1_0_8_6_2_17_4_6_2_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_29_1_17_1_6_2_17_1_29_1_6_1_0_8_6_1_29_2_6_2_29_2_6_1_0_234_6_4_0_12_6_4_0_60_6_4_0_12_6_4_0_12_6_4_0_13_6_2_0_14_6_2_0_317_6_4_0_12_6_4_0_12_6_4_0_998",
            //    //        "0_1172_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_200_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_1_6_2_0_5_6_8_0_1_6_2_0_5_6_10_0_6_6_9_0_7_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_1_6_2_0_5_6_8_0_1_6_2_0_4_6_11_0_5_6_10_0_7_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_6_6_10_0_6_6_10_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_5_6_2_0_1_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_5_6_2_0_1_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_216_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_17_1_6_4_17_1_6_1_0_8_6_2_17_4_6_2_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_29_1_17_1_6_2_17_1_29_1_6_1_0_8_6_1_29_2_6_2_29_2_6_1_0_234_6_4_0_12_6_4_0_60_6_4_0_12_6_4_0_12_6_4_0_13_6_2_0_14_6_2_0_317_6_4_0_12_6_4_0_12_6_4_0_998",
            //    //        "0_1162_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_206_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_9_0_7_6_9_0_7_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_1_6_1_0_5_6_11_0_5_6_10_0_7_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_6_6_1_0_1_6_8_0_1_6_2_0_3_6_10_0_1_6_2_0_4_6_9_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_1_6_2_0_2_6_2_0_1_6_8_0_1_6_2_0_5_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_14_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_5_6_2_0_1_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_200_33_2_0_14_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_17_1_6_4_17_1_6_1_0_8_6_2_17_4_6_2_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_29_1_17_1_6_2_17_1_29_1_6_1_0_8_6_1_29_2_6_2_29_2_6_1_0_234_6_4_0_12_6_4_0_60_6_4_0_12_6_4_0_12_6_4_0_13_6_2_0_14_6_2_0_317_6_4_0_12_6_4_0_12_6_4_0_998",
            //    //        "0_1162_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_206_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_191_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_9_0_7_6_9_0_7_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_5_6_2_0_1_6_8_0_6_6_12_0_5_6_11_0_6_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_5_6_2_0_1_6_8_0_6_6_10_0_1_6_2_0_4_6_9_0_1_6_2_0_5_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_14_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_1_6_2_0_5_6_8_0_1_6_2_0_5_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_14_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_200_33_2_0_14_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_17_1_6_4_17_1_6_1_0_8_6_2_17_4_6_2_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_29_1_17_1_6_2_17_1_29_1_6_1_0_8_6_1_29_2_6_2_29_2_6_1_0_200_33_2_0_32_6_4_0_12_6_4_0_60_6_4_0_12_6_4_0_12_6_4_0_13_6_2_0_14_6_2_0_317_6_4_0_12_6_4_0_12_6_4_0_998",
            //    //        "0_1162_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_206_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_5_6_2_0_1_6_9_0_7_6_9_0_7_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_5_6_13_0_4_6_12_0_6_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_6_6_10_0_1_6_1_0_5_6_9_0_1_6_1_0_6_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_1_6_2_0_5_6_8_0_1_6_2_0_5_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_14_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_1_6_2_0_5_6_8_0_1_6_2_0_5_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_200_33_2_0_14_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_17_1_6_4_17_1_6_1_0_8_6_2_17_4_6_2_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_29_1_17_1_6_2_17_1_29_1_6_1_0_8_6_1_29_2_6_2_29_2_6_1_0_234_6_4_0_12_6_4_0_60_6_4_0_12_6_4_0_12_6_4_0_13_6_2_0_14_6_2_0_317_6_4_0_12_6_4_0_12_6_4_0_998",
            //    //        "0_1172_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_200_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_9_0_7_6_9_0_7_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_5_6_2_0_1_6_8_0_6_6_12_0_5_6_11_0_6_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_5_6_2_0_1_6_8_0_6_6_10_0_1_6_2_0_4_6_9_0_1_6_2_0_5_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_1_6_2_0_5_6_8_0_1_6_2_0_5_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_216_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_17_1_6_4_17_1_6_1_0_8_6_2_17_4_6_2_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_29_1_17_1_6_2_17_1_29_1_6_1_0_8_6_1_29_2_6_2_29_2_6_1_0_234_6_4_0_12_6_4_0_60_6_4_0_12_6_4_0_12_6_4_0_13_6_2_0_14_6_2_0_317_6_4_0_12_6_4_0_12_6_4_0_998",
            //    //        "0_1156_33_2_0_14_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_200_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_9_0_7_6_9_0_7_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_1_6_1_0_5_6_11_0_5_6_10_0_7_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_6_6_1_0_1_6_8_0_1_6_2_0_3_6_10_0_1_6_2_0_4_6_9_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_1_6_2_0_2_6_2_0_1_6_8_0_1_6_2_0_5_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_191_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_5_6_2_0_1_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_206_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_17_1_6_4_17_1_6_1_0_8_6_2_17_4_6_2_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_29_1_17_1_6_2_17_1_29_1_6_1_0_8_6_1_29_2_6_2_29_2_6_1_0_234_6_4_0_12_6_4_0_60_6_4_0_12_6_4_0_12_6_4_0_13_6_2_0_14_6_2_0_317_6_4_0_12_6_4_0_12_6_4_0_998",
            //    //        "0_1156_33_2_0_14_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_200_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_1_6_2_0_5_6_8_0_1_6_2_0_5_6_10_0_6_6_9_0_7_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_1_6_2_0_5_6_8_0_1_6_2_0_4_6_11_0_5_6_10_0_7_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_6_6_10_0_6_6_10_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_191_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_5_6_2_0_1_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_191_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_5_6_2_0_1_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_206_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_17_1_6_4_17_1_6_1_0_8_6_2_17_4_6_2_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_29_1_17_1_6_2_17_1_29_1_6_1_0_8_6_1_29_2_6_2_29_2_6_1_0_206_33_2_0_26_6_4_0_12_6_4_0_60_6_4_0_12_6_4_0_12_6_4_0_13_6_2_0_14_6_2_0_317_6_4_0_12_6_4_0_12_6_4_0_998",
            //    //        "0_1156_33_2_0_14_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_200_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_1_6_2_0_5_6_8_0_1_6_2_0_5_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_1_6_2_0_5_6_11_0_5_6_10_0_6_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_7_6_11_0_5_6_10_0_7_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_6_6_10_0_6_6_10_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_185_33_2_0_4_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_6_6_1_0_1_6_8_0_6_6_1_0_1_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_9_6_6_0_191_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_5_6_2_0_1_6_8_0_5_6_2_0_1_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_8_0_206_33_2_0_8_33_8_0_8_6_8_0_8_6_8_0_8_6_8_0_8_6_1_17_1_6_4_17_1_6_1_0_5_6_2_0_1_6_2_17_4_6_2_0_5_6_2_0_1_6_8_0_8_6_8_0_8_6_8_0_8_6_1_29_1_17_1_6_2_17_1_29_1_6_1_0_8_6_1_29_2_6_2_29_2_6_1_0_234_6_4_0_12_6_4_0_60_6_4_0_12_6_4_0_12_6_4_0_13_6_2_0_14_6_2_0_317_6_4_0_12_6_4_0_12_6_4_0_998",
            //    //    };
            //    //    version = SaveVersion.Ver2;
            //    //    break;
            //    //case VoxelObjName.Coin:
            //    //    limits = StandardLimits;
            //    //    data =new List<string>{ "0R2742]_9_9_9_9_0R11]_9_9_9_9_9_9_0R9]_9_9_9_0_0_9_9_9_0R8]_9_9_0_0_0_0_9_9_0R8]_9_9_0_0_0_0_9_9_0R8]_9_9_9_0_0_9_9_9_0R9]_9_9_9_9_9_9_0R11]_9_9_9_9_0R268]_9_9_9_9_0R11]_9_0_0_0_0_9_0R9]_9_0_0_9_9_0_0_9_0R8]_9_0_9_9_9_9_0_9_0R8]_9_0_9_9_9_9_0_9_0R8]_9_0_0_9_9_0_0_9_0R9]_9_0_0_0_0_9_0R11]_9_9_9_9_0R2902]"};
            //    //    break;
            //    //case VoxelObjName.Arrow:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0_0_0_0_0_0_11_0_11_0R381]_11_4_11_0R381]_11_2_11_0R381]_11_2_11_0R382]_4_0R383]_4_0R383]_4_0R383]_4_0R383]_4_0R383]_4_0R383]_4_0R383]_2_0R382]_8_8_8_0R382]_8_0R383]_8_0R760]"};
            //    //    break;
            //    //case VoxelObjName.HeartContainer:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R2343]_14_14_0R13]_14_14_14_14_0R11]_14_14_14_14_14_14_0R9]_14R8]_0_0_0_0_0_0_0_14R10]_0_0_0_0_0_0_14R10]_0_0_0_0_0_0_14R10]_0_0_0_0_0_0_14_14_14_14_0_0_14_14_14_14_0_0_0_0_0_0_0_14_14_0_0_0_0_14_14_0R235]_11_11_0R13]_11_14_14_11_0R11]_11_14_14_14_14_11_0R9]_11_14_14_14_14_14_14_11_0_0_0_0_0_0_0_11_14R8]_11_0_0_0_0_0_11_14R10]_11_0_0_0_0_11_14R10]_11_0_0_0_0_11_14R10]_11_0_0_0_0_11_14_14_14_14_11_11_14_14_14_14_11_0_0_0_0_0_11_14_14_11_0_0_11_14_14_11_0_0_0_0_0_0_0_11_11_0_0_0_0_11_11_0R219]_11_11_0R13]_11_14_14_11_0R11]_11_14_14_14_14_11_0R9]_11_14_14_14_14_14_14_11_0_0_0_0_0_0_0_11_14R8]_11_0_0_0_0_0_11_14R10]_11_0_0_0_0_11_14R10]_11_0_0_0_0_11_14R10]_11_0_0_0_0_11_14_14_14_14_11_11_14_14_14_14_11_0_0_0_0_0_11_14_14_11_0_0_11_14_14_11_0_0_0_0_0_0_0_11_11_0_0_0_0_11_11_0R235]_14_14_0R13]_14_14_14_14_0R11]_14_14_14_14_14_14_0R9]_14R8]_0_0_0_0_0_0_0_14R10]_0_0_0_0_0_0_14R10]_0_0_0_0_0_0_14R10]_0_0_0_0_0_0_14_14_14_14_0_0_14_14_14_14_0_0_0_0_0_0_0_14_14_0_0_0_0_14_14_0R2516]"};
            //    //    break;
            //    //case VoxelObjName.Stone1:
            //    //    limits = StandardLimits;
            //    //    data = new List<string> { "0_1926_21_3_0_13_21_3_0_364_21_5_0_11_21_5_0_13_21_2_0_347_21_7_0_10_21_6_0_11_21_4_0_346_21_7_0_9_21_7_0_11_21_4_0_346_21_6_0_10_21_7_0_11_21_3_0_348_21_2_0_1_21_2_0_11_21_5_0_2278", };
            //    //    version = SaveVersion.Ver2;
            //    //    break;
            //    //case VoxelObjName.Stone2:
            //    //    limits = StandardLimits;
            //    //    data = new List<string> { "0_1924_21_5_0_11_21_5_0_12_21_3_0_348_21_5_0_11_21_5_0_11_21_4_0_348_21_5_0_11_21_5_0_11_21_4_0_348_21_4_0_13_21_3_0_752_21_2_0_15_21_1_0_366_21_3_0_13_21_3_0_366_21_2_0_14_21_2_0_1509", };
            //    //    version = SaveVersion.Ver2;
            //    //    break;

            //    //case VoxelObjName.Stub:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R2694]_3_3_3_0R14]_3_3_0R14]_3_3_0R15]_3_0R334]_3_3_3_0R13]_3_3_0R14]_3_3_0R14]_3_3_0R335]_3_0R2679]"};
            //    //    break;
            //    //case VoxelObjName.House:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0_1923_6_10_0_6_6_10_0_6_6_10_0_6_6_10_0_6_6_10_0_6_7_10_0_294_6_1_2_8_6_1_0_6_6_1_0_8_6_1_0_6_6_1_0_8_6_1_0_6_6_1_0_8_6_1_0_6_6_1_0_8_6_1_0_6_6_1_7_9_0_6_7_10_0_278_6_1_2_8_6_2_0_5_6_1_0_8_6_2_0_5_6_1_0_8_6_2_0_5_6_1_0_8_6_2_0_5_6_1_0_8_6_2_0_5_6_1_7_9_6_1_0_5_6_1_7_9_6_1_0_5_7_10_6_1_0_14_6_2_0_245_6_1_2_8_6_2_0_5_6_1_0_8_6_2_0_5_6_1_0_8_6_2_0_5_6_1_0_8_6_2_0_5_6_1_0_8_6_2_0_5_6_1_7_9_6_1_0_5_6_1_7_9_6_1_0_5_6_1_7_9_6_1_0_5_7_9_6_2_0_245_6_1_2_8_6_1_0_6_6_1_0_8_6_1_0_6_6_1_0_8_6_1_0_6_6_1_0_8_6_1_0_6_6_1_0_8_6_1_0_6_6_1_7_8_6_1_0_6_6_1_7_8_6_1_0_6_7_10_0_262_6_1_2_8_6_1_0_6_6_1_0_8_6_1_0_6_6_1_0_8_6_1_0_6_6_1_0_8_6_1_0_6_6_1_0_8_6_1_0_6_6_1_7_8_6_1_0_6_7_10_0_278_6_6_4_3_6_1_0_6_6_6_4_3_6_1_0_6_6_2_0_2_6_2_4_2_3_1_6_1_0_6_6_2_0_2_6_2_4_3_6_1_0_6_6_10_0_6_7_10_0_1827",};
            //    //    version = SaveVersion.Ver2;
            //    //    break;
            //    //case VoxelObjName.Hill:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R1540]_2R8]_0R8]_2R8]_0R8]_1_1_1_1_1_1_1_0R345]_2R8]_0R8]_2R8]_0R8]_1R8]_0R10]_1_1_1_0R331]_2R8]_0R8]_2R8]_0R8]_1R8]_0R9]_1_1_1_1_1_0R330]_2R8]_0R8]_2_2_2_2_2_2_2_1_0R8]_1_1_1_1_1_1_1_0R10]_1_1_1_1_1_0R330]_2R8]_0R8]_2R8]_0R8]_1R8]_0R9]_1_1_1_1_1_0R330]_2R8]_0R8]_2R8]_0R9]_1_1_1_1_1_1_1_0R12]_21_0R332]_2_2_2_2_2_2_2_0R9]_2_2_1_1_1_2_0R11]_1_1_1_1_1_0R347]_2_2_2_2_2_0R13]_1_1_0R1894]"};
            //    //    break;
            //    //case VoxelObjName.Cactus:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R2695]_23_23_0R14]_23_23_0R14]_23_23_0R14]_23_23_23_23_0R12]_23_23_0_23_0R12]_23_23_0R14]_23_23_0R286]_23_23_0R14]_23_23_0R14]_23_23_0R12]_23_23_23_23_0R12]_23_0_23_23_0R14]_23_23_0R14]_23_23_0R2967]"};
            //    //    break;
            //    //case VoxelObjName.Apple:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R1607]_14_14_0R13]_14_14_14_14_0R12]_14_14_14_14_0R12]_14_14_14_14_0R315]_14_14_14_14_14_14_0R10]_14_14_14_14_14_14_0R10]_14_14_14_14_14_14_0R10]_14_14_14_14_14_14_0R10]_14_14_14_14_14_14_0R11]_14_14_14_14_0R284]_14_14_14_14_0R11]_14_14_14_14_14_14_0R9]_14R8]_0R8]_14R8]_0R8]_14R8]_0R8]_14R8]_0R9]_14_14_14_14_14_14_0R282]_14_14_2_2_14_14_0R9]_14R8]_0R8]_14R8]_0R8]_14R8]_0R8]_14R8]_0R8]_14R8]_0R9]_14_14_2_0_14_14_0R12]_2_0R15]_2_0R13]_2_2_0R238]_14_14_2_2_14_14_0R9]_14R8]_0R8]_14R8]_0R8]_14R8]_0R8]_14R8]_0R8]_14R8]_0R9]_14_14_0_0_14_14_0R283]_14_14_14_14_0R11]_14_14_14_14_14_14_0R9]_14R8]_0R8]_14R8]_0R8]_14R8]_0R8]_14R8]_0R9]_14_14_14_14_14_14_0R298]_14_14_14_14_14_14_0R10]_14_14_14_14_14_14_0R10]_14_14_14_14_14_14_0R10]_14_14_14_14_14_14_0R10]_14_14_14_14_14_14_0R11]_14_14_14_14_0R317]_14_14_0R13]_14_14_14_14_0R12]_14_14_14_14_0R12]_14_14_14_14_0R1798]"};
            //    //    break;
            //    //case VoxelObjName.HelmetVendel:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R246]_8_8_8_8_0R12]_8_8_8_8_0R12]_8_8_8_8_0R12]_9_9_9_9_0R12]_8_9_9_8_0R13]_9_9_0R282]_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_9_9_9_9_9_8_8_9_9_9_0_0_0_0_0_0_0_8R8]_0R10]_8_8_8_8_0R13]_9_9_0R265]_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_9_9_8R8]_9_9_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0R9]_8_8_8_8_0R13]_9_9_0R249]_8_8_0R8]_8_8_0_0_0_0_8_8_0R8]_8_8_0_0_0_0_8_8_0R8]_8_8_0_0_0_0_8_8_0R8]_8_8_0_0_0_0_9_8_0R8]_8_9_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0_0_0_0_0_0_0_8R8]_0R11]_9_9_0R249]_8_8_0R8]_8_8_0_0_0_0_8_8_0R8]_8_8_0_0_0_0_8_8_0R8]_8_8_0_0_0_0_8_8_0R8]_8_8_0_0_0_0_9_8_0R8]_8_9_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0_0_0_0_0_0_0_8R8]_0R10]_8_8_8_8_0R13]_9_9_0R233]_8_8_0R8]_8_8_0_0_0_0_8_8_0R8]_8_8_0_0_0_0_8_8_0R8]_8_8_0_0_0_0_8_8_0R8]_8_8_0_0_0_0_9_8_0R8]_8_9_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0_0_0_0_0_0_0_8R8]_0R10]_8_9_9_8_0R13]_9_9_0R297]_8_8_0R8]_8_8_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0_0_0_0_0_0_0_8R8]_0R10]_8_9_9_8_0R13]_9_9_0R297]_8_8_0R8]_8_8_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0_0_0_0_0_0_0_8R8]_0R10]_8_9_9_8_0R13]_9_9_0R233]_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_9_8_0R8]_8_9_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0_0_0_0_0_0_0_8R8]_0R10]_8_9_9_8_0R13]_9_9_0R217]_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_9_8_0R8]_8_9_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0_0_0_0_0_0_0_8R8]_0R11]_9_9_0R313]_8_8_0R8]_8_8_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0_0_0_0_0_0_0_8R8]_0R11]_9_9_0R314]_8R10]_0_0_0_0_0_0_0_8R8]_0R8]_8R8]_0R11]_9_9_0R302]_9_9_0R14]_9_9_0R11]_9R8]_0R9]_8_8_9_9_8_8_0R12]_9_9_0R1207]"};
            //    //    break;
            //    //case VoxelObjName.HelmetHorned1:
            //    //    limits = StandardLimits;
            //    //    data =new List<string>{ "0R647]_9_9_0R14]_8_8_0R14]_8_8_0R14]_8_8_0R331]_9_9_9_8_8_9_9_9_0R8]_8R8]_0R8]_8R8]_0R8]_8R8]_0R11]_8_8_0R314]_9_8R8]_9_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0R8]_8_8_8_8_8_8_0R12]_8_8_0R298]_9_8R8]_9_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_0_8R8]_0R11]_8_8_0R297]_9_8R10]_9_0_0_0_11_9_8R10]_9_11_0_0_11_9_8R10]_9_11_0_0_11_9_8R10]_9_11_0_0_0_0_0_8R8]_0R11]_8_8_0R14]_8_8_0R280]_11_9_8R10]_9_11_0_0_11_9_8R10]_9_11_0_11_11_9_8R10]_9_11_11_11_11_9_8R10]_9_11_11_11_11_0_0_8R8]_0_0_11_11_11_11_0_0_0_0_8_8_8_8_0_0_0_0_11_11_11_0_0_0_0_0_0_8_8_0_0_0_0_0_0_11_0R273]_11_9_8R10]_9_11_0_0_11_9_8R10]_9_11_0_11_11_9_8R10]_9_11_11_11_11_9_8R10]_9_11_11_11_11_0_0_8R8]_0_0_11_11_11_11_0_0_0_0_8_8_8_8_0_0_0_0_11_11_11_0_0_0_0_0_0_8_8_0_0_0_0_0_0_11_0R274]_9_8R10]_9_0_0_0_11_9_8R10]_9_11_0_0_11_9_8R10]_9_11_0_0_11_9_8R10]_9_11_0_0_0_0_0_8R8]_0R11]_8_8_0R14]_8_8_0R282]_9_8R8]_9_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_0_8R8]_0R11]_8_8_0R314]_9_0R8]_9_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0R8]_8_8_8_8_8_8_0R12]_8_8_0R330]_9R10]_0_0_0_0_0_0_0_8_8_8_9_9_8_8_8_0R11]_8_8_0R348]_9_9_9_9_9_9_0R12]_9_9_0R1223]"};
            //    //    break;
            //    //case VoxelObjName.HelmetKnight:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R931]_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0R213]_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0R197]_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0R197]_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0R197]_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0R197]_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0R197]_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0R197]_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0R197]_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_9_0R10]_9_0_0_0_0_9_0R10]_9_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0R197]_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_8_0R10]_8_0_0_0_0_9_0R10]_9_0R36]_9_8R10]_9_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_8R12]_0_0_0_0_0_8R10]_0R198]_8_8_8_9_0_0_9_8_8_8_0_0_0_0_0_0_8_8_8_9_0_0_9_8_8_8_0_0_0_0_0_0_8_8_8_9_0_0_9_8_8_8_0_0_0_0_0_0_8_8_8_9_0_0_9_8_8_8_0_0_0_0_0_0_9_9_9_9_0_0_9_9_9_9_0R38]_9R10]_0_0_0_0_0_0_8_8_8_9_9_9_9_8_8_8_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_8R10]_0R1203]"};
            //    //    break;
            //    //case VoxelObjName.BeardLarge:
                    
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R3251]_2_0R8]_2_0_0_0_0_0_0_2_0R8]_2_0_0_0_0_0_0_2_0R8]_2_0_0_0_0_0_0_2_0R8]_2_0R278]_2R10]_0_0_0_0_0_0_2R10]_0_0_0_0_0_0_2R10]_0_0_0_0_0_0_2_0R8]_2_0_0_0_0_0_0_2_0R8]_2_0_0_0_0_0_0_2_0R8]_2_0_0_0_0_0_0_2_0R8]_2_0R279]_2R8]_0R8]_2R8]_0R8]_2_2_0_0_0_0_2_2_0_0_0_0_0_0_0_2_2_0_0_0_0_0_0_0_2_0_0_0_0_0_0_2_0R8]_2_0_0_0_0_0_0_2_0R8]_2_0R280]_2_0_0_0_0_2_0R9]_2R8]_0R8]_2R8]_0R8]_2R8]_0R8]_2_2_0_0_0_0_2_2_0R8]_2_0_2_2_2_2_0_2_0R8]_2_2_0_0_0_0_2_2_0R283]_2_2_0R13]_2_2_2_2_0R12]_2_2_2_2_0R11]_2_2_2_2_2_2_0R10]_2_0_0_0_0_2_0R10]_2_2_2_2_2_2_0R1333]"};
            //    //    break;
            //    //case VoxelObjName.Mustache:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R4806]_2_2_2_2_0R11]_2_2_0_0_2_2_0R1317]"};
            //    //    break;
            //    //case VoxelObjName.MustacheBikers:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R4373]_2_0_0_0_0_2_0R378]_2_0_0_0_0_2_0R10]_2_0_0_0_0_2_0R10]_2_0_0_0_0_2_0R10]_2_2_2_2_2_2_0R1333]"};
            //    //    break;
            //    //case VoxelObjName.CastleDoor:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R2178]_33_0_0_0_0_33_33_0_0_0_0_33_0_0_33R16]_0R96]_4_4_0R12]_4_4_4_4_0R12]_4_4_4_4_0R12]_4_4_4_4_0R12]_4_4_4_4_0R12]_4_4_4_4_0R12]_4_4_4_4_0R12]_4_4_4_4_0R12]_4_4_4_4_0R12]_4_4_4_4_0R12]_4_4_4_4_0R12]_4_4_4_4_0R12]_4_4_4_4_0R12]_4_4_4_4_0R12]_4_4_4_4_0R12]_4_4_4_4_33_0_0_0_0_33_33_0_0_0_0_33_4_4_4_4_33_0_0_0_0_33_33_0_0_0_0_33_4_4_4_33R15]_0R96]_33_33_33_33_0R8]_33R8]_0R8]_33R8]_0R8]_33R8]_0R8]_33R8]_0R8]_33R8]_0R8]_33R8]_0R8]_33R8]_0R8]_33R8]_0R8]_33R8]_0R8]_33R9]_0_0_0_0_0_0_33R11]_0_0_0_0_33R102]_0R96]_6_33_33_0_4_4_4_4_0_0_0_0_0_33_33_33_33_33_33_0_4_4_4_4_0_0_0_0_0_33_33_33_33_33_33_8_8_4_4_4_0_0_0_0_8_33_33_33_33_33_33_0_4_4_4_4_0_0_0_0_0_33_33_33_33_33_33_0_4_4_4_4_0_0_0_0_0_33_33_33_33_33_33_0_4_4_4_8_0_0_0_0_0_33_33_33_33_33_33_0_4_4_4_4_0_0_0_0_0_33_33_33_33_33_33_8_8_4_4_4_0_0_0_0_8_33_33_33_33_33_33_0_4_4_4_4_0_0_0_0_0_33_33_33_33_33_33_0_4_4_4_4_0_0_0_0_0_33_33_33_33_33_33_33_0_4_4_4_0_0_0_0_33R9]_0_4_4_0_0_0_33R11]_0_0_0_0_33R102]_0R80]_33_33_0R10]_4_0_33_33_33_0R11]_4_0_0_33_33_0R11]_8_0_0_33_33_0R11]_4_0_0_33_33_0R11]_4_0_0_33_33_0R11]_4_0_0_33_33_0R11]_4_0_0_33_33_0R11]_8_0_0_33_33_0R11]_4_0_0_33_33_0R11]_4_0_0_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0_0_0_33_0_0_0_0_0_0_33_0_0_0_33_33_0_0_0_33_0_0_0_0_0_0_33_0_0_0_33R52]_0_0_33_33_33_33_33_33_0_0_33_33_33_33_33_33_0_0_33_33_33_33_33_33_0_0_33_33_33_0R60]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R87]_33_0_0_0_0_0_0_33_0_0_0_0_33R35]_0_0_33_33_33_33_33_33_0_0_33_33_33_33_33_33_0_0_33_33_33_33_33_33_0_0_33_33_33_0R60]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R83]_33R35]_0_0_33_33_33_33_33_33_0_0_33_33_33_33_33_33_0_0_33_33_33_33_33_33_0_0_33_33_33_0R60]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_8_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R15]_4_0R1347]"};
            //    //    break;
            //    //case VoxelObjName.CastleWallSmith:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R1430]_33_33_33_33_0R12]_33_33_33_33_0R12]_33_33_33_33_0R348]_33_0_0_33_0R12]_33_0_0_33_0R12]_33_0_0_33_0R330]_33_0_0_0_0_0_0_33_0_0_0_0_33_33_33_33_33_33_33_0_0_33R14]_0_0_33R10]_0_33_33_33_0_0_33_33_33_0_33_33_33_0R64]_33R17]_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0_0_0_33_0_0_0_0_0_0_33_0_0_0_33_33_0_0_0_33_0_0_0_0_0_0_33_0_0_0_33R40]_0_0_33R10]_0_33_33_33_0_0_33_33_33_0_33_33_33_0R64]_33R71]_0_0_0_33R221]_0_0_33_33_33_33_33_33_33_0R80]_33R288]_0R96]_33_33_33_33_33_33_33_6_6_33_33_33_33_33_33_33_0_0_0_0_4_0_0_6_6_0_0_4_0R8]_4_0_0_6_6_0_0_4_0R8]_4_0_0_0_0_0_0_4_0R8]_4_0_0_0_0_0_0_4_0R8]_4_0_0_0_0_0_0_4_0R8]_4_0_0_0_0_0_0_4_0_0_0_0_0_0_0_4_4_0_0_0_0_0_0_4_0_0_0_0_0_0_4_0_4_26_0_0_0_0_26_4_0_0_0_0_0_4_0_0_4_0_26_26_26_26_0_4_0_0_0_0_4_0_0_0_4_0_0_0_0_0_0_4_0_0_0_4_0_0_0_0_4_0_0_0_0_0_0_4_0_0_4_0_0_0_0_0_4_0_0_0_0_0_0_4_0_4_0_0_0_0_0_0_4R9]_0_0_0_0_0_0_0_4_0_0_0_0_0_0_4_0_0_0_0_0_0_33_0_4_0_0_33_33_0_0_4_0_33_0_0_0_0_33_0_4_0_0_33_33_0_0_4_0_33_0_0_33_33_33_33_33_33_18_18_18_18_33_33_33_33_33_33_0R100]_3_0_6_6_6_6_0_4_0R10]_6_17_17_6_0R10]_4_0_6_0_0_6_0_4_0R10]_6_6_6_6_0R13]_6_6_0R14]_6_6_0R11]_4_0_0_6_6_0_0_4_0R8]_26_0_0_6_6_0_0_26_0R9]_26_0_6_6_0_26_0R11]_26_6_6_26_0R13]_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R9]_33_0_0_0_0_6_6_0_0_0_0_33_0_0_33_33_33_33_33_33_18_6_6_18_33_33_33_33_33_33_0_0_0_0_0_0_0_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R27]_3_0_6_6_6_6_0_4_0R10]_6_0_0_6_0R10]_4_0_6_0_0_6_0_4_0R10]_6_6_6_6_0R13]_6_6_0R14]_6_6_0R11]_4_0_0_6_6_0_0_4_0R8]_26_0_0_6_6_0_0_26_0R9]_26_0_6_6_0_26_0R11]_26_6_6_26_0R13]_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R14]_6_6_0R27]_3_0_0_0_0_0_0_4_0R24]_4_0_0_0_0_0_0_4_0R56]_4_0_0_0_0_0_0_4_0R8]_26_0_0_0_0_0_0_26_0R9]_26_0_0_0_0_26_0R11]_26_26_26_26_0R234]_3_0_0_0_0_0_0_4_0R24]_4_0_0_0_0_0_0_4_0R56]_4_0_0_0_0_0_0_4_0R8]_26_0_0_0_0_0_0_26_0R9]_26_26_26_26_26_26_0R249]_3_0_0_0_0_0_0_4_0R24]_4_0_0_0_0_0_0_4_0R56]_4_0_0_0_0_0_0_4_0R8]_26R8]_0R264]_4_0_0_0_0_0_0_4_0R8]_4_0_0_0_0_0_0_4_0R8]_4_0_0_0_0_0_0_4_0R8]_4_0_0_0_0_0_0_4_0R8]_4_0_0_0_0_0_0_4_0R8]_4_0_0_0_0_0_0_4_0R8]_4R8]_0R276]"};
            //    //    break;
            //    //case VoxelObjName.CastleWallTarget:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R2180]_33_0_0_0_0_0_0_33_0_0_0_0_33R35]_0_33_33_33_0_0_33_33_33_0_33_33_33_0R64]_33R17]_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0R14]_33_33_0_0_0_33_0_0_0_0_0_0_33_0_0_0_33_33_0_0_0_33_0_0_0_0_0_0_33_0_0_0_33R52]_0_33_33_33_0_0_33_33_33_0_33_33_33_0R64]_33R71]_0_0_0_33R29]_0_0_0_33R13]_0_0_0_33R13]_0_0_0_33R13]_0_0_0_33R13]_0_0_0_33R13]_0_0_0_33R14]_0_33R103]_0R80]_33R120]_0_33R15]_0_33R15]_0_33R15]_0_33R119]_0R96]_33R16]_0_0_0_0_4_4_0_0_0_0_0_4_0R8]_4_0_4_0_0_0_0_4_0R8]_4_0_0_4_0_0_0_4_0R8]_4_0_0_0_4_0_0_4_0R8]_4_0_0_0_0_4_0_4_0R8]_4_0_0_0_0_0_4_4_0_0_0_0_0_0_0_4_4_0_0_0_0_0_0_4_0_0_0_0_0_0_4_0_4_0_0_0_0_0_0_4_0_0_0_0_0_4_0_0_4_0_0_0_0_0_0_4_0_0_0_0_4_0_0_0_4_0_0_0_0_0_0_4_0_0_0_4_0_0_0_0_4_0_0_0_0_0_0_4_0_0_4_0_0_0_0_0_4_0_0_0_0_0_0_4_0_4_0_0_0_0_0_0_4R9]_0_0_0_0_0_0_0_4_0_0_0_0_0_0_4_0_0_0_0_0_0_33_0_4_0_0_33_33_0_0_4_0_33_0_0_0_0_33_0_4_0_0_33_33_0_0_4_0_33_0_0_33R16]_0R354]_33_0_0_0_0_33_33_0_0_0_0_33_0_0_33R16]_0R1252]_4_0_0_0_0_0_0_4_0R8]_4_0_0_0_0_0_0_4_0R8]_11_0_0_0_0_0_0_11_0_0_0_0_0_0_0_11_11_11_0_0_0_0_11_11_11_0_0_0_0_0_11_11_14_11_11_0_0_11_11_14_11_11_0_0_0_0_0_11_11_11_0_0_0_0_11_11_11_0_0_0_0_0_0_0_11_0_0_0_0_0_0_11_0R660]"};
            //    //    break;
            //    //case VoxelObjName.Well:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R1926]_21_21_21_21_21_0R11]_21_21_21_21_21_0R13]_4_0R15]_4_0R15]_4_0R317]_21_20_20_20_21_0R11]_21_0_0_0_21_0R45]_4_0R317]_21_20_20_20_21_0R11]_21_0_11_0_21_0R13]_11_0R15]_11_0R15]_11_0R317]_21_20_20_20_21_0R11]_21_0_0_0_21_0R45]_4_0R317]_21_21_21_21_21_0R11]_21_21_21_21_21_0R13]_4_0R15]_4_0R15]_4_0R383]_8_0R383]_8_8_0R383]_8_0R1462]"};
            //    //    break;
            //    //case VoxelObjName.EnemySlime:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>
            //    //    {
            //    //        "0_772_6_9_0_8_6_7_0_359_6_11_0_6_6_9_0_7_6_1_0_349_6_13_0_3_6_12_0_6_6_9_0_7_6_8_0_37_6_3_0_286_6_13_0_3_6_13_0_5_6_9_0_7_6_9_0_7_6_6_0_10_6_1_0_1_6_4_0_9_6_2_0_1_6_3_0_281_6_13_0_3_6_13_0_5_6_10_0_6_6_9_0_8_6_7_0_10_6_4_0_1_6_1_0_10_6_3_0_2_6_3_0_9_6_2_0_4_6_2_0_259_6_13_0_3_6_13_0_5_6_10_0_6_6_9_0_8_6_6_0_11_6_4_0_12_6_3_0_14_6_2_0_265_6_13_0_3_6_13_0_5_6_10_0_6_6_9_0_8_6_6_0_10_6_5_0_11_6_1_0_15_6_1_0_13_6_2_0_13_6_2_0_14_6_1_0_223_6_13_0_3_6_13_0_5_6_10_0_6_6_9_0_9_6_6_0_10_6_3_0_2_6_1_0_15_6_2_0_15_6_2_0_15_6_1_0_15_6_3_0_226_6_13_0_3_6_13_0_5_6_10_0_6_6_1_29_1_6_6_29_1_0_10_6_2_0_313_6_13_0_3_6_13_0_5_6_10_0_6_6_1_17_1_6_1_29_1_6_2_29_1_6_1_17_1_0_325_6_13_0_4_6_11_0_6_6_9_0_8_6_2_17_1_6_2_17_1_6_1_0_327_6_11_0_6_6_9_0_8_6_7_0_344_6_9_0_8_6_7_0_740",
            //    //        "0_772_6_9_0_8_6_7_0_359_6_11_0_6_6_9_0_7_6_1_0_349_6_13_0_3_6_12_0_6_6_9_0_7_6_8_0_39_6_2_0_285_6_13_0_3_6_13_0_5_6_9_0_7_6_9_0_7_6_6_0_10_6_1_0_1_6_4_0_6_6_3_0_1_6_1_0_1_6_3_0_282_6_12_0_3_6_13_0_3_6_12_0_6_6_9_0_8_6_7_0_10_6_4_0_1_6_2_0_9_6_3_0_3_6_3_0_8_6_2_0_5_6_1_0_260_6_12_0_3_6_13_0_3_6_12_0_6_6_9_0_8_6_6_0_11_6_4_0_12_6_3_0_14_6_2_0_265_6_13_0_3_6_13_0_5_6_10_0_6_6_9_0_8_6_6_0_10_6_5_0_11_6_1_0_15_6_1_0_14_6_1_0_14_6_2_0_13_6_2_0_222_6_12_0_4_6_13_0_5_6_11_0_5_6_10_0_8_6_6_0_10_6_3_0_2_6_1_0_15_6_2_0_15_6_2_0_15_6_1_0_15_6_3_0_226_6_12_0_4_6_13_0_5_6_11_0_5_6_1_29_1_6_6_29_1_6_1_0_9_6_2_0_313_6_13_0_3_6_13_0_5_6_10_0_6_6_1_17_1_6_1_29_1_6_2_29_1_6_1_17_1_0_325_6_13_0_4_6_11_0_6_6_9_0_8_6_2_17_1_6_2_17_1_6_1_0_327_6_11_0_6_6_9_0_8_6_7_0_344_6_9_0_8_6_7_0_740",
            //    //        "0_772_6_9_0_8_6_7_0_359_6_11_0_6_6_9_0_7_6_1_0_349_6_13_0_3_6_12_0_6_6_9_0_7_6_8_0_38_6_2_0_287_6_12_0_3_6_13_0_3_6_11_0_7_6_9_0_7_6_6_0_10_6_1_0_1_6_4_0_6_6_2_0_1_6_2_0_1_6_3_0_282_6_12_0_3_6_13_0_3_6_12_0_6_6_9_0_8_6_7_0_10_6_4_0_1_6_1_0_10_6_3_0_2_6_3_0_9_6_2_0_4_6_2_0_259_6_13_0_3_6_13_0_5_6_10_0_6_6_9_0_8_6_6_0_11_6_4_0_12_6_3_0_14_6_2_0_265_6_12_0_4_6_13_0_5_6_11_0_5_6_10_0_7_6_6_0_10_6_5_0_11_6_1_0_15_6_1_0_14_6_1_0_14_6_2_0_13_6_2_0_14_6_1_0_207_6_12_0_4_6_13_0_5_6_11_0_5_6_10_0_8_6_6_0_10_6_3_0_2_6_1_0_15_6_1_0_15_6_3_0_15_6_1_0_15_6_3_0_226_6_13_0_3_6_13_0_5_6_10_0_6_6_1_29_1_6_6_29_1_0_10_6_2_0_314_6_12_0_3_6_13_0_3_6_12_0_6_6_1_17_1_6_1_29_1_6_2_29_1_6_1_17_1_0_326_6_12_0_4_6_11_0_5_6_10_0_8_6_2_17_1_6_2_17_1_6_1_0_327_6_11_0_6_6_9_0_8_6_7_0_344_6_9_0_8_6_7_0_740",
            //    //        "0_772_6_9_0_8_6_7_0_359_6_11_0_6_6_9_0_7_6_1_0_350_6_12_0_3_6_12_0_4_6_11_0_7_6_8_0_37_6_3_0_287_6_12_0_3_6_13_0_3_6_11_0_7_6_9_0_7_6_6_0_10_6_1_0_1_6_4_0_9_6_2_0_1_6_3_0_281_6_13_0_3_6_13_0_5_6_10_0_6_6_9_0_8_6_7_0_10_6_4_0_1_6_1_0_10_6_3_0_2_6_2_0_10_6_2_0_3_6_3_0_259_6_12_0_4_6_13_0_5_6_11_0_5_6_10_0_7_6_6_0_11_6_4_0_12_6_3_0_14_6_2_0_265_6_12_0_4_6_13_0_5_6_11_0_5_6_10_0_7_6_6_0_10_6_5_0_11_6_1_0_15_6_1_0_13_6_2_0_14_6_1_0_14_6_2_0_14_6_1_0_207_6_13_0_3_6_13_0_5_6_10_0_6_6_9_0_9_6_6_0_10_6_3_0_2_6_1_0_15_6_1_0_15_6_2_0_15_6_2_0_15_6_3_0_227_6_12_0_3_6_13_0_3_6_12_0_6_6_1_29_1_6_6_29_1_0_10_6_2_0_314_6_12_0_3_6_13_0_3_6_12_0_6_6_1_17_1_6_1_29_1_6_2_29_1_6_1_17_1_0_325_6_13_0_4_6_11_0_6_6_9_0_8_6_2_17_1_6_2_17_1_6_1_0_327_6_11_0_6_6_9_0_8_6_7_0_344_6_9_0_8_6_7_0_740",
            //    //        "0_772_6_9_0_8_6_7_0_359_6_11_0_6_6_9_0_7_6_1_0_349_6_13_0_3_6_12_0_6_6_9_0_7_6_8_0_37_6_2_0_287_6_13_0_3_6_13_0_5_6_9_0_7_6_9_0_7_6_6_0_10_6_1_0_1_6_4_0_8_6_3_0_1_6_3_0_281_6_12_0_4_6_13_0_5_6_11_0_5_6_10_0_7_6_7_0_10_6_4_0_1_6_1_0_10_6_3_0_2_6_1_0_11_6_2_0_2_6_4_0_259_6_12_0_4_6_13_0_5_6_11_0_5_6_10_0_7_6_6_0_11_6_4_0_12_6_3_0_14_6_2_0_265_6_13_0_3_6_13_0_5_6_10_0_6_6_9_0_8_6_6_0_10_6_5_0_11_6_1_0_15_6_1_0_13_6_2_0_13_6_2_0_14_6_1_0_15_6_1_0_208_6_12_0_3_6_13_0_3_6_12_0_6_6_9_0_9_6_6_0_10_6_3_0_2_6_1_0_15_6_1_0_15_6_2_0_15_6_1_0_15_6_1_0_1_6_2_0_13_6_1_0_213_6_12_0_3_6_13_0_3_6_12_0_6_6_1_29_1_6_6_29_1_0_10_6_2_0_313_6_13_0_3_6_13_0_5_6_10_0_6_6_1_17_1_6_1_29_1_6_2_29_1_6_1_17_1_0_325_6_13_0_4_6_11_0_6_6_9_0_8_6_2_17_1_6_2_17_1_6_1_0_327_6_11_0_6_6_9_0_8_6_7_0_344_6_9_0_8_6_7_0_740",
            //    //        "0_772_6_9_0_8_6_7_0_359_6_11_0_6_6_9_0_7_6_1_0_349_6_13_0_3_6_12_0_6_6_9_0_7_6_8_0_37_6_1_0_288_6_12_0_4_6_13_0_5_6_11_0_5_6_9_0_7_6_6_0_10_6_1_0_1_6_4_0_7_6_4_0_1_6_3_0_281_6_12_0_4_6_13_0_5_6_11_0_5_6_10_0_7_6_7_0_10_6_4_0_1_6_1_0_10_6_3_0_2_6_3_0_9_6_2_0_3_6_3_0_259_6_13_0_3_6_13_0_5_6_10_0_6_6_9_0_8_6_6_0_11_6_4_0_12_6_3_0_14_6_2_0_266_6_12_0_3_6_13_0_3_6_12_0_6_6_9_0_8_6_6_0_10_6_5_0_11_6_1_0_15_6_1_0_13_6_2_0_13_6_2_0_14_6_1_0_224_6_12_0_3_6_13_0_3_6_12_0_6_6_9_0_9_6_6_0_10_6_3_0_2_6_1_0_15_6_2_0_15_6_1_0_15_6_1_0_15_6_2_0_1_6_1_0_14_6_1_0_211_6_13_0_3_6_13_0_5_6_10_0_6_6_1_29_1_6_6_29_1_0_10_6_2_0_313_6_13_0_3_6_13_0_5_6_10_0_6_6_1_17_1_6_1_29_1_6_2_29_1_6_1_17_1_0_325_6_12_0_5_6_11_0_6_6_10_0_7_6_2_17_1_6_2_17_1_6_1_0_327_6_11_0_6_6_9_0_8_6_7_0_344_6_9_0_8_6_7_0_740",
            //    //        "0_772_6_9_0_8_6_7_0_359_6_11_0_6_6_9_0_7_6_1_0_349_6_12_0_4_6_12_0_6_6_9_0_7_6_8_0_24_6_1_0_15_6_1_0_285_6_12_0_4_6_13_0_5_6_9_0_1_6_1_0_5_6_9_0_7_6_6_0_10_6_1_0_1_6_4_0_7_6_4_0_1_6_3_0_281_6_13_0_3_6_13_0_5_6_10_0_6_6_9_0_8_6_7_0_10_6_4_0_1_6_1_0_10_6_3_0_2_6_3_0_9_6_2_0_4_6_2_0_260_6_12_0_3_6_13_0_3_6_12_0_6_6_9_0_8_6_6_0_11_6_4_0_12_6_3_0_14_6_2_0_266_6_12_0_3_6_13_0_3_6_12_0_6_6_9_0_8_6_6_0_10_6_5_0_11_6_1_0_15_6_1_0_15_6_1_0_12_6_3_0_13_6_1_0_223_6_13_0_3_6_13_0_5_6_10_0_6_6_9_0_9_6_6_0_10_6_3_0_2_6_1_0_15_6_2_0_15_6_1_0_15_6_2_0_15_6_2_0_16_6_1_0_210_6_13_0_3_6_13_0_5_6_10_0_6_6_1_29_1_6_6_29_1_0_10_6_2_0_313_6_12_0_4_6_13_0_5_6_11_0_5_6_1_17_1_6_1_29_1_6_2_29_1_6_1_17_1_6_1_0_324_6_12_0_5_6_11_0_6_6_9_0_8_6_2_17_1_6_2_17_1_6_1_0_327_6_11_0_6_6_9_0_8_6_7_0_344_6_9_0_8_6_7_0_740",
            //    //    };
            //    //    version = SaveVersion.Ver2;
            //    //    break;
            //    //case VoxelObjName.SalesTent:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R402]_29R12]_0R372]_29R12]_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0R309]_29_29_4_0_0_0_4_0_0_4_29_29_0_0_0_0_0_29_4_0_0_0_4_4_4_4_29_0_0_0_0_0_0_29_4_0_0_0_0_9_9_4_29_0_0_0_0_0_0_29_4_0_0_0_0_0_0_4_29_0_0_0_0_0_0_29_4R8]_29_0_0_0_0_0_0_0_29R8]_0R294]_29_29_9_0_0_0_0_0_0_0_29_29_0_0_0_0_0_29_0_0_0_0_4_4_4_0_29_0_0_0_0_0_0_29_0_0_0_0_0_9_9_0_29_0_0_0_0_0_0_29_0R8]_29_0_0_0_0_0_0_29_4_0_0_0_0_0_0_4_29_0_0_0_0_0_0_0_29R8]_0R294]_29_29_0R8]_29_29_0_0_0_0_0_29_0_0_0_0_4_4_4_0_29_0_0_0_0_0_0_29_0_0_0_0_0_9_0_0_29_0_0_0_0_0_0_29_0R8]_29_0_0_0_0_0_0_29_4_0_0_0_0_0_0_4_29_0_0_0_0_0_0_0_29R8]_0R294]_29_29_0R8]_29_29_0_0_0_0_0_29_0_0_0_0_4_4_4_0_29_0_0_0_0_0_0_29_0R8]_29_0_0_0_0_0_0_29_0R8]_29_0_0_0_0_0_0_29_4_0_0_0_0_0_0_4_29_0_0_0_0_0_0_0_29R8]_0R294]_29_29_4_0_0_0_4_0_0_4_29_29_0_0_0_0_0_29_4_0_0_0_4_4_4_4_29_0_0_0_0_0_0_29_4_0_0_0_0_0_0_4_29_0_0_0_0_0_0_29_4_0_0_0_0_0_0_4_29_0_0_0_0_0_0_29_4_0_0_0_0_0_0_4_29_0_0_0_0_0_0_0_29R8]_0R1047]_32_30_32_30_32_30_32_30_32_0R375]_30R9]_0R8]_8_0_0_0_0_8_0R361]_30R9]_0R13]_8_0R361]_32_30_32_30_32_30_32_30_32_0R1140]"};
            //    //    break;

            //    //case VoxelObjName.Hawk:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R805]_26_0_26_26_0_26_0R378]_26_26_26_26_26_26_0R379]_26_26_26_26_0R374]_26_0_26_0_26_0_0_26_26_0_0_26_0_26_0_26_0R359]_26_26_0_0_0_0_0_0_0_26R16]_0R358]_26_26_26_26_0_0_0_0_0_0_26R16]_0R358]_26_26_26_26_0_0_0_0_0_0_26R16]_0R358]_26_26_26_26_0_0_0_0_0_0_26R16]_0R359]_26_26_0R8]_26_26_26_26_26_0_26_26_0_26_26_26_26_26_0R360]_26_26_0R9]_26_26_0_0_0_26_26_0_0_0_26_26_0R361]_26_26_0R14]_26_26_0R366]_27_27_0R14]_26_26_0R366]_27_27_0R743]"};
            //    //    break;
            //    //case VoxelObjName.ApplePie:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R790]_8_8_8_8_0R364]_8_8_8_8_0R10]_8R8]_0R10]_4_14_4_4_0R346]_8R8]_0_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_0_4_4_4_4_4_4_14_4_0R344]_8R8]_0_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_0_4R8]_0R10]_4_4_4_4_0R329]_8R10]_0_0_0_0_0_8R12]_0_0_0_0_0_4_14_4R8]_0R8]_4_4_4_4_14_4_0R328]_8R10]_0_0_0_0_0_8R12]_0_0_0_0_0_4R10]_0R8]_4_4_4_4_4_4_0R328]_8R10]_0_0_0_0_0_8R12]_0_0_0_0_0_14_4R9]_0R8]_4_14_4_4_4_4_0R328]_8R10]_0_0_0_0_0_8R12]_0_0_0_0_0_4R8]_14_4_0R8]_4_4_4_4_4_4_0R329]_8R8]_0_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_0_4R8]_0R10]_4_4_4_4_0R330]_8R8]_0_0_0_0_0_0_0_8R10]_0_0_0_0_0_0_0_4_14_4_4_14_4_4_4_0R346]_8_8_8_8_0R10]_8R8]_0R10]_4_4_4_4_0R364]_8_8_8_8_0R1126]"};
            //    //    break;
            //    //case VoxelObjName.Mountain:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0_0_21R14]_0_0_21R10]_0_21_21_21_0_0_0_21_21_21_21_0_21_21_21_21_0_21_21_21_0_0_0_21_21_0_21_0_21_21_21_21_0_21_21_0R323]_21R14]_0_0_21R14]_0_0_21R14]_0_0_21R13]_0_0_0_0_21R11]_0_0_0_0_0_21_21_21_21_0_21_21_0_21_21_0R291]_21R63]_0_0_0_0_21R11]_0_0_0_0_0_21R11]_0_0_0_0_0_21_21_21_0_0_0_21_21_21_21_0_0_0_0_0_0_21_21_21_0_0_0_21_21_21_21_0_0_0_0_0_0_21_0_21_0_0_0_21_21_21_21_0_0_0_0_0_0_21_0_21_0R234]_21R47]_0_21R15]_0_0_21R13]_0_0_0_21R13]_0_0_0_21R12]_0_0_0_0_21R12]_0_0_0_0_21_21_21_21_21_0_0_0_21_21_21_21_0_0_0_0_0_21_21_21_21_0R234]_21R63]_0_21R14]_0_0_21R14]_0_0_0_21R12]_0_0_0_0_21R12]_0_0_0_0_21R12]_0_0_0_0_21_21_21_21_21_21_0_21_21_21_0R8]_21_21_21_21_0_21_21_21_0R213]_21R63]_0_21R14]_0_0_0_21R13]_0_0_0_0_21R11]_0_0_0_0_0_21R11]_0_0_0_0_0_0_21R10]_0_0_0_0_0_0_21R8]_0R8]_21R8]_0R10]_21_21_21_21_21_0R198]_21R63]_0_0_21R13]_0_0_0_21R13]_0_0_0_0_21R11]_0_0_0_0_0_21R11]_0_0_0_0_0_0_21R10]_0_0_0_0_0_0_21R8]_0R8]_21_21_21_21_21_21_21_0R11]_21_21_21_21_21_0R13]_21_21_0R183]_21R63]_0_0_21R13]_0_0_0_21R13]_0_0_0_21R11]_0_0_0_0_0_0_21R10]_0_0_0_0_0_0_0_21R8]_0R8]_21R8]_0R8]_21R8]_0R11]_21_21_21_21_0R13]_21_21_0R183]_21R16]_0_21R15]_0_21R15]_0_21R15]_0_21R13]_0_0_0_21R13]_0_0_0_0_21R10]_0_0_0_0_0_0_21R10]_0_0_0_0_0_0_0_21R8]_0R8]_21R8]_0R8]_21R8]_0R10]_21_21_21_21_21_0R198]_21R64]_0_21R13]_0_0_0_21R13]_0_0_0_0_21R10]_0_0_0_0_0_0_21R10]_0_0_0_0_0_0_0_21R8]_0R8]_21_21_21_21_21_21_0_21_0R8]_21_21_21_21_21_21_0_21_0R213]_21R47]_0_21R15]_0_0_21R13]_0_0_0_0_21R11]_0_0_0_0_0_21R10]_0_0_0_0_0_0_21_21_0_21_21_21_21_21_21_21_0R260]_21R32]_0_21R15]_0_21R15]_0_21R13]_0_0_0_0_21R12]_0R10]_21_21_0R278]_21R48]_0_21R15]_0_21R13]_0_0_0_0_21R12]_0R290]_21R55]_0_21R8]_0_21_21_21_21_21_21_0_21_21_21_21_21_21_0R307]_21_21_0_0_0_21R9]_0_0_0_0_0_0_0_21R9]_0R10]_21_21_21_21_21_21_0R11]_21_21_21_21_21_0R327]_21R9]_0_0_0_0_0_0_0_21R9]_0R11]_21_21_21_21_21_0R11]_21_21_21_0R323]"};
            //    //    break;
            //    //case VoxelObjName.Pigeon:
            //    //    limits = StandardLimits;
            //    //    data = new List<string>{"0R422]_25_29_29_25_0R379]_25_29_25_25_29_25_0R378]_29_29_29_29_29_29_0R373]_25_25_0_0_0_0_29_29_29_29_0_0_0_0_25_25_0R368]_25_29_25_0_0_0_29_29_29_29_0_0_0_25_29_25_0R359]_29_29_0_0_0_0_0_0_0_29_29_29_25_25_29_29_29_29_29_29_25_25_29_29_29_0R358]_29_29_29_29_0_0_0_0_0_0_29R16]_0R358]_29_29_29_29_0_0_0_0_0_0_25_25_29_29_29_25_29_29_29_29_25_29_29_29_25_25_0R358]_29_29_29_29_0_0_0_0_0_0_29_29_25_25_25_29_29_29_29_29_29_25_25_25_29_29_0R358]_26_26_26_26_0_0_0_0_0_0_0_29_29_29_29_29_26_26_26_26_29_29_29_29_29_0R360]_26_26_0R9]_29_29_29_0_0_26_26_0_0_29_29_29_0R361]_26_26_0R14]_26_26_0R366]_13_13_0R14]_26_26_0R366]_13_13_0R743]"};
            //    //    break;
            //    //case VoxelObjName.BossShield:
            //    //   limits = StandardLimits;
            //    //   data = new List<string>{"0R1155]_11R10]_0R374]_11_31R8]_11_0R374]_11_31_31_31_32_32_31_31_31_11_0R374]_11_31_31_32_31_31_32_31_31_11_0R374]_11_31_32_31_32_32_31_32_31_11_0R374]_11_31_31_32_32_32_32_32_31_11_0R375]_11_31_31_31_31_31_31_11_0R377]_11_31_31_31_31_11_0R379]_11_31_31_11_0R381]_11_11_0R1527]"}; 
            //    //   break;
            //    //case VoxelObjName.Grandpa:
            //    //   limits = StandardLimits;
            //    //   data = new List<string>{"0R916]_29R8]_0R8]_29R8]_0R8]_29R8]_0R8]_29R8]_0R8]_29R8]_0R8]_29R8]_0R8]_29R8]_0R8]_16R8]_0R8]_16R8]_0R247]_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29_16_16_16_16_16_16_16_29_29_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_0_16R8]_0R121]_13_13_13_13_13_13_0R10]_13_13_13_13_13_13_0R10]_13_13_13_13_13_13_13_0R9]_13_13_13_13_13_13_13_0R9]_13_13_13_13_13_13_0R10]_13_13_13_13_13_13_0R24]_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_0_16R8]_0R89]_13_13_13_13_13_13_0R10]_13_13_13_13_13_13_0R10]_13_13_13_13_13_13_13_0_0_0_0_0_0_0_13R9]_0_0_0_0_0_0_0_13R9]_0_0_0_0_0_0_0_13R9]_0_0_0_0_0_0_0_13_13_13_13_13_13_13_29_29_0R9]_13_13_13_13_13_13_13_0R10]_16_16_16_16_29_29_0_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_0_16R8]_0R88]_13R8]_0R8]_13R8]_0R8]_13R8]_0_0_0_0_0_0_0_13R10]_0_0_0_0_0_0_13R10]_0_0_0_0_0_0_13R10]_0_0_0_0_0_0_13_13_13_13_13_13_13_29_29_13_0R8]_13_13_13_13_13_13_29_0R10]_16_16_16_16_29_29_0_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_16R12]_0_0_0_0_16R10]_29_16_0_0_0_0_16R10]_29_16_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_0_16R8]_0R87]_13R10]_0_0_0_0_0_0_13R10]_0_0_0_0_0_0_0_13R8]_0_0_0_0_0_0_0_13R10]_0_0_0_0_0_0_13R10]_0R8]_13R8]_0R8]_13_13_13_13_13_29_29_13_0R8]_13_13_13_13_13_13_29_0R10]_16_16_16_16_29_29_0_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_29_16R8]_29_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_0_16R8]_0R86]_13R12]_0_0_0_0_0_13R10]_0_0_0_0_0_0_0_13R8]_0_0_0_0_0_0_0_13R10]_0_0_0_0_0_0_13R10]_0R8]_13_13_13_13_13_13_0R10]_13_13_13_13_13_29_29_0R9]_13_13_13_13_13_13_29_0_0_0_0_0_0_0_29_0_0_16_16_16_16_29_29_29_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_0_16R8]_0R86]_13R12]_0_0_0_0_0_13R10]_0_0_0_0_0_0_0_13R8]_0_0_0_0_0_0_0_13_13_29_13_13_13_13_29_13_13_0_0_0_0_0_0_13_13_29_13_13_13_13_29_13_13_0_0_0_0_0_0_0_29R8]_0R8]_29R8]_0R8]_29R8]_0_0_0_0_0_0_0_29R9]_0_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_0_16R8]_0R86]_13_13_13_13_13_13_12_12_12_13_13_13_0_0_0_0_0_13_13_13_13_13_12_12_12_13_13_0_0_0_0_0_0_0_13R8]_0_0_0_0_0_0_0_13_13_29_13_13_13_13_29_13_13_0_0_0_0_0_0_13_13_29_13_13_13_13_29_13_13_0_0_0_0_0_0_0_29R8]_0R8]_29R8]_0R8]_29R8]_0_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_29R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_16R10]_0_0_0_0_0_0_0_16R8]_0R89]_12_12_0_0_12_12_0R14]_12_12_0_0_0_4_0_0_0_0_16_16_0_29_29_0_29_0_16_16_0_4_0_0_0_4R13]_0_0_0_0_16_16_29_29_29_29_29_29_16_16_0_4_0_0_0_0_0_29R8]_0R8]_29R8]_0R8]_29R8]_0R8]_29R8]_0R8]_29R8]_0R8]_29_29_17_17_17_17_29_29_0R8]_29_17_16_16_16_16_17_29_0R8]_16R8]_0R8]_16_29_29_16_16_29_29_16_0R8]_16_29_17_16_16_29_17_16_0R8]_16R8]_0R8]_16R8]_0R8]_16R8]_0R105]_12_12_0_0_12_12_0R14]_12_12_0R8]_16_16_0_0_0_0_0_0_16_16_0_0_0_0_0_0_16_16_0_0_0_0_0_0_16_16_0_0_0_0_0_0_16_16_0_0_0_0_0_0_16_16_0R25]_29_29_0_29_0R12]_29_29_29_29_0R12]_29_29_29_29_0R11]_29_29_29_29_29_29_0R10]_29_0_0_0_0_29_0R10]_29_29_29_29_29_29_0R12]_16_16_0R14]_16_16_0R27]_29_29_29_0_0_29_29_29_0R8]_29_29_29_0_0_29_29_29_0R315]_16_16_0R14]_16_16_0R935]"};
            //    //   break;
            //    //case VoxelObjName.TestLoad:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> {
            //    //       "0R1190]_6_6_6_6_0R12]_6_6_6_6_0R12]_6_6_6_6_0R347]_6_6_6_6_6_6_0R10]_6_6_6_6_6_6_0R10]_6_6_6_6_6_6_0R310]_33_0R12]_33_0_0_33_0R12]_33_0_0_33_0_0_6R8]_0_0_33_0_0_33_0_0_6R8]_0_0_33_0_0_0_33_0_33_6_6_6_6_6_6_33_0_33_0_0_0_0_0_33_0R8]_33_0R327]_6R8]_0R8]_6R8]_0R9]_6_6_6_6_6_6_0R345]_6R8]_0R8]_6R8]_0R9]_6_6_6_6_6_6_0R310]_33_0R12]_33_0_0_33_0R12]_33_0_0_33_0_0_6R8]_0_0_33_0_0_33_0_0_6R8]_0_0_33_0_0_0_33_0_33_6_6_6_6_6_6_33_0_33_0_0_0_0_0_33_0R8]_33_0R327]_6R8]_0R8]_6R8]_0R9]_6_6_6_6_6_6_0R345]_6R8]_0R8]_6R8]_0R9]_6_6_6_6_6_6_0R310]_33_0R12]_33_0_0_33_0R12]_33_0_0_33_0_0_6R8]_0_0_33_0_0_33_0_0_6R8]_0_0_33_0_0_0_33_0_33_6_6_6_6_6_6_33_0_33_0_0_0_0_0_33_0R8]_33_0R327]_6R8]_0R8]_6R8]_0R9]_29_29_6_6_29_29_0R345]_6R8]_0R8]_6R8]_0R9]_17_29_6_6_17_29_0R347]_6_6_6_6_0R12]_6_6_6_6_0R710]"
                        
            //    //   };
            //    //   break;
            //    //case VoxelObjName.Statue:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_1925_21_6_0_137_11_3_0_2_11_3_0_8_11_3_0_2_11_3_0_6_11_5_0_2_11_5_0_3_11_6_0_2_11_6_0_182_21_6_0_11_21_4_0_12_21_4_0_12_21_1_0_2_21_1_0_12_21_4_0_60_11_1_0_15_11_1_0_2_11_1_0_12_11_1_0_2_11_1_0_12_11_1_0_2_11_1_0_203_21_6_0_11_21_4_0_12_21_4_0_13_21_2_0_13_21_4_0_12_11_1_0_2_11_1_0_12_11_1_0_2_11_1_0_12_11_4_0_12_11_4_0_12_11_4_0_12_11_4_0_13_11_2_0_14_11_2_0_14_11_2_0_172_21_6_0_11_21_4_0_12_21_4_0_13_21_2_0_13_21_4_0_12_11_1_0_2_11_1_0_28_11_4_0_12_11_4_0_12_11_4_0_12_11_4_0_13_11_2_0_14_11_2_0_14_11_2_0_172_21_6_0_11_21_4_0_12_21_4_0_12_21_1_0_2_21_1_0_12_21_4_0_92_11_1_0_2_11_1_0_29_11_2_0_14_11_2_0_172_21_6_0_155_11_1_0_2_11_1_0_29_11_2_0_338_11_1_0_7_11_11_0_8_11_1_0_2_11_1_0_1_11_1_0_362_11_1_0_2_11_1_0_12_11_1_0_2_11_1_0_1350", };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.HangPlace:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_1543_4_2_0_14_4_2_0_14_4_2_0_14_4_2_0_14_4_2_0_11_4_1_0_2_4_2_0_11_4_1_0_2_4_2_0_11_4_1_0_2_4_2_0_14_4_2_0_14_4_2_0_14_4_2_0_14_4_2_0_14_4_2_0_14_4_2_0_14_4_2_0_14_4_2_0_139_4_1_0_6_4_1_0_8_4_8_0_8_4_1_0_6_4_1_0_8_4_1_0_6_4_1_0_8_4_1_0_6_4_1_0_8_4_10_0_105_4_1_0_48_4_1_0_155_4_1_0_6_4_1_0_56_4_2_3_5_4_3_0_121_4_1_0_32_4_1_0_155_4_1_0_6_4_1_0_48_4_2_0_6_4_2_8_1_4_3_3_1_4_1_0_139_4_1_0_16_4_1_0_155_4_1_0_6_4_1_0_32_4_2_0_22_4_2_3_1_4_3_3_1_4_1_0_155_4_2_0_155_4_1_0_6_4_1_0_16_4_2_0_38_4_2_8_1_4_3_3_1_4_1_0_44_29_1_0_15_29_1_0_15_29_1_0_15_29_1_0_15_29_1_0_15_29_1_0_15_29_1_0_15_29_1_0_155_4_1_0_6_4_3_0_54_4_2_3_5_4_1_0_92_29_1_0_63_4_1_0_139_4_1_0_6_4_3_0_6_4_8_0_8_4_1_0_6_4_1_0_8_4_1_0_6_4_1_0_8_4_1_0_6_4_1_0_8_4_8_0_1828", };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.Bone:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_2768_11_2_0_11_11_2_0_1_11_2_0_11_11_2_0_353_11_15_0_1_11_15_0_354_11_13_0_3_11_13_0_354_11_2_0_11_11_2_0_1_11_2_0_11_11_2_0_353_11_2_0_11_11_2_0_1_11_2_0_11_11_2_0_1809", };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.TownWall:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_1920_1_3_0_1_1_6_0_1_1_5_0_368_1_23_0_1_1_4_0_2_1_2_0_352_1_32_4_1_19_14_4_2_19_3_0_1_19_6_0_1_19_3_4_2_19_3_0_1_19_6_0_1_19_3_4_2_19_14_4_2_0_1_19_1_0_1_19_1_0_1_19_1_0_2_19_1_0_1_19_1_0_1_19_1_0_1_4_2_0_14_4_1_0_256_1_18_0_1_1_13_4_1_0_6_4_2_0_6_4_2_0_6_4_2_0_6_4_2_0_6_4_2_0_6_4_18_0_14_4_2_0_14_4_1_0_256_1_6_0_1_1_9_0_2672", };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.TownWallTower:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_1_1_1_0_1_1_4_0_1_1_2_0_1_1_4_0_369_1_16_0_1_1_13_0_3_4_2_19_10_4_2_0_2_4_2_19_10_4_2_0_2_4_2_19_2_0_1_19_4_0_1_19_2_4_2_0_2_4_2_19_2_0_1_19_4_0_1_19_2_4_2_0_2_4_2_19_10_4_2_0_2_4_2_19_10_4_2_0_2_4_2_0_1_19_2_0_3_19_2_0_2_4_2_0_2_4_2_0_10_4_2_0_225_1_16_0_1_1_5_0_1_1_5_4_1_1_2_0_2_4_2_0_8_4_1_0_1_4_2_0_2_4_2_0_7_4_1_0_2_4_2_0_2_4_2_0_6_4_1_0_3_4_2_0_2_4_2_0_5_4_1_0_4_4_2_0_2_4_2_0_4_4_1_0_5_4_2_0_2_4_6_0_5_4_3_0_2_4_2_0_10_4_2_0_2_4_2_0_10_4_2_0_225_1_8_0_1_1_6_0_2_1_1_0_5_4_1_0_4_4_2_1_1_0_2_19_1_0_5_4_1_0_3_4_1_0_2_19_1_0_2_19_1_0_5_4_1_0_2_4_1_0_3_19_1_0_2_19_1_0_5_4_1_0_1_4_1_0_4_19_1_0_2_19_1_0_5_4_2_0_5_19_1_0_2_19_1_0_5_4_1_0_6_19_1_0_2_19_1_4_5_0_5_4_2_19_1_0_257_1_4_0_7_1_5_0_1_1_2_0_9_1_3_0_2_19_1_0_12_19_1_0_2_19_1_0_12_19_1_0_2_19_1_0_12_19_1_0_2_19_1_0_12_19_1_0_2_19_1_0_12_19_1_0_2_19_1_4_2_0_9_4_1_19_1_0_15_19_1_0_241_1_4_0_7_1_5_0_1_1_2_0_9_1_3_0_2_19_1_0_12_19_1_0_2_19_1_0_12_19_1_0_34_19_1_0_12_19_1_0_2_19_1_4_2_0_9_4_1_19_1_0_2_19_1_0_12_19_1_0_241_1_3_0_9_1_4_0_1_1_2_0_9_1_3_0_2_19_1_0_12_19_1_0_2_19_1_0_12_19_1_0_2_19_1_0_12_19_1_0_2_19_1_0_12_19_1_0_2_19_1_0_12_19_1_0_2_19_1_4_2_0_9_4_1_19_1_0_2_19_1_0_255_1_2_0_8_1_5_0_1_1_2_0_9_1_4_0_1_19_1_0_12_19_1_4_1_0_1_19_1_0_12_19_1_4_1_0_1_19_1_0_12_19_1_4_1_0_1_19_1_0_12_19_1_4_1_0_1_19_1_0_12_19_1_4_1_0_1_19_1_4_2_0_9_4_1_19_1_4_1_0_256_1_3_0_8_1_5_0_1_1_2_0_9_1_4_0_1_19_1_0_12_19_1_4_1_0_1_19_1_0_12_19_1_4_1_0_1_19_1_0_12_19_1_4_1_0_1_19_1_0_12_19_1_4_1_0_1_19_1_0_12_19_1_4_1_0_1_19_1_4_2_0_9_4_1_19_1_4_1_0_256_1_4_0_7_1_5_0_1_1_2_0_9_1_3_0_2_19_1_0_12_19_1_0_2_19_1_0_12_19_1_0_2_19_1_0_12_19_1_0_2_19_1_0_12_19_1_0_2_19_1_0_12_19_1_0_2_19_1_4_2_0_9_4_1_19_1_0_15_19_1_0_241_1_4_0_7_4_1_1_3_4_1_0_2_1_1_0_9_4_3_0_2_19_1_0_15_19_1_0_47_19_1_0_12_19_1_0_2_19_1_4_2_0_9_4_1_19_1_0_2_19_1_0_12_19_1_0_242_1_3_0_7_4_1_1_3_4_1_0_1_1_2_0_9_4_3_0_2_19_1_0_15_19_1_0_15_19_1_0_15_19_1_0_15_19_1_0_12_19_1_0_2_19_1_4_2_0_9_4_1_19_1_0_2_19_1_0_254_1_11_4_1_1_3_4_1_0_1_1_2_0_9_4_3_0_2_19_1_0_15_19_1_0_15_19_1_0_15_19_1_0_15_19_1_0_12_19_1_0_2_19_1_4_2_0_9_4_1_19_1_0_257_1_16_0_1_1_6_0_1_1_6_4_1_0_2_4_2_0_10_4_2_0_2_4_2_0_10_4_2_0_2_4_2_0_10_4_2_0_2_4_2_0_10_4_2_0_2_4_2_0_10_4_2_0_2_4_14_0_2_4_2_0_10_4_2_0_2_4_2_0_10_4_2_0_225_1_16_0_1_1_14_0_2_4_2_19_10_4_2_0_2_4_2_19_10_4_2_0_2_4_2_19_2_0_1_19_4_0_1_19_2_4_2_0_2_4_2_19_2_0_1_19_4_0_1_19_2_4_2_0_2_4_2_19_10_4_2_0_2_4_2_19_10_4_2_0_2_4_2_0_2_19_2_0_3_19_2_0_1_4_2_0_2_4_2_0_10_4_2_0_226_1_14_0_8_1_2_0_4_1_1_0_9_4_2_0_14_4_2_0_14_4_2_0_14_4_2_0_14_4_2_0_14_4_2_0_263",};
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.TownHouse:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_465_26_15_0_65_7_15_0_210_26_1_6_5_26_1_6_5_26_1_0_3_26_1_6_5_26_1_6_5_26_1_0_3_26_1_6_2_0_1_6_2_26_1_6_2_0_1_6_2_26_1_0_3_26_1_6_2_0_1_6_2_26_1_6_2_0_1_6_2_26_1_0_3_26_1_6_5_26_1_6_5_26_1_0_2_26_15_0_2_26_1_4_5_26_1_4_5_26_1_0_3_26_1_4_2_0_1_4_2_26_1_4_2_0_1_4_2_26_1_0_3_26_1_4_2_0_1_4_2_26_1_4_2_0_1_4_2_26_1_0_3_26_1_4_5_26_1_4_5_26_1_0_3_26_1_7_11_26_1_0_2_7_15_0_194_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_2_26_15_0_2_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_26_2_7_9_26_2_0_3_26_1_7_11_26_1_0_2_7_15_0_178_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_2_26_15_0_2_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_26_2_7_9_26_2_0_3_4_1_7_11_4_1_0_3_26_1_7_11_26_1_0_2_7_15_0_162_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_35_6_1_0_11_6_1_0_2_26_15_0_2_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_26_2_7_9_26_2_0_3_4_2_7_9_4_2_0_3_4_2_7_9_4_2_0_3_26_2_7_9_26_2_0_2_7_15_0_146_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_35_6_1_0_11_6_1_0_2_26_15_0_2_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_26_2_7_9_26_2_0_3_4_2_7_9_4_2_0_3_4_2_7_9_4_2_0_3_26_2_7_9_26_2_0_2_7_15_0_146_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_2_26_15_0_2_4_1_0_11_4_1_0_35_4_1_0_11_4_1_0_3_26_2_7_9_26_2_0_3_4_2_7_9_4_2_0_3_4_2_7_9_4_2_0_3_26_2_7_9_26_2_0_2_7_15_0_3_6_2_0_7_6_2_0_5_6_2_0_7_6_2_0_116_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_2_26_15_0_2_4_1_0_11_4_1_0_35_4_1_0_11_4_1_0_3_26_2_7_9_26_2_0_3_4_2_7_9_4_2_0_3_4_2_7_9_4_2_0_3_26_2_7_9_26_2_0_2_7_15_0_3_6_2_0_7_6_2_0_5_6_2_0_7_6_2_0_116_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_35_6_1_0_11_6_1_0_2_26_15_0_2_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_26_2_7_9_26_2_0_3_4_2_7_9_4_2_0_3_4_2_7_9_4_2_0_3_26_2_7_9_26_2_0_2_7_15_0_146_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_35_6_1_0_11_6_1_0_2_26_15_0_2_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_26_2_7_9_26_2_0_3_4_2_7_9_4_2_0_3_4_2_7_9_4_2_0_3_26_2_7_9_26_2_0_2_7_15_0_146_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_2_26_15_0_2_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_26_2_7_9_26_2_0_3_4_1_7_11_4_1_0_3_26_1_7_11_26_1_0_2_7_15_0_162_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_3_6_1_0_11_6_1_0_2_26_15_0_2_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_4_1_0_11_4_1_0_3_26_2_7_9_26_2_0_3_26_1_7_11_26_1_0_2_7_15_0_178_26_1_6_5_26_1_6_1_4_3_6_1_26_1_0_3_26_1_6_5_26_1_6_1_4_2_3_1_6_1_26_1_0_3_26_1_6_2_0_1_6_2_26_1_6_1_4_3_6_1_26_1_0_3_26_1_6_2_0_1_6_2_26_1_6_1_4_3_6_1_26_1_0_3_26_1_6_5_26_1_6_5_26_1_0_2_26_15_0_2_26_1_4_5_26_1_4_5_26_1_0_3_26_1_4_2_0_1_4_2_26_1_4_2_0_1_4_2_26_1_0_3_26_1_4_2_0_1_4_2_26_1_4_2_0_1_4_2_26_1_0_3_26_1_4_5_26_1_4_5_26_1_0_3_26_1_7_11_26_1_0_2_7_15_0_273_26_15_0_65_7_15_0_592", };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    ////case VoxelObjName.CastleRingWall:
            //    ////   limits = StandardLimits;
            //    //   data = new List<string> { "0_2068_33_1_0_6_33_1_0_4_33_35_0_1_33_3_0_2_33_3_0_1_33_3_0_176_33_195_0_1_33_3_0_2_33_3_0_1_33_3_0_176_33_192_0_192_33_176_0_208_33_176_0_354_33_1_0_4_33_2_0_4_33_1_0_2_33_16_0_2128" };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.CastleIndoorWall:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "33_48_0_336_33_48_0_5712" };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.FireCannon:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_2310_33_4_0_12_33_4_0_13_25_2_0_13_25_1_0_2_25_1_0_12_25_1_0_2_25_1_0_13_25_2_0_301_33_4_0_12_33_4_0_13_25_2_0_13_25_1_14_2_25_1_0_12_25_1_14_2_25_1_0_13_25_2_0_301_33_4_0_12_33_4_0_13_25_2_0_13_25_4_0_12_25_4_0_13_25_2_0_301_33_4_0_12_33_4_0_29_25_2_0_14_25_2_0_2615", };
            //    //   version = SaveVersion.Ver2;
            //    //   break;

            //    //case VoxelObjName.Tree1:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_964_5_3_0_27_5_3_0_350_5_5_0_10_3_1_0_15_5_4_0_12_5_3_0_334_3_2_5_3_0_8_5_2_3_2_5_2_0_12_5_4_0_11_5_4_0_145_3_2_0_14_3_2_0_141_3_4_0_12_3_2_0_12_3_3_5_1_0_9_5_6_0_3_5_3_0_6_5_5_0_10_5_4_0_145_3_4_0_12_3_3_0_14_3_2_0_14_3_2_0_14_3_2_0_14_3_2_0_14_3_2_0_14_3_2_0_14_3_2_0_14_3_2_0_11_3_5_5_2_0_9_3_3_0_13_3_2_0_11_5_3_0_5_5_3_0_7_5_4_0_6_5_2_0_2_5_4_0_5_5_5_0_136_3_5_0_11_3_4_0_12_3_3_0_13_3_3_0_13_3_3_0_13_3_3_0_13_3_3_0_13_3_3_0_12_3_4_0_12_3_4_0_12_3_5_0_14_3_3_0_14_3_3_0_4_5_3_0_5_5_3_3_2_0_5_5_4_0_6_5_2_0_3_5_3_0_4_5_7_0_11_5_3_0_122_3_4_0_12_3_3_0_13_3_2_0_14_3_1_0_15_3_1_0_15_3_1_0_15_3_1_0_15_3_1_0_15_3_1_0_15_3_1_0_14_3_4_0_14_3_2_0_14_3_4_0_15_3_2_0_5_5_4_0_6_5_2_0_10_5_7_0_11_5_4_0_121_3_2_0_14_3_1_0_142_3_2_0_14_3_1_0_19_5_2_0_16_5_3_0_3_5_3_0_7_5_2_0_10_5_7_0_11_5_5_0_120_3_1_0_158_3_1_0_15_3_1_0_19_5_2_0_8_5_3_0_5_5_3_0_25_5_7_0_11_5_5_0_295_3_1_0_15_3_1_0_13_5_4_0_4_5_2_0_6_5_3_0_17_5_7_0_13_5_3_0_308_3_4_0_12_5_3_3_1_5_1_0_12_5_3_0_18_5_5_0_341_5_5_0_12_5_3_0_364_5_3_0_15_5_1_0_537", };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.Tree2:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_548_5_3_0_15_5_1_0_365_5_5_0_12_5_3_0_351_3_1_0_12_5_3_3_1_5_1_0_10_5_9_0_14_5_5_0_292_5_2_0_34_3_1_0_13_5_4_0_4_5_2_0_3_5_10_0_13_5_7_0_13_5_1_0_276_5_3_0_14_5_3_0_1_3_1_0_15_3_1_0_1_5_4_0_8_5_3_0_1_5_2_0_2_5_3_0_2_5_3_0_3_5_4_0_8_5_12_0_6_5_3_0_2_5_3_0_24_5_2_0_17_5_2_0_125_3_1_0_105_5_3_0_2_3_2_0_10_5_3_0_1_3_1_0_17_5_4_0_9_3_1_0_2_5_2_0_2_5_3_0_2_5_4_0_2_5_4_0_1_5_2_0_5_5_12_0_5_5_5_0_1_5_3_0_23_5_3_0_17_5_3_0_123_3_3_0_14_3_1_0_15_3_1_0_15_3_1_0_15_3_1_0_15_3_1_0_15_3_1_0_10_5_2_0_2_3_4_0_8_5_3_0_2_3_3_0_13_3_1_5_2_3_2_0_10_3_2_5_2_0_1_3_2_0_5_5_4_0_1_3_1_5_3_0_1_5_2_0_5_5_4_3_1_5_1_0_3_5_3_0_5_5_3_3_1_5_1_0_3_5_2_0_21_5_4_0_14_5_6_0_121_3_5_0_12_3_4_0_13_3_3_0_13_3_3_0_13_3_3_0_13_3_3_0_13_3_3_0_13_3_4_0_8_5_3_0_1_3_5_0_10_3_7_0_4_5_3_0_1_3_5_5_2_3_1_5_3_0_3_5_4_3_3_5_2_0_1_5_2_0_3_5_5_3_2_0_4_5_3_0_5_5_2_3_2_0_4_5_1_0_8_3_3_0_11_5_3_3_2_0_13_5_6_0_11_5_3_0_109_3_3_0_14_3_1_0_15_3_1_0_15_3_1_0_15_3_1_0_15_3_1_0_15_3_1_0_15_3_1_0_11_3_3_0_1_3_1_0_1_5_2_0_8_3_2_0_2_3_1_5_3_0_5_5_3_0_4_3_1_5_3_0_1_5_3_0_3_5_4_0_1_3_1_5_6_0_2_5_7_3_1_5_1_0_3_5_2_0_6_5_3_3_1_5_1_0_13_3_1_0_12_5_3_3_1_0_1_3_1_0_1_5_2_0_7_5_8_0_11_5_3_0_110_3_1_0_124_3_2_0_3_5_2_0_7_3_3_5_1_0_3_5_2_0_4_5_6_0_2_3_1_5_3_3_1_5_3_0_2_5_13_0_2_5_1_0_2_5_1_3_1_5_6_0_10_5_3_0_29_3_2_0_3_5_2_0_7_5_8_0_11_5_3_0_249_3_2_5_7_0_4_5_2_3_2_5_2_0_3_3_2_0_1_3_1_0_5_5_13_0_2_5_2_0_1_5_8_0_43_3_1_0_3_5_2_0_7_5_7_0_264_5_9_0_6_3_1_0_1_3_2_0_5_3_1_0_1_5_2_0_3_5_13_0_3_5_6_0_51_5_2_0_7_5_2_0_1_5_4_0_265_5_3_0_1_5_4_0_17_5_2_0_3_5_12_0_7_5_3_0_61_5_4_0_271_5_4_0_25_5_7_0_349_5_2_0_29_5_3_0_197", };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.Tree3:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_1655_5_2_0_381_5_4_0_29_5_3_0_346_5_7_0_13_5_3_0_11_5_4_0_29_5_3_0_206_3_1_0_15_3_1_0_15_3_1_0_15_3_1_0_15_3_1_0_15_3_1_0_13_5_2_3_1_5_1_0_10_5_4_3_1_5_2_0_10_5_3_3_1_5_3_0_10_5_2_3_1_5_1_0_14_3_1_0_13_5_4_0_14_5_1_0_285_5_1_3_2_5_1_0_11_5_6_0_10_5_7_0_10_5_4_0_28_5_4_0_14_5_1_0_286_5_2_0_13_5_1_3_1_5_3_0_10_5_7_0_11_5_2_0_31_5_2_0_317_3_1_5_2_0_12_5_3_0_14_5_2_0_31_5_1_0_334_5_2_0_1783", };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.Ruin:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_1159_33_2_0_15_33_1_0_366_33_2_0_14_1_1_33_1_0_366_33_2_0_14_1_1_33_1_0_361_33_1_0_2_33_1_0_1_33_4_0_12_33_2_1_1_0_14_1_1_0_350_33_4_0_12_33_2_1_1_0_14_33_1_0_347_33_1_0_2_33_2_0_14_33_2_0_14_33_2_0_15_33_1_0_334_33_2_1_1_33_2_0_11_33_2_0_14_33_2_0_14_33_2_0_334_33_2_1_1_33_2_0_11_33_1_1_1_0_14_33_1_1_1_0_347_33_5_0_12_33_1_1_1_33_2_0_14_1_2_0_347_33_5_1_1_0_11_1_2_33_2_0_14_33_2_0_14_1_2_0_334_33_2_0_14_33_2_0_14_33_1_1_1_0_14_33_2_0_334_1_2_0_759", };
            //    //   version = SaveVersion.Ver2;
            //    //   break;

            //    //case VoxelObjName.DeadTree:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_2311_3_1_0_15_3_1_0_15_3_1_0_350_3_4_0_12_3_3_0_14_3_2_0_14_3_2_0_14_3_2_0_14_3_2_0_14_3_3_0_13_3_2_0_1_3_2_0_11_3_2_0_14_3_2_0_14_3_2_0_14_3_1_0_207_3_4_0_12_3_3_0_13_3_3_0_13_3_2_0_14_3_2_0_14_3_2_0_14_3_2_0_13_3_3_0_12_3_1_0_1_3_2_0_14_3_2_0_239_3_1_0_15_3_1_0_15_3_1_0_79_3_1_0_2567", };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.RCCar1:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_54_8_4_0_12_8_4_0_59_8_1_0_4_8_1_0_263_25_2_0_8_25_2_0_4_25_2_0_1_8_1_48_4_8_1_0_1_25_2_0_4_25_2_24_1_8_6_24_1_25_2_0_7_8_6_0_10_8_1_0_4_8_1_0_10_8_1_0_4_8_1_0_10_8_1_0_4_8_1_0_10_8_1_0_4_8_1_0_247_25_2_0_8_25_2_0_5_25_1_0_8_25_1_0_6_25_1_0_1_48_6_0_1_25_1_0_6_25_1_0_1_48_6_0_1_25_1_0_5_25_2_24_1_48_6_24_1_25_2_0_7_43_6_0_10_43_6_0_10_43_6_0_263_25_2_0_8_25_2_0_5_25_1_0_8_25_1_0_6_25_1_0_1_48_6_0_1_25_1_0_6_25_1_0_1_48_6_0_1_25_1_0_5_25_2_24_1_48_1_0_4_48_1_24_1_25_2_0_7_48_1_0_4_48_1_0_26_43_6_0_263_25_2_0_8_25_2_0_5_25_1_0_8_25_1_0_6_25_1_0_1_48_6_0_1_25_1_0_6_25_1_0_1_48_6_0_1_25_1_0_5_25_2_24_1_48_1_0_4_48_1_24_1_25_2_0_7_48_1_0_4_48_1_0_26_43_6_0_279_25_2_0_8_25_2_0_4_25_2_0_1_24_1_48_4_24_1_0_1_25_2_0_4_25_2_0_1_48_6_0_1_25_2_0_6_24_1_48_1_0_4_48_1_24_1_0_9_48_1_0_4_48_1_0_26_43_6_0_297_24_2_48_4_24_2_0_8_24_1_48_6_24_1_0_9_48_1_0_4_48_1_0_10_48_1_0_4_48_1_0_26_43_6_0_297_24_2_48_4_24_2_0_9_48_6_0_10_48_1_0_4_48_1_0_10_48_1_0_4_48_1_0_26_43_6_0_297_24_2_48_4_24_2_0_9_48_6_0_10_48_6_0_10_48_6_0_10_48_1_0_4_48_1_0_10_43_6_0_297_24_2_48_4_24_2_0_10_48_4_0_12_48_4_0_13_48_2_0_331_24_2_48_4_24_2_0_8_24_2_48_4_24_2_0_10_48_4_0_13_48_2_0_314_25_2_0_6_25_2_0_6_25_2_0_1_48_4_0_1_25_2_0_9_48_4_0_10_24_2_48_4_24_2_0_11_48_2_0_298_25_2_0_6_25_2_0_7_25_1_17_6_25_1_0_8_25_1_17_6_25_1_0_7_25_2_0_1_48_4_0_1_25_2_0_7_24_2_48_4_24_2_0_11_48_2_0_298_25_2_0_6_25_2_0_7_25_1_17_6_25_1_0_8_25_1_17_6_25_1_0_7_25_2_0_1_48_4_0_1_25_2_0_7_24_2_48_4_24_2_0_11_48_2_0_314_25_2_0_6_25_2_0_6_25_2_0_1_48_1_24_2_48_1_0_1_25_2_0_9_48_1_24_2_48_1_0_10_24_2_48_1_24_2_48_1_24_2_0_11_48_2_0_316_24_6_0_11_24_1_0_2_24_1_0_12_24_1_0_2_24_1_0_10_8_2_24_1_0_2_24_1_8_2_0_8_8_2_0_1_24_2_0_1_8_2_0_292" };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.RCTank1:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_21_48_6_0_8_37_2_48_1_0_4_48_1_37_2_0_8_48_1_8_4_48_1_0_8_37_2_48_1_0_4_48_1_37_2_0_8_48_1_8_4_48_1_0_8_37_2_48_6_37_2_0_278_37_2_48_6_37_2_0_5_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_1_0_4_48_1_37_2_48_1_0_4_48_1_37_2_48_1_8_4_48_1_37_2_48_1_0_4_48_1_37_2_48_1_0_4_48_1_37_2_48_1_0_4_48_1_37_2_48_1_8_4_48_1_37_2_48_1_0_7_48_1_13_4_48_1_0_282_48_6_0_7_48_1_37_2_48_6_37_2_48_1_0_5_48_1_37_1_48_1_8_4_48_1_37_1_48_1_0_5_48_1_37_2_48_1_8_4_48_1_37_2_48_1_0_5_48_1_37_1_48_1_8_4_48_1_37_1_48_1_0_5_48_1_37_2_48_2_13_2_48_2_37_2_48_1_0_5_37_2_48_1_13_1_0_2_13_1_48_1_37_2_0_278_37_2_48_6_37_2_0_5_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_1_8_4_48_1_37_2_48_1_0_4_48_1_37_2_48_1_8_4_48_1_37_2_48_1_0_4_48_1_37_2_48_1_8_4_48_1_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_7_48_1_13_4_48_1_0_282_48_6_0_7_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_5_37_2_48_6_37_2_0_41_48_4_0_12_13_4_0_12_48_4_0_201_37_2_48_6_37_2_0_5_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_7_48_6_0_43_48_4_0_12_13_4_0_12_48_4_0_203_48_6_0_7_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_5_37_2_48_6_37_2_0_9_48_4_0_12_25_4_0_12_48_4_0_12_13_4_0_12_48_1_13_2_48_1_0_201_37_2_48_6_37_2_0_5_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_7_48_6_0_11_48_4_0_12_25_4_0_12_48_4_0_12_13_4_0_12_48_4_0_203_48_6_0_7_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_7_48_6_0_11_25_4_0_12_48_4_0_12_13_4_0_12_48_4_0_201_37_2_48_6_37_2_0_5_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_5_37_2_48_6_37_2_0_9_25_4_0_12_48_4_0_12_48_4_0_12_48_4_0_203_48_6_0_7_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_7_48_6_0_11_25_4_0_12_48_4_0_12_48_4_0_12_48_4_0_201_37_2_48_6_37_2_0_5_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_5_37_2_48_6_37_2_0_26_48_2_0_14_48_2_0_218_37_2_48_6_37_2_0_8_48_6_0_7_48_1_37_2_48_6_37_2_48_1_0_4_48_1_0_2_48_6_0_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_5_48_1_37_1_48_6_37_1_48_1_0_5_48_1_37_2_48_6_37_2_48_1_0_7_48_6_0_28_48_2_0_14_48_2_0_250_37_2_48_6_37_2_0_8_48_6_0_7_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_4_48_1_37_2_48_6_37_2_48_1_0_5_37_2_48_6_37_2_0_26_48_2_0_14_48_2_0_282_37_2_48_6_37_2_0_8_48_6_0_8_37_2_48_6_37_2_0_8_48_6_0_28_25_2_0_14_25_2_0_316_48_6_0_10_48_6_0_245" };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
                
            //    //case VoxelObjName.HatCap:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_675_9_10_0_6_8_10_0_6_8_10_0_7_8_8_0_327_9_10_0_6_8_10_0_6_8_10_0_6_8_10_0_326_9_10_0_6_8_10_0_6_8_10_0_6_8_10_0_326_9_10_0_6_8_10_0_6_8_10_0_6_8_10_0_326_9_10_0_6_8_10_0_6_8_10_0_6_8_10_0_326_9_10_0_6_8_10_0_6_8_10_0_6_8_10_0_326_9_10_0_6_8_10_0_6_8_10_0_6_8_10_0_326_9_10_0_6_8_10_0_6_8_10_0_6_8_10_0_326_9_10_0_6_8_10_0_6_8_10_0_6_8_10_0_326_9_10_0_6_8_10_0_6_8_10_0_6_8_10_0_326_9_10_0_6_8_10_0_6_8_10_0_7_8_8_0_327_9_10_0_374_9_10_0_374_9_10_0_375_9_8_0_84", };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.HatFootball:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_580_8_3_9_2_8_3_0_8_8_3_9_2_8_3_0_8_8_3_9_2_8_3_0_8_8_3_9_2_8_3_0_8_8_3_9_2_8_3_0_9_8_2_9_2_8_2_0_296_8_10_0_6_8_10_0_6_8_10_0_6_8_10_0_6_8_10_0_6_8_10_0_6_8_4_9_2_8_4_0_7_8_3_9_2_8_3_0_263_8_10_0_5_8_12_0_4_8_12_0_4_8_12_0_4_8_12_0_4_8_12_0_5_8_10_0_6_8_10_0_7_8_3_9_2_8_3_0_246_8_2_0_8_8_2_0_4_8_12_0_4_8_12_0_4_8_12_0_4_8_12_0_4_8_12_0_4_8_12_0_5_8_10_0_7_8_3_9_2_8_3_0_246_8_1_0_10_8_1_0_4_8_12_0_4_8_12_0_4_8_12_0_4_8_12_0_4_8_12_0_4_8_12_0_5_8_10_0_7_8_3_9_2_8_3_0_246_8_1_0_10_8_1_0_4_8_12_0_4_8_12_0_4_8_12_0_4_8_12_0_4_8_12_0_4_8_12_0_5_8_10_0_7_8_3_9_2_8_3_0_246_8_1_0_10_8_1_0_4_8_12_0_4_8_12_0_4_8_12_0_4_8_12_0_4_8_12_0_4_8_12_0_5_8_10_0_7_8_3_9_2_8_3_0_246_8_1_0_10_8_1_0_4_8_1_0_10_8_1_0_4_8_1_0_10_8_1_0_4_8_1_0_10_8_1_0_4_8_12_0_4_8_12_0_4_8_12_0_5_8_10_0_7_8_3_9_2_8_3_0_246_8_1_0_10_8_1_0_4_8_1_0_10_8_1_0_4_8_1_0_10_8_1_0_4_8_2_0_8_8_2_0_5_8_1_0_8_8_1_0_6_8_1_0_8_8_1_0_6_8_10_0_6_8_10_0_7_8_3_9_2_8_3_0_247_49_1_0_7_49_2_0_70_49_1_0_8_49_1_0_6_8_10_0_6_8_10_0_7_8_3_9_2_8_3_0_247_49_1_0_7_49_2_0_70_49_10_0_6_8_4_9_2_8_4_0_7_8_3_9_2_8_3_0_232_49_1_0_6_49_1_0_8_49_1_0_6_49_1_0_8_49_1_0_6_49_1_0_72_49_8_0_265_49_6_0_11_49_1_0_2_49_1_0_11_49_6_0_949", };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.HatSpartan:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_628_8_8_0_26_8_4_0_12_8_4_0_13_8_2_0_14_9_2_0_14_9_2_0_282_8_10_0_7_8_8_0_8_8_8_0_8_8_8_0_10_8_4_0_13_8_2_0_14_9_2_0_14_9_2_0_265_8_12_0_5_8_10_0_6_8_10_0_6_8_10_0_7_8_8_0_10_8_4_0_13_8_2_0_14_9_2_0_14_9_2_0_249_8_12_0_5_8_10_0_6_8_10_0_6_8_10_0_7_8_8_0_10_8_4_0_13_8_2_0_14_9_2_0_14_9_2_0_249_8_12_0_5_8_10_0_6_8_10_0_6_8_10_0_7_8_8_0_10_8_4_0_13_8_2_0_14_9_2_0_14_9_2_0_282_8_1_0_8_8_1_0_6_8_10_0_7_8_8_0_10_8_4_0_13_8_2_0_14_9_2_0_14_9_2_0_282_8_1_0_8_8_1_0_6_8_10_0_7_8_8_0_10_8_4_0_13_8_2_0_14_9_2_0_14_9_2_0_266_8_1_0_8_8_1_0_6_8_1_0_8_8_1_0_6_8_10_0_7_8_8_0_10_8_4_0_13_8_2_0_14_9_2_0_14_9_2_0_186_8_2_0_7_8_1_0_6_8_2_0_7_8_1_0_6_8_2_0_7_8_1_0_6_8_2_0_7_8_1_0_6_8_1_0_8_8_1_0_6_8_1_0_8_8_1_0_6_8_1_0_8_8_1_0_6_8_10_0_7_8_8_0_10_8_4_0_13_8_2_0_14_9_2_0_14_9_2_0_186_8_2_0_7_8_1_0_6_8_2_0_7_8_1_0_6_8_2_0_7_8_1_0_6_8_2_0_7_8_1_0_6_8_1_0_8_8_1_0_6_8_1_0_8_8_1_0_6_8_10_0_6_8_10_0_7_8_8_0_10_8_4_0_13_8_2_0_14_9_2_0_14_9_2_0_171_8_1_0_6_8_1_0_8_8_1_0_6_8_1_0_8_8_1_0_6_8_1_0_8_8_1_0_6_8_1_0_8_8_1_0_6_8_1_0_8_8_1_0_2_8_2_0_2_8_1_0_11_8_2_0_11_8_8_0_7_8_10_0_9_8_4_0_13_8_2_0_14_9_2_0_14_9_2_0_14_9_2_0_156_8_1_0_4_8_1_0_10_8_1_0_4_8_1_0_10_8_1_0_4_8_1_0_10_8_1_0_4_8_1_0_10_8_1_0_4_8_1_0_10_8_1_0_4_8_1_0_12_8_2_0_14_8_2_0_11_8_8_0_11_8_2_0_62_9_2_0_14_9_2_0_382_9_2_0_775",
            //    //        };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //    //case VoxelObjName.HatWitch:
            //    //   limits = StandardLimits;
            //    //   data = new List<string> { "0_290_8_12_0_371_8_14_0_370_8_14_0_370_8_14_0_5_9_8_0_8_8_8_0_341_8_14_0_5_9_8_0_8_8_8_0_9_8_6_0_12_8_4_0_13_8_2_0_295_8_14_0_5_9_8_0_8_8_8_0_9_8_6_0_12_8_4_0_13_8_3_0_294_8_14_0_5_9_8_0_8_8_8_0_9_8_6_0_12_8_4_0_13_8_3_0_294_8_14_0_5_9_8_0_8_8_8_0_9_8_6_0_12_8_4_0_13_8_3_0_294_8_14_0_5_9_8_0_8_8_8_0_9_8_6_0_12_8_4_0_310_8_14_0_5_9_8_0_8_8_8_0_9_8_6_0_326_8_14_0_5_9_8_0_8_8_8_0_341_8_14_0_371_8_12_0_373_8_10_0_375_8_8_0_468", };
            //    //   version = SaveVersion.Ver2;
            //    //   break;
            //}

            //return LoadObjFromFile(data, limits, version);

        }

    }
    enum SaveVersion
    {
        Ver1,
        Ver2,
    }
    enum VoxelObjName
    {
        boss_lock,
        goldsmith_station,
        bow_station,
        armour_station,
        cook_station,
        vulcan_station,
        sheild_station,
        salesman_station,
        lumberjack_station,
        miner_station,
        healer_station,
        weaponsmith_station,
        blacksmith_station,
        wiselady_station, wiselady_cabin,
        tailor_station,
        priest_station,
        //arcade_tank1, arcade_tank2, arcade_tank3, 
        //arcade_bullet, arcade_base, arcade_spawner, stone_block, wood_block,
        magician, end_magician, end_magician_riding, 
        orch, orc2, orch_leader, orc_leader2, grunt, grunt2,
        evil_spider,
        squig_lvl1, squig_lvl2,
        wolf_lvl1, wolf_lvl2,
        hog_lvl1, hog_lvl2, old_swine,
        crockodile1, crockodile2, frog1, frog2, fire_goblin, fire_goblin2, 
        scorpion1, scorpion2, lizard1, lizard2, spider1, spider2, 
        ent, ent2, bee, bee2,
        endbossmount,
        ogre_body, ogre_arm, ogre_leg,
        BlockTrap, shootingturret,
        mommy, ghost,
        //EnemyDog,
        //EnemySpider,
        //EnemySquig,
        //EnemySlime,
        //EnemyBlock,

        //Stone1,
        //Stone2,
        //Tree1,
        //Tree2,
        //Tree3,
        //DeadTree,
        //Stub,
        //Skeleton,

        EnemyProjectile, enemy_projectile_green, ent_root,
        endmagician_projectile,
        stick,
        sword_base, axe_base, axe2h_base, knife_base, pickaxe_base, sickle_base, spear_base, buildhammer,
        Sword1, pickaxe,
        Sword2, axe, broom, magicstaff,
        staff_fire, staff_poision, staff_light, staff_evil,
        Sword3, doubleaxe, pimpstick,
        shortbow, longbow, bronzebow, ironbow, silverbow, goldbow, mithrilbow, firebow, poisionbow, lightningbow, evilbow,
        slingshot,
        warvet_sword,
        npc_fork, npc_spearaxe,
        orc_sword1, orc_sword2, orcbow,
        magician_sword,

        EyeNormal, EyeCross, EyeEvil, EyeFrown, EyeLoony, EyeRed, EyeSleepy, EyeSlim, EyeSunshine, EyesCrossed1, EyesCrossed2, EyesCyclops,
        EyesPirate,
        EyesGirly1, EyesGirly2, EyesGirly3,

        MouthOrch, MouthBigSmile, MouthHmm, MouthLoony, MouthOMG, MouthSmile, MouthSour, MouthWideSmile, MouthStraight, MouthGasp, mouth_laugh,
        MouthPirate,
        MouthGirly1, MouthGirly2,

        express_anger, express_hi, express_laugh, express_teasing, express_thumbup,

        chest_open, chest_closed,
        discard_pile,
        Character,
        
       
        Coin,
        bosskey,
        stone_heart,

        Arrow, golden_arrow, slingstone,
        orc_arrow,
        //HeartContainer,
        
        //House,
        
        //Hill,
        //Cactus,
        Apple,
        smallhealthup, smallmagicup,
        apple_scrap, applepie_scrap, bread_scrap, grape_scrap, meat_scrap,
        HelmetVendel,
        HelmetHorned1, HelmetHorned2, HelmetHorned3, HelmetHorned4,
        HelmetKnight,
      
        HatCap,
        HatFootball,
        HatSpartan,
        HatWitch,
        HairGirly1, HairGirly2,
        HatPirate1, HatPirate2, HatPirate3,
        belt_slim, belt_thick,
        BeardSmall,
        BeardLarge,
        Mustache,
        MustacheBikers,
        //castle_brickwall,
        //castle_bricktower,
        //castle_blackwall,
        //castle_blackopening,
        //castle_blacktower,
        end_tomb, end_pillar,
        //CastleDoor,
        //CastleRingWall,
        //castle_ringtower,
        //CastleIndoorWall,
        //CastleWallSmith,
        //CastleWallTarget,
        Well,
        //castle_tower, castle_wall, castle_ringwall1, castle_ringwall2,
        //yardopenE1, yardopenS1, yardwallE1, yardwallS1,
       
        
        //SalesTent,
        //uttonY,
        //Hawk,
        ApplePie,
        //Bow1,
        //Bow2,
        //Bow3,
        //Sword1Icon,
        //Sword2Icon,
        //Sword3Icon,
        //Mountain,
        //Pigeon,
        //BossShield,
        Grandpa,
        messenger,
        war_veteran,
        Shield1,
        Shield2,
        Shield3,
        healup_effect, magicup_effect,
        Guard,
        ThrowingSpear,
        //TestLoad,
        //HangPlace,
        //Statue,
        //MainMenuBackground,
        Humanoid,
        //TempCharacter,
        temp_block,
        temp_block_animated,
        dummie,
        Pig,
        Hen,
        white_hen,
        sheep,
        pet_pig,
        pet_falcon,
        pet_dragon,
        pet_angel,
        pet_bird,
        pet_projectile_angel,
        pet_projectile_pig,
        pet_bird_projectile,

        FenceYard,
        Bone, loot_leather, loot_feather, loot_sack, mining_loot,
        homehouse, privatehouse,
        buildarea1, buildarea2, buildarea3,
        //TownWall,
        //TownWallTower,
        //TownHouse,
        //FireCannon, 
        witch_magic,
        FireBall, fire_goblin_ball,
      //  Ruin,
        //RCTank1, RCCar1, RCplane1, RCShip1, RCheli1,
        //rc_ball,
        
        cape,

        
        zombie1, zombiebaby, harpy, harpy2, wizard, zombieleader, zombiemom, fatzombie,
        babyaxe,
        building_hero,
        present,
        //swamptree1, swamptree2, swamptree3, swamptree4, swamptree5, mushroom1, mushroom2, mushroom_group, mushroom_group2,
        //waterfall, stoneplatform1, stoneplatform2, mountain1, mountain2, mountain3,
        //skullstatue, burnedmountain, blacktree1, blacktree2, volcano, volcano2,
        //sandstone1, sandstone2, whitetree, cactus1, cactus2, cactus3,
        //forresttree, forresttree2, forresttree3, forresttree4, mossystone, mossystone2,
        
        horsetransport,
        npc_male, npc_female,
        npc_female_hair1, npc_female_hair2, npc_female_hair3,
        father, mother, guardHead, guardCaptainHead,
        im_error, im_glitch, im_bug,
        horse_hat, workerhat,
        thunder, lightning, light_spark,
        poision_mushroom,
        poision_emitter,
        eggnest, beehive, 
        granpa2,
        Salesman, Lumberjack, Miner, blacksmith,
        housebuilder,
        cook,
        healer,
        wiselady, priest,
        weaponsmith, armorsmith, bowmaker,
        tailor,
        volcan,
        banker,

        i, enemyattention, target_warning,
        bottle,
        explosion,
        using_menu, using_guide, using_build, using_rc,
        barrelX,
        TM_Lumberjack,
        TM_Zeus,
        TM_shop,
        TM_spider,
        Wolf,//the pet version
        herb_fire, herb_leaf, herb_red, herb_rose,
        mining_spot,
        NUM_Empty
    }
    enum CenterAdjType
    {
        None,
        BottomCenter,
        CenterAll,
    }
}
