using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.XP
{
    class ResearchMenu
    {
        City city;
        LocalPlayer player;
        public void ToHud(City city, LocalPlayer player, RichBoxContent content)
        {
            this.city = city;
            this.player = player;

            if (arraylib.InBound(city.researchBuildings, city.selectedResearchBuilding))
            {

            }
            else
            {
                //List buildings
                if (arraylib.HasMembers(city.researchBuildings))
                {

                }
            }
        }
    }
}
