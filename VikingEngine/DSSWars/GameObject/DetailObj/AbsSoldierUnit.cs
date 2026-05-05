using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.EngineSpace.Graphics.In3D;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.PJ.Strategy;
using VikingEngine.ToGG.MoonFall;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.DSSWars.GameObject
{

    abstract class AbsSoldierUnit : AbsDetailUnit
    {
        protected static float GoalReachDist_GROUP = DssVar.StandardBoundRadius * 2f;
        protected static float GoalReachDist_WhenColliding = GoalReachDist_GROUP * 3f;

        IntVector2 prevTilePos;
        GameTimeStamp prevTileTimeStamp = GameTimeStamp.None; 
        
        public Vector3 walkingGoal;
        public Vector2 groupOffset;
        public bool lockMovement = true;

        int walkStraightUpdates = 0;

        public IntVector2 gridPlacement;

        public SoldierState2 state2 = SoldierState2.wakeup;
        public float stateTime;
        public float goalRotation;

        public int bonusProjectiles = 0;

        public UnitBuildType unitBuildType;
        float reactionTime;        
        SoldierBattleData battleData = null;
        public float boundRadius;
        public bool isBannerMan = false;
        public override AbsDetailUnitBuilder Profile()
        {
            return DssRef.units.Get(unitBuildType);
        }
        public AbsSoldierBuilder SoldierProfile()
        {
            return DssRef.units.Get(unitBuildType);
        }

        virtual public void copyDataToUpgradedUnit(AbsSoldierUnit upgradeUnit)
        {
            upgradeUnit.walkingGoal = walkingGoal;
            upgradeUnit.groupOffset = groupOffset;
            upgradeUnit.lockMovement = lockMovement;
            upgradeUnit.gridPlacement = gridPlacement;
            upgradeUnit.myIndex = myIndex;

            upgradeUnit.myIndex = myIndex;
            upgradeUnit.attackTarget = attackTarget;
            upgradeUnit.tilePos = tilePos;
            upgradeUnit.rotation = rotation;
            upgradeUnit.state = state;
            upgradeUnit.position = position;
        }



        public static void OldRead(System.IO.BinaryReader r)
        {
            SoldierAiState aiState = (SoldierAiState)r.ReadByte();
            WP.readPosXZ_old(r, out Vector3 position, out IntVector2 tilePos);
            var rotation = new Rotation1D();
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
            myIndex = group.GetFaction_NoChecks().pickNextUnitId();

            boundRadius = soldierData.boundRadius;

            init(false);
            tilePos = tile;

            bonusProjectiles = soldierData.bonusProjectiles;

            lockMovement = false;

            switch (group.soldierConscript.conscript.training)
            {
                case Conscript.TrainingLevel.Minimal:
                    reactionTime = 300 + Ref.peRnd.Float(300);
                    break;
                case Conscript.TrainingLevel.Basic:
                    reactionTime = 200 + Ref.peRnd.Float(200);
                    break;
                case Conscript.TrainingLevel.Skillful:
                    reactionTime = 100 + Ref.peRnd.Float(100);
                    break;
                case Conscript.TrainingLevel.Professional:
                    reactionTime = 50 + Ref.peRnd.Float(50);
                    break;
            }
        }

        public Physics.CircleBound Bound2D(Physics.CircleBound bound)
        {
            bound.center.X = position.X;
            bound.center.Y = position.Z;
            bound.radius = boundRadius;
            return bound;
        }

        public void initUpgrade(SoldierGroup group)
        {
            this.group = group;

            init(true);
        }

        public void setDetailLevel(bool unitDetailView)
        {
            if (unitDetailView)
            {
                if (model == null)
                {
                    model = initModel(isBannerMan);
                    model.update(this);
                }
            }
            else
            {
                model?.DeleteMe();
                model = null;
            }
        }
        public override string Name(out bool mayEdit)
        {
            IconName.Item(group.soldierConscript.conscript.weapon, out SpriteName weaponIcon, out string weaponName);
            mayEdit = false;
            string name = weaponName;
            if (group.soldierConscript.conscript.armorLevel != Resource.ItemResourceType.NONE)
            {
                IconName.Item(group.soldierConscript.conscript.weapon, out SpriteName armorIcon, out string armorName);
                name += " " + armorName;
            }
            return name;
        }
        public override string TypeName()
        {
            return group.soldierConscript.conscript.TypeName() + " (" + myIndex.ToString() + ")";
        }
        public override void TypeIcon(RichBoxContent content)
        {
            group.TypeIcon(content);
        }
        override public void netShareUnit()
        {
            
        }

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
#if DEBUG
            if (soldierData.basehealth <= 0)
            {
                throw new Exception();
            }
#endif
            health = soldierData.basehealth;
            radius = soldierData.boundRadius;

            if (!asUpgrade)
            {
                refreshGroupOffset();
                updateGroupPosition();
            }
        }

        public override bool rectangleCollision(ScreenToSpaceRectangleBound rectangle)
        {
            return rectangle.Intersects(position, boundRadius);
        }

        public void refreshGroupOffset()
        {
         
            
            groupOffset.X = gridPlacement.X * soldierData.groupSpacing +
                Ref.peRnd.Plus_MinusF(soldierData.groupSpacingRndOffset);

            groupOffset.Y = (gridPlacement.Y + group.halfColDepth) * soldierData.groupSpacing +
                Ref.peRnd.Plus_MinusF(soldierData.groupSpacingRndOffset);
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
            Vector3 result = position;
            Vector2 rotatedOffset = VectorExt.RotateVector(groupOffset, groupRotation);
           
            result.X = groupCenter.X + rotatedOffset.X;
            result.Z = groupCenter.Z + rotatedOffset.Y;

            return result;
        }

        override public void update(float time, bool fullUpdate)
        {
           
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
        public void update_client()
        {
            updateGroudY(false);
        }
        public void update2(float time, bool fullUpdate, float groupWalkSpeed)
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
                followPathUpdate(time, groupWalkSpeed);
                if (group.state == GroupState.GoingIdle && state2 == SoldierState2.waiting)
                {
                    state2 = SoldierState2.idle;
                }

                updateGroudY(false);

                if (fullUpdate)
                {
                    model?.update(this);
                }
            }

           
        }

        void followPathUpdate(float time, float groupWalkSpeed)
        {
            walkingGoal = groupPosition(group.position, group.rotation.radians);
            if (!walkTowards(time, walkingGoal, groupWalkSpeed))
            {
                rotateToAngle(group.rotation.radians);
            }
        }

        bool freeToMove(float time)
        {
            if (battleData != null)
            {
                if (battleData.queueTime > 0)
                { 
                    battleData.queueTime-= time;
                    if (battleData.queueTime <= 0)
                    {
                        battleData.InQueue(this);
                    }
                }

                return battleData.queueTime <= 0;
            }

            return true;
        }

        public void update2_battle_move(float time, bool fullUpate, float groupWalkSpeed)
        {
            followPathUpdate(time, groupWalkSpeed);
            
            battleData?.update(this);

            updateGroudY(false);

            if (fullUpate)
            {
                model?.update(this);
            }
        }
        public void update2_battle_attack(float time, bool fullUpate, float groupWalkSpeed)
        {
            if (group.debugTagged)
            {
                lib.DoNothing();
                var attack = attackTarget;
                var attack2 = nextAttackTarget;

            }

            updateMoveAttackPrio(time, fullUpate, freeToMove(time), groupWalkSpeed);
            
            battleData?.update(this);

            updateGroudY(false);

            if (fullUpate)
            {
                model?.update(this);
            }
        }

        public void update2_battle_attack_static(float time, bool fullUpate, float groupWalkSpeed)
        {
            
            updateMoveAttackPrio(time, fullUpate, false,groupWalkSpeed);

            battleData?.update(this);

            if (fullUpate)
            {
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

        public void teleport()
        {
            firstUpdate();
        }

        public void enterBattleState(bool enter)
        {
            if (enter)
            {
                battleData = new SoldierBattleData(this);
            }
            else
            { 
                battleData = null;
            }
        }

        public void firstUpdate()
        {
            updateGroupPosition();
            updateGroudY(true);

            state.walking = false;
            state.idle = true;
            model?.update(this);

            if (group.state != GroupState.Idle)
            {
                state2 = SoldierState2.wakeup;
                stateTime = 0;
            }
        }

        public void setReGroupState()
        {
            state.walkingOrderComplete = false;
            state.idle = false;
            walkingGoal = groupPosition(group.position, group.rotation.radians);
            bonusProjectiles = soldierData.bonusProjectiles;
        }

        public void setBattleNode()
        {
            walkingGoal = groupPosition(group.goalWp, group.rotation.radians);
        }

        const float ModelGroundYAdj = -0.0001f;
        protected void updateGroudY(bool set)
        {
            if (unitBuildType == UnitBuildType.CityGuard)
            {
                var guards = group.GetGuardGroup();
                if (guards.assignedToPost_IdAndPosition > 0)
                {
                    position.Y = guards.postYPos;
                    return;
                }
            } 
            
            if (DssRef.world.unitBounds.IntersectPoint(position.X, position.Z))//position.X > 0 && position.Z>0)
            {
                float y = DssRef.world.SubTileHeight(position, out SubTile subTile) + ModelGroundYAdj;

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
                            position.Y += diff * 0.2f * Ref.UpdateTimes60FPS;                            
                        }
                    }
                }
            }
        }

        void updateTurn()
        {
            float diff = rotation.AngleDifference(goalRotation);
            float speed = soldierData.rotationSpeed * Ref.DeltaGameTimeSec;

            if (Math.Abs(diff) > speed)
            {
                state.rotating = true;
                rotation.Add(lib.ToLeftRight(diff) * speed);
            }
            else
            {
                state.rotating = false;
                rotation = goalRotation;

                state.walking = true;

                position = VectorExt.AddXZ(position, rotation.Direction(walkingSpeedWithModifiers(Ref.DeltaGameTimeMs)));
                
            }
        }


        protected void updateMoveAttackPrio(float time, bool fullUpdate, bool mayMove, float groupWalkSpeed)
        {
            refreshAttackTarget();

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
            else if (state2 == SoldierState2.Turn)
            {
                updateTurn();
                
                if (battleData.queueTime <= 0)
                { 
                    state2 = SoldierState2.walking;
                }
            }
            else
            {
                var inReach = checkTargetInReach();

                if (inReach != HasTargetInReach.NoTarget)
                {
                    applyTargetReach(inReach);
                }
                else if (group.attackTarget_soldierGroupOrCity != null)
                {//Walk straight while searching opponent
                    if (mayMove)
                    {
                        walkStraightForward(time);
                    }
                    else
                    {
                        rotateTowards(attackTarget, soldierData.rotationSpeed);
                        state.walking = false;
                    }
                }
            }

            if (!state.idle)
            {
                recievedProjectileAttackWhileIdle = false;
            }

            void applyTargetReach(HasTargetInReach inReach)
            {
                state.rotating = false;
                var attackTarget_sp = attackTarget;

                if (attackTarget_sp != null)
                {
                    switch (inReach)
                    {
                        case HasTargetInReach.InReach:
                            commitAttack(fullUpdate);
                            break;
                        case HasTargetInReach.UseBlankTarget:
                            break;
                        case HasTargetInReach.MustRotate:
                            state.walking = true;
                            state.rotating = true;
                            rotateTowards(attackTarget_sp, soldierData.rotationSpeed);
                            break;
                        case HasTargetInReach.MustWalk:
                            if (mayMove)
                            {

                                walkTowards(time, attackTarget_sp.position, groupWalkSpeed);
                            }
                            else
                            {
                                rotateTowards(attackTarget_sp, soldierData.rotationSpeed);
                                state.walking = false;
                            }
                            break;
                    }
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
        }

        protected AbsDetailUnit closestTarget(bool restrictAngle, float angle)
        {
            FindMinValuePointer<AbsDetailUnit> closest = new FindMinValuePointer<AbsDetailUnit>();

            AbsGroup attack_sp = null;
            group.attackTarget_soldierGroupOrCity.TryGetTarget(out attack_sp);
            if (attack_sp != null)
            {
                if (attack_sp.gameobjectType() == GameObjectType.SoldierGroup)
                {
                    var soldiers_sp = attack_sp.GetGroup().soldiers;
                    if (soldiers_sp != null)
                    {
                        var soldiersC = soldiers_sp.counter();
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
                }
               
            }

            return closest.minMember;
        }

        HasTargetInReach checkTargetInReach()
        {
           
            if (attackTarget == null)
            {
                attackTarget = RefExt.Target_safe(group.attackTarget_soldierGroupOrCity)?.Soldiers()?.GetRandomSafe(Ref.peRnd);
            }

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
                return soldierData.secondaryAttackRange + group.soldierAttackRangeBonus;
            }
            return soldierData.attackRange + group.soldierAttackRangeBonus;
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


        bool walkTowards(float time, Vector3 goal, float groupWalkSpeedTime)
        {
            Vector3 walkDir = goal - position;
            walkDir.Y = 0;


            float l = walkDir.Length();
            if (l > 0.0001f)
            {
                float speed = groupWalkSpeedTime;
                float orgsPeed = walkingSpeedWithModifiers(time);
                if (l < speed * 2f)
                {
                    //slow speed
                    speed = Math.Min(speed * 0.2f, l);
                }

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
                    else
                    {
                        //Stand still and rotate
                        float rotationSpeed = Math.Min(soldierData.rotationSpeed * Ref.DeltaGameTimeSec, abs_anglediff);
                        rotation.Add(lib.ToLeftRight(anglediff) * rotationSpeed);
                    }

                    return true;
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

        public float walkingSpeedWithModifiers(float time)
        {
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
            float speed = soldierData.rotationSpeed * Ref.DeltaGameTimeSec;

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
                rotation = goalAngle;
            }
        }

        public void asyncBattleUpdate()
        {
            var newTilePos = WP.ToTilePos(position);

            if (newTilePos != tilePos)
            {
                tilePos = newTilePos;
                prevTilePos = tilePos;
                prevTileTimeStamp.setNow();
            }

            battleData?.asycUpdate(this);
        }

        public override void takeDamage(int damageAmount, float blockReduce, AbsDetailUnit meleeAttacker, Rotation1D attackDir, Faction enemyFaction, bool fullUpdate, out bool blocked)
        {
            float diff = Rotation1D.AngleDifference_Absolute(attackDir.radians, rotation.radians);

            if (diff > MathExt.TauOver3 && Ref.peRnd.ChanceF(soldierData.blockChance * blockReduce))
            {
                var battle_sp = battleData;
                if (battle_sp == null || battle_sp.spendBlock())
                {
                    blocked = true;
                    if (fullUpdate)
                    {
                        GoreManager.ViewBlock(this, damageAmount, attackDir);
                    }
                    return;
                }
            }

            base.takeDamage(damageAmount, blockReduce, meleeAttacker, attackDir, enemyFaction, fullUpdate, out blocked);

            if (meleeAttacker != null)
            {
                battleData?.onTakeMeleeDamage(this, meleeAttacker);
            }
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

        public void selectionFramePlacement(out Vector3 pos, out Vector3 scale)
        {
            pos = position;
            scale = new Vector3(radius * 2f);
        }
        public override void selectionFrame(LocalPlayer player, bool hover, Selection selection)
        {
            var soldiers_sp = group.soldiers;

            if (soldiers_sp != null)
            {
                var soldiersC = soldiers_sp.counter();
                int i = 0;

                selection.groupModels_detail.BeginGroupModel();
                while (soldiersC.Next())
                {
                    soldiersC.sel.selectionFramePlacement(out var pos, out var scale);
                    selection.groupModels_detail.setGroupModel( i, pos, scale, hover, soldiersC.sel == this, false);
                    ++i;
                }

                var target_sp = group.GetAttackTarget();
                if (player.faction == GetFaction() && target_sp != null)
                {
                    selection.TargetLine(ref group.position, ref target_sp.position);
                }
                else
                {
                    selection.hideTargetLine();
                }

                if (group.HasIdleState())
                {
                    selection.viewGroupPath(null);
                }
                else
                {
                    selection.viewGroupPath(group.detailPath);
                }
            }
        }
        public override void toTooltip(ObjectHudArgs args)
        {
            group.toTooltip(args);
        }
        public override void toHud(ObjectHudArgs args)
        {
            group.toHud(args);
            if (args.ShowFull)
            {
                

                stateDebugText(args.content);
            }
        }

        public override void stateDebugText(RichBoxContent content)
        {
            content.newLine();
            content.text("SoldierAiState: " + state2.ToString());

            content.Add(new RbNewLine(true));
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

        public override bool defeatedBy(int attackerFaction)
        {
            return Dead_IncomingDamageIncluded();
        }

        public override void AddDebugTag()
        {
            base.AddDebugTag();
            group.AddDebugTag();
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
            group.army.TryGetTarget(out var tArmy);
            return tArmy;
        }

        public override GameObjectType gameobjectType()
        {
            return GameObject.GameObjectType.Soldier;
        }

        public override AbsSoldierUnit GetSoldier()
        {
            return this;
        }

        public override SoldierGroup GetSoldierGroup()
        {
            return group;
        }

        public override bool IsSoldiers()
        {
            return true;
        }

        public override AbsArmy GetAbsArmy()
        {
            group.army.TryGetTarget(out var tArmy);
            return tArmy;
        }
        public override UnitBuildType DetailUnitType()
        {
            return unitBuildType;
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
        Turn,
    }
}
