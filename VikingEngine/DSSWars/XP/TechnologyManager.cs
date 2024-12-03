using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.XP
{
    class TechnologyManager
    {
        public void asyncOneMinuteUpdate()
        {
            //Collect tech spread

            foreach (var city in DssRef.world.cities)
            {   
                foreach (var ni in city.neighborCities)
                {
                    var nCity = DssRef.world.cities[ni];
                    if (city.faction == nCity.faction)
                    {
                        city.technology.gainTechSpread(nCity.technology, DssConst.TechnologyGain_CitySpread);
                    }
                    else
                    {
                        switch (DssRef.diplomacy.GetRelationType(city.faction, nCity.faction))
                        {
                            case RelationType.RelationType2_Good:
                                city.technology.gainTechSpread(nCity.technology, DssConst.TechnologyGain_GoodRelation_PerMin);
                                break;
                            case RelationType.RelationType3_Ally:
                                city.technology.gainTechSpread(nCity.technology, DssConst.TechnologyGain_AllyRelation_PerMin);
                                break;
                        }
                    }
                }
            }

            //Faction tech overview
            var factionsC = DssRef.world.factions.counter();
            while (factionsC.Next())
            {
                TechnologyTemplate factionTech = new TechnologyTemplate();
                factionTech.addFactionUnlocked(factionsC.sel.technology);
                var citiesC = factionsC.sel.cities.counter();
                while (citiesC.Next())
                {
                    factionTech.Add(citiesC.sel.technology);
                }



                factionsC.sel.technology = factionTech;
            }
        }
    }
}
