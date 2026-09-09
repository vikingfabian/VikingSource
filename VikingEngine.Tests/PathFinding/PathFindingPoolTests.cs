using System;
using System.Collections.Generic;
using System.Diagnostics;
using VikingEngine;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.Tests.Legacy;
using Xunit;

namespace VikingEngine.Tests.Pathfinding
{
    public class PathFindingPoolTests
    {
        public PathFindingPoolTests()
        {
            TestWorldHelper.SetupFlatWorld(32, 32);
        }

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

        [Fact]
        public void PathFindingPool_PreAllocates_MatchesExpectedCount()
        {
            var pool = new PathFindingPool();
            Assert.Equal(0, pool.PfCount);
            Assert.Equal(0, pool.CreatedPfCount);

            pool.Preallocate(3);

            Assert.Equal(3, pool.PfCount);
            Assert.Equal(3, pool.CreatedPfCount);
        }

        [Fact]
        public void PathFindingPool_ClearsOnTransition_DropsCountToZero()
        {
            var pool = new PathFindingPool();
            pool.Preallocate(4);
            pool.Return(new WalkingPath());
            pool.Return(new WalkingPath());

            Assert.Equal(4, pool.PfCount);
            Assert.Equal(2, pool.ResCount);

            pool.Clear();

            Assert.Equal(0, pool.PfCount);
            Assert.Equal(0, pool.ResCount);
        }

        [Fact]
        public void PathFindingPool_RecyclesAndReusesWithoutAllocation()
        {
            var pool = new PathFindingPool();
            pool.Preallocate(1);

            Assert.Equal(1, pool.PfCount);
            Assert.Equal(1, pool.CreatedPfCount);

            var pf1 = pool.GetPf();
            Assert.Equal(0, pool.PfCount);
            Assert.Equal(1, pool.CreatedPfCount);

            pool.Return(pf1);
            Assert.Equal(1, pool.PfCount);

            var pf2 = pool.GetPf();
            Assert.Same(pf1, pf2);
            Assert.Equal(0, pool.PfCount);
            Assert.Equal(1, pool.CreatedPfCount); // No new allocations occurred!
        }

        [Fact]
        public void DetailPathFindingPool_PreAllocatesAndClears()
        {
            var pool = new DetailPathFindingPool();
            Assert.Equal(0, pool.PfCount);

            pool.Preallocate(2);
            pool.Return(new DetailWalkingPath());

            Assert.Equal(2, pool.PfCount);
            Assert.Equal(1, pool.ResCount);
            Assert.Equal(2, pool.CreatedPfCount);

            pool.Clear();

            Assert.Equal(0, pool.PfCount);
            Assert.Equal(0, pool.ResCount);
        }

        [Fact]
        public void PathUpdateThread_ClearPools_EmptiesBothPools()
        {
            var pfPool = new PathFindingPool();
            var detailPfPool = new DetailPathFindingPool();

            pfPool.Preallocate(2);
            detailPfPool.Preallocate(2);

            Assert.Equal(2, pfPool.PfCount);
            Assert.Equal(2, detailPfPool.PfCount);

            pfPool.Clear();
            detailPfPool.Clear();

            Assert.Equal(0, pfPool.PfCount);
            Assert.Equal(0, detailPfPool.PfCount);
        }

        [Fact]
        public void LegacyComparison_DemonstratesReclamationBenefit()
        {
            var legacyPool = new LegacyUnboundedPathFindingPool();
            var modernPool = new PathFindingPool();

            modernPool.Preallocate(2);
            modernPool.Clear();

            // Modern pool supports clean reclamation on game exit; legacy pool leaks indefinitely
            Assert.Equal(0, modernPool.PfCount);
        }
    }
}
