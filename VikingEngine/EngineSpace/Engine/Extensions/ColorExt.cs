using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine
{
    static class ColorExt
    {
        public static readonly Color AlmostWhite = new Color(254, 254, 254);
        public static readonly Color Error = new Color(byte.MaxValue, 0, byte.MaxValue);
        public static readonly Color Empty = new Color(byte.MinValue, byte.MinValue, byte.MinValue, byte.MinValue);
        public static readonly Color DarkGrayer =  new Color(80, 80, 80);
        public static readonly Color VeryDarkGray = GrayScale(0.1f);


        public static Color GrayScale(float white)
        {
            return new Color(white, white, white);
        }
        public static Color GrayScale(byte white)
        {
            return new Color(white, white, white);
        }

        public static Color FromAlpha(float alpha)
        {
            Color res = Color.White;
            res.A = (byte)(alpha * byte.MaxValue);
            return res;
        }

        public static Color Mix(Color col1, Color col2, float percentageCol1)
        {
            return new Color(col1.ToVector4() * percentageCol1 + col2.ToVector4() * (1f - percentageCol1));
        }

        public static Color Multiply(Color col1, Color col2)
        {
            return new Color(col1.ToVector3() * col2.ToVector3());
        }

        public static Color Reverse(Color col)
        {
            return new Color(Microsoft.Xna.Framework.Vector3.One - col.ToVector3());
        }

        public static Color Multiply(Color col1, float brightness)
        {
            return new Color(col1.ToVector3() * brightness);
        }

        public static Color ChangeBrighness(Color col, int change)
        {
            col.R = Bound.Byte(col.R + change);
            col.G = Bound.Byte(col.G + change);
            col.B = Bound.Byte(col.B + change);
            return col;
        }

        public static float GetBrightNess(Color col)
        {
            float r, g, b;
            col.Deconstruct(out r, out g, out b);
            return (r + g + b) / 3f;
        }

        public static Color ChangeColor(Color col, int addR, int addG, int addB)
        {
            addR += col.R;
            if (addR <= byte.MinValue) { col.R = byte.MinValue; }
            else if (addR >= byte.MaxValue) { col.R = byte.MaxValue; }
            else { col.R = (byte)addR; }

            addG += col.G;
            if (addG <= byte.MinValue) { col.G = byte.MinValue; }
            else if (addG >= byte.MaxValue) { col.G = byte.MaxValue; }
            else { col.G = (byte)addG; }

            addB += col.B;
            if (addB <= byte.MinValue) { col.B = byte.MinValue; }
            else if (addB >= byte.MaxValue) { col.B = byte.MaxValue; }
            else { col.B = (byte)addB; }

            return col;
        }

        public static float Alpha(this Color value)
        {
            return value.A * PublicConstants.ByteToPercent;
        }
    }
}
