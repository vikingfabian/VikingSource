using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using VikingEngine.Benchmarks.Legacy;

namespace VikingEngine.Benchmarks.Engine
{
    [Config(typeof(Config))]
    [MemoryDiagnoser]
    public class SyncActionBenchmarks
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                AddJob(Job.MediumRun
                    .WithToolchain(InProcessEmitToolchain.Instance));
            }
        }

        private static int _counter = 0;
        private static readonly Action _action = () => { _counter++; };

        private readonly LegacySyncActionQueue _legacyQueue = new LegacySyncActionQueue();
        private readonly List<ISyncAction> _modernQueue = new List<ISyncAction>(128);
        private readonly List<ISyncAction> _modernProcessingQueue = new List<ISyncAction>(128);
        private readonly object _modernLock = new object();

        [Benchmark(Baseline = true)]
        public int Legacy_QueueAndProcess_100Actions()
        {
            _counter = 0;

            // Enqueue: struct boxed on push + ConcurrentStack Node allocation
            for (int i = 0; i < 100; i++)
            {
                _legacyQueue.AddSyncAction(new LegacyStructSyncAction(_action));
            }

            // Dequeue and execute
            _legacyQueue.ProcessAll();
            return _counter;
        }

        [Benchmark]
        public int Modern_QueueAndProcess_100Actions()
        {
            _counter = 0;

            // Enqueue: class instance (no interface boxing, no node allocation)
            for (int i = 0; i < 100; i++)
            {
                var action = new SyncAction(_action);
                lock (_modernLock)
                {
                    _modernQueue.Add(action);
                }
            }

            // Double buffered swap and execute
            List<ISyncAction> toProcess;
            lock (_modernLock)
            {
                toProcess = _modernQueue;
            }

            for (int i = 0; i < toProcess.Count; i++)
            {
                toProcess[i].runSyncAction();
            }

            lock (_modernLock)
            {
                toProcess.Clear();
            }

            return _counter;
        }
    }
}
