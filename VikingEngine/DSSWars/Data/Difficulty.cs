using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.HUD;
using VikingEngine.Input;

namespace VikingEngine.DSSWars.Data
{
    class Difficulty
    {
        public const int DefaultOption = 2;

        int difficulty = DefaultOption;

        static readonly int[] options = new int[] { 25, 50, 75, 100, 125, 150, 175, 200 };
        public static readonly int[] AiEconomyLevel = new int[] { 50, 75, 100, 125, 150 };

        public AiAggressivity aiAggressivity = AiAggressivity.Medium;
        public BossSize bossSize = BossSize.Medium;
        public BossTimeSettings bossTimeSettings = BossTimeSettings.Normal;
        

        public int aiEconomyLevel = 1;
        public double aiEconomyMultiplier = 1.0;
        public int aiDelayTimeSec = 0;

        public const int DiplomacyDifficultyCount = 3;
        public int diplomacyDifficulty = 1;

        public bool honorGuard = true;
        public bool resourcesStartHelp = false;

        public bool setting_allowPauseCommand = true;
        public float setting_foodMulti = 1;
        public GameMode setting_gameMode = GameMode.FullStory;
        public bool boss = true;
        public bool peaceful = false;

        public int MercenaryPurchaseCost_Start;
        public int MercenaryPurchaseCost_Add;
        public float toPeacefulPercentage;

        public double resourceMultiplyChance = 0;
        public bool resourceMultiplyDecrease;

        public Difficulty(int difficulty = DefaultOption)
        {
            set(difficulty);
        }

        public int PercDifficulty => options[difficulty];

        public static void OptionsGui(GuiLayout layout, Action<int> difficultyOptionsLink)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Difficulty difficulty = new Difficulty(i);

