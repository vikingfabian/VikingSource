using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.LootFest.Map;
using VikingEngine.PJ.Bagatelle;

namespace VikingEngine.DSSWars.Data
{
    static class CityTag
    {
        public const SpriteName NoBackSprite = SpriteName.BluePrintSquareFull;

        public static SpriteName BackSprite(CityTagBack back)
        {
            switch (back)
            {
                case CityTagBack.White:
                    return SpriteName.warsFolder_white;
                case CityTagBack.Carton:
                    return SpriteName.warsFolder_carton;
                case CityTagBack.Yellow:
                    return SpriteName.warsFolder_yellow;
                case CityTagBack.Orange:
                    return SpriteName.warsFolder_orange;
                case CityTagBack.Pink:
                    return SpriteName.warsFolder_pink;
                case CityTagBack.Cyan:
                    return SpriteName.warsFolder_cyan;
                case CityTagBack.Blue:
                    return SpriteName.warsFolder_blue;
                case CityTagBack.Green:
                    return SpriteName.warsFolder_green;

                default: return NoBackSprite;
            }
        }

        public static SpriteName ArtSprite(CityTagArt art)
        {
            if (art == CityTagArt.None)
            {
                return SpriteName.NO_IMAGE;
            }
            else if (art <= CityTagArt.ItemResourceTypeMithrilArmor)
            {
                switch (art)
                {
                    case CityTagArt.ItemResourceTypeGoldOre: return SpriteName.WarsResource_GoldOre;
                    case CityTagArt.ItemResourceTypeGold: return SpriteName.rtsMoney;
                    case CityTagArt.ItemResourceTypeWater: return SpriteName.WarsResource_Water;
                    case CityTagArt.ItemResourceTypeWood: return SpriteName.WarsResource_Wood;
                    case CityTagArt.ItemResourceTypeFuel: return SpriteName.WarsResource_Fuel;
                    case CityTagArt.ItemResourceTypeStone: return SpriteName.WarsResource_Stone;
                    //case CityTagArt.ItemResourceTypeRaw_Coal: return SpriteName.GoodsCoal;
                    case CityTagArt.ItemResourceTypeRaw_Meat: return SpriteName.WarsResource_RawMeat;
                    case CityTagArt.ItemResourceTypeRaw_Wheat: return SpriteName.WarsResource_Wheat;
                    case CityTagArt.ItemResourceTypeRaw_Linen: return SpriteName.WarsResource_Linen;
                    case CityTagArt.ItemResourceTypeRaw_Hemp: return SpriteName.WarsResource_Hemp;
                    case CityTagArt.ItemResourceTypeRaw_Rapeseed: return SpriteName.WarsResource_Rapeseed;

                    case CityTagArt.ItemResourceTypeRawFood: return SpriteName.WarsResource_RawFood;
                    case CityTagArt.ItemResourceTypeFood: return SpriteName.WarsResource_Food;
                    case CityTagArt.ItemResourceTypeBeer: return SpriteName.WarsResource_Beer;
                    case CityTagArt.ItemResourceTypeSkinLinen: return SpriteName.WarsResource_LinenCloth;
                    case CityTagArt.ItemResourceTypeIronOre: return SpriteName.WarsResource_IronOre;
                    case CityTagArt.ItemResourceTypeIron: return SpriteName.WarsResource_Iron;

                    case CityTagArt.ItemResourceTypeSword: return SpriteName.WarsResource_Sword;
                    case CityTagArt.ItemResourceTypeSharpStick: return SpriteName.WarsResource_Sharpstick;
                    case CityTagArt.ItemResourceTypeTwoHandSword: return SpriteName.WarsResource_TwoHandSword;
                    case CityTagArt.ItemResourceTypeKnightsLance: return SpriteName.WarsResource_KnightsLance;
                    case CityTagArt.ItemResourceTypeBow: return SpriteName.WarsResource_Bow;
                    case CityTagArt.ItemResourceTypeLongBow: return SpriteName.WarsResource_Longbow;
                    case CityTagArt.ItemResourceTypeBallista: return SpriteName.WarsResource_Ballista;

                    case CityTagArt.ItemResourceTypePaddedArmor: return SpriteName.WarsResource_PaddedArmor;
                    case CityTagArt.ItemResourceTypeMailArmor: return SpriteName.WarsResource_IronArmor;
                    case CityTagArt.ItemResourceTypeFullPlateArmor: return SpriteName.WarsResource_FullPlateArmor;
                }
            }
            else if (art <= CityTagArt.BuildPavementFlower)
            {
                switch (art)
                {
                    case CityTagArt.BuildWorkerHuts: return SpriteName.WarsBuild_WorkerHuts;
                    case CityTagArt.BuildPostal: return SpriteName.WarsBuild_Postal;
                    case CityTagArt.BuildRecruitment: return SpriteName.WarsBuild_Recruitment;
                    case CityTagArt.BuildBarracks: return SpriteName.WarsBuild_Barracks;
                    case CityTagArt.BuildNobelhouse: return SpriteName.WarsBuild_Nobelhouse;
                    case CityTagArt.BuildTavern: return SpriteName.WarsBuild_Tavern;
                    case CityTagArt.BuildStorehouse: return SpriteName.WarsBuild_Storehouse;
                    case CityTagArt.BuildBrewery: return SpriteName.WarsBuild_Brewery;
                    case CityTagArt.BuildCook: return SpriteName.WarsBuild_Cook;
                    case CityTagArt.BuildCoalPit: return SpriteName.WarsBuild_CoalPit;
                    case CityTagArt.BuildWorkBench: return SpriteName.WarsBuild_WorkBench;
                    case CityTagArt.BuildSmith: return SpriteName.WarsBuild_Smith;
                    case CityTagArt.BuildCarpenter: return SpriteName.WarsBuild_Carpenter;
                    case CityTagArt.BuildWheatFarm: return SpriteName.WarsBuild_WheatFarms;
                    case CityTagArt.BuildLinenFarm: return SpriteName.WarsBuild_LinenFarms;
                    case CityTagArt.BuildHempFarm: return SpriteName.WarsBuild_HempFarms;
                    case CityTagArt.BuildRapeSeedFarm: return SpriteName.WarsBuild_RapeseedFarms;
                    case CityTagArt.BuildPigPen: return SpriteName.WarsBuild_PigPen;
                    case CityTagArt.BuildHenPen: return SpriteName.WarsBuild_HenPen;
                    case CityTagArt.BuildStatue_ThePlayer: return SpriteName.WarsBuild_Statue;
                    case CityTagArt.BuildPavement: return SpriteName.WarsBuild_Pavement;
                    case CityTagArt.BuildPavementFlower: return SpriteName.WarsBuild_PavementFlowers;
                }
            }
            else if (art <= CityTagArt.UnitType_Viking)
            {
                switch (art)
                {
                    case CityTagArt.Worker: return SpriteName.WarsWorker;
                    case CityTagArt.UnitType_SharpStick: return SpriteName.WarsUnitIcon_Folkman;
                    case CityTagArt.UnitType_Sword: return SpriteName.WarsUnitIcon_Soldier;
                    case CityTagArt.UnitType_LongSword: return SpriteName.WarsUnitIcon_Longsword;

                    case CityTagArt.UnitType_Warhammer: return SpriteName.WarsUnitIcon_Hammerknight;
                    case CityTagArt.UnitType_TwohandSword: return SpriteName.WarsUnitIcon_TwoHand;
                    case CityTagArt.UnitType_Knight: return SpriteName.WarsUnitIcon_Knight;
                    case CityTagArt.UnitType_MithrilKnight: return SpriteName.WarsUnitIcon_MithrilMan;
                    case CityTagArt.UnitType_MithrilArcher: return SpriteName.WarsUnitIcon_MithrilArcher;

                    case CityTagArt.UnitType_Slingshot: return SpriteName.WarsUnitIcon_Slingshot;
                    case CityTagArt.UnitType_Javelin: return SpriteName.WarsUnitIcon_Javelin;
                    case CityTagArt.UnitType_Bow: return SpriteName.WarsUnitIcon_Archer;
                    case CityTagArt.UnitType_Crossbow: return SpriteName.LittleUnitIconCrossBowman;

                    case CityTagArt.UnitType_Rifle: return SpriteName.WarsUnitIcon_BronzeRifle;
                    case CityTagArt.UnitType_Shotgun: return SpriteName.WarsResource_BronzeShotgun;

                    case CityTagArt.UnitType_Ballista: return SpriteName.WarsUnitIcon_Ballista;
                    case CityTagArt.UnitType_ManuBallista: return SpriteName.WarsUnitIcon_Manuballista;
                    case CityTagArt.UnitType_Catapult: return SpriteName.WarsUnitIcon_Catapult;

                    case CityTagArt.UnitType_SiegeBronzeCannon: return SpriteName.WarsUnitIcon_BronzeSiegeCannon;
                    case CityTagArt.UnitType_ManBronzeCannon: return SpriteName.WarsUnitIcon_BronzeManCannon;
                    case CityTagArt.UnitType_SiegeIronCannon: return SpriteName.WarsUnitIcon_IronSiegeCannon;
                    case CityTagArt.UnitType_ManIronCannon: return SpriteName.WarsUnitIcon_IronManCannon;

                    case CityTagArt.UnitType_HonourGuard: return SpriteName.WarsUnitIcon_Honorguard;
                    case CityTagArt.UnitType_Viking: return SpriteName.WarsUnitIcon_Viking;
                }
            }
            else if (art <= CityTagArt.IconHeart)
            {
                switch (art)
                {
                    case CityTagArt.IconBuild: return SpriteName.WarsHammer;
                    case CityTagArt.IconMovebox: return SpriteName.WarsWorkMove;
                    case CityTagArt.IconHandCollect: return SpriteName.WarsWorkCollect;
                    case CityTagArt.IconMine: return SpriteName.WarsWorkMine;
                    case CityTagArt.IconDig: return SpriteName.WarsWorkFarm;
                    case CityTagArt.IconBed: return SpriteName.WarsBedIcon;
                    case CityTagArt.IconMap: return SpriteName.WarsMapIcon;
                    case CityTagArt.IconFaction: return SpriteName.WarsGovernmentIcon;
                    case CityTagArt.IconThumbsUp: return SpriteName.unitEmoteThumbUp;
                    case CityTagArt.IconThumbsDown: return SpriteName.unitEmoteThumbDown;
                    case CityTagArt.IconHeart: return SpriteName.unitEmoteLove;
                }
            }
            else
            {
                switch (art)
                {
                    case CityTagArt.Num0: return SpriteName.pjNum0;
                    case CityTagArt.Num1: return SpriteName.pjNum1;
                    case CityTagArt.Num2: return SpriteName.pjNum2;
                    case CityTagArt.Num3: return SpriteName.pjNum3;
                    case CityTagArt.Num4: return SpriteName.pjNum4;
                    case CityTagArt.Num5: return SpriteName.pjNum5;
                    case CityTagArt.Num6: return SpriteName.pjNum6;
                    case CityTagArt.Num7: return SpriteName.pjNum7;
                    case CityTagArt.Num8: return SpriteName.pjNum8;
                    case CityTagArt.Num9: return SpriteName.pjNum9;
                    case CityTagArt.NumQuestion: return SpriteName.pjNumQuestion;
                    case CityTagArt.NumExpression: return SpriteName.pjNumExpression;
                    case CityTagArt.NumEqual: return SpriteName.pjNumEquals;
                    case CityTagArt.NumArrow: return SpriteName.pjNumArrowR;
                }
            }

            return SpriteName.NO_IMAGE;
        }

