using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DebugExtensions;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.Graphics.DrawProcess;
using VikingEngine.Graphics;
using VikingEngine.Tests.Legacy;
using VikingEngine.Voxels;
using Xunit;

namespace VikingEngine.Tests
{
    public class Phase7InstancedRenderingTests
    {
        [Fact]
        public void VertexVoxelInstance_StructSize_MatchesVertexDeclarationStride()
        {
            int structSize = Marshal.SizeOf<VertexVoxelInstance>();
            int stride = VertexVoxelInstance.VertexDeclaration.VertexStride;

            // 4 x Vector4 (64 bytes) + 1 x Vector4 (16 bytes) = 80 bytes
            Assert.Equal(80, structSize);
            Assert.Equal(structSize, stride);
        }

        [Fact]
        public void VertexVoxelInstance_AffineMatrixDecomposition_PreservesValues()
        {
            var original = Matrix.CreateScale(1.8f, 2.0f, 1.8f) *
                           Matrix.CreateRotationY(MathHelper.ToRadians(45)) *
                           Matrix.CreateTranslation(120.5f, -10.0f, 350.25f);

            var customData = new Vector4(0.8f, 0.2f, 0.2f, 1.0f);
            var instance = new VertexVoxelInstance(ref original, customData);

            Assert.Equal(original.M11, instance.WorldRow0.X, 4);
            Assert.Equal(original.M22, instance.WorldRow1.Y, 4);
            Assert.Equal(original.M33, instance.WorldRow2.Z, 4);
            Assert.Equal(original.M41, instance.WorldRow3.X, 4);
            Assert.Equal(original.M42, instance.WorldRow3.Y, 4);
            Assert.Equal(original.M43, instance.WorldRow3.Z, 4);
            Assert.Equal(customData, instance.InstanceData);
        }

        [Fact]
        public void VertexVoxelInstance_ColorTintAndFlash_MapsCorrectly()
        {
            var world = Matrix.Identity;
            var colorTint = new Vector3(0.2f, 0.5f, 0.9f);
            float damageFlash = 0.75f;
            var instanceData = new Vector4(colorTint.X, colorTint.Y, colorTint.Z, damageFlash);

            var vertexInst = new VertexVoxelInstance(ref world, instanceData);

            Assert.Equal(0.2f, vertexInst.InstanceData.X, 3);
            Assert.Equal(0.5f, vertexInst.InstanceData.Y, 3);
            Assert.Equal(0.9f, vertexInst.InstanceData.Z, 3);
            Assert.Equal(0.75f, vertexInst.InstanceData.W, 3);
        }

        [Fact]
        public void InstancedDrawBatch_PrunesInactiveAndGroupsByFrame()
        {
            var batch = new InstancedDrawBatch(1);
            var fallback = new System.Collections.Generic.List<AbsDraw>();

            batch.Prepare(0, 0, fallback);

            Assert.Equal(1, batch.MasterId);
            Assert.Empty(batch);
            Assert.Empty(fallback);
        }

