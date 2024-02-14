//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest.GO.WeaponAttack.Boss
//{


//    class NecroMetroid : AbsNecroProjectile
//    {
//        const float FallSpeed = -0.02f;
//        const float SideSpeed = FallSpeed * 0.1f;
        

//        public NecroMetroid(Vector3 startpos, Rotation1D dir)
//            :base()
//        {
//            linear3DProjectileSetup(new DamageData(1, WeaponUserType.Enemy,
//                NetworkId.Empty, Magic.MagicElement.Evil),
//                startpos,
//                dir.Direction(SideSpeed), FallSpeed,
//                LootFest.ObjSingleBound.QuickBoundingBox(Scale * ScaleToBound));
//            lifeTime = lib.SecondsToMS(25);
//            rotateSpeed = Ref.rnd.Vector3_Sq(Vector3.Zero, 0.04f);
//            NetworkShareObject();

//        }

//        public static void MetroidRain(Vector3 center, Rotation1D dir)
//        {
//            IntervalF radius = new IntervalF(6, 40);
//            IntervalF height = new IntervalF(36, 64);
//            int num = 6 + Ref.rnd.Int(4);

//            float angle = MathHelper.TwoPi / num;
//            Rotation1D currentangle = Rotation1D.D0;

//            for (int i = 0; i < num; ++i)
//            {
//                new NecroMetroid(center + new Vector2toV3(currentangle.Direction(radius.GetRandom()), height.GetRandom()), dir);
//                currentangle.Add(angle);
//            }

//        }

//        const float Scale = 4f;
//        override protected float ImageScale
//        {
//            get { return Scale; }
//        }

//        public override GameObjectType Type
//        {
//            get { return GameObjectType.NecroMetroite; }
//        }
//    }
//}
