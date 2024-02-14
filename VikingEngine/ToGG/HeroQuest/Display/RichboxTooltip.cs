using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    class RichboxTooltip : AbsToolTip
    {
        public RichboxTooltip(List<AbsRichBoxMember> richbox, 
            MapControls mapControls)
            : base(mapControls)
        {
            AddRichBox(richbox);
        }
    }
}
