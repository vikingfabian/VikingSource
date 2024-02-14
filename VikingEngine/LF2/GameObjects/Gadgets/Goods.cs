using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Gadgets
{
   
    struct Goods : IGadget
    {
        public const int FirstEnumIndex = (int)GoodsType.GOODS_START + 1;
        public const GoodsType FirstType = (GoodsType)FirstEnumIndex;
        public const int NumTypes = (int)GoodsType.END_GOODS - FirstEnumIndex;
        //public static readonly Quality NonQualityGoods = Quality.High;
        public GoodsType Type;
        public Quality Quality;
        public int Amount;
        public int StackAmount { get { return Amount; } set { Amount = value; } }

        public static string Name(GoodsType type)
        {
            return TextLib.EnumName(type.ToString());
        }

        public SpriteName Icon
        {
            get { return GadgetLib.GadgetIcon(Type); }
        }

        public static bool QualityType(GoodsType type)
        {
            return true;
        }


        public GameObjects.Gadgets.FoodHealingPower FoodRestore()
        {
            GameObjects.Gadgets.FoodHealingPower restore = new FoodHealingPower();
            restore.Goods = this;
            foreach (FoodHealingPower fh in LootfestLib.EatingOrderAndHealthRestore)
            {
                if (fh.Goods.Type == Type)
                {
                    restore.BasicHealing = fh.BasicHealing * LootfestLib.FoodQualityHealAdd[(int)Quality];
                    break;
                }
            }
            return restore;
        }

        public string GadgetInfo
        {
            get
            {
                string result = "NO INFO";

                bool ingredient = Type == GoodsType.Apple || Type == GoodsType.Meat || Type == GoodsType.Seed;
                if (ingredient || Type == GoodsType.Apple_pie || Type == GoodsType.Bread || Type == GoodsType.Grapes || 
                    Type == GoodsType.Grilled_apple || Type == GoodsType.Grilled_meat || Type == GoodsType.Wine)
                {
                    float restore = FoodRestore().BasicHealing;

                    Percent hp = new Percent(restore);
                    result = "Food that can be eaten to restore " + hp.ToString() + " health";
                    if (ingredient)
                    {
                        result += ". Can also be used as an ingredient for cooking.";
                    }
                }
                else if (Type == GoodsType.Blood_finger_herb || Type == GoodsType.Blue_rose_herb || Type == GoodsType.Fire_star_herb || Type == GoodsType.Frog_heart_herb)
                {
                    result = "Herbes are used in magic recepies";
                }
                else if (Type == GoodsType.Leather || Type == GoodsType.Scaley_skin || Type == GoodsType.Skin)
                {
                    result = "Can be crafted to armor";
                }
                else if (Type == GoodsType.Horn || Type == GoodsType.Sharp_teeth || Type == GoodsType.Nose_horn || Type == GoodsType.Tusks)
                {
                    result = "Used in weapon crafting";
                }
                else if (Type == GoodsType.Flint || Type ==  GoodsType.Granite || Type ==  GoodsType.Marble || Type ==  GoodsType.Sandstone || Type ==  GoodsType.Stick || Type ==  GoodsType.Wood)
                {
                    result = "Used in item crafting";
                }
                else if (Type ==  GoodsType.Animal_paw || Type ==  GoodsType.Black_tooth || Type ==  GoodsType.Bladder_stone || Type ==  GoodsType.Monster_egg || 
                    Type == GoodsType.Plastma || Type ==  GoodsType.Poision_sting || Type ==  GoodsType.Red_eye || Type ==  GoodsType.Rib)
                {
                    result = "Used in magic crafting";
                }
                else if (Type == GoodsType.Bronze || Type == GoodsType.Gold || Type == GoodsType.Iron || Type == GoodsType.Silver || Type == GoodsType.Mithril)
                {
                    result = "Metals are a valuable resource and are used in both armor and weapons";
                }
                else if (Type == GoodsType.Crystal || Type == GoodsType.Diamond || Type == GoodsType.Marble || Type == GoodsType.Ruby)
                {
                    result = "Magic stones are rare and can be used to enchant items";
                }
               
                else if (Type== GoodsType.Wine || Type ==  GoodsType.Fur || Type ==  GoodsType.Glass || Type ==  GoodsType.Orc_mead || Type ==  GoodsType.Beer)
                {
                    result = "A popular trading resource";
                }
                return result;
            }
        }

        public Goods(GoodsType type)
            : this(type, Quality.Medium, 1)
        { }
        public Goods(GoodsType type, int amount)
            : this(type, Quality.Medium, amount)
        { }
        public Goods(GoodsType type, Quality quality)
            :this(type, quality, 1)
        { }
        public Goods(GoodsType type, Quality quality, int amount)
        {
            this.Type = type;
            this.Quality = quality;
            Amount = amount;
        }
        public Goods(int type, int quality, int amount)
            : this((GoodsType)type, (Quality)quality, amount)
        { }
        public bool HaveQuality
        {
            get { return true; }//GoodsHaveQuality(Type); }
        }
        public void EquipEvent()
        { }
        //public static bool GoodsHaveQuality(GoodsType type)
        //{
        //    return type >= GoodsType.Coin;
        //}
        public GadgetType GadgetType { get { return Gadgets.GadgetType.Goods; } }

        public override string ToString()
        {
            string result = Amount.ToString() + " " + TextLib.EnumName(Type.ToString());
            if (QualityType(Type))
                result+= ", " + this.Quality.ToString() + " quality";

            return result; 
        }

        public bool EquipAble { get { return false; } }


        public static Goods FromStream(System.IO.BinaryReader r, byte version)
        {
            Goods g = new Goods();
            g.ReadStream(r, version, WeaponGadget.GadgetSaveCategory.NUM_NONE);
            return g;
        }
        public void WriteStream(System.IO.BinaryWriter w)
        {
            w.Write((byte)Type);
            w.Write((byte)Quality);
            DataStream.DataStreamLib.WriteGrowingAddValue(w, Amount);
        }


        public void ReadStream(System.IO.BinaryReader r, byte version, GameObjects.Gadgets.WeaponGadget.GadgetSaveCategory saveCategory)
        {
            byte type = r.ReadByte();
            Type = (GoodsType)type;
            if (type == byte.MaxValue)
                return;
            
            if (version == GadgetLib.FistReleaseVersion)
            {
                Type += FirstEnumIndex;
            }
            Quality = (Gadgets.Quality)r.ReadByte();
            Amount = DataStream.DataStreamLib.ReadGrowingAddValue(r);//DataLib.SaveLoad.ReadGrowingValue(r);
        }

        public bool Eatable
        {
            get
            {
                return Type <= GoodsType.Grilled_meat;
            }
        }
        public ushort ItemHashTag
        {
            get
            {
                ushort result = GadgetLib.GadgetTypeHash(this.GadgetType);
                result += (ushort)((int)Quality * 128 + (int)Type);
                return result;
            }
        }
        public bool Scrappable { get { return false; } }
        public GadgetList ScrapResult() { throw new NotImplementedException(); }

        public int Weight { get { return LootfestLib.GoodsWeight * Amount; } }
        public bool Empty { get { return Type == GoodsType.NONE; } }
    }
    struct QualityAccess
    {
        public bool GotQuality;
        public int[] Use;
        public override string ToString()
        {
            string result = TextLib.EmptyString;
            for (int i = 0; i < (int)Quality.NUM; i++)
            {
                if (Use[i] > 0)
                {
                    result += Use[i].ToString() + ((Quality)i).ToString() + " ";
                }
            }
            return result;
        }

        //public int UseHigh;
        //public int UseMedium;
        //public int UseLow;

    }
    
    
    enum Quality
    {
        Low,
        Medium,
        High,
        NUM
    }
}
