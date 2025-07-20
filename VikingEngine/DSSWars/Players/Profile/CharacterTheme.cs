using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.Players.Profile
{
    static class CharacterTheme
    {
       
        public static ArmThemeModels arm(ArmsTheme theme,
            WeaponLeftArmType ltype, WeaponRightArmType rtype)
        {
            ArmThemeModels result = new ArmThemeModels();

            switch (theme)
            {
                default:
                    {
                        switch (ltype)
                        {
                            default: 
                                result.left = VoxelModelName.modsoldier_larm_empty1;
                                break;
                            case WeaponLeftArmType.Shield: 
                                result.left = VoxelModelName.modsoldier_larm_shield1;
                                break;
                        }

                        switch (rtype)
                        {
                            default:
                                result.right = VoxelModelName.modsoldier_rarm_sword1;
                                break;
                            case WeaponRightArmType.Bow: 
                                result.right = VoxelModelName.modsoldier_rarm_bow1v2;
                                break;
                        }
                    }
                    break;

                case ArmsTheme.Naked:
                    {
                        switch (ltype)
                        {
                            default:
                                result.left = VoxelModelName.modsoldier_larm_empty2naked;
                                break;
                            case WeaponLeftArmType.Shield:
                                result.left = VoxelModelName.modsoldier_larm_shield2naked;
                                break;
                        }

                        switch (rtype)
                        {
                            default:
                                result.right = VoxelModelName.modsoldier_rarm_sword2naked;
                                break;
                            case WeaponRightArmType.Bow:
                                result.right = VoxelModelName.modsoldier_rarm_bow2naked;
                                break;
                        }
                    }
                    break;

            }

            return result;
        }
    }

    struct ArmThemeModels
    {
        public VoxelModelName left;
        public VoxelModelName right;
    }

    //abstract class AbsCharacterArmTheme
    //{
    //    abstract public VoxelModelName Left(WeaponLeftArmType type);
    //    abstract public VoxelModelName Right(WeaponRightArmType type);

    //}

    //class DefaultArmTheme : AbsCharacterArmTheme
    //{
    //    public override VoxelModelName Left(WeaponLeftArmType type)
    //    {
    //        switch (type)
    //        {
    //            default: return VoxelModelName.modsoldier_larm_empty1;
    //            case WeaponLeftArmType.Shield: return VoxelModelName.modsoldier_larm_shield1;
    //        }
    //    }

    //    public override VoxelModelName Right(WeaponRightArmType type)
    //    {
    //        switch (type)
    //        {
    //            default: return VoxelModelName.modsoldier_rarm_sword1;
    //            case WeaponRightArmType.Bow: return VoxelModelName.modsoldier_rarm_bow1v2;
    //        }
    //    }
    //}

    //class NakedArmTheme : AbsCharacterArmTheme
    //{
    //    public override VoxelModelName Left(WeaponLeftArmType type)
    //    {
    //        switch (type)
    //        {
    //            default: return VoxelModelName.modsoldier_larm_empty2naked;
    //            case WeaponLeftArmType.Shield: return VoxelModelName.modsoldier_larm_shield2naked;
    //        }
    //    }

    //    public override VoxelModelName Right(WeaponRightArmType type)
    //    {
    //        switch (type)
    //        {
    //            default: return VoxelModelName.modsoldier_rarm_sword2naked;
    //            case WeaponRightArmType.Bow: return VoxelModelName.modsoldier_rarm_bow2naked;
    //        }
    //    }
    //}


    enum CharacterHatGenre
    {
        NoHat,
        FollowWeapon,
        FollowArmor,
        Uniform,
    }

    enum WeaponRightArmType
    {
        Sword,
        Bow,
    }
    enum WeaponLeftArmType
    {
        None,
        Shield,
    }

    enum FaceTheme
    {
        Default,
        Orc,
        Skeleton,
        NUM,
    }
    enum ArmsTheme
    {
        Default,
        Naked,
        NUM,
    }
}
