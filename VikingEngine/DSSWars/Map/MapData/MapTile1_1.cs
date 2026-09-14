using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.DSSWars.Map.Path3;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.LootFest.Players;
using VikingEngine.DSSWars.Map.MapLib;

namespace VikingEngine.DSSWars.Map.MapData
{
    //struct LandTileData
    //{ 
    //    public byte subTerrain = byte.MaxValue;
    //    /// <summary>
    //    /// Amount of resources that can be extracted, animation frame for resources, or other value like building size
    //    /// </summary>
    //    public byte terrainAmount = 0;

    //    public byte health = 100;

    //    public byte orientation = 0;

    //    //public byte terrainQuality = 0;

    //    /// <summary>
    //    /// Pointer to array with all resources found lying on ground
    //    /// </summary>
    //    public int collectionPointer = -1;

    //    public LandTileData() 
    //    { }
    //}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MapTile1_1
    {

        public static readonly MapTile1_1 Empty = new MapTile1_1() { mainTerrain = TerrainMainType.NUM };

        public const int ModelScale_Inv = 8;
        public const int ModelScale_Inv_Half = ModelScale_Inv / 2;
        public const float ModelScale = 1f / ModelScale_Inv;

        public const int ModelScale_Inv_MaxIndex = ModelScale_Inv - 1;

        public static readonly Vector2 ModelScaleV2 = new Vector2(ModelScale);
        public static readonly float SubTileHalfWidth = ModelScale * 0.5f;

        
        //public Color color;
        public byte heightValue;
        public TerrainMainType mainTerrain = TerrainMainType.NUM;

        //public int landDataIndex = -1;
        public byte subTerrain = byte.MaxValue;
        /// <summary>
        /// Amount of resources that can be extracted, animation frame for resources, or other value like building size
        /// </summary>
        public byte terrainAmount = 0;

        public byte health = 100;

        public byte orientation = 0;

        //public byte terrainQuality = 0;

        /// <summary>
        /// Pointer to array with all resources found lying on ground
        /// </summary>
        public int collectionPointer = -1;

        public float groundY => heightValue * MapHeight2.HeightY;

        public float GroundY_aboveWater()
        {
           return Bound.Min(heightValue* MapHeight2.HeightY, MapHeight2.WaterSurfaceY);
        }

        public bool IsLand()
        {
            return heightValue > MapHeight2.WaterPlaneHeight;
        }

        public MapTile1_1(TerrainMainType type, int subType)
        {
            this.mainTerrain = type;
            this.subTerrain = (byte)subType;
            terrainAmount = 1;
        }

        public MapTile1_1(TerrainMainType type, int subType, byte heightValue/*, Color color, float groundY*/)
        {
#if DEBUG
            //if (color == ColorExt.Empty)
            //{
            //    throw new Exception("Empty col");
            //}
#endif
            //this.color = color;
            //this.groundY = groundY;

            this.mainTerrain = type;
            this.subTerrain = (byte)subType;
        }

        public float TerrainBlockMultipleValue()
        {
            switch (mainTerrain)
            {
                case TerrainMainType.Wall:
                    return DetailPathNode.MoveCostWall;

                case TerrainMainType.Foil:
                    switch ((TerrainSubFoilType)subTerrain)
                    {
                        case TerrainSubFoilType.TreeHard:
                        case TerrainSubFoilType.TreeSoft:
                        case TerrainSubFoilType.DryWood:
                            return DetailPathNode.MoveCostHindering;
                    }
                    break;
                case TerrainMainType.Building:
                    return DetailPathNode.MoveCostHindering;

                case TerrainMainType.Mine:
                    return DetailPathNode.MoveCostHindering;
            }

            return 1;
        }

        public void SetType(TerrainMainType main, int under, int amount)
        {
            mainTerrain = main;
            subTerrain = (byte)under;
            terrainAmount = (byte)amount;
        }

        const int EqMainTerrainIx = 0;
        const int EqSubterrainIx = 1;
        const int EqTerrainAmountIx = 2;
        const int EqCollectionPointerIx = 3;
        public void write(System.IO.BinaryWriter w, ref MapTile1_1 previous)
        {
            //TODO check repeats with previous, use eightbit
            bool eqMainTerrain = mainTerrain == previous.mainTerrain;
            bool eqSubterrain = subTerrain == previous.subTerrain;
            bool eqTerrainAmount = terrainAmount == previous.terrainAmount;
            bool eqCollectionPointer = collectionPointer == previous.collectionPointer;

            EightBit reapeats = new EightBit();
            reapeats.Set(EqMainTerrainIx, eqMainTerrain);
            reapeats.Set(EqSubterrainIx, eqSubterrain);
            reapeats.Set(EqTerrainAmountIx, eqTerrainAmount);
            reapeats.Set(EqCollectionPointerIx, eqCollectionPointer);

            reapeats.write(w);

            if (!eqMainTerrain)
            {
                w.Write((byte)mainTerrain);
            }

            if (!eqSubterrain)
            {
                w.Write((byte)subTerrain);
            }

            if (!eqTerrainAmount)
            {
                w.Write((byte)terrainAmount);
            }

            if (!eqCollectionPointer)
            {
                w.Write(collectionPointer);
            }

            w.Write(heightValue);
            //StreamLib.WriteColorStream_3B(w, color);
        }

        public void read(System.IO.BinaryReader r, ref MapTile1_1 previous, int version)
        {
            EightBit reapeats = new EightBit(r);

            if (reapeats.Get(EqMainTerrainIx))
            {
                mainTerrain = previous.mainTerrain;
            }
            else
            {
                mainTerrain = (TerrainMainType)r.ReadByte();
            }

            if (reapeats.Get(EqSubterrainIx))
            {
                subTerrain = previous.subTerrain;
            }
            else
            {
                subTerrain = r.ReadByte();
            }

            if (reapeats.Get(EqTerrainAmountIx))
            {
                terrainAmount = previous.terrainAmount;
            }
            else
            {
                terrainAmount = r.ReadByte();
            }

            if (reapeats.Get(EqCollectionPointerIx))
            {
                collectionPointer = previous.collectionPointer;
            }
            else
            {
                collectionPointer = r.ReadInt32();
            }

            heightValue = r.ReadByte();
            //groundY = r.ReadSingle();
            //color = StreamLib.ReadColorStream_3B(r);
#if DEBUG
            //if (color == ColorExt.Empty)
            //{
            //    throw new Exception("Empty col");
            //}
#endif
        }

        public bool EqualTerrain(MapTile1_1 other)
        {
            return mainTerrain == other.mainTerrain &&
                subTerrain == other.subTerrain;
        }
        public bool EqualTerrain(TerrainMainType main, int sub)
        {
            return mainTerrain == main &&
                subTerrain == (byte)sub;
        }
        public bool EqualSaveData(ref MapTile1_1 other)
        {
            return  terrainAmount == other.terrainAmount && 
                mainTerrain == other.mainTerrain && 
                subTerrain == other.subTerrain &&
                collectionPointer == other.collectionPointer &&
                heightValue == other.heightValue;            
        }

        public void copySaveDataFrom(ref MapTile1_1 other)
        { 
            this.terrainAmount = other.terrainAmount;
            this.mainTerrain = other.mainTerrain;
            this.subTerrain = other.subTerrain;
            this.heightValue = other.heightValue;
            //this.color = other.color;

#if DEBUG
            //if (color == ColorExt.Empty)
            //{
            //    throw new Exception("Empty col");
            //}
#endif
        }

        public bool TileContentIsCity()
        {
            if (mainTerrain == TerrainMainType.Building)
            {
                switch ((TerrainBuildingType)subTerrain)
                {
                    case TerrainBuildingType.CityHall_Capital:
                    case TerrainBuildingType.CityHall_Tent:
                    case TerrainBuildingType.CityHall_Town:
                    case TerrainBuildingType.CityHall_Unclaimed:
                    case TerrainBuildingType.CityHall_Village:
                        return true;
                }
            }

            return false;
        }
        public bool MayBuild(BuildAndExpandType build, out bool upgrade)
        {
            upgrade = false;
            switch (mainTerrain)
            {
                case TerrainMainType.Building:
                    switch ((TerrainBuildingType)subTerrain)
                    {
                        default: return false;

                        case TerrainBuildingType.Postal:
                            upgrade = true;
                            return build == BuildAndExpandType.PostalLevel2 || build == BuildAndExpandType.PostalLevel3;
                        case TerrainBuildingType.PostalLevel2:
                            upgrade = true;
                            return build == BuildAndExpandType.PostalLevel3;

                        case TerrainBuildingType.Recruitment:
                            upgrade = true;
                            return build == BuildAndExpandType.RecruitmentLevel2 || build == BuildAndExpandType.RecruitmentLevel3;
                        case TerrainBuildingType.RecruitmentLevel2:
                            upgrade = true;
                            return build == BuildAndExpandType.RecruitmentLevel3;
                    }

                case TerrainMainType.Mine:
                case TerrainMainType.DefaultSea:
                    return false;

                case TerrainMainType.Foil:
                    switch ((TerrainSubFoilType)subTerrain)
                    {
                        case TerrainSubFoilType.WheatFarm:
                            upgrade = true;
                            return build == BuildAndExpandType.WheatFarmUpgraded;

                        case TerrainSubFoilType.LinenFarm:
                            upgrade = true;
                            return build == BuildAndExpandType.LinenFarmUpgraded;

                        case TerrainSubFoilType.RapeSeedFarm:
                            upgrade = true;
                            return build == BuildAndExpandType.RapeSeedFarmUpgraded;

                        case TerrainSubFoilType.HempFarm:
                            upgrade = true;
                            return build == BuildAndExpandType.HempFarmUpgraded;

                        //case TerrainSubFoilType.WheatFarmUpgraded:
                        //case TerrainSubFoilType.LinenFarmUpgraded:
                        //case TerrainSubFoilType.RapeSeedFarmUpgraded:
                        //case TerrainSubFoilType.HempFarmUpgraded:
                        case TerrainSubFoilType.BogIron:
                        case TerrainSubFoilType.ClayPit:
                        case TerrainSubFoilType.SaltPit:
                            return false;
                        
                    }
                    break;
            }

            return true;
        }

        public TerrainSubFoilType GetFoilType()
        {
            if (mainTerrain == TerrainMainType.Foil &&
                subTerrain >= 0)
            {
                return (TerrainSubFoilType)subTerrain;
            }

            return TerrainSubFoilType.NUM_NONE;
        }

        public TerrainBuildingType GetBuildingType()
        {
            if (mainTerrain == TerrainMainType.Building &&
                subTerrain >= 0)
            {
                return (TerrainBuildingType)subTerrain;
            }

            return TerrainBuildingType.NUM_NONE;
        }

        public TerrainWallType GetWallType()
        {
            if (mainTerrain == TerrainMainType.Wall &&
                subTerrain >= 0)
            {
                return (TerrainWallType)subTerrain;
            }

            return TerrainWallType.NUM_NONE;
        }

        public string TypeToString()
        {
            IconName.Terrain(mainTerrain, subTerrain, out _, out string name);
            return name;
        }

        public bool IsWater()
        {
            return mainTerrain == TerrainMainType.DefaultSea;
        }

        public float BuildingHeight()
        {
            switch (mainTerrain)
            {
                default:
                    return 0;

                case TerrainMainType.Building:
                    return Map.MapData.MapTile1_1.ModelScale * 0.4f;
                case TerrainMainType.Wall:
                    switch ((TerrainWallType)subTerrain)
                    {
                        default:
                           return Map.MapData.MapTile1_1.ModelScale * 0.5f;

                        case TerrainWallType.Palisade:
                            return 0;

                        case TerrainWallType.DirtWall:
                            return Map.MapData.MapTile1_1.ModelScale * 0.3f;
                        case TerrainWallType.DirtTower:
                            return Map.MapData.MapTile1_1.ModelScale * 0.4f;


                        case TerrainWallType.WoodWall:
                            return Map.MapData.MapTile1_1.ModelScale * 0.4f;
                        case TerrainWallType.WoodTower:
                            return Map.MapData.MapTile1_1.ModelScale * 0.4f;

                        case TerrainWallType.StoneWall:
                        case TerrainWallType.StoneWallBlueRoof:
                        case TerrainWallType.StoneWallGreen:
                        case TerrainWallType.StoneWallWoodHouse:
                        case TerrainWallType.StoneGate:
                            return Map.MapData.MapTile1_1.ModelScale * 0.6f;

                        case TerrainWallType.StoneTower:
                            return Map.MapData.MapTile1_1.ModelScale * 1.3f;

                    }
            }
        }

        public MoveCost GetMoveCost()
        {
            switch (mainTerrain)
            {
                default:
                case TerrainMainType.DefaultLand:
                    switch ((TerrainDefaultLandType)subTerrain)
                    {
                        case TerrainDefaultLandType.Mountain:
                            return new MoveCost(20, 2000) { deadlyTerrain = true };

                        default:
                        case TerrainDefaultLandType.Flat:
                            return new MoveCost(1, 100);
                    }

                case TerrainMainType.DefaultSea:
                    if (heightValue >= MapHeight2.ShallowWaterHeight)
                    {
                        return new MoveCost(50, 1f) { isWater = true };
                    }
                    else
                    {
                        return new MoveCost(10000, 1f) { isWater = true };
                    }

                case TerrainMainType.Destroyed:
                    return new MoveCost(0.9f, 90f);

                case TerrainMainType.Resourses:
                case TerrainMainType.Mine:
                    return new MoveCost(2f, 200);
                case TerrainMainType.Decor:
                case TerrainMainType.Building:
                    return new MoveCost(2f, 200) { urbanTerrain = true };

                case TerrainMainType.Foil:
                    return new MoveCost(4f, 400) { natureTerrain = true };

                case TerrainMainType.Wall:
                    // Water cost for walls was originally cost.land * 100.
                    // These values are pre-calculated below.
                    switch ((TerrainWallType)subTerrain)
                    {
                        case TerrainWallType.Palisade:
                            return new MoveCost(3f) { urbanTerrain = true };

                        case TerrainWallType.DirtWall:
                        case TerrainWallType.DirtTower:
                            return new MoveCost(4f) { urbanTerrain = true };

                        case TerrainWallType.WoodWall:
                        case TerrainWallType.WoodTower:
                            return new MoveCost(5f) { urbanTerrain = true };

                        default:
                            return new MoveCost(8f) { urbanTerrain = true };
                    }

                case TerrainMainType.Road:
                    //switch ((TerrainRoadType)subTerrain)
                    //{
                    //    case TerrainRoadType.
                    //}
                    return new MoveCost(0.75f, 75f);
            }
        }

        const float ModelGroundYAdj = 0.06f;
        public float UnitGroundY()
        {
            float result = groundY + ModelGroundYAdj;
            if (result > MapHeight2.WaterSurfaceY)
            {
                return result;
            }

            return MapHeight2.WaterSurfaceY;
        }
    }

}
