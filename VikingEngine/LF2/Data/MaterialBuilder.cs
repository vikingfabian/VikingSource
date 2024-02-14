using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.Data
{
    static class MaterialBuilder
    {
        public static Material[] Materials;

        static readonly List<byte>[] Commoness = new List<byte>[]
        {
            new List<byte> { 1 },
            new List<byte> { 3, 1 },
            new List<byte> { 3, 1, 1 },
            new List<byte> { 4, 2, 1, 1 },
            new List<byte> { 4, 2, 2, 1, 1 },
            new List<byte> { 4, 3, 2, 2, 1, 1 },
            new List<byte> { 4, 4, 3, 2, 2, 1, 1 }//7
        };

        public const int RandomTileNumValues = 101;
        const int RandomTileMaxVariants = 7;
        public static byte[,] RandomTile_NumVariants_Value;

        public static void InitMaterials()
        { 
            //List<AbsDraw> generatedImages = new List<AbsDraw>();
            Materials = new Material[(int)MaterialType.NUM];
            
            Materials[(int)MaterialType.grass] = new Material(228,2, 224,4, MaterialType.grass, true);//
            Materials[(int)MaterialType.dirt] = new Material(113, 4, MaterialType.dirt, true);//
            Materials[(int)MaterialType.wood_growing] = new Material(117, 3, 120, 1, MaterialType.wood_growing, false);//
            Materials[(int)MaterialType.wood] = new Material(212, 2, 214, 2, MaterialType.wood, false);//
            Materials[(int)MaterialType.leaves] = new Material(256, 4, 260, 2, MaterialType.leaves, false);//5//
            Materials[(int)MaterialType.red_bricks] = new Material(10, 3, 13, 1, MaterialType.red_bricks, false);//
            Materials[(int)MaterialType.red_roof] = new Material(147, 2, MaterialType.red_roof, true);//
            Materials[(int)MaterialType.iron] = new Material(370, 1, MaterialType.iron, true);//
            Materials[(int)MaterialType.gold] = new Material(234, 1, MaterialType.gold, true);//
            Materials[(int)MaterialType.copper] = new Material(15, 2, MaterialType.copper, true);//10//
            Materials[(int)MaterialType.bone] = new Material(51, 2, MaterialType.bone, true);//
            Materials[(int)MaterialType.leather] = new Material(145, 2, MaterialType.leather, true);//
            Materials[(int)MaterialType.blue_gray] = new Material(241, 2, MaterialType.blue_gray, true);//
            Materials[(int)MaterialType.red] = new Material(295, 2, MaterialType.red, true);//
            Materials[(int)MaterialType.yellow] = new Material(232, 2, MaterialType.yellow, true);//15
            Materials[(int)MaterialType.skin] = new Material(4, 1, MaterialType.skin, true);//
            Materials[(int)MaterialType.flat_black] = new Material(278, 1, MaterialType.flat_black, true);//
            Materials[(int)MaterialType.burnt_ground] = new Material(273, 4, MaterialType.burnt_ground, true);//
            Materials[(int)MaterialType.sand] = new Material(180, 4, MaterialType.sand, true);//
            Materials[(int)MaterialType.water] = new Material(375, 1, MaterialType.water, true);//20
            Materials[(int)MaterialType.cobble_stone] = new Material(53, 6, MaterialType.cobble_stone, true);//
            Materials[(int)MaterialType.forest] = new Material(288, 4, MaterialType.forest, true);//
            Materials[(int)MaterialType.cactus] = new Material(293, 1, 292, 1, MaterialType.cactus, false);//
            Materials[(int)MaterialType.dark_skin] = new Material(177, 3, MaterialType.dark_skin, true);//
            Materials[(int)MaterialType.black] = new Material(279, 2, MaterialType.black, true);//25
            Materials[(int)MaterialType.red_brown] = new Material(149, 2, MaterialType.red_brown, true);//
            Materials[(int)MaterialType.brown] = new Material(151, 2, MaterialType.brown, true);//
            Materials[(int)MaterialType.blonde] = new Material(5, 2, MaterialType.blonde, true);//
            Materials[(int)MaterialType.white] = new Material(49, 2, MaterialType.white, true);//
            Materials[(int)MaterialType.mossy_green] = new Material(356, 2, MaterialType.mossy_green, true);//30
            Materials[(int)MaterialType.blue] = new Material(371, 2, MaterialType.blue, true);//
            Materials[(int)MaterialType.dark_blue] = new Material(373, 2, MaterialType.dark_blue, true);//
            Materials[(int)MaterialType.gray_bricks] = new Material(21, 3, 24, 1, MaterialType.gray_bricks, false); //    
            Materials[(int)MaterialType.lava] = new Material(297, 1, MaterialType.lava, true);//
            Materials[(int)MaterialType.orc_skin] = new Material(324, 2, MaterialType.orc_skin, true);//35//
            Materials[(int)MaterialType.purple_skin] = new Material(433, 2, MaterialType.purple_skin, true);//
            Materials[(int)MaterialType.red_orange] = new Material(298, 3, MaterialType.red_orange, true);//
            Materials[(int)MaterialType.bronze] = new Material(266, 3, MaterialType.bronze, true);//
            Materials[(int)MaterialType.lightning] = new Material(369, 1, MaterialType.lightning, true);//
            Materials[(int)MaterialType.desert_sand] = new Material(0, 4, MaterialType.desert_sand, true);//40//

            Materials[(int)MaterialType.sand_stone] = new Material(36, 4, MaterialType.sand_stone, true);//
            Materials[(int)MaterialType.sand_stone_bricks] = new Material(32, 4, MaterialType.sand_stone_bricks, false);//
            Materials[(int)MaterialType.marble] = new Material(17, 4, MaterialType.marble, true);//
            const int Pink = 436;
            Materials[(int)MaterialType.pink] = new Material(Pink, 2, MaterialType.pink, true);//
            Materials[(int)MaterialType.violet] = new Material(465, 2, MaterialType.violet, true);//
            Materials[(int)MaterialType.orange] = new Material(235, 2, MaterialType.orange, true);//
            Materials[(int)MaterialType.cyan] = new Material(501, 2, MaterialType.cyan, true);//
            const int DarkGray = 311;
            Materials[(int)MaterialType.dark_gray] = new Material(DarkGray, 2, MaterialType.dark_gray, true);//
            Materials[(int)MaterialType.light_gray] = new Material(26, 2, MaterialType.light_gray, true);//
            Materials[(int)MaterialType.zombie_skin] = new Material(192, 2, MaterialType.zombie_skin, true);//
            Materials[(int)MaterialType.mossy_stone] = new Material(352, 4, MaterialType.mossy_stone, true);//

            Materials[(int)MaterialType.flowers] = new Material(228, 2, 320, 4, MaterialType.flowers, false);//
            Materials[(int)MaterialType.pink_heart] = new Material(438, 1, Pink, 2, MaterialType.pink_heart, false);//
            Materials[(int)MaterialType.pirate] = new Material(310, 1, DarkGray, 2, MaterialType.pirate, false);//
            Materials[(int)MaterialType.Gamefarm] = new Material(544, 4, MaterialType.Gamefarm, false);//


            Materials[(int)MaterialType.flat_skin] = new Material(7, 1, MaterialType.flat_skin, true);
            Materials[(int)MaterialType.flat_sand] = new Material(8, 1, MaterialType.flat_sand, true);
            Materials[(int)MaterialType.flat_white] = new Material(48, 1, MaterialType.flat_white, true);
            Materials[(int)MaterialType.flat_green] = new Material(449, 1, MaterialType.flat_green, true);
            Materials[(int)MaterialType.flat_mossy_green] = new Material(389, 1, MaterialType.flat_mossy_green, true);
            Materials[(int)MaterialType.flat_light_green] = new Material(388, 1, MaterialType.flat_light_green, true);
            Materials[(int)MaterialType.flat_yellow] = new Material(231, 1, MaterialType.flat_yellow, true);//232, 2, MaterialType.Yellow);
            Materials[(int)MaterialType.flat_orange] = new Material(331, 1, MaterialType.flat_orange, true);
            Materials[(int)MaterialType.flat_red] = new Material(330, 1, MaterialType.flat_red, true);
            Materials[(int)MaterialType.flat_red_orange] = new Material(331, 1, MaterialType.flat_red_orange, true);
            Materials[(int)MaterialType.flat_purple] = new Material(498, 1, MaterialType.flat_purple, true);
            Materials[(int)MaterialType.flat_magenta] = new Material(499, 1, MaterialType.flat_magenta, true);
            Materials[(int)MaterialType.flat_blue] = new Material(337, 1, MaterialType.flat_blue, true);
            Materials[(int)MaterialType.flat_dark_blue] = new Material(338, 1, MaterialType.flat_dark_blue, true);
            Materials[(int)MaterialType.flat_sky_blue] = new Material(339, 1, MaterialType.flat_sky_blue, true);
            Materials[(int)MaterialType.flat_cyan] = new Material(340, 1, MaterialType.flat_cyan, true);
            Materials[(int)MaterialType.flat_pink] = new Material(439, 1, MaterialType.flat_pink, true);
            Materials[(int)MaterialType.flat_dark_gray] = new Material(283, 1, MaterialType.flat_dark_gray, true);
            Materials[(int)MaterialType.flat_gray] = new Material(284, 1, MaterialType.flat_gray, true);
            Materials[(int)MaterialType.flat_brown] = new Material(184, 1, MaterialType.flat_brown, true);
            Materials[(int)MaterialType.flat_dark_brown] = new Material(185, 1, MaterialType.flat_dark_brown, true);
            Materials[(int)MaterialType.flat_light_brown] = new Material(186, 1, MaterialType.flat_light_brown, true);

            Materials[(int)MaterialType.gray] = new Material(281, 2, MaterialType.gray, true);
            Materials[(int)MaterialType.light_brown] = new Material(121, 2, MaterialType.light_brown, true);
            Materials[(int)MaterialType.magenta] = new Material(467, 2, MaterialType.magenta, true);
            Materials[(int)MaterialType.green] = new Material(416, 2, MaterialType.green, true);
            Materials[(int)MaterialType.dark_green] = new Material(418, 2, MaterialType.dark_green, true);
            Materials[(int)MaterialType.yellow_green] = new Material(420, 2, MaterialType.yellow_green, true);
            Materials[(int)MaterialType.gray_wood] = new Material(313, 3, 316, 1, MaterialType.gray_wood, false);
            Materials[(int)MaterialType.black_wood] = new Material(249, 3, 252, 1, MaterialType.black_wood, false);
            Materials[(int)MaterialType.light_red] = new Material(327, 1, MaterialType.light_red, true);
            Materials[(int)MaterialType.patterned_stone] = new Material(90, 4, MaterialType.patterned_stone, true);
            Materials[(int)MaterialType.patterned_mossy_stone] = new Material(384, 4, MaterialType.patterned_mossy_stone, true);
            Materials[(int)MaterialType.patterned_marble] = new Material(81, 4, MaterialType.patterned_marble, true);
            Materials[(int)MaterialType.patterned_sand_stone] = new Material(123, 4, MaterialType.patterned_sand_stone, false);
            Materials[(int)MaterialType.patterned_wood] = new Material(216, 3, 219, 1, MaterialType.patterned_wood, false);
            Materials[(int)MaterialType.patterned_growing_wood] = new Material(153, 3, 156, 1, MaterialType.patterned_growing_wood, false);
            Materials[(int)MaterialType.patterned_gray_wood] = new Material(285, 3, 319, 1, MaterialType.patterned_gray_wood, false);
            Materials[(int)MaterialType.red_carpet] = new Material(329, 1, MaterialType.red_carpet, true);
            Materials[(int)MaterialType.blue_carpet] = new Material(503, 1, MaterialType.blue_carpet, true);
            Materials[(int)MaterialType.brown_carpet] = new Material(176, 1, MaterialType.brown_carpet, true);
            Materials[(int)MaterialType.green_carpet] = new Material(448, 1, MaterialType.green_carpet, true);
            Materials[(int)MaterialType.stony_grass] = new Material(228, 2, 194, 4, MaterialType.stony_grass, true);
            Materials[(int)MaterialType.stony_forest] = new Material(288, 4, 450, 4, MaterialType.stony_forest, true);
            Materials[(int)MaterialType.red_stones] = new Material(208, 4, MaterialType.red_stones, true);
            Materials[(int)MaterialType.blue_stones] = new Material(376, 4, MaterialType.blue_stones, true);
            Materials[(int)MaterialType.runes] = new Material(85, 5, MaterialType.runes, false);
            Materials[(int)MaterialType.silver] = new Material(506, 1, MaterialType.silver, true);
            Materials[(int)MaterialType.mithril] = new Material(504, 2, MaterialType.mithril, true);
            Materials[(int)MaterialType.green_gem] = new Material(269, 1, MaterialType.green_gem, false);
            Materials[(int)MaterialType.red_gem] = new Material(271, 1, MaterialType.red_gem, false);
            Materials[(int)MaterialType.blue_gem] = new Material(270, 1, MaterialType.blue_gem, false);
            Materials[(int)MaterialType.blue_roof] = new Material(380, 2, MaterialType.blue_roof, true);
            Materials[(int)MaterialType.straw] = new Material(237, 2, MaterialType.straw, true);
            Materials[(int)MaterialType.stone] = new Material(58, 5, MaterialType.stone, true);

            Materials[(int)MaterialType.TM_grass] = new Material(483, 1, 481, 1, MaterialType.TM_grass, true);
            Materials[(int)MaterialType.TM_dirt] = new Material(253, 1, MaterialType.TM_dirt, true);
            Materials[(int)MaterialType.TM_leaves] = new Material(482, 1, MaterialType.TM_leaves, true);
            Materials[(int)MaterialType.TM_braided_leaves] = new Material(480, 1, MaterialType.TM_braided_leaves, true);
            Materials[(int)MaterialType.TM_wood] = new Material(254, 1, 255, 1, MaterialType.TM_wood, false);
            Materials[(int)MaterialType.TM_rock] = new Material(317, 1, MaterialType.TM_rock, true);
            Materials[(int)MaterialType.TM_TNT] = new Material(556, 1, 557, 1, MaterialType.TM_TNT, false);
            Materials[(int)MaterialType.TM_work_bench] = new Material(553, 3, 552, 1, MaterialType.TM_work_bench, false);
            Materials[(int)MaterialType.TM_shop] = new Material(157, 3, 127, 1, MaterialType.TM_shop, false);
            Materials[(int)MaterialType.total_invader] = new Material(548, 4, 278, 1, MaterialType.total_invader, false);
            Materials[(int)MaterialType.temp] = new Material(548, 4, 278, 1, MaterialType.temp, false);

            Materials[(int)MaterialType.AntiBlock] = new Material(498, 1, MaterialType.AntiBlock, true);
            const int EmptyLetter = 138;
            int ix = 64;
            for (MaterialType type = MaterialType.LetterA; type <= MaterialType.LetterP; type++)
            {
                Materials[(int)type] = new Material(ix, 1, EmptyLetter, 1, type, false);
                ix++;
            }
            ix = 96;
            for (MaterialType type = MaterialType.LetterQ; type <= MaterialType.LetterZ; type++)
            {
                Materials[(int)type] = new Material(ix, 1, EmptyLetter, 1, type, false);
                ix++;
            }
            Materials[(int)MaterialType.LetterDot] = new Material(ix, 1, EmptyLetter, 1, MaterialType.LetterDot, false); ix++;
            Materials[(int)MaterialType.LetterQuest] = new Material(ix, 1, EmptyLetter, 1, MaterialType.LetterQuest, false); ix++;
            Materials[(int)MaterialType.LetterExpress] = new Material(ix, 1, EmptyLetter, 1, MaterialType.LetterExpress, false);
            ix = 128;
            for (MaterialType type = MaterialType.Letter0; type <= MaterialType.Letter9; type++)
            {
                Materials[(int)type] = new Material(ix, 1, EmptyLetter, 1, type, false);
                ix++;
            }
            Materials[(int)MaterialType.empty_letter] = new Material(EmptyLetter, 1, MaterialType.empty_letter, false);

            

            RandomTile_NumVariants_Value = new byte[RandomTileMaxVariants, RandomTileNumValues];
            for (int variants = 0; variants < RandomTileMaxVariants; variants++)
            {
                List<byte> values = new List<byte>();
                for (byte com = 0; com < Commoness[variants].Count; com++)
                {
                    for (int val = 0; val < Commoness[variants][com]; val++)
                    {
                        values.Add(com);
                    }
                }
                for (int i = 0; i < RandomTileNumValues; i++)
                {
                    RandomTile_NumVariants_Value[variants, i] = values[Ref.rnd.Int(values.Count)];
                }
            }
        }

        public static void InitRenderTarget()
        {
            //Warning! cant be threaded
            LootfestLib.Images.AddImagesToTarget(new List<AbsDraw> { new Image(SpriteName.TextureVoxelFace, Vector2.Zero, 
                new Vector2(LoadTiles.BlockTextureWidth), ImageLayers.Background4) }); 
        }

        public static SpriteName MaterialTile(byte material)
        {
            return (SpriteName)(material + (int)SpriteName.LFMaterialStart);
        }
        public static SpriteName MaterialTile(MaterialType material)
        {
            return MaterialTile((byte)material);
        }
    }
    struct Material
    {
        public MaterialTiles TopTiles;
        public MaterialTiles SideTiles;
        
        static readonly List<int> OneType = new List<int> { 1 };

        public Material(int startTile, int numTiles, MaterialType material, bool iconFromTop)
            :this(startTile, numTiles, startTile, numTiles, material, iconFromTop)
        {
        }
        public Material(int startSideTile, int numSideTiles, int startTopTile, int numTopTiles, MaterialType material, bool iconFromTop)
        {
            TopTiles = new MaterialTiles(startTopTile, numTopTiles);
            SideTiles = new MaterialTiles(startSideTile, numSideTiles);

            LootfestLib.Images.AddSpriteName(MaterialBuilder.MaterialTile(material), iconFromTop? startTopTile : startSideTile);
        }

       
    }
    
    struct MaterialTiles
    {
        
        public int startTile;
        int numTiles;

        public MaterialTiles(int startIx, int numTiles)
        {
            this.numTiles = Bound.Min(numTiles, 1);
#if WINDOWS
            if (numTiles < 1)
                throw new Exception();
#endif
           
            startTile = startIx;
            
        }
        public int GetStandardTile()
        {
            return startTile;
        }
        public int GetRandomTile(IntVector3 pos)
        {
            int posAdd = Math.Abs(pos.X * 3 + pos.Y * 7 + pos.Z * 31);
            int val = posAdd % MaterialBuilder.RandomTileNumValues;
            return startTile + MaterialBuilder.RandomTile_NumVariants_Value[numTiles - 1, val];
        }
    }
    public enum MaterialType
    {
        NO_MATERIAL,
        grass,
        dirt,
        wood_growing,
        wood,
        leaves,//5
        red_bricks,
        red_roof,
        iron,
        gold,
        copper,//10
        bone,
        leather,
        blue_gray,
        red,
        yellow,//15
        skin,
        flat_black,
        burnt_ground,
        sand,
        water,//20
        cobble_stone,
        forest,
        cactus,
        dark_skin,
        black,//25
        red_brown,
        brown,
        blonde,
        white,
        mossy_green,//30
        blue,
        dark_blue,
        gray_bricks,
        lava,
        orc_skin,//35
        purple_skin,
        red_orange,
        bronze,
        lightning,
        desert_sand,//40
        sand_stone,
        sand_stone_bricks,
        marble,
        pink,
        violet,//45
        orange,
        cyan,
        dark_gray,
        light_gray,
        zombie_skin,//50
        mossy_stone,
        flowers,
        pink_heart,
        pirate,
        Gamefarm,//55
        LetterA,
        LetterB,
        LetterC,
        LetterD,
        LetterE,//60
        LetterF,
        LetterG,
        LetterH,
        LetterI,
        LetterJ,
        LetterK,
        LetterL,
        LetterM,
        LetterN,
        LetterO,
        LetterP,
        LetterQ,
        LetterR,
        LetterS,
        LetterT,
        LetterU,
        LetterV,
        LetterW,
        LetterX,
        LetterY,
        LetterZ,
        Letter0,
        Letter1,
        Letter2,
        Letter3,
        Letter4,
        Letter5,
        Letter6,
        Letter7,
        Letter8,
        Letter9,
        LetterDot,
        empty_letter,
        LetterQuest,
        LetterExpress,
        flat_skin,
        flat_sand,
        flat_white,
        flat_green,
        flat_mossy_green,
        flat_light_green,
        flat_yellow,
        flat_orange,
        flat_red,
        flat_red_orange,
        flat_purple,
        flat_magenta,
        flat_blue,
        flat_dark_blue,
        flat_sky_blue,
        flat_cyan,
        flat_pink,
        flat_dark_gray,
        flat_gray,
        flat_brown,
        flat_dark_brown,
        flat_light_brown,

        magenta,
        green,
        dark_green,
        yellow_green,
        light_red,
        light_brown,
        gray,
        gray_wood,
        black_wood,
        patterned_stone,
        patterned_mossy_stone,
        patterned_marble, 
        patterned_sand_stone,
        patterned_wood,
        patterned_growing_wood,
        patterned_gray_wood,
        red_carpet,
        blue_carpet,
        brown_carpet,
        green_carpet,
        stony_grass,
        stony_forest,
        red_stones,
        blue_stones,
        runes,
        silver,
        mithril,
        green_gem,
        red_gem,
        blue_gem,
        blue_roof,
        straw,
        stone,//59nya

        TM_grass,
        TM_dirt,
        TM_leaves,
        TM_braided_leaves,
        TM_wood,
        TM_rock,
        TM_TNT,
        TM_work_bench,
        TM_shop,
        total_invader,//69
        temp,

        AntiBlock = Byte.MaxValue,//Voxels.VoxelLib.AntiBlock,
        NUM 
    }
}
