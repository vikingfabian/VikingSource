using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.DSSWars.Resource
{

    class ItemProperties
    {
        /// <summary>
        /// Weight is measured in man-carry, 1 is a standard carry weight for a worker
        /// </summary>
        public float weight;
        public int carryCount;
        public WorkPriorityType work;
        public CraftBlueprint bp1;
        public CraftBlueprint bp2;
        public StorageType storageType;
        
        public SoldierData soldierData = new SoldierData();
        public bool Filter_IsWarMachine = false;
        public bool Filter_IsTwoHandWeapon = true;
        public bool Filter_IsRidingAnimal = false;
        public WagonPull wagonPull = WagonPull.None;
        public ArmorCarry armorCarry = ArmorCarry.None;
        public int cityResourceIndex;

        public ItemSource itemSource1 = ItemSource.None, itemSource2 = ItemSource.None, itemSource3 = ItemSource.None;
        public CityBiome restrictedToBiom = CityBiome.NUM_NONE;
        
        public ItemProperties(ItemResourceType type, int cityResourceIndex, float weight, WorkPriorityType work, 
            CraftBlueprint bp1, CraftBlueprint bp2, StorageType storageType)
        {   
            this.cityResourceIndex = cityResourceIndex;
            this.weight = weight;
            if (weight == 0)
            {
                carryCount = 20;
            }
            else
            {
                this.carryCount = MathExt.DivideInt(1.0, weight);
            }
            this.work = work;
            this.bp1 = bp1;
            this.bp2 = bp2;
            this.storageType = storageType;

            ItemPropertyColl.items[(int)type] = this;
        }

        public void AddItemSource(ItemSource source)
        {
            if (itemSource1.sourceId < 0)
            {
                itemSource1 = source;
            }
            else if (itemSource2.sourceId < 0)
            {
                itemSource2 = source;
            }
            else if (itemSource3.sourceId < 0)
            {
                itemSource3 = source;
            }
        }

        public void AddItemSource(ItemSource source1, ItemSource source2)
        {
            AddItemSource(source1);
            AddItemSource(source2);
        }

        public void AddItemSource(ItemSource source1, ItemSource source2, ItemSource source3)
        {
            AddItemSource(source1);
            AddItemSource(source2);
            AddItemSource(source3);
        }

        public void ItemSourceToHud(RichBoxContent content)
        {
            itemSource1.ToHud(content);
            itemSource2.ToHud(content);
            itemSource3.ToHud(content);

        }
    }

    enum ItemSourceType
    { 
        NONE,
        Terrain,
        Mine,
        Farm,
        Crafting,
        AnamalHabitat,
        Building,
        NUM
    }

    enum ItemMainCategory
    { 
        Gold,
        Population,
        BaseResource,
        RefinedResource,
        Metal,
        Military,
        Animal,
    }

    enum WagonPull
    { 
        None,
        LightOnly,
        All,
        Balcon,
    }

    enum ArmorCarry
    { 
        None,
        LightOnly,
        All,
    }
}
