using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2.GameObjects.Gadgets;

namespace VikingEngine.LF2
{
    static class LootfestLib
    {
        public const int MaxEnemiesSpawn = 15;
        public const int MaxGameObjectsGoal = 40;

        public const int MaxLocalGamers = 4;
        public const int MaxGamers = 5;

        #region SAVE_LOAD
        public const string ContentFolder = "Lootfest\\";
        public const string DataFolder = ContentFolder + "Data\\";
        public const string MusicFolder = ContentFolder + "Music\\";
        public const string VoxelModelFolder = DataFolder + "VoxelObj";
        public const string SceneFolder = DataFolder + "Scene";
        public const string SceneModelFolder = SceneFolder + "\\Models";

        #endregion

        #region HEALTH_N_DAMAGE
        //Magic
        public const int HeroMagicBar = 100;
        public const int StaffFireMagicUse = 10;
        public const int StaffBlastMagicUse = 25;


        //Friendly
        public const int HeroHealth = 80;
        public const int FlyingPetHealth = 40;
        public const int BombSheepHealth = 60;
        public const int CritterHealth = 15;
        public const int NPCAttackDamage = 10;
        public const int StandardNPCHealth = 90;
        public const int GuardHealth = 90;
        //Enemies
        public const int MonsterLowCollisionDamage = 6;
        public const int MonsterMediumCollisionDamage = 12;
        public const int MonsterHighCollisionDamage = 16;

        public const int BasicCollisionDamageLvl1 = 10;
        //public const int BasicCollisionDamageLvl2 = 18;
        public const int EggNestHealth = 80;
        public const int GruntHealth = 16;
        public const int GruntStoneDamage = 4;
        public const int HumanoidHealthPerSize = 14;
        public const float HumanoidStandardArmor = 1;

        public const float HumanoidLeaderArmor = 2;
        public const float HumanoidBruteArmor = 0;
        public const float HumanoidBruteHealthBonus = 10;
        public const float HumanoidArcherArmor = 0.4f;
        public const float HumanoidArcherHealthPercent = 0.6f;

        public const float HogHealth = 45;
        public const int WolfHealth = 25;
        public const int SquigHealth = 30;
        public const int SquigSpawnHealth = 22;

        public const int CrockodileHealth = 40;
        public const int EntHealth = 60;
        public const int FireGoblinHealth = 40;
        public const int FrogHealth = 30;
        public const int HarpyHealth = 30;
        public const int LizardHealth = 40;
        public const int ScorpionHealth = 60;
        public const int SpiderHealth = 40;
        public const int MummyHealth = 60;
        public const int GhostHealth = 80;
        public const float CastleMonsterHealthLvlMulti = 0.1f;
        public const int HumanoidDamageLvl1 = 10;
        public const int HumanoidDamageLvl2 = 16;
        public const float HumanoidLeaderBoostDamage = 1.4f;
        public const int BeeHiveHealth = 40;
        public const int BeeHealth = 20;

        public const float Level2HealthMultiply = 3f;
        public const float Level2DamageMultiply = 1.8f;
        //Boss
        public const int OldSwineHealth = 100;
        public const float MagicianWeaknessMultiply = 4; //how much harder a weakness weapon will hurt
        public const int MagicianSwordBaseDamage = 20;
        public const int MagicianSwordLvlDamageAdd = 6;

        
        public const int MagicianBaseHealth = 200;
        public const int MagicianLvlHealthAdd = 160;

        public const int BossMountCollDamage = 20;
        public const int BossMountHealth = 200;
        public const int EndMagicianHealth = 300;
        public const int EndMagicianSwordDamage = 40;
        public const int EndMagicianProjectileDamage = 30;

        //Enemy weapons
        public const int RootAttackDamage = 25;
        
        public const int TurretBulletDamage = 14;
        public const int SquigBulletDamage = 10;
        public const int SquigSpawnBulletDamage = 5;
        public const int ScorpionBulletDamage = 12;
        public const int FireGoblinBall = 10;
        public const int FireBreathDamage = 16;

        //Weapons
        public const int WoodenStickDamage = 6;
        public const int ShortBowDamage = 18;
        public const int LongBowDamage = 26;
        public const int MetalBowDamage = 30;
        public const int SlingStoneDamage = 8;
        public const int JavelinDamage = 20;

