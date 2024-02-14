using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//
using VikingEngine.LF2.Editor;

namespace VikingEngine.LF2.GameObjects.Gadgets.WeaponGadget
{
    abstract class AbsWeaponGadget2 : IGadget
    {
        protected Data.Gadgets.BluePrint weaponType;
        public Data.Gadgets.BluePrint Type
        {
            get { return weaponType; }
        }

        public bool IsBow
        {
            get
            {
                return weaponType == Data.Gadgets.BluePrint.EnchantedLongbow || weaponType == Data.Gadgets.BluePrint.EnchantedMetalbow ||
                    weaponType == Data.Gadgets.BluePrint.LongBow || weaponType == Data.Gadgets.BluePrint.MetalBow ||
                    weaponType == Data.Gadgets.BluePrint.ShortBow;
            }
        }

        protected float scale;
        protected WeaponAttack.DamageData damage;
        public bool Blessed = false;
        protected WeaponAttack.DamageData modifiedDamage;
        public void SetOwnerIx(ByteVector2 user)
        {
            damage.UserIndex = user;
        }

        protected float attacktime;
        protected float reloadtime;
        
        virtual public float HideShieldTime
        {
            get { return attacktime; }
        }

        public void DoneLoadingCheckCorruption()
        {
            if (attacktime == 0 || reloadtime == 0 || damage.Damage <= 0)
            {
                throw new Debug.LoadingItemException("Weapon is corrupt, you must start a new world: " + this.ToString());
            }
        }
        
        
        public float Reloadtime(Characters.Hero hero)
        {
#if !CMODE
            if (hero.Player.Progress.GotSkill(Magic.MagicRingSkill.Light_hand))
            {
                return reloadtime * 0.8f; 
            }
#endif
            return reloadtime;
        }


        public AbsWeaponGadget2(Data.Gadgets.BluePrint weaponType, GoodsType materialType)
        {
            this.weaponType = weaponType; 
            this.damage.Material = materialType;
        }

        public AbsWeaponGadget2(Data.Gadgets.CraftingTemplate template)
        {
            //this.template = template;
            this.damage.Material = template.useItems[0].Type;
            weaponType = template.Type;
        }
        public AbsWeaponGadget2()
        {

        }
        public override string ToString()
        {
            string result = weaponType.ToString();
            if (this.damage.Magic != Magic.MagicElement.NoMagic)
                result += ",  " + damage.Magic.ToString() + " magic";
            return result;//template.Type.ToString();
        }
        virtual public void WriteStream(System.IO.BinaryWriter w)
        { }
        virtual public void ReadStream(System.IO.BinaryReader r, byte version, GadgetSaveCategory saveCategory)
        { }
        virtual public void SendDummie(System.IO.BinaryWriter w)
        { }
        abstract public bool UsesAmmo { get; }
        abstract public bool UsesTargeting { get; }


        virtual public VoxelObjName VisualBowImage
        {
            get { throw new NotImplementedException(); }
        }

        abstract public GadgetSaveCategory SaveCategory
        {
            get;
        }

        protected bool equipped = false;
        public virtual void EquipEvent()
        {
            if (!equipped) CreateMesh();
            equipped = true;
        }
        virtual protected void CreateMesh() { }

        const float FullPower = 100;
        protected const int MediumPower = 30;
        protected const int HighPower = 60;
        protected byte power;
        protected void powerToDamage()
        {
            if (weaponType == Data.Gadgets.BluePrint.PickAxe)
            {
                float quality = power / FullPower;
                float material = LootfestLib.MetalDamageValueRange.GetValuePercentPos(LootfestLib.MetalDamageValue(damage.Material));

                const float QualityEffect = 0.4f;
                const float MaterialEffect = 1 - QualityEffect;
                damage.Damage = LootfestLib.PickAxeAndSickleDamageRange.PercentPosition(QualityEffect * quality + MaterialEffect * material);
            }
            else
            {
                if (damage.Damage == 0)
                {
                    setStandardDamage();
                }
                //damage.Damage *= power / FullPower;
                float fulldamage = damage.Damage / MediumPower * FullPower;

                damage.Damage = (int)(fulldamage * power / FullPower) + 1;
            }
        }
        protected void templateToPower(Data.Gadgets.CraftingTemplate template)
        {
            power = (byte)(template.QualitySumPercentage().Value * FullPower);
        }