        public static SpriteName ArtSprite(ArmyTagArt art)
        {
            if (art == ArmyTagArt.None)
            {
                return SpriteName.NO_IMAGE;
            }
            else if (art <= ArmyTagArt.UnitType_HonourGuard)
            {
                switch (art)
                {
                    case ArmyTagArt.UnitType_SharpStick: return SpriteName.WarsUnitIcon_Folkman;
                    case ArmyTagArt.UnitType_Sword: return SpriteName.WarsUnitIcon_Soldier;
                    case ArmyTagArt.UnitType_LongSword: return SpriteName.WarsUnitIcon_Longsword;

                    case ArmyTagArt.UnitType_Warhammer: return SpriteName.WarsUnitIcon_Hammerknight;
                    case ArmyTagArt.UnitType_TwohandSword: return SpriteName.WarsUnitIcon_TwoHand;
                    case ArmyTagArt.UnitType_Knight: return SpriteName.WarsUnitIcon_Knight;
                    case ArmyTagArt.UnitType_MithrilKnight: return SpriteName.WarsUnitIcon_MithrilMan;
                    case ArmyTagArt.UnitType_MithrilArcher: return SpriteName.WarsUnitIcon_MithrilArcher;

                    case ArmyTagArt.UnitType_Slingshot: return SpriteName.WarsUnitIcon_Slingshot;
                    case ArmyTagArt.UnitType_Javelin: return SpriteName.WarsUnitIcon_Javelin;
                    case ArmyTagArt.UnitType_Bow: return SpriteName.WarsUnitIcon_Archer;
                    case ArmyTagArt.UnitType_Crossbow: return SpriteName.LittleUnitIconCrossBowman;

                    case ArmyTagArt.UnitType_Rifle: return SpriteName.WarsUnitIcon_BronzeRifle;
                    case ArmyTagArt.UnitType_Shotgun: return SpriteName.WarsResource_BronzeShotgun;

                    case ArmyTagArt.UnitType_Ballista: return SpriteName.WarsUnitIcon_Ballista;
                    case ArmyTagArt.UnitType_ManuBallista: return SpriteName.WarsUnitIcon_Manuballista;
                    case ArmyTagArt.UnitType_Catapult: return SpriteName.WarsUnitIcon_Catapult;

                    case ArmyTagArt.UnitType_SiegeBronzeCannon: return SpriteName.WarsUnitIcon_BronzeSiegeCannon;
                    case ArmyTagArt.UnitType_ManBronzeCannon: return SpriteName.WarsUnitIcon_BronzeManCannon;
                    case ArmyTagArt.UnitType_SiegeIronCannon: return SpriteName.WarsUnitIcon_IronSiegeCannon;
                    case ArmyTagArt.UnitType_ManIronCannon: return SpriteName.WarsUnitIcon_IronManCannon;

                    case ArmyTagArt.UnitType_HonourGuard: return SpriteName.WarsUnitIcon_Honorguard;
                }
            }
            else if (art <= ArmyTagArt.ItemResourceTypeMithrilArmor)
            {
                switch (art)
                {
                    case ArmyTagArt.ItemResourceTypeSword: return SpriteName.WarsResource_Sword;
                    case ArmyTagArt.ItemResourceTypeSharpStick: return SpriteName.WarsResource_Sharpstick;
                    case ArmyTagArt.ItemResourceTypeTwoHandSword: return SpriteName.WarsResource_TwoHandSword;
                    case ArmyTagArt.ItemResourceTypeKnightsLance: return SpriteName.WarsResource_KnightsLance;
                    case ArmyTagArt.ItemResourceTypeBow: return SpriteName.WarsResource_Bow;
                    case ArmyTagArt.ItemResourceTypeLongBow: return SpriteName.WarsResource_Longbow;
                    case ArmyTagArt.ItemResourceTypeBallista: return SpriteName.WarsResource_Ballista;

                    case ArmyTagArt.ItemResourceTypePaddedArmor: return SpriteName.WarsResource_PaddedArmor;
                    case ArmyTagArt.ItemResourceTypeMail: return SpriteName.WarsResource_IronArmor;
                    case ArmyTagArt.ItemResourceTypeFullPlate: return SpriteName.WarsResource_FullPlateArmor;
                }
            }
            else if (art <= ArmyTagArt.Specialize_Tradition)
            {
                switch (art)
                {
                    case ArmyTagArt.Specialize_Field:
                        return SpriteName.WarsSpecializeField;
                    case ArmyTagArt.Specialize_Sea:
                        return SpriteName.WarsSpecializeSea;
                    case ArmyTagArt.Specialize_Siege:
                        return SpriteName.WarsSpecializeSiege;
                    case ArmyTagArt.Specialize_Tradition:
                        return SpriteName.WarsSpecializeTradition;

                }
            }
            else if (art <= ArmyTagArt.LevelLegend)
            {
                switch (art)
                {
                    case ArmyTagArt.Specialize_Field:
                        return SpriteName.WarsSpecializeField;
                    case ArmyTagArt.Specialize_Sea:
                        return SpriteName.WarsSpecializeSea;
                    case ArmyTagArt.Specialize_Siege:
                        return SpriteName.WarsSpecializeSiege;
                    case ArmyTagArt.Specialize_Tradition:
                        return SpriteName.WarsSpecializeTradition;
                    case ArmyTagArt.LevelMinimal:
                        return SpriteName.WarsUnitLevelMinimal;
                    case ArmyTagArt.LevelBasic:
                        return SpriteName.WarsUnitLevelBasic;
                    case ArmyTagArt.LevelSkillful:
                        return SpriteName.WarsUnitLevelSkillful;
                    case ArmyTagArt.LevelProfessional:
                        return SpriteName.WarsUnitLevelProfessional;
                    case ArmyTagArt.LevelMaster:
                        return SpriteName.WarsUnitLevelMaster;
                    case ArmyTagArt.LevelLegend:
                        return SpriteName.WarsUnitLevelLegend;
                }
            }
            else if (art <= ArmyTagArt.icon_BrokenShield)
            {
                switch (art)
                {
                    case ArmyTagArt.icon_Lightning:
                        return SpriteName.warsArmyTag_Lightning;
                    case ArmyTagArt.icon_Fire:
                        return SpriteName.warsArmyTag_Fire;
                    case ArmyTagArt.icon_Hit:
                        return SpriteName.warsArmyTag_Hit;
                    case ArmyTagArt.icon_HitExpress:
                        return SpriteName.warsArmyTag_HitExpress;
                    case ArmyTagArt.icon_Retreat:
                        return SpriteName.warsArmyTag_Retreat;
                    case ArmyTagArt.icon_Return:
                        return SpriteName.warsArmyTag_Return;
                    case ArmyTagArt.icon_Anchor:
                        return SpriteName.warsArmyTag_Anchor;
                    case ArmyTagArt.icon_Shield:
                        return SpriteName.warsArmyTag_Shield;
                    case ArmyTagArt.icon_GoldShield:
                        return SpriteName.warsArmyTag_GoldShield;
                    case ArmyTagArt.icon_BrokenShield:
                        return SpriteName.warsArmyTag_BrokenShield;
                    case ArmyTagArt.icon_RoundShield:
                        return SpriteName.warsArmyTag_RoundShield;
                }
            }
            else if (art <= ArmyTagArt.WarsRelationTotalWar)
            {
                switch (art)
                {
                    case ArmyTagArt.WarsRelationAlly:
                        return SpriteName.WarsRelationAlly;
                    case ArmyTagArt.WarsRelationGood:
                        return SpriteName.WarsRelationGood;
                    case ArmyTagArt.WarsRelationPeace:
                        return SpriteName.WarsRelationPeace;
                    case ArmyTagArt.WarsRelationNeutral:
                        return SpriteName.WarsRelationNeutral;
                    case ArmyTagArt.WarsRelationTruce:
                        return SpriteName.WarsRelationTruce;
                    case ArmyTagArt.WarsRelationWar:
                        return SpriteName.WarsRelationWar;
                    case ArmyTagArt.WarsRelationTotalWar:
                        return SpriteName.WarsRelationTotalWar;
                }
            }
            else
            {
                switch (art)
                {
                    case ArmyTagArt.Num0: return SpriteName.pjNum0;
                    case ArmyTagArt.Num1: return SpriteName.pjNum1;
                    case ArmyTagArt.Num2: return SpriteName.pjNum2;
                    case ArmyTagArt.Num3: return SpriteName.pjNum3;
                    case ArmyTagArt.Num4: return SpriteName.pjNum4;
                    case ArmyTagArt.Num5: return SpriteName.pjNum5;
                    case ArmyTagArt.Num6: return SpriteName.pjNum6;
                    case ArmyTagArt.Num7: return SpriteName.pjNum7;
                    case ArmyTagArt.Num8: return SpriteName.pjNum8;
                    case ArmyTagArt.Num9: return SpriteName.pjNum9;
                    case ArmyTagArt.NumQuestion: return SpriteName.pjNumQuestion;
                    case ArmyTagArt.NumExpression: return SpriteName.pjNumExpression;
                    case ArmyTagArt.NumEqual: return SpriteName.pjNumEquals;
                    case ArmyTagArt.NumArrow: return SpriteName.pjNumArrowR;
                }
            }
            return SpriteName.NO_IMAGE;
        }
    }

