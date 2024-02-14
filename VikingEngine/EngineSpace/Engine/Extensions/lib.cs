using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;

namespace VikingEngine
{
    static class lib
    {
        public static bool HasFlag(int value, int flag)
        {
            return (value & flag) > 0;
        }

        public const int CornerCount = (int)Corner.NUM;
        public const int Dir4Count = (int)Dir4.NUM_NON;
        public const int Dir8Count = (int)Dir8.NUM;

        public static readonly bool[] Facing4DirIsXdim = new bool[] { false, true, false, true };

        public static double ValueOnPercentScale(double min, double max, double value, bool cap)
        {
            double result = (value - min) / (max - min);

            if (cap)
            {
                if (result < 0) return 0;
                if (result > 1) return 1;
            }

            return result;
        }

        public static int ArrayPosToCellIndex(IntVector2 arrayPos, int arrayWidth)
        {
            return arrayPos.X + arrayPos.Y * arrayWidth;
        }
        public static IntVector2 CellIndexToArrayPos(int cellIndex, int arrayWidth)
        {
            IntVector2 result = IntVector2.Zero;
            result.Y = cellIndex / arrayWidth;
            result.X = cellIndex - result.Y * arrayWidth;
            return result;
        }

        public static float GetLongestV3Dimention(Vector3 value)
        {
            float max = value.X > value.Y ? value.X : value.Y;
            return max > value.Z? max : value.Z;
        }
        public static float GetLongestAbsV3Dimention(Vector3 value)
        {
            value.X = Math.Abs(value.X);
            value.Y = Math.Abs(value.Y);
            value.Z = Math.Abs(value.Z);
            return GetLongestV3Dimention(value);
        }
        public static Vector3 RotatePointAroundCenter3D(Vector3 centerPos, Vector3 pointPos, RotationQuarterion rotation)
        {
            pointPos.X -= centerPos.X;
            pointPos.Y -= centerPos.Y;
            pointPos.Z -= centerPos.Z;
            return rotation.TranslateAlongAxis(pointPos, centerPos);
        }


        public static Vector2 RotatePointAroundCenter(Vector2 centerPos, Vector2 pointPos, float rotation)
        {
            //If you rotate point (px, py) around point (ox, oy) by angle theta you'll get:

            //p'x = cos(theta) * (px-ox) - sin(theta) * (py-oy) + ox
            //p'y = sin(theta) * (px-ox) + cos(theta) * (py-oy) + oy

            if (rotation == 0f)
            {
                return pointPos;
            }

            Vector2 diff = pointPos - centerPos;

            return new Vector2(
                (float)(Math.Cos(rotation) * diff.X - Math.Sin(rotation) * diff.Y) + centerPos.X,
                (float)(Math.Sin(rotation) * diff.X + Math.Cos(rotation) * diff.Y) + centerPos.Y);
        }
        
        public static Vector3 RotatePosDiffAroundCenter(Vector3 centerPos, Vector3 posDiff, float rotation)
        {
            posDiff.Z = -posDiff.Z;
            centerPos +=  VectorExt.V2toV3XZ(RotatePointAroundOrigo(VectorExt.V3XZtoV2(posDiff), rotation));
            centerPos.Y += posDiff.Y;

            return centerPos;
        }

        public static Vector2 RotatePosDiffAroundCenter(Vector2 centerPos, Vector2 posDiff, float rotation)
        {
            double angle = 0;
            if (posDiff.X == 0)
            {
                //Måste ha med om det är + eller - 90grader
                angle = MathHelper.PiOver2; //90 degree
                if (posDiff.Y < 0) { angle = angle * -1; }
            }
            else if (posDiff.Y != 0)
            {
                angle = Math.Atan(posDiff.Y / posDiff.X); //must watch out for divition close to zero
            }
            //Add rotation
            angle += (double)rotation;
            //Calculate new position
            //cos(a) = x / l
            float length = posDiff.Length();
            centerPos.X += (float)(length * Math.Cos(angle));
            centerPos.Y += (float)(length * Math.Sin(angle));
            return centerPos;
        }

        public static Vector2 RotatePointAroundOrigo(Vector2 pointPos, float rotation)
        {
            double angle = 0;
            if (pointPos.X == 0)
            {
                //Måste ha med om det är + eller - 90grader
                angle = MathHelper.PiOver2; //90 degree
                if (pointPos.Y < 0) { angle = angle * -1; }
            }
            else if (pointPos.Y != 0)
            {
                angle = Math.Atan(pointPos.Y / pointPos.X); //must watch out for divition close to zero
            }
            //Add rotation
            angle += (double)rotation;
            //Calculate new position
            //cos(a) = x / l
            Vector2 result = Vector2.Zero;
            float length = pointPos.Length();
            result.X = (float)(length * Math.Cos(angle));
            result.Y = (float)(length * Math.Sin(angle));
            return result;
        }

        /// <summary>
        /// Used to set breakpoints during debug
        /// </summary>
        public static void DoNothing()
        {
            if (true)
                return;
        }        
        
        public static float ColorFloatToByte(float value, bool ToByte)
        {
            if (ToByte)
            { return value * PublicConstants.MaxColor; }
            return value / PublicConstants.MaxColor;
        }

        public static bool PointInsideRectangle(Vector2 point, Rectangle area)
        {
            IntVector2 p = IntVector2.FromVec2(point);
            if (point.X < area.X || point.Y < area.Y)
                return false;
            if (point.X > area.X + area.Width || point.Y > area.Y + area.Height)
                return false;

            return true;
        }

