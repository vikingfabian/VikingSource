using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Display.CutScene
{
    class EndSceneDisplay
    {
        EndSceneLeftDisplayMain left;
        EndSceneCenterDisplayMain center;
        EndSceneRightDisplayMain right;
        public EndSceneDisplay()
        { 
            left = new EndSceneLeftDisplayMain();
            center = new EndSceneCenterDisplayMain();
            right = new EndSceneRightDisplayMain();
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

        public EndSceneCenterDisplayMain()
            : base(HudLib.cutsceneGui, DssRef.state.localPlayers[0].input)
        {
            part = new EndSceneCenterDisplayPart(this);

            parts = new List<HUD.RichBox.RichboxGuiPart>()
            {
                part
            };
        }
    }

    class EndSceneCenterDisplayPart : RichboxGuiPart
    {
        public EndSceneCenterDisplayPart(RichboxGui gui)
            : base(gui)
        {
            content.h1("Victory!").overrideColor = Color.Yellow;
            content.text("In times of peace, we mourn the dead");

            content.newParagraph();
            content.Button("Continue", null, null, true);
            content.space();
            content.Button("Exit", null, null, true);

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
