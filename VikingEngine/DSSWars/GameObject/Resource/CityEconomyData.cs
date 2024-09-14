﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    struct CityEconomyData
    {
        public double tax;
        public float cityGuardUpkeep;
        public float blackMarketCosts_Food;
         
        public int total()
        {
            return Convert.ToInt32(Math.Floor(tax - cityGuardUpkeep));
        }

        public void Add(CityEconomyData add)
        { 
            this.tax += add.tax;
            this.cityGuardUpkeep += add.cityGuardUpkeep;
            this.blackMarketCosts_Food += add.blackMarketCosts_Food;
        }
    }
}