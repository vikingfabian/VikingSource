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

        public static AnimalModelData pigModel;
        public static AnimalModelData dogModel;

        public static Dictionary<ItemResourceType, ShieldProperties> Shields;
        public static void UpdateConstants()
        {
            pigModel = new AnimalModelData(VoxelModelName.Pig, DssConst.Men_StandardModelScale * 0.5f, new WalkingAnimation(1, 2, WalkingAnimation.StandardMoveFrames));
            dogModel = new AnimalModelData(VoxelModelName.dog1, DssConst.Men_StandardModelScale * 0.6f, new WalkingAnimation(1, 4, WalkingAnimation.StandardMoveFrames * 1.1f));
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
