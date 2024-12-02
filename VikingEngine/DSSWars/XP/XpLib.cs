using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.XP
{
    static class XpLib
    {
    }

    enum WorkExperienceType : byte
    {
        NONE,
        Farm,
        AnimalCare,
        HouseBuilding,
        WoodCutter,
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
