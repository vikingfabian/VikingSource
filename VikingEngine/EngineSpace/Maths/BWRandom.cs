using System;

//Craig Martin

namespace VikingEngine.EngineSpace.Maths
{
    

    public struct BWRandomOld
    {
        bool useLock;
        Random random;

        public BWRandomOld(bool useLock)
        {
            this.useLock = useLock;
            random = new Random();
        }

        public BWRandomOld(int seed, bool useLock)
        {
            this.useLock = useLock;
            random = new Random(seed);
        }

        public void ReSeed(int seed, bool useLock)
        {
            this.useLock = useLock;
            random = new Random(seed);
        }

        public int Next()
        {
            if (useLock)
                lock (random) return random.Next();
            else
                return random.Next();
        }

        public int Next(int maxValue)
        {
            if (useLock)
                lock (random) return random.Next(maxValue);
            else
                return random.Next(maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            if (useLock)
                lock (random) return random.Next(minValue, maxValue);
            else
                return random.Next(minValue, maxValue);
        }

        public void NextBytes(byte[] buffer)
        {
            if (useLock)
                lock (random) random.NextBytes(buffer);
            else
                random.NextBytes(buffer);
        }

        public double NextDouble()
        {
            if (useLock)
                lock (random) return random.NextDouble();
            else
                return random.NextDouble();
        }

        public bool RandomChance(double chance)
        {
            return NextDouble() <= chance;
        }

        public bool RandomChanceTime(double seconds)
        {
            return Next((int)(seconds * 60)) == 0;
        }

        #region Static Allocate / Initialize

        //public static BWRandom Initialize(int handle)
        //{
        //    RandomPool.List[handle] = new BWRandom();
        //    //RandomPool.List[handle].Reinitialize();
        //    return RandomPool.List[handle];
        //}

        //public static BWRandom Initialize(int handle, int seed)
        //{
        //    RandomPool.List[handle] = new BWRandom(seed);
        //    //RandomPool.List[handle].Reinitialize(seed);
        //    return RandomPool.List[handle];
        //}

        //public static int GetRandom()
        //{
        //    int handle = RandomPool.GetNext();
        //    RandomPool.List[handle] = new BWRandom();
        //    //RandomPool.List[handle].Reinitialize();
        //    return handle;
        //}

        //public static int GetRandom(int seed)
        //{
        //    int handle = RandomPool.GetNext();
        //    RandomPool.List[handle] = new BWRandom(seed);
        //    //RandomPool.List[handle].Reinitialize(seed);
        //    return handle;
        //}

        //public static void Release(int handle)
        //{
        //    RandomPool.Release(handle);
        //}

        #endregion
    }

    public class BWRandom
    {
        const int MBIG = Int32.MaxValue;
        const int MSEED = 161803398;
        const int MZ = 0;
        int inext, inextp;
        int[] SeedArray = new int[56];
        bool useLock;
        object thislock;

        public BWRandom(bool useLock)
            : this(Environment.TickCount, useLock)
        {
        }

        public BWRandom(int seed, bool useLock)
        {
            Reseed(seed, useLock);
        }

        public void Reseed(int seed, bool useLock)
        {
            this.useLock = useLock;
            if (useLock && thislock == null) thislock = new object();
            else if (!useLock && thislock != null) thislock = null;

            if (useLock)
            {
                lock (thislock)
                {
                    ReseedCore(seed);
                }
            }
            else
            {
                ReseedCore(seed);
            }
        }

        void ReseedCore(int seed)
        {
            int ii;
            int mj, mk;

            //Initialize our Seed array.
            //This algorithm comes from Numerical Recipes in C (2nd Ed.)
            mj = MSEED - Math.Abs(seed);
            SeedArray[55] = mj;
            mk = 1;
            for (int i = 1; i < 55; i++)
            { 
                //Apparently the range [1..55] is special (Knuth) and so we're wasting the 0'th position.
                ii = (21 * i) % 55;
                SeedArray[ii] = mk;
                mk = mj - mk;
                if (mk < 0) mk += MBIG;
                mj = SeedArray[ii];
            }
            for (int k = 1; k < 5; k++)
            {
                for (int i = 1; i < 56; i++)
                {
                    SeedArray[i] -= SeedArray[1 + (i + 30) % 55];
                    if (SeedArray[i] < 0) SeedArray[i] += MBIG;
                }
            }
            inext = 0;
            inextp = 21;
        }

        protected double Sample()
        {
            if (useLock)
            {
                lock (thislock)
                {
                    return SampleCore();
                }
            }
            else
            {
                return SampleCore();
            }
        }

        protected virtual double SampleCore()
        {
            int retVal;
            int locINext = inext;
            int locINextp = inextp;

            if (++locINext >= 56) locINext = 1;
            if (++locINextp >= 56) locINextp = 1;

            retVal = SeedArray[locINext] - SeedArray[locINextp];

            if (retVal < 0) retVal += MBIG;

            SeedArray[locINext] = retVal;

            inext = locINext;
            inextp = locINextp;

            //Including this division at the end gives us significantly improved
            //random number distribution.
            return (retVal * (1.0 / MBIG));
        }

        public virtual int Next()
        {
            return (int)(Sample() * Int32.MaxValue);
        }

        public virtual int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException("minValue", String.Format("Argument_MinMaxValue", "minValue", "maxValue"));
            }

            int range = (maxValue - minValue);

            //This is the case where we flipped around (e.g. MaxValue-MinValue);
            if (range < 0)
            {
                long longRange = (long)maxValue - (long)minValue;
                return (int)(((long)(Sample() * ((double)longRange))) + minValue);
            }

            return ((int)(Sample() * (range))) + minValue;
        }

        public virtual int Next(int maxValue)
        {
            if (maxValue < 0)
            {
                throw new ArgumentOutOfRangeException("maxValue", String.Format("ArgumentOutOfRange_MustBePositive", "maxValue"));
            }

            return (int)(Sample() * maxValue);
        }

        public virtual double NextDouble()
        {
            return Sample();
        }

        public virtual void NextBytes(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)(Sample() * (Byte.MaxValue + 1));
            }
        }

        public bool RandomChance(double chance)
        {
            return Sample() <= chance;
        }

        public bool RandomChanceTime(double seconds)
        {
            return (int)(Sample() * (seconds * 60)) == 0;
        }
    }
}
