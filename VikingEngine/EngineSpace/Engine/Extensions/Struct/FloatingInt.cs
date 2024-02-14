using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VikingEngine
{ 
    struct FloatingInt
    {
        public double value;

        public int Int()
        { 
            return (int)Math.Floor(value);
        }

        public int pull(int max)
        {
            double pullValue = Math.Min(Math.Floor(value), max);
            value -= pullValue;
            return (int)pullValue;
        }

        public bool pay(int amount, bool allowDept)
        {
            if (allowDept || value >= amount)
            {
                value -= amount;
                return true;
            }
            
            return false;
        }

        public void add(double add, double max)
        {
            value += add;
            if (value > max)
            { 
                value = max;
            }
        }

        public void reduceTowardsZero(double reduce)
        {
            value-=reduce;
            if (value < 0) 
            { value = 0; }
        }

        public bool HasValue()
        {  return value >= 1.0; }
    }
}
