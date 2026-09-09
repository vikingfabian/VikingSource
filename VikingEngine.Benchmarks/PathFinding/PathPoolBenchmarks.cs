using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using VikingEngine.Benchmarks.Legacy;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Path;

namespace VikingEngine.Benchmarks.Pathfinding
{
    [Config(typeof(Config))]
    [MemoryDiagnoser]
    public class PathPoolBenchmarks
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                AddJob(Job.MediumRun
                    .WithToolchain(InProcessEmitToolchain.Instance));
            }
        }

        private PathFindingPool _modernPool = null!;
        private LegacyUnboundedPathFindingPool _legacyPool = null!;

        [GlobalSetup]
        public void Setup()
        {
            TestWorldHelper.SetupFlatWorld(32, 32);

            _modernPool = new PathFindingPool();
            _modernPool.Preallocate(1);

            _legacyPool = new LegacyUnboundedPathFindingPool();
            var warm = _legacyPool.GetPf();
            _legacyPool.Return(warm);
        }

        [Benchmark(Baseline = true)]
        public void LegacyPool_GetAndReturn()
        {
            var pf = _legacyPool.GetPf();
            _legacyPool.Return(pf);
        }

        [Benchmark]
        public void ModernPool_Preallocated_GetAndReturn()
        {
            var pf = _modernPool.GetPf();
            _modernPool.Return(pf);
        }

        [Benchmark]
        public object Unpooled_NewInstanceAlloc()
        {
            return new PathFinding();
        }
    }
}
