using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataLib;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;

namespace VikingEngine.DSSWars.Work
{
    static class WorkLib
    {
        public static byte[] WorkToXPTable;

        public static WorkPriorityType[][] ViewOrder_Group_Work;

        public static void Init()
        {
            ViewOrder_Group_Work = new WorkPriorityType[(int)ResourceGroupType.NUM][];

            ViewOrder_Group_Work[(int)ResourceGroupType.Resources] = new WorkPriorityType[]
            {
        WorkPriorityType.move,
        WorkPriorityType.wood,
        WorkPriorityType.stone,
        WorkPriorityType.collectClay,
        WorkPriorityType.miningBrick,
        WorkPriorityType.craftBrick,
        WorkPriorityType.farmFood,
        WorkPriorityType.farmRawFood,
        WorkPriorityType.miningSalt,
        WorkPriorityType.farmfuel,
        WorkPriorityType.farmlinen,
        WorkPriorityType.craftFood,
        WorkPriorityType.craftConservedFood,
        WorkPriorityType.craftFuel,
        WorkPriorityType.craftBeer,
        WorkPriorityType.craftCoolingFluid,
        WorkPriorityType.craftContainer,
        WorkPriorityType.craftPalisade,
        WorkPriorityType.craftToolkit,
        WorkPriorityType.craftWagon2Wheel,
        WorkPriorityType.craftWagon4Wheel,
        WorkPriorityType.craftWagonClosed,
        WorkPriorityType.craftWagonIron,
        WorkPriorityType.craftWagonSteel,
        WorkPriorityType.craftBlackPowder,
        WorkPriorityType.craftGunPowder,
        WorkPriorityType.craftBullet,
        WorkPriorityType.autoBuild,
        WorkPriorityType.buildOrders
            };

            ViewOrder_Group_Work[(int)ResourceGroupType.Metals] = new WorkPriorityType[]
            {
        WorkPriorityType.bogiron,
        WorkPriorityType.miningIron,
        WorkPriorityType.miningTin,
        WorkPriorityType.miningCopper,
        WorkPriorityType.miningLead,
        WorkPriorityType.miningSilver,
        WorkPriorityType.miningGold,
        WorkPriorityType.miningMithril,
        WorkPriorityType.miningSulfur,
        WorkPriorityType.miningCoal,
        WorkPriorityType.smeltIron,
        WorkPriorityType.smeltTin,
        WorkPriorityType.smeltCopper,
        WorkPriorityType.smeltLead,
        WorkPriorityType.smeltSilver,
        WorkPriorityType.smeltGold,
        WorkPriorityType.craftBronze,
        WorkPriorityType.craftCastIron,
        WorkPriorityType.craftBloomeryIron,
        WorkPriorityType.craftSteel,
        WorkPriorityType.craftMithril
            };

            ViewOrder_Group_Work[(int)ResourceGroupType.Weapons] = new WorkPriorityType[]
            {
        WorkPriorityType.craftSharpStick,
        WorkPriorityType.craftBronzeSword,
        WorkPriorityType.craftShortSword,
        WorkPriorityType.craftSword,
        WorkPriorityType.craftLongSword,
        WorkPriorityType.craftHandSpear,
        WorkPriorityType.craftWarhammer,
        WorkPriorityType.craftTwoHandSword,
        WorkPriorityType.craftMithrilSword,
        WorkPriorityType.craftBucklerShield,
        WorkPriorityType.craftRoundShield,
        WorkPriorityType.craftHeaterShield,
        WorkPriorityType.craftTowerShield
            };

            ViewOrder_Group_Work[(int)ResourceGroupType.Projectile] = new WorkPriorityType[]
            {
        WorkPriorityType.craftSlingshot,
        WorkPriorityType.craftThrowingspear,
        WorkPriorityType.craftBow,
        WorkPriorityType.craftLongbow,
        WorkPriorityType.craftCrossbow,
        WorkPriorityType.craftMithrilbow,
        WorkPriorityType.craftHandCannon,
        WorkPriorityType.craftHandCulverin,
        WorkPriorityType.craftRifle,
        WorkPriorityType.craftBlunderbuss,
        WorkPriorityType.craftBallista,
        WorkPriorityType.craftManuBallista,
        WorkPriorityType.craftCatapult,
        WorkPriorityType.craftSiegeCannonBronze,
        WorkPriorityType.craftManCannonBronze,
        WorkPriorityType.craftSiegeCannonIron,
        WorkPriorityType.craftManCannonIron
            };

            ViewOrder_Group_Work[(int)ResourceGroupType.Armor] = new WorkPriorityType[]
            {
        WorkPriorityType.craftPaddedArmor,
        WorkPriorityType.craftHeavyPaddedArmor,
        WorkPriorityType.craftBronzeArmor,
        WorkPriorityType.craftMailArmor,
        WorkPriorityType.craftHeavyMailArmor,
        WorkPriorityType.craftPlateArmor,
        WorkPriorityType.craftFullPlateArmor,
        WorkPriorityType.craftMithrilArmor,
        WorkPriorityType.craftMountPaddedArmor,
        WorkPriorityType.craftMountHeavyPaddedArmor,
        WorkPriorityType.craftMountBronzeArmor,
        WorkPriorityType.craftMountMailArmor,
        WorkPriorityType.craftMountHeavyMailArmor,
        WorkPriorityType.craftMountPlateArmor,
        WorkPriorityType.craftMountFullPlateArmor,
        WorkPriorityType.craftMountMithrilArmor
            };

            ViewOrder_Group_Work[(int)ResourceGroupType.Animals] = new WorkPriorityType[]
            {
        WorkPriorityType.SlaughterFowl,
        WorkPriorityType.SlaughterHen,
        WorkPriorityType.SlaughterBoar,
        WorkPriorityType.SlaughterPig,
        WorkPriorityType.SlaughterOxen,
        WorkPriorityType.SlaughterKineOxen,
        WorkPriorityType.SlaughterPony,
        WorkPriorityType.SlaughterHorse,
        WorkPriorityType.SlaughterWarHorse,
        WorkPriorityType.SlaughterDraftHorse,
        WorkPriorityType.SlaughterWildPig,
        WorkPriorityType.SlaughterWildHog,
        WorkPriorityType.SlaughterWarHog,
        WorkPriorityType.SlaughterStagHog,
        WorkPriorityType.SlaughterWolf,
        WorkPriorityType.SlaughterWarg,
        WorkPriorityType.SlaughterAlphaWarg,
        WorkPriorityType.SlaughterWildCat,
        WorkPriorityType.SlaughterLion,
        WorkPriorityType.SlaughterWarLion,
        WorkPriorityType.SlaughterElephant,
        WorkPriorityType.SlaughterWarElephant,
        WorkPriorityType.SlaughterOliphant
            };

            ViewOrder_Group_Work[(int)ResourceGroupType.Mint] = new WorkPriorityType[]
            {
        WorkPriorityType.coinmaker_copper,
        WorkPriorityType.coinmaker_bronze,
        WorkPriorityType.coinmaker_silver,
        WorkPriorityType.coinmaker_mithril
            };


            WorkToXPTable = new byte[(int)WorkExperienceType.NUM_NONE];
            //WorkToXPTable[(int)WorkExperienceType.NUM_NONE] = byte.MaxValue;
            WorkToXPTable[(int)WorkExperienceType.Farm] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.AnimalCare] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.HouseBuilding] = (byte)(DssConst.DefaultWorkXpGain * 2);
            WorkToXPTable[(int)WorkExperienceType.WoodWork] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.StoneCutter] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.Mining] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.Transport] = 2;
            WorkToXPTable[(int)WorkExperienceType.Cook] = 2;
            WorkToXPTable[(int)WorkExperienceType.Fletcher] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.Smelting] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.CastMetal] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.CraftMetal] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.CraftArmor] = DssConst.DefaultWorkXpGain;
            //WorkToXPTable[(int)WorkExperienceType.CraftWeapon] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.CraftFuel] = 2;
            WorkToXPTable[(int)WorkExperienceType.Chemistry] = DssConst.DefaultWorkXpGain;


