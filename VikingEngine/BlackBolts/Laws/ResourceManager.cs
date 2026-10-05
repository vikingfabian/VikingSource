using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.Map;

namespace VikingEngine.Core.BlackBolts.Laws
{
    static class ResourceManager
    {
        public static bool CanDispenceResource(IntVector2 tilePos)
        {
            return BlackRef.mapData.tileGrid.TryGet(tilePos, out Tile tile) && tile.canPlaceResource();
        }

        public static void DispenceResource(IntVector2 tilePos, ResourceType resourceType)
        {
            var resource = BlackRef.mapData.SpawnResource(resourceType);
            resource.placeResourceOnFloor(tilePos);
        }
    }
}
