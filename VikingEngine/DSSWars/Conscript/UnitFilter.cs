using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Resource;
using VikingEngine.ToGG.HeroQuest.Gadgets;

namespace VikingEngine.DSSWars.Conscript
{
    struct UnitFilter
    {
        public ThirtyTwoBit value;

        public void Add(UnitFilterType type)
        {
            value.Set((int)type, true);
        }
        public void Remove(UnitFilterType type)
        {
            value.Set((int)type, false);
        }
        public bool Contains(UnitFilterType type)
        {
           return value.Get((int)type);
        }

        public bool RangedNotWarMachine()
        {
            return value.Get((int)UnitFilterType.Ranged) && !value.Get((int)UnitFilterType.WarMachine);
        }
        public bool MeleeNotWarMachine()
        {
            return value.Get((int)UnitFilterType.Melee) && !value.Get((int)UnitFilterType.WarMachine);
        }

        
    }

    static class UnitFilterLib
    {
        public static void Init()
        {
            int weaponSortValue = 0;
            foreach (var res in Resource.ResourceLib.MovableCityResource_WeaponMelee)
            {
                ItemPropertyColl.Get(res).UnitSortValue = weaponSortValue;
                weaponSortValue += 10_000;
            }
            foreach (var res in Resource.ResourceLib.MovableCityResource_WeaponRanged)
            {
                ItemPropertyColl.Get(res).UnitSortValue = weaponSortValue;
                weaponSortValue += 10_000;
            }

            int animalSortValue = 0;
            foreach (var res in Resource.ResourceLib.MovableCityResource_Animals)
            {
                ItemPropertyColl.Get(res).UnitSortValue = animalSortValue;
                animalSortValue += 100;
            }

            int armorSortValue = 0;
            foreach (var res in Resource.ResourceLib.MovableCityResource_Armor)
            {
                ItemPropertyColl.Get(res).UnitSortValue = armorSortValue;
                armorSortValue += 1;
            }
        }
    }

    enum UnitFilterType
    {
        Melee,
        Ranged,

        FootSoldier,
        Animal,
        AnimalCompanion,
        AnimalRider,
        WagonRider,
        WarMachine,

        Primitive,
        GunPowder,
    }
}
