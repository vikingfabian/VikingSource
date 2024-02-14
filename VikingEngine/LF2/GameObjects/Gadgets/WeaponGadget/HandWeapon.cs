using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//

namespace VikingEngine.LF2.GameObjects.Gadgets.WeaponGadget
{
    class HandWeapon : AbsWeaponGadget2, Process.ILoadImage
    {
        const float SpearAttackTime = 350;
        const float SpearReloadTime = 300;
        const float SpearRushAttackTime = 700;
        const float SpearRushReloadTime = 600;

        protected Graphics.VoxelObj originalSwordMesh;// = LootfestLib.Images.StandardVoxelObjects[VoxelModelName.Sword1];
        GadgetSaveCategory weaponSaveCategory = GadgetSaveCategory.ERR; 
        static readonly WeaponAttack.DamageData NoDamage = new WeaponAttack.DamageData(0, WeaponAttack.WeaponUserType.Player, ByteVector2.Zero);

        public HandWeapon(System.IO.BinaryReader r, byte version, GadgetSaveCategory saveCategory)
            : base()
        {
            if (saveCategory != GadgetSaveCategory.StandardHandWeapon && saveCategory != GadgetSaveCategory.CraftedHandWeapon)
                throw new NotImplementedException("Hand weapon cant load save cat: " + saveCategory.ToString());

            ReadStream(r, version, saveCategory);
            //createMesh();
        }

        public HandWeapon()
        {
            //do nothing //for reading file
        }


        //StandardHandWeapon init
        public HandWeapon(StandardHandWeaponType type)
        {
            weaponSaveCategory = GadgetSaveCategory.StandardHandWeapon;
            standardHandWeaponType = type;
            setStandardDamage();
            power = MediumPower;
            basicSetup();
        }


        /// <summary>
        /// calc weapon attributes, weapontype and material type must be set first
        /// </summary>
        protected void basicSetup() 
        {
            updateAttackReloadTime();
            //setStandardDamage();
            updateScale();
            //createMesh();
        }

        //public HandWeapon(Data.Gadgets.BluePrint weaponType, GoodsType materialType)
        //    : base(weaponType, materialType)
        //{
        //    weaponSaveCategory = GadgetSaveCategory.CraftedHandWeapon;
        //      // createMesh();
               
        //}


        public HandWeapon(Data.Gadgets.BluePrint weaponType, GoodsType materialType, Magic.MagicElement magicType)
        {
            weaponSaveCategory = GadgetSaveCategory.CraftedHandWeapon;
            this.weaponType = weaponType;
            power = 100;
            damage = new WeaponAttack.DamageData(0, WeaponAttack.WeaponUserType.Player, ByteVector2.Zero, materialType, magicType);
            powerToDamage();
            basicSetup();
        }


        public HandWeapon(Data.Gadgets.CraftingTemplate template)
            : base(template)
        {
            weaponSaveCategory = GadgetSaveCategory.CraftedHandWeapon;
            if (template.useItems != null)
            {
                damage = new WeaponAttack.DamageData(0, WeaponAttack.WeaponUserType.Player, ByteVector2.Zero);
                setStandardDamage();
                damage.Material = template.useItems[0].Type;
                updateAttackReloadTime();

                templateToPower(template);
                powerToDamage();
                updateScale();
            }
            //createMesh();
        }

        public override void WriteStream(System.IO.BinaryWriter w)
        {
            if (weaponSaveCategory == GadgetSaveCategory.StandardHandWeapon)
                writeStandardHandWeapon(w);
            else
                WriteCraftedWep(w);
        }
        public override void ReadStream(System.IO.BinaryReader r, byte version, GadgetSaveCategory saveCategory)
        {
            weaponSaveCategory = saveCategory;
            if (weaponSaveCategory == GadgetSaveCategory.StandardHandWeapon)
                readStandardHandWeapon(r, version);
            else
                ReadCraftedWep(r, version);
        }

