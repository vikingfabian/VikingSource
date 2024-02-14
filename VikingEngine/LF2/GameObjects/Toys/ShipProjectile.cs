//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;

//namespace Game1.LootFest.GameObjects.Toys
//{
//    class ShipProjectile : ToyProjectile
//    {
//        public ShipProjectile(float damage, Vector3 startPos, Vector3 speed, ByteVector2 parentIndex)
//            : base(damage, startPos, speed, Vector3.Zero, parentIndex)
//        {

//        }
//        public ShipProjectile(System.IO.BinaryReader System.IO.BinaryReader)
//            : base(r)
//        {

//        }
//        const float Gravity = LootFest.AbsPhysics.StandardGravity * 0.001f;
//        protected override float gravity
//        {
//            get
//            {
//                return Gravity;
//            }
//        }
//        public override int UnderType
//        {
//            get
//            {
//                return (int)WeaponAttack.WeaponUtype.ToyBoatProjectile;
//            }
//        }
//    }
//}
