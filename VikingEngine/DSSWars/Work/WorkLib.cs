﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;

namespace VikingEngine.DSSWars.Work
{
    static class WorkLib
    {
        public static byte[] WorkToXPTable;

        public static void Init()
        {
            
            WorkToXPTable = new byte[(int)WorkExperienceType.NUM];
            WorkToXPTable[(int)WorkExperienceType.Farm] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.AnimalCare] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.HouseBuilding] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.WoodCutter] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.StoneCutter] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.Mining] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.Transport] = 2;
            WorkToXPTable[(int)WorkExperienceType.Cook] = 2;
            WorkToXPTable[(int)WorkExperienceType.Fletcher] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.CraftMetal] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.CraftArmor] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.CraftWeapon] = DssConst.DefaultWorkXpGain;
            WorkToXPTable[(int)WorkExperienceType.CraftFuel] = 1;
        }

        public static WorkExperienceType WorkToExperienceType(WorkType work, int workSubType, IntVector2 subTileEnd)
        {
            WorkExperienceType gainXp = WorkExperienceType.NONE;
            switch (work)
            {
                case WorkType.GatherFoil:
                    {
                        SubTile subTile = DssRef.world.subTileGrid.Get(subTileEnd);

                        switch (subTile.GetFoilType())
                        {
                            case TerrainSubFoilType.TreeSoft:
                            case TerrainSubFoilType.TreeHard:
                            case TerrainSubFoilType.DryWood:
                                gainXp = WorkExperienceType.WoodCutter;
                                break;

                            case TerrainSubFoilType.WheatFarm:
                            case TerrainSubFoilType.LinenFarm:
                            case TerrainSubFoilType.RapeSeedFarm:
                            case TerrainSubFoilType.HempFarm:
                                gainXp = WorkExperienceType.Farm;
                                break;


                            case TerrainSubFoilType.StoneBlock:
                            case TerrainSubFoilType.Stones:
                                gainXp = WorkExperienceType.StoneCutter;
                                break;

                            case TerrainSubFoilType.BogIron:
                                gainXp = WorkExperienceType.Mining;
                                break;
                        }                      
                    }
                    break;

                case WorkType.Till:
                    gainXp = WorkExperienceType.Farm;
                    break;

                case WorkType.Plant:
                    gainXp = WorkExperienceType.Farm;
                    break;

                case WorkType.PickUpProduce:
                    gainXp = WorkExperienceType.AnimalCare;
                    break;

                case WorkType.DropOff:
                case WorkType.PickUpResource:
                    gainXp = WorkExperienceType.Transport;
                    break;

                case WorkType.Mine:
                    gainXp = WorkExperienceType.Mining;
                    break;

                case WorkType.Craft:
                    ItemResourceType item = (ItemResourceType)workSubType;
                    ItemPropertyColl.Blueprint(item, out CraftBlueprint bp1, out var bp2);
                    gainXp = bp1.experienceType;
                    break;

                case WorkType.Build:
                    var build = BuildLib.BuildOptions[workSubType];
                    gainXp = build.experienceType();
                    break;
            }

            return gainXp;
        }

        public static ExperienceLevel ToLevel(byte xp)
        {
            ExperienceLevel level = (ExperienceLevel)(xp / DssConst.WorkXpToLevel);
            return level;
        }

        public static float WorkTimePerc(byte xp, byte timeBonusPerc)
        {
            return 1f - (xp / DssConst.WorkXpToLevel * DssConst.XpLevelWorkTimePercReduction) - timeBonusPerc * MathExt.OnePercentage;
        }
    }

    enum WorkType
    {
        IsDeleted,
        Idle,
        Exit,
        Starving,
        Eat,

        Till,
        Plant,
        GatherFoil,
        //GatherCityProduce,
        Mine,
        PickUpResource,
        PickUpProduce,
        DropOff,
        Craft,
        Build,
        Demolish,
        LocalTrade,

        TrossCityTrade,
        TrossReturnToArmy,
    }

    
}