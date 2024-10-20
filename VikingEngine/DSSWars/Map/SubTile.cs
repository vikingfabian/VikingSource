﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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
        /// Amount of resources that can be extracted, or other value like building size
        /// </summary>
        public int terrainValue = 0;
        //public int colorVariant = 0;
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

        public void SetType(TerrainMainType main, int under, int value)
        {
            mainTerrain = main;
            subTerrain = under;
            terrainValue = value;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(color.R);
            w.Write(color.G);
            w.Write(color.B);
            w.Write(groundY);
            w.Write((byte)mainTerrain);
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            byte rValue = r.ReadByte();
            byte gValue = r.ReadByte();
            byte bValue = r.ReadByte();
            color = new Color(rValue, gValue, bValue);
            groundY = r.ReadSingle();
            mainTerrain = (TerrainMainType)r.ReadByte();
        }
    }

}
