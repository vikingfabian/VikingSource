using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;

namespace VikingEngine
{
    static class EnumExtensions
    {
        public static bool RepresentsNegativeDirection(this Dir4 dir)
        {
            return dir == Dir4.N || dir == Dir4.W;
        }

        public static Dir4 OppositeDir(this Dir4 dir)
        {
            switch(dir)
            {
                case Dir4.E:
                    return Dir4.W;
                case Dir4.N:
                    return Dir4.S;
                case Dir4.S:
                    return Dir4.N;
                case Dir4.W:
                    return Dir4.E;
                default:
                    throw new ArgumentException("Expected a cardinal direction to oppose.");
            }
        }

        public static IntVector2 ToIntVec(this Dir4 dir, int length)
        {
            return conv.ToIntV2(dir, length);
        }

        public static Vector2 ToVec(this Dir4 dir, float length)
        {
            return conv.ToIntV2(dir, 1).Vec * length;
        }
    }


    public enum TimeUnit
    { 
        Milliseconds,
        Seconds,
        Minutes,
        Hours,
        Days,
        Weeks,
        Months,
        Years,
        NUM_NON
    }
    
    public enum LoadedSound
    {
        NON,
        Sword1, Sword2, Sword3,
        shieldcrash, out_of_ammo,
        Coin1, Coin2, Coin3,
        TakeDamage1, TakeDamage2,
        EnemyProj1, EnemyProj2,
        HealthUp,
        Bow,

        MonsterHit1, MonsterHit2,
        deathpop,
        weaponclink,
        MenuMove, 
        MenuSelect,
        MenuBack,
        MenuNotAllowed,
        CraftSuccessful,
        Dialogue_Neutral,
        Dialogue_DidYouKnow,
        Dialogue_Question,
        Dialogue_QuestAccomplished,
        Trophy,
        PickUp,
        //Test,
        open_map,
        //DESIGN
        block_place_1,
        buy,
        chat_message,
        enter_build,
        SmallExplosion,
        
        player_enters,
        tool_dig,
        tool_select,
        door,
        
        express_anger,
        express_hi1,
        express_hi2,
        express_hi3,
        express_laugh,
        express_teasing1,
        express_teasing2,
        express_thumbup1,
        express_thumbup2,

        smack,
        SmackEchoes,
        smallSmack,
        flap,
        flowerfire,
        minefire,

        birdToasty,
        birdTimesUp,
        violin_pluck,
        bass_pluck,
        bassdrop,
        wolfScare,

        #region BG_LIVE
        dice1,
        dice2,
        dice3,
        pickupPiece,
        placePiece,
        #endregion
        #region CCG
        //SoundTut2/5: namnge ditt ljud i enumen här
        PlaceCard_Elf_0_3,
        PlaceCard_Elf_4_6,
        PlaceCard_Elf_7_10,

        PlaceCard_Human_0_3,
        PlaceCard_Human_4_6,
        PlaceCard_Human_7_10,

        PlaceCard_Neutral_0_3,
        PlaceCard_Neutral_4_6,
        PlaceCard_Neutral_7_10,

        PlaceCard_Orc_0_3,
        PlaceCard_Orc_4_6,
        PlaceCard_Orc_7_10,

        PlaceCard_Undead_0_3,
        PlaceCard_Undead_4_6,
        PlaceCard_Undead_7_10,

        AtkDown,
        AtkUp,
        TakeDamage,
        Death,
        Freeze,
        Heal,
        Hide,
        SwitchRow,

        InfoShow,
        InfoHide,

        HitMagic,
        HitMelee,
        RangedFire,
        RangedHit,

        SelectStartingHand,
        ChooseManaCards,
        PlacePiece,
        SelectAttacker,
        
        TimerTickTock,

        MatchStart,
        MatchWin,
        MatchDraw,
        MatchLose,
        TurnShift,

        Card_draw,
        Card_put,

        CcgNotAvailable,
        BongaBong,
        #endregion

        // LF3 NEW
        MenuLo100MS,
        MenuHi100MS,
        FastSwing,
        LargeExplosion,
        Melee,

        order1,
        order2,
        order3,
        order4,
        order5,
        order6,
        order7,
        order8,
        warsbuild,
        warsbuildingdestroy,
        warscatapult,
        warsJavelin,
        warsKnifethrow,

        NUM
    };
    public enum LoadedFont
    {
        Regular,
        Bold,
        Console,
        NUM_NON,
    };
    public enum LoadedEffect
    {
        //TestFontArial,
        ParticleEffect,
        LightParticleEffect,
        NUM_NoEffect,
    };
   
