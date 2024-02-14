using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace VikingEngine
{

    public struct TextFormat
    {
        public LoadedFont Font;
        public float size;
        public Color Color;
        public Color BgColor;
        public TextFormat(LoadedFont font, float size, Color color, Color bgCol)
        {
            this.Font = font;
            this.size = size;
            this.Color = color;
            BgColor = bgCol;
        }

        public Vector2 Scale => new Vector2(size);
    }

    public struct TwoStrings
    {
        public string String1;
        public string String2;
        public TwoStrings(string s1, string s2)
        { String1 = s1; String2 = s2; }

        public override string ToString()
        {
            return String1 + ", " + String2;
        }
    }
    
    public struct ThreeStrings
    {
        public string String1;
        public string String2;
        public string String3;
        public ThreeStrings(string s1, string s2, string s3)
        { String1 = s1; String2 = s2; String3 = s3; }
    }

    public struct TextAndIndex
    {
        public string Text;
        public int Index;
        public TextAndIndex(string text, int index)
        {
            this.Text = text;
            this.Index = index;
        }
        
    }
}
