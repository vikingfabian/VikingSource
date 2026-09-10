using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.Tests.Legacy;
using Xunit;

namespace VikingEngine.Tests
{
    public class Phase1StructTests
    {
        [Fact]
        public void SubTile_PackedSize_Is16Bytes()
        {
            int size = Marshal.SizeOf<MapTile1_1>();
            Assert.Equal(16, size);
        }

        [Fact]
        public void Tile_PackedSize_Is34Bytes()
        {
            int size = Marshal.SizeOf<SumTile4_4>();
            Assert.Equal(34, size);
        }

        [Fact]
        public void SubTile_SizeReduction_Saves12BytesPerInstance()
        {
            int legacySize = Marshal.SizeOf<LegacySubTile>();
            int newSize = Marshal.SizeOf<MapTile1_1>();

            Assert.Equal(28, legacySize);
            Assert.Equal(16, newSize);
            Assert.Equal(12, legacySize - newSize);
        }

        [Fact]
        public void Tile_SizeReduction_Saves30BytesPerInstance()
        {
            int legacySize = Marshal.SizeOf<LegacyTile>();
            int newSize = Marshal.SizeOf<SumTile4_4>();

            Assert.Equal(64, legacySize);
            Assert.Equal(34, newSize);
            Assert.Equal(30, legacySize - newSize);
        }

        [Fact]
        public void Tile_SentinelsAndDefaults_ArePreserved()
        {
            var tile = new SumTile4_4();

            Assert.Equal((short)(-1), tile.CityIndex);
            Assert.Equal(SumTile4_4.NoBorderRegion, tile.BorderRegion_North);
            Assert.Equal(SumTile4_4.NoBorderRegion, tile.BorderRegion_East);
            Assert.Equal(SumTile4_4.NoBorderRegion, tile.BorderRegion_South);
            Assert.Equal(SumTile4_4.NoBorderRegion, tile.BorderRegion_West);
            Assert.Equal(TileContent.NONE, tile.tileContent);
            Assert.Equal(ColorHeight.DeepWaterHeight, tile.heightLevel);
        }

        [Fact]
        public void SubTile_Serialization_RoundTrip_PreservesData()
        {
            var original = new SubTile(TerrainMainType.Building, 3, Color.Red, 12.5f);
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
            Assert.Equal(original.groundY, loaded.groundY);
            Assert.Equal(original.color, loaded.color);
        }

        [Fact]
        public void Tile_Serialization_RoundTrip_Version12_PreservesData()
        {
            var original = new SumTile4_4();
            original.CityIndex = 42;
            original.biom = BiomType.Frozen;
            original.heightLevel = 5;
            original.tileContent = TileContent.City;
            original.BorderRegion_North = 10;
            original.BorderRegion_East = SumTile4_4.SeaBorder;
            original.BorderRegion_South = SumTile4_4.NoBorderRegion;
            original.BorderRegion_West = 3;

            byte[] data;
            using (var ms = new MemoryStream())
            using (var w = new BinaryWriter(ms))
            {
                var prev = new SumTile4_4();
                original.writeMapFile(w, prev);
                data = ms.ToArray();
            }

            SumTile4_4 loaded = new SumTile4_4();
            using (var ms = new MemoryStream(data))
            using (var r = new BinaryReader(ms))
            {
                var prev = new SumTile4_4();
                loaded.readMapFile(r, prev, 12);
            }

            Assert.Equal((short)42, loaded.CityIndex);
            Assert.Equal(BiomType.Frozen, loaded.biom);
            Assert.Equal((byte)5, loaded.heightLevel);
            Assert.Equal(TileContent.City, loaded.tileContent);
            Assert.Equal((short)10, loaded.BorderRegion_North);
            Assert.Equal(SumTile4_4.SeaBorder, loaded.BorderRegion_East);
            Assert.Equal(SumTile4_4.NoBorderRegion, loaded.BorderRegion_South);
            Assert.Equal((short)3, loaded.BorderRegion_West);
        }

        [Fact]
        public void Tile_Serialization_BackwardCompatibility_Version11()
        {
            byte[] v11Data;
            using (var ms = new MemoryStream())
            using (var w = new BinaryWriter(ms))
            {
                EightBit saveOpt = new EightBit();
                saveOpt.Set(0, true);  // IsCity
                saveOpt.Set(1, false); // eqCityIndex = false (write city index)
                saveOpt.Set(2, false); // eqBiom = false
                saveOpt.Set(3, false); // eqHeight = false
                saveOpt.Set(4, false); // HasBorderN
                saveOpt.Set(5, false); // HasBorderE
                saveOpt.Set(6, false); // HasBorderS
                saveOpt.Set(7, false); // HasBorderW
                saveOpt.write(w);

                w.Write((ushort)150);       // CityIndex as ushort in v11
                w.Write((byte)BiomType.Tundra); // Biom
                w.Write((byte)7);           // HeightLevel
                v11Data = ms.ToArray();
            }

            SumTile4_4 loaded = new SumTile4_4();
            using (var ms = new MemoryStream(v11Data))
            using (var r = new BinaryReader(ms))
            {
                var prev = new SumTile4_4();
                loaded.readMapFile(r, prev, 11);
            }

            Assert.Equal((short)150, loaded.CityIndex);
            Assert.Equal(BiomType.Tundra, loaded.biom);
            Assert.Equal((byte)7, loaded.heightLevel);
            Assert.Equal(TileContent.City, loaded.tileContent);
        }
    }
}
