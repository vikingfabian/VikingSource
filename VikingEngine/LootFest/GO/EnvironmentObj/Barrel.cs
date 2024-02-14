using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    class AbsCarryObject : AbsGameObject
    {
         

        public bool isCarried = false;
        public bool isThrown = false;

        public AbsCarryObject(GoArgs args, VoxelModelName modelName, float modelScale, BoundSaveData BoundSett)
            : base(args)
        {
            WorldPos = args.startWp;
            WorldPos.SetAtClosestFreeY(2);

            Health = 0.2f;

            this.modelScale = modelScale;//modelScale = 2.2f;

            image = LfRef.modelLoad.AutoLoadModelInstance(modelName, modelScale, 0, false);
            image.position = WorldPos.PositionV3;
            CollisionAndDefaultBound = new Bounds.ObjectBound(BoundSett, this);
            //createBarrelModelAndBound();

            BoundSaveData pickupBoundSett = BoundSett.Clone();
            pickupBoundSett.scale *= 1.6f;
            DamageBound = new Bounds.ObjectBound(pickupBoundSett, this);

        }

       //abstract protected void createBarrelModelAndBound();

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);

            if (localMember && !isCarried)
            {
                if (managedGameObject)
                {
                    checkSleepingState();
                }
                else
                {
                    if (checkOutsideUpdateArea_StartChunk())
                    {
                        DeleteMe();
                        return;
                    }
                }

                physics.Update(args.time);
                SolidBodyCheck(args.localMembersCounter);

                
            }
        }

        public override float Scale1D
        {
            get
            {
                return modelScale;
            }
        }

        public override void HandleColl3D(Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            if (ObjCollision != null && isThrown && !(ObjCollision is PlayerCharacter.AbsHero))
            {
                new Timer.Action1ArgTrigger<AbsUpdateObj>(onObjectThrowHit, ObjCollision);
            }
            base.HandleColl3D(collData, ObjCollision);
        }

        void onObjectThrowHit(AbsUpdateObj obj)
        {
            if (this.Alive)
            {
                Health = 0;
                BlockSplatter();
                DeleteMe();

                obj.stunForce(1, LfLib.HeroStunDamage, false, true);
            }
        }
        

        public override void onGroundPounce(float fallSpeed)
        {
            if (isThrown)
            {
                base.onGroundPounce(fallSpeed);
                Health = 0;
                BlockSplatter();
                DeleteMe();
            }
        }


        public override void AsynchGOUpdate(UpdateArgs args)
        {
            if (!isCarried)
            {

                var hero = GetClosestHero(true);

                if (hero.carryObject == null && hero.CollisionAndDefaultBound.Intersect2(DamageBound) != null)
                {
                    new Timer.Action1ArgTrigger<AbsUpdateObj>(hero.InteractPrompt_ver2, this);
                    //new Process.UnthreadedInteractPrompt(this, hero);
                }
                //float dist = distanceToObject(target);
                //if (target != null && distanceToObject(target) > MaxSlideDistance)
                //{
                //    target = null;
                //}
            }
        }

        //public override bool IsWeaponTarget
        //{
        //    get
        //    {
        //        return base.IsWeaponTarget;
        //    }
        //}
        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            base.handleDamage(damage, local);

            BlockSplatter();
        }

        protected override RecieveDamageType recieveDamageType
        {
            get
            {
                return RecieveDamageType.ReceiveDamage;
            }
        }
        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.PassiveEnemy;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.ExplodingBarrel; }
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.BouncingObj2;
            }
        }

        public override bool SolidBody
        {
            get
            {
                return true;
            }
        }

        public override SpriteName InteractVersion2_interactIcon
        {
            get
            {
                return SpriteName.LfBarrelIcon;
            }
        }
        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            hero.PickUpCarryObject(this);
        }

        public override bool ChildObject_Update(Characters.AbsCharacter parent)
        {
            Vector3 offset = new Vector3(0, 2.6f, 0);

            image.position = parent.RotationQuarterion.TranslateAlongAxis(offset, parent.Position);
            image.Rotation = parent.RotationQuarterion;

            return !isCarried;
        }

        
    
    }

    class Barrel : AbsCarryObject
    {
        //static readonly Data.TempBlockReplacementSett TempModel = new Data.TempBlockReplacementSett(
            //new Color(161, 106, 24), new Vector3(1.2f, 1.2f, 1.8f));
        static readonly BoundSaveData BoundSett = new BoundSaveData(Bounds.BoundShape.Cylinder,
            new Vector3(0.45f, 0.5f, 0), new Vector3(0, 0.5f, 0));

        public Barrel(GoArgs args)
            : base(args, VoxelModelName.barrel, 2.2f, BoundSett)
        { }

        static readonly Effects.BouncingBlockColors DmgCols = new Effects.BouncingBlockColors(
            Data.MaterialType.dark_yellow_orange,
            Data.MaterialType.dark_red_orange);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DmgCols;
            }
        }
    }

    class Snowman : AbsCarryObject
    {
        //static readonly Data.TempBlockReplacementSett TempModel = new Data.TempBlockReplacementSett(
            //Color.White, new Vector3(1.2f, 1.2f, 1.8f));
        static readonly BoundSaveData BoundSett = new BoundSaveData(Bounds.BoundShape.Cylinder,
            new Vector3(0.45f, 0.5f, 0), new Vector3(0, 0.5f, 0));

        public Snowman(GoArgs args)
            : base(args, VoxelModelName.snowman, 4f, BoundSett)
        { }

        static readonly Effects.BouncingBlockColors DmgCols = new Effects.BouncingBlockColors(
            Data.MaterialType.snow,
             Data.MaterialType.dark_red_orange,
            Data.MaterialType.light_blue);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DmgCols;
            }
        }
    }   
}
