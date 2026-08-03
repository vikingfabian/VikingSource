using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Map;
using VikingEngine.ToGG.HeroQuest;

namespace VikingEngine.DSSWars.Map
{
    struct Tile
    {
        public const int NoBorderRegion = -2;
        public const int SeaBorder = -1;
        const int CompareToAmountCities = 8;

        public static void Init()
        {
            TypeToHeight_aboveWater = new float[TypeToHeight.Length];
            for (int i = 0; i < TypeToHeight.Length; i++)
            {
                TypeToHeight_aboveWater[i] = Math.Max(TypeToHeight[i], 0);

                if (i >= Height.MountainHeightStart)
                { 
                    TypeToHeight_aboveWater[i] += 0.2f;
                }
            }

            TypeToWalkingMultiplier = new float[TypeToWalkingDistance.Length];
            TypeToShipTravelMultiplier = new float[TypeToWalkingDistance.Length];
            for (int i = 0; i < TypeToWalkingDistance.Length; ++i)
            {
                TypeToWalkingMultiplier[i] = 1f / TypeToWalkingDistance[i];
                TypeToShipTravelMultiplier[i] = 1f / TypeToShipDistance[i];
            }
        }

        //Save data
        public int CityIndex;
        public BiomType biom =  BiomType.Green;
        public float secondaryBiomStrength = 0;
        public BiomType secondaryBiom = BiomType.Green;
        public int heightLevel;
        public TileContent tileContent = TileContent.NONE;
        public int BorderCount;
        public int BorderRegion_North, BorderRegion_East, BorderRegion_South, BorderRegion_West;
        public int seaDistanceHeatMap = int.MinValue;
        //public int prevFoliageCount = 32;
        //--

        //public int WorkerCount = 0;
        
        public float exitRenderTimeStamp_TotSec = 0;
        //public byte renderStateA = Culling.NoRender;
        //public byte renderStateB = Culling.NoRender;
        public byte bits_renderStateA = Culling.NoRender;
        public byte bits_renderStateB = Culling.NoRender;
        public bool hasTileInRender = false;

        public int subtileVisualEdits = 0;
        //public bool inRender = false;

        public bool OutOfRenderTimeOut()
        { 
            return (Ref.TotalGameTimeSec - exitRenderTimeStamp_TotSec) > 1f;
        }

        public Tile()
        {
            heightLevel = Height.DeepWaterHeight;

            clearCityData();
        }

        public void clearCityData()
        { 
             CityIndex = -1;
            tileContent = TileContent.NONE;
            BorderCount = 0;
            BorderRegion_North = NoBorderRegion; 
            BorderRegion_East = NoBorderRegion; 
            BorderRegion_South = NoBorderRegion; 
            BorderRegion_West = NoBorderRegion;
            seaDistanceHeatMap = int.MinValue;
        }

        public Tile(System.IO.BinaryReader r, Tile previous, int version)
            :this()
        {
            readMapFile(r, previous, version);
        }

        public void setWaterHeat_Land(int heat)
        {
            //int heat = neighborHeat + (diagonal ? 12 : 10);
            if (seaDistanceHeatMap == int.MinValue)
            {
                seaDistanceHeatMap = heat;
            }
            else
            {
                if (heat < seaDistanceHeatMap)
                {
                    seaDistanceHeatMap = heat;
                }
            }
        }

        public void setWaterHeat_Water(int heat)
        {
            if (seaDistanceHeatMap == int.MinValue)
            {
                seaDistanceHeatMap = heat;
            }
            else
            {
                if (heat > seaDistanceHeatMap)
                {
                    seaDistanceHeatMap = heat;
                }
            }
        }


        const int SaveOpt_IsCity_Ix = 0;
        const int SaveOpt_CityIndex_Ix = 1;
        const int SaveOpt_biom_Ix = 2;
        const int SaveOpt_heightLevel_Ix = 3;
        const int SaveOpt_HasBorderN_Ix = 4;
        const int SaveOpt_HasBorderE_Ix = 5;
        const int SaveOpt_HasBorderS_Ix = 6;
        const int SaveOpt_HasBorderW_Ix = 7;


