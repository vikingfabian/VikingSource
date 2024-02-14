using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Players
{
    class PlayerStorageGroup
    {
        public const int FileSaveVersion = 22;
        public const int FilesCount = 6;
        public PlayerStorage[] storages;
        DataStream.FilePath filePath;
        public int[] playerIndexPreviousSaveFile;
        int earlyAdopterGeneration = 2;

        public PlayerStorageGroup()
        {
            storages = new PlayerStorage[FilesCount];
            resetSelectStorageLock();
            LfRef.storage = this;
            filePath = new DataStream.FilePath(null, "PlayerSaveFiles", ".sav", true, true);
           

            if (DataStream.DataStreamHandler.FileExists(filePath))
            {
                DataStream.BeginReadWrite.BinaryIO(false, filePath, WriteStream, TryReadStream,
                    null, false);
            }
        }

        public void clearRAM()
        {
            foreach (var s in storages)
            {
                if (s != null) s.clearRAM();
            }
        }

        public void resetSelectStorageLock()
        {
            playerIndexPreviousSaveFile = new int[4] { -1, -1, -1, -1 };
            
            foreach (var s in storages)
            {
                if (s != null) s.player = null;
            }
        }

        public static string FileFolderName(int fileIx, bool worldHost)
        {
            if (worldHost)
            {
                return "File" + TextLib.IndexToString(fileIx);
            }
            else
            {
                return "File Net Guest";
            }
        }

        public void Save()
        {
            DataStream.BeginReadWrite.BinaryIO(true, filePath, WriteStream, TryReadStream,
                null, true);
        }

        public void AssignToPlayer(int index, Player player)
        {
            if (LfRef.storage.storages[index] == null)
            {
                LfRef.storage.storages[index] = new Players.PlayerStorage();
            }
            LfRef.storage.storages[index].AssignToPlayer(player);
            player.Storage.StorageGroupIx = index;

            int playerIndex = (int)player.PlayerIndex;
            playerIndexPreviousSaveFile[playerIndex] = index;
        }

        public void WriteStream(System.IO.BinaryWriter w)
        {
            w.Write(FileSaveVersion);

            w.Write(earlyAdopterGeneration);

            for (int i = 0; i < playerIndexPreviousSaveFile.Length; ++i)
            {
                w.Write(playerIndexPreviousSaveFile[i]);
            }

            for (int i = 0; i < storages.Length; ++i)
            {
                if (storages[i] == null)
                {
                    w.Write(false);
                }
                else
                {
                    w.Write(true);
                    storages[i].WriteStream(w);
                }
            }
        }
        public void TryReadStream(System.IO.BinaryReader r)
        {
            if (PlatformSettings.DevBuild)
            {
                ReadStream(r);
            }
            else
            {
                try
                {
                    ReadStream(r);
                }
                catch (Exception e)
                {
                    Debug.LogError("Load player storages, " + e.Message);
                }
            }
        }

        public void ReadStream(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();

            if (version < 11)
            {
                earlyAdopterGeneration = 1;
            }
            else
            {
                earlyAdopterGeneration = r.ReadInt32();
            }

            if (version < 22)
                return;

            if (version > FileSaveVersion)
            {
                Debug.LogError("Save file is from a newer version: " + version.ToString());
            }
            else
            {
                for (int i = 0; i < playerIndexPreviousSaveFile.Length; ++i)
                {
                    playerIndexPreviousSaveFile[i] = r.ReadInt32();
                }

                for (int i = 0; i < storages.Length; ++i)
                {
                    if (r.ReadBoolean())
                    {
                        storages[i] = new PlayerStorage(r, version);
                    }
                }
            }
        }

    }
}
