using System;
using System.Collections.Generic;
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
        }
    }
}
