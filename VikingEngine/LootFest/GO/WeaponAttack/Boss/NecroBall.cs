﻿//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest.GO.WeaponAttack.Boss
//{
//    abstract class AbsNecroProjectile : Linear3DProjectile
//    {
//        protected const float ScaleToBound = 0.2f;

//        public AbsNecroProjectile()
//            : base()
//        {
//            rotateSpeed = Ref.rnd.Vector3_Sq(Vector3.Zero, 0.04f);
//        }

//        public override void Time_Update(UpdateArgs args)
//        {
//            base.Time_Update(args);

//            Engine.ParticleHandler.AddParticleArea(
//               Ref.rnd.RandomChance(80) ? Graphics.ParticleSystemType.Smoke : Graphics.ParticleSystemType.Fire,
//               image.Position, ImageScale * PublicConstants.Half, 2);
//        }

//        public override void DeleteMe(bool local)
//        {
            
//            base.DeleteMe(local);

//            if (!Engine.Update.IsRunningSlow)
//            {
//                float scale = image.OneBlockScale * 2;
//                Vector3 pos = image.Position;
//                pos.Y += 3;
//                for (int i = 0; i < 4; i++)
//                {
//                    new Effects.BouncingBlock2(pos, DamageColors.GetRandom(), scale);
//                }
//                for (int i = 0; i < 8; i++)
//                {
//                    new Effects.BouncingBlock2Dummie(pos, DamageColors.GetRandom(), scale);
//                }
//            }
            
//            Effects.EffectLib.VibrationCenter(image.Position, 10f, 300, 20); 
//        }
//        static readonly Effects.BouncingBlockColors BouncingBlockColors = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
//        public override Effects.BouncingBlockColors DamageColors
//        {
//            get
//            {
//                return BouncingBlockColors;
//            }
//        }

//        override protected VoxelModelName VoxelObjName
//        {
//            get { return VoxelModelName.endmagician_projectile; }
//        }
//        //protected override WeaponTrophyType weaponTrophyType
//        //{
//        //    get { return WeaponTrophyType.Other; }
//        //}
//    }

//    class NecroBall : AbsNecroProjectile
//    {
//        const float ProjectileSpeed = 0.02f;

//        public NecroBall(AbsUpdateObj parent,Vector3 startPos, AbsUpdateObj target)
//            : base()
//        {
//            linear3DProjectileSetup(new DamageData(2, WeaponUserType.Enemy,
//                NetworkId.Empty, Magic.MagicElement.Evil, SpecialDamage.NONE, WeaponPush.Large,
//                new Rotation1D(parent.AngleDirToObject(target))), startPos, target.Position, 0.05f, ProjectileSpeed, LootFest.ObjSingleBound.QuickBoundingBox(Scale * ScaleToBound));

//            lifeTime = lib.SecondsToMS(10);
            
//            NetworkShareObject();
//        }

//        const float Scale = 2f;
//        override protected float ImageScale
//        {
//            get { return Scale; }
//        }
//        public override GameObjectType Type
//        {
//            get { return GameObjectType.NecroBall; }
//        }

//        public override WeaponUserType WeaponTargetType
//        {
//            get
//            {
//                return base.WeaponTargetType;
//            }
//        }
//    }
//}
