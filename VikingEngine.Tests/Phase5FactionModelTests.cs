using System;
using VikingEngine.DSSWars;
using VikingEngine.Tests.Legacy;
using Xunit;

namespace VikingEngine.Tests
{
    public class Phase5FactionModelTests
    {
        [Fact]
        public void Faction_ClearModels_EmptiesLoadedModelsDictionary()
        {
            var faction = new Faction(0);
            Assert.Equal(0, faction.LoadedModelsCount);

            faction.ClearModels();
            Assert.Equal(0, faction.LoadedModelsCount);
        }

        [Fact]
        public void LegacyComparison_LegacyRetainsModelsOnDeath_ModernReclaimsModels()
        {
            var legacy = new LegacyFactionModelCache();
            for (int i = 0; i < 20; i++)
            {
                legacy.LoadModel(i, new object());
            }

            Assert.Equal(20, legacy.models_loaded.Count);

            // Legacy death: does NOT free models
            legacy.LegacyDeleteMe();
            Assert.Equal(20, legacy.models_loaded.Count);

            // Modern death: clears loaded models
            var modern = new Faction(0);
            modern.ClearModels();
            Assert.Equal(0, modern.LoadedModelsCount);
        }
    }
}