        public void WriteCraftedWep(System.IO.BinaryWriter w)
        {
            w.Write((byte)weaponType);
            w.Write((byte)damage.Material);
            w.Write((byte)damage.Magic);

            w.Write(power);
        }
        public void ReadCraftedWep(System.IO.BinaryReader r, byte version)
        {
            weaponType = (Data.Gadgets.BluePrint)r.ReadByte();
            
            //remove enchanted items from old save version
            if (weaponType == Data.Gadgets.BluePrint.EnchantedAxe)
            {
                weaponType = Data.Gadgets.BluePrint.Axe;
            }
            else if (weaponType == Data.Gadgets.BluePrint.EnchantedSword)
            {
                weaponType = Data.Gadgets.BluePrint.Sword;
            }
            else if (weaponType == Data.Gadgets.BluePrint.EnchantedWoodSword)
            {
                weaponType = Data.Gadgets.BluePrint.WoodSword;
            }
            else if (weaponType == Data.Gadgets.BluePrint.EnchantedLongbow)
            {
                weaponType = Data.Gadgets.BluePrint.LongBow;
            }
            else if (weaponType == Data.Gadgets.BluePrint.EnchantedMetalbow)
            {
                weaponType = Data.Gadgets.BluePrint.MetalBow;
            }

            damage = new WeaponAttack.DamageData(0, WeaponAttack.WeaponUserType.Player, ByteVector2.Zero);
            damage.Material = (GoodsType)r.ReadByte();
            damage.Magic = (Magic.MagicElement)r.ReadByte();
            power = r.ReadByte();
            if (power <= 0) power = MediumPower;

            powerToDamage();
            updateScale();
            updateAttackReloadTime();
        }

        public void writeStandardHandWeapon(System.IO.BinaryWriter w)
        {
            writeStandardWeapon(w, (int)standardHandWeaponType);
        }
        public void readStandardHandWeapon(System.IO.BinaryReader r, byte version)
        {
            standardHandWeaponType = (StandardHandWeaponType)readStandardWeapon(r, version);
            setStandardDamage();
            basicSetup();
            updateAttackReloadTime();
            updateScale();
        }

        protected void updateScale()
        {
            const float HandWeaponScale = 0.2f;
            const float TwoHandSwordScale = 0.26f;
            const float TwoHandAxeScale = 0.26f;
            const float KnifeScale = 0.12f;
            const float SpearScale = 0.16f;

            if (weaponType == Data.Gadgets.BluePrint.Stick)
            {
                scale = Stick.StickScale;
            }
            else if (weaponType == Data.Gadgets.BluePrint.LongSword || weaponType == Data.Gadgets.BluePrint.EnchantedLongSword)
            {
                scale = TwoHandSwordScale;
            }
            else if (weaponType == Data.Gadgets.BluePrint.LongAxe)
            {
                scale = TwoHandAxeScale;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Spear)
            {
                scale = SpearScale;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Dagger)
            {
                scale = KnifeScale;
            }
            else
            {
                scale = HandWeaponScale;
            }
        }
        protected void updateAttackReloadTime()
        {
            if (weaponType == Data.Gadgets.BluePrint.LongSword || weaponType == Data.Gadgets.BluePrint.EnchantedLongSword)
            {
                attacktime = 400;
                reloadtime = 500;
            }
            else if (weaponType == Data.Gadgets.BluePrint.PickAxe || weaponType ==  Data.Gadgets.BluePrint.Sickle || weaponType == Data.Gadgets.BluePrint.BuildHammer)
            {
                attacktime = 300;
                reloadtime = 400;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Axe)
            {
                attacktime = 400;
                reloadtime = 300;
            }
            else if (weaponType == Data.Gadgets.BluePrint.LongAxe)
            {
                attacktime = 400;
                reloadtime = 500;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Sword)
            {
                attacktime = 500;
                reloadtime = 200;
            }
            else if (weaponType == Data.Gadgets.BluePrint.WoodSword)
            {
                attacktime = 500;
                reloadtime = 150;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Dagger)
            {
                attacktime = 300;
                reloadtime = 200;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Spear)
            {
                attacktime = SpearAttackTime;
                reloadtime = SpearReloadTime;
            }
            else
            {
                throw new NotImplementedException("updateAttackReloadTime on type: " + weaponType.ToString());
            }

        }

