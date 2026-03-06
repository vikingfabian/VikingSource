using System;
using System.IO;
using VikingEngine.DSSWars.Communication;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
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
            if (factions.Array.Length * CityResoureIndex.COUNT >= factionResourceOverviews.Length)
            {
                int startIndex = factionResourceOverviews.Length;
                Array.Resize(ref factionResourceOverviews, factionResourceOverviews.Length * 2);
                Array.Resize(ref factionWork, factionWork.Length * 2);
                //Array.Resize(ref diplomaticRelations, MathExt.GaussSum((factions.Array.Length -1) * 2));
                new Diplomacy((factions.Array.Length - 1) * 2);
            }

            initFaction(faction);            
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

            int start = faction.myIndex * WorkTemplate.COUNT;
            int exEnd = start + WorkTemplate.COUNT;
            for (int i = start; i < exEnd; ++i)
            {
                factionWork[i] = new WorkPriority(0);
            }
            WorkTemplate.InitComponents(factionWork, start);
            //faction.workTemplate.initComponents(false, factionWork, faction.myIndex * WorkTemplate.COUNT);
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
                            cityResouces[city.resourceComponentStartIndex + cityResourceIndex].copyLimitFrom(factionResourceOverviews[faction.resourceComponentStartIndex + cityResourceIndex]);
                            break;
                        case CopyPasteOption.CityToFaction:
                            factionResourceOverviews[faction.resourceComponentStartIndex + cityResourceIndex].copyLimitFrom(cityResouces[city.resourceComponentStartIndex + cityResourceIndex]);
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
                                player.stockPileCopy[cityResourceIndex].copyLimitFrom(factionResourceOverviews[faction.resourceComponentStartIndex + cityResourceIndex]);
                            }
                            else
                            {
                                player.stockPileCopy[cityResourceIndex].copyLimitFrom(cityResouces[city.resourceComponentStartIndex + cityResourceIndex]);
                            }
                            break;

                        case CopyPasteOption.FromMemory:
                            if (player.stockPileCopy != null)
                            {
                                if (city == null)
                                {
                                    factionResourceOverviews[faction.resourceComponentStartIndex + cityResourceIndex].copyLimitFrom(player.stockPileCopy[cityResourceIndex]);
                                }
                                else
                                {
                                    cityResouces[city.resourceComponentStartIndex + cityResourceIndex].copyLimitFrom(player.stockPileCopy[cityResourceIndex]);
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
