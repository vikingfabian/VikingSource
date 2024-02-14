using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.Map
{
    class Tile
    {
        public static TerrainSettings[,] TerrainTypes;

        public static void Init()
        {
            TypeToHeight_aboveWater =new float[TypeToHeight.Length];
            for (int i = 0; i < TypeToHeight.Length; i++)
            {
                TypeToHeight_aboveWater[i] = Math.Max(TypeToHeight[i], 0);
            }

            TerrainTypes = new TerrainSettings[TerrainSettings.BiomCount, MaxHeight + 1];
            for (int biom = 0; biom < TerrainSettings.BiomCount; ++biom)
            {
                for (int height = 0; height <= MaxHeight; ++height)
                {
                    TerrainTypes[biom, height] = new TerrainSettings(biom, height);
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

        public const int NoBorderRegion = -2;
        public const int SeaBorder = -1;
        const int CompareToAmountCities = 8;

        public const int DeepWaterHeight = 0;
        public const int LowWaterHeight = 1;
        public const int MinLandHeight = 2;
        public const int MountainHeightStart = 6;
        const int MaxHeight = 7;


        //Save data
        public int CityIndex;
        public int biom = TerrainSettings.BiomTypeGreen;
        public int heightLevel;
        public TileContent tileContent = TileContent.NONE;
        public int BorderCount;
        public int BorderRegion_North, BorderRegion_East, BorderRegion_South, BorderRegion_West;
        //--

        
        public int WorkerCount = 0;
        public byte renderStateA = Culling.NoRender;
        public byte renderStateB = Culling.NoRender;
        public bool hasTileInRender = false; 

        //public bool inRender = false;

        public Tile()
        {
            CityIndex = -1;

            heightLevel = DeepWaterHeight;
           
            BorderCount = 0;
            BorderRegion_North = NoBorderRegion; 
            BorderRegion_East = NoBorderRegion; 
            BorderRegion_South = NoBorderRegion; 
            BorderRegion_West = NoBorderRegion;
        }

        public Tile(System.IO.BinaryReader r, int version)
        {
            read(r, version);
        }


        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Debug.Ushort_OrCrash(CityIndex));//(ushort)CityIndex);
            w.Write(Debug.Byte_OrCrash(biom));//(byte)biom);
            w.Write(Debug.Byte_OrCrash(heightLevel));//(byte)heightLevel);
            w.Write(Debug.Byte_OrCrash((int)tileContent));//(byte)tileContent);
            w.Write(Debug.Ushort_OrCrash(BorderCount));//(ushort)BorderCount);

            w.Write(Debug.Short_OrCrash(BorderRegion_North));//(short)BorderRegion_North);
            w.Write(Debug.Short_OrCrash(BorderRegion_East));//(short)BorderRegion_East);
            w.Write(Debug.Short_OrCrash(BorderRegion_South));//(short)BorderRegion_South);
            w.Write(Debug.Short_OrCrash(BorderRegion_West));//(short)BorderRegion_West);
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            CityIndex = r.ReadUInt16();
            biom = r.ReadByte();
            heightLevel = r.ReadByte();
            tileContent = (TileContent)r.ReadByte();
            BorderCount = r.ReadUInt16();

            BorderRegion_North = r.ReadInt16();
            BorderRegion_East = r.ReadInt16();
            BorderRegion_South = r.ReadInt16();
            BorderRegion_West = r.ReadInt16();
        }

        public void removeCity()
        {
            BorderCount=0;
            BorderRegion_North = NoBorderRegion; 
            BorderRegion_East = NoBorderRegion; 
            BorderRegion_South = NoBorderRegion; 
            BorderRegion_West = NoBorderRegion;
            CityIndex = -1;
            tileContent = TileContent.NONE;
        }

        //public void setRenderState(bool inRender)
        //{
        //    this.inRender = inRender;

        //    City()?.setRenderState(inRender);
        //}

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

        //public void FindOwner(IntVector2 myPos, List<City> cities, WorldData tiles, StaticList<KeyValuePair<float, City>> closeCities)
        //{

        //    if (IsLand() && tileContent != TileContent.City)
        //    {
        //       //First pick out the closest cities, ignoring terrain
        //        closeCities.QuickClear();

        //        float furthestAddedCity = 0;
        //        int furthestAddedCityIndex = -1;

        //        int preFillCount = lib.SmallestValue(CompareToAmountCities, cities.Count);
        //        for (int preSetIx = 0; preSetIx < preFillCount; ++preSetIx)
        //        {
        //            float dist = WP.birdDistance(cities[preSetIx], myPos);

        //            if (dist <= 2)
        //            { //Very close to city, will auto assign to that
        //                CityIndex = cities[preSetIx].index;
        //                return;
        //            }

        //            closeCities.Add(new KeyValuePair<float, City>(dist, cities[preSetIx]));
        //            if (dist > furthestAddedCity)
        //            {
        //                furthestAddedCity = dist;
        //                furthestAddedCityIndex = preSetIx;
        //            }
        //        }

        //        for (int i = CompareToAmountCities; i < cities.Count; ++i)
        //        {
        //            float dist = WP.birdDistance(cities[i], myPos);
        //            if (dist < furthestAddedCity)
        //            {
        //                closeCities.Array[furthestAddedCityIndex] = new KeyValuePair<float, City>(dist, cities[i]);
        //                furthestAddedCity = 0;
        //                for (int cc = 0; cc < closeCities.Count; ++cc)
        //                {
        //                    if (closeCities.Array[cc].Key > furthestAddedCity)
        //                    {
        //                        furthestAddedCity = dist;
        //                        furthestAddedCityIndex = cc;
        //                    }
        //                }

        //            }
        //        }


        //        FindMinValue closest = new FindMinValue(true);
        //        //compare the distance to the closest cities, with terrain included
        //        for (int cc = 0; cc < closeCities.Count; ++cc)
        //        {
        //            float dist = distanceValueToTile(myPos, closeCities[cc].Value.tilePos, tiles);
        //            closest.Next(dist, cc);
        //        }
        //        CityIndex = closeCities[closest.minMemberIndex].Value.index;
        //    }
        //}

        //static float distanceValueToTile(IntVector2 start, IntVector2 end, WorldData tiles)
        //{
        //    const float StepLenghtMulti = 0.8f;

        //    float result = 0;
        //    Vector2 pos = start.Vec;
        //    IntVector2 diff = (end - start);
        //    Vector2 stepLength = diff.Vec;
        //    stepLength.Normalize();
        //    stepLength *= StepLenghtMulti;

        //    float lenght = diff.Length();
        //    int numSteps = (int)(lenght / StepLenghtMulti);
        //    for (int i = 1; i < numSteps; ++i)
        //    {
        //        pos += stepLength;
        //        result += tiles.GetTile(pos).TroupWalkingDistance(false) + (i * 1f);
        //    }

        //    return result; //+ lib.Square(lenght);
        //}

        //public static float birdDistanceToCity(City city, IntVector2 tilePos)
        //{
        //    return (city.tilePos - tilePos).Length();
        //}

        public City City()
        {
            if (CityIndex < 0)
            {
                return null;
            }
            return DssRef.world.cities[CityIndex]; 
        }

        //public bool IsCity()
        //{ 
        //    return tileContent == TileContent.City;
        //}

        static readonly Color HeadCity = new Color(255,174,184);
        static readonly Color LargeCity = new Color(253,0,30);
        static readonly Color SmallCity = new Color(148,0,17);

        

        ////const int StartCol
        //const int MinimapLevelCol = 10;
        //static readonly Color MinimapLvl1 = Color.White;
        //static readonly Color MinimapLvl2 = new Color(byte.MaxValue - MinimapLevelCol, byte.MaxValue - MinimapLevelCol, byte.MaxValue - MinimapLevelCol);
        //static readonly Color MinimapLvl3 = new Color(byte.MaxValue - MinimapLevelCol * 2, byte.MaxValue - MinimapLevelCol * 2, byte.MaxValue - MinimapLevelCol * 2);
        //static readonly Color MinimapLvl4 = new Color(byte.MaxValue - MinimapLevelCol * 3, byte.MaxValue - MinimapLevelCol * 3, byte.MaxValue - MinimapLevelCol * 3);
        //static readonly Color MinimapLvl5 = new Color(byte.MaxValue - MinimapLevelCol * 4, byte.MaxValue - MinimapLevelCol * 4, byte.MaxValue - MinimapLevelCol * 4);
        //static readonly Color MinimapLvl6 = new Color(byte.MaxValue - MinimapLevelCol * 5, byte.MaxValue - MinimapLevelCol * 5, byte.MaxValue - MinimapLevelCol * 5);

        public bool HasBorderImage() { return BorderCount > 0; }

        //public Color TerrainColor
        //{
        //    get
        //    {
        //        switch (height)
        //        {
        //            case TerrainType.DeepWater_0: return DeepWaterCol1;
        //            case TerrainType.LowWater_1: return SeaBottomCol;
        //            case TerrainType.OpenField_2: return Biom ? Dry1 : Ground1;
        //            case TerrainType.Plains_3: return Biom ? Dry2 : Ground2;
        //            case TerrainType.Vegetation_4: return Biom ? Dry3 : Ground3;
        //            case TerrainType.Hills_5: return Biom ? Dry4 : Ground4;
        //            case TerrainType.Mountain_6: return Biom ? Dry5 : Ground5;
        //            case TerrainType.MountainRidge_7: return Biom ? Dry6 : Ground6;

        //           // case TerrainType.Urban: return cityColor;


        //            default: throw new NotImplementedException();
        //        }
        //    }
        //}

        //public VoxelModelName TerrainModel
        //{
        //    get
        //    {
        //        switch (heightLevel)
        //        {
        //            //case TerrainType.DeepWater_0: return VoxelModelName.NUM_NON;//DeepWaterCol1;
        //            //case TerrainType.LowWater_1: return VoxelModelName.warmap_sand1;// SeaBottomCol;
        //            //case TerrainType.OpenField_2: return VoxelModelName.warmap_sand1; //DryTerrain ? Dry1 : Ground1;
        //            //case TerrainType.Plains_3: return VoxelModelName.warmap_sanddark1; //DryTerrain ? Dry2 : Ground2;
        //            //case TerrainType.Vegetation_4: return VoxelModelName.warmap_grass1;//DryTerrain ? Dry3 : Ground3;
        //            //case TerrainType.Hills_5: return VoxelModelName.warmap_grassdark1; //DryTerrain ? Dry4 : Ground4;
        //            //case TerrainType.Mountain_6: return VoxelModelName.warmap_mountain1; //ryTerrain ? Dry5 : Ground5;
        //            //case TerrainType.MountainRidge_7: return VoxelModelName.warmap_mountaindark1; //DryTerrain ? Dry6 : Ground6;

        //            // case TerrainType.Urban: return cityColor;


        //            default: throw new NotImplementedException();
        //        }
        //    }
        //}

        public Color MinimapColor(IntVector2 pos)
        {
            
            if (tileContent == TileContent.City)
                return cityColor;

            if (heightLevel <= LowWaterHeight)
            {
                return lib.IsEven(pos.X + pos.Y) ? 
                    WorldData.WaterDarkCol : WorldData.WaterDarkCol2;
            }
            else
            {
                return heightAndFactionCol(pos);
            }
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
            //if (IsLand)
            //{
            //    float result = TypeToHeight[(int)terrain];
            //    if (AdjacantToCity != null || IsCity)//Special is TileSpecial_AdjacentToCity || IsCity)
            //    {
            //        result += 0.3f;
            //    }
            //    return result;
            //}
            //else
            //{
            //    return WaterHeight;
            //}
        }

        Color heightAndFactionCol(IntVector2 pos)
        {   
            float brightness = 1f - ((int)heightLevel - 2) * 0.05f;

            Tile nTile;
            var faction = City().faction;
            City nCity;
            bool isCityAdjacent = false;
            bool isCityAdjacentCorner = false;

            foreach (var dir in IntVector2.AllDiagonalsArray)
            {
                if (DssRef.world.tileGrid.TryGet(pos + dir, out nTile))
                {
                    if (nTile.tileContent == TileContent.City)
                    {
                        isCityAdjacent = true;
                        isCityAdjacentCorner = true;
                        break;
                    }
                }
            }

            if (!isCityAdjacent)
            {
                foreach (var dir in IntVector2.Dir4Array)
                {
                    if (DssRef.world.tileGrid.TryGet(pos + dir, out nTile))
                    {
                        if (nTile.tileContent == TileContent.City)
                        {
                            isCityAdjacent = true;                            
                            break;
                        }
                        else
                        {
                            nCity = nTile.City();
                            if (nCity != null && faction != nCity.faction)
                            {
                                brightness -= 0.2f;
                                break;
                            }
                        }
                    }
                }
            }

            if (isCityAdjacent)
            {
                brightness = isCityAdjacentCorner? 1.15f : 1.25f;
            }

            Color color = new Color(faction.Color().ToVector3() * brightness);
            return color;
        }

        public TerrainSettings terrain()
        {
            return TerrainTypes[biom, heightLevel];
        }
        //Color heightColor()
        //{
        //    switch (height)
        //    {
        //        case TerrainType.DeepWater_0: return DeepWaterCol1;
        //        case TerrainType.LowWater_1: return SeaBottomCol;
        //        case TerrainType.OpenField_2: return MinimapLvl1;
        //        case TerrainType.Plains_3: return MinimapLvl2;
        //        case TerrainType.Vegetation_4: return MinimapLvl3;
        //        case TerrainType.Hills_5: return MinimapLvl4;
        //        case TerrainType.Mountain_6: return MinimapLvl5;
        //        case TerrainType.MountainRidge_7: return MinimapLvl6;

        //        //case TerrainType.Urban: return cityColor;
        //        default: throw new NotImplementedException();
        //    }            
        //}

        Color cityColor
        {
            get
            {
                switch (City().CityType)
                {
                    default: return HeadCity;
                    case CityType.Large: return LargeCity;
                    case CityType.Small: return SmallCity;
                }
            }
        }
        static float[] TypeToWalkingMultiplier;

        static readonly float[] TypeToWalkingDistance = new float[]
        {
            20,//Deep water
            3,//Water_0,
            0.8f,//OpenField_1,
            1,//Plains_2,
            1.4f,//Vegetation_3,
            1.5f,//Hills_4,
            2.4f,//Mountain_5,
            4,//MountainRidge_6,
        };

        static float[] TypeToShipTravelMultiplier;

        static readonly float[] TypeToShipDistance = new float[]
        {
            0.8f,//Deep water
            1f,//Water_0,
            8f,//OpenField_1,
            12,//Plains_2,
            15,//Vegetation_3,
            28,//Hills_4,
            48,//Mountain_5,
            200,//MountainRidge_6,
        };

        public const float WaterSurfaceY = -0.1f;
        public const float UnitMinY = WaterSurfaceY + 0.02f;

        static readonly float[] TypeToHeight = new float[]
        {
            WaterSurfaceY - 0.1f,//Deep water
            WaterSurfaceY - 0.08f,//Water_0,
            0f,//OpenField_1,
            0.1f,//Plains_2,
            0.2f,//Vegetation_3,
            0.3f,//Hills_4,
            0.45f,//Mountain_5,
            0.6f,//MountainRidge_6,
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

        public bool IsLand() { return heightLevel > LowWaterHeight; }
        public bool IsWater() { return heightLevel <= LowWaterHeight; }
        //TileSpecialType TileSpecialType
        //{
        //    get 
        //    {
        //        if (Special == null)
        //            return Map.TileSpecialType.NON;
        //        else return Special.Type;
        //    }
        //}
        public override string ToString()
        {
            if (IsWater())
            {
                return "Water";
            }
            return heightLevel.ToString() + " {" + City().ToString() + "}";
        }
    }

    //enum BiomType
    //{
    //    Green,
    //    Dry,
    //}

    enum TerrainType
    {
        DeepWater_0,
        LowWater_1,
        OpenField_2,
        Plains_3,
        Vegetation_4,
        Hills_5,
        Mountain_6,
        MountainRidge_7,

       // Urban,
        NUM
    }

    enum BattleTerrain
    { 
        //Water,
        Ship,
        Land,
        City,
        LandAndCityMix,
        NUM
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
        WorkerHut,
    }
}
