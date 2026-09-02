using System;
using VikingEngine.DSSWars.Battle;
using VikingEngine.Tests.Legacy;
using Xunit;

namespace VikingEngine.Tests
{
    public class Phase3BattlePoolTests
    {
        public Phase3BattlePoolTests()
        {
            SoldierBattleData.ClearPool();
        }

        [Fact]
        public void SoldierBattleData_Rent_CreatesOrPopsInstance()
        {
            SoldierBattleData.ClearPool();
            Assert.Equal(0, SoldierBattleData.PoolCount);

            var item1 = new SoldierBattleData();
            SoldierBattleData.Return(item1);
            Assert.Equal(1, SoldierBattleData.PoolCount);

            var item2 = SoldierBattleData.Rent(null!);
            Assert.Same(item1, item2);
            Assert.Equal(0, SoldierBattleData.PoolCount);
        }

        [Fact]
        public void SoldierBattleData_Return_ResetsFieldsAndRetainsCapacity()
        {
            var data = new SoldierBattleData();
            data.queueTime = 500f;

            SoldierBattleData.Return(data);

            Assert.Equal(0, data.queueTime);
            Assert.Equal(1, SoldierBattleData.PoolCount);
        }

        [Fact]
        public void SoldierBattleData_ClearPool_EmptiesAllRetainedInstances()
        {
            for (int i = 0; i < 5; i++)
            {
                SoldierBattleData.Return(new SoldierBattleData());
            }

            Assert.Equal(5, SoldierBattleData.PoolCount);

            SoldierBattleData.ClearPool();

            Assert.Equal(0, SoldierBattleData.PoolCount);
        }

        [Fact]
        public void LegacyComparison_UnpooledAllocatesNewHeapObjects()
        {
            // Legacy model: every battle entry creates new instance + new List
            var legacy1 = new LegacySoldierBattleData(5);
            var legacy2 = new LegacySoldierBattleData(5);

            Assert.NotSame(legacy1, legacy2);

            // Modern model: recycled through pool
            SoldierBattleData.ClearPool();
            var modern1 = new SoldierBattleData();
            SoldierBattleData.Return(modern1);

            var modern2 = SoldierBattleData.Rent(null!);
            Assert.Same(modern1, modern2);
        }
    }
}
