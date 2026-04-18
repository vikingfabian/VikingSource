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
        public bool InBound(IntVector2 pos)
        {
            return pos.X >= 0 && pos.X < Width &&
                 pos.Y >= 0 && pos.Y < Height;
        }

        public bool InBound_TwoPixels(IntVector2 pos)
        {
            return pos.X >= 0 && pos.X + 1 < Width &&
                 pos.Y >= 0 && pos.Y + 1 < Height;
        }
        public void SetPixel(IntVector2 pos, Color col)
        {
            pixels[pos.X + pos.Y * Width] = col;
        }

        public void SetPixel(int x, int y, Color col)
        {
            pixels[x + y * Width] = col;
        }

        public void SetTwoPixels(IntVector2 pos, Color col1, Color col2)
        {
            int index = pos.X + pos.Y * Width;
            pixels[index] = col1;
            pixels[index + 1] = col2;
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

        public void ClearPixelArray(Color color)
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }
        }

        public void SaveAsPNG(string path)
        {
            DateTime date = DateTime.Now;
            Stream stream = File.Create(path + ".png");
            SaveAsPng(stream, Width, Height);
        }
    }
}
