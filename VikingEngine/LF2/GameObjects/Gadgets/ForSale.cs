using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Gadgets
{
    //struct ForSaleLib
    //{
    //    public const int LinkBuy = 1001;
    //    public const int LinkDontBuy = 1002;
    //}
    //interface IForSale
    //{
    //    string Name { get; }
    //    string Describtion { get; }
    //    /// <summary>
    //    /// If there is anything to say about the product when it is bought
    //    /// </summary>
    //    string BoughtDialoge { get; }
    //    int Price { get; }
    //    SpriteName Icon { get; }
    //    bool CanBeBought(Characters.Hero hero);
    //    void Buy(Characters.Hero hero);
    //}

    //struct ShieldUpgrade : IForSale
    //{
    //    byte level;
    //    int price;
    //    Range priceRange;
    //    public ShieldUpgrade(byte level)
    //    {
    //        this.level = level;
    //        switch (level)
    //        {
    //            default:
    //                priceRange = new Range(30, 35);
    //                break;
    //            case 1:
    //                priceRange = new Range(80, 95);
    //                break;
    //            case 2:
    //                priceRange = new Range(200, 230);
    //                break;
    //        }
    //        price = Data.RandomSeed.Instance.Next(priceRange);
    //    }
    //    public string Name { get { return LanguageManager.Wrapper.ItemTypeShield() + LanguageManager.Wrapper.Level()+ (level + 1).ToString(); } }
    //    public string Describtion 
    //    {
    //        get {
    //            return LanguageManager.Wrapper.SaleShieldDesc(level);
    //            //switch (level)
    //            //{
    //            //    default:
    //            //        return "Will bounce off projectiles, that comes straight in front of you";
    //            //    case 1:
    //            //        return "Will bounce off projectiles, from the front and sides.";
    //            //    case 2:
    //            //        return "Will bounce off projectiles and turn them against your enemy";
    //            //}
    //            //return "Get some protection against projectiles"; 
    //        } 
    //    }
    //    /// <summary>
    //    /// If there is anything to say about the product when it is bought
    //    /// </summary>
    //    public string BoughtDialoge { get { return TextLib.EmptyString; } }
    //    public int Price { get { return price; } }

    //    public bool CanBeBought(Characters.Hero hero)
    //    {
    //        return true;
    //    }
    //    public void Buy(Characters.Hero hero)
    //    {
            
    //    }
    //    public SpriteName Icon
    //    { get { 
            
    //        return (SpriteName)SpriteName.LFShield1 +level; } }
    //}

    //struct SwordUpgrade : IForSale
    //{
    //    byte level;
    //    int price;
    //    Range priceRange;
    //    public SwordUpgrade(byte level)
    //    {
    //        this.level = level;
    //        switch (level)
    //        {
    //            default:
    //                priceRange=new Range(20, 25);
    //                break;
    //            case 2:
    //                priceRange=new Range(70, 90);
    //                break;
    //        }
    //        price = Data.RandomSeed.Instance.Next(priceRange);
    //    }
    //    public string Name { get { return LanguageManager.Wrapper.WeaponTypeSword() + LanguageManager.Wrapper.Level() + (level + 1).ToString(); } }
    //    public string Describtion { get { return LanguageManager.Wrapper.SaleSwordDesc(); } }
    //    /// <summary>
    //    /// If there is anything to say about the product when it is bought
    //    /// </summary>
    //    public string BoughtDialoge { get { return TextLib.EmptyString; } }
    //    public int Price { get { return price; } }

    //    public bool CanBeBought(Characters.Hero hero)
    //    {
    //        return level > hero.SwordLevel;
    //    }
    //    public void Buy(Characters.Hero hero)
    //    {
    //        hero.SwordLevel = level;
    //    }
    //    public SpriteName Icon 
    //    { get { return level == 1? SpriteName.LFSwordIcon2 : SpriteName.LFSwordIcon3; } }
    //}
    //struct BowUpgrade : IForSale
    //{
    //    byte level;
    //    int price;
    //    Range priceRange;
    //    public BowUpgrade(byte level)
    //    {
    //        this.level = level;
    //        switch (level)
    //        {
    //            default:
    //                priceRange = new Range(30, 35);
    //                break;
    //            case 1:
    //                priceRange = new Range(50, 60);
    //                break;
    //            case 2:
    //                priceRange = new Range(70, 90);
    //                break;

    //        }
    //        price = Data.RandomSeed.Instance.Next(priceRange);
    //    }
    //    public string Name { get { return LanguageManager.Wrapper.WeaponTypeBow() + LanguageManager.Wrapper.Level() + (level + 1).ToString(); } }
    //    public string Describtion { get { return
    //        LanguageManager.Wrapper.SaleBowDesc(level);
    //    }
    //    }
    //    /// <summary>
    //    /// If there is anything to say about the product when it is bought
    //    /// </summary>
    //    public string BoughtDialoge { get { return TextLib.EmptyString; } }
    //    public int Price { get { return price; } }

    //    public bool CanBeBought(Characters.Hero hero)
    //    {
    //        return true;
    //    }
    //    public void Buy(Characters.Hero hero)
    //    {
    //    }
    //    public SpriteName Icon
    //    { get {
    //        //switch (level)
    //        //{
    //        //    default:
    //                return SpriteName.LFBow1 + level;
    //        //    case
    //        //}
    //    } }
    //}
    //struct BasicProduct : IForSale
    //{
    //    //string name;
    //    //string desc;
    //    //string boughtDialoge;
    //    int price;
    //    BasicProductType type;

    //    public BasicProduct(BasicProductType type)
    //    {
    //        this.type = type;
    //        Range priceRange;
    //        switch (type)
    //        {
    //            default: //BOW
    //                priceRange = new Range(30, 50);
    //                break;
    //            case BasicProductType.Arrows10p:
    //                priceRange = new Range(6, 8);
    //                break;
    //            case BasicProductType.Hawk:
    //                priceRange = new Range(4, 5);
    //                break;
    //            case BasicProductType.Health:
    //                priceRange = new Range(6, 10);
    //                break;
    //            case BasicProductType.Pigeon:
    //                priceRange = new Range(2, 2);
    //                break;
    //        }
    //        price = Data.RandomSeed.Instance.Next(priceRange);
    //    }
    //    public string Name 
    //    { get 
    //        {
    //            switch (type)
    //            {
    //                default: //BOW
    //                    return LanguageManager.Wrapper.Bow();
    //                case BasicProductType.Arrows10p:
    //                    return "10 " + LanguageManager.Wrapper.Arrows();
    //                case BasicProductType.Hawk:
    //                    return LanguageManager.Wrapper.Hawk();
    //                case BasicProductType.Health:
    //                    return LanguageManager.Wrapper.ApplePie();
    //                case BasicProductType.Pigeon:
    //                    return LanguageManager.Wrapper.Pigeon();
    //            }
    //        } 
    //    }
    //    public string BoughtDialoge
    //    { 
    //        get 
    //        {
    //            //const string Bird1 = "Access your ";
    //            //const string Bird2 =  "in the pause menu by pressing Start";
    //            switch (type)
    //            {
    //                default: //BOW
    //                    return TextLib.EmptyString;
    //                case BasicProductType.Hawk:
    //                    return LanguageManager.Wrapper.SaleBirdAccess(LanguageManager.Wrapper.Hawk()); //Bird1 +"hawk" + Bird2;
    //                case BasicProductType.Pigeon:
    //                    return LanguageManager.Wrapper.SaleBirdAccess(LanguageManager.Wrapper.Pigeon()); //Bird1 +"hawk" + Bird2;
    //                case BasicProductType.Health:
    //                    return LanguageManager.Wrapper.SaleRBtoEat();

    //            }
    //        } 
    //    }
    //    public int Price { get { return price; } }
    //    public string Describtion 
    //    {
    //        get
    //        {
    //            switch (type)
    //            {
    //                //turn "You need this to defeat the final boss";
    //                default: //case BasicProductType.Arrows10p:
    //                    return LanguageManager.Wrapper.SaleArrowsDesc();
    //                case BasicProductType.Hawk:
    //                    return LanguageManager.Wrapper.SaleHawkDesc();
    //                case BasicProductType.Pigeon:
    //                    return LanguageManager.Wrapper.SalePigeonDesc();
    //                case BasicProductType.Health:
    //                    return LanguageManager.Wrapper.SalePieDesc();

    //            }
    //        }
    //    }
    //    public bool CanBeBought(Characters.Hero hero)
    //    {
    //        switch (type)
    //        {
    //            //default: //BOW
    //            //    return !hero.HaveBow;
    //            default:
    //                return true;
    //            //case BasicProductType.Health:
    //            //    return hero.NumPies < Characters.Hero.MaxNumPies;

    //        }
    //    }
    //    public SpriteName Icon 
    //    { 
    //        get
    //        {
    //            switch (type)
    //            {
    //                default: //BOW
    //                    return SpriteName.LFBow1;
    //                case BasicProductType.Arrows10p:
    //                    return SpriteName.LFArrow;
    //                case BasicProductType.Hawk:
    //                    return SpriteName.LFHawk;
    //                case BasicProductType.Health:
    //                    return SpriteName.LFApplePie;
    //                case BasicProductType.Pigeon:
    //                    return SpriteName.LFPigeon;
    //            }
    //        }
    //    }
    //    //public void Buy(Characters.Hero hero)
    //    //{
    //    //    hero.AddProduct(type);
    //    //}
    //}

    //enum BasicProductType : byte
    //{
    //    //Bow,
    //    Arrows10p,
    //    Health,
    //    Hawk,
    //    Pigeon,
    //}
}
