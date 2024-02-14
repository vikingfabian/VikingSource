
using System.Collections.Generic;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Map.Generate
{
    class GenerateRegion
    {
        public List<City> cities = new List<City>(16);
        public int goalWorkForce, currentWorkforce;
        public Faction tempFaction = new Faction();

        public void Reset(int goalWorkForce)
        {
            this.goalWorkForce = goalWorkForce;
            currentWorkforce = 0;

            cities.Clear();
        }

        void addCity(City city)
        {
            city.faction = tempFaction;
            cities.Add(city);
            currentWorkforce += city.workForce.max;
        }


        public void GetStartFactionRegion(City startCity, WorldData world)
        {
            //if (startCity.faction == null)
            //{
            //faction = new Faction(factions.Count);
            //factions.Add(faction);
            addCity(startCity);


            List<City> checkCities = new List<City>(8);

            int loopCount = 0;
            while (++loopCount < 20)
            {
                checkCities.Clear();
                checkCities.AddRange(cities);

                foreach (City check in checkCities)
                {
                    foreach (int n in check.neighborCities)
                    {
                        //if (!arraylib.InBound(world.cities, n))
                        //{ 
                        //    lib.DoNothing();
                        //}
                        City c = world.cities[n];
                        if (c.faction == null)
                        {
                            addCity(c);

                            if (currentWorkforce >= goalWorkForce)
                            {
                                //faction.availableForPlayer = true;
                                return;
                            }
                        }
                    }
                }
            }
            //}
        }

        public void ApplyFaction(Faction faction)
        {
            foreach (var c in cities)
            {
                c.faction = null;
                faction.AddCity(c, true);
            }
        }
    }
}
