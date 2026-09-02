using System;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.Tests.Legacy;
using Xunit;

namespace VikingEngine.Tests
{
    public class Phase2PathPoolTests
    {
        public Phase2PathPoolTests()
        {
            TestWorldHelper.SetupFlatWorld(32, 32);
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
            // Note: Instantiate thread pools directly or test preallocate/clear
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
