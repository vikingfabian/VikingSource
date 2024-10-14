using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars.Display;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;

namespace VikingEngine.HUD.RichBox
{
    interface IRichboxGuiInputMap
    {
        IButtonMap RichboxGuiSelect { get; }
        IntVector2 RichboxGuiMove();
        bool RichboxGuiUseMove { get; }
    }

    struct RichboxGuiSettings
    {
        public Color bgCol;
        public float bgAlpha;
        public float edgeWidth;
        public float width;
        public ImageLayers contentLayer;
        public ImageLayers bglayer;
        public RichBoxSettings RbSettings;       
    }
    class RichboxGui
    {
        public IRichboxGuiInputMap input;
        public RichboxGuiSettings settings;
        public List<RichboxGuiPart> parts= new List<RichboxGuiPart>();

        public List<string> menuState = new List<string>();
        public bool menuStateHasChange = false;

        public string CurrentMenuState => menuState.LastOrDefault();
        public int movePos_part = -1;
        public IntVector2 movePos_grid = IntVector2.Zero;
        public int lockInput = 0;

        public RichboxGui()
        { }

        public RichboxGui(RichboxGuiSettings settings, IRichboxGuiInputMap input)
        {
            this.input = input;
            this.settings = settings;
        }

        public void DeleteMe()
        {
            foreach (var p in parts)
            {
                p.DeleteMe();
            }
        }

        public void clearState()
        {
            menuState.Clear();
            menuStateHasChange = true;
        }

        public bool update()
        {
            bool interaction = false;
            foreach (var p in parts)
            {
                interaction |= p.update();
            }

            return interaction;
        }

        public void beginMove(int part)
        {
            if (input.RichboxGuiUseMove)
            {
                lockInput = 2;
                movePos_part = part;
                movePos_grid = IntVector2.Zero;

                if (parts[movePos_part].canMoveInteract())
                {
                    parts[movePos_part].interaction.hover = parts[movePos_part].richBox.buttonGrid_Y_X[movePos_grid.Y][movePos_grid.X];
                    parts[movePos_part].interaction.refreshSelectOutline();
                }
            }
        }

        public void updateMove(out bool refresh)
        {
            refresh = false;
            if (input.RichboxGuiUseMove && lockInput <= 0)
            {
                if (movePos_part >= 0 && parts[movePos_part].canMoveInteract())
                {
                    IntVector2 prevGrid = movePos_grid;
                    int prevPart = movePos_part;

                    IntVector2 move = input.RichboxGuiMove();
                    
                    if (move.Y != 0)
                    {
                        movePos_grid.Y += move.Y;
                        if (movePos_grid.Y < 0)
                        {
                            do
                            {
                                movePos_part--;
                                if (movePos_part < 0)
                                {
                                    movePos_part = parts.Count - 1;
                                }
                            } while (parts[movePos_part].canMoveInteract() == false);

                            movePos_grid.Y = parts[movePos_part].richBox.buttonGrid_Y_X.Count - 1;
                        }
                        else if (movePos_grid.Y >= parts[movePos_part].richBox.buttonGrid_Y_X.Count)
                        {
                            do
                            {
                                movePos_part++;
                                if (movePos_part >= parts.Count)
                                {
                                    movePos_part = 0;
                                }
                            } while (parts[movePos_part].canMoveInteract() == false);

                            movePos_grid.Y = 0;
                        }

                        var line = parts[movePos_part].richBox.buttonGrid_Y_X[movePos_grid.Y];
                        movePos_grid.X = Bound.Max(movePos_grid.X, line.Count - 1);
                        
                        parts[movePos_part].interaction.hover = line[movePos_grid.X];

                    }
                    else if (move.X != 0)
                    {
                        movePos_grid.X += move.X;
                        var line = parts[movePos_part].richBox.buttonGrid_Y_X[movePos_grid.Y];
                        movePos_grid.X = Bound.SetRollover(movePos_grid.X, 0, line.Count - 1);
                    }

                    if (prevGrid != movePos_grid || prevPart != movePos_part)
                    {
                        //Finalize movement
                        parts[movePos_part].interaction.hover = parts[movePos_part].richBox.buttonGrid_Y_X[movePos_grid.Y][movePos_grid.X];
                        parts[movePos_part].interaction.refreshSelectOutline();

                        if (prevPart != movePos_part)
                        {
                            parts[prevPart].interaction.clearSelection();
                        }
                    }

                    //if (XInput.controllers[0].IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A))
                    //{
                    //    lib.DoNothing();
                    //}

                    if (movePos_part >= 0 && input.RichboxGuiSelect.DownEvent)
                    {
                        parts[movePos_part].interaction.hover.onClick();
                        refresh = true;
                    }
                }
            }
            --lockInput;
        }

        public Vector2 controllerSelectionPos()
        {
            return parts[movePos_part].interaction.hover.area().Position;
        }

        public void clearMoveSelection()
        {
            if (movePos_part >= 0 && arraylib.InBound(parts, movePos_part) && parts[movePos_part].interaction != null)
            {
                parts[movePos_part].interaction.clearSelection();
            }
            movePos_part = -1;
        }