    enum CityTagBack
    {
        NONE,
        White,
        Carton,
        Yellow,
        Orange,
        Pink,
        Cyan,
        Blue,
        Green,
        NUM,
    }

    enum ArmyTagArt
    {
        None = 0,
        UnitType_SharpStick,
        UnitType_Sword,
        UnitType_LongSword,

        UnitType_Warhammer,
        UnitType_TwohandSword,
        UnitType_Knight,
        UnitType_MithrilKnight,
        UnitType_MithrilArcher,
        UnitType_HonourGuard,

        UnitType_Slingshot,
        UnitType_Javelin,
        UnitType_Bow,
        UnitType_Crossbow,

        UnitType_Rifle,
        UnitType_Shotgun,

        UnitType_Ballista,
        UnitType_ManuBallista,
        UnitType_Catapult,

        UnitType_SiegeBronzeCannon,
        UnitType_ManBronzeCannon,
        UnitType_SiegeIronCannon,
        UnitType_ManIronCannon,


        ItemResourceTypeSharpStick,
        ItemResourceTypeBronzeSword,
        ItemResourceTypeShortSword,
        ItemResourceTypeSword,
        ItemResourceTypeLongSword,

        ItemResourceTypeWarHammer,
        ItemResourceTypeTwoHandSword,
        ItemResourceTypeKnightsLance,
        ItemResourceTypeMithrilSword,
        ItemResourceTypeMithrilBow,