        public override void Enchant(Goods magicGem)
        {
            base.Enchant(magicGem);
            weaponSaveCategory = GadgetSaveCategory.CraftedHandWeapon;
            CreateMesh(); //the weapon should change appearance
        }

        

        override protected void CreateMesh()
        {
            if (originalSwordMesh == null)
                originalSwordMesh = LootfestLib.Images.StandardVoxelObjects[VoxelModelName.Sword1];

            bool gotShaft = false;
            bool modifyColors = true;
            VoxelModelName model;


            if (weaponType == Data.Gadgets.BluePrint.Axe)
            {
                model = VoxelModelName.axe_base;
                gotShaft = true;
            }
            else if (weaponType == Data.Gadgets.BluePrint.LongAxe)
            {
                model = VoxelModelName.axe2h_base;
                gotShaft = true;
            }
            else if (weaponType == Data.Gadgets.BluePrint.PickAxe)
            {
                model = VoxelModelName.pickaxe_base;
                gotShaft = true;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Dagger)
            {
                model = VoxelModelName.knife_base;
                gotShaft = true;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Sickle)
            {
                model = VoxelModelName.sickle_base;
                gotShaft = true;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Spear)
            {
                model = VoxelModelName.spear_base;
                gotShaft = true;
            }
            else if (weaponType == Data.Gadgets.BluePrint.BuildHammer)
            {
                model = VoxelModelName.buildhammer;
                modifyColors = false;
            }
            else
            {
                model = VoxelModelName.sword_base;
                gotShaft = false;
            }


            if (modifyColors)
            {

                Data.MaterialType bladeColor;
                Data.MaterialType edgeColor;
                Data.MaterialType hiltOrShaftCol;

                switch (damage.Material)
                {
                    default:
                        bladeColor = Data.MaterialType.blue_gray;
                        edgeColor = Data.MaterialType.iron;
                        hiltOrShaftCol = Data.MaterialType.dark_gray;
                        break;
                    case GoodsType.Bronze:
                        bladeColor = Data.MaterialType.red_brown;
                        edgeColor = Data.MaterialType.bronze;
                        hiltOrShaftCol = Data.MaterialType.bronze;
                        break;
                    case GoodsType.Silver:
                        bladeColor = Data.MaterialType.white;
                        edgeColor = Data.MaterialType.blue;
                        hiltOrShaftCol = Data.MaterialType.dark_blue;
                        break;
                    case GoodsType.Gold:
                        bladeColor = Data.MaterialType.yellow;
                        edgeColor = Data.MaterialType.gold;
                        hiltOrShaftCol = Data.MaterialType.bone;
                        break;
                    case GoodsType.Mithril:
                        bladeColor = Data.MaterialType.cyan;
                        edgeColor = Data.MaterialType.white;
                        hiltOrShaftCol = Data.MaterialType.blue;
                        break;
                    case GoodsType.Wood:
                        bladeColor = Data.MaterialType.wood_growing;
                        edgeColor = Data.MaterialType.wood;
                        hiltOrShaftCol = Data.MaterialType.brown;
                        break;
                }

                if (damage.Magic != Magic.MagicElement.NoMagic)
                {
                    switch (damage.Magic)
                    {
                        case Magic.MagicElement.Fire:
                            bladeColor = Data.MaterialType.yellow;
                            edgeColor = Data.MaterialType.red;
                            break;
                        case Magic.MagicElement.Lightning:
                            bladeColor = Data.MaterialType.blue;
                            edgeColor = Data.MaterialType.lightning;
                            break;
                        case Magic.MagicElement.Poision:
                            bladeColor = Data.MaterialType.mossy_green;
                            edgeColor = Data.MaterialType.zombie_skin;
                            break;
                        case Magic.MagicElement.Evil:
                            bladeColor = Data.MaterialType.black;
                            edgeColor = Data.MaterialType.dark_gray;
                            hiltOrShaftCol = Data.MaterialType.red;
                            break;

                    }
                }

                if (gotShaft)
                {
                    hiltOrShaftCol = Data.MaterialType.wood_growing;
                }

                new Process.ModifiedImage(this, model,
                    new List<ByteVector2>
                {
                    new ByteVector2((byte)Data.MaterialType.blue_gray, (byte)bladeColor),
                    new ByteVector2((byte)Data.MaterialType.iron, (byte)edgeColor),
                    new ByteVector2((byte)Data.MaterialType.gold, (byte)hiltOrShaftCol),

                }, null, Vector3.Zero);
            }
            else
            {
                LoadMasterImage(model);
            }

        }
        public void SetCustomImage(Graphics.VoxelModel original, int link)
        {
            originalSwordMesh = (Graphics.VoxelObj)original;
        }

