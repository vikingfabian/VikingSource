using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.Display
{
    class GameHud
    {
        LocalPlayer player;
                
        public Tooltip tooltip;
        Timer.Basic refreshTimer = new Timer.Basic(500, false);
        public bool mouseOver = false;
        public bool needRefresh = false;
        public HudDetailLevel detailLevel = HudDetailLevel.Normal;

        public GameHudDisplays displays;
        //public GameHudMenu hudmenu;
        public MessageGroup messages;
        public bool menuFocus = false;

        public PlayerHud_Head head;
        public PlayerHud_HeadOptions headOptions;
        public PlayerHud_Object objMenu;


        public GameHud(LocalPlayer player, int numPlayers)
        {
            this.player = player;
            player.hud = this;
            displays = new GameHudDisplays(player);

            head = new PlayerHud_Head(player);
            headOptions = new PlayerHud_HeadOptions(player);
            objMenu = new PlayerHud_Object(player);
            //hudmenu = new GameHudMenu(player);
            messages = new MessageGroup(player, numPlayers, HudLib.richboxGui);
            tooltip = new Tooltip();
        }

        public void OpenAutomationMenu()
        {
            if (displays.HasMenuState(HeadDisplay.AutomationMenuState))
            {
                displays.clearState();
            }
            else
            {
                player.clearSelection();
                displays.SetMenuState(HeadDisplay.AutomationMenuState);
                if (player.input.inputSource.IsController)
                {
                    setHeadMenuFocus(true);
                }
            }
        }

        public void clearState()
        {
            setHeadMenuFocus(false);
            displays.clearState();
        }

        public void setHeadMenuFocus(bool set)
        {
            if (menuFocus != set)
            {
                displays.headDisplay.viewOutLine(set);
                if (set)
                {
                    displays.beginMove(0);
                }
                else
                {
                    displays.clearMoveSelection();
                }

                player.mapControls.focusMap(!set);
                menuFocus = set;
            }
        }

        public void updateMenuFocus()
        {
            displays.updateMove(out bool bRefresh);
            needRefresh |= bRefresh;

            if (player.input.AutomationSetting.DownEvent ||
                player.input.ControllerCancel.DownEvent)
            {
                player.clearSelection();
            }
        }

        public void update()
        {
            bool refresh = refreshTimer.Update();

            refresh |= player.mapControls.selection.isNew ||
                player.mapControls.hover.isNew ||
                needRefresh;

            if (player.input.ToggleHudDetail.DownEvent)
            {
                detailLevel++;
                if (detailLevel >= HudDetailLevel.NUM)
                { 
                    detailLevel = 0;
                }
                refresh = true;
            }

            needRefresh = false;
            updateMenuDisplays(refresh);
            
            if (refresh)
            {
                refreshTimer.Reset();
                head.oneSecondUpdate(player);
            }

            if (player.input.inputSource.HasMouse)
            {
                //needRefresh |= displays.update();
                //mouseOver = hudMouseOver();

                needRefresh |= head.updateMouseInput();
                needRefresh |= headOptions.updateMouseInput();
                needRefresh |= objMenu.updateMouseInput();

            }

            if (displays.menuStateHasChange)
            {
                updateMenuDisplays(true);
                displays.menuStateHasChange = false;
            }

            if (mouseOver)
            {
                tooltip.updateDiplayTip(player, displays.hasInteractButtonHover());
            }
            else
            {
                if (!player.mapControls.focusedObjectMenuState())
                {
                    tooltip.updateMapTip(player, refresh);
                }
            }

            messages.Update(displays.BottomLeft());

            void updateMenuDisplays(bool refresh)
            {
                if (player.diplomacyMap != null)
                {                    
                    displays.headDisplay.refreshUpdate(player, !player.diplomacyMap.hasSelectionOrHover(), refresh, player.faction);

                    if (player.diplomacyMap.hasSelectionOrHover())
                    {
                        if (refresh)
                        {
                            Vector2 pos = displays.headDisplay.area.LeftBottom;
                            pos.Y += Engine.Screen.BorderWidth * 2f;
                            displays.diplomacyDisplay.refresh(pos);
                            displays.diplomacyDisplay.viewOutLine(player.diplomacyMap.hasSelection());
                        }
                    }
                    else
                    {
                        displays.diplomacyDisplay.setVisible(false);
                    }
                    displays.objectDisplay.setVisible(false);
                }
                else if (player.mapControls.selection.obj != null)
                {
                    displays.headDisplay.refreshUpdate(player, false, refresh, player.faction);
                    updateObjectDisplay(player.mapControls.selection.obj, true, refresh);
                    displays.diplomacyDisplay.setVisible(false);
                }
                else if (player.mapControls.hover.obj != null)
                {
                    displays.headDisplay.refreshUpdate(player, false, refresh, player.faction);
                    updateObjectDisplay(player.mapControls.hover.obj, false, refresh);
                    displays.diplomacyDisplay.setVisible(false);
                }
                else
                {
                    displays.headDisplay.refreshUpdate(player, true, refresh, player.faction);
                    updateObjectDisplay(null, false, refresh);
                    displays.diplomacyDisplay.setVisible(false);
                }
            }

            void updateObjectDisplay(GameObject.AbsGameObject obj, bool selected, bool refresh)
            {
                if (refresh)
                {
                    //Vector2 pos = displays.headDisplay.area.LeftBottom;
                    //pos.Y += Engine.Screen.BorderWidth * 2f;
                    //displays.objectDisplay.refresh(player, obj, selected, pos);
                    objMenu.refreshObject(player, obj, selected);
                }
            }
        }

        //public void oneSecondUpdate(LocalPlayer player)
        //{
        //    refreshFaction(player);
        //}

        public bool hudMouseOver()
        {
            return displays.mouseOver() || messages.mouseOver();
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
