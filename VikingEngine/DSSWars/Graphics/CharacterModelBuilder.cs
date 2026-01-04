using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars
{
    class CharacterModelBuilder : Voxels.ModelBuilder
    {
        //modsoldier_hat_allclasses
        /*Hats
         * 0 soldier
         * 1 archer
         * 2 slinger
         * 3 javelin
         * 4 bannerman
         * 5 hirdman
         * 6 hammer
         * 7 pike
         * 8 longsword
         * 9 viking
         * 10 crossbow
         * 11 gun far
         * 12 gun near
         * 13 farmer
         * 14 zweihander
         */


        /*Weapon animation
         * 0 idle
         * 1 attack
         * 2 move
         */

        static readonly IntVector3 GridSize = new IntVector3(26, 48, 78);
        const int FrameCount = 7;

        //26*48*78

        public Graphics.VoxelModel buildModel(PlayerProfile profile, SoldierModelData modelData)
        {
            IntVector3 hatOffset = new IntVector3(2, 5, 30);
            VoxelModelName weaponModel;
            int weaponHatFrame;
            int hatFrame = profile.character.customHat;
            
            int faceFrame = 0;
            int armorFrame = (int)modelData.armor;
            VoxelModelName shield = VoxelModelName.NUM_NON;

            WeaponLeftArmType weaponLeftArmType = WeaponLeftArmType.None;
            WeaponRightArmType weaponRightArmType = WeaponRightArmType.Sword;
           
            switch (modelData.weapon)
            {
                case Resource.ItemResourceType.Settler:
                    weaponModel = VoxelModelName.modweapon_settler;
                    weaponRightArmType = WeaponRightArmType.Bow;
                    weaponHatFrame = 15;
                    break;
                case Resource.ItemResourceType.SharpStick:
                    weaponModel = VoxelModelName.modweapon_sharpstick;
                    weaponRightArmType = WeaponRightArmType.Bow;
                    weaponHatFrame = 13;
                    break;
                case Resource.ItemResourceType.BronzeSword:
                    weaponModel = VoxelModelName.modweapon_bronzesword;
                    weaponHatFrame = 0;
                    break;
                case Resource.ItemResourceType.ShortSword:
                    weaponModel = VoxelModelName.modweapon_shortsword;
                    weaponHatFrame = 0;
                    break;
                case Resource.ItemResourceType.Sword:
                    weaponModel = VoxelModelName.modweapon_sword1;
                    weaponHatFrame = 0;
                    break;
                case Resource.ItemResourceType.LongSword:
                    weaponModel = VoxelModelName.modweapon_longsword;
                    weaponHatFrame = 8;
                    break;
                case Resource.ItemResourceType.HandSpear:
                    weaponModel = VoxelModelName.modweapon_spear;
                    weaponHatFrame = 0;//missing
                    shield = VoxelModelName.modshield_roman;
                    weaponLeftArmType = WeaponLeftArmType.Shield;
                    break;

                case Resource.ItemResourceType.SlingShot:
                    weaponModel = VoxelModelName.modweapon_sling;
                    weaponRightArmType = WeaponRightArmType.Bow;
                    weaponHatFrame = 2;
                    break;
                case Resource.ItemResourceType.ThrowingSpear:
                    weaponModel = VoxelModelName.modweapon_javelin;
                    weaponHatFrame = 3;

                    shield = VoxelModelName.modshield_javelin;

                    weaponLeftArmType = WeaponLeftArmType.Shield;
                    break;

                case Resource.ItemResourceType.TwoHandSword:
                    weaponModel = VoxelModelName.modweapon_twohand;
                    weaponHatFrame = 14;
                    break;
                case Resource.ItemResourceType.Warhammer:
                    weaponModel = VoxelModelName.modweapon_hammer;
                    weaponRightArmType = WeaponRightArmType.Bow;
                    shield = VoxelModelName.modshield_knightsmallside;
                    
                    weaponHatFrame = 6;
                    break;
                case Resource.ItemResourceType.MithrilBow:
                    weaponModel = VoxelModelName.modweapon_mithrilbow;
                    weaponRightArmType = WeaponRightArmType.Bow;
                    weaponHatFrame = 5;
                    break;
                case Resource.ItemResourceType.MithrilSword:
                    weaponModel = VoxelModelName.modweapon_mithrilsword;
                    weaponRightArmType = WeaponRightArmType.Bow;
                    weaponHatFrame = 5;
                    break;

                case Resource.ItemResourceType.Bow:
                    weaponModel = VoxelModelName.modweapon_shortbow;
                    weaponRightArmType = WeaponRightArmType.Bow;
                    weaponHatFrame = 1;
                    break;
                case Resource.ItemResourceType.LongBow:
                    weaponModel = VoxelModelName.modweapon_longbow;
                    weaponRightArmType = WeaponRightArmType.Bow;
                    weaponHatFrame = 1;
                    break;
                case Resource.ItemResourceType.Crossbow:
                    weaponModel = VoxelModelName.modweapon_crossbow;
                    weaponRightArmType = WeaponRightArmType.Bow;
                    weaponHatFrame = 10;
                    break;

                case Resource.ItemResourceType.HandCannon:
                    weaponModel = VoxelModelName.modweapon_handcannon;
                    weaponRightArmType = WeaponRightArmType.Bow;
                    weaponHatFrame = 11;
                    break;
                case Resource.ItemResourceType.HandCulverin:
                    weaponModel = VoxelModelName.modweapon_culvertin;
                    weaponRightArmType = WeaponRightArmType.Bow;
                    weaponHatFrame = 12;
                    break;
                case Resource.ItemResourceType.Rifle:
                    weaponModel = VoxelModelName.modweapon_rifle;
                    weaponRightArmType = WeaponRightArmType.Bow;
                    weaponHatFrame = 11;
                    break;
                case Resource.ItemResourceType.Blunderbuss:
                    weaponModel = VoxelModelName.modweapon_blunderbuss;
                    weaponRightArmType = WeaponRightArmType.Bow;
                    weaponHatFrame = 12;
                    break;

                default:
                    weaponModel = VoxelModelName.modweapon_sword1;
                    weaponHatFrame = 0;
                    break;
            }


            ArmThemeModels armtheme = CharacterTheme.arm(profile.character.arms,
                weaponLeftArmType, weaponRightArmType);
            
            switch (modelData.specialization)
            {
                case Conscript.SpecializationType.HonorGuard:
                    weaponHatFrame = 5;
                    shield = VoxelModelName.modshield_roman;
                    weaponLeftArmType = WeaponLeftArmType.Shield;
                    break;
                case Conscript.SpecializationType.Viking:
                    weaponHatFrame = 9;
                    break;
            }

            VoxelModelName hatmodel;
            switch (profile.character.hatGenre)
            { 
                default:
                    hatmodel = VoxelModelName.modsoldier_hat_soldier_all;
                    hatFrame = weaponHatFrame;
                    break;
                case CharacterHatGenre.NoHat:
                    hatmodel = VoxelModelName.NUM_NON;
                    break;
                case CharacterHatGenre.Uniform:
                    hatmodel = VoxelModelName.modsoldier_hat_custom_all;
                    hatOffset.Y = 5;
                    break;
            }

            

            VoxelModelName faceModel;

            switch (profile.character.face)
            {
                default:
                    faceModel = VoxelModelName.modsoldier_face1;
                    break;
                case  FaceTheme.Orc:
                    faceModel = VoxelModelName.modsoldier_face_orc;
                    break;
                case  FaceTheme.Skeleton:
                    faceModel = VoxelModelName.modsoldier_face_skull;
                    break;

            }

            VoxelModelName bodyModel;

            switch (profile.character.body)
            {
                default:
                    bodyModel = VoxelModelName.modsoldier_body1;
                    break;
                case 1:
                    bodyModel = VoxelModelName.modsoldier_body_beef1;
                    break;
                case 2:
                    bodyModel = VoxelModelName.modsoldier_body3lady;
                    break;

            }

            IntVector3 legOffSet = new IntVector3(5, 0, 32);
            IntVector3 rArmOffSet = new IntVector3(4, 0, 30);
            IntVector3 lArmOffset = VectorExt.AddX(rArmOffSet, 9);
            IntVector3 faceOffset = new IntVector3(2, 5, 30);

            VoxelObjGridDataAnimHD grid = new VoxelObjGridDataAnimHD(GridSize, FrameCount);

            //var debug = DssRef.models.rawModels[VoxelModelName.modsoldier_debug];
            var face = DssRef.models.rawModels[faceModel];
            
            var body = DssRef.models.rawModels[bodyModel];
            var leg = DssRef.models.rawModels[VoxelModelName.modsoldier_leg1];

           
            var leftArm = DssRef.models.rawModels[armtheme.left];
            var rightArm = DssRef.models.rawModels[armtheme.right];

            var rightHandItem = DssRef.models.weaponModels[weaponModel];
            
            var profileColors = profile.flag.GetColorReplaceTable();

            var legsIdle = leg.Frame(0).GetVoxelArray(legOffSet, profileColors, GridSize);
            for (int frame = 0; frame < 2; frame++)
            {                
                grid.Frame(frame).AddVoxels(legsIdle);
            }
            for (int frame = 2; frame < FrameCount; frame++)
            {
                grid.Frame(frame).AddVoxels(leg.Frame(frame).GetVoxelArray(legOffSet, profileColors, GridSize));
            }

            var bodyVoxels = body.Frame(armorFrame).GetVoxelArray(new IntVector3(4, 0, 33), profileColors, GridSize);
            var faceVoxels = face.Frame(faceFrame).GetVoxelArray(faceOffset, profileColors, GridSize);
            var faceBlinkVoxels = face.Frame(faceFrame +1).GetVoxelArray(faceOffset, profileColors, GridSize);

            WeaponModel leftHandItemVoxels = null;
            ushort leftHandItemJointValue = 0;
            if (shield != VoxelModelName.NUM_NON)
            {
                leftHandItemVoxels = DssRef.models.weaponModels[shield].recolor(profileColors);
                leftHandItemJointValue = leftHandItemVoxels.idle_jointPos.value;
                //var shieldVoxels = shieldModel.Frame(0].GetVoxelArray(new IntVector3(6, 0, 33), profileColors, GridSize);
            }

            for (int frame = 0; frame < FrameCount; frame++)
            {
                grid.Frame(frame).AddVoxels(frame == 1 ? faceBlinkVoxels : faceVoxels);
            }
                        
            if (hatmodel != VoxelModelName.NUM_NON)
            {
                VoxelObjGridDataAnimHD hat = DssRef.models.rawModels[hatmodel];
                List<VoxelHD> hatVoxels = null;
                hatVoxels = hat.Frame(hatFrame).GetVoxelArray(hatOffset, profileColors, GridSize);
                for (int frame = 0; frame < FrameCount; frame++)
                {
                    grid.Frame(frame).AddVoxels(hatVoxels);
                }
            }

            var larmIdle = leftArm.Frame(0).GetVoxelArray(lArmOffset, profileColors, leftHandItemJointValue, out IntVector3 larm_jointPos);
            var rarmIdle = rightArm.Frame(0).GetVoxelArray(rArmOffSet, profileColors, rightHandItem.idle_jointPos.value, out IntVector3 rarm_jointPos);
            
            for (int frame = 0; frame < 2; frame++)
            {
                grid.Frame(frame).AddVoxels(larmIdle);
                grid.Frame(frame).AddVoxels(rarmIdle);

                if (leftHandItemVoxels != null)
                { 
                    leftHandItemVoxels.addToGrid(grid.Frame(frame), adjustJointPos(leftHandItemVoxels.idle_jointPos, larm_jointPos), WeaponModel.IdleFrame);
                }
                rightHandItem.addToGrid(grid.Frame(frame), adjustJointPos(rightHandItem.idle_jointPos, rarm_jointPos)/*rarm_jointPos*/, WeaponModel.IdleFrame);
            }

            for (int frame = 2; frame < FrameCount; frame++)
            {
                bool attackFrame = frame == 2;

                grid.Frame(frame).AddVoxels(leftArm.Frame(frame - 1).GetVoxelArray(lArmOffset, profileColors, out _, out larm_jointPos));

                VoxelJoint rightHandJointValue = attackFrame ? rightHandItem.attack_jointPos : rightHandItem.move_jointPos;
                grid.Frame(frame).AddVoxels(rightArm.Frame(frame - 1).GetVoxelArray(rArmOffSet, profileColors,
                    rightHandJointValue.value, out rarm_jointPos));

                if (leftHandItemVoxels != null)
                {
                    leftHandItemVoxels.addToGrid(grid.Frame(frame), adjustJointPos(leftHandItemVoxels.idle_jointPos, larm_jointPos), WeaponModel.IdleFrame);
                }
                rightHandItem.addToGrid(grid.Frame(frame), adjustJointPos(rightHandJointValue, rarm_jointPos)/*rarm_jointPos*/, attackFrame ? WeaponModel.AttackFrame : WeaponModel.MoveFrame);
            }

            for (int frame = 0; frame < FrameCount; frame++)
            {
                grid.Frame(frame).AddVoxels(bodyVoxels);
            }

            accessory(profile.character.accessoryBack, VoxelModelName.modsoldier_addons, IntVector3.Zero);
            accessory(profile.character.accessoryFace, VoxelModelName.modsoldier_face_access, faceOffset);

            var centerAdjust = grid.Frame(0).BottomCenterAdj();
            buildVerticeDataHD_ColorNormal(grid.Frames, centerAdjust);
            Graphics.VoxelModel model = modelFromVertices();

            return model;


            void accessory(int index, VoxelModelName model, IntVector3 offset)
            {
                if (index >= 0)
                {
                    var access = DssRef.models.rawModels[model];
                    if (index < access.Frames.Count)
                    {
                        var accessVoxels = access.Frame(index).GetVoxelArray(offset, profileColors, GridSize);
                        for (int frame = 0; frame < FrameCount; frame++)
                        {
                            grid.Frame(frame).AddVoxels(accessVoxels);
                        }
                    }
                }
            }
        }

        IntVector3 adjustJointPos(VoxelJoint joint, IntVector3 pos)
        {
            if (joint.value == BlockHD.JointForward)
            {
                pos.Z -= 1;
            }
            else
            {
                pos.Y -= 1;
            }

            return pos;
        }
    }

    
}
