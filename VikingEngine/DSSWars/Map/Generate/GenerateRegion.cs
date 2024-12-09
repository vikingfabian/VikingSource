
using System.Collections.Generic;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Map.Generate
{
    class GenerateRegion
    {
        public List<City> cities = new List<City>(16);
        
        //public Faction tempFaction = new Faction();

        //public void Reset(int goalWorkForce)
        //{
        //    this.goalWorkForce = goalWorkForce;
        //    currentWorkforce = 0;

        //    cities.Clear();
        //}

        


        public int GetStartFactionRegion(int goalWorkForce, City startCity, WorldData world, Faction faction)
        {
            cities.Clear();

            int currentWorkforce = 0;
            addCity(startCity);

            int checkStartIx = 0;
            int checkEndIx = 0;
            

            int loopCount = 0;
            while (++loopCount < 20)
            {
                //checkCities.Clear();
                //checkCities.AddRange(cities);
                checkEndIx = cities.Count -1;
                //foreach (City check in checkCities)
                for (int cityIx = checkStartIx; cityIx <= checkEndIx; cityIx++)
                {
                    foreach (int n in cities[cityIx].neighborCities)
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
                                //faction.refreshMainCity();
                                return currentWorkforce;
                            }
                        }
                    }

                    checkStartIx = checkEndIx +1;
                }
            }

            //faction.refreshMainCity();
            return currentWorkforce;

            void addCity(City city)
            {
                //city.faction = faction;
                faction.AddCity(city, true);
                cities.Add(city);
                currentWorkforce += city.workForceMax;
            }
        }

        //public void ApplyFaction(Faction faction)
        //{
        //    foreach (var c in cities)
        //    {
        //        c.faction = null;
        //        faction.AddCity(c, true);
        //    }
        //}
    }
}
