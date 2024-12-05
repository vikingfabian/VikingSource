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
        public const int Unlocked = 100;
        public const int FactionUnlock = 100000;

        public int advancedBuilding;
        public int advancedFarming; //hemp, pigs, toolbox
        public int advancedCasting;

        public int iron;
        public int steel;

        public int catapult;
        
        public int blackPowder;
        public int gunPowder;

        public Unlocks GetUnlocks(bool factionView)
        {
            Unlocks unlocks = new Unlocks();
            int unlockAt = factionView ? 1 : Unlocked;

            if (advancedBuilding >= unlockAt)
            {
                unlocks.UnlockAdvancedBuilding();
            }
            if (advancedFarming >= unlockAt)
            {
                unlocks.UnlockAdvancedFarming();
            }
            if (advancedCasting >= unlockAt)
            {
                unlocks.UnlockAdvancedCasting();
            }
            if (iron >= unlockAt)
            {
                unlocks.UnlockIron();
            }
            if (steel >= unlockAt)
            {
                unlocks.UnlockSteel();
            }
            if (catapult >= unlockAt)
            {
                unlocks.UnlockCatapult();
            }
            if (blackPowder >= unlockAt)
            {
                unlocks.UnlockBlackPowder();
            }
            if (gunPowder >= unlockAt)
            {
                unlocks.UnlockGunPowder();
            }

            return unlocks;
        }

        public void destroyTechOnTakeOver()
        {
            tech(ref advancedBuilding);
            tech(ref advancedFarming);
            tech(ref advancedCasting);
            tech(ref iron);
            tech(ref steel);
            tech(ref catapult);
            tech(ref blackPowder);
            tech(ref gunPowder);

            void tech(ref int thisTech)
            {
                if (thisTech > 0)
                {
                    thisTech = Math.Min(Ref.rnd.Int(thisTech), Ref.rnd.Int(thisTech));
                }
            }
        }


        public void gainTechSpread(TechnologyTemplate from, int gainSpeed)
        {
            tech(ref advancedBuilding, from.advancedBuilding);
            tech(ref advancedFarming, from.advancedFarming);
            tech(ref advancedCasting, from.advancedCasting);
            tech(ref iron, from.iron);
            if (iron >= Unlocked)
            {
                tech(ref steel, from.steel);
            }
            tech(ref catapult, from.catapult);
            tech(ref blackPowder, from.blackPowder);
            if (blackPowder >= Unlocked)
            {
                tech(ref gunPowder, from.gunPowder);
            }

            void tech(ref int thisTech, int otherTech)
            {
                if (otherTech >= Unlocked && thisTech < Unlocked)
                {
                    thisTech = Bound.Max(thisTech + gainSpeed, Unlocked);
                }
            }
        }

        public void addFactionUnlocked(TechnologyTemplate from, bool includeProgress)
        {
            tech(ref advancedBuilding, from.advancedBuilding);
            tech(ref advancedFarming, from.advancedFarming);
            tech(ref advancedCasting, from.advancedCasting);
            tech(ref iron, from.iron);
            tech(ref steel, from.steel);
            tech(ref catapult, from.catapult);
            tech(ref blackPowder, from.blackPowder);
            tech(ref gunPowder, from.gunPowder);

            void tech(ref int thisTech, int otherTech)
            {
                if (otherTech >= FactionUnlock)
                { 
                    thisTech = Unlocked;
                }
                else if (includeProgress)
                { 
                    thisTech = otherTech;
                }
            }
        }

        public void checkCityCount(int cityCount)
        {
            tech(ref advancedBuilding);
            tech(ref advancedFarming);
            tech(ref advancedCasting);
            tech(ref iron);
            tech(ref steel);
            tech(ref catapult);
            tech(ref blackPowder);
            tech(ref gunPowder);

            void tech(ref int thisTech)
            {
                if (thisTech >= cityCount)
                {
                    thisTech = FactionUnlock;
                }
            }
        }

        public void Add(TechnologyTemplate city)
        {
            advancedBuilding += city.advancedBuilding;
            advancedFarming += city.advancedFarming;
            advancedCasting += city.advancedCasting;
            iron += city.iron;
            steel += city.steel;
            blackPowder += city.blackPowder;
            gunPowder += city.gunPowder;
        }

        public static int PercentProgress(int value)
        {
            return Bound.Max(value, 100);
        }
    }

    struct Unlocks
    {
        public bool building_stoneBuildings;

        public bool item_tools;
        public bool building_mixedFarms;

        public bool item_cannon;
        public bool building_cannonBarrack;

        public bool item_castIron;
        public bool item_castMithril;

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

        public void UnlockAdvancedFarming()
        {
            item_tools = true;
            building_mixedFarms = true;
        }

        public void UnlockAdvancedCasting()
        {
            item_castIron = true;
            item_castMithril = true;
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
            if (building_mixedFarms)
            {
                builds.Add(BuildAndExpandType.HempFarm);
                builds.Add(BuildAndExpandType.PigPen);
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

            if (item_tools)
            {
                items.Add(ItemResourceType.Toolkit);
            }
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

            if (item_castIron)
            {
                items.Add(ItemResourceType.CastIron);
            }
            if (item_castMithril)
            {
                items.Add(ItemResourceType.Mithril);
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
                
            }

            if (item_gunPowder)
            {
                items.Add(ItemResourceType.GunPowder);
                items.Add(ItemResourceType.Rifle);
                items.Add(ItemResourceType.Blunderbus);                
            }

            if (item_cannon)
            {
                items.Add(ItemResourceType.SiegeCannonBronze);
                items.Add(ItemResourceType.ManCannonBronze);
                items.Add(ItemResourceType.SiegeCannonIron);
                items.Add(ItemResourceType.ManCannonIron);                
            }            

            return items;
        }

    }
}
