using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.HUD
{
    class ProcessBar : Graphics.AbsDraw2D
    {
        int borderSize = 2;
        Graphics.Image background;
        Graphics.Image bar;
        Percent val;
        public Percent Value 
        { 
            get { return val; }
            set
            {
                val = value;
                changedApperance();
            }
        }
        public override float Width
        {
            get
            {
                return background.Width;
            }
            set
            {
                background.Width = value;
            }
        }
        public Color BackgroundColor
        {
            get { return background.Color; }
            set { background.Color = value; }
        }
        public override Color Color
        {
            get
            {
                return bar.Color;
            }
            set
            {
                bar.Color = value;
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            background.DeleteMe();
            bar.DeleteMe();
        }
        public ProcessBar(Vector2 pos, Vector2 sz, ImageLayers layer):
            this(pos, sz, layer, new Percent(50), Color.Blue)
        { }

        public ProcessBar(Vector2 pos, Vector2 sz, ImageLayers layer, Percent value, Color color)
        {
            val = value;
            this.position = pos;
            this.size = sz;

            background = new VikingEngine.Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.One, layer + 1);
            bar = new VikingEngine.Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.One, layer);

            this.PaintLayer = background.PaintLayer;
            background.Color = Color.Black;
            bar.Color = color;
            

            changedApperance();
        }

        protected override void changedApperance()
        {
            background.Position = this.position;
            background.Size = this.size;

            bar.Position = this.position + VectorExt.V2(borderSize);
            if (this.Width > this.Height)
            {
                bar.Height = this.Height - borderSize * 2;
                bar.Width = (this.Width - borderSize * 2) * val.Value;
            }
            else
            {
                float totalHeight = this.Height - borderSize * 2;
                bar.Height = totalHeight  * val.Value;
                bar.Width = this.Width - borderSize * 2;
                bar.Ypos += totalHeight - bar.Height;
            }

            background.PaintLayer = this.PaintLayer;
            bar.PaintLayer = this.PaintLayer - PublicConstants.LayerMinDiff;
        }
        public override void Draw(int cameraIndex)
        { }

        public override bool Visible
        {
            get
            {
                return bar.Visible;
            }
            set
            {
                bar.Visible = value;
                background.Visible = value;
            }
        }

        public override void SetSpriteName(SpriteName name)
        {
            throw new NotImplementedException();
        }
    }
}
