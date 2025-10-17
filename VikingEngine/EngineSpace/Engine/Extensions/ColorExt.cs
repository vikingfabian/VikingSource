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
            col.Deconstruct(out byte r, out byte g, out byte b);
            return new Color(r+change, g+change, b+change);
        }

        public static Color ChangeYellow(Color col, int change)
        {
            col.Deconstruct(out byte r, out byte g, out byte b);
            
            return new Color(r + change, g + change, b);
        }

        public static float GetBrightNess(Color col)
        {
            float r, g, b;
            col.Deconstruct(out r, out g, out b);
            return (r + g + b) / 3f;
        }

        public static Color ChangeColor(Color col, int addR, int addG, int addB)
        {
            col.Deconstruct(out byte r, out byte g, out byte b);
            return new Color(r + addR, g + addG, b + addB);
        }

        public static float Alpha(this Color value)
        {
            return value.A * PublicConstants.ByteToPercent;
        }

        /// <summary>
        /// Returns a color from black → red → yellow → white based on a 0–1 value.
        /// </summary>
        public static Color HeatColor_Inferno(float value)
        {
            value = MathHelper.Clamp(value, 0f, 1f);

            if (value < 0.33f)
            {
                // 0.0 → 0.33 : black → red
                float t = value / 0.33f;
                return new Color(t, 0f, 0f); // RGB: (t, 0, 0)
            }
            else if (value < 0.66f)
            {
                // 0.33 → 0.66 : red → yellow
                float t = (value - 0.33f) / 0.33f;
                return new Color(1f, t, 0f); // RGB: (1, t, 0)
            }
            else
            {
                // 0.66 → 1.0 : yellow → white
                float t = (value - 0.66f) / 0.34f;
                return new Color(1f, 1f, t); // RGB: (1, 1, t)
            }
        }
    }
}
