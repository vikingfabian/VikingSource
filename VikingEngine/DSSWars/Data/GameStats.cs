using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.SteamWrapping;

namespace VikingEngine.DSSWars.Data
{
    class GameStats : AbsGameStats
    {
        public StatsInt startTutorial = new StatsInt("start_tutorial");
        public StatsInt completeTutorial = new StatsInt("complete_tutorial");
        public StatsInt skipTutorial = new StatsInt("skip_tutorial");

        public StatsInt startNewStory = new StatsInt("startnew_story");
        public StatsInt startNewSandbox = new StatsInt("startnew_sandbox");
        public StatsInt startNewPeaceful = new StatsInt("startnew_peaceful");
        public StatsInt keyboard_user = new StatsInt("keyboard_user");
        public StatsInt controller_user = new StatsInt("controller_user");

        public StatsInt startNew25perc = new StatsInt("startnew_25perc");
        public StatsInt startNew50perc = new StatsInt("startnew_50perc");
        public StatsInt startNew75perc = new StatsInt("startnew_75perc");
        public StatsInt startNew100perc = new StatsInt("startnew_100perc");
        public StatsInt startNew125perc = new StatsInt("startnew_125perc");
        public StatsInt startNew150perc = new StatsInt("startnew_150perc");
        public StatsInt startNew175perc = new StatsInt("startnew_175perc");
        public StatsInt startNew200perc = new StatsInt("startnew_200perc");

        public StatsInt startNew_MapSmall = new StatsInt("startnew_map_small");
        public StatsInt startNew_MapLarge = new StatsInt("startnew_map_large");
        public StatsInt startNew_MapHuge = new StatsInt("startnew_map_huge");

        public StatsInt startNewLocalMultiplayer = new StatsInt("startnew_localmp");

        public StatsInt saveCustomFlag = new StatsInt("save_flag");

        public StatsInt won25perc = new StatsInt("won_25perc");
        public StatsInt won50perc = new StatsInt("won_50perc");
        public StatsInt won75perc = new StatsInt("won_75perc");
        public StatsInt won100perc = new StatsInt("won_100perc");
        public StatsInt won125perc = new StatsInt("won_125perc");
        public StatsInt won150perc = new StatsInt("won_150perc");
        public StatsInt won175perc = new StatsInt("won_175perc");
        public StatsInt won200perc = new StatsInt("won_200perc");

        public StatsInt lost25perc = new StatsInt("lost_25perc");
        public StatsInt lost50perc = new StatsInt("lost_50perc");
        public StatsInt lost75perc = new StatsInt("lost_75perc");
        public StatsInt lost100perc = new StatsInt("lost_100perc");
        public StatsInt lost125perc = new StatsInt("lost_125perc");
        public StatsInt lost150perc = new StatsInt("lost_150perc");
        public StatsInt lost175perc = new StatsInt("lost_175perc");
        public StatsInt lost200perc = new StatsInt("lost_200perc");

        StatsInt gameLength_passed5min = new StatsInt("lenght_5min");
        StatsInt gameLength_passed15min = new StatsInt("lenght_15min");
        StatsInt gameLength_passed30min = new StatsInt("lenght_30min");
        StatsInt gameLength_passed1hour = new StatsInt("lenght_1h");
        StatsInt gameLength_passed2hour = new StatsInt("lenght_2h");
        StatsInt gameLength_passed5hour = new StatsInt("lenght_5h");
        StatsInt gameLength_passed10hour = new StatsInt("lenght_10h");
        StatsInt gameLength_passed20hour = new StatsInt("lenght_20h");
        StatsInt gameLength_passed30hour = new StatsInt("lenght_30h");

        public GameStats()
        {
            DssRef.stats = this;
        }

        public override List<IStatsValue> collectTimedValues()
        {
            return new List<IStatsValue>
            {
            };
        }

