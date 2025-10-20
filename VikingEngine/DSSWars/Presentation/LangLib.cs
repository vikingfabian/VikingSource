using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.DSSWars.XP;
using VikingEngine.Graphics;
using VikingEngine.LootFest.GO;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Presentation
{
    static class LangLib
    {       
        public static void GameModeText(GameModeMainType mode, out string caption, out string desc)
        {
            caption = null;
            desc = null;
            switch (mode)
            {
                case GameModeMainType.FullStory:
                    caption = DssRef.lang.Settings_Mode_Story;
                    desc = DssRef.lang.Settings_Mode_IncludeBoss + " " + DssRef.lang.Settings_Mode_IncludeAttacks;
                    break;
                case GameModeMainType.QuickMatch:
                    caption = DssRef.todoLang.GameMode_QuickMatch;
                    desc = DssRef.todoLang.GameMode_QuickMatch_Description;
                    break;
                case GameModeMainType.Sandbox:
                    caption = DssRef.lang.Settings_Mode_Sandbox;
                    desc = DssRef.lang.Settings_Mode_IncludeAttacks;
                    break;
                case GameModeMainType.Peaceful:
                    caption = DssRef.lang.Settings_Mode_Peaceful;
                    desc = DssRef.lang.Settings_Mode_Peaceful_Description + " " + Ref.langOpt.Settings_Mode_No_Achivements;
                    break;
                case GameModeMainType.Spectator:
                    caption = DssRef.lang.Settings_Mode_Spectator;
                    desc = DssRef.lang.Settings_Mode_Spectator_Description + " " + Ref.langOpt.Settings_Mode_No_Achivements;
                    break;
            }
        }
        public static void WorkNameIcon(WorkPriorityType type, out string name, out SpriteName workIcon, out SpriteName typeIcon)
        {
            switch (type)
            {
                case WorkPriorityType.move:
                    name = DssRef.lang.Work_Move;
                    workIcon = SpriteName.WarsWorkMove;
                    typeIcon = SpriteName.WarsBuild_Storehouse;
                    break;

                case WorkPriorityType.wood:
                    name = string.Format(DssRef.lang.Work_GatherXResource, DssRef.lang.Resource_TypeName_Wood);
                    workIcon = SpriteName.WarsWorkCollect;
                    typeIcon = SpriteName.WarsResource_Wood;
                    break;

                case WorkPriorityType.stone:
                    name = string.Format(DssRef.lang.Work_GatherXResource, DssRef.lang.Resource_TypeName_Stone);
                    workIcon = SpriteName.WarsWorkCollect;
                    typeIcon = SpriteName.WarsResource_Stone;
                    break;

                case WorkPriorityType.craftFuel:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Fuel);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Fuel;
                    break;

                case WorkPriorityType.craftFood:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Food);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Food;
                    break;

                case WorkPriorityType.craftBeer:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Beer);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Beer;
                    break;

                case WorkPriorityType.craftCoolingFluid:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_CoolingFluid);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_CoolingFluid;
                    break;

                case WorkPriorityType.craftToolkit:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Toolkit);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Toolkit;
                    break;

                case WorkPriorityType.craftWagonLight:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Wagon2Wheel);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Wagon2Wheel;
                    break;

                case WorkPriorityType.craftWagonHeavy:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Wagon4Wheel);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Wagon4Wheel;
                    break;

                case WorkPriorityType.craftBlackPowder:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_BlackPowder);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_BlackPowder;
                    break;

                case WorkPriorityType.craftGunPowder:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_GunPowder);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_GunPowder;
                    break;

                case WorkPriorityType.craftBullet:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_LedBullet);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Bullets;
                    break;

                case WorkPriorityType.smeltIron:
                    name = string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Iron);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Iron;
                    break;

                case WorkPriorityType.smeltTin:
                    name = string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Tin);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Tin;
                    break;

                case WorkPriorityType.smeltCopper:
                    name = string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Copper);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Copper;
                    break;

                case WorkPriorityType.smeltLead:
                    name = string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Lead);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Lead;
                    break;

                case WorkPriorityType.smeltSilver:
                    name = string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Silver);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Silver;
                    break;

                case WorkPriorityType.craftBronze:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Bronze);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Bronze;
                    break;

                case WorkPriorityType.craftCastIron:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_CastIron);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_CastIron;
                    break;

                case WorkPriorityType.craftBloomeryIron:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_BloomIron);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_BloomeryIron;
                    break;

                case WorkPriorityType.craftSteel:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Steel);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Steel;
                    break;

                case WorkPriorityType.craftMithril:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Mithril);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_MithrilAlloy;
                    break;

                case WorkPriorityType.farmfood:
                    name = DssRef.lang.Work_Farming + ": " + DssRef.lang.Resource_TypeName_Food;
                    workIcon = SpriteName.WarsWorkFarm;
                    typeIcon = SpriteName.WarsResource_RawFood;
                    break;

                case WorkPriorityType.farmfuel:
                    name = DssRef.lang.Work_Farming + ": " + DssRef.lang.Resource_TypeName_Fuel;
                    workIcon = SpriteName.WarsWorkFarm;
                    typeIcon = SpriteName.WarsResource_Fuel;
                    break;

                case WorkPriorityType.farmlinen:
                    name = DssRef.lang.Work_Farming + ": " + DssRef.lang.Resource_TypeName_Linen;
                    workIcon = SpriteName.WarsWorkFarm;
                    typeIcon = SpriteName.WarsResource_LinenCloth;
                    break;

                case WorkPriorityType.bogiron:
                    name = DssRef.lang.Resource_TypeName_BogIron;
                    workIcon = SpriteName.WarsWorkCollect;
                    typeIcon = SpriteName.WarsResource_IronOre;
                    break;

                case WorkPriorityType.miningIron:
                    name = string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Iron);
                    workIcon = SpriteName.WarsWorkMine;
                    typeIcon = SpriteName.WarsResource_Iron;
                    break;

                case WorkPriorityType.miningTin:
                    name = string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Tin);
                    workIcon = SpriteName.WarsWorkMine;
                    typeIcon = SpriteName.WarsResource_Tin;
                    break;

                case WorkPriorityType.miningCopper:
                    name = string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Copper);
                    workIcon = SpriteName.WarsWorkMine;
                    typeIcon = SpriteName.WarsResource_Copper;
                    break;

                case WorkPriorityType.miningLead:
                    name = string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Lead);
                    workIcon = SpriteName.WarsWorkMine;
                    typeIcon = SpriteName.WarsResource_Lead;
                    break;

                case WorkPriorityType.miningSilver:
                    name = string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Silver);
                    workIcon = SpriteName.WarsWorkMine;
                    typeIcon = SpriteName.WarsResource_Silver;
                    break;

                case WorkPriorityType.miningGold:
                    name = string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.ResourceType_Gold);
                    workIcon = SpriteName.WarsWorkMine;
                    typeIcon = SpriteName.WarsResource_Gold;
                    break;
                case WorkPriorityType.miningSulfur:
                    name = string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Sulfur);
                    workIcon = SpriteName.WarsWorkMine;
                    typeIcon = SpriteName.WarsResource_Sulfur;
                    break;
                case WorkPriorityType.miningMithril:
                    name = string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_RawMithril);
                    workIcon = SpriteName.WarsWorkMine;
                    typeIcon = SpriteName.WarsResource_Mithril;
                    break;

                case WorkPriorityType.craftSharpStick:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_SharpStick);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Sharpstick;
                    break;

                case WorkPriorityType.craftBronzeSword:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_BronzeSword);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_BronzeSword;
                    break;

                case WorkPriorityType.craftShortSword:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_ShortSword);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_ShortSword;
                    break;

                case WorkPriorityType.craftSword:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Sword);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Sword;
                    break;

                case WorkPriorityType.craftLongSword:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_LongSword);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Longsword;
                    break;

                case WorkPriorityType.craftHandSpear:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HandSpear);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_HandSpear;
                    break;

                case WorkPriorityType.craftWarhammer:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Warhammer);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Warhammer;
                    break;

                case WorkPriorityType.craftTwoHandSword:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_TwoHandSword);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_TwoHandSword;
                    break;

                case WorkPriorityType.craftKnightsLance:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_KnightsLance);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_KnightsLance;
                    break;

                case WorkPriorityType.craftMithrilSword:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_MithrilSword);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_MithrilSword;
                    break;

                case WorkPriorityType.craftSlingshot:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_SlingShot);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Slingshot;
                    break;

                case WorkPriorityType.craftThrowingspear:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_ThrowingSpear);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_ThrowSpear;
                    break;

                case WorkPriorityType.craftBow:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Bow);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Bow;
                    break;

                case WorkPriorityType.craftLongbow:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Longbow);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Longbow;
                    break;

                case WorkPriorityType.craftCrossbow:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Crossbow);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Crossbow;
                    break;

                case WorkPriorityType.craftMithrilbow:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_MithrilBow);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Mithrilbow;
                    break;

                case WorkPriorityType.craftHandCannon:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HandCannon);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_BronzeRifle;
                    break;

                case WorkPriorityType.craftHandCulverin:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HandCulverin);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_BronzeShotgun;
                    break;

                case WorkPriorityType.craftRifle:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Rifle);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_IronRifle;
                    break;

                case WorkPriorityType.craftBlunderbus:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Blunderbuss);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_IronShotgun;
                    break;

                case WorkPriorityType.craftBallista:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.UnitType_Ballista);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Ballista;
                    break;

                case WorkPriorityType.craftManuBallista:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Manuballista);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Manuballista;
                    break;

                case WorkPriorityType.craftCatapult:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Catapult);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_Catapult;
                    break;

                case WorkPriorityType.craftBatteringRam:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_BatteringRam);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.MissingImage;
                    break;

                case WorkPriorityType.craftSiegeCannonBronze:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_SiegeCannonBronze);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_BronzeSiegeCannon;
                    break;

                case WorkPriorityType.craftManCannonBronze:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_ManCannonBronze);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_BronzeManCannon;
                    break;

                case WorkPriorityType.craftSiegeCannonIron:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_SiegeCannonIron);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_IronSiegeCannon;
                    break;

                case WorkPriorityType.craftManCannonIron:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_ManCannonIron);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_IronManCannon;
                    break;

                case WorkPriorityType.craftPaddedArmor:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_PaddedArmor);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_PaddedArmor;
                    break;

                case WorkPriorityType.craftHeavyPaddedArmor:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HeavyPaddedArmor);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_HeavyPaddedArmor;
                    break;

                case WorkPriorityType.craftBronzeArmor:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_BronzeArmor);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_BronzeArmor;
                    break;

                case WorkPriorityType.craftMailArmor:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_IronArmor);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_IronArmor;
                    break;

                case WorkPriorityType.craftHeavyMailArmor:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HeavyIronArmor);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_HeavyIronArmor;
                    break;

                case WorkPriorityType.craftPlateArmor:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_LightPlateArmor);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_LightPlateArmor;
                    break;

                case WorkPriorityType.craftFullPlateArmor:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_FullPlateArmor);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_FullPlateArmor;
                    break;

                case WorkPriorityType.craftMithrilArmor:
                    name = string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_MithrilArmor);
                    workIcon = SpriteName.WarsHammer;
                    typeIcon = SpriteName.WarsResource_MithrilArmor;
                    break;

                default:
                    name = "Unknown Work Type";
                    workIcon = SpriteName.NO_IMAGE;
                    typeIcon = SpriteName.NO_IMAGE;
                    break;
            }
        }
        public static string ExperienceLevel(ExperienceLevel level)
        {
            switch (level)
            {
                case XP.ExperienceLevel.Beginner_1:
                    return DssRef.lang.ExperienceLevel_1;
                case XP.ExperienceLevel.Practitioner_2:
                    return DssRef.lang.ExperienceLevel_2;
                case XP.ExperienceLevel.Expert_3:
                    return DssRef.lang.ExperienceLevel_3;
                case XP.ExperienceLevel.Master_4:
                    return DssRef.lang.ExperienceLevel_4;
                case XP.ExperienceLevel.Legendary_5:
                    return DssRef.lang.ExperienceLevel_5;
            }

            return TextLib.Error;
        }
        public static void ResearchType(bool isResearchCenter ,out string name, out SpriteName icon)
        {
            if (isResearchCenter)
            {
                icon = SpriteName.WarsBuild_ResearchCenter;
                name = DssRef.lang.BuildingType_ReseachCenter;
            }
            else
            {
                icon = SpriteName.WarsBuild_Bookpress;
                name = DssRef.lang.BuildingType_Bookpress;
            }
        }
        public static void ExperienceType(WorkExperienceType type, out string name, out SpriteName icon)
        {
            switch (type)
            {
                case WorkExperienceType.NONE:
                    name = DssRef.lang.Hud_None;
                    icon = SpriteName.BluePrintSquareFull;
                    break;

                case WorkExperienceType.Farm:
                    name = DssRef.lang.ExperienceType_Farm;
                    icon = SpriteName.WarsWorkFarm;
                    break;
                case WorkExperienceType.AnimalCare:
                    name = DssRef.lang.ExperienceType_AnimalCare;
                    icon = SpriteName.WarsBuild_HenPen;
                    break;
                case WorkExperienceType.HouseBuilding:
                    name = DssRef.lang.ExperienceType_HouseBuilding;
                    icon = SpriteName.WarsHammer;
                    break;
                case WorkExperienceType.WoodWork:
                    name = DssRef.lang.ExperienceType_WoodWork;
                    icon = SpriteName.WarsResource_Wood;
                    break;
                case WorkExperienceType.StoneCutter:
                    name = DssRef.lang.ExperienceType_StoneCutter;
                    icon = SpriteName.WarsResource_Stone;
                    break;
                case WorkExperienceType.Mining:
                    name = DssRef.lang.ExperienceType_Mining;
                    icon = SpriteName.WarsWorkMine;
                    break;
                case WorkExperienceType.Transport:
                    name = DssRef.lang.ExperienceType_Transport;
                    icon = SpriteName.WarsWorkMove;
                    break;
                case WorkExperienceType.Cook:
                    name = DssRef.lang.ExperienceType_Cook;
                    icon = SpriteName.WarsResource_Food;
                    break;
                case WorkExperienceType.Fletcher:
                    name = DssRef.lang.ExperienceType_Fletcher;
                    icon = SpriteName.WarsFletcherArrowIcon;
                    break;
                case WorkExperienceType.Smelting:
                    name = DssRef.lang.ExperienceType_RefineOre;
                    icon = SpriteName.WarsWorkSmelting;
                    break;
                case WorkExperienceType.CastMetal:
                    name = DssRef.lang.ExperienceType_Casting;
                    icon = SpriteName.WarsResource_Bronze;
                    break;
                case WorkExperienceType.CraftMetal:
                    name = DssRef.lang.ExperienceType_CraftMetal;
                    icon = SpriteName.WarsBuild_Smith;
                    break;
                case WorkExperienceType.CraftArmor:
                    name = DssRef.lang.ExperienceType_CraftArmor;
                    icon = SpriteName.WarsResource_IronArmor;
                    break;
                //case WorkExperienceType.CraftWeapon:
                //    name = DssRef.lang.ExperienceType_CraftWeapon;
                //    icon = SpriteName.WarsResource_Sword;
                //    break;
                case WorkExperienceType.CraftFuel:
                    name = DssRef.lang.ExperienceType_CraftFuel;
                    icon = SpriteName.WarsResource_Fuel;
                    break;
                case WorkExperienceType.Chemistry:
                    name = DssRef.lang.BuildingType_Chemist;
                    icon = SpriteName.WarsBuild_Chemist;
                    break;

                default:
                    name = TextLib.Error;
                    icon = SpriteName.NO_IMAGE;
                    break;
            }
        }

        public static string ValueSymbol(int value)
        {
            string result;
            var abs = Math.Abs(value);
            switch (abs)
            {
                case 100:
                    result = DssRef.lang.EngineHud_SymbolFor100;
                    break;
                case 1000:
                    result = DssRef.lang.EngineHud_SymbolFor1000;
                    break;
                case 10000:
                    result = DssRef.lang.EngineHud_SymbolFor10000;
                    break;
                default:
                    result = abs.ToString();
                    break;
            }

            if (value >= 0)
            {
                return "+" + result;
            }
            else
            {
                return "-" + result;
            }
        }
        public static SpriteName ExperienceLevelIcon(ExperienceLevel level)
        {
            if (level >= XP.ExperienceLevel.Legendary_5)
            {
                return SpriteName.WarsUnitLevelLegend;
            }
            return  (SpriteName)((int)SpriteName.WarsUnitLevelMinimal + (int)level);
        }

        //public static string Armor(ArmorLevel level)
        //{
        //    switch (level)
        //    {
        //        case ArmorLevel.None:
        //            return "None";
        //        case ArmorLevel.PaddedArmor: return DssRef.lang.Resource_TypeName_LightArmor;
        //        case ArmorLevel.Mail: return DssRef.lang.Resource_TypeName_MediumArmor;
        //        case ArmorLevel.FullPlate: return DssRef.lang.Resource_TypeName_HeavyArmor;

        //        default:
        //            throw new NotImplementedException();
        //    }
        //}

        //public static string Weapon(MainWeapon weapon)
        //{
        //    switch (weapon)
        //    {
        //        case MainWeapon.SharpStick: return DssRef.lang.Resource_TypeName_SharpStick;
        //        case MainWeapon.Sword: return DssRef.lang.Resource_TypeName_Sword;
        //        case MainWeapon.TwoHandSword: return DssRef.lang.Resource_TypeName_TwoHandSword;
        //        case MainWeapon.KnightsLance: return DssRef.lang.Resource_TypeName_KnightsLance;
        //        case MainWeapon.Bow: return DssRef.lang.Resource_TypeName_Bow;
        //        case MainWeapon.Longbow: return DssRef.lang.Resource_TypeName_Longbow;
        //        case MainWeapon.Ballista: return DssRef.lang.UnitType_Ballista;

        //        default:
        //            return TextLib.Error;
        //    }
        //}

        public static string InputActionName(InputActionType action)
        {
            switch (action)
            {
                case InputActionType.StopStart:
                    return DssRef.lang.InputActionName_StopStart;
                case InputActionType.ToggleHudDetail:
                    return DssRef.lang.InputActionName_ToggleHudDetail;
                case InputActionType.GameSpeed:
                    return DssRef.lang.Input_GameSpeed;
                case InputActionType.PauseGame:
                    return DssRef.lang.Input_Pause;
                case InputActionType.NextCity:
                    return DssRef.lang.InputActionName_NextCity;
                case InputActionType.NextArmy:
                    return DssRef.lang.InputActionName_NextArmy;
                case InputActionType.NextWar:
                    return DssRef.lang.InputActionName_NextWar;
                case InputActionType.Build:
                    return DssRef.lang.InputActionName_Build;
                case InputActionType.Copy:
                    return DssRef.lang.InputActionName_Copy;
                case InputActionType.Paste:
                    return DssRef.lang.InputActionName_Paste;
                case InputActionType.Menu:
                    return DssRef.lang.InputActionName_Menu;
                case InputActionType.FlagDesign_ToggleColor_Prev:
                    return DssRef.lang.InputActionName_FlagDesign_ToggleColor_Prev;
                case InputActionType.FlagDesign_ToggleColor_Next:
                    return DssRef.lang.InputActionName_FlagDesign_ToggleColor_Next;
                case InputActionType.FlagDesign_PaintBucket:
                    return DssRef.lang.InputActionName_FlagDesign_PaintBucket;
                case InputActionType.Controller_FlagDesign_Colorpicker:
                    return DssRef.lang.InputActionName_Controller_FlagDesign_Colorpicker;
                case InputActionType.ControllerFocus:
                    return DssRef.lang.InputActionName_ControllerFocus;
                case InputActionType.ControllerCancel:
                    return DssRef.lang.InputActionName_ControllerCancel;
                case InputActionType.ControllerMessageClick:
                    return DssRef.lang.InputActionName_ControllerMessageClick;
                //case InputActionType.ControllerSelect:
                //    return DssRef.lang.InputActionName_ControllerSelect;
                case InputActionType.WASD_UP:
                    return DssRef.lang.InputActionName_WASD_UP;
                case InputActionType.WASD_DOWN:
                    return DssRef.lang.InputActionName_WASD_DOWN;
                case InputActionType.WASD_LEFT:
                    return DssRef.lang.InputActionName_WASD_LEFT;
                case InputActionType.WASD_RIGHT:
                    return DssRef.lang.InputActionName_WASD_RIGHT;
                case InputActionType.CameraTiltLeft:
                    return DssRef.lang.InputActionName_CameraTiltLeft;
                case InputActionType.CameraTiltRight:
                    return DssRef.lang.InputActionName_CameraTiltRight;
                case InputActionType.CameraTiltUp:
                    return DssRef.lang.InputActionName_CameraTiltUp;
                case InputActionType.ZoomInKey:
                    return DssRef.lang.InputActionName_ZoomInKey;
                case InputActionType.ZoomOutKey:
                    return DssRef.lang.InputActionName_ZoomOutKey;

                default:
                    throw new NotImplementedException($"InputActionName not implemented for {action}");
            }
        }
        //public string Name(InputActionType type)
        //{
        //    switch (type)
        //    {

        //        case InputActionType.StopStart:
        //            return DssRef.lang.ArmyOption_Halt;
        //        //case InputButtonType.AutomationSetting:
        //        //    return DssRef.lang.Automation_Title;
        //        //case InputButtonType.Home:
        //        //    return;
        //        case InputActionType.ToggleHudDetail:
        //            return DssRef.lang.Input_ToggleHudDetail;
        //        case InputActionType.GameSpeed:
        //            return DssRef.lang.Input_GameSpeed;
        //        case InputActionType.PauseGame:
        //            return DssRef.lang.Input_Pause;
        //        case InputActionType.NextCity:
        //            return DssRef.lang.Input_NextCity;
        //        case InputActionType.NextArmy:
        //            return DssRef.lang.Input_NextArmy;
        //        case InputActionType.NextBattle:
        //            return DssRef.lang.Input_NextBattle;

        //        case InputActionType.ControllerFocus:
        //            return DssRef.lang.Input_ToggleHudFocus;
        //        case InputActionType.ControllerCancel:
        //            return Ref.langOpt.Hud_Cancel;
        //        case InputActionType.ControllerMessageClick:
        //            return DssRef.lang.Input_ClickMessage;

        //        case InputActionType.WASD_UP:
        //            return DssRef.lang.Input_Up;
        //        case InputActionType.WASD_DOWN:
        //            return DssRef.lang.Input_Down;
        //        case InputActionType.WASD_LEFT:
        //            return DssRef.lang.Input_Left;
        //        case InputActionType.WASD_RIGHT:
        //            return DssRef.lang.Input_Right;

        //        default:
        //            return "ERR";

        //    }
        //}
        public static SpriteName Training_Icon(TrainingLevel training)
        {
            return (SpriteName)((int)SpriteName.WarsUnitLevelMinimal + (int)training);
        }

        public static string Training(TrainingLevel training)
        {
            switch (training)
            {
                case TrainingLevel.Minimal: return DssRef.lang.Conscript_Training_Minimal;
                case TrainingLevel.Basic: return DssRef.lang.Conscript_Training_Basic;
                case TrainingLevel.Skillful: return DssRef.lang.Conscript_Training_Skillful;
                case TrainingLevel.Professional: return DssRef.lang.Conscript_Training_Professional;
                case TrainingLevel.Champion: return DssRef.lang.Conscript_Training_Champion;
                case TrainingLevel.Legendary: return DssRef.lang.Conscript_Training_Legendary;

                default:
                    throw new NotImplementedException();
            }
        }

        public static string SpecializationTypeName(SpecializationType specialization, out SpriteName icon)
        {
            switch (specialization)
            {
                case SpecializationType.None:
                    icon = SpriteName.BluePrintSquareFull;
                    return DssRef.lang.Hud_None;
                case SpecializationType.Field:
                    icon = SpriteName.WarsSpecializeField;
                    return DssRef.lang.Conscript_Specialization_Field;
                case SpecializationType.Sea:
                    icon = SpriteName.WarsSpecializeSea;
                    return DssRef.lang.Conscript_Specialization_Sea;
                case SpecializationType.Siege:
                    icon = SpriteName.WarsSpecializeSiege;
                    return DssRef.lang.Conscript_Specialization_Siege;
                case SpecializationType.Viking:
                    icon = SpriteName.WarsUnitIcon_Viking;
                    return DssRef.lang.UnitType_Viking;
                case SpecializationType.HonorGuard:
                    icon = SpriteName.WarsUnitIcon_Honorguard;
                    return DssRef.lang.UnitType_HonorGuard;
                case SpecializationType.Green:
                    icon = SpriteName.WarsUnitIcon_Greensoldier; 
                    return DssRef.lang.UnitType_GreenSoldier;
                case SpecializationType.Traditional:
                    icon = SpriteName.WarsSpecializeTradition;
                    return DssRef.lang.Conscript_Specialization_Traditional;
                case SpecializationType.AntiCavalry:

                    icon = SpriteName.WarsSpecializeAntiCavalry;
                    return DssRef.lang.Conscript_Specialization_AntiCavalry;
                case SpecializationType.CityGuard:

                    icon = SpriteName.WarsGuard;
                    return DssRef.lang.Conscript_Soldiers_GuardType;

                default:
                    icon = SpriteName.NO_IMAGE;
                    return TextLib.Error;
            }
        }

        public static string Tab(MenuTab tab, out string description, out Color? color)
        {
            color = null;

            switch (tab)
            {
                case MenuTab.Info:
                    description = null;
                    return DssRef.lang.MenuTab_Info;
                case MenuTab.Tag:
                    description = null;
                    return DssRef.lang.MenuTab_Tag;

                case MenuTab.Build:
                    description = DssRef.lang.MenuTab_Build_Description;
                    return DssRef.lang.MenuTab_Build;
                case MenuTab.Conscript:
                    description = DssRef.lang.BuildingType_Barracks_Description;
                    return DssRef.lang.Conscription_Title;
                case MenuTab.Defence:
                    description = null;
                    return DssRef.lang.Defence_Title;
                case MenuTab.Trade:
                    description = null;
                    return DssRef.lang.MenuTab_Trade;
                case MenuTab.BlackMarket:
                    description = DssRef.lang.MenuTab_BlackMarket_Description;
                    return DssRef.lang.Hud_BlackMarket;
                case MenuTab.Economy:
                    description = null;
                    return DssRef.lang.MenuTab_Economy;
                case MenuTab.Delivery:
                    description = DssRef.lang.BuildingType_Postal_Description;
                    return DssRef.lang.MenuTab_Delivery;
                case MenuTab.Resources:
                    description = DssRef.lang.MenuTab_Resources_Description;
                    return DssRef.lang.MenuTab_Resources;
                case MenuTab.Work:
                    description = DssRef.lang.MenuTab_Work_Description;
                    return DssRef.lang.MenuTab_Work;
                case MenuTab.Automation:
                    description = DssRef.lang.MenuTab_Automation_Description;
                    return DssRef.lang.Automation_Title;
                case MenuTab.Divide:
                    description = null;
                    return DssRef.lang.ArmyOption_Divide;
                case MenuTab.Disband:
                    description = null;
                    return DssRef.lang.ArmyOption_Disband;
                case MenuTab.Progress:
                    description= null;
                    return DssRef.lang.MenuTab_Progress;
                case MenuTab.Mix:
                    description = "All info compressed to one place";
                    return ".Mix";

                case MenuTab.Help:
                    description = null;
                    return DssRef.lang.Help_Title;

                case MenuTab.God_Recruit:
                    color = HudLib.GodPower_ColorBg;
                    description = null;
                    return DssRef.lang.MenuTab_Recruit;

                case MenuTab.Casual_Recruit:
                    description = null;
                    return DssRef.lang.MenuTab_Recruit;

                case MenuTab.Casual_Build:
                    description = DssRef.lang.MenuTab_Build_Description;
                    return DssRef.lang.MenuTab_Build;
                default:
                    throw new NotImplementedException();
            }
        }

        public static string TerrainName(TerrainMainType mainType, int subType)
        {
            switch (mainType)
            {
                case TerrainMainType.Building:
                    switch ((TerrainBuildingType)subType)
                    {
                        case TerrainBuildingType.Logistics:
                            return DssRef.lang.BuildingType_Logistics;
                        case TerrainBuildingType.SoldierBarracks:
                            return DssRef.lang.BuildingType_SoldierBarracks;
                        case TerrainBuildingType.Bank:
                            return DssRef.lang.BuildingType_Bank;
                        case TerrainBuildingType.CoinMinter:
                            return DssRef.lang.BuildingType_CoinMaker;
                        case TerrainBuildingType.Brewery:
                            return DssRef.lang.BuildingType_Brewery;
                        case TerrainBuildingType.Carpenter:
                            return DssRef.lang.BuildingType_Carpenter;
                        case TerrainBuildingType.Work_CoalPit:
                            return DssRef.lang.BuildingType_CoalPit;
                        case TerrainBuildingType.Work_Cook:
                            return DssRef.lang.BuildingType_Cook;
                        case TerrainBuildingType.HenPen:
                            return DssRef.lang.BuildingType_HenPen;
                        case TerrainBuildingType.Nobelhouse:
                            return DssRef.lang.Building_NobleHouse;
                        case TerrainBuildingType.ImmigrationTent:
                            return DssRef.lang.BuildingType_ImmigrationTent;
                        case TerrainBuildingType.PigPen:
                            return DssRef.lang.BuildingType_PigPen;

                        case TerrainBuildingType.Postal:
                            return DssRef.lang.BuildingType_Postal;
                        case TerrainBuildingType.PostalLevel2:
                        case TerrainBuildingType.PostalLevel3:
                            return string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_Postal);

                        case TerrainBuildingType.GoldDeliveryLevel1:
                            return DssRef.lang.BuildingType_GoldDelivery;
                        case TerrainBuildingType.GoldDeliveryLevel2:
                        case TerrainBuildingType.GoldDeliveryLevel3:
                            return string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_GoldDelivery);

                        case TerrainBuildingType.Recruitment:
                            return DssRef.lang.BuildingType_Recruitment;
                        case TerrainBuildingType.RecruitmentLevel2:
                        case TerrainBuildingType.RecruitmentLevel3:
                            return string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_Recruitment);

                        case TerrainBuildingType.Work_Smith:
                            return DssRef.lang.BuildingType_Smith;                       
                        case TerrainBuildingType.Storehouse:
                            return DssRef.lang.BuildingType_Storage;
                        case TerrainBuildingType.Tavern:
                            return DssRef.lang.BuildingType_Tavern;
                        case TerrainBuildingType.Work_Bench:
                            return DssRef.lang.BuildingType_WorkBench;
                        case TerrainBuildingType.WorkerHut:
                        case TerrainBuildingType.WorkerHutLarge:
                            return DssRef.lang.BuildingType_WorkerHut;

                        case TerrainBuildingType.Smelter:
                            return DssRef.lang.BuildingType_SmeltingFurnace;
                        case TerrainBuildingType.WoodCutter:
                            return DssRef.lang.BuildingType_WoodCutter;
                        case TerrainBuildingType.StoneCutter:
                            return DssRef.lang.BuildingType_StoneCutter;
                        case TerrainBuildingType.Embassy:
                            return DssRef.lang.BuildingType_Embassy;
                        case TerrainBuildingType.WaterResovoir:
                            return DssRef.lang.BuildingType_WaterResovoir;

                        case TerrainBuildingType.GuardHouse_Small:
                        case TerrainBuildingType.GuardHouse_Large:
                            return DssRef.lang.BuildingType_GuardOffice;
                        
                        case TerrainBuildingType.ArcherBarracks:
                            return DssRef.lang.BuildingType_ArcherBarracks;
                        case TerrainBuildingType.WarmachineBarracks:
                            return DssRef.lang.BuildingType_WarmachineBarracks;
                        case TerrainBuildingType.GunBarracks:
                            return DssRef.lang.BuildingType_GunBarracks;
                        case TerrainBuildingType.CannonBarracks:
                            return DssRef.lang.BuildingType_CannonBarracks;
                        case TerrainBuildingType.KnightsBarracks:
                            return DssRef.lang.BuildingType_KnightsBarracks;
                        case TerrainBuildingType.Foundry:
                            return DssRef.lang.BuildingType_Foundry;
                        case TerrainBuildingType.Armory:
                            return DssRef.lang.BuildingType_Armory;
                        case TerrainBuildingType.Chemist:
                            return DssRef.lang.BuildingType_Chemist;
                        case TerrainBuildingType.Gunmaker:
                            return DssRef.lang.BuildingType_Gunmaker;
                        case TerrainBuildingType.School:
                            return DssRef.lang.BuildingType_School;
                        case TerrainBuildingType.ResearchCenter:
                            return DssRef.lang.BuildingType_ReseachCenter;
                        case TerrainBuildingType.BookPress:
                            return DssRef.lang.BuildingType_Bookpress;

                        case TerrainBuildingType.ServiceMenHouse_small:
                        case TerrainBuildingType.ServiceMenHouse_Large:
                            return DssRef.lang.BuildingType_ServiceHouse;
                        
                        default:
                            return DssRef.lang.BuildingType_DefaultName;
                    }

                case TerrainMainType.Foil:
                    switch ((TerrainSubFoilType)subType)
                    {
                        default:
                            return DssRef.lang.LandType_Flatland;
                                                  

                        case TerrainSubFoilType.TreeHard:
                        case TerrainSubFoilType.TreeSoft:
                        case TerrainSubFoilType.DryWood:
                            return DssRef.lang.Resource_TypeName_Wood;

                        case TerrainSubFoilType.TreeSoftSprout:
                            return DssRef.lang.Building_TreeSprout_Soft;
                        case TerrainSubFoilType.TreeHardSprout:
                            return DssRef.lang.Building_TreeSprout_Hard;

                        case TerrainSubFoilType.StoneBlock:
                        case TerrainSubFoilType.Stones:
                            return DssRef.lang.Resource_TypeName_Stone;

                        case TerrainSubFoilType.LinenFarm:
                            return string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Linen);
                        case TerrainSubFoilType.LinenFarmUpgraded:
                            return string.Format(DssRef.lang.BuildingType_IsUpgraded, string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Linen));

                        case TerrainSubFoilType.WheatFarm:
                            return string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Wheat);
                        case TerrainSubFoilType.WheatFarmUpgraded:
                            return string.Format(DssRef.lang.BuildingType_IsUpgraded, string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Wheat));

                        case TerrainSubFoilType.RapeSeedFarm:
                            return string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Rapeseed);
                        case TerrainSubFoilType.RapeSeedFarmUpgraded:
                            return string.Format(DssRef.lang.BuildingType_IsUpgraded, string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Rapeseed));

                        case TerrainSubFoilType.HempFarm:
                            return string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Hemp);
                        case TerrainSubFoilType.HempFarmUpgraded:
                            return string.Format(DssRef.lang.BuildingType_IsUpgraded, string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Hemp));

                        case TerrainSubFoilType.BogIron:
                            return DssRef.lang.Resource_TypeName_BogIron;
                    }
                    //break;

                case TerrainMainType.Mine:
                    switch ((TerrainMineType)subType)
                    {
                        case TerrainMineType.IronOre:
                            return string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Iron);
                        case TerrainMineType.Coal:
                            return string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Coal);
                        case TerrainMineType.GoldOre:
                            return string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.ResourceType_Gold);

                        case TerrainMineType.TinOre:
                            return string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Tin);
                        case TerrainMineType.CopperOre:
                            return string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Copper);
                        case TerrainMineType.SilverOre:
                            return string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Silver);
                        case TerrainMineType.LeadOre:
                            return string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Lead);
                        case TerrainMineType.Mithril:
                            return string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Mithril);
                        case TerrainMineType.Sulfur:
                            return string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Sulfur);

                    }
                    break;

                case TerrainMainType.Road:
                    switch ((TerrainRoadType)subType)
                    {
                        case TerrainRoadType.DirtRoad:
                            return DssRef.lang.BuildingType_DirtRoad;
                    }
                    break;

                case TerrainMainType.Decor:
                    switch ((TerrainDecorType)subType)
                    {
                        case TerrainDecorType.Pavement:
                            return string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Pavement);
                        case TerrainDecorType.PavementFlower:
                            return string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Pavement);
                        case TerrainDecorType.PavementRectFlower:
                            return string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Pavement);
                        case TerrainDecorType.PavementLamp: 
                            return string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Pavement);
                        case TerrainDecorType.PavemenFountain: 
                            return string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Pavement);
                        

                        case TerrainDecorType.Statue_ThePlayer:
                            return string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Statue);

                        case TerrainDecorType.GardenFourBushes: return string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Garden); 
                        case TerrainDecorType.GardenLongTree: return string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Garden);
                        case TerrainDecorType.GardenWalledBush: return string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Garden); 
                        case TerrainDecorType.GardenGrass: return string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Garden);
                        case TerrainDecorType.GardenBird: return string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Garden);


                        case TerrainDecorType.GardenMemoryStone: return string.Format(DssRef.lang.VariantType_F, DssRef.lang.DecorType_Garden);
                        case TerrainDecorType.Statue_Leader: return string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Statue);
                        case TerrainDecorType.Statue_Lion: return string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Statue);
                        case TerrainDecorType.Statue_Horse: return string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Statue);
                        case TerrainDecorType.Statue_Pillar: return string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Statue);

                        case TerrainDecorType.FlagPole_LongBanner:
                            return string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Banner);
                        case TerrainDecorType.FlagPole_Banner:
                            return string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Banner);
                        case TerrainDecorType.FlagPole_SlimBanner:
                            return string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Banner);

                        case TerrainDecorType.FlagPole_Flag:
                            return string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Flag);
                        case TerrainDecorType.FlagPole_FlagRound:
                            return string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Flag);
                        case TerrainDecorType.FlagPole_FlagLarge:
                            return string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Flag);
                        case TerrainDecorType.FlagPole_Streamer:
                            return string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Flag);
                        case TerrainDecorType.FlagPole_Triangle:
                            return string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Flag);

                        case TerrainDecorType.CobbleStones:
                            return DssRef.lang.DecorType_CobbleStones;
                        case TerrainDecorType.Square:
                            return DssRef.lang.DecorType_Square;
                    }
                    break;

                case TerrainMainType.Destroyed:
                case TerrainMainType.DefaultLand:
                    return DssRef.lang.LandType_Flatland;
                case TerrainMainType.DefaultSea:
                    return DssRef.lang.LandType_Water;

                case TerrainMainType.Resourses:
                    return DssRef.lang.Resource;

                case TerrainMainType.Wall:

                    switch ((TerrainWallType)subType)
                    {
                        case TerrainWallType.Palisade: return DssRef.lang.BuildingType_Palisade;
                        case TerrainWallType.DirtWall:
                            return DssRef.lang.BuildingType_DirtWall;
                        case TerrainWallType.DirtTower:
                            return DssRef.lang.BuildingType_DirtTower;
                        case TerrainWallType.WoodWall: return DssRef.lang.BuildingType_WoodWall;
                        case TerrainWallType.WoodTower: return DssRef.lang.BuildingType_WoodTower;
                        case TerrainWallType.StoneWall: return string.Format(DssRef.lang.VariantType_A, DssRef.lang.BuildingType_StoneWall);
                        case TerrainWallType.StoneTower: return DssRef.lang.BuildingType_StoneTower;
                        case TerrainWallType.StoneWallGreen: return string.Format(DssRef.lang.VariantType_B, DssRef.lang.BuildingType_StoneWall);
                        case TerrainWallType.StoneWallBlueRoof: return string.Format(DssRef.lang.VariantType_C, DssRef.lang.BuildingType_StoneWall);
                        case TerrainWallType.StoneWallWoodHouse: return string.Format(DssRef.lang.VariantType_D, DssRef.lang.BuildingType_StoneWall);
                        case TerrainWallType.StoneGate: return DssRef.lang.BuildingType_StoneGate;
                        case TerrainWallType.StoneHouse: return DssRef.lang.BuildingType_StoneHouse;
                    }

                    return DssRef.lang.BuildingType_Wall;
            }


            return TextLib.Error + " (" + mainType.ToString() + " " + subType.ToString()+ ")";
        }
        public static string BuildingName(BuildAndExpandType buildingType)
        {
            var build = BuildLib.BuildOptions[(int)buildingType];
            return TerrainName(build.mainType, build.subType);
        }
        public static string BuildingDescription(TerrainBuildingType buildingType)
        {
            switch (buildingType)
            {
                case TerrainBuildingType.Logistics:
                    return DssRef.lang.BuildingType_Logistics_Description;
                case TerrainBuildingType.Tavern:
                    return DssRef.lang.BuildingType_Tavern_Description;
                case TerrainBuildingType.Storehouse:
                    return DssRef.lang.BuildingType_Storehouse_Description;
                case TerrainBuildingType.PigPen:
                    return DssRef.lang.BuildingType_PigPen_Description;
                case TerrainBuildingType.HenPen:
                    return DssRef.lang.BuildingType_HenPen_Description;
                case TerrainBuildingType.WorkerHut:
                    return string.Format(DssRef.lang.BuildingType_WorkerHut_DescriptionLimitX, DssConst.HousingCount_WorkerHut);
                case TerrainBuildingType.WorkerHutLarge:
                    return string.Format(DssRef.lang.BuildingType_WorkerHut_DescriptionLimitX, DssConst.HousingCount_WorkerHutLarge);

                case TerrainBuildingType.ServiceMenHouse_small:
                    return string.Format(DssRef.lang.BuildingType_ServiceHouse_DescriptionAddX, DssConst.HousingCount_ServiceHouse_Small);
                case TerrainBuildingType.ServiceMenHouse_Large:
                    return string.Format(DssRef.lang.BuildingType_ServiceHouse_DescriptionAddX, DssConst.HousingCount_ServiceHouse_Large);

                case TerrainBuildingType.GuardHouse_Small:
                    return string.Format(DssRef.lang.BuildingType_GuardOffice_DescriptionAddX, DssConst.HousingCount_GuardsOffice_Small);
                case TerrainBuildingType.GuardHouse_Large:
                    return string.Format(DssRef.lang.BuildingType_GuardOffice_DescriptionAddX, DssConst.HousingCount_GuardsOffice_Large);


                case TerrainBuildingType.Postal:
                case TerrainBuildingType.PostalLevel2:
                case TerrainBuildingType.PostalLevel3:
                    return DssRef.lang.BuildingType_Postal_Description;

                case TerrainBuildingType.Recruitment:
                case TerrainBuildingType.RecruitmentLevel2:
                case TerrainBuildingType.RecruitmentLevel3:
                    return DssRef.lang.BuildingType_Recruitment_Description;

                case TerrainBuildingType.SoldierBarracks:
                case TerrainBuildingType.ArcherBarracks:
                case TerrainBuildingType.WarmachineBarracks:
                case TerrainBuildingType.KnightsBarracks:
                case TerrainBuildingType.GunBarracks:
                case TerrainBuildingType.CannonBarracks:
                    return DssRef.lang.BuildingType_Barracks_Description;

                case TerrainBuildingType.Work_Cook:
                    return DssRef.lang.BuildingType_Cook_Description;
                case TerrainBuildingType.Work_Bench:
                    return DssRef.lang.BuildingType_Bench_Description;
                case TerrainBuildingType.Work_Smith:
                    return DssRef.lang.BuildingType_Smith_Description;
                case TerrainBuildingType.Smelter:
                    return DssRef.lang.BuildingType_SmeltingFurnace_Description;
                case TerrainBuildingType.Foundry:
                    return DssRef.lang.BuildingType_Foundry_Description;
                case TerrainBuildingType.Carpenter:
                    return DssRef.lang.BuildingType_Carpenter_Description;
                case TerrainBuildingType.Armory:
                    return DssRef.lang.BuildingType_Armory_Description;
                case TerrainBuildingType.Gunmaker:
                    return DssRef.lang.BuildingType_Gunmaker_Description;
                case TerrainBuildingType.Chemist:
                    return DssRef.lang.BuildingType_Chemist_Description;

                case TerrainBuildingType.Nobelhouse:
                    return DssRef.lang.BuildingType_Nobelhouse_Description;
                case TerrainBuildingType.Embassy:
                    return DssRef.lang.BuildingType_Embassy_Description;
                case TerrainBuildingType.Brewery:
                    return DssRef.lang.BuildingType_Tavern_Brewery;
                case TerrainBuildingType.Work_CoalPit:
                    return DssRef.lang.BuildingType_CoalPit_Description;
                case TerrainBuildingType.Bank:
                    return DssRef.lang.BuildingType_Bank_Description;
                case TerrainBuildingType.CoinMinter:
                    return DssRef.lang.BuildingType_CoinMaker_Description;

                case TerrainBuildingType.GoldDeliveryLevel1:
                case TerrainBuildingType.GoldDeliveryLevel2:
                case TerrainBuildingType.GoldDeliveryLevel3:
                    return DssRef.lang.BuildingType_GoldDelivery_Description;

                case TerrainBuildingType.WoodCutter:
                case TerrainBuildingType.StoneCutter:
                    return DssRef.lang.BuildingType_Workshop_Description;

                case TerrainBuildingType.WaterResovoir:
                    return DssRef.lang.BuildingType_WaterResovoir_Description;

                case TerrainBuildingType.School:
                    return DssRef.lang.BuildingType_School_Description;

                case TerrainBuildingType.BookPress:
                case TerrainBuildingType.ResearchCenter:
                    return DssRef.lang.BuildingType_Research_BaseDescription;

                case TerrainBuildingType.ImmigrationTent:
                    return string.Format(DssRef.lang.BuildingType_ImmigrationTent_Description, DssConst.ImmigrantionTent_Capacity);

                

                default:
                    return TextLib.Error;
            }
        }

        public static string Filter(BuildFilterTag tag)
        {
            switch (tag)
            {
                case BuildFilterTag.Workers: return DssRef.lang.ResourceType_Workers;
                case BuildFilterTag.Farm: return DssRef.lang.Work_Farming;
                case BuildFilterTag.Gold: return DssRef.lang.ResourceType_Gold;
                case BuildFilterTag.Resources: return DssRef.lang.Resource;
                case BuildFilterTag.Metals: return DssRef.lang.WarsResourceGroup_Metal;
                case BuildFilterTag.Water: return TextLib.LargeFirstLetter( DssRef.lang.Resource_TypeName_Water);
                case BuildFilterTag.Food: return TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Food);
                case BuildFilterTag.Fuel: return TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Fuel);
                case BuildFilterTag.Craft: return DssRef.lang.Work_Craft;
                case BuildFilterTag.Transport: return DssRef.lang.ExperienceType_Transport;
                case BuildFilterTag.Weapons: return DssRef.lang.WarsResourceGroup_Weapons;
                case BuildFilterTag.Military: return DssRef.lang.BuildCategory_Military;
                case BuildFilterTag.Soldiers: return DssRef.lang.UnitType_Soldier;
                case BuildFilterTag.Guards: return DssRef.lang.Conscript_Soldiers_GuardType;
                case BuildFilterTag.Walls: return DssRef.lang.BuildingType_Wall;
                case BuildFilterTag.Optimize: return DssRef.lang.Category_Optimize;
                case BuildFilterTag.Upgrade: return DssRef.lang.BuildCategory_Upgrade;
                case BuildFilterTag.Research: return DssRef.lang.Research_Tab;
                case BuildFilterTag.Road: return DssRef.lang.BuildCategory_Road;
                case BuildFilterTag.Garden: return DssRef.lang.DecorType_Garden;
                case BuildFilterTag.Statue: return DssRef.lang.DecorType_Statue;
                case BuildFilterTag.Flag: return DssRef.lang.DecorType_Flag;
                default: return tag.ToString(); // fallback, or you can return a specific string like DssRef.lang.Unknown
            }
        }


        public static string TechnologyExample()
        {
            // 0: beer, 1: chemistry, 2: gun powder
            return string.Format(DssRef.lang.Technology_ReseachExample,
                DssRef.lang.Resource_TypeName_Beer,
                DssRef.lang.ExperienceType_Chemist,
                XpLib.TechnologyName_BlackPowder());
        }

        public static void Technology(TechnologyTreeType type, out SpriteName icon, out string name)
        {
            switch (type)
            {
                case TechnologyTreeType.advancedBuilding:
                    icon = SpriteName.WarsBuild_Nobelhouse;
                    name = DssRef.lang.Technology_AdvancedBuildings;
                    break;
                case TechnologyTreeType.advancedCasting:
                    icon = SpriteName.WarsResource_IronManCannon;
                    name = DssRef.lang.Technology_AdvancedCasting;
                    break;
                case TechnologyTreeType.advancedFarming:
                    icon = SpriteName.WarsWorkFarm;
                    name = DssRef.lang.Technology_AdvancedFarming;
                    break;
                case TechnologyTreeType.iron:
                    icon = SpriteName.WarsResource_Iron;
                    name = DssRef.lang.Resource_TypeName_Iron;
                    break;
                case TechnologyTreeType.steel:
                    icon = SpriteName.WarsResource_Steel;
                    name = DssRef.lang.Resource_TypeName_Steel;
                    break;
                case TechnologyTreeType.blackPowder:
                    icon = SpriteName.WarsResource_BlackPowder;
                    name = XpLib.TechnologyName_BlackPowder();
                    break;
                case TechnologyTreeType.gunPowder:
                    icon = SpriteName.WarsResource_GunPowder;
                    name = DssRef.lang.Resource_TypeName_GunPowder;
                    break;
                case TechnologyTreeType.catapult:
                    icon = SpriteName.WarsResource_Catapult;
                    name = DssRef.lang.Resource_TypeName_Catapult;
                    break;

                default:
                    name = TextLib.Error;
                    icon = SpriteName.MissingImage;
                    break;


            }

            //tech(technology.advancedBuilding.points, TechnologyTemplate.AdvancedBuildingUnlock, SpriteName.WarsBuild_Nobelhouse, DssRef.lang.Technology_AdvancedBuildings);
            //tech(technology.advancedFarming.points, TechnologyTemplate.AdvancedFarmingUnlock, SpriteName.WarsWorkFarm, DssRef.lang.Technology_AdvancedFarming);
            //tech(technology.advancedCasting.points, TechnologyTemplate.AdvancedCastingUnlock, SpriteName.WarsResource_IronManCannon, DssRef.lang.Technology_AdvancedCasting);

            //tech(technology.iron.points, TechnologyTemplate.IronUnlock, SpriteName.WarsResource_Iron, DssRef.lang.Resource_TypeName_Iron);
            //tech(technology.steel.points, TechnologyTemplate.SteelUnlock, SpriteName.WarsResource_Steel, DssRef.lang.Resource_TypeName_Steel);
            //tech(technology.catapult.points, TechnologyTemplate.CatapultUnlock, SpriteName.WarsResource_Catapult, DssRef.lang.Resource_TypeName_Catapult);
            //tech(technology.blackPowder.points, TechnologyTemplate.BlackPowderUnlock, SpriteName.WarsResource_BronzeRifle, XpLib.TechnologyName_BlackPowder());
            //tech(technology.gunPowder.points, TechnologyTemplate.GunPowderUnlock, SpriteName.WarsResource_IronRifle, DssRef.lang.Resource_TypeName_GunPowder);
        }

        public static string Item(ItemResourceType item)
        {
            switch (item)
            {
                case ItemResourceType.NONE: return DssRef.lang.Hud_None;

                case ItemResourceType.GoldOre: return DssRef.lang.Resource_TypeName_GoldOre;
                case ItemResourceType.Gold: return DssRef.lang.ResourceType_Gold;

                case ItemResourceType.Water_G: return DssRef.lang.Resource_TypeName_Water;
                case ItemResourceType.Beer: return DssRef.lang.Resource_TypeName_Beer;
                case ItemResourceType.CoolingFluid: return DssRef.lang.Resource_TypeName_CoolingFluid;
                case ItemResourceType.IronOre_G: return DssRef.lang.Resource_TypeName_IronOre;
                case ItemResourceType.Iron_G: return DssRef.lang.Resource_TypeName_Iron;
                case ItemResourceType.BogIron: return DssRef.lang.Resource_TypeName_BogIron;
                case ItemResourceType.Food_G: return DssRef.lang.Resource_TypeName_Food;
                case ItemResourceType.Stone_G: return DssRef.lang.Resource_TypeName_Stone;

                case ItemResourceType.DryWood:
                case ItemResourceType.SoftWood:
                case ItemResourceType.HardWood:
                case ItemResourceType.Wood_Group: 
                    return DssRef.lang.Resource_TypeName_Wood;

                case ItemResourceType.Coal:
                case ItemResourceType.Fuel_G: 
                    return DssRef.lang.Resource_TypeName_Fuel;
                
                case ItemResourceType.Pig:
                case ItemResourceType.Hen:
                case ItemResourceType.Egg:
                case ItemResourceType.Wheat:
                case ItemResourceType.RawFood_Group:
                    return DssRef.lang.Resource_TypeName_RawFood;

                case ItemResourceType.Linen:
                case ItemResourceType.SkinLinen_Group: 
                    return DssRef.lang.Resource_TypeName_Linen;

                case ItemResourceType.Rapeseed:
                    return DssRef.lang.Resource_TypeName_Rapeseed;
                case ItemResourceType.Hemp:
                    return DssRef.lang.Resource_TypeName_Hemp;

                case ItemResourceType.SharpStick:
                    return DssRef.lang.Resource_TypeName_SharpStick;
                case ItemResourceType.Sword:
                    return DssRef.lang.Resource_TypeName_Sword;
                case ItemResourceType.TwoHandSword:
                    return DssRef.lang.Resource_TypeName_TwoHandSword;
                case ItemResourceType.KnightsLance:
                    return DssRef.lang.Resource_TypeName_KnightsLance;
                case ItemResourceType.Bow:
                    return DssRef.lang.Resource_TypeName_Bow;
                case ItemResourceType.LongBow:
                    return DssRef.lang.Resource_TypeName_Longbow;
                case ItemResourceType.Ballista:
                    return DssRef.lang.UnitType_Ballista;

                //case ItemResourceType.PaddedArmor:
                //    return DssRef.lang.Resource_TypeName_LightArmor;
                //case ItemResourceType.IronArmor:
                //    return DssRef.lang.Resource_TypeName_MediumArmor;
                //case ItemResourceType.HeavyIronArmor:
                //    return DssRef.lang.Resource_TypeName_HeavyArmor;


                case ItemResourceType.Palisade:
                    return DssRef.lang.Resource_TypeName_Palisade;
                case ItemResourceType.Wagon2Wheel:
                    return DssRef.lang.Resource_TypeName_Wagon2Wheel;
                case ItemResourceType.Wagon4Wheel:
                    return DssRef.lang.Resource_TypeName_Wagon4Wheel;
                case ItemResourceType.Tin:
                    return DssRef.lang.Resource_TypeName_Tin;
                case ItemResourceType.TinOre:
                    return DssRef.lang.Resource_TypeName_TinOre;
                case ItemResourceType.Bronze:
                    return DssRef.lang.Resource_TypeName_Bronze;
                case ItemResourceType.Copper:
                    return DssRef.lang.Resource_TypeName_Copper;
                case ItemResourceType.CopperOre:
                    return DssRef.lang.Resource_TypeName_CopperOre;
                case ItemResourceType.Silver:
                    return DssRef.lang.Resource_TypeName_Silver;
                case ItemResourceType.SilverOre:
                    return DssRef.lang.Resource_TypeName_SilverOre;
                case ItemResourceType.Mithril:
                    return DssRef.lang.Resource_TypeName_Mithril;
                case ItemResourceType.RawMithril:
                    return DssRef.lang.Resource_TypeName_RawMithril;

                case ItemResourceType.BronzeSword:
                    return DssRef.lang.Resource_TypeName_BronzeSword;
                case ItemResourceType.ShortSword:
                    return DssRef.lang.Resource_TypeName_ShortSword;
                case ItemResourceType.LongSword:
                    return DssRef.lang.Resource_TypeName_LongSword;
                case ItemResourceType.HandSpear:
                    return DssRef.lang.Resource_TypeName_HandSpear;
                case ItemResourceType.Pike:
                    return DssRef.lang.Resource_TypeName_Pike;

                case ItemResourceType.Warhammer:
                    return DssRef.lang.Resource_TypeName_Warhammer;
                case ItemResourceType.MithrilSword:
                    return DssRef.lang.Resource_TypeName_MithrilSword;
                case ItemResourceType.SlingShot:
                    return DssRef.lang.Resource_TypeName_SlingShot;
                case ItemResourceType.ThrowingSpear:
                    return DssRef.lang.Resource_TypeName_ThrowingSpear;
                case ItemResourceType.Crossbow:
                    return DssRef.lang.Resource_TypeName_Crossbow;
                case ItemResourceType.MithrilBow:
                    return DssRef.lang.Resource_TypeName_MithrilBow;

                case ItemResourceType.Toolkit:
                    return DssRef.lang.Resource_TypeName_Toolkit;

                case ItemResourceType.Sulfur:
                    return DssRef.lang.Resource_TypeName_Sulfur;
                case ItemResourceType.LeadOre:
                    return DssRef.lang.Resource_TypeName_LeadOre;
                case ItemResourceType.Lead:
                    return DssRef.lang.Resource_TypeName_Lead;
                case ItemResourceType.BloomeryIron:
                    return DssRef.lang.Resource_TypeName_BloomIron;
                case ItemResourceType.Steel:
                    return DssRef.lang.Resource_TypeName_Steel;
                case ItemResourceType.CastIron:
                    return DssRef.lang.Resource_TypeName_CastIron;

                case ItemResourceType.BlackPowder:
                    return DssRef.lang.Resource_TypeName_BlackPowder;
                case ItemResourceType.GunPowder:
                    return DssRef.lang.Resource_TypeName_GunPowder;
                case ItemResourceType.LedBullet:
                    return DssRef.lang.Resource_TypeName_LedBullet;

                case ItemResourceType.HandCannon:
                    return DssRef.lang.Resource_TypeName_HandCannon;
                case ItemResourceType.HandCulverin:
                    return DssRef.lang.Resource_TypeName_HandCulverin;
                case ItemResourceType.Rifle:
                    return DssRef.lang.Resource_TypeName_Rifle;
                case ItemResourceType.Blunderbuss:
                    return DssRef.lang.Resource_TypeName_Blunderbuss;

                case ItemResourceType.Manuballista:
                    return DssRef.lang.Resource_TypeName_Manuballista;
                case ItemResourceType.Catapult:
                    return DssRef.lang.Resource_TypeName_Catapult;
                case ItemResourceType.SiegeCannonBronze:
                    return DssRef.lang.Resource_TypeName_SiegeCannonBronze;
                case ItemResourceType.ManCannonBronze:
                    return DssRef.lang.Resource_TypeName_ManCannonBronze;
                case ItemResourceType.SiegeCannonIron:
                    return DssRef.lang.Resource_TypeName_SiegeCannonIron;
                case ItemResourceType.ManCannonIron:
                    return DssRef.lang.Resource_TypeName_ManCannonIron;

                case ItemResourceType.PaddedArmor:
                    return DssRef.lang.Resource_TypeName_PaddedArmor;
                case ItemResourceType.HeavyPaddedArmor:
                    return DssRef.lang.Resource_TypeName_HeavyPaddedArmor;

                case ItemResourceType.IronArmor:
                    return DssRef.lang.Resource_TypeName_IronArmor;
                case ItemResourceType.HeavyIronArmor:
                    return DssRef.lang.Resource_TypeName_HeavyIronArmor;

                case ItemResourceType.BronzeArmor:
                    return DssRef.lang.Resource_TypeName_BronzeArmor;

                case ItemResourceType.LightPlateArmor:
                    return DssRef.lang.Resource_TypeName_LightPlateArmor;
                case ItemResourceType.FullPlateArmor:
                    return DssRef.lang.Resource_TypeName_FullPlateArmor;
                case ItemResourceType.MithrilArmor:
                    return DssRef.lang.Resource_TypeName_MithrilArmor;

                case ItemResourceType.Men:
                    return DssRef.lang.ResourceType_Workers;
                case ItemResourceType.ServiceMen:
                    return DssRef.lang.ResourceType_ServiceMen;

                case ItemResourceType.CopperCoin:
                case ItemResourceType.BronzeCoin:
                case ItemResourceType.SilverCoin:
                case ItemResourceType.ElfCoin:
                    return DssRef.lang.Resource_TypeName_Coin;

                default:
                    return TextLib.Error;
            }
        }

        public static string CityCulture(CityCulture cityCulture, bool title)
        {
            switch (cityCulture)
            {
                case DSSWars.CityCulture.AnimalBreeder: return title ? DssRef.lang.CityCulture_AnimalBreeder : DssRef.lang.CityCulture_AnimalBreeder_Description;
                case DSSWars.CityCulture.Archers: return title ? DssRef.lang.CityCulture_Archers : DssRef.lang.CityCulture_Archers_Description;
                case DSSWars.CityCulture.Builders: return title ? DssRef.lang.CityCulture_Builders : DssRef.lang.CityCulture_Builders_Description;
                case DSSWars.CityCulture.CrabMentality: return title ? DssRef.lang.CityCulture_CrabMentality : DssRef.lang.CityCulture_CrabMentality_Description;
                case DSSWars.CityCulture.DeepWell: return title ? DssRef.lang.CityCulture_DeepWell : DssRef.lang.CityCulture_DeepWell_Description;
                case DSSWars.CityCulture.FertileGround: return title ? DssRef.lang.CityCulture_FertileGround : DssRef.lang.CityCulture_FertileGround_Description;
                case DSSWars.CityCulture.LargeFamilies: return title ? DssRef.lang.CityCulture_LargeFamilies : DssRef.lang.CityCulture_LargeFamilies_Description;
                case DSSWars.CityCulture.Miners: return title ? DssRef.lang.CityCulture_Miners : DssRef.lang.CityCulture_Miners_Description;
                case DSSWars.CityCulture.Warriors: return title ? DssRef.lang.CityCulture_Warriors : DssRef.lang.CityCulture_Warriors_Description;
                case DSSWars.CityCulture.Woodcutters: return title ? DssRef.lang.CityCulture_Woodcutters : DssRef.lang.CityCulture_Woodcutters_Description;
                case DSSWars.CityCulture.Networker: return title ? DssRef.lang.CityCulture_Networker : DssRef.lang.CityCulture_Networker_Description;
                case DSSWars.CityCulture.PitMasters: return title ? DssRef.lang.CityCulture_PitMasters : DssRef.lang.CityCulture_PitMasters_Description;


                case DSSWars.CityCulture.Stonemason:
                    return title ? DssRef.lang.CityCulture_Stonemason : DssRef.lang.CityCulture_Stonemason_Description;
                case DSSWars.CityCulture.Brewmaster:
                    return title ? DssRef.lang.CityCulture_Brewmaster : DssRef.lang.CityCulture_Brewmaster_Description;
                case DSSWars.CityCulture.Weavers:
                    return title ? DssRef.lang.CityCulture_Weavers : DssRef.lang.CityCulture_Weavers_Description;
                case DSSWars.CityCulture.SiegeEngineer:
                    return title ? DssRef.lang.CityCulture_SiegeEngineer : DssRef.lang.CityCulture_SiegeEngineer_Description;
                case DSSWars.CityCulture.Armorsmith:
                    return title ? DssRef.lang.CityCulture_Armorsmith : DssRef.lang.CityCulture_Armorsmith_Description;
                case DSSWars.CityCulture.Noblemen:
                    return title ? DssRef.lang.CityCulture_Noblemen : DssRef.lang.CityCulture_Noblemen_Description;
                case DSSWars.CityCulture.Seafaring:
                    return title ? DssRef.lang.CityCulture_Seafaring : DssRef.lang.CityCulture_Seafaring_Description;
                case DSSWars.CityCulture.Backtrader:
                    return title ? DssRef.lang.CityCulture_Backtrader : DssRef.lang.CityCulture_Backtrader_Description;
                case DSSWars.CityCulture.Lawbiding:
                    return title ? DssRef.lang.CityCulture_LawAbiding : DssRef.lang.CityCulture_LawAbiding_Description;


                case DSSWars.CityCulture.Smelters:
                    return title ? DssRef.lang.CityCulture_Smelters : DssRef.lang.CityCulture_Smelters_Description;
                case DSSWars.CityCulture.BronzeCasters:
                    return title ? DssRef.lang.CityCulture_BronzeCasters : DssRef.lang.CityCulture_BronzeCasters_Description;
                case DSSWars.CityCulture.Apprentices:
                    return title ? DssRef.lang.CityCulture_Apprentices : DssRef.lang.CityCulture_Apprentices_Description;

                default: return TextLib.Error;
            }
        }

        public static string UnitFilterName(UnitFilterType filterType)
        {
            switch (filterType)
            {
                case UnitFilterType.SharpStick:
                    return DssRef.lang.UnitType_Folkman;
                
                case UnitFilterType.Sword:
                case UnitFilterType.LongSword:
                    return DssRef.lang.UnitType_Soldier;
                case UnitFilterType.Pike:
                case UnitFilterType.SpearAndShield:
                    return DssRef.lang.UnitType_Spearman;

                case UnitFilterType.Skirmisher:
                    return DssRef.lang.UnitType_Skirmisher;
                case UnitFilterType.Bow:
                    return DssRef.lang.UnitType_Archer;

                case UnitFilterType.CrossBow:
                    return DssRef.lang.UnitType_Crossbow;
                case UnitFilterType.Rifle:
                    return DssRef.lang.UnitType_LongRangeRifle;
                case UnitFilterType.Shotgun:
                    return DssRef.lang.UnitType_CloseRangeRifle;


                case UnitFilterType.Ballista:
                    return DssRef.lang.UnitType_Ballista;
                case UnitFilterType.ManuBallista:
                    return DssRef.lang.Resource_TypeName_Manuballista;
                case UnitFilterType.Catapult:
                    return DssRef.lang.Resource_TypeName_Catapult;

                case UnitFilterType.ManCannonBronze:
                    return DssRef.lang.Resource_TypeName_ManCannonBronze;
                case UnitFilterType.SiegeCannonBronze:
                    return DssRef.lang.Resource_TypeName_SiegeCannonBronze;
                case UnitFilterType.ManCannonIron:
                    return DssRef.lang.Resource_TypeName_ManCannonIron;
                case UnitFilterType.SiegeCannonIron:
                    return DssRef.lang.Resource_TypeName_SiegeCannonIron;

                case UnitFilterType.TwohandSword:
                case UnitFilterType.Warhammer:
                    return DssRef.lang.UnitType_FootKnight;
                case UnitFilterType.Knight:
                    return DssRef.lang.UnitType_CavalryKnight;
                case UnitFilterType.MithrilBow:
                    return DssRef.lang.UnitType_MithrilArcher;
                case UnitFilterType.MithrilKnight:
                    return DssRef.lang.UnitType_MithrilSwordsman;

                case UnitFilterType.GreenSoldier:
                    return DssRef.lang.FactionName_Greenwood;
                case UnitFilterType.HonourGuard:
                    return DssRef.lang.UnitType_HonorGuard;
                case UnitFilterType.Viking:
                    return DssRef.lang.UnitType_Viking;

                default:
                    return TextLib.Error;
            }
        }
    }
}
