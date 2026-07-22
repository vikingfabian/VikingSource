using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VikingEngine.DataStream;
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

        static readonly int[] options = new int[] { 25, 50, 75, 100, 125, 150, 175, 200, 300 };
        public static readonly int[] AiEconomyLevel = new int[] { 50, 75, 100, 125, 150, 300 };
        
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
        

        public int setting_QuickMatch_PlayerCount = 4;
        public bool setting_QuickMatch_TwoTeams = false;

       
        

        public const GameModeMainType DefaultMode = GameModeMainType.FullStory;
        public GameModeMainType setting_gameMode = DefaultMode;
        public bool runStory = true;
        public bool peaceful = false;
        public bool extremeAggression = false;
        //public bool toPeacefulCheck = true;

        public int MercenaryPurchaseCost_Start;
        public int MercenaryPurchaseCost_Add;
        public float tooPeacefulPercentage = 0;

        public double resourceMultiplyChance = 0;
        public bool resourceMultiplyDecrease;
        
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
                    string.Format( string.Format(DssRef.todoLang.Language_LabelAndText_Colon, DssRef.todoLang.DifficultyDescription_BotAggression) /*DssRef.lang.DifficultyDescription_AiAggression*/, TextLib.IndexDivition((int)difficultyLvl.aiAggressivity, (int)AiAggressivity.NUM)) + Environment.NewLine +
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

        public bool GodPowers()
        { 
            return setting_gameMode == GameModeMainType.Spectator;
        }

        public static bool ModeSupportsTutorial(GameModeMainType gameMode, FactionStartSize startSize)
        {
            if (startSize == FactionStartSize.Settler)
            {
                return false;
            }

            switch (gameMode)
            {
                case GameModeMainType.Spectator:
                case GameModeMainType.QuickMatch:
                    return false;

                default: return true;
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
                content.Add(new RbText(string.Format(DssRef.todoLang.Language_LabelAndText_Colon, DssRef.todoLang.DifficultyDescription_BotAggression, TextLib.IndexDivition((int)difficultyLvl.aiAggressivity, (int)AiAggressivity.NUM))));
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
            if (difficultyLvl.extremeAggression)
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.WarsRelationTotalWar));
                content.hspace();
                content.Add(new RbText(DssRef.lang.DifficultyDescription_ExtremeAggression));
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
                result += 50;

            }
            if (!DssRef.storage.ruleset.centralGold)
            {
                result += 25;
            }
            switch (setting_gameMode)
            {
                case GameModeMainType.QuickBoss:
                    //result += 25;
                    result += GameRuleset.QuickBossOptions_Time_Difficulty[DssRef.storage.ruleset.QuickBossTimeOption].Value2;
                    break;
                case GameModeMainType.FullStory:
                    result += 50;
                    break;
                case GameModeMainType.Peaceful:
                    result *= 0.5;
                    break;
            }

            return Convert.ToInt32(result);
        }

        public bool AllyCountCost()
        {
            return diplomacyDifficulty > 0;
        }
        public bool UseTruceFailure(out float failChance)
        {
            if (diplomacyDifficulty > 0)
            {
                failChance = 0.15f + 0.05f * diplomacyDifficulty;
                return true;
            }
            failChance = 0;
            return false;
        }

        public void refreshSettings()
        {
            //FoodEnergySett = Convert.ToInt32(DssConst.FoodEnergy * setting_foodMulti);

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
                    aiDelayTimeSec = 60 * TimeExt.MinuteInSeconds;
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
                    aiDelayTimeSec = 30 * TimeExt.MinuteInSeconds;
                    PlayerBonusGold = 4000;
                    break;

                case 2://defalut 75%
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
                    aiDelayTimeSec = 20 * TimeExt.MinuteInSeconds;
                    tooPeacefulPercentage = 0.1f;
                    PlayerBonusGold = 2000;
                    break;

                case 3: //Medium
                    aiAggressivity = AiAggressivity.Medium;
                    bossSize = BossSize.Medium;
                    //bossTimeSettings = BossTimeSettings.Normal;
                    aiEconomyLevel = 2;
                    diplomacyDifficulty = 1;
                    honorGuard = false;
                    //toPeacefulCheck = true;
                    aiDelayTimeSec = 30;
                    tooPeacefulPercentage = 0.2f;
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
                    tooPeacefulPercentage = 0.4f;
                    break;

                case 5:
                    aiAggressivity = AiAggressivity.Medium;
                    bossSize = BossSize.Large;
                    //bossTimeSettings = BossTimeSettings.Early;
                    aiEconomyLevel = 2;
                    diplomacyDifficulty = 1;
                    honorGuard = false;
                    aiDelayTimeSec = 0;
                    //toPeacefulCheck = true;
                    tooPeacefulPercentage = 0.7f;
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
                    aiDelayTimeSec = 0;
                    //toPeacefulCheck = true;
                    tooPeacefulPercentage = 2f;
                    break;

                case 7: //200%
                    aiAggressivity = AiAggressivity.High;
                    bossSize = BossSize.Huge;
                    aiEconomyLevel = 4;


                    resourceMultiplyChance = 0.5;
                    resourceMultiplyDecrease = false;
                    diplomacyDifficulty = 2;
                    honorGuard = false;
                    aiDelayTimeSec = 0;
                    tooPeacefulPercentage = 5f;
                    break;

                case 8: // 300%
                    aiAggressivity = AiAggressivity.High;
                    bossSize = BossSize.Huge;
                    aiEconomyLevel = 5;

                    resourceMultiplyChance = 0.5;
                    resourceMultiplyDecrease = false;
                    diplomacyDifficulty = 2;
                    honorGuard = false;
                    aiDelayTimeSec = 0;
                    tooPeacefulPercentage = 10f;
                    extremeAggression = true;
                    break;
            }

            int mediumOffset = difficulty - 3;

            switch (setting_gameMode)
            {
                case GameModeMainType.QuickBoss:
                case GameModeMainType.FullStory:
                    runStory = true;
                    peaceful = false;
                    break;
                case GameModeMainType.QuickMatch:
                    runStory = false;
                    peaceful = false;
                    tooPeacefulPercentage = 0;
                    break;
                case GameModeMainType.Sandbox:
                case GameModeMainType.Spectator:
                    runStory = false;
                    peaceful = false;
                    break;
                case GameModeMainType.Peaceful:
                    runStory = false;
                    peaceful = true;
                    tooPeacefulPercentage = 0;
                    //toPeacefulCheck = false;
                    break;
            }

            MercenaryPurchaseCost_Start = 3500 + mediumOffset * 500;
            MercenaryPurchaseCost_Add = 100 + mediumOffset * 20;

            aiEconomyMultiplier = AiEconomyLevel[aiEconomyLevel] / 100.0;

           
        }

        public int honorGuardCount()
        {
            if (honorGuard)
            {
                switch (DssRef.storage.ruleset.factionStartSize)
                {
                    case FactionStartSize.Full:
                        return 12;
                    case FactionStartSize.OneCity:
                        return 4;
                    case FactionStartSize.Settler:
                        return 2;
                }

            }

            return 0;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(setting_allowPauseCommand);
            //w.Write(boss);
            w.Write((byte)setting_gameMode);
            //w.Write(setting_foodMulti);
            //w.Write(setting_waterMulti);
            //w.Write(setting_childMulti);
            //w.Write(setting_craftMulti);
            //w.Write(setting_techMulti);
            //w.Write(setting_techMulti_QuickMatch);
            w.Write(setting_QuickMatch_PlayerCount);
            w.Write(setting_QuickMatch_TwoTeams);
            w.Write(difficulty);

            Debug.WriteCheck(w);
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

                if (storageversion < 37)
                {
                    DssRef.storage.ruleset.setting_foodMulti = Bound.ResetOffBounds(r.ReadSingle(), 1, GameRuleset.FoodMultiBound);
                    if (storageversion >= 24)
                    {
                        DssRef.storage.ruleset.setting_waterMulti = Bound.ResetOffBounds(r.ReadSingle(), 1, GameRuleset.WaterMultiBound);
                    }
                    if (storageversion >= 25)
                    {
                        DssRef.storage.ruleset.setting_childMulti = Bound.ResetOffBounds(r.ReadSingle(), 1, GameRuleset.ChildMultiBound);
                        DssRef.storage.ruleset.setting_craftMulti = Bound.ResetOffBounds(r.ReadSingle(), 1, GameRuleset.CraftMultiBound);
                    }
                    if (storageversion >= 33)
                    {
                        DssRef.storage.ruleset.setting_techMulti = Bound.ResetOffBounds(r.ReadInt32(), 1, GameRuleset.TechMultiBound);
                        DssRef.storage.ruleset.setting_techMulti_QuickMatch = r.ReadInt32();
                    }
                }
                if (storageversion >= 33)
                {
                    setting_QuickMatch_PlayerCount = r.ReadInt32();
                    setting_QuickMatch_TwoTeams = r.ReadBoolean();
                }
            }
            difficulty = r.ReadInt32();
            Bound.SetToArray(ref difficulty, options.Length);

            if (storageversion >= 32)
            {
                Debug.ReadCheck(r);
            }

            refreshSettings();
        }

        public Difficulty Clone()
        {
            Difficulty clone = new Difficulty();

            MemoryStreamHandler memoryStream = new MemoryStreamHandler();
            var w = memoryStream.GetWriter();
            write(w);

            var r = memoryStream.GetReader();
            clone.read(r, int.MaxValue);

            return clone;
        }

        public int QuickMatchPlayerStartSize()
        {
            int goalWorkForce = MathExt.MultiplyInt( DssConst.HeadCityStartMaxWorkForce, 2.5);
            return goalWorkForce;
        }

    }


    enum GameModeMainType
    { 
        FullStory,
        Sandbox,
        Peaceful,
        Spectator,
        QuickMatch,
        QuickBoss,
        NUM
    }
    //enum AiResourceMultiplyType
    //{ 
    //    None,
    //    Add,
    //    Remove,
    //}
}
