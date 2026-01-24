using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign
{
    static class Const
    {
        public static readonly Range PositiveBounds = new Range(0, MaxValue);
        public static readonly Range Bounds = new Range(-MaxValue, MaxValue);
        public const int MaxValue = 999999;
    }
}
