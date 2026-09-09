using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using VikingEngine.Benchmarks.Legacy;
using VikingEngine.DSSWars.Battle;

namespace VikingEngine.Benchmarks.Battle
{
    [Config(typeof(Config))]
    [MemoryDiagnoser]
    public class SoldierBattleDataBenchmarks
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                AddJob(Job.MediumRun
                    .WithToolchain(InProcessEmitToolchain.Instance));
            }
        }

        private SoldierBattleData[] _pooledBuffer = new SoldierBattleData[100];
        private LegacySoldierBattleData?[] _legacyBuffer = new LegacySoldierBattleData?[100];

        [GlobalSetup]
        public void Setup()
        {
            SoldierBattleData.ClearPool();
            // Pre-warm the pool with 100 instances
            for (int i = 0; i < 100; i++)
            {
                SoldierBattleData.Return(new SoldierBattleData());
            }
        }

        [Benchmark(Baseline = true)]
        public void Legacy_EnterExitBattle_100Soldiers()
        {
            // Enter battle: allocates new object + new List<T>(8)
            for (int i = 0; i < 100; i++)
            {
                _legacyBuffer[i] = new LegacySoldierBattleData(5);
            }

            // Exit battle: drops to null (creates GC garbage)
            for (int i = 0; i < 100; i++)
            {
                _legacyBuffer[i] = null;
            }
        }

        [Benchmark]
        public void Modern_Pooled_EnterExitBattle_100Soldiers()
        {
            // Enter battle: rents from ConcurrentStack
            for (int i = 0; i < 100; i++)
            {
                _pooledBuffer[i] = SoldierBattleData.Rent(null!);
            }

            // Exit battle: returns to pool (0 allocations)
            for (int i = 0; i < 100; i++)
            {
                var item = _pooledBuffer[i];
                _pooledBuffer[i] = null!;
                SoldierBattleData.Return(item);
            }
        }
    }
}
