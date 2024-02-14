using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace VikingEngine.Graphics
{
    /// <summary>
    /// Image with easy access to change idividual pixels
    /// </summary>
    class PixelImage : ImageAdvanced
    {
        public PixelTexture pixelTexture;

        public PixelImage(Vector2 pos, Vector2 drawSize, ImageLayers layer, bool centerMidpoint, IntVector2 texureSize, bool addToRender)
            : base(SpriteName.NO_IMAGE, pos, drawSize, layer, centerMidpoint, addToRender)
        {
            pixelTexture = new PixelTexture(texureSize);
            ImageSource = new Rectangle(0, 0, Texture.Width, Texture.Height);
        }        
    }

    class PixelTexture : Texture2D
    {
        Color[] pixels;

        public PixelTexture(IntVector2 texureSize)
            : base(Engine.Draw.graphicsDeviceManager.GraphicsDevice, texureSize.X, texureSize.Y)
        {
            pixels = new Color[texureSize.X * texureSize.Y];
        }

        public void SetPixel(IntVector2 pos, Color col)
        {
            pixels[pos.X + pos.Y * Width] = col;
        }

        public void SetPixel(int x, int y, Color col)
        {
            pixels[x + y * Width] = col;
        }


        public void ApplyPixelsToTexture()
        {
            SetData(pixels);
        }
        public void ApplyPixelsToTexture(Color[] pixels)
        {
            this.pixels = pixels;
            base.SetData(pixels);
        }

        public void SaveAsPNG(string path)
        {
            DateTime date = DateTime.Now;
            Stream stream = File.Create(path + ".png");
            SaveAsPng(stream, Width, Height);
        }
    }
}
