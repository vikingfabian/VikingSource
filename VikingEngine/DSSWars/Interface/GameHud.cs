using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface.HudPinUi;
using VikingEngine.DSSWars.Interface.MapObjMenu;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Interface
{
    interface IPlayerHud_Menu
    {
        RichMenu Menu { get; }
        bool IsFactionMenu { get; }
    }

    class GameHud
    {
        LocalPlayer player;
                
        public Tooltip tooltip;
        Timer.Basic refreshTimer = new Timer.Basic(500, false);
        public bool mouseOverHud = false;
        public bool needRefresh = false;
        //public HudDetailLevel detailLevel = HudDetailLevel.Normal;
        public bool maximizedHud = true;
        //public GameHudDisplays displays;
        //public GameHudMenu hudmenu;
        public MessageGroup_Ingame messages;
        public bool menuFocus = false;

        public PlayerHud_Head head;
        public PlayerHud_HeadOptions headOptions;
        public PlayerHud_Pins pinHud;
        public PlayerHud_Faction factionMenu;
        public PlayerHud_Object objMenu;
        public PlayerHud_InputHelp inputHelp;


        public Map.MiniMap miniMap;

        public PopMenu popMenu = null;
        public Vector2 MessageStart;
        public HudPinManager pins;

        public GameHud(LocalPlayer player, int numPlayers)
        {
            this.player = player;
            player.hud = this;
            MessageStart = new Vector2(player.playerData.view.safeScreenArea.Right - (RichMenu.DefaultRenderEdge.X + HudLib.MessageDisplayWidth),
               player.playerData.view.safeScreenArea.Y);

            //displays = new GameHudDisplays(player);
            if (DssRef.state.PlayType() == GameState.PlayStateType.Play)
            {
                head = new PlayerHud_Head(player);
            }
            if (player.IsLocalHost())
            {
                headOptions = new PlayerHud_HeadOptions(player);
            }
            if (DssRef.state.PlayType() == GameState.PlayStateType.Play)
            {
                pinHud = new PlayerHud_Pins(player);
            }
            objMenu = new PlayerHud_Object(player);
            factionMenu = new PlayerHud_Faction();

            if (numPlayers <= 1 && DssRef.difficulty.setting_gameMode != Data.GameModeMainType.Spectator)
            {
                miniMap = new MiniMap(player, false);
            }
            inputHelp = new PlayerHud_InputHelp(player, inputHelpBottom());
            

            //hudmenu = new GameHudMenu(player);
            messages = new MessageGroup_Ingame(player, numPlayers, HudLib.richboxGui);
            tooltip = new Tooltip();
            pins = new HudPinManager();
            
        }

        public bool HasPin(HudPin pinType)
        {
            var city = player.gameControls.map.selection.obj.GetCity();
            if (city != null)
            {
                return pins.TryGet(new CityHudPinId(city.myIndex, pinType));
            }
            return false;
        }
        public float inputHelpBottom()
        {
            if (miniMap == null)
            {
                return player.playerData.view.DrawArea.Bottom;
            }
            else
            {
                return miniMap.area.Y;
            }
        }

        public void initMap()
        {
            //miniMap = new Map.MiniMap(player.playerData);
        }

        public void OpenAutomationMenu()
        {
            //if (displays.HasMenuState(HeadDisplay.AutomationMenuState))
            //{
            //    displays.clearState();
            //}
            //else
            //{
            //    player.clearSelection();
            //    displays.SetMenuState(HeadDisplay.AutomationMenuState);
            //    if (player.input.inputSource.IsController)
            //    {
            //        setHeadMenuFocus(true);
            //    }
            //}
        }

        public void clearState()
        {
            setHeadMenuFocus(false);
            needRefresh = true;
            //displays.clearState();
        }

        public void setHeadMenuFocus(bool set)
        {
            if (menuFocus != set)
            {
                //displays.headDisplay.viewOutLine(set);
                //if (set)
                //{
                //    displays.beginMove(0);
                //}
                //else
                //{
                //    displays.clearMoveSelection();
                //}

                player.gameControls.map.focusMap(!set);
                menuFocus = set;
            }
        }

        public void updateMenuFocus()
        {
            //displays.updateMove(out bool bRefresh);
            //needRefresh |= bRefresh;

            //if (player.gameControls.input.CancelKey.DownEvent)
            //{
            //    player.gameControls.clearSelection();
            //}
        }

        public bool maxHudProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                maximizedHud = value;
                updateMenuDisplays(true);
            }
            return maximizedHud;
        }

        public bool minimapProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                if (value)
                {
                    miniMap = new MiniMap(player, true);
                }
                else
                {
                    miniMap?.DeleteMe();
                    miniMap = null;
                }
                inputHelp.deleteMenu();
                inputHelp.refreshUpdate(player);
            }
            return miniMap != null;
        }

        public void update(out bool refresh, bool allowInput)
        {
            //Debug.Log("game hud update");
            mouseOverHud = false;

            if (miniMap != null)
            {
                miniMap.update(player, allowInput, out bool miniMapMouseOver);
                mouseOverHud |= miniMapMouseOver;
            }

            if (allowInput)
            {
                
                refresh = refreshTimer.Update();

                refresh |= player.gameControls.map.selection.isNew ||
                    player.gameControls.map.hover.isNew ||
                    SteamWrapping.SteamInputManager.InputLayerChange ||
                    needRefresh;


                if (player.gameControls.input.ToggleHudDetail.DownEvent)
                {
                    //detailLevel++;
                    //if (detailLevel >= HudDetailLevel.NUM)
                    //{
                    //    detailLevel = 0;
                    //}
                    lib.Invert(ref maximizedHud);
                    refresh = true;
                }

                if (player.gameControls.input.ToggleMinimap.DownEvent)
                {
                    minimapProperty(null, true, miniMap == null);
                    //if (miniMap == null)
                    //{
                    //    miniMap = new MiniMap(player, true);                        
                    //}
                    //else
                    //{
                    //    miniMap?.DeleteMe();
                    //    miniMap = null;
                    //}
                    //inputHelp.deleteMenu();
                    //inputHelp.refreshUpdate(player);
                }


                if (player.gameControls.input.inputSource.HasMouseInstance)
                {
                    if (head != null)
                    {
                        refresh |= head.updateMouseInput(ref mouseOverHud);
                        refresh |= factionMenu.updateMouseInput(ref mouseOverHud);
                    }
                    if (headOptions != null)
                    {
                        refresh |= headOptions.updateMouseInput(ref mouseOverHud);
                    }
                    if (pinHud != null)
                    {
                        refresh |= pinHud.updateMouseInput(ref mouseOverHud);
                    }
                    refresh |= objMenu.updateMouseInput(ref mouseOverHud);

                }
                player.tutorial?.update(ref mouseOverHud);
                messages.Update(ref mouseOverHud);

                //if (displays.menuStateHasChange)
                //{
                //   refresh = true;
                //    displays.menuStateHasChange = false;
                //}



                if (refresh)
                {
                    //Debug.Log("game hud -refresh");
                    refreshTimer.Reset();
                    head?.refreshUpdate(player);
                    headOptions?.refreshUpdate();
                    pinHud?.refreshUpdate(player);
                    updateMenuDisplays(true);
                    factionMenu.refreshUpdate(player);
                    inputHelp.refreshUpdate(player);

                    needRefresh = false;
                }



                

                
            }
            else
            {
                refresh = false;
            }
        }

        public void updateMenuDisplays(bool refresh)
        {

            if (player.gameControls.diplomacy != null)
            {
                var faction = player.gameControls.diplomacy.mainSelection(out bool selected);

                objMenu.refreshDiplomacy(player, faction, selected);

                player.factionTab = MenuTab.NUM_NONE;
            }
            else if (player.gameControls.map.selection.obj != null)
            {
                updateObjectDisplay(player.gameControls.map.selection.obj, true, refresh);
                player.factionTab = MenuTab.NUM_NONE;


            }
            else if (player.gameControls.map.hover.obj != null)
            {
                updateObjectDisplay(player.gameControls.map.hover.obj, false, refresh);
                player.factionTab = MenuTab.NUM_NONE;
            }
            else if (player.factionTab != MenuTab.NUM_NONE)
            {
                //updateObjectDisplay(null, false, refresh);
                if (refresh)
                {
                    objMenu.deleteMenu();
                }
            }
            else if (!player.updateObjectDisplay())
            {
                //Remove display
                updateObjectDisplay(null, false, refresh);
            }

            void updateObjectDisplay(GameObject.AbsGameObject obj, bool selected, bool refresh)
            {
                if (refresh)
                {
                    objMenu.refreshObject(player, obj, selected);
                }
            }
        }

        public void updateToolTip_menu()
        {
            tooltip.clear();               
        }


        public void updateToolTip_diplomacy(bool refresh)
        {
            tooltip.updateMapTip(player, refresh, false);
            
        }

        public void updateToolTip_map(bool refresh)
        {
            
            if (!player.gameControls.map.focusedObjectMenuState())
            {
                tooltip.updateMapTip(player, refresh, false);
            }
        }

        public void updateToolTip_multiselect(bool refresh, bool aboveMouse)
        {
            
            if (!player.gameControls.map.focusedObjectMenuState())
            {
                tooltip.updateMapTip(player, refresh, aboveMouse);
            }
        }

        public bool hudMouseOver()
        {
            return mouseOverHud;
        }
    }
    //enum HudDetailLevel
    //{ 
    //    Minimal,
    //    Normal,
    //    //Extended,
    //    NUM
    //}
}
