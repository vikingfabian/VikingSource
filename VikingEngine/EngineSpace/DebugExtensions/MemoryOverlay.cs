using System;
using System.Text;
using VikingEngine.Engine;

namespace VikingEngine.DebugExtensions
{
    public class MemoryOverlay
    {
        public static MemoryOverlay Instance = new MemoryOverlay();

        public bool IsEnabled = true;

        private int _prevGen0 = 0;
        private int _prevGen1 = 0;
        private int _prevGen2 = 0;
        private long _prevAllocatedBytes = 0;

        public int Gen0Delta { get; private set; } = 0;
        public int Gen1Delta { get; private set; } = 0;
        public int Gen2Delta { get; private set; } = 0;
        public long TotalHeapBytes { get; private set; } = 0;
        public long AllocatedBytesDelta { get; private set; } = 0;

        private float _minFrameTimeMs = float.MaxValue;
        private float _maxFrameTimeMs = 0f;
        private float _totalFrameTimeMs = 0f;
        private uint _sampleCount = 0;

        public float MinFrameTimeMs { get; private set; } = 0f;
        public float MaxFrameTimeMs { get; private set; } = 0f;
        public float AvgFrameTimeMs { get; private set; } = 0f;

        public string FormattedText { get; private set; } = string.Empty;

        public MemoryOverlay()
        {
            _prevGen0 = GC.CollectionCount(0);
            _prevGen1 = GC.CollectionCount(1);
            _prevGen2 = GC.CollectionCount(2);
            _prevAllocatedBytes = GC.GetTotalAllocatedBytes(true);
            TotalHeapBytes = GC.GetTotalMemory(false);
        }

        public void RecordFrame(float frameTimeMs)
        {
            if (frameTimeMs < _minFrameTimeMs)
            {
                _minFrameTimeMs = frameTimeMs;
            }
            if (frameTimeMs > _maxFrameTimeMs)
            {
                _maxFrameTimeMs = frameTimeMs;
            }
            _totalFrameTimeMs += frameTimeMs;

            _sampleCount++;
        }

        public void UpdateOneSecond()
        {
            var currentGen0 = GC.CollectionCount(0);
            var currentGen1 = GC.CollectionCount(1);
            var currentGen2 = GC.CollectionCount(2);
            var currentAllocated = GC.GetTotalAllocatedBytes(true);

            Gen0Delta = currentGen0 - _prevGen0;
            Gen1Delta = currentGen1 - _prevGen1;
            Gen2Delta = currentGen2 - _prevGen2;
            AllocatedBytesDelta = currentAllocated - _prevAllocatedBytes;
            TotalHeapBytes = GC.GetTotalMemory(false);

            _prevGen0 = currentGen0;
            _prevGen1 = currentGen1;
            _prevGen2 = currentGen2;
            _prevAllocatedBytes = currentAllocated;

            if (_sampleCount > 0)
            {
                MinFrameTimeMs = _minFrameTimeMs;
                MaxFrameTimeMs = _maxFrameTimeMs;
                AvgFrameTimeMs = _totalFrameTimeMs / _sampleCount;
            }
            else
            {
                MinFrameTimeMs = 0f;
                MaxFrameTimeMs = 0f;
                AvgFrameTimeMs = 0f;
            }

            _minFrameTimeMs = float.MaxValue;
            _maxFrameTimeMs = 0f;
            _totalFrameTimeMs = 0f;
            _sampleCount = 0;

            var heapMb = TotalHeapBytes / (1024.0 * 1024.0);
            var allocRateMb = AllocatedBytesDelta / (1024.0 * 1024.0);

            FormattedText = $"Heap: {heapMb:F1}MB | Alloc: {allocRateMb:F2}MB/s | GC: [{Gen0Delta}/{Gen1Delta}/{Gen2Delta}] | Frame: {AvgFrameTimeMs:F1}ms (min: {MinFrameTimeMs:F1}, max: {MaxFrameTimeMs:F1})";
        }
    }
}
