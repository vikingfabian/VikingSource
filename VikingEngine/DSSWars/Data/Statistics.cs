using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Data
{
    class Statistics
    {
        public int SoldiersRecruited = 0;

        public int FriendlySoldiersLost = 0;
        public int EnemySoldiersKilled = 0;
        public int SoldiersDeserted = 0;

        public int CitiesCaptured = 0;
        public int CitiesLost = 0;

        public int BattlesWon = 0;
        public int BattlesLost = 0;

        public int WarsStartedByYou = 0;
        public int WarsStartedByEnemy = 0;

        public int AlliedFactions = 0;
        public int ServantFactions = 0;

        int decorBuilt = 0;
        int statuesBuilt = 0;


        public void ToHud(RichBoxContent content)
        {
            content.text(string.Format(DssRef.lang.EndGameStatistics_SoldiersRecruited, TextLib.LargeNumber(SoldiersRecruited)));
            content.text(string.Format(DssRef.lang.EndGameStatistics_FriendlySoldiersLost, TextLib.LargeNumber(FriendlySoldiersLost)));
            content.text(string.Format(DssRef.lang.EndGameStatistics_EnemySoldiersKilled, TextLib.LargeNumber(EnemySoldiersKilled)));
            content.text(string.Format(DssRef.lang.EndGameStatistics_SoldiersDeserted, TextLib.LargeNumber(SoldiersDeserted)));
            content.text(string.Format(DssRef.lang.EndGameStatistics_CitiesCaptured, TextLib.LargeNumber(CitiesCaptured)));
            content.text(string.Format(DssRef.lang.EndGameStatistics_CitiesLost, TextLib.LargeNumber(CitiesLost)));
            content.text(string.Format(DssRef.lang.EndGameStatistics_BattlesWon, TextLib.LargeNumber(BattlesWon)));
            content.text(string.Format(DssRef.lang.EndGameStatistics_BattlesLost, TextLib.LargeNumber(BattlesLost)));
            content.text(string.Format(DssRef.lang.EndGameStatistics_WarsStartedByYou, TextLib.LargeNumber(WarsStartedByYou)));
            content.text(string.Format(DssRef.lang.EndGameStatistics_WarsStartedByEnemy, TextLib.LargeNumber(WarsStartedByEnemy)));
            content.newParagraph();
            content.text(string.Format(DssRef.lang.EndGameStatistics_AlliedFactions, TextLib.LargeNumber(AlliedFactions)));
            content.text(string.Format(DssRef.lang.EndGameStatistics_ServantFactions, TextLib.LargeNumber(ServantFactions)));
            content.newParagraph();
            content.text(string.Format(DssRef.lang.EndGameStatistics_StatuesBuilt, statuesBuilt));
            content.text(string.Format(DssRef.lang.EndGameStatistics_DecorsBuilt, decorBuilt));

        }

        public void onDecorBuild_async(bool statue)
        {
            decorBuilt++;
            if (statue)
            {
                DssRef.achieve.UnlockAchievement_async(AchievementIndex.statue);
                statuesBuilt++;
            }

            if (decorBuilt >= Achievements.DecorationsTotalCount &&
                statuesBuilt >= Achievements.DecorationsStatueCount)
            {
                DssRef.achieve.UnlockAchievement_async(AchievementIndex.decorations);
            }
        }

        public void writeGameState(BinaryWriter w)
        {
            w.Write(SoldiersRecruited);
            w.Write(FriendlySoldiersLost);
            w.Write(EnemySoldiersKilled);

            w.Write((ushort)CitiesCaptured);
            w.Write((ushort)CitiesLost);
            w.Write((ushort)BattlesWon);
            w.Write((ushort)BattlesLost);
            w.Write((ushort)WarsStartedByYou);
            w.Write((ushort)WarsStartedByEnemy);
            w.Write((ushort)AlliedFactions);
            w.Write((ushort)ServantFactions);

            w.Write((ushort)decorBuilt);
            w.Write((ushort)statuesBuilt);
        }

        public void readGameState(BinaryReader r, int subVersion)
        {
             SoldiersRecruited =r.ReadInt32();
             FriendlySoldiersLost = r.ReadInt32();
            EnemySoldiersKilled = r.ReadInt32();

            CitiesCaptured = r.ReadUInt16();
            CitiesLost = r.ReadUInt16();
            BattlesWon = r.ReadUInt16();
            BattlesLost = r.ReadUInt16();
            WarsStartedByYou = r.ReadUInt16();
            WarsStartedByEnemy = r.ReadUInt16();
            AlliedFactions = r.ReadUInt16();
            ServantFactions = r.ReadUInt16();

            if (subVersion >= 17)
            {
                decorBuilt = r.ReadUInt16();
                statuesBuilt = r.ReadUInt16();
            }
        }
    }
}
