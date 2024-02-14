using Microsoft.Xna.Framework;
using VikingEngine.ToGG.HeroQuest.Display;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class ItemSlot
    {
        public SlotType slotType;
        public ItemSlotButton button = null;
        public AbsItem item = null;

        public ItemSlot(SlotType type)
        {
            this.slotType = type;
        }

        public ItemSlot(SlotType type, VectorRect area, ImageLayers layer)
        {
            this.slotType = type;
            createButton(area, layer);
        }

        public void createButton(VectorRect area, ImageLayers layer)
        {
            if (button == null)
            {
                button = new ItemSlotButton(area, layer, this);
            }
        }

        public void removeButton()
        {
            button?.DeleteMe();
            item?.DeleteImage();
            button = null;
        }
        
        public void addItem(AbsItem item)
        {
            this.item = item;

            button?.refreshItem();
        }

        public AbsItem pullItem()
        {
            AbsItem result = this.item;
            item = null;

            button?.refreshItem();

            return result;
        }

        public void clear()
        {
            if (item != null)
            {
                pullItem().DeleteImage();
                item = null;
            }
        }

        public bool Visible
        {
            get
            {
                return button.Visible;
            }

            set
            {
                button.Visible = value;
                item?.setVisible(value);
            }
        }

        public SlotType quickMove()//out SlotType moveClick, out SlotType equipClick)
        {
            if (slotType == SlotType.Backpack)
            {
                return SlotType.OnGround;
            }
            else
            {
                return SlotType.Backpack;
            }

            //if (slotType == SlotType.Backpack || slotType == SlotType.OnGround)
            //{
            //    equipClick = item.QuickEquipSlot();
            //}
            //else
            //{
            //    equipClick = SlotType.None;
            //}
        }

        public static SpriteName SlotTypeIcon(SlotType type)
        {
            switch (type)
            {
                case SlotType.Backpack:
                    return SpriteName.cmdBackpack;

                case SlotType.OnGround:
                    return SpriteName.cmdItemPouch;

                case SlotType.EquipBody:
                    return SpriteName.equipArmor;

                case SlotType.EquipMainHand:
                    return SpriteName.equipHand1;

                case SlotType.EquipSecondHand:
                    return SpriteName.equipHand2;

                case SlotType.EquipTrinket:
                    return SpriteName.equipTrinket;

                case SlotType.Quickbelt:
                    return SpriteName.equipQuickbelt;

                default:
                    return SpriteName.MissingImage;
            }
        }

        public override string ToString()
        {
            return "Slot(" + slotType.ToString() + ") item(" + TextLib.ToString_Safe(item) + ") button(" + (button != null).ToString() + ")";
        }

        public bool HasItem => item != null;
        public bool Empty => item == null;

        public bool Contains(ItemFilter type)
        {
            return item != null && item.type == type;
        }

    }


    enum SlotType
    {
        None,
        OnGround,
        Backpack,
        Quickbelt,

        EquipBody,
        EquipMainHand,
        EquipSecondHand,
        EquipTrinket,

        SendToPlayer,
        NUM,
        AnyWearingEquipment,
    }
}