        public override List<IStatsValue> listGlobalStats()
        {
            return new List<IStatsValue>
            {
            startTutorial,
            completeTutorial,
            skipTutorial,

            startNewStory,
            startNewSandbox,
            startNewPeaceful,
            keyboard_user,
            controller_user,

            startNew25perc,
            startNew50perc,
            startNew75perc,
            startNew150perc,
            startNew175perc,
            startNew200perc,

            startNew_MapSmall,
            startNew_MapLarge,
            startNew_MapHuge,

            startNewLocalMultiplayer,//"startnew_localmp");

            saveCustomFlag,//"save_flag");

            won25perc,//"won_25perc");
            won50perc,//"won_50perc");
            won75perc,//"won_75perc");
            won100perc,//"won_100perc");
            won125perc,//"won_125perc");
            won150perc,//"won_150perc");
            won175perc,//"won_175perc");
            won200perc,//"won_200perc");

            lost25perc,//"lost_25perc");
            lost50perc,//"lost_50perc");
            lost75perc,//"lost_75perc");
            lost100perc,//"lost_100perc");
            lost125perc,//"lost_125perc");
            lost150perc,//"lost_150perc");
            lost175perc,//"lost_175perc");
            lost200perc,//"lost_200perc");

            gameLength_passed5min,//"lenght_5min");
            gameLength_passed15min,//"lenght_15min");
            gameLength_passed30min,//"lenght_30min");
            gameLength_passed1hour,//"lenght_1h");
            gameLength_passed2hour,//"lenght_2h");
            gameLength_passed5hour,//"lenght_5h");
            gameLength_passed10hour,//"lenght_10h");
            gameLength_passed20hour,//"lenght_20h");
            gameLength_passed30hour
            };
        }

        public override void getStats()
        {
            startTutorial.getStat();
            completeTutorial.getStat(); 
            skipTutorial.getStat();

            startNewStory.getStat();
            startNewSandbox.getStat();
            startNewPeaceful.getStat();                
            keyboard_user.getStat();
            controller_user.getStat();

            startNew25perc.getStat();
            startNew50perc.getStat();
            startNew75perc.getStat();
            startNew150perc.getStat();
            startNew175perc.getStat();
            startNew200perc.getStat();

            startNew_MapSmall.getStat();
            startNew_MapLarge.getStat();
            startNew_MapHuge.getStat();

            startNewLocalMultiplayer.getStat();//"startnew_localmp");

            saveCustomFlag.getStat();//"save_flag");

            won25perc.getStat();//"won_25perc");
            won50perc.getStat();//"won_50perc");
            won75perc.getStat();//"won_75perc");
            won100perc.getStat();//"won_100perc");
            won125perc.getStat();//"won_125perc");
            won150perc.getStat();//"won_150perc");
            won175perc.getStat();//"won_175perc");
            won200perc.getStat();//"won_200perc");

            lost25perc.getStat();//"lost_25perc");
            lost50perc.getStat();//"lost_50perc");
            lost75perc.getStat();//"lost_75perc");
            lost100perc.getStat();//"lost_100perc");
            lost125perc.getStat();//"lost_125perc");
            lost150perc.getStat();//"lost_150perc");
            lost175perc.getStat();//"lost_175perc");
            lost200perc.getStat();//"lost_200perc");

            gameLength_passed5min.getStat();//"lenght_5min");
            gameLength_passed15min.getStat();//"lenght_15min");
            gameLength_passed30min.getStat();//"lenght_30min");
            gameLength_passed1hour.getStat();//"lenght_1h");
            gameLength_passed2hour.getStat();//"lenght_2h");
            gameLength_passed5hour.getStat();//"lenght_5h");
            gameLength_passed10hour.getStat();//"lenght_10h");
            gameLength_passed20hour.getStat();//"lenght_20h");
            gameLength_passed30hour.getStat();//"lenght_30h");
        }

        public override void collectValues(float prevTotalTimeSec)
        {
            var gametime = DssRef.time.TotalIngameTime();
            if (gametime.TotalMinutes >= 5)
            {
                gameLength_passed5min.addOne_ifUnset();
            }
            if (gametime.TotalMinutes >= 15)
            {
                gameLength_passed15min.addOne_ifUnset();
            }
            if (gametime.TotalMinutes >= 30)
            {
                gameLength_passed30min.addOne_ifUnset();
            }

            if (gametime.TotalHours >= 1)
            {
                gameLength_passed1hour.addOne_ifUnset();
            }
            if (gametime.TotalHours >= 2)
            {
                gameLength_passed2hour.addOne_ifUnset();
            }
            if (gametime.TotalHours >= 5)
            {
                gameLength_passed5hour.addOne_ifUnset();
            }
            if (gametime.TotalHours >= 10)
            {
                gameLength_passed10hour.addOne_ifUnset();
            }
            if (gametime.TotalHours >= 20)
            {
                gameLength_passed20hour.addOne_ifUnset();
            }
            if (gametime.TotalHours >= 30)
            {
                gameLength_passed30hour.addOne_ifUnset();
            }

        }
    }
}
