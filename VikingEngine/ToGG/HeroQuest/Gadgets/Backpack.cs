using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class Backpack
    {
        ListWithSelection<BackPackPage> pages = new ListWithSelection<BackPackPage>(2);

        public Equipment equipment;

        public Backpack(AbsHQPlayer player)
        {
            equipment = new Equipment(player);
            pages.Add(new BackPackPage(), true);
        }

        public void NetShare()
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqShareEquipment, 
                Network.PacketReliability.Reliable);
            equipment.write(w);
        }

        public void add(AbsItem item, bool tryEquip = false)
        {
            item.OnPickUp();

            if (tryEquip && equipment.equip(item))
            {
                return;
            }

            var p = pages.list[0];
            p.slots.LoopBegin();
            while (p.slots.LoopNext())
            {
                var slotItem = p.slots.LoopValueGet().item;
                if (slotItem != null && slotItem.Combine(item))
                {
                    if (item.count == 0)
                    {
                        return;
                    }
                }
            }

            p.slots.LoopBegin();
            while (p.slots.LoopNext())
            {
                if (p.slots.LoopValueGet().item == null)
                {
                    p.slots.LoopValueGet().item = item;
                    return;
                }
            }

            throw new Exception();
        }

        public void equip(AbsItem item, SlotType slotType, int slotIx)
        {
            if (slotType == SlotType.Backpack)
            {
                add(item);
            }
            else
            {
                var replaced = equipment.equip(item, slotType, slotIx);
                if (replaced != null)
                {
                    add(replaced);
                }
            }
        }

        public void quickMoveEquip(ItemSlot fromSlot, out SlotType moveClick, out SlotType equipClick)
        {
            moveClick = fromSlot.quickMove();

            if (EquipSlots.IsWearEquipSlot(fromSlot.slotType))
            {
                equipClick = SlotType.None;
            }
            else
            {
                equipClick = equipment.quickEquipSlot(fromSlot.item, true);

                if (equipClick == SlotType.None)
                {
                    equipClick = equipment.quickEquipSlot(fromSlot.item, false);
                }
            }
        }


        public ItemCollection listItems()
        {
            ItemCollection coll = new ItemCollection();

            foreach (var pg in pages.list)
            {
                pg.slots.LoopBegin();
                while (pg.slots.LoopNext())
                {
                    var slot = pg.slots.LoopValueGet();
                    if (slot.item != null)
                    {
                        coll.Add(slot.item);
                    }
                }
            }

            return coll;
        }

        public ItemCollection listItems(AbsItem item, bool stack)
        {
            return listItems(item.type, stack);
        }

        public ItemCollection listItems(ItemFilter type, bool stack)
        {
            ItemCollection coll = new ItemCollection();

            for (int pageIx = 0; pageIx < pages.list.Count; ++pageIx)//each (var pg in pages.list)
            {
                var pg = pages.list[pageIx];
                pg.slots.LoopBegin();
                while (pg.slots.LoopNext())
                {
                    var slot = pg.slots.LoopValueGet();
                    if (slot.item != null)
                    {
                        if (slot.item.type == type)
                        {
                            slot.item.placementPage = pageIx;
                            slot.item.placement = pg.slots.LoopPosition;

                            if (stack)
                            {
                                coll.add_stackAlways(slot.item.Clone());
                            }
                            else
                            {
                                coll.Add(slot.item);
                            }
                        }
                    }
                }
            }

            return coll;
        }

        public bool RemoveItem(ItemFilter itemFilter, int count)
        {
            if (SpendItem(itemFilter, count))
            {
                return true;
            }
            else
            {
                return equipment.RemoveItem(itemFilter, count);
            }
        }

        public bool SpendItem(ItemFilter type, int count)
        {
            var available = listItems(type, false);

            int totalCount = 0;
            foreach (var m in available)
            {
                totalCount += m.count;
            }

            if (totalCount < count)
            {
                return false;
            }

            while (count > 0)
            {
                AbsItem smallestStack = null;
                foreach (var m in available)
                {
                    if (smallestStack == null ||
                        m.count <= smallestStack.count)
                    {
                        smallestStack = m;
                    }
                }

                if (smallestStack.count <= count)
                {
                    count -= smallestStack.count;
                    smallestStack.count = 0;
                }
                else
                {
                    smallestStack.count -= count;
                    count = 0;
                }

                if (smallestStack.count == 0)
                {   
                    pages.list[smallestStack.placementPage].slots.Get(smallestStack.placement).clear();
                }
            }

            return true;
        }

        public bool purchase(int cost)
        {
            return SpendItem(ItemFilter.Coins, cost);
        }

        public bool canPurchase(int cost)
        {
            var available = listItems(ItemFilter.Coins, true);
            return arraylib.HasMembers(available) && 
                available[0].count >= cost;
        }

        public BackPackPage Page
        {
            get { return pages.Selected(); }
        }
    }

    class BackPackPage
    {
        public static readonly IntVector2 SquareCount = new IntVector2(10, 8);

        public Grid2D<ItemSlot> slots;

        public BackPackPage()
        {
            slots = new Grid2D<ItemSlot>(SquareCount);
            slots.LoopBegin();
            while (slots.LoopNext())
            {
                slots.LoopValueSet(new ItemSlot(SlotType.Backpack));
            }
        }
    }
}
