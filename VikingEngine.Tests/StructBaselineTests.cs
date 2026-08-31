using System;
using System.Runtime.InteropServices;
using VikingEngine.DSSWars.Map;
using VikingEngine.Tests.Legacy;
using Xunit;

namespace VikingEngine.Tests
{
    public class StructBaselineTests
    {
        [Fact]
        public void LegacySubTile_CurrentSize_Is28Bytes()
        {
            int size = Marshal.SizeOf<LegacySubTile>();
            Assert.Equal(28, size);
        }

        [Fact]
        public void LegacyTile_CurrentSize_Is64Bytes()
        {
            int size = Marshal.SizeOf<LegacyTile>();
            Assert.Equal(64, size);
        }

        [Fact]
        public void SubTile_MatchesLegacyBaseline()
        {
            Assert.Equal(Marshal.SizeOf<LegacySubTile>(), Marshal.SizeOf<SubTile>());
        }

        [Fact]
        public void Tile_MatchesLegacyBaseline()
        {
            Assert.Equal(Marshal.SizeOf<LegacyTile>(), Marshal.SizeOf<Tile>());
        }
    }
}
