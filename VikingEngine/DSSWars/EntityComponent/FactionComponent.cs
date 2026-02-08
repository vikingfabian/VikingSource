using System;
using System.IO;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
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
            if (factions.Array.Length * CityResoureIndex.COUNT >= factionResourceOverviews.Length)
            {
                int startIndex = factionResourceOverviews.Length;
                Array.Resize(ref factionResourceOverviews, factionResourceOverviews.Length * 2);
            }

            initFaction(faction);            
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
                        resource.stockPileLimit = properties.defaultStockPile;
                    }
                }
            }
        }

        public void writeStockPile(BinaryWriter w, Faction faction)
        {
            for (int i = 0; i < CityResoureIndex.COUNT; i++)
            {
                factionResourceOverviews[faction.resourceComponentStartIndex + i].writeStockPile(w);
            }
        }
        public void readStockPile(BinaryReader r, int subVersion, Faction faction)
        {
            for (int i = 0; i < CityResoureIndex.COUNT; i++)
            {
                factionResourceOverviews[faction.resourceComponentStartIndex + i].readStockPile(r, subVersion);
            }
        }

        public void copyStockPile(LocalPlayer player, Faction faction, City city, CopyPasteOption copyPaste, ResourceGroupType resourceGroup)
        {
            if (faction != null)
            {
                if (copyPaste == CopyPasteOption.ToAllCities)
                {
                    SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                    while (citiesC.Next(ref faction.cities, DssRef.world.cities, out City _city))
                    {
                        copyStockPile(player, faction, _city, CopyPasteOption.FactionToCity, resourceGroup);
                    }
                    return;
                }


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
                    switch (copyPaste)
                    {
                        case CopyPasteOption.FactionToCity:
                            cityResouces[city.resourceComponentStartIndex + cityResourceIndex].stockPileLimit = factionResourceOverviews[faction.resourceComponentStartIndex + cityResourceIndex].stockPileLimit;
                            break;
                        case CopyPasteOption.CityToFaction:
                            factionResourceOverviews[faction.resourceComponentStartIndex + cityResourceIndex].stockPileLimit = cityResouces[city.resourceComponentStartIndex + cityResourceIndex].stockPileLimit;
                            break;

                            //if (toCity)
                            //{
                            //    cityResouces[city.resourceComponentStartIndex + cityResourceIndex].goalBuffer = factionResourceOverviews[faction.resourceComponentStartIndex + cityResourceIndex].goalBuffer;
                            //}
                            //else
                            //{
                            //    factionResourceOverviews[faction.resourceComponentStartIndex + cityResourceIndex].goalBuffer = cityResouces[city.resourceComponentStartIndex + cityResourceIndex].goalBuffer;
                            //}
                            //break;

                        case CopyPasteOption.ToMemory:
                            if (player.stockPileCopy == null)
                            {
                                player.stockPileCopy = new GroupedResource[CityResoureIndex.COUNT];
                            }

                            if (city == null)
                            {
                                player.stockPileCopy[cityResourceIndex].stockPileLimit = factionResourceOverviews[faction.resourceComponentStartIndex + cityResourceIndex].stockPileLimit;
                            }
                            else
                            {
                                player.stockPileCopy[cityResourceIndex].stockPileLimit = cityResouces[city.resourceComponentStartIndex + cityResourceIndex].stockPileLimit;
                            }
                            break;

                        case CopyPasteOption.FromMemory:
                            if (player.stockPileCopy != null)
                            {
                                if (city == null)
                                {
                                    factionResourceOverviews[faction.resourceComponentStartIndex + cityResourceIndex].stockPileLimit= player.stockPileCopy[cityResourceIndex].stockPileLimit;
                                }
                                else
                                {
                                    cityResouces[city.resourceComponentStartIndex + cityResourceIndex].stockPileLimit= player.stockPileCopy[cityResourceIndex].stockPileLimit;
                                }
                            }
                            break;

                    }

                }
            }
        }

    }

    enum CopyPasteOption
    { 
        None,
        ToMemory,
        FromMemory,
        FactionToCity,
        CityToFaction,
        ToAllCities,
    }
}
