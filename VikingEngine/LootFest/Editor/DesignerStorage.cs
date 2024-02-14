using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Voxels;
using VikingEngine.DataStream;

namespace VikingEngine.LootFest.Editor
{
    class DesignerStorage
    {
        public const string UserVoxelObjFolder = "VoxelObjSave";
        public static bool[] HasChatergory;

        public static void InitFolderStructure()
        {
            HasChatergory = new bool[(int)SaveCategory.NUM];

            for (int i = 0; i < HasChatergory.Length; ++i)
            {
                var path = TemplatePath(i, null);
                if (DataLib.SaveLoad.FolderExistAndHaveFilesInit(path.LocalDirectoryPath))
                {
                    HasChatergory[i] = true;
                }
                else
                {
                    DataStream.FilePath.CreateStorageFolder(Editor.DesignerStorage.TemplateFolder(i));
                }
            }
        }

        static string randomName()
        {
            return "VX" + Ref.rnd.Int(9999).ToString();
        }
        public static DataStream.FilePath CustomVoxelObjPath(string name)
        {
            return new DataStream.FilePath(UserVoxelObjFolder, name, Voxels.VoxelLib.VoxelObjByteArrayEnding);
        }

        public string saveFileName = randomName();
        VoxelDesigner designer;
        
        public DesignerStorage(VoxelDesigner designer)
        {
            this.designer = designer;
        }

        public void loadRetailModel(VoxelModelName modelName)
        {
            var model = VoxelObjDataLoader.LoadVoxelObjGrid(modelName);
            modelLoaded(new VoxelObjGridDataAnimHD(model));
           
           saveFileName = modelName.ToString();
           
            Debug.Log("Loading vox model: " + modelName.ToString());
        }

        void modelLoaded(VoxelObjGridDataAnimHD model)
        {
            if (designer.inGame)
            {
                designer.voxelGridToSelection(model.Frames[0]);
            }
            else
            {
                designer.addLoadedModel(model, designer.combineLoading);
                //updateVoxelObj();
            }
        }


        public void loadUserModel(string name)
        {
            saveFileName = name;
            new LoadCreatorImage(CustomVoxelObjPath(saveFileName), modelLoaded);
        }

        public void save()
        {
            designer.print("Saving...");

            new DataStream.WriteBinaryIO(new DataStream.FilePath(UserVoxelObjFolder, saveFileName, Voxels.VoxelLib.VoxelObjByteArrayEnding, true, false),
                designer.animationFrames.WriteBinaryStream, null);

        }

        int backupId = 0;
        public void saveBackUp()
        {
            new DataStream.WriteBinaryIO(new DataStream.FilePath(UserVoxelObjFolder, "backup_save" + backupId.ToString(), 
                Voxels.VoxelLib.VoxelObjByteArrayEnding, true, false),
                designer.animationFrames.WriteBinaryStream, null);

            backupId++;
            if (backupId >= 10)
            {
                backupId = 0;
            }

        }

        public static FilePath TemplatePath(int category, string name)
        {
            return new FilePath(TemplateFolder(category), name, VoxelLib.VoxelObjByteArrayEnding, true, false);
        }

        public static string TemplateFolder(int category)
        {
            const string TemplateCategoryFolder = "Template";

            return TemplateCategoryFolder + category.ToString();
        }

        public void beginStoreSelectionAsTemplate(int category)
        {
            HasChatergory[category] = true;

            FilePath path = TemplatePath(category, DateTime.Now.Ticks.ToString());

            //DataStream.FilePath.CreateStorageFolder(path.DirectoryPath);

            DataStream.BeginReadWrite.BinaryIO(true, path, writeSelection, null, null);
            //new Timer.Asynch2ArgTrigger<VoxelObjGridDataHD, int>(storeSelectionAsTemplateAsynch, designer.SelectionToGrid(), category, true);
        }

        public void beginLoadTemplate(FilePath path)
        {
            DataStream.BeginReadWrite.BinaryIO(false, path, null, readSelection, null);
        }
        

        void writeSelection(System.IO.BinaryWriter w)
        {
            var grid = designer.SelectionToGrid();

            Voxels.VoxelLib.WriteVoxelObjAnimHD(w, grid.MaterialGrid);
        }

        void readSelection(System.IO.BinaryReader r)
        {
            var model = Voxels.VoxelLib.ReadVoxelObjectAnimHD(r);

            modelLoaded(new VoxelObjGridDataAnimHD(model));
        }

        public void clearName()
        {
            saveFileName = randomName();
        }
        
    }
}
