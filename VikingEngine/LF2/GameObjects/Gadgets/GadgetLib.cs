using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2.GameObjects.Gadgets.WeaponGadget;

namespace VikingEngine.LF2.GameObjects.Gadgets
{
    interface IGadget
    {
        GadgetType GadgetType { get; }
        bool EquipAble { get; }
        SpriteName Icon { get; }
        string GadgetInfo { get; }
        ushort ItemHashTag { get; }
        int StackAmount { get; set; }
        int Weight { get;  }

        void WriteStream(System.IO.BinaryWriter w);
        void ReadStream(System.IO.BinaryReader r, byte version, GameObjects.Gadgets.WeaponGadget.GadgetSaveCategory saveCategory);

        /// <summary>
        /// When the item is equipped by its user
        /// </summary>
        void EquipEvent();
        bool Scrappable { get; }
        GadgetList ScrapResult();
        
        /// <summary>
        /// If the item should be treated as null, default = false
        /// </summary>
        bool Empty { get; }
    }

    static class GadgetLib
    {
        public const byte FistReleaseVersion = 1;
        public const byte SaveVersion = 2;
        public static SpriteName GadgetIcon(GoodsType type)
        {
            switch (type)
            {
                case GoodsType.Apple:
                    return SpriteName.GoodsApple;
                case GoodsType.Apple_pie:
                    return SpriteName.LFApplePie;
                case GoodsType.Bread:
                    return SpriteName.GoodsBread;
                case GoodsType.Broken_glass:
                    return SpriteName.GoodsGlassBroken;
                case GoodsType.Bronze:
                    return SpriteName.GoodsMetalBronze;
                case GoodsType.Crystal:
                    return SpriteName.GoodsGemChrystal;
                case GoodsType.Diamond:
                    return SpriteName.GoodsGemDiamond;
                case GoodsType.Fur:
                    return SpriteName.GoodsFur;
                case GoodsType.Feathers:
                    return SpriteName.GoodsFeather;
                case GoodsType.Flint:
                    return SpriteName.GoodsStoneFlint;
                case GoodsType.Glass:
                    return SpriteName.GoodsGlass;

                case GoodsType.Grapes:
                    return SpriteName.GoodsGrapes;
                case GoodsType.Gold:
                    return SpriteName.GoodsMetalGold;
                case GoodsType.Grilled_apple:
                    return SpriteName.GoodsAppleGrilled;
                case GoodsType.Granite:
                    return SpriteName.GoodsStoneGranit;
                case GoodsType.Grilled_meat:
                    return SpriteName.GoodsGrilledMeat;
                case GoodsType.Horn:
                    return SpriteName.GoodsHorn;
                case GoodsType.Iron:
                    return SpriteName.GoodsMetalIron;
                case GoodsType.Leather:
                    return SpriteName.GoodsLeather;

                case GoodsType.Mithril:
                    return SpriteName.GoodsMetalMithril;

                case GoodsType.Marble:
                    return SpriteName.GoodsStoneMarmour;


                case GoodsType.Meat:
                    return SpriteName.GoodsMeat;
                case GoodsType.Nose_horn:
                    return SpriteName.GoodsNoseHorn;
                case GoodsType.Ruby:
                    return SpriteName.GoodsGemRed;
                case GoodsType.Silver:
                    return SpriteName.GoodsMetalSilver;
                case GoodsType.Sandstone:
                    return SpriteName.GoodsStoneSand;
                case GoodsType.sapphire:
                    return SpriteName.GoodsGemBlack;
                case GoodsType.Stick:
                    return SpriteName.GoodsStick;
                case GoodsType.Skin:
                    return SpriteName.GoodsSkinn;
                case GoodsType.Scaley_skin:
                    return SpriteName.GoodsLeatherScaled;
                case GoodsType.Seed:
                    return SpriteName.GoodsSeed;
                case GoodsType.Sharp_teeth:
                    return SpriteName.GoodsTeeth;
                case GoodsType.Tusks:
                    return SpriteName.GoodsTusk;
                case GoodsType.Wine:
                    return SpriteName.GoodsWine;
                case GoodsType.Wood:
                    return SpriteName.GoodsWood;

                case GoodsType.Blood_finger_herb:
                    return SpriteName.GoodsBloodFingerHerb;
                case GoodsType.Blue_rose_herb:
                    return SpriteName.GoodsBlueRoseHerb;
                case GoodsType.Fire_star_herb:
                    return SpriteName.GoodsFireStarHerb;
                case GoodsType.Frog_heart_herb: return SpriteName.GoodsFrogHeartHerb;

                case GoodsType.Black_tooth: return SpriteName.GoodsBlackTooth;
                case GoodsType.Bladder_stone: return SpriteName.GoodsBladderStone;
                case GoodsType.Rib: return SpriteName.GoodsRib;
                case GoodsType.Plastma: return SpriteName.GoodsPlastma;
                case GoodsType.Thread: return SpriteName.GoodsThread;
                case GoodsType.Coal: return SpriteName.GoodsCoal;
                case GoodsType.Ink: return SpriteName.GoodsInk;
                case GoodsType.Poision_sting: return SpriteName.GoodsPoisionSting;
                case GoodsType.Honny: return SpriteName.GoodsHonny;
                case GoodsType.Wax: return SpriteName.GoodsVax;
                case GoodsType.Red_eye: return SpriteName.GoodsBlackEye;
                case GoodsType.Animal_paw: return SpriteName.GoodsPaw;

                case GoodsType.Arrow:
                    return SpriteName.LFArrow;
                case GoodsType.Coins:
                    return SpriteName.LFIconCoins;
                case GoodsType.Javelin:
                    return SpriteName.IconThrowSpear;
                case GoodsType.SlingStone:
                    return SpriteName.GoodsSlingstone;
                case GoodsType.Evil_bomb:
                    return SpriteName.ItemPurpleBottle;
                case GoodsType.Fire_bomb:
                    return SpriteName.ItemRedBottle;
                case GoodsType.Fluffy_bomb:
                    return SpriteName.ItemYellowBottle;
                case GoodsType.Lightning_bomb:
                    return SpriteName.ItemBlueBottle;
                case GoodsType.Poision_bomb:
                    return SpriteName.ItemGreenBottle;
                case GoodsType.GoldenArrow:
                    return SpriteName.ItemGoldenArrow;
                case GoodsType.Holy_bomb:
                    return SpriteName.ItemGoldenBomb;
                   
                    
                case GoodsType.Cookie: return SpriteName.ItemCookie;
                case GoodsType.Golden_cookie: return SpriteName.ItemGoldenCookie;
                case GoodsType.Candle: return SpriteName.ItemCandle;
                case GoodsType.Beer: return SpriteName.ItemBeer;
                case GoodsType.Orc_mead: return SpriteName.ItemOrcMead;
                case GoodsType.Lucky_paw: return SpriteName.ItemLuckyPaw;
                case GoodsType.Orc_coins: return SpriteName.ItemCoinOrc;
                case GoodsType.Dwarf_coins: return SpriteName.ItemCoinDwarf;
                case GoodsType.South_kingdom_coins: return SpriteName.ItemCoinSouth;
                case GoodsType.Elvish_coins: return SpriteName.ItemCoinElf;
                case GoodsType.Ancient_coins: return SpriteName.ItemCoinAncient;
                case GoodsType.Barbarian_coins: return SpriteName.ItemCoinNordic;
                case GoodsType.Water_bottle: return SpriteName.ItemWaterFull;
                case GoodsType.Empty_bottle: return SpriteName.ItemWaterEmpty;
                case GoodsType.Holy_water: return SpriteName.GoodsHolyWater;
                case GoodsType.Text_scroll: return SpriteName.GoodsScroll;
                case GoodsType.Monster_egg: return SpriteName.IconEggNestDestroyed;

                case GoodsType.Repair_kit: return SpriteName.ItemRepairKit;
            }
            return SpriteName.NO_IMAGE;
        }

