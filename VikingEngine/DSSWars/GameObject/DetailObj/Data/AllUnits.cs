using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using VikingEngine.DSSWars.GameObject.DetailObj.Soldiers;
using VikingEngine.LootFest;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.DSSWars.GameObject
{
    class AllUnits
    {
        public static float AverageGroupStrength;
        public const float HealthToStrengthConvertion = 0.5f;

        AbsSoldierData[] soldiers = new AbsSoldierData[(int)UnitType.NUM];
        AbsSoldierData[] recruits = new AbsSoldierData[(int)UnitType.NUM];

        public AllUnits()
        {
            DssRef.unitsdata = this;

            add(new FolkWarshipData(UnitType.FolkWarship, 
                add(new FolkManData())));

            add(new SoldierWarshipData(UnitType.SoldierWarship,
                add(new SoldierData())));

            add(new SoldierWarshipData(UnitType.HonorGuardWarship,
                add(new HonorGuardData())));

            add(new VikingWarshipData(UnitType.SailorWarship, 
                add(new SailorData())));

            add(new KnightWarshipData(UnitType.KnightWarship,
                add(new KnightData())));
           
            add(new ArcherWarshipData(UnitType.ArcherWarship,
                add(new ArcherData())));
            
            add(new BallistaWarshipData(UnitType.BallistaWarship,
                add(new BallistaData())));

            add(new BannerManData());

            add(new SoldierWarshipData(UnitType.PikemanWarship,
                add(new Pikeman())));

            add(new ArcherWarshipData(UnitType.CrossbowWarship,
                add(new CrossBow())));

            add(new BallistaWarshipData(UnitType.TrollcannonWarship,
                add(new TrollCannon())));

            add(new KnightWarshipData(UnitType.GreenWarship,
                add(new GreenSoldier())));

            add(new VikingWarshipData(UnitType.VikingWarship,
                add(new Viking())));

            add(new DarkLordWarshipData(UnitType.DarkLordWarship,
                add(new DarkLordData())));

            var recruit = recruits[(int)UnitType.Soldier];
            add(new FolkWarshipData(UnitType.RecruitWarship,
                recruit));

            int defaultDps = DssRef.unitsdata.Get(UnitType.Soldier).DPS_land();
            AverageGroupStrength = AbsSoldierData.GroupDefaultCount * (defaultDps + HealthToStrengthConvertion * AbsSoldierData.DefaultHealth) ;
        }

        public void AddModelsToLoad(List<VoxelModelName> modelNames)
        {
            foreach (var s in soldiers)
            {
                if (s != null)
                {
                    for (int i = 0; i < s.modelVariationCount; ++i)
                    {
                        arraylib.ListAddIfNotExist(modelNames, s.modelName + i);
                    }
                }
            }
        }

        AbsSoldierData add(AbsSoldierData soldier)
        {
            soldiers[(int)soldier.unitType] = soldier;

            recruits[(int)soldier.unitType] = new RecruitData(soldier);

            return soldier;
        }

        public AbsSoldierData Get(UnitType type)
        {
            return soldiers[(int)type];
        }

        public AbsSoldierUnit createSoldier(UnitType type, bool recruit)
        {
            AbsSoldierData data;
            if (recruit)
            {
                data = recruits[(int)type];
            }
            else
            {
                data = soldiers[(int)type];
            }
            var soldier = data.CreateUnit() as AbsSoldierUnit;//TODO borde ta bort konvertering
            soldier.data = data;

            return soldier;
        }

        public string Name(UnitType type)
        {
            switch (type)
            {
                case UnitType.Soldier:
                    return DssRef.lang.UnitType_Soldier;

                case UnitType.Sailor:
                    return DssRef.lang.UnitType_Sailor;

                case UnitType.Folkman:
                    return DssRef.lang.UnitType_Folkman;

                case UnitType.Spearman:
                    return DssRef.lang.UnitType_Spearman;

                case UnitType.HonorGuard:
                    return DssRef.lang.UnitType_HonorGuard;

                case UnitType.Pikeman:
                    return DssRef.lang.UnitType_Pikeman;

                case UnitType.Knight:
                    return DssRef.lang.UnitType_Knight;

                case UnitType.Archer:
                    return DssRef.lang.UnitType_Archer;

                case UnitType.CrossBow:
                    return DssRef.lang.UnitType_Crossbow;

                case UnitType.Ballista:
                    return DssRef.lang.UnitType_Ballista;

                case UnitType.Trollcannon:
                    return DssRef.lang.UnitType_Trollcannon;

                case UnitType.GreenSoldier:
                    return DssRef.lang.UnitType_GreenSoldier;

                case UnitType.Viking:
                    return DssRef.lang.UnitType_Viking;

                case UnitType.DarkLord:
                    return DssRef.lang.UnitType_DarkLord;

                case UnitType.BannerMan:
                    return DssRef.lang.UnitType_Bannerman;

                // Warship cases
                case UnitType.SoldierWarship:
                    return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Soldier);

                case UnitType.SailorWarship:
                    return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Sailor);

                case UnitType.HonorGuardWarship:
                    return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_HonorGuard);

                case UnitType.PikemanWarship:
                    return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Pikeman);

                case UnitType.ArcherWarship:
                    return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Archer);

                case UnitType.CrossbowWarship:
                    return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Crossbow);

                case UnitType.BallistaWarship:
                    return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Ballista);

                case UnitType.TrollcannonWarship:
                    return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Trollcannon);

                case UnitType.VikingWarship:
                    return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Viking);

                case UnitType.DarkLordWarship:
                    return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_DarkLord);

                default:
                    return "Unknown Unit Type";
            }
        }
    }

    enum UnitType
    {
        NULL = -1,
        //King = 34,
        //KingsGuard = 35,
        Recruit =0,
        Soldier =1,
        Sailor =2,
        Folkman =3,
        Spearman =4,
        HonorGuard=10,
        Pikeman =5,
        Knight=6,
        Archer=7,
        CrossBow=8,

        Ballista=9,        
        Trollcannon=11,
        GreenSoldier = 13,
        Viking = 14,
        DarkLord = 16,
        BannerMan =12,
        City=15,

        RecruitWarship = 17,
        FolkWarship = 18,

        SoldierWarship = 19,
        HonorGuardWarship = 20,
        PikemanWarship = 21,

        ArcherWarship = 22,
        CrossbowWarship = 23,

        BallistaWarship = 24,
        TrollcannonWarship = 25,

        SailorWarship = 26,
        VikingWarship = 27,

        GreenWarship = 28,
        KnightWarship = 29,
        DarkLordWarship = 30,


        NUM = 35,
       
    }
}
