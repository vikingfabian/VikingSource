using System;
using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using VikingEngine;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Path;

namespace VikingEngine.Benchmarks.Pathfinding
{
    class LegacyWalkingPath
    {
        public double timeStamp;
        public List<PathNodeResult> nodes = new List<PathNodeResult>(64);
        public void recycle()
        {
            nodes.Clear();
        }
    }

    class LegacyPathFindingPool
    {
        ConcurrentQueue<LegacyWalkingPath> poolRes = new ConcurrentQueue<LegacyWalkingPath>();

        public LegacyWalkingPath GetRes()
        {
            if (poolRes.TryDequeue(out LegacyWalkingPath path))
            {
                if (path.timeStamp + 2 >= Ref.TotalFrameCount)
                {
                    poolRes.Enqueue(new LegacyWalkingPath());
                    poolRes.Enqueue(new LegacyWalkingPath());
                }
                path.recycle();
                return path;
            }
            else
            {
                return new LegacyWalkingPath();
            }
        }

        public void Return(LegacyWalkingPath pathresult)
        {
            if (pathresult != null)
            {
                pathresult.timeStamp = Ref.TotalFrameCount;
                poolRes.Enqueue(pathresult);
            }
        }
    }

    [InProcess]
    [MemoryDiagnoser]
    public class PoolBenchmarks
    {
        private PathFindingPool _cleanPool = null!;
        private WalkingPath _largePath = null!;
        private WalkingPath _normalPath = null!;

        [GlobalSetup]
        public void Setup()
        {
            _cleanPool = new PathFindingPool();
            _cleanPool.Return(new WalkingPath());

            _largePath = new WalkingPath();
            for (int i = 0; i < 600; i++)
            {
                _largePath.nodes.Add(new PathNodeResult(new IntVector2(i, i), false));
            }

            _normalPath = new WalkingPath();
            for (int i = 0; i < 100; i++)
            {
                _normalPath.nodes.Add(new PathNodeResult(new IntVector2(i, i), false));
            }
        }

        [Benchmark(Baseline = true)]
        public void Pool_Burst100_Legacy()
        {
            var pool = new LegacyPathFindingPool();
            pool.Return(new LegacyWalkingPath());

            for (int i = 0; i < 100; i++)
            {
                var path = pool.GetRes();
                path.nodes.Add(new PathNodeResult(new IntVector2(1, 1), false));
                pool.Return(path);
            }
        }

        [Benchmark]
        public void Pool_Burst100_Refactored()
        {
            var pool = new PathFindingPool();
            pool.Return(new WalkingPath());

            for (int i = 0; i < 100; i++)
            {
                var path = pool.GetRes();
                path.nodes.Add(new PathNodeResult(new IntVector2(1, 1), false));
                pool.Return(path);
            }
        }

        [Benchmark]
        public void Recycle_NormalPath_NoAlloc()
        {
            _normalPath.recycle();
        }

        [Benchmark]
        public void Recycle_LargePath_CapacityTrim()
        {
            _largePath.recycle();
        }
    }
}
