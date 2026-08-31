using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
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
            int size = Marshal.SizeOf<SubTile>();
            Assert.Equal(16, size);
        }

        [Fact]
        public void Tile_PackedSize_Is34Bytes()
        {
            int size = Marshal.SizeOf<Tile>();
            Assert.Equal(34, size);
        }

        [Fact]
        public void SubTile_SizeReduction_Saves12BytesPerInstance()
        {
            int legacySize = Marshal.SizeOf<LegacySubTile>();
            int newSize = Marshal.SizeOf<SubTile>();

            Assert.Equal(28, legacySize);
            Assert.Equal(16, newSize);
            Assert.Equal(12, legacySize - newSize);
        }

        [Fact]
        public void Tile_SizeReduction_Saves30BytesPerInstance()
        {
            int legacySize = Marshal.SizeOf<LegacyTile>();
            int newSize = Marshal.SizeOf<Tile>();

            Assert.Equal(64, legacySize);
            Assert.Equal(34, newSize);
            Assert.Equal(30, legacySize - newSize);
        }

        [Fact]
        public void Tile_SentinelsAndDefaults_ArePreserved()
        {
            var tile = new Tile();

            Assert.Equal((short)(-1), tile.CityIndex);
            Assert.Equal(Tile.NoBorderRegion, tile.BorderRegion_North);
            Assert.Equal(Tile.NoBorderRegion, tile.BorderRegion_East);
            Assert.Equal(Tile.NoBorderRegion, tile.BorderRegion_South);
            Assert.Equal(Tile.NoBorderRegion, tile.BorderRegion_West);
            Assert.Equal(TileContent.NONE, tile.tileContent);
            Assert.Equal(Height.DeepWaterHeight, tile.heightLevel);
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
                var prev = new SubTile();
                original.write(w, ref prev);
                data = ms.ToArray();
            }

            SubTile loaded = new SubTile();
            using (var ms = new MemoryStream(data))
            using (var r = new BinaryReader(ms))
            {
                var prev = new SubTile();
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
            var original = new Tile();
            original.CityIndex = 42;
            original.biom = BiomType.Frozen;
            original.heightLevel = 5;
            original.tileContent = TileContent.City;
            original.BorderRegion_North = 10;
            original.BorderRegion_East = Tile.SeaBorder;
            original.BorderRegion_South = Tile.NoBorderRegion;
            original.BorderRegion_West = 3;

            byte[] data;
            using (var ms = new MemoryStream())
            using (var w = new BinaryWriter(ms))
            {
                var prev = new Tile();
                original.writeMapFile(w, prev);
                data = ms.ToArray();
            }

            Tile loaded = new Tile();
            using (var ms = new MemoryStream(data))
            using (var r = new BinaryReader(ms))
            {
                var prev = new Tile();
                loaded.readMapFile(r, prev, 12);
            }

            Assert.Equal((short)42, loaded.CityIndex);
            Assert.Equal(BiomType.Frozen, loaded.biom);
            Assert.Equal((byte)5, loaded.heightLevel);
            Assert.Equal(TileContent.City, loaded.tileContent);
            Assert.Equal((short)10, loaded.BorderRegion_North);
            Assert.Equal(Tile.SeaBorder, loaded.BorderRegion_East);
            Assert.Equal(Tile.NoBorderRegion, loaded.BorderRegion_South);
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

            Tile loaded = new Tile();
            using (var ms = new MemoryStream(v11Data))
            using (var r = new BinaryReader(ms))
            {
                var prev = new Tile();
                loaded.readMapFile(r, prev, 11);
            }

            Assert.Equal((short)150, loaded.CityIndex);
            Assert.Equal(BiomType.Tundra, loaded.biom);
            Assert.Equal((byte)7, loaded.heightLevel);
            Assert.Equal(TileContent.City, loaded.tileContent);
        }
    }
}
