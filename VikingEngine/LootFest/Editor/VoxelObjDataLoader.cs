using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Voxels;

namespace VikingEngine.LootFest.Editor
{
    static class VoxelObjDataLoader
    {
        public const int StandardBlockWitdh = 16;
        public const int StandardHalfBlockWitdh = StandardBlockWitdh / PublicConstants.Twice;
        public static readonly IntVector3 StandardLimits = new IntVector3(StandardBlockWitdh - 1, 23, StandardBlockWitdh -1);

        //public static Graphics.VoxelModel GetVoxelObjAnim(VoxelModelName name)
        //{
        //    if (name == VoxelModelName.NUM_NON)
        //    {
        //        throw new Exception();
        //    }
        //    return GetVoxelObjAnim(name, Vector3.Zero);
        //}
        public static Graphics.VoxelModel GetVoxelObjAnimWithColReplace(VoxelModelName name, Vector3 centerAdjust, List<byte> find, List<byte> replace)
        {
            if (name == VoxelModelName.NUM_NON)
            {
                throw new Exception();
            }
            //skapa ett voxelObj som kan hantera frames
            List<VoxelObjGridData> loadedFrames = LoadObj(name, find, replace);
            List<List<Voxel>> voxels = new List<List<Voxel>>();
            foreach (VoxelObjGridData data in loadedFrames)
            {
                voxels.Add(data.GetVoxelArray());
            }
            return VoxelObjBuilder.BuildAnimatedFromVoxels(new IntervalIntV3(IntVector3.Zero, loadedFrames[0].Limits), voxels, loadedFrames[0].BottomCenterAdj() + centerAdjust);
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

        public static Graphics.VoxelModel GetVoxelObjAnim(VoxelModelName name, Vector3 centerAdjust)
        {
            if (name == VoxelModelName.NUM_NON)
            {
                throw new Exception();
            }
            //skapa ett voxelObj som kan hantera frames
            List<VoxelObjGridDataHD> loadedFrames = LoadVoxelObjGrid(name);
            List<List<VoxelHD>> voxels = new List<List<VoxelHD>>();
            foreach (var data in loadedFrames)
            {
                voxels.Add(data.GetVoxelArray());
            }
            Graphics.VoxelModel result = VoxelObjBuilder.BuildModelHD(voxels, loadedFrames[0].Size, loadedFrames[0].BottomCenterAdj() + centerAdjust);
            result.SetOneScale(loadedFrames[0].Size);
            return result;
        }
        //public static Graphics.VoxelObj GetVoxelObj(VoxelModelName name, bool addToRender)
        //{
        //    if (name == VoxelModelName.NUM_NON)
        //    {
        //        throw new Exception();
        //    }

        //    return GetVoxelObj(name, addToRender, CenterAdjType.BottomCenter, Vector3.Zero);
        //}
        //public static Graphics.VoxelObj GetVoxelObj(VoxelModelName name, bool addToRender, CenterAdjType centerType, Vector3 centerAdjust)
        //{
        //    if (name == VoxelModelName.NUM_NON)
        //    {
        //        throw new Exception();
        //    }
        //    //skapa ett voxelObj som kan hantera frames
        //    VoxelObjGridData data = LoadVoxelObjGrid(name)[0];

        //    switch (centerType)
        //    {
        //        case CenterAdjType.BottomCenter:
        //            centerAdjust += data.BottomCenterAdj();
        //            break;
        //        case CenterAdjType.CenterAll:
        //            centerAdjust += data.CenterAdj();
        //            break;
        //    }

        //    Graphics.VoxelObj result =
        //        VoxelObjBuilder.BuildFromVoxelGrid2(data, centerAdjust);
        //    if (addToRender)
        //    {
        //        Ref.draw.AddToRenderList(result);
        //    }
        //    result.SetOneScale(data.Limits);
        //    return result;
        //}

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

        public static VoxelObjListDataHD VoxelObjListData(VoxelModelName name)
        {
            if (name == VoxelModelName.NUM_NON)
            {
                throw new Exception();
            }
            return new VoxelObjListDataHD(Editor.VoxelObjDataLoader.LoadVoxelObjGrid(name)[0].GetVoxelArray());

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
        public static DataStream.FilePath ContentPath(VoxelModelName name)
        {
            if (name == VoxelModelName.NUM_NON)
            {
                throw new Exception();
            }

            string fileName = name.ToString();
            DataStream.FilePath userMadePath = new DataStream.FilePath(LfLib.OverrideModelsFolder, fileName,
                Voxels.VoxelLib.VoxelObjByteArrayEnding, true, false);
            if (userMadePath.Exists())
            {
                return userMadePath;
            }

            string folder;

            if (name > VoxelModelName.CATEGORY_WARS_6)
            {
                folder = LfLib.ModelsCategoryWars;
            }
            else if (name > VoxelModelName.CATEGORY_OTHER_5)
            {
                folder = LfLib.ModelsCategoryOther;
            }
            else if (name > VoxelModelName.CATEGORY_BLOCKPATTERN_4)
            {
                folder = LfLib.ModelsCategoryBlockpattern;
            }
            else if (name > VoxelModelName.CATEGORY_TERRAIN_3)
            {
                folder = LfLib.ModelsCategoryTerrain;
            }
            else if (name > VoxelModelName.CATEGORY_APPEARANCE_2)
            {
                folder = LfLib.ModelsCategoryAppearance;
            }
            else if (name > VoxelModelName.CATEGORY_WEAPON_1)
            {
                folder = LfLib.ModelsCategoryWeapon;
            }
            else
            {
                folder = LfLib.ModelsCategoryCharacter;
            }
            return new DataStream.FilePath(folder, fileName, 
                Voxels.VoxelLib.VoxelObjByteArrayEnding, false);
        }

        public static List<VoxelObjGridData> LoadObj(VoxelModelName name, List<byte> find, List<byte> replace)
        {
            if (name == VoxelModelName.NUM_NON)
            {
                throw new Exception();
            }

            VoxelObjGridDataAnimWithColReplace result = new VoxelObjGridDataAnimWithColReplace();
            result.Save(false, ContentPath(name), false, find, replace);
            return result.Frames;
        }

        /// <summary>
        /// Load a player custom made animated obj 
        /// </summary>
        public static List<VoxelObjGridData> LoadVoxelObjGrid(string name)
        {
            VoxelObjGridDataAnim result = new VoxelObjGridDataAnim();
            result.Save(false, Editor.DesignerStorage.CustomVoxelObjPath(name));

            return result.Frames;
        }
        public static List<VoxelObjGridData> LoadVoxelObjGrid(System.IO.BinaryReader r)
        {
            VoxelObjGridDataAnim result = new VoxelObjGridDataAnim();
            result.ReadBinaryStream(r);//Path(name), false);
            return result.Frames;
        }

        public static List<VoxelObjGridDataHD> LoadVoxelObjGridHD(System.IO.BinaryReader r)
        {
            VoxelObjGridDataAnimHD result = new VoxelObjGridDataAnimHD();
            result.ReadBinaryStream(r);//Path(name), false);
            return result.Frames;
        }

        public static Graphics.VoxelModel ReadVoxelObjAnimHD(System.IO.BinaryReader r, Vector3 centerAdjust, bool centerY)
        {
            List<VoxelObjGridDataHD> loadedFrames = LoadVoxelObjGridHD(r);
            if (loadedFrames == null) return null; //Err corrupt file

            if (centerY)
                centerAdjust += loadedFrames[0].CenterAdj();
            else
                centerAdjust += loadedFrames[0].BottomCenterAdj();


            Graphics.VoxelModel result = VoxelObjBuilder.BuildModelHD(
                loadedFrames, centerAdjust);

            //result.SetOneScale(loadedFrames[0].Size);
            return result;
        }


        public static Graphics.VoxelModel GetVoxelObjMaster(VoxelModelName name, Vector3 centerAdjust)
        {
            if (name == VoxelModelName.NUM_NON)
            {
                throw new Exception();
            }

            Debug.CrashIfMainThread();
            DataStream.FilePath path = ContentPath(name);
            System.IO.BinaryReader r = DataStream.DataStreamHandler.ReadBinaryIO(path);
            return GetVoxelObjMaster(r, centerAdjust);
        }

        public static Graphics.VoxelModel GetVoxelObjMaster(System.IO.BinaryReader r, Vector3 centerAdjust)
        {
            //skapa ett voxelObj som kan hantera frames
            List<VoxelObjGridDataHD> loadedFrames = LoadVoxelObjGridHD(r);
           
            centerAdjust += loadedFrames[0].BottomCenterAdj();
            Graphics.VoxelModel result;
            //if (loadedFrames.Count == 1)
            //{
            //    result = VoxelObjBuilder.BuildFromVoxelGrid2(loadedFrames[0], centerAdjust);//BuildFromVoxels(loadedFrames[0].Limits, voxels[0], centerAdjust);
            //}
            //else
            //{
                result = VoxelObjBuilder.BuildModelHD(loadedFrames, centerAdjust);//BuildAnimatedFromVoxels(new RangeIntV3(IntVector3.Zero, loadedFrames[0].Limits), voxels, centerAdjust);
            //}
            result.SetOneScale(loadedFrames[0].Size);
            return result;
        }


       
        public static List<VoxelObjGridDataHD> LoadVoxelObjGrid(VoxelModelName name)
        {
            if (name == VoxelModelName.NUM_NON)
            {
                throw new Exception();
            }

            VoxelObjGridDataAnimHD result = new VoxelObjGridDataAnimHD();
            result.Save(false, ContentPath(name));
            return result.Frames;
        }
        public static List<VoxelObjGridDataHD> LoadVoxelObjGridHD(VoxelModelName name)
        {
            if (name == VoxelModelName.NUM_NON)
            {
                throw new Exception();
            }

            VoxelObjGridDataAnimHD result = new VoxelObjGridDataAnimHD();
            result.Save(false, ContentPath(name));
            return result.Frames;
        }
    }
    enum SaveVersion
    {
        Ver1,
        Ver2,
    }
   
    enum CenterAdjType
    {
        None,
        BottomCenter,
        CenterAll,
    }
}