        protected void LoadMasterImage(VoxelModelName name)
        {
            new Process.LoadImage(this, name, Vector3.Zero);
        }

        public override void SetupDamage(Characters.Hero parent, List<Characters.Condition.AbsHeroCondition> modifiers)
        {
#if !CMODE
            if (weaponType == Data.Gadgets.BluePrint.WoodSword || weaponType == Data.Gadgets.BluePrint.EnchantedWoodSword)
            {
                if (parent.Player.Progress.GotSkill(Magic.MagicRingSkill.Elven_warrior))
                    modifiedDamage.Damage = LootfestLib.SwordDamage;
            }
            if (damage.Magic != Magic.MagicElement.NoMagic)
            {
                if (parent.Player.Progress.GotSkill(Magic.MagicRingSkill.Paladins))
                {
                    damage.Special = WeaponAttack.SpecialDamage.HandWeaponProjectile;
                }
            }
#endif
            base.SetupDamage(parent, modifiers);
            
        }

        public override Vector2 Use(Characters.Hero parent, Vector3 target, GadgetAlternativeUseType altUse, bool localUse)
        {
            Vector2 time_reload = new Vector2(attacktime, reloadtime);
            modifiedDamage.User = WeaponAttack.WeaponUserType.Player;

            if (originalSwordMesh == null)
                throw new NullReferenceException("Use handweapon");

            if (localUse && weaponType == Data.Gadgets.BluePrint.BuildHammer)
            { //creative mode
                parent.Player.BeginCreationModeIfAllowed();
            }

            if (weaponType == Data.Gadgets.BluePrint.Spear)
            {
                if (altUse == GadgetAlternativeUseType.Rush)
                {
                    time_reload.X = SpearRushAttackTime;
                    time_reload.Y = SpearRushReloadTime;
                }
                new WeaponAttack.HandSpearAttack(new Graphics.VoxelModelInstance(originalSwordMesh), time_reload.X, parent, modifiedDamage, localUse);
            }
            else
            {
                new WeaponAttack.HandWeaponAttack(time_reload.X, parent, new Graphics.VoxelModelInstance(originalSwordMesh),
                    scale, modifiedDamage, weaponType, localUse);
            }
            return time_reload;
        }
        public override string ToString()
        {
            return damage.Material.ToString() + " " + weaponType.ToString();
        }
        public override bool UsesAmmo
        {
            get { return false; }
        }

        override public GadgetSaveCategory SaveCategory
        {
            get { return weaponSaveCategory; }
        }

