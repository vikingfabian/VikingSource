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

        public ThreadedTemplateStamp(AbsVoxelDesigner designer, VoxelObjListDataHD selection)
            : base( QuedTasksType.QueAndSynch)
        {
            this.designer = designer;
            this.selection = selection;
            beginAutoTasksRun();
        }
        protected override void runQuedAsynchTask()
        {
            //base.runQuedAsynchTask();
            updateArea = selection.getMinMax();
            designer.MakeThreadedStamp(selection, updateArea);
            //return true;
        }
        protected override void runQuedMainTask()
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
