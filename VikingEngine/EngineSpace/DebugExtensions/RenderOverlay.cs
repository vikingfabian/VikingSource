using System;
using System.Text;
using VikingEngine.Engine;

namespace VikingEngine.DebugExtensions
{
    public class RenderOverlay
    {
        public static RenderOverlay Instance = new RenderOverlay();

        public bool IsEnabled = true;

        // Rolling metrics for current frame accumulation
        private float _minRenderTimeMs = float.MaxValue;
        private float _maxRenderTimeMs = 0f;
        private float _totalRenderTimeMs = 0f;
        private uint _sampleCount = 0;

        private int _totalStandardDrawCalls = 0;
        private int _totalInstancedDrawCalls = 0;
        private int _totalRenderedInstances = 0;
        private int _totalInstancedBatches = 0;
        private int _totalFrameSlices = 0;
        private long _totalUploadedBytes = 0;

        // Aggregated 1-second results
        public int FPS { get; private set; } = 0;
        public float MinRenderTimeMs { get; private set; } = 0f;
        public float MaxRenderTimeMs { get; private set; } = 0f;
        public float AvgRenderTimeMs { get; private set; } = 0f;
        public double PeakRenderTimeMs { get; private set; } = 0;
        public double PeakUpdateTimeMs { get; private set; } = 0;

        public float AvgStandardDrawCallsPerFrame { get; private set; } = 0f;
        public float AvgInstancedDrawCallsPerFrame { get; private set; } = 0f;
        public float AvgTotalDrawCallsPerFrame { get; private set; } = 0f;
        public float AvgRenderedInstancesPerFrame { get; private set; } = 0f;
        public float AvgInstancedBatchesPerFrame { get; private set; } = 0f;
        public float AvgFrameSlicesPerFrame { get; private set; } = 0f;
        public float AvgUploadedKBPerFrame { get; private set; } = 0f;

        public string FormattedText { get; private set; } = string.Empty;

        public RenderOverlay()
        {
        }

        public void RecordFrame(
            float renderTimeMs,
            int standardDrawCalls = 0,
            int instancedDrawCalls = 0,
            int renderedInstances = 0,
            int batchCount = 0,
            int frameSliceCount = 0,
            long uploadedBytes = 0)
        {
            if (renderTimeMs < _minRenderTimeMs)
            {
                _minRenderTimeMs = renderTimeMs;
            }
            if (renderTimeMs > _maxRenderTimeMs)
            {
                _maxRenderTimeMs = renderTimeMs;
            }
            _totalRenderTimeMs += renderTimeMs;

            _totalStandardDrawCalls += standardDrawCalls;
            _totalInstancedDrawCalls += instancedDrawCalls;
            _totalRenderedInstances += renderedInstances;
            _totalInstancedBatches += batchCount;
            _totalFrameSlices += frameSliceCount;
            _totalUploadedBytes += uploadedBytes;

            _sampleCount++;
        }

        public void UpdateOneSecond(int frameCount, double renderPeak, double updatePeak)
        {
            FPS = frameCount;
            PeakRenderTimeMs = renderPeak;
            PeakUpdateTimeMs = updatePeak;

            if (_sampleCount > 0)
            {
                MinRenderTimeMs = _minRenderTimeMs;
                MaxRenderTimeMs = _maxRenderTimeMs;
                AvgRenderTimeMs = _totalRenderTimeMs / _sampleCount;

                AvgStandardDrawCallsPerFrame = (float)_totalStandardDrawCalls / _sampleCount;
                AvgInstancedDrawCallsPerFrame = (float)_totalInstancedDrawCalls / _sampleCount;
                AvgTotalDrawCallsPerFrame = AvgStandardDrawCallsPerFrame + AvgInstancedDrawCallsPerFrame;
                AvgRenderedInstancesPerFrame = (float)_totalRenderedInstances / _sampleCount;
                AvgInstancedBatchesPerFrame = (float)_totalInstancedBatches / _sampleCount;
                AvgFrameSlicesPerFrame = (float)_totalFrameSlices / _sampleCount;
                AvgUploadedKBPerFrame = (_totalUploadedBytes / 1024f) / _sampleCount;
            }
            else
            {
                MinRenderTimeMs = 0f;
                MaxRenderTimeMs = 0f;
                AvgRenderTimeMs = 0f;

                AvgStandardDrawCallsPerFrame = 0f;
                AvgInstancedDrawCallsPerFrame = 0f;
                AvgTotalDrawCallsPerFrame = 0f;
                AvgRenderedInstancesPerFrame = 0f;
                AvgInstancedBatchesPerFrame = 0f;
                AvgFrameSlicesPerFrame = 0f;
                AvgUploadedKBPerFrame = 0f;
            }

            // Reset rolling accumulators
            _minRenderTimeMs = float.MaxValue;
            _maxRenderTimeMs = 0f;
            _totalRenderTimeMs = 0f;
            _totalStandardDrawCalls = 0;
            _totalInstancedDrawCalls = 0;
            _totalRenderedInstances = 0;
            _totalInstancedBatches = 0;
            _totalFrameSlices = 0;
            _totalUploadedBytes = 0;
            _sampleCount = 0;

            FormattedText = $"{FPS} FPS (r: {AvgRenderTimeMs:F1}ms [min: {MinRenderTimeMs:F1}, max: {MaxRenderTimeMs:F1}, peak: {PeakRenderTimeMs:F1}], uPeak: {PeakUpdateTimeMs:F1}ms) | " +
                            $"DrawCalls: {AvgTotalDrawCallsPerFrame:F0} (Inst: {AvgInstancedDrawCallsPerFrame:F0}, Std: {AvgStandardDrawCallsPerFrame:F0}) | " +
                            $"Units/Inst: {AvgRenderedInstancesPerFrame:F0} (Batches: {AvgInstancedBatchesPerFrame:F0}, Slices: {AvgFrameSlicesPerFrame:F0}) | " +
                            $"VBO Stream: {AvgUploadedKBPerFrame:F1} KB/f";
        }
    }
}
