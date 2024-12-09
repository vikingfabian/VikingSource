using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Players
{
    partial class AiPlayer
    {
        static readonly ItemResourceType[] conscriptWeaponPrioOrder =
        {
            ItemResourceType.Sword,
            ItemResourceType.Bow,
            ItemResourceType.SharpStick
        };

        static readonly ItemResourceType[] conscriptArmorPrioOrder =
        {
            ItemResourceType.FullPlateArmor,
            ItemResourceType.IronArmor,
            ItemResourceType.PaddedArmor,
        };

        void setupConscriptAi_async(City city)
        {
            
            if (city.conscriptBuildings.Count > 0)
            {
                ItemResourceType weapon = ItemResourceType.SharpStick;
                ItemResourceType armorLevel = ItemResourceType.NONE;

                foreach (var w in conscriptWeaponPrioOrder)
                {
                    //ItemResourceType weaponItem = ConscriptProfile.WeaponItem(w);
                    int availableWeapons = city.GetGroupedResource(w).amount;

                    if (availableWeapons >= DssConst.SoldierGroup_DefaultCount)
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
