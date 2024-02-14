using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.HeroQuest.Gadgets;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class AbsPopupWindow
    {
        static readonly Color TitleCol = new Color(224, 216, 204);
        public Graphics.ImageGroup images = new Graphics.ImageGroup(32);

        protected HeroQuest.Display.Button exitButton;

        protected Vector2 contentStartPos()
        {
            return new Vector2(sideEdge(), titleHeight() + sideEdge());
        }

        protected float sideEdge()
        {
            return Engine.Screen.BorderWidth * 2f;
        }

        protected float titleHeight()
        {
            return Engine.Screen.TextTitleHeight + Engine.Screen.BorderWidth * 2f;
        }

        protected void completeWindow(VectorRect area, SpriteName titleIcon, string titleText, 
            ImageLayers contentLayer, bool bExitButton)
        {
            Vector2 titleContentPos = VectorExt.AddY(area.CenterTop, Engine.Screen.BorderWidth);
            
            Graphics.Text2 title = new Graphics.Text2(titleText, LoadedFont.Bold,
                titleContentPos, Engine.Screen.TextTitleHeight,
                TitleCol, contentLayer);
            title.OrigoAtCenterWidth();
            images.Add(title);

            if (titleIcon != SpriteName.NO_IMAGE)
            {
                Vector2 pos = title.position;
                pos.X -= title.MeasureText().X * 0.5f;
                pos.Y += Engine.Screen.TextTitleHeight * 0.5f;

                Graphics.Image icon = new Graphics.Image(titleIcon, pos,
                   new Vector2(Engine.Screen.TextTitleHeight * 1.1f), contentLayer, true);
                icon.OrigoAtCenterWidth();
                icon.Xpos -= icon.Width * 0.5f;
                images.Add(icon);
            }

            Color titleBgCol = new Color(71, 51, 30);
            VectorRect titleBgAr = new VectorRect(area.Position, new Vector2(area.Width, titleHeight()));
            titleBgAr.AddToTopSide(-4);
            titleBgAr.AddXRadius(-4);
            Graphics.Image titleBg = new Graphics.Image(SpriteName.WhiteArea, titleBgAr.Position, titleBgAr.Size,
                ImageLayers.AbsoluteBottomLayer);
            titleBg.Color = titleBgCol;
            titleBg.LayerBelow(title);
            images.Add(titleBg);

            if (bExitButton)
            {
                Vector2 btnSz = new Vector2(1.5f, 1f) * titleHeight();

                VectorRect btnAr = new VectorRect(area.RightTop, btnSz);
                btnAr.X -= btnAr.Width;

                exitButton = new HeroQuest.Display.Button(btnAr, contentLayer - 2, 
                    HeroQuest.Display.ButtonTextureStyle.PopupEdge);
                var cross = exitButton.addCoverImage(SpriteName.cmdHudCross, 0.8f);
                cross.Width = cross.Height;
                cross.Color = new Color(23, 15, 10);
                cross.Center = btnAr.Center;
            }
            var texture = new HUD.NineSplitAreaTexture(
               SpriteName.cmdHudBorderPopup, 1, 12, area, HudLib.BorderScale, true, contentLayer +2, true);
            images.Add(texture);
        }

        virtual public void DeleteMe()
        {
            images.DeleteAll();
            exitButton?.DeleteMe();
        }
    }
}
