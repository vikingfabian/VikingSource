using System;
using System.Collections.Generic;
using System.Diagnostics;
using VikingEngine;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Path;
using Xunit;

namespace VikingEngine.Tests.Pathfinding
{
    public class PathFindingPoolTests
    {
        [Fact]
        public void WalkingPath_Recycle_TrimsExcessCapacityWhenOversized()
        {
            var path = new WalkingPath();
            // Artificially grow capacity beyond 512
            for (int i = 0; i < 600; i++)
            {
                path.nodes.Add(new PathNodeResult(new IntVector2(i, i), false));
            }

            Assert.True(path.nodes.Capacity > 512);

            path.recycle();

            Assert.Empty(path.nodes);
            Assert.Equal(256, path.nodes.Capacity);
        }

        [Fact]
        public void WalkingPath_Recycle_RetainsCapacityWhenUnderThreshold()
        {
            var path = new WalkingPath();
            for (int i = 0; i < 100; i++)
            {
                path.nodes.Add(new PathNodeResult(new IntVector2(i, i), false));
            }

            int initialCap = path.nodes.Capacity;
            Assert.True(initialCap <= 512);

            path.recycle();

            Assert.Empty(path.nodes);
            Assert.Equal(initialCap, path.nodes.Capacity);
        }

        [Fact]
        public void DetailWalkingPath_Recycle_TrimsExcessCapacityWhenOversized()
        {
            var path = new DetailWalkingPath();
            for (int i = 0; i < 600; i++)
            {
                path.nodes.Add(new DetailPathNodeResult(new IntVector2(i, i), false));
            }

            Assert.True(path.nodes.Capacity > 512);

            path.recycle();

            Assert.Empty(path.nodes);
            Assert.Equal(256, path.nodes.Capacity);
        }

        [Fact]
        public void PathFindingPool_RapidReuse_DoesNotSleepOrLeak()
        {
            var pool = new PathFindingPool();
            var stopwatch = Stopwatch.StartNew();

            // Simulate high-throughput reuse loop (1000 requests in quick succession)
            for (int i = 0; i < 1000; i++)
            {
                var path = pool.GetRes();
                path.nodes.Add(new PathNodeResult(new IntVector2(i, i), false));
                pool.Return(path);
            }

            stopwatch.Stop();

            // 1000 iterations without Thread.Sleep(32) should complete in under 50ms.
            // With the old bug (32ms sleep per trigger), 1000 iterations would take over 30 seconds.
            Assert.True(stopwatch.ElapsedMilliseconds < 500, $"Expected rapid pool reuse to be under 500ms, but took {stopwatch.ElapsedMilliseconds}ms");

            // Getting an item now should return the clean recycled path
            var finalPath = pool.GetRes();
            Assert.Empty(finalPath.nodes);
        }

        [Fact]
        public void DetailPathFindingPool_RapidReuse_DoesNotSleepOrLeak()
        {
            var pool = new DetailPathFindingPool();
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < 1000; i++)
            {
                var path = pool.GetRes();
                path.nodes.Add(new DetailPathNodeResult(new IntVector2(i, i), false));
                pool.Return(path);
            }

            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds < 500, $"Expected rapid detail pool reuse to be under 500ms, but took {stopwatch.ElapsedMilliseconds}ms");

            var finalPath = pool.GetRes();
            Assert.Empty(finalPath.nodes);
        }
    }
}
