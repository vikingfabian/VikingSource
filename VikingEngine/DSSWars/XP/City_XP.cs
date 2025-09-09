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
using VikingEngine.DSSWars.Presentation;
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
        //public ExperienceLevel topskill_CraftWeapon = 0;
        public ExperienceLevel topskill_CraftFuel = 0;
        public ExperienceLevel topskill_Chemistry = 0;

        public ExperienceOrDistancePrio experenceOrDistance = ExperienceOrDistancePrio.Mix;

        public int selectedSchool = -1;
        public List<SchoolStatus> schoolBuildings = new List<SchoolStatus>();

        public int selectedResearchBuilding = -1;
        public List<ResearchBuilding> researchBuildings = null;

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

        public void addResearchBuilding(IntVector2 subPos, bool isResearchCenter)
        {
            ResearchBuilding newBuilding = new ResearchBuilding()
            {
                assignedTech = TechnologyTreeType.NUM_NONE,
                idAndPosition = conv.IntVector2ToInt(subPos),
                isResearchCenter  = isResearchCenter
            };

            if (researchBuildings == null)
            {
                researchBuildings = new List<ResearchBuilding>(4);
            }

            lock (researchBuildings)
            {
                researchBuildings.Add(newBuilding);
            }
        }
        public void destroyResearchBuilding(IntVector2 subPos)
        {
            lock (researchBuildings)
            {
                int index = ResearchIxFromSubTile(subPos);
                researchBuildings.RemoveAt(index);
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
        public int ResearchIxFromSubTile(IntVector2 subTilePos)
        {
            int id = conv.IntVector2ToInt(subTilePos);
            for (int i = 0; i < researchBuildings.Count; ++i)
            {
                if (researchBuildings[i].idAndPosition == id)
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
                case WorkExperienceType.AnimalCare: return topskill_AnimalCare;
                case WorkExperienceType.HouseBuilding: return topskill_HouseBuilding;
                case WorkExperienceType.WoodWork: return topskill_WoodCutter;
                case WorkExperienceType.StoneCutter: return topskill_StoneCutter;
                case WorkExperienceType.Mining: return topskill_Mining;
                case WorkExperienceType.Transport: return topskill_Transport;
                case WorkExperienceType.Cook: return topskill_Cook;
                case WorkExperienceType.Fletcher: return topskill_Fletcher;
                case WorkExperienceType.Smelting: return topskill_Smelting;
                case WorkExperienceType.CastMetal: return topskill_Casting;
                case WorkExperienceType.CraftMetal: return topskill_CraftMetal;
                case WorkExperienceType.CraftArmor: return topskill_CraftArmor;
                //case WorkExperienceType.CraftWeapon: return topskill_CraftWeapon;
                case WorkExperienceType.CraftFuel: return topskill_CraftFuel;
                case WorkExperienceType.Chemistry: return topskill_Chemistry;

                default: throw new NotImplementedException();
            }
        }

        public void addTechPoints(WorkExperienceType experienceType, int gain, TechnologyGainReason reason)
        {
            TechnologyTreeType techType = technology.ExperienceToTechField(experienceType);
            if (techType != TechnologyTreeType.NUM_NONE)
            {
                addTechPoints(techType, gain, reason);
            }
        }
        public void addTechPoints(TechnologyTreeType techType, int gain, TechnologyGainReason reason)
        {
            ref var progress = ref TechnologyTemplate.GetResearchProgressRef(ref technology, techType);

            if (reason == TechnologyGainReason.WorkerLevel)
            {
                int buildingCount = researchBuildingCount(true, techType);
                progress.workerLevelUp(buildingCount, ref gain);
            }
            else
            {
                progress.points += gain;
            }

            onTechnologyGain(techType, gain, reason, progress);
        }

        int researchBuildingCount(bool isResearchCenter, TechnologyTreeType techType)
        {
            int buildingCount = 0;
            if (researchBuildings != null)
            {
                lock (researchBuildings)
                {
                    foreach (var m in researchBuildings)
                    {
                        if (m.isResearchCenter == isResearchCenter && m.assignedTech == techType)
                        {
                            buildingCount++;
                        }
                    }
                }
            }

            return buildingCount;
        }

        public void onTechnologyGain(TechnologyTreeType techType, int gain, TechnologyGainReason reason)
        {
            ResearchProgress progress = TechnologyTemplate.GetResearchProgressRef(ref technology, techType);

            onTechnologyGain(techType, gain, reason, progress);
        }

        void onTechnologyGain(TechnologyTreeType techType, int gain, TechnologyGainReason reason, ResearchProgress progress)
        {
            if (reason != TechnologyGainReason.BookPress)
            {
                int buildingCount = researchBuildingCount(false, techType);
                if (buildingCount > 0)
                {
                    //Spread 
                    var citiesC = GetFaction().cities.counter();
                    while (citiesC.Next())
                    {
                        if (citiesC.sel != this && citiesC.sel.researchBuildingCount(true, techType) > 0)
                        {
                            citiesC.sel.addTechPoints(techType, gain, TechnologyGainReason.BookPress);
                        }
                    }
                }
            }
        }

        public bool toggleSchoolStop(int index)
        {
            lock (schoolBuildings)
            {
                if (arraylib.InBound(schoolBuildings, index))
                {
                    var currentStatus = schoolBuildings[index];
                    currentStatus.que++;
                    if (currentStatus.que > 2)
                    {
                        currentStatus.que = 0;
                    }
                   
                    schoolBuildings[index] = currentStatus;
                    return currentStatus.que > 0;
                }
            }
            return false;
        }

        public void _commitResearch(LocalPlayer player)
        {
            commitResearch(player);
        }

        public bool commitResearch(LocalPlayer player)
        {
            lock (researchBuildings)
            {
                var building = researchBuildings[selectedResearchBuilding];
                if (building.assignedTech == TechnologyTreeType.NUM_NONE)
                {
                    building.assignedTech = player.selectedTech;
                    researchBuildings[selectedResearchBuilding] = building;
                    return true;
                }
            }
            return false;
        }
        //void onTechnologyGain(TechnologyTreeType techType, int gained, TechnologyGainReason reason)
        //{
        //    if (reason != TechnologyGainReason.BookPress)
        //    {

        //    }
        //}
    }
}
