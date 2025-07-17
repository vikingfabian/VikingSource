using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Voxels
{
    class ThreadedTemplateStamp : AbsQuedTasks//QueAndSynch
    {
        AbsVoxelDesigner designer;
        VoxelObjListDataHD selection;
        IntervalIntV3 updateArea;
        int frame;
        public ThreadedTemplateStamp(AbsVoxelDesigner designer, VoxelObjListDataHD selection, int frame)
            : base( QuedTasksType.QueAndSynch)
        {
            this.frame = frame;
            this.designer = designer;
            this.selection = selection;
            beginAutoTasksRun();
        }
        protected override void runQuedAsynchTask()
        {
            //base.runQuedAsynchTask();
            updateArea = selection.getMinMax();
            designer.MakeThreadedStamp(selection, updateArea, frame);
            //return true;
        }
        public override void runSyncAction()
        {
        //    base.runQuedMainTask();
        //}
        //{
            designer.UpdateImageAfterThread(updateArea);
        }
    }

    enum ThreadedActionType
    {
        DottedLine,
        Rectangle,

    }
}
