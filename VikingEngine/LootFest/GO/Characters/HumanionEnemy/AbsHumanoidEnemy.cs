using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.GO.Characters.AI;

namespace VikingEngine.LootFest.GO.Characters
{
    /// <summary>
    /// Lootfest 3
    /// </summary>
    abstract class AbsHumanoidEnemy : AbsMonster3
    {
        protected bool tryKeepRangedDistance = false;
        
        protected HumanoidEnemyAgressivity aggresivity = HumanoidEnemyAgressivity.Agressive_3;

        protected bool hasHandWeapon = true, hasRangedWeapon = false, hasRangedWeaponAsRider = false;
        AI.PatrolRoute patrolRoute = null;
        
        protected float distanceToTarget;

        float goalRotation;
        

        protected float preAttackTime = 500;
        protected float preRangeAttackTime = 600;
        
       
        protected float shieldWalkDist = 5;
        float braveryFromAllied = 0;
        protected float maxDistanceFromStart = 0;

        protected Gadgets.HumanoidEnemyHandWeapon handWeapon;
        protected HumanoidEnemyShield shield = null;
        protected GO.AbsChildModel preRangedAttackWeaponEffect = null;
        

        public AbsHumanoidEnemy(GoArgs args)
            : base(args)
        {
            Health = 1;
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (localMember)
            {
                bool bUpdateStateTime = true;

                if (this.Type == GameObjectType.TrollBoss)
                {
                    lib.DoNothing();
                }

                if (aiState == AiState.IsStunned)
                {
                    aiPhys.MovUpdate_StandStill();
                   // updateStateTimer();
                    goto endUpdate;
                }
                else if (attentionStatus == HumanoidEnemyAttentionStatus.Sleeping_0)
                {
                    aiPhys.MovUpdate_StandStill();
                    bUpdateStateTime = false;
                    goto endUpdate;
                }
                else if (aiState == AiState.WaitingForRider)
                {
                    aiPhys.MovUpdate_StandStill();
                    bUpdateStateTime = false;
                    goto endUpdate;
                }
                else if (aiState == AiState.MoveBackHome)
                {
                    if (!aiPhys.MovUpdate_MoveTowards(startWp.PositionV3, 2, walkingSpeed))
                    {
                        aiStateTimer = 0;
                    }
                   // updateStateTimer();
                    goto endUpdate;
                }
                else if (aiState == AiState.WalkTowardsMount)
                {
                    if (assignedRiderOrMount.Alive)
                    {
                        const float MountDist = 2.4f;

                        if (aiPhys.MovUpdate_MoveTowards(assignedRiderOrMount, MountDist, walkingSpeed) == false &&
                               planeDistanceToObject(assignedRiderOrMount) <= MountDist)
                        {
                            //close enough
                            MountAnimal(assignedRiderOrMount);
                        }
                    }
                    else
                    {
                        aiState = AiState.Waiting;
                        aiStateTimer.MilliSeconds = 1;
                    }
                    goto endUpdate;
                }

                if (isMounted)
                {
                    if (MountType == GO.MountType.Rider)
                    {
                        if (!assignedRiderOrMount.Alive)
                        {
                            onLostMount();
                        }
                        else
                        {
                            updateRiding();
                        }
                        goto endUpdate;
                    }
                    else
                    { //is mount
                        assignedRiderOrMount.UpdateRiderPos(this.mountSaddlePos());
                    }
                }

                if (aiState == AiState.LockedInAttack)
                {
                    updateLockedInAttackState();
                    bUpdateStateTime = false;
                }
                else if (attentionStatus == HumanoidEnemyAttentionStatus.LazyPatrol_3a || attentionStatus == HumanoidEnemyAttentionStatus.ActivePatrol_3b)
                {
                    updatePatrol();
                    bUpdateStateTime = false;
                }
                else if (attentionStatus == HumanoidEnemyAttentionStatus.FoundHero_5)
                {
                    if (aiState == AiState.InShock || aiState == AiState.Attacking)
                    {
                        //updateStateTimer();
                        aiPhys.MovUpdate_StandStill();
                    }
                    else if (aiState == AiState.PreAttack || aiState == AiState.PreRangedAttack)
                    {
                        bUpdateStateTime = false;
                        updatePreAttackTimeout();
                        aiPhys.MovUpdate_RotateTowards(target, 0.1f);
                    }
                    else if (target == null)
                    {
                        aiPhys.MovUpdate_StandStill();
                        attentionStatus = HumanoidEnemyAttentionStatus.WalkingAndSearching_4;
                    }
                    else
                    {
                        distanceToTarget = distanceToObject(target);

                        braveryCheck();

                        if (aiState == AiState.Flee)//aggresivity < HumanoidEnemyAgressivity.Agressive_3 && heroIsLookingAtMe())
                        {
                            aiPhys.MovUpdate_FleeFrom(target, walkingSpeed, 0);
                            //updateStateTimer();
                        }
                        else if (aiState == AiState.LookAtTarget)
                        {
                            aiPhys.MovUpdate_RotateTowards(target, 0.1f);
                            //updateStateTimer();
                        }
                        else
                        {
                            if (checkCCAttack())
                            {
                                aiPhys.MovUpdate_RotateTowards(target, 0.1f);
                            }
                            else if (checkProjectileRange())
                            {
                                aiPhys.MovUpdate_RotateTowards(target, 0.1f);
                            }
                            else
                            {
                                aiPhys.MovUpdate_MoveTowards(target, shieldWalkDist * 0.7f, walkingSpeed);
                            }

                            
                        }
                    }
                }
                else
                {
                    //Random walking around

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

            endUpdate:
                if (localMember)
                {
                    activeCheckUpdate();
                    immortalityTime.CountDown();
                }
                if (bUpdateStateTime)
                {
                    updateStateTimer();
                }

                updateAnimation();
                UpdateBound();


                UpdateAllChildObjects();
                if (hasRangedWeapon)
                { nextRangedAttackTimer.CountDown(); }
            }
            else
            { //REMOTE UPDATE
                base.Time_Update(args);

                if (aiStateTimer.CountDown())
                {
                    //if (aiState == AiState.Client_PreAttack || aiState == AiState.Client_PreRangedAttack)
                    //{
                        aiState = AiState.Client_Normal;
                        clearState();
                    //}
                }
            }
        }

        bool willTurnBackToStart()
        {
            if (maxDistanceFromStart > 0)
            {
                return VectorExt.SideLength_XZ(startWp.WorldGrindex, WorldPos.WorldGrindex) > maxDistanceFromStart;
            }
            return false;
        }

        private void updatePreAttackTimeout()
        {
            if (aiStateTimer.CountDown())
            {
                if (target != null && target.Alive)
                {

                    if (aiState == AiState.PreAttack)
                        createCCAttack(true);
                    else
                        createRangedAttack();
                }
                else
                {
                    target = null;
                    clearState();
                    aiState = AiState.Waiting;
                }
            }
        }

        virtual protected void updateRiding()
        {
            rotation = AssignedRiderOrMount.Rotation;
            if (hasRangedWeaponAsRider)
            {
                switch (aiState)
                {
                    case AiState.PreRangedAttack:
                        updatePreAttackTimeout();
                        break;
                    default:
                        checkProjectileRange();
                        break;
                }
                
            }
        }

        protected override void activeCheckUpdate()
        {
            if (managedGameObject)
            {
                checkSleepingState();
            }
            else
            {
                if (checkOutsideUpdateArea_ActiveChunk())
                {
                    DeleteMe();
                    return;
                }
            }
        }

        private bool checkCCAttack()
        {
            if (nextCCAttackTimer.TimeOut && distanceToObject(target) <= shieldWalkDist)
            {
                aiState = AiState.PreAttack;
                NetworkWriteObjectState(aiState);
                handWeapon.PreAttack(this);
                aiStateTimer.MilliSeconds = preAttackTime;
                return true;
            }
            return false;
        }

        
        bool checkProjectileRange()
        {
            if (hasRangedWeapon && nextRangedAttackTimer.TimeOut && projectileRange.IsWithinRange(distanceToObject(target)))
            {
                aiState = AiState.PreRangedAttack;
                NetworkWriteObjectState(aiState);
                aiStateTimer.MilliSeconds = preRangeAttackTime;
                createPreRangedAttackEffect();
                return true;
            }
            return false;
        }

        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        {
            if (state == AiState.PreAttack)
            {
                handWeapon.PreAttack(this);
                aiStateTimer.MilliSeconds = preAttackTime * 1.5f;

                aiState = AiState.Client_PreAttack;
            }
            else if (state == AiState.PreRangedAttack)
            {
                aiStateTimer.MilliSeconds = preRangeAttackTime * 1.2f;
                createPreRangedAttackEffect();

                aiState = AiState.Client_PreRangedAttack;
            }
            else if (state == AiState.Attacking)
            {
                createCCAttack(false);
            }
        }

        virtual protected void createPreRangedAttackEffect() { }

        static readonly IntervalF FleeTimeRange = new IntervalF(300, 400);
        void braveryCheck()
        {
            if (aiState != AiState.Flee)
            {
                if (tryKeepRangedDistance && !nextRangedAttackTimer.TimeOut)
                {
                    if (Math.Abs(distanceToTarget - projectileRange.Center) < 3)
                    {
                        aiState = AiState.LookAtTarget;
                        aiStateTimer.MilliSeconds = Ref.rnd.Float(800, 1200);
                        return;
                    }
                    else if (distanceToTarget < projectileRange.Center)
                    {
                        aiState = AiState.Flee;
                        aiStateTimer.MilliSeconds = FleeTimeRange.GetRandom();
                        return;
                    }
                }


                const float ToCloseToFlee = 3f;

                if (target.LookingTowardObject(this, MathHelper.PiOver4) &&
                    distanceToTarget > ToCloseToFlee)//inte vända när för nära
                {
                    float braveryValue = braveryFromAllied + distanceToTarget * 0.3f;
                    switch (aggresivity)
                    {
                        case HumanoidEnemyAgressivity.ChickenShit_0:
                            braveryValue -= 10;
                            break;
                        case HumanoidEnemyAgressivity.Dodgy_1:
                            braveryValue -= 5;
                            break;
                        case HumanoidEnemyAgressivity.Careful_2:
                            braveryValue -= 3;
                            break;
                    }

                    if (braveryValue < 0)
                    {
                        aiState = AiState.Flee;
                        aiStateTimer.MilliSeconds = FleeTimeRange.GetRandom();
                    }
                }
            }
        }


        virtual protected void updateLockedInAttackState()
        {  }

        protected void updateStateTimer()
        {
            if (aiStateTimer.CountDown())
            {
                onStateTimeout();
            }
        }
        virtual protected void onStateTimeout()
        {
            if (aiState == AiState.InShock)
            {
                if (searchForAvailableMount())
                {
                    return;
                }
            }


            aiState = AiState.Waiting;

            if (willTurnBackToStart())
            {
                aiState = AiState.MoveBackHome;
                aiStateTimer.Seconds = Ref.rnd.Float(2f, 10f);
            }
        }

        protected float LockedAfterAttackTime = 500;
        virtual protected void createCCAttack(bool local)
        {

            float attTime = handWeapon.Attack();

            aiStateTimer.MilliSeconds = attTime + LockedAfterAttackTime;

            if (local)
            {
                nextCCAttackTimer = attackRate;
                aiState = AiState.Attacking;

                NetworkWriteObjectState(aiState);
            }
            else
            {
                aiState = AiState.Client_Attack;
            }
        }

        void removePreRangeAttack()
        {
            if (preRangedAttackWeaponEffect != null)
            {
                preRangedAttackWeaponEffect.DeleteMe();
                preRangedAttackWeaponEffect = null;
            }
        }

        virtual protected void createRangedAttack()
        {
            removePreRangeAttack();

            aiStateTimer.Seconds = 0.5f;
            nextRangedAttackTimer = projectileRate;
            aiState = AiState.Attacking;
        }

        void swordAttackEvent()
        {
            image.Frame = weaponAttackFrame;
        }

        virtual protected void onLostMount()
        {
            aiState = AiState.Waiting;
            isMounted = false;
            assignedRiderOrMount = null;
            immortalityTime.MilliSeconds = 500;
        }
       

        override protected float preAttackScale { get { return 0.25f; } }
        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return new Vector3(0, image.scale.Y * 20, 0);
            }
        }

        

        private void updatePatrol()
        {
            if (aiState == AiState.RotatingTowardsGoal)
            {
                if (rotateTowardsGoalDir(goalRotation, 0.006f, 0f))
                {
                    
                    //aiPhys.MovUpdate_MoveTowards(patrolRoute.WalkingTowards(), 0.4f, casualWalkSpeed);
                    aiState = AiState.Walking;
                }

                aiPhys.MovUpdate_StandStill();
                setImageDirFromRotation();
            }
            else if (aiState == AiState.StopBeforeRotating || aiState == AiState.Waiting)
            {
                if (aiStateTimer.CountDown())
                {
                    aiState = AiState.RotatingTowardsGoal;
                    Velocity.SetZeroPlaneSpeed();
                    goalRotation = AngleDirToObject(patrolRoute.WalkingTowards());
                }

                aiPhys.MovUpdate_StandStill();
            }
            else
            {
                aiPhys.MovUpdate_MoveTowards(patrolRoute.WalkingTowards(), 0.2f, casualWalkSpeed);

                float dist = VectorExt.SideLength_XZ(image.position, patrolRoute.WalkingTowards());
                if (dist < 0.5f)
                {
                    patrolRoute.Next();
                    aiState = AiState.StopBeforeRotating;
                    aiStateTimer.Seconds = 0.5f;
                }
            }
        }

