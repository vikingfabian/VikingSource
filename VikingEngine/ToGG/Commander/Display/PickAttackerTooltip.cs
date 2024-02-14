using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.Commander.Display
{
    class PickAttackerTooltip : AbsToolTip
    {
        public PickAttackerTooltip(bool attacker, Commander.Players.LocalPlayer player)
            : base(player.mapControls)
        {
            RichBoxContent members = new RichBoxContent();

            members.h2(attacker? "Pick attacker" : "Pick target");

            //members.add("Ordered units must rest for one turn");

            AddRichBox(members);
        }
    }

}
