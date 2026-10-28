using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.EngineSpace.Engine.Extensions.Struct
{
    struct EndlessInt
    {
        public static readonly EndlessInt NoLimit = new EndlessInt(NoLimitValue);
        public static readonly EndlessInt Null = new EndlessInt(IsNullValue);

        const int NoLimitValue = int.MaxValue;
        const int IsNullValue = int.MinValue;
        
        public int Value;

        public EndlessInt(int value)
        {
            Value = value;
        }

        public bool IsUnlimited => Value == NoLimitValue;

        public bool IsNull => Value == IsNullValue;

        public static bool operator ==(EndlessInt a, int b) => a.Value == b;
        public static bool operator !=(EndlessInt a, int b) => a.Value != b;

        public static bool operator ==(EndlessInt a, EndlessInt b) => a.Value == b.Value;
        public static bool operator !=(EndlessInt a, EndlessInt b) => a.Value != b.Value;

        public override bool Equals(object obj)
        {
            if (obj is EndlessInt other) return this.Value == other.Value;
            if (obj is int otherInt) return this.Value == otherInt;
            return false;
        }

        public bool Equals(EndlessInt other) => this.Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();

        // --- Comparison Operators ---
        // Note: C# requires > and < to be defined whenever >= and <= are overloaded.

        public static bool operator >=(EndlessInt a, int b) => a.Value >= b;
        public static bool operator <=(EndlessInt a, int b) => a.Value <= b;
        public static bool operator >(EndlessInt a, int b) => a.Value > b;
        public static bool operator <(EndlessInt a, int b) => a.Value < b;

        public static bool operator >=(EndlessInt a, EndlessInt b) => a.Value >= b.Value;
        public static bool operator <=(EndlessInt a, EndlessInt b) => a.Value <= b.Value;
        public static bool operator >(EndlessInt a, EndlessInt b) => a.Value > b.Value;
        public static bool operator <(EndlessInt a, EndlessInt b) => a.Value < b.Value;

        // --- Arithmetic Operators ---

        public static EndlessInt operator +(EndlessInt a, int b)
        {
            if (a.IsUnlimited || a.IsNull)
                return a;

            return new EndlessInt(a.Value + b);
        }

        public static EndlessInt operator -(EndlessInt a, int b)
        {
            if (a.IsUnlimited || a.IsNull)
                return a; 

            return new EndlessInt(a.Value - b);
        }

        public static implicit operator EndlessInt(int value) => new EndlessInt(value);

        public string ValueString(bool largeValueFormat, string noLimitString, string nullString)
        {
            if (IsUnlimited)
            {
                return noLimitString;
            }
            else if (IsNull)
            {
                return nullString;
            }
            else if (largeValueFormat)
            {
                return TextLib.LargeNumber(Value);
            }
            else
            {
                return Value.ToString();
            }
        }
    }
}
