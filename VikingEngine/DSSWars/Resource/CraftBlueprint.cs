using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.XP;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.HeroQuest.HeroStrategy;

namespace VikingEngine.DSSWars.Resource
{
    class CraftBlueprint
    {
        public UseResource[] resources;
        CraftResultType resultType;
        int resultSubType1 = -1;
        int resultAmount1;
        int resultSubType2 = -1;
        int resultAmount2;

        public BuildAndExpandType requirement;
        public int tooltipId = -1;
        public WorkExperienceType experienceType;
        public ExperienceLevel levelRequirement;
        public CraftBlueprint upgradeFrom = null;

        public int workTag = -1;

        public CraftBlueprint(CraftResultType resultType, int resultSubType, int resultAmount, UseResource[] resources, XP.WorkExperienceType experienceType, ExperienceLevel levelRequirement = ExperienceLevel.Beginner_1, BuildAndExpandType requirement = BuildAndExpandType.NUM_NONE)
        {
            //this.icon = icon;
            this.experienceType = experienceType;
            this.resultType = resultType;
            this.resultSubType1 = resultSubType;
            this.resultAmount1 = resultAmount;
            this.resources = resources;
            this.levelRequirement = levelRequirement;
            this.requirement = requirement;
        }

        public void addSecondResult(ItemResourceType item, int count)
        {
            resultSubType2 = (int)item;
            resultAmount2 = count;
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

        public bool hasResources_buildAndUpgrade(City city)
        {
            if (upgradeFrom != null && !upgradeFrom.hasResources_buildAndUpgrade(city))
            { return false; }

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

        public bool hasFullStock(City city)
        {
            foreach (var r in resources)
            {
                var res = city.GetGroupedResource(r.type);
                if (!res.almostReachedBuffer())
                {
                    return false;
                }
            }
            return true;
        }

        public int payResources(City city)
        {
            foreach (var r in resources)
            {
                city.AddGroupedResource(r.type, -r.amount);
            }

            return resultAmount1;
        }

        public int payResources_BuildAndUpgrade(City city)
        {
            upgradeFrom?.payResources_BuildAndUpgrade(city);

            foreach (var r in resources)
            {
                city.AddGroupedResource(r.type, -r.amount);
            }

            return resultAmount1;
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

            return resultAmount1;
        }

        void iconName(int resultNumber, out SpriteName icon, out string name)
        {

            int resultSubType; /*= resultNumber == 1? resultSubType1 : resultSubType2;*/
            int resultAmount;

            if (resultNumber == 1)
            {
                resultSubType = resultSubType1;
                resultAmount = resultAmount1;
            }
            else
            {
                resultSubType = resultSubType2;
                resultAmount = resultAmount2;
            }

            switch (resultType)
            {
                case CraftResultType.Resource:
                    name = LangLib.Item((ItemResourceType)resultSubType);

                    break;
                case CraftResultType.Building:
                    name = BuildLib.BuildOptions[resultSubType].Label();

                    break;
            }
        }

        string name()
        {
            switch (resultType)
            {
                case CraftResultType.Resource:
                    return LangLib.Item((ItemResourceType)resultSubType1);
                case CraftResultType.Building:
                    return BuildLib.BuildOptions[resultSubType1].Label();
            }

            return TextLib.Error;
        }

        SpriteName icon()
        {
            switch (resultType)
            {
                case CraftResultType.Resource:
                    return ResourceLib.Icon((ItemResourceType)resultSubType1);
                case CraftResultType.Building:
                    return BuildLib.BuildOptions[resultSubType1].sprite;
            }

            return SpriteName.NO_IMAGE;
        }

        public void resultTypeToMenu(RichBoxContent content)
        { 
            content.Add(new RbImage(icon()));
            content.space();
            content.Add(new RbText(name()));
        }

        public void toMenu(RichBoxContent content, City city, bool upgradeOnly = false, bool newLine = true, bool includeAvailable = true, bool includeLevel = true)
        {
            if (upgradeFrom != null && !upgradeOnly)
            {
                upgradeFrom.toMenu(content, city, newLine);
                newLine = true;
            }

            if (newLine)
            {
                content.newLine();
            }

            bool first = true;
            bool available = false;
            foreach (var r in resources)
            {
                if (city != null)
                {
                    available = city.GetGroupedResource(r.type).amount >= r.amount;
                }
                addResources(r.amount, ResourceLib.Icon(r.type), LangLib.Item(r.type), available);
                first = false;
            }

            void addResources(int count, SpriteName sprite, string name, bool available)
            {
                if (count > 0)
                {
                    if (!first)
                    {
                        content.Add(new RbImage(SpriteName.pjNumPlus));
                    }

                    var countText = new RbText(count.ToString());
                    if (includeAvailable)
                    {
                        content.Add(new RbImage(available ? SpriteName.warsResourceChunkAvailable : SpriteName.warsResourceChunkNotAvailable));
                        content.space(0.5f);
                        countText.overrideColor = available ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                    }

                    content.Add(countText);
                    content.hspace();
                    content.Add(new RbImage(sprite));
                    content.hspace();
                    content.Add(new RbText(TextLib.LargeFirstLetter(name)));
                }
            }
            

            if (resultType != CraftResultType.NoSet)
            {
                if (resources.Length > 1)
                {
                    content.newLine();
                }

                var arrow = new RbImage(SpriteName.pjNumArrowR);
                arrow.color = Color.CornflowerBlue;
                content.Add(arrow);
                
                
                content.hspace();
                if (resultType == CraftResultType.Building)
                {
                    if (resultAmount1 > 1)
                    {
                        content.Add(new RbImage((SpriteName)((int)SpriteName.WarsUnitLevelMinimal + resultAmount1 - 1)));
                    }
                }
                else
                {
                    content.hspace();
                    content.Add(new RbText(resultAmount1.ToString()));
                    content.hspace();
                }
                content.Add(new RbImage(icon()));
                content.space();
                content.Add(new RbText(name()));

                if (resultSubType2 >= 0)
                {
                    
                }
            }
            if (includeLevel && experienceType != WorkExperienceType.NONE)
            {
                content.newLine();
                HudLib.Label(content, DssRef.lang.Experience_Required);
                content.newLine();

                bool gotskill = city.cityExperienceLevels.Get(experienceType).Max() >= levelRequirement;
                content.Add(new RbImage(gotskill ? SpriteName.warsResourceChunkAvailable : SpriteName.warsResourceChunkNotAvailable));

                LangLib.ExperienceType(experienceType, out string expName, out SpriteName expIcon);
                content.Add(new RbImage(expIcon));
                content.space();
                var expText = new RbText(expName);
                content.Add(expText);
                content.space();

                content.Add(new RbImage(LangLib.ExperienceLevelIcon(levelRequirement)));
                content.space();
                var levelText = new RbText(LangLib.ExperienceLevel(levelRequirement));
                levelText.overrideColor = HudLib.TitleColor_TypeName;
            
                content.Add(levelText);
                if (newLine)
                {
                    content.newLine();
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

            if (requirement != BuildAndExpandType.NUM_NONE)
            {
                if (content != null)
                {
                    content.newLine();
                    HudLib.Label(content, DssRef.lang.Hud_PurchaseTitle_Requirement);
                    content.newLine();
                    HudLib.BulletPoint(content);
                }
                
                IconName.Building(requirement, out SpriteName icon, out string reqText);
                available = city.buildingStructure.getCount(requirement) > 0;
                

                if (content != null)
                {
                    content.Add(new RbImage(available ? SpriteName.warsResourceChunkAvailable : SpriteName.warsResourceChunkNotAvailable));

                    content.Add(new RbImage(icon));
                    content.space();
                    RbText requirement1 = new RbText(reqText);
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

    //enum CraftRequirement
    //{
    //    None = 0,
    //    Carpenter,
    //    Brewery,
    //    Smelter,
    //    Smith,
    //    ArmorSmith,
    //    Foundry,
    //    CoalPit,
    //    CoinMaker,
    //    Chemist,
    //    Gunmaker,
    //    Logistics1,
    //    Logistics2,
    //    Minter
    //}

    enum CraftResultType
    {
        Resource,
        Building,
        NoSet,
    }
}