        public void writeMapFile(System.IO.BinaryWriter w, Tile previuos)
        {
            EightBit saveOpt = new EightBit();
            bool bIsCity = tileContent == TileContent.City;
            bool eqCityIndex = CityIndex == previuos.CityIndex;
            bool eqBiom = biom == previuos.biom;
            bool eqHeight = heightLevel == previuos.heightLevel;

            bool bBorderN = BorderRegion_North != NoBorderRegion;
            bool bBorderE = BorderRegion_East != NoBorderRegion;
            bool bBorderS = BorderRegion_South != NoBorderRegion;
            bool bBorderW = BorderRegion_West != NoBorderRegion;

            saveOpt.Set(SaveOpt_IsCity_Ix, bIsCity);
            saveOpt.Set(SaveOpt_CityIndex_Ix, eqCityIndex);
            saveOpt.Set(SaveOpt_biom_Ix, eqBiom);
            saveOpt.Set(SaveOpt_heightLevel_Ix, eqHeight);
            saveOpt.Set(SaveOpt_HasBorderN_Ix, bBorderN);
            saveOpt.Set(SaveOpt_HasBorderE_Ix, bBorderE);
            saveOpt.Set(SaveOpt_HasBorderS_Ix, bBorderS);
            saveOpt.Set(SaveOpt_HasBorderW_Ix, bBorderW);

            saveOpt.write(w);

            //w.Write(Debug.Byte_OrCrash((int)tileContent));
            if (!eqCityIndex)
            {
                w.Write(Debug.Ushort_OrCrash(CityIndex));
            }

            if (!eqBiom)
            {
                w.Write(Debug.Byte_OrCrash((byte)biom));
            }

            if (!eqHeight)
            {
                w.Write(Debug.Byte_OrCrash(heightLevel));
            }


            //w.Write(Debug.Ushort_OrCrash(BorderCount));
            if (bBorderN)
            {
                w.Write(Debug.Short_OrCrash(BorderRegion_North));
            }
            if (bBorderE)
            {
                w.Write(Debug.Short_OrCrash(BorderRegion_East));
            }
            if (bBorderS)
            {
                w.Write(Debug.Short_OrCrash(BorderRegion_South));
            }
            if (bBorderW)
            {
                w.Write(Debug.Short_OrCrash(BorderRegion_West));
            }
        }

        public void readMapFile(System.IO.BinaryReader r, Tile previuos, int version)
        {
            //TODO optimera med att spara bool för repeat av vanliga värden

            //tileContent = (TileContent)r.ReadByte();

            EightBit saveOpt = new EightBit(r);

            if (saveOpt.Get(SaveOpt_IsCity_Ix))
            {
                tileContent = TileContent.City;
            }

            if (saveOpt.Get(SaveOpt_CityIndex_Ix))
            {
                CityIndex = previuos.CityIndex;
            }
            else
            {
                CityIndex = r.ReadUInt16();
            }

            if (saveOpt.Get(SaveOpt_biom_Ix))
            {
                biom = previuos.biom;
            }
            else
            {
                biom = (BiomType)r.ReadByte();
            }

            if (saveOpt.Get(SaveOpt_heightLevel_Ix))
            {
                heightLevel = previuos.heightLevel;
            }
            else
            {
                heightLevel = r.ReadByte();
            }

            //BorderCount = r.ReadUInt16();

            //TODO optimera med att spara bytebools för icke NO, och tilecontent == city
            //Spara inte border count
            BorderCount = 0;
            if (saveOpt.Get(SaveOpt_HasBorderN_Ix))
            {
                ++BorderCount;
                BorderRegion_North = r.ReadInt16();
            }
            else
            { 
                BorderRegion_North = NoBorderRegion;
            }

            if (saveOpt.Get(SaveOpt_HasBorderE_Ix))
            {
                ++BorderCount;
                BorderRegion_East = r.ReadInt16();
            }
            else
            {
                BorderRegion_East = NoBorderRegion;
            }

            if (saveOpt.Get(SaveOpt_HasBorderS_Ix))
            {
                ++BorderCount;
                BorderRegion_South = r.ReadInt16();
            }
            else
            {
                BorderRegion_South = NoBorderRegion;
            }

            if (saveOpt.Get(SaveOpt_HasBorderW_Ix))
            {
                ++BorderCount;
                BorderRegion_West = r.ReadInt16();
            }
            else
            {
                BorderRegion_West = NoBorderRegion;
            }

        }

        public void AddBorder(int dir, int toregion)
        {
            ++BorderCount;
            
                switch (dir)
                {
                    case 0:
                        BorderRegion_North = toregion;
                        break;
                    case 1:
                        BorderRegion_East = toregion;
                        break;
                    case 2:
                        BorderRegion_South = toregion;
                        break;
                    case 3:
                        BorderRegion_West = toregion;
                        break;
                }
            
        }

