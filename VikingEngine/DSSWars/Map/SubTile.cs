using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Valve.Steamworks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Map
{
    struct SubTile
    {
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

        public SubTile(TerrainMainType type, int subType, Color color, float groundY)
        {
            this.color = color;
            this.groundY = groundY;
            this.mainTerrain = type;
            this.subTerrain = subType;
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
            SaveLib.WriteColorStream_3B(w, color);
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
            color = SaveLib.ReadColorStream_3B(r);
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
        }

        public bool MayBuild(BuildAndExpandType build, out bool upgrade)
        {
            upgrade = false;
            switch (mainTerrain)
            {
                case TerrainMainType.Building:
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

                        case TerrainSubFoilType.WheatFarmUpgraded:
                        case TerrainSubFoilType.LinenFarmUpgraded:
                        case TerrainSubFoilType.RapeSeedFarmUpgraded:
                        case TerrainSubFoilType.HempFarmUpgraded:
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

        public TerrainBuildingType GeBuildingType()
        {
            if (mainTerrain == TerrainMainType.Building &&
                subTerrain >= 0)
            {
                return (TerrainBuildingType)subTerrain;
            }

            return TerrainBuildingType.NUM_NONE;
        }

        public string TypeToString()
        {
            return LangLib.TerrainName(mainTerrain, subTerrain);
        }
    }

}
