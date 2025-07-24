using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Players.PlayerControls.Casual
{
    enum CasualSoldierType
    { 
        Guard,
        FolkMen,
        Seamen,
        Melee,
        Ranged,
        Rider,
        Siege,
    }

    struct SoldierPurchaseOption
    {
        public int price;
        public ItemResourceType weapon;
        public TrainingLevel training;

        public bool Available => price > 0;
    }

    struct CasualCityProfile
    {
        public SoldierPurchaseOption guard;

        public SoldierPurchaseOption folkmen;
        public SoldierPurchaseOption shipmen;
        public SoldierPurchaseOption meleeMen;
        public SoldierPurchaseOption rangedMen;
        public SoldierPurchaseOption riderMen;
        public SoldierPurchaseOption siegeMen;

        bool armorBonus;

        public void InitCulture(City city, CityAreaCulture culture)
        {
            guard.price = 50;
            guard.weapon = ItemResourceType.Bow;
            guard.training = TrainingLevel.Basic;

            folkmen.price = 1;
            folkmen.weapon = ItemResourceType.SharpStick;
            folkmen.training = TrainingLevel.Minimal;

            meleeMen.price = 1;
            meleeMen.weapon = ItemResourceType.Sword;
            meleeMen.training = TrainingLevel.Basic;

            rangedMen.price = 1;
            rangedMen.weapon = ItemResourceType.Bow;
            rangedMen.training = TrainingLevel.Basic;

            riderMen.weapon = ItemResourceType.KnightsLance;
            rangedMen.training = TrainingLevel.Skillful;

            siegeMen.price = 1;
            siegeMen.weapon = ItemResourceType.Ballista;
            siegeMen.training = TrainingLevel.Basic;

            if (city.cityType >= CityType.Capital)
            {
                if (culture.percPlains > 0.2)
                {
                    riderMen.price = 1;
                    folkmen.price = 0;
                }
            }

            if (culture.percMountain > 0.1 && culture.percForest > 0.1)
            {
                siegeMen.price = 1;
            }

            if (city.Culture == CityCulture.Seafaring || culture.percWater > 0.5)
            {
                shipmen.price = 1;
                siegeMen.price = 0;
            }

            if (folkmen.price > 0)
            {
                switch (city.cityType)
                {
                    case CityType.Village:
                        folkmen.price = 40;
                        break;
                    case CityType.Town:
                        folkmen.price = 50;
                        break;
                    case CityType.Capital:
                        folkmen.price = 70;
                        break;
                }
            }

            if (shipmen.Available) shipmen.price = 200;
            if (meleeMen.Available) meleeMen.price = 200;
            if (rangedMen.Available) rangedMen.price = 200;
            if (riderMen.Available) riderMen.price = 500;
            if (siegeMen.Available) siegeMen.price = 200;

            switch (city.Culture)
            {
                case CityCulture.AnimalBreeder:
                case CityCulture.FertileGround:
                case CityCulture.LargeFamilies:
                case CityCulture.Lawbiding:
                case CityCulture.CrabMentality:
                    folkmen.price = Math.Max(0, folkmen.price - 20);
                    break;

                case CityCulture.BronzeCasters:
                    meleeMen.weapon = ItemResourceType.BronzeSword;
                    break;

                case CityCulture.Archers:
                    folkmen.weapon = ItemResourceType.SlingShot;
                    break;
            }
        }
    }

}
