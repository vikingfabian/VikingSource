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

    partial class AbsPlayer
    {
        protected static readonly AutoWeaponOption[] ConscriptWeaponPrioOrder =
        {
             new AutoWeaponOption(ItemResourceType.MithrilSword, true, BuildAndExpandType.SoldierBarracks),
             new AutoWeaponOption(ItemResourceType.MithrilBow,false, BuildAndExpandType.ArcherBarracks),
             //new AutoWeaponOption(ItemResourceType.KnightsLance,true, BuildAndExpandType.KnightsBarracks),
             new AutoWeaponOption(ItemResourceType.TwoHandSword,true, BuildAndExpandType.SoldierBarracks),
             new AutoWeaponOption(ItemResourceType.Warhammer,true, BuildAndExpandType.SoldierBarracks),

             new AutoWeaponOption(ItemResourceType.LongSword,true, BuildAndExpandType.SoldierBarracks),
             new AutoWeaponOption(ItemResourceType.Sword,true, BuildAndExpandType.SoldierBarracks),

             new AutoWeaponOption(ItemResourceType.Blunderbuss,true, BuildAndExpandType.GunBarracks),
             new AutoWeaponOption(ItemResourceType.Rifle,false, BuildAndExpandType.GunBarracks),
             new AutoWeaponOption(ItemResourceType.HandCulverin,true, BuildAndExpandType.GunBarracks),
             new AutoWeaponOption(ItemResourceType.HandCannon,false, BuildAndExpandType.GunBarracks),
             new AutoWeaponOption(ItemResourceType.Crossbow,false, BuildAndExpandType.ArcherBarracks),
             new AutoWeaponOption(ItemResourceType.LongBow,false, BuildAndExpandType.ArcherBarracks),

             new AutoWeaponOption(ItemResourceType.ManCannonIron,false, BuildAndExpandType.CannonBarracks),
             new AutoWeaponOption(ItemResourceType.ManCannonBronze,false, BuildAndExpandType.CannonBarracks),
             new AutoWeaponOption(ItemResourceType.SiegeCannonIron,false, BuildAndExpandType.CannonBarracks),
             new AutoWeaponOption(ItemResourceType.SiegeCannonIron,false, BuildAndExpandType.CannonBarracks),
             new AutoWeaponOption(ItemResourceType.Catapult,false, BuildAndExpandType.WarmachineBarracks),
             new AutoWeaponOption(ItemResourceType.Manuballista,false, BuildAndExpandType.WarmachineBarracks),
             
             new AutoWeaponOption(ItemResourceType.ShortSword,true, BuildAndExpandType.SoldierBarracks),
             new AutoWeaponOption(ItemResourceType.BronzeSword,true, BuildAndExpandType.SoldierBarracks),
             new AutoWeaponOption(ItemResourceType.ThrowingSpear,true, BuildAndExpandType.ArcherBarracks),
             new AutoWeaponOption(ItemResourceType.Bow,false, BuildAndExpandType.ArcherBarracks),
             new AutoWeaponOption(ItemResourceType.Ballista,false, BuildAndExpandType.WarmachineBarracks),

             new AutoWeaponOption( ItemResourceType.SlingShot,false, BuildAndExpandType.ArcherBarracks),
             new AutoWeaponOption( ItemResourceType.SharpStick,true, BuildAndExpandType.SoldierBarracks),
        };

        protected static readonly ItemResourceType[] conscriptShieldPrioOrder =
        {
            ItemResourceType.HeaterShield,
            ItemResourceType.TowerShield,
            ItemResourceType.RoundShield,
            ItemResourceType.BucklerShield,
        };

        protected static readonly ItemResourceType[] conscriptArmorPrioOrder =
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

       

        protected static readonly ItemResourceType[] conscriptMountPrioOrder =
         {
            ItemResourceType.Oliphant,
            ItemResourceType.WarElephant,
            ItemResourceType.AlphaWarg,
            ItemResourceType.WarLion,
            ItemResourceType.WarHog,
            ItemResourceType.WarHorse,

            ItemResourceType.Elephant,
            ItemResourceType.Warg,
            ItemResourceType.Lion,
            ItemResourceType.WildHog,
            ItemResourceType.Horse,

            ItemResourceType.Hound,

            ItemResourceType.Wolf,
            ItemResourceType.WildCat,
            ItemResourceType.WildPig,
            ItemResourceType.Pony,

            ItemResourceType.Dog,
            
            ItemResourceType.DraftHorse,
            ItemResourceType.StagHog,
        };
 protected static readonly ItemResourceType[] conscriptMountArmorPrioOrder =
         {
            ItemResourceType.MountMithrilArmor,
            ItemResourceType.MountFullPlateArmor,
            ItemResourceType.MountLightPlateArmor,
            ItemResourceType.MountHeavyIronArmor,
            ItemResourceType.MountIronArmor,
            ItemResourceType.MountBronzeArmor,
            ItemResourceType.MountHeavyPaddedArmor,
            ItemResourceType.MountPaddedArmor,
        };

        protected static readonly ItemResourceType[] conscriptVehiclePrioOrder =
         {
             ItemResourceType.WagonSteel,
              ItemResourceType.WagonIron,
               ItemResourceType.WagonClosed,
                ItemResourceType.Wagon4Wheel,
                 ItemResourceType.Wagon2Wheel,
        };
        

        void setupConscriptAi_async(City city, bool aggresive, out ConscriptProfile profile, out BuildAndExpandType barracksType, out int barracksCount, out int manCount, out int unitCount)
        {
            //if (city.myIndex == 500)
            //{
            //    lib.DoNothing();
            //}

            bool guard = false;

            int minGuardCount = 2 + (int)city.cityType * 2;

            if ((!aggresive || city.groups.Count < minGuardCount) && city.AvailableGuardHousing() >= DssConst.SoldierGroup_GuardCount)
            {
                lock (city.defenceBuildings.array)
                {
                    for (int i = 0; i < city.defenceBuildings.Count; i++)
                    {
                        if (city.defenceBuildings[i].AvailableForAutoAssign(city, IsBot()))
                        {
                            guard = true;
                            break;
                        }
                    }
                }
            }

            manCount = 0;
            unitCount = 0;
            barracksCount = 0;
            barracksType = BuildAndExpandType.NUM_NONE;

            if (AutoConscriptLib.HasEnoughFoodAndGold(pfaction.GetFaction(), city, guard, aggresive) &&
                city.conscriptBuildings.Count > 0)
            {
                AutoWeaponOption weapon = new AutoWeaponOption(ItemResourceType.NONE, false, BuildAndExpandType.SoldierBarracks);
                //ItemResourceType armorLevel = ItemResourceType.NONE;

                foreach (var w in ConscriptWeaponPrioOrder)
                {
                    unitCount = ItemPropertyColl.Get(w.item).soldierData.UnitCount(guard);

                    if (city.GetGroupedResource(w.item).amount >= unitCount &&
                        city.buildingStructure.getBarracksCount(w.barracks) > 0 &&
                        AutoConscriptLib.MayUseItemInConscript(city, w.item, true, guard))
                    {  
                        weapon = w;                        
                        break;
                    }
                }

                profile = new ConscriptProfile()
                {
                    weapon = weapon.item,
                    //armorLevel = armorLevel,
                    training = TrainingLevel.Basic,
                    specialization = guard ? SpecializationType.CityGuard : SpecializationType.None,
                };

                if (weapon.item == ItemResourceType.NONE)
                {
                    profile = ConscriptProfile.Empty;
                    return;
                }

                var weaponProp = ItemPropertyColl.Get(weapon.item);
                manCount = weaponProp.soldierData.workForceCount(guard);

                if (weaponProp.Filter_IsTwoHandWeapon)
                {
                    if (city.GetGroupedResource(ItemResourceType.BucklerShield).amount >= unitCount)
                    {
                        profile.shield = ItemResourceType.BucklerShield;
                    }
                }
                else
                {
                    foreach (var shield in conscriptShieldPrioOrder)
                    {
                        if (city.GetGroupedResource(shield).amount >= unitCount)
                        {
                            profile.shield = shield;
                            break;
                        }
                    }
                }

                foreach (var a in conscriptArmorPrioOrder)
                {
                    int availableArmor = city.GetGroupedResource(a).amount;

                    if (availableArmor >= unitCount)
                    {
                        profile.armorLevel = a;
                        break;
                    }
                }

                if (weapon.item == ItemResourceType.NONE ||
                    !AutoConscriptLib.MayUseItemInConscript(city, profile.armorLevel, false, guard))                   
                {
                    //Item is too low quality
                    profile = ConscriptProfile.Empty;
                    return;
                }

                if (!guard)
                {
                    foreach (var animal in conscriptMountPrioOrder)
                    {
                        if (city.GetGroupedResource(animal).amount >= unitCount)
                        {
                            profile.animal = animal;
                            break;
                        }
                    }

                    if (profile.animal != ItemResourceType.NONE)
                    {
                        foreach (var mountArmor in conscriptMountArmorPrioOrder)
                        {
                            if (city.GetGroupedResource(mountArmor).amount >= unitCount)
                            {
                                profile.mountArmor = mountArmor;
                                break;
                            }
                        }

                        foreach (var vehicle in conscriptVehiclePrioOrder)
                        {
                            if (city.GetGroupedResource(vehicle).amount >= unitCount)
                            {
                                profile.vehicle = vehicle;
                                break;
                            }
                        }
                    }
                }

                Conscript.ConscriptOptions conscriptOptions = new ConscriptOptions(profile);
                conscriptOptions.CheckLegal(ref profile);
                //profile = new ConscriptProfile()
                //{
                //    weapon = weapon.item,
                //    armorLevel = armorLevel,
                //    training = TrainingLevel.Basic,
                //    specialization = guard? SpecializationType.CityGuard : SpecializationType.None,
                //};

                barracksType = weapon.barracks;

                lock (city.conscriptBuildings)
                {
                    for (int i = 0; i < city.conscriptBuildings.Count; ++i)
                    {
                        if (city.conscriptBuildings[i].type == weapon.barracks)
                        {
                            ++barracksCount;
                            var conscript = city.conscriptBuildings[i];
                            conscript.profile = profile;
                            conscript.checkSpecialization();
                            city.conscriptBuildings[i] = conscript;
                        }
                    }
                }
                
            }
            else
            {
                profile = new ConscriptProfile();
            }
        }

        protected bool buySoldiersBalanceCheck_asynch(City city, bool aggresive, double overrideChance, out bool guardOnly)
        {
            guardOnly = false;

            if (!Ref.rnd.Chance(overrideChance))
            {
                if (aggressionLevel == AggressionLevel0_Passive)
                {
                    guardOnly = true;
                }
                else
                {
                    float multiply = 0.2f + 0.3f * aggressionLevel;
                    if (aggresive)
                    {
                        multiply += 0.5f;
                    }

                    int maxCount = Convert.ToInt32(city.workForce.amount * multiply);

                    var armiesC = pfaction.GetFaction().armies.counter();
                    while (armiesC.Next())
                    {
                        if (DssRef.world.tileGrid.Get(armiesC.sel.tilePos).CityIndex == city.myIndex)
                        {
                            maxCount -= armiesC.sel.soldiersCount;
                            if (maxCount < 0)
                            {
                               guardOnly = true;
                                break;
                            }
                        }
                    }
                }
            }
            return buySoldiers(city, aggresive, guardOnly, false);
        }

        virtual protected bool buySoldiers(City city, bool aggresive, bool guardOnly, bool commit)
        {
            
            if (!aggresive && !AutoConscriptLib.HasEnoughMen(city))
            {
                return false;
            }

            if (city.pfaction != pfaction)
            {
                return false;
            }

            setupConscriptAi_async(city, aggresive, out ConscriptProfile profile, out BuildAndExpandType barracksType, out int barracksCount, out int manCount, out int unitCount);

            if (guardOnly && profile.specialization != SpecializationType.CityGuard)
            {
                return false;
            }

           
            if (profile.weapon == ItemResourceType.NONE ||
                barracksCount == 0)
            { 
                return false;
            }

            int availableWeapons = city.GetGroupedResource(profile.weapon).amount / unitCount;
            int availableArmors = city.GetGroupedResource(profile.armorLevel).amount / unitCount;
            int availableMen = (city.workForce.amount / manCount) - 1;

            int get = lib.SmallestValue(availableArmors, availableWeapons, availableMen, barracksCount);

            if (city.nextAutoConscriptTime.TimeOut() && commit && get > 0)
            {
                city.AddGroupedResource(profile.weapon, -get * unitCount);
                city.AddGroupedResource(profile.armorLevel, -get * unitCount);
                city.workForce.amount -= get * manCount;

                var aiPlayer = city.pfaction.GetPlayer().GetAiPlayer();

                if (aiPlayer != null)
                {
                    switch (aiPlayer.aiConscript)
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
                }
                
                city.conscriptArmy(profile, city.defaultConscriptPos(), get);

                city.nextAutoConscriptTime.setTimeFromNow(ConscriptProfile.TrainingTime(profile.training, profile.animal, barracksType) / barracksCount);
            }

            return get > 0;
        }
    }
}