        public bool hasBorder(out bool sameFaction)
        {
            if (BorderCount > 0)
            {
                PFaction owner = DssRef.world.cities[CityIndex].pfaction;
                if (BorderRegion_North >= 0 && DssRef.world.cities[BorderRegion_North].pfaction != owner)
                {
                    sameFaction = false;
                    return true;
                }
                if (BorderRegion_East >= 0 && DssRef.world.cities[BorderRegion_East].pfaction != owner)
                {
                    sameFaction = false;
                    return true;
                }
                if (BorderRegion_South >= 0 && DssRef.world.cities[BorderRegion_South].pfaction != owner)
                {
                    sameFaction = false;
                    return true;
                }
                if (BorderRegion_West >= 0 && DssRef.world.cities[BorderRegion_West].pfaction != owner)
                {
                    sameFaction = false;
                    return true;
                }

                sameFaction = true;
                return true;
            }

            sameFaction = false;
            return false;
        }

        public int GetBorder(int dir)
        {
            switch (dir)
            {
                default:
                    return BorderRegion_North;
                case 1:
                    return BorderRegion_East;
                case 2:
                    return BorderRegion_South;
                case 3:
                    return BorderRegion_West;
            }
        }

        public static void FindOwnerInit(out StaticList<KeyValuePair<float, City>> closeCities)
        {
            closeCities = new StaticList<KeyValuePair<float, City>>(CompareToAmountCities);
        }

        public City City()
        {
            if (CityIndex < 0)
            {
                return null;
            }
            return DssRef.world.cities[CityIndex]; 
        }

        public Faction Faction()
        {
           return DssRef.world.cities[CityIndex].pfaction.GetFaction();
        }
        //public Faction Faction_Safe()
        //{
        //    return DssRef.world.cities[CityIndex].GetFaction_Safe();
        //}

        public Color FactionColor()
        {
            var c = DssRef.world.cities[CityIndex];
            var p = c.pfaction.GetPlayer();
            if (p != null && p.profile.flag != null)
            {
                return p.profile.flag.col0_Main;
            }
            else
            {
                return Color.Gray;
            }
        }

        static readonly Color MapCol_HeadCity = new Color(255,174,184);
        static readonly Color MapCol_LargeCity = new Color(253,0,30);
        static readonly Color MapCol_SmallCity = new Color(148, 0, 17);
        static readonly Color MapCol_CampsiteCity = new Color(148, 0, 17);
        static readonly Color MapCol_UnclaimedCity = Color.Blue;

        static readonly Color MiniMapCol_HeadCity = new Color(251, 37, 114);
        static readonly Color MiniMapCol_LargeCity = new Color(226, 11, 88);
        static readonly Color MiniMapCol_SmallCity = new Color(194, 4, 72);
        static readonly Color MiniMapCol_CampsiteCity = new Color(148, 0, 17);
        static readonly Color MiniMapCol_UnclaimedCity = Color.Blue;

        public bool HasBorderImage() { return BorderCount > 0; }


        public Color MinimapColor_Faction(IntVector2 pos)
        {
            
            if (tileContent == TileContent.City)
                return cityColor();

            if (heightLevel <= Height.LowerWaterHeight)
            {
                foreach (var dir in IntVector2.Dir4Array)
                {
                    if (DssRef.world.tileGrid.TryGet(pos + dir, out var nTile))
                    {
                        if (nTile.heightLevel > Height.LowerWaterHeight)
                        {
                            return lib.IsEven(pos.X + pos.Y) ?
                                WorldData.WaterDarkCol1 : WorldData.WaterDarkCol2;
                        }
                    }
                }

                return lib.IsEven(pos.X + pos.Y) ?
                    WorldData.WaterVeryDarkCol1 : WorldData.WaterVeryDarkCol2;
            }
            else if (heightLevel == Height.LowWaterHeight)
            {
                return lib.IsEven(pos.X + pos.Y) ? WorldData.WaterEdgeColorBright : WorldData.WaterEdgeColor;
            }
            else
            {
                return heightAndFactionCol(pos);
            }
        }

