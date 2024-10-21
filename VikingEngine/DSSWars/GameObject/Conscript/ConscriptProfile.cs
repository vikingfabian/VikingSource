using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject.Delivery;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Data;
using VikingEngine.ToGG.ToggEngine;

namespace VikingEngine.DSSWars.GameObject.Conscript
{
    struct BarracksStatus
    {
        public const int MaxQue = 5;

        public ConscriptActiveStatus active;
        public ConscriptProfile profile;

        public ConscriptProfile inProgress;
        public TimeInGameCountdown countdown;
        public bool nobelmen;
        public int menCollected;
        public int equipmentCollected;

        public int idAndPosition;
        public int que;

        public BarracksStatus(bool nobelmen)
            : this()
        {
            this.nobelmen = nobelmen;
            if (nobelmen)
            {
                profile.weapon = MainWeapon.KnightsLance;
            }
        }

        public void halt(City city)
        {
            que = 0;

            returnItems(city);

        }

        public void returnItems(City city)
        {
            if (active == ConscriptActiveStatus.CollectingEquipment ||
                    active == ConscriptActiveStatus.CollectingMen)
            {
                //return items
                ItemResourceType weaponItem = ConscriptProfile.WeaponItem(inProgress.weapon);
                ItemResourceType armorItem = ConscriptProfile.ArmorItem(inProgress.armorLevel);

                city.AddGroupedResource(weaponItem, equipmentCollected);

                if (inProgress.armorLevel != ArmorLevel.None)
                {
                    city.AddGroupedResource(armorItem, equipmentCollected);
                }

                city.workForce += menCollected;

                active = ConscriptActiveStatus.Idle;

                //city.conscriptBuildings[selectedConscript] = status;
            }
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)active);
            profile.writeGameState(w);
            if (active != ConscriptActiveStatus.Idle)
            {
                inProgress.writeGameState(w);
            }
            switch (active)
            {
                case ConscriptActiveStatus.CollectingEquipment:
                    w.Write((byte)equipmentCollected);
                    break;

                case ConscriptActiveStatus.CollectingMen:
                    w.Write((byte)menCollected);
                    break;

                case ConscriptActiveStatus.Training:
                    countdown.writeGameState(w);
                    break;
            }
            w.Write(nobelmen);
            w.Write(idAndPosition);
            w.Write((byte)que);
        }

        public void readGameState(System.IO.BinaryReader r, int subVersion)
        {
            active = (ConscriptActiveStatus)r.ReadByte();
            profile.readGameState(r);
            if (active != ConscriptActiveStatus.Idle)
            {
                inProgress.readGameState(r);
            }
            switch (active)
            {
                case ConscriptActiveStatus.CollectingEquipment:
                    equipmentCollected = r.ReadByte();
                    break;

                case ConscriptActiveStatus.CollectingMen:
                    equipmentCollected = DssConst.SoldierGroup_DefaultCount;
                    menCollected = r.ReadByte();
                    break;

                case ConscriptActiveStatus.Training:
                    equipmentCollected = DssConst.SoldierGroup_DefaultCount;
                    menCollected = DssConst.SoldierGroup_DefaultCount;
                    countdown.readGameState(r);
                    break;
            }
            if (subVersion >= 13)
            { 
                nobelmen = r.ReadBoolean();
            }
            idAndPosition = r.ReadInt32();
            que = r.ReadByte();
        }
        public bool CountDownQue()
        {
            if (que > 0)
            {
                if (que <= MaxQue)
                { 
                    --que;
                }

                return true;
            }

            return false;
        }

        
        public TimeLength TimeLength()
        {
            return new TimeLength(ConscriptProfile.TrainingTime(inProgress.training, nobelmen));
        }

        public string activeStringOf(ConscriptActiveStatus status)
        {
            string result = null;

           
            switch (status)
            {
                case ConscriptActiveStatus.Idle:
                    result = DssRef.lang.Hud_Idle;
                    break;

                case ConscriptActiveStatus.CollectingEquipment:
                    {
                        var progress = string.Format(DssRef.lang.Language_CollectProgress, equipmentCollected, DssConst.SoldierGroup_DefaultCount);
                        result = string.Format(DssRef.lang.Conscription_Status_CollectingEquipment, progress);
                    }
                    break;

                case ConscriptActiveStatus.CollectingMen:
                    {
                        var progress = string.Format(DssRef.lang.Language_CollectProgress, menCollected, DssConst.SoldierGroup_DefaultCount);
                        result = string.Format(DssRef.lang.Conscription_Status_CollectingMen, progress);
                    }
                    break;
            }            

            return result;
        }

        public string shortActiveString()
        {
            string result = null;
            if (active == ConscriptActiveStatus.Training)
            {
                result = string.Format(DssRef.lang.Conscription_Status_Training, countdown.RemainingLength().ShortString());
            }
            else
            { 
                result = activeStringOf(active) + ", " + string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_Queue, que <= MaxQue? que.ToString() : DssRef.lang.Hud_NoLimit);
            }
            
            return result;
        }

        public string longTimeProgress()
        {
            string remaining;
            if (active == ConscriptActiveStatus.Training)
            {
                remaining = countdown.RemainingLength().LongString();
            }
            else
            {
                remaining = TimeLength().LongString();
            }
            return string.Format(DssRef.lang.Conscription_Status_Training, remaining);
        }
    }

    struct SoldierConscriptProfile
    {
        public ConscriptProfile conscript;
        public float skillBonus;

        

        public void writeGameState(System.IO.BinaryWriter w)
        {
            conscript.writeGameState(w);
            SaveLib.WriteFloatMultiplier(skillBonus, w);
        }
        public void readGameState(System.IO.BinaryReader r)
        {
            conscript.readGameState(r);
            skillBonus = SaveLib.ReadFloatMultiplier(r);
        }

        public UnitType unitType()
        {
            if (conscript.specialization == SpecializationType.DarkLord)
            {
                return UnitType.DarkLord;
            }
            switch (conscript.weapon)
            {
                case MainWeapon.Ballista:
                    return UnitType.ConscriptWarmashine;
                case MainWeapon.KnightsLance:
                    return UnitType.ConscriptCavalry;

                default:
                    return UnitType.Conscript;
            }
        }

        public UnitFilterType filterType()
        {
            switch (conscript.specialization)
            {
                default:
                    switch (conscript.weapon)
                    {
                        case MainWeapon.SharpStick:
                            return UnitFilterType.SharpStick;
                        case MainWeapon.Sword:
                            return UnitFilterType.Sword;
                        case MainWeapon.Pike:
                            return UnitFilterType.Pike;
                        case MainWeapon.TwoHandSword:
                            return UnitFilterType.TwohandSword;
                        case MainWeapon.KnightsLance:
                            return UnitFilterType.Knight;
                        case MainWeapon.Bow:
                        case MainWeapon.Longbow:
                            return UnitFilterType.Bow;
                        case MainWeapon.CrossBow:
                            return UnitFilterType.CrossBow;
                        case MainWeapon.Ballista:
                            return UnitFilterType.Ballista;

                        default:
                            throw  new NotImplementedException();

                    }

                case SpecializationType.Green:
                    return UnitFilterType.GreenSoldier;
                case SpecializationType.HonorGuard:
                    return UnitFilterType.HonourGuard;
                case SpecializationType.Viking:
                    return UnitFilterType.Viking;
                case SpecializationType.DarkLord:
                    return UnitFilterType.DarkLord;
            }
        }

        public SpriteName Icon()
        {
           return init(DssRef.profile.bannerman).icon;
        }

        public SoldierData init(AbsSoldierProfile profile)
        {
            if (skillBonus <= 0)
            {
                skillBonus = 1;
            }

            SoldierData soldierData = profile.data;

            soldierData.basehealth = ConscriptProfile.ArmorHealth(conscript.armorLevel);
            soldierData.attackDamage = Convert.ToInt32(ConscriptProfile.WeaponDamage(conscript.weapon) * skillBonus);
            soldierData.attackDamageStructure = soldierData.attackDamage;
            soldierData.attackDamageSea = soldierData.attackDamage;

            soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;

            switch (conscript.weapon)
            {
                case MainWeapon.SharpStick:
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.03f;
                    soldierData.modelName = LootFest.VoxelModelName.war_folkman;
                    soldierData.icon = SpriteName.WarsUnitIcon_Folkman;
                    break;

                case MainWeapon.Sword:
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.04f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_soldier;
                    soldierData.modelVariationCount = 3;
                    soldierData.icon = SpriteName.WarsUnitIcon_Soldier;
                    break;

                case MainWeapon.Pike:
                    soldierData.arrowWeakness = true;
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.055f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_piker;
                    soldierData.modelVariationCount = 1;
                    soldierData.modelScale *= 1.6f;
                    soldierData.icon = SpriteName.WarsUnitIcon_Pikeman;
                    conscript.specialization = SpecializationType.AntiCavalry;
                    break;

                case MainWeapon.TwoHandSword:
                    soldierData.arrowWeakness = true;
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.08f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_twohand;
                    soldierData.modelVariationCount = 1;
                    soldierData.modelScale *= 1.6f;
                    soldierData.icon = SpriteName.WarsUnitIcon_TwoHand;
                    break;

                case MainWeapon.KnightsLance:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 2f;
                    soldierData.attackRange = 0.06f;
                    soldierData.basehealth *= 3;
                    soldierData.mainAttack = AttackType.Melee;
                    //result.attackDamage = 120;
                    soldierData.attackDamageStructure = Convert.ToInt32(30 * skillBonus);
                    soldierData.attackDamageSea = Convert.ToInt32(20 * skillBonus);
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 0.8f;
                    soldierData.modelName = LootFest.VoxelModelName.war_knight;
                    soldierData.modelVariationCount = 3;
                    soldierData.modelScale *= 1.5f;
                    soldierData.icon = SpriteName.WarsUnitIcon_Knight;
                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 3;
                    //soldierData.ArmySpeedBonusLand = 0.8;
                    break;

                case MainWeapon.Bow:
                    soldierData.mainAttack = AttackType.Arrow;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    soldierData.attackRange = 1.3f;
                    soldierData.modelName = LootFest.VoxelModelName.war_archer;
                    soldierData.modelVariationCount = 2;
                    soldierData.icon = SpriteName.WarsUnitIcon_Archer;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;
                    break;

                case MainWeapon.Longbow:
                    soldierData.mainAttack = AttackType.Arrow;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    soldierData.attackRange = 1.7f;
                    soldierData.modelName = LootFest.VoxelModelName.war_archer;
                    soldierData.modelVariationCount = 2;
                    soldierData.icon = SpriteName.WarsUnitIcon_Archer;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;
                    break;

                case MainWeapon.CrossBow:
                    soldierData.mainAttack = AttackType.Bolt;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    soldierData.attackRange = 1.7f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_crossbow;
                    soldierData.modelVariationCount = 1;
                    soldierData.icon = SpriteName.LittleUnitIconCrossBowman;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 15f;
                    break;

                case MainWeapon.Ballista:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                    soldierData.attackRange = WarmashineProfile.BallistaRange;

                    soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
                    soldierData.mainAttack = AttackType.Ballista;
                    soldierData.attackDamageStructure = Convert.ToInt32(1500 * skillBonus);
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 16f;

                    soldierData.modelName = LootFest.VoxelModelName.war_ballista;
                    soldierData.modelVariationCount = 2;

                    soldierData.modelScale = DssConst.Men_StandardModelScale * 2f;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Back;

                    soldierData.icon = SpriteName.WarsUnitIcon_Ballista;

                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;
                    break;
            }

            switch (conscript.specialization)
            {
                case SpecializationType.Field:
                    soldierData.attackDamage = MathExt.AddPercentage(soldierData.attackDamage, DssConst.Conscript_SpecializePercentage);
                    soldierData.attackDamageSea = MathExt.SubtractPercentage(soldierData.attackDamageSea, DssConst.Conscript_SpecializePercentage);
                    soldierData.attackDamageStructure = MathExt.SubtractPercentage(soldierData.attackDamageStructure, DssConst.Conscript_SpecializePercentage);
                    break;

                case SpecializationType.Viking:
                case SpecializationType.Sea:
                    soldierData.attackDamage = MathExt.SubtractPercentage(soldierData.attackDamage, DssConst.Conscript_SpecializePercentage);
                    float seaDamagePerc = conscript.specialization == SpecializationType.Sea ?
                        DssConst.Conscript_SpecializePercentage : DssConst.Conscript_SpecializePercentage * 3f;
                    soldierData.attackDamageSea = MathExt.AddPercentage(soldierData.attackDamageSea, seaDamagePerc);
                    soldierData.attackDamageStructure = MathExt.SubtractPercentage(soldierData.attackDamageStructure, DssConst.Conscript_SpecializePercentage);

                    if (!conscript.RangedUnit())
                    {
                        soldierData.modelName = LootFest.VoxelModelName.war_sailor;
                        soldierData.modelVariationCount = 2;
                        soldierData.icon = SpriteName.WarsUnitIcon_Viking;
                    }
                    break;

                case SpecializationType.Siege:
                    soldierData.attackDamage = MathExt.SubtractPercentage(soldierData.attackDamage, DssConst.Conscript_SpecializePercentage);
                    soldierData.attackDamageSea = MathExt.SubtractPercentage(soldierData.attackDamageSea, DssConst.Conscript_SpecializePercentage);
                    soldierData.attackDamageStructure = MathExt.AddPercentage(soldierData.attackDamageStructure, DssConst.Conscript_SpecializePercentage);
                    break;

                case SpecializationType.HonorGuard:
                    soldierData.modelScale = DssConst.Men_StandardModelScale * 1.2f;
                    soldierData.energyPerSoldier = 0;
                    soldierData.modelName = LootFest.VoxelModelName.little_hirdman;
                    soldierData.modelVariationCount = 1;
                    soldierData.icon = SpriteName.WarsUnitIcon_Honorguard;
                    break;

                case SpecializationType.Traditional:
                    soldierData.energyPerSoldier *= 0.5f;
                    break;

                case SpecializationType.Green:
                    soldierData.secondaryAttack = AttackType.Arrow;
                    soldierData.secondaryAttackDamage = 100;
                    soldierData.secondaryAttackRange = 1.7f;
                    soldierData.bonusProjectiles = 2;
                    soldierData.icon = SpriteName.WarsUnitIcon_Greensoldier;
                    break;

                case SpecializationType.DarkLord:
                    soldierData.modelScale = DssConst.Men_StandardModelScale;
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Back;
                    soldierData.basehealth = DssConst.Soldier_DefaultHealth * 4;
                    soldierData.modelName = LootFest.VoxelModelName.wars_darklord;
                    break;
            }

            soldierData.attackTimePlusCoolDown /= ConscriptProfile.TrainingAttackSpeed(conscript.training);
            soldierData.attackTimePlusCoolDown /= 1f + skillBonus;


            return soldierData;
        }

        public SoldierData bannermanSetup(SoldierData soldierData)
        {
            soldierData.modelScale = DssConst.Men_StandardModelScale;
            soldierData.canAttackCharacters = false;
            soldierData.canAttackStructure = false;

            soldierData.modelName = LootFest.VoxelModelName.war_bannerman;
            soldierData.modelVariationCount = 1;

            return soldierData;
        }

        public void shipSetup(ref SoldierData soldierData)
        {
            soldierData.modelName = LootFest.VoxelModelName.NUM_NON;

            soldierData.walkingSpeed = DssConst.Men_StandardShipSpeed;

            soldierData.modelScale = DssConst.Men_StandardModelScale * 6f;

            switch (conscript.specialization)
            {
                case SpecializationType.Viking:
                    if (!conscript.RangedUnit())
                    {
                        soldierData.modelName = LootFest.VoxelModelName.wars_viking_ship;

                        soldierData.mainAttack = AttackType.Javelin;
                        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 2.5f;
                        soldierData.attackRange = 1f;
                    }
                    soldierData.walkingSpeed *= 1.5f;
                    break;

                case SpecializationType.DarkLord:
                    soldierData.modelName = LootFest.VoxelModelName.wars_knight_ship;

                    soldierData.mainAttack = AttackType.Javelin;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 2.5f;
                    soldierData.attackRange = 2f;

                    soldierData.attackDamage = 500;
                    soldierData.attackDamageStructure = soldierData.attackDamage;
                    soldierData.attackDamageSea = soldierData.attackDamage;

                    soldierData.walkingSpeed *= 1.5f;
                    break;
            }

            if (soldierData.modelName == LootFest.VoxelModelName.NUM_NON)
            {
                switch (conscript.weapon)
                {
                    case MainWeapon.SharpStick:
                        soldierData.modelName = LootFest.VoxelModelName.wars_folk_ship;

                        break;
                    case MainWeapon.Pike:
                    case MainWeapon.Sword:
                        soldierData.modelName = LootFest.VoxelModelName.wars_soldier_ship;
                        break;

                    case MainWeapon.CrossBow:
                    case MainWeapon.Longbow:
                    case MainWeapon.Bow:
                        soldierData.modelName = LootFest.VoxelModelName.wars_archer_ship;
                        break;

                    case MainWeapon.Ballista:
                        soldierData.modelName = LootFest.VoxelModelName.wars_ballista_ship;
                        break;

                    case MainWeapon.TwoHandSword:
                    case MainWeapon.KnightsLance:
                        soldierData.modelName = LootFest.VoxelModelName.wars_knight_ship;
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }

    struct ConscriptProfile
    {
        public MainWeapon weapon;
        public ArmorLevel armorLevel;
        public TrainingLevel training;
        public SpecializationType specialization;

        public bool RangedUnit()
        {
            return weapon == MainWeapon.Bow || weapon == MainWeapon.CrossBow || weapon == MainWeapon.Ballista;
        }

        public int DefaultArmyRow()
        {
            switch (weapon)
            {
                case MainWeapon.Bow:
                case MainWeapon.CrossBow:
                    return ArmyPlacementGrid.Row_Second;
                case MainWeapon.Ballista:
                    return ArmyPlacementGrid.Row_Behind;
                default:
                    return ArmyPlacementGrid.Row_Body;
            }
        }

        public double armySpeedBonus(bool land)
        {
            if (land)
            {
                switch (weapon)
                {
                    case MainWeapon.KnightsLance:
                        return 0.8;
                    case MainWeapon.Ballista:
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

        public void defaultSetup(bool nobelmen)
        {
            if (nobelmen)
            {
                weapon = MainWeapon.TwoHandSword;
                training = TrainingLevel.Basic;
            }
        }

        public string TypeName()
        {
            switch (specialization)
            {
                case SpecializationType.HonorGuard:
                    return DssRef.lang.UnitType_HonorGuard;
                case SpecializationType.Viking:
                    return DssRef.lang.UnitType_Viking;
                case SpecializationType.Green:
                    return DssRef.lang.UnitType_GreenSoldier;
                case SpecializationType.DarkLord:
                    return DssRef.lang.UnitType_DarkLord;

                default:
                    switch (weapon)
                    {
                        case MainWeapon.Bow:
                        case MainWeapon.Longbow:
                            return DssRef.lang.UnitType_Archer;
                        case MainWeapon.CrossBow:
                            return DssRef.lang.UnitType_Crossbow;
                        case MainWeapon.Ballista:
                            return DssRef.lang.UnitType_Ballista;
                        case MainWeapon.SharpStick:
                            return DssRef.lang.UnitType_Folkman;
                        case MainWeapon.Pike:
                            return DssRef.lang.UnitType_Pikeman;
                        case MainWeapon.Sword:
                            return DssRef.lang.UnitType_Soldier;
                        case MainWeapon.KnightsLance:
                            return DssRef.lang.UnitType_CavalryKnight;
                        case MainWeapon.TwoHandSword:
                            return DssRef.lang.UnitType_FootKnight;


                        default:
                            return TextLib.Error;
                    }
            }
        }

        public SpecializationType[] avaialableSpecializations()
        {
            SpecializationType[] specializationTypes;
            if (weapon == MainWeapon.TwoHandSword)
            {
                specializationTypes = new SpecializationType[] { SpecializationType.AntiCavalry };
            }
            else if (weapon == MainWeapon.Ballista)
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

        public void toHud(RichBoxContent content)
        {
            content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Conscript_WeaponTitle, LangLib.Weapon(weapon)));
            content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Conscript_ArmorTitle, LangLib.Armor(armorLevel)));
            content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Conscript_TrainingTitle, LangLib.Training(training)));
            content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Conscript_SpecializationTitle, LangLib.SpecializationTypeName(specialization)));

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
            weapon = (MainWeapon)r.ReadByte();
            armorLevel = (ArmorLevel)r.ReadByte(); 
            training = (TrainingLevel)r.ReadByte();
            specialization = (SpecializationType)r.ReadByte();
        }

        //make these static
        public static int WeaponDamage(MainWeapon weapon)
        {
            switch (weapon)
            {
                case MainWeapon.SharpStick: return DssConst.WeaponDamage_SharpStick;
                case MainWeapon.Sword: return DssConst.WeaponDamage_Sword;
                case MainWeapon.Pike: return DssConst.WeaponDamage_Pike;
                case MainWeapon.TwoHandSword: return DssConst.WeaponDamage_TwoHandSword;
                case MainWeapon.KnightsLance: return DssConst.WeaponDamage_KnigtsLance;
                case MainWeapon.Bow: return DssConst.WeaponDamage_Bow;
                case MainWeapon.Longbow: return DssConst.WeaponDamage_Longbow;
                case MainWeapon.CrossBow: return DssConst.WeaponDamage_CrossBow;
                case MainWeapon.Ballista: return DssConst.WeaponDamage_Ballista;

                default: throw new NotImplementedException();
            }
        }

        public static Resource.ItemResourceType WeaponItem(MainWeapon weapon)
        {
            switch (weapon)
            {
                case MainWeapon.SharpStick: return Resource.ItemResourceType.SharpStick;
                case MainWeapon.Sword: return Resource.ItemResourceType.Sword;
                case MainWeapon.TwoHandSword: return Resource.ItemResourceType.TwoHandSword;
                case MainWeapon.KnightsLance: return Resource.ItemResourceType.KnightsLance;
                case MainWeapon.Bow: return Resource.ItemResourceType.Bow;
                case MainWeapon.Longbow: return Resource.ItemResourceType.LongBow;
                case MainWeapon.Ballista: return Resource.ItemResourceType.Ballista;

                default: throw new NotImplementedException();
            }
        }

        public static int ArmorHealth(ArmorLevel armorLevel)
        {
            switch (armorLevel)
            {
                case ArmorLevel.None: return DssConst.ArmorHealth_None;
                case ArmorLevel.Light: return DssConst.ArmorHealth_Light;
                case ArmorLevel.Medium: return DssConst.ArmorHealth_Medium;
                case ArmorLevel.Heavy: return DssConst.ArmorHealth_Heavy;
                default: throw new NotImplementedException();
            }
        }

        public static Resource.ItemResourceType ArmorItem(ArmorLevel armorLevel)
        {
            switch (armorLevel)
            {
                case ArmorLevel.None: return Resource.ItemResourceType.NONE;
                case ArmorLevel.Light: return Resource.ItemResourceType.LightArmor;
                case ArmorLevel.Medium: return Resource.ItemResourceType.MediumArmor;
                case ArmorLevel.Heavy: return Resource.ItemResourceType.HeavyArmor;
                default: throw new NotImplementedException();
            }
        }

        public static float TrainingAttackSpeed(TrainingLevel training)
        {
            switch (training)
            {
                case TrainingLevel.Minimal: return DssConst.TrainingAttackSpeed_Minimal;
                case TrainingLevel.Basic: return DssConst.TrainingAttackSpeed_Basic;
                case TrainingLevel.Skillful: return DssConst.TrainingAttackSpeed_Skillful;
                case TrainingLevel.Professional: return DssConst.TrainingAttackSpeed_Professional;
                default: throw new NotImplementedException();
            }
        }

        public static float TrainingTime(TrainingLevel training, bool nobelMen)
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

            if (nobelMen)
            {
                result += DssConst.TrainingTimeSec_NobelmenAdd;
            }

            return result;
        }
    }

    enum ArmorLevel
    {
        None,
        Light,
        Medium,
        Heavy,
        NUM
    }

    enum MainWeapon
    {
        SharpStick,
        Sword,
        Pike,
        Bow,
        CrossBow,
        TwoHandSword,
        KnightsLance,
        Ballista,
        Longbow,
        NUM
    }

    enum TrainingLevel
    {
        Minimal,
        Basic,
        Skillful,
        Professional,
        NUM
    }

    enum SpecializationType
    { 
        None,
        Field,
        Sea,
        Siege,
        NUM,
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
