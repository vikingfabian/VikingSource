using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.LootFest;
using VikingEngine.ToGG.HeroQuest;

namespace VikingEngine.DSSWars.Map
{
    class Tile
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
        public int heightLevel;
        public TileContent tileContent = TileContent.NONE;
        public int BorderCount;
        public int BorderRegion_North, BorderRegion_East, BorderRegion_South, BorderRegion_West;
        public int seaDistanceHeatMap = int.MinValue;
        //--

        public int WorkerCount = 0;
        public byte renderStateA = Culling.NoRender;
        public byte renderStateB = Culling.NoRender;
        public bool hasTileInRender = false; 

        //public bool inRender = false;

        public Tile()
        {
            CityIndex = -1;

            heightLevel = Height.DeepWaterHeight;
           
            BorderCount = 0;
            BorderRegion_North = NoBorderRegion; 
            BorderRegion_East = NoBorderRegion; 
            BorderRegion_South = NoBorderRegion; 
            BorderRegion_West = NoBorderRegion;
        }

        public Tile(System.IO.BinaryReader r, int version)
        {
            readMapFile(r, version);
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


        public void writeMapFile(System.IO.BinaryWriter w)
        {
            w.Write(Debug.Ushort_OrCrash(CityIndex));//(ushort)CityIndex);
            w.Write(Debug.Byte_OrCrash((byte)biom));//(byte)biom);
            w.Write(Debug.Byte_OrCrash(heightLevel));//(byte)heightLevel);
            w.Write(Debug.Byte_OrCrash((int)tileContent));//(byte)tileContent);
            w.Write(Debug.Ushort_OrCrash(BorderCount));//(ushort)BorderCount);

            w.Write(Debug.Short_OrCrash(BorderRegion_North));//(short)BorderRegion_North);
            w.Write(Debug.Short_OrCrash(BorderRegion_East));//(short)BorderRegion_East);
            w.Write(Debug.Short_OrCrash(BorderRegion_South));//(short)BorderRegion_South);
            w.Write(Debug.Short_OrCrash(BorderRegion_West));//(short)BorderRegion_West);
        }

        public void readMapFile(System.IO.BinaryReader r, int version)
        {
            CityIndex = r.ReadUInt16();
            biom = (BiomType)r.ReadByte();
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

        public City City()
        {
            if (CityIndex < 0)
            {
                return null;
            }
            return DssRef.world.cities[CityIndex]; 
        }

        static readonly Color HeadCity = new Color(255,174,184);
        static readonly Color LargeCity = new Color(253,0,30);
        static readonly Color SmallCity = new Color(148,0,17);

        public bool HasBorderImage() { return BorderCount > 0; }

        public Color MinimapColor(IntVector2 pos)
        {
            
            if (tileContent == TileContent.City)
                return cityColor;

            if (heightLevel <= Height.LowWaterHeight)
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

        public Height heightSett()
        {
            return DssRef.map.heigts[heightLevel];
        }

        public Biom Biom()
        {
            return DssRef.map.bioms.bioms[(int)biom];
        }

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
            6,//Deep water
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
            4f,//OpenField_1,
            6,//Plains_2,
            6,//Vegetation_3,
            6,//Hills_4,
            6,//Mountain_5,
            6,//MountainRidge_6,
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

        public bool IsLand() { return heightLevel > Height.LowWaterHeight; }
        public bool IsWater() { return heightLevel <= Height.LowWaterHeight; }
        
        public override string ToString()
        {
            if (IsWater())
            {
                return "Water";
            }
            return heightLevel.ToString() + " {" + City().ToString() + "}";
        }
    }

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
