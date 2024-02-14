using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VikingEngine.HUD;
using VikingEngine.ToGG.Commander;
using VikingEngine.ToGG.HeroQuest;
using VikingEngine.ToGG.HeroQuest.Gadgets;
using VikingEngine.ToGG.ToggEngine.Map;
using VikingEngine.ToGG.ToggEngine.MapEditor;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.Commander.LevelSetup;

namespace VikingEngine.ToGG.ToggEngine.MapEditor
{
    partial class EditorState : AbsPlayState, VikingEngine.DataStream.IStreamIOCallback
    {
        AbsGenericPlayer player;        

        ToolType tool = ToolType.NoTool;
        SquareType selectedTerrain = SquareType.NUM_NON;
        SquareVisualProperties squareVisualProperties = new SquareVisualProperties();
        int selectedUnitType = -1;
        TileObjectType selectedTileSpecial = TileObjectType.NONE;
        SquareTag2 squareTag = SquareTag2.None;

        SelectionType selectionType = SelectionType.ObjectsAndTerrain;
        SelectionData selectionData = null;

        byte tagId = 0;
        int unitOwnerIndex;

        SpawnPointData spawnPointData = new SpawnPointData();
        Map.MapFurnishType furnish = Map.MapFurnishType.Box;
        TileItemCollData chest = new TileItemCollData(HeroQuest.LootLevel.Level1);

        DoorSettings doorsettings;
        public HeroQuest.MapGen.AbsRoomFlagSettings roomFlag = new HeroQuest.MapGen.AreaTypeSpawn();

        HUD.MessageHandler messageHandler;
        FileManager filemanager;

        IntVector2 setSize;
        Graphics.Text2 infoCoords, infoSquare, infoTool, infoTag;
        bool waitForPaintKeyUp = false;
        //IntVector2 prevPaintPos = IntVector2.Zero;
        IntVector2 mouseDownPos = IntVector2.Zero;
        bool hasRectangleMouseDown = false;
        EditorInputMap input;
        ToolShape toolShape;
        MouseToolTip mouseToolTip;
        AreaDisplay areaDisplay = null;
        bool hasTestSpawned = false;