        public static bool IsGoodsType(GoodsType type)
        {
            return type > GoodsType.GOODS_START && type < GoodsType.END_GOODS;
        }

        public static GoodsType BluePrintToGoodstype(Data.Gadgets.BluePrint bp)
        {
            switch (bp)
            {
                case Data.Gadgets.BluePrint.FireBomb: return GoodsType.Fire_bomb;
                case Data.Gadgets.BluePrint.LightningBomb: return GoodsType.Lightning_bomb;
                case Data.Gadgets.BluePrint.PoisionBomb: return GoodsType.Poision_bomb;
                case Data.Gadgets.BluePrint.EvilBomb: return GoodsType.Evil_bomb;
                case Data.Gadgets.BluePrint.FluffyBomb: return GoodsType.Fluffy_bomb;


                default: throw new NotImplementedException("BluePrintToGoodstype, " + bp.ToString());
            }
        }


        public static SpriteName HandsIcon(UseHands hands)
        {
            switch (hands)
            {
                default:
                    return SpriteName.LFIconQuickDraw;
                case UseHands.OneHand:
                    return SpriteName.LFIconOneHandR;
                case UseHands.TwoHands:
                    return SpriteName.LFIconTwoHands;
            }
        }

        const int GadgetTypeHashValueShift = 12;
        public static ushort GadgetTypeHash(GadgetType type)
        {//just nu 6typer //1,2,4,8
            return (ushort)((int)type << GadgetTypeHashValueShift);
        }
        public static GadgetType HashToGadgetType(ushort hash)
        {
            return (GadgetType)(hash >> GadgetTypeHashValueShift);
        }

