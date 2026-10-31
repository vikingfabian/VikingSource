using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Net.Security;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Map.MapModels
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
        protected const int TilesPerPixelScale = 4;
        protected const float OneTilePercentage = 1f / (TilesPerPixelScale * TilesPerPixelScale);
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
            var previousTexture = texture;
            texture = new Graphics.PixelTexture(PixelTextureScale());
            Graphics.GpuResourceSync.Release(previousTexture);
        }

        public static IntVector2 PixelTextureScale()
        {
            return DssRef.world.Size / TilesPerPixelScale;
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
        public ItemResourceType resourceFilter = ItemResourceType.Wood_G;

        public bool overview = false;
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
                        case ItemResourceType.Wood_G:
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
            refreshArea(DssRef.world.maptileBounds);
            version++;
        }
        
        void refreshArea(Rectangle2 area)
        {

            //SumTile4_4 t;
            ForXYLoop loop = new ForXYLoop(DssRef.world.Size) { stepLength = TilesPerPixelScale };

            switch (overview? FactionMapFilter.Terrain : filter)
            {

                case FactionMapFilter.FactionCols:
                    while (loop.Next())
                    {
                        var tile = DssRef.world.GetCombinedTileAndLean(loop.Position, out int leanY);
                        texture.SetPixel(loop.Position / TilesPerPixelScale, TileColor.FactionAndTerrainColor(tile, leanY));
                    }

                    AddCityIcons();
                    break;

                case FactionMapFilter.Terrain:
                    while (loop.Next())
                    {
                        //midu
                        var tile = DssRef.world.GetCombinedTileAndLean(loop.Position, out int leanY);
                        Color color = TileColor.BorderAndTerrainColor(tile, leanY, out var groundColor);
                        if (groundColor)
                        {
                            addTerrainAndBuildingColor(loop.Position, ref color);
                        }
                        texture.SetPixel(loop.Position / TilesPerPixelScale, color);
                    }
                    break;
                case FactionMapFilter.Minimap:
                    Faction playerFaction = DssRef.state.localPlayers[playerIx].pfaction.GetFaction();
                    while (loop.Next())
                    {
                        var tile = DssRef.world.GetCombinedTileAndLean(loop.Position, out int leanY);
                        texture.SetPixel(loop.Position / TilesPerPixelScale, TileColor.MinimapColor(tile, leanY));
                    }
                    AddCityIcons();
                    break;
                case FactionMapFilter.PopulationHeatmap:
                case FactionMapFilter.StrengthHeatmap:
                case FactionMapFilter.ResourceHeatmap:
                    /*
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

                        //if (t.tileContent == TileContent.City)
                        //    color = t.cityColor();

                        if (t.heightLevel <= BiomHeightColor.LowerWaterHeight)
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
                    */
                    break;
            }

            texture.ApplyPixelsToTexture();
            
        }

        void AddCityIcons()
        {
            var citiesC = DssRef.world.cities.counter();
            while (citiesC.Next()) //foreach (var city in DssRef.world.cities)
            {
                TileColor.cityColor(citiesC.sel, out Color centerCol, out Color outline, out Color factionCol);
                IntVector2 center = (citiesC.sel.maptilePos + (TilesPerPixelScale / 2))/ TilesPerPixelScale;
                texture.SetPixel(center, centerCol);
                Rectangle2 cityArea = Rectangle2.FromCenterTileAndRadius(center, 1);
                ForXYEdgeLoop edgeLoop = new ForXYEdgeLoop(cityArea);
                while (edgeLoop.Next())
                {
                    texture.TrySetPixel(edgeLoop.Position, outline);
                }

                cityArea.AddRadius(1);
                edgeLoop = new ForXYEdgeLoop(cityArea);
                while (edgeLoop.Next())
                {
                    texture.TrySetPixel(edgeLoop.Position, factionCol);
                }

            }
        }

        void addTerrainAndBuildingColor(IntVector2 topLeftMapTile, ref Color groundcolor)
        {
            int building = 0;
            int wall = 0;
            float tree = 0;
            int road = 0;

            ForXYLoop loop = new ForXYLoop(new Rectangle2(topLeftMapTile, new IntVector2(TilesPerPixelScale)));
            while (loop.Next())
            {
                var tile = DssRef.world.subTileGrid.Get(loop.Position);
                switch (tile.mainTerrain)
                {
                    case MapLib.TerrainMainType.Building:
                        building++;
                        break;
                    case MapLib.TerrainMainType.Wall:
                        wall++;
                        break;
                    case MapLib.TerrainMainType.Road:
                        road++;
                        break;
                    case TerrainMainType.AnimalPen:
                    case TerrainMainType.Farm:
                        tree += 0.5f;
                        break;
                    case MapLib.TerrainMainType.Foil:
                        switch ((TerrainSubFoilType)tile.subTerrain)
                        {
                            case TerrainSubFoilType.TreeSoft:
                            case TerrainSubFoilType.TreeHard:
                                tree += 1;
                                break;
                            case TerrainSubFoilType.Bush:
                                tree += 0.5f;
                                break;
                         }
                        break;
                }
            }

            if (building > 2)
            {
                groundcolor = ColorExt.Mix(Color.DarkGray, groundcolor, building * OneTilePercentage);
            }
            if (wall > 2)
            {
                groundcolor = ColorExt.Mix(Color.LightPink, groundcolor, wall * OneTilePercentage);
            }
            if (road > 2)
            {
                groundcolor = ColorExt.Mix(Color.Yellow, groundcolor, road * OneTilePercentage);
            }
            if (tree > 2)
            {
                groundcolor = ColorExt.Mix(Color.DarkGreen, groundcolor, tree * OneTilePercentage * 0.5f);
            }


        }
    }
}
