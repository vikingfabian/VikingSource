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
        public EndSceneDisplay(bool victory, bool bossVictory, Action watchEpilogue)
        { 
            left = new EndSceneLeftDisplayMain();
            center = new EndSceneCenterDisplayMain(victory, bossVictory, watchEpilogue);
            right = new EndSceneRightDisplayMain();

            center.beginMove(0);
        }

        public void update()
        {
            center.updateMove();
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
            : base(HudLib.cutsceneGui, DssRef.state.localPlayers[0].input)
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

            content.icontext(HudLib.CheckImage(DssRef.difficulty.allowPauseCommand), DssRef.lang.Settings_AllowPause);

            var time = DssRef.time.TotalIngameTime();
            content.text(string.Format(DssRef.lang.EndGameStatistics_Time, time));

            content.newParagraph();
            content.text(DateTime.Now.ToShortDateString());
            content.text(string.Format(DssRef.lang.Lobby_GameVersion, Engine.LoadContent.SteamVersion));
            
            Vector2 pos = Engine.Screen.SafeArea.CenterTop;
            pos.X -= HudLib.cutsceneGui.width * 1.5f + Engine.Screen.IconSize;
            endRefresh(pos, true);
        }

    }



    class EndSceneCenterDisplayMain: RichboxGui
    {
        EndSceneCenterDisplayPart part;

        public EndSceneCenterDisplayMain(bool victory, bool bossVictory, Action watchEpilogue)
            : base(HudLib.cutsceneGui, DssRef.state.localPlayers[0].input)
        {
            part = new EndSceneCenterDisplayPart(victory, bossVictory, this, watchEpilogue);

            parts = new List<HUD.RichBox.RichboxGuiPart>()
            {
                part
            };
        }
    }

    class EndSceneCenterDisplayPart : RichboxGuiPart
    {
        public EndSceneCenterDisplayPart(bool victory, bool bossVictory, RichboxGui gui, Action watchEpilogue)
            : base(gui)
        {
            if (victory)
            {

                content.h1(DssRef.lang.EndScreen_VictoryTitle).overrideColor = Color.Yellow;

                if (bossVictory)
                {
                    content.text(arraylib.RandomListMember(DssRef.lang.EndScreen_VictoryQuotes));
                }
                else
                {
                    content.text(DssRef.lang.EndScreen_DominationVictoryQuote);
                }
            }
            else
            {

                content.h1(DssRef.lang.EndScreen_FailTitle).overrideColor = Color.Yellow;
                content.text(arraylib.RandomListMember(DssRef.lang.EndScreen_FailureQuotes));
            }

            content.newParagraph();
            if (victory && bossVictory)
            {
                content.Button(DssRef.lang.EndScreen_WatchEpilogue, new RbAction(watchEpilogue), null, true);
                content.newLine();
            }
            content.Button(DssRef.lang.GameMenu_ContinueGame, new RbAction(DssRef.state.cutScene.Close), null, true);
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
            : base(HudLib.cutsceneGui, DssRef.state.localPlayers[0].input)
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
