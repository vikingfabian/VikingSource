
using Microsoft.Xna.Framework;
using Sentry;
using System;
using System.Collections.Generic;
using VikingEngine;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.Gadgets;


namespace VikingEngine.DSSWars.Work
{
    struct WorkTemplate
    {
        public const byte NoPrio = 0;
        public const byte MinPrio = 1;
        public const byte MaxPrio = 5;
        public const int SafeGuardPrio = MaxPrio + 1;

        //public WorkPriority move = new WorkPriority(3);
        //public WorkPriority wood = new WorkPriority(2);
        //public WorkPriority stone = new WorkPriority(2);
        //public WorkPriority craft_fuel = new WorkPriority(1);
        //public WorkPriority craft_food = new WorkPriority(0);
        //public WorkPriority craft_beer = new WorkPriority(0);
        //public WorkPriority craft_coolingfluid = new WorkPriority(0);
        public static readonly int COUNT = (int)WorkPriorityType.NUM_NONE;

        //public WorkPriority move = new WorkPriority(3);
        //public WorkPriority wood = new WorkPriority(2);
        //public WorkPriority stone = new WorkPriority(2);
        //public WorkPriority craft_fuel = new WorkPriority(4);
        //public WorkPriority craft_food = new WorkPriority(4);
        //public WorkPriority craft_beer = new WorkPriority(1);

        //public WorkPriority craft_iron = new WorkPriority(3);

        //public WorkPriority craft_sharpstick = new WorkPriority(1);

        //public WorkPriority craft_paddedarmor = new WorkPriority(1);        

        //public WorkPriority farm_food = new WorkPriority(2);
        //public WorkPriority farm_fuel = new WorkPriority(2);
        //public WorkPriority farm_linen = new WorkPriority(1);
        //public WorkPriority bogiron = new WorkPriority(1);
        //public WorkPriority mining_iron = new WorkPriority(3);
        //public WorkPriority trading = new WorkPriority(2);
        //public WorkPriority autoBuild = new WorkPriority(1);
        //public WorkPriority buildOrder = new WorkPriority(2);
        //public WorkPriority smeltgold = new WorkPriority(3);


        //public bool coinmaker_copper_fullStock = true;
        //public bool coinmaker_bronze_fullStock = true;
        //public bool coinmaker_silver_fullStock = true;
        //public bool coinmaker_mithril_fullStock = true;

        bool isCity;
        public int workComponentStartIndex;
        //public void initComponents(bool isCity, WorkPriority[] work, int startIndex)
        //{
        //    this.isCity = isCity;
        //    workComponentStartIndex = startIndex;
        //    //int exEnd = workComponentStartIndex + COUNT;

        //    //for (int i = workComponentStartIndex; i < exEnd; i++)
        //    //{
        //    //     work[i] = new WorkPriority(0);
        //    //}

        //    //work[startIndex + (int)WorkPriorityType.move].value = 3;
        //    //work[startIndex + (int)WorkPriorityType.wood].value = 2;
        //    //work[startIndex + (int)WorkPriorityType.stone].value = 2;
        //    //work[startIndex + (int)WorkPriorityType.craftFuel].value = 1;
        //    //work[startIndex + (int)WorkPriorityType.farmFood].value = 4;
        //    //work[startIndex + (int)WorkPriorityType.farmRawFood].value = 1;
        //    //work[startIndex + (int)WorkPriorityType.craftBeer].value = 1;

        //    //work[startIndex + (int)WorkPriorityType.smeltIron].value = 3;
        //    //work[startIndex + (int)WorkPriorityType.craftSharpStick].value = 1;
        //    //work[startIndex + (int)WorkPriorityType.craftPaddedArmor].value = 1;
        //    //work[startIndex + (int)WorkPriorityType.farmfuel].value = 2;
        //    //work[startIndex + (int)WorkPriorityType.farmlinen].value = 1;
        //    //work[startIndex + (int)WorkPriorityType.bogiron].value = 1;
        //    //work[startIndex + (int)WorkPriorityType.miningIron].value = 3;
        //    //work[startIndex + (int)WorkPriorityType.trading].value = 2;
        //    //work[startIndex + (int)WorkPriorityType.autoBuild].value = 1;
        //    //work[startIndex + (int)WorkPriorityType.buildOrders].value = 2;
        //    //work[startIndex + (int)WorkPriorityType.smeltGold].value = 3;
        //}
        public static void InitComponents(WorkPriority[] work, int startIndex)
        {
            work[startIndex + (int)WorkPriorityType.move].value = 3;
            work[startIndex + (int)WorkPriorityType.wood].value = 2;
            work[startIndex + (int)WorkPriorityType.stone].value = 2;
            work[startIndex + (int)WorkPriorityType.collectClay].value = 1;
            work[startIndex + (int)WorkPriorityType.craftBrick].value = 1;
            work[startIndex + (int)WorkPriorityType.craftFuel].value = 1;
            work[startIndex + (int)WorkPriorityType.farmFood].value = 4;
            work[startIndex + (int)WorkPriorityType.farmRawFood].value = 1;
            work[startIndex + (int)WorkPriorityType.craftBeer].value = 1;

            work[startIndex + (int)WorkPriorityType.smeltIron].value = 3;
            work[startIndex + (int)WorkPriorityType.craftSharpStick].value = 1;
            work[startIndex + (int)WorkPriorityType.craftPaddedArmor].value = 1;
            work[startIndex + (int)WorkPriorityType.farmfuel].value = 2;
            work[startIndex + (int)WorkPriorityType.farmlinen].value = 1;
            work[startIndex + (int)WorkPriorityType.bogiron].value = 1;
            work[startIndex + (int)WorkPriorityType.miningIron].value = 3;
            work[startIndex + (int)WorkPriorityType.trading].value = 2;
            work[startIndex + (int)WorkPriorityType.autoBuild].value = 1;
            work[startIndex + (int)WorkPriorityType.buildOrders].value = 2;
            work[startIndex + (int)WorkPriorityType.smeltGold].value = 3;
        }
        WorkPriority[] Work()
        {
            return isCity ? DssRef.world.cityWork : DssRef.world.factionWork;
        }

        public void applyUnlock(Unlocks unlocks)
        {
            //new
            WorkPriority[] work = Work();

            //for (int i = 0; i < WorkTemplate.COUNT; ++i)
            //{ 
            //    work[
            //}

            // Coinage
            work[workComponentStartIndex + (int)WorkPriorityType.coinmaker_copper].unlocked = unlocks.coinMaking;
            work[workComponentStartIndex + (int)WorkPriorityType.coinmaker_bronze].unlocked = unlocks.coinMaking;
            work[workComponentStartIndex + (int)WorkPriorityType.coinmaker_silver].unlocked = unlocks.coinMaking;
            work[workComponentStartIndex + (int)WorkPriorityType.coinmaker_mithril].unlocked = unlocks.coinMaking;

            // Tools
            work[workComponentStartIndex + (int)WorkPriorityType.craftToolkit].unlocked = unlocks.item_tools;

            // Advanced Metals
            work[workComponentStartIndex + (int)WorkPriorityType.craftCastIron].unlocked = unlocks.item_castIron;
            work[workComponentStartIndex + (int)WorkPriorityType.craftMithril].unlocked = unlocks.item_castMithril;

            // Iron Tier
            work[workComponentStartIndex + (int)WorkPriorityType.smeltIron].unlocked = unlocks.item_Iron; // Mapped craft_iron to smeltIron
            work[workComponentStartIndex + (int)WorkPriorityType.craftShortSword].unlocked = unlocks.item_Sword;
            work[workComponentStartIndex + (int)WorkPriorityType.craftSword].unlocked = unlocks.item_Sword;
            work[workComponentStartIndex + (int)WorkPriorityType.craftMailArmor].unlocked = unlocks.item_IronArmor;
            work[workComponentStartIndex + (int)WorkPriorityType.craftHeavyMailArmor].unlocked = unlocks.item_IronArmor;
            work[workComponentStartIndex + (int)WorkPriorityType.craftWarhammer].unlocked = unlocks.item_Sword;

            // work[resourceComponentStartIndex + (int)WorkPriorityType.craftKnightslance].unlocked = unlocks.item_Sword; // TODO: Add 'craftKnightslance' to Enum

            // Steel Tier
            work[workComponentStartIndex + (int)WorkPriorityType.craftBloomeryIron].unlocked = unlocks.item_Steel;
            work[workComponentStartIndex + (int)WorkPriorityType.craftSteel].unlocked = unlocks.item_Steel;
            work[workComponentStartIndex + (int)WorkPriorityType.craftLongSword].unlocked = unlocks.item_LongSword;
            work[workComponentStartIndex + (int)WorkPriorityType.craftTwoHandSword].unlocked = unlocks.item_LongSword;
            work[workComponentStartIndex + (int)WorkPriorityType.craftPlateArmor].unlocked = unlocks.item_SteelArmor;
            work[workComponentStartIndex + (int)WorkPriorityType.craftFullPlateArmor].unlocked = unlocks.item_SteelArmor;

            // Siege Engines
            work[workComponentStartIndex + (int)WorkPriorityType.craftCatapult].unlocked = unlocks.item_catapult;
            work[workComponentStartIndex + (int)WorkPriorityType.craftManuBallista].unlocked = unlocks.item_catapult;
            work[workComponentStartIndex + (int)WorkPriorityType.craftCrossbow].unlocked = unlocks.item_crossbow;

            // Black Powder
            work[workComponentStartIndex + (int)WorkPriorityType.craftBlackPowder].unlocked = unlocks.item_blackPowder;
            work[workComponentStartIndex + (int)WorkPriorityType.craftBullet].unlocked = unlocks.item_blackPowder;
            work[workComponentStartIndex + (int)WorkPriorityType.craftHandCannon].unlocked = unlocks.item_blackPowder;
            work[workComponentStartIndex + (int)WorkPriorityType.craftHandCulverin].unlocked = unlocks.item_blackPowder;

            // Gunpowder
            work[workComponentStartIndex + (int)WorkPriorityType.craftGunPowder].unlocked = unlocks.item_gunPowder;
            work[workComponentStartIndex + (int)WorkPriorityType.craftRifle].unlocked = unlocks.item_gunPowder;
            work[workComponentStartIndex + (int)WorkPriorityType.craftBlunderbuss].unlocked = unlocks.item_gunPowder;

            // Cannons
            work[workComponentStartIndex + (int)WorkPriorityType.craftSiegeCannonBronze].unlocked = unlocks.item_cannon;
            work[workComponentStartIndex + (int)WorkPriorityType.craftManCannonBronze].unlocked = unlocks.item_cannon;
            work[workComponentStartIndex + (int)WorkPriorityType.craftSiegeCannonIron].unlocked = unlocks.item_cannon;
            work[workComponentStartIndex + (int)WorkPriorityType.craftManCannonIron].unlocked = unlocks.item_cannon;
        }

        public WorkTemplate(bool isCity, int index)
        {
            this.isCity = isCity;
            workComponentStartIndex = index * COUNT;
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            WorkPriority[] work = Work();
            int exEnd = workComponentStartIndex + COUNT;

            for (int i = workComponentStartIndex; i < exEnd; i++)
            {
                work[i].writeGameState(w, isCity);
            }
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, bool isCity)
        {   
            this.isCity = isCity;

            WorkPriority[] work = Work();
            int exEnd = workComponentStartIndex + COUNT;

            for (int i = workComponentStartIndex; i < exEnd; i++)
            {
                work[i].readGameState(r, subversion, isCity);
            }
        }

        public void setAllToFollowFactionAndUpdate(City city, WorkTemplate factionTemplate)
        {
            setAllToFollowFaction();
            onFactionChange(city, factionTemplate);
        }