        public EditorState(GameMode editorMode)
            : base(editorMode)
        {
            toggRef.editor = this;
            doorsettings = new DoorSettings();

            messageHandler = new HUD.MessageHandler(3, SpriteName.WhiteArea, 4000);
            unitMessages = new ToggEngine.Display3D.UnitMessagesHandler();
            filemanager = new FileManager(this);
            gameSetup = new GameSetup();

            gameSetup.lobbyMembers = new List<AbsLobbyMember>
            {
                new DesignPlayerLobbyMember(),
                new AiLobbyMember(),
            };

            input = new EditorInputMap();
            new Board(gameSetup, BoardType.Editor);

            if (editorMode == GameMode.HeroQuest)
            {
                new HeroQuest.Data.AllUnitsData();
                new HeroQuest.Players.PlayerCollection().sortPlayers();
                player = hqRef.players.editorHost;
            }
            else
            {
                new Commander.Players.PlayerCollection().createPlayers(gameSetup);
                player = Commander.cmdRef.players.ActiveLocalPlayer();
            }

            {//Info texts
                float infoH = Engine.Screen.SmallIconSize;
                Vector2 pos = Engine.Screen.SafeArea.LeftBottom;
                pos.Y -= infoH;

                infoCoords = new Graphics.Text2("--COORD--", LoadedFont.Bold, pos,
                    infoH, Color.White, ImageLayers.Background2);

                pos.Y -= infoH + Engine.Screen.BorderWidth;

                infoTag = new Graphics.Text2("--TAG--", LoadedFont.Bold, pos,
                    infoH, Color.White, ImageLayers.Background2);

                pos.Y -= infoH + Engine.Screen.BorderWidth;

                infoSquare = new Graphics.Text2("--SQUARE--", LoadedFont.Bold, pos,
                    infoH, Color.White, ImageLayers.Background2);

                pos.Y -= infoH + Engine.Screen.BorderWidth;

                infoTool = new Graphics.Text2("--TOOL--", LoadedFont.Bold, pos,
                    infoH, Color.White, ImageLayers.Background2);
            }
                        
            Graphics.Text2 inputOverview = new Graphics.Text2(
                "Esc: MENU" + Environment.NewLine +
                "Alt: Pick" + Environment.NewLine +
                "Ctrl: Pencil" + Environment.NewLine +
                "Shift: Bucket" + Environment.NewLine +
                "Ctrl + Shift: Replace all", LoadedFont.Console,
                Engine.Screen.SafeArea.Position,
                Engine.Screen.SmallIconSize * 0.8f, Color.White, ImageLayers.Background4); 
            
            mouseToolTip = new MouseToolTip(player.mapControls);
            player.mapControls.setAvailable(MapSquareAvailableType.None);

            //Ref.analytics.onStateChange(GameStateType.MapEditor);

            new MenuSystem(Input.InputSource.DefaultPC);
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            updateToolShape();

            if (filemanager.lockedInSaving)
            {
                return;
            }
            else if (toggRef.menu.Open)
            {
                if (toggRef.menu.Update())
                {
                    toggRef.menu.CloseMenu();
                }
                hasRectangleMouseDown = false;
            }
            else if (waitForPaintKeyUp)
            {
                if (toggRef.inputmap.click.IsDown == false)
                {
                    waitForPaintKeyUp = false;
                }
            }
            else
            {
                if (toggRef.inputmap.menuInput.openCloseInputEvent())
                {
                    mainMenu();
                }
                
                if (toggRef.inputmap.click.DownEvent)
                {
                    mouseDownPos = player.mapControls.selectionIntV2;

                    if (tool == ToolType.Gadgets)
                    {
                        gadgetsSquareOptions();
                    }
                    else
                    {
                        switch (toolShape)
                        {
                            case ToolShape.Pick:
                                pick();
                                break;
                            case ToolShape.Pencil:
                                paintOnSquare(player.mapControls.selectionIntV2);
                                break;
                            case ToolShape.Rectangle:
                                hasRectangleMouseDown = true;
                                break;
                            case ToolShape.Bucket:
                                if (tool == ToolType.Terrain)
                                {
                                    bucketFill(player.mapControls.selectionIntV2);
                                }
                                break;
                            case ToolShape.ReplaceAll:
                                if (tool == ToolType.Terrain)
                                {
                                    replaceAll(player.mapControls.selectionIntV2);
                                }
                                break;
                        }
                    }
                }
                else
                {
                    if (toggRef.inputmap.click.IsDown)
                    {
                        if (player.mapControls.isOnNewTile)
                        {
                            if (toolShape == ToolShape.Pencil)
                            {
                                paintOnSquare(player.mapControls.selectionIntV2);
                            }
                            else if (hasRectangleMouseDown)
                            {
                                player.mapControls.selectArea(paintArea);
                            }
                        }
                    }
                    else
                    {
                        player.mapControls.removeMultiselection();

                        if (toggRef.inputmap.click.UpEvent)
                        {
                            if (toolShape == ToolShape.Rectangle && hasRectangleMouseDown)
                            {//Rectangle fill
                                if (tool == ToolType.SelectArea)
                                {
                                    selectOptions();
                                }
                                else
                                {
                                    ForXYLoop loop = new ForXYLoop(paintArea);
                                    while (loop.Next())
                                    {
                                        paintOnSquare(loop.Position);
                                    }
                                }
                            }

                            hasRectangleMouseDown = false;
                        }
                    }
                }
                
                player.mapControls.updateMapMovement(false);

                if (player.mapControls.isOnNewTile)
                {
                    if (areaDisplay != null)
                    {
                        areaDisplay.DeleteMe();
                        areaDisplay = null;
                    }

                    var flag = player.mapControls.selectedTile.tileObjects.GetObject(TileObjectType.RoomFlag);
                    if (flag != null)
                    {
                        areaDisplay = new AreaDisplay(((HeroQuest.MapGen.RoomFlag)flag).tiles);
                    }
                }

                toggRef.cam.update();
                //camera.Time_Update(time);

                messageHandler.Time_Update(Ref.DeltaTimeMs);

                updateInfo();
                
            }

            unitMessages.update();
            toggRef.board.update();
        }

        

