using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;
using Microsoft.Xna.Framework;

namespace VikingEngine.ToGG.Commander.LevelSetup
{
    class ChallengeLevelsData
    {
        public const UnitPropertyType GoodDummyProperty = UnitPropertyType.Unknown3;

        public static readonly LevelEnum[] TutorialLevelsOrder = new LevelEnum[]
        {
            LevelEnum.OrderMoveAttack,
            LevelEnum.UseSpyglass,
            LevelEnum.FirstBattle,

        };
        static readonly LevelEnum[] ChallengeLevelsOrder = new LevelEnum[]
        {
            LevelEnum.SupportChallenge,
            LevelEnum.RestingChallenge,
            LevelEnum.StrategyCardsChallenge,
        };

        public void listTutorials(Action<LevelEnum> startTutorial)
        {
            GuiLayout layout = new GuiLayout("Tutorial Missions", toggRef.menu.menu);
            {
                bool allCompleted = true;

                for (int i = 0; i < TutorialLevelsOrder.Length; ++i)
                {
                    bool completed = toggRef.storage.challengeCompleted[(int)TutorialLevelsOrder[i]];
                    allCompleted = allCompleted && completed;
                    SpriteName icon = completed ? SpriteName.winnerParticle : SpriteName.birdPlayerFrameClose;
                    GameSetup setup = levelSetup(TutorialLevelsOrder[i]);

                    new GuiIconTextButton(icon, TextLib.IndexToString(i) + ". " +  setup.missionName, setup.missionDescription,
                        new GuiAction1Arg<LevelEnum>(startTutorial, TutorialLevelsOrder[i]), false, layout);
                }

                new GuiSectionSeparator(layout);
                new GuiLabel("Extra challenges", layout);
                for (int i = 0; i < ChallengeLevelsOrder.Length; ++i)
                {
                    bool completed = toggRef.storage.challengeCompleted[(int)ChallengeLevelsOrder[i]];

                    IGuiAction link;
                    SpriteName icon;
                    Color textCol;
                    if (allCompleted)
                    {
                        icon = completed ? SpriteName.winnerParticle : SpriteName.birdPlayerFrameClose;
                        textCol = layout.gui.style.textFormat.Color;
                        link = new GuiAction1Arg<LevelEnum>(startTutorial, ChallengeLevelsOrder[i]);
                    }
                    else
                    {
                        icon = SpriteName.birdLock;
                        textCol = Color.Gray;
                        link = null;
                    }
                    GameSetup setup = levelSetup(ChallengeLevelsOrder[i]);

                    var button = new GuiIconTextButton(icon, setup.missionName, setup.missionDescription,
                        link, false, layout);
                    button.text.Color = textCol;
                   
                }
            }
            layout.End();
        }

        public void startMission(LevelEnum level)
        {
            new StartLevelState(levelSetup(level));
        }