        ItemResourceTypeSlingshot,
        ItemResourceTypeThrowingspear,
        ItemResourceTypeBow,
        ItemResourceTypeLongBow,
        ItemResourceTypeCrossbow,

        ItemResourceTypeHandCannon,
        ItemResourceTypeHandCulverin,
        ItemResourceTypeRifle,
        ItemResourceTypeBlunderbus,

        ItemResourceTypeBallista,
        ItemResourceTypeMenuBallista,
        ItemResourceTypeCatapult,
        ItemResourceTypeSiegeCannonBronze,
        ItemResourceTypeManCannonBronze,
        ItemResourceTypeSiegeCannonIron,
        ItemResourceTypeManCannonIron,

        ItemResourceTypePaddedArmor,
        ItemResourceTypeHeavyPaddedArmor,
        ItemResourceTypeBronzeArmor,
        ItemResourceTypeMail,
        ItemResourceTypeHeavyMail,
        ItemResourceTypePlate,
        ItemResourceTypeFullPlate,
        ItemResourceTypeMithrilArmor,

        Specialize_Field,
        Specialize_Sea,
        Specialize_Siege,
        Specialize_Tradition,

        LevelMinimal,
        LevelBasic,
        LevelSkillful,
        LevelProfessional,
        LevelMaster,
        LevelLegend,

