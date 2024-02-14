using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;
using VikingEngine.EngineSpace.Maths;

namespace VikingEngine.HUD
{
    class GuiLayout : IDeleteable
    {
        /* Static */
        static int NextId = 0;

        /* Events */
        public event Action OnDelete;

        /* Properties */
        public GuiMember Selected
        {
            get { return members[cursorPos]; }
            set
            {
                var it = members.GetEnumerator();

                do
                {
                    if (it.Current.Value == value)
                    {
                        cursorPos = it.Current.Key;
                    }
                }
                while (it.MoveNext());
            }
        }
        public ImageLayers Layer { get { return layer; } set { layer = value; renderList.Layer = layer; } }

        public ImageLayers LayoutBackgroundLayer { get { return layer + 1; } }
        public ImageLayers MemberBackgroundLayer { get { return layer; } }
        public ImageLayers UnderTextLayer { get { return layer - 1; } }
        public ImageLayers TextLayer { get { return layer - 2; } }
        public ImageLayers AboveTextLayer { get { return layer - 3; } }
        public ImageLayers NextLayer { get { return layer - 4; } }

        /* Fields */
        public Gui gui;
        public RenderTargetDrawContainer renderList = null;//Is a target image, rendering the menu content
        public RenderTargetDrawContainer scrollerRenderer = null;
        public float totalHeight;
        public int Id = NextId++;

        Dictionary<IntVector2, GuiMember> members;
        IntVector2 cursorPos;
        Rectangle2 dimensions;
        Image background;
        Action refreshAction;
        Vector2 scrollerRendererPositionOffset;
        ImageLayers layer;
        GuiLayoutMode layoutMode;
        public bool scrollOnly;
        public bool contentOnlyStyle = false;

        Graphics.ImageGroup headbarImages = null;
        public GuiLayoutHeadButton returnButton, closeMenuButton;
        //Graphics.Image headBar = null;

        /* Constructors */
        public GuiLayout()
        { }

        public GuiLayout(SpriteName titleImage, Gui gui)
            : this(titleImage, null, gui, GuiLayoutMode.SingleColumn)
        { }
        public GuiLayout(string title, Gui gui)
            : this(SpriteName.NO_IMAGE, title, gui, GuiLayoutMode.SingleColumn)
        { }
        public GuiLayout(string title, Gui gui, GuiLayoutMode layoutMode, Action refreshAction)
            : this(SpriteName.NO_IMAGE, title, gui, layoutMode)
        {
            this.refreshAction = refreshAction;
        }
        public GuiLayout(string title, Gui gui, GuiLayoutMode layoutMode, bool scrollOnly)
            : this(SpriteName.NO_IMAGE, title, gui, layoutMode)
        {
            this.scrollOnly = scrollOnly;
        }
        public GuiLayout(SpriteName titleImage, string title, Gui gui, GuiLayoutMode layoutMode, 
            bool contentOnlyStyle = false)
        {
            init(titleImage, title, gui, layoutMode, contentOnlyStyle);
        }

