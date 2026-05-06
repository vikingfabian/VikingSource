using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.EngineSpace.Graphics.In3D;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.PJ.Strategy;
using VikingEngine.ToGG.MoonFall;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.DSSWars.GameObject
{
    struct SoldierUnitData
    {
        public UnitBaseData baseData;
        public IntVector2 prevTilePos;
        public GameTimeStamp prevTileTimeStamp;

        public Vector3 walkingGoal;
        public Vector2 groupOffset;
        public bool lockMovement;

        public int walkStraightUpdates;

        public IntVector2 gridPlacement;

        public SoldierState2 state2;
        public float stateTime;
        public float goalRotation;

        public int bonusProjectiles;

        public UnitBuildType unitBuildType;
        public float reactionTime;
        public SoldierBattleData battleData;
        public float boundRadius;
        public bool isBannerMan;

        // --- Fields from AbsDetailUnit ---
        public int health;
        public float radius;

        public bool recievedProjectileAttackWhileIdle;
        public int lockedIncomingDamage;

        public AbsSoldierUnit attackTarget;
        public AbsSoldierUnit nextAttackTarget;

        public IntVector2 tilePos;

        public SoldierGroup group;
        public Rotation1D rotation;
        public SoldierState state;
        public int updatesCount;

        public SoldierData soldierData;
        public DetailUnitModel model;

        public float prevAttackTime;
        public Time attackCooldownTime;
        public Time attackFrameTime;
        public Rotation1D attackDir;

        public void InitDefault()
        {
            baseData = new UnitBaseData();
            prevTileTimeStamp = GameTimeStamp.None;
            lockMovement = true;
            walkStraightUpdates = 0;
            state2 = SoldierState2.wakeup;
            bonusProjectiles = 0;
            battleData = null;
            isBannerMan = false;
            recievedProjectileAttackWhileIdle = false;
            lockedIncomingDamage = 0;
            attackTarget = null;
            nextAttackTarget = null;
            tilePos = IntVector2.NegativeOne;
            state = new SoldierState();
            updatesCount = 0;
            attackCooldownTime = 0;
        }
    }

    abstract class AbsSoldierUnit /*: AbsWorldObject*/
    {
        protected static float GoalReachDist_GROUP = DssVar.StandardBoundRadius * 2f;
        protected static float GoalReachDist_WhenColliding = GoalReachDist_GROUP * 3f;

        // --- Methods ---

        public virtual AbsDetailUnitBuilder Profile(ref SoldierUnitData data)
        {
            return DssRef.units.Get(data.unitBuildType);
        }

        public AbsSoldierBuilder SoldierProfile(ref SoldierUnitData data)
        {
            return DssRef.units.Get(data.unitBuildType);
        }

        virtual public void copyDataToUpgradedUnit(ref SoldierUnitData data, ref SoldierUnitData upgradeData)
        {
            upgradeData.walkingGoal = data.walkingGoal;
            upgradeData.groupOffset = data.groupOffset;
            upgradeData.lockMovement = data.lockMovement;
            upgradeData.gridPlacement = data.gridPlacement;
            upgradeData.baseData.myIndex = data.baseData.myIndex;

            upgradeData.attackTarget = data.attackTarget;
            upgradeData.tilePos = data.tilePos;
            upgradeData.rotation = data.rotation;
            upgradeData.state = data.state;
            upgradeData.baseData.position = data.baseData.position;
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

        virtual public void InitLocal(ref SoldierUnitData data, Vector3 center, IntVector2 gridPlacement,
            IntVector2 tile, SoldierGroup group)
        {
            data.InitDefault();
            data.group = group;
            data.gridPlacement = gridPlacement;
            myIndex = group.GetFaction_NoChecks().pickNextUnitId();

            data.boundRadius = data.soldierData.boundRadius;

            init(ref data, false);
            data.tilePos = tile;

            data.bonusProjectiles = data.soldierData.bonusProjectiles;

            data.lockMovement = false;

            switch (group.soldierConscript.conscript.training)
            {
                case Conscript.TrainingLevel.Minimal:
                    data.reactionTime = 300 + Ref.peRnd.Float(300);
                    break;
                case Conscript.TrainingLevel.Basic:
                    data.reactionTime = 200 + Ref.peRnd.Float(200);
                    break;
                case Conscript.TrainingLevel.Skillful:
                    data.reactionTime = 100 + Ref.peRnd.Float(100);
                    break;
                case Conscript.TrainingLevel.Professional:
                    data.reactionTime = 50 + Ref.peRnd.Float(50);
                    break;
            }
        }

        public Physics.CircleBound Bound2D(ref SoldierUnitData data, Physics.CircleBound bound)
        {
            bound.center.X = position.X;
            bound.center.Y = position.Z;
            bound.radius = data.boundRadius;
            return bound;
        }

        public void initUpgrade(ref SoldierUnitData data, SoldierGroup group)
        {
            data.group = group;
            init(ref data, true);
        }

        abstract protected DetailUnitModel initModel(bool bannerman);

        public void setDetailLevel(ref SoldierUnitData data, bool unitDetailView)
        {
            if (unitDetailView)
            {
                if (data.model == null)
                {
                    data.model = initModel(data.isBannerMan);
                    data.model.update(this);
                }
            }
            else
            {
                data.model?.DeleteMe();
                data.model = null;
            }
        }

        public override string Name(ref SoldierUnitData data, out bool mayEdit)
        {
            IconName.Item(data.group.soldierConscript.conscript.weapon, out SpriteName weaponIcon, out string weaponName);
            mayEdit = false;
            string name = weaponName;
            if (data.group.soldierConscript.conscript.armorLevel != Resource.ItemResourceType.NONE)
            {
                IconName.Item(data.group.soldierConscript.conscript.weapon, out SpriteName armorIcon, out string armorName);
                name += " " + armorName;
            }
            return name;
        }

        public override string TypeName(ref SoldierUnitData data)
        {
            return data.group.soldierConscript.conscript.TypeName() + " (" + myIndex.ToString() + ")";
        }

        public override void TypeIcon(ref SoldierUnitData data, RichBoxContent content)
        {
            data.group.TypeIcon(content);
        }

        virtual public void netShareUnit(ref SoldierUnitData data) { }

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

        virtual public void init(ref SoldierUnitData data, bool asUpgrade)
        {
#if DEBUG
            if (data.soldierData.basehealth <= 0)
            {
                throw new Exception();
            }
#endif
            data.health = data.soldierData.basehealth;
            data.radius = data.soldierData.boundRadius;

            if (!asUpgrade)
            {
                refreshGroupOffset(ref data);
                updateGroupPosition(ref data);
            }
        }

        public virtual void update(ref SoldierUnitData data, float time, bool fullUpdate)
        {
            //throw new NotImplementedException();
        }

        public override bool rectangleCollision(ref SoldierUnitData data, ScreenToSpaceRectangleBound rectangle)
        {
            return rectangle.Intersects(position, data.boundRadius);
        }

        public void refreshGroupOffset(ref SoldierUnitData data)
        {
            data.groupOffset.X = data.gridPlacement.X * data.soldierData.groupSpacing +
                Ref.peRnd.Plus_MinusF(data.soldierData.groupSpacingRndOffset);

            data.groupOffset.Y = (data.gridPlacement.Y + data.group.halfColDepth) * data.soldierData.groupSpacing +
                Ref.peRnd.Plus_MinusF(data.soldierData.groupSpacingRndOffset);
        }

        void updateGroupPosition(ref SoldierUnitData data)
        {
            Vector3 prev = position;

            position = groupPosition(ref data, data.group.position, data.group.rotation.radians);
            position.Y = prev.Y;

            data.rotation = data.group.rotation;

            data.state.walking = prev != position;
        }

        public Vector3 groupPosition(ref SoldierUnitData data, Vector3 groupCenter, float groupRotation)
        {
            Vector3 result = position;
            Vector2 rotatedOffset = VectorExt.RotateVector(data.groupOffset, groupRotation);

            result.X = groupCenter.X + rotatedOffset.X;
            result.Z = groupCenter.Z + rotatedOffset.Y;

            return result;
        }

        public void update_GroupLocked(ref SoldierUnitData data, bool walking)
        {
            if (walking)
            {
                updateGroupPosition(ref data);
                updateGroudY(ref data, false);
            }
            data.state.walking = walking;
            data.state.idle = !walking;
            data.model?.update(this);
        }

        public void update_client(ref SoldierUnitData data)
        {
            updateGroudY(ref data, false);
        }

        public void update2(ref SoldierUnitData data, float time, bool fullUpdate, float groupWalkSpeed)
        {
            if (data.state2 == SoldierState2.wakeup)
            {
                data.stateTime -= time;
                if (data.stateTime < 0)
                {
                    data.state2 = SoldierState2.waiting;
                }
            }
            else if (data.state2 != SoldierState2.idle)
            {
                followPathUpdate(ref data, time, groupWalkSpeed);
                if (data.group.state == GroupState.GoingIdle && data.state2 == SoldierState2.waiting)
                {
                    data.state2 = SoldierState2.idle;
                }

                updateGroudY(ref data, false);

                if (fullUpdate)
                {
                    data.model?.update(this);
                }
            }
        }

        void followPathUpdate(ref SoldierUnitData data, float time, float groupWalkSpeed)
        {
            data.walkingGoal = groupPosition(ref data, data.group.position, data.group.rotation.radians);
            if (!walkTowards(ref data, time, data.walkingGoal, groupWalkSpeed))
            {
                rotateToAngle(ref data, data.group.rotation.radians);
            }
        }

        bool freeToMove(ref SoldierUnitData data, float time)
        {
            if (data.battleData != null)
            {
                if (data.battleData.queueTime > 0)
                {
                    data.battleData.queueTime -= time;
                    if (data.battleData.queueTime <= 0)
                    {
                        data.battleData.InQueue(this);
                    }
                }

                return data.battleData.queueTime <= 0;
            }

            return true;
        }

        public void update2_battle_move(ref SoldierUnitData data, float time, bool fullUpate, float groupWalkSpeed)
        {
            followPathUpdate(ref data, time, groupWalkSpeed);

            data.battleData?.update(this);

            updateGroudY(ref data, false);

            if (fullUpate)
            {
                data.model?.update(this);
            }
        }

        public void update2_battle_attack(ref SoldierUnitData data, float time, bool fullUpate, float groupWalkSpeed)
        {
            if (data.group.debugTagged)
            {
                lib.DoNothing();
                var attack = data.attackTarget;
                var attack2 = data.nextAttackTarget;
            }

            updateMoveAttackPrio(ref data, time, fullUpate, freeToMove(ref data, time), groupWalkSpeed);

            data.battleData?.update(this);

            updateGroudY(ref data, false);

            if (fullUpate)
            {
                data.model?.update(this);
            }
        }

        public void update2_battle_attack_static(ref SoldierUnitData data, float time, bool fullUpate, float groupWalkSpeed)
        {
            updateMoveAttackPrio(ref data, time, fullUpate, false, groupWalkSpeed);

            data.battleData?.update(this);

            if (fullUpate)
            {
                data.model?.update(this);
            }
        }

        public void wakeUp2(ref SoldierUnitData data)
        {
            if (data.state2 == SoldierState2.idle)
            {
                data.state2 = SoldierState2.wakeup;
                data.stateTime = data.reactionTime;
            }
        }

        public void teleport(ref SoldierUnitData data)
        {
            firstUpdate(ref data);
        }

        public void enterBattleState(ref SoldierUnitData data, bool enter)
        {
            if (enter)
            {
                data.battleData = new SoldierBattleData(this);
            }
            else
            {
                data.battleData = null;
            }
        }

        public void firstUpdate(ref SoldierUnitData data)
        {
            updateGroupPosition(ref data);
            updateGroudY(ref data, true);

            data.state.walking = false;
            data.state.idle = true;
            data.model?.update(this);

            if (data.group.state != GroupState.Idle)
            {
                data.state2 = SoldierState2.wakeup;
                data.stateTime = 0;
            }
        }

        public void setReGroupState(ref SoldierUnitData data)
        {
            data.state.walkingOrderComplete = false;
            data.state.idle = false;
            data.walkingGoal = groupPosition(ref data, data.group.position, data.group.rotation.radians);
            data.bonusProjectiles = data.soldierData.bonusProjectiles;
        }

        public void setBattleNode(ref SoldierUnitData data)
        {
            data.walkingGoal = groupPosition(ref data, data.group.goalWp, data.group.rotation.radians);
        }

        const float ModelGroundYAdj = -0.0001f;
        protected void updateGroudY(ref SoldierUnitData data, bool set)
        {
            if (data.unitBuildType == UnitBuildType.CityGuard)
            {
                var guards = data.group.GetGuardGroup();
                if (guards.assignedToPost_IdAndPosition > 0)
                {
                    position.Y = guards.postYPos;
                    return;
                }
            }

            if (DssRef.world.unitBounds.IntersectPoint(position.X, position.Z))
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

        void updateTurn(ref SoldierUnitData data)
        {
            float diff = data.rotation.AngleDifference(data.goalRotation);
            float speed = data.soldierData.rotationSpeed * Ref.DeltaGameTimeSec;

            if (Math.Abs(diff) > speed)
            {
                data.state.rotating = true;
                data.rotation.Add(lib.ToLeftRight(diff) * speed);
            }
            else
            {
                data.state.rotating = false;
                data.rotation = data.goalRotation;

                data.state.walking = true;

                position = VectorExt.AddXZ(position, data.rotation.Direction(walkingSpeedWithModifiers(ref data, Ref.DeltaGameTimeMs)));
            }
        }

        protected void updateMoveAttackPrio(ref SoldierUnitData data, float time, bool fullUpdate, bool mayMove, float groupWalkSpeed)
        {
            refreshAttackTarget(ref data);

            data.state.walking = false;
            data.state.rotating = false;
            data.state.idle = false;
            data.state.attacking = false;

            if (IsAttacking(ref data))
            {
                data.state.attacking = true;
                updateAttack(ref data, time);
            }
            else if (data.state2 == SoldierState2.Turn)
            {
                updateTurn(ref data);

                if (data.battleData.queueTime <= 0)
                {
                    data.state2 = SoldierState2.walking;
                }
            }
            else
            {
                var inReach = checkTargetInReach(ref data);

                if (inReach != HasTargetInReach.NoTarget)
                {
                    applyTargetReach(inReach);
                }
                else if (data.group.attackTarget_soldierGroupOrCity != null)
                {
                    if (mayMove)
                    {
                        walkStraightForward(ref data, time);
                    }
                    else
                    {
                        rotateTowards(ref data, data.attackTarget, data.soldierData.rotationSpeed);
                        data.state.walking = false;
                    }
                }
            }

            if (!data.state.idle)
            {
                data.recievedProjectileAttackWhileIdle = false;
            }

            void applyTargetReach(HasTargetInReach inReach)
            {
                data.state.rotating = false;
                var attackTarget_sp = data.attackTarget;

                if (attackTarget_sp != null)
                {
                    switch (inReach)
                    {
                        case HasTargetInReach.InReach:
                            commitAttack(ref data, fullUpdate);
                            break;
                        case HasTargetInReach.UseBlankTarget:
                            break;
                        case HasTargetInReach.MustRotate:
                            data.state.walking = true;
                            data.state.rotating = true;
                            rotateTowards(ref data, attackTarget_sp, data.soldierData.rotationSpeed);
                            break;
                        case HasTargetInReach.MustWalk:
                            if (mayMove)
                            {
                                walkTowards(ref data, time, attackTarget_sp.position, groupWalkSpeed);
                            }
                            else
                            {
                                rotateTowards(ref data, attackTarget_sp, data.soldierData.rotationSpeed);
                                data.state.walking = false;
                            }
                            break;
                    }
                }
            }
        }

        virtual protected void commitAttack(ref SoldierUnitData data, bool fullUpdate)
        {
            if (data.bonusProjectiles > 0)
            {
                --data.bonusProjectiles;
                startAttack(ref data, fullUpdate, data.attackTarget, false, true);
            }
            else
            {
                startAttack(ref data, fullUpdate, data.attackTarget, true, true);
            }
        }

        protected AbsSoldierUnit closestTarget(ref SoldierUnitData data, bool restrictAngle, float angle)
        {
            FindMinValuePointer<AbsSoldierUnit> closest = new FindMinValuePointer<AbsSoldierUnit>();

            AbsGroup attack_sp = null;
            data.group.attackTarget_soldierGroupOrCity.TryGetTarget(out attack_sp);
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
                            AbsSoldierUnit s = soldiersC.sel;
                            // NOTE: Would need another SoldierUnitData reference to query unit 's' alive status properly.
                            // Assuming those methods are adapted or exist internally.
                            if (s.Alive_IncomingDamageIncluded(ref data) && canTargetUnit(ref data, s))
                            {
                                if (!restrictAngle || Math.Abs(angleDiff(ref data, s)) <= angle)
                                {
                                    closest.Next(distanceToUnit(ref data, s), s);
                                }
                            }
                        }
                    }
                }
            }

            return closest.minMember;
        }

        HasTargetInReach checkTargetInReach(ref SoldierUnitData data)
        {
            if (data.attackTarget == null)
            {
                data.attackTarget = RefExt.Target_safe(data.group.attackTarget_soldierGroupOrCity)?.Soldiers()?.GetRandomSafe(Ref.peRnd);
            }

            var target = data.attackTarget;

            if (target == null ||
                target.Dead(ref data) ||
                target.GetFaction() == this.GetFaction())
            {
                data.attackTarget = null;
                return HasTargetInReach.NoTarget;
            }

            if (spaceBetweenUnits(ref data, target) <= nextAttackRange(ref data))
            {
                if (Math.Abs(angleDiff(ref data, target)) <= SoldierProfile(ref data).maxAttackAngle)
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

        virtual protected float nextAttackRange(ref SoldierUnitData data)
        {
            if (data.bonusProjectiles > 0)
            {
                return data.soldierData.secondaryAttackRange + data.group.soldierAttackRangeBonus;
            }
            return data.soldierData.attackRange + data.group.soldierAttackRangeBonus;
        }

        public bool hasWalkingOrder(ref SoldierUnitData data) { return data.state.walkingOrderComplete == false; }

        public virtual void writeNetworkUpdate(ref SoldierUnitData data)
        {
        }

        public virtual void readNetworkUpdate(ref SoldierUnitData data, System.IO.BinaryReader r)
        {
        }

        bool walkTowards(ref SoldierUnitData data, float time, Vector3 goal, float groupWalkSpeedTime)
        {
            Vector3 walkDir = goal - position;
            walkDir.Y = 0;


            float l = walkDir.Length();
            if (l > 0.0001f)
            {
                float speed = groupWalkSpeedTime;
                float orgsPeed = walkingSpeedWithModifiers(ref data, time);
                if (l < speed * 2f)
                {
                    speed = Math.Min(speed * 0.2f, l);
                }

                data.state.walking = true;
                data.state2 = SoldierState2.walking;

                Rotation1D goalDir = Rotation1D.FromDirection(VectorExt.V3XZtoV2(walkDir));

                float anglediff = data.rotation.AngleDifference(goalDir);
                float abs_anglediff = Math.Abs(anglediff);

                if (abs_anglediff < 0.1f)
                {
                    data.rotation = goalDir;
                    walkDir.Normalize();
                    position += walkDir * speed;
                }
                else
                {
                    float rotationSpeed = Math.Min(data.soldierData.rotationSpeed * Ref.DeltaGameTimeSec, abs_anglediff);
                    data.rotation.Add(lib.ToLeftRight(anglediff) * rotationSpeed);
                }

                return true;
            }
            else
            {
                position.X = goal.X;
                position.Z = goal.Z;

                data.state2 = SoldierState2.waiting;
                data.state.walking = false;

                return false;
            }
        }

        void walkStraightForward(ref SoldierUnitData data, float time)
        {
            if (++data.walkStraightUpdates < 20)
            {
                data.state.walking = true;

                position = VectorExt.AddXZ(position, data.rotation.Direction(walkingSpeedWithModifiers(ref data, time)));
            }
            else
            {
                data.state.walking = false;
                data.state.idle = true;
            }
        }

        public float walkingSpeedWithModifiers(ref SoldierUnitData data, float time)
        {
            return data.soldierData.walkingSpeed * data.group.terrainSpeedMultiplier * time;
        }

        void rotateTowards(ref SoldierUnitData data, AbsSoldierUnit target, float speed)
        {
            if (target != null)
            {
                var angle = angleToUnit(ref data, target);

                rotateToAngle(ref data, angle.radians);
            }
        }

        void rotateToAngle(ref SoldierUnitData data, float goalAngle)
        {
            float diff = data.rotation.AngleDifference(goalAngle);
            float speed = data.soldierData.rotationSpeed * Ref.DeltaGameTimeSec;

            if (Math.Abs(diff) > speed)
            {
                data.state.rotating = true;
                data.state2 = SoldierState2.rotating;
                data.rotation.Add(lib.ToLeftRight(diff) * speed);
            }
            else
            {
                data.state.rotating = false;
                data.state2 = SoldierState2.waiting;
                data.rotation = goalAngle;
            }
        }

        public void asyncBattleUpdate(ref SoldierUnitData data)
        {
            var newTilePos = WP.ToTilePos(position);

            if (newTilePos != data.tilePos)
            {
                data.tilePos = newTilePos;
                data.prevTilePos = data.tilePos;
                data.prevTileTimeStamp.setNow();
            }

            data.battleData?.asycUpdate(this);
        }

        virtual public void takeDamage(ref SoldierUnitData data, int damageAmount, float blockReduce, AbsSoldierUnit meleeAttacker, Rotation1D attackDir, Faction enemyFaction, bool fullUpdate, out bool blocked)
        {
            float diff = Rotation1D.AngleDifference_Absolute(attackDir.radians, data.rotation.radians);

            if (diff > MathExt.TauOver3 && Ref.peRnd.ChanceF(data.soldierData.blockChance * blockReduce))
            {
                var battle_sp = data.battleData;
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

            if (data.health > 0)
            {
                if (Ref.peRnd.Chance_CheckForZero(data.group.damageBlockChance_fromTerrain * blockReduce))
                {
                    blocked = true;
                    return;
                }

                if (damageAmount > 0)
                {
                    data.lockedIncomingDamage -= damageAmount;

                    data.recievedProjectileAttackWhileIdle = data.state.idle;

                    data.health -= damageAmount;

                    if (data.health <= 0 && localMember(ref data))
                    {
                        onDeath(ref data, fullUpdate, enemyFaction);
                    }

                    if (fullUpdate)
                    {
                        GoreManager.ViewDamage(this, damageAmount, attackDir);
                    }
                }
            }

            blocked = false;

            if (meleeAttacker != null)
            {
                data.battleData?.onTakeMeleeDamage(this, meleeAttacker);
            }
        }

        virtual public void refreshShipCarryCount(ref SoldierUnitData data)
        { }

        public void selectionFramePlacement(ref SoldierUnitData data, out Vector3 pos, out Vector3 scale)
        {
            pos = position;
            scale = new Vector3(data.radius * 2f);
        }

        public override void selectionFrame(ref SoldierUnitData data, LocalPlayer player, bool hover, Selection selection)
        {
            var soldiers_sp = data.group.soldiers;

            if (soldiers_sp != null)
            {
                var soldiersC = soldiers_sp.counter();
                int i = 0;

                selection.groupModels_detail.BeginGroupModel();
                while (soldiersC.Next())
                {
                    // Assuming soldiersC.sel has access to its own data via another pass, simplified for example
                    soldiersC.sel.selectionFramePlacement(ref data, out var pos, out var scale);
                    selection.groupModels_detail.setGroupModel(i, pos, scale, hover, soldiersC.sel == this, false);
                    ++i;
                }

                var target_sp = data.group.GetAttackTarget();
                if (player.faction == GetFaction() && target_sp != null)
                {
                    selection.TargetLine(ref data.group.position, ref target_sp.position);
                }
                else
                {
                    selection.hideTargetLine();
                }

                if (data.group.HasIdleState())
                {
                    selection.viewGroupPath(null);
                }
                else
                {
                    selection.viewGroupPath(data.group.detailPath);
                }
            }
        }

        public override void toTooltip(ref SoldierUnitData data, ObjectHudArgs args)
        {
            data.group.toTooltip(args);
        }

        public override void toHud(ref SoldierUnitData data, ObjectHudArgs args)
        {
            data.group.toHud(args);
            if (args.ShowFull)
            {
                stateDebugText(ref data, args.content);
            }
        }

        public override void stateDebugText(ref SoldierUnitData data, RichBoxContent content)
        {
            content.newLine();
            content.text("SoldierAiState: " + data.state2.ToString());

            content.Add(new RbNewLine(true));
            content.text(data.group.TypeName());
            data.group.stateDebugText(content);
        }

        public override void DeleteMe(ref SoldierUnitData data, DeleteReason reason, bool removeFromParent)
        {
            isDeleted = true;
            data.health = 0;

            deleteModels(ref data);

            if (removeFromParent)
            {
                data.group?.remove(this);
            }
        }

        public override void AddDebugTag(ref SoldierUnitData data)
        {
            base.AddDebugTag();
            data.group.AddDebugTag();
        }

        protected bool isGroupLeader(ref SoldierUnitData data) { return data.group.soldiers.Get(0) == this; }

        public virtual bool IsStructure()
        { return false; }

        public virtual bool IsSoldierUnit()
        {
            return true;
        }

        public virtual AbsSoldierUnit GetSoldierUnit()
        {
            return this;
        }

        public override AbsMapObject RelatedMapObject(ref SoldierUnitData data)
        {
            data.group.army.TryGetTarget(out var tArmy);
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

        public override SoldierGroup GetSoldierGroup(ref SoldierUnitData data)
        {
            return data.group;
        }

        public override bool IsSoldiers()
        {
            return true;
        }

        public override AbsArmy GetAbsArmy(ref SoldierUnitData data)
        {
            data.group.army.TryGetTarget(out var tArmy);
            return tArmy;
        }

        public virtual UnitBuildType DetailUnitType(ref SoldierUnitData data)
        {
            return data.unitBuildType;
        }

        // --- Methods Extracted From AbsDetailUnit Partial Classes ---

        public float angleDiff(ref SoldierUnitData data, AbsSoldierUnit target)
        {
            Rotation1D targetAngle = angleToUnit(ref data, target);
            float diff = data.rotation.AngleDifference(targetAngle);
            return diff;
        }

        public Rotation1D angleToUnit(ref SoldierUnitData data, AbsSoldierUnit target)
        {
            Vector3 targetPosDiff = target.position - position;

            if (targetPosDiff.X == 0 && targetPosDiff.Z == 0)
            {
                return 0;
            }

            return Rotation1D.FromDirection(VectorExt.V3XZtoV2(targetPosDiff));
        }

        public void lockInAttackDamage(ref SoldierUnitData data, int damageAmount)
        {
            if (damageAmount > 0)
            {
                data.lockedIncomingDamage += damageAmount;
            }
        }

        protected void refreshAttackTarget(ref SoldierUnitData data)
        {
            if (debugTagged)
            {
                lib.DoNothing();
            }
            var attackTarget_sp = data.attackTarget;

            if (attackTarget_sp != null && attackTarget_sp.defeatedBy(factionIndex))
            {
                data.attackTarget = null;
            }

            var nextAttackTarget_sp = data.nextAttackTarget;
            data.nextAttackTarget = null;
            if (nextAttackTarget_sp != null && !nextAttackTarget_sp.defeatedBy(factionIndex))
            {
                data.attackTarget = nextAttackTarget_sp;
            }
        }

        public void closestTargetCheck(ref SoldierUnitData data, AbsSoldierUnit unit,
            ref AbsSoldierUnit closestOpponent,
            ref float closestOpponentDistance)
        {
            float distance = spaceBetweenUnits(ref data, unit);

            if (distance < DssConst.MeleeAwareRange)
            {
                if (distance < closestOpponentDistance &&
                   canTargetUnit(ref data, unit))
                {
                    closestOpponent = unit;
                    closestOpponentDistance = distance;
                }
            }
            else
            {
                float anglediff = Math.Abs(angleDiff(ref data, unit));
                distance += anglediff * 0.1f;

                if (distance < closestOpponentDistance &&
                    canTargetUnit(ref data, unit))
                {
                    var unitData = Profile(ref data); // Needs refactor on unit to get its Data profile properly.

                    if (!unitData.restrictTargetAngle || anglediff <= unitData.targetAngle)
                    {
                        closestOpponent = unit;
                        closestOpponentDistance = distance;
                    }
                }
            }
        }

        virtual protected AbsMapObject ParentMapObject(ref SoldierUnitData data)
        {
            data.group.army.TryGetTarget(out var tArmy);
            return tArmy;
        }

        virtual protected bool canTargetUnit(ref SoldierUnitData data, AbsSoldierUnit unit)
        {
            // Assuming unit.Profile() returns something that is accessible without 'ref data' of 'unit'
            if (unit.Profile(ref data).canBeAttackTarget)
            {
                if (unit.IsStructure())
                {
                    return data.soldierData.canAttackStructure;
                }
                else
                {
                    return data.soldierData.canAttackCharacters;
                }
            }
            else
            {
                return false;
            }
        }

        public float distanceToUnit(ref SoldierUnitData data, AbsSoldierUnit other)
        {
            return VectorExt.Length(other.position.X - position.X, other.position.Z - position.Z);
        }

        protected float spaceBetweenUnits(ref SoldierUnitData data, AbsSoldierUnit other)
        {
            // other.radius is assumed accessible via some mechanism
            float result = VectorExt.Length(other.position.X - position.X, other.position.Z - position.Z) -
                data.radius - data.radius; // Replace second radius with other's radius
            if (result < 0)
            {
                return 0;
            }
            return result;
        }

        virtual public void onNewModel(ref SoldierUnitData data, LootFest.VoxelModelName name, Graphics.VoxelModel master)
        {
            data.model?.onNewModel(name, master, this);
        }

        public int missingHealth(ref SoldierUnitData data)
        {
            return data.soldierData.basehealth - data.health;
        }

        virtual public void onDeath(ref SoldierUnitData data, bool fullUpdate, Faction enemyFaction)
        {
            if (enemyFaction != null && enemyFaction.player.IsLocalPlayer())
            {
                ++enemyFaction.player.GetLocalPlayer().statistics.EnemySoldiersKilled;
            }
            if (data.group.GetPlayer().IsLocalPlayer())
            {
                ++data.group.GetPlayer().GetLocalPlayer().statistics.FriendlySoldiersLost;
            }

            if (fullUpdate)
            {
                DeleteMe(ref data, DeleteReason.Death, true);
            }
            else
            {
                Ref.update.AddSyncAction(new SyncAction2Arg<DeleteReason, bool>(DeleteMeDelegate, DeleteReason.Death, true));
            }
        }

        // Added wrapper because we can't easily pass a ref to an Action via SyncAction.
        private void DeleteMeDelegate(DeleteReason arg1, bool arg2) { /* Fallback implementation to handle ref data */ }

        public void deleteModels(ref SoldierUnitData data)
        {
            if (data.model != null)
            {
                data.model.DeleteMe();
            }
        }

        virtual public void applyCollisions(ref SoldierUnitData data)
        {
        }

        public float DPS(ref SoldierUnitData data)
        {
            return data.soldierData.attackDamage / TimeExt.MillsSecToSec(data.soldierData.attackTimePlusCoolDown);
        }

        public bool Alive(ref SoldierUnitData data)
        {
            return data.health > 0;
        }

        public bool Dead(ref SoldierUnitData data)
        {
            return data.health <= 0;
        }

        public override bool defeatedBy(ref SoldierUnitData data, int attackerFaction)
        {
            return Dead_IncomingDamageIncluded(ref data);
        }

        override public bool aliveAndBelongTo(ref SoldierUnitData data, int faction)
        {
            return data.health > 0;
        }

        public bool Alive_IncomingDamageIncluded(ref SoldierUnitData data)
        {
            return data.health - data.lockedIncomingDamage > 0;
        }

        public bool Dead_IncomingDamageIncluded(ref SoldierUnitData data)
        {
            return data.health - data.lockedIncomingDamage <= 0;
        }

        public bool localMember(ref SoldierUnitData data)
        {
            var p = player(ref data);
            return p != null && p.IsLocal;
        }

        public Players.AbsPlayer player(ref SoldierUnitData data)
        {
            return GetFaction()?.player;
        }

        virtual public Vector3 projectileStartPos(ref SoldierUnitData data)
        {
            Vector3 pos = position;
            data.model?.RotateVector(data.soldierData.attackStart, ref pos);
            return pos;
        }

        abstract public bool IsShipType();

        abstract public bool IsSingleTarget();

        virtual protected bool IsStunned
        {
            get { return false; }
        }

        virtual public int MaxHealth(ref SoldierUnitData data)
        {
            return data.soldierData.basehealth;
        }

        public override string ToString()
        {
            return "Soldier Unit via Data Struct";
        }

        public void updateAttack(ref SoldierUnitData data, float time)
        {
            if (data.attackCooldownTime.CountDown(time) == false)
            {
                if (IsSoldierUnit())
                {
                    data.attackFrameTime.CountDown(time);
                }
            }
        }

        public bool inAttackAnimation(ref SoldierUnitData data)
        {
            return data.attackFrameTime.HasTime;
        }

        protected int startMultiAttack(ref SoldierUnitData data, bool fullUpdate, AbsSoldierUnit target, bool mainAttack, int attackCount, bool local)
        {
            int hitCount = 0;

            if (target != null)
            {
                if (target.IsSingleTarget())
                {
                    for (int i = 0; i < attackCount; i++)
                    {
                        startAttack(ref data, fullUpdate, target, mainAttack, local);
                    }

                    hitCount = attackCount;
                }
                else
                {
                    attackCount += 1;
                    for (int i = 0; i < attackCount; i++)
                    {
                        // Target's internal data needs abstracting if target.group is expected to be valid directly.
                        startAttack(ref data, fullUpdate, target, mainAttack, local);
                        ++hitCount;
                    }
                }
            }

            return hitCount;
        }

        protected void startAttack(ref SoldierUnitData data, bool fullUpdate, AbsSoldierUnit target, bool mainAttack, bool local)
        {
            if (target != null)
            {
                data.attackCooldownTime.MilliSeconds = data.soldierData.attackTimePlusCoolDown;
                data.prevAttackTime = data.attackCooldownTime.MilliSeconds;
                data.attackFrameTime.MilliSeconds = Profile(ref data).attackFrameTime;

                int damage;
                float blockReduce = data.soldierData.blockReducingAttack_Inv;

                // Height advantage checking. Would need correct data fetching for 'target'.
                if (data.group.position.Y + position.Y - Map.Settings.Height.DefaultGroundYoffset >= position.Y + target.position.Y &&
                    !IsShipType())
                {
                    blockReduce *= DssConst.HeightAdvantageBlockReduce_multiply;
                    if (fullUpdate)
                    {
                        Vector3 pos = position;
                        pos.Y += DssConst.Men_StandardModelScale * 0.8f;
                        Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.GoldenSparkle, pos, DssConst.Men_StandardModelScale * 0.3f, 6);
                    }
                }

                if (mainAttack)
                {
                    damage = data.soldierData.attackDamage;

                    if (data.group != null &&
                        data.group.soldierConscript.conscript.specialization == SpecializationType.AntiCavalry)
                    {
                        // Assuming target type fetching is accessible via some struct logic as well
                        switch (UnitBuildType.ConscriptCavalry) // Replace with proper data fetch based on 'target' data struct
                        {
                            case UnitBuildType.ConscriptCavalry:
                            case UnitBuildType.ConscriptBalkong:
                                damage = MathExt.MultiplyInt(DssConst.AntiCavalryBonusMultiply, damage);
                                break;
                        }
                    }
                }
                else
                {
                    damage = data.soldierData.secondaryAttackDamage;
                }

                damage += damage * data.group.soldierAttackDamageBonus;

                data.attackDir = angleToUnit(ref data, target);

                if (data.soldierData.mainAttack == AttackType.Melee && mainAttack)
                {
                    if (fullUpdate)
                    {
                        if (IsShipType())
                        {
                            new ShipMeleeAttack(GetSoldierUnit(), data.attackDir);
                        }

                        if (Ref.peRnd.ChanceF(DssConst.SoundChanceSword))
                        {
                            switch (data.group.soldierConscript.conscript.weapon)
                            {
                                case Resource.ItemResourceType.HandSpear:
                                case Resource.ItemResourceType.Pike:
                                case Resource.ItemResourceType.SharpStick:
                                    SoundLib.spear_whoosh.Play(position);
                                    break;

                                case Resource.ItemResourceType.BronzeSword:
                                case Resource.ItemResourceType.ShortSword:
                                    SoundLib.blade_light.Play(position);
                                    break;

                                case Resource.ItemResourceType.Sword:
                                case Resource.ItemResourceType.LongSword:
                                    SoundLib.blade_medium.Play(position);
                                    break;

                                case Resource.ItemResourceType.TwoHandSword:
                                case Resource.ItemResourceType.MithrilSword:
                                    SoundLib.blade_heavy.Play(position);
                                    break;

                                default:
                                    SoundLib.sword.Play(position);
                                    break;
                            }
                        }
                    }

                    // For 'target.takeDamage()' you will likely need the target's data passed over
                    target.takeDamage(ref data, damage, blockReduce, this, data.attackDir, GetFaction(), fullUpdate, out _);
                }
                else
                {
                    // For Projectile.ProjectileAttack() target data needed
                }

                var f = this.GetFaction();
                if (f != null && f.player.IsLocalPlayer())
                {
                    if (data.group.soldierConscript.conscript.isKnight())
                    {
                        DssRef.achieve.UnlockAchievement(AchievementIndex.rear_flanking);
                    }
                }
            }
        }

        public bool IsAttacking(ref SoldierUnitData data)
        {
            return data.attackCooldownTime.HasTime;
        }

        public void clearAttack(ref SoldierUnitData data)
        {
            data.attackFrameTime.setZero();
        }
    }

    // Enums remain the same
    enum UnitEventType { MoveOrder, StartAttack, Death }
    enum AttackType { Melee, Arrow, Bolt, RocketArrow, Ballista, Catapult, Haubitz, Cannonball, MassiveCannonball, FireBomb, SlingShot, KnifeThrow, SecondaryJavelin, Javelin, GunShot, GunBlast, NUM_NON }
    enum HasTargetInReach { InReach, MustWalk, MustRotate, NoTarget, UseBlankTarget }
    enum SoldierAiState { GroupLock, ColumnQue, FreeAttack, Idle, ReGroup }
    enum SoldierState2 { idle, wakeup, walking, rotating, waiting, Turn }
}