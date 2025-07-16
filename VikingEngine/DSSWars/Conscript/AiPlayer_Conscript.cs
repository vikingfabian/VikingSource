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
        static readonly AutoWeaponOption[] ConscriptWeaponPrioOrder =
        {
            new AutoWeaponOption(ItemResourceType.MithrilSword, true, BuildAndExpandType.KnightsBarracks),
            new AutoWeaponOption( ItemResourceType.MithrilBow,false, BuildAndExpandType.KnightsBarracks),
             new AutoWeaponOption(ItemResourceType.KnightsLance,true, BuildAndExpandType.KnightsBarracks),
             new AutoWeaponOption(ItemResourceType.TwoHandSword,true, BuildAndExpandType.KnightsBarracks),
             new AutoWeaponOption(ItemResourceType.Warhammer,true, BuildAndExpandType.KnightsBarracks),

             new AutoWeaponOption(ItemResourceType.LongSword,true, BuildAndExpandType.SoldierBarracks),
             new AutoWeaponOption(ItemResourceType.Sword,true, BuildAndExpandType.SoldierBarracks),

             new AutoWeaponOption(ItemResourceType.Blunderbuss,true, BuildAndExpandType.GunBarracks),
             new AutoWeaponOption(ItemResourceType.Rifle,false, BuildAndExpandType.GunBarracks),
            new AutoWeaponOption( ItemResourceType.HandCulverin,true, BuildAndExpandType.GunBarracks),
             new AutoWeaponOption(ItemResourceType.HandCannon,false, BuildAndExpandType.GunBarracks),
             new AutoWeaponOption(ItemResourceType.Crossbow,false, BuildAndExpandType.ArcherBarracks),

             new AutoWeaponOption(ItemResourceType.ManCannonIron,false, BuildAndExpandType.CannonBarracks),
             new AutoWeaponOption(ItemResourceType.ManCannonBronze,false, BuildAndExpandType.CannonBarracks),
             new AutoWeaponOption(ItemResourceType.SiegeCannonIron,false, BuildAndExpandType.CannonBarracks),
             new AutoWeaponOption(ItemResourceType.SiegeCannonIron,false, BuildAndExpandType.CannonBarracks),
             new AutoWeaponOption(ItemResourceType.Catapult,false, BuildAndExpandType.WarmachineBarracks),
             new AutoWeaponOption(ItemResourceType.Ballista,false, BuildAndExpandType.WarmachineBarracks),

             new AutoWeaponOption(ItemResourceType.LongBow,false, BuildAndExpandType.ArcherBarracks),
             new AutoWeaponOption(ItemResourceType.Manuballista,false, BuildAndExpandType.WarmachineBarracks),
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

       

        int setupConscriptAi_async(City city, bool aggresive, out ConscriptProfile profile, out int manCount, out int unitCount)
        {
            if (city.myIndex == 500)
            {
                lib.DoNothing();
            }

            bool guard = false;
            if (!aggresive && city.AvailableGuardHousing() >= DssConst.SoldierGroup_GuardCount)
            {
                lock (city.defenceBuildings.array)
                {
                    for (int i = 0; i < city.defenceBuildings.Count; i++)
                    {
                        if (city.defenceBuildings[i].AvailableForAutoAssign())
                        {
                            guard = true;
                            break;
                        }
                    }
                }
            }

            manCount = 0;
            unitCount = 0;

            if (AutoConscriptLib.HasEnoughFood(city) &&
                city.conscriptBuildings.Count > 0)
            {
                AutoWeaponOption weapon = new AutoWeaponOption(ItemResourceType.NONE, false, BuildAndExpandType.SoldierBarracks);
                ItemResourceType armorLevel = ItemResourceType.NONE;

                foreach (var w in ConscriptWeaponPrioOrder)
                {
                    unitCount = ItemPropertyColl.Get(w.item).soldierData.UnitCount(guard);

                    if (city.GetGroupedResource(w.item).amount >= unitCount &&
                        city.buildingStructure.getBarracksCount(w.barracks) > 0 &&
                        AutoConscriptLib.MayUseItemInConscript(city, w.item, true))
                    {  
                        weapon = w;                        
                        break;
                    }
                }

                manCount = ItemPropertyColl.Get(weapon.item).soldierData.workForceCount(guard);

                foreach (var a in conscriptArmorPrioOrder)
                {
                    int availableArmor = city.GetGroupedResource(a).amount;

                    if (availableArmor >= unitCount)
                    {
                        armorLevel = a;
                        break;
                    }
                }

                if (weapon.item == ItemResourceType.NONE ||
                    !AutoConscriptLib.MayUseItemInConscript(city, armorLevel, false))                   
                {
                    //Item is too low quality
                    profile = ConscriptProfile.Empty;
                    return 0;
                }

                profile = new ConscriptProfile()
                {
                    weapon = weapon.item,
                    armorLevel = armorLevel,
                    training = TrainingLevel.Basic,
                    specialization = guard? SpecializationType.CityGuard : SpecializationType.None,
                };

                
                lock (city.conscriptBuildings)
                {
                    for (int i = 0; i < city.conscriptBuildings.Count; ++i)//each (var c in city.conscriptBuildings)
                    {
                        if (city.conscriptBuildings[i].type == weapon.barracks)
                        {
                            ++manCount;
                            var conscript = city.conscriptBuildings[i];
                            conscript.profile = profile;
                            city.conscriptBuildings[i] = conscript;
                        }
                    }
                }
                
            }
            else
            {
                profile = new ConscriptProfile();
            }

            return manCount;
        }

        virtual protected bool buySoldiers(City city, bool aggresive, bool commit)
        {
            if (!aggresive && !AutoConscriptLib.HasEnoughMen(city))//city.workForce.amount < city.HousingCount_Workers - DssConst.SoldierGroup_DefaultCount)
            {
                return false;
            }

            int barracksCount = setupConscriptAi_async(city, aggresive, out ConscriptProfile profile, out int manCount, out int unitCount);
           
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

                var aiPlayer = city.GetPlayer().GetAiPlayer();

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

                city.nextAutoConscriptTime.setTimeFromNow(DssConst.TrainingTimeSec_Basic / barracksCount);
            }

            return get > 0;
        }
    }
}
