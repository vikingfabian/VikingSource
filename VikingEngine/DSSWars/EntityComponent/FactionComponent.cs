using System;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars
{
    partial class WorldData
    {
        public GroupedResource[] factionResourceOverviews = new GroupedResource[64 * CityResoureIndex.COUNT];

        void init_FactionComponents()
        {
            factionResourceOverviews = new GroupedResource[factions.Array.Length * CityResoureIndex.COUNT];

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
            }
        }

        void initFaction(Faction faction)
        {
            faction.resourceComponentStartIndex = faction.myIndex * CityResoureIndex.COUNT;

            runList(City.MovableCityResource_Misc);
            runList(City.MovableCityResource_Metals);
            runList(City.MovableCityResource_WeaponMelee);
            runList(City.MovableCityResource_WeaponRanged);
            runList(City.MovableCityResource_Armor);

            void runList(ItemResourceType[] items)
            {
                foreach (ItemResourceType item in items)
                {
                    var properties = ItemPropertyColl.Get(item);
                    if (properties.cityResourceIndex >= 0)
                    {
                        ref GroupedResource resource = ref factionResourceOverviews[faction.resourceComponentStartIndex + properties.cityResourceIndex];
                        resource.goalBuffer = properties.defaultStockPile;
                    }
                }
            }
        }

        public void copyStockPile(City city, bool toCity, ResourceGroupType resourceGroup)
        {
            if (city.TryGetFaction(out var faction))
            {
                if (resourceGroup == ResourceGroupType.NUM)
                {
                    for (int i = 0; i < CityResoureIndex.COUNT; i++)
                    {
                        copy(i);
                    }
                }
                else
                {
                    ItemResourceType[] items = Resource.ResourceLib.ResourceGroupList(resourceGroup);
                    foreach (ItemResourceType item in items)
                    {
                        copy(ItemPropertyColl.Get(item).cityResourceIndex);
                    }
                }
                
                void copy(int cityResourceIndex)
                {
                    if (toCity)
                    {
                        cityResouces[city.resourceComponentStartIndex + cityResourceIndex].goalBuffer = factionResourceOverviews[faction.resourceComponentStartIndex + cityResourceIndex].goalBuffer;
                    }
                    else
                    {
                        factionResourceOverviews[faction.resourceComponentStartIndex + cityResourceIndex].goalBuffer = cityResouces[city.resourceComponentStartIndex + cityResourceIndex].goalBuffer;
                    }
                }
            }
        }

    }
}
