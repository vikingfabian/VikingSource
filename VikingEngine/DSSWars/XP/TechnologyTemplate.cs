using System;
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
        //public static readonly TechnologyTemplate Start = new TechnologyTemplate();

        public const int FactionUnlock = 100000;

        public const int AdvancedBuildingUnlock = 20;
        public const int AdvancedFarmingUnlock = 50;
        public const int AdvancedCastingUnlock = 150;
        public const int IronUnlock = 100;
        public const int SteelUnlock = 150;
        public const int CatapultUnlock = 120;
        public const int BlackPowderUnlock = 180;
        public const int GunPowderUnlock = 200;

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
            advancedBuilding = 0;
            advancedFarming = 0;
            advancedCasting = 0;
            iron = 0;
            steel = 0;
            catapult = 0;
            blackPowder = 0;
            gunPowder = 0;
        }

        public void zero()
        {
            advancedBuilding = 0;
            advancedFarming = 0;
            advancedCasting = 0;
            iron = 0;
            steel = 0;
            catapult = 0;
            blackPowder = 0;
            gunPowder = 0;
        }

        public static int SetRandom(int startValue, int unlock, double percentageAdd = 0.5)
        {
            if (startValue > unlock)
                return startValue;

            double total = unlock - startValue;
            return startValue + (int)(total * percentageAdd * Ref.rnd.Double());
        }

        public static void MultiplyProgress(ref int value, int unlock, double reduceGap = 0.5)
        {
            int gap = unlock - value;
            if (gap > 0)
            {
                value = value + (int)(gap * reduceGap);
            }
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)Bound.Max(advancedBuilding, AdvancedBuildingUnlock));
            w.Write((byte)Bound.Max(advancedFarming, AdvancedFarmingUnlock));
            w.Write((byte)Bound.Max(advancedCasting, AdvancedCastingUnlock));
            w.Write((byte)Bound.Max(iron, IronUnlock));
            w.Write((byte)Bound.Max(steel, SteelUnlock));
            w.Write((byte)Bound.Max(catapult, CatapultUnlock));
            w.Write((byte)Bound.Max(blackPowder, BlackPowderUnlock));
            w.Write((byte)Bound.Max(gunPowder, GunPowderUnlock));
        }

        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            if (subversion < 61)
            { 
                readGameState_old(r);
                return;
            }
            advancedBuilding = r.ReadByte();
            advancedFarming = r.ReadByte();
            advancedCasting = r.ReadByte();
            iron = r.ReadByte();
            steel = r.ReadByte();
            catapult = r.ReadByte();
            blackPowder = r.ReadByte();
            gunPowder = r.ReadByte();
        }
        public void readGameState_old(System.IO.BinaryReader r)
        {
            int oldUnlocked = 200; // The old unlock constant

            advancedBuilding = adjust(r.ReadByte(), AdvancedBuildingUnlock, oldUnlocked);
            advancedFarming = adjust(r.ReadByte(), AdvancedFarmingUnlock, oldUnlocked);
            advancedCasting = adjust(r.ReadByte(), AdvancedCastingUnlock, oldUnlocked);
            iron = adjust(r.ReadByte(), IronUnlock, oldUnlocked);
            steel = adjust(r.ReadByte(), SteelUnlock, oldUnlocked);
            catapult = adjust(r.ReadByte(), CatapultUnlock, oldUnlocked);
            blackPowder = adjust(r.ReadByte(), BlackPowderUnlock, oldUnlocked);
            gunPowder = adjust(r.ReadByte(), GunPowderUnlock, oldUnlocked);

            int adjust(int value, int newUnlock, int oldUnlock)
            {
                return Bound.Set(value - oldUnlock + newUnlock, 0, newUnlock);
            }
        }

        public Unlocks GetUnlocks(bool factionView)
        {
            Unlocks unlocks = new Unlocks();

            if (advancedBuilding >= (factionView ? 1 : AdvancedBuildingUnlock))
                unlocks.UnlockAdvancedBuilding();

            if (advancedFarming >= (factionView ? 1 : AdvancedFarmingUnlock))
                unlocks.UnlockAdvancedFarming();

            if (advancedCasting >= (factionView ? 1 : AdvancedCastingUnlock))
                unlocks.UnlockAdvancedCasting();

            if (iron >= (factionView ? 1 : IronUnlock))
                unlocks.UnlockIron();

            if (steel >= (factionView ? 1 : SteelUnlock))
                unlocks.UnlockSteel();

            if (catapult >= (factionView ? 1 : CatapultUnlock))
                unlocks.UnlockCatapult();

            if (blackPowder >= (factionView ? 1 : BlackPowderUnlock))
                unlocks.UnlockBlackPowder();

            if (gunPowder >= (factionView ? 1 : GunPowderUnlock))
                unlocks.UnlockGunPowder();

            return unlocks;
        }

        public void destroyTechOnTakeOver()
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
                if (thisTech > 0)
                {
                    int points = thisTech;
                    thisTech = Math.Min(Ref.rnd.Int(points), Ref.rnd.Int(points));
#if DEBUG
                    if (thisTech < 0)
                        throw new Exception();
#endif
                }
            }
        }

        public void gainTechSpread(TechnologyTemplate from, int gainSpeed)
        {
#if DEBUG
            if (gainSpeed < 0)
                throw new Exception();
#endif

            tech(ref advancedBuilding, from.advancedBuilding, AdvancedBuildingUnlock);
            tech(ref advancedFarming, from.advancedFarming, AdvancedFarmingUnlock);
            tech(ref advancedCasting, from.advancedCasting, AdvancedCastingUnlock);
            tech(ref iron, from.iron, IronUnlock);
            if (iron >= IronUnlock)
                tech(ref steel, from.steel, SteelUnlock);
            tech(ref catapult, from.catapult, CatapultUnlock);
            tech(ref blackPowder, from.blackPowder, BlackPowderUnlock);
            if (blackPowder >= BlackPowderUnlock)
                tech(ref gunPowder, from.gunPowder, GunPowderUnlock);

            void tech(ref int thisTech, int otherTech, int unlock)
            {
                if (otherTech >= unlock && thisTech < unlock)
                {
                    thisTech = Bound.Max(thisTech + gainSpeed, unlock);
                }
            }
        }

        public void addFactionUnlocked(TechnologyTemplate from, bool toCity, bool includeProgress)
        {
            tech(ref advancedBuilding, from.advancedBuilding, AdvancedBuildingUnlock);
            tech(ref advancedFarming, from.advancedFarming, AdvancedFarmingUnlock);
            tech(ref advancedCasting, from.advancedCasting, AdvancedCastingUnlock);
            tech(ref iron, from.iron, IronUnlock);
            tech(ref steel, from.steel, SteelUnlock);
            tech(ref catapult, from.catapult, CatapultUnlock);
            tech(ref blackPowder, from.blackPowder, BlackPowderUnlock);
            tech(ref gunPowder, from.gunPowder, GunPowderUnlock);

            void tech(ref int thisTech, int otherTech, int unlock)
            {
                if (otherTech >= FactionUnlock)
                {
                    thisTech = toCity ? unlock : FactionUnlock;
                }
                else if (includeProgress)
                {
                    thisTech = otherTech;
                }
            }
        }

        public void checkCityCount(int cityCount)
        {
            tech(ref advancedBuilding, AdvancedBuildingUnlock);
            tech(ref advancedFarming, AdvancedFarmingUnlock);
            tech(ref advancedCasting, AdvancedCastingUnlock);
            tech(ref iron, IronUnlock);
            tech(ref steel, SteelUnlock);
            tech(ref catapult, CatapultUnlock);
            tech(ref blackPowder, BlackPowderUnlock);
            tech(ref gunPowder, GunPowderUnlock);

            void tech(ref int thisTech, int unlock)
            {
                if (thisTech >= cityCount)
                {
                    thisTech = FactionUnlock;
                }
            }
        }

        public void countUnlocks(TechnologyTemplate city)
        {
            tech(ref advancedBuilding, AdvancedBuildingUnlock, city.advancedBuilding);
            tech(ref advancedFarming, AdvancedFarmingUnlock, city.advancedFarming);
            tech(ref advancedCasting, AdvancedCastingUnlock, city.advancedCasting);
            tech(ref iron, IronUnlock, city.iron);
            tech(ref steel, SteelUnlock, city.steel);
            tech(ref catapult, CatapultUnlock, city.catapult);
            tech(ref blackPowder, BlackPowderUnlock, city.blackPowder);
            tech(ref gunPowder, GunPowderUnlock, city.gunPowder);

            void tech(ref int thisTech, int unlock, int cityTech)
            {
                if (cityTech >= unlock)
                {
                    thisTech++;
                }
            }
        }

        public static int PercentProgress(int value)
        {
            return Bound.Max(value, 100);
        }
    }


}
