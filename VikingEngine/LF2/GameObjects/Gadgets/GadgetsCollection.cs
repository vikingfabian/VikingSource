using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2;

namespace VikingEngine.LF2.GameObjects.Gadgets
{
    //all objects that can be found/picked up/given away/kept in chests
    interface IGadgetsCollection
    {
        void AddItem(IGadget gadget);
        void TransferreItem(IGadget item, IGadgetsCollection toCollection);
    }

    class GadgetsCollection : IGadgetsCollection
    {
        public List<WeaponGadget.AbsWeaponGadget2> Weapons; //list med igadget
        public List<IGadget> Other;
        public GoodsCollection Goods;
        protected Dictionary<GoodsType, int> items;

        public GadgetsCollection()
        {
            Weapons = new List<Gadgets.WeaponGadget.AbsWeaponGadget2>();
            Goods = new GoodsCollection();
            Other = new List<IGadget> { };
            items = new Dictionary<GoodsType, int>();
        }

        public int Weight()
        {
            int result = Weapons.Count * LootfestLib.WeaponWeight + Goods.Weight();
            foreach (KeyValuePair<GoodsType, int> kv in items)
            {
                if (kv.Key != GoodsType.Coins)
                    result += kv.Value * LootfestLib.GoodsWeight;
            }
            foreach (IGadget g in Other)
            {
                result += g.Weight;
            }
            return result;
        }

        public List<Item> GetItemList()
        {
            List<Item> result = new List<Item>();
            foreach (KeyValuePair<GoodsType, int> kv in items)
            {
                result.Add(new Item(kv.Key, kv.Value));
            }
            return result;
        }

        public bool Empty
        {
            get
            {
                return Weapons.Count == 0 && Other.Count == 0 && items.Count == 0 && Goods.Empty;
            }
        }

        public void Clear()
        {
            Weapons.Clear();
            Other.Clear();
            Goods = new GoodsCollection();
            items.Clear();
        }
        public bool Pay(int coins)
        {
            bool canPay = RemoveItem(new Item(GoodsType.Coins, coins));
            if (canPay)
                Music.SoundManager.PlayFlatSound(LoadedSound.buy);
            return canPay;
        }
        public int Money
        {
            get
            {
                return GetItemAmount(GoodsType.Coins);
            }
            set
            {
                if (items.ContainsKey(GoodsType.Coins))
                {
                    items[GoodsType.Coins] = (ushort)value;
                }
                else
                {
                    items.Add(GoodsType.Coins, (ushort)value);
                }
            }
        }

        public int GetItemAmount(GoodsType type)
        {
            if (items.ContainsKey(type))
            {
                return items[type];
            }
            return 0;
        }

        public void AddMoney(int amount)
        {
            AddItem(new Item(GoodsType.Coins, amount));
        }

        public void AddItem(GadgetsCollection collection)
        {
            Weapons.AddRange(collection.Weapons);
            Other.AddRange(collection.Other);
            Goods.AddCollection(collection.Goods);
            List<Item> items = collection.GetItemList();
            foreach (Item i in items)
            {
                AddItem(i);
            }

            foreach (WeaponGadget.AbsWeaponGadget2 wep in collection.Weapons)
            {
                equipableAddedEvent(wep);
            }
            foreach (IGadget g in collection.Other)
            {
                if (g.EquipAble)
                {
                    equipableAddedEvent(g);
                }
            }

        }

