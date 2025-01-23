using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Engine
{
    class LoadBaseTextures : AbsSpriteSheetLayout
    {
        public LoadBaseTextures()
        {
            const int WhiteAreaSize = 256;
            addTexture(SpriteName.WhiteArea, WhiteAreaSize, WhiteAreaSize, LoadedTexture.WhiteArea);
        }
    }

    abstract class AbsSpriteSheetLayout
    {
        protected LoadedTexture TileSheetIx = LoadedTexture.NO_TEXTURE;
        protected int currentIndex = 0;
        
        int TileSheetSize;
        protected int numTilesWidth;
        protected int TileSize;
        protected int TileHalfSize;
        float TilePercentSize;

        protected void Settings(int tileSheetSize, int numTilesWidth)
        {
            TileSheetSize = tileSheetSize;
            this.numTilesWidth = numTilesWidth;
            TileSize = TileSheetSize / numTilesWidth;
            TileHalfSize = TileSize / 2;
            TilePercentSize = 1f / numTilesWidth;
        }
        const int StandardTileSize = 1;
        protected void addFullQtile(SpriteName nwSprite, SpriteName neSprite, SpriteName swSprite, SpriteName seSprite)
        {
            addQtile(nwSprite, Corner.NW, currentIndex);
            addQtile(neSprite, Corner.NE, currentIndex);
            addQtile(swSprite, Corner.SW, currentIndex);
            addQtile(seSprite, Corner.SE, currentIndex);
        }
        protected void addFullQtile(SpriteName nwSprite, SpriteName neSprite, SpriteName swSprite, SpriteName seSprite, int index)
        {
            addQtile(nwSprite, Corner.NW, index);
            addQtile(neSprite, Corner.NE, index);
            addQtile(swSprite, Corner.SW, index);
            addQtile(seSprite, Corner.SE, index);
        }
        protected void addQtile(SpriteName name, Corner quarterIndex)
        {
            this.addQtile(name, quarterIndex, currentIndex);
        }
        protected void addQtile(SpriteName name, Corner quarterIndex, int index)
        {
            if (quarterIndex == Corner.SE)
                currentIndex = index + 1;
            else
                currentIndex = index;

            int ypos = tileIxYpos(ref index);
            Rectangle source = new Microsoft.Xna.Framework.Rectangle(
                index * TileSize, ypos * TileSize,
                TileHalfSize, TileHalfSize);
            switch (quarterIndex)
            {
                case Corner.NE:
                    source.X += TileHalfSize;
                    break;
                case Corner.SW:
                    source.Y += TileHalfSize;
                    break;
                case Corner.SE:
                    source.X += TileHalfSize;
                    source.Y += TileHalfSize;
                    break;

            }
            DataLib.SpriteCollection.Set(name, new Sprite(source, TileSheetIx));
        }
       
        protected void add(SpriteName name)
        {
            this.add(name, currentIndex, StandardTileSize, StandardTileSize);
        }
        protected void add(SpriteName name, int index)
        {
            this.add(name, index, StandardTileSize, StandardTileSize);
        }

        protected void add(SpriteName name, int width, int height)
        {
            this.add(name, currentIndex, width, height);
        }
        /// <summary>
        /// Om du har alla dina tiles i ett rut system, man räknar rutorna på samma sätt man läser
        /// </summary>
        /// <param name="name">Enum för att namnge tilen</param>
        /// <param name="index">Vilket ruta du använder</param>
        /// <param name="width">Antal rutor i bredd</param>
        /// <param name="height">Antal rutor i höjd</param>
        protected void add(SpriteName name, int index, int width, int height)
        {
            currentIndex = index + width;
            int ypos = tileIxYpos(ref index);
            DataLib.SpriteCollection.Set(name, new Sprite(new Microsoft.Xna.Framework.Rectangle(
                index * TileSize, ypos * TileSize,
                width * TileSize, height * TileSize), TileSheetIx));
        }

        protected void addWithSizeDef(SpriteName name, int index, int xposAdd, int yposAdd, int pixWidth, int pixHeight)
        {
            int squaresW = (int)Math.Ceiling((double)pixWidth / TileSize);

            currentIndex = index + squaresW;
            int ypos = tileIxYpos(ref index);
            DataLib.SpriteCollection.Set(name, new Sprite(new Microsoft.Xna.Framework.Rectangle(
                index * TileSize + xposAdd, ypos * TileSize + yposAdd,
                pixWidth, pixHeight), TileSheetIx));
        }

        protected void addWithSizeDef(SpriteName name, int index, int pixWidth, int pixHeight)
        {
            addWithSizeDef(name, index, 0, 0, pixWidth, pixHeight);
        }

        protected void addTexture(SpriteName name, int width, int height, LoadedTexture texture)
        {
            Sprite file = new Sprite();
            file.textureIndex = (int)texture;
            file.Source.Width = width;
            file.Source.Height = height;
            file.UpdateSourcePolygon(false);
            DataLib.SpriteCollection.Set(name, file);
        }

        void addTexture(SpriteName name, Sprite file)
        {
            DataLib.SpriteCollection.Set(name, file);
        }

        protected int tileIxYpos(ref int tileIndex)
        {
            int ypos = (int)(tileIndex / numTilesWidth);
            tileIndex -= ypos * numTilesWidth;
            return ypos;
        }
        
        protected void AddWhiteArea()
        {
            const int WhiteAreaSize = 256;
            addTexture(SpriteName.WhiteArea, WhiteAreaSize, WhiteAreaSize, LoadedTexture.WhiteArea);
        }
    }
}
