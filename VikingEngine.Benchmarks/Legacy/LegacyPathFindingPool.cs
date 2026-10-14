using System;
using System.Collections.Concurrent;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Path;

namespace VikingEngine.Benchmarks.Legacy
{
    /// <summary>
    /// Snapshot of the legacy unbounded PathFindingPool before Phase 2 reclamation and preallocation.
    /// </summary>
    class LegacyUnboundedPathFindingPool
    {
        ConcurrentStack<PathFinding> poolPf = new ConcurrentStack<PathFinding>();
        ConcurrentQueue<WalkingPath> poolRes = new ConcurrentQueue<WalkingPath>();
        
        public PathFinding GetPf()
        {
            if (poolPf.TryPop(out PathFinding? path) && path != null)
            {
                return path;
            }
            else
            {
                return new PathFinding();
            }
        }

        public WalkingPath GetRes()
        {
            if (poolRes.TryDequeue(out WalkingPath? path) && path != null)
            {
                path.recycle();
                return path;
            }
            else
            {
                return new WalkingPath();
            }
        }

        public void Return(PathFinding path)
        {
            if (path != null)
            {
                path.recycle();
                poolPf.Push(path);
            }
        }

        public void Return(WalkingPath pathresult)
        {
            if (pathresult != null)
            {
                poolRes.Enqueue(pathresult);
            }
        }
    }
}
