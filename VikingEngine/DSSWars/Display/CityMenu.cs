using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Linq;
using Valve.Steamworks;
using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.Graphics;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.Map;
using VikingEngine.PJ;
using VikingEngine.ToGG;
using VikingEngine.ToGG.MoonFall;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Display
{
    class CityMenu
    {
        public static readonly List<MenuTab> Tabs = new List<MenuTab>() { 
            MenuTab.Info, MenuTab.Resources, MenuTab.Work, MenuTab.BlackMarket, MenuTab.Build, MenuTab.Delivery, MenuTab.Conscript, MenuTab.Tag, MenuTab.Mix };
        Players.LocalPlayer player;
        City city;
        static readonly int[] StockPileControls = { 100, 1000 };
        public CityMenu(Players.LocalPlayer player, City city, RichBoxContent content)
        {
            this.player = player;
            this.city = city;

            content.newLine();
            
            int tabSel = 0;

            var tabs = new List<RichboxTabMember>((int)MenuTab.NUM);

            List<MenuTab> availableTabs =player.AvailableCityTabs();
            for (int i = 0; i < availableTabs.Count; ++i)
            {
                var text = new RichBoxText(LangLib.Tab(availableTabs[i], out string description));
                text.overrideColor = HudLib.RbSettings.tabSelected.Color;

                AbsRbAction enter = null;
                if (description != null)
                {
                    enter = new RbAction(() =>
                    {
                        RichBoxContent content = new RichBoxContent();
                        content.text(description).overrideColor = HudLib.InfoYellow_Light;

                        player.hud.tooltip.create(player, content, true);
                    });
                }

                tabs.Add(new RichboxTabMember(new List<AbsRichBoxMember>
                    {
                        text
                    }, enter));

                if (availableTabs[i] == player.cityTab)
                {
                    tabSel = i;
                }
            }

            content.Add(new RichboxTabgroup(tabs, tabSel, player.cityTabClick, null, SoundLib.menutab, null, null));

            content.newLine();

            switch (player.cityTab)
            { 
                case MenuTab.Info:
                    city.CityDetailsHud(false, player, content);
                    purchaseOptions(content);
                    break;

                case MenuTab.Tag:
                    tagsToMenu(content);
                    break;

                case MenuTab.Work:
                    workTab(content);
                    break;

                case MenuTab.Conscript:
                    conscriptTab(content);
                    break;

                case MenuTab.BlackMarket:
                    BlackMarketResources.ToHud(player, content, city);
                    break;

                case MenuTab.Delivery:
                    deliveryTab(content);
                    break;

                case MenuTab.Resources:
                    resourcesToMenu(content);
                    break;

                case MenuTab.Trade:
                    tradeTab(content);
                    break;

                case MenuTab.Build:
                    player.buildControls.toHud(player, content, city);
                    break;

                case MenuTab.Mix:
                    mixTab(content);
                    break;
            }
        }

        void mixTab(RichBoxContent content)
        {
            //if (player.tutorial == null || player.tutorial.DisplayStockpile())
            {
                for (ResourcesSubTab resourcesSubTab = 0; resourcesSubTab <= ResourcesSubTab.Overview_Armor; ++resourcesSubTab)
                {
                    var tabContent = new RichBoxContent();
                    //string text = null;
                    switch (resourcesSubTab)
                    {
                        case ResourcesSubTab.Overview_Resources:
                            tabContent.Add(new RichBoxText(DssRef.lang.Resource_Tab_Overview));
                            tabContent.space();
                            tabContent.Add(new RichBoxImage(SpriteName.WarsResource_Wood));
                            break;

                        case ResourcesSubTab.Overview_Metals:
                            tabContent.Add(new RichBoxImage(SpriteName.WarsResource_Iron));
                            break;

                        case ResourcesSubTab.Overview_Weapons:
                            tabContent.Add(new RichBoxImage(SpriteName.WarsResource_Sword));
                            break;

                        case ResourcesSubTab.Overview_Projectile:
                            tabContent.Add(new RichBoxImage(SpriteName.WarsResource_Bow));
                            break;

                        case ResourcesSubTab.Overview_Armor:
                            tabContent.Add(new RichBoxImage(SpriteName.cmdMailArmor));
                            break;


                    }
                    var subTab = new RichboxButton(tabContent,
                        new RbAction1Arg<ResourcesSubTab>((ResourcesSubTab resourcesSubTab) =>
                        {
                            player.resourcesSubTab = resourcesSubTab;
                        }, resourcesSubTab, SoundLib.menutab));
                    subTab.setGroupSelectionColor(HudLib.RbSettings, player.resourcesSubTab == resourcesSubTab);
                    content.Add(subTab);
                    content.space();
                }
                content.newParagraph();
            }

            bool reachedBuffer = false;

            switch (player.resourcesSubTab)
            {
                case ResourcesSubTab.Overview_Resources:
                    mixResource(ItemResourceType.Wood_Group, false, WorkPriorityType.wood, SpriteName.WarsWorkCollect);
                    mixResource(ItemResourceType.Stone_G, false, WorkPriorityType.wood, SpriteName.WarsWorkCollect);
                    mixResource(ItemResourceType.RawFood_Group, false, WorkPriorityType.wood, SpriteName.WarsWorkCollect);
                    mixResource(ItemResourceType.SkinLinen_Group, false, WorkPriorityType.farmlinen, SpriteName.WarsWorkFarm);
                    break;
            }

            void mixResource(ItemResourceType item, bool safeGuard, WorkPriorityType workPriorityType, SpriteName prioIcon)
            {
                content.newLine();

                var typeIcon = ResourceLib.Icon(item);
                var typeName = LangLib.Item(item);
                var city_res = city.GetGroupedResource(item);

                var infoContent = new List<AbsRichBoxMember>(2);
                infoContent.Add(new RichBoxImage(typeIcon));
                var amountText = new RichBoxText(city_res.amount.ToString());
                amountText.overrideColor = Color.White;
                infoContent.Add(amountText);

                var infoButton = new RichboxButton(infoContent, null, new RbAction(() =>
                {
                    RichBoxContent content = new RichBoxContent();
                    content.Add(new RichBoxText(typeName));
                    player.hud.tooltip.create(player, content, true);
                }));
                infoButton.overrideBgColor = HudLib.InfoYellow_BG;

                content.Add(infoButton);
                content.space();
                content.Add(new RichBoxTab(0.25f));

                if (item != ItemResourceType.Water_G &&
                   item != ItemResourceType.Gold &&
                   item != ItemResourceType.Men)
                {
                    var stockpileContent = new List<AbsRichBoxMember>(2);
                    stockpileContent.Add(new RichBoxText(city_res.goalBuffer.ToString()));

                    bool reached = city_res.amount >= city_res.goalBuffer;
                    reachedBuffer |= reached;
                    SpriteName stockIcon;
                    if (safeGuard)
                    {
                        stockIcon = SpriteName.WarsStockpileAdd_Protected;
                    }
                    else if (reached)
                    {
                        stockIcon = SpriteName.WarsStockpileStop;
                    }
                    else
                    {
                        stockIcon = SpriteName.WarsStockpileAdd;
                    }
                    var icon = new RichBoxImage(stockIcon);
                    stockpileContent.Add(icon);


                    var stockpileButton = new RichboxButton(stockpileContent, new RbAction(() =>
                    {
                        player.mixTabEditType = MixTabEditType.Stockpile;
                        player.mixTabItem = item;
                    }, SoundLib.menu));

                    content.Add(stockpileButton);
                }

                if (workPriorityType != WorkPriorityType.NUM_NONE)
                {

                }

                if (player.mixTabEditType != MixTabEditType.None &&
                    item == player.mixTabItem)
                {
                    content.newLine();
                    switch (player.mixTabEditType)
                    {
                        case MixTabEditType.Stockpile:
                            stockPileEdit(content, item, city_res);
                            break;
                } }
            }
        }
        void workTab(RichBoxContent content)
        {
            if (player.tutorial == null)
            {
                for (WorkSubTab workSubTab = 0; workSubTab < WorkSubTab.NUM; ++workSubTab)
                {
                    var tabContent = new RichBoxContent();
                    //string text = null;
                    switch (workSubTab)
                    {
                        case WorkSubTab.Priority_Resources:
                            tabContent.Add(new RichBoxText(DssRef.lang.Work_OrderPrioTitle));
                            tabContent.Add(new RichBoxImage(SpriteName.WarsResource_Wood));
                            break;

                        case WorkSubTab.Priority_Metals:
                            tabContent.Add(new RichBoxImage(SpriteName.WarsResource_Iron));
                            break;
                        case WorkSubTab.Priority_Weapons:
                            tabContent.Add(new RichBoxImage(SpriteName.WarsResource_Sword));
                            break;
                        case WorkSubTab.Priority_Armor:
                            tabContent.Add(new RichBoxImage(SpriteName.WarsResource_IronArmor));
                            break;

                        case WorkSubTab.Experience:
                            tabContent.Add(new RichBoxText(DssRef.todoLang.Experience_Title));
                            break;
                    }
                    var subTab = new RichboxButton(tabContent,
                        new RbAction1Arg<WorkSubTab>((WorkSubTab resourcesSubTab) =>
                        {
                            player.workSubTab = resourcesSubTab;
                        }, workSubTab, SoundLib.menutab));
                    subTab.setGroupSelectionColor(HudLib.RbSettings, player.workSubTab == workSubTab);
                    content.Add(subTab);
                    content.space(workSubTab== WorkSubTab.Priority_Armor ? 2 : 1);
                }
                content.newParagraph();
            }

            switch (player.workSubTab)
            {
                default:
                    city.workTemplate.toHud(player, content, player.workSubTab, city.faction, city);
                    break;
                case WorkSubTab.Experience:
                    experienceTab(content);
                    break;
            }

            
        }

        void experienceTab(RichBoxContent content)
        {
            HudLib.Label(content, DssRef.todoLang.Experience_TopExperience);
            experience(SpriteName.WarsWorkFarm, DssRef.todoLang.ExperienceType_Farm, city.topskill_Farm);
            experience(SpriteName.WarsBuild_PigPen, DssRef.todoLang.ExperienceType_AnimalCare, city.topskill_AnimalCare);
            experience(SpriteName.WarsHammer, DssRef.todoLang.ExperienceType_HouseBuilding, city.topskill_HouseBuilding);
            experience(SpriteName.WarsResource_Wood, DssRef.todoLang.ExperienceType_WoodCutter, city.topskill_WoodCutter);
            experience(SpriteName.WarsResource_Stone, DssRef.todoLang.ExperienceType_StoneCutter, city.topskill_StoneCutter);
            experience(SpriteName.WarsWorkMine, DssRef.todoLang.ExperienceType_Mining, city.topskill_Mining);
            experience(SpriteName.WarsWorkMove, DssRef.todoLang.ExperienceType_Transport, city.topskill_Transport);
            experience(SpriteName.WarsResource_Food, DssRef.todoLang.ExperienceType_Cook, city.topskill_Cook);
            experience(SpriteName.WarsWoodCraftIcon, DssRef.todoLang.ExperienceType_Fletcher, city.topskill_Fletcher);
            experience(SpriteName.WarsResource_Iron, DssRef.todoLang.ExperienceType_CraftMetal, city.topskill_CraftMetal);
            experience(SpriteName.WarsResource_IronArmor, DssRef.todoLang.ExperienceType_CraftArmor, city.topskill_CraftArmor);
            experience(SpriteName.WarsResource_Sword, DssRef.todoLang.ExperienceType_CraftWeapon, city.topskill_CraftWeapon);
            experience(SpriteName.WarsResource_Fuel, DssRef.todoLang.ExperienceType_CraftFuel, city.topskill_CraftFuel);
            experience(SpriteName.WarsBuild_Chemist, DssRef.todoLang.ExperienceType_Chemist, city.topskill_Chemistry);
            //experience(SpriteName.WarsResource_GunPowder, DssRef.todoLang.BuildingType_Gunmaker, city.);
            content.newParagraph();
            HudLib.Description(content, string.Format(DssRef.todoLang.Experience_TimeReductionDescription, MathExt.PercentageInteger(DssConst.XpLevelWorkTimePercReduction)));

            content.newParagraph();
            content.Add(new RichBoxBeginTitle());
            var prioTitle = new RichBoxText( DssRef.todoLang.ExperenceOrDistancePrio_Title);
            prioTitle.overrideColor = HudLib.TitleColor_Label;
            content.Add(prioTitle);
            content.space();
            HudLib.InfoButton(content, new RbAction(() =>
            {
                RichBoxContent content = new RichBoxContent();
                content.text(DssRef.todoLang.ExperenceOrDistancePrio_Description);
                player.hud.tooltip.create(player, content, true);
            }));

            content.newLine();
            for (ExperenceOrDistancePrio prio = 0; prio < ExperenceOrDistancePrio.NUM; ++prio)
            {
                string text = null;
                switch (prio)
                {
                    case ExperenceOrDistancePrio.Distance:
                        text = DssRef.todoLang.Hud_Distance;
                        break;
                    case ExperenceOrDistancePrio.Mix:
                        text = DssRef.todoLang.Hud_Mixed;
                        break;
                    case ExperenceOrDistancePrio.Experience:
                        text = DssRef.todoLang.Experience_Title;
                        break;

                }
                var option = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(text) },
                    new RbAction1Arg<ExperenceOrDistancePrio>((ExperenceOrDistancePrio val) =>
                    {
                        city.experenceOrDistance = val;
                    }, prio, SoundLib.menu));
                option.setGroupSelectionColor(HudLib.RbSettings, city.experenceOrDistance == prio);
                content.Add(option);
                content.space();
            }
            
        

            void experience(SpriteName typeIcon, string typeName, ExperienceLevel level)
            {
                content.newLine();
                content.Add(new RichBoxImage(typeIcon));
                content.space();
                var typeNameText = new RichBoxText(typeName + ":");
                typeNameText.overrideColor = HudLib.TitleColor_TypeName;
                content.Add(typeNameText);

                content.Add(new RichBoxTab(0.4f));
                content.Add(new RichBoxImage(LangLib.ExperienceLevelIcon(level)));
                content.Add(new RichBoxText(LangLib.ExperienceLevel(level)));
            }
        }

        public void tagsToMenu(RichBoxContent content)
        {
            content.newLine();
            content.Add(new RichboxCheckbox(new List<AbsRichBoxMember> { new RichBoxText(DssRef.lang.Tag_ViewOnMap) }, player.CityTagsOnMapProperty));
            content.newParagraph();

            for (CityTagBack back = CityTagBack.NONE; back < CityTagBack.NUM; back++)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember> {
                    new RichBoxImage(Data.CityTag.BackSprite(back))
                }, new RbAction1Arg<CityTagBack>((CityTagBack back) => { city.tagBack = back; }, back));
                button.setGroupSelectionColor(HudLib.RbSettings, back == city.tagBack);
                content.Add(button);

                if (back == CityTagBack.NONE)
                {
                    content.newLine();
                }
                else
                {
                    content.space();
                }
            }

            if (city.tagBack != CityTagBack.NONE)
            {
                content.newParagraph();
                for (CityTagArt art = CityTagArt.None; art < CityTagArt.NUM; art++)
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember> {
                    new RichBoxImage(Data.CityTag.ArtSprite(art))
                    }, new RbAction1Arg<CityTagArt>((CityTagArt art) => { city.tagArt = art; }, art));
                    button.setGroupSelectionColor(HudLib.RbSettings, art == city.tagArt);
                    content.Add(button);
                    content.space();
                }
            }
        }

        public void resourcesToMenu(RichBoxContent content)
        {
            if (player.tutorial == null || player.tutorial.DisplayStockpile())
            {
                for (ResourcesSubTab resourcesSubTab = 0; resourcesSubTab < ResourcesSubTab.NUM; ++resourcesSubTab)
                {
                    var tabContent = new RichBoxContent();
                    //string text = null;
                    switch (resourcesSubTab)
                    {
                        case ResourcesSubTab.Overview_Resources:
                            tabContent.Add(new RichBoxText(DssRef.lang.Resource_Tab_Overview));//text = DssRef.lang.Resource_Tab_Overview;
                            tabContent.space();
                            tabContent.Add(new RichBoxImage(SpriteName.WarsResource_Wood));
                            break;

                        case ResourcesSubTab.Overview_Metals:
                        case ResourcesSubTab.Stockpile_Metals:
                            tabContent.Add(new RichBoxImage(SpriteName.WarsResource_Iron));
                            break;
                        case ResourcesSubTab.Overview_Weapons:
                        case ResourcesSubTab.Stockpile_Weapons:
                            tabContent.Add(new RichBoxImage(SpriteName.WarsResource_Sword));
                            break;

                        case ResourcesSubTab.Overview_Projectile:
                        case ResourcesSubTab.Stockpile_Projectile:
                            tabContent.Add(new RichBoxImage(SpriteName.WarsResource_Bow));
                            break;

                        case ResourcesSubTab.Overview_Armor:
                        case ResourcesSubTab.Stockpile_Armor:
                            tabContent.Add(new RichBoxImage(SpriteName.cmdMailArmor));
                            break;

                        case ResourcesSubTab.Stockpile_Resources:
                            //text = DssRef.lang.Resource_Tab_Stockpile;
                            tabContent.Add(new RichBoxText(DssRef.lang.Resource_Tab_Stockpile));//text = DssRef.lang.Resource_Tab_Overview;
                            tabContent.space();
                            tabContent.Add(new RichBoxImage(SpriteName.WarsResource_Wood));
                            break;
                    }
                    var subTab = new RichboxButton(tabContent,
                        new RbAction1Arg<ResourcesSubTab>((ResourcesSubTab resourcesSubTab) =>
                        {
                            player.resourcesSubTab = resourcesSubTab;
                        }, resourcesSubTab, SoundLib.menutab));
                    subTab.setGroupSelectionColor(HudLib.RbSettings, player.resourcesSubTab == resourcesSubTab);
                    content.Add(subTab);
                    content.space();
                }
                content.newParagraph();
            }

            bool reachedBuffer = false;

            switch (player.resourcesSubTab)
            {
                case ResourcesSubTab.Overview_Resources:
                    //content.h1(DssRef.lang.MenuTab_Resources).overrideColor = HudLib.TitleColor_Label;
                    //content.newLine();

                    content.Add(new RichBoxImage(SpriteName.WarsResource_Water));
                    content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Water + ": " + string.Format(DssRef.lang.Language_CollectProgress, city.res_water.amount, city.maxWaterTotal)));
                    content.Add(new RichBoxTab(0.4f));
                    content.Add(new RichBoxImage(SpriteName.WarsResource_WaterAdd));
                    content.Add(new RichBoxText(TextLib.OneDecimal(city.waterAddPerSec)));

                    var foodSafeGuard = city.foodSafeGuardIsActive(out bool fuelSafeGuard, out bool rawFoodSafeGuard, out bool woodSafeGuard);

                    city.res_wood.toMenu(content, ItemResourceType.Wood_Group, woodSafeGuard, ref reachedBuffer);
                    city.res_stone.toMenu(content, ItemResourceType.Stone_G, false, ref reachedBuffer);
                    city.res_rawFood.toMenu(content, ItemResourceType.RawFood_Group, rawFoodSafeGuard, ref reachedBuffer);
                    city.res_skinLinnen.toMenu(content, ItemResourceType.SkinLinen_Group, false, ref reachedBuffer);
                    content.newParagraph();


                    city.res_food.toMenu(content, ItemResourceType.Food_G, foodSafeGuard, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Food1, CraftResourceLib.Food2);
                    content.space();
                    content.Add(new RichboxButton(new List<AbsRichBoxMember> {
                            new RichBoxImage(city.res_food_safeguard? SpriteName.WarsProtectedStockpileOn : SpriteName.WarsProtectedStockpileOff),
                        },
                    new RbAction(() =>{
                        city.res_food_safeguard = !city.res_food_safeguard;
                    }),
                    new RbAction(() =>
                    {
                        RichBoxContent content = new RichBoxContent();
                        content.text(string.Format(DssRef.lang.Resource_FoodSafeGuard_Description, DssConst.WorkSafeGuardAmount)).overrideColor = HudLib.InfoYellow_Light;
                        content.text(city.res_food_safeguard? DssRef.lang.Hud_On : DssRef.lang.Hud_Off);
                        player.hud.tooltip.create(player, content, true);
                    })));


                    city.res_fuel.toMenu(content, ItemResourceType.Fuel_G, fuelSafeGuard, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Fuel1, null, true);
                    content.space();
                    blueprintButton(player, content, CraftResourceLib.Charcoal);

                    city.res_beer.toMenu(content, ItemResourceType.Beer, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Beer);

                    city.res_coolingfluid.toMenu(content, ItemResourceType.CoolingFluid, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.CoolingFluid);
                    content.newParagraph();

                    city.res_Toolkit.toMenu(content, ItemResourceType.Toolkit, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Toolkit);

                    city.res_Wagon2Wheel.toMenu(content, ItemResourceType.Wagon2Wheel, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.WagonLight);

                    city.res_Wagon4Wheel.toMenu(content, ItemResourceType.Wagon4Wheel, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.WagonHeavy);

                    city.res_BlackPowder.toMenu(content, ItemResourceType.BlackPowder, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.BlackPowder);

                    city.res_GunPowder.toMenu(content, ItemResourceType.GunPowder, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.GunPowder);

                    city.res_LedBullet.toMenu(content, ItemResourceType.LedBullet, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.LedBullets);

                    
                   

                    content.Add(new RichBoxSeperationLine());
                    GroupedResource.BufferIconInfo(content, false);
                    if (foodSafeGuard)
                    {
                        GroupedResource.BufferIconInfo(content, true);
                    }
                    ResourceLib.ConvertGoldOre.toMenu(content, city);
                    {
                        content.Add(new RichBoxText(1.ToString()));
                        content.Add(new RichBoxImage(ResourceLib.Icon(ItemResourceType.Food_G)));
                        content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Food));
                        var arrow = new RichBoxImage(SpriteName.pjNumArrowR);
                        arrow.color = Color.CornflowerBlue;
                        content.Add(arrow);
                        content.Add(new RichBoxText(string.Format(DssRef.lang.Hud_EnergyAmount,DssRef.difficulty.FoodEnergySett)));
                    }
                    break;

                case ResourcesSubTab.Overview_Metals:

                    city.res_ironore.toMenu(content, ItemResourceType.IronOre_G, false, ref reachedBuffer);
                    city.res_TinOre.toMenu(content, ItemResourceType.TinOre, false, ref reachedBuffer);
                    city.res_CupperOre.toMenu(content, ItemResourceType.CupperOre, false, ref reachedBuffer);
                    city.res_LeadOre.toMenu(content, ItemResourceType.LeadOre, false, ref reachedBuffer);
                    city.res_SilverOre.toMenu(content, ItemResourceType.SilverOre, false, ref reachedBuffer);
                    content.newParagraph();


                    city.res_iron.toMenu(content, ItemResourceType.Iron_G, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Iron, CraftResourceLib.Iron_AndCooling);

                    city.res_Tin.toMenu(content, ItemResourceType.Tin, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Tin);

                    city.res_Cupper.toMenu(content, ItemResourceType.Cupper, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Cupper, CraftResourceLib.Cupper_AndCooling);

                    city.res_Lead.toMenu(content, ItemResourceType.Lead, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Lead);

                    city.res_Silver.toMenu(content, ItemResourceType.Silver, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Silver, CraftResourceLib.Silver_AndCooling);

                    city.res_RawMithril.toMenu(content, ItemResourceType.RawMithril, false, ref reachedBuffer);
                    content.newParagraph();


                    city.res_Bronze.toMenu(content, ItemResourceType.Bronze, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Bronze);

                    city.res_CastIron.toMenu(content, ItemResourceType.CastIron, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.CastIron);

                    city.res_BloomeryIron.toMenu(content, ItemResourceType.BloomeryIron, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.BloomeryIron);
                    
                    city.res_Steel.toMenu(content, ItemResourceType.Steel, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Steel, CraftResourceLib.Steel_AndCooling);

                    city.res_Mithril.toMenu(content, ItemResourceType.Mithril, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Mithril);
                    break;

                case ResourcesSubTab.Overview_Weapons:

                    city.res_sharpstick.toMenu(content, ItemResourceType.SharpStick, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.SharpStick);

                    city.res_BronzeSword.toMenu(content, ItemResourceType.BronzeSword, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.BronzeSword);

                    city.res_shortsword.toMenu(content, ItemResourceType.ShortSword, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.ShortSword);

                    city.res_Sword.toMenu(content, ItemResourceType.Sword, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Sword);

                    city.res_LongSword.toMenu(content, ItemResourceType.LongSword, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.LongSword);
                    
                    city.res_HandSpear.toMenu(content, ItemResourceType.HandSpear, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.HandSpearIron, CraftResourceLib.HandSpearBronze);
                    
                    content.newParagraph();

                    city.res_Warhammer.toMenu(content, ItemResourceType.Warhammer, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.WarhammerIron, CraftResourceLib.WarhammerBronze);

                    city.res_twohandsword.toMenu(content, ItemResourceType.TwoHandSword, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.TwoHandSword);

                    city.res_knightslance.toMenu(content, ItemResourceType.KnightsLance, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.KnightsLance);

                    city.res_MithrilSword.toMenu(content, ItemResourceType.MithrilSword, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.MithrilSword);
                   
                    break;

                case ResourcesSubTab.Overview_Projectile:

                    city.res_SlingShot.toMenu(content, ItemResourceType.SlingShot, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Slingshot);

                    city.res_ThrowingSpear.toMenu(content, ItemResourceType.ThrowingSpear, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.ThrowingSpear1, CraftResourceLib.ThrowingSpear2);

                    city.res_bow.toMenu(content, ItemResourceType.Bow, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Bow);

                    city.res_longbow.toMenu(content, ItemResourceType.LongBow, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.LongBow);

                    city.res_crossbow.toMenu(content, ItemResourceType.Crossbow, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.CrossBow);

                    city.res_MithrilBow.toMenu(content, ItemResourceType.MithrilBow, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.MithrilBow);


                    city.res_HandCannon.toMenu(content, ItemResourceType.HandCannon, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.BronzeHandCannon);

                    city.res_HandCulvertin.toMenu(content, ItemResourceType.HandCulverin, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.BronzeHandCulverin);

                    city.res_Rifle.toMenu(content, ItemResourceType.Rifle, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Rifle);

                    city.res_Blunderbus.toMenu(content, ItemResourceType.Blunderbus, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Blunderbus);
                    content.newParagraph();

                    city.res_ballista.toMenu(content, ItemResourceType.Ballista, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Ballista);

                    city.res_Manuballista.toMenu(content, ItemResourceType.Manuballista, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.ManuBallista);

                    city.res_Catapult.toMenu(content, ItemResourceType.Catapult, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.Catapult);

                    city.res_SiegeCannonBronze.toMenu(content, ItemResourceType.SiegeCannonBronze, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.SiegeCannonBronze);

                    city.res_ManCannonBronze.toMenu(content, ItemResourceType.ManCannonBronze, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.ManCannonBronze);

                    city.res_SiegeCannonIron.toMenu(content, ItemResourceType.SiegeCannonIron, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.SiegeCannonIron);

                    city.res_SiegeCannonIron.toMenu(content, ItemResourceType.ManCannonIron, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.ManCannonIron);

                    break;

                case ResourcesSubTab.Overview_Armor:

                    city.res_paddedArmor.toMenu(content, ItemResourceType.PaddedArmor, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.PaddedArmor);

                    city.res_HeavyPaddedArmor.toMenu(content, ItemResourceType.HeavyPaddedArmor, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.HeavyPaddedArmor);
                    
                    city.res_BronzeArmor.toMenu(content, ItemResourceType.BronzeArmor, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.BronzeArmor);

                    city.res_mailArmor.toMenu(content, ItemResourceType.IronArmor, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.MailArmor);

                    city.res_heavyMailArmor.toMenu(content, ItemResourceType.HeavyIronArmor, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.HeavyMailArmor);

                    city.res_LightPlateArmor.toMenu(content, ItemResourceType.LightPlateArmor, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.PlateArmor);

                    city.res_FullPlateArmor.toMenu(content, ItemResourceType.FullPlateArmor, false, ref reachedBuffer);
                    blueprintButton(player, content, CraftResourceLib.FullPlateArmor);
                    break;

                case ResourcesSubTab.Stockpile_Resources:
                    //content.h1(DssRef.lang.Resource_Tab_Stockpile).overrideColor = HudLib.TitleColor_Label;

                    stockpile(ItemResourceType.Wood_Group);
                    stockpile(ItemResourceType.Stone_G);
                    stockpile(ItemResourceType.RawFood_Group);
                    stockpile(ItemResourceType.SkinLinen_Group);
                    content.newParagraph();

                    stockpile(ItemResourceType.Food_G);
                    stockpile(ItemResourceType.Fuel_G);
                    stockpile(ItemResourceType.Beer);
                    stockpile(ItemResourceType.CoolingFluid);
                    content.newParagraph();

                    stockpile(ItemResourceType.Toolkit);
                    stockpile(ItemResourceType.Wagon2Wheel);
                    stockpile(ItemResourceType.Wagon4Wheel);
                    stockpile(ItemResourceType.BlackPowder);
                    stockpile(ItemResourceType.GunPowder);
                    stockpile(ItemResourceType.LedBullet);

                    content.newParagraph();
                    HudLib.Description(content, DssRef.lang.Resource_StockPile_Info);
                    GroupedResource.BufferIconInfo(content, false);
                    break;

                case ResourcesSubTab.Stockpile_Metals:
                    stockpile(ItemResourceType.IronOre_G);
                    stockpile(ItemResourceType.TinOre);
                    stockpile(ItemResourceType.CupperOre);
                    stockpile(ItemResourceType.LeadOre);
                    stockpile(ItemResourceType.SilverOre);
                    content.newParagraph();

                    stockpile(ItemResourceType.Iron_G);
                    stockpile(ItemResourceType.Tin);
                    stockpile(ItemResourceType.Cupper);
                    stockpile(ItemResourceType.Lead);
                    stockpile(ItemResourceType.Silver);
                    stockpile(ItemResourceType.RawMithril);
                    content.newParagraph();

                    stockpile(ItemResourceType.Bronze);
                    stockpile(ItemResourceType.CastIron);
                    stockpile(ItemResourceType.BloomeryIron);
                    stockpile(ItemResourceType.Steel);
                    stockpile(ItemResourceType.Mithril);

                    break;
                case ResourcesSubTab.Stockpile_Weapons:
                    stockpile(ItemResourceType.BronzeSword);
                    stockpile(ItemResourceType.ShortSword);
                    stockpile(ItemResourceType.Sword);
                    stockpile(ItemResourceType.LongSword);
                    stockpile(ItemResourceType.HandSpear);
                    content.newParagraph();

                    stockpile(ItemResourceType.Warhammer);
                    stockpile(ItemResourceType.TwoHandSword);
                    stockpile(ItemResourceType.KnightsLance);
                    stockpile(ItemResourceType.MithrilSword);
                    
                    break;

                case ResourcesSubTab.Stockpile_Projectile:
                    
                    stockpile(ItemResourceType.SlingShot);
                    stockpile(ItemResourceType.ThrowingSpear);
                    stockpile(ItemResourceType.Bow);
                    stockpile(ItemResourceType.LongBow);
                    stockpile(ItemResourceType.Crossbow);
                    stockpile(ItemResourceType.MithrilBow);
                    content.newParagraph();

                    stockpile(ItemResourceType.HandCannon);
                    stockpile(ItemResourceType.HandCulverin);
                    stockpile(ItemResourceType.Rifle);
                    stockpile(ItemResourceType.Blunderbus);

                    content.newParagraph();

                    stockpile(ItemResourceType.Ballista);
                    stockpile(ItemResourceType.Manuballista);
                    stockpile(ItemResourceType.Catapult);

                    stockpile(ItemResourceType.SiegeCannonBronze);
                    stockpile(ItemResourceType.ManCannonBronze);
                    stockpile(ItemResourceType.SiegeCannonIron);
                    stockpile(ItemResourceType.ManCannonIron);

                    break;

                case ResourcesSubTab.Stockpile_Armor:
                    stockpile(ItemResourceType.HeavyPaddedArmor);
                    stockpile(ItemResourceType.PaddedArmor);
                    stockpile(ItemResourceType.IronArmor);
                    stockpile(ItemResourceType.HeavyIronArmor);
                    stockpile(ItemResourceType.LightPlateArmor);
                    stockpile(ItemResourceType.FullPlateArmor);
                    break;
            }

            void stockpile(ItemResourceType item)
            {
                


                GroupedResource res = city.GetGroupedResource(item);

                content.newLine();
                var icon = new RichBoxImage(res.amount >= res.goalBuffer ? SpriteName.WarsStockpileStop : SpriteName.WarsStockpileAdd);
                //if (res.amount >= res.goalBuffer)
                //{
                //    icon.color = Color.OrangeRed;
                //}
                content.Add(icon);
                content.Add(new RichBoxImage(ResourceLib.Icon(item)));
                content.space();
                //content.Add(new RichBoxText(LangLib.Item(item) + ": "));

                //content.newLine();
                stockPileEdit(content, item, res);
            }
        }

        void stockPileEdit(RichBoxContent content, ItemResourceType item, GroupedResource res)
        {
            RbAction hover = new RbAction(() => {
                RichBoxContent content = new RichBoxContent();
                bool buffer = false;
                city.GetGroupedResource(item).toMenu(content, item, false, ref buffer);//content.Add(new RichBoxText(LangLib.Item(item)));
                player.hud.tooltip.create(player, content, true);
            });

            for (int i = StockPileControls.Length - 1; i >= 0; i--)
            {
                int change = -StockPileControls[i];
                content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(TextLib.PlusMinus(change)) },
                    new RbAction1Arg<int>((int change) => {
                        var res = city.GetGroupedResource(item);
                        res.goalBuffer = Bound.Set(res.goalBuffer + change, DssConst.StockPileMinBound, DssConst.StockPileMaxBound);
                        city.SetGroupedResource(item, res);

                    }, change, SoundLib.menu), hover));

                content.space();
            }

            content.Add(new RichBoxText(res.goalBuffer.ToString()));

            for (int i = 0; i < StockPileControls.Length; i++)
            {
                content.space();

                int change = StockPileControls[i];
                content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(TextLib.PlusMinus(change)) },
                    new RbAction1Arg<int>((int change) => {
                        var res = city.GetGroupedResource(item);
                        res.goalBuffer = Bound.Set(res.goalBuffer + change, DssConst.StockPileMinBound, DssConst.StockPileMaxBound);
                        city.SetGroupedResource(item, res);

                    }, change, SoundLib.menu), hover));
            }
        }

        void purchaseOptions(RichBoxContent content)
        {
            if (city.battleGroup == null)
            {
                if (city.damages.HasValue())
                {
                    content.newLine();
                    content.Add(new RichboxButton(new List<AbsRichBoxMember>{
                                    new RichBoxImage(SpriteName.unitEmoteLove),
                                    new RichBoxText(DssRef.lang.CityOption_Repair),
                                },
                        new RbAction1Arg<bool>(buyRepairAction, true, SoundLib.menuBuy),
                        new RbAction1Arg<bool>(buyRepairToolTip, true),
                        city.buyRepair(false, true)));
                }

                content.newLine();

                //if (city.battleGroup == null)
                //{
                //    content.Add(new RichboxButton(new List<AbsRichBoxMember>{
                //            new RichBoxImage(SpriteName.birdFireball),
                //            new RichBoxText(DssRef.lang.CityOption_BurnItDown),
                //        },
                //        new RbAction(city.burnItDown, SoundLib.menu),
                //        new RbAction(burnToolTip),
                //         city.damages.value < city.MaxDamages()));

                //    content.newLine();
                //}

                {
                    int count = 1;
                    content.Add(new RichboxButton(new List<AbsRichBoxMember>{
                                    new RichBoxImage(SpriteName.WarsGuardAdd),
                                    new RichBoxText( DssRef.lang.CityOption_ExpandGuardSize),
                                },
                        new RbAction1Arg<int>(buyCityGuardsAction, count, SoundLib.menuBuy),
                        new RbAction1Arg<int>(buyGuardSizeToolTip, count),
                        city.buyCityGuards(false, count)));
                }
                content.Add(new RichBoxSpace());
                {
                    int count = 5;
                    content.Button(string.Format(DssRef.lang.Hud_XTimes, count),
                    new RbAction1Arg<int>(buyCityGuardsAction, count, SoundLib.menuBuy),
                    new RbAction1Arg<int>(buyGuardSizeToolTip, count),
                    city.buyCityGuards(false, count));
                }

                content.newLine();
                {
                    int count = 1;
                    content.Add(new RichboxButton(new List<AbsRichBoxMember>{
                                    new RichBoxImage(SpriteName.WarsGuard),
                                    new RichBoxText( DssRef.todoLang.CityOption_LowerGuardSize),
                                },
                        new RbAction1Arg<int>(city.releaseGuardSize, count * DssConst.ExpandGuardSize, SoundLib.menuBuy),
                        new RbAction1Arg<int>(releaseGuardSizeToolTip, count),
                        city.canReleaseGuardSize(count)));
                }
                //if (!city.nobelHouse && city.canEverGetNobelHouse())
                //{
                //    content.Button(DssRef.lang.Building_NobleHouse,
                //            new RbAction(city.buyNobelHouseAction, SoundLib.menuBuy),
                //            new RbAction(buyNobelhouseTooltip),
                //            city.canBuyNobelHouse());
                //}
            }
        }

        void blueprintButton(LocalPlayer player, RichBoxContent content, CraftBlueprint blueprint, CraftBlueprint optionalBp = null, bool roomForAnotherButton = false)
        {
            
            content.Add(new RichBoxTab(0.65f));//roomForAnotherButton? 0.65f : 0.8f));

            content.Add(new RichboxButton(new List<AbsRichBoxMember> { 
                new RichBoxImage(SpriteName.WarsBluePrint)
            },
               null, new RbAction2Arg<CraftBlueprint, CraftBlueprint>(blueprintTooltip, blueprint, optionalBp)));

        }

        void blueprintTooltip(CraftBlueprint blueprint, CraftBlueprint optionalBp)
        {
            //hover
            RichBoxContent content = new RichBoxContent();
            content.h2(DssRef.lang.Blueprint_Title).overrideColor = HudLib.TitleColor_TypeName;
            blueprint.toMenu(content, city);
            if (optionalBp != null)
            { 
                content.newLine();
                optionalBp.toMenu(content, city);
            }

            blueprint.requirementToHud(content, city, out _);

            content.Add(new RichBoxSeperationLine());
            content.newParagraph();
            content.h2(DssRef.lang.MenuTab_Resources).overrideColor = HudLib.TitleColor_Label;
            blueprint.listResources(content, city, optionalBp);
            

            player.hud.tooltip.create(player, content, true, blueprint.tooltipId);
        }

        

        void conscriptTab(RichBoxContent content)
        {
            new ConscriptMenu().ToHud(city, player, content);
        }

        void deliveryTab(RichBoxContent content)
        {
            new DeliveryMenu().ToHud(city, player, content);
        }

        //void recruitTab(RichBoxContent content)
        //{            
        //    content.newLine();

        //    ArmyStatus status;
        //    Army recruitArmy = city.recruitToClosestArmy();
        //    if (recruitArmy != null)
        //    {
        //        status = recruitArmy.Status();
        //    }
        //    else
        //    {
        //        status = new ArmyStatus();
        //    }

        //    content.h2(DssRef.lang.UnitType_Recruit);
        //    foreach (var opt in city.cityPurchaseOptions)
        //    {
        //        if (opt.available)
        //        {
        //            content.newLine();

        //            string recruitText = string.Format(DssRef.lang.CityOption_RecruitType, DssRef.profile.Name(opt.unitType));
        //            string count = status.typeCount[(int)opt.unitType].ToString();
        //            AbsSoldierProfile typeData = DssRef.profile.Get(opt.unitType);

        //            content.Add(new RichBoxText(count));
        //            content.Add(new RichBoxImage(typeData.icon));

        //            content.Add(new RichBoxSpace());

        //            content.Add(new RichboxButton(
        //                new List<AbsRichBoxMember>
        //                {
        //                new RichBoxText(recruitText),
        //                },
        //                new RbAction3Arg<UnitType, int, LocalPlayer>(city.buySoldiersAction, opt.unitType, 1, player, SoundLib.menuBuy),
        //                new RbAction2Arg<CityPurchaseOption, int>(buySoldiersTip, opt, 1),
        //                canBuySoldiers(opt.unitType, 1)));

        //            content.space();
        //            multiBuy(5);

        //            content.space();

        //            multiBuy(25);

        //            void multiBuy(int multiCount)
        //            {
        //                content.Button(string.Format(DssRef.lang.Hud_XTimes, multiCount),
        //                    new RbAction3Arg<UnitType, int, LocalPlayer>(city.buySoldiersAction, opt.unitType, multiCount, player, SoundLib.menuBuy),
        //                    new RbAction2Arg<CityPurchaseOption, int>(buySoldiersTip, opt, multiCount),
        //                    canBuySoldiers(opt.unitType, multiCount));
        //            }
        //        }
        //    }

        //    if (!player.inTutorialMode)
        //    {
        //        content.newLine();

        //        content.icontext(SpriteName.WarsSoldierIcon, string.Format(DssRef.lang.CityOption_XMercenaries, TextLib.LargeNumber(city.mercenaries)));

        //        content.newLine();

        //        //string importMecenariesText = "Import {0} mercenaries";

        //        content.Add(new RichboxButton(new List<AbsRichBoxMember>{
        //        new RichBoxImage(SpriteName.WarsSoldierIcon),
        //        new RichBoxText( string.Format(DssRef.lang.CityOption_BuyXMercenaries, DssLib.MercenaryPurchaseCount)),
        //    },
        //            new RbAction1Arg<int>(buyMercenaryAction, 1, SoundLib.menuBuy),
        //            new RbAction1Arg<int>(buyMercenaryToolTip, 1),
        //            city.buyMercenary(false, 1)));

        //        content.Add(new RichBoxSpace());

        //        content.Button((DssLib.MercenaryPurchaseCount * 5).ToString(),
        //            new RbAction1Arg<int>(buyMercenaryAction, 5, SoundLib.menuBuy),
        //            new RbAction1Arg<int>(buyMercenaryToolTip, 5),
        //            city.buyMercenary(false, 5));

        //        content.Add(new RichBoxNewLine(true));


        //        if (city.damages.HasValue())
        //        {
        //            content.Add(new RichboxButton(new List<AbsRichBoxMember>{
        //                            new RichBoxImage(SpriteName.unitEmoteLove),
        //                            new RichBoxText(DssRef.lang.CityOption_Repair),
        //                        },
        //                new RbAction1Arg<bool>(buyRepairAction, true, SoundLib.menuBuy),
        //                new RbAction1Arg<bool>(buyRepairToolTip, true),
        //                city.buyRepair(false, true)));
        //        }
        //        //else
        //        //{
        //        //    content.Add(new RichboxButton(new List<AbsRichBoxMember>{
        //        //            new RichBoxImage(SpriteName.WarsWorkerAdd),
        //        //            new RichBoxText(DssRef.lang.CityOption_ExpandWorkForce),
        //        //        },
        //        //        new RbAction1Arg<int>(buyWorkforceAction, 1, SoundLib.menuBuy),
        //        //        new RbAction1Arg<int>(buyWorkforceToolTip, 1),
        //        //        city.buyWorkforce(false, 1)));
        //        //}


        //        content.newLine();

        //        //if (city.battleGroup == null)
        //        //{
        //        //    content.Add(new RichboxButton(new List<AbsRichBoxMember>{
        //        //            new RichBoxImage(SpriteName.birdFireball),
        //        //            new RichBoxText(DssRef.lang.CityOption_BurnItDown),
        //        //        },
        //        //        new RbAction(city.burnItDown, SoundLib.menu),
        //        //        new RbAction(burnToolTip),
        //        //         city.damages.value < city.MaxDamages()));

        //        //    content.newLine();
        //        //}

        //        {
        //            int count = 1;
        //            content.Add(new RichboxButton(new List<AbsRichBoxMember>{
        //                            new RichBoxImage(SpriteName.WarsGuardAdd),
        //                            new RichBoxText( DssRef.lang.CityOption_ExpandGuardSize),
        //                        },
        //                new RbAction1Arg<int>(buyCityGuardsAction, count, SoundLib.menuBuy),
        //                new RbAction1Arg<int>(buyGuardSizeToolTip, count),
        //                city.buyCityGuards(false, count)));
        //        }
        //        content.Add(new RichBoxSpace());
        //        {
        //            int count = 5;
        //            content.Button(string.Format(DssRef.lang.Hud_XTimes, count),
        //            new RbAction1Arg<int>(buyCityGuardsAction, count, SoundLib.menuBuy),
        //            new RbAction1Arg<int>(buyGuardSizeToolTip, count),
        //            city.buyCityGuards(false, count));
        //        }

        //        content.newLine();

        //        if (!city.nobelHouse && city.canEverGetNobelHouse())
        //        {
        //            content.Button(DssRef.lang.Building_NobleHouse,
        //                    new RbAction(city.buyNobelHouseAction, SoundLib.menuBuy),
        //                    new RbAction(buyNobelhouseTooltip),
        //                    city.canBuyNobelHouse());
        //        }

        //        content.newLine();
        //    }
            
        //}

        void tradeTab(RichBoxContent content)
        {
            city.tradeTemplate.toHud(player,content, city.faction, city);
        }

        //void tabClick(int tab)
        //{
        //    this.player.cityTab = (MenuTab)tab;
        //}

        //bool canBuySoldiers(UnitType unitType, int count)
        //{
        //    Army army;
        //    return city.buySoldiers(unitType, count, false, out army);
        //}
       
        //void buyNobelhouseTooltip()
        //{
        //    RichBoxContent content = new RichBoxContent(); 

        //    if (city.nobelHouse)
        //    {
        //        content.h2(DssRef.lang.Building_IsBuilt);
        //    }
        //    else
        //    {
        //        content.h2(DssRef.lang.Building_BuildAction);
        //        content.newLine();
        //        content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement);
        //        content.newLine();
        //        HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Worker, DssLib.NobelHouseWorkForceReqiurement, city.workForce);
        //        content.newLine();
        //        content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
        //        content.newLine();
        //        HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Gold, DssLib.NobleHouseCost, player.faction.gold);
        //        HudLib.Upkeep(content, Convert.ToInt32(DssLib.NobleHouseUpkeep));
        //        content.newLine();
        //        content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
                
        //    }

        //    content.newLine();

        //    //string addDiplomacy = "1 diplomacy point per {0} seconds";
        //    int diplomacydSec = Convert.ToInt32(DssRef.diplomacy.NobelHouseAddDiplomacy * 3600);
        //    //string addDiplomacyMax = "+{0} to diplomacy point max limit";
        //    //string addCommand = "1 command point per {0} seconds";
        //    //int commandSec = Convert.ToInt32(DssLib.NobelHouseAddCommand * 3600);
        //    //string upkeep = "upkeep +{0}";


        //    HudLib.BulletPoint(content);
        //    content.Add(new RichBoxImage(SpriteName.WarsDiplomaticAddTime));
        //    content.Add(new RichBoxText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsAdd, diplomacydSec)));
        //    content.newLine();

        //    HudLib.BulletPoint(content);
        //    content.Add(new RichBoxImage(SpriteName.WarsDiplomaticPoint));
        //    content.Add(new RichBoxText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsLimit, DssRef.diplomacy.NobelHouseAddMaxDiplomacy)));
        //    content.newLine();

        //    //content.ListDot();
        //    //content.Add(new RichBoxImage(SpriteName.WarsCommandAddTime));
        //    //content.Add(new RichBoxText(string.Format(addCommand, commandSec)));
        //    //content.newLine();

        //    HudLib.BulletPoint(content);
        //    content.Add(new RichBoxText(DssRef.lang.Building_NobleHouse_UnlocksKnight));
        //    content.newLine();

        //    //content.ListDot();
        //    //content.Add(new RichBoxImage(SpriteName.rtsUpkeepTime));
        //    //HudLib.Upkeep(content, Convert.ToInt32(DssLib.NobelHouseUpkeep), true);
        //    //content.Add(new RichBoxText(string.Format(upkeep, DssLib.NobelHouseUpkeep)));
        //    content.newLine();

        //    player.hud.tooltip.create(player, content, true);
        
        
        //}


        //void buyMercenaryAction(int count)
        //{
        //    city.buyMercenary(true, count);
        //}

        //public void buyMercenaryToolTip(int count)
        //{
        //    RichBoxContent content = new RichBoxContent();

        //    int cost = city.buyMercenaryCost(count);

        //    content.text(TextLib.Quote(DssRef.lang.CityOption_Mercenaries_Description));
        //    content.newLine();
        //    content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
        //    content.newLine();
        //    HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Gold, cost, player.faction.gold);
        //    content.text(string.Format(DssRef.lang.Hud_Purchase_CostWillIncreaseByX, DssRef.difficulty.MercenaryPurchaseCost_Add * count));
        //    content.newLine();
        //    HudLib.ResourceCost(content, GameObject.Resource.ResourceType.MercenaryOnMarket, DssLib.MercenaryPurchaseCount * count, player.mercenaryMarket.Int());

        //    content.newParagraph();
        //    content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
        //    content.newLine();
        //    content.icontext(SpriteName.WarsSoldierIcon, string.Format(DssRef.lang.CityOption_XMercenaries, DssLib.MercenaryPurchaseCount * count));
            
        //    player.hud.tooltip.create(player, content, true);
        //}


        //void buyWorkforceAction(int count)
        //{
        //    city.buyWorkforce(true, count);
        //}

        void buyRepairAction(bool all)
        {
            city.buyRepair(true, all);
        }

        //public void buyWorkforceToolTip(int count)
        //{
        //    RichBoxContent content = new RichBoxContent();
        //    if (city.canExpandWorkForce(count))
        //    {
        //        content.text(TextLib.Quote(DssRef.lang.ResourceType_Workers_Description));
        //        content.newLine();
        //        content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
        //        content.newLine();
        //        HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Gold, city.expandWorkForceCost()* count, player.faction.gold);
        //        content.newLine();
        //        content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
        //        content.newLine();
        //        content.icontext(SpriteName.WarsWorkerAdd, string.Format(DssRef.lang.CityOption_ExpandWorkForce_IncreaseMax, DssConst.ExpandWorkForce * count));
        //    }
        //    else 
        //    {
        //        content.Add(new RichBoxText(DssRef.lang.Hud_Purchase_MaxCapacity, Color.Red));
        //    }
        //    player.hud.tooltip.create(player, content, true);
        //}

        public void burnToolTip()
        {
            RichBoxContent content = new RichBoxContent();

            content.text(DssRef.lang.CityOption_BurnItDown_Description);

            player.hud.tooltip.create(player, content, true);
        }

        public void buyRepairToolTip(bool all)
        {
            RichBoxContent content = new RichBoxContent();
            int count, cost;
            city.repairCountAndCost( all, out count, out cost);

            content.text(TextLib.Quote(DssRef.lang.CityOption_Repair_Description));
            content.newLine();
            content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
            content.newLine();
            HudLib.ResourceCost(content, ResourceType.Gold, cost, player.faction.gold);
            content.newLine();
            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
            content.newLine();
            content.icontext(SpriteName.unitEmoteLove, string.Format(DssRef.lang.CityOption_RepairGain, city.damages.Int()));
            
            player.hud.tooltip.create(player, content, true);
        }

        void buyCityGuardsAction(int count)
        {
            city.buyCityGuards(true, count);
        }

        public void releaseGuardSizeToolTip(int count)
        {
            RichBoxContent content = new RichBoxContent();

            if (city.canReleaseGuardSize(count))
            {
                content.h2(DssRef.lang.Hud_PurchaseTitle_Cost).overrideColor = HudLib.TitleColor_Label;
                content.newLine();
                content.icontext(SpriteName.WarsGuard, string.Format(DssRef.lang.Hud_IncreaseMaxGuardCount, -DssConst.ExpandGuardSize * count));
                
                
                content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;
                HudLib.ItemCount(content, SpriteName.rtsIncome, DssRef.lang.ResourceType_Gold, (DssConst.ReleaseGuardSizeGain * count).ToString());

            }
            else
            {
                content.Add(new RichBoxText(DssRef.todoLang.Hud_Purchase_MinCapacity, Color.Red));
            }

            player.hud.tooltip.create(player, content, true);
        }

        public void buyGuardSizeToolTip(int count)
        {
            RichBoxContent content = new RichBoxContent();

            if (city.canIncreaseGuardSize(count, false))
            {
                content.h2(DssRef.lang.Hud_PurchaseTitle_Cost).overrideColor = HudLib.TitleColor_Label;
                content.newLine();
                HudLib.ResourceCost(content, ResourceType.Gold, DssConst.ExpandGuardSizeCost * count, player.faction.gold);
                content.newLine();
                //content.icontext(SpriteName.rtsUpkeepTime, "Upkeep +" + city.GuardUpkeep(City.ExpandGuardSize * count).ToString());
                HudLib.Upkeep(content, city.GuardUpkeep(DssConst.ExpandGuardSize * count));

                content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;

                content.icontext(SpriteName.WarsGuardAdd, string.Format(DssRef.lang.Hud_IncreaseMaxGuardCount, TextLib.PlusMinus( DssConst.ExpandGuardSize * count)));
            }
            else 
            {
                content.Add(new RichBoxText(DssRef.lang.Hud_Purchase_MaxCapacity, Color.Red));
                content.newLine();
                content.Add(new RichBoxText(DssRef.lang.Hud_GuardCount_MustExpandCityMessage, Color.Red));
            }

            player.hud.tooltip.create(player, content, true);
        }

        //public void buySoldiersTip(CityPurchaseOption opt, int count)
        //{
        //    var typeData = DssRef.profile.Get(opt.unitType);
        //    var soldierData = DssRef.profile.Get(UnitType.Soldier);
        //    int dpsSoldier = soldierData.DPS_land();
        //    RichBoxContent content = new RichBoxContent();
        //    HudLib.Description(content, typeData.description);//content.text(TextLib.Quote(typeData.description));
        //    content.newLine();
        //    content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
        //    content.newLine();
        //    HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Gold, opt.goldCost * count, player.faction.gold);
        //    content.newLine();
        //    HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Worker, typeData.workForceCount() * count, city.workForce);
        //    content.newLine();
        //    content.newLine();

        //    content.text(string.Format(DssRef.lang.Hud_EnergyUpkeepX, typeData.energyPerSoldier));
        //    //HudLib.Upkeep(content, typeData.Upkeep() * count);
        //    //content.icontext(SpriteName.rtsUpkeep, DssRef.lang.Hud_Upkeep + ": " + (typeData.Upkeep() * count).ToString());
        //    content.newParagraph();

        //    content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
        //    int unitCount = typeData.rowWidth * typeData.columnsDepth;
        //    //string countText = "{0} groups, a total of {1} units";
        //    content.text(string.Format(DssRef.lang.SoldierStats_GroupCountAndSoldierCount, count, unitCount * count));
        //    content.newParagraph();

        //    content.h2(DssRef.lang.SoldierStats_Title);
        //    content.text(DssRef.lang.Hud_PurchaseTitle_Cost + ": " + TextLib.OneDecimal(opt.goldCost / (double)unitCount));
        //    //content.text(DssRef.lang.Hud_Upkeep + ": " + string.Format(HudLib.OneDecimalFormat, typeData.Upkeep() / (double)unitCount));
        //    //HudLib.Upkeep(content, typeData.Upkeep() / (double)unitCount);

            
        //    content.text(string.Format(DssRef.lang.SoldierStats_AttackStrengthLandSeaCity, dpsCompared(typeData.DPS_land(), dpsSoldier), dpsCompared(typeData.DPS_sea(), dpsSoldier), dpsCompared(typeData.DPS_structure(), dpsSoldier)));
        //    content.text(string.Format( DssRef.lang.SoldierStats_Health, typeData.basehealth));
        //    content.text(string.Format(DssRef.lang.SoldierStats_RecruitTrainingTimeMinutes, TextLib.OneDecimal(typeData.recruitTrainingTimeSec / 60.0)));

        //    speedBonus(true, typeData.ArmySpeedBonusLand);
        //    speedBonus(false, typeData.ArmySpeedBonusSea);


        //    player.hud.tooltip.create(player, content, true);

        //    void speedBonus(bool land, double bonus)
        //    {
        //        if (bonus != 0)
        //        {                    
        //            string bonusText = land? DssRef.lang.SoldierStats_SpeedBonusLand : DssRef.lang.SoldierStats_SpeedBonusSea;
        //            content.text(string.Format(bonusText, TextLib.PercentAddText((float)bonus)));
        //        }
        //    }
        //}

        string dpsCompared(int dps, int dpsSoldier)
        {
           return TextLib.OneDecimal(dps / (double)dpsSoldier);
        }

        
    }

    enum MenuTab
    {         
        Info,
        Tag,
        Conscript,
        Recruit,
        Economy,
        Resources,
        Work,
        Trade,
        BlackMarket,
        Delivery,
        Build,
        Automation,
        Disband,
        Divide,
        Mix,
        NUM
    }

    enum MixTabEditType
    { 
        None,
        Stockpile,
        WorkPrio,
    }

    enum ResourcesSubTab
    { 
        Overview_Resources,
        Overview_Metals,
        Overview_Weapons,
        Overview_Projectile,
        Overview_Armor,

        Stockpile_Resources,
        Stockpile_Metals,
        Stockpile_Weapons,
        Stockpile_Projectile,
        Stockpile_Armor,
        NUM
    }

    

    enum WorkSubTab
    {
        Priority_Resources,
        Priority_Metals,
        Priority_Weapons,
        Priority_Armor,
        Experience,
        NUM
    }
}
