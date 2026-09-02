using System;
using System.Collections.Generic;
using VikingEngine.Graphics;

namespace VikingEngine.Tests.Legacy
{
    class LegacyDrawBatchCollection
    {
        public Queue<AbsVoxelModelInstance> loadingQueue = new Queue<AbsVoxelModelInstance>();
        public Dictionary<int, LegacyDrawBatch> batches = new Dictionary<int, LegacyDrawBatch>(128);

        public void Add(int masterId, AbsDraw model)
        {
            if (batches.TryGetValue(masterId, out var batch))
            {
                batch.Add(model);
            }
            else
            {
                batches.Add(masterId, new LegacyDrawBatch(model));
            }
        }

        public int Count
        {
            get
            {
                int total = 0;
                foreach (var b in batches.Values)
                {
                    total += b.Count;
                }
                return total;
            }
        }
    }

    class LegacyDrawBatch : List<AbsDraw>
    {
        public LegacyDrawBatch(AbsDraw model) : base(16)
        {
            Add(model);
        }
    }
}
