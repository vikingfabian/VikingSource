using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map.Map2
{
    class HeightMapTexture
    {
        public PixelTexture pixelTexture;

        public float scale = 1;
        public float bottomHeight = 0;
        public float topHeight = Map2Generator.Height_MountainPeek;
        public IntVector2 offset = IntVector2.Zero;

        public string Name;

        public HeightMapTexture(string path)
        {
            Name = System.IO.Path.GetFileName(path);
            Texture2D texture = Texture2D.FromFile(Draw.graphicsDeviceManager.GraphicsDevice, path);
            pixelTexture = new PixelTexture(texture);
        }

        public void apply(Grid2D_L<GenTile> dataGrid)
        {
            IntVector2 trueOffset = offset;
            trueOffset.Multiply(scale);

            int endX = Math.Min(pixelTexture.Width, dataGrid.Size.X);
            int endY = Math.Min(pixelTexture.Height, dataGrid.Size.Y);
            IntervalF heightSpan = new IntervalF(bottomHeight, topHeight);

            Parallel.For(0, endX, x =>
            {
                for (int y = 0; y < endY; y++)
                {
                    var tile = dataGrid.Get(x, y);

                    IntVector2 pixelPos = new IntVector2(x / scale, y / scale) - trueOffset;

                    if (pixelTexture.TryGetPixel(pixelPos, out var pixel))
                    {
                        tile.groundY = heightSpan.GetFromPercent(pixel.GetBrightness());
                    }
                    else
                    {
                        tile.groundY = Map2Generator.Height_WaterBottom;
                    }

                    dataGrid.Set(x, y, tile);
                }
            });
        }

        public float scaleProperty(object tag, bool set, float value)
        {
            if (set)
            {
                scale = value;
            }
            return scale;
        }

        public float bottomHeightProperty(object tag, bool set, float value)
        {
            if (set)
            {
                bottomHeight = value;
            }
            return bottomHeight;
        }
        public float topHeightProperty(object tag, bool set, float value)
        {
            if (set)
            {
                topHeight = value;
            }
            return topHeight;
        }

        public int offSetXProperty(object tag, bool set, int value)
        {
            if (set)
            {
                offset.X = value;
            }
            return offset.X;
        }
        public int offSetYProperty(object tag, bool set, int value)
        {
            if (set)
            {
                offset.Y = value;
            }
            return offset.Y;
        }
    }
}
