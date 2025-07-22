using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Players.PlayerControls.Casual
{
    struct CasualCityProfile
    {
        /// <summary>
        /// Price for the unit, zero is not available
        /// </summary>
        public int folkmen;
        public int shipmen;
        public int meleeMen;
        public int rangedMen;
        public int riderMen;
        public int siegeMen;

        public ItemResourceType meleeWeapon;
        public ItemResourceType rangedWeapon;
        public ItemResourceType riderWeapon;
        public ItemResourceType siegeWeapon;

        bool armorBonus;

        public void InitCulture(City city, CityAreaCulture culture)
        {
            folkmen = 1;

            if (city.cityType >= CityType.Capital) 
            {
                if (culture.percPlains > 0.2)
                {
                    riderMen = 1;
                    folkmen = 0;
                }
            }

            if (culture.percMountain > 0.1 && culture.percForest > 0.1) 
            { 
                siegeMen = 1;
            }

            if (city.Culture == CityCulture.Seafaring || culture.percWater > 0.5)
            { 
                shipmen = 1;
                siegeMen = 0;
            }

            if (folkmen > 0)
            {
                switch (city.cityType)
                {
                    case CityType.Village:
                        folkmen = 40;
                        break;
                    case CityType.Town:
                        folkmen = 50;
                        break;
                    case CityType.Capital:
                        folkmen = 70;
                        break;
                }

                
            }

            switch (city.Culture)
            {
                case CityCulture.AnimalBreeder:
                case CityCulture.FertileGround:
                case CityCulture.LargeFamilies:
                case CityCulture.Lawbiding:
                case CityCulture.CrabMentality:
                    folkmen -= 20;
                    break;



            }

        }
}
