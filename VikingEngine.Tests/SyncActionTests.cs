using System;
using System.Collections.Generic;
using VikingEngine.Engine;
using VikingEngine.Tests.Legacy;
using Xunit;

namespace VikingEngine.Tests
{
    public class SyncActionTests
    {
        [Fact]
        public void SyncAction_ExecutesActionSuccessfully()
        {
            bool executed = false;
            var action = new SyncAction(() => { executed = true; });

            action.runSyncAction();

            Assert.True(executed);
        }

        [Fact]
        public void SyncAction1Arg_ExecutesActionWithArgument()
        {
            int received = 0;
            var action = new SyncAction1Arg<int>(arg => { received = arg; }, 42);

            action.runSyncAction();

            Assert.Equal(42, received);
        }

        [Fact]
        public void SyncAction2Arg_ExecutesActionWithArguments()
        {
            string strResult = "";
            int intResult = 0;
            var action = new SyncAction2Arg<string, int>((s, i) =>
            {
                strResult = s;
                intResult = i;
            }, "Hello", 99);

            action.runSyncAction();

            Assert.Equal("Hello", strResult);
            Assert.Equal(99, intResult);
        }

        [Fact]
        public void LegacyComparison_LegacyBoxesStruct_ModernUsesReferenceDirectly()
        {
            int executedCount = 0;
            Action callback = () => { executedCount++; };

            // Legacy: struct boxed into interface
            var legacyQueue = new LegacySyncActionQueue();
            legacyQueue.AddSyncAction(new LegacyStructSyncAction(callback));
            legacyQueue.ProcessAll();

            Assert.Equal(1, executedCount);

            // Modern: class avoids boxing into interface
            ISyncAction modern = new SyncAction(callback);
            modern.runSyncAction();

            Assert.Equal(2, executedCount);
        }

        [Fact]
        public void BudgetConstants_AreTieredCorrectly()
        {
            Assert.True(Update.IdealSyncActionBudgetMs > 0, "Ideal budget should be positive");
            Assert.True(Update.IdealSyncActionBudgetMs < Update.IncreasedSyncActionBudgetMs, "Increased budget should exceed ideal budget");
            Assert.True(Update.IncreasedSyncActionBudgetMs < Update.MaxSyncActionBudgetMs, "Max budget should exceed increased budget");
        }

        [Fact]
        public void Update_SyncQueThrottling_RespectsIdealBudget()
        {
            var update = new Update(null);

            int actionsExecuted = 0;
            for (int i = 0; i < 4; i++)
            {
                update.AddSyncAction(new SyncAction(() =>
                {
                    System.Threading.Thread.Sleep(5);
                    actionsExecuted++;
                }));
            }

            Assert.Equal(4, update.SyncQueCount);

            // With 4 actions (<= 50), budget is IdealSyncActionBudgetMs (2.0ms).
            // Since each action takes ~5ms, throttling must stop after the first action.
            update.Time_Update(16.0f);

            Assert.True(actionsExecuted < 4, $"Expected throttling to leave items in queue, but executed {actionsExecuted}");
            Assert.True(actionsExecuted >= 1, $"Expected at least 1 action to execute, but executed {actionsExecuted}");
            Assert.Equal(4 - actionsExecuted, update.SyncQueCount);
        }

        [Fact]
        public void Update_DynamicSyncQueBudget_ExpandsUnderBacklog()
        {
            var update = new Update(null);

            // Add 60 actions to trigger the > 50 backlog threshold (IncreasedSyncActionBudgetMs = 6.0ms)
            int executed = 0;
            for (int i = 0; i < 60; i++)
            {
                update.AddSyncAction(new SyncAction(() =>
                {
                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    while (sw.ElapsedTicks < System.Diagnostics.Stopwatch.Frequency / 10000) { } // ~0.1ms spin
                    executed++;
                }));
            }

            update.Time_Update(16.0f);

            // With dynamic backlog budget (6.0ms vs 2.0ms ideal), at least 4 actions execute
            Assert.True(executed >= 4, $"Expected dynamic budget to allow more executions, but executed {executed}");
            Assert.True(executed < 60, "Expected throttling to not drain all 60 actions in one tick");
        }

        [Fact]
        public void Update_DynamicSyncQueBudget_MaxTierUnderHeavyBacklog()
        {
            var update = new Update(null);

            // Add 250 actions to trigger the > 200 heavy backlog threshold (MaxSyncActionBudgetMs = 12.0ms)
            int executed = 0;
            for (int i = 0; i < 250; i++)
            {
                update.AddSyncAction(new SyncAction(() =>
                {
                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    while (sw.ElapsedTicks < System.Diagnostics.Stopwatch.Frequency / 10000) { } // ~0.1ms spin
                    executed++;
                }));
            }

            update.Time_Update(16.0f);

            // With max budget (12.0ms), substantially more actions execute than under ideal budget
            Assert.True(executed >= 8, $"Expected heavy backlog budget to allow more executions, but executed {executed}");
            Assert.True(executed < 250, "Expected throttling to not drain all 250 actions in one tick");
        }
    }
}
