using Microsoft.Xna.Framework;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.Core.DSSWars.Map.MapProcess.CityStructure
{
    

    struct TerrainStructure
    {
        public static readonly ItemResourceType[] AllTerrainResources = {
        ItemResourceType.Wood_G,
        ItemResourceType.Stone_G,
        ItemResourceType.Clay,
        ItemResourceType.Brick,
        ItemResourceType.Salt,
        ItemResourceType.Coal,
                
        ItemResourceType.BogIron,
        ItemResourceType.IronOre_G,
        ItemResourceType.TinOre,
        ItemResourceType.CopperOre,
        ItemResourceType.LeadOre,
        ItemResourceType.SilverOre,
        ItemResourceType.GoldOre,
        ItemResourceType.RawMithril,
        
        ItemResourceType.Sulfur,

        ItemResourceType.Fowl,
        ItemResourceType.Boar,
        ItemResourceType.Dog,
        ItemResourceType.Oxen,
        ItemResourceType.Pony,
        ItemResourceType.Wolf,
        ItemResourceType.WildCat,
        ItemResourceType.Elephant,
    };

        public int mineCount_waterSpring;
        public int mineCount_bogIron;

        public int mineCount_iron;
        public int mineCount_tin;
        public int mineCount_copper;
        public int mineCount_lead;
        public int mineCount_silver;
        public int mineCount_gold;
        public int mineCount_mithril;
        public int mineCount_sulfur;
        public int mineCount_coal;

        public int mineCount_salt;

        public int resourceCount_stone;
        public int resourceCount_wood;

        public int mineCount_stoneblock;
        public int resourceCount_clay;

        public int wildAnimalCount_Fowl;
        public int wildAnimalCount_Boar;
        public int wildAnimalCount_Dog;
        public int wildAnimalCount_Ox;

        public int wildAnimalCount_Pony;
        public int wildAnimalCount_Wolf;
        public int wildAnimalCount_Cat;
        public int wildAnimalCount_Elephant;


        static readonly MapTile1_1 TerrainType_wood = new MapTile1_1(TerrainMainType.Foil, (int)TerrainSubFoilType.TreeSoft);
        static readonly MapTile1_1 TerrainType_stone = new MapTile1_1(TerrainMainType.Foil, (int)TerrainSubFoilType.SmallStones);
        static readonly MapTile1_1 TerrainType_stoneblock = new MapTile1_1(TerrainMainType.Mine, (int)TerrainMineType.StoneBlock); //New

        static readonly MapTile1_1 TerrainType_waterspring = new MapTile1_1(TerrainMainType.Mine, (int)TerrainMineType.WaterNaturalSpring); //New
        static readonly MapTile1_1 TerrainType_clay = new MapTile1_1(TerrainMainType.Mine, (int)TerrainMineType.ClayPit); //New
        static readonly MapTile1_1 TerrainType_bogiron = new MapTile1_1(TerrainMainType.Mine, (int)TerrainMineType.BogIron);
        static readonly MapTile1_1 TerrainType_iron = new MapTile1_1(TerrainMainType.Mine, (int)TerrainMineType.IronOre);
        static readonly MapTile1_1 TerrainType_tin = new MapTile1_1(TerrainMainType.Mine, (int)TerrainMineType.TinOre);
        static readonly MapTile1_1 TerrainType_copper = new MapTile1_1(TerrainMainType.Mine, (int)TerrainMineType.CopperOre);
        static readonly MapTile1_1 TerrainType_lead = new MapTile1_1(TerrainMainType.Mine, (int)TerrainMineType.LeadOre);
        static readonly MapTile1_1 TerrainType_silver = new MapTile1_1(TerrainMainType.Mine, (int)TerrainMineType.SilverOre);
        static readonly MapTile1_1 TerrainType_gold = new MapTile1_1(TerrainMainType.Mine, (int)TerrainMineType.GoldOre);
        static readonly MapTile1_1 TerrainType_mithril = new MapTile1_1(TerrainMainType.Mine, (int)TerrainMineType.Mithril);

        static readonly MapTile1_1 TerrainType_salt = new MapTile1_1(TerrainMainType.Mine, (int)TerrainMineType.Salt); //New
        static readonly MapTile1_1 TerrainType_sulfur = new MapTile1_1(TerrainMainType.Mine, (int)TerrainMineType.Sulfur);
        static readonly MapTile1_1 TerrainType_coal = new MapTile1_1(TerrainMainType.Mine, (int)TerrainMineType.Coal);

        static readonly MapTile1_1 TerrainType_fowlHabitat = new MapTile1_1(TerrainMainType.Building, (int)TerrainSubAnimalPenType.FowlHabitat);
        static readonly MapTile1_1 TerrainType_boarHabitat = new MapTile1_1(TerrainMainType.Building, (int)TerrainSubAnimalPenType.BoarHabitat);
        static readonly MapTile1_1 TerrainType_dogHabitat = new MapTile1_1(TerrainMainType.Building, (int)TerrainSubAnimalPenType.DogHabitat);
        static readonly MapTile1_1 TerrainType_oxHabitat = new MapTile1_1(TerrainMainType.Building, (int)TerrainSubAnimalPenType.OxHabitat);
        static readonly MapTile1_1 TerrainType_ponyHabitat = new MapTile1_1(TerrainMainType.Building, (int)TerrainSubAnimalPenType.PonyHabitat);
        static readonly MapTile1_1 TerrainType_wolfHabitat = new MapTile1_1(TerrainMainType.Building, (int)TerrainSubAnimalPenType.WolfHabitat);
        static readonly MapTile1_1 TerrainType_catHabitat = new MapTile1_1(TerrainMainType.Building, (int)TerrainSubAnimalPenType.CatHabitat);
        static readonly MapTile1_1 TerrainType_elephantHabitat = new MapTile1_1(TerrainMainType.Building, (int)TerrainSubAnimalPenType.ElephantHabitat);


        public bool HasIndependantResources()
        {
            return mineCount_bogIron + mineCount_bogIron >= 1 &&
                resourceCount_wood >= 3 &&
                resourceCount_stone >= 1;
        }

        public void miningOverviewHud(LocalPlayer player, RichBoxContent content)
        {
            content.newLine();

            content.Add(new RbImage(SpriteName.WarsWorkMine));
            content.space();

            int totalCount = 0;

            naturalResource(player, content, resourceCount_wood, ItemResourceType.Wood_G, TerrainType_wood, ref totalCount);
            naturalResource(player, content, resourceCount_stone, ItemResourceType.Stone_G, TerrainType_stone, ref totalCount);
            
            naturalResource(player, content, resourceCount_clay, ItemResourceType.Clay, TerrainType_clay, ref totalCount);

            animalHabitat(player, content, wildAnimalCount_Fowl, ItemResourceType.Fowl, TerrainType_fowlHabitat);
            animalHabitat(player, content, wildAnimalCount_Boar, ItemResourceType.Boar, TerrainType_boarHabitat);
            animalHabitat(player, content, wildAnimalCount_Dog, ItemResourceType.Dog, TerrainType_dogHabitat);
            animalHabitat(player, content, wildAnimalCount_Ox, ItemResourceType.Oxen, TerrainType_oxHabitat);
            animalHabitat(player, content, wildAnimalCount_Pony, ItemResourceType.Pony, TerrainType_ponyHabitat);
            animalHabitat(player, content, wildAnimalCount_Wolf, ItemResourceType.Wolf, TerrainType_wolfHabitat);
            animalHabitat(player, content, wildAnimalCount_Cat, ItemResourceType.WildCat, TerrainType_catHabitat);
            animalHabitat(player, content, wildAnimalCount_Elephant, ItemResourceType.Elephant, TerrainType_elephantHabitat);


            mine(player, content, mineCount_coal, ItemResourceType.Coal, TerrainType_coal, ref totalCount);
            mine(player, content, mineCount_bogIron, ItemResourceType.BogIron, TerrainType_bogiron, ref totalCount);
            mine(player, content, mineCount_iron, ItemResourceType.Iron_G, TerrainType_iron, ref totalCount);
            mine(player, content, mineCount_tin, ItemResourceType.Tin, TerrainType_tin, ref totalCount);
            mine(player, content, mineCount_copper, ItemResourceType.Copper, TerrainType_copper, ref totalCount);
            mine(player, content, mineCount_lead, ItemResourceType.Lead, TerrainType_lead, ref totalCount);
            mine(player, content, mineCount_silver, ItemResourceType.Silver, TerrainType_silver, ref totalCount);
            mine(player, content, mineCount_gold, ItemResourceType.Gold, TerrainType_gold, ref totalCount);
            mine(player, content, mineCount_mithril, ItemResourceType.Mithril, TerrainType_mithril, ref totalCount);

            // Added Salt to mines
            mine(player, content, mineCount_salt, ItemResourceType.Salt, TerrainType_salt, ref totalCount);
            mine(player, content, mineCount_sulfur, ItemResourceType.Sulfur, TerrainType_sulfur, ref totalCount);
            mine(player, content, mineCount_stoneblock, ItemResourceType.Brick, TerrainType_stoneblock, ref totalCount);

            if (totalCount == 0)
            {
                content.Add(new RbText(DssRef.lang.Hud_EmptyList));
            }

        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)mineCount_bogIron);
            w.Write((byte)mineCount_iron);
            w.Write((byte)mineCount_tin);
            w.Write((byte)mineCount_copper);
            w.Write((byte)mineCount_lead);
            w.Write((byte)mineCount_silver);
            w.Write((byte)mineCount_gold);
            w.Write((byte)mineCount_mithril);
            w.Write((byte)mineCount_sulfur);
            w.Write((byte)mineCount_coal);
            w.Write((byte)mineCount_salt);
    
            w.Write(resourceCount_stone);
            w.Write(resourceCount_wood);
            w.Write(mineCount_stoneblock);
            w.Write((byte)resourceCount_clay);
    
            w.Write((byte)wildAnimalCount_Fowl);
            w.Write((byte)wildAnimalCount_Boar);
            w.Write((byte)wildAnimalCount_Dog);
            w.Write((byte)wildAnimalCount_Ox);
            w.Write((byte)wildAnimalCount_Pony);
            w.Write((byte)wildAnimalCount_Wolf);
            w.Write((byte)wildAnimalCount_Cat);
            w.Write((byte)wildAnimalCount_Elephant);
            Debug.WriteCheck(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            mineCount_bogIron = r.ReadByte();
            mineCount_iron = r.ReadByte();
            mineCount_tin = r.ReadByte();
            mineCount_copper = r.ReadByte();
            mineCount_lead = r.ReadByte();
            mineCount_silver = r.ReadByte();
            mineCount_gold = r.ReadByte();
            mineCount_mithril = r.ReadByte();
            mineCount_sulfur = r.ReadByte();
            mineCount_coal = r.ReadByte();
            mineCount_salt = r.ReadByte();
    
            resourceCount_stone = r.ReadInt32();
            resourceCount_wood = r.ReadInt32();
            mineCount_stoneblock = r.ReadInt32();
            resourceCount_clay = r.ReadByte();
    
            wildAnimalCount_Fowl = r.ReadByte();
            wildAnimalCount_Boar = r.ReadByte();
            wildAnimalCount_Dog = r.ReadByte();
            wildAnimalCount_Ox = r.ReadByte();
            wildAnimalCount_Pony = r.ReadByte();
            wildAnimalCount_Wolf = r.ReadByte();
            wildAnimalCount_Cat = r.ReadByte();
            wildAnimalCount_Elephant = r.ReadByte();
            Debug.ReadCheck(r);
        }

        public int Get(ItemResourceType type)
        {
            switch (type)
            {
                case ItemResourceType.BogIron:
                    return mineCount_bogIron;
                case ItemResourceType.IronOre_G:
                    return mineCount_iron;
                case ItemResourceType.TinOre:
                    return mineCount_tin;
                case ItemResourceType.CopperOre:
                    return mineCount_copper;
                case ItemResourceType.LeadOre:
                    return mineCount_lead;
                case ItemResourceType.SilverOre:
                    return mineCount_silver;
                case ItemResourceType.GoldOre:
                    return mineCount_gold;
                case ItemResourceType.RawMithril:
                    return mineCount_mithril;
                case ItemResourceType.Sulfur:
                    return mineCount_sulfur;
                case ItemResourceType.Coal:
                    return mineCount_coal;
                case ItemResourceType.Stone_G:
                    return resourceCount_stone;
                case ItemResourceType.Wood_G:
                    return resourceCount_wood;

                case ItemResourceType.Salt:
                    return mineCount_salt;
                case ItemResourceType.Clay:
                    return resourceCount_clay;
                case ItemResourceType.Brick:
                    return mineCount_stoneblock;


                case ItemResourceType.Fowl: return wildAnimalCount_Fowl;
                case ItemResourceType.Boar: return wildAnimalCount_Boar;
                case ItemResourceType.Dog: return wildAnimalCount_Dog;
                case ItemResourceType.Oxen: return wildAnimalCount_Ox;
                case ItemResourceType.Pony: return wildAnimalCount_Pony;
                case ItemResourceType.Wolf: return wildAnimalCount_Wolf;
                case ItemResourceType.WildCat: return wildAnimalCount_Cat;
                case ItemResourceType.Elephant: return wildAnimalCount_Elephant;

                default:
                    return 0;
            }
        }



        public void naturalResource(LocalPlayer player, RichBoxContent content, int count, ItemResourceType resource, MapTile1_1 terrainType, ref int totalCount)
        {
            resourceHoverButton(player, content, count, resource, DssRef.lang.Work_GatherXResource, SpriteName.WarsWorkCollect, terrainType, false, ref totalCount);
        }

        public void animalHabitat(LocalPlayer player, RichBoxContent content, int count, ItemResourceType resource, MapTile1_1 terrainType)
        {
            if (count > 0)
            {
                IconName.Item(resource, out SpriteName icon, out string resourceName);

                var infoContent = new RichBoxContent();

                infoContent.Add(new RbImage(icon));
                infoContent.space();
                var countText = new RbText(count.ToString());
                countText.overrideColor = Color.White;
                infoContent.Add(countText);

                var infoButton = new ArtButton(RbButtonStyle.Outline, infoContent, new RbAction1Arg<MapTile1_1>(player.gameControls.map.terrainSearchClick, terrainType),
                    new RbTooltip((RichBoxContent content, object tag) =>
                    {
                        content.Add(new RbImage(icon));
                        content.space();
                        var habitatString = string.Format(DssRef.lang.Terrain_XAnimalHabitat, resourceName);
                        content.Add(new RbText(TextLib.LargeFirstLetter(string.Format(DssRef.lang.Language_XCountIsY, habitatString, count))));

                    }));

                content.Add(infoButton);
            }
        }

        public void mine(LocalPlayer player, RichBoxContent content, int count, ItemResourceType resource, MapTile1_1 terrainType, ref int totalCount)
        {
            resourceHoverButton(player, content, count, resource, DssRef.lang.BuildingType_ResourceMine, SpriteName.WarsWorkMine, terrainType, terrainType.mainTerrain != TerrainMainType.NUM, ref totalCount);
        }

        public void resourceHoverButton(LocalPlayer player, RichBoxContent content, int count, ItemResourceType resource, string categoryName, SpriteName workIcon, MapTile1_1 terrainType, bool clickable, ref int totalCount)
        {
            totalCount += count;
            if (count > 0)
            {
                IconName.Item(resource, out SpriteName icon, out string resourceName);
                
                var infoContent = new RichBoxContent();

                infoContent.Add(new RbOverlapImage(new RbImage(icon), workIcon, VectorExt.V2FromX(-0.2f), 0.8f));
                infoContent.space();
                var countText = new RbText(count.ToString());
                countText.overrideColor = Color.White;
                infoContent.Add(countText);

                var infoButton = new ArtButton(clickable? RbButtonStyle.Outline : RbButtonStyle.HoverArea, infoContent, clickable? new RbAction1Arg<MapTile1_1>( player.gameControls.map.terrainSearchClick, terrainType) : null,
                    new RbTooltip((RichBoxContent content, object tag) =>
                    {
                        content.Add(new RbOverlapImage(new RbImage(icon), workIcon, Vector2.Zero, 0.8f));
                        content.space();
                        var mineString = string.Format(categoryName, resourceName);
                        content.Add(new RbText(TextLib.LargeFirstLetter(string.Format(DssRef.lang.Language_XCountIsY, mineString, count))));

                    }));

                content.Add(infoButton);
            }
        }
    }

}