        public const int WoodenSwordDamage = 14;
        public const int SwordDamage = 20;
        public const int LongSwordDamage = 22;
        public const int AxeDamage = 26;
        public const int LongAxeDamage = 30;
        public const int SpearDamage = 22;
        public const int KnifeDamage = 36;
        public const int StaffBasicDamage = 14;
        //public const int SickleDamage = 2;

       // public const float MithrilDamageBonus = 1.6f; //60% more damage
        public const float GoldenArrowDamageMultiplier = 3f;
        public const int FlyingPetDamage = 14;
        //Magic
        public const int PoisionMushroomDamage = 20;
        public const int PoisionConditionmDamage = 2;
        public const int ThunderStrikeDamage = 30;
        public const int FireBombDamage = 30;
        public const int HolyBombDamage = 40;
        public const int PoisionBombDamage = 10;
        public const int SpiderBombHealth = 80;
        public const int SpiderBombDamage = 20;
        public const int LightningSparkDamage = 40;

        public const int SecondaryFireDamage = 2;
        public const int SecondaryLightningDamage = 4;
        public const int MaxLightningChains = 6;
        public const int ElementFireDamage = 10;

        //Magic rings
        public const float TinyMagicBoost = 1.2f;
        public const float SmallMagicBoost = 1.4f;
        public const int EvilTouchDamage = 10;
        public const int EvilAuraFriendlyDamage = 2;
        public const int EvilAuraDamage = 5;
        public const float AppleLoverMulti = 1.2f;
        public const float HobbitSkillMulti = 1.2f;
        public const int ProjectileEvilBurstDamage = 4;
        public const int ProjectilePoisionBurstDamage = 10;
        public const int ProjectileFireBurstDamage = 15;
        public const int ProjectileFireBurstRadius = 3;
        public const int EvilMissileDamage = 6;
        public const float RingOfProtectionReduceTo = 0.8f;

        //Food
        public static readonly float[] FoodQualityHealAdd = new float[] { 0.6f, 1, 1.4f };
        public static readonly FoodHealingPower[] EatingOrderAndHealthRestore = new FoodHealingPower[] //Auto eat with RT
        {
            
            new FoodHealingPower( GoodsType.Grilled_apple, Quality.NUM, 0.3f),
            new FoodHealingPower( GoodsType.Grilled_meat, Quality.NUM, 0.4f),
            new FoodHealingPower( GoodsType.Bread, Quality.NUM, 0.45f),
            new FoodHealingPower( GoodsType.Apple_pie, Quality.NUM, 0.8f),
            new FoodHealingPower( GoodsType.Apple, Quality.NUM, 0.2f), //20% restore
            new FoodHealingPower( GoodsType.Meat, Quality.NUM, 0.1f),
            new FoodHealingPower( GoodsType.Grapes, Quality.NUM, 0.2f),
            new FoodHealingPower( GoodsType.Seed, Quality.NUM, 0.1f),
            
        };


        



        public static readonly IntervalF MetalDamageValueRange = new IntervalF(0.6f, 1.5f);
        public static float MetalDamageValue(GoodsType metal)
        {
            switch (metal)
            {
                case GoodsType.Bronze:
                    return MetalDamageValueRange.Min;
                case GoodsType.Silver:
                    return 1.2f;
                case GoodsType.Gold:
                    return 1.3f;
                case GoodsType.Mithril:
                    return MetalDamageValueRange.Max;
            }
            return 1;
        }

        #endregion

        #region PRICES
        public const int DeathCost = 10;
        public const int TravelCost = 5;
        public const int PrivateHomeCost = 400;
        public const int WaterRefillCost = 10;
        public const float LowQualPrice = 0.5f;
        public const float MedQualPrice = 1f;
        public const float HighQualPrice = 3f;
        const float BombValue = 2;
        public static float ItemsValue(GoodsType type)
        {
            switch (type)
            {
                case GoodsType.Arrow:
                    return 0.2f;
                case GoodsType.SlingStone:
                    return 0.05f;
                case GoodsType.Javelin:
                    return 0.3f;
                case GoodsType.Evil_bomb:
                    return BombValue;
                case GoodsType.Fire_bomb:
                    return BombValue;
                case GoodsType.Fluffy_bomb:
                    return BombValue;
                case GoodsType.Lightning_bomb:
                    return BombValue;
                case GoodsType.Poision_bomb:
                    return BombValue;
            }
            throw new NotImplementedException("ItemsValue " + type.ToString());
        }

