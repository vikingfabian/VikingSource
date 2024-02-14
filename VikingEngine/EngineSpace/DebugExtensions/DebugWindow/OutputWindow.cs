using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.DebugExtensions
{
    class OutputWindow
    {
        const float Edge = 4;
        const int NumLines = 40;
        const int LineHeigth = 16;
        const int Width = 600;
        const int LetterW = 8;
        const int MaxLetters = Width / LetterW;

        Graphics.Image background;
        List<Graphics.TextG> lines;

        public OutputWindow()
        {
            background = new Graphics.Image(SpriteName.WhiteArea, new Vector2(Engine.Screen.Width - Width, 0), new Vector2(Width, NumLines * LineHeigth + Edge * 2),
                 ImageLayers.Foreground2);
            background.Color = Color.Black;
            background.Opacity = 0.4f;
            lines = new List<Graphics.TextG>();
        }

        public void AddLine(string line)
        {
            line = TextLib.FirstLetters(line, MaxLetters);
            arraylib.CropArrayAtBeginning(lines, NumLines -1);
            lines.Add(new Graphics.TextG(LoadedFont.Console, Vector2.Zero, Vector2.One, Graphics.Align.Zero, line, Color.White, ImageLayers.Foreground1));
            updateLinesPos();
        }

        void updateLinesPos()
        {
            
            Vector2 pos = new Vector2(background.Xpos + Edge, background.Ypos + Edge);
            foreach (Graphics.TextG l in lines)
            {
                l.Position = pos;
                pos.Y += LineHeigth;
            }
        }

        public void DeleteMe()
        {
            background.DeleteMe();
            foreach (Graphics.TextG l in lines)
            {
                l.DeleteMe();
            }
        }
    }
}