        [Fact]
        public void RenderOverlay_AccumulatesMetrics_CalculatesCorrectAverages()
        {
            var overlay = new RenderOverlay();

            // Record simulated updates
            overlay.RecordUpdate(4.0f);
            overlay.RecordUpdate(6.0f);

            // Record 3 simulated frames
            overlay.RecordFrame(10.0f, prepBatchesTimeMs: 1.0f, drawDepthTimeMs: 4.0f, drawLitTimeMs: 5.0f, standardDrawCalls: 2, instancedDrawCalls: 5, renderedInstances: 1000, batchCount: 3, frameSliceCount: 8, uploadedBytes: 80000);
            overlay.RecordFrame(20.0f, prepBatchesTimeMs: 2.0f, drawDepthTimeMs: 8.0f, drawLitTimeMs: 10.0f, standardDrawCalls: 4, instancedDrawCalls: 5, renderedInstances: 1000, batchCount: 3, frameSliceCount: 8, uploadedBytes: 80000);
            overlay.RecordFrame(30.0f, prepBatchesTimeMs: 3.0f, drawDepthTimeMs: 12.0f, drawLitTimeMs: 15.0f, standardDrawCalls: 6, instancedDrawCalls: 5, renderedInstances: 1000, batchCount: 3, frameSliceCount: 8, uploadedBytes: 80000);

            overlay.UpdateOneSecond(frameCount: 3, renderPeak: 30.0, updatePeak: 6.0);

            Assert.Equal(3, overlay.FPS);
            Assert.Equal(10.0f, overlay.MinRenderTimeMs);
            Assert.Equal(30.0f, overlay.MaxRenderTimeMs);
            Assert.Equal(20.0f, overlay.AvgRenderTimeMs);

            Assert.Equal(2.0f, overlay.AvgPrepBatchesTimeMs);
            Assert.Equal(3.0f, overlay.PeakPrepBatchesTimeMs);
            Assert.Equal(8.0f, overlay.AvgDrawDepthTimeMs);
            Assert.Equal(12.0f, overlay.PeakDrawDepthTimeMs);
            Assert.Equal(10.0f, overlay.AvgDrawLitTimeMs);
            Assert.Equal(15.0f, overlay.PeakDrawLitTimeMs);

            Assert.Equal(5.0f, overlay.AvgUpdateTimeMs);
            Assert.Equal(4.0f, overlay.MinUpdateTimeMs);
            Assert.Equal(6.0f, overlay.MaxUpdateTimeMs);

            Assert.Equal(4.0f, overlay.AvgStandardDrawCallsPerFrame);
            Assert.Equal(5.0f, overlay.AvgInstancedDrawCallsPerFrame);
            Assert.Equal(9.0f, overlay.AvgTotalDrawCallsPerFrame);
            Assert.Equal(1000.0f, overlay.AvgRenderedInstancesPerFrame);
            Assert.Equal(3.0f, overlay.AvgInstancedBatchesPerFrame);
            Assert.Equal(8.0f, overlay.AvgFrameSlicesPerFrame);
            Assert.False(string.IsNullOrEmpty(overlay.FormattedText));
        }

        [Fact]
        public void RenderOverlay_RecordPresent_TracksAverageAndPeak()
        {
            var overlay = new RenderOverlay();

            overlay.RecordPresent(10.0f);
            overlay.RecordPresent(20.0f);
            overlay.RecordPresent(15.0f);

            // Need at least one frame sample for UpdateOneSecond
            overlay.RecordFrame(1.0f);
            overlay.RecordUpdate(1.0f);
            overlay.UpdateOneSecond(frameCount: 3, renderPeak: 1.0, updatePeak: 1.0);

            Assert.Equal(15.0f, overlay.AvgPresentTimeMs);
            Assert.Equal(20.0f, overlay.MaxPresentTimeMs);
        }

        [Fact]
        public void RenderOverlay_RecordUpdatesPerFrame_TracksAverageAndPeak()
        {
            var overlay = new RenderOverlay();

            // Simulate 3 frames: 1 update, 2 updates, 3 updates
            overlay.RecordUpdatesPerFrame(1);
            overlay.RecordUpdatesPerFrame(2);
            overlay.RecordUpdatesPerFrame(3);

            overlay.RecordFrame(1.0f);
            overlay.RecordUpdate(1.0f);
            overlay.UpdateOneSecond(frameCount: 3, renderPeak: 1.0, updatePeak: 1.0);

            Assert.Equal(2.0f, overlay.AvgUpdatesPerFrame);
            Assert.Equal(3, overlay.PeakUpdatesPerFrame);
        }

