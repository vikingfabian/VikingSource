using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map.Map2
{
    class HeightMapTexture
    {
        PixelTexture pixelTexture;
        public HeightMapTexture(string path)
        {
            Texture2D texture = Texture2D.FromFile(Draw.graphicsDeviceManager.GraphicsDevice, path);
            pixelTexture = new PixelTexture(texture);
        }

        public void apply(Grid2D_L<GenTile> dataGrid)
        {

            int endX = Math.Min(pixelTexture.Width, dataGrid.Size.X);
            int endY = Math.Min(pixelTexture.Height, dataGrid.Size.Y);
            IntervalF heightSpan = new IntervalF(Map2Generator.Height_WaterBottom, Map2Generator.Height_MountainPeek);

            Parallel.For(0, endX, x =>
            {
                for (int y = 0; y < endY; y++)
                {
                    var tile = dataGrid.Get(x, y);
                    tile.groundY = heightSpan.GetFromBytePercent(pixelTexture.GetPixel(x, y).R);

                    dataGrid.Set(x, y, tile);
                }
            });
        }
    }
}
