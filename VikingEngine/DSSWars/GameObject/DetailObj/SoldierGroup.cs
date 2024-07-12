using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.GameObject
{

    /*
     * Förflyttning
     * -Kan gå mot nod eller object
     * -nod: är den ruta bort, kolla om den är längst armypath, annars egen path
     * -object: kolla avstånd
     */

    partial class SoldierGroup : AbsGroup
    {
        public const int GroupObjective_FollowArmyObjective = 0;
        public const int GroupObjective_IsSplit = 1;
        public const int GroupObjective_ReGrouping = 2;
        public const int GroupObjective_FindArmyPlacement = 3;

        int skirmishCount;

        public float halfColDepth;

       
        public const float GroupSpacing = AbsSoldierData.RowWidth * AbsSoldierData.DefaultGroupSpacing * 1.2f;

        //public bool hasWalkingOrder = false;
        public SpottedArray<AbsSoldierUnit> soldiers;

        public Army army;
        public IntVector2 walkingOrderTo;

        //public Time groupWaitingTime;
        public bool groupIsIdle = false;
        public bool allInduvidualsAreIdle = false;
        //public SpriteName icon;
        public bool needWalkPathCheck = false;
        //public SoldierWalkingPath walkingPath = null;

        public Vector3 goalWp;

        public IntVector2 tilePos;
        public IntVector2 armyLocalPlacement = IntVector2.Zero;
        
        public int groupId;

        public UnitType type;
        //public int projectileHits = 0;
        //public bool isShip = false;
        //public ShipTransform inShipTransform = null;
        public bool lockMovement=false;
        public Rotation1D rotation;

        public float terrainSpeedMultiplier = 1.0f;
        public float walkSpeed = AbsDetailUnitData.StandardWalkingSpeed;
        float rotateSpeed;

        public AbsGroup attacking_soldierGroupOrCity = null;
        public AggressionCommand aggression = AggressionCommand.Normal;


        //bool armyLocalPlacementInitialized = false;
        public const int LifeState_New = 0;
        public const int LifeState_GainedLocalPositon = 1;
        public const int LifeState_GainedYpos = 2;

        public int lifeState = LifeState_New;

        public int groupObjective = GroupObjective_FollowArmyObjective;
        public bool attackState = false;
        bool isRecruit;
        public bool inShipTransform = false;
        
        

        public SoldierGroup(Army army, UnitType type, bool recruit)
        {
            this.type = type;
            this.isRecruit = recruit;
            tilePos = army.tilePos;

            this.groupId = DssRef.state.nextGroupId++;
            this.army = army;

            AbsSoldierData typeData = DssRef.unitsdata.Get(type);
            
            recruit &= typeData.recruitTrainingTimeSec > 0;
            
            initPart1(typeData);

            //Column for column spawning
            int count = typeData.rowWidth * typeData.columnsDepth;
            createAllSoldiers(recruit, typeData, count);

            if (recruit)
            {                
                new TrainingCompleteTimer(this);
            }

            initPart2(typeData);

            if (army.faction.player.IsPlayer())
            {
                army.faction.player.GetLocalPlayer().statistics.SoldiersRecruited += count;
            }
        }

        void initPart1(AbsSoldierData typeData)
        {
            int count = typeData.rowWidth * typeData.columnsDepth;

            soldiers = new SpottedArray<AbsSoldierUnit>(count);
            halfColDepth = typeData.columnsDepth * -0.5f;

            Vector2 boxSz = new Vector2(
                (typeData.rowWidth - 1) * typeData.groupSpacing,
                (typeData.columnsDepth - 1) * typeData.groupSpacing);
            float radius = boxSz.Length() * 0.5f;

            skirmishCount = MathExt.MultiplyInt(0.3, count);

            groupRadius = radius;
        }

        void initPart2(AbsSoldierData typeData)
        { 
            refreshAttackRadius(typeData);
            refreshRotateSpeed();            

            army.AddSoldierGroup(this);
            rotation = army.rotation;
        }

        public SoldierGroup(Army army, System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
        {
            this.army = army;
            readGameState(r, version, pointers);            
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)type);
            w.Write((byte)FirstSoldierData().unitType);

            armyLocalPlacement.writeShort(w);

            bool lockedInArmyGrid = army.IdleObjetive() && groupObjective == GroupObjective_FollowArmyObjective;
            w.Write(lockedInArmyGrid);

            if (!lockedInArmyGrid)
            {
                WP.writePosXZ(w, position);
                w.Write(rotation.ByteDir);
            }

            w.Write((byte)groupObjective);

            w.Write((byte)soldiers.Count);
            bool soldiersLockedInGroup = groupObjective == GroupObjective_FollowArmyObjective;
            
            if (!soldiersLockedInGroup)
            {
                var soldiersC = soldiers.counter();
                while (soldiersC.Next())
                { 
                    soldiersC.sel.writeGameState(w);
                }
            }
        }
        public void readGameState(System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
        {
            type = (UnitType)r.ReadByte();
            UnitType soldierType = (UnitType)r.ReadByte();

            armyLocalPlacement.readShort(r);

            bool lockedInArmyGrid = r.ReadBoolean();

            if (!lockedInArmyGrid)
            {
                WP.readPosXZ(r,out position, out tilePos);
                rotation.ByteDir = r.ReadByte();
            }

            groupObjective = r.ReadByte();

            int soldiersCount = r.ReadByte();
            bool soldiersLockedInGroup = groupObjective == GroupObjective_FollowArmyObjective;

            AbsSoldierData typeData = DssRef.unitsdata.Get(type);
            initPart1(typeData);

            AbsSoldierData soldierData = DssRef.unitsdata.Get(soldierType);

            createAllSoldiers(soldierType == UnitType.Recruit , (soldierData != null && soldierData.IsShip())? soldierData : typeData, soldiersCount);

            if (!soldiersLockedInGroup)
            {
                var soldiersC = soldiers.counter();
                while (soldiersC.Next())
                {
                    soldiersC.sel.readGameState(r, version);
                }
            }

            initPart2(typeData);
        }

        public void writeNet(System.IO.BinaryWriter w)
        {

        }
        public void readNet(System.IO.BinaryReader r)
        {

        }

        void refreshAttackRadius(AbsSoldierData typeData)
        {
            if (typeData.bonusProjectiles > 0)
            {
                attackRadius = groupRadius + typeData.secondaryAttackRange;
            }
            else
            {
                attackRadius = groupRadius + typeData.attackRange;
                //if (typeData.mainAttack == AttackType.Melee)
                //{
                //    attackRadius *= 0.75f;
                //}
            }
        }

        private void createAllSoldiers(bool recruit, AbsSoldierData typeData, int count)
        {
            int xStart = -typeData.rowWidth / 2;
            IntVector2 bannerPos = bannerManPos();

            int columnDepth = MathExt.Div_Ceiling(count, typeData.rowWidth);

            for (int x = 0; x < typeData.rowWidth; ++x)
            {
                int leadUnit = -1;
                for (int y = 0; y < columnDepth; ++y)
                {
                    AbsSoldierUnit unit;
                    if (bannerPos.Equals(x, y))
                    {
                        unit = createUnit(UnitType.BannerMan,
                            false,
                            new IntVector2(x + xStart, y), tilePos);
                    }
                    else
                    {
                        unit = createUnit(typeData.unitType,
                          recruit,
                          new IntVector2(x + xStart, y), tilePos);
                    }


                    unit.following = leadUnit;
                    leadUnit = unit.parentArrayIndex;

                    if (--count <= 0)
                    {
                        return;
                    }
                }
            }
        }

        void refreshRotateSpeed()
        {
            rotateSpeed = (float)Math.Abs(Math.Atan2(walkSpeed, groupRadius));
        }

        IntVector2 bannerManPos()
        {
            var typeData = DssRef.unitsdata.Get(type);

            IntVector2 bannerPos;
            if (typeData.hasBannerMan)
            {
                bannerPos = new IntVector2(typeData.rowWidth / 2, typeData.columnsDepth - 1);
            }
            else
            {
                bannerPos = IntVector2.NegativeOne;
            }

            return bannerPos;
        }

        public AbsSoldierData SoldierData()
        {
            return DssRef.unitsdata.Get(type);
        }

        public void completeTransform(SoldierTransformType transformType)
        {
            if (isDeleted) return;

            
            var soldiersC = soldiers.counter();

            if (transformType == SoldierTransformType.TraningComplete)
            {
                isRecruit = false;
                UnitType toType = IsShip() ? SoldierData().convertSoldierShipType : type;

                while (soldiersC.Next())
                {
                    var oldUnit = soldiersC.sel;

                    if (oldUnit.DetailUnitType() != UnitType.BannerMan)
                    {
                        AbsSoldierUnit upgradedUnit;

                        upgradedUnit = DssRef.unitsdata.createSoldier(toType, false);

                        upgradedUnit.initUpgrade(this);

                        oldUnit.copyDataToUpgradedUnit(upgradedUnit);

                        soldiers.Array[oldUnit.parentArrayIndex] = upgradedUnit;

                        oldUnit.DeleteMe(DeleteReason.Transform, false);

                        if (army.inRender)
                        {
                            upgradedUnit.setDetailLevel(true);
                            upgradedUnit.update(1f, true);
                        }
                    }
                }

            }
            else
            {
                if (IsShip() != (transformType == SoldierTransformType.ToShip))
                {
                    int totalHealth = 0;

                    while (soldiersC.Next())
                    {
                        totalHealth += soldiersC.sel.health;
                        soldiersC.sel.DeleteMe(DeleteReason.Transform, false);
                    }
                    soldiers.Clear();

                    AbsSoldierData typeData;

                    if (transformType == SoldierTransformType.ToShip)
                    {
                        UnitType toType = isRecruit ? UnitType.RecruitWarship : SoldierData().convertSoldierShipType;
                        var ship = createUnit(toType, false, IntVector2.Zero, WP.ToTilePos(position));
                        typeData = ship.data;
                        ship.health = totalHealth;
                        ship.refreshShipCarryCount();
                    }
                    else
                    {
                        typeData = SoldierData();
                        int count = totalHealth / typeData.basehealth;

                        createAllSoldiers(false, typeData, count);
                    }

                    refreshAttackRadius(typeData);
                }

                inShipTransform = false;
            }
        }

        //public void completeTraining()
        //{
        //    if (isDeleted) return;

        //    var soldiersC = soldiers.counter();
        //    while (soldiersC.Next())
        //    {
        //        var oldUnit = soldiersC.sel;

        //        if (oldUnit.DetailUnitType() != UnitType.BannerMan)
        //        {
        //            var upgradedUnit = DssRef.unitsdata.createSoldier(type, false);
        //            upgradedUnit.initUpgrade(this);

        //            oldUnit.copyDataToUpgradedUnit(upgradedUnit);

        //            soldiers.Array[oldUnit.parentArrayIndex] = upgradedUnit;

        //            oldUnit.DeleteMe(DeleteReason.Transform, false);

        //            if (army.inRender)
        //            {
        //                upgradedUnit.setDetailLevel(true);
        //                upgradedUnit.update(1f, true);
        //            }
        //        }
        //    }
        //}

        //public void completeShipTransform(bool toShip)
        //{
        //    if (!isDeleted)
        //    {
        //        isShip = toShip;
        //        var counter = soldiers.counter();
        //        while (counter.Next())
        //        {
        //            counter.sel.lockMovement = false;
        //            counter.sel.model?.Adv().setShip(toShip);
        //        }
        //    }
        //    //refreshWalkSpeed();
        //}

        public Vector3 armyPlacement(Vector3 center)
        {
            Vector2 offset = VectorExt.RotateVector(new Vector2(
                armyLocalPlacement.X * GroupSpacing,
                armyLocalPlacement.Y * GroupSpacing),
                army.rotation.radians);

            center.X += offset.X;
            center.Z += offset.Y;

            DssRef.world.unitBounds.KeepPointInsideBound_Position(ref center.X, ref center.Z);

            return center;
        }

        public AbsSoldierUnit createUnit(UnitType type, bool recruit, IntVector2 gridPlacement, IntVector2 area)
        {
            var s = DssRef.unitsdata.createSoldier(type, recruit);
            s.InitLocal(position, gridPlacement, area, this);
            s.position = WP.ToWorldPos(area); //temp pos
            s.parentArrayIndex = soldiers.Add(s);

            if (army.inRender)
            {
                s.setDetailLevel(true);
                s.update(1f, true);
            }
            return s;
        }

        public void update(float time, bool fullUpdate)
        {
            //if (army.id == 1)
            //{
            //    lib.DoNothing();
            //}
            if (debugTagged)//groupId == 1611)
            {
                lib.DoNothing();
            }


            if (soldiers.Count > 0)
            {
                if (!lockMovement)
                {
                    var battleGroup_sp = army.battleGroup;
                    if (battleGroup_sp != null &&
                        battleGroup_sp.battleState != Battle.BattleState.Battle)
                    {
                        update_battlePreparations(time, fullUpdate);
                        return;
                    }


                    //UPDATE OBJECTIVE
                    {
                        bool newIdleGroup = false;

                        int newObjective = groupObjective;
                        bool newAttackState = false;
                        bool induvidualUpdate;
                        bool walking = false;
                        if (groupObjective == GroupObjective_IsSplit)
                        {
                            induvidualUpdate = true;

                            if (//attacking_soldierGroupOrCity == null &&
                                army.battleGroup == null)//dont regroup in battle (start spinning)
                            {
                                refreshGroupPositions();

                                if (fullUpdate)
                                {
                                    var soldiersC = soldiers.counter();
                                    while (soldiersC.Next())
                                    {
                                        soldiersC.sel.setReGroupState();
                                    }

                                    newObjective = GroupObjective_ReGrouping;
                                }
                                else
                                {
                                    induvidualUpdate = false;
                                    newObjective = GroupObjective_FindArmyPlacement;
                                }
                            }
                        }
                        else if (groupObjective == GroupObjective_ReGrouping)
                        {
                            if (fullUpdate)
                            {
                                induvidualUpdate = true;

                                if (allInduvidualsAreIdle)
                                {
                                    //LOCK in place
                                    newObjective = GroupObjective_FindArmyPlacement;

                                    var soldiersC = soldiers.counter();
                                    while (soldiersC.Next())
                                    {
                                        soldiersC.sel.clearAttack();
                                        soldiersC.sel.aiState = SoldierAiState.GroupLock;
                                    }
                                }
                            }
                            else
                            {
                                induvidualUpdate = false;
                                newObjective = GroupObjective_FindArmyPlacement;
                            }
                        }
                        else
                        { //Moving like a group

                            induvidualUpdate = false;
                            var closest_sp = attacking_soldierGroupOrCity;
                            if (closest_sp != null)
                            {
                                if (groupCollisionDistance(closest_sp) < 0.02f)
                                {
                                    //SPLIT GROUP
                                    newObjective = GroupObjective_IsSplit;
                                    newAttackState = true;

                                    var soldiersC = soldiers.counter();
                                    while (soldiersC.Next())
                                    {
                                        soldiersC.sel.setAttackState();
                                    }
                                }
                                else
                                {
                                    //Group attack move
                                    walking = !updateWalking(goalWp, false, Rotation1D.D0, time);
                                    //walking = !updateWalking(closest_sp.position, true, army.rotation, time);
                                }
                            }
                            else if (army.battleGroup != null)
                            {
                                walking = !updateWalking(goalWp, false, Rotation1D.D0, time);
                            }
                            else if (groupObjective == GroupObjective_FindArmyPlacement)
                            {
                                if (updateWalking(goalWp, true, army.rotation, time))
                                {
                                    newObjective = GroupObjective_FollowArmyObjective;
                                    newIdleGroup = true;
                                }
                                else
                                {
                                    walking = true;
                                }
                            }
                            else
                            {
                                if (army.objective == ArmyObjective.MoveTo ||
                                    army.objective == ArmyObjective.Attack)
                                {
                                    if (updateWalking(goalWp, true, army.rotation, time))
                                    {
                                        newIdleGroup = true;
                                    }
                                    else
                                    {
                                        walking = true;
                                    }
                                    newObjective = GroupObjective_FindArmyPlacement;
                                }
                                else
                                {
                                    lib.DoNothing();
                                }
                                //Follow army

                            }

                        }

                        if (induvidualUpdate)
                        {
                            var soldiersC = soldiers.counter();
                            while (soldiersC.Next())
                            {
                                soldiersC.sel.update(time, fullUpdate);
                            }
                        }
                        else
                        {
                            var soldiersC = soldiers.counter();
                            while (soldiersC.Next())
                            {
                                soldiersC.sel.update_GroupLocked(walking);
                            }
                        }

                        groupIsIdle = newIdleGroup;

                        if (newObjective != groupObjective)
                        {
                            groupObjective = newObjective;
                        }
                        attackState = newAttackState;
                    }
                }

            }
            else
            {
                if (!inShipTransform)
                {
                    if (fullUpdate)
                    {
                        DeleteMe(DeleteReason.EmptyGroup, true);
                    }
                    else
                    {
                        Ref.update.AddSyncAction(new SyncAction2Arg<DeleteReason, bool>(DeleteMe, DeleteReason.EmptyGroup, true));
                    }
                }
            }
        }


        public override void toHud(ObjectHudArgs args)
        {
            //base.toHud(args);
            var typeData = DssRef.unitsdata.Get(type);

            args.content.h2(typeData.unitType.ToString() + " " + DssRef.lang.UnitType_SoldierGroup);
            args.content.newLine();
            if (args.selected && GetFaction() == args.player.faction)
            {
                new Display.GroupMenu(args.player, this, args.content);
            }
        }

        public bool soldiersShouldFollowWalkingOrder()
        {
            return groupObjective == GroupObjective_FollowArmyObjective;//hasWalkingOrder && attacking.Count == 0;
        }

        public void EnterPeaceEvent()
        {
            attacking_soldierGroupOrCity = null;
            groupObjective = GroupObjective_IsSplit;
        }

        void refreshGroupPositions()
        {
            attacking_soldierGroupOrCity = null;

            //Refresh placements
            {
                IntVector2 bannerPos = bannerManPos();
                AbsSoldierUnit bannerMan = null;
                var typeData = DssRef.unitsdata.Get(type);

                IntVector2 nextPos = IntVector2.Zero;
                int bannerLead = -1;
                int xStart = -typeData.rowWidth / 2;

                var soldiersC = soldiers.counter();
                AbsSoldierUnit[] leadRow = new AbsSoldierUnit[typeData.rowWidth];

                while (soldiersC.Next())
                {
                    soldiersC.sel.clearAttack();
                    if (soldiersC.sel.DetailUnitType() == UnitType.BannerMan)
                    {
                        bannerMan = soldiersC.sel;
                    }
                    else
                    {
                        soldiersC.sel.gridPlacement.X = nextPos.X + xStart;
                        soldiersC.sel.gridPlacement.Y = nextPos.Y;
                        soldiersC.sel.refreshGroupOffset();

                        if (nextPos.Y > 0)
                        {
                            soldiersC.sel.following = leadRow[nextPos.X].parentArrayIndex;
                        }
                        leadRow[nextPos.X] = soldiersC.sel;

                        if (nextPos.X == bannerPos.X)
                        {
                            bannerLead = soldiersC.sel.parentArrayIndex;
                        }

                        do
                        {
                            if (++nextPos.X >= typeData.rowWidth)
                            {
                                nextPos.X = 0;
                                nextPos.Y++;
                            }
                        }
                        while (nextPos == bannerPos);
                    }
                }

                if (bannerMan != null)
                {
                    bannerMan.following = bannerLead;
                    if (bannerLead == -1)
                    {
                        bannerMan.gridPlacement.Y = 0;
                    }
                    else
                    {
                        bannerMan.gridPlacement = soldiers.Array[bannerLead].gridPlacement;
                        bannerMan.gridPlacement.Y++;
                    }

                    bannerMan.refreshGroupOffset();
                }
            }

            //if (fullUpdate)
            //{
            //    //if (!hasWalkingOrder)
            //    //{
            //    //    armyPathGoal = armyPlacement(WP.ToWorldPos(army.tilePos));

            //    //    position = armyPathGoal;
            //    //    rotation = army.rotation;
            //    //    hasWalkingOrder = true;
            //    //    walkingOrderTo = army.tilePos;
            //    //}

                
            //}
            //else
            //{
            //    setGroupLock();
            //}
        }

        //void setGroupLock()
        //{
        //    //isSplit = false;
        //    setObjective(GroupObjective.FindArmyPlacement);

        //    var soldiersC = soldiers.counter();
        //    while (soldiersC.Next())
        //    {
        //        soldiersC.sel.clearAttack();
        //        soldiersC.sel.aiState = SoldierAiState.GroupLock;
        //    }
        //}


        bool updateWalking(Vector3 walkTowards, bool rotate, Rotation1D finalRotation, float time)
        {
            Vector2 diff = new Vector2(
                walkTowards.X - position.X,
                walkTowards.Z - position.Z);

            float speed = walkSpeed * terrainSpeedMultiplier * time;

            if (diff.Length() > speed)
            {
                Rotation1D dir = Rotation1D.FromDirection(diff);
                if (rotateTowardsAngle(dir, time))
                {
                    Vector2 move = VectorExt.SetLength(diff, speed);
                    position.X += move.X;
                    position.Z += move.Y;
                }
            }
            else if (rotate)
            {
                //final adjust when reached goal
                if (rotateTowardsAngle(finalRotation, time))
                {
                    //hasWalkingOrder = false;
                    return true;
                }
            }
            else
            { return true; }

            return false;            
        }

        bool rotateTowardsAngle(Rotation1D goalDir, float time)
        {
            float adiff = rotation.AngleDifference(goalDir.radians);
            float abs_adiff = Math.Abs(adiff);

            float angleAdd = rotateSpeed * time;
            if (abs_adiff <= angleAdd)
            {
                rotation = goalDir;
                return true;
            }
            else
            {//Rotate group
                rotation.Add(angleAdd * lib.ToLeftRight(adiff));
                return false;
            }
        }

        //void updateArmyWalkingGoal(out Vector3 walkTowards)
        //{
        //    walkTowards = armyPathGoal;

        //}

        //bool updateAttackGoal(out Vector3 walkTowards)
        //{
        //    var closest_sp = closestOpponent; //Safe pointer
        //    if (closest_sp != null)
        //    {
        //        walkTowards = closestOpponent.position;
        //        return true;
        //    }

        //    walkTowards = Vector3.Zero;
        //    return false;
        //}

        bool updateWalkingGoal(out Vector3 walkTowards)
        {
            //const float WarDeclareDistance = 0.2f;

            //bool warDeclared = attacking!= null;

            //var closest_sp = attacking;//closestOpponent; //Safe pointer
            //if (closest_sp != null)
            //{
            //    float dist = groupCollisionDistance(closest_sp);

            //    float warDist = 0.2f;
            //    if (army.ai.objective == ArmyObjective.Attack &&
            //        army.ai.attackTarget.faction == closest_sp.Faction())
            //    {
            //        warDist = 1.5f;
            //    }
            //    //freeAttackAggression = true;
            //    //}

            //    warDeclared = dist <= warDist;
            //}

            var attack_sp = attacking_soldierGroupOrCity;

            bool attackAggression= attack_sp != null;

            //if (army.ai.objective == ArmyObjective.None)
            //{
            //    attackAggression = warDeclared || attacking.Count > 0 || closestFriendInBattle != null;
            //}
            //else
            //{
            //    //bool reachedGoal = army.ai.walkGoal.SideLength(tilePos)
            //    attackAggression = attacking.Count > 0;                
            //}

            if (aggression == AggressionCommand.Hold)
            {
                attackAggression = false;
            }

            if (attackAggression)
            {
                //var closest_sp = closestOpponent; //Safe pointer
                //if (closest_sp != null)
                //{
                    walkTowards = attack_sp.position;
                    return true;
                //}

                //var friendly_sp = closestFriendInBattle;
                //if (friendly_sp != null)
                //{
                //    walkTowards = friendly_sp.position;
                //    return true;
                //}

            }
            walkTowards = goalWp;
            return true;//hasWalkingOrder;
        }



        //float groupCollisionCheck(AbsGroup group)
        //{
        //    float distance = Physics.PhysicsLib2D.CirkleDistance(
        //        position, spotEnemyRadius,
        //        group.position, group.groupRadius);
        //    //if (Physics.PhysicsLib2D.CirkleCollision(
        //    //        position, attackRadius,
        //    //        group.position, group.groupRadius))
        //    if (distance <= 0)
        //    {
        //        addAttackTarget(group);
        //    }

        //    return distance;
        //}

        float groupCollisionDistance(AbsGroup group)
        {
            float distance = Physics.PhysicsLib2D.CirkleDistance(
                position, attackRadius,
                group.position, group.groupRadius);
            
            return distance;
        }

        public float distance(AbsGroup group)
        {
            return VectorExt.Length(group.position.X - position.X, group.position.Z - position.Z);
        }

        public void addAttackTarget(AbsGroup newTarget)
        {
            refreshAttacking();

            if (!newTarget.defeatedBy(army.faction) && newTarget != attacking_soldierGroupOrCity)
            {
                if (attacking_soldierGroupOrCity != null)
                {   
                    //Compare distance
                    if (distanceValueTo(attacking_soldierGroupOrCity) <= distanceValueTo(newTarget))
                    {
                        return;
                    }
                }

                attacking_soldierGroupOrCity = newTarget;

            }
        }

        void refreshAttacking()
        {
            if (attacking_soldierGroupOrCity != null && 
                (attacking_soldierGroupOrCity.defeated() || !DssRef.diplomacy.InWar(army.faction, attacking_soldierGroupOrCity.GetFaction())))
            {
                attacking_soldierGroupOrCity = null;
            }
        }


        public void asynchNearObjectsUpdate()
        {
            if (debugTagged)
            {
                lib.DoNothing();
            }

            List<GameObject.AbsGroup> opponents, friendly;

            DssRef.world.unitCollAreaGrid.collectOpponentsAndFriendlies(army.faction, tilePos, out opponents, out friendly);

            refreshAttacking();

            if (attacking_soldierGroupOrCity == null && army.battles.Count > 0)
            {               
                //Version 2: förlitar sig på att mapobject.battles 
                AbsGroup nearest = null;
                float distanceValue = float.MaxValue;
                var battlesC = army.battles.counter();
                while (battlesC.Next())
                {
                    var type = battlesC.sel.gameobjectType();
                    if (type == GameObjectType.Army)
                    {
                        var groupsC = battlesC.sel.GetArmy().groups.counter();
                        while (groupsC.Next())
                        {
                            if (groupsC.sel.soldiers.Count > 0)
                            {
                                var dist = distanceValueTo(groupsC.sel);

                                if (dist < distanceValue)
                                {
                                    distanceValue = dist;
                                    nearest = groupsC.sel;
                                }
                            }
                        }
                    }
                    else if (type == GameObjectType.City)
                    {
                        if (battlesC.sel.GetCity().guardCount > 0)
                        {
                            var dist = distanceValueTo(battlesC.sel);
                            if (dist < distanceValue)
                            {
                                distanceValue = dist;
                                nearest = battlesC.sel;
                            }
                        }
                    }
                }

                //closestOpponent = nearest;
                if (nearest != null)
                {
                    addAttackTarget(nearest);
                }
            }
           
        }

        float distanceValueTo(AbsGroup toGroup)
        {
            const float AngleDiffValue = 0.5f;

            Vector2 diff = new Vector2(toGroup.position.X - this.position.X, toGroup.position.Z - this.position.Z);

            float l = diff.Length();

            float dir = lib.V2ToAngle(diff);
            float aDiff =Rotation1D.AngleDifference_Absolute(rotation.radians, dir);
            float anglePercDiff = Math.Abs(aDiff) / MathExt.TauOver2;

            float value = l + l * anglePercDiff * AngleDiffValue;

            return value;
        }

        public void asynchUpdate()
        {
            int soldiersWithWalkingOrder = 0;
            int idleSoldiers = 0;
            int soldierCount = 0;


            Vector3 sumPos = Vector3.Zero;
            {
                var counter = soldiers.counter();
                while (counter.Next())
                {
                    var soldier = counter.sel;
                    soldier.asynchUpdate();

                    sumPos += soldier.position;
                    soldierCount++;

                    if (soldier.hasWalkingOrder)
                    {
                        soldiersWithWalkingOrder++;
                    }
                    if (soldier.state.idle)
                    {
                        idleSoldiers++;
                    }
                }
            }


            if (soldierCount == 0)
            {
                //groupObjective = GroupObjective.Destroyed;
                return;
            }

            //updateObjective();

            if (groupObjective == GroupObjective_IsSplit || position.X<=0)
            {
                position = sumPos / soldierCount;
            }

            tilePos = WP.ToTilePos(position);

            Map.Tile tile;
            if ( DssRef.world.tileGrid.TryGet(tilePos, out tile))
            {
                terrainSpeedMultiplier = tile.TerrainSpeedMultiplier(IsShip()) * 0.4f + army.terrainSpeedMultiplier * 0.6f;
                position.Y = tile.UnitGroundY();
            }
            allInduvidualsAreIdle = idleSoldiers == soldierCount;
        }

        void setObjective(int objective)
        {
            if (objective != groupObjective)
            {
                if (debugTagged)
                {
                    Debug.Log("New Objective (" + TypeName() + "): " +
                        groupObjective.ToString() + " > " + objective.ToString());
                }

                groupObjective = objective;
            }
        }

        public void DrawOverviewIcon(int cameraIndex)
        {

        }

        public void setWalkNode(IntVector2 area,
            bool nextIsFootTransform, bool nextIsShipTransform)
        {
            if (groupId== 5152)
            {
                lib.DoNothing();
            }

            walkingOrderTo = area;
            Vector3 areaCenter = WP.ToWorldPos(area);
            goalWp = armyPlacement(areaCenter);

            if ((nextIsFootTransform && IsShip()) ||
                (nextIsShipTransform && !IsShip()))
            {
                if (!inShipTransform)
                {
                    inShipTransform = true;
                    new ShipTransform(this, false);
                }
            }

        }

        public void bumpWalkToNode(IntVector2 nodePos)
        {
            walkingOrderTo = nodePos;
            Vector3 areaCenter = WP.ToWorldPos(walkingOrderTo);
            goalWp = armyPlacement(areaCenter);
        }

        public void setGroundY()
        {
            var counter = soldiers.counter();
            while (counter.Next())
            {
                counter.sel.firstUpdate();
            }
            ++lifeState;
        }

        //public void OrderHalt()
        //{
        //    //walkingPath = null;
        //    //hasWalkingOrder = false;
        //    //walkingOrderTo = IntVector2.NegativeOne;
        //    setObjective(GroupObjective.Idle);
        //}

        //public bool canOrderWalkTo(IntVector2 area)
        //{
        //    if (hasWalkingOrder && walkingOrderTo == area)
        //    {
        //        return false;
        //    }

        //    if (soldiers.Count > 0)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        public void remove(AbsSoldierUnit soldier)
        {
            Debug.CrashIfThreaded();

            soldiers.RemoveAt_EqualSafeCheck(soldier, soldier.parentArrayIndex);
            if (soldiers.Count == skirmishCount)
            {//GO SKIRMISH
                var soldiersC = soldiers.counter();
                while (soldiersC.Next())
                {
                    soldiersC.sel.setFreeAttack();
                }
            }
            else if (soldiers.Count == 0)
            {
                //army.remove(this);
                //isDeleted = true;
                DeleteMe(DeleteReason.EmptyGroup, true);
                //deleteGroup(false);
            }
        }

       

        public void onDisband(bool deserter)
        {
            //Immigrate to closest city
            var closestCity = DssRef.world.unitCollAreaGrid.closestCity(tilePos);
            Vector2 dir;
            RotationQuarterion rot;
            if (closestCity != null)
            {
                double dist = WP.birdDistance(closestCity, tilePos);
                double keep = lib.ValueOnPercentScale(12, 3, dist, true) * 0.7 + 0.1;

                if (deserter && closestCity.faction == this.GetFaction())
                { 
                    keep *= 0.6;
                }
                
                double immigrants = soldiers.Count * keep;

                closestCity.immigrants.value += immigrants;

                dir = VectorExt.SafeNormalizeV2(VectorExt.V3XZtoV2(closestCity.position - position));
                rot = WP.ToQuaterion(lib.V2ToAngle(-dir));
            }
            else
            {
                rotation.flip180();
                dir = rotation.Direction(1f);
                rot = WP.ToQuaterion(rotation.radians);
            }

            if (army.inRender)
            {
                Vector3 moveDir_dir = VectorExt.V2toV3XZ(dir);

                var soldiersC = soldiers.counter();
                while (soldiersC.Next())
                {
                    new DeserterAnimation(soldiersC.sel, moveDir_dir, rot);
                }
            }
        }


        //public bool inBattle()
        //{
        //    var soldiersC = soldiers.counter();
        //    while (soldiersC.Next())
        //    {
        //        if (soldiersC.sel.attackTarget != null)
        //            return true;
        //    }

        //    return false;
        //}

        public float strengthValue()
        {
            float result = 0;

            var soldiersC = soldiers.counter();
            while (soldiersC.Next())
            {
                result += soldiersC.sel.DPS();
            }

            return result;
        }

        public AbsSoldierUnit FirstSoldier()
        {
            return soldiers.First();
        }

        public override void AddDebugTag()
        {
            base.AddDebugTag();
            army.AddDebugTag();
        }

        override public bool isMelee()
        {
            return DssRef.unitsdata.Get(type).mainAttack == AttackType.Melee;
        }

        public bool ScoutMovement()
        {
            AbsSoldierUnit s = FirstSoldier();
            if (s == null)
            {
                return false;
            }
            return s.data.scoutMovement;
        }

        //public IntVector2 targetArea()
        //{
        //    if (groupObjective == GroupObjective_FollowArmyObjective)
        //    {
        //        return walkingOrderTo;
        //    }
        //    else
        //    {
        //        return tilePos;
        //    }
        //}

        public void onNewModel(LootFest.VoxelModelName name, Graphics.VoxelModel master)
        {
            var counter = soldiers.counter();
            while (counter.Next())
            {
                counter.sel.onNewModel(name, master);
            }
        }

        public void Upkeep(ref float food)
        {
            var typeData = DssRef.unitsdata.Get(type);
            food += typeData.foodPerSoldier * soldiers.Count;
        }

        public override void DeleteMe(DeleteReason reason, bool removeFromParent)
        {
            isDeleted = true;

            if (reason == DeleteReason.Disband)
            {
                onDisband(false);
            }
            else if (reason == DeleteReason.Desert)
            {
                onDisband(true);
            }

            if (soldiers.Count > 0)
            {
                var soldiersC = soldiers.counter();
                while (soldiersC.Next())
                {
                    soldiersC.sel.DeleteMe(reason, false);
                }
            }

            if (removeFromParent)
            {
                army.remove(this);
            }
        }

        //public override void DeleteMe(bool netShare)
        //{
        //    base.DeleteMe(netShare);
        //    var soldiersC = soldiers.counter();
        //    while (soldiersC.Next())
        //    {
        //        soldiersC.sel.DeleteMe(false);
        //    }

        //    soldiers.Clear();
        //}

        //public void deleteGroup(bool quickDelete)
        //{
        //    Debug.CrashIfThreaded();

        //    if (soldiers.Count > 0)
        //    {
        //        var soldiersC = soldiers.counter();
        //        while (soldiersC.Next())
        //        {
        //            soldiersC.sel.DeleteMe(true);
        //        }
        //    }

        //    if (!quickDelete)
        //    {
        //        army.remove(this);
        //        isDeleted = true;
        //        //groupObjective = GroupObjective.Destroyed;
        //    }
        //}

        public override bool defeatedBy(Faction attacker)
        {
            return soldiers.Count == 0;
        }

        public override bool aliveAndBelongTo(Faction faction)
        {
            return soldiers.Count > 0;
        }

        public bool canMoveTo(IntVector2 from, IntVector2 to)
        {
            return true;
        }

        public override SoldierGroup GetGroup()
        {
            return this;
        }

        public override GameObjectType gameobjectType()
        {
            return GameObject.GameObjectType.SoldierGroup;
        }

        public override void stateDebugText(RichBoxContent content)
        {
            content.newLine();
            content.text("Objective: " + groupObjective.ToString());

            if (attacking_soldierGroupOrCity!=null)
            {
                //var c = attacking.counter();
                //while (c.Next())
                //{
                    content.text("attacking: " + attacking_soldierGroupOrCity.TypeName());
                //}
            }
            else
            {
                content.text("attacking: None");
            }

            content.Add(new RichBoxNewLine(true));
            content.text(army.TypeName());
            army.stateDebugText(content);
        }

        public void SetArmyPlacement(IntVector2 newLocalPlacement, bool onPurchase)
        {
            if (armyLocalPlacement != newLocalPlacement ||  onPurchase)
            {

                armyLocalPlacement = newLocalPlacement;
                goalWp = armyPlacement(army.position);

                //if (currentArmyPosition.X < 2 && currentArmyPosition.Z < 2)
                //{
                //    lib.DoNothing();
                //}

                if (!army.inRender || lifeState == LifeState_New)
                {
                    ++lifeState;
                    position = goalWp;
                    setGroundY();
                }
                else if (groupObjective == GroupObjective_FollowArmyObjective)
                {
                    groupObjective = GroupObjective_FindArmyPlacement;
                }
            }            
        }

        public override Faction GetFaction()
        {
            return army.faction;
        }

        public override AbsMapObject RelatedMapObject()
        {
            return army;
        }


        public bool IsShip()
        { 
            var  first = soldiers.First();
            if (first != null)
            { 
                return first.IsShipType();
            }

            return false;
        }

        public AbsSoldierData FirstSoldierData()
        {
            var first = soldiers.First();
            if (first != null)
            {
                return first.data;
            }

            return DssRef.unitsdata.Get(type);
        }

        public override string TypeName()
        {
            return type.ToString() + " Group(" + groupId.ToString() + ")";
        }

        public override string ToString()
        {
            return "Group " + type.ToString() + " x" + soldiers.Count.ToString() + ", id" + groupId.ToString();
        }
    }    

    enum AggressionCommand
    {
        Hold,
        Normal,
    }

    //enum GroupObjective
    //{
    //    FollowArmyObjective,

    //    IsSplit,
    //    ReGrouping,
    //    FindArmyPlacement,
    //}
}
