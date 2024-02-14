using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DataStream;

namespace VikingEngine.LootFest.BlockMap
{
    class AreaDesignStorageCollection
    {
        public bool netRecieved = false;
        public List<DesignAreaStorage> areas = new List<DesignAreaStorage>();

        public void delayedNetWrite()
        {
            new Timer.TimedAction0ArgTrigger(netWrite, 400);
        }

        public void netWrite()
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.DesignAreaStorageHeader, Network.PacketReliability.Reliable);

            foreach (var m in areas)
            {
                m.writeEditedChunks(w);
            }
        }

        public void netRead(Network.ReceivedPacket packet)
        {
            foreach (var m in areas)
            {
                m.readEditedChunks(m.editedChunks.Size, packet.r);
            }
            netRecieved = true;
        }

        public DesignAreaStorage getArea(IntVector2 chunk, out IntVector2 localPos)
        {
            foreach (var m in areas)
            {
                if (m.chunkArea.IntersectTilePoint(chunk))
                {
                    localPos = chunk - m.chunkArea.pos;
                    return m;
                }
            }

            localPos = IntVector2.Zero;
            return null;
        }


        public int editCount()
        {
            int result = 0;
            foreach (var m in areas)
            {
                result += m.editCount();
            }
            return result;
        }
    }

    class DesignAreaStorage
    {
        const int Edited_BitIndex = 0;
        public const int NetRecieved_BitIndex = 1;

        public Rectangle2 chunkArea;
        AreaDesignType type;
        int areaIndex;
        public Grid2D<byte> editedChunks;
        public bool unsavedChanges = false;

        public DesignAreaStorage(AreaDesignType type, int areaIndex, Rectangle2 chunkArea)
        {
            this.type = type;
            this.areaIndex = areaIndex;
            this.chunkArea = chunkArea;
            editedChunks = new Grid2D<byte>(chunkArea.size);

            if (LfRef.WorldHost)
            {
                saveload(false);
            }
        }

        public bool inArea(IntVector2 chunk)
        {
            return chunkArea.IntersectTilePoint(chunk);
        }

        public DataStream.FilePath ChunkPath(IntVector2 chunk)
        {
            IntVector2 pos = chunk - chunkArea.pos;

            return new DataStream.FilePath(
                VikingEngine.LootFest.Players.PlayerStorageGroup.FileFolderName(LfRef.gamestate.LocalHostingPlayer.Storage.StorageGroupIx, LfRef.WorldHost),
                type.ToString() + areaIndex.ToString() + "C" + lib.IntVec2Text(pos),
                VikingEngine.LootFest.Map.Chunk.SaveFileEnding, true, VikingEngine.LootFest.Map.Chunk.NumBackupVersions, true);
        }

        public DataStream.FilePath HeaderPath()
        {
            return new DataStream.FilePath(
                VikingEngine.LootFest.Players.PlayerStorageGroup.FileFolderName(LfRef.gamestate.LocalHostingPlayer.Storage.StorageGroupIx, LfRef.WorldHost),
                type.ToString() + areaIndex.ToString(), ".hed", true, VikingEngine.LootFest.Map.Chunk.NumBackupVersions, true);
        }


        public void onSavingChunk(IntVector2 chunk)
        {
            chunk -= chunkArea.pos;
            Set(chunk, true, Edited_BitIndex);
            unsavedChanges = true;
        }

        public void SaveHeader()
        {
            if (unsavedChanges)
            {
                unsavedChanges = false;

                saveload(true);
            }
        }

        void saveload(bool save)
        {
            DataStream.BeginReadWrite.BinaryIO(save, HeaderPath(), writeHead, readHead, null, true);
        }

        byte Version = 0;
        public void writeHead(System.IO.BinaryWriter w)
        {
            w.Write(Version);
            editedChunks.Size.writeByte(w);

            writeEditedChunks(w);
        }

        public void writeEditedChunks(System.IO.BinaryWriter w)
        {
            BoolStreamer boolstream = new BoolStreamer();

            ForXYLoop loop = new ForXYLoop(editedChunks.Size);
            while (loop.Next())
            {
                boolstream.writeNext(editedChunks.Get(loop.Position) != 0, w);
            }
            boolstream.endWrite(w);
        }

        public void readHead(System.IO.BinaryReader r)
        {
            int version = r.ReadByte();
            IntVector2 sz = IntVector2.FromReadByte(r);

            readEditedChunks(sz, r);
        }

        public void readEditedChunks(IntVector2 sz, System.IO.BinaryReader r)
        {
            BoolStreamer boolstream = new BoolStreamer();

            ForXYLoop loop = new ForXYLoop(sz);
            while (loop.Next())
            {
                bool value = boolstream.readNext(r);

                if (editedChunks.InBounds(loop.Position))
                {
                    if (value)
                    {
                        lib.DoNothing();
                    }
                    Set(loop.Position, value, Edited_BitIndex);
                }
            }
        }

        public void Set(IntVector2 localPos, bool value, int bitIndex)
        {
            editedChunks.array[localPos.X, localPos.Y] = EightBit.SetBit(editedChunks.array[localPos.X, localPos.Y], bitIndex, value);
        }

        public void Get(IntVector2 localPos, out bool edited, out bool netRecieved)
        {
            if (localPos == new IntVector2(3, 4))
            {
                lib.DoNothing();
            }
            byte byteVal = editedChunks.array[localPos.X, localPos.Y];
            edited = EightBit.GetBit(byteVal, Edited_BitIndex);
            netRecieved = EightBit.GetBit(byteVal, NetRecieved_BitIndex);
        }

        public int editCount()
        {
            int result = 0;

            ForXYLoop loop = new ForXYLoop(editedChunks.Size);
            while (loop.Next())
            {
                if (editedChunks.Get(loop.Position) != 0)
                {
                    ++result;
                }
            }

            return result;
        }
    }

    enum AreaDesignType
    {
        PublicBuild,
    }
}
