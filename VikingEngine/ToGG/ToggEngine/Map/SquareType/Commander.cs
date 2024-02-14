using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class MountainSquare : AbsSquare
    {
        public MountainSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTileMountain };
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.Random, args.rnd);
        }

        override public SquareType Type { get { return SquareType.GreenMountain; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Mountain; } }
        override public string Name { get { return "Mountain"; } }
        override public bool brightColors { get { return false; } }
    }

    class TownSquare : AbsSquare
    {
        public TownSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTileTown };
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.Random, args.rnd);
        }

        override public SquareType Type { get { return SquareType.GreenTown; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.cmdTown; } }
        override public string Name { get { return "Building"; } }
        override public bool brightColors { get { return false; } }
    }

    class SwampSquare : AbsSquare
    {
        public SwampSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTileSwamp };
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.Random, args.rnd);
        }

        override public SquareType Type { get { return SquareType.GreenSwamp; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Swamp; } }
        override public string Name { get { return "Swamp"; } }
        override public bool brightColors { get { return false; } }
    }

    class WaterSquare : AbsSquare
    {
        public WaterSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTileOpenWater };
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.Random, args.rnd);
        }

        override public SquareType Type { get { return SquareType.GreenWaterPuddle; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Water; } }
        override public string Name { get { return "Water"; } }
        override public bool brightColors { get { return false; } }
    }

    class RubbleSquare : AbsSquare
    {
        public RubbleSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTileRubble };
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.Random, args.rnd);
        }

        override public SquareType Type { get { return SquareType.GreenRubble; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.cmdRubble; } }
        override public string Name { get { return "Rubble"; } }
        override public bool brightColors { get { return false; } }
    }

    class PalisadSquare : AbsSquare
    {
        public PalisadSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTilePalisad };
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.Random, args.rnd);
        }

        override public SquareType Type { get { return SquareType.GreenPalisad; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.cmdPalisad; } }
        override public string Name { get { return "Palisad"; } }
        override public bool brightColors { get { return false; } }
    }

    class PavedRoadSquare : AbsSquare
    {
        public PavedRoadSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTilePavedRoad };
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.Random, args.rnd);
        }

        override public SquareType Type { get { return SquareType.GreenMountain; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.PavedRoad; } }
        override public string Name { get { return "Road"; } }
        override public bool brightColors { get { return false; } }
    }

    class TowerSquare : AbsSquare
    {
        public TowerSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTileCastle };
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.Random, args.rnd);
        }

        override public SquareType Type { get { return SquareType.GreenMountain; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Tower; } }
        override public string Name { get { return "Tower"; } }
        override public bool brightColors { get { return false; } }
    }
}
