using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.MapModels;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.LootFest.Map;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars.Map.MapLayer
{
    class MapLayer3_Far : AbsFarLayer
    {
        public UnitsOnMap_FarLayer unitsOnMap;

        public MapLayer3_Far(CrossHeightMap crossHeightMap)
            :base(DrawGame.FarLayer, crossHeightMap)
        {
            unitsOnMap = new UnitsOnMap_FarLayer();
        }
        public void HalfSecondUpdate()
        {
            unitsOnMap.update();
        }
    }
}
