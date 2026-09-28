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
        protected object WorkerStatusLock = new object();
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
                for (int i = workerUnits.Count -1; i>=0;--i)
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

        static HashSet<int> ExistingWorkers = new HashSet<int>(1024);

        void addMissingWorkerUnits()
        {
            ExistingWorkers.Clear();

            if (pfaction.TryGetFaction(out _))
            {

                foreach (var unit in workerUnits)
                {
                    ExistingWorkers.Add(unit.myIndex);
                }

                lock (WorkerStatusLock)
                {
                    for (int i = 0; i < workerStatuses.Count; i++)
                    {
                        if (workerStatuses.Array[i].work != WorkType.IsDeleted &&
                            !ExistingWorkers.Contains(i))
                        {
                            workerUnits.Add(new WorkerUnit(this, workerStatuses.Array[i], i));
                        }
                    }
                }
            }
        }

        public void setTimeOnAllWorkers()
        {
            for (int i = 0; i < workerStatuses.Count; ++i)
            {
                ref var status = ref workerStatuses.Array[i];
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

        public WorkerStatus getWorkerStatus(int index)
        {
            //lock (workerStatuses.array)
            //{
                return workerStatuses.Array[index];
            //}
        }

        public ref WorkerStatus getRefWorkerStatus(int index)
        {
            return ref workerStatuses.Array[index];
        }

        public void setWorkerStatus(int index, ref WorkerStatus status)
        {
            lock (WorkerStatusLock)
            {
                workerStatuses.Array[index] = status;
            }
        }
    }
}
