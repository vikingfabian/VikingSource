using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;
using VikingEngine.HUD;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    class Button : AbsButton
    {
        protected ImageLayers layer;
        public VectorRect contentArea;
        bool nineSplitTex = false;
        HUD.NineSplitAreaTexture texture;
        ButtonTextureStyle textureStyle;

        public Button(VectorRect area, ImageLayers layer, ButtonTextureStyle textureStyle)
            : base(area, layer + 1)
        {
            this.area = area;
            this.layer = layer;

            this.textureStyle = textureStyle;
            refreshTexture();

            highlight.ChangePaintLayer(2);
            //highlight.Color = Color.White;
        }


        void refreshTexture()
        {
            switch (textureStyle)
            {
                case ButtonTextureStyle.Standard:
                    if (enabled)
                    {
                        createNineSplitTex(SpriteName.cmdHudBorderButton);
                    }
                    else
                    {
                        createNineSplitTex(SpriteName.cmdHudBorderButtonGray);
                    }
                    break;
                case ButtonTextureStyle.StandardEdge:
                    createNineSplitTex(SpriteName.cmdHudBorderThickButton);
                    break;
                case ButtonTextureStyle.Popup:
                    createNineSplitTex(SpriteName.cmdHudPopupButton);
                    break;
                case ButtonTextureStyle.PopupEdge:
                    createNineSplitTex(SpriteName.cmdHudBorderPopupButton);
                    break;
            }
        }

        public override bool Visible
        {
            get => base.Visible;
            set
            {
                base.Visible = value;

                if (nineSplitTex)
                { baseImage.Visible = false; }

                texture?.SetVisible(value);
            }
        }

        protected override void onEnableChange()
        {
            base.onEnableChange();

            refreshTexture();
        }

        public static float EdgeSz()
        {
            return 6 * HudLib.BorderScale;
        }

        void createNineSplitTex(SpriteName sprite)
        {
            texture?.DeleteMe();

            texture = new HUD.NineSplitAreaTexture(
                sprite, 1, 8, area, HudLib.BorderScale, true, layer, true);

            contentArea = area;
            contentArea.AddRadius(-EdgeSz());
            //imagegroup.images.AddRange(texture.images);
            nineSplitTex = true;
            baseImage.Visible = false;
        }

        public override TextG addCenterText(string textString, Color col, float percHeight)
        {
            var text = base.addCenterText(textString, col, percHeight);
            text.Layer = layer - 1;
            return text;
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            texture?.DeleteMe();
        }

        public override void Move(Vector2 move)
        {
            base.Move(move);
            if (texture != null)
            {
                foreach (var m in texture.images)
                {
                    m.AddXY(move);
                }                
            }
        }
    }

    enum ButtonTextureStyle
    {
        None,
        Standard,
        StandardEdge,
        Popup,
        PopupEdge,
    }
}
