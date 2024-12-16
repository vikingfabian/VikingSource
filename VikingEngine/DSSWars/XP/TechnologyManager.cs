using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.XP
{
    class TechnologyManager
    {
        /// <remark>
        /// Players must be fully initialized
        /// </remark>
        public void initGame(bool newGame)
        {
            if (newGame)
            {
                Task.Factory.StartNew(() =>
                {
                    var factionsCounter = DssRef.world.factions.counter();
                    while (factionsCounter.Next())
                    {
                        var citiesC = factionsCounter.sel.cities.counter();
                        while (citiesC.Next())
                        {
                            citiesC.sel.technology.addFactionUnlocked(factionsCounter.sel.technology, true, true);
                        }
                    }

                    asyncOneMinuteUpdate(false);
                });
            }
        }

        public void asyncOneMinuteUpdate(bool runSpread)
        {
            //Collect tech spread

            if (runSpread)
            {
                foreach (var city in DssRef.world.cities)
                {
                    if (city.debugTagged)
                    {
                        lib.DoNothing();
                    }
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
            }

            //Faction tech overview
            //Apply unlock to work
            var factionsC = DssRef.world.factions.counter();
            while (factionsC.Next())
            {
                TechnologyTemplate factionTech = new TechnologyTemplate();
                factionTech.addFactionUnlocked(factionsC.sel.technology, false, false);


                var citiesC = factionsC.sel.cities.counter();
                while (citiesC.Next())
                {
                    citiesC.sel.workTemplate.applyUnlock(citiesC.sel.technology.GetUnlocks(false));
                    factionTech.Add(citiesC.sel.technology);
                }

                factionsC.sel.technology = factionTech;
                factionsC.sel.workTemplate.applyUnlock(factionTech.GetUnlocks(true));
            }
        }
    }
}
