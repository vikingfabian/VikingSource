using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.XP
{
    struct TechnologyTemplate
    {
        public const byte Unlocked = 100;

        public byte advancedBuilding;
        public byte advancedCasting;

        public byte iron;
        public byte steel;

        public byte catapult;
        
        public byte blackPowder;

        public byte gunPowder;

        public static int PercentProgress(byte value)
        {
            return Bound.Max(value, 100);
        }
    }



    struct Unlocks
    {
        public bool building_stoneBuildings;

        public bool item_cannon;
        public bool building_cannonBarrack;

        public bool item_Iron;
        public bool item_Sword;
        public bool item_IronArmor;

        public bool item_Steel;
        public bool item_LongSword;
        public bool item_SteelArmor;

        public bool item_catapult;
        public bool item_crossbow;

        public bool item_blackPowder;
        public bool building_gunBarrack;
        public bool building_chemist;

        public bool item_gunPowder;

        public void UnlockAdvancedBuilding()
        {
            building_stoneBuildings = true;
        }

        public void UnlockAdvancedCasting()
        {
            item_cannon = true;
            building_cannonBarrack = true;
        }

        public void UnlockIron()
        {
            item_Iron = true;
            item_Sword = true;
            item_IronArmor = true;
        }

        public void UnlockSteel()
        {
            item_LongSword = true;
            item_Steel = true;
            item_SteelArmor = true;
        }

        public void UnlockCatapult()
        {
            item_catapult = true;
            item_crossbow = true;
        }

        public void UnlockBlackPowder()
        {
            building_chemist = true;
            item_blackPowder = true;
        }

        public void UnlockGunPowder()
        {
            item_gunPowder = true;
        }

        public List<BuildAndExpandType> ListBuildings()
        { 
            List<BuildAndExpandType> builds = new List<BuildAndExpandType>();
            if (building_stoneBuildings)
            {
                builds.Add(BuildAndExpandType.Nobelhouse);
                builds.Add(BuildAndExpandType.Bank);
            }

            if (building_cannonBarrack)
            {
                builds.Add(BuildAndExpandType.GunBarracks);
                builds.Add(BuildAndExpandType.CannonBarracks);
            }

            if (building_chemist)
            {
                builds.Add(BuildAndExpandType.Chemist);
            }

            return builds;
        }

        public List<ItemResourceType> ListItems()
        {
            
            List <ItemResourceType> items = new List < ItemResourceType >();

            

            if (item_Iron)
            {
                items.Add(ItemResourceType.Iron_G);
            }
            if (item_Sword)
            {
                items.Add(ItemResourceType.Sword);
            }
            if (item_IronArmor)
            {
                items.Add(ItemResourceType.IronArmor);
            }

            if (item_Steel)
            {
                items.Add(ItemResourceType.Steel);
            }
            if (item_LongSword)
            {
                items.Add(ItemResourceType.LongSword);
            }
            if (item_SteelArmor)
            {
                items.Add(ItemResourceType.FullPlateArmor);
            }

            if (item_catapult)
            {
                items.Add(ItemResourceType.Manuballista);
                items.Add(ItemResourceType.Catapult);
            }
            if (item_crossbow)
            {
                items.Add(ItemResourceType.Crossbow);
            }

            if (item_blackPowder)
            {
                items.Add(ItemResourceType.BlackPowder);
                items.Add(ItemResourceType.HandCannon);
                items.Add(ItemResourceType.HandCulverin);
                items.Add(ItemResourceType.ManCannonBronze);
                items.Add(ItemResourceType.SiegeCannonBronze);

            }

            return items;
        }

    }
}