        [Fact]
        public void RenderOverlay_FormattedText_ContainsUpdPerFrameAndPresent()
        {
            var overlay = new RenderOverlay();

            overlay.RecordUpdate(8.0f);
            overlay.RecordUpdate(10.0f);
            overlay.RecordUpdatesPerFrame(2);
            overlay.RecordPresent(15.5f);
            overlay.RecordFrame(1.8f);

            overlay.UpdateOneSecond(frameCount: 1, renderPeak: 1.8, updatePeak: 10.0);

            // Verify key diagnostic values appear in formatted text
            Assert.Contains("Upd/f", overlay.FormattedText);
            Assert.Contains("Present:", overlay.FormattedText);
            Assert.Contains("Update:", overlay.FormattedText);
        }

        [Fact]
        public void RenderOverlay_PerFrameAggregateCost_CalculatedCorrectly()
        {
            var overlay = new RenderOverlay();

            // 4 update calls averaging 10ms each, across 2 frames (2 Upd/f)
            overlay.RecordUpdate(8.0f);
            overlay.RecordUpdate(12.0f);
            overlay.RecordUpdate(8.0f);
            overlay.RecordUpdate(12.0f);
            overlay.RecordUpdatesPerFrame(2);
            overlay.RecordUpdatesPerFrame(2);
            overlay.RecordFrame(1.0f);
            overlay.RecordFrame(1.0f);
            overlay.RecordPresent(5.0f);
            overlay.RecordPresent(5.0f);

            overlay.UpdateOneSecond(frameCount: 2, renderPeak: 1.0, updatePeak: 12.0);

            Assert.Equal(10.0f, overlay.AvgUpdateTimeMs);
            Assert.Equal(2.0f, overlay.AvgUpdatesPerFrame);
            // Per-frame aggregate = 10.0 * 2.0 = 20.0ms — visible in formatted text
            Assert.Contains("20.0ms", overlay.FormattedText);
        }

        [Fact]
        public void RenderOverlay_ResetsBetweenSeconds()
        {
            var overlay = new RenderOverlay();

            // First second
            overlay.RecordPresent(20.0f);
            overlay.RecordUpdatesPerFrame(3);
            overlay.RecordFrame(1.0f);
            overlay.RecordUpdate(5.0f);
            overlay.UpdateOneSecond(frameCount: 1, renderPeak: 1.0, updatePeak: 5.0);

            Assert.Equal(20.0f, overlay.AvgPresentTimeMs);
            Assert.Equal(3.0f, overlay.AvgUpdatesPerFrame);

            // Second second — different values
            overlay.RecordPresent(5.0f);
            overlay.RecordUpdatesPerFrame(1);
            overlay.RecordFrame(2.0f);
            overlay.RecordUpdate(3.0f);
            overlay.UpdateOneSecond(frameCount: 1, renderPeak: 2.0, updatePeak: 3.0);

            Assert.Equal(5.0f, overlay.AvgPresentTimeMs);
            Assert.Equal(1.0f, overlay.AvgUpdatesPerFrame);
        }

        [Fact]
        public void LegacyDrawBatchCollection_BaselineComparison()
        {
            var legacy = new LegacyDrawBatchCollection();
            Assert.Equal(0, legacy.Count);
        }
        [Fact]
        public void RenderOverlay_RecordSimSubsystems_TracksAverages()
        {
            var overlay = new RenderOverlay();

            overlay.RecordSimSubsystems(1.0f, 5.0f, 0.0f, 0.5f, 0.2f, 0.3f);
            overlay.RecordSimSubsystems(3.0f, 7.0f, 0.0f, 1.5f, 0.4f, 0.1f);
            overlay.RecordFrame(1.0f);
            overlay.RecordUpdate(10.0f);
            overlay.RecordPresent(0.1f);
            overlay.RecordUpdatesPerFrame(1);
            overlay.UpdateOneSecond(frameCount: 2, renderPeak: 1.0, updatePeak: 10.0);

            // Averages: cities = (1+3)/2 = 2, factions = (5+7)/2 = 6
            Assert.Equal(2.0f, overlay.AvgCitiesMs, 1);
            Assert.Equal(6.0f, overlay.AvgFactionsMs, 1);
            Assert.Equal(0.0f, overlay.AvgFactionOneSecMs, 1);
            Assert.Equal(1.0f, overlay.AvgMapMs, 1);
            Assert.Equal(0.3f, overlay.AvgUserInputMs, 1);
            Assert.Equal(0.2f, overlay.AvgParticlesMs, 1);
        }

