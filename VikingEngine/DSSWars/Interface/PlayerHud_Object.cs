using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Interface
{
    class PlayerHud_Object
    {
        List<GameObject.AbsGameObject> selectHistory = new List<AbsGameObject>();
        
        DiplomacyDisplay diplomacy;
        public RichMenu menu;
        public AbsArmy otherArmy;



        public PlayerHud_Object(LocalPlayer player)
        {
            diplomacy = new DiplomacyDisplay(player);
        }

        public void createMenu(LocalPlayer player, bool highOpacity = true)
        {
            if (menu == null)
            {
                var objectMenuArea = player.playerData.view.safeScreenArea;
                objectMenuArea.Width = HudLib.HeadDisplayWidth;

                if (player.hud.head != null)
                {
                    objectMenuArea.Position.Y = player.hud.head.Bottom + Engine.Screen.IconSize * 0.5f;
                }
                objectMenuArea.SetBottom(player.playerData.view.safeScreenArea.Bottom, true);
                menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
                var bgTex = menu.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);

                bgTex.SetColor(ColorExt.GrayScale(0.9f));
               
            }

            menu.backgroundTextures.SetOpacity(highOpacity ? 0.95f : 0.92f);
        }

        public void deleteMenu()
        {
            menu?.DeleteMe();
            menu = null;
        }

        void historyDisplay(Players.LocalPlayer player)
        {
            createMenu(player, false);

            var content = new RichBoxContent();

            if (DssRef.world.tileGrid.TryGet(player.gameControls.map.tilePosition, out var tile))
            {
                var hoverCity = tile.City();
                hoverCity.CityPresentationHud(new ObjectHudArgs(content), true);

                if (hoverCity.factionIndex == player.faction.myIndex)
                {
                    content.newLine();
                    //RichBoxContent buttonContent = new RichBoxContent();
                    SpriteName buttonIcon;
                    if (player.gameControls.input.inputSource.IsController)
                    {
                        buttonIcon = player.gameControls.input.mouseSelect.Icon;
                    }
                    else
                    {
                        buttonIcon = player.gameControls.input.QuickSelect.Icon;
                    }
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                        new List<AbsRichBoxMember> {
                            new RbImage(buttonIcon),
                            new RbSpace(),
                            new RbText(DssRef.lang.Hud_SelectCity)
                        }, new RbAction(player.gameControls.selectAreaCity)));
                }
                content.Add(new RbSeperationLine());
            }

            if (DssRef.state.remotePlayers.Count > 0)
            {
                content.h2("Net session", HudLib.TitleColor_Head);
                var remoteC = DssRef.state.remotePlayers.counter();
                while(remoteC.Next())
                {
                    content.newLine();
                    remoteC.sel.RemoteToHud(content);
                }
                content.Add(new RbSeperationLine());
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
                    content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> {
                    new RbText(obj.Name(out _), HudLib.TitleColor_Name),
                    new RbImage(SpriteName.warsBulletSeperationPoint),
                    new RbText(obj.TypeName(), HudLib.TitleColor_TypeName) },
                        new RbAction1Arg<AbsGameObject>((AbsGameObject obj) =>
                        {
                            player.gameControls.selectObject(obj);
                        }, obj)));
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

            if (player.hud.detailLevel == HudDetailLevel.Minimal &&
                (faction == null || !selected))
            {
                deleteMenu();
                return;
            }

            if (faction == null)
            {
                historyDisplay(player);
            }
            else
            {
                createMenu(player);

                var content = new RichBoxContent();
                diplomacy.toHud(content, faction, selected);
                menu.Refresh(content, player.gameControls.controllerPointer);
            }
        }

        public void refreshObject(Players.LocalPlayer player, GameObject.AbsGameObject obj, bool selected)
        {
            if (menu != null && menu.BlockRefresh())
            {
                return;
            }

            if (player.hud.detailLevel == HudDetailLevel.Minimal && 
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
                createMenu(player);

                var content = new RichBoxContent();
                obj.toHud(new ObjectHudArgs(content, player, selected));
                menu.Refresh(content, player.gameControls.controllerPointer);
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
        }

        /// <returns>need refresh</returns>
        public bool updateMouseInput(ref bool mouseOver)
        {
            if (menu != null)
            {
                menu.updateMouseInput(ref mouseOver);
                return menu.needRefresh;
            }
            return false;
        }
    }
}