        override public bool HaltUser
        {
            get
            {
                return weaponType == Data.Gadgets.BluePrint.Axe || 
                   weaponType ==  Data.Gadgets.BluePrint.EnchantedAxe ||
                   weaponType ==  Data.Gadgets.BluePrint.LongAxe||
                   weaponType == Data.Gadgets.BluePrint.PickAxe|| 
                   weaponType == Data.Gadgets.BluePrint.Spear;
            }
        }
        public override SpriteName Icon
        {
            get 
            {
                if (weaponType == Data.Gadgets.BluePrint.EnchantedWoodSword || weaponType == Data.Gadgets.BluePrint.WoodSword)
                {
                    return SpriteName.WeaponSwordWood;
                }
                else if (weaponType == Data.Gadgets.BluePrint.Sword || weaponType == Data.Gadgets.BluePrint.EnchantedSword)
                {
                    switch (damage.Material)
                    {
                        case GoodsType.Wood:
                            return SpriteName.WeaponSwordWood;
                        case GoodsType.Iron:
                            return SpriteName.WeaponSwordIron;
                        case GoodsType.Bronze:
                            return SpriteName.WeaponSwordBronze;
                        case GoodsType.Silver:
                            return SpriteName.WeaponSwordSilver;
                        case GoodsType.Gold:
                            return SpriteName.WeaponSwordGold;
                        case GoodsType.Mithril:
                            return SpriteName.WeaponSwordMithril;
                    }
                }
                else if (weaponType == Data.Gadgets.BluePrint.LongSword || weaponType == Data.Gadgets.BluePrint.EnchantedLongSword)
                {
                    switch (damage.Material)
                    {
                        
                        case GoodsType.Iron:
                            return SpriteName.WeaponLongSwordIron;
                        case GoodsType.Bronze:
                            return SpriteName.WeaponLongSwordBronze;
                        case GoodsType.Silver:
                            return SpriteName.WeaponLongSwordSilver;
                        case GoodsType.Gold:
                            return SpriteName.WeaponLongSwordGold;
                        case GoodsType.Mithril:
                            return SpriteName.WeaponLongSwordMithril;
                    }
                }
                else if (weaponType == Data.Gadgets.BluePrint.Axe || weaponType == Data.Gadgets.BluePrint.EnchantedAxe)
                {
                    switch (damage.Material)
                    {
                        case GoodsType.Iron:
                            return SpriteName.WeaponAxeIron;
                        case GoodsType.Bronze:
                            return SpriteName.WeaponAxeBronze;
                        case GoodsType.Silver:
                            return SpriteName.WeaponAxeSilver;
                        case GoodsType.Gold:
                            return SpriteName.WeaponAxeGold;
                        case GoodsType.Mithril:
                            return SpriteName.WeaponAxeMithril;
                    }
                
                }
                else if (weaponType == Data.Gadgets.BluePrint.Spear)
                {
                    return SpriteName.WeaponHandSpear;
                }
                else if (weaponType == Data.Gadgets.BluePrint.PickAxe)
                {
                    return SpriteName.WeaponPickAxe;
                }
                else if (weaponType == Data.Gadgets.BluePrint.Sickle)
                {
                    return SpriteName.WeaponSickle;
                }
                else if (weaponType == Data.Gadgets.BluePrint.LongAxe)
                {
                    switch (damage.Material)
                    {
                        case GoodsType.Bronze:
                            return SpriteName.WeaponDaneAxeBronze;
                        case GoodsType.Iron:
                            return SpriteName.WeaponDaneAxeIron;
                        case GoodsType.Silver:
                            return SpriteName.WeaponDaneAxeSilver;
                        case GoodsType.Gold:
                            return SpriteName.WeaponDaneAxeGold;
                        case GoodsType.Mithril:
                            return SpriteName.WeaponDaneAxeMithril;
                    }
                }
                else if (weaponType == Data.Gadgets.BluePrint.Dagger)
                {
                    switch (damage.Material)
                    {
                        case GoodsType.Bronze:
                            return SpriteName.WeaponDaggerBronze;
                        case GoodsType.Iron:
                            return SpriteName.WeaponDaggerIron;
                        case GoodsType.Silver:
                            return SpriteName.WeaponDaggerSilver;
                        case GoodsType.Gold:
                            return SpriteName.WeaponDaggerGold;
                        case GoodsType.Mithril:
                            return SpriteName.WeaponDaggerMithril;
                    }
                }
                else if (weaponType == Data.Gadgets.BluePrint.BuildHammer)
                {
                    return SpriteName.WeaponBuildHammer;
                }
                return SpriteName.NO_IMAGE;  
            }
        }

