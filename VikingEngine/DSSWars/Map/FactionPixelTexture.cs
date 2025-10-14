using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Map
{

    enum FactionMapFilter
    { 
        FactionCols,
        Terrain,
        Minimap,
        NUM
    }

    abstract class AbsMapPixelTexture
    {
        protected Faction playerFaction;
        public Graphics.PixelTexture texture;

        public AbsMapPixelTexture(Faction faction)
        {
            this.playerFaction = faction;
        }

        public void initTexture()
        {
            texture = new Graphics.PixelTexture(TextureScale());
        }

        virtual protected IntVector2 TextureScale()
        {
            return DssRef.world.Size;
        }
    }

    class FactionPixelTexture : AbsMapPixelTexture
    {
        public FactionMapFilter filter;
        public FactionPixelTexture(Faction faction, bool init, FactionMapFilter filter)
            : base(faction)
        {
            this.filter = filter;
            if (init)
            {
                initTexture();
                refreshWorld();
            }
        }

        public void refreshWorld()
        {
            refreshArea(DssRef.world.tileBounds);
        }
        
        void refreshArea(Rectangle2 area)
        {
            Tile t;

            ForXYLoop loop = new ForXYLoop(area);

            switch (filter)
            {

                case FactionMapFilter.FactionCols:
                    while (loop.Next())
                    {
                        t = DssRef.world.tileGrid.Get(loop.Position);
                        texture.SetPixel(loop.Position, t.MinimapColor_Faction(loop.Position));
                    }
                    break;

                case FactionMapFilter.Terrain:
                    while (loop.Next())
                    {
                        t = DssRef.world.tileGrid.Get(loop.Position);
                        texture.SetPixel(loop.Position, t.MinimapColor_Terrain(loop.Position));
                    }
                    break;
                case FactionMapFilter.Minimap:
                    while (loop.Next())
                    {
                        t = DssRef.world.tileGrid.Get(loop.Position);
                        texture.SetPixel(loop.Position, t.MinimapColor_Minimap(playerFaction, loop.Position));
                    }
                    break;
            }

            texture.ApplyPixelsToTexture();
        }

        //public void SetNewTexture()
        //{
        //    texture.ApplyPixelsToTexture();
        //}

    }
}