        void init(SpriteName titleImage, string title, Gui gui, GuiLayoutMode layoutMode,
            bool contentOnlyStyle = false)
        {
            this.gui = gui;
            this.layoutMode = layoutMode;
            this.contentOnlyStyle = contentOnlyStyle;

            layer = gui.layer;
            float scrollerWidth = gui.style.memberHeight * 0.66f;
            Vector2 pos = gui.area.Position;//Point pos = Engine.XGuide.GetPlayer(gui.player).view.DrawArea.Location;
            renderList = new RenderTargetDrawContainer(pos, gui.area.Size - new Vector2(scrollerWidth - 1, 0), layer, new List<AbsDraw>());
            renderList.Opacity = 0f;

            members = new Dictionary<IntVector2, GuiMember>();
            cursorPos = IntVector2.Zero;
            dimensions = new Rectangle2(IntVector2.Zero, IntVector2.One);

            scrollerRendererPositionOffset = new Vector2(pos.X + gui.area.Width - scrollerWidth, pos.Y);
            scrollerRenderer = new RenderTargetDrawContainer(scrollerRendererPositionOffset, new Vector2(scrollerWidth, gui.area.Height), layer, new List<AbsDraw>());
            scrollerRenderer.Opacity = 0f;

            if (gui.style.headBar && !contentOnlyStyle)
            {
                var headBar = new Image(SpriteName.WhiteArea, gui.area.Position, new Vector2(renderList.Width, gui.style.headBarHeight), layer);
                headBar.Color = gui.style.headBarColor;
                headBar.Ypos -= headBar.Height;

                var titleText = new TextG(gui.style.textFormat.Font, headBar.Center,// + new Vector2(headBar.Height, headBar.Height * 0.5f),
                    gui.style.textFormat.Scale * 0.9f, Align.CenterAll, title, gui.style.headBarTextColor, ImageLayers.AbsoluteTopLayer);
                titleText.LayerAbove(headBar);

                headbarImages = new ImageGroup(titleText, headBar);

                if (gui.layoutStack.Count > 0 && gui.style.headReturnIcon != SpriteName.NO_IMAGE)
                {
                    returnButton = new GuiLayoutHeadButton(gui.style.headReturnIcon, new Vector2(headBar.Xpos + headBar.Height * 0.5f, headBar.Center.Y),
                        headBar.Height, gui.inputmap.back.Icon, 1, headBar.PaintLayer);
                }
                if (gui.style.headCloseIcon != SpriteName.NO_IMAGE)
                {
                    closeMenuButton = new GuiLayoutHeadButton(gui.style.headCloseIcon, new Vector2(headBar.Right - headBar.Height * 0.5f, headBar.Center.Y),
                        headBar.Height, gui.inputmap.MainOpenCloseKey(gui.inputSource).Icon, -1, headBar.PaintLayer);
                }
            }

            Ref.draw.AddToContainer = renderList;

            if (!gui.style.headBar && !contentOnlyStyle)
            {
                if (title == null)
                {
                    new GuiImageTitle(titleImage, this);
                }
                else
                {
                    new GuiTitle(title, this);
                }
            }
            scrollOnly = false;
        }


        public void AreYouSure_Layout(Gui menu, GuiMember menuMember, Action action)
        {
            init(SpriteName.NO_IMAGE, null, menu, GuiLayoutMode.MultipleColumns, true);
            
            new GuiBigIcon(SpriteName.cmdLargePlainCheckOn, null, new GuiAction(action), false, this);
            new GuiBigIcon(SpriteName.cmdLargePlainCheckOff, null, new GuiAction(gui.PopLayout), false, this);

            this.End();

            renderList.position = VectorExt.AddY( menuMember.ParentPosition + menu.area.Position, menuMember.size.Y);
        }

        /* Methods */
        public void UpdateFading(Vector2 positionOffset, float opacity)
        {

            new TargetFade(renderList, opacity, gui.style.fadeTimeMS);

            new TargetFade(scrollerRenderer, opacity, gui.style.fadeTimeMS);

            if (headbarImages != null)
            {
                headbarImages.SetOpacity(opacity);
            }
        }

        public void SetPaintLayer(float layer)
        {
            renderList.PaintLayer = layer;
            scrollerRenderer.PaintLayer = layer;

            if (headbarImages != null)
            {
                for (int i = 0; i < headbarImages.images.Count; ++i)
                {
                    headbarImages.images[i].PaintLayer = layer + PublicConstants.LayerMinDiff * i;
                }

                if (returnButton != null) returnButton.setLayer(layer);

                if (closeMenuButton != null) closeMenuButton.setLayer(layer);
            }
        }

        float ScrollerProperty(bool set, float val)
        {
            if (renderList != null)
            {
                if (set)
                {
                    renderList.Transform2D = new Vector2(renderList.Transform2D.X, -val);
                }
                return -renderList.Transform2D.Y;
            }
            return 0;
        }