        public GameSetup levelSetup(LevelEnum lvl)
        {
            const string DestroyAllDummiesDesc = "Destroy all dummies in one turn.";
            const string DestroyAllDummies2TurnsDesc = "Destroy all dummies in 2 turns.";
            const string CaptureAllFlagsDesc = "Capture all banners in one turn.";

            GameSetup setup = new GameSetup();
            setup.lobbyMembers = new List<AbsLobbyMember>
            {
                new LocalLobbyMember(0),
                new AiLobbyMember(),
            };

            setup.level = lvl;
            setup.passiveOpponent = true;
            setup.category = GameCategory.Tutorial;

            switch (lvl)
            {
                case LevelEnum.OrderMoveAttack:
                    setup.loadMap = "tut1";
                    setup.useStrategyCards = false;
                    setup.WinningConditions = new WinningCondition_DestroyAllMission(1);
                    setup.missionName = "Order, Move & Attack";
                    setup.missionDescription = DestroyAllDummiesDesc;
                    break;
                case LevelEnum.UseSpyglass:
                    setup.loadMap = "tut_spyglass";
                    setup.useStrategyCards = false;
                    setup.WinningConditions = new WinningCondition_DestroyAllEvil(1);
                    setup.missionName = "Spyglass";
                    setup.missionDescription = "Destroy all evil dummies in one turn, save the good one.";
                    break;
                case LevelEnum.FirstBattle:
                    setup.loadMap = "tut_firstbattle";
                    setup.passiveOpponent = false;
                    setup.useStrategyCards = false;
                    setup.WinningConditions = new WinningCondition_FirstBattleTut(setup);
                    setup.missionName = "First battle";
                    setup.missionDescription = setup.WinningConditions.Description();//DestroyAllDummiesDesc;
                    break;
                case LevelEnum.RangeSupport:
                    setup.loadMap = "tut_rangesupport";
                    setup.useStrategyCards = false;
                    setup.WinningConditions = new WinningCondition_DestroyAllMission(1);
                    setup.missionName = "Range support";
                    setup.missionDescription = DestroyAllDummiesDesc;
                    break;

                case LevelEnum.Backstap:
                    setup.loadMap = "tut_avoidbackstab2";
                    setup.useStrategyCards = false;
                    setup.WinningConditions = new WinningCondition_CaptureFlagsAndTakeNoDamage(1);
                    setup.missionName = "Avoid backstabs";
                    setup.missionDescription = CaptureAllFlagsDesc + ". Take no damage.";
                    break;

                //case LevelEnum.MoveAllStrategy:
                //    setup.loadMap = "tut_moveall";
                //    setup.useStrategyCards = true;
                //    setup.availableStrategyCards = new List<CommandCard.CommandType>
                //    {
                //        CommandCard.CommandType.Order_3, 
                //        CommandCard.CommandType.Wide_Advance,  
                //    };
                //    setup.WinningConditions = new WinningCondition_CaptureFlags(1);
                //    setup.missionName = "Strategy cards";
                //    setup.missionDescription = CaptureAllFlagsDesc;
                //    break;
                case LevelEnum.Resting:
                    setup.loadMap = "tut_resting3";
                    setup.useStrategyCards = false;
                    setup.WinningConditions = new WinningCondition_DestroyAllMission(3);
                    setup.missionName = "Resting units";
                    setup.missionDescription = "Destroy all dummies in 3 turns.";

                    break;
                case LevelEnum.SupportChallenge:
                    setup.loadMap = "tut2";
                    setup.useStrategyCards = false;
                    setup.WinningConditions = new WinningCondition_DestroyAllMission(1);
                    setup.missionName = "Attack support";
                    setup.missionDescription = DestroyAllDummiesDesc;
                    break;

                case LevelEnum.RestingChallenge:
                    setup.loadMap = "challenge_restingadv";
                    setup.useStrategyCards = true;
                    setup.WinningConditions = new WinningCondition_DestroyAllMission(3);
                    setup.missionName = "Resting - advanced";
                    setup.missionDescription = "Destroy all dummies in 3 turns.";
                    break;                    

                case LevelEnum.StrategyCardsChallenge:
                    setup.loadMap = "challenge_cards";
                    setup.useStrategyCards = true;
                    setup.WinningConditions = new WinningCondition_DestroyAllMission(2);
                    setup.missionName = "Strategy cards - advanced";
                    setup.missionDescription = DestroyAllDummies2TurnsDesc;
                    break;
            }
            return setup;
        }
    }
    class WinningCondition_FirstBattleTut : AbsWinningCondition
    {
        const int WinnerScore = 6;

        public WinningCondition_FirstBattleTut(GameSetup setup)
        {
            foreach (var m in setup.lobbyMembers)
            {
                m.playersetup.collectingVP = WinnerScore;
            }
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (player.VictoryPoints >= WinnerScore)
            {//Game ends
                if (player.IsScenarioOpponent)
                {
                    return EndGameResult.MissionFailed;
                }
                else
                {
                    return EndGameResult.MissionSuccess;
                }
            }
            return EndGameResult.non;
        }

        public override string Description()
        {
            return "Reach " + WinnerScore.ToString() + " Victory Points, before your opponent do.";
        }
    }
}
