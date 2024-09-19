using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.LootFest.Data;

namespace VikingEngine.DSSWars.GameObject.Conscript
{
    struct BarracksStatus
    {
        public const int MaxQue = 5;

        public ConscriptActiveStatus active;
        public ConscriptProfile profile;

        public ConscriptProfile inProgress;
        public TimeInGameCountdown countdown;
        public int menCollected;
        public int equipmentCollected;

        public int idAndPosition;
        public int que;

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

    struct SoldierProfile
    {
        public ConscriptProfile conscript;
        public float skillBonus;

        public bool RangedUnit()
        {
            return conscript.weapon == MainWeapon.Bow;
        }
    }

    struct ConscriptProfile
    {
        public MainWeapon weapon;
        public ArmorLevel armorLevel;
        public TrainingLevel training;
       

        //make these static
        public static int WeaponDamage(MainWeapon weapon)
        {
            switch (weapon)
            {
                case MainWeapon.SharpStick: return DssConst.WeaponDamage_SharpStick;
                case MainWeapon.Sword: return DssConst.WeaponDamage_Sword;
                case MainWeapon.Bow: return DssConst.WeaponDamage_Bow;

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

    enum ConscriptActiveStatus
    { 
        Idle,
        CollectingEquipment,
        CollectingMen,
        Training,
    }
}