        StandardHandWeaponType standardHandWeaponType
        {
            get
            {
                switch (weaponType)
                {
                    case Data.Gadgets.BluePrint.Dagger:
                        switch (damage.Material)
                        {
                            case GoodsType.Iron:
                                return StandardHandWeaponType.IronDagger;
                            case GoodsType.Bronze:
                                return StandardHandWeaponType.BronzeDagger;
                        }
                        break;
                    case Data.Gadgets.BluePrint.Sword:
                        switch (damage.Material)
                        {
                            case GoodsType.Iron:
                                return StandardHandWeaponType.IronSword;
                            case GoodsType.Bronze:
                                return StandardHandWeaponType.BronzeSword;
                        }
                        break;
                    case Data.Gadgets.BluePrint.LongSword:
                        switch (damage.Material)
                        {
                            case GoodsType.Iron:
                                return StandardHandWeaponType.IronLongSword;
                            case GoodsType.Bronze:
                                return StandardHandWeaponType.BronzeLongSword;
                        }
                        break;
                    case Data.Gadgets.BluePrint.Axe:
                        switch (damage.Material)
                        {
                            case GoodsType.Iron:
                                return StandardHandWeaponType.IronAxe;
                            case GoodsType.Bronze:
                                return StandardHandWeaponType.BronzeAxe;
                        }
                       break;
                    case Data.Gadgets.BluePrint.LongAxe:
                       switch (damage.Material)
                       {
                           case GoodsType.Iron:
                               return StandardHandWeaponType.IronLongAxe;
                           case GoodsType.Bronze:
                               return StandardHandWeaponType.BronzeLongAxe;
                       }
                       break;
                    case Data.Gadgets.BluePrint.Sickle:
                       return StandardHandWeaponType.Sickle;
                    case Data.Gadgets.BluePrint.PickAxe:
                       return StandardHandWeaponType.PickAxe;
                    case Data.Gadgets.BluePrint.BuildHammer:
                       return StandardHandWeaponType.BuildHammer;
                    case Data.Gadgets.BluePrint.Spear:
                       return StandardHandWeaponType.Spear;
                }
                throw new NotImplementedException("get standardHandWeaponType:" + weaponType.ToString());
            }
            set
            {
                switch (value)
                {
                    case StandardHandWeaponType.BronzeAxe:
                        damage.Material = GoodsType.Bronze;
                        weaponType = Data.Gadgets.BluePrint.Axe;
                        break;
                    case StandardHandWeaponType.BronzeSword:
                        damage.Material = GoodsType.Bronze;
                        weaponType = Data.Gadgets.BluePrint.Sword;
                        break;
                    case StandardHandWeaponType.BronzeDagger:
                        damage.Material = GoodsType.Bronze;
                        weaponType = Data.Gadgets.BluePrint.Dagger;
                        break;

                    case StandardHandWeaponType.IronAxe:
                        damage.Material = GoodsType.Iron;
                        weaponType = Data.Gadgets.BluePrint.Axe;
                        break;
                    case StandardHandWeaponType.IronLongAxe:
                        damage.Material = GoodsType.Iron;
                        weaponType = Data.Gadgets.BluePrint.LongAxe;
                        break;
                    
                    case StandardHandWeaponType.IronSword:
                        damage.Material = GoodsType.Iron;
                        weaponType = Data.Gadgets.BluePrint.Sword;
                        break;
                    case StandardHandWeaponType.IronDagger:
                        damage.Material = GoodsType.Iron;
                        weaponType = Data.Gadgets.BluePrint.Dagger;
                        break;
                    case StandardHandWeaponType.BronzeLongSword:
                        damage.Material = GoodsType.Bronze;
                        weaponType = Data.Gadgets.BluePrint.LongSword;
                        break;
                    case StandardHandWeaponType.BronzeLongAxe:
                        damage.Material = GoodsType.Bronze;
                        weaponType = Data.Gadgets.BluePrint.LongAxe;
                        break;
                    case StandardHandWeaponType.IronLongSword:
                        damage.Material = GoodsType.Iron;
                        weaponType = Data.Gadgets.BluePrint.LongSword;
                        break;
                    case StandardHandWeaponType.Sickle:
                        damage.Material = GoodsType.Silver;
                        weaponType = Data.Gadgets.BluePrint.Sickle;
                        break;
                    case StandardHandWeaponType.PickAxe:
                        damage.Material = GoodsType.Iron;
                        weaponType = Data.Gadgets.BluePrint.PickAxe;
                        break;
                    case StandardHandWeaponType.BuildHammer:
                        damage.Material = GoodsType.Iron;
                        weaponType = Data.Gadgets.BluePrint.BuildHammer;
                        break;
                    case StandardHandWeaponType.Spear:
                        damage.Material = GoodsType.Iron;
                        weaponType = Data.Gadgets.BluePrint.Spear;
                        break;
                    default:
                        throw new NotImplementedException("set standardHandWeaponType:" + value.ToString());


                }
            }
        }

