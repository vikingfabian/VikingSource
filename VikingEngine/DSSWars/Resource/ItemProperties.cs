using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
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
    //struct ItemSource
    //{

    //    public ItemSourceType source;
    //    public int sourceId;

    //    public ItemSource(TerrainSubFoilType terrain)
    //    {
    //        source = ItemSourceType.Terrain;
    //        sourceId = (int)terrain;
    //    }

    //    public ItemSource(TerrainMineType mineType)
    //    {
    //        source = ItemSourceType.Mine;
    //        sourceId = (int)mineType;
    //    }

    //    public ItemSource(ItemSourceType source, BuildAndExpandType buiding1)
    //    {
    //        this.source = source;
    //        sourceId = (int)buiding1;
    //    }

    //    public ItemSource(BuildAndExpandType buiding1)
    //    {
    //        this.source =  ItemSourceType.Crafting;
    //        sourceId = (int)buiding1;
    //    }

    //    public void ToHud(RichBoxContent content)
    //    {
    //        if (sourceId >= 0)
    //        {
    //            content.newLine();
    //            switch (source)
    //            {
    //                case ItemSourceType.Terrain:
    //                    label(DssRef.todoLang.ItemSource_Terrain);
    //                    terrain(sourceId);

    //                    void terrain(int terrainType)
    //                    {
    //                        if (terrainType >= 0)
    //                        {
    //                            IconName.Terrain(TerrainMainType.Foil, terrainType, out var icon, out var name);
    //                            content.Add(new RbImage(icon));
    //                            content.hspace();
    //                            content.Add(new RbText(name));
    //                        }
    //                    }

    //                    break;

    //                case ItemSourceType.Farm:
    //                    label(DssRef.todoLang.ItemSource_Farm);
    //                    addBuilding(sourceId);
    //                    break;

    //                case ItemSourceType.Crafting:

    //                    label(DssRef.todoLang.ItemSource_CraftStation);
    //                    addBuilding(sourceId);
    //                    break;

    //                case ItemSourceType.Mine:
    //                    label(DssRef.todoLang.ItemSource_Gathering);
    //                    IconName.Terrain(TerrainMainType.Mine, sourceId, out var icon, out var name);
    //                    content.Add(new RbImage(icon));
    //                    content.hspace();
    //                    content.Add(new RbText(name));
    //                    break;

    //            }

    //            void label(string typeName)
    //            {
    //                content.Add(new RbText(typeName + ":", HudLib.TitleColor_Label));
    //                content.space();
    //            }

    //            void addBuilding(int building)
    //            {
    //                IconName.Building((BuildAndExpandType)building, out var icon, out var name);
    //                content.Add(new RbImage(icon));
    //                content.hspace();
    //                content.Add(new RbText(TextLib.LargeFirstLetter(name)));
    //            }
    //        }
    //    }
    //}

    class ItemProperties
    {
        /// <summary>
        /// Weight is measured in man-carry, 1 is a standard carry weight for a worker
        /// </summary>
        public float weight;
        public WorkPriorityType work;
        public CraftBlueprint bp1;
        public CraftBlueprint bp2;
        public StorageType storageType;

        public SoldierData soldierData = new SoldierData();
        public bool Filter_IsSiegeWeapon = false;
        public bool Filter_IsTwoHandWeapon = true;
        public int cityResourceIndex;
        public int defaultStockPile = 100;

        public ItemSource itemSource1 = ItemSource.None, itemSource2 = ItemSource.None, itemSource3 = ItemSource.None;
        
        public ItemProperties(ItemResourceType type, int cityResourceIndex, float weight, WorkPriorityType work, 
            CraftBlueprint bp1, CraftBlueprint bp2, StorageType storageType)
        {   
            this.cityResourceIndex = cityResourceIndex;
            this.weight = weight;
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
        NUM
    }
}
