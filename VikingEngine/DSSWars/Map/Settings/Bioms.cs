﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map.Settings
{
    class Bioms
    {
        public BiomColor[] colors = new BiomColor[(int)BiomType.NUM];
        public Bioms()
        {
            colors[(int)BiomType.WetGreen] = new BiomColor(
                new TileColor( new Color(113, 123, 31), SurfaceTextureType.Grass),
                //new TileColor(new Color(118, 98, 30), SurfaceTextureType.Grass), 
                //new TileColor(new Color(175, 135, 68), SurfaceTextureType.Grass),

                new TileColor(new Color(208,207,148), SurfaceTextureType.Sand),//new TileColor(new Color(189, 211, 174), SurfaceTextureType.Sand), 
                new TileColor(new Color(40, 43, 19), SurfaceTextureType.None), 
                new TileColor(new Color(75, 76, 73), SurfaceTextureType.None)
                );

            colors[(int)BiomType.Green] = new BiomColor(
                new TileColor(new Color(104,146,70), SurfaceTextureType.Grass),
                //new TileColor(new Color(98,140,64), SurfaceTextureType.Grass), 
                //new TileColor(new Color(120,164,85), SurfaceTextureType.Grass),

                new TileColor(new Color(255,254,181), SurfaceTextureType.Sand), //new TileColor(new Color(206, 204, 163), SurfaceTextureType.Sand), 
                new TileColor(new Color(8, 71, 6), SurfaceTextureType.None), 
                new TileColor(new Color(73, 76, 73), SurfaceTextureType.None)
                );

            colors[(int)BiomType.YellowDry] = new BiomColor(
                new TileColor(new Color(171,162,54), SurfaceTextureType.Sand), //new TileColor(new Color(218, 163, 27), SurfaceTextureType.Sand), 
                                                                               //new TileColor(new Color(195, 141, 27), SurfaceTextureType.Sand), 
                                                                               //new TileColor(new Color(233, 213, 97), SurfaceTextureType.Sand),

                new TileColor(new Color(255,237,130), SurfaceTextureType.Sand), //new TileColor(new Color(242, 236, 197), SurfaceTextureType.Sand), 
                new TileColor(new Color(80, 60, 2), SurfaceTextureType.None), 
                new TileColor(new Color(81, 79, 68), SurfaceTextureType.None)
                );

            colors[(int)BiomType.RedDry] = new BiomColor(
                new TileColor(new Color(171,120,54), SurfaceTextureType.Sand),  //new TileColor(new Color(160, 82, 32), SurfaceTextureType.Sand),
                                                                                //new TileColor(new Color(134, 84, 50), SurfaceTextureType.Sand), 
                                                                                //new TileColor(new Color(182, 153, 102), SurfaceTextureType.Sand),

               new TileColor(new Color(255,220,130), SurfaceTextureType.Sand), //new TileColor(new Color(242, 220, 197), SurfaceTextureType.Sand), 
                new TileColor(new Color(60, 33, 9), SurfaceTextureType.None), 
                new TileColor(new Color(90, 79, 65), SurfaceTextureType.None)
                );

            colors[(int)BiomType.Frozen] = new BiomColor(
                new TileColor(new Color(86, 109, 83), SurfaceTextureType.Grass), 
                //new TileColor(new Color(131, 134, 133), SurfaceTextureType.Grass), 
                //new TileColor(new Color(230, 230, 230), SurfaceTextureType.Sand),

                new TileColor(new Color(197, 242, 242), SurfaceTextureType.Sand), 
                new TileColor(new Color(40, 53, 47), SurfaceTextureType.None), 
                new TileColor(new Color(97, 114, 114), SurfaceTextureType.None)
                );
        }
    }

    class BiomColor
    {
        const int Variants = 3;
        const int MainColorHeight = 4;
        public TileColor[] colors_height = new TileColor[Height.MaxHeight+1];
        public TileColor brightCoast;

        public SurfaceTextureType textureType = SurfaceTextureType.None;

        public BiomColor(TileColor mainCol, //TileColor mainColDarkVariant, TileColor mainColLightVariant,
            TileColor brightCoast, TileColor darkGradient, TileColor mountain)
        {
            this.brightCoast = brightCoast;
            //Under water coastal color
            for (int height = 0; height <= Height.LowWaterHeight; height++)
            {
                //for (int variant = 0; variant < Variants; variant++)
                {
                    colors_height[height] = brightCoast;
                }
            }

            //Mix towards bright coast
            {
                int height = 2;
                float percCoast = 0.5f;
                colors_height[height] = TileColor.Mix(brightCoast, mainCol, percCoast);
                //colors_height[height, 1] = TileColor.Mix(brightCoast, mainColDarkVariant, percCoast);
                //colors_height[height, 2] = TileColor.Mix(brightCoast, mainColLightVariant, percCoast);
            }

            {
                int height = 3;
                float percCoast = 0.2f;
                colors_height[height] = TileColor.Mix(brightCoast, mainCol, percCoast);
                //colors_height[height, 1] = TileColor.Mix(brightCoast, mainColDarkVariant, percCoast);
                //colors_height[height, 2] = TileColor.Mix(brightCoast, mainColLightVariant, percCoast);
            }

            //Main level colors
            {
                colors_height[MainColorHeight] = mainCol;
                //colors_height[MainColorHeight, 1] = mainColDarkVariant;
                //colors_height[MainColorHeight, 2] = mainColLightVariant;
            }

            //Mix towards dark mountain
            {
                int height = 5;
                float percDark = 0.2f;
                colors_height[height] = TileColor.Mix(darkGradient, mainCol, percDark);
                //colors_height[height, 1] = TileColor.Mix(darkGradient, mainColDarkVariant, percDark);
                //colors_height[height, 2] = TileColor.Mix(darkGradient, mainColLightVariant, percDark);
            }

            {
                int height = 6;
                float percDark = 0.4f;
                colors_height[height] = TileColor.Mix(darkGradient, mainCol, percDark);
                //colors_height[height, 1] = TileColor.Mix(darkGradient, mainColDarkVariant, percDark);
                //colors_height[height, 2] = TileColor.Mix(darkGradient, mainColLightVariant, percDark);

                float percMountainGray = 0.8f;
                colors_height[Height.MaxHeight] = TileColor.Mix(mountain, colors_height[height], percMountainGray);
                //colors_height[Height.MaxHeight, 1] = TileColor.Mix(mountain, colors_height[height, 1], percMountainGray);
                //colors_height[Height.MaxHeight, 2] = TileColor.Mix(mountain, colors_height[height, 2], percMountainGray);
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