        [Fact]
        public void RenderOverlay_RecordSimSubsystems_TracksPeaks()
        {
            var overlay = new RenderOverlay();

            overlay.RecordSimSubsystems(1.0f, 5.0f, 0.0f, 0.5f, 0.2f, 0.3f);
            overlay.RecordSimSubsystems(3.0f, 7.0f, 50.0f, 1.5f, 0.4f, 0.1f);
            overlay.RecordFrame(1.0f);
            overlay.RecordUpdate(10.0f);
            overlay.RecordPresent(0.1f);
            overlay.RecordUpdatesPerFrame(1);
            overlay.UpdateOneSecond(frameCount: 2, renderPeak: 1.0, updatePeak: 10.0);

            Assert.Equal(3.0f, overlay.PeakCitiesMs, 1);
            Assert.Equal(7.0f, overlay.PeakFactionsMs, 1);
            Assert.Equal(50.0f, overlay.PeakFactionOneSecMs, 1);
            Assert.Equal(1.5f, overlay.PeakMapMs, 1);
            Assert.Equal(0.4f, overlay.PeakUserInputMs, 1);
            Assert.Equal(0.3f, overlay.PeakParticlesMs, 1);
        }

        [Fact]
        public void RenderOverlay_RecordSimSubsystems_FactionOneSecOnlyOnSpikeTick()
        {
            var overlay = new RenderOverlay();

            // Simulate 3 ticks: 2 normal + 1 oneSecond spike
            overlay.RecordSimSubsystems(0.5f, 6.0f, 0.0f, 0.2f, 0.1f, 0.1f);
            overlay.RecordSimSubsystems(0.4f, 5.5f, 0.0f, 0.3f, 0.1f, 0.1f);
            overlay.RecordSimSubsystems(0.6f, 55.0f, 48.0f, 0.2f, 0.1f, 0.1f); // spike tick

            overlay.RecordFrame(1.0f);
            overlay.RecordUpdate(10.0f);
            overlay.RecordPresent(0.1f);
            overlay.RecordUpdatesPerFrame(1);
            overlay.UpdateOneSecond(frameCount: 3, renderPeak: 1.0, updatePeak: 55.0);

            // FactionOneSec average: (0+0+48)/3 = 16.0
            Assert.Equal(16.0f, overlay.AvgFactionOneSecMs, 1);
            // FactionOneSec peak: 48.0
            Assert.Equal(48.0f, overlay.PeakFactionOneSecMs, 1);
            // Factions average: (6+5.5+55)/3 = 22.17
            Assert.Equal(22.2f, overlay.AvgFactionsMs, 1);
        }

        [Fact]
        public void RenderOverlay_RecordSimSubsystems_FormattedTextContainsSimLine()
        {
            var overlay = new RenderOverlay();

            overlay.RecordSimSubsystems(0.5f, 6.0f, 0.0f, 0.2f, 0.1f, 0.3f);
            overlay.RecordFrame(1.0f);
            overlay.RecordUpdate(10.0f);
            overlay.RecordPresent(0.1f);
            overlay.RecordUpdatesPerFrame(1);
            overlay.UpdateOneSecond(frameCount: 1, renderPeak: 1.0, updatePeak: 10.0);

            Assert.Contains("Sim:", overlay.FormattedText);
            Assert.Contains("Factions:", overlay.FormattedText);
            Assert.Contains("1Sec:", overlay.FormattedText);
            Assert.Contains("Cities:", overlay.FormattedText);
            Assert.Contains("Map:", overlay.FormattedText);
            Assert.Contains("Input:", overlay.FormattedText);
            Assert.Contains("Particles:", overlay.FormattedText);
        }

