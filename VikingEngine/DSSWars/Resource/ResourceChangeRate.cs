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
    //    public int amount;
    //    public int goalBuffer;

    //    public void clearFactionOverView()
    //    {
    //        amount = 0;
    //        changeRate.prevProduced = 0;
    //        changeRate.prevConsumed = 0;
    //    }

    //    public void toFactionOverViewMenu(RichBoxContent content, ItemResourceType item)
    //    {
    //        content.newLine();

    //        content.Add(new RbImage(ResourceLib.Icon(item)));
    //        content.space();
    //        content.Add(new RbText(TextLib.LargeFirstLetter(LangLib.Item(item)) + ": "));
    //        content.Add(new RbTab(0.4f));
    //        content.Add(new RbText(TextLib.LargeNumber(amount)));


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
