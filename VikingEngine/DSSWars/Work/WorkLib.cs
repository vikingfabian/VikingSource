using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Work
{
    static class WorkLib
    {
        public static ExperienceLevel ToLevel(byte xp)
        {
            ExperienceLevel level = (ExperienceLevel)(xp / DssConst.WorkXpToLevel);
            return level;
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
        LocalTrade,

        TrossCityTrade,
        TrossReturnToArmy,
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
        CraftWood,
        CraftIron,
        CraftArmor,
        CraftWeapon,
        CraftFuel,
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
}