        public static Vector2 SpeedTowardsPoint(Vector2 speed, Vector2 posDiff)
        {
            posDiff *= PublicConstants.Invert;
            speed += posDiff;
            return speed * PublicConstants.Half;
        }

        public static Dir8 Invert(Dir8 dir)
        {
            dir += 4;
            if (dir >= Dir8.NUM)
            { 
                dir -= 8;
            }
            return dir;
        }

        public static void Invert(ref bool value)
        {
            value = !value;
        }

        public static void Invert(ref Vector2 value)
        {
            value = -value;
        }
        public static void Invert(ref int value)
        {
            value = -value;
        }

        public static Dir4 Invert(Dir4 dir)
        {
            return WrapDir(dir + 2);
        }

        public static Dir4 Rotate(Dir4 dir, int add)
        {
            return WrapDir(dir + add);
        }

        public static Dir8 Rotate(Dir8 dir, int add)
        {
            return WrapDir(dir + add);
        }

        public static Dir4 WrapDir(Dir4 dir)
        {
            if (dir < 0)
            {
                do
                {
                    dir += Dir4Count;
                } while (dir < 0);
            }
            else if (dir >= Dir4.NUM_NON)
            {
                do
                {
                    dir -= Dir4Count;
                } while (dir >= Dir4.NUM_NON);
            }
            return dir;
        }
        public static Dir8 WrapDir(Dir8 dir)
        {
            if (dir < 0)
            {
                do
                {
                    dir += Dir8Count;
                } while (dir < 0);
            }
            else if (dir >= Dir8.NUM)
            {
                do
                {
                    dir -= Dir8Count;
                } while (dir >= Dir8.NUM);
            }
            return dir;
        }

        public static Corner WrapCorner(Corner corner)
        {
            if (corner < 0)
            {
                do
                {
                    corner += CornerCount;
                } while (corner < 0);
            }
            else if (corner >= Corner.NUM)
            {
                do
                {
                    corner -= CornerCount;
                } while (corner >= Corner.NUM);
            }
            return corner;
        }


        /// <returns>Is closest to first value</returns>
        public static bool closestTo(float value, float compareTo1, float compareTo2, out float closestCompare, out float closestDist)
        {
            float dist1 = Math.Abs(compareTo1 - value);
            float dist2 = Math.Abs(compareTo2 - value);

            if (dist1 < dist2)
            {
                closestCompare = compareTo1;
                closestDist = dist1;
                return true;
            }
            else
            {
                closestCompare = compareTo2;
                closestDist = dist2;
                return false;
            }
        }

        
        public static float SafeDiv(float value, float divideBy)
        {
            if (divideBy == 0)
            { divideBy = 0.01f; }
            return value / divideBy;
        }

        /// <summary>
        /// will ignore the error in retail but crash when debugging
        /// </summary>
        public static float RetailSafeDiv(float value, float divideBy, int location)
        {
            if (divideBy == 0)
            {
                if (PlatformSettings.ViewErrorWarnings)
                {
                    throw new DivideByZeroException("RetailSafeDiv at location " + location.ToString());
                }
                else
                {
                    return value / 0.01f;
                }
            }
            return value / divideBy;
        }

        public static void SwitchPointers<T>(ref T pointer1, ref T pointer2)
        {
            var store = pointer1;
            pointer1 = pointer2;
            pointer2 = store;
        }

        public static void Switch(ref Vector2 val1, ref Vector2 val2)
        {
            Vector2 val1Store = val1;
            val1 = val2;
            val2 = val1Store;
        }

        public static SpriteName SumSpriteName(SpriteName sprite, int add)
        {
            return (SpriteName)((int)sprite + add);
        }

        public static Dir4 SumFacingAngles(Dir4 angle1, Dir4 angle2)
        {
            int dir = (int)angle1 + (int)angle2;
            if (dir >= Dir4Count)
            {
                dir -= Dir4Count;
            }
            return (Dir4)dir;
        }
        
        public static int ToLeftRight(float value)
        {
            if (value == 0)
            { return 0; }
            else
            { return (value > 0 ? 1 : -1); }
        }
        public static int ToLeftRight(int value)
        {
            if (value == 0)
            { return 0; }
            else
            { return (value > 0 ? 1 : -1); }
        }

        public static bool SameDirection(float dir1, float dir2)
        {
            if (dir1 > 0)
                return dir2 > 0;
            if (dir1 < 0)
                return dir2 < 0;

            return dir2 == 0;
        }

        public static bool SameDirection(double dir1, double dir2)
        {
            if (dir1 > 0)
                return dir2 > 0;
            if (dir1 < 0)
                return dir2 < 0;

            return dir2 == 0;
        }

        public static int BoolToLeftRight(bool positive)
        {
            return positive ? 1 : -1;
        }
        
        public static List<string> textValToStringArray(string value)
        {
            List<string> ret = new List<string> { TextLib.EmptyString };
            int array = 0;
            foreach (char c in value)
            {
                if (c == '*')
                {
                    ret.Add(TextLib.EmptyString);
                    array++;
                }
                else
                { ret[array] += c; }
            }
            return ret;
        }

        public static string StringArrayToText(List<string> value)
        {
            string ret = value[0];
            for (int i = 1; i < value.Count; i++)
            {
                ret += SaveData.Dimension + value[i];
            }
            return ret;
        }

        public static List<string> CompressStringArray(List<string> values)
        {
            string currentType = values[0];
            int repeateCount = 0;
            List<string> result = new List<string>();

            foreach (string s in values)
            {
                if (s == currentType)
                {
                    repeateCount++;
                }
                else
                {
                    checkArrayRepetitions(repeateCount, currentType, result);
                    repeateCount = 1;
                    currentType = s;
                }
                
            }
            checkArrayRepetitions(repeateCount, currentType, result);
            return result;
        }

