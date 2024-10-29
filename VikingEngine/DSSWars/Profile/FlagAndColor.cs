using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Net;
using System.Text;
using VikingEngine.DSSWars.Data;
using VikingEngine.HUD;
using VikingEngine.LootFest.GO.WeaponAttack.ItemThrow;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.DSSWars
{
    class FlagAndColor
    {
        public static readonly int ColorCount = (int)ProfileColorType.NUM;
        static readonly ColorRange AiColorRange = new ColorRange(new Color(new Vector3(0.1f)), new Color(new Vector3(0.9f)));

        public static AppearanceMaterial
            SkinCol, HairCol, MainCol, AltMainCol, DetailCol1, DetailCol2;

        public static void Init()
        {
            //Detta är färgerna som ersätts

            SkinCol = new AppearanceMaterial(Color.Gray);
            HairCol = new AppearanceMaterial(Color.Brown);

            MainCol = new AppearanceMaterial(new Color(65, 74, 129)); //Blå
            AltMainCol = new AppearanceMaterial(new Color(25, 54, 109)); //Mörk Blå

            DetailCol1 = new AppearanceMaterial(new Color(133, 78, 65));//Röd
            DetailCol2 = new AppearanceMaterial(new Color(65, 133, 69));//Grön


        }

        int index;
        public Color col0_Main;
        public Color col1_Detail1;
        public Color col2_Detail2;

        public Color col3_Skin;
        public Color col4_Hair;

        //public Color[] colors = new Color[ColorCount];
        public ushort[] blockColors;
        public FlagDesign flagDesign;
        public List<BlockHDPair> modelColorReplace;
        FactionFlavorType factionFlavorType = FactionFlavorType.Other;


        public FlagAndColor(FactionType factiontype, int index, WorldMetaData worldMeta)
        {
            this.index = index;
            

            switch (factiontype)
            {
                case FactionType.DefaultAi:
                    {
                        var color1 = AiColorRange.GetRandom(worldMeta.objRnd);
                        var color2 = AiColorRange.GetRandom(worldMeta.objRnd);

                        col0_Main = color1;
                        col1_Detail1 = color2;
                        col2_Detail2 = Color.Gray;

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = arraylib.RandomListMember(FlagDesign.AiBanner, worldMeta.objRnd);
                    }
                    break;


                case FactionType.Player:
                    {
                        switch (index)
                        {
                            case 0:
                                col0_Main = Color.Blue;
                                col1_Detail1 = Color.Yellow;
                                col2_Detail2 = Color.Orange;
                                break;

                            case 1:
                                col0_Main = Color.Red;
                                col1_Detail1 = Color.MediumPurple;
                                col2_Detail2 = Color.Blue;
                                break;

                            case 2:
                                col0_Main = Color.Green;
                                col1_Detail1 = Color.Yellow;
                                col2_Detail2 = Color.YellowGreen;
                                break;

                            case 3:
                                col0_Main = Color.Orange;
                                col1_Detail1 = Color.Pink;
                                col2_Detail2 = Color.Brown;
                                break;

                            case 4:
                                col0_Main = new Color(63, 79, 63);
                                col1_Detail1 = new Color(0, 0, 0);
                                col2_Detail2 = new Color(220, 213, 222);

                                flagDesign = FlagDesign.PlayerGriffin;
                                break;

                            case 5:
                                col0_Main = new Color(139, 2, 2);
                                col1_Detail1 = new Color(181, 133, 94);
                                col2_Detail2 = new Color(220, 213, 222);

                                flagDesign = FlagDesign.PlayerGriffin;
                                break;

                            case 6:
                                col0_Main = new Color(46, 73, 94);
                                col1_Detail1 = new Color(99, 175, 174);
                                col2_Detail2 = new Color(243, 232, 191);

                                flagDesign = FlagDesign.PlayerGriffin;
                                break;

                            case 7:
                                col0_Main = new Color(98, 42, 52);
                                col1_Detail1 = new Color(205, 193, 68);
                                col2_Detail2 = new Color(240, 193, 193);

                                flagDesign = FlagDesign.PlayerGriffin;
                                break;



                            default:
                                col0_Main = Color.DarkGray;
                                col1_Detail1 = Color.Brown;
                                col2_Detail2 = Color.LightGray;
                                break;
                        }

                        col3_Skin = Color.Beige;
                        col4_Hair = Color.Brown;

                        if (flagDesign == null)
                        {
                            flagDesign = new FlagDesign();
                        }
                    }
                    break;

                case FactionType.UnitedKingdom:
                    col0_Main = new Color(248,248,216);
                    col1_Detail1 = new Color(120, 72, 8);
                    col2_Detail2 = new Color(120, 40, 8);

                    col3_Skin = Color.LightGray;
                    col4_Hair = Color.DarkGray;

                    flagDesign = new FlagDesign(new byte[]
                    {
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 2, 0, 2, 2, 0, 2, 0, 0, 0, 0, 0,
                        0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                        0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
                        0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
                        0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
                        0, 2, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 2, 0,
                        0, 0, 2, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 2, 0, 0,
                        0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
                        0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0,
                        0, 0, 2, 2, 0, 2, 0, 0, 0, 0, 2, 0, 2, 2, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                    });
                    break;
                case FactionType.DarkFollower:
                    {
                        col0_Main = new Color(56, 8, 56);
                        col1_Detail1 = new Color(216,8,8);
                        col2_Detail2 = new Color(56,8,8);

                        col3_Skin = Color.LightGreen;
                        col4_Hair = Color.DarkGreen;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0,
                            0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0,
                            0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.GreenWood:
                    {
                        col0_Main = new Color(56, 88, 8);
                        col1_Detail1 = new Color(248, 200, 24);
                        col2_Detail2 = new Color(168, 216, 24);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 2, 0, 0, 1, 0, 0, 1, 0, 0, 2, 0, 0, 0,
                            0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0,
                            0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0,
                            0, 0, 2, 0, 2, 0, 2, 0, 0, 2, 0, 2, 0, 2, 0, 0,
                            0, 0, 2, 0, 2, 0, 2, 0, 0, 2, 0, 2, 0, 2, 0, 0,
                            0, 0, 0, 2, 2, 0, 2, 0, 0, 2, 0, 2, 2, 0, 0, 0,
                            0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;
                case FactionType.EasternEmpire:
                    {
                        col0_Main = new Color(184, 8, 8);
                        col1_Detail1 = new Color(248, 184, 8);
                        col2_Detail2 = new Color(248, 248, 152);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            2, 2, 2, 2, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                            2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            2, 2, 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            2, 2, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                            1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
                            1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0,
                            0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1,
                            1, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1,
                            1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                            1, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0,
                            1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0,
                            1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0,
                            1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0,
                            1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1,
                            1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1,
                            1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1,
                        });
                    }
                    break;
                case FactionType.NordicRealm:
                    {
                        col0_Main = new Color(24, 168, 248);
                        col1_Detail1 = new Color(8, 120, 184);
                        col2_Detail2 = new Color(232, 232, 120);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.OrangeRed;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
                            0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 2, 0, 2, 2, 0, 2, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.BearClaw:
                    {
                        col0_Main = new Color(200, 88, 24);
                        col1_Detail1 = new Color(8, 56, 120);
                        col2_Detail2 = new Color(232,232, 120);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.OrangeRed;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
                            0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 1, 2, 1, 2, 1, 2, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 2, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 0, 1, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0,
                            0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;
                case FactionType.NordicSpur:
                    {
                        col0_Main = new Color(200, 88, 24);
                        col1_Detail1 = new Color(56, 40, 8);
                        col2_Detail2 = new Color(232, 232, 120);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.OrangeRed;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
                            0, 0, 0, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 1, 2, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 2, 2, 2, 0, 2, 2, 2, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 1, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 0, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;
                case FactionType.IceRaven:
                    {
                        col0_Main = new Color(8, 40, 88);
                        col1_Detail1 = new Color(152, 184, 248);
                        col2_Detail2 = new Color(8, 8, 56);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.OrangeRed;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
                            0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 0, 0,
                            0, 0, 1, 1, 2, 2, 1, 1, 2, 1, 2, 2, 2, 1, 0, 0,
                            0, 0, 1, 2, 1, 2, 2, 2, 2, 2, 2, 1, 2, 1, 0, 0,
                            0, 0, 1, 1, 2, 1, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0,
                            0, 0, 1, 2, 1, 2, 2, 1, 1, 1, 2, 2, 1, 1, 0, 0,
                            0, 0, 1, 1, 2, 1, 2, 1, 1, 2, 1, 2, 1, 1, 0, 0,
                            0, 0, 0, 1, 1, 2, 1, 1, 1, 1, 2, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.DyingMonger:
                    {
                        col0_Main = Color.CornflowerBlue;
                        col1_Detail1 = new Color(248, 184, 8);
                        col2_Detail2 = new Color(248, 248, 152);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            2, 2, 2, 2, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                            2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            2, 2, 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            2, 2, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                            1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
                            1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0,
                            0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1,
                            1, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1,
                            1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                            1, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0,
                            1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0,
                            1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0,
                            1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0,
                            1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1,
                            1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1,
                            1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1,
                        });
                    }
                    break;
                case FactionType.DyingDestru:
                    {
                        col0_Main = Color.Blue;
                        col1_Detail1 = new Color(248, 184, 8);
                        col2_Detail2 = new Color(248, 248, 152);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            2, 2, 2, 2, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                            2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            2, 2, 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            2, 2, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                            1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
                            1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0,
                            0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1,
                            1, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1,
                            1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                            1, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0,
                            1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0,
                            1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0,
                            1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0,
                            1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1,
                            1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1,
                            1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1,
                        });
                    }
                    break;
                case FactionType.DyingHate:
                    {
                        col0_Main = Color.LightBlue;
                        col1_Detail1 = new Color(248, 184, 8);
                        col2_Detail2 = new Color(248, 248, 152);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            2, 2, 2, 2, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                            2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            2, 2, 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            2, 2, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                            1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
                            1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0,
                            0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1,
                            1, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1,
                            1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                            1, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0,
                            1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0,
                            1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0,
                            1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0,
                            1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1,
                            1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1,
                            1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1,
                        });
                    }
                    break;

                case FactionType.DragonSlayer:
                    {
                        col0_Main = new Color(24, 56, 8);
                        col1_Detail1 = new Color(216, 8, 88);
                        col2_Detail2 = new Color(8, 24, 56);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0,
                            0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 2, 0, 2, 0,
                            0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 0, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 1, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0,
                            0, 0, 2, 0, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                            0, 2, 0, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                            2, 0, 2, 2, 2, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0,
                            0, 2, 2, 2, 0, 2, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0,
                            0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0,
                            0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.DarkLord:
                    {
                        col0_Main = new Color(24, 8, 8);
                        col1_Detail1 = new Color(232, 216, 88);
                        col2_Detail2 = new Color(136, 8, 152);

                        col3_Skin = Color.LightGreen;
                        col4_Hair = Color.DarkGreen;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0,
                            0, 2, 2, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 2, 2, 0,
                            0, 2, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0,
                            0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
                            0, 2, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 2, 0,
                            0, 2, 0, 0, 0, 2, 1, 0, 0, 1, 2, 0, 0, 0, 2, 0,
                            0, 2, 0, 2, 2, 1, 1, 0, 0, 1, 1, 2, 2, 0, 2, 0,
                            0, 2, 0, 2, 2, 2, 1, 0, 0, 1, 2, 2, 2, 0, 2, 0,
                            0, 2, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 2, 0,
                            0, 2, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 2, 0,
                            0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
                            0, 2, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0,
                            0, 2, 2, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 2, 2, 0,
                            0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;
                case FactionType.SouthHara:
                    {
                        col0_Main = new Color(184,40,8);
                        col1_Detail1 = new Color(0,0,0);
                        col2_Detail2 = new Color(248,200,24);

                        col3_Skin = Color.LightGreen;
                        col4_Hair = Color.DarkGreen;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                            0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                            0, 0, 1, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.Bluepeak:
                    {
                        factionFlavorType = FactionFlavorType.Mountain;
                        col0_Main = new Color(202, 203, 238);
                        col1_Detail1 = new Color(84, 80, 236);
                        col2_Detail2 = new Color(19, 3, 120);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 2, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 2, 0, 0, 0, 0,
                            0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 1, 1, 2, 0, 0, 0,
                            0, 0, 2, 1, 2, 2, 0, 0, 1, 0, 1, 1, 1, 2, 0, 0,
                            0, 2, 2, 0, 1, 2, 0, 1, 0, 0, 0, 1, 1, 1, 2, 0,
                            2, 0, 0, 0, 1, 1, 2, 0, 0, 0, 0, 1, 1, 2, 1, 2,
                            0, 0, 1, 0, 1, 1, 1, 1, 0, 2, 0, 1, 2, 0, 1, 1,
                            0, 1, 0, 0, 1, 1, 1, 1, 2, 1, 2, 1, 0, 0, 1, 1,
                            0, 1, 0, 0, 1, 1, 1, 2, 0, 1, 1, 2, 0, 0, 0, 1,
                            1, 0, 0, 0, 0, 1, 2, 0, 1, 0, 1, 1, 2, 1, 1, 1,
                            0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1,
                        });
                    }
                    break;

                case FactionType.Sivo:
                    {
                        factionFlavorType = FactionFlavorType.Horse;
                        col0_Main = new Color(22, 7, 113);
                        col1_Detail1 = new Color(151, 97, 43);
                        col2_Detail2 = new Color(163, 199, 220);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 1, 2, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 2, 1, 0,
                            0, 1, 2, 1, 2, 0, 0, 0, 0, 0, 0, 2, 1, 2, 1, 0,
                            0, 1, 0, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 0, 1, 0,
                            0, 1, 0, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 0, 1, 0,
                            0, 1, 0, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 0, 1, 0,
                            0, 1, 0, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 0, 1, 0,
                            0, 1, 0, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 0, 1, 0,
                            0, 1, 0, 2, 2, 2, 0, 0, 0, 0, 2, 2, 2, 0, 1, 0,
                            0, 1, 0, 2, 2, 1, 2, 2, 2, 2, 1, 2, 2, 0, 1, 0,
                            0, 1, 0, 0, 1, 2, 2, 2, 2, 2, 2, 1, 0, 0, 1, 0,
                            0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                            0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;


                case FactionType.Starshield:
                    {
                        factionFlavorType = FactionFlavorType.Mountain;
                        col0_Main = new Color(45, 114, 206);
                        col1_Detail1 = new Color(49, 16, 148);
                        col2_Detail2 = new Color(237, 194, 18);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0,
0, 0, 1, 0, 0, 2, 0, 0, 1, 0, 0, 2, 0, 1, 0, 0,
0, 0, 1, 0, 2, 2, 2, 0, 1, 0, 2, 2, 2, 1, 0, 0,
0, 0, 1, 0, 0, 2, 0, 0, 1, 0, 0, 2, 0, 1, 0, 0,
0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0,
0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0,
0, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0, 2, 0, 1, 0, 0,
0, 0, 0, 1, 0, 2, 0, 0, 1, 0, 2, 0, 1, 0, 0, 0,
0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0,
0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0,
0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0,
0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0,
0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.RiverStallion:
                    {
                        factionFlavorType = FactionFlavorType.Horse;
                        col0_Main = new Color(11, 39, 122);
                        col1_Detail1 = new Color(109, 60, 7);
                        col2_Detail2 = new Color(208, 221, 218);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0,
0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 1, 0,
0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 1, 0,
0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 1, 0,
0, 1, 0, 2, 2, 0, 0, 0, 0, 2, 2, 2, 0, 2, 1, 0,
0, 1, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 1, 0,
0, 1, 2, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 1, 0,
0, 1, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 1, 0,
0, 1, 0, 0, 2, 0, 2, 0, 0, 0, 2, 0, 0, 0, 1, 0,
0, 1, 0, 0, 2, 0, 2, 0, 0, 0, 2, 0, 0, 0, 1, 0,
0, 1, 0, 0, 0, 2, 0, 2, 0, 0, 2, 0, 0, 0, 1, 0,
0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0,
0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.Hoft:
                    {
                        factionFlavorType = FactionFlavorType.Horse;
                        col0_Main = new Color(246, 154, 11);
                        col1_Detail1 = new Color(8, 38, 7);
                        col2_Detail2 = new Color(68, 15, 247);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0,
0, 1, 0, 0, 1, 0, 0, 0, 0, 2, 0, 0, 0, 1, 1, 0,
0, 1, 0, 0, 1, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0,
0, 0, 1, 1, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0,
0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0,
0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0,
0, 0, 2, 2, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0,
0, 2, 2, 0, 2, 0, 2, 2, 2, 2, 2, 2, 0, 0, 2, 0,
0, 2, 2, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0,
0, 0, 2, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 2, 0, 0,
0, 2, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0,
0, 0, 0, 0, 2, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0,
0, 1, 1, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0,
0, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0,
0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                //case FactionType.Bluepeak:
                //    {
                //        factionFlavorType = FactionFlavorType.Mountain;
                //        col0_Main = new Color(202, 203, 238);
                //        col1_Detail1 = new Color(84, 80, 236);
                //        col2_Detail2 = new Color(19, 3, 120);

                //        col3_Skin = Color.LightGray;
                //        col4_Hair = Color.DarkGray;

                //        flagDesign = new FlagDesign(new byte[]
                //        {
                //            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                //            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0,
                //            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0,
                //            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0,
                //            0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 2, 0, 2, 0,
                //            0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                //            0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                //            0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                //            0, 0, 0, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 0, 0, 0,
                //            0, 0, 0, 0, 1, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0,
                //            0, 0, 2, 0, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                //            0, 2, 0, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                //            2, 0, 2, 2, 2, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0,
                //            0, 2, 2, 2, 0, 2, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0,
                //            0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0,
                //            0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                //        });
                //    }
                //    break;

                //case FactionType.Bluepeak:
                //    {
                //        factionFlavorType = FactionFlavorType.Mountain;
                //        col0_Main = new Color(202, 203, 238);
                //        col1_Detail1 = new Color(84, 80, 236);
                //        col2_Detail2 = new Color(19, 3, 120);

                //        col3_Skin = Color.LightGray;
                //        col4_Hair = Color.DarkGray;

                //        flagDesign = new FlagDesign(new byte[]
                //        {
                //            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                //            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0,
                //            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0,
                //            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0,
                //            0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 2, 0, 2, 0,
                //            0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                //            0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                //            0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                //            0, 0, 0, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 0, 0, 0,
                //            0, 0, 0, 0, 1, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0,
                //            0, 0, 2, 0, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                //            0, 2, 0, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                //            2, 0, 2, 2, 2, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0,
                //            0, 2, 2, 2, 0, 2, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0,
                //            0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0,
                //            0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                //        });
                //    }
                //    break;
            }
            

            //if (factiontype == FactionType.Player)
            //{
            //    switch (index)
            //    {
            //        case 0:
            //            col0_Main = Color.Blue);
            //            col1_Detail1 = Color.Yellow);
            //            col2_Detail2 = Color.Orange);
            //            break;

            //        case 1:
            //            col0_Main = Color.Red);
            //            col1_Detail1 = Color.MediumPurple);
            //            col2_Detail2 = Color.Blue);
            //            break;

            //        case 2:
            //            col0_Main = Color.Green);
            //            col1_Detail1 = Color.Yellow);
            //            col2_Detail2 = Color.YellowGreen);
            //            break;

            //        case 3:
            //            col0_Main = Color.Orange);
            //            col1_Detail1 = Color.Pink);
            //            col2_Detail2 = Color.Brown);
            //            break;
            //    }

            //    col3_Skin = Color.Beige);
            //    col4_Hair = Color.Brown);

            //    flagDesign = new FlagDesign();
            //}
            //else if (factiontype == FactionType.DarkLord)
            //{
            //    col0_Main = new Color(24, 8, 8));
            //    col1_Detail1 = new Color(232, 216, 88));
            //    col2_Detail2 = new Color(136, 8, 152));

            //    col3_Skin = Color.LightGreen);
            //    col4_Hair = Color.DarkGreen);

            //    flagDesign = FlagDesign.EvilBanner;
            //}
            //else
            //{
            //    var color1 = AiColorRange.GetRandom();
            //    var color2 = AiColorRange.GetRandom();

            //    col0_Main = color1);
            //    col1_Detail1 = color2);
            //    col2_Detail2 = Color.Gray);

            //    col3_Skin = Color.LightGray);
            //    col4_Hair = Color.DarkGray);

            //    flagDesign = arraylib.RandomListMember(FlagDesign.AiBanner);
            //}
        }

        public FlagAndColor(System.IO.BinaryReader r)
        {
            read(r);
        }

        public FlagAndColor Clone()
        {
            FlagAndColor clonedData = new FlagAndColor(FactionType.Player, this.index, null)
            {
                //colors = this.colors != null ? (Color[])this.colors.Clone() : null,
                col0_Main = col0_Main,
                col1_Detail1 = col1_Detail1,
                col2_Detail2 = col2_Detail2,
                col3_Skin = col3_Skin,
                col4_Hair = col4_Hair,
                blockColors = this.blockColors != null ? (ushort[])this.blockColors.Clone() : null,
                flagDesign = this.flagDesign != null ? this.flagDesign.CloneFlag() : null,
                modelColorReplace = this.modelColorReplace != null ? new List<BlockHDPair>(this.modelColorReplace) : null
            };

            return clonedData;
        }

        public void gameStartInit()
        {           

            blockColors = new ushort[ColorCount];
            //for (int i = 0; i < blockColors.Length; ++i)
            //{
            //    blockColors[i] = BlockHD.ToBlockValue(colors[i], BlockHD.UnknownMaterial);
            //}
            blockColors[0] = BlockHD.ToBlockValue(col0_Main, BlockHD.UnknownMaterial);
            blockColors[1] = BlockHD.ToBlockValue(col1_Detail1, BlockHD.UnknownMaterial);
            blockColors[2] = BlockHD.ToBlockValue(col2_Detail2, BlockHD.UnknownMaterial);
            blockColors[3] = BlockHD.ToBlockValue(col3_Skin, BlockHD.UnknownMaterial);
            blockColors[4] = BlockHD.ToBlockValue(col4_Hair, BlockHD.UnknownMaterial);


            //var mainCol = getColor(ProfileColorType.Main);

            BlockHD main = new BlockHD(col0_Main);
            BlockHD darkMain = main;
            darkMain.tintSteps(-1, -1, -1);

            Color altCol;
            if (ColorExt.GetBrightNess(col0_Main) >= 0.3f)
            {
                altCol = ColorExt.ChangeBrighness( col0_Main, -30);
            }
            else
            {
                altCol = ColorExt.ChangeBrighness(col0_Main, 30);
            }
            BlockHD mainAlt = new BlockHD(altCol);

            //var skinCol = getColor(ProfileColorType.Skin);
            BlockHD skin = new BlockHD(col3_Skin);
            BlockHD redskin = skin;
            redskin.tintSteps(1, 0, 0);

            BlockHD darkskin = skin;
            darkskin.tintSteps(-1, -1, -1);

            modelColorReplace = new List<BlockHDPair>
            {
                new BlockHDPair(MainCol.baseColor, main.BlockValue),
                new BlockHDPair(MainCol.darker, darkMain.BlockValue),

                new BlockHDPair(AltMainCol.baseColor, mainAlt.BlockValue),

                 new BlockHDPair(DetailCol1.baseColor, BlockHD.ToBlockValue(
                     col1_Detail1, BlockHD.UnknownMaterial)),

                new BlockHDPair(DetailCol2.baseColor, BlockHD.ToBlockValue(
                     col2_Detail2, BlockHD.UnknownMaterial)),

                new BlockHDPair(SkinCol.baseColor, skin.BlockValue),
                new BlockHDPair(SkinCol.darker, darkskin.BlockValue),
                new BlockHDPair(SkinCol.redTint, redskin.BlockValue),

                new BlockHDPair(HairCol.baseColor, BlockHD.ToBlockValue(
                     col4_Hair, BlockHD.UnknownMaterial)),
            };
            
        }

        public void PrintFlagColors()
        {
            //ProfileColorType[] colors = { ProfileColorType.Main, ProfileColorType.Detail1, ProfileColorType.Detail2 };
            //foreach (var col in colors)
            //{
            //    System.Diagnostics.Debug.WriteLine(col.ToString() + ": " + getColor(col).ToString());
            //}
            System.Diagnostics.Debug.WriteLine($"col0_Main = new Color({col0_Main.R}, {col0_Main.G}, {col0_Main.B});");
            System.Diagnostics.Debug.WriteLine($"col1_Detail1 = new Color({col1_Detail1.R}, {col1_Detail1.G}, {col1_Detail1.B});");
            System.Diagnostics.Debug.WriteLine($"col2_Detail2 = new Color({col2_Detail2.R}, {col2_Detail2.G}, {col2_Detail2.B});");
        }

        public void setColor(ProfileColorType type, Color color)
        {
            switch (type)
            {
                case ProfileColorType.Main:
                    col0_Main = color;
                    break;
                case ProfileColorType.Detail1:
                    col1_Detail1 = color;
                    break;
                case ProfileColorType.Detail2:
                    col2_Detail2 = color;
                    break;
                case ProfileColorType.Skin:
                    col3_Skin = color;
                    break;
                case ProfileColorType.Hair:
                    col4_Hair = color;
                    break;

            }
            //colors[(int)type] = color;

        }

        public Color getColor(ProfileColorType type)
        {
            switch (type)
            {
                case ProfileColorType.Main:
                    return col0_Main;
                case ProfileColorType.Detail1:
                    return col1_Detail1;
                case ProfileColorType.Detail2:
                    return col2_Detail2;
                case ProfileColorType.Skin:
                    return col3_Skin;
                case ProfileColorType.Hair:
                    return col4_Hair;

                default:
                    throw new NotImplementedException();
            }

            //return colors[(int)type];
        }

        public void Button(GuiLayout layout, IGuiAction action, bool moreArrow)
        {
            var button = new GuiIconTextButton(SpriteName.MissingImage, string.Format( DssRef.lang.Lobby_FlagNumbered ,index+1),
                null, action, moreArrow, layout);

            button.icon.Texture = flagDesign.CreateTexture(this);
            button.icon.SetFullTextureSource();
        }

        //public void write(System.IO.BinaryWriter w)
        //{
        //    for (int i = 0; i < ColorCount; ++i)
        //    {
        //        SaveLib.WriteColorStream_3B(w, colors[i]);
        //    }

        //    flagDesign.write(w);
        //}

        public void read_old(System.IO.BinaryReader r)
        {
            //for (int i = 0; i < ColorCount; ++i)
            //{
            //    colors[i] = SaveLib.ReadColorStream_3B(r);
            //}
            col0_Main = SaveLib.ReadColorStream_3B(r);
            col1_Detail1=SaveLib.ReadColorStream_3B(r);
            col2_Detail2 = SaveLib.ReadColorStream_3B(r);
            col3_Skin = SaveLib.ReadColorStream_3B(r);
            col4_Hair = SaveLib.ReadColorStream_3B(r);

            flagDesign.read(r);
        }

        public void write(System.IO.BinaryWriter w)
        {
            const int Version = 2;

            w.Write(Version);

            //for (int i = 0; i < ColorCount; ++i)
            //{
            //    SaveLib.WriteColorStream_3B(w, colors[i]);
            //}
            SaveLib.WriteColorStream_3B(w, col0_Main);
            SaveLib.WriteColorStream_3B(w, col1_Detail1);
            SaveLib.WriteColorStream_3B(w, col2_Detail2);
            SaveLib.WriteColorStream_3B(w, col3_Skin);
            SaveLib.WriteColorStream_3B(w, col4_Hair);

            flagDesign.write(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();

            //for (int i = 0; i < ColorCount; ++i)
            //{
            //    colors[i] = SaveLib.ReadColorStream_3B(r);
            //}
            col0_Main = SaveLib.ReadColorStream_3B(r);
            col1_Detail1 = SaveLib.ReadColorStream_3B(r);
            col2_Detail2 = SaveLib.ReadColorStream_3B(r);
            col3_Skin = SaveLib.ReadColorStream_3B(r);
            col4_Hair = SaveLib.ReadColorStream_3B(r);

            //flagDesign.read(r);
            flagDesign = new FlagDesign(r);
        }

    }

    //enum ProfileType
    //{ 
    //    Player,
    //    Ai,
    //    Evil
    //}

    enum ProfileColorType
    {
        Main = 0,
        Detail1,
        Detail2,

        Skin,
        Hair,
        NUM
    }
}
