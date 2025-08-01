using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Linq;
using Valve.Steamworks;
using VikingEngine.DSSWars.Build;
//using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.PlayerControls.Casual;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.DSSWars.XP;
using VikingEngine.Graphics;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.Map;
using VikingEngine.PJ;
using VikingEngine.ToGG;
using VikingEngine.ToGG.MoonFall;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Interface
{
    class CityMenu
    {
        public static readonly List<MenuTab> Tabs = new List<MenuTab>() { 
            MenuTab.Info, MenuTab.Resources, MenuTab.BlackMarket, 
            MenuTab.Build, MenuTab.Delivery, MenuTab.Conscript, MenuTab.Defence, MenuTab.Progress,
            MenuTab.Tag,MenuTab.Help,};

        protected Players.LocalPlayer player;
        protected City city;
        static readonly List<float> StockPileControls = new List<float> { 100 };

        public static readonly AutomationFocus[] AvailableAutomationFocuses =
        {
            AutomationFocus.NoFocus,
            AutomationFocus.Food,
            AutomationFocus.Grow,
            AutomationFocus.Export,
            AutomationFocus.Military
        };
        public CityMenu(Players.LocalPlayer player, City city, RichBoxContent content)
        {
            this.player = player;
            this.city = city;

            if (!DssRef.storage.centralGold)
            {
                content.newLine();
                content.Add(new RbImage(SpriteName.rtsMoney));
                content.space();
                content.Add(new RbText(DssRef.lang.ResourceType_Gold + ": " + TextLib.LargeNumber(city.money.GetGold()),  HudLib.NegativeRed(city.money.GetGold())));
                content.Add(new RbNewLine());
            }

            content.newLine();

            if (city.automateCity)
            {
                city.CityDetailsHud(false, player, content);
            }
            else
            {
#if DEBUG
                //content.Button("*soldier", new RbAction(()=> { city.debugGuardConscript( ItemResourceType.Sword); }) , null, true);
                //content.Button("*archer", new RbAction(() => { city.debugConscript(ItemResourceType.Bow); }), null, true);
                //content.Button("*ballista", new RbAction(() => { city.debugConscript(ItemResourceType.Ballista); }), null, true);
#endif

                int tabSel = 0;

                var tabs = new List<ArtTabMember>((int)MenuTab.NUM_NONE);

                List<MenuTab> availableTabs = player.AvailableCityTabs();
                for (int i = 0; i < availableTabs.Count; ++i)
                {
                    var text = new RbText(LangLib.Tab(availableTabs[i], out string description));
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

                    tabs.Add(new ArtTabMember(new List<AbsRichBoxMember>
                    {
                        text
                    }, enter));

                    if (availableTabs[i] == player.cityTab)
                    {
                        tabSel = i;
                    }
                }

                bool viewControllerTabs = player.gameControls.tabFocusColor(Players.PlayerControls.ControllerTabFocus.CityMenu, out Color focusColor);
                if (viewControllerTabs)
                {
                    content.Add(new RbImage(player.gameControls.input.Controller_TabLeft.Icon) { color = focusColor });
                    content.space(0.5f);
                }
               
                var tabGroup = new ArtTabgroup(tabs, tabSel, player.cityTabClick, null, SoundLib.menutab, null);
                if (viewControllerTabs)
                {
                    tabGroup.endAttach = new List<AbsRichBoxMember> { new RbSpace(0.5f), new RbImage(player.gameControls.input.Controller_TabRight.Icon) { color = focusColor } };
                }
                content.Add(tabGroup);
                content.newLine();

                switch (player.cityTab)
                {
                    case MenuTab.Info:
                        city.CityDetailsHud(false, player, content);
                        break;

                    case MenuTab.Tag:
                        tagsToMenu(content);
                        break;

                    case MenuTab.Conscript:
                        conscriptTab(content);
                        break;

                    case MenuTab.Defence:
                        defenceTab(content);
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
                        player.gameControls.build.toHud(player, content, city);
                        break;

                    case MenuTab.Progress:
                        progressTab(content);
                        break;

                    case MenuTab.Mix:
                        mixTab(content);
                        break;

                    case MenuTab.Help:
                        helpTab(content);
                        break;

                    case MenuTab.Casual_Recruit:                        
                        casualRecruitTab(content);
                        break;
                    case MenuTab.Casual_Build:
                        CasualBuild.ToHud(player, content, city);

                        break;
                }
            }
        }

        static readonly int[] RecruitTabCounts = [2, 5, 10, 25];

        void casualRecruitTab(RichBoxContent content)
        {
            if (city.getCount(CasualBuildType.Barracks) > 0)
            {
            
                buySoldierOption(city.casualCityProfile.guard,  CasualSoldierType.Guard);
                content.newParagraph();
                buySoldierOption(city.casualCityProfile.folkmen, CasualSoldierType.FolkMen);
                buySoldierOption(city.casualCityProfile.shipmen,  CasualSoldierType.Seamen);
                buySoldierOption(city.casualCityProfile.meleeMen, CasualSoldierType.Melee);
                buySoldierOption(city.casualCityProfile.rangedMen, CasualSoldierType.Ranged);
                buySoldierOption(city.casualCityProfile.riderMen, CasualSoldierType.Rider);
                buySoldierOption(city.casualCityProfile.siegeMen, CasualSoldierType.Siege);


                city.GetCasualProgress().RecruitToHud(player, city, content);
            }
            else
            {
                content.text(DssRef.lang.Hud_EmptyList).overrideColor = HudLib.InfoYellow_Light;
                content.newParagraph();
                content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement).overrideColor = HudLib.TitleColor_Label;
                content.newLine();
                content.Add(new RbImage(SpriteName.WarsBuild_Barracks));
                content.space();
                content.Add(new RbText(DssRef.lang.BuildingType_Barracks));
            }


            void buySoldierOption(SoldierPurchaseOption option, CasualSoldierType soldierType)
            {
                if (option.Available)
                {
                    //SoldierConscriptProfile soldierConscript = new SoldierConscriptProfile()
                    //{
                    //    conscript = new ConscriptProfile() { weapon = option.weapon },
                    //};
                    content.newLine();

                    //SpriteName icon;
                    //string caption;
                    //if (soldierType == CasualSoldierType.Guard)
                    //{
                    //    icon = SpriteName.WarsGuard;
                    //    caption = DssRef.lang.Conscript_Soldiers_GuardType;
                    //}
                    //else
                    //{
                    //    icon = soldierConscript.Icon();
                    //    caption = soldierConscript.conscript.TypeName();
                    //}
                    option.ButtonVisuals(soldierType, out SpriteName icon, out string caption);

                    var recruitOption = new CasualRecruitQueueItem(soldierType, option, 1);

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                        new RbImage(icon),
                        new RbSpace(),
                        new RbText(caption),
                        new RbSpace(2),
                        new RbImage(SpriteName.rtsMoney),
                        new RbText(option.FullPrice.ToString(), player.faction.hasGold(option.FullPrice, city)? HudLib.AvailableColor_Dark : HudLib.NotAvailableColor_Dark),
                    }, new RbAction1Arg<CasualRecruitQueueItem>(casualRecruitGroup, recruitOption), new RbTooltip(casualRecruitTooltip, recruitOption)));

                    content.Add(new RbTab(0.4f));
                    foreach (var counts in RecruitTabCounts)
                    {
                        recruitOption.count = counts;

                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {                       
                            new RbText("x" + counts, player.faction.hasGold(option.FullPrice * counts, city)? HudLib.AvailableColor_Dark : HudLib.NotAvailableColor_Dark),
                            },
                            new RbAction1Arg<CasualRecruitQueueItem>(casualRecruitGroup, recruitOption), new RbTooltip(casualRecruitTooltip, recruitOption)));
                    }                    
                }               
            }
            void casualRecruitGroup(CasualRecruitQueueItem recruitOption)//CasualSoldierType soldierType, SoldierPurchaseOption option, int count)
            {
                city.GetCasualProgress().AddRecruit(city, recruitOption);
            }

            void casualRecruitTooltip(RichBoxContent content, object tag)
            {
                CasualRecruitQueueItem recruitOption = (CasualRecruitQueueItem)tag;
                var conscript = recruitOption.ConscriptProfile(city);
                content.newLine();

                SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                {
                    conscript = conscript,
                };
                var data = SoldierProfile.init();


                content.h1(string.Format(DssRef.lang.Language_XCountIsY, DssRef.lang.UnitType_SoldierGroup, recruitOption.count), HudLib.TitleColor_Head);
                content.h2(DssRef.lang.Hud_PurchaseTitle_Cost, HudLib.TitleColor_Label);

                content.newLine();
                HudLib.BulletPoint(content);
                HudLib.ResourceCost(content, ResourceType.Gold, recruitOption.purchaseOption.FullPrice * recruitOption.count, player.faction.GetGold(city));

                content.newLine();
                HudLib.BulletPoint(content);
                HudLib.ResourceCost(content, ResourceType.Worker, data.workForceCount() * recruitOption.count, city.workForce.amount);

                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.IconSandGlass));
                content.space();
                content.Add(new RbText(string.Format(DssRef.lang.Conscript_TrainingTime, new TimeLength(city.casualRecruitTime_sec(recruitOption.soldierType) * recruitOption.count).LongString())));

                content.newParagraph();
                content.h2(DssRef.lang.Hud_PurchaseTitle_Gain, HudLib.TitleColor_Label);
                conscript.toHud(content, false);

                content.newLine();
                content.Add(new RbImage(SpriteName.WarsStrengthIcon));
                content.Add(new RbText(TextLib.TwoDecimal(AllUnits.GroupStrengh(data.UnitCount(), ref data, true))));

            }
        }

        

        void helpTab(RichBoxContent content)
        {
            content.h2(DssRef.lang.Help_Work_Title, HudLib.TitleColor_Head);

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsBluePrint));
            content.space();
            content.Add(new RbText(DssRef.lang.Help_Work_Resources));
            
            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsUnitLevelProfessional));
            content.space();
            content.Add(new RbText(DssRef.lang.Help_Work_Skill));

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsStockpileStop));
            content.space();
            content.Add(new RbText(DssRef.lang.Help_Work_Stockpile));

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsHammer));
            content.space();
            content.Add(new RbText(DssRef.lang.Help_Work_Priority));

            content.newParagraph();

            content.h2(DssRef.lang.Help_Soldiers_Title, HudLib.TitleColor_Head);

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsBuild_Barracks));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Help_Soldiers_PlaceBuildingX, DssRef.lang.BuildingType_Barracks)));

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsWorker));
            content.space();
            content.Add(new RbText(DssRef.lang.Help_Soldiers_Workers));

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsResource_Sword));
            content.space();
            content.Add(new RbText(DssRef.lang.Help_Soldiers_Weapon));

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsSoldierIcon));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Help_Soldiers_StartX, DssRef.lang.Hud_ProductionQueue)));


            content.newParagraph();

            content.h2(DssRef.lang.Resource_TypeName_Food, HudLib.TitleColor_Head);

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsResource_FoodSub));
            content.space();
            content.Add(new RbText(DssRef.lang.Help_Food_WhoEats));

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsArmy));
            content.space();
            content.Add(new RbText(DssRef.lang.Help_Food_BigArmy));

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsBuild_WheatFarms));
            content.space();
            content.Add(new RbText(DssRef.lang.Help_Food_DontBuild));

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsResource_Water));
            content.space();
            content.Add(new RbText(DssRef.lang.Help_Food_UseWater));

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsBuild_Postal));
            content.space();
            content.Add(new RbText(DssRef.lang.Help_Food_Postal));



        }
        void progressTab(RichBoxContent content)
        {
            //bool foodSafeGuard = false;

            for (ProgressSubTab workSubTab = 0; workSubTab < ProgressSubTab.NUM; ++workSubTab)
            {
                var tabContent = new RichBoxContent();
                string description = null;
                //string text = null;
                switch (workSubTab)
                {
                    case ProgressSubTab.Technology:
                        tabContent.Add(new RbImage(SpriteName.WarsTechnology_Unlocked));
                        tabContent.space(0.6f);
                        tabContent.Add(new RbText(DssRef.lang.Technology_Title));
                        description = DssRef.lang.Technology_Description;
                        break;

                    case ProgressSubTab.Experience:
                        tabContent.Add(new RbImage(SpriteName.WarsUnitLevelProfessional));
                        tabContent.space(0.6f);
                        tabContent.Add(new RbText(DssRef.lang.Experience_Title));
                        description = DssRef.lang.Experience_Description;
                        break;

                    case ProgressSubTab.Schools:
                        tabContent.Add(new RbImage(SpriteName.WarsBuild_School));
                        tabContent.space(0.6f);
                        tabContent.Add(new RbText(DssRef.lang.BuildingType_School_Tab));
                        description = DssRef.lang.BuildingType_School_Description + Environment.NewLine +
                             DssRef.lang.Building_ListDescription;
                        break;
                    case ProgressSubTab.Research:
                        tabContent.Add(new RbImage(SpriteName.WarsBuild_ResearchCenter));
                        tabContent.space(0.6f);
                        tabContent.Add(new RbText(DssRef.lang.Research_Tab));
                        description = DssRef.lang.Building_ListDescription;
                        break;
                }
            
                var subTab = new ArtButton(player.progressSubTab == workSubTab? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected, tabContent,
                    new RbAction1Arg<ProgressSubTab>((ProgressSubTab resourcesSubTab) =>
                    {
                        player.progressSubTab = resourcesSubTab;
                    }, workSubTab, SoundLib.menutab), new RbTooltip_Text(description));
                //subTab.setGroupSelectionColor(HudLib.RbSettings, player.progressSubTab == workSubTab);
                content.Add(subTab);
                //content.space();
            }
            content.newParagraph();

            switch (player.progressSubTab)
            {
                default:
                    new TechnologyHud().technologyHud(content, player, city, city.GetFaction());
                    break;

                case ProgressSubTab.Experience:
                    experienceTab(content);
                    break;

                case ProgressSubTab.Schools:
                    new SchoolMenu().ToHud(city, player, content);
                    break;

                case ProgressSubTab.Research:
                    new ResearchMenu().ToHud(city, player, content);
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
                            tabContent.Add(new RbText(DssRef.lang.Resource_Tab_Overview));
                            tabContent.space();
                            tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
                            break;

                        case ResourcesSubTab.Overview_Metals:
                            tabContent.Add(new RbImage(SpriteName.WarsResource_Iron));
                            break;

                        case ResourcesSubTab.Overview_Weapons:
                            tabContent.Add(new RbImage(SpriteName.WarsResource_Sword));
                            break;

                        case ResourcesSubTab.Overview_Projectile:
                            tabContent.Add(new RbImage(SpriteName.WarsResource_Bow));
                            break;

                        case ResourcesSubTab.Overview_Armor:
                            tabContent.Add(new RbImage(SpriteName.cmdMailArmor));
                            break;


                    }
                    var subTab = new ArtOption(player.resourcesSubTab == resourcesSubTab,tabContent,
                        new RbAction1Arg<ResourcesSubTab>((ResourcesSubTab resourcesSubTab) =>
                        {
                            player.resourcesSubTab = resourcesSubTab;
                        }, resourcesSubTab, SoundLib.menutab));
                    //subTab.setGroupSelectionColor(HudLib.RbSettings, player.resourcesSubTab == resourcesSubTab);
                    content.Add(subTab);
                    content.space();
                }
                content.newParagraph();
            }

            bool reachedBuffer = false;

            switch (player.resourcesSubTab)
            {
                case ResourcesSubTab.Overview_Resources:
                    {
                        ItemResourceType item = ItemResourceType.Wood_Group;
                        mixResource(item, false);
                        work(item, WorkPriorityType.wood);
                        work(item, WorkPriorityType.move);
                        blackMarket(item);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Stone_G;
                        mixResource(item, false);
                        work(item, WorkPriorityType.stone);
                        blackMarket(item);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Fuel_G;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Fuel1, CraftResourceLib.Charcoal);
                        work(item, WorkPriorityType.farmfuel);
                        work(item, WorkPriorityType.craftFuel);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.RawFood_Group;
                        mixResource(item, false);
                        work(item, WorkPriorityType.farmfood);
                        blackMarket(item);
                        end(item);
                    }                    

                    {
                        ItemResourceType item = ItemResourceType.Food_G;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Food1, CraftResourceLib.Food2);
                        work(item, WorkPriorityType.craftFood);
                        blackMarket(item);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Beer;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Beer);
                        work(item, WorkPriorityType.craftBeer);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.CoolingFluid;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.CoolingFluid);
                        work(item, WorkPriorityType.craftCoolingFluid);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.SkinLinen_Group;
                        mixResource(item, false);
                        work(item, WorkPriorityType.farmlinen);
                        blackMarket(item);
                        end(item);
                    }
                    content.newParagraph();
                    {
                        ItemResourceType item = ItemResourceType.Toolkit;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Toolkit);
                        work(item, WorkPriorityType.craftToolkit);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Wagon2Wheel;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.WagonLight);
                        work(item, WorkPriorityType.craftWagonLight);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Wagon4Wheel;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.WagonHeavy);
                        work(item, WorkPriorityType.craftWagonHeavy);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.BlackPowder;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.BlackPowder);
                        work(item, WorkPriorityType.craftBlackPowder);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.LedBullet;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.LedBullets);
                        work(item, WorkPriorityType.craftBullet);
                        end(item);
                    }

                    break;
                case ResourcesSubTab.Overview_Metals:

                    {
                        ItemResourceType item = ItemResourceType.IronOre_G;
                        mixResource(item, false);
                        work(item, WorkPriorityType.miningIron);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.TinOre;
                        mixResource(item, false);
                        work(item, WorkPriorityType.miningTin);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.CopperOre;
                        mixResource(item, false);
                        work(item, WorkPriorityType.miningCopper);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.LeadOre;
                        mixResource(item, false);
                        work(item, WorkPriorityType.miningLead);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.SilverOre;
                        mixResource(item, false);
                        work(item, WorkPriorityType.miningSilver);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Sulfur;
                        mixResource(item, false);
                        work(item, WorkPriorityType.miningSulfur);
                        end(item);
                    }
                    content.newParagraph();
                    {
                        ItemResourceType item = ItemResourceType.Iron_G;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Iron, CraftResourceLib.Iron_AndCooling);
                        work(item, WorkPriorityType.smeltIron);
                        blackMarket(item);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Tin;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Tin);
                        work(item, WorkPriorityType.smeltTin);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Copper;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Copper, CraftResourceLib.Cupper_AndCooling);
                        work(item, WorkPriorityType.smeltCopper);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Lead;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Lead);
                        work(item, WorkPriorityType.smeltLead);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Silver;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Silver, CraftResourceLib.Silver_AndCooling);
                        work(item, WorkPriorityType.smeltSilver);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.RawMithril;
                        mixResource(item, false);
                        work(item, WorkPriorityType.miningMithril);
                        end(item);
                    }
                    content.newParagraph();

                    {
                        ItemResourceType item = ItemResourceType.Bronze;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Bronze);
                        work(item, WorkPriorityType.craftBronze);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.CastIron;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.CastIron);
                        work(item, WorkPriorityType.craftCastIron);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.BloomeryIron;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.BloomeryIron);
                        work(item, WorkPriorityType.craftBloomeryIron);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Steel;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Steel, CraftResourceLib.Steel_AndCooling);
                        work(item, WorkPriorityType.craftSteel);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Mithril;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Mithril);
                        work(item, WorkPriorityType.craftMithril);
                        end(item);
                    }
                    break;

                case ResourcesSubTab.Overview_Weapons:

                    {
                        ItemResourceType item = ItemResourceType.SharpStick;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.SharpStick);
                        work(item, WorkPriorityType.craftSharpStick);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.HandSpear;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.HandSpearIron, CraftResourceLib.HandSpearBronze);
                        work(item, WorkPriorityType.craftHandSpear);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.BronzeSword;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.BronzeSword);
                        work(item, WorkPriorityType.craftBronzeSword);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.ShortSword;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.ShortSword);
                        work(item, WorkPriorityType.craftShortSword);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Sword;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Sword);
                        work(item, WorkPriorityType.craftSword);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.LongSword;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.LongSword);
                        work(item, WorkPriorityType.craftLongSword);
                        end(item);
                    }
                    content.newParagraph();
                    {
                        ItemResourceType item = ItemResourceType.Warhammer;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.WarhammerIron, CraftResourceLib.WarhammerBronze);
                        work(item, WorkPriorityType.craftWarhammer);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.TwoHandSword;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.TwoHandSword);
                        work(item, WorkPriorityType.craftTwoHandSword);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.KnightsLance;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.KnightsLance);
                        work(item, WorkPriorityType.craftKnightsLance);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.MithrilSword;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.MithrilSword);
                        work(item, WorkPriorityType.craftMithrilSword);
                        end(item);
                    }
                    break;

                case ResourcesSubTab.Overview_Projectile:

                    {
                        ItemResourceType item = ItemResourceType.SlingShot;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Slingshot);
                        work(item, WorkPriorityType.craftSlingshot);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.ThrowingSpear;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.ThrowingSpear1,CraftResourceLib.ThrowingSpear2);
                        work(item, WorkPriorityType.craftThrowingspear);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Bow;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Bow);
                        work(item, WorkPriorityType.craftBow);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.LongBow;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.LongBow);
                        work(item, WorkPriorityType.craftLongbow);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Crossbow;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.CrossBow);
                        work(item, WorkPriorityType.craftCrossbow);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.MithrilBow;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.MithrilBow);
                        work(item, WorkPriorityType.craftMithrilbow);
                        end(item);
                    }
                    content.newParagraph();
                    {
                        ItemResourceType item = ItemResourceType.HandCannon;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.BronzeHandCannon);
                        work(item, WorkPriorityType.craftHandCannon);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.HandCulverin;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.BronzeHandCulverin);
                        work(item, WorkPriorityType.craftHandCulverin);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Rifle;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Rifle);
                        work(item, WorkPriorityType.craftRifle);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Blunderbuss;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Blunderbus);
                        work(item, WorkPriorityType.craftBlunderbus);
                        end(item);
                    }
                    content.newParagraph();
                    {
                        ItemResourceType item = ItemResourceType.Ballista;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Ballista_Iron,CraftResourceLib.Ballista_Bronze);
                        work(item, WorkPriorityType.craftBallista);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Manuballista;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.ManuBallista);
                        work(item, WorkPriorityType.craftManuBallista);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.Catapult;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.Catapult);
                        work(item, WorkPriorityType.craftCatapult);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.SiegeCannonBronze;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.SiegeCannonBronze);
                        work(item, WorkPriorityType.craftSiegeCannonBronze);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.ManCannonBronze;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.ManCannonBronze);
                        work(item, WorkPriorityType.craftManCannonBronze);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.SiegeCannonIron;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.SiegeCannonIron);
                        work(item, WorkPriorityType.craftSiegeCannonIron);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.ManCannonIron;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.ManCannonIron);
                        work(item, WorkPriorityType.craftManCannonIron);
                        end(item);
                    }

                    break;

                case ResourcesSubTab.Overview_Armor:

                    {
                        ItemResourceType item = ItemResourceType.PaddedArmor;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.PaddedArmor);
                        work(item, WorkPriorityType.craftPaddedArmor);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.HeavyPaddedArmor;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.HeavyPaddedArmor);
                        work(item, WorkPriorityType.craftHeavyPaddedArmor);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.BronzeArmor;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.BronzeArmor);
                        work(item, WorkPriorityType.craftBronzeArmor);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.IronArmor;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.MailArmor);
                        work(item, WorkPriorityType.craftMailArmor);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.HeavyIronArmor;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.HeavyMailArmor);
                        work(item, WorkPriorityType.craftHeavyMailArmor);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.LightPlateArmor;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.PlateArmor);
                        work(item, WorkPriorityType.craftPlateArmor);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.FullPlateArmor;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.FullPlateArmor);
                        work(item, WorkPriorityType.craftFullPlateArmor);
                        end(item);
                    }
                    {
                        ItemResourceType item = ItemResourceType.MithrilArmor;
                        mixResource(item, false);
                        blueprint(CraftResourceLib.MithrilArmor);
                        work(item, WorkPriorityType.craftMithrilArmor);
                        end(item);
                    }

                    break;
            }

            void mixResource(ItemResourceType item, bool safeGuard)
            {
                content.newLine();

                var typeIcon = ResourceLib.Icon(item);
                var typeName = LangLib.Item(item);
                var city_res = city.GetGroupedResource(item);

                var infoContent = new List<AbsRichBoxMember>(2);
                infoContent.Add(new RbImage(typeIcon));
                infoContent.Add(new RbSpace());
                var amountText = new RbText(city_res.amount.ToString());
                amountText.overrideColor = Color.White;
                infoContent.Add(amountText);

                var infoButton = new RbButton(infoContent, null, new RbAction(() =>
                {
                    RichBoxContent content = new RichBoxContent();
                    content.Add(new RbText(typeName));
                    player.hud.tooltip.create(player, content, true);
                }));
                infoButton.overrideBgColor = HudLib.InfoYellow_BG;

                content.Add(infoButton);
                content.space();
                

                if (item != ItemResourceType.Water_G &&
                   item != ItemResourceType.Gold &&
                   item != ItemResourceType.Men)
                {
                    var stockpileContent = new List<AbsRichBoxMember>(2);
                    stockpileContent.Add(new RbText(city_res.goalBuffer.ToString()));

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
                    var icon = new RbImage(stockIcon);
                    stockpileContent.Add(icon);


                    var stockpileButton = new ArtButton( RbButtonStyle.HoverArea, stockpileContent, 
                        new RbAction(() =>
                        {
                            player.mixTabEditType = MixTabEditType.Stockpile;
                            player.mixTabItem = item;
                        }, SoundLib.menu), 
                        new RbAction(()=> {
                            var content = new RichBoxContent();
                            content.text(DssRef.lang.Resource_Tab_Stockpile);
                            player.hud.tooltip.create(player, content, true);
                        }));

                    content.Add(new RbTab(0.22f));
                    content.Add(stockpileButton);
                    content.space();
                }
            }

            void blueprint(CraftBlueprint blueprint, CraftBlueprint optionalBp = null)
            {

                content.Add(new ArtButton( RbButtonStyle.HoverArea,new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsBluePrint)
                    },
                    null,new RbTooltip(blueprintTooltip, new BlueprintTooltipArgs() { blueprint = blueprint, optionalBp = optionalBp }
                    )));
                content.space();
            }

            void work(ItemResourceType item, WorkPriorityType workPriorityType)
            {
                LangLib.WorkNameIcon(workPriorityType, out string name, out SpriteName workIcon, out SpriteName typeIcon);
                var buttonContent = new RichBoxContent();
                buttonContent.Add(new RbImage(workIcon));
                var prio = city.workTemplate.GetWorkPriority(workPriorityType);
                buttonContent.Add(new RbText(prio.value.ToString()));

                var button = new RbButton(buttonContent, new RbAction(() =>
                {
                    player.mixTabEditType = MixTabEditType.WorkPrio;
                    player.mixWorkType = workPriorityType;
                    player.mixTabItem = item;
                }, SoundLib.menu),
                new RbAction(()=> 
                {
                    var content = new RichBoxContent();
                    HudLib.Label(content, DssRef.lang.Work_OrderPrioTitle);
                    content.text(name);
                    player.hud.tooltip.create(player, content, true);
                }));

                //content.Add(new RichBoxTab(0.5f));
                content.Add(button);
                content.space();
            }

            void blackMarket(ItemResourceType item)
            {
                var buttonContent = new RichBoxContent();
                buttonContent.Add(new RbText("BM"));

                var button = new ArtButton( RbButtonStyle.Primary,buttonContent, new RbAction(() =>
                {
                    player.mixTabEditType = MixTabEditType.BlackMarket;
                    player.mixTabItem = item;
                }, SoundLib.menu),
                new RbAction(() =>
                {
                    var content = new RichBoxContent();
                    HudLib.Label(content, DssRef.lang.Hud_BlackMarket);
                   
                    player.hud.tooltip.create(player, content, true);
                }));
                //button.overrideBgColor = Color.DarkViolet;
                content.Add(button);
                content.space();
            }

            void end(ItemResourceType item)
            {
                if (player.mixTabEditType != MixTabEditType.None &&
                   item == player.mixTabItem)
                {
                    var city_res = city.GetGroupedResource(item);

                    content.newLine();
                    switch (player.mixTabEditType)
                    {
                        case MixTabEditType.Stockpile:
                            stockPileEdit(content, item, city_res);
                            break;
                        case MixTabEditType.WorkPrio:
                            LangLib.WorkNameIcon(player.mixWorkType, out string name, out SpriteName workIcon, out SpriteName typeIcon);
                            city.workTemplate.GetWorkPriority(player.mixWorkType).toHud(player, content, name, workIcon, typeIcon, player.mixWorkType, player.faction, city);
                            break;
                        case MixTabEditType.BlackMarket:
                            BlackMarketResources.ResourceToHud(item, player, content, city);
                            break;
                    }
                }
            }
        }
      
        void experienceTab(RichBoxContent content)
        {
            HudLib.Label(content, DssRef.lang.Experience_TopExperience);
            experience(SpriteName.WarsWorkFarm, DssRef.lang.ExperienceType_Farm, city.topskill_Farm);
            experience(SpriteName.WarsBuild_HenPen, DssRef.lang.ExperienceType_AnimalCare, city.topskill_AnimalCare);
            experience(SpriteName.WarsHammer, DssRef.lang.ExperienceType_HouseBuilding, city.topskill_HouseBuilding);
            experience(SpriteName.WarsResource_Wood, DssRef.lang.ExperienceType_WoodWork, city.topskill_WoodCutter);
            experience(SpriteName.WarsResource_Stone, DssRef.lang.ExperienceType_StoneCutter, city.topskill_StoneCutter);
            experience(SpriteName.WarsWorkMine, DssRef.lang.ExperienceType_Mining, city.topskill_Mining);
            experience(SpriteName.WarsWorkMove, DssRef.lang.ExperienceType_Transport, city.topskill_Transport);
            experience(SpriteName.WarsResource_Food, DssRef.lang.ExperienceType_Cook, city.topskill_Cook);
            experience(SpriteName.WarsFletcherArrowIcon, DssRef.lang.ExperienceType_Fletcher, city.topskill_Fletcher);
            experience(SpriteName.WarsWorkSmelting, DssRef.todoLang.ExperienceType_Smelting, city.topskill_Smelting);
            experience(SpriteName.WarsResource_CastIron, DssRef.lang.ExperienceType_Casting, city.topskill_Casting);
            experience(SpriteName.WarsResource_Iron, DssRef.lang.ExperienceType_CraftMetal, city.topskill_CraftMetal);
            experience(SpriteName.WarsResource_IronArmor, DssRef.lang.ExperienceType_CraftArmor, city.topskill_CraftArmor);
            experience(SpriteName.WarsResource_Sword, DssRef.lang.ExperienceType_CraftWeapon, city.topskill_CraftWeapon);
            experience(SpriteName.WarsResource_Fuel, DssRef.lang.ExperienceType_CraftFuel, city.topskill_CraftFuel);
            experience(SpriteName.WarsBuild_Chemist, DssRef.lang.ExperienceType_Chemist, city.topskill_Chemistry);

            content.newParagraph();
            HudLib.Description(content, string.Format(DssRef.lang.Experience_TimeReductionDescription, MathExt.PercentageInteger(DssConst.XpLevelWorkTimePercReduction)));

            content.newParagraph();
            content.Add(new RbBeginTitle());
            var prioTitle = new RbText( DssRef.lang.ExperenceOrDistancePrio_Title);
            prioTitle.overrideColor = HudLib.TitleColor_Label;
            content.Add(prioTitle);
            content.space();
            HudLib.InfoButton(content, new RbTooltip_Text(DssRef.lang.ExperenceOrDistancePrio_Description));

            content.newLine();
            for (ExperienceOrDistancePrio prio = 0; prio < ExperienceOrDistancePrio.NUM; ++prio)
            {
                string text = null;
                switch (prio)
                {
                    case ExperienceOrDistancePrio.Distance:
                        text = DssRef.lang.Hud_Distance;
                        break;
                    case ExperienceOrDistancePrio.Mix:
                        text = DssRef.lang.Hud_Mixed;
                        break;
                    case ExperienceOrDistancePrio.Experience:
                        text = DssRef.lang.Experience_Title;
                        break;

                }
                var option = new ArtOption(city.experenceOrDistance == prio, new List<AbsRichBoxMember> { new RbText(text) },
                    new RbAction1Arg<ExperienceOrDistancePrio>((ExperienceOrDistancePrio val) =>
                    {
                        city.experenceOrDistance = val;
                    }, prio, SoundLib.menu));
                content.Add(option);
            }
            
            void experience(SpriteName typeIcon, string typeName, ExperienceLevel level)
            {
                content.newLine();
                content.Add(new RbImage(typeIcon));
                content.space();
                var typeNameText = new RbText(typeName + ":");
                typeNameText.overrideColor = HudLib.TitleColor_TypeName;
                content.Add(typeNameText);

                content.Add(new RbTab(0.4f));
                content.Add(new RbImage(LangLib.ExperienceLevelIcon(level)));
                content.Add(new RbText(LangLib.ExperienceLevel(level)));
            }
        }

        public void tagsToMenu(RichBoxContent content)
        {
            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Tag_ViewOnMap) }, player.CityTagsOnMapProperty));
            content.newParagraph();

            for (CityTagBack back = CityTagBack.NONE; back < CityTagBack.NUM; back++)
            {
                var button = new ArtToggle(back == city.tagBack, new List<AbsRichBoxMember> {
                    new RbImage(Data.CityTag.BackSprite(back))
                }, new RbAction1Arg<CityTagBack>((CityTagBack back) => { city.tagBack = back; }, back));
                content.Add(button);

                if (back == CityTagBack.NONE)
                {
                    content.newLine();
                }
            }

            if (city.tagBack != CityTagBack.NONE)
            {
                content.newParagraph();
                for (CityTagArt art = CityTagArt.None; art < CityTagArt.NUM; art++)
                {
                    var button = new ArtToggle(art == city.tagArt, new List<AbsRichBoxMember> {
                    new RbImage(Data.CityTag.ArtSprite(art))
                    }, new RbAction1Arg<CityTagArt>((CityTagArt art) => { city.tagArt = art; }, art));
                    content.Add(button);
                }
            }
        }

        public void resourcesToMenu(RichBoxContent content)
        {
            if (player.tutorial == null || player.tutorial.DisplayResourseSubTabs())
            {
                for (ResourcesSubTab resourcesSubTab = 0; resourcesSubTab < ResourcesSubTab.Auto; ++resourcesSubTab)
                {
                    var tabContent = new RichBoxContent();
                    //string text = null;
                    switch (resourcesSubTab)
                    {
                        case ResourcesSubTab.Overview_Metals:
                        case ResourcesSubTab.Stockpile_Metals:
                        case ResourcesSubTab.Work_Metals:
                            tabContent.Add(new RbImage(SpriteName.WarsResource_Iron));
                            break;
                        case ResourcesSubTab.Overview_Weapons:
                        case ResourcesSubTab.Stockpile_Weapons:
                        case ResourcesSubTab.Work_Weapons:
                            tabContent.Add(new RbImage(SpriteName.WarsResource_Sword));
                            break;

                        case ResourcesSubTab.Overview_Projectile:
                        case ResourcesSubTab.Stockpile_Projectile:
                        case ResourcesSubTab.Work_Projectile:
                            tabContent.Add(new RbImage(SpriteName.WarsResource_Bow));
                            break;

                        case ResourcesSubTab.Overview_Armor:
                        case ResourcesSubTab.Stockpile_Armor:
                        case ResourcesSubTab.Work_Armor:
                            tabContent.Add(new RbImage(SpriteName.WarsResource_IronArmor));

                            break;

                        case ResourcesSubTab.Overview_Resources:
                            content.Add(new ArtButton(RbButtonStyle.HoverArea,
                                new List<AbsRichBoxMember> { new RbImage(SpriteName.MenuPixelIconManual) },
                                null, new RbTooltip_Text(DssRef.lang.Resource_Tab_Overview)));
                            //content.h2(DssRef.lang.Resource_Tab_Overview);
                            //content.newLine();
                            //tabContent.Add(new RbText(DssRef.lang.Resource_Tab_Overview));
                            //tabContent.Add(new RbTab(0.25f));
                            tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
                            break;

                        case ResourcesSubTab.Stockpile_Resources:
                            content.Add(new ArtButton(RbButtonStyle.HoverArea,
                                new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsStockpileAdd) },
                                null, new RbTooltip_Text(DssRef.lang.Resource_Tab_Stockpile)));
                            //content.h2(DssRef.lang.Resource_Tab_Stockpile);
                            //content.newLine();
                            //tabContent.Add(new RbText(DssRef.lang.Resource_Tab_Stockpile));
                            //tabContent.Add(new RbTab(0.25f));
                            tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
                            break;

                        case ResourcesSubTab.Work_Resources:
                            content.Add(new ArtButton(RbButtonStyle.HoverArea,
                                new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHammer) },
                                null, new RbTooltip_Text(DssRef.lang.MenuTab_Work)));
                            //content.h2(DssRef.lang.MenuTab_Work);
                            //content.newLine();
                            //tabContent.Add(new RbText(DssRef.lang.MenuTab_Work));
                            //tabContent.Add(new RbTab(0.25f));
                            tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
                            break;
                    }
                    var subTab = new ArtButton(player.resourcesSubTab == resourcesSubTab ? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected,
                        tabContent,
                        new RbAction1Arg<ResourcesSubTab>((ResourcesSubTab resourcesSubTab) =>
                        {
                            player.resourcesSubTab = resourcesSubTab;
                        }, resourcesSubTab, SoundLib.menutab), new RbTooltip_Text(DssRef.lang.Work_SelectCategory));
                    //subTab.setGroupSelectionColor(HudLib.RbSettings, player.resourcesSubTab == resourcesSubTab);
                    content.Add(subTab);

                    switch (resourcesSubTab)
                    {
                        case ResourcesSubTab.Overview_Armor:
                            HudLib.InfoButton(content,
                               new RbTooltip((RichBoxContent content, object tag) =>
                               {
                                   GroupedResource.BufferIconInfo(content, false);
                                   bool foodSafeGuard = city.foodSafeGuardIsActive(out bool fuelSafeGuard, out bool rawFoodSafeGuard, out bool woodSafeGuard);
                                   if (foodSafeGuard)
                                   {
                                       GroupedResource.BufferIconInfo(content, true);
                                   }
                                   ResourceLib.ConvertGoldOre.toMenu(content, city, false, true, false, false);
                                   {
                                       content.newLine();
                                       content.Add(new RbText(1.ToString()));
                                       content.Add(new RbImage(ResourceLib.Icon(ItemResourceType.Food_G)));
                                       content.Add(new RbText(DssRef.lang.Resource_TypeName_Food));
                                       var arrow = new RbImage(SpriteName.pjNumArrowR);
                                       arrow.color = Color.CornflowerBlue;
                                       content.Add(arrow);
                                       content.Add(new RbText(string.Format(DssRef.lang.Hud_EnergyAmount, DssRef.difficulty.FoodEnergySett)));
                                   }
                                   content.newLine();
                                   HudLib.BulletPoint(content);
                                   content.Add(new RbText(DssRef.lang.Work_BadValueDescription, HudLib.InfoYellow_Light));
                               }));
                            content.newLine();
                            break;
                        case ResourcesSubTab.Stockpile_Armor:
                            HudLib.InfoButton(content,
                               new RbTooltip((RichBoxContent content, object tag) =>
                               {
                                   HudLib.Description(content, DssRef.lang.Resource_StockPile_Info);
                                   GroupedResource.BufferIconInfo(content, false);
                                   content.newLine();
                                   HudLib.BulletPoint(content);
                                   content.Add(new RbText(DssRef.lang.Work_BadValueDescription, HudLib.InfoYellow_Light));
                               }));
                            content.newLine();
                            break;
                        case ResourcesSubTab.Work_Armor:
                            HudLib.InfoButton(content,
                               new RbTooltip((RichBoxContent content, object tag) =>
                               {
                                   HudLib.Description(content, string.Format(DssRef.lang.Work_OrderPrioDescription, WorkTemplate.MaxPrio));
                               }));
                            break;
                    }
                    //if (resourcesSubTab == ResourcesSubTab.Overview_Armor ||
                    //    resourcesSubTab == ResourcesSubTab.Stockpile_Armor)
                    //    //resourcesSubTab == ResourcesSubTab.Work_Armor)
                    //{
                        
                    //}
                    //else
                    //{
                    //    content.space();
                    //}
                }
                content.newParagraph();
            }

            bool reachedBuffer = false;

            switch (player.resourcesSubTab)
            {
                case ResourcesSubTab.Overview_Resources:
                    //content.h1(DssRef.lang.MenuTab_Resources).overrideColor = HudLib.TitleColor_Label;
                    //content.newLine();

                    content.Add(new RbImage(SpriteName.WarsResource_Water));
                    content.space();
                    content.Add(new RbText(TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Water) + ": " + string.Format(DssRef.lang.Language_CollectProgress, city.res_water.amount, city.maxWaterTotal)));
                    content.Add(new RbTab(0.4f));
                    content.Add(new RbImage(SpriteName.WarsResource_WaterAdd));
                    content.Add(new RbText(TextLib.OneDecimal(city.waterAddPerSec)));
                    content.space();
                    HudLib.InfoButton(content,
                       new RbTooltip((RichBoxContent content, object tag) =>                       
                       {
                        //RichBoxContent content = new RichBoxContent();
                        content.h2(TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Water)).overrideColor = HudLib.TitleColor_Label;
                        content.newLine();
                        content.Add(new RbImage(SpriteName.WarsResource_Water));
                        content.Add(new RbText( string.Format(DssRef.lang.Resource_CurrentAmount, city.res_water.amount)));

                        content.text(string.Format(DssRef.lang.Resource_MaxAmount, city.maxWaterTotal));

                        content.newLine();
                        content.Add(new RbImage(SpriteName.WarsResource_WaterAdd));
                        content.Add(new RbText(string.Format(DssRef.lang.Resource_AddPerSec, TextLib.OneDecimal( city.waterAddPerSec))));

                        content.newParagraph();
                        HudLib.Description(content, DssRef.lang.Resource_WaterAddLimit);

                        //player.hud.tooltip.create(player, content, true);
                    }));

                    bool foodSafeGuard = city.foodSafeGuardIsActive(out bool fuelSafeGuard, out bool rawFoodSafeGuard, out bool woodSafeGuard);

                    city.res_wood.toMenu(content, ItemResourceType.Wood_Group, woodSafeGuard, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    city.res_stone.toMenu(content, ItemResourceType.Stone_G, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    city.res_rawFood.toMenu(content, ItemResourceType.RawFood_Group, rawFoodSafeGuard, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    city.res_skinLinnen.toMenu(content, ItemResourceType.SkinLinen_Group, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    content.newParagraph();


                    city.res_food.toMenu(content, ItemResourceType.Food_G, foodSafeGuard, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    blueprintButton(player, content, CraftResourceLib.Food1, CraftResourceLib.Food2);
                    content.space();

                    content.Add(new ArtToggle(city.res_food_safeguard,new List<AbsRichBoxMember> {
                            new RbImage(city.res_food_safeguard? SpriteName.WarsProtectedStockpileOn : SpriteName.WarsProtectedStockpileOff, 0.7f),
                        },
                    new RbAction(() =>{
                        city.res_food_safeguard = !city.res_food_safeguard;
                    }),
                    new RbTooltip((RichBoxContent content, object tag) =>
                    {
                         
                        content.text(string.Format(DssRef.lang.Resource_FoodSafeGuard_Description, DssConst.WorkSafeGuardAmount)).overrideColor = HudLib.InfoYellow_Light;
                        content.text(city.res_food_safeguard? DssRef.lang.Hud_On : DssRef.lang.Hud_Off);
                        
                    })));

                    city.res_beer.toMenu(content, ItemResourceType.Beer, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    blueprintButton(player, content, CraftResourceLib.Beer);

                    city.res_coolingfluid.toMenu(content, ItemResourceType.CoolingFluid, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    blueprintButton(player, content, CraftResourceLib.CoolingFluid);
                    content.newParagraph();

                    city.res_fuel.toMenu(content, ItemResourceType.Fuel_G, fuelSafeGuard, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    int totalmines = 0;
                    city.terrainStructure.mine(content, city.terrainStructure.mineCount_coal, ItemResourceType.Coal, ref totalmines);
                    blueprintButton(player, content, CraftResourceLib.Fuel1, null, true);
                    content.space();
                    blueprintButton(player, content, CraftResourceLib.Charcoal);
                    

                    city.res_Palisade.toMenu(content, ItemResourceType.Palisade, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    blueprintButton(player, content, CraftResourceLib.Palisade);

                    city.res_Toolkit.toMenu(content, ItemResourceType.Toolkit, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    blueprintButton(player, content, CraftResourceLib.Toolkit);

                    city.res_Wagon2Wheel.toMenu(content, ItemResourceType.Wagon2Wheel, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    blueprintButton(player, content, CraftResourceLib.WagonLight);

                    city.res_Wagon4Wheel.toMenu(content, ItemResourceType.Wagon4Wheel, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    blueprintButton(player, content, CraftResourceLib.WagonHeavy);

                    city.res_BlackPowder.toMenu(content, ItemResourceType.BlackPowder, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    blueprintButton(player, content, CraftResourceLib.BlackPowder);

                    city.res_GunPowder.toMenu(content, ItemResourceType.GunPowder, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    blueprintButton(player, content, CraftResourceLib.GunPowder);

                    city.res_LedBullet.toMenu(content, ItemResourceType.LedBullet, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Resources);
                    blueprintButton(player, content, CraftResourceLib.LedBullets);

                    
                    //content.Add(new RbSeperationLine());
                    //GroupedResource.BufferIconInfo(content, false);
                    //if (foodSafeGuard)
                    //{
                    //    GroupedResource.BufferIconInfo(content, true);
                    //}
                    //ResourceLib.ConvertGoldOre.toMenu(content, city, false, true, false, false);
                    //{
                    //    content.newLine();
                    //    content.Add(new RbText(1.ToString()));
                    //    content.Add(new RbImage(ResourceLib.Icon(ItemResourceType.Food_G)));
                    //    content.Add(new RbText(DssRef.lang.Resource_TypeName_Food));
                    //    var arrow = new RbImage(SpriteName.pjNumArrowR);
                    //    arrow.color = Color.CornflowerBlue;
                    //    content.Add(arrow);
                    //    content.Add(new RbText(string.Format(DssRef.lang.Hud_EnergyAmount,DssRef.difficulty.FoodEnergySett)));
                    //}
                    break;

                case ResourcesSubTab.Overview_Metals:

                    int totalMines = 0;

                    city.res_ironore.toMenu(content, ItemResourceType.IronOre_G, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    city.terrainStructure.mine(content, city.terrainStructure.mineCount_bogIron, ItemResourceType.BogIron, ref totalMines);
                    city.terrainStructure.mine(content, city.terrainStructure.mineCount_iron, ItemResourceType.Iron_G, ref totalMines);

                    city.res_TinOre.toMenu(content, ItemResourceType.TinOre, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    city.terrainStructure.mine(content, city.terrainStructure.mineCount_tin, ItemResourceType.Tin, ref totalMines);

                    city.res_CupperOre.toMenu(content, ItemResourceType.CopperOre, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals); 
                    city.terrainStructure.mine(content, city.terrainStructure.mineCount_copper, ItemResourceType.Copper, ref totalMines);

                    city.res_LeadOre.toMenu(content, ItemResourceType.LeadOre, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    city.terrainStructure.mine(content, city.terrainStructure.mineCount_lead, ItemResourceType.Lead, ref totalMines);

                    city.res_SilverOre.toMenu(content, ItemResourceType.SilverOre, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    city.terrainStructure.mine(content, city.terrainStructure.mineCount_silver, ItemResourceType.Silver, ref totalMines);

                    content.newParagraph();


                    city.res_iron.toMenu(content, ItemResourceType.Iron_G, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    blueprintButton(player, content, CraftResourceLib.Iron, CraftResourceLib.Iron_AndCooling);

                    city.res_Tin.toMenu(content, ItemResourceType.Tin, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    blueprintButton(player, content, CraftResourceLib.Tin);

                    city.res_Cupper.toMenu(content, ItemResourceType.Copper, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    blueprintButton(player, content, CraftResourceLib.Copper, CraftResourceLib.Cupper_AndCooling);

                    city.res_Lead.toMenu(content, ItemResourceType.Lead, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    blueprintButton(player, content, CraftResourceLib.Lead);

                    city.res_Silver.toMenu(content, ItemResourceType.Silver, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    blueprintButton(player, content, CraftResourceLib.Silver, CraftResourceLib.Silver_AndCooling);

                    city.res_RawMithril.toMenu(content, ItemResourceType.RawMithril, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    city.terrainStructure.mine(content, city.terrainStructure.mineCount_mithril, ItemResourceType.Mithril, ref totalMines);

                    city.res_Sulfur.toMenu(content, ItemResourceType.Sulfur, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    city.terrainStructure.mine(content, city.terrainStructure.mineCount_sulfur, ItemResourceType.Sulfur, ref totalMines);
                    content.newParagraph();


                    city.res_Bronze.toMenu(content, ItemResourceType.Bronze, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    blueprintButton(player, content, CraftResourceLib.Bronze);

                    city.res_CastIron.toMenu(content, ItemResourceType.CastIron, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    blueprintButton(player, content, CraftResourceLib.CastIron);

                    city.res_BloomeryIron.toMenu(content, ItemResourceType.BloomeryIron, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    blueprintButton(player, content, CraftResourceLib.BloomeryIron);
                    
                    city.res_Steel.toMenu(content, ItemResourceType.Steel, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    blueprintButton(player, content, CraftResourceLib.Steel, CraftResourceLib.Steel_AndCooling);

                    city.res_Mithril.toMenu(content, ItemResourceType.Mithril, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Metals);
                    blueprintButton(player, content, CraftResourceLib.Mithril);
                    break;

                case ResourcesSubTab.Overview_Weapons:

                    city.res_sharpstick.toMenu(content, ItemResourceType.SharpStick, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Weapons);
                    blueprintButton(player, content, CraftResourceLib.SharpStick);

                    city.res_BronzeSword.toMenu(content, ItemResourceType.BronzeSword, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Weapons);
                    blueprintButton(player, content, CraftResourceLib.BronzeSword);

                    city.res_shortsword.toMenu(content, ItemResourceType.ShortSword, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Weapons);
                    blueprintButton(player, content, CraftResourceLib.ShortSword);

                    city.res_Sword.toMenu(content, ItemResourceType.Sword, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Weapons);
                    blueprintButton(player, content, CraftResourceLib.Sword);

                    city.res_LongSword.toMenu(content, ItemResourceType.LongSword, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Weapons);
                    blueprintButton(player, content, CraftResourceLib.LongSword);
                    
                    city.res_HandSpear.toMenu(content, ItemResourceType.HandSpear, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Weapons);
                    blueprintButton(player, content, CraftResourceLib.HandSpearIron, CraftResourceLib.HandSpearBronze);
                    
                    content.newParagraph();

                    city.res_Warhammer.toMenu(content, ItemResourceType.Warhammer, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Weapons);
                    blueprintButton(player, content, CraftResourceLib.WarhammerIron, CraftResourceLib.WarhammerBronze);

                    city.res_twohandsword.toMenu(content, ItemResourceType.TwoHandSword, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Weapons);
                    blueprintButton(player, content, CraftResourceLib.TwoHandSword);

                    city.res_knightslance.toMenu(content, ItemResourceType.KnightsLance, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Weapons);
                    blueprintButton(player, content, CraftResourceLib.KnightsLance);

                    city.res_MithrilSword.toMenu(content, ItemResourceType.MithrilSword, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Weapons);
                    blueprintButton(player, content, CraftResourceLib.MithrilSword);
                   
                    break;

                case ResourcesSubTab.Overview_Projectile:

                    city.res_SlingShot.toMenu(content, ItemResourceType.SlingShot, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.Slingshot);

                    city.res_ThrowingSpear.toMenu(content, ItemResourceType.ThrowingSpear, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.ThrowingSpear1, CraftResourceLib.ThrowingSpear2);

                    city.res_bow.toMenu(content, ItemResourceType.Bow, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.Bow);

                    city.res_longbow.toMenu(content, ItemResourceType.LongBow, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.LongBow);

                    city.res_crossbow.toMenu(content, ItemResourceType.Crossbow, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.CrossBow);

                    city.res_MithrilBow.toMenu(content, ItemResourceType.MithrilBow, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.MithrilBow);


                    city.res_HandCannon.toMenu(content, ItemResourceType.HandCannon, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.BronzeHandCannon);

                    city.res_HandCulvertin.toMenu(content, ItemResourceType.HandCulverin, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.BronzeHandCulverin);

                    city.res_Rifle.toMenu(content, ItemResourceType.Rifle, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.Rifle);

                    city.res_Blunderbuss.toMenu(content, ItemResourceType.Blunderbuss, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.Blunderbus);
                    content.newParagraph();

                    city.res_ballista.toMenu(content, ItemResourceType.Ballista, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.Ballista_Iron, CraftResourceLib.Ballista_Bronze);

                    city.res_Manuballista.toMenu(content, ItemResourceType.Manuballista, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.ManuBallista);

                    city.res_Catapult.toMenu(content, ItemResourceType.Catapult, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.Catapult);

                    city.res_SiegeCannonBronze.toMenu(content, ItemResourceType.SiegeCannonBronze, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.SiegeCannonBronze);

                    city.res_ManCannonBronze.toMenu(content, ItemResourceType.ManCannonBronze, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.ManCannonBronze);

                    city.res_SiegeCannonIron.toMenu(content, ItemResourceType.SiegeCannonIron, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.SiegeCannonIron);

                    city.res_SiegeCannonIron.toMenu(content, ItemResourceType.ManCannonIron, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Projectile);
                    blueprintButton(player, content, CraftResourceLib.ManCannonIron);

                    break;

                case ResourcesSubTab.Overview_Armor:

                    city.res_paddedArmor.toMenu(content, ItemResourceType.PaddedArmor, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Armor);
                    blueprintButton(player, content, CraftResourceLib.PaddedArmor);

                    city.res_HeavyPaddedArmor.toMenu(content, ItemResourceType.HeavyPaddedArmor, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Armor);
                    blueprintButton(player, content, CraftResourceLib.HeavyPaddedArmor);
                    
                    city.res_BronzeArmor.toMenu(content, ItemResourceType.BronzeArmor, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Armor);
                    blueprintButton(player, content, CraftResourceLib.BronzeArmor);

                    city.res_mailArmor.toMenu(content, ItemResourceType.IronArmor, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Armor);
                    blueprintButton(player, content, CraftResourceLib.MailArmor);

                    city.res_heavyMailArmor.toMenu(content, ItemResourceType.HeavyIronArmor, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Armor);
                    blueprintButton(player, content, CraftResourceLib.HeavyMailArmor);

                    city.res_LightPlateArmor.toMenu(content, ItemResourceType.LightPlateArmor, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Armor);
                    blueprintButton(player, content, CraftResourceLib.PlateArmor);

                    city.res_FullPlateArmor.toMenu(content, ItemResourceType.FullPlateArmor, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Armor);
                    blueprintButton(player, content, CraftResourceLib.FullPlateArmor);

                    city.res_MithrilArmor.toMenu(content, ItemResourceType.MithrilArmor, false, ref reachedBuffer, player, city, ResourcesSubTab.Stockpile_Armor);
                    blueprintButton(player, content, CraftResourceLib.MithrilArmor);
                    break;

                case ResourcesSubTab.Stockpile_Resources:
                    content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);

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

                    stockpile(ItemResourceType.Palisade);
                    stockpile(ItemResourceType.Toolkit);
                    stockpile(ItemResourceType.Wagon2Wheel);
                    stockpile(ItemResourceType.Wagon4Wheel);
                    stockpile(ItemResourceType.BlackPowder);
                    stockpile(ItemResourceType.GunPowder);
                    stockpile(ItemResourceType.LedBullet);

                    //content.newParagraph();
                    //HudLib.Description(content, DssRef.lang.Resource_StockPile_Info);
                    //GroupedResource.BufferIconInfo(content, false);
                    break;

                case ResourcesSubTab.Stockpile_Metals:
                    content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);

                    stockpile(ItemResourceType.IronOre_G);
                    stockpile(ItemResourceType.TinOre);
                    stockpile(ItemResourceType.CopperOre);
                    stockpile(ItemResourceType.LeadOre);
                    stockpile(ItemResourceType.SilverOre);                    
                    content.newParagraph();

                    stockpile(ItemResourceType.Iron_G);
                    stockpile(ItemResourceType.Tin);
                    stockpile(ItemResourceType.Copper);
                    stockpile(ItemResourceType.Lead);
                    stockpile(ItemResourceType.Silver);
                    stockpile(ItemResourceType.RawMithril);
                    stockpile(ItemResourceType.Sulfur);
                    content.newParagraph();

                    stockpile(ItemResourceType.Bronze);
                    stockpile(ItemResourceType.CastIron);
                    stockpile(ItemResourceType.BloomeryIron);
                    stockpile(ItemResourceType.Steel);
                    stockpile(ItemResourceType.Mithril);

                    break;
                case ResourcesSubTab.Stockpile_Weapons:
                    content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);
                    stockpile(ItemResourceType.SharpStick);
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
                    content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);

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
                    stockpile(ItemResourceType.Blunderbuss);

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
                    content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);
                    stockpile(ItemResourceType.HeavyPaddedArmor);
                    stockpile(ItemResourceType.PaddedArmor);
                    stockpile(ItemResourceType.IronArmor);
                    stockpile(ItemResourceType.HeavyIronArmor);
                    stockpile(ItemResourceType.LightPlateArmor);
                    stockpile(ItemResourceType.FullPlateArmor);
                    stockpile(ItemResourceType.MithrilArmor);
                    break;

                default:
                    content.h2(DssRef.lang.Work_OrderPrioTitle, HudLib.TitleColor_Head);
                    city.workTemplate.toHud(player, content, player.resourcesSubTab, city.GetFaction(), city);
                    break;
            }

            void stockpile(ItemResourceType item)
            {   
                GroupedResource res = city.GetGroupedResource(item);

                content.newLine();

                content.Add(new ArtButton(RbButtonStyle.HoverArea, 
                    new List<AbsRichBoxMember>{
                        new RbImage(res.amount >= res.goalBuffer ? SpriteName.WarsStockpileStop : SpriteName.WarsStockpileAdd),
                        new RbImage(ResourceLib.Icon(item))},null,
                        new RbTooltip((RichBoxContent content, object tag) =>
                        {
                            bool buffer = false;
                            city.GetGroupedResource(item).toMenu(content, item, false, ref buffer);                           
                        }
                        )));
                
                content.space();
               
                stockPileEdit(content, item, res);
            }
        }

        void stockPileEdit(RichBoxContent content, ItemResourceType item, GroupedResource res)
        {
            RbDragButton.RbDragButtonGroup(content, StockPileControls, new DragButtonSettings(DssConst.StockPileMinBound, DssConst.StockPileMaxBound, 100),
                (bool set, int value) => {
                    bool buffer = false;
                    var res = city.GetGroupedResource(item);
                    if (set)
                    {
                        res.goalBuffer = value;
                        city.SetGroupedResource(item, res);
                    }
                    return res.goalBuffer; });

            //int StockGetSet(bool set, int value)
            //{
            //    return 0;
            //}

            //RbAction hover = new RbAction(() => {
            //    RichBoxContent content = new RichBoxContent();
            //    bool buffer = false;
            //    city.GetGroupedResource(item).toMenu(content, item, false, ref buffer);//content.Add(new RichBoxText(LangLib.Item(item)));
            //    player.hud.tooltip.create(player, content, true);
            //});

            //for (int i = StockPileControls.Length - 1; i >= 0; i--)
            //{
            //    int change = -StockPileControls[i];
            //    content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(TextLib.PlusMinus(change)) },
            //        new RbAction1Arg<int>((int change) => {
            //            var res = city.GetGroupedResource(item);
            //            res.goalBuffer = Bound.Set(res.goalBuffer + change, DssConst.StockPileMinBound, DssConst.StockPileMaxBound);
            //            city.SetGroupedResource(item, res);

            //        }, change, SoundLib.menu), hover));

            //    content.space();
            //}

            //content.Add(new RbText(res.goalBuffer.ToString()));

            //for (int i = 0; i < StockPileControls.Length; i++)
            //{
            //    content.space();

            //    int change = StockPileControls[i];
            //    content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(TextLib.PlusMinus(change)) },
            //        new RbAction1Arg<int>((int change) => {
            //            var res = city.GetGroupedResource(item);
            //            res.goalBuffer = Bound.Set(res.goalBuffer + change, DssConst.StockPileMinBound, DssConst.StockPileMaxBound);
            //            city.SetGroupedResource(item, res);

            //        }, change, SoundLib.menu), hover));
            //}
        }

        //void purchaseOptions(RichBoxContent content)
        //{
        //    //if (city.battleGroup == null)
        //    {
        //        if (city.damages.HasValue())
        //        {
        //            content.newLine();
        //            content.Add(new ArtButton( RbButtonStyle.Primary,new List<AbsRichBoxMember>{
        //                            new RbImage(SpriteName.unitEmoteLove),
        //                            new RbText(DssRef.lang.CityOption_Repair),
        //                        },
        //                new RbAction1Arg<bool>(buyRepairAction, true, SoundLib.menuBuy),
        //                new RbTooltip(buyRepairToolTip, true),
        //                city.buyRepair(false, true)));
        //        }

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
        //            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>{
        //                            new RbImage(SpriteName.WarsGuardAdd),
        //                            new RbText( DssRef.lang.CityOption_ExpandGuardSize),
        //                        },
        //                new RbAction1Arg<int>(buyCityGuardsAction, count, SoundLib.menuBuy),
        //                new RbTooltip(buyGuardSizeToolTip, count),
        //                city.buyCityGuards(false, count)));
        //        }
        //        //content.Add(new RichBoxSpace());
        //        {
        //            int count = 5;
        //            content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { 
        //                    new RbText(string.Format(DssRef.lang.Hud_XTimes, count)) 
        //                },
        //                new RbAction1Arg<int>(buyCityGuardsAction, count, SoundLib.menuBuy),
        //                new RbTooltip(buyGuardSizeToolTip, count),
        //                city.buyCityGuards(false, count)));
        //        }

        //        content.newLine();
        //        {
        //            int count = 1;
        //            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>{
        //                            new RbImage(SpriteName.WarsGuard),
        //                            new RbText( DssRef.lang.CityOption_LowerGuardSize),
        //                        },
        //                new RbAction1Arg<int>(city.releaseGuardSize, count * DssConst.ExpandGuardSize, SoundLib.menuBuy),
        //                new RbTooltip(releaseGuardSizeToolTip, count),
        //                city.canReleaseGuardSize(count)));
        //        }
        //        //if (!city.nobelHouse && city.canEverGetNobelHouse())
        //        //{
        //        //    content.Button(DssRef.lang.Building_NobleHouse,
        //        //            new RbAction(city.buyNobelHouseAction, SoundLib.menuBuy),
        //        //            new RbAction(buyNobelhouseTooltip),
        //        //            city.canBuyNobelHouse());
        //        //}
        //    }
        //}

        void blueprintButton(LocalPlayer player, RichBoxContent content, CraftBlueprint blueprint, CraftBlueprint optionalBp = null, bool roomForAnotherButton = false)
        {
            
            content.Add(new RbTab(0.65f));//roomForAnotherButton? 0.65f : 0.8f));

            var tooltip = new RbTooltip(blueprintTooltip, new BlueprintTooltipArgs()
            {
                blueprint = blueprint,
                optionalBp = optionalBp
            });

            if (blueprint == CraftResourceLib.Food1)
            {
                tooltip.tagId = Tooltip.Food_BlueprintId;
            }

            content.Add(new ArtButton( RbButtonStyle.HoverArea,new List<AbsRichBoxMember> {
                new RbImage(SpriteName.WarsBluePrint)
            },
            null, tooltip));

        }

        class BlueprintTooltipArgs
        {
            public CraftBlueprint blueprint;
            public CraftBlueprint optionalBp;
        }

        void blueprintTooltip(RichBoxContent content, object tag)
        {
            //hover
            BlueprintTooltipArgs args = (BlueprintTooltipArgs)tag;
            //RichBoxContent content = new RichBoxContent();
            content.h2(DssRef.lang.Blueprint_Title, HudLib.TitleColor_Head);
            args.blueprint.toMenu(content, city);
            if (args.optionalBp != null)
            { 
                content.newParagraph();
                args.optionalBp.toMenu(content, city);
            }

            args.blueprint.requirementToHud(content, city, out _);

            content.Add(new RbSeperationLine());
            content.newParagraph();
            content.h2(DssRef.lang.MenuTab_Resources).overrideColor = HudLib.TitleColor_Label;
            args.blueprint.listResources(content, city, args.optionalBp);

            if (args.blueprint.levelRequirement > ExperienceLevel.Beginner_1)
            {
                content.newLine();

                HudLib.Experience(content, args.blueprint.experienceType, city.GetTopSkill(args.blueprint.experienceType));
            }

            //player.hud.tooltip.create(player, content, true, blueprint.tooltipId);
        }

        

        void conscriptTab(RichBoxContent content)
        {
            new ConscriptMenu().ToHud(city, player, content);
        }

        void defenceTab(RichBoxContent content)
        {
            new DefenceMenu().ToHud(city, player, content);
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
            city.tradeTemplate.toHud(player,content, city.GetFaction(), city);
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

        //void buyRepairAction(bool all)
        //{
        //    city.buyRepair(true, all);
        //}

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

            //player.hud.tooltip.create(player, content, true);
        }

        //public void buyRepairToolTip(RichBoxContent content, object tag)
        //{
        //    bool all = (bool)tag;
        //    //RichBoxContent content = new RichBoxContent();
        //    int count, cost;
        //    city.repairCountAndCost( all, out count, out cost);

        //    content.text(TextLib.Quote(DssRef.lang.CityOption_Repair_Description));
        //    content.newLine();
        //    content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
        //    content.newLine();
        //    HudLib.ResourceCost(content, ResourceType.Gold, cost, player.faction.gold);
        //    content.newLine();
        //    content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
        //    content.newLine();
        //    content.icontext(SpriteName.unitEmoteLove, string.Format(DssRef.lang.CityOption_RepairGain, city.damages.Int()));
            
        //    //player.hud.tooltip.create(player, content, true);
        //}

        //void buyCityGuardsAction(int count)
        //{
        //    city.buyCityGuards(true, count);
        //}

        //public void releaseGuardSizeToolTip(RichBoxContent content, object tag)//int count)
        //{
        //    int count = (int)tag;
        //    //RichBoxContent content = new RichBoxContent();

        //    if (city.canReleaseGuardSize(count))
        //    {
        //        content.h2(DssRef.lang.Hud_PurchaseTitle_Cost).overrideColor = HudLib.TitleColor_Label;
        //        content.newLine();
        //        content.icontext(SpriteName.WarsGuard, string.Format(DssRef.lang.Hud_IncreaseMaxGuardCount, -DssConst.ExpandGuardSize * count));
                
                
        //        content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;
        //        HudLib.ItemCount(content, SpriteName.rtsIncome, DssRef.lang.ResourceType_Gold, (DssConst.ReleaseGuardSizeGain * count).ToString());

        //    }
        //    else
        //    {
        //        content.Add(new RbText(DssRef.lang.Hud_Purchase_MinCapacity, Color.Red));
        //    }

        //    //player.hud.tooltip.create(player, content, true);
        //}

        //public void buyGuardSizeToolTip(RichBoxContent content, object tag)//int count)
        //{
        //    int count = (int)tag;
        //    //RichBoxContent content = new RichBoxContent();

        //    if (city.canIncreaseGuardSize(count, false))
        //    {
        //        content.h2(DssRef.lang.Hud_PurchaseTitle_Cost).overrideColor = HudLib.TitleColor_Label;
        //        content.newLine();
        //        HudLib.ResourceCost(content, ResourceType.Gold, DssConst.ExpandGuardSizeCost * count, player.faction.gold);
        //        content.newLine();
        //        //content.icontext(SpriteName.rtsUpkeepTime, "Upkeep +" + city.GuardUpkeep(City.ExpandGuardSize * count).ToString());
        //        HudLib.Upkeep(content, city.GuardUpkeep(DssConst.ExpandGuardSize * count));

        //        content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;

        //        content.icontext(SpriteName.WarsGuardAdd, string.Format(DssRef.lang.Hud_IncreaseMaxGuardCount, TextLib.PlusMinus( DssConst.ExpandGuardSize * count)));
        //    }
        //    else 
        //    {
        //        content.Add(new RbText(DssRef.lang.Hud_Purchase_MaxCapacity, Color.Red));
        //        content.newLine();
        //        content.Add(new RbText(DssRef.lang.Hud_GuardCount_MustExpandCityMessage, Color.Red));
        //    }

        //    //player.hud.tooltip.create(player, content, true);
        //}

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
        Progress,
        Mix,
        Help,
        Defence,

        Casual_Recruit,
        Casual_Build,
        NUM_NONE
    }

    enum MixTabEditType
    { 
        None,
        Stockpile,
        WorkPrio,
        BlackMarket,
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

        Work_Resources,
        Work_Metals,
        Work_Weapons,
        Work_Projectile,
        Work_Armor,

        Auto,
        
    }

    

    //enum WorkSubTab
    //{
    //    Priority_Resources,
    //    Priority_Metals,
    //    Priority_Weapons,
    //    Priority_Armor,
        
    //    NUM,
    //        Experience,
    //}

    enum ProgressSubTab
    { 
        Technology,
        Experience,
        Schools,
        Research,
        NUM
    }
}
