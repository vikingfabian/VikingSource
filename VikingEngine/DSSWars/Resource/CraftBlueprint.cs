using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.XP;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.HeroQuest.HeroStrategy;

namespace VikingEngine.DSSWars.Resource
{
    class CraftBlueprint
    {
        //SpriteName icon;
        //string name;
        public UseResource[] resources;
        CraftResultType resultType;
        int resultSubType;
        int resultAmount;

        public CraftRequirement requirement;
        public int tooltipId = -1;
        public WorkExperienceType experienceType;
        public ExperienceLevel levelRequirement;

        public CraftBlueprint(CraftResultType resultType, int resultSubType, int resultAmount, UseResource[] resources, XP.WorkExperienceType experienceType, ExperienceLevel levelRequirement = ExperienceLevel.Beginner_1, CraftRequirement requirement = CraftRequirement.None)
        {
            //this.icon = icon;
            this.experienceType = experienceType;
            this.resultType = resultType;
            this.resultSubType = resultSubType;
            this.resultAmount = resultAmount;
            this.resources = resources;
            this.levelRequirement = levelRequirement;
            this.requirement = requirement;
        }

        //public void createBackOrder(City city)
        //{
        //    foreach (var r in resources)
        //    {
        //        var res = city.GetGroupedResource(r.type);
        //        res.backOrder += r.amount;
        //        city.SetGroupedResource(r.type, res);
        //    }

        //}
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

        public bool hasResources(City city)
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

                min = lib.SmallestValue(res.amount / r.amount, min);
            }
            return min;
        }

        public int payResources(City city)
        {
            foreach (var r in resources)
            {
                city.AddGroupedResource(r.type, -r.amount);
            }

            return resultAmount;
        }

        public int tryPayResources(City city)
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
                    countText.overrideColor = available ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                    //}
                    content.Add(countText);
                    content.Add(new RichBoxImage(sprite));
                    content.Add(new RichBoxText(name));
                }
            }
        }

        public bool meetsRequirements(City city)
        {
            requirementToHud(null, city, out bool result);
            return result;
        }

        public void requirementToHud(RichBoxContent content, City city, out bool available)
        {
            available = true;

            if (requirement != CraftRequirement.None)
            {
                if (content != null)
                {
                    content.newLine();
                    HudLib.Label(content, DssRef.lang.Hud_PurchaseTitle_Requirement);
                    content.newLine();
                    HudLib.BulletPoint(content);
                }
                string reqText;

                switch (requirement)
                {
                    case CraftRequirement.Carpenter:
                        reqText = DssRef.lang.BuildingType_Carpenter;
                        available = city.hasBuilding_carpenter;
                        break;
                    case CraftRequirement.Brewery:
                        reqText = DssRef.lang.BuildingType_Brewery;
                        available = city.hasBuilding_brewery;
                        break;
                    case CraftRequirement.Smelter:
                        reqText = DssRef.todoLang.BuildingType_SmeltingFurnace;
                        available = city.buildingCount_smelter > 0;
                        break;
                    case CraftRequirement.Chemist:
                        reqText = DssRef.todoLang.BuildingType_Chemist;
                        available = city.buildingCount_chemist > 0;
                        break;
                    case CraftRequirement.CoinMaker:
                        reqText = DssRef.todoLang.BuildingType_CoinMaker;
                        available = city.buildingCount_coinmaker > 0;
                        break;
                    case CraftRequirement.Foundry:
                        reqText = DssRef.todoLang.BuildingType_Foundry;
                        available = city.buildingCount_foundry > 0;
                        break;
                    case CraftRequirement.Smith:
                        reqText = DssRef.lang.BuildingType_Smith;
                        available = city.hasBuilding_smith;
                        break;
                    case CraftRequirement.CoalPit:
                        reqText = DssRef.lang.BuildingType_CoalPit;
                        available = city.buildingCount_coalpit > 0;
                        break;
                    case CraftRequirement.Logistics1:
                        reqText = string.Format(DssRef.lang.Requirements_XItemStorageOfY, DssRef.lang.Resource_TypeName_Food, City.Logistics1FoodStorage);
                        available = city.res_food.amount >= City.Logistics1FoodStorage;
                        break;

                    default:
                        throw new NotImplementedException();
                }

                if (content != null)
                {
                    RichBoxText requirement1 = new RichBoxText(reqText);
                    requirement1.overrideColor = available ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                    content.Add(requirement1);
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
        Smelter,
        Smith,
        Foundry,
        CoalPit,
        CoinMaker,
        Chemist,
        Logistics1,
        Logistics2,

    }

    enum CraftResultType
    {
        Resource,
        Building,
    }
}
