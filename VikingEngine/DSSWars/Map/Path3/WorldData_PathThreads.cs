using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.DSSWars.Map.Path3;

namespace VikingEngine.DSSWars
{
    partial class WorldData
    {
        //public PathFindingPool pathFindingPool = new PathFindingPool();
        //public DetailPathFindingPool detailPathFindingPool = new DetailPathFindingPool();
        public Path3Thread path3Thread_test;

        Path3Thread[] pathThreads;
        Path3Thread_Player pathThreadPlayer;

        ConcurrentStack<LayerPathFinding> poolPathLayer2 = new ConcurrentStack<LayerPathFinding>();
        ConcurrentStack<LayerPathFinding> poolPathLayer4 = new ConcurrentStack<LayerPathFinding>();
        ConcurrentQueue<LayerWalkingPathResult> poolLayerResult = new ConcurrentQueue<LayerWalkingPathResult>();
        ConcurrentStack<DetailPathFinding> poolPathDetail = new ConcurrentStack<DetailPathFinding>();
        ConcurrentQueue<DetailWalkingPathResult> poolDetailResult = new ConcurrentQueue<DetailWalkingPathResult>();

        public void initPaths(int count)
        {
            pathThreads = new Path3Thread[count];

            int startIx = 0;
            int factionLength = DssRef.world.factions.Count / count;
            for (int i = 0; i < count; i++)
            {
                int end;
                if (i == count - 1)
                {
                    //last
                    end = 1_000_000;
                }
                else
                {
                    end = startIx + factionLength;
                }
                pathThreads[i] = new Path3Thread();
                pathThreads[i].startProcess(i, startIx, end);
                startIx = end + 1;
            }
            pathThreadPlayer = new Path3Thread_Player();
            pathThreadPlayer.startProcess(-1, -1, -1, System.Threading.ThreadPriority.Normal);
            
        }

        public void ClearPools()
        {
            //pathFindingPool.Clear();
            //detailPathFindingPool.Clear();
            poolDetailResult.Clear();
        }

        public void PreallocatePools(int pfCount = 1, int detailPfCount = 1)
        {
            //pathFindingPool.Preallocate(pfCount);
            //detailPathFindingPool.Preallocate(detailPfCount);
        }

        public LayerWalkingPathResult GetPath3FromPool()
        {
            if (poolLayerResult.TryDequeue(out LayerWalkingPathResult path))
            {
                path.recycle();
                return path;
            }

            return new LayerWalkingPathResult();

        }

        public void ReturnToPool(LayerPathFinding path)
        {
            // Reset the node to a default state
            if (path != null)
            {
                path.recycle();
                if (path.layer == 2)
                {
                    poolPathLayer2.Push(path);
                }
                else
                {
                    poolPathLayer4.Push(path);
                }
            }
        }

        public void ReturnToPool(LayerWalkingPathResult pathresult)
        {
            // Reset the node to a default state
            if (pathresult != null)
            {
                //path.recycle();
                pathresult.timeStamp = Ref.TotalFrameCount;
                poolLayerResult.Enqueue(pathresult);
            }
        }

        public DetailPathFinding GetDetailPathFromPool()
        {
            if (poolPathDetail.TryPop(out DetailPathFinding path))
            {
                return path;
            }
            else
            {
                return new DetailPathFinding();
            }
        }
        public void ReturnToPool(DetailPathFinding path)
        {
            // Reset the node to a default state
            if (path != null)
            {
                path.recycle();
                poolPathDetail.Push(path);
            }
        }
        public DetailWalkingPathResult GetDetailResultFromPool()
        {
            if (poolDetailResult.TryDequeue(out DetailWalkingPathResult path))
            {
                path.recycle();
                return path;
            }
            else
            {
                return new DetailWalkingPathResult();
            }
        }
        public void ReturnToPool(DetailWalkingPathResult pathresult)
        {
            if (pathresult != null)
            {
                poolDetailResult.Enqueue(pathresult);
            }
        }

    }
}