        void updateToolShape()
        {
            SpriteName icon;

            if (input.pickTool.IsDown)
            {
                icon = SpriteName.IconColorPick;
                toolShape = ToolShape.Pick;
            }
            else if (input.freePaint.IsDown)
            {
                if (input.bucketTool.IsDown)
                {
                    icon = SpriteName.cmdWarningTriangle;
                    toolShape = ToolShape.ReplaceAll;
                }
                else
                {
                    icon = SpriteName.EditorToolPencil;
                    toolShape = ToolShape.Pencil;
                }
            }
            else if (input.bucketTool.IsDown)
            {
                icon = SpriteName.LFAppearHatTypeBucket;
                toolShape = ToolShape.Bucket;
            }
            else
            {
                icon = SpriteName.IconBuildMoveSelection;
                toolShape = ToolShape.Rectangle;
            }

            mouseToolTip.toolicon.SetSpriteName(icon);
        }

        void updateInfo()
        {
            if (player.mapControls.selectedTile != null)
            {
                infoCoords.TextString =
                       "X:" + player.mapControls.selectionIntV2.X.ToString() +
                       " Y:" + player.mapControls.selectionIntV2.Y.ToString();

                infoTool.TextString = "Tool: " + tool.ToString();
                switch (tool)
                {
                    case ToolType.SpecialTile:
                        infoTool.TextString += "(" + selectedTileSpecial.ToString() + ")";
                        break;
                    case ToolType.Terrain:
                        infoTool.TextString += "(" + selectedTerrain.ToString() + ")";
                        break;
                    case ToolType.Units:
                        infoTool.TextString += "(" + selectedUnitType.ToString() + ")";
                        break;
                }

                infoSquare.TextString = "";
                if (player.mapControls.selectedTile.unit != null)
                {
                    infoSquare.TextString += player.mapControls.selectedTile.unit.ToString();
                    if (player.mapControls.selectedTile.unit.HasTag)
                    {
                        infoSquare.TextString += "(TAGGED)";
                    }
                }

                if (arraylib.HasMembers(player.mapControls.selectedTile.tileObjects.members))
                {
                    foreach (var m in player.mapControls.selectedTile.tileObjects.members)
                    {
                        infoSplit();
                        infoSquare.TextString += m.ToString();
                    }
                }

                infoSplit();
                infoSquare.TextString += player.mapControls.selectedTile.squareType.ToString();

                infoSquare.TextString = "Square: " + infoSquare.TextString;

                infoTag.TextString = player.mapControls.selectedTile.tag.ToString();
            }
        }

        void infoSplit()
        {
            if (infoSquare.TextString.Length > 0)
            {
                infoSquare.TextString += " | ";
            }
        }

        void pick()
        {
            //PICK
            string message;
            AbsUnit unit = player.mapControls.selectedTile.unit;

            if (unit != null)
            {
                tool = ToolType.Units;

                if (toggRef.mode == GameMode.HeroQuest)
                {
                    selectedUnitType = (int)unit.hq().data.Type;
                }
                else
                {
                    selectedUnitType = (int)unit.cmd().data.Type;
                }

                unitOwnerIndex = unit.globalPlayerIndex;
                unitStartHealth = unit.health.Value;
                message = selectedUnitType.ToString();
            }
            else if (player.mapControls.selectedTile.tileObjects.HasMembers)
            {
                tool = ToolType.SpecialTile;
                AbsTileObject obj = arraylib.Last(player.mapControls.selectedTile.tileObjects.members);

                selectedTileSpecial = obj.Type;

                switch (obj.Type)
                {
                    case TileObjectType.Door:
                        var door = obj as Door;
                        doorsettings = door.sett.Clone();
                        break;
                    case TileObjectType.SquareTag:
                        tagId = ((Map.SquareTag)obj).TagId;
                        break;
                    case TileObjectType.SpawnPoint:
                        var spawn = obj as Map.SpawnPoint;
                        spawnPointData = spawn.data;
                        break;
                    case TileObjectType.RoomFlag:
                        roomFlag = ((HeroQuest.MapGen.RoomFlag)obj).settings.Clone();
                        toggRef.menu.OpenMenu(false);
                        roomFlag.settingsToMenu();
                        break;
                    case TileObjectType.ItemCollection:
                        var itemColl = obj as TileItemCollection;
                        if (itemColl.data.Chest)
                        {
                            chest = itemColl.data;
                        }
                        else
                        {
                            tool = ToolType.Gadgets;
                        }
                        break;
                }

                message = obj.ToString();
            }
            else if (player.mapControls.selectedTile.tag.HasValue)
            {
                tool = ToolType.TileTags;
                squareTag = player.mapControls.selectedTile.tag;

                message = squareTag.ToString();
            }
            else
            {
                tool = ToolType.Terrain;
                selectedTerrain = player.mapControls.selectedTile.squareType;
                squareVisualProperties = player.mapControls.selectedTile.visualProperties;
                message = selectedTerrain.ToString();
            }

            messageHandler.Print("Picked: " + message);
        }
        
