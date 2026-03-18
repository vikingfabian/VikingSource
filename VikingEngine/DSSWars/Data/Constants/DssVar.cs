using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.LootFest;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.DSSWars
{
    static class DssVar
    {
        public static float Men_AsynchCollisionGroupRadius;
        public static float StandardBoundRadius;
        public static float DefaultGroupSpacing;
        public static float SoldierGroup_Spacing;
        public static float SoldierGroup_Spacing_Radius;
        public static float SoldierGroup_CollisionRadius;
        public static float SoldierGroup_MoveCollisionRadius;

        public static float SoldierGroup_GridExtraSpacing;
        public static float Worker_StandardBoundRadius;
        public static float Men_StandardWalkingSpeed_PerSec;
        public static Vector3 WorkerUnit_ResourcePosDiff;

        public static AnimalModelData emptyAnimalModel;
        public static AnimalModelData fowlModel, henModel, pheasantModel;
        public static AnimalModelData boarModel;
        public static AnimalModelData oxenModel, kineOxenModel;
        public static AnimalModelData pigModel;
        public static AnimalModelData dogModel;
        public static AnimalModelData elephantModel, warElephantModel, oliphantModel;
        public static AnimalModelData ponyModel, horseModel, warHorseModel, draftHorseModel;
        public static AnimalModelData wolfModel, wargModel, alphaWargModel;
        public static AnimalModelData hogModel;
        public static AnimalModelData lionModel;

        public static Dictionary<ItemResourceType, ShieldProperties> Shields;
        public static void UpdateConstants()
        {
            emptyAnimalModel = new AnimalModelData(VoxelModelName.ErrorCube, DssConst.Men_StandardModelScale * 0.5f, new WalkingAnimation());

            henModel = new AnimalModelData(VoxelModelName.Hen, DssConst.Men_StandardModelScale * 0.3f, new WalkingAnimation(1, 4, WalkingAnimation.StandardMoveFrames * 0.25f));
            fowlModel = henModel.Copy(VoxelModelName.Fowl, 0.9f);
            pheasantModel = henModel.Copy(VoxelModelName.Pheasant, 1f);

            pigModel = new AnimalModelData(VoxelModelName.Pig, DssConst.Men_StandardModelScale * 0.5f, new WalkingAnimation(1, 2, WalkingAnimation.StandardMoveFrames));
            boarModel = pigModel.Copy(VoxelModelName.Boar, 0.8f);

            dogModel = new AnimalModelData(VoxelModelName.dog1, DssConst.Men_StandardModelScale * 0.6f, new WalkingAnimation(1, 4, WalkingAnimation.StandardMoveFrames * 1.1f));
            
            oxenModel = new AnimalModelData(VoxelModelName.oxen1, DssConst.Men_StandardModelScale * 1.1f, new WalkingAnimation(1, 4, WalkingAnimation.StandardMoveFrames * 1.1f));
            kineOxenModel = oxenModel.Copy(VoxelModelName.kineoxen1, 1.2f);

            horseModel = new AnimalModelData(VoxelModelName.horse_brown, DssConst.Men_StandardModelScale * 1.1f, new WalkingAnimation(1, 6, WalkingAnimation.StandardMoveFrames * 1f), 0.018f);
            ponyModel = horseModel.Copy(VoxelModelName.pony_brown, 0.8f);
            warHorseModel = horseModel.Copy(VoxelModelName.warhorse_brown, 1.1f);
            draftHorseModel = horseModel.Copy(VoxelModelName.drafthorse_red, 1.2f);

            wolfModel = new AnimalModelData(VoxelModelName.wolf1, DssConst.Men_StandardModelScale * 1.1f, new WalkingAnimation(2, 6, WalkingAnimation.StandardMoveFrames * 0.9f), 0.018f);
            wargModel = wolfModel.Copy(VoxelModelName.warg1, 1.1f);
            alphaWargModel = wargModel.Copy(VoxelModelName.alphawarg1, 1.1f);

            hogModel = new AnimalModelData(VoxelModelName.hog1, DssConst.Men_StandardModelScale * 1.1f, new WalkingAnimation(1, 4, WalkingAnimation.StandardMoveFrames * 1f), 0.013f, 0.2f);
            lionModel = new AnimalModelData(VoxelModelName.lion1, DssConst.Men_StandardModelScale * 1.1f, new WalkingAnimation(2, 6, WalkingAnimation.StandardMoveFrames * 0.9f));


            elephantModel = new AnimalModelData(VoxelModelName.Elephant_default, DssConst.Men_StandardModelScale * 1.9f, new WalkingAnimation(1, 2, WalkingAnimation.StandardMoveFrames * 2));
            elephantModel.riderY = 0.38f * elephantModel.scale;
            warElephantModel = elephantModel.Copy(VoxelModelName.Elephant_war, 1.1f);
            oliphantModel = warElephantModel.Copy(VoxelModelName.Elephant_oli, 1.5f);
            //    public static readonly float HorseScale = DssConst.Men_StandardModelScale * 1.1f;
            //public static readonly float HogScale = DssConst.Men_StandardModelScale * 1.1f;
            //public static readonly float LionScale = DssConst.Men_StandardModelScale * 1.1f;
            //public static readonly float WolfScale = DssConst.Men_StandardModelScale * 1.1f;
            //public static readonly float ElephantScale = DssConst.Men_StandardModelScale * 1.9f;

            //public static readonly WalkingAnimation HorseAnimation = new WalkingAnimation(1, 6, WalkingAnimation.StandardMoveFrames * 1f);
            //public static readonly WalkingAnimation HogAnimation = new WalkingAnimation(1, 4, WalkingAnimation.StandardMoveFrames * 1f);
            //public static readonly WalkingAnimation LionAnimation = new WalkingAnimation(2, 6, WalkingAnimation.StandardMoveFrames * 0.9f);
            //public static readonly WalkingAnimation WolfAnimation = new WalkingAnimation(2, 6, WalkingAnimation.StandardMoveFrames * 0.9f);
            //public static readonly WalkingAnimation ElephantAnimation = new WalkingAnimation(1, 2, WalkingAnimation.StandardMoveFrames * 2);




            Projectile.Projectile_PeekHeight = DssConst.Men_StandardModelScale * 1f;
            Men_AsynchCollisionGroupRadius = StandardBoundRadius * 2f;
            StandardBoundRadius = 0.4f * DssConst.Men_StandardModelScale;
            DefaultGroupSpacing = StandardBoundRadius * 2.5f;
            SoldierGroup_Spacing = DssConst.SoldierGroup_RowWidth * DefaultGroupSpacing * 1.15f;
            SoldierGroup_Spacing_Radius = SoldierGroup_Spacing * 0.5f;
            SoldierGroup_CollisionRadius = DssConst.SoldierGroup_RowWidth * DefaultGroupSpacing * 0.45f;
            SoldierGroup_MoveCollisionRadius = SoldierGroup_CollisionRadius * 0.6f;

            SoldierGroup_GridExtraSpacing = DefaultGroupSpacing;
            Worker_StandardBoundRadius = StandardBoundRadius * 2f;
            Men_StandardWalkingSpeed_PerSec = DssConst.Men_StandardWalkingSpeed * TimeExt.SecondToMs;
            WorkerUnit_ResourcePosDiff = new Vector3(0, DssConst.Men_StandardModelScale * 1.2f, DssConst.Men_StandardModelScale * 0.25f);

            Shields = new Dictionary<ItemResourceType, ShieldProperties>
            {
                { ItemResourceType.NONE, new ShieldProperties() },
                { ItemResourceType.BucklerShield, new ShieldProperties() { blocksRefillTimeSecMultiply = 2, meleeSpeedBonus = 0.4f} },
                { ItemResourceType.RoundShield, new ShieldProperties() { blocksRefillTimeSecMultiply = 3f, meleeSpeedBonus = 0.2f, armorBonus = MathExt.MultiplyInt(DssConst.Soldier_DefaultHealth, 0.25)  } },
                { ItemResourceType.HeaterShield, new ShieldProperties() { blocksRefillTimeSecMultiply = 5, meleeSpeedBonus = 0.0f, armorBonus = MathExt.MultiplyInt(DssConst.Soldier_DefaultHealth, 0.5), moveSpeedMultiply = 0.9f }},
                { ItemResourceType.TowerShield, new ShieldProperties() { blocksRefillTimeSecMultiply = 8, meleeSpeedBonus = -0.4f, armorBonus = MathExt.MultiplyInt(DssConst.Soldier_DefaultHealth, 2), moveSpeedMultiply = 0.7f }},
            };

            BloodBlock.UpdateConstants();
            SoldierGroup.Init();
        }
    }
}
