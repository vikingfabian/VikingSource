using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Input;
using VikingEngine.Input;
using VikingEngine.Engine;

namespace VikingEngine.HUD
{
    class Gui
    {
        public static void LoadContent()
        {
            string soundDir =
#if PJ
                PJ.PjLib.ContentFolder;
#elif TOGG
                ToGG.toggLib.ContentFolder;
#elif DSS
                ToGG.toggLib.ContentFolder;
#else
                LootFest.LfLib.ContentFolder + "Sound\\NEW\\";
#endif
            Engine.LoadContent.LoadSound(LoadedSound.MenuLo100MS, soundDir + "lo_100ms");
            Engine.LoadContent.LoadSound(LoadedSound.MenuHi100MS, soundDir + "hi_100ms");
        }

        public GuiStyle style;
        public VectorRect area;
        public ImageLayers layer;

        Input.ThumbStick moveinput;
        public Stack<GuiLayout> layoutStack;

        List<GuiOverlay> overlays;
        public InputSource inputSource;
        public MenuInputMap inputmap;

        bool inputBlocked;
        public bool useAnyControllerInput = false;
        public float soundVolume = 0f;

        public Gui(GuiStyle style, VectorRect safeArea, float leftAlignPerc, ImageLayers layer, InputSource inputSource)
        {
            this.inputSource = inputSource;
            inputmap = new MenuInputMap();
       
            if (inputSource.sourceType == InputSourceType.XController)
            {
                inputmap.xboxSetup(inputSource.controllerIndex);
            }
            else
            {
                inputmap.keyboardSetup();
            }

            this.style = style;
            this.layer = layer;
            area = safeArea;
            area.Width = style.layoutWidth;

            area.X = safeArea.X + safeArea.Width * leftAlignPerc - area.Width * 0.5f;

            if (area.X < safeArea.X)
            {
                area.X = safeArea.X;
            }
            else if (area.Right > safeArea.Right)
            {
                area.SetRight(safeArea.Right, true);
            }

            if (style.headBar)
            {
                area.AddToTopSide(-style.memberHeight);
            }

            layoutStack = new Stack<GuiLayout>();
            overlays = new List<GuiOverlay>();

            overlays.Add(new GuiTooltipOverlay(inputmap, style, 0));
            moveinput = new ThumbStick(ThumbStickType.NUM_NON, -1);
            inputBlocked = false;

            soundVolume = 1f;
        }

        public void PushLayout(GuiLayout layout)
        {
            if (layoutStack.Count > 0)
            {
                style.openSound?.Play();//.PlayFlat(soundVolume);
            }
            layoutStack.Push(layout);
            UpdateLayoutFadings();
        }

        public void PopLayout()
        {
            if (layoutStack.Count > 0)
            {
                releaseInput();
                GuiLayout layout = layoutStack.Pop();

                if (layout != null)
                    layout.Kill();

                UpdateLayoutFadings();

                if (layoutStack.Count != 0)
                {
                    layout = layoutStack.Peek();
                }

                style.closeSound?.Play();//.PlayFlat(soundVolume);
                layout.TryDoRefreshAction();
            }
        }

        public void PopLayouts(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                PopLayout();
            }
        }

        public GuiLayout PeekLayout()
        {
            if (layoutStack.Count == 0)
                return null;
            
            return layoutStack.Peek();
        }

        public void PopAllLayouts()
        {
            while (layoutStack.Count != 0)
            {
                PopLayout();
            }
        }

        void UpdateLayoutFadings()
        {
            float lay = GraphicsLib.ToPaintLayer(layer);

            GuiLayout[] layouts = layoutStack.ToArray();
            for (int i = 0; i < layouts.Length; ++i)
            {
                GuiLayout layout = layouts[i];
                layout.UpdateFading(Vector2.Zero, i == 0 ? 1 : (float)Math.Pow(4, -i));

                layout.SetPaintLayer(lay + PublicConstants.LayerMinDiff * (i * 10));
            }
        }

        Vector2 moveInput()
        {
            if (useAnyControllerInput)
            {
                foreach (var ins in Input.XInput.controllers)
                {
                    if (ins.Connected && ins.bLeftStick)
                    {
                        return ins.JoyStickValue(ThumbStickType.Left).Direction;
                    }
                }
            }
            return inputmap.movement.direction;
        }

        bool tabUpInput()
        {
            if (useAnyControllerInput)
            {
                if (Input.XInput.KeyDownEvent(Buttons.LeftShoulder) ||
                    Input.XInput.KeyDownEvent(Buttons.LeftTrigger))
                {
                    return true;
                }

            }
            return inputmap.tabLeftUp.DownEvent;
        }
        bool tabDownInput()
        {
            if (useAnyControllerInput)
            {
                if (Input.XInput.KeyDownEvent(Buttons.RightShoulder) ||
                    Input.XInput.KeyDownEvent(Buttons.RightTrigger))
                {
                    return true;
                }

            }
            return inputmap.tabRightDown.DownEvent;
        }
        
        bool selectInput()
        {
            if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
            {
                return false;
            }

            if (useAnyControllerInput)
            {
                if (Input.XInput.KeyDownEvent(Buttons.A) ||
                    Input.XInput.KeyDownEvent(Buttons.X))
                {
                    return true;
                }

            }

            return inputmap.click.DownEvent;
        }

        bool backInput()
        {
            if (useAnyControllerInput)
            {
                if (Input.XInput.KeyDownEvent(Buttons.B))
                {
                    return true;
                }

            }

            return inputmap.back.DownEvent;
        }

        GuiMember downInput = null;

