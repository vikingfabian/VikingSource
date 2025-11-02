using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Work;

namespace VikingEngine.DSSWars.XP
{
    static class XpLib
    {
        public static WorkExperienceType[] ExperienceTypes =
        {
            WorkExperienceType.Farm,
            WorkExperienceType.AnimalCare,
            WorkExperienceType.HouseBuilding,
            WorkExperienceType.WoodWork,
            WorkExperienceType.StoneCutter,
            WorkExperienceType.Mining,
            WorkExperienceType.Transport,
            WorkExperienceType.Cook,
            WorkExperienceType.Fletcher,
            WorkExperienceType.Smelting,
            WorkExperienceType.CastMetal,
            WorkExperienceType.CraftMetal,
            WorkExperienceType.CraftArmor,
            //WorkExperienceType.CraftWeapon,
            WorkExperienceType.CraftFuel,
            WorkExperienceType.Chemistry,
        };

        public static TechnologyUnlock Unlock;

        public static readonly int XpLevelCount = (int)ExperienceLevel.NUM;
        public static readonly int MaxXpLevel = XpLevelCount - 1;
        public static ExperienceLevel ToLevel(byte xp)
        {
            ExperienceLevel level = (ExperienceLevel)(xp / DssConst.WorkXpToLevel);
            return level;
        }

        //public static int ToLevel_int(byte xp)
        //{
        //    ExperienceLevel level = xp / DssConst.WorkXpToLevel;
        //    return level;
        //}

        public static string TechnologyName_BlackPowder()
        {
            return DssRef.lang.Resource_TypeName_BlackPowder;
        }

        public static void AdjustVersion80Skill(ref WorkExperienceType experienceType)
        {
            if (experienceType >= WorkExperienceType.CraftFuel)
            {
                if (experienceType == WorkExperienceType.CraftFuel)
                {
                    experienceType = WorkExperienceType.CraftMetal;
                }
                else
                {
                    --experienceType;
                }
            }
        }

        //    tech(technology.advancedBuilding, TechnologyTemplate.AdvancedBuildingUnlock, SpriteName.WarsBuild_Nobelhouse, DssRef.lang.Technology_AdvancedBuildings);
        //tech(technology.advancedFarming, TechnologyTemplate.AdvancedFarmingUnlock, SpriteName.WarsWorkFarm, DssRef.lang.Technology_AdvancedFarming);
        //tech(technology.advancedCasting, TechnologyTemplate.AdvancedCastingUnlock, SpriteName.WarsResource_IronManCannon, DssRef.lang.Technology_AdvancedCasting);

        //tech(technology.iron, TechnologyTemplate.IronUnlock, SpriteName.WarsResource_Iron, DssRef.lang.Resource_TypeName_Iron);
        //tech(technology.steel, TechnologyTemplate.SteelUnlock, SpriteName.WarsResource_Steel, DssRef.lang.Resource_TypeName_Steel);
        //tech(technology.catapult, TechnologyTemplate.CatapultUnlock, SpriteName.WarsResource_Catapult, DssRef.lang.Resource_TypeName_Catapult);
        //tech(technology.blackPowder, TechnologyTemplate.BlackPowderUnlock, SpriteName.WarsResource_BronzeRifle, DssRef.lang.Resource_TypeName_BlackPowder);
        //tech(technology.gunPowder, TechnologyTemplate.GunPowderUnlock, SpriteName.WarsResource_IronRifle, DssRef.lang.Resource_TypeName_GunPowder);

    }

    class WorkerSkillCollector
    {
        int[,] WorkType_LevelCount = new int[(int)WorkExperienceType.NUM, (int)XpLib.XpLevelCount];

        public void Add(ref WorkerStatus status)
        {
            addXp(status.xpType1, status.xp1);
            addXp(status.xpType2, status.xp2);
            addXp(status.xpType3, status.xp3);

            void addXp(WorkExperienceType type, byte xp)
            {
                if (xp > 0)
                {
                    int level = Bound.Max( xp / DssConst.WorkXpToLevel, XpLib.MaxXpLevel);
                    ++WorkType_LevelCount[(int)type, level];
                }
            }
        }

