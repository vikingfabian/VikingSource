#if PCGAME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.SteamWrapping;

namespace VikingEngine.PJ.PjEngine
{
    class GameStats : AbsGameStats
    {
        StatsInt testint = new StatsInt("testint");
        StatsFloat testfloat = new StatsFloat("testfloat");

        StatsInt dlcCount = new StatsInt("dlc");
        StatsFloat playHoursTotal = new StatsFloat("playHoursTotal");
        public StatsInt multiplayerSessions = new StatsInt("numMpSessions");

        StatsFloat modeJoustHours = new StatsFloat("modeJoustHours");
        StatsFloat modeBagatelleHours = new StatsFloat("modeBagatelleHours");
        StatsFloat modeGolfHours = new StatsFloat("modeGolfHours");
        StatsFloat modeCarBallHours = new StatsFloat("modeCarBallHours");
        StatsFloat modeRiskHours = new StatsFloat("modeRiskHours");
        StatsFloat modeSpaceWarsHours = new StatsFloat("modeSpaceWarsHours");

        public GameStats()
        {
            PjRef.stats = this;
        }

        public override List<IStatsValue> listValues()
        {
            return new List<IStatsValue>
            {
                testint,
                testfloat,

                dlcCount,
                playHoursTotal,
                multiplayerSessions,

                modeJoustHours,
                modeBagatelleHours,
                modeGolfHours,
                modeCarBallHours,
                modeRiskHours,
                modeSpaceWarsHours,
            };
        }

        public override void getStats()
        {
            testint.getStat();
            testfloat.getStat();

            dlcCount.getStat();
            playHoursTotal.getStat();
            multiplayerSessions.getStat();

            modeJoustHours.getStat();
            modeBagatelleHours.getStat();
            modeGolfHours.getStat();
            modeCarBallHours.getStat();
            modeRiskHours.getStat();
            modeSpaceWarsHours.getStat();
        }

        public override void collectValues(TimeStamp timePassed)
        {
            playHoursTotal.value += timePassed.Hours;

            switch (PjRef.storage.Mode)
            {
                case PartyGameMode.Jousting:
                    modeJoustHours.value += timePassed.Hours;
                    break;
                case PartyGameMode.Bagatelle:
                    modeBagatelleHours.value += timePassed.Hours;
                    break;
                case PartyGameMode.MiniGolf:
                    modeGolfHours.value += timePassed.Hours;
                    break;
                case PartyGameMode.CarBall:
                    modeCarBallHours.value += timePassed.Hours;
                    break;
                case PartyGameMode.Strategy:
                    modeRiskHours.value += timePassed.Hours;
                    break;
                case PartyGameMode.SpacePirate:
                    modeSpaceWarsHours.value += timePassed.Hours;
                    break;

            }

            dlcCount.value = Ref.steam.DLC.Count();
        }
    }
}
#endif