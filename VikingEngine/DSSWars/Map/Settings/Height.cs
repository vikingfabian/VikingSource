using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Map.Settings
{
    class Height
    {
        public const int DeepWaterHeight = 0;
        public const int LowWaterHeight = 1;
        public const int MinLandHeight = 2;
        public const int MountainHeightStart = 6;
        public const int MaxHeight = 7;

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
        const float DefaultGroundYoffset = 0.012f;

        //public Color color;
        public double groundYoffsetChance;
        public float groundYoffset;
        public float[,] mountainPeak = null;
        //public SurfaceTextureType textureType = SurfaceTextureType.None;
        //public bool[] foilEnabled = new bool[(int)SubTileFoilType.NUM];

        public TerrainCultureType culture = TerrainCultureType.Plains;

        public int influenceCost;

        public float percTree = 0;



        public Height(int height)
        {
            groundYoffsetChance = DefaultGroundYoffsetChance;
            groundYoffset = DefaultGroundYoffset;


            switch (height)
            {
                case DeepWaterHeight:
                    //color = DeepWaterCol1;
                    influenceCost = 1600;
                    break;

                case LowWaterHeight:
                    //color = SeaBottomCol;
                    influenceCost = 800;
                    break;

                case 2:
                    //foilEnabled[(int)SubTileFoilType.Stones] = true;
                    //switch (biom)
                    //{
                    //    case BiomTypeGreen:
                    //        color = Ground0;
                    //        textureType = SurfaceTextureType.Sand;
                    //        percTree = 0.3f;
                    //        break;
                    //    case BiomTypeDry:
                    //        color = Dry0;
                    //        textureType = SurfaceTextureType.Sand;
                    //        percTree = 0.1f;
                    //        break;
                    //}
                    percTree = 0.3f;
                    influenceCost = 10;

                    break;

                case 3:
                    //foilEnabled[(int)SubTileFoilType.Stones] = true;
                    //switch (biom)
                    //{
                    //    case BiomTypeGreen:
                    //        color = Ground1;
                    //        textureType = SurfaceTextureType.Grass;
                    //        foilEnabled[(int)SubTileFoilType.TreeHard] = true;
                    //        culture = TerrainCultureType.Forest;
                    //        percTree = 0.6f;
                    //        break;
                    //    case BiomTypeDry:
                    //        color = Dry1;
                    //        textureType = SurfaceTextureType.Sand;
                    //        percTree = 0.3f;
                    //        break;
                    //}
                    culture = TerrainCultureType.Forest;
                    influenceCost = 12;
                    percTree = 0.4f;
                    break;

                case 4:
                    //foilEnabled[(int)SubTileFoilType.Stones] = true;
                    //switch (biom)
                    //{
                    //    case BiomTypeGreen:
                    //        color = Ground2;
                    //        textureType = SurfaceTextureType.Grass;
                    //        foilEnabled[(int)SubTileFoilType.TreeHard] = true;
                    //        culture = TerrainCultureType.Forest;
                    //        percTree = 0.6f;
                    //        break;
                    //    case BiomTypeDry:
                    //        color = Dry2;
                    //        culture = TerrainCultureType.Mountain;
                    //        percTree = 0.4f;
                    //        break;
                    //}
                    culture = TerrainCultureType.Forest;
                    percTree = 0.6f;
                    influenceCost = 14;
                    break;

                case 5:
                    //foilEnabled[(int)SubTileFoilType.Stones] = true;
                    //switch (biom)
                    //{
                    //    case BiomTypeGreen:
                    //        color = Ground3;
                    //        textureType = SurfaceTextureType.Grass;
                    //        foilEnabled[(int)SubTileFoilType.TreeHard] = true;
                    //        culture = TerrainCultureType.Forest;
                    //        percTree = 0.75f;
                    //        break;
                    //    case BiomTypeDry:
                    //        color = Dry3;
                    //        culture = TerrainCultureType.Mountain;
                    //        percTree = 0.45f;
                    //        break;
                    //}
                    culture = TerrainCultureType.Forest;
                    percTree = 0.75f;
                    influenceCost = 16;
                    break;

                case 6:
                    groundYoffset = DefaultGroundYoffset * 1.2f;
                    groundYoffsetChance = 0.7;
                    createMountainPeak(0.14f);
                    culture = TerrainCultureType.Mountain;

                    //switch (biom)
                    //{
                    //    case BiomTypeGreen:
                    //        color = Ground4;
                    //        percTree = 0.4f;
                    //        break;
                    //    case BiomTypeDry:
                    //        color = Dry4;
                    //        percTree = 0.2f;
                    //        break;
                    //}
                    percTree = 0.4f;
                    influenceCost = 18;
                    break;

                case 7:
                    groundYoffset = DefaultGroundYoffset * 1.4f;
                    groundYoffsetChance = 0.8;
                    createMountainPeak(0.22f);
                    culture = TerrainCultureType.Mountain;

                    //switch (biom)
                    //{
                    //    case BiomTypeGreen:
                    //        color = Ground5;
                    //        break;
                    //    case BiomTypeDry:
                    //        color = Dry5;
                    //        break;
                    //}
                    influenceCost = 100;
                    break;

                default: throw new NotImplementedException();
            }
        }

        void createMountainPeak(float peakHeight)
        {
            mountainPeak = new float[WorldData.SubTileWidth, WorldData.SubTileWidth];

            ForXYLoop loop = new ForXYLoop(new IntVector2(WorldData.SubTileWidth));

            Vector2 center = new Vector2((WorldData.SubTileWidth - 1) * 0.5f);

            float half = WorldData.SubTileWidth * 0.5f;

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
