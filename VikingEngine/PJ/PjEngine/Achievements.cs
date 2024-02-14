using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.PlatformService;

namespace VikingEngine.PJ.PjEngine
{
    class Achievements
    {
        public const float BagatelleLongLiveTimeSec = 20;

        //SECRET
        public AchievementSettings secretWolfScare = new AchievementSettings("Wolf Scare", "secret1a", "2", FeatureStage.Tested_2);//
        public AchievementSettings secretMeatPie = new AchievementSettings("Meat Pie", "secret1b", "3", FeatureStage.Tested_2);//
        public AchievementSettings secretChickenEgg = new AchievementSettings("Chicken and the Egg", "secret2", "4", FeatureStage.Tested_2);//
        public AchievementSettings secretToasty = new AchievementSettings("Toasty", "secret3", "5", FeatureStage.Tested_2);//

        //JOUST
        public AchievementSettings joustDiveKill = new AchievementSettings("Joust dive kill", "joust_divekill", "6", FeatureStage.Tested_2);//
        public AchievementSettings joustLowFire = new AchievementSettings("Joust laser kill laying", "joust_laserlaying", "7", FeatureStage.Tested_2);//
        public AchievementSettings joustDoubleKill = new AchievementSettings("Joust double kill", "joust_2kill", "8", FeatureStage.Tested_2);//

        //BAGATELLE
        public AchievementSettings bagatelleDoubleKill = new AchievementSettings("Bagatelle one ball two kill", "bag_2kill", "9", FeatureStage.Tested_2);//
        public AchievementSettings bagatelleBoth20s = new AchievementSettings("Bagatelle both 20 coins", "bag_bigcoins", "10", FeatureStage.Tested_2);//
        public AchievementSettings bagatelleLongLive = new AchievementSettings("Bagatelle long live", "bag_live", "11", FeatureStage.Tested_2);//

        //GOLF
        public AchievementSettings golfBombHit = new AchievementSettings("Golf bomb hit", "golf_bomb", "12", FeatureStage.Tested_2);//

        /// <summary>
        /// Pick flag and drop in hole
        /// </summary>
        public AchievementSettings golfBirdie = new AchievementSettings("Golf birdie", "golf_birdie", "13", FeatureStage.Tested_2);//
        public AchievementSettings golfCollect3 = new AchievementSettings("Golf collect 3", "golf_coll3", "14", FeatureStage.Tested_2);//
        public AchievementSettings golfSpikeDodge = new AchievementSettings("Golf spike dodge", "golf_dodge", "15", FeatureStage.Tested_2);//

        //CAR BALL
        public AchievementSettings cballSave = new AchievementSettings("Carball paddle save", "cball_save", "16", FeatureStage.Tested_2);//
        public AchievementSettings cballDoubleKill = new AchievementSettings("Carball double kill", "cball_2kill", "17", FeatureStage.Tested_2);//

        /// <summary>
        /// Two goals without touching walls
        /// </summary>
        public AchievementSettings cballFieldTwoGoal = new AchievementSettings("Carball on field two goals", "cball_field2goal", "18", FeatureStage.Tested_2);//

        //MATCH3
        public AchievementSettings m3Combo3 = new AchievementSettings("Match3 Three combo", "m3_combo3", "19", FeatureStage.Tested_2);//
        public AchievementSettings m3Timeout = new AchievementSettings("Match3 Time out", "m3_timeout", "20", FeatureStage.Tested_2);//


        public Achievements()
        {
            PjRef.achievements = this;
        }

        public List<AchievementSettings> All()
        {
            return new List<AchievementSettings>
            {
                secretWolfScare,
                secretMeatPie,
                secretChickenEgg,
                secretToasty,

                joustDiveKill,
                joustLowFire,
                joustDoubleKill,

                bagatelleDoubleKill,
                bagatelleBoth20s,
                bagatelleLongLive,

                golfBombHit,
                golfBirdie,
                golfCollect3,
                golfSpikeDodge,
                cballSave,
                cballDoubleKill,
                cballFieldTwoGoal,
                m3Combo3,
                m3Timeout,
            };
        }
    }
}