#if DEBUG
            foreach (var xp in WorkToXPTable)
            {
                if (xp == 0)
                {
                    throw new Exception();
                }
            }
#endif
        }

        public static WorkExperienceType WorkToExperienceType(WorkType work, int workSubType, byte bonus, IntVector2 subTileEnd, City city, 
            out ExperienceLevel requiredLvl, out int requiredXp, out int maxXp)
        {
            WorkExperienceType gainXpType = WorkExperienceType.NUM_NONE;
            maxXp = int.MaxValue;
            requiredXp = 0;
            requiredLvl = ExperienceLevel.Beginner_1;

            switch (work)
            {
                case WorkType.GatherFoil:
                    {
                        if (DssRef.world.subTileGrid.TryGet(subTileEnd, out MapTile1_1 subTile))
                        {

                            switch (subTile.GetFoilType())
                            {
                                case TerrainSubFoilType.TreeSoft:
                                case TerrainSubFoilType.TreeHard:
                                case TerrainSubFoilType.DryWood:
                                    gainXpType = WorkExperienceType.WoodWork;
                                    break;

                                case TerrainSubFoilType.LargeStones:
                                case TerrainSubFoilType.SmallStones:
                                    //case TerrainSubFoilType.ClayPit:
                                    gainXpType = WorkExperienceType.StoneCutter;
                                    break;

                                    //case TerrainSubFoilType.BogIron:
                                    //    gainXpType = WorkExperienceType.Mining;
                                    //    break;


                            }
                        }
                    }
                    break;

                case WorkType.GatherFarm:
                    gainXpType = WorkExperienceType.Farm;
                    break;
                //case WorkType.Till:
                //    gainXp = WorkExperienceType.Farm;
                //    break;

                case WorkType.Plant:
                    gainXpType = WorkExperienceType.Farm;
                    break;

                case WorkType.PickUpProduce:
                    gainXpType = WorkExperienceType.AnimalCare;
                    break;

                case WorkType.DropOff:
                case WorkType.PickUpResource:
                    gainXpType = WorkExperienceType.Transport;
                    break;

                case WorkType.Mine:
                    {
                        if (DssRef.world.subTileGrid.TryGet(subTileEnd, out MapTile1_1 subTile))
                        {
                            switch ((TerrainMineType)subTile.subTerrain)
                            {
                                case TerrainMineType.ClayPit:
                                case TerrainMineType.StoneBlock:
                                    gainXpType = WorkExperienceType.StoneCutter;
                                    break;
                                default:
                                    gainXpType = WorkExperienceType.Mining;
                                    break;
                            }
                        }
                    }
                    
                    break;

                case WorkType.Craft:
                    ItemResourceType item = (ItemResourceType)workSubType;
                    ItemPropertyColl.Blueprint(item, out CraftBlueprint bp1, out var bp2);
                    gainXpType = bp1.experienceType;
                    requiredLvl = bp1.levelRequirement;
                    requiredXp = DssConst.WorkXpToLevel * (int)requiredLvl;
                    break;

                case WorkType.Build:
                    var build = BuildLib.BuildOptions[workSubType];
                    gainXpType = build.experienceType();
                    requiredLvl = build.blueprint.levelRequirement;
                    requiredXp = DssConst.WorkXpToLevel * (int)requiredLvl;
                    break;

                case WorkType.School:
                    gainXpType = (WorkExperienceType)workSubType;
                    maxXp = bonus * DssConst.WorkXpToLevel;
                    //lock (city.schoolBuildings)
                    //{
                    //    var ix = city.SchoolIxFromSubTile(subTileEnd);
                    //    if (arraylib.TryGet(city.schoolBuildings, ix, out SchoolStatus status))
                    //    { 
                    //        maxXp = (int)status.toLevel * DssConst.WorkXpToLevel;
                    //    }
                    //}
                    break;
            }

            return gainXpType;
        }

        

        public static float WorkTimePerc(byte xp, byte timeBonusPerc)
        {
            return 1f - (xp / DssConst.WorkXpToLevel * DssConst.XpLevelWorkTimePercReduction) - timeBonusPerc * MathExt.OnePercentage;
        }
    }

    enum WorkType : byte
    {
        IsDeleted,
        Idle,
        Exit,
        Starving,
        Eat,
        Plant,
        GatherFoil,
        GatherFarm,
        Mine,
        PickUpResource,
        PickUpProduce,
        DropOff,
        Craft,
        Build,
        Upgrade,
        Demolish,
        LocalTrade,

        TrossCityTrade,
        TrossReturnToArmy,

        School,
    }

    
}
