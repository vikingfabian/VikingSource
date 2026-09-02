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
        protected int playerIx;
        public Graphics.PixelTexture texture;

        public AbsMapPixelTexture(int playerIx)
        {
            this.playerIx = playerIx;
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
        public FactionPixelTexture(int playerIx, bool init, FactionMapFilter filter)
            : base(playerIx)
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
            content.h2(DssRef.lang.MapFilter, HudLib.TitleColor_Head);

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
                    //icon = ResourceLib.Icon(resourceFilter);
                    IconName.Item(resourceFilter, out icon, out _);
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
                    IconName.Item(res, out SpriteName itemIcon, out string itemName);

                    SpriteName collectIcon;
                    switch (res)
                    { 
                        case ItemResourceType.Wood_Group:
                        case ItemResourceType.Stone_G:
                        case ItemResourceType.BogIron:
                        case ItemResourceType.Clay:
                            collectIcon = SpriteName.WarsWorkCollect;
                            break;

                        default:
                            collectIcon = ItemPropertyColl.Get(res).storageType == StorageType.AnimalStorage ? SpriteName.WarsBuild_Trapper : SpriteName.WarsWorkMine;
                            break;

                    }

                    content.Add(new ArtOption(res == resourceFilter,
                        new List<AbsRichBoxMember> {
                            new RbImage(collectIcon),
                            new RbImage(itemIcon),
                            new RbSpace(0.5f),
                            new RbText(itemName),
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
                    Faction playerFaction = DssRef.state.localPlayers[playerIx].pfaction.GetFaction();
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
                            City city = t.City();
                            Faction faction = city.pfaction.GetFaction();

                            if (faction != null)
                            {
                                float value = 0;
                                switch (filter)
                                {
                                    case FactionMapFilter.PopulationHeatmap:
                                        value = faction.totalWorkForce;
                                        break;

                                    case FactionMapFilter.StrengthHeatmap:
                                        value = faction.militaryStrength;
                                        break;

                                    case FactionMapFilter.ResourceHeatmap:
                                        value = city.terrainStructure.Get(resourceFilter);
                                        break;
                                }

                                color = ColorExt.HeatColor_Inferno(value / max);
                                prevCity = t.CityIndex;
                                prevColor = color;
                            }
                            else
                            {
                                color = Color.Gray;
                            }
                        }

                        texture.SetPixel(loop.Position, color);
                    }                    
                    break;
            }

            texture.ApplyPixelsToTexture();
        }

    }
}
