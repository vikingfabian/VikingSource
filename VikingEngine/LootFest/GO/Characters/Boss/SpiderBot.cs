using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.GO.Bounds;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.GO.Characters.Boss
{
    class SpiderBot : AbsEnemy
    {
        const float WalkingSpeed = 0.003f;
        const float AttackSpeed = WalkingSpeed * 2f;
        const float NormalRotSpeed = 1f;
        const float AttackRotSpeed = NormalRotSpeed * 9f;

        public const float BodySize = 8f;

        float groundY = 0;
        SpiderbotGoblin goblin;
        SpiderBotLimb[] limbs;
        Rotation1D legWalkingRotation = Rotation1D.D0;
        Rotation1D moveDir;

        int state_walking0_jumping1_attacking2_return3 = 0;

        int legsDamaged = 0;

        float rotationSpeed = NormalRotSpeed;
        Time attackTimer;

        int explosionCount = 0;
        Time exposionTimer = 0;

        public SpiderBot(GoArgs args, BlockMap.AbsLevel level)
            : base(args)
        {
            WorldPos = args.startWp;

            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.spiderbot_mid, 
                BodySize, 0, false);
            image.position = WorldPos.PositionV3;

            AbsBound[] collisionBounds = new AbsBound[5];
            AbsBound[]  damageBounds= new AbsBound[4];

            AbsBound bodyBound = AbsBound.CreateBound(Bounds.BoundShape.Cylinder, image.position,
                VectorExt.V3FromXY(BodySize * 0.45f, BodySize * 0.2f), BodySize * 0.2f * Vector3.Up);
            bodyBound.ignoresDamage = true;
            collisionBounds[0] = bodyBound;

            limbs = new SpiderBotLimb[4];

            for (int i = 0; i < limbs.Length; ++i)
            {
                limbs[i] = new SpiderBotLimb(new Rotation1D(i * MathHelper.PiOver2), i, this, collisionBounds, damageBounds);
            }

            CollisionAndDefaultBound = new ObjectBound(collisionBounds);
            CollisionAndDefaultBound.setBoundIndexes();
            DamageBound = new ObjectBound(damageBounds);
            DamageBound.DebugBoundColor(Color.Red);

            moveDir = Rotation1D.Random();

            physics.Gravity = 0;

            goblin = new SpiderbotGoblin();
            AddChildObject(goblin);
            AddChildObject(new SpiderbotBaby());

            if (args.LocalMember)
            {
                //var lvl = LfRef.levels.GetSubLevelUnsafe(args.startWp).WorldLevel;
                new Director.BossManager(this, level, Players.BabyLocation.Spider);
            }
        }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            int legIndex = damage.recievingBoundIndex - 1;
            limbs[legIndex].takeDamage();
            goblin.model.Frame = 1;

            legsDamaged = 0;
            foreach (var limb in limbs)
            {
                if (!limb.alive)
                {
                    legsDamaged++;
                }
            }

            if (legsDamaged < 4)
            {
                state_walking0_jumping1_attacking2_return3 = 1;

                Velocity.SetZero();

                physics.Gravity = AbsPhysics.StandardGravity * 0.2f;
                physics.Jump(1.5f, image);
            }
        }


        const float AttackY = 2f;
        const float WalkY = 8.5f;

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);


            switch (state_walking0_jumping1_attacking2_return3)
            {
                case 0:
                    if (aiStateTimer.CountDown())
                    {
                        aiStateTimer.MilliSeconds = Ref.rnd.Int(1000, 1200);
                        moveDir.Add(MathHelper.Pi + Ref.rnd.Plus_MinusF(MathHelper.PiOver2));
                    }

                    Velocity.Set(moveDir, WalkingSpeed);
                    Velocity.Update(Ref.DeltaTimeMs, image);

                    if (Ref.TimePassed16ms)
                    {
                        float yDiff = groundY + WalkY - image.position.Y;
                        image.position.Y += yDiff * 0.2f;
                    }

                    legWalkingRotation.Add(8f * Ref.DeltaTimeSec);


                    if (legsDamaged >= 4) 
                    {//dying state
                        if (exposionTimer.CountDown())
                        {
                            explosionCount++;
                            exposionTimer.Seconds = Ref.rnd.Float(0.2f, 0.3f);

                            Vector3 pos = CollisionAndDefaultBound.MainBound.center + Ref.rnd.Vector3_Sq(CollisionAndDefaultBound.MainBound.halfSize * 2f);
                            float radius = 3f;
                            Engine.ParticleHandler.AddExpandingParticleArea(ParticleSystemType.ExplosionFire, pos, radius * 0.2f, 10, 2.6f);
                            Engine.ParticleHandler.AddParticleArea(ParticleSystemType.Smoke, pos, radius * 0.3f, 8);

                            Music.SoundManager.PlaySound(LoadedSound.SmallExplosion, pos);

                            goblin.model.Frame = goblin.model.Frame == 0 ? 1 : 0;

                            if (localMember && explosionCount >= 10)
                            {
                                IsKilled = true;
                                DeleteMe();

                                BlockSplatter();
                                BlockSplatter();
                                BlockSplatter();

                            }
                        }
                    }
                    break;

                case 1:
                    {
                        float goalY = groundY + AttackY;

                        Velocity.Update(Ref.DeltaTimeMs, image);

                        if (Velocity.Y <= 0)
                        {
                            //fäll ut ben
                            foreach (var limb in limbs)
                            {
                                limb.ExtrudeLeg(true);
                            }
                        }

                        rotationSpeed = Bound.Max(rotationSpeed + 10f * Ref.DeltaTimeSec, AttackRotSpeed);

                        if (image.position.Y <= goalY)
                        {
                            //Start rotation attack
                            image.position.Y = goalY;
                            state_walking0_jumping1_attacking2_return3++;
                            attackTimer = new Time(4f + legsDamaged * 1.5f, TimeUnit.Seconds);
                            goblin.model.Frame = 0;
                        }
                    }
                    break;

                case 2:
                    if (aiStateTimer.CountDown())
                    {
                        aiStateTimer.MilliSeconds = Ref.rnd.Int(1000, 1200);
                        moveDir.Add(MathHelper.Pi + Ref.rnd.Plus_MinusF(MathHelper.PiOver2));
                    }

                    Velocity.Set(moveDir, AttackSpeed);
                    Velocity.Update(Ref.DeltaTimeMs, image);

                    image.position.Y = groundY + AttackY;

                    if (attackTimer.CountDown())
                    {
                        //Jump up on legs
                        Velocity.SetZero();
                        physics.Jump(1.5f, image);
                        state_walking0_jumping1_attacking2_return3++;
                    }
                    break;
                case 3:
                    {
                        float goalY = groundY + WalkY;

                        Velocity.Update(Ref.DeltaTimeMs, image);

                        if (Velocity.Y <= 0)
                        {
                            //fäll ut ben
                            foreach (var limb in limbs)
                            {
                                limb.ExtrudeLeg(false);
                            }

                            if (image.position.Y <= goalY)
                            {
                                //Start rotation attack
                                image.position.Y = goalY;
                                Velocity.SetZero();
                                physics.Gravity = 0;
                                state_walking0_jumping1_attacking2_return3 = 0;
                            }
                        }

                        rotationSpeed = Bound.Min(rotationSpeed - 20f * Ref.DeltaTimeSec, NormalRotSpeed);
                    }
                    break;
            }

            

            
            CollisionAndDefaultBound.MainBound.updatePosition(image.position, rotation.Radians);

            rotation.Add(rotationSpeed * Ref.DeltaTimeSec);
            setImageDirFromRotation();
            

            foreach (var limb in limbs)
            {
                limb.updateWalking(legWalkingRotation);
            }

            
            CollisionAndDefaultBound.updateVisualBounds();
            DamageBound.updateVisualBounds();

            UpdateAllChildObjects();
        }

        public override void HandleColl3D(BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            //base.HandleColl3D(collData, ObjCollision);
        }

        public override void AsynchGOUpdate(UpdateArgs args)
        {
            base.AsynchGOUpdate(args);

            groundY = WorldPos.GetHeightMapHeight();

            SolidBodyCheck(args.allMembersCounter);
        }

         

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.gray_85,
            Data.MaterialType.darker_green,
            Data.MaterialType.darker_red_orange);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            foreach (var limb in limbs)
            {
                limb.DeleteMe();
            }
        }

        protected override bool givesContactDamage
        {
            get
            {
                return true;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SpiderBot; }
        }
    }

    class SpiderBotLimb : CharacterLimb
    {
        SpiderBot parent;
        Rotation1D dir;
        Vector3 moveLeg;

        AbsBound collisionBound, damageBound;

        int frame = 0;

        Timer.Basic flashLight = new Timer.Basic(600, true);
        Time smokeTimer = new Time(2, TimeUnit.Seconds);

        public bool alive = true;

        bool exdrudedState = false;

        
        public SpiderBotLimb(Rotation1D dir, int index, SpiderBot parent, AbsBound[] collisionBounds, AbsBound[]  damageBounds)
        {
            this.dir = dir;
            this.parent = parent;
            model = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.spiderbot_leg,
                SpiderBot.BodySize * 1.4f, 0, true);

            RelRotation.X = dir.Radians +MathHelper.Pi;
            relpos = VectorExt.V2toV3XZ(dir.Direction(SpiderBot.BodySize * 0.74f), -SpiderBot.BodySize * 0.25f);

            switch (index)
            {
                case 0:
                    moveLeg.X = 1;
                    break;
                case 1:
                    moveLeg.Z = 1;
                    break;
                case 2:
                    moveLeg.X = -1;
                    break;
                case 3:
                    moveLeg.Z = -1;
                    break;
            }

            collisionBound = AbsBound.CreateBound(BoundShape.Cylinder, Vector3.Zero, VectorExt.V3FromXY(1f, 1.1f), new Vector3(0, 0, 1f));

            damageBound = AbsBound.CreateBound(BoundShape.Cylinder, Vector3.Zero, VectorExt.V3FromXY(0.8f, 1.4f), new Vector3(0, -4.5f, 0));

            collisionBounds[index + 1] = collisionBound;
            damageBounds[index] = damageBound;
        }


        public void updateWalking(Rotation1D legWalkingRotation)
        {
            if (exdrudedState)
            {
                Position = relpos * 0.3f;

                Position.Y = 3f;
            }
            else
            {
                legWalkingRotation.Add(dir);

                Vector2 move = legWalkingRotation.Direction(1.6f) * new Vector2(0.8f, 1f);
                Position.X = move.X * moveLeg.X;
                Position.Z = move.X * moveLeg.Z;
                Position.Y = move.Y;                
            }
            Update(parent.image);

            collisionBound.updatePosition(model.Rotation.TranslateAlongAxis(collisionBound.offset, model.position), 0f);
            damageBound.updatePosition(model.Rotation.TranslateAlongAxis(damageBound.offset, model.position), 0);

            if (exdrudedState)
            {
                if (Ref.TimePassed16ms)
                {
                    Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.BulletTrace, damageBound.center, 0.5f, 4);
                }
            }

            if (alive)
            {
                if (flashLight.Update())
                {
                    frame = frame == 0 ? 1 : 0;
                }

                model.Frame = frame;
            }
            else
            {
                if (!smokeTimer.TimeOut)
                {
                    smokeTimer.CountDown();
                    if (Ref.TimePassed16ms)
                    {
                        Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.Smoke, collisionBound.center, 0.5f, 4);
                    }
                }
            }
        }

        public void ExtrudeLeg(bool extrude)
        {
            if (extrude != exdrudedState)
            {
                exdrudedState = extrude;
                if (extrude)
                {
                    RelRotation.Y = -1.2f;
                }
                else
                {
                    RelRotation.Y = 0f;
                }
            }
        }

        public void takeDamage()
        {
            alive = false;
            model.Frame = 2;
            collisionBound.ignoresDamage = true;

            for (int i = 0; i < 8; i++)
            {
                new Effects.BouncingBlock2(collisionBound.center, MaterialType.light_yellow, 0.4f);
            }

            Music.SoundManager.PlaySound(LoadedSound.SmallExplosion, collisionBound.center);
        }
    }

    class SpiderbotGoblin : VikingEngine.LootFest.GO.AbsChildModel
    {
        public SpiderbotGoblin()
        {
            model = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.spiderbot_goblin,
               2f, 0, false);
            posOffset = new Vector3(0, 1.6f, 1f);


        }
    }

    class SpiderbotBaby : VikingEngine.LootFest.GO.AbsChildModel
    {
        public SpiderbotBaby()
        {
            model = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.baby,
               2.2f, 0, false);
            posOffset = new Vector3(0, 2.0f, -2.3f);


        }
    }
}
