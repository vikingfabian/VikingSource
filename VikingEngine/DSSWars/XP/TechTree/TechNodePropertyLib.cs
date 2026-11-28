using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.DSSWars.XP.TechTree
{
    struct TechNodeProperty
    {   
        public string name;
        public SpriteName sprite;

        public TechNodeProperty(string name, SpriteName sprite)
        {
            this.name = name;
            this.sprite = sprite;
        }

    }
    static class TechNodePropertyLib
    {
        static TechNodeProperty[] NodeProperties;

        public static TechNodeProperty Get(TechNodeType nodeType)
        {
            return NodeProperties[(int)nodeType];
        }

        public static void Init()
        {
            NodeProperties = new TechNodeProperty[(int)TechNodeType.NUM_NONE];

            //BUILDINGS
            NodeProperties[(int)TechNodeType.Logistics] = new TechNodeProperty("Logistics", SpriteName.WarsBuild_Logistics);
            NodeProperties[(int)TechNodeType.CraftBuildings] = new TechNodeProperty("Craft Buildings", SpriteName.WarsBuild_Armory);
            NodeProperties[(int)TechNodeType.Logistics2] = new TechNodeProperty("Logistics 2", SpriteName.WarsBuild_Logistics);
            NodeProperties[(int)TechNodeType.CityHall_village] = new TechNodeProperty("Village City Hall", SpriteName.WarsBuild_WorkerHuts);
            NodeProperties[(int)TechNodeType.CityHall_town] = new TechNodeProperty("Town City Hall", SpriteName.WarsBuild_WorkerHutLarge);
            NodeProperties[(int)TechNodeType.CityHall_capital] = new TechNodeProperty("Capital City Hall", SpriteName.WarsBuild_GreatHall);
            NodeProperties[(int)TechNodeType.ImmigrantTent] = new TechNodeProperty("Immigrant Tent", SpriteName.WarsBuild_Tent);
            NodeProperties[(int)TechNodeType.Delivery] = new TechNodeProperty("Delivery", SpriteName.WarsBuild_Postal);
            NodeProperties[(int)TechNodeType.DeliveryUpgraded] = new TechNodeProperty("Delivery Upgraded", SpriteName.WarsBuild_PostalLevel3);
            NodeProperties[(int)TechNodeType.CommonHall] = new TechNodeProperty("Common Hall", SpriteName.WarsBuild_Tavern);
            NodeProperties[(int)TechNodeType.LordSeat1] = new TechNodeProperty("Lord Seat 1", SpriteName.WarsCityHall);
            NodeProperties[(int)TechNodeType.LordSeat2] = new TechNodeProperty("Lord Seat 2", SpriteName.WarsCityHall);
            NodeProperties[(int)TechNodeType.LordSeat3] = new TechNodeProperty("Lord Seat 3", SpriteName.WarsCityHall);
            NodeProperties[(int)TechNodeType.Stakes] = new TechNodeProperty("Stakes", SpriteName.MissingImage);
            NodeProperties[(int)TechNodeType.WoodWall] = new TechNodeProperty("Wood Wall", SpriteName.WarsBuild_WoodWall);
            NodeProperties[(int)TechNodeType.StoneWall] = new TechNodeProperty("Stone Wall", SpriteName.WarsBuild_StoneWall);
            NodeProperties[(int)TechNodeType.NobelHouse] = new TechNodeProperty("Nobel House", SpriteName.WarsBuild_Nobelhouse);
            NodeProperties[(int)TechNodeType.Embassy] = new TechNodeProperty("Embassy", SpriteName.WarsBuild_Embassy);
            NodeProperties[(int)TechNodeType.Bank] = new TechNodeProperty("Bank", SpriteName.WarsBuild_Bank);
            NodeProperties[(int)TechNodeType.GravelRoad] = new TechNodeProperty("Gravel Road", SpriteName.DssMapTextureGravelRoad1);
            NodeProperties[(int)TechNodeType.CobblestoneRoad] = new TechNodeProperty("Cobblestone Road", SpriteName.DssMapTextureCobbleRoad1);
            NodeProperties[(int)TechNodeType.PavedRoad] = new TechNodeProperty("Paved Road", SpriteName.DssMapTexturePavedRoad1);
            NodeProperties[(int)TechNodeType.DecorRoad] = new TechNodeProperty("Decor Road", SpriteName.WarsBuild_Pavement);

            //FARM
            NodeProperties[(int)TechNodeType.Trapper] = new TechNodeProperty("Trapper", SpriteName.WarsBuild_Trapper);
            NodeProperties[(int)TechNodeType.PenTier1] = new TechNodeProperty("Pen Tier 1", SpriteName.WarsBuild_BoarPen);
            NodeProperties[(int)TechNodeType.PenTier2] = new TechNodeProperty("Pen Tier 2", SpriteName.WarsBuild_PigPen);
            NodeProperties[(int)TechNodeType.DraftPen] = new TechNodeProperty("Draft Pen", SpriteName.WarsBuild_DraftHorsePen);
            NodeProperties[(int)TechNodeType.WarPen] = new TechNodeProperty("War Pen", SpriteName.WarsBuild_WarHorsePen);
            NodeProperties[(int)TechNodeType.WildPenTier1] = new TechNodeProperty("Wild Pen Tier 1", SpriteName.WarsBuild_WolfPen);
            NodeProperties[(int)TechNodeType.WildPenTier2] = new TechNodeProperty("Wild Pen Tier 2", SpriteName.WarsBuild_WargPen);
            NodeProperties[(int)TechNodeType.WildWarPen] = new TechNodeProperty("Wild War Pen", SpriteName.WarsBuild_AlphaWargPen);
            NodeProperties[(int)TechNodeType.BasePlant] = new TechNodeProperty("Base Plant", SpriteName.WarsBuild_WheatFarms);
            NodeProperties[(int)TechNodeType.Hemp] = new TechNodeProperty("Hemp", SpriteName.WarsBuild_HempFarms);
            NodeProperties[(int)TechNodeType.TreePlant] = new TechNodeProperty("Tree Plant", SpriteName.WarsBuild_TreeSeedlingHard);
            NodeProperties[(int)TechNodeType.UpgradedFarm] = new TechNodeProperty("Upgraded Farm", SpriteName.WarsBuild_WheatFarms);
            NodeProperties[(int)TechNodeType.GardenGrass] = new TechNodeProperty("Garden Grass", SpriteName.WarsBuild_GardenGrass);
            NodeProperties[(int)TechNodeType.GardenPlants] = new TechNodeProperty("Garden Plants", SpriteName.WarsBuild_GardenLongTree);

            //CAST
            NodeProperties[(int)TechNodeType.CastCopper] = new TechNodeProperty("Cast Copper", SpriteName.WarsResource_Copper);
            NodeProperties[(int)TechNodeType.CastBronze] = new TechNodeProperty("Cast Bronze", SpriteName.WarsResource_Bronze);
            NodeProperties[(int)TechNodeType.CastBronzeGun] = new TechNodeProperty("Cast Bronze Gun", SpriteName.WarsResource_BronzeRifle);
            NodeProperties[(int)TechNodeType.CastBronzeCannon] = new TechNodeProperty("Cast Bronze Cannon", SpriteName.WarsResource_BronzeManCannon);
            NodeProperties[(int)TechNodeType.CastIron] = new TechNodeProperty("Cast Iron", SpriteName.WarsResource_CastIron);
            NodeProperties[(int)TechNodeType.CastIronGun] = new TechNodeProperty("Cast Iron Gun", SpriteName.WarsResource_IronRifle);
            NodeProperties[(int)TechNodeType.CastIronCannon] = new TechNodeProperty("Cast Iron Cannon", SpriteName.WarsResource_IronManCannon);
            NodeProperties[(int)TechNodeType.CastSilverGold] = new TechNodeProperty("Cast Silver Gold", SpriteName.WarsResource_Gold);
            NodeProperties[(int)TechNodeType.CastTinLead] = new TechNodeProperty("Cast Tin Lead", SpriteName.WarsResource_Tin);
            NodeProperties[(int)TechNodeType.CastMithril] = new TechNodeProperty("Cast Mithril", SpriteName.WarsResource_Mithril);

            //METAL
            NodeProperties[(int)TechNodeType.ironProduction] = new TechNodeProperty("Iron Production", SpriteName.WarsResource_Iron);
            NodeProperties[(int)TechNodeType.steelProduction] = new TechNodeProperty("Steel Production", SpriteName.WarsResource_Steel);
            NodeProperties[(int)TechNodeType.ironCraft_base] = new TechNodeProperty("Iron Craft Base", SpriteName.WarsBuild_Smith);
            NodeProperties[(int)TechNodeType.shortsword] = new TechNodeProperty("Shortsword", SpriteName.WarsResource_ShortSword);
            NodeProperties[(int)TechNodeType.sword] = new TechNodeProperty("Sword", SpriteName.WarsResource_Sword);
            NodeProperties[(int)TechNodeType.longsword] = new TechNodeProperty("Longsword", SpriteName.WarsResource_Longsword);
            NodeProperties[(int)TechNodeType.steelsword] = new TechNodeProperty("Steel Sword", SpriteName.MissingImage); // Not assigned in metalFieldStructure
            NodeProperties[(int)TechNodeType.zweihänder] = new TechNodeProperty("Zweihänder", SpriteName.WarsResource_TwoHandSword);
            NodeProperties[(int)TechNodeType.HeaterShield] = new TechNodeProperty("Heater Shield", SpriteName.WarsResource_HeaterShield);
            NodeProperties[(int)TechNodeType.BucklerShield] = new TechNodeProperty("Buckler Shield", SpriteName.WarsResource_BucklerShield);
            NodeProperties[(int)TechNodeType.TowerShield] = new TechNodeProperty("Tower Shield", SpriteName.WarsResource_TowerShield);
            NodeProperties[(int)TechNodeType.heavyPaddedArmor] = new TechNodeProperty("Heavy Padded Armor", SpriteName.MissingImage); // Not assigned in metalFieldStructure
            NodeProperties[(int)TechNodeType.lightIronArmor] = new TechNodeProperty("Light Iron Armor", SpriteName.WarsResource_IronArmor);
            NodeProperties[(int)TechNodeType.heavyIronArmor] = new TechNodeProperty("Heavy Iron Armor", SpriteName.WarsResource_HeavyIronArmor);
            NodeProperties[(int)TechNodeType.steelarmor] = new TechNodeProperty("Steel Armor", SpriteName.WarsResource_LightPlateArmor);
            NodeProperties[(int)TechNodeType.warhammer] = new TechNodeProperty("Warhammer", SpriteName.WarsResource_Warhammer);
            NodeProperties[(int)TechNodeType.heavySteelarmor] = new TechNodeProperty("Heavy Steel Armor", SpriteName.WarsResource_FullPlateArmor);
            NodeProperties[(int)TechNodeType.mechanics_base] = new TechNodeProperty("Mechanics Base", SpriteName.IconMenuSettings);
            NodeProperties[(int)TechNodeType.wagon4] = new TechNodeProperty("Wagon 4", SpriteName.WarsResource_Wagon4Wheel);
            NodeProperties[(int)TechNodeType.wagonClosed] = new TechNodeProperty("Wagon Closed", SpriteName.WarsResource_WagonClosed);
            NodeProperties[(int)TechNodeType.ironwagon] = new TechNodeProperty("Iron Wagon", SpriteName.WarsResource_WagonIron);
            NodeProperties[(int)TechNodeType.steelwagon] = new TechNodeProperty("Steel Wagon", SpriteName.WarsResource_WagonSteel);
            NodeProperties[(int)TechNodeType.toolkit] = new TechNodeProperty("Toolkit", SpriteName.WarsResource_Toolkit);
            NodeProperties[(int)TechNodeType.NutLockMechanic] = new TechNodeProperty("Nut Lock Mechanic", SpriteName.MissingImage);
            NodeProperties[(int)TechNodeType.MatchLockMechanic] = new TechNodeProperty("Match Lock Mechanic", SpriteName.MissingImage);

            //WOOD
            NodeProperties[(int)TechNodeType.woodBase] = new TechNodeProperty("Wood Base", SpriteName.WarsBuild_Carpenter);
            NodeProperties[(int)TechNodeType.javelin] = new TechNodeProperty("Javelin", SpriteName.WarsResource_ThrowSpear);
            NodeProperties[(int)TechNodeType.shortBow] = new TechNodeProperty("Short Bow", SpriteName.WarsResource_ShortSword);
            NodeProperties[(int)TechNodeType.LongBow] = new TechNodeProperty("Long Bow", SpriteName.WarsResource_Longbow);
            NodeProperties[(int)TechNodeType.CrossBow] = new TechNodeProperty("Crossbow", SpriteName.WarsResource_Crossbow);
            NodeProperties[(int)TechNodeType.ballista] = new TechNodeProperty("Ballista", SpriteName.WarsResource_Ballista);
            NodeProperties[(int)TechNodeType.catapult] = new TechNodeProperty("Catapult", SpriteName.WarsResource_Catapult);
            NodeProperties[(int)TechNodeType.manuballista] = new TechNodeProperty("Manuballista", SpriteName.WarsResource_Manuballista);

            //CHEMISTRY
            NodeProperties[(int)TechNodeType.coalPit] = new TechNodeProperty("Coal Pit", SpriteName.WarsBuild_CoalPit);
            NodeProperties[(int)TechNodeType.foodConservation] = new TechNodeProperty("Food Conservation", SpriteName.WarsResource_ConservedFood);
            NodeProperties[(int)TechNodeType.foodStorage] = new TechNodeProperty("Food Storage", SpriteName.WarsBuild_FoodStorage);
            NodeProperties[(int)TechNodeType.beer] = new TechNodeProperty("Beer", SpriteName.WarsResource_Beer);
            NodeProperties[(int)TechNodeType.coolingFluid] = new TechNodeProperty("Cooling Fluid", SpriteName.WarsResource_CoolingFluid);
            NodeProperties[(int)TechNodeType.blackPowder] = new TechNodeProperty("Black Powder", SpriteName.WarsResource_BlackPowder);
            NodeProperties[(int)TechNodeType.gunPowder] = new TechNodeProperty("Gun Powder", SpriteName.WarsResource_GunPowder);
        }

    
    }
}
