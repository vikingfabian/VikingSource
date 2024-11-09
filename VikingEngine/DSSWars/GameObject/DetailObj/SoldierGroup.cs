﻿//#define VISUAL_NODES

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.PJ.CarBall;
using VikingEngine.ToGG.MoonFall;
using VikingEngine.ToGG.MoonFall.GO;
using VikingEngine.ToGG.ToggEngine.Map;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

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
        public static Physics.CircleBound GroupBound, OtherBound;
        public static Physics.CircleBound WalkDirBound;
        static float WalkDirCheckLength;
        public static void Init()
        { 
            GroupBound = new Physics.CircleBound(DssVar.SoldierGroup_CollisionRadius);
            OtherBound = new Physics.CircleBound(DssVar.SoldierGroup_CollisionRadius);
            WalkDirBound = new Physics.CircleBound(DssVar.SoldierGroup_MoveCollisionRadius);
            WalkDirCheckLength = DssVar.SoldierGroup_CollisionRadius * 0.9f;
        }

        public const int GroupObjective_FollowArmyObjective = 0;
        public const int GroupObjective_IsSplit = 1;
        public const int GroupObjective_ReGrouping = 2;
        public const int GroupObjective_FindArmyPlacement = 3;

        bool isWalkingIntoOtherGroup = false;

        int skirmishCount;

        public float halfColDepth;

        public int soldierCount = 0;
        int shipHealth;
        public SpottedArray<AbsSoldierUnit> soldiers = null;

        public Army army;
        
        public bool groupIsIdle = false;
        public bool allInduvidualsAreIdle = false;
        //public SpriteName icon;
        public bool needWalkPathCheck = false;
        //public SoldierWalkingPath walkingPath = null;

        public Vector3 goalWp;
        
        int followsGoalId = int.MinValue;

        public IntVector2 tilePos;

        //center, left, right / scout, front, second, behind
        public IntVector2 armyLocalPlacement = IntVector2.Zero;
        public IntVector2 armyGridPlacement2 = IntVector2.Zero;
        
        public UnitType type;
        public bool lockMovement = false;
        public Rotation1D rotation;

        public float terrainSpeedMultiplier = 1.0f;
        public float walkSpeed = DssConst.Men_StandardWalkingSpeed;
        float rotateSpeed;

        public AbsGroup attacking_soldierGroupOrCity = null;
        public AggressionCommand aggression = AggressionCommand.Normal;


        //bool armyLocalPlacementInitialized = false;
        public const int LifeState_New = 0;
        public const int LifeState_GainedLocalPositon = 1;
        public const int LifeState_GainedYpos = 2;

        public int lifeState = LifeState_New;

        public GroupState state = GroupState.Idle;
        public int groupObjective = GroupObjective_FollowArmyObjective;
        public bool attackState = false;
        //bool isRecruit;
        public bool inShipTransform = false;

        public AbsSoldierProfile typeCurrentData;
        public AbsSoldierProfile typeSoldierData;
        public AbsSoldierProfile typeShipData;

        public SoldierConscriptProfile soldierConscript;
        //float energyPerSoldier;
        public SoldierData soldierData;
        public bool isShip = false;

#if VISUAL_NODES
        Graphics.Mesh collisionModel;
