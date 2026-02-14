using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Communication;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Command;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.Engine;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Players.PlayerControls
{
    class GameControls
    {
        public MapControls map;
        public ArmyControls army = null;
        public SoldierControls soldier = null;
        public DiplomacyMap diplomacy = null;
        public Build.BuildControls build;
        public AbsCommandTarget commandTarget = null;
        LocalPlayer player;
        public InputMap input;
        bool cityUpdate;
        int tabCity = -1;
        SpottedArrayCounter<Army> tabArmy;
        int tabWarFaction = -1;
        public int[] GameSpeedOptions;
        public InputHelpState inputHelpState = InputHelpState.Map;
        public RichMenuControllerPointer controllerPointer = null;
        GameObjectType controllerPointer_objectFocus;
        Vector2 controllerPointer_storedPos_city;
        Vector2 controllerPointer_storedPos_army;
        Vector2 controllerPointer_storedPos_defaultObject;
        Vector2 controllerPointer_storedPos_faction;
        Vector2 controllerPointer_storedPos_diplomacy;
       
        public GameControls(LocalPlayer player, InputMap input)
        {
            
            this.player = player;
            this.input = input;
            player.gameControls = this;

            build = new Build.BuildControls(player);

            cityUpdate = DssRef.state.PlayType() == GameState.PlayStateType.Play;

            tabArmy = player.faction.armies.counter();            

            map = new Players.MapControls(player);
            if (player.faction.mainCity != null)
            {
                map.setCameraPos(player.faction.mainCity.tilePos);
            }

            if (DssRef.difficulty.setting_gameMode == GameModeMainType.Spectator)
            {
                GameSpeedOptions = new int[] { 1, 2, 5, 15 };
            }
            else if (DssRef.storage.speed5x)
            {
                GameSpeedOptions = new int[] { 1, 2, DssConst.MaxSpeedOption };
            }
            else
            {
                GameSpeedOptions = new int[] { 1, 2 };
            }
        }

        public void refreshInput()
        {
            if (input.inputSource.HasMouse)
            {
                input.copyDataFrom(Ref.gamesett.keyboardMap);
            }
        }

        public void update()
        {
            if (Input.Keyboard.Ctrl)
            {
                lib.DoNothing();
            }

            if (player.hud.popMenu != null)
            {
                if (player.hud.popMenu.update(player, out bool overPopHud))
                {
                    player.hud.popMenu.DeleteMe();
                    player.hud.popMenu = null;
                }
                
                if (overPopHud)
                {
                    return;
                }
            }


            bool hudState = false;
            bool uiRefresh = false; 
            //bool buildMode = false;

            if (input.inputSource.ControllerMode)
            {                
                if (controllerPointer != null)
                {
                    hudState = true;
                    var menu = controllerPointer.menu;
                    menu.updateControllerInput(controllerPointer);
                    player.hud.needRefresh |= menu.needRefresh;
                    
                }
                player.hud.update(out uiRefresh, true);
            }
            else
            {
                if (map.overridingDrag())
                {
                    player.hud.update(out uiRefresh, false);
                }
                else
                {
                    player.hud.update(out uiRefresh, true);
                    hudState = player.hud.mouseOverHud;
                }
            }

            if (hudState)
            {
                inputHelpState = InputHelpState.Menu;
                map.leftFocusUpdate();
                player.hud.updateToolTip_menu();
            }
            else if (diplomacy != null)
            {   
                map.mapControlsUpdate();
                player.hud.updateToolTip_menu();

                if (input.QuickSelect.DownEvent)
                {
                    //lib.DoNothing();
                    player.hud.objMenu.diplomacy.quickSelect();
                }

                player.hud.updateToolTip_diplomacy(uiRefresh);
                //diplomacy.update();
            }
            else
            {
                inputHelpState = InputHelpState.Map;
                map.focusedUpdate();

                if (commandTarget != null)
                {
                    inputHelpState = InputHelpState.CommandTarget;
                    if (commandTarget.update(player))
                    {
                        commandTarget.DeleteMe();
                        commandTarget = null;
                    }
                }
                else if ((map.hover.subTile.hasSelection && InBuildOrdersMode()) || build.buildKeyDown)
                {
                    inputHelpState = InputHelpState.Build;
                    map.cancelRectangleSelect();
                    build.updateBuildMode();                    
                }
                else
                {
                    if (input.mouseSelect.DownEvent)
                    {
                        mapSelect();
                    }

                    if (input.QuickSelect.DownEvent)
                    {
                        selectAreaCity();

                        if (input.inputSource.ControllerMode)
                        {
                            setMenuFocus(true, true);
                        }
                    }

                    if (input.mouseOrder.DownEvent)
                    {
                        mapExecute();
                    }
                }

                if (map.HasRectangleSelect())
                {
                    player.hud.updateToolTip_multiselect(true, map.RectangleSelect_ToolipAboveMouse());
                }
                else
                {
                    player.hud.updateToolTip_map(uiRefresh);
                }
            }

            map.passiveUpdate();

            if (army != null)
            {
                inputHelpState = InputHelpState.Army;
                army.update();
            }
            else
            {
                updateMapShortCuts();
            }

            if (input.cancelDownEvent())
            {
                if (InBuildOrdersMode(false))
                {
                    player.hud.needRefresh = true;
                    player.hud.tooltip.clear();
                    player.gameControls.map.hover.subTile.clear();
                    build.buildMode = SelectTileResult.None;
                    map.selection.subTile.selectTileResult = SelectTileResult.None;
                }
            }

            if (input.inputSource.ControllerMode)
            {
                if (input.ControllerFocus.DownEvent)
                {
                    //Toggle menu focus
                    bool toFocus = controllerPointer == null;
                    bool objectMenu = true;

                    if (toFocus && player.hud.factionMenu.IsOpen())
                    {
                        objectMenu = false;
                    }
                    else if (toFocus && map.selection.obj == null)
                    {
                        mapSelect();
                    }
                    setMenuFocus(toFocus, objectMenu);
                }

                if (input.ControllerFaction.DownEvent &&
                    controller_mayUseHeadDisplay())
                {
                    if (controllerPointer != null && controllerPointer.menu == player.hud.factionMenu.menu)
                    {
                        setMenuFocus(false, false);
                        
                    }
                    else
                    {
                        setMenuFocus(false, false);

                        player.hud.head.TabClick(player.hud.head.factionTabOptions()[0]);
                        player.hud.factionMenu.createMenu(player);
                        player.hud.objMenu.deleteMenu();
                        map.hover.obj = null;
                        setMenuFocus(true, false);
                    }
                }

                if (input.cancelDownEvent())
                {
                    setMenuFocus(false, true);
                    clearSelection();
                }

                if (input.Controller_TabLeft.DownEvent)
                {
                    controllerTabbing(-1, true);
                }
                if (input.Controller_TabRight.DownEvent)
                {
                    controllerTabbing(1, true);
                }
                //if (input.Controller_SubTabLeft.DownEvent)
                //{
                //    controllerTabbing(-1, false);
                //}
                //if (input.Controller_SubTabRight.DownEvent)
                //{
                //    controllerTabbing(1, false);
                //}

                if (input.ControllerMessageClick.DownEvent)
                {
                    player.hud.messages.onControllerClick();
                }

            }

            //if (input.inputSource.IsController)
            //{

            //    bool friendlyHoverObj = mapControls.hover.obj != null && mapControls.hover.obj.GetFaction() == faction;
            //    if (!menuFocusState &&
            //    !hud.menuFocus &&
            //        (input.Select.DownEvent || (friendlyHoverObj && input.ControllerFocus.DownEvent)))
            //    {
            //        if (armyControls != null &&
            //            (mapControls.hover.obj == null || mapControls.armyMayAttackHoverObj()))
            //        {
            //            mapExecute();
            //        }
            //        else
            //        {
            //            mapSelect();
            //        }
            //    }

            //    if (input.ControllerMessageClick.DownEvent)
            //    {
            //        hud.messages.onControllerClick();
            //    }

            //    if (inputConnected && !input.Connected)
            //    {
            //        DssRef.state.menuSystem.controllerLost();
            //    }
            //    inputConnected = input.Connected;
            //}
            //else
            //{
            //    if (!hud.mouseOverHud)
            //    {

            //    }
            //}



            gameSpeedInput();

            updateObjectTabbing();
        }

        public bool controller_mayUseHeadDisplay()
        {
            return diplomacy == null;
        }


        public ControllerTabFocus tabFocus()
        {            
            if (controllerPointer != null)
            {
                if (controllerPointer.menu == player.hud.objMenu.menu)
                {
                    switch (map.FocusObjectType())
                    {
                        case GameObjectType.City:
                            return ControllerTabFocus.CityMenu;
                        case GameObjectType.Army:
                            return ControllerTabFocus.ArmyMenu;
                        case GameObjectType.Faction:
                            return ControllerTabFocus.None;
                        default:
                            return ControllerTabFocus.GeneralObjectsMenu;
                    }
                }
                if (controllerPointer.menu == player.hud.factionMenu.menu)
                {
                    return ControllerTabFocus.Headmenu;
                }
            }

            if (InBuildOrdersMode())
            {
                return ControllerTabFocus.Build;
            }

            return ControllerTabFocus.Pause_GamePlay;
        }

        void controllerTabbing(int dir, bool mainTabbing)
        {
            if (mainTabbing)
            {
                switch (tabFocus())
                {
                    case ControllerTabFocus.CityMenu:
                        {
                            var tabs = player.AvailableCityTabs();
                            var index = arraylib.IndexFromValue(tabs, player.cityTab);
                            index = Bound.SetRollover(index + dir, 0, tabs.Count - 1);
                            player.cityTab = tabs[index];
                        }
                        break;
                    case ControllerTabFocus.ArmyMenu:
                        {
                            var tabs = player.AvailableArmyTabs();
                            var index = arraylib.IndexFromValue(tabs, player.armyTab);
                            index = Bound.SetRollover(index + dir, 0, tabs.Count - 1);
                            player.armyTab = tabs[index];
                        }
                        break;
                    case ControllerTabFocus.Headmenu:
                        {
                            var tabs = player.hud.head.factionTabOptions();
                            var index = arraylib.IndexFromValue(tabs, player.factionTab);
                            index = Bound.SetRollover(index + dir, 0, tabs.Length - 1);
                            player.hud.head.TabClick(tabs[index]);
                        }
                        break;
                    case ControllerTabFocus.Build:
                        var city = map.selection.obj?.GetCity();
                        if (city != null)
                        {
                            var tabs = build.availableBuildOptions(city);
                            var index = arraylib.IndexFromValue(tabs, build.placeBuildingType);
                            index = Bound.SetRollover(index + dir, 0, tabs.Count - 1);
                            build.buildingTypeClick(tabs[index]);
                        }
                        break;
                    case ControllerTabFocus.Pause_GamePlay:
                        if (dir < 0)
                        {
                            if (DssRef.difficulty.setting_allowPauseCommand)
                            {
                                Ref.TogglePause();
                            }
                        }
                        else
                        {
                            setNextGameSpeed();
                        }
                        break;
                }
            }
            else
            {
                switch (tabFocus())
                {
                    case ControllerTabFocus.Pause_GamePlay:
                        if (dir < 0)
                        {
                            selectAreaCity();
                            setMenuFocus(true, true);
                        }
                        else
                        {
                            map.toggleCameraTiltUp();
                        }
                        break;
                }
            }
            player.hud.needRefresh = true;
        }

        

        public bool tabFocusColor(ControllerTabFocus inFocus, out Color color)
        {
            if (input.inputSource.ControllerMode)
            {
                color = tabFocus() == inFocus ? Color.White : Color.Black;
                return true;
            }

            color = Color.White;
            return false;
        }

        public void setMenuFocus(bool set, bool objectMenu)
        {
            if (input.inputSource.ControllerMode) 
            {
                if (set)
                {
                    if (controllerPointer == null)
                    {
                        controllerPointer = new RichMenuControllerPointer(input);
                        if (objectMenu)
                        {
                            Vector2 storedPos;
                            controllerPointer_objectFocus = map.FocusObjectType();
                            switch (controllerPointer_objectFocus)
                            {
                                case GameObjectType.City:
                                    storedPos = controllerPointer_storedPos_city;
                                    break;
                                case GameObjectType.Army:
                                    storedPos = controllerPointer_storedPos_army;
                                    break;
                                case GameObjectType.Faction:
                                    storedPos = controllerPointer_storedPos_diplomacy;
                                    break;
                                default:
                                    storedPos = controllerPointer_storedPos_defaultObject;
                                    break;
                            }
                            player.hud.objMenu.createMenu(player);
                            controllerPointer.setMenu(player.hud.objMenu.menu, storedPos);
                        }
                        else
                        {
                            controllerPointer.setMenu(player.hud.factionMenu.menu, controllerPointer_storedPos_faction);
                        }
                        player.hud.needRefresh = true;
                    }
                }
                else
                {
                    if (controllerPointer != null)
                    {
                        if (controllerPointer.menu == player.hud.objMenu.menu)
                        {
                            controllerPointer.DeleteMe(out Vector2 storedPos);
                            switch (controllerPointer_objectFocus)
                            {
                                case GameObjectType.City:
                                    controllerPointer_storedPos_city = storedPos;
                                    break;
                                case GameObjectType.Army:
                                    controllerPointer_storedPos_army = storedPos;
                                    break;
                                default:
                                    controllerPointer_storedPos_defaultObject = storedPos;
                                    break;
                            }
                        }
                        else
                        {
                            controllerPointer.DeleteMe(out controllerPointer_storedPos_faction);
                        }
                        controllerPointer = null;
                        player.hud.needRefresh = true;
                    }
                }


                map.controllerPointer.Visible = !set;
            }

        }

        void updateMapShortCuts()
        {
            if (player.mapLayersManager.current.DrawDetailLayer)
            {
                if (input.Build.DownEvent && map.hover.subTile.city.factionIndex == player.faction.myIndex)
                {
                    if (player.profile.casualControls)
                    {
                        selectAreaCity();
                        player.cityTab = MenuTab.Casual_Build;
                    }
                    else
                    {
                        var order = player.orders.orderOnSubTile(map.hover.subTile.subTilePos) as BuildOrder;
                        if (order != null)
                        {
                            setBuildMode(map.hover.subTile.city, order.buildingType);
                            return;
                        }

                        var build = BuildLib.BuildTypeFromTerrain(map.hover.subTile.subTile.mainTerrain, map.hover.subTile.subTile.subTerrain);
                        setBuildMode(map.hover.subTile.city, build);
                        return;
                    }
                }

                bool inHotkeyRepeceptiveMenu = map.selection.obj != null &&
                    map.selection.obj.gameobjectType() == GameObjectType.City &&
                    (player.cityTab == MenuTab.Delivery || player.cityTab == MenuTab.Conscript);

                if (!inHotkeyRepeceptiveMenu)
                {
                    switch (map.hover.subTile.subTile.mainTerrain)
                    {
                        case TerrainMainType.Building:

                            switch ((TerrainBuildingType)map.hover.subTile.subTile.subTerrain)
                            {
                                case TerrainBuildingType.Recruitment:
                                case TerrainBuildingType.Postal:
                                    if (input.Copy.DownEvent)
                                    {
                                        int ix = map.hover.subTile.city.deliveryIxFromSubTile(map.hover.subTile.subTilePos);
                                        map.hover.subTile.city.copyDelivery(player, ix);
                                        SoundLib.copy.Play();
                                    }
                                    if (input.Paste.DownEvent)
                                    {
                                        int ix = map.hover.subTile.city.deliveryIxFromSubTile(map.hover.subTile.subTilePos);
                                        map.hover.subTile.city.pasteDelivery(player, ix);
                                        SoundLib.paste.Play();
                                    }
                                    if (input.StopStart.DownEvent)
                                    {
                                        int ix = map.hover.subTile.city.deliveryIxFromSubTile(map.hover.subTile.subTilePos);
                                        bool start = map.hover.subTile.city.toggleDeliveryStop(ix);
                                        (start ? SoundLib.start : SoundLib.stop).Play();
                                    }
                                    break;

                                case TerrainBuildingType.Nobelhouse:
                                case TerrainBuildingType.SoldierBarracks:
                                    if (input.Copy.DownEvent)
                                    {
                                        int ix = map.hover.subTile.city.conscriptIxFromSubTile(map.hover.subTile.subTilePos);
                                        map.hover.subTile.city.copyConscript(player, ix);
                                        SoundLib.copy.Play();
                                    }
                                    if (input.Paste.DownEvent)
                                    {
                                        int ix = map.hover.subTile.city.conscriptIxFromSubTile(map.hover.subTile.subTilePos);
                                        map.hover.subTile.city.pasteConscript(player, ix);
                                        SoundLib.paste.Play();
                                    }
                                    if (input.StopStart.DownEvent)
                                    {
                                        int ix = map.hover.subTile.city.conscriptIxFromSubTile(map.hover.subTile.subTilePos);
                                        bool start = map.hover.subTile.city.toggleConscriptStop(ix);
                                        (start ? SoundLib.start : SoundLib.stop).Play();
                                        player.hud.needRefresh = true;
                                    }
                                    break;
                            }
                            break;
                    }
                }

#if DEBUG
                if (VikingEngine.Input.Keyboard.KeyDownEvent(Keys.P))
                {
                    var subtile = DssRef.world.subTileGrid.Get(map.hover.subTile.subTilePos);
                    subtile.SetType(TerrainMainType.Wall, (int)TerrainWallType.StoneWall, 1);
                    DssRef.world.subTileGrid.Set(map.hover.subTile.subTilePos, subtile);
                }
#endif

            }

            if (map.selection.obj != null &&
                map.selection.obj.gameobjectType() == GameObjectType.City)
            {
                var city = map.selection.obj.GetCity();
                switch (player.cityTab)
                {
                    case MenuTab.Delivery:
                        if (input.StopStart.DownEvent)
                        {
                            bool start = city.toggleDeliveryStop(city.selectedDelivery);
                            player.hud.needRefresh = true;
                            (start ? SoundLib.start : SoundLib.stop).Play();
                        }
                        if (input.Copy.DownEvent)
                        {
                            city.copyDelivery(player);
                            SoundLib.copy.Play();
                        }
                        if (input.Paste.DownEvent)
                        {
                            city.pasteDelivery(player);
                            SoundLib.paste.Play();
                            player.hud.needRefresh = true;
                        }
                        break;
                    case MenuTab.Conscript:
                        if (input.StopStart.DownEvent)
                        {
                            bool start = city.toggleConscriptStop(city.selectedConscript);
                            player.hud.needRefresh = true;
                            (start ? SoundLib.start : SoundLib.stop).Play();
                        }
                        if (input.Copy.DownEvent)
                        {
                            city.copyConscript(player);
                            SoundLib.copy.Play();
                        }
                        if (input.Paste.DownEvent)
                        {
                            city.pasteConscript(player);
                            SoundLib.paste.Play();
                            player.hud.needRefresh = true;
                        }
                        break;
                    case MenuTab.Progress:
                        switch (player.progressSubTab)
                        {
                            case ProgressSubTab.Schools:
                                if (input.StopStart.DownEvent)
                                {
                                    bool start = city.toggleSchoolStop(city.selectedSchool);
                                    player.hud.needRefresh = true;
                                    (start ? SoundLib.start : SoundLib.stop).Play();
                                }
                                if (input.Copy.DownEvent)
                                {
                                    city.copySchool(player);
                                    SoundLib.copy.Play();
                                }
                                if (input.Paste.DownEvent)
                                {
                                    city.pasteSchool(player);
                                    SoundLib.paste.Play();
                                    player.hud.needRefresh = true;
                                }
                                break;
                            case ProgressSubTab.Research:
                                if (input.StopStart.DownEvent)
                                {
                                    if (city.commitResearch(player))
                                    {
                                        player.hud.needRefresh = true;
                                        SoundLib.start.Play();
                                    }
                                }
                                break;
                        }
                        break;

                }
            }
        }
        void mapSelect()
        {

            bool sameMapObject = map.selection.obj != null;
            if (map.hover.subTile.hasSelection)
            {
                sameMapObject &= map.selection.obj == map.hover.subTile.city;
            }
            else
            {
                sameMapObject &= map.hover.obj == map.selection.obj;
            }

            bool oldselection = clearSelection();

            bool newselection = clickHover(sameMapObject);

            if (newselection && input.inputSource.ControllerMode)
            {
                if (input.ControllerFocus.DownEvent || map.focusedObjectMenuState())
                {
                    setMenuFocus(true, true);
                }
            }


            if (oldselection && !newselection)
            {
                SoundLib.back.Play();
            }

        }

        bool clickHover(bool sameMapObject)
        {
            if (map.hover.subTile.hasSelection)//.selectable(faction, out var city))
            {
                SoundLib.click.Play();

                map.onTileSelect(map.hover.subTile, sameMapObject);

                return true;
            }

            if (map.hover.obj != null &&
                (map.hover.obj.GetFaction() == player.faction || DssRef.difficulty.setting_gameMode == GameModeMainType.Spectator))
            {
                SoundLib.click.Play();
                map.onSelect();

                switch (map.selection.obj.gameobjectType())
                {
                    case GameObjectType.Army:
                        SoundLib.select_army.Play();
                        {
                            army = new ArmyControls(player, new ArmyCollection(map.selection.obj.GetArmy()));
                        }
                        break;
                    case GameObjectType.City:
                        SoundLib.select_city.Play();
                        break;

                    case GameObjectType.Soldier:
                        SoundLib.select_army.Play();
                        {
                            soldier = new SoldierControls(new List<SoldierGroup> { map.selection.obj.GetSoldierGroup() });
                        }
                        break;
                    case GameObjectType.Worker:
                        SoundLib.select_city.Play();
                        break;
                        //case GameObjectType.Faction:
                        //    SoundLib.select_faction.Play();
                        //    break;
                }

                return true;
            }



            return false;
        }

        public void mapSelect(AbsWorldObject mapObject)
        {
            bool sameMapObject = map.selection.obj != null && mapObject == map.selection.obj;
            clearSelection();

            map.hover.obj = mapObject;
            clickHover(sameMapObject);

        }

        public void selectObject(GameObject.AbsGameObject obj)
        {
            map.cameraFocus = obj;
            mapSelect(obj.GetWorldObject());
            
            if (input.inputSource.ControllerMode && obj.gameobjectType() != GameObjectType.City)
            {
                setMenuFocus(false, false);
            }
        }

        public void selectAreaCity()
        {
            if (map.selection.obj == null &&
                DssRef.world.tileGrid.TryGet(map.tilePosition, out var tile))
            {
                var city = tile.City();
                if (city.factionIndex == player.faction.myIndex)
                {
                    mapSelect(city);
                }
            }
        }

        void updateObjectTabbing()
        {
            //CITY
            if (input.NextCity.DownEvent && player.faction.cities.Count > 0)
            {
                nextCity();
                if (input.inputSource.ControllerMode)
                {
                    setMenuFocus(true, true);
                }
            }

            //ARMY
            if (input.NextArmy.DownEvent)
            {
                nextArmy(!Input.Keyboard.Shift);
         
            }

            if (input.NextWar.DownEvent)
            {
                nextWar(!Input.Keyboard.Shift);
            }
        }

        public void nextCity(City city)
        {
            map.cameraFocus = city;
            mapSelect(city);
            player.hud.needRefresh = true;
        }

        public void nextCity()
        {
            player.hud.needRefresh = true;

            int dir = 1;
            if (Input.Keyboard.Shift &&
                player.gameControls.input.inputSource.HasKeyBoard)
            {
                dir = -1;
            }

            int loops = 0;
            do
            {
                tabCity = Bound.SetRollover(tabCity + dir, 0, player.faction.cities.Array.Length-1);

                var cIx = player.faction.cities.Array[tabCity];
                if (cIx >= 0)
                {
                    var city = DssRef.world.cities[cIx];
                    if (city.factionIndex == player.faction.myIndex)
                    {
                        if (city.automateCity &&
                            player.gameControls.input.inputSource.HasKeyBoard &&
                            Input.Keyboard.Alt)
                        {
                            continue;
                        }
                        else
                        {
                            //focus on city
                            map.cameraFocus = city;
                            mapSelect(city);
                            return;
                        }
                    }
                }

            } while (++loops < player.faction.cities.Array.Length);

            //if (forward)
            //{
            //    tabCity++;
            //    if (tabCity >= player.faction.cities.Count)
            //    {
            //        tabCity = 0;
            //    }
            //}
            //else
            //{
            //    tabCity--;
            //    if (tabCity < 0)
            //    {
            //        tabCity = player.faction.cities.Count - 1;
            //    }
            //}


            //int current = 0;
            
            //SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            //while (citiesC.Next(ref player.faction.cities, DssRef.world.cities, out City citySel))
            //{
            //    if (current == tabCity)
            //    {
            //        //focus on city
            //        map.cameraFocus = citySel;
            //        mapSelect(citySel);

            //        return;
            //    }
            //    current++;
            //}
            
        }
        public void nextArmy(bool forward)
        {
            player.hud.needRefresh = true;
            if (forward)
            {
                if (tabArmy.Next_Rollover())
                {
                    map.cameraFocus = tabArmy.sel;
                    mapSelect(tabArmy.sel);

                    return;
                }
            }
            else
            {
                if (tabArmy.Prev_Rollover())
                {
                    map.cameraFocus = tabArmy.sel;
                    mapSelect(tabArmy.sel);

                    return;
                }
            }
            
        }

        public void nextWar(bool forward)
        {
            tabWarFaction++;
            for (int i = tabWarFaction; i < player.faction.diplomaticRelations.Length; i++)
            {
                if (checkRelation(i))
                {
                    tabWarFaction = i;
                    return;
                }
                //var rel = player.faction.diplomaticRelations[i];
                //if (rel != null && rel.Relation <= RelationType.RelationTypeN3_War)
                //{
                //    var enemy = DssRef.world.factions.GetIndex_Safe(i);
                //    if (enemy != null)
                //    {
                //        if (enemy.mainCity != null)
                //        {
                //            map.cameraFocus = enemy.mainCity;

                //            return;
                //        }

                //        var army = enemy.armies.First();
                //        if (army != null)
                //        {
                //            map.cameraFocus = army;

                //            return;
                //        }
                //    }                    
                //}
            }

            for (int i = 0; i < tabWarFaction; i++)
            {
                if (checkRelation(i))
                {
                    tabWarFaction = i;
                    return;
                }
            }

            bool checkRelation(int i)
            {
                var rel = player.faction.diplomaticRelations[i];
                if (rel != null && rel.Relation <= RelationType.RelationTypeN3_War)
                {
                    var enemy = DssRef.world.faction(i);
                    if (enemy != null)
                    {
                        if (enemy.mainCity != null)
                        {
                            map.cameraFocus = enemy.mainCity;

                            return true;
                        }

                        var army = enemy.armies.First();
                        if (army != null)
                        {
                            map.cameraFocus = army;

                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public bool clearSelection()
        {
            bool bClear = false;

            if (army != null)
            {
                army.clearState();
                army = null;
            }

            if (soldier != null)
            {
                soldier = null;
            }

            bClear = map.clearSelection();
            player.hud.clearState();

            return bClear;
        }

        void mapExecute()
        {
            if (army != null)
            {
                army.mapExecute();
                

                if (input.inputSource.ControllerMode)
                {
                    clearSelection();
                }
            }

            if (soldier != null)
            {
                soldier.mapExecute(player);
            }
        }
        void gameSpeedInput()
        {
            if (DssRef.state.IsSinglePlayer_Local())
            {
                if (DssRef.difficulty.setting_allowPauseCommand &&
                    input.PauseGame.DownEvent)
                {
                    Ref.TogglePause();
                }

                if (input.GameSpeed.DownEvent)
                {
                    setNextGameSpeed();
                }
            }
        }

        void setNextGameSpeed()
        {
            if (DssRef.state.IsSinglePlayer_Local())
            {
                if (Ref.isPaused)
                {
                    //Ref.isPaused = false;
                    //Ref.GameTimeSpeed = 1f;
                    Ref.SetPause(false);
                }
                else
                {
                    for (int i = 0; i < GameSpeedOptions.Length; i++)
                    {
                        if (GameSpeedOptions[i] == Ref.GameTimeSpeed)
                        {
                            int next = Bound.SetRollover(i + 1, 0, GameSpeedOptions.Length - 1);
                            Ref.SetGameSpeed(GameSpeedOptions[next]);
                            player.hud.needRefresh = true;
                            break;
                        }
                    }
                }
            }
        }

        void setBuildMode(City city, BuildAndExpandType type)
        {
            mapSelect(city);
            player.cityTab = MenuTab.Build;
            
                build.buildMode = SelectTileResult.Build;
            if (type != BuildAndExpandType.NUM_NONE)
            {
                build.placeBuildingType = type;
            }
        }
        public bool InBuildOrdersMode(bool includeZoomLevel = true)
        {
            return player.cityTab == Interface.MenuTab.Build &&
                map.selection.obj != null &&
                map.selection.obj.gameobjectType() == GameObjectType.City &&
                build.buildMode != SelectTileResult.None &&
                (!includeZoomLevel || player.mapLayersManager.current.DrawDetailLayer);
        }
    }

    enum ControllerTabFocus
    { 
        None,
        Pause_GamePlay,
        CityMenu,
        ArmyMenu,
        GeneralObjectsMenu,
        Headmenu,
        Build,
    }
}
