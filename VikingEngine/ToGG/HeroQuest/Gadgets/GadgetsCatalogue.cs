using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class GadgetsCatalogue
    {
        CatalogueMember[] members;

        public GadgetsCatalogue()
        {
            members = new CatalogueMember[]
            {
                new CatalogueMember(ItemMainType.Armor, (int)ArmorType.Chainmail, HeroClass.None, 
                    0, 0, 1, 60, 1, 70),
                new CatalogueMember(ItemMainType.Armor, (int)ArmorType.Elf, HeroClass.None, 
                    0, 0, 1, 60, 0, 0),
                new CatalogueMember(ItemMainType.Armor, (int)ArmorType.Leather, HeroClass.None, 
                    1, 70, 0, 0, 0, 0),

                new CatalogueMember(ItemMainType.RuneKey, (int)RuneKeyType.Hera),
                new CatalogueMember(ItemMainType.RuneKey, (int)RuneKeyType.Bast),
                new CatalogueMember(ItemMainType.RuneKey, (int)RuneKeyType.Froe),
                new CatalogueMember(ItemMainType.RuneKey, (int)RuneKeyType.Ami),

                new CatalogueMember(ItemMainType.Potion, (int)PotionType.Apple, HeroClass.None,
                    3, 60, 0, 0, 0, 0),
                new CatalogueMember(ItemMainType.Potion, (int)PotionType.HealAndStamina, HeroClass.None,
                    0, 0, 0, 0, 1, 50),
                new CatalogueMember(ItemMainType.Potion, (int)PotionType.Healing, HeroClass.None,
                    1, 30, 1, 50, 0, 0),
                new CatalogueMember(ItemMainType.Potion, (int)PotionType.SmokeBomb, HeroClass.None,
                    0, 0, 1, 5, 0, 0),
                new CatalogueMember(ItemMainType.Potion, (int)PotionType.Stamina, HeroClass.None,
                    1, 10, 1, 50, 0, 0),

                new CatalogueMember(ItemMainType.QuickWeapon, (int)QuickWeaponType.ThrowingKnife, HeroClass.None,
                    2, 40, 0, 0, 0, 0),

                 new CatalogueMember(ItemMainType.Shield, (int)ShieldType.Round, HeroClass.None,
                    0, 0, 1, 60, 0, 0),

                 new CatalogueMember(ItemMainType.Spawn, (int)ItemSpawnType.ArmoredDecoy, HeroClass.None,
                    0, 0, 1, 50, 0, 0),
                 new CatalogueMember(ItemMainType.Spawn, (int)ItemSpawnType.Decoy, HeroClass.None,
                    1, 50, 0, 0, 0, 0),
                 new CatalogueMember(ItemMainType.Spawn, (int)ItemSpawnType.RougeTrap, HeroClass.None,
                    0, 0, 1, 10, 0, 0),
                 new CatalogueMember(ItemMainType.Spawn, (int)ItemSpawnType.TrapDecoy, HeroClass.None,
                    0, 0, 1, 10, 1, 10),

                 new CatalogueMember(ItemMainType.SpecialArrow, (int)ArrowSpecialType.Piercing2, HeroClass.Archer,
                    2, 40, 0, 0, 0, 0),

                 new CatalogueMember(ItemMainType.Trinket, (int)TrinketType.ProtectionRune, HeroClass.None,
                    1, 30, 1, 50, 0, 0),
                 new CatalogueMember(ItemMainType.Trinket, (int)TrinketType.WaterBottle, HeroClass.None,
                    1, 30, 0, 0, 0, 0),

                 new CatalogueMember(ItemMainType.Weapon, (int)WeaponType.ArcherDagger, HeroClass.None,
                    0, 0, 1, 60, 0, 0),
                new CatalogueMember(ItemMainType.Weapon, (int)WeaponType.BaseDagger),
                new CatalogueMember(ItemMainType.Weapon, (int)WeaponType.BerserkerAxe, HeroClass.None,
                    0, 0, 1, 60, 0, 0),
                new CatalogueMember(ItemMainType.Weapon, (int)WeaponType.ElfBow, HeroClass.None,
                    0, 0, 0, 0, 1, 90),
                new CatalogueMember(ItemMainType.Weapon, (int)WeaponType.KnightSword, HeroClass.None,
                    0, 0, 0, 0, 1, 90),
                new CatalogueMember(ItemMainType.Weapon, (int)WeaponType.Spear, HeroClass.None,
                    1, 60, 0, 0, 0, 0),
                new CatalogueMember(ItemMainType.Weapon, (int)WeaponType.StartingBow),
                new CatalogueMember(ItemMainType.Weapon, (int)WeaponType.StartingSword),
                new CatalogueMember(ItemMainType.Weapon, (int)WeaponType.StartingThrowDaggers),
                new CatalogueMember(ItemMainType.Weapon, (int)WeaponType.ThrowDaggersPoisionous, HeroClass.None,
                    0, 0, 1, 60, 0, 0),
                new CatalogueMember(ItemMainType.Weapon, (int)WeaponType.ThrowDaggersTier2, HeroClass.None,
                    0, 0, 0, 0, 1, 90),
                new CatalogueMember(ItemMainType.Weapon, (int)WeaponType.Whip, HeroClass.None,
                    1, 40, 0, 0, 0, 0),

                //0, 0, 0, 0, 0, 0
            };
        }

        public List<AbsItem> levelItems(LootLevel level, PcgRandom random)
        {
            List<AbsItem> items = new List<AbsItem>(8);
            int count, chance;

            foreach (var m in members)
            {
                m.tierSpawn(level, out count, out chance);

                for (int i = 0; i < count; ++i)
                {
                    if (random.Chance(chance))
                    {
                        items.Add(m.create());
                    }
                }
            }

            return items;
        }

        public List<AbsItem> listAll()
        {
            List<AbsItem> items = new List<AbsItem>(members.Length);
            
            foreach (var m in members)
            {
               items.Add(m.create());
            }

            return items;
        }

    }

    struct CatalogueMember
    {
        public ItemMainType type;
        public int utype;

        HeroClass classSpecific;

        int tier1ChestCount;
        int tier1ChestChance;
        int tier2ChestCount;
        int tier2ChestChance;
        int tier3ChestCount;
        int tier3ChestChance;


        public CatalogueMember(ItemMainType type, int utype)
            : this()
        {
            this.type = type;
            this.utype = utype;
        }

        public CatalogueMember(ItemMainType type, int utype, HeroClass classSpecific,
            int tier1ChestCount,
            int tier1ChestChance,
            int tier2ChestCount,
            int tier2ChestChance,
            int tier3ChestCount,
            int tier3ChestChance)
        {
            this.type = type;
            this.utype = utype;

            this.classSpecific = classSpecific;

            this.tier1ChestCount = tier1ChestCount;
            this.tier1ChestChance = tier1ChestChance;
            this.tier2ChestCount = tier1ChestCount;
            this.tier2ChestChance = tier1ChestChance;
            this.tier3ChestCount = tier1ChestCount;
            this.tier3ChestChance = tier1ChestChance;
        }

        public void tierSpawn(LootLevel level, out int count, out int chance)
        {
            switch (level)
            {
                default:
                    count = tier1ChestCount;
                    chance = tier1ChestChance;
                    break;
                case LootLevel.Level2:
                    count = tier2ChestCount;
                    chance = tier2ChestChance;
                    break;
                case LootLevel.Level3:
                    count = tier3ChestCount;
                    chance = tier3ChestChance;
                    break;
            }
        }

        public AbsItem create()
        {
            return AbsItem.createItem(type, utype);
        }
    }
}
