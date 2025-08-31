using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Map.Settings
{
    class Height
    {
        public const int DeepWaterHeight = 0;
        public const int LowerWaterHeight = 1;
        public const int LowWaterHeight = 2;
        public const int MinLandHeight = 3;
        public const int MineHeightStart = 6;
        public const int MountainHeightStart = 7;
        public const int MountainLowPeak = 8;

        public const int MaxHeight = 9;

        
        public const float LowWater_Tile2Y = LowWaterHeight * DefaultGroundYoffset;
        public const float AboveWater_Tile2Y = LowWater_Tile2Y + DefaultGroundYoffset;
        public const float MaxLand_Tile2Y = (MineHeightStart -1) * DefaultGroundYoffset;
        public const float MountainStart_Tile2Y = MountainHeightStart * DefaultGroundYoffset;
        public const float MountainLowPeak_Tile2Y = MountainLowPeak * DefaultGroundYoffset;
        //public const int BiomTypeGreen = 0;
        //public const int BiomTypeDry = 1;
        //public const int BiomCount = 2;



        //static readonly Color Dry0 = new Color(253, 198, 137);
        //static readonly Color Dry1 = new Color(198, 156, 109);
        //static readonly Color Dry2 = new Color(130, 123, 0);
        //static readonly Color Dry3 = new Color(105, 99, 0);
        //static readonly Color Dry4 = new Color(83, 71, 65);
        //static readonly Color Dry5 = new Color(54, 47, 45);

        //static readonly Color Ground0 = new Color(188, 204, 102);
        //static readonly Color Ground1 = new Color(156, 170, 79);
        //static readonly Color Ground2 = new Color(25, 123, 48);
        //static readonly Color Ground3 = new Color(18, 102, 38);
        //static readonly Color Ground4 = new Color(137, 137, 137);
        //static readonly Color Ground5 = new Color(70, 70, 70);

        const double DefaultGroundYoffsetChance = 0.6;
        public const float DefaultGroundYoffset = 0.012f;

        //public Color color;
        public double groundYoffsetChance;
        public float groundYoffset;
        public float[,] mountainPeak = null;
        //public SurfaceTextureType textureType = SurfaceTextureType.None;
        //public bool[] foilEnabled = new bool[(int)SubTileFoilType.NUM];

        public TerrainCultureType culture = TerrainCultureType.Plains;

        public int influenceCost;

        public float percTree = 0;
        public bool isMountainPeek= false;


        public Height(int height)
        {
            groundYoffsetChance = DefaultGroundYoffsetChance;
            groundYoffset = DefaultGroundYoffset;


            switch (height)
            {
                case DeepWaterHeight:
                    influenceCost = 2000;
                    break;

                case 1:
                    influenceCost = 1600;
                    break;

                case LowWaterHeight:
                    influenceCost = 800;
                    break;

                case 3:
                    percTree = 0.3f;
                    influenceCost = 10;

                    break;

                case 4:
                    culture = TerrainCultureType.Forest;
                    influenceCost = 12;
                    percTree = 0.4f;
                    break;

                case 5:
                    culture = TerrainCultureType.Forest;
                    percTree = 0.6f;
                    influenceCost = 14;
                    break;

                case 6:
                    culture = TerrainCultureType.Forest;
                    percTree = 0.75f;
                    influenceCost = 16;
                    break;

                case 7:
                    groundYoffset = DefaultGroundYoffset * 1.2f;
                    groundYoffsetChance = 0.7;
                    createMountainPeak(0.07f);
                    culture = TerrainCultureType.Mountain;

                    percTree = 0.4f;
                    influenceCost = 18;
                    break;

                case 8:
                    groundYoffset = DefaultGroundYoffset * 1.8f;
                    groundYoffsetChance = 0.8;
                    createMountainPeak(0.20f);
                    culture = TerrainCultureType.Mountain;

                    influenceCost = 100;
                    isMountainPeek = true;
                    break;

                case 9:
                    groundYoffset = DefaultGroundYoffset * 2.2f;
                    groundYoffsetChance = 0.8;
                    createMountainPeak(0.28f);
                    culture = TerrainCultureType.Mountain;

                    influenceCost = 200;
                    isMountainPeek = true;
                    break;

                default: throw new NotImplementedException();
            }
        }

        void createMountainPeak(float peakHeight)
        {
            mountainPeak = new float[WorldData.TileSubDivitions, WorldData.TileSubDivitions];

            ForXYLoop loop = new ForXYLoop(new IntVector2(WorldData.TileSubDivitions));

            Vector2 center = new Vector2((WorldData.TileSubDivitions - 1) * 0.5f);

            float half = WorldData.TileSubDivitions * 0.5f;

            while (loop.Next())
            {
                float offsetPerc = VectorExt.SideLength(loop.Position.Vec - center) / half;
                mountainPeak[loop.Position.X, loop.Position.Y] = (1f - offsetPerc) * peakHeight;
            }
        }
    }

    

    enum TerrainCultureType
    {
        Plains,
        Forest,
        Mountain,
    }
}