        public CityExperienceLevels ExportData()
        {
            CityExperienceLevels result = new CityExperienceLevels();
            workType(WorkExperienceType.Farm, ref result.levels_Farm);
            workType(WorkExperienceType.AnimalCare, ref result.levels_AnimalCare);
            workType(WorkExperienceType.HouseBuilding, ref result.levels_HouseBuilding);
            workType(WorkExperienceType.WoodWork, ref result.levels_WoodCutter);
            workType(WorkExperienceType.StoneCutter, ref result.levels_StoneCutter);
            workType(WorkExperienceType.Mining, ref result.levels_Mining);
            workType(WorkExperienceType.Transport, ref result.levels_Transport);
            workType(WorkExperienceType.Cook, ref result.levels_Cook);
            workType(WorkExperienceType.Fletcher, ref result.levels_Fletcher);
            workType(WorkExperienceType.Smelting, ref result.levels_Smelting);
            workType(WorkExperienceType.CastMetal, ref result.levels_Casting);
            workType(WorkExperienceType.CraftMetal, ref result.levels_CraftMetal);
            workType(WorkExperienceType.CraftArmor, ref result.levels_CraftArmor);
            workType(WorkExperienceType.CraftFuel, ref result.levels_CraftFuel);
            workType(WorkExperienceType.Chemistry, ref result.levels_Chemistry);

            return result;

            void workType(WorkExperienceType type, ref WorkExperienceLevels levels)
            {
                int typeIx = (int)type;

                levelCount(0, ref levels, ref levels.Beginner_1_count);
                levelCount(1, ref levels, ref levels.Practitioner_2_count);
                levelCount(2, ref levels, ref levels.Expert_3_count);
                levelCount(3, ref levels, ref levels.Master_4_count);
                levelCount(4, ref levels, ref levels.Legendary_5_count);
                //add all 5 levels

                void levelCount(int level, ref WorkExperienceLevels levels, ref int count)
                {
                    count = WorkType_LevelCount[typeIx, level];
                    WorkType_LevelCount[typeIx, level] = 0;

                    if (count > 0)
                    {
                        levels.maxLevel = level;
                    }
                }
            }
        }
    }

    struct CityExperienceLevels
    {
        public WorkExperienceLevels levels_Farm;
        public WorkExperienceLevels levels_AnimalCare;
        public WorkExperienceLevels levels_HouseBuilding;
        public WorkExperienceLevels levels_WoodCutter;
        public WorkExperienceLevels levels_StoneCutter;
        public WorkExperienceLevels levels_Mining;
        public WorkExperienceLevels levels_Transport;
        public WorkExperienceLevels levels_Cook;
        public WorkExperienceLevels levels_Fletcher;
        public WorkExperienceLevels levels_Smelting;
        public WorkExperienceLevels levels_Casting;
        public WorkExperienceLevels levels_CraftMetal;
        public WorkExperienceLevels levels_CraftArmor;
        public WorkExperienceLevels levels_CraftFuel;
        public WorkExperienceLevels levels_Chemistry;

        public WorkExperienceLevels Get(WorkExperienceType experienceType)
        {
            switch (experienceType)
            {
                case WorkExperienceType.Farm: return levels_Farm;
                case WorkExperienceType.AnimalCare: return levels_AnimalCare;
                case WorkExperienceType.HouseBuilding: return levels_HouseBuilding;
                case WorkExperienceType.WoodWork: return levels_WoodCutter;
                case WorkExperienceType.StoneCutter: return levels_StoneCutter;
                case WorkExperienceType.Mining: return levels_Mining;
                case WorkExperienceType.Transport: return levels_Transport;
                case WorkExperienceType.Cook: return levels_Cook;
                case WorkExperienceType.Fletcher: return levels_Fletcher;
                case WorkExperienceType.Smelting: return levels_Smelting;
                case WorkExperienceType.CastMetal: return levels_Casting;
                case WorkExperienceType.CraftMetal: return levels_CraftMetal;
                case WorkExperienceType.CraftArmor: return levels_CraftArmor;
                //case WorkExperienceType.CraftWeapon: return levels_CraftWeapon;
                case WorkExperienceType.CraftFuel: return levels_CraftFuel;
                case WorkExperienceType.Chemistry: return levels_Chemistry;

                default: throw new NotImplementedException();
            }
        }
    }

    struct WorkExperienceLevels
    {
        public int maxLevel;

        public int Beginner_1_count;
        public int Practitioner_2_count;
        public int Expert_3_count;
        public int Master_4_count;
        public int Legendary_5_count;

        public ExperienceLevel Max()
        {
            return (ExperienceLevel)maxLevel;
        }
    }

    enum WorkExperienceType : byte
    {
        NONE,
        Farm,
        AnimalCare,
        HouseBuilding,
        WoodWork,
        StoneCutter,
        Mining,
        Transport,
        Cook,
        Fletcher,
        Smelting,
        CastMetal,
        CraftMetal,
        CraftArmor,
        //CraftWeapon,
        CraftFuel,
        Chemistry,
        //GodPower,
        NUM
    }
    enum TechnologyTreeType
    {
        advancedBuilding,
        advancedFarming,
        advancedCasting,
        iron,
        steel,
        catapult,
        blackPowder,
        gunPowder,
        NUM_NONE
    }
    enum ExperienceLevel
    {
        Beginner_1,
        Practitioner_2,
        //Specialist,
        Expert_3,
        Master_4,
        Legendary_5,
        NUM
    }


    enum ExperienceOrDistancePrio
    {
        Distance,
        Mix,
        Experience,
        NUM
    }

    enum  TechnologyGainReason
    {
        WorkerLevel,
        CityToCitySpread,
        FactionToFactionSpread,
        BookPress,
    }
}
