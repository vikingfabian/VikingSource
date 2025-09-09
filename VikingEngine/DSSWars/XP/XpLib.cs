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
            //WorkExperienceType.CraftWeapon,
            WorkExperienceType.CraftFuel,
            WorkExperienceType.Chemistry,
        };

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

        //    tech(technology.advancedBuilding, TechnologyTemplate.AdvancedBuildingUnlock, SpriteName.WarsBuild_Nobelhouse, DssRef.lang.Technology_AdvancedBuildings);
        //tech(technology.advancedFarming, TechnologyTemplate.AdvancedFarmingUnlock, SpriteName.WarsWorkFarm, DssRef.lang.Technology_AdvancedFarming);
        //tech(technology.advancedCasting, TechnologyTemplate.AdvancedCastingUnlock, SpriteName.WarsResource_IronManCannon, DssRef.lang.Technology_AdvancedCasting);

        //tech(technology.iron, TechnologyTemplate.IronUnlock, SpriteName.WarsResource_Iron, DssRef.lang.Resource_TypeName_Iron);
        //tech(technology.steel, TechnologyTemplate.SteelUnlock, SpriteName.WarsResource_Steel, DssRef.lang.Resource_TypeName_Steel);
        //tech(technology.catapult, TechnologyTemplate.CatapultUnlock, SpriteName.WarsResource_Catapult, DssRef.lang.Resource_TypeName_Catapult);
        //tech(technology.blackPowder, TechnologyTemplate.BlackPowderUnlock, SpriteName.WarsResource_BronzeRifle, DssRef.lang.Resource_TypeName_BlackPowder);
        //tech(technology.gunPowder, TechnologyTemplate.GunPowderUnlock, SpriteName.WarsResource_IronRifle, DssRef.lang.Resource_TypeName_GunPowder);

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