        public void AutoSortGadgets()
        {
            if (Weapons.Count > 2)
            {
                Data.Gadgets.BluePrint[] sortOrder = 
                {
                    Data.Gadgets.BluePrint.Stick,
                     Data.Gadgets.BluePrint.WoodSword,

                     Data.Gadgets.BluePrint.Axe,
                     Data.Gadgets.BluePrint.LongAxe,
                     Data.Gadgets.BluePrint.Sword,
                     Data.Gadgets.BluePrint.LongSword,
                     Data.Gadgets.BluePrint.Dagger,
                     Data.Gadgets.BluePrint.Spear,
                     Data.Gadgets.BluePrint.Sickle,
                     Data.Gadgets.BluePrint.PickAxe,

                    Data.Gadgets.BluePrint.EnchantedWoodSword,
                    Data.Gadgets.BluePrint.EnchantedAxe,
                    Data.Gadgets.BluePrint.EnchantedSword,
                    Data.Gadgets.BluePrint.EnchantedLongSword,

                    Data.Gadgets.BluePrint.Sling,
                    Data.Gadgets.BluePrint.ShortBow,
                    Data.Gadgets.BluePrint.LongBow,
                    Data.Gadgets.BluePrint.MetalBow,
                    Data.Gadgets.BluePrint.EnchantedLongbow,
                    Data.Gadgets.BluePrint.EnchantedMetalbow,
                };
                List<Gadgets.WeaponGadget.AbsWeaponGadget2> sortedWeapons = new List<WeaponGadget.AbsWeaponGadget2>(Weapons.Count);
                foreach (Data.Gadgets.BluePrint bp in sortOrder)//for (Data.Gadgets.BluePrint type = (Data.Gadgets.BluePrint)0; type < Data.Gadgets.BluePrint.Gambison; type++)
                {
                    foreach (Gadgets.WeaponGadget.AbsWeaponGadget2 wep in Weapons)
                    {
                        if (wep.Type == bp)
                        {
                            sortedWeapons.Add(wep);
                        }
                    }
                }

                if (sortedWeapons.Count != Weapons.Count)
                {
                    throw new Exception("weapon sorting fail");
                }

                Weapons = sortedWeapons;
            }

            if (Other.Count > 2)
            {
                List<Gadgets.IGadget> sortedOther = new List<IGadget>(Other.Count);
                GadgetType[] sortOrder = { GadgetType.Shield, GadgetType.Armor, GadgetType.Jevelery };
                foreach (GadgetType iType in sortOrder)
                {
                    foreach (Gadgets.IGadget gadget in Other)
                    {
                        if (gadget.GadgetType == iType)
                        {
                            sortedOther.Add(gadget);
                        }
                    }
                }

                if (sortedOther.Count != Other.Count)
                {
                    throw new Exception("Other items sorting fail");
                }

                Other = sortedOther;
        
            }
        }

        virtual public void AddItem(IGadget gadget)
        {
            switch (gadget.GadgetType)
            {
                case GadgetType.Goods:
                    Goods.AddItem(gadget);
                    break;
                case GadgetType.Weapon:
                    Weapons.Add((Gadgets.WeaponGadget.AbsWeaponGadget2)gadget);
                    equipableAddedEvent(gadget);
                    break;
                case GadgetType.Item:
                    AddItem((Item)gadget);
                    break;
                case GadgetType.GadgetList:
                    GadgetList list = (GadgetList)gadget;
                    foreach (IGadget g in list.Gadgets)
                    {
                        this.AddItem(g);
                    }
                    break;
                default:
                    Other.Add(gadget);
                    if (gadget.EquipAble)
                    {
                        equipableAddedEvent(gadget);
                    }
                    break;
            }
        }

        virtual protected void AddItem(Item item)
        {
            if (items.ContainsKey(item.Type))
            {
                items[item.Type] += item.Amount;
            }
            else
            {
                items.Add(item.Type, item.Amount);
            }

            if (item.EquipAble)
            {
                equipableAddedEvent(item);
            }
        }

        //static readonly HUD.Link Dialogue = new HUD.Link((int)Players.Link.SelectItem_dialogue);

        public bool GotHealing()
        {
            foreach (FoodHealingPower food in LootfestLib.EatingOrderAndHealthRestore)
            {
                if (Goods.GotItemAnyQuality(food.Goods.Type, 1))
                    return true;
            }
            return false;
        }
        
        public bool GotItemForCrafting(GoodsType type, int amount)
        {
            if (type < GoodsType.END_GOODS)
            {
                return Goods.GotItemAnyQuality(type, amount);
            }
            else
            {
                return GetItemAmount(type) >= amount;
            }
        }

