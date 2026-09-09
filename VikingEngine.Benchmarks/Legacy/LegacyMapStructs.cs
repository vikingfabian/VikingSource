using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.Benchmarks.Legacy
{
    public struct LegacySubTile
    {
        public static readonly LegacySubTile Empty = new LegacySubTile() { mainTerrain = TerrainMainType.NUM };

        public Color color;
        public float groundY;
        internal TerrainMainType mainTerrain = TerrainMainType.NUM;
        public int subTerrain = byte.MaxValue;
        public int terrainAmount = 0;
        public int terrainQuality = 0;
        public int collectionPointer = -1;

        public LegacySubTile()
        {
        }
    }

    public struct LegacyTile
    {
        public const int NoBorderRegion = -2;
        public const int SeaBorder = -1;

        public int CityIndex;
        internal BiomType biom = BiomType.Green;
        public float secondaryBiomStrength = 0;
        internal BiomType secondaryBiom = BiomType.Green;
        public int heightLevel;
        internal TileContent tileContent = TileContent.NONE;
        public int BorderCount;
        public int BorderRegion_North, BorderRegion_East, BorderRegion_South, BorderRegion_West;
        public int seaDistanceHeatMap = int.MinValue;
        public float exitRenderTimeStamp_TotSec = 0;
        public byte bits_renderStateA = 0;
        public byte bits_renderStateB = 0;
        public bool hasTileInRender = false;
        public int subtileVisualEdits = 0;

        public LegacyTile()
        {
            CityIndex = -1;
            heightLevel = Height.DeepWaterHeight;
        }
    }
}