        Rectangle2 paintArea => Rectangle2.FromTwoTilePoints(mouseDownPos, player.mapControls.selectionIntV2);

        
        void bucketFill(IntVector2 startPos)
        {
            List<IntVector2> area = new List<IntVector2>(32);
            Grid2D<bool> used = new Grid2D<bool>(toggRef.board.Size);

            findEqualType(startPos);
            paintOnSquare(area);

            void findEqualType(IntVector2 centerPos)
            {
                area.Add(centerPos);
                used.Set(centerPos, true);

                foreach (var dir in IntVector2.Dir4Array)
                {
                    var npos = centerPos + dir;
                    if (used.InBounds(npos) && !used.Get(npos))
                    {
                        if (isSameType(startPos, npos)) //nSquare.squareType == startSquare.squareType)
                        {
                            findEqualType(npos);
                        }
                        else
                        {
                            used.Set(npos, true);
                        }
                    }
                }
            }
        }

        void replaceAll(IntVector2 position)
        {
            ForXYLoop loop = new ForXYLoop(toggRef.board.Size);
            
            while (loop.Next())
            {
                if (position != loop.Position &&
                    isSameType(position, loop.Position))
                {
                    paintOnSquare(loop.Position);
                }
            }

            paintOnSquare(position);
        }

        bool isSameType(IntVector2 pos1, IntVector2 pos2)
        {
            var sq1 = toggRef.Square(pos1);
            var sq2 = toggRef.Square(pos2);

            return sq1.squareType == sq2.squareType;
        }

        void paintOnSquare(List<IntVector2> area)
        {
            foreach (var pos in area)
            {
                paintOnSquare(pos);
            }
        }

        void paintOnSquare(IntVector2 pos)
        {
            BoardSquareContent tile = toggRef.board.tileGrid.Get(pos);

            switch (tool)
            {
                case ToolType.Terrain:
                    tile.squareType = selectedTerrain;
                    tile.visualProperties = squareVisualProperties;
                    toggRef.board.refresh();
                    break;
                case ToolType.TileTags:
                    if (squareTag.Equals(tile.tag))
                    {
                        tile.tag = SquareTag2.None;
                    }
                    else
                    {
                        tile.tag = squareTag;
                    }
                    toggRef.board.refresh();
                    break;
                case ToolType.Units:
                    if (tile.unit == null)
                    {
                        AbsUnit unit;
                        if (toggRef.mode == GameMode.HeroQuest)
                        {
                            unit = new HeroQuest.Unit(pos, (HeroQuest.HqUnitType)selectedUnitType, HeroQuest.hqRef.players.dungeonMaster);
                        }
                        else
                        {
                            unit = new Commander.GO.Unit(pos, (UnitType)selectedUnitType, Commander.cmdRef.players.allPlayers.list[unitOwnerIndex]);
                            unit.setHealth(unitStartHealth);
                        }
                    }
                    else
                    {
                        removeSelectedUnit();
                    }
                    break;
                case ToolType.UnitTags:
                    if (tile.unit != null)
                    {
                        if (tile.unit.hq().HasTag)
                        {
                            tile.unit.hq().removeTag();
                            new ToggEngine.Display3D.UnitMessageRichbox(tile.unit, "Tag removed");

                        }
                        else
                        {
                            tile.unit.hq().setTag();
                            new ToggEngine.Display3D.UnitMessageRichbox(tile.unit, "Tagged");
                        }
                    }
                    break;
                case ToolType.SpecialTile:
                    var hasObj = tile.tileObjects.GetObject(selectedTileSpecial);

                    if (hasObj != null)
                    {
                        if (selectedTileSpecial == TileObjectType.Lootdrop)
                        {
                            var loot = (HeroQuest.Gadgets.Lootdrop)hasObj;
                            if (loot.Count < 4)
                            {
                                loot.StackOne();
                            }
                            else
                            {
                                loot.DeleteMe();
                            }
                        }
                        else
                        {
                            hasObj.DeleteMe();
                        }                        
                    }
                    else
                    {
                        object args = null;

                        switch (selectedTileSpecial)
                        {
                            case TileObjectType.Door:
                                args =  doorsettings.Clone();
                                break;
                            case TileObjectType.SpawnPoint:
                                args = spawnPointData;
                                break;
                            case TileObjectType.SquareTag:
                                args = tagId;
                                break;
                            case TileObjectType.Furnishing:
                                args = furnish;
                                break;
                            case TileObjectType.ItemCollection:
                                args = chest;
                                break;
                            case TileObjectType.RoomFlag:
                                args = roomFlag.Clone();
                                toggRef.board.refresh();
                                break;
                            case TileObjectType.EventFlag:
                                args = eventFlagId;
                                break;
                        }

                        TileObjLib.CreateObject(selectedTileSpecial, pos, args, false);
                    }
                    break;
                    
            }
        }    