        /// <returns>Health Percent Add</returns>
        public FoodHealingPower EatHealing(float currentPercentHealth, bool isAppleLover, bool hobbitSkill)
        {
            
            FoodHealingPower bestHeal = new FoodHealingPower(GoodsType.NONE, Quality.NUM, 0);
            float bestScore = float.MinValue;


            for (int i = 0;  i < LootfestLib.EatingOrderAndHealthRestore.Length; i++)
            {
                FoodHealingPower current = LootfestLib.EatingOrderAndHealthRestore[i];
                for (Gadgets.Quality q = (Quality)0; q < Quality.NUM; q++)
                {
                   // type.Quality = q;
                    current.Goods.Quality = q;
                    if (Goods.GotItem(current.Goods))
                    {
                        
                        
                        current.BasicHealing = current.Healing(isAppleLover, hobbitSkill) * LootfestLib.FoodQualityHealAdd[(int)q];
                        //gice the healing a score
                            //-get score for being close to perfect full healing
                            //-get score for being higher prio
                        float healingScore = -Math.Abs(current.BasicHealing + currentPercentHealth - 1) - i * 0.05f;

                        if (healingScore > bestScore)
                        {
                            bestScore = healingScore;
                            bestHeal = current;
                        }
                    }
                }
            }

            if (bestHeal.BasicHealing > 0)
            {
                Goods.TryRemove(bestHeal.Goods);
            }
            return bestHeal;
        }

#if !CMODE

        
        public File ToMenu(File file, MenuFilter filter)
        {
            return this.ToMenu(file, new GadgetLink(null, this, null, null), filter, null);
        }
        public File ToMenu(File file, GadgetLink link, MenuFilter filter)
        {
            return this.ToMenu(file, link, filter, null);
        }
        public File ToMenu(File file, GadgetLink link, MenuFilter filter, Players.PlayerProgress progress)
        {
            //GadgetLink link = new GadgetLink(linkEvent, this, null);
            link.Coll = this;

            //link: 1: Item type, 2: Index, 3: UnderIndex
            File equipped = new File();
            File backPack = file;
            file = new File();

            int currentIndex = 0;
            if (filter == MenuFilter.All || 
                filter == MenuFilter.ButtonEquipable ||  
                filter == MenuFilter.MetalScrap ||  
                filter == MenuFilter.Enchant || 
                filter == MenuFilter.BackpackMenu)
            {
                if (Weapons.Count > 0)
                {
                   // link.Value1 = (int)GadgetType.Weapon;
                    for (int i = 0; i < Weapons.Count; i++)
                    {
                        bool view = true;
                        if (filter == MenuFilter.MetalScrap)  view = Weapons[i].Scrappable;
                        else if (filter == MenuFilter.Enchant)  view = Weapons[i].CanBeEnchanted();

                        if (view)
                        {
                            if (progress != null && progress.GadgetIsEquipped(Weapons[i]))
                            {
                                file = equipped;
                            }
                            else
                            {
                                file = backPack;
                            }
                            file.Add(new GadgetButtonData(link, Weapons[i], SpriteName.LFMenuItemBackground, true));
                        }
                    }
                }
            }
            currentIndex = Weapons.Count;
            
            for (int i = 0; i < Other.Count; i++)
            {
                
                if (
                    filter == MenuFilter.All || filter == MenuFilter.BackpackMenu ||
                    (filter == MenuFilter.MetalScrap && Other[i].Scrappable) ||
                    (filter == MenuFilter.Shields && Other[i].GadgetType == GadgetType.Shield) ||
                    (filter == MenuFilter.Armor && Other[i].GadgetType == GadgetType.Armor && !((Armor)Other[i]).Helmet) ||
                    (filter == MenuFilter.Helmet && Other[i].GadgetType == GadgetType.Armor && ((Armor)Other[i]).Helmet) ||
                    (filter == MenuFilter.Rings && Other[i].GadgetType == GadgetType.Jevelery)
                    )
                {
                    if (progress != null && progress.GadgetIsEquipped(Other[i]))
                    {
                        file = equipped;
                    }
                    else
                    {
                        file = backPack;
                    }
                    file.Add(new GadgetButtonData(link, Other[i], SpriteName.LFMenuItemBackground, true));
                }
            }

            file = backPack;
            if (filter == MenuFilter.All || filter == MenuFilter.ButtonEquipable || filter ==MenuFilter.BackpackMenu)
            {
                currentIndex = Weapons.Count + Other.Count;
               // GoodsType[] itemTypes = items.Keys.ToArray();
                foreach (KeyValuePair<GoodsType, int> kv in items)
                {
                    Item item = new Item(kv.Key, kv.Value);
                    if (filter == MenuFilter.All || filter== MenuFilter.BackpackMenu || item.EquipAble)
                    {
                        //link.Value4 = currentIndex + i;
                        file.Add(new GadgetButtonData(link, item, SpriteName.LFMenuItemBackground, true));
                    }
                }
            }

            if (filter == MenuFilter.All || filter == MenuFilter.BackpackMenu)
            {
                currentIndex = Weapons.Count + Other.Count + items.Count;
                Goods.ToMenu(file, currentIndex, link);
            }

            if (!equipped.Empty)
            {
                File equipTitle = new File();
                if (filter != MenuFilter.BackpackMenu)
                {
                    if (!equipped.Empty)
                    {
                        equipTitle.AddDescription("Equipped");
                        equipTitle.Combine(equipped);
                    }
                    if (!backPack.Empty) equipTitle.AddDescription("Backpack");
                   
                }
                equipTitle.Combine(backPack);

                return equipTitle;
            }
            return file;
        }