        private static void checkArrayRepetitions(int repeateCount, string currentType, List<string> result)
        {
            result.Add(currentType.ToString()); result.Add(repeateCount.ToString()); 
        }

        public static int SafeStringToInt(string value)
        {
            if (value == TextLib.EmptyString) { return 0; }
            return Convert.ToInt16(value);
            //return 0;
        }
        public static double safeStringValue(string value)
        {
            bool gotDot = false;
            bool negative = false;
            string text = TextLib.EmptyString;
            string test = Convert.ToString(0.1);

            char comma = test[1];

            foreach (char c in value)
            {
                if (c == ',' || c == '.')
                {
                    if (!gotDot)
                    {
                        if (text == TextLib.EmptyString)
                        { text = "0" + comma; }
                        else
                        { text += comma; }
                        gotDot = true;
                    }
                }
                if (c == '-')
                { negative = true; }
                if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' ||
                    c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                {
                    text += c;
                }
            }
            if (text == TextLib.EmptyString) { return 0; }
            else if (text[text.Length - 1] == '.') { text += "0"; }
            //return Convert.ToDouble(text);
            double val = Convert.ToDouble(text);
            if (negative) { val = -val; }
            return val;
        }
        
        public static void Rotation1DToQuaterion(Graphics.AbsVoxelObj mesh, float rotation)
        {
            mesh.Rotation.QuadRotation = Quaternion.Identity;
            mesh.Rotation.RotateWorldX(MathHelper.TwoPi - rotation);
        }

        public static void Rotation1DToQuaterion(Graphics.Mesh mesh, float rotation)
        {
            mesh.Rotation.QuadRotation = Quaternion.Identity;
            Vector3 rot = Vector3.Zero;
            rot.X = MathHelper.TwoPi - rotation;
            mesh.Rotation.RotateWorld(rot);
        }
        
        public static bool HasDecimal(double number)
        {
            return number != Convert.ToDouble((int)number);
        }
        public static double TrimmDecimal(double number, int decimals)
        {
            string snumb = Convert.ToString(number);
            if (HasDecimal(number))
            {
                int ix = 0;
                foreach (char c in snumb)
                {
                    if (c == ',' || c == '.')
                    {
                        break;
                    }
                    ix++;
                }
                decimals++;
                if (snumb.Length > decimals + ix)
                { return Convert.ToDouble(snumb.Remove(ix + decimals, snumb.Length - (ix + decimals))); }

            }
            return number;
        }
        public static Vector2 AbsVector2(Vector2 value)
        {
            value.X = Math.Abs(value.X);
            value.Y = Math.Abs(value.Y);
            return value;
        }
        
        public static string Vec4Text(Vector4 val)
        {
            return SafeFloatToString(val.X) + SaveData.Dimension + SafeFloatToString(val.Y) + SaveData.Dimension +
                SafeFloatToString(val.Z) + SaveData.Dimension + SafeFloatToString(val.W);
        }
        public static string RectangleText(Rectangle val)
        {
            return SafeFloatToString(val.X) + SaveData.Dimension + SafeFloatToString(val.Y) + SaveData.Dimension +
                SafeFloatToString(val.Width) + SaveData.Dimension + SafeFloatToString(val.Height);
        }

        public static string Vec3Text(Vector3 val)
        {
            return SafeFloatToString(val.X) + SaveData.Dimension + SafeFloatToString(val.Y) + SaveData.Dimension + SafeFloatToString(val.Z);
        }
        public static string SafeFloatToString(float val)
        {
            const int MAX_DECI = 4;

            if (Math.Abs(val) < 0.001f) return "0";
            string ret = val.ToString();
            int length = ((int)val).ToString().Length + MAX_DECI;
            if (ret.Length > length)
            {
                ret = ret.Remove(length, ret.Length - length);
            }
            return ret;
        }
        public static string IntVec2Text(IntVector2 val)
        {
            return val.X.ToString() + SaveData.Dimension + val.Y.ToString();
        }
        public static string IntVec2Text(ShortVector2 val)
        {
            return val.X.ToString() + SaveData.Dimension + val.Y.ToString();
        }
        public static string IntV3Text(IntVector3 val)
        {
            return val.X.ToString() + SaveData.Dimension + val.Y.ToString() + SaveData.Dimension + val.Z.ToString();
        }
        public static IntVector2 StringToIntV2(string val)
        {
            List<float> dimentions = StingDimentions(val);
            IntVector2 ret = IntVector2.Zero;
            if (dimentions.Count > 1)
            {
                ret.X = (int)dimentions[0];
                ret.Y = (int)dimentions[1];
            }
            return ret;
        }
        public static IntVector3 StringToIntV3(string val)
        {
            List<float> dimentions = StingDimentions(val);
            IntVector3 ret = IntVector3.Zero;
            if (dimentions.Count > 1)
            {
                ret.X = (int)dimentions[0];
                ret.Y = (int)dimentions[1];
                ret.Z = (int)dimentions[2];

            }
            return ret;
        }
        public static string Vec2Text(Vector2 val)
        {
            return SafeFloatToString(val.X) + SaveData.Dimension + SafeFloatToString(val.Y);
        }
        public static string QuatText(Quaternion val)
        {
            return SafeFloatToString(val.X) + SaveData.Dimension + SafeFloatToString(val.Y) + SaveData.Dimension +
                SafeFloatToString(val.Z) + SaveData.Dimension + SafeFloatToString(val.W);
        }