        public override string GadgetInfo
        {
            get
            {
                string result = damage.DamageText() + TextLib.NewLine;

                if (weaponType == Data.Gadgets.BluePrint.WoodSword || weaponType == Data.Gadgets.BluePrint.EnchantedWoodSword)
                {
                    result += "A practice weapon. Not supposed to be used on real combat.";
                }
                else if (weaponType == Data.Gadgets.BluePrint.Axe || weaponType == Data.Gadgets.BluePrint.EnchantedAxe)
                {
                    result += "Axes are strong and cheap. But they are slow and requires a lot of skill to use.";
                }
                else if (weaponType == Data.Gadgets.BluePrint.LongSword || weaponType == Data.Gadgets.BluePrint.EnchantedLongSword)
                {
                    result += "A two handed sword with a powerful range. Can't be used together with a shield.";
                }
                else if (weaponType == Data.Gadgets.BluePrint.LongAxe)
                {
                    result += "A two handed axe, has a very slow but powerful swing.";
                }
                else if (weaponType == Data.Gadgets.BluePrint.PickAxe)
                {
                    result += "Use on the stone pieces that you can mine around the world. A good pickaxe will increase the chance of finding rare minerals.";
                }
                else if (weaponType == Data.Gadgets.BluePrint.BuildHammer)
                {
                    result = "Use in build areas to start creative mode";
                }
                else if (weaponType == Data.Gadgets.BluePrint.Sickle)
                {
                    result += "A tool used to cut magic herbes without damaging them.";
                }
                else if (weaponType == Data.Gadgets.BluePrint.Dagger)
                {
                    result += "A knife requires you to get close and personal, to deliver a deadly strike.";
                }
                else if (weaponType == Data.Gadgets.BluePrint.Sword)
                {
                    result += "Swords are fast and easy to use.";
                }
                else if (weaponType == Data.Gadgets.BluePrint.Spear)
                {
                    result += "Spears are useful to poke your enemies at a safe distance";
                }
                else
                {
                    throw new NotImplementedException("GadgetInfo: " + weaponType.ToString());
                }

                if (damage.Magic != Magic.MagicElement.NoMagic)
                {
                    result += " The weapon is enchanted with " + damage.Magic.ToString() + " magic.";
                }

                return result;
            }

        }
        public override UseHands Hands
        {
            get
            {
                return (weaponType == Data.Gadgets.BluePrint.LongSword || weaponType == Data.Gadgets.BluePrint.EnchantedLongSword || weaponType == Data.Gadgets.BluePrint.LongAxe)? 
                    UseHands.TwoHands : UseHands.OneHand;
            }
        }

