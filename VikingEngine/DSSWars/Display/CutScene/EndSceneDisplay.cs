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
        public EndSceneDisplay(bool victory, Action watchEpilogue)
        { 
            left = new EndSceneLeftDisplayMain();
            center = new EndSceneCenterDisplayMain(victory, watchEpilogue);
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

            content.icontext(HudLib.CheckImage(DssRef.difficulty.allowPauseCommand), "Allow pause and command");

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

        public EndSceneCenterDisplayMain(bool victory, Action watchEpilogue)
            : base(HudLib.cutsceneGui, DssRef.state.localPlayers[0].input)
        {
            part = new EndSceneCenterDisplayPart(victory, this, watchEpilogue);

            parts = new List<HUD.RichBox.RichboxGuiPart>()
            {
                part
            };
        }
    }

    class EndSceneCenterDisplayPart : RichboxGuiPart
    {
        public EndSceneCenterDisplayPart(bool victory, RichboxGui gui, Action watchEpilogue)
            : base(gui)
        {
            if (victory)
            {
                List<string> victoryQoutes = new List<string>
                {
                    "In times of peace, we mourn the dead.",
                    "Every triumph carries a shadow of sacrifice.",
                    "Remember the journey that brought us here, dotted with the souls of the brave.",
                    "Our minds are light from victory, our hearts are heavy from the weight of the fallen"
                };

                content.h1("Victory!").overrideColor = Color.Yellow;
                content.text(arraylib.RandomListMember(victoryQoutes));
            }
            else
            {
                List<string> failureQoutes = new List<string>
                {
                    "With our bodies torn from marching and nights of worry, we welcome the end.",
                    "Defeat may darken our lands, but they cannot extinguish the light of our determination.",
                    "Extinguish the flames in our hearts, from their ashes, our children shall forge a new dawn.",
                    "Let our tales be the ember that kindles tomorrow's victory.",
                };

                content.h1("Failure!").overrideColor = Color.Yellow;
                content.text(arraylib.RandomListMember(failureQoutes));
            }

            content.newParagraph();
            if (victory)
            {
                content.Button("Watch epilogue", new RbAction(watchEpilogue), null, true);
                content.newLine();
            }
            content.Button("Continue", new RbAction(DssRef.state.cutScene.Close), null, true);
            content.newLine();
            content.Button("Exit game", new RbAction(DssRef.state.exit), null, true);

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
            content.h1("Stats");

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