        public Color MinimapColor_Terrain(IntVector2 pos)
        {
            if (tileContent == TileContent.City)
                return cityColor();

            if (heightLevel <= Height.LowWaterHeight)
            {
                return lib.IsEven(pos.X + pos.Y) ?
                    WorldData.WaterDarkCol : WorldData.WaterDarkCol2;
            }
            else
            {
                var col = DssRef.map.bioms.bioms[(int)biom].TileColor(this).Color;
                if (secondaryBiomStrength > 0)
                {
                    var col2 = DssRef.map.bioms.bioms[(int)secondaryBiom].TileColor(this).Color;
                    col = ColorExt.Mix(col2, col, secondaryBiomStrength * 0.25f);
                }

                if (hasBorder(out bool sameFaction))
                {
                    col = ColorExt.ChangeBrighness(col, sameFaction ? 20 : -50);
                }   

                return col;
            }
        }

        public Color MinimapColor_Minimap(Faction playerFaction, IntVector2 pos)
        {
            if (tileContent == TileContent.City)
            {
                if (City().cityType == CityType.UnClaimed)
                {
                    lib.DoNothing();
                }
                return cityColor_Minimap();
            }
           
            if (heightLevel <= Height.LowWaterHeight)
            { 
                return WorldData.WaterDarkCol2;
            }
            else
            {
                return heightAndMinimapCol(playerFaction, pos);
            }
        }

        public Color BiomColor()
        {
            var col = DssRef.map.bioms.bioms[(int)biom].TileColor(this).Color;
            if (secondaryBiomStrength > 0)
            {
                var col2 = DssRef.map.bioms.bioms[(int)secondaryBiom].TileColor(this).Color;
                return ColorExt.Mix(col2, col, secondaryBiomStrength * 0.25f);
            }
            return col;
        }

        public float GroundY() { return TypeToHeight[heightLevel]; }

        public float GroundY_aboveWater() { return TypeToHeight_aboveWater[heightLevel]; }

        const float ModelGroundYAdj = 0.06f;

        public float ModelGroundY() { return TypeToHeight[heightLevel] + ModelGroundYAdj; }


        public float UnitGroundY()
        {
            float result = TypeToHeight[heightLevel] + ModelGroundYAdj;
            if (result > WaterSurfaceY)
            {
                return result;
            }

            return WaterSurfaceY;
        }

        Color heightAndMinimapCol(Faction playerFaction, IntVector2 pos)
        {
            float brightness = 1f - ((int)heightLevel - 2) * 0.05f;

            City city = City();
            //if (city.IsNetHosted)
            //{
            //    lib.DoNothing();
            //}
            //int faction = city.factionIndex;
            float red = 0;
            float green = 0;

            if (city.pfaction.IsEmpty())
            {
                return ColorExt.VeryDarkGray;
            }

            if (city.pfaction == playerFaction.pfaction)
            {
                brightness *= 0.5f;
            }
            else
            {
               var rel = DssRef.world.diplomacy.GetRelation(playerFaction.pfaction, city.pfaction).Relation;

                if (rel <= RelationType.RelationTypeN2_Truce)
                {
                    red = 0.2f;
                }
                else if (rel >= RelationType.RelationType3_Ally)
                {
                    green = 0.2f;
                }

                brightness *= 0.2f;
            }

            int distance = city.tilePos.SideLength(pos);
            
            if (distance == 1)
            {
                brightness *= 1.5f;
            }
            else if (hasBorder(out bool sameFaction))
            {
                if (sameFaction)
                {
                    brightness *= 1.25f;
                }
                else
                {
                    brightness *= 0.6f;
                }
            }

            return new Color(brightness + red, brightness + green, brightness);
        }


        Color heightAndFactionCol(IntVector2 pos)
        {   
            float brightness = 1f - ((int)heightLevel - 2) * 0.05f;

            City city = City();
            //int faction = city.factionIndex;

            Color factionCol;
            //if (faction < 0 || faction >= DssRef.world.factions.Array.Length)
            //{
            //    factionCol = Color.Gray;
            //}
            if (city.pfaction.TryGetFaction(out var faction))
            {
                factionCol = faction.Color();
            }
            else
            { 
                factionCol = Color.Gray;
            }
            int distance = city.tilePos.SideLength(pos);
            
            if (distance == 1)
            {
                brightness = 1.15f;
            }
            else if (hasBorder(out bool sameFaction))
            {
                if (faction == null)
                {
                    factionCol = Color.LightGray;
                }
                else if (ColorExt.GetBrightNess(factionCol) > 0.3f)
                {
                    if (sameFaction)
                    {
                        brightness -= 0.1f;
                    }
                    else
                    {
                        brightness -= 0.3f;
                    }
                }
                else
                {
                    if (sameFaction)
                    {
                        factionCol = ColorExt.ChangeBrighness(factionCol, 5);
                        brightness += 0.05f;
                    }
                    else
                    {
                        factionCol = ColorExt.ChangeBrighness(factionCol, 15);
                        brightness += 0.15f;
                    }
                
                }
            }

            Color color = Color.Multiply(factionCol, brightness);
            return color;
        }

