using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    struct SquareVisualProperties
    {
        public byte variant;
        
        public void write(System.IO.BinaryWriter w)
        {
            w.Write(variant);
        }

        public void read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            variant = r.ReadByte();            
        }
    }

    class VisualTerrainData
    {
        static VisualTerrainData[] typeData;
        public static void Init()
        {
            typeData = new VisualTerrainData[(int)SquareType.NUM_NON];

            typeData[(int)SquareType.Grass] = new VisualTerrainData(new AbsTileModelData[] {
                new GroundTileModelData(SpriteName.cmdTileGrass1),
                new GroundTileModelData(SpriteName.cmdTileGrass2),
                new GroundTileModelData(SpriteName.cmdTileGrass3),
                new GroundTileModelData(SpriteName.cmdTileGrass4)},  
                TileTextureVariantType.Random,
                false, MainTerrainType.Ground, SquareType.NUM_NON);

            typeData[(int)SquareType.GreenRoad] = new VisualTerrainData(new GroundTileModelData(SpriteName.cmdTilePavedRoad), 
                true, MainTerrainType.PavedRoad, SquareType.NUM_NON);

            typeData[(int)SquareType.GreenHill] = new VisualTerrainData(new GroundTileModelData(SpriteName.cmdTileHill), 
                false, MainTerrainType.Hill, SquareType.NUM_NON);

            typeData[(int)SquareType.GreenForest] = new VisualTerrainData(new GroundTileModelData(SpriteName.cmdTileForest), 
                false, MainTerrainType.Forest, SquareType.NUM_NON);

            typeData[(int)SquareType.GreenMountain] = new VisualTerrainData(new GroundTileModelData(SpriteName.cmdTileMountain), 
                false, MainTerrainType.Mountain, SquareType.NUM_NON);

            typeData[(int)SquareType.GreenTown] = new VisualTerrainData(new GroundTileModelData(SpriteName.cmdTileTown), 
                false, MainTerrainType.cmdTown, SquareType.Grass);

            typeData[(int)SquareType.GreenSwamp] = new VisualTerrainData(new GroundTileModelData(SpriteName.cmdTileSwamp), 
                false, MainTerrainType.Swamp, SquareType.NUM_NON);

            typeData[(int)SquareType.GreenWaterPuddle] = new VisualTerrainData(new GroundTileModelData(SpriteName.cmdTileWater), 
                false, MainTerrainType.Water, SquareType.NUM_NON);

            typeData[(int)SquareType.GreenRubble] = new VisualTerrainData(new GroundTileModelData(SpriteName.cmdTileRubble), 
                false, MainTerrainType.cmdRubble, SquareType.NUM_NON);

            typeData[(int)SquareType.OpenWater] = new VisualTerrainData(new GroundTileModelData(SpriteName.cmdTileOpenWater), 
                false, MainTerrainType.Water, SquareType.NUM_NON);

            typeData[(int)SquareType.GreenPalisad] = new VisualTerrainData(new GroundTileModelData(SpriteName.cmdTilePalisad), 
                false, MainTerrainType.cmdPalisad, SquareType.Grass);

            typeData[(int)SquareType.StoneTower] = new VisualTerrainData(new GroundTileModelData(SpriteName.cmdTileCastle), 
                true, MainTerrainType.Tower, SquareType.Grass);

            typeData[(int)SquareType.GreenStoneWall] = new VisualTerrainData(new GroundTileModelData(SpriteName.cmdTileStoneWall), 
                false, MainTerrainType.Wall, SquareType.Grass);

            typeData[(int)SquareType.GreenStoneGate] = new VisualTerrainData(new GroundTileModelData(SpriteName.cmdTileStoneGate), 
                false, MainTerrainType.Gate, SquareType.Grass);

            typeData[(int)SquareType.MountainGround] = new VisualTerrainData(new GroundTileModelData(SpriteName.cmdTileDungeonGround), 
                false, MainTerrainType.Ground, SquareType.NUM_NON);

            typeData[(int)SquareType.MountainWall] = new VisualTerrainData(new WallTileModelData(SpriteName.cmdTileDungeonWall, SpriteName.cmdTileDungeonWall), 
                false, MainTerrainType.Wall, SquareType.NUM_NON);
            
            typeData[(int)SquareType.MountBrickWall] = new VisualTerrainData(
                new AbsTileModelData[] {
                    new WallTileModelData(SpriteName.cmdTileMountainwall1, SpriteName.cmdTileMountainwallTop),
                    new WallTileModelData(SpriteName.cmdTileMountainwall2, SpriteName.cmdTileMountainwallTop),
                    new WallTileModelData(SpriteName.cmdTileMountainwall3, SpriteName.cmdTileMountainwallTop),
                },  TileTextureVariantType.Random,                
                false, MainTerrainType.Wall, SquareType.MountBrickGround);

            typeData[(int)SquareType.MountBrickGround] = new VisualTerrainData(new AbsTileModelData[] 
            {
                    new GroundTileModelData(SpriteName.cmdTileMountainGround1),
                    new GroundTileModelData(SpriteName.cmdTileMountainGround2),
                    new GroundTileModelData(SpriteName.cmdTileMountainGround3),
                    new GroundTileModelData(SpriteName.cmdTileMountainGround4),
            },  TileTextureVariantType.TwoByTwo,
            false, MainTerrainType.Ground, SquareType.MountBrickGround);

        }
        
        TileTextureVariantType variationType;
        AbsTileModelData[] imageVariants;
        public MainTerrainType terrainType;
        public SquareType subTerrain;
        public bool brightColors;

        public VisualTerrainData(AbsTileModelData image, bool brightColors, 
            MainTerrainType terrain, SquareType subTerrain )
            :this(new AbsTileModelData[] { image },  TileTextureVariantType.None, brightColors, terrain, subTerrain)
        {
        }

        public VisualTerrainData(AbsTileModelData[] imageVariants, TileTextureVariantType variationType, bool brightColors,
            MainTerrainType terrain, SquareType subTerrain)
        {
            this.brightColors = brightColors;
            this.imageVariants = imageVariants;
            this.variationType = variationType;
            this.terrainType = terrain;
            this.subTerrain = subTerrain;
        }

        public MainTerrainProperties Terrain
        {
            get { return MainTerrainProperties.Get(terrainType); }
        }

        public SquareModelType ModelType
        {
            get { return imageVariants[0].Type; }
        }

        public AbsTileModelData modelData(IntVector2 pos)
        {
            switch (variationType)
            {
                case TileTextureVariantType.None:
                    return imageVariants[0];
                case TileTextureVariantType.Random:
                    return arraylib.RandomListMember(imageVariants);
                case TileTextureVariantType.TwoByTwo:
                    pos.X %= 2;
                    pos.Y %= 2;
                    int ix = pos.X + pos.Y * 2;
                    return imageVariants[ix];
            }
            throw new NotImplementedException();
        }

        public SpriteName LabelImage()
        {
            return imageVariants[0].image;
        }
    }

    enum TileTextureVariantType
    {
        None,
        Random,
        TwoByTwo,
    }

    

    
}
