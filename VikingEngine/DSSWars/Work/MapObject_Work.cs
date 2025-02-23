﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Work;

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
                    addMissingWorkerUnits();
                }

                var city = GetCity();
                foreach (var w in workerUnits)
                {
                    w.update(city);
                }
            }
        }

        void addMissingWorkerUnits()
        {
            for (int i = workerUnits.Count; i < workerStatuses.Count; i++)
            {
                if (workerStatuses[i].work != WorkType.IsDeleted)
                {
                    workerUnits.Add(new WorkerUnit(this, workerStatuses[i], i));
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
            status = workerStatuses[index];
        }

        public void setWorkerStatus(int index, ref WorkerStatus status)
        {
            workerStatuses[index] = status;
        }
    }
}