    public enum Corner
    {
        NW, NE,
        SE, SW, 
        NUM,
        NO_CORNER,
    }

    [Flags]
    public enum DirectionFlags
    {
        NONE = 0,
        Special = 1,
        North = 2,
        East = 4,
        South = 8,
        West = 16
    }

    public enum EnableType
    {
        Enabled,
        Able_NotRecommended,
        Disabled,
        Locked,
        Hidden,
        NON
    }

    public enum Dir4
    {
        N = 0,
        E,
        S,
        W,
        NUM_NON,
    }
    public enum Dir8
    {
        N = 0,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW,
        NUM,
        NO_DIR,
    }

    /// <summary>
    /// For aligning objects in space
    /// </summary>
    public enum RelativePlacement
    {
        Behind,
        Centered,
        InFront,
        NUM
    }

    /// <summary>
    /// The six direction around a cube
    /// </summary>
    public enum CubeFace
    {
        /// <summary>
        /// Top
        /// </summary>
        Ypositive, 
        /// <summary>
        /// Bottom
        /// </summary>
        Ynegative,
        Zpositive, Znegative,
        Xpositive, Xnegative,
        NUM
    }
    public enum MouseButton
    {
        Left,
        Right,
        Middle,
        X1,
        X2,
        NUM,

        DoubleClick,
        //Touch,
        //DoubleTouch,
    }

    public enum numBUTTON
    {
        UNNOWN = -1,
        A,
        B,
        X,
        Y,
        LB,
        RB,
        LT,
        RT,
        Start,
        Back,
        LS_Click,
        RS_Click,
        NUM
    };

    public enum ImageLayers
    { //Top to bottom
        AbsoluteTopLayer = 0,

        Top0_AbsFront,
        Top0_Front,
        Top0,
        Top0_Back,

        Top1_AbsFront,
        Top1_Front,
        Top1,
        Top1_Back,

        Top2_AbsFront,
        Top2_Front,
        Top2,
        Top2_Back,

        Top3_AbsFront,
        Top3_Front,
        Top3,
        Top3_Back,

        Top4_AbsFront,
        Top4_Front,
        Top4,
        Top4_Back,

        Top5_AbsFront,
        Top5_Front,
        Top5,
        Top5_Back,

        Top6_AbsFront,
        Top6_Front,
        Top6,
        Top6_Back,

        Top7_AbsFront,
        Top7_Front,
        Top7,
        Top7_Back,

        Top8_AbsFront,
        Top8_Front,
        Top8,
        Top8_Back,

        Top9_AbsFront,
        Top9_Front,
        Top9,
        Top9_Back,

        Foreground0_AbsFront,
        Foreground0_Front,
        Foreground0,
        Foreground0_Back,

        Foreground1_AbsFront,
        Foreground1_Front,
        Foreground1,
        Foreground1_Back,

        Foreground2_AbsFront,
        Foreground2_Front,
        Foreground2,
        Foreground2_Back,

        Foreground3_AbsFront,
        Foreground3_Front,
        Foreground3,
        Foreground3_Back,

        Foreground4_AbsFront,
        Foreground4_Front,
        Foreground4,
        Foreground4_Back,

        Foreground5_AbsFront,
        Foreground5_Front,
        Foreground5,
        Foreground5_Back,

        Foreground6_AbsFront,
        Foreground6_Front,
        Foreground6,
        Foreground6_Back,

        Foreground7_AbsFront,
        Foreground7_Front,
        Foreground7,
        Foreground7_Back,

        Foreground8_AbsFront,
        Foreground8_Front,
        Foreground8,
        Foreground8_Back,

        Foreground9_AbsFront,
        Foreground9_Front,
        Foreground9,
        Foreground9_Back,
        
        Lay0_AbsFront,
        Lay0_Front,
        Lay0,
        Lay0_Back,

        Lay1_AbsFront,
        Lay1_Front,
        Lay1,
        Lay1_Back,

        Lay2_AbsFront,
        Lay2_Front,
        Lay2,
        Lay2_Back,

        Lay3_AbsFront,
        Lay3_Front,
        Lay3,
        Lay3_Back,

        Lay4_AbsFront,
        Lay4_Front,
        Lay4,
        Lay4_Back,

        Lay5_AbsFront,
        Lay5_Front,
        Lay5,
        Lay5_Back,

        Lay6_AbsFront,
        Lay6_Front,
        Lay6,
        Lay6_Back,

        Lay7_AbsFront,
        Lay7_Front,
        Lay7,
        Lay7_Back,

