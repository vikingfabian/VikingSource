using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Map
{
    /// <summary>
    /// To be able to toggle between natural resources in a city region
    /// </summary>
    class TerrainTypeCollection
    {
        public City city = null;
        Rectangle2 area;
        IntVector2 start;
        IntVector2 currentPos;
        SubTile terrainType;

        public Vector3 FindNext(City city, SubTile terrainType)
        {
            bool newSearch = this.city != city ||
                this.terrainType.mainTerrain != terrainType.mainTerrain ||
                this.terrainType.subTerrain != terrainType.subTerrain;



        }

    }
}
