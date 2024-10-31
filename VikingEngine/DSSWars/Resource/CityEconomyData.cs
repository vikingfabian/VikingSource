using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Resource
{
    struct CityEconomyData
    {
        public float tax() { return workerCount * DssConst.TaxPerWorker; }
        public int workerCount;
        public float cityGuardUpkeep;
        public float blackMarketCosts_Food;

        public int total()
        {
            return Convert.ToInt32(Math.Floor(tax() - cityGuardUpkeep));
        }

        public void Add(CityEconomyData add)
        {
            workerCount += add.workerCount;
            cityGuardUpkeep += add.cityGuardUpkeep;
            blackMarketCosts_Food += add.blackMarketCosts_Food;
        }
    }
}