        public static float GoodsValue(GoodsType type)
        {
            if (GadgetLib.IsGoodsType(type))
            {
                switch (type)
                {
                    case GoodsType.Apple:
                        return 1;
                    case GoodsType.Apple_pie:
                        return 5;
                    case GoodsType.Bread:
                        return 4;
                    
                    case GoodsType.Orc_mead:
                        return 10;
                    case GoodsType.Beer:
                        return 30;

                    case GoodsType.Bronze:
                        return 20;
                    case GoodsType.Crystal:
                        return 100;
                    case GoodsType.Diamond:
                        return 100;
                    case GoodsType.Feathers:
                        return 0.2f;
                    case GoodsType.Glass:
                        return 40;
                    case GoodsType.Broken_glass:
                        return 8;
                    case GoodsType.Gold:
                        return 50;
                    case GoodsType.Grapes:
                        return 8;
                    case GoodsType.Grilled_apple:
                        return 3;
                    case GoodsType.Grilled_meat:
                        return 3;
                    case GoodsType.Iron:
                        return 10;
                    case GoodsType.Meat:
                        return 1;
                    case GoodsType.Mithril:
                        return 200;
                    case GoodsType.Ruby:
                        return 100;
                    case GoodsType.sapphire:
                        return 100;
                    case GoodsType.Seed:
                        return 2;
                    case GoodsType.Silver:
                        return 30;
                    case GoodsType.Stick:
                        return 0.2f;
                    case GoodsType.Wine:
                        return 40;
                    case GoodsType.Wood:
                        return 1;

                    case GoodsType.Leather:
                        return 1;
                    case GoodsType.Skin:
                        return 1;
                    case GoodsType.Scaley_skin:
                        return 8;
                    case GoodsType.Sharp_teeth:
                        return 1;
                    case GoodsType.Nose_horn:
                        return 4;
                    case GoodsType.Horn:
                        return 2;
                    case GoodsType.Tusks:
                        return 2;
                    case GoodsType.Fur:
                        return 15;

                    case GoodsType.Blood_finger_herb:
                        return 6;
                    case GoodsType.Blue_rose_herb:
                        return 8;
                    case GoodsType.Fire_star_herb:
                        return 6;
                    case GoodsType.Frog_heart_herb:
                        return 4;

                    case GoodsType.Granite:
                        return 2;
                    case GoodsType.Flint:
                        return 3;
                    case GoodsType.Marble:
                        return 4;
                    case GoodsType.Sandstone:
                        return 1;

                }
            }
            else
            {
                switch (type)
                {
                    case GoodsType.Ancient_coins:
                        return 10;
                    case GoodsType.Barbarian_coins:
                        return 2;
                    case GoodsType.Dwarf_coins:
                        return 8;
                    case GoodsType.Elvish_coins:
                        return 30;
                    case GoodsType.Orc_coins:
                        return 1;
                    case GoodsType.South_kingdom_coins:
                        return 5;
                    case GoodsType.Cookie:
                        return 20;
                    
                }
            }
            throw new NotImplementedException("GoodsValue " + type.ToString());
            //return 1;
        }

