using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf
{
    class Editor
    {
        Graphics.ImageGroup images = new Graphics.ImageGroup();
        Grid2D<VectorRect> cornerBounds;
        Graphics.Image cornerSelection;

        IntVector2 toolDownPos;
        Graphics.Line edgeTool = null;

        EditorMenu menusystem;
        public GolfEditorTool tool = GolfEditorTool.Edges;
        public int toolIndex = 0;
        IntVector2 selectedArea = IntVector2.Zero;
        Graphics.RectangleLines areaRectangle;

        Vector2 mouseDownPos;
        Graphics.Image paintArea = null;

        public Editor()
        {
            menusystem = new EditorMenu();
            areaRectangle = new Graphics.RectangleLines(VectorRect.Zero, 4, 1f, ImageLayers.Foreground0);

            cornerSelection = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.Zero, GolfLib.FieldEditorSelectionLayer, false);
            cornerSelection.Color = Color.Yellow;

            GolfRef.editor = this;
            GolfRef.objects.ClearItems();

            {//CORNERS
                cornerBounds = new Grid2D<VectorRect>(GolfRef.field.cornersCount);
                ForXYLoop loop = new ForXYLoop(GolfRef.field.cornersCount);
                while (loop.Next())
                {
                    Vector2 pos = GolfRef.field.squaresStart + GolfRef.field.squareSize * loop.Position.Vec;
                    Graphics.Image cornerImg = new Graphics.Image(SpriteName.WhiteArea, pos, GolfRef.field.squareSize * 0.1f, GolfLib.FieldEditorLayer, true);
                    images.Add(cornerImg);

                    cornerBounds.Set(loop.Position, VectorRect.FromCenterSize(pos, GolfRef.field.squareSize * 0.5f));
                }
            }

            {//SQUARES
                GolfRef.field.squares.LoopBegin();
                while (GolfRef.field.squares.LoopNext())
                {
                    Graphics.TextG text = new Graphics.TextG(LoadedFont.Regular,
                        GolfRef.field.toSquareScreenArea(GolfRef.field.squares.LoopPosition).Center,
                        Engine.Screen.TextSizeV2 * 0.6f, Graphics.Align.CenterAll, null, Color.White, GolfLib.FieldEditorLayer, true);
                    text.Rotation = MathHelper.PiOver4;
                    GolfRef.field.squares.LoopValueGet().editorText = text;
                }

                GolfRef.field.refreshSquareText();
            }
        }



        public void update()
        {
            if (menuUpdate()) return;

            switch (tool)
            {
                case GolfEditorTool.Edges:
                    paintEdgesUpdate();
                    break;
                case GolfEditorTool.Area:
                    paintAreaUpdate();
                    break;
                case GolfEditorTool.Choke:
                    paintAreaUpdate();
                    break;
                case GolfEditorTool.LaunchCannon:
                    paintCannonUpdate();
                    break;
            }
        }


        public void onMapLoaded()
        {
            GolfRef.field.refreshSquareText();
        }

        void numberKeyUpdate()
        {
            if (Input.Keyboard.KeyDownEvent(Keys.D0))
                toolIndex = 0;

            if (Input.Keyboard.KeyDownEvent(Keys.D1))
                toolIndex = 1;

            if (Input.Keyboard.KeyDownEvent(Keys.D2))
                toolIndex = 2;

            if (Input.Keyboard.KeyDownEvent(Keys.D3))
                toolIndex = 3;

            if (Input.Keyboard.KeyDownEvent(Keys.D4))
                toolIndex = 4;

            if (Input.Keyboard.KeyDownEvent(Keys.D5))
                toolIndex = 5;

            if (Input.Keyboard.KeyDownEvent(Keys.D6))
                toolIndex = 6;

            if (Input.Keyboard.KeyDownEvent(Keys.D7))
                toolIndex = 7;

            if (Input.Keyboard.KeyDownEvent(Keys.D8))
                toolIndex = 8;

            if (Input.Keyboard.KeyDownEvent(Keys.D9))
                toolIndex = 9;
        }

        bool menuUpdate()
        {
            if (menusystem.Open)
            {
                if (menusystem.Update())
                {
                    menusystem.CloseMenu();
                }
                return true;
            }

            if (Input.Keyboard.KeyDownEvent(Keys.Escape))
            {
                menusystem.main();
            }

            return false;
        }

        void paintAreaUpdate()
        {
            numberKeyUpdate();

            if (paintArea != null)
            {
                paintArea.Area = VectorRect.FromTwoPoints(mouseDownPos, Input.Mouse.Position);
            }

            if (updateAreaSelection())
            {
                if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    mouseDownPos = Input.Mouse.Position;
                    if (paintArea == null)
                    {
                        paintArea = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.Zero, ImageLayers.Foreground1);
                        paintArea.Opacity = 0.2f;
                    }
                }
                else if (Input.Mouse.ButtonUpEvent(MouseButton.Left))
                {
                    if (paintArea != null)
                    {
                        paintArea.DeleteMe();
                        paintArea = null;

                        paint(GolfRef.field.toSquarePos(mouseDownPos), GolfRef.field.toSquarePos(Input.Mouse.Position));
                    }
                }
            }
        }

        void paint(IntVector2 start, IntVector2 end)
        {
            if (start.X >= 0 && end.X >= 0)
            {
                if (tool == GolfEditorTool.Area)
                {
                    Rectangle2 rect = Rectangle2.FromTwoTilePoints(start, end);

                    ForXYLoop loop = new ForXYLoop(rect);
                    while (loop.Next())
                    {
                        paintSquare(loop.Position, FieldSquareType.Area);
                    }
                }
                else if (tool == GolfEditorTool.Choke)
                {
                    if (start != end)
                    {
                        paintSquare(start, FieldSquareType.ChokeStart);
                        paintSquare(end, FieldSquareType.ChokeEnd);

                    }
                }
            }

            GolfRef.field.refreshSquareText();
        }

        void paintCannonUpdate()
        {
            if (updateAreaSelection())
            {
                if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    paintSquare(GolfRef.field.toSquarePos(Input.Mouse.Position), FieldSquareType.LaunchCannon);
                    GolfRef.field.refreshSquareText();
                }
            }
        }

        void paintSquare(IntVector2 pos, FieldSquareType value)
        {
            var square = GolfRef.field.squares.Get(pos);
            if (square.type == value)
            {
                square.Clear();
            }
            else
            {
                square.type = value;
                square.typeIndex = toolIndex;
            }
        }

        bool updateAreaSelection()
        {
            IntVector2 area = GolfRef.field.toSquarePos(Input.Mouse.Position);
            if (area.X >= 0)
            {
                areaRectangle.Visible = true;
                areaRectangle.SetRectangle(GolfRef.field.toSquareScreenArea(area));
                return true;
            }
            else
            {
                areaRectangle.Visible = false;
                return false;
            }
        }

        void paintEdgesUpdate()
        {
            bool foundCorner = selectCornersUpdate();

            if (!foundCorner)
            {
                selectEdgeUpdate();
            }
        }

        bool selectCornersUpdate()
        {
            cornerSelection.Visible = false;
            cornerBounds.LoopBegin();

            while (cornerBounds.LoopNext())
            {
                if (cornerBounds.LoopValueGet().IntersectPoint(Input.Mouse.Position))
                {
                    cornerSelection.OrigoAtTopLeft();
                    cornerSelection.Visible = true;
                    cornerSelection.Rotation = 0;
                    cornerSelection.Position = cornerBounds.LoopValueGet().Position;
                    cornerSelection.Size = cornerBounds.LoopValueGet().Size;

                    if (edgeTool != null)
                    {
                        edgeTool.P2 = GolfRef.field.cornerPos(cornerBounds.LoopPosition);
                    }

                    if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                    {
                        if (edgeTool == null)
                        {
                            toolDownPos = cornerBounds.LoopPosition;
                            edgeTool = new Graphics.Line(3, ImageLayers.Foreground0, Color.White, GolfRef.field.cornerPos(toolDownPos), Input.Mouse.Position);

                        }
                        else
                        {
                            new FieldEdge(toolDownPos, cornerBounds.LoopPosition);
                            edgeTool.DeleteMe();
                            edgeTool = null;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        void selectEdgeUpdate()
        {
            foreach (var m in GolfRef.objects.edges)
            {
                if (m.bound.Intersect(Input.Mouse.Position))
                {
                    cornerSelection.OrigoAtCenter();
                    cornerSelection.Position = m.outlineImage.Position;
                    cornerSelection.Size = m.outlineImage.Size;
                    cornerSelection.Rotation = m.outlineImage.Rotation;
                    cornerSelection.Visible = true;

                    if (Input.Keyboard.KeyDownEvent(Keys.Delete))
                    {
                        m.DeleteMe();
                        GolfRef.objects.edges.Remove(m);
                    }

                    return;
                }
            }
        }       

        public void DeleteMe()
        {
            images.DeleteAll();
            cornerSelection.DeleteMe();

            {//SQUARES
                GolfRef.field.squares.LoopBegin();
                while (GolfRef.field.squares.LoopNext())
                {
                    GolfRef.field.squares.LoopValueGet().editorText.DeleteMe();
                }

                GolfRef.field.refreshSquareText();
            }
        }
    }
}
