using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.Commander;
using VikingEngine.ToGG.Commander.CommandCard;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.Display3D;
using VikingEngine.ToGG.ToggEngine.Map;
using VikingEngine.ToGG.ToggEngine.Data;

namespace VikingEngine.ToGG.ToggEngine.GO
{   
    abstract class AbsUnit : IComparable, IUnitCardDisplay
    {
        const int DefaultAlertDistance = 8;
        public const int GameStartAlertDistance = 5;
        const int SleepyAlertDistance = 4;
        public const int MinProjectileRange = 2;
        public const SpriteName MoveBoxImage = SpriteName.cmdStatsMoveBox;
        public const SpriteName AttackBoxImage = SpriteName.cmdStatsMeleeBox;
        public const SpriteName ProjectileBoxImage = SpriteName.cmdStatsRangedBox;
        public const SpriteName HealthBoxImage = SpriteName.cmdStatsHealthBox;
        static readonly UnitPropertyType[] CatapultProperties = new UnitPropertyType[] 
            { UnitPropertyType.Catapult, UnitPropertyType.Catapult_Plus };

        public UnitModel soldierModel;
        public UnitHoverGui statusGui = null;
        protected UnitTag unitTag = null;
        
        public bool gotStartPosition = false;
        public IntVector2 squarePos, visualPos;
        
        public IntVector2 orderStartPos;
        public Commander.UnitsData.UnitProgress progress = null;
        public OrderedUnit order;
        public MoveLinesGroup movelines = null;

        public bool AttackedThisTurn = false, UsedFrenzy = false;
        public bool MovedThisTurn { get { return squarePos != orderStartPos; } }

        public int globalPlayerIndex;
        public float lastMoveTime = 0;
        public int collectionIndex;
        public bool isDeleted = false;
        public ValueBar health;

        public int aiOrderValue;
        public IntVector2 aiSuggestedMove;
        public bool aiAlerted = false;
        public bool aiHasBeenActivated = false;
        public bool hasMadeSpecialAction = false;
        public bool hasEndedMovement = false;


        public AbsUnit(System.IO.BinaryReader r, DataStream.FileVersion version, 
            UnitCollection coll, bool placeOnMap = true)
        {
            globalPlayerIndex = coll.playerGlobalIx;
            readPlayerCollection(r, version, coll, placeOnMap);
        }

        public AbsUnit()
        { }

        virtual public void onNewUnit()
        {
            Player.unitsColl.OnNewUnit(this);
            toggRef.absPlayers.OnUnitsCountChange();
        }

        protected void assignPlayer(AbsGenericPlayer player)
        {
            globalPlayerIndex = player.pData.globalPlayerIndex;
            collectionIndex = player.addUnit(this);            
        }

        virtual protected void basicInit(IntVector2 startPos, UnitCollection coll, int? storedStartHealth, bool placeOnMap)
        {
            var data = this.Data;
            data.unit = this;

            if (storedStartHealth == null)
            {
                health = new ValueBar(data.startHealth);
            }
            else
            {
                health = new ValueBar(storedStartHealth.Value, data.startHealth);
            }

            PlayerRelationVisuals relationVisuals;
            if (Player == null)
            {
                relationVisuals = PlayerRelationVisuals.Empty();
            }
            else
            {
                relationVisuals = Player.relationVisuals;
            }

            soldierModel = new UnitModel();
            data.generateModel(this, relationVisuals);

            if (placeOnMap)
            {
                SetPosition(startPos);
            }
            else
            {
                squarePos = startPos;
            }
        }

        public void setFrame(int frame)
        {
            var data = Data;
            if (data.modelSettings.frame != frame)
            {
                data.modelSettings.frame = frame;
                data.generateModel(this, Player.relationVisuals);
            }
        }

        virtual public void update_asynch()
        { }

        virtual public void writeAllData(System.IO.BinaryWriter w)
        {

        }
        
        public void writePlayerCollection(System.IO.BinaryWriter w)
        {
            w.Write((byte)collectionIndex);
            writeDataType(w);
            if (health.IsMax)
            {
                w.Write(byte.MaxValue);
            }
            else
            {
                w.Write((byte)health.Value);
            }

            toggRef.board.WritePosition(w, squarePos);
            w.Write(aiAlerted);

            w.Write(unitTag != null);
        }

        protected void readPlayerCollection(System.IO.BinaryReader r, DataStream.FileVersion version, 
            UnitCollection coll, bool placeOnMap)
        {
            collectionIndex = r.ReadByte();
            //fixa dynamisk set
            coll.Set(this, collectionIndex);

            readDataType(r, version);
            byte readHealth = r.ReadByte();
            int? storedStartHealth;

            if (readHealth == byte.MaxValue)
            {
                storedStartHealth = null;
            }
            else
            {
                storedStartHealth = readHealth;
            }

            IntVector2 pos = toggRef.board.ReadPosition(r);

            if (version.release >= 4)
            {
                aiAlerted = r.ReadBoolean();
            }
            
            initFromRead(pos, storedStartHealth, coll, placeOnMap);

            if (version.release >= 3)
            {
                bool bTag = r.ReadBoolean();
                if (bTag)
                {
                    setTag();
                }
            }
        }

