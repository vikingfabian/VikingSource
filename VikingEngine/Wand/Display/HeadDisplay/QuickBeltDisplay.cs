
using Microsoft.Xna.Framework;
using System.Linq;
using VikingEngine.Core.Wand.GO.ObjectPointer;
using VikingEngine.Engine;
using VikingEngine.HUD;
using VikingEngine.ToGG;

namespace VikingEngine.Core.Wand.Display.HeadDisplay
{
    struct QuickBeltItem
    {
        public static readonly QuickBeltItem Empty = new QuickBeltItem();

        public TileItemType itemType;
        public int count;

        public QuickBeltItem(TileItemType itemType, int count)
        {
            this.itemType = itemType;
            this.count = count;
        }

        public bool IsEmpty()
        {
            return count == 0;
        }

        public bool HasValue()
        {
            return count > 0 && itemType != TileItemType.NONE;
        }

        public bool CanStack(QuickBeltItem other)
        {
            return  count > 0 && itemType == other.itemType;
        }
    }

    class QuickBeltDisplay
    {
        public const ImageLayers Layer = ImageLayers.Lay5;
        public const int SlotCount = 6;
        List2<QuickBeltDisplayMember> members;
        VectorRect mouseArea;

        Graphics.RectangleLines outline;
        Graphics.RectangleLines selectOutline;

        public QuickBeltDisplay(float leftPos)
        {
            Vector2 size = Screen.IconSizeV2 * 1.1f;
            size.Round();
            float spacing = Engine.Screen.BorderWidth;

            VectorRect area = new VectorRect(new Vector2(leftPos, Engine.Screen.SafeArea.Bottom - size.Y), size);
            mouseArea = area;
            members = new List2<QuickBeltDisplayMember>(SlotCount);
            for (int i = 0; i < SlotCount; ++i)
            {
                members.Add(new QuickBeltDisplayMember(area));

                if (i == SlotCount - 1)
                {
                    mouseArea.includeArea(area);
                }
                else
                {
                    area.nextAreaX(1, spacing);
                }
            }

            outline = new Graphics.RectangleLines(area, 2, 1, Layer - 2);
            outline.setColor(Color.Gray);

            selectOutline = new Graphics.RectangleLines(area, 4, 1, Layer - 2);
            selectOutline.Visible = false;

            mouseArea.AddRadius(Screen.BorderWidth);
        }

        public QuickBeltItem? Selected()
        {
            if (members.HasSelection())
            {
                if (members.Sel().item.IsEmpty())
                {
                    members.UnSelect();
                }
                else
                {
                    selectOutline.Visible = true;

                    return members.Sel().item;
                }
            }

            selectOutline.Visible = false;
            return null;
        }

        public bool Add(QuickBeltItem item)
        {
            int? freeSlot = null;

            for (int i = 0; i < members.Count; i++)
            {
                if (freeSlot == null)
                {
                    if (members.Array[i].item.IsEmpty())
                    {
                        freeSlot = i;
                    }
                }

                if (members.Array[i].item.CanStack(item))
                {
                    members.Array[i].item.count += item.count;
                    members.Array[i].refresh();
                    return true;
                }
            }

            if (freeSlot.HasValue)
            {
                members.Array[freeSlot.Value].item = item;
                members.Array[freeSlot.Value].refresh();
                return true;
            }

            return false;
        }

        public void update(ref bool mouseOver)
        {
            int? selected = null;
            if (mouseArea.IntersectPoint(Input.Mouse.Position))
            {
                mouseOver = true;

                for (int i = 0; i < members.Count; i++)
                {
                    if (members.Array[i].area.IntersectPoint(Input.Mouse.Position))
                    {
                        selected = i;
                    }
                }
            }

            if (selected.HasValue)
            {
                outline.Visible = true;
                outline.SetRectangle(members.Array[selected.Value].area);

                if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    if (members.Array[selected.Value].item.HasValue())
                    {
                        switch (members.Array[selected.Value].item.itemType)
                        {
                            case TileItemType.Health_potion:
                                members.Array[selected.Value].SpendItem();
                                WandRef.world.hero.Heal(2);
                                break;
                            case TileItemType.Throwing_knife:
                                if (members.selIndex == selected.Value)
                                {
                                    members.UnSelect();
                                }
                                else
                                {
                                    members.selIndex = selected.Value;
                                    selectOutline.SetRectangle(members.Sel().area);
                                }
                                Refresh();
                                break;
                        }
                    }
                }
            }
            else
            {
                outline.Visible = false;
            }
        }

        public void Refresh()
        {
            Selected();

        }
    }

    class QuickBeltDisplayMember
    {
        public VectorRect area;
        public QuickBeltItem item;

        Graphics.Image itemIcon;
        Graphics.Text2 itemCount;

        public QuickBeltDisplayMember(VectorRect area)
        {
            this.area = area;
            NineSplitAreaTexture areaTexture = new NineSplitAreaTexture(SpriteName.WarsHudPrimaryButtonDisabled, 4, 4, area, 1f, true, QuickBeltDisplay.Layer, true);

            VectorRect iconArea = area;
            iconArea.AddRadius(-6);
            itemIcon = new Graphics.Image(SpriteName.MissingImage, iconArea.Position, iconArea.Size, QuickBeltDisplay.Layer - 2, false);
            itemCount = new Graphics.Text2("0", LoadedFont.Bold, iconArea.PercentToPosition(new Vector2(0.75f, 0.5f)), iconArea.Height * 0.6f, Color.Black, QuickBeltDisplay.Layer - 3);

            refresh();
        }

        public void SpendItem()
        {
            item.count--;
            refresh();
        }

        public void refresh()
        {
            if (item.IsEmpty())
            {
                itemIcon.Visible = false;
                itemCount.Visible = false;
            }
            else
            {
                switch (item.itemType)
                {
                    case TileItemType.Throwing_knife:
                        itemIcon.SetSpriteName(SpriteName.cmdThrowKnife);
                        break;
                    case TileItemType.Health_potion:
                        itemIcon.SetSpriteName(SpriteName.cmdPotionHealth);
                        break;
                }

                itemCount.TextString = item.count.ToString();

                itemIcon.Visible = true;
                itemCount.Visible = true;
            }
        }
    }
}