        icon_Lightning,
        icon_Fire,        
        icon_Hit,
        icon_HitExpress,
        icon_Retreat,
        icon_Return,
        icon_Anchor,
        icon_Shield,
        icon_GoldShield,
        icon_RoundShield,
        icon_BrokenShield,

        WarsRelationAlly,
        WarsRelationGood,
        WarsRelationPeace,
        WarsRelationNeutral,
        WarsRelationTruce,
        WarsRelationWar,
        WarsRelationTotalWar,

        Num0,
        Num1,
        Num2,
        Num3,
        Num4,
        Num5,
        Num6,
        Num7,
        Num8,
        Num9,
        NumQuestion,
        NumExpression,
        NumEqual,
        NumArrow,

        NUM,
    }

    enum CityTagArt
    {
        None = 0,


        ItemResourceTypeGoldOre,
        ItemResourceTypeGold,
        ItemResourceTypeWater,

        ItemResourceTypeWood,
        ItemResourceTypeStone,
        ItemResourceTypeRaw_Meat,
        ItemResourceTypeRaw_Wheat,
        ItemResourceTypeRaw_Linen,
        ItemResourceTypeRaw_Hemp,
        ItemResourceTypeRaw_Rapeseed,
        ItemResourceTypeRawFood,
        ItemResourceTypeFuel,
        ItemResourceTypeSkinLinen,
        ItemResourceTypeFood,
        ItemResourceTypeBeer,
        ItemResourceTypeCoolingFluid,

