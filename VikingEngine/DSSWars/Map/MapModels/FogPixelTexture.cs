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
        static readonly Color TerrainOnlyCol = Color.MultiplyAlpha(DssLib.FogColor, 0.25f);//new Color(byte.MinValue, byte.MinValue, byte.MinValue, (byte)100);
        static readonly Color FullViewCol = ColorExt.Empty;
        
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
            texture = new Graphics.PixelTexture(DssRef.world.sumtileGrid.Size);
            texture.ClearPixelArray(DssLib.FogColor);
            texture.ApplyPixelsToTexture();

            Graphics.GpuResourceSync.Release(previousTexture);
        }

        public void refreshWorld()
        {
            refreshArea(DssRef.world.sumtileGrid.Area);
            
        }

        public void refreshArea(Rectangle2 sumTileArea)
        {
            ForXYLoop loop = new ForXYLoop(sumTileArea);
            while (loop.Next())
            {
                var tile = DssRef.world.sumtileGrid.Get(loop.Position);
                //if (tile.fogExploreLevel != VikingEngine.DSSWars.Map.MapData.FogOfWar_ExlporeLevel.None)
                //{
                //    lib.DoNothing();
                //}
                if (tile.fogExploreLevel > WorldData.FogOfWarView_None)
                {
                    if (tile.fogExploreLevel > WorldData.FogOfWarView_TerrainOnly)
                    {
                        texture.SetPixel(loop.Position, FullViewCol);
                    }
                    else
                    {
                        texture.SetPixel(loop.Position, TerrainOnlyCol);
                    }
                }
                
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