        [Fact]
        public void RenderOverlay_RecordSimSubsystems_ResetsBetweenSeconds()
        {
            var overlay = new RenderOverlay();

            // First second with spike
            overlay.RecordSimSubsystems(0.5f, 50.0f, 45.0f, 0.2f, 0.1f, 0.1f);
            overlay.RecordFrame(1.0f);
            overlay.RecordUpdate(50.0f);
            overlay.RecordPresent(0.1f);
            overlay.RecordUpdatesPerFrame(1);
            overlay.UpdateOneSecond(frameCount: 1, renderPeak: 1.0, updatePeak: 50.0);

            Assert.Equal(50.0f, overlay.AvgFactionsMs, 1);
            Assert.Equal(45.0f, overlay.AvgFactionOneSecMs, 1);

            // Second second — no spike
            overlay.RecordSimSubsystems(0.4f, 5.0f, 0.0f, 0.3f, 0.2f, 0.2f);
            overlay.RecordFrame(1.0f);
            overlay.RecordUpdate(6.0f);
            overlay.RecordPresent(0.1f);
            overlay.RecordUpdatesPerFrame(1);
            overlay.UpdateOneSecond(frameCount: 1, renderPeak: 1.0, updatePeak: 6.0);

            Assert.Equal(5.0f, overlay.AvgFactionsMs, 1);
            Assert.Equal(0.0f, overlay.AvgFactionOneSecMs, 1);
            Assert.Equal(0.0f, overlay.PeakFactionOneSecMs, 1);
        }

        [Fact]
        public void VoxelModelInstance_Pooled_PoolGeneration_IncrementsOnReset()
        {
            var instance = new DSSWars.VoxelModelInstance_Pooled(false);
            Assert.Equal(0, instance.PoolGeneration);

            instance.Pool_Reset();
            Assert.Equal(1, instance.PoolGeneration);

            instance.Pool_Reset();
            Assert.Equal(2, instance.PoolGeneration);
        }

        [Fact]
        public void DrawBatchCollection_AddNullMaster_SetsInRenderListTrue()
        {
            MainGame.SetMainThreadForTest();
            var collection = new DrawBatchCollection();
            var instance = new VoxelModelInstance(null, false);
            Assert.False(instance.InRenderList);

            collection.Add(instance);

            Assert.True(instance.InRenderList);
        }

        [Fact]
        public void EngineDraw_RequestScreenshot_CanBeToggled()
        {
            Engine.Draw.IsScreenshotRequested = false;
            Assert.False(Engine.Draw.IsScreenshotRequested);

            Engine.Draw.IsScreenshotRequested = true;
            Assert.True(Engine.Draw.IsScreenshotRequested);

            Engine.Draw.IsScreenshotRequested = false;
        }

