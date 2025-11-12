using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
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
        ResourceHeatmap,

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

        float max = 1;
        public FactionMapFilter filter;
        public ItemResourceType resourceFilter = ItemResourceType.Wood_Group;
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

        public bool HeatMap()
        {
            return filter >= FactionMapFilter.PopulationHeatmap;
        }

        public void HeatMapInfoHud(RichBoxContent content)
        {
            content.h2(DssRef.todoLang.MapFilter, HudLib.TitleColor_Head);

            SpriteName icon = SpriteName.NO_IMAGE;
            string caption = null;
            switch (filter)
            {
                case FactionMapFilter.PopulationHeatmap:
                    icon = SpriteName.WarsWorker;
                    caption = DssRef.lang.ResourceType_Workers;
                    break;
                case FactionMapFilter.StrengthHeatmap:
                    icon = SpriteName.WarsStrengthIcon;
                    caption = string.Format(DssRef.lang.Hud_TotalStrengthRating, string.Empty);
                    break;
                case FactionMapFilter.ResourceHeatmap:
                    icon = ResourceLib.Icon(resourceFilter);
                    caption = DssRef.lang.Resource;
                    break;
            }

            const float Tab = 0.25f;
            float tabLength = 0;

            content.newLine();
            content.Add(new RbImage(icon));
            content.hspace();
            content.Add(new RbText(caption, HudLib.TitleColor_Label));

            content.newLine();
            content.Add(new RbImage(SpriteName.WhiteArea, 1f, ColorExt.HeatColor_Inferno(0)));
            content.hspace();
            content.Add(new RbText(": 0"));

            tabLength += Tab;
            content.Add(new RbTab(tabLength));

            if (max > 3)
            {                
                content.Add(new RbImage(SpriteName.WhiteArea, 1f, ColorExt.HeatColor_Inferno(1f / max)));
                content.hspace();
                content.Add(new RbText(": 1"));

                tabLength += Tab;
                content.Add(new RbTab(tabLength));
            }
            //else
            //{
            //    content.newLine();
            //}

            //content.newLine();
            content.Add(new RbImage(SpriteName.WhiteArea, 1f, ColorExt.HeatColor_Inferno(0.5f)));
            content.hspace();
            content.Add(new RbText(": " + TextLib.LargeNumber((int)(max * 0.5f))));

            //content.newLine();
            tabLength += Tab;
            content.Add(new RbTab(tabLength));
            content.Add(new RbImage(SpriteName.WhiteArea, 1f, ColorExt.HeatColor_Inferno(1f)));
            content.hspace();
            content.Add(new RbText(": " + TextLib.LargeNumber((int)max)));


            if (filter == FactionMapFilter.ResourceHeatmap)
            {
                content.newParagraph();
                foreach (var res in TerrainStructure.AllTerrainResources)
                {
                    content.Add(new ArtOption(res == resourceFilter,
                        new List<AbsRichBoxMember> {
                            new RbImage((res == ItemResourceType.Wood_Group || res == ItemResourceType.Stone_G)? SpriteName.WarsWorkCollect : SpriteName.WarsWorkMine),
                            new RbImage(ResourceLib.Icon(res)),
                            new RbSpace(0.5f),
                            new RbText(LangLib.Item(res)),
                        },
                        new RbAction1Arg<ItemResourceType>((ItemResourceType resource) => { 
                            resourceFilter = resource;
                            DssRef.world.BordersUpdated = true;
                        },
                        res)));

                    content.newLine();
                }
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
                case FactionMapFilter.ResourceHeatmap:

                    max = 0;                    
                    switch (filter)
                    {
                        case FactionMapFilter.PopulationHeatmap:
                            {
                                var factionsC = DssRef.world.factions.counter();
                                while (factionsC.Next())
                                {
                                    if (factionsC.sel.isAlive)
                                    {
                                        max = Math.Max(max, factionsC.sel.totalWorkForce);
                                    }
                                }
                            }
                            break;

                        case FactionMapFilter.StrengthHeatmap:
                            {
                                var factionsC = DssRef.world.factions.counter();
                                while (factionsC.Next())
                                {
                                    if (factionsC.sel.isAlive)
                                    {
                                        max = Math.Max(max, factionsC.sel.militaryStrength);
                                    }
                                }
                            }
                            break;

                        case FactionMapFilter.ResourceHeatmap:
                            {
                                foreach (var city in DssRef.world.cities)
                                {
                                    max = Math.Max(max, city.terrainStructure.Get(resourceFilter));
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
                        {
                            color = Color.DarkBlue;//t.cityColor();
                        }
                        else if (t.heightLevel <= Height.LowWaterHeight)
                        {
                            color = Color.CornflowerBlue;
                        }
                        else if (t.CityIndex == prevCity)
                        {
                            color = prevColor;
                            if (t.hasBorder(out _))
                            {
                                color = ColorExt.ChangeBrighness(color, -20);
                            }
                        }
                        else
                        {
                            City city = t.City();
                            Faction faction = city.GetFaction();

                            float value = 0;

                            switch (filter)
                            {
                                case FactionMapFilter.PopulationHeatmap:
                                    if (faction != null)
                                    {
                                        value = faction.totalWorkForce;
                                    }
                                    break;

                                case FactionMapFilter.StrengthHeatmap:
                                    if (faction != null)
                                    {
                                        value = faction.militaryStrength;
                                    }
                                    break;

                                case FactionMapFilter.ResourceHeatmap:
                                    value = city.terrainStructure.Get(resourceFilter);
                                    break;
                            }

                            color = ColorExt.HeatColor_Inferno(value / max);
                            prevColor = color;
                            if (t.hasBorder(out _))
                            {
                                color = ColorExt.ChangeBrighness(color, -20);
                            }
                            prevCity = t.CityIndex;
                            
                        }

                        texture.SetPixel(loop.Position, color);
                    }
                    break;
            }

            texture.ApplyPixelsToTexture();
        }

    }
}