        public static Vector2 StringToV2(string val)
        {
            List<float> dimentions = StingDimentions(val);
            Vector2 ret = Vector2.Zero;
            if (dimentions.Count > 1)
            {
                ret.X = dimentions[0];
                ret.Y = dimentions[1];
            }
            return ret;
        }
        public static Vector3 StringToV3(string val)
        {
            List<float> dimentions = StingDimentions(val);
            Vector3 ret = Vector3.Zero;
            if (dimentions.Count > 2)
            {
                ret.X = dimentions[0];
                ret.Y = dimentions[1];
                ret.Z = dimentions[2];
            }
            return ret;
        }
        public static Vector4 StringToV4(string val)
        {
            List<float> dimentions = StingDimentions(val);
            Vector4 ret = Vector4.Zero;
            if (dimentions.Count > 3)
            {
                ret.X = dimentions[0];
                ret.Y = dimentions[1];
                ret.Z = dimentions[2];
                ret.W = dimentions[3];
            }
            return ret;
        }
        public static Rectangle StringToRectangle(string val)
        {
            List<float> dimentions = StingDimentions(val);
            Rectangle ret = Rectangle.Empty;
            if (dimentions.Count > 3)
            {
                ret.X = (int)dimentions[0];
                ret.Y = (int)dimentions[1];
                ret.Width = (int)dimentions[2];
                ret.Height = (int)dimentions[3];
            }
            return ret;
        }
        public static Quaternion StringToQuat(string val)
        {
            List<float> dimentions = StingDimentions(val);
            Quaternion ret = Quaternion.Identity;
            if (dimentions.Count > 3)
            {
                ret.X = dimentions[0];
                ret.Y = dimentions[1];
                ret.Z = dimentions[2];
                ret.W = dimentions[3];
            }
            return ret;
        }
        public static List<float> StingDimentions(string val)
        {
            if (val == SaveData.EmptyDataArray)
            {
                return new List<float>();
            }

            val += SaveData.Dimension;
            List<float> result = new List<float>();
            string txt = TextLib.EmptyString;
            foreach (char c in val)
            {
                if (c == SaveData.Dimension)
                {
                    result.Add((float)safeStringValue(txt));
                    txt = TextLib.EmptyString;
                }
                else
                { txt += c; }
            }
            return result;
        }
        public static List<int> StingIntDimentions(string val)
        {
            if (val == SaveData.EmptyDataArray)
            {
                return new List<int>();
            }

            val += SaveData.Dimension;
            List<int> result = new List<int>();
            string txt = TextLib.EmptyString;
            for (int i = 0; i < val.Length; i++ )
            {
                if (val[i] == SaveData.RepeateDataArray)
                {
                    //the last value is repeated a certain number of times
                    //*10R5] ten is repeated five times
                    int repeateVal = SafeStringToInt(txt);
                    txt = TextLib.EmptyString;
                    for (i = i + 1; i < val.Length; i++)
                    {
                        if (val[i] != SaveData.EndRepeateDataArray)
                        {
                            txt += val[i];
                        }
                        else
                        {
                            i++; //to jump over next *
                            break;
                        }
                            
                    }
                    int numRepetitions = SafeStringToInt(txt);
                    txt = TextLib.EmptyString;
                    for (int rep = 0; rep < numRepetitions; rep++)
                    {
                        result.Add(repeateVal);
                    }
                }
                else if (val[i] == SaveData.Dimension)
                {
                    result.Add(SafeStringToInt(txt));
                    txt = TextLib.EmptyString;
                }
                else
                { txt += val[i]; }
            }
            return result;
        }

        public static List<int> UncompressStingIntDimentionsVer2(string val)
        {
            if (val == SaveData.EmptyDataArray)
            {
                return new List<int>();
            }

            val += SaveData.Dimension;
            List<int> result = new List<int>();
            string txt = TextLib.EmptyString;
            bool repetitions = false;
            int value = 0;
            foreach (char c in val)
            {
                if (c == SaveData.Dimension)
                {
                    if (repetitions)
                    {
                        int numRep = lib.SafeStringToInt(txt);
                        for (int i = 0; i < numRep; i++)
                        {
                            result.Add(value);
                        }
                    }
                    else
                    {
                        value = lib.SafeStringToInt(txt);
                    }
                    repetitions = !repetitions;
                    txt = TextLib.EmptyString;
                }
                else
                {
                    txt += c;
                }
            }
            
            return result;
        }

        public static bool EvenValue(int value)
        {
            return (value & 1) == 0;
        }
        
        public static string BoolTo10(bool val)
        {
            if (val) { return "1"; }
            return "0";
        }
        /// <summary>
        /// Returns a 1 or 0
        /// </summary>
        public static int BoolToInt01(bool val)
        {
            return val ? 1 : 0;
        }

        public static int AlternateBetweenTwoValues(int currentValue, int value1, int value2)
        {
            return currentValue == value1 ? value2 : value1;
        }

        public static bool ToBool(int value)
        {
            return value != 0;
        }

