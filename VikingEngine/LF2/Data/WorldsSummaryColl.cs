using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Data
{
    static class WorldsSummaryColl
    {
        public static Data.WorldSummary CurrentWorld = null;//is playing now

        public static List<Data.WorldSummary> savedWorlds = null;
        const string FileName = WorldSummary.WorldFileName + "sSummaryColl";
        static readonly DataStream.FilePath Path = new DataStream.FilePath(null, FileName, ".sum");

        public static void BeginSave(List<Players.Player> activePlayers, Data.Progress progress)
        {
            CurrentWorld.UpdateData(activePlayers, progress);
            //new DataStream.WriteBinaryIO(Path, WriteStream, null); 
            BeginSave();
        }
        public static void BeginSave()
        {
            new DataStream.WriteBinaryIO(Path, WriteStream, null);
        }

        public static Data.WorldSummary ContinueWorld
        {
            get
            {
                for (int i = savedWorlds.Count - 1; i >= 0; i--)
                {
                    if (!savedWorlds[i].IsVisitedWorld)
                        return savedWorlds[i];
                }
                return null;
                //return savedWorlds[savedWorlds.Count - 1];
            }
        }

        public static void Load() //must be threaded
        {
#if WINDOWS
            Debug.DebugLib.CrashIfMainThread();
#endif
            if (DataStream.DataStreamHandler.FileExists(Path))
            {
                ReadStream(DataStream.DataStreamHandler.ReadBinaryIO(Path));
            }
            else
            {
                old_ThreadedLoading();
            }
        }

        public static void WriteStream(System.IO.BinaryWriter w)
        {
            w.Write((ushort)savedWorlds.Count);
            for (int i = 0; i < savedWorlds.Count; i++)
            {
                savedWorlds[i].WriteStream(w);
            }
        }
        public static void ReadStream(System.IO.BinaryReader r)
        {
            int numWorlds = r.ReadUInt16();
            savedWorlds = new List<WorldSummary>(numWorlds);
            for (int i = 0; i < numWorlds; i++)
            {
                WorldSummary sum = new WorldSummary();

                sum.ReadStream(r);
                savedWorlds.Add(sum);
            }
        }

        static void old_ThreadedLoading()
        {
            //load worlds
            savedWorlds = new List<Data.WorldSummary>();
            List<string> worldFolders = DataLib.SaveLoad.ListStorageFolders(Data.WorldSummary.WorldFileName + "*", true);

            for (int i = 0; i < worldFolders.Count; i++)
            {
                Data.WorldSummary world = new Data.WorldSummary();
                world.SaveIndex = lib.SafeStringToInt(worldFolders[i].Remove(0, Data.WorldSummary.WorldFileName.Length));
                world.Save(false, false);
                savedWorlds.Add(world);
            }
        }

        public static void PlayInWorld(Data.WorldSummary sum)
        {
            savedWorlds.Remove(sum);
            savedWorlds.Add(sum);
            CurrentWorld = sum;
            CurrentWorld.StartPlayingThisWorld();
        }

        public static Data.WorldSummary CreateNewWorld(int saveIndex)
        {
            CurrentWorld = new Data.WorldSummary();
            CurrentWorld.SaveIndex = saveIndex;
            Data.RandomSeed.NewWorld();
            CurrentWorld.CreateFolder();

            PlayInWorld(CurrentWorld);
            return CurrentWorld;
        }

        static Data.WorldSummary removingWorld = null;
        public static void SlowWorldDeleteCheck()
        {
            if (Engine.Storage.Empty && savedWorlds != null && savedWorlds.Count > 0)
            {
                //pick random world and check if it needs to be deleted
                removingWorld = arraylib.RandomListMemeber(savedWorlds);
                if (removingWorld.Removed)
                {
                    Engine.Storage.AddToSaveQue(RemoveWorldEvent, true);
                }
            }
        }

        public static void RemoveWorldEvent(bool saveque)
        {
            if (DataStream.DataStreamHandler.StorageFolderExists(removingWorld.FolderPath))
            {
                if (DataStream.DataStreamHandler.StorageFolderIsEmpty(removingWorld.FolderPath))
                {
                    DataStream.DataStreamHandler.RemoveEmptyFolder(removingWorld.FolderPath);
                    savedWorlds.Remove(removingWorld);
                }
                else
                {
                    if (DataStream.DataStreamHandler.RemoveFolder(removingWorld.FolderPath, true, 3))
                        savedWorlds.Remove(removingWorld);
                }
            }
            else
            {
                savedWorlds.Remove(removingWorld);
            }
        }

        public static bool HasVisitorWorldName(string suggestedName)
        {
            foreach (WorldSummary w in savedWorlds)
            {
                if (w.IsVisitedWorld && w.WorldName == suggestedName)
                    return true;
            }
            return false;
        }
    }
}
