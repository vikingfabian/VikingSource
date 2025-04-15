using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.PJ;
using VikingEngine.ToGG.HeroQuest.Data.Condition;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.DSSWars.Display.CutScene
{
    class EndSceneDisplay
    {
        EndSceneLeftDisplayMain left;
        EndSceneCenterDisplayMain center;
        EndSceneRightDisplayMain right;
        public EndSceneDisplay(GameEndReason endReason, bool bossVictory, Action watchEpilogue)
        { 
            left = new EndSceneLeftDisplayMain();
            center = new EndSceneCenterDisplayMain(endReason, bossVictory, watchEpilogue);
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
            content.h1(string.Format(DssRef.lang.Settings_TotalDifficulty, DssRef.difficulty.TotalDifficulty()));
            content.text(string.Format(DssRef.lang.Settings_DifficultyLevel, DssRef.difficulty.PercDifficulty));

            content.icontext(HudLib.CheckImage(DssRef.difficulty.setting_allowPauseCommand), DssRef.lang.Settings_AllowPause);

            var time = HudLib.TimeSpan_LongText(DssRef.time.TotalIngameTime());
            content.text(string.Format(DssRef.lang.EndGameStatistics_Time, time));

            content.newParagraph();
            content.text(HudLib.Date(DateTime.Now));
            content.text(string.Format(HudLib.EngineVersionString, Engine.LoadContent.SteamVersion));
            
            Vector2 pos = Engine.Screen.SafeArea.CenterTop;
            pos.X -= HudLib.cutsceneGui.width * 1.5f + Engine.Screen.IconSize;
            endRefresh(pos, true);
        }
    }



    class EndSceneCenterDisplayMain: RichboxGui
    {
        EndSceneCenterDisplayPart part;

        public EndSceneCenterDisplayMain(GameEndReason endReason, bool bossVictory, Action watchEpilogue)
            : base(HudLib.cutsceneGui, DssRef.state.localPlayers[0].gameControls.input)
        {
            part = new EndSceneCenterDisplayPart(endReason, bossVictory, this, watchEpilogue);

            parts = new List<HUD.RichBox.RichboxGuiPart>()
            {
                part
            };
        }
    }

    class EndSceneCenterDisplayPart : RichboxGuiPart
    {
        public EndSceneCenterDisplayPart(GameEndReason endReason, bool bossVictory, RichboxGui gui, Action watchEpilogue)
            : base(gui)
        {
            switch (endReason)
            {
                case GameEndReason.Victory:
                    content.h1(DssRef.lang.EndScreen_VictoryTitle).overrideColor = Color.Yellow;

                    if (bossVictory)
                    {
                        content.text(arraylib.RandomListMember(DssRef.lang.EndScreen_VictoryQuotes));
                    }
                    else
                    {
                        content.text(DssRef.lang.EndScreen_DominationVictoryQuote);
                    }
                    break;
                
                case GameEndReason.Defeat:
                    content.h1(DssRef.lang.EndScreen_FailTitle).overrideColor = Color.Yellow;
                    content.text(arraylib.RandomListMember(DssRef.lang.EndScreen_FailureQuotes));
                    break;

                case GameEndReason.TimesUp:
                    content.h1(DssRef.lang.EndScreen_TimeHasEndedTitle).overrideColor = Color.Yellow;
                    break;
            }

            content.newParagraph();
            if (endReason == GameEndReason.Victory && bossVictory && !PlatformSettings.STEAM_DEMO)
            {
                content.Button(DssRef.lang.EndScreen_WatchEpilogue, new RbAction(watchEpilogue), null, true);
                
            }
            if (!PlatformSettings.STEAM_DEMO)
            {
                content.newLine();
                content.Button(DssRef.lang.GameMenu_ContinueGame, new RbAction(DssRef.state.cutScene.Close), null, true);
            }

            HudLib.WishListButton(content);

            content.newLine();
            content.Button(DssRef.lang.GameMenu_ExitGame, new RbAction(DssRef.state.exit), null, true);

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
            content.h1(DssRef.lang.EndGameStatistics_Title);

            foreach (var p in DssRef.state.localPlayers)
            {
                //if (DssRef.state.localPlayers.Count > 0)
                {
                    content.h2(p.Name);
                }
                p.statistics.ToHud(content);
            }

            Vector2 pos = Engine.Screen.SafeArea.CenterTop;
            pos.X += HudLib.cutsceneGui.width * 0.5f + Engine.Screen.IconSize;
            endRefresh(pos, true);
        }
    }
}
