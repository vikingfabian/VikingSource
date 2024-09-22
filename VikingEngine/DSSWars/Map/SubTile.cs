﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Valve.Steamworks;

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
            //w.Write(color.R);
            //w.Write(color.G);
            //w.Write(color.B);
            //w.Write(groundY);
            w.Write((byte)mainTerrain);
            w.Write((byte)subTerrain);
            w.Write((byte)terrainAmount);
            w.Write(collectionPointer);
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            //byte rValue = r.ReadByte();
            //byte gValue = r.ReadByte();
            //byte bValue = r.ReadByte();
            //color = new Color(rValue, gValue, bValue);
            //groundY = r.ReadSingle();
            mainTerrain = (TerrainMainType)r.ReadByte();
            subTerrain = r.ReadByte();
            terrainAmount = r.ReadByte();
            collectionPointer = r.ReadInt32();
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
           string result =  mainTerrain.ToString();

            switch (mainTerrain)
            {
                case TerrainMainType.Building:
                    result += " - " + ((TerrainBuildingType)subTerrain).ToString();
                    break;
                case TerrainMainType.Foil:
                    result += " - " + ((TerrainSubFoilType)subTerrain).ToString();
                    break;
                case TerrainMainType.Mine:
                    result += " - " + ((TerrainMineType)subTerrain).ToString();
                    break;
            }

            return result;
        }
    }

}