        ItemResourceTypeToolkit,
        ItemResourceTypeWagon2Wheel,
        ItemResourceTypeWagon4Wheel,
        ItemResourceTypeBlackPowder,
        ItemResourceTypeGunPowder,
        ItemResourceTypeLedBullet,
                
        ItemResourceTypeIronOre,
        ItemResourceTypeTinOre,
        ItemResourceTypeCupperOre,
        ItemResourceTypeLeadOre,
        ItemResourceTypeSilverOre,

        ItemResourceTypeIron,
        ItemResourceTypeTin,
        ItemResourceTypeCupper,
        ItemResourceTypeLead,
        ItemResourceTypeSilver,
        ItemResourceTypeRawMithril,

        ItemResourceTypeBronze,
        ItemResourceTypeCastIron,
        ItemResourceTypeBloomeryIron,
        ItemResourceTypeSteel,        
        ItemResourceTypeMithril,

        ItemResourceTypeSharpStick,
        ItemResourceTypeBronzeSword,
        ItemResourceTypeShortSword,
        ItemResourceTypeSword,
        ItemResourceTypeLongSword,

        ItemResourceTypeWarHammer,
        ItemResourceTypeTwoHandSword,
        ItemResourceTypeKnightsLance,
        ItemResourceTypeMithrilSword,
        ItemResourceTypeMithrilBow,

