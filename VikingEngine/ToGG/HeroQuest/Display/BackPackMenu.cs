using Microsoft.Xna.Framework;
using System.Collections.Generic;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.HeroQuest.Gadgets;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    class BackPackMenu
    {
        public static bool DebugAccess = false;
        public static readonly IntVector2 GroundSquareCount = new IntVector2(6, 6);
        
        Button exitButton;
        Button takeAllGroundButton;
        Graphics.ImageGroup images;
        List<ItemSlot> slots = new List<ItemSlot>();
        Grid2D<ItemSlot> groundSlots;

        public LocalPlayer player;
        public PackItemDragNDrop itemDrag = null;
        public ItemCountDialogue countDialogue = null;

        List<ToggEngine.Display2D.PulsatingHud> areaHighlights = new List<ToggEngine.Display2D.PulsatingHud>();

        VectorRect mainArea, groundArea, equipArea, beltArea;
        public Backpack backpack;
        List2<SendToPlayerButton> sendToPlayers;

        Graphics.Image groundicon;
        Graphics.Text2 groundtitle;

        public BackPackMenu(LocalPlayer player)
        {
            this.player = player;
            backpack = player.Backpack();
            Vector2 squareSz = Engine.Screen.IconSizeV2;
            float spacing = Engine.Screen.BorderWidth;
            float edge = Engine.Screen.BorderWidth * 2;
            float topSize = Engine.Screen.TextTitleHeight + spacing;

            images = new Graphics.ImageGroup();

            //MAIN
            {
                Vector2 tableSize = new Vector2(
                    Table.TotalWidth(BackPackPage.SquareCount.X, squareSz.X, spacing),
                    Table.TotalWidth(BackPackPage.SquareCount.Y, squareSz.Y, spacing));
                                
                tableSize.Y += topSize;

                mainArea = VectorRect.FromCenterSize(Engine.Screen.CenterScreen, tableSize);

                Graphics.Image icon;
                Graphics.Text2 title;
                areaTitle(mainArea, SpriteName.cmdBackpack, "Backpack", out icon, out title);

                if (!access())
                {
                    Graphics.Text2 noAccesText = new Graphics.Text2("Rest to access the backpack", 
                        LoadedFont.Regular, new Vector2( title.MeasureRightPos() + edge, title.Ypos), 
                        Engine.Screen.TextTitleHeight, Color.LightGray, HudLib.BackpackLayer);
                    images.Add(noAccesText);
                }

                Vector2 cellStart = VectorExt.AddY(mainArea.Position, topSize);
                backpack.Page.slots.LoopBegin();

                while (backpack.Page.slots.LoopNext())
                {
                    var buttonAr = new VectorRect(
                        cellStart + backpack.Page.slots.LoopPosition.Vec * VectorExt.Add(squareSz, spacing),
                        squareSz);

                    var slot = backpack.Page.slots.LoopValueGet();
                    slot.createButton(buttonAr, HudLib.BackpackLayer);

                    slots.Add(slot);
                }

                mainArea.AddRadius(edge);
                var border = HudLib.ThickBorder(mainArea, HudLib.BackpackLayer + 1);
                images.Add(border);

                var exitButtonAr = new VectorRect(mainArea.RightTop, Engine.Screen.SmallIconSizeV2);
                exitButtonAr.X -= exitButtonAr.Width;
                exitButtonAr.X -= 2;
                exitButtonAr.Y += 2;

                exitButton = new Button(exitButtonAr, HudLib.BackpackLayer - 1, ButtonTextureStyle.StandardEdge);
                exitButton.addCoverImage(SpriteName.cmdHudCross, 0.9f).Color = HudLib.TitleTextBronze;
            }

            //QUICK BELT
            {
                beltArea = mainArea;
                beltArea.Y = mainArea.Bottom + edge;
                beltArea.Height = squareSz.Y + edge * 2f;
                 var border = HudLib.ThickBorder(beltArea, HudLib.BackpackLayer + 1);
                images.Add(border);

                //Vector2 contentSz = new Vector2(
                //    Table.TotalWidth(backpack.equipment.quickbelt.slots.Count, squareSz.X, spacing), squareSz.Y);

                Vector2 center = beltArea.Center;
                var qbSlots = backpack.equipment.quickbelt.slots;

                for (int i = 0; i < qbSlots.Count; ++i)
                {
                    var buttonAr = Table.CellPlacement(center, true, i, qbSlots.Count, 
                        squareSz, new Vector2(spacing));
                    
                    var slot = qbSlots[i];
                    slot.removeButton();
                    slot.createButton(buttonAr, HudLib.BackpackLayer);
                    slot.Visible = true;

                    slots.Add(slot);
                }
            }

            //GROUND
            if (access())
            {
                Vector2 tableSize = new Vector2(
                    Table.TotalWidth(GroundSquareCount.X, squareSz.X, spacing),
                    Table.TotalWidth(GroundSquareCount.Y, squareSz.Y, spacing));

                tableSize.Y += topSize;

                groundArea = new VectorRect(mainArea.RightTop, tableSize);
                groundArea.X += edge;
                groundArea.Position += new Vector2(edge);

                
                areaTitle(groundArea, SpriteName.MissingImage, TextLib.Error, out groundicon, out groundtitle);

                Vector2 cellStart = VectorExt.AddY(groundArea.Position, topSize);
                ForXYLoop loop = new ForXYLoop(GroundSquareCount);
                groundSlots = new Grid2D<ItemSlot>(GroundSquareCount);

                while (loop.Next())
                {
                    var buttonAr = new VectorRect(
                        cellStart + loop.Position.Vec * VectorExt.Add(squareSz, spacing),
                        squareSz);
                    ItemSlot slot = new ItemSlot(SlotType.OnGround, buttonAr, HudLib.BackpackLayer);
                    groundSlots.Set(loop.Position, slot);

                    slots.Add(slot);
                }

                if (access())
                {
                    VectorRect buttonAr = groundArea;
                    buttonAr.Y = groundArea.Bottom + spacing;
                    buttonAr.Height = squareSz.Y;
                    takeAllGroundButton = new Button(buttonAr, HudLib.BackpackLayer, ButtonTextureStyle.Standard);
                    takeAllGroundButton.addCenterText("Take All", Color.White, 0.8f);

                    groundArea.SetBottom(buttonAr.Bottom, true);
                }

                groundArea.AddRadius(edge);
                var border = HudLib.ThinBorder(groundArea, HudLib.BackpackLayer + 1);
                images.Add(border);

                collectGroundItems();
            }

            //EQUIP
            {
                IntVector2 squares = new IntVector2(2, backpack.equipment.list.Count);

                Vector2 tableSize = new Vector2(
                    Table.TotalWidth(squares.X, squareSz.X, spacing),
                    Table.TotalWidth(squares.Y, squareSz.Y, spacing));

                tableSize.Y += topSize;

                equipArea = new VectorRect(mainArea.Position, tableSize);
                equipArea.X -= edge * 2 + tableSize.X;
                equipArea.Y += edge;

                Graphics.Text2 title = new Graphics.Text2("Equip", LoadedFont.Bold,
                    equipArea.Position, Engine.Screen.TextTitleHeight, HudLib.TitleTextBronze, HudLib.BackpackLayer);
                images.Add(title);

                Vector2 cellStart = VectorExt.AddY(equipArea.Position, topSize);
                ForXYLoop loop = new ForXYLoop(squares);

                while (loop.Next())
                {
                    var buttonAr = new VectorRect(
                        cellStart + loop.Position.Vec * VectorExt.Add(squareSz, spacing),
                        squareSz);

                    var slot = backpack.equipment.list[loop.Position.Y];

                    if (loop.Position.X == 0)
                    {
                        Graphics.Image icon = new Graphics.Image(ItemSlot.SlotTypeIcon(slot.slotType),
                            buttonAr.Position, buttonAr.Size, HudLib.BackpackLayer);
                        images.Add(icon);
                    }
                    else
                    {   
                        slot.createButton(buttonAr, HudLib.BackpackLayer);
                        slots.Add(slot);
                    }
                }

                equipArea.AddRadius(edge);
                var border = HudLib.ThickBorder(equipArea, HudLib.BackpackLayer + 1);
                images.Add(border);
            }

            //SEND TO PLAYERS
            if (access())
            {
                var hero = player.HeroUnit;
                var adjacent = hero.collectAdjacentHeroes(hero.squarePos, true);

                sendToPlayers = new List2<SendToPlayerButton>(4);
                var remotes = hqRef.players.remotePlayersCounter;
                remotes.Reset();

                Vector2 sz = Engine.Screen.IconSizeV2;
                VectorRect area = new VectorRect(mainArea.Position, sz);
                area.X += edge;
                area.Y -= sz.Y + edge;

                while (remotes.Next())
                {
                    var rp = remotes.GetSelection;
                    SendToPlayerButton button = new SendToPlayerButton(area, rp);
                    sendToPlayers.Add(button);
                    button.Enabled = adjacent.Contains(rp.HeroUnit);

                    area.nextAreaX(1, spacing);
                }
            }

            float extendButtonArea = spacing * 0.5f;

            SlotView view = access() ? SlotView.Backpack : SlotView.Backpack_NoAccess;

            foreach (var m in slots)
            {
                m.button.slotView = view;
                m.button.area.AddRadius(extendButtonArea);
            }

            refreshGroundTitle();
        }

        void areaTitle(VectorRect area, SpriteName icon, string text, 
           out Graphics.Image icImg, out Graphics.Text2 title)
        {
            icImg = new Graphics.Image(icon, area.Position + new Vector2(Engine.Screen.TextTitleHeight * 0.5f), new Vector2(Engine.Screen.TextTitleHeight * 1.4f), HudLib.BackpackLayer, true);

            title = new Graphics.Text2(text, LoadedFont.Bold,
                   VectorExt.AddX(area.Position, icImg.Width), Engine.Screen.TextTitleHeight, HudLib.TitleTextBronze, HudLib.BackpackLayer);

            images.Add(icImg); images.Add(title);
        }

        void refreshGroundTitle()
        {
            if (groundicon != null)
            {
                var coll = groundCollection(false);
                if (coll != null && coll.data.Chest)
                {
                    groundicon.SetSpriteName(coll.data.texture());
                    groundtitle.TextString = coll.data.name();
                }
                else
                {
                    groundicon.SetSpriteName(SpriteName.cmdItemPouch);
                    groundtitle.TextString = "Ground";
                }
            }
        } 
        
        bool access()
        {
            if (DebugAccess)
            {
                return true;
            }

            var strat = player.Hero.availableStrategies.active;
            return strat != null && strat.Type == HeroStrategyType.Rest;
        }

        public bool update()
        {
            bool accessUpdate = access();

            if (countDialogue != null)
            {
                if (countDialogue.update())
                {
                    countDialogue.DeleteMe();
                    countDialogue = null;
                }
                return false;
            }

            if (exitButton.update())
            {
                return true;
            }

            if (accessUpdate && takeAllGroundButton.update())
            {
                groundSlots.LoopBegin();
                while (groundSlots.LoopNext())
                {
                    if (groundSlots.LoopValueGet().item != null)
                    {
                        hqRef.items.quickDragNdropItem(this, groundSlots.LoopValueGet(), SlotType.Backpack);
                    }
                }
            }

            if (toggRef.inputmap.menuInput.openCloseInputEvent())
            {
                if (itemDrag != null)
                {
                    itemDrag.cancel();
                }
                else
                {
                    return true;
                }
            }

            if (itemDrag != null)
            {
                updateItemDrag();
            }
            else
            {
                ItemSlot selected = null;
                ItemSlot dragInpt = null;

                if (Input.Keyboard.Ctrl)
                {
                    lib.DoNothing();
                }

                foreach (var m in slots)
                {
                    if (selected == null)
                    {
                        if (m.button.update())
                        {
                            dragInpt = m;
                        }

                        if (m.button.mouseOver)
                        {
                            selected = m;
                        }
                    }
                    else
                    {
                        m.button.emptyUpdate();
                    }
                }

                if (accessUpdate)
                {
                    foreach (var m in sendToPlayers)
                    {
                        m.update();
                    }

                    if (selected != null && selected.item != null)
                    {
                        SlotType moveClick;
                        SlotType equipClick;
                        backpack.quickMoveEquip(selected, out moveClick, out equipClick);

                        if (toggRef.inputmap.quickMoveItem.DownEvent)
                        {
                            hqRef.items.quickDragNdropItem(this, selected, moveClick);
                        }
                        else if (toggRef.inputmap.click.DownEvent && 
                            Input.Keyboard.Ctrl &&
                            equipClick != SlotType.None)
                        {
                            hqRef.items.quickDragNdropItem(this, selected, equipClick);                            
                        }
                        else if (dragInpt != null)
                        {
                            itemDrag = new PackItemDragNDrop(this, dragInpt);
                        }
                    }
                }
            }
            return false;
        }
               
        public void onDrag(AbsItem item)
        {
            areaHighlights.Add(new ToggEngine.Display2D.PulsatingHud(mainArea, false));
            areaHighlights.Add(new ToggEngine.Display2D.PulsatingHud(groundArea, false));

            if (item.slotAccess(SlotType.Quickbelt))
            {
                areaHighlights.Add(new ToggEngine.Display2D.PulsatingHud(beltArea, false));

                foreach (var m in backpack.equipment.quickbelt.slots)
                {
                    highlightSlot(m);
                }
            }

            if (item.slotAccess(SlotType.AnyWearingEquipment))
            {
                areaHighlights.Add(new ToggEngine.Display2D.PulsatingHud(equipArea, false));

                foreach (var equip in item.Equip.slots)
                {
                    if (EquipSlots.IsWearEquipSlot(equip))
                    {
                        var slots = backpack.equipment.slot(equip);
                        foreach (var m in slots)
                        {
                            highlightSlot(m);
                        }
                    }
                }
            }

            void highlightSlot(ItemSlot slot)
            {
                var area = slot.button.area;
                area.AddRadius(-HudLib.ThickBorderEdgeSize);
                areaHighlights.Add(new ToggEngine.Display2D.PulsatingHud(area, false, 
                    slot.button.BottomPaintLayer));
            }
        }

        void updateItemDrag()
        {
            ItemSlot mouseOver = null;
            foreach (var m in slots)
            {
                if (itemDrag.item == null ||
                    itemDrag.item.slotAccess(m))
                {
                    if (mouseOver == null)
                    {
                        m.button.update_EvenIfDisabled();
                        if (m.button.mouseOver)
                        {
                            mouseOver = m;
                        }
                    }
                    else
                    {
                        m.button.emptyUpdate();
                    }
                }
            }

            foreach (var m in sendToPlayers)
            {
                m.update();
                if (m.mouseOver)
                {
                    mouseOver = m.slot;
                }
            }

            itemDrag.update(mouseOver);
        }

        public void EndDragItem()
        {
            arraylib.DeleteAndClearArray(areaHighlights);
            itemDrag = null;

            foreach (var m in slots)
            {
                m.button.emptyUpdate();                
            }
        }

        public void refreshBackpack()
        {
            var pg = player.Backpack().Page;
            pg.slots.LoopBegin();
            while (pg.slots.LoopNext())
            {
                var btn = pg.slots.LoopValueGet().button;
                btn.refreshItem();
            }

            refreshGroundTitle();
        }

        public void refreshQuickBelt()
        {
            var qbSlots = backpack.equipment.quickbelt.slots;

            for (int i = 0; i < qbSlots.Count; ++i)
            {
                qbSlots[i].button.refreshItem();
            }
        }
        
        void collectGroundItems()
        {
            var coll = groundCollection(false);
            if (coll != null)
            {
                coll.onAccess();

                groundSlots.LoopBegin();
                foreach (var m in coll.items)
                {
                    groundSlots.LoopNext();
                    groundSlots.LoopValueGet().addItem(m);
                }
            }
        }

        public void refreshGround()
        {
            if (access())
            {
                var coll = groundCollection(false);
                if (coll != null)
                {
                    HashSet<int> containsAlready = new HashSet<int>();
                    groundSlots.LoopBegin();
                    while (groundSlots.LoopNext())
                    {
                        var item = groundSlots.LoopValueGet().item;
                        if (item != null)
                        {
                            groundSlots.LoopValueGet().button.refreshItem();
                            containsAlready.Add(item.id);
                        }
                    }

                    groundSlots.LoopBegin();
                    foreach (var m in coll.items)
                    {
                        if (containsAlready.Contains(m.id) == false)
                        {
                            //find an empy slot to place the new item
                            while (groundSlots.LoopNext())
                            {
                                var item = groundSlots.LoopValueGet().item;
                                if (item == null)
                                {
                                    groundSlots.LoopValueGet().addItem(m);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        TileItemCollection groundCollection(bool createIfMissing)
        {
            var sq = player.HeroUnit.squarePos;

            return hqRef.items.groundCollection(sq, createIfMissing);
        }

        public void DeleteMe()
        {
            if (itemDrag != null)
            {
                itemDrag.cancel();
            }

            arraylib.DeleteAndClearArray(areaHighlights);
            arraylib.DeleteAndClearArray(sendToPlayers);

            foreach (var m in slots)
            {
                m.removeButton();
            }
            images.DeleteAll();
            exitButton.DeleteMe();

            backpack.equipment.quickbelt.createButtons();

            takeAllGroundButton?.DeleteMe();
        }
    }

    
}
