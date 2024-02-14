using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.WeaponAttack.ItemThrow
{
    class Barrel : AbsVoxelObj
    {
        public static readonly Data.TempBlockReplacementSett BarrelTempImage = new Data.TempBlockReplacementSett(new Color(104,72,30), new Vector3(1.5f, 2, 1.5f));

        AbsUpdateObj callBackObj;
        public const float BarrelScale = 1.8f;
        //int numBounces = 2;
        float lifeTime = 2000 + Ref.rnd.Int(500);

        public Barrel(bool thrown, Rotation1D dir, Vector3 position,  AbsUpdateObj callBackObj)
        {
            this.callBackObj = callBackObj;
            createImage();
            image.position = position;
            basicInit(dir, thrown);
            NetworkShareObject();
        }
        public Barrel(bool thrown, Rotation1D dir, Graphics.AbsVoxelObj barrelImg,  AbsUpdateObj callBackObj)
            :base()
        {
            this.callBackObj = callBackObj;
             image = barrelImg;
             image.position.Y += 1;
             basicInit(dir, thrown);
             NetworkShareObject();
        }

        public Barrel(System.IO.BinaryReader r)
            :base(r)
        {
            createImage();
        }

        void createImage()
        {
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.barrelX, BarrelTempImage, BarrelScale, 1);
        }

        protected void basicInit(Rotation1D dir, bool thrown)
        {
            WorldPosition = new Map.WorldPosition(image.position);
            Velocity.Set(dir, thrown? 0.008f : 0.001f);
            physics.SpeedY = 0.012f;

            CollisionBound = LF2.ObjSingleBound.QuickCylinderBound(0.48f * BarrelScale, 0.6f * BarrelScale);
            TerrainInteractBound = LF2.ObjSingleBound.QuickBoundingBox(BarrelScale);
        }

        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
        {
           // numBounces--;
        }

        static readonly GameObjects.WeaponAttack.DamageData ExplosionDamage = new GameObjects.WeaponAttack.DamageData(LootfestLib.FireBombDamage);
        static readonly IntervalF ExplosionRadius = new IntervalF(6, 7);
        public override void Time_Update(UpdateArgs args)
        {


            base.Time_Update(args);
            if (localMember)
                physics.Update(args.time);
            lifeTime -= args.time;
            if (lifeTime <= 0)
            {
                if (localMember)
                    new GameObjects.WeaponAttack.Explosion(args.allMembersCounter, image.position, ExplosionDamage, ExplosionRadius.GetRandom(), Data.MaterialType.lava, true, true, callBackObj);
                        
                this.DeleteMe();

                for (int i = 0; i < 6; i++)
                {
                    new Effects.BouncingBlock2(image.position, Data.MaterialType.wood, 0.2f);
                }
                    
            }

            Graphics.ParticleInitData p =  new Graphics.ParticleInitData(image.position + Vector3.Up * 2, Vector3.Up);
            Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, p);
            if (Ref.rnd.RandomChance(30))
            {
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, p);
            }


        }


       protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            character.TakeDamage(WeaponAttack.DamageData.BasicCollDamage, true);
            return false;
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.BouncingObj;
            }
        }

        public override ObjectType Type
        {
            get { return ObjectType.WeaponAttack; }
        }
        public override int UnderType
        {
            get { return (int)WeaponUtype.ExplodingBarrel; }
        }
        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.NON;
            }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GameObjects.NetworkShare.FullExceptRemoval;
            }
        }
       
    }
}
