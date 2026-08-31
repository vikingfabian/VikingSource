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
        public void SubTile_IsSmallerThanLegacyBaseline()
        {
            Assert.True(Marshal.SizeOf<SubTile>() < Marshal.SizeOf<LegacySubTile>());
            Assert.Equal(16, Marshal.SizeOf<SubTile>());
        }

        [Fact]
        public void Tile_IsSmallerThanLegacyBaseline()
        {
            Assert.True(Marshal.SizeOf<Tile>() < Marshal.SizeOf<LegacyTile>());
            Assert.Equal(34, Marshal.SizeOf<Tile>());
        }
    }
}
