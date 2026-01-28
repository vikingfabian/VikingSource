using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign.CardData
{
    struct Number
    {
        public static readonly Range PositiveBounds = new Range(0, MaxValue);
        public static readonly Range Bounds = new Range(-MaxValue, MaxValue);
        public const int MaxValue = 999999;

        public static readonly Number Endless = new Number(MaxValue + 1);
        public int value;

        public Number()
        { }

        public Number(int value) 
        { this.value = value; }

        public int UiProperty(object tag, bool set, int value)
        {
            if (set)
            {
                this.value = value;
            }
            return this.value;
        }

        public bool IsInfinite => Math.Abs(value) > MaxValue;

        public override string ToString()
        {
            if (value > MaxValue)
            {
                return "∞";
            }
            else if (value < -MaxValue)
            {
                return "-∞";
            }
            return value.ToString();
        }

        public string PlusMinusString()
        {
            if (value > MaxValue)
            {
                return "+∞";
            }
            else if (value < -MaxValue)
            {
                return "-∞";
            }
            if (value > 0)
            { 
                return "+" + value.ToString();
            }
            return value.ToString();
        }

        public void Add(int add)
        {
            if (Math.Abs(add) > MaxValue)
            {
                value = add;
            }
            else if (Math.Abs(value) <= MaxValue)
            { 
                value += add;
            }
        }
    }
}
