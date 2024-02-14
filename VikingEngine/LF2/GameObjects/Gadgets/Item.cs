using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Gadgets
{
    struct Item : IGadget
    {
        public const int FirstEnumIndex = (int)GoodsType.ITEM_START + 1;
        public const GoodsType FirstType = (GoodsType)FirstEnumIndex;
        public const int NumTypes = (int)GoodsType.END_ITEMS - FirstEnumIndex;

        public static readonly Item NoItem = new Item(GoodsType.NONE, 0);
        public GoodsType Type;
        public int Amount;
        public int StackAmount { get { return Amount; } set { Amount = value; } }

        public Item(GoodsType type)
            :this(type, 1)
        {
        }
        public Item(GoodsType type, int amount)
        {
            this.Type = type;
            this.Amount = (ushort)amount;
        }

        public GadgetType GadgetType { get { return Gadgets.GadgetType.Item; } }

        public string GadgetInfo
        {
            get
            {
                if (Type == GoodsType.Javelin || 
                    Type == GoodsType.Evil_bomb || 
                    Type == GoodsType.Fire_bomb || 
                    Type == GoodsType.Lightning_bomb || 
                    Type == GoodsType.Poision_bomb ||
                    Type == GoodsType.Fluffy_bomb)
                {
                    return "Throwing weapon";
                }
                else if (Type == GoodsType.Coins)
                {
                    return "Basic money";
                }
                else if (Type == GoodsType.Holy_bomb)
                {
                    return "Very powerful bomb";
                }
                else if (Type == GoodsType.Arrow)
                {
                    return "Ammo for bows";
                }
                else if (Type == GoodsType.GoldenArrow)
                {
                    return "Extra powerful arrows";
                }
                else if (Type == GoodsType.SlingStone)
                {
                    return "Ammo for slingshots";
                }
                else if (Type == GoodsType.Text_scroll)
                {
                    return "This scroll may contain important information, find someone that can interpret the text";
                }
                else if (Type == GoodsType.Empty_bottle)
                {
                    return "Empty water container";
                }
                else if (Type == GoodsType.Water_bottle)
                {
                    return "Water container, will recover you from the magic use dehydration";
                }
                else if (Type == GoodsType.Cookie || Type == GoodsType.Golden_cookie)
                {
                    return "Get a short lived boost from the sugar rush";
                }
                else if (Type == GoodsType.Repair_kit)
                {
                    return "Recover worn out equipment";
                }
                if (Type == GoodsType.Elvish_coins || 
                    Type == GoodsType.Orc_coins || 
                    Type == GoodsType.Dwarf_coins || 
                    Type == GoodsType.Barbarian_coins || 
                    Type == GoodsType.South_kingdom_coins ||
                    Type == GoodsType.Ancient_coins)
                {
                    return "Coins used in other regions of the world";
                }

                return "ERR";
            }
        }

        public static bool ButtonEquipable(GoodsType type)
        {
            return type >= GoodsType.Javelin;
        }

        public SpriteName Icon 
        {
            get { return GadgetLib.GadgetIcon(Type); }
        }

        public void WriteStream(System.IO.BinaryWriter w)
        {
            w.Write((byte)Type);
            DataStream.DataStreamLib.WriteGrowingAddValue(w, Amount);
        }
        public void ReadStream(System.IO.BinaryReader r, byte version, GameObjects.Gadgets.WeaponGadget.GadgetSaveCategory saveCategory)
        {
            
            Type = (GoodsType)r.ReadByte();
            if (version == GadgetLib.FistReleaseVersion)
            {
                Type += FirstEnumIndex;
            }
            Amount = (ushort)DataStream.DataStreamLib.ReadGrowingAddValue(r);

        }
        public void EquipEvent() { }

        public override string ToString()
        {
            return Amount.ToString() + " " + TextLib.EnumName(Type.ToString());
        }

        public bool EquipAble { get { return 
            Type == GoodsType.Javelin||
            Type ==GoodsType.Fire_bomb||
            Type ==GoodsType.Evil_bomb||
            Type == GoodsType.Poision_bomb||
            Type == GoodsType.Lightning_bomb||
            Type == GoodsType.Fluffy_bomb||
            Type == GoodsType.Holy_bomb||
            Type == GoodsType.Water_bottle||
            Type == GoodsType.Cookie||
            Type ==  GoodsType.Golden_cookie; 
        } }

        public ushort ItemHashTag
        {
            get
            {
                ushort result = GadgetLib.GadgetTypeHash(this.GadgetType);

                result += (ushort)Type;
                return result;
            }
        }
        public bool Scrappable { get { return false; } }
        public GadgetList ScrapResult() { throw new NotImplementedException(); }
        public int Weight { get {
            if (Type == GoodsType.Coins) return 0;
            return LootfestLib.GoodsWeight * Amount; } }
        public bool Empty { get { return Type == GoodsType.NONE; } }
    }

}