        public void PageUpDown(bool up)
        {
            float move = gui.area.Height;
            if (up)
            {
                move *= -1;
            }

            IntVector2 target = new IntVector2(Selected.ParentX, Math.Max(Selected.ParentY + move, 0));

            IntVector2 closestCursorPos = cursorPos;
            float closestDistance = float.MaxValue;

            foreach (var mem in members)
            {
                if (mem.Value.SelectionType == GuiMemberSelectionType.Selectable)
                {
                    Vector2 memPos = mem.Value.ParentPosition;
                    float diff = Math.Abs((target.Vec - memPos).LengthSquared());

                    if (diff <= closestDistance)
                    {
                        closestDistance = diff;
                        closestCursorPos = mem.Key;
                    }
                }
            }

            cursorPos = closestCursorPos;
            KeepMemberInView(Selected, gui.style.percentageOfViewToEdgeWhenScrolling);
            UpdateScrolling(Vector2.Zero);
        }

        public void KeepMemberInView(GuiMember member, float percentageOfViewToEdge)
        {
            if (!(member is GuiScrollbar))
            {
                Vector2 pos = member.ParentPosition;
                Vector2 offset = renderList.Transform2D;
                Vector2 visibleSize = gui.area.Size;

                Vector2 testDistance = visibleSize * percentageOfViewToEdge;

                if (pos.X + offset.X < 0)
                {
                    offset.X = -pos.X;
                }
                else if (pos.X + member.size.X + offset.X > visibleSize.X)
                {
                    offset.X = visibleSize.X - pos.X - member.size.X;
                }

                if (offset.Y < testDistance.Y - pos.Y)
                {
                    offset.Y = testDistance.Y - pos.Y;
                }
                else if (offset.Y > visibleSize.Y - pos.Y - member.size.Y - testDistance.Y)
                {
                    offset.Y = visibleSize.Y - pos.Y - member.size.Y - testDistance.Y;
                }

                renderList.Transform2D = offset;
            }
        }

        public void UpdateScrolling(Vector2 scrollDiff)
        {
            Vector2 drawOffset = renderList.Transform2D;

            drawOffset.Y -= scrollDiff.Y;
            float maxYOffset = Math.Min(gui.area.Height - totalHeight, 0);
            drawOffset.Y = MathHelper.Clamp(drawOffset.Y, maxYOffset, 0);

            renderList.Transform2D = drawOffset;
        }

        #region Building

        public void Add(GuiMember member)
        {
            if (members.Count > 0)
            {
                // If we already have some members then we need to increment the position, and maybe also grow,
                // depending on for instance if we have moved to another column or not.
                
                switch(layoutMode)
                {
                    case GuiLayoutMode.SingleColumn:
                        NewRow();
                        break;
                    case GuiLayoutMode.MultipleColumns:
                        if (Selected.size.X + gui.style.memberSpacing * 2 < renderList.Width && // if the previous member does not by itself transgress the layout width (by being a label for instance)
                            (cursorPos.X + 2) * (member.size.X + gui.style.memberSpacing) <= renderList.Width)
                        {
                            // If we're ok to add one more element before transgressing the layout width
                            // + 2 comes from (+ 1 for index-to-count) and (+ 1 for one more element)
                            NewColumn();
                        }
                        else
                        {
                            // Else we start a new row and add to the total height.
                            NewRow();
                        }
                        break;
                }
            }
            member.ParentPosition = new Vector2(member.size.X * cursorPos.Vec.X + gui.style.memberSpacing * (cursorPos.Vec.X + 1), totalHeight);
            
            members.Add(cursorPos, member);
        }

        void NewRow()
        {
            // Before we move, we need to accumulate the height of the member we most recently added.
            totalHeight += Selected.size.Y + gui.style.memberSpacing;

            // Regardless of where we are, we will need to move down.
            ++cursorPos.Y;
            if (cursorPos.Y >= dimensions.size.Y)
            {
                // If we are now in a position that is outside the grid bounds, we need to expand the grid.
                // We can only happen to find ourselves at most one index off, wherefore this is not a while-loop.
                ++dimensions.size.Y;
            }

            // Make sure we start at the beginning of the row.
            cursorPos.X = 0;
        }

        void NewColumn()
        {
            // Regardless of where we are, we will need to move right.
            ++cursorPos.X;
            if (cursorPos.X >= dimensions.size.X)
            {
                // If we are now in a position that is outside the grid bounds, we need to expand the grid.
                // We can only happen to find ourselves at most one index off, wherefore this is not a while-loop.
                ++dimensions.size.X;
            }
        }

