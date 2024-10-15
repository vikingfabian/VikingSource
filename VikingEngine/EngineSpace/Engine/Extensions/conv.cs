using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine
{
    /// <summary>
    /// Viking Engine converter lib
    /// </summary>
    static class conv
    {
        public static Dir4 ToDir4(Vector2 dir, bool SouthIsPositive)
        {
            return ToDir4(dir.X, dir.Y, SouthIsPositive);
        }

        public static Dir4 ToDir4(float x, float y, bool SouthIsPositive)
        {
            if (x == 0 && y == 0) { return Dir4.NUM_NON; }

            if (Math.Abs(x) > Math.Abs(y))
            {
                if (x > 0)
                {
                    return Dir4.E;
                }
                else
                {
                    return Dir4.W;
                }
            }
            else
            {
                if (y > 0)
                {
                    if (SouthIsPositive)
                        return Dir4.S;
                    else
                        return Dir4.N;
                }
                else
                {
                    if (SouthIsPositive)
                        return Dir4.N;
                    else
                        return Dir4.S;
                }
            }
        }

        public static Dir8 ToDir8(Rotation1D rotation)
        {
            rotation.Add(-MathHelper.Pi / lib.Dir8Count);
            int dir8 = Convert.ToInt32(rotation.Radians / (MathHelper.TwoPi / lib.Dir8Count));
            dir8 %= lib.Dir8Count;
            return (Dir8)dir8;
        }

        public static int ToDir8_INT(Rotation1D rotation)
        {
            rotation.Add(-MathHelper.Pi / lib.Dir8Count);
            int dir8 = Convert.ToInt32(rotation.Radians / (MathHelper.TwoPi / lib.Dir8Count));
            dir8 %= lib.Dir8Count;
            return dir8;
        }


        public static Dir8 ToDir8(IntVector2 vector)
        {
            return ToDir8(vector.X, vector.Y);
        }

        public static Dir8 ToDir8(float x, float y)
        {
            if (x == 0 && y == 0) { return Dir8.NO_DIR; }

            float l = VectorExt.Length(x, y);
            x = Convert.ToInt32(x / l);
            y = Convert.ToInt32(y / l);

            if (x > 0)
            {
                if (y > 0)
                {
                    return Dir8.SE;
                }
                else if (y < 0)
                {
                    return Dir8.NE;
                }
                else
                {
                    return Dir8.E;
                }
            }
            else if (x < 0)
            {
                if (y > 0)
                {
                    return Dir8.SW;
                }
                else if (y < 0)
                {
                    return Dir8.NW;
                }
                else
                {
                    return Dir8.W;
                }
            }
            else
            {
                if (y > 0)
                {
                    return Dir8.S;
                }
                else if (y < 0)
                {
                    return Dir8.N;
                }
                else
                {
                    return Dir8.NO_DIR;
                }
            }
        }

        public static Dir8 RadiansToDir8(float angle)
        {
            return lib.WrapDir((Dir8)Convert.ToInt16(angle / MathHelper.PiOver4));
        }

        public static Rotation1D ToRadians(Dir8 dir)
        {
            return new Rotation1D((int)dir * MathHelper.PiOver4);
        }
        public static Rotation1D ToRadians(Dir4 dir)
        {
            return new Rotation1D((int)dir * MathHelper.PiOver2);
        }
        public static Vector2 ToV2(Dir4 dir, float length)
        {
            Vector2 result = Vector2.Zero;
            switch (dir)
            {
                case Dir4.N:
                    result.Y -= length;
                    break;
                case Dir4.S:
                    result.Y += length;
                    break;
                case Dir4.W:
                    result.X -= length;
                    break;
                case Dir4.E:
                    result.X += length;
                    break;
                default:
                    result = ToV2(lib.WrapDir(dir), length);
                    break;
            }
            return result;
        }

        public static IntVector2 ToIntV2(Dir4 facing, int length)
        {
            IntVector2 dir = IntVector2.Zero;
            switch (facing)
            {
                case Dir4.N:
                    dir.Y -= length;
                    break;
                case Dir4.S:
                    dir.Y += length;
                    break;
                case Dir4.W:
                    dir.X -= length;
                    break;
                case Dir4.E:
                    dir.X += length;
                    break;
                    //default:
                    //    dir = Facing4ToVec2((Facing4Dir)FixRotation((int)facing), Add);
                    //    break;
            }
            return dir;
        }
        public static Vector2 Facing8ToVec2(Dir8 facing, float Add)
        {
            Vector2 dir = Vector2.Zero;

            switch (facing)
            {
                case Dir8.N:
                    dir.Y -= Add;
                    break;
                case Dir8.S:
                    dir.Y += Add;
                    break;
                case Dir8.W:
                    dir.X -= Add;
                    break;
                case Dir8.E:
                    dir.X += Add;
                    break;
                case Dir8.NW:
                    dir.Y -= Add;
                    dir.X -= Add;
                    break;
                case Dir8.NE:
                    dir.Y -= Add;
                    dir.X += Add;
                    break;
                case Dir8.SW:
                    dir.Y += Add;
                    dir.X -= Add;
                    break;
                case Dir8.SE:
                    dir.Y += Add;
                    dir.X += Add;
                    break;
                default:
                    //RotationVecDir(ref dir, (Facing4Dir)FixRotation((int)facing), Add);
                    break;
            }
            return dir;
        }

        public static void IntToUShorts(int value, out ushort high, out ushort low)
        {
            high = (ushort)(value >> 16); // Get the high 16 bits
            low = (ushort)(value & 0xFFFF); // Get the low 16 bits
        }

        public static int UShortsToInt(ushort high, ushort low)
        {
            return (high << 16) | low;
        }

        public static IntVector2 IntToIntVector2(int value)
        {
            return new IntVector2(
                value >> 16, // Get the high 16 bits
                value & 0xFFFF // Get the low 16 bits
                );
        }

        public static int IntVector2ToInt(IntVector2 value)
        {
            return (value.X << 16) | value.Y;
        }
    }
}
