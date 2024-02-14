using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.LF2.Editor;

namespace VikingEngine.LF2.GameObjects.Gadgets.WeaponGadget
{
    class Bow : AbsWeaponGadget2
    {
        GoodsType ammo
        {
            get { return weaponType == Data.Gadgets.BluePrint.Sling ? Gadgets.GoodsType.SlingStone : GoodsType.Arrow; }
        }
        
        //StandardBow init
        public Bow(Data.Gadgets.BluePrint bowType)
            : base()
        {
            weaponSaveCategory = GadgetSaveCategory.StandardBow;
            damage = new WeaponAttack.DamageData(0, WeaponAttack.WeaponUserType.Player, ByteVector2.Zero, GoodsType.Wood);
            weaponType = bowType;
            checkStandardBowMaterial();
            setStandardDamage();
            bowAttackReloadTime();
        }

        

        //public Bow()
        //    : base()
        //{ }


        public Bow(Data.Gadgets.BluePrint bowType, GoodsType bowMaterial, Magic.MagicElement magicType)
        {
            weaponSaveCategory = GadgetSaveCategory.CraftedBow;
            weaponType = bowType;
            damage = new WeaponAttack.DamageData(0, WeaponAttack.WeaponUserType.Player, ByteVector2.Zero, bowMaterial, magicType);
            //bowTypeToAmmo(bowType);
            setStandardDamage();
            power = 100;
            powerToDamage();
            bowAttackReloadTime();
        }

        public Bow(Data.Gadgets.BluePrint bowType, Data.Gadgets.CraftingTemplate template)
            : base(template)
        {
            weaponSaveCategory = GadgetSaveCategory.CraftedBow;
            damage = new WeaponAttack.DamageData(0, WeaponAttack.WeaponUserType.Player, ByteVector2.Zero, GoodsType.Wood);
            //bowTypeToAmmo(bowType);
            //basicInit();
            setStandardDamage();

            if (bowType == Data.Gadgets.BluePrint.MetalBow || bowType == Data.Gadgets.BluePrint.EnchantedMetalbow)
            {
                damage.Material = template.useItems[1].Type;//the shoes
            }
            if (weaponType == Data.Gadgets.BluePrint.EnchantedLongbow || weaponType == Data.Gadgets.BluePrint.EnchantedMetalbow)
            {
                damage.Magic = Magic.MagicLib.GemToMagic[template.useItems[3].Type];
            }
            templateToPower(template);
            powerToDamage();
            bowAttackReloadTime();
        }


        //public StandardBow(System.IO.BinaryReader r, byte version)
        //    : base(r, version)
        //{
        //    damage = new WeaponAttack.DamageData(0, WeaponAttack.WeaponUserType.Player, ByteVector2.Zero, GoodsType.Wood);
        //    checkMaterial();
        //    setStandardDamage();
        //    bowAttackReloadTime();
        //}

        public Bow(System.IO.BinaryReader r, byte version, GadgetSaveCategory saveCategory)
            : base()
        {
            if (saveCategory != GadgetSaveCategory.StandardBow && saveCategory != GadgetSaveCategory.CraftedBow)
                throw new NotImplementedException("Bow cant load save cat: " + saveCategory.ToString());

            weaponSaveCategory = saveCategory;
            

            if (saveCategory == GadgetSaveCategory.StandardBow)
            {
                damage = new WeaponAttack.DamageData(0, WeaponAttack.WeaponUserType.Player, ByteVector2.Zero, GoodsType.Wood);
                checkStandardBowMaterial();
                setStandardDamage();
                bowAttackReloadTime();
            }
            ReadStream(r, version, saveCategory);
            bowAttackReloadTime();
        }

        void checkStandardBowMaterial()
        {
            if (weaponType == Data.Gadgets.BluePrint.MetalBow)
            {
                damage.Material = GoodsType.Bronze;
            }
        }

        protected void bowAttackReloadTime()
        {
            const float BisyFireingTime = 400;
            attacktime = 100;
            reloadtime = BisyFireingTime; 
        }


        //protected void bowTypeToAmmo(Data.Gadgets.BluePrint bowType)
        //{
        //    ammo = 
        //}

        //public override float HideShieldTime
        //{
        //    get
        //    {
        //        return BisyFireingTime;
        //    }
        //}


