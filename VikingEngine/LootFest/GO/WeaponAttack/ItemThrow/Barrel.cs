//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest.GO.WeaponAttack.ItemThrow
//{
//    class Barrel : AbsVoxelObj
//    {
//        public //static readonly Data.TempBlockReplacementSett BarrelTempImage = new Data.TempBlockReplacementSett(new Color(104,72,30), new Vector3(1.5f, 2, 1.5f));

//        AbsUpdateObj callBackObj;
//        public const float BarrelScale = 1.8f;
//        //int numBounces = 2;
//        float lifeTime = 2000 + Ref.rnd.Int(500);

//        public Barrel(bool thrown, Rotation1D dir, Vector3 position,  AbsUpdateObj callBackObj)
//        {
//            this.callBackObj = callBackObj;
//            createImage();
//            image.Position = position;
//            basicInit(dir, thrown);
//            NetworkShareObject();
//        }
//        public Barrel(bool thrown, Rotation1D dir, Graphics.AbsVoxelObj barrelImg,  AbsUpdateObj callBackObj)
//            :base()
//        {
//            this.callBackObj = callBackObj;
//             image = barrelImg;
//             image.Position.Y += 1;
//             basicInit(dir, thrown);
//             NetworkShareObject();
//        }

//        public Barrel(System.IO.BinaryReader r)
//            :base(r)
//        {
//            createImage();
//        }

//        void createImage()
//        {
//            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.barrelX, BarrelTempImage, BarrelScale, 1, false);
//        }

//        protected void basicInit(Rotation1D dir, bool thrown)
//        {
//            WorldPos = new Map.WorldPosition(image.Position);
//            Velocity.Set(dir, thrown? 0.008f : 0.001f);
//            physics.SpeedY = 0.012f;

//            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBound(0.48f * BarrelScale, 0.6f * BarrelScale);
//            TerrainInteractBound = LootFest.ObjSingleBound.QuickBoundingBox(BarrelScale);
//        }

//        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
//        {
//           // numBounces--;
//        }

//        static readonly GO.WeaponAttack.DamageData ExplosionDamage = new GO.WeaponAttack.DamageData(LfLib.HeroNormalAttack);
//        static readonly IntervalF ExplosionRadius = new IntervalF(6, 7);
//        public override void Time_Update(UpdateArgs args)
//        {


//            base.Time_Update(args);
//            if (localMember)
//                physics.Update(args.time);
//            lifeTime -= args.time;
//            if (lifeTime <= 0)
//            {
//                if (localMember)
//                    new GO.WeaponAttack.Explosion(args.allMembersCounter, image.Position, ExplosionDamage, ExplosionRadius.GetRandom(), Data.MaterialType.lava, true, true, callBackObj);
                        
//                this.DeleteMe();

//                for (int i = 0; i < 6; i++)
//                {
//                    new Effects.BouncingBlock2(image.Position, Data.MaterialType.planks_bright_hori, 0.2f);
//                }
                    
//            }

//            Graphics.ParticleInitData p =  new Graphics.ParticleInitData(image.Position + Vector3.Up * 2, Vector3.Up);
//            Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, p);
//            if (Ref.rnd.RandomChance(30))
//            {
//                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, p);
//            }


//        }


//       protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
//        {
//            character.TakeDamage(WeaponAttack.DamageData.BasicCollDamage, true);
//            return false;
//        }

//        public override ObjPhysicsType PhysicsType
//        {
//            get
//            {
//                return ObjPhysicsType.BouncingObj;
//            }
//        }

       
//        public override GameObjectType Type
//        {
//            get { return GameObjectType.ExplodingBarrel; }
//        }
//        public override WeaponUserType WeaponTargetType
//        {
//            get
//            {
//                return WeaponAttack.WeaponUserType.NON;
//            }
//        }

//        public override NetworkShare NetworkShareSettings
//        {
//            get
//            {
//                return GO.NetworkShare.FullExceptRemoval;
//            }
//        }
       
//    }
//}
