using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.PJ.MiniGolf;

namespace VikingEngine
{
    public delegate void ActionIndexEvent(int index);
    public delegate void ActionDoubleIndexEvent(int index1, int index2);

    public delegate void ActionBoolEvent(bool value);

    public delegate void WriteBinaryStream(System.IO.BinaryWriter w);
    public delegate void ReadBinaryStream(System.IO.BinaryReader r);

    /// <summary>
    /// Event called upoin after text box input
    /// </summary>
    /// <param name="result">input, null is canceled</param>
    public delegate void TextInputEvent(string result, object tag);

    public delegate bool BoolGetSet(int index, bool set, bool value);
    public delegate bool BoolGetSet_Tag(object tag, bool set, bool value);
    public delegate int IntGetSet(bool set, int value);
    public delegate int IntGetSetIx(int index, bool set, int value);
    public delegate int IntGetSetTag(object tag, bool set, int value);
    public delegate float FloatGetSet(bool set, float value);
    public delegate float FloatGetSetTag(object tag, bool set, float value);
    public delegate Color ColorGetSet(bool set, Color value);
    public delegate T GenericGetSet<T>(bool set, T value);
    public delegate string StringGetSet(bool set, string value);



    public class GetSet
    {
        public static T Do<T>(bool set, ref T original, T value)
        {
            if (set)
            {
                original = value;
            }

            return original;
        }
    }
}
