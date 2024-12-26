
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest.Gadgets;

namespace VikingEngine.ToGG.HeroQuest.Players.Phase
{
    class LootDialogue :AbsPlayerPhase
    {
        List<AbsItem> itemLootStack;
        List2<LootDialogueCell> cells;
        Display.Button closeButton;

        public LootDialogue(LocalPlayer localPlayer, List<AbsItem> itemLootStack)
            :base(localPlayer)
        {
            this.itemLootStack = itemLootStack;
        }

        public override void onBegin()
        {
            base.onBegin();
            cells = new List2<LootDialogueCell>(itemLootStack.Count);

            Vector2 cellSize = Engine.Screen.IconSizeV2 * 5f;
            float spacing = Engine.Screen.BorderWidth;
            int columns = Bound.Max(itemLootStack.Count, 4);

            VectorRect firstCellArea = Table.CellPlacement(Engine.Screen.CenterScreen, true, 0, columns, cellSize, new Vector2(spacing));

            for (int i = 0; i < itemLootStack.Count; ++i)
            {
                VectorRect ar = firstCellArea;
                ar.nextAreaX(i, spacing);
                cells.Add(new LootDialogueCell(itemLootStack[i], 
                    ar, player));
            }

            Vector2 closeButtonSz = new Vector2(6, 1) * Engine.Screen.IconSize;
            VectorRect closeButtonAr = new VectorRect(
                Engine.Screen.CenterScreen.X - closeButtonSz.X * 0.5f,
                firstCellArea.Y - Engine.Screen.IconSize - closeButtonSz.Y,
                closeButtonSz.X, closeButtonSz.Y);
            closeButton = new Display.Button(closeButtonAr, HudLib.PopupLayer, Display.ButtonTextureStyle.Standard);
            closeButton.addCenterText("Close", Color.White, 0.8f);

            itemLootStack.Clear();
        }

        Time inputDelay = new Time(200);

        public override void update(ref PlayerDisplay display)
        {
            if (inputDelay.CountDown())
            {
                cells.loopBegin();
                while (cells.loopNext())
                {
                    if (cells.sel.update())
                    {
                        cells.sel.DeleteMe();
                        cells.loopRemove();

                        foreach (var m in cells)
                        {
                            m.refreshEquiped();
                        }
                    }
                }

                if (cells.Count == 0 ||
                    closeButton.update() ||
                    toggRef.inputmap.menuInput.openCloseInputEvent())
                {
                    end();
                }
            }
        }