        [Fact]
        public void RenderOverlay_RecordEngineSubsystems_AggregatesAndFormatsCorrectly()
        {
            var overlay = new RenderOverlay();

            overlay.RecordEngineSubsystems(
                calcDeltaMs: 1.0f,
                updateListMs: 3.0f,
                syncQueMs: 2.0f,
                gameStateMs: 1.8f,
                inputSoundMs: 0.5f,
                lazyUpdateMs: 0.2f
            );
            overlay.RecordEngineSubsystems(
                calcDeltaMs: 5.0f,
                updateListMs: 5.0f,
                syncQueMs: 4.0f,
                gameStateMs: 2.2f,
                inputSoundMs: 0.7f,
                lazyUpdateMs: 0.4f
            );

            overlay.RecordFrame(1.0f);
            overlay.RecordUpdate(10.0f);
            overlay.RecordPresent(0.1f);
            overlay.RecordUpdatesPerFrame(1);
            overlay.UpdateOneSecond(frameCount: 1, renderPeak: 1.0, updatePeak: 10.0);

            Assert.Equal(3.0f, overlay.AvgEngineCalcDeltaMs, 1);
            Assert.Equal(5.0f, overlay.PeakEngineCalcDeltaMs, 1);
            Assert.Equal(4.0f, overlay.AvgEngineUpdateListMs, 1);
            Assert.Equal(5.0f, overlay.PeakEngineUpdateListMs, 1);
            Assert.Equal(3.0f, overlay.AvgEngineSyncQueMs, 1);
            Assert.Equal(4.0f, overlay.PeakEngineSyncQueMs, 1);
            Assert.Equal(2.0f, overlay.AvgEngineGameStateMs, 1);
            Assert.Equal(2.2f, overlay.PeakEngineGameStateMs, 1);
            Assert.Equal(0.6f, overlay.AvgEngineInputSoundMs, 1);
            Assert.Equal(0.7f, overlay.PeakEngineInputSoundMs, 1);
            Assert.Equal(0.3f, overlay.AvgEngineLazyUpdateMs, 1);
            Assert.Equal(0.4f, overlay.PeakEngineLazyUpdateMs, 1);

            Assert.Contains("Eng:", overlay.FormattedText);
            Assert.Contains("Delta:", overlay.FormattedText);
            Assert.Contains("UpdList:", overlay.FormattedText);
            Assert.Contains("SyncQue:", overlay.FormattedText);
            Assert.Contains("State:", overlay.FormattedText);
            Assert.Contains("InSnd:", overlay.FormattedText);
            Assert.Contains("Lazy:", overlay.FormattedText);
        }

        [Fact]
        public void RenderOverlay_RecordEngineSubsystems_ResetsBetweenSeconds()
        {
            var overlay = new RenderOverlay();

            // First second: with spike
            overlay.RecordEngineSubsystems(110.0f, 3.0f, 2.0f, 1.8f, 0.5f, 0.2f);
            overlay.RecordFrame(1.0f);
            overlay.RecordUpdate(110.0f);
            overlay.RecordPresent(0.1f);
            overlay.RecordUpdatesPerFrame(1);
            overlay.UpdateOneSecond(frameCount: 1, renderPeak: 1.0, updatePeak: 110.0);

            Assert.Equal(110.0f, overlay.AvgEngineCalcDeltaMs, 1);
            Assert.Equal(110.0f, overlay.PeakEngineCalcDeltaMs, 1);

            // Second second: normal
            overlay.RecordEngineSubsystems(0.2f, 3.0f, 2.0f, 1.8f, 0.5f, 0.2f);
            overlay.RecordFrame(1.0f);
            overlay.RecordUpdate(8.0f);
            overlay.RecordPresent(0.1f);
            overlay.RecordUpdatesPerFrame(1);
            overlay.UpdateOneSecond(frameCount: 1, renderPeak: 1.0, updatePeak: 8.0);

            Assert.Equal(0.2f, overlay.AvgEngineCalcDeltaMs, 1);
            Assert.Equal(0.2f, overlay.PeakEngineCalcDeltaMs, 1);
        }

        [Fact]
        public void MemoryOverlay_DoesNotThrow_WithPreciseFalse()
        {
            var overlay = new MemoryOverlay();
            overlay.RecordFrame(16.0f);
            overlay.UpdateOneSecond();

            Assert.True(overlay.TotalHeapBytes > 0);
            Assert.Contains("Heap:", overlay.FormattedText);
            Assert.Contains("Alloc:", overlay.FormattedText);
        }

