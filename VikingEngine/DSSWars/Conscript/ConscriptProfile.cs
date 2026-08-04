
using System;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.PlayerControls.Casual;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Conscript
{
    struct ConscriptProfile
    {
        
        public static readonly ConscriptProfile Empty = new ConscriptProfile();
        
        public ItemResourceType man;
        public ItemResourceType weapon;
        public ItemResourceType shield;
        public ItemResourceType armorLevel;
        public ItemResourceType animal;
        public ItemResourceType mountArmor;
        public ItemResourceType vehicle;
        public TrainingLevel training;
        public SpecializationType specialization;

        public ConscriptProfile()
        {
            man = ItemResourceType.Men;
            weapon = ItemResourceType.SharpStick;
            shield = ItemResourceType.NONE;
            armorLevel = ItemResourceType.NONE;

            animal = ItemResourceType.NONE;
            mountArmor = ItemResourceType.NONE;
            vehicle = ItemResourceType.NONE;

            training = 0;
            specialization = SpecializationType.None;
        }
        public bool Equals(ConscriptProfile other)
        {
            bool result = man == other.man &&
                   weapon == other.weapon &&
                   shield == other.shield &&
                   armorLevel == other.armorLevel &&
                   animal == other.animal &&
                   mountArmor == other.mountArmor &&
                   vehicle == other.vehicle &&
                   training == other.training &&
                   specialization == other.specialization;

            if (!result)
            {
                lib.DoNothing();
            }

            return result;
        }

        public int menCost()
        {
            SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
            {
                conscript = this,
            };
            var data = SoldierProfile.createSoldierData();
            return data.workForceCount();
        }

        public float copperUpkeepPerSoldier()
        {
            var result = DssConst.TrainingCopperUpkeep[(int)training];
            if (man == ItemResourceType.NobleMen)
            {
                result += DssConst.Nobel_GoldUpkeep;
            }
            return result;
        }

        public bool isKnight()
        {
            if (man == ItemResourceType.NobleMen)
            {
                switch (animal)
                {
                    case ItemResourceType.Horse:
                    case ItemResourceType.WarHorse:
                    case ItemResourceType.WildCat:
                    case ItemResourceType.Lion:
                    case ItemResourceType.WarLion:
                    case ItemResourceType.Wolf:
                    case ItemResourceType.Warg:
                    case ItemResourceType.AlphaWarg:
                        return true;
                }
            }

            return false;
        }

        public UnitFilter classify()//out bool ranged, out bool rangedMan, out bool meleeMan, out bool warmachine, out bool animalCompanion, out bool animalMount, out bool wagonRide)
        {
            UnitFilter unitFilter = new UnitFilter();
            if (animal == ItemResourceType.NONE)
            {
                unitFilter.Add(UnitFilterType.FootSoldier);
            }
            else
            {
                unitFilter.Add(UnitFilterType.Animal);
                switch (animal)
                {
                    case ItemResourceType.Pig:
                    case ItemResourceType.Dog:
                    case ItemResourceType.Hound:
                        unitFilter.Add(UnitFilterType.AnimalCompanion);
                        //animalCompanion = true;
                        //animalMount = false;
                        break;

                    default:
                        unitFilter.Add(UnitFilterType.AnimalRider);
                        //animalCompanion = false;
                        //animalMount = true;
                        break;
                }
            }

            if (vehicle != ItemResourceType.NONE)
            {
                if (unitFilter.Contains(UnitFilterType.AnimalRider))
                {
                    //animalMount = false;
                    //wagonRide = true;
                    unitFilter.Remove(UnitFilterType.AnimalRider);
                    unitFilter.Add(UnitFilterType.WagonRider);
                }
                //else
                //{
                //    wagonRide = false;
                //}
            }

            switch (weapon)
            {
                case ItemResourceType.Settler:
                case ItemResourceType.SharpStick:
                case ItemResourceType.BronzeSword:
                case ItemResourceType.ShortSword:
                case ItemResourceType.Sword:
                case ItemResourceType.LongSword:
                case ItemResourceType.HandSpear:
                case ItemResourceType.Pike:
                case ItemResourceType.Warhammer:
                case ItemResourceType.TwoHandSword:
                //case ItemResourceType.KnightsLance:
                case ItemResourceType.MithrilSword:
                    //ranged = false;
                    //rangedMan = false;
                    //meleeMan = true;

                    //warmachine = false;
                    unitFilter.Add(UnitFilterType.Melee);
                    break;

                case ItemResourceType.SlingShot:
                case ItemResourceType.ThrowingSpear:
                case ItemResourceType.Bow:
                case ItemResourceType.LongBow:
                case ItemResourceType.Crossbow:

                case ItemResourceType.HandCannon:
                case ItemResourceType.HandCulverin:
                case ItemResourceType.Rifle:
                case ItemResourceType.Blunderbuss:
                case ItemResourceType.MithrilBow:
                    //ranged = true;
                    //rangedMan = true;
                    //meleeMan = false;

                    //warmachine = false;
                    unitFilter.Add(UnitFilterType.Ranged);
                    break;

                



                case ItemResourceType.Ballista:
                case ItemResourceType.Manuballista:
                case ItemResourceType.Catapult:

                case ItemResourceType.SiegeCannonBronze:
                case ItemResourceType.ManCannonBronze:
                case ItemResourceType.SiegeCannonIron:
                case ItemResourceType.ManCannonIron:
                    //ranged = true;
                    //rangedMan = false;
                    //meleeMan = false;

                    //warmachine = true;
                    unitFilter.Add(UnitFilterType.Ranged);
                    unitFilter.Add(UnitFilterType.WarMachine);
                    break;

                case ItemResourceType.UN_BatteringRam:
                    //ranged = false;
                    //rangedMan = false;
                    //meleeMan = false;

                    //warmachine = true;
                    unitFilter.Add(UnitFilterType.Melee);
                    unitFilter.Add(UnitFilterType.WarMachine);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return unitFilter;
        }

        public double armySpeedBonus(bool land)
        {
            if (land)
            {
                switch (weapon)
                {
                    //case ItemResourceType.KnightsLance:
                    //    return 0.8;
                    case ItemResourceType.Ballista:
                    case ItemResourceType.Manuballista:
                    case ItemResourceType.Catapult:
                    case ItemResourceType.SiegeCannonBronze:
                    case ItemResourceType.SiegeCannonIron:
                    case ItemResourceType.ManCannonBronze:
                    case ItemResourceType.ManCannonIron:
                        return -0.5;
                }
            }
            else
            {
                if (specialization == SpecializationType.Sea)
                    return 0.4;
                else if (specialization == SpecializationType.Viking)
                    return 0.6;
            }

            return 0;
        }

        public string TypeName()
        {
            string name = null;

            switch (specialization)
            {
                case SpecializationType.HonorGuard:
                    name = DssRef.lang.UnitType_HonorGuard; break;
                case SpecializationType.Viking:
                    name = DssRef.lang.UnitType_Viking; break;
                case SpecializationType.Green:
                    name = DssRef.lang.UnitType_GreenSoldier; break;
                case SpecializationType.DarkLord:
                    name = DssRef.lang.UnitType_DarkLord; break;

                default:
                    switch (weapon)
                    {
                        case ItemResourceType.Settler:
                            name = DssRef.lang.UnitType_Settler; break;

                        case ItemResourceType.SharpStick:
                            name = DssRef.lang.UnitType_Folkman; break;
                        case ItemResourceType.Pike:
                            name = DssRef.lang.UnitType_Pikeman; break;

                        case ItemResourceType.BronzeSword:
                        case ItemResourceType.ShortSword:
                        case ItemResourceType.Sword:
                        case ItemResourceType.LongSword:
                            name = DssRef.lang.UnitType_Soldier; break;

                        case ItemResourceType.HandSpear:
                            name =  DssRef.lang.UnitType_SpearAndShield; break;

                        case ItemResourceType.Warhammer:
                            name = DssRef.lang.UnitType_Warhammer; break;
                        //case ItemResourceType.KnightsLance:
                        //    name = DssRef.lang.UnitType_CavalryKnight; break;
                        case ItemResourceType.TwoHandSword:
                            name = DssRef.lang.UnitType_FootKnight; break;
                        case ItemResourceType.MithrilSword:
                            name = DssRef.lang.UnitType_MithrilSwordsman; break;
                        case ItemResourceType.MithrilBow:
                            name = DssRef.lang.UnitType_MithrilArcher; break;

                        case ItemResourceType.SlingShot:
                            name = DssRef.lang.Resource_TypeName_SlingShot; break;
                        case ItemResourceType.ThrowingSpear:
                            name = DssRef.lang.Resource_TypeName_ThrowingSpear; break;
                        case ItemResourceType.Bow:
                        case ItemResourceType.LongBow:
                            name = DssRef.lang.UnitType_Archer; break;
                        case ItemResourceType.Crossbow:
                            name = DssRef.lang.UnitType_Crossbow; break;

                        case ItemResourceType.HandCannon:
                            name = DssRef.lang.Resource_TypeName_HandCannon; break;
                        case ItemResourceType.HandCulverin:
                            name = DssRef.lang.Resource_TypeName_HandCulverin; break;
                        case ItemResourceType.Rifle:
                            name = DssRef.lang.Resource_TypeName_Rifle; break;
                        case ItemResourceType.Blunderbuss:
                            name = DssRef.lang.Resource_TypeName_Blunderbuss; break;


                        case ItemResourceType.Ballista:
                            name = DssRef.lang.UnitType_Ballista; break;
                        case ItemResourceType.Manuballista:
                            name = DssRef.lang.Resource_TypeName_Manuballista; break;
                        case ItemResourceType.Catapult:
                            name = DssRef.lang.Resource_TypeName_Catapult; break;
                        case ItemResourceType.UN_BatteringRam:
                            name = DssRef.lang.Resource_TypeName_BatteringRam; break;

                        case ItemResourceType.SiegeCannonBronze:
                            name = DssRef.lang.Resource_TypeName_SiegeCannonBronze; break;
                        case ItemResourceType.ManCannonBronze:
                            name = DssRef.lang.Resource_TypeName_ManCannonBronze; break;
                        case ItemResourceType.SiegeCannonIron:
                            name = DssRef.lang.Resource_TypeName_SiegeCannonIron; break;
                        case ItemResourceType.ManCannonIron:
                            name = DssRef.lang.Resource_TypeName_ManCannonIron; break;


                        default:
                            name = TextLib.Error; break;
                    }
                    break;
            }

            //switch (animal)
            //{
            //    case ItemResourceType.NONE:
            //        break;
            //    //case ItemResourceType.Pig:
            //    //case ItemResourceType.Dog:
            //    //case ItemResourceType.Hound:
            //    //    break;

            //    case ItemResourceType.Pony:
            //    case ItemResourceType.Horse:
            //    case ItemResourceType.WarHorse:
            //    case ItemResourceType.DraftHorse:
            //        if (vehicle == ItemResourceType.NONE)
            //        {
            //            name += " .horse rider";
            //        }
            //        else
            //        {
            //            name += " .wagon";
            //        }
            //        break;

            //    case ItemResourceType.WildPig:
            //    case ItemResourceType.WildHog:
            //    case ItemResourceType.WarHog:
            //    case ItemResourceType.StagHog:
            //        if (vehicle == ItemResourceType.NONE)
            //        {
            //            name += " .hog rider";
            //        }
            //        else
            //        {
            //            name += " .wagon";
            //        }
            //        break;

            //    case ItemResourceType.Elephant:
            //    case ItemResourceType.WarElephant:
            //    case ItemResourceType.Oliphant:
            //        if (vehicle == ItemResourceType.NONE)
            //        {
            //            name += " .elephant rider";
            //        }
            //        else
            //        {
            //            name += " .howdah";
            //        }
            //        break;
            //}

            if (animal != ItemResourceType.NONE)
            {
                var animalProp = ItemPropertyColl.Get(animal);
                if (animalProp.Filter_IsRidingAnimal)
                {
                    if (vehicle == ItemResourceType.NONE || animalProp.wagonPull == WagonPull.Balcon)
                    {
                        name = string.Format(DssRef.lang.UnitType_UnitOnMount, name);
                    }
                    else
                    {
                        name = string.Format(DssRef.lang.UnitType_UnitOnWagon, name);
                    }
                }
                else
                {
                    IconName.Item(animal, out _, out string animalName);
                    name = string.Format(DssRef.lang.UnitType_LeashAnimalHandler, name, animalName);
                }
            }

            if (man == ItemResourceType.NobleMen)
            {
                name = string.Format(DssRef.lang.UnitType_NobelUnit, name);
            }

            return TextLib.LargeFirstLetter(name);
        }

        static readonly SpecializationType[] Specializations_AntiCavalry = { SpecializationType.AntiCavalry };
        static readonly SpecializationType[] Specializations_Siege = { SpecializationType.Siege };
        static readonly SpecializationType[] Specializations_Default = {
                            SpecializationType.None,
                            SpecializationType.Field,
                            SpecializationType.Sea,
                            SpecializationType.Siege,
        };


        public SpecializationType[] avaialableSpecializations()
        {
            //SpecializationType[] specializationTypes;

            //switch (weapon)
            //{

            //    case ItemResourceType.Pike:
            //    case ItemResourceType.HandSpear:
            //    case ItemResourceType.TwoHandSword:
            //        specializationTypes = Specializations_AntiCavalry;
            //        break;

            //    case ItemResourceType.Ballista:
            //    case ItemResourceType.SiegeCannonIron:
            //    case ItemResourceType.SiegeCannonBronze:
            //        specializationTypes = Specializations_Siege;
            //        break;

            //    default:
            //        specializationTypes = Specializations_Default;
            //        break;

            //}
            switch (LockedSpecialization(weapon))
            {
                case SpecializationType.AntiCavalry:
                    return Specializations_AntiCavalry;

                case SpecializationType.Siege:
                    return Specializations_Siege;
            }

            return Specializations_Default;
        }

        public static SpecializationType LockedSpecialization(ItemResourceType weapon)
        {
            switch (weapon)
            {

                case ItemResourceType.Pike:
                case ItemResourceType.HandSpear:
                case ItemResourceType.TwoHandSword:
                    return SpecializationType.AntiCavalry;

                case ItemResourceType.Ballista:
                case ItemResourceType.SiegeCannonIron:
                case ItemResourceType.SiegeCannonBronze:
                    return SpecializationType.Siege;

                default:
                    return SpecializationType.None;

            }
        }

        public void toHud(RichBoxContent content, bool compact)
        {
            IconName.Item(weapon, out var weaponIcon, out string weaponName);
            IconName.Item(armorLevel, out var armorIcon, out string armorName);

            //content.newLine();

            if (compact)
            {
                content.Add(new RbImage(LangLib.Training_Icon(training)));

                if (animal != ItemResourceType.NONE)
                {
                    IconName.Item(animal, out SpriteName animalIcon, out string animalName);
                    content.Add(new RbImage(animalIcon));
                    if (mountArmor != ItemResourceType.NONE)
                    {
                        IconName.Item(mountArmor, out SpriteName mountArmorIcon, out string mountArmorName);
                        content.Add(new RbImage(mountArmorIcon));
                    }

                    if (vehicle != ItemResourceType.NONE)
                    {
                        IconName.Item(vehicle, out SpriteName vehicleIcon, out string vehicleName);
                        content.Add(new RbImage(vehicleIcon));
                    }
                }

                content.Add(new RbImage(weaponIcon));
                if (shield != ItemResourceType.NONE)
                {
                    IconName.Item(shield, out SpriteName shieldIcon, out string shieldName);
                    content.Add(new RbImage(shieldIcon));
                }
                if (armorLevel != ItemResourceType.NONE)
                {
                    content.Add(new RbImage(armorIcon));
                }

                if (specialization != SpecializationType.None)
                {
                    IconName.SpecializationTypeName(specialization, out var specIcon, out string specName);
                    content.Add(new RbImage(specIcon));
                }
            }
            else
            {
                label(DssRef.lang.Conscript_TrainingTitle, LangLib.Training_Icon(training), LangLib.Training(training));

                if (animal != ItemResourceType.NONE)
                {
                    labelItem(DssRef.lang.Resource_TypeName_Animal, animal);
                    labelItem(DssRef.lang.Resource_TypeName_MountArmorTitle, mountArmor);
                    labelItem(DssRef.lang.Resource_TypeName_Vehicle, vehicle);
                }
               

                labelItem(DssRef.lang.Conscript_WeaponTitle, weapon);
                labelItem(DssRef.lang.Resource_TypeName_Shield, shield);
                labelItem(DssRef.lang.Conscript_ArmorTitle, armorLevel);


                if (specialization != SpecializationType.None)
                {
                    IconName.SpecializationTypeName(specialization, out var specIcon, out string specName);
                    label(DssRef.lang.Conscript_SpecializationTitle, specIcon, specName);
                }

                void label(string label, SpriteName icon, string name)
                {
                    HudLib.Label(content, TextLib.LargeFirstLetter(label));
                    content.Add(new RbImage(icon));
                    content.hspace();
                    content.Add(new RbText(TextLib.LargeFirstLetter(name), HudLib.TitleColor_TypeName));
                }
                void labelItem(string label, ItemResourceType item)
                {
                    if (item != ItemResourceType.NONE)
                    {
                        IconName.Item(item, out SpriteName icon, out string name);

                        HudLib.Label(content, TextLib.LargeFirstLetter(label));
                        content.Add(new RbImage(icon));
                        content.hspace();
                        content.Add(new RbText(TextLib.LargeFirstLetter(name), HudLib.TitleColor_TypeName));
                    }
                }
            }
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            bool special_man = man != ItemResourceType.Men;
            bool special_shield = shield != ItemResourceType.NONE;
            bool special_animal = animal != ItemResourceType.NONE;
            bool special_mountArmor = mountArmor != ItemResourceType.NONE;
            bool special_vehicle = vehicle != ItemResourceType.NONE;
            bool special_specialization = specialization != SpecializationType.None;

            new EightBit(special_man, special_shield, special_animal, special_mountArmor, special_vehicle, special_specialization).write(w);

            if (special_man)
            {
                w.Write((byte)man);
            }
            w.Write((byte)weapon);
            if (special_shield)
            {
                w.Write((byte)shield);
            }
            w.Write((byte)armorLevel);
            if (special_animal)
            {
                w.Write((byte)animal);
            }
            if (special_mountArmor)
            {
                w.Write((byte)mountArmor);
            }
            if (special_vehicle)
            {
                w.Write((byte)vehicle);
            }
            w.Write((byte)training);
            if (special_specialization)
            {
                w.Write((byte)specialization);
            }
        }

        public void readGameState(System.IO.BinaryReader r)
        {
            EightBit specials = EightBit.FromStream(r);
            specials.Get(out bool special_man, out bool special_shield, out bool special_animal, out bool special_mountArmor, out bool special_vehicle, out bool special_specialization);

            if (special_man)
            {
                man = (ItemResourceType)r.ReadByte();
            }

            if (man == ItemResourceType.NONE)
            {
                man = ItemResourceType.Men;
            }

            weapon = (ItemResourceType)r.ReadByte();
            if (special_shield)
            {
                shield = (ItemResourceType)r.ReadByte();
            }
            armorLevel = (ItemResourceType)r.ReadByte();
            if (special_animal)
            {
                animal = (ItemResourceType)r.ReadByte();
            }
            if (special_mountArmor)
            {
                mountArmor = (ItemResourceType)r.ReadByte();
            }
            if (special_vehicle)
            {
                vehicle = (ItemResourceType)r.ReadByte();
            }
            training = (TrainingLevel)r.ReadByte();
            if (special_specialization)
            {
                specialization = (SpecializationType)r.ReadByte();
            }
        }

        //make these static
        public static int WeaponDamage(ItemResourceType weapon, out int splashCount)
        {
            var soldierData = ItemPropertyColl.Get(weapon).soldierData;
            splashCount = soldierData.attackSplashCount;
            return soldierData.attackDamage;

            //splashCount = 0;
            //switch (weapon)
            //{
            //    case ItemResourceType.SharpStick: return DssConst.WeaponDamage_SharpStick;
            //    case ItemResourceType.BronzeSword: return DssConst.WeaponDamage_BronzeSword;
            //    case ItemResourceType.ShortSword: return DssConst.WeaponDamage_ShortSword;
            //    case ItemResourceType.Sword: return DssConst.WeaponDamage_Sword;
            //    case ItemResourceType.LongSword: return DssConst.WeaponDamage_LongSword;
            //    case ItemResourceType.Pike: return DssConst.WeaponDamage_Pike;
            //    case ItemResourceType.HandSpear: return DssConst.WeaponDamage_Handspear;

            //    case ItemResourceType.Warhammer: return DssConst.WeaponDamage_Warhammer;
            //    case ItemResourceType.TwoHandSword: return DssConst.WeaponDamage_TwoHandSword;
            //    case ItemResourceType.KnightsLance: return DssConst.WeaponDamage_KnigtsLance;
            //    case ItemResourceType.MithrilSword: return DssConst.WeaponDamage_MithrilSword;

            //    case ItemResourceType.SlingShot: return DssConst.WeaponDamage_Slingshot;
            //    case ItemResourceType.ThrowingSpear: return DssConst.WeaponDamage_Throwingspear;
            //    case ItemResourceType.Bow: return DssConst.WeaponDamage_Bow;
            //    case ItemResourceType.LongBow: return DssConst.WeaponDamage_Longbow;
            //    case ItemResourceType.Crossbow: return DssConst.WeaponDamage_CrossBow;
            //    case ItemResourceType.MithrilBow: return DssConst.WeaponDamage_MithrilBow;

            //    case ItemResourceType.HandCannon: return DssConst.WeaponDamage_Handcannon;
            //    case ItemResourceType.HandCulverin:
            //        splashCount = 7;
            //        return DssConst.WeaponDamage_Handculvetin;
            //    case ItemResourceType.Rifle: return DssConst.WeaponDamage_Rifle;
            //    case ItemResourceType.Blunderbuss:
            //        splashCount = 8;
            //        return DssConst.WeaponDamage_Blunderbus;

            //    case ItemResourceType.Ballista:
            //        splashCount = 1;
            //        return DssConst.WeaponDamage_Ballista;
            //    case ItemResourceType.Manuballista:
            //        splashCount = 1;
            //        return DssConst.WeaponDamage_ManuBallista;
            //    case ItemResourceType.Catapult:
            //        splashCount = 3; 
            //        return DssConst.WeaponDamage_Catapult;

            //    case ItemResourceType.SiegeCannonBronze:
            //        splashCount = 12; 
            //        return DssConst.WeaponDamage_SiegeCannonBronze;
            //    case ItemResourceType.ManCannonBronze:
            //        splashCount = 5; return DssConst.WeaponDamage_ManCannonBronze;
            //    case ItemResourceType.SiegeCannonIron:
            //        splashCount = 2; return DssConst.WeaponDamage_SiegeCannonIron;
            //    case ItemResourceType.ManCannonIron:
            //        splashCount = 6; return DssConst.WeaponDamage_ManCannonIron;

            //    case ItemResourceType.RoseWarrior_soldier:
            //        return DssConst.WeaponDamage_LongSword;

            //    case ItemResourceType.RoseWarrior_tank:
            //        return DssConst.WeaponDamage_MithrilSword;

            //    case ItemResourceType.RoseWarrior_dog:
            //        return DssConst.WeaponDamage_Sword;


            //    default: throw new NotImplementedException();
            //}
        }

        //public static Resource.ItemResourceType WeaponItem(ItemResourceType weapon)
        //{
        //    switch (weapon)
        //    {
        //        case ItemResourceType.SharpStick: return Resource.ItemResourceType.SharpStick;
        //        case ItemResourceType.Sword: return Resource.ItemResourceType.Sword;
        //        case ItemResourceType.TwoHandSword: return Resource.ItemResourceType.TwoHandSword;
        //        case ItemResourceType.KnightsLance: return Resource.ItemResourceType.KnightsLance;
        //        case ItemResourceType.Bow: return Resource.ItemResourceType.Bow;
        //        case ItemResourceType.LongBow: return Resource.ItemResourceType.LongBow;
        //        case ItemResourceType.Ballista: return Resource.ItemResourceType.Ballista;

        //        default: throw new NotImplementedException();
        //    }
        //}

        public static int ArmorHealth(ItemResourceType armorLevel)
        {
            return ItemPropertyColl.Get(armorLevel).soldierData.basehealth;
            //switch (armorLevel)
            //{
            //    case ItemResourceType.NONE: return DssConst.ArmorHealth_None;
            //    case ItemResourceType.PaddedArmor: return DssConst.ArmorHealth_Padded;
            //    case ItemResourceType.HeavyPaddedArmor: return DssConst.ArmorHealth_HeavyPadded;
            //    case ItemResourceType.BronzeArmor: return DssConst.ArmorHealth_Bronze;
            //    case ItemResourceType.IronArmor: return DssConst.ArmorHealth_Mail;
            //    case ItemResourceType.HeavyIronArmor: return DssConst.ArmorHealth_HeavyMail;
            //    case ItemResourceType.LightPlateArmor: return DssConst.ArmorHealth_Plate;
            //    case ItemResourceType.FullPlateArmor: return DssConst.ArmorHealth_FullPlate;
            //    case ItemResourceType.MithrilArmor: return DssConst.ArmorHealth_Mithril;
            //    default: throw new NotImplementedException();
            //}
        }

        //public static Resource.ItemResourceType ArmorItem(ItemResourceType armorLevel)
        //{
        //    switch (armorLevel)
        //    {
        //        case ItemResourceType.None: return Resource.ItemResourceType.NONE;
        //        case ItemResourceType.PaddedArmor: return Resource.ItemResourceType.PaddedArmor;
        //        case ItemResourceType.Mail: return Resource.ItemResourceType.IronArmor;
        //        case ItemResourceType.FullPlate: return Resource.ItemResourceType.HeavyIronArmor;
        //        default: throw new NotImplementedException();
        //    }
        //}

        public static float TrainingAttackSpeed(TrainingLevel training)
        {
            switch (training)
            {
                case TrainingLevel.Minimal: return DssConst.TrainingAttackSpeed_Minimal;
                case TrainingLevel.Basic: return DssConst.TrainingAttackSpeed_Basic;
                case TrainingLevel.Skillful: return DssConst.TrainingAttackSpeed_Skillful;
                case TrainingLevel.Professional: return DssConst.TrainingAttackSpeed_Professional;
                case TrainingLevel.Champion: return DssConst.TrainingAttackSpeed_Champion;
#if DEBUG
                default: throw new NotImplementedException();
#else
                default: return DssConst.TrainingAttackSpeed_Basic; 
#endif
            }
        }

        public static float TrainingTime(TrainingLevel training, ItemResourceType animal, BuildAndExpandType type)
        {
            float result;
            switch (training)
            {
                case TrainingLevel.Minimal:
                    result = DssConst.TrainingTimeSec_Minimal;
                    break;
                case TrainingLevel.Basic:
                    result = DssConst.TrainingTimeSec_Basic;
                    break;
                case TrainingLevel.Skillful:
                    result = DssConst.TrainingTimeSec_Skillful;
                    break;
                case TrainingLevel.Professional:
                    result = DssConst.TrainingTimeSec_Professional;
                    break;

                default: throw new NotImplementedException();
            }

            switch (type)
            { 
                case BuildAndExpandType.GunBarracks:
                case BuildAndExpandType.CannonBarracks:
                    result /= 2;
                    break;
            }

            if (animal != ItemResourceType.NONE)
            {
                result += DssConst.TrainingTimeSec_Mount;
            }
            
            return result;
        }

        public static float TrainingTime(CasualSoldierType casualType)
        {
            float result;
            switch (casualType)
            {
                case CasualSoldierType.FolkMen:
                    result = DssConst.TrainingTimeSec_Minimal;
                    break;
                default:
                    result = DssConst.TrainingTimeSec_Basic;
                    break;
                case  CasualSoldierType.Rider:
                    result = DssConst.TrainingTimeSec_Skillful;
                    break;
            }

            return result;
        }
    }

    enum TrainingLevel
    {
        Minimal,
        Basic,
        Skillful,
        Professional,
        Champion,
        Legendary,
        NUM
    }

    enum SpecializationType
    {
        None,
        Field,
        Sea,
        Siege,
        CityGuard,
        //NUM,
        Traditional,
        Viking,
        HonorGuard,
        Green,
        AntiCavalry,
        DarkLord,
    }

    enum ConscriptActiveStatus
    {
        Idle,
        CollectingEquipment,
        //CollectingMen,
        Training,
    }

}
