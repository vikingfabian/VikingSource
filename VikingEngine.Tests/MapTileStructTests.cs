using System;
using System.IO;
using System.Runtime.InteropServices;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.Tests.Legacy;
using Xunit;

namespace VikingEngine.Tests
{
    public class MapTileStructTests
    {
        [Fact]
        public void SubTile_PackedSize_Is11Bytes()
        {
            int size = Marshal.SizeOf<MapTile1_1>();
            Assert.Equal(11, size);
        }

        [Fact]
        public void Tile_PackedSize_Is23Bytes()
        {
            int size = Marshal.SizeOf<SumTile4_4>();
            Assert.Equal(23, size);
        }

        [Fact]
        public void SubTile_SizeReduction_Saves17BytesPerInstance()
        {
            int legacySize = Marshal.SizeOf<LegacySubTile>();
            int newSize = Marshal.SizeOf<MapTile1_1>();

            Assert.Equal(28, legacySize);
            Assert.Equal(11, newSize);
            Assert.Equal(17, legacySize - newSize);
        }

        [Fact]
        public void Tile_SizeReduction_Saves41BytesPerInstance()
        {
            int legacySize = Marshal.SizeOf<LegacyTile>();
            int newSize = Marshal.SizeOf<SumTile4_4>();

            Assert.Equal(64, legacySize);
            Assert.Equal(23, newSize);
            Assert.Equal(41, legacySize - newSize);
        }

        [Fact]
        public void Tile_SentinelsAndDefaults_ArePreserved()
        {
            var tile = new SumTile4_4();

            Assert.Equal(PCity.Empty, tile.pcity);
            Assert.Equal(SumTile4_4.NoBorderRegion, tile.BorderRegion_North);
            Assert.Equal(SumTile4_4.NoBorderRegion, tile.BorderRegion_East);
            Assert.Equal(SumTile4_4.NoBorderRegion, tile.BorderRegion_South);
            Assert.Equal(SumTile4_4.NoBorderRegion, tile.BorderRegion_West);
            Assert.Equal(BiomType.Green, tile.biom1);
            Assert.Equal(int.MinValue, tile.seaDistanceHeatMap);
        }

        [Fact]
        public void SubTile_Serialization_RoundTrip_PreservesData()
        {
            var original = new MapTile1_1(TerrainMainType.Building, 3, 12);
            original.heightValue = 12;
            original.terrainAmount = 4;
            original.collectionPointer = 100;

            byte[] data;
            using (var ms = new MemoryStream())
            using (var w = new BinaryWriter(ms))
            {
                var prev = new MapTile1_1();
                original.write(w, ref prev);
                data = ms.ToArray();
            }

            MapTile1_1 loaded = new MapTile1_1();
            using (var ms = new MemoryStream(data))
            using (var r = new BinaryReader(ms))
            {
                var prev = new MapTile1_1();
                loaded.read(r, ref prev, 12);
            }

            Assert.Equal(original.mainTerrain, loaded.mainTerrain);
            Assert.Equal(original.subTerrain, loaded.subTerrain);
            Assert.Equal(original.terrainAmount, loaded.terrainAmount);
            Assert.Equal(original.collectionPointer, loaded.collectionPointer);
            Assert.Equal(original.heightValue, loaded.heightValue);
            Assert.Equal(original.groundY, loaded.groundY);
        }
    }
}