        public List<IGadget> ToList()
        {
            List<IGadget> result = new List<IGadget>();
            foreach (WeaponGadget.AbsWeaponGadget2 wep in Weapons)
                result.Add(wep);
            //result.AddRange(Weapons);
            result.AddRange(Other);
            foreach (KeyValuePair<GoodsType, int> kv in items)
            {
                result.Add(new Item(kv.Key, kv.Value));
            }
            result.AddRange(Goods.ToGadgetList());
            return result;
        }

        public void ListButtonEquipable(ref File file, GadgetLink linkEvent, Players.PlayerProgress progress)
        {
            file = ToMenu(file, linkEvent, MenuFilter.ButtonEquipable, progress);
             
        }
#endif
        public GameObjects.Gadgets.IGadget PickButtonEquipable(FourBytes setupIx_buttonIx_type_typeIndex)
        {
            return new Item(items.Keys.ToArray()[setupIx_buttonIx_type_typeIndex.Get(3)]);
        }

        public void WriteGadgetIndex(System.IO.BinaryWriter w, GameObjects.Gadgets.IGadget gadget)
        {
            //w.Write(GetIndexFromItem(gadget));
            int? index = GetIndexFromItem(gadget);
            if (index == null)
            {
                Debug.LogError( "writing gadget, " + gadget.ToString());
                DataStream.DataStreamLib.WriteGrowingAddValue(w, 0);
            }
            else
            {
                DataStream.DataStreamLib.WriteGrowingAddValue(w, index.Value);
            }
        }
        public GameObjects.Gadgets.IGadget ReadGadgetIndex(System.IO.BinaryReader r)
        {
            return PickItemFromIndex(DataStream.DataStreamLib.ReadGrowingAddValue(r));
        }

        public int? GetIndexFromItem(IGadget item)
        {
            if (item == null)
                return null;
            if (item.GadgetType == GadgetType.Weapon)
            {
                for (int i = 0; i < Weapons.Count; i++)
                {
                    if (Weapons[i] == item)
                        return i;
                }
            }
            else
            {
                for (int i = 0; i < Other.Count; i++)
                {
                    if (Other[i] == item)
                        return (i + Weapons.Count);
                }
            }

            return null;
        }


        public IGadget PickItemFromHashTag(ushort hashtag, int amount, bool remove)
        {
            GadgetType type = GadgetLib.HashToGadgetType(hashtag);

            switch (type)
            {
                case GadgetType.Goods:
                    return Goods.PickItemFromHashTag(hashtag, amount, remove);
                case GadgetType.Item:
                    Item selectedItem = Item.NoItem;
                    foreach (KeyValuePair<GoodsType, int> kv in items)
                    {
                        Item item = new Item(kv.Key, amount);//kv.Value);
                        if (item.ItemHashTag == hashtag && amount <= kv.Value)
                        {
                            selectedItem = item;
                            break;
                        }
                    }
                    if (selectedItem.Amount > 0)
                    {
                        if (remove)
                        {
                            RemoveItem(selectedItem);
                            //items.Remove(selectedItem.Type);
                        }
                        return selectedItem;
                    }
                    break;
                case GadgetType.Weapon:
                    for (int i = 0; i < Weapons.Count; i++ )
                    {
                        if (Weapons[i].ItemHashTag == hashtag)
                        {
                            Gadgets.WeaponGadget.AbsWeaponGadget2 wep = Weapons[i];
                            if (remove)
                            {
                                equipableRemovedEvent(wep);
                                Weapons.RemoveAt(i);
                            }
                            return wep;
                        }
                    }
                    break;
                default:
                    for (int i = 0; i < Other.Count; i++)
                    {
                        if (Other[i].ItemHashTag == hashtag)
                        {
                            IGadget gadget = Other[i];
                            if (remove)
                            {
                                if (Other[i].EquipAble)
                                    equipableRemovedEvent(gadget);
                                Other.RemoveAt(i);
                            }
                            return gadget;
                        }
                    }
                    break;
            }

            return null;

        }

        public IGadget PickItemFromIndex(int index, int amount, bool transferre, IGadgetsCollection toCollection)
        {
            IGadget item = PickItemFromIndex(index, amount, transferre);
            if (item != null && transferre)
            {
                toCollection.AddItem(item);
            }
            return item;
        }

