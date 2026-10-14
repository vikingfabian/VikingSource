using System;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using VikingEngine.DSSWars.Map;
using VikingEngine.Benchmarks.Legacy;

namespace VikingEngine.Benchmarks
{
    [InProcess]
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class MemoryBaselines
    {
        [BenchmarkCategory("SubTile_Size"), Benchmark(Baseline = true)]
        public int MeasureSubTile_LegacySize()
        {
            return Marshal.SizeOf<LegacySubTile>();
        }

        [BenchmarkCategory("SubTile_Size"), Benchmark]
        public int MeasureSubTile_CurrentSize()
        {
            return Marshal.SizeOf<SubTile>();
        }

        [BenchmarkCategory("Tile_Size"), Benchmark(Baseline = true)]
        public int MeasureTile_LegacySize()
        {
            return Marshal.SizeOf<LegacyTile>();
        }

        [BenchmarkCategory("Tile_Size"), Benchmark]
        public int MeasureTile_CurrentSize()
        {
            return Marshal.SizeOf<Tile>();
        }

        [BenchmarkCategory("SubTile_ChunkAlloc"), Benchmark(Baseline = true)]
        public object AllocateSubTileChunk_Legacy()
        {
            var chunk = new LegacySubTile[4096];
            chunk[0].terrainAmount = 10;
            return chunk;
        }

        [BenchmarkCategory("SubTile_ChunkAlloc"), Benchmark]
        public object AllocateSubTileChunk_Current()
        {
            var chunk = new SubTile[4096];
            chunk[0].terrainAmount = 10;
            return chunk;
        }

        [BenchmarkCategory("Tile_ChunkAlloc"), Benchmark(Baseline = true)]
        public object AllocateTileChunk_Legacy()
        {
            var chunk = new LegacyTile[4096];
            chunk[0].CityIndex = 1;
            return chunk;
        }

        [BenchmarkCategory("Tile_ChunkAlloc"), Benchmark]
        public object AllocateTileChunk_Current()
        {
            var chunk = new Tile[4096];
            chunk[0].CityIndex = 1;
            return chunk;
        }
    }
}
