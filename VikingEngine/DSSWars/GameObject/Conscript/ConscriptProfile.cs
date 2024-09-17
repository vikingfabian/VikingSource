using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;

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
        Collecting,
        Training,
    }
}
