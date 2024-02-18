using Microsoft.Xna.Framework;
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
        public SubTileMainType maintype = SubTileMainType.NUM;
        public int undertype = -1;
        /// <summary>
        /// Amount of resources that can be extracted, or other value like building size
        /// </summary>
        public int typeValue = 0;

        public int typeQuality = 0;

        /// <summary>
        /// Pointer to array with all resources found lying on ground
        /// </summary>
        public int collectionPointer = -1;

        public SubTile(SubTileMainType type, Color color, float groundY)
        {
            this.color = color;
            this.groundY = groundY;
            this.maintype = type;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(color.R);
            w.Write(color.G);
            w.Write(color.B);
            w.Write(groundY);
            w.Write((byte)maintype);
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            byte rValue = r.ReadByte();
            byte gValue = r.ReadByte();
            byte bValue = r.ReadByte();
            color = new Color(rValue, gValue, bValue);
            groundY = r.ReadSingle();
            maintype = (SubTileMainType)r.ReadByte();
        }
    }

    enum SubTileMainType
    {
        DefaultLand,
        DefaultSea,

        Foil,
        Terrain,
        Building,
        NUM
    }

    enum SubTileFoilType
    {
        TreeSprout,
        Tree,
        Bush,
        Stones,
        NUM
    }

    enum SubTileTerrainType
    {
        River,
        Sea,
        Rock,
    }
}
