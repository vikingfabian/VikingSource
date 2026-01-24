using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars;

namespace VikingEngine.CardDesign
{
    class LoadContent
    {
        public static readonly string ContentDir = "Card" + DataStream.FilePath.Dir;
        public LoadContent() 
        {
            Engine.LoadContent.LoadTexture(LoadedTexture.CardTiles, ContentDir + "CCGTiles");
            new SpriteSheet();
        }
    }
}
