using System;
using System.Collections.Generic;
using System.Text;
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

    class IconWorldData
    {
        public WorldMetaData2 metaData2;
        public Grid2D_L<GenTile> iconGrid;
        public List<CityPlacementData> cities = null;
        public PcgRandom rnd;
        const int Version = 1;

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

            metaData2 = new WorldMetaData2();
            iconGrid = new Grid2D_L<GenTile>(IntVector2.FromReadUshort(r));
            for (int i = 0; i < iconGrid.array.Length; i++)
            {
                iconGrid.array[i].readIcon(r);
            }
        }

        public IconWorldData(IntVector2 iconSize)
        {
            metaData2 = new WorldMetaData2();

            rnd = new PcgRandom(metaData2.seed);

            iconGrid = new Grid2D_L<GenTile>(iconSize);

            IntVector2 tileSz = iconGrid.Size * 16;

        }

        public IconWorldData CloneMe()
        {
            IconWorldData clone = new IconWorldData(this.iconGrid.Size);

            clone.metaData2 = this.metaData2;

            if (this.cities != null)
            {
                clone.cities = new List<CityPlacementData>(this.cities);
            }

            clone.rnd = new PcgRandom(this.metaData2.seed);

            clone.iconGrid = this.iconGrid.Clone();

            return clone;
        }
    }
}
