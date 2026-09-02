using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.Benchmarks.Graphics
{
    [Config(typeof(Config))]
    [MemoryDiagnoser]
    public class InstancedSoldierRenderingBenchmarks
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                AddJob(Job.MediumRun.WithToolchain(InProcessEmitToolchain.Instance));
            }
        }

        private const int SoldierCount = 5000;
        private Matrix[] _transforms;
        private VertexVoxelInstance[] _instanceBuffer;

        [GlobalSetup]
        public void Setup()
        {
            _transforms = new Matrix[SoldierCount];
            _instanceBuffer = new VertexVoxelInstance[SoldierCount];

            int side = (int)Math.Sqrt(SoldierCount);
            for (int i = 0; i < SoldierCount; i++)
            {
                int x = i % side;
                int z = i / side;
                _transforms[i] = Matrix.CreateScale(0.2f) * Matrix.CreateTranslation(x * 1.5f, 0, z * 1.5f);
            }
        }

        [Benchmark(Baseline = true)]
        public int Legacy_5000Soldiers_IndividualMatrixParamSet()
        {
            int totalOperations = 0;
            var customData = new Vector4(1, 1, 1, 0);

            // Simulates legacy draw loop: computing and setting matrix parameters per soldier
            for (int i = 0; i < SoldierCount; i++)
            {
                Matrix m = _transforms[i];
                // Simulated shader parameter set
                totalOperations += (int)(m.M11 + m.M41);
            }

            return totalOperations;
        }

        [Benchmark]
        public int Modern_5000Soldiers_InstancedBufferPack()
        {
            var customData = new Vector4(1, 1, 1, 0);

            // Simulates packing into continuous memory for DrawInstancedPrimitives
            for (int i = 0; i < SoldierCount; i++)
            {
                _instanceBuffer[i].Set(ref _transforms[i], customData);
            }

            return _instanceBuffer.Length;
        }
    }
}
