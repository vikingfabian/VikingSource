using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.Players.PlayerControls
{
    /// <summary>
    /// To be able to toggle between natural resources in a city region
    /// </summary>
    struct TerrainTypeSearch
    {
        int cityIx;
        Rectangle2 area;
        IntVector2 start;
        IntVector2 currentPos;
        SubTile terrainType;

        public TerrainTypeSearch()
        {
            cityIx = -1; 
        }

        public Vector3 FindNext(City city, SubTile terrainType)
        {
            bool newSearch = cityIx != city.myIndex ||
                !this.terrainType.EqualTerrain(terrainType);

            if (newSearch)
            {
                cityIx = city.myIndex;
                this.terrainType = terrainType;

                area = WP.ToSubTilePos(city.cityTileArea);

                currentPos = area.pos;
            }

            start = currentPos;

            do
            {
                if (DssRef.world.subTileGrid.Get(currentPos).EqualTerrain(terrainType))
                {
                    if (DssRef.world.tileGrid.Get(WP.SubtileToTilePos(currentPos)).CityIndex == cityIx)
                    {                        
                        var result = WP.SubtileToWorldPosXZ(currentPos);
                        Next();
                        return result;
                    }
                }
            } while (Next());

            return city.position;
        }

        bool Next()
        {
            currentPos.X++;
            if (currentPos.X > area.Right)
            {
                currentPos.X = area.X;
                currentPos.Y++;
                if (currentPos.Y > area.Bottom)
                {
                    currentPos.Y = area.Y;
                }
            }

            return currentPos != start;
        }

    }
}