#endif
        WalkingPath path = null;
        DetailWalkingPath detailPath = null;
        float waitTime = 0;

        public SoldierGroup(Army army, SoldierConscriptProfile conscript, Vector3 startPos)
        {
            this.army = army;
            soldierConscript = conscript;
            initPart1();

            position = startPos;
            tilePos = WP.ToTilePos(position);

            initPart2(typeCurrentData);

            soldierCount = typeCurrentData.rowWidth * typeCurrentData.columnsDepth;
            soldierData = soldierConscript.init(typeCurrentData);
            
            initPart3(typeCurrentData);

            if (army.faction.player.IsPlayer())
            {
                army.faction.player.GetLocalPlayer().statistics.SoldiersRecruited += soldierCount;
            }            
        }

        private void initPart1()
        {
#if VISUAL_NODES
            collisionModel = new Graphics.Mesh(LoadedMesh.SelectCircleSolid, position, new Vector3(WalkDirBound.radius * 2f), TextureEffectType.Flat, SpriteName.WhiteArea, Color.HotPink, false);
            collisionModel.AddToRender(DrawGame.UnitDetailLayer);
#endif
            type = soldierConscript.unitType();
            typeSoldierData = DssRef.profile.Get(type);//new ConscriptedSoldierData();
            typeShipData = DssRef.profile.Get(typeSoldierData.ShipType());

            typeCurrentData = typeSoldierData;

            armyGridPlacement2 = army.nextArmyPlacement(soldierConscript.conscript.DefaultArmyRow());
           
        }

        void initPart2(AbsSoldierProfile typeData)
        {
            int count = typeData.rowWidth * typeData.columnsDepth;

            
            halfColDepth = typeData.columnsDepth * -0.5f;

            skirmishCount = MathExt.MultiplyInt(0.3, count);

            groupRadius = 0.2f;
        }

        void initPart3(AbsSoldierProfile typeData)
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

        public void setDetailLevel(bool unitDetailView)
        {
            if (unitDetailView)
            {
                if (soldiers == null)
                {
                    createAllSoldiers(typeCurrentData, soldierCount);
                    if (typeCurrentData.IsShip())
                    {
                        FirstSoldier().health = shipHealth;
                    }
                }
            }
            else
            {
                if (soldiers != null)
                {
                    if (typeCurrentData.IsShip())
                    {
                        var first = FirstSoldier();
                        if (first != null)
                        {
                            shipHealth = first.health;
                        } 
                    }
                    deleteAllSoldiers(DeleteReason.CameraCulling);
                }
            }
        }

        //public bool enterRender_detailLayer_async = false;
        //public bool inRender_detailLayer = false;

        //public void asynchCullingUpdate(bool bStateA)
        //{
        //    bool enterRender_overviewLayer_async
        //    DssRef.state.culling.InRender_Asynch(ref enterRender_overviewLayer_async, ref enterRender_detailLayer_async, tilePos);
        //}

        public void writeGameState(System.IO.BinaryWriter w)
        {
            //w.Write((byte)type);
            //w.Write((byte)FirstSoldierData().unitType);

            //typeSoldierData.writeGameState(w);
            soldierConscript.writeGameState(w);
            w.Write(isShip);

            //typeCurrentData = typeSoldierData;

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
            soldierConscript.readGameState(r);

            initPart1();

            isShip = r.ReadBoolean();
            typeCurrentData = isShip ? typeShipData : typeSoldierData;


            armyLocalPlacement.readShort(r);

            bool lockedInArmyGrid = r.ReadBoolean();

            if (!lockedInArmyGrid)
            {
                WP.readPosXZ(r, out position, out tilePos);
                rotation.ByteDir = r.ReadByte();
            }

            groupObjective = r.ReadByte();

            int soldiersCount = r.ReadByte();
            bool soldiersLockedInGroup = groupObjective == GroupObjective_FollowArmyObjective;

            //AbsSoldierData typeData = DssRef.unitsdata.Get(type);
            initPart2(typeCurrentData);

            //AbsSoldierData soldierData = DssRef.unitsdata.Get(soldierType);

            //if (soldierType == UnitType.Recruit)
            //{
            //    new TrainingCompleteTimer(this);
            //}

            createAllSoldiers(typeCurrentData, soldiersCount);

            if (!soldiersLockedInGroup)
            {
                var soldiersC = soldiers.counter();
                while (soldiersC.Next())
                {
                    soldiersC.sel.readGameState(r, version);
                }
            }

            initPart3(typeCurrentData);
        }

        public void writeNet(System.IO.BinaryWriter w)
        {

        }
        public void readNet(System.IO.BinaryReader r)
        {

        }

        void refreshAttackRadius(AbsSoldierProfile typeData)
        {
            //var first = FirstSoldier();
            //if (first != null)
            //{
                if (soldierData.bonusProjectiles > 0)
                {
                    attackRadius = groupRadius + soldierData.secondaryAttackRange;
                }
                else
                {
                    attackRadius = groupRadius + soldierData.attackRange;
                }
            //}
        }

        private void createAllSoldiers(AbsSoldierProfile typeProfile, int count)
        {
            soldiers = new SpottedArray<AbsSoldierUnit>(count);
            soldierData = soldierConscript.init(typeProfile);
            //energyPerSoldier = soldierData.energyPerSoldier;
            if (typeProfile.IsShip())
            {
                soldierConscript.shipSetup(ref soldierData);
            }

            int xStart = -typeProfile.rowWidth / 2;
            IntVector2 bannerPos = bannerManPos();

            int columnDepth = MathExt.Div_Ceiling(count, typeProfile.rowWidth);

            //setGroundY();

            for (int x = 0; x < typeProfile.rowWidth; ++x)
            {
                int leadUnit = -1;
                for (int y = 0; y < columnDepth; ++y)
                {
                    AbsSoldierUnit unit;
                    if (bannerPos.Equals(x, y))
                    {
                        var bannerData = soldierConscript.bannermanSetup(soldierData);
                        unit = createUnit(DssRef.profile.bannerman, new IntVector2(x + xStart, y), tilePos, ref bannerData);
                    }
                    else
                    {
                        unit = createUnit(typeProfile, new IntVector2(x + xStart, y), tilePos, ref soldierData);
                    }


                    unit.following = leadUnit;
                    unit.firstUpdate();
                    leadUnit = unit.parentArrayIndex;

                    if (--count <= 0)
                    {
                        
                        return;
                    }
                }
            }

            //setGroundY();

            //var soldiersC = soldiers.counter();
            //while (soldiersC.Next())
            //{
            //    soldiersC.sel.firstUpdate();
            //}
        }

        void deleteAllSoldiers(DeleteReason reason)
        { 
            soldierCount = soldiers.Count;
            var soldiersC = soldiers.counter();
            while (soldiersC.Next())
            {
                soldiersC.sel.DeleteMe(reason, false);
            }

            soldiers = null;
        }

        void refreshRotateSpeed()
        {
            float muliply = 1.6f - 0.15f * (int)soldierConscript.conscript.training;
            rotateSpeed = (float)Math.Abs(Math.Atan2(walkSpeed * muliply, groupRadius));
        }

        IntVector2 bannerManPos()
        {
            IntVector2 bannerPos;
            if (typeCurrentData.hasBannerMan)
            {
                bannerPos = new IntVector2(typeCurrentData.rowWidth / 2, typeCurrentData.columnsDepth - 1);
            }
            else
            {
                bannerPos = IntVector2.NegativeOne;
            }

            return bannerPos;
        }

        public void completeTransform(SoldierTransformType transformType)
        {
            if (isDeleted) return;

            if (isShip != (transformType == SoldierTransformType.ToShip))
            {   
                isShip = transformType == SoldierTransformType.ToShip;

                if (isShip)
                {
                    shipHealth = soldierData.basehealth * soldierCount;
                    typeCurrentData = typeShipData;
                    soldierData = soldierConscript.init(typeCurrentData);                    
                }
                else
                {
                    typeCurrentData = typeSoldierData;
                    soldierData = soldierConscript.init(typeCurrentData);
                }

                if (soldiers != null)
                {
                    int totalHealth = 0;

                    var soldiersC = soldiers.counter();
                    while (soldiersC.Next())
                    {
                        totalHealth += soldiersC.sel.health;
                        soldiersC.sel.DeleteMe(DeleteReason.Transform, false);
                    }
                    soldiers.Clear();

                    if (transformType == SoldierTransformType.ToShip)
                    {
                        var shipData = soldierData;
                        soldierConscript.shipSetup(ref shipData);

                        var ship = createUnit(typeShipData, IntVector2.Zero, WP.ToTilePos(position), ref shipData);
                        ship.position = position;
                        ship.health = totalHealth;
                        ship.refreshShipCarryCount();
                    }
                    else
                    {   
                        int count = (int)Math.Ceiling(totalHealth / (double)soldierData.basehealth);

                        createAllSoldiers(typeCurrentData, count);
                    }

                    refreshAttackRadius(typeCurrentData);
                }

                state = GroupState.FindArmyPlacement;
            }
               
            inShipTransform = false;            
        }

        public Vector3 armyPlacement(Vector3 center)
        {
            Vector2 offset = VectorExt.RotateVector(new Vector2(
                armyLocalPlacement.X * DssVar.SoldierGroup_Spacing,
                armyLocalPlacement.Y * DssVar.SoldierGroup_Spacing),
                army.rotation.radians);

            center.X += offset.X;
            center.Z += offset.Y;

            DssRef.world.unitBounds.KeepPointInsideBound_Position(ref center.X, ref center.Z);

            return center;
        }

        public AbsSoldierUnit createUnit(AbsSoldierProfile typeProfile, IntVector2 gridPlacement, IntVector2 area, ref SoldierData data)
        {
            AbsSoldierUnit s;
            
            s = typeProfile.CreateUnit();
            s.UnitType = typeProfile.unitType;
            s.soldierData = data;
          
            
            s.InitLocal(position, gridPlacement, area, this);
            s.position = WP.ToWorldPos(area); //temp pos
            s.parentArrayIndex = soldiers.Add(s);

            if (army.inRender_detailLayer)
            {
                s.setDetailLevel(true);
                s.update(1f, true);
            }
            return s;
        }

        Vector3 walkingGoalWp(out bool shiptransform)
        {
            var path_sp = detailPath;
            if (path_sp != null && path_sp.NodeCountLeft() > 1)
            {
               Vector3 result = path_sp.NextNodeWp(position, out bool complete, out shiptransform);
               return result;
            }

            shiptransform = isShip;
            return goalWp;
        }

        Vector3 walkingGoalAttackTarget(AbsGroup target, out bool shiptransform)
        {            
            var path_sp = detailPath;
            if (path_sp != null && path_sp.NodeCountLeft() > 1)
            {
                Vector3 result = path_sp.NextNodeWp(position, out bool complete, out shiptransform);
                return result;
            }

            shiptransform = isShip;
            return target.position;
        }

        public void update(float time, bool fullUpdate)
        {
            if (inShipTransform)
            {
                return;
            }

            var attack_sp = attacking_soldierGroupOrCity;

            if (attack_sp != null)
            {
                Vector2 diff = new Vector2(
                    attack_sp.position.X - position.X,
                    attack_sp.position.Z - position.Z);

                //float speed = walkSpeed * terrainSpeedMultiplier * time;

                if (diff.Length() < attackRadius)
                {
                    //Attack
                    if (soldiers != null)
                    {
                        var soldiersC = soldiers.counter();
                        while (soldiersC.Next())
                        {
                            soldiersC.sel.update2_battle(time, fullUpdate);
                        }
                    }
                }
                else
                {

                    //Battle update
                    if (updateWalking(walkingGoalAttackTarget(attack_sp, out bool shipTransform), false, 0, time))
                    {
                        //on target
                    }

                    if (soldiers != null)
                    {
                        var soldiersC = soldiers.counter();
                        while (soldiersC.Next())
                        {
                            soldiersC.sel.update2(time);

                        }
                    }
                }

                
            }
            else
            {
                switch (state)
                {
                    case GroupState.Idle:
                        waitTime += time;
                        if (waitTime >= 5000)
                        {
                            waitTime = 0f;
                            if ((goalWp - position).PlaneXZLength() > WorldData.SubTileHalfWidth)
                            {
                                state = GroupState.FindArmyPlacement;
                                wakeupSoldiers();
                            }
                        }

                        break;

                    case GroupState.FindArmyPlacement:
                        waitTime += time;

                        bool move = true;

                        if (isWalkingIntoOtherGroup)
                        {
                            //queueu, holding position
                            if (waitTime > 3000)
                            {
                                waitTime = -2000;
                            }
                            else if (waitTime < 0)
                            {
                                move = true;
                            }
                            else
                            {
                                move = false;
                            }
                        }
                        else
                        {
                            waitTime = 0;
                        }

                        if (move)
                        {
                            if (updateWalking(walkingGoalWp(out bool shipTransform), true, army.rotation, time))
                            {
                                state = GroupState.GoingIdle;
                                waitTime = 0;
                            }

                            if (shipTransform != isShip)
                            {
                                //    if ((nextIsFootTransform && IsShip()) ||
                                //        (nextIsShipTransform && !IsShip()))
                                //    {
                                if (!inShipTransform)
                                {
                                    inShipTransform = true;
                                    new ShipTransform(this, true);
                                }
                                //    }
                            }
                        }
                        break;
                    case GroupState.GoingIdle:
                        waitTime += time;
                        break;
                }

                bool allIdle = true;

                if (state == GroupState.Idle)
                {
                    //Passive check of souroundings
                }
                else
                {
                    if (soldiers != null)
                    {
                        var soldiersC = soldiers.counter();
                        while (soldiersC.Next())
                        {
                            soldiersC.sel.update2(time);
                            allIdle &= soldiersC.sel.state2 == SoldierState2.idle;
                        }
                    }
                }

                if (allIdle &&
                    state == GroupState.GoingIdle &&
                    waitTime >= 2000)
                {
                    state = GroupState.Idle;
                }
            }

            //return;

            ////OLD
            //if (debugTagged)//groupId == 1611)
            //{
            //    lib.DoNothing();
            //}

            //if (soldiers.Count > 0)
            //{
            //    if (!lockMovement)
            //    {
            //        var battleGroup_sp = army.battleGroup;
            //        if (battleGroup_sp != null &&
            //            battleGroup_sp.battleState != Battle.BattleState.Battle)
            //        {
            //            update_battlePreparations(time, fullUpdate);
            //            return;
            //        }


            //        //UPDATE OBJECTIVE
            //        {
            //            bool newIdleGroup = false;

            //            int newObjective = groupObjective;
            //            bool newAttackState = false;
            //            bool induvidualUpdate;
            //            bool walking = false;
            //            if (groupObjective == GroupObjective_IsSplit)
            //            {
            //                induvidualUpdate = true;

            //                if (//attacking_soldierGroupOrCity == null &&
            //                    army.battleGroup == null)//dont regroup in battle (start spinning)
            //                {
            //                    refreshGroupPositions();

            //                    if (fullUpdate)
            //                    {
            //                        var soldiersC = soldiers.counter();
            //                        while (soldiersC.Next())
            //                        {
            //                            soldiersC.sel.setReGroupState();
            //                        }

            //                        newObjective = GroupObjective_ReGrouping;
            //                    }
            //                    else
            //                    {
            //                        induvidualUpdate = false;
            //                        newObjective = GroupObjective_FindArmyPlacement;
            //                    }
            //                }
            //            }
            //            else if (groupObjective == GroupObjective_ReGrouping)
            //            {
            //                if (fullUpdate)
            //                {
            //                    induvidualUpdate = true;

            //                    if (allInduvidualsAreIdle)
            //                    {
            //                        //LOCK in place
            //                        newObjective = GroupObjective_FindArmyPlacement;

            //                        var soldiersC = soldiers.counter();
            //                        while (soldiersC.Next())
            //                        {
            //                            soldiersC.sel.clearAttack();
            //                            soldiersC.sel.aiState = SoldierAiState.GroupLock;
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    induvidualUpdate = false;
            //                    newObjective = GroupObjective_FindArmyPlacement;
            //                }
            //            }
            //            else
            //            { //Moving like a group

            //                induvidualUpdate = false;
            //                var closest_sp = attacking_soldierGroupOrCity;
            //                if (closest_sp != null)
            //                {
            //                    if (groupCollisionDistance(closest_sp) < 0.02f)
            //                    {
            //                        //SPLIT GROUP
            //                        newObjective = GroupObjective_IsSplit;
            //                        newAttackState = true;

            //                        var soldiersC = soldiers.counter();
            //                        while (soldiersC.Next())
            //                        {
            //                            soldiersC.sel.setAttackState();
            //                        }
            //                    }
            //                    else
            //                    {
            //                        //Group attack move
            //                        walking = !updateWalking(goalWp, false, Rotation1D.D0, time);
            //                    }
            //                }
            //                else if (army.battleGroup != null)
            //                {
            //                    walking = !updateWalking(goalWp, false, Rotation1D.D0, time);
            //                }
            //                else if (groupObjective == GroupObjective_FindArmyPlacement)
            //                {
            //                    if (updateWalking(goalWp, true, army.rotation, time))
            //                    {
            //                        newObjective = GroupObjective_FollowArmyObjective;
            //                        newIdleGroup = true;
            //                    }
            //                    else
            //                    {
            //                        walking = true;
            //                    }
            //                }
            //                else
            //                {
            //                    if (army.objective == ArmyObjective.MoveTo ||
            //                        army.objective == ArmyObjective.Attack)
            //                    {
            //                        if (updateWalking(goalWp, true, army.rotation, time))
            //                        {
            //                            newIdleGroup = true;
            //                        }
            //                        else
            //                        {
            //                            walking = true;
            //                        }
            //                        newObjective = GroupObjective_FindArmyPlacement;
            //                    }
            //                    else
            //                    {
            //                        lib.DoNothing();
            //                    }
            //                    //Follow army

            //                }

            //            }

            //            if (induvidualUpdate)
            //            {
            //                var soldiersC = soldiers.counter();
            //                while (soldiersC.Next())
            //                {
            //                    soldiersC.sel.update(time, fullUpdate);
            //                }
            //            }
            //            else
            //            {
            //                var soldiersC = soldiers.counter();
            //                while (soldiersC.Next())
            //                {
            //                    soldiersC.sel.update_GroupLocked(walking);
            //                }
            //            }

            //            groupIsIdle = newIdleGroup;

            //            if (newObjective != groupObjective)
            //            {
            //                groupObjective = newObjective;
            //            }
            //            attackState = newAttackState;
            //        }
            //    }

            //}
            //else
            //{
            //    if (!inShipTransform)
            //    {
            //        if (fullUpdate)
            //        {
            //            DeleteMe(DeleteReason.EmptyGroup, true);
            //        }
            //        else
            //        {
            //            Ref.update.AddSyncAction(new SyncAction2Arg<DeleteReason, bool>(DeleteMe, DeleteReason.EmptyGroup, true));
            //        }
            //    }
            //}
        }


        public override void toHud(ObjectHudArgs args)
        {
            //var typeData = typeSoldierData..conscript;

            args.content.h2(soldierConscript.conscript.TypeName() + " " + DssRef.lang.UnitType_SoldierGroup + " (" + parentArrayIndex.ToString() + ")").overrideColor = HudLib.TitleColor_TypeName;
            args.content.newLine();

#if DEBUG
            debugTagButton(args.content);

            HudLib.Label(args.content, "Army column width");
            args.content.newLine();
            for (int w = Army.MinColumnWidth; w <= Army.MaxColumnWidth; w += 2)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(w.ToString()) },
                    new RbAction1Arg<int>(army.armyColumnWidthClick, w, SoundLib.menu));
                button.setGroupSelectionColor(HudLib.RbSettings, w == army.armyColumnWidth);
                args.content.Add(button);
                args.content.space();
            }

            args.content.newLine();
            //args.content.text($"column {armyGridPlacement2.X}");
            //args.content.text($"row {armyGridPlacement2.Y}");
            args.content.Add(new RichBoxSeperationLine());

            for (int y = 0; y < ArmyPlacementGrid.RowsCount; y++)
            {
                int rowY = y - ArmyPlacementGrid.PosYAdd;

                string name;
                switch (rowY)
                {
                    case ArmyPlacementGrid.Row_Front:
                        name = "Front";
                        break;
                    default:
                        name = "Body";
                        break;
                    case ArmyPlacementGrid.Row_Second:
                        name = "Second";
                        break;
                    case ArmyPlacementGrid.Row_Behind:
                        name = "Behind";
                        break;

                }

                args.content.newLine();
                args.content.Add(new RichBoxText(name));
                args.content.Add(new RichBoxTab(0.3f));
                for (int x = 0; x < ArmyPlacementGrid.ColsCount; x++)
                {
                    args.content.space();

                    int colX = x - ArmyPlacementGrid.PosXAdd;
                    
                    string caption = colX == 0 ? " C " : TextLib.PlusMinus(colX);
                    var button = new RichboxButton(new List<AbsRichBoxMember> {
                        new RichBoxText(caption)
                    },
                    new RbAction2Arg<int, int>(setNewArmyPlacement, colX, rowY), null);

                    button.setGroupSelectionColor(HudLib.RbSettings, armyGridPlacement2.X == colX && armyGridPlacement2.Y == rowY);
                    args.content.Add(button);
                }

            }
            args.content.Add(new RichBoxSeperationLine());
            //args.content.Button("debug tag", new HUD.RichBox.RbAction(AddDebugTag), null, true);
#endif
            soldierConscript.conscript.toHud(args.content);
            args.content.newLine();
            //if (args.selected && GetFaction() == args.player.faction)
            //{
            //    new Display.GroupMenu(args.player, this, args.content);
            //}
        }

        void setNewArmyPlacement(int colX, int rowY)
        { 
            armyGridPlacement2.X = colX;
            armyGridPlacement2.Y = rowY;

            army.refreshPositions(false);
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
                //var typeData = DssRef.unitsdata.Get(type);

                IntVector2 nextPos = IntVector2.Zero;
                int bannerLead = -1;
                int xStart = -typeCurrentData.rowWidth / 2;

                var soldiersC = soldiers.counter();
                AbsSoldierUnit[] leadRow = new AbsSoldierUnit[typeCurrentData.rowWidth];

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
                            if (++nextPos.X >= typeCurrentData.rowWidth)
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

        static readonly float GoalCompleteDistance = WorldData.SubTileWidth * 0.2f;
        bool updateWalking(Vector3 walkTowards, bool rotate, Rotation1D finalRotation, float time)
        {
            Vector2 diff = new Vector2(
                walkTowards.X - position.X,
                walkTowards.Z - position.Z);

            float speed = walkSpeed * terrainSpeedMultiplier * time;

            if (diff.Length() > Math.Max(speed, GoalCompleteDistance))
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


            var path_sp = detailPath;
            if (detailPath != null && detailPath.HasMoreNodes())
            {
                angleAdd *= 1.5f;
            }

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
                    if (distanceValueTo(attacking_soldierGroupOrCity, float.MaxValue) <= distanceValueTo(newTarget, float.MaxValue))
                    {
                        return;
                    }
                }

                if (attacking_soldierGroupOrCity == null)
                {
                    //Enter attack
                    detailPath = null;
                    path = null;
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

            List<GameObject.SoldierGroup> opponents;//, friendly;

            opponents = DssRef.world.unitCollAreaGrid.collectOpponentGroups(army.faction, tilePos);

            refreshAttacking();

            AbsGroup nearest = null;
            float distanceValue = float.MaxValue;

            float aggroRange = 2f;
            
            foreach (var opponent in opponents)
            {
                var type = opponent.gameobjectType();
                if (type == GameObjectType.SoldierGroup)
                {
                    //var groupsC = opponent.GetArmy().groups.counter();
                    //while (groupsC.Next())
                    //{
                    var group = opponent.GetGroup();
                    if (group.soldierCount > 0)
                    {
                        var dist = distanceValueTo(group, aggroRange);

                        if (dist < distanceValue)
                        {
                            distanceValue = dist;
                            nearest = group;
                        }
                    }
                    //}
                }
                else if (type == GameObjectType.City)
                {
                    if (opponent.GetCity().guardCount > 0)
                    {
                        var dist = distanceValueTo(opponent, aggroRange);
                        if (dist < distanceValue)
                        {
                            distanceValue = dist;
                            nearest = opponent;
                        }
                    }
                }
            }

            if (nearest != null)
            {
                addAttackTarget(nearest);
            }

            //if (opponents.Count > 0)
            //{
            //    lib.DoNothing();
            //}

            //if (attacking_soldierGroupOrCity == null && army.battles.Count > 0)
            //{               
            //    //Version 2: förlitar sig på att mapobject.battles 
            //    AbsGroup nearest = null;
            //    float distanceValue = float.MaxValue;
            //    var battlesC = army.battles.counter();
            //    while (battlesC.Next())
            //    {
            //        var type = battlesC.sel.gameobjectType();
            //        if (type == GameObjectType.Army)
            //        {
            //            var groupsC = battlesC.sel.GetArmy().groups.counter();
            //            while (groupsC.Next())
            //            {
            //                if (groupsC.sel.soldiers.Count > 0)
            //                {
            //                    var dist = distanceValueTo(groupsC.sel);

            //                    if (dist < distanceValue)
            //                    {
            //                        distanceValue = dist;
            //                        nearest = groupsC.sel;
            //                    }
            //                }
            //            }
            //        }
            //        else if (type == GameObjectType.City)
            //        {
            //            if (battlesC.sel.GetCity().guardCount > 0)
            //            {
            //                var dist = distanceValueTo(battlesC.sel);
            //                if (dist < distanceValue)
            //                {
            //                    distanceValue = dist;
            //                    nearest = battlesC.sel;
            //                }
            //            }
            //        }
            //    }

            //    //closestOpponent = nearest;
            //    if (nearest != null)
            //    {
            //        addAttackTarget(nearest);
            //    }
            //}

        }

        float distanceValueTo(AbsGroup toGroup, float maxRange)
        {
            Vector2 diff = new Vector2(toGroup.position.X - this.position.X, toGroup.position.Z - this.position.Z);

            float l = diff.Length();
            if (l > maxRange)
            {
                return float.MaxValue;
            }

            const float AngleDiffValue = 0.5f;

            float dir = lib.V2ToAngle(diff);
            float aDiff =Rotation1D.AngleDifference_Absolute(rotation.radians, dir);
            float anglePercDiff = Math.Abs(aDiff) / MathExt.TauOver2;

            float value = l + l * anglePercDiff * AngleDiffValue;

            return value;
        }

        static List<GameObject.Army> ArmiesColl_asyncupdate = new List<Army>();

        void collsionUpate_async()
        {
            Vector2 diff = new Vector2(
                goalWp.X - position.X,
                goalWp.Z - position.Z);

            Vector2 norm = VectorExt.Normalize(diff, out float l);

            if (l > WorldData.SubTileWidth)
            {
                Vector2 center = new Vector2(position.X, position.Z);
                WalkDirBound.center = center + norm * WalkDirCheckLength;
#if VISUAL_NODES
                collisionModel.Visible = true;
                collisionModel.Color = Color.HotPink;
                collisionModel.position = VectorExt.V2toV3XZ(WalkDirBound.center, position.Y);
#endif
                DssRef.world.unitCollAreaGrid.collectArmies(tilePos, ArmiesColl_asyncupdate);
                foreach (var army in ArmiesColl_asyncupdate)
                {
                    var groupC = army.groups.counter();
                    while (groupC.Next())
                    {
                        if (groupC.sel != this)
                        {
                            OtherBound.center = VectorExt.V3XZtoV2(groupC.sel.position);
                            if (WalkDirBound.Intersect(OtherBound))
                            {
                                if (
                                    !VectorExt.IsMovingCloser(center, norm, OtherBound.center) || 
                                    (groupC.sel.state == GroupState.Idle && l > DssVar.DefaultGroupSpacing * 2)
                                    )
                                {
                                    //Ignore and walk through
                                }
                                else
                                {
#if VISUAL_NODES
                                    collisionModel.Color = Color.Red;
#endif
                                    isWalkingIntoOtherGroup = true;
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            isWalkingIntoOtherGroup = false;
        }

        public void asynchUpdate()
        {
#if VISUAL_NODES
            collisionModel.Visible = false; 
#endif
            asynchNearObjectsUpdate();

            var attack_sp = attacking_soldierGroupOrCity;

            if (attack_sp != null)
            {
                if (soldiers != null)
                {
                    tilePos = WP.ToTilePos(position);
                    setGroundY();

                    IntVector2 goalSubTile = WP.ToSubTilePos(attack_sp.position);
                    if (detailPath == null || detailPath.goal != goalSubTile)
                    {
                        pathCalulate_detail(goalSubTile);
                    }

                    var counter = soldiers.counter();
                    while (counter.Next())
                    {
                        counter.sel.asyncBattleUpdate();
                    }
                }
            }
            else
            {
                if (followsGoalId != army.goalId)
                {
                    followsGoalId = army.goalId;
                    path = null;
                    detailPath = null;
                    if (state <= GroupState.GoingIdle)
                    {
                        state = GroupState.FindArmyPlacement;
                        wakeupSoldiers();
                    }
                }

                if (state != GroupState.Idle)
                {
                    tilePos = WP.ToTilePos(position);
                    setGroundY();
                    collsionUpate_async();

                    const float DetailMaxLength = 3.5f;
                    Vector3 diff = goalWp - position;
                    float l = VectorExt.PlaneXZLength(diff);
                    IntVector2 goalSubTile;
                    if (l > DetailMaxLength)
                    {
                        if (path == null)
                        {
                            pathCalulate();
                        }

                        //pick three tiles ahead
                        IntVector2 aheadPathTile = path.getNodeAhead(3, tilePos);
                        goalSubTile = WP.ToSubTilePos_Centered(aheadPathTile);
                    }
                    else
                    {
                        goalSubTile = WP.ToSubTilePos(goalWp);
                    }

                    if (detailPath == null || detailPath.goal != goalSubTile)
                    {
                        pathCalulate_detail(goalSubTile);
                    }
                }
            }
            
            //return;

            //int soldiersWithWalkingOrder = 0;
            //int idleSoldiers = 0;
            //int soldierCount = 0;


            //Vector3 sumPos = Vector3.Zero;
            //{
            //    var counter = soldiers.counter();
            //    while (counter.Next())
            //    {
            //        var soldier = counter.sel;
            //        soldier.asynchUpdate();

            //        sumPos += soldier.position;
            //        soldierCount++;

            //        if (soldier.hasWalkingOrder)
            //        {
            //            soldiersWithWalkingOrder++;
            //        }
            //        if (soldier.state.idle)
            //        {
            //            idleSoldiers++;
            //        }
            //    }
            //}


            //if (soldierCount == 0)
            //{
            //    //groupObjective = GroupObjective.Destroyed;
            //    return;
            //}

            ////updateObjective();

            //if (groupObjective == GroupObjective_IsSplit || position.X<=0)
            //{
            //    position = sumPos / soldierCount;
            //}

            //tilePos = WP.ToTilePos(position);

            //Map.Tile tile;
            //if ( DssRef.world.tileGrid.TryGet(tilePos, out tile))
            //{
            //    terrainSpeedMultiplier = tile.TerrainSpeedMultiplier(isShip) * 0.4f + army.terrainSpeedMultiplier * 0.6f;
            //    position.Y = tile.UnitGroundY();
            //}
            //allInduvidualsAreIdle = idleSoldiers == soldierCount;
        }

        void pathCalulate()
        {
            PathFinding pf = DssRef.state.pathFindingPool.Get();
            { 
                path = pf.FindPath(tilePos, rotation, WP.ToTilePos( goalWp), isShip);
            }
            DssRef.state.pathFindingPool.Return(pf);
        }


        void pathCalulate_detail(IntVector2 goal)
        {
            //Path towards army end position
            //make a big path towards end pos
            //make detail path 3 tiles long at a time
            
            //bool endAsShip = DssRef.world.tileGrid.Get(army.adjustedWalkGoal).IsWater();

            DetailPathFinding pf = DssRef.state.detailPathFindingPool.Get();
            {
                detailPath = pf.FindPath(WP.ToSubTilePos(position), rotation, goal,
                    isShip, army.walkGoalAsShip);
            }
            DssRef.state.detailPathFindingPool.Return(pf);

            if (detailPath.blockedPath)
            {
                if (detailPath.HasMoreNodes())
                {
                    goalWp = WP.SubtileToWorldPosXZ(detailPath.LastNode());
                }
                else
                {
                    goalWp = position;
                }
            }
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

        //public void setWalkNode(IntVector2 area,
        //    bool nextIsFootTransform, bool nextIsShipTransform)
        //{
        //    //if (parentArrayIndex== 5152)
        //    //{
        //    //    lib.DoNothing();
        //    //}

        //    walkingOrderTo = area;
        //    Vector3 areaCenter = WP.ToWorldPos(area);
        //    //goalWp = armyPlacement(areaCenter);


        //    if ((nextIsFootTransform && IsShip()) ||
        //        (nextIsShipTransform && !IsShip()))
        //    {
        //        if (!inShipTransform)
        //        {
        //            inShipTransform = true;
        //            new ShipTransform(this, false);
        //        }
        //    }

        //}

        public void bumpWalkToNode(IntVector2 nodePos)
        {
            //walkingOrderTo = nodePos;
            Vector3 areaCenter = WP.ToWorldPos(army.nextNodePos);
            goalWp = armyPlacement(areaCenter);
        }

        public void setGroundY()
        {
            position.Y = DssRef.world.tileGrid.Get(tilePos).GroundY_aboveWater();
            //var counter = soldiers.counter();
            //while (counter.Next())
            //{
            //    counter.sel.firstUpdate();
            //}
            //++lifeState;
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
                
                double immigrants = soldierCount * keep;

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

            if (army.inRender_detailLayer)
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

        //override public bool isMelee()
        //{
        //    return typeCurrentData.mainAttack == AttackType.Melee;
        //}

        //public bool ScoutMovement()
        //{
        //    AbsSoldierUnit s = FirstSoldier();
        //    if (s == null)
        //    {
        //        return false;
        //    }
        //    return s.data.scoutMovement;
        //}

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

        public void Upkeep(ref float energy)
        {
            energy += soldierData.energyPerSoldier * soldierCount;
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

            if (soldiers != null)
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
            return soldierCount == 0;
        }

        public override bool aliveAndBelongTo(int faction)
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

        public void setArmyPlacement2(Vector3 wp)
        {
            goalWp = wp;

            //bool onSpawn = false;
            if (lifeState == LifeState_New)
            {
                //onSpawn = true;
                ++lifeState;
                //position = goalWp;

                if (army.inRender_detailLayer)
                {
                    setDetailLevel(true);
                }
            }
            wakeupSoldiers();

            state = GroupState.FindArmyPlacement;
        }

        void wakeupSoldiers()
        {
            var soldiers_sp = soldiers; 
            if (soldiers_sp != null)
            {  
                var soldiersC = soldiers_sp.counter();
                while (soldiersC.Next())
                {
                    soldiersC.sel.wakeUp2();
                }

            }
        }

       

        public void SetArmyPlacement(IntVector2 newLocalPlacement, bool onPurchase)
        {
            if (armyLocalPlacement != newLocalPlacement ||  onPurchase)
            {

                //armyLocalPlacement = newLocalPlacement;
                //goalWp = armyPlacement(army.position);

                if (!army.inRender_detailLayer || lifeState == LifeState_New)
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


        //public bool IsShip()
        //{ 
            
        //    var  first = soldiers.First();
        //    if (first != null)
        //    { 
        //        return first.IsShipType();
        //    }

        //    return false;
        //}

        //public AbsSoldierProfile FirstSoldierData()
        //{
        //    var first = soldiers.First();
        //    if (first != null)
        //    {
        //        return first.profile;
        //    }

        //    return typeCurrentData;//DssRef.unitsdata.Get(type);
        //}

        public override string TypeName()
        {
            return soldierConscript.conscript.TypeName() + " Group(" + parentArrayIndex.ToString() + ")";
        }

        //public override SpriteName TypeIcon()
        //{
        //    return AllUnits.UnitFilterIcon( soldierConscript.filterType());
        //}

        public override void TypeIcon(RichBoxContent content)
        {
            content.Add(new RichBoxImage(AllUnits.UnitFilterIcon(soldierConscript.filterType())));
        }

        public override string ToString()
        {
            return "Group " + type.ToString() + " x" + soldiers.Count.ToString() + ", id" + parentArrayIndex.ToString();
        }
    }    

    enum AggressionCommand
    {
        Hold,
        Normal,
    }

    enum GroupState
    { 
        Idle,
        GoingIdle,
        FindArmyPlacement,
        Battle,
        //Rotate,
    }

    //enum GroupObjective
    //{
    //    FollowArmyObjective,

    //    IsSplit,
    //    ReGrouping,
    //    FindArmyPlacement,
    //}
}
