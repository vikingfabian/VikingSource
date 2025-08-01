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
        public int upgradePrice;
        public ItemResourceType armor;
        public ItemResourceType weapon;
        public TrainingLevel training;

        public SoldierPurchaseOption(int price,
            ItemResourceType armor, ItemResourceType weapon, TrainingLevel training)
        { 
            this.price = price;
            upgradePrice = 0;
            this.armor = armor;
            this.weapon = weapon;
            this.training = training;
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((ushort)price);
            w.Write((byte)weapon);
        }
        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            price = r.ReadUInt16();
            weapon = (ItemResourceType)r.ReadByte();
        }

        public int FullPrice => price + upgradePrice;

        public bool Available => price > 0;

        public SoldierConscriptProfile SoldierProfile()
        {
            SoldierConscriptProfile soldierConscript = new SoldierConscriptProfile()
            {
                conscript = new ConscriptProfile() { weapon = weapon },
                
            };

            return soldierConscript;
        }

        public void ButtonVisuals(CasualSoldierType soldierType, out SpriteName icon, out string caption)
        {
            if (soldierType == CasualSoldierType.Guard)
            {
                icon = SpriteName.WarsGuard;
                caption = DssRef.lang.Conscript_Soldiers_GuardType;
            }
            else
            {
                var profile = SoldierProfile();
                icon = profile.Icon();
                caption = profile.conscript.TypeName();
            }
        }

        
    }

    struct CasualCityProfile
    {
        public int maxHuts;
        public SoldierPurchaseOption guard;

        public SoldierPurchaseOption folkmen;
        public SoldierPurchaseOption shipmen;
        public SoldierPurchaseOption meleeMen;
        public SoldierPurchaseOption rangedMen;
        public SoldierPurchaseOption riderMen;
        public SoldierPurchaseOption siegeMen;

        //bool armorBonus;

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
        }

        public void InitCulture(City city, CityAreaCulture culture)
        {
            guard = new SoldierPurchaseOption(50, ItemResourceType.PaddedArmor, ItemResourceType.Bow, TrainingLevel.Basic);
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

        public void refreshTech(CityCasualProgress progress)
        {
            guard.upgradePrice = 0;
            meleeMen.upgradePrice = 0;
            rangedMen.upgradePrice = 0;
            siegeMen.upgradePrice = 0;

            switch (progress.unlock_armor)
            {
                case 1:
                    guard.armor = ItemResourceType.HeavyIronArmor;
                    guard.upgradePrice += 50;

                    meleeMen.armor = ItemResourceType.HeavyIronArmor;
                    meleeMen.upgradePrice += 100;

                    rangedMen.armor = ItemResourceType.IronArmor;
                    rangedMen.upgradePrice += 100;
                    break;


                case 2:
                    guard.armor = ItemResourceType.FullPlateArmor;
                    guard.upgradePrice += 10;

                    meleeMen.armor = ItemResourceType.FullPlateArmor;
                    meleeMen.upgradePrice += 200;

                    rangedMen.armor = ItemResourceType.LightPlateArmor;
                    rangedMen.upgradePrice += 200;
                    break;
            }

            switch (progress.unlock_sword)
            {
                case 1:
                    meleeMen.weapon = ItemResourceType.Sword;
                    meleeMen.upgradePrice += 100;
                    break;
                case 2:
                    meleeMen.weapon = ItemResourceType.LongSword;
                    meleeMen.upgradePrice += 200;
                    break;
            }

            switch (progress.unlock_projectile)
            {
                case 1:
                    rangedMen.weapon = ItemResourceType.Crossbow;
                    rangedMen.upgradePrice += 100;

                    siegeMen.weapon = ItemResourceType.Catapult;
                    siegeMen.upgradePrice += 50;
                    break;

                case 2:
                    rangedMen.weapon = ItemResourceType.HandCulverin;
                    rangedMen.upgradePrice += 200;

                    siegeMen.weapon = ItemResourceType.ManCannonBronze;
                    siegeMen.upgradePrice += 150;
                    break;

                case 3:
                    rangedMen.weapon = ItemResourceType.Rifle;
                    rangedMen.upgradePrice += 300;

                    siegeMen.weapon = ItemResourceType.ManCannonIron;
                    siegeMen.upgradePrice += 250;
                    break;
            }
        }
    }

}
