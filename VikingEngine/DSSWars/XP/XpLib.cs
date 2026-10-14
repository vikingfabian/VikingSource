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

        public static TechTreeStructure TechTree;

        public static readonly int XpLevelCount = (int)ExperienceLevel.NUM;
        public static readonly int MaxXpLevel = XpLevelCount - 1;
        public static ExperienceLevel ToLevel(byte xp)
        {
            ExperienceLevel level = (ExperienceLevel)(xp / DssConst.WorkXpToLevel);
            return level;
        }

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
