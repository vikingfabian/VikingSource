using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Map
{
    struct SubTile
    {
        public static readonly SubTile Empty = new SubTile() { mainTerrain = TerrainMainType.NUM };

        public Color color;
        public float groundY;
        //public FoilType foil = FoilType.None;
        public TerrainMainType mainTerrain = TerrainMainType.NUM;
        public int subTerrain = byte.MaxValue;
        /// <summary>
        /// Amount of resources that can be extracted, animation frame for resources, or other value like building size
        /// </summary>
        public int terrainAmount = 0;

        public int terrainQuality = 0;

        /// <summary>
        /// Pointer to array with all resources found lying on ground
        /// </summary>
        public int collectionPointer = -1;

        public SubTile(TerrainMainType type, int subType)
        {
            this.mainTerrain = type;
            this.subTerrain = subType;
            terrainAmount = 1;
        }

        public SubTile(TerrainMainType type, int subType, Color color, float groundY)
        {
#if DEBUG
            //if (color == ColorExt.Empty)
            //{
            //    throw new Exception("Empty col");
            //}
#endif
            this.color = color;
            this.groundY = groundY;

            this.mainTerrain = type;
            this.subTerrain = subType;
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
            subTerrain = under;
            terrainAmount = amount;
        }

        const int EqMainTerrainIx = 0;
        const int EqSubterrainIx = 1;
        const int EqTerrainAmountIx = 2;
        const int EqCollectionPointerIx = 3;
        public void write(System.IO.BinaryWriter w, ref SubTile previous)
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
                w.Write(Debug.Byte_OrCrash(subTerrain));
            }

            if (!eqTerrainAmount)
            {
                w.Write((byte)terrainAmount);
            }

            if (!eqCollectionPointer)
            {
                w.Write(collectionPointer);
            }

            w.Write(groundY);
            StreamLib.WriteColorStream_3B(w, color);
        }

        public void read(System.IO.BinaryReader r, ref SubTile previous, int version)
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

            groundY = r.ReadSingle();
            color = StreamLib.ReadColorStream_3B(r);
#if DEBUG
            //if (color == ColorExt.Empty)
            //{
            //    throw new Exception("Empty col");
            //}
#endif
        }

        public bool EqualTerrain(SubTile other)
        {
            return mainTerrain == other.mainTerrain &&
                subTerrain == other.subTerrain;
        }
        public bool EqualTerrain(TerrainMainType main, int sub)
        {
            return mainTerrain == main &&
                subTerrain == sub;
        }
        public bool EqualSaveData(ref SubTile other)
        {
            return  terrainAmount == other.terrainAmount && 
                mainTerrain == other.mainTerrain && 
                subTerrain == other.subTerrain &&
                collectionPointer == other.collectionPointer &&
                groundY == other.groundY;            
        }

        public void copySaveDataFrom(ref SubTile other)
        { 
            this.terrainAmount = other.terrainAmount;
            this.mainTerrain = other.mainTerrain;
            this.subTerrain = other.subTerrain;
            this.groundY = other.groundY;
            this.color = other.color;

#if DEBUG
            //if (color == ColorExt.Empty)
            //{
            //    throw new Exception("Empty col");
            //}
#endif
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
                    return WorldData.SubTileWidth * 0.4f;
                case TerrainMainType.Wall:
                    switch ((TerrainWallType)subTerrain)
                    {
                        default:
                           return WorldData.SubTileWidth * 0.5f;

                        case TerrainWallType.Palisade:
                            return 0;

                        case TerrainWallType.DirtWall:
                            return WorldData.SubTileWidth * 0.3f;
                        case TerrainWallType.DirtTower:
                            return WorldData.SubTileWidth * 0.4f;


                        case TerrainWallType.WoodWall:
                            return WorldData.SubTileWidth * 0.4f;
                        case TerrainWallType.WoodTower:
                            return WorldData.SubTileWidth * 0.4f;

                        case TerrainWallType.StoneWall:
                        case TerrainWallType.StoneWallBlueRoof:
                        case TerrainWallType.StoneWallGreen:
                        case TerrainWallType.StoneWallWoodHouse:
                        case TerrainWallType.StoneGate:
                            return WorldData.SubTileWidth * 0.6f;

                        case TerrainWallType.StoneTower:
                            return WorldData.SubTileWidth * 1.3f;

                    }
            }
        }
    }

}
