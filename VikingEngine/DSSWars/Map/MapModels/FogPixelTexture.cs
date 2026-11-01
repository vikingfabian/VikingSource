using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars;

namespace VikingEngine.Core.DSSWars.Map.MapModels
{
    class FogPixelTexture
    {
        static readonly Color[] FogLevelColor = new Color[] {
            Color.Black,//None,
            new Color(byte.MinValue, byte.MinValue, byte.MinValue, (byte)100),//TerrainOnly,
            ColorExt.Empty,//Full,
        };
        public Graphics.PixelTexture texture;
        bool textureNeedsRefresh = false;

        public FogPixelTexture()
        {
            //initTexture();
        }

        public void Update_async()
        {
            //if (textureNeedsRefresh)
            //{
                texture.ApplyPixelsToTexture();
                textureNeedsRefresh = false;
            //}
        }

        public void initTexture()
        {
            var previousTexture = texture;
            texture = new Graphics.PixelTexture(DssRef.world.tileGrid.Size);
            texture.ClearPixelArray(Color.Black);
            texture.ApplyPixelsToTexture();

            Graphics.GpuResourceSync.Release(previousTexture);
        }

        public void refreshWorld()
        {
            refreshArea(DssRef.world.tileGrid.Area);
            
        }

        public void refreshArea(Rectangle2 sumTileArea)
        {
            ForXYLoop loop = new ForXYLoop(sumTileArea);
            while (loop.Next())
            {
                var tile = DssRef.world.tileGrid.Get(loop.Position);
                //if (tile.fogExploreLevel != VikingEngine.DSSWars.Map.MapData.FogOfWar_ExlporeLevel.None)
                //{
                //    lib.DoNothing();
                //}
                texture.SetPixel(loop.Position, FogLevelColor[(int)tile.fogExploreLevel]);
                //switch (DssRef.world.tileGrid.Get(loop.Position).fogExploreLevel)
                //{
                //    case VikingEngine.DSSWars.Map.MapData.FogOfWar_ExlporeLevel.Full:
                //        texture.SetPixel(loop.Position, ColorExt.
                //        break;
                //}
            }

            textureNeedsRefresh = true;
        }
    }
}
