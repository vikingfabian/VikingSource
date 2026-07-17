using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Net;
using System.Text;
using System.Threading;
using VikingEngine.DSSWars.Data;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.GO.WeaponAttack.ItemThrow;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.PJ;

namespace VikingEngine.DSSWars.Players.Profile
{
    class FlagAndColor
    {
        public static readonly int ColorCount = (int)ProfileColorType.NUM;
        public static readonly ColorRange AiColorRange = new ColorRange(new Color(new Vector3(0.1f)), new Color(new Vector3(0.9f)));

        public static AppearanceMaterial
            SkinCol, HairCol, MainCol, AltMainCol, DetailCol1, DetailCol2, TunicCol, PantsCol, LeaderCol;

        public static readonly Color AiGraySkin = new Color(206, 162, 126);
        public static readonly Color AiGrayHair = new Color(103, 81, 63);
        static readonly Color DefaultPlayerSkin = new Color(224, 168, 123);
        public static void Init()
        {
            //Detta är färgerna som ersätts

            SkinCol = new AppearanceMaterial(Color.Gray, true);
            HairCol = new AppearanceMaterial(Color.Brown, false);

            MainCol = new AppearanceMaterial(new Color(65, 74, 129), false); //Blå
            AltMainCol = new AppearanceMaterial(new Color(25, 54, 109), false); //Mörk Blå

            DetailCol1 = new AppearanceMaterial(new Color(133, 78, 65), false);//Röd
            DetailCol2 = new AppearanceMaterial(new Color(65, 133, 69), false);//Grön

            TunicCol = new AppearanceMaterial(Color.Yellow, false);
            PantsCol = new AppearanceMaterial(Color.GreenYellow, false);
            LeaderCol = new AppearanceMaterial(new Color(50, 25, 0), false); //Dark brown


            BlockHD.JointUp = BlockHD.ToBlockValue(new Color(128, 0, 128), BlockHD.ReplaceMaterial); //purple
            BlockHD.JointForward = BlockHD.ToBlockValue(new Color(170, 0, 128), BlockHD.ReplaceMaterial); //red purple
            BlockHD.JointBack = BlockHD.ToBlockValue(new Color(128, 0, 170), BlockHD.ReplaceMaterial); //blue purple
        }

        public int StorageIndex;
        public Color col0_Main;
        public Color col1_Detail1;
        public Color col2_Detail2;
        public Color col5_AltMain;
        public Color col3_Skin;
        public Color col4_Hair;
       
        public Color col6_Tunic = new Color(129,119,103);
        public Color col7_Pants = new Color(145, 114, 0);
        public Color col8_Leader = Color.SaddleBrown;

        public FlagDesign flagDesign;
        public FactionFlavorType factionFlavorType = FactionFlavorType.Other;

        public CharacterProfile character = new CharacterProfile();


        public void autoAltColor()
        {
            col0_Main.Deconstruct(out byte r, out byte g, out byte b);
            col5_AltMain = new Color(adjust(r), adjust(g), adjust(b));

            int adjust(byte col)
            {
                if (col > 200)
                {
                    return col - 40;
                }
                return col + 40;
            }
        }

        public FlagAndColor(FactionType factiontype, int index, WorldMetaData worldMeta, int factionIndex = -1)
        {
            this.StorageIndex = index;
            
            switch (factiontype)
            {
                case FactionType.DefaultAi:
                    {
                        worldMeta.objRnd.SetSeed(factionIndex);

                        var color1 = AiColorRange.GetRandom(worldMeta.objRnd);
                        var color2 = AiColorRange.GetRandom(worldMeta.objRnd);

                        col0_Main = color1;
                        col1_Detail1 = color2;
                        col2_Detail2 = Color.Gray;

                        col3_Skin = AiGraySkin;
                        col4_Hair = AiGrayHair;

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

                        col3_Skin = DefaultPlayerSkin;
                        col4_Hair = Color.Brown;

                        if (flagDesign == null)
                        {
                            flagDesign = new FlagDesign();
                        }
                    }
                    break;

                default:
                    FlagAndColor_NamedFaction.InitFlag(factiontype, this);
                    break;


            }

            autoAltColor();
        }

        

        public FlagAndColor(System.IO.BinaryReader r)
        {
            read(r);
        }

