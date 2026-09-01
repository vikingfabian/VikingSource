using System;
using System.Collections.Generic;
using VikingEngine.Tests.Legacy;
using Xunit;

namespace VikingEngine.Tests
{
    public class Phase6SyncActionTests
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
    }
}