        /// <summary>
        /// Returns true if menu is to be closed
        /// </summary>
        public bool Update()
        {
            if (layoutStack.Count == 0)
            {
                return true;
            }

            if (!inputBlocked)
            {
                GuiLayout layout = PeekLayout();

                GuiMember prevSelected = layout.Selected;
                float scrollPos = layout.renderList.Transform2D.Y;

                moveSelection(moveInput());
                if (tabUpInput())
                {
                    layout.PageUpDown(true);
                }
                if (tabDownInput())
                {
                    layout.PageUpDown(false);
                }
                if (prevSelected != layout.Selected && layout.Selected != null)
                {
                    layout.KeepMemberInView(layout.Selected, style.percentageOfViewToEdgeWhenScrolling);
                }
                updateMouseOver();
                if (prevSelected != layout.Selected && layout.Selected != null)
                {
                    layout.KeepMemberInView(layout.Selected, 0f);
                }

                layout.UpdateScrolling(inputmap.scroll.directionAndTime);
                if (layout.renderList.Transform2D.Y != scrollPos) // Scrolling changed
                {
                    RefreshAllMembers(null);
                }

                foreach (GuiMember m in layoutStack.Peek().GetMembers())
                {
                    if (m == layout.Selected)
                        m.IsHovered = true;
                    else
                        m.IsHovered = false;

                    m.update();
                }

                if (layout.Selected != null)
                {
                    if (Input.Mouse.ButtonDownEvent(MouseButton.Left) && layout.Selected.GetVisibleRect().IntersectPoint(Input.Mouse.Position))
                    {
                        downInput = layout.Selected;
                        downInput.IsPressed = true;
                    }
                    else if (Input.Mouse.ButtonUpEvent(MouseButton.Left))
                    {
                        //layout.Selected.IsPressed = false;
                        releaseInput();
                    }
                    else if (selectInput())
                    {
                        layout.Selected.IsPressed = !layout.Selected.IsPressed;
                    }
                }

                if (backInput())
                {
                    if (layout.Selected != null && layout.Selected.IsPressed)
                        layout.Selected.IsPressed = false;
                    else
                    {
                        if (tryPopLayout())
                        {
                            return true;
                        }
                    }
                }

                //Man ska bara kunna stänga menyn utifrån sett
                if (inputmap.openCloseInputEvent())
                {
                    if (tryPopLayout())
                    {
                        return true;
                    }
                }

                if (layout.returnButton != null && layout.returnButton.update())
                {
                    PopLayout();
                }
                if (layout.closeMenuButton != null && layout.closeMenuButton.update())
                {
                    return true;
                }

                foreach (GuiOverlay overlay in overlays)
                {
                    overlay.Update(
                        (prevSelected != layout.Selected) || // the selection changed if either the selected member
                        (layoutStack.Count == 0 || layout != layoutStack.Peek()) || // or the layout changed
                        (scrollPos != layout.renderList.Transform2D.Y), // or we scrolled the view
                        layout.Selected);
                }
            }

            return false;
        }

        void releaseInput()
        {
            if (downInput != null)
            {
                downInput.IsPressed = false;
                downInput = null;
            }
        }

        bool tryPopLayout()
        {
            if (layoutStack.Count > 1)
            {
                PopLayout();
                return false;
            }
            return true;
        }

        public void BlockInput()
        {
            inputBlocked = true;
        }

        public void ReenableInput()
        {
            inputBlocked = false;
        }

        public int PageId 
        {
            //Ifall en ny sida laddas asynch, och man vill veta att spelaren inte hoppat vidare
            get
            {
                if (layoutStack.Count == 0)
                {
                    return int.MinValue;
                }
                return layoutStack.Peek().Id;
            } 
        }

        public void RefreshAllMembers(GuiMember sender)
        {
            if (layoutStack.Count != 0)
            {
                foreach (GuiLayout lay in layoutStack)
                {
                    foreach (GuiMember mem in lay.GetMembers())
                    {
                        mem.Refresh(sender);
                    }
                }
            }
        }

        void updateMouseOver()
        {
            if (PlatformSettings.RunningWindows)
            {
                if (Input.Mouse.MoveDistance != Vector2.Zero) // allows for key movement in menu, even if mouse is over
                {
                    VectorRect area = VectorRect.Zero;

                    GuiLayout layout = layoutStack.Peek();

                    if (layout.Selected != null && layout.Selected.IsPressed)
                    {
                        layout.Selected.OnMouseDrag();
                    }
                    else
                    {
                        foreach (GuiMember mem in layout.GetMembers())
                        {
                            area = mem.GetVisibleRect();
                            if (area.IntersectPoint(Input.Mouse.Position))
                            {
                                if (mem.SelectionType != GuiMemberSelectionType.None)
                                {
                                    layout.Selected = mem;
                                }
                                return;
                            }
                        }
                    }
                }
            }
        }

        void moveSelection(Vector2 direction)
        {
            moveinput.UpdateStepBuffer(direction);

            if (VectorExt.HasValue(direction))
            {

                if (direction.Length() < 0.7f)
                    direction = Vector2.Zero;

                if (direction == Vector2.Zero)
                {
                    direction += Input.PlayerInputMap.arrowKeys.direction;
                }

                if (Input.Keyboard.Ctrl)
                {
                    direction.X *= 0.1f;
                }

                GuiLayout layout = layoutStack.Peek();

                layout.Move(moveinput);
            }
        }

        public bool IsDeleted { get; private set; }
        public void DeleteMe()
        {
            foreach (GuiOverlay overlay in overlays)
            {
                overlay.DeleteMe();
            }
            PopAllLayouts();
            IsDeleted = true;
        }

        public bool Visible { get { return !IsDeleted; } }
    }

    
}
