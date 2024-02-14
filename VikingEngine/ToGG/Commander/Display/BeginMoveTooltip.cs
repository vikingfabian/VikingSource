using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.Commander.Display
{
    class BeginMoveTooltip : AbsToolTip
    {
        public BeginMoveTooltip(bool ableToMove, Commander.Players.LocalPlayer player)
            : base(player.mapControls)
        {
            RichBoxContent members = new RichBoxContent();
            if (ableToMove)
            {
                members.h2("Move unit");
            }
            else
            {
                members.Add(new RichBoxBeginTitle());
                members.Add(new RichBoxText("Out of movement", HudLib.UnavailableRedCol));
            }

            AddRichBox(members);
        }
    }
}
