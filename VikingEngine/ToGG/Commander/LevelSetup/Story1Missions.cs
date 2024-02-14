using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;
using VikingEngine.ToGG.Commander.UnitsData;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.LevelSetup
{
    class Story1Missions
    {
        public const string FileName = "S1V1";

        const int LightUnitsSpawn = 1;
        const int CavalrySpawn = 2;
        const int HeavyUnitsSpawn = 3;
        const int BallistaSpawn = 4;

        public static readonly LevelEnum[] Levels = new LevelEnum[]
        {
            LevelEnum.Story1Practice1,
            LevelEnum.Story1Practice2,
            //Practice 3 med escort mission
            LevelEnum.Story1Overruned,
            LevelEnum.Story1CitySupport,
            LevelEnum.Story1BreakSiege,
            LevelEnum.Story1SaveTheEngineer,
            LevelEnum.Story1TheGate,
            LevelEnum.Story1Skirmish,
            LevelEnum.Story1StormTheCastle,
            LevelEnum.Story1Boss,
        };

        public GameSetup levelSetup(LevelEnum lvl)
        {
            GameSetup setup = new GameSetup();
            setup.lobbyMembers = new List<AbsLobbyMember>
            {
                new LocalLobbyMember(0),
                new AiLobbyMember(),
            };

            setup.level = lvl;
            setup.useStrategyCards = true;
            setup.AiMod_SuperAggressive = true;
            setup.AiMod_TerrainIgnorant = true;
            setup.category = GameCategory.Story1;
                    
            switch (lvl)
            {
                case LevelEnum.Story1Practice1:
                    {
                        setup.loadMap = "Story1Prologue1";
                        setup.WinningConditions = new WinningCondition_Prologue1(setup);
                        setup.missionName = "Prologue";
                    }
                    break;
                case LevelEnum.Story1Overruned:
                    {
                        setup.loadMap = "Story1Mission1";
                        setup.WinningConditions = new WinningCondition_Mission1(6, setup);
                        setup.missionName = "Mission 1";
                    }
                    break;

                case LevelEnum.Story1CitySupport:
                    {
                        setup.loadMap = "Story1Mission2";
                        setup.WinningConditions = new WinningCondition_Mission2(setup);
                        setup.missionName = "Mission 2";
                    }
                    break;

                case LevelEnum.Story1BreakSiege:
                    {
                        List<StoryArmyMember> reinforce = new List<StoryArmyMember>
                        {
                            new StoryArmyMember(UnitType.Human_Spearman, 1, LightUnitsSpawn),
                            new StoryArmyMember(UnitType.Human_LongBow, 2, LightUnitsSpawn),
                            new StoryArmyMember(UnitType.Human_HeavyCavalry, 1, CavalrySpawn),
                        };
                        toggRef.storage.activeStoryProgress.add(reinforce);

                        setup.loadMap = "Story1Mission3";
                        setup.WinningConditions = new WinningCondition_Mission3();
                        setup.missionName = "Mission 3";
                    }
                    break;
                case LevelEnum.Story1SaveTheEngineer:
                    {
                        setup.loadMap = "Story1Mission4";
                        setup.WinningConditions = new WinningCondition_Mission4();
                        setup.missionName = "Mission 4";
                    }
                    break;
                case LevelEnum.Story1TheGate:
                    {
                        List<StoryArmyMember> reinforce = new List<StoryArmyMember>
                        {
                            new StoryArmyMember(UnitType.Story1_Catapult, 3, BallistaSpawn),
                        };
                        toggRef.storage.activeStoryProgress.add(reinforce);

                        setup.AiMod_BannerLowPrio = true;
                        setup.loadMap = "Story1Mission5";
                        setup.WinningConditions = new WinningCondition_Mission5(setup);
                        setup.missionName = "Mission 5";
                    }
                    break;
                case LevelEnum.Story1Skirmish:
                    {
                        toggRef.storage.activeStoryProgress.setSpawnStatus(BallistaSpawn, false);
                        List<StoryArmyMember> reinforce = new List<StoryArmyMember>
                        {
                            new StoryArmyMember(UnitType.Human_Warrior, 3, LightUnitsSpawn),
                        };
                        toggRef.storage.activeStoryProgress.add(reinforce);

                        setup.AiMod_BannerLowPrio = true;
                        setup.loadMap = "Story1Mission6";
                        setup.WinningConditions = new WinningCondition_Mission6();
                        setup.missionName = "Mission 6";
                    }
                    break;
                case LevelEnum.Story1StormTheCastle:
                    {
                        toggRef.storage.activeStoryProgress.setSpawnStatus(BallistaSpawn, true);
                        setup.loadMap = "Story1Mission7";
                        setup.WinningConditions = new WinningCondition_Mission7(setup);
                        setup.missionName = "Mission 7";
                        setup.armyScale = ArmyScale.Double;
                    }
                    break;
                case LevelEnum.Story1Boss:
                    {
                        toggRef.storage.activeStoryProgress.setSpawnStatus(BallistaSpawn, false);

                        { //Refill so the army wont be too small
                            const int MinLightUnits = 10;
                            const int MinCavalryUnits = 2;

                            int lightCount = toggRef.storage.activeStoryProgress.SpawnCount(LightUnitsSpawn);
                            int cavalryCount = toggRef.storage.activeStoryProgress.SpawnCount(CavalrySpawn);

                            if (lightCount < MinLightUnits)
                            {
                                List<StoryArmyMember> reinforce = new List<StoryArmyMember>
                                {
                                    new StoryArmyMember(UnitType.Elf_LongBow, MinLightUnits - lightCount, LightUnitsSpawn),
                                };
                                toggRef.storage.activeStoryProgress.add(reinforce);
                            }

                            if (cavalryCount < MinCavalryUnits)
                            {
                                List<StoryArmyMember> reinforce = new List<StoryArmyMember>
                                {
                                    new StoryArmyMember(UnitType.Elf_HeavyCavalry, MinCavalryUnits - cavalryCount, CavalrySpawn),
                                };
                                toggRef.storage.activeStoryProgress.add(reinforce);
                            }
                        }

                        setup.loadMap = "Story1Mission8";
                        setup.WinningConditions = new WinningCondition_FinalBoss();
                        setup.missionName = "Final Boss";
                    }
                    break;
            }

            setup.missionDescription = setup.WinningConditions.Description();
            return setup;
        }

        public void ListMissions()
        {
            int currentLevel = toggRef.storage.activeStoryStorage.currentLevel(Levels);

            LevelEnum lvl = Levels[currentLevel];

            string text;
            string name = LevelName(lvl, out text);

            GuiLayout layout = new GuiLayout(name, toggRef.menu.menu);
            {

                new GuiLabel(text, true, layout.gui.style.textFormat, layout);
                new GuiTextButton("Begin", null, new GuiAction1Arg<LevelEnum>(startMission, lvl), false, layout);

                new GuiSectionSeparator(layout);
                if (currentLevel > 0)
                {
                    new GuiDialogButton("Start New", "Remove the current progress", new GuiAction1Arg<LevelEnum>(startMission, Levels[0]), false, layout);
                }
                new GuiTextButton("Back", null, toggRef.menu.menu.PopLayout, false, layout);
                    //for (int i = 0; i < Levels.Length; ++i)
                //{
                //    LevelEnum lvl = Levels[i];

                //    if (i < currentLevel)
                //    {
                //        new GuiIconTextButton(SpriteName.winnerParticle, LevelName(lvl), null, new GuiAction1Arg<LevelEnum>(startMission, lvl), false, layout);
                //    }
                //    else if (i == currentLevel)
                //    {
                //        new GuiLargeTextButton(LevelName(lvl), null, new GuiAction1Arg<LevelEnum>(startMission, lvl), false, layout);
                //    }
                //    else
                //    {
                //        new GuiIconTextButton(SpriteName.birdPlayerFrameClose, "-------", null, new GuiNoAction(), false, layout);
                //    }
                //}

            } layout.End();
        }

        public static string LevelName(LevelEnum lvl, out string flavorText)
        {
            switch (lvl)
            {
                case LevelEnum.Story1Practice1:
                    flavorText = "In a small outpost, you hold a friendly practice fight.";
                    return "Prologue part 1";

                case LevelEnum.Story1Practice2:
                    flavorText = "Day two of practice.";
                    return "Prologue part 2";

                case LevelEnum.Story1Overruned:
                    flavorText = "You are overrun by the enemy army, flee!";
                    return "1. Flee to the city";

                case LevelEnum.Story1CitySupport:
                    flavorText = "We a have a chance to flank the enemies and take out their battering ram.";
                    return "2. City gate";

                case LevelEnum.Story1BreakSiege:
                    flavorText = "The city is now under siege, lets strike just before dawn and take out their supplies!";
                    return "3. Night raid";

                case LevelEnum.Story1SaveTheEngineer:
                    flavorText = "It is time to strike back! We need an engineer that can build war mashines.";
                    return "4. Save the engineer";

                case LevelEnum.Story1TheGate:
                    flavorText = "Our path is blocked by a gate in a narrow pass.";
                    return "5. Blow the gate";

                case LevelEnum.Story1Skirmish:
                    flavorText = "The enemies has sent out a small troup to take out our supplies.";
                    return "6. Skirmish";

                case LevelEnum.Story1StormTheCastle:
                    flavorText = "The elf princess has sent a request to ally in our siege. We are finally here, this is the day we get to vengance our dead kin!";
                    return "7. Storm the castle";

                case LevelEnum.Story1Boss:
                    flavorText = "The necromancer have finally stepped out of the shadows.";
                    return "8. Boss fight!";

                default:
                    throw new NotImplementedException();
            }
        }

        public void startMission(LevelEnum level)
        {
            PlayerStoryProgress progress;

            if (toggRef.storage.activeStoryStorage == null)
            {
                toggRef.storage.activeStoryStorage = new Storage.StoryStorage("err");
            }

            if (level == Levels[0])
            {
                toggRef.storage.activeStoryStorage.clear();
            }

            if (toggRef.storage.activeStoryStorage.progressPoints.TryGetValue(level, out progress) == false)
            {
                List<StoryArmyMember> armysetup = new List<StoryArmyMember>
                {
                    new StoryArmyMember(UnitType.Human_Spearman, 6, LightUnitsSpawn),
                    new StoryArmyMember(UnitType.Human_ShortBow, 3, LightUnitsSpawn),
                    new StoryArmyMember(UnitType.Human_Cavalry, 2, CavalrySpawn),
                    new StoryArmyMember(UnitType.Human_HeavySpearman, 1, HeavyUnitsSpawn),
                    new StoryArmyMember(UnitType.Story1_DrillSergeant, 1, HeavyUnitsSpawn),
                };

                progress = new PlayerStoryProgress(armysetup);
            }

            toggRef.storage.activeStoryProgress = progress.clone();
            GameSetup game = levelSetup(level);

            game.useProgressSpawning = true;

            new Commander.CmdPlayState(game);
        }
    }

    class WinningCondition_Prologue1 : AbsWinningCondition
    {
        const int WinnerScore = 8;

        public WinningCondition_Prologue1(GameSetup setup)
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
            return "In this practice fight you need to reach " + WinnerScore.ToString() + " Victory Points, before your opponent do.";
        }
    }

    class WinningCondition_Prologue2 : AbsWinningCondition
    {
        const int TimeLimit = 6;
        List<AbsUnit> supplies;
        public WinningCondition_Prologue2(GameSetup setup)
        {
            this.timeLimit = TimeLimit;
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (player.IsScenarioFriendly)
            {

                bool holdingBanners = true;
                foreach (var m in supplies)
                {
                    if (m.Alive)
                    {
                        holdingBanners = false;
                        break;
                    }
                }

                if (holdingBanners)
                {
                    return EndGameResult.MissionSuccess;
                }
                if (player.TurnsCount >= timeLimit)
                {
                    return EndGameResult.MissionFailed;
                }
            }
            
            return EndGameResult.non;
        }

        public override string Description()
        {
            return "Hold both banners within " + TimeLimit.ToString() + " turns.";
        }
    }

    class WinningCondition_Mission1 : AbsWinningCondition
    {
        int MaxOpponentVP;
        bool suppliesDestroyed = false;

        AbsUnit supplyWagon;

        public WinningCondition_Mission1(int MaxOpponentVP, GameSetup setup)
        {
            this.MaxOpponentVP = MaxOpponentVP;
            setup.lobbyMembers[1].playersetup.collectingVP = MaxOpponentVP;
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (suppliesDestroyed)
            {
                return EndGameResult.MissionFailed;
            }

            if (player is Commander.Players.LocalPlayer)
            {
                if (supplyWagon.isDeleted)
                {
                    return EndGameResult.MissionSuccess;
                }
            }
            else
            {
                if (player.VictoryPoints >= MaxOpponentVP)
                {//Game ends
                    return EndGameResult.MissionFailed;
                }
            }
            return EndGameResult.non;
        }

        

        public override void gameStartSetup()
        {
            supplyWagon = Commander.cmdRef.players.ActiveLocalPlayer().GetUnits(UnitUnderType.Special_Supplies)[0];

            AiModifier_TargetUnit targetWagon = new AiModifier_TargetUnit(supplyWagon);
            AiModifier_TargetUnit targetLeader = new AiModifier_TargetUnit(Commander.cmdRef.players.localHost.GetUnits(UnitPropertyType.Leader)[0]);

            toggRef.gamestate.gameSetup.aiTargetUnits.Add(targetWagon);
            toggRef.gamestate.gameSetup.aiTargetUnits.Add(targetLeader);
        }

        public override void onUnitDestroyed(AbsUnit u)
        {
            if (u.cmd().data.underType == UnitUnderType.Special_Supplies)
            {
                suppliesDestroyed = true;
            }
        }

        public override string Description()
        {
            return "Escape with the supplies. " + MaxOpponentVPDesc(MaxOpponentVP) + " " + NoDeadLeader;
        }
    }

    class WinningCondition_Mission2 : AbsWinningCondition
    { //Stop the Battering Ram
        const int MaxOpponentVP = 8;
        bool battleRamDestroyed = false;
        bool leaderKilled = false;
        AiModifier_MoveUnit moveBattleRam1, moveBattleRam2;

        public WinningCondition_Mission2(GameSetup setup)
        {
            setup.lobbyMembers[1].playersetup.collectingVP = MaxOpponentVP;
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (player.IsScenarioFriendly)
            {
                if (battleRamDestroyed)
                {
                    return EndGameResult.MissionSuccess;
                }
            }
            else
            {
                if (leaderKilled)
                {
                    return EndGameResult.MissionFailed;
                }

                if (moveBattleRam1.OnGoal())
                {
                    toggRef.gamestate.gameSetup.aiMoveUnits.Remove(moveBattleRam1);
                    toggRef.gamestate.gameSetup.aiMoveUnits.Add(moveBattleRam2);
                }
                else if (moveBattleRam2.OnGoal())
                {
                    return EndGameResult.MissionFailed;
                }

                if (player.VictoryPoints >= MaxOpponentVP)
                {//Game ends
                    return EndGameResult.MissionFailed;
                }
            }
            return EndGameResult.non;
        }

        public override void gameStartSetup()
        {
            var aiPlayer = Commander.cmdRef.players.allPlayers.list[1];

            AiModifier_TargetUnit targetLeader = new AiModifier_TargetUnit(Commander.cmdRef.players.localHost.GetUnits(UnitPropertyType.Leader)[0]);

            AbsUnit ram = aiPlayer.GetUnits(UnitUnderType.Warmashine_BatteringRam)[0];

            moveBattleRam1 = new AiModifier_MoveUnit(
                ram, toggRef.board.GetTag(1)[0]);
            
            moveBattleRam2 = new AiModifier_MoveUnit(
                ram, toggRef.board.GetTag(2)[0]);

            toggRef.gamestate.gameSetup.aiTargetUnits.Add(targetLeader);
            toggRef.gamestate.gameSetup.aiMoveUnits.Add(moveBattleRam1);
        }

        public override void onUnitDestroyed(AbsUnit u)
        {
            if (u.Player.IsScenarioFriendly)
            {
                if (u.HasProperty(UnitPropertyType.Leader))
                {
                    leaderKilled = true;
                }
            }
            else
            {
                if (u.cmd().data.underType == UnitUnderType.Warmashine_BatteringRam)
                {
                    battleRamDestroyed = true;
                }
            }
        }

        public override string Description()
        {
            return "Destroy the battering ram before it reaches the gate. " + MaxOpponentVPDesc(MaxOpponentVP) + " " + NoDeadLeader;
        }
    }

    class WinningCondition_Mission3 : AbsWinningCondition
    { //Break the siege
        const int TimeLimit = 9;

        List<AbsUnit> sleepingTents;
        List<AbsUnit> supplies;
        bool leaderKilled = false;
        
        public WinningCondition_Mission3()
        {
            this.timeLimit = TimeLimit;
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (player.IsScenarioFriendly)
            {

                bool destroyedSupplies = true;
                foreach (var m in supplies)
                {
                    if (m.Alive)
                    {
                        destroyedSupplies = false;
                        break;
                    }
                }

                if (destroyedSupplies)
                {
                    return EndGameResult.MissionSuccess;
                }
                if (player.TurnsCount >= timeLimit)
                {
                    return EndGameResult.MissionFailed;
                }
            }
            else
            {
               
                if (leaderKilled)
                {
                    return EndGameResult.MissionFailed;
                }
            }
            return EndGameResult.non;
        }

        public override void onEndTurn(Commander.Players.AbsCmdPlayer player)
        {
            base.onEndTurn(player);

            if (!player.IsScenarioFriendly)
            {
                if (player.TurnsCount > 0)
                {
                    spawnUnitFromTent();
                }
            }
        }

        void spawnUnitFromTent()
        {
            AbsUnit tent = arraylib.RandomListMember(sleepingTents);

            if (tent.Alive)
            {
                List<IntVector2> available = new List<IntVector2>(8);

                foreach (var dir in IntVector2.Dir8Array)
                {
                    IntVector2 pos = dir + tent.squarePos;
                    var sq = toggRef.board.tileGrid.Get(pos);
                    if (sq.unit == null)
                    {
                        available.Add(pos);
                    }
                }

                if (available.Count > 0)
                {
                    IntVector2 spawnPos = arraylib.RandomListMember(available);

                    var unitSpawnTypes = new RandomObjects<UnitType>(
                        new ObjectCommonessPair<UnitType>(UnitType.Undead_SpearBeast, 10),
                        new ObjectCommonessPair<UnitType>(UnitType.Undead_Warrior, 5),
                        new ObjectCommonessPair<UnitType>(UnitType.Undead_HeavyBeast, 2),
                        new ObjectCommonessPair<UnitType>(UnitType.Orc_ShortBow, 8));

                    var aiPlayer = Commander.cmdRef.players.allPlayers.list[1];
                    var spawnType = unitSpawnTypes.GetRandom();
                    var u = new Commander.GO.Unit(spawnPos, spawnType, aiPlayer);
                    u.SetVisualPosition(tent.squarePos);
                    u.SlideToSquare(spawnPos, true);
                }
            }
        }

        public override void onUnitDestroyed(AbsUnit u)
        {
            if (u.Player.IsScenarioFriendly)
            {
                if (u.HasProperty(UnitPropertyType.Leader))
                {
                    leaderKilled = true;
                }
            }
        }

        public override void gameStartSetup()
        {
            var aiPlayer = Commander.cmdRef.players.allPlayers.list[1];
            sleepingTents = aiPlayer.GetUnits(UnitType.SleepingTent);
            supplies = aiPlayer.GetUnits(UnitType.SupplyWagon);
        }
        

        public override string Description()
        {
            return "Destroy the supplies within " + TimeLimit.ToString() + " turns. " + NoDeadLeader + " A new oppnent will walk out of a tent, at the end of each turn.";
        }
    }

    class WinningCondition_Mission4 : AbsWinningCondition
    {//Save the engineer
        bool leaderKilled = false;
        bool engineerkilled = false;

        public override void gameStartSetup()
        {
            var aiPlayer = Commander.cmdRef.players.allPlayers.list[1];

            AiModifier_TargetUnit targetLeader = new AiModifier_TargetUnit(Commander.cmdRef.players.localHost.GetUnits(UnitPropertyType.Leader)[0]);
            AiModifier_TargetUnit targetEngineer = new AiModifier_TargetUnit(Commander.cmdRef.players.localHost.GetUnits(UnitType.Story1_Engineer)[0]);

            toggRef.gamestate.gameSetup.aiTargetUnits.Add(targetLeader);
            toggRef.gamestate.gameSetup.aiTargetUnits.Add(targetEngineer);
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (player.IsScenarioFriendly)
            {
                var units =  Commander.cmdRef.players.CollectEnemyUnits(player.pData.globalPlayerIndex);
                if (units.array.Count == 0)
                {
                    return EndGameResult.MissionSuccess;
                }
            }
            else
            {
                if (leaderKilled || engineerkilled)
                {
                    return EndGameResult.MissionFailed;
                }

            }
            return EndGameResult.non;
        }

        public override void onUnitDestroyed(AbsUnit u)
        {
            if (u.Player.IsScenarioFriendly)
            {
                if (u.HasProperty(UnitPropertyType.Leader))
                {
                    leaderKilled = true;
                }
                else if (u.cmd().data.Type == UnitType.Story1_Engineer)
                {
                    engineerkilled = true;
                }
            }
        }

        public override string Description()
        {
            return "Destroy all opponents and save the ballista engineer. " + NoDeadLeader;
        }
    }

    class WinningCondition_Mission5 : AbsWinningCondition
    {
        const int MaxOpponentVP = 18;

        bool valuablaUnitKilled = false;

        public WinningCondition_Mission5(GameSetup setup)
        {
            setup.lobbyMembers[1].playersetup.collectingVP = MaxOpponentVP;
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (player.IsScenarioFriendly)
            {
                if (playerHasAllFlags(player))
                {
                    return EndGameResult.MissionSuccess;
                }
            }
            else
            { //opponent
                if (valuablaUnitKilled)
                {
                    return EndGameResult.MissionFailed;
                }

                if (player.VictoryPoints >= MaxOpponentVP)
                {
                    return EndGameResult.MissionFailed;
                }
            }

            return EndGameResult.non;
        }

        public override void onUnitDestroyed(AbsUnit u)
        {
            if (u.Player.IsScenarioFriendly)
            {
                if (u.HasProperty(UnitPropertyType.Valuable))
                {
                    valuablaUnitKilled = true;
                }
            }
        }

        

        public override string Description()
        {
            return HoldTheBanner + MaxOpponentVPDesc(MaxOpponentVP) + " " + NoDeadValuable;
        }
    }

    class WinningCondition_Mission6 : AbsWinningCondition
    {
        int suppliesDestroyed = 0;

        List<AbsUnit> supplyWagons;

        public WinningCondition_Mission6()
        {
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (suppliesDestroyed >= supplyWagons.Count)
            {
                return EndGameResult.MissionFailed;
            }

            if (player.IsScenarioFriendly)
            {
                foreach (var m in player.escapedUnits)
                {
                    if (m.cmd().data.Type == UnitType.SupplyWagon)
                    {
                        return EndGameResult.MissionSuccess;
                    }
                }
            }
            //else
            //{
            //    if (player.VictoryPoints >= MaxOpponentVP)
            //    {//Game ends
            //        return EndGame.MissionFailed;
            //    }
            //}
            return EndGameResult.non;
        }

        public override void gameStartSetup()
        {
            supplyWagons = Commander.cmdRef.players.ActiveLocalPlayer().GetUnits(UnitUnderType.Special_Supplies);

            foreach (var m in supplyWagons)
            {
                AiModifier_TargetUnit targetWagon = new AiModifier_TargetUnit(m);

                toggRef.gamestate.gameSetup.aiTargetUnits.Add(targetWagon);
            }
        }

        public override void onUnitDestroyed(AbsUnit u)
        {
            if (u.cmd().data.underType == UnitUnderType.Special_Supplies)
            {
                suppliesDestroyed++;
            }
        }

        public override string Description()
        {
            return "Escape with at least one supply wagon. " + NoDeadLeader;
        }
    }

    class WinningCondition_Mission7 : AbsWinningCondition
    {
        const int MaxOpponentVP = 18;

        //bool valuablaUnitKilled = false;

        public WinningCondition_Mission7(GameSetup setup)
        {
            setup.lobbyMembers[1].playersetup.collectingVP = MaxOpponentVP;
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (player.IsScenarioFriendly)
            {
                if (playerHasAllFlags(player))
                {
                    return EndGameResult.MissionSuccess;
                }
            }
            else
            { //opponent
                //if (valuablaUnitKilled)
                //{
                //    return EndGame.MissionFailed;
                //}

                if (player.VictoryPoints >= MaxOpponentVP)
                {
                    return EndGameResult.MissionFailed;
                }
            }

            return EndGameResult.non;
        }

        public override void onUnitDestroyed(AbsUnit u)
        {
            //if (u.Player.IsScenarioFriendly)
            //{
            //    if (u.HasProperty(UnitPropertyType.Valuable))
            //    {
            //        valuablaUnitKilled = true;
            //    }
            //}
        }

        public override string Description()
        {
            return HoldTheBanner + MaxOpponentVPDesc(MaxOpponentVP);
        }
    }

    class WinningCondition_FinalBoss : AbsWinningCondition
    {
        bool bossKilled = false;

        public WinningCondition_FinalBoss()
        { }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (bossKilled)
            {
                 return EndGameResult.MissionSuccess;
            }

            return EndGameResult.non;
        }

        public override void onUnitDestroyed(AbsUnit u)
        {
            if (u.cmd().data.Type == UnitType.Story1_NecroDragon)
            {
                bossKilled = true;
            }
        }

        public override string Description()
        {
            return "Destroy the necromancer";
        }
    }
}
