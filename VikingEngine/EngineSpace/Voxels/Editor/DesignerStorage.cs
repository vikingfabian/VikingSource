using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;
using VikingEngine.DSSWars;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest;
using VikingEngine.Voxels;

namespace VikingEngine.Voxels
{
    class DesignerStorage : IStreamIOCallback
    {
        public const string VoxelModelFolder = "Voxel Model";
        public const string VoxelProjectFolder = "Voxel Project";
        public static bool[] HasChatergory;

        //public static void InitFolderStructure()
        //{
        //    HasChatergory = new bool[(int)SaveCategory.NUM];

        //    for (int i = 0; i < HasChatergory.Length; ++i)
        //    {
        //        var path = TemplatePath(i, null);
        //        if (DataLib.SaveLoad.FolderExistAndHaveFilesInit(path.LocalDirectoryPath))
        //        {
        //            HasChatergory[i] = true;
        //        }
        //        else
        //        {
        //            FilePath.CreateStorageFolder(TemplateFolder(i));
        //        }
        //    }
        //}

        static string randomName()
        {
            return "VX" + Ref.rnd.Int(9999).ToString();
        }
        public static FilePath CustomVoxelObjPath(string name)
        {
            return new FilePath(VoxelModelFolder, name, Voxels.VoxelLib.VoxelObjByteArrayEnding);
        }

        public static FilePath VoxelProjectPath(string name)
        {
            return new FilePath(VoxelProjectFolder, name, Voxels.VoxelLib.VoxelProjectEnding);
        }

        public static FilePath InGameVoxelObjPath(string name)
        {
            return new FilePath(LfLib.ModelsCategoryWars, name, Voxels.VoxelLib.VoxelObjByteArrayEnding, false);
        }

        public string saveFileName = randomName();
        AbsVoxelDesigner designer;
        
        public DesignerStorage(AbsVoxelDesigner designer)
        {
            this.designer = designer;
        }

        public void loadRetailModel(VoxelModelName modelName)
        {
            var model = VoxelObjDataLoader.LoadVoxelObjGrid(modelName);
            modelLoaded(new VoxelObjGridDataAnimHD(model));
           
           saveFileName = modelName.ToString();
           
            //Debug.Log("Loading vox model: " + modelName.ToString());
        }

        public void loadRetailModel(string modelName)
        {
            var model = VoxelObjDataLoader.LoadVoxelObjGrid(modelName);
            modelLoaded(new VoxelObjGridDataAnimHD(model));

            saveFileName = modelName;

            //Debug.Log("Loading vox model: " + modelName.ToString());
        }

        void modelLoaded(VoxelObjGridDataAnimHD model)
        {            
            designer.addLoadedModel(model);            
        }

        void projectLoaded(VoxelProject project)
        {
            designer.addLoadedProject(project);
        }

        public void loadUserModel(string name)
        {
            saveFileName = name;
            new LoadCreatorImage(CustomVoxelObjPath(saveFileName), modelLoaded);
        }

        public void loadProject(string name)
        {
            saveFileName = name;
            new LoadVoxelProject(VoxelProjectPath(saveFileName), projectLoaded);
        }

        public FilePath VoxSavePath()
        {
            return new FilePath(VoxelModelFolder, saveFileName, Voxels.VoxelLib.VoxelObjByteArrayEnding, true, false);
        }

        //public async Task save()
        //public void save()
        //{
        //    //designer.print("Saving...");

        //    FilePath voxpath = SavePath();
        //    var projectPath = voxpath;
        //    projectPath.FileEnd = VoxelLib.VoxelProjectArrayEnding;

        //    VoxelObjGridDataAnimHD allMergedData = null;
        //    var mergeTask = Task.Run(() =>
        //    {
        //        allMergedData = designer.voxelProject.refreshMerged(true);
        //    });

        //    new WriteBinaryIO(projectPath,
        //        designer.voxelProject.write, this);

        //    if (designer.voxelProject.currentFrame.Length > 1)
        //    {
        //       var layers = designer.voxelProject.LayersCopy();
        //        for (int i = 0; i < layers.Count; ++i)
        //        {
        //            var layerPath = voxpath;
        //            layerPath.FileName += "_" + layers[i].Name(i);
        //            new WriteBinaryIO(layerPath,
        //               layers[i].animationFrames.WriteBinaryStream, this);
        //        }
        //    }

        //    await mergeTask; //how do my main thread waih for this task?

        //    new WriteBinaryIO(voxpath,
        //        allMergedData.WriteBinaryStream, this);

