using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Gadgets
{
    class Armor : IGadget
    {
        bool helmet;
        public bool Helmet
        { get { return helmet; } }
        public Percent Protection;
        GoodsType material;

        public Armor(GoodsType material, bool helmet)
        {
            this.helmet = helmet;
            this.material = material;

            updateStats(LootfestLib.StandardItemQualityPercent);
        }

        public Armor(Data.Gadgets.CraftingTemplate template)
        {
            this.material = template.useItems[0].Type;
            this.helmet = template.Type == Data.Gadgets.BluePrint.LeatherHelmet || template.Type == Data.Gadgets.BluePrint.MetalHelmet;

            updateStats(template.QualitySumPercentage().Value);
                //template.QualitySumStrength() * Protection.TextValue);
        }

        public Armor(System.IO.BinaryReader r, byte version)
        {
            this.ReadStream(r, version, WeaponGadget.GadgetSaveCategory.NUM_NONE);
        }

        public void WriteStream(System.IO.BinaryWriter w)
        {
            w.Write((byte)material);
            w.Write((byte)Protection.TextValue);
            w.Write(helmet);
        }

        public void ReadStream(System.IO.BinaryReader r, byte version, GameObjects.Gadgets.WeaponGadget.GadgetSaveCategory saveCategory)
        {
            material = (GoodsType)r.ReadByte();
            Protection.TextValue = r.ReadByte();
            helmet = r.ReadBoolean();
        }

        void updateStats(float qualityPercent)
        {
            float QualityPercentToItemStrength = LootfestLib.QualityPercentToItemStrength.PercentPosition(qualityPercent);
            float materialProtect = LootfestLib.MaterialProtectionValue(material);

            Protection.Value = 
                 QualityPercentToItemStrength * materialProtect * 
                 (helmet ? LootfestLib.MaxHelmetArmor : LootfestLib.MaxBodyArmor);
        }

        

        public GameObjects.WeaponAttack.DamageData DamageReduce(GameObjects.WeaponAttack.DamageData damage)
        {
            damage.Damage *= (1 - Protection.Value);
            return damage;
        }

        public GadgetType GadgetType { get { return Gadgets.GadgetType.Armor; } }

        public GameObjects.WeaponAttack.DamageData TakeDamage(GameObjects.WeaponAttack.DamageData damage, bool local)
        {
            damage.Damage *= Protection.Value;
            return damage;
        }

        public override string ToString()
        {
            return material.ToString() + (helmet? " Helmet:" : " Armor:") + Protection.ToString();
        }
        public void EquipEvent()
        { }

        public string GadgetInfo
        {
            get
            {
                return "The armor will remove " + Protection.ToString() + " of received damage";
            }
        }

        public bool EquipAble { get { return true; } }

        public SpriteName Icon
        {
            get
            {
                if (helmet)
                {
                    switch (material)
                    {
                        case GoodsType.Skin:
                            return SpriteName.HelmetLeather;
                        case GoodsType.Leather:
                            return SpriteName.HelmetLeather;
                        case GoodsType.Scaley_skin:
                            return SpriteName.HelmetLeather;
                        case GoodsType.Bronze:
                            return SpriteName.HelmetBronze;
                        case GoodsType.Silver:
                            return SpriteName.HelmetIron;
                        case GoodsType.Gold:
                            return SpriteName.HelmetGold;
                        case GoodsType.Iron:
                            return SpriteName.HelmetIron;
                        case GoodsType.Mithril:
                            return SpriteName.HelmetMithril;
                    }
                }
                else
                {
                    switch (material)
                    {
                        case GoodsType.Skin:
                            return SpriteName.ArmourSkin;
                        case GoodsType.Leather:
                            return SpriteName.ArmourLeather;
                        case GoodsType.Scaley_skin:
                            return SpriteName.ArmourScaled;
                        case GoodsType.Bronze:
                            return SpriteName.ArmourBronze;
                        case GoodsType.Silver:
                            return SpriteName.ArmourSilver;
                        case GoodsType.Gold:
                            return SpriteName.ArmourGold;
                        case GoodsType.Iron:
                            return SpriteName.ArmourIron;
                        case GoodsType.Mithril:
                            return SpriteName.ArmourMithril;

                    }
                }
                return SpriteName.NO_IMAGE;
            }
        }

        public ushort ItemHashTag
        {
            get
            {
                ushort result = GadgetLib.GadgetTypeHash(this.GadgetType);
                if (helmet)
                    result += 4096;
                result += (ushort)((int)material * 64 + Protection.TextValue);
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

        public bool Scrappable
        {
            get
            {
                return
                    material == GoodsType.Bronze ||
                    material == GoodsType.Iron ||
                    material == GoodsType.Silver ||
                    material == GoodsType.Gold ||
                    material == GoodsType.Mithril;            
            } 
        }
        public GadgetList ScrapResult()
        {
            float power = Protection.Value / (helmet ? LootfestLib.MaxHelmetArmor : LootfestLib.MaxBodyArmor);
            power /= LootfestLib.MaterialProtectionValue(material);

            GadgetList result = GadgetLib.ScrapMaterial(material, helmet ? LootfestLib.CraftingHelmetArmorAmount : LootfestLib.CraftingBodyArmorAmount, 
                (int)(power * Percent.MaxTextPercentage), 50, Percent.MaxTextPercentage);

            return result;
        }

        public int Weight { get { return LootfestLib.SheildWeight; } }
        public bool Empty { get { return false; } }
    }
}
