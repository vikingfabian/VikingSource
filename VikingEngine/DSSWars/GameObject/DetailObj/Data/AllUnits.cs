using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.GameObject.DetailObj.Soldiers;
using VikingEngine.LootFest;
using VikingEngine.LootFest.BlockMap.Level;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.DSSWars.GameObject
{
    class AllUnits
    {
        public static float AverageGroupStrength;
        public const float HealthToStrengthConvertion = 0.36f;

        AbsSoldierBuilder[] profiles = new AbsSoldierBuilder[(int)UnitBuildType.NUM];
        //public CityDetailProfile city;
        public BannerManBuilder bannerman;

        public AllUnits()
        {
            DssRef.units = this;

            //city = new CityDetailProfile();
            bannerman = new BannerManBuilder();

            add(bannerman);

            add(new ConscriptedSoldierBuilder());
            add(new ConscriptedWarshipData());

            add(new WarmachineProfile());
            add(new CavalryBuilder());
            add(new WagonBuilder());
            add(new HoundBuilder());

            //add(new DarkLordBuilder());
            //add(new DarkLordWarshipData());

            add(new CityGuardSoldierBuilder());

            var defaultShield = DssVar.Shields[Resource.ItemResourceType.RoundShield];
            int defaultAttackDamage = DssConst.WeaponDamage_Sword;
            int defaultDps = DPS(defaultAttackDamage, DssConst.Soldier_StandardAttackAndCoolDownTime);//Convert.ToInt32(defaultAttackDamage / (DssConst.Soldier_StandardAttackAndCoolDownTime / 1000.0));
            defaultDps = MathExt.MultiplyInt(defaultDps, 1f + defaultShield.meleeSpeedBonus);
            //int defaultDps = DssRef.profile.Get(UnitType.Soldier).DPS_land();
            AverageGroupStrength = GroupStrengh_Raw(DssConst.SoldierGroup_DefaultCount, defaultDps, DssConst.Soldier_DefaultHealth + defaultShield.armorBonus);//DssConst.SoldierGroup_DefaultCount * (defaultDps + HealthToStrengthConvertion * DssConst.Soldier_DefaultHealth) ;
            
        }

        public bool IsShip(UnitBuildType type)
        {
            return profiles[(int)type].IsShip();
        }

        static int DPS(int damage, float attackAndCoolDownTime)
        {
            return Convert.ToInt32(damage / (attackAndCoolDownTime / 1000.0));
        }
        static float GroupStrengh_Raw(int soldierCount, float dps, int health)
        {
            return soldierCount * (dps + HealthToStrengthConvertion * health);
        }

        public static float GroupStrengh(int soldierCount, ref SoldierData data, bool land)
        {
            //int damage;
            //if (land)
            //{ 
            
            //}
            //else
            //{
            //    crewCount = MathExt.Div_Ceiling(this.health, data.basehealth);

            //}

            var raw = GroupStrengh_Raw(soldierCount, DPS(land? data.attackDamage : data.attackDamageSea, data.attackTimePlusCoolDown), data.basehealth);
            
            return raw / AverageGroupStrength;
        }

        public void AddRawModelsToLoad(List<VoxelModelName> modelNames)
        {
            modelNames.AddRange(
              new List<VoxelModelName>() {
                LootFest.VoxelModelName.war_bannerman,

                LootFest.VoxelModelName.wars_soldier,
                LootFest.VoxelModelName.wars_soldier_i2,
                LootFest.VoxelModelName.wars_soldier_i3,
                LootFest.VoxelModelName.wars_longsword,

                LootFest.VoxelModelName.wars_piker,
                LootFest.VoxelModelName.wars_spearman,

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

                 //LootFest.VoxelModelName.wars_darklord,
            });
        }

        public List<VoxelModelName> AddUniqueModelsToLoad()
        {
            return new List<VoxelModelName>() {

                 LootFest.VoxelModelName.wars_darklord,
                LootFest.VoxelModelName.wars_rosewarrior,
                LootFest.VoxelModelName.wars_rosetank,
                LootFest.VoxelModelName.wars_rosedog,

            };
        }

        void add(AbsSoldierBuilder builder)
        {
            profiles[(int)builder.unitBuildType] = builder;
        }

        public AbsSoldierBuilder Get(UnitBuildType type)
        {
            return profiles[(int)type];
        }

        public static SpriteName UnitFilterIcon(UnitFilterType filterType)
        {
            switch (filterType)
            {
                case UnitFilterType.Settler:
                    return SpriteName.WarsSettler;
                case UnitFilterType.SharpStick:
                    return SpriteName.WarsUnitIcon_Folkman;

                case UnitFilterType.SpearAndShield:
                    return SpriteName.LittleUnitIconSpearman;


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

                case UnitFilterType.Skirmisher:
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

      
    }

    enum UnitFilterType
    { 
        Settler,
        SharpStick,
        Sword,
        LongSword,
        Pike,
        SpearAndShield,
        Warhammer,
        TwohandSword,
        Knight,
        MithrilKnight,

        Skirmisher,
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
        RoseWarrior,
        NUM
    }

    enum UnitBuildType
    {
        NULL = -1,
        //King = 34,
        //KingsGuard = 35,
        //Recruit =0,
        Conscript = 0,
        ConscriptWarship = 1,
        BannerMan = 2,
        ConscriptCavalry = 3,
        ConscriptWagon = 4,
        ConscriptHound = 5,
        ConscriptWarmachine = 6,
        //DarkLordWarship = 6,
        //DarkLord = 7,

        CityGuard = 7,
        CityGuardWarship = 8,
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
