using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Event;
using VikingEngine.DSSWars.Interface.CutScene;
using VikingEngine.SteamWrapping;

namespace VikingEngine.DSSWars.Data
{
    abstract class AbsLeaderBoard: SteamLeaderBoardLocal
    {    
        protected void setup(LeaderBoardType type, int score)
        {
            name = $"{type}_{StartupSettings.LeaderboardVersion}";
            if (StartupSettings.LeaderboardInBeta)
            {
                name = "beta_" + name;
            }
            this.score = score;

            int.TryParse(Engine.LoadContent.EngineVersion, out int version);
            scoreDetails.Add(version);
        }

        public override void BeginUpload()
        {
            if (DssRef.state.importedWorld && DssRef.storage.blockImportAchievements)
            {
                return;
            }

            base.BeginUpload();
        }
    }

    class VictoryLeaderBoard : AbsLeaderBoard
    { 
        public VictoryLeaderBoard(GameEndReason endReason, VictoryType vType)
        {            

            if (endReason == GameEndReason.Victory)
            {
                var difficulty = DssRef.difficulty.TotalDifficulty();

                switch (vType)
                {
                    case VictoryType.DefeatBoss:
                        setup(LeaderBoardType.story_difficulty, difficulty);
                        scoreDetails.Add((int)DssRef.time.TotalIngameTime().TotalSeconds);
                        BeginUpload();
                        break;

                    case VictoryType.Domination:
                        LeaderBoardType type;

                        if (difficulty >= 150)
                        {
                            type = LeaderBoardType.domination_speed150;
                        }
                        else if (difficulty >= 100)
                        {
                            type = LeaderBoardType.domination_speed100;
                        }
                        else
                        {
                            type = LeaderBoardType.domination_speed50;
                        }
                        setup(type, (int)DssRef.time.TotalIngameTime().TotalSeconds);
                        scoreDetails.Add(difficulty);
                        BeginUpload();
                        break;

                }
            }
        }
    }

    enum LeaderBoardType
    {
        story_difficulty,
        domination_speed50,
        domination_speed100,
        domination_speed150,
        city_size,
        survive300_time,
    }
}