        public void onFactionChange(City city, WorkTemplate factionTemplate)
        {
            for (int i = 0; i < COUNT; i++)
            {
                var factionValue = DssRef.world.factionWork[factionTemplate.workComponentStartIndex + i];
                DssRef.world.cityWork[workComponentStartIndex + i].onFactionValueChange(factionValue);
            }

            //move.onFactionValueChange(factionTemplate.move);
            //wood.onFactionValueChange(factionTemplate.wood);
            //stone.onFactionValueChange(factionTemplate.stone);
            //craft_fuel.onFactionValueChange(factionTemplate.craft_fuel);
            //craft_food.onFactionValueChange(factionTemplate.craft_food);
            //craft_beer.onFactionValueChange(factionTemplate.craft_beer);
            //craft_coolingfluid.onFactionValueChange(factionTemplate.craft_coolingfluid);

            //craft_iron.onFactionValueChange(factionTemplate.craft_iron);
            //craft_tin.onFactionValueChange(factionTemplate.craft_tin);
            //craft_cupper.onFactionValueChange(factionTemplate.craft_cupper);
            //craft_lead.onFactionValueChange(factionTemplate.craft_lead);
            //craft_silver.onFactionValueChange(factionTemplate.craft_silver);

            //craft_bronze.onFactionValueChange(factionTemplate.craft_bronze);
            //craft_castiron.onFactionValueChange(factionTemplate.craft_castiron);
            //craft_bloomeryiron.onFactionValueChange(factionTemplate.craft_bloomeryiron);
            //craft_steel.onFactionValueChange(factionTemplate.craft_steel);
            //craft_mithril.onFactionValueChange(factionTemplate.craft_mithril);

            //craft_palisade.onFactionValueChange(factionTemplate.craft_palisade);
            //craft_toolkit.onFactionValueChange(factionTemplate.craft_toolkit);
            //craft_wagonlight.onFactionValueChange(factionTemplate.craft_wagonlight);
            //craft_wagonheavy.onFactionValueChange(factionTemplate.craft_wagonheavy);
            //craft_blackpowder.onFactionValueChange(factionTemplate.craft_blackpowder);
            //craft_gunpowder.onFactionValueChange(factionTemplate.craft_gunpowder);
            //craft_bullet.onFactionValueChange(factionTemplate.craft_bullet);

            //craft_sharpstick.onFactionValueChange(factionTemplate.craft_sharpstick);
            //craft_bronzesword.onFactionValueChange(factionTemplate.craft_bronzesword);
            //craft_shortsword.onFactionValueChange(factionTemplate.craft_shortsword);
            //craft_sword.onFactionValueChange(factionTemplate.craft_sword);
            //craft_longsword.onFactionValueChange(factionTemplate.craft_longsword);
            //craft_handspear.onFactionValueChange(factionTemplate.craft_handspear);
            //craft_mithrilsword.onFactionValueChange(factionTemplate.craft_mithrilsword);
            //craft_warhammer.onFactionValueChange(factionTemplate.craft_warhammer);
            //craft_twohandsword.onFactionValueChange(factionTemplate.craft_twohandsword);
            //craft_knightslance.onFactionValueChange(factionTemplate.craft_knightslance);

            //craft_slingshot.onFactionValueChange(factionTemplate.craft_slingshot);
            //craft_throwingspear.onFactionValueChange(factionTemplate.craft_throwingspear);
            //craft_bow.onFactionValueChange(factionTemplate.craft_bow);
            //craft_longbow.onFactionValueChange(factionTemplate.craft_longbow);
            //craft_crossbow.onFactionValueChange(factionTemplate.craft_crossbow);
            //craft_mithrilbow.onFactionValueChange(factionTemplate.craft_mithrilbow);

            //craft_handcannon.onFactionValueChange(factionTemplate.craft_handcannon);
            //craft_handculverin.onFactionValueChange(factionTemplate.craft_handculverin);
            //craft_rifle.onFactionValueChange(factionTemplate.craft_rifle);
            //craft_blunderbus.onFactionValueChange(factionTemplate.craft_blunderbus);

            //craft_ballista.onFactionValueChange(factionTemplate.craft_ballista);
            //craft_manuballista.onFactionValueChange(factionTemplate.craft_manuballista);
            //craft_catapult.onFactionValueChange(factionTemplate.craft_catapult);
            //craft_batteringram.onFactionValueChange(factionTemplate.craft_batteringram);

            //craft_siegecannonbronze.onFactionValueChange(factionTemplate.craft_siegecannonbronze);
            //craft_mancannonbronze.onFactionValueChange(factionTemplate.craft_mancannonbronze);
            //craft_siegecannoniron.onFactionValueChange(factionTemplate.craft_siegecannoniron);
            //craft_mancannoniron.onFactionValueChange(factionTemplate.craft_mancannoniron);

            //craft_paddedarmor.onFactionValueChange(factionTemplate.craft_paddedarmor);
            //craft_heavypaddedarmor.onFactionValueChange(factionTemplate.craft_heavypaddedarmor);
            //craft_bronzearmor.onFactionValueChange(factionTemplate.craft_bronzearmor);
            //craft_mailarmor.onFactionValueChange(factionTemplate.craft_mailarmor);
            //craft_heavymailarmor.onFactionValueChange(factionTemplate.craft_heavymailarmor);
            //craft_platearmor.onFactionValueChange(factionTemplate.craft_platearmor);
            //craft_fullplatearmor.onFactionValueChange(factionTemplate.craft_fullplatearmor);
            //craft_mithrilarmor.onFactionValueChange(factionTemplate.craft_mithrilarmor);

            //farm_food.onFactionValueChange(factionTemplate.farm_food);
            //farm_fuel.onFactionValueChange(factionTemplate.farm_fuel);
            //farm_linen.onFactionValueChange(factionTemplate.farm_linen);
            //bogiron.onFactionValueChange(factionTemplate.bogiron);
            //mining_iron.onFactionValueChange(factionTemplate.mining_iron);
            //mining_tin.onFactionValueChange(factionTemplate.mining_tin);
            //mining_copper.onFactionValueChange(factionTemplate.mining_copper);
            //mining_lead.onFactionValueChange(factionTemplate.mining_lead);
            //mining_silver.onFactionValueChange(factionTemplate.mining_silver);
            //mining_gold.onFactionValueChange(factionTemplate.mining_gold);
            //mining_mithril.onFactionValueChange(factionTemplate.mining_mithril);
            //mining_sulfur.onFactionValueChange(factionTemplate.mining_sulfur);
            //mining_coal.onFactionValueChange(factionTemplate.mining_coal);

            //trading.onFactionValueChange(factionTemplate.trading);
            //autoBuild.onFactionValueChange(factionTemplate.autoBuild);

            //if (buildOrder.followFaction && buildOrder.value != factionTemplate.buildOrder.value)
            //{
            //    buildOrder.value = Bound.Max(factionTemplate.buildOrder.value, city.MaxBuildPrio());
            //}

            //smeltgold.onFactionValueChange(factionTemplate.smeltgold);
            //coinmaker_copper.onFactionValueChange(factionTemplate.coinmaker_copper);
            //coinmaker_bronze.onFactionValueChange(factionTemplate.coinmaker_bronze);
            //coinmaker_silver.onFactionValueChange(factionTemplate.coinmaker_silver);
            //coinmaker_mithril.onFactionValueChange(factionTemplate.coinmaker_mithril);
        }

        public void setAllToFollowFaction()
        {
            WorkPriority[] work = Work();
            int exEnd = workComponentStartIndex + COUNT;

            for (int i = workComponentStartIndex; i < exEnd; i++)
            {
                work[i].followFaction = true;
            }
        }

        public void setWorkPrio(WorkPriorityType priorityType, byte set)
        {
            ref var work = ref GetRefWorkPriority(priorityType);
            work.value = set;
            work.followFaction = false;
        }

        public void setWorkPrio(WorkPriorityType priorityType, byte set, bool fullStock)
        {
            ref var work = ref GetRefWorkPriority(priorityType);
            work.value = set;
            work.followFaction = false;
            work.waitForStockpile = fullStock;
        }
        //public void setWorkPrioSafeGuard(bool set, WorkPriorityType priorityType)
        //{
        //    var work = GetWorkPriority(priorityType);
        //    work.safeguard = set;//Bound.Set(work.value + set, NoPrio, MaxPrio);
        //    work.followFaction = false;
        //    SetWorkPriority(priorityType, work);
        //}
        public void followFactionClick(WorkPriorityType priorityType, WorkTemplate factionTemplate)
        {
            ref var work = ref GetRefWorkPriority(priorityType);
            work.followFaction = !work.followFaction;
            work.onFactionValueChange(factionTemplate.Get(priorityType));
            //SetWorkPriority(prioType, work);
        }

        //public WorkPriority GetWorkPriorityAndStockCheck(ItemResourceType item, out bool waitForFullStock)
        //{
        //    switch (item)
        //    {
        //        case ItemResourceType.CopperCoin:
        //            waitForFullStock = coinmaker_copper_fullStock;
        //            return coinmaker_copper;

        //        case ItemResourceType.BronzeCoin:
        //            waitForFullStock = coinmaker_bronze_fullStock;
        //            return coinmaker_bronze;

        //        case ItemResourceType.SilverCoin:
        //            waitForFullStock = coinmaker_silver_fullStock;
        //            return coinmaker_silver;

        //        case ItemResourceType.ElfCoin:
        //            waitForFullStock = coinmaker_mithril_fullStock;
        //            return coinmaker_mithril;


        //        default:
        //            throw new NotImplementedException();
        //    }
        //}



        public WorkPriority GetWorkPriority(ItemResourceType item, out bool hasPriority)
        {
            var priorityType = ItemPropertyColl.Get(item).work;
            if (priorityType == WorkPriorityType.NUM_NONE)
            {
                hasPriority = false;
                return WorkPriority.Empty;
            }
            else
            {
                hasPriority = true;
                return Work()[workComponentStartIndex + (int)priorityType];
            }
            //hasPriority = true;
            //switch (item)
            //{
            //    case ItemResourceType.Fuel_G:
            //        return craft_fuel;
            //    case ItemResourceType.Food_G:
            //        return craft_food;
            //    case ItemResourceType.Beer:
            //        return craft_beer;
            //    case ItemResourceType.CoolingFluid:
            //        return craft_coolingfluid;

            //    case ItemResourceType.Iron_G:
            //        return craft_iron;
            //    case ItemResourceType.Tin:
            //        return craft_tin;
            //    case ItemResourceType.Copper:
            //        return craft_cupper;
            //    case ItemResourceType.Lead:
            //        return craft_lead;
            //    case ItemResourceType.Silver:
            //        return craft_silver;

            //    case ItemResourceType.Bronze:
            //        return craft_bronze;
            //    case ItemResourceType.CastIron:
            //        return craft_castiron;
            //    case ItemResourceType.BloomeryIron:
            //        return craft_bloomeryiron;
            //    case ItemResourceType.Steel:
            //        return craft_steel;
            //    case ItemResourceType.Mithril:
            //        return craft_mithril;

            //    case ItemResourceType.Palisade:
            //        return craft_palisade;
            //    case ItemResourceType.Toolkit:
            //        return craft_toolkit;
            //    case ItemResourceType.Wagon2Wheel:
            //        return craft_wagonlight;
            //    case ItemResourceType.Wagon4Wheel:
            //        return craft_wagonheavy;
            //    case ItemResourceType.BlackPowder:
            //        return craft_blackpowder;
            //    case ItemResourceType.GunPowder:
            //        return craft_gunpowder;
            //    case ItemResourceType.LedBullet:
            //        return craft_bullet;

            //    case ItemResourceType.PaddedArmor:
            //        return craft_paddedarmor;
            //    case ItemResourceType.HeavyPaddedArmor:
            //        return craft_heavypaddedarmor;
            //    case ItemResourceType.BronzeArmor:
            //        return craft_bronzearmor;
            //    case ItemResourceType.IronArmor:
            //        return craft_mailarmor;
            //    case ItemResourceType.HeavyIronArmor:
            //        return craft_heavymailarmor;
            //    case ItemResourceType.LightPlateArmor:
            //        return craft_platearmor;
            //    case ItemResourceType.FullPlateArmor:
            //        return craft_fullplatearmor;
            //    case ItemResourceType.MithrilArmor:
            //        return craft_mithrilarmor;

            //    case ItemResourceType.SharpStick:
            //        return craft_sharpstick;
            //    case ItemResourceType.BronzeSword:
            //        return craft_bronzesword;
            //    case ItemResourceType.ShortSword:
            //        return craft_shortsword;
            //    case ItemResourceType.Sword:
            //        return craft_sword;
            //    case ItemResourceType.LongSword:
            //        return craft_longsword;
            //    case ItemResourceType.HandSpear:
            //        return craft_handspear;
            //    case ItemResourceType.MithrilSword:
            //        return craft_mithrilsword;
            //    case ItemResourceType.Warhammer:
            //        return craft_warhammer;
            //    case ItemResourceType.TwoHandSword:
            //        return craft_twohandsword;
            //    //case ItemResourceType.KnightsLance:
            //    //    return craft_knightslance;

            //    case ItemResourceType.SlingShot:
            //        return craft_slingshot;
            //    case ItemResourceType.ThrowingSpear:
            //        return craft_throwingspear;
            //    case ItemResourceType.Bow:
            //        return craft_bow;
            //    case ItemResourceType.LongBow:
            //        return craft_longbow;
            //    case ItemResourceType.Crossbow:
            //        return craft_crossbow;
            //    case ItemResourceType.MithrilBow:
            //        return craft_mithrilbow;

            //    case ItemResourceType.HandCannon:
            //        return craft_handcannon;
            //    case ItemResourceType.HandCulverin:
            //        return craft_handculverin;
            //    case ItemResourceType.Rifle:
            //        return craft_rifle;
            //    case ItemResourceType.Blunderbuss:
            //        return craft_blunderbus;

            //    case ItemResourceType.Ballista:
            //        return craft_ballista;
            //    case ItemResourceType.Manuballista:
            //        return craft_manuballista;
            //    case ItemResourceType.Catapult:
            //        return craft_catapult;
            //    case ItemResourceType.UN_BatteringRam:
            //        return craft_batteringram;
            //    case ItemResourceType.SiegeCannonBronze:
            //        return craft_siegecannonbronze;
            //    case ItemResourceType.ManCannonBronze:
            //        return craft_mancannonbronze;
            //    case ItemResourceType.SiegeCannonIron:
            //        return craft_siegecannoniron;
            //    case ItemResourceType.ManCannonIron:
            //        return craft_mancannoniron;

            //    case ItemResourceType.Gold:
            //        return smeltgold;

            //    case ItemResourceType.CopperCoin:
            //        return coinmaker_copper;
            //    case ItemResourceType.BronzeCoin:
            //        return coinmaker_bronze;
            //    case ItemResourceType.SilverCoin:
            //        return coinmaker_silver;
            //    case ItemResourceType.ElfCoin:
            //        return coinmaker_mithril;


            //    default:
            //        hasPriority = false;
            //        return WorkPriority.Empty;
            //        //throw new NotImplementedException();
            //}
        }

        public ref WorkPriority GetRefWorkPriority(WorkPriorityType priorityType)
        {
            return ref Work()[workComponentStartIndex + (int)priorityType];
        }

        public WorkPriority Get(WorkPriorityType priorityType)
        {
            return Work()[workComponentStartIndex + (int)priorityType];
            //switch (priorityType)
            //{
            //    case WorkPriorityType.move:
            //        return move;
            //    case WorkPriorityType.wood:
            //        return wood;
            //    case WorkPriorityType.stone:
            //        return stone;
            //    case WorkPriorityType.craftFuel:
            //        return craft_fuel;
            //    case WorkPriorityType.craftFood:
            //        return craft_food;
            //    case WorkPriorityType.craftBeer:
            //        return craft_beer;
            //    case WorkPriorityType.craftCoolingFluid:
            //        return craft_coolingfluid;

            //    case WorkPriorityType.smeltIron:
            //        return craft_iron;
            //    case WorkPriorityType.smeltTin:
            //        return craft_tin;
            //    case WorkPriorityType.smeltCopper:
            //        return craft_cupper;
            //    case WorkPriorityType.smeltLead:
            //        return craft_lead;
            //    case WorkPriorityType.smeltSilver:
            //        return craft_silver;
            //    case WorkPriorityType.craftBronze:
            //        return craft_bronze;
            //    case WorkPriorityType.craftCastIron:
            //        return craft_castiron;
            //    case WorkPriorityType.craftBloomeryIron:
            //        return craft_bloomeryiron;
            //    case WorkPriorityType.craftSteel:
            //        return craft_steel;
            //    case WorkPriorityType.craftMithril:
            //        return craft_mithril;

            //    case WorkPriorityType.craftPalisade:
            //        return craft_palisade;
            //    case WorkPriorityType.craftToolkit:
            //        return craft_toolkit;
            //    case WorkPriorityType.craftWagon2Wheel:
            //        return craft_wagonlight;
            //    case WorkPriorityType.craftWagon4Wheel:
            //        return craft_wagonheavy;
            //    case WorkPriorityType.craftBlackPowder:
            //        return craft_blackpowder;
            //    case WorkPriorityType.craftGunPowder:
            //        return craft_gunpowder;
            //    case WorkPriorityType.craftBullet:
            //        return craft_bullet;

            //    case WorkPriorityType.craftSharpStick:
            //        return craft_sharpstick;
            //    case WorkPriorityType.craftBronzeSword:
            //        return craft_bronzesword;
            //    case WorkPriorityType.craftShortSword:
            //        return craft_shortsword;
            //    case WorkPriorityType.craftSword:
            //        return craft_sword;
            //    case WorkPriorityType.craftLongSword:
            //        return craft_longsword;
            //    case WorkPriorityType.craftHandSpear:
            //        return craft_handspear;

            //    case WorkPriorityType.craftWarhammer:
            //        return craft_warhammer;
            //    case WorkPriorityType.craftTwoHandSword:
            //        return craft_twohandsword;
            //    case WorkPriorityType.craftKnightsLance:
            //        return craft_knightslance;
            //    case WorkPriorityType.craftMithrilSword:
            //        return craft_mithrilsword;
            //    case WorkPriorityType.craftMithrilbow:
            //        return craft_mithrilbow;

            //    case WorkPriorityType.craftSlingshot:
            //        return craft_slingshot;
            //    case WorkPriorityType.craftThrowingspear:
            //        return craft_throwingspear;
            //    case WorkPriorityType.craftBow:
            //        return craft_bow;
            //    case WorkPriorityType.craftLongbow:
            //        return craft_longbow;
            //    case WorkPriorityType.craftCrossbow:
            //        return craft_crossbow;

            //    case WorkPriorityType.craftHandCannon:
            //        return craft_handcannon;
            //    case WorkPriorityType.craftHandCulverin:
            //        return craft_handculverin;
            //    case WorkPriorityType.craftRifle:
            //        return craft_rifle;
            //    case WorkPriorityType.craftBlunderbuss:
            //        return craft_blunderbus;

            //    case WorkPriorityType.craftBallista:
            //        return craft_ballista;
            //    case WorkPriorityType.craftManuBallista:
            //        return craft_manuballista;
            //    case WorkPriorityType.craftCatapult:
            //        return craft_catapult;
            //    case WorkPriorityType.craftBatteringRam:
            //        return craft_batteringram;

            //    case WorkPriorityType.craftSiegeCannonBronze:
            //        return craft_siegecannonbronze;
            //    case WorkPriorityType.craftManCannonBronze:
            //        return craft_mancannonbronze;
            //    case WorkPriorityType.craftSiegeCannonIron:
            //        return craft_siegecannoniron;
            //    case WorkPriorityType.craftManCannonIron:
            //        return craft_mancannoniron;

            //    case WorkPriorityType.craftPaddedArmor:
            //        return craft_paddedarmor;
            //    case WorkPriorityType.craftHeavyPaddedArmor:
            //        return craft_heavypaddedarmor;
            //    case WorkPriorityType.craftBronzeArmor:
            //        return craft_bronzearmor;
            //    case WorkPriorityType.craftMailArmor:
            //        return craft_mailarmor;
            //    case WorkPriorityType.craftHeavyMailArmor:
            //        return craft_heavymailarmor;
            //    case WorkPriorityType.craftPlateArmor:
            //        return craft_platearmor;
            //    case WorkPriorityType.craftFullPlateArmor:
            //        return craft_fullplatearmor;
            //    case WorkPriorityType.craftMithrilArmor:
            //        return craft_mithrilarmor;

            //    case WorkPriorityType.farmfood:
            //        return farm_food;
            //    case WorkPriorityType.farmfuel:
            //        return farm_fuel;
            //    case WorkPriorityType.farmlinen:
            //        return farm_linen;

            //    case WorkPriorityType.bogiron:
            //        return bogiron;

            //    case WorkPriorityType.miningIron:
            //        return mining_iron;
            //    case WorkPriorityType.miningTin:
            //        return mining_tin;
            //    case WorkPriorityType.miningCopper:
            //        return mining_copper;
            //    case WorkPriorityType.miningLead:
            //        return mining_lead;
            //    case WorkPriorityType.miningSilver:
            //        return mining_silver;
            //    case WorkPriorityType.miningGold:
            //        return mining_gold;
            //    case WorkPriorityType.miningMithril:
            //        return mining_mithril;
            //    case WorkPriorityType.miningSulfur:
            //        return mining_sulfur;
            //    case WorkPriorityType.miningCoal:
            //        return mining_coal;

            //    case WorkPriorityType.trading:
            //        return trading;
            //    case WorkPriorityType.autoBuild:
            //        return autoBuild;
            //    case WorkPriorityType.buildOrders:
            //        return buildOrder;
            //    case WorkPriorityType.expandFarms:
            //        return expandFarms;
            //    case WorkPriorityType.smeltGold:
            //        return smeltgold;

            //    case WorkPriorityType.coinmaker_copper:
            //        return coinmaker_copper;
            //    case WorkPriorityType.coinmaker_bronze:
            //        return coinmaker_bronze;
            //    case WorkPriorityType.coinmaker_silver:
            //        return coinmaker_silver;
            //    case WorkPriorityType.coinmaker_mithril:
            //        return coinmaker_mithril;

            //    default:
            //        throw new NotImplementedException();
            //}
        }

        public void SetWorkPriority(ItemResourceType item, WorkPriority work)
        {
            var priorityType = ItemPropertyColl.Get(item).work;
            if (priorityType != WorkPriorityType.NUM_NONE)
            {
                Work()[workComponentStartIndex + (int)priorityType] = work;
            }
            //switch (item)
            //{
            //    case ItemResourceType.SoftWood:
            //    case ItemResourceType.HardWood:
            //    case ItemResourceType.DryWood:
            //    case ItemResourceType.Wood_Group:
            //        wood = work;
            //        break;

            //    //case ItemResourceType.UN_StoneBlock:
            //    case ItemResourceType.Stone_G:
            //        stone = work;
            //        break;

            //    case ItemResourceType.Fuel_G:
            //        craft_fuel = work;
            //        break;

            //    case ItemResourceType.Food_G:
            //    case ItemResourceType.Wheat:
            //    case ItemResourceType.Meat:
            //    case ItemResourceType.Egg:
            //        craft_food = work;
            //        break;

            //    case ItemResourceType.Beer:
            //        craft_beer = work;
            //        break;

            //    case ItemResourceType.CoolingFluid:
            //        craft_coolingfluid = work;
            //        break;

            //    case ItemResourceType.Iron_G:
            //        craft_iron = work;
            //        break;
            //    case ItemResourceType.Tin:
            //        craft_tin = work;
            //        break;
            //    case ItemResourceType.Copper:
            //        craft_cupper = work;
            //        break;
            //    case ItemResourceType.Lead:
            //        craft_lead = work;
            //        break;
            //    case ItemResourceType.Silver:
            //        craft_silver = work;
            //        break;
            //    case ItemResourceType.Bronze:
            //        craft_bronze = work;
            //        break;
            //    case ItemResourceType.CastIron:
            //        craft_castiron = work;
            //        break;
            //    case ItemResourceType.BloomeryIron:
            //        craft_bloomeryiron = work;
            //        break;
            //    case ItemResourceType.Steel:
            //        craft_steel = work;
            //        break;
            //    case ItemResourceType.Mithril:
            //        craft_mithril = work;
            //        break;

            //    case ItemResourceType.Palisade:
            //        craft_palisade = work;
            //        break;
            //    case ItemResourceType.Toolkit:
            //        craft_toolkit = work;
            //        break;
            //    case ItemResourceType.Wagon2Wheel:
            //        craft_wagonlight = work;
            //        break;
            //    case ItemResourceType.Wagon4Wheel:
            //        craft_wagonheavy = work;
            //        break;
            //    case ItemResourceType.BlackPowder:
            //        craft_blackpowder = work;
            //        break;
            //    case ItemResourceType.GunPowder:
            //        craft_gunpowder = work;
            //        break;
            //    case ItemResourceType.LedBullet:
            //        craft_bullet = work;
            //        break;

            //    case ItemResourceType.SharpStick:
            //        craft_sharpstick = work;
            //        break;
            //    case ItemResourceType.BronzeSword:
            //        craft_bronzesword = work;
            //        break;
            //    case ItemResourceType.ShortSword:
            //        craft_shortsword = work;
            //        break;
            //    case ItemResourceType.Sword:
            //        craft_sword = work;
            //        break;
            //    case ItemResourceType.LongSword:
            //        craft_longsword = work;
            //        break;
            //    case ItemResourceType.HandSpear:
            //    case ItemResourceType.Pike:
            //        craft_handspear = work;
            //        break;

            //    case ItemResourceType.Warhammer:
            //        craft_warhammer = work;
            //        break;
            //    case ItemResourceType.TwoHandSword:
            //        craft_twohandsword = work;
            //        break;
            //    //case ItemResourceType.KnightsLance:
            //    //    craft_knightslance = work;
            //    //    break;
            //    case ItemResourceType.MithrilSword:
            //        craft_mithrilsword = work;
            //        break;
            //    case ItemResourceType.MithrilBow:
            //        craft_mithrilbow = work;
            //        break;

            //    case ItemResourceType.SlingShot:
            //        craft_slingshot = work;
            //        break;
            //    case ItemResourceType.ThrowingSpear:
            //        craft_throwingspear = work;
            //        break;
            //    case ItemResourceType.Bow:
            //        craft_bow = work;
            //        break;
            //    case ItemResourceType.LongBow:
            //        craft_longbow = work;
            //        break;
            //    case ItemResourceType.Crossbow:
            //        craft_crossbow = work;
            //        break;

            //    case ItemResourceType.HandCannon:
            //        craft_handcannon = work;
            //        break;
            //    case ItemResourceType.HandCulverin:
            //        craft_handculverin = work;
            //        break;
            //    case ItemResourceType.Rifle:
            //        craft_rifle = work;
            //        break;
            //    case ItemResourceType.Blunderbuss:
            //        craft_blunderbus = work;
            //        break;

            //    case ItemResourceType.Ballista:
            //        craft_ballista = work;
            //        break;
            //    case ItemResourceType.Manuballista:
            //        craft_manuballista = work;
            //        break;
            //    case ItemResourceType.Catapult:
            //        craft_catapult = work;
            //        break;
            //    case ItemResourceType.UN_BatteringRam:
            //        craft_batteringram = work;
            //        break;

            //    case ItemResourceType.SiegeCannonBronze:
            //        craft_siegecannonbronze = work;
            //        break;
            //    case ItemResourceType.ManCannonBronze:
            //        craft_mancannonbronze = work;
            //        break;
            //    case ItemResourceType.SiegeCannonIron:
            //        craft_siegecannoniron = work;
            //        break;
            //    case ItemResourceType.ManCannonIron:
            //        craft_mancannoniron = work;
            //        break;

            //    case ItemResourceType.PaddedArmor:
            //        craft_paddedarmor = work;
            //        break;
            //    case ItemResourceType.HeavyPaddedArmor:
            //        craft_heavypaddedarmor = work;
            //        break;
            //    case ItemResourceType.BronzeArmor:
            //        craft_bronzearmor = work;
            //        break;
            //    case ItemResourceType.IronArmor:
            //        craft_mailarmor = work;
            //        break;
            //    case ItemResourceType.HeavyIronArmor:
            //        craft_heavymailarmor = work;
            //        break;
            //    case ItemResourceType.LightPlateArmor:
            //        craft_platearmor = work;
            //        break;
            //    case ItemResourceType.FullPlateArmor:
            //        craft_fullplatearmor = work;
            //        break;
            //    case ItemResourceType.MithrilArmor:
            //        craft_mithrilarmor = work;
            //        break;

            //    case ItemResourceType.Rapeseed:
            //    case ItemResourceType.Hemp:
            //    case ItemResourceType.Linen:
            //    case ItemResourceType.SkinLinen_Group:
            //        farm_linen = work;
            //        break;
            //    //case ItemResourceType.Food_G: // already mapped, but keep farming group here if needed
            //    //    farm_food = work;
            //    //    break;
            //    //case ItemResourceType.Fuel_G: // already mapped, but keep fuel group here if needed
            //    //    farm_fuel = work;
            //    //    break;

            //    case ItemResourceType.BogIron:
            //        bogiron = work;
            //        break;

            //    case ItemResourceType.IronOre_G:
            //        mining_iron = work;
            //        break;
            //    case ItemResourceType.TinOre:
            //        mining_tin = work;
            //        break;
            //    case ItemResourceType.CopperOre:
            //        mining_copper = work;
            //        break;
            //    case ItemResourceType.LeadOre:
            //        mining_lead = work;
            //        break;
            //    case ItemResourceType.SilverOre:
            //        mining_silver = work;
            //        break;
            //    case ItemResourceType.GoldOre:
            //        mining_gold = work;
            //        break;
            //    case ItemResourceType.RawMithril:
            //        mining_mithril = work;
            //        break;
            //    case ItemResourceType.Sulfur:
            //        mining_sulfur = work;
            //        break;
            //    case ItemResourceType.Coal:
            //        mining_coal = work;
            //        break;

            //    case ItemResourceType.CopperCoin:
            //        coinmaker_copper = work;
            //        break;
            //    case ItemResourceType.BronzeCoin:
            //        coinmaker_bronze = work;
            //        break;
            //    case ItemResourceType.SilverCoin:
            //        coinmaker_silver = work;
            //        break;
            //    case ItemResourceType.ElfCoin:
            //        coinmaker_mithril = work;
            //        break;

            //    case ItemResourceType.Gold:
            //        smeltgold = work;
            //        break;

            //    // Things that don't directly map to crafting priorities
            //    //case ItemResourceType.NONE:
            //    //case ItemResourceType.Hen:
            //    //case ItemResourceType.Pig:
            //    //case ItemResourceType.UN_Ox:
            //    //case ItemResourceType.Egg:
            //    //case ItemResourceType.Meat:
            //    //case ItemResourceType.Water_G:
            //    //case ItemResourceType.Leather:
            //    //case ItemResourceType.UN_Clay:
            //    //case ItemResourceType.UN_Brick:
            //    //case ItemResourceType.UN_Trebuchet:
            //    //case ItemResourceType.AutomatedItem:
            //    //case ItemResourceType.RoseWarrior_soldier:
            //    //case ItemResourceType.RoseWarrior_tank:
            //    //case ItemResourceType.RoseWarrior_dog:
            //    //case ItemResourceType.Men:
            //    //case ItemResourceType.ServiceMen:
            //    //case ItemResourceType.NUM:
            //    //case ItemResourceType.RESOURCES:
            //    //    // No priority mapping implemented
            //    //    break;

            //    default:
            //        throw new NotImplementedException($"SetWorkPriority not implemented for {item}");
            //}
        }




        void SetWorkPriority(WorkPriorityType priorityType, WorkPriority value)
        {
            Work()[workComponentStartIndex + (int)priorityType] = value;
            //switch (priorityType)
            //{
            //    case WorkPriorityType.move:
            //        move = value;
            //        break;
            //    case WorkPriorityType.wood:
            //        wood = value;
            //        break;
            //    case WorkPriorityType.stone:
            //        stone = value;
            //        break;
            //    case WorkPriorityType.craftFuel:
            //        craft_fuel = value;
            //        break;
            //    case WorkPriorityType.craftFood:
            //        craft_food = value;
            //        break;
            //    case WorkPriorityType.craftBeer:
            //        craft_beer = value;
            //        break;
            //    case WorkPriorityType.craftCoolingFluid:
            //        craft_coolingfluid = value;
            //        break;

            //    case WorkPriorityType.smeltIron:
            //        craft_iron = value;
            //        break;
            //    case WorkPriorityType.smeltTin:
            //        craft_tin = value;
            //        break;
            //    case WorkPriorityType.smeltCopper:
            //        craft_cupper = value;
            //        break;
            //    case WorkPriorityType.smeltLead:
            //        craft_lead = value;
            //        break;
            //    case WorkPriorityType.smeltSilver:
            //        craft_silver = value;
            //        break;

            //    case WorkPriorityType.craftBronze:
            //        craft_bronze = value;
            //        break;
            //    case WorkPriorityType.craftCastIron:
            //        craft_castiron = value;
            //        break;
            //    case WorkPriorityType.craftBloomeryIron:
            //        craft_bloomeryiron = value;
            //        break;
            //    case WorkPriorityType.craftSteel:
            //        craft_steel = value;
            //        break;
            //    case WorkPriorityType.craftMithril:
            //        craft_mithril = value;
            //        break;

            //    case WorkPriorityType.craftPalisade:
            //        craft_palisade = value;
            //        break;
            //    case WorkPriorityType.craftToolkit:
            //        craft_toolkit = value;
            //        break;
            //    case WorkPriorityType.craftWagon2Wheel:
            //        craft_wagonlight = value;
            //        break;
            //    case WorkPriorityType.craftWagon4Wheel:
            //        craft_wagonheavy = value;
            //        break;
            //    case WorkPriorityType.craftBlackPowder:
            //        craft_blackpowder = value;
            //        break;
            //    case WorkPriorityType.craftGunPowder:
            //        craft_gunpowder = value;
            //        break;
            //    case WorkPriorityType.craftBullet:
            //        craft_bullet = value;
            //        break;

            //    case WorkPriorityType.craftSharpStick:
            //        craft_sharpstick = value;
            //        break;
            //    case WorkPriorityType.craftBronzeSword:
            //        craft_bronzesword = value;
            //        break;
            //    case WorkPriorityType.craftShortSword:
            //        craft_shortsword = value;
            //        break;
            //    case WorkPriorityType.craftSword:
            //        craft_sword = value;
            //        break;
            //    case WorkPriorityType.craftLongSword:
            //        craft_longsword = value;
            //        break;
            //    case WorkPriorityType.craftHandSpear:
            //        craft_handspear = value;
            //        break;

            //    case WorkPriorityType.craftWarhammer:
            //        craft_warhammer = value;
            //        break;
            //    case WorkPriorityType.craftTwoHandSword:
            //        craft_twohandsword = value;
            //        break;
            //    case WorkPriorityType.craftKnightsLance:
            //        craft_knightslance = value;
            //        break;
            //    case WorkPriorityType.craftMithrilSword:
            //        craft_mithrilsword = value;
            //        break;

            //    case WorkPriorityType.craftSlingshot:
            //        craft_slingshot = value;
            //        break;
            //    case WorkPriorityType.craftThrowingspear:
            //        craft_throwingspear = value;
            //        break;
            //    case WorkPriorityType.craftBow:
            //        craft_bow = value;
            //        break;
            //    case WorkPriorityType.craftLongbow:
            //        craft_longbow = value;
            //        break;
            //    case WorkPriorityType.craftCrossbow:
            //        craft_crossbow = value;
            //        break;
            //    case WorkPriorityType.craftMithrilbow:
            //        craft_mithrilbow = value;
            //        break;

            //    case WorkPriorityType.craftHandCannon:
            //        craft_handcannon = value;
            //        break;
            //    case WorkPriorityType.craftHandCulverin:
            //        craft_handculverin = value;
            //        break;
            //    case WorkPriorityType.craftRifle:
            //        craft_rifle = value;
            //        break;
            //    case WorkPriorityType.craftBlunderbuss:
            //        craft_blunderbus = value;
            //        break;

            //    case WorkPriorityType.craftBallista:
            //        craft_ballista = value;
            //        break;
            //    case WorkPriorityType.craftManuBallista:
            //        craft_manuballista = value;
            //        break;
            //    case WorkPriorityType.craftCatapult:
            //        craft_catapult = value;
            //        break;
            //    case WorkPriorityType.craftBatteringRam:
            //        craft_batteringram = value;
            //        break;

            //    case WorkPriorityType.craftSiegeCannonBronze:
            //        craft_siegecannonbronze = value;
            //        break;
            //    case WorkPriorityType.craftManCannonBronze:
            //        craft_mancannonbronze = value;
            //        break;
            //    case WorkPriorityType.craftSiegeCannonIron:
            //        craft_siegecannoniron = value;
            //        break;
            //    case WorkPriorityType.craftManCannonIron:
            //        craft_mancannoniron = value;
            //        break;

            //    case WorkPriorityType.craftPaddedArmor:
            //        craft_paddedarmor = value;
            //        break;
            //    case WorkPriorityType.craftHeavyPaddedArmor:
            //        craft_heavypaddedarmor = value;
            //        break;
            //    case WorkPriorityType.craftBronzeArmor:
            //        craft_bronzearmor = value;
            //        break;
            //    case WorkPriorityType.craftMailArmor:
            //        craft_mailarmor = value;
            //        break;
            //    case WorkPriorityType.craftHeavyMailArmor:
            //        craft_heavymailarmor = value;
            //        break;
            //    case WorkPriorityType.craftPlateArmor:
            //        craft_platearmor = value;
            //        break;
            //    case WorkPriorityType.craftFullPlateArmor:
            //        craft_fullplatearmor = value;
            //        break;
            //    case WorkPriorityType.craftMithrilArmor:
            //        craft_mithrilarmor = value;
            //        break;

            //    case WorkPriorityType.farmfood:
            //        farm_food = value;
            //        break;
            //    case WorkPriorityType.farmfuel:
            //        farm_fuel = value;
            //        break;
            //    case WorkPriorityType.farmlinen:
            //        farm_linen = value;
            //        break;

            //    case WorkPriorityType.bogiron:
            //        bogiron = value;
            //        break;
            //    case WorkPriorityType.miningIron:
            //        mining_iron = value;
            //        break;
            //    case WorkPriorityType.miningTin:
            //        mining_tin = value;
            //        break;
            //    case WorkPriorityType.miningCopper:
            //        mining_copper = value;
            //        break;
            //    case WorkPriorityType.miningLead:
            //        mining_lead = value;
            //        break;
            //    case WorkPriorityType.miningSilver:
            //        mining_silver = value;
            //        break;
            //    case WorkPriorityType.miningGold:
            //        mining_gold = value;
            //        break;
            //    case WorkPriorityType.miningMithril:
            //        mining_mithril = value;
            //        break;
            //    case WorkPriorityType.miningSulfur:
            //        mining_sulfur = value;
            //        break;
            //    case WorkPriorityType.miningCoal:
            //        mining_coal = value;
            //        break;

            //    case WorkPriorityType.trading:
            //        trading = value;
            //        break;
            //    case WorkPriorityType.autoBuild:
            //        autoBuild = value;
            //        break;
            //    case WorkPriorityType.buildOrders:
            //        buildOrder = value;
            //        break;
            //    //case WorkPriorityType.expandFarms:
            //    //     = value;
            //    //    break;
            //    case WorkPriorityType.smeltGold:
            //        smeltgold = value;
            //        break;
            //    case WorkPriorityType.coinmaker_copper:
            //        coinmaker_copper = value;
            //        break;
            //    case WorkPriorityType.coinmaker_bronze:
            //        coinmaker_bronze = value;
            //        break;
            //    case WorkPriorityType.coinmaker_silver:
            //        coinmaker_silver = value;
            //        break;
            //    case WorkPriorityType.coinmaker_mithril:
            //        coinmaker_mithril = value;
            //        break;

            //    default:
            //        throw new NotImplementedException();
            //}
        }
        public void toHud(Players.LocalPlayer player, RichBoxContent content, ResourceGroupType tab, Faction faction, City city)
        {
            switch (tab)
            {
                case ResourceGroupType.Resources:
                    Get(WorkPriorityType.move).toHud(player, content, DssRef.lang.Work_Move, SpriteName.WarsWorkMove, SpriteName.WarsBuild_Storehouse, WorkPriorityType.move, faction, city, ItemResourceType.NONE);
                    Get(WorkPriorityType.wood).toHud(player, content, string.Format(DssRef.lang.Work_GatherXResource, DssRef.lang.Resource_TypeName_Wood.ToLowerInvariant()), SpriteName.WarsWorkCollect, SpriteName.WarsResource_Wood, WorkPriorityType.wood, faction, city, ItemResourceType.Wood_Group);
                    Get(WorkPriorityType.stone).toHud(player, content, string.Format(DssRef.lang.Work_GatherXResource, DssRef.lang.Resource_TypeName_Stone.ToLowerInvariant()), SpriteName.WarsWorkCollect, SpriteName.WarsResource_Stone, WorkPriorityType.stone, faction, city, ItemResourceType.Stone_G);
                    Get(WorkPriorityType.collectClay).toHud(player, content, string.Format(DssRef.lang.Work_GatherXResource, DssRef.todoLang.Resource_TypeName_Clay), SpriteName.WarsWorkCollect, SpriteName.WarsResource_Clay, WorkPriorityType.collectClay, faction, city, ItemResourceType.Clay);
                    Get(WorkPriorityType.miningBrick).toHud(player, content, string.Format(DssRef.lang.Work_MiningResource, DssRef.todoLang.Resource_TypeName_Brick), SpriteName.WarsWorkMine, SpriteName.WarsResource_Brick, WorkPriorityType.miningBrick, faction, city, ItemResourceType.Brick,
                         WorkViewMode.Default, ItemResourceType.NONE, city == null ? 0 : city.terrainStructure.mineCount_stoneblock);
                    Get(WorkPriorityType.craftBrick).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Brick), SpriteName.WarsHammer, SpriteName.WarsResource_Brick, WorkPriorityType.craftBrick, faction, city, ItemResourceType.Brick);

                    Get(WorkPriorityType.farmFood).toHud(player, content, DssRef.lang.Work_Farming + ": " + DssRef.lang.BuildingType_Orchard, SpriteName.WarsWorkFarm, SpriteName.WarsResource_Food, WorkPriorityType.farmFood, faction, city, ItemResourceType.Food_G);
                    Get(WorkPriorityType.farmRawFood).toHud(player, content, DssRef.lang.Work_Farming + ": " + DssRef.lang.Resource_TypeName_Food.ToLowerInvariant(), SpriteName.WarsWorkFarm, SpriteName.WarsResource_RawFood, WorkPriorityType.farmRawFood, faction, city, ItemResourceType.RawFood_Group);
                    Get(WorkPriorityType.miningSalt).toHud(player, content, string.Format(DssRef.lang.Work_MiningResource, DssRef.todoLang.Resource_TypeName_Salt.ToLowerInvariant()), SpriteName.WarsWorkMine, SpriteName.WarsResource_Salt, WorkPriorityType.miningSalt, faction, city, ItemResourceType.Salt,
                        WorkViewMode.Default, ItemResourceType.NONE, city == null ? 0 : city.terrainStructure.mineCount_salt + city.buildingStructure.DryingPan_count);

                    Get(WorkPriorityType.farmfuel).toHud(player, content, DssRef.lang.Work_Farming + ": " + DssRef.lang.Resource_TypeName_Fuel.ToLowerInvariant(), SpriteName.WarsWorkFarm, SpriteName.WarsResource_Fuel, WorkPriorityType.farmfuel, faction, city, ItemResourceType.Fuel_G);
                    Get(WorkPriorityType.farmlinen).toHud(player, content, DssRef.lang.Work_Farming + ": " + DssRef.lang.Resource_TypeName_Linen.ToLowerInvariant(), SpriteName.WarsWorkFarm, SpriteName.WarsResource_LinenCloth, WorkPriorityType.farmlinen, faction, city, ItemResourceType.SkinLinen_Group);

