using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.CardDesign.CardEditor
{
    class NumberEditor
    {
        public void DragButton(RichBoxContent content, RichMenu menu, string caption, Range bounds, IntGetSetTag intProp)
        {
            DSSWars.HudLib.Label(content, caption);
            NumberDragButton.RbDragButtonGroup(content, new DragButtonSettings(bounds, 1), intProp, false);
        }
    }
}
