using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VikingEngine.DSSWars.Map.Settings;
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
        PopulationHeatmap,
        StrengthHeatmap,

        NUM
    }

    abstract class AbsMapPixelTexture
    {
        int lastCheckVersion = 0;
        public int version = 0;
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

        public bool NewVersion()
        {
            if (version > lastCheckVersion)
            { 
                lastCheckVersion = version;
                return true;
            }
            return false;
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
            version++;
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
                case FactionMapFilter.PopulationHeatmap:
                case FactionMapFilter.StrengthHeatmap:

                    float max = 0;

                    var factionsC = DssRef.world.factions.counter();
                    switch (filter)
                    {
                        case FactionMapFilter.PopulationHeatmap:
                            while (factionsC.Next())
                            {
                                if (factionsC.sel.isAlive)
                                {
                                    max = Math.Max(max, factionsC.sel.totalWorkForce);
                                }
                            }
                            break;

                        case FactionMapFilter.StrengthHeatmap:
                            while (factionsC.Next())
                            {
                                if (factionsC.sel.isAlive)
                                {
                                    max = Math.Max(max, factionsC.sel.militaryStrength);
                                }
                            }
                            break;
                    }

                    int prevCity = -1;
                    Color prevColor = Color.Black;

                    while (loop.Next())
                    {
                        Color color;
                        t = DssRef.world.tileGrid.Get(loop.Position);

                        if (t.tileContent == TileContent.City)
                            color = t.cityColor();

                        if (t.heightLevel <= Height.LowerWaterHeight)
                        {
                            color = Color.CornflowerBlue;
                        }
                        else if (t.CityIndex == prevCity)
                        {
                            color = prevColor;
                        }
                        else
                        {
                            Faction faction = t.Faction();

                            float value = 0;
                            switch (filter)
                            {
                                case FactionMapFilter.PopulationHeatmap:
                                   value = faction.totalWorkForce;
                                   break;

                                case FactionMapFilter.StrengthHeatmap:
                                    value = faction.militaryStrength;
                                    break;
                            }

                            color = ColorExt.HeatColor_Inferno(value / max);
                            prevCity = t.CityIndex;
                            prevColor = color;
                        }

                        texture.SetPixel(loop.Position, color);
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
