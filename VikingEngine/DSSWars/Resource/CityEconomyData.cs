using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
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
        //public int nobelMenCosts_copp;
        public CityEconomyData(City city)
        {
            taxIncome_copp = (int)tax(city);
            servicemenUpkeep_copp = city.workingAndFreeServiceMen * DssConst.UpkeepPerServiceMan_copp;
            cityGuardUpkeep_copp = city.soldiersCount * DssConst.UpkeepPerGuard_copp;
            //nobelMenCosts_copp = DssConst.NobleHouseUpkeep_copp * city.buildingStructure.Nobelhouse_count;
        }

        public float tax(City city) 
        {
            float taxPerc = DssConst.TaxPerWorker_copp;
            if (city != null)
            {
                if (city.GetCasual())
                {
                    switch (city.GetCasualProgress().unlock_farming)
                    {
                        case 1:
                            taxPerc += DssConst.Casual_Farm2TaxIncreasePercUnits;
                            break;
                        case 2:
                            taxPerc += DssConst.Casual_Farm3TaxIncreasePercUnits;
                            break;
                    }
                }
                else
                {
                    if (city.buildingStructure.Bank_count > 0)
                    {
                        taxPerc += DssConst.BankTaxIncreasePercUnits;
                    }
                    if (city.Culture == CityCulture.Lawbiding)
                    {
                        taxPerc *= 2f;
                    }
                }
            }
            else
            { 
                return workerCount *  taxPerc;
            }

            return city.workForce.amount * taxPerc;
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
