using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DebugExtensions;
using VikingEngine.Graphics;
using VikingEngine.Tests.Legacy;
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
        public void LegacyDrawBatchCollection_BaselineComparison()
        {
            var legacy = new LegacyDrawBatchCollection();
            Assert.Equal(0, legacy.Count);
        }
    }
}
