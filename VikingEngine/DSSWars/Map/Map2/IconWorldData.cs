using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.DSSWars.Map.Map2
{
    struct IconWorldDataMeta
    {
        public Guid_8char id;
        public string name;

        public IconWorldDataMeta()
        { 
            id = Guid_8char.NewId();
            name = "iconmap " + id;
        }
    }

    struct CityPlacementData
    {
        public int myIndex;
        public IntVector2 pos;

    }

    class IconWorldData
    {
        

        public WorldMetaData metaData;
        public Grid2D_L<GenTile> iconGrid;
        public List<CityPlacementData> cities = null;
        public PcgRandom rnd;
        const int Version = 1;

        
        public IconWorldData()
        {
            rnd = new PcgRandom(metaData.worldId.seed);

        }

        public IconWorldData(IntVector2 iconSize)
        {
            metaData = new WorldMetaData(Ref.rnd.Ushort(), MapSize.Medium, -1);

            rnd = new PcgRandom(metaData.worldId.seed);

            iconGrid = new Grid2D_L<GenTile>(iconSize);

            //IntVector2 tileSz = iconGrid.Size * 16;

        }
        public void writeIcon(System.IO.BinaryWriter w)
        {
            w.Write(Version);

            iconGrid.Size.writeUshort(w);
            for (int i = 0; i < iconGrid.array.Length; i++)
            {
                iconGrid.array[i].writeIcon(w);
            }
        }

        public void readIcon(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();

            metaData = new WorldMetaData(Ref.rnd.Ushort(), MapSize.Medium, -1);
            iconGrid = new Grid2D_L<GenTile>(IntVector2.FromReadUshort(r));
            for (int i = 0; i < iconGrid.array.Length; i++)
            {
                iconGrid.array[i].readIcon(r);
            }
        }


        public IconWorldData CloneMe()
        {
            IconWorldData clone = new IconWorldData(this.iconGrid.Size);

            clone.metaData = this.metaData;

            if (this.cities != null)
            {
                clone.cities = new List<CityPlacementData>(this.cities);
            }

            clone.rnd = new PcgRandom(this.metaData.worldId.seed);

            clone.iconGrid = this.iconGrid.Clone();

            return clone;
        }
    }
}
