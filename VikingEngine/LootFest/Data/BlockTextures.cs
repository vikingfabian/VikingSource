using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LootFest.Data
{
    static class BlockTextures
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
            // missat light green cyan(520) -pure green

            new Material(6, 2, 2, 4, MaterialType.grass_green, true);
            new Material(8, 2, 10, 4, MaterialType.grass_mossy, true);
            new Material(14, 2, 16, 4, MaterialType.grass_red, true);
            new Material(20, 2, 22, 4, MaterialType.grass_yellow, true);
            new Material(26, 2, 28, 4, MaterialType.grass_dry, true);

            new Material(34, 4, MaterialType.desert_yellow);
            new Material(38, 4, MaterialType.desert_red);
            new Material(42, 4, MaterialType.desert_gray);
            new Material(46, 4, MaterialType.desert_white);
            new Material(50, 4, MaterialType.desert_black);

            new Material(54, 3, MaterialType.bricks_gray);
            new Material(57, 3, MaterialType.bricks_black);
            new Material(60, 3, MaterialType.bricks_red);
            //radbyte
            new Material(64, 3, MaterialType.bricks_sand);
            new Material(67, 3, MaterialType.bricks_brown);
            new Material(70, 3, MaterialType.block_sand);
            new Material(73, 3, MaterialType.block_black);
            new Material(76, 3, MaterialType.block_red);
            new Material(79, 3, MaterialType.block_gray);
            new Material(82, 3, MaterialType.block_blue);
            new Material(85, 3, MaterialType.block_white);

            new Material(6, 2, 96, 4, MaterialType.grass_green_flowers, true);
            new Material(8, 2, 100, 4, MaterialType.grass_mossy_flowers, true);
            new Material(14, 2, 104, 4, MaterialType.grass_red_flowers, true);
            new Material(20, 2, 108, 4, MaterialType.grass_yellow_flowers, true);
            new Material(26, 2, 112, 4, MaterialType.grass_dry_flowers, true);

            new Material(128, 4, MaterialType.stones_gray);
            new Material(132, 4, MaterialType.stones_black);
            new Material(136, 4, MaterialType.stones_blue);
            new Material(140, 4, MaterialType.stones_red);
            new Material(144, 4, MaterialType.stones_sand);

            new Material(148, 4, MaterialType.mossy_stones_blue);
            new Material(152, 4, MaterialType.mossy_stones_gray);

            new Material(156, 3, MaterialType.snow);
            new Material(160, 3, MaterialType.mountain_gray);
            new Material(163, 3, MaterialType.mountain_blue);
            new Material(166, 3, MaterialType.mountain_red);
            new Material(169, 3, MaterialType.mountain_sand);
            new Material(172, 3, MaterialType.mountain_black);

            new Material(176, 3, 175, 1, MaterialType.wood_brown, false);
            new Material(180, 3, 179, 1, MaterialType.wood_white, false);
            new Material(184, 3, 183, 1, MaterialType.wood_gray, false);
            new Material(188, 3, 187, 1, MaterialType.wood_purple, false);
            //
            new Material(192, 3, 195, 2, MaterialType.leaves_green, false);
            new Material(197, 3, 200, 2, MaterialType.leaves_red, false);
            new Material(202, 3, 205, 2, MaterialType.leaves_pink, false);
            //
            new Material(208, 3, 207, 1, MaterialType.wood_green, false);
            new Material(212, 3, 211, 1, MaterialType.wood_black, false);
            //
            new Material(215, 3, MaterialType.dirt_brown);
            new Material(218, 3, MaterialType.dirt_orange);
            new Material(221, 3, MaterialType.dirt_sand);
            //
            new Material(224, 3, 227, 2, MaterialType.leaves_dry, false);
            new Material(229, 3, 232, 2, MaterialType.leaves_blue, false);
            new Material(234, 3, 237, 2, MaterialType.leaves_yellow, false);
            //
            new Material(239, 3, MaterialType.dirt_blue);
            new Material(242, 3, MaterialType.dirt_red);
            new Material(245, 3, MaterialType.dirt_black);
            new Material(248, 3, MaterialType.dirt_purple);
            new Material(251, 3, MaterialType.dirty_snow);

            new Material(256, 3, 156, 3, MaterialType.leaves_snow, false);
            new Material(259, 1, MaterialType.carpet_green);
            new Material(260, 1, MaterialType.carpet_red);
            new Material(261, 1, MaterialType.carpet_brown);
            new Material(262, 1, MaterialType.carpet_blue);

 //___________

            new Material(288, 1, MaterialType.RGB_red);
            new Material(289, 1, MaterialType.RGB_yellow);
            new Material(290, 1, MaterialType.RGB_green);
            new Material(291, 1, MaterialType.RGB_Cyan);
            new Material(292, 1, MaterialType.RGB_Blue);
            new Material(293, 1, MaterialType.RGB_Magenta);

            new Material(294, 1, MaterialType.CMYK_red);
            new Material(295, 1, MaterialType.CMYK_yellow);
            new Material(296, 1, MaterialType.CMYK_green);
            new Material(297, 1, MaterialType.CMYK_Cyan);
            new Material(298, 1, MaterialType.CMYK_Blue);
            new Material(299, 1, MaterialType.CMYK_Magenta);

            new Material(300, 1, MaterialType.white);
            new Material(301, 2, MaterialType.gray_10);
            new Material(303, 2, MaterialType.gray_15);
            new Material(305, 2, MaterialType.gray_20);
            new Material(307, 2, MaterialType.gray_25);
            new Material(309, 2, MaterialType.gray_30);
            new Material(311, 2, MaterialType.gray_35);
            new Material(313, 2, MaterialType.gray_40);
            new Material(315, 2, MaterialType.gray_45);
            new Material(317, 2, MaterialType.gray_50);

            new Material(320, 2, MaterialType.gray_55);
            new Material(322, 2, MaterialType.gray_60);
            new Material(324, 2, MaterialType.gray_65);
            new Material(326, 2, MaterialType.gray_70);
            new Material(328, 2, MaterialType.gray_75);

            new Material(331, 2, MaterialType.gray_80);
            new Material(333, 2, MaterialType.gray_85);
            new Material(335, 2, MaterialType.gray_90);
            new Material(337, 1, MaterialType.black);

            new Material(338, 2, MaterialType.pastel_red);
            new Material(340, 2, MaterialType.pastel_red_orange);
            new Material(342, 2, MaterialType.pastel_yellow_orange);
            new Material(344, 2, MaterialType.pastel_yellow);
            
            new Material(346, 2, MaterialType.pastel_yellow_green);
            new Material(348, 2, MaterialType.pastel_green);
            new Material(350, 2, MaterialType.pastel_green_cyan);
            new Material(352, 2, MaterialType.pastel_cyan);
            new Material(354, 2, MaterialType.pastel_cyan_blue);
            new Material(356, 2, MaterialType.pastel_blue);
            new Material(358, 2, MaterialType.pastel_blue_violet);
            new Material(360, 2, MaterialType.pastel_violet);
            new Material(362, 2, MaterialType.pastel_violet_magenta);
            new Material(364, 2, MaterialType.pastel_magenta);
            new Material(366, 2, MaterialType.pastel_magenta_red);

            new Material(368, 2, MaterialType.light_red);
            new Material(370, 2, MaterialType.light_red_orange);
            new Material(372, 2, MaterialType.light_yellow_orange);
            new Material(374, 2, MaterialType.light_yellow);
            new Material(376, 2, MaterialType.light_pea_green);
            new Material(378, 2, MaterialType.light_yellow_green);
            new Material(380, 2, MaterialType.light_green);
            new Material(382, 2, MaterialType.pure_red_orange);

            //radbyte
            new Material(384, 2, MaterialType.pure_green_cyan);
            new Material(386, 2, MaterialType.pure_cyan);
            new Material(388, 2, MaterialType.pure_cyan_blue);     
            new Material(390, 2, MaterialType.pure_blue);
            new Material(392, 2, MaterialType.pure_blue_violet);
            new Material(394, 2, MaterialType.pure_violet);
            new Material(396, 2, MaterialType.pure_violet_magenta);
            new Material(398, 2, MaterialType.pure_magenta);
            new Material(400, 2, MaterialType.pure_magenta_red);

            new Material(402, 2, MaterialType.dark_red);
            new Material(404, 2, MaterialType.dark_red_orange);
                new Material(550, 2, MaterialType.dark_yellow_orange);//missad
            new Material(406, 2, MaterialType.dark_yellow);
            new Material(408, 2, MaterialType.dark_pea_green);
            new Material(410, 2, MaterialType.dark_yellow_green);
            new Material(412, 2, MaterialType.dark_green);
            new Material(414, 2, MaterialType.dark_green_cyan);
            new Material(416, 2, MaterialType.dark_cyan);
            new Material(418, 2, MaterialType.dark_cyan_blue);
                
            new Material(420, 2, MaterialType.dark_violet);
            new Material(422, 2, MaterialType.dark_violet_magenta);
            new Material(424, 2, MaterialType.dark_magenta);
            new Material(426, 2, MaterialType.dark_magenta_red);

            new Material(428, 2, MaterialType.pale_cool_brown);
            new Material(430, 2, MaterialType.light_cool_brown);
            new Material(432, 2, MaterialType.medium_cool_brown);
            new Material(434, 2, MaterialType.dark_cool_brown);
            new Material(436, 2, MaterialType.darker_cool_brown);
        
            new Material(438, 2, MaterialType.pale_warm_brown);
            new Material(440, 2, MaterialType.light_warm_brown);
            new Material(442, 2, MaterialType.medium_warm_brown);
            new Material(444, 2, MaterialType.dark_warm_brown);
            new Material(446, 2, MaterialType.darker_warm_brown);
        
            new Material(448, 2, MaterialType.pale_skin);
            new Material(450, 2, MaterialType.soft_yellow);
            new Material(452, 2, MaterialType.pink);
            new Material(454, 2, MaterialType.zombie_skin);

            //--
            new Material(456, 2, MaterialType.planks_bright_verti);
            new Material(458, 2, MaterialType.planks_bright_hori);
            new Material(460, 2, MaterialType.planks_red_verti);
            new Material(462, 2, MaterialType.planks_red_hori);
            new Material(464, 2, MaterialType.planks_brown_verti);
            new Material(466, 2, MaterialType.planks_brown_hori);
            new Material(468, 2, MaterialType.planks_black_verti);
            new Material(470, 2, MaterialType.planks_black_hori);

            new Material(473, 2, 472, 1, MaterialType.cactus_dark, false);
            new Material(476, 2, 475, 1, MaterialType.cactus_bright, false);
            new Material(478, 2, MaterialType.pastel_pea_green);

            new Material(480, 1, MaterialType.LetterA);
            new Material(481, 1, MaterialType.LetterB);
            new Material(482, 1, MaterialType.LetterC);
            new Material(483, 1, MaterialType.LetterD);
            new Material(484, 1, MaterialType.LetterE);
            new Material(485, 1, MaterialType.LetterF);
            new Material(486, 1, MaterialType.LetterG);
            new Material(487, 1, MaterialType.LetterH);
            new Material(488, 1, MaterialType.LetterI);
            new Material(489, 1, MaterialType.LetterJ);
            new Material(490, 1, MaterialType.LetterK);
            new Material(491, 1, MaterialType.LetterL);
            new Material(492, 1, MaterialType.LetterM);
            new Material(493, 1, MaterialType.LetterN);
            new Material(494, 1, MaterialType.LetterO);
            new Material(495, 1, MaterialType.LetterP);
            new Material(496, 1, MaterialType.LetterQ);
            new Material(497, 1, MaterialType.LetterR);
            new Material(498, 1, MaterialType.LetterS);
            new Material(499, 1, MaterialType.LetterT);
            new Material(500, 1, MaterialType.LetterU);
            new Material(501, 1, MaterialType.LetterV);
            new Material(502, 1, MaterialType.LetterW);
            new Material(503, 1, MaterialType.LetterX);
            new Material(504, 1, MaterialType.LetterY);
            new Material(505, 1, MaterialType.LetterZ);
            new Material(506, 1, MaterialType.LetterDot);
            
            new Material(507, 1, MaterialType.LetterQuest);
            new Material(508, 1, MaterialType.LetterExpress);
            new Material(509, 1, MaterialType.Letter0);
            new Material(510, 1, MaterialType.Letter1);
            new Material(511, 1, MaterialType.Letter2);
            new Material(512, 1, MaterialType.Letter3);
            new Material(513, 1, MaterialType.Letter4);
            new Material(514, 1, MaterialType.Letter5);
            new Material(515, 1, MaterialType.Letter6);
            new Material(516, 1, MaterialType.Letter7);
            new Material(517, 1, MaterialType.Letter8);
            new Material(518, 1, MaterialType.Letter9);
            new Material(519, 1, MaterialType.empty_letter);

            //missade
            new Material(520, 2, MaterialType.light_green_cyan);// missat light green cyan(520) -pure green
            new Material(522, 2, MaterialType.light_cyan);
            new Material(524, 2, MaterialType.light_cyan_blue);
            new Material(526, 2, MaterialType.light_blue);
            new Material(528, 2, MaterialType.light_blue_violet);
            new Material(530, 2, MaterialType.light_violet);
            new Material(532, 2, MaterialType.light_violet_magenta);
            new Material(534, 2, MaterialType.light_magenta);
            new Material(536, 2, MaterialType.light_magenta_red);
            new Material(538, 2, MaterialType.pure_red);
            new Material(540, 2, MaterialType.pure_yellow_orange);
            new Material(542, 2, MaterialType.pure_yellow);
            new Material(544, 2, MaterialType.pure_pea_green);
            new Material(546, 2, MaterialType.pure_yellow_green);
            new Material(548, 2, MaterialType.pure_green);//--

            new Material(552, 2, MaterialType.dark_blue);//missad
            new Material(454, 2, MaterialType.dark_blue_violet);//missad

            new Material(556, 2, MaterialType.darker_red);
            new Material(558, 2, MaterialType.darker_red_orange);
            new Material(560, 2, MaterialType.darker_yellow_orange);
            new Material(562, 2, MaterialType.darker_yellow);
            new Material(564, 2, MaterialType.darker_pea_green);
            new Material(566, 2, MaterialType.darker_yellow_green);
            new Material(568, 2, MaterialType.darker_green);
            new Material(570, 2, MaterialType.darker_green_cyan);
            new Material(572, 2, MaterialType.darker_cyan);
            new Material(574, 2, MaterialType.darker_cyan_blue);
            new Material(576, 2, MaterialType.darker_blue);
            new Material(578, 2, MaterialType.darker_blue_violet);
            new Material(580, 2, MaterialType.darker_violet);
            new Material(582, 2, MaterialType.darker_violet_magenta);
            new Material(584, 2, MaterialType.darker_magenta);
            new Material(586, 2, MaterialType.darker_magenta_red);

            new Material(640, 2, MaterialType.water);
            new Material(642, 2, MaterialType.lava);

            // USED IN DEVMODE ONLY
            new Material(63, 1, MaterialType.Martin);
            new Material(319, 1, MaterialType.Fabian);

            new Material(509, 1, MaterialType.Joint0);
            new Material(510, 1, MaterialType.Joint1);
            new Material(511, 1, MaterialType.Joint2);
            new Material(512, 1, MaterialType.Joint3);
            new Material(513, 1, MaterialType.Joint4);
            new Material(514, 1, MaterialType.Joint5);
            new Material(515, 1, MaterialType.Joint6);
            new Material(516, 1, MaterialType.Joint7);
            new Material(517, 1, MaterialType.Joint8);
            new Material(518, 1, MaterialType.Joint9);
            // END


            new Material(293, 1, 291, 1, MaterialType.AntiBlock, false);

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

        public Material(int startTile, int numTiles, MaterialType material)
            :this(startTile, numTiles, startTile, numTiles, material, false)
        {
        }
        public Material(int startSideTile, int numSideTiles, int startTopTile, int numTopTiles, MaterialType material, bool iconFromTop)
        {
            TopTiles = new MaterialTiles(startTopTile, numTopTiles);
            SideTiles = new MaterialTiles(startSideTile, numSideTiles);

            LfRef.Images.AddSpriteName(BlockTextures.MaterialTile(material), iconFromTop? startTopTile : startSideTile);
            BlockTextures.Materials[(int)material] = this;
        }

       
    }
    
    struct MaterialTiles
    {
        public int startTile;
        int numTiles;

        public MaterialTiles(int startIx, int numTiles)
        {
            this.numTiles = Bound.Min(numTiles, 1);
            startTile = startIx;
        }
        public int GetStandardTile()
        {
            return startTile;
        }
        public int GetRandomTile(IntVector3 pos)
        {
            int posAdd = Math.Abs(pos.X * 3 + pos.Y * 7 + pos.Z * 31);
            int val = posAdd % BlockTextures.RandomTileNumValues;
            return startTile + BlockTextures.RandomTile_NumVariants_Value[Math.Max(numTiles - 1, 0), val];
        }
    }

    public enum MaterialType
    {
        NO_MATERIAL,
        RGB_red,
        RGB_yellow,
        RGB_green,
        RGB_Cyan,
        RGB_Blue,
        RGB_Magenta,

        CMYK_red,
        CMYK_yellow,
        CMYK_green,
        CMYK_Cyan,
        CMYK_Blue,
        CMYK_Magenta,

        white,
        gray_10,
        gray_15,
        gray_20,
        gray_25,
        gray_30,
        gray_35,
        gray_40,
        gray_45,
        gray_50,
        gray_55,
        gray_60,
        gray_65,
        gray_70,
        gray_75,
        gray_80,
        gray_85,
        gray_90,
        black,

        pastel_red,
        pastel_red_orange,
        pastel_yellow_orange,
        pastel_yellow,
        pastel_pea_green,
        pastel_yellow_green,
        pastel_green,
        pastel_green_cyan,
        pastel_cyan,
        pastel_cyan_blue,
        pastel_blue,
        pastel_blue_violet,
        pastel_violet,
        pastel_violet_magenta,
        pastel_magenta,
        pastel_magenta_red,

        light_red,
        light_red_orange,
        light_yellow_orange,
        light_yellow,
        light_pea_green,
        light_yellow_green,
        light_green,
        light_green_cyan,
        light_cyan,
        light_cyan_blue,
        light_blue,
        light_blue_violet,
        light_violet,
        light_violet_magenta,
        light_magenta,
        light_magenta_red,

        pure_red,
        pure_red_orange,
        pure_yellow_orange,
        pure_yellow,
        pure_pea_green,
        pure_yellow_green,
        pure_green,
        pure_green_cyan,
        pure_cyan,
        pure_cyan_blue,
        pure_blue,
        pure_blue_violet,
        pure_violet,
        pure_violet_magenta,
        pure_magenta,
        pure_magenta_red,

        dark_red,
        dark_red_orange,
        dark_yellow_orange,
        dark_yellow,
        dark_pea_green,
        dark_yellow_green,
        dark_green,
        dark_green_cyan,
        dark_cyan,
        dark_cyan_blue,
        dark_blue,
        dark_blue_violet,
        dark_violet,
        dark_violet_magenta,
        dark_magenta,
        dark_magenta_red,

        darker_red,
        darker_red_orange,
        darker_yellow_orange,
        darker_yellow,
        darker_pea_green,
        darker_yellow_green,
        darker_green,
        darker_green_cyan,
        darker_cyan,
        darker_cyan_blue,
        darker_blue,
        darker_blue_violet,
        darker_violet,
        darker_violet_magenta,
        darker_magenta,
        darker_magenta_red,

        pale_cool_brown,
        light_cool_brown,
        medium_cool_brown,
        dark_cool_brown,
        darker_cool_brown,
        
        pale_warm_brown,
        light_warm_brown,
        medium_warm_brown,
        dark_warm_brown,
        darker_warm_brown,
        
        pale_skin,
        soft_yellow,
        pink,
        zombie_skin,
        //red_orange,

        grass_green,
        grass_mossy,
        grass_red,
        grass_yellow,
        grass_dry,
        desert_yellow,
        desert_red,
        desert_gray,
        desert_white,
        desert_black,
        bricks_gray,
        bricks_black,
        bricks_red,
        bricks_sand,
        bricks_brown,
        block_sand,
        block_black,
        block_red,
        block_gray,
        block_blue,
        block_white,

        grass_green_flowers,
        grass_mossy_flowers,
        grass_red_flowers,
        grass_yellow_flowers,
        grass_dry_flowers,

        stones_gray,
        stones_black,
        stones_blue,
        stones_red,
        stones_sand,

        mossy_stones_blue,
        mossy_stones_gray,

        snow,
        dirty_snow,


        mountain_gray,
        mountain_blue,
        mountain_red,
        mountain_sand,
        mountain_black,

        wood_brown,
        wood_white,
        wood_gray,
        wood_purple,
        wood_green,
        wood_black,

        leaves_green,
        leaves_red,
        leaves_pink,
        leaves_dry,
        leaves_blue,
        leaves_yellow,
        leaves_snow,

        dirt_brown,
        dirt_orange,
        dirt_sand,
        dirt_blue,
        dirt_red,
        dirt_black,
        dirt_purple,

        carpet_green,
        carpet_red,
        carpet_brown,
        carpet_blue,

        planks_bright_verti,
        planks_bright_hori,
        planks_red_verti,
        planks_red_hori,
        planks_brown_verti,
        planks_brown_hori,
        planks_black_verti,
        planks_black_hori,

        cactus_dark,
        cactus_bright,

        LetterA,
        LetterB,
        LetterC,
        LetterD,
        LetterE,
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

        water,
        lava,
        Martin,
        Fabian,
        temp,

        Joint0,
        Joint1,
        Joint2,
        Joint3,
        Joint4,
        Joint5,
        Joint6,
        Joint7,
        Joint8,
        Joint9,
        AntiBlock,
        NUM 
    }
}
