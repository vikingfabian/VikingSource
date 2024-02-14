//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest.GO.WeaponAttack
//{
//    class MagicianThrowSword : Linear3DProjectile
//    {
//        AbsVoxelObj parent;
//        static readonly GO.Bounds.ObjectBound ProjectileBound = LootFest.ObjSingleBound.QuickBoundingBox(1.5f);
//        float velocity;
//        float scale;

//        public MagicianThrowSword(AbsVoxelObj parent, float velocity, WeaponAttack.DamageData wepDamage, float scale)
//            : base( ,wepDamage, parent.Position, parent.Rotation, ProjectileBound, velocity)
//        {
//            this.scale = scale * 16;
//            this.parent = parent;
//            this.velocity = velocity;
//            //Vector3 pos = parent.Position;
//            //pos.Y = lib.SetMinFloatVal(pos.Y, 13);
//            imageSetup(parent.Position);

//            rotateSpeed.X = 0.25f;
//            lifeTime = 3600;
//        }

//        protected override VoxelModelName VoxelObjName
//        {
//            get { 
//                return VoxelModelName.magician_sword; }
//        }
//        //static readonly Vector3 ProjectileScale = VectorExt.V3(0.02f);
//        protected override float ImageScale
//        {
//            get { return scale; }
//        }

//        protected override bool autoImageSetup
//        {
//            get
//            {
//                return false;
//            }
//        }

//        public override GameObjectType Type
//        {
//            get { return GameObjectType.MagicianThrowSword; }
//        }
//        //protected override WeaponTrophyType weaponTrophyType
//        //{
//        //    get { return WeaponTrophyType.Other; }
//        //}
//        protected override bool LocalDamageCheck
//        {
//            get
//            {
//                return true;
//            }
//        }

//        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
//        {
//            //base.HandleColl3D(collData, ObjCollision);
//        }
//    }
//}
