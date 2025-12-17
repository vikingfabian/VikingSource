using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
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
        public int cityResourceIndex;
        public int defaultStockPile = 100;

        public ItemSource itemSource1 = ItemSource.None, itemSource2 = ItemSource.None, itemSource3 = ItemSource.None;
        //public ItemSourceType source = ItemSourceType.NONE;
        //public int sourceId1 = -1, sourceId2 = -1, sourceId3 = -1;

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

        //public void SetItemSource(TerrainSubFoilType terrain)
        //{
        //    source = ItemSourceType.Terrain;
        //    sourceId1 = (int)terrain;
        //}

        //public void SetItemSource(TerrainMineType mineType)
        //{
        //    source = ItemSourceType.Mine;
        //    sourceId1 = (int)mineType;
        //}

        //public void SetItemSource(ItemSourceType source, BuildAndExpandType buiding1, BuildAndExpandType buiding2 = BuildAndExpandType.NUM_NONE, BuildAndExpandType buiding3 = BuildAndExpandType.NUM_NONE)
        //{
        //    this.source = source;
        //    sourceId1 = (int)buiding1;
        //    if (buiding2 != BuildAndExpandType.NUM_NONE)
        //    { 
        //        sourceId2 = (int)buiding2;
        //    }
        //    if (buiding3 != BuildAndExpandType.NUM_NONE)
        //    {
        //        sourceId3 = (int)buiding3;
        //    }
        //}
        
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

        //public void AddCraftSource(BuildAndExpandType buiding)
        //{
        //    source = ItemSourceType.Crafting;
        //    if (sourceId1 < 0)
        //    {
        //        sourceId1 = (int)buiding;
        //    }
        //    else if (sourceId2 < 0)
        //    {
        //        sourceId2 = (int)buiding;
        //    }
        //    else
        //    {
        //        throw new Exception("AddCraftSource");
        //    }
        //}

        public void ItemSourceToHud(RichBoxContent content)
        {
            itemSource1.ToHud(content);
            itemSource2.ToHud(content);
            itemSource3.ToHud(content);

            //switch (source) 
            //{
            //    case ItemSourceType.Terrain:
            //        label(".Terrain");
            //        terrain(sourceId1);
            //        terrain(sourceId2);

            //        void terrain(int terrainType)
            //        {
            //            if (terrainType >= 0)
            //            {
            //                IconName.Terrain(TerrainMainType.Foil, terrainType, out var icon, out var name);
            //                content.Add(new RbImage(icon));
            //                content.hspace();
            //                content.Add(new RbText(name));
            //            }
            //        }

            //        break;

            //    case ItemSourceType.Farm:
            //        label(".Farm");
            //        addBuilding(sourceId1);
            //        if (sourceId2 >= 0)
            //        {
            //            HudLib.BulletSeperationPoint(content);
            //            addBuilding(sourceId2);
            //        }
            //        if (sourceId3 >= 0)
            //        {
            //            HudLib.BulletSeperationPoint(content);
            //            addBuilding(sourceId3);
            //        }
            //        break;

            //    case ItemSourceType.Crafting:

            //        label(".Craft station");
            //        addBuilding(sourceId1);
            //        if (sourceId2 >= 0)
            //        {
            //            HudLib.BulletSeperationPoint(content);
            //            addBuilding(sourceId2);
            //        }
            //        break;

            //}

            //void label(string typeName)
            //{
            //    content.Add(new RbText(typeName + ":", HudLib.TitleColor_Label));
            //    content.space();
            //}

            //void addBuilding(int building)
            //{
            //    IconName.Building((BuildAndExpandType)building, out var icon, out var name);
            //    content.Add(new RbImage(icon));
            //    content.hspace();
            //    content.Add(new RbText(TextLib.LargeFirstLetter( name)));
            //}
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