        public FlagAndColor Clone()
        {
            FlagAndColor clonedData = new FlagAndColor(FactionType.Player, StorageIndex, null)
            {
                col0_Main = col0_Main,
                col1_Detail1 = col1_Detail1,
                col2_Detail2 = col2_Detail2,
                col3_Skin = col3_Skin,
                col4_Hair = col4_Hair,
                col5_AltMain = col5_AltMain,
                col6_Tunic = col6_Tunic,
                col7_Pants = col7_Pants,
                col8_Leader = col8_Leader,
                flagDesign = flagDesign != null ? flagDesign.CloneFlag() : null,
            };

            return clonedData;
        }



        public void FillBlockColors(Span<ushort> buffer)
        {
            buffer[0] = BlockHD.ToBlockValue(col0_Main, BlockHD.DefaultMaterial);
            buffer[1] = BlockHD.ToBlockValue(col1_Detail1, BlockHD.DefaultMaterial);
            buffer[2] = BlockHD.ToBlockValue(col2_Detail2, BlockHD.DefaultMaterial);
            buffer[3] = BlockHD.ToBlockValue(col3_Skin, BlockHD.DefaultMaterial);
            buffer[4] = BlockHD.ToBlockValue(col4_Hair, BlockHD.DefaultMaterial);
            buffer[5] = BlockHD.ToBlockValue(col5_AltMain, BlockHD.DefaultMaterial);
            buffer[6] = BlockHD.ToBlockValue(col6_Tunic, BlockHD.DefaultMaterial);
            buffer[7] = BlockHD.ToBlockValue(col7_Pants, BlockHD.DefaultMaterial);
            buffer[8] = BlockHD.ToBlockValue(col8_Leader, BlockHD.DefaultMaterial);
        }



        public Dictionary<ushort, ushort> GetColorReplaceTable()
        {
            Dictionary<ushort, ushort> result = new Dictionary<ushort, ushort>(32);

            ColorReplaceTable(result);

            return result;
        }
        public void ColorReplaceTable(Dictionary<ushort, ushort> findReplace)
        {
           

            addColor(ref MainCol, ref col0_Main);
            addColor(ref DetailCol1, ref col1_Detail1);
            addColor(ref DetailCol2, ref col2_Detail2);
            addColor(ref SkinCol, ref col3_Skin);
            addColor(ref HairCol, ref col4_Hair);
            addColor(ref AltMainCol, ref col5_AltMain);
            addColor(ref TunicCol, ref col6_Tunic);
            addColor(ref PantsCol, ref col7_Pants);
            addColor(ref LeaderCol, ref col8_Leader);

            void addColor(ref AppearanceMaterial material, ref Color color)
            {
                BlockHD baseCol = new BlockHD(color);
                BlockHD dark = baseCol;
                dark.tintSteps(-1, -1, -1);
                BlockHD bright = baseCol;
                bright.tintSteps(1, 1, 1);

                findReplace.Add(material.baseColor, baseCol.BlockValue);
                findReplace.Add(material.darker, dark.BlockValue);
                findReplace.Add(material.brighter, bright.BlockValue);

                if (material.redTint != BlockHD.EmptyBlock)
                {
                    BlockHD red = baseCol;
                    red.tintSteps(1, 0, 0);

                    findReplace.Add(material.redTint, red.BlockValue);
                }
            }
        }

        public void PrintFlagColors()
        {
            System.Diagnostics.Debug.WriteLine($"col0_Main = new Color({col0_Main.R}, {col0_Main.G}, {col0_Main.B});");
            System.Diagnostics.Debug.WriteLine($"col1_Detail1 = new Color({col1_Detail1.R}, {col1_Detail1.G}, {col1_Detail1.B});");
            System.Diagnostics.Debug.WriteLine($"col2_Detail2 = new Color({col2_Detail2.R}, {col2_Detail2.G}, {col2_Detail2.B});");
            System.Diagnostics.Debug.WriteLine("-");
            System.Diagnostics.Debug.WriteLine($"col5_AltMain = new Color({col5_AltMain.R}, {col5_AltMain.G}, {col5_AltMain.B});");
            System.Diagnostics.Debug.WriteLine($"col3_Skin = new Color({col3_Skin.R}, {col3_Skin.G}, {col3_Skin.B});");
            System.Diagnostics.Debug.WriteLine($"col4_Hair = new Color({col4_Hair.R}, {col4_Hair.G}, {col4_Hair.B});");
            System.Diagnostics.Debug.WriteLine($"col6_Tunic = new Color({col6_Tunic.R}, {col6_Tunic.G}, {col6_Tunic.B});");
            System.Diagnostics.Debug.WriteLine($"col7_Pants = new Color({col7_Pants.R}, {col7_Pants.G}, {col7_Pants.B});");
            System.Diagnostics.Debug.WriteLine($"col8_Leader = new Color({col8_Leader.R}, {col8_Leader.G}, {col8_Leader.B});");


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
                case ProfileColorType.AltMain:
                    col5_AltMain = color;
                    break;
                case ProfileColorType.Tunic:
                    col6_Tunic = color;
                    break;
                case ProfileColorType.Pants:
                    col7_Pants = color;
                    break;
                case ProfileColorType.Leader:
                    col8_Leader = color;
                    break;
            }
        }


