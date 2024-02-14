using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.Commander.Display
{
    class NextPhaseButton : ToggEngine.Display2D.AbsPhaseButton
    {
        public NextPhaseButton(VectorRect area)
            : base(area)
        {
            //tooltipText = "End turn";
            //refreshIcon(false, player);
            
            icon.SetSpriteName(SpriteName.pjForwardIcon);
            buttonMap = toggRef.inputmap.nextPhase;
            tooltipText = "Next phase";
        }

        //public void refreshIcon(bool readyCheck)
        //{
        //    if (readyCheck)
        //    {
        //        icon.SetSpriteName(SpriteName.cmdOrderCheckFlat);
        //        tooltipText = "Cancel: waiting for the other players";
        //    }
        //    else
        //    {
        //        icon.SetSpriteName(SpriteName.pjForwardIcon);

        //        if (player.nextPhaseIsEndTurn())
        //        {
        //            tooltipText = "End turn";
        //        }
        //        else
        //        {
        //            tooltipText = "Next Hero";
        //        }
        //    }
        //}
    }

    class PrevPhaseButton : ToggEngine.Display2D.AbsPhaseButton
    {
        public PrevPhaseButton(VectorRect area)
            : base(area)
        {
            icon.spriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            tooltipText = "Back";
        }
    }
}
