using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Data;

namespace VikingEngine.ToGG.Storage
{
    class StoryStorage : VikingEngine.DataStream.IStreamIOCallback
    {
        string storyName;
        public Dictionary<LevelEnum, PlayerStoryProgress> progressPoints = new Dictionary<LevelEnum, PlayerStoryProgress>();

        public StoryStorage(string storyName)
        {
            this.storyName = storyName;
        }

        public void clear()
        {
            progressPoints.Clear();
        }

        public void StorePoint(LevelEnum nextLevel)
        {
            lib.DictionaryAddOrReplace(progressPoints, nextLevel, toggRef.storage.activeStoryProgress.clone());

            saveLoad(true);
        }

        public int currentLevel(LevelEnum[] Levels)
        {
            for (int i = Levels.Length - 1; i >= 0; --i)
            {
                if (progressPoints.ContainsKey(Levels[i]))
                {
                    return i;
                }
            }
            return 0;
        }

        DataStream.FilePath path()
        {
            return new DataStream.FilePath(null, "StoryProgress_" + storyName, ".sav", true, true);
        }

        public void saveLoad(bool save)
        {
            var filepath = path();
            if (!save && !filepath.Exists())
            {
                return;
            }

            DataStream.BeginReadWrite.BinaryIO(save, filepath, write, read, this, true);
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            if (!save)
            {
                if (Ref.gamestate is GameState.MainMenuState)
                {
                    ((GameState.MainMenuState)Ref.gamestate).onStoryLoaded();
                }
            }
        }

        public bool hasFile()
        {
            return path().Exists();
        }

        const int Version = 0;
        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Version);

            w.Write(progressPoints.Count);
            foreach (var kv in progressPoints)
            {
                w.Write((byte)kv.Key);
                kv.Value.write(w);
            }
        }
        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();

            int progressPointsCount = r.ReadInt32();
            for (int i = 0; i < progressPointsCount; ++i)
            {
                LevelEnum key = (LevelEnum)r.ReadByte();
                PlayerStoryProgress progress = new PlayerStoryProgress();
                progress.read(r, version);

                progressPoints.Add(key, progress);
            }
        }
    }
}
