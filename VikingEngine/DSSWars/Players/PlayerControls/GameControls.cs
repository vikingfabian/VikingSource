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
using VikingEngine.Input;
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

            //bool menuFocusState = mapControls.focusedObjectMenuState();
            
            
            player.hud.update();

            bool hudState = player.hud.mouseOverHud;

            if (hudState)
            {
                mapControls.leftFocusUpdate();
            }
            else
            {
                mapControls.focusedUpdate();

                if ((mapControls.hover.subTile.hasSelection && InBuildOrdersMode()) || buildControls.buildKeyDown)
                {
                    buildControls.updateBuildMode();
                    if (input.ControllerCancel.DownEvent)
                    {
                        buildControls.buildMode = SelectTileResult.None;
                    }
                }
                else
                {
                    if (input.ControllerSelect.DownEvent)
                    {
                        mapSelect();
                    }

                    if (input.Execute.DownEvent)
                    {
                        mapExecute();
                    }
                }
            }

            mapControls.passiveUpdate();

            //if (cityUpdate && player.input.AutomationSetting.DownEvent)
            //{
            //    hud.OpenAutomationMenu();
            //}

            if (armyControls != null)
            {
                armyControls.update();
            }
            else
            {
                updateMapShortCuts();
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
                        mapControls.setObjectMenuFocus(true);
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
                            armyControls = new ArmyControls(player, new List<AbsMapObject> { mapControls.selection.obj.GetArmy() });
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

            if (DssRef.difficulty.setting_allowPauseCommand &&
                input.PauseGame.DownEvent &&
                DssRef.state.localPlayers.Count == 1)//IsLocalHost())
            {
                player.hud.headOptions.pauseAction();
            }

            if (DssRef.state.IsSinglePlayer() && input.GameSpeed.DownEvent)
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
                buildControls.buildMode != SelectTileResult.None;
        }
    }
}
