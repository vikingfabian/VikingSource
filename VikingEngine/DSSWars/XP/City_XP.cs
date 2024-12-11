using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.XP;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.Players;
using VikingEngine.DSSWars.Display.Translation;
using System.Reflection.Metadata;
using VikingEngine.DSSWars.Delivery;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        public TechnologyTemplate technology = new TechnologyTemplate();

        public ExperienceLevel topskill_Farm = 0;
        public ExperienceLevel topskill_AnimalCare = 0;
        public ExperienceLevel topskill_HouseBuilding = 0;
        public ExperienceLevel topskill_WoodCutter = 0;
        public ExperienceLevel topskill_StoneCutter = 0;
        public ExperienceLevel topskill_Mining = 0;
        public ExperienceLevel topskill_Transport = 0;
        public ExperienceLevel topskill_Cook = 0;
        public ExperienceLevel topskill_Fletcher = 0;
        public ExperienceLevel topskill_Smelting = 0;
        public ExperienceLevel topskill_Casting = 0;
        public ExperienceLevel topskill_CraftMetal = 0;
        public ExperienceLevel topskill_CraftArmor = 0;
        public ExperienceLevel topskill_CraftWeapon = 0;
        public ExperienceLevel topskill_CraftFuel = 0;
        public ExperienceLevel topskill_Chemistry = 0;

        public ExperienceOrDistancePrio experenceOrDistance = ExperienceOrDistancePrio.Mix;

        public int selectedSchool = -1;
        public List<SchoolStatus> schoolBuildings = new List<SchoolStatus>();

        public void onSchoolComplete_async(IntVector2 subPos)
        {
            Ref.update.AddSyncAction(new SyncAction(() =>
            {
                var index = SchoolIxFromSubTile(subPos);
                if (arraylib.InBound(schoolBuildings, index))
                {
                    SchoolStatus currentStatus = schoolBuildings[index];
                    --currentStatus.que;
                   schoolBuildings[index] = currentStatus;
                }
            }));
        }

        public void addSchool(IntVector2 subPos)
        {
            SchoolStatus newBuilding = new SchoolStatus()
            {
                idAndPosition = conv.IntVector2ToInt(subPos),
            };
            newBuilding.defaulSetup();

            lock (schoolBuildings)
            {
                schoolBuildings.Add(newBuilding);
            }
        }
        public void destroySchool(IntVector2 subPos)
        {
            lock (schoolBuildings)
            {
                int index = SchoolIxFromSubTile(subPos);
                schoolBuildings.RemoveAt(index);
            }
        }

        public int SchoolIxFromSubTile(IntVector2 subTilePos)
        {
            int id = conv.IntVector2ToInt(subTilePos);
            for (int i = 0; i < schoolBuildings.Count; ++i)
            {
                if (schoolBuildings[i].idAndPosition == id)
                {
                    return i;
                }
            }

            return -1;
        }
        public ExperienceLevel GetTopSkill(WorkExperienceType experienceType)
        {
            switch (experienceType)
            {
                case WorkExperienceType.Farm: return topskill_Farm;

                case WorkExperienceType.AnimalCare: return topskill_Farm;
                case WorkExperienceType.HouseBuilding: return topskill_Farm;
                case WorkExperienceType.WoodCutter: return topskill_Farm;
                case WorkExperienceType.StoneCutter: return topskill_Farm;
                case WorkExperienceType.Mining: return topskill_Farm;
                case WorkExperienceType.Transport: return topskill_Farm;
                case WorkExperienceType.Cook: return topskill_Farm;
                case WorkExperienceType.Fletcher: return topskill_Farm;
                case WorkExperienceType.Smelting: return topskill_Farm;
                case WorkExperienceType.CastMetal: return topskill_Farm;
                case WorkExperienceType.CraftMetal: return topskill_Farm;
                case WorkExperienceType.CraftArmor: return topskill_Farm;
                case WorkExperienceType.CraftWeapon: return topskill_Farm;
                case WorkExperienceType.CraftFuel: return topskill_Farm;
                case WorkExperienceType.Chemistry: return topskill_Farm;

                default: throw new NotImplementedException();
            }
        }

        public void onMasterLevel(WorkExperienceType experienceType)
        {
            switch (experienceType)
            {
                case WorkExperienceType.HouseBuilding:
                case WorkExperienceType.StoneCutter:
                    technology.advancedBuilding += DssConst.TechnologyGain_Master;
                    break;

                case WorkExperienceType.Farm:
                case WorkExperienceType.AnimalCare:
                    technology.advancedFarming += DssConst.TechnologyGain_Master;
                    break;

                case WorkExperienceType.Smelting:
                case WorkExperienceType.CastMetal:
                    technology.advancedCasting += DssConst.TechnologyGain_Master;
                    break;

                case WorkExperienceType.Mining:
                case WorkExperienceType.CraftMetal:
                    if (technology.iron < TechnologyTemplate.Unlocked)
                    {
                        technology.iron += DssConst.TechnologyGain_Master;
                    }
                    else
                    {
                        technology.steel += DssConst.TechnologyGain_Master;
                    }
                    break;

                case WorkExperienceType.WoodCutter:
                case WorkExperienceType.Fletcher:
                    technology.catapult += DssConst.TechnologyGain_Master;
                    break;

                case WorkExperienceType.CraftFuel:
                case WorkExperienceType.Chemistry:
                    if (technology.iron < TechnologyTemplate.Unlocked)
                    {
                        technology.blackPowder += DssConst.TechnologyGain_Master;
                    }
                    else
                    {
                        technology.gunPowder += DssConst.TechnologyGain_Master;
                    }
                    break;



                    //    var advBuildingFields = new List<WorkExperienceType>
                    //{
                    //    WorkExperienceType.HouseBuilding,
                    //    WorkExperienceType.StoneCutter,
                    //};
                    //var advFarmingFields = new List<WorkExperienceType>
                    //{
                    //    WorkExperienceType.,
                    //    WorkExperienceType.AnimalCare,
                    //};
                    //var advCastingFields = new List<WorkExperienceType>
                    //{
                    //    WorkExperienceType.Smelting,
                    //    WorkExperienceType.CastMetal,
                    //};
                    //var ironSteelFields = new List<WorkExperienceType>
                    //{
                    //    WorkExperienceType.Mining,
                    //    WorkExperienceType.CraftMetal,
                    //};
                    //var catapultFields = new List<WorkExperienceType>
                    //{
                    //    WorkExperienceType.WoodCutter,
                    //    WorkExperienceType.Fletcher,
                    //};

                    //var gunPowderFields = new List<WorkExperienceType>
                    //{
                    //    WorkExperienceType.CraftFuel,
                    //    WorkExperienceType.Chemistry,
                    //};
            }
        }
    }
}
