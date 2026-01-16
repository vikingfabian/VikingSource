
using System;
using System.Collections.Generic;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Map.Generate
{
    class GenerateRegion
    {
        public List<City> cities = new List<City>(16);
        
        public int GetStartFactionRegion(int goalWorkForce, bool oneCity, City startCity, WorldData world, Faction faction)
        {
            cities.Clear();

            int currentWorkforce = 0;
            addCity(startCity);

            if (oneCity)
            {
                return currentWorkforce;
            }

            int checkStartIx = 0;
            int checkEndIx = 0;            

            int loopCount = 0;

            while (++loopCount < 3)
            {
                checkEndIx = cities.Count -1;
                for (int cityIx = checkStartIx; cityIx <= checkEndIx; cityIx++)
                {
                    var city = cities[cityIx];
                    EcsStaticArrayCounter neighbors = new EcsStaticArrayCounter(world.neighborCities, city.myIndex, city.neighborCitiesCount);//cities[cityIx].CityNeighbors();                    
                    while (neighbors.Next(world.cities, out City nCity))//foreach (int n in cities[cityIx].neighborCities)
                    {
                        //City c = world.cities[n];
                        if (nCity.factionIndex < 0 && nCity.cityType > CityType.UnClaimed)
                        {
                            addCity(nCity);

                            if (currentWorkforce >= goalWorkForce)
                            {
                                return currentWorkforce;
                            }
                        }
                    }

                    checkStartIx = checkEndIx +1;
                }
            }

            return currentWorkforce;

            void addCity(City city)
            {
                faction.AddCity(city, true);
                cities.Add(city);
                currentWorkforce += city.HousingCount_Workers;
            }
        }
    }
}
