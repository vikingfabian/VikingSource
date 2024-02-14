using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.Effects;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.GO.Characters
{
    abstract class AbsMonster3 : AbsEnemy
    {
        protected const float AlarmSoundRadius = 40;
        protected const float DeathPopSoundRadius = 20;
        protected float targetSearchDistanceIdle = 40, targetSearchDistanceTaunted = 80;

        protected Time attackRate = new Time(2, TimeUnit.Seconds);
        protected Time projectileRate = new Time(6, TimeUnit.Seconds);
        protected IntervalF projectileRange = new IntervalF(4, 32);
        protected Time nextCCAttackTimer = 0;
        protected Time nextRangedAttackTimer = new Time(Ref.rnd.Float(0.2f, 1f), TimeUnit.Seconds);

        protected Effects.SleepingZZZ sleepingEffect = null;
        protected int sleepingFrame1 = 4;
        public HumanoidEnemyAttentionStatus attentionStatus = HumanoidEnemyAttentionStatus.SmallMovement_2;
        protected AiPhysicsLf3 aiPhys;
        protected int preRangeAttackFrame = 9;
        protected int weaponAttackFrame = 3;
        protected int ridingFrame = 1;
        protected int ridingAndAttackingFrame = 2;

        protected int jumpFrame = -1;
        float riderBounce = 0;

        protected bool goBackToIdle = true;
        

        public AbsMonster3(GoArgs args)
            : base(args)
        {
            this.WorldPos = args.startWp;
        }

        virtual protected void createAiPhys()
        {
            if (localMember)
            {
                aiPhys = new AiPhysicsLf3(this, PhysicsType == ObjPhysicsType.FlyingAi);
                physics = aiPhys;
            }
        }

        protected void createImage(VoxelModelName imgType, float scale, Graphics.AnimationsSettings animation)
        {
            createImage(imgType, scale, 1, animation);
        }

        protected void createImage(VoxelModelName imgType, float scale, float modelYadj, Graphics.AnimationsSettings animation)
        {
            animSettings = animation;
            modelScale = scale;
            image = LfRef.modelLoad.AutoLoadModelInstance(imgType, scale, modelYadj, false);
            image.position = WorldPos.PositionV3;
        }

        public override void NetWriteUpdate(System.IO.BinaryWriter w)
        {
            base.NetWriteUpdate(w);
            w.Write((byte)netSharedAiState());
        }
        public override void NetReadUpdate(System.IO.BinaryReader r)
        {
            base.NetReadUpdate(r);
            var prevState = aiState;
            aiState = (AiState)r.ReadByte();

            if (prevState != aiState)
            {
                if (aiState == AiState.Client_Sleeping)
                {
                    if (sleepingEffect == null)
                    { sleepingEffect = new Effects.SleepingZZZ(this); }
                }
                else
                {
                    removeSleepEffect();
                }
            }
        }

        public void alwaysFullAttension()
        {
            attentionStatus = HumanoidEnemyAttentionStatus.WalkingAndSearching_4;
            goBackToIdle = false;
        }

        virtual protected AiState netSharedAiState()
        {
            if (sleepingEffect != null)
            {
                return AiState.Client_Sleeping;
            }
            else if (aiState == AiState.PreAttack)
            {
                return AiState.Client_PreAttack;
            }
            else if (aiState == AiState.PreRangedAttack)
            {
                return AiState.Client_PreRangedAttack;
            }
            else if (aiState == AiState.Attacking)
            {
                return AiState.Client_Attack;
            }
            else
            {
                return AiState.Client_Normal;
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (this is GreatWolf)
            {
                lib.DoNothing();
            }
            base.Time_Update(args);

            if (localMember)
            {
                if (isMounted)
                {
                    if (MountType == GO.MountType.Rider)
                    {
                        if (!assignedRiderOrMount.Alive)
                        {
                            aiState = AiState.Waiting;
                            isMounted = false;
                            assignedRiderOrMount = null;
                        }
                        return;
                    }
                    else
                    { //is mount
                        assignedRiderOrMount.UpdateRiderPos(this.mountSaddlePos());
                    }
                }

                if (aiState == AiState.IsStunned)
                {
                    if (aiPhys.FlyingState)
                    {
                        aiPhys.MovUpdate_FallToGround(physics.Gravity);
                    }
                    else
                    {
                        aiPhys.MovUpdate_StandStill();
                    }

                    if (aiStateTimer.CountDown())
                    {
                        aiState = AiState.Waiting;
                    }
                }
                else if (attentionStatus == HumanoidEnemyAttentionStatus.FoundHero_5 && target != null)
                {
                    updateAiMovement_Attacking();
                    updateAiMovement();
                }
                else
                {
                    updateAiMovement_Idle();
                    updateAiMovement();
                }
            }

            UpdateAllChildObjects();
        }

        public override void Force(Vector3 center, float force)
        {
            if (pushable)
            {
                aiPhys.AddPushForce(VectorExt.SafeNormalizeV2(VectorExt.V3XZtoV2(image.position - center)) * force);
            }
        }

        virtual protected void updateAiMovement() { }
        virtual protected void updateAiMovement_Idle() { }
        virtual protected void updateAiMovement_Attacking() { }

        protected void defaultIdleMovement()
        {
            if (sleepingEffect != null)
            {
                aiPhys.MovUpdate_StandStill();
                return;
            }

            if (aiStateTimer.CountDown())
            {
                if (aiState == AiState.Waiting)
                {
                    aiStateTimer.Seconds = Ref.rnd.Float(2f, 4f);
                    rotation = Rotation1D.Random();
                    aiState = AiState.Walking;
                }
                else
                {
                    aiStateTimer.Seconds = Ref.rnd.Float(1f, 2f);

                    aiState = AiState.Waiting;
                }
            }


            if (aiState == AiState.Walking)
            {
                aiPhys.MovUpdate_MoveForward(rotation, casualWalkSpeed);
            }
            else
            {
                aiPhys.MovUpdate_StandStill();
            }
        }
        protected void defaultAttackMovement()
        {
            aiPhys.MovUpdate_MoveTowards(target, 4f, walkingSpeed);
        }

        public override void AsynchGOUpdate(UpdateArgs args)
        {
            base.AsynchGOUpdate(args);

            if (localMember)
            {
                searchTarget_asynch(args);

                if (aiPhys != null)
                { aiPhys.AsynchUpdate(args.time); }
                
                outerBoundCheck_Asynch();
            }
            if (CollisionAndDefaultBound != null)
            {
                SolidBodyCheck(args.localMembersCounter);
            }

        }

        protected void outerBoundCheck_Asynch()
        {
            //Outer bounds check
            if (boundary != null)
            {
                boundary.update(this);
            }
            else
            {
                //if (subLevel == null)
                //{
                //    subLevel = LfRef.levels.GetSubLevelUnsafe(WorldPos);
                //}
                //else
                //{
                //    subLevel.KeepGOInsideAllBounds(this);
                //}
            }
        }

        

        protected void searchTarget_asynch(GO.UpdateArgs args)
        {
            float searchDist = attentionStatus < HumanoidEnemyAttentionStatus.WalkingAndSearching_4 ? targetSearchDistanceIdle : targetSearchDistanceTaunted;

            target = getClosestCharacter(searchDist, args.allMembersCounter, WeaponAttack.WeaponUserType.Enemy);

            if (target == null)
            {
                if (attentionStatus == HumanoidEnemyAttentionStatus.FoundHero_5)
                {
                    if (goBackToIdle)
                    {
                        if (hasPatrolRoute)
                        {
                            attentionStatus = HumanoidEnemyAttentionStatus.ActivePatrol_3b;
                        }
                        else
                        {
                            attentionStatus = HumanoidEnemyAttentionStatus.SmallMovement_2;
                        }
                    }
                    else
                    {
                        attentionStatus = HumanoidEnemyAttentionStatus.WalkingAndSearching_4;
                    }
                }
            }
            else
            {
                if (attentionStatus < HumanoidEnemyAttentionStatus.FoundHero_5)
                {
                    if (attentionStatus != HumanoidEnemyAttentionStatus.Sleeping_0 && LookingTowardObject(target, MathHelper.PiOver2))
                    {
                        Vector3 diff = target.Position - Position;
                        float len = diff.Length() * 2;
                        Vector3 dir = diff / len;
                        for (int i = 0; i < len; ++i)
                        {
                            Vector3 checkPos = HeadPosition + i * dir;
                            WorldPosition wp = new WorldPosition(checkPos);
                            if (wp.BlockHasMaterial())
                            {
                                // view is obstructed, we can't see the hero
                                target = null;
                                return;
                            }
                        }

                        //searchForAvailableMount();
                        attentionStatus = HumanoidEnemyAttentionStatus.FoundHero_5;
                        new Timer.Action0ArgTrigger(onFoundHero);
                    }
                    else
                    { target = null; }
                }
            }
        }

        

        protected bool foundHeroEffect = false;
        float prevVirtualSoundTime = 0;

        virtual public void onFoundHero()
        {
            if (Alive && !foundHeroEffect && goBackToIdle)
            {
                if (localMember)
                {
                   
                    if (!aiPhys.FlyingState)
                    {
                        physics.Jump(0.3f, image);
                    }
                    aiState = AiState.InShock;
                    aiStateTimer.MilliSeconds = 400;
                    Velocity.SetZeroPlaneSpeed();
                    

                    if (assignedRiderOrMount != null)
                    {
                        if (MountType == GO.MountType.Rider)
                            aiState = AiState.WalkTowardsMount;
                        else
                            aiState = AiState.WaitingForRider;
                    }
                    

                    if (HasNetworkClient)
                    {
                        System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.FoundHeroEffect, Network.PacketReliability.Reliable);
                        ObjOwnerAndId.write(w);
                    }
                }
                foundHeroEffect = true;

                new Effects.EnemyAttention(new Time(600), image, expressionEffectPosOffset, preAttackScale, Effects.EnemyAttentionType.Expression);
            }

            if ((Ref.TotalTimeSec - prevVirtualSoundTime) >= 1f)
            {
                LfRef.gamestate.GameObjCollection.virtualSounds.Add(new Director.VirtualSoundSphere(Director.VirtualSoundType.Alarm,
                        image.position, AlarmSoundRadius));
                prevVirtualSoundTime = Ref.TotalTimeSec;
            }
            removeSleepEffect();
            attentionStatus = HumanoidEnemyAttentionStatus.FoundHero_5;
        }

        void removeSleepEffect()
        {
            if (sleepingEffect != null) { sleepingEffect.DeleteMe(); sleepingEffect = null; }
        }

        public override void stunForce(float power, float takeDamage, bool headStomp, bool local)
        {
            if (canBeStunned)
            {
                if (takeDamage > 0)
                {
                    TakeDamage(new WeaponAttack.DamageData(takeDamage, WeaponAttack.WeaponUserType.NON, NetworkId.Empty), true);
                }
                if (Alive)
                {
                    if (local && IsOrHasClient)
                    {
                        var w = Ref.netSession.BeginWritingPacket(Network.PacketType.StunForce, Network.PacketReliability.Reliable);
                        ObjOwnerAndId.write(w);
                        w.Write((byte)(power * 10));
                        w.Write(headStomp);
                    }

                    //base.stunForce(power, takeDamage, headStomp);
                    clearState();

                    aiStateTimer.Seconds = power * 2;
                    addStunnEffect();
                    if (headStomp)
                    {
                        ChildObjectsCounter count = new ChildObjectsCounter(childObjects);
                        while (count.Next())
                        {
                            if (count.currentChild is Effects.ScaleSpringY)
                            {
                                Effects.ScaleSpringY eff = (Effects.ScaleSpringY)count.currentChild;
                                eff.restart();
                                return;
                            }
                        }
                        this.AddChildObject(new ScaleSpringY(image));
                    }
                }
            }
        }

        public void netReadStunForce(System.IO.BinaryReader r)
        {
            float power = r.ReadByte() / 10f;
            bool headStomp = r.ReadBoolean();

            stunForce(power, 0f, headStomp, false);
        }

        public void GoToSleep()
        {
            attentionStatus = HumanoidEnemyAttentionStatus.Sleeping_0;

            rotation = Rotation1D.Random();
            setImageDirFromRotation();

            if (sleepingEffect == null) sleepingEffect = new Effects.SleepingZZZ(this);

            refreshLightY();
        }



        virtual protected void clearState()
        { }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            clearState();

            if (sleepingEffect != null) { sleepingEffect.DeleteMe(); }
        }

#region MOUNT
        protected bool isMounted = false;
        protected AbsMonster3 assignedRiderOrMount = null;
        public override AbsUpdateObj AssignedRiderOrMount
        {
            get
            {
                return assignedRiderOrMount;
            }
        }

        public void AssignRider(AbsMonster3 rider)
        {
            assignedRiderOrMount = rider;
            isMounted = true;

            removeSleepEffect();
            aiState = AiState.Waiting;
            if (rider.attentionStatus > this.attentionStatus)
            {
                attentionStatus = rider.attentionStatus;
            }
        }
        //public void OnMount(AbsMonster3 rider)
        //{
        //    assignedRiderOrMount = rider;
            
            
            

        //    isMounted = true;

        //    removeSleepEffect();
        //    aiState = AiState.Waiting;
        //    if (rider.attentionStatus > this.attentionStatus)
        //    {
        //        attentionStatus = rider.attentionStatus;
        //    }
        //}

        protected Vector3 calcSaddlePos(float forward, float up, float bounce)
        {
            Vector3 result = image.position;
            Vector2 forwardDir = rotation.Direction(image.Scale1D * forward);
            result.X += forwardDir.X;
            result.Y += image.Scale1D * up;
            result.Z += forwardDir.Y;

            if (bounce > 0 && !physics.PhysicsStatusFalling)
            { 
                riderBounce += Ref.DeltaTimeMs * Velocity.PlaneLength() * 0.8f; 
            }
            result.Y += (float)Math.Cos(riderBounce) * bounce * image.Scale1D;

            return result;
        }
        virtual protected Vector3 mountSaddlePos()
        {
            throw new NotImplementedException();
        }

        public void UpdateRiderPos(Vector3 saddlePos)
        {
            //Mount måste uppdatera sin rider för att inte uppdateringen ska bli fel
            saddlePos.Y -= image.Scale1D * 3f;
            image.position = saddlePos;
            image.Rotation = assignedRiderOrMount.RotationQuarterion;
        }

        protected bool searchForAvailableMount()
        {
            if (MountType == MountType.Rider)
            {
                //Look for nearby mount
                var counter = LfRef.gamestate.GameObjCollection.AllMembersUpdateCounter;
                while (counter.Next())
                {
                    AbsUpdateObj go = counter.GetSelection;
                    if (go.MountType == GO.MountType.Mount)
                    {
                        if (go.AssignedRiderOrMount == null && distanceToObject(go) <= 16)
                        {
                            //if (go is AbsMonster3)
                            //{
                                assignedRiderOrMount = (AbsMonster3)go;
                                aiState = AiState.WalkTowardsMount;
                                aiStateTimer.Seconds = 5f;
                                //MountAnimal((AbsMonster3)go);
                                return true;
                            //}

                        }
                    }
                }
            }
            return false;
        }

        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            if (ObjCollision != null && ObjCollision == assignedRiderOrMount)
            {
                return;
            }
            if (localMember)
            {
                if (ObjCollision != null && attentionStatus < HumanoidEnemyAttentionStatus.FoundHero_5 && ObjCollision is PlayerCharacter.AbsHero)
                {//React to hero touch
                    new Timer.Action0ArgTrigger(onFoundHero);
                }
            }
            base.HandleColl3D(collData, ObjCollision);
        }

        public override void HandleObsticle(bool wallNotPit, AbsUpdateObj ObjCollision)
        {
            aiPhys.hitObsticle();
            base.HandleObsticle(wallNotPit, ObjCollision);
        }


        public void MountAnimal(AbsMonster3 mount)
        {
            mount.AssignRider(this);
            this.assignedRiderOrMount = mount;
            aiState = AiState.Waiting;
            isMounted = true;
            image.Frame = ridingFrame;

        }
