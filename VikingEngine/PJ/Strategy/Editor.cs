using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VikingEngine.HUD;

namespace VikingEngine.PJ.Strategy
{
    class Editor : AbsStrategyState
    {
        Graphics.Image selectionFrame;
        
        MenuSystem menusystem = null;

        VikingEngine.Input.DirectionalButtonsMap moveInput = VikingEngine.Input.PlayerInputMap.arrowKeys;
        Graphics.TextG selectionInfo;

        public Editor()
            : base(true)
        {
            new Map();
            selectionFrame = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, new Vector2(36), ImageLayers.Background3, true);

            StrategyLib.SetHudLayer();
            selectionInfo = new Graphics.TextG(LoadedFont.Console, Engine.Screen.SafeArea.CenterTop, new Vector2(1f),
                Graphics.Align.CenterAll, "XX", Color.White, ImageLayers.Top3);
            StrategyLib.SetMapLayer();
            
        }

        //protected override void createDrawManager()
        //{
        //    draw = new VikingEngine.PJ.Strategy.Draw();
        //}

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
           

            if (menusystem != null)
            {
                if (menusystem.menu.Update() || Input.Keyboard.KeyDownEvent(Keys.Escape))
                {
                    CloseMenu();
                }
                return;
            }

            if (StrategyRef.map.isLoaded)
            {
                if (Input.Mouse.IsButtonDown(MouseButton.Right))
                {
                    StrategyRef.map.cameraPos += Input.Mouse.MoveDistance;
                    draw.Set2DTranslation(0, StrategyRef.map.cameraPos);
                }

                if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    if (Input.Keyboard.IsKeyDown(Keys.LeftControl))
                    {
                        //Connect adjacant areas
                        MapArea adjacentArea = StrategyRef.map.pointingOnArea();

                        if (StrategyRef.map.selectedArea != null &&
                            adjacentArea != null && adjacentArea != StrategyRef.map.selectedArea)
                        {
                            StrategyRef.map.selectedArea.connectToArea(adjacentArea, StrategyRef.map.selectedArea);
                        }
                    }
                    else
                    {
                        //Kolla val av pil
                        if (StrategyRef.map.selectedArea != null)
                        {
                            foreach (var m in StrategyRef.map.selectedArea.adjacentAreas)
                            {
                                if (m.arrowArea.IntersectPoint(StrategyRef.map.pointerPos))
                                {
                                    select(m);
                                    return;
                                }
                            }
                        }

                        //Kolla först selection
                        MapArea sel = StrategyRef.map.pointingOnArea();

                        if (sel == null)
                        {
                            //Create area center
                            var newArea = new MapArea(StrategyRef.map.pointerPos);
                            StrategyRef.map.areas.Add(newArea);
                            select(newArea);
                        }
                        else
                        {
                            select(sel);
                            //selectedArea = sel;
                        }

                        
                    }
                }

                if (Input.Keyboard.KeyDownEvent(Keys.Space))
                {
                    if (StrategyRef.map.selectedArea != null)
                    {
                        if (StrategyRef.map.selectedConnection == null)
                        {
                            StrategyRef.map.selectedArea.nextStartPrio();
                            refreshSelection();
                        }
                        else
                        {
                            StrategyRef.map.selectedConnection.nextConnectionType();
                        }
                    }
                }

                if (Input.Keyboard.KeyDownEvent(Keys.Delete))
                {
                    if (StrategyRef.map.selectedArea != null)
                    {
                        StrategyRef.map.selectedArea.DeleteMe();
                        StrategyRef.map.areas.Remove(StrategyRef.map.selectedArea);
                        unselect();
                        refreshSelection();
                    }
                }


                if (!moveInput.stepping.IsZero() && StrategyRef.map.selectedArea != null)
                {
                    IntVector2 move = moveInput.stepping;
                    if (Input.Keyboard.Shift)
                    {
                        move *= 10;
                    }

                    if (StrategyRef.map.selectedConnection == null)
                    {
                        StrategyRef.map.selectedArea.fineAdjust(move, StrategyRef.map.selectedArea);
                    }
                    else
                    {
                        StrategyRef.map.selectedConnection.fineAdjust(move);
                    }
                }

                float rotateInput = -lib.BoolToInt01(Input.Keyboard.KeyDownEvent(Keys.D1)) + 
                    lib.BoolToInt01(Input.Keyboard.KeyDownEvent(Keys.D2));
                if (rotateInput != 0 && StrategyRef.map.selectedConnection != null)
                {
                    rotateInput *= 0.04f;

                    if (Input.Keyboard.Shift)
                    {
                        rotateInput *= 10;
                    }
                    StrategyRef.map.selectedConnection.fineAdjustRot(rotateInput);
                }

                if (Input.Keyboard.KeyDownEvent(Keys.Escape))
                {
                    openMenu(Input.InputSource.DefaultPC);
                }
            }
        }

        void unselect()
        {
            if (StrategyRef.map.selectedArea != null)
            {
                foreach (var m in StrategyRef.map.selectedArea.adjacentAreas)
                {
                    m.arrow.Visible = false;
                }
            }
            StrategyRef.map.selectedArea = null;
            StrategyRef.map.selectedConnection = null;
            refreshSelection();
        }

        void select(MapArea area)
        {
            if (area != StrategyRef.map.selectedArea)
            {
                unselect();
                StrategyRef.map.selectedArea = area;
                foreach (var m in StrategyRef.map.selectedArea.adjacentAreas)
                {
                    m.arrow.Visible = true;
                }
                refreshSelection();
            }
        }

        void select(AreaConnection connection)
        {
            select(connection.fromArea);
            StrategyRef.map.selectedConnection = connection;
            refreshSelection();
        }

        public void CloseMenu()
        {
            StrategyLib.SetHudLayer();
            if (menusystem != null)
            {
                menusystem.DeleteMe();
                menusystem = null;
            }
            StrategyLib.SetMapLayer();
        }

        public void openMenu(Input.InputSource inputSource)
        {
            StrategyLib.SetHudLayer();
            menusystem = new MenuSystem(inputSource);

            GuiLayout layout = new GuiLayout("Editor menu", menusystem.menu);
            {
                new GuiTextButton("Save", null, saveLink, false, layout);
                new GuiTextButton("Clear area connections", null, clearAreaConnections, false, layout);
            }
            layout.End();

            StrategyLib.SetMapLayer();
        }

        void clearAreaConnections()
        {
            if (StrategyRef.map.selectedArea != null)
            {
                StrategyRef.map.selectedArea.clearConnections(StrategyRef.map.selectedArea);
            }
            CloseMenu();
        }

        void saveLink()
        {
            StrategyRef.map.saveload(true);
            CloseMenu();
        }

        void refreshSelection()
        {
            if (StrategyRef.map.selectedArea == null)
            {
                selectionFrame.Visible = false;
                selectionInfo.TextString = TextLib.EmptyString;
            }
            else
            {
                if (StrategyRef.map.selectedConnection == null)
                {
                    selectionFrame.Position = StrategyRef.map.selectedArea.center;
                }
                else
                {
                    selectionFrame.Position = StrategyRef.map.selectedConnection.arrowPos;
                }
                selectionFrame.Visible = true;
                selectionInfo.TextString = StrategyRef.map.selectedArea.ToString();
            }
        }
    }

    
    

}