        public static float StandardHandWeaponValue(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType type)
        {
            const float SwordToAxeValue = 0.6f;
            const float SwordToKnifeValue = 0.6f;
            const float BronzeToIronValue = 3.6f;
            const float ShortToLongValue = 1.6f;

            const float BronzeSword = 50;
            const float BronzeAxe = BronzeSword * SwordToAxeValue;
            const float BronzeLongSword = BronzeSword * ShortToLongValue;

            switch (type)
            {
                case GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeAxe:
                    return BronzeAxe;
                case GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeSword:
                    return BronzeSword;
                case GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeLongSword:
                    return BronzeLongSword;
                case GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronAxe:
                    return BronzeAxe * BronzeToIronValue;
                case GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronSword:
                    return BronzeSword * BronzeToIronValue;
                case GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronLongSword:
                    return BronzeLongSword * BronzeToIronValue;

                case GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeDagger:
                    return BronzeSword * SwordToKnifeValue;
                case GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronDagger:
                    return BronzeSword * SwordToKnifeValue * BronzeToIronValue;
                case GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeLongAxe:
                    return BronzeAxe * ShortToLongValue;
                case GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronLongAxe:
                    return BronzeAxe * ShortToLongValue * BronzeToIronValue;
                case GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.PickAxe:
                    return 200;
                case GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.Sickle:
                    return 400;

                case GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.Spear:
                    return 150;
            }
            throw new NotImplementedException("StandardHandWeaponValue " + type.ToString());
        }
        public static float StandardBowValue(Data.Gadgets.BluePrint type)
        {
            switch (type)
            {
                case Data.Gadgets.BluePrint.ShortBow:
                    return 60;
                case Data.Gadgets.BluePrint.LongBow:
                    return 190;
            }
            throw new NotImplementedException("StandardBowValue " + type.ToString());

        }

        #endregion

        #region CRAFTING
        public const int RefineMaterialExchange = 4;
        public const int CraftingSwordMetalAmount = 4;
        public const int CraftingAxeMetalAmount = 2;
        public const int CraftingSpearMetalAmount = 2;
        public const int CraftingLongAxeMetalAmount = 4;
        public const int CraftingKnifeMetalAmount = 2;
        public const int CraftingLongSwordMetalAmount = 8;
        public const int CraftingHelmetArmorAmount = 6;
        public const int CraftingBodyArmorAmount = 14;
        public const int CraftingShield1MetalAmount = 1;
        public const int CraftingShield2MetalAmount = 2;
        public const int CraftingShield3MetalAmount = 4;
        public const int CraftingBowMetalAmount = 2;
        public const int CraftingPickAxeMetalAmount = 2;
        public const int CraftingSickleMetalAmount = 2;

        public static float MaterialProtectionValue(GoodsType material)
        {
            switch (material)
            {
                case GoodsType.Skin:
                    return 0.3f;
                case GoodsType.Leather:
                    return 0.35f;
                case GoodsType.Scaley_skin:
                    return 0.4f;
                case GoodsType.Bronze:
                    return 0.45f;
                case GoodsType.Iron:
                    return 0.6f;
                case GoodsType.Silver:
                    return 0.7f;
                case GoodsType.Gold:
                    return 0.8f;
                case GoodsType.Mithril:
                    return 1f;
            }
            throw new ArgumentOutOfRangeException();
        }
        
        public const float MaxBodyArmor = 0.7f;
        public const float MaxHelmetArmor = 0.4f;

        public static readonly IntervalF QualityPercentToItemStrength = new IntervalF(0.2f, 1f);
        public const float StandardItemQualityPercent = 0.3f;
#endregion

        #region MAP

        public const int BossCount = 5;
        public const int NumCities = 4;
        public const int NumVillages = 4;
        //boss tips
        const int NumTipsPerBoss = 3;
        //Not start boss or start village or start city
        public const int TotalNumTips = (BossCount - 1) * NumTipsPerBoss + NumVillages - 1 + NumCities - 2; //18
        public const int TipsPerVillage = 2;
        public const int TipsPerCity = 3;
        const int TotalNumTipGivers = TipsPerVillage * NumVillages + TipsPerCity * NumCities; //8 + 12 = 20

        public const int NestsTips = TotalNumTipGivers - TotalNumTips; //3
        public const int PrivateAreaSize = 8;
        public const int PublicBuildAreaSize = 12;
        #endregion

        #region INPUT
        public const numBUTTON InputHealUp = numBUTTON.LB;
        public const SpriteName InputHealUpImg = SpriteName.ButtonLB;

        #endregion

        const int StandardHeavyWeight = 20;
        const int StandardLightWeight = 1;
        public const int WeaponWeight = StandardHeavyWeight;
        public const int ArmorWeight = StandardHeavyWeight;
        public const int SheildWeight = StandardHeavyWeight;
        public const int GoodsWeight = StandardLightWeight;
        public const int JeveleryWeight = StandardLightWeight;
        public const int MaxWeight = 400;


        public const string ViewBackText = "Press BACK";
        public static Data.Images Images = new Data.Images();