        void removeSelectedUnit()
        {
            if (player.mapControls.selectedTile.unit != null)
            {
                player.mapControls.selectedTile.unit.DeleteMe();
            }
        }
        
        void onFileNameChange(int user, string result, int index)
        {
            filemanager.fileName = result;
        }

        void save()
        {
            filemanager.saveMap();
        }

        override protected void loadCustomMap(string fileName, bool fromStorage)
        {
            filemanager.loadFile(fileName, fromStorage);
        }

        //void listSaveFiles(bool storage)
        //{
        //    var layout = new GuiLayout("Loading...", toggRef.menu.menu);
        //    {
        //        new GuiTextButton("Cancel", null, toggRef.menu.menu.PopLayout, false, layout);
        //    } layout.End();

        //    new Timer.Asynch2ArgTrigger<int, bool>(loadFiles_Asynch, toggRef.menu.menu.PageId, storage);
        //}

        //void loadFiles_Asynch(int pageId, bool storage)
        //{
        //    var f = filemanager.mapfilePath(storage);
        //    f.FileName = "";
        //    string[] files = f.listFilesInDir();
        //    new Timer.Action3ArgTrigger<int, string[], bool>(loadFilesComplete, pageId, files, storage);
        //}

        //void loadFilesComplete(int pageId, string[] files, bool storage)
        //{
        //    if (toggRef.menu.Open &&
        //        pageId == toggRef.menu.menu.PageId)
        //    {
        //        toggRef.menu.menu.PopLayout();


        //        var layout = new GuiLayout("Load Map", toggRef.menu.menu);
        //        {
        //            if (files.Length == 0)
        //            {
        //                new GuiLabel("No files", layout);
        //            }
        //            else
        //            {
        //                int pathLength = filemanager.mapfilePath(true).CompleteDirectory.Length + 1;
        //                foreach (var m in files)
        //                {

        //                    string name = System.IO.Path.GetFileName(m);
        //                    new GuiTextButton(name, null, new GuiAction2Arg<string, bool>(loadFile, name, storage), false, layout);
        //                }
        //            }
        //        } layout.End();
        //    }
        //}

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            messageHandler.Print((save ? "Save" : "Load") + " complete (" + filemanager.fileName + ")");

            if (save)
            {
                closeMenu();
            }
            else
            {
                hasTestSpawned = false;
                //if (redoSpawnWaiting)
                //{
                //    redoSpawnWaiting = false;
                //    testSpawnCommit();
                //}
            }
            
            
        }

        public override void MapLoadComplete()
        {
            base.MapLoadComplete();
            if (redoSpawnWaiting)
            {
                redoSpawnWaiting = false;
                testSpawnCommit();
            }
        }

        int boardWidthProperty(bool set, int value)
        {
            if (set)
            {
                setSize.X = value;
            }
            return setSize.X;
        }

        int boardHeightProperty(bool set, int value)
        {
            if (set)
            {
                setSize.Y = value;
            }
            return setSize.Y;
        }

        void applyNewSize()
        {
            toggRef.board.Resize(setSize);
            toggRef.menu.CloseMenu();
        }

