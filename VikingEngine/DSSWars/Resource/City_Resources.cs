using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.GO.PickUp;
using VikingEngine.PJ.Joust;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        MinuteStats blackMarketCosts_food = new MinuteStats();
        
        public MinuteStats soldResources = new MinuteStats();

        public int maxWaterBase = DssConst.Maxwater;
        public int maxWaterTotal = DssConst.Maxwater;
        FloatingInt nextWater = new FloatingInt();
        public float waterAddPerSec;
        static readonly GroupedResource Res_Nothing = new GroupedResource() { amount = 100000 };

        
        public GroupedResource res_water = new GroupedResource();
        

        public bool res_food_safeguard = true;
        public int resourceComponentStartIndex;

        bool followFaction_Stockpile_Resources = true;
        bool followFaction_Stockpile_Metals = true;
        bool followFaction_Stockpile_Weapons = true;
        bool followFaction_Stockpile_Projectile = true;
        bool followFaction_Stockpile_Armor = true;

        public int resourceAmount(int cityResourceIndex)
        { 
            return DssRef.world.cityResouces[resourceComponentStartIndex + cityResourceIndex].amount;
        }

        public void resourceAmountSet(int cityResourceIndex, int amount)
        {
            ref var resource = ref DssRef.world.cityResouces[resourceComponentStartIndex + cityResourceIndex];
            resource.amount = amount;
        }

        public void resourceAmountSet_Minimum(int cityResourceIndex, int amount)
        {
            ref var resource = ref DssRef.world.cityResouces[resourceComponentStartIndex + cityResourceIndex];
            if (resource.amount < amount)
            {
                resource.amount = amount;
            }
        }

        override public bool lowFood()
        {
            return resourceAmount(CityResourceIndex.food) <= workForce.amount;//DssConst.WorkSafeGuardAmount;
        }


        public const int DefaultFoodBuffer = 500;
       
        public void defaultResourceBuffer(WorldData world)
        {
            
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
                        ref GroupedResource resource = ref world.cityResouces[resourceComponentStartIndex + properties.cityResourceIndex];
                        
                    }
                }
            }
        }
        public void AddGroupedResource(ItemResourceType type, int add)
        {
//#if DEBUG
//            if (type == ItemResourceType.ShortSword)
//            {
//                lib.DoNothing();
//            }
//#endif

            int itemIndex = ItemPropertyColl.CityIndex(type);

            if (itemIndex < 0) 
            {
                var faction = pfaction.GetFaction();

                if (faction == null)
                {
                    return;
                }

                switch (type)
                {
                    case ItemResourceType.Gold:
                    case ItemResourceType.CopperCoin:
                    case ItemResourceType.BronzeCoin:
                    case ItemResourceType.SilverCoin:
                    case ItemResourceType.ElfCoin:
                        faction.addGold(add, this);
                        return;

                    case ItemResourceType.ServiceMen:
                        freeServiceMen.amount += add;
                        return;

                    case ItemResourceType.Men:
                        workForce.amount += add;
                        return;

                    case ItemResourceType.ImmigrantsOrWorkers:
                        if (add > 0)
                        {
                            workForce.amount += add;
                            if (workForce.amount > HousingCount_Workers)
                            {
                                immigrants.value += workForce.amount - HousingCount_Workers;
                                workForce.amount = HousingCount_Workers;
                            }
                        }
                        else
                        {
                            int spendImmigrants = Math.Min(immigrants.Int(), -add);
                            immigrants.value -= spendImmigrants;
                            workForce.amount += add + spendImmigrants;
                        }
                        return;

                    case ItemResourceType.NobleMen:
                        freeNobelMen.amount += add;
                        return;

                    case ItemResourceType.Water_G:
                        res_water.amount += add;
                        return;

                    case ItemResourceType.NONE:
                        return;

                    default: throw new ArgumentOutOfRangeException("AddGroupedResource " + type.ToString());
                }
            }
            AddGroupedResource(itemIndex, add, false);
           
        }

        public StorageSize GetStorage(StorageType storageType)
        {
            return DssRef.world.cityStorage[StorageSize.COUNT * myIndex + (int)storageType];
        }

        public ref StorageSize GetRefStorage(StorageType storageType)
        {
            return ref DssRef.world.cityStorage[StorageSize.COUNT * myIndex + (int)storageType];
        }

        void refreshStorageSize(StorageType storageType)
        {
            DssRef.world.cityStorage[StorageSize.COUNT * myIndex + (int)storageType].refreshCapacity(this, storageType);
        }

        void addStorageBuilding(StorageType storageType, bool add)
        {
            DssRef.world.cityStorage[StorageSize.COUNT * myIndex + (int)storageType].addStorage(this, storageType, add);
        }

        void AddGroupedResource(WorldData world, int itemIndex, int add)
        {
#if DEBUG
            //if (factionIndex < 0)
            //{
            //    throw new Exception();
            //}
            if (resourceComponentStartIndex + itemIndex >= world.cityResouces.Length)
            {
                throw new Exception();
            }
            if (itemIndex + pfaction.factionIndex * CityResourceIndex.COUNT >= world.factionResourceOverviews.Length)
            {
                throw new Exception();
            }
#endif

            ref GroupedResource resource = ref world.cityResouces[resourceComponentStartIndex + itemIndex];
            resource.amount += add;
            
        }


        public void AddGroupedResource(int itemIndex, int add, bool respectLimit)
        {
            if (pfaction.factionIndex < 0)
            {
                return;
            }
#if DEBUG
            if (pfaction.factionIndex < 0)
            {
                throw new Exception();
            }
            if (resourceComponentStartIndex + itemIndex >= DssRef.world.cityResouces.Length)
            {
                throw new Exception();
            }
            if (itemIndex + pfaction.factionIndex * CityResourceIndex.COUNT >= DssRef.world.factionResourceOverviews.Length)
            {
                throw new Exception();
            }
#endif

            ref GroupedResource resource = ref DssRef.world.cityResouces[resourceComponentStartIndex + itemIndex];
            resource.amount += add;
            if (resource.amount >= resource.MaxLimit())
            {
                if (resource.hasCesspit)
                {
                    int remove = resource.amount - resource.MaxLimit() + 10;
                    resource.amount -= remove;
                    if (Ref.peRnd.ChanceF(DssConst.CessPitConvertToFuelPercentage))
                    {
                        AddGroupedResource(CityResourceIndex.fuel, remove, true);
                    }
                }
                //else
                //{
                //    int remove = resource.amount - resource.stockPileLimit;
                //    add -= remove;
                //    resource.amount = resource.stockPileLimit;
                //}
            }
            resource.changeRate.onChange(add);

        }

        public bool payResource(int itemIndex, int cost, bool allowNegative)
        {
            ref GroupedResource resource = ref DssRef.world.cityResouces[resourceComponentStartIndex + itemIndex];
            if (allowNegative || resource.amount >= cost)
            {
                resource.amount -= cost;
                return true;
            }

            return false;
        }

        public GroupedResource GetGroupedResource(int cityResourceIndex)
        {
            return DssRef.world.cityResouces[resourceComponentStartIndex + cityResourceIndex];
        }
        public ref GroupedResource GetRefGroupedResource(int cityResourceIndex)
        {
            return ref DssRef.world.cityResouces[resourceComponentStartIndex + cityResourceIndex];
        }
        public GroupedResource GetGroupedResource(ItemResourceType type)
        {
            int cityResourceIndex = ItemPropertyColl.CityIndex(type);

            if (cityResourceIndex < 0)
            {
                switch (type)
                {
                    case ItemResourceType.Gold:
                        int amount;
                        if (DssRef.storage.ruleset_instance.centralGold)
                        {
                            var faction = pfaction.GetFaction();
                            if (faction != null)
                            {
                                amount = faction.money.GetGold32();
                            }
                            else
                            {
                                amount = 0;
                            }
                        }
                        else
                        {
                            amount = money.GetGold32();
                        }

                        return new GroupedResource() { amount = amount, stockPileLimit = int.MaxValue };
                    case ItemResourceType.Men:
                        return workForce;
                    case ItemResourceType.NobleMen:
                        return freeNobelMen;
                    case ItemResourceType.ServiceMen:
                        return freeServiceMen;

                    case ItemResourceType.Water_G: return res_water;
                    case ItemResourceType.NONE: return Res_Nothing;

                }
            }

            return DssRef.world.cityResouces[resourceComponentStartIndex + cityResourceIndex];
        }

        public ref GroupedResource GetRefGroupedResource(ItemResourceType type)
        {
            int cityResourceIndex = ItemPropertyColl.CityIndex(type);
#if DEBUG
            if (cityResourceIndex < 0)
            {
                throw new NotImplementedException();
            }
#endif
            return ref DssRef.world.cityResouces[resourceComponentStartIndex + cityResourceIndex];
        }

        public void SetGroupedResource(ItemResourceType type, GroupedResource resource)
        {
            int itemIndex = ItemPropertyColl.CityIndex(type);
            if (itemIndex < 0)
            {
                return;
            }

            DssRef.world.cityResouces[resourceComponentStartIndex + itemIndex] = resource;
        }

        public void SetGroupedResource(ItemResourceType type, int amount)
        {
            int itemIndex = ItemPropertyColl.CityIndex(type);
            if (itemIndex < 0)
            {
                return;
            }

            DssRef.world.cityResouces[resourceComponentStartIndex + itemIndex].amount = amount;
        }


        public bool needMore(ItemResourceType type)
        {
            switch (type)
            {
                case ItemResourceType.RawFood_Group:
                case ItemResourceType.Wheat:
                case ItemResourceType.Egg:
                    return needMore(CityResourceIndex.rawFood);

                case ItemResourceType.Wood_Group:
                case ItemResourceType.DryWood:
                case ItemResourceType.SoftWood:
                case ItemResourceType.HardWood:
                    return needMore(CityResourceIndex.wood);

                case ItemResourceType.NONE:
                    return false;

                default:
                    return GetGroupedResource(type).needMore();
            }

            
        }
        public bool needMore(int cityResourceIndex)
        {
            return DssRef.world.cityResouces[resourceComponentStartIndex + cityResourceIndex].needMore();
        }

        public ItemResource MakeTrade(ItemResourceType itemResourceType, int payment, float maxWeight = 1f)
        {
            int carry = ItemPropertyColl.CarryAmount(itemResourceType, maxWeight);

            AddGroupedResource(itemResourceType, -carry);
            
            return new ItemResource(itemResourceType, 1, payment, carry);
        }

        public void dropOffItem(ItemResource item, out ItemResource convert1, out ItemResource convert2)
        {
            convert1 = item;
            convert2 = ItemResource.Empty;

            switch (item.type)
            {
                case ItemResourceType.DryWood:
                case ItemResourceType.SoftWood:
                case  ItemResourceType.HardWood:
                    convert1.type = ItemResourceType.Wood_Group;
                    break;
                
                case ItemResourceType.Coal:
                    convert1.type = ItemResourceType.Fuel_G;
                    break;

                case ItemResourceType.Wheat:
                    convert1.type = ItemResourceType.RawFood_Group;
                    //convert1.amount = DssConst.WheatFoodAmount;
                    break;

                case ItemResourceType.Egg:                                   
                //case ItemResourceType.Hen:
                    convert1.type = ItemResourceType.RawFood_Group;
                    convert1.amount = DssConst.HenRawFoodAmout;
                    //animalResourceBonus(ref item);
                    break;

                //case ItemResourceType.Pig:
                //    convert1.type = ItemResourceType.RawFood_Group;
                //    convert1.amount = DssConst.PigRawFoodAmout;
                //    //animalResourceBonus(ref item);

                //    convert2 = new ItemResource(ItemResourceType.SkinLinen_Group, 1, 1, DssConst.PigSkinAmount);
                //    break;

                case ItemResourceType.Linen:
                    convert1.type = ItemResourceType.SkinLinen_Group;
                    //convert1.amount = DssConst.LinenHarvestAmount;
                    break;

                case ItemResourceType.Rapeseed:
                    convert1.type = ItemResourceType.Fuel_G;
                    //convert1.amount = DssConst.RapeSeedFuelAmount;
                    break;

                case ItemResourceType.Hemp:
                    convert1.type = ItemResourceType.SkinLinen_Group;
                    //convert1.amount = DssConst.HempLinenAndFuelAmount;

                    convert2.type = ItemResourceType.Fuel_G;
                    convert2.amount = convert1.amount;//DssConst.HempLinenAndFuelAmount;
                    break;

                //case ItemResourceType.GoldOre:
                //    {
                //        var price = convert1.amount * DssConst.GoldOreSellValue;
                //        GetFaction().addGold( price, this);
                //        soldResources.add(price);

                //        convert1.type = ItemResourceType.Gold;
                //        convert1.amount = price;
                //    }
                //    break;
            }

            if (Ref.peRnd.Chance(DssRef.difficulty.resourceMultiplyChance) &&
                pfaction.GetPlayer().IsBot())
            {
                if (DssRef.difficulty.resourceMultiplyDecrease)
                {
                    return;
                }
                else
                {
                    convert1.amount *= 2;
                    convert2.amount *= 2;
                }
            }

            AddGroupedResource(convert1.type, convert1.amount);
            if (convert2.amount > 0)
            {
                AddGroupedResource(convert2.type, convert2.amount);
            }
        }

        //void animalResourceBonus(ref ItemResource item)
        //{
        //    if (Culture == CityCulture.AnimalBreeder)
        //    {
        //        item.amount *= 2;
        //    }
        //}
        
        public void tradeTab()
        { 
            
        }

        public void blackMarketPurchase(ItemResourceType resourceType, int count, Money cost)
        {
            var faction = pfaction.GetFaction();
            ref Money money = ref faction.GetRefMoney(this);
            if (money.pay(cost * count, false, faction.player))
            {
                AddGroupedResource(resourceType, count);
                faction.player.GetLocalPlayer()?.tutorial?.onBuyFromBlackMarket(resourceType);
            }
        }
    }   
        
}
