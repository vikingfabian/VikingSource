using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Net;
using System.Text;
using System.Threading;
using VikingEngine.DSSWars.Data;
using VikingEngine.HUD;
using VikingEngine.LootFest.GO.WeaponAttack.ItemThrow;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.PJ;

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
        public FactionFlavorType factionFlavorType = FactionFlavorType.Other;


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

                case FactionType.AelthrenConclave:
                    {
                        factionFlavorType = FactionFlavorType.Noble;
                        col0_Main = new Color(137, 52, 197);
                        col1_Detail1 = new Color(241, 241, 168);
                        col2_Detail2 = new Color(63, 34, 25);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0,
0, 2, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 2, 0,
0, 2, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 2, 0,
0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0,
0, 2, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 2, 0,
0, 2, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 2, 0,
0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0,
0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0,
0, 0, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 0, 0,
0, 0, 2, 2, 2, 2, 0, 2, 2, 0, 2, 2, 2, 2, 0, 0,
0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.VrakasundEnclave:
                    {
                        factionFlavorType = FactionFlavorType.Mountain;
                        col0_Main = new Color(57, 90, 124);
                        col1_Detail1 = new Color(48, 46, 73);
                        col2_Detail2 = new Color(236, 236, 18);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0,
                            0, 0, 0, 2, 0, 0, 0, 2, 2, 0, 0, 0, 2, 0, 0, 0,
                            0, 0, 2, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 2, 0, 0,
                            0, 2, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 2, 0,
                            0, 2, 0, 0, 2, 0, 0, 2, 2, 0, 0, 2, 0, 0, 2, 0,
                            0, 2, 0, 2, 0, 0, 2, 1, 1, 2, 0, 0, 2, 0, 2, 0,
                            0, 2, 0, 2, 0, 2, 1, 1, 1, 1, 2, 0, 2, 0, 2, 0,
                            0, 2, 0, 2, 0, 2, 1, 1, 1, 1, 2, 0, 2, 0, 2, 0,
                            0, 2, 0, 2, 0, 2, 1, 1, 1, 1, 2, 0, 2, 0, 2, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.Tormürd:
                    {
                        factionFlavorType = FactionFlavorType.Noble;
                        col0_Main = new Color(15, 13, 29);
                        col1_Detail1 = new Color(208, 193, 196);
                        col2_Detail2 = new Color(131, 148, 232);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 2, 0,
                                0, 0, 1, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 2, 0,
                                0, 1, 0, 0, 0, 1, 0, 2, 0, 0, 0, 0, 0, 0, 2, 0,
                                0, 1, 1, 0, 1, 1, 0, 0, 2, 1, 1, 1, 1, 2, 0, 0,
                                0, 0, 1, 1, 1, 0, 0, 0, 0, 2, 1, 1, 2, 0, 0, 0,
                                0, 0, 0, 1, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0,
                                0, 0, 1, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0,
                                0, 1, 1, 0, 0, 0, 0, 2, 0, 0, 1, 1, 0, 0, 2, 0,
                                0, 0, 1, 0, 0, 0, 0, 2, 0, 1, 1, 1, 1, 0, 2, 0,
                                0, 0, 0, 1, 0, 0, 0, 2, 1, 1, 1, 1, 1, 1, 2, 0,
                                0, 0, 0, 1, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.ElderysFyrd:
                    {
                        factionFlavorType = FactionFlavorType.Forest;
                        col0_Main = new Color(255, 255, 255);
                        col1_Detail1 = new Color(110, 107, 213);
                        col2_Detail2 = new Color(222, 165, 35);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
                                0, 0, 0, 0, 0, 2, 0, 2, 2, 0, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 2, 0, 2, 0, 2, 2, 0, 2, 0, 2, 0, 0, 0,
                                0, 0, 0, 2, 0, 2, 0, 2, 2, 0, 2, 0, 2, 0, 0, 0,
                                0, 0, 0, 2, 0, 2, 0, 2, 2, 0, 2, 0, 2, 0, 0, 0,
                                0, 0, 0, 2, 0, 2, 0, 2, 2, 0, 2, 0, 2, 0, 0, 0,
                                0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 1, 0, 0, 0,
                                0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 1, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 2, 2, 0, 0, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 0, 2, 0, 0, 2, 0, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.Hólmgar:
                    {
                        factionFlavorType = FactionFlavorType.Sea;
                        col0_Main = new Color(12, 125, 241);
                        col1_Detail1 = new Color(211, 231, 237);
                        col2_Detail2 = new Color(116, 54, 11);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 0, 2, 2, 2, 2, 0, 0, 0, 2, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 2, 0, 0, 0,
                                0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0,
                                0, 2, 2, 2, 2, 0, 0, 2, 2, 0, 0, 2, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 0, 0, 2, 2, 0, 0, 2, 2, 2, 2, 0,
                                0, 1, 0, 2, 2, 1, 0, 0, 0, 1, 0, 2, 2, 0, 1, 0,
                                1, 0, 1, 1, 2, 2, 1, 1, 1, 0, 2, 2, 1, 1, 0, 1,
                                0, 0, 0, 2, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 2, 0, 0, 0, 2, 2, 2, 2, 0, 2, 0, 0, 0, 0,
                                0, 0, 1, 0, 0, 0, 1, 2, 0, 0, 1, 0, 2, 0, 1, 0,
                                0, 1, 0, 1, 0, 1, 0, 2, 0, 1, 0, 1, 0, 1, 0, 1,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.RûnothalOrder:
                    {
                        factionFlavorType = FactionFlavorType.Mystical;
                        col0_Main = new Color(47, 118, 45);
                        col1_Detail1 = new Color(43, 23, 85);
                        col2_Detail2 = new Color(44, 13, 187);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 2, 2, 0, 2, 2, 0, 2, 2, 0, 2, 2, 0, 2, 2, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
                                0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
                                0, 2, 2, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 2, 0,
                                0, 2, 2, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0,
                                0, 2, 2, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0,
                                0, 0, 2, 2, 2, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0,
                                0, 2, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0,
                                0, 2, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 0,
                                0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0,
                                0, 2, 2, 0, 2, 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.GrimwardEotain:
                    {
                        factionFlavorType = FactionFlavorType.Warrior;
                        col0_Main = new Color(202, 11, 13);
                        col1_Detail1 = new Color(245, 111, 8);
                        col2_Detail2 = new Color(32, 30, 57);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1,
                                0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0,
                                0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0,
                                0, 1, 0, 1, 1, 2, 2, 1, 2, 2, 1, 2, 2, 1, 0, 0,
                                0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0,
                                0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0,
                                0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0,
                                0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0,
                                0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0,
                                0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0,
                                0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0,
                                0, 0, 0, 1, 1, 1, 2, 0, 0, 0, 0, 2, 1, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.SkaeldraHaim:
                    {
                        factionFlavorType = FactionFlavorType.Forest;
                        col0_Main = new Color(71, 152, 28);
                        col1_Detail1 = new Color(11, 69, 107);
                        col2_Detail2 = new Color(114, 247, 167);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                                0, 0, 0, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0,
                                0, 0, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 0,
                                0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0,
                                0, 0, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 0,
                                0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0,
                                0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0,
                                0, 0, 0, 0, 1, 2, 1, 2, 1, 2, 1, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 2, 0, 2, 0, 2, 2, 1, 1, 0, 2, 2, 0, 2, 0, 2,
                                2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 2, 2, 2, 2, 2, 2,
                                2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 2, 2, 2, 2, 2, 2,
                                2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 2, 2, 2, 2, 2, 2,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                        });
                    }
                    break;

                case FactionType.MordwynnCompact:
                    {
                        factionFlavorType = FactionFlavorType.People;
                        col0_Main = new Color(128, 15, 125);
                        col1_Detail1 = new Color(150, 191, 250);
                        col2_Detail2 = new Color(192, 154, 131);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0,
                                0, 1, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0,
                                0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
                                2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2,
                                2, 2, 2, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 2, 2, 2,
                                0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0,
                                0, 1, 2, 2, 2, 2, 0, 0, 0, 0, 2, 2, 2, 2, 1, 0,
                                0, 1, 2, 2, 2, 0, 2, 0, 0, 2, 0, 2, 2, 2, 1, 0,
                                0, 1, 0, 2, 0, 2, 0, 0, 0, 0, 2, 0, 2, 0, 1, 0,
                                0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0,
                                0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0,
                                0, 1, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0,
                                0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.AethmireSovren:
                    {
                        factionFlavorType = FactionFlavorType.Noble;
                        col0_Main = new Color(17, 242, 238);
                        col1_Detail1 = new Color(221, 149, 63);
                        col2_Detail2 = new Color(255, 255, 255);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 2, 2, 0, 0, 2, 2, 0, 0, 0, 2, 2, 2, 2, 0,
                                0, 2, 0, 0, 2, 2, 0, 0, 2, 0, 0, 0, 0, 2, 2, 2,
                                0, 2, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 2, 0, 0, 0,
                                0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 2, 0, 2, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0,
                                2, 2, 2, 2, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0,
                                0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0,
                                0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 0, 1, 0, 0,
                                0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0,
                                0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0,
                                0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.ThurlanKin:
                    {
                        factionFlavorType = FactionFlavorType.Forest;
                        col0_Main = new Color(78, 121, 51);
                        col1_Detail1 = new Color(13, 74, 10);
                        col2_Detail2 = new Color(246, 175, 41);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 0, 2, 2, 2, 2, 2, 0, 0, 0,
                                0, 0, 0, 1, 0, 0, 1, 1, 0, 2, 2, 0, 0, 1, 0, 0,
                                0, 0, 1, 1, 0, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 0,
                                0, 1, 2, 2, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1, 0,
                                1, 2, 2, 1, 2, 1, 2, 2, 2, 2, 2, 2, 1, 1, 2, 0,
                                1, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0,
                                1, 1, 2, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 2, 1, 0,
                                1, 2, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 0,
                                1, 1, 1, 1, 2, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 0,
                                0, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0,
                                0, 0, 1, 0, 0, 2, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.ValestennOrder:
                    {
                        factionFlavorType = FactionFlavorType.Forest;
                        col0_Main = new Color(72, 92, 180);
                        col1_Detail1 = new Color(40, 23, 69);
                        col2_Detail2 = new Color(125, 248, 54);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 2, 0, 2, 2, 0, 2, 2, 0, 0, 0, 0,
                                0, 0, 2, 0, 2, 2, 0, 0, 0, 0, 2, 2, 0, 2, 0, 0,
                                0, 0, 0, 2, 0, 0, 1, 1, 0, 0, 0, 0, 2, 0, 0, 0,
                                0, 2, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 2, 0,
                                0, 2, 2, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 2, 2, 0,
                                0, 0, 2, 2, 0, 1, 0, 0, 0, 0, 1, 0, 2, 2, 0, 0,
                                0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
                                0, 2, 2, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 2, 2, 0,
                                0, 2, 2, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 2, 2, 0,
                                0, 0, 0, 2, 0, 1, 0, 0, 0, 0, 1, 0, 2, 0, 0, 0,
                                0, 0, 2, 2, 0, 1, 0, 0, 0, 0, 1, 0, 2, 2, 0, 0,
                                0, 0, 2, 2, 1, 1, 0, 0, 0, 0, 1, 1, 2, 2, 0, 0,
                                0, 0, 0, 0, 2, 1, 1, 1, 1, 1, 1, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.Mournfold:
                    {
                        factionFlavorType = FactionFlavorType.Mystical;
                        col0_Main = new Color(79, 3, 3);
                        col1_Detail1 = new Color(79, 109, 220);
                        col2_Detail2 = new Color(229, 179, 230);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
                                0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0,
                                0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
                                0, 1, 0, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 0, 1, 0,
                                0, 1, 0, 1, 1, 2, 2, 1, 1, 1, 2, 2, 1, 0, 1, 0,
                                0, 1, 0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 0, 1, 0,
                                0, 1, 0, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 0, 1, 0,
                                0, 1, 0, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 0, 1, 0,
                                0, 1, 0, 1, 1, 2, 2, 1, 2, 1, 1, 1, 1, 0, 1, 0,
                                0, 1, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 0, 1, 0,
                                0, 1, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 0, 1, 0,
                                0, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 0,
                                0, 1, 0, 1, 0, 0, 0, 0, 2, 0, 0, 0, 1, 0, 1, 0,
                                0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.OrentharTribes:
                    {
                        factionFlavorType = FactionFlavorType.Desert;
                        col0_Main = new Color(127, 94, 66);
                        col1_Detail1 = new Color(132, 46, 28);
                        col2_Detail2 = new Color(225, 235, 87);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 2, 2, 2, 1, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 1, 2, 2, 2, 1, 0, 0, 0, 0,
                                0, 0, 1, 1, 0, 0, 0, 0, 1, 2, 2, 2, 1, 0, 0, 0,
                                0, 1, 0, 0, 1, 0, 0, 0, 1, 2, 2, 2, 1, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 1, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 1, 2, 2, 2, 2, 1, 0, 0, 0, 1, 1, 0, 0,
                                0, 0, 1, 2, 2, 2, 2, 1, 0, 0, 0, 1, 0, 0, 1, 0,
                                0, 0, 0, 1, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 1, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 1, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 1, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.SkarnVael:
                    {
                        factionFlavorType = FactionFlavorType.Mountain;
                        col0_Main = new Color(62, 13, 26);
                        col1_Detail1 = new Color(206, 133, 207);
                        col2_Detail2 = new Color(176, 56, 157);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
                                0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
                                0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0,
                                0, 0, 2, 0, 0, 0, 0, 2, 1, 2, 0, 0, 0, 0, 2, 0,
                                0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 2, 0, 0, 0, 0, 0,
                                0, 0, 2, 0, 0, 2, 0, 0, 0, 1, 1, 2, 0, 0, 0, 0,
                                0, 2, 1, 2, 2, 0, 0, 1, 0, 1, 1, 1, 2, 0, 0, 0,
                                2, 2, 0, 1, 2, 0, 1, 0, 0, 0, 1, 1, 1, 2, 0, 2,
                                0, 0, 0, 1, 1, 2, 0, 0, 0, 0, 1, 1, 2, 1, 2, 1,
                                0, 1, 0, 1, 1, 1, 1, 0, 2, 0, 1, 2, 0, 1, 1, 0,
                                1, 0, 0, 1, 1, 1, 1, 2, 1, 2, 1, 0, 0, 1, 1, 0,
                                1, 0, 0, 1, 1, 1, 2, 0, 1, 1, 2, 0, 0, 0, 1, 0,
                                0, 0, 0, 0, 1, 2, 0, 1, 0, 1, 1, 2, 1, 1, 1, 1,
                                0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0,
                        });
                    }
                    break;

                case FactionType.Glimmerfell:
                    {
                        factionFlavorType = FactionFlavorType.Mystical;
                        col0_Main = new Color(30, 9, 123);
                        col1_Detail1 = new Color(226, 145, 17);
                        col2_Detail2 = new Color(208, 221, 218);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 0, 2, 0, 0, 0, 2, 2, 0, 0, 0, 2, 0, 1, 0,
                                0, 1, 0, 0, 2, 0, 2, 2, 2, 2, 0, 2, 0, 0, 1, 0,
                                0, 1, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 1, 0,
                                0, 1, 0, 0, 2, 2, 2, 0, 0, 2, 2, 2, 0, 0, 1, 0,
                                0, 1, 2, 2, 2, 2, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0,
                                0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 2, 2, 2, 2, 1, 0,
                                0, 1, 0, 0, 2, 2, 2, 0, 0, 2, 2, 2, 0, 0, 1, 0,
                                0, 1, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 1, 0,
                                0, 1, 0, 0, 2, 0, 2, 2, 2, 2, 0, 2, 0, 0, 1, 0,
                                0, 1, 0, 2, 0, 0, 0, 2, 2, 0, 0, 0, 2, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 0, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.BleakwaterFold:
                    {
                        factionFlavorType = FactionFlavorType.People;
                        col0_Main = new Color(42, 140, 85);
                        col1_Detail1 = new Color(9, 78, 186);
                        col2_Detail2 = new Color(173, 189, 214);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 2, 2, 0, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 2, 1, 2, 2, 2, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 1,
                                0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 2, 2, 2, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 2, 2, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.Oathmaeren:
                    {
                        factionFlavorType = FactionFlavorType.Mountain;
                        col0_Main = new Color(208, 227, 220);
                        col1_Detail1 = new Color(226, 117, 8);
                        col2_Detail2 = new Color(15, 65, 214);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0,
                                0, 0, 1, 1, 0, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0,
                                0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 0, 0, 0, 1, 1, 1, 2, 0, 0, 0,
                                0, 0, 2, 2, 2, 1, 1, 0, 1, 1, 1, 2, 2, 2, 0, 0,
                                0, 0, 2, 2, 0, 1, 0, 1, 1, 1, 0, 0, 2, 2, 0, 0,
                                0, 0, 2, 2, 0, 0, 1, 1, 1, 0, 0, 0, 2, 2, 0, 0,
                                0, 0, 2, 1, 0, 1, 1, 1, 0, 1, 0, 0, 2, 2, 0, 0,
                                0, 0, 2, 2, 1, 1, 1, 0, 1, 1, 1, 0, 1, 2, 0, 0,
                                0, 0, 2, 2, 1, 1, 0, 0, 0, 1, 1, 1, 2, 2, 0, 0,
                                0, 0, 0, 1, 2, 2, 1, 0, 0, 0, 1, 2, 2, 0, 0, 0,
                                0, 0, 1, 0, 2, 2, 2, 2, 2, 1, 2, 2, 1, 0, 0, 0,
                                0, 1, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 1, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.Elderforge:
                    {
                        factionFlavorType = FactionFlavorType.Mountain;
                        col0_Main = new Color(51, 73, 200);
                        col1_Detail1 = new Color(84, 53, 30);
                        col2_Detail2 = new Color(206, 249, 251);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 0, 2, 2, 2, 2, 0, 2, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 0, 2, 2, 2, 2, 0, 2, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 0, 2, 2, 2, 2, 0, 2, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 0, 2, 2, 2, 2, 0, 2, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 0, 1, 1, 0, 2, 2, 2, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 2, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 2, 0, 0,
                                0, 2, 0, 2, 0, 0, 0, 1, 1, 0, 0, 0, 2, 0, 2, 0,
                                0, 0, 2, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 2, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.MarhollowCartel:
                    {
                        factionFlavorType = FactionFlavorType.Sea;
                        col0_Main = new Color(43, 7, 169);
                        col1_Detail1 = new Color(251, 194, 47);
                        col2_Detail2 = new Color(215, 193, 212);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 1, 0, 0, 2, 2, 0, 0, 1, 0, 0, 0, 0,
                            0, 0, 1, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 1, 0, 0,
                            0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                            0, 1, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 0, 1, 1, 2, 2, 1, 1, 0, 0, 0, 1, 0,
                            0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                            0, 1, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 1, 0,
                            0, 0, 2, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 2, 0, 0,
                            0, 2, 2, 2, 0, 0, 0, 2, 2, 0, 0, 0, 2, 2, 2, 0,
                            0, 0, 2, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 2, 0, 0,
                            0, 0, 2, 2, 0, 0, 0, 2, 2, 0, 0, 0, 2, 2, 0, 0,
                            0, 1, 0, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 0, 1, 0,
                            0, 0, 0, 0, 2, 2, 1, 2, 2, 1, 2, 2, 0, 0, 0, 0,
                            0, 0, 1, 0, 0, 2, 1, 2, 2, 1, 2, 0, 0, 1, 0, 0,
                            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.TharvaniDominion:
                    {
                        factionFlavorType = FactionFlavorType.City;
                        col0_Main = new Color(166, 31, 15);
                        col1_Detail1 = new Color(163, 65, 31);
                        col2_Detail2 = new Color(243, 207, 9);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 0, 2, 2, 0, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 0, 2, 0, 0, 0, 0, 0, 0,
                                1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1,
                                0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0,
                                1, 1, 1, 1, 1, 1, 2, 2, 0, 2, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 2, 2, 0, 2, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                        });
                    }
                    break;

                case FactionType.KystraAscendancy:
                    {
                        factionFlavorType = FactionFlavorType.City;
                        col0_Main = new Color(45, 144, 255);
                        col1_Detail1 = new Color(195, 191, 144);
                        col2_Detail2 = new Color(38, 51, 58);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 1, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 2, 0, 0, 0, 0, 0,
                                0, 0, 2, 0, 0, 2, 0, 0, 0, 1, 1, 2, 0, 0, 0, 0,
                                0, 2, 1, 2, 2, 0, 0, 1, 0, 1, 1, 1, 2, 0, 0, 0,
                                2, 2, 0, 1, 2, 0, 1, 0, 0, 0, 1, 1, 1, 2, 0, 2,
                                0, 0, 0, 1, 1, 2, 0, 0, 0, 0, 1, 1, 2, 1, 2, 1,
                                0, 1, 0, 1, 1, 1, 1, 0, 2, 0, 1, 2, 0, 1, 1, 0,
                                1, 0, 0, 1, 1, 1, 1, 2, 1, 2, 1, 0, 0, 1, 1, 0,
                                1, 0, 0, 1, 1, 1, 2, 0, 1, 1, 2, 0, 0, 0, 1, 0,
                                0, 0, 0, 0, 1, 2, 0, 1, 0, 1, 1, 2, 1, 1, 1, 1,
                        });
                    }
                    break;



                case FactionType.GildenmarkUnion:
                    {
                        factionFlavorType = FactionFlavorType.Noble;
                        col0_Main = new Color(99, 105, 3);
                        col1_Detail1 = new Color(241, 246, 37);
                        col2_Detail2 = new Color(158, 253, 13);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 2, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 2, 0,
                                0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0,
                                0, 0, 2, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 2, 0, 0,
                                0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 2, 0,
                                0, 2, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 2, 0,
                                0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0, 0, 0,
                                0, 0, 0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                                0, 2, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 2, 0,
                                0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 2, 0, 0,
                                0, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 2, 0,
                                0, 2, 0, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 0, 2, 0,
                                0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;



                case FactionType.AurecanEmpire:
                    {
                        factionFlavorType = FactionFlavorType.City;
                        col0_Main = new Color(245, 206, 99);
                        col1_Detail1 = new Color(150, 80, 16);
                        col2_Detail2 = new Color(69, 97, 176);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
                                0, 0, 2, 2, 0, 0, 0, 2, 2, 2, 0, 0, 2, 2, 0, 0,
                                0, 2, 2, 2, 2, 0, 0, 2, 2, 0, 0, 2, 2, 2, 2, 0,
                                0, 0, 2, 2, 2, 0, 0, 2, 2, 0, 0, 2, 2, 2, 0, 0,
                                0, 2, 2, 2, 2, 0, 2, 2, 2, 2, 0, 2, 2, 2, 2, 0,
                                0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0,
                                0, 0, 2, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 2, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 0, 0, 0,
                                0, 1, 0, 0, 0, 0, 2, 1, 0, 2, 0, 0, 1, 1, 0, 0,
                                0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0,
                                0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                                0, 0, 0, 0, 2, 0, 2, 1, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;


                case FactionType.BronzeReach:
                    {
                        factionFlavorType = FactionFlavorType.City;
                        col0_Main = new Color(252, 131, 11);
                        col1_Detail1 = new Color(113, 73, 4);
                        col2_Detail2 = new Color(13, 79, 29);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0,
                                0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0,
                                0, 0, 1, 0, 0, 2, 0, 2, 0, 2, 0, 0, 0, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 2, 2, 2, 2, 0, 2, 0, 1, 0, 0,
                                0, 1, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 1, 0,
                                0, 1, 0, 0, 2, 2, 0, 0, 0, 0, 2, 2, 1, 2, 1, 0,
                                0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 1, 0, 0,
                                0, 0, 1, 2, 2, 0, 0, 2, 2, 0, 0, 2, 2, 2, 0, 0,
                                0, 1, 2, 2, 2, 0, 0, 2, 2, 0, 0, 2, 2, 0, 1, 0,
                                0, 1, 0, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 2, 1, 0,
                                0, 0, 2, 0, 2, 2, 0, 0, 0, 0, 2, 2, 0, 1, 0, 0,
                                0, 0, 1, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0,
                                0, 1, 0, 1, 2, 0, 2, 2, 2, 2, 0, 0, 1, 0, 1, 0,
                                0, 1, 0, 1, 0, 0, 2, 0, 2, 0, 2, 0, 1, 0, 1, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                        });
                    }
                    break;

                case FactionType.ElbrethGuild:
                    {
                        factionFlavorType = FactionFlavorType.Sea;
                        col0_Main = new Color(0, 139, 115);
                        col1_Detail1 = new Color(195, 219, 214);
                        col2_Detail2 = new Color(34, 8, 167);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0,
                                1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1,
                                1, 1, 1, 1, 1, 0, 0, 2, 2, 0, 0, 1, 1, 1, 1, 1,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;


                case FactionType.ValosianSenate:
                    {
                        factionFlavorType = FactionFlavorType.City;
                        col0_Main = new Color(146, 50, 16);
                        col1_Detail1 = new Color(89, 105, 111);
                        col2_Detail2 = new Color(206, 239, 240);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1,
                                1, 1, 1, 2, 1, 1, 2, 2, 2, 2, 1, 1, 2, 1, 1, 1,
                                1, 0, 0, 2, 2, 0, 2, 1, 2, 1, 0, 2, 2, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 2, 1, 2, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 2, 1, 2, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 2, 1, 2, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 2, 1, 2, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 2, 1, 2, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 2, 1, 2, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 2, 1, 2, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 2, 1, 2, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 2, 1, 2, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 2, 1, 2, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 2, 2, 0, 2, 1, 2, 1, 0, 2, 2, 0, 0, 1,
                                1, 1, 1, 2, 1, 1, 2, 2, 2, 2, 1, 1, 2, 1, 1, 1,
                                1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1,
                        });
                    }
                    break;


                case FactionType.IronmarchCompact:
                    {
                        factionFlavorType = FactionFlavorType.City;
                        col0_Main = new Color(79, 73, 119);
                        col1_Detail1 = new Color(41, 23, 4);
                        col2_Detail2 = new Color(211, 222, 233);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                0, 0, 1, 1, 0, 0, 0, 2, 2, 0, 0, 0, 0, 1, 1, 0,
                                0, 0, 1, 1, 1, 2, 2, 2, 0, 2, 2, 2, 1, 1, 1, 0,
                                0, 0, 0, 1, 1, 2, 2, 2, 0, 0, 0, 0, 1, 1, 0, 0,
                                0, 0, 0, 2, 2, 1, 2, 2, 0, 0, 0, 1, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 1, 2, 0, 0, 1, 0, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 2, 1, 0, 1, 0, 0, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 2, 2, 1, 0, 0, 0, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 2, 1, 0, 1, 0, 0, 2, 0, 0, 0,
                                0, 0, 0, 0, 2, 2, 1, 2, 0, 0, 1, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 1, 2, 2, 0, 0, 0, 1, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 2, 2, 2, 0, 0, 2, 0, 1, 0, 0, 0,
                                0, 0, 0, 1, 0, 0, 2, 2, 0, 2, 0, 0, 0, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;


                case FactionType.KaranthCollective:
                    {
                        factionFlavorType = FactionFlavorType.City;
                        col0_Main = new Color(53, 87, 122);
                        col1_Detail1 = new Color(245, 157, 52);
                        col2_Detail2 = new Color(233, 233, 174);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
                                0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 0,
                                0, 1, 2, 1, 1, 2, 2, 2, 1, 2, 1, 2, 1, 2, 1, 0,
                                0, 1, 2, 1, 2, 2, 2, 1, 1, 2, 1, 2, 2, 2, 1, 0,
                                0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 0,
                                0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
                                0, 1, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0,
                                0, 1, 2, 2, 2, 1, 2, 1, 1, 2, 2, 2, 1, 2, 1, 0,
                                0, 1, 2, 1, 2, 1, 2, 1, 2, 2, 2, 1, 1, 2, 1, 0,
                                0, 1, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0,
                                0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;


                case FactionType.VerdicAlliance:
                    {
                        factionFlavorType = FactionFlavorType.Forest;
                        col0_Main = new Color(101, 72, 30);
                        col1_Detail1 = new Color(247, 232, 15);
                        col2_Detail2 = new Color(56, 193, 2);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 2, 2, 0, 0,
                                0, 0, 2, 2, 0, 1, 1, 1, 1, 1, 1, 0, 2, 2, 0, 0,
                                0, 0, 2, 2, 0, 1, 1, 1, 2, 1, 1, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 2, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 2, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 2, 0, 2, 1, 1, 1, 1, 0, 0, 2, 2, 0, 0,
                                0, 0, 0, 0, 2, 2, 1, 1, 1, 1, 0, 0, 2, 2, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 2, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 2, 0, 0, 0,
                                0, 2, 2, 0, 2, 0, 1, 1, 1, 2, 0, 0, 2, 0, 2, 0,
                                0, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.OrokhCircles:
                    {
                        factionFlavorType = FactionFlavorType.Mountain;
                        col0_Main = new Color(65, 65, 65);
                        col1_Detail1 = new Color(35, 29, 42);
                        col2_Detail2 = new Color(119, 189, 217);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 2, 2, 0, 0, 0, 2, 2, 0, 0, 0, 2, 2, 0, 0,
                                0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0,
                                0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0,
                                0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0,
                                0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0,
                                0, 0, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1,
                                0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                        });
                    }
                    break;

                case FactionType.TannagHorde:
                    {
                        factionFlavorType = FactionFlavorType.Horse;
                        col0_Main = new Color(85, 146, 113);
                        col1_Detail1 = new Color(3, 107, 49);
                        col2_Detail2 = new Color(252, 239, 34);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 2, 2, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0,
                                0, 2, 2, 0, 2, 0, 2, 2, 2, 2, 2, 2, 0, 0, 2, 0,
                                1, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1,
                                1, 1, 2, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 2, 1, 1,
                                1, 2, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 2, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                        });
                    }
                    break;

                case FactionType.BraghkRaiders:
                    {
                        factionFlavorType = FactionFlavorType.Warrior;
                        col0_Main = new Color(53, 94, 76);
                        col1_Detail1 = new Color(41, 23, 4);
                        col2_Detail2 = new Color(195, 249, 196);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0,
                                0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0,
                                0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0,
                                0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0,
                                0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0,
                                0, 0, 2, 0, 0, 1, 2, 0, 0, 0, 2, 0, 1, 0, 2, 0,
                                2, 2, 0, 2, 1, 2, 0, 2, 2, 2, 0, 2, 2, 2, 0, 2,
                                0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                2, 0, 1, 0, 2, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0,
                                2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                        });
                    }
                    break;

                case FactionType.ThurvanniStonekeepers:
                    {
                        factionFlavorType = FactionFlavorType.Warrior;
                        col0_Main = new Color(95, 54, 43);
                        col1_Detail1 = new Color(34, 41, 48);
                        col2_Detail2 = new Color(230, 43, 25);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 1, 2, 2, 1, 1, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 2, 1, 1, 2, 1, 1, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                                0, 0, 0, 0, 1, 2, 1, 1, 2, 1, 1, 2, 1, 0, 0, 0,
                                0, 0, 0, 0, 1, 2, 1, 2, 1, 2, 1, 1, 1, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 1, 1, 2, 1, 1, 2, 1, 0, 0, 0,
                                0, 0, 0, 0, 1, 2, 1, 1, 2, 1, 1, 2, 1, 0, 0, 0,
                                0, 0, 0, 0, 1, 2, 1, 1, 1, 1, 1, 2, 1, 0, 0, 0,
                                0, 0, 0, 0, 1, 2, 1, 1, 2, 1, 1, 1, 1, 0, 0, 0,
                                0, 0, 0, 0, 1, 2, 1, 1, 1, 1, 1, 2, 1, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.KolvrenHunters:
                    {
                        factionFlavorType = FactionFlavorType.Warrior;
                        col0_Main = new Color(31, 136, 6);
                        col1_Detail1 = new Color(60, 17, 166);
                        col2_Detail2 = new Color(179, 200, 206);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0,
                                0, 1, 0, 0, 0, 0, 2, 0, 2, 0, 2, 0, 0, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 1, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
                                0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.JorathBloodbound:
                    {
                        factionFlavorType = FactionFlavorType.Mountain;
                        col0_Main = new Color(91, 4, 1);
                        col1_Detail1 = new Color(0, 0, 0);
                        col2_Detail2 = new Color(71, 78, 13);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 1, 2, 2, 2, 0, 2, 0, 0, 0, 0,
                                0, 0, 0, 2, 2, 1, 1, 1, 1, 1, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 2, 0, 2, 0, 0,
                                0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                                0, 0, 1, 1, 1, 1, 1, 1, 2, 0, 1, 2, 2, 2, 0, 0,
                                0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 0, 0, 0,
                                0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 2, 2, 2, 0, 0,
                                0, 0, 2, 1, 1, 1, 1, 1, 0, 0, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 0, 0, 0,
                                0, 0, 0, 0, 2, 0, 2, 1, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.UlrethSkycallers:
                    {
                        factionFlavorType = FactionFlavorType.People;
                        col0_Main = new Color(18, 91, 189);
                        col1_Detail1 = new Color(252, 250, 151);
                        col2_Detail2 = new Color(16, 23, 42);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
                                0, 0, 2, 2, 0, 0, 0, 2, 2, 2, 0, 0, 2, 2, 0, 0,
                                0, 2, 2, 2, 2, 0, 0, 2, 2, 0, 0, 2, 2, 2, 2, 0,
                                0, 0, 2, 2, 2, 0, 0, 2, 2, 0, 0, 2, 2, 2, 0, 0,
                                0, 2, 2, 2, 2, 0, 2, 2, 2, 2, 0, 2, 2, 2, 2, 0,
                                0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0,
                                0, 0, 2, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 2, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 1, 0, 0, 2, 2, 1, 2, 1, 0, 0, 0, 0, 0,
                                0, 0, 1, 1, 1, 0, 2, 1, 1, 2, 1, 0, 0, 1, 0, 0,
                                0, 1, 1, 0, 1, 1, 1, 1, 2, 2, 1, 1, 1, 0, 0, 0,
                                0, 1, 0, 0, 0, 1, 2, 2, 2, 0, 0, 1, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.GharjaRavagers:
                    {
                        factionFlavorType = FactionFlavorType.Mountain;
                        col0_Main = new Color(228, 108, 16);
                        col1_Detail1 = new Color(198, 0, 0);
                        col2_Detail2 = new Color(20, 12, 61);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 2, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 2, 2, 1, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 1, 2, 1, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 0, 1, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
                                0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.RavkanShield:
                    {
                        factionFlavorType = FactionFlavorType.Mystical;
                        col0_Main = new Color(48, 71, 57);
                        col1_Detail1 = new Color(150, 217, 205);
                        col2_Detail2 = new Color(175, 235, 98);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 2, 2, 0, 0,
                                0, 2, 0, 0, 2, 2, 0, 2, 2, 0, 0, 2, 0, 2, 2, 0,
                                0, 0, 2, 0, 2, 2, 0, 2, 2, 0, 2, 2, 2, 2, 2, 0,
                                0, 0, 2, 0, 2, 2, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 2, 2, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
                                0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.FenskaarTidewalkers:
                    {
                        factionFlavorType = FactionFlavorType.Mountain;
                        col0_Main = new Color(28, 86, 78);
                        col1_Detail1 = new Color(180, 221, 177);
                        col2_Detail2 = new Color(220, 213, 222);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0,
                                0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 2, 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0,
                                0, 2, 0, 2, 0, 0, 0, 0, 0, 2, 0, 2, 0, 2, 0, 0,
                                0, 2, 0, 0, 0, 0, 2, 2, 0, 2, 0, 0, 0, 2, 0, 0,
                                0, 2, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1,
                                0, 2, 2, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.HroldaniStormguard:
                    {
                        factionFlavorType = FactionFlavorType.Sea;
                        col0_Main = new Color(35, 39, 60);
                        col1_Detail1 = new Color(185, 184, 222);
                        col2_Detail2 = new Color(224, 241, 172);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 1, 0, 0, 0, 0, 2, 2, 0, 0, 0, 1, 0, 0,
                                0, 1, 0, 0, 1, 0, 0, 2, 2, 0, 0, 1, 0, 0, 0, 0,
                                0, 0, 1, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1,
                                1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.SkirnirWolfkin:
                    {
                        factionFlavorType = FactionFlavorType.People;
                        col0_Main = new Color(161, 195, 5);
                        col1_Detail1 = new Color(0, 0, 0);
                        col2_Detail2 = new Color(75, 112, 247);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 2, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0,
                                0, 0, 0, 1, 1, 0, 0, 0, 0, 2, 0, 0, 2, 2, 0, 0,
                                0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
                                0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                                0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.ThalgarBearclaw:
                    {
                        factionFlavorType = FactionFlavorType.Warrior;
                        col0_Main = new Color(124, 52, 23);
                        col1_Detail1 = new Color(19, 74, 8);
                        col2_Detail2 = new Color(138, 199, 213);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0,
                                0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0,
                                0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 2, 1, 2, 1, 2, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 2, 1, 2, 1, 2, 1, 2, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                        });
                    }
                    break;

                case FactionType.VarnokRimeguard:
                    {
                        factionFlavorType = FactionFlavorType.Mystical;
                        col0_Main = new Color(10, 85, 210);
                        col1_Detail1 = new Color(66, 82, 105);
                        col2_Detail2 = new Color(192, 221, 219);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0,
                                0, 0, 0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 0, 0,
                                0, 0, 0, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 2, 0, 0,
                                0, 0, 0, 2, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 0, 0,
                                0, 0, 0, 2, 1, 2, 1, 1, 2, 1, 1, 2, 1, 2, 0, 0,
                                0, 0, 0, 2, 1, 1, 2, 2, 1, 2, 2, 1, 1, 2, 0, 0,
                                0, 0, 0, 2, 1, 2, 1, 1, 2, 1, 1, 2, 1, 2, 0, 0,
                                0, 0, 0, 2, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 0, 0,
                                0, 0, 0, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 2, 0, 0,
                                0, 0, 0, 0, 2, 1, 1, 1, 1, 1, 1, 1, 2, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 1, 1, 1, 1, 1, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 1, 1, 1, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.KorrakFirehand:
                    {
                        factionFlavorType = FactionFlavorType.Desert;
                        col0_Main = new Color(31, 25, 43);
                        col1_Detail1 = new Color(245, 111, 8);
                        col2_Detail2 = new Color(233, 0, 2);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 0,
                                0, 1, 0, 0, 1, 2, 2, 1, 2, 2, 1, 2, 2, 1, 0, 1,
                                0, 0, 0, 0, 1, 2, 2, 1, 2, 2, 1, 2, 2, 1, 0, 0,
                                0, 0, 0, 1, 1, 2, 2, 1, 2, 2, 1, 2, 2, 1, 1, 0,
                                0, 1, 0, 1, 1, 2, 2, 1, 2, 2, 1, 2, 2, 1, 0, 0,
                                0, 0, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0,
                                0, 0, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0,
                                0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0,
                                0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0,
                                0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0,
                                0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0,
                                0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0,
                                0, 0, 0, 1, 1, 1, 2, 0, 0, 0, 0, 2, 1, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.MoongladeGat:
                    {
                        factionFlavorType = FactionFlavorType.Forest;
                        col0_Main = new Color(146, 14, 196);
                        col1_Detail1 = new Color(17, 189, 137);
                        col2_Detail2 = new Color(191, 246, 252);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0,
                                0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0,
                                0, 0, 1, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 0, 0, 0, 2, 2, 2, 2, 0, 1, 0, 0,
                                1, 1, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 1, 1,
                                1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 1, 1,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 1, 0,
                                1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 1, 1,
                                1, 1, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 1, 1,
                                0, 1, 0, 0, 2, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 1, 0, 0,
                                0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0,
                                0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.DraskarSons:
                    {
                        factionFlavorType = FactionFlavorType.Sea;
                        col0_Main = new Color(6, 15, 114);
                        col1_Detail1 = new Color(182, 246, 244);
                        col2_Detail2 = new Color(194, 165, 134);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0,
                                0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0,
                                0, 1, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 1, 0,
                                0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 1, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.YrdenFlamekeepers:
                    {
                        factionFlavorType = FactionFlavorType.Mystical;
                        col0_Main = new Color(244, 43, 45);
                        col1_Detail1 = new Color(122, 1, 18);
                        col2_Detail2 = new Color(235, 246, 38);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 0, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 0, 2, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 0, 2, 0, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 2, 2, 2, 0, 2, 2, 1, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.BrundirWarhorns:
                    {
                        factionFlavorType = FactionFlavorType.Warrior;
                        col0_Main = new Color(186, 148, 0);
                        col1_Detail1 = new Color(248, 247, 224);
                        col2_Detail2 = new Color(42, 0, 0);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0,
                                0, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 1, 2, 0, 0, 0,
                                0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 0, 0,
                                0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
                                0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.OltunBonecarvers:
                    {
                        factionFlavorType = FactionFlavorType.Desert;
                        col0_Main = new Color(192, 163, 130);
                        col1_Detail1 = new Color(15, 177, 191);
                        col2_Detail2 = new Color(61, 20, 68);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 1, 0,
                                0, 0, 1, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 1, 0, 0,
                                0, 0, 0, 0, 1, 2, 1, 2, 2, 1, 2, 1, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 1, 1, 2, 2, 1, 1, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 0, 0, 2, 2, 0, 0, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 0, 0, 1, 1, 0, 0, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 0, 0, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 1, 2, 2, 1, 1, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 0, 2, 0, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 1, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.HaskariEmber:
                    {
                        factionFlavorType = FactionFlavorType.People;
                        col0_Main = new Color(31, 25, 43);
                        col1_Detail1 = new Color(228, 47, 4);
                        col2_Detail2 = new Color(210, 189, 145);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                               0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0,
                                0, 0, 0, 2, 0, 2, 0, 2, 0, 0, 0, 2, 0, 0, 2, 0,
                                0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 2, 0, 0, 2, 0, 0,
                                0, 0, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 0, 2,
                                0, 2, 2, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 2, 2, 0,
                                0, 2, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 2, 0,
                                0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 0,
                                0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 0,
                                0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 0,
                                0, 0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 0, 0,
                                0, 0, 0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 0, 0, 0,
                                0, 0, 0, 0, 2, 1, 1, 1, 1, 1, 1, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 1, 1, 1, 1, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 1, 1, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.ZalfrikThunderborn:
                    {
                        factionFlavorType = FactionFlavorType.People;
                        col0_Main = new Color(32, 10, 113);
                        col1_Detail1 = new Color(158, 162, 247);
                        col2_Detail2 = new Color(225, 228, 217);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                               1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1,
                                1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1,
                                0, 1, 1, 1, 1, 1, 0, 2, 2, 0, 1, 1, 1, 1, 1, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0,
                                1, 0, 0, 1, 1, 2, 2, 0, 1, 1, 0, 0, 1, 1, 0, 0,
                                0, 1, 1, 0, 0, 1, 2, 2, 2, 2, 1, 1, 0, 0, 1, 1,
                                0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.BjorunStonetender:
                    {
                        factionFlavorType = FactionFlavorType.People;
                        col0_Main = new Color(69, 53, 79);
                        col1_Detail1 = new Color(97, 88, 30);
                        col2_Detail2 = new Color(160, 160, 199);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0,
                                0, 0, 0, 0, 2, 2, 1, 2, 2, 1, 2, 1, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 1, 2, 2, 1, 2, 1, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 1, 1, 1, 2, 2, 1, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 1, 2, 2, 2, 2, 1, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 1, 1, 1, 1, 1, 2, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 0, 0, 0,
                                0, 1, 1, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1, 1, 0,
                                0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.MyrdarrIcewalkers:
                    {
                        factionFlavorType = FactionFlavorType.People;
                        col0_Main = new Color(55, 203, 219);
                        col1_Detail1 = new Color(47, 46, 69);
                        col2_Detail2 = new Color(192, 221, 219);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                            0, 0, 0, 0, 0, 0, 2, 2, 0, 2, 2, 0, 0, 0, 0, 0,
                            0, 0, 0, 0, 2, 2, 1, 1, 1, 1, 1, 2, 2, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 0, 0,
                            0, 0, 2, 2, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 2, 1, 1, 2, 1, 1, 2, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 1, 2, 2, 1, 2, 2, 1, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 2, 1, 1, 2, 1, 1, 2, 1, 1, 0, 0,
                            0, 0, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 0, 0,
                            0, 0, 0, 1, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                            0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.SkelvikSpear:
                    {
                        factionFlavorType = FactionFlavorType.Warrior;
                        col0_Main = new Color(246, 204, 151);
                        col1_Detail1 = new Color(49, 43, 108);
                        col2_Detail2 = new Color(114, 141, 206);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
                                0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0,
                                0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0,
                                0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.VaragThroatcallers:
                    {
                        factionFlavorType = FactionFlavorType.People;
                        col0_Main = new Color(37, 102, 51);
                        col1_Detail1 = new Color(219, 87, 53);
                        col2_Detail2 = new Color(206, 125, 44);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 0, 2, 0, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 2, 2, 0, 1, 1, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 2, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 2, 2, 2, 2, 0, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0,
                                0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0,
                                0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 2, 2, 2, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 2, 2, 0, 0,
                                0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 2, 2, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 2, 2, 2, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        });
                    }
                    break;

                case FactionType.Durakai:
                    {
                        factionFlavorType = FactionFlavorType.Desert;
                        col0_Main = new Color(240, 216, 48);
                        col1_Detail1 = new Color(238, 75, 23);
                        col2_Detail2 = new Color(216, 111, 8);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
                                0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                                0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                                0, 2, 2, 1, 1, 2, 2, 1, 1, 2, 2, 1, 1, 2, 2, 0,
                                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                        });
                    }
                    break;

                case FactionType.FjornfellWarhowl:
                    {
                        factionFlavorType = FactionFlavorType.Warrior;
                        col0_Main = new Color(50, 108, 248);
                        col1_Detail1 = new Color(20, 2, 71);
                        col2_Detail2 = new Color(70, 196, 251);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 2, 2, 0, 1, 1, 1, 0, 2, 2, 0, 0, 2, 2, 0, 0,
                                2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2,
                                2, 2, 2, 2, 1, 1, 1, 0, 2, 1, 1, 1, 1, 2, 2, 2,
                                2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2,
                                2, 2, 1, 1, 1, 1, 1, 1, 2, 2, 2, 1, 2, 2, 2, 2,
                                2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2,
                                2, 2, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 2, 2, 2, 2,
                                2, 1, 2, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2,
                                2, 2, 1, 2, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2,
                                2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2,
                                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                        });
                    }
                    break;

                case FactionType.AshgroveWard:
                    {
                        factionFlavorType = FactionFlavorType.Forest;
                        col0_Main = new Color(19, 120, 42);
                        col1_Detail1 = new Color(122, 215, 144);
                        col2_Detail2 = new Color(255, 255, 255);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                                1, 1, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 1, 1,
                                1, 1, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 1, 1,
                                0, 1, 0, 2, 0, 0, 1, 0, 0, 1, 0, 0, 2, 0, 1, 0,
                                0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0,
                                0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0,
                                0, 0, 2, 0, 2, 0, 2, 0, 0, 2, 0, 2, 0, 2, 0, 0,
                                0, 0, 2, 0, 2, 0, 2, 0, 0, 2, 0, 2, 0, 2, 0, 0,
                                0, 0, 0, 2, 2, 0, 2, 0, 0, 2, 0, 2, 2, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0,
                                0, 1, 1, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 1, 1, 0,
                                0, 1, 1, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 1, 1, 0,
                                0, 0, 0, 1, 0, 0, 2, 2, 2, 2, 0, 0, 1, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
                                0, 1, 1, 0, 0, 2, 0, 2, 2, 0, 2, 0, 0, 1, 1, 0,
                                0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0,
                        });
                    }
                    break;

                case FactionType.HragmarHorncarvers:
                    {
                        factionFlavorType = FactionFlavorType.People;
                        col0_Main = new Color(118, 121, 45);
                        col1_Detail1 = new Color(193, 137, 56);
                        col2_Detail2 = new Color(202, 166, 132);

                        col3_Skin = Color.LightGray;
                        col4_Hair = Color.DarkGray;

                        flagDesign = new FlagDesign(new byte[]
                        {
                               0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0,
                                0, 0, 0, 2, 2, 0, 0, 0, 2, 2, 1, 2, 2, 0, 0, 0,
                                0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0,
                                0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 1, 2, 0, 0,
                                0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0,
                                0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0,
                                0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0,
                                0, 0, 2, 2, 1, 2, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0,
                                0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 2, 2, 0, 0,
                                0, 0, 0, 0, 2, 1, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0,
                                0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
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
