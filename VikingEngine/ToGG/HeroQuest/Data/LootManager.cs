using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest.Gadgets;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class LootManager
    {
        public PcgRandom random;
        public GadgetsCatalogue gadgetsCatalogue;

        LootChestQue[] lootChests;

        public LootManager()
        {
            hqRef.loot = this;

            random = new PcgRandom(toggRef.Seed + 11);
            gadgetsCatalogue = new GadgetsCatalogue();
            lootChests = new LootChestQue[(int)LootLevel.NUM_NONE];
            for (LootLevel lvl = 0; lvl < LootLevel.NUM_NONE; ++lvl)
            {
                lootChests[(int)lvl] = new LootChestQue(lvl, this);
            }
        }

        //public List<AbsItem> levelItems(LootLevel level, bool includeCoins)
        //{
        //    List<AbsItem> items = new List<AbsItem>(8);

        //    switch (level)
        //    {
        //        case LootLevel.Level1:
        //            {
        //                if (includeCoins)
        //                {
        //                    //coinPile(80, 3, new Interval(1, 4), items);
        //                }
        //                chanceCount(30, 1, ItemMainType.Potion, (int)PotionType.Healing, items);
        //                chanceCount(10, 1, ItemMainType.Potion, (int)PotionType.Stamina, items);
        //                chanceCount(60, 3, ItemMainType.Potion, (int)PotionType.Apple, items);

        //                chanceCount(40, 2, ItemMainType.QuickWeapon, (int)QuickWeaponType.ThrowingKnife, items);
        //                chanceCount(60, 1, ItemMainType.Weapon, (int)WeaponType.Spear, items);
        //                chanceCount(40, 1, ItemMainType.Weapon, (int)WeaponType.Whip, items);
        //                chanceCount(40, 2, ItemMainType.SpecialArrow, (int)ArrowSpecialType.Piercing2, items);
        //                chanceCount(50, 1, ItemMainType.Spawn, (int)ItemSpawnType.Decoy, items);
        //                chanceCount(30, 1, ItemMainType.Trinket, (int)TrinketType.ProtectionRune, items);
        //                chanceCount(30, 1, ItemMainType.Trinket, (int)TrinketType.WaterBottle, items);
        //                chanceCount(30, 1, ItemMainType.Armor, (int)ArmorType.Leather, items);
        //            }
        //            break;
        //        case LootLevel.Level2:
        //            {
        //                if (includeCoins)
        //                {
        //                   // coinPile(50, 3, new Interval(3, 6), items);
        //                }
        //                chanceCount(50, 1, ItemMainType.Potion, (int)PotionType.Healing, items);
        //                chanceCount(50, 1, ItemMainType.Potion, (int)PotionType.Stamina, items);
        //                chanceCount(5, 1, ItemMainType.Potion, (int)PotionType.SmokeBomb, items);
        //                chanceCount(50, 1, ItemMainType.Spawn, (int)ItemSpawnType.ArmoredDecoy, items);
        //                chanceCount(10, 1, ItemMainType.Spawn, (int)ItemSpawnType.RougeTrap, items);
        //                chanceCount(10, 1, ItemMainType.Spawn, (int)ItemSpawnType.TrapDecoy, items);

                        
        //                chanceCount(60, 1, ItemMainType.Weapon, (int)WeaponType.BerserkerAxe, items);
        //                chanceCount(60, 1, ItemMainType.Weapon, (int)WeaponType.ArcherDagger, items);
        //                chanceCount(60, 1, ItemMainType.Weapon, (int)WeaponType.ThrowDaggersPoisionous, items);
        //                chanceCount(60, 1, ItemMainType.Shield, (int)ShieldType.Round, items);

        //                chanceCount(60, 1, ItemMainType.Armor, (int)ArmorType.Chainmail, items);
        //                chanceCount(60, 1, ItemMainType.Armor, (int)ArmorType.Elf, items);
        //            }
        //            break;
        //        case LootLevel.Level3:
        //            {
        //                chanceCount(50, 1, ItemMainType.Potion, (int)PotionType.HealAndStamina, items);
        //                chanceCount(90, 1, ItemMainType.Weapon, (int)WeaponType.KnightSword, items);
        //                chanceCount(90, 1, ItemMainType.Weapon, (int)WeaponType.ElfBow, items);
        //                chanceCount(90, 1, ItemMainType.Weapon, (int)WeaponType.ThrowDaggersTier2, items);
        //                chanceCount(70, 1, ItemMainType.Armor, (int)ArmorType.Chainmail, items);
        //            }
        //            break;
        //    }

        //    return items;
        //}

        //void chanceCount(int chance, int maxCount, ItemMainType mainType, int subType, List<AbsItem> toList)
        //{
        //    for (int i = 0; i < maxCount; ++i)
        //    {
        //        if (random.RandomChance(chance))
        //        {
        //            toList.Add(AbsItem.createItem(mainType, subType));
        //        }
        //    }
        //}

        void coinPile(int chance, int maxCount, Range coinCount, List<AbsItem> toList)
        {
            for (int i = 0; i < maxCount; ++i)
            {
                if (random.Chance(chance))
                {
                    toList.Add(new Coins(coinCount.GetRandom(random)));
                }
            }
        }

        public void chestItems(TileItemCollection chest)
        {
            for (int i = 0; i < 2; ++i)
            {
                var lootque = lootChests[(int)chest.data.lootLevel];

                if (lootque.items.Count == 0)
                {
                    lootque.reset(this);
                }

                chest.items.Add(arraylib.PullLastMember(lootque.items));
            }
        }
    }

    class LootChestQue
    {
        LootLevel level;
        public List<AbsItem> items;

        public LootChestQue(LootLevel level, LootManager loot)
        {
            this.level = level;
            reset(loot);
        }

        public void reset(LootManager loot)
        {
            do
            {
                items = loot.gadgetsCatalogue.levelItems(level, loot.random);

                if (level > LootLevel.Level1)
                {
                    var below = loot.gadgetsCatalogue.levelItems(level -1, loot.random);//loot.levelItems(level - 1, false);

                    for (int i = 0; i < 2; ++i)
                    {
                        if (below.Count > 1)
                        {
                            items.Add(arraylib.RandomListMemberPop(below));
                        }
                    }
                }

                if (level < LootLevel.Level3)
                {
                    var above = loot.gadgetsCatalogue.levelItems(level +1, loot.random);//loot.levelItems(level + 1, false);

                    for (int i = 0; i < 1; ++i)
                    {
                        if (above.Count > 1)
                        {
                            items.Add(arraylib.RandomListMemberPop(above));
                        }
                    }
                }
            } while (items.Count <= 2);

            arraylib.Shuffle(items, loot.random);
        }
    }
}
