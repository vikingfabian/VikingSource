using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class QuestSetup
    {
        public QuestName quest;
        public string customName;
        public DefaultLevelConditions conditions;
        public float playerCountDifficulty;
        public int setupForPlayerCount;

        public QuestSetup(QuestName quest)
        {
            toggRef.NewSeed();
            this.quest = quest;
            customName = null;
        }

        public void RefreshConditions()
        {
            switch (quest)
            {
                case QuestName.TutorialPractice:
                    conditions = new Tutorial1Conditions();
                    break;
                case QuestName.StoryGoblinFoodSteal:
                    conditions = new LevelConditions.GoblinFoodSteal();
                    break;
                case QuestName.StoryGoblinFoodBoss:
                    conditions = new LevelConditions.GoblinFoodBoss();
                    break;
                case QuestName.StoryGoblinCounter:
                    conditions = new LevelConditions.GoblinCounter();
                    break;
                case QuestName.StoryGoblinCastle:
                    conditions = new LevelConditions.GoblinCastle();
                    break;
                case QuestName.CyclopsBoss:
                    conditions = new LevelConditions.CyclopsConditions();
                    break;
                case QuestName.NagaBoss:
                    conditions = new NagaBossConditions();
                    break;
                default:
                    conditions = new DefaultLevelConditions();
                    break;
            }
        }

        public void onStart()
        {
            int playerCount = Ref.netSession.RemoteGamersCount + 1;
            setupForPlayerCount = playerCount;

            switch (playerCount)
            {
                default://1, 2, 3 
                    playerCountDifficulty = 0.75f;
                    setupForPlayerCount = 3;
                    break;
                case 4:
                    playerCountDifficulty = 1f;
                    break;
                case 5:
                    playerCountDifficulty = 1.25f;
                    break;
            }
        }

        public void netWrite(System.IO.BinaryWriter w)
        {
            //w.Write((ushort)seed);
            toggRef.WriteSeed(w);
            w.Write((byte)quest);
        }

        public void netRead(System.IO.BinaryReader r)
        {
            //seed = r.ReadUInt16();
            toggRef.ReadSeed(r);
            quest = (QuestName)r.ReadByte();
            
            if (quest == QuestName.Custom)
            {
                customName = SaveLib.ReadString(r);
            }
        }

        public override string ToString()
        {
            if (quest == QuestName.Custom)
            {
                return "Custom: " + customName;
            }
            else
            {
                return hqLib.QuestTitle(quest);//quest.ToString();
            }
        }
    }
}
