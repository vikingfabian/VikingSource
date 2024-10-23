using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject
{

    abstract class AbsSoldierUnit : AbsDetailUnit
    {
        //public const int SoldierAiState_GroupLock = 0;
        // const int SoldierAiState_ColumnQue = 1;
        // const int SoldierAiState_FreeAttack = 2;
        // const int SoldierAiState_Idle = 3;
        // const int SoldierAiState_ReGroup = 4;

        protected static float GoalReachDist_GROUP = DssVar.StandardBoundRadius * 2f;
        protected static float GoalReachDist_WhenColliding = GoalReachDist_GROUP * 3f;
        //const float GoalReachDist_Induvidual = AbsSoldierData.StandardBoundRadius * 0.04f;

        //const float FleeMinDistanceToGoal = AbsSoldierData.StandardBoundRadius * 8;
        //const float FleeMinDistanceToEnemy = AbsSoldierData.StandardBoundRadius * 5;

        public Vector3 walkingGoal;
        Vector2 groupOffset;
        float goalDistans = 0;
        public WalkingPathInstance walkPath;
        public bool lockMovement = true;

        int walkStraightUpdates = 0;

        public IntVector2 gridPlacement;

        public SoldierAiState aiState = SoldierAiState.GroupLock;
        public SoldierState2 state2 = SoldierState2.idle;
        float stateTime;
        public int following = -1;

        public int bonusProjectiles = 0;

        public UnitType UnitType;
        float reactionTime;
        //override public AbsSoldierProfile profile()
        //{ 
        //    return DssRef.profile.Get(UnitType);
        //}
        public override AbsDetailUnitProfile Profile()
        {
            return DssRef.profile.Get(UnitType);
        }
        public AbsSoldierProfile SoldierProfile()
        {
            return DssRef.profile.Get(UnitType);
        }

        virtual public void copyDataToUpgradedUnit(AbsSoldierUnit upgradeUnit)
        {
            upgradeUnit.walkingGoal = walkingGoal;
            upgradeUnit.groupOffset = groupOffset;
            upgradeUnit.lockMovement = lockMovement;
            upgradeUnit.gridPlacement = gridPlacement;
            upgradeUnit.aiState = aiState;
            upgradeUnit.parentArrayIndex = parentArrayIndex;
            upgradeUnit.following = following;

            upgradeUnit.parentArrayIndex = parentArrayIndex;
            upgradeUnit.attackTarget = attackTarget;
            upgradeUnit.tilePos = tilePos;
            upgradeUnit.rotation = rotation;
            upgradeUnit.state = state;
            upgradeUnit.position = position;
        }



        //public void write(System.IO.BinaryWriter w)
        //{
        //    w.Write((byte)data.unitType);
        //    WritePosition(w, position);
        //    w.Write(rotation.ByteDir);
        //}

        //public void read(System.IO.BinaryReader r, Players.AbsPlayer player, SoldierGroup group)
        //{

        //}

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)aiState);
            WP.writePosXZ(w, position);
            w.Write(rotation.ByteDir);

        }
        public void readGameState(System.IO.BinaryReader r, int version)
        {
            aiState = (SoldierAiState)r.ReadByte();
            WP.readPosXZ(r, out position, out tilePos);
            rotation.ByteDir = r.ReadByte();
        }

        public void writeNet(System.IO.BinaryWriter w)
        {

        }
        public void readNet(System.IO.BinaryReader r)
        {

        }

        virtual public void InitLocal(Vector3 center, IntVector2 gridPlacement,
            IntVector2 tile, SoldierGroup group)
        {
            this.group = group;
            this.gridPlacement = gridPlacement;
            parentArrayIndex = group.army.faction.pickNextUnitId();

            init(false);
            tilePos = tile;

            bonusProjectiles = soldierData.bonusProjectiles;

            lockMovement = false;

            switch (group.soldierConscript.conscript.training)
            {
                case Conscript.TrainingLevel.Minimal:
                    reactionTime = 300 + Ref.rnd.Float(300);
                    break;
                case Conscript.TrainingLevel.Basic:
                    reactionTime = 200 + Ref.rnd.Float(200);
                    break;
                case Conscript.TrainingLevel.Skillful:
                    reactionTime = 100 + Ref.rnd.Float(100);
                    break;
                case Conscript.TrainingLevel.Professional:
                    reactionTime = 50 + Ref.rnd.Float(50);
                    break;
            }
        }

        public void initUpgrade(SoldierGroup group)
        {
            this.group = group;

            init(true);
        }

        public void setDetailLevel(bool unitDetailView)
        {
            Debug.CrashIfThreaded();
            if (unitDetailView)
            {
                if (model == null)
                {
                    model = initModel();
                    model.update(this);
                }
            }
            else
            {
                model?.DeleteMe();
                model = null;
            }
        }

        public override string TypeName()
        {
            return group.soldierConscript.conscript.TypeName() + " (" + parentArrayIndex.ToString() + ")";
        }
        public override SpriteName TypeIcon()
        {
            return group.TypeIcon();
        }

        override public void netShareUnit()
        {
            //var w = beginWriteAddUnit(player);
            //WritePosition(w, model.position);
        }

        //override public void InitRemote(Players.AbsPlayer player, System.IO.BinaryReader r)
        //{
        //    //base.InitRemote(player, r);

        //    //readId(r);
        //    //Vector3 startPos = ReadPosition(r);
        //    //clientPosition = startPos;

        //    //init(startPos);
        //}

        //abstract protected void initData();

        public static void WritePosition(System.IO.BinaryWriter w, Vector3 position)
        {
            w.Write(position.X);
            w.Write(position.Z);
        }

        public static Vector3 ReadPosition(System.IO.BinaryReader r)
        {
            Vector3 result = Vector3.Zero;
            result.X = r.ReadSingle();
            result.Z = r.ReadSingle();

            return result;
        }

        public static void WriteArea(System.IO.BinaryWriter w, IntVector2 area)
        {
            area += 1;
            area.write(w);
        }
        public static IntVector2 ReadArea(System.IO.BinaryReader r)
        {
            IntVector2 area = IntVector2.FromRead(r);
            area -= 1;
            return area;
        }

        virtual public void init(bool asUpgrade)
        {
            health = soldierData.basehealth;
            radius = Profile().boundRadius;

            if (!asUpgrade)
            {
                refreshGroupOffset();
                updateGroupPosition();
            }
        }

        public void refreshGroupOffset()
        {
            groupOffset.X = gridPlacement.X * SoldierProfile().groupSpacing +
                Ref.rnd.Plus_MinusF(SoldierProfile().groupSpacingRndOffset);

            groupOffset.Y = (gridPlacement.Y + group.halfColDepth) * SoldierProfile().groupSpacing +
                Ref.rnd.Plus_MinusF(SoldierProfile().groupSpacingRndOffset);
        }

        void updateGroupPosition()
        {
            Vector3 prev = position;

            position = groupPosition(group.position, group.rotation.radians);
            position.Y = prev.Y;

            rotation = group.rotation;

            state.walking = prev != position;
        }

        public Vector3 groupPosition(Vector3 groupCenter, float groupRotation)
        {
            //if (debugTagged)
            //{
            //    lib.DoNothing();
            //}
            Vector3 result = position;
            Vector2 rotatedOffset = VectorExt.RotateVector(groupOffset, groupRotation);
            //if (Math.Abs(rotatedOffset.X) > 0.5f)
            //{
            //    lib.DoNothing();
            //}
            result.X = groupCenter.X + rotatedOffset.X;
            result.Z = groupCenter.Z + rotatedOffset.Y;

            return result;
        }

        override public void update(float time, bool fullUpdate)
        {
            if (!lockMovement)
            {
                if (fullUpdate)
                {
                    switch (aiState)
                    {
                        case SoldierAiState.ColumnQue:
                            if (soldierData.mainAttack != AttackType.Melee ||
                               bonusProjectiles > 0)
                            {
                                updateRangeAttackIfAble(time, fullUpdate);
                            }
                            updateFollowQue(time);
                            break;
                        case SoldierAiState.FreeAttack:
                            updateMoveAttackPrio(time, fullUpdate);
                            collisionUpdate();
                            break;
                        case SoldierAiState.Idle:
                            state.idle = true;
                            state.walking = false;
                            break;
                        case SoldierAiState.ReGroup:
                            if (!walkTowards(time, walkingGoal))
                            {
                                rotateToAngle(group.rotation.radians);
                                state.idle = !state.rotating;
                            }
                            collisionUpdate();
                            break;
                        case SoldierAiState.GroupLock:
                            //In wrong state
                            setFreeAttack();
                            break;
                    }

                    updateGroudY(false);
                }
                else
                {
                    switch (aiState)
                    {
                        case SoldierAiState.FreeAttack:
                            updateMoveAttackPrio(time, fullUpdate);
                            break;
                        case SoldierAiState.GroupLock:
                            //In wrong state
                            setFreeAttack();
                            break;
                    }
                }

                model?.update(this);
            }
        }

        public void update_GroupLocked(bool walking)
        {
            if (walking)
            {
                updateGroupPosition();
                updateGroudY(false);
            }
            state.walking = walking;
            state.idle = !walking;
            model?.update(this);
        }

        public void update2(float time)
        {
            if (state2 == SoldierState2.wakeup)
            {
                stateTime -= time;
                if (stateTime < 0)
                {
                    state2 = SoldierState2.waiting;
                }
            }
            else if (state2 != SoldierState2.idle)
            {
                walkingGoal = groupPosition(group.position, group.rotation.radians);
                if (!walkTowards(time, walkingGoal))
                {
                    rotateToAngle(group.rotation.radians);
                }
                if (group.state == GroupState.GoingIdle && state2 == SoldierState2.waiting)
                {
                    state2 = SoldierState2.idle;
                }

                updateGroudY(false);
                model?.update(this);
            }
        }

        public void wakeUp2()
        {
            if (state2 == SoldierState2.idle)
            {
                state2 = SoldierState2.wakeup;

                stateTime = reactionTime;
                
            }
        }

        public void firstUpdate()
        {
            updateGroupPosition();
            updateGroudY(true);

            state.walking = false;
            state.idle = true;
            model?.update(this);
        }

        public void setReGroupState()
        {
            aiState = SoldierAiState.ReGroup;
            state.walkingOrderComplete = false;
            state.idle = false;
            walkingGoal = groupPosition(group.position, group.rotation.radians);
            bonusProjectiles = soldierData.bonusProjectiles;
        }

        public void setBattleNode()
        {
            walkingGoal = groupPosition(group.goalWp, group.rotation.radians);
        }


        public void setAttackState()
        {
            aiState = SoldierAiState.ColumnQue;

            if (following < 0)
            {
                setFreeAttack();
            }
        }

        const float ModelGroundYAdj = 0.02f;
        protected void updateGroudY(bool set)
        {
            if (DssRef.world.unitBounds.IntersectPoint(position.X, position.Z))//position.X > 0 && position.Z>0)
            {
                float y = DssRef.world.SubTileHeight(position) + ModelGroundYAdj;

                if (y < Map.Tile.UnitMinY)
                {
                    y = Map.Tile.UnitMinY;
                }

                if (y != position.Y)
                {
                    if (set)
                    {
                        position.Y = y;
                    }
                    else
                    {
                        float diff = y - position.Y;
                        if (Math.Abs(diff) < 0.01f)
                        {
                            position.Y = y;
                        }
                        else
                        {
                            position.Y += diff * 0.06f;
                        }
                    }
                }
            }
        }

        void updateRangeAttackIfAble(float time, bool fullUpdate)
        {
            state.attacking = false;

            if (IsAttacking)
            {
                state.attacking = true;
                //Attacking
                updateAttack(time);
            }
            else
            {
                var inReach = checkTargetInReach();

                if (inReach == HasTargetInReach.InReach)
                {
                    commitAttack(fullUpdate);
                }
                else if (inReach == HasTargetInReach.MustRotate)
                {
                    state.walking = true;
                    state.rotating = true;
                    rotateTowards(attackTarget, SoldierProfile().rotationSpeed);
                }
            }
        }

        private void updateMoveAttackPrio(float time, bool fullUpdate)
        {
            searchTargetUpdate();

            state.walking = false;
            state.rotating = false;
            state.idle = false;
            state.attacking = false;

            if (IsAttacking)
            {
                state.attacking = true;
                //Attacking
                updateAttack(time);
            }
            else if (attackTarget == null)
            {
                state.idle = true;
                if (group.army.battleGroup == null)
                {
                    setReGroupState();
                }
            }
            else
            {
                var inReach = checkTargetInReach();

                if (inReach != HasTargetInReach.InReach && mustCompleteAttackSet())
                { //Try to find a nearby target
                    attackTarget = closestTarget(true, SoldierProfile().maxAttackAngle * 4f);
                    inReach = checkTargetInReach();

                    if (inReach == HasTargetInReach.MustRotate)
                    {
                        inReach = HasTargetInReach.InReach;
                    }
                }

                if (inReach != HasTargetInReach.NoTarget)
                {
                    applyTargetReach(inReach);
                }
                else if (group.attacking_soldierGroupOrCity != null)
                {//Walk straight while searching opponent
                    walkStraightForward(time);
                }
                else if (hasWalkingOrder)
                {
                    walkTowards(time, walkingGoal);
                    checkWalkingGoalCompletetion();
                }
                else
                {
                    state.idle = true;
                }
            }

            if (!state.idle)
            {
                recievedProjectileAttackWhileIdle = false;
            }

            void applyTargetReach(HasTargetInReach inReach)
            {
                state.rotating = false;

                switch (inReach)
                {
                    case HasTargetInReach.InReach:
                        commitAttack(fullUpdate);
                        break;
                    case HasTargetInReach.UseBlankTarget:
                        //startAttack(fullUpdate, null, true, true);
                        //onEvent(UnitEventType.StartAttack);
                        break;
                    case HasTargetInReach.MustRotate:
                        state.walking = true;
                        state.rotating = true;
                        rotateTowards(attackTarget, SoldierProfile().rotationSpeed);
                        break;
                    case HasTargetInReach.MustWalk:
                        walkTowards(time, attackTarget.position);
                        break;
                }
            }
        }

        virtual protected void commitAttack(bool fullUpdate)
        {
            if (bonusProjectiles > 0)
            {
                --bonusProjectiles;
                startAttack(fullUpdate, attackTarget, false, true);
            }
            else
            {
                startAttack(fullUpdate, attackTarget, true, true);
            }
            //onEvent(UnitEventType.StartAttack);
        }

        protected AbsDetailUnit closestTarget(bool restrictAngle, float angle)
        {
            FindMinValuePointer<AbsDetailUnit> closest = new FindMinValuePointer<AbsDetailUnit>();

            var attack_sp = group.attacking_soldierGroupOrCity;
            if (attack_sp != null)
            {
                if (attack_sp.gameobjectType() == GameObjectType.SoldierGroup)
                {
                    var soldiersC = attack_sp.GetGroup().soldiers.counter();
                    while (soldiersC.Next())
                    {
                        AbsDetailUnit s = soldiersC.sel;
                        if (s.Alive_IncomingDamageIncluded() && canTargetUnit(s))
                        {
                            if (!restrictAngle || Math.Abs(angleDiff(s)) <= angle)
                            {
                                closest.Next(distanceToUnit(s), s);
                            }
                        }
                    }

                }
                else if (attack_sp.gameobjectType() == GameObjectType.City)
                {
                    var city = attack_sp.GetCity().detailObj;
                    closest.Next(distanceToUnit(city), city);
                }
            }

            return closest.minMember;
        }

        HasTargetInReach checkTargetInReach()
        {
            var target = attackTarget;

            if (target == null ||
                target.Dead() ||
                target.GetFaction() == this.GetFaction())
            {
                attackTarget = null;
                return HasTargetInReach.NoTarget;
            }

            if (spaceBetweenUnits(target) <= nextAttackRange())
            {
                if (Math.Abs(angleDiff(target)) <= SoldierProfile().maxAttackAngle)//0.15f)
                {
                    return HasTargetInReach.InReach;
                }
                else
                {
                    return HasTargetInReach.MustRotate;
                }
            }
            else
            {
                return HasTargetInReach.MustWalk;
            }
        }

        virtual protected float nextAttackRange()
        {
            if (bonusProjectiles > 0)
            {
                return soldierData.secondaryAttackRange;
            }
            return soldierData.attackRange;
        }

        public bool hasWalkingOrder { get { return state.walkingOrderComplete == false; } } //(group != null && group.hasWalkingOrder) && 

        override public void writeNetworkUpdate()
        {
            //var w = Ref.netSession.BeginWritingPacket(Network.PacketType.stupGameObjectUpdate, Network.PacketReliability.Unrelyable);
            ////writeId(w);
            //warsRef.gamestate.writeUnit(w, this);
            //WritePosition(w, model.position);
            //WriteArea(w, inArea);

            //state.write(w);
            ////w.Write(state.walkingOrderComplete);
            //if (state.walkingOrderComplete == false)
            //{
            //    WritePosition(w, walkingGoal);
            //}

            //w.Write(rotation.ByteDir);
            //w.Write(health);

            //netWriteConditions(w);
        }

        override public void readNetworkUpdate(System.IO.BinaryReader r)
        {
            //clientPosition = ReadPosition(r);
            //inArea = ReadArea(r);

            //state.read(r);
            ////walkingOrderComplete = r.ReadBoolean();
            //if (state.walkingOrderComplete == false)
            //{
            //    walkingGoal = ReadPosition(r);
            //}

            //clientRotation.ByteDir = r.ReadByte();
            //health = r.ReadInt32();
            //refreshHealthbar();

            //netReadConditions(r);
        }



        void checkWalkingGoalCompletetion()
        {
            if (goalDistans < GoalReachDist_GROUP)
            {
                state.walkingOrderComplete = true;
            }
        }

        void searchTargetUpdate()
        {
            refreshAttackTarget();
        }

        void asynchFriendlyCollisionsCheck()
        {
            collisionGroup.processList.QuickClear();

            var soldiers = group.soldiers.counter();
            while (soldiers.Next())
            {
                if (soldiers.sel != this)
                {
                    collisionGroupCheck(soldiers.sel, distanceToUnit(soldiers.sel));
                }
            }
            collisionGroup.swap();
        }

        void collisionUpdate()
        {
            if (model != null)
            {
                for (int t = 0; t < Ref.GameTimePassed16ms; ++t)
                {
                    bool hasIdleUnitCollision = false;

                    collisionGroup.loopBegin();
                    while (collisionGroup.loopNext())
                    {
                        var otherModel = collisionGroup.sel.model;
                        if (otherModel != null)
                        {
                            Physics.Collision2D intersection = model.bound.Intersect2(otherModel.bound);
                            if (intersection.IsCollision)
                            {
                                collisionForce += intersection.direction;
                                if (!collisionGroup.sel.state.walking)
                                {
                                    hasIdleUnitCollision = true;
                                }
                            }
                        }
                    }

                    if (hasIdleUnitCollision)
                    {
                        collisionFrames++;
                    }
                    else
                    {
                        collisionFrames = 0;
                    }

                    applyCollisions();
                }
            }

        }

        override public void applyCollisions()
        {
            if (VectorExt.HasValue(collisionForce))
            {
                if (state.walking)
                {
                    collisionForce *= 0.2f;
                }

                position += VectorExt.V2toV3XZ(0.04f * collisionForce);

                collisionForce = Vector2.Zero;
            }
        }

        void updateFollowQue(float time)
        {
            var leadUnit = group.soldiers.GetIndex_Safe(following);
            if (leadUnit != null)
            {
                if (distanceToUnit(leadUnit) > SoldierProfile().groupSpacing)
                {
                    walkTowards(time, leadUnit.position);
                }
                else
                {
                    //Wait
                    state.idle = true;
                    state.walking = false;
                }

                return;
            }

            following = -1;
            setFreeAttack();

        }

        public void setFreeAttack()
        {
            if (aiState != SoldierAiState.ReGroup)
            {
                walkStraightUpdates = 0;

                if (soldierData.canAttackCharacters)
                {
                    aiState = SoldierAiState.FreeAttack;
                }
                else
                {
                    aiState = SoldierAiState.Idle;
                }

            }
        }



        bool walkTowards(float time, Vector3 goal)
        {
            Vector3 walkDir = goal - position;
            walkDir.Y = 0;

            

            float l = walkDir.Length();
            if (l > 0.0001f)
            {
                float speed = walkingSpeedWithModifiers(time);
                if (l < speed * 2)
                {
                    //slow speed
                    speed = Math.Min(speed * 0.2f, l);
                }

                //if (l > speed)
                //{
                    state.walking = true;
                    state2 = SoldierState2.walking;

                    Rotation1D goalDir = Rotation1D.FromDirection(VectorExt.V3XZtoV2(walkDir));

                    float anglediff = rotation.AngleDifference(goalDir);
                    float abs_anglediff = Math.Abs(anglediff);

                    
                    if (abs_anglediff < 0.1f)
                    {

                        rotation = goalDir;
                        walkDir.Normalize();
                        position += walkDir * speed;
                    }
                    //else if (Math.Abs(anglediff) < 1f)
                    //{
                    //    //rotate and walk
                    //    state.rotating = true;
                    //}
                    else
                    {
                        //Stand still and rotate
                        //state2 = SoldierState2.rotating;
                        float rotationSpeed = Math.Min(SoldierProfile().rotationSpeed * Ref.DeltaGameTimeSec, abs_anglediff);
                        rotation.Add(lib.ToLeftRight(anglediff) * rotationSpeed);
                        //state.walking = false;
                        //state.rotating = true;
                    }

                    //if (state.rotating)
                    //{
                    //    rotation.Add(lib.ToLeftRight(anglediff) * SoldierProfile().rotationSpeed * Ref.DeltaGameTimeSec);
                    //}

                    //if (state.walking)
                    //{
                    //    walkDir.Normalize();
                    //    position += walkDir * speed;
                    //}

                    return true;
                //}
            }
            else
            {
                position.X = goal.X;
                position.Z = goal.Z;

                state2 = SoldierState2.waiting;
                state.walking = false;

                return false;
            }
        }

        void walkStraightForward(float time)
        {
            if (++walkStraightUpdates < 20)
            {
                state.walking = true;

                position = VectorExt.AddXZ(position, rotation.Direction(walkingSpeedWithModifiers(time)));
            }
            else
            {
                state.walking = false;
                state.idle = true;
            }
        }

        //protected void updateClientDummieMotion(float time)
        //{
        //    Vector3 posDiff = clientPosition - position;
        //    posDiff.Y = 0;
        //    float length = posDiff.Length();
        //    float speed = 0;

        //    const float MinLenght = 0.04f;

        //    state.walking = length >= MinLenght;

        //    if (state.walking)
        //    {
        //        const float LengthToSpeed = 0.05f;
        //        speed = lib.LargestValue(LengthToSpeed * length, walkingSpeedWithModifiers(time));

        //        posDiff.Normalize();
        //        position += posDiff * speed;
        //    }


        //    float rotDiff = rotation.AngleDifference(clientRotation);
        //    if (Math.Abs(rotDiff) <= 0.01f)
        //    {
        //        rotation = clientRotation;
        //    }
        //    else
        //    {
        //        const float RotationSpeed = 0.4f;
        //        rotation.Add(rotDiff * RotationSpeed);
        //    }
        //}

        public float walkingSpeedWithModifiers(float time)
        {
            //if (group.isShip)
            //{
            //    return data.shipSpeed * group.terrainSpeedMultiplier * time;
            //}
            return soldierData.walkingSpeed * group.terrainSpeedMultiplier * time;
        }



        void rotateTowards(AbsDetailUnit target, float speed)
        {
            if (target != null)
            {
                var angle = angleToUnit(target);

                rotateToAngle(angle.radians);
            }
        }

        void rotateToAngle(float goalAngle)
        {
            float diff = rotation.AngleDifference(goalAngle);
            float speed = SoldierProfile().rotationSpeed * Ref.DeltaGameTimeSec;

            if (Math.Abs(diff) > speed)
            {
                state.rotating = true;
                state2 = SoldierState2.rotating;
                rotation.Add(lib.ToLeftRight(diff) * speed);
            }
            else
            {
                state.rotating = false;
                state2 = SoldierState2.waiting;
                rotation = group.rotation;
            }
        }

        override public void asynchUpdate()
        {
            if (localMember)
            {
                tilePos = WP.ToTilePos(position);

                goalDistans = VectorExt.PlaneXZLength(walkingGoal - position);

                if (aiState == SoldierAiState.ColumnQue)
                {
                    bumpIntoEnemyWhileQue_asynch();

                    if (soldierData.mainAttack != AttackType.Melee || bonusProjectiles > 0)
                    {
                        groupAttackTarget_asynch();
                    }
                }
                else if (aiState == SoldierAiState.FreeAttack)
                {
                    groupAttackTarget_asynch();
                    asynchFriendlyCollisionsCheck();
                }

            }
        }

        void bumpIntoEnemyWhileQue_asynch()
        {
            var attacking_sp = group.attacking_soldierGroupOrCity;

            if (attacking_sp != null)
            {
                var sGroup = attacking_sp.GetGroup();
                if (sGroup != null)
                {
                    var soldiers = sGroup.soldiers.counter();
                    while (soldiers.Next())
                    {
                        if (soldiers.sel.Alive_IncomingDamageIncluded())
                        {
                            if (distanceToUnit(soldiers.sel) < SoldierProfile().groupSpacing)
                            {
                                setFreeAttack();
                                return;
                            }
                        }
                    }
                }
            }
        }

        void groupAttackTarget_asynch()
        {

            AbsDetailUnit closestOpponent = null;
            float closestOpponentDistance = float.MaxValue;
            var attacking_sp = group.attacking_soldierGroupOrCity;

            if (attacking_sp != null)
            {
                switch (attacking_sp.gameobjectType())
                {
                    case GameObjectType.SoldierGroup:
                        var soldiers = attacking_sp.GetGroup().soldiers.counter();
                        while (soldiers.Next())
                        {
                            if (soldiers.sel.Alive_IncomingDamageIncluded())
                            {
                                closestTargetCheck(soldiers.sel,
                                    ref closestOpponent, ref closestOpponentDistance);
                            }
                        }
                        break;

                    case GameObjectType.City:
                        closestTargetCheck(attacking_sp.GetCity().detailObj,
                            ref closestOpponent, ref closestOpponentDistance);
                        break;
                }
            }

            this.nextAttackTarget = closestOpponent;
            refreshAttackTarget();
        }

        virtual public void refreshShipCarryCount()
        { }


        protected override bool canTargetUnit(AbsDetailUnit unit)
        {
            if (unit.Profile().canBeAttackTarget)
            {
                if (unit.IsStructure())
                {
                    return soldierData.canAttackStructure;
                }
                else
                {
                    return soldierData.canAttackCharacters;
                }
            }
            else
            {
                return false;
            }
        }

        public override void selectionFrame(bool hover, Selection selection)
        {
            Vector3 scale = new Vector3(radius * 2f);


            var soldiersC = group.soldiers.counter();
            int i = 0;

            selection.BeginGroupModel(true);
            while (soldiersC.Next())
            {
                selection.setGroupModel(i, soldiersC.sel.position, scale, hover, soldiersC.sel == this, false);
                ++i;
            }
        }

        public override void toHud(ObjectHudArgs args)
        {
            base.toHud(args);

            group.toHud(args);

            stateDebugText(args.content);
        }

        public override void stateDebugText(RichBoxContent content)
        {
            content.newLine();
            content.text("SoldierAiState: " + aiState.ToString());

            content.Add(new RichBoxNewLine(true));
            content.text(group.TypeName());
            group.stateDebugText(content);
        }

        public override void DeleteMe(DeleteReason reason, bool removeFromParent)
        {
            isDeleted = true;
            health = 0;

            deleteModels();

            if (removeFromParent)
            {
                group?.remove(this);
            }
        }

        public override bool defeatedBy(Faction attacker)
        {
            return Dead_IncomingDamageIncluded();
        }

        public override void AddDebugTag()
        {
            base.AddDebugTag();
            group.AddDebugTag();
        }

        //public override AbsDetailUnitProfile Profile()
        //{
        //    return profile;
        //}

        protected bool isGroupLeader { get { return group.soldiers.Get(0) == this; } }

        public override bool IsStructure()
        { return false; }

        public override bool IsSoldierUnit()
        {
            return true;
        }

        public override AbsSoldierUnit GetSoldierUnit()
        {
            return this;
        }

        public override AbsMapObject RelatedMapObject()
        {
            return group.army;
        }

        public override GameObjectType gameobjectType()
        {
            return GameObject.GameObjectType.Soldier;
        }
        public override UnitType DetailUnitType()
        {
            return UnitType;//profile.unitType;
        }
    }

    enum SoldierAiState
    {
        GroupLock,
        ColumnQue,
        FreeAttack,
        Idle,
        ReGroup,
    }

    enum SoldierState2
    {
        idle,
        wakeup,
        walking, 
        rotating,
        waiting,
        //attacking, 
    }
}
