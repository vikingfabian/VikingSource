using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//

namespace VikingEngine.LF2.Data
{
    static class RandomSeed
    {
        public const int NumLoadedFiles  = 2;
        public const int SeedFileLength = 4080;
        public const int SeedLength = NumLoadedFiles * SeedFileLength;
        //static CirkleCounterUp currentSeed;
        static byte[] randomNumbersList;
        public static bool Empty { get { return randomNumbersList == null; } }
        public static RandomSeedInstance Instance;

        public static void NewWorld()
        {
            Instance = new RandomSeedInstance();
            CreateSeed();
            CreateRandomList();
        }

        static void CreateSeed()
        {
            if (Data.WorldsSummaryColl.CurrentWorld != null)
            {
                Data.WorldsSummaryColl.CurrentWorld.seedListIndex = new byte[NumLoadedFiles];
                //Load new seed lists and add a key value, to make the world unique
                const int NumLists = 64;


                Data.WorldsSummaryColl.CurrentWorld.key = (byte)Ref.rnd.Int(byte.MaxValue);

                for (int i = 0; i < NumLoadedFiles; i++)
                {
                    Data.WorldsSummaryColl.CurrentWorld.seedListIndex[i] = (byte)Ref.rnd.Int(NumLists);
                }

            }
        }

        public static void LoadWorld()
        {
            CreateRandomList();
        }
        static void CreateRandomList()
        {
            //new
            if (Data.WorldsSummaryColl.CurrentWorld != null && Data.WorldsSummaryColl.CurrentWorld.seedListIndex == null)
            {
                //corrupt file
                CreateSeed();
                Engine.XGuide.Message("Repaired corrupt save file", "The world may look a bit different", Engine.XGuide.LocalHostIndex);
            }

            List<byte> seed = new List<byte>();
            for (int i = 0; i < NumLoadedFiles; i++)
            {
                int seedIx = i;
                if (Data.WorldsSummaryColl.CurrentWorld != null)
                {
                    seedIx = Data.WorldsSummaryColl.CurrentWorld.seedListIndex[i];
                }
                seed.AddRange(DataLib.SaveLoad.LoadByteArray(Engine.LoadContent.Content.RootDirectory + TextLib.Dir + 
                    LootfestLib.DataFolder + "Seed\\" + GameState.SeedGenerator2.Path(seedIx), false));
            }
            randomNumbersList = seed.ToArray();
#if WINDOWS
            if (randomNumbersList.Length != SeedLength)
                throw new Exception(); 
#endif
           
        }

        public static void ShareWorldIx()
        {
            //this packet can only be recieved by player in MainMenu state, others wont read it
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_WorldIndex, Network.PacketReliability.Reliable, LootfestLib.LocalHostIx);
            Data.WorldsSummaryColl.CurrentWorld.WriteNetShare(w);
        }
        public static void RecieveWorldIx(System.IO.BinaryReader reader)
        {
            Data.WorldsSummaryColl.CurrentWorld = new WorldSummary();
            Data.WorldsSummaryColl.CurrentWorld.ReadNetShare(reader, byte.MaxValue);
            CreateRandomList();
        }

        
        static byte addKey(byte val)
        {
            int result = val + Data.WorldsSummaryColl.CurrentWorld.key;
            if (result > byte.MaxValue)
            { result -= byte.MaxValue; }
            return (byte)result;
        }

        public static byte GetSeed(int index)
        {
            return randomNumbersList[index];
        }
        
    }
    struct RandomSeedInstance
    {
        static CirkleCounterUp currentSeed = new CirkleCounterUp(RandomSeed.SeedLength - 1);
        public int Next(Range range)
        {
            return range.Min + Next(range.Difference + 1);
        }
        public float Next(IntervalF range)
        {
            return range.Min + Next(range.Difference);
        }
        public bool Bool()
        {
            const byte HalfByte = 128;
            return NextByte() < HalfByte;
        }
        public bool BytePercent(byte chance)
        {
            return NextByte() < chance;
        }
        public byte NextByte()
        {
            return RandomSeed.GetSeed(currentSeed.Next());

        }
        public float Next(float num)
        {
            const int MaxValue = (byte.MaxValue << 8) + byte.MaxValue;

            byte a = NextByte();
            byte b = NextByte();
            float sum = (a << 8) + b;

            float result = num * (sum / MaxValue);

            return result;
        }
        public int Next(int num)
        {
            int result = (int)Next((float)num);

            if (result >= num)
            {
                result = num - 1;
            }
            return result;
        }

        public Range GetRandomRange(Range min, Range add)
        {
            int minVal = Next(min);
            return new Range(minVal, minVal + Next(add));
        }
        public void SetSeedPosition(int position)
        {
            position %= RandomSeed.SeedLength;
            currentSeed.Set(position);
        }
        public int GetSeedPosition { get { return currentSeed.Value; } }

        public void SetSeedPosition(IntVector2 position)
        {
            int pos = position.X * 3 + position.Y * 11;
            pos %= RandomSeed.SeedLength;
            if (PlatformSettings.Debug <= BuildDebugLevel.TesterDebug_2)
            {
                if (pos < 0 || pos >= RandomSeed.SeedLength)
                {
                    throw new Exception();
                }
            }
            currentSeed.Set(pos);
        }

        ///// <summary>
        ///// Returns and removes a random position in a List
        ///// </summary>
        //public object RandomListMemeberRemove(System.Collections.IList list)
        //{
        //    int rndIx = Next(list.Count); //next is picking a random pos
        //    object result = list[rndIx];
        //    list.RemoveAt(rndIx);
        //    return result;
        //}

        public T RandomListMemeber<T>(List<T> list)
        {
            int rndIx = Next(list.Count);
            return list[rndIx];
        }
        public T RandomListMemeber<T>(T[] array)
        {
            int rndIx = Next(array.Length);
            return array[rndIx];
        }


        /// <summary>
        /// Removes a random object from the list and return it 
        /// </summary>
        public T RandomListMemeberRemove<T>(List<T> list)
        {
            int ix = Next(list.Count);
            T result = list[ix];
            list.RemoveAt(ix);
            return result;
        }
    }
}
