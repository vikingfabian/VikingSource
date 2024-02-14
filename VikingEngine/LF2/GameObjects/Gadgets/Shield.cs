using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Gadgets
{
    class Shield : IGadget
    {
        public ShieldType Type;
        byte armorBonus = 0;

        public Shield(ShieldType type, Data.Gadgets.CraftingTemplate template)
            :this(type)
        {
            //min 0.6
            float strength = template.QualitySumStrength();
            if (strength > 0.6f)
            {
                float armorBonusLvl;
                switch (Type)
                {
                    default:
                        armorBonusLvl = 3;
                        break;
                    case ShieldType.Round:
                        armorBonusLvl = 5;
                        break;
                    case ShieldType.Square:
                        armorBonusLvl = 8;
                        break;
                }
                if (template.useItems[1].Type == GoodsType.Mithril) //metal buckle
                {
                    armorBonusLvl *= 2;
                }

                armorBonusLvl *= strength;
                armorBonus = (byte)armorBonusLvl;
            }
        }
        public Shield(ShieldType type)
        {
            this.Type = type;
        }
        public Shield(System.IO.BinaryReader r, byte version)
        {
            ReadStream(r, version, WeaponGadget.GadgetSaveCategory.NUM_NONE);
        }

        public void WriteStream(System.IO.BinaryWriter w)
        { 
            w.Write((byte)Type);
            w.Write(armorBonus);
        }

        public void ReadStream(System.IO.BinaryReader r, byte version, GameObjects.Gadgets.WeaponGadget.GadgetSaveCategory saveCategory)
        { 
            Type = (ShieldType)r.ReadByte(); 
            armorBonus = r.ReadByte(); 
        }

        public GadgetType GadgetType { get { return Gadgets.GadgetType.Shield; } }

        public SpriteName Icon
        {
            get
            {
                switch (Type)
                {
                    case ShieldType.Buckle:
                        return SpriteName.ShieldBuckle;
                    case ShieldType.Round:
                        return SpriteName.ShieldRound;
                    case ShieldType.Square:
                        return SpriteName.ShieldSquare;
                }
                return SpriteName.NO_IMAGE;
            }
        }

        public override string ToString()
        {
            return Type.ToString() + " shield";
        }
        public void EquipEvent()
        { }
        public string GadgetInfo
        {
            get
            {
                return "A useful extra protection in battle. Can only be used together with one handed weapons. " + armorBonus.ToString() + "% armor bonus.";
            }
        }
        public bool EquipAble { get { return true; } }

        public ushort ItemHashTag
        {
            get
            {
                ushort result = GadgetLib.GadgetTypeHash(this.GadgetType);
                result += (ushort)((int)Type * 128 + armorBonus);
                return result;
            }
        }
        public int StackAmount
        {
            get { return 1; }
            set
            { //do nothing
            }
        }

        public bool Scrappable { get { return false; } }
        public GadgetList ScrapResult() 
        { 
            throw new NotImplementedException(); 
        }
        public int Weight { get { return LootfestLib.SheildWeight; } }
        public bool Empty { get { return false; } }
    }

    enum ShieldType
    {
        Buckle,
        Round,
        Square,
    }
}
