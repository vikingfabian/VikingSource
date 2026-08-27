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
        /// <summary>
        /// Multiplies the RGBA component values of a color by the specified value.
        /// </summary>
        /// <param name="value">The source color value to multiply.</param>
        /// <param name="scale">The value to multiply the RGBA component values by.</param>
        /// <returns>The new color value created as a result of the multiplication.</returns>
        public static Color MultiplyRGBA(Color value, float scale)
        {
            return new Color((int)(value.R * scale), (int)(value.G * scale), (int)(value.B * scale), (int)(value.A * scale));
        }
        /// <summary>
        /// Multiplies the RGB component values of a color by the specified value.
        /// </summary>
        /// <param name="value">The source color value to multiply.</param>
        /// <param name="scale">The value to multiply the RGB component values by.</param>
        /// <returns>The new color value created as a result of the multiplication.</returns>
        public static Color MultiplyRGB(Color value, float scale)
        {
            return new Color((int)(value.R * scale), (int)(value.G * scale), (int)(value.B * scale), value.A);
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

        public static int ValueDifference(Color col1, Color col2)
        {
            return Math.Abs(col1.R - col2.R) + Math.Abs(col1.G - col2.G) + Math.Abs(col1.B - col2.B);
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
        /// Returns a color from white → yellow → red → dark red based on a 0–1 value.
        /// </summary>
        public static Color HeatColor_Inferno(float value)
        {
            value = MathHelper.Clamp(value, 0f, 1f);

            if (value < 0.33f)
            {
                if (value <= 0)
                { 
                    return Color.Gray;
                }
                // 0.0 → 0.33 : white → yellow
                float t = value / 0.33f;
                return new Color(1f, 1f, 1f - t); // RGB: (1, 1, 1−t)
            }
            else if (value < 0.66f)
            {
                // 0.33 → 0.66 : yellow → red
                float t = (value - 0.33f) / 0.33f;
                return new Color(1f, 1f - t, 0f); // RGB: (1, 1−t, 0)
            }
            else
            {
                // 0.66 → 1.0 : red → dark red
                float t = (value - 0.66f) / 0.34f;
                // Interpolate from red (1,0,0) to dark red (0.4,0,0)
                return new Color(1f - 0.6f * t, 0f, 0f);
            }
        }

    }
}
