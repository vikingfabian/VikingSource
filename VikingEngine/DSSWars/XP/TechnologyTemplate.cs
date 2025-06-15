using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Resource;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.XP
{
    struct ResearchProgress
    {
        public int points = 0;
        public int bookpress_IdAndPos = -1;
        public int researchCenter_IdAndPos = -1;

        public ResearchProgress()
        { }

        public void writeGameState(System.IO.BinaryWriter w, int unlock, bool faction)
        {
            w.Write((byte)Bound.Max(points, unlock));
            if (!faction)
            {
                new EightBit(bookpress_IdAndPos >= 0, researchCenter_IdAndPos >= 0).write(w);

                if (bookpress_IdAndPos >= 0)
                {
                    w.Write(bookpress_IdAndPos);
                }
                if (researchCenter_IdAndPos >= 0)
                {
                    w.Write(researchCenter_IdAndPos);
                }
            }
        }

        public void readGameState(System.IO.BinaryReader r, int subversion, bool faction)
        {
            points = r.ReadByte();
            if (!faction)
            {
                EightBit bools = EightBit.FromStream(r);
                if (bools.Get(0))
                {
                    bookpress_IdAndPos = r.ReadInt32();
                }
                if (bools.Get(1))
                {
                    researchCenter_IdAndPos = r.ReadInt32();
                }
            }
        }
    
    }

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

        public ResearchProgress advancedBuilding;
        public ResearchProgress advancedFarming;
        public ResearchProgress advancedCasting;
        public ResearchProgress iron;
        public ResearchProgress steel;
        public ResearchProgress catapult;
        public ResearchProgress blackPowder;
        public ResearchProgress gunPowder;

        public TechnologyTemplate()
        {
            advancedBuilding =  new ResearchProgress();
            advancedFarming = new ResearchProgress();
            advancedCasting = new ResearchProgress();
            iron = new ResearchProgress();
            steel = new ResearchProgress();
            catapult = new ResearchProgress();
            blackPowder = new ResearchProgress();
            gunPowder = new ResearchProgress();
        }

        public void zero()
        {
            advancedBuilding.points = 0;
            advancedFarming.points = 0;
            advancedCasting.points = 0;
            iron.points = 0;
            steel.points = 0;
            catapult.points = 0;
            blackPowder.points = 0;
            gunPowder.points = 0;
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

        public void writeGameState(System.IO.BinaryWriter w, bool faction)
        {
            //new
            advancedBuilding.writeGameState(w, AdvancedBuildingUnlock, faction);
            advancedFarming.writeGameState(w, AdvancedFarmingUnlock, faction);
            advancedCasting.writeGameState(w, AdvancedCastingUnlock, faction);
            iron.writeGameState(w, IronUnlock, faction);
            steel.writeGameState(w, SteelUnlock, faction);
            catapult.writeGameState(w, CatapultUnlock, faction);
            blackPowder.writeGameState(w, BlackPowderUnlock, faction);
            gunPowder.writeGameState(w, GunPowderUnlock, faction);

            //old
            //w.Write((byte)Bound.Max(advancedBuilding, AdvancedBuildingUnlock));
            //w.Write((byte)Bound.Max(advancedFarming, AdvancedFarmingUnlock));
            //w.Write((byte)Bound.Max(advancedCasting, AdvancedCastingUnlock));
            //w.Write((byte)Bound.Max(iron, IronUnlock));
            //w.Write((byte)Bound.Max(steel, SteelUnlock));
            //w.Write((byte)Bound.Max(catapult, CatapultUnlock));
            //w.Write((byte)Bound.Max(blackPowder, BlackPowderUnlock));
            //w.Write((byte)Bound.Max(gunPowder, GunPowderUnlock));
        }

        public void readGameState(System.IO.BinaryReader r, int subversion, bool faction)
        {
            if (subversion < 61)
            {
                readGameState_old(r);
            }
            else if (subversion < 64)
            {
                advancedBuilding.points = r.ReadByte();
                advancedFarming.points = r.ReadByte();
                advancedCasting.points = r.ReadByte();
                iron.points = r.ReadByte();
                steel.points = r.ReadByte();
                catapult.points = r.ReadByte();
                blackPowder.points = r.ReadByte();
                gunPowder.points = r.ReadByte();
            }
            else //new
            {
                advancedBuilding.readGameState(r, subversion, faction);
                advancedFarming.readGameState(r, subversion, faction);
                advancedCasting.readGameState(r, subversion, faction);
                iron.readGameState(r, subversion, faction);
                steel.readGameState(r, subversion, faction);
                catapult.readGameState(r, subversion, faction);
                blackPowder.readGameState(r, subversion, faction);
                gunPowder.readGameState(r, subversion, faction);

            }
        }
        public void readGameState_old(System.IO.BinaryReader r)
        {
            int oldUnlocked = 200; // The old unlock constant

            advancedBuilding.points = adjust(r.ReadByte(), AdvancedBuildingUnlock, oldUnlocked);
            advancedFarming.points = adjust(r.ReadByte(), AdvancedFarmingUnlock, oldUnlocked);
            advancedCasting.points = adjust(r.ReadByte(), AdvancedCastingUnlock, oldUnlocked);
            iron.points = adjust(r.ReadByte(), IronUnlock, oldUnlocked);
            steel.points = adjust(r.ReadByte(), SteelUnlock, oldUnlocked);
            catapult.points = adjust(r.ReadByte(), CatapultUnlock, oldUnlocked);
            blackPowder.points = adjust(r.ReadByte(), BlackPowderUnlock, oldUnlocked);
            gunPowder.points = adjust(r.ReadByte(), GunPowderUnlock, oldUnlocked);

            int adjust(int value, int newUnlock, int oldUnlock)
            {
                return Bound.Set(value - oldUnlock + newUnlock, 0, newUnlock);
            }
        }

        public Unlocks GetUnlocks(bool factionView)
        {
            Unlocks unlocks = new Unlocks();

            if (advancedBuilding.points >= (factionView ? 1 : AdvancedBuildingUnlock))
                unlocks.UnlockAdvancedBuilding();

            if (advancedFarming.points >= (factionView ? 1 : AdvancedFarmingUnlock))
                unlocks.UnlockAdvancedFarming();

            if (advancedCasting.points >= (factionView ? 1 : AdvancedCastingUnlock))
                unlocks.UnlockAdvancedCasting();

            if (iron.points >= (factionView ? 1 : IronUnlock))
                unlocks.UnlockIron();

            if (steel.points >= (factionView ? 1 : SteelUnlock))
                unlocks.UnlockSteel();

            if (catapult.points >= (factionView ? 1 : CatapultUnlock))
                unlocks.UnlockCatapult();

            if (blackPowder.points >= (factionView ? 1 : BlackPowderUnlock))
                unlocks.UnlockBlackPowder();

            if (gunPowder.points >= (factionView ? 1 : GunPowderUnlock))
                unlocks.UnlockGunPowder();

            return unlocks;
        }

        public void destroyTechOnTakeOver()
        {
            tech(ref advancedBuilding.points);
            tech(ref advancedFarming.points);
            tech(ref advancedCasting.points);
            tech(ref iron.points);
            tech(ref steel.points);
            tech(ref catapult.points);
            tech(ref blackPowder.points);
            tech(ref gunPowder.points);

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

            tech(ref advancedBuilding.points, from.advancedBuilding.points, AdvancedBuildingUnlock);
            tech(ref advancedFarming.points, from.advancedFarming.points, AdvancedFarmingUnlock);
            tech(ref advancedCasting.points, from.advancedCasting.points, AdvancedCastingUnlock);
            tech(ref iron.points, from.iron.points, IronUnlock);
            if (iron.points >= IronUnlock)
                tech(ref steel.points, from.steel.points, SteelUnlock);
            tech(ref catapult.points, from.catapult.points, CatapultUnlock);
            tech(ref blackPowder.points, from.blackPowder.points, BlackPowderUnlock);
            if (blackPowder.points >= BlackPowderUnlock)
                tech(ref gunPowder.points, from.gunPowder.points, GunPowderUnlock);

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
            tech(ref advancedBuilding.points, from.advancedBuilding.points, AdvancedBuildingUnlock);
            tech(ref advancedFarming.points, from.advancedFarming.points, AdvancedFarmingUnlock);
            tech(ref advancedCasting.points, from.advancedCasting.points, AdvancedCastingUnlock);
            tech(ref iron.points, from.iron.points, IronUnlock);
            tech(ref steel.points, from.steel.points, SteelUnlock);
            tech(ref catapult.points, from.catapult.points, CatapultUnlock);
            tech(ref blackPowder.points, from.blackPowder.points, BlackPowderUnlock);
            tech(ref gunPowder.points, from.gunPowder.points, GunPowderUnlock);

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
            tech(ref advancedBuilding.points, AdvancedBuildingUnlock);
            tech(ref advancedFarming.points, AdvancedFarmingUnlock);
            tech(ref advancedCasting.points, AdvancedCastingUnlock);
            tech(ref iron.points, IronUnlock);
            tech(ref steel.points, SteelUnlock);
            tech(ref catapult.points, CatapultUnlock);
            tech(ref blackPowder.points, BlackPowderUnlock);
            tech(ref gunPowder.points, GunPowderUnlock);

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
            tech(ref advancedBuilding.points, AdvancedBuildingUnlock, city.advancedBuilding.points);
            tech(ref advancedFarming.points, AdvancedFarmingUnlock, city.advancedFarming.points);
            tech(ref advancedCasting.points, AdvancedCastingUnlock, city.advancedCasting.points);
            tech(ref iron.points, IronUnlock, city.iron.points);
            tech(ref steel.points, SteelUnlock, city.steel.points);
            tech(ref catapult.points, CatapultUnlock, city.catapult.points);
            tech(ref blackPowder.points, BlackPowderUnlock, city.blackPowder.points);
            tech(ref gunPowder.points, GunPowderUnlock, city.gunPowder.points);

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