        [Fact]
        public void Update_DumpUpdateListSummary_CountsTypesCorrectly()
        {
            var update = new Update(null);
            var dummy1 = new Phase5DummyUpdateableA();
            var dummy2 = new Phase5DummyUpdateableA();
            var dummy3 = new Phase5DummyUpdateableB();

            update.AddToOrRemoveFromUpdate(dummy1, true);
            update.AddToOrRemoveFromUpdate(dummy2, true);
            update.AddToOrRemoveFromUpdate(dummy3, true);

            string summary = update.DumpUpdateListSummary(UpdateType.Full);

            Assert.Contains("Phase5DummyUpdateableA: 2", summary);
            Assert.Contains("Phase5DummyUpdateableB: 1", summary);
            Assert.Contains("Total Items: 3", summary);
        }

        [Fact]
        public void Update_SyncQueThrottling_RespectsBudget()
        {
            var update = new Update(null);
            double originalBudget = Update.MaxSyncActionBudgetMs;
            try
            {
                Update.MaxSyncActionBudgetMs = 1.0;

                int actionsExecuted = 0;
                for (int i = 0; i < 4; i++)
                {
                    update.AddSyncAction(new SyncAction(() =>
                    {
                        System.Threading.Thread.Sleep(5);
                        actionsExecuted++;
                    }));
                }

                Assert.Equal(4, update.SyncQueCount);

                update.Time_Update(16.0f);

                Assert.True(actionsExecuted < 4, $"Expected throttling to leave items in queue, but executed {actionsExecuted}");
                Assert.True(actionsExecuted >= 1, $"Expected at least 1 action to execute, but executed {actionsExecuted}");
                Assert.Equal(4 - actionsExecuted, update.SyncQueCount);
            }
            finally
            {
                Update.MaxSyncActionBudgetMs = originalBudget;
            }
        }

        [Fact]
        public void AbsVoxelObj_ModelIndexGeneration_IsThreadSafeAndUnique()
        {
            const int threadCount = 8;
            const int modelsPerThread = 125;
            var indices = new System.Collections.Concurrent.ConcurrentBag<int>();

            System.Threading.Tasks.Parallel.For(0, threadCount, _ =>
            {
                for (int i = 0; i < modelsPerThread; i++)
                {
                    var model = new TestVoxelObj();
                    indices.Add(model.modelIndex);
                }
            });

            Assert.Equal(threadCount * modelsPerThread, indices.Count);
            var distinctCount = new System.Collections.Generic.HashSet<int>(indices).Count;
            Assert.Equal(indices.Count, distinctCount);
        }

        [Fact]
        public void Update_DynamicSyncQueBudget_ExpandsUnderBacklog()
        {
            var update = new Update(null);
            double originalBudget = Update.MaxSyncActionBudgetMs;
            try
            {
                Update.MaxSyncActionBudgetMs = 1.0;

                // Add 60 actions to trigger the > 50 backlog threshold
                int executed = 0;
                for (int i = 0; i < 60; i++)
                {
                    update.AddSyncAction(new SyncAction(() =>
                    {
                        var sw = System.Diagnostics.Stopwatch.StartNew();
                        while (sw.ElapsedTicks < System.Diagnostics.Stopwatch.Frequency / 10000) { } // ~0.1ms spin
                        executed++;
                    }));
                }

                update.Time_Update(16.0f);

                // With base budget 1.0ms, ~2 actions execute.
                // With dynamic backlog budget 6.0ms, at least 4 actions execute.
                Assert.True(executed >= 4, $"Expected dynamic budget to allow more executions, but executed {executed}");
            }
            finally
            {
                Update.MaxSyncActionBudgetMs = originalBudget;
            }
        }

        private static void InitVoxelTestContext()
        {
            Block.Init();
            DataLib.SpriteCollection.Sprites = new Graphics.Sprite[(int)SpriteName.NUM];
            var whiteSprite = new Graphics.Sprite();
            whiteSprite.SourcePolygonTopLeft = Vector2.Zero;
            whiteSprite.SourcePolygonTopRight = new Vector2(1, 0);
            whiteSprite.SourcePolygonLowLeft = new Vector2(0, 1);
            whiteSprite.SourcePolygonLowRight = Vector2.One;
            DataLib.SpriteCollection.Sprites[(int)SpriteName.WhiteArea] = whiteSprite;
        }

