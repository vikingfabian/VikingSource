using System;
using VikingEngine.DSSWars.Communication;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars
{
    partial class WorldData
    {
        public GroupedResource[] factionResourceOverviews = new GroupedResource[64 * CityResoureIndex.COUNT];
        public RelationSystem relationSystem = new RelationSystem(64);
        void init_FactionComponents()
        {
            factionResourceOverviews = new GroupedResource[factions.Array.Length * CityResoureIndex.COUNT];
            //diplomaticRelations = new DiplomaticRelation[MathExt.GaussSum(factions.Array.Length)];
            relationSystem = new RelationSystem(factions.Array.Length);

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
                //Array.Resize(ref diplomaticRelations, MathExt.GaussSum((factions.Array.Length -1) * 2));
                relationSystem = new RelationSystem((factions.Array.Length - 1) * 2);
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

    }
}