        static readonly List<Magic.MagicRingSkill> checkMagicBurstsSkills = new List<Magic.MagicRingSkill>
        {
            Magic.MagicRingSkill.Projectile_evil_burst, Magic.MagicRingSkill.Projectile_fire_burst, Magic.MagicRingSkill.Projectile_lightning_burst, Magic.MagicRingSkill.Projectile_poision_burst
        };
        public override Vector2 Use(Characters.Hero parent, Vector3 target, GadgetAlternativeUseType altUse,  bool localUse)
        {
            const float ArrowBoundSz = 0.5f;

            GoodsType ammoType = ammo;
            if (weaponType != Data.Gadgets.BluePrint.Sling && altUse ==  GadgetAlternativeUseType.GoldenArrow)
            {
                ammoType = GoodsType.GoldenArrow;
            }
            if (parent.Player == null || parent.Player.Progress.Items.RemoveItem(ammoType))
            {
                Vector3 firePos = parent.Position;
                firePos.Y += 1.2f;
                firePos += Map.WorldPosition.V2toV3(parent.FireDir.Direction(1.2f));
                Magic.MagicRingSkill burstMagic = Magic.MagicRingSkill.NO_SKILL;
                foreach (Magic.MagicRingSkill s in checkMagicBurstsSkills)
                {
                    if (parent.Player.Progress.GotSkill(s))
                    {
                        burstMagic = s;
                        break;
                    }
                }

                if (ammo == GoodsType.Arrow)
                {
                    bool reuse = Ref.rnd.RandomChance(25) && parent.Player.Progress.GotSkill(Magic.MagicRingSkill.Recylcling_bowman);

                    LF2.ObjSingleBound bound = WeaponAttack.GravityArrow.ArrowBound(Rotation1D.D0);

                    if (altUse ==  GadgetAlternativeUseType.GoldenArrow)
                    {
                        new WeaponAttack.GoldenArrow(modifiedDamage, firePos, target,
                           bound,
                           reuse, burstMagic, weaponType, parent.Player);
                    }
                    else
                    {
                        new WeaponAttack.GravityArrow(modifiedDamage, firePos, target,
                            bound, reuse, burstMagic, weaponType, parent.Player);
                    }

                }
                else
                {
                    new WeaponAttack.SlingStone(modifiedDamage, firePos, target,
                       LF2.ObjSingleBound.QuickBoundingBox(ArrowBoundSz));
                }
            }
            else
            {//out of ammo
                parent.Player.OutOfAmmo("ammo");
            }
            return new Vector2(attacktime, reloadtime);
        }


        public override void WriteStream(System.IO.BinaryWriter w)
        {
            if (weaponSaveCategory == GadgetSaveCategory.StandardBow)
                writeStandardBow(w);
            else
                writeCraftedBow(w);
        }
        public override void ReadStream(System.IO.BinaryReader r, byte version, GadgetSaveCategory saveCategory)
        {
            if (saveCategory == GadgetSaveCategory.StandardBow)
                readStandardBow(r, version);
            else
                readCraftedBow(r, version);
        }


        void writeCraftedBow(System.IO.BinaryWriter w)
        {
            w.Write((byte)weaponType);
            if (weaponType == Data.Gadgets.BluePrint.MetalBow || weaponType == Data.Gadgets.BluePrint.EnchantedMetalbow)
                w.Write((byte)damage.Material);
            if (weaponType == Data.Gadgets.BluePrint.EnchantedLongbow || weaponType == Data.Gadgets.BluePrint.EnchantedMetalbow)
                w.Write((byte)damage.Magic);
            w.Write(power);
        }
        void readCraftedBow(System.IO.BinaryReader r, byte version)
        {
            damage = new WeaponAttack.DamageData(0, WeaponAttack.WeaponUserType.Player, ByteVector2.Zero);
            weaponType = (Data.Gadgets.BluePrint)r.ReadByte();
            if (weaponType == Data.Gadgets.BluePrint.MetalBow || weaponType == Data.Gadgets.BluePrint.EnchantedMetalbow)
                damage.Material = (GoodsType)r.ReadByte();
            if (weaponType == Data.Gadgets.BluePrint.EnchantedLongbow || weaponType == Data.Gadgets.BluePrint.EnchantedMetalbow)
                damage.Magic = (Magic.MagicElement)r.ReadByte();
            power = r.ReadByte();
            if (power <= 0)
            {
                power = MediumPower;
            }
            powerToDamage();
        }

        void writeStandardBow(System.IO.BinaryWriter w)
        {
            writeStandardWeapon(w, (int)weaponType);
        }
        void readStandardBow(System.IO.BinaryReader r, byte version)
        {
            weaponType = (Data.Gadgets.BluePrint)readStandardWeapon(r, version);
        }


