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

        public const int DiplomacyDifficultyCount = 3;
        public int diplomacyDifficulty = 1;

        public bool honorGuard = true;

        public bool allowPauseCommand = true;
        public bool boss = true;

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
                    "Ai aggressivity: " + TextLib.IndexDivition((int)difficulty.aiAggressivity, (int)AiAggressivity.NUM) + "." + Environment.NewLine +
                    "Boss size: " + TextLib.IndexDivition((int)difficulty.bossSize, (int)BossSize.NUM) + "." + Environment.NewLine +
                    "Boss enter time: " + TextLib.IndexDivition((int)difficulty.bossTimeSettings, (int)BossTimeSettings.NUM) + "." + Environment.NewLine +
                    "Ai Economy: " + AiEconomyLevel[difficulty.aiEconomyLevel].ToString() + "%. " + Environment.NewLine +
                    "Diplomacy difficulty: " + TextLib.IndexDivition(difficulty.diplomacyDifficulty, DiplomacyDifficultyCount) + "." + Environment.NewLine +
                    "Honor guards: " + TextLib.YesNo(difficulty.honorGuard) + ".",

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
            if (!allowPauseCommand)
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
            switch (difficulty)
            {
                case 0:
                    aiAggressivity = AiAggressivity.Low;
                    bossSize = BossSize.Small;
                    bossTimeSettings = BossTimeSettings.VeryLate;
                    aiEconomyLevel = 0;
                    diplomacyDifficulty = 0;
                    honorGuard = true;
                    break;

                case 1:
                    aiAggressivity = AiAggressivity.Low;
                    bossSize = BossSize.Small;
                    bossTimeSettings = BossTimeSettings.Late;
                    aiEconomyLevel = 1;
                    diplomacyDifficulty = 0;
                    honorGuard = true;
                    break;

                case 2:
                    aiAggressivity = AiAggressivity.Low;
                    bossSize = BossSize.Medium;
                    bossTimeSettings = BossTimeSettings.Late;
                    aiEconomyLevel = 1;
                    diplomacyDifficulty = 1;
                    honorGuard = true;
                    break;

                case 3: //Medium
                    aiAggressivity = AiAggressivity.Medium;
                    bossSize = BossSize.Medium;
                    bossTimeSettings = BossTimeSettings.Normal;
                    aiEconomyLevel = 2;
                    diplomacyDifficulty = 1;
                    honorGuard = true;
                    break;

                case 4:
                    aiAggressivity = AiAggressivity.Medium;
                    bossSize = BossSize.Medium;
                    bossTimeSettings = BossTimeSettings.Normal;
                    aiEconomyLevel = 2;
                    diplomacyDifficulty = 1;
                    honorGuard = false;
                    break;

                case 5:
                    aiAggressivity = AiAggressivity.Medium;
                    bossSize = BossSize.Large;
                    bossTimeSettings = BossTimeSettings.Early;
                    aiEconomyLevel = 2;
                    diplomacyDifficulty = 1;
                    honorGuard = false;
                    break;

                case 6:
                    aiAggressivity = AiAggressivity.High;
                    bossSize = BossSize.Huge;
                    bossTimeSettings = BossTimeSettings.Early;
                    aiEconomyLevel = 3;
                    diplomacyDifficulty = 2;
                    honorGuard = false;
                    break;

                case 7: //Max
                    aiAggressivity = AiAggressivity.High;
                    bossSize = BossSize.Huge;
                    bossTimeSettings = BossTimeSettings.Immediate;
                    aiEconomyLevel = 4;
                    diplomacyDifficulty = 2;
                    honorGuard = false;
                    break;
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(allowPauseCommand);
            w.Write(boss);
            w.Write(difficulty);
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            allowPauseCommand = r.ReadBoolean();
            boss = r.ReadBoolean();
            difficulty = r.ReadInt32();

            refreshSettings(difficulty);
        }

    }
}
