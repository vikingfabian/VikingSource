using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Map
{//58*89
    class GrassSquare : AbsSquare
    {
        public GrassSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTileGrassB1, SpriteName.cmdTileGrassB2, SpriteName.cmdTileGrassB3 };
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.Random, args.rnd);
        }

        override public SquareType Type { get { return SquareType.Grass; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Ground; } }
        override public string Name { get { return "Grass"; } }
        override public bool brightColors { get { return false; } }
    }

    class GrassObsticle : AbsSquare
    {
        public GrassObsticle()
        {
            //textures = new SpriteName[] { SpriteName.cmdTileGrassB1, SpriteName.cmdTileGrassB2, SpriteName.cmdTileGrassB3 };
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            switch (args.squareContent.visualProperties.variant)
            {
                default: //0
                    SquareModelLib.FlatGroundModel(args, SpriteName.cmdTileGrassBTree);
                    SquareModelLib.GroundBillboardModel(args, SpriteName.cmdTileGrassBTreeModel,
                        new IntVector2(2, 3), 0.7f, new Vector2(0.0f, 0.3f));
                    break;
                case 1:
                    SquareModelLib.FlatGroundModel(args, SpriteName.cmdTileGrassBRock);
                    SquareModelLib.GroundBillboardModel(args, SpriteName.cmdTileGrassBRockModel,
                        new IntVector2(2, 2), 0.8f, new Vector2(0.0f, 0.4f));
                    break;
            }

        }

        public override string[] Variants()
        {
            return new string[] { "Tree", "Boulder" };
        }

        public override SpriteName LabelImage()
        {
            return SpriteName.cmdTileGrassBRock;
        }

        override public SquareType Type { get { return SquareType.GrassObsticle; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Wall; } }
        override public string Name { get { return "Grass Obsticle"; } }
        override public bool brightColors { get { return false; } }
    }

    class GrassMudSquare : AbsSquare
    {
        public GrassMudSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTileGrassBMud1, SpriteName.cmdTileGrassBMud2 };
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.Random, args.rnd);
        }

        override public SquareType Type { get { return SquareType.Grass; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Ground; } }
        override public string Name { get { return "Grass"; } }
        override public bool brightColors { get { return false; } }
    }

    class GrassHillSquare : AbsSquare
    {
        public GrassHillSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTileHill };
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.Random, args.rnd);
        }

        override public SquareType Type { get { return SquareType.GreenHill; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Hill; } }
        override public string Name { get { return "Hill"; } }
        override public bool brightColors { get { return false; } }
    }

    class GrassForestSquare : AbsSquare
    {
        public GrassForestSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTileForest };
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.Random, args.rnd);
        }

        override public SquareType Type { get { return SquareType.GreenForest; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Forest; } }
        override public string Name { get { return "Forest"; } }
        override public bool brightColors { get { return false; } }
    }
}