        abstract protected void writeDataType(System.IO.BinaryWriter w);

        abstract protected void readDataType(System.IO.BinaryReader r, DataStream.FileVersion version);
            
        protected void initFromRead(IntVector2 pos, int? storedStartHealth, UnitCollection coll, bool placeOnMap)
        {
            basicInit(pos, coll, storedStartHealth, placeOnMap);
        }

        public void writeIndex(System.IO.BinaryWriter w)
        {
            w.Write((byte)globalPlayerIndex);
            w.Write((byte)collectionIndex);
        }

        public static void ReadIndex(System.IO.BinaryReader r, out int globalPlayerIndex, out int unitIndex)
        {
            globalPlayerIndex = r.ReadByte();
            unitIndex = r.ReadByte();
        }

        public void setHealth(int health)
        {
            this.health.Value = health;
            soldierModel.refreshModel(soldierCount());
        }

        virtual public void TakeHit(AbsUnit fromOpponent, int damage = 1, bool local = true)
        {
            if (health.HasValue)
            {
                new UnitDeathAnimation(this);

                health.add(-damage);
                Player.totalDamageRecieved+= damage;

                if (fromOpponent != null && fromOpponent.progress != null) fromOpponent.progress.damageGiven++;
                if (this.progress != null) this.progress.damageRecieved++;

                if (health.IsZero)
                {
                    //Unit is terminated
                    onDeath(local);
                }
                else
                {
                    soldierModel.refreshModel(soldierCount());
                }

                statusGui?.refresh();
            }
        }

        virtual protected void onDeath(bool local)
        {
            toggRef.gamestate.gameSetup.WinningConditions.onUnitDestroyed(this);
            DeleteMe();
        }

        public void onCollectedPoints(int points)
        {

        }

        public void ai_onTurnStart()
        {
            hasMadeSpecialAction = false;
            aiHasBeenActivated = false;
            orderStartPos = squarePos;
        }

        public void textAnimation(SpriteName icon, string text)
        {
            new Display3D.UnitMessageRichbox(this, icon, text);
            //toggLib.TextAnimation(text, textCol, bgCol, squarePos);
        }

        virtual public void onAttack()
        {
            AttackedThisTurn = true;
        }

