using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.Core.DSSWars.Resource
{
    struct ResourceConvertOption
    {
        public ItemResource from;
        public ItemResource to;

        public ResourceConvertOption(ItemResource from, ItemResource to)
        {
            this.from = from;
            this.to = to;
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
                content.newLine();
                /*content.Add(new RbText(opt.fro)) */
                opt.from.ToHud(content);
                content.space();
                content.Add(new RbImage(SpriteName.cmdConvertArrow));
                content.space();
                opt.to.ToHud(content);
            }
        }
    }
}
