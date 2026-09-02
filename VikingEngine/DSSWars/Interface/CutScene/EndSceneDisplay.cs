using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Event;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Data;
using VikingEngine.PJ;
using VikingEngine.ToGG.HeroQuest.Data.Condition;
using VikingEngine.ToGG.HeroQuest.Display;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.DSSWars.Interface.CutScene
{
    class EndSceneDisplay
    {
        EndSceneLeftDisplayMain left;
        EndSceneCenterDisplayMain center;
        EndSceneRightDisplayMain right;
        public EndSceneDisplay(GameEndReason endReason, VictoryType vType, Action watchEpilogue, MatchResult matchResult)
        { 
            left = new EndSceneLeftDisplayMain();
            center = new EndSceneCenterDisplayMain(endReason, vType, watchEpilogue, matchResult);
            right = new EndSceneRightDisplayMain();

            center.beginMove(0);
        }

        public void update()
        {
            center.updateMove(out _);
            center.update();
        }

        public void DeleteMe()
        {
            left.DeleteMe();
            center.DeleteMe();
            right.DeleteMe();
        }
    }

    class EndSceneLeftDisplayMain : RichboxGui
    {
        EndSceneLeftDisplayPart part;

        public EndSceneLeftDisplayMain()
            : base(HudLib.cutsceneGui, DssRef.state.localPlayers[0].gameControls.input)
        {
            part = new EndSceneLeftDisplayPart(this);

            parts = new List<HUD.RichBox.RichboxGuiPart>()
            {
                part
            };
        }
    }

    class EndSceneLeftDisplayPart : RichboxGuiPart
    {
        public EndSceneLeftDisplayPart(RichboxGui gui)
            : base(gui)
        {
            LangLib.GameModeText(DssRef.difficulty.setting_gameMode, out string caption, out _);
            content.h1(caption, HudLib.TitleColor_Head);
            content.h2(string.Format(DssRef.lang.Settings_TotalDifficulty, DssRef.difficulty.TotalDifficulty()), HudLib.TitleColor_Label);

            content.text(string.Format(DssRef.lang.Settings_DifficultyLevel, DssRef.difficulty.PercDifficulty));

            content.icontext(SpriteName.WarsMapIcon, DssRef.lang.Lobby_MapSizeTitle + ": " + WorldData.SizeString(DssRef.world.metaData.mapSize));
            
            content.icontext(HudLib.CheckImage(DssRef.storage.ruleset_instance.centralGold), DssRef.lang.Settings_CentralGold);
            content.icontext(HudLib.CheckImage(DssRef.difficulty.setting_allowPauseCommand), DssRef.lang.Settings_AllowPause);

            content.icontext(SpriteName.WarsResource_Food, string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Settings_FoodMultiplier, TextLib.OneDecimal(DssRef.storage.ruleset_instance.setting_foodMulti)));
            content.icontext(SpriteName.WarsResource_WaterAdd, string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Settings_WaterMultiplier, TextLib.OneDecimal(DssRef.storage.ruleset_instance.setting_waterMulti)));
            content.icontext(SpriteName.WarsWorker, string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Settings_ChildMultiplier, TextLib.OneDecimal(DssRef.storage.ruleset_instance.setting_childMulti)));
            content.icontext(SpriteName.WarsHammer, string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Settings_CraftMultiplier, TextLib.OneDecimal(DssRef.storage.ruleset_instance.setting_craftMulti)));
            content.icontext(SpriteName.WarsTechnology_Unlocked, string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Settings_TechMultiplier, DssRef.storage.ruleset_instance.TechMultiProperty(null, false, 0)));

            var time = HudLib.TimeSpan_LongText(DssRef.time.TotalIngameTime());
            content.text(string.Format(DssRef.lang.EndGameStatistics_Time, time));

            content.newParagraph();
            content.text(HudLib.Date(DateTime.Now));
            content.text(string.Format(HudLib.EngineVersionString, Engine.LoadContent.EngineVersion));
            
            Vector2 pos = Engine.Screen.SafeArea.CenterTop;
            pos.X -= HudLib.cutsceneGui.width * 1.5f + Engine.Screen.IconSize;
            endRefresh(pos, true);
        }
    }



    class EndSceneCenterDisplayMain: RichboxGui
    {
        EndSceneCenterDisplayPart part;

        public EndSceneCenterDisplayMain(GameEndReason endReason, VictoryType vType, Action watchEpilogue, MatchResult matchResult)
            : base(HudLib.cutsceneGui, DssRef.state.localPlayers[0].gameControls.input)
        {
            part = new EndSceneCenterDisplayPart(endReason, vType, this, watchEpilogue, matchResult);

            parts = new List<HUD.RichBox.RichboxGuiPart>()
            {
                part
            };
        }
    }

    class EndSceneCenterDisplayPart : RichboxGuiPart
    {
        public EndSceneCenterDisplayPart(GameEndReason endReason, VictoryType vType, RichboxGui gui, Action watchEpilogue, MatchResult matchResult)
            : base(gui)
        {
            if (matchResult != null)
            {
                bool playerWin = false;

                content.h1(DssRef.lang.EndScreen_MatchComplete, Color.Yellow);
                content.h2(DssRef.lang.EndScreen_VictoryTitle, HudLib.TitleColor_Label);
                foreach (var m in matchResult.winner)
                {
                    content.newLine();
                    content.Add(m.FlagTextureToHud());
                    content.space(0.5f);
                    content.Add(new RbText(m.player.Name, HudLib.TitleColor_Name));

                    playerWin |= m.player.IsLocalPlayer();
                }

                content.newParagraph();
                content.h2(DssRef.lang.EndScreen_FailTitle, HudLib.TitleColor_Label);
                foreach (var m in matchResult.loser)
                {
                    content.newLine();
                    content.Add(m.FlagTextureToHud());
                    content.space(0.5f);
                    content.Add(new RbText(m.player.Name, HudLib.TitleColor_Name));
                }

                if (playerWin)
                {
                    DssRef.achieve.UnlockAchievement_onAny_50_100_150(AchievementIndex.quick_victory_any, AchievementIndex.quick_victory_50, AchievementIndex.quick_victory_100, AchievementIndex.quick_victory_150);
                }
            }
            else
            {
                switch (endReason)
                {
                    case GameEndReason.Victory:
                        content.h1(DssRef.lang.EndScreen_VictoryTitle, Color.Yellow);


                        string endquote = null;
                        string typeText = null;
                        switch (vType)
                        {
                            case VictoryType.DefeatBoss:
                                typeText = DssRef.lang.VictoryType_DefeatBoss;
                                endquote = arraylib.RandomListMember(DssRef.lang.EndScreen_VictoryQuotes);
                                break;
                            case VictoryType.Domination:
                                typeText = DssRef.lang.VictoryType_Domination;
                                endquote = DssRef.lang.EndScreen_DominationVictoryQuote;
                                break;
                            case VictoryType.WorldPeace:
                                typeText = DssRef.lang.VictoryType_WorldPeace;
                                endquote = DssRef.lang.EndScreen_PeaceVictoryQuote;
                                break;
                        }

                        content.h2(typeText, HudLib.TitleColor_TypeName);
                        content.text(endquote, HudLib.InfoYellow_Light);
                        break;

                    case GameEndReason.Defeat:
                        content.h1(DssRef.lang.EndScreen_FailTitle).overrideColor = Color.Yellow;
                        content.text(arraylib.RandomListMember(DssRef.lang.EndScreen_FailureQuotes));
                        break;

                    case GameEndReason.TimesUp:
                        content.h1(DssRef.lang.EndScreen_TimeHasEndedTitle).overrideColor = Color.Yellow;
                        break;
                }
            }
            content.newParagraph();

            Color ButtonColor = new Color(146, 161, 153);//new Color(48, 80, 101);
            Color ButtonCaptionColor = Color.Black;


            if (!PlatformSettings.STEAM_DEMO)
            {
                content.newLine();
                content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(DssRef.lang.GameMenu_ContinueGame, ButtonCaptionColor) },
                    new RbAction(DssRef.state.cutScene.Close))
                { overrideBgColor = ButtonColor });

                //content.Button(DssRef.lang.GameMenu_ContinueGame, new RbAction(DssRef.state.cutScene.Close), null, true);
            }

            HudLib.WishListButton(content);

            content.newLine();
            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(DssRef.lang.GameMenu_ExitGame, ButtonCaptionColor) },
                    new RbAction(DssRef.state.beginExit, RbSoundType.Back))
            { overrideBgColor = ButtonColor });
            //content.Button(DssRef.lang.GameMenu_ExitGame, new RbAction(DssRef.state.exit, RbSoundType.Back), null, true);

            Vector2 pos = Engine.Screen.SafeArea.CenterTop;
            pos.X -= HudLib.cutsceneGui.width * 0.5f;
            endRefresh(pos, true);
        }        
    }


    class EndSceneRightDisplayMain : RichboxGui
    {
        EndSceneRightDisplayPart part;

        public EndSceneRightDisplayMain()
            : base(HudLib.cutsceneGui, DssRef.state.localPlayers[0].gameControls.input)
        {
            part = new EndSceneRightDisplayPart(this);

            parts = new List<HUD.RichBox.RichboxGuiPart>()
            {
                part
            };
        }
    }

    class EndSceneRightDisplayPart : RichboxGuiPart
    {
        public EndSceneRightDisplayPart(RichboxGui gui)
            : base(gui)
        {
            content.h1(DssRef.lang.Statistics_Title, HudLib.TitleColor_Head2);

            foreach (var p in DssRef.state.localPlayers)
            {
                content.h2(p.Name, HudLib.TitleColor_Name);
                
                p.statistics.ToHud(content);
            }

            Vector2 pos = Engine.Screen.SafeArea.CenterTop;
            pos.X += HudLib.cutsceneGui.width * 0.5f + Engine.Screen.IconSize;
            endRefresh(pos, true);
        }
    }
}