        //public static Debug.RealTimeFloatTweak HandSwordSize;
        //public static Debug.RealTimeFloatTweak ShieldScale;
        //public static Debug.RealTimeFloatTweak NPCYadj;
        public static void InitTweakValues()
        {
            //HandSwordSize = new Debug.RealTimeFloatTweak("HandSword Size", 0.2f);
            //ShieldScale = new Debug.RealTimeFloatTweak("Shield scale", 1.6f);
            //NPCYadj = new Debug.RealTimeFloatTweak("NPC y adj", -3.5f);
        }
        
        public static int LocalHostIx
        {
            get { return ((PlayState)LfRef.gamestate).LocalHostingPlayer.Index; }
        }

        public static string RandomTip(Players.PlayerProgress player)
        {
            return LfRef.gamestate.Progress.missionHelp(player);
        }

#region LOOT
        public static readonly IntervalF PickAxeAndSickleDamageRange = new IntervalF(1, SwordDamage);
        static readonly List<GoodsType> Lvl1Goods = new List<GoodsType>
        {
            GoodsType.Grilled_meat, 
            GoodsType.Stick, 
            GoodsType.Feathers,
            GoodsType.Grapes, 
            GoodsType.Animal_paw, 
            GoodsType.Bladder_stone, 
            GoodsType.Red_eye,
        };
        static readonly List<GoodsType> Lvl2Goods = new List<GoodsType>
        {
            GoodsType.Glass, 
            GoodsType.Bronze, 
            GoodsType.Bread, 
            GoodsType.Wine, 
            GoodsType.Grilled_meat, 
            GoodsType.Wine, 
            GoodsType.Seed, 
        };

        static readonly List<GoodsType> Lvl3Goods = new List<GoodsType>
        {
            GoodsType.Ruby, 
            GoodsType.sapphire, 
            GoodsType.Diamond, 
            GoodsType.Crystal, 
            GoodsType.Silver,
            GoodsType.Gold,
            GoodsType.Blood_finger_herb,
            GoodsType.Blue_rose_herb,
            GoodsType.Fire_star_herb,
            GoodsType.Frog_heart_herb,

            GoodsType.Plastma,
            GoodsType.Paper,
            GoodsType.Candle,
            GoodsType.Ink,
        };

        public static Goods GetRandomGoods(int lootLvl)
        {
            Goods result = new Goods(GoodsType.NONE,
                    lib.SmallestOfTwoValues(Ref.rnd.Int((int)Quality.NUM), Ref.rnd.Int((int)Quality.NUM)));
            int rndAmout = Ref.rnd.Int(100);
            if (rndAmout < 5)
                result.Amount = 3;
            else if (rndAmout < 30)
                result.Amount = 2;
            else
                result.Amount = 1;

            if (Ref.rnd.RandomChance(1 + 10 * lootLvl))
                ++result.Amount;

            int rnd = Ref.rnd.Int(lootLvl / 2);
            if (Ref.rnd.RandomChance(10))
                rnd++;
            List<GoodsType> list;
            switch (rnd)
            {
                case 0:
                    list = Lvl1Goods;
                    break;
                case 1:
                    list = Lvl2Goods;
                    break;
                default: //2+
                    list = Lvl3Goods;
                    break;

            }
            result.Type = list[Ref.rnd.Int(list.Count)];
            return result; 
        }

        static readonly List<GoodsType> Lvl1Items = new List<GoodsType>
        {
            GoodsType.Arrow,
            GoodsType.Coins,
            GoodsType.SlingStone,
            GoodsType.Orc_coins,
        };
        static readonly List<GoodsType> Lvl2Items = new List<GoodsType>
        {
            GoodsType.Javelin,
            GoodsType.Text_scroll,
            GoodsType.Dwarf_coins,
            GoodsType.Barbarian_coins,
            GoodsType.South_kingdom_coins,
            GoodsType.Ancient_coins,
            GoodsType.Empty_bottle,
            GoodsType.Cookie,
        };
        static readonly List<GoodsType> Lvl3Items = new List<GoodsType>
        {
            GoodsType.Evil_bomb,
            GoodsType.Fire_bomb,
            GoodsType.Fluffy_bomb,
            GoodsType.GoldenArrow,
            GoodsType.Lightning_bomb,
            GoodsType.Poision_bomb,

            
            GoodsType.Water_bottle,
         
            GoodsType.Golden_cookie,
            GoodsType.Repair_kit,

            
       
        };

