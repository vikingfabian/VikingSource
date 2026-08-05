using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Communication;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Interface
{
    class PlayerHud_Object : IPlayerHud_Menu
    {
        List<GameObject.AbsGameObject> selectHistory = new List<AbsGameObject>();
        public NetSessionDisplay netSessionDisplay = new NetSessionDisplay();

        public DiplomacyDisplay diplomacy;
        public RichMenu menu, secondMenu;
        public AbsArmy otherArmy;

        public RichMenu Menu => menu;
        public bool IsFactionMenu { get { return false; } }
        public PlayerHud_Object(LocalPlayer player)
        {
            diplomacy = new DiplomacyDisplay(player);
        }

        public void createMenu(bool defaultMenu, LocalPlayer player, bool highOpacity = true)
        {
            RichMenu richMenu = defaultMenu? menu: secondMenu;

            if (richMenu == null)
            {
                var objectMenuArea = player.playerData.view.safeScreenArea;
                objectMenuArea.Width = HudLib.HeadDisplayWidth;

                if (!defaultMenu)
                {
                    objectMenuArea.nextAreaX(1, 16);
                }

                if (player.hud.head != null)
                {
                    objectMenuArea.Position.Y = player.hud.head.Bottom + Engine.Screen.IconSize * 0.5f;
                }
                objectMenuArea.SetBottom(player.playerData.view.safeScreenArea.Bottom, true);
                richMenu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
                var bgTex = richMenu.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);

                bgTex.SetColor(ColorExt.GrayScale(0.9f));

                if (defaultMenu)
                {
                    menu = richMenu;
                }
                else
                { 
                    secondMenu = richMenu;
                }
            }

            richMenu.backgroundTextures.SetOpacity(highOpacity ? 0.95f : 0.92f);
        }

        public void deleteMenu()
        {
            menu?.DeleteMe();
            menu = null;

            secondMenu?.DeleteMe();
            secondMenu = null;
        }

        void historyDisplay(Players.LocalPlayer player)
        {
            createMenu(true, player, false);

            var content = new RichBoxContent();

            if (player.DisplayBattleLab(content, menu))
            { 
            
            }
            else if (menu.menuStack.Count > 0)
            {
                switch (menu.menuStack.Last())
                {
                    case NetSessionDisplay.PAGE_BANWARNING:
                        netSessionDisplay.BanWarning(player, content, menu);
                        break;
                    case NetSessionDisplay.PAGE_REQUESTBLOCK:
                        netSessionDisplay.RequestBlock(player, content, menu);
                        break;
                    case NetSessionDisplay.PAGE_KICK:
                        netSessionDisplay.Kick(player, content, menu);
                        break;
                    case NetSessionDisplay.PAGE_BLOCK:
                        netSessionDisplay.Block(player, content, menu);
                        break;
                    case NetSessionDisplay.PAGE_RECOLOR:
                        netSessionDisplay.recolor(player, content, menu);
                        break;
                    case NetSessionDisplay.PAGE_DEBUG:
                        HudLib.returnButton(content, menu, true, null);
                        DssRef.state.playstate().PacketCountToHud(content);
                        break;
                }
            }
            else if (netSessionDisplay.ClientInteractDisplay)
            {
                netSessionDisplay.clientToHud(player, content, menu);
                netSessionDisplay.checkAlive();
            }
            else
            {
                if (DssRef.world.tileGrid.TryGet(player.gameControls.map.tilePosition, out var tile))
                {
                    var hoverCity = tile.City();
                    hoverCity.CityPresentationHud(new ObjectHudArgs(content), true);

                    if (hoverCity.pfaction == player.pfaction &&
                        player.mapLayer() <= Map.MapDetailLayerType.TerrainOverview2)
                    {
                        content.newLine();

                        player.gameControls.input.QuickSelect.ToRichContent(content);
                        content.space();
                        content.Add(new ArtButton(RbButtonStyle.Primary,
                            new List<AbsRichBoxMember> {
                            new RbText(DssRef.lang.Hud_SelectCity)
                            }, new RbAction(player.gameControls.selectAreaCity)));
                    }
                    content.Add(new RbSeperationLine());
                }

                
                if (DssRef.state.remotePlayers.Count > 0)
                {
                   

                    netSessionDisplay.overviewToHud(player, content, menu);
                    
                    content.newParagraph();
                }
                
                content.h2(DssRef.lang.Hud_SelectHistory, HudLib.TitleColor_Head);

                for (int i = selectHistory.Count - 1; i >= 0; --i)
                {
                    var obj = selectHistory[i];

                    if (obj.IsDeleted())
                    {
                        selectHistory.RemoveAt(i);
                    }
                    else
                    {
                        content.newLine();
                        RichBoxContent buttonContent = new RichBoxContent();
                        obj.toButtonContent(buttonContent, false);
                        content.Add(new ArtButton(RbButtonStyle.Outline,
                            buttonContent,

                            new RbAction1Arg<AbsGameObject>((AbsGameObject obj) =>
                            {
                                player.gameControls.selectObject(obj);
                            }, obj)));
                    }
                }
            }
            


            menu.Refresh(content, player.gameControls.controllerPointer);
        }

        public void refresh(Players.LocalPlayer player, RichBoxContent content)
        {
            //createMenu(player);
            menu.Refresh(content, player.gameControls.controllerPointer);
        }

        public void refreshDiplomacy(Players.LocalPlayer player, Faction faction, bool selected)
        {
            if (menu != null && menu.BlockRefresh())
            {
                return;
            }

            if (!player.hud.maximizedHud &&
                (faction == null || !selected))
            {
                deleteMenu();
                return;
            }

            createMenu(true,player);

            if (faction != null)
            {
                var content = new RichBoxContent();
                diplomacy.toHud(content, faction, true);
                menu.Refresh(content, player.gameControls.controllerPointer);
            }
            else if (player.factionPixelTexture.HeatMap())
            {
                var content = new RichBoxContent();
                player.factionPixelTexture.HeatMapInfoHud(content);
                menu.Refresh(content, player.gameControls.controllerPointer);
            }
            else
            {
                historyDisplay(player);
            }
        }

        public void refreshObject(Players.LocalPlayer player, GameObject.AbsGameObject obj, bool selected)
        {
            bool bUseSecondMenu = false;


            if (menu != null && menu.BlockRefresh())
            {
                return;
            }

            if (!player.hud.maximizedHud && 
                (obj == null || !selected))
            {
                deleteMenu();
                return;
            }

            if (obj == null)
            {
                historyDisplay(player);
            }
            else
            {
                createMenu(true, player);

                var content = new RichBoxContent();
                obj.toHud(new ObjectHudArgs(content, player, selected), out var secondMenuContent);
                menu.Refresh(content, player.gameControls.controllerPointer);

                if (secondMenuContent != null)
                {
                    bUseSecondMenu = true;
                    createMenu(false, player);
                    secondMenu.Refresh(secondMenuContent, player.gameControls.controllerPointer);
                }
            }

            if (selected)
            {
                for (int i = 0; i < selectHistory.Count; ++i)
                {
                    if (selectHistory[i] == obj)
                    {
                        selectHistory.RemoveAt(i);
                        break;
                    }
                }

                if (selectHistory.Count >= 8)
                {
                    selectHistory.RemoveAt(0);
                }

                selectHistory.Add(obj);                
            }

            if (!bUseSecondMenu)
            {
                secondMenu?.DeleteMe();
                secondMenu = null;
            }
        }

        /// <returns>need refresh</returns>
        public bool updateMouseInput(ref bool mouseOver)
        {
            bool needRefresh = false;

            if (secondMenu != null)
            {
                secondMenu.updateMouseInput(ref mouseOver);
                needRefresh = secondMenu.needRefresh;
            }

            if (menu != null)
            {
                menu.updateMouseInput(ref mouseOver);
                needRefresh |= menu.needRefresh;
            }
            return needRefresh;
        }
    }
}
