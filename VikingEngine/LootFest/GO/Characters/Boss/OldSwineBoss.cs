using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.GO.Characters.Monsters
{
    //fixa loot
    class OldSwineBoss : Hog3
    {
        /* Constants */
        const float SwineBossWalkingSpeed = HogWalkingSpeed * 0.6f;
        const float SwineBossRunningSpeed = HogRunningSpeed * 0.8f;
        const float SwineBossRushSpeed = HogRunningSpeed * 4f;
        const int BabyCount = 6;

        /* Static readonlies */
        static readonly IntervalF ScaleRange = new IntervalF(8f, 8f);
        
        /* Properties */
        public override GameObjectType Type { get { return GameObjectType.OldSwine; } }
        public override CardType CardCaptureType { get { return CardType.OldHog; } }
        public override float LightSourceRadius { get { return base.LightSourceRadius; } }
        public override Vector3 LightSourcePosition { get { return base.LightSourcePosition; } }
       

        //protected override IntervalF scaleRange { get { return ScaleRange; } }
       // protected override VoxelModelName imageName { get { return VoxelModelName.old_swine; } }
        protected override float walkingSpeed { get { return SwineBossWalkingSpeed; } }
        protected override float rushSpeed { get { return SwineBossRunningSpeed; } }
        //protected override WeaponAttack.DamageData contactDamage { get { return CollisionDamage; } }

        /* Fields */
        protected Time clinkSoundTime = new Time();

        bool ragedMode = false;
        Graphics.AbsVoxelObj redModel;
        Graphics.VoxelModel masterImageStored;

        /* Constructors */
        public OldSwineBoss(GoArgs args, BlockMap.AbsLevel level)
            : base(args, VoxelModelName.old_swine, ScaleRange)
        {
            Health = LfLib.BossEnemyHealth;
            //this.subLevel = subLevel;

            redModel = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.old_swine_red, image.Scale1D, 0, false, false);
            animSettings = new Graphics.AnimationsSettings(5, 0.8f, 1);

            if (args.LocalMember)
            {
                levelCollider.SetLockedToArea();
                var manager = new Director.BossManager(this, level, Players.BabyLocation.Hog);

                Rotation1D dir = Rotation1D.Random();
                for (int i = 0; i < BabyCount; ++i)
                {
                    Vector3 pos = image.position + VectorExt.V2toV3XZ(dir.Direction(5));
                    var minion = new HogBaby(new GoArgs(pos, 0));
                    dir.Add(MathHelper.TwoPi / BabyCount);

                    manager.addBossObject(minion, true);
                }

                NetworkShareObject();
            }
        }

        /* Family methods */
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            clinkSoundTime.CountDown();
            //noseParticles();
            if (localMember)
            {
                //Debug.Log("aiStateTimer " + aiStateTimer.MilliSeconds.ToString());
                aiStateTimer.CountDown();


                if (ragedMode)
                {
                    if (aiStateTimer.MilliSeconds <= 0)
                    {
                        if (aiState != AiState.Rush)
                        { //Ska stå och fnysa innan rus attack

                            aiState = AiState.Rush;
                            //Velocity.Set(rotation, SwineBossRushSpeed);

                            aiStateTimer.Seconds = 6;

                            if (Health < 1f)
                            {
                                aiStateTimer.MilliSeconds *= 1.8f;
                            }
                            else if (Health < 2f)
                            {
                                aiStateTimer.MilliSeconds *= 1.4f;
                            }
                        }
                        else
                        { //Rusa runt i en tid
                            startRageMode(false);
                            NetworkWriteObjectState(AiState.EndRush);
                        }
                    }

                    if (aiState == AiState.Waiting)
                    {
                        noseParticles();
                    }
                    else
                    {
                        for (int i = 0; i < 3; ++i)
                        {
                            Engine.ParticleHandler.AddParticles(VikingEngine.Graphics.ParticleSystemType.RunningSmoke, 
                                new VikingEngine.Graphics.ParticleInitData(Ref.rnd.Vector3_Sq(image.position, 0.6f), Vector3.Zero));
                        }
                    }
                }
                else
                {

                    if (aiStateTimer.MilliSeconds <= 0)
                    {
                        //change state
                        if (aiState == AiState.Waiting)
                        {

                            float moveSpeed = walkingSpeed;
                            target = getClosestCharacter(20, args.allMembersCounter, WeaponAttack.WeaponUserType.Enemy);
                            rotation.Radians = AngleDirToObject(target);
                            aiStateTimer.MilliSeconds = 3000;
                            if (target != null)
                            {//attack
                                aiState = AiState.Attacking;
                                aiStateTimer.MilliSeconds += 2000;
                            }
                            else //walk around
                            {
                                aiState = AiState.Walking;
                                rotation = Rotation1D.Random();
                            }
                            //Velocity.Set(rotation, moveSpeed);

                        }
                        else
                        {
                            aiState = AiState.Waiting;
                            //Velocity.SetZeroPlaneSpeed();
                            aiStateTimer.MilliSeconds = 900 + Ref.rnd.Int(600);
                        }

                        setImageDirFromRotation();
                    }
                    else
                    {
                        if (aiState == AiState.Attacking)
                        {
                            rotation.Radians = AngleDirToObject(target);
                            Velocity.Set(rotation, walkingSpeed);
                            setImageDirFromRotation();
                        }
                    }
                }
            }

        }

        protected override void updateAiMovement()
        {
            if (ragedMode)
            {
                if (aiState == AiState.Rush)
                {
                    aiPhys.MovUpdate_MoveForward(rotation, SwineBossRushSpeed);
                }
                else
                {
                    aiPhys.MovUpdate_StandStill();
                }
            }
            else
            {
                base.updateAiMovement();
            }
        }


        public override void HandleTerrainColl3D(TerrainColl collSata, Vector3 oldPos)
        {
            if (ragedMode)
            {
                if (aiState != AiState.Waiting)
                {
                    rotation.Add(MathHelper.PiOver2 + Ref.rnd.Plus_MinusF(0.5f));
                    Velocity.Set(rotation, SwineBossRushSpeed);
                    setImageDirFromRotation();
                }
            }
            else
            {
                base.HandleTerrainColl3D(collSata, oldPos);
            }
        }

        public override void HandleObsticle(bool wallNotPit, AbsUpdateObj ObjCollision)
        {
            if (ragedMode)
            {
                if (aiState != AiState.Waiting)
                {
                    rotation.Add(MathHelper.PiOver2 + Ref.rnd.Plus_MinusF(0.5f));
                    Velocity.Set(rotation, SwineBossRushSpeed);
                    setImageDirFromRotation();
                }
            }
            base.HandleObsticle(wallNotPit, ObjCollision);
        }

        //public override void AsynchGOUpdate(GO.UpdateArgs args)
        //{
        //    //basicAIupdate(args);
        //    UpdateWorldPos();
        //    if (CollisionAndDefaultBound != null)
        //    {
        //        SolidBodyCheck(args.localMembersCounter);
        //    }
        //    if (boundary != null)
        //    {
        //        boundary.update(this);
        //    }
        //}

        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            if (ragedMode)
            {
                if (aiState == AiState.Rush && clinkSoundTime.TimeOut)
                {
                    Music.SoundManager.WeaponClink(image.position);
                    clinkSoundTime.MilliSeconds = 600;
                }
                return false;
            }
            else
            {
                return base.willReceiveDamage(damage);
            }
        }
        protected override void TakeDamageAction()
        {
            base.TakeDamageAction();

            if (localMember)
            {
                startRageMode(true);
                NetworkWriteObjectState(AiState.Rush);
            }
            //smoke out of the nose
        }

        void startRageMode(bool start)
        {
            ragedMode = start;

            if (start)
            {
                aiState = AiState.Waiting;
                Velocity.SetZeroPlaneSpeed();
                aiStateTimer.MilliSeconds = 1200; //Tiden han frustar innan rush
                masterImageStored = image.GetMaster();
                image.SetMaster(redModel.GetMaster());
            }
            else
            {
                aiState = AiState.Waiting;
                aiStateTimer.MilliSeconds = 1f;

                image.SetMaster(masterImageStored);
            }
        }

        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        {
            base.networkReadObjectState(state, r);

            if (state == AiState.Rush)
            { startRageMode(true); }
            else if (state == AiState.EndRush)
            { startRageMode(false); }
        }

        /* Novelty methods */
        void noseParticles()
        {
            const float ParticleSpeed = 6f;
            
            Vector3 pos = image.position;
            Vector2 forwardVector = rotation.Direction(1f);

            pos += VectorExt.V2toV3XZ(forwardVector * image.Scale1D * 10f);
            pos.Y += image.Scale1D * 8f;

            Vector3 left = VectorExt.V2toV3XZ(VectorExt.RotateVector90DegreeLeft(forwardVector) * ParticleSpeed);
            Vector3 right = VectorExt.V2toV3XZ(VectorExt.RotateVector90DegreeRight(forwardVector) * ParticleSpeed);

            for (int i = 0; i < 2; ++i)
            {
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.RunningSmoke,
                    new Graphics.ParticleInitData(Ref.rnd.Vector3_Sq(pos, 0.3f), left));
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.RunningSmoke,
                    new Graphics.ParticleInitData(Ref.rnd.Vector3_Sq(pos, 0.3f), right));

            }
        }

        public static readonly Effects.BouncingBlockColors OldHogDamageColors = new Effects.BouncingBlockColors(
            Data.MaterialType.gray_65,
            Data.MaterialType.gray_25,
            Data.MaterialType.darker_red_orange);
        public override Effects.BouncingBlockColors DamageColors { get { return OldHogDamageColors; } }
    }
}
