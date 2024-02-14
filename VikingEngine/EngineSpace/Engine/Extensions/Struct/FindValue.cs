using System;
using System.Collections.Generic;

namespace VikingEngine
{
    struct FindMinValue
    {
        public bool hasValue;
        bool absolute;
        public int minMemberIndex;
        public float minValue;

        public float AbsMinValue
        {
            get { return Math.Abs(minValue); }
        }

        public FindMinValue(bool absolute)
             : this()
        {
            this.absolute = absolute;
        }
        public void Next(float value, int index)
        {
            if (!hasValue)
            {
                hasValue = true;
                minValue = float.MaxValue;
            }
            if (value < minValue)
            {
                minValue = value;
                minMemberIndex = index;
            }
        }
    }

    struct FindMaxValue
    {
        bool absolute;
        public int maxMemberIndex;
        public float maxValue;

        public FindMaxValue(bool absolute)
        {
            maxMemberIndex = 0;
            this.absolute = absolute;
            maxValue = float.MinValue;
        }
        public FindMaxValue(bool absolute, float minValue)
        {
            maxMemberIndex = 0;
            this.absolute = absolute;
            maxValue = minValue;
        }
        public void Next(float value)
        {
            if (value > maxValue)
            {
                maxValue = value;
            }
        }
        public void Next(float value, int index)
        {
            if (value > maxValue)
            {
                maxValue = value;
                maxMemberIndex = index;
            }
        }
    }

    struct FindMinValuePointer<T>
    {
        public bool hasValue;
        bool absolute;
        public T minMember;
        public float minValue;

        public FindMinValuePointer(bool absolute)
            : this()
        {
            this.absolute = absolute;
        }
        public void Next(float value, T member)
        {
            if (!hasValue)
            {
                hasValue = true;
                minValue = float.MaxValue;
            }
            if (value < minValue)
            {
                minValue = value;
                minMember = member;
            }
        }
    }

    struct FindMaxValuePointer<T>
    {
        bool absolute;
        public T maxMember;
        public float maxValue;

        public FindMaxValuePointer(bool absolute)
        {
            maxMember = default(T);
            this.absolute = absolute;
            maxValue = float.MinValue;
        }
        public FindMaxValuePointer(bool absolute, float minValue)
        {
            maxMember = default(T);
            this.absolute = absolute;
            maxValue = minValue;
        }
        public void Next(float value)
        {
            if (value > maxValue)
            {
                maxValue = value;
            }
        }
        public void Next(float value, T member)
        {
            if (value > maxValue)
            {
                maxValue = value;
                maxMember = member;
            }
        }

        public bool HasMember => maxMember != null;
    }

    class FindValues<T>
    {
        FindValueType find;
        int capacity;
        public List<ValuePointerPair<T>> values; 

        public FindValues(FindValueType find, int cap)
        {
            this.find = find;
            this.capacity = cap;

            values = new List<ValuePointerPair<T>>(cap);
        }

        public void next(float value, T member)
        {
            if (values.Count < capacity)
            {
                values.Add(new ValuePointerPair<T>(value, member));
            }
            else
            {
                switch (find)
                {
                    case FindValueType.Max:
                        for (int i = 0; i < values.Count; ++i)
                        {
                            if (value > values[i].value)
                            {
                                values[i].set(value, member);
                                return;
                            }
                        }
                        break;
                    case FindValueType.Min:
                        for (int i = 0; i < values.Count; ++i)
                        {
                            if (value < values[i].value)
                            {
                                values[i].set(value, member);
                                return;
                            }
                        }
                        break;
                }
            }
        }
    }

    class ValuePointerPair<T>
    {
        public float value;
        public T pointer;

        public ValuePointerPair(float value, T pointer)
        {
            this.value = value;
            this.pointer = pointer;
        }

        public void set(float value, T pointer)
        {
            this.value = value;
            this.pointer = pointer;
        }
    }

    enum FindValueType
    {
        Min,
        Max,
    }

}
