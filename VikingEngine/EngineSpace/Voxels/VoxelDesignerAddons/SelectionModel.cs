using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.Voxels
{
    class SelectionModel
    { 
        Time selectionFlashTimer = new Time(500);
        Graphics.VoxelModel model = null, newModel = null;

        bool needsRefresh = false;
        bool isBuildingLock = false;
        bool buildingComplete = false;

        VoxelObjGridDataHD data = null, buildingData = null;
        Vector3 offset, buildingOffset;

        public void refresh(VoxelObjListDataHD voxels, IntVector3 size, Vector3 offset)
        {
            if (voxels.Voxels.Count == 0)
            {
                clear();
            }
            else
            {
                needsRefresh = true;

                this.offset = offset;


                IntervalIntV3 volume = voxels.getMinMax();
                data = new VoxelObjGridDataHD(volume.Size, voxels.Voxels, -volume.Min);
                this.offset += volume.Min.Vec;

                checkNeedRefresh();
            }
        }

        public void move(IntVector3 moveLength)
        {
            if (model != null)
            {
                model.position += moveLength.Vec;
            }
        }

        public void clear()
        {
            needsRefresh = true;
            data = null;
            if (model != null)
            { model.Visible = false; }
            //checkNeedRefresh();
        }

        public void update()
        {
            if (model != null)
            {
                if (selectionFlashTimer.CountDown())
                {
                    model.Visible = !model.Visible;
                    selectionFlashTimer.MilliSeconds = 300;
                }
            }

            checkNeedRefresh();

            if (buildingComplete)
            {
                buildingComplete = false;

                if (model != null)
                { model.DeleteMe(); }

                selectionFlashTimer.Seconds = 0.8f;
                model = newModel;
                newModel = null;
                Ref.draw.AddToRenderList(model);
            }
        }

        void checkNeedRefresh()
        {
            if (needsRefresh && !isBuildingLock)
            {
                if (model != null)
                {
                    model.Visible = false;
                }

                if (data == null)
                {
                    if (model != null)
                    { model.DeleteMe(); }

                    needsRefresh = false;
                }
                else
                {
                    buildingData = data;
                    buildingOffset = offset;

                    needsRefresh = false;
                    isBuildingLock = true;
                    new Timer.AsynchActionTrigger(build_Asynch);
                }
            }
        }

        void build_Asynch()
        {
            Vector3 scaleAdd = new Vector3(AbsVoxelDesigner.BlockScale * 0.02f);
            newModel = LootFest.Editor.VoxelObjBuilder.BuildModelHD(
                new List<VoxelObjGridDataHD> { buildingData }, -scaleAdd * PublicConstants.Half);

            newModel.position = buildingOffset;
            newModel.AlwaysInCameraCulling = true;

            buildingComplete = true;
            isBuildingLock = false;
        }

        public void DeleteMe()
        {
            if (model != null)
                model.DeleteMe();
        }
    }
}
