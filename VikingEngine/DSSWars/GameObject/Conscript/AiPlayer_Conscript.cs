using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Conscript;
using VikingEngine.DSSWars.GameObject.Resource;

namespace VikingEngine.DSSWars.Players
{
    partial class AiPlayer
    {
        static readonly MainWeapon[] conscriptWeaponPrioOrder =
        {
            MainWeapon.Sword,
            MainWeapon.Bow,
            MainWeapon.SharpStick
        };

        static readonly ArmorLevel[] conscriptArmorPrioOrder =
        {
            ArmorLevel.Heavy,
            ArmorLevel.Medium,
            ArmorLevel.Light,
        };

        void setupConscriptAi_async(City city)
        {
            
            if (city.conscriptBuildings.Count > 0)
            {
                MainWeapon weapon = 0;
                ArmorLevel armorLevel = ArmorLevel.None;

                foreach (var w in conscriptWeaponPrioOrder)
                {
                    ItemResourceType weaponItem = ConscriptProfile.WeaponItem(w);
                    int availableWeapons = city.GetGroupedResource(weaponItem).amount;

                    if (availableWeapons >= DssConst.SoldierGroup_DefaultCount)
                    {
                        weapon = w;
                        break;
                    }
                }

                foreach (var a in conscriptArmorPrioOrder)
                {
                    ItemResourceType armorItem = ConscriptProfile.ArmorItem(a);
                    int availableArmor = city.GetGroupedResource(armorItem).amount;

                    if (availableArmor >= DssConst.SoldierGroup_DefaultCount)
                    {
                        armorLevel = a;
                        break;
                    }
                }

                lock (city.conscriptBuildings)
                {
                    if (city.conscriptBuildings.Count > 0)
                    {
                        city.conscriptBuildings[0] = new BarracksStatus() 
                        { 
                            profile = new ConscriptProfile()
                            { 
                                weapon = weapon,
                                armorLevel = armorLevel,
                                training = TrainingLevel.Basic,
                            }
                        };
                    }
                }
            }
        }

        //int canConscriptCount()
        //{ 

        //}

        virtual protected bool buySoldiers(City city, bool aggresive, bool commit)
        {
            if (!aggresive && city.workForce < city.workForceMax - DssConst.SoldierGroup_DefaultCount)
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
            }

            ItemResourceType weaponItem = ConscriptProfile.WeaponItem(profile.weapon);
            ItemResourceType armorItem = ConscriptProfile.ArmorItem(profile.armorLevel);

            int availableWeapons = city.GetGroupedResource(weaponItem).amount / DssConst.SoldierGroup_DefaultCount;
            int availableArmors = city.GetGroupedResource(armorItem).amount / DssConst.SoldierGroup_DefaultCount;
            int availableMen = (city.workForce / DssConst.SoldierGroup_DefaultCount) - 1;

            int get = lib.SmallestValue(availableArmors, availableWeapons, availableMen, city.conscriptBuildings.Count * 2);

            if (commit && get > 0)
            {
                city.AddGroupedResource(weaponItem, -get * DssConst.SoldierGroup_DefaultCount);
                city.AddGroupedResource(armorItem, -get * DssConst.SoldierGroup_DefaultCount);

                switch (aiConscript)
                { 
                    case AiConscript.Orcs:
                        switch (weaponItem)
                        { 
                            case ItemResourceType.Bow:
                                profile.weapon = MainWeapon.CrossBow;
                                break;
                            case ItemResourceType.Sword:
                                profile.weapon = MainWeapon.Pike;
                                break;
                        }
                        break;

                    case AiConscript.Viking:
                        profile.specialization = SpecializationType.Viking;
                        break;

                    case AiConscript.DragonSlayer:
                        switch (weaponItem)
                        {
                            case ItemResourceType.Bow:
                                profile.weapon = MainWeapon.CrossBow;
                                break;
                            case ItemResourceType.Sword:
                                profile.weapon = MainWeapon.Ballista;
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
