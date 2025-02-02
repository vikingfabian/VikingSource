//using Microsoft.VisualBasic;
//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Metadata;
//using System.Text;
//using System.Threading.Tasks;
//using VikingEngine.DSSWars.Players;
//using VikingEngine.Graphics;
//using VikingEngine.HUD;
//using VikingEngine.HUD.RichBox;
//using VikingEngine.HUD.RichMenu;

//namespace VikingEngine.DSSWars.Display
//{
//    class GameHudMenu
//    {
//        ImageAdvanced flag;
//        NineSplitAreaTexture flagBg;
//        RichMenu headDisplay, optionsDisplay;
//        RichMenu factionDisplay;

//        RichMenu objectDisplay;


//        public GameHudMenu(LocalPlayer player)
//        {
//            float headWidth = HudLib.HeadDisplayWidth * 1.6f;
//            var headMenuArea = player.playerData.view.safeScreenArea;
//            headMenuArea.Width = headWidth;
//            headDisplay = new RichMenu(HudLib.RbSettings_Head, headMenuArea, new Vector2(HudLib.MenuEdgeSize), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
//            refreshFaction(player);
//            headDisplay.updateHeightFromContent();

//            VectorRect flagBgArea = new VectorRect(headMenuArea.Position, new Vector2(headDisplay.backgroundArea.Height * 1.1f));
//            var flagBgTexSett = new NineSplitSettings(SpriteName.WarsHudFlagBorder, 1, 8, 1f, true, true);
//            flagBg = new NineSplitAreaTexture(flagBgTexSett, flagBgArea, HudLib.GUILayer + 2);
//            headDisplay.move(VectorExt.V2FromX(flagBgArea.Size.X - 4));
//            flagBgArea.AddRadius(-(flagBgTexSett.BorderWidth() + 6));
//            flag = new ImageAdvanced(SpriteName.NO_IMAGE, flagBgArea.Position, flagBgArea.Size, HudLib.GUILayer, false);
//            flag.Texture = player.faction.flagTexture;
//            flag.SetFullTextureSource();
                        
//            var headBgTex = headDisplay.addBackground(new NineSplitSettings(SpriteName.WarsHudHeadBarBg, 1, 16, 1f, true, true), HudLib.GUILayer + 4);
//            headBgTex.SetOpacity(0.95f);

//            var objectMenuArea = player.playerData.view.safeScreenArea;
//            objectMenuArea.Width = HudLib.HeadDisplayWidth;
//            objectMenuArea.Position.Y = headDisplay.backgroundArea.Bottom + Engine.Screen.IconSize * 0.5f;
//            objectMenuArea.SetBottom(player.playerData.view.safeScreenArea.Bottom, true);
//            objectDisplay = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
//            var bgTex = objectDisplay.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);
            
//            bgTex.SetColor(ColorExt.GrayScale(0.9f));
//            bgTex.SetOpacity(0.95f);

//            var optionsDisplayAr = player.playerData.view.safeScreenArea;
//            optionsDisplayAr.X = 0;
//            optionsDisplay = new RichMenu(HudLib.RbSettings_HeadOptions, optionsDisplayAr, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
//            {
//                RichBoxContent content = new RichBoxContent();
//                player.headOptionsMenu(content);
//                optionsDisplay.Refresh(content);
//                optionsDisplay.updateWidthFromContent();
//                optionsDisplay.move(VectorExt.V2FromX(player.playerData.view.safeScreenArea.Right - optionsDisplay.backgroundArea.Width));
//                optionsDisplay.updateHeightFromContent(false);

//                NineSplitAreaTexture bg = new NineSplitAreaTexture(new NineSplitSettings(SpriteName.WarsHudHeadBarSecondaryBg, 1, 11, 1f, true, true), optionsDisplay.backgroundArea, HudLib.GUILayer + 4);
//            }

//            MessageStart = new Vector2(player.playerData.view.safeScreenArea.Right - ( RichMenu.DefaultRenderEdge.X + HudLib.MessageDisplayWidth),
//                optionsDisplay.backgroundArea.Bottom + Engine.Screen.IconSize * 0.5f);
//        }

//        //public void updateInput()
//        //{ 
            
//        //}

//        public void oneSecondUpdate(LocalPlayer player)
//        {
//            refreshFaction(player);
//        }

//        public void refreshFaction(Players.LocalPlayer player)
//        {
//            var content = new RichBoxContent();
//            player.faction.headMenu(content, false);
//            headDisplay.Refresh(content);
//        }

//        public void refreshObject(Players.LocalPlayer player, GameObject.AbsGameObject obj, bool selected)
//        {
//            //interaction?.DeleteMe();
//            //interaction = null;

//            //setVisible(obj != null);

//            if (obj != null)
//            {
//                //beginRefresh();
//                //if (obj.CanMenuFocus() && player.input.inputSource.IsController)
//                //{
//                //    content.Add(new HUD.RichBox.RbImage(player.input.ControllerFocus.Icon));
//                //    content.Add(new HUD.RichBox.RbText(":"));
//                //    content.newLine();
//                //}
//                var content = new RichBoxContent();
//                obj.toHud(new ObjectHudArgs(content, player, selected));
//                objectDisplay.Refresh(content);
//                //if (gui.menuState.Count > 0)
//                //{
//                //    content.newLine();
//                //    content.Button(Ref.langOpt.Hud_Back, new RbAction(gui.menuBack, SoundLib.menuBack),
//                //        null, true);
//                //}
//                //endRefresh(pos, selected);
//                //viewOutLine(player.mapControls.focusedObjectMenuState());
//            }
//        }
//    }
//}