        protected void setStandardDamage()
        {
            bool calcMetalValue = true;
            GoodsType m = GoodsType.NONE;
            if (damage.Damage != 0)
            {
                throw new Exception();
            }
            if (weaponType == Data.Gadgets.BluePrint.WoodSword || weaponType == Data.Gadgets.BluePrint.EnchantedWoodSword)
            {
                damage.Damage = LootfestLib.WoodenSwordDamage;
                m = GoodsType.Iron;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Sword || weaponType == Data.Gadgets.BluePrint.EnchantedSword)
            {
                damage.Damage = LootfestLib.SwordDamage;
                m = GoodsType.Iron;
            }
            else if (weaponType == Data.Gadgets.BluePrint.LongSword || weaponType == Data.Gadgets.BluePrint.EnchantedLongSword)
            {
                damage.Damage = LootfestLib.LongSwordDamage;
                m = GoodsType.Iron;
            }
            else if (weaponType == Data.Gadgets.BluePrint.LongAxe)
            {
                damage.Damage = LootfestLib.LongAxeDamage;
                m = GoodsType.Iron;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Axe || weaponType == Data.Gadgets.BluePrint.EnchantedAxe)
            {
                damage.Damage = LootfestLib.AxeDamage;
                m = GoodsType.Iron;
            }
            else if (weaponType == Data.Gadgets.BluePrint.PickAxe)
            {
                damage.Damage = LootfestLib.PickAxeAndSickleDamageRange.Center;
                damage.Special = WeaponAttack.SpecialDamage.PickAxe;
                m = GoodsType.Iron;
            }
            else if (weaponType == Data.Gadgets.BluePrint.BuildHammer)
            {
                damage.Damage = LootfestLib.WoodenStickDamage;
                m = GoodsType.Iron;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Sling)
            {
                damage.Damage = LootfestLib.SlingStoneDamage;
                m = GoodsType.Granite;
            }
            else if (weaponType == Data.Gadgets.BluePrint.ShortBow)
            {
                damage.Damage = LootfestLib.ShortBowDamage;
                m = GoodsType.Wood;
            }
            else if (weaponType == Data.Gadgets.BluePrint.LongBow || weaponType == Data.Gadgets.BluePrint.EnchantedLongbow)
            {
                damage.Damage = LootfestLib.LongBowDamage;
                m = GoodsType.Wood;
            }
            else if (weaponType == Data.Gadgets.BluePrint.MetalBow || weaponType == Data.Gadgets.BluePrint.EnchantedMetalbow)
            {
                damage.Damage = LootfestLib.MetalBowDamage;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Spear)
            {
                damage.Damage = LootfestLib.SpearDamage;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Dagger)
            {
                damage.Damage = LootfestLib.KnifeDamage;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Sickle)
            {
                damage.Damage = LootfestLib.PickAxeAndSickleDamageRange.Center;
                damage.Special = WeaponAttack.SpecialDamage.Sickle;
            }
            else if (weaponType == Data.Gadgets.BluePrint.EvilStaff || weaponType == Data.Gadgets.BluePrint.FireStaff ||
                weaponType == Data.Gadgets.BluePrint.LightningStaff || weaponType == Data.Gadgets.BluePrint.PoisionStaff)
            {
                damage.Damage = LootfestLib.StaffBasicDamage;
                calcMetalValue = false;
            }
            else
            {
                throw new Exception(weaponType.ToString() + " no standard damage");
            }

            if (damage.Material == GoodsType.NONE)//avoid overwrite
                damage.Material = m;

            if (calcMetalValue)
                damage.Damage *= LootfestLib.MetalDamageValue(damage.Material);
        }

        virtual public Gadgets.GoodsType AmmoType { get { return GoodsType.NONE; } }
        /// <returns>attack time, reload time</returns>
        /// <param name="target">target for projectile weapons</param>
        abstract public Vector2 Use(Characters.Hero parent, Vector3 target, GadgetAlternativeUseType altUse, bool localUse);
        virtual public void SetupDamage(Characters.Hero parent, List<Characters.Condition.AbsHeroCondition> modifiers)
        {
            modifiedDamage = damage;
            modifiedDamage.User = WeaponAttack.WeaponUserType.Player;
            modifiedDamage.UserIndex = parent.ObjOwnerAndId;
            modifiedDamage.AddHeroSpecials(parent, !UsesAmmo);
            foreach (Characters.Condition.AbsHeroCondition c in modifiers)
            {
                modifiedDamage = c.SpecialsToAttack(modifiedDamage);
            }

        }
        public GadgetType GadgetType { get { return Gadgets.GadgetType.Weapon; } }
        public bool EquipAble { get { return true; } }
        
        public Magic.MagicElement Enchantment { get { return damage.Magic; } }
        abstract public SpriteName Icon { get; }
        abstract public bool Scrappable { get; }
        abstract public GadgetList ScrapResult();

        virtual public UseHands Hands { get { return UseHands.OneHand; } }
        virtual public bool HaltUser { get { return false; } }
        abstract public string GadgetInfo { get; }

        public ushort ItemHashTag
        {
            get
            {
                ushort result = GadgetLib.GadgetTypeHash(this.GadgetType);
                result += (ushort)((int)weaponType * 64 + WeaponHashTag);
                return result;
            }
        }
        abstract protected byte WeaponHashTag { get; }
        public int StackAmount { get { return 1; } set { //do nothing
        } }

        abstract public bool CanBeEnchanted();
        virtual public void Enchant(GameObjects.Gadgets.Goods magicGem)
        {
            damage.MagicLevel = magicGem.Quality;
            damage.Magic = Magic.MagicLib.GemToMagic[magicGem.Type];
        }

        protected void writeStandardWeapon(System.IO.BinaryWriter w, int type)
        {
            w.Write((byte)type);
            EightBit hasMagic_blessed = new EightBit(damage.Magic != Magic.MagicElement.NoMagic, Blessed);
            hasMagic_blessed.WriteStream(w);

            if (damage.Magic != Magic.MagicElement.NoMagic)
            {
                w.Write((byte)damage.MagicLevel);
            }
        }

        protected int readStandardWeapon(System.IO.BinaryReader r, byte version)
        {
            int standardHandWeaponType = r.ReadByte();
            if (version > GadgetLib.FistReleaseVersion)
            {
                EightBit hasMagic_blessed = EightBit.FromStream(r);
                if (hasMagic_blessed.Get(0))
                {
                    damage.MagicLevel = (Quality)r.ReadByte();
                }
                Blessed = hasMagic_blessed.Get(1);
            }
            return standardHandWeaponType;
        }

        virtual public List<GadgetAlternativeUse> AlternativeUses(LF2.Players.PlayerProgress progress)
        {
            return null;
        }

        public int Weight { get { return LootfestLib.WeaponWeight; } }
        virtual public bool Empty { get { return false; } }
    }

    

    enum GadgetSaveCategory
    {
        ERR,
        StandardHandWeapon,
        CraftedHandWeapon,
        CraftedBow,
        StandardBow,
        Staff,
        Spear,

        DefaultStick,
        //BuildHammer,
        NUM_NONE,
    }

}
