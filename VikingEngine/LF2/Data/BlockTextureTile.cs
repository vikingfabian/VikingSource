using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Data
{
    class BlockTextureTile : Engine.AbsLoadTiles
    {
        public BlockTextureTile()
        {
            addTexture(SpriteName.BlockTexture, LoadTiles.BlockTextureWidth, LoadTiles.BlockTextureWidth, LoadedTexture.LF_TargetSheet);
        }
    }
}