        Lay8_AbsFront,
        Lay8_Front,
        Lay8,
        Lay8_Back,

        Lay9_AbsFront,
        Lay9_Front,
        Lay9,
        Lay9_Back,

        Background0_AbsFront,
        Background0_Front,
        Background0,
        Background0_Back,

        Background1_Abs_Front,
        Background1_Front,
        Background1,
        Background1_Back,

        Background2_Abs_Front,
        Background2_Front,
        Background2,
        Background2_Back,

        Background3_Abs_Front,
        Background3_Front,
        Background3,
        Background3_Back,

        Background4_Abs_Front,
        Background4_Front,
        Background4,
        Background4_Back,

        Background5_Abs_Front,
        Background5_Front,
        Background5,
        Background5_Back,

        Background6_Abs_Front,
        Background6_Front,
        Background6,
        Background6_Back,

        Background7_Abs_Front,
        Background7_Front,
        Background7,
        Background7_Back,

        Background8_Abs_Front,
        Background8_Front,
        Background8,
        Background8_Back,

        Background9_Abs_Front,
        Background9_Front,
        Background9,
        Background9_Back,

        Bottom0_Abs_Front,
        Bottom0_Front,
        Bottom0,
        Bottom0_Back,

        Bottom1_Abs_Front,
        Bottom1_Front,
        Bottom1,
        Bottom1_Back,

        Bottom2_Abs_Front,
        Bottom2_Front,
        Bottom2,
        Bottom2_Back,

        Bottom3_Abs_Front,
        Bottom3_Front,
        Bottom3,
        Bottom3_Back,

        Bottom4_Abs_Front,
        Bottom4_Front,
        Bottom4,
        Bottom4_Back,

        Bottom5_Abs_Front,
        Bottom5_Front,
        Bottom5,
        Bottom5_Back,

        Bottom6_Abs_Front,
        Bottom6_Front,
        Bottom6,
        Bottom6_Back,
   
        Bottom7_Abs_Front,
        Bottom7_Front,
        Bottom7,
        Bottom7_Back,

        Bottom8_Abs_Front,
        Bottom8_Front,
        Bottom8,
        Bottom8_Back,

        Bottom9_Abs_Front,
        Bottom9_Front,
        Bottom9,
        Bottom9_Back,

        AbsoluteBottomLayer,
        NUM
    };
   
    
    public enum ThumbStickType
    { 
        Left = 0,
        Right,
        D,
        NUM_NON,
    }
    public enum RenderLayer
    {
        Basic = 0,
        Layer2,
        Layer3,
        //Layer4,
        NUM
    };

    public enum DataType
    {
        Unknown = 0,
        Boolean,
        Integer,
        Float,
        Vector2,
        Vector3,
        Vector4,
        Quaterion,
        String,
    };
    public enum ScreenType
    {
        OldTV,
        OldTV_HoriSplit,
        LCDTVsmall,
        HD480,
        HD720,
        HD720_HoriSplit,
        HD900,
        HD1080,
        FullScreenWindow70Perc,
        FullScreenWindow100Perc,
        FullScreen,
        PhoneLow,
        PhoneHigh,
        ExtremeSmall,
        NUM
    };
    public enum Dimensions
    {
        X = 0,
        Y,
        Z,
        W,
        NUM,
        NON,
    }

    public enum MyColor
    {
        White,
        Black,
        Sky_blue,
        Dark_blue,
        Dark_Gray,
        NUM
        //Color.White, Color.Black, Color.CornflowerBlue, Color.DarkBlue, Color.DarkGray,
    }

    /// <summary>
    /// How much debug options the build should contain
    /// </summary>
    public enum BuildDebugLevel
    {
        Dev,
        DebugDemo,
        ShowDemo,
        Release,
        PublicDemo,

        NUM
    }

    public enum BuildTargetPlatform
    {
        XboxDebugOnWindows,
        Xbox,
        PCGame,
    }
    enum StartProgram
    {
        LootFest3,
        //Seedgenerator,
        //TemplateEditor,
        //TableTop,
        //Evolution,
        //GameCrash,
        //PlunderParty,
        PartyJousting,
        ToGG,
        //CCG,
        //Wars,
        DSS,

        Special,
    }

    enum MultiThreadType
    {
        Main,
        Asynch,
        Storage,
        UNKNOWN
    }

    enum FeatureStage
    {
        Planned_0,
        Added_1,
        Tested_2,
    }

    //enum MultiThreadType
    //{
    //    Main,
    //    Storage,
    //    Storage,
    //    Asynch,
    //}
}
