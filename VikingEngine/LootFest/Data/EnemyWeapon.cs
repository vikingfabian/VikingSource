using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

//namespace VikingEngine.LootFest.Data.Characters
//{
//    class EnemyWeapon
//    {
//        const float ScaleBound = 0.4f;
//        const float ScaleLvl1 = 0.1f;
//        const float ScaleLvl2 = 0.2f;
//        const float ScaleLvl3 = 0.3f;


//        static readonly LootFest.ObjSingleBound[] Bound = new LootFest.ObjSingleBound[] 
//        {
//            LootFest.ObjSingleBound.QuickBoundingBox(ScaleLvl1 * ScaleBound),
//            LootFest.ObjSingleBound.QuickBoundingBox(ScaleLvl2 * ScaleBound),
//            LootFest.ObjSingleBound.QuickBoundingBox(ScaleLvl3 * ScaleBound),

//        };
//        static readonly Vector3[] ScalePerLevel = new Vector3[]
//        {
//            VectorExt.V3(ScaleLvl1),
//            VectorExt.V3(ScaleLvl2),
//            VectorExt.V3(ScaleLvl3)

//        };
        
//        Graphics.VoxelObj projectileMesh;
//        int projectileLevel;

//        public EnemyWeapon(float enemyScale)
//        {
//            projectileMesh = Editor.VoxelObjDataLoader.GetVoxelObj(VoxelObjName.EnemyProjectile, false);

//            if (enemyScale > 4)
//            {
//                projectileLevel = 2;
//            }
//            else if (enemyScale > 1.8f)
//            {
//                projectileLevel = 1;
//            }
//            else
//            {
//                projectileLevel = 0;
//            }

//        }
//        public void Fire(float angle, Vector3 pos)
//        {
//            new GO.Weapons.MonsterProjectile(GO.Weapons.DamageData.BasicCollDamage, pos, angle,
//                new Graphics.VoxelModelInstance(projectileMesh),
//                ScalePerLevel[projectileLevel], Bound[projectileLevel]);
//        }
//    }
//}
