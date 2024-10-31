﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Data
{
    static class CityTag
    {
        //public static readonly Dictionary<int, SpriteName> TagArtOptions = new Dictionary<int, SpriteName>
        //{
        //    { 0, SpriteName.NO_IMAGE },
        //    { CityTagArt.ItemResourceTypeGold, SpriteName.NO_IMAGE },
        //    { 2, SpriteName.NO_IMAGE },
        //    { 3, SpriteName.NO_IMAGE },
        //    { 4, SpriteName.NO_IMAGE },
        //    { 5, SpriteName.NO_IMAGE },
        //    { 6, SpriteName.NO_IMAGE },

        //};

        public static SpriteName ArtSprite(CityTagArt art)
        {
            if (art == CityTagArt.None)
            {
                return SpriteName.NO_IMAGE;
            }
            else if (art <= CityTagArt.ItemResourceTypeHeavyArmor)
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

                    case CityTagArt.ItemResourceTypeLightArmor: return SpriteName.WarsResource_LightArmor;
                    case CityTagArt.ItemResourceTypeMediumArmor: return SpriteName.WarsResource_MediumArmor;
                    case CityTagArt.ItemResourceTypeHeavyArmor: return SpriteName.WarsResource_HeavyArmor;
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
            else if (art <= CityTagArt.SoldierUnitViking)
            {
                switch (art)
                {
                    case CityTagArt.Worker: return SpriteName.WarsWorker;
                    case CityTagArt.SoldierUnitSharpStick: return SpriteName.WarsUnitIcon_Folkman;
                    case CityTagArt.SoldierUnitSword: return SpriteName.WarsUnitIcon_Soldier;
                    case CityTagArt.SoldierUnitTwohandSword: return SpriteName.WarsUnitIcon_TwoHand;
                    case CityTagArt.SoldierUnitKnight: return SpriteName.WarsUnitIcon_Knight;
                    case CityTagArt.SoldierUnitBow: return SpriteName.WarsUnitIcon_Archer;
                    case CityTagArt.SoldierUnitBallista: return SpriteName.WarsUnitIcon_Ballista;
                    case CityTagArt.SoldierUnitHonourGuard: return SpriteName.WarsUnitIcon_Honorguard;
                    case CityTagArt.SoldierUnitViking: return SpriteName.WarsUnitIcon_Viking;
                }
            }
            else if (art <= CityTagArt.IconHeart)
            {
                switch (art)
                {
                    case CityTagArt.IconBuild: return SpriteName.NO_IMAGE;
                    case CityTagArt.IconMovebox: return SpriteName.NO_IMAGE;
                    case CityTagArt.IconHandCollect: return SpriteName.NO_IMAGE;
                    case CityTagArt.IconMine: return SpriteName.NO_IMAGE;
                    case CityTagArt.IconDig: return SpriteName.NO_IMAGE;
                    case CityTagArt.IconState: return SpriteName.NO_IMAGE;
                    case CityTagArt.IconBed: return SpriteName.NO_IMAGE;
                    case CityTagArt.IconMap: return SpriteName.NO_IMAGE;
                    case CityTagArt.IconFaction: return SpriteName.NO_IMAGE;
                    case CityTagArt.IconThumbsUp: return SpriteName.NO_IMAGE;
                    case CityTagArt.IconThumbsDown: return SpriteName.NO_IMAGE;
                    case CityTagArt.IconHeart: return SpriteName.NO_IMAGE;
                }
            }
            else
            {
                switch (art)
                {
                    case CityTagArt.Num0: return SpriteName.NO_IMAGE;
                    case CityTagArt.Num1: return SpriteName.NO_IMAGE;
                    case CityTagArt.Num2: return SpriteName.NO_IMAGE;
                    case CityTagArt.Num3: return SpriteName.NO_IMAGE;
                    case CityTagArt.Num4: return SpriteName.NO_IMAGE;
                    case CityTagArt.Num5: return SpriteName.NO_IMAGE;
                    case CityTagArt.Num6: return SpriteName.NO_IMAGE;
                    case CityTagArt.Num7: return SpriteName.NO_IMAGE;
                    case CityTagArt.Num8: return SpriteName.NO_IMAGE;
                    case CityTagArt.Num9: return SpriteName.NO_IMAGE;
                    case CityTagArt.NumQuestion: return SpriteName.NO_IMAGE;
                    case CityTagArt.NumExpression: return SpriteName.NO_IMAGE;
                    case CityTagArt.NumEqual: return SpriteName.NO_IMAGE;
                    case CityTagArt.NumArrow: return SpriteName.NO_IMAGE;
                }
            }

            return SpriteName.NO_IMAGE;
        }
    }

    enum CityTagBack
    { 
        White,
        Carton,

    }
    //add(SpriteName.warsFolder_carton);
    //add(SpriteName.warsFolder_white);
    //add(SpriteName.warsFolder_orange);
    //add(SpriteName.warsFolder_yellow);
    //add(SpriteName.warsFolder_green);
    //add(SpriteName.warsFolder_pink);
    //add(SpriteName.warsFolder_blue);
    //add(SpriteName.warsFolder_cyan);

    enum CityTagArt
    {
        None = 0,
        ItemResourceTypeGoldOre,
        ItemResourceTypeGold,
        ItemResourceTypeWater,
        ItemResourceTypeWood,
        ItemResourceTypeFuel,
        ItemResourceTypeStone,
        //ItemResourceTypeRaw_Coal,
        ItemResourceTypeRaw_Meat,
        ItemResourceTypeRaw_Wheat,

        ItemResourceTypeRaw_Linen,
        ItemResourceTypeRaw_Hemp,
        ItemResourceTypeRaw_Rapeseed,

        ItemResourceTypeRawFood,
        ItemResourceTypeFood,
        ItemResourceTypeBeer,
        ItemResourceTypeSkinLinen,
        ItemResourceTypeIronOre,
        ItemResourceTypeIron,

        ItemResourceTypeSword,
        ItemResourceTypeSharpStick,
        ItemResourceTypeTwoHandSword,
        ItemResourceTypeKnightsLance,
        ItemResourceTypeBow,
        ItemResourceTypeLongBow,
        ItemResourceTypeBallista,

        ItemResourceTypeLightArmor,
        ItemResourceTypeMediumArmor,
        ItemResourceTypeHeavyArmor,



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
        SoldierUnitSharpStick,
        SoldierUnitSword,
        SoldierUnitTwohandSword,
        SoldierUnitKnight,
        SoldierUnitBow,
        SoldierUnitBallista,
        SoldierUnitHonourGuard,
        SoldierUnitViking,

        IconBuild,
        IconMovebox,
        IconHandCollect,
        IconMine,
        IconDig,
        IconState,
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
    
    }
}
