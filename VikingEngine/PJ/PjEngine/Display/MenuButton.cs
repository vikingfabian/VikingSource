using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;
using VikingEngine.Input;

namespace VikingEngine.PJ.Display
{
    class AbsPjButton : HUD.ImageGroupButton
    {
        public Graphics.Image iconImg;
        VectorRect normalSz, hoverSz;
        public float iconScaleDown = 0.1f;

        public AbsPjButton(VectorRect area, ImageLayers layer, HUD.ButtonGuiSettings sett)
            : base(area, layer, sett)
        {
            normalSz = area;
            hoverSz = area;
            hoverSz.AddRadius(Engine.Screen.BorderWidth * 0.5f);
        }

        protected void initIconImg(SpriteName sprite)
        {
            iconImg = new Graphics.Image(sprite, area.Position, area.Size, ImageLayers.AbsoluteBottomLayer);
            iconImg.LayerAbove(baseImage);
            
            refreshIconSz();
        }

        protected override void onMouseEnter(bool enter)
        {
            base.onMouseEnter(enter);

            area = enter ? hoverSz : normalSz;
            baseImage.Area = area;

            refreshIconSz();
        }
        public void refreshIconSz()
        {
            if (iconImg != null)
            {
                var iconArea = area;
                iconArea.AddPercentRadius(-iconScaleDown);

                iconImg.Area = iconArea;
            }
        }

        public override bool Visible
        {
            get => base.Visible;
            set
            {
                base.Visible = value;
                if (iconImg != null)
                {
                    iconImg.Visible = value;
                }
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            iconImg.DeleteMe();
        }
    }
    
    class ImageButton : AbsPjButton
    {        
        public ImageButton(SpriteName sprite, VectorRect area, HUD.ButtonGuiSettings sett)
            : base(area, HudLib.LayButtons, sett)
        {
            baseImage.SetSpriteName(sprite);
        }
    }
    
    class MenuButton : AbsPjButton
    {
        Image inputImg;
        
        public MenuButton(VectorRect area, bool blue, SpriteName icon, ImageLayers layer, HUD.ButtonGuiSettings sett)
            : base(area, layer, sett)
        {
            baseImage.SetSpriteName(blue? SpriteName.pjButtonTexBlue : SpriteName.pjButtonTexRed);

            initIconImg(icon);
        }

        public override Image addInputIcon(Dir4 side, IButtonMap buttonMap)
        {
            return createInputIcon(side, buttonMap.Icon);
        }

        public Image createInputIcon(Dir4 side, SpriteName sprite)
        {
            Vector2 sz = Engine.Screen.IconSizeV2 * 0.8f;

            Vector2 center = area.Center + IntVector2.FromDir4(side).Vec * (sz.X * new Vector2(0.6f) + area.Size * new Vector2(0.5f));

            inputImg = new Graphics.Image(sprite, center, sz, ImageLayers.AbsoluteBottomLayer, true);
            inputImg.PaintLayer = baseImage.PaintLayer;

            imagegroup.Add(inputImg);

            inputImg.origo = new Vector2(0.5f, 0);
            inputImg.SetBottom(area.Bottom, false);

            return inputImg;
        }

        public void refreshInputIcon(IButtonMap buttonMap)
        {
            inputImg.SetSpriteName(buttonMap.Icon);
        }


        protected override void onEnableChange()
        {
            base.onEnableChange();

            float opacity = enabled? 1f : 0.3f;
            
            baseImage.Opacity = opacity;
            iconImg.Opacity = opacity;
            inputImg.Opacity = opacity;            
        }

        public static MenuButton ExitToLobbyButton()
        {
            VectorRect area = new VectorRect( Engine.Screen.SafeArea.RightBottom, HudLib.BigButtonsSize);
            area.Position -= area.Size;
            MenuButton button = new MenuButton(area, false, SpriteName.MenuIconResume, 
                HudLib.LayButtons, HudLib.LargeButtonSettings);

            IButtonMap menuInput, startInput, modeInput;
            HudLib.HudInputDisplay(out menuInput, out startInput, out modeInput);

            button.addInputIcon(Dir4.W, startInput);

            return button;
        }
    }
}
