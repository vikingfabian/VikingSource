using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.PlatformService;
using VikingEngine.ToGG.HeroQuest;

namespace VikingEngine.ToGG.ToggEngine.Data
{
    class Achievements
    {
        public Achievements()
        {
            toggRef.achievements = this;
        }

        public AchievementSettings Mission(QuestName quest, DoomChestLevel chestLevel)
        {
            string chest = "_ch" + TextLib.IndexToString((int)chestLevel);

            AchievementSettings achievement = new AchievementSettings(
                quest.ToString() + " " + chestLevel.ToString(),
                "mi_" + quest.ToString() + chest, null, FeatureStage.Tested_2);

            //Ex: mi_CyclopsBoss_ch1

            return achievement;
        }

        public GiftedAchievement GetGiftedAchievement(int id)
        {
            var list = giftedList();
            foreach (var m in list)
            {
                if (m.id == id)
                {
                    return m;
                }
            }

            throw new NotImplementedException();
        }

        public List<GiftedAchievement> giftedList()
        {
            return new List<GiftedAchievement>{
                new GiftedAchievement("Test Dummy", "Wictim of development crash testing", 1),
                new GiftedAchievement("Pet neglect", "Did you at least crack the window?", 2),

                new GiftedAchievement("Kill stealer", "Someone is waiting for that last hit", 3),
                new GiftedAchievement("Hiding rabbit", "Some people always run from danger", 4),
                new GiftedAchievement("Smiling sadist", "You made it impossible on purpose", 5),
                new GiftedAchievement("The scavanger", "Someone is picking up every bread crum", 6),
            };
        }
    }

    struct GiftedAchievement
    {
        public string name, desc;
        AchievementSettings settings;
        public int id;

        public GiftedAchievement(string name, string desc, int id)
        {
            this.name = name;
            this.desc = desc;

            this.id = id;

            settings = new AchievementSettings("Gift: " + name,
                "gift_" + id.ToString(), null, FeatureStage.Tested_2);
        }

        public void NetSend(HeroQuest.Players.AbsHQPlayer reciever)
        {
            if (reciever != null)
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqGiftAchievement, 
                    Network.PacketReliability.Reliable);
                HeroQuest.hqRef.players.netWritePlayer(w, reciever);
                w.Write((byte)id);
            }
        }

        public static void NetRead(System.IO.BinaryReader r)
        {
            HeroQuest.hqRef.players.netReadPlayer(r);
            int id = r.ReadByte();

            var list = toggRef.achievements.giftedList();
            foreach (var m in list)
            {
                if (m.id == id)
                {
                    m.Unlock();
                    break;
                }
            }
        }

        public void Unlock()
        {
            settings.Unlock();
        }
    }
}
