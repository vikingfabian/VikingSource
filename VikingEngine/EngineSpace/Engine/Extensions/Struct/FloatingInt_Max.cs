﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    struct FloatingInt_Max
    {
        public double value;
        public int max;

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

        public void add(double add)
        {
            value += add;
            if (value > max)
            {
                value = max;
            }
        }

        public void add(double add, int max)
        {
            value += add;
            if (value > max)
            {
                value = max;
            }
        }

        public void setMax(double max)
        {
            this.max= (int)Math.Floor(max);
            if (value > max)
            {
                value = max;
            }
        }

        public void reduceTowardsZero(double reduce)
        {
            value -= reduce;
            if (value < 0)
            { value = 0; }
        }

        public bool HasValue()
        { return value >= 1.0; }


        public bool IsMax()
        {
            return value >= max;
        }

        public override string ToString()
        {
            return Int().ToString() + " / " + max.ToString();
        }

        public void write16bit(System.IO.BinaryWriter w)
        { 
            w.Write(Convert.ToUInt16(value));
            w.Write(Convert.ToUInt16(max));
        }

        public void read16bit(System.IO.BinaryReader r)
        {
            value = r.ReadUInt16();
            max = r.ReadUInt16();
        }
    }
}
