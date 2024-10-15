using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map.Settings
{
    class WorldBioms
    {
        public Biom[] bioms = new Biom[(int)BiomType.NUM];
        public WorldBioms()
        {
            bioms[(int)BiomType.WetGreen] = new Biom(
                new TileColor( new Color(113, 123, 31), SurfaceTextureType.Grass),

                new TileColor(new Color(208,207,148), SurfaceTextureType.Sand),
                new TileColor(new Color(40, 43, 19), SurfaceTextureType.None), 
                new TileColor(new Color(75, 76, 73), SurfaceTextureType.None),
                1.1f, 0.6f, 0
                );

            bioms[(int)BiomType.Green] = new Biom(
                new TileColor(new Color(104,146,70), SurfaceTextureType.Grass),

                new TileColor(new Color(255,254,181), SurfaceTextureType.Sand),
                new TileColor(new Color(8, 71, 6), SurfaceTextureType.None), 
                new TileColor(new Color(73, 76, 73), SurfaceTextureType.None),
                1f, 0.25f, 0
                );

            bioms[(int)BiomType.YellowDry] = new Biom(
                new TileColor(new Color(171,162,54), SurfaceTextureType.Sand), 

                new TileColor(new Color(255,237,130), SurfaceTextureType.Sand), 
                new TileColor(new Color(80, 60, 2), SurfaceTextureType.None), 
                new TileColor(new Color(81, 79, 68), SurfaceTextureType.None),
                0.5f, 0, 0.6f
                );

            bioms[(int)BiomType.RedDry] = new Biom(
                new TileColor(new Color(171,120,54), SurfaceTextureType.Sand),  

               new TileColor(new Color(255,220,130), SurfaceTextureType.Sand),
                new TileColor(new Color(60, 33, 9), SurfaceTextureType.None), 
                new TileColor(new Color(90, 79, 65), SurfaceTextureType.None),
                0.6f, 0, 0.5f
                );

            bioms[(int)BiomType.Frozen] = new Biom(
                new TileColor(new Color(86, 109, 83), SurfaceTextureType.Grass), 

                new TileColor(new Color(197, 242, 242), SurfaceTextureType.Sand), 
                new TileColor(new Color(40, 53, 47), SurfaceTextureType.None), 
                new TileColor(new Color(97, 114, 114), SurfaceTextureType.None),
                1.3f, 0.8f, 0.2f
                );
        }
    }

    class Biom
    {
        const int MainColorHeight = 4;
        public TileColor[] colors_height = new TileColor[Height.MaxHeight+1];
        public TileColor brightCoast;
        public float percTree;
        public float percSoftTree;
        public float percDryWood;

        public SurfaceTextureType textureType = SurfaceTextureType.None;

        public Biom(TileColor mainCol, 
            TileColor brightCoast, TileColor darkGradient, TileColor mountain,
            float percTree, float percSoftTree, float percDryWood)
        {
            this.percTree = percTree;
            this.percSoftTree = percSoftTree;
            this.percDryWood = percDryWood;
            this.brightCoast = brightCoast;
            //Under water coastal color
            for (int height = 0; height <= Height.LowWaterHeight; height++)
            {
                colors_height[height] = brightCoast;
                
            }

            //Mix towards bright coast
            {
                int height = 2;
                float percCoast = 0.5f;
                colors_height[height] = TileColor.Mix(brightCoast, mainCol, percCoast);
            }

            {
                int height = 3;
                float percCoast = 0.2f;
                colors_height[height] = TileColor.Mix(brightCoast, mainCol, percCoast);
            }

            //Main level colors
            {
                colors_height[MainColorHeight] = mainCol;
            }

            //Mix towards dark mountain
            {
                int height = 5;
                float percDark = 0.2f;
                colors_height[height] = TileColor.Mix(darkGradient, mainCol, percDark);
            }

            {
                int height = 6;
                float percDark = 0.4f;
                colors_height[height] = TileColor.Mix(darkGradient, mainCol, percDark);

                float percMountainGray = 0.8f;
                colors_height[Height.MaxHeight] = TileColor.Mix(mountain, colors_height[height], percMountainGray);
            }


        }

        public TileColor Color(Tile tile)
        {
            var result = colors_height[tile.heightLevel];
            if (tile.seaDistanceHeatMap <= 12)
            {
                result.Color = ColorExt.Mix(result.Color, brightCoast.Color, 0.5f);
            }
            return result;
        }
    }

    struct TileColor
    {
        public Color Color;
        public SurfaceTextureType Texture;

        public TileColor(Color color, SurfaceTextureType texture)
        {
            this.Color = color;
            this.Texture = texture;
        }

        public static TileColor Mix(TileColor col1, TileColor col2, float percentageCol1)
        {
            var result = new TileColor();
            result.Color = ColorExt.Mix(col1.Color, col2.Color, percentageCol1);
            result.Texture = percentageCol1 >=0.5f? col1.Texture : col2.Texture;

            return result;
        }
    }

    enum SurfaceTextureType
    {
        None,
        Grass,
        Sand,
    }

    enum BiomType
    {
        Green,
        WetGreen,
        Frozen,
        YellowDry,
        RedDry,
        NUM
    }
}
