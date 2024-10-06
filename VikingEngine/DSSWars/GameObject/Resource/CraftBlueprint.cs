using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    class CraftBlueprint
    {
        SpriteName icon;
        string name;
        UseResource[] resources;
        int resultCount;
        public CraftRequirement requirement;

        public CraftBlueprint(SpriteName icon, string name, int result, UseResource[] resources, CraftRequirement requirement = CraftRequirement.None)
        {
            this.icon = icon;
            this.name = name;
            this.resultCount = result;
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

        public int craft(City city)
        {
            foreach (var r in resources)
            {
                city.AddGroupedResource(r.type, -r.amount);
            }

            return resultCount;
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

            return resultCount;
        }

        public void toMenu(RichBoxContent content, City city)
        {
            //string resourcesString = string.Empty;
            content.newLine();
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
            content.Add(new RichBoxText(resultCount.ToString()));
            if (icon != SpriteName.NO_IMAGE)
            {
                content.Add(new RichBoxImage(icon));
            }
            else
            {
                content.space();
            }
            content.Add(new RichBoxText(name));

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
    }
    
}
