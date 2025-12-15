using Microsoft.Xna.Framework.Content;
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
    struct CasualCityProfile
    {
        public const int Projectile1_Catapult = 1;
        public const int Projectile2_BlackPowder = 2;
        public const int ProjectileMax_GunPowder = 3;

        public const int ArmorMax_Steel = 2;
        public const int SwordMax_Steel = 2;
        public const int FarmingMax = 2;

        public int maxHuts;
        public SoldierPurchaseOption guard;

        public SoldierPurchaseOption folkmen;
        public SoldierPurchaseOption shipmen;
        public SoldierPurchaseOption meleeMen;
        public SoldierPurchaseOption rangedMen;
        public SoldierPurchaseOption riderMen;
        public SoldierPurchaseOption siegeMen;

        public bool unlock_logistics;
        public bool unlock_research;
        public int unlock_armor;
        public int unlock_sword;
        public int unlock_projectile;
        public int unlock_farming;

        public void onCasualUpgrade()
        {
            if (unlock_armor == ArmorMax_Steel &&
                unlock_sword == SwordMax_Steel &&
                unlock_projectile == ProjectileMax_GunPowder &&
                unlock_farming == FarmingMax)
            {
                DssRef.achieve.UnlockAchievement(AchievementIndex.maxout_casual);
            }
        }

        public void availableBuildings(City city, out List<CasualBuildType> available,out List<CasualBuildType> complete)
        {
            available = new List<CasualBuildType>(8);
            complete = new List<CasualBuildType>(8);

            if (unlock_logistics)
            {
                available.Add(CasualBuildType.Tent);
            }
            available.Add(CasualBuildType.WorkerHut);
            available.Add(CasualBuildType.Barracks);

            available.Add(CasualBuildType.GuardTower_Wood);

            if (unlock_logistics)
            {
                complete.Add(CasualBuildType.Logistics);
                available.Add(CasualBuildType.GuardTower_Stone);

                if (city.buildingStructure.Embassy_count == 0)
                {
                    available.Add(CasualBuildType.Embassy);
                }
                else
                {
                    complete.Add(CasualBuildType.Embassy);
                }

                if (unlock_research)
                {
                    complete.Add(CasualBuildType.ResearchCenter);

                    switch (unlock_armor)
                    {
                        case 0:
                            available.Add(CasualBuildType.UnlockIronArmor);
                            break;
                        case 1:
                            complete.Add(CasualBuildType.UnlockIronArmor);
                            available.Add(CasualBuildType.UnlockSteelArmor);
                            break;
                        default:
                            complete.Add(CasualBuildType.UnlockIronArmor);
                            complete.Add(CasualBuildType.UnlockSteelArmor);
                            break;
                    }

                    switch (unlock_sword)
                    {
                        case 0:
                            available.Add(CasualBuildType.UnlockSword);
                            break;
                        case 1:
                            complete.Add(CasualBuildType.UnlockSword);
                            available.Add(CasualBuildType.UnlockSteelSword);
                            break;
                        default:
                            complete.Add(CasualBuildType.UnlockSword);
                            complete.Add(CasualBuildType.UnlockSteelSword);
                            break;
                    }

                    switch (unlock_projectile)
                    {
                        case 0:
                            available.Add(CasualBuildType.UnlockCatapult);
                            break;
                        case 1:
                            complete.Add(CasualBuildType.UnlockCatapult);
                            available.Add(CasualBuildType.UnlockBlackPower);
                            break;
                        case 2:
                            complete.Add(CasualBuildType.UnlockCatapult);
                            complete.Add(CasualBuildType.UnlockBlackPower);
                            available.Add(CasualBuildType.UnlockGunPower);
                            break;
                        default:
                            complete.Add(CasualBuildType.UnlockCatapult);
                            complete.Add(CasualBuildType.UnlockBlackPower);
                            complete.Add(CasualBuildType.UnlockGunPower);
                            break;
                    }

                    switch (unlock_farming)
                    {
                        case 0:
                            available.Add(CasualBuildType.UnlockFarming2);
                            break;
                        case 1:
                            complete.Add(CasualBuildType.UnlockFarming2);
                            available.Add(CasualBuildType.UnlockFarming3);
                            break;
                        default:
                            complete.Add(CasualBuildType.UnlockFarming2);
                            complete.Add(CasualBuildType.UnlockFarming3);
                            break;
                    }
                }
                else
                {
                    available.Add(CasualBuildType.ResearchCenter);
                }
            }
            else
            {
                available.Add(CasualBuildType.Logistics);
            }
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)maxHuts);
            guard.writeGameState(w);
            folkmen.writeGameState(w);
            shipmen.writeGameState(w);
            meleeMen.writeGameState(w);
            rangedMen.writeGameState(w);
            riderMen.writeGameState(w);
            siegeMen.writeGameState(w);

            var bools = new EightBit(unlock_logistics, unlock_research);
            bools.write(w);

            TwoHalfByte armor_sword = new TwoHalfByte(unlock_armor, unlock_sword);
            armor_sword.write(w);

            TwoHalfByte projectile_farming = new TwoHalfByte(unlock_projectile, unlock_farming);
            projectile_farming.write(w);
        }

        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            maxHuts = r.ReadByte();
            guard.readGameState(r, subversion);
            folkmen.readGameState(r, subversion);
            shipmen.readGameState(r, subversion);
            meleeMen.readGameState(r, subversion);
            rangedMen.readGameState(r, subversion);
            riderMen.readGameState(r, subversion);
            siegeMen.readGameState(r, subversion);

            var bools = new EightBit(r);
            bools.Get(out unlock_logistics, out unlock_research);

            TwoHalfByte armor_sword = TwoHalfByte.FromStream(r);
            unlock_armor = armor_sword.Value1;
            unlock_sword = armor_sword.Value2;

            TwoHalfByte projectile_farming = TwoHalfByte.FromStream(r);
            unlock_projectile = projectile_farming.Value1;
            unlock_farming = projectile_farming.Value2;

            refreshTech();
        }

        public void InitCulture(City city, CityAreaCulture culture)
        {
            guard = new SoldierPurchaseOption(300, ItemResourceType.PaddedArmor, ItemResourceType.Bow, TrainingLevel.Basic);
            folkmen = new SoldierPurchaseOption(1, ItemResourceType.NONE,ItemResourceType.SharpStick, TrainingLevel.Minimal);
            meleeMen = new SoldierPurchaseOption(1, ItemResourceType.HeavyPaddedArmor, ItemResourceType.ShortSword, TrainingLevel.Basic);
            rangedMen = new SoldierPurchaseOption(1, ItemResourceType.PaddedArmor, ItemResourceType.Bow, TrainingLevel.Basic);
            riderMen = new SoldierPurchaseOption(0, ItemResourceType.IronArmor, ItemResourceType.KnightsLance, TrainingLevel.Skillful);
            siegeMen = new SoldierPurchaseOption(1, ItemResourceType.NONE, ItemResourceType.Ballista, TrainingLevel.Basic);

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
                shipmen = new SoldierPurchaseOption(  1, ItemResourceType.PaddedArmor, ItemResourceType.ThrowingSpear, TrainingLevel.Basic);
                
                siegeMen.price = 0;
            }

            if (folkmen.price > 0)
            {
                switch (city.cityType)
                {
                    case CityType.Village:
                        folkmen.price = 250;
                        break;
                    case CityType.Town:
                        folkmen.price = 300;
                        break;
                    case CityType.Capital:
                        folkmen.price = 500;
                        break;
                }
            }

            if (shipmen.Available) shipmen.price = 1200;
            if (meleeMen.Available) meleeMen.price = 1200;
            if (rangedMen.Available) rangedMen.price = 1200;
            if (riderMen.Available) riderMen.price = 3000;
            if (siegeMen.Available) siegeMen.price = 1200;

            switch (city.Culture)
            {
                case CityCulture.AnimalBreeder:
                case CityCulture.FertileGround:
                case CityCulture.LargeFamilies:
                case CityCulture.Lawbiding:
                case CityCulture.CrabMentality:
                    folkmen.price = Math.Max(0, folkmen.price - 40);
                    break;

                case CityCulture.BronzeCasters:
                    meleeMen.weapon = ItemResourceType.BronzeSword;
                    break;

                case CityCulture.Archers:
                    folkmen.weapon = ItemResourceType.SlingShot;
                    break;
            }
        }

        public void refreshTech()
        {
            guard.upgradePrice = 0;
            meleeMen.upgradePrice = 0;
            rangedMen.upgradePrice = 0;
            siegeMen.upgradePrice = 0;

            switch (unlock_armor)
            {
                case 1:
                    guard.armor = ItemResourceType.HeavyIronArmor;
                    guard.upgradePrice += 300;

                    meleeMen.armor = ItemResourceType.HeavyIronArmor;
                    meleeMen.upgradePrice += 600;

                    rangedMen.armor = ItemResourceType.IronArmor;
                    rangedMen.upgradePrice += 600;
                    break;


                case 2:
                    guard.armor = ItemResourceType.FullPlateArmor;
                    guard.upgradePrice += 600;

                    meleeMen.armor = ItemResourceType.FullPlateArmor;
                    meleeMen.upgradePrice += 1200;

                    rangedMen.armor = ItemResourceType.LightPlateArmor;
                    rangedMen.upgradePrice += 1800;
                    break;
            }

            switch (unlock_sword)
            {
                case 1:
                    meleeMen.weapon = ItemResourceType.Sword;
                    meleeMen.upgradePrice += 600;
                    break;
                case 2:
                    meleeMen.weapon = ItemResourceType.LongSword;
                    meleeMen.upgradePrice += 1200;
                    break;
            }

            switch (unlock_projectile)
            {
                case 1:
                    guard.weapon = ItemResourceType.Crossbow;
                    guard.upgradePrice += 150;

                    rangedMen.weapon = ItemResourceType.Crossbow;
                    rangedMen.upgradePrice +=600;

                    siegeMen.weapon = ItemResourceType.Catapult;
                    siegeMen.upgradePrice += 300;
                    break;

                case 2:
                    guard.weapon = ItemResourceType.HandCulverin;
                    guard.upgradePrice += 300;

                    rangedMen.weapon = ItemResourceType.HandCulverin;
                    rangedMen.upgradePrice += 1200;

                    siegeMen.weapon = ItemResourceType.ManCannonBronze;
                    siegeMen.upgradePrice += 900;
                    break;

                case 3:
                    guard.weapon = ItemResourceType.Rifle;
                    guard.upgradePrice += 600;

                    rangedMen.weapon = ItemResourceType.Rifle;
                    rangedMen.upgradePrice += 1800;

                    siegeMen.weapon = ItemResourceType.ManCannonIron;
                    siegeMen.upgradePrice += 1500;
                    break;
            }
        }
    }

}
