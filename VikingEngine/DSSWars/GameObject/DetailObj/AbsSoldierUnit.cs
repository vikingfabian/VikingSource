﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject
{

   abstract class AbsSoldierUnit: AbsDetailUnit
    {
       public const int SoldierAiState_GroupLock = 0;
        const int SoldierAiState_ColumnQue = 1;
        const int SoldierAiState_FreeAttack = 2;
        const int SoldierAiState_Idle = 3;
        const int SoldierAiState_ReGroup = 4;

        protected const float GoalReachDist_GROUP = AbsSoldierData.StandardBoundRadius * 2f;
        protected const float GoalReachDist_WhenColliding = GoalReachDist_GROUP * 3f;
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

        public int aiState = SoldierAiState_GroupLock;
        public int following = -1;

        public int bonusProjectiles = 0;
        

        public AbsSoldierData data;

        virtual public void copyDataToUpgradedUnit(AbsSoldierUnit upgradeUnit)
        {
            upgradeUnit.walkingGoal = walkingGoal;
            upgradeUnit.groupOffset = groupOffset;
            upgradeUnit.lockMovement = lockMovement;
            upgradeUnit.gridPlacement = gridPlacement;
            upgradeUnit.aiState = aiState;
            upgradeUnit.parentArrayIndex = parentArrayIndex;
            upgradeUnit.following = following;

            upgradeUnit.id = id;
            upgradeUnit.attackTarget = attackTarget;
            upgradeUnit.tilePos = tilePos;
            upgradeUnit.rotation = rotation;
            upgradeUnit.state = state;
            upgradeUnit.position = position;
        }

        

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)data.unitType);
            WritePosition(w, position);
            w.Write(rotation.ByteDir);
        }

        public void read(System.IO.BinaryReader r, Players.AbsPlayer player, SoldierGroup group)
        {

        }

        virtual public void InitLocal(Vector3 center, IntVector2 gridPlacement, 
            IntVector2 tile, SoldierGroup group)
        {
            this.group = group;
            this.gridPlacement = gridPlacement;
            id = group.army.faction.pickNextUnitId();

            init(false);
            tilePos = tile;

            bonusProjectiles = data.bonusProjectiles;

            lockMovement=false;
        }

        public void initUpgrade(SoldierGroup group)
        {
            this.group = group;

            init(true);
        }
        
        override public void netShareUnit()
        {
            //var w = beginWriteAddUnit(player);
            //WritePosition(w, model.position);
        }

        override public void InitRemote(Players.AbsPlayer player, System.IO.BinaryReader r)
        {
            //base.InitRemote(player, r);

            //readId(r);
            //Vector3 startPos = ReadPosition(r);
            //clientPosition = startPos;

            //init(startPos);
        }

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
            health = data.basehealth;
            radius = data.boundRadius;

            if (!asUpgrade)
            {
                refreshGroupOffset();
                updateGroupPosition();
            }
        }
                
        public void refreshGroupOffset()
        {
            groupOffset.X = gridPlacement.X * data.groupSpacing +
                Ref.rnd.Plus_MinusF(data.groupSpacingRndOffset);

            groupOffset.Y = (gridPlacement.Y + group.halfColDepth) * data.groupSpacing +
                Ref.rnd.Plus_MinusF(data.groupSpacingRndOffset);
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
            if (id == 37)
            {
                lib.DoNothing();
            }
            Vector3 result = position;
            Vector2 rotatedOffset = VectorExt.RotateVector(groupOffset, groupRotation);
            if (Math.Abs(rotatedOffset.X) > 0.5f)
            {
                lib.DoNothing();
            }
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
                        case SoldierAiState_ColumnQue:
                             if (data.mainAttack != AttackType.Melee ||
                                bonusProjectiles > 0)
                            {
                                updateRangeAttackIfAble(time, fullUpdate);
                            }
                            updateFollowQue(time);
                            break;
                        case SoldierAiState_FreeAttack:
                            updateMoveAttackPrio(time, fullUpdate);
                            collisionUpdate();
                            break;
                        case SoldierAiState_Idle:
                            state.idle = true;
                            state.walking = false;
                            break;
                        case SoldierAiState_ReGroup:
                            if (!walkTowards(time, walkingGoal))
                            {
                                rotateToAngle(group.rotation.radians);
                                state.idle = !state.rotating;
                            }
                            collisionUpdate();
                            break;
                    }
                    
                    updateGroudY(false);
                }
                else
                {
                    switch (aiState)
                    {
                        case SoldierAiState_FreeAttack:
                            updateMoveAttackPrio(time, fullUpdate);
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
            state.walking= walking;
            state.idle= !walking;
            model?.update(this);
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
            aiState = SoldierAiState_ReGroup;
            state.walkingOrderComplete = false;
            state.idle = false;
            walkingGoal = groupPosition(group.position, group.rotation.radians);
            bonusProjectiles = data.bonusProjectiles;
        }


        public void setAttackState()
        {
            aiState = SoldierAiState_ColumnQue;

            if (following < 0)
            {
                setFreeAttack();
            }
        }

        const float ModelGroundYAdj = 0.02f;
        void updateGroudY(bool set)
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
                    rotateTowards(attackTarget, data.rotationSpeed);
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
            else
            {
                var inReach = checkTargetInReach();

                if (inReach != HasTargetInReach.InReach && mustCompleteAttackSet())
                { //Try to find a nearby target
                    attackTarget = closestTarget(true, data.maxAttackAngle * 4f);
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
                        rotateTowards(attackTarget, data.rotationSpeed);
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
                target.Faction() == this.Faction())
            {
                attackTarget = null;
                return HasTargetInReach.NoTarget;
            }

            if (spaceBetweenUnits(target) <= nextAttackRange())
            {
                if (Math.Abs(angleDiff(target)) <= data.maxAttackAngle)//0.15f)
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
                return data.secondaryAttackRange;
            }
            return data.attackRange;
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
            var leadUnit = group.soldiers[following];
            if (leadUnit != null)
            {
                if (distanceToUnit(leadUnit) > data.groupSpacing)
                {
                    walkTowards(time, leadUnit.position);
                }
                else
                {
                    //Wait
                    state.idle = true;
                    state.walking = false;
                }
            }
            else
            {
                following = -1;
                setFreeAttack();
            }
        }

        public void setFreeAttack()
        {
            if (aiState != SoldierAiState_ReGroup)
            { 
                walkStraightUpdates = 0;

                if (data.canAttackCharacters)
                {
                    aiState = SoldierAiState_FreeAttack;
                }
                else
                {
                    aiState = SoldierAiState_Idle;
                }

            }
        }

        

        bool walkTowards(float time, Vector3 goal)
        {
            Vector3 walkDir = goal - position;
            walkDir.Y = 0;

            float speed = walkingSpeedWithModifiers(time);

            if (walkDir.Length() > speed)
            {
                state.walking = true;


                Rotation1D goalDir = Rotation1D.FromDirection(VectorExt.V3XZtoV2(walkDir));

                float anglediff = rotation.AngleDifference(goalDir);

                if (Math.Abs(anglediff) < 0.1f)
                {
                    rotation = goalDir;
                }
                else if (Math.Abs(anglediff) < 1f)
                {
                    //rotate and walk
                    state.rotating = true;
                }
                else
                {
                    //Stand still and rotate
                    state.walking = false;
                    state.rotating = true;
                }

                if (state.rotating)
                {
                    rotation.Add(lib.ToLeftRight(anglediff) * data.rotationSpeed * Ref.DeltaGameTimeSec);
                }

                if (state.walking)
                {
                    walkDir.Normalize();
                    position += walkDir * speed;
                }

                return true;
            }
            else
            {
                position.X = goal.X;
                position.Z = goal.Z;

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
            return data.walkingSpeed * group.terrainSpeedMultiplier * time;
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
            float speed = data.rotationSpeed * Ref.DeltaGameTimeSec;

            if (Math.Abs(diff) > speed)
            {
                state.rotating = true;
                rotation.Add(lib.ToLeftRight(diff) * speed);
            }
            else
            {
                state.rotating = false;
                rotation = group.rotation;
            }
        }

        override public void asynchUpdate()
        {
            if (localMember)
            {
                tilePos = WP.ToTilePos(position);

                goalDistans = VectorExt.PlaneXZLength(walkingGoal - position);

                if (aiState == SoldierAiState_ColumnQue)
                {
                    bumpIntoEnemyWhileQue_asynch();

                    if (data.mainAttack != AttackType.Melee || bonusProjectiles > 0)
                    {
                        groupAttackTarget_asynch();
                    }
                }
                else if (aiState == SoldierAiState_FreeAttack)
                {
                    groupAttackTarget_asynch();
                    asynchFriendlyCollisionsCheck();
                }
                
            }
        }

        void bumpIntoEnemyWhileQue_asynch()
        {
            var attacking_sp = group.attacking_soldierGroupOrCity;

            if (attacking_sp!=null)
            {
                var sGroup = attacking_sp.GetGroup();
                if (sGroup != null)
                {
                    var soldiers = sGroup.soldiers.counter();
                    while (soldiers.Next())
                    {
                        if (soldiers.sel.Alive_IncomingDamageIncluded())
                        {
                            if (distanceToUnit(soldiers.sel) < data.groupSpacing)
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
            if (unit.Data().canBeAttackTarget)
            {
                if (unit.IsStructure())
                {
                    return data.canAttackStructure;
                }
                else
                {
                    return data.canAttackCharacters;
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

            selection.BeginGroupModel();
            while (soldiersC.Next())
            {
                selection.setGroupModel(i, soldiersC.sel.position, scale, hover, soldiersC.sel == this);
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
            content.text(group.Name());
            group.stateDebugText(content);
        }
        
        public override void DeleteMe(DeleteReason reason, bool removeFromParent)
        {   
            isDeleted= true;
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

        public override void tagObject()
        {
            base.tagObject();
            group.tagObject();
        }

        public override AbsDetailUnitData Data()
        {
            return data;
        }

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
            return data.unitType;
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
}
