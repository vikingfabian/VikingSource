using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VikingEngine.DataStream;

namespace VikingEngine.LootFest.BlockMap
{
    struct SegmentHeader
    {
        public ushort id;
        public byte northWays, eastWays, southWays, westWays;
        public SegmentHeadType type;

        public SegmentHeader(SegmentHeadType type, byte northWays, byte eastWays, byte southWays, byte westWays)
        {
            id = 0;
            this.northWays = northWays;
            this.eastWays = eastWays;
            this.southWays = southWays;
            this.westWays = westWays;
            this.type = type;
        }

        public SegmentHeader(BinaryReader r, int version)
            : this()
        {
            read(r, version);
        }

        public bool isMatch(SegmentHeader profile)
        {
            return this.type == profile.type &&
                this.northWays == profile.northWays &&
                this.eastWays == profile.eastWays &&
                this.southWays == profile.southWays &&
                this.westWays == profile.westWays;
        }

        public void write(BinaryWriter w)
        {
            w.Write(id);
            w.Write((byte)type);
            w.Write(northWays);
            w.Write(eastWays);
            w.Write(southWays);
            w.Write(westWays);
        }
        public void read(BinaryReader r, int version)
        {
            id = r.ReadUInt16();
            type = (SegmentHeadType)r.ReadByte();
            northWays = r.ReadByte();
            eastWays = r.ReadByte();
            southWays = r.ReadByte();
            westWays = r.ReadByte();
        }

        public override string ToString()
        {
            return type.ToString() + id.ToString() + 
                "(n" + northWays.ToString() + 
                "e" + eastWays.ToString() + 
                "s" + southWays.ToString() + 
                "w" + westWays.ToString() +
                ")";
        }
    }

    class BlockMapSegment
    {
        public SegmentHeader header;
        public IntVector2 chunkSize = new IntVector2(4, 4);
        public Grid2D<BlockMapSquare> squares;

        public BlockMapSegment()
        {
            init(LfRef.blockmaps.getNextId());
        }

        public BlockMapSegment(ushort useId)
        {
            init(useId);
        }

        void init(ushort useId)
        {
            header.id = useId;
            header.type = SegmentHeadType.None;
            createGrid();
        }

        public BlockMapSegment(SegmentHeader header, IStreamIOCallback callback)
        {
            this.header = header;
            SaveLoad(false, true, callback);
        }

        public BlockMapSegment(SegmentHeader header)
        {
            this.header = header;

            if (PlatformSettings.DevBuild)
            {
                Debug.Log("Load Segment: " + header.ToString());
            }
            SaveLoad(false, false, null);
        }

        public void Rotate(bool clockWise)
        {
            chunkSize = new IntVector2(chunkSize.Y, chunkSize.X);
            
            int addRotation = clockWise ? 1 : 3;
            squares = squares.Rotate(addRotation);

            squares.LoopBegin();
            while (squares.LoopNext())
            {
                var sq = squares.LoopValueGet();
                sq.specialDir += (byte)addRotation;
                if (sq.specialDir >= 4)
                {
                    sq.specialDir -= (byte)4;
                }

                squares.LoopValueSet(sq);
            }
        }

        public void flip(bool xAxis)
        {
            squares = squares.Flip(xAxis);

            squares.LoopBegin();
            while (squares.LoopNext())
            {
                var sq = squares.LoopValueGet();

                if (xAxis)
                {
                    if (sq.Dir4 == Dir4.W)
                    {
                        sq.Dir4 = Dir4.E;
                    }
                    else if (sq.Dir4 == Dir4.E)
                    {
                        sq.Dir4 = Dir4.W;
                    }
                }
                else
                {
                    if (sq.Dir4 == Dir4.N)
                    {
                        sq.Dir4 = Dir4.S;
                    }
                    else if (sq.Dir4 == Dir4.S)
                    {
                        sq.Dir4 = Dir4.N;
                    }
                }

                squares.LoopValueSet(sq);
            }
        }

        public void Resize(IntVector2 newChunkSize)
        {
            var oldGrid = squares;

            chunkSize = newChunkSize;
            createGrid();

            IntVector2 copySize = new IntVector2(
                lib.SmallestValue(oldGrid.Size.X, squares.Size.X), 
                lib.SmallestValue(oldGrid.Size.Y, squares.Size.Y));
            ForXYLoop loop = new ForXYLoop(copySize);
            while (loop.Next())
            {
                squares.Set(loop.Position, oldGrid.Get(loop.Position));
            }
        }

        void createGrid()
        {
            squares = new Grid2D<BlockMapSquare>(chunkSize * BlockMapLib.SquaresPerChunkW);
        }

        public void SaveLoad(bool save, bool threaded, IStreamIOCallback callback)
        {
            if (save)
            {
                updateHeader();
            }

            //FilePath filePath = new FilePath(BlockmapCollection.BlockMapFolder, header.id.ToString(),
            //    ".seg", true, false);

            FilePath filePath = BlockmapCollection.BlockMapFilesDir();

            filePath.FileName = header.id.ToString();
            filePath.FileEnd = ".seg";

            if (save)
            {
                Directory.CreateDirectory(filePath.CompleteDirectory);
            }
            filePath.Storage = PlatformSettings.DevBuild;
            DataStream.BeginReadWrite.BinaryIO(save, filePath, write, read, callback, threaded);
        }

        public void updateHeader()
        {
            if (header.id == 0)
            {
                header.id = LfRef.blockmaps.getNextId();
            }
            header.northWays = 0; header.eastWays = 0; header.southWays = 0; header.westWays = 0;

            squares.LoopBegin();
            while (squares.LoopNext())
            {
                if (squares.LoopValueGet().special == MapBlockSpecialType.Entrance)
                {
                    switch (squares.LoopValueGet().specialDir)
                    {
                        case 0: header.northWays++; break;
                        case 1: header.eastWays++; break;
                        case 2: header.southWays++; break;
                        case 3: header.westWays++; break;
                    }
                }
                
            }

            LfRef.blockmaps.add(header);
        }

        public IntVector2 getEntranceSqPos(Dir4 fromEntrance, int fromEntranceIx)
        {
            byte dir = (byte)fromEntrance;
             squares.LoopBegin();
             while (squares.LoopNext())
             {
                 var sq = squares.LoopValueGet();
                 if (sq.special == MapBlockSpecialType.Entrance &&
                     sq.specialDir == dir &&
                     sq.specialIndex == fromEntranceIx)
                 {
                     return squares.LoopPosition;
                 }
             }

             return IntVector2.NegativeOne;
        }

        public void write(BinaryWriter w)
        {
            w.Write(BlockmapCollection.SaveFileVersion);
            header.write(w);
            chunkSize.writeByte(w);

            squares.LoopBegin();
            while (squares.LoopNext())
            {
                squares.LoopValueGet().write(w);
            }
        }
        public void read(BinaryReader r)
        {
            byte version = r.ReadByte();
            header.read(r, version);
            chunkSize.readByte(r);
            createGrid();

            squares.LoopBegin();
            while (squares.LoopNext())
            {
                squares.LoopValueSet(new BlockMapSquare(r, version));
            }
        }
    }
}