                new GuiTextButton(options[i].ToString() + "%",
                    string.Format( DssRef.lang.DifficultyDescription_AiAggression, TextLib.IndexDivition((int)difficulty.aiAggressivity, (int)AiAggressivity.NUM)) + Environment.NewLine +
                    string.Format(DssRef.lang.DifficultyDescription_BossSize,TextLib.IndexDivition((int)difficulty.bossSize, (int)BossSize.NUM)) + Environment.NewLine +
                    string.Format(DssRef.lang.DifficultyDescription_BossEnterTime, TextLib.IndexDivition((int)difficulty.bossTimeSettings, (int)BossTimeSettings.NUM)) + Environment.NewLine +
                    string.Format(DssRef.lang.DifficultyDescription_AiEconomy, AiEconomyLevel[difficulty.aiEconomyLevel].ToString()) + Environment.NewLine +
                    string.Format(DssRef.lang.DifficultyDescription_AiDelay, TimeSpan.FromSeconds(difficulty.aiDelayTimeSec).ToString()) + Environment.NewLine +
                    string.Format(DssRef.lang.DifficultyDescription_DiplomacyDifficulty, TextLib.IndexDivition(difficulty.diplomacyDifficulty, DiplomacyDifficultyCount)) + Environment.NewLine +
                    string.Format(DssRef.lang.DifficultyDescription_MercenaryCost, difficulty.MercenaryPurchaseCost_Start.ToString() )+ Environment.NewLine +
                    string.Format(DssRef.lang.DifficultyDescription_HonorGuards, difficulty.honorGuard? Ref.langOpt.Hud_Yes : Ref.langOpt.Hud_No),

                    new GuiAction1Arg<int>(difficultyOptionsLink, i),
                    false, layout);
            }
        }

        public void set(int difficulty)
        {
            this.difficulty = difficulty;
            refreshSettings(difficulty);
        }

        public int TotalDifficulty()
        {
            double result = PercDifficulty;
            if (!setting_allowPauseCommand)
            {
                result *= 1.25;
            }
            if (!boss)
            {
                result *= 0.75;
            }

            return Convert.ToInt32(result);
        }

        void refreshSettings(int difficulty)
        {
            switch (setting_gameMode)
            {
                case GameMode.FullStory:
                    boss = true;
                    peaceful = false; 
                    break;
                case GameMode.Sandbox:
                    boss = false;
                    peaceful = false;
                    break;
                case GameMode.Peaceful:
                    boss = false;
                    peaceful = true;
                    break;
            }

            switch (difficulty)
            {
                case 0:
                    aiAggressivity = AiAggressivity.Low;
                    bossSize = BossSize.Small;
                    bossTimeSettings = BossTimeSettings.VeryLate;
                    aiEconomyLevel = 0;
                    resourceMultiplyChance = 0.5;
                    resourceMultiplyDecrease = true;
                    diplomacyDifficulty = 0;
                    honorGuard = true;
                    resourcesStartHelp = true;
                    aiDelayTimeSec = 30 * TimeExt.MinuteInSeconds;
                    toPeacefulPercentage = 0.01f;
                    break;

                case 1:
                    aiAggressivity = AiAggressivity.Low;
                    bossSize = BossSize.Small;
                    bossTimeSettings = BossTimeSettings.Late;
                    aiEconomyLevel = 1;
                    resourceMultiplyChance = 0.25;
                    resourceMultiplyDecrease = true;
                    diplomacyDifficulty = 0;
                    honorGuard = true;
                    resourcesStartHelp = true;
                    aiDelayTimeSec = 15 * TimeExt.MinuteInSeconds;
                    toPeacefulPercentage = 0.05f;
                    break;

                case 2:
                    aiAggressivity = AiAggressivity.Low;
                    bossSize = BossSize.Medium;
                    bossTimeSettings = BossTimeSettings.Late;
                    aiEconomyLevel = 1;
                    resourceMultiplyChance = 0.25;
                    resourceMultiplyDecrease = true;
                    diplomacyDifficulty = 1;
                    honorGuard = true;
                    resourcesStartHelp = true;
                    aiDelayTimeSec = 8 * TimeExt.MinuteInSeconds;
                    toPeacefulPercentage = 0.1f;
                    break;

                case 3: //Medium
                    aiAggressivity = AiAggressivity.Medium;
                    bossSize = BossSize.Medium;
                    bossTimeSettings = BossTimeSettings.Normal;
                    aiEconomyLevel = 2;
                    diplomacyDifficulty = 1;
                    honorGuard = true;
                    aiDelayTimeSec = 30;
                    toPeacefulPercentage = 0.2f;
                    break;

                case 4:
                    aiAggressivity = AiAggressivity.Medium;
                    bossSize = BossSize.Medium;
                    bossTimeSettings = BossTimeSettings.Normal;
                    aiEconomyLevel = 2;
                    diplomacyDifficulty = 1;
                    honorGuard = false;
                    aiDelayTimeSec = 10;
                    toPeacefulPercentage = 0.5f;
                    break;

                case 5:
                    aiAggressivity = AiAggressivity.Medium;
                    bossSize = BossSize.Large;
                    bossTimeSettings = BossTimeSettings.Early;
                    aiEconomyLevel = 2;
                    diplomacyDifficulty = 1;
                    honorGuard = false;
                    toPeacefulPercentage = 0.75f;
                    break;

                case 6:
                    aiAggressivity = AiAggressivity.High;
                    bossSize = BossSize.Huge;
                    bossTimeSettings = BossTimeSettings.Early;
                    aiEconomyLevel = 3;

                    resourceMultiplyChance = 0.25;
                    resourceMultiplyDecrease = false;
                    diplomacyDifficulty = 2;
                    honorGuard = false;
                    toPeacefulPercentage = 1.5f;
                    break;

                case 7: //Max
                    aiAggressivity = AiAggressivity.High;
                    bossSize = BossSize.Huge;
                    bossTimeSettings = BossTimeSettings.Immediate;
                    aiEconomyLevel = 4;


                    resourceMultiplyChance = 0.5;
                    resourceMultiplyDecrease = false;
                    diplomacyDifficulty = 2;
                    honorGuard = false;
                    toPeacefulPercentage = 2f;
                    break;
            }

            int mediumOffset = difficulty - 3;

            MercenaryPurchaseCost_Start = 3500 + mediumOffset * 500;
            MercenaryPurchaseCost_Add = 100 + mediumOffset * 20;

            aiEconomyMultiplier = AiEconomyLevel[aiEconomyLevel] / 100.0;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(setting_allowPauseCommand);
            w.Write(boss);
            w.Write(difficulty);
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            setting_allowPauseCommand = r.ReadBoolean();
            boss = r.ReadBoolean();
            difficulty = r.ReadInt32();

            refreshSettings(difficulty);
        }

    }


    enum GameMode
    { 
        FullStory,
        Sandbox,
        Peaceful,
        NUM
    }
    //enum AiResourceMultiplyType
    //{ 
    //    None,
    //    Add,
    //    Remove,
    //}
}
