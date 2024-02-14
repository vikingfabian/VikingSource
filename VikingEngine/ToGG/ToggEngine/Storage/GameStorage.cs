using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.LevelSetup;
using VikingEngine.ToGG.Data;

namespace VikingEngine.ToGG.Storage
{
    class GameStorage 
    {
        public bool[] challengeCompleted = new bool[(int)LevelEnum.NUM];
        public int wonQuickPlay = 0, lostQuickPlay = 0;

        public PlayerStoryProgress activeStoryProgress = null;
        public StoryStorage activeStoryStorage = null;

        public GameStorage()
        {
            toggRef.storage = this;
            saveLoad(false);
        }

        public void onChallengeComplete(LevelEnum lvl)
        {
            challengeCompleted[(int)lvl] = true;
            saveLoad(true);
        }

        public void saveLoad(bool save)
        {
            var filePath = new DataStream.FilePath(null, "CommanderSave", ".sav", true, true);
            if (!save && !filePath.Exists())
            {
                return;
            }

            DataStream.BeginReadWrite.BinaryIO(save, filePath, write, read, null, save);
        }

       

        int Version = 1;
        void write(System.IO.BinaryWriter w)
        {
            w.Write(Version);

            w.Write((byte)challengeCompleted.Length);
            for (int i = 0; i < challengeCompleted.Length; ++i)
            {
                w.Write(challengeCompleted[i]);
            }

            w.Write(wonQuickPlay);
            w.Write(lostQuickPlay);

        }
        void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();

            int challengeCompletedLength = r.ReadByte();
            for (int i = 0; i < challengeCompletedLength; ++i)
            {
                challengeCompleted[i] = r.ReadBoolean();
            }

            if (version >= 1)
            {
                wonQuickPlay = r.ReadInt32();
                lostQuickPlay = r.ReadInt32();
            }
            
        }

        public void debugUnlockAll()
        {
            for (int i = 0; i < challengeCompleted.Length; ++i)
            {
                challengeCompleted[i] = true;
            }

            //saveLoad(true);
        }
        public void debugResetProgress()
        {
            for (int i = 0; i < challengeCompleted.Length; ++i)
            {
                challengeCompleted[i] = false;
            }

            wonQuickPlay = 0; lostQuickPlay = 0;
            //saveLoad(true);
        }

        public bool completedTutorial()
        {
            foreach (var m in ChallengeLevelsData.TutorialLevelsOrder)
            {
                if (challengeCompleted[(int)m] == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
