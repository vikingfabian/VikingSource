using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VikingEngine.HUD;
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
        public Color[] colors;
        public ushort[] blockColors;
        public FlagDesign flagDesign;
        public List<BlockHDPair> modelColorReplace;



        public FlagAndColor(FactionType factiontype, int index)
        {
            this.index = index;
            colors = new Color[ColorCount];

            switch (factiontype)
            {
                case FactionType.DefaultAi:
                    {
                        var color1 = AiColorRange.GetRandom();
                        var color2 = AiColorRange.GetRandom();

                        setColor(ProfileColorType.Main, color1);
                        setColor(ProfileColorType.Detail1, color2);
                        setColor(ProfileColorType.Detail2, Color.Gray);

                        setColor(ProfileColorType.Skin, Color.LightGray);
                        setColor(ProfileColorType.Hair, Color.DarkGray);

                        flagDesign = arraylib.RandomListMember(FlagDesign.AiBanner);
                    }
                    break;


                case FactionType.Player:
                    {
                        switch (index)
                        {
                            case 0:
                                setColor(ProfileColorType.Main, Color.Blue);
                                setColor(ProfileColorType.Detail1, Color.Yellow);
                                setColor(ProfileColorType.Detail2, Color.Orange);
                                break;

                            case 1:
                                setColor(ProfileColorType.Main, Color.Red);
                                setColor(ProfileColorType.Detail1, Color.MediumPurple);
                                setColor(ProfileColorType.Detail2, Color.Blue);
                                break;

                            case 2:
                                setColor(ProfileColorType.Main, Color.Green);
                                setColor(ProfileColorType.Detail1, Color.Yellow);
                                setColor(ProfileColorType.Detail2, Color.YellowGreen);
                                break;

                            case 3:
                                setColor(ProfileColorType.Main, Color.Orange);
                                setColor(ProfileColorType.Detail1, Color.Pink);
                                setColor(ProfileColorType.Detail2, Color.Brown);
                                break;

                            case 4:
                                setColor(ProfileColorType.Main, new Color(63, 79, 63));
                                setColor(ProfileColorType.Detail1, new Color(0, 0, 0));
                                setColor(ProfileColorType.Detail2, new Color(220, 213, 222));

                                flagDesign = FlagDesign.PlayerGriffin;
                                break;

                            case 5:
                                setColor(ProfileColorType.Main, new Color(139, 2, 2));
                                setColor(ProfileColorType.Detail1, new Color(181, 133, 94));
                                setColor(ProfileColorType.Detail2, new Color(220, 213, 222));

                                flagDesign = FlagDesign.PlayerGriffin;
                                break;

                            case 6:
                                setColor(ProfileColorType.Main, new Color(46, 73, 94));
                                setColor(ProfileColorType.Detail1, new Color(99, 175, 174));
                                setColor(ProfileColorType.Detail2, new Color(243, 232, 191));

                                flagDesign = FlagDesign.PlayerGriffin;
                                break;

                            case 7:
                                setColor(ProfileColorType.Main, new Color(98, 42, 52));
                                setColor(ProfileColorType.Detail1, new Color(205, 193, 68));
                                setColor(ProfileColorType.Detail2, new Color(240, 193, 193));

                                flagDesign = FlagDesign.PlayerGriffin;
                                break;



                            default:
                                setColor(ProfileColorType.Main, Color.DarkGray);
                                setColor(ProfileColorType.Detail1, Color.Brown);
                                setColor(ProfileColorType.Detail2, Color.LightGray);
                                break;
                        }

                        setColor(ProfileColorType.Skin, Color.Beige);
                        setColor(ProfileColorType.Hair, Color.Brown);

                        if (flagDesign == null)
                        {
                            flagDesign = new FlagDesign();
                        }
                    }
                    break;

                case FactionType.UnitedKingdom:
                    setColor(ProfileColorType.Main, new Color(248,248,216));
                    setColor(ProfileColorType.Detail1, new Color(120, 72, 8));
                    setColor(ProfileColorType.Detail2, new Color(120, 40, 8));

                    setColor(ProfileColorType.Skin, Color.LightGray);
                    setColor(ProfileColorType.Hair, Color.DarkGray);

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
                        setColor(ProfileColorType.Main, new Color(56, 8, 56));
                        setColor(ProfileColorType.Detail1, new Color(216,8,8));
                        setColor(ProfileColorType.Detail2, new Color(56,8,8));

                        setColor(ProfileColorType.Skin, Color.LightGreen);
                        setColor(ProfileColorType.Hair, Color.DarkGreen);

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
                        setColor(ProfileColorType.Main, new Color(56, 88, 8));
                        setColor(ProfileColorType.Detail1, new Color(248, 200, 24));
                        setColor(ProfileColorType.Detail2, new Color(168, 216, 24));

                        setColor(ProfileColorType.Skin, Color.LightGray);
                        setColor(ProfileColorType.Hair, Color.DarkGray);

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
                        setColor(ProfileColorType.Main, new Color(184, 8, 8));
                        setColor(ProfileColorType.Detail1, new Color(248, 184, 8));
                        setColor(ProfileColorType.Detail2, new Color(248, 248, 152));

                        setColor(ProfileColorType.Skin, Color.LightGray);
                        setColor(ProfileColorType.Hair, Color.DarkGray);

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
                        setColor(ProfileColorType.Main, new Color(24, 168, 248));
                        setColor(ProfileColorType.Detail1, new Color(8, 120, 184));
                        setColor(ProfileColorType.Detail2, new Color(232, 232, 120));

                        setColor(ProfileColorType.Skin, Color.LightGray);
                        setColor(ProfileColorType.Hair, Color.OrangeRed);

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
                        setColor(ProfileColorType.Main, new Color(200, 88, 24));
                        setColor(ProfileColorType.Detail1, new Color(8, 56, 120));
                        setColor(ProfileColorType.Detail2, new Color(232,232, 120));

                        setColor(ProfileColorType.Skin, Color.LightGray);
                        setColor(ProfileColorType.Hair, Color.OrangeRed);

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
                        setColor(ProfileColorType.Main, new Color(200, 88, 24));
                        setColor(ProfileColorType.Detail1, new Color(56, 40, 8));
                        setColor(ProfileColorType.Detail2, new Color(232, 232, 120));

                        setColor(ProfileColorType.Skin, Color.LightGray);
                        setColor(ProfileColorType.Hair, Color.OrangeRed);

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
                        setColor(ProfileColorType.Main, new Color(8, 40, 88));
                        setColor(ProfileColorType.Detail1, new Color(152, 184, 248));
                        setColor(ProfileColorType.Detail2, new Color(8, 8, 56));

                        setColor(ProfileColorType.Skin, Color.LightGray);
                        setColor(ProfileColorType.Hair, Color.OrangeRed);

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

                case FactionType.DragonSlayer:
                    {
                        setColor(ProfileColorType.Main, new Color(24, 56, 8));
                        setColor(ProfileColorType.Detail1, new Color(216, 8, 88));
                        setColor(ProfileColorType.Detail2, new Color(8, 24, 56));

                        setColor(ProfileColorType.Skin, Color.LightGray);
                        setColor(ProfileColorType.Hair, Color.DarkGray);

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
                        setColor(ProfileColorType.Main, new Color(24, 8, 8));
                        setColor(ProfileColorType.Detail1, new Color(232, 216, 88));
                        setColor(ProfileColorType.Detail2, new Color(136, 8, 152));

                        setColor(ProfileColorType.Skin, Color.LightGreen);
                        setColor(ProfileColorType.Hair, Color.DarkGreen);

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
                        setColor(ProfileColorType.Main, new Color(184,40,8));
                        setColor(ProfileColorType.Detail1, new Color(0,0,0));
                        setColor(ProfileColorType.Detail2, new Color(248,200,24));

                        setColor(ProfileColorType.Skin, Color.LightGreen);
                        setColor(ProfileColorType.Hair, Color.DarkGreen);

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
            }
            

            //if (factiontype == FactionType.Player)
            //{
            //    switch (index)
            //    {
            //        case 0:
            //            setColor(ProfileColorType.Main, Color.Blue);
            //            setColor(ProfileColorType.Detail1, Color.Yellow);
            //            setColor(ProfileColorType.Detail2, Color.Orange);
            //            break;

            //        case 1:
            //            setColor(ProfileColorType.Main, Color.Red);
            //            setColor(ProfileColorType.Detail1, Color.MediumPurple);
            //            setColor(ProfileColorType.Detail2, Color.Blue);
            //            break;

            //        case 2:
            //            setColor(ProfileColorType.Main, Color.Green);
            //            setColor(ProfileColorType.Detail1, Color.Yellow);
            //            setColor(ProfileColorType.Detail2, Color.YellowGreen);
            //            break;

            //        case 3:
            //            setColor(ProfileColorType.Main, Color.Orange);
            //            setColor(ProfileColorType.Detail1, Color.Pink);
            //            setColor(ProfileColorType.Detail2, Color.Brown);
            //            break;
            //    }

            //    setColor(ProfileColorType.Skin, Color.Beige);
            //    setColor(ProfileColorType.Hair, Color.Brown);

            //    flagDesign = new FlagDesign();
            //}
            //else if (factiontype == FactionType.DarkLord)
            //{
            //    setColor(ProfileColorType.Main, new Color(24, 8, 8));
            //    setColor(ProfileColorType.Detail1, new Color(232, 216, 88));
            //    setColor(ProfileColorType.Detail2, new Color(136, 8, 152));

            //    setColor(ProfileColorType.Skin, Color.LightGreen);
            //    setColor(ProfileColorType.Hair, Color.DarkGreen);

            //    flagDesign = FlagDesign.EvilBanner;
            //}
            //else
            //{
            //    var color1 = AiColorRange.GetRandom();
            //    var color2 = AiColorRange.GetRandom();

            //    setColor(ProfileColorType.Main, color1);
            //    setColor(ProfileColorType.Detail1, color2);
            //    setColor(ProfileColorType.Detail2, Color.Gray);

            //    setColor(ProfileColorType.Skin, Color.LightGray);
            //    setColor(ProfileColorType.Hair, Color.DarkGray);

            //    flagDesign = arraylib.RandomListMember(FlagDesign.AiBanner);
            //}
        }

        public FlagAndColor(System.IO.BinaryReader r)
        {
            read_old(r);
        }

        public FlagAndColor Clone()
        {
            FlagAndColor clonedData = new FlagAndColor(FactionType.Player, this.index)
            {
                colors = this.colors != null ? (Color[])this.colors.Clone() : null,
                blockColors = this.blockColors != null ? (ushort[])this.blockColors.Clone() : null,
                flagDesign = this.flagDesign != null ? this.flagDesign.Clone() : null,
                modelColorReplace = this.modelColorReplace != null ? new List<BlockHDPair>(this.modelColorReplace) : null
            };

            return clonedData;
        }

        public void gameStartInit()
        {
            

            blockColors = new ushort[colors.Length];
            for (int i = 0; i < blockColors.Length; ++i)
            {
                blockColors[i] = BlockHD.ToBlockValue(colors[i], BlockHD.UnknownMaterial);
            }

            var mainCol = getColor(ProfileColorType.Main);

            BlockHD main = new BlockHD(mainCol);
            BlockHD darkMain = main;
            darkMain.tintSteps(-1, -1, -1);

            Color altCol;
            if (ColorExt.GetBrightNess(mainCol) >= 0.3f)
            {
                altCol = ColorExt.ChangeBrighness(mainCol, -30);
            }
            else
            {
                altCol = ColorExt.ChangeBrighness(mainCol, 30);
            }
            BlockHD mainAlt = new BlockHD(altCol);

            var skinCol = getColor(ProfileColorType.Skin);
            BlockHD skin = new BlockHD(skinCol);
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
                     getColor(ProfileColorType.Detail1), BlockHD.UnknownMaterial)),

                new BlockHDPair(DetailCol2.baseColor, BlockHD.ToBlockValue(
                     getColor(ProfileColorType.Detail2), BlockHD.UnknownMaterial)),

                new BlockHDPair(SkinCol.baseColor, skin.BlockValue),
                new BlockHDPair(SkinCol.darker, darkskin.BlockValue),
                new BlockHDPair(SkinCol.redTint, redskin.BlockValue),

                new BlockHDPair(HairCol.baseColor, BlockHD.ToBlockValue(
                     getColor(ProfileColorType.Hair), BlockHD.UnknownMaterial)),
            };
            
        }

        public void PrintFlagColors()
        {
            ProfileColorType[] colors = { ProfileColorType.Main, ProfileColorType.Detail1, ProfileColorType.Detail2 };
            foreach (var col in colors)
            {
                System.Diagnostics.Debug.WriteLine(col.ToString() + ": " + getColor(col).ToString());
            }
        }

        public void setColor(ProfileColorType type, Color color)
        {
            colors[(int)type] = color;
        }

        public Color getColor(ProfileColorType type)
        {
            return colors[(int)type];
        }

        public void Button(GuiLayout layout, IGuiAction action, bool moreArrow)
        {
            var button = new GuiIconTextButton(SpriteName.MissingImage, string.Format( DssRef.lang.Lobby_ProfileNumbered ,index+1),
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
            for (int i = 0; i < ColorCount; ++i)
            {
                colors[i] = SaveLib.ReadColorStream_3B(r);
            }

            flagDesign.read(r);
        }

        public void write(System.IO.BinaryWriter w)
        {
            const int Version = 2;

            w.Write(Version);

            for (int i = 0; i < ColorCount; ++i)
            {
                SaveLib.WriteColorStream_3B(w, colors[i]);
            }

            flagDesign.write(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();

            for (int i = 0; i < ColorCount; ++i)
            {
                colors[i] = SaveLib.ReadColorStream_3B(r);
            }

            flagDesign.read(r);
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
