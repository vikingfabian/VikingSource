using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Conscript
{
    struct ConscriptOptions
    {
        public ItemResourceType[] AvailableShields;
        public ItemResourceType[] AvailableAnimalArmor;
        public ItemResourceType[] AvailableWagons;
        
        public ConscriptOptions(ConscriptProfile profile) 
        {
            var weaponProp = ItemPropertyColl.Get(profile.weapon);
            if (weaponProp.Filter_IsTwoHandWeapon)
            {
                AvailableShields = ConscriptDataLib.SideShield;
            }
            else
            {
                AvailableShields = ConscriptDataLib.AllShields;
            }

            if (profile.animal == ItemResourceType.NONE)
            {
                AvailableWagons = null;
                AvailableAnimalArmor = null;
            }
            else
            {
                var animalProp = ItemPropertyColl.Get(profile.animal);

                switch (animalProp.armorCarry)
                {
                    default:
                    case ArmorCarry.None:
                        AvailableAnimalArmor = null;
                        break;

                    case ArmorCarry.LightOnly:
                        AvailableAnimalArmor = ConscriptDataLib.MountArmorTypesLight;
                        break;

                    case ArmorCarry.All:
                        AvailableAnimalArmor = ConscriptDataLib.MountArmorTypes;
                        break;
                }

                switch (animalProp.wagonPull)
                {
                    default:
                    case WagonPull.None:
                        AvailableWagons = null;
                        break;

                    case WagonPull.LightOnly:
                        AvailableWagons = ConscriptDataLib.VehicleTypesLight;
                        break;

                    case WagonPull.All:
                        AvailableWagons = ConscriptDataLib.VehicleTypes;
                        break;

                    case WagonPull.Balcon:
                        if (weaponProp.Filter_IsWarMachine)
                        {
                            AvailableWagons = ConscriptDataLib.VehicleRequired;
                        }
                        else
                        {
                            AvailableWagons = ConscriptDataLib.VehicleTypes;
                        }
                        break;
                }

                if (profile.weapon == ItemResourceType.SiegeCannonBronze)
                {
                    AvailableWagons = null;
                }
            }
        }

        public void CheckLegal(ref ConscriptProfile profile)
        {
            if (!AvailableShields.Contains(profile.shield))
            {
                profile.shield = AvailableShields[0];
            }

            if (AvailableAnimalArmor == null)
            {
                profile.mountArmor = ItemResourceType.NONE;
            }
            else if (!AvailableAnimalArmor.Contains(profile.mountArmor))
            {
                profile.mountArmor = AvailableAnimalArmor[0];
            }

            if (AvailableWagons == null)
            {
                profile.vehicle = ItemResourceType.NONE;
            }
            else if (!AvailableWagons.Contains(profile.vehicle))
            {
                profile.vehicle = AvailableWagons[0];
            }
        }
    }
}
