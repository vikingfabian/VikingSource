using System;
using VikingEngine.DebugExtensions;
using Xunit;

namespace VikingEngine.Tests
{
    public class MemoryOverlayTests
    {
        [Fact]
        public void MemoryOverlay_DisplaysMetrics_WithoutExceptions()
        {
            var overlay = new MemoryOverlay();

            overlay.RecordFrame(16.6f);
            overlay.RecordFrame(15.0f);
            overlay.RecordFrame(20.0f);

            overlay.UpdateOneSecond();

            Assert.False(string.IsNullOrEmpty(overlay.FormattedText));
            Assert.Contains("Heap:", overlay.FormattedText);
            Assert.Contains("Alloc:", overlay.FormattedText);
            Assert.Contains("GC:", overlay.FormattedText);
            Assert.Contains("Frame:", overlay.FormattedText);

            Assert.True(overlay.TotalHeapBytes > 0);
            Assert.InRange(overlay.AvgFrameTimeMs, 15.0f, 20.0f);
            Assert.Equal(15.0f, overlay.MinFrameTimeMs);
            Assert.Equal(20.0f, overlay.MaxFrameTimeMs);
        }
    }
}
