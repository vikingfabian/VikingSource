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
            content.text(string.Format(DssRef.lang.EndGameStatistics_SoldiersRecruited, SoldiersRecruited));
            content.text(string.Format(DssRef.lang.EndGameStatistics_FriendlySoldiersLost, FriendlySoldiersLost));
            content.text(string.Format(DssRef.lang.EndGameStatistics_EnemySoldiersKilled, EnemySoldiersKilled));
            content.text(string.Format(DssRef.lang.EndGameStatistics_CitiesCaptured, CitiesCaptured));
            content.text(string.Format(DssRef.lang.EndGameStatistics_CitiesLost, CitiesLost));
            content.text(string.Format(DssRef.lang.EndGameStatistics_BattlesWon, BattlesWon));
            content.text(string.Format(DssRef.lang.EndGameStatistics_BattlesLost, BattlesLost));
            content.text(string.Format(DssRef.lang.EndGameStatistics_WarsStartedByYou, WarsStartedByYou));
            content.text(string.Format(DssRef.lang.EndGameStatistics_WarsStartedByEnemy, WarsStartedByEnemy));
            content.newParagraph();
            content.text(string.Format(DssRef.lang.EndGameStatistics_AlliedFactions, AlliedFactions));
            content.text(string.Format(DssRef.lang.EndGameStatistics_ServantFactions, ServantFactions));
        }
    }
}