        //}
        public void save()
        {
            FilePath voxpath = VoxSavePath();
            var projectPath = voxpath;

            projectPath.LocalDirectoryPath = VoxelProjectFolder;
            projectPath.FileEnd = VoxelLib.VoxelProjectEnding;

            // Start the merge task
            var mergeTask = Task.Run(() =>
            {
                return designer.voxelProject.refreshMerged(true);
            });

            // Write project data
            new WriteBinaryIO(projectPath,
                designer.voxelProject.write, null);

            // Save each layer
           
            var layers = designer.voxelProject.LayersCopy();
            if (layers.Count > 1)
            {
                for (int i = 0; i < layers.Count; ++i)
                {
                    var layerPath = voxpath;
                    layerPath.FileName += "_" + layers[i].Name(i);
                    new WriteBinaryIO(layerPath,
                        layers[i].animationFrames.WriteBinaryStream, null);
                }
            }

            // Wait for merge task to complete (synchronously)
            VoxelObjGridDataAnimHD allMergedData = mergeTask.GetAwaiter().GetResult();

            // Save merged data
            new WriteBinaryIO(voxpath,
                allMergedData.WriteBinaryStream, this);

            FilePath iconPath = projectPath;
            iconPath.FileEnd = ".png";


            const int Size = 64;

            try
            {
                using (FileStream stream = new FileStream(iconPath.CompleteLocalPath(true), FileMode.Create))
                {
                    renderModel().SaveAsPng(stream, Size, Size);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw;
#endif
            }

            RenderTarget2D renderModel()
            {
                RenderTargetImage target = new RenderTargetImage(Vector2.Zero, new Vector2(Size), ImageLayers.Foreground4, false);
                TopViewCamera modelView = new TopViewCamera(22, new Vector2(MathHelper.PiOver2 - 0.8f, MathHelper.PiOver4 + 0.12f),
                        Size, Size);

                Vector3 modelGridSz = designer.voxelProject.drawLimits.Size.Vec;
                modelView.LookTarget = modelGridSz * 0.5f;
                modelView.Time_Update(0);
                modelView.RecalculateMatrices();

                target.Camera = modelView;
                target.DrawImagesToTarget(null, new List<AbsDraw> { designer.voxelObj }, true, 0);
                return target.renderTarget;
            }
        }

        public void saveCurrentFrame(int frame, string name)
        {
            FilePath voxpath = VoxSavePath();
            voxpath.FileName = name;
            //var projectPath = voxpath;

            //projectPath.LocalDirectoryPath = VoxelProjectFolder;
            //projectPath.FileEnd = VoxelLib.VoxelProjectEnding;

            // Start the merge task
            var mergeTask = Task.Run(() =>
            {
                return designer.voxelProject.refreshMerged(true);
            });

            //// Write project data
            //new WriteBinaryIO(projectPath,
            //    designer.voxelProject.write, null);

            //// Save each layer

            //var layers = designer.voxelProject.LayersCopy();
            //if (layers.Count > 1)
            //{
            //    for (int i = 0; i < layers.Count; ++i)
            //    {
            //        var layerPath = voxpath;
            //        layerPath.FileName += "_" + layers[i].Name(i);
            //        new WriteBinaryIO(layerPath,
            //            layers[i].animationFrames.WriteBinaryStream, null);
            //    }
            //}

            // Wait for merge task to complete (synchronously)
            VoxelObjGridDataAnimHD allMergedData = mergeTask.GetAwaiter().GetResult();
            allMergedData.Frames = new List<VoxelObjGridDataHD> { allMergedData.Frames[frame] };

            // Save merged data
            new WriteBinaryIO(voxpath,
                allMergedData.WriteBinaryStream, this);

            //FilePath iconPath = projectPath;
            //iconPath.FileEnd = ".png";


           
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            if (save && completed)
            {
                RichBoxContent content = new RichBoxContent();
                content.h2(LoadContent.CheckCharsSafety(saveFileName, LoadedFont.Regular), HudLib.TitleColor_Name);

                content.newLine();
                content.Add(new RbImage(SpriteName.WarsHudIconSave));
                content.space();
                content.Add(new RbText(DssRef.lang.Hud_SaveCompleted));

                designer.print(content);
            }
        }

        int backupId = 0;
        public void saveBackUp()
        {
            new WriteBinaryIO(new FilePath(VoxelModelFolder, "backup_save" + backupId.ToString(), 
                Voxels.VoxelLib.VoxelObjByteArrayEnding, true, false),
                designer.voxelProject.AnimationFrames.WriteBinaryStream, null);

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

            BeginReadWrite.BinaryIO(true, path, writeSelection, null, null);
            //new Timer.Asynch2ArgTrigger<VoxelObjGridDataHD, int>(storeSelectionAsTemplateAsynch, designer.SelectionToGrid(), category, true);
        }

        public void beginLoadTemplate(FilePath path)
        {
            BeginReadWrite.BinaryIO(false, path, null, readSelection, null);
        }
        

        void writeSelection(System.IO.BinaryWriter w)
        {
            var grid = designer.SelectionToGrid();

            Voxels.VoxelLib.WriteVoxelObjAnimHD(w, grid);
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
