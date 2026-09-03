using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map.Map2
{
    struct WorldMetaData2
    {
        public ushort seed;

        public WorldMetaData2()
        { 
            seed = Ref.rnd.Ushort();
        }
    }
    struct CityPlacementData
    {
        public int myIndex;
        public IntVector2 pos;

    }

    class IconWorldData
    {
        public WorldMetaData2 metaData2;
        public Grid2D_L<GenTile> iconGrid;
        public List<CityPlacementData> cities = null;

        public PcgRandom rnd;

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

    class WorldData2
    {
        public WorldMetaData2 metaData2;
        public Grid2D_L<GenTile> tileGrid;
        public PcgRandom rnd;

        public WorldData2(IconWorldData icon)
        {
            metaData2 = icon.metaData2;

            rnd = new PcgRandom(metaData2.seed);

            IntVector2 tileSz = icon.iconGrid.Size * 16;
            
        }

    }
}
