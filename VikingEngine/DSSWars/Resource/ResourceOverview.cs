using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Resource
{
    struct ResourceOverview
    {
        public int current, produced, consumed;
        public int prevCurrent, prevProduced, prevConsumed;

        public void onChange(int change)
        {
            if (change > 0)
            {
                produced += change;
            }
            else
            { 
                consumed -= change;
            }
        }

        public void clearCurrent()
        {
            prevCurrent = current;
            current = 0;
        }

        public void oneSecondUpdate()
        {
            prevProduced = produced;
            prevConsumed = consumed;
            produced = 0;
            consumed = 0;
        }

        public void toMenu(RichBoxContent content, ItemResourceType item)
        {
            content.newLine();

            content.Add(new RichBoxImage(ResourceLib.Icon(item)));
            content.space();
            content.Add(new RichBoxText(LangLib.Item(item) + ": "));
            content.Add(new RichBoxTab(0.4f));
            content.Add(new RichBoxText(TextLib.LargeNumber(prevCurrent)));


            content.Add(new RichBoxTab(0.5f));
            content.Add(new RichBoxImage(SpriteName.WarsDecreaseArrowDown));
            var downText = new RichBoxText(TextLib.LargeNumber(prevConsumed));
            downText.overrideColor = HudLib.NotAvailableColor;
            content.Add(downText);

            content.Add(new RichBoxTab(0.6f));
            content.Add(new RichBoxImage(SpriteName.WarsIncreaseArrowUp));
            var upText = new RichBoxText(TextLib.LargeNumber(prevProduced));
            upText.overrideColor = HudLib.AvailableColor;
            content.Add(upText);

        }
    }
}
