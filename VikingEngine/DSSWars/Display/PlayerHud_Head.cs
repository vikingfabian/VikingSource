using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichMenu;
using VikingEngine.HUD;
using Microsoft.Xna.Framework;
using VikingEngine.HUD.RichBox;
using VikingEngine.DSSWars.Players;

namespace VikingEngine.DSSWars.Display
{
    class PlayerHud_Head
    {
        ImageAdvanced flag;
        NineSplitAreaTexture flagBg;
        RichMenu menu;
        public float Bottom;
        public PlayerHud_Head(LocalPlayer player)
        {
            float headWidth = HudLib.HeadDisplayWidth * 1.6f;
            var headMenuArea = player.playerData.view.safeScreenArea;
            headMenuArea.Width = headWidth;
            menu = new RichMenu(HudLib.RbSettings_Head, headMenuArea, new Vector2(HudLib.MenuEdgeSize), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
            refreshFaction(player);
            menu.updateHeightFromContent();

            VectorRect flagBgArea = new VectorRect(headMenuArea.Position, new Vector2(menu.backgroundArea.Height * 1.1f));
            var flagBgTexSett = new NineSplitSettings(SpriteName.WarsHudFlagBorder, 1, 8, 1f, true, true);
            flagBg = new NineSplitAreaTexture(flagBgTexSett, flagBgArea, HudLib.GUILayer + 2);
            menu.move(VectorExt.V2FromX(flagBgArea.Size.X - 4));
            flagBgArea.AddRadius(-(flagBgTexSett.BorderWidth() + 6));
            flag = new ImageAdvanced(SpriteName.NO_IMAGE, flagBgArea.Position, flagBgArea.Size, HudLib.GUILayer, false);
            flag.Texture = player.faction.flagTexture;
            flag.SetFullTextureSource();

            var headBgTex = menu.addBackground(new NineSplitSettings(SpriteName.WarsHudHeadBarBg, 1, 16, 1f, true, true), HudLib.GUILayer + 4);
            headBgTex.SetOpacity(0.95f);

            Bottom = menu.backgroundArea.Bottom;
        }
        public void refreshFaction(Players.LocalPlayer player)
        {
            var content = new RichBoxContent();
            player.faction.headMenu(content, false);
            menu.Refresh(content);
        }
        public void oneSecondUpdate(LocalPlayer player)
        {
            refreshFaction(player);
        }

        /// <returns>need refresh</returns>
        public bool updateMouseInput(ref bool mouseOver)
        { 
            menu.updateMouseInput(ref mouseOver);
            return menu.needRefresh;
        }
    }
    class PlayerHud_Faction
    {
    }
    class PlayerHud_HeadOptions
    {
        public Vector2 MessageStart;
        RichMenu menu;

        public PlayerHud_HeadOptions(LocalPlayer player)
        {
            
            //
            var optionsDisplayAr = player.playerData.view.safeScreenArea;
            optionsDisplayAr.X = 0;
            menu = new RichMenu(HudLib.RbSettings_HeadOptions, optionsDisplayAr, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
            {
                RichBoxContent content = new RichBoxContent();
                player.headOptionsMenu(content);
                menu.Refresh(content);
                menu.updateWidthFromContent();
                menu.move(VectorExt.V2FromX(player.playerData.view.safeScreenArea.Right - menu.backgroundArea.Width));
                menu.updateHeightFromContent(false);

                NineSplitAreaTexture bg = new NineSplitAreaTexture(new NineSplitSettings(SpriteName.WarsHudHeadBarSecondaryBg, 1, 11, 1f, true, true), menu.backgroundArea, HudLib.GUILayer + 4);
            }

            MessageStart = new Vector2(player.playerData.view.safeScreenArea.Right - (RichMenu.DefaultRenderEdge.X + HudLib.MessageDisplayWidth),
                menu.backgroundArea.Bottom + Engine.Screen.IconSize * 0.5f);
        }
        /// <returns>need refresh</returns>
        public bool updateMouseInput(ref bool mouseOver)
        {
            menu.updateMouseInput(ref mouseOver);
            return menu.needRefresh;
        }
    }
    class PlayerHud_Object
    {
        RichMenu menu;
        public PlayerHud_Object(LocalPlayer player)
        {           
            //
            var objectMenuArea = player.playerData.view.safeScreenArea;
            objectMenuArea.Width = HudLib.HeadDisplayWidth;
            objectMenuArea.Position.Y = player.hud.head.Bottom + Engine.Screen.IconSize * 0.5f;
            objectMenuArea.SetBottom(player.playerData.view.safeScreenArea.Bottom, true);
            menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
            var bgTex = menu.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);

            bgTex.SetColor(ColorExt.GrayScale(0.9f));
            bgTex.SetOpacity(0.95f);

           
        }
        public void refreshObject(Players.LocalPlayer player, GameObject.AbsGameObject obj, bool selected)
        {
            //interaction?.DeleteMe();
            //interaction = null;

            //setVisible(obj != null);

            if (obj != null)
            {
                //beginRefresh();
                //if (obj.CanMenuFocus() && player.input.inputSource.IsController)
                //{
                //    content.Add(new HUD.RichBox.RbImage(player.input.ControllerFocus.Icon));
                //    content.Add(new HUD.RichBox.RbText(":"));
                //    content.newLine();
                //}
                var content = new RichBoxContent();
                obj.toHud(new ObjectHudArgs(content, player, selected));
                menu.Refresh(content);
                //if (gui.menuState.Count > 0)
                //{
                //    content.newLine();
                //    content.Button(Ref.langOpt.Hud_Back, new RbAction(gui.menuBack, SoundLib.menuBack),
                //        null, true);
                //}
                //endRefresh(pos, selected);
                //viewOutLine(player.mapControls.focusedObjectMenuState());
            }
        }

        /// <returns>need refresh</returns>
        public bool updateMouseInput(ref bool mouseOver)
        {
            menu.updateMouseInput(ref mouseOver);
            return menu.needRefresh;
        }
    }
}
