using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using VikingEngine.Benchmarks.Legacy;

namespace VikingEngine.Benchmarks.Collections
{
    [Config(typeof(Config))]
    [MemoryDiagnoser]
    public class SpottedArrayBenchmarks
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                AddJob(Job.MediumRun
                    .WithToolchain(InProcessEmitToolchain.Instance));
            }
        }

        private const int PeakCount = 500;
        private const int RemainingCount = 10;

        [Benchmark(Baseline = true)]
        public object Legacy_BurstThenShrink_NoTrim()
        {
            var array = new LegacySpottedArray<string>(4);
            for (int i = 0; i < PeakCount; i++)
            {
                array.Add("TestItem");
            }

            for (int i = RemainingCount; i < PeakCount; i++)
            {
                array.RemoveAt(i);
            }

            return array;
        }

        [Benchmark]
        public object Modern_BurstThenShrink_WithTrimExcess()
        {
            var array = new SpottedArray<string>(4);
            for (int i = 0; i < PeakCount; i++)
            {
                array.Add("TestItem");
            }

            for (int i = RemainingCount; i < PeakCount; i++)
            {
                array.RemoveAt(i);
            }

            // Called on state transition or battle exit
            array.TrimExcess();
            return array;
        }
    }
}
