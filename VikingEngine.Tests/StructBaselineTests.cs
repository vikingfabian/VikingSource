using System;
using System.Runtime.InteropServices;
using VikingEngine.DSSWars.Map.MapData;
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
            Assert.True(Marshal.SizeOf<MapTile1_1>() < Marshal.SizeOf<LegacySubTile>());
            Assert.Equal(11, Marshal.SizeOf<MapTile1_1>());
        }

        [Fact]
        public void Tile_IsSmallerThanLegacyBaseline()
        {
            Assert.True(Marshal.SizeOf<SumTile4_4>() < Marshal.SizeOf<LegacyTile>());
            Assert.Equal(23, Marshal.SizeOf<SumTile4_4>());
        }
    }
}
