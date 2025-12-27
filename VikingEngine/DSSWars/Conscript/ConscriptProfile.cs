
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
            armorLevel = ItemResourceType.NONE;

            animal = ItemResourceType.NONE;
            mountArmor = ItemResourceType.NONE;
            vehicle = ItemResourceType.NONE;

            training = 0;
            specialization = SpecializationType.None;
        }

        public int menCost()
        {
            SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
            {
                conscript = this,
            };
            var data = SoldierProfile.init();
            return data.workForceCount();
        }

        public bool isKnight()
        {
            if (man == ItemResourceType.NobelMen)
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

        public void classify(out bool ranged, out bool rangedMan, out bool meleeMan, out bool warmachine, out bool animalCompanion, out bool animalMount, out bool wagonRide)
        {
            switch (animal)
            {
                case ItemResourceType.NONE:
                    animalCompanion = false;
                    animalMount = false;
                    break;

                case ItemResourceType.Pig:
                case ItemResourceType.Dog:
                case ItemResourceType.Hound:
                    animalCompanion = true;
                    animalMount = false;
                    break;

                default:                    
                    animalCompanion = false;
                    animalMount = true;
                    break;
            }

            if (vehicle == ItemResourceType.NONE)
            {
                wagonRide = false;
            }
            else
            {
                if (animalMount)
                {
                    animalMount = false;
                    wagonRide = true;
                }
                else
                {
                    wagonRide = false;
                }
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
                    ranged = false;
                    rangedMan = false;
                    meleeMan = true;
                    
                    warmachine = false;
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
                    ranged = true;
                    rangedMan = true;
                    meleeMan = false;
                    
                    warmachine = false;
                    break;

                case ItemResourceType.Warhammer:
                case ItemResourceType.TwoHandSword:
                //case ItemResourceType.KnightsLance:
                case ItemResourceType.MithrilSword:
                    ranged = false;
                    rangedMan = false;
                    meleeMan = true;
                    
                    warmachine = false;
                    break;

                case ItemResourceType.MithrilBow:
                    ranged = true;
                    rangedMan = true;
                    meleeMan = false;
                    
                    warmachine = false;
                    break;

                case ItemResourceType.Ballista:
                case ItemResourceType.Manuballista:
                case ItemResourceType.Catapult:

                case ItemResourceType.SiegeCannonBronze:
                case ItemResourceType.ManCannonBronze:
                case ItemResourceType.SiegeCannonIron:
                case ItemResourceType.ManCannonIron:
                    ranged = true;
                    rangedMan = false;
                    meleeMan = false;
                    
                    warmachine = true;
                    break;

                case ItemResourceType.UN_BatteringRam:
                    ranged = false;
                    rangedMan = false;
                    meleeMan = false;
                    
                    warmachine = true;
                    break;

                default:
                    throw new NotImplementedException();
            }
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

        //public void defaultSetup(BuildAndExpandType barrackType)
        //{
        //    switch (barrackType)
        //    {
        //        case BuildAndExpandType.SoldierBarracks:
        //            weapon = ItemResourceType.SharpStick;
        //            break;
        //        case BuildAndExpandType.ArcherBarracks:
        //            weapon = ItemResourceType.SlingShot;
        //            break;
        //        case BuildAndExpandType.WarmachineBarracks:
        //            weapon = ItemResourceType.Ballista;
        //            break;
        //        case BuildAndExpandType.KnightsBarracks:
        //            weapon = ItemResourceType.Warhammer;
        //            training = TrainingLevel.Basic;
        //            break;
        //        case BuildAndExpandType.GunBarracks:
        //            weapon = ItemResourceType.HandCannon;
        //            break;
        //        case BuildAndExpandType.CannonBarracks:
        //            weapon = ItemResourceType.ManCannonBronze;
        //            break;
        //    }

        //}

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

            switch (animal)
            {
                case ItemResourceType.NONE:
                    break;
                //case ItemResourceType.Pig:
                //case ItemResourceType.Dog:
                //case ItemResourceType.Hound:
                //    break;

                case ItemResourceType.Pony:
                case ItemResourceType.Horse:
                case ItemResourceType.WarHorse:
                case ItemResourceType.DraftHorse:
                    if (vehicle == ItemResourceType.NONE)
                    {
                        name += " .horse rider";
                    }
                    else
                    {
                        name += " .wagon";
                    }
                    break;

                case ItemResourceType.WildPig:
                case ItemResourceType.WildHog:
                case ItemResourceType.WarHog:
                case ItemResourceType.StagHog:
                    if (vehicle == ItemResourceType.NONE)
                    {
                        name += " .hog rider";
                    }
                    else
                    {
                        name += " .wagon";
                    }
                    break;

                case ItemResourceType.Elephant:
                case ItemResourceType.WarElephant:
                case ItemResourceType.Oliphant:
                    if (vehicle == ItemResourceType.NONE)
                    {
                        name += " .elephant rider";
                    }
                    else
                    {
                        name += " .howdah";
                    }
                    break;
            }

            return name;
        }

        public SpecializationType[] avaialableSpecializations()
        {
            SpecializationType[] specializationTypes;
            if (weapon == ItemResourceType.TwoHandSword)
            {
                specializationTypes = new SpecializationType[] { SpecializationType.AntiCavalry };
            }
            else if (weapon == ItemResourceType.Ballista)
            {
                specializationTypes = new SpecializationType[] { SpecializationType.Siege };
            }
            else
            {
                specializationTypes = new SpecializationType[]
                    {
                            SpecializationType.None,
                            SpecializationType.Field,
                            SpecializationType.Sea,
                            SpecializationType.Siege,
                    };
            }

            return specializationTypes;
        }

        public void toHud(RichBoxContent content, bool compact)
        {
            IconName.Item(weapon, out var weaponIcon, out string weaponName);
            IconName.Item(armorLevel, out var armorIcon, out string armorName);


            content.newLine();

            if (compact)
            {
                //HudLib.BulletSeperationPoint(content);
                content.Add(new RbImage(LangLib.Training_Icon(training)));

                content.Add(new RbImage(weaponIcon));

                if (armorLevel != ItemResourceType.NONE)
                {
                    content.Add(new RbImage(armorIcon));
                }
            }
            else
            {

                HudLib.BulletSeperationPoint(content);

                content.Add(new RbImage(LangLib.Training_Icon(training)));
                content.Add(new RbText(LangLib.Training(training), HudLib.TitleColor_TypeName));

                HudLib.BulletSeperationPoint(content);

                content.Add(new RbImage(weaponIcon));
                content.Add(new RbText(weaponName, HudLib.TitleColor_TypeName));

                if (armorLevel != ItemResourceType.NONE)
                {
                    HudLib.BulletSeperationPoint(content);

                    content.Add(new RbImage(armorIcon));
                    content.Add(new RbText(armorName, HudLib.TitleColor_TypeName));
                }

                if (specialization != SpecializationType.None)
                {
                    content.newLine();
                    HudLib.BulletSeperationPoint(content);
                    content.space();
                    content.Add(new RbText(LangLib.SpecializationTypeName(specialization, out var specIcon), HudLib.TitleColor_TypeName));
                }
            }

            //content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Conscript_WeaponTitle, LangLib.Item(weapon)));
            //content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Conscript_ArmorTitle, LangLib.Item(armorLevel)));
            //content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Conscript_TrainingTitle, LangLib.Training(training)));
            //content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Conscript_SpecializationTitle, LangLib.SpecializationTypeName(specialization)));
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)weapon);
            w.Write((byte)armorLevel);
            w.Write((byte)training);
            w.Write((byte)specialization);
        }

        public void readGameState(System.IO.BinaryReader r)
        {
            weapon = (ItemResourceType)r.ReadByte();
            armorLevel = (ItemResourceType)r.ReadByte();
            training = (TrainingLevel)r.ReadByte();
            specialization = (SpecializationType)r.ReadByte();
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

        public static float TrainingTime(TrainingLevel training, BuildAndExpandType type)
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
                //case BuildAndExpandType.KnightsBarracks:
                //    result += DssConst.TrainingTimeSec_NobelmenAdd;
                //    break;
                case BuildAndExpandType.GunBarracks:
                case BuildAndExpandType.CannonBarracks:
                    result /= 2;
                    break;
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
        CollectingMen,
        Training,
    }

}
