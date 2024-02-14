using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.Commander.Display
{
    class QuestBanner : AbsQuestBanner
    {
        public QuestBanner(float rightPos)
            : base(rightPos)
        { }

        protected override void createToolTip()
        {
            base.createToolTip();

            HUD.RichBox.RichBoxContent content = new HUD.RichBox.RichBoxContent();

            string info;

            if (toggRef.gamestate.gameSetup.level == LevelEnum.NONE)
            {
                content.h2("Reach " + toggLib.WinnerScore.ToString() + " victory points to win");
                content.newLine();
                content.newLine();
                content.text(string.Format("Destoyed unit: {0} points", toggLib.VP_DestroyUnit));
                content.text(string.Format("Hold banner: {0} points", toggLib.VP_TacticalBanner));
                content.text(string.Format("Destoy enemy base: {0} points", toggLib.VP_DestroyEnemyBase));
            }
            else
            {
                info = toggRef.gamestate.gameSetup.missionName + ":" + Environment.NewLine +
                    toggRef.gamestate.gameSetup.missionDescription;

                if (toggRef.gamestate.gameSetup.WinningConditions.HasTurnLimit)
                {
                    info += Environment.NewLine + "Turn " + Commander.cmdRef.players.ActiveLocalPlayer().TurnsCount + "/" + toggRef.gamestate.gameSetup.WinningConditions.timeLimit.ToString();
                }

                content.text(info);
            }

            
            

            //var richbox = hqRef.setup.conditions.questDescription();
            HudLib.AddTooltipText(tooltip, content, Dir4.S, area,
                area, true, false);
        }
    }
}
