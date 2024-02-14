using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class MountainGroundSquare : AbsSquare
    {
        public MountainGroundSquare()
        {
            textures = new SpriteName[] {
                SpriteName.cmdTileMountainGround1, SpriteName.cmdTileMountainGround2,
                SpriteName.cmdTileMountainGround3, SpriteName.cmdTileMountainGround4};
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.TwoByTwo, args.rnd);
        }

        override public SquareType Type { get { return SquareType.MountainGround; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Ground; } }
        override public string Name { get { return "Mountain Ground"; } }
        override public bool brightColors { get { return false; } }
    }

    class MountBrickGroundSquare : AbsSquare
    {
        public MountBrickGroundSquare()
        {
            textures = new SpriteName[] {
                SpriteName.cmdTileMountBrickGround1, SpriteName.cmdTileMountBrickGround2,
                SpriteName.cmdTileMountBrickGround3, SpriteName.cmdTileMountBrickGround4};
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.TwoByTwo, args.rnd);
        }

        override public SquareType Type { get { return SquareType.MountBrickGround; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Ground; } }
        override public string Name { get { return "Mountain Bricks Ground"; } }
        override public bool brightColors { get { return false; } }
    }

    class MountBrickWallSquare : AbsSquare
    {
        public MountBrickWallSquare()
        {
            //useProperty_Light = true;

            textures = new SpriteName[] { SpriteName.cmdTileMountBrickWall };
            wallTopTexture(SpriteName.cmdTileMountBrickWallTop);
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SpriteName frontTex;

            bool torchlight = args.squareContent.visualProperties.variant == 1;
            bool plants = args.squareContent.visualProperties.variant == 2;

            if (torchlight)
            {
                frontTex = SpriteName.cmdTileMountBrickWallTorch;
            }
            else
            {
                frontTex = textures[0];
            }

            SquareModelLib.WallModel(args, frontTex, textures[0], topEdgeTex, topCornerTex);

            if (torchlight && args.openTerrainS)
            {
                //Torch
                const float HWidth = 0.28f;
                const float HHeight = HWidth * 0.5f;


                Vector3 center = args.wallSPoly.Center();
                center.Y += 0.12f;
                center.Z += 0.08f;

                float zDiff = GenerateBoardModelArgs.WallFrontLeanPerUnit * HHeight;

                Graphics.PolygonColor poly = new PolygonColor(
                    new Vector3(center.X - HWidth, center.Y + HHeight, center.Z - zDiff),
                     new Vector3(center.X + HWidth, center.Y + HHeight, center.Z - zDiff),
                     new Vector3(center.X - HWidth, center.Y - HHeight, center.Z + zDiff),
                     new Vector3(center.X + HWidth, center.Y - HHeight, center.Z + zDiff),
                     SpriteName.cmdTileMountBrickWallTorchSprite, Dir4.N, Color.White);

                args.polygons.Add(poly);

                new ToggEngine.TorchFire(args, new Vector3(center.X, center.Y + HHeight * 0.7f, center.Z));
                //args.torches.Add(new Vector3(center.X, center.Y + HHeight * 0.7f, center.Z + 0.00f));
            }

            if (plants)
            {
                SquareModelLib.FullWallDecal_S(args, SpriteName.cmdTileWallIvy1);
            }
        }

        public override string[] Variants()
        {
            return new string[] { DefaultVariantName, "Torch", "Plants" };
        }

        override public SquareType Type { get { return SquareType.MountBrickWall; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Wall; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Wall; } }
        override public string Name { get { return "Mountain Bricks Wall"; } }
        override public bool brightColors { get { return false; } }
    }

    class MountainWallSquare : AbsSquare
    {
        SpriteName[] floorEdgeTextures = new SpriteName[]{
            SpriteName.cmdTileMountainwallFloorEdge2,
            SpriteName.cmdTileMountainwallFloorEdge3,
            SpriteName.cmdTileMountainwallFloorEdge1
        };

        public MountainWallSquare()
        {
            textures = new SpriteName[] { SpriteName.cmdTileMountainwall1, SpriteName.cmdTileMountainwall2, SpriteName.cmdTileMountainwall3 };
            wallTopTexture(SpriteName.cmdTileMountainwallTop);
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            SquareModelLib.WallModel(args, textures, TileTextureVariantType.Random, args.rnd, topEdgeTex, topCornerTex);
            SquareModelLib.WallFloorEdge(args, floorEdgeTextures);

            if (args.squareContent.visualProperties.variant == 1)
            {
                SquareModelLib.FullWallDecal_S(args, SpriteName.cmdTileWallIvy1);
            }
        }

        public override string[] Variants()
        {
            return new string[] { DefaultVariantName, "Plants" };
        }

        override public SquareType Type { get { return SquareType.MountainWall; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Wall; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Wall; } }
        override public string Name { get { return "Mountain Wall"; } }
        override public bool brightColors { get { return false; } }
    }
}
