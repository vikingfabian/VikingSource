using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Resource
{    
    struct ResourceChangeRate
    {
        public ushort produced, consumed;
        public ushort prevProduced, prevConsumed;

        public void onChange(int change)
        {
            if (change > 0)
            {
                produced += (ushort)change;
            }
            else
            {
                consumed -= (ushort)change;
            }
        }

        public void toMenu(RichBoxContent content)
        {
            content.Add(new RbText(DssRef.lang.Resource_ConsumedProduced + ":", HudLib.TitleColor_Label));

            content.space();
            content.Add(new RbImage(SpriteName.WarsDecreaseArrowDown));
            var downText = new RbText(TextLib.LargeNumber(prevConsumed));
            downText.overrideColor = HudLib.NotAvailableColor;
            content.Add(downText);

            content.space();
            content.Add(new RbImage(SpriteName.WarsIncreaseArrowUp));
            var upText = new RbText(TextLib.LargeNumber(prevProduced));
            upText.overrideColor = HudLib.AvailableColor;
            content.Add(upText);

        }

        public void oneMinuteUpdate()
        {
            prevProduced = produced;
            prevConsumed = consumed;
            produced = 0;
            consumed = 0;
        }

        public int Change => prevProduced - prevConsumed;
        
    }
}