        void quickResolve()
        {
            foreach (var m in cells)
            {
                m.quickResolve();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();


            quickResolve();

            closeButton.DeleteMe();

            foreach (var m in cells)
            {
                m.DeleteMe();
            }

            player.Backpack().NetShare();
        }

        public override PhaseType Type => PhaseType.LootDialogue;

        class LootDialogueCell
        {
            Graphics.ImageGroup images = new Graphics.ImageGroup();
            List<EquipOptionButton> options = new List<EquipOptionButton>(4);
            public LootDialogueCell(AbsItem item, VectorRect area, LocalPlayer player)
            {
                VectorRect contentArea = area;
                contentArea.AddRadius(-Engine.Screen.BorderWidth * 2f);
                Graphics.Text2 title = new Graphics.Text2(item.Name, LoadedFont.Bold,
                    contentArea.CenterTop, Engine.Screen.TextTitleHeight, HudLib.TitleTextBronze, HudLib.PopupLayer);
                title.OrigoAtCenterWidth();
                images.Add(title);

                Graphics.Image portrait;
                {
                    portrait = new Graphics.Image(item.Icon, VectorExt.AddY(title.position, Engine.Screen.TextTitleHeight),
                        Engine.Screen.IconSizeV2, HudLib.PopupLayer);
                    portrait.Xpos -= portrait.Width * 0.5f;
                    VectorRect pArea = portrait.Area;
                    var portraitBg = new Graphics.Image(SpriteName.WhiteArea_LFtiles, pArea.Position, pArea.Size, ImageLayers.AbsoluteBottomLayer);
                    portraitBg.ColorAndAlpha(Color.Black, 0.5f);
                    portraitBg.LayerBelow(portrait);
                    var portraitBorder = new Graphics.RectangleLines(pArea, 2, 1f, HudLib.PopupLayer);
                    portraitBorder.setColor(ColorExt.VeryDarkGray);
                    images.Add(portrait);
                    images.Add(portraitBg);
                    images.Add(portraitBorder);
                }

                HUD.RichBox.RichBoxGroup richBox;
                {
                    Vector2 descStart = new Vector2(contentArea.X, portrait.Bottom + Engine.Screen.BorderWidth);
                    var rbMembers = item.DescAsRichbox();
                    richBox = new HUD.RichBox.RichBoxGroup(descStart, contentArea.Width, HudLib.PopupLayer,
                    HudLib.MouseTipRichBoxSett, rbMembers);

                    images.Add(richBox);
                }

                {//Equip options

                    VectorRect buttonArea = new VectorRect(contentArea.X,
                        portrait.Bottom + Engine.Screen.IconSize * 3,
                        contentArea.Width, 
                        Engine.Screen.IconSize);

                    foreach (var m in item.Equip.slots)
                    {
                        int slotCount = player.Backpack().equipment.slotCount(m);

                        for (int i = 0; i < slotCount; ++i)
                        {
                            addEquipOption(m, i, slotCount);
                        }
                    }

                    addEquipOption(SlotType.Backpack, 0, 1);

                    area.SetBottom(buttonArea.Y + Engine.Screen.BorderWidth, true);

                    void addEquipOption(SlotType slot, int slotIx, int slotCount)
                    {
                        EquipOptionButton button = new EquipOptionButton(buttonArea, item, slot, 
                            slotIx, slotCount,
                            player.Backpack());
                        options.Add(button);
                        buttonArea.nextAreaY(1, Engine.Screen.BorderWidth);
                    }
                }
                
                var bg = HudLib.ThinBorder(area, HudLib.PopupLayer + 1);
                images.Add(bg);
            }

            public void refreshEquiped()
            {
                foreach (var m in options)
                {
                    m.refreshEquiped();
                }
            }

            public bool update()
            {
                foreach (var m in options)
                {
                    if (m.update())
                    {
                        return true;
                    }
                }
                return false;
            }

            public void quickResolve()
            {
                arraylib.Last(options).QuickResolve();
            }

            public void DeleteMe()
            {
                images.DeleteAll();
                foreach (var m in options)
                {
                    m.DeleteMe();
                }
            }

            class EquipOptionButton : Display.Button
            {
                AbsItem item;
                SlotType slot;
                int slotIx;
                Gadgets.Backpack backpack;

                public bool willReplaceItem;
                Graphics.ImageGroup replaceItemImg = new Graphics.ImageGroup();
                VectorRect iconAr;

                public EquipOptionButton(VectorRect area, AbsItem item, 
                    SlotType slot, int slotIx, int slotCount,
                    Gadgets.Backpack backpack)
                    :base(area, HudLib.PopupLayer, Display.ButtonTextureStyle.Standard)
                {
                    this.item = item;
                    this.slot = slot;
                    this.slotIx = slotIx;
                    this.backpack = backpack;

                    SpriteName icon;
                    string text;
                    if (slot == SlotType.Backpack)
                    {
                        icon = SpriteName.cmdAddToBackpack;
                        text = "Backpack";
                    }
                    else
                    {
                        icon = ItemSlot.SlotTypeIcon(slot);
                        text = "Equip";

                        if (slotCount > 1)
                        {
                            text += " (slot " + TextLib.IndexToString(slotIx) + ")";
                        }
                    }
                    iconAr = new VectorRect(area.Position, new Vector2(area.Height));
                    iconAr.AddPercentRadius(-0.1f);
                    Graphics.Image iconImg = new Graphics.Image(icon, iconAr.Position, iconAr.Size, HudLib.PopupLayer - 1);
                    Graphics.Text2 textImg = new Graphics.Text2(text, LoadedFont.Regular, iconAr.RightCenter, iconAr.Height * 0.8f,
                        Color.White, HudLib.PopupLayer - 1);
                    textImg.OrigoAtCenterHeight();

                    imagegroup.Add(iconImg);
                    imagegroup.Add(textImg);

                    refreshEquiped();
                }

                public void refreshEquiped()
                {
                    if (slot != SlotType.Backpack)
                    {
                        replaceItemImg.DeleteAll();

                        var itemSlot = backpack.equipment.slot(slot, slotIx);
                        willReplaceItem = itemSlot.HasItem;
                        if (willReplaceItem)
                        {
                            VectorRect ar = iconAr;
                            ar.Size *= 0.6f;
                            ar.SetRightBottom( iconAr.RightBottom, false);

                            var itemIcon = new Graphics.Image(itemSlot.item.Icon, 
                                ar.Position, ar.Size, HudLib.PopupLayer - 2);
                            var itemBg = new Graphics.Image(SpriteName.WhiteArea_LFtiles,
                                ar.Position, ar.Size, 0);
                            itemBg.LayerBelow(itemIcon);
                            itemBg.ColorAndAlpha(Color.Black, 0.9f);

                            replaceItemImg.Add(itemIcon);
                            replaceItemImg.Add(itemBg);
                        }
                    }
                }

                protected override void onClick()
                {
                    base.onClick();
                    backpack.equip(item, slot, slotIx);
                }

                public void QuickResolve()
                {
                    backpack.add(item, true);
                }
                
                protected override void createToolTip()
                {
                    base.createToolTip();

                    if (slot != SlotType.Backpack)
                    {
                        var itemSlot = backpack.equipment.slot(slot, slotIx);
                        if (itemSlot.HasItem)
                        {
                            var richbox = itemSlot.item.DescAsRichbox();
                            richbox.InsertRange(0, new List<HUD.RichBox.AbsRichBoxMember>
                            {
                                new HUD.RichBox.RichBoxBeginTitle(),
                                new HUD.RichBox.RichBoxText("Replace"),
                                new HUD.RichBox.RichBoxNewLine(),
                                new HUD.RichBox.RichBoxImage(itemSlot.item.Icon),
                                new HUD.RichBox.RichBoxText(itemSlot.item.Name),
                                new HUD.RichBox.RichBoxNewLine(),
                            });
                            HudLib.AddTooltipText(tooltip, richbox, Dir4.S, area,
                                area, true, false);
                        }
                    }
                }

                public override void DeleteMe()
                {
                    base.DeleteMe();
                    replaceItemImg.DeleteAll();
                }
            }
        }
    }
}
