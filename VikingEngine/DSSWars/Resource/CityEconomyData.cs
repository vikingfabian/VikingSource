using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Resource
{
    struct CityEconomyData
    {
        public float tax(City city) 
        {
            float taxPerc = DssConst.TaxPerWorker;
            if (city.buildingStructure.Bank_count > 0)
            {
                taxPerc += DssConst.BankTaxIncreasePercUnits;
            }
            float result = workerCount * taxPerc;
            if (city != null && city.Culture == CityCulture.Lawbiding)
            {
                result *= 2f;
            }

            return result;  
        }
        public int workerCount;
        public float cityGuardUpkeep;
        public float blackMarketCosts_Food;

        public int total(City city)
        {
            return Convert.ToInt32(Math.Floor(tax(city) - cityGuardUpkeep));
        }

        public void Add(CityEconomyData add)
        {
            workerCount += add.workerCount;
            cityGuardUpkeep += add.cityGuardUpkeep;
            blackMarketCosts_Food += add.blackMarketCosts_Food;
        }
    }
}
