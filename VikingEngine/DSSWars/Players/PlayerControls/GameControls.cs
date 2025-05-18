using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Players.PlayerControls
{
    class GameControls
    {
        public MapControls mapControls;
        public ArmyControls armyControls = null;
        public SoldierControls soldierControls = null;
        public Build.BuildControls buildControls;
        LocalPlayer player;
        public InputMap input;
        bool cityUpdate;
        int tabCity = -1;
        SpottedArrayCounter<Army> tabArmy;
        public int[] GameSpeedOptions;
        public InputHelpState inputHelpState = InputHelpState.Map;
        public RichMenuControllerPointer controllerPointer = null;
        GameObjectType controllerPointer_objectFocus;
        Vector2 controllerPointer_storedPos_city;
        Vector2 controllerPointer_storedPos_army;
        Vector2 controllerPointer_storedPos_defaultObject;
        Vector2 controllerPointer_storedPos_faction;


        public GameControls(LocalPlayer player, InputMap input)
        { 
            this.player = player;
            this.input = input;
            player.gameControls = this;

            buildControls = new Build.BuildControls(player);

            cityUpdate = DssRef.state.PlayType() == GameState.PlayStateType.Play;

            tabArmy = player.faction.armies.counter();            

            mapControls = new Players.MapControls(player);
            if (player.faction.mainCity != null)
            {
                mapControls.setCameraPos(player.faction.mainCity.tilePos);
            }

            if (DssRef.storage.speed5x)
            {
                GameSpeedOptions = new int[] { 1, 2, DssConst.MaxSpeedOption };
            }
            else
            {
                GameSpeedOptions = new int[] { 1, 2 };
            }
        }

        public void update()
        {
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

            if (input.inputSource.IsController)
            {                
                if (controllerPointer != null)
                {
                    hudState = true;
                    var menu = controllerPointer.menu;
                    menu.updateControllerInput(controllerPointer);
                    player.hud.needRefresh |= menu.needRefresh;
                    
                }
                player.hud.update(out uiRefresh);
            }
            else
            {
                if (!mapControls.overridingDrag())
                {
                    player.hud.update(out uiRefresh);
                    hudState = player.hud.mouseOverHud;
                }
            }

            if (hudState)
            {
                inputHelpState = InputHelpState.Menu;
                mapControls.leftFocusUpdate();
                player.hud.updateToolTip_menu(uiRefresh);
            }
            else
            {
                inputHelpState = InputHelpState.Map;
                mapControls.focusedUpdate();

                if ((mapControls.hover.subTile.hasSelection && InBuildOrdersMode()) || buildControls.buildKeyDown)
                {
                    inputHelpState = InputHelpState.Build;
                    mapControls.cancelRectangleSelect();
                    buildControls.updateBuildMode();
                    if (input.CancelKey.DownEvent)
                    {
                        
                        player.hud.needRefresh = true;
                        buildControls.buildMode = SelectTileResult.None;
                        mapControls.selection.subTile.selectTileResult = SelectTileResult.None;
                    }
                }
                else
                {
                    if (input.mouseSelect.DownEvent)
                    {
                        mapSelect();
                    }

                    if (input.mouseOrder.DownEvent)
                    {
                        mapExecute();
                    }
                }

                if (mapControls.HasRectangleSelect())
                {
                    player.hud.updateToolTip_multiselect(true, mapControls.RectangleSelect_ToolipAboveMouse());
                }
                else
                {
                    player.hud.updateToolTip_map(uiRefresh);
                }
            }

            mapControls.passiveUpdate();

            if (armyControls != null)
            {
                inputHelpState = InputHelpState.Army;
                armyControls.update();
            }
            else
            {
                updateMapShortCuts();
            }

            if (input.inputSource.IsController)
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
                    else if (toFocus && mapControls.selection.obj == null)
                    {
                        mapSelect();
                    }
                    setMenuFocus(toFocus, objectMenu);
                }

                if (input.ControllerFaction.DownEvent)
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
                        mapControls.hover.obj = null;
                        setMenuFocus(true, false);
                    }
                }

                if (input.CancelKey.DownEvent)
                {
                    setMenuFocus(false, true);
                    clearSelection();
                }

                if (input.Controller_TabLeft.DownEvent)
                {
                    controllerTabbing(-1);
                }
                if (input.Controller_TabRight.DownEvent)
                {
                    controllerTabbing(1);
                }

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



        public ControllerTabFocus tabFocus()
        {            
            if (controllerPointer != null)
            {
                if (controllerPointer.menu == player.hud.objMenu.menu)
                {
                    switch (mapControls.FocusObjectType())
                    {
                        case GameObjectType.City:
                            return ControllerTabFocus.CityMenu;
                        case GameObjectType.Army:
                            return ControllerTabFocus.ArmyMenu;
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

            return ControllerTabFocus.Pause;
        }

        void controllerTabbing(int dir)
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
                    var city = mapControls.selection.obj?.GetCity();
                    if (city != null)
                    {
                        var tabs = buildControls.availableBuildOptions(city);
                        var index = arraylib.IndexFromValue(tabs, buildControls.placeBuildingType);
                        index = Bound.SetRollover(index + dir, 0, tabs.Count - 1);
                        buildControls.buildingTypeClick(tabs[index]);
                    }
                    break;
                case ControllerTabFocus.Pause:
                    if (dir < 0)
                    {
                        player.hud.headOptions.pauseAction();
                    }
                    else
                    {
                        setNextGameSpeed();
                    }
                    break;
            }
            player.hud.needRefresh = true;
        }

        public bool tabFocusColor(ControllerTabFocus inFocus, out Color color)
        {
            if (input.inputSource.IsController)
            {
                color = tabFocus() == inFocus ? Color.White : Color.Black;
                return true;
            }

            color = Color.White;
            return false;
        }

        public void setMenuFocus(bool set, bool objectMenu)
        {
            if (input.inputSource.IsController) 
            {
                if (set)
                {
                    if (controllerPointer == null)
                    {
                        controllerPointer = new RichMenuControllerPointer(input);
                        if (objectMenu)
                        {
                            Vector2 storedPos;
                            controllerPointer_objectFocus = mapControls.FocusObjectType();
                            switch (controllerPointer_objectFocus)
                            {
                                case GameObjectType.City:
                                    storedPos = controllerPointer_storedPos_city;
                                    break;
                                case GameObjectType.Army:
                                    storedPos = controllerPointer_storedPos_army;
                                    break;
                                default:
                                    storedPos = controllerPointer_storedPos_defaultObject;
                                    break;
                            }
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


                mapControls.controllerPointer.Visible = !set;
            }

        }

        void updateMapShortCuts()
        {
            if (player.drawUnitsView.current.DrawDetailLayer)
            {
                if (input.Build.DownEvent && mapControls.hover.subTile.city.faction == player.faction)
                {
                    var order = player.orders.orderOnSubTile(mapControls.hover.subTile.subTilePos) as BuildOrder;
                    if (order != null)
                    {
                        setBuildMode(mapControls.hover.subTile.city, order.buildingType);
                        return;
                    }

                    var build = BuildLib.BuildTypeFromTerrain(mapControls.hover.subTile.subTile.mainTerrain, mapControls.hover.subTile.subTile.subTerrain);
                    setBuildMode(mapControls.hover.subTile.city, build);
                    return;
                }

                bool inHotkeyRepeceptiveMenu = mapControls.selection.obj != null &&
                    mapControls.selection.obj.gameobjectType() == GameObjectType.City &&
                    (player.cityTab == MenuTab.Delivery || player.cityTab == MenuTab.Conscript);

                if (!inHotkeyRepeceptiveMenu)
                {
                    switch (mapControls.hover.subTile.subTile.mainTerrain)
                    {
                        case TerrainMainType.Building:

                            switch ((TerrainBuildingType)mapControls.hover.subTile.subTile.subTerrain)
                            {
                                case TerrainBuildingType.Recruitment:
                                case TerrainBuildingType.Postal:
                                    if (input.Copy.DownEvent)
                                    {
                                        int ix = mapControls.hover.subTile.city.deliveryIxFromSubTile(mapControls.hover.subTile.subTilePos);
                                        mapControls.hover.subTile.city.copyDelivery(player, ix);
                                        SoundLib.copy.Play();
                                    }
                                    if (input.Paste.DownEvent)
                                    {
                                        int ix = mapControls.hover.subTile.city.deliveryIxFromSubTile(mapControls.hover.subTile.subTilePos);
                                        mapControls.hover.subTile.city.pasteDelivery(player, ix);
                                        SoundLib.paste.Play();
                                    }
                                    if (input.StopStart.DownEvent)
                                    {
                                        int ix = mapControls.hover.subTile.city.deliveryIxFromSubTile(mapControls.hover.subTile.subTilePos);
                                        bool start = mapControls.hover.subTile.city.toggleDeliveryStop(ix);
                                        (start ? SoundLib.start : SoundLib.stop).Play();
                                    }
                                    break;

                                case TerrainBuildingType.Nobelhouse:
                                case TerrainBuildingType.SoldierBarracks:
                                    if (input.Copy.DownEvent)
                                    {
                                        int ix = mapControls.hover.subTile.city.conscriptIxFromSubTile(mapControls.hover.subTile.subTilePos);
                                        mapControls.hover.subTile.city.copyConscript(player, ix);
                                        SoundLib.copy.Play();
                                    }
                                    if (input.Paste.DownEvent)
                                    {
                                        int ix = mapControls.hover.subTile.city.conscriptIxFromSubTile(mapControls.hover.subTile.subTilePos);
                                        mapControls.hover.subTile.city.pasteConscript(player, ix);
                                        SoundLib.paste.Play();
                                    }
                                    if (input.StopStart.DownEvent)
                                    {
                                        int ix = mapControls.hover.subTile.city.conscriptIxFromSubTile(mapControls.hover.subTile.subTilePos);
                                        bool start = mapControls.hover.subTile.city.toggleConscriptStop(ix);
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
                    var subtile = DssRef.world.subTileGrid.Get(mapControls.hover.subTile.subTilePos);
                    subtile.SetType(TerrainMainType.Wall, (int)TerrainWallType.StoneWall, 1);
                    DssRef.world.subTileGrid.Set(mapControls.hover.subTile.subTilePos, subtile);
                }
#endif

            }

            if (mapControls.selection.obj != null &&
                mapControls.selection.obj.gameobjectType() == GameObjectType.City)
            {
                var city = mapControls.selection.obj.GetCity();
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
                }
            }
        }
        void mapSelect()
        {

            //if (mapControls.hover.subTile.hasSelection && InBuildOrdersMode())
            //{
            //    buildControls.onTileSelect(mapControls.hover.subTile);
            //}
            //else if (downEvent)
            {
                bool sameMapObject = mapControls.selection.obj != null;
                if (mapControls.hover.subTile.hasSelection)
                {
                    sameMapObject &= mapControls.selection.obj == mapControls.hover.subTile.city;
                }
                else
                {
                    sameMapObject &= mapControls.hover.obj == mapControls.selection.obj;
                }

                bool oldselection = clearSelection();

                bool newselection = clickHover(sameMapObject);

                if (newselection && input.inputSource.IsController)
                {
                    if (input.ControllerFocus.DownEvent || mapControls.focusedObjectMenuState())
                    {
                        //mapControls.setObjectMenuFocus(true);
                        setMenuFocus(true, true);
                    }
                }


                if (oldselection && !newselection)
                {
                    SoundLib.back.Play();
                }
            }
        }

        bool clickHover(bool sameMapObject)
        {
            if (mapControls.hover.subTile.hasSelection)//.selectable(faction, out var city))
            {

                SoundLib.click.Play();

                mapControls.onTileSelect(mapControls.hover.subTile, sameMapObject);

                return true;
            }

            if (mapControls.hover.obj != null &&
                mapControls.hover.obj.GetFaction() == player.faction)
            {
                SoundLib.click.Play();
                mapControls.onSelect();

                switch (mapControls.selection.obj.gameobjectType())
                {
                    case GameObjectType.Army:
                        SoundLib.select_army.Play();
                        {
                            armyControls = new ArmyControls(player, new ArmyCollection(mapControls.selection.obj.GetArmy()));
                        }
                        break;
                    case GameObjectType.City:
                        SoundLib.select_city.Play();
                        break;

                    case GameObjectType.Soldier:
                        SoundLib.select_army.Play();
                        {
                            soldierControls = new SoldierControls(new List<SoldierGroup> { mapControls.selection.obj.GetSoldierGroup() });
                        }
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
            bool sameMapObject = mapControls.selection.obj != null && mapObject == mapControls.selection.obj;
            clearSelection();

            mapControls.hover.obj = mapObject;
            clickHover(sameMapObject);

        }

        public void selectObject(GameObject.AbsGameObject obj)
        {
            mapControls.cameraFocus = obj;
            mapSelect(obj.GetWorldObject());
        }


        void updateObjectTabbing()
        {
            //CITY
            if (input.NextCity.DownEvent && player.faction.cities.Count > 0)
            {
                nextCity(!Input.Keyboard.Shift);
            }

            //ARMY
            if (input.NextArmy.DownEvent)
            {
                nextArmy(!Input.Keyboard.Shift);
         
            }

        }

        public void nextCity(bool forward)
        {
            if (forward)
            {
                tabCity++;
                if (tabCity >= player.faction.cities.Count)
                {
                    tabCity = 0;
                }
            }
            else
            {
                tabCity--;
                if (tabCity < 0)
                {
                    tabCity = player.faction.cities.Count - 1;
                }
            }


            int current = 0;
            var citiesC = player.faction.cities.counter();
            while (citiesC.Next())
            {
                if (current == tabCity)
                {
                    //focus on city
                    mapControls.cameraFocus = citiesC.sel;
                    mapSelect(citiesC.sel);

                    return;
                }
                current++;
            }
        }
        public void nextArmy(bool forward)
        {
            if (forward)
            {
                if (tabArmy.Next_Rollover())
                {
                    mapControls.cameraFocus = tabArmy.sel;
                    mapSelect(tabArmy.sel);

                    return;
                }
            }
            else
            {
                if (tabArmy.Prev_Rollover())
                {
                    mapControls.cameraFocus = tabArmy.sel;
                    mapSelect(tabArmy.sel);

                    return;
                }
            }
        }
        public bool clearSelection()
        {
            bool bClear = false;

            if (armyControls != null)
            {
                armyControls.clearState();
                armyControls = null;
            }

            if (soldierControls != null)
            {
                soldierControls = null;
            }

            bClear = mapControls.clearSelection();
            player.hud.clearState();

            return bClear;
        }

        void mapExecute()
        {
            if (armyControls != null)
            {
                armyControls.mapExecute();
                armyControls.moveOrderEffect();

                if (input.inputSource.IsController)
                {
                    clearSelection();
                }
            }

            if (soldierControls != null)
            {
                soldierControls.mapExecute(player);
            }
        }
        void gameSpeedInput()
        {
            if (DssRef.state.IsSinglePlayer())
            {
                if (DssRef.difficulty.setting_allowPauseCommand &&
                    input.PauseGame.DownEvent)//IsLocalHost())
                {
                    player.hud.headOptions.pauseAction();
                }

                if (input.GameSpeed.DownEvent)
                {
                    setNextGameSpeed();
                }
            }
        }

        void setNextGameSpeed()
        {
            if (DssRef.state.IsSinglePlayer())
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
            if (type != BuildAndExpandType.NUM_NONE)
            {
                buildControls.buildMode = SelectTileResult.Build;
                buildControls.placeBuildingType = type;
            }
        }
        public bool InBuildOrdersMode()
        {
            return player.cityTab == Display.MenuTab.Build &&
                mapControls.selection.obj != null &&
                mapControls.selection.obj.gameobjectType() == GameObjectType.City &&
                buildControls.buildMode != SelectTileResult.None &&
                player.drawUnitsView.current.DrawDetailLayer;
        }
    }

    enum ControllerTabFocus
    { 
        Pause,
        CityMenu,
        ArmyMenu,
        GeneralObjectsMenu,
        Headmenu,
        Build,
    }
}
