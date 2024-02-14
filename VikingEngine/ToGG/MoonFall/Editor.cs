using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.MoonFall
{
    class Editor
    {
        MapNode selectedNode = null;
        MapNode dragNode = null;
        //MapArea selectedArea = null;
        MapArea connectFrom = null;
        VikingEngine.Input.DirectionalButtonsMap moveInput = VikingEngine.Input.PlayerInputMap.arrowKeys;
        Graphics.Image selectionSquare;

        public Editor()
        {
            selectionSquare = new Graphics.Image(SpriteName.WhiteArea, 
                Vector2.Zero, Vector2.One, ImageLayers.AbsoluteBottomLayer);
            select(null);

            Graphics.Text2 inputInfo = new Graphics.Text2(
                "Ctrl+click: add area" + Environment.NewLine +
                "Del: remove" + Environment.NewLine +
                "Alt+click: connect areas",
                 LoadedFont.Regular, Engine.Screen.SafeArea.Position,
                 Engine.Screen.TextBreadHeight,
                 Color.White, ImageLayers.Top0);
            
        }

        void select(MapNode select)
        {
            selectedNode = select;
            if (selectedNode == null)
            {
                selectionSquare.Visible = false;
            }
            else
            {
                selectionSquare.Visible = true;
                selectionSquare.Area = select.selectionArea;
                selectionSquare.LayerAbove(select.image);
            }
        }

        public void update()
        {
            if (dragNode != null)
            {
                if (Input.Mouse.ButtonUpEvent(MouseButton.Left))
                {
                    dragNode = null;
                }
                else
                {
                    dragNode.move(Input.Mouse.MoveDistance);
                    select(dragNode);
                }

                return;
            }
            
            updateSelection();
            MapArea selArea = selectedNode as MapArea;

            if (Input.Mouse.IsButtonDown(MouseButton.Right))
            {
                moonRef.map.cameraPos += Input.Mouse.MoveDistance;
                Ref.draw.Set2DTranslation(0, moonRef.map.cameraPos);
            }

            if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
            {
                if (selArea == null)
                {
                    if (Input.Keyboard.IsKeyDown(Keys.LeftControl))
                    {
                        //Create area center
                        var newArea = new MapArea(moonRef.map.pointerPos);
                        moonRef.map.areas.Add(newArea);
                        select(newArea);
                        return;
                    }
                }
                else
                {
                    if (Input.Keyboard.IsKeyDown(Keys.LeftAlt))
                    {
                        if (connectFrom == null)
                        {
                            connectFrom = selArea;
                        }
                        else
                        {
                            selArea.connectToArea(connectFrom);
                            connectFrom = null;
                        }
                        return;
                    }
                }

                if (selectedNode != null)
                {
                    dragNode = selectedNode;
                }
            }
            
            if (Input.Keyboard.KeyDownEvent(Keys.Delete))
            {
                if (selectedNode != null)
                {
                    selectedNode.DeleteMe();
                    //moonRef.map.areas.Remove(moonRef.map.selectedArea);
                    selectedNode = null;
                }
            }

            if (!moveInput.stepping.IsZero() && selectedNode != null)
            {
                IntVector2 move = moveInput.stepping;
                if (Input.Keyboard.Shift)
                {
                    move *= 10;
                }

                //if (moonRef.map.selectedConnection == null)
                {
                    selectedNode.fineAdjust(move);
                }
                //else
                {
                    // moonRef.map.selectedConnection.fineAdjust(move);
                }
            }
        }

        private void updateSelection()
        {


            var sel = moonRef.map.pointingOnNode();

            //if (sel != null)
            //{
                select(sel);
            //}
            //else
            //{
            //     moonRef.map.areas
            //}
            //if (sel == null)
            //{
            //    //Create area center
            //    var newArea = new MapArea(moonRef.map.pointerPos);
            //    moonRef.map.areas.Add(newArea);
            //    select(newArea);
            //}
            //else
            //{

            //selectedArea = sel;
            //}
        }
    }
}
