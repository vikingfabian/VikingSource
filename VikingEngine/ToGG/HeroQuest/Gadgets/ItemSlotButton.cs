using Microsoft.Xna.Framework;
using System.Collections.Generic;
using VikingEngine.ToGG.HeroQuest.Display;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class ItemSlotButton : AbsButton
    {
        const float BgOpacity = 0.3f;
        static readonly Color EdgeCol = ColorExt.GrayScale(0.6f), EdgeColEmpty = ColorExt.GrayScale(0.3f);
        Graphics.RectangleLines edge;
        ImageLayers itemlayer;
        ItemSlot itemSlot;
        Graphics.Text2 count = null;
        Graphics.Image countBg = null;

        VectorRect imageArea;
        public SlotView slotView;

        public ItemSlotButton(VectorRect area, ImageLayers layer, ItemSlot itemSlot)
           : base(area, layer)
        {
            imageArea = area;
            imageArea.AddRadius(-1);
            this.itemSlot = itemSlot;
            itemlayer = layer - 2;
            baseImage.Color = Color.Black;
            baseImage.Opacity = BgOpacity;

            edge = new Graphics.RectangleLines(area, 2f, 0f, layer - 1);
            imagegroup.Add(edge.lines);
            
            refreshItem();
        }

        public void refreshItem()
        {
            if (itemSlot.item == null)
            {
                edge.setColor(EdgeColEmpty);
                Enabled = false;

                removeCount();
            }
            else
            {
                itemSlot.item.placeInSlot(imageArea, itemlayer);
                itemSlot.item.setVisible(baseImage.Visible);
                edge.setColor(EdgeCol);
                Enabled = true;

                if (itemSlot.item.count > 1)
                {
                    if (count == null)
                    {
                        float h = Engine.Screen.TextBreadHeight;
                        VectorRect countAr = new VectorRect(
                            Vector2.Zero, new Vector2(h * 1.2f, h));
                        
                        countAr.Position = imageArea.RightBottom - countAr.Size;

                        countBg = new Graphics.Image(SpriteName.WhiteArea,
                            countAr.Position, countAr.Size, itemlayer - 1);
                        countBg.Color = Color.Black;
                        countBg.Opacity = 0.6f;

                        count = new Graphics.Text2("XX", LoadedFont.Bold, countAr.Center,
                            h, Color.White, ImageLayers.AbsoluteBottomLayer);
                        count.LayerAbove(countBg);
                        count.OrigoAtCenter();
                    }
                    count.TextString = itemSlot.item.count.ToString();
                }
                else
                {
                    removeCount();
                }
            }
        }

        protected override void onMouseEnter(bool enter)
        {
            base.onMouseEnter(enter);
            baseImage.Opacity = enter ? 0.8f : BgOpacity;
        }

        protected override void createToolTip()
        {
            if (itemSlot.item != null)
            {
                List<HUD.RichBox.AbsRichBoxMember> members = new List<HUD.RichBox.AbsRichBoxMember>
                {
                    new HUD.RichBox.RichBoxBeginTitle(),
                    new HUD.RichBox.RichBoxText(itemSlot.item.Name),
                    new HUD.RichBox.RichBoxNewLine(false),
                };

                var desc = itemSlot.item.DescriptionAdvanced();
                if (desc == null)
                {
                    members.Add(new HUD.RichBox.RichBoxText(itemSlot.item.Description));                    
                }
                else
                {
                    members.AddRange(desc);
                }

                if (slotView == SlotView.Backpack)
                {
                    SlotType moveClick;
                    SlotType equipClick;
                    hqRef.players.localHost.Backpack().quickMoveEquip(itemSlot, out moveClick, out equipClick);

                    members.Add(new HUD.RichBox.RichBoxNewLine(true));
                    members.Add(new HUD.RichBox.RichBoxImage(toggRef.inputmap.quickMoveItem.Icon));
                    members.Add(new HUD.RichBox.RichBoxImage(SpriteName.cmdConvertArrow, 0.6f));
                    members.Add(new HUD.RichBox.RichBoxImage(ItemSlot.SlotTypeIcon(moveClick)));
                    members.Add(new HUD.RichBox.RichBoxNewLine(false));

                    if (equipClick != SlotType.None)
                    {
                        members.Add(new HUD.RichBox.RichBoxImage(SpriteName.KeyCtrl));
                        members.Add(new HUD.RichBox.RichBoxImage(SpriteName.cmdPlus, 0.6f));
                        members.Add(new HUD.RichBox.RichBoxImage(toggRef.inputmap.click.Icon));

                        members.Add(new HUD.RichBox.RichBoxImage(SpriteName.cmdConvertArrow, 0.6f));
                        members.Add(new HUD.RichBox.RichBoxImage(ItemSlot.SlotTypeIcon(equipClick)));
                    }
                }

                HudLib.AddTooltipText(tooltip, members, Dir4.N, this.area, null);
            }
        }

        protected override void onEnableChange()
        {
            base.onEnableChange();
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            removeCount();
        }

        void removeCount()
        {
            if (count != null)
            {
                count.DeleteMe();
                countBg.DeleteMe();

                count = null;
                countBg = null;
            }
        }

        public override string ToString()
        {
            return "Slot Button (" + itemSlot.ToString() + ")";
        }

        public float BottomPaintLayer => edge.lines[0].PaintLayer;
    }

    enum SlotView
    {
        Hud,
        Backpack,
        Backpack_NoAccess,

    }
}
