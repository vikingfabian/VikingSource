using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using VikingEngine;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map;
using VikingEngine.Benchmarks.Pathfinding.Legacy;

namespace VikingEngine.Benchmarks.Pathfinding
{
    [InProcess]
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class PathFindingBenchmarks
    {
        private PathFinding _refactored = null!;
        private LegacyPathFinding _legacy = null!;

        [Params(200, 512, 1184)]
        public int MapWidth;

        public int MapHeight => MapWidth switch
        {
            200 => 176,
            512 => 432,
            1184 => 1024,
            _ => MapWidth
        };

        [GlobalSetup]
        public void Setup()
        {
            TestWorldHelper.SetupFlatWorld(MapWidth, MapHeight);
            _refactored = new PathFinding();
            _legacy = new LegacyPathFinding();
        }

        [BenchmarkCategory("FindPath"), Benchmark(Baseline = true)]
        public object FindPath_Legacy()
        {
            var result = _legacy.FindPath(
                pathThreadIndex: -1,
                center: new IntVector2(10, 10),
                startDir: 0,
                goal: new IntVector2(MapWidth - 10, MapHeight - 10),
                startAsShip: false);
            _legacy.recycle();
            return result;
        }

        [BenchmarkCategory("FindPath"), Benchmark]
        public object FindPath_Refactored()
        {
            var result = _refactored.FindPath(
                pathThreadIndex: -1,
                center: new IntVector2(10, 10),
                startDir: 0,
                goal: new IntVector2(MapWidth - 10, MapHeight - 10),
                startAsShip: false);
            _refactored.recycle();
            return result;
        }

        [BenchmarkCategory("Recycle"), Benchmark(Baseline = true)]
        public void Recycle_Legacy()
        {
            _legacy.recycle();
        }

        [BenchmarkCategory("Recycle"), Benchmark]
        public void Recycle_Refactored()
        {
            _refactored.recycle();
        }
    }
}