        public static Item GetRandomItem(int lootLvl)
        {
            int lvl = lootLvl / 2;
            int listLength = Lvl1Items.Count;

            if (lvl >= 2)
            {
                listLength += Lvl2Items.Count;
            }
            if (lvl >= 3)
            {
                listLength += Lvl3Items.Count;
            }

            int itemIx = Ref.rnd.Int(listLength);
            GoodsType type;
            int maxAmount = lootLvl * 2 + 2;

            if (itemIx < Lvl1Items.Count)
            {
                type = Lvl1Items[itemIx];
            }
            else if (itemIx < Lvl1Items.Count + Lvl2Items.Count)
            {
                maxAmount /= 2;
                type = Lvl2Items[itemIx - Lvl1Items.Count];
            }
            else
            {
                maxAmount /= 3;
                type = Lvl3Items[itemIx - Lvl1Items.Count - Lvl2Items.Count];
            }

            return new Item(type, 1 + Ref.rnd.Int(maxAmount));
        }

        public static IGadget GetRandomRareGadget()
        {
            switch (Ref.rnd.Int(21))
            {
                default:
                    return new Item(GoodsType.GoldenArrow, 4);
                case 1:
                    return new Item(GoodsType.Holy_bomb, 1);
                case 2:
                    return new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronAxe);
                case 3:
                    return new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronSword);
                case 4:
                    return new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.LongBow);
                case 5:
                    return new GameObjects.Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.MetalBow);
                case 6:
                    return new GameObjects.Gadgets.Shield(ShieldType.Buckle);
                case 7:
                    return new GameObjects.Gadgets.Shield(ShieldType.Round);
                case 8:
                    return new GameObjects.Gadgets.Armor(GoodsType.Bronze, true);
                case 9:
                    return new GameObjects.Gadgets.Armor(GoodsType.Bronze, false);
                case 10:
                    return new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.Spear);
                case 11:
                    return new Goods(GoodsType.Mithril, (Quality)Ref.rnd.Int((int)Quality.NUM), 1);
                case 12:
                    return new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeLongSword);
                case 13:
                    return new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronLongSword);

                case 14:
                    return new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.PickAxe);
                case 15:
                    return new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.Sickle);
                case 16:
                    return new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeDagger);
                case 17:
                    return new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronDagger);
                case 18:
                    return new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeLongAxe);
                 case 19:
                    return new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronLongAxe);

                 case 20:
                    return new Item(GoodsType.Elvish_coins, 1);
            }
        }

        public static IGadget GetRandomAnyGadget(int lootLvl)
        {
            int rareChance = lib.SetMaxVal(lootLvl * 3, 30);
            if (Ref.rnd.RandomChance(rareChance))
            {
                return GetRandomRareGadget();
            }
            if (Ref.rnd.RandomChance(70))
            {
                return GetRandomGoods(lootLvl);
            }
            else
            {
                return GetRandomItem(lootLvl);
            }
        }

#endregion

