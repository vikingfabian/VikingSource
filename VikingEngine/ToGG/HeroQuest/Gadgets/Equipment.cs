using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class Equipment
    {
        public static readonly SlotType[] EquipPrioOrder = new SlotType[]
        {
            SlotType.EquipMainHand,
            SlotType.EquipBody,
            SlotType.EquipSecondHand,
            SlotType.EquipTrinket,
            SlotType.Quickbelt,
        };

        public ItemSlot mainhand, secondHand, body, trinket1, trinket2;
        public SlotArray mainhandArray, secondHandArray, bodyArray, trinketArray;
        public List2<ItemSlot> list;

        public QuickBelt quickbelt;

        public Equipment(AbsHQPlayer player)
        {
            mainhand = new ItemSlot(SlotType.EquipMainHand);
            secondHand = new ItemSlot(SlotType.EquipSecondHand);
            body = new ItemSlot(SlotType.EquipBody);
            trinket1 = new ItemSlot(SlotType.EquipTrinket);
            trinket2 = new ItemSlot(SlotType.EquipTrinket);

            mainhandArray = new SlotArray() { mainhand };
            secondHandArray = new SlotArray() { secondHand };
            bodyArray = new SlotArray() { body };
            trinketArray = new SlotArray() { trinket1, trinket2 };

            list = new List2<ItemSlot>
            {
                mainhand, secondHand, body, trinket1, trinket2
            };

            quickbelt = new QuickBelt();
        }

        public void write(System.IO.BinaryWriter w)
        {
            EightBit slotsUsed = new EightBit();
            for (int i = 0; i < list.Count; ++i)
            {
                slotsUsed.Set(i, list[i].HasItem);
            }

            slotsUsed.write(w);

            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i].HasItem)
                {
                    list[i].item.type.write(w);
                }
            }
        }

        public void read(System.IO.BinaryReader r)
        {
            EightBit slotsUsed = new EightBit();
            slotsUsed.read(r);

            for (int i = 0; i < list.Count; ++i)
            {
                if (slotsUsed.Get(i))
                {
                    ItemFilter type = new ItemFilter(r);

                    if (list[i].Contains(type) == false)
                    {
                        var item = AbsItem.createItem(type);
                        list[i].addItem(item);
                    }
                }
                else
                {
                    list[i].clear();
                }
            }
        }

        public AbsItem equip(AbsItem item, SlotType slotType, int slotIx)
        {
            ItemSlot itemSlot = slot(slotType, slotIx);
            AbsItem replaced = itemSlot.pullItem();
            itemSlot.addItem(item);

            return replaced;
        }

        public bool equip(AbsItem item)
        {
            bool result = false;
                        
            var slots = item.Equip.slots;
            if (slots != null)
            {
                foreach (var m in slots)
                {                    
                    switch (m)
                    {
                        case SlotType.EquipMainHand:
                            result = tryEquip(item, mainhand);
                            break;

                        case SlotType.EquipSecondHand:
                            result = tryEquip(item, secondHand);
                            break;

                        case SlotType.EquipBody:
                            result = tryEquip(item, body);
                            break;

                        case SlotType.EquipTrinket:
                            if (tryEquip(item, trinket1))
                            {
                                result = true;
                            }
                            else
                            {
                                result = tryEquip(item, trinket2);
                            }
                            break;

                        case SlotType.Quickbelt:
                            result = quickbelt.addItem(item);
                            break;
                    }

                    if (result)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        bool tryEquip(AbsItem item, ItemSlot toSlot)
        {
            if (toSlot.item == null)
            {
                toSlot.item = item;
                return true;
            }

            return false;
        }

        public int hasEquipment(ItemFilter itemFilter)
        {
            int count = 0;

            foreach (var m in list)
            {
                if (m.item != null && m.item.type == itemFilter)
                {
                    count++;
                }
            }

            return count;
        }

        public bool RemoveItem(ItemFilter itemFilter, int count)
        {
            return quickbelt.RemoveItem(itemFilter, count);
        }

        public WeaponStats weaponStatsMerged()
        {
            WeaponStats stats = new WeaponStats();
            stats.DefaultWeaponSetup();

            stats.Merge(secondHand.item, SlotType.EquipSecondHand);
            stats.Merge(mainhand.item, SlotType.EquipMainHand);

            return stats;
        }

        public AbsWeapon mainWeapon(bool melee)
        {
            var stats = weaponStatsMerged();
            SlotType slottype = melee ? stats.meleeSlot : stats.projectileSlot;

            var s = slot(slottype);
            if (s != null)
            {
                return s.First.item as AbsWeapon;
            }

            return null;
        }
        public ItemSlot slot(SlotType type, int slotIx)
        {
            return slot(type)[slotIx];
        }
        public SlotArray slot(SlotType type)
        {
            switch (type)
            {
                case SlotType.EquipMainHand:
                    return mainhandArray;
                case SlotType.EquipSecondHand:
                    return secondHandArray;
                case SlotType.EquipBody:
                    return bodyArray;
                case SlotType.EquipTrinket:
                    return trinketArray;
                case SlotType.Quickbelt:
                    return quickbelt.slots;

                default:
                    return null;
            }
        }

        public int slotCount(SlotType type)
        {
            return slot(type).Count;
        }

        public SlotType quickEquipSlot(AbsItem item, bool findEmptySlot)
        {
            foreach (var m in EquipPrioOrder)
            {
                if (item.slotAccess(m))
                {
                    if (findEmptySlot)
                    {
                        var slotArray = slot(m);

                        foreach (var slot in slotArray)
                        {
                            if (slot.item == null)
                            {
                                return m;
                            }
                        }
                    }
                    else
                    {
                        return m;
                    }
                }
            }

            return SlotType.None;
        }

        public ItemSlot emptySlot(SlotType slotType)
        {
            var slotArray = slot(slotType);

            foreach (var slot in slotArray)
            {
                if (slot.item == null)
                {
                    return slot;
                }
            }

            return null;
        }
    }

    class SlotArray : List<ItemSlot>
    {
        public SlotArray()
            : base()
        { }

        public SlotArray(int cap)
            : base(cap)
        { }

        public ItemSlot First => this[0];

        public ItemSlot QuickEquipSlot()
        {
            foreach (var m in this)
            {
                if (m.item == null)
                {
                    return m;
                }
            }

            return First;
        }
    }

    struct EquipSlots
    {
        public static readonly EquipSlots None = new EquipSlots(null);
        public static readonly EquipSlots Quickbelt = new EquipSlots(new SlotType[] { SlotType.Quickbelt });
        public static readonly EquipSlots MainHand = new EquipSlots(new SlotType[] { SlotType.EquipMainHand });
        public static readonly EquipSlots SecondHand = new EquipSlots(new SlotType[] { SlotType.EquipSecondHand });
        public static readonly EquipSlots AnyHand = new EquipSlots(new SlotType[] { SlotType.EquipMainHand, SlotType.EquipSecondHand });
        public static readonly EquipSlots Body = new EquipSlots(new SlotType[] { SlotType.EquipBody });
        public static readonly EquipSlots Trinket = new EquipSlots(new SlotType[] { SlotType.EquipTrinket });

        public SlotType[] slots;

        public EquipSlots(SlotType[] slots)
        {
            this.slots = slots;
        }

        public static bool IsWearEquipSlot(SlotType slot)
        {
            return slot == SlotType.EquipMainHand ||
                slot == SlotType.EquipSecondHand ||
                slot == SlotType.EquipBody ||
                slot == SlotType.EquipTrinket;
        }

        public bool Contains(SlotType type)
        {
            if (slots != null)
            {
                if (type == SlotType.AnyWearingEquipment)
                {
                    foreach (var m in slots)
                    {
                        if (m != SlotType.Quickbelt)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return arraylib.Contains(slots, type);
                }
            }

            return false;   
        }
    }
}