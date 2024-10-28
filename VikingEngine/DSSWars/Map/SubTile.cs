using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Valve.Steamworks;
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
        public int subTerrain = -1;
        /// <summary>
        /// Amount of resources that can be extracted, animation frame for resources, or other value like building size
        /// </summary>
        public int terrainAmount = 0;

        public int terrainQuality = 0;

        /// <summary>
        /// Pointer to array with all resources found lying on ground
        /// </summary>
        public int collectionPointer = -1;

        public SubTile(TerrainMainType type, Color color, float groundY)
        {
            this.color = color;
            this.groundY = groundY;
            this.mainTerrain = type;
        }

        public void SetType(TerrainMainType main, int under, int amount)
        {
            mainTerrain = main;
            subTerrain = under;
            terrainAmount = amount;
        }

        public void write(System.IO.BinaryWriter w)
        {            
            w.Write((byte)mainTerrain);
            w.Write((byte)subTerrain);
            w.Write((byte)terrainAmount);
            w.Write(collectionPointer);
        }

        public void read(System.IO.BinaryReader r, int version)
        {            
            mainTerrain = (TerrainMainType)r.ReadByte();
            subTerrain = r.ReadByte();
            terrainAmount = r.ReadByte();
            collectionPointer = r.ReadInt32();
        }

        public bool EqualSaveData(ref SubTile other)
        {
            return  terrainAmount == other.terrainAmount && 
                mainTerrain == other.mainTerrain && 
                subTerrain == other.subTerrain &&
                collectionPointer == other.collectionPointer;            
        }

        public void copySaveDataFrom(ref SubTile other)
        { 
            this.terrainAmount = other.terrainAmount;
            this.mainTerrain = other.mainTerrain;
            this.subTerrain = other.subTerrain;
        }

        public bool MayBuild()
        {
            switch (mainTerrain)
            {
                case TerrainMainType.Building:
                case TerrainMainType.DefaultSea:
                    return false;

                case TerrainMainType.Foil:
                    switch ((TerrainSubFoilType)subTerrain)
                    {
                        case TerrainSubFoilType.LinenFarm:
                        case TerrainSubFoilType.WheatFarm:
                        case TerrainSubFoilType.RapeSeedFarm:
                        case TerrainSubFoilType.HempFarm:
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
