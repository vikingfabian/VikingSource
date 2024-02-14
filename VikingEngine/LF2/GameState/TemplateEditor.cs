using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.LF2.GameState
{
    class TemplateEditor : Engine.GameState
    {
        static readonly TemplateType TType = TemplateType.DotMap;
        Image[,] squares;
        int[,] value;
        IntVector2 size;
        Vector2 squareSz;
        Image pencil;
        const int MaxHeight = 6;
        const float ColorPerHeight = (byte.MaxValue / MaxHeight);
        int height = 1;
        const int PaintWith = 4;
        bool detailPaint = false;
        int saveIx = 0;

        Image border;

        public TemplateEditor()
            :base()
        {
            Engine.LoadContent.LoadTextures(new List<LoadedTexture> { LoadedTexture.LFTiles, LoadedTexture.WhiteArea });
            switch (TType)
            {
                case TemplateType.Height:
                    size = new IntVector2(
                        Map.Terrain.HeightTemplate.Width,
                        Map.Terrain.HeightTemplate.Width
                        );
                    break;
                case TemplateType.Detail:
                    size = new IntVector2(
                        Map.WorldPosition.ChunkWidth,
                        Map.WorldPosition.ChunkWidth
                        );
                    break;
                case TemplateType.DotMap:
                    size = new IntVector2(
                        Map.WorldPosition.ChunkWidth,
                        Map.WorldPosition.ChunkWidth
                        );
                    break;

            }
            
            value = new int[size.X, size.Y];
            squares = new Image[size.X, size.Y];
            squareSz = new Vector2(Engine.Screen.Height / size.Y);
            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    squares[x, y] = new Image(SpriteName.EffectGreenBubble, new Vector2(x * squareSz.X, y * squareSz.Y), 
                        squareSz, ImageLayers.Background4);
                    squares[x, y].Color = Color.Red;
                }
            }

            pencil = new Image(SpriteName.WhiteArea, Vector2.Zero, PaintWith * squareSz, ImageLayers.Lay1);
            pencil.Color = Color.Yellow;
            pencil.Transparentsy = 0.4f;


            if (TType == TemplateType.Height)
            {
                int borderstart = Map.Terrain.HeightTemplate.Radius - Map.WorldPosition.ChunkWidth / 2;
                   
                border = new Image(SpriteName.LFEdge, new Vector2(borderstart * squareSz.X), new Vector2(Map.WorldPosition.ChunkWidth * squareSz.X), ImageLayers.Background1);
                border.Color = Color.Blue;
                border.Transparentsy = 0.3f;
            }

           // WatchKeys = new List<Microsoft.Xna.Framework.Input.Keys> { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.S };
        }
        public override void KeyPressEvent(Keys key, bool keydown)
        {
            //if (keydown)
            //{
            //    if (key == Keys.S)
            //    {
            //        if (keyCtrl)
            //        {
            //            List<string> lines = new List<string>();
            //            if (TType == TemplateType.DotMap)
            //            {
            //                SaveDots(lines);
            //            }
            //            else
            //            {
            //                // Probably never happens
            //                SaveGrid(lines);
            //            }

            //            DataLib.SaveLoad.CreateTextFile(TType.ToString() + "Template" + saveIx.ToString() + ".txt", lines);
            //            saveIx++;
            //            System.Diagnostics.Debug.WriteLine("Saving..");
            //        }
            //    }
            //    else if (key == Keys.D0)
            //    {
            //        height = 0;
            //        System.Diagnostics.Debug.WriteLine("Erase");
            //    }
            //    else if (key >= Keys.D1 && key >= Keys.D0)
            //    {
            //        height = (int)(key - Keys.D1) +1;
            //        System.Diagnostics.Debug.WriteLine("Height" + height.ToString());
            //    }
            //}
        }
        public void SaveGrid(List<string> lines)
        {
            //SAVE
            for (int y = 0; y < size.Y; y++)
            {
                string line = "{";
                for (int x = 0; x < size.X; x++)
                {
                    line += value[x, y].ToString() + ",";
                }
                lines.Add(line + "},");
            }
        }
        public void SaveDots(List<string> lines)
        {
            //SAVE
            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    if (value[x, y] != 0)
                    {
                        lines.Add("new ByteVector2(" + x.ToString() + ", " + y.ToString() + "),");
                    }
                }
            }
        }
        public override void MouseMove_Event(Vector2 position, Vector2 deltaPos)
        {
            pencil.Position = IntVector2.FromVec2(position).Vec;
            if (mouseDown)
                paint(position);
        }
        bool mouseDown = false; 
        public override void MouseClick_Event(Vector2 position, MouseButton button, bool keydown)
        {
            mouseDown = keydown;
            detailPaint = button == MouseButton.Right;
            paint(position);
        }
        List<IntVector2> avoid = new List<IntVector2>
        {
            new IntVector2(0,0), new IntVector2(PaintWith -1, 0),
            new IntVector2(0, PaintWith -1), new IntVector2(PaintWith -1)
        };
        void paint(Vector2 pos)
        {
            IntVector2 gridpos = new IntVector2(
                (int)(pos.X / squareSz.X),
                (int)(pos.Y / squareSz.Y));
            int w = detailPaint? 1 : PaintWith;

            IntVector2 xy = IntVector2.Zero;
            for (xy.Y= 0; xy.Y < w; xy.Y++)
            {
                for (xy.X = 0; xy.X < w; xy.X++)
                {
                    if (detailPaint ||
                        !avoid.Contains(xy))
                    {
                        IntVector2 paintPos = gridpos;
                        //paintPos.X += x;
                        //paintPos.Y += y;xy
                        paintPos.Add(xy);
                        if (paintPos.X < size.X && paintPos.Y < size.Y &&
                            paintPos.X >=0 && paintPos.Y >= 0)
                        {
                            value[paintPos.X, paintPos.Y] = height;
                            if (height == 0)
                            {
                                squares[paintPos.X, paintPos.Y].Color = Color.Red;
                            }
                            else
                            {
                                byte colorVal = (byte)(ColorPerHeight * height);
                                squares[paintPos.X, paintPos.Y].Color = new Color(colorVal, colorVal, colorVal);
                            }
                        }
                    }
                }
            }
        }
        public override Engine.GameStateType Type
        {
            get { return Engine.GameStateType.Editor; }
        }
    }
    enum TemplateType
    {
        Height,
        Detail,
        DotMap, //för placering utav träd och liknande
    }
}
