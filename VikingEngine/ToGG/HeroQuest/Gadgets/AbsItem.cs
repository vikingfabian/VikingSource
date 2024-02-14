using System;
using System.Collections.Generic;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    abstract class AbsItem : Net.INetRequestReciever
    {
        protected const int MaxStackCount = 99;
        abstract public EquipSlots Equip { get; }

        static int NextId = 0;

        public ItemFilter type;
        protected LocalPlayer player;
        public Graphics.Image itemImage = null;
        public int id = NextId++;
        public int count = 1;
        public IntVector2 placement;
        public int placementPage;

        //abstract public ItemMainType MainType { get; }
        //abstract public int SubType { get; }
        
        public AbsItem(ItemMainType mainType, int subType = int.MinValue)
        {
            type = new ItemFilter(mainType, subType);
        }

        abstract public SpriteName Icon { get; }
        abstract public string Name { get; }

        public string NameAndCount()
        {
            if (StackLimit <= 1)
            {
                return Name;
            }
            else
            {
                return Name + " (" + count.ToString() + ")";
            }
        }

        virtual public List<HUD.RichBox.AbsRichBoxMember> DescriptionAdvanced()
        {
            return null;
        }

        public List<HUD.RichBox.AbsRichBoxMember> DescAsRichbox()
        {
            List<HUD.RichBox.AbsRichBoxMember> result = DescriptionAdvanced();
            if (result == null)
            {
                result = new List<HUD.RichBox.AbsRichBoxMember>
                { new HUD.RichBox.RichBoxText(Description) };
            }

            return result;
        }

        abstract public string Description { get; }
        virtual public List<HUD.RichBox.AbsRichBoxMember> DescriptionIcons()
        {
            return null;
        }

        public void toRichbox(List<HUD.RichBox.AbsRichBoxMember> richbox)
        {
            richbox.Add(new HUD.RichBox.RichBoxImage(Icon));
            richbox.Add(new HUD.RichBox.RichBoxText(NameAndCount()));
            richbox.Add(new HUD.RichBox.RichBoxNewLine());
        }

        virtual public int StackLimit => 1;

        public AbsItem Clone()
        {
            AbsItem clone = createItem(this.type);
            cloneDataTo(clone);

            return clone;
        }
        virtual protected void cloneDataTo(AbsItem toItem)
        {
            toItem.placement = this.placement;
            toItem.count = this.count;
        }

        public void placeInSlot(VectorRect area, ImageLayers itemlayer)
        {
            if (itemImage == null)
            {
                itemImage = new Graphics.Image(Icon, area.Center, area.Size, itemlayer, true);
            }

            if (itemImage.IsDeleted)
            {
                itemImage.AddToRender();
            }
            itemImage.Area = area.CenterSize;
            itemImage.Layer = itemlayer;
            itemImage.Visible = true;
        }

        public void setVisible(bool value)
        {
            itemImage.Visible = value;
        }

        virtual public List<IntVector2> quickUse_TargetSquares(Unit user, out bool attackTarget)
        {
            throw new NotImplementedException();
        }

        protected static System.IO.BinaryWriter NetWriteQuickUse(AbsItem item)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqUseItem, Network.PacketReliability.Reliable);
            item.writeItem(w, null);

            return w;
        }

        public static void NetReadQuickUse(Network.ReceivedPacket packet)
        {
            var item = ReadItem(packet.r);
            item.netReadQuickUse(packet);
        }

        virtual protected void netReadQuickUse(Network.ReceivedPacket packet)
        {
            Unit u = Unit.NetReadUnitId(packet.r);
            quickUseOnUnit(null, u);
        }

        virtual public void quickUse(LocalPlayer player, IntVector2 pos)
        {
            this.player = player;
        }

        virtual public void quickUseOnUnit(LocalPlayer player, Unit unit)
        {
            throw new NotImplementedException();
        }

        public void DeleteImage()
        {
            itemImage?.DeleteMe();
            itemImage = null;
        }

        public void restoreToPlayer()
        {
            player.Backpack().equipment.quickbelt.addItem(this);
        }

        virtual public void writeItem(System.IO.BinaryWriter w, int? orCount)
        {
            type.write(w);
            if (orCount == null)
            {
                orCount = count;
            }
            w.Write((byte)orCount.Value);
        }

        public static AbsItem ReadItem(System.IO.BinaryReader r)
        {
            ItemFilter type = new ItemFilter(r);

            AbsItem item = createItem(type);

            if (item == null)
            {
                throw new NotImplementedExceptionExt("read " + type.ToString(), type.TypeId);
            }
            item.count = r.ReadByte();

            return item;
        }
        public static AbsItem createItem(ItemMainType mainType, int subtype = int.MinValue)
        {
            return createItem(new ItemFilter(mainType, subtype));
        }

        public static AbsItem createItem(ItemFilter type)
        {
            AbsItem item = null;

            switch (type.mainType)
            {
                case ItemMainType.Coins:
                    item = new Coins(1);
                    break;
                    
                case ItemMainType.Potion:
                    switch ((PotionType)type.subType)
                    {
                        case PotionType.Healing:
                            item = new HealingPotion();
                            break;
                        case PotionType.Stamina:
                            item = new StaminaPotion();
                            break;
                        case PotionType.HealAndStamina:
                            item = new PheonixPotion();
                            break;
                        case PotionType.Apple:
                            item = new ApplePotion();
                            break;
                        case PotionType.SmokeBomb:
                            item = new SmokeBomb();
                            break;
                    }
                    break;

                case ItemMainType.QuickWeapon:
                    item = new ThrowingKnife();
                    break;

                case ItemMainType.Spawn:
                    switch ((ItemSpawnType)type.subType)
                    {
                        case ItemSpawnType.Decoy:
                            item = new Decoy();
                            break;
                        case ItemSpawnType.ArmoredDecoy:
                            item = new ArmoredDecoy();
                            break;
                        case ItemSpawnType.RougeTrap:
                            item = new RougeTrapItem();
                            break;
                        case ItemSpawnType.TrapDecoy:
                            item = new TrapDecoy();
                            break;
                    }
                    break;

                case ItemMainType.SpecialBolt:
                case ItemMainType.SpecialArrow:
                    item = new SpecialArrow(type.mainType == ItemMainType.SpecialArrow ? ArrowType.Arrow : ArrowType.Bolt,
                        (ArrowSpecialType)type.subType, 1);
                    break;

                case ItemMainType.RuneKey:
                    item = new RuneKey((RuneKeyType)type.subType);
                    break;

                case ItemMainType.Trinket:
                    switch ((TrinketType)type.subType)
                    {
                        case TrinketType.ProtectionRune:
                            item = new ProtectionRune();
                            break;
                        case TrinketType.WaterBottle:
                            item = new WaterBottle();
                            break;
                    }
                    break;

                case ItemMainType.Weapon:
                    switch ((WeaponType)type.subType)
                    {
                        case WeaponType.ArcherDagger:
                            item = new ArcherDagger();
                            break;
                        case WeaponType.BaseDagger:
                            item = new BaseDagger();
                            break;
                        case WeaponType.BerserkerAxe:
                            item = new BerserkerAxe();
                            break;
                        case WeaponType.ElfBow:
                            item = new ElfBow();
                            break;
                        case WeaponType.KnightSword:
                            item = new KnightSword();
                            break;
                        case WeaponType.Spear:
                            item = new Spear();
                            break;
                        case WeaponType.StartingBow:
                            item = new StartingBow();
                            break;
                        case WeaponType.StartingSword:
                            item = new StartingSword();
                            break;
                        case WeaponType.StartingThrowDaggers:
                            item = new StartingThrowDaggers();
                            break;
                        case WeaponType.Whip:
                            item = new Whip();
                            break;
                        case WeaponType.ThrowDaggersTier2:
                            item = new ThrowDaggersTier2();
                            break;
                        case WeaponType.ThrowDaggersPoisionous:
                            item = new ThrowDaggersPoisionous();
                            break;
                    }
                    break;

                case ItemMainType.Shield:
                    switch ((ShieldType)type.subType)
                    {
                        case ShieldType.Round:
                            item = new ShieldRound();
                            break;
                    }
                    break;

                case ItemMainType.Armor:
                    switch ((ArmorType)type.subType)
                    {
                        case ArmorType.Chainmail:
                            item = new ArmorChainmail();
                            break;
                        case ArmorType.Elf:
                            item = new ArmorElf();
                            break;
                        case ArmorType.Leather:
                            item = new ArmorLeather();
                            break;
                    }
                    break;

                //default:
                //    throw new NotImplementedExceptionExt("Create " + type.ToString(), type.TypeId);
            }

            if (item == null)
            {
                throw new NotImplementedExceptionExt("Create " + type.ToString(), type.TypeId);
            }

            return item;
        }

        virtual public void NetRequestCallback(bool successful)
        {
            if (!successful)
            {
                restoreToPlayer();
            }
        }

        virtual public Display3D.UnitStatusGuiSettings? targetUnitsGui()
        {
            return null;
        }

        public bool slotAccess(ItemSlot slot)
        {
            return slotAccess(slot.slotType);
        }

        public bool slotAccess(SlotType type)
        {
            if (type == SlotType.Backpack ||
                type == SlotType.OnGround)
            {
                return true;
            }
            else
            {
                return Equip.Contains(type);
            }
        }
        
        virtual public bool Combine(AbsItem otherItem)
        {
            if (stackTo(otherItem))
            {
                count += otherItem.count;

                if (count <= StackLimit)
                {
                    otherItem.count = 0;
                    otherItem.DeleteImage();
                }
                else
                {
                    otherItem.count = count - StackLimit;
                    count = StackLimit;
                }

                return true;
            }

            return false;
        }

        virtual public bool stackTo(AbsItem otherItem)
        {
            if (StackLimit > 1 &&
                otherItem.type == this.type &&
                count < StackLimit &&
                otherItem.count < otherItem.StackLimit)
            {
                return true;
            }

            return false;
        }

        public bool isType(AbsItem otherItem)
        {
            return otherItem.type == this.type;
        }

        public bool isType(ItemFilter otherType)
        {
            return this.type == otherType;
        }

        virtual public void OnTurnStart()
        {
        }

        virtual public void OnPickUp()
        {
        }

        virtual public void collectDefence(DefenceData defence, bool onCommit)
        {

        }

        virtual public bool CarryOnly => false;

        public override string ToString()
        {
            return "Item(" + Name + "(" + count.ToString() + "), id" + id.ToString() + ")";
        }



        virtual public int CoinValue => throw new NotImplementedExceptionExt("Coin value " + this.ToString(), type.TypeId); 
    }

    struct ItemFilter
    {
        public static readonly ItemFilter Coins = new ItemFilter(ItemMainType.Coins);

        public ItemMainType mainType;
        public int subType;

        public ItemFilter(ItemMainType mainType, int subType = int.MinValue)
        {
            this.mainType = mainType;
            this.subType = subType;
            
        }

        public ItemFilter(System.IO.BinaryReader r)
        {
            mainType = (ItemMainType)r.ReadByte();
            byte sub = r.ReadByte();
            if (sub == byte.MaxValue)
            {
                subType = int.MinValue;
            }
            else
            {
                subType = sub;
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)mainType);
            if (subType >= 0)
            {
                w.Write((byte)subType);
            }
            else
            {
                w.Write(byte.MaxValue); 
            }

        }
                
        public static bool operator ==(ItemFilter value1, ItemFilter value2)
        {
            if (value1.mainType == value2.mainType)
            {
                if (value1.HasSubType && value2.HasSubType)
                {
                    return value1.subType == value2.subType;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        public static bool operator !=(ItemFilter value1, ItemFilter value2)
        {
            return value1.mainType != value2.mainType || value1.subType != value2.subType;
        }

        public bool HasSubType => subType >= 0;

        public int TypeId => 1000 * (int)mainType + subType;

        public override string ToString()
        {
            string result= "Item: " +  mainType.ToString();
            if (subType >= 0)
            {
                result += subType.ToString();
            }

            return result;
        }
    }

    enum ItemUseType
    {
        OnUnit,
        OnSquare,
    }

    enum ItemMainType
    {
        Coins,
        //CoinPile,
        Potion,
        QuickWeapon,
        Spawn,
        SpecialArrow,
        SpecialBolt,
        Armor,
        Shield,
        Trinket,
        Weapon,
        RuneKey,
        Carry,
    }
}
