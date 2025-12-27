using System;
using VikingEngine.DSSWars.Communication;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;

namespace VikingEngine.DSSWars
{
    partial class WorldData
    {
        const int DefaultFactionCount = 64;
        public GroupedResource[] factionResourceOverviews = new GroupedResource[DefaultFactionCount * CityResoureIndex.COUNT];
        //public Di relationSystem = new RelationSystem(64);
       // public ResourceFactionOverview[] factionResourceOverviews = new ResourceFactionOverview[DefaultFactionCount * CityResoureIndex.COUNT];
        public WorkPriority[] factionWork = new WorkPriority[DefaultFactionCount * WorkTemplate.COUNT];

        void init_FactionComponents()
        {
            factionResourceOverviews = new GroupedResource[factions.Array.Length * CityResoureIndex.COUNT];
            factionWork = new WorkPriority[factions.Array.Length * WorkTemplate.COUNT];
            //diplomaticRelations = new DiplomaticRelation[MathExt.GaussSum(factions.Array.Length)];
            new Diplomacy(factions.Array.Length);

            for (int i = 0; i < factions.Array.Length; i++)
            {
                if (factions.Array[i] != null)
                { 
                    //factions.Array[i].resourceComponentStartIndex = i * CityResoureIndex.COUNT;
                    initFaction(factions.Array[i]);
                }
            }
        }

        public void factionComponentsAdd(Faction faction)
        {
            initFaction(faction);

            if (factions.Array.Length * CityResoureIndex.COUNT >= factionResourceOverviews.Length)
            {
                int startIndex = factionResourceOverviews.Length;
                Array.Resize(ref factionResourceOverviews, factionResourceOverviews.Length * 2);
                Array.Resize(ref factionWork, factionWork.Length * 2);
                //Array.Resize(ref diplomaticRelations, MathExt.GaussSum((factions.Array.Length -1) * 2));
                new Diplomacy((factions.Array.Length - 1) * 2);
            }
        }

        void initFaction(Faction faction)
        {
            faction.resourceComponentStartIndex = faction.myIndex * CityResoureIndex.COUNT;

            runList(ResourceLib.MovableCityResource_Misc);
            runList(ResourceLib.MovableCityResource_Metals);
            runList(ResourceLib.MovableCityResource_WeaponMelee);
            runList(ResourceLib.MovableCityResource_WeaponRanged);
            runList(ResourceLib.MovableCityResource_Armor);

            void runList(ItemResourceType[] items)
            {
                foreach (ItemResourceType item in items)
                {
                    var properties = ItemPropertyColl.Get(item);
                    if (properties.cityResourceIndex >= 0)
                    {
                        ref GroupedResource resource = ref factionResourceOverviews[faction.resourceComponentStartIndex + properties.cityResourceIndex];
                        resource.stockPileLimit = properties.defaultStockPile;
                    }
                }
            }

            faction.workTemplate.initComponents(false, factionWork, faction.myIndex * WorkTemplate.COUNT);
        }

    }
}
