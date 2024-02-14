using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VikingEngine.HUD;
using VikingEngine.ToGG.HeroQuest;
using VikingEngine.ToGG.HeroQuest.Gadgets;
using VikingEngine.ToGG.ToggEngine.Map;
using VikingEngine.ToGG.ToggEngine.MapEditor;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.MapEditor
{
    partial class EditorState
    {   
        void mainMenu()
        {
            waitForPaintKeyUp = true;
            toggRef.menu.OpenMenu(false);
            GuiLayout layout = new GuiLayout("Designer Menu", toggRef.menu.menu);
            {
                if (hasTestSpawned)
                {
                    new GuiTextButton("Undo spawn", null, undoSpawn, false, layout);
                    new GuiTextButton("Redo spawn", "Test the spawn with another seed", redoSpawn, false, layout);
                    new GuiSectionSeparator(layout);
                }

                new GuiTextButton("Terrain", null, terrainMenu, true, layout);
                new GuiTextButton("Tile tags", null, tileTagsMenu, true, layout);
                new GuiTextButton("Unit tags", "Killing all tagged units will end the game (more options will come)", unitTagsLink, false, layout);
                new GuiTextButton("Room flag", "Spawn settings for an area", 
                    new GuiAction1Arg<TileObjectType>(selectSpecialTileType,  TileObjectType.RoomFlag), 
                    true, layout);
                if (PlatformSettings.DevBuild)
                {
                    new GuiTextButton("Event flag", null,
                        new GuiAction1Arg<TileObjectType>(selectSpecialTileType, TileObjectType.EventFlag),
                        true, layout);
                }
                new GuiTextButton("Units", null, hqunitsMenu, true, layout);
                new GuiTextButton("Tile Objects", null, tilesObjectsMenu, true, layout);
                new GuiTextButton("Gadgets", null, gadgetsTool, false, layout);
                new GuiSectionSeparator(layout);
                new GuiTextButton("Selection tool", "Use to copy/paste areas", selectionTool, false, layout);
                new GuiTextButton("Board size", null, boardSizeMenu, true, layout);
                new GuiTextButton("Move everything", null, moveEverythingMenu, true, layout);
                new GuiSectionSeparator(layout);
                new GuiTextButton("Test spawn", null, testSpawnWarning, true, layout);
                new GuiTextButton("Save/Load", null, saveOrLoadMenu, true, layout);
                new GuiTextButton("Play now", null, playNow, false, layout);

                new GuiLabel("A map need 'Tile tags>Entrance' for player spawn", layout);


                if (tool == ToolType.SpecialTile)
                {
                    switch (selectedTileSpecial)
                    {
                        case TileObjectType.SpawnPoint:
                            spawnPointContent(layout);
                            break;
                        case TileObjectType.Furnishing:
                            furnishContent(layout);
                            break;
                        case TileObjectType.ItemCollection:
                            chestContent(layout);
                            break;
                        case TileObjectType.Door:
                            doorsettings.toEditorSettings(layout);
                            break;
                        case TileObjectType.RoomFlag:
                            new GuiSectionSeparator(layout);
                            roomFlag.toEditorSettings(layout);
                            break;
                        case TileObjectType.EventFlag:
                            eventFlagContent(layout);
                            break;

                    }
                }
                if (tool == ToolType.Terrain)
                {
                    listSquareProperties(selectedTerrain, layout);
                }

                if (tool == ToolType.TileTags)
                {
                    if (SquareTagHasSettings)
                    {
                        tileTagSettings(layout);
                    }
                }

                toggRef.menu.quitToMenuButton(layout);
            }
            layout.End();
        }

        void selectOptions()
        {
            toggRef.menu.OpenMenu(false);

            GuiLayout layout = new GuiLayout("Select options", toggRef.menu.menu);
            {
                new GuiTextButton("Cut", null, selectionCut, false, layout);
                new GuiTextButton("Copy", null, selectionCopy, false, layout);
                if (selectionData != null)
                {
                    new GuiTextButton("Paste", null, selectionPaste, false, layout);
                }
                new GuiTextButton("Clear", null, selectionClear, false, layout);
                new GuiSectionSeparator(layout);
                new GuiTextButton("CANCEL", null, closeMenu, false, layout);
            }
            layout.End();
        }

        void selectionCut()
        {
            selectionCopy();
            selectionClear();
        }
        void selectionCopy()
        {
            selectionData = new SelectionData(paintArea);
            closeMenu();
        }
        void selectionPaste()
        {
            selectionData.paste(paintArea.pos);
            toggRef.board.refresh();
            closeMenu();
        }

        void selectionClear()
        {
            ForXYLoop loop = new ForXYLoop(paintArea);
            while (loop.Next())
            {
                toggRef.board.tileGrid.Get(loop.Position).ClearAll();
            }

            toggRef.board.refresh();

            closeMenu();
        }

        void testSpawnWarning()
        {
            GuiLayout layout = new GuiLayout("Test spawn", toggRef.menu.menu);
            {
                new GuiLabel("Warning! Save first, the map will change", layout);
                new GuiTextButton("SPAWN", null, testSpawnCommit, false, layout);
            }
            layout.End();
        }

        void playNow()
        {
            filemanager.writeMapToMemory();
            new HeroQuest.Lobby.LobbyState(true, QuestName.None).initEditorPlay(filemanager);
        }

        void unitTagsLink()
        {
            tool = ToolType.UnitTags;
            toggRef.menu.CloseMenu();
        }

        string resetMapName;

        void testSpawnCommit()
        {
            toggRef.menu.CloseMenu();
            resetMapName = filemanager.fileName;
            filemanager.fileName += "_spawned";

            hqRef.setup.playerCountDifficulty = 1f;
            new HeroQuest.MapGen.SpawnManager();

            toggRef.NewSeed();
            hasTestSpawned = true;
        }

        void undoSpawn()
        {
            filemanager.loadFile(resetMapName, true);
            toggRef.menu.CloseMenu();
        }

        bool redoSpawnWaiting = false;
        void redoSpawn()
        {
            redoSpawnWaiting = true;
            undoSpawn();
            //testSpawnCommit();
        }


        void tilesObjectsMenu()
        {
            GuiLayout layout = new GuiLayout("Tile Objects", toggRef.menu.menu, GuiLayoutMode.MultipleColumns, null);
            {
                //tilesObjectsLink(TileObjectType.SpawnPoint, true);
                //tilesObjectsLink(TileObjectType.SquareTag, true);
                
                tilesObjectsLink(TileObjectType.DamageTrap, false);
                tilesObjectsLink(TileObjectType.RougeTrap, false);
                tilesObjectsLink(TileObjectType.DeathTrap, false);
                tilesObjectsLink(TileObjectType.NetTrap, false);
                tilesObjectsLink(TileObjectType.SpringTrap, false);
                new GuiSectionSeparator(layout);
                tilesObjectsLink(TileObjectType.Lootdrop, false);
                tilesObjectsLink(TileObjectType.ItemCollection, true);
                tilesObjectsLink(TileObjectType.Piggybank, false);
                tilesObjectsLink(TileObjectType.Campfire, false);
                tilesObjectsLink(TileObjectType.Door, true);
                new GuiSectionSeparator(layout);
                tilesObjectsLink(TileObjectType.Furnishing, true);
                tilesObjectsLink(TileObjectType.FoodStorage, false);
                tilesObjectsLink(TileObjectType.AlarmBell, false);
            }
            layout.End();

            void tilesObjectsLink(TileObjectType type, bool moreArrow)
            {
                new GuiTextButton(type.ToString(), null, new GuiAction1Arg<TileObjectType>(selectSpecialTileType, type), moreArrow, layout);
            }
        }

        void gadgetsSquareOptions()
        {
            var available = new GadgetsCatalogue().listAll();
            if (hqRef.items == null)
            {
                new ItemManager();
            }

            var items = hqRef.items.groundCollection(player.mapControls.selectionIntV2, false);
            
            toggRef.menu.OpenMenu(false);
            GuiLayout layout = new GuiLayout("Gadgets", toggRef.menu.menu, GuiLayoutMode.MultipleColumns, null);
            {
                new GuiLabel("Add", layout);
                foreach (var m in available)
                {
                    new GuiIcon(m.Icon, m.Name, new GuiAction2Arg<AbsItem, bool>(addGadget, m, true), false, layout);
                }

                if (items != null)
                {
                    new GuiSectionSeparator(layout);
                    new GuiLabel("Remove", layout);
                    foreach (var m in items.items)
                    {
                        new GuiIcon(m.Icon, m.Name, new GuiAction2Arg<AbsItem, bool>(addGadget, m, false), false, layout);
                    }
                }
            }
            layout.End();
        }

        void addGadget(AbsItem item, bool add)
        {
            if (add)
            {
                hqRef.items.moveToGround(item, player.mapControls.selectionIntV2);
            }
            else
            {
                hqRef.items.removeFromGround(item, player.mapControls.selectionIntV2);
            }

            toggRef.menu.menu.PopLayout();
            gadgetsSquareOptions();
        }

        void tileTagsMenu()
        {
            GuiLayout layout = new GuiLayout("Tile Tags", toggRef.menu.menu, GuiLayoutMode.MultipleColumns, null);
            {
                new GuiTextButton("None", null, 
                    new GuiAction1Arg<SquareTagType>(mainSquareTagSelect, SquareTagType.None), false, layout);
                new GuiTextButton("Hero start", "Heroes will spawn here",
                    new GuiAction1Arg<SquareTagType>(mainSquareTagSelect, SquareTagType.HeroStart), true, layout);
                new GuiTextButton("Room divider", "Set area limits for the room flag",
                    new GuiAction1Arg<SquareTagType>(mainSquareTagSelect, SquareTagType.RoomDivider), false, layout);
                new GuiTextButton("Tag", "Marker for scripted events",
                    new GuiAction1Arg<SquareTagType>(mainSquareTagSelect, SquareTagType.Tag), true, layout);
                new GuiTextButton("Map entrance", "Enter and exit point for scripted events",
                   new GuiAction1Arg<SquareTagType>(mainSquareTagSelect, SquareTagType.MapEnter), true, layout);

            }
            layout.End();
        }

        bool SquareTagHasSettings => squareTag.tagType != SquareTagType.None && squareTag.tagType != SquareTagType.RoomDivider;

        void mainSquareTagSelect(SquareTagType type)
        {
            squareTag.tagType = type;
            squareTag.tagId = 0;
            tool = ToolType.TileTags;
                       

            if (SquareTagHasSettings)
            {
                GuiLayout layout = new GuiLayout(type.ToString() + " settings", toggRef.menu.menu, GuiLayoutMode.MultipleColumns, null);
                {
                    tileTagSettings(layout);
                    new GuiTextButton("OK", null, closeMenu, false, layout);
                }
                layout.End();
            }
            else
            {
                closeMenu();
            }
        }

        void tileTagSettings(GuiLayout layout)
        {
            switch (squareTag.tagType)
            {
                case SquareTagType.HeroStart:
                    var options = new List<GuiOption<int>>{
                        new GuiOption<int>("High", 0),
                        new GuiOption<int>("Medium", 1),
                        new GuiOption<int>("Low", 2)
                    };

                    new GuiOptionsList<int>(SpriteName.NO_IMAGE, "Priority", options, tagIdProperty, layout);
                    break;

                default:
                    new GuiIntSlider(SpriteName.NO_IMAGE, "ID", tagIdProperty, new IntervalF(0, 19), false, layout);
                    break;
            }
        }
        
        int tagIdProperty(bool set, int value)
        {
            if (set)
            {
                squareTag.tagId = (byte)value;
            }
            return squareTag.tagId;
        }

        void selectSpecialTileType(TileObjectType type)
        {
            selectedTileSpecial = type;
            tool = ToolType.SpecialTile;

            switch (type)
            {
                case TileObjectType.SquareTag:
                    listTags();
                    break;
                case TileObjectType.SpawnPoint:
                    spawnPointsMenu();
                    break;
                case TileObjectType.Furnishing:
                    furnishMenu();
                    break;
                case TileObjectType.ItemCollection:
                    chestMenu();
                    break;
                case TileObjectType.Door:
                    doorMenu();
                    break;
                case TileObjectType.RoomFlag:
                    roomFlag.settingsToMenu();
                    break;
                case TileObjectType.EventFlag:
                    eventFlagMenu();
                    break;
                default:
                    closeMenu();
                    break;
            }

        }

        void closeMenu()
        {
            toggRef.menu.CloseMenu();
        }

        void saveOrLoadMenu()
        {
            GuiLayout layout = new GuiLayout("Map storage", toggRef.menu.menu);
            {
                new GuiTextInputbox(filemanager.fileName, onFileNameChange, layout);
                new GuiTextButton("Save Map", "Will store in " + VikingEngine.DataStream.FilePath.StorageDirectory(), save, false, layout);
                new GuiTextButton("Load Map", null, new GuiAction1Arg<bool>(listSaveFiles, true), true, layout);
                new GuiTextButton("Copy Retail map", null, new GuiAction1Arg<bool>(listSaveFiles, false), true, layout);

            }
            layout.End();
        }

        void boardSizeMenu()
        {
            setSize = toggRef.board.Size;

            GuiLayout layout = new GuiLayout("Board Size", toggRef.menu.menu);
            {
                new GuiIntSlider(SpriteName.NO_IMAGE, "Width", boardWidthProperty, toggLib.BoardSizeLimits_Width, false, layout);
                new GuiIntSlider(SpriteName.NO_IMAGE, "Height", boardHeightProperty, toggLib.BoardSizeLimits_Height, false, layout);
                new GuiTextButton("Apply new Size", null, applyNewSize, false, layout);
            }
            layout.End();
        }

        void selectionTool()
        {
            tool = ToolType.SelectArea;
            closeMenu();
        }

        void gadgetsTool()
        {
            tool = ToolType.Gadgets;
            closeMenu();
        }

        void moveEverythingMenu()
        {
            GuiLayout layout = new GuiLayout("Move everything", toggRef.menu.menu);
            {
                new GuiTextButton("Left", null, new GuiAction1Arg<IntVector2>(toggRef.board.MoveEverything, IntVector2.Left), false, layout);
                new GuiTextButton("Right", null, new GuiAction1Arg<IntVector2>(toggRef.board.MoveEverything, IntVector2.Right), false, layout);
                new GuiTextButton("Up", null, new GuiAction1Arg<IntVector2>(toggRef.board.MoveEverything, IntVector2.NegativeY), false, layout);
                new GuiTextButton("Down", null, new GuiAction1Arg<IntVector2>(toggRef.board.MoveEverything, IntVector2.PositiveY), false, layout);
            }
            layout.End();
        }

        void spawnPointsMenu()
        {
            GuiLayout layout = new GuiLayout("Spawn Points", toggRef.menu.menu, GuiLayoutMode.SingleColumn, null);
            {
                spawnPointContent(layout);
                new GuiTextButton("OK", null, closeMenu, false, layout);
            }
            layout.End();
        }

        void selectUnitType(HqUnitType utype)
        {
            selectedUnitType = (int)utype;
            tool = ToolType.Units;

            var data = hqRef.unitsdata.Get(utype);
            GuiLayout layout = new GuiLayout(data.Name, toggRef.menu.menu);
            {
                data.ToInfoMenu(layout);
                new GuiTextButton("OK", null, closeMenu, false, layout);
            }
            layout.End();
            //toggRef.menu.UnitInfoMenu(utype, null, selectUnitInsert);
        }

        void eventFlagMenu()
        {
            GuiLayout layout = new GuiLayout("Event flag", toggRef.menu.menu, GuiLayoutMode.SingleColumn, null);
            {
                eventFlagContent(layout);
                new GuiTextButton("OK", null, closeMenu, false, layout);
            }
            layout.End();
        }

        void eventFlagContent(GuiLayout layout)
        {
            new GuiIntSlider(SpriteName.NO_IMAGE, "Event ID",
               eventFlagIdProperty, new IntervalF(0, 100), false, layout);
        }

        int eventFlagId = 0;
        int eventFlagIdProperty(bool set, int value)
        {
            if (set)
            {
                eventFlagId = value;
            }
            return eventFlagId;
        }

        void doorMenu()
        {
            GuiLayout layout = new GuiLayout("Door", toggRef.menu.menu, GuiLayoutMode.SingleColumn, null);
            {
                doorsettings.toEditorSettings(layout);
                new GuiTextButton("OK", null, closeMenu, false, layout);
            }
            layout.End();
        }

        void furnishMenu()
        {
            GuiLayout layout = new GuiLayout("Furnish", toggRef.menu.menu, GuiLayoutMode.SingleColumn, null);
            {
                furnishContent(layout);
                new GuiTextButton("OK", null, closeMenu, false, layout);
            }
            layout.End();
        }

        void chestMenu()
        {
            GuiLayout layout = new GuiLayout("Loot Chest", toggRef.menu.menu, GuiLayoutMode.SingleColumn, null);
            {
                chestContent(layout);
                new GuiTextButton("OK", null, closeMenu, false, layout);
            }
            layout.End();
        }

        void spawnPointContent(GuiLayout layout)
        {
            new GuiIntSlider(SpriteName.NO_IMAGE, "Player", spawnPlayerProperty, new IntervalF(0, 1), false, layout);
            new GuiIntSlider(SpriteName.NO_IMAGE, "Type", spawnTypeProperty, new IntervalF(0, 9), false, layout);
        }

        void furnishContent(GuiLayout layout)
        {
            List<GuiOption<Map.MapFurnishType>> options = new List<GuiOption<Map.MapFurnishType>>
            {
                new GuiOption<Map.MapFurnishType>(Map.MapFurnishType.Box),
                new GuiOption<Map.MapFurnishType>(Map.MapFurnishType.Rat),
                new GuiOption<Map.MapFurnishType>(Map.MapFurnishType.Sparrow),
            };
            new GuiOptionsList<Map.MapFurnishType>(SpriteName.NO_IMAGE, "TYPE", options, furnishProperty, layout);
        }

        void chestContent(GuiLayout layout)
        {
            List<GuiOption<LootLevel>> options = new List<GuiOption<LootLevel>>((int)LootLevel.NUM_NONE);
            for (LootLevel lvl = 0; lvl < LootLevel.NUM_NONE; ++lvl)
            {
                options.Add(new GuiOption<LootLevel>(lvl.ToString(), lvl));
            }

            new GuiOptionsList<LootLevel>(SpriteName.NO_IMAGE, "Level", options, chestProperty, layout);
        }

        void listTags()
        {
            GuiLayout layout = new GuiLayout("Select Tag ID", toggRef.menu.menu, GuiLayoutMode.SingleColumn, null);
            {
                for (int i = 1; i <= 20; ++i)
                {
                    new GuiTextButton("Tag " + i.ToString(), null, new GuiAction1Arg<int>(selectTagId, i), false, layout);
                }
            }
            layout.End();
        }

        void unitsMenu()
        {
            GuiLayout layout = new GuiLayout("Units", toggRef.menu.menu, GuiLayoutMode.MultipleColumns, null);
            {

                //for (ArmyRace race = 0; race < ArmyRace.NUM_NON; ++race)
                //{
                //    new GuiLabel(race.ToString(), layout);
                //    var units = cmdRef.units.GetRaceSheet(race);
                //    foreach (var u in units)
                //    {
                //        AbsUnitData data = cmdRef.units.GetUnit(u);
                //        new GuiBigIcon(data.image, data.Name, new GuiAction1Arg<UnitType>(selectUnitType, u), true, layout);
                //    }
                //}
            }
            layout.End();
        }

        void hqunitsMenu()
        {
            GuiLayout layout = new GuiLayout("Units", toggRef.menu.menu, GuiLayoutMode.MultipleColumns, null);
            {
                foreach (var utype in HeroQuest.Data.AllUnitsData.EditorReadyUnits)
                {
                    var data = hqRef.unitsdata.Get(utype);
                    new GuiBigIcon(data.modelSettings.image, data.Name, new GuiAction1Arg<HqUnitType>(selectUnitType, utype), true, layout);
                }
            }layout.End();
        }
    }
}
