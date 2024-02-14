using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.HeroQuest.Players.Phase
{
    class UnitActionSelectTarget : AbsPlayerPhase
    {
        public Display.ActionButton button = null;

        public UnitActionSelectTarget(Display.ActionButton button, LocalPlayer player)
            :base(player)
        {
            this.button = button;
        }

        public override void onBegin()
        {
            player.mapControls.SetAvailableTiles(null, 
                button.skill.unitActionTargets(button.unit));
        }

        override public void update(ref PlayerDisplay display)
        {
            player.mapUpdate(ref display, false);

            List<IntVector2> groupSelection;
            if (button.skill.IsValidActionTarget(button.unit, 
                player.mapControls.selectionIntV2, out groupSelection))
            {
                player.availableMapSquare = MapSquareAvailableType.Enabled;

                if (groupSelection != null)
                {
                    player.mapControls.GroupFrame(groupSelection);
                }

                if (toggRef.inputmap.click.DownEvent)
                {
                    button.Use(player.mapControls.selectionIntV2);
                    end();
                }
            }
            else
            {
                player.availableMapSquare = MapSquareAvailableType.Disabled;

                if (toggRef.inputmap.click.DownEvent)
                {
                    end();
                    //player.clearUnitActionMode();
                }
            }

            if (toggRef.inputmap.menuInput.openCloseInputEvent() ||
                toggRef.inputmap.back.DownEvent)
            {
                end();
                //player.clearUnitActionMode();
            }

            if (player.RefreshUi)
            {
                if (player.availableMapSquare == MapSquareAvailableType.Enabled)
                {
                    var tooltip = button.skill.actionTargetToolTip();
                    new Display.RichboxTooltip(tooltip, player.mapControls);
                }
                else
                {
                    player.mapControls.removeToolTip();
                }
            }
        }

        public override PhaseType Type => PhaseType.UnitActionSelectTarget;
    }
}
