using System;
using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using VikingEngine.Benchmarks.Legacy;

namespace VikingEngine.Benchmarks.Graphics
{
    [Config(typeof(Config))]
    [MemoryDiagnoser]
    public class FactionModelBenchmarks
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                AddJob(Job.MediumRun
                    .WithToolchain(InProcessEmitToolchain.Instance));
            }
        }

        private const int FactionCount = 50;
        private const int ModelsPerFaction = 20;

        [Benchmark(Baseline = true)]
        public object Legacy_50FactionsEliminated_NoCleanup()
        {
            var list = new LegacyFactionModelCache[FactionCount];
            for (int f = 0; f < FactionCount; f++)
            {
                list[f] = new LegacyFactionModelCache();
                for (int m = 0; m < ModelsPerFaction; m++)
                {
                    list[f].LoadModel(m, new byte[1024]);
                }
            }

            // Factions die
            for (int f = 0; f < FactionCount; f++)
            {
                list[f].LegacyDeleteMe();
            }

            return list;
        }

        [Benchmark]
        public object Modern_50FactionsEliminated_ClearModels()
        {
            var list = new ConcurrentDictionary<int, object>[FactionCount];
            for (int f = 0; f < FactionCount; f++)
            {
                list[f] = new ConcurrentDictionary<int, object>();
                for (int m = 0; m < ModelsPerFaction; m++)
                {
                    list[f].TryAdd(m, new byte[1024]);
                }
            }

            // Factions die: clear models cache
            for (int f = 0; f < FactionCount; f++)
            {
                list[f].Clear();
            }

            return list;
        }
    }
}