        ItemResourceTypeSlingshot,
        ItemResourceTypeThrowingSpear,
        ItemResourceTypeBow,
        ItemResourceTypeLongBow,
        ItemResourceTypeCrossbow,
        ItemResourceTypeHandCannon,
        ItemResourceTypeHandCulverin,
        ItemResourceTypeRifle,
        ItemResourceTypeBlunderbus,
        ItemResourceTypeBallista,
        ItemResourceTypeManuBallista,
        ItemResourceTypeCatapult,
        ItemResourceTypeSiegeCannonBronze,
        ItemResourceTypeManCannonBronze,
        ItemResourceTypeSiegeCannonIron,
        ItemResourceTypeManCannonIron,

        ItemResourceTypePaddedArmor,
        ItemResourceTypeHeavyPaddedArmor,
        ItemResourceTypeBronzeArmor,
        ItemResourceTypeMailArmor,
        ItemResourceTypeHeavyMailArmor,
        ItemResourceTypeLightPlateArmor,
        ItemResourceTypeFullPlateArmor,

        ItemResourceTypeMithrilArmor,
        //ItemResourceTypeGoldOre,
        //ItemResourceTypeGold,
        //ItemResourceTypeWater,
        //ItemResourceTypeWood,
        //ItemResourceTypeFuel,
        //ItemResourceTypeStone,
        //ItemResourceTypeRaw_Coal,
        //ItemResourceTypeRaw_Meat,
        //ItemResourceTypeRaw_Wheat,