        void terrainMenu()
        {
            GuiLayout layout = new GuiLayout("Terrain", toggRef.menu.menu, GuiLayoutMode.MultipleColumns, null);
            {
                foreach (var type in Map.SquareDic.Available)//for (SquareType type = 0; type < SquareType.NUM_NON; ++type)
                {
                    var data = toggRef.sq.Get(type);
                    if (data != null)
                    {
                        new HUD.GuiBigIcon(data.LabelImage(), data.Name, new GuiAction1Arg<SquareType>(viewTerrainType, type), false, layout);
                    }
                }
            }layout.End();
        }

        void viewTerrainType(SquareType type)
        {
            toggRef.menu.TerrainInfoMenu(type, useTerrainButton);
        }
        void useTerrainButton(SquareType type, GuiLayout layout)
        {
            new GuiTextButton("USE", null, new GuiAction1Arg<SquareType>(selectTerrainType, type), false, layout);
            
            listSquareProperties(type, layout);
        }

        void listSquareProperties(SquareType type, GuiLayout layout)
        {
            var data = toggRef.sq.Get(type);

            var variants = data.Variants();
            if (variants != null)
            {
                List<GuiOption<int>> variantOpt = new List<GuiOption<int>>(variants.Length);
                for (int i = 0; i < variants.Length; ++i)
                {
                    variantOpt.Add(new GuiOption<int>(variants[i], i));
                }
                new GuiOptionsList<int>(SpriteName.NO_IMAGE, "Variant", variantOpt, visualVariantProperty, layout);
            }            
        }

        public int visualVariantProperty(bool set, int value)
        {
            if (set)
            {
                squareVisualProperties.variant = (byte)value;
            }
            return squareVisualProperties.variant;
        }
        
        void selectTerrainType(SquareType type)
        {
            //menu.CloseMenu();
            toggRef.menu.CloseMenu();
            selectedTerrain = type;
            tool = ToolType.Terrain;
        }
                

        Map.MapFurnishType furnishProperty(bool set, Map.MapFurnishType value)
        {
            if (set)
            {
                furnish = value;
            }
            return furnish;
        }

        LootLevel chestProperty(bool set, LootLevel value)
        {
            if (set)
            {
                chest.lootLevel = value;
            }
            return chest.lootLevel;
        }

        int spawnPlayerProperty(bool set, int value)
        {
            if (set)
            {
                spawnPointData.playerIx = value;
            }
            return spawnPointData.playerIx;
        }
        int spawnTypeProperty(bool set, int value)
        {
            if (set)
            {
                spawnPointData.spawnType = value;
            }
            return spawnPointData.spawnType;
        }
        
        void selectTagId(int id)
        {
            tagId = (byte)id;
            closeMenu();
        }

        void selectUnitType(UnitType utype)
        {
            selectedUnitType = (int)utype;
            tool = ToolType.Units;

            toggRef.menu.UnitInfoMenu(utype, null, selectUnitInsert);
        }

        

        void selectUnitInsert(UnitType utype, GuiLayout layout)
        {
            new GuiLabel("Select Owner", layout);
            for (int i = 0; i < 2; ++i)
            {
                new GuiTextButton("Player" + TextLib.IndexToString(i), null, new GuiAction1Arg<int>(selectUnitOwner, i), false, layout);
            }
            new GuiSectionSeparator(layout);

            unitStartHealth = cmdRef.units.GetUnit(utype).startHealth;
            if (unitStartHealth > 1)
            {
                new GuiIntSlider(SpriteName.cmdStatsHealth, "Health", startHealthProperty, new IntervalF(1, unitStartHealth), false, layout);
            }
        }

        int unitStartHealth;
        int startHealthProperty(bool set, int value)
        {
            if (set)
            {
                unitStartHealth = value;
            }
            return unitStartHealth;
        }

        void selectUnitOwner(int playerNum)
        {
            toggRef.menu.CloseMenu();
            unitOwnerIndex = playerNum;
        }

        void exit()
        {
            new GameState.MainMenuState();
        }
    }

    enum ToolType
    {
        NoTool,
        SelectArea,

        Terrain,
        Units,
        SpecialTile,
        TileTags,
        UnitTags,
        Gadgets,
    }

    enum ToolShape
    {
        Rectangle,
        Pencil,
        Pick,
        Bucket,
        ReplaceAll,
    }

    enum SelectionType
    {
        Objects,
        Terrain,
        ObjectsAndTerrain,
    }
}