                    Get(WorkPriorityType.craftFood).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Food.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Food, WorkPriorityType.craftFood, faction, city, ItemResourceType.Food_G);
                    Get(WorkPriorityType.craftConservedFood).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_ConservedFood.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_ConservedFood, WorkPriorityType.craftConservedFood, faction, city, ItemResourceType.ConservedFood);
                    Get(WorkPriorityType.craftFuel).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Fuel.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Fuel, WorkPriorityType.craftFuel, faction, city, ItemResourceType.Fuel_G);
                    Get(WorkPriorityType.craftBeer).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Beer.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Beer, WorkPriorityType.craftBeer, faction, city, ItemResourceType.Beer);
                    Get(WorkPriorityType.craftCoolingFluid).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_CoolingFluid.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_CoolingFluid, WorkPriorityType.craftCoolingFluid, faction, city, ItemResourceType.CoolingFluid);

                    Get(WorkPriorityType.craftContainer).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Container.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Container, WorkPriorityType.craftContainer, faction, city, ItemResourceType.Container);
                    Get(WorkPriorityType.craftPalisade).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Palisade.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Palisade, WorkPriorityType.craftPalisade, faction, city, ItemResourceType.Palisade);
                    Get(WorkPriorityType.craftToolkit).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Toolkit.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Toolkit, WorkPriorityType.craftToolkit, faction, city, ItemResourceType.Toolkit);
                    Get(WorkPriorityType.craftWagon2Wheel).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Wagon2Wheel.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Wagon2Wheel, WorkPriorityType.craftWagon2Wheel, faction, city, ItemResourceType.Wagon2Wheel);
                    Get(WorkPriorityType.craftWagon4Wheel).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Wagon4Wheel.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Wagon4Wheel, WorkPriorityType.craftWagon4Wheel, faction, city, ItemResourceType.Wagon4Wheel);

                    // New Wagon types (Assumed types based on naming convention)
                    Get(WorkPriorityType.craftWagonClosed).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_WagonClosed.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_WagonClosed, WorkPriorityType.craftWagonClosed, faction, city, ItemResourceType.WagonClosed);
                    Get(WorkPriorityType.craftWagonIron).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_WagonIron.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_WagonIron, WorkPriorityType.craftWagonIron, faction, city, ItemResourceType.WagonIron);
                    Get(WorkPriorityType.craftWagonSteel).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_WagonSteel.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_WagonSteel, WorkPriorityType.craftWagonSteel, faction, city, ItemResourceType.WagonSteel);

                    Get(WorkPriorityType.craftBlackPowder).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_BlackPowder.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_BlackPowder, WorkPriorityType.craftBlackPowder, faction, city, ItemResourceType.BlackPowder);
                    Get(WorkPriorityType.craftGunPowder).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_GunPowder.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_GunPowder, WorkPriorityType.craftGunPowder, faction, city, ItemResourceType.GunPowder);
                    Get(WorkPriorityType.craftBullet).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_LedBullet.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Bullets, WorkPriorityType.craftBullet, faction, city, ItemResourceType.LedBullet);

                    content.newParagraph();
                    Get(WorkPriorityType.autoBuild).toHud(player, content, DssRef.lang.Work_AutoBuild, SpriteName.AutomationGearIcon, SpriteName.warsBuildCategoryHouse, WorkPriorityType.autoBuild, faction, city, ItemResourceType.NONE);
                    Get(WorkPriorityType.buildOrders).toHud(player, content, DssRef.lang.Build_Order, SpriteName.WarsHammer, SpriteName.warsBuildCategoryHouse, WorkPriorityType.buildOrders, faction, city, ItemResourceType.NONE);
                    break;

                case ResourceGroupType.Metals:
                    Get(WorkPriorityType.bogiron).toHud(player, content, DssRef.lang.Resource_TypeName_BogIron, SpriteName.WarsWorkCollect, SpriteName.WarsResource_IronOre, WorkPriorityType.bogiron, faction, city, ItemResourceType.IronOre_G);
                    content.space();
                    HudLib.InfoButton(content, new RbTooltip_Text(DssRef.lang.Resource_BogIronDescription));

                    Get(WorkPriorityType.miningIron).toHud(player, content, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Iron.ToLowerInvariant()), SpriteName.WarsWorkMine, SpriteName.WarsResource_Iron, WorkPriorityType.miningIron, faction, city, ItemResourceType.IronOre_G,
                        WorkViewMode.Default, ItemResourceType.NONE, city == null ? 0 : city.terrainStructure.mineCount_bogIron + city.terrainStructure.mineCount_iron);

                    Get(WorkPriorityType.miningTin).toHud(player, content, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Tin.ToLowerInvariant()), SpriteName.WarsWorkMine, SpriteName.WarsResource_Tin, WorkPriorityType.miningTin, faction, city, ItemResourceType.TinOre,
                        WorkViewMode.Default, ItemResourceType.NONE, city == null ? 0 : city.terrainStructure.mineCount_tin);

                    Get(WorkPriorityType.miningCopper).toHud(player, content, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Copper.ToLowerInvariant()), SpriteName.WarsWorkMine, SpriteName.WarsResource_Copper, WorkPriorityType.miningCopper, faction, city, ItemResourceType.CopperOre,
                         WorkViewMode.Default, ItemResourceType.NONE, city == null ? 0 : city.terrainStructure.mineCount_copper);

                    Get(WorkPriorityType.miningLead).toHud(player, content, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Lead.ToLowerInvariant()), SpriteName.WarsWorkMine, SpriteName.WarsResource_Lead, WorkPriorityType.miningLead, faction, city, ItemResourceType.LeadOre,
                         WorkViewMode.Default, ItemResourceType.NONE, city == null ? 0 : city.terrainStructure.mineCount_lead);

                    Get(WorkPriorityType.miningSilver).toHud(player, content, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Silver.ToLowerInvariant()), SpriteName.WarsWorkMine, SpriteName.WarsResource_Silver, WorkPriorityType.miningSilver, faction, city, ItemResourceType.SilverOre,
                         WorkViewMode.Default, ItemResourceType.NONE, city == null ? 0 : city.terrainStructure.mineCount_silver);

                    Get(WorkPriorityType.miningGold).toHud(player, content, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.ResourceType_Gold.ToLowerInvariant()), SpriteName.WarsWorkMine, SpriteName.WarsResource_Gold, WorkPriorityType.miningGold, faction, city, ItemResourceType.GoldOre,
                         WorkViewMode.Default, ItemResourceType.NONE, city == null ? 0 : city.terrainStructure.mineCount_gold);

                    Get(WorkPriorityType.miningMithril).toHud(player, content, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Mithril.ToLowerInvariant()), SpriteName.WarsWorkMine, SpriteName.WarsResource_Mithril, WorkPriorityType.miningMithril, faction, city, ItemResourceType.RawMithril,
                         WorkViewMode.Default, ItemResourceType.NONE, city == null ? 0 : city.terrainStructure.mineCount_mithril);

                    Get(WorkPriorityType.miningSulfur).toHud(player, content, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Sulfur.ToLowerInvariant()), SpriteName.WarsWorkMine, SpriteName.WarsResource_Sulfur, WorkPriorityType.miningSulfur, faction, city, ItemResourceType.Sulfur,
                         WorkViewMode.Default, ItemResourceType.NONE, city == null ? 0 : city.terrainStructure.mineCount_sulfur);

                    Get(WorkPriorityType.miningCoal).toHud(player, content, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Coal.ToLowerInvariant()), SpriteName.WarsWorkMine, SpriteName.WarsResource_Fuel, WorkPriorityType.miningCoal, faction, city, ItemResourceType.Fuel_G,
                         WorkViewMode.Default, ItemResourceType.NONE, city == null ? 0 : city.terrainStructure.mineCount_coal);
                    content.newParagraph();

                    Get(WorkPriorityType.smeltIron).toHud(player, content, string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Iron.ToLowerInvariant()), SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Iron, WorkPriorityType.smeltIron, faction, city, ItemResourceType.Iron_G);
                    Get(WorkPriorityType.smeltTin).toHud(player, content, string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Tin.ToLowerInvariant()), SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Tin, WorkPriorityType.smeltTin, faction, city, ItemResourceType.Tin);
                    Get(WorkPriorityType.smeltCopper).toHud(player, content, string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Copper.ToLowerInvariant()), SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Copper, WorkPriorityType.smeltCopper, faction, city, ItemResourceType.Copper);
                    Get(WorkPriorityType.smeltLead).toHud(player, content, string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Lead.ToLowerInvariant()), SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Lead, WorkPriorityType.smeltLead, faction, city, ItemResourceType.Lead);
                    Get(WorkPriorityType.smeltSilver).toHud(player, content, string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Silver.ToLowerInvariant()), SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Silver, WorkPriorityType.smeltSilver, faction, city, ItemResourceType.Silver);
                    Get(WorkPriorityType.smeltGold).toHud(player, content, string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.ResourceType_Gold.ToLowerInvariant()), SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Gold, WorkPriorityType.smeltGold, faction, city, ItemResourceType.Gold);

                    Get(WorkPriorityType.craftBronze).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Bronze.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Bronze, WorkPriorityType.craftBronze, faction, city, ItemResourceType.Bronze);
                    Get(WorkPriorityType.craftCastIron).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_CastIron.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_CastIron, WorkPriorityType.craftCastIron, faction, city, ItemResourceType.CastIron);
                    Get(WorkPriorityType.craftBloomeryIron).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_BloomIron.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_BloomeryIron, WorkPriorityType.craftBloomeryIron, faction, city, ItemResourceType.BloomeryIron);
                    Get(WorkPriorityType.craftSteel).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Steel.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Steel, WorkPriorityType.craftSteel, faction, city, ItemResourceType.Steel);
                    Get(WorkPriorityType.craftMithril).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Mithril.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_MithrilAlloy, WorkPriorityType.craftMithril, faction, city, ItemResourceType.Mithril);
                    break;

                case ResourceGroupType.Weapons:
                    Get(WorkPriorityType.craftSharpStick).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_SharpStick.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Sharpstick, WorkPriorityType.craftSharpStick, faction, city, ItemResourceType.SharpStick);
                    Get(WorkPriorityType.craftBronzeSword).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_BronzeSword.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_BronzeSword, WorkPriorityType.craftBronzeSword, faction, city, ItemResourceType.BronzeSword);
                    Get(WorkPriorityType.craftShortSword).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_ShortSword.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_ShortSword, WorkPriorityType.craftShortSword, faction, city, ItemResourceType.ShortSword);
                    Get(WorkPriorityType.craftSword).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Sword.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Sword, WorkPriorityType.craftSword, faction, city, ItemResourceType.Sword);
                    Get(WorkPriorityType.craftLongSword).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_LongSword.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Longsword, WorkPriorityType.craftLongSword, faction, city, ItemResourceType.LongSword);
                    Get(WorkPriorityType.craftHandSpear).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HandSpear.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_HandSpear, WorkPriorityType.craftHandSpear, faction, city, ItemResourceType.HandSpear);

                    Get(WorkPriorityType.craftWarhammer).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Warhammer.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Warhammer, WorkPriorityType.craftWarhammer, faction, city, ItemResourceType.Warhammer);
                    Get(WorkPriorityType.craftTwoHandSword).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_TwoHandSword.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_TwoHandSword, WorkPriorityType.craftTwoHandSword, faction, city, ItemResourceType.TwoHandSword);
                    //Get(WorkPriorityType.craftKnightsLance).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_KnightsLance.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_KnightsLance, WorkPriorityType.craftKnightsLance, faction, city, ItemResourceType.KnightsLance);
                    Get(WorkPriorityType.craftMithrilSword).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_MithrilSword.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_MithrilSword, WorkPriorityType.craftMithrilSword, faction, city, ItemResourceType.MithrilSword);

                    content.newParagraph();

                    Get(WorkPriorityType.craftBucklerShield).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_BucklerShield), SpriteName.WarsHammer, SpriteName.WarsResource_BucklerShield, WorkPriorityType.craftBucklerShield, faction, city, ItemResourceType.BucklerShield);
                    Get(WorkPriorityType.craftRoundShield).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_RoundShield), SpriteName.WarsHammer, SpriteName.WarsResource_RoundShield, WorkPriorityType.craftRoundShield, faction, city, ItemResourceType.RoundShield);
                    Get(WorkPriorityType.craftHeaterShield).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_HeaterShield), SpriteName.WarsHammer, SpriteName.WarsResource_HeaterShield, WorkPriorityType.craftHeaterShield, faction, city, ItemResourceType.HeaterShield);
                    Get(WorkPriorityType.craftTowerShield).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_TowerShield), SpriteName.WarsHammer, SpriteName.WarsResource_TowerShield, WorkPriorityType.craftTowerShield, faction, city, ItemResourceType.TowerShield);
                    
                    break;

                case ResourceGroupType.Projectile:
                    Get(WorkPriorityType.craftSlingshot).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_SlingShot.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Slingshot, WorkPriorityType.craftSlingshot, faction, city, ItemResourceType.SlingShot);
                    Get(WorkPriorityType.craftThrowingspear).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_ThrowingSpear.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_ThrowSpear, WorkPriorityType.craftThrowingspear, faction, city, ItemResourceType.ThrowingSpear);
                    Get(WorkPriorityType.craftBow).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Bow.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Bow, WorkPriorityType.craftBow, faction, city, ItemResourceType.Bow);
                    Get(WorkPriorityType.craftLongbow).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Longbow.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Longbow, WorkPriorityType.craftLongbow, faction, city, ItemResourceType.LongBow);
                    Get(WorkPriorityType.craftCrossbow).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Crossbow.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Crossbow, WorkPriorityType.craftCrossbow, faction, city, ItemResourceType.Crossbow);
                    Get(WorkPriorityType.craftMithrilbow).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_MithrilBow.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Mithrilbow, WorkPriorityType.craftMithrilbow, faction, city, ItemResourceType.MithrilBow);

                    Get(WorkPriorityType.craftHandCannon).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HandCannon.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_BronzeRifle, WorkPriorityType.craftHandCannon, faction, city, ItemResourceType.HandCannon);
                    Get(WorkPriorityType.craftHandCulverin).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HandCulverin.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_BronzeShotgun, WorkPriorityType.craftHandCulverin, faction, city, ItemResourceType.HandCulverin);
                    Get(WorkPriorityType.craftRifle).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Rifle.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_IronRifle, WorkPriorityType.craftRifle, faction, city, ItemResourceType.Rifle);
                    Get(WorkPriorityType.craftBlunderbuss).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Blunderbuss.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_IronShotgun, WorkPriorityType.craftBlunderbuss, faction, city, ItemResourceType.Blunderbuss);

                    Get(WorkPriorityType.craftBallista).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.UnitType_Ballista.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Ballista, WorkPriorityType.craftBallista, faction, city, ItemResourceType.Ballista);
                    Get(WorkPriorityType.craftManuBallista).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Manuballista.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Manuballista, WorkPriorityType.craftManuBallista, faction, city, ItemResourceType.Manuballista);
                    Get(WorkPriorityType.craftCatapult).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Catapult.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_Catapult, WorkPriorityType.craftCatapult, faction, city, ItemResourceType.Catapult);

                    Get(WorkPriorityType.craftSiegeCannonBronze).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_SiegeCannonBronze.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_BronzeSiegeCannon, WorkPriorityType.craftSiegeCannonBronze, faction, city, ItemResourceType.SiegeCannonBronze);
                    Get(WorkPriorityType.craftManCannonBronze).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_ManCannonBronze.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_BronzeManCannon, WorkPriorityType.craftManCannonBronze, faction, city, ItemResourceType.ManCannonBronze);
                    Get(WorkPriorityType.craftSiegeCannonIron).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_SiegeCannonIron.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_IronSiegeCannon, WorkPriorityType.craftSiegeCannonIron, faction, city, ItemResourceType.SiegeCannonIron);
                    Get(WorkPriorityType.craftManCannonIron).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_ManCannonIron.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_IronManCannon, WorkPriorityType.craftManCannonIron, faction, city, ItemResourceType.ManCannonIron);
                    break;

                case ResourceGroupType.Armor:
                    // --- Human Armor ---
                    Get(WorkPriorityType.craftPaddedArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_PaddedArmor.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_PaddedArmor, WorkPriorityType.craftPaddedArmor, faction, city, ItemResourceType.PaddedArmor);
                    Get(WorkPriorityType.craftHeavyPaddedArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HeavyPaddedArmor.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_HeavyPaddedArmor, WorkPriorityType.craftHeavyPaddedArmor, faction, city, ItemResourceType.HeavyPaddedArmor);
                    Get(WorkPriorityType.craftBronzeArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_BronzeArmor.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_BronzeArmor, WorkPriorityType.craftBronzeArmor, faction, city, ItemResourceType.BronzeArmor);
                    Get(WorkPriorityType.craftMailArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_IronArmor.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_IronArmor, WorkPriorityType.craftMailArmor, faction, city, ItemResourceType.IronArmor);
                    Get(WorkPriorityType.craftHeavyMailArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HeavyIronArmor.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_HeavyIronArmor, WorkPriorityType.craftHeavyMailArmor, faction, city, ItemResourceType.HeavyIronArmor);
                    Get(WorkPriorityType.craftPlateArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_LightPlateArmor.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_LightPlateArmor, WorkPriorityType.craftPlateArmor, faction, city, ItemResourceType.LightPlateArmor);
                    Get(WorkPriorityType.craftFullPlateArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_FullPlateArmor.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_FullPlateArmor, WorkPriorityType.craftFullPlateArmor, faction, city, ItemResourceType.FullPlateArmor);
                    Get(WorkPriorityType.craftMithrilArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_MithrilArmor.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_MithrilArmor, WorkPriorityType.craftMithrilArmor, faction, city, ItemResourceType.MithrilArmor);

                    content.newParagraph();

                    // --- Mount Armor (Assumed types based on naming convention) ---
                    Get(WorkPriorityType.craftMountPaddedArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_PaddedArmor.ToLowerInvariant())), SpriteName.WarsHammer, SpriteName.WarsResource_MountPaddedArmor, WorkPriorityType.craftMountPaddedArmor, faction, city, ItemResourceType.MountPaddedArmor);
                    Get(WorkPriorityType.craftMountHeavyPaddedArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_HeavyPaddedArmor.ToLowerInvariant())), SpriteName.WarsHammer, SpriteName.WarsResource_MountHeavyPaddedArmor, WorkPriorityType.craftMountHeavyPaddedArmor, faction, city, ItemResourceType.MountHeavyPaddedArmor);
                    Get(WorkPriorityType.craftMountBronzeArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_BronzeArmor.ToLowerInvariant())), SpriteName.WarsHammer, SpriteName.WarsResource_MountBronzeArmor, WorkPriorityType.craftMountBronzeArmor, faction, city, ItemResourceType.MountBronzeArmor);
                    Get(WorkPriorityType.craftMountMailArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_IronArmor.ToLowerInvariant())), SpriteName.WarsHammer, SpriteName.WarsResource_MountIronArmor, WorkPriorityType.craftMountMailArmor, faction, city, ItemResourceType.MountIronArmor);
                    Get(WorkPriorityType.craftMountHeavyMailArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_HeavyIronArmor.ToLowerInvariant())), SpriteName.WarsHammer, SpriteName.WarsResource_MountHeavyIronArmor, WorkPriorityType.craftMountHeavyMailArmor, faction, city, ItemResourceType.MountHeavyIronArmor);
                    Get(WorkPriorityType.craftMountPlateArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_LightPlateArmor.ToLowerInvariant())), SpriteName.WarsHammer, SpriteName.WarsResource_MountLightPlateArmor, WorkPriorityType.craftMountPlateArmor, faction, city, ItemResourceType.MountLightPlateArmor);
                    Get(WorkPriorityType.craftMountFullPlateArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_FullPlateArmor.ToLowerInvariant())), SpriteName.WarsHammer, SpriteName.WarsResource_MountFullPlateArmor, WorkPriorityType.craftMountFullPlateArmor, faction, city, ItemResourceType.MountFullPlateArmor);
                    Get(WorkPriorityType.craftMountMithrilArmor).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_MithrilArmor.ToLowerInvariant())), SpriteName.WarsHammer, SpriteName.WarsResource_MountMithrilArmor, WorkPriorityType.craftMountMithrilArmor, faction, city, ItemResourceType.MountMithrilArmor);

                    //content.newParagraph();

                    //// --- Shields (Assumed types based on naming convention) ---
                    //Get(WorkPriorityType.craftBucklerShield).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_BucklerShield.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_BucklerShield, WorkPriorityType.craftBucklerShield, faction, city, ItemResourceType.BucklerShield);
                    //Get(WorkPriorityType.craftRoundShield).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_RoundShield.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_RoundShield, WorkPriorityType.craftRoundShield, faction, city, ItemResourceType.RoundShield);
                    //Get(WorkPriorityType.craftHeaterShield).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_HeaterShield.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_HeaterShield, WorkPriorityType.craftHeaterShield, faction, city, ItemResourceType.HeaterShield);
                    //Get(WorkPriorityType.craftTowerShield).toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_TowerShield.ToLowerInvariant()), SpriteName.WarsHammer, SpriteName.WarsResource_TowerShield, WorkPriorityType.craftTowerShield, faction, city, ItemResourceType.TowerShield);
                    break;



                case ResourceGroupType.Animals:
                    Get(WorkPriorityType.SlaughterHen).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_Hen), SpriteName.WarsSlaughter, SpriteName.WarsResource_Hen, WorkPriorityType.SlaughterHen, faction, city, ItemResourceType.Hen, WorkViewMode.Slaughter, ItemResourceType.SlaughterHen);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterHen);

                    Get(WorkPriorityType.SlaughterPig).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_Pig), SpriteName.WarsSlaughter, SpriteName.WarsResource_Pig, WorkPriorityType.SlaughterPig, faction, city, ItemResourceType.Pig, WorkViewMode.Slaughter, ItemResourceType.SlaughterPig);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterPig);

                    // --- Oxen ---
                    Get(WorkPriorityType.SlaughterOxen).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_Oxen), SpriteName.WarsSlaughter, SpriteName.WarsResource_Oxen, WorkPriorityType.SlaughterOxen, faction, city, ItemResourceType.Oxen, WorkViewMode.Slaughter, ItemResourceType.SlaughterOxen);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterOxen);

                    Get(WorkPriorityType.SlaughterKineOxen).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_KineOxen), SpriteName.WarsSlaughter, SpriteName.WarsResource_KineOxen, WorkPriorityType.SlaughterKineOxen, faction, city, ItemResourceType.KineOxen, WorkViewMode.Slaughter, ItemResourceType.SlaughterKineOxen);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterKineOxen);

                    // --- Horses ---
                    Get(WorkPriorityType.SlaughterPony).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_Pony), SpriteName.WarsSlaughter, SpriteName.WarsResource_Pony, WorkPriorityType.SlaughterPony, faction, city, ItemResourceType.Pony, WorkViewMode.Slaughter, ItemResourceType.SlaughterPony);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterPony);

                    Get(WorkPriorityType.SlaughterHorse).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_Horse), SpriteName.WarsSlaughter, SpriteName.WarsResource_Horse, WorkPriorityType.SlaughterHorse, faction, city, ItemResourceType.Horse, WorkViewMode.Slaughter, ItemResourceType.SlaughterHorse);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterHorse);

                    Get(WorkPriorityType.SlaughterWarHorse).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_WarHorse), SpriteName.WarsSlaughter, SpriteName.WarsResource_WarHorse, WorkPriorityType.SlaughterWarHorse, faction, city, ItemResourceType.WarHorse, WorkViewMode.Slaughter, ItemResourceType.SlaughterWarHorse);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterWarHorse);

                    Get(WorkPriorityType.SlaughterDraftHorse).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_DraftHorse), SpriteName.WarsSlaughter, SpriteName.WarsResource_DraftHorse, WorkPriorityType.SlaughterDraftHorse, faction, city, ItemResourceType.DraftHorse, WorkViewMode.Slaughter, ItemResourceType.SlaughterDraftHorse);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterDraftHorse);

                    // --- Wild Pigs ---
                    Get(WorkPriorityType.SlaughterWildPig).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_WildPig), SpriteName.WarsSlaughter, SpriteName.WarsResource_WildPig, WorkPriorityType.SlaughterWildPig, faction, city, ItemResourceType.WildPig, WorkViewMode.Slaughter, ItemResourceType.SlaughterWildPig);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterWildPig);

                    Get(WorkPriorityType.SlaughterWildHog).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_WildHog), SpriteName.WarsSlaughter, SpriteName.WarsResource_WildHog, WorkPriorityType.SlaughterWildHog, faction, city, ItemResourceType.WildHog, WorkViewMode.Slaughter, ItemResourceType.SlaughterWildHog);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterWildHog);

                    Get(WorkPriorityType.SlaughterWarHog).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_WarHog), SpriteName.WarsSlaughter, SpriteName.WarsResource_WarHog, WorkPriorityType.SlaughterWarHog, faction, city, ItemResourceType.WarHog, WorkViewMode.Slaughter, ItemResourceType.SlaughterWarHog);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterWarHog);

                    Get(WorkPriorityType.SlaughterStagHog).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_StagHog), SpriteName.WarsSlaughter, SpriteName.WarsResource_StagHog, WorkPriorityType.SlaughterStagHog, faction, city, ItemResourceType.StagHog, WorkViewMode.Slaughter, ItemResourceType.SlaughterStagHog);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterStagHog);

                    // --- Wolves ---
                    Get(WorkPriorityType.SlaughterWolf).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_Wolf), SpriteName.WarsSlaughter, SpriteName.WarsResource_Wolf, WorkPriorityType.SlaughterWolf, faction, city, ItemResourceType.Wolf, WorkViewMode.Slaughter, ItemResourceType.SlaughterWolf);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterWolf);

                    Get(WorkPriorityType.SlaughterWarg).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_Warg), SpriteName.WarsSlaughter, SpriteName.WarsResource_Warg, WorkPriorityType.SlaughterWarg, faction, city, ItemResourceType.Warg, WorkViewMode.Slaughter, ItemResourceType.SlaughterWarg);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterWarg);

                    Get(WorkPriorityType.SlaughterAlphaWarg).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_AlphaWarg), SpriteName.WarsSlaughter, SpriteName.WarsResource_AlphaWarg, WorkPriorityType.SlaughterAlphaWarg, faction, city, ItemResourceType.AlphaWarg, WorkViewMode.Slaughter, ItemResourceType.SlaughterAlphaWarg);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterAlphaWarg);

                    // --- Cats ---
                    Get(WorkPriorityType.SlaughterWildCat).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_WildCat), SpriteName.WarsSlaughter, SpriteName.WarsResource_WildCat, WorkPriorityType.SlaughterWildCat, faction, city, ItemResourceType.WildCat, WorkViewMode.Slaughter, ItemResourceType.SlaughterWildCat);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterWildCat);

                    Get(WorkPriorityType.SlaughterLion).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_Lion), SpriteName.WarsSlaughter, SpriteName.WarsResource_Lion, WorkPriorityType.SlaughterLion, faction, city, ItemResourceType.Lion, WorkViewMode.Slaughter, ItemResourceType.SlaughterLion);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterLion);

                    Get(WorkPriorityType.SlaughterWarLion).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_WarLion), SpriteName.WarsSlaughter, SpriteName.WarsResource_WarLion, WorkPriorityType.SlaughterWarLion, faction, city, ItemResourceType.WarLion, WorkViewMode.Slaughter, ItemResourceType.SlaughterWarLion);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterWarLion);

                    // --- Elephants ---
                    Get(WorkPriorityType.SlaughterElephant).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_Elephant), SpriteName.WarsSlaughter, SpriteName.WarsResource_Elephant, WorkPriorityType.SlaughterElephant, faction, city, ItemResourceType.Elephant, WorkViewMode.Slaughter, ItemResourceType.SlaughterElephant);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterElephant);

                    Get(WorkPriorityType.SlaughterWarElephant).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_WarElephant), SpriteName.WarsSlaughter, SpriteName.WarsResource_WarElephant, WorkPriorityType.SlaughterWarElephant, faction, city, ItemResourceType.WarElephant, WorkViewMode.Slaughter, ItemResourceType.SlaughterWarElephant);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterWarElephant);

                    Get(WorkPriorityType.SlaughterOliphant).toHud(player, content, string.Format(DssRef.todoLang.Work_SlaughterX, DssRef.todoLang.Resource_TypeName_Oliphant), SpriteName.WarsSlaughter, SpriteName.WarsResource_Oliphant, WorkPriorityType.SlaughterOliphant, faction, city, ItemResourceType.Oliphant, WorkViewMode.Slaughter, ItemResourceType.SlaughterOliphant);
                    content.space(1);
                    waitForFullStock(WorkPriorityType.SlaughterOliphant);
                    break;

                case ResourceGroupType.Mint:
                    const string CraftCoinCaption = "{0} - {1}";

                    {
                        content.newLine();
                        Get(WorkPriorityType.coinmaker_copper).titleToHud(content, string.Format(CraftCoinCaption, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Coin), DssRef.lang.Resource_TypeName_Copper),
                            SpriteName.WarsSlaughter, SpriteName.WarsResource_CopperCoin);
                        content.space();
                        HudLib.blueprintButton(city, player, content, Minting.CopperCoin);
                        content.newLine();
                        Get(WorkPriorityType.coinmaker_copper).priorityToHud(player, content, WorkPriorityType.coinmaker_copper, faction, city);
                        content.space(2);
                        waitForFullStock(WorkPriorityType.coinmaker_copper);
                    }
                    {
                        content.newParagraph();
                        Get(WorkPriorityType.coinmaker_bronze).titleToHud(content, string.Format(CraftCoinCaption, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Coin), DssRef.lang.Resource_TypeName_Bronze),
                            SpriteName.WarsSlaughter, SpriteName.WarsResource_BonzeCoin);
                        content.space();
                        HudLib.blueprintButton(city, player, content, Minting.BronzeCoin);
                        content.newLine();
                        Get(WorkPriorityType.coinmaker_bronze).priorityToHud(player, content, WorkPriorityType.coinmaker_bronze, faction, city);
                        content.space(2);
                        waitForFullStock(WorkPriorityType.coinmaker_bronze);
                    }
                    {
                        content.newParagraph();
                        Get(WorkPriorityType.coinmaker_silver).titleToHud(content, string.Format(CraftCoinCaption, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Coin), DssRef.lang.Resource_TypeName_Silver),
                            SpriteName.WarsSlaughter, SpriteName.WarsResource_SilverCoin);
                        content.space();
                        HudLib.blueprintButton(city, player, content, Minting.SilverCoin);
                        content.newLine();
                        Get(WorkPriorityType.coinmaker_silver).priorityToHud(player, content, WorkPriorityType.coinmaker_silver, faction, city);
                        content.space(2);
                        waitForFullStock(WorkPriorityType.coinmaker_silver);
                    }
                    {
                        content.newParagraph();
                        Get(WorkPriorityType.coinmaker_mithril).titleToHud(content, string.Format(CraftCoinCaption, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Coin), DssRef.lang.Resource_TypeName_Mithril),
                            SpriteName.WarsSlaughter, SpriteName.WarsResource_ElfCoin);
                        content.space();
                        HudLib.blueprintButton(city, player, content, Minting.ElfCoin);
                        content.newLine();
                        Get(WorkPriorityType.coinmaker_mithril).priorityToHud(player, content, WorkPriorityType.coinmaker_mithril, faction, city);
                        content.space(2);
                        waitForFullStock(WorkPriorityType.coinmaker_mithril);
                    }

                    void waitForFullStock(WorkPriorityType priorityType)
                    {
                        BoolGetSet_Tag property = city == null ? faction.craftOnFullStockProperty : city.craftOnFullStockProperty;

                        content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsStockpileStop) },
                            property, new RbTooltip_Text(DssRef.lang.Work_OnlyCraftOnFullStock))
                        { propertyTag = priorityType, });

                    }
                    break;
            }
        }
    }

    struct WorkPriority
    {
        public static readonly WorkPriority Empty = new WorkPriority();

        public byte value;
        public bool followFaction;
        public bool unlocked;
        public bool waitForStockpile;
        //public bool safeguard;

        public WorkPriority()
        {
            followFaction = true;
            unlocked = true;
        }

        public WorkPriority(byte defaultVal)//, bool safeguard)
        {
            followFaction = true;
            unlocked = true;
            value = defaultVal;
        }


        public void set(int value)
        { 
            this.value = (byte)value;
            followFaction = false;
        }

        public void onFactionValueChange(WorkPriority factionTemplate)
        {
            if (followFaction && unlocked)
            {
                value = factionTemplate.value;
            }
        }
        public void titleToHud(RichBoxContent content, string name, SpriteName sprite1, SpriteName sprite2)
        {
            content.Add(new RbImage(sprite1));
            if (sprite2 != SpriteName.NO_IMAGE)
            {
                content.Add(new RbImage(sprite2));
            }
            content.hspace();
            content.Add(new RbText(name, HudLib.TitleColor_Label));


        }
        public void toHud(LocalPlayer player, RichBoxContent content, string name, SpriteName sprite1, SpriteName sprite2, WorkPriorityType priorityType, Faction faction, City city,
            ItemResourceType resourceInfo, WorkViewMode viewMode = WorkViewMode.Default, ItemResourceType secondResourceType = ItemResourceType.NONE, int? mineCount = null)
        {
            content.newLine();
            var infoContent = new List<AbsRichBoxMember>(2);
            infoContent.Add(new RbImage(sprite1));
            if (sprite2 != SpriteName.NO_IMAGE)
            {
                infoContent.Add(new RbImage(sprite2));
            }
            var infoButton = new ArtButton(RbButtonStyle.HoverArea, infoContent, null, new RbTooltip(workTooltip, new WorkTooltipArgs() { Faction = player.faction, City = city, Name = name, resourceInfo = resourceInfo, mineCount = mineCount, viewMode = viewMode, secondaryResourceInfo = secondResourceType }));

            content.Add(infoButton);
            content.Add(new RbTab(0.2f));

            if (mineCount.HasValue && mineCount <= 0 && city != null)
            {
                content.Add(new RbText(DssRef.lang.Work_NoMines, HudLib.NotAvailableColor));
            }
            else
            {
                priorityToHud(player, content, priorityType, faction, city);
            }
        }

        struct WorkTooltipArgs
        {
            public Faction Faction;
            public City City;
            public string Name;

            public ItemResourceType resourceInfo;
            public ItemResourceType secondaryResourceInfo;
            public int? mineCount;
            public WorkViewMode viewMode;
        }

        void workTooltip(RichBoxContent content, object tag)
        {
            WorkTooltipArgs args = (WorkTooltipArgs)tag;
            content.h1(args.Name, HudLib.TitleColor_Head);

            if (args.viewMode == WorkViewMode.Slaughter)
            {
                var properties = ItemPropertyColl.Get(args.secondaryResourceInfo);
                properties.bp1.toMenu(content, args.City, false, true);
            }


            if (args.resourceInfo != ItemResourceType.NONE)
            {
                if (args.mineCount != null)
                {
                    IconName.Item(args.resourceInfo, out SpriteName itemIcon, out string itemName);
                    content.icontext(SpriteName.WarsWorkMine, TextLib.LargeFirstLetter(string.Format(DssRef.lang.Language_XCountIsY, string.Format(DssRef.lang.BuildingType_ResourceMine, itemName), args.mineCount.Value)));

                    content.newLine();
                }
                content.Add(new RbSeperationLine());
                ResourceLib.FullResourceInfo(args.Faction, args.City, args.resourceInfo, content);
            }
        }

        public void priorityToHud(Players.LocalPlayer player, RichBoxContent content, WorkPriorityType priorityType, Faction faction, City city)
        {
            if (unlocked)
            {
                if (city != null)
                {
                    HudLib.FollowFactionButton(followFaction,
                        faction.workTemplate.Get(priorityType).value,
                        new RbAction2Arg<WorkPriorityType, City>(faction.workFollowFactionClick, priorityType, city, followFaction ? RbSoundType.Back : RbSoundType.Default),
                        player, content);
                }

                int min = 0, max = WorkTemplate.MaxPrio;

                if (priorityType == WorkPriorityType.buildOrders)
                {
                    min = 1;
                    if (city != null)
                    {
                        max = city.MaxBuildPrio();
                    }
                }

                for (int prio = min; prio <= max; prio++)
                {
                    var button = new ArtToggle(prio == value, new List<AbsRichBoxMember> {
                                new RbText(prio.ToString())
                            },
                        new RbAction3Arg<int, WorkPriorityType, City>(faction.setWorkPrio, prio, priorityType, city, RbSoundType.Option),
                        Bound.IsWithin( prio, WorkTemplate.MinPrio +1, WorkTemplate.MaxPrio -1) ? null : new RbTooltip(prioTooltip, prio));
                    
                    content.Add(button);
                    if (prio == 0)
                    {
                        content.space();
                    }
                }
            }
            else
            {
                content.Add(new RbImage(SpriteName.birdLock));
            }
        }

        void prioTooltip(RichBoxContent content, object tag)
        {
            SpriteName icon = SpriteName.NO_IMAGE;
            string prioText = null;
            switch ((int)tag)
            {
                case WorkTemplate.NoPrio:
                    icon = SpriteName.WarsHudIconSpeed_Pause;
                    prioText = DssRef.lang.Work_OrderPrio_No;
                    break;

                case WorkTemplate.MinPrio:
                    icon = SpriteName.WarsHudIconSpeed_Low;
                    prioText = DssRef.lang.Work_OrderPrio_Min;
                    break;

                case WorkTemplate.MaxPrio:
                    icon = SpriteName.WarsHudIconSpeed_High;
                    prioText = DssRef.lang.Work_OrderPrio_Max;
                    break;
            }

            content.icontext(icon, prioText);
        }

        public void writeGameState(System.IO.BinaryWriter w, bool isCity)
        {
            EightBit eightBit = new EightBit(followFaction, waitForStockpile);
            eightBit.write(w);

            if (!isCity || !followFaction)
            {
                w.Write(value);
            }
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, bool isCity)
        {
            EightBit eightBit = new EightBit(r);
            followFaction = eightBit.Get(0);
            waitForStockpile = eightBit.Get(1);

            if (!isCity || !followFaction)
            {
                value = r.ReadByte();
            }          
        }

        public void addPrio(int add)
        {
            followFaction = false;
            value = (byte)Bound.Set(value + add, 0, WorkTemplate.MaxPrio);
        }

        public void addPrio_belowMax(int add)
        {
            followFaction = false;
            value = (byte)Bound.Set(value + add, 0, WorkTemplate.MaxPrio - 1);
        }

        public bool HasPrio()
        {
            return value > WorkTemplate.NoPrio;
        }
        public bool HasPrio_r(out byte prio)
        {
            prio = value;
            return value > WorkTemplate.NoPrio;
        }
    }

    enum WorkPriorityType
    {
        move,
        wood,
        stone,
        miningBrick,
        craftBrick,
        craftFuel,
        craftFood,
        craftConservedFood,
        craftBeer,
        craftCoolingFluid,

        smeltIron,
        smeltTin,
        smeltCopper,
        smeltLead,
        smeltSilver,
        craftBronze,
        craftCastIron,
        craftBloomeryIron,
        craftSteel,
        craftMithril,
        craftPalisade,
        craftContainer,
        craftToolkit,
        craftWagon2Wheel,
        craftWagon4Wheel,
        craftWagonClosed,
        craftWagonIron,
        craftWagonSteel,
        craftBlackPowder,
        craftGunPowder,
        craftBullet,

        craftSharpStick,
        craftBronzeSword,
        craftShortSword,
        craftSword,
        craftLongSword,
        craftHandSpear,

        craftWarhammer,
        craftTwoHandSword,
        craftMithrilSword,

        craftSlingshot,
        craftThrowingspear,
        craftBow,
        craftLongbow,
        craftCrossbow,
        craftMithrilbow,

        craftHandCannon,
        craftHandCulverin,
        craftRifle,
        craftBlunderbuss,

        craftBallista,
        craftManuBallista,
        craftCatapult,
        craftBatteringRam,
        craftSiegeCannonBronze,
        craftManCannonBronze,
        craftSiegeCannonIron,
        craftManCannonIron,

        craftPaddedArmor,
        craftHeavyPaddedArmor,
        craftBronzeArmor,
        craftMailArmor,
        craftHeavyMailArmor,
        craftPlateArmor,
        craftFullPlateArmor,
        craftMithrilArmor,

        craftMountPaddedArmor,
        craftMountHeavyPaddedArmor,
        craftMountBronzeArmor,
        craftMountMailArmor,
        craftMountHeavyMailArmor,
        craftMountPlateArmor,
        craftMountFullPlateArmor,
        craftMountMithrilArmor,

        craftBucklerShield,
        craftRoundShield,
        craftHeaterShield,
        craftTowerShield,

        farmFood,
        farmRawFood,
        farmfuel,
        farmlinen,
        bogiron,
        collectClay,
        miningIron,
        miningTin,
        miningCopper,
        miningLead,
        miningSilver,
        miningGold,
        miningMithril,
        miningSalt,
        miningSulfur,
        miningCoal,

        trading,
        autoBuild,
        buildOrders,
        //expandFarms,
        smeltGold,
        coinmaker_copper,
        coinmaker_bronze,
        coinmaker_silver,
        coinmaker_mithril,

        SlaughterHen,
        SlaughterPig,
        SlaughterOxen,
        SlaughterKineOxen,

        SlaughterPony,
        SlaughterHorse,
        SlaughterWarHorse,
        SlaughterDraftHorse,

        SlaughterWildPig,
        SlaughterWildHog,
        SlaughterWarHog,
        SlaughterStagHog,

        SlaughterWolf,
        SlaughterWarg,
        SlaughterAlphaWarg,

        SlaughterWildCat,
        SlaughterLion,
        SlaughterWarLion,

        SlaughterElephant,
        SlaughterWarElephant,
        SlaughterOliphant,

        SlaughterFowl,
        SlaughterBoar,
        
        RESERVED3,
            RESERVED4,
            RESERVED5,
            RESERVED6,
            RESERVED7,
            RESERVED8,
            RESERVED9,
            RESERVED10,


        NUM_NONE
    }

    enum WorkViewMode
    { 
        Default,
        Slaughter,
    }

}
