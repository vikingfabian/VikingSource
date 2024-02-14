using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine
{
    interface ICirkleCounter
    {
        int Value { get; }
        int Max { get; }
        int Next();
        void Reset();
    }

    struct FrameStamp
    {
        public static readonly FrameStamp None = new FrameStamp(-1000000);

        public int frame;

        public FrameStamp(int frame)
        {
            this.frame = frame;
        }
        
        public void setNow()
        {
            this.frame = Ref.TotalFrameCount;
        }

        public bool framesPassed(int frames)
        {
            return Ref.TotalFrameCount - frame >= frames;
        }
    }

    struct TimeStamp
    {
        public static readonly TimeStamp None = new TimeStamp(-1000000);

        //const float SecToMs = 0.001f;
        public float totalGameTimeStampSec;
        
        public TimeStamp(float totalGameTimeStampSec)
        {
            this.totalGameTimeStampSec = totalGameTimeStampSec;
        }

        public static TimeStamp Now()
        {
            return new TimeStamp(Ref.TotalTimeSec);
        }

        public bool secPassed(float seconds)
        {
            return Ref.TotalTimeSec - totalGameTimeStampSec >= seconds;
        }
        public bool msPassed(float ms)
        {
            return Ref.TotalTimeSec - totalGameTimeStampSec >= TimeExt.MsToSec * ms;
        }

        public void setNow()
        {
            this.totalGameTimeStampSec = Ref.TotalTimeSec;
        }

        /// <summary>
        /// Time passed this frame
        /// </summary>
        public bool event_sec(float seconds)
        {
            return Ref.PrevTotalTimeSec - totalGameTimeStampSec < seconds &&
                Ref.TotalTimeSec - totalGameTimeStampSec >= seconds;
        }

        /// <summary>
        /// Time passed this frame
        /// </summary>
        public bool event_ms(float ms)
        {
            return event_sec(ms * TimeExt.MsToSec);
        }

        public float Hours
        {
            get { return TimeExt.SecondsToHours(Ref.TotalTimeSec - totalGameTimeStampSec); }
        }
        public float Seconds
        {
            get { return Ref.TotalTimeSec - totalGameTimeStampSec; }
        }
        public float MilliSec
        {
            get { return TimeExt.SecondsToMS(Ref.TotalTimeSec - totalGameTimeStampSec); }
        }
    }   

    struct AddFloatPerTime
    {
        float addPerMS;
        float total;
        int dir;

        public AddFloatPerTime(float total, float addPerSec)
        {
            this.addPerMS = addPerSec * 0.001f;
            this.total = Math.Abs(total);
            this.dir = lib.ToLeftRight(total);
        }

        public float NextStep(float time)
        {
            if (total > 0)
            {
                float result = time * addPerMS;
                total -= result;
                if (total < 0)
                {
                    result += total;
                }
                return result * dir;
            }
            return 0;
        }

        public bool Alive
        { get { return total > 0; } }
    }

    struct IntInLowBound
    {
        int value;
        public int lowerBound;
        public IntInLowBound(int value, int lowerBound)
        {
            this.value = value;
            this.lowerBound = lowerBound;
        }

        public int Value
        {
            get { return value; }
            set { this.value = value; if (this.value < lowerBound) { this.value = lowerBound; } }
        }
    }

    struct ValueBar
    {
        public static readonly ValueBar None = new ValueBar(0);

        const int MinValue = 0;
        int value;
        public int maxValue;
        public bool exlusiveMax;

        public ValueBar(int value, int maxValue, bool exlusiveMax = false)
        {
            this.maxValue = maxValue;
            this.exlusiveMax = exlusiveMax;
            this.value = 0;

            Value = value;            
        }
        public ValueBar(int maxValue, bool exlusiveMax = false)
        {
            this.value = maxValue;
            this.maxValue = maxValue;
            this.exlusiveMax = exlusiveMax;
        }

        public bool canSpend(int spend)
        {
            return value >= spend;
        }
        public bool spend(int spend)
        {
            if (value >= spend)
            {
                value -= spend;
                return true;
            }
            return false;
        }

        public void WriteByteStream(System.IO.BinaryWriter w)
        {
            w.Write((byte)value);
            w.Write((byte)maxValue);
        }
        public void ReadByteStream(System.IO.BinaryReader r)
        {
            value = r.ReadByte();
            maxValue = r.ReadByte();
        }

        public bool IsZero { get { return value == 0; } }
        public bool IsNone { get { return maxValue == 0; } }
        public bool IsMax { get { return value == EndValue(); } }
        public bool HasValue { get { return value > 0; } }
        //public int ValueLeft { get { return maxValue - value; } } 

        public int Value
        {
            get { return value; }
            set { this.value = Bound.Set(value, MinValue, EndValue()); }
        }

        public int ValueRemoved
        { get { return EndValue() - value; } }

        public void add(int add)
        {
            this.value = Bound.Set(value + add, MinValue, EndValue());
        }

        public bool willAddChangeValue(int add)
        {
            int newValue = Bound.Set(value + add, MinValue, EndValue());
            return newValue != value;
        }

        public void plusOne()
        {
            if (value < EndValue())
            {
                ++value;
            }
        }
        public void minusOne()
        {
            if (value > MinValue)
            {
                --value;
            }
        }

        public void setMax()
        {
            value = EndValue();
        }

        public void setZero()
        {
            value = MinValue;
        }

        int EndValue()
        {
            return exlusiveMax ? maxValue - 1 : maxValue;
        }

        public int CompareValueAdded(ValueBar previous)
        {
            return value - previous.value;
        }

        public string BarText(bool divisorIfMax = true)
        {
            if (IsMax && !divisorIfMax)
            {
                return value.ToString();
            }
            return value.ToString() + "/" + maxValue.ToString();
        }

        public string BarSpentText()
        {
            return ValueRemoved.ToString() + "/" + maxValue.ToString();
        }
    }

    struct FloatInBound
    {
        float value;
        bool rollOver;
        public IntervalF Bounds;
        public float Value
        {
            get { return value; }
            set
            {
                this.value = Bounds.SetBounds(value);
            }
        }
        /// <summary>
        /// If you want to change the value and know if it hits the bounds
        /// </summary>
        /// <returns>if <p>value</p> was outside the bounds</returns>
        public bool SetValue(float value)
        {
            Value = value;
            return !Bounds.IsWithinRange(value);
        }
        public FloatInBound(float value, IntervalF bounds)
            : this(value, bounds, false)
        { }
        public FloatInBound(float value, IntervalF bounds, bool rollOver)
        {
            this.value = value;
            Bounds = bounds;
            this.rollOver = rollOver;
        }
        public override string ToString()
        {
            return value.ToString();
        }

        public float Percent
        {
            get
            {
                return Bounds.GetValuePercentPos(value);
            }
            set
            {
                this.value = Bounds.GetFromPercent(value);
            }
        }

        public bool IsMax { get { return value == Bounds.Max; } }
    }

    //struct IntInBound
    //{
    //    int value;
    //    bool rollOver;
    //    public Interval Bounds;
    //    public int Value
    //    {
    //        get { return value; }
    //        set
    //        {
    //            this.value = Bounds.SetBounds(value);
    //        }
    //    }
    //    /// <summary>
    //    /// If you want to change the value and know if it hits the bounds
    //    /// </summary>
    //    /// <returns>if <p>value</p> was outside the bounds</returns>
    //    public bool SetValue(int value)
    //    {
    //        Value = value;
    //        return !Bounds.IsWithinRange(value);
    //    }
    //    public IntInBound(int value, int size)
    //        : this(value, new Interval(0, size-1), false)
    //    { }
    //    public IntInBound(int value, Interval bounds)
    //        : this(value, bounds, false)
    //    { }
    //    public IntInBound(int value, Interval bounds, bool rollOver)
    //    {
    //        this.value = value;
    //        Bounds = bounds;
    //        this.rollOver = rollOver;
    //    }
    //    public override string ToString()
    //    {
    //        return value.ToString();
    //    }

    //    public bool IsMax { get { return value == Bounds.Max; } }
    //}
    /// <summary>
    /// Contains a index that you count up until it will reach max, and then go back to zero
    /// </summary>
    public struct CirkleCounter
    {
        int value;
        public int Value
        { 
            get { return value; }
            set
            { this.value = Bound.Set(value, min, max); }
        }
        int max;
        public int Max
        { 
            get { return max; }
            set { max = value; Next(0); }
        }
        int min;
        public int Min
        { get { return min; } }

        public CirkleCounter(int maxValue)
            : this(0, 0, maxValue)
        {
        }
        public CirkleCounter(int startVal,int minValue, int maxValue)
        {
            min = minValue;
            max = maxValue;
            value = 0;
        }
        public int Next(int dir)
        {
            int result = value;
            value = Bound.SetRollover(value + dir, min, max);
            return result;
        }
        public void Reset()
        {
            value = min;
        }
    }

    /// <summary>
    /// Contains a index that you count up until it will reach max, and then go back to zero
    /// </summary>
    public struct CirkleCounterUp : ICirkleCounter
    {
        public int value;
        public int Value
        { get { return value; } }
        int max;
        public int Max
        { get { return max; } }

        public CirkleCounterUp(int maxValue)
            : this(0, maxValue)
        {
        }
        public CirkleCounterUp(int startVal, int maxValue)
        {
            max = maxValue;
            value = startVal;
        }
        public static CirkleCounterUp operator ++(CirkleCounterUp val)
        {
            val.Next();
            return val;
        }
        public int Next()
        {
            int result = value;
            value++;
            if (value > max)
            {
                value = 0;
            }
            return result;
        }

        /// <returns>If it has reached past its max value, and reset</returns>
        public bool Next_IsReset()
        {
            return Next() == max;
        }
        public void Set(int value)
        {
            this.value = value % max;
        }
        public void Reset()
        {
            value = 0;
        }
    }
    /// <summary>
    /// Contains a index that you count up until it will reach max, and then go back to zero
    /// </summary>
    public struct CirkleCounterDown : ICirkleCounter
    {
        public int value;
        public int Value
        { get { return value; } }
        int max;
        public int Max
        { get { return max; } }

        public CirkleCounterDown(int maxValue)
        {
            max = maxValue;
            value = maxValue;
        }
        public int Next()
        {
            int result = value;
            value--;
            if (value < 0)
            {
                value = max;
            }
            return result;
        }
        public void Reset()
        {
            value = max;
        }
    }

    /// <summary>
    /// Contains a index that you count up until it will reach max
    /// </summary>
    public struct CountUp
    {
        public int min;
        public int max;
        public int value;
        
        public CountUp(int max)
            : this(0, 0, max)
        {
        }

        public CountUp(int min, int startValue, int max)
        {
            this.min = min;
            this.max = max;
            this.value = 0;
            Set(startValue);
        }

        public static CountUp operator ++(CountUp val)
        {
            val.Next();
            return val;
        }

        public bool Next()
        {
            if (value < max)
            {
                ++value;
                return true;
            }
            else
            {
                return false;
            }
        }
       
        public void Set(int value)
        {
            this.value = value;
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
        }

        public void Reset()
        {
            value = min;
        }
    }

    /// <summary>
    /// Will count back n forward between bounds
    /// </summary>
    public struct PingPongCounter
    {
        public int dir;
        public int Min;
        public int Max;
        public int value;

        public PingPongCounter(int minValue, int maxValue, int startValue)
        {
            dir = 1;
            this.Min = minValue;
            this.Max = maxValue;
            this.value = startValue;
        }

        public int Next()
        {
            int result = value;
            value += dir;
            if (value < Min)
            {
                value = Min;
                dir = 1;
            }
            else if (value > Max)
            {
                value = Max;
                dir = -1;
            }
            return result;
        }
    }

    struct LeftRight
    {
        const int LeftDir = -1;
        const int RightDir = 1;

        public static readonly LeftRight Left = new LeftRight(LeftDir);
        public static readonly LeftRight Right = new LeftRight(RightDir);

        public static LeftRight Random()
        {
            return new LeftRight(Ref.rnd.LeftRight());
        }


        int value;

        public LeftRight(int value)
            :this()
        {
            set(value);
        }

        public LeftRight(float value)
            : this()
        {
            set(value);
        }

        public int Value
        {
            get { return value; }

            set
            {
                set(value);                
            }
        }

        public float ValueF
        {
            get { return value; }

            set
            {
                set(value);
            }
        }

        void set(float dir)
        {
            if (dir < 0)
            {
                value = LeftDir;
            }
            else if (dir > 0)
            {
                value = RightDir;
            }
            else
            {
                value = 0;
            }
        }

        public Vector2 applyToX(Vector2 vector)
        {
            vector.X = Math.Abs(vector.X) * this.value;
            return vector;
        }

        public void Invert()
        {
            value = -value;
        }

        public bool IsRight => value > 0;
        public bool IsLeft => value < 0;

        public static implicit operator LeftRight(float value)
        {
            return new LeftRight(value);
        }

        public static float operator *(LeftRight value1, float value2)
        {
            return value1.value * value2;
        }

        public static float operator *(float value1, LeftRight value2)
        {
            return value2.value * value1;
        }
    }


    struct Pan
    {
        public static readonly Pan Center = new Pan(0);
        float side;
        public const float Min = -1;
        public const float Max = 1;


        public Pan(float _side)
        {
            side = Bound.Set(_side, Min, Max);
        }

        public float Value
        {
            set { side = Bound.Set(value, Min, Max); }
            get { return side; }
        }

        public byte ByteValue
        {
            get
            {
                return (byte)((side + 1f) / 2f * byte.MaxValue);
            }
            set
            {
                side = (float)value / byte.MaxValue * 2f - 1f;
            }
        }

        public void Add(float value)
        {
            side = Bound.Set(side + value, Min, Max);
        }

        public float GetSide(float volume, bool left)
        {
            float p = side + 1;
            p /= 2;
            if (left)
            {
                return (1 - p) * volume;
            }
            else
            {
                return p * volume;
            }
        }

        public static Pan PositionToPan(float currentPos, float minPos, float maxPos)
        {
            maxPos -= minPos;
            float percent = currentPos / maxPos;
            return new Pan(percent * 2 - 1);
        }
        public static Pan Left
        {
            get { return new Pan(Min); }
        }
        public static Pan Right
        {
            get { return new Pan(Max); }
        }

        public override string ToString()
        {
            return side.ToString();
        }
    }
   
    public struct Percent
    {
        public const float MaxPercentage = 1;
        public const int MaxTextPercentage = 100;
        public float Value;

        public Percent(int textValue)
        {
            Value = (float)textValue / MaxTextPercentage;
        }
        public Percent(float value)
        { Value = value; Value = value; }
        public Percent(float part, float wholeValue)
        { Value = 0; Value = lib.SafeDiv(part, wholeValue); }

        public int TextValue
        {
            get { return Convert.ToInt16(Value * MaxTextPercentage); }
            set { Value = (float)value / MaxTextPercentage; }
        }
        public bool DiceRoll()
        {
            return Ref.rnd.Double() < Value;
        }
        
        public override string ToString()
        {
            return TextValue.ToString() + "%";
        }
        public Percent Inverse()
        {
            return new Percent(MaxPercentage - Value);
        }
       
        public static Percent Fifty = new Percent(PublicConstants.Half);
        public static Percent Full = new Percent(MaxPercentage);

        public static Percent Zero = new Percent(0);
        public static Percent Random
        {
            get { return new Percent((float)Ref.rnd.Double()); }
        }
        public byte ByteVal
        {
            get { return (byte)(Value * Byte.MaxValue); }
            set { Value = (float)value / Byte.MaxValue; }
        }
    }
}
