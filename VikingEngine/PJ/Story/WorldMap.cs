using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DataStream;
using System.IO;

namespace VikingEngine.PJ.Story
{
    class WorldMap
    {
        public const string StoryFolder = "StoryLevels";
        const string LevelFileName = "storylvl";
        public const int SaveFileVersion = 0;

        SortedDictionary<IntVector2, Chunk> chunks = new SortedDictionary<IntVector2, Chunk>();
        public int level = 0;

        public WorldMap()
        {
            storyRef.map = this;
            //var c = addChunk(IntVector2.Zero);

            //c.generateModel();
        }

        Chunk addChunk(IntVector2 grindex)
        {
            var chunk = new Chunk(grindex);
            chunks.Add(chunk.mapGrindex, chunk);

            return chunk;
        }

        public void set(IntVector2 gridPos, int value)
        {
            getChunk_fromWP(gridPos, true).set(gridPos, value);
        }

        public int get(IntVector2 gridPos)
        {
            var c = getChunk_fromWP(gridPos, false);
            if (c == null)
            {
                return 0;
            }

            return c.get(gridPos);
        }

        Chunk getChunk_fromWP(IntVector2 worldGridPos, bool createIfMissing)
        {
            return getChunk_fromIx(toChunkGrindex(worldGridPos), createIfMissing);
        }

        Chunk getChunk_fromIx(IntVector2 cGrindex, bool createIfMissing)
        {
            Chunk result = null;

            if (chunks.TryGetValue(cGrindex, out result) == false &&
                createIfMissing)
            {
                return addChunk(cGrindex);
            }

            return result;
        }

        public IntVector2 toChunkGrindex(IntVector2 worldGridPos)
        {
            if (worldGridPos.X < 0)
            {
                worldGridPos.X -= Chunk.ChunkMaxPos;
            }
            if (worldGridPos.Y < 0)
            {
                worldGridPos.Y -= Chunk.ChunkMaxPos;
            }

            return worldGridPos / Chunk.Size;
        }

        public void refreshChunks(Rectangle2 area)
        {
            IntVector2 startC = toChunkGrindex(area.pos);
            IntVector2 endC = toChunkGrindex(area.BottomRightTile);

            ForXYLoop loop = new ForXYLoop(startC, endC);
            
            while(loop.Next())
            {
                var c = getChunk_fromIx(loop.Position, false);
                if (c != null)
                {
                    c.refresh();
                }
            }
        }

        public void generateChunksData()
        {
            foreach (var kv in chunks)
            {
                kv.Value.generateMeshData();
            }
        }

        public void endGenerateChunks()
        {
            foreach (var kv in chunks)
            {
                kv.Value.createModel();
            }
        }


        public void SaveLoad(bool save, bool threaded)
        {
            FilePath filePath = BlockMapFilesDir();

            filePath.FileName = LevelFileName + level.ToString();
            filePath.FileEnd = ".sav";

            if (save)
            {
                Directory.CreateDirectory(filePath.CompleteDirectory);
            }
            if (!save && PlatformSettings.DevBuild)
            {
                if (!filePath.Exists())
                {
                    return;
                }
            }

            DataStream.BeginReadWrite.BinaryIO(save, filePath, write, read, null, threaded);
        }

        void write(BinaryWriter w)
        {
            w.Write(SaveFileVersion);

            w.Write(chunks.Count);
            foreach (var kv in chunks)
            {
                kv.Value.mapGrindex.write(w);
                kv.Value.write(w);
            }
        }
        void read(BinaryReader r)
        {
            int version = r.ReadInt32();

            int chunksCount = r.ReadInt32();
            for (int i = 0; i < chunksCount; ++i)
            {
                var mapGrindex = IntVector2.FromRead(r);
                var c = addChunk(mapGrindex);
                c.read(r, version);
            }
         }

        public static FilePath BlockMapFilesDir()
        {
            bool loadFromStorage = PlatformSettings.DevBuild;

            FilePath filePath = new FilePath(
                loadFromStorage ?
                    StoryFolder :
                    PjLib.ContentFolder + StoryFolder,
                null,
                null,
                loadFromStorage,
                false);

            return filePath;
        }
    }

    

}
