using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
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
            :this()
        { 
            this.nobelmen = nobelmen;
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
            return new TimeLength(ConscriptProfile.TrainingTime(inProgress.training));
        }

        public string activeStringOf(ConscriptActiveStatus status)
        {
            string result = null;

           
            switch (status)
            {
                case ConscriptActiveStatus.Idle:
                    result = DssRef.todoLang.Hud_Idle;
                    break;

                case ConscriptActiveStatus.CollectingEquipment:
                    {
                        var progress = string.Format(DssRef.todoLang.Language_CollectProgress, equipmentCollected, DssConst.SoldierGroup_DefaultCount);
                        result = string.Format(DssRef.todoLang.Conscription_Status_CollectingEquipment, progress);
                    }
                    break;

                case ConscriptActiveStatus.CollectingMen:
                    {
                        var progress = string.Format(DssRef.todoLang.Language_CollectProgress, menCollected, DssConst.SoldierGroup_DefaultCount);
                        result = string.Format(DssRef.todoLang.Conscription_Status_CollectingMen, progress);
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
                result = string.Format(DssRef.todoLang.Conscription_Status_Training, countdown.RemainingLength().ShortString());
            }
            else
            { 
                result = activeStringOf(active) + ", " + string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.todoLang.Hud_Que, que <= MaxQue? que.ToString() : DssRef.todoLang.Hud_NoLimit);
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
            return string.Format(DssRef.todoLang.Conscription_Status_Training, remaining);
        }


    }

    struct SoldierConscriptProfile
    {
        public ConscriptProfile conscript;
        public float skillBonus;

        public bool RangedUnit()
        {
            return conscript.weapon == MainWeapon.Bow;
        }

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

        public SoldierData init(AbsSoldierProfile profile)
        {
            SoldierData result = profile.data;

            result.basehealth = ConscriptProfile.ArmorHealth(conscript.armorLevel);

            result.attackDamage = Convert.ToInt32(ConscriptProfile.WeaponDamage(conscript.weapon) * skillBonus);
            result.attackDamageStructure = result.attackDamage;
            result.attackDamageSea = result.attackDamage;

            result.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;

            switch (conscript.weapon)
            {
                case MainWeapon.SharpStick:
                    result.mainAttack = AttackType.Melee;
                    result.attackRange = 0.03f;
                    result.modelName = LootFest.VoxelModelName.war_folkman;
                    result.icon = SpriteName.WarsUnitIcon_Folkman;
                    break;
                case MainWeapon.Sword:
                    result.mainAttack = AttackType.Melee;
                    result.attackRange = 0.04f;
                    result.modelName = LootFest.VoxelModelName.wars_soldier;
                    result.modelVariationCount = 3;
                    result.icon = SpriteName.WarsUnitIcon_Soldier;
                    break;
                case MainWeapon.TwoHandSword:
                    result.mainAttack = AttackType.Melee;
                    result.attackRange = 0.08f;
                    result.modelName = LootFest.VoxelModelName.wars_twohand;
                    result.modelVariationCount = 1;
                    result.icon = SpriteName.WarsUnitIcon_Soldier;
                    break;
                case MainWeapon.Bow:
                    result.mainAttack = AttackType.Arrow;
                    result.ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    result.attackRange = 1.7f;
                    result.modelName = LootFest.VoxelModelName.war_archer;
                    result.modelVariationCount = 2;
                    result.icon = SpriteName.WarsUnitIcon_Archer;
                    result.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;
                    break;
            }

            switch (conscript.specialization)
            {
                case SpecializationType.Field:
                    result.attackDamage = MathExt.AddPercentage(result.attackDamage, DssConst.Conscript_SpecializePercentage);
                    result.attackDamageSea = MathExt.SubtractPercentage(result.attackDamageSea, DssConst.Conscript_SpecializePercentage);
                    result.attackDamageStructure = MathExt.SubtractPercentage(result.attackDamageStructure, DssConst.Conscript_SpecializePercentage);
                    break;

                case SpecializationType.Viking:
                case SpecializationType.Sea:
                    result.attackDamage = MathExt.SubtractPercentage(result.attackDamage, DssConst.Conscript_SpecializePercentage);
                    float seaDamagePerc = conscript.specialization == SpecializationType.Sea ?
                        DssConst.Conscript_SpecializePercentage : DssConst.Conscript_SpecializePercentage * 3f;
                    result.attackDamageSea = MathExt.AddPercentage(result.attackDamageSea, seaDamagePerc);
                    result.attackDamageStructure = MathExt.SubtractPercentage(result.attackDamageStructure, DssConst.Conscript_SpecializePercentage);


                    if (!RangedUnit())
                    {
                        result.modelName = LootFest.VoxelModelName.war_sailor;
                        result.modelVariationCount = 2;
                        result.icon = SpriteName.WarsUnitIcon_Viking;
                    }
                    break;

                case SpecializationType.Siege:
                    result.attackDamage = MathExt.SubtractPercentage(result.attackDamage, DssConst.Conscript_SpecializePercentage);
                    result.attackDamageSea = MathExt.SubtractPercentage(result.attackDamageSea, DssConst.Conscript_SpecializePercentage);
                    result.attackDamageStructure = MathExt.AddPercentage(result.attackDamageStructure, DssConst.Conscript_SpecializePercentage);
                    break;

                case SpecializationType.HonorGuard:
                    result.energyPerSoldier = 0;
                    result.modelName = LootFest.VoxelModelName.little_hirdman;
                    result.modelVariationCount = 1;
                    result.icon = SpriteName.WarsUnitIcon_Honorguard;
                    break;

                case SpecializationType.Traditional:
                    result.energyPerSoldier *= 0.5f;
                    break;

                case SpecializationType.Green:
                    result.secondaryAttack = AttackType.Arrow;
                    result.secondaryAttackDamage = 100;
                    result.secondaryAttackRange = 1.7f;
                    result.bonusProjectiles = 2;
                    result.icon = SpriteName.WarsUnitIcon_Greensoldier;

                    break;
            }

            result.attackTimePlusCoolDown /= ConscriptProfile.TrainingAttackSpeed(conscript.training);
            result.attackTimePlusCoolDown /= 1f + skillBonus;


            return result;
        }

        public SoldierData bannermanSetup(SoldierData soldierData)
        {
            soldierData.canAttackCharacters = false;
            soldierData.canAttackStructure = false;

            soldierData.modelName = LootFest.VoxelModelName.war_bannerman;
            soldierData.modelVariationCount = 1;

            return soldierData;
        }

        public void shipSetup(ref SoldierData soldierData)
        {
            soldierData.walkingSpeed = DssConst.Men_StandardShipSpeed;

            if (conscript.specialization == SpecializationType.Sea)
            {
                if (!RangedUnit())
                {
                    soldierData.modelName = LootFest.VoxelModelName.wars_viking_ship;


                    soldierData.mainAttack = AttackType.Javelin;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 2.5f;
                    soldierData.attackRange = 1f;
                }
                soldierData.walkingSpeed *= 1.5f;
            }

            if (soldierData.modelName != LootFest.VoxelModelName.wars_viking_ship)
            {
                switch (conscript.weapon)
                {
                    case MainWeapon.SharpStick:
                        soldierData.modelName = LootFest.VoxelModelName.wars_folk_ship;

                        break;
                    case MainWeapon.Sword:
                        soldierData.modelName = LootFest.VoxelModelName.wars_soldier_ship;

                        break;
                    case MainWeapon.Bow:
                        soldierData.modelName = LootFest.VoxelModelName.wars_archer_ship;

                        break;
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

                default:
                    switch (weapon)
                    {
                        case MainWeapon.Bow:
                            return DssRef.lang.UnitType_Archer;
                        case MainWeapon.SharpStick:
                            return DssRef.lang.UnitType_Folkman;
                        case MainWeapon.Sword:
                            return DssRef.lang.UnitType_Soldier;

                        default:
                            return TextLib.Error;
                    }
            }
        }

        public void toHud(RichBoxContent content)
        {
            content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.todoLang.Conscript_WeaponTitle, LangLib.Weapon(weapon)));
            content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.todoLang.Conscript_ArmorTitle, LangLib.Armor(armorLevel)));
            content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.todoLang.Conscript_TrainingTitle, LangLib.Training(training)));
            content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.todoLang.Conscript_SpecializationTitle, LangLib.SpecializationTypeName(specialization)));

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
                case MainWeapon.Bow: return DssConst.WeaponDamage_Bow;
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
                case MainWeapon.Bow: return Resource.ItemResourceType.Bow;
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

        public static float TrainingTime(TrainingLevel training)
        {
            switch (training)
            {
                case TrainingLevel.Minimal: return DssConst.TrainingTimeSec_Minimal;
                case TrainingLevel.Basic: return DssConst.TrainingTimeSec_Basic;
                case TrainingLevel.Skillful: return DssConst.TrainingTimeSec_Skillful;
                case TrainingLevel.Professional: return DssConst.TrainingTimeSec_Professional;
                
                default: throw new NotImplementedException();
            }
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
        Bow,
        Ballista,
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
    }

    enum ConscriptActiveStatus
    { 
        Idle,
        CollectingEquipment,
        CollectingMen,
        Training,
    }
}
