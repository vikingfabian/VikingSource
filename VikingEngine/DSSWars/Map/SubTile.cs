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
        public FoilType foil = FoilType.None;

        public SubTile(Color color, float groundY)
        {
            this.color = color;
            this.groundY = groundY;
        }

        //public SubTile(System.IO.BinaryReader r, int version)
        //{
        //    read(r, version);
        //}

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(color.R);
            w.Write(color.G);
            w.Write(color.B);
            w.Write(groundY);
            w.Write((byte)foil);
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            byte rValue = r.ReadByte();
            byte gValue = r.ReadByte();
            byte bValue = r.ReadByte();
            color = new Color(rValue, gValue, bValue);
            groundY = r.ReadSingle();
            foil = (FoilType)r.ReadByte();
        }
    }

    enum FoilType
    {
        None,
        Tree,
        Stones,
        Num
    }
}
