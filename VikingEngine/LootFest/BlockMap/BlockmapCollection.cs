using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VikingEngine.DataStream;

namespace VikingEngine.LootFest.BlockMap
{
    class BlockmapCollection
    {
        public const byte SaveFileVersion = 1;

        public const string BlockMapFolder = "Blockmap";
        const string SegmentCollection = "SegmentCollection";
        public List<SegmentHeader> storedSegments = new List<SegmentHeader>();

        public ushort getNextId()
        {
            int highestId = 1;

            foreach (var m in storedSegments)
            {
                if (m.id >= highestId)
                {
                    highestId = m.id + 1;
                }
            }

            return (ushort)highestId;
        }

        public void add(SegmentHeader segment)
        {
            for (int i = 0; i < storedSegments.Count; ++i)
            {
                if (storedSegments[i].id == segment.id)
                {
                    storedSegments.RemoveAt(i);
                    break;
                }
            }

            storedSegments.Add(segment);
        }

        public static FilePath BlockMapFilesDir()
        {
            bool loadFromStorage = PlatformSettings.DevBuild;

            FilePath filePath = new FilePath(
                loadFromStorage ?
                    BlockMapFolder :
                    LfLib.ContentFolder + BlockMapFolder,
                null,
                null, 
                loadFromStorage, 
                false);

            return filePath;
        }

        public void SaveLoad(bool save, bool threaded)
        {
            FilePath filePath = BlockMapFilesDir();

            filePath.FileName = SegmentCollection;
            filePath.FileEnd = ".sav";

            if (!save && PlatformSettings.DevBuild)
            {
                if (!filePath.Exists())
                {
                    return;
                }
            }
            
            DataStream.BeginReadWrite.BinaryIO(save, filePath, write, read, null, threaded);
        }

        List<SegmentHeader> matches = new List<SegmentHeader>();
        public BlockMapSegment loadRandomMatchingSegment(SegmentHeader profile, PcgRandom rnd)
        {
            matches.Clear();
            foreach (var m in storedSegments)
            {
                if (m.isMatch(profile))
                {
                    matches.Add(m);
                }
            }

            if (matches.Count == 0)
            {
                throw new Exception("loadRandomMatchingSegment " + profile.ToString());
            }

            SegmentHeader header;
            
            if (matches.Count == 1)
            {
                header = matches[0];
            }
            else
            {
                header = arraylib.RandomListMember(matches, rnd);
            }

            BlockMapSegment result = new BlockMapSegment(header);

            return result;
        }

        public BlockMapSegment loadSegment(ushort id)
        {
            foreach (var m in storedSegments)
            {
                if (m.id == id)
                {
                   return new BlockMapSegment(m);
                }
            }
            throw new Exception("Segment id could not be found: " + id.ToString());
        }

        void write(BinaryWriter w)
        {
            w.Write(SaveFileVersion);

            w.Write(storedSegments.Count);
            foreach (var m in storedSegments)
            {
                m.write(w);
            }
        }
        void read(BinaryReader r)
        {
            byte version = r.ReadByte();

            int storedSegmentsCount = r.ReadInt32();
            for (int i = 0; i < storedSegmentsCount; ++i)
            {
                storedSegments.Add(new SegmentHeader(r, version));
            }
        }
    }
}
