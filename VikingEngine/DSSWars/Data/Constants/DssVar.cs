using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars
{
    static class DssVar
    {
        public static float Men_AsynchCollisionGroupRadius;
        public static float StandardBoundRadius;
        public static float DefaultGroupSpacing;
        public static float SoldierGroup_Spacing;
        public static float SoldierGroup_CollisionRadius;
        public static float SoldierGroup_MoveCollisionRadius;

        public static float SoldierGroup_GridExtraSpacing;
        public static float Worker_StandardBoundRadius;
        public static float Men_StandardWalkingSpeed_PerSec;
        public static Vector3 WorkerUnit_ResourcePosDiff;
        public static void UpdateConstants()
        {
            Projectile.Projectile_PeekHeight = DssConst.Men_StandardModelScale * 1f;
            Men_AsynchCollisionGroupRadius = StandardBoundRadius * 2f;
            StandardBoundRadius = 0.4f * DssConst.Men_StandardModelScale;
            DefaultGroupSpacing = StandardBoundRadius * 2.5f;
            SoldierGroup_Spacing = DssConst.SoldierGroup_RowWidth * DefaultGroupSpacing * 1.15f;
            SoldierGroup_CollisionRadius = DssConst.SoldierGroup_RowWidth * DefaultGroupSpacing * 0.45f;
            SoldierGroup_MoveCollisionRadius = SoldierGroup_CollisionRadius * 0.6f;

            SoldierGroup_GridExtraSpacing = DefaultGroupSpacing;
            Worker_StandardBoundRadius = StandardBoundRadius * 2f;
            Men_StandardWalkingSpeed_PerSec = DssConst.Men_StandardWalkingSpeed * TimeExt.SecondToMs;
            WorkerUnit_ResourcePosDiff = new Vector3(0, DssConst.Men_StandardModelScale * 1.2f, DssConst.Men_StandardModelScale * 0.25f);

            BloodBlock.UpdateConstants();
            SoldierGroup.Init();
        }
    }
}