#region TROPHY
        public const int Trophy_HitEnemiesWithSword = 4;
        public const int Trophy_HitEnemiesWithAxe = 3;
        public const int Trophy_HitEnemiesWithSpear = 2;
        public const int Trophy_KillEnemiesWithAttack = 3;
        public const int Trophy_HitWithBow = 10;
        public const int Trophy_HitWithJavelin = 6;
        public const int Trophy_KillOfEachEnemyType = 10;
        public const int Trophy_HitWithLightning = MaxLightningChains;
        public const int Trophy_KillWithoutHit = 50;
        public const int Trophy_BarrelKill = 1;

        
        public static string TrophyDescription(Trophies type)
        {
            switch (type)
            {
                case Trophies.CraftMithrilBodyArmor: return "Craft a mithril body armor";
                case Trophies.CraftGoldSword: return "Craft a sword from gold";
                case Trophies.Hit4EnemiesInOneSwordAttack: return "Hit 4 enemies with one sword swing. Must be a one handed sword.";
                case Trophies.Hit3EnemiesInOneAxeAttack: return "Hit 3 enemies with one axe swing";
                case Trophies.Hit2EnemiesInOneSpearAttack: return "Hit " + Trophy_HitEnemiesWithSpear.ToString() + " enemies with one spear attack";
                case Trophies.Kill3EnemiesInOneAttack: return "Kill 3 enemies in one attack, using a one handed weapon.";
                case Trophies.Hit10BowShots: return "Hit 10 times in a row with a bow";
                case Trophies.Hit6Javelins: return "Hit 6 times in a row with a javelin";
                case Trophies.DefeatFinalBoss: return "Defeat the final boss";
                case Trophies.Kill10OfEachEnemyType: return "Kill " + Trophy_KillOfEachEnemyType.ToString() +" of each monster type";
                case Trophies.DefeatBossWithStick: return "Kill a boss with the default stick weapon";
                case Trophies.Hit6EnemiesWithLightning: return "Hit " + Trophy_HitWithLightning.ToString() + " enemies with a chain lightning";

                case Trophies.Kill25WithoutBeingHit: return "Kill " + Trophy_KillWithoutHit.ToString() + " enemies in a row, without taking damage";
                case Trophies.Kill3InABarrelBlast: return "Blow up one enemy in a barrel blast";
                case Trophies.CompleteGameWihtOutSingleDeath: return "Complete the game without a single death, including your coop partners";

            }
            throw new NotImplementedException();
        }

        public static SpriteName TrophyIcon(Trophies type)
        {
            switch (type)
            {
                case Trophies.CraftMithrilBodyArmor: return SpriteName.ArmourMithril;
                case Trophies.CraftGoldSword: return SpriteName.WeaponSwordGold;
                case Trophies.Hit4EnemiesInOneSwordAttack: return SpriteName.TrophySword;
                case Trophies.Hit3EnemiesInOneAxeAttack: return SpriteName.TrophyAxe;
                case Trophies.Hit2EnemiesInOneSpearAttack: return SpriteName.TrophySpear;
                case Trophies.Kill3EnemiesInOneAttack: return SpriteName.TrophyTrippleKill;
                case Trophies.Hit10BowShots: return SpriteName.WeaponShortBow;
                case Trophies.Hit6Javelins: return SpriteName.IconThrowSpear;
                case Trophies.DefeatFinalBoss: return SpriteName.TrophyCompleteGame;
                case Trophies.Kill10OfEachEnemyType: return SpriteName.TrophyKillMonsters;
                case Trophies.DefeatBossWithStick: return SpriteName.TrophyStick;
                case Trophies.Hit6EnemiesWithLightning: return SpriteName.TrophyLighting;

                case Trophies.Kill25WithoutBeingHit: return SpriteName.TrophyHealthy;
                case Trophies.Kill3InABarrelBlast: return SpriteName.TrophyBarrelBlast;
                case Trophies.CompleteGameWihtOutSingleDeath: return SpriteName.LFHealIcon;

            }
            throw new NotImplementedException();
        }

        public static void PrintError(string err)
        {
            LfRef.gamestate.AddChat(new ChatMessageData(err, "ERROR"), true);
        }
#endregion

#region LANGUAGE
        public const string MoneyText = " coins";
#endregion
    }
    enum MenuPageName
    {
        SalesmanSell,
        SalesmanBuy,
        
    }
    enum Trophies
    {
        CraftMithrilBodyArmor, CraftGoldSword,//-crafta en mithril rustning, guld svärd
        Hit4EnemiesInOneSwordAttack, Hit3EnemiesInOneAxeAttack, Hit2EnemiesInOneSpearAttack,//-träffa/döda flera i ett sving (4svärd, 3yxa, 2spjut)
        Kill3EnemiesInOneAttack,
        Hit10BowShots,//-skjut 10bågskott som träffar i rad
        Hit6Javelins,//-skjut 6javelins -||-
        Kill3InABarrelBlast,
        Kill10OfEachEnemyType,//-döda en av varje fiende typ
        Kill25WithoutBeingHit,
        DefeatBossWithStick,//-döda en boss med träpinnen
        Hit6EnemiesWithLightning,//-yx lightning 6 träff/kills
        DefeatFinalBoss,//-klara slutbossen
        CompleteGameWihtOutSingleDeath,
        NUM,
    }
}