        public Height heightSett()
        {
            return DssRef.map.heigts[heightLevel];
        }

        public Biom Biom()
        {
            return DssRef.map.bioms.bioms[(int)biom];
        }

        public Color cityColor()
        {
            switch (City().cityType)
            {
                default: return MapCol_HeadCity;
                case CityType.Town: return MapCol_LargeCity;
                case CityType.Village: return MapCol_SmallCity;
                case CityType.Campsite: return MapCol_CampsiteCity;
                case CityType.UnClaimed: return MapCol_UnclaimedCity;

            }
        }
        public Color cityColor_Minimap()
        {
            switch (City().cityType)
            {
                default: return MiniMapCol_HeadCity;
                case CityType.Town: return MiniMapCol_LargeCity;
                case CityType.Village: return MiniMapCol_SmallCity;
                case CityType.Campsite: return MiniMapCol_CampsiteCity;
                case CityType.UnClaimed: return MiniMapCol_UnclaimedCity;
            }
        }
        static float[] TypeToWalkingMultiplier;

        static readonly float[] TypeToWalkingDistance = new float[]
        {
            12,//Deep water
            6,//Deep water
            3,//Water_0,
            0.8f,//OpenField_1,
            1,//Plains_2,
            1.4f,//Vegetation_3,
            1.5f,//Hills_4,
            2.4f,//Mountain_5,
            4,//MountainRidge_6,
            8,
        };

        static float[] TypeToShipTravelMultiplier;

        static readonly float[] TypeToShipDistance = new float[]
        {
            0.8f,//Deep water
            0.8f,
            1f,//Water_0,
            4f,//OpenField_1,
            6,//Plains_2,
            6,//Vegetation_3,
            6,//Hills_4,
            6,//Mountain_5,
            6,
            6,//MountainRidge_6,
        };

        public const float WaterSurfaceY = -0.1f;
        public const float WaterFoamY = WaterSurfaceY + 0.01f;
        public const float UnitMinY = WaterSurfaceY; //+ 0.02f;
        public const float UnitQuadMinY = WaterSurfaceY + 0.07f;
        const float LayerHeight = 0.06f;

        static readonly float[] TypeToHeight = new float[]
        {
            WaterSurfaceY - 0.3f,//Deep water
            WaterSurfaceY - 0.18f,//Deep water
            WaterSurfaceY - 0.07f,//Water_0,
            0f,//OpenField_1,
            LayerHeight,//Plains_2,
            LayerHeight * 2f,//Vegetation_3,
            LayerHeight * 3f,//Hills_4,
            LayerHeight * 4.2f,//Mountain_5,
            LayerHeight * 5.4f,
            LayerHeight * 6.8f,//MountainRidge_6,
        };

        static float[] TypeToHeight_aboveWater;

        public float TroupWalkingDistance(bool ship)
        {
            if (ship) return TypeToShipDistance[(int)heightLevel];
            else return TypeToWalkingDistance[(int)heightLevel];
        }

        public float TerrainSpeedMultiplier(bool ship)
        {            
            if (ship) return TypeToShipTravelMultiplier[heightLevel];
            else return TypeToWalkingMultiplier[heightLevel];
        }

        public float TerrainSpeedMultiplier(out bool isLand)
        {
            isLand = IsLand();
            if (isLand) return TypeToWalkingMultiplier[heightLevel];
            else return TypeToShipTravelMultiplier[heightLevel];
        }

        public bool IsLand() { return heightLevel > Height.LowWaterHeight; }

        public bool MayBuild() { return heightLevel > Height.LowWaterHeight && heightLevel < Height.MountainLowPeak; }

        public bool IsWater() { return heightLevel <= Height.LowWaterHeight; }
        
        public override string ToString()
        {
            return (IsWater()? "water" : "land") + heightLevel.ToString() + " city:" + CityIndex.ToString();
        }
    }

    enum TileSpecialType
    {
        NON,
        AdjacantToCity,
        Border,
    }

    enum TileContent
    {
        NONE,
        City,
    }
}
