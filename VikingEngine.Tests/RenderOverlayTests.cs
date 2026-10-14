using System;
using VikingEngine.DebugExtensions;
using Xunit;

namespace VikingEngine.Tests
{
    public class RenderOverlayTests
    {
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
    }
}