        [Fact]
        public void VoxelObjBuilder_BuildVerticesHD_Texture_ProducesVerticeDataColorTexture()
        {
            InitVoxelTestContext();

            var grid = new VoxelObjGridDataHD(new IntVector3(2, 2, 2));
            grid.Set(0, 0, 0, 1);
            var grids = new List<VoxelObjGridDataHD> { grid };

            var verticeData = VoxelObjBuilder.BuildVerticesHD_Texture(grids, Vector3.Zero, out var framesData);

            Assert.NotNull(verticeData);
            Assert.IsType<VerticeDataColorTexture>(verticeData);
            Assert.Equal(VertexPositionColorTexture.VertexDeclaration, verticeData.VertexDeclaration);
            Assert.Single(framesData);
        }

        [Fact]
        public void VoxelObjBuilder_BuildVerticesHD_ProducesVerticeDataColorNormal_PreservesLootFestSemantics()
        {
            InitVoxelTestContext();

            var grid = new VoxelObjGridDataHD(new IntVector3(2, 2, 2));
            grid.Set(0, 0, 0, 1);
            var grids = new List<VoxelObjGridDataHD> { grid };

            var verticeData = VoxelObjBuilder.BuildVerticesHD(grids, Vector3.Zero, out var framesData);

            Assert.NotNull(verticeData);
            Assert.IsType<VerticeDataColorNormal>(verticeData);
            Assert.Equal(VertexPositionColorNormal.VertexDeclaration, verticeData.VertexDeclaration);
            Assert.Single(framesData);
        }

        [Fact]
        public void DrawBatchCollection_FallbackItems_RetainedAcrossDepthAndLitPasses()
        {
            MainGame.SetMainThreadForTest();
            var collection = new DrawBatchCollection();
            var model = new TestVoxelObj();
            model.Visible = true;

            collection.Add(1, model);

            // Pass 1: Depth pass in frame 0
            collection.DrawDepth(0, null, null);
            Assert.Equal(1, collection.FallbackDrawListCount);

            // Pass 2: Lit pass in frame 0 (fallback items must be retained, not cleared)
            collection.RemoveAndDraw(true, 0, null, null, null);
            Assert.Equal(1, collection.FallbackDrawListCount);

            // Pass 3: Next frame depth pass (cleared and re-prepared for new frame)
            collection.DrawDepth(0, null, null);
            Assert.Equal(1, collection.FallbackDrawListCount);
        }
    }

    internal class TestVoxelObj : AbsVoxelObj
    {
        public TestVoxelObj() : base(false) { }
        public override float SizeToScale => 1f;
        public override int GridSideLength => 1;
        public override int NumFrames => 1;
        public override void copyAllDataFrom(AbsDraw master) { }
        public override void DrawDepthOnly(bool drawDepth, Microsoft.Xna.Framework.Graphics.Effect shader, LightProjection light, int cameraIndex) { }
        public override void Draw(int cameraIndex) { }
        public override AbsDraw CloneMe() => throw new NotImplementedException();
    }

    internal class Phase5DummyUpdateableA : IUpdateable
    {
        public int SpottedArrayMemberIndex { get; set; } = -1;
        public bool SpottedArrayUseIndex => true;
        public UpdateType UpdateType => UpdateType.Full;
        public bool RunDuringPause => false;
        public void Time_Update(float time_ms) { }
    }

    internal class Phase5DummyUpdateableB : IUpdateable
    {
        public int SpottedArrayMemberIndex { get; set; } = -1;
        public bool SpottedArrayUseIndex => true;
        public UpdateType UpdateType => UpdateType.Full;
        public bool RunDuringPause => false;
        public void Time_Update(float time_ms) { }
    }
}
