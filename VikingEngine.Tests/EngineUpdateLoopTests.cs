using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using Xunit;

namespace VikingEngine.Tests
{
    public class EngineUpdateLoopTests
    {
        [Fact]
        public void RenderTargetDrawContainer_ContractVerification()
        {
            var containerType = typeof(RenderTargetDrawContainer);

            Assert.False(typeof(IUpdateable).IsAssignableFrom(containerType), "RenderTargetDrawContainer must NOT implement IUpdateable");
            Assert.True(typeof(IRenderTargetContainer).IsAssignableFrom(containerType), "RenderTargetDrawContainer must implement IRenderTargetContainer");
            Assert.True(typeof(IDrawContainer).IsAssignableFrom(containerType), "RenderTargetDrawContainer must implement IDrawContainer");
        }

        [Fact]
        public void RenderTargetDraw3dContainer_ContractVerification()
        {
            var container3dType = typeof(RenderTargetDraw3dContainer);

            Assert.True(typeof(IRenderTargetContainer).IsAssignableFrom(container3dType), "RenderTargetDraw3dContainer must implement IRenderTargetContainer");
            Assert.True(typeof(IDrawContainer).IsAssignableFrom(container3dType), "RenderTargetDraw3dContainer must implement IDrawContainer");
        }

        [Fact]
        public void RenderTargetDrawContainer_HasDirtyAndAlwaysRedrawProperties()
        {
            var containerType = typeof(RenderTargetDrawContainer);

            var isDirtyField = containerType.GetField("isDirty");
            var alwaysRedrawField = containerType.GetField("alwaysRedraw");

            Assert.NotNull(isDirtyField);
            Assert.NotNull(alwaysRedrawField);
            Assert.Equal(typeof(bool), isDirtyField.FieldType);
            Assert.Equal(typeof(bool), alwaysRedrawField.FieldType);
        }

        [Theory]
        [InlineData(30)]
        [InlineData(60)]
        [InlineData(75)]
        [InlineData(100)]
        [InlineData(120)]
        [InlineData(144)]
        [InlineData(165)]
        [InlineData(240)]
        [InlineData(360)]
        public void SetFrameRate_SetsTargetDeltaTimeConsistently(int fps)
        {
            Engine.Update.SetFrameRate(fps);

            float expectedMs = 1000f / fps;
            Assert.InRange(Ref.TargetDeltaTimeMs, expectedMs - 0.1f, expectedMs + 0.1f);
            Assert.Equal(fps / 30, Ref.UpdateTimes30FPS);
            Assert.Equal(fps / 60f, Ref.UpdateTimes60FPS);
        }

        [Fact]
        public void FrameRateAssignmentOrder_NeverViolatesMonoGameInvariant()
        {
            // Verify simulated TargetElapsedTime / MaxElapsedTime property setters
            // never throw ArgumentOutOfRangeException across all FPS transitions
            int[] testPresets = { 30, 60, 75, 100, 120, 144, 165, 240, 360 };

            TimeSpan currentTarget = TimeSpan.FromTicks(166667); // 60 FPS
            TimeSpan currentMax = TimeSpan.FromTicks(Math.Max(TimeSpan.FromMilliseconds(500).Ticks, currentTarget.Ticks * 4));

            foreach (int fromFps in testPresets)
            {
                foreach (int toFps in testPresets)
                {
                    TimeSpan newTarget = new TimeSpan((long)(TimeSpan.TicksPerMillisecond * (1000.0 / toFps)));
                    TimeSpan newMax = TimeSpan.FromTicks(Math.Max(TimeSpan.FromMilliseconds(500).Ticks, newTarget.Ticks * 4));

                    // Execute the exact assignment logic used in Engine.Update.SetFrameRate
                    if (newTarget > currentMax)
                    {
                        // Set Max first, then Target
                        currentMax = newMax;
                        Assert.True(currentMax >= currentTarget, "Intermediate state violated: max < currentTarget");

                        currentTarget = newTarget;
                        Assert.True(currentTarget <= currentMax, "Final state violated: target > max");
                    }
                    else
                    {
                        // Set Target first, then Max
                        currentTarget = newTarget;
                        Assert.True(currentTarget <= currentMax, "Intermediate state violated: newTarget > currentMax");

                        currentMax = newMax;
                        Assert.True(currentMax >= currentTarget, "Final state violated: max < target");
                    }

                    Assert.True(currentTarget <= currentMax);
                    Assert.Equal(Math.Max(TimeSpan.FromMilliseconds(500).Ticks, newTarget.Ticks * 4), currentMax.Ticks);
                    Assert.True(currentMax.TotalMilliseconds >= 500.0);
                }
            }
        }

