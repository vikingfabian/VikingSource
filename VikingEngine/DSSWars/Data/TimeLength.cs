using System;
using System.Collections.Generic;
using System.Linq;
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

        public override string ToString()
        {
            return $"Time length: {seconds} seconds";
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
            return Ref.TotalGameTimeSec >= endTimeSec;
        }

        public TimeLength RemainingLength()
        {
            return new TimeLength(Bound.Min(endTimeSec - Ref.TotalGameTimeSec, 0));
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((ushort)Bound.Min(RemainingLength().seconds, 0));
        }

        public void readGameState(System.IO.BinaryReader r)
        {
            float remaining = r.ReadUInt16();
            if (remaining > 0)
            {
                start(new TimeLength(remaining));
            }
        }

        public override string ToString()
        {
            return $"Count down: {RemainingLength()}/{length.seconds} seconds";
        }
    }
}
