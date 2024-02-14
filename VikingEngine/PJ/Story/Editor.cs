using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.HUD;

namespace VikingEngine.PJ.Story
{
    class Editor
    {
        IntVector2 selectedTile;
        IntVector2 mouseDownTile;
        Graphics.Mesh selection, rectangleStart;
        
        PaintTool tool;
        Graphics.TextS infoText;
        

        public Editor()
        {
            selection = new Graphics.Mesh(LoadedMesh.cube_repeating, Vector3.Zero, new Vector3( 0.4f),
                Graphics.TextureEffectType.Flat, SpriteName.WhiteArea, Color.White);
            rectangleStart = selection.CloneMe() as Graphics.Mesh;
            rectangleStart.Color = Color.Yellow;
            rectangleStart.Visible = false;

            infoText = new Graphics.TextS(LoadedFont.Console, Vector2.Zero, Vector2.One, Graphics.Align.Zero, "--", Color.Black, ImageLayers.Top1);
            Graphics.Image infoBg = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, new Vector2(Engine.Screen.Width, infoText.MeasureText().Y * 1.2f), ImageLayers.AbsoluteBottomLayer);
            infoBg.LayerBelow(infoText);

            setTool(PaintTool.Brush);
        }

        string toolInfo;

        public void update()
        {
            if (Input.Mouse.ButtonDownEvent(MouseButton.Right))
            {
                mainMenu();
            }

            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.E))
            {
                setTool(PaintTool.Eraser);
            }
            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.B))
            {
                setTool(PaintTool.Brush);
            }
            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.M))
            {
                setTool(PaintTool.Marqee);
            }

            cameraInput();

            updatePointer();

            int paintValue = tool == PaintTool.Eraser ? 0 : 1;

            if (Input.Keyboard.Shift || tool == PaintTool.Marqee)
            {
                if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    mouseDownTile = selectedTile;
                    rectangleStart.Visible = true;
                    rectangleStart.Position = storyLib.toWorldPos(mouseDownTile);

                }
                else if (Input.Mouse.ButtonUpEvent(MouseButton.Left))
                {
                    if (rectangleStart.Visible)
                    {
                        rectangleStart.Visible = false;
                        var rectArea = Rectangle2.FromTwoTilePoints(mouseDownTile, selectedTile);
                        paint(rectArea, paintValue);
                    }
                }
            }
            else
            {
                rectangleStart.Visible = false;

                if (Input.Mouse.IsButtonDown(MouseButton.Left))
                {
                    if (Input.Mouse.ButtonDownEvent(MouseButton.Left) ||
                        Input.Mouse.bMoveInput)
                    {


                        paint(new Rectangle2(selectedTile, IntVector2.One), paintValue);
                    }
                }
            }

            infoText.TextString = toolInfo + ", pos" + selectedTile.ToString();
        }

        void mainMenu()
        {
            var menu = storyRef.state.openMenu();

            GuiLayout layout = new GuiLayout("Options", menu);
            {
                new GuiLabel("Level " + storyRef.map.level.ToString(), layout);
                new GuiTextButton("Save", null, save, false, layout);
                //new GuiTextButton("Save segment", null, saveSegment, false, layout);
                //new GuiTextButton("Save As New", null, saveSegmentAsNew, false, layout);
                //new GuiTextButton("Load segment", null, listStoredSegments, false, layout);
                //new GuiDialogButton("Delete segment", null, new GuiAction(deleteSegment), false, layout);
                //new GuiTextButton("New segment", null, newSegment, false, layout);
                //new GuiOptionsList<SegmentHeadType>("Type", types, typeProperty, layout);
                //new GuiIntSlider(SpriteName.NO_IMAGE, "Chunk Size X", sizeXProperty, new IntervalF(2, AbsLevel.ChunkSize.X), false, layout);
                //new GuiIntSlider(SpriteName.NO_IMAGE, "Chunk Size Y", sizeYProperty, new IntervalF(2, AbsLevel.ChunkSize.Y), false, layout);
                //new GuiTextButton("Apply new Size", null, applyNewSize, false, layout);

                //new GuiTextButton("Rotate CW", null, rotateClockWise, false, layout);
                //new GuiTextButton("Rotate CC", null, rotateCounterClockWise, false, layout);
                //new GuiTextButton("Flip X", null, flipHori, false, layout);
                //new GuiTextButton("Flip Y", null, flipVerti, false, layout);

                //new GuiTextButton("Move Left", null, new GuiAction1Arg<Dir4>(moveMap, Dir4.W), false, layout);
                //new GuiTextButton("Move Right", null, new GuiAction1Arg<Dir4>(moveMap, Dir4.E), false, layout);
                //new GuiTextButton("Move Up", null, new GuiAction1Arg<Dir4>(moveMap, Dir4.N), false, layout);
                //new GuiTextButton("Move Down", null, new GuiAction1Arg<Dir4>(moveMap, Dir4.S), false, layout);
            }
            layout.End();
        }

        void save()
        {
            storyRef.map.SaveLoad(true, false);
            storyRef.state.closeMenu();
        }

        void setTool(PaintTool value)
        {
            tool = value;
            toolInfo = "Tool: " + tool.ToString();
            if (tool == PaintTool.Brush || tool == PaintTool.Eraser)
            {
                toolInfo += " +Shift: Rect";
            }
        }

        Rectangle2 prevPaintArea;
        int prevPaintValue;

        void paint(Rectangle2 area, int value)
        {
            if (area != prevPaintArea || value != prevPaintValue)
            {
                prevPaintArea = area;
                prevPaintValue = value;

                ForXYLoop loop = new ForXYLoop(area);

                while (loop.Next())
                {
                    storyRef.map.set(loop.Position, value);
                }

                storyRef.map.refreshChunks(area);
            }
        }

        private void cameraInput()
        {
            if (Input.PlayerInputMap.arrowKeys.direction != Vector2.Zero)
            {
                storyRef.state.CameraCenter += Input.PlayerInputMap.arrowKeys.direction * 16f *
                    Ref.DeltaTimeSec * new Vector2(1f, -1f);
            }
        }

        private void updatePointer()
        {
            //Vector3 mousePosition = storyRef.state.camera.CastRayInto3DPlane(Input.Mouse.Position, Engine.Draw.graphicsDeviceManager.GraphicsDevice.Viewport, new Plane(Vector3.UnitZ, 0));
            //selectedTile = storyLib.toGridPos(mousePosition);
            //selection.Position = storyLib.toWorldPos(selectedTile);
        }
    }
}