        public static void WriteGadget(IGadget gadget, System.IO.BinaryWriter w)
        {
            //System.Diagnostics.Debug.WriteLine("Write gadget: " + gadget.ToString());
            w.Write((byte)gadget.GadgetType);
            if (gadget.GadgetType == GadgetType.Weapon)
            {
                GadgetSaveCategory cat = ((Gadgets.WeaponGadget.AbsWeaponGadget2)gadget).SaveCategory;
                if (cat == GadgetSaveCategory.ERR)
                    throw new Exception("Wep gadget has no save category: " + gadget.ToString());
                w.Write((byte)cat);
            }
            gadget.WriteStream(w);
        }
        public static IGadget ReadGadget(System.IO.BinaryReader r)
        {
            IGadget result;
            GadgetType type = (GadgetType)r.ReadByte();

            switch (type)
            {
                case GadgetType.Goods:
                    result = Gadgets.Goods.FromStream(r, SaveVersion);
                    break;
                case GadgetType.Item:
                    Gadgets.Item item = new Item(); item.ReadStream(r, SaveVersion, GadgetSaveCategory.NUM_NONE);
                    result = item;
                    break;
                case GadgetType.Weapon:
                    result = ReadWeaponGadget(r);
                    break;
                case GadgetType.Armor:
                    result = (new Armor(r, SaveVersion));
                    break;
                case GadgetType.Jevelery:
                    result = (new Jevelery(r, SaveVersion));
                    break;
                case GadgetType.Shield:
                    result = (new Shield(r, SaveVersion));
                    break;
                default:
                    throw new Exception("Can't read gadget");
            }
            //System.Diagnostics.Debug.WriteLine("Read gadget: " + result.ToString());
            return result;  
        }


        public static void WriteWeaponGadget(Gadgets.WeaponGadget.AbsWeaponGadget2 wep, System.IO.BinaryWriter w)
        {
            //System.Diagnostics.Debug.WriteLine("writing wep: " + wep.ToString());
            w.Write((byte)wep.SaveCategory);
            wep.WriteStream(w);
        }

        public static Gadgets.WeaponGadget.AbsWeaponGadget2 ReadWeaponGadget(System.IO.BinaryReader r)
        {
            Gadgets.WeaponGadget.AbsWeaponGadget2 result;
            WeaponGadget.GadgetSaveCategory category = (WeaponGadget.GadgetSaveCategory)r.ReadByte();
            switch (category)
            {
                default:
                    throw new Debug.MissingReadException("wep " + category.ToString());
                
                case WeaponGadget.GadgetSaveCategory.CraftedBow:
                    result = (new WeaponGadget.Bow(r, SaveVersion, category));
                    break;
                
                case WeaponGadget.GadgetSaveCategory.CraftedHandWeapon:
                    result = (new WeaponGadget.HandWeapon(r, SaveVersion, category));
                    break;
                case WeaponGadget.GadgetSaveCategory.Staff:
                    result = (new WeaponGadget.Staff(r, SaveVersion));
                    break;
                case WeaponGadget.GadgetSaveCategory.Spear:
                    result = new WeaponGadget.HandWeapon(StandardHandWeaponType.Spear);
                    break;
                    //goto case WeaponGadget.GadgetSaveCategory.CraftedHandWeapon;
                case WeaponGadget.GadgetSaveCategory.StandardHandWeapon:
                    //result = (new WeaponGadget.HandWeapon(r, SaveVersion));//StandardHandWeapon
                    //break;
                    goto case WeaponGadget.GadgetSaveCategory.CraftedHandWeapon;
                case WeaponGadget.GadgetSaveCategory.StandardBow:
                    //result = (new WeaponGadget.Bow(r, SaveVersion)); //StandardBow
                    //break;
                    goto case WeaponGadget.GadgetSaveCategory.CraftedBow;
            }
            //System.Diagnostics.Debug.WriteLine("reading wep: " + result.ToString());
            result.DoneLoadingCheckCorruption();

            return result;
        }

