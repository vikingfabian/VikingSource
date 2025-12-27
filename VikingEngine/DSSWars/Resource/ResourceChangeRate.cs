using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Resource
{
    //struct ResourceFactionOverview
    //{
    //    public ResourceChangeRate changeRate;
    //    public int current;
    //    public int goalBuffer;

    //    public void clearOverview()
    //    {
    //        current = 0;
    //        changeRate.prevProduced = 0;
    //        changeRate.prevConsumed = 0;
    //    }

    //    public void toFactionViewMenu(RichBoxContent content, ItemResourceType item)
    //    {
    //        content.newLine();

    //        IconName.Item(item, out SpriteName itemIcon, out string itemName);

    //        content.Add(new RbImage(itemIcon));
    //        content.space();
    //        content.Add(new RbText(TextLib.LargeFirstLetter(itemName) + ": "));
    //        content.Add(new RbTab(0.4f));
    //        content.Add(new RbText(TextLib.LargeNumber(current)));


    //        content.Add(new RbTab(0.5f));
    //        content.Add(new RbImage(SpriteName.WarsDecreaseArrowDown));
    //        var downText = new RbText(TextLib.LargeNumber(changeRate.prevConsumed));
    //        downText.overrideColor = HudLib.NotAvailableColor;
    //        content.Add(downText);

    //        content.Add(new RbTab(0.6f));
    //        content.Add(new RbImage(SpriteName.WarsIncreaseArrowUp));
    //        var upText = new RbText(TextLib.LargeNumber(changeRate.prevProduced));
    //        upText.overrideColor = HudLib.AvailableColor;
    //        content.Add(upText);

    //    }
    //}

    struct ResourceChangeRate
    {
        public int /*current, */produced, consumed;
        public int /*prevCurrent,*/ prevProduced, prevConsumed;

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

        public void oneSecondUpdate()
        {
            prevProduced = produced;
            prevConsumed = consumed;
            produced = 0;
            consumed = 0;
        }

        
    }
}
