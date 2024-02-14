using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    /// <summary>
    /// Setup for a board square terrain properties and visuals
    /// </summary>
    abstract class AbsSquare
    {
        protected SpriteName[] textures;
        protected Graphics.Sprite topEdgeTex, topCornerTex;

        abstract public void generateModel(GenerateBoardModelArgs args);

        public MainTerrainProperties Terrain()
        {
            return MainTerrainProperties.Get(TerrainType);
        }

        protected void wallTopTexture(SpriteName sprite)
        {
            topEdgeTex = DataLib.SpriteCollection.Get(sprite);
            topCornerTex = topEdgeTex;

            int texH = topEdgeTex.Texture().Height;
            topEdgeTex.Source.Height /= 2;
            topEdgeTex.UpdateSourcePolygon(true);

            topCornerTex.Source.Width /= 2;
            topCornerTex.Source.Height /= 2;
            topCornerTex.Source.Y += topCornerTex.Source.Height;
            topCornerTex.UpdateSourcePolygon(true);
        }        

        abstract public SquareType Type { get; }
        abstract public SquareType SubType { get; }
        abstract public SquareModelType ModelType { get; }
        abstract public MainTerrainType TerrainType { get; }
        abstract public string Name { get; }
        abstract public bool brightColors { get; }

        virtual public SpriteName LabelImage()
        {
            return textures[0];
        }

        protected string DefaultVariantName = "Default";
        virtual public string[] Variants()
        {
            return null;
        }

        public float ModelHeight()
        {
            if (this.ModelType == SquareModelType.Wall)
            {
                return SquareModelLib.WallHeight;
            }
            return 0;
        }
    }

    class FallpitSquare : AbsSquare
    {
        public FallpitSquare()
        {
            textures = null;
        }

        public override void generateModel(GenerateBoardModelArgs args)
        {
            //SquareModelLib.FlatGroundModel(args, textures, TileTextureVariantType.Random, args.rnd);
        }

        override public SquareType Type { get { return SquareType.Fallpit; } }
        override public SquareType SubType { get { return SquareType.NUM_NON; } }
        override public SquareModelType ModelType { get { return SquareModelType.Ground; } }
        override public MainTerrainType TerrainType { get { return MainTerrainType.Pit; } }
        override public string Name { get { return "Fall pit"; } }
        override public bool brightColors { get { return false; } }

        public override SpriteName LabelImage()
        {
            return SpriteName.WhiteArea_LFtiles;
        }
    }
}
