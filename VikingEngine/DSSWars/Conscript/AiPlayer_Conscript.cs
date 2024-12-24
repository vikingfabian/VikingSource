using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Players
{
    struct AutoWeaponOption
    {
        public ItemResourceType item;
        public bool frontline;
        public BuildAndExpandType barracks;

        public AutoWeaponOption(ItemResourceType weapon, bool frontline, BuildAndExpandType barracks)
        { 
            this.item = weapon;
            this.frontline = frontline;
            this.barracks = barracks;
        }
    }

    partial class AiPlayer
    {
        static readonly AutoWeaponOption[] conscriptWeaponPrioOrder =
        {
            new AutoWeaponOption(ItemResourceType.MithrilSword, true, BuildAndExpandType.KnightsBarracks),
            new AutoWeaponOption( ItemResourceType.MithrilBow,false, BuildAndExpandType.KnightsBarracks),
             new AutoWeaponOption(ItemResourceType.KnightsLance,true, BuildAndExpandType.KnightsBarracks),
             new AutoWeaponOption(ItemResourceType.TwoHandSword,true, BuildAndExpandType.KnightsBarracks),
             new AutoWeaponOption(ItemResourceType.Warhammer,true, BuildAndExpandType.KnightsBarracks),

             new AutoWeaponOption(ItemResourceType.LongSword,true, BuildAndExpandType.SoldierBarracks),
             new AutoWeaponOption(ItemResourceType.Sword,true, BuildAndExpandType.SoldierBarracks),

             new AutoWeaponOption(ItemResourceType.Blunderbus,true, BuildAndExpandType.GunBarracks),
             new AutoWeaponOption(ItemResourceType.Rifle,false, BuildAndExpandType.GunBarracks),
            new AutoWeaponOption( ItemResourceType.HandCulverin,true, BuildAndExpandType.GunBarracks),
             new AutoWeaponOption(ItemResourceType.HandCannon,false, BuildAndExpandType.GunBarracks),
             new AutoWeaponOption(ItemResourceType.Crossbow,false, BuildAndExpandType.ArcherBarracks),

             new AutoWeaponOption(ItemResourceType.ManCannonIron,false, BuildAndExpandType.CannonBarracks),
             new AutoWeaponOption(ItemResourceType.ManCannonBronze,false, BuildAndExpandType.CannonBarracks),
             new AutoWeaponOption(ItemResourceType.SiegeCannonIron,false, BuildAndExpandType.CannonBarracks),
             new AutoWeaponOption(ItemResourceType.SiegeCannonIron,false, BuildAndExpandType.CannonBarracks),
             new AutoWeaponOption(ItemResourceType.Catapult,false, BuildAndExpandType.WarmashineBarracks),
             new AutoWeaponOption(ItemResourceType.Ballista,false, BuildAndExpandType.WarmashineBarracks),

             new AutoWeaponOption(ItemResourceType.LongBow,false, BuildAndExpandType.ArcherBarracks),
             new AutoWeaponOption(ItemResourceType.Manuballista,false, BuildAndExpandType.WarmashineBarracks),
             new AutoWeaponOption(ItemResourceType.ShortSword,true, BuildAndExpandType.SoldierBarracks),
             new AutoWeaponOption(ItemResourceType.BronzeSword,true, BuildAndExpandType.SoldierBarracks),
             new AutoWeaponOption(ItemResourceType.ThrowingSpear,true, BuildAndExpandType.ArcherBarracks),
             new AutoWeaponOption(ItemResourceType.Bow,false, BuildAndExpandType.ArcherBarracks),
            new AutoWeaponOption( ItemResourceType.SharpStick,true, BuildAndExpandType.SoldierBarracks),
        };

        static readonly ItemResourceType[] conscriptArmorPrioOrder =
        {
            ItemResourceType.MithrilArmor,
            ItemResourceType.FullPlateArmor,
            ItemResourceType.LightPlateArmor,
            ItemResourceType.HeavyIronArmor,
            ItemResourceType.IronArmor,
            ItemResourceType.BronzeArmor,
            ItemResourceType.HeavyPaddedArmor,
            ItemResourceType.PaddedArmor,
        };

        void setupConscriptAi_async(City city)
        {
            
            if (city.res_food.amount > 20 &&
                city.conscriptBuildings.Count > 0)
            {
                AutoWeaponOption weapon = new AutoWeaponOption(ItemResourceType.SlingShot, false, BuildAndExpandType.ArcherBarracks);
                ItemResourceType armorLevel = ItemResourceType.NONE;

                foreach (var w in conscriptWeaponPrioOrder)
                {
                    
                    if (city.GetGroupedResource(w.item).amount >= DssConst.SoldierGroup_DefaultCount &&
                        city.buildingStructure.getBarracksCount(w.barracks) > 0)
                    {
                        weapon = w;
                        break;
                    }
                }

                foreach (var a in conscriptArmorPrioOrder)
                {
                    //ItemResourceType armorItem = ConscriptProfile.ArmorItem(a);
                    int availableArmor = city.GetGroupedResource(a).amount;

                    if (availableArmor >= DssConst.SoldierGroup_DefaultCount)
                    {
                        armorLevel = a;
                        break;
                    }
                }

                lock (city.conscriptBuildings)
                {
                    for (int i = 0; i < city.conscriptBuildings.Count; ++i)//each (var c in city.conscriptBuildings)
                    {
                        if (city.conscriptBuildings[i].type == weapon.barracks)
                        {
                            var conscript = city.conscriptBuildings[i];
                            conscript.profile = new ConscriptProfile()
                            {
                                weapon = weapon.item,
                                armorLevel = armorLevel,
                                training = TrainingLevel.Basic,
                            };
                            city.conscriptBuildings[i] = conscript;
                        }
                    }
                    //if (city.conscriptBuildings.Count > 0)
                    //{
                    //    city.conscriptBuildings[0] = new BarracksStatus() 
                    //    { 
                    //        profile = new ConscriptProfile()
                    //        { 
                    //            weapon = weapon.item,
                    //            armorLevel = armorLevel,
                    //            training = TrainingLevel.Basic,
                    //        }
                    //    };
                    //}
                }
            }
        }

        //int canConscriptCount()
        //{ 

        //}

        virtual protected bool buySoldiers(City city, bool aggresive, bool commit)
        {
            if (!aggresive && city.workForce.amount < city.workForceMax - DssConst.SoldierGroup_DefaultCount)
            {
                return false;
            }

            if (!commit)
            {
                setupConscriptAi_async(city);
            }

            ConscriptProfile profile = new ConscriptProfile();

            lock (city.conscriptBuildings)
            {
                if (city.conscriptBuildings.Count > 0)
                {
                    profile = city.conscriptBuildings[0].profile;
                }
                else
                {
                    return false;
                }
            }

            if (profile.weapon == ItemResourceType.NONE)
            { 
                return false;
            }

            //ItemResourceType weaponItem = ConscriptProfile.WeaponItem(profile.weapon);
            //ItemResourceType armorItem = ConscriptProfile.ArmorItem(profile.armorLevel);

            int availableWeapons = city.GetGroupedResource(profile.weapon).amount / DssConst.SoldierGroup_DefaultCount;
            int availableArmors = city.GetGroupedResource(profile.armorLevel).amount / DssConst.SoldierGroup_DefaultCount;
            int availableMen = (city.workForce.amount / DssConst.SoldierGroup_DefaultCount) - 1;

            int get = lib.SmallestValue(availableArmors, availableWeapons, availableMen, city.conscriptBuildings.Count * 2);

            if (commit && get > 0)
            {
                city.AddGroupedResource(profile.weapon, -get * DssConst.SoldierGroup_DefaultCount);
                city.AddGroupedResource(profile.armorLevel, -get * DssConst.SoldierGroup_DefaultCount);
                city.workForce.amount -= get * DssConst.SoldierGroup_DefaultCount;

                switch (aiConscript)
                { 
                    case AiConscript.Orcs:
                        switch (profile.weapon)
                        { 
                            case ItemResourceType.Bow:
                                profile.weapon = ItemResourceType.Crossbow;
                                break;
                            case ItemResourceType.Sword:
                                profile.weapon = ItemResourceType.Pike;
                                break;
                        }
                        break;

                    case AiConscript.Viking:
                        profile.specialization = SpecializationType.Viking;
                        break;

                    case AiConscript.DragonSlayer:
                        switch (profile.weapon)
                        {
                            case ItemResourceType.Bow:
                                profile.weapon = ItemResourceType.Crossbow;
                                break;
                            case ItemResourceType.Sword:
                                profile.weapon = ItemResourceType.Ballista;
                                break;
                        }
                        profile.specialization = SpecializationType.Siege;
                        break;

                    case AiConscript.Green:
                        profile.specialization = SpecializationType.Green;
                        profile.training = TrainingLevel.Skillful;
                        break;
                }

                
                city.conscriptArmy(profile, city.defaultConscriptPos(), get);
            }

            return get > 0;
        }
    }
}
