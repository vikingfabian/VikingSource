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

        public byte bits_renderStateA = Culling.NoRender;
        public byte bits_renderStateB = Culling.NoRender;
        public byte heightLevel;

        [MarshalAs(UnmanagedType.I1)]
        public bool hasTileInRender = false;

        public MapChunkData8_8()
        { }
    }
}