        public Color getColor(ProfileColorType type)
        {
            return type switch
            {
                ProfileColorType.Main => col0_Main,
                ProfileColorType.Detail1 => col1_Detail1,
                ProfileColorType.Detail2 => col2_Detail2,
                ProfileColorType.Skin => col3_Skin,
                ProfileColorType.Hair => col4_Hair,
                ProfileColorType.AltMain => col5_AltMain,
                ProfileColorType.Tunic => col6_Tunic,
                ProfileColorType.Pants => col7_Pants,
                ProfileColorType.Leader => col8_Leader,
                _ => throw new NotImplementedException(),
            };
        }


        public void Button(GuiLayout layout, IGuiAction action, bool moreArrow)
        {
            var button = new GuiIconTextButton(SpriteName.MissingImage, string.Format( DssRef.lang.Lobby_FlagNumbered ,StorageIndex+1),
                null, action, moreArrow, layout);

            button.icon.Texture = flagDesign.CreateTexture(this);
            button.icon.SetFullTextureSource();
        }

        public DropDownOption RbButton()
        {
            DropDownOption result = new DropDownOption();
            result.Add(new RbTexture(flagDesign.CreateTexture(this)));
            result.Add(new RbSpace());
            result.Add(new RbText(string.Format(DssRef.lang.Lobby_FlagNumbered, StorageIndex + 1)));
            return result;
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
            col0_Main = StreamLib.ReadColorStream_3B(r);
            col1_Detail1=StreamLib.ReadColorStream_3B(r);
            col2_Detail2 = StreamLib.ReadColorStream_3B(r);
            col3_Skin = StreamLib.ReadColorStream_3B(r);
            col4_Hair = StreamLib.ReadColorStream_3B(r);

            flagDesign.read(r);
        }

        const int Version = 4;
        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Version); // Current version is 4

            StreamLib.WriteColorStream_3B(w, col0_Main);
            StreamLib.WriteColorStream_3B(w, col1_Detail1);
            StreamLib.WriteColorStream_3B(w, col2_Detail2);
            StreamLib.WriteColorStream_3B(w, col3_Skin);
            StreamLib.WriteColorStream_3B(w, col4_Hair);
            StreamLib.WriteColorStream_3B(w, col5_AltMain);
            StreamLib.WriteColorStream_3B(w, col6_Tunic);
            StreamLib.WriteColorStream_3B(w, col7_Pants);
            StreamLib.WriteColorStream_3B(w, col8_Leader);

            flagDesign.write(w);
        }


        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();
            if (version > Version) { return; }

            col0_Main = StreamLib.ReadColorStream_3B(r);
            col1_Detail1 = StreamLib.ReadColorStream_3B(r);
            col2_Detail2 = StreamLib.ReadColorStream_3B(r);
            col3_Skin = StreamLib.ReadColorStream_3B(r);
            col4_Hair = StreamLib.ReadColorStream_3B(r);

            if (version >= 3)
            {
                col5_AltMain = StreamLib.ReadColorStream_3B(r);
            }
            else
            {
                col5_AltMain = Color.White;
            }

            if (version >= 4)
            {
                col6_Tunic = StreamLib.ReadColorStream_3B(r);
                col7_Pants = StreamLib.ReadColorStream_3B(r);
                col8_Leader = StreamLib.ReadColorStream_3B(r);
            }

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
        AltMain,
        Tunic, 
        Pants, 
        Leader,
        NUM
    }
}
