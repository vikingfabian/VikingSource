using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.Core.DSSWars.Resource
{
    struct ResourceConvertOption
    {
        public ItemResource from;
        public ItemResource to;

        public int count;

        public ResourceConvertOption(ItemResource from, ItemResource to)
        {
            this.from = from;
            this.to = to;
            count = 1;
        }

    }

    static class ResourceQuickConvert
    {
        static readonly ResourceConvertOption FineWood = new ResourceConvertOption(new ItemResource(ItemResourceType.FineWood_G, 1), new ItemResource(ItemResourceType.Wood_G, 1));
        static readonly ResourceConvertOption Brick = new ResourceConvertOption(new ItemResource(ItemResourceType.Brick, 1), new ItemResource(ItemResourceType.Stone_G, 1));

        public static void ToMenu(LocalPlayer player, City city, RichBoxContent content)
        {
            option(FineWood);
            option(Brick);

            void option(ResourceConvertOption opt)
            {
                content.newParagraph();
                
                opt.from.ToHud(content);
                content.space();
                content.Add(new RbImage(SpriteName.cmdConvertArrow));
                content.space();
                opt.to.ToHud(content);

                content.space();
                convertButton(1);
                convertButton(10);
                convertButton(100);
                convertButton(1000);
                int fromAmount = city.GetGroupedResource(opt.from.type).amount;
                if (fromAmount > opt.from.amount)
                {
                    convertButton(fromAmount / opt.from.amount);
                }

                void convertButton(int amount)
                {
                    ResourceConvertOption opt_count = opt;
                    opt_count.count = amount;
                    bool available = city.GetGroupedResource(opt_count.from.type).amount >= opt_count.from.amount * opt_count.count;
                    
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(string.Format( DssRef.lang.Hud_XTimes, TextLib.LargeNumber(amount))) },
                        new RbAction1Arg<ResourceConvertOption>(convertAction, opt_count),
                        new RbTooltip(tooltip, opt_count), available));

                   
                }
            }

            void convertAction(ResourceConvertOption opt_count)
            {
                int fromCount = opt_count.from.amount * opt_count.count;
                int toCount = opt_count.to.amount * opt_count.count;
                city.GetRefGroupedResource(opt_count.from.type).amount -= fromCount;
                city.GetRefGroupedResource(opt_count.to.type).amount += toCount;
            }

            void tooltip(RichBoxContent content, object tag)
            {
                ResourceConvertOption opt_count = (ResourceConvertOption)tag;
                content.h1(SpriteName.cmdConvertArrow, DssRef.todoLang.Resource_ConvertTitle, HudLib.TitleColor_Action);

                content.h2(DssRef.lang.Hud_PurchaseTitle_CurrentlyOwn, HudLib.TitleColor_Label);
                listItem(opt_count.from.type);
                listItem(opt_count.to.type);

                void listItem(ItemResourceType resourceType)
                {
                    content.newLine();
                    IconName.Item(resourceType, out SpriteName itemIcon, out string itemName);
                    content.Add(new RbText(city.GetGroupedResource(resourceType).amount.ToString()));
                    content.hspace();
                    content.Add(new RbImage(itemIcon));
                    content.hspace();
                    content.Add(new RbText(TextLib.LargeFirstLetter(itemName)));
                }
                
            }
        }

       
    }
}