        public static void AngleInside2Pi(ref float angle)
        {
            if (angle < 0) { angle += MathHelper.TwoPi; }
            else if (angle > MathHelper.TwoPi) { angle -= MathHelper.TwoPi; }
        }
        public static void AngleInside2PiSigned(ref float angle)
        {
            if (angle < -MathHelper.TwoPi) { angle += MathHelper.TwoPi; }
            else if (angle > MathHelper.TwoPi) { angle -= MathHelper.TwoPi; }
        }
        public static float SmallestValue(float val1, float val2)
        {
            return val1 < val2 ? val1 : val2;
        }
        public static int SmallestValue(int val1, int val2)
        {
            return val1 < val2 ? val1 : val2;
        }
        public static int SmallestAbsoluteValue(int val1, int val2)
        {
            return Math.Abs(val1) < Math.Abs(val2) ? val1 : val2;
        }
        public static int LargestValue(int val1, int val2)
        {
            return val1 > val2 ? val1 : val2;
        }
        public static float LargestAbsoluteValue(float val1, float val2)
        {
            return Math.Abs(val1) > Math.Abs(val2) ? val1 : val2;
        }
        public static float LargestValue(float val1, float val2)
        {
            return val1 > val2 ? val1 : val2;
        }
        public static int LargestValue(int val1, int val2, int val3)
        {
            if (val1 > val2)
            {
                return val1 > val3 ? val1 : val3;
            }
            return val2 > val3 ? val2 : val3;
        }

        public static int SmallestValue(int val1, int val2, int val3)
        {
            if (val1 < val2)
            {
                return val1 < val3 ? val1 : val3;
            }
            return val2 < val3 ? val2 : val3;
        }
        public static float SmallestValue(float val1, float val2, float val3)
        {
            if (val1 < val2)
            {
                return val1 < val3 ? val1 : val3;
            }
            return val2 < val3 ? val2 : val3;
        }

        public static float SquareLength(Vector3 pos1, Vector3 pos2)
        {
            float longest = Math.Abs(pos1.X - pos2.X);
            float val2 = Math.Abs(pos1.Y - pos2.Y);
            if (val2 > longest)
                longest = val2;
            val2 = Math.Abs(pos1.Z - pos2.Z);
            if (val2 > longest)
                return val2;
            return longest;
        }


        public static int TileRadiusToArea(int radius)
        {
            int w = radius * 2 + 1;
            return w * w;
        }

        public static string BoolToScript(bool value)
        { return value ? "t" : "f"; }
        public static bool ScriptToBool(string script)
        { return script == "t"; }

        public static Vector3 QuaternionToEuler(Quaternion q)
        {
            Vector3 v = Vector3.Zero;

            v.X = (float)Math.Atan2
            (
                2 * q.Y * q.W - 2 * q.X * q.Z,
                   1 - 2 * Math.Pow(q.Y, 2) - 2 * Math.Pow(q.Z, 2)
            );

            v.Z = (float)Math.Asin
            (
                2 * q.X * q.Y + 2 * q.Z * q.W
            );

            v.Y = (float)Math.Atan2
            (
                2 * q.X * q.W - 2 * q.Y * q.Z,
                1 - 2 * Math.Pow(q.X, 2) - 2 * Math.Pow(q.Z, 2)
            );

            if (q.X * q.Y + q.Z * q.W == 0.5)
            {
                v.X = (float)(2 * Math.Atan2(q.X, q.W));
                v.Y = 0;
            }

            else if (q.X * q.Y + q.Z * q.W == -0.5)
            {
                v.X = (float)(-2 * Math.Atan2(q.X, q.W));
                v.Y = 0;
            }

            return v;
        }
        public static float UnsignedAngleBetweenTwoV3(Vector3 v1, Vector3 v2)
        {
            v1.Normalize();
            v2.Normalize();
            double Angle = (float)Math.Acos(Vector3.Dot(v1, v2));
            return (float)Angle;
        }

        /// Find the angle between two vectors. This will not only give the angle difference, but the direction.
        /// For example, it may give you -1 radian, or 1 radian, depending on the direction. Angle given will be the 
        /// angle from the FromVector to the DestVector, in radians.
        /// </summary>
        /// <param name="FromVector">Vector to start at.</param>
        /// <param name="DestVector">Destination vector.</param>
        /// <param name="DestVectorsRight">Right vector of the destination vector</param>
        /// <returns>Signed angle, in radians</returns>        
        /// <remarks>All three vectors must lie along the same plane.</remarks>
        public static float SignedAngleBetweenTwoV3(Vector3 FromVector, Vector3 DestVector, Vector3 DestVectorsRight)
        {
            FromVector.Normalize();
            DestVector.Normalize();
            DestVectorsRight.Normalize();

            float forwardDot = Vector3.Dot(FromVector, DestVector);
            float rightDot = Vector3.Dot(FromVector, DestVectorsRight);

            // Keep dot in range to prevent rounding errors
            forwardDot = MathHelper.Clamp(forwardDot, -1.0f, 1.0f);

            double angleBetween = Math.Acos(forwardDot);

            if (rightDot < 0.0f)
                angleBetween *= -1.0f;

            return (float)angleBetween;
        }

        public static Vector2 AngleToV2(float angle, float lenght)
        {
            Vector2 direction = Vector2.Zero;
            direction.X = (float)(Math.Sin(angle) * lenght);
            direction.Y = (float)(Math.Cos(angle) * -lenght);
            return direction;
        }
        public static float V2ToAngle(Vector2 direction)
        {
            if (direction == Vector2.Zero) return 0;
            direction.Normalize();
            return (float)Math.Atan2(direction.X, -direction.Y);

        }
        //public static Vector2 ChangeV2Angle(Vector2 dir, Rotation1D angleDiff)
        //{
        //    angleDiff.Add(Rotation1D.FromDirection(dir));
        //    return angleDiff.Direction(dir.Length());
        //}
        
        //public static Color HSL2RGB(double h, double sl, double l)
        /// <param name="hue">Hue in 0-1</param>
        public static Color HSL2RGB(double hue, double saturation, double lightness)
        {

            double v;

            double r, g, b;

            //r = l;   // default to gray
            //g = l;
            //b = l;
            //v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            r = lightness;   // default to gray
            g = lightness;
            b = lightness;
            v = (lightness <= 0.5) ? (lightness * (1.0 + saturation)) : (lightness + saturation - lightness * saturation);

            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;
               // m = l + l - v;
                m = lightness + lightness - v;
                sv = (v - m) / v;
                //h *= 5.99;
                //sextant = (int)h;
                //fract = h - sextant;
                hue *= 5.99;
                sextant = (int)hue;
                fract = hue - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                       break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }

            }

