using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.Event;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;

namespace VikingEngine.DSSWars.Data
{
    class Difficulty
    {
        public const int DefaultOption = 2;

        int difficulty = DefaultOption;

        static readonly int[] options = new int[] { 25, 50, 75, 100, 125, 150, 175, 200 };
        public static readonly int[] AiEconomyLevel = new int[] { 50, 75, 100, 125, 150 };
        public static readonly GameModeMainType[] AvailableModes = [GameModeMainType.FullStory, GameModeMainType.Sandbox, GameModeMainType.Peaceful, GameModeMainType.Spectator];

        public AiAggressivity aiAggressivity = AiAggressivity.Medium;
        public BossSize bossSize = BossSize.Medium;
        //public BossTimeSettings bossTimeSettings = BossTimeSettings.Normal;
        
        public int aiEconomyLevel = 1;
        public double aiEconomyMultiplier = 1.0;
        public int aiDelayTimeSec = 0;

        public const int DiplomacyDifficultyCount = 3;
        public int diplomacyDifficulty = 1;

        public bool honorGuard = true;
        public bool resourcesStartHelp = false;

        public bool setting_allowPauseCommand = true;
        public float setting_foodMulti = 1;
        public float setting_waterMulti = 1;
        public float setting_childMulti = 1;
        public float setting_craftMulti = 1;
        public const GameModeMainType DefaultMode = GameModeMainType.FullStory;
        public GameModeMainType setting_gameMode = DefaultMode;
        public bool runStory = true;
        public bool peaceful = false;
        //public bool toPeacefulCheck = true;

        public int MercenaryPurchaseCost_Start;
        public int MercenaryPurchaseCost_Add;
        public float toPeacefulPercentage=0;

        public double resourceMultiplyChance = 0;
        public bool resourceMultiplyDecrease;
        public int FoodEnergySett;
        public int PlayerBonusGold = 0;

        public Difficulty(int difficulty = DefaultOption)
        {
            set(difficulty);
        }

        public int PercDifficulty => options[difficulty];

        public static void OptionsGui(GuiLayout layout, Action<int> difficultyOptionsLink)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Difficulty difficultyLvl = new Difficulty(i);

