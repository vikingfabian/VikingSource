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

        private float _totalPrepBatchesTimeMs = 0f;
        private float _maxPrepBatchesTimeMs = 0f;

        private float _totalDrawDepthTimeMs = 0f;
        private float _maxDrawDepthTimeMs = 0f;

        private float _totalDrawLitTimeMs = 0f;
        private float _maxDrawLitTimeMs = 0f;

        private int _totalStandardDrawCalls = 0;
        private int _totalInstancedDrawCalls = 0;
        private int _totalRenderedInstances = 0;
        private int _totalInstancedBatches = 0;
        private int _totalFrameSlices = 0;
        private long _totalUploadedBytes = 0;

        // Simulation update rolling metrics
        private float _minUpdateTimeMs = float.MaxValue;
        private float _maxUpdateTimeMs = 0f;
        private float _totalUpdateTimeMs = 0f;
        private uint _updateSampleCount = 0;

        // Aggregated 1-second results
        public int FPS { get; private set; } = 0;
        public float MinRenderTimeMs { get; private set; } = 0f;
        public float MaxRenderTimeMs { get; private set; } = 0f;
        public float AvgRenderTimeMs { get; private set; } = 0f;
        public double PeakRenderTimeMs { get; private set; } = 0;

        public float AvgPrepBatchesTimeMs { get; private set; } = 0f;
        public float PeakPrepBatchesTimeMs { get; private set; } = 0f;

        public float AvgDrawDepthTimeMs { get; private set; } = 0f;
        public float PeakDrawDepthTimeMs { get; private set; } = 0f;

        public float AvgDrawLitTimeMs { get; private set; } = 0f;
        public float PeakDrawLitTimeMs { get; private set; } = 0f;

        public float AvgUpdateTimeMs { get; private set; } = 0f;
        public float MinUpdateTimeMs { get; private set; } = 0f;
        public float MaxUpdateTimeMs { get; private set; } = 0f;
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

        public void RecordUpdate(float updateTimeMs)
        {
            if (updateTimeMs < _minUpdateTimeMs)
            {
                _minUpdateTimeMs = updateTimeMs;
            }
            if (updateTimeMs > _maxUpdateTimeMs)
            {
                _maxUpdateTimeMs = updateTimeMs;
            }
            _totalUpdateTimeMs += updateTimeMs;
            _updateSampleCount++;
        }

        public void RecordFrame(
            float renderTimeMs,
            float prepBatchesTimeMs = 0f,
            float drawDepthTimeMs = 0f,
            float drawLitTimeMs = 0f,
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

            if (prepBatchesTimeMs > _maxPrepBatchesTimeMs)
            {
                _maxPrepBatchesTimeMs = prepBatchesTimeMs;
            }
            _totalPrepBatchesTimeMs += prepBatchesTimeMs;

            if (drawDepthTimeMs > _maxDrawDepthTimeMs)
            {
                _maxDrawDepthTimeMs = drawDepthTimeMs;
            }
            _totalDrawDepthTimeMs += drawDepthTimeMs;

            if (drawLitTimeMs > _maxDrawLitTimeMs)
            {
                _maxDrawLitTimeMs = drawLitTimeMs;
            }
            _totalDrawLitTimeMs += drawLitTimeMs;

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

                AvgPrepBatchesTimeMs = _totalPrepBatchesTimeMs / _sampleCount;
                PeakPrepBatchesTimeMs = _maxPrepBatchesTimeMs;

                AvgDrawDepthTimeMs = _totalDrawDepthTimeMs / _sampleCount;
                PeakDrawDepthTimeMs = _maxDrawDepthTimeMs;

                AvgDrawLitTimeMs = _totalDrawLitTimeMs / _sampleCount;
                PeakDrawLitTimeMs = _maxDrawLitTimeMs;

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

                AvgPrepBatchesTimeMs = 0f;
                PeakPrepBatchesTimeMs = 0f;

                AvgDrawDepthTimeMs = 0f;
                PeakDrawDepthTimeMs = 0f;

                AvgDrawLitTimeMs = 0f;
                PeakDrawLitTimeMs = 0f;

                AvgStandardDrawCallsPerFrame = 0f;
                AvgInstancedDrawCallsPerFrame = 0f;
                AvgTotalDrawCallsPerFrame = 0f;
                AvgRenderedInstancesPerFrame = 0f;
                AvgInstancedBatchesPerFrame = 0f;
                AvgFrameSlicesPerFrame = 0f;
                AvgUploadedKBPerFrame = 0f;
            }

            if (_updateSampleCount > 0)
            {
                MinUpdateTimeMs = _minUpdateTimeMs;
                MaxUpdateTimeMs = _maxUpdateTimeMs;
                AvgUpdateTimeMs = _totalUpdateTimeMs / _updateSampleCount;
            }
            else
            {
                MinUpdateTimeMs = 0f;
                MaxUpdateTimeMs = 0f;
                AvgUpdateTimeMs = 0f;
            }

            // Reset rolling accumulators
            _minRenderTimeMs = float.MaxValue;
            _maxRenderTimeMs = 0f;
            _totalRenderTimeMs = 0f;
            _totalPrepBatchesTimeMs = 0f;
            _maxPrepBatchesTimeMs = 0f;
            _totalDrawDepthTimeMs = 0f;
            _maxDrawDepthTimeMs = 0f;
            _totalDrawLitTimeMs = 0f;
            _maxDrawLitTimeMs = 0f;

            _totalStandardDrawCalls = 0;
            _totalInstancedDrawCalls = 0;
            _totalRenderedInstances = 0;
            _totalInstancedBatches = 0;
            _totalFrameSlices = 0;
            _totalUploadedBytes = 0;
            _sampleCount = 0;

            _minUpdateTimeMs = float.MaxValue;
            _maxUpdateTimeMs = 0f;
            _totalUpdateTimeMs = 0f;
            _updateSampleCount = 0;

            FormattedText = $"{FPS} FPS | Render: {AvgRenderTimeMs:F1}ms (Prep: {AvgPrepBatchesTimeMs:F1}ms, Depth: {AvgDrawDepthTimeMs:F1}ms, Lit: {AvgDrawLitTimeMs:F1}ms, Peak: {PeakRenderTimeMs:F1}ms) | " +
                            $"Simulation Update: {AvgUpdateTimeMs:F1}ms (Peak: {PeakUpdateTimeMs:F1}ms) | VBO Stream: {AvgUploadedKBPerFrame:F1} KB/f | " +
                            $"DrawCalls: {AvgTotalDrawCallsPerFrame:F0} (Inst: {AvgInstancedDrawCallsPerFrame:F0}, Std: {AvgStandardDrawCallsPerFrame:F0}) | Units/Inst: {AvgRenderedInstancesPerFrame:F0} (Batches: {AvgInstancedBatchesPerFrame:F0}, Slices: {AvgFrameSlicesPerFrame:F0})";
        }
    }
}