#endregion

        //virtual protected Vector3 headEffectPos()
        //{
        //    Vector3 result = image.Position;
        //    result.Y += ExspectedHeight * 0.8f;
        //    return result;
        //}

        public override Vector3 HeadPosition
        {
            get
            {
                Vector3 result = image.position;
                result.Y += ExspectedHeight * 0.8f;
                return result;
            }
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.GroundAi;
            }
        }

        protected const float StandardWalkingSpeed = 0.006f;
        protected const float StandardCasualWalkSpeed = StandardWalkingSpeed * 0.5f;
        protected const float StandardShieldWalkSpeed = StandardCasualWalkSpeed;
        protected const float StandardRushSpeed = 0.01f;

        virtual protected float casualWalkSpeed
        {
            get { return StandardCasualWalkSpeed; }
        }
        virtual protected float shieldWalkSpeed
        {
            get { return StandardShieldWalkSpeed; }
        }
        virtual protected float walkingSpeed
        {
            get { return StandardWalkingSpeed; }
        }
        virtual protected float rushSpeed
        {
            get { return StandardRushSpeed; }
        }

        protected override void addPhysics()
        {
            //do nothing
        }

        Time sleepAnimTimer;
        bool firstSleepFrame = false;
        protected override void updateAnimation()
        {
            if (sleepingEffect != null)
            {
                if (sleepAnimTimer.CountDown())
                {
                    sleepAnimTimer.MilliSeconds = Ref.rnd.Float(1400, 1800);

                    if (firstSleepFrame)
                        image.Frame = sleepingFrame1;
                    else
                        image.Frame = sleepingFrame1 + 1;

                    firstSleepFrame = !firstSleepFrame;
                }
            }
            else if (isMounted && MountType == GO.MountType.Rider)
            {
                if (aiState == AiState.Attacking || aiState == AiState.LockedInAttack || aiState == AiState.Client_Attack)
                {
                    image.Frame = ridingAndAttackingFrame;
                }
                else
                {
                    image.Frame = ridingFrame;
                }
            }
            else if (aiState == AiState.Attacking || aiState == AiState.LockedInAttack || aiState == AiState.Client_Attack)
            {
                image.Frame = weaponAttackFrame;
            }
            else if (aiState == AiState.PreRangedAttack || aiState == AiState.Client_PreRangedAttack)
            {
                image.Frame = preRangeAttackFrame;
            }

            else if (aiState != AiState.Attacking && aiState != AiState.LockedInAttack)
            {
                base.updateAnimation();
            }
        }

        public override void setSpawnArgument(AbsSpawnArgument spawnArg)
        {
            if (spawnArg is SleepingSpawnArg)
            {
                GoToSleep();
            }
            else
            {
                base.setSpawnArgument(spawnArg);
            }
        }

        public override Graphics.LightParticleType LightSourceType
        {
            get
            {
                if (isMounted && MountType == GO.MountType.Rider)
                    return Graphics.LightParticleType.NUM_NON;

                return Graphics.LightParticleType.Shadow;
            }

        }

        public override bool canBeCardCaptured
        {
            get
            {
                return !canBeStunned || aiState == AiState.IsStunned;
            }
        }

        public virtual bool canBeStunned
        {
            get { return true; }
        }
        protected virtual bool hasPatrolRoute
        {
            get
            {
                return false;
            }
        }
    }
}
