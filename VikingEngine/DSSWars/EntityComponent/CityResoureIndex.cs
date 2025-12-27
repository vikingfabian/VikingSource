using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.EntityComponent
{
    static class CityResoureIndex
    {
        // Indices for per-city resources
        public const int wood = 0;
        public const int fuel = 1;
        public const int water = 2;
        public const int stone = 3;
        public const int rawFood = 4;
        public const int food = 5;
        public const int beer = 6;
        public const int coolingfluid = 7;
        public const int skinLinnen = 8;

        // Ores
        public const int ironore = 9;
        public const int TinOre = 10;
        public const int CopperOre = 11;
        public const int LeadOre = 12;
        public const int SilverOre = 13;
        public const int GoldOre = 14;

        // Refined / materials
        public const int iron = 15;
        public const int Tin = 16;
        public const int Copper = 17;
        public const int Lead = 18;
        public const int Silver = 19;
        public const int RawMithril = 20;
        public const int Sulfur = 21;

        // Alloys / specials
        public const int Bronze = 22;
        public const int Steel = 23;
        public const int CastIron = 24;
        public const int BloomeryIron = 25;
        public const int Mithril = 26;

        // Tools / components / melee
        public const int Palisade = 27;
        public const int Toolkit = 28;
        public const int Wagon2Wheel = 29;
        public const int Wagon4Wheel = 30;
        public const int BlackPowder = 31;
        public const int GunPowder = 32;
        public const int LedBullet = 33;
        public const int sharpstick = 34;
        public const int BronzeSword = 35;
        public const int shortsword = 36;
        public const int Sword = 37;
        public const int LongSword = 38;
        public const int HandSpear = 39;
        public const int MithrilSword = 40;

        // More weapons (melee/ranged)
        public const int Warhammer = 41;
        public const int twohandsword = 42;
        
        public const int SlingShot = 44;
        public const int ThrowingSpear = 45;
        public const int bow = 46;
        public const int longbow = 47;
        public const int crossbow = 48;
        public const int MithrilBow = 49;

        // Early firearms
        public const int HandCannon = 50;
        public const int HandCulvertin = 51;
        public const int Rifle = 52;
        public const int Blunderbuss = 53;

        // Siege
        public const int BatteringRam = 54;
        public const int ballista = 55;
        public const int Manuballista = 56;
        public const int Catapult = 57;
        public const int SiegeCannonBronze = 58;
        public const int ManCannonBronze = 59;
        public const int SiegeCannonIron = 60;
        public const int ManCannonIron = 61;

        // Armor
        public const int paddedArmor = 62;
        public const int HeavyPaddedArmor = 63;
        public const int BronzeArmor = 64;
        public const int mailArmor = 65;
        public const int heavyMailArmor = 66;
        public const int LightPlateArmor = 67;
        public const int FullPlateArmor = 68;
        public const int MithrilArmor = 69;

        // --- NEW ADDITIONS ---

        // New Food & Materials
        public const int Meat = 70;
        public const int ConservedFood = 71;
        public const int Clay = 72;
        public const int Brick = 73;
        public const int Salt = 74;

        // New Wagons
        public const int WagonClosed = 75;
        public const int WagonIron = 76;
        public const int WagonSteel = 77;

        // Shields
        public const int BucklerShield = 78;
        public const int RoundShield = 79;
        public const int HeaterShield = 80;
        public const int TowerShield = 81;

        // Mount Armor
        public const int MountBronzeArmor = 82;
        public const int MountPaddedArmor = 83;
        public const int MountHeavyPaddedArmor = 84;
        public const int MountIronArmor = 85;
        public const int MountHeavyIronArmor = 86;
        public const int MountLightPlateArmor = 87;
        public const int MountFullPlateArmor = 88;
        public const int MountMithrilArmor = 89;

        // Mounts - Oxen
        public const int Oxen = 90;
        public const int KineOxen = 91;

        // Mounts - Horses
        public const int Pony = 92;
        public const int Horse = 93;
        public const int WarHorse = 94;
        public const int DraftHorse = 95;

        // Mounts - Pigs
        public const int WildPig = 96;
        public const int WildHog = 97;
        public const int WarHog = 98;
        public const int StagHog = 99;

        // Mounts - Wolves
        public const int Wolf = 100;
        public const int Warg = 101;
        public const int AlphaWarg = 102;

        // Mounts - Cats
        public const int WildCat = 103;
        public const int Lion = 104;
        public const int WarLion = 105;

        // Mounts - Elephants
        public const int Elephant = 106;
        public const int WarElephant = 107;
        public const int Oliphant = 108;

        public const int Dog = 109;
        public const int Hound = 110;
        
        public const int Container = 111;

        public const int Pig = 112;
        public const int Hen = 113;

        // Reserve some indices (Increased from 80 to 120)
        public const int COUNT = 120;
    }
}
