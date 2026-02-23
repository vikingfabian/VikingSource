using System;
using VikingEngine.DSSWars.Work;

namespace VikingEngine.DSSWars.XP
{
    class WorkerSkillCollector
    {
        int[,] WorkType_LevelCount = new int[(int)WorkExperienceType.NUM_NONE, (int)XpLib.XpLevelCount];

        public void Add(ref WorkerStatus status)
        {
            //addXp(status.xpType1, status.xp1);
            //addXp(status.xpType2, status.xp2);
            //addXp(status.xpType3, status.xp3);
            for (WorkExperienceType type = 0; type < WorkExperienceType.NUM_NONE; type++)
            {
                addXp(type, status.getXpFor(type).xp);
            }

            void addXp(WorkExperienceType type, byte xp)
            {
                if (xp > 0)
                {
                    int level = Bound.Max(xp / DssConst.WorkXpToLevel, XpLib.MaxXpLevel);
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
    
}
