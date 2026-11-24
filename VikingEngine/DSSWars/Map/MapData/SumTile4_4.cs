using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Map;
using VikingEngine.ToGG.HeroQuest;

namespace VikingEngine.DSSWars.Map.MapData
{
    
    /// <summary>
    /// Group of 4 by 4 tiles
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct SumTile4_4
    {
        public static readonly SumTile4_4 Empty = new SumTile4_4();
        public const int TileWidth = 4;

        public const float ModelScale = MapTile1_1.ModelScale * TileWidth;
        public const float ModelScale_Inv = 1f / ModelScale;
        const int CompareToAmountCities = 8;

        public int seaDistanceHeatMap = int.MinValue; //Not in use

        
        public PCity pcity;// 2-byte
        public BiomType biom1 = BiomType.Green; 
        public BiomType biom2 = BiomType.Green;
        public byte secondBiomWeight;
        public byte biomColorHeight;
        public byte fogExploreLevel = WorldData.FogOfWarView_None;
        public bool IsBorderTile;
        public bool IsFullTile;

        public void writeMapFile(System.IO.BinaryWriter w, ref SumTile4_4 previuos)
        {
            bool eqCity = pcity == previuos.pcity;
            bool eqBiom1 = biom1 == previuos.biom1;
            bool eqBiom2 = biom2 == biom1;//Local comparison!
            bool eqBiom2Weight = secondBiomWeight == previuos.secondBiomWeight;
            bool eqColHeight = biomColorHeight == previuos.biomColorHeight;
            bool eqFog = fogExploreLevel == previuos.fogExploreLevel;

            new EightBit(eqCity, eqBiom1, eqBiom2, eqBiom2Weight, eqColHeight, eqFog, IsBorderTile, IsFullTile).write(w);

            if (!eqCity) { pcity.write(w); }
            if (!eqBiom1) { w.Write((byte)biom1); } //Could be nibbs
            if (!eqBiom2) { w.Write((byte)biom2); } //Could be nibbs
            if (!eqBiom2Weight) { w.Write(secondBiomWeight); }
            if (!eqColHeight) { w.Write(biomColorHeight); }
            if (!eqFog) { w.Write(fogExploreLevel); }
        }

        public void readMapFile(System.IO.BinaryReader r, ref SumTile4_4 previuos, int version)
        {
            EightBit bits = new EightBit(r);
            bits.Get(out var eqCity, out var eqBiom1, out var eqBiom2, out var eqBiom2Weight, out var eqColHeight, out var eqFog, out IsBorderTile, out IsFullTile);
            if (eqCity)
            {
                this.pcity = previuos.pcity;
            }
            else
            {
                this.pcity = new PCity(r);
            }
            if (eqBiom1)
            {
                this.biom1 = previuos.biom1;
            }
            else
            {
                this.biom1 = (BiomType)r.ReadByte();
            }

            // local comparison
            if (eqBiom2)
            {
                this.biom2 = this.biom1;
            }
            else
            {
                this.biom2 = (BiomType)r.ReadByte();
            }

            if (eqBiom2Weight)
            {
                this.secondBiomWeight = previuos.secondBiomWeight;
            }
            else
            {
                this.secondBiomWeight = r.ReadByte();
            }

            if (eqColHeight)
            {
                this.biomColorHeight = previuos.biomColorHeight;
            }
            else
            {
                this.biomColorHeight = r.ReadByte();
            }

            if (eqFog)
            {
                this.fogExploreLevel = previuos.fogExploreLevel;
            }
            else
            {
                this.fogExploreLevel = r.ReadByte();
            }
        }

        public SumTile4_4()
        {
            pcity = PCity.Empty;
        }

        public void GetBiom(out BiomHeightColor biomHeight, out Biom biom)
        {
            biom = DssRef.map.bioms.bioms[(int)biom1];
            biomHeight = DssRef.map.heigts[biomColorHeight];
        }

        public SumTile4_4(System.IO.BinaryReader r, ref SumTile4_4 previous, int version)
            :this()
        {
            readMapFile(r, ref previous, version);
        }

        public int mapInflenceCost()
        {
            return 1;
        }

        public void setWaterHeat_Land(int heat)
        {
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



        public bool hasBorder(out bool sameFaction)
        {
            /*
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
            */
            sameFaction = false;
            
            return false;
        }

        //public int GetBorder(int dir)
        //{
        //    switch (dir)
        //    {
        //        default:
        //            return BorderRegion_North;
        //        case 1:
        //            return BorderRegion_East;
        //        case 2:
        //            return BorderRegion_South;
        //        case 3:
        //            return BorderRegion_West;
        //    }
        //}

        public static void FindOwnerInit(out StaticList<KeyValuePair<float, City>> closeCities)
        {
            closeCities = new StaticList<KeyValuePair<float, City>>(CompareToAmountCities);
        }

        //public City pcity.City()
        //{
        //    //if (CityIndex == ushort.MaxValue)
        //    //{
        //    //    return null;
        //    //}
        //    return DssRef.world.cities[0];
        //}
        //public Faction pcity.Faction()
        //{
        //    return null;
        //}

        //public Faction Faction_Safe()
        //{
        //    return DssRef.world.cities[CityIndex].GetFaction_Safe();
        //}

        public Color FactionColor()
        {
            var c = pcity.City();//DssRef.world.cities[CityIndex];
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

       

        //public bool HasBorderImage() { return BorderCount > 0; }

       
       
        /*
        public Color MinimapColor_Faction(IntVector2 pos)
        {
            
            //if (tileContent == TileContent.City)
            //    return cityColor();

            if (heightLevel <= ColorHeight.LowerWaterHeight)
            {
                foreach (var dir in IntVector2.Dir4Array)
                {
                    if (DssRef.world.tileGrid.TryGet(pos + dir, out var nTile))
                    {
                        if (nTile.heightLevel > ColorHeight.LowerWaterHeight)
                        {
                            return lib.IsEven(pos.X + pos.Y) ?
                                WorldData.WaterDarkCol1 : WorldData.WaterDarkCol2;
                        }
                    }
                }

                return lib.IsEven(pos.X + pos.Y) ?
                    WorldData.WaterVeryDarkCol1 : WorldData.WaterVeryDarkCol2;
            }
            else if (heightLevel == ColorHeight.LowWaterHeight)
            {
                return lib.IsEven(pos.X + pos.Y) ? WorldData.WaterEdgeColorBright : WorldData.WaterEdgeColor;
            }
            else
            {
                return heightAndFactionCol(pos);
            }
        }
        */
        /*
        public Color MinimapColor_Terrain(IntVector2 pos)
        {
            if (tileContent == TileContent.City)
                return cityColor();

            if (heightLevel <= ColorHeight.LowWaterHeight)
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
           
            if (heightLevel <= ColorHeight.LowWaterHeight)
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

            Color color = ColorExt.MultiplyRGBA(factionCol, brightness);
            return color;
        }
        */
        //public ColorHeight heightSett()
        //{
        //    return DssRef.map.heigts[heightLevel];
        //}

        //public Biom Biom()
        //{
        //    return DssRef.map.bioms.bioms[(int)biom];
        //}

        //public Color cityColor()
        //{
        //    switch (pcity.City().cityType)
        //    {
        //        default: return MapCol_HeadCity;
        //        case CityType.Town: return MapCol_LargeCity;
        //        case CityType.Village: return MapCol_SmallCity;
        //        case CityType.Campsite: return MapCol_CampsiteCity;
        //        case CityType.UnClaimed: return MapCol_UnclaimedCity;

        //    }
        //}
        //public Color cityColor_Minimap()
        //{
        //    switch (pcity.City().cityType)
        //    {
        //        default: return MiniMapCol_HeadCity;
        //        case CityType.Town: return MiniMapCol_LargeCity;
        //        case CityType.Village: return MiniMapCol_SmallCity;
        //        case CityType.Campsite: return MiniMapCol_CampsiteCity;
        //        case CityType.UnClaimed: return MiniMapCol_UnclaimedCity;
        //    }
        //}

        public override string ToString()
        {
            return $"SumTile4_4:\n" +
                   $"  CityIndex: {pcity}\n" +
                   $"  SeaDistanceHeatMap: {seaDistanceHeatMap}\n" +
                   //$"  Borders (N/E/S/W): {BorderRegion_North} / {BorderRegion_East} / {BorderRegion_South} / {BorderRegion_West} (Count: {BorderCount})\n" +
                   $"  Biom1: {biom1}\n" +
                   $"  Biom2: {biom2} (Weight: {secondBiomWeight})\n" +
                   $"  BiomColorHeight: {biomColorHeight}";
        }
        //static float[] TypeToWalkingMultiplier;

        //static readonly float[] TypeToWalkingDistance = new float[]
        //{
        //    12,//Deep water
        //    6,//Deep water
        //    3,//Water_0,
        //    0.8f,//OpenField_1,
        //    1,//Plains_2,
        //    1.4f,//Vegetation_3,
        //    1.5f,//Hills_4,
        //    2.4f,//Mountain_5,
        //    4,//MountainRidge_6,
        //    8,
        //};

        //static float[] TypeToShipTravelMultiplier;

        //static readonly float[] TypeToShipDistance = new float[]
        //{
        //    0.8f,//Deep water
        //    0.8f,
        //    1f,//Water_0,
        //    4f,//OpenField_1,
        //    6,//Plains_2,
        //    6,//Vegetation_3,
        //    6,//Hills_4,
        //    6,//Mountain_5,
        //    6,
        //    6,//MountainRidge_6,
        //};



        //public const float UnitQuadMinY = WaterSurfaceY + 0.07f;
        //const float LayerHeight = 0.06f;
        //public const float LowWaterY = WaterSurfaceY - 0.07f;

        //static readonly float[] TypeToHeight = new float[]
        //{
        //    WaterSurfaceY - 0.3f,//Deep water
        //    WaterSurfaceY - 0.18f,//Deep water
        //    LowWaterY,//Water_0,
        //    0f,//OpenField_1,
        //    LayerHeight,//Plains_2,
        //    LayerHeight * 2f,//Vegetation_3,
        //    LayerHeight * 3f,//Hills_4,
        //    LayerHeight * 4.2f,//Mountain_5,
        //    LayerHeight * 5.4f,
        //    LayerHeight * 6.8f,//MountainRidge_6,
        //};

        //static float[] TypeToHeight_aboveWater;

        //public float TroupWalkingDistance(bool ship)
        //{
        //    if (ship) return TypeToShipDistance[(int)heightLevel];
        //    else return TypeToWalkingDistance[(int)heightLevel];
        //}

        //public float TerrainSpeedMultiplier(bool ship)
        //{            
        //    if (ship) return TypeToShipTravelMultiplier[heightLevel];
        //    else return TypeToWalkingMultiplier[heightLevel];
        //}

        //public float TerrainSpeedMultiplier(out bool isLand)
        //{
        //    isLand = IsLand();
        //    if (isLand) return TypeToWalkingMultiplier[heightLevel];
        //    else return TypeToShipTravelMultiplier[heightLevel];
        //}

        //public bool IsLand() { return heightLevel > ColorHeight.LowWaterHeight; }

        //public bool MayBuild() { return heightLevel > ColorHeight.LowWaterHeight && heightLevel < ColorHeight.MountainLowPeak; }

        //public bool IsWater() { return heightLevel <= ColorHeight.LowWaterHeight; }

        //public override string ToString()
        //{
        //    return (IsWater()? "water" : "land") + heightLevel.ToString() + " city:" + CityIndex.ToString();
        //}
    }

    //enum TileSpecialType
    //{
    //    NON,
    //    AdjacantToCity,
    //    Border,
    //}

    //enum TileContent : byte
    //{
    //    NONE,
    //    City,
    //}
}
