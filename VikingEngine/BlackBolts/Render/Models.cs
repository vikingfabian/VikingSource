using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars;
using VikingEngine.Graphics;
using VikingEngine.LootFest;
using VikingEngine.Voxels;

namespace VikingEngine.Core.BlackBolts.Render
{
    class Models
    {
        public Dictionary<VoxelModelName, VoxelModel> voxelModels = new Dictionary<VoxelModelName, VoxelModel>();
        public Dictionary<VoxelModelName, VoxelObjGridDataAnimHD> rawModels = new Dictionary<VoxelModelName, VoxelObjGridDataAnimHD>();

        public Models()
        {
            BlackRef.models = this;
            load();
        }

        public void load()
        {
            loadVoxelModel(VoxelModelName.ErrorCube, false);
            loadVoxelModel(VoxelModelName.goblin_worker, false);
            loadVoxelModel(VoxelModelName.bb_belt, false);
            loadVoxelModel(VoxelModelName.bb_rotate, false);
            loadVoxelModel(VoxelModelName.bb_onetile, false);
            loadVoxelModel(VoxelModelName.bb_item, false);
            loadVoxelModel(VoxelModelName.Hen, false);
            loadVoxelModel(VoxelModelName.bb_nightdemon, false);
            loadVoxelModel(VoxelModelName.bb_dragon, false);
            loadVoxelModel(VoxelModelName.bb_whiteknight, false);
            loadVoxelModel(VoxelModelName.bb_blackknight, false);

            loadRawModel(VoxelModelName.bb_io_base);
            loadRawModel(VoxelModelName.bb_io_mark);


            void loadVoxelModel(VoxelModelName modelName, bool centerY)
            {
                float yAdjust = 0;

                DataStream.FilePath path = VoxelObjDataLoader.ContentPath(modelName);
                path.UseTimeMark = false;
                byte[] data = DataStream.FileToDiskManager.Read(path);

                System.IO.MemoryStream s = new System.IO.MemoryStream(data);
                System.IO.BinaryReader r = new System.IO.BinaryReader(s);

                Vector3 centerAdjust = new Vector3(0, yAdjust, 0);

                List<VoxelObjGridDataHD> loadedFrames = VoxelObjDataLoader.LoadVoxelObjGridHD(r);

                if (centerY)
                {
                    centerAdjust += loadedFrames[0].CenterAdj();
                }
                else
                {
                    centerAdjust += loadedFrames[0].BottomCenterAdj();
                }

                IntVector3 gridSz = loadedFrames[0].Size;

                List<Frame> framesData;
                IVerticeData verticeData = VoxelObjBuilder.BuildVerticesHD(loadedFrames, centerAdjust, out framesData);

                //loadedData.Add(new VoxelModelData(modelName, verticeData, gridSz, framesData));

                voxelModels.Add(modelName, new VoxelModelData(modelName, verticeData, gridSz, framesData).sychedProcessing());
            }

            void loadRawModel(VoxelModelName modelName)
            {
               
                    DataStream.FilePath path = VoxelObjDataLoader.ContentPath(modelName);
                    byte[] data = DataStream.FileToDiskManager.Read(path);
                    //Task.Run(() =>
                    //{
                    //    try
                    //    {
                            System.IO.MemoryStream s = new System.IO.MemoryStream(data);
                            System.IO.BinaryReader r = new System.IO.BinaryReader(s);

                            var grids = VoxelObjDataLoader.LoadVoxelObjGridHD(r);
                            var result = new VoxelObjGridDataAnimHD(grids);

                            rawModels.Add(modelName, result);
                        //}
                        //catch (Exception ex)
                        //{
                        //    DebugExtensions.BlueScreen.ThreadException = ex;
                        //}
                    //});
                

            }
        }
    }
}
