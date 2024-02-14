using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class HouseGroundSquare : AbsSquare
    {
        SpriteName[] oddTextures = new SpriteName[] { SpriteName.cmdTileHouseGround2, SpriteName.cmdTileHouseGround4 };

        public HouseGroundSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTileHouseGround1, SpriteName.cmdTileHouseGround3 };
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SpriteName[] texs;
            if (lib.IsEven(args.squarePos.X + args.squarePos.Y))
            {
                texs = textures;
            }
            else
            {
                texs = oddTextures;
            }
            SquareModelLib.FlatGroundModel(args, texs, TileTextureVariantType.Random, args.rnd);
        }

        override public SquareType Type { get { return SquareType.HouseGround; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Ground; } }
        override public string Name { get { return "House Ground"; } }
        override public bool brightColors { get { return false; } }
    }

    class HouseWallSquare : AbsSquare
    {
        public HouseWallSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTileHouseWall1, SpriteName.cmdTileHouseWall2, SpriteName.cmdTileHouseWall3 };
            wallTopTexture(SpriteName.cmdTileHouseTop);
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.WallModel(args, textures, TileTextureVariantType.Random, args.rnd, topEdgeTex, topCornerTex);

            if (args.squareContent.visualProperties.variant == 1)
            {
                SquareModelLib.FullWallDecal_S(args, SpriteName.cmdTileWallIvy1);
            }
        }

        override public SquareType Type { get { return SquareType.HouseWall; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Wall; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Wall; } }
        override public string Name { get { return "House Wall"; } }
        override public bool brightColors { get { return false; } }
    }
}
