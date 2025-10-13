using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.LootFest.Map;

namespace VikingEngine.DSSWars.Map
{
    class MapLayer_Factions
    {
        IntVector2 mapsz;
        public Map.FactionPixelTexture factionColorsTex;

        public MapLayer_Factions()
        {
           mapsz = DssRef.world.Size;

            factionColorsTex = new FactionPixelTexture(true);/*vol.Position, vol.Scale*/
        }

        public void asyncTask()
        {
            if (mapsz != DssRef.world.Size)
            { 
                mapsz = DssRef.world.Size;
                factionColorsTex.refreshScale();
            }
            factionColorsTex.RefreshWorld_FactionCol();
        }

        public void syncTask()
        {
            factionColorsTex.SetNewTexture();
        }
    }
}
