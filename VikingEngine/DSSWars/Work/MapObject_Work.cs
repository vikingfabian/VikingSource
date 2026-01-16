using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Work;
using VikingEngine.EngineSpace;

namespace VikingEngine.DSSWars.GameObject
{
    partial class AbsArmy
    {
        protected StructList<WorkerStatus> workerStatuses = new StructList<WorkerStatus>(16);
        public List<WorkerUnit> workerUnits = null;

        protected void updateWorkerUnits()
        {
            if (workerUnits != null)
            {
                if (workerUnits.Count < workerStatuses.Count)
                {
                    addMissingWorkerUnits();
                }

                var city = GetCity();
                for (int i = workerUnits.Count -1; i>=0;--i)//each (var w in workerUnits)
                {
                    if (workerUnits[i].update(city))
                    { 
                        workerUnits.RemoveAt(i);
                    }
                }
            }
        }

        //public void setTimeOnAllWorkers()
        //{
        //    for (int i = 0; i < workerStatuses.Count; ++i)
        //    {
        //        var status = workerStatuses[i];
        //        status.processTimeStartStampSec = Ref.TotalGameTimeSec;

        //        workerStatuses[i] = status;
        //    }
        //}

        void addMissingWorkerUnits()
        {
            lock (workerStatuses.array)
            {
                for (int i = workerUnits.Count; i < workerStatuses.Count; i++)
                {
                    if (workerStatuses[i].work != WorkType.IsDeleted)
                    {
                        workerUnits.Add(new WorkerUnit(this, workerStatuses[i], i));
                    }
                }
            }
        }

        public void setTimeOnAllWorkers()
        {
            for (int i = 0; i < workerStatuses.Count; ++i)
            {
                ref var status = ref workerStatuses.array[i];
                status.processTimeStartStampSec = Ref.TotalGameTimeSec;
            }
        }

        protected void setWorkersInRenderState()
        {
            if (inRender_detailLayer)
            {
                if (workerUnits == null)
                {
                    workerUnits = new List<WorkerUnit>(workerStatuses.Count);
                    addMissingWorkerUnits();
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
            lock (workerStatuses.array)
            {
                status = workerStatuses[index];
            }
        }

        public void setWorkerStatus(int index, ref WorkerStatus status)
        {
            lock (workerStatuses.array)
            {
                workerStatuses[index] = status;
            }
        }
    }
}