        [Fact]
        public void TimeUpdate_ResetsAndIncrements16msTimersCorrectly()
        {
            var update = new Update(null);
            bool originalPaused = Ref.isPaused;
            float originalDelta = Ref.DeltaTimeMs;
            float originalSpeed = Ref.GameTimeSpeed;

            try
            {
                Ref.isPaused = false;
                Ref.GameTimeSpeed = 1f;

                // 1. Initial tick with delta exceeding Time16ms (~33.33ms)
                Ref.DeltaTimeMs = 40f;
                update.Time_Update(40f);

                Assert.Equal(1, Ref.GameTimePassed16ms);
                Assert.Equal(1, Ref.TimePassed16ms);

                // 2. Subsequent tick with small delta: both timers should reset to 0
                Ref.DeltaTimeMs = 5f;
                update.Time_Update(5f);

                Assert.Equal(0, Ref.GameTimePassed16ms);
                Assert.Equal(0, Ref.TimePassed16ms);

                // 3. Paused state: TimePassed16ms increments with real time, GameTimePassed16ms stays 0
                Ref.isPaused = true;
                Ref.DeltaTimeMs = 40f;
                update.Time_Update(40f);

                Assert.Equal(0, Ref.GameTimePassed16ms);
                Assert.Equal(1, Ref.TimePassed16ms);

                // 4. Tick with 0 delta: both reset to 0
                Ref.DeltaTimeMs = 0f;
                update.Time_Update(0f);

                Assert.Equal(0, Ref.GameTimePassed16ms);
                Assert.Equal(0, Ref.TimePassed16ms);
            }
            finally
            {
                Ref.isPaused = originalPaused;
                Ref.DeltaTimeMs = originalDelta;
                Ref.GameTimeSpeed = originalSpeed;
            }
        }

        [Fact]
        public void Update_DumpUpdateListSummary_CountsTypesCorrectly()
        {
            var update = new Update(null);
            var dummy1 = new DummyUpdateableA();
            var dummy2 = new DummyUpdateableA();
            var dummy3 = new DummyUpdateableB();

            update.AddToOrRemoveFromUpdate(dummy1, true);
            update.AddToOrRemoveFromUpdate(dummy2, true);
            update.AddToOrRemoveFromUpdate(dummy3, true);

            string summary = update.DumpUpdateListSummary(UpdateType.Full);

            Assert.Contains("DummyUpdateableA: 2", summary);
            Assert.Contains("DummyUpdateableB: 1", summary);
            Assert.Contains("Total Items: 3", summary);
        }
    }

    internal class DummyUpdateableA : IUpdateable
    {
        public int SpottedArrayMemberIndex { get; set; } = -1;
        public bool SpottedArrayUseIndex => true;
        public UpdateType UpdateType => UpdateType.Full;
        public bool RunDuringPause => false;
        public void Time_Update(float time_ms) { }
    }

    internal class DummyUpdateableB : IUpdateable
    {
        public int SpottedArrayMemberIndex { get; set; } = -1;
        public bool SpottedArrayUseIndex => true;
        public UpdateType UpdateType => UpdateType.Full;
        public bool RunDuringPause => false;
        public void Time_Update(float time_ms) { }
    }
}
