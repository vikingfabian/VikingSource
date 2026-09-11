using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VikingEngine.DSSWars.Map.MapData
{
    /// <summary>
    /// Groups 8 by 8 tiles in one model
    /// </summary>
    struct MapChunkData8_8
    {
        public const int TileWidth = 8;
        public const float ModelScale = MapTile1_1.ModelScale * TileWidth;


        public bool hasTileInRender = false;
        public byte bits_renderStateA = Culling.NoRender;
        public byte bits_renderStateB = Culling.NoRender;
        //public byte heightLevel;
        public float exitRenderTimeStamp_TotSec = 0;
        public int subtileVisualEdits = 0;

        
        public MapChunkData8_8()
        { }

        public bool OutOfRenderTimeOut()
        {
            return (Ref.TotalGameTimeSec - exitRenderTimeStamp_TotSec) > 1f;
        }
    }
}
