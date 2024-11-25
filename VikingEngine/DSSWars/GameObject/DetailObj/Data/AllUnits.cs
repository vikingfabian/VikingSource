using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using VikingEngine.DSSWars.GameObject.DetailObj.Soldiers;
using VikingEngine.LootFest;
using VikingEngine.LootFest.BlockMap.Level;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.DSSWars.GameObject
{
    class AllUnits
    {
        public static float AverageGroupStrength;
        public const float HealthToStrengthConvertion = 0.5f;

        AbsSoldierProfile[] profiles = new AbsSoldierProfile[(int)UnitType.NUM];
        public CityDetailProfile city;
        public BannerManProfile bannerman;

        public AllUnits()
        {
            DssRef.profile = this;

            city = new CityDetailProfile();
            bannerman = new BannerManProfile();

            add(bannerman);

            add(new ConscriptedSoldierProfile());
            add(new ConscriptedWarshipData());

            add(new WarmashineProfile());
            add(new CavalryProfile());

            add(new DarkLordProfile());
            add(new DarkLordWarshipData());
            //add(new FolkWarshipData(UnitType.FolkWarship, 
            //    add(new FolkManData())));

            //add(new SoldierWarshipData(UnitType.SoldierWarship,
            //    add(new SoldierData())));

            //add(new SoldierWarshipData(UnitType.HonorGuardWarship,
            //    add(new HonorGuardData())));

            //add(new VikingWarshipData(UnitType.SailorWarship, 
            //    add(new SailorData())));

            //add(new KnightWarshipData(UnitType.KnightWarship,
            //    add(new KnightData())));

            //add(new ArcherWarshipData(UnitType.ArcherWarship,
            //    add(new ArcherData())));

            //add(new BallistaWarshipData(UnitType.BallistaWarship,
            //    add(new BallistaData())));

            //add(new BannerManData());

            //add(new SoldierWarshipData(UnitType.PikemanWarship,
            //    add(new Pikeman())));

            //add(new ArcherWarshipData(UnitType.CrossbowWarship,
            //    add(new CrossBow())));

            //add(new BallistaWarshipData(UnitType.TrollcannonWarship,
            //    add(new TrollCannon())));

            //add(new KnightWarshipData(UnitType.GreenWarship,
            //    add(new GreenSoldier())));

            //add(new VikingWarshipData(UnitType.VikingWarship,
            //    add(new Viking())));

            //add(new DarkLordWarshipData(UnitType.DarkLordWarship,
            //    add(new DarkLordData())));

            //var recruit = recruits[(int)UnitType.Soldier];
            //add(new FolkWarshipData(UnitType.RecruitWarship,
            //    recruit));

            int defaultAttackDamage = 50;
            int defaultDps = Convert.ToInt32(defaultAttackDamage / (DssConst.Soldier_StandardAttackAndCoolDownTime / 1000.0));
            //int defaultDps = DssRef.profile.Get(UnitType.Soldier).DPS_land();
            AverageGroupStrength = DssConst.SoldierGroup_DefaultCount * (defaultDps + HealthToStrengthConvertion * DssConst.Soldier_DefaultHealth) ;
        }

        public void AddModelsToLoad(List<VoxelModelName> modelNames)
        {
            modelNames.AddRange(
              new List<VoxelModelName>() {
                LootFest.VoxelModelName.war_bannerman,

                LootFest.VoxelModelName.wars_soldier,
                LootFest.VoxelModelName.wars_soldier_i2,
                LootFest.VoxelModelName.wars_soldier_i3,
                LootFest.VoxelModelName.wars_longsword,

                LootFest.VoxelModelName.wars_piker,

                LootFest.VoxelModelName.war_folkman,

                LootFest.VoxelModelName.war_sailor,
                LootFest.VoxelModelName.war_sailor_i2,

                LootFest.VoxelModelName.wars_hammer,
                LootFest.VoxelModelName.wars_twohand,
                LootFest.VoxelModelName.wars_mithrilman,
                LootFest.VoxelModelName.wars_mithrilarcher,

                LootFest.VoxelModelName.war_knight,
                LootFest.VoxelModelName.war_knight_i2,
                LootFest.VoxelModelName.war_knight_i3,

                LootFest.VoxelModelName.wars_slingman,
                LootFest.VoxelModelName.wars_javelin,
                LootFest.VoxelModelName.war_archer,
                LootFest.VoxelModelName.war_archer_i2,

                LootFest.VoxelModelName.wars_crossbow,
                 LootFest.VoxelModelName.wars_handcannon,
                 LootFest.VoxelModelName.wars_culvertin,

                LootFest.VoxelModelName.war_ballista,
                LootFest.VoxelModelName.war_ballista_i2,

                LootFest.VoxelModelName.wars_manuballista,
                LootFest.VoxelModelName.wars_catapult,
                LootFest.VoxelModelName.wars_bronzesiegecannon,
                LootFest.VoxelModelName.wars_bronzemancannon,
                LootFest.VoxelModelName.wars_ironsiegecannon,
                LootFest.VoxelModelName.wars_ironmancannon,

                LootFest.VoxelModelName.little_hirdman,

                LootFest.VoxelModelName.wars_soldier_ship,
                LootFest.VoxelModelName.wars_archer_ship,
                LootFest.VoxelModelName.wars_folk_ship,
                LootFest.VoxelModelName.wars_viking_ship,
                LootFest.VoxelModelName.wars_ballista_ship,
                LootFest.VoxelModelName.wars_knight_ship,

                 LootFest.VoxelModelName.wars_darklord,
            });

            //foreach (var s in soldiers)
            //{
            //    if (s != null)
            //    {
            //        for (int i = 0; i < s.modelVariationCount; ++i)
            //        {
            //            arraylib.ListAddIfNotExist(modelNames, s.modelName + i);
            //        }
            //    }
            //}
        }

        AbsSoldierProfile add(AbsSoldierProfile soldier)
        {
            profiles[(int)soldier.unitType] = soldier;

            //recruits[(int)soldier.unitType] = new RecruitData(soldier);

            return soldier;
        }

        public AbsSoldierProfile Get(UnitType type)
        {
            return profiles[(int)type];
        }

        public static SpriteName UnitFilterIcon(UnitFilterType filterType)
        {
            switch (filterType)
            {
                case UnitFilterType.SharpStick:
                    return SpriteName.WarsUnitIcon_Folkman;


                case UnitFilterType.Sword:
                    return SpriteName.WarsUnitIcon_Soldier;
                case UnitFilterType.LongSword:
                    return SpriteName.WarsUnitIcon_Longsword;
                case UnitFilterType.Pike:
                    return SpriteName.WarsUnitIcon_Pikeman;

                case UnitFilterType.Warhammer:
                    return SpriteName.WarsUnitIcon_Hammerknight;
                case UnitFilterType.TwohandSword:
                    return SpriteName.WarsUnitIcon_TwoHand;
                case UnitFilterType.Knight:
                    return SpriteName.WarsUnitIcon_Knight;
                case UnitFilterType.MithrilKnight:
                    return SpriteName.WarsUnitIcon_MithrilMan;
                case UnitFilterType.MithrilBow:
                    return SpriteName.WarsUnitIcon_MithrilArcher;

                case UnitFilterType.Slingshot:
                    return SpriteName.WarsUnitIcon_Slingshot;
                case UnitFilterType.Throwingspear:
                    return SpriteName.WarsUnitIcon_Javelin;
                case UnitFilterType.Bow:
                    return SpriteName.WarsUnitIcon_Archer;
                case UnitFilterType.CrossBow:
                    return SpriteName.LittleUnitIconCrossBowman;

                case UnitFilterType.Rifle:
                    return SpriteName.WarsUnitIcon_BronzeRifle;
                case UnitFilterType.Shotgun:
                    return SpriteName.WarsResource_BronzeShotgun;

                case UnitFilterType.Ballista:
                    return SpriteName.WarsUnitIcon_Ballista;
                case UnitFilterType.ManuBallista:
                    return SpriteName.WarsUnitIcon_Manuballista;
                case UnitFilterType.Catapult:
                    return SpriteName.WarsUnitIcon_Catapult;

                case UnitFilterType.SiegeCannonBronze:
                    return SpriteName.WarsUnitIcon_BronzeSiegeCannon;
                case UnitFilterType.ManCannonBronze:
                    return SpriteName.WarsResource_BronzeManCannon;
                case UnitFilterType.SiegeCannonIron:
                    return SpriteName.WarsResource_IronSiegeCannon;
                case UnitFilterType.ManCannonIron:
                    return SpriteName.WarsUnitIcon_IronManCannon;


                case UnitFilterType.GreenSoldier:
                    return SpriteName.WarsUnitIcon_Greensoldier;
                case UnitFilterType.HonourGuard:
                    return SpriteName.WarsUnitIcon_Honorguard;
                case UnitFilterType.Viking:
                    return SpriteName.WarsUnitIcon_Viking;
                case UnitFilterType.DarkLord:
                    return SpriteName.WarsDarkLordBossIcon;

                default:
                    return SpriteName.NO_IMAGE;
            }
        }

        //public AbsSoldierUnit createSoldier(UnitType type, bool recruit)
        //{
        //    AbsSoldierProfile data;
        //    if (recruit)
        //    {
        //        data = recruits[(int)type];
        //    }
        //    else
        //    {
        //        data = soldiers[(int)type];
        //    }
        //    var soldier = data.CreateUnit() as AbsSoldierUnit;//TODO borde ta bort konvertering
        //    soldier.data = data;

        //    return soldier;
        //}

        //public AbsSoldierUnit createSoldier(AbsSoldierProfile data)
        //{
        //    //AbsSoldierData data;
        //    //if (recruit)
        //    //{
        //    //    data = recruits[(int)type];
        //    //}
        //    //else
        //    //{
        //    //    data = soldiers[(int)type];
        //    //}
        //    var soldier = data.CreateUnit() as AbsSoldierUnit;//TODO borde ta bort konvertering
        //    soldier.soldierData = data;

        //    return soldier;
        //}

        //public string Name(UnitType type)
        //{
        //    switch (type)
        //    {
        //        case UnitType.Recruit:
        //            return DssRef.lang.UnitType_Recruit;

        //        case UnitType.Soldier:
        //            return DssRef.lang.UnitType_Soldier;

        //        case UnitType.Sailor:
        //            return DssRef.lang.UnitType_Sailor;

        //        case UnitType.Folkman:
        //            return DssRef.lang.UnitType_Folkman;

        //        case UnitType.Spearman:
        //            return DssRef.lang.UnitType_Spearman;

        //        case UnitType.HonorGuard:
        //            return DssRef.lang.UnitType_HonorGuard;

        //        case UnitType.Pikeman:
        //            return DssRef.lang.UnitType_Pikeman;

        //        case UnitType.Knight:
        //            return DssRef.lang.UnitType_Knight;

        //        case UnitType.Archer:
        //            return DssRef.lang.UnitType_Archer;

        //        case UnitType.CrossBow:
        //            return DssRef.lang.UnitType_Crossbow;

        //        case UnitType.Ballista:
        //            return DssRef.lang.UnitType_Ballista;

        //        case UnitType.Trollcannon:
        //            return DssRef.lang.UnitType_Trollcannon;

        //        case UnitType.GreenSoldier:
        //            return DssRef.lang.UnitType_GreenSoldier;

        //        case UnitType.Viking:
        //            return DssRef.lang.UnitType_Viking;

        //        case UnitType.DarkLord:
        //            return DssRef.lang.UnitType_DarkLord;

        //        case UnitType.BannerMan:
        //            return DssRef.lang.UnitType_Bannerman;

        //        // Warship cases
        //        case UnitType.SoldierWarship:
        //            return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Soldier);

        //        case UnitType.SailorWarship:
        //            return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Sailor);

        //        case UnitType.HonorGuardWarship:
        //            return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_HonorGuard);

        //        case UnitType.PikemanWarship:
        //            return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Pikeman);

        //        case UnitType.KnightWarship:
        //            return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Knight);

        //        case UnitType.ArcherWarship:
        //            return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Archer);

        //        case UnitType.CrossbowWarship:
        //            return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Crossbow);

        //        case UnitType.BallistaWarship:
        //            return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Ballista);

        //        case UnitType.TrollcannonWarship:
        //            return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Trollcannon);

        //        case UnitType.VikingWarship:
        //            return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_Viking);

        //        case UnitType.DarkLordWarship:
        //            return string.Format(DssRef.lang.UnitType_WarshipWithUnit, DssRef.lang.UnitType_DarkLord);

        //        default:
        //            return "???";
        //    }
        //}
    }

    enum UnitFilterType
    { 
        SharpStick,
        Sword,
        LongSword,
        Pike,
        Warhammer,
        TwohandSword,
        Knight,
        MithrilKnight,

        Slingshot,
        Throwingspear,
        Bow,
        CrossBow,
        MithrilBow,

        Rifle,
        Shotgun,

        Ballista,
        ManuBallista,
        Catapult,

        SiegeCannonBronze,
        ManCannonBronze,
        SiegeCannonIron,
        ManCannonIron,

        HonourGuard,
        Viking,
        GreenSoldier,
        DarkLord,
        NUM
    }

    enum UnitType
    {
        NULL = -1,
        //King = 34,
        //KingsGuard = 35,
        //Recruit =0,
        Conscript = 0,
        ConscriptWarship = 1,
        BannerMan = 2,
        ConscriptCavalry = 3,
        ConscriptWarmashine = 4,
        DarkLordWarship = 5,
        DarkLord = 6,
        //Soldier =1,
        //Sailor =2,
        //Folkman =3,
        //Spearman =4,
        //HonorGuard=10,
        //Pikeman =5,
        //Knight=6,
        //Archer=7,
        //CrossBow=8,

        //Ballista=9,        
        //Trollcannon=11,
        //GreenSoldier = 13,
        //Viking = 14,
        //DarkLord = 16,
        //BannerMan =12,7
        NUM,
        City,

        //RecruitWarship = 17,
        //FolkWarship = 18,

        //SoldierWarship = 19,
        //HonorGuardWarship = 20,
        //PikemanWarship = 21,

        //ArcherWarship = 22,
        //CrossbowWarship = 23,

        //BallistaWarship = 24,
        //TrollcannonWarship = 25,

        //SailorWarship = 26,
        //VikingWarship = 27,

        //GreenWarship = 28,
        //KnightWarship = 29,
        //DarkLordWarship = 30,


        
       
    }
}
