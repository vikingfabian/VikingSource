using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;

namespace VikingEngine.DSSWars.Conscript
{
    struct ConscriptUnitCount
    {
        public int groupUnitCount;
        public int menPerUnit;
        //public int totalMen;
        public int weaponsPerUnit;
        public int animalsPerUnit;
        public bool seperateAnimalUnit;
        public int vehiclesPerUnit;

        public ConscriptUnitCount(ConscriptProfile conscript)
        {
            animalsPerUnit = 0;
            vehiclesPerUnit = 0;
            weaponsPerUnit = 1;
            menPerUnit = 1;
            var weaponProp = Resource.ItemPropertyColl.Get(conscript.weapon);

            if (conscript.vehicle != Resource.ItemResourceType.NONE)
            {
                vehiclesPerUnit = 1;
                animalsPerUnit = 2;
                groupUnitCount = Resource.ItemPropertyColl.WagonRowWidth * Resource.ItemPropertyColl.WagonColumnDepth;

                if (weaponProp.Filter_IsSiegeWeapon)
                {
                    //The wagon is one big weapon
                    menPerUnit = 2;
                }
                else
                {
                    //Carries soldiers
                    menPerUnit = 4;
                    weaponsPerUnit = 4;
                }

            }
            else if (conscript.animal != Resource.ItemResourceType.NONE)
            {
                animalsPerUnit = 1;
                var animalProp = Resource.ItemPropertyColl.Get(conscript.animal);
                seperateAnimalUnit = !animalProp.Filter_IsRidingAnimal;

                if (seperateAnimalUnit)
                {
                    groupUnitCount = weaponProp.soldierData.UnitCount() / 2;
                }
                else
                {
                    groupUnitCount = animalProp.soldierData.UnitCount();
                }

                if (weaponProp.Filter_IsSiegeWeapon)
                {
#if DEBUG
                    throw new Exception();
#endif
                }
            }
            else
            {
                groupUnitCount = weaponProp.soldierData.UnitCount(conscript.specialization == SpecializationType.CityGuard);
                if (weaponProp.Filter_IsSiegeWeapon)
                {
                    //The wagon is one big weapon
                    menPerUnit = 2;
                }
            }
        }
    }
}