        /// <summary>
        /// Breaks down the material an item is made from, the result is based on luck
        /// </summary>
        /// <param name="baseAmount">The amount of goods it was made from</param>
        /// <param name="materialQualityLevel">The mix of quality</param>
        /// <param name="MediumQualityLevel">Expected level from medium quality materials</param>
        /// <param name="HighQualityLevel">Expected level from high quality materials</param>
        /// <returns>List of random generated goods, equal or less than the original was made of</returns>
        public static GadgetList ScrapMaterial(GoodsType type, int baseAmount, int materialQualityLevel, int MediumQualityLevel, int HighQualityLevel)
        {
            int amountLow = 0;
            int amountMedium = 0;
            int amountHight = 0;

            //randomly redude the amount of goods
            IntervalF reduceToAmount = new IntervalF(0.6f, 1);
            baseAmount = Bound.Min(
                (int)(reduceToAmount.GetRandom() * baseAmount), 1);

            //ramdomize the quality
            materialQualityLevel = (int)(materialQualityLevel * 1.1f);

            for (int i = 0; i < baseAmount; ++i)
            {
                int rndQual = Ref.rnd.Int(materialQualityLevel);
                if (rndQual >= HighQualityLevel)
                    ++amountHight;
                else if (rndQual >= MediumQualityLevel)
                    ++amountMedium;
                else
                    ++amountLow;
            }

            //sum up the result
            GadgetList result = new GadgetList();
            if (amountLow > 0)
            {
                result.Gadgets.Add(new Goods(type, Quality.Low, amountLow));
            }
            if (amountMedium > 0)
            {
                result.Gadgets.Add(new Goods(type, Quality.Medium, amountMedium));
            }
            if (amountHight > 0)
            {
                result.Gadgets.Add(new Goods(type, Quality.High, amountHight));
            }
            return result;
        }
    }

    struct GadgetAlternativeUse
    {
        public GadgetAlternativeUseType Type;
        public string Name;

        public GadgetAlternativeUse(GadgetAlternativeUseType Type, string Name)
        {
            this.Type = Type;
            this.Name = Name;
        }
    }

    enum GoodsType
    {
        NONE,

        GOODS_START,
        Apple,
        Grilled_apple,
        Seed,
        Bread,
        Apple_pie,
        Grapes,
        Wine,
        Meat,
        Grilled_meat,
        //
        Glass,
        Broken_glass, //lägga ut som mina?
        //
        Diamond, Ruby, Crystal, sapphire,
        //
        Granite, Marble, Sandstone, Flint,
        //
        Skin,
        Leather,
        Fur,
        Scaley_skin,
        Feathers,

        //
        Nose_horn,
        Horn,
        Tusks,
        Sharp_teeth,
        //
        Bronze, Iron, Silver, Gold, Mithril,
        //
        Stick,
        Wood,
        //herbs
        Blood_finger_herb,
        Frog_heart_herb,
        Fire_star_herb,
        Blue_rose_herb,//39

        //update1
        Honny,
        Wax,
        Poision_sting, //giftig tagg
        Red_eye,
        Black_tooth,
        Bladder_stone,
        Rib,
        Animal_paw,
        Plastma,
        Lucky_paw,//ge bonus i gambling?
        Monster_egg,
        Ink,
        Paper,
        Thread,
        Coal, //bonus om man eldar upp nån
        Candle,
        Holy_water,
        Orc_mead,
        Beer,
        END_GOODS,

        ITEM_START = 100,
        Coins,
        Arrow,
        GoldenArrow,
        SlingStone,

        Javelin,
        Fire_bomb,
        Lightning_bomb,
        Poision_bomb,
        Evil_bomb,
        Fluffy_bomb,
        Holy_bomb,

        //update2
        Empty_bottle,
        Water_bottle,
        Cookie,
        Golden_cookie,
        Repair_kit,

        Elvish_coins,
        Orc_coins,
        Dwarf_coins,
        Barbarian_coins,
        South_kingdom_coins,
        Ancient_coins,

        Text_scroll,
        END_ITEMS,

        //NoMaterial,
    }
    enum GadgetAlternativeUseType
    {
        Standard,
        //Arrow,
        GoldenArrow,
        Rush,
        Staff_fire,
        Staff_blast,
    }

    enum UseHands
    {
        QuickDraw,
        OneHand,
        TwoHands,
    }

    
}
