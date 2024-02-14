using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    class NextPhaseButton : ToggEngine.Display2D.AbsPhaseButton
    {
        public NextPhaseButton(VectorRect area, LocalPlayer player)
            : base(area)
        {
            //tooltipText = "End turn";
            refreshIcon(false, player);
            buttonMap = toggRef.inputmap.nextPhase;
        }

        public void refreshIcon(bool readyCheck, LocalPlayer player)
        {
            if (readyCheck)
            {
                icon.SetSpriteName(SpriteName.cmdOrderCheckFlat);
                tooltipText = "Cancel: waiting for the other players";
            }
            else
            {
                icon.SetSpriteName(SpriteName.pjForwardIcon);

                if (player.nextPhaseIsEndTurn())
                {
                    tooltipText = "End turn";
                }
                else
                {
                    tooltipText = "Next Hero";
                }
            }
        }
    }

    class PrevPhaseButton : ToggEngine.Display2D.AbsPhaseButton
    {
        public PrevPhaseButton(VectorRect area)
            : base(area)
        {
            icon.spriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            tooltipText = "Back to Strategy Selection";
        }
    }

}
