using System;
using System.Collections.Concurrent;

namespace VikingEngine.Benchmarks.Legacy
{
    public interface ILegacySyncAction
    {
        void runSyncAction();
    }

    /// <summary>
    /// Snapshot of the legacy struct-based SyncAction before Phase 6.
    /// In the legacy implementation, SyncAction was a struct implementing interface ISyncAction,
    /// which caused boxing allocations on every Push to ConcurrentStack<ISyncAction>.
    /// </summary>
    public struct LegacyStructSyncAction : ILegacySyncAction
    {
        public Action action;

        public LegacyStructSyncAction(Action action)
        {
            this.action = action;
        }

        public void runSyncAction()
        {
            action();
        }
    }

    public class LegacySyncActionQueue
    {
        public ConcurrentStack<ILegacySyncAction> syncQue = new ConcurrentStack<ILegacySyncAction>();

        public void AddSyncAction(ILegacySyncAction action)
        {
            syncQue.Push(action);
        }

        public void ProcessAll()
        {
            while (syncQue.TryPop(out var action))
            {
                action.runSyncAction();
            }
        }
    }
}
