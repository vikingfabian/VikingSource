using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Display
{
    class GameHud
    {
        LocalPlayer player;
                
        public Tooltip tooltip;
        Timer.Basic refreshTimer = new Timer.Basic(500, false);
        public bool mouseOverHud = false;
        public bool needRefresh = false;
        public HudDetailLevel detailLevel = HudDetailLevel.Normal;

        //public GameHudDisplays displays;
        //public GameHudMenu hudmenu;
        public MessageGroup messages;
        public bool menuFocus = false;

        public PlayerHud_Head head;
        public PlayerHud_HeadOptions headOptions;
        public PlayerHud_Faction factionMenu;
        public PlayerHud_Object objMenu;

         Map.MiniMap miniMap;

        public PopMenu popMenu = null;

        public GameHud(LocalPlayer player, int numPlayers)
        {
            this.player = player;
            player.hud = this;
            //displays = new GameHudDisplays(player);
            if (DssRef.state.PlayType() == GameState.PlayStateType.Play)
            {
                head = new PlayerHud_Head(player);
            }
            headOptions = new PlayerHud_HeadOptions(player);
            objMenu = new PlayerHud_Object(player);
            factionMenu = new PlayerHud_Faction();
           
            //hudmenu = new GameHudMenu(player);
            messages = new MessageGroup(player, numPlayers, HudLib.richboxGui);
            tooltip = new Tooltip();
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

                player.gameControls.mapControls.focusMap(!set);
                menuFocus = set;
            }
        }

        public void updateMenuFocus()
        {
            //displays.updateMove(out bool bRefresh);
            //needRefresh |= bRefresh;

            if (player.gameControls.input.CancelKey.DownEvent)
            {
                player.gameControls.clearSelection();
            }
        }

        public void update(out bool refresh)
        {
            //Debug.Log("game hud update");

            mouseOverHud = false;
            refresh = refreshTimer.Update();

            refresh |= player.gameControls.mapControls.selection.isNew ||
                player.gameControls.mapControls.hover.isNew ||
                needRefresh;

            

            if (player.gameControls.input.ToggleHudDetail.DownEvent)
            {
                detailLevel++;
                if (detailLevel >= HudDetailLevel.NUM)
                { 
                    detailLevel = 0;
                }
                refresh = true;
            }

            
            //updateMenuDisplays(refresh);
            
            

            if (player.gameControls.input.inputSource.HasMouse)
            {
                //needRefresh |= displays.update();
                //mouseOver = hudMouseOver();

                if (head != null)
                {
                    refresh |= head.updateMouseInput(ref mouseOverHud);
                    refresh |= factionMenu.updateMouseInput(ref mouseOverHud);
                }
                refresh |= headOptions.updateMouseInput(ref mouseOverHud);
                refresh |= objMenu.updateMouseInput(ref mouseOverHud);
                //refresh |= head.updateMouseInput(ref mouseOverHud);

                //refresh = false;
                player.tutorial?.update(ref mouseOverHud);
                messages.Update(ref mouseOverHud);
            }


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
                headOptions.refreshUpdate();
                updateMenuDisplays(true);
                factionMenu.refreshUpdate(player);

                needRefresh = false;
            }

            

            void updateMenuDisplays(bool refresh)
            {

                if (player.diplomacyMap != null)
                {
                    var faction = player.diplomacyMap.mainSelection(out bool selected);

                    objMenu.refreshDiplomacy(player, faction, selected);

                    player.factionTab = MenuTab.NUM_NONE;
                }
                else if (player.gameControls.mapControls.selection.obj != null)
                {
                    updateObjectDisplay(player.gameControls.mapControls.selection.obj, true, refresh);
                    player.factionTab = MenuTab.NUM_NONE;

                    
                }
                else if (player.gameControls.mapControls.hover.obj != null)
                {
                    updateObjectDisplay(player.gameControls.mapControls.hover.obj, false, refresh);
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
            }

            void updateObjectDisplay(GameObject.AbsGameObject obj, bool selected, bool refresh)
            {
                if (refresh)
                {
                    objMenu.refreshObject(player, obj, selected);
                }
            }
        }

        public void updateToolTip_menu(bool refresh)
        {
            tooltip.clear();               
        }

        public void updateToolTip_map(bool refresh)
        {
            
            if (!player.gameControls.mapControls.focusedObjectMenuState())
            {
                tooltip.updateMapTip(player, refresh, false);
            }
        }

        public void updateToolTip_multiselect(bool refresh, bool aboveMouse)
        {
            
            if (!player.gameControls.mapControls.focusedObjectMenuState())
            {
                tooltip.updateMapTip(player, refresh, aboveMouse);
            }
        }

        public bool hudMouseOver()
        {
            return mouseOverHud;
        }
    }
    enum HudDetailLevel
    { 
        Minimal,
        Normal,
        //Extended,
        NUM
    }
}
