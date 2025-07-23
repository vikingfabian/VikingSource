using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Players.PlayerControls.Casual;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        public CasualCityProfile casualCityProfile = new CasualCityProfile();
        CityCasualProgress casualProgress = null;

        public void CasualBuild(CasualBuildType type, int price, int count)
        { 
            
        }

        public CityCasualProgress GetCasualProgress()
        {
            if (casualProgress == null)
            {
                casualProgress = new CityCasualProgress();
            }

            return casualProgress;
        }

        public int casualRecruitTime_sec(CasualSoldierType soldierType)
        {
            int barracksCount = Math.Min(buildingStructure.SoldierBarracks_count, buildingStructure.ArcherBarracks_count);
            if (barracksCount == 0)
            {
                barracksCount = 1;
            }

            return Convert.ToInt32(ConscriptProfile.TrainingTime(soldierType) / barracksCount);
        }
        //protected void initCasual()
        //{


        //}
    }
}
