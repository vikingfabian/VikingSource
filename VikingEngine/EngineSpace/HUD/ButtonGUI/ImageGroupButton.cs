using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD
{
    class ImageGroupButton : AbsButtonGui
    {
        public Graphics.ImageGroup imagegroup;
        public bool grayImagesOnDisable = false;

        public ImageGroupButton(VectorRect area, ImageLayers layer, ButtonGuiSettings sett)
            :base(sett)
        {
            this.area = area;
            baseImage = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, layer, false);
            baseImage.Color = sett.bgColor;

            imagegroup = new Graphics.ImageGroup(baseImage);
            createHighlight();
        }

        virtual public Graphics.TextG addCenterText(string textString, Color col, float percHeight)
        {
            Graphics.TextG textImg = new Graphics.TextG(LoadedFont.Regular,
                area.Center, Vector2.One, Graphics.Align.CenterAll, textString,
                col, ImageLayers.AbsoluteTopLayer);
            textImg.SetHeight(area.Height * percHeight);
            textImg.LayerAbove(baseImage);

            imagegroup.Add(textImg);

            return textImg;
        }

        public Graphics.Image addCoverImage(SpriteName sprite, float percSz = 1f)
        {
            VectorRect ar = area;
            if (percSz != 1f)
            {
                ar.Size *= percSz;
                ar.Center = area.Center;
            }

            var img = new Graphics.Image(sprite, ar.Position, ar.Size, ImageLayers.AbsoluteBottomLayer, false);
            img.LayerAbove(baseImage);
            img.PaintLayer -= Graphics.GraphicsLib.LayerDiff;

            imagegroup.Add(img);

            return img;
        }

        virtual public Graphics.Image addInputIcon(Dir4 side, Input.IButtonMap buttonMap)
        {
            return addInputIcon(side, buttonMap, imagegroup);
        }

        /// <summary>
        /// All images will be places in a row over the button center
        /// </summary>
        public void addImageStrip(List<Graphics.AbsDraw2D> imageStrip)
        {
            float totalW = 0;
            foreach (var m in imageStrip)
            {
                totalW += m.Width;
            }

            Vector2 center = area.Center;
            float xpos = center.X - totalW * 0.5f;

            foreach (var m in imageStrip)
            {
                imagegroup.Add(m);
                m.Position = new Vector2(xpos, center.Y - m.Height * 0.5f);
                xpos += m.Width;
            }
         }

        public override void DeleteMe()
        {
            base.DeleteMe();
            imagegroup.DeleteAll();
        }

        public override void Move(Vector2 move)
        {
            base.Move(move);
            imagegroup.Move(move);
        }

        protected override void onEnableChange()
        {
            base.onEnableChange();

            if (grayImagesOnDisable && imagegroup != null)
            {
                if (enabled)
                {
                    imagegroup.ColorAndAlpha(Color.White, 1f);
                }
                else
                {
                    imagegroup.ColorAndAlpha(Color.DarkGray, 0.5f);
                }
            }

        }

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }

            set
            {
                imagegroup.SetVisible(value);
                base.Visible = value;
            }
        }
    }
}
