using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//xna

namespace VikingEngine.LootFest.Data
{
    class WorldSeed
    {
        public Random rnd;
        public int seed;

        public WorldSeed()
        {
            if (DebugSett.WorldSeed == null)
            {
                seed = Ref.rnd.Int(10000);
            }
            else
            {
                seed = DebugSett.WorldSeed.Value;
            }
        }

        public void ShareWorldSeed()
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.WorldSeed, Network.PacketReliability.Reliable, LfLib.LocalHostIx);
            w.Write(seed);
            //Data.WorldsSummaryColl.CurrentWorld.WriteNetShare(w);
        }

        //public void RecieveWorldSeed(Network.ReceivedPacket packet)
        //{
        //    seed = packet.r.ReadInt32();
        //}

        public Random GetRandom_FromChunk(IntVector2 chunkGrindex)
        {
            rnd = new Random(chunkGrindex.X * 3 + chunkGrindex.Y * 11 + seed);
            return rnd;
        }
        public Random GetRandom_FromWorldPos(IntVector3 worldGrindex)
        {
            rnd = new Random(worldGrindex.X * 3 + worldGrindex.Y * 7 + worldGrindex.Z * 11 + seed);
            return rnd;
        }

    }
}