        override protected byte WeaponHashTag { get { return (byte)(((int)damage.Material + power + (int)damage.Magic) % PublicConstants.ByteSize); } }
        override public bool UsesAmmo { get { return true; } }
        override public VoxelObjName VisualBowImage
        {
            get 
            {
                if (damage.Magic == Magic.MagicElement.NoMagic)
                {
                    switch (weaponType)
                    {
                        default:
                            return VoxelObjName.shortbow;
                        case Data.Gadgets.BluePrint.LongBow:
                            return VoxelObjName.longbow;
                        case Data.Gadgets.BluePrint.MetalBow:
                            switch (damage.Material)
                            {
                                default:
                                    return VoxelObjName.ironbow;
                                case GoodsType.Bronze:
                                    return VoxelObjName.bronzebow;
                                case GoodsType.Silver:
                                    return VoxelObjName.silverbow;
                                case GoodsType.Gold:
                                    return VoxelObjName.goldbow;
                                case GoodsType.Mithril:
                                    return VoxelObjName.mithrilbow;

                            }
                        case Data.Gadgets.BluePrint.Sling:
                            return VoxelObjName.slingshot;
                    }

                }
                else
                {
                    switch (damage.Magic)
                    {
                        default:
                            return VoxelObjName.firebow;
                        case Magic.MagicElement.Evil:
                            return VoxelObjName.evilbow;
                        case Magic.MagicElement.Lightning:
                            return VoxelObjName.lightningbow;
                        case Magic.MagicElement.Poision:
                            return VoxelObjName.poisionbow;

                    }
                }
            
            
            }
        }
        override public Gadgets.GoodsType AmmoType { get { return ammo; } }

        GadgetSaveCategory weaponSaveCategory;
        override public GadgetSaveCategory SaveCategory
        {
            get { return weaponSaveCategory; }//WeaponSaveCategory.CraftedBow; }
        }
        public override UseHands Hands
        {
            get { return UseHands.TwoHands; }
        }

        public override void Enchant(Goods magicGem)
        {
            base.Enchant(magicGem);
            weaponSaveCategory = GadgetSaveCategory.CraftedBow;
            
        }


        public override SpriteName Icon
        {
            get 
            {
                switch (weaponType)
                {
                    case Data.Gadgets.BluePrint.Sling:
                        return SpriteName.WeaponSlingshot;
                    case Data.Gadgets.BluePrint.ShortBow:
                        return SpriteName.WeaponShortBow;
                    case Data.Gadgets.BluePrint.LongBow:
                        return SpriteName.WeaponLongBow;
                    case Data.Gadgets.BluePrint.MetalBow:
                        return SpriteName.WeaponSpecialBow;

                    case Data.Gadgets.BluePrint.EnchantedLongbow:
                        return SpriteName.WeaponLongBow;
                    case Data.Gadgets.BluePrint.EnchantedMetalbow:
                        return SpriteName.WeaponSpecialBow;
                }
                throw new NotImplementedException();
            }
        }
        public override string ToString()
        {
            switch (weaponType)
            {
                case Data.Gadgets.BluePrint.Sling:
                    return "Sling shot";
                case Data.Gadgets.BluePrint.ShortBow:
                    return "Short bow";
                case Data.Gadgets.BluePrint.LongBow:
                    return "Long bow";
                case Data.Gadgets.BluePrint.EnchantedLongbow:
                    return damage.Magic.ToString() + " enchanted long bow";
                case Data.Gadgets.BluePrint.MetalBow:
                    return Gadgets.Goods.Name(damage.Material) + " bow";
                case Data.Gadgets.BluePrint.EnchantedMetalbow:
                    return damage.Magic.ToString() + " enchanted " + Gadgets.Goods.Name(damage.Material) + " bow";

            }
            throw new NotImplementedException();
        }
        public override string GadgetInfo
        {
            get { return damage.DamageText() + TextLib.NewLine + "A weapon that fires projectiles. Requires " + ammo.ToString() + "s as ammo"; }
        }

        public override bool Scrappable
        {
            get { return weaponType == Data.Gadgets.BluePrint.MetalBow; }
        }
        override public GadgetList ScrapResult() 
        {
            GadgetList result = GadgetLib.ScrapMaterial(damage.Material, LootfestLib.CraftingBowMetalAmount, power, MediumPower, HighPower);
            return result;
        }
        public override bool CanBeEnchanted()
        {
            return (weaponType == Data.Gadgets.BluePrint.LongBow || weaponType == Data.Gadgets.BluePrint.MetalBow) &&
                damage.Magic == Magic.MagicElement.NoMagic;
        }
        public override bool UsesTargeting
        {
            get { return true; }
        }

        public override List<GadgetAlternativeUse> AlternativeUses(Players.PlayerProgress progress)
        {
            if (weaponType == Data.Gadgets.BluePrint.Sling) return null;

            return new List<GadgetAlternativeUse>
                {
                    new GadgetAlternativeUse( GadgetAlternativeUseType.Standard, "Arrow"),
                    new GadgetAlternativeUse( GadgetAlternativeUseType.GoldenArrow, "Golden arrow"),
                };
        }
    }
}