        public IGadget PickItemFromMenu(HUD.Link link)
        {
            return PickItemFromMenu(link, int.MaxValue, false);
        }

        public IGadget PickItemFromMenu(HUD.Link link, int amount, bool remove)
        {
            //link: 1: Item type, 2: Index, 3: UnderIndex
            switch ((GadgetType)link.Value1)
            {
                case GadgetType.Goods:
                    return Goods.PickItemFromMenu(link, amount, remove);
                case GadgetType.Item:
                    GoodsType itype = (GoodsType)link.Value2;
                    Item result = new Item(itype, items[itype]);
                    result.Amount = lib.SmallestOfTwoValues(amount, result.Amount);

                    if (remove)
                    {
                        RemoveItem(result);
                    }
                    return result;
                case GadgetType.Weapon:
                    WeaponGadget.AbsWeaponGadget2 wep = Weapons[link.Value2];
                    if (remove)
                    {
                        equipableRemovedEvent(wep);
                        Weapons.RemoveAt(link.Value2);
                    }
                    return wep;
                default:
                    IGadget other = Other[link.Value2];
                    if (remove)
                    {
                        if (other.EquipAble)
                            equipableRemovedEvent(other);
                        Other.RemoveAt(link.Value2);
                    }
                    return other;
             }

        }


        public IGadget PickItemFromIndex(int index)
        {
            if (index == byte.MaxValue)
                return null;
            return PickItemFromIndex(index, int.MaxValue, false);
        }

        public IGadget PickItemFromIndex(int index, int amount, bool remove)
        {
            if (index < Weapons.Count)
            {
                IGadget result = Weapons[index];
                if (remove)
                {
                    equipableRemovedEvent(result);
                    Weapons.RemoveAt(index);
                }
                return result;
            }
            index -= Weapons.Count;
            if (index < Other.Count)
            {
                IGadget result = Other[index];
                
                if (remove)
                {
                    if (Other[index].EquipAble)
                        equipableRemovedEvent(result);
                    Other.RemoveAt(index);
                }
                return result;
            }
            index -= Other.Count;
            if (index < items.Count)
            {
                GoodsType itype = items.Keys.ToArray()[index];
                Item result = new Item(itype, items[itype]);
                result.Amount = lib.SmallestOfTwoValues(amount, result.Amount);

                if (remove)
                {
                    RemoveItem(result);
                }
                return result;
            }
            index -= items.Count;
            return Goods.PickItemFromIndex(index, amount, remove);
        }


        public bool RemoveItem(IGadget item)
        {
            bool hasItem = false;

            switch (item.GadgetType)
            {
                case GadgetType.Goods:
                    Gadgets.Goods goods = (Gadgets.Goods)item;
                    //goods.Amount = amount;
                   hasItem = Goods.TryRemove(goods);
                   break;
                case GadgetType.Item:
                    Item i = (Item)item;
                    //i.Amount = amount;
                    hasItem = RemoveItem(i);
                    break;
                case GadgetType.Weapon:
                    Gadgets.WeaponGadget.AbsWeaponGadget2 wep = (Gadgets.WeaponGadget.AbsWeaponGadget2)item;
                    if (Weapons.Contains(wep))
                    {
                        Weapons.Remove(wep);
                        hasItem =  true;
                    }
                   break;
                default:
                    if (Other.Contains(item))
                    {
                        Other.Remove(item);
                        hasItem = true;
                    }
                   break;
            }
            if (item.EquipAble)
                equipableRemovedEvent(item);
            return hasItem;
        }

        public void TransferreItem(IGadget item, IGadgetsCollection toCollection)
        {
            //item.StackAmount = lib.SmallestOfTwoValues(item.StackAmount,);
            if (RemoveItem(item))
            {
                toCollection.AddItem(item);
            }
        }

        public bool HasEnchantableItems()
        {
            foreach (WeaponGadget.AbsWeaponGadget2 g in Weapons)
            {
                if (g.CanBeEnchanted()) return true;
            }
            return false;
        }
        //public bool RemoveItem(IGadget item, int amount, bool transferre, IGadgetsCollection toCollection)
        //{
        //    bool result = RemoveItem(item, amount);
        //    if (result && transferre)
        //    {
        //        toCollection.AddItem(item);
        //    }
        //    return result;
        //}

