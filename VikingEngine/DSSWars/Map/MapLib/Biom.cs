using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.MapProcess;
using VikingEngine.LootFest.Display;

namespace VikingEngine.DSSWars.Map.Settings
{
    

    class Biom
    {
        const int MainColorHeight = 5;
        public TileColor[] colors_height = new TileColor[BiomHeightColor.MaxHeight+1];
        public TileColor brightCoast;
        public float percTree;
        public float percSoftTree;
        public float percDryWood;
        public Color mudColor = new Color(221, 193, 77);

        public SurfaceTextureType textureType = SurfaceTextureType.None;

        public LootFest.VoxelModelName treeHard = LootFest.VoxelModelName.fol_tree_hard;
        public LootFest.VoxelModelName treeSoft = LootFest.VoxelModelName.fol_tree_soft;

        BiomGroundType defaultGround;
        public BiomGroundType beachGround;
        BiomGroundType[] stoneNoiseGround;
        BiomGroundType[] herbNoiseGround;
        BiomGroundType[] treeNoiseGround;


        public Biom(TileColor mainCol, 
            TileColor brightCoast, TileColor darkGradient, TileColor mountain,
            /*float percTree, float percSoftTree, float percDryWood*/
            BiomGroundType defaultGround,
            BiomGroundType beachGround,
            BiomGroundType[] treeNoiseGround,
            BiomGroundType[] stoneNoiseGround, 
            BiomGroundType[] herbNoiseGround)
        {
            this.defaultGround = defaultGround;
            this.beachGround = beachGround;
            this.treeNoiseGround = treeNoiseGround;
            this.stoneNoiseGround = stoneNoiseGround;
            this.herbNoiseGround = herbNoiseGround;
            //this.percTree = percTree;
            //this.percSoftTree = percSoftTree;
            //this.percDryWood = percDryWood;
            this.brightCoast = brightCoast;
            TileColor mountainTop = mountain;
            mountainTop.Color = ColorExt.Mix(mountain.Color, Color.White, 0.7f);
            //Under water coastal color
            //for (int height = 0; height <= Height.LowWaterHeight; height++)
            {
                TileColor seafloor = brightCoast;
                seafloor.Color = new Color(60, 44, 24);//ColorExt.Mix(brightCoast.Color, Color.SandyBrown, 0.6f);//ColorExt.VeryDarkGray;//ColorExt.ChangeBrighness(WorldData.WaterDarkCol, -50);
                colors_height[0] = seafloor;

                TileColor lowWater = seafloor;
                lowWater.Color = ColorExt.Mix(brightCoast.Color, seafloor.Color, 0.7f);
                colors_height[BiomHeightColor.LowerWaterHeight] = lowWater;
                colors_height[BiomHeightColor.WaterSurfaceHeight] = brightCoast;    //2            
            }

            //Mix towards bright coast
            {
                int height = BiomHeightColor.MinLandHeight;//3
                float percCoast = 0.5f;
                colors_height[height] = Settings.TileColor.Mix(brightCoast, mainCol, percCoast);
            }

            {
                int height = BiomHeightColor.MinLandHeight + 1;//4
                float percCoast = 0.2f;
                colors_height[height] = Settings.TileColor.Mix(brightCoast, mainCol, percCoast);
            }

            //Main level colors
            {
                colors_height[MainColorHeight] = mainCol;//5
            }

            //Mix towards dark mountain
            {
                //int height = MainColorHeight + 1;
                //float percDark = 0.2f;
                colors_height[6] = mountain;//Settings.TileColor.Mix(darkGradient, mainCol, percDark);
            }

            {
               // const int MountainStart = 5;
                //float percDark = 0.4f;
                colors_height[7] = mountainTop;//Settings.TileColor.Mix(darkGradient, mainCol, percDark);

                //float percMountainGray = 0.8f;
                //colors_height[BiomHeightColor.MaxHeight] = Settings.TileColor.Mix(mountain, colors_height[7], percMountainGray);


                colors_height[BiomHeightColor.MaxHeight] = mountainTop;
            }
        }

        public BiomGroundType GetTileGroundType(int x, int y, EngineSpace.Maths.SimplexNoise2D noiseMap)
        {
            if (stoneNoiseGround != null)
            {
                float stonenoise = noiseMap.OctaveNoise2D(4, 0.8f, 5, -x, y);
                foreach (var ground in stoneNoiseGround)
                {
                    if (ground.noiseValue.IsWithinRange(stonenoise))
                    {
                        return ground;
                    }
                }
            }

            if (treeNoiseGround != null)
            {
                float treenoise = noiseMap.OctaveNoise2D_Normal(4, 0.75f, 1, x, y);
                foreach (var ground in treeNoiseGround)
                {
                    if (ground.noiseValue.IsWithinRange(treenoise))
                    { 
                        return ground;
                    }
                }
            }
            
            if (herbNoiseGround != null)
            {
                float herbnoise = noiseMap.OctaveNoise2D(4, 0.8f, 5, x, -y);
                foreach (var ground in herbNoiseGround)
                {
                    if (ground.noiseValue.IsWithinRange(herbnoise))
                    {
                        return ground;
                    }
                }
            }

            return defaultGround;
        }

        public TileColor TileColor(SumTile4_4 tile)
        {
            var result = colors_height[tile.biomColorHeight];
            if (tile.seaDistanceHeatMap <= 12)
            {
                result.Color = ColorExt.Mix(result.Color, brightCoast.Color, 0.5f);
            }
            return result;
        }

        public Color Tile2Color(float y)
        { 
            int level = MathExt.SplitFloat(y / BiomHeightColor.DefaultGroundYoffset, out float fraction);
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

    enum BiomType : byte
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
