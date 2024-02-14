using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;
using VikingEngine.ToGG.ToggEngine.Map;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngineShared.ToGG.ToggEngine.Data;
using VikingEngine.ToGG.Commander;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class Board
    {
        public const float SquareWidth = 1f;
        public const float SquareRadius = SquareWidth * 0.5f;
        public const float MoveCollRadius = SquareRadius * 0.95f;

        public BoardMetaData metaData;
        public Grid2D<BoardSquareContent> tileGrid;
        public List<Graphics.Mesh> tileTagModels = new List<Mesh>(32);

        public IntVector2 HalfSize, MaxPos;
        public VectorRect selectionBounds;
        public VectorRect camBounds;
        public BoardModel model;
        public MapFogModel fogModel;

        public const int PlacementHeight = 3;
        
        static readonly IntVector2 StandardSize = new IntVector2(14, 8);

        bool needsRefresh = false;
        bool waitingForModelGen = true, waitingForMetaGen = true;

        public Board(BoardType type)
        {
            if (type == BoardType.HeroQuest)
            {
                initHeroQuest();
            }

            init();
        }

        public Board(Commander.LevelSetup.GameSetup gameSetup, BoardType type)
        {
            generateDataGrid();

            init();

            if (gameSetup.loadMap == null)
            {
                if (type == BoardType.Match)
                {
                    placeVpSquares();

                    randomTerrain();
                }
                deploymentTiles();
                model = new BoardModel();
                if (type == BoardType.Editor)
                {
                    model.beginGenerateModel();
                }
            }            
        }

        void init()
        {
            toggRef.board = this;
        }

        public BoardSquareContent adjacentSquare(IntVector2 center, Dir8 dir)
        {
            IntVector2 npos = center + IntVector2.FromDir8(dir);
            BoardSquareContent result = null;
            tileGrid.TryGet(npos, out result);
            return result;
        }

        public List<AbsUnit> collectUnits(List<IntVector2> squares)
        {
            List<AbsUnit> units = new List<AbsUnit>(squares.Count);
            foreach (var m in squares)
            {
                var u = tileGrid.Get(m).unit;
                if (u != null)
                {
                    units.Add(u);
                }
            }

            return units;
        }

        void initHeroQuest()
        {
            generateDataGrid();


            model = new BoardModel();
            if (toggRef.InEditor)
            {
                model.beginGenerateModel();
            }

            fogModel = new MapFogModel();
        }

        private void generateDataGrid()
        {
            tileGrid = new Grid2D<BoardSquareContent>(StandardSize);
            setBounds();

            tileGrid.LoopBegin();

            while (tileGrid.LoopNext())
            {
                BoardSquareContent tile = new BoardSquareContent();
                tileGrid.LoopValueSet(tile);
            }
        }

        public void update()
        {
            if (model != null)
            {
                if (needsRefresh && !model.isGenerating)
                {
                    waitingForModelGen = true;
                    waitingForMetaGen = true;
                    needsRefresh = false;
                    model.beginGenerateModel();
                    new QueAndSynchTask(collectMetaData_asynch, onCollectMetaDataComplete);
                }

                if (model.content != null && Ref.TimePassed16ms)
                {
                    foreach (var m in model.content.emitters)
                    {
                        m.update();
                    }
                }
            }
        }

        void collectMetaData_asynch()
        {
            BoardMetaData newData = new BoardMetaData();
            newData.collect_asynch();

            metaData = newData;
        }

        void onCollectMetaDataComplete()
        {
            waitingForMetaGen = false;

            modelAndMetaIsReadyCheck();
        }

        public void onNewModel(BoardModel newModel)
        {
            model?.DeleteMe();
            model = newModel;

            arraylib.DeleteAndClearArray(tileTagModels);

            if (toggRef.InEditor)
            {
                string text;
                Color color;

                tileGrid.LoopBegin();
                while (tileGrid.LoopNext())
                {
                    if (tileGrid.LoopValueGet().tag.boardText(out text, out color))
                    {
                        var model = new Graphics.Text3DBillboard(
                            LoadedFont.Console, text, color,
                            Color.White, Vector3.Zero,
                            0.2f, 1f, true);
                        model.FaceCamera = false;

                        model.Position = toggRef.board.toWorldPos_Center(
                            tileGrid.LoopPosition, 0.05f);
                        model.Z -= 0.2f;

                        tileTagModels.Add(model);
                    }
                }
            }
            else
            {
                tileGrid.LoopBegin();
                while (tileGrid.LoopNext())
                {
                    if (tileGrid.LoopValueGet().tag.tagType == SquareTagType.MapEnter)
                    {
                        var model = new Graphics.Mesh(LoadedMesh.plane,
                            toggRef.board.toWorldPos_Center(tileGrid.LoopPosition, 0.05f),
                            new Vector3(0.3f), TextureEffectType.Flat, SpriteName.cmdMapEntranceIcon, Color.White);
                        model.Z -= 0.2f;
                        tileTagModels.Add(model);
                    }
                }
            }

            waitingForModelGen = false;
            modelAndMetaIsReadyCheck();
        }

        void setBounds()
        {
            HalfSize = tileGrid.Size / 2;
            MaxPos = tileGrid.Size - 1;
            selectionBounds = new VectorRect(Vector2.Zero, MaxPos.Vec);
            selectionBounds.AddRadius(0.5f);

            camBounds = new VectorRect(Vector2.Zero, tileGrid.Size.Vec);
            camBounds.AddXRadius(1f);
            camBounds.AddYRadius(1f);

            Ref.draw.Camera.LookTarget = VectorExt.V2toV3XZ(camBounds.Center);
        }

        public void Resize(IntVector2 newSize)
        {
            tileGrid.ReSize(newSize, ResizeGrid2D_GetDefault, ResizeGrid2D_Removing);
            refresh();
            setBounds();
        }

        public void MoveEverything(IntVector2 dir)
        {
            MoveEverything(dir, true);
        }

        public void MoveEverything(IntVector2 dir, bool refreshVisuals)
        {
            tileGrid.MoveEveryThing(dir);

            if (refreshVisuals)
            {
                tileGrid.LoopBegin();
                while (tileGrid.LoopNext())
                {
                    tileGrid.LoopValueGet().newPosition(tileGrid.LoopPosition);
                }
                refresh();
            }
        }
        
        public void refreshUnitsPlacements()
        {
            tileGrid.LoopBegin();

            while (tileGrid.LoopNext())
            {
                tileGrid.LoopValueGet().unit = null;
            }

            foreach (var p in Commander.cmdRef.players.allPlayers.list)
            {
                p.unitsColl.unitsCounter.Reset();
                while (p.unitsColl.unitsCounter.Next())
                {
                    p.unitsColl.unitsCounter.sel.SetDataPosition(p.unitsColl.unitsCounter.sel.squarePos);
                }
            }
        }

        BoardSquareContent ResizeGrid2D_GetDefault(int x, int y)
        {
            BoardSquareContent m;
            if (tileGrid.TryGet(x, y, out m))
            {
                if (m.unit != null)
                    m.unit.DeleteMe();
            }
            return new BoardSquareContent();
        }

        void ResizeGrid2D_Removing(BoardSquareContent square, int x, int y)
        {
            if (square.unit != null)
                square.unit.DeleteMe();
        }
        
        void placeVpSquares()
        {
            int targetCount = Ref.rnd.Int(2, 4);

            //int placed = 0;
            List<Commander.GO.Banner> placed = new List<Commander.GO.Banner>(4);

            const int FromEdge = 2;
            const int MinSpacing = 2;
            IntVector2 rndPos;
            Rectangle2 placemnetArea = new Rectangle2(FromEdge, PlacementHeight, Size.X - FromEdge * 2, Size.Y - PlacementHeight * 2);
            while (placed.Count < targetCount)
            {
                rndPos = placemnetArea.RandomTile();
                BoardSquareContent square = tileGrid.Get(rndPos);

                bool canPlace = true;

                foreach (var m in placed)
                {
                    if ((m.position - rndPos).SideLength() < MinSpacing)
                    {
                        canPlace = false;
                        break;
                    }
                }

                if (canPlace)
                {
                    placed.Add(new Commander.GO.Banner(rndPos));
                }

            }
        }

        void deploymentTiles()
        {
            const int PlacementSquares = 26;
            const int PlacementSideEdge = 1;
            Rectangle2 setupArea = new Rectangle2(PlacementSideEdge, 0, tileGrid.Size.X - PlacementSideEdge * 2, PlacementHeight);

            for (int view = 0; view < 2; ++view)
            {
                bool firstView = view == 0;
                BoardSquareContent square = null;

                for (int i = 0; i < PlacementSquares; ++i)
                {
                    IntVector2 placementPos = IntVector2.Zero;
                    int loopCount = 0;
                    while (loopCount < 100)
                    {
                        placementPos = setupArea.RandomTile();
                        placementPos = placementTileToPosition(placementPos, firstView);
                        square = tileGrid.Get(placementPos);
                        if (square.squareType == SquareType.Grass && !square.adjacentToFlag && square.playerPlacement < 0)
                        {
                            break;
                        }
                        loopCount++;
                    }

                    square.playerPlacement = view;
                }
            }
        }

        IntVector2 placementTileToPosition(IntVector2 placeTile, bool bottomPlayer)
        {
            if (bottomPlayer)
            {
                placeTile.Y = tileGrid.Size.Y - PlacementHeight + placeTile.Y;
            }
            else
            {
                placeTile.X = tileGrid.Size.X - placeTile.X;
                placeTile.Y = PlacementHeight - placeTile.Y - 1;
            }

            return placeTile;
        }

        public int placementRowToYPosition(int row, bool bottomPlayer)
        {
            return placementTileToPosition(new IntVector2(0, row), bottomPlayer).Y;
        }

        void randomTerrain()
        {
            int numForestTiles = Ref.rnd.Int(8, 20);
            int numHillTiles = Ref.rnd.Int(4, 10);

            int numMountainTiles = Ref.rnd.Int(0, 4);
            int numWaterTiles = Ref.rnd.Int(0, 6);
            int numTownTiles = Ref.rnd.Int(0, 2);
            int numRubbleTiles = Ref.rnd.Int(0, 10);

            for (int i = 0; i < numForestTiles; ++i)
            {
                setRandomTileTo(SquareType.GreenForest, false);
            }
            for (int i = 0; i < numHillTiles; ++i)
            {
                setRandomTileTo(SquareType.GreenHill, true);
            }

            //for (int i = 0; i < numMountainTiles; ++i)
            //{
            //    setRandomTileTo(TerrainType.Mountain, false);//tileGrid.RandomMember().TerrainType = TerrainType.Mountain;
            //} 
            //for (int i = 0; i < numWaterTiles; ++i)
            //{
            //    setRandomTileTo(TerrainType.Water, false);//tileGrid.RandomMember().TerrainType = TerrainType.Water;
            //} 
            //for (int i = 0; i < numTownTiles; ++i)
            //{
            //    setRandomTileTo(TerrainType.Town, true);//tileGrid.RandomMember().TerrainType = TerrainType.Town;
            //} 
            //for (int i = 0; i < numRubbleTiles; ++i)
            //{
            //    setRandomTileTo(TerrainType.Rubble, true);//tileGrid.RandomMember().TerrainType = TerrainType.Rubble;
            //} 
        }

        void setRandomTileTo(SquareType type, bool canBeOnFlags)
        {
            BoardSquareContent square = tileGrid.RandomMember();
            if (canBeOnFlags || square.tileObjects.HasObject(TileObjectType.TacticalBanner) == false)
            {
                square.squareType = type;
                //square.refreshImageVariantions();
            }
        }

        public void refresh()
        {
            if (model == null)
            {
                model = new BoardModel();
            }
            needsRefresh = true;
        }

        public List<IntVector2> GetTag(byte id)
        {
            List<IntVector2> result = new List<IntVector2>();
            var loop = tileGrid.LoopInstance();

            while (loop.Next())
            {
                var tag = tileGrid.Get(loop.Position).tileObjects.GetObject(TileObjectType.SquareTag);
                if (tag != null)
                {
                    if (((Map.SquareTag)tag).TagId == id)
                    {
                        result.Add(loop.Position);
                    }
                }
            }

            return result;
        }

        public bool CanEndMovementOn(IntVector2 square, AbsUnit unit)
        {
            var rest = MovementRestriction(square, unit);
            return rest != MovementRestrictionType.WalkThroughCantStop && rest != MovementRestrictionType.Impassable;
        }

        public MovementRestrictionType MovementRestriction(IntVector2 square, AbsUnit unit, 
            bool ignoreUnits = false)
        {
            if (!tileGrid.InBounds(square))
            {
                return MovementRestrictionType.Impassable;
            }

            BoardSquareContent tile = tileGrid.Get(square);
            MovementRestrictionType objRestriction;

            if (!ignoreUnits && tile.unit != null && tile.unit != unit)
            {
                return unit.canMoveThrough(tile.unit) ? 
                    MovementRestrictionType.WalkThroughCantStop : MovementRestrictionType.Impassable;
            }

            PropertyMoveModifiers moveModifiers = unit.Data.properties.moveModifiers();

            if (tile.tileObjects.HasObjectWithOverridingMoveRestriction(unit, out objRestriction))
            {
                return objRestriction;
            }
            
            if (tile.HasProperty(TerrainPropertyType.Impassable))
            {
                return MovementRestrictionType.Impassable;
            }
            else if (tile.HasProperty(TerrainPropertyType.FlyOverObsticle))
            {
                if (moveModifiers.flyOverObsticles)
                {
                    return MovementRestrictionType.NoRestrictions;
                }
                return MovementRestrictionType.Impassable;
            }
            else if (tile.HasProperty(TerrainPropertyType.MustStop))
            {
                if (moveModifiers.ignoresTerrain)
                {
                    return MovementRestrictionType.NoRestrictions;
                }
                else if (unit.HasProperty(UnitPropertyType.Ignore_terrain) == false)
                {
                    return MovementRestrictionType.MustStop;
                }
            }
            else if (tile.HasProperty(TerrainPropertyType.HorseMustStop))
            {
                if (unit.cmd().data.mainType == UnitMainType.Cavalry)
                {
                    return MovementRestrictionType.MustStop;
                }
            }

            return MovementRestrictionType.NoRestrictions;            
        }

        public bool IsSpawnAvailableSquare(IntVector2 square)
        {
            return IsEmptyFloorSquare(square);
        }

        public bool IsEmptyFloorSquare(IntVector2 square, bool includeTags = true)
        {
            BoardSquareContent tile;
            if (tileGrid.TryGet(square, out tile))
            {
                return tile.unit == null &&
                    tile.IsFloor &&
                    !tile.tileObjects.HasTileFillingUnit() &&
                    (tile.tag.tagType == SquareTagType.None || !includeTags);
            }
            return false;
        }

        public bool IsFloor(IntVector2 square)
        {
            BoardSquareContent tile;

            if (tileGrid.TryGet(square, out tile))
            {
                return tile.IsFloor;
                //return !tile.HasProperty(TerrainPropertyType.Impassable) &&
                //    !tile.HasProperty(TerrainPropertyType.FlyOverObsticle);
            }

            return false;
        }

        public bool IsWall(IntVector2 square)
        {
            BoardSquareContent tile;
            if (tileGrid.TryGet(square, out tile))
            {
                 return tile.IsWall;
            }

            return false;
        }

        public void clearOutMap()
        {
            metaData = new BoardMetaData();
            toggRef.absPlayers?.clearUnits();

            var loop = tileGrid.LoopInstance();
            while (loop.Next())
            {
                tileGrid.Get(loop.Position).clear();
            }
        }
        
        public void Write(System.IO.BinaryWriter w, bool isSaveState)
        {
            tileGrid.Size.writeByte(w);
            ForXYLoop loop = new ForXYLoop(tileGrid.Size);
            while (loop.Next())
            {
                tileGrid.Get(loop.Position).Write(w);
            }
        }

        public void Read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            //Is main thread
            IntVector2 size = IntVector2.FromReadByte(r);
            tileGrid = new Grid2D<BoardSquareContent>(size);
            ForXYLoop loop = new ForXYLoop(tileGrid.Size);

            while (loop.Next())
            {
                BoardSquareContent square = new BoardSquareContent();
                tileGrid.Set(loop.Position, square);
                square.Read(r, loop.Position, version, false);            
            }

            setBounds();
        }

        public void ReadOldVersion(System.IO.BinaryReader r)
        {
            byte version = r.ReadByte();
            IntVector2 size = IntVector2.FromReadByte(r);
            tileGrid = new Grid2D<BoardSquareContent>(size);
            ForXYLoop loop = new ForXYLoop(tileGrid.Size);
            while (loop.Next())
            {
                BoardSquareContent square = new BoardSquareContent();
                square.ReadOldVersion(r, loop.Position, version);
                tileGrid.Set(loop.Position, square);
            }

            setBounds();

            if (version == 1)
            {
                //int vpSquaresCount = r.ReadByte();
                //for (int i = 0; i < vpSquaresCount; ++i)
                //{
                //    new Banner(null, IntVector2.FromByteStream(r));
                //}
            }
        }

        public void onLoadComplete()
        {
            tileGrid.LoopBegin();
            while (tileGrid.LoopNext())
            {
                tileGrid.LoopValueGet().onLoadComplete();
            }
        }

        void modelAndMetaIsReadyCheck()
        {
            if (!waitingForModelGen && !waitingForMetaGen)
            {
                toggRef.gamestate.MapLoadComplete();
            }
        }

        public void WritePosition(System.IO.BinaryWriter w, IntVector2 squarePos)
        {
            squarePos.writeByte(w);
        }

        public IntVector2 ReadPosition(System.IO.BinaryReader r)
        {
            return IntVector2.FromReadByte(r);
        }

        public bool InLineOfSight(IntVector2 fromPos, IntVector2 toPos, out IntVector2 blockingTile, 
            AbsUnit attackingUnit, bool unitsAreObsticles, bool terrainTarget)
        {
            blockingTile = IntVector2.Zero;
            BoardSquareContent targetTile;

            if (toggRef.board.tileGrid.TryGet(toPos, out targetTile))
            {
                if (!terrainTarget && targetTile.BlocksLOS())//HasProperty(TerrainPropertyType.ArrowBlock))
                {
                    blockingTile = toPos;
                    return false;
                }

                const float StepsLenght = 0.05f;
                
                Vector2 diff = (toPos - fromPos).Vec;
                int steps = (int)(diff.Length() / StepsLenght);
                diff.Normalize();
                Vector2 stepDir = diff * StepsLenght;

                //Trace with thee lines from center and sides
                Vector2 centerLine = fromPos.Vec;
                if (traceLine(fromPos, toPos, centerLine, steps, stepDir, attackingUnit, out blockingTile, unitsAreObsticles))
                {
                    return true;
                }

                const float Radius = 0.45f;
                Vector2 left = new Vector2(-diff.Y, -diff.X);
                Vector2 leftLine = centerLine + left * Radius;
                if (traceLine(fromPos, toPos, leftLine, steps, stepDir, attackingUnit, out blockingTile, unitsAreObsticles))
                {
                    return true;
                }

                Vector2 right = new Vector2(diff.Y, diff.X);
                Vector2 rightLine = centerLine + right * Radius;
                if (traceLine(fromPos, toPos, rightLine, steps, stepDir, attackingUnit, out blockingTile, unitsAreObsticles))
                {
                    return true;
                }
            }

            return false;            
        }

        public bool InLineOfSight_Simplified(IntVector2 fromPos, IntVector2 toPos, bool unitsAreObsticles)
        {
            const float StepsLenght = 0.5f;

            IntVector2 blockingTile;
            Vector2 diff = (toPos - fromPos).Vec;
            int steps = (int)(diff.Length() / StepsLenght);
            diff.Normalize();
            Vector2 stepDir = diff * StepsLenght;

            Vector2 centerLine = fromPos.Vec;
            return traceLine(fromPos, toPos, centerLine, steps, stepDir, null, out blockingTile, unitsAreObsticles);
        }

        bool traceLine(IntVector2 startTile, IntVector2 toPos, Vector2 startPos, int steps, Vector2 stepDir, 
            AbsUnit attackingUnit, out IntVector2 blockingTile, bool unitsAreObsticles)
        {
            blockingTile = IntVector2.Zero;
            IntVector2 prevTile = startTile;
            IntVector2 tile = IntVector2.Zero;
            Vector2 position = startPos;
            for (int i = 0; i < steps; ++i)
            {
                position += stepDir;
                tile.X = Convert.ToInt32(position.X);
                tile.Y = Convert.ToInt32(position.Y);

                if (tile != prevTile)
                {
                    if (tile == toPos)
                    {
                        //Reached target, end search
                        return true;
                    }
                    else
                    {
                        bool occupyingUnit = false;
                        BoardSquareContent square = toggRef.board.tileGrid.Get(tile);

                        if (unitsAreObsticles)
                        {
                            occupyingUnit = square.unit != null;
                            if (occupyingUnit)
                            {
                                if (attackingUnit != null && square.unit.globalPlayerIndex == attackingUnit.globalPlayerIndex &&
                                    attackingUnit.HasProperty(UnitPropertyType.Over_shoulder))
                                {
                                    occupyingUnit = false;
                                }
                            }
                        }

                        if (occupyingUnit || square.BlocksLOS())
                        {
                            blockingTile = tile;
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public Vector3 toModelCenter(IntVector2 pos, float terrainY)
        {
            Vector3 result = toWorldPos_Center(pos, 0);

            float modelY = 0;
            BoardSquareContent sq;
            if (toggRef.board.tileGrid.TryGet(pos, out sq) &&
                sq != null)
            {
                modelY = sq.Square.ModelHeight();
            }

            result += toggLib.TowardCamVector_Yis1 *
                    (modelY + terrainY);
            return result;
        }

        public Vector3 toGuiPos_Center(IntVector2 square, float height)
        {
            var result = toggLib.TowardCamVector_Yis1 * height;
            return VectorExt.AddXZ(result, square);
        }

        public Vector3 toWorldPos_Center(IntVector2 square, float height)
        {
            return new Vector3(square.X, height, square.Y);
        }

        public Vector3 toWorldPos_adjustForUnitZ(IntVector2 square, float height)
        {
            var result = toWorldPos_Center(square, height);
            AbsUnit unit = getUnit(square);
            if (unit != null)
            {
                result.Z += unit.Data.modelSettings.centerOffset.Z;
            }

            return result;
        }

        public Vector3 toWorldPos_TopLeft(IntVector2 square, float height)
        {
            return new Vector3(square.X - 0.5f, height, square.Y - 0.5f);
        }

        public Vector2 toWorldPosXZ_Center(IntVector2 square)
        {
            return new Vector2(square.X, square.Y);
        }

        public IntVector2 toGridPos(Vector3 worldpos)
        {
            return new IntVector2(worldpos.X, worldpos.Z);
        }

        public BoardSquareContent square(Vector3 worldpos)
        {
            BoardSquareContent sq;
            if (tileGrid.TryGet(toGridPos(worldpos), out sq) == false)
            {
                return null;
            }

            return sq;
        }

        public void onUnitMovement(AbsUnit unit, MoveLinesGroup move)
        {
            if (move != null)
            {
                for (int i = move.lines.Count - 1; i >= 0; --i)
                {
                    IntVector2 pos = move.lines[i].toPos;
                    tileGrid.Get(pos).tileObjects.stompOnTile(unit, pos, false, true);

                    BoardSquareContent nSq;
                    foreach (var dir in IntVector2.Dir8Array)
                    {
                        var npos = pos + dir;
                        if (tileGrid.TryGet(npos, out nSq))
                        {
                            nSq.tileObjects.stompOnTile(unit, pos, false, true);
                        }
                    }
                }
            }
        }

        public void OnEvent(ToGG.Data.EventType eventType)
        {
            tileGrid.LoopBegin();
            while (tileGrid.LoopNext())
            {
                tileGrid.LoopValueGet().tileObjects.OnEvent(eventType);
            }
        }

        public void soundFromSquare(IntVector2 center)
        {
            Rectangle2 area = Rectangle2.FromCenterSize(center, new IntVector2(5));

            area.SetBounds(tileGrid.Area);

            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                tileGrid.Get(loop.Position).tileObjects.stompOnTile(null, center, true, true);
            }
        }

        public void netreadTileStomp(System.IO.BinaryReader r)
        {
            IntVector2 from = ReadPosition(r);
            IntVector2 onTile = ReadPosition(r);

            tileGrid.Get(onTile).tileObjects.stompOnTile(null, from, false, false);
        }

        public AbsUnit getUnit(IntVector2 pos)
        {
            BoardSquareContent sq;
            if (tileGrid.TryGet(pos, out sq))
            {
                return sq.unit;
            }
            else
            {
                return null;
            }
        }

        public void removeAllFog()
        {
            tileGrid.LoopBegin();

            while (tileGrid.LoopNext())
            {
                tileGrid.LoopValueGet().hidden = false;
            }
            fogModel.refresh();
        }
        public IntVector2 Size { get { return tileGrid.Size; } }
    }

    enum BoardType
    { 
        Match,
        HeroQuest,
        Editor,
    }
    
}