        public bool RemoveItem(GoodsType item)
        {
            return this.RemoveItem(new Item(item));
        }
        virtual public bool RemoveItem(Item item)
        {
            if (items.ContainsKey(item.Type))
            {
                if (items[item.Type] >= item.Amount)
                {
                    items[item.Type] -= item.Amount;
                    if (items[item.Type] <= 0)
                    {
                        items.Remove(item.Type);
                    }
                    return true;
                }
            }
            return false;
        }


        public void WriteStream(System.IO.BinaryWriter w)
        {
            
            w.Write(GadgetLib.SaveVersion);
            Goods.WriteStream(w);//15
            //5
            DataStream.DataStreamLib.WriteGrowingAddValue(w, Weapons.Count);
            foreach (Gadgets.WeaponGadget.AbsWeaponGadget2 wep in Weapons)
            {
                GadgetLib.WriteWeaponGadget(wep, w);
            }
            //6
            DataStream.DataStreamLib.WriteGrowingAddValue(w, Other.Count);
            foreach (IGadget o in Other)
            {
                w.Write((byte)o.GadgetType);
                o.WriteStream(w);
            }
            //7
            w.Write((byte)items.Count);
            foreach (KeyValuePair<GoodsType, int> kv in items)
            {
                //8
                w.Write((byte)kv.Key);
                //9
                DataStream.DataStreamLib.WriteGrowingAddValue(w, kv.Value);
                //10
            }
  
        }
        public void ReadStream(System.IO.BinaryReader r)
        {
            Weapons.Clear();

            byte version = r.ReadByte();
            Goods.ReadStream(r, version);
            //5
            int numWeapons = DataStream.DataStreamLib.ReadGrowingAddValue(r);
            //6
            for (int i = 0; i < numWeapons; i++)
            {
                Weapons.Add(GadgetLib.ReadWeaponGadget(r));
                
            }

            int numGadgets = DataStream.DataStreamLib.ReadGrowingAddValue(r);
            //7
            for (int i = 0; i < numGadgets; i++)
            {
                Other.Add(GadgetLib.ReadGadget(r));
               
            }

            items.Clear();
            int numItems = r.ReadByte();
            //8
            for (int i = 0; i < numItems; i++)
            {
                GoodsType type = (GoodsType)r.ReadByte();
                int amount = DataStream.DataStreamLib.ReadGrowingAddValue(r);
                items.Add(type, amount);
            }
        }

        public IconAndText MenuResourceDesc(GoodsType type)
        {
            if (type < GoodsType.ITEM_START)
            {
                throw new NotImplementedException();
            }
            else
            {
               return new IconAndText(GadgetLib.GadgetIcon(type), ":" + GetItemAmount(type));
            }
        }

        //virtual protected void weaponRemovedEvent(IGadget weapon)
        //{ }
        virtual protected void equipableRemovedEvent(IGadget gadget)
        { }
        virtual protected void equipableAddedEvent(IGadget gadget)
        { }

        public bool HasEnchantingGoods()
        {
            foreach (GoodsType g in Magic.MagicLib.GemTypes)
            {
                if (Goods.GotItemAnyQuality(g, 1))
                    return true;
            }
            return false;
        }
        public bool HasItemThatCanBeBlessed()
        {
            foreach (Gadgets.WeaponGadget.AbsWeaponGadget2 wep in Weapons)
            {
                if (!wep.Blessed) return true;
            }
            return false;
        }
    }

    struct FoodHealingPower
    {
        public Goods Goods;
        float healing;
        public float BasicHealing
        {
            get
            {
                return healing;
            }
            set
            {
                healing = value;
            }
        }
        public float Healing(bool appleLover, bool hobbitSkill)
        {
            float result = healing;
            if (appleLover && (Goods.Type == GoodsType.Apple || Goods.Type == GoodsType.Grilled_apple || Goods.Type == GoodsType.Apple_pie))
            {
                result *= LootfestLib.AppleLoverMulti;
            }
            if (hobbitSkill)
            {
                result *= LootfestLib.HobbitSkillMulti;
            }

            return result;
        }
        public FoodHealingPower(GoodsType type, Quality quality, float healing)
        {
            Goods = new Gadgets.Goods(type, quality, 1);
            this.healing = healing;
        }
    }

    enum MenuFilter
    {
        All,
        ButtonEquipable,
        BackpackMenu,
        Helmet,
        Armor,
        Rings,
        Shields,
        MetalScrap,
        Enchant,
    }
    

    enum GadgetType
    {
        Goods,
        Weapon,
        Shield,
        Armor,
        Jevelery,
        Item,
        GadgetList,
        NON
    }
}
