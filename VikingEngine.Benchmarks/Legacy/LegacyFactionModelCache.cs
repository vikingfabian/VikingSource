using System;
using System.Collections.Concurrent;

namespace VikingEngine.Benchmarks.Legacy
{
    /// <summary>
    /// Snapshot of the legacy Faction model cache before Phase 5.
    /// In the legacy implementation, models_loaded grew indefinitely and was never cleared on faction death or game exit.
    /// </summary>
    class LegacyFactionModelCache
    {
        public ConcurrentDictionary<int, object> models_loaded = new ConcurrentDictionary<int, object>();

        public void LoadModel(int id, object model)
        {
            models_loaded.TryAdd(id, model);
        }

        // Legacy DeleteMe: did NOT clear models_loaded
        public void LegacyDeleteMe()
        {
            // isAlive = false;
            // No models_loaded.Clear()
        }
    }
}