        override protected byte WeaponHashTag { get { 
            if (weaponSaveCategory == GadgetSaveCategory.StandardHandWeapon)
                return (byte)((int)standardHandWeaponType + 11);
            else
                return (byte)(((int)damage.Material + power) % PublicConstants.ByteSize); 
        } }
        public override bool Scrappable
        {
            get {  return weaponType != Data.Gadgets.BluePrint.WoodSword; }
        }
        override public GadgetList ScrapResult()
        {
            int materialAmount;
            if (weaponType == Data.Gadgets.BluePrint.Axe || weaponType == Data.Gadgets.BluePrint.EnchantedAxe)
            {
               materialAmount = LootfestLib.CraftingAxeMetalAmount;
            }
            else if (weaponType == Data.Gadgets.BluePrint.LongSword || weaponType == Data.Gadgets.BluePrint.EnchantedLongSword)
            {
                materialAmount = LootfestLib.CraftingLongSwordMetalAmount;
            }
            else if (weaponType == Data.Gadgets.BluePrint.LongAxe)
            {
                materialAmount = LootfestLib.CraftingLongAxeMetalAmount;
            }
            else if (weaponType == Data.Gadgets.BluePrint.PickAxe)
            {
                materialAmount = LootfestLib.CraftingPickAxeMetalAmount;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Sickle)
            {
                materialAmount = LootfestLib.CraftingSickleMetalAmount;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Sword)
            { //short sword
                materialAmount = LootfestLib.CraftingSwordMetalAmount;
            }
            else if (weaponType == Data.Gadgets.BluePrint.Spear)
            { //short sword
                materialAmount = LootfestLib.CraftingSpearMetalAmount;
            }
            else
            {
                throw new NotImplementedException("ScrapResult, " + this.ToString());
            }
            GadgetList result = GadgetLib.ScrapMaterial(damage.Material, materialAmount, power, MediumPower, HighPower);

            return result;
        }

        public override List<GadgetAlternativeUse> AlternativeUses(Players.PlayerProgress progress)
        {
            if (weaponType == Data.Gadgets.BluePrint.Spear && progress.GotSkill(GameObjects.Magic.MagicRingSkill.Spear_rush))
            {
                return new List<GadgetAlternativeUse>
                {
                    new GadgetAlternativeUse( GadgetAlternativeUseType.Standard, "Standard"),
                    new GadgetAlternativeUse( GadgetAlternativeUseType.Rush, "Rush"),
                };
            }
            return base.AlternativeUses(progress);
        }

        public override bool CanBeEnchanted()
        {
            return weaponType != Data.Gadgets.BluePrint.PickAxe && weaponType != Data.Gadgets.BluePrint.Sickle &&
                damage.Magic == Magic.MagicElement.NoMagic;
        }
        public override bool UsesTargeting
        {
            get { return false; }
        }
        override  public bool Empty { get { return weaponType == Data.Gadgets.BluePrint.BuildHammer; } }
    }

    enum StandardHandWeaponType
    {
        BronzeAxe,
        BronzeSword,
        IronSword,
        IronAxe,
        BronzeLongSword,
        IronLongSword,
        BronzeLongAxe,
        IronLongAxe,
        BronzeDagger,
        IronDagger,
        PickAxe,
        Sickle,
        Spear,

        BuildHammer,
        NUM,

        
    }
}
