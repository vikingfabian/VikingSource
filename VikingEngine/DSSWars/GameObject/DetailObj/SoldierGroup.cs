//#define VISUAL_NODES

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.GameObject.DetailObj.Soldiers;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.DSSWars.Net;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Command;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.EngineSpace.Graphics.In3D;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.PJ.MiniGolf;
using VikingEngine.ToGG.HeroQuest.Gadgets;

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

        public static Physics.CircleBound OtherBound;
        public static Physics.CircleBound WalkDirBound;
        static float WalkDirCheckLength;
        public static void Init()
        {
            OtherBound = new Physics.CircleBound(DssVar.SoldierGroup_CollisionRadius);
            WalkDirBound = new Physics.CircleBound(DssVar.SoldierGroup_MoveCollisionRadius);
            WalkDirCheckLength = DssVar.SoldierGroup_CollisionRadius * 0.9f;
        }

        bool isWalkingIntoOtherGroup = false;
        public bool restingGuardMode = false;
        public float soldierAttackRangeBonus = 0;
        public int soldierAttackDamageBonus = 0;
        public float halfColDepth;

        int soldierCountValue = 1;
        public int soldierCount
        {
            get { return soldierCountValue; }
            set { 
                soldierCountValue = value;
                if (soldierCountValue <= 0)
                {
                    lib.DoNothing();
                }
            }
        }
        int shipHealth;
        public SpottedArray<AbsSoldierUnit> soldiers = null;

        public WeakReference<AbsArmy> army;

        public Vector3 goalWp, armyPlacementWp;

        int followsGoalId = int.MinValue;

        public IntVector2 tilePos;
        public IntVector2 armyGridPlacement2 = IntVector2.Zero;
        public Rotation1D rotation;

        public float terrainSpeedMultiplier = 1.0f;
        public float walkSpeed_peace = DssConst.Men_StandardWalkingSpeed;
        float rotateSpeed;

        //public WeakReference<AbsGroup> attackTarget_soldierGroupOrCity = null;

        public PGameObject attackTarget_soldierGroupOrCity = PGameObject.Empty;
        GameTimeStamp attackTargetTimeLock = GameTimeStamp.None;
        
        public GroupState state = GroupState.Idle;
        
        public bool inShipOrGuardTransform = false;
        public float damageBlockChance_fromTerrain = 0;

        public UnitBuildType currentBuilder;
        public UnitBuildType landBuilder;
        public UnitBuildType shipBuilder;

        public SoldierConscriptProfile soldierConscript;
        public SoldierData soldierData;
        public SoldierData soldierData_soldier;
        public bool isShip = false;
        public TimeStamp lastNetUpdate = new TimeStamp();

#if VISUAL_NODES
        Graphics.Mesh collisionModel;