        public void End()
        {
            if (members.Count == 0)
            {
                new GuiLabel("EMPTY", this);
            }

            // We haven't added the final row's height yet
            totalHeight += Selected.size.Y + gui.style.memberSpacing;

            if (!contentOnlyStyle)
            {
                // Now that we know our final size, we can make our background...
                background = new Image(gui.style.background, Vector2.Zero, new Vector2(gui.style.layoutWidth, totalHeight), LayoutBackgroundLayer);
                background.Color = gui.style.BackgroundColor;
                renderList.AddImage(background);
            }

            // .. and also make a scrollbar when neccessary
            if (totalHeight > gui.area.Height)
            {
                Ref.draw.AddToContainer = scrollerRenderer;
                GuiScrollbar bar = new GuiScrollbar(ScrollerProperty, this);
                bar.ParentPosition = Vector2.Zero;
                members.Add(cursorPos + new IntVector2(0, 1), bar);
            }

            // No more members to add to drawing
            Ref.draw.AddToContainer = null;

            // Try to select an item.
            cursorPos = IntVector2.Zero; // fall back to zero
            foreach (GuiMember mem in GetMembers())
            {
                if (mem.SelectionType == GuiMemberSelectionType.Selectable)
                {
                    Selected = mem;
                    break;
                }
            }

            foreach (GuiMember mem in GetMembers())
            {
                mem.Refresh(null);
            }

            // Open immediately
            gui.PushLayout(this);
        }

        #endregion

        #region Traversal

        public void Move(Input.ThumbStick input)
        {
            if (scrollOnly)
            {
                UpdateScrolling(input.Value);
            }
            else
            {
                if (Selected != null)
                {
                    if (Selected.IsPressed)
                    {
                        Selected.MoveInputWhenSelected(input.Value);
                    }
                    else
                    {
                        Selected.MoveInputWhenHover(input.Value);

                        //IntVector2 stepping = moveInputStepping.update(input.Value);//repeater.GetStepping(fineMove);
                        if (input.Stepping.HasValue())
                        {
                            MoveSteppingInput(input.Stepping);
                        }
                    }
                }
            }
        }

        public void MoveSteppingInput(IntVector2 move)
        {
            // Count the number of traversable members.
            int selectableNonScrollbars = 0;
            foreach (var kv in members)//(GuiMember mem in GetMembers())
            {
                if (kv.Value.SelectionType == GuiMemberSelectionType.Selectable && !(kv.Value is GuiScrollbar))
                {
                    ++selectableNonScrollbars;
                }
            }

            // Let's not move at all if there's nothing to move between.
            if (selectableNonScrollbars == 0)
                return;

            if (move.X != 0 && move.Y != 0)
            {
                // If we need to move along more than one axis, do it one at a time
                MoveSteppingInput(new IntVector2(0, move.Y));
                MoveSteppingInput(new IntVector2(move.X, 0));
                return;
            }

            IntVector2 startPos = cursorPos;
            IntVector2 nextPos = nextPosition(move);//cursorPos + move;
            int loopCount = 0;
            while (nextPos != startPos) // Until we end up where we started off
            {
                GuiMember selected;
                members.TryGetValue(nextPos, out selected);

                if (selected == null && nextPos.X > 0)
                {//Go from multiple colums to one
                    IntVector2 checkPos = nextPos;
                    for (int x = nextPos.X - 1; x >= 0; --x)
                    {
                        checkPos.X = x;
                        if (members.TryGetValue(checkPos, out selected))
                        {
                            nextPos = checkPos;
                            break;
                        }
                    }
                }

                // If the position we would go to exists,
                if (selected != null)//members.TryGetValue(nextPos, out selected))
                {
                    // then we go there.
                    cursorPos = nextPos;

                    // If we can select the item, we return.
                    if (selected.SelectionType == GuiMemberSelectionType.Selectable)
                        return;
                }
                else
                {
                    // If it doesn't exist, we need to find one that does.

                    // If we were moving along Y, and we're in a layout with
                    // multiple columns,
                    if (move.Y != 0 && layoutMode == GuiLayoutMode.MultipleColumns)
                    {
                        // we may be able to find a viable option with the same
                        // Y coordinate but with a different X.
                        for (int x = 0; x < dimensions.X; ++x)
                        {
                            IntVector2 nextPosDifferentX = new IntVector2(x, nextPos.Y);
                            if (members.TryGetValue(nextPosDifferentX, out selected))
                            {
                                // If we find one, we go there.
                                cursorPos = nextPosDifferentX;

                                // If it's also selectable, we return.
                                if (selected.SelectionType == GuiMemberSelectionType.Selectable)
                                    return;
                            }
                        }
                    }

                    // else we just continue the loop.
                    cursorPos = nextPos;
                }

                nextPos = nextPosition(move);
                if (++loopCount > 1000)
                {
                    throw new EndlessLoopException("Gui Layout Traverse");
                }
            }
        }