        //ItemResourceTypeRaw_Linen,
        //ItemResourceTypeRaw_Hemp,
        //ItemResourceTypeRaw_Rapeseed,

        //ItemResourceTypeRawFood,
        //ItemResourceTypeFood,
        //ItemResourceTypeBeer,
        //ItemResourceTypeSkinLinen,
        //ItemResourceTypeIronOre,
        //ItemResourceTypeIron,

        //ItemResourceTypeSharpStick,
        //ItemResourceTypeBronzeSword,
        //ItemResourceTypeShortSword,
        //ItemResourceTypeSword,
        //ItemResourceTypeLongSword,

        //ItemResourceTypeWarHammer,
        //ItemResourceTypeTwoHandSword,
        //ItemResourceTypeKnightsLance,
        //ItemResourceTypeMithrilSword,
        //ItemResourceTypeMithrilBow,

        //ItemResourceTypeSlingshot,
        //ItemResourceTypeThrowingspear,
        //ItemResourceTypeBow,
        //ItemResourceTypeLongBow,
        //ItemResourceTypeCrossbow,

        //ItemResourceTypeHandCannon,
        //ItemResourceTypeHandCulverin,
        //ItemResourceTypeRifle,
        //ItemResourceTypeBlunderbus,

        //ItemResourceTypeBallista,
        //ItemResourceTypeMenuBallista,
        //ItemResourceTypeCatapult,
        //ItemResourceTypeSiegeCannonBronze,
        //ItemResourceTypeManCannonBronze,
        //ItemResourceTypeSiegeCannonIron,
        //ItemResourceTypeManCannonIron,

        //ItemResourceTypePaddedArmor,
        //ItemResourceTypeHeavyPaddedArmor,
        //ItemResourceTypeMail,
        //ItemResourceTypeHeavyMail,
        //ItemResourceTypePlate,
        //ItemResourceTypeFullPlate,


        BuildWorkerHuts,
        BuildPostal,
        BuildRecruitment,
        BuildBarracks,
        BuildNobelhouse,
        BuildTavern,
        BuildStorehouse,
        BuildBrewery,
        BuildCook,
        BuildCoalPit,
        BuildWorkBench,
        BuildSmith,
        BuildCarpenter,
        BuildWheatFarm,
        BuildLinenFarm,
        BuildHempFarm,
        BuildRapeSeedFarm,
        BuildPigPen,
        BuildHenPen,
        BuildStatue_ThePlayer,
        BuildPavement,
        BuildPavementFlower,

        Worker,
       
        
        
        UnitType_SharpStick,
        UnitType_Sword,
        UnitType_LongSword,

        UnitType_Warhammer,
        UnitType_TwohandSword,
        UnitType_Knight,
        UnitType_MithrilKnight,
        UnitType_MithrilArcher,
        UnitType_HonourGuard,

        UnitType_Slingshot,
        UnitType_Javelin,
        UnitType_Bow,
        UnitType_Crossbow,

        UnitType_Rifle,
        UnitType_Shotgun,

        UnitType_Ballista,
        UnitType_ManuBallista,
        UnitType_Catapult,

        UnitType_SiegeBronzeCannon,
        UnitType_ManBronzeCannon,
        UnitType_SiegeIronCannon,
        UnitType_ManIronCannon,

        UnitType_Viking,

        IconBuild,
        IconMovebox,
        IconHandCollect,
        IconMine,
        IconDig,
        //IconState,
        IconBed,
        IconMap,
        IconFaction,
        IconThumbsUp,
        IconThumbsDown,
        IconHeart,


        Num0,
        Num1,
        Num2,
        Num3,
        Num4,
        Num5,
        Num6,
        Num7,
        Num8,
        Num9,
        NumQuestion,
        NumExpression,
        NumEqual,
        NumArrow,

        NUM    
    }
}
