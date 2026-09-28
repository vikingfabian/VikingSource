using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.Benchmarks.Graphics
{
    /// <summary>
    /// Comprehensive benchmark comparing:
    /// 1. ORIGINAL IMPLEMENTATION:
    ///    - Synchronous resource synchronization on the main render thread.
    ///    - Uncontended, single-threaded DrawBatch ingestion.
    /// 2. LOCK-BASED IMPLEMENTATION:
    ///    - Asynchronous resource synchronization on worker threads.
    ///    - Coarse-grained lock(_batches) around RenderBatches and DrawBatch.Add().
    /// 3. REFACTORED IMPLEMENTATION:
    ///    - Asynchronous resource synchronization on worker threads.
    ///    - Lock-free ConcurrentQueue staging queue drained on the render thread at frame start.
    ///    - Zero locks in RenderBatches.
    /// </summary>
    [Config(typeof(Config))]
    [MemoryDiagnoser]
    public class AsyncResourceSyncBenchmarks
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                AddJob(Job.Default
                    .WithToolchain(InProcessEmitToolchain.Instance)
                    .WithWarmupCount(3)
                    .WithIterationCount(6)
                    .WithLaunchCount(1));
            }
        }

        public class BenchmarkDrawModel : AbsDraw
        {
            public override DrawObjType DrawType => DrawObjType.NotDrawable;
            public override Color Color { get => Color.White; set { } }
            public override float Opacity { get => 1f; set { } }
            protected override bool drawable => false;

            private Vector3 _position = Vector3.Zero;

            public override float PositionX { get => _position.X; set => _position.X = value; }
            public override float PositionY { get => _position.Y; set => _position.Y = value; }
            public override float PositionZ { get => _position.Z; set => _position.Z = value; }

            public override Vector2 PositionXY
            {
                get => new Vector2(_position.X, _position.Y);
                set
                {
                    _position.X = value.X;
                    _position.Y = value.Y;
                }
            }

            public override Vector2 PositionXZ
            {
                get => new Vector2(_position.X, _position.Z);
                set
                {
                    _position.X = value.X;
                    _position.Z = value.Y;
                }
            }

            public override Vector3 PositionXYZ
            {
                get => _position;
                set => _position = value;
            }

            public override void AddXY(Vector2 value)
            {
                _position.X += value.X;
                _position.Y += value.Y;
            }

            public override void ColorAndAlpha(Color color, float alpha) { }
            public override void UpdateCulling() { }

            public BenchmarkDrawModel() : base(false)
            {
                inRenderList = true;
            }

            public override void Draw(int cameraIndex) { }
            public override AbsDraw CloneMe() => throw new NotImplementedException();
            public override void copyAllDataFrom(AbsDraw master) { }
        }

        /// <summary>
        /// Exact replica of lock(_batches) approach for side-by-side benchmark comparison.
        /// </summary>
        public class FabianLockDrawBatch
        {
            private readonly Dictionary<int, InstancedDrawBatch> _batches = new Dictionary<int, InstancedDrawBatch>(128);
            private readonly List<AbsDraw> _fallbackDrawList = new List<AbsDraw>(64);
            private int _currentFrame = 0;

            public int FallbackCount => _fallbackDrawList.Count;

            public void Add(int masterId, AbsDraw model)
            {
                lock (_batches)
                {
                    if (!_batches.TryGetValue(masterId, out var batch))
                    {
                        batch = new InstancedDrawBatch(masterId);
                        _batches.Add(masterId, batch);
                    }
                    batch.Add(model);
                }
                model.OnDrawBatchAdd();
            }

            public void RenderBatches()
            {
                lock (_batches)
                {
                    _fallbackDrawList.Clear();
                    foreach (var kv in _batches)
                    {
                        kv.Value.Prepare(0, _currentFrame, _fallbackDrawList);
                    }
                    _currentFrame++;
                }
            }
        }

        public struct SimulatedTile
        {
            public int TileX;
            public int TileY;
            public BenchmarkDrawModel[] FoliageModels;
            public int[] MasterIds;
            public VertexPositionColor[] Vertices;

            private const int SubDivisions = 16; // 16x16 = 256 sub-tiles per tile, matching WorldData.TileSubDivisions

            public static SimulatedTile Create(int x, int y, int foliageCount, int numMasters)
            {
                var models = new BenchmarkDrawModel[foliageCount];
                var masterIds = new int[foliageCount];

                for (int i = 0; i < foliageCount; i++)
                {
                    models[i] = new BenchmarkDrawModel();
                    masterIds[i] = (x * 37 + y * 13 + i) % numMasters;
                }

                // Simulate realistic procedural sub-tile generation (256 sub-tiles per map tile, matching DetailMapTile)
                var vertices = new VertexPositionColor[SubDivisions * SubDivisions * 4];
                int vIdx = 0;

                for (int sy = 0; sy < SubDivisions; sy++)
                {
                    for (int sx = 0; sx < SubDivisions; sx++)
                    {
                        int subX = x * SubDivisions + sx;
                        int subY = y * SubDivisions + sy;
                        int seed = subX * 3 + subY * 11;
                        float height = MathF.Sin(seed * 0.05f) * 2.0f;
                        byte colorVal = (byte)((seed * 17) & 0xFF);
                        var color = new Color(colorVal, (byte)(255 - colorVal), 128);

                        float wx = sx * 0.5f;
                        float wz = sy * 0.5f;

                        vertices[vIdx++] = new VertexPositionColor(new Vector3(wx, height, wz), color);
                        vertices[vIdx++] = new VertexPositionColor(new Vector3(wx + 0.5f, height, wz), color);
                        vertices[vIdx++] = new VertexPositionColor(new Vector3(wx, height, wz + 0.5f), color);
                        vertices[vIdx++] = new VertexPositionColor(new Vector3(wx + 0.5f, height, wz + 0.5f), color);
                    }
                }

                return new SimulatedTile
                {
                    TileX = x,
                    TileY = y,
                    FoliageModels = models,
                    MasterIds = masterIds,
                    Vertices = vertices,
                };
            }
        }

        // Parameters: Number of map tiles streaming into view
        [Params(5, 20, 50)]
        public int TileCount;

        private const int FoliagePerTile = 32;
        private const int NumMasterModels = 8;

        private SimulatedTile[] _tiles = null!;
        private BenchmarkDrawModel[] _contentionModels = null!;

        // Pre-populated batches for steady-state render benchmarks
        private FabianLockDrawBatch _fabianPreSyncedBatch = null!;
        private DrawBatchCollection _refactoredPreSyncedBatch = null!;

        [GlobalSetup]
        public void Setup()
        {
            _tiles = new SimulatedTile[TileCount];
            for (int i = 0; i < TileCount; i++)
            {
                _tiles[i] = SimulatedTile.Create(i % 10, i / 10, FoliagePerTile, NumMasterModels);
            }

            _contentionModels = new BenchmarkDrawModel[TileCount * FoliagePerTile];
            for (int i = 0; i < _contentionModels.Length; i++)
            {
                _contentionModels[i] = new BenchmarkDrawModel();
            }

            // Populate pre-synced batches
            _fabianPreSyncedBatch = new FabianLockDrawBatch();
            _refactoredPreSyncedBatch = new DrawBatchCollection();

            for (int t = 0; t < TileCount; t++)
            {
                var tile = _tiles[t];
                for (int f = 0; f < tile.FoliageModels.Length; f++)
                {
                    _fabianPreSyncedBatch.Add(tile.MasterIds[f], tile.FoliageModels[f]);
                    _refactoredPreSyncedBatch.Add(tile.MasterIds[f], tile.FoliageModels[f]);
                }
            }
        }

        // ====================================================================
        // Category 1: Render Thread Frame Time & Streaming Hitch
        // ====================================================================

        /// <summary>
        /// 1. ORIGINAL IMPLEMENTATION:
        /// Main render thread has to synchronize all streaming tiles synchronously during frame update.
        /// </summary>
        [Benchmark(Baseline = true)]
        public int RenderThread_1_Original_SyncIngestion()
        {
            var batch = new DrawBatchCollection();

            for (int t = 0; t < TileCount; t++)
            {
                var tile = _tiles[t];
                for (int f = 0; f < tile.FoliageModels.Length; f++)
                {
                    batch.Add(tile.MasterIds[f], tile.FoliageModels[f]);
                }
            }

            batch.RemoveAndDraw(false, 0, null, null, null);
            return batch.FallbackDrawListCount;
        }

        /// <summary>
        /// 2. LOCK-BASED IMPLEMENTATION:
        /// Tiles are pre-synchronized async. Render thread only runs RenderBatches wrapped in lock(_batches).
        /// </summary>
        [Benchmark]
        public int RenderThread_2_Fabian_AsyncPreSynced()
        {
            _fabianPreSyncedBatch.RenderBatches();
            return _fabianPreSyncedBatch.FallbackCount;
        }

        /// <summary>
        /// 3. REFACTORED IMPLEMENTATION:
        /// Tiles are pre-synchronized async. Render thread drains lock-free queue and renders with zero locks.
        /// </summary>
        [Benchmark]
        public int RenderThread_3_Refactored_StagedAsyncPreSynced()
        {
            _refactoredPreSyncedBatch.RemoveAndDraw(false, 0, null, null, null);
            return _refactoredPreSyncedBatch.FallbackDrawListCount;
        }

        // ====================================================================
        // Category 2: Ingestion & Lock Contention
        // ====================================================================

        /// <summary>
        /// 1. ORIGINAL PATTERN:
        /// Sequential ingestion of models into DrawBatch on a single thread.
        /// </summary>
        [Benchmark]
        public int Ingestion_1_Original_Sequential()
        {
            var batch = new FabianLockDrawBatch();
            for (int i = 0; i < _contentionModels.Length; i++)
            {
                batch.Add(i % NumMasterModels, _contentionModels[i]);
            }
            batch.RenderBatches();
            return batch.FallbackCount;
        }

        /// <summary>
        /// 2. LOCK-BASED CONCURRENCY:
        /// Multi-threaded parallel addition contending heavily on lock(_batches).
        /// </summary>
        [Benchmark]
        public int Ingestion_2_Fabian_Parallel_LockContention()
        {
            var batch = new FabianLockDrawBatch();
            Parallel.For(0, _contentionModels.Length, i =>
            {
                batch.Add(i % NumMasterModels, _contentionModels[i]);
            });
            batch.RenderBatches();
            return batch.FallbackCount;
        }

        /// <summary>
        /// 3. REFACTORED LOCK-FREE STAGING CONCURRENCY:
        /// Multi-threaded parallel addition enqueuing into lock-free ConcurrentQueue,
        /// followed by fast sequential drain on render thread.
        /// </summary>
        [Benchmark]
        public int Ingestion_3_Refactored_Parallel_LockFreeQueue()
        {
            var batch = new DrawBatchCollection();
            Parallel.For(0, _contentionModels.Length, i =>
            {
                batch.Add(i % NumMasterModels, _contentionModels[i]);
            });
            batch.RemoveAndDraw(false, 0, null, null, null);
            return batch.FallbackDrawListCount;
        }

        // ====================================================================
        // Category 3: Total End-to-End Pipeline Throughput
        // ====================================================================

        /// <summary>
        /// 1. ORIGINAL PIPELINE:
        /// Worker generates tile data -> Queued to render thread -> Render thread syncs all models.
        /// </summary>
        [Benchmark]
        public int Pipeline_1_Original_WorkerGen_RenderSync()
        {
            var workerResult = Task.Run(() =>
            {
                var tiles = new SimulatedTile[TileCount];
                for (int i = 0; i < TileCount; i++)
                {
                    tiles[i] = SimulatedTile.Create(i % 10, i / 10, FoliagePerTile, NumMasterModels);
                }
                return tiles;
            }).Result;

            var batch = new DrawBatchCollection();
            for (int t = 0; t < workerResult.Length; t++)
            {
                var tile = workerResult[t];
                for (int f = 0; f < tile.FoliageModels.Length; f++)
                {
                    batch.Add(tile.MasterIds[f], tile.FoliageModels[f]);
                }
            }

            batch.RemoveAndDraw(false, 0, null, null, null);
            return batch.FallbackDrawListCount;
        }

        /// <summary>
        /// 2. LOCK-BASED CONCURRENCY:
        /// Workers generate and sync in parallel, contending on lock(_batches).
        /// </summary>
        [Benchmark]
        public int Pipeline_2_Fabian_AsyncGenAndSync_Lock()
        {
            var batch = new FabianLockDrawBatch();

            Parallel.For(0, TileCount, t =>
            {
                var tile = SimulatedTile.Create(t % 10, t / 10, FoliagePerTile, NumMasterModels);
                for (int f = 0; f < tile.FoliageModels.Length; f++)
                {
                    batch.Add(tile.MasterIds[f], tile.FoliageModels[f]);
                }
            });

            batch.RenderBatches();
            return batch.FallbackCount;
        }

        /// <summary>
        /// 3. REFACTORED PIPELINE:
        /// Workers generate and sync in parallel lock-free using ConcurrentQueue.
        /// </summary>
        [Benchmark]
        public int Pipeline_3_Refactored_AsyncGenAndSync_Staged()
        {
            var batch = new DrawBatchCollection();

            Parallel.For(0, TileCount, t =>
            {
                var tile = SimulatedTile.Create(t % 10, t / 10, FoliagePerTile, NumMasterModels);
                for (int f = 0; f < tile.FoliageModels.Length; f++)
                {
                    batch.Add(tile.MasterIds[f], tile.FoliageModels[f]);
                }
            });

            batch.RemoveAndDraw(false, 0, null, null, null);
            return batch.FallbackDrawListCount;
        }
    }
}
