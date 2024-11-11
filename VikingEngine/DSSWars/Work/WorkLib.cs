using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Work
{
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
    }

    enum ExperienceLevel
    { 
        Beginner,
        Practitioner,
        //Specialist,
        Expert,
        Master,
        Legendary,
        NUM
    }
}
