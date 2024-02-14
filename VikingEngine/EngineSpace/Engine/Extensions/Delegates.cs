using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;

namespace VikingEngine
{
    delegate void ActionIndexEvent(int index);
    delegate void ActionDoubleIndexEvent(int index1, int index2);

    delegate void ActionBoolEvent(bool value);

    delegate void WriteBinaryStream(System.IO.BinaryWriter w);
    delegate void ReadBinaryStream(System.IO.BinaryReader r);

    /// <summary>
    /// Event called upoin after text box input
    /// </summary>
    /// <param name="result">input, null is canceled</param>
    public delegate void TextInputEvent(int user, string result, int index);
    
    delegate bool BoolGetSet(int index, bool set, bool value);
    delegate int IntGetSet(bool set, int value);
    delegate float FloatGetSet(bool set, float value);
    delegate Color ColorGetSet(bool set, Color value);
    delegate T GenericGetSet<T>(bool set, T value);
    delegate string StringGetSet(bool set, string value);

    class GetSet
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