        public override void setSpawnArgument(AbsSpawnArgument spawnArg)
        {
            if (spawnArg is PatrolRoute)
            {
                SetPatrolRoute((PatrolRoute)spawnArg);
            }
            else
            {
                base.setSpawnArgument(spawnArg);
            }
        }

        public void SetPatrolRoute(PatrolRoute patrolRoute)
        {
            this.patrolRoute = patrolRoute;
            //refreshPatrolWalk();
            attentionStatus = patrolRoute.activePatrol? HumanoidEnemyAttentionStatus.ActivePatrol_3b : HumanoidEnemyAttentionStatus.LazyPatrol_3a;
        }
        

        

        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            
            UpdateWorldPos();

            if (CollisionAndDefaultBound != null)
            {
                SolidBodyCheck(args.localMembersCounter);
            }

            if (localMember)
            {
                nextCCAttackTimer.CountDown(args.time);
                searchTarget_asynch(args);

                if (isMounted && MountType == GO.MountType.Rider)
                {
                    return;
                }

                if (attentionStatus == HumanoidEnemyAttentionStatus.Sleeping_0 || aiState == AiState.WaitingForRider || aiState == AiState.LockedInAttack)
                {
                    return;
                }
                
                //else if (attentionStatus == HumanoidEnemyAttentionStatus.LazyPatrol_3a || attentionStatus == HumanoidEnemyAttentionStatus.ActivePatrol_3b)
                //{
                //    //if (patrolRoute != null && aiState == AiState.Walking)
                //    //{
                //    //    refreshPatrolWalk();
                //    //}
                //}
                
                aiPhys.AsynchUpdate(args.time);

               
                if (aggresivity < HumanoidEnemyAgressivity.Agressive_3 && 
                    aiState != AiState.WalkTowardsMount)
                {
                    float braveColl = 0;
                    args.allMembersCounter.Reset();
                    while (args.allMembersCounter.Next())
                    {
                        var go = args.allMembersCounter.GetSelection;
                        if (go.WeaponTargetType == WeaponAttack.WeaponUserType.Enemy)
                        {
                            float distance = distanceToObject(go);
                            if (distance <= 16)
                            {
                                braveColl += args.allMembersCounter.GetSelection.GivesBravery;
                            }
                            else if (distance <= 40)
                            {
                                braveColl += args.allMembersCounter.GetSelection.GivesBravery * 0.5f;
                            }
                        }
                    }

                    braveryFromAllied = braveColl;
                }

                outerBoundCheck_Asynch();
            }
        }

        //float attackDistance = 2;
        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            if (isMounted && ObjCollision == assignedRiderOrMount)
                return;

            if (pushable || ObjCollision == null)
            {
                HandleObsticle(true, ObjCollision);
            }
            if (ObjCollision != null)
            {
                if (givesContactDamage && WeaponAttack.WeaponLib.IsFoeTarget(this, ObjCollision, false))
                {
                    //unthread this
                    handleCharacterColl(ObjCollision, collData, false);//damage 0??
                }
                collData.IntersectionDepth *= 0.5f;
                base.HandleColl3D(collData, ObjCollision);
            }

        }

        public override void HandleObsticle(bool wallNotPit, AbsUpdateObj ObjCollision)
        {
            //base.HandleObsticle(wallNotPit, ObjCollision);
            if (aiState == AiState.Walking)
            {
                aiStateTimer.MilliSeconds = 1;
            }
        }
        

        //void refreshPatrolWalk()
        //{
        //    //if (movetowards(patrolRout.WalkingTowards(), 0.6f, casualWalkSpeed))
        //    //{
        //    //    aiState = AiState.Walking;
        //    //}
        //    //else
        //    //{
        //    //    patrolRout.Next();
        //    //    Velocity.SetZeroPlaneSpeed();
        //    //    aiState = AiState.StopBeforeRotating;
        //    //    aiStateTimer.MilliSeconds = 600;
        //    //}
        //}

        

        public override void listenToVirtualSound_asynch(Director.VirtualSoundSphere sound)
        {
            if (attentionStatus < HumanoidEnemyAttentionStatus.FoundHero_5 && sound.inRange(image.position))
            {
                float delay;

                if (attentionStatus == HumanoidEnemyAttentionStatus.Sleeping_0)
                {
                    delay = Ref.rnd.Float(500, 700);
                }
                else if (attentionStatus == HumanoidEnemyAttentionStatus.StandingStill_1)
                {
                    delay = Ref.rnd.Float(200, 300);
                }
                else
                {
                    delay = Ref.rnd.Float(100, 150);
                }
                searchForAvailableMount();

                new Timer.ActionEventTimedTrigger(onFoundHero, delay);
            }
        }


        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            if (shield != null && damage.Special != WeaponAttack.SpecialDamage.IgnoreShield &&
                damage.Special != WeaponAttack.SpecialDamage.CardThrow)
            {
                Rotation1D attackDir = damage.PushDir;
                attackDir.Add(MathHelper.Pi);
                if (Math.Abs(attackDir.AngleDifference(rotation.Radians)) <= MathHelper.PiOver2)
                {
                    shieldHit(damage, true);
                    return;
                }
            }
            base.handleDamage(damage, local);
            if (attentionStatus < HumanoidEnemyAttentionStatus.FoundHero_5 && Alive)
            {
                onFoundHero();
            }
        }

        virtual protected void shieldHit(WeaponAttack.DamageData damage, bool local)
        {
            if (shield != null)
            {
                if (shield.indestructable || damage.Damage < 1f)
                {
                    Music.SoundManager.WeaponClink(image.position);
                }
                else
                {
                    shield.TakeHit();
                    shield = null;
                }
            }

            if (pushable)
            {
                if (localMember)
                {
                    aiPhys.AddPushForce(damage);
                }
                else
                {
                    //todo share push
                }
            }

            if (attentionStatus < HumanoidEnemyAttentionStatus.FoundHero_5)
            {
                onFoundHero();
            }

            if (local)
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.ShieldHit, Network.PacketReliability.Reliable);
                this.ObjOwnerAndId.write(w);
                damage.writeDamage(w);
                w.Write(damage.PushDir.ByteDir);
            }
        }

        public void netReadShieldHit(System.IO.BinaryReader r)
        {
            WeaponAttack.DamageData damage = WeaponAttack.DamageData.NoN;
            damage.ReadDamage(r);
            damage.PushDir.ByteDir = r.ReadByte();

            shieldHit(damage, false);
        }

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            base.DeathEvent(local, damage);
            LfRef.gamestate.GameObjCollection.virtualSounds.Add(new Director.VirtualSoundSphere(Director.VirtualSoundType.DeathPop, image.position, DeathPopSoundRadius));
        }

        

        override protected void clearState()
        {
            removePreRangeAttack();
            handWeapon.Clear();
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            //if (shield != null) shield.DeleteMe();
        }


        override protected bool givesContactDamage
        { get { return false; } }

        protected override bool hasPatrolRoute
        {
            get
            {
                return patrolRoute != null;
            }
        }

        
    }

    enum HumanoidEnemyAgressivity
    {
        ChickenShit_0,
        Dodgy_1,
        Careful_2,
        Agressive_3,
        Berserk_4,
    }
    enum HumanoidEnemyAttentionStatus
    {
        Sleeping_0,
        StandingStill_1,
        SmallMovement_2,
        LazyPatrol_3a,
        ActivePatrol_3b,
        WalkingAndSearching_4,
        FoundHero_5,
    }

    

    
}
