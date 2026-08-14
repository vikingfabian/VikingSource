using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.LootFest.Data;

namespace VikingEngine.DSSWars.Data
{
    struct TimeLength
    {
        public float seconds;
        public static readonly TimeLength Zero = new TimeLength(0);
        // Constructor to initialize milliseconds
        public TimeLength(float seconds)
        {
            this.seconds = seconds;
        }

        public float Minutes
        {
            get { return seconds / TimeExt.MinuteInSeconds; }
            set { seconds = TimeExt.MinuteInSeconds * value; }
        }

        // Override ToString method to display seconds and milliseconds
        public string LongString()
        {
            if (seconds < TimeExt.MinuteInSeconds)
            {
                return ((int)seconds).ToString() + " seconds";
            }
            else
            {
                // Calculate minutes and remaining seconds
                int minutes = (int)seconds / TimeExt.MinuteInSeconds;
                float remainingSeconds = (int)Math.Ceiling( seconds % TimeExt.MinuteInSeconds);
                return $"{minutes} minutes " + remainingSeconds + " seconds";
            }
        }

        public string ShortString()
        {
            // Calculate minutes and remaining seconds
            int minutes = (int)seconds / TimeExt.MinuteInSeconds;
            int remainingSeconds = (int)(seconds % TimeExt.MinuteInSeconds);

            // Format the output as "MM:SS" with leading zeros
            return $"{minutes:D2}:{remainingSeconds:D2}";
        }

        public static TimeLength FromMinutes(float minutes)
        {
            return new TimeLength(minutes * TimeExt.MinuteInSeconds);
        }

        public static TimeLength FromHours(float hours)
        {
            return new TimeLength(hours * TimeExt.HourInSeconds);
        }

        public TimeSpan TimeSpan
        {
            get { return TimeSpan.FromSeconds(seconds); }
            set { seconds = (float)value.TotalSeconds; }
        }

        public override string ToString()
        {
            return $"Time length: {seconds} seconds";
        }

        public void write_ushort(System.IO.BinaryWriter w)
        {
           w.Write(Bound.UShort(seconds));
        }

        public void read_ushort(System.IO.BinaryReader r)
        {
            seconds = r.ReadUInt16();
        }
    }

    struct TimeInGameCountdown
    {
        public TimeLength length;
        public float endTimeSec;

        public TimeInGameCountdown(TimeLength length)
            : this()
        {
            this.start(length);
        }

        public void start(float lengthSec)
        {
            this.length = new TimeLength(lengthSec);
            start();//endTimeSec = Ref.TotalGameTimeSec + length.seconds;
        }

        public void start(TimeLength length)
        {
            this.length = length;
            start();//endTimeSec = Ref.TotalGameTimeSec + length.seconds;
        }

        public void start(IntervalF randomSecondsRange)
        {
            this.length = new TimeLength(randomSecondsRange.GetRandom());
            start();//endTimeSec = Ref.TotalGameTimeSec + length.seconds;
        }

        public void start()
        { 
            endTimeSec = Ref.TotalGameTimeSec + length.seconds;
        }

        public void zero()
        { 
            length = TimeLength.Zero;
            endTimeSec = 0;
        }
        public bool TimeOut()
        {
            if (endTimeSec > 0)
            {
                return Ref.TotalGameTimeSec >= endTimeSec;
            }
            else
            {
                start();
                return false;
            }
        }

        public TimeLength TimePassed()
        {
            return new TimeLength(length.seconds - endTimeSec + Ref.TotalGameTimeSec);
        }

        public TimeLength RemainingLength()
        {
            return new TimeLength(Bound.Min(endTimeSec - Ref.TotalGameTimeSec, 0));
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((ushort)Bound.Min(RemainingLength().seconds, 0));
        }

        public void readGameState(System.IO.BinaryReader r, bool bStart = false)
        {
            float remaining = r.ReadUInt16();
            if (remaining > 0)
            {
                length = new TimeLength(remaining);
            }

            if (bStart)
            {
                start();
            }
        }

        public override string ToString()
        {
            return $"Count down: {RemainingLength()}/{length.seconds} seconds";
        }
    }

    struct UseTimeLimit
    {
        public bool use;
        public TimeLength time;

        public UseTimeLimit(bool use, TimeLength time)
        { 
            this.use = use;
            this.time = time;
        }

        public void write_ushort(System.IO.BinaryWriter w, bool storeTime)
        {
            w.Write(use);
            if (use || storeTime)
            {
                time.write_ushort(w);
            }
        }

        public void read_ushort(System.IO.BinaryReader r, bool storeTime)
        {
            use = r.ReadBoolean();
            if (use || storeTime)
            {
                time.read_ushort(r);
            }
        }

        public bool UseProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                use = value;
            }
            return use;
        }

        public float MinuteProperty(object tag, bool set, float value)
        {
            if (set)
            {
                time.Minutes = value;
            }
            return time.Minutes;
        }
    }
}
