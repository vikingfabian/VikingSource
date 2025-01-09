#if PCGAME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.SteamWrapping;

namespace VikingEngine.LootFest.Data
{
    class GameStats : AbsGameStats
    {
        StatsInt testint = new StatsInt("testint");
        StatsFloat testfloat = new StatsFloat("testfloat");

        StatsInt editedChunks =  new StatsInt("numEditChunks");
        public StatsInt multiplayerSessions = new StatsInt("numMpSessions");

        public StatsInt debugMenuVisits = new StatsInt("debugMenuVisits");
        public StatsInt editorBlocksPlaced = new StatsInt("editorBlocksPlaced");
        StatsFloat playHours = new StatsFloat("playHours");
        StatsFloat editHours = new StatsFloat("editHours");


        public GameStats()
        {
            LfRef.stats = this;
        }

        public override List<IStatsValue> collectTimedValues()
        {
            return new List<IStatsValue>
            {
                testint,
                testfloat,
                
                editedChunks,
                multiplayerSessions,

                debugMenuVisits,
                editorBlocksPlaced,
                playHours,
                editHours,
            };
        }

        public override List<IStatsValue> listGlobalStats()
        {
            throw new NotImplementedException();
        }

        public override void getStats()
        {
            testint.getStat();
            testfloat.getStat();

            editedChunks.getStat();
            multiplayerSessions.getStat();

            debugMenuVisits.getStat();
            editorBlocksPlaced.getStat();
            playHours.getStat();
            editHours.getStat();
        }

        public override void collectValues(float prevTotalTimeSec)
        {
            //var creativeMap = LfRef.levels2.GetLevelUnsafe(BlockMap.LevelEnum.Creative);
            //if (creativeMap != null)
            //{
            //    editedChunks.value = creativeMap.designAreas.editCount();
            //}

            //playHours.value += timePassed.Hours;
            //var p = LfRef.gamestate.LocalHostingPlayer;
            //if (p.inEditor)
            //{
            //    editHours.value += timePassed.Hours;
            //}
        }
    }
}
#endif