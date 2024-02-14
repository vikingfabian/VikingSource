using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Input;
using VikingEngine.HUD;
using VikingEngine.DataStream;
using System.IO;
using VikingEngine.LootFest.BlockMap.Level;

namespace VikingEngine.LootFest.BlockMap
{
    class EditorState : Engine.GameState, IStreamIOCallback
    {
        static readonly Vector2 VisualTileSize = new Vector2(24);
        Vector2 visualTileOffset = Vector2.Zero;

        BlockMapSegment segment;
        Graphics.ImageGroup tileImages = new Graphics.ImageGroup();
        HUD.Gui menu = null;

        PaintType paintType = PaintType.BlockType;
        int paintUnderType = (int)MapBlockType.Open;
        byte paintDir = 0;
        byte paintIndex = 0;
        bool editDirAndIndex = true;

        IntVector2 mouseSqPos = new IntVector2(-1);
        Vector2 mouseDownPos;
        Graphics.Image rectangleArea;
        bool waitForMouseUp = false;
        bool loadingLock = false;
        Graphics.TextS squareInfo;

        public EditorState()
        {
            segment = new BlockMapSegment();
            refreshVisuals();
            rectangleArea = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.Zero, ImageLayers.Lay3);
            rectangleArea.Opacity = 0.2f;
            rectangleArea.Visible = false;
            squareInfo = new Graphics.TextS(LoadedFont.Console, Engine.Screen.Area.LeftBottom, Vector2.One,
                new Graphics.Align(Vector2.UnitY), "--", Color.White, ImageLayers.Lay8); 
        }

        void refreshVisuals()
        {
            tileImages.DeleteAll();

            Vector2 edge = new Vector2(1);
            Vector2 edgedSz = VisualTileSize - edge * 2f;

            segment.squares.LoopBegin();

            while (segment.squares.LoopNext())
            {
                BlockMapSquare square = segment.squares.LoopValueGet();

                Vector2 pos = segment.squares.LoopPosition.Vec * VisualTileSize + visualTileOffset;
                pos += edge;
                Graphics.Image tileImage = new Graphics.Image(SpriteName.WhiteArea, pos, edgedSz, ImageLayers.Background4);
                tileImages.Add(tileImage);
                
                tileImage.Color = square.typeColor();

                if (square.special != MapBlockSpecialType.None)
                {
                    string text;
                    switch (square.special)
                    {
                        case MapBlockSpecialType.Entrance: text = ((Dir4)square.specialDir).ToString(); break;
                        case MapBlockSpecialType.SpawnPos: text = "+"; break;
                        case MapBlockSpecialType.SpecialPoint: text = "o"; break;
                        case MapBlockSpecialType.TerrainModel: text = "T"; break;
                        case MapBlockSpecialType.SpecialModel: text = "M"; break;
                        case MapBlockSpecialType.Item: text = "i"; break;
                        case MapBlockSpecialType.Landmark: text = "!"; break;
                        default: text = "ERROR"; break;
                    }

                    text += square.specialIndex.ToString();

                    Graphics.TextG specialLetter = new Graphics.TextG(LoadedFont.Regular, tileImage.Center,
                        new Vector2(0.5f), Graphics.Align.CenterAll, text, Color.Yellow, ImageLayers.Background2);
                    tileImages.Add(specialLetter);

                }
            }
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (menu != null)
            {
                if (menu.Update())
                {
                    closeMenu();
                }
                if (Engine.XGuide.LocalHost.inputMap.menuInput.openCloseInputEvent())//.DownEvent(ButtonActionType.MenuReturnToGame) || Engine.XGuide.LocalHost.inputMap.openCloseMenuKeyInput())
                {
                    closeMenu();
                }
                return;
            }

            if (waitForMouseUp)
            {
                if (Input.Mouse.ButtonDownEvent(MouseButton.Left) || Input.Mouse.ButtonUpEvent(MouseButton.Left))
                {
                    waitForMouseUp = false;
                }
                else
                {
                    return;
                }
            }
            if (loadingLock)
            {
                return;
            }

            IntVector2 mouseSqPos_old = mouseSqPos;

            mouseSqPos = toSquarePos(Input.Mouse.Position);

            if (Input.Keyboard.Shift)
            {
                if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    mouseDownPos = Input.Mouse.Position;
                    rectangleArea.Visible = true;
                }
                else if (Input.Mouse.ButtonUpEvent(MouseButton.Left))
                {
                    //paint rect
                    rectangleArea.Visible = false;

                    VectorRect area = VectorRect.FromTwoPoints(mouseDownPos, Input.Mouse.Position);
                    ForXYLoop loop = new ForXYLoop(toSquarePos(area.Position), toSquarePos(area.RightBottom));

                    while (loop.Next())
                    {
                        paint(loop.Position);
                    }
                    refreshVisuals();
                }

                if (rectangleArea.Visible && Input.Mouse.bMoveInput)
                {
                    VectorRect area = VectorRect.FromTwoPoints(mouseDownPos, Input.Mouse.Position);
                    rectangleArea.Area = area;
                }
            }
            else
            {
                rectangleArea.Visible = false;
                if (mouseSqPos != mouseSqPos_old)
                {
                    onNewSquare();
                }

                if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    if (Input.Keyboard.Alt)
                    {
                        pick();
                    }
                    else
                    {
                        paint();
                    }
                }
            }

            if (Input.Mouse.ButtonDownEvent(MouseButton.Right))
            {
                editorMenu();
            }

            if (Input.PlayerInputMap.arrowKeys.stepping.HasValue())
            {
                visualTileOffset -= (Input.PlayerInputMap.arrowKeys.stepping * 8).Vec * VisualTileSize;
                refreshVisuals();
            }
            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Home))
            {
                visualTileOffset = Vector2.Zero;
                refreshVisuals();
            }
            //if (.IsZero == )
            //{
                
            //}

            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                optionsMenu();
            }
        }

        IntVector2 toSquarePos(Vector2 screenPos)
        {
            screenPos -= visualTileOffset;
            return new IntVector2((int)(screenPos.X / VisualTileSize.X), (int)(screenPos.Y / VisualTileSize.Y));
        }

        void editorMenu()
        {
            openMenu();
            GuiLayout layout = new GuiLayout("Paint Menu", menu);
            {
                new GuiLabel("Block type", layout);
                for (MapBlockType i = 0; i < MapBlockType.NUM; ++i)
                {
                    new GuiTextButton(i.ToString(), null, new GuiAction2Arg<PaintType, int>(setPaint, PaintType.BlockType, 
                        (int)i), false, layout);
                }

                new GuiCheckbox("Change Dir/Index", null, editDirIndexProperty, layout);
                new GuiTextButton("Dir " + paintDir.ToString(), null, paintDirMenu, true, layout);
                new GuiTextButton("Index " + paintIndex.ToString(), null, paintIndexMenu, true, layout);

                new GuiLabel("Special type", layout);
                for (MapBlockSpecialType i = 0; i < MapBlockSpecialType.NUM; ++i)
                {
                    new GuiTextButton(i.ToString(), null, new GuiAction2Arg<PaintType, int>(setPaint, PaintType.SpecialType,
                        (int)i), false, layout);
                }
            }
            layout.End();
        }

        void paintDirMenu()
        {
            GuiLayout layout = new GuiLayout("Dir", menu);
            {
                for (int i = 0; i < 4; ++i)
                {
                    new GuiTextButton(i.ToString(), null, new GuiActionIndex(selectDir, i), false, layout);
                }
            } layout.End();
        }

        void selectDir(int dir)
        {
            paintDir = (byte)dir;
            closeMenu();
        }

        void paintIndexMenu()
        {
            GuiLayout layout = new GuiLayout("Index", menu);
            {
                for (int i = 0; i < 20; ++i)
                {
                    new GuiTextButton(i.ToString(), null, new GuiActionIndex(selectIndex, i), false, layout);
                }
            } layout.End();
        }
        
        void selectIndex(int index)
        {
            paintIndex = (byte)index;
            closeMenu();
        }


        void optionsMenu()
        {
            newSize = segment.chunkSize;
            var types = new List<GuiOption<SegmentHeadType>>((int)SegmentHeadType.NUM);
            for (SegmentHeadType t = 0; t < SegmentHeadType.NUM; ++t)
            {
                types.Add(new GuiOption<SegmentHeadType>(t.ToString(), t));
            }

            openMenu();
            GuiLayout layout = new GuiLayout("Options", menu);
            {
                new GuiLabel("id: " + segment.header.id.ToString(), layout);
                new GuiTextButton("Save segment", null, saveSegment, false, layout);
                new GuiTextButton("Save As New", null, saveSegmentAsNew, false, layout);
                new GuiTextButton("Load segment", null, listStoredSegments, false, layout);
                new GuiDialogButton("Delete segment", null, new GuiAction(deleteSegment), false, layout);
                new GuiTextButton("New segment", null, newSegment, false, layout);
                new GuiOptionsList<SegmentHeadType>(SpriteName.NO_IMAGE, "Type", types, typeProperty, layout);
                new GuiIntSlider(SpriteName.NO_IMAGE, "Chunk Size X", sizeXProperty, new IntervalF(2, AbsLevel.ChunkSize.X), false, layout);
                new GuiIntSlider(SpriteName.NO_IMAGE, "Chunk Size Y", sizeYProperty, new IntervalF(2, AbsLevel.ChunkSize.Y), false, layout);
                new GuiTextButton("Apply new Size", null, applyNewSize, false, layout);

                new GuiTextButton("Rotate CW", null, rotateClockWise, false, layout);
                new GuiTextButton("Rotate CC", null, rotateCounterClockWise, false, layout);
                new GuiTextButton("Flip X", null, flipHori, false, layout);
                new GuiTextButton("Flip Y", null, flipVerti, false, layout);
                
                new GuiTextButton("Move Left", null, new GuiAction1Arg<Dir4>(moveMap, Dir4.W), false, layout);
                new GuiTextButton("Move Right", null, new GuiAction1Arg<Dir4>(moveMap, Dir4.E), false, layout);
                new GuiTextButton("Move Up", null, new GuiAction1Arg<Dir4>(moveMap, Dir4.N), false, layout);
                new GuiTextButton("Move Down", null, new GuiAction1Arg<Dir4>(moveMap, Dir4.S), false, layout);
            }
            layout.End();
        }



        void moveMap(Dir4 dir)
        {
            segment.squares.MoveEveryThing(conv.ToIntV2(dir, 1));
            refreshVisuals();
        }

        void rotateClockWise()
        {
            segment.Rotate(true);
            refreshVisuals();
        }
        void rotateCounterClockWise()
        {
            segment.Rotate(false);
            refreshVisuals();
        }
        void flipHori()
        {
            segment.flip(true);
            refreshVisuals();
        }
        void flipVerti()
        {
            segment.flip(false);
            refreshVisuals();
        }

        SegmentHeadType typeProperty(bool set, SegmentHeadType value)
        {
            if (set)
            {
                segment.header.type = value;
            }
            return segment.header.type;
        }

        bool editDirIndexProperty(int index, bool set, bool value)
        {
            if (set)
            {
                editDirAndIndex = value;
            }
            return editDirAndIndex;
        }

        IntVector2 newSize;
        int sizeXProperty(bool set, int value)
        {
            if (set)
            {
                newSize.X = value;
            }
            return newSize.X;
        }
        int sizeYProperty(bool set, int value)
        {
            if (set)
            {
                newSize.Y = value;
            }
            return newSize.Y;
        }
        void applyNewSize()
        {
            segment.Resize(newSize);
            refreshVisuals();
        }

        void saveSegmentAsNew()
        {
            segment.header.id = 0;
            segment.updateHeader();
            saveSegment();
        }

        void deleteSegment()
        {
            segment = new BlockMapSegment(segment.header.id);
            saveSegment();
            refreshVisuals();
        }

        void saveSegment()
        {
            Debug.Log("SAVE SEGMENT: " + segment.header.ToString());
            new Timer.AsynchActionTrigger(saveAsynch, true);
            closeMenu();
        }
        void listStoredSegments()
        {
            GuiLayout layout = new GuiLayout("Load", menu);
            {
                for (int i = LfRef.blockmaps.storedSegments.Count -1; i >= 0; --i)
                {
                    new GuiTextButton(LfRef.blockmaps.storedSegments[i].ToString(), null, new GuiActionIndex(loadSegment, i), false, layout);
                }
            }
            layout.End();
        }

        void newSegment()
        {
            segment = new BlockMapSegment();
            refreshVisuals();
            closeMenu();
        }

        void loadSegment(int index)
        {
            segment = new BlockMapSegment(LfRef.blockmaps.storedSegments[index], this);
        }

        void saveAsynch()
        {
            segment.SaveLoad(true, false, null);
            LfRef.blockmaps.SaveLoad(true, false);
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            if (!save)
            {
                loadingLock = false;
                refreshVisuals();
            }
        }

        void setPaint(PaintType paint, int utype)
        {
            this.paintType = paint;
            this.paintUnderType = utype;
            closeMenu();
        }

        void openMenu()
        {
            //Engine.XGuide.LocalHost.inputMap.SetGameStateLayout(ControllerActionSetType.MenuControls);
            if (menu == null)
            { 
                menu = new HUD.Gui(LootFest.MenuSystem2.GuiStyle(), 
                    Engine.Screen.SafeArea, 0, ImageLayers.Top4, 
                    Engine.XGuide.LocalHost.inputMap.inputSource); 
            }
        }

        void closeMenu()
        {
            if (menu != null)
            {
                menu.DeleteMe();
                menu = null;

                if (Input.Mouse.IsButtonDown(MouseButton.Left))
                {
                    waitForMouseUp = true;
                }
            }
           // Engine.XGuide.LocalHost.inputMap.SetGameStateLayout(ControllerActionSetType.CardGameControls);
        }

        void onNewSquare()
        {
            refreshInfo();
            if (Input.Mouse.IsButtonDown(MouseButton.Left) && !Input.Keyboard.Alt)
            {
                paint();
            }
        }

        void refreshInfo()
        {
            if (segment.squares.InBounds(mouseSqPos))
            {
                var sq = segment.squares.Get(mouseSqPos);

                squareInfo.TextString = mouseSqPos.ToString() + " : " + sq.ToString();
            }
        }

        void paint()
        {
            if (paint(mouseSqPos))
            {
                refreshVisuals();
            }
        }

        bool paint(IntVector2 pos)
        {
            if (segment.squares.InBounds(pos))
            {
                var sq = segment.squares.Get(pos);
                switch (paintType)
                {
                    case PaintType.BlockType:
                        sq.type = (MapBlockType)paintUnderType;
                        break;
                    case PaintType.SpecialType:
                        sq.special = (MapBlockSpecialType)paintUnderType;
                        break;
                }

                if (editDirAndIndex)
                {
                    sq.specialDir = paintDir;
                    sq.specialIndex = paintIndex;
                }
                segment.squares.Set(pos, sq);
                
                return true;
            }
            return false;
        }

        void pick()
        {
            if (segment.squares.InBounds(mouseSqPos))
            {
                var sq = segment.squares.Get(mouseSqPos);

                if (sq.special != MapBlockSpecialType.None)
                {
                    paintType = PaintType.SpecialType;
                    paintUnderType = (int)sq.special;
                }
                else
                {
                    paintType = PaintType.BlockType;
                    paintUnderType = (int)sq.type;
                }

                paintDir = sq.specialDir;
                paintIndex = sq.specialIndex;
            }
        }
    }

   
}