            //Color rgb = new Color((float)r, (float)g, (float)b); 
            return new Color((float)r, (float)g, (float)b);

            //return rgb;
        }
        public static Vector3 RGB2HSL(Color rgb)
        {
            //Vector3 result = Vector3.Zero;
            Vector3 resultHLS = Vector3.Zero;
            Vector3 rgbV3 = rgb.ToVector3();

            double r = rgbV3.X;
            double g = rgbV3.Y;
            double b = rgbV3.Z;


            double v;

            double m;

            double vm;

            double r2, g2, b2;
            v = Math.Max(r, g);

            v = Math.Max(v, b);

            m = Math.Min(r, g);

            m = Math.Min(m, b);

            //result.Z = (float)((m + v) / 2.0);
            resultHLS.Z = (float)((m + v) / 2.0);

            //if (result.Z <= 0.0)
            if (resultHLS.Z <= 0.0)
            {

                return Vector3.Zero;

            }

            vm = v - m;

            //result.Y = (float)vm;
            resultHLS.Y = (float)vm;

            //if (result.Y > 0.0)
            if (resultHLS.Y > 0.0)
            {

                //result.Y /= (float)((result.Y <= 0.5f) ? (v + m) : (2.0 - v - m));
                resultHLS.Y /= (float)((resultHLS.Y <= 0.5f) ? (v + m) : (2.0 - v - m));

            }

            else
            {

                return Vector3.One;

            }

            r2 = (v - r) / vm;

            g2 = (v - g) / vm;

            b2 = (v - b) / vm;

            if (r == v)
            {

                //result.X = (float)(g == m ? 5.0 + b2 : 1.0 - g2);
                resultHLS.X = (float)(g == m ? 5.0 + b2 : 1.0 - g2);

            }

            else if (g == v)
            {

                //result.X = (float)(b == m ? 1.0 + r2 : 3.0 - b2);
                resultHLS.X = (float)(b == m ? 1.0 + r2 : 3.0 - b2);

            }

            else
            {

                //result.X = (float)(r == m ? 3.0 + g2 : 5.0 - r2);
                resultHLS.X = (float)(r == m ? 3.0 + g2 : 5.0 - r2);

            }

            //result.X /= 6.0f;
            //return result;
            resultHLS.X /= 6.0f;
            return resultHLS;
        }
                
        public static Color GetColor(MyColor col)
        {
            switch (col)
            {
                default:
                    return Color.Black;
                case MyColor.Dark_blue:
                    return Color.DarkBlue;
                case MyColor.Dark_Gray:
                    return Color.DarkGray;
                case MyColor.Sky_blue:
                    return Color.CornflowerBlue;
                case MyColor.White:
                    return Color.White;
            }
        }

        public static bool SafeDelete(IDeleteable obj)
        {
            if (obj != null && !obj.IsDeleted)
            {
                obj.DeleteMe();
                return true;
            }
            return false;
        }

        public static void DeleteAndNull(ref IDeleteable obj)
        {
            if (obj != null && !obj.IsDeleted)
            {
                obj.DeleteMe();                
            }
            obj = null;
        }

        public static T IfElseIf<T>(bool ifCond, T firstConditionTrue, bool elseIfCond, T secondConditionTrue, T secondConditionFalse)
        {
            if (ifCond) return firstConditionTrue;
            else if (elseIfCond) return secondConditionTrue;
            else return secondConditionFalse;
        }

        /// <summary>
        /// Adds a value, replaces it if already exists
        /// </summary>
        public static void DictionaryAddOrReplace<key, val>(Dictionary<key, val> dictionary, key k, val v)
        {
            if (dictionary.ContainsKey(k))
                dictionary[k] = v;
            else
                dictionary.Add(k, v);
        }

        public static key DictionaryKeyFromValue<key, val>(Dictionary<key, val> dictionary, val v)
        {
            foreach (KeyValuePair<key, val> kv in dictionary)
            {
                if (kv.Value.Equals(v))
                {
                    return kv.Key;
                }
            }
            throw new Exception("Dictionary is missing the value: " + v.ToString());
        }

        static readonly CubeFace[] FacingToCubefaceArray = { CubeFace.Znegative, CubeFace.Xpositive, CubeFace.Zpositive, CubeFace.Xnegative };
        public static CubeFace FacingToCubeface(Dir4 dir)
        {
            return FacingToCubefaceArray[(int)dir];
        }

        public static Dimensions GetPerpendicularDimensionXY(Dimensions dim)
        {
            return dim == Dimensions.X ? Dimensions.Y : Dimensions.X;
        }
        public static Dir4 GetPerpendicularDirection(Dir4 dir)
        {
            return (Dir4)Bound.SetRollover((int)dir + 1, 0, 3);
        }
        public static CubeFace GetPerpendicularDirection(CubeFace dir, Dir4 lookDir)
        {
            switch (dir)
            {
                case CubeFace.Xnegative: return CubeFace.Zpositive;
                case CubeFace.Zpositive: return CubeFace.Xpositive;
                case CubeFace.Xpositive: return CubeFace.Znegative;
                case CubeFace.Znegative: return CubeFace.Xnegative;
                case CubeFace.Ynegative: // Fallthrough
                case CubeFace.Ypositive:
                    switch (lookDir)
                    {
                        case Dir4.E: return CubeFace.Znegative;
                        case Dir4.N: return CubeFace.Xpositive;
                        case Dir4.S: return CubeFace.Xnegative;
                        case Dir4.W: return CubeFace.Zpositive;
                        default: throw new NotImplementedException("Bad lookDir!");
                    }
                default: throw new NotImplementedException("Bad dir!");
            }
        }
        public static Dir4 OppositeDir(Dir4 dir)
        {
            return (Dir4)(((int)dir + 2) % 4);
        }
        public static CubeFace GetOppositeDirection(CubeFace facing)
        {
            switch (facing)
            {
                case CubeFace.Xnegative: return CubeFace.Xpositive;
                case CubeFace.Xpositive: return CubeFace.Xnegative;
                case CubeFace.Ynegative: return CubeFace.Ypositive;
                case CubeFace.Ypositive: return CubeFace.Ynegative;
                case CubeFace.Znegative: return CubeFace.Zpositive;
                case CubeFace.Zpositive: return CubeFace.Znegative;
                default: throw new NotImplementedException("Bad call, please give a valid facing");
            }
        }
        public static bool IsDirAlongAxisZ_NS(Dir4 dir)
        {
            if (dir == Dir4.N || dir == Dir4.S)
                return true;
            return false;
        }
        public static bool IsDirAlongAxisX_EW(Dir4 dir)
        {
            if (dir == Dir4.E || dir == Dir4.W)
                return true;
            return false;
        }
        public static bool IsDirNorthOrWest(Dir4 dir)
        {
            if (dir == Dir4.N || dir == Dir4.W)
                return true;
            return false;
        }
        static void ChangeStructElementTest()
        {
            IntVector2[] array = { new IntVector2(1, 1), new IntVector2(2, 2) };
            
            int index = 1;
            IntVector2 n = array[index];
            n.Y = 10;
            
            array[index] = n;

        }

        /// <returns>Changed to a new value</returns>
        public static bool ChangeValue<T>(ref T value, T changeTo)
        {
            if (value.Equals(changeTo))
            {
                return false;
            }
            else
            {
                value = changeTo;
                return true;
            }
        }

        /// <summary>
        /// Helper to change values in a array of structs
        /// </summary>
        /// <typeparam name="T">A struct</typeparam>
        /// <param name="overridingValues">will override values that arent null/non/default</param>
        public static void ChangeStructArrayValue(IUpdateableStructInArray[] array, int arrayIndex, IUpdateableStructInArray overridingValues) 
        {
            IUpdateableStructInArray element = array[arrayIndex];
            element.ChangeStructArrayValue(overridingValues);
            array[arrayIndex] = element;
        }

        public static void ChangeStructArrayValue<T>(T[,] array, int arrayGrindexX, int arrayGrindexY, T overridingValues) 
            where T : IUpdateableStructInArray
        {
            T element = array[arrayGrindexX, arrayGrindexY];
            element.ChangeStructArrayValue(overridingValues);
            array[arrayGrindexX, arrayGrindexY] = element;
        }

        public static void SetValueIfNotEqualTo<T>(ref T value, T setTo, T ifNotEqualTo)
        {
            if (!setTo.Equals(ifNotEqualTo)) value = setTo;
        }

        /// <summary>
        /// Simply tells what axis the cubeface is working on
        /// </summary>
        /// <param name="facing">The CubeFace</param>
        /// <returns>The dimension of the CubeFace</returns>
        public static Dimensions CubeFaceToDimensions(CubeFace facing)
        {
            switch (facing)
            {
                case CubeFace.Xnegative: return Dimensions.X;
                case CubeFace.Xpositive: return Dimensions.X;
                case CubeFace.Ynegative: return Dimensions.Y;
                case CubeFace.Ypositive: return Dimensions.Y;
                case CubeFace.Znegative: return Dimensions.Z;
                case CubeFace.Zpositive: return Dimensions.Z;
                default: throw new NotImplementedException("Bad call, please give a valid facing");
            }
        }

        /// <summary>
        /// Simply tells what axis the facing is working on
        /// </summary>
        /// <param name="facing">The facing</param>
        /// <returns>The dimension of the facing</returns>
        public static Dimensions Facing4DirToDimensionsXZ(Dir4 facing)
        {
            switch (facing)
            {
                case Dir4.E: return Dimensions.X;
                case Dir4.W: return Dimensions.X;
                case Dir4.N: return Dimensions.Z;
                case Dir4.S: return Dimensions.Z;
                default: throw new NotImplementedException("Bad call, please give a valid facing");
            }
        }

        /// <summary>
        /// Simply tells what axis the facing is working on
        /// </summary>
        /// <param name="facing">The facing</param>
        /// <returns>The dimension of the facing</returns>
        public static Dimensions Dir4ToDimensionsXY(Dir4 facing)
        {
            switch (facing)
            {
                case Dir4.E: return Dimensions.X;
                case Dir4.W: return Dimensions.X;
                case Dir4.N: return Dimensions.Y;
                case Dir4.S: return Dimensions.Y;
                default: throw new NotImplementedException("Bad call, please give a valid facing");
            }
        }

        /// <summary>
        /// Gets a letter block material appropriate to display a character
        /// </summary>
        /// <param name="c">The character to match</param>
        /// <returns>A MaterialType letter block</returns>
        public static LootFest.Data.MaterialType LetterBlockFromChar(char c)
        {
            c = char.ToLower(c);

            switch (c)
            {
                default:
                    return LootFest.Data.MaterialType.empty_letter;
                    
                case 'a':
                    return LootFest.Data.MaterialType.LetterA;

                case 'b':
                    return LootFest.Data.MaterialType.LetterB;

                case 'c':
                    return LootFest.Data.MaterialType.LetterC;

                case 'd':
                    return LootFest.Data.MaterialType.LetterD;

                case 'e':
                    return LootFest.Data.MaterialType.LetterE;
                    
                case 'f':
                    return LootFest.Data.MaterialType.LetterF;
                    
                case 'g':
                    return LootFest.Data.MaterialType.LetterG;
                    
                case 'h':
                    return LootFest.Data.MaterialType.LetterH;
                    
                case 'i':
                    return LootFest.Data.MaterialType.LetterI;
                    
                case 'j':
                    return LootFest.Data.MaterialType.LetterJ;
                    
                case 'k':
                    return LootFest.Data.MaterialType.LetterK;
                    
                case 'l':
                    return LootFest.Data.MaterialType.LetterL;
                    
                case 'm':
                    return LootFest.Data.MaterialType.LetterM;
                    
                case 'n':
                    return LootFest.Data.MaterialType.LetterN;
                    
                case 'o':
                    return LootFest.Data.MaterialType.LetterO;
                    
                case 'p':
                    return LootFest.Data.MaterialType.LetterP;
                    
                case 'q':
                    return LootFest.Data.MaterialType.LetterQ;
                    
                case 'r':
                    return LootFest.Data.MaterialType.LetterR;
                    
                case 's':
                    return LootFest.Data.MaterialType.LetterS;
                    
                case 't':
                    return LootFest.Data.MaterialType.LetterT;
                    
                case 'u':
                    return LootFest.Data.MaterialType.LetterU;
                    
                case 'v':
                    return LootFest.Data.MaterialType.LetterV;
                    
                case 'w':
                    return LootFest.Data.MaterialType.LetterW;
                    
                case 'x':
                    return LootFest.Data.MaterialType.LetterX;
                    
                case 'y':
                    return LootFest.Data.MaterialType.LetterY;
                    
                case 'z':
                    return LootFest.Data.MaterialType.LetterZ;
                    
                case '0':
                    return LootFest.Data.MaterialType.Letter0;
                    
                case '1':
                    return LootFest.Data.MaterialType.Letter1;
                    
                case '2':
                    return LootFest.Data.MaterialType.Letter2;
                    
                case '3':
                    return LootFest.Data.MaterialType.Letter3;
                    
                case '4':
                    return LootFest.Data.MaterialType.Letter4;
                    
                case '5':
                    return LootFest.Data.MaterialType.Letter5;
                    
                case '6':
                    return LootFest.Data.MaterialType.Letter6;
                    
                case '7':
                    return LootFest.Data.MaterialType.Letter7;

                case '8':
                    return LootFest.Data.MaterialType.Letter8;

                case '9':
                    return LootFest.Data.MaterialType.Letter9;

                case '.':
                    return LootFest.Data.MaterialType.LetterDot;

                case ',':
                    return LootFest.Data.MaterialType.LetterDot;

                case '?':
                    return LootFest.Data.MaterialType.LetterQuest;

                case '!':
                    return LootFest.Data.MaterialType.LetterExpress;
            }
        }
        
        /// <summary>
        /// Get the facing that is perpendicular but positive (North or East)
        /// </summary>
        /// <param name="dir">direction to compare</param>
        /// <returns>A positive perpendicular direction</returns>
        public static Dir4 GetPositivePerpendicularFacing(Dir4 dir)
        {
            switch (dir)
            {
                case Dir4.N: // Fallthrough.;
                case Dir4.S: return Dir4.E;
                case Dir4.W: // Fallthrough.
                case Dir4.E: return Dir4.S;
                default: throw new ArgumentException("Please give a valid facing!");
            }
        }

        /// <summary>
        /// Get the facing that runs along the same dimension and is positive
        /// </summary>
        /// <param name="dir">direction to compare</param>
        /// <returns>positive direction along same axis</returns>
        public static Dir4 GetPositiveFacing(Dir4 dir)
        {
            switch (dir)
            {
                case Dir4.N: // Fallthrough.;
                case Dir4.S: return Dir4.S;
                case Dir4.W: // Fallthrough.
                case Dir4.E: return Dir4.E;
                default: throw new ArgumentException("Please give a valid facing!");
            }
        }
        /// <summary>
        /// Converts a Facing4Dir variable to a DirectionFlags variable.
        /// </summary>
        /// <param name="dir">Facing</param>
        /// <returns>Equivalent DirectionFlags value</returns>
        public static DirectionFlags Dir4ToDirectionFlags(Dir4 dir)
        {
            switch (dir)
            {
                case Dir4.N: return DirectionFlags.North;
                case Dir4.S: return DirectionFlags.South;
                case Dir4.W: return DirectionFlags.West;
                case Dir4.E: return DirectionFlags.East;
                default: return DirectionFlags.NONE;
            }
        }

        /// <summary>
        /// True if val is odd
        /// </summary>
        /// <param name="val">value</param>
        /// <returns>true if val is odd</returns>
        public static bool IsOdd(int val)
        {
            return val % 2 == 1;
        }

        /// <summary>
        /// True if val is even
        /// </summary>
        /// <param name="val">value</param>
        /// <returns>true if val is even</returns>
        public static bool IsEven(int val)
        {
            return val % 2 == 0;
        }

        public static Dir4[] AllDirections()
        {
            return new Dir4[] { Dir4.E, Dir4.N, Dir4.S, Dir4.W };
        }

        public static Action Combine(Action a1, Action a2)
        {
            a1 += a2;
            return a1;
        }
    }
}
