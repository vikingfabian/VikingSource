﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.XP
{
    struct TechnologyTemplate
    {
        public static readonly TechnologyTemplate Start = new TechnologyTemplate();

        public const int Unlocked = 200;
        public const int FactionUnlock = 100000;

        public int advancedBuilding;
        public int advancedFarming;
        public int advancedCasting;

        public int iron;
        public int steel;

        public int catapult;
        
        public int blackPowder;
        public int gunPowder;


        public TechnologyTemplate()
        { 
            advancedBuilding = Unlocked - 20;
            advancedFarming = Unlocked - 50;
            advancedCasting = Unlocked - 50;

            iron = Unlocked - 50;
            steel = Unlocked - 100;

            catapult = Unlocked - 50;

            blackPowder = Unlocked - 100;
            gunPowder = Unlocked - 200;
        }

        public static int SetRandom(int startValue, double percentageAdd = 0.5)
        {
            if (startValue > Unlocked)
            {
                return startValue;
            }

            double total = Unlocked - startValue;
            return startValue + (int)(total * percentageAdd * Ref.rnd.Double());
        }


        public static void MultiplyProgress(ref int value, double reduceGap = 0.5)
        {
            int gap = Unlocked - value;
            if (gap > 0) {
                value = value + (int)(gap * reduceGap);
            }
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)Bound.Max(advancedBuilding, Unlocked));
            w.Write((byte)Bound.Max(advancedFarming, Unlocked));
            w.Write((byte)Bound.Max(advancedCasting, Unlocked));
            w.Write((byte)Bound.Max(iron, Unlocked));
            w.Write((byte)Bound.Max(steel, Unlocked));
            w.Write((byte)Bound.Max(catapult, Unlocked));
            w.Write((byte)Bound.Max(blackPowder, Unlocked));
            w.Write((byte)Bound.Max(gunPowder, Unlocked));

        }
        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            advancedBuilding = r.ReadByte();
            advancedFarming = r.ReadByte();
            advancedCasting = r.ReadByte();
            iron = r.ReadByte();
            steel = r.ReadByte();
            catapult = r.ReadByte();
            blackPowder = r.ReadByte();
            gunPowder = r.ReadByte();
        }

        public Unlocks GetUnlocks(bool factionView)
        {
            Unlocks unlocks = new Unlocks();
            int unlockAt = factionView ? 1 : Unlocked;

            if (advancedBuilding >= unlockAt)
            {
                unlocks.UnlockAdvancedBuilding();
            }
            if (advancedFarming >= unlockAt)
            {
                unlocks.UnlockAdvancedFarming();
            }
            if (advancedCasting >= unlockAt)
            {
                unlocks.UnlockAdvancedCasting();
            }
            if (iron >= unlockAt)
            {
                unlocks.UnlockIron();
            }
            if (steel >= unlockAt)
            {
                unlocks.UnlockSteel();
            }
            if (catapult >= unlockAt)
            {
                unlocks.UnlockCatapult();
            }
            if (blackPowder >= unlockAt)
            {
                unlocks.UnlockBlackPowder();
            }
            if (gunPowder >= unlockAt)
            {
                unlocks.UnlockGunPowder();
            }

            return unlocks;
        }

        public void destroyTechOnTakeOver()
        {
            tech(ref advancedBuilding, Start.advancedBuilding);
            tech(ref advancedFarming, Start.advancedFarming);
            tech(ref advancedCasting, Start.advancedCasting);
            tech(ref iron, Start.iron);
            tech(ref steel, Start.steel);
            tech(ref catapult, Start.catapult);
            tech(ref blackPowder, Start.blackPowder);
            tech(ref gunPowder, Start.gunPowder);

            void tech(ref int thisTech, int start)
            {
                if (thisTech > start)
                {
                    int points = thisTech - start;
                    thisTech = start + Math.Min(Ref.rnd.Int(points), Ref.rnd.Int(points));
                }
            }
        }

        public void gainTechSpread(TechnologyTemplate from, int gainSpeed)
        {
            tech(ref advancedBuilding, from.advancedBuilding);
            tech(ref advancedFarming, from.advancedFarming);
            tech(ref advancedCasting, from.advancedCasting);
            tech(ref iron, from.iron);
            if (iron >= Unlocked)
            {
                tech(ref steel, from.steel);
            }
            tech(ref catapult, from.catapult);
            tech(ref blackPowder, from.blackPowder);
            if (blackPowder >= Unlocked)
            {
                tech(ref gunPowder, from.gunPowder);
            }

            void tech(ref int thisTech, int otherTech)
            {
                if (otherTech >= Unlocked && thisTech < Unlocked)
                {
                    thisTech = Bound.Max(thisTech + gainSpeed, Unlocked);
                }
            }
        }

        public void addFactionUnlocked(TechnologyTemplate from, bool toCity, bool includeProgress)
        {
            tech(ref advancedBuilding, from.advancedBuilding);
            tech(ref advancedFarming, from.advancedFarming);
            tech(ref advancedCasting, from.advancedCasting);
            tech(ref iron, from.iron);
            tech(ref steel, from.steel);
            tech(ref catapult, from.catapult);
            tech(ref blackPowder, from.blackPowder);
            tech(ref gunPowder, from.gunPowder);

            void tech(ref int thisTech, int otherTech)
            {
                if (otherTech >= FactionUnlock)
                { 
                    thisTech = toCity? Unlocked : FactionUnlock;
                }
                else if (includeProgress)
                { 
                    thisTech = otherTech;
                }
            }
        }

        public void checkCityCount(int cityCount)
        {
            tech(ref advancedBuilding);
            tech(ref advancedFarming);
            tech(ref advancedCasting);
            tech(ref iron);
            tech(ref steel);
            tech(ref catapult);
            tech(ref blackPowder);
            tech(ref gunPowder);

            void tech(ref int thisTech)
            {
                if (thisTech >= cityCount)
                {
                    thisTech = FactionUnlock;
                }
            }
        }

        public void Add(TechnologyTemplate city)
        {
            advancedBuilding += city.advancedBuilding;
            advancedFarming += city.advancedFarming;
            advancedCasting += city.advancedCasting;
            iron += city.iron;
            steel += city.steel;
            blackPowder += city.blackPowder;
            gunPowder += city.gunPowder;
        }

        public static int PercentProgress(int value)
        {
            return Bound.Max(value, 100);
        }
    }

    
}
