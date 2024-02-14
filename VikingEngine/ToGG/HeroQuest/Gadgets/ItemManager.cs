using Microsoft.Xna.Framework;
using System.Collections.Generic;
using VikingEngine.ToGG.HeroQuest.Display;
using VikingEngine.ToGG.HeroQuest.Players;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class ItemManager
    {
        public ItemManager()
        {
            hqRef.items = this;
        }

        public void quickDragNdropItem(BackPackMenu menu, ItemSlot fromSlot, SlotType moveTo)
        {            
            if (moveTo == SlotType.Quickbelt)
            {
                if (menu.backpack.equipment.quickbelt.IsFull())
                {
                    return;
                }
            }
            
            var item = beginDragItem(fromSlot);
            item.itemImage.Visible = false;
            if (fromSlot.slotType == SlotType.OnGround)
            {
                removeFromGround(item, menu.player.ActiveUnitPos);
            }

            bool refreshBackpack = false;
            var equipSlotArray = menu.backpack.equipment.slot(moveTo);
            ItemSlot equipSlot = null;

            if (equipSlotArray != null)
            {
                equipSlot = equipSlotArray.QuickEquipSlot();

                if (equipSlot.HasItem)
                {
                    refreshBackpack = quickReplaceItem(equipSlot, fromSlot);
                }
            }

            switch (moveTo)
            {
                case SlotType.Backpack:
                    menu.backpack.add(item);
                    refreshBackpack = true;
                    break;
                case SlotType.OnGround:
                    hqRef.items.moveToGround(item, menu.player.ActiveUnitPos);
                    menu.refreshGround();
                    break;

                case SlotType.Quickbelt:
                    menu.backpack.equipment.quickbelt.addItem(item);
                    menu.refreshQuickBelt();
                    break;

                default:
                    if (equipSlot != null)
                    {
                        equipSlot.addItem(item);
                    }
                    else
                    {
                        throw new System.NotImplementedException();
                    }
                    break;
            }

            if (refreshBackpack)
            {
                menu.refreshBackpack();
            }
           
            onBackPackAccess(menu.player, fromSlot.slotType, moveTo);

            bool quickReplaceItem(ItemSlot dropSlot, ItemSlot movedItemPrevSlot)
            {
                AbsItem swapItem = beginDragItem(dropSlot);
                if (swapItem != null)
                {
                    menu.backpack.add(swapItem);
                    return true;
                }
                return false;
            }
        }

        void onBackPackAccess(LocalPlayer player, SlotType from, SlotType to)
        {
            player.onBackPackAccess(EquipSlots.IsWearEquipSlot(from) || EquipSlots.IsWearEquipSlot(to));

            if (from == SlotType.OnGround || to == SlotType.OnGround)
            {
                TileItemCollection.NetWrite(player.ActiveUnitPos);
            }
        }

        public AbsItem beginDragItem(ItemSlot fromSlot)
        {
            if (fromSlot.item != null)
            {
                var item = fromSlot.pullItem();
                return item;
            }
            return null;
        }

        public void beginDropItem(BackPackMenu menu, AbsItem item, ItemSlot fromSlot, ItemSlot toSlot)
        {
            bool transfere = fromSlot.slotType == SlotType.OnGround ||
                toSlot.slotType == SlotType.OnGround ||
                toSlot.slotType == SlotType.SendToPlayer;

            if (item.count > 1 &&
               fromSlot.slotType != toSlot.slotType &&
               transfere)
            {
                menu.countDialogue = new ItemCountDialogue(menu, item, fromSlot, toSlot);
                toSlot.button?.emptyUpdate();
            }
            else
            {
                dropItem(menu, item, fromSlot, toSlot, item.count);
            }
        }

        public void dropItem(BackPackMenu menu, AbsItem item, ItemSlot fromSlot, ItemSlot toSlot, int dropCount)
        {
            int keep = item.count - dropCount;

            if (keep > 0)
            {
                AbsItem split = item.Clone();
                split.count = keep;

                returnItem(split, fromSlot, true);
            }

            AbsItem swapItem = beginDragItem(toSlot);

            item.count = dropCount;

            if (toSlot.slotType == SlotType.SendToPlayer)
            {
                netWriteSendItem(item, dropCount, ((SendToPlayerSlot)toSlot).player);
                item.DeleteImage();
            }
            else
            {
                toSlot.addItem(item);
            }

            if (fromSlot.slotType != toSlot.slotType)
            {
                if (toSlot.slotType == SlotType.OnGround)
                {
                    moveToGround(item, menu.player.ActiveUnitPos);
                }
                else if (fromSlot.slotType == SlotType.OnGround)
                {
                    removeFromGround(item, menu.player.ActiveUnitPos);
                }
            }

            if (swapItem != null)
            {
                if (item.Combine(swapItem))
                {
                    if (swapItem.count == 0)
                    {
                        swapItem = null;
                    }
                    toSlot.button.refreshItem();
                }
            }

            if (swapItem != null)
            {
                menu.itemDrag.onItemSwap(swapItem, toSlot);
            }
            else
            {
                menu.EndDragItem();
            }

            onBackPackAccess(menu.player, fromSlot.slotType, toSlot.slotType);
        }

        void netWriteSendItem(AbsItem item, int dropCount, Players.AbsHQPlayer toPlayer)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqSendItem, 
                Network.SendPacketTo.OneSpecific,
                toPlayer.pData.netPeer().fullId, 
                Network.PacketReliability.Reliable, null);
            w.Write((byte)1);
            item.writeItem(w, dropCount);
        }

        void netWriteSendItems(List<AbsItem> items, Players.AbsHQPlayer toPlayer)
        {
            if (items.Count > 0)
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqSendItem,
                    Network.SendPacketTo.OneSpecific,
                    toPlayer.pData.netPeer().fullId,
                    Network.PacketReliability.Reliable, null);
                w.Write((byte)items.Count);
                foreach (var m in items)
                {
                    m.writeItem(w, null);
                }
            }
        }

        public void netReadSendItem(Network.ReceivedPacket packet)
        {
            byte listCount = packet.r.ReadByte();
            
            for (int i = 0; i < listCount; ++i)
            {
                var item = Gadgets.AbsItem.ReadItem(packet.r);
                hqRef.players.localHost.recieveItemFromPlayer(item);
            }
        }

        public void returnItem(AbsItem item, ItemSlot toSlot, bool removalHasBeenShared)
        {
            toSlot.addItem(item);

            if (removalHasBeenShared)
            {
                if (toSlot.slotType == SlotType.OnGround)
                {
                    hqRef.items.moveToGround(item, item.placement);
                }
            }
        }

        public void moveToGround(AbsItem item, IntVector2 pos)
        {
            var coll = groundCollection(pos, true);
            coll.Add(item);
            item.placement = pos;
            //TODO net share
        }

        public void removeFromGround(AbsItem item, IntVector2 pos)
        {
            var coll = groundCollection(pos, false);
            if (coll != null)
            {
                coll.items.loopBegin();
                while(coll.items.loopNext())
                {
                    if (coll.items.sel.id == item.id)
                    {
                        //TODO net request

                        //Found it!
                        coll.items.loopRemove();
                        removeIfEmpty(coll);
                        return;
                    }
                }
            }

            throw new System.Exception("missing ground item: " + item.ToString());
        }

        void removeIfEmpty(TileItemCollection coll)
        {
            if (coll.items.Count == 0)
            {
                coll.DeleteMe();
            }
        }

        public void spawnChest(IntVector2 pos, LootLevel lvl)
        {
            var chest = groundCollection(pos, true);
            chest.setChest(lvl);

            TileItemCollection.NetWrite(pos);
        }

        public TileItemCollection groundCollection(IntVector2 pos, bool createIfMissing)
        {
            var sq = toggRef.board.tileGrid.Get(pos);
            var coll = sq.tileObjects.GetObject(TileObjectType.ItemCollection) as TileItemCollection;
            if (createIfMissing && coll == null)
            {
                coll = new TileItemCollection(pos, null);
                coll.onLoadComplete();
            }

            return coll;
        }

        public void interaction_takeAllItems(Unit unit, ItemCollection items)
        {
            var lc = unit.data.properties.Get(UnitPropertyType.LootCollector);
            if (lc != null)
            {
                ((Data.LootCollector)lc).add(items);
            }
            else
            {
                if (unit.PlayerHQ is RemotePlayer)
                {
                    netWriteSendItems(items, unit.PlayerHQ);
                }
                else
                {
                    foreach (var m in items)
                    {
                        unit.PlayerHQ.lootItem(m);
                    }
                }
            }
            items.Clear();
        }
    }
}