#endif

        public WalkingPath path = null;
        public DetailWalkingPath detailPath = null;
        float waitTime = 0;
        public AbsCommand command = null;
        GameTimeStamp enterBattleStateTime = GameTimeStamp.None;

        public SoldierGroup(AbsArmy tArmy, SoldierConscriptProfile conscript, Vector3 startPos)
        {
            //if (tArmy.IsCity())
            //{
            //    lib.DoNothing();
            //}

            this.army =new WeakReference<AbsArmy>( tArmy);
            this.pfaction = tArmy.pfaction;
            soldierConscript = conscript;
            initPart1(tArmy);

            position = startPos;
            goalWp = startPos;
            tilePos = WP.ToTilePos(position);

            initPart2();

            soldierCount = soldierData.UnitCount();
            if (soldierCount <= 0)
            {
                lib.DoNothing();
            }
            soldierData = soldierConscript.createSoldierData();

            initPart3(tArmy);

            if (tArmy.inRender_detailLayer)
            {
                setDetailLevel(true);
            }

            if (pfaction.TryGetPlayer(out var player) && player.IsLocalPlayer())
            {
                player.GetLocalPlayer().statistics.SoldiersRecruited += soldierCount;
            }
        }

        public PSoldierGroup pointer()
        {
            if (army.TryGetTarget(out var target))
            {
                return new PSoldierGroup(target.mapObjPointer(), myIndex);
            }
            return PSoldierGroup.Empty;
        }
        public override PGameObject goPointer()
        {
            if (army.TryGetTarget(out var tarmy))
            {
                return new PGameObject(tarmy.gameobjectType(), GameObjectType.SoldierGroup, pfaction, tarmy.myIndex, myIndex);
            }
            return PGameObject.Empty;
        }
        private void initPart1(AbsArmy tArmy)
        {
#if VISUAL_NODES
            collisionModel = new Graphics.Mesh(LoadedMesh.SelectCircleSolid, position, new Vector3(WalkDirBound.radius * 2f), TextureEffectType.Flat, SpriteName.WhiteArea, Color.HotPink, false);
            collisionModel.AddToRender(DrawGame.UnitDetailLayer);
#endif
            var type = soldierConscript.unitType();
            landBuilder = type;
            shipBuilder = DssRef.units.Get(landBuilder).ShipType();

            soldierData_soldier = soldierConscript.createSoldierData();
            soldierData = soldierData_soldier;
            currentBuilder = landBuilder;

            armyGridPlacement2 = tArmy.nextArmyPlacement(soldierData_soldier.defaultArmyPlacement);

        }

        void initPart2()
        {
            halfColDepth = soldierData.columnsDepth * -0.5f;

            groupRadius = 0.2f;
        }

        void initPart3(AbsArmy tArmy)
        {
            refreshAttackRadius();
            refreshRotateSpeed();

            if (myIndex < 0)
            {
                tArmy.AddSoldierGroup(this);
            }
            rotation = tArmy.rotation;
        }
        public SoldierGroup(AbsArmy army)
        {
            this.army = new WeakReference<AbsArmy>( army);
            pfaction = army.pfaction;
        }
        public SoldierGroup(AbsArmy tArmy, System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
        {
            this.army = new WeakReference<AbsArmy>( tArmy);
            this.pfaction = tArmy.pfaction;
            readGameState(tArmy, r, version, true, true, pointers);
        }

        public void setDetailLevel(bool unitDetailView)
        {
            if (unitDetailView)
            {
                createSoldierObjects(unitDetailView, true);
            }
            else
            {
                if (state == GroupState.Battle)
                {
                    var soldiers_sp = soldiers;
                    if (soldiers_sp != null)
                    {
                        var soldiersC = soldiers_sp.counter();
                        while (soldiersC.Next())
                        {
                            soldiersC.sel.setDetailLevel(unitDetailView);
                        }
                    }
                }
                else
                {
                    createSoldierObjects(unitDetailView, true);
                }
            }
        }

        
        override public SpottedArray<AbsSoldierUnit> Soldiers()
        {
            return soldiers;
        }

        void createSoldierObjects(bool create, bool models)
        {

            if (create)
            {
                if (soldiers == null)
                {
                    if (soldierCount > 0)
                    {
                        createAllSoldiers(currentBuilder, soldierCount, models);
                        if (DssRef.units.IsShip(currentBuilder))
                        {
                            var first = FirstSoldier();
                            if (first != null)
                            {
                               first.health = shipHealth;
                            }
                        }
                    }
                    else
                    {
                        soldiers = new SpottedArray<AbsSoldierUnit>(0);
                    }
                }
                else if (models)
                {
                    var soldiersC = soldiers.counter();
                    while (soldiersC.Next())
                    {
                        soldiersC.sel.setDetailLevel(create);
                    }
                }
            }
            else
            {
                if (soldiers != null)
                {
                    if (DssRef.units.IsShip(currentBuilder))
                    {
                        var first = FirstSoldier();
                        if (first != null)
                        {
                            shipHealth = Bound.Min( first.health, 1);
                        }
                    }
                    deleteAllSoldiers(DeleteReason.CameraCulling);
                }
            }
        }

        public void writeNet(System.IO.BinaryWriter w)
        {
            if (pfaction.TryGetLocalPlayer(out _))
            {
                lib.DoNothing();
            }
            w.Write((byte)state);
            writeGameState(w, state <= GroupState.FindArmyPlacement);

            switch (state)
            {
                case GroupState.CityCapture:
                case GroupState.FindArmyPlacement:
                    WP.WritePosXZPercentU16(w, goalWp);
                    break;
                case GroupState.FollowCommand:
                    WP.WritePosXZPercentU16(w, position);

                    Vector3 goal = Vector3.Zero;

                    var command_sp = command;
                    if (command_sp != null)
                    {
                        if (!command_sp.haltCommand && command_sp.hasPathCommand(out bool towardsUnit))
                        {
                            goal = towardsUnit ? command_sp.AttackTarget().position : command_sp.GoalPosition();
                        }
                    }

                    WP.WritePosXZPercentU16(w, goal);
                    break;
                case GroupState.Battle:
                    WP.WritePosXZPercentU16(w, position);

                    attackTarget_soldierGroupOrCity.write(w);
                    attackTargetTimeLock.write_byte(w);
                    break;
            }

        }
        public void readNet(AbsArmy tArmy, System.IO.BinaryReader r, bool needInit)
        {
            state = (GroupState)r.ReadByte();
            readGameState(tArmy, r, int.MaxValue, needInit, state <= GroupState.FindArmyPlacement, null);
            setGroundY();
            

            //Debug.Log("## Soldiergroup read Net, state: " + state.ToString());
            //Debug.Log($"goal: {goalWp}, pos: {position}" );


            switch (state)
            {
                default:
                    goalWp = position;
                    break;
                case GroupState.CityCapture:
                case GroupState.FindArmyPlacement:
                    {
                        if (WP.ReadPosXZPercentU16_ZeroCheck(r, out var newGoalWp, out _))
                        {
                           goalWp = newGoalWp;
                           armyPlacementWp = goalWp;
                        }
                    }
                    break;
                case GroupState.FollowCommand:
                    {
                        WP.ReadPosXZPercentU16(r, out position, out tilePos);
                        if (VectorExt.PlaneXZDistance(ref position, ref goalWp) > WorldData.SubTileWidth)
                        {
                            goalWp = position;
                        }
                        
                        if (WP.ReadPosXZPercentU16_ZeroCheck(r, out var newGoalWp, out _))
                        {
                            var command_sp = command;
                            if (command_sp == null)
                            {
                                if (VectorExt.PlaneXZDistance(ref position, ref newGoalWp) > WorldData.SubTileWidth)
                                {
                                    command = new NetClientCommand(this, newGoalWp);
                                }
                            }
                            else
                            {
                                command_sp.refreshGoal(newGoalWp);
                            }
                        }
                        else
                        {
                            cancelCommand();
                        }
                    }
                    break;
                case GroupState.Battle:
                    
                    WP.ReadPosXZPercentU16(r, out var rPosition, out tilePos);

                    if (VectorExt.PlaneXZDistance(ref position, ref rPosition) > WorldData.SubTileWidth * 8)
                    {
                        position = rPosition;
                    }
                    attackTarget_soldierGroupOrCity.read(r);
                    attackTargetTimeLock.read_byte(r);
                    break;
            }

            //Debug.Log($"new goal: {goalWp}");

        }

        public void net_onUpdate()
        {
            lastNetUpdate.setNow();
        }

        public void net_updateclient(bool playerDetailView)
        {
            //bool visible = playerDetailView && !lastNetUpdate.secPassed(30);
            //createSoldierObjects(visible, true);
            //updateDetailLevel();

            if (soldiers != null)
            {
                var soldiersC = soldiers.counter();
                while (soldiersC.Next())
                {
                    soldiersC.sel.update_client();
                }
            }
        }

        virtual public void writeGameState(System.IO.BinaryWriter w, bool includePosition)
        {
            soldierConscript.writeGameState(w);
            w.Write(isShip);

            if (includePosition)
            {
                armyGridPlacement2.writeShort(w);
                WP.WritePosXZPercentU16(w, position);
                w.Write(rotation.ByteDir);
            }

            w.Write((byte)soldierCount);
            w.Write(shipHealth);
        }

        virtual public void readGameState(AbsArmy tArmy, System.IO.BinaryReader r, int subVersion, bool needInit, bool includePosition, ObjectPointerCollection pointers)
        {
            soldierConscript.readGameState(r);

            if (needInit)
            {
                initPart1(tArmy);
            }

            isShip = r.ReadBoolean();
            if (isShip)
            {
                lib.DoNothing();
            }
            currentBuilder = isShip ? shipBuilder : landBuilder;

            if (includePosition)
            {
                armyGridPlacement2.readShort(r);

                if (subVersion < 62)
                {
                    WP.readPosXZ_old(r, out position, out tilePos);
                }
                else
                {
                    WP.ReadPosXZPercentU16(r, out position, out tilePos);
                }
                rotation.ByteDir = r.ReadByte();
            }

            soldierCount = r.ReadByte();
            shipHealth = Bound.Min( r.ReadInt32(), 1);

            if (soldierCount <= 0)
            {
                lib.DoNothing();
            }

            if (needInit)
            {
                initPart2();

                initPart3(tArmy);
            }
        }

        public void readGameState_old(AbsArmy tArmy, System.IO.BinaryReader r, int version)
        {
            soldierConscript.readGameState(r);

            initPart1(tArmy);

            bool isShip = r.ReadBoolean();
            currentBuilder = isShip ? shipBuilder : landBuilder;


            armyGridPlacement2.readShort(r);

            bool lockedInArmyGrid = r.ReadBoolean();

            if (lockedInArmyGrid)
            {
                position = tArmy.position;
                rotation = tArmy.rotation;
            }
            else
            {
                WP.readPosXZ_old(r, out position, out tilePos);
                rotation.ByteDir = r.ReadByte();
            }

            var groupObjective = r.ReadByte();

            soldierCount = r.ReadByte();
            if (soldierCount <= 0)
            {
                lib.DoNothing();
            }
            bool soldiersLockedInGroup = groupObjective == 0;//GroupObjective_FollowArmyObjective;

            initPart2();

            //createAllSoldiers(typeCurrentData, soldiersCount);

            if (!soldiersLockedInGroup)
            {
                for (int i = 0; i < soldierCount; i++)
                {
                    AbsSoldierUnit.OldRead(r);
                }
            }

            initPart3(tArmy);
        }

        


        void refreshAttackRadius()
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

            attackRadius += 1f;
            //}
        }

        virtual protected void createAllSoldiers(UnitBuildType type, int count, bool createModels)
        {
            AbsSoldierBuilder builder = DssRef.units.Get(type);

            soldiers = new SpottedArray<AbsSoldierUnit>(count +1);
            soldierData = soldierConscript.createSoldierData();

            int xStart;

            if (builder.IsShip())
            {
                soldierConscript.shipSetup(ref soldierData);
                xStart = 0;
                create(0, 0, false, builder, ref soldierData);
            }
            else
            {
                xStart = -soldierData.rowWidth / 2;
                IntVector2 bannerPos = bannerManPos();

                int columnDepth = MathExt.Div_Ceiling(count, soldierData.rowWidth);


                switch (soldierConscript.conscript.animal)
                {
                    default:
                        for (int y = 0; y < columnDepth; ++y)
                        {
                            for (int x = 0; x < soldierData.rowWidth; ++x)
                            {
                                if (!create(x, y, bannerPos.Equals(x, y), builder, ref soldierData))
                                {
                                    return;
                                }
                            }
                        }
                        break;

                    case ItemResourceType.Dog:
                    case ItemResourceType.Hound:
                    case ItemResourceType.Pig:

                        AbsSoldierBuilder houndbuilder = DssRef.units.Get(UnitBuildType.ConscriptHound);
                        var houndSoldierData = ItemPropertyColl.Get(soldierConscript.conscript.animal).soldierData;

                        if (soldierData.columnsDepth == 1)
                        {
                            for (int x = 0; x < soldierData.rowWidth; ++x)
                            {
                                if (!create(x, 0, bannerPos.Equals(x, 0), lib.IsEven(x) ? builder : houndbuilder, ref soldierData))
                                {
                                    return;
                                }
                            }
                        }
                        else
                        {
                            int houndColumnExMax = soldierData.columnsDepth / 2;

                            for (int y = houndColumnExMax; y < columnDepth; ++y)
                            {
                                for (int x = 0; x < soldierData.rowWidth; ++x)
                                {
                                    if (!create(x, y, bannerPos.Equals(x, y), builder, ref soldierData))
                                    {
                                        return;
                                    }
                                }
                            }

                            
                            for (int y = 0; y < houndColumnExMax; ++y)
                            {
                                for (int x = 0; x < soldierData.rowWidth; ++x)
                                {
                                    if (!create(x, y, false, houndbuilder, ref houndSoldierData))
                                    {
                                        return;
                                    }
                                }
                            }
                           
                        }
                        break;
                }
                
            }

            bool create(int x, int y, bool banner, AbsSoldierBuilder builder, ref SoldierData soldierData)
            {
                AbsSoldierUnit unit = createUnit(builder, new IntVector2(x + xStart, y),
                        banner, tilePos, ref soldierData, createModels);

                if (unit == null)
                {
                    return false;
                }
                else
                {
                    unit.firstUpdate();                    
                }

                return --count > 0;
            }


            //AbsSoldierUnit unit = createUnit(typeProfile, new IntVector2(x + xStart, y), 
            //    bannerPos.Equals(x, y), tilePos, ref soldierData, createModels);

            //if (unit == null)
            //{
            //    return;
            //}
            //else
            //{
            //    unit.firstUpdate();
            //}

            //if (--count <= 0)
            //{
            //    return;
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
            rotateSpeed = (float)Math.Abs(Math.Atan2(walkSpeed_peace * muliply, groupRadius));
        }

        IntVector2 bannerManPos()
        {
            IntVector2 bannerPos;
            if (soldierData.hasBannerMan)//typeCurrentData.hasBannerMan)
            {
                bannerPos = new IntVector2(soldierData.rowWidth / 2, soldierData.columnsDepth - 1);
            }
            else
            {
                bannerPos = IntVector2.NegativeOne;
            }

            return bannerPos;
        }

        virtual public void completeTransform(SoldierTransformType transformType, int positionId)
        {
            if (isDeleted) return;

            if (isShip != (transformType == SoldierTransformType.ToShip))
            {
                isShip = transformType == SoldierTransformType.ToShip;

                if (isShip)
                {
                    shipHealth = soldierData_soldier.basehealth * soldierCount;
                    soldierCount = 1;
                    currentBuilder = shipBuilder;
                    soldierData = soldierConscript.createSoldierData();
                }
                else
                {
                    soldierCount = shipHealth / soldierData_soldier.basehealth;
                    if (soldierCount <= 0)
                    {
                        lib.DoNothing();
                    }
                    currentBuilder = landBuilder;
                    soldierData = soldierConscript.createSoldierData();
                }

                var soldiers_sp = soldiers;
                if (soldiers_sp != null)
                {
                    int totalHealth = 0;

                    var soldiersC = soldiers_sp.counter();
                    while (soldiersC.Next())
                    {
                        totalHealth += soldiersC.sel.health;
                        soldiersC.sel.DeleteMe(DeleteReason.Transform, false);
                    }
                    soldiers_sp.Clear();

                    if (transformType == SoldierTransformType.ToShip)
                    {
                        var shipData = soldierData;
                        soldierConscript.shipSetup(ref shipData);

                        var ship = createUnit(DssRef.units.Get(shipBuilder), IntVector2.Zero, false, WP.ToTilePos(position), ref shipData, true);

                        if (ship != null)
                        {
                            ship.position = position;
                            ship.health = shipHealth;
                            ship.refreshShipCarryCount();
                        }
                    }
                    else
                    {
                        //int count = (int)Math.Ceiling(totalHealth / (double)soldierData.basehealth);
                        shipHealth = Bound.Min( totalHealth, 1);
                        createAllSoldiers(currentBuilder, soldierCount, true);
                    }

                    refreshAttackRadius();
                }

                state = GroupState.FindArmyPlacement;
            }

            inShipOrGuardTransform = false;
        }


        public AbsSoldierUnit createUnit(AbsSoldierBuilder typeProfile, IntVector2 gridPlacement, bool bBannerPos, IntVector2 area, ref SoldierData data, bool models)
        {
            var soldiers_sp = soldiers;

            if (soldiers_sp != null)
            {
                AbsSoldierUnit s;

                s = typeProfile.CreateUnit(bBannerPos);
                s.pfaction = this.pfaction;
                s.unitBuildType = typeProfile.unitBuildType;

                s.isBannerMan = bBannerPos;

                if (bBannerPos && typeProfile.unitBuildType == UnitBuildType.Conscript)
                {
                    s.soldierData = soldierConscript.bannermanSetup(data);
                }
                else
                {
                    s.soldierData = data;
                }

                s.InitLocal(position, gridPlacement, area, this);
                s.position = WP.ToWorldPos(area); //temp pos
                s.myIndex = soldiers_sp.Add(s);

                if (army != null && army.TryGetTarget(out var tArmy) &&
                    tArmy.inRender_detailLayer && models)
                {
                    s.setDetailLevel(true);
                }

                if (soldiers == null)
                {
                    s.DeleteMe(DeleteReason.CameraCulling, false);
                }
                else
                {
                    return s;
                }
                
            }
            return null;
        }

        void walking_Peace(AbsArmy tArmy, float time, Vector3 goalWp, bool induvidualSpeed, out bool complete, ref float groupWalkSpeed)
        {
            waitTime += time;

            bool move = true;

            if (isWalkingIntoOtherGroup)
            {
                //queueu, holding position
                if (waitTime > 10000)
                {
                    waitTime = 0;
                }
                else if (waitTime > 3000)
                {
                    waitTime = -8000;
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
            //else
            //{
            //    waitTime = 0;
            //}

            if (move)
            {
                Vector3 goal = walkingGoalWp(goalWp, out bool waterNode, out bool ready);
                complete = updateWalking(goal, true, true, induvidualSpeed, tArmy.armyGoalRotation, time, out groupWalkSpeed);
                if (ready)
                {
                    if (waterNode != isShip)
                    {
                        if (!inShipOrGuardTransform)
                        {
                            new ShipTransform(this, true);
                        }
                    }
                }
            }
            else
            { 
                complete = false;
            }
        }

        Vector3 walkingGoalWp(Vector3 goalWp, out bool waterNode, out bool pathIsReady)
        {
            var path_sp = detailPath;
            if (path_sp != null)
            {
                pathIsReady = true;
                if (path_sp.NodeCountLeft() > 1)
                {
                    Vector3 result = path_sp.NextNodeWp(position, out bool complete, out waterNode);
                    return result;
                }
            }
            else
            { 
                pathIsReady = false;
            }

            if (DssRef.world.tileGrid.TryGet(tilePos, out var tile))
            {
                waterNode = tile.IsWater();
            }
            else
            {
                waterNode = isShip;
            }
            return goalWp;
        }

        Vector3 walkingGoalAttackTarget(AbsGroup target, out bool waterNode)
        {
            var path_sp = detailPath;
            if (path_sp != null && path_sp.NodeCountLeft() > 1)
            {
                Vector3 result = path_sp.NextNodeWp(position, out bool complete, out waterNode);
                if (waterNode != isShip)
                {
                    if (!inShipOrGuardTransform)
                    {
                        new ShipTransform(this, true);
                    }
                }

                return result;
            }

            
            waterNode = isShip;
            return target.position;
        }

        public void enterBattleState(bool enter, bool localAction, AbsGroup targetObject)
        {
            if (enter != (state == GroupState.Battle))
            {
                if (enter)
                {
                    enterBattleStateTime = GameTimeStamp.Now();

                    state = GroupState.Battle;
                    createSoldierObjects(enter, false);

                    if (localAction && targetObject != null)
                    {
                        var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssEnterBattle, Network.PacketReliability.Reliable, out var packet);
                        {
                            Net.ObjectId.WriteSoldierGroup(w, this);

                            attackTarget_soldierGroupOrCity.write(w);
                            attackTargetTimeLock.write_byte(w);
                            if (attackTarget_soldierGroupOrCity.objectType == GameObjectType.SoldierGroup)
                            {
                                var tsoldiers = (SoldierGroup)targetObject;
                                tsoldiers.writeNet(w);
                            }
                            
                        }
                        packet.EndWrite_Asynch();
                    }
                }
                else
                {
                    if (pfaction.TryGetLocalPlayer(out _))
                    {
                        Debug.Log($">End battle state, time {enterBattleStateTime.Seconds}");
                    }
                    if (!enterBattleStateTime.secPassed(5))
                    {
                        return;
                    }

                    highTargetValueToOpponent = float.MaxValue;
                    state = GroupState.FindArmyPlacement;      
                }

                var soldiers_sp = soldiers;
                if (soldiers_sp != null)
                {
                    var soldiersC = soldiers_sp.counter();
                    while (soldiersC.Next())
                    {
                        soldiersC.sel.enterBattleState(enter);
                    }
                }
            }
        }

        public static void NetReadEnterBattle(System.IO.BinaryReader r)
        {
            //var group = ObjectId.ReadSoldierGroup(r, true, out _);
            PSoldierGroup pSoldierGroup = new PSoldierGroup(r);
            var target = new PGameObject(r);
            GameTimeStamp time = GameTimeStamp.None;
            time.read_byte(r);

            if (target.objectType == GameObjectType.SoldierGroup)
            {
                SoldierGroup targetObject = ObjectId.GetSoldierGroup(
                    target.GetSoldierGroupPointer(),
                    true, out var tarmy);
                targetObject?.readNet(tarmy, r, false);
            }

            var group = ObjectId.GetSoldierGroup(pSoldierGroup, false, out var absArmy);

            if (group != null && group.state != GroupState.Battle)
            {
                group.attackTarget_soldierGroupOrCity = target;
                group.attackTargetTimeLock = time;
                group.enterBattleState(true, false, null);
            }
        }

        public void setAsStartArmy()
        {
            position = goalWp;
            tilePos = WP.ToTilePos(position);
            setGroundY();
        }

        //static readonly float FlankDistAdd = WorldData.SubTileWidth * 0.6f;

        void updateMoveAndAttackTarget(float time, bool fullUpdate, AbsGroup attack_sp, ref float groupWalkSpeed)
        {
            Vector2 diff = new Vector2(
                    attack_sp.position.X - position.X,
                    attack_sp.position.Z - position.Z);

            if (diff.Length() - attack_sp.groupRadius < attackRadius)
            {
                //Attack
                if (soldiers != null)
                {
                    var soldiersC = soldiers.counter();

                    if (Ref.peRnd.Chance(0.1))
                    {
                        Vector3 posSum = Vector3.Zero;
                        int posCount = 0;
                        while (soldiersC.Next())
                        {
                            soldiersC.sel.update2_battle_attack(time, fullUpdate, groupWalkSpeed); //same
                            posSum += soldiersC.sel.position;
                            ++posCount;
                        }

                        if (posCount > 0)
                        {
                            position = posSum / posCount;
                        }
                    }
                    else
                    {
                        while (soldiersC.Next())
                        {
                            soldiersC.sel.update2_battle_attack(time, fullUpdate, groupWalkSpeed); //same
                        }
                    }
                }
            }
            else
            {
                //Battle update
                updateWalking(walkingGoalAttackTarget(attack_sp, out bool shipTransform), true, false, true, 0, time, out groupWalkSpeed);

                if (soldiers != null)
                {
                    var soldiersC = soldiers.counter();
                    while (soldiersC.Next())
                    {
                        soldiersC.sel.update2_battle_move(time, fullUpdate, groupWalkSpeed);

                    }
                }
            }
        }

        void updateStaticAttack(float time, bool fullUpdate, AbsGroup attack_sp)
        {
            Vector2 diff = new Vector2(
                    attack_sp.position.X - position.X,
                    attack_sp.position.Z - position.Z);

            if (diff.Length() - attack_sp.groupRadius < attackRadius)
            {
                //Attack
                if (soldiers != null)
                {
                    var soldiersC = soldiers.counter();
                    while (soldiersC.Next())
                    {
                        soldiersC.sel.update2_battle_attack_static(time, fullUpdate, 0);
                    }
                }
            }
        }

        public void cancelCommand()
        {
            if (command != null)
            {
                if (command.nextCommand != null)
                {
                    command = command.nextCommand;
                    command.begin(this);
                }
                else
                {
                    command = null;
                    if (state == GroupState.FollowCommand)
                    {
                        state = GroupState.GoingIdle;
                    }
                }
            }
        }

        const double CaptureCheckChance = 0.1;
        const float CaptureCheckMulti = (float)(1 / CaptureCheckChance);
        const float CaptureAddPerMs = 0.1f * CaptureCheckMulti;
        const float CaptureDistance = 0.4f;
        
        virtual public void update(float time, bool fullUpdate)
        {
            //if (pfaction.TryGetLocalPlayer(out _))
            //{
            //     lib.DoNothing();
            //}
            //if (fullUpdate && IsGuardGroup())
            //{
            //    lib.DoNothing();
            //}

            if (inShipOrGuardTransform)
            {
                return;
            }

            if (!army.TryGetTarget(out var tArmy) || (soldierCount == 0 && tArmy.IsNetHosted))
            {
                if (fullUpdate)
                {
                    DeleteMe(DeleteReason.EmptyGroup, true);
                }
                else
                {
                    Ref.update.AddSyncAction(new SyncAction2Arg<DeleteReason, bool>(DeleteMe, DeleteReason.EmptyGroup, true));
                }
                return;
            }


            //AbsGroup attack_sp = null;
            //attackTarget_soldierGroupOrCity?.TryGetTarget(out attack_sp);

            var command_sp = command;
            float groupWalkSpeedTime = soldierData.walkingSpeed * time;
            
            if (attackTarget_soldierGroupOrCity.TryGetGroup(out var attack_sp))
            {
                if (state != GroupState.Battle)
                {
                    enterBattleState(true, true, attack_sp);
                }
                enterBattleStateTime.setNow();

                if (command_sp != null)
                {
                    if (Ref.peRnd.Chance(0.1))
                    {

                        Vector2 diff = new Vector2(
                           attack_sp.position.X - position.X,
                           attack_sp.position.Z - position.Z);

                        if (diff.Length() < groupRadius)
                        {
                            cancelCommand();
                        }

                        var goalTarget = command_sp.AttackTarget();
                        if (goalTarget != null && distance(goalTarget) < attackRadius)
                        {
                            attackTarget_soldierGroupOrCity = goalTarget.goPointer();
                            goalTarget.OnBecomeAttackTarget(this);
                            cancelCommand();
                            updateMoveAndAttackTarget(time, fullUpdate, goalTarget, ref groupWalkSpeedTime);
                        }
                    }
                    if (command_sp.hasPathCommand(out bool towardsUnit))
                    {
                        Vector3 nodePos = towardsUnit ? walkingGoalAttackTarget(command_sp.AttackTarget(), out _) : walkingGoalWp(command_sp.GoalPosition(), out _, out _);
                        if (updateWalking(nodePos, true, false, true, 0, time, out groupWalkSpeedTime))
                        {
                            cancelCommand();
                        }

                        if (soldiers != null)
                        {
                            var soldiersC = soldiers.counter();
                            while (soldiersC.Next())
                            {
                                soldiersC.sel.update2_battle_move(time, fullUpdate, groupWalkSpeedTime);
                            }
                        }
                    }

                }
                else
                {
                    if (InGuardPost())
                    {
                        updateStaticAttack(time, fullUpdate, attack_sp);
                    }
                    else
                    {
                        updateMoveAndAttackTarget(time, fullUpdate, attack_sp, ref groupWalkSpeedTime);
                    }
                }
            }
            else //No attack target is found
            {
                
                

                switch (state)
                {
                    case GroupState.Battle:
                        {
                            if (pfaction.TryGetLocalPlayer(out _))
                            {
                                Debug.Log($"Exit battle, time {enterBattleStateTime.Seconds}");
                            }
                            if (enterBattleStateTime.secPassed(fullUpdate? 5 :20))
                            {
                                //Capture city here
                                if (tArmy.IsArmy())
                                {
                                    if (DssRef.world.tileGrid.TryGet(tilePos, out var tile))
                                    {
                                        var city = tile.City();
                                        if (DssRef.world.diplomacy.GetRelation(tArmy.pfaction, city.pfaction).InWar())
                                        {
                                            if (city.tilePos.SideLength(tilePos) <= 2 || tArmy.GetArmy().attackTarget == city)
                                            {
                                                goalWp = WP.ToWorldPos(city.tilePos);
                                                state = GroupState.CityCapture;
                                                return;
                                            }
                                        }
                                    }
                                }

                            
                                enterBattleState(false, true, null);
                            }
                            
                        }
                        break;

                    case GroupState.CityCapture:
                        if (command == null)
                        {
                            walking_Peace(tArmy, time, goalWp, true, out bool complete, ref groupWalkSpeedTime);

                            if (Ref.peRnd.Chance(0.1))
                            {
                                if (DssRef.world.tileGrid.TryGet(tilePos, out var tile))
                                {
                                    var city = tile.City();
                                    if (DssRef.world.diplomacy.GetRelation(tArmy.pfaction, city.pfaction).InWar())
                                    {
                                        goalWp = WP.ToWorldPos(city.tilePos);

                                        if (VectorExt.PlaneXZLength(goalWp - position) < CaptureDistance)
                                        {
                                            city.capturePoints += CaptureAddPerMs * time;
                                        }

                                    }
                                    else
                                    {
                                        goalWp = armyPlacementWp;
                                        state = GroupState.Battle;
                                    }
                                }
                            }

                            goto UpdateInduviduals;
                        }
                        break;
                }

                if (command_sp != null)
                {
                    if (command_sp.hasPathCommand(out bool towardsUnit))
                    {
                        walking_Peace(tArmy, time, towardsUnit? command_sp.AttackTarget().position : command_sp.GoalPosition(), false, out bool complete, ref groupWalkSpeedTime);
                        if (complete)
                        {
                            command_sp.OnMovePathComplete(this);
                        }
                        state = GroupState.FollowCommand;
                    }
                    else if (command_sp.haltCommand)
                    {
                        if (state != GroupState.Idle)
                        {
                            state = GroupState.GoingIdle;
                            waitTime += time;
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
                            walking_Peace(tArmy, time, goalWp, false, out bool complete, ref groupWalkSpeedTime);
                            if (complete)
                            {
                                state = GroupState.GoingIdle;
                                waitTime = 0;
                            }
                            break;

                        case GroupState.GoingIdle:
                            waitTime += time;
                            break;
                    }
                }

            UpdateInduviduals:
                bool allIdle = true;

            
                if (state == GroupState.Idle)
                {
                    //Passive check of souroundings
                    if (Ref.peRnd.ChanceF(0.2f))
                    {
                        var soldiers_sp = soldiers;
                        if (soldiers_sp != null)
                        {
                            var soldiersC = soldiers_sp.counter();
                            while (soldiersC.Next())
                            {
                                soldiersC.sel.updateGroudY(true);

                                soldiersC.sel.model?.update(soldiersC.sel);
                            }
                        }
                    }
                }
                else
                {
                    var soldiers_sp = soldiers;
                    if (soldiers_sp != null)
                    {
                        var soldiersC = soldiers_sp.counter();
                        while (soldiersC.Next())
                        {
                            soldiersC.sel.update2(time, fullUpdate, groupWalkSpeedTime);
                            allIdle &= soldiersC.sel.state2 == SoldierState2.idle;
                        }
                    }
                }

                if (allIdle &&
                    HasIdleCommand() &&
                    state == GroupState.GoingIdle &&
                    waitTime >= 5000)
                {
                    state = GroupState.Idle;
                }
            }
        }

        public void clientPauseUpdate()
        {
            var soldiers_sp = soldiers;
            if (soldiers_sp != null)
            {
                var soldiersC = soldiers_sp.counter();
                while (soldiersC.Next())
                {
                    soldiersC.sel.clientPauseUpdate();
                }
            }
        }



        public override void OnBecomeAttackTarget(AbsGroup attacker)
        {
            enterBattleState(true, true, attacker);
        }

        public bool HasIdleCommand()
        {
            var command_sp = command;
            if (command_sp != null)
            {
                return command_sp.haltCommand;
            }
            return true;
        }

        public bool HasIdleState()
        {
            var command_sp = command;
            if (command_sp != null)
            {
                return command_sp.haltCommand;
            }

            return state == GroupState.Idle || state == GroupState.GoingIdle;
        }


        void SoldiersPresentationHud(ObjectHudArgs args, bool tooltipOrGroup, bool compact)
        {
            var faction = pfaction.GetFaction();
            //if (faction == null)
            //{ return; }



            if (pfaction != args.player.pfaction &&
                args.player.gameControls.map.selection.obj != null &&
                args.player.gameControls.map.selection.obj.IsSoldiers() &&
                !DssRef.world.diplomacy.GetRelation(pfaction, args.player.pfaction).InWar())
            {
                args.content.Add(new RbImage(SpriteName.RedErrorCross));
                args.content.hspace();
                args.content.Add(new RbText(DssRef.lang.Battle_DeclarWarReminder, HudLib.NotAvailableColor));
                args.content.Add(new RbSeperationLine());
            }

            args.content.Add(new RbBeginTitle(tooltipOrGroup? 2 : 1));
            args.content.Add(faction.FlagTextureToHud());
            args.content.hspace();

            if (pfaction != args.player.pfaction)
            {
                IconName.Relation(DssRef.world.diplomacy.GetRelation(pfaction, args.player.pfaction).Relation, out SpriteName relIcon, out string relName);
                args.content.Add(new RbImage(relIcon));
                args.content.space();
            }

            TypeIcon(args.content);
            args.content.hspace();
            args.content.Add(new RbText(soldierConscript.conscript.TypeName(), tooltipOrGroup ? HudLib.TitleColor_TypeName : HudLib.TitleColor_Head));

            args.content.space();
            args.content.Add(new RbText(string.Format(DssRef.lang.UnitId, myIndex), HudLib.SecondaryTextColor));


            if (args.selected && IsArmyGroup() && pfaction == args.player.pfaction && army.TryGetTarget(out var tArmy))
            {
                RichBoxContent armyContent = new RichBoxContent();
                tArmy.toButtonContent(armyContent, false);
                args.content.Add(new ArtButton(RbButtonStyle.Outline, armyContent,
                    new RbAction1Arg<AbsArmy>(args.player.gameControls.mapSelect, tArmy), new RbTooltip_Text(DssRef.lang.Tutorial_SelectInput)));
            }

            args.content.newLine();
            soldierConscript.conscript.toHud(args.content, compact);
            args.content.newLine();
            
            args.content.Add(new RbImage(SpriteName.WarsStrengthIcon));
            args.content.hspace();
            args.content.Add(new RbText(TextLib.TwoDecimal(strengthValue())));

            args.content.space(2);
            args.content.Add(new RbImage(SpriteName.WarsMobilityIcon));
            args.content.hspace();
            args.content.Add(new RbText(TextLib.TwoDecimal(mobilityValue())));
        }

        public override void toTooltip(ObjectHudArgs args)
        {
            SoldiersPresentationHud(args, true, true);
        }

        public override void toHud(ObjectHudArgs args)
        {
            if (!army.TryGetTarget(out var tArmy))
            {
                return;
            }

            SoldiersPresentationHud(args, false, false);
            args.content.Add(new RbSeperationLine());

            soldierData.StatsToHud(args.content);
            args.content.Add(new RbSeperationLine());
            int tabSel = 0;

            var tabs = new List<ArtTabMember>((int)MenuTab.NUM_NONE);

            List<MenuTab> availableTabs = args.player.AvailableArmyTabs();
            for (int i = 0; i < availableTabs.Count; ++i)
            {
                var text = new RbText(LangLib.Tab(availableTabs[i], out string description, out _));
                text.overrideColor = HudLib.RbSettings.tabSelected.Color;

                AbsRbAction enter = null;
                if (description != null)
                {
                    enter = new RbAction(() =>
                    {
                        RichBoxContent content = new RichBoxContent();
                        content.text(description).overrideColor = HudLib.InfoYellow_Light;

                        args.player.hud.tooltip.create(args.player, content, true);
                    });
                }

                tabs.Add(new ArtTabMember(new List<AbsRichBoxMember>
                            {
                                text
                            }, enter));

                if (availableTabs[i] == args.player.armyTab)
                {
                    tabSel = i;
                }
            }
#if DEBUG
            args.content.newLine();
            debugTagButton(args.content);
#endif
            if (tArmy.IsArmy())
            {
                //HudLib.Label(args.content, DssRef.lang.ArmyStructure_ColumnWidth);
                //args.content.newLine();
                //for (int w = Army.MinColumnWidth; w <= Army.MaxColumnWidth; w += 2)
                //{
                //    var button = new ArtOption(w == tArmy.armyColumnWidth, 
                //        new List<AbsRichBoxMember> { new RbText(w.ToString()) },
                //        new RbAction1Arg<int>(tArmy.armyColumnWidthClick, w, RbSoundType.Option));

                //    args.content.Add(button);
                //}
                ArmyMenu.ColumnWidth(args.content, tArmy);

                args.content.newParagraph();

                HudLib.Label(args.content, DssRef.lang.ArmyStructure_ArmyPlacement);
                args.content.Add(new RbSeperationLine());

                for (int y = 0; y < ArmyPlacementGrid.RowsCount; y++)
                {
                    int rowY = y - ArmyPlacementGrid.PosYAdd;

                    string name;
                    switch (rowY)
                    {
                        case ArmyPlacementGrid.Row_Front:
                            name = DssRef.lang.ArmyStructure_Row_Front;
                            break;
                        default:
                            name = DssRef.lang.ArmyStructure_Row_Body;
                            break;
                        case ArmyPlacementGrid.Row_Second:
                            name = DssRef.lang.ArmyStructure_Row_Second;
                            break;
                        case ArmyPlacementGrid.Row_Behind:
                            name = DssRef.lang.ArmyStructure_Row_Behind;
                            break;

                    }

                    args.content.newLine();
                    args.content.Add(new RbText(name, HudLib.TitleColor_TypeName));
                    args.content.Add(new RbTab(0.3f));
                    for (int x = 0; x < ArmyPlacementGrid.ColsCount; x++)
                    {
                        //args.content.space();

                        int colX = x - ArmyPlacementGrid.PosXAdd;

                        string caption = colX == 0 ? " C " : TextLib.PlusMinus(colX);
                        var button = new ArtToggle(armyGridPlacement2.X == colX && armyGridPlacement2.Y == rowY, new List<AbsRichBoxMember> {
                        new RbText(caption)
                    },
                        new RbAction2Arg<int, int>(setNewArmyPlacement, colX, rowY, RbSoundType.Option), null);

                        args.content.Add(button);
                    }

                }
                args.content.Add(new RbSeperationLine());
            }
            
            args.content.newLine();

            if (soldierConscript.conscript.weapon == ItemResourceType.Settler)
            {
                ArmyMenu.settlerButton(args.player, args.content, this);
            }
        }

        void setNewArmyPlacement(int colX, int rowY)
        {
            armyGridPlacement2.X = colX;
            armyGridPlacement2.Y = rowY;

            if (army.TryGetTarget(out var tArmy))
            {
                tArmy.GetArmy().refreshPositions(false);
            }
        }

        //public bool soldiersShouldFollowWalkingOrder()
        //{
        //    return groupObjective == GroupObjective_FollowArmyObjective;//hasWalkingOrder && attacking.Count == 0;
        //}

        //public void EnterPeaceEvent()
        //{
        //    if (pfaction.TryGetLocalPlayer(out _))
        //    {
        //        lib.DoNothing();
        //    }
        //    attackTarget_soldierGroupOrCity =  PGameObject.Empty;
        //    //groupObjective = GroupObjective_IsSplit;
        //}

        //void refreshGroupPositions()
        //{
        //    attacking_soldierGroupOrCity = null;

        //    //Refresh placements
        //    {
        //        //IntVector2 bannerPos = bannerManPos();
        //        //AbsSoldierUnit bannerMan = null;
        //        //var typeData = DssRef.unitsdata.Get(type);

        //        IntVector2 nextPos = IntVector2.Zero;
        //        //int bannerLead = -1;
        //        int xStart = -typeCurrentData.rowWidth / 2;

        //        var soldiersC = soldiers.counter();
        //        AbsSoldierUnit[] leadRow = new AbsSoldierUnit[typeCurrentData.rowWidth];

        //        while (soldiersC.Next())
        //        {
        //            soldiersC.sel.clearAttack();
        //            //if (soldiersC.sel.DetailUnitType() == UnitType.BannerMan)
        //            //{
        //            //    bannerMan = soldiersC.sel;
        //            //}
        //            //else
        //            //{
        //                soldiersC.sel.gridPlacement.X = nextPos.X + xStart;
        //                soldiersC.sel.gridPlacement.Y = nextPos.Y;
        //                soldiersC.sel.refreshGroupOffset();

        //                //if (nextPos.Y > 0)
        //                //{
        //                //    soldiersC.sel.following = leadRow[nextPos.X].parentArrayIndex;
        //                //}
        //                leadRow[nextPos.X] = soldiersC.sel;

        //                //if (nextPos.X == bannerPos.X)
        //                //{
        //                //    bannerLead = soldiersC.sel.parentArrayIndex;
        //                //}

        //            //    do
        //            //    {
        //            //        if (++nextPos.X >= typeCurrentData.rowWidth)
        //            //        {
        //            //            nextPos.X = 0;
        //            //            nextPos.Y++;
        //            //        }
        //            //    }
        //            //    while (nextPos == bannerPos);
        //            //}
        //        }

        //        //if (bannerMan != null)
        //        //{
        //        //    //bannerMan.following = bannerLead;
        //        //    if (bannerLead == -1)
        //        //    {
        //        //        bannerMan.gridPlacement.Y = 0;
        //        //    }
        //        //    else
        //        //    {
        //        //        bannerMan.gridPlacement = soldiers.Array[bannerLead].gridPlacement;
        //        //        bannerMan.gridPlacement.Y++;
        //        //    }

        //        //    bannerMan.refreshGroupOffset();
        //        //}
        //    }

        //    //if (fullUpdate)
        //    //{
        //    //    //if (!hasWalkingOrder)
        //    //    //{
        //    //    //    armyPathGoal = armyPlacement(WP.ToWorldPos(army.tilePos));

        //    //    //    position = armyPathGoal;
        //    //    //    rotation = army.rotation;
        //    //    //    hasWalkingOrder = true;
        //    //    //    walkingOrderTo = army.tilePos;
        //    //    //}


        //    //}
        //    //else
        //    //{
        //    //    setGroupLock();
        //    //}
        //}

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
        bool updateWalking(Vector3 walkTowards, bool walk, bool rotate, bool induvidualSpeed, Rotation1D finalRotation, float time, out float speed)
        {
            Vector2 diff = new Vector2(
                walkTowards.X - position.X,
                walkTowards.Z - position.Z);

            //float speed;

            if (induvidualSpeed)
            {
                speed = soldierData.walkingSpeed * terrainSpeedMultiplier * time;
            }
            else
            {
                speed = walkSpeed_peace * terrainSpeedMultiplier * time;
            }
            float l = diff.Length();
            if (l > Math.Max(speed, GoalCompleteDistance))
            {
                Rotation1D dir = Rotation1D.FromDirection(diff);
                if (rotateTowardsAngle(dir, time))
                {
                    if (walk)
                    {
                        Vector2 move = VectorExt.SetLength(diff, speed);
                        position.X += move.X;
                        position.Z += move.Y;
                    }
                }
            }
            else if (l > walkSpeed_peace)
            {
                position.X = walkTowards.X;
                position.Z = walkTowards.Z;
            }
            else if (rotate)
            {
                //final adjust when reached goal
                if (command != null && command.goalRotation != float.MinValue)
                {
                    if (rotateTowardsAngle(command.goalRotation, time))
                    {
                        return true;
                    }
                }
                else
                {
                    if (rotateTowardsAngle(finalRotation, time))
                    {
                        return true;
                    }
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


            var detailPath_sp = detailPath;
            if (detailPath_sp != null && detailPath_sp.HasMoreNodes())
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

        //bool updateWalkingGoal(out Vector3 walkTowards)
        //{
        //    //const float WarDeclareDistance = 0.2f;

        //    //bool warDeclared = attacking!= null;

        //    //var closest_sp = attacking;//closestOpponent; //Safe pointer
        //    //if (closest_sp != null)
        //    //{
        //    //    float dist = groupCollisionDistance(closest_sp);

        //    //    float warDist = 0.2f;
        //    //    if (army.ai.objective == ArmyObjective.Attack &&
        //    //        army.ai.attackTarget.faction == closest_sp.Faction())
        //    //    {
        //    //        warDist = 1.5f;
        //    //    }
        //    //    //freeAttackAggression = true;
        //    //    //}

        //    //    warDeclared = dist <= warDist;
        //    //}

        //    var attack_sp = attacking_soldierGroupOrCity;

        //    bool attackAggression= attack_sp != null;

        //    //if (army.ai.objective == ArmyObjective.None)
        //    //{
        //    //    attackAggression = warDeclared || attacking.Count > 0 || closestFriendInBattle != null;
        //    //}
        //    //else
        //    //{
        //    //    //bool reachedGoal = army.ai.walkGoal.SideLength(tilePos)
        //    //    attackAggression = attacking.Count > 0;                
        //    //}

        //    if (aggression == AggressionCommand.Hold)
        //    {
        //        attackAggression = false;
        //    }

        //    if (attackAggression)
        //    {
        //        //var closest_sp = closestOpponent; //Safe pointer
        //        //if (closest_sp != null)
        //        //{
        //            walkTowards = attack_sp.position;
        //            return true;
        //        //}

        //        //var friendly_sp = closestFriendInBattle;
        //        //if (friendly_sp != null)
        //        //{
        //        //    walkTowards = friendly_sp.position;
        //        //    return true;
        //        //}

        //    }
        //    walkTowards = goalWp;
        //    return true;//hasWalkingOrder;
        //}



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

        //public void addAttackTarget(AbsGroup newTarget)
        //{
        //    refreshAttackTarget();

        //    if (!newTarget.defeatedBy(army.faction) && newTarget != attackTarget_soldierGroupOrCity)
        //    {
        //        if (attackTarget_soldierGroupOrCity != null)
        //        {
        //            //Compare distance
        //            if (distanceValueTo(attackTarget_soldierGroupOrCity, float.MaxValue) <= distanceValueTo(newTarget, float.MaxValue))
        //            {
        //                return;
        //            }
        //        }
        //        attackTarget_soldierGroupOrCity = newTarget;
        //    }
        //}

        void recyclePath(bool large, bool detail, int pathThreadIndex)
        {
            if (large && path != null)
            {
                //lock (DssRef.state.pathFindingPool)
                //{
                    DssRef.state.pathUpdates[pathThreadIndex].pathFindingPool.Return(path);
                    path = null;
                //}
            }

            if (detail && detailPath != null)
            {
                //lock (DssRef.state.detailPathFindingPool)
                //{
                DssRef.state.pathUpdates[pathThreadIndex].detailPathFindingPool.Return(detailPath);
                    detailPath = null;
                //}
            }
        }

        void refreshAttackTarget()
        {
            if (attackTarget_soldierGroupOrCity.TryGetGroup(out var target) &&                    

                (target.defeated() || 
                !DssRef.world.diplomacy.GetRelation(pfaction, target.pfaction).InWar() ||
                distance(target) > 4)
            )
            {
                attackTargetTimeLock = GameTimeStamp.None;
                attackTarget_soldierGroupOrCity = PGameObject.Empty;

                if (pfaction.TryGetPlayer(out var player) && player.IsRemotePlayer())
                {
                    lib.DoNothing();
                }
            }
        }


        public void asynchNearObjectsUpdate()
        {
            if (debugTagged)
            {
                lib.DoNothing();
            }
            if (pfaction.TryGetLocalPlayer(out _))
            {
                lib.DoNothing();
            }

            if (!army.TryGetTarget(out var tArmy))
            { return; }

            if (Ref.peRnd.Chance(0.05))
            {
                highTargetValueToOpponent = float.MaxValue;
            }
                        
            refreshAttackTarget();

            if (attackTarget_soldierGroupOrCity.HasValue() && attackTargetTimeLock.HasTime())//attackTargetTimeLock > Ref.TotalGameTimeSec)
            {
                return;
            }

            DssRef.world.unitCollAreaGrid.collectOpponentGroups(pfaction, tilePos, out  List<GameObject.SoldierGroup> groups, out List<City> cities);

            AbsGroup nearest = null;
            float distanceValue = float.MaxValue;

            foreach (var opponent in groups)
            {
                var group = opponent?.GetGroup();
                if (group != null &&
                    group.soldierCount > 0 && 
                    opponent.army.TryGetTarget(out var tOpponentArmy))
                {
                    var dist = distanceValueTo(group, aggroRange(tOpponentArmy));

                    if (dist < distanceValue)
                    {
                        distanceValue = dist;
                        nearest = group;
                    }
                }

            }

            if (nearest == null)
            {
                foreach (var opponent in cities)
                {
                    //if (opponent.guardCount > 0)
                    {
                        var score = distanceValueTo(opponent, aggroRange(opponent));

                        if (score < distanceValue)
                        {
                            distanceValue = score;
                            nearest = opponent;
                        }
                    }
                }
            }

            if (nearest != null)
            {
                var pNearest = nearest.goPointer();
                //var target = RefExt.Target_safe(attackTarget_soldierGroupOrCity);
                if (!nearest.defeatedBy(pfaction) && pNearest != attackTarget_soldierGroupOrCity)
                {
                    if (attackTarget_soldierGroupOrCity.TryGetGroup(out var target))//target != null)
                    {
                        //Compare distance
                        if (distanceValueTo(target, float.MaxValue) * 2f <= distanceValueTo(nearest, float.MaxValue))
                        {
                            return;
                        }
                    }

                    attackTargetTimeLock.setTimeFromNow(2f + distanceValue);//Ref.TotalGameTimeSec + 2f + distanceValue;
                    attackTarget_soldierGroupOrCity =pNearest;
                    nearest.OnBecomeAttackTarget(this);
                }
            }


            float aggroRange(AbsMapObject target)
            {
                if (tArmy.IsArmy() &&
                    target == tArmy.GetArmy().attackTarget)
                {
                    return 4.5f;
                }
                return 3.5f;
            }

            command?.asyncUpdate(this);
        }

        float distanceValueTo(AbsGroup toGroup, float maxRange)
        {
            Vector2 diff = new Vector2(toGroup.position.X - this.position.X, toGroup.position.Z - this.position.Z);

            float l = diff.Length();
            if (l > maxRange)
            {
                return float.MaxValue;
            }

            const float AngleWeight = 30f;

            float dir = lib.V2ToAngle(diff);
            float aDiff = Rotation1D.AngleDifference_Absolute(rotation.radians, dir);
            float anglePercDiff = aDiff / MathF.PI;

            float rawValue = l + l * anglePercDiff * AngleWeight;
            float value = rawValue;

            if (rawValue <= toGroup.highTargetValueToOpponent)
            {
                toGroup.highTargetValueToOpponent = rawValue;
                toGroup.highTargetValueToOpponent_tagId = this.myIndex;
            }
            else if (toGroup.highTargetValueToOpponent_tagId != this.myIndex)
            {
                value *= 2;
            }

            if ( toGroup.goPointer() == attackTarget_soldierGroupOrCity)
            {
                value *= 0.5f;
            }

            return value;
        }

        void groupToGroupCollsionUpate_async(int pathThreadIndex)
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
                List<AbsArmy> ArmiesColl_asyncupdate = DssRef.state.pathUpdates[pathThreadIndex].ArmiesColl_asyncupdate;
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

        public void asyncBattleUpdate(ref InBattleWith battles)
        {
            if (state == GroupState.Battle)
            {
                battles.groupsInBattle++;

                var target_sp = attackTarget_soldierGroupOrCity;
                if (target_sp.TryGetGroup(out var tMapObj))
                {
                    battles.factions.TryAddIfNotContains(tMapObj.pfaction);
                    battles.attackingCity |= tMapObj.IsGuardGroup();
                }
            }
            
            var soldiers_sp = soldiers;        
            if (soldiers_sp != null)
            {
                var counter = soldiers_sp.counter();
                while (counter.Next())
                {
                    counter.sel.asyncBattleUpdate();                    
                }
            }
            tilePos = WP.ToTilePos(position);
            if (DssRef.world.tileBounds.IntersectPoint(tilePos))
            {
                position.Y = DssRef.world.tileGrid.Get(tilePos).GroundY();
            }
        }

        public void asyncPathUpdate(int pathThreadIndex)
        {
#if VISUAL_NODES
            collisionModel.Visible = false; 
#endif

            //AbsGroup attack_sp = null;
            attackTarget_soldierGroupOrCity.TryGetGroup(out var attack_sp);
            var command_sp = command;

            if (command_sp != null && command_sp.hasPathCommand(out bool towardsUnit))
            {
                if (command_sp.clearOldPath)
                {
                    command_sp.clearOldPath = false;
                    recyclePath(true, true, pathThreadIndex);
                }

                if (command_sp.haltCommand)
                {
                    return;
                }
                else if (towardsUnit)
                {
                    pathTowardsTarget(command_sp.AttackTarget());
                }
                else
                {
                    pathTowardsPosition(command_sp.GoalPosition());
                }
            }
            else if (attack_sp != null)
            {
                if (InGuardPost())
                {
                    recyclePath(true, true, pathThreadIndex);
                }
                else
                {
                    pathTowardsTarget(attack_sp);
                }
            }
            else
            {
                if (army.TryGetTarget(out var tArmy))
                {
                    if (followsGoalId != tArmy.goalId)
                    {
                        followsGoalId = tArmy.goalId;

                        recyclePath(true, true, pathThreadIndex);

                        if (state <= GroupState.GoingIdle)
                        {
                            state = GroupState.FindArmyPlacement;
                            wakeupSoldiers();
                        }
                    }

                    if (state != GroupState.Idle)
                    {
                        pathTowardsPosition(goalWp);
                    }
                }
            }

            void pathTowardsTarget(AbsGroup target)
            {
                if (soldiers != null)
                {
                    tilePos = WP.ToTilePos(position);
                    setGroundY();

                    IntVector2 goalSubTile = WP.ToSubTilePos(target.position);
                    var detailPath_sp = detailPath;
                    if (detailPath_sp == null || detailPath_sp.goal != goalSubTile)
                    {
                        pathCalulate_detail(goalSubTile, true, pathThreadIndex);
                    }
                }
            }

            void pathTowardsPosition(Vector3 goalWp)
            {
                if (DssRef.world.unitBounds.IntersectPoint(position.X, position.Z) &&
                    DssRef.world.unitBounds.IntersectPoint(goalWp.X, goalWp.Z))
                {
                    tilePos = WP.ToTilePos(position);
                    setGroundY();
                    groupToGroupCollsionUpate_async(pathThreadIndex);

                    const float DetailMaxLength = 3.5f;
                    Vector3 diff = goalWp - position;
                    float l = VectorExt.PlaneXZLength(diff);
                    IntVector2 goalSubTile;
                    bool isTravelNode = false;
                    if (l > DetailMaxLength)
                    {
                        if (path == null)
                        {
                            pathCalulate(pathThreadIndex, goalWp);
                        }

                        //pick three tiles ahead
                        var path_sp = path;
                        if (path_sp != null)
                        {
                            IntVector2 aheadPathTile = path_sp.getNodeAhead(3, tilePos, out isTravelNode);
                            goalSubTile = WP.ToSubTilePos_Centered(aheadPathTile);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        goalSubTile = WP.ToSubTilePos(goalWp);
                    }

                    if (l >= WorldData.SubTileWidth &&
                        (detailPath == null || detailPath.goal != goalSubTile))
                    {
                        pathCalulate_detail(goalSubTile, isTravelNode, pathThreadIndex);
                    }
                }
            }
        }

        void pathCalulate(int pathThreadIndex, Vector3 goalWp)
        {
            recyclePath(true, false, pathThreadIndex);

            PathFinding pf = DssRef.state.pathUpdates[pathThreadIndex].pathFindingPool.GetPf();
            { 
                path = pf.FindPath(pathThreadIndex, tilePos, conv.ToDir8_INT(rotation), WP.ToTilePos( goalWp), isShip);
            }
            DssRef.state.pathUpdates[pathThreadIndex].pathFindingPool.Return(pf);
        }


        void pathCalulate_detail(IntVector2 goal, bool isTravelNode, int pathThreadIndex)
        {
            //Path towards army end position
            //make a big path towards end pos
            //make detail path 3 tiles long at a time
            
            //bool endAsShip = DssRef.world.tileGrid.Get(army.adjustedWalkGoal).IsWater();

            if (!army.TryGetTarget(out var tArmy) || position.X <= 0)
            {
                return;
            }

            recyclePath(false, true, pathThreadIndex);

            DetailPathFinding pf = DssRef.state.pathUpdates[pathThreadIndex].detailPathFindingPool.GetPf();
            {
                detailPath = pf.FindPath(pathThreadIndex, WP.ToSubTilePos(position), rotation, goal,
                    isShip, tArmy.walkGoalAsShip, isTravelNode);
            }
            DssRef.state.pathUpdates[pathThreadIndex].detailPathFindingPool.Return(pf);

            var detailPath_sp = detailPath;

            if (detailPath_sp != null && detailPath_sp.blockedPath)
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

        virtual public float GroupMoveBoundRadius()
        {
            return DssVar.SoldierGroup_Spacing_Radius;
        }

        //void setObjective(int objective)
        //{
        //    if (objective != groupObjective)
        //    {
        //        if (debugTagged)
        //        {
        //            Debug.Log("New Objective (" + TypeName() + "): " +
        //                groupObjective.ToString() + " > " + objective.ToString());
        //        }

        //        groupObjective = objective;
        //    }
        //}

        //public void DrawOverviewIcon(int cameraIndex)
        //{

        //}

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

        //public void bumpWalkToNode(IntVector2 nodePos)
        //{
        //    //walkingOrderTo = nodePos;
        //    Vector3 areaCenter = WP.ToWorldPos(army.nextNodePos);
        //    goalWp = armyPlacement(areaCenter);
        //}

        virtual public void setGroundY()
        {
            if (DssRef.world.tileGrid.TryGet(tilePos, out Tile tile))
            {
                position.Y = tile.GroundY_aboveWater();
            }
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
            //Debug.CrashIfThreaded();

            if (soldiers != null)
            {
                soldiers.RemoveAt_EqualSafeCheck(soldier, soldier.myIndex);
                soldierCount = soldiers.Count;

                if (soldiers.Count <= 0)
                {
                    DeleteMe(DeleteReason.EmptyGroup, true);
                }
            }
        }

        public void removeOneSoldier()
        {
            if (--soldierCount <= 0)
            {
                DeleteMe(DeleteReason.EmptyGroup, true);
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
                double keep = 0.8;

                if (deserter && closestCity.pfaction == this.pfaction)
                { 
                    keep = 0.5;
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

            if (army.TryGetTarget(out var tArmy) && tArmy.inRender_detailLayer)
            {
                Vector3 moveDir_dir = VectorExt.V2toV3XZ(dir);

                var soldiers_sp = soldiers;

                if (soldiers_sp != null)
                {
                    var soldiersC = soldiers_sp.counter();
                    while (soldiersC.Next())
                    {
                        new DeserterAnimation(soldiersC.sel, moveDir_dir, rot);
                    }
                }
            }
        }

        public override void selectionFrame(LocalPlayer player, bool hover, Selection selection)
        {
            //Vector3 scale = new Vector3(radius * 2f);

            var soldiers_sp = soldiers;

            if (soldiers_sp != null)
            {
                var soldiersC = soldiers_sp.counter();
                int i = 0;

                selection.groupModels_detail.BeginGroupModel();
                while (soldiersC.Next())
                {
                    soldiersC.sel.selectionFramePlacement(out var pos, out var scale);
                    selection.groupModels_detail.setGroupModel(i, pos, scale, hover, true, false);
                    ++i;
                }

                var target_sp = GetAttackTarget();
                if (player.pfaction == pfaction && target_sp != null)
                {
                    selection.TargetLine(ref position, ref target_sp.position);
                }
                else
                {
                    selection.hideTargetLine();
                }

                if (HasIdleState())
                {
                    selection.viewGroupPath(null);
                }
                else
                {
                    selection.viewGroupPath(detailPath);
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
            return AllUnits.GroupStrengh(soldierCount, ref soldierData, !isShip);   
        }

        public float mobilityValue()
        {
            return soldierData.mobilityValue();
        }

        public AbsSoldierUnit FirstSoldier()
        {
            return soldiers?.First();
        }

        public override void AddDebugTag()
        {
            base.AddDebugTag();
            RefExt.Target( army)?.AddDebugTag();
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

        public void Upkeep(bool casual, ref SoldierUpkeep upkeep, ref float moneyCarry)
        {
            float upkeepCount = soldierData.upkeepMultiplier * soldierCount;

            upkeep.food += upkeepCount * DssRef.storage.ruleset_instance.manFoodUpkeep +
                 soldierData.animalFoodUpkeep(soldierCount);//soldierData.animalFoodMultiplier * DssRef.difficulty.mountFoodUpkeep;

            if (casual)
            {
                upkeep.copper += upkeepCount * DssConst.CasualSoldierDefaultCost_Copp;
            }
            else
            {
                upkeep.copper += upkeepCount * soldierConscript.conscript.copperUpkeepPerSoldier();//soldierData.copperUpkeepPerSoldier;
            }
            moneyCarry += soldierCount * DssConst.MoneyCarryPerSoldier;
        }

        //public static float copperUpkeepPerSoldier(ConscriptProfile conscript)
        //{
        //    var result = DssConst.TrainingGoldUpkeep[(int)conscript.training];
        //    if (conscript.man == ItemResourceType.NobelMen)
        //    {
        //        result += DssConst.Nobel_GoldUpkeep;
        //    }
        //}

        public override void DeleteMe(DeleteReason reason, bool removeFromParent)
        {
            isDeleted = true;
            if (debugTagged)
            {
                lib.DoNothing();
            }
            if (army.TryGetTarget(out var tArmy2) && tArmy2.IsNetHosted)
            {
               
            }

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

            if (removeFromParent &&
                army.TryGetTarget(out var tArmy))
            {
                tArmy.remove(this);
            }
        }


        public override bool defeatedBy(PFaction attackerFaction)
        {
            return soldierCount <= 0;
        }
        public override bool aliveAndBelongTo(PFaction faction)
        {
            return soldierCount > 0;
        }
        public override bool defeated()
        {
            return soldierCount <= 0;
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
        public override Vector3 WorldPos()
        {
            return position;
        }
        public override void stateDebugText(RichBoxContent content)
        {
            if (!army.TryGetTarget(out var tArmy))
                { return; }

            content.newLine();
            content.text("Group State: " + state.ToString());
            content.text("target string: " + attackTarget_soldierGroupOrCity.ToString());
            if (attackTarget_soldierGroupOrCity.TryGetGroup(out var target))
            {
                content.text("attacking: " + target.TypeName());
                
            }
            else
            {
                content.text("attacking: None");
            }

            content.Add(new RbNewLine(true));
            content.text(tArmy.TypeName());
            tArmy.stateDebugText(content);
        }

        public void setArmyPlacement2(Vector3 wp, bool resetCommand, bool telePort, bool hostedAction)
        {
            goalWp = wp;
            armyPlacementWp = goalWp;

            if (telePort)
            {
                position = wp;
                tilePos = WP.ToTilePos(position);
                setGroundY();

                if (army.TryGetTarget(out var tArmy))
                {
                    rotation = tArmy.rotation;
                }
                //state = GroupState.Idle;

                if (hostedAction && DssRef.world.tileGrid.TryGet(tilePos, out Tile tile))
                {
                    bool waterNode = DssRef.world.tileGrid.Get(tilePos).IsWater();
                    if (waterNode != isShip)
                    {
                        Ref.update.AddSyncAction(new SyncAction2Arg<SoldierTransformType, int>(completeTransform, waterNode ? SoldierTransformType.ToShip : SoldierTransformType.FromShip, -1));
                        //completeTransform(waterNode ? SoldierTransformType.ToShip : SoldierTransformType.FromShip, -1);
                    }
                }
                teleportSoldiers();
            }
            else
            {
                if (resetCommand)
                {
                    cancelCommand();
                }

                wakeupSoldiers();
                
                state = GroupState.FindArmyPlacement;
            }
            
        }

        public AbsGroup GetAttackTarget()
        {
            if (command != null)
            {
                return command.AttackTarget();
            }
            else
            {
                return attackTarget_soldierGroupOrCity.Get() as AbsGroup;
            }
        }

        public void wakeupSoldiers()
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

        void teleportSoldiers()
        {
            var soldiers_sp = soldiers;
            if (soldiers_sp != null)
            {
                var soldiersC = soldiers_sp.counter();
                while (soldiersC.Next())
                {
                    soldiersC.sel.teleport();
                }

            }
        }

        //public void SetArmyPlacement(IntVector2 newLocalPlacement, bool onPurchase)
        //{
        //    if (armyLocalPlacement != newLocalPlacement ||  onPurchase)
        //    {

        //        //armyLocalPlacement = newLocalPlacement;
        //        //goalWp = armyPlacement(army.position);

        //        if (!army.inRender_detailLayer || lifeState == LifeState_New)
        //        {
        //            ++lifeState;
        //            position = goalWp;
        //            setGroundY();
        //        }
        //        //else if (groupObjective == GroupObjective_FollowArmyObjective)
        //        //{
        //        //    groupObjective = GroupObjective_FindArmyPlacement;
        //        //}
        //    }            
        //}

        //public override Faction GetFaction()
        //{
        //    return army.faction;
        //}

        public override AbsMapObject RelatedMapObject()
        {
            army.TryGetTarget(out var result);
            return result;
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
            return soldierConscript.conscript.TypeName(); //+ " Group(" + parentArrayIndex.ToString() + ")";
        }

        virtual public bool IsArmyGroup()
        {
            return true;
        }
        
        //public override SpriteName TypeIcon()
        //{
        //    return AllUnits.UnitFilterIcon( soldierConscript.filterType());
        //}

        virtual public Defence.GuardGroup GetGuardGroup()
        { throw new NotImplementedException(); }

        public override void TypeIcon(RichBoxContent content)
        {
            content.Add(new RbImage(AllUnits.UnitFilterIcon(soldierConscript.filterType())));
        }

        public override string ToString()
        {
            var type = soldierConscript.unitType();
            return "Group " + type.ToString() + " x" + soldiers.Count.ToString() + ", id" + myIndex.ToString();
        }

        virtual public bool InGuardPost()
        {
            return false;
        }

        public override bool IsSoldiers()
        {
            return true;
        }

        public override bool rectangleCollision(ScreenToSpaceRectangleBound rectangle)
        {
            var soldiers_sp = soldiers;
            if (soldiers_sp != null)
            {
                var soldiersC = soldiers_sp.counter();
                while (soldiersC.Next())
                {
                    if (soldiersC.sel.rectangleCollision(rectangle))
                    { 
                        return true;
                    }
                }
            }

            return false;
        }

        public void toGroupHud(ObjectHudArgs args)
        {
            //string name = Name(out _);

            //if (name != null)
            //{
            //    content.text(name).overrideColor = Color.LightYellow;
            //    content.newLine();
            //}

            //content.Add(new RbBeginTitle());
            //content.Add(GetFaction().FlagTextureToHud());
            //content.space(0.5f);
            //content.Add(new RbText(soldierConscript.conscript.TypeName(), HudLib.TitleColor_TypeName));
            //content.space(0.5f);
            //content.Add(new RbText(string.Format(DssRef.lang.UnitId, parentArrayIndex), HudLib.SecondaryTextColor));
            SoldiersPresentationHud(args, true, true);
            
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
        FollowCommand,
        CityCapture,
        //GameStart,
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
