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
                new TileColor( dampColors(new Color(94, 118, 25)), SurfaceTextureType.Grass),

                new TileColor(dampColors(new Color(210, 209, 136)), SurfaceTextureType.Sand),
                new TileColor(dampColors(new Color(68, 85, 20)), SurfaceTextureType.Grass), 
                new TileColor(dampColors(new Color(75, 76, 73)), SurfaceTextureType.None),
                1.1f, 0.6f, 0
                );

            bioms[(int)BiomType.Swamp] = new Biom(
                new TileColor(dampColors(new Color(113, 123, 31)), SurfaceTextureType.Grass),

                new TileColor(dampColors(new Color(208, 207, 148)), SurfaceTextureType.Sand),
                new TileColor(dampColors(new Color(40, 43, 19)), SurfaceTextureType.Grass),
                new TileColor(dampColors(new Color(75,82, 59)), SurfaceTextureType.None),
                1.1f, 0.2f, 0
                );

            bioms[(int)BiomType.Green] = new Biom(
                new TileColor(dampColors(new Color(104,146,70)), SurfaceTextureType.Grass),

                new TileColor(dampColors(new Color(255,254,181)), SurfaceTextureType.Sand),
                new TileColor(dampColors(ColorExt.ChangeBrighness( new Color(8, 71, 6), -10)), SurfaceTextureType.Grass), 
                new TileColor(dampColors(ColorExt.ChangeBrighness(new Color(73, 76, 73), -10)), SurfaceTextureType.None),
                1f, 0.25f, 0
                );

            bioms[(int)BiomType.Hills] = new Biom(
               new TileColor(dampColors(new Color(115, 198, 68)), SurfaceTextureType.Grass),

               new TileColor(dampColors(new Color(216, 230, 129)), SurfaceTextureType.Sand),
               new TileColor(dampColors(new Color(70, 151, 41)), SurfaceTextureType.Grass),
               new TileColor(dampColors(new Color(73, 76, 73)), SurfaceTextureType.None),
               1.2f, 0.1f, 0
               );

            bioms[(int)BiomType.GreenDry] = new Biom(
               new TileColor(dampColors(new Color(208, 188, 119)), SurfaceTextureType.Grass),

               new TileColor(dampColors(new Color(230, 214, 162)), SurfaceTextureType.Sand),
               new TileColor(dampColors(ColorExt.ChangeBrighness(new Color(180, 161, 97), -10)), SurfaceTextureType.Grass),
               new TileColor(dampColors(ColorExt.ChangeBrighness(new Color(124, 128, 107), -10)), SurfaceTextureType.None),
               0.5f, 0.9f, 0.5f
               );

            bioms[(int)BiomType.YellowDry] = new Biom(
                new TileColor(dampColors(new Color(171,162,54)), SurfaceTextureType.Sand), 

                new TileColor(dampColors(new Color(255,237,130)), SurfaceTextureType.Sand), 
                new TileColor(dampColors(new Color(80, 60, 2)), SurfaceTextureType.None), 
                new TileColor(dampColors(new Color(81, 79, 68)), SurfaceTextureType.None),
                0.5f, 0, 0.6f
                );

            bioms[(int)BiomType.RedDry] = new Biom(
                new TileColor(dampColors(new Color(171,120,54)), SurfaceTextureType.Sand),  

               new TileColor(dampColors(new Color(255,220,130)), SurfaceTextureType.Sand),
                new TileColor(dampColors(new Color(60, 33, 9)), SurfaceTextureType.None), 
                new TileColor(dampColors(new Color(90, 79, 65)), SurfaceTextureType.None),
                0.6f, 0, 0.5f
                );

            bioms[(int)BiomType.DarkLands] = new Biom(
                new TileColor(dampColors(new Color(58, 94, 108)), SurfaceTextureType.None),

               new TileColor(dampColors(new Color(102, 115, 116)), SurfaceTextureType.Sand),
                new TileColor(dampColors(new Color(58, 94, 108)), SurfaceTextureType.None),
                new TileColor(dampColors(new Color(39, 59, 57)), SurfaceTextureType.None),
                0.6f, 0, 0.6f)
                { 
                    mudColor= dampColors(new Color(24, 56, 67)), 
                    treeHard = LootFest.VoxelModelName.fol_tree_hard_lava,
                    treeSoft = LootFest.VoxelModelName.fol_tree_soft_lava,
                };

            bioms[(int)BiomType.Frozen] = new Biom(
                new TileColor(dampColors(new Color(86, 109, 83)), SurfaceTextureType.Grass), 

                new TileColor(dampColors(new Color(197, 242, 242)), SurfaceTextureType.Sand), 
                new TileColor(dampColors(new Color(40, 53, 47)), SurfaceTextureType.None), 
                new TileColor(dampColors(new Color(97, 114, 114)), SurfaceTextureType.None),
                1.3f, 0.8f, 0.2f
                )
            {
                treeHard = LootFest.VoxelModelName.fol_tree_hard_snow,
                treeSoft = LootFest.VoxelModelName.fol_tree_soft_snow,
            };

            bioms[(int)BiomType.Tundra] = new Biom(
                new TileColor(dampColors(new Color(148,133,55)), SurfaceTextureType.Grass),

                new TileColor(dampColors(new Color(178, 188, 152)), SurfaceTextureType.Sand),
                new TileColor(dampColors(new Color(100, 91, 42)), SurfaceTextureType.Grass),
                new TileColor(dampColors(new Color(86, 91, 75)), SurfaceTextureType.None),
                0.5f, 0.9f, 0.5f
                );

            Color dampColors(Color color)
            { 
                color.Deconstruct(out byte r, out byte g, out byte b);

                r = (byte)(26 + contrast(r));
                g = (byte)(16 + contrast(g));
                b = (byte)(10 + contrast(b));

                int contrast(int value)
                {
                    if (value < 140)
                    {
                        return (int)(value * 0.8);
                    }
                    else
                    {
                        return (int)(value * 0.9);
                    }
                }

                return new Color(r, g, b);
            }
        }
    }

    class Biom
    {
        const int MainColorHeight = 5;
        public TileColor[] colors_height = new TileColor[Height.MaxHeight+1];
        public TileColor brightCoast;
        public float percTree;
        public float percSoftTree;
        public float percDryWood;
        public Color mudColor = new Color(221, 193, 77);

        public SurfaceTextureType textureType = SurfaceTextureType.None;

        public LootFest.VoxelModelName treeHard = LootFest.VoxelModelName.fol_tree_hard;
        public LootFest.VoxelModelName treeSoft = LootFest.VoxelModelName.fol_tree_soft;


        public Biom(TileColor mainCol, 
            TileColor brightCoast, TileColor darkGradient, TileColor mountain,
            float percTree, float percSoftTree, float percDryWood)
        {
            this.percTree = percTree;
            this.percSoftTree = percSoftTree;
            this.percDryWood = percDryWood;
            this.brightCoast = brightCoast;
            //Under water coastal color
            //for (int height = 0; height <= Height.LowWaterHeight; height++)
            {
                TileColor seafloor = brightCoast;
                seafloor.Color = Color.Black;//ColorExt.VeryDarkGray;//ColorExt.ChangeBrighness(WorldData.WaterDarkCol, -50);
                colors_height[Height.LowerWaterHeight] = seafloor;
                colors_height[Height.LowWaterHeight] = brightCoast;                
            }

            //Mix towards bright coast
            {
                int height = Height.MinLandHeight;
                float percCoast = 0.5f;
                colors_height[height] = Settings.TileColor.Mix(brightCoast, mainCol, percCoast);
            }

            {
                int height = Height.MinLandHeight + 1;
                float percCoast = 0.2f;
                colors_height[height] = Settings.TileColor.Mix(brightCoast, mainCol, percCoast);
            }

            //Main level colors
            {
                colors_height[MainColorHeight] = mainCol;
            }

            //Mix towards dark mountain
            {
                int height = MainColorHeight + 1;
                float percDark = 0.2f;
                colors_height[height] = Settings.TileColor.Mix(darkGradient, mainCol, percDark);
            }

            {
                float percDark = 0.4f;
                colors_height[Height.MountainHeightStart] = Settings.TileColor.Mix(darkGradient, mainCol, percDark);

                float percMountainGray = 0.8f;
                colors_height[Height.MountainHeightStart + 1] = Settings.TileColor.Mix(mountain, colors_height[Height.MountainHeightStart], percMountainGray);

                
                colors_height[Height.MaxHeight] = mountain;
            }


        }

        public TileColor TileColor(Tile tile)
        {
            var result = colors_height[tile.heightLevel];
            if (tile.seaDistanceHeatMap <= 12)
            {
                result.Color = ColorExt.Mix(result.Color, brightCoast.Color, 0.5f);
            }
            return result;
        }

        public Color Tile2Color(float y)
        { 
            int level = MathExt.SplitFloat(y / Height.DefaultGroundYoffset, out float fraction);
            //float percFraction = fraction;


            var col1 = arraylib.GetClamped(colors_height, level);
            var col2 = arraylib.GetClamped(colors_height, level+1);

            return ColorExt.Mix(col1.Color, col2.Color, fraction);

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
        Hills,
        Green,
        GreenDry,
        WetGreen,
        Swamp,
        Frozen,
        Tundra,
        YellowDry,
        RedDry,
        DarkLands,
        NUM
    }
}