        public void onRefresh(RichboxGuiPart part)
        {
            if (input.RichboxGuiUseMove)
            {
                if (movePos_part >= 0 && part == parts[movePos_part])
                {
                    if (parts[movePos_part].richBox.buttonGrid_Y_X.Count == 0)
                    {
                        movePos_part = -1;
                    }
                    else if (menuStateHasChange)
                    {
                        menuMoveRefreshOnStateChange();
                    }
                    else
                    {
                        var menuPart = parts[movePos_part];
                        var interaction = menuPart.interaction;
                        if (interaction != null)
                        {
                            if (Bound.SetToArray(ref movePos_grid.Y, menuPart.richBox.buttonGrid_Y_X.Count))
                            {
                                if (Bound.SetToArray(ref movePos_grid.X, menuPart.richBox.buttonGrid_Y_X[movePos_grid.Y].Count))
                                {
                                    interaction.hover =  menuPart.richBox.buttonGrid_Y_X[movePos_grid.Y][movePos_grid.X];
                                }
                            }
                            
                            interaction.refreshSelectOutline();
                        }
                    }
                }
            }
        }

        public void SetMenuState(string state)
        {
            menuState.Add(state);
            menuStateHasChange = true;
        }

        public bool HasMenuState(string state)
        { 
            return menuState.Contains(state);
        }
        public void menuBack()
        {
            arraylib.RemoveLast(menuState);
            menuStateHasChange = true;
        }

        void menuMoveRefreshOnStateChange()
        {
            if (input.RichboxGuiUseMove && movePos_part>=0)
            {
                beginMove(movePos_part);
            }
        }

        public bool mouseOver()
        {
            foreach (var p in parts)
            {
                if (p.mouseOver())
                { 
                    return true;
                }
            }

            return false;
        }

        public bool hasInteractButtonHover()
        {
            foreach (var p in parts)
            {
                if (p.interaction != null && p.interaction.hover != null)
                {
                    return true;
                }
            }

            return false;
        }
    }

    class RichboxGuiPart
    {
        public RichBoxGroup richBox;
        protected RichBoxContent content = new RichBoxContent();
        protected Graphics.Image bg;
        public VectorRect area;
        public RbInteraction interaction = null;
        protected RichboxGui gui;
        Graphics.RectangleLines outLine;
        int index;

        public RichboxGuiPart(RichboxGui gui)
        {
            this.gui = gui;
            bg = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.Zero, gui.settings.bglayer);
            bg.ColorAndAlpha(gui.settings.bgCol, gui.settings.bgAlpha);
        }

        public void DeleteMe()
        {
            bg.DeleteMe();
            beginRefresh();
        }

        public bool update()
        {
            if (interaction != null)
            {
                return interaction.update();
            }
            return false;
        }

        public void viewOutLine(bool view)
        {
            if (view)
            {
                if (outLine == null)
                {
                    outLine = new Graphics.RectangleLines(VectorRect.ZeroOne, 4f, 0, gui.settings.contentLayer);
                    outLine.Visible = false;
                }
            }
            else
            {
                outLine?.DeleteMe();
                outLine = null;
            }
        }

        protected void beginRefresh()
        {
            richBox?.DeleteAll();
            interaction?.DeleteMe();
            content.Clear();
        }

        protected void endRefresh(Vector2 position, bool interact)
        {
            richBox = new RichBoxGroup(Vector2.Zero,
                    gui.settings.width, gui.settings.contentLayer, gui.settings.RbSettings, content, true, true, false);

            area = richBox.area;

            area.AddRadius(gui.settings.edgeWidth);
            area.Position = position;
            richBox.Move(area.Position + new Vector2(gui.settings.edgeWidth));

            
            bg.Area = area;

            //bottomLeft = area.LeftBottom;
            //topRight = area.RightTop;

            if (interact)
            {
                interaction = new RbInteraction(content, gui.settings.contentLayer,
                    gui.input.RichboxGuiSelect);
            }

            if (outLine != null)
            {
                bg.Opacity = 1f;
                outLine.SetRectangle(bg.Area);
            }
            else
            {
                bg.Opacity = gui.settings.bgAlpha;
            }

            gui.onRefresh(this);
        }

        virtual public void setVisible(bool visible)
        {
            if (visible != bg.Visible)
            {
                bg.Visible = visible;
                richBox?.SetVisible(visible);
            }

            if (outLine != null)
            {
                outLine.Visible = visible;
            }

            if (!visible)
            {
                interaction?.DeleteMe();
                interaction = null;
            } 
        }

        public bool GetVisible()
        {
            return bg.Visible;
        }

        public bool mouseOver()
        {
            return bg.Visible && bg.Area.IntersectPoint(Input.Mouse.Position);
        }

        protected Color? negativeRed(int value)
        {
            if (value < 0)
            {
                return Color.Red;
            }
            else
            {
                return null;
            }
        }

        public bool canMoveInteract()
        {
            if (interaction != null)
            {
                var grid = richBox.buttonGrid_Y_X;
                if (grid.Count > 0 && grid[0].Count > 0)
                {
                    return true;
                }
            }

            return false;   
        }
    }
}
