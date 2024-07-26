using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Worker;

namespace VikingEngine.DSSWars.GameObject
{
    partial class AbsMapObject
    {
        protected List<WorkerStatus> workerStatuses = new List<WorkerStatus>();
        public List<WorkerUnit> workerUnits = null;

        protected void updateWorkerUnits()
        {
            if (workerUnits != null)
            {
                if (workerUnits.Count < workerStatuses.Count)
                {
                    for (int i = workerUnits.Count; i < workerStatuses.Count; i++)
                    {
                        workerUnits.Add(new WorkerUnit(this, workerStatuses[i], i));
                    }
                }

                foreach (var w in workerUnits)
                {
                    w.update();
                }
            }
        }

        protected void setWorkersInRenderState()
        {
            if (inRender_detailLayer)
            {
                if (workerUnits == null)
                {
                    workerUnits = new List<WorkerUnit>(workerStatuses.Count);
                }
            }
            else
            {
                if (workerUnits != null)
                {
                    foreach (var w in workerUnits)
                    {
                        w.DeleteMe();
                    }

                    workerUnits = null;
                }
            }
        }

        public void getWorkerStatus(int index, ref WorkerStatus status)
        {
            status = workerStatuses[index];
        }

        public void setWorkerStatus(int index, ref WorkerStatus status)
        {
            workerStatuses[index] = status;
        }
    }
}
