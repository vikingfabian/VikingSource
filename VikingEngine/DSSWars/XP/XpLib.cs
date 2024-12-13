using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            WorkExperienceType.CraftWeapon,
            WorkExperienceType.CraftFuel,
            WorkExperienceType.Chemistry,
        };

        public static ExperienceLevel ToLevel(byte xp)
        {
            ExperienceLevel level = (ExperienceLevel)(xp / DssConst.WorkXpToLevel);
            return level;
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
        CraftWeapon,
        CraftFuel,
        Chemistry,
        NUM
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
}
