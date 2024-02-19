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
        Map.FactionColorsTexture factionColorsTex;

        public MapLayer_Factions()
        {
           var vol =  MapLayer_Overview.WaterModelVolume();
           factionColorsTex = new FactionColorsTexture(vol.Position, vol.Scale);
        }

        public void asyncTask()
        {
            factionColorsTex.quedEvent();
        }

        public void syncTask()
        {
            factionColorsTex.SetNewTexture();
        }
    }
}