                new GuiTextButton(options[i].ToString() + "%",
                    string.Format( DssRef.lang.DifficultyDescription_AiAggression, TextLib.IndexDivition((int)difficultyLvl.aiAggressivity, (int)AiAggressivity.NUM)) + Environment.NewLine +
                    string.Format(DssRef.lang.DifficultyDescription_BossSize,TextLib.IndexDivition((int)difficultyLvl.bossSize, (int)BossSize.NUM)) + Environment.NewLine +
                    //string.Format(DssRef.lang.DifficultyDescription_BossEnterTime, TextLib.IndexDivition((int)difficultyLvl.bossTimeSettings, (int)BossTimeSettings.NUM)) + Environment.NewLine +
                    string.Format(DssRef.lang.DifficultyDescription_AiEconomy, AiEconomyLevel[difficultyLvl.aiEconomyLevel].ToString()) + Environment.NewLine +
                    //string.Format(DssRef.lang.DifficultyDescription_AiDelay, TimeSpan.FromSeconds(difficultyLvl.aiDelayTimeSec).ToString()) + Environment.NewLine +
                    string.Format(DssRef.lang.DifficultyDescription_DiplomacyDifficulty, TextLib.IndexDivition(difficultyLvl.diplomacyDifficulty, DiplomacyDifficultyCount)) + Environment.NewLine +
                    //string.Format(DssRef.lang.DifficultyDescription_MercenaryCost, difficulty.MercenaryPurchaseCost_Start.ToString() )+ Environment.NewLine +
                    string.Format(DssRef.lang.DifficultyDescription_HonorGuards, difficultyLvl.honorGuard? Ref.langOpt.Hud_Yes : Ref.langOpt.Hud_No),

                    new GuiAction1Arg<int>(difficultyOptionsLink, i),
                    false, layout);
            }
        }

        public static void OptionsRb(RichBoxContent content, RichMenu menu, Action<int> callback)
        {
            DropDownBuilder mapSzOptions = new DropDownBuilder("difficulty");
            for (int i = 0; i < options.Length; i++)
            {
                Difficulty difficultyLvl = new Difficulty(i);
                content.newLine();
                mapSzOptions.AddOption(options[i].ToString() + "%", DssRef.difficulty.difficulty == i, DefaultOption == i,
                    new RbAction1Arg<int>(callback, i), new RbTooltip(difficultyToolTip, i));
                //content.Add(new ArtOption(DssRef.difficulty.difficulty == i, new List<AbsRichBoxMember> { new RbText(options[i].ToString() + "%") }, null));
            }
            mapSzOptions.DropDown(content, SpriteName.NO_IMAGE, string.Format(DssRef.lang.Settings_DifficultyLevel, DssRef.difficulty.PercDifficulty), menu.OnDropDownClick, menu.activeDropDown);
        }

        static void difficultyToolTip(RichBoxContent content, object tag)
        {
            Difficulty difficultyLvl = new Difficulty((int)tag);
            content.h2(string.Format(DssRef.lang.Settings_DifficultyLevel, difficultyLvl.PercDifficulty), HudLib.TitleColor_Head);

            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.WarsBattleIcon));
                content.hspace();
                content.Add(new RbText(string.Format(DssRef.lang.DifficultyDescription_AiAggression, TextLib.IndexDivition((int)difficultyLvl.aiAggressivity, (int)AiAggressivity.NUM))));
            }
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.WarsDarkLordBossIcon));
                content.hspace();
                content.Add(new RbText(string.Format(DssRef.lang.DifficultyDescription_BossSize, TextLib.IndexDivition((int)difficultyLvl.bossSize, (int)BossSize.NUM))));
            }
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.rtsIncome));
                content.hspace();
                content.Add(new RbText(string.Format(DssRef.lang.DifficultyDescription_AiEconomy, AiEconomyLevel[difficultyLvl.aiEconomyLevel].ToString())));
            }
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.WarsDiplomaticPoint));
                content.hspace();
                content.Add(new RbText(string.Format(DssRef.lang.DifficultyDescription_DiplomacyDifficulty, TextLib.IndexDivition(difficultyLvl.diplomacyDifficulty, DiplomacyDifficultyCount))));
            }
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.WarsUnitIcon_Honorguard));
                content.hspace();
                content.Add(new RbText(string.Format(DssRef.lang.DifficultyDescription_HonorGuards, difficultyLvl.honorGuard ? Ref.langOpt.Hud_Yes : Ref.langOpt.Hud_No)));
            }

        }

        public void set(int difficulty)
        {
            this.difficulty = difficulty;
            refreshSettings();
        }

        public int TotalDifficulty()
        {
            double result = PercDifficulty;
            if (!setting_allowPauseCommand)
            {
                result *= 1.25;
            }
            if (!DssRef.storage.centralGold)
            {
                result *= 1.5;
            }
            switch (setting_gameMode)
            {
                case GameModeMainType.Sandbox:
                    result *= 0.75;
                    break;
                case GameModeMainType.Peaceful:
                    result *= 0.25;
                    break;
            }

            return Convert.ToInt32(result);
        }

        public void refreshSettings()
        {
            FoodEnergySett = Convert.ToInt32(DssConst.FoodEnergy * setting_foodMulti);

            

            switch (difficulty)
            {
                case 0:
                    aiAggressivity = AiAggressivity.Low;
                    bossSize = BossSize.Small;
                    //bossTimeSettings = BossTimeSettings.VeryLate;
                    aiEconomyLevel = 0;
                    resourceMultiplyChance = 0.5;
                    resourceMultiplyDecrease = true;
                    diplomacyDifficulty = 0;
                    honorGuard = true;
                    resourcesStartHelp = true;
                    //toPeacefulCheck = false;
                    aiDelayTimeSec = 30 * TimeExt.MinuteInSeconds;
                    //toPeacefulPercentage = 0.01f;
                    PlayerBonusGold = 6000;
                    break;

                case 1:
                    aiAggressivity = AiAggressivity.Low;
                    bossSize = BossSize.Small;
                    //bossTimeSettings = BossTimeSettings.Late;
                    aiEconomyLevel = 1;
                    resourceMultiplyChance = 0.25;
                    resourceMultiplyDecrease = true;
                    diplomacyDifficulty = 0;
                    honorGuard = true;
                    resourcesStartHelp = true;
                    //toPeacefulCheck = false;
                    aiDelayTimeSec = 15 * TimeExt.MinuteInSeconds;
                    //toPeacefulPercentage = 0.05f;
                    PlayerBonusGold = 4000;
                    break;

                case 2:
                    aiAggressivity = AiAggressivity.Low;
                    bossSize = BossSize.Medium;
                    //bossTimeSettings = BossTimeSettings.Late;
                    aiEconomyLevel = 1;
                    resourceMultiplyChance = 0.25;
                    resourceMultiplyDecrease = true;
                    diplomacyDifficulty = 1;
                    honorGuard = true;
                    resourcesStartHelp = true;
                    //toPeacefulCheck = true;
                    aiDelayTimeSec = 8 * TimeExt.MinuteInSeconds;
                    toPeacefulPercentage = 0.1f;
                    PlayerBonusGold = 2000;
                    break;

                case 3: //Medium
                    aiAggressivity = AiAggressivity.Medium;
                    bossSize = BossSize.Medium;
                    //bossTimeSettings = BossTimeSettings.Normal;
                    aiEconomyLevel = 2;
                    diplomacyDifficulty = 1;
                    honorGuard = true;
                    //toPeacefulCheck = true;
                    aiDelayTimeSec = 30;
                    toPeacefulPercentage = 0.2f;
                    break;

                case 4:
                    aiAggressivity = AiAggressivity.Medium;
                    bossSize = BossSize.Medium;
                    //bossTimeSettings = BossTimeSettings.Normal;
                    aiEconomyLevel = 2;
                    diplomacyDifficulty = 1;
                    honorGuard = false;
                    //toPeacefulCheck = true;
                    aiDelayTimeSec = 10;
                    toPeacefulPercentage = 0.5f;
                    break;

                case 5:
                    aiAggressivity = AiAggressivity.Medium;
                    bossSize = BossSize.Large;
                    //bossTimeSettings = BossTimeSettings.Early;
                    aiEconomyLevel = 2;
                    diplomacyDifficulty = 1;
                    honorGuard = false;
                    //toPeacefulCheck = true;
                    toPeacefulPercentage = 0.75f;
                    break;

                case 6:
                    aiAggressivity = AiAggressivity.High;
                    bossSize = BossSize.Huge;
                    //bossTimeSettings = BossTimeSettings.Early;
                    aiEconomyLevel = 3;

                    resourceMultiplyChance = 0.25;
                    resourceMultiplyDecrease = false;
                    diplomacyDifficulty = 2;
                    honorGuard = false;
                    //toPeacefulCheck = true;
                    toPeacefulPercentage = 1.5f;
                    break;

                case 7: //Max
                    aiAggressivity = AiAggressivity.High;
                    bossSize = BossSize.Huge;
                    //bossTimeSettings = BossTimeSettings.Immediate;
                    aiEconomyLevel = 4;


                    resourceMultiplyChance = 0.5;
                    resourceMultiplyDecrease = false;
                    diplomacyDifficulty = 2;
                    honorGuard = false;
                    //toPeacefulCheck = true;
                    toPeacefulPercentage = 2f;
                    break;
            }

            int mediumOffset = difficulty - 3;

            switch (setting_gameMode)
            {
                case GameModeMainType.FullStory:
                    runStory = true;
                    peaceful = false;
                    break;
                case GameModeMainType.Sandbox:
                case GameModeMainType.Spectator:
                    runStory = false;
                    peaceful = false;
                    break;
                case GameModeMainType.Peaceful:
                    runStory = false;
                    peaceful = true;
                    toPeacefulPercentage = 0;
                    //toPeacefulCheck = false;
                    break;
            }

            MercenaryPurchaseCost_Start = 3500 + mediumOffset * 500;
            MercenaryPurchaseCost_Add = 100 + mediumOffset * 20;

            aiEconomyMultiplier = AiEconomyLevel[aiEconomyLevel] / 100.0;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(setting_allowPauseCommand);
            //w.Write(boss);
            w.Write((byte)setting_gameMode);
            w.Write(setting_foodMulti);
            w.Write(setting_waterMulti);
            w.Write(setting_childMulti);
            w.Write(setting_craftMulti);
            w.Write(difficulty);
        }

        public void read(System.IO.BinaryReader r, int storageversion)
        {
            setting_allowPauseCommand = r.ReadBoolean();
            if (storageversion < 20)
            {
                runStory = r.ReadBoolean();
                if (!runStory)
                {
                    setting_gameMode = GameModeMainType.Sandbox;
                }
            }
            else
            {
                //NEW
                setting_gameMode = (GameModeMainType)r.ReadByte();
                setting_foodMulti = r.ReadSingle();
                if (storageversion >= 24)
                {
                    setting_waterMulti = r.ReadSingle();
                }
                if (storageversion >= 25)
                {
                    setting_childMulti = r.ReadSingle();
                    setting_craftMulti = r.ReadSingle();
                }
            }
            difficulty = r.ReadInt32();

            refreshSettings();
        }

    }


    enum GameModeMainType
    { 
        FullStory,
        Sandbox,
        Peaceful,
        Spectator,
        NUM
    }
    //enum AiResourceMultiplyType
    //{ 
    //    None,
    //    Add,
    //    Remove,
    //}
}
