using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Data
{
    class BlockTextureTile : Engine.AbsSpriteSheetLayout
    {
        public BlockTextureTile()
        {
            addTexture(SpriteName.BlockTexture, SpriteSheet.BlockTextureWidth, SpriteSheet.BlockTextureWidth, LfLib.BlockTexture);
        }
    }
}
