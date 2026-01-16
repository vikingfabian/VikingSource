using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.XP
{
    struct ResearchProgress
    {
        public int points = 0;
        //public int bookpressCount = 0;
        //public int researchCenterCount = 0;

        public ResearchProgress()
        { }

        public void workerLevelUp(int researchCenterCount, ref int gainPoints)
        {
            gainPoints += researchCenterCount * DssConst.TechnologyGain_ResearchCenter;
            
            points += gainPoints;
        }

        public void writeGameState(System.IO.BinaryWriter w, int unlock, bool faction)
        {
            w.Write((ushort)Bound.Max(points, unlock));
        }

        public void readGameState(System.IO.BinaryReader r, int subversion, bool faction)
        {
            if (subversion < 65)
            {
                points = r.ReadByte();
            }
            else
            { 
                points = r.ReadUInt16();
            }
        }    
    }
    struct TechnologyUnlock
    {
        public int AdvancedBuildingUnlock;
        public int AdvancedFarmingUnlock;
        public int AdvancedCastingUnlock;
        public int IronUnlock;
        public int SteelUnlock;
        public int CatapultUnlock;
        public int BlackPowderUnlock;
        public int GunPowderUnlock;

        public TechnologyUnlock(int speed)
        {
            AdvancedBuildingUnlock = 50 / speed;
            AdvancedFarmingUnlock = 150 / speed;
            AdvancedCastingUnlock = 200 / speed;
            IronUnlock = 200 / speed;
            SteelUnlock = 300 / speed;
            CatapultUnlock = 200 / speed;
            BlackPowderUnlock = 500 / speed;
            GunPowderUnlock = 1000 / speed;
        }
    }



    struct TechnologyTemplate
    {
        //public static readonly TechnologyTemplate Start = new TechnologyTemplate();

        public const int FactionUnlock = 100000;

        

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

        public static ref ResearchProgress GetResearchProgressRef(ref TechnologyTemplate template, TechnologyTreeType techType)
        {
            switch (techType)
            {
                case TechnologyTreeType.advancedBuilding:
                    return ref template.advancedBuilding;
                case TechnologyTreeType.advancedFarming:
                    return ref template.advancedFarming;
                case TechnologyTreeType.advancedCasting:
                    return ref template.advancedCasting;
                case TechnologyTreeType.iron:
                    return ref template.iron;
                case TechnologyTreeType.steel:
                    return ref template.steel;
                case TechnologyTreeType.catapult:
                    return ref template.catapult;
                case TechnologyTreeType.blackPowder:
                    return ref template.blackPowder;
                case TechnologyTreeType.gunPowder:
                    return ref template.gunPowder;
                default:
                    throw new ArgumentOutOfRangeException(nameof(techType), $"Invalid TechnologyTreeType: {techType}");
            }
        }

        public ResearchProgress progress(TechnologyTreeType techType, out int goal)
        {
            switch (techType)
            {
                case TechnologyTreeType.advancedBuilding:
                    goal = XpLib.Unlock.AdvancedBuildingUnlock;
                    return advancedBuilding;
                case TechnologyTreeType.advancedFarming:
                    goal = XpLib.Unlock.AdvancedFarmingUnlock;
                    return advancedFarming;
                case TechnologyTreeType.advancedCasting:
                    goal = XpLib.Unlock.AdvancedCastingUnlock;
                    return advancedCasting;
                case TechnologyTreeType.iron:
                    goal = XpLib.Unlock.IronUnlock;
                    return iron;
                case TechnologyTreeType.steel:
                    goal = XpLib.Unlock.SteelUnlock;
                    return steel;
                case TechnologyTreeType.catapult:
                    goal = XpLib.Unlock.CatapultUnlock;
                    return catapult;
                case TechnologyTreeType.blackPowder:
                    goal = XpLib.Unlock.BlackPowderUnlock;
                    return blackPowder;
                case TechnologyTreeType.gunPowder:
                    goal = XpLib.Unlock.GunPowderUnlock;
                    return gunPowder;
                default:
                    throw new ArgumentOutOfRangeException(nameof(techType), $"Invalid TechnologyTreeType: {techType}");
            }
        }

        public void writeGameState(System.IO.BinaryWriter w, bool faction)
        {
            //new
            advancedBuilding.writeGameState(w, XpLib.Unlock.AdvancedBuildingUnlock, faction);
            advancedFarming.writeGameState(w, XpLib.Unlock.AdvancedFarmingUnlock, faction);
            advancedCasting.writeGameState(w, XpLib.Unlock.AdvancedCastingUnlock, faction);
            iron.writeGameState(w, XpLib.Unlock.IronUnlock, faction);
            steel.writeGameState(w, XpLib.Unlock.SteelUnlock, faction);
            catapult.writeGameState(w, XpLib.Unlock.CatapultUnlock, faction);
            blackPowder.writeGameState(w, XpLib.Unlock.BlackPowderUnlock, faction);
            gunPowder.writeGameState(w, XpLib.Unlock.GunPowderUnlock, faction);

            
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

            advancedBuilding.points = adjust(r.ReadByte(), XpLib.Unlock.AdvancedBuildingUnlock, oldUnlocked);
            advancedFarming.points = adjust(r.ReadByte(), XpLib.Unlock.AdvancedFarmingUnlock, oldUnlocked);
            advancedCasting.points = adjust(r.ReadByte(), XpLib.Unlock.AdvancedCastingUnlock, oldUnlocked);
            iron.points = adjust(r.ReadByte(), XpLib.Unlock.IronUnlock, oldUnlocked);
            steel.points = adjust(r.ReadByte(), XpLib.Unlock.SteelUnlock, oldUnlocked);
            catapult.points = adjust(r.ReadByte(), XpLib.Unlock.CatapultUnlock, oldUnlocked);
            blackPowder.points = adjust(r.ReadByte(), XpLib.Unlock.BlackPowderUnlock, oldUnlocked);
            gunPowder.points = adjust(r.ReadByte(), XpLib.Unlock.GunPowderUnlock, oldUnlocked);

            int adjust(int value, int newUnlock, int oldUnlock)
            {
                return Bound.Set(value - oldUnlock + newUnlock, 0, newUnlock);
            }
        }

        public Unlocks GetUnlocks(bool factionView)
        {
            Unlocks unlocks = new Unlocks();
            unlocks.allUnlocked = true;

            if (advancedBuilding.points >= (factionView ? 1 : XpLib.Unlock.AdvancedBuildingUnlock))
            {
                unlocks.UnlockAdvancedBuilding();
            }
            else
            {
                unlocks.allUnlocked = false;
            }

            if (advancedFarming.points >= (factionView ? 1 : XpLib.Unlock.AdvancedFarmingUnlock))
            {
                unlocks.UnlockAdvancedFarming();
            }
            else
            {
                unlocks.allUnlocked = false;
            }

            if (advancedCasting.points >= (factionView ? 1 : XpLib.Unlock.AdvancedCastingUnlock))
            {
                unlocks.UnlockAdvancedCasting();
            }
            else
            {
                unlocks.allUnlocked = false;
            }

            if (iron.points >= (factionView ? 1 : XpLib.Unlock.IronUnlock))
            {
                unlocks.UnlockIron();
            }
            else
            {
                unlocks.allUnlocked = false;
            }

            if (steel.points >= (factionView ? 1 : XpLib.Unlock.SteelUnlock))
            {
                unlocks.UnlockSteel();
            }
            else
            {
                unlocks.allUnlocked = false;
            }

            if (catapult.points >= (factionView ? 1 : XpLib.Unlock.CatapultUnlock))
            {
                unlocks.UnlockCatapult();
            }
            else
            {
                unlocks.allUnlocked = false;
            }

            if (blackPowder.points >= (factionView ? 1 : XpLib.Unlock.BlackPowderUnlock))
            {
                unlocks.UnlockBlackPowder();
            }
            else
            {
                unlocks.allUnlocked = false;
            }

            if (gunPowder.points >= (factionView ? 1 : XpLib.Unlock.GunPowderUnlock))
            {
                unlocks.UnlockGunPowder();
            }
            else
            {
                unlocks.allUnlocked = false;
            }

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

        public static void GainTechSpread(City city, TechnologyTemplate from, int gainSpeed, TechnologyGainReason reason)
        {
#if DEBUG
            if (gainSpeed < 0)
                throw new Exception();
#endif
            tech(TechnologyTreeType.advancedBuilding, ref city.technology.advancedBuilding.points, from.advancedBuilding.points, XpLib.Unlock.AdvancedBuildingUnlock);
            tech(TechnologyTreeType.advancedFarming, ref city.technology.advancedFarming.points, from.advancedFarming.points, XpLib.Unlock.AdvancedFarmingUnlock);
            tech(TechnologyTreeType.advancedCasting, ref city.technology.advancedCasting.points, from.advancedCasting.points, XpLib.Unlock.AdvancedCastingUnlock);
            tech(TechnologyTreeType.iron, ref city.technology.iron.points, from.iron.points, XpLib.Unlock.IronUnlock);
            if (city.technology.iron.points >= XpLib.Unlock.IronUnlock)
                tech(TechnologyTreeType.steel, ref city.technology.steel.points, from.steel.points, XpLib.Unlock.SteelUnlock);
            tech(TechnologyTreeType.catapult, ref city.technology.catapult.points, from.catapult.points, XpLib.Unlock.CatapultUnlock);
            tech(TechnologyTreeType.blackPowder, ref city.technology.blackPowder.points, from.blackPowder.points, XpLib.Unlock.BlackPowderUnlock);
            if (city.technology.blackPowder.points >= XpLib.Unlock.BlackPowderUnlock)
                tech(TechnologyTreeType.gunPowder, ref city.technology.gunPowder.points, from.gunPowder.points, XpLib.Unlock.GunPowderUnlock);

            void tech(TechnologyTreeType type, ref int thisTech, int otherTech, int unlock)
            {
                if (otherTech >= unlock && thisTech < unlock)
                {
                    thisTech = Bound.Max(thisTech + gainSpeed, unlock);
                    city.onTechnologyGain(type, gainSpeed, reason);
                }
            }
        }

        public List<TechnologyTreeType> availableTech()
        {
            List <TechnologyTreeType> result = new List <TechnologyTreeType>(8);

            tech(TechnologyTreeType.advancedBuilding, advancedBuilding.points, XpLib.Unlock.AdvancedBuildingUnlock);
            tech(TechnologyTreeType.advancedFarming, advancedFarming.points, XpLib.Unlock.AdvancedFarmingUnlock);
            tech(TechnologyTreeType.advancedCasting, advancedCasting.points, XpLib.Unlock.AdvancedCastingUnlock);
            tech(TechnologyTreeType.iron, iron.points, XpLib.Unlock.IronUnlock);
            if (iron.points >= XpLib.Unlock.IronUnlock)
                tech(TechnologyTreeType.steel, steel.points, XpLib.Unlock.SteelUnlock);
            tech(TechnologyTreeType.catapult,   catapult.points, XpLib.Unlock.CatapultUnlock);
            tech(TechnologyTreeType.blackPowder, blackPowder.points, XpLib.Unlock.BlackPowderUnlock);
            if (blackPowder.points >= XpLib.Unlock.BlackPowderUnlock)
                tech(TechnologyTreeType.gunPowder, gunPowder.points, XpLib.Unlock.GunPowderUnlock);
            
            return result;

            void tech(TechnologyTreeType type, int thisTech, int unlock)
            {
                if (thisTech < unlock)
                { 
                    result.Add(type);
                }
            }
        }
//        public void gainTechSpread(TechnologyTemplate from, int gainSpeed)
//        {
//#if DEBUG
//            if (gainSpeed < 0)
//                throw new Exception();
//#endif

//            tech(ref advancedBuilding.points, from.advancedBuilding.points, AdvancedBuildingUnlock);
//            tech(ref advancedFarming.points, from.advancedFarming.points, AdvancedFarmingUnlock);
//            tech(ref advancedCasting.points, from.advancedCasting.points, AdvancedCastingUnlock);
//            tech(ref iron.points, from.iron.points, IronUnlock);
//            if (iron.points >= IronUnlock)
//                tech(ref steel.points, from.steel.points, SteelUnlock);
//            tech(ref catapult.points, from.catapult.points, CatapultUnlock);
//            tech(ref blackPowder.points, from.blackPowder.points, BlackPowderUnlock);
//            if (blackPowder.points >= BlackPowderUnlock)
//                tech(ref gunPowder.points, from.gunPowder.points, GunPowderUnlock);

//            void tech(ref int thisTech, int otherTech, int unlock)
//            {
//                if (otherTech >= unlock && thisTech < unlock)
//                {
//                    thisTech = Bound.Max(thisTech + gainSpeed, unlock);
//                }
//            }
//        }

        public void addFactionUnlocked(TechnologyTemplate from, bool toCity, bool includeProgress)
        {
            tech(ref advancedBuilding.points, from.advancedBuilding.points, XpLib.Unlock.AdvancedBuildingUnlock);
            tech(ref advancedFarming.points, from.advancedFarming.points, XpLib.Unlock.AdvancedFarmingUnlock);
            tech(ref advancedCasting.points, from.advancedCasting.points, XpLib.Unlock.AdvancedCastingUnlock);
            tech(ref iron.points, from.iron.points, XpLib.Unlock.IronUnlock);
            tech(ref steel.points, from.steel.points, XpLib.Unlock.SteelUnlock);
            tech(ref catapult.points, from.catapult.points, XpLib.Unlock.CatapultUnlock);
            tech(ref blackPowder.points, from.blackPowder.points, XpLib.Unlock.BlackPowderUnlock);
            tech(ref gunPowder.points, from.gunPowder.points, XpLib.Unlock.GunPowderUnlock);

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
            tech(ref advancedBuilding.points, XpLib.Unlock.AdvancedBuildingUnlock);
            tech(ref advancedFarming.points, XpLib.Unlock.AdvancedFarmingUnlock);
            tech(ref advancedCasting.points, XpLib.Unlock.AdvancedCastingUnlock);
            tech(ref iron.points, XpLib.Unlock.IronUnlock);
            tech(ref steel.points, XpLib.Unlock.SteelUnlock);
            tech(ref catapult.points, XpLib.Unlock.CatapultUnlock);
            tech(ref blackPowder.points, XpLib.Unlock.BlackPowderUnlock);
            tech(ref gunPowder.points, XpLib.Unlock.GunPowderUnlock);

            void tech(ref int thisTech, int unlock)
            {
                if (thisTech >= cityCount)
                {
                    thisTech = FactionUnlock;
                }
            }
        }

        public void unlockAll_debug()
        {
            tech(ref advancedBuilding.points, XpLib.Unlock.AdvancedBuildingUnlock);
            tech(ref advancedFarming.points, XpLib.Unlock.AdvancedFarmingUnlock);
            tech(ref advancedCasting.points, XpLib.Unlock.AdvancedCastingUnlock);
            tech(ref iron.points, XpLib.Unlock.IronUnlock);
            tech(ref steel.points, XpLib.Unlock.SteelUnlock);
            tech(ref catapult.points, XpLib.Unlock.CatapultUnlock);
            tech(ref blackPowder.points,    XpLib.Unlock.BlackPowderUnlock);
            tech(ref gunPowder.points, XpLib.Unlock.GunPowderUnlock);

            void tech(ref int thisTech, int unlock)
            {
                thisTech = unlock;
            }
        }

        public void countUnlocks(TechnologyTemplate city)
        {
            tech(ref advancedBuilding.points, XpLib.Unlock.AdvancedBuildingUnlock, city.advancedBuilding.points);
            tech(ref advancedFarming.points, XpLib.Unlock.AdvancedFarmingUnlock, city.advancedFarming.points);
            tech(ref advancedCasting.points, XpLib.Unlock.AdvancedCastingUnlock, city.advancedCasting.points);
            tech(ref iron.points, XpLib.Unlock.IronUnlock, city.iron.points);
            tech(ref steel.points, XpLib.Unlock.SteelUnlock, city.steel.points);
            tech(ref catapult.points, XpLib.Unlock.CatapultUnlock, city.catapult.points);
            tech(ref blackPowder.points, XpLib.Unlock.BlackPowderUnlock, city.blackPowder.points);
            tech(ref gunPowder.points, XpLib.Unlock.GunPowderUnlock, city.gunPowder.points);

            void tech(ref int thisTech, int unlock, int cityTech)
            {
                if (cityTech >= unlock)
                {
                    thisTech++;
                }
            }
        }

        public TechnologyTreeType ExperienceToTechField(WorkExperienceType experienceType)
        {
            switch (experienceType)
            {
                case WorkExperienceType.HouseBuilding:
                case WorkExperienceType.StoneCutter:
                    return TechnologyTreeType.advancedBuilding;

                case WorkExperienceType.Farm:
                case WorkExperienceType.AnimalCare:
                    return TechnologyTreeType.advancedFarming;

                case WorkExperienceType.Mining:
                case WorkExperienceType.Smelting:
                case WorkExperienceType.CastMetal:
                    return TechnologyTreeType.advancedCasting;
               
                case WorkExperienceType.CraftMetal:
                //case WorkExperienceType.CraftWeapon:
                case WorkExperienceType.CraftArmor:
                    return (iron.points < XpLib.Unlock.IronUnlock)
                        ? TechnologyTreeType.iron
                        : TechnologyTreeType.steel;

                case WorkExperienceType.WoodWork:
                case WorkExperienceType.Fletcher:
                    return TechnologyTreeType.catapult;

                case WorkExperienceType.CraftFuel:
                case WorkExperienceType.Chemistry:
                    return (blackPowder.points < XpLib.Unlock.BlackPowderUnlock)
                        ? TechnologyTreeType.blackPowder
                        : TechnologyTreeType.gunPowder;

                default:
                    return TechnologyTreeType.NUM_NONE;
                    //throw new ArgumentOutOfRangeException(nameof(experienceType), $"Unhandled experience type: {experienceType}");
            }
        }


        public static int PercentProgress(int value)
        {
            return Bound.Max(value, 100);
        }
    }


}