        IntVector2 nextPosition(IntVector2 move)
        {
            IntVector2 nextPos = cursorPos + move;

            if (dimensions.IntersectTilePoint(nextPos) == false)
            {
                // If we're at the scrollbar
                //if (selected is GuiScrollbar)
                //{
                //    // We will want to go back to the top
                //    nextPos = IntVector2.Zero;
                //    move = new IntVector2(0, 1);
                //}
                //else
                //{
                    // If we would move and leave but do not have a parent, we just wrap back along the axis 
                    // we were to move about along
                    nextPos = Wrap(move);
                //}
            }

            return nextPos;
        }
        

        IntVector2 Wrap(IntVector2 move)
        {
            IntVector2 result = cursorPos;

            if (move.X > 0)
            {
                // if we were moving to the right, we wrap back to the left
                result.X = 0;
            }
            else if (move.X < 0)
            {
                // if moving L, wrap back to R
                result.X = dimensions.Width - 1; // note that we're in an array-like grid
            }

            // same logic applies along the Y axis
            if (move.Y > 0)
            {
                result.Y = 0;
            }
            else if (move.Y < 0)
            {
                result.Y = dimensions.Height - 1;
            }

            return result;
        }

        public List<GuiMember> GetMembers()
        {
            List<GuiMember> result = new List<GuiMember>();
            foreach (GuiMember mem in members.Values)
            {
                result.Add(mem);
            }
            return result;
        }

        #endregion

        public void Kill()
        {
            new TargetFade(renderList, 0, gui.style.fadeTimeMS).OnComplete += DeleteMe;
            new TargetFade(scrollerRenderer, 0, gui.style.fadeTimeMS);
        }
        public bool IsDeleted
        {
            get { return renderList == null; }
        }
        public void DeleteMe()
        {
            renderList.DeleteMe();
            renderList = null;
            scrollerRenderer.DeleteMe();

            if (headbarImages != null)
            {
                headbarImages.DeleteAll();

                if (returnButton != null) returnButton.DeleteMe();

                if (closeMenuButton != null) closeMenuButton.DeleteMe();
            }
            
            if (OnDelete != null)
            {
                OnDelete();
                OnDelete = null;
            }

            foreach (KeyValuePair<IntVector2, GuiMember> keyVal in members)
            {
                keyVal.Value.DeleteMe();
            }
        }

        public void MemberHeightUpdate(GuiMember guiMember, float changeAmount)
        {
            IntVector2 key = IntVector2.NegativeOne;

            // Find member key
            foreach (KeyValuePair<IntVector2, GuiMember> pair in members)
            {
                if (pair.Value == guiMember)
                    key = pair.Key;
            }

            if (key == IntVector2.NegativeOne)
            {
                return; // could not find it, so we don't have to worry - it's not in the layout. (this prolly won't happen though)
            }

            foreach (KeyValuePair<IntVector2, GuiMember> pair in members)
            {
                // Now for every member with a y position bigger than the one which changed,
                // we need to move it down by the change amount.
                if (pair.Key.Y > key.Y)
                {
                    pair.Value.ParentY += changeAmount;
                }
            }

            // Also, our total height changed just as much,
            totalHeight += changeAmount;

            // and therefore our background should too.
            background.Size = new Vector2(background.Size.X, totalHeight);
        }

        public void TryDoRefreshAction()
        {
            if (refreshAction != null)
                refreshAction();

            foreach (KeyValuePair<IntVector2, GuiMember> keyVal in members)
            {
                keyVal.Value.Refresh(null);
            }
        }
    }


    
}
