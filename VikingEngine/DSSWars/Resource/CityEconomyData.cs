using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Resource
{
    struct CityEconomyData
    {
        public int taxIncome_copp;
        public int workerCount;
        public int servicemenUpkeep_copp;
        public int cityGuardUpkeep_copp;
        public float blackMarketCosts_Food_gold;
        public float animalPenUpkeep;
        //public int nobelMenCosts_copp;
        public CityEconomyData(City city)
        {
            taxIncome_copp = (int)tax(city, out _);
            servicemenUpkeep_copp = city.workingAndFreeServiceMen * DssConst.UpkeepPerServiceMan_copp;
            cityGuardUpkeep_copp = city.soldiersCount * DssConst.UpkeepPerGuard_copp;
            animalPenUpkeep = (float)city.PenFoodUpkeep_minute / TimeExt.MinuteInSeconds;
            //nobelMenCosts_copp = DssConst.NobleHouseUpkeep_copp * city.buildingStructure.Nobelhouse_count;
        }

        public float tax(City city, out float taxPerWorker_copp)
        {
            taxPerWorker_copp = DssConst.TaxPerWorker_copp;
            if (city != null)
            {
                if (city.GetCasual())
                {
                    taxPerWorker_copp = DssConst.Casual_TaxPerWorker_copp;

                    switch (city.casualCityProfile.unlock_farming)
                    {
                        case 1:
                            taxPerWorker_copp += DssConst.Casual_Farm2TaxIncreasePercUnits_copp;
                            break;
                        case 2:
                            taxPerWorker_copp += DssConst.Casual_Farm3TaxIncreasePercUnits_copp;
                            break;
                    }
                }
                else
                {
                    if (city.buildingStructure.Bank_count > 0)
                    {
                        taxPerWorker_copp += DssConst.BankTaxIncreasePercUnits_copp;
                    }
                    if (city.cityCulture == CityCulture.Lawbiding)
                    {
                        taxPerWorker_copp *= 2f;
                    }
                }
            }
            else
            { 
                return workerCount *  taxPerWorker_copp;
            }

            return city.workForce.amount * taxPerWorker_copp;
        }

        public int IncomeAndUpkeep_Total()
        { 
            return taxIncome_copp - servicemenUpkeep_copp - cityGuardUpkeep_copp;
        }

        public int IncomeAndUpkeep_Total_Casual()
        {
            return taxIncome_copp - cityGuardUpkeep_copp;
        }

        //public int total(City city)
        //{
        //    return Convert.ToInt32(Math.Floor(tax(city) - cityGuardUpkeep_copp));
        //}

        public void Add(CityEconomyData add)
        {
            taxIncome_copp += add.taxIncome_copp;
            workerCount += add.workerCount;
            servicemenUpkeep_copp += add.servicemenUpkeep_copp;
            cityGuardUpkeep_copp += add.cityGuardUpkeep_copp;
            //nobelMenCosts_copp += add.nobelMenCosts_copp;
            blackMarketCosts_Food_gold += add.blackMarketCosts_Food_gold;
        }
    }
}