        /// <returns>Succesful retreat</returns>
        public bool Retreat(AbsUnit fromUnit)
        {
            const float DirCount = 8f;

            if (Alive)
            {
                Vector2 diff = (fromUnit.squarePos - squarePos).Vec;
                Rotation1D dir = Rotation1D.FromDirection(diff);
                dir.Add(MathHelper.Pi / DirCount);
                Dir8 dir8 = (Dir8)(dir.Radians / (MathHelper.TwoPi / DirCount));
                IntVector2 fleeDir = -IntVector2.Dir8Array[(int)dir8];

                IntVector2 toSquare = squarePos + fleeDir;

                if (canRetreatToSquare(toSquare))
                {
                    SlideToSquare(toSquare, true);
                    return true;
                }
                else
                { //Cant retreat, takes damage instead
                    SlideToSquare(toSquare, false);
                    TakeHit(fromUnit);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void SlideToSquare(IntVector2 toSquare, bool succeded)
        {
           
            //animate
            Vector3 goalPos = toggLib.ToV3(toSquare);
            Vector3 posDiff = goalPos - soldierModel.Position;

            if (succeded)
            {
                SetDataPosition(toSquare);
                visualPos = toSquare;

                new Graphics.Motion3d(Graphics.MotionType.MOVE, soldierModel, posDiff, Graphics.MotionRepeate.NO_REPEAT,
                    400, true);
            }
            else
            {
                posDiff *= 0.3f;

                new Graphics.Motion3d(Graphics.MotionType.MOVE, soldierModel, posDiff, Graphics.MotionRepeate.BackNForwardOnce,
                    80, true);
            }
        }

        public IntVector2 vecTowards(AbsUnit towards)
        {
            return vecTowards(towards.squarePos);
        }

        public IntVector2 vecTowards(IntVector2 towards)
        {
            IntVector2 result = towards - squarePos;
            if (result.SideLength() > 1)
            {
                result = result.Normal_RoundUp();
            }

            return result;
        }

        bool canRetreatToSquare(IntVector2 toSquare)
        {
            if (toggRef.board.tileGrid.InBounds(toSquare))
            {
                BoardSquareContent tile = toggRef.board.tileGrid.Get(toSquare);
                var moveRest = toggRef.board.MovementRestriction(toSquare, this);
                if (moveRest == MovementRestrictionType.Impassable)
                {
                    return false;
                }
                else if (moveRest == MovementRestrictionType.MustStop)
                {
                    if (HasProperty(UnitPropertyType.Light))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else //no restrictions
                {
                    return true;
                }
            }
            return false;
        }

        public void undoMovement(bool ignoreLocks)
        {
            if (movelines != null)
            {
                IntVector2 returnPos = movelines.start;

                if (!ignoreLocks && movelines.hasPartialLock())
                {
                    for (int i = 0; i < movelines.lines.Count; ++i)
                    {
                        if (!movelines.lines[i].partialLock)
                        {
                            returnPos = movelines.lines[i].fromPos;
                            movelines.removesLines(i);
                            movelines.refreshCurrentPosDot();
                            break;
                        }
                    }
                }
                else
                {
                    returnPos = movelines.start;
                    deleteMoveLines();

                    if (order != null)
                    {
                        order.CheckList_Enabled = true;
                    }
                }

                SetVisualPosition(returnPos);
                SetDataPosition(returnPos);
            }
        }

        public void deleteMoveLines()
        {
            if (movelines != null)
            {
                movelines.DeleteMe();
                movelines = null;
            }
        }

        public void DeleteMe()
        {
            soldierModel.DeleteMe();
            movelines?.DeleteMe();
            toggRef.absPlayers.GenPlayer(globalPlayerIndex).unitsColl.units.Remove(this);
            removeFromMapTile();
            isDeleted = true;
            health.setZero();

            toggRef.absPlayers.OnUnitsCountChange();
        }

        public BoardSquareContent OnSquare
        {
            get { return toggRef.board.tileGrid.Get(squarePos); }
        }

        public BoardSquareContent onSquare(bool useOrderStartPos)
        {
            return toggRef.board.tileGrid.Get(useOrderStartPos? orderStartPos : squarePos);
        }

        public bool HasProperty(UnitPropertyType property)
        {
            return Data.properties.HasProperty(property);
        }

        public bool HasProperty(UnitPropertyType property1, UnitPropertyType property2)
        {
            return Data.properties.HasProperty(property1) ||
                Data.properties.HasProperty(property2);
        }
        public bool HasProperty(UnitPropertyType property1, UnitPropertyType property2, UnitPropertyType property3)
        {
            return Data.properties.HasProperty(property1) ||
                Data.properties.HasProperty(property2) ||
                Data.properties.HasProperty(property3);
        }

        public bool TryGetProperty(UnitPropertyType type, out ToGG.Data.Property.AbsUnitProperty result)
        {
            result = Data.properties.Get(type);
            return result != null;
        }

        public bool TryGetProperty(UnitPropertyType[] types, out ToGG.Data.Property.AbsUnitProperty result)
        {
            foreach (var type in types)
            {
                result = Data.properties.Get(type);
                if (result != null)
                {
                    return true;
                }
            }

            result = null;
            return false;
        }

        virtual public void OnTurnStart()
        {
            AttackedThisTurn = false;
            UsedFrenzy = false;            
        }
        virtual public void OnTurnEnd()
        {
            orderStartPos = squarePos;
        }

        public void SetPosition(IntVector2 square)
        {
            SetDataPosition(square);
            SetVisualPosition(square);
        }
        
        virtual public void SetDataPosition(IntVector2 square)
        {
            if (squarePos != square && Alive)
            {
                removeFromMapTile();
                
                lastMoveTime = Ref.TotalTimeSec;
                squarePos = square;
                if (toggRef.InRunningGame)
                {
                    this.Player.onMovedUnit(this);
                    OnEvent(ToGG.Data.EventType.UnitPositionChange, true, null);
                }
                gotStartPosition = true;

                toggRef.board.tileGrid.Get(squarePos).unit = this;                
            }
        }

        public void SetVisualPosition(IntVector2 square, bool netShare = false)
        {
            visualPos = square;
            Vector3 wp = toggRef.board.toWorldPos_Center(square, 0);
            soldierModel.SetPositionXZ(wp.X, wp.Z);

            statusGui?.UpdatePosition();

            if (netShare)
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.toggUnitVisualPos, Network.PacketReliability.Reliable);
                writeIndex(w);
                toggRef.board.WritePosition(w, square);
            }
        }

        public void SetVisualPosition(Vector3 pos)
        {
            soldierModel.Position = pos;

            statusGui?.UpdatePosition();
        }

        //public void SetTempAnimationPosition(IntVector2 square, bool netShare = false)
        //{
        //    SetVisualPosition(square);
        //    squarePos = square;

        //    if (netShare)
        //    {
        //        var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqTempAnimationPos, Network.PacketReliability.Reliable);
        //        writeIndex(w);
        //        toggRef.board.WritePosition(w, square);
        //    }
        //}

        public void ShiftPosition(IntVector2 dir)
        {
            squarePos.X = Bound.SetRollover(squarePos.X + dir.X, 0, toggRef.board.MaxPos.X);
            squarePos.Y = Bound.SetRollover(squarePos.Y + dir.Y, 0, toggRef.board.MaxPos.Y);

            SetVisualPosition(squarePos);
        }

        public void removeFromMapTile()
        {
            BoardSquareContent prev;
            if (toggRef.board.tileGrid.TryGet(squarePos, out prev))
            {
                if (prev.unit == this)
                {
                    prev.unit = null;
                }
            }
        }

        public bool InMeleeRange(IntVector2 target)
        {
            return InMeleeRange(squarePos, target);//squarePos.SideLength(target) <= 1;            
        }

        public bool InMeleeRange(IntVector2 fromPos, IntVector2 target)
        {
            return fromPos.SideLength(target) <= 1;
        }

        public bool InInteractRange(IntVector2 target)
        {
            return squarePos.SideLength(target) <= 1;
        }

        public bool InRangeAndSight(IntVector2 target, int range, bool includeAdjacent, bool terrainTarget)
        {
            return InRangeAndSight(squarePos, target, range, includeAdjacent, terrainTarget);
        }

        public bool InRangeAndSight(IntVector2 fromPos, IntVector2 target, int range, 
            bool includeAdjacent, bool terrainTarget)
        {
            if (includeAdjacent && AdjacentTo(target))
            {
                return true;
            }

            IntVector2 non;
            return InProjectileRange(fromPos, target, range) &&
                InLineOfSight(fromPos, target, terrainTarget, out non);
        }

        public bool InMinProjectileRange(IntVector2 target)
        {
            return InMinProjectileRange(squarePos, target);
        }

        public bool InMinProjectileRange(IntVector2 fromPos, IntVector2 target)
        {
            return fromPos.SideLength(target) >= MinProjectileRange;
        }        

        public bool InProjectileRange(IntVector2 fromPos, IntVector2 target, int range)
        {
            return InMinProjectileRange(fromPos, target) && fromPos.SideLength(target) <= range;
        }

        public bool InLineOfSight(IntVector2 fromPos, IntVector2 target, bool terrainTarget, out IntVector2 blockingTile)
        {
            if (toggRef.board.tileGrid.Get(fromPos).HasProperty(TerrainPropertyType.Oversight))
            {
                blockingTile = IntVector2.Zero;
                return true;
            }
            return toggRef.board.InLineOfSight(fromPos, target, out blockingTile, this, toggRef.UnitsBlockLOS, terrainTarget);            
        }

        public bool InLineOfSight(IntVector2 target)
        {
            IntVector2 blockingTile;
            return toggRef.board.InLineOfSight(squarePos, target, out blockingTile, this, toggRef.UnitsBlockLOS, false);
        }

        public bool canAttackSkillCheck()
        {
            if (CatapultAttackType && MovedThisTurn)
            {
                return false;
            }

            return true;
        }

        virtual public bool canTargetUnit(AbsUnit attackTarget)
        {
            return true;
        }

        public bool bAdjacentOpponents()
        {
            return adjacentOpponentsCount(squarePos) > 0;
        }
        public bool bAdjacentOpponents(IntVector2 position)
        {
            return adjacentOpponentsCount(position) > 0;
        }

        public int adjacentOpponentsCount(IntVector2 position)
        {
            int result = 0;
            foreach (IntVector2 dir in IntVector2.Dir8Array)
            {
                AbsUnit nUnit = toggRef.board.getUnit(dir + position);
                if (nUnit != null && IsOpponent(nUnit))
                {
                    ++result;
                }
            }

            return result;
        }

        public List<AbsUnit> adjacentOpponents(IntVector2 position)
        {
            List<AbsUnit> result = null;

            foreach (IntVector2 dir in IntVector2.Dir8Array)
            {
                AbsUnit nUnit = toggRef.board.getUnit(dir + position);
                if (nUnit != null && IsOpponent(nUnit) && canTargetUnit(nUnit))
                {
                    arraylib.AddOrCreate(ref result, nUnit);
                }
            }

            return result;
        }

        public bool hasOpponentsInArea(IntVector2 center, int radius)
        {
            ForXYLoop loop = new ForXYLoop(Rectangle2.FromCenterTileAndRadius(center, radius));
            while (loop.Next())
            {
                AbsUnit nUnit = toggRef.board.getUnit(loop.Position);
                if (nUnit != null && IsOpponent(nUnit))
                {
                    return true;
                }
            }

            return false;
        }

        public List<AbsUnit> adjacentFriendlies(IntVector2 position)
        {
            List<AbsUnit> result = null;

            if (position.X < 0)
            {
                position = squarePos;
            }

            foreach (IntVector2 dir in IntVector2.Dir8Array)
            {
                AbsUnit nUnit = toggRef.board.getUnit(dir + position);
                if (nUnit != null && IsAlly(nUnit))
                {
                    arraylib.AddOrCreate(ref result, nUnit);
                }
            }

            return result;
        }

        public List<AbsUnit> adjacentUnits(IntVector2 position)
        {
            List<AbsUnit> result = null;

            if (position.X < 0)
            {
                position = squarePos;
            }

            foreach (IntVector2 dir in IntVector2.Dir8Array)
            {
                AbsUnit nUnit = toggRef.board.getUnit(dir + position);
                if (nUnit != null)
                {
                    arraylib.AddOrCreate(ref result, nUnit);
                }
            }

            return result;
        }

        public bool AdjacentTo(AbsUnit otherUnit)
        {
            if (otherUnit == this)
                throw new Exception("AdjacentTo myself");

            return (otherUnit.squarePos - this.squarePos).SideLength() == 1;
        }

        public bool AdjacentTo(IntVector2 position)
        {
            return squarePos.SideLength(position) == 1;
        }

        public int sideDistanceTo(AbsUnit other)
        {
            return (other.squarePos - this.squarePos).SideLength();
        }

        public bool Alive
        {
            get { return health.HasValue; }
        }
        public bool Dead
        {
            get { return health.IsZero; }
        }

        public bool Resting
        {
            get 
            { 
                return soldierModel.RestVisibleGet(); 
            }
            set
            {
                if (Alive)
                {
                    soldierModel.RestVisibleSet(value, Player.relationVisuals);
                }
            }
        }

        public bool CatapultAttackType
        {
            get 
            { 
                //UnitPropertyType non;
                return Data.properties.HasProperty(CatapultProperties);
            }
        }

        virtual public AbsGenericPlayer Player
        {
            get { return toggRef.absPlayers.getGenericPlayer(this.globalPlayerIndex); }
        }
        public Commander.Players.AbsLocalPlayer LocalPlayer
        {
            get { return toggRef.absPlayers.getGenericPlayer(this.globalPlayerIndex) as Commander.Players.AbsLocalPlayer; }
        }

        virtual public bool canBeAlerted()
        { return true; }

        public override string ToString()
        {
            return "Unit (" + NameAndId() + ")";
        }

        public string NameAndId()
        {
            return Data.Name + UnitId.ToString();
        }

        virtual public bool canMoveThrough(AbsUnit otherUnit)
        {
            return false;
        }

        virtual public bool IsScenarioOpponent()
        {            
            return false;
        }

        public bool IsParent(AbsGenericPlayer player)
        {
            return player != null && player.pData.globalPlayerIndex == globalPlayerIndex;
        }
        abstract public bool IsOpponent(AbsUnit otherUnit);

        abstract public bool IsAlly(AbsUnit otherUnit);

        public bool IsTargetOpponent(AbsUnit otherUnit)
        {
            return otherUnit != null && IsOpponent(otherUnit) && canTargetUnit(otherUnit);
        }

        public BattleEngine.AttackMainType IsAvailableTarget(IntVector2 fromPos, AbsUnit otherUnit)
        {
            if (IsTargetOpponent(otherUnit))
            {
                if (InMeleeRange(fromPos, otherUnit.squarePos) && Data.HasMeleeAttack)
                { //MELEE range
                    return BattleEngine.AttackMainType.Melee;
                }

                if (ableToProjectileAttack(fromPos) &&
                    InRangeAndSight(fromPos, otherUnit.squarePos, FireRangeWithModifiers(fromPos), 
                    false, false))
                {  //PROJECTILE attack
                    return BattleEngine.AttackMainType.Ranged;
                }
            }

            return BattleEngine.AttackMainType.NONE;
        }

        virtual public int alertDistance()
        {
            if (HasProperty(UnitPropertyType.Sleepy))
            { return SleepyAlertDistance; }

            return DefaultAlertDistance;
        }

        public bool lockedInMelee()
        {
            return lockedInMelee(squarePos);
        }

        public bool lockedInMelee(IntVector2 fromPos)
        {
            BoardSquareContent sq;
            foreach (var dir in IntVector2.Dir8Array)
            {
                if (toggRef.board.tileGrid.TryGet(fromPos + dir, out sq))
                {
                    if (sq.unit != null &&
                        IsOpponent(sq.unit) &&
                        sq.unit.Data.HasMeleeAttack)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool ableToProjectileAttack()
        {
            return ableToProjectileAttack(squarePos);
        }
        public bool ableToProjectileAttack(IntVector2 fromPos)
        {
            return Data.WeaponStats.HasProjectileAttack && !lockedInMelee(fromPos);
        }

        virtual public bool hasHealth()
        { return true; }

        virtual public bool hasStamina()
        { return false; }

        public WalkingPath FindPath(IntVector2 toPos, bool adjacentToGoal)
        {
            return FindPath(squarePos, toPos, adjacentToGoal);
        }

        public WalkingPath FindPath(IntVector2 fromPos, IntVector2 toPos, bool adjacentToGoal)
        {
            PathFinding path = new PathFinding();
            return path.FindPath(this, fromPos, toPos, adjacentToGoal);
        }

        public void calcOrderValue()
        {
            AvailableMovement moves = new AvailableMovement(Player, this, false, true);

            int mostValuableMove = int.MinValue;
            IntVector2 mostValuablePos = squarePos;

            foreach (IntVector2 pos in moves.available)
            {
                int value = positionValue(pos);

                if (pos != squarePos)
                {
                    WalkingPath path = FindPath(pos, false);
                    value -= (int)(path.expectedDamage * 300);
                }

                if (value > mostValuableMove)
                {
                    mostValuableMove = value;
                    mostValuablePos = pos;
                }
            }

            aiOrderValue = mostValuableMove;
            aiSuggestedMove = mostValuablePos;

            if (OnSquare.tileObjects.HasObject(TileObjectType.TacticalBanner))
            { //Rather do nothing if holding position
                aiOrderValue -= 600;
            }
        }

        public int positionValue(IntVector2 position)
        {
            var data = this.Data;
            int value = 0;
            BoardSquareContent square = toggRef.board.tileGrid.Get(position);

            int terrainValue = 0;

            if (square.HasProperty(TerrainPropertyType.ArrowBlock))
            {
                terrainValue += 50;
            }
            if (square.HasProperty(TerrainPropertyType.Block1))
            {
                terrainValue += 100;
            }

            if (square.HasProperty(TerrainPropertyType.SittingDuck))
            {
                terrainValue -= 200;
            }
            if (square.HasProperty(TerrainPropertyType.Oversight) && data.WeaponStats.HasProjectileAttack)
            {
                terrainValue += 70;
            }

            if (square.HasProperty(TerrainPropertyType.ReducedTo1))
            {
                if (data.wep.meleeStrength > 1)
                    terrainValue -= 300;
                else
                    terrainValue -= 50;
            }
            else if (square.HasProperty(TerrainPropertyType.ReducedTo2))
            {
                if (data.wep.meleeStrength > 2)
                    terrainValue -= 200;
                else
                    terrainValue -= 50;
            }

            if (toggRef.gamestate.gameSetup.AiMod_TerrainIgnorant)
            {
                terrainValue /= 10;
            }
            value += terrainValue;

            if (square.HasProperty(TerrainPropertyType.MoveBonus))
            {
                value += 5;

                if (data.move == 1)
                {
                    value += 100;
                }
            }

            //For supporting adjacent units
            BoardSquareContent adjTile;
            int enemyAttackStrength = 0;
            int friendlyAttackStrength = meleeAttackStrengthValue();

            foreach (IntVector2 dir in IntVector2.Dir8Array)
            {
                IntVector2 adjacentPos = dir + position;
                if (toggRef.board.tileGrid.TryGet(adjacentPos, out adjTile))
                {
                    if (adjTile.unit != null && adjTile.unit != this)
                    {
                        int attackStrength = adjTile.unit.meleeAttackStrengthValue();

                        if (adjTile.unit.globalPlayerIndex == globalPlayerIndex)
                        {
                            friendlyAttackStrength += attackStrength;
                        }
                        else
                        {
                            enemyAttackStrength += attackStrength;
                            if (adjTile.unit.HasProperty(UnitPropertyType.Valuable))
                            {
                                //Valuable target
                                value += 300;
                            }
                            else if (adjTile.unit.HasProperty(UnitPropertyType.Expendable))
                            {
                                value -= 100;
                            }

                            if (!data.WeaponStats.HasProjectileAttack && adjTile.unit.Data.WeaponStats.HasProjectileAttack)
                            {
                                //Bonus for block ranged fire
                                value += 50 * adjTile.unit.Data.wep.projectileStrength;
                            }
                        }
                    }
                }
            }

            if (enemyAttackStrength > 0)
            {
                if (toggRef.gamestate.gameSetup.AiMod_SuperAggressive)
                {
                    friendlyAttackStrength *= 8;
                }
                int ccValue = (friendlyAttackStrength - enemyAttackStrength) * 4;
                value += ccValue;
            }
            else
            {
                //Small bonus for sticking together
                value += friendlyAttackStrength / 10;

                if (data.WeaponStats.HasProjectileAttack)
                {
                    //Check ability to fire on opponents
                    AttackTargetCollection att = new AttackTargetCollection(this, position);

                    if (att.targets.Count > 0)
                    {
                        int mostValuableTarget = int.MinValue;
                        foreach (var target in att.targets.list)
                        {
                            mostValuableTarget = lib.LargestValue(mostValuableTarget, target.unit.TargetValue());
                        }

                        value += mostValuableTarget;
                    }
                }
            }

            if (position != squarePos)
            {
                //Get value for doing something at least
                value += 40;

                //Get some value for moving forward
                //int forwardValue = boardRowValue(position.Y) - boardRowValue(squarePos.Y);
                //value += forwardValue;
            }

            foreach (var m in toggRef.gamestate.gameSetup.aiMoveUnits)
            {
                if (m.unit == this)
                {
                    int closer = closerToTarget(this.squarePos, position, m.goalPos);
                    if (closer > 0)
                    {
                        value += closer * 2000;
                        if (square.HasProperty(TerrainPropertyType.MoveBonus))
                        {
                            value += 400;
                        }
                    }
                }
            }

            foreach (var m in toggRef.gamestate.gameSetup.aiTargetUnits)
            {
                if (m.unit.globalPlayerIndex != this.globalPlayerIndex)
                {
                    value += closerToTarget(this.squarePos, position, m.unit.squarePos) * 50;
                }
            }

            //Value for tactical points
            if (square.adjacentToFlag)
            {
                if (toggRef.gamestate.gameSetup.AiMod_BannerLowPrio)
                {
                    value += 10;
                }
                else
                {
                    value += 200;
                }
            }
            else if (square.tileObjects.HasObject(TileObjectType.TacticalBanner))
            {
                if (toggRef.gamestate.gameSetup.AiMod_BannerLowPrio)
                {
                    value += 40;
                }
                else
                {
                    value += 800;
                }
            }

            return value;
        }

        int closerToTarget(IntVector2 moveFrom, IntVector2 moveTo, IntVector2 target)
        {
            int from = (target - moveFrom).SideLength();
            int to = (target - moveTo).SideLength();

            return from - to;
        }

        int meleeAttackStrengthValue()
        {
            if (Data.HasMeleeAttack)
            {
                return MathExt.Square(Data.wep.meleeStrength) * health.Value + 1;
            }
            return -8;
        }

        int boardRowValue(int ypos)
        {
            int row;
            if (toggLib.BottomPlayer(globalPlayerIndex))
            {
                row = toggRef.board.Size.Y - ypos;
            }
            else
            {
                row = ypos;
            }

            return row * 2;
        }

        public int soldierCount()
        {
            if (Data.oneManArmy) return 1;

            return health.Value;
        }

        virtual public int AttackWithUnitValue(AbsUnit targetUnit, bool closeCombat)
        {
            return -1;
            //int value = targetUnit.TargetValue();
            //AttackTarget target = new AttackTarget(targetUnit, closeCombat ? BattleEngine.AttackType.Melee : BattleEngine.AttackType.Ranged);

            //Commander.Battle.BattleSetup attacks = new Commander.Battle.BattleSetup(
            //    new List<AbsUnit> { this }, target, CommandType.Order_3);

            //int attackPower = (attacks.attackerSetup.attackStrength - attacks.hitBlocks) * 4 - attacks.retreatIgnores;

            //int counterPower = 0;
            //if (closeCombat)
            //{
            //    Commander.Battle.BattleSetup counterattacks = new Commander.Battle.BattleSetup(
            //        new List<AbsUnit> { targetUnit }, new AttackTarget(this, BattleEngine.AttackType.CounterAttack), CommandType.Order_3);

            //    counterPower = (counterattacks.attackerSetup.attackStrength - counterattacks.hitBlocks) * 3;

            //    //Value declines if unit is on a flag, dont wanna risk losing the spot
            //    if (OnSquare.tileObjects.HasObject(TileObjectType.TacticalBanner))
            //    {
            //        value -= 100;
            //    }
            //}

            //value += (attackPower - counterPower) * 20;

            //return value;
        }

        public int TargetValue()
        {
            int value = (6 - health.Value) * 20;
            if (HasProperty(UnitPropertyType.Valuable))
            {
                value *= 2;
            }
            if (HasProperty(UnitPropertyType.Block))
            {
                value -= 50;
            }

            if (OnSquare.tileObjects.HasObject(TileObjectType.TacticalBanner))
            {
                value += 200;
            }
            else if (OnSquare.adjacentToFlag)
            {
                value += 50;
            }

            return value;
        }

        virtual public int MoveLengthWithModifiers(bool useOrderStartPos)
        {
            int hasMoved, movementLeft, max, staminaMoves, backStabs;
            moveInfo(out hasMoved, out movementLeft, out staminaMoves, out max, out backStabs);

            int moveLength = useOrderStartPos ? max : movementLeft;

            if (onSquare(useOrderStartPos).HasProperty(TerrainPropertyType.MoveBonus))
            {
                moveLength += 1;
            }
            return moveLength;
        }
        
        virtual public void moveInfo(out int hasMoved, out int movementLeft, out int staminaMoves, out int max, out int backStabs)
        {
            hasMoved = 0;
            max = Data.move;
            movementLeft = max;
            staminaMoves = 0;
            backStabs = 0;
        }

        public int FireRangeWithModifiers(IntVector2 fromPos)
        {
            int result = Data.WeaponStats.projectileRange;

            if (Data.WeaponStats.HasProjectileAttack)
            {
                if (toggRef.board.tileGrid.Get(fromPos).HasProperty(TerrainPropertyType.Oversight))
                {
                    result += 1;
                }
            }

            return result;
        }

        public Graphics.ImageGroupParent2D Card(Graphics.ImageGroupParent2D images, int? count)
        {
            var data = this.Data;

            Graphics.Image icon;
            images = HudLib.HudCardBasics(images, data.Name, data.modelSettings.image, 1f, out icon);

            //Stats
            Vector2 statsIconSz = new Vector2(HudLib.cardContentArea.Height * 0.6f);
            Vector2 nextPos = new Vector2(icon.Right * 0.8f, HudLib.cardContentArea.Bottom - statsIconSz.Y * 0.95f);

            cardStats(MoveBoxImage, data.move, ref nextPos, statsIconSz, images);
            cardStats(AttackBoxImage, data.wep.meleeStrength, ref nextPos, statsIconSz, images);
            if (data.WeaponStats.HasProjectileAttack)
            {
                Graphics.TextG rangeText = new Graphics.TextG(LoadedFont.Regular,
                    nextPos, Vector2.One,//new Vector2(Engine.Screen.TextSize * 1f, Engine.Screen.TextSize * 0.95f),
                    Graphics.Align.CenterWidth, data.wep.projectileRange.ToString(), Color.Black, HudLib.ContentLayer);
                rangeText.SetHeight(statsIconSz.Y * 0.54f);
                rangeText.outline = Graphics.TextOutlineType.Border8Dir;
                rangeText.outlineColor = Color.White;
                rangeText.outlineThickness = 2f;
                rangeText.Xpos += statsIconSz.X * 0.7f;
                rangeText.Ypos = HudLib.cardContentArea.Bottom - statsIconSz.Y * 1.15f;
                rangeText.PaintLayer -= PublicConstants.LayerMinDiff;
                images.Add(rangeText);

                cardStats(ProjectileBoxImage, data.wep.projectileStrength, ref nextPos, statsIconSz, images);
            }
            cardStats(HealthBoxImage, health.Value, ref nextPos, statsIconSz, images);


            Vector2 skillTextPos = new Vector2(HudLib.cardContentArea.Width * 0.62f, HudLib.cardContentArea.Y + HudLib.cardContentArea.Height * 0.1f);

            //var propList = data.properties.ToList((int)UnitPropertyType.Num_Non);
            if (data.properties.HasMembers())
            {
                foreach (var prop in data.properties.members)
                {
                    HudLib.SkillText(prop.Name, ref skillTextPos, images);
                }
            }

            if (count != null)
            {
                //Count
                Graphics.TextG countText = new Graphics.TextG(LoadedFont.Regular, HudLib.cardContentArea.RightCenter, new Vector2(Engine.Screen.TextSize * 2),
                   new Graphics.Align(new Vector2(1f, 0.5f)), "x" + count.Value.ToString(), Color.Blue, HudLib.ContentLayer - 1);
                images.Add(countText);
            }
            return images;
        }

        void cardStats(SpriteName staticon, int value, ref Vector2 nextPos, Vector2 statsIconSz, Graphics.ImageGroupParent2D images)
        {
            Graphics.Image icon = new Graphics.Image(staticon, nextPos, statsIconSz, HudLib.ContentLayer);
            Graphics.TextG valueText = new Graphics.TextG(LoadedFont.Regular, icon.Center, Vector2.One,
                Graphics.Align.CenterHeight, value.ToString(), Color.White, HudLib.ContentLayer);
            valueText.SetHeight(icon.Height * 0.55f);

            valueText.PaintLayer -= PublicConstants.LayerMinDiff;
            valueText.Xpos += icon.Width * 0.12f;
            valueText.Ypos += icon.Width * 0.02f;
            nextPos.X += statsIconSz.X * 1f;

            images.Add(icon); images.Add(valueText);
        }

        public Vector2 SquarePosV2 { get { return toggRef.board.toWorldPosXZ_Center(squarePos); } }

        virtual public float expectedMoveDamage(IntVector2 from, IntVector2 to)
        {
            int backstabs = checkAdjacentEnemies(from);
            if (HasProperty(UnitPropertyType.Block))
            {
                backstabs -= 1;
            }

            return Bound.Min(backstabs, 0);
        }

        int checkAdjacentEnemies(IntVector2 pos)
        {
            return toggRef.absPlayers.adjacentOpponents(this, pos).Count;
        }

        public int needHealing(HealType healType)
        {
            int damage = health.ValueRemoved;
            if (damage > 0)
            {
                if (healType == HealType.Dark)
                {
                    if (HasProperty(UnitPropertyType.Undead) == false)
                    {
                        return 0;
                    }
                }
                else if (healType == HealType.Magic)
                {
                    if (HasProperty(UnitPropertyType.Undead))
                    {
                        return 0;
                    }
                }
            }

            return damage;
        }

        public void setTag()
        {
            unitTag = new Data.UnitTag();
            if (!toggRef.InEditor)
            {
                HeroQuest.hqRef.setup.conditions.addUnit(this);
            }
        }

        public void removeTag()
        {
            unitTag = null;
        }

        public bool HasTag => unitTag != null;

        virtual public void AddToUnitCard(ToGG.ToggEngine.Display2D.UnitCardDisplay card, ref Vector2 position)
        { }

        virtual public void Alert()
        {
            aiAlerted = true;
        }

        public float StrengthValue()
        {
            return health.Value + Data.armorValue();
        }

        virtual public void OnEvent(ToGG.Data.EventType eventType, bool local, object tag)
        {
            Data.properties?.OnEvent(eventType, local, tag, this);
        }

        public void blockAnimation()
        {
            textAnimation(SpriteName.cmdArmorResult, "BLOCK!");
        }

        public void blockRetreatAnimation()
        {
            textAnimation(SpriteName.cmdIgnoreRetreat, "Ignore!");
        }

        // Returns:
        //     A value that indicates the relative order of the objects being compared.
        //     The return value has these meanings: Value Meaning Less than zero This instance
        //     is less than obj. Zero This instance is equal to obj. Greater than zero This
        //     instance is greater than obj.
        public int CompareTo(object obj)
        {
            return aiOrderValue - ((AbsUnit)obj).aiOrderValue;
        }

        abstract public Commander.GO.Unit cmd();
        abstract public HeroQuest.Unit hq();

        abstract public AbsUnitData Data
        {
            get; set;
        }

        public void addProperty(ToGG.Data.Property.AbsUnitProperty m)
        {
            Data.properties.Add(m, this);
        }

        public bool IsLocal => toggRef.absPlayers.getGenericPlayer(globalPlayerIndex).IsLocal;

        public int UnitId => globalPlayerIndex * 1000 + collectionIndex;
    }
}
