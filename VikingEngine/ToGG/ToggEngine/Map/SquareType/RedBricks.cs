using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Map
{    
    class RedBrickGroundSquare : AbsSquare
    {
        public RedBrickGroundSquare()
        {
            textures = new SpriteName[] {
                SpriteName.cmdTileRedBrickGround1, SpriteName.cmdTileRedBrickGround2,
                SpriteName.cmdTileRedBrickGround3, SpriteName.cmdTileRedBrickGround4};
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.TwoByTwo, args.rnd);
        }

        override public SquareType Type { get { return SquareType.RedBrickGround; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Ground; } }
        override public string Name { get { return "Red bricks Ground"; } }
        override public bool brightColors { get { return false; } }
    }
}
