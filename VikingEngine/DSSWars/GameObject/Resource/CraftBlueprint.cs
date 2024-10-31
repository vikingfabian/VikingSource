using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.Map;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.HeroQuest.HeroStrategy;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    class CraftBlueprint
    {
        //SpriteName icon;
        //string name;
        UseResource[] resources;
        CraftResultType resultType;
        int resultSubType;
        int resultAmount;

        public CraftRequirement requirement;
        public int tooltipId = -1;

        public CraftBlueprint(CraftResultType resultType, int resultSubType, int resultAmount, UseResource[] resources, CraftRequirement requirement = CraftRequirement.None)
        {
            //this.icon = icon;
            this.resultType = resultType;
            this.resultSubType = resultSubType;
            this.resultAmount = resultAmount;
            this.resources = resources;
            this.requirement = requirement;
        }

        public void createBackOrder(City city)
        {
            foreach (var r in resources)
            {
                var res = city.GetGroupedResource(r.type);
                res.backOrder += r.amount;
                city.SetGroupedResource(r.type, res);
            }
            
        }
        public bool available(City city)
        {
            foreach (var r in resources)
            {
                var res = city.GetGroupedResource(r.type);
                if (res.amount < r.amount)
                {
                    return false;
                }
            }
            return true;
        }

        public bool canCraft(City city)
        {
            foreach (var r in resources)
            {
                var res = city.GetGroupedResource(r.type);
                if (res.amount < r.amount)
                {
                    return false;
                }
            }
            return true;
        }

        public int canCraftCount(City city)
        {
            int min = int.MaxValue;
            foreach (var r in resources)
            {
                var res = city.GetGroupedResource(r.type);
                if (res.amount < r.amount)
                {
                    return 0;
                }

                min = lib.SmallestValue( res.amount / r.amount, min);
            }
            return min;
        }

        public int craft(City city)
        {
            foreach (var r in resources)
            {
                city.AddGroupedResource(r.type, -r.amount);
            }

            return resultAmount;
        }

        public int tryCraft(City city)
        {
            foreach (var r in resources)
            {
                var res = city.GetGroupedResource(r.type);
                if (res.amount < r.amount)
                {
                    return 0;
                }
            }
            foreach (var r in resources)
            {
                city.AddGroupedResource(r.type, -r.amount);
            }

            return resultAmount;
        }

        string name()
        {
            switch (resultType)
            {
                case CraftResultType.Resource:
                    return LangLib.Item((ItemResourceType)resultSubType);
                case CraftResultType.Building:
                    return BuildLib.BuildOptions[resultSubType].Label();
            }

            return TextLib.Error;
        }

        SpriteName icon()
        {
            switch (resultType)
            {
                case CraftResultType.Resource:
                    return ResourceLib.Icon((ItemResourceType)resultSubType);
                case CraftResultType.Building:
                    return BuildLib.BuildOptions[resultSubType].sprite;
            }

            return SpriteName.NO_IMAGE;
        }

        public void toMenu(RichBoxContent content, City city, bool newLine = true)
        {
            if (newLine)
            {
                content.newLine();
            }
            bool first = true;
            bool available;
            foreach (var r in resources)
            {
                available = city.GetGroupedResource(r.type).amount >= r.amount;
                addResources(r.amount, ResourceLib.Icon(r.type), LangLib.Item(r.type));
                first = false;
            }

            var arrow = new RichBoxImage(SpriteName.pjNumArrowR);
            arrow.color = Color.CornflowerBlue;
            content.Add(arrow);
            content.Add(new RichBoxText(resultAmount.ToString()));            
            content.Add(new RichBoxImage(icon()));           
            content.Add(new RichBoxText(name()));

            content.newLine();

            void addResources(int count, SpriteName sprite, string name)
            {
                if (count > 0)
                {
                    //string countString = count.ToString();
                    if (!first)
                    {
                        content.Add(new RichBoxImage(SpriteName.pjNumPlus));
                        //countString = " + " + countString;
                    }
                    var countText = new RichBoxText(count.ToString());
                    //if (!available)
                    //{ 
                        countText.overrideColor = available? HudLib.AvailableColor : HudLib.NotAvailableColor;
                    //}
                    content.Add(countText);
                    content.Add(new RichBoxImage(sprite));
                    content.Add(new RichBoxText(name));
                }
            }
        }

        public void listResources(RichBoxContent content, City city, CraftBlueprint optionalBp = null)
        {
            bool reachedBuffer = false;
            content.newLine();
            foreach (var r in resources)
            {
               var cityResource = city.GetGroupedResource(r.type);
                bool safeGuard = city.foodSafeGuardIsActive(r.type);
                cityResource.toMenu(content, r.type, safeGuard, ref reachedBuffer);               
            }

            if (optionalBp != null)
            {
                foreach (var r in optionalBp.resources)
                {
                    if (!resources.Contains(r))
                    {
                        var cityResource = city.GetGroupedResource(r.type);
                        bool safeGuard = city.foodSafeGuardIsActive(r.type);
                        cityResource.toMenu(content, r.type, safeGuard, ref reachedBuffer);
                    }
                }
            }

            //if (reachedBuffer)
            //{
            //    GroupedResource.BufferIconInfo(content);
            //}
        }
    }

    struct UseResource
    {
        public ItemResourceType type;
        public int amount;

        public UseResource(ItemResourceType type, int amount)
        { 
            this.type = type;
            this.amount = amount;
        }       
    }

    enum CraftRequirement
    { 
        None = 0,
        Carpenter,
        Brewery,
        Smith,
        CoalPit,
    }

    enum CraftResultType
    {
        Resource,
        Building,
    }
}
