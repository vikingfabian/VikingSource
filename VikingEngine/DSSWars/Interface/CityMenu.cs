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
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.GameState.BattleLab;
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
        public static List<MenuTab> Tabs;
        public static List<MenuTab> CasualTabs = new List<MenuTab> { MenuTab.Info, MenuTab.Casual_Recruit, MenuTab.Casual_Build, MenuTab.Tag };

        protected Players.LocalPlayer player;
        protected City city;
        

        public static readonly AutomationFocus[] AvailableAutomationFocuses =
        {
            AutomationFocus.NoFocus,
            AutomationFocus.Food,
            AutomationFocus.Grow,
            AutomationFocus.Export,
            AutomationFocus.Military
        };

        public static void InitGame()
        {
            Tabs = new List<MenuTab>() {
                MenuTab.Info, MenuTab.Resources, MenuTab.BlackMarket,
                MenuTab.Build, MenuTab.Delivery, MenuTab.Conscript, MenuTab.Defence, MenuTab.Progress,
                MenuTab.Tag};

            if (DssRef.difficulty.setting_gameMode == GameModeMainType.Spectator)
            {
                Tabs.Insert(1, MenuTab.God_Recruit);
            }
            else
            {
                Tabs.Add(MenuTab.Help);
            }
        }

        public CityMenu(Players.LocalPlayer player, City city, RichBoxContent content)
        {
            this.player = player;
            this.city = city;

            if (!DssRef.storage.gameRuleset.centralGold)
            {
                content.newLine();
                content.Add(new RbImage(SpriteName.rtsMoney));
                content.space();
                content.Add(new RbText(DssRef.lang.ResourceType_Gold + ": " + TextLib.LargeNumber(city.money.GetGold()),  HudLib.NegativeRed(city.money.GetGold())));
                content.Add(new RbNewLine());
            }

            content.newLine();

            if (city.automateCity && !player.profile.casualControls) 
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
                    var text = new RbText(LangLib.Tab(availableTabs[i], out string description, out var tabColor));

                    if (tabColor == null)
                    {
                        text.overrideColor = HudLib.RbSettings.tabSelected.Color;
                    }
                    else
                    {
                        text.overrideColor = tabColor;
                    }

                    AbsRbAction enter = null;
                    if (description != null)
                    {
                        enter = new RbAction(() =>
                        {
                            RichBoxContent content = new RichBoxContent();
                            content.text(description).overrideColor = HudLib.InfoYellow_Light;

                            player.hud.tooltip.create(player, content, true);
                        }, RbSoundType.NUM_NONE);
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
               
                var tabGroup = new ArtTabgroup(tabs, tabSel, player.cityTabClick);
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

                    case MenuTab.Resources:
                        resourcesToMenu(content);
                        break;

                    case MenuTab.BlackMarket:
                        BlackMarketResources.ToHud(player, content, city);
                        break;

                    case MenuTab.Conscript:
                        conscriptTab(content);
                        break;

                    case MenuTab.Defence:
                        defenceTab(content);
                        break;

                    case MenuTab.Delivery:
                        deliveryTab(content);
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

                    //case MenuTab.Mix:
                    //    mixTab(content);
                    //    break;

                    case MenuTab.Tag:
                        tagsToMenu(content);
                        break;

                    case MenuTab.Help:
                        helpTab(content);
                        break;

                    case MenuTab.God_Recruit:
                        godRecruitTab(content);
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


        void godRecruitTab(RichBoxContent content)
        {
            GodConscript.ToHud(content, addSoldier);

            void addSoldier(int count)
            {
                SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                {
                    conscript = new ConscriptProfile()
                    {
                        weapon = BattleLabStorage.Singleton.setup.selectedWeapon,
                        armorLevel = Resource.ItemResourceType.PaddedArmor,
                        training = TrainingLevel.Basic,
                        specialization = SpecializationType.Traditional,
                    }
                };

                var army = city.recruitToClosestArmy();

                if (army == null)
                {
                    army = city.GetFaction().NewArmy(city.recruitToTile);
                }

                for (int i = 0; i < count; ++i)
                {                    
                    new SoldierGroup(army, SoldierProfile, army.position);                    
                }

                army.setAsStartArmy();
                //army.GetArmy().OnSoldierPurchaseCompleted();
            }
        }

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
                    content.newLine();

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
                            new RbText(string.Format(DssRef.lang.Hud_XTimes, counts), player.faction.hasGold(option.FullPrice * counts, city)? HudLib.AvailableColor_Dark : HudLib.NotAvailableColor_Dark),
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
                HudLib.ResourceCost(content, ResourceType.Gold, recruitOption.purchaseOption.FullPrice * recruitOption.count, (int)player.faction.GetGold(city));

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
            content.Add(new RbImage(SpriteName.WarsHammer));
            content.space();
            content.Add(new RbText(DssRef.lang.Tutorial_HighPriority));

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
                    }, workSubTab, RbSoundType.Tab), new RbTooltip_Text(description));
                //subTab.setGroupSelectionColor(HudLib.RbSettings, player.progressSubTab == workSubTab);
                content.Add(subTab);
                //content.space();
            }
            content.newParagraph();

            switch (player.progressSubTab)
            {
                default:
                    new TechnologyHud(player, city).technologyHud(content, city.GetFaction());
                    break;

                case ProgressSubTab.Experience:
                    experienceTab(content);
                    break;

                case ProgressSubTab.Schools:
                    new SchoolMenu().ToHud(city, player, content);
                    break;

                case ProgressSubTab.Research:
                    new ResearchMenu().ToHud(city, player, content, player.hud.objMenu.menu);
                    break;
            }
            
        }

        //void mixTab(RichBoxContent content)
        //{
        //    for (ResourceManagementType managementType = 0; managementType < ResourceManagementType.Auto; managementType++)
        //    {
        //        for (ResourceGroup group = 0; group < ResourceGroup.NUM; group++)
        //        {
                    
        //            if (group == ResourceGroup.Mint)
        //            {
        //                bool includeMint = managementType == ResourceManagementType.Work && city.buildingStructure.CoinMinter_count > 0;
        //                if (!includeMint)
        //                {
        //                    continue;
        //                }

        //                IconName.Tab(group, out SpriteName groupIcon, out string groupName);
        //                var tab = new ResourcesSubTab(managementType, group);
        //                content.Add(new ArtOption(player.resourcesSubTab.EqualTab(tab), 
        //                    new List<AbsRichBoxMember> { new RbImage(groupIcon) },
        //                    new RbAction1Arg<ResourcesSubTab>((ResourcesSubTab resourcesSubTab) =>
        //                    {
        //                        player.resourcesSubTab = resourcesSubTab;
        //                    }, tab, RbSoundType.Option)));
        //                content.space();
        //            }

        //        }
        //    }


        //    //if (player.tutorial == null || player.tutorial.DisplayStockpile())
        //    {
        //        for (ResourcesSubTab resourcesSubTab = 0; resourcesSubTab <= ResourcesSubTab.Overview_Armor; ++resourcesSubTab)
        //        {
        //            var tabContent = new RichBoxContent();
        //            //string text = null;
        //            switch (resourcesSubTab)
        //            {
        //                case ResourcesSubTab.Overview_Resources:
        //                    tabContent.Add(new RbText(DssRef.lang.Resource_Tab_Overview));
        //                    tabContent.space();
        //                    tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
        //                    break;

        //                case ResourcesSubTab.Overview_Metals:
        //                    tabContent.Add(new RbImage(SpriteName.WarsResource_Iron));
        //                    break;

        //                case ResourcesSubTab.Overview_Weapons:
        //                    tabContent.Add(new RbImage(SpriteName.WarsResource_Sword));
        //                    break;

        //                case ResourcesSubTab.Overview_Projectile:
        //                    tabContent.Add(new RbImage(SpriteName.WarsResource_Bow));
        //                    break;

        //                case ResourcesSubTab.Overview_Armor:
        //                    tabContent.Add(new RbImage(SpriteName.cmdMailArmor));
        //                    break;


        //            }
        //            var subTab = new ArtOption(player.resourcesSubTab == resourcesSubTab,tabContent,
        //                new RbAction1Arg<ResourcesSubTab>((ResourcesSubTab resourcesSubTab) =>
        //                {
        //                    player.resourcesSubTab = resourcesSubTab;
        //                }, resourcesSubTab, RbSoundType.Option));
        //            //subTab.setGroupSelectionColor(HudLib.RbSettings, player.resourcesSubTab == resourcesSubTab);
        //            content.Add(subTab);
        //            content.space();
        //        }
        //        content.newParagraph();
        //    }

        //    bool reachedBuffer = false;

        //    switch (player.resourcesSubTab)
        //    {
        //        case ResourcesSubTab.Overview_Resources:
        //            {
        //                ItemResourceType item = ItemResourceType.Wood_Group;
        //                mixResource(item, false);
        //                work(item, WorkPriorityType.wood);
        //                work(item, WorkPriorityType.move);
        //                blackMarket(item);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Stone_G;
        //                mixResource(item, false);
        //                work(item, WorkPriorityType.stone);
        //                blackMarket(item);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Fuel_G;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Fuel1, CraftResourceLib.Charcoal);
        //                work(item, WorkPriorityType.farmfuel);
        //                work(item, WorkPriorityType.craftFuel);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.RawFood_Group;
        //                mixResource(item, false);
        //                work(item, WorkPriorityType.farmfood);
        //                blackMarket(item);
        //                end(item);
        //            }                    

        //            {
        //                ItemResourceType item = ItemResourceType.Food_G;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Food1, CraftResourceLib.Food2);
        //                work(item, WorkPriorityType.craftFood);
        //                blackMarket(item);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Beer;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Beer);
        //                work(item, WorkPriorityType.craftBeer);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.CoolingFluid;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.CoolingFluid);
        //                work(item, WorkPriorityType.craftCoolingFluid);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.SkinLinen_Group;
        //                mixResource(item, false);
        //                work(item, WorkPriorityType.farmlinen);
        //                blackMarket(item);
        //                end(item);
        //            }
        //            content.newParagraph();
        //            {
        //                ItemResourceType item = ItemResourceType.Toolkit;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Toolkit);
        //                work(item, WorkPriorityType.craftToolkit);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Wagon2Wheel;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Wagon2Wheel);
        //                work(item, WorkPriorityType.craftWagonLight);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Wagon4Wheel;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Wagon4Wheel);
        //                work(item, WorkPriorityType.craftWagonHeavy);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.BlackPowder;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.BlackPowder);
        //                work(item, WorkPriorityType.craftBlackPowder);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.LedBullet;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.LedBullets);
        //                work(item, WorkPriorityType.craftBullet);
        //                end(item);
        //            }

        //            break;
        //        case ResourcesSubTab.Overview_Metals:

        //            {
        //                ItemResourceType item = ItemResourceType.IronOre_G;
        //                mixResource(item, false);
        //                work(item, WorkPriorityType.miningIron);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.TinOre;
        //                mixResource(item, false);
        //                work(item, WorkPriorityType.miningTin);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.CopperOre;
        //                mixResource(item, false);
        //                work(item, WorkPriorityType.miningCopper);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.LeadOre;
        //                mixResource(item, false);
        //                work(item, WorkPriorityType.miningLead);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.SilverOre;
        //                mixResource(item, false);
        //                work(item, WorkPriorityType.miningSilver);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Sulfur;
        //                mixResource(item, false);
        //                work(item, WorkPriorityType.miningSulfur);
        //                end(item);
        //            }
        //            content.newParagraph();
        //            {
        //                ItemResourceType item = ItemResourceType.Iron_G;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Iron, CraftResourceLib.Iron_AndCooling);
        //                work(item, WorkPriorityType.smeltIron);
        //                blackMarket(item);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Tin;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Tin);
        //                work(item, WorkPriorityType.smeltTin);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Copper;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Copper, CraftResourceLib.Cupper_AndCooling);
        //                work(item, WorkPriorityType.smeltCopper);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Lead;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Lead);
        //                work(item, WorkPriorityType.smeltLead);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Silver;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Silver, CraftResourceLib.Silver_AndCooling);
        //                work(item, WorkPriorityType.smeltSilver);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.RawMithril;
        //                mixResource(item, false);
        //                work(item, WorkPriorityType.miningMithril);
        //                end(item);
        //            }
        //            content.newParagraph();

        //            {
        //                ItemResourceType item = ItemResourceType.Bronze;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Bronze);
        //                work(item, WorkPriorityType.craftBronze);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.CastIron;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.CastIron);
        //                work(item, WorkPriorityType.craftCastIron);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.BloomeryIron;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.BloomeryIron);
        //                work(item, WorkPriorityType.craftBloomeryIron);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Steel;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Steel, CraftResourceLib.Steel_AndCooling);
        //                work(item, WorkPriorityType.craftSteel);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Mithril;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Mithril);
        //                work(item, WorkPriorityType.craftMithril);
        //                end(item);
        //            }
        //            break;

        //        case ResourcesSubTab.Overview_Weapons:

        //            {
        //                ItemResourceType item = ItemResourceType.SharpStick;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.SharpStick);
        //                work(item, WorkPriorityType.craftSharpStick);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.HandSpear;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.HandSpearIron, CraftResourceLib.HandSpearBronze);
        //                work(item, WorkPriorityType.craftHandSpear);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.BronzeSword;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.BronzeSword);
        //                work(item, WorkPriorityType.craftBronzeSword);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.ShortSword;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.ShortSword);
        //                work(item, WorkPriorityType.craftShortSword);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Sword;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Sword);
        //                work(item, WorkPriorityType.craftSword);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.LongSword;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.LongSword);
        //                work(item, WorkPriorityType.craftLongSword);
        //                end(item);
        //            }
        //            content.newParagraph();
        //            {
        //                ItemResourceType item = ItemResourceType.Warhammer;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.WarhammerIron, CraftResourceLib.WarhammerBronze);
        //                work(item, WorkPriorityType.craftWarhammer);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.TwoHandSword;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.TwoHandSword);
        //                work(item, WorkPriorityType.craftTwoHandSword);
        //                end(item);
        //            }
        //            //{
        //            //    ItemResourceType item = ItemResourceType.KnightsLance;
        //            //    mixResource(item, false);
        //            //    HudLib.blueprint(content, CraftResourceLib.KnightsLance);
        //            //    work(item, WorkPriorityType.craftKnightsLance);
        //            //    end(item);
        //            //}
        //            {
        //                ItemResourceType item = ItemResourceType.MithrilSword;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.MithrilSword);
        //                work(item, WorkPriorityType.craftMithrilSword);
        //                end(item);
        //            }
        //            break;

        //        case ResourcesSubTab.Overview_Projectile:

        //            {
        //                ItemResourceType item = ItemResourceType.SlingShot;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Slingshot);
        //                work(item, WorkPriorityType.craftSlingshot);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.ThrowingSpear;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.ThrowingSpear1,CraftResourceLib.ThrowingSpear2);
        //                work(item, WorkPriorityType.craftThrowingspear);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Bow;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Bow);
        //                work(item, WorkPriorityType.craftBow);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.LongBow;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.LongBow);
        //                work(item, WorkPriorityType.craftLongbow);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Crossbow;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.CrossBow);
        //                work(item, WorkPriorityType.craftCrossbow);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.MithrilBow;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.MithrilBow);
        //                work(item, WorkPriorityType.craftMithrilbow);
        //                end(item);
        //            }
        //            content.newParagraph();
        //            {
        //                ItemResourceType item = ItemResourceType.HandCannon;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.BronzeHandCannon);
        //                work(item, WorkPriorityType.craftHandCannon);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.HandCulverin;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.BronzeHandCulverin);
        //                work(item, WorkPriorityType.craftHandCulverin);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Rifle;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Rifle);
        //                work(item, WorkPriorityType.craftRifle);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Blunderbuss;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Blunderbus);
        //                work(item, WorkPriorityType.craftBlunderbus);
        //                end(item);
        //            }
        //            content.newParagraph();
        //            {
        //                ItemResourceType item = ItemResourceType.Ballista;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Ballista_Iron,CraftResourceLib.Ballista_Bronze);
        //                work(item, WorkPriorityType.craftBallista);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Manuballista;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.ManuBallista);
        //                work(item, WorkPriorityType.craftManuBallista);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.Catapult;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.Catapult);
        //                work(item, WorkPriorityType.craftCatapult);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.SiegeCannonBronze;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.SiegeCannonBronze);
        //                work(item, WorkPriorityType.craftSiegeCannonBronze);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.ManCannonBronze;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.ManCannonBronze);
        //                work(item, WorkPriorityType.craftManCannonBronze);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.SiegeCannonIron;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.SiegeCannonIron);
        //                work(item, WorkPriorityType.craftSiegeCannonIron);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.ManCannonIron;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.ManCannonIron);
        //                work(item, WorkPriorityType.craftManCannonIron);
        //                end(item);
        //            }

        //            break;

        //        case ResourcesSubTab.Overview_Armor:

        //            {
        //                ItemResourceType item = ItemResourceType.PaddedArmor;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.PaddedArmor);
        //                work(item, WorkPriorityType.craftPaddedArmor);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.HeavyPaddedArmor;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.HeavyPaddedArmor);
        //                work(item, WorkPriorityType.craftHeavyPaddedArmor);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.BronzeArmor;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.BronzeArmor);
        //                work(item, WorkPriorityType.craftBronzeArmor);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.IronArmor;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.MailArmor);
        //                work(item, WorkPriorityType.craftMailArmor);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.HeavyIronArmor;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.HeavyMailArmor);
        //                work(item, WorkPriorityType.craftHeavyMailArmor);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.LightPlateArmor;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.PlateArmor);
        //                work(item, WorkPriorityType.craftPlateArmor);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.FullPlateArmor;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.FullPlateArmor);
        //                work(item, WorkPriorityType.craftFullPlateArmor);
        //                end(item);
        //            }
        //            {
        //                ItemResourceType item = ItemResourceType.MithrilArmor;
        //                mixResource(item, false);
        //                HudLib.blueprint(content, CraftResourceLib.MithrilArmor);
        //                work(item, WorkPriorityType.craftMithrilArmor);
        //                end(item);
        //            }

        //            break;
        //    }

        //    void mixResource(ItemResourceType item, bool safeGuard)
        //    {
        //        content.newLine();

        //        var typeIcon = ResourceLib.Icon(item);
        //        var typeName = LangLib.Item(item);
        //        var city_res = city.GetGroupedResource(item);

        //        var infoContent = new List<AbsRichBoxMember>(2);
        //        infoContent.Add(new RbImage(typeIcon));
        //        infoContent.Add(new RbSpace());
        //        var amountText = new RbText(city_res.amount.ToString());
        //        amountText.overrideColor = Color.White;
        //        infoContent.Add(amountText);

        //        var infoButton = new RbButton(infoContent, null, new RbAction(() =>
        //        {
        //            RichBoxContent content = new RichBoxContent();
        //            content.Add(new RbText(typeName));
        //            player.hud.tooltip.create(player, content, true);
        //        }));
        //        infoButton.overrideBgColor = HudLib.InfoYellow_BG;

        //        content.Add(infoButton);
        //        content.space();
                

        //        if (item != ItemResourceType.Water_G &&
        //           item != ItemResourceType.Gold &&
        //           item != ItemResourceType.Men)
        //        {
        //            var stockpileContent = new List<AbsRichBoxMember>(2);
        //            stockpileContent.Add(new RbText(city_res.goalBuffer.ToString()));

        //            bool reached = city_res.amount >= city_res.goalBuffer;
        //            reachedBuffer |= reached;
        //            SpriteName stockIcon;
        //            if (safeGuard)
        //            {
        //                stockIcon = SpriteName.WarsStockpileAdd_Protected;
        //            }
        //            else if (reached)
        //            {
        //                stockIcon = SpriteName.WarsStockpileStop;
        //            }
        //            else
        //            {
        //                stockIcon = SpriteName.WarsStockpileAdd;
        //            }
        //            var icon = new RbImage(stockIcon);
        //            stockpileContent.Add(icon);


        //            var stockpileButton = new ArtButton( RbButtonStyle.HoverArea, stockpileContent, 
        //                new RbAction(() =>
        //                {
        //                    player.mixTabEditType = MixTabEditType.Stockpile;
        //                    player.mixTabItem = item;
        //                }, RbSoundType.Default), 
        //                new RbAction(()=> {
        //                    var content = new RichBoxContent();
        //                    content.text(DssRef.lang.Resource_Tab_Stockpile);
        //                    player.hud.tooltip.create(player, content, true);
        //                }));

        //            content.Add(new RbTab(0.22f));
        //            content.Add(stockpileButton);
        //            content.space();
        //        }
        //    }

            

        //    void work(ItemResourceType item, WorkPriorityType workPriorityType)
        //    {
        //        LangLib.WorkNameIcon(workPriorityType, out string name, out SpriteName workIcon, out SpriteName typeIcon);
        //        var buttonContent = new RichBoxContent();
        //        buttonContent.Add(new RbImage(workIcon));
        //        var prio = city.workTemplate.GetWorkPriority(workPriorityType);
        //        buttonContent.Add(new RbText(prio.value.ToString()));

        //        var button = new RbButton(buttonContent, new RbAction(() =>
        //        {
        //            player.mixTabEditType = MixTabEditType.WorkPrio;
        //            player.mixWorkType = workPriorityType;
        //            player.mixTabItem = item;
        //        }, RbSoundType.Default),
        //        new RbAction(()=> 
        //        {
        //            var content = new RichBoxContent();
        //            HudLib.Label(content, DssRef.lang.Work_OrderPrioTitle);
        //            content.text(name);
        //            player.hud.tooltip.create(player, content, true);
        //        }));

        //        //content.Add(new RichBoxTab(0.5f));
        //        content.Add(button);
        //        content.space();
        //    }

        //    void blackMarket(ItemResourceType item)
        //    {
        //        var buttonContent = new RichBoxContent();
        //        buttonContent.Add(new RbText("BM"));

        //        var button = new ArtButton( RbButtonStyle.Primary,buttonContent, new RbAction(() =>
        //        {
        //            player.mixTabEditType = MixTabEditType.BlackMarket;
        //            player.mixTabItem = item;
        //        }, RbSoundType.Default),
        //        new RbAction(() =>
        //        {
        //            var content = new RichBoxContent();
        //            HudLib.Label(content, DssRef.lang.Hud_BlackMarket);
                   
        //            player.hud.tooltip.create(player, content, true);
        //        }));
        //        //button.overrideBgColor = Color.DarkViolet;
        //        content.Add(button);
        //        content.space();
        //    }

        //    void end(ItemResourceType item)
        //    {
        //        if (player.mixTabEditType != MixTabEditType.None &&
        //           item == player.mixTabItem)
        //        {
        //            var city_res = city.GetGroupedResource(item);

        //            content.newLine();
        //            switch (player.mixTabEditType)
        //            {
        //                case MixTabEditType.Stockpile:
        //                    //stockPileEdit(content, item, city_res);
        //                    break;
        //                case MixTabEditType.WorkPrio:
        //                    LangLib.WorkNameIcon(player.mixWorkType, out string name, out SpriteName workIcon, out SpriteName typeIcon);
        //                    city.workTemplate.GetWorkPriority(player.mixWorkType).toHud(player, content, name, workIcon, typeIcon, player.mixWorkType, player.faction, city);
        //                    break;
        //                case MixTabEditType.BlackMarket:
        //                    BlackMarketResources.ResourceToHud(item, player, content, city);
        //                    break;
        //            }
        //        }
        //    }
        //}
      
        void experienceTab(RichBoxContent content)
        {
            HudLib.Label(content, DssRef.lang.Experience_TopExperience);
            experience( WorkExperienceType.Farm, SpriteName.WarsWorkFarm, DssRef.lang.ExperienceType_Farm, city.cityExperienceLevels.levels_Farm);
            experience(WorkExperienceType.AnimalCare, SpriteName.WarsBuild_HenPen, DssRef.lang.ExperienceType_AnimalCare, city.cityExperienceLevels.levels_AnimalCare);
            experience(WorkExperienceType.HouseBuilding, SpriteName.WarsHammer, DssRef.lang.ExperienceType_HouseBuilding, city.cityExperienceLevels.levels_HouseBuilding);
            experience(WorkExperienceType.WoodWork, SpriteName.WarsResource_Wood, DssRef.lang.ExperienceType_WoodWork, city.cityExperienceLevels.levels_WoodCutter);
            experience(WorkExperienceType.StoneCutter, SpriteName.WarsResource_Stone, DssRef.lang.ExperienceType_StoneCutter, city.cityExperienceLevels.levels_StoneCutter);
            experience(WorkExperienceType.Mining, SpriteName.WarsWorkMine, DssRef.lang.ExperienceType_Mining, city.cityExperienceLevels.levels_Mining);
            experience(WorkExperienceType.Transport, SpriteName.WarsWorkMove, DssRef.lang.ExperienceType_Transport, city.cityExperienceLevels.levels_Transport);
            experience(WorkExperienceType.Cook, SpriteName.WarsResource_Food, DssRef.lang.ExperienceType_Cook, city.cityExperienceLevels.levels_Cook);
            experience(WorkExperienceType.Fletcher, SpriteName.WarsFletcherArrowIcon, DssRef.lang.ExperienceType_Fletcher, city.cityExperienceLevels.levels_Fletcher);
            experience(WorkExperienceType.Smelting, SpriteName.WarsWorkSmelting, DssRef.lang.ExperienceType_Smelting, city.cityExperienceLevels.levels_Smelting);
            experience(WorkExperienceType.CastMetal, SpriteName.WarsWorkCasting, DssRef.lang.ExperienceType_Casting, city.cityExperienceLevels.levels_Casting);
            experience(WorkExperienceType.CraftMetal, SpriteName.WarsResource_Iron, DssRef.lang.ExperienceType_CraftMetal, city.cityExperienceLevels.levels_CraftMetal);
            experience(WorkExperienceType.CraftArmor, SpriteName.WarsResource_IronArmor, DssRef.lang.ExperienceType_CraftArmor, city.cityExperienceLevels.levels_CraftArmor);
            //experience(SpriteName.WarsResource_Sword, DssRef.lang.ExperienceType_CraftWeapon, city.cityExperienceLevels.levels_CraftWeapon);
            experience(WorkExperienceType.CraftFuel, SpriteName.WarsResource_Fuel, DssRef.lang.ExperienceType_CraftFuel, city.cityExperienceLevels.levels_CraftFuel);
            experience(WorkExperienceType.Chemistry, SpriteName.WarsBuild_Chemist, DssRef.lang.ExperienceType_Chemist, city.cityExperienceLevels.levels_Chemistry);

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
                    }, prio, RbSoundType.Option));
                content.Add(option);
            }
            
            void experience(WorkExperienceType experienceType, SpriteName typeIcon, string typeName, WorkExperienceLevels experienceLevels)
            {
                ExperienceLevel level = experienceLevels.Max();

                content.newLine();
                content.Add(new RbImage(typeIcon));
                content.space();
                var typeNameText = new RbText(typeName + ":");
                typeNameText.overrideColor = HudLib.TitleColor_TypeName;
                content.Add(typeNameText);

                content.Add(new RbTab(0.4f));
                content.Add(new RbImage(LangLib.ExperienceLevelIcon(level)));
                content.Add(new RbText(LangLib.ExperienceLevel(level)));

                content.Add(new RbTab(0.7f));               
                content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> { new RbImage(SpriteName.cmdSpyglass) }, null, new RbTooltip(infoTooltip, experienceLevels)));
                player.hud.pins.toggleButton(content, new CityHudPinId(city.myIndex, new HudPin(experienceType)));

                void infoTooltip(RichBoxContent content, object tag)
                {
                    WorkExperienceLevels experienceLevels = (WorkExperienceLevels)tag;

                    int total = 0;
                    level(ExperienceLevel.Beginner_1, experienceLevels.Beginner_1_count);
                    level(ExperienceLevel.Practitioner_2, experienceLevels.Practitioner_2_count);
                    level(ExperienceLevel.Expert_3, experienceLevels.Expert_3_count);
                    level(ExperienceLevel.Master_4, experienceLevels.Master_4_count);
                    level(ExperienceLevel.Legendary_5, experienceLevels.Legendary_5_count);

                    if (total == 0)
                    {
                        content.text(DssRef.lang.Hud_EmptyList, HudLib.InfoYellow_Light);
                    }

                    void level(ExperienceLevel level, int count)
                    {
                        if (count > 0)
                        {
                            total++;
                            content.Add(new RbText(count.ToString()));
                            content.Add(new RbImage(SpriteName.WarsWorker));
                            content.Add(new RbTab(0.16f));
                            content.Add(new RbImage(LangLib.ExperienceLevelIcon(level)));
                            content.Add(new RbText(LangLib.ExperienceLevel(level)));
                            content.newLine();
                        }
                    }
                }
            }
        }

        public void tagsToMenu(RichBoxContent content)
        {
            for (TagSubTab subTabType = 0; subTabType < TagSubTab.NUM; ++subTabType)
            {
                var tabContent = new RichBoxContent();
                string description = null;
                //string text = null;
                switch (subTabType)
                {
                    case TagSubTab.Tag:
                        tabContent.Add(new RbImage(SpriteName.warsFolder_carton, 0.7f));
                        tabContent.space(0.6f);
                        tabContent.Add(new RbText(DssRef.lang.MenuTab_Tag));
                        description = DssRef.lang.ObjectTag_Description;
                        break;

                    case TagSubTab.HudPin:
                        tabContent.Add(new RbImage(SpriteName.HudPinIcon, 0.7f));
                        tabContent.space(0.6f);
                        tabContent.Add(new RbText(DssRef.lang.HudPins));
                        description = DssRef.lang.HudPins_Description;
                        break;

                }

                var subTab = new ArtButton(player.tagSubTab == subTabType ? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected, tabContent,
                    new RbAction1Arg<TagSubTab>((TagSubTab subTabType) =>
                    {
                        player.tagSubTab = subTabType;
                    }, subTabType, RbSoundType.Tab), new RbTooltip_Text(description));
                content.Add(subTab);
            }
            content.newParagraph();

            switch (player.tagSubTab)
            {
                default:
                    //__
                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Tag_ViewOnMap) }, player.CityTagsOnMapProperty));
                    content.newParagraph();

                    for (CityTagBack back = CityTagBack.NONE; back < CityTagBack.NUM; back++)
                    {
                        var button = new ArtToggle(back == city.tagBack, new List<AbsRichBoxMember>
                        {
                    new RbImage(Data.CityTag.BackSprite(back), 0.8f)
                            }, new RbAction1Arg<CityTagBack>((CityTagBack back) => { city.tagBack = back; }, back, back == CityTagBack.NONE ? RbSoundType.Deselect : RbSoundType.Option));
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
                    }, new RbAction1Arg<CityTagArt>((CityTagArt art) => { city.tagArt = art; }, art, art == CityTagArt.None ? RbSoundType.Deselect : RbSoundType.Option));
                            content.Add(button);
                        }
                    }
                    break;

                case TagSubTab.HudPin:

                    //{
                    //    if (player.resourcesSubTab > ResourcesSubTab.Overview_Armor)
                    //    {
                    //        player.resourcesSubTab = ResourcesSubTab.Overview_Resources;
                    //    }

                    //    for (ResourcesSubTab resourcesSubTab = ResourcesSubTab.Overview_Resources; resourcesSubTab <= ResourcesSubTab.Overview_Armor; ++resourcesSubTab)
                    //    {
                    //        var tabContent = new RichBoxContent();
                            
                    //        switch (resourcesSubTab)
                    //        {
                    //            case ResourcesSubTab.Overview_Metals:
                    //                tabContent.Add(new RbImage(SpriteName.WarsResource_Iron));
                    //                break;
                    //            case ResourcesSubTab.Overview_Weapons:
                    //                tabContent.Add(new RbImage(SpriteName.WarsResource_Sword));
                    //                break;

                    //            case ResourcesSubTab.Overview_Projectile:
                    //                tabContent.Add(new RbImage(SpriteName.WarsResource_Bow));
                    //                break;

                    //            case ResourcesSubTab.Overview_Armor:
                    //                tabContent.Add(new RbImage(SpriteName.WarsResource_IronArmor));
                    //                break;

                    //            case ResourcesSubTab.Overview_Resources:
                    //                tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
                    //                break;


                    //        }
                    //        var subTab = new ArtButton(player.resourcesSubTab == resourcesSubTab ? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected,
                    //            tabContent,
                    //            new RbAction1Arg<ResourcesSubTab>((ResourcesSubTab resourcesSubTab) =>
                    //            {
                    //                player.resourcesSubTab = resourcesSubTab;
                    //            }, resourcesSubTab, RbSoundType.Tab), new RbTooltip_Text(DssRef.lang.Work_SelectCategory));
                            
                    //        content.Add(subTab);
                    //    }
                    //    switch (player.resourcesSubTab)
                    //    {
                    //        case ResourcesSubTab.Overview_Resources:
                    //            foreach (var item in City.MovableCityResource_Misc)
                    //            {
                    //                resourcePin(item);
                    //            }
                    //            break;
                    //        case ResourcesSubTab.Overview_Metals:
                    //            foreach (var item in City.MovableCityResource_Metals)
                    //            {
                    //                resourcePin(item);
                    //            }
                    //            break;
                    //        case ResourcesSubTab.Overview_Weapons:
                    //            foreach (var item in City.MovableCityResource_WeaponMelee)
                    //            {
                    //                resourcePin(item);
                    //            }
                    //            break;
                    //        case ResourcesSubTab.Overview_Projectile:
                    //            foreach (var item in City.MovableCityResource_WeaponRanged)
                    //            {
                    //                resourcePin(item);
                    //            }
                    //            break;
                    //        case ResourcesSubTab.Overview_Armor:
                    //            foreach (var item in City.MovableCityResource_Armor)
                    //            {
                    //                resourcePin(item);
                    //            }
                    //            break;

                    //    }

                    //    content.newParagraph();

                    //    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Editor_Canvas_Clear) },
                    //        new RbAction(()=> { player.hud.pins.clear(city); })));

                    //    void resourcePin(ItemResourceType item)
                    //    {
                    //        content.newLine();
                    //        content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                    //        new RbImage(ResourceLib.Icon(item)), new RbSpace(), new RbText(TextLib.LargeFirstLetter(LangLib.Item(item))) },
                    //            player.hud.pins.isPinnedProperty)
                    //        { propertyTag = new CityHudPinId(city.myIndex, new HudPin(item)) });
                    //    }
                    //}
                    break;

            }


        }

        void resourceTabToolTip(RichBoxContent content, object tag)
        {
            ResourcesSubTab tab = (ResourcesSubTab)tag;
            IconName.Tab(tab.managementType, out SpriteName categoryIcon, out string category); 
            IconName.Tab(tab.resourceGroup,  out SpriteName tabIcon, out string tabName);

            content.text(DssRef.lang.Work_SelectCategory, HudLib.TitleColor_Action);
            content.newParagraph();
            content.Add(new RbBeginTitle());
            content.Add(new RbImage(categoryIcon));
            content.space();
            content.Add(new RbText(category, HudLib.TitleColor_Head));

            content.icontext(tabIcon, tabName);
        }

        public void resourcesToMenu(RichBoxContent content)
        {
            if (player.tutorial == null || player.tutorial.DisplayResourseSubTabs())
            {
                for (ResourceManagementType managementType = 0; managementType < ResourceManagementType.Auto; managementType++)
                {
                    IconName.Tab(managementType, out SpriteName managementIcon, out string managementName);

                    content.newLine();
                    content.Add(new ArtButton(RbButtonStyle.HoverArea,
                        new List<AbsRichBoxMember> { new RbImage(managementIcon) },
                        null, new RbTooltip_Text(managementName)));

                    for (ResourceGroup group = 0; group < ResourceGroup.NUM; group++)
                    {
                        if (group == ResourceGroup.Mint)
                        {
                            bool includeMint = managementType == ResourceManagementType.Work && city.buildingStructure.CoinMinter_count > 0;
                            if (!includeMint)
                            {
                                continue;
                            }
                        }

                        IconName.Tab(group, out SpriteName groupIcon, out string groupName);
                        var tab = new ResourcesSubTab(managementType, group);
                        content.Add(new ArtOption(player.resourcesSubTab.EqualTab(tab),
                            new List<AbsRichBoxMember> { new RbImage(groupIcon) },
                            new RbAction1Arg<ResourcesSubTab>((ResourcesSubTab resourcesSubTab) =>
                            {
                                player.resourcesSubTab = resourcesSubTab;
                            }, tab, RbSoundType.Option),
                            new RbTooltip(resourceTabToolTip, tab)));
                        content.space();
                    }

                    //Info buttons
                    switch (managementType)
                    {
                        case ResourceManagementType.Overview:
                            HudLib.InfoButton(content,
                               new RbTooltip((RichBoxContent content, object tag) =>
                               {
                                   GroupedResource.BufferIconInfo(content, false);
                                   bool foodSafeGuard = city.foodSafeGuardIsActive(out bool fuelSafeGuard, out bool rawFoodSafeGuard, out bool woodSafeGuard);
                                   if (foodSafeGuard)
                                   {
                                       GroupedResource.BufferIconInfo(content, true);
                                   }
                                   //Minting.ConvertGoldOre.toMenu(content, city, false, true, false, false);
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

                        case ResourceManagementType.Work:
                            HudLib.InfoButton(content,
                               new RbTooltip((RichBoxContent content, object tag) =>
                               {
                                   HudLib.Description(content, string.Format(DssRef.lang.Work_OrderPrioDescription, WorkTemplate.MaxPrio));
                               }));
                            content.newLine();
                            break;

                        case ResourceManagementType.Stockpile:
                            HudLib.InfoButton(content,
                               new RbTooltip((RichBoxContent content, object tag) =>
                               {
                                   HudLib.Description(content, DssRef.lang.Resource_StockPile_Info);
                                   GroupedResource.BufferIconInfo(content, false);
                                   content.newLine();
                                   HudLib.BulletPoint(content);
                                   content.Add(new RbText(DssRef.lang.Work_BadValueDescription, HudLib.InfoYellow_Light));
                               }));

                            break;
                    }
                }
            }


                //OLD

            //    for (ResourcesSubTab resourcesSubTab = 0; resourcesSubTab < ResourcesSubTab.Auto; ++resourcesSubTab)
            //    {
            //        IconName.Tab(resourcesSubTab, out SpriteName categoryIcon, out string category, out SpriteName tabIcon, out string tabName);
            //        var tabContent = new RichBoxContent();
                   
            //        //string text = null;
            //        switch (resourcesSubTab)
            //        {
            //            //case ResourcesSubTab.Overview_Metals:
            //            //case ResourcesSubTab.Stockpile_Metals:
            //            //case ResourcesSubTab.Work_Metals:
            //            //    tabContent.Add(new RbImage(SpriteName.WarsResource_Iron));
            //            //    break;
            //            //case ResourcesSubTab.Overview_Weapons:
            //            //case ResourcesSubTab.Stockpile_Weapons:
            //            //case ResourcesSubTab.Work_Weapons:
            //            //    tabContent.Add(new RbImage(SpriteName.WarsResource_Sword));
            //            //    break;

            //            //case ResourcesSubTab.Overview_Projectile:
            //            //case ResourcesSubTab.Stockpile_Projectile:
            //            //case ResourcesSubTab.Work_Projectile:
            //            //    tabContent.Add(new RbImage(SpriteName.WarsResource_Bow));
            //            //    break;

            //            //case ResourcesSubTab.Overview_Armor:
            //            //case ResourcesSubTab.Stockpile_Armor:
            //            //case ResourcesSubTab.Work_Armor:
            //            //    tabContent.Add(new RbImage(SpriteName.WarsResource_IronArmor));
            //            //    break;

            //            case ResourcesSubTab.Work_Mint:
            //                //tabContent.Add(new RbImage(SpriteName.WarsResource_SilverCoin));
            //                if (city.buildingStructure.CoinMinter_count == 0)
            //                {
            //                    //continue;
            //                    goto skipTab;
            //                }
            //                break;

            //            case ResourcesSubTab.Overview_Resources:
            //                content.Add(new ArtButton(RbButtonStyle.HoverArea,
            //                    new List<AbsRichBoxMember> { new RbImage(SpriteName.MenuPixelIconManual) },
            //                    null, new RbTooltip_Text(DssRef.lang.Resource_Tab_Overview)));
                            
            //                //tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
            //                break;

            //            case ResourcesSubTab.Stockpile_Resources:
            //                content.Add(new ArtButton(RbButtonStyle.HoverArea,
            //                    new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsStockpileAdd) },
            //                    null, new RbTooltip_Text(DssRef.lang.Resource_Tab_Stockpile)));
                            
            //                //tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
            //                break;

            //            case ResourcesSubTab.Work_Resources:
            //                content.Add(new ArtButton(RbButtonStyle.HoverArea,
            //                    new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHammer) },
            //                    null, new RbTooltip_Text(DssRef.lang.MenuTab_Work)));
                           
            //                //tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
            //                break;
            //        }

            //        tabContent.Add(new RbImage(tabIcon));

            //        var subTab = new ArtButton(player.resourcesSubTab == resourcesSubTab ? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected,
            //            tabContent,
            //            new RbAction1Arg<ResourcesSubTab>((ResourcesSubTab resourcesSubTab) =>
            //            {
            //                player.resourcesSubTab = resourcesSubTab;
            //            }, resourcesSubTab, RbSoundType.Tab), new RbTooltip(resourceTabToolTip, resourcesSubTab)/*new RbTooltip_Text(DssRef.lang.Work_SelectCategory)*/);
            //        //subTab.setGroupSelectionColor(HudLib.RbSettings, player.resourcesSubTab == resourcesSubTab);
            //        content.Add(subTab);

            //    skipTab:
            //        switch (resourcesSubTab)
            //        {
            //            case ResourcesSubTab.Overview_Armor:
            //                HudLib.InfoButton(content,
            //                   new RbTooltip((RichBoxContent content, object tag) =>
            //                   {
            //                       GroupedResource.BufferIconInfo(content, false);
            //                       bool foodSafeGuard = city.foodSafeGuardIsActive(out bool fuelSafeGuard, out bool rawFoodSafeGuard, out bool woodSafeGuard);
            //                       if (foodSafeGuard)
            //                       {
            //                           GroupedResource.BufferIconInfo(content, true);
            //                       }
            //                       //Minting.ConvertGoldOre.toMenu(content, city, false, true, false, false);
            //                       {
            //                           content.newLine();
            //                           content.Add(new RbText(1.ToString()));
            //                           content.Add(new RbImage(ResourceLib.Icon(ItemResourceType.Food_G)));
            //                           content.Add(new RbText(DssRef.lang.Resource_TypeName_Food));
            //                           var arrow = new RbImage(SpriteName.pjNumArrowR);
            //                           arrow.color = Color.CornflowerBlue;
            //                           content.Add(arrow);
            //                           content.Add(new RbText(string.Format(DssRef.lang.Hud_EnergyAmount, DssRef.difficulty.FoodEnergySett)));
            //                       }
            //                       content.newLine();
            //                       HudLib.BulletPoint(content);
            //                       content.Add(new RbText(DssRef.lang.Work_BadValueDescription, HudLib.InfoYellow_Light));
            //                   }));
            //                content.newLine();
            //                break;

            //            case ResourcesSubTab.Work_Mint:
            //                HudLib.InfoButton(content,
            //                   new RbTooltip((RichBoxContent content, object tag) =>
            //                   {
            //                       HudLib.Description(content, string.Format(DssRef.lang.Work_OrderPrioDescription, WorkTemplate.MaxPrio));
            //                   }));
            //                content.newLine();
            //                break;

            //            case ResourcesSubTab.Stockpile_Armor:
            //                HudLib.InfoButton(content,
            //                   new RbTooltip((RichBoxContent content, object tag) =>
            //                   {
            //                       HudLib.Description(content, DssRef.lang.Resource_StockPile_Info);
            //                       GroupedResource.BufferIconInfo(content, false);
            //                       content.newLine();
            //                       HudLib.BulletPoint(content);
            //                       content.Add(new RbText(DssRef.lang.Work_BadValueDescription, HudLib.InfoYellow_Light));
            //                   }));

            //                break;
            //        }
                    
            //    }
            //    content.newParagraph();
            //}

            //bool reachedBuffer = false;

            switch (player.resourcesSubTab.managementType)
            {
                case ResourceManagementType.Overview:
                    resourceOverview(content, player.resourcesSubTab.resourceGroup); 
                    break;                

                case ResourceManagementType.Work:
                    content.h2(DssRef.lang.Work_OrderPrioTitle, HudLib.TitleColor_Head);
                    city.workTemplate.toHud(player, content, player.resourcesSubTab, city.GetFaction(), city);
                    break;

                case ResourceManagementType.Stockpile:
                    content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);
                    new StockPileMenu(content, city, null).toHud(player.resourcesSubTab);
                    break;
            }

            //void stockpile(ItemResourceType item)
            //{   
            //    GroupedResource res = city.GetGroupedResource(item);

            //    content.newLine();

            //    content.Add(new ArtButton(RbButtonStyle.HoverArea, 
            //        new List<AbsRichBoxMember>{
            //            new RbImage(res.amount >= res.goalBuffer ? SpriteName.WarsStockpileStop : SpriteName.WarsStockpileAdd),
            //            new RbImage(ResourceLib.Icon(item))},null,
            //            //new RbTooltip((RichBoxContent content, object tag) =>
            //            //{
            //            //    bool buffer = false;
            //            //    city.GetGroupedResource(item).toMenu(content, item, false, ref buffer);                           
            //            //}
            //            new RbTooltip(ResourceLib.FullResourceInfo, new ResourceInfoTag(city, item))
            //            ));
                
            //    content.space();
               
            //    //stockPileEdit(content, item, res);
            //}
        }

        void resourceOverview(RichBoxContent content, ResourceGroup resourceGroup)
        {
            bool reachedBuffer = false;

            switch (resourceGroup)
            {
                case ResourceGroup.Resources:

                    city.waterToHud(content, true);

                    bool foodSafeGuard = city.foodSafeGuardIsActive(out bool fuelSafeGuard, out bool rawFoodSafeGuard, out bool woodSafeGuard);

                    city.GetGroupedResource(CityResoureIndex.wood).toMenu(content, ItemResourceType.Wood_Group, woodSafeGuard, ref reachedBuffer, player, city);//New solution
                    city.GetGroupedResource(CityResoureIndex.stone).toMenu(content, ItemResourceType.Stone_G, false, ref reachedBuffer, player, city);// Replace "res_stone", and continue with the rest
                    city.GetGroupedResource(CityResoureIndex.Clay).toMenu(content, ItemResourceType.Clay, false, ref reachedBuffer, player, city);// Replace "res_stone", and continue with the rest
                    city.GetGroupedResource(CityResoureIndex.Brick).toMenu(content, ItemResourceType.Brick, false, ref reachedBuffer, player, city);// Replace "res_stone", and continue with the rest
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.ClayBrick);

                    city.GetGroupedResource(CityResoureIndex.rawFood).toMenu(content, ItemResourceType.RawFood_Group, rawFoodSafeGuard, ref reachedBuffer, player, city);
                    city.GetGroupedResource(CityResoureIndex.Salt).toMenu(content, ItemResourceType.Salt, false, ref reachedBuffer, player, city);
                    city.GetGroupedResource(CityResoureIndex.skinLinnen).toMenu(content, ItemResourceType.SkinLinen_Group, false, ref reachedBuffer, player, city);
                    content.newParagraph();

                    city.GetGroupedResource(CityResoureIndex.food).toMenu(content, ItemResourceType.Food_G, foodSafeGuard, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Food1, CraftResourceLib.Food2);
                    content.space();

                    content.Add(new ArtToggle(city.res_food_safeguard, new List<AbsRichBoxMember> {
                            new RbImage(city.res_food_safeguard? SpriteName.WarsProtectedStockpileOn : SpriteName.WarsProtectedStockpileOff, 0.7f),
                        },
                    new RbAction(() =>
                    {
                        city.res_food_safeguard = !city.res_food_safeguard;
                    }),
                    new RbTooltip((RichBoxContent content, object tag) =>
                    {
                        content.text(string.Format(DssRef.lang.Resource_FoodSafeGuard_Description, DssConst.WorkSafeGuardAmount)).overrideColor = HudLib.InfoYellow_Light;
                        content.text(city.res_food_safeguard ? DssRef.lang.Hud_On : DssRef.lang.Hud_Off);
                    })));

                    city.GetGroupedResource(CityResoureIndex.ConservedFood).toMenu(content, ItemResourceType.ConservedFood, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.ConservedFood1, CraftResourceLib.ConservedFood2);

                    city.GetGroupedResource(CityResoureIndex.beer).toMenu(content, ItemResourceType.Beer, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Beer);

                    city.GetGroupedResource(CityResoureIndex.coolingfluid).toMenu(content, ItemResourceType.CoolingFluid, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.CoolingFluid);
                    content.newParagraph();

                    city.GetGroupedResource(CityResoureIndex.fuel).toMenu(content, ItemResourceType.Fuel_G, fuelSafeGuard, ref reachedBuffer, player, city);
                    int totalmines = 0;
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_coal, ItemResourceType.Coal, Map.SubTile.Empty, ref totalmines);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Fuel1, null, true);
                    content.space();
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Charcoal);


                    city.GetGroupedResource(CityResoureIndex.StorageBox).toMenu(content, ItemResourceType.StorageBox, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.StorageBox_wood, CraftResourceLib.StorageBox_clay);

                    city.GetGroupedResource(CityResoureIndex.Palisade).toMenu(content, ItemResourceType.Palisade, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Palisade);

                    city.GetGroupedResource(CityResoureIndex.Toolkit).toMenu(content, ItemResourceType.Toolkit, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Toolkit);

                    city.GetGroupedResource(CityResoureIndex.Wagon2Wheel).toMenu(content, ItemResourceType.Wagon2Wheel, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Wagon2Wheel);

                    city.GetGroupedResource(CityResoureIndex.Wagon4Wheel).toMenu(content, ItemResourceType.Wagon4Wheel, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Wagon4Wheel);

                    city.GetGroupedResource(CityResoureIndex.WagonClosed).toMenu(content, ItemResourceType.WagonClosed, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.WagonClosed);

                    city.GetGroupedResource(CityResoureIndex.WagonIron).toMenu(content, ItemResourceType.WagonIron, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.WagonIron);

                    city.GetGroupedResource(CityResoureIndex.WagonSteel).toMenu(content, ItemResourceType.WagonSteel, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.WagonSteel);

                    city.GetGroupedResource(CityResoureIndex.BlackPowder).toMenu(content, ItemResourceType.BlackPowder, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.BlackPowder);

                    city.GetGroupedResource(CityResoureIndex.GunPowder).toMenu(content, ItemResourceType.GunPowder, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.GunPowder);

                    city.GetGroupedResource(CityResoureIndex.LedBullet).toMenu(content, ItemResourceType.LedBullet, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.LedBullets);

                    godPowerSetAllResources(content, City.MovableCityResource_Misc);

                    break;

                case ResourceGroup.Metals:

                    int totalMines = 0;

                    city.GetGroupedResource(CityResoureIndex.ironore).toMenu(content, ItemResourceType.IronOre_G, false, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_bogIron, ItemResourceType.BogIron, Map.SubTile.Empty, ref totalMines);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_iron, ItemResourceType.Iron_G, Map.SubTile.Empty, ref totalMines);

                    city.GetGroupedResource(CityResoureIndex.TinOre).toMenu(content, ItemResourceType.TinOre, false, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_tin, ItemResourceType.Tin, Map.SubTile.Empty, ref totalMines);

                    city.GetGroupedResource(CityResoureIndex.CopperOre).toMenu(content, ItemResourceType.CopperOre, false, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_copper, ItemResourceType.Copper, Map.SubTile.Empty, ref totalMines);

                    city.GetGroupedResource(CityResoureIndex.LeadOre).toMenu(content, ItemResourceType.LeadOre, false, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_lead, ItemResourceType.Lead, Map.SubTile.Empty, ref totalMines);

                    city.GetGroupedResource(CityResoureIndex.SilverOre).toMenu(content, ItemResourceType.SilverOre, false, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_silver, ItemResourceType.Silver, Map.SubTile.Empty, ref totalMines);

                    city.GetGroupedResource(CityResoureIndex.GoldOre).toMenu(content, ItemResourceType.GoldOre, false, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_gold, ItemResourceType.Gold, Map.SubTile.Empty, ref totalMines);
                    HudLib.blueprintButton(city, player, content, Minting.ConvertGoldOre);

                    content.newParagraph();


                    city.GetGroupedResource(CityResoureIndex.iron).toMenu(content, ItemResourceType.Iron_G, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Iron, CraftResourceLib.Iron_AndCooling);

                    city.GetGroupedResource(CityResoureIndex.Tin).toMenu(content, ItemResourceType.Tin, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Tin);

                    city.GetGroupedResource(CityResoureIndex.Copper).toMenu(content, ItemResourceType.Copper, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Copper, CraftResourceLib.Cupper_AndCooling);

                    city.GetGroupedResource(CityResoureIndex.Lead).toMenu(content, ItemResourceType.Lead, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Lead);

                    city.GetGroupedResource(CityResoureIndex.Silver).toMenu(content, ItemResourceType.Silver, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Silver, CraftResourceLib.Silver_AndCooling);

                    city.GetGroupedResource(CityResoureIndex.RawMithril).toMenu(content, ItemResourceType.RawMithril, false, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_mithril, ItemResourceType.Mithril, Map.SubTile.Empty, ref totalMines);

                    city.GetGroupedResource(CityResoureIndex.Sulfur).toMenu(content, ItemResourceType.Sulfur, false, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_sulfur, ItemResourceType.Sulfur, Map.SubTile.Empty, ref totalMines);
                    content.newParagraph();


                    city.GetGroupedResource(CityResoureIndex.Bronze).toMenu(content, ItemResourceType.Bronze, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Bronze);

                    city.GetGroupedResource(CityResoureIndex.CastIron).toMenu(content, ItemResourceType.CastIron, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.CastIron);

                    city.GetGroupedResource(CityResoureIndex.BloomeryIron).toMenu(content, ItemResourceType.BloomeryIron, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.BloomeryIron);

                    city.GetGroupedResource(CityResoureIndex.Steel).toMenu(content, ItemResourceType.Steel, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Steel, CraftResourceLib.Steel_AndCooling);

                    city.GetGroupedResource(CityResoureIndex.Mithril).toMenu(content, ItemResourceType.Mithril, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Mithril);


                    godPowerSetAllResources(content, City.MovableCityResource_Metals);
                    break;

                case ResourceGroup.Weapons:

                    city.GetGroupedResource(CityResoureIndex.sharpstick).toMenu(content, ItemResourceType.SharpStick, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.SharpStick);

                    city.GetGroupedResource(CityResoureIndex.BronzeSword).toMenu(content, ItemResourceType.BronzeSword, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.BronzeSword);

                    city.GetGroupedResource(CityResoureIndex.shortsword).toMenu(content, ItemResourceType.ShortSword, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.ShortSword);

                    city.GetGroupedResource(CityResoureIndex.Sword).toMenu(content, ItemResourceType.Sword, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Sword);

                    city.GetGroupedResource(CityResoureIndex.LongSword).toMenu(content, ItemResourceType.LongSword, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.LongSword);

                    city.GetGroupedResource(CityResoureIndex.HandSpear).toMenu(content, ItemResourceType.HandSpear, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.HandSpearIron, CraftResourceLib.HandSpearBronze);

                    content.newParagraph();

                    city.GetGroupedResource(CityResoureIndex.Warhammer).toMenu(content, ItemResourceType.Warhammer, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.WarhammerIron, CraftResourceLib.WarhammerBronze);

                    city.GetGroupedResource(CityResoureIndex.twohandsword).toMenu(content, ItemResourceType.TwoHandSword, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.TwoHandSword);

                    city.GetGroupedResource(CityResoureIndex.MithrilSword).toMenu(content, ItemResourceType.MithrilSword, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MithrilSword);

                    godPowerSetAllResources(content, City.MovableCityResource_WeaponMelee);

                    break;

                case ResourceGroup.Projectile:

                    city.GetGroupedResource(CityResoureIndex.SlingShot).toMenu(content, ItemResourceType.SlingShot, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Slingshot);

                    city.GetGroupedResource(CityResoureIndex.ThrowingSpear).toMenu(content, ItemResourceType.ThrowingSpear, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.ThrowingSpear1, CraftResourceLib.ThrowingSpear2);

                    city.GetGroupedResource(CityResoureIndex.bow).toMenu(content, ItemResourceType.Bow, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Bow);

                    city.GetGroupedResource(CityResoureIndex.longbow).toMenu(content, ItemResourceType.LongBow, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.LongBow);

                    city.GetGroupedResource(CityResoureIndex.crossbow).toMenu(content, ItemResourceType.Crossbow, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.CrossBow);

                    city.GetGroupedResource(CityResoureIndex.MithrilBow).toMenu(content, ItemResourceType.MithrilBow, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MithrilBow);


                    city.GetGroupedResource(CityResoureIndex.HandCannon).toMenu(content, ItemResourceType.HandCannon, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.BronzeHandCannon);

                    city.GetGroupedResource(CityResoureIndex.HandCulvertin).toMenu(content, ItemResourceType.HandCulverin, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.BronzeHandCulverin);

                    city.GetGroupedResource(CityResoureIndex.Rifle).toMenu(content, ItemResourceType.Rifle, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Rifle);

                    city.GetGroupedResource(CityResoureIndex.Blunderbuss).toMenu(content, ItemResourceType.Blunderbuss, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Blunderbus);
                    content.newParagraph();

                    city.GetGroupedResource(CityResoureIndex.ballista).toMenu(content, ItemResourceType.Ballista, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Ballista_Iron, CraftResourceLib.Ballista_Bronze);

                    city.GetGroupedResource(CityResoureIndex.Manuballista).toMenu(content, ItemResourceType.Manuballista, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.ManuBallista);

                    city.GetGroupedResource(CityResoureIndex.Catapult).toMenu(content, ItemResourceType.Catapult, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Catapult);

                    city.GetGroupedResource(CityResoureIndex.SiegeCannonBronze).toMenu(content, ItemResourceType.SiegeCannonBronze, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.SiegeCannonBronze);

                    city.GetGroupedResource(CityResoureIndex.ManCannonBronze).toMenu(content, ItemResourceType.ManCannonBronze, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.ManCannonBronze);

                    city.GetGroupedResource(CityResoureIndex.SiegeCannonIron).toMenu(content, ItemResourceType.SiegeCannonIron, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.SiegeCannonIron);

                    city.GetGroupedResource(CityResoureIndex.ManCannonIron).toMenu(content, ItemResourceType.ManCannonIron, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.ManCannonIron);


                    godPowerSetAllResources(content, City.MovableCityResource_WeaponRanged);

                    break;

                case ResourceGroup.Armor:

                    city.GetGroupedResource(CityResoureIndex.paddedArmor).toMenu(content, ItemResourceType.PaddedArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.PaddedArmor);

                    city.GetGroupedResource(CityResoureIndex.HeavyPaddedArmor).toMenu(content, ItemResourceType.HeavyPaddedArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.HeavyPaddedArmor);

                    city.GetGroupedResource(CityResoureIndex.BronzeArmor).toMenu(content, ItemResourceType.BronzeArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.BronzeArmor);

                    city.GetGroupedResource(CityResoureIndex.mailArmor).toMenu(content, ItemResourceType.IronArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MailArmor);

                    city.GetGroupedResource(CityResoureIndex.heavyMailArmor).toMenu(content, ItemResourceType.HeavyIronArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.HeavyMailArmor);

                    city.GetGroupedResource(CityResoureIndex.LightPlateArmor).toMenu(content, ItemResourceType.LightPlateArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.PlateArmor);

                    city.GetGroupedResource(CityResoureIndex.FullPlateArmor).toMenu(content, ItemResourceType.FullPlateArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.FullPlateArmor);

                    city.GetGroupedResource(CityResoureIndex.MithrilArmor).toMenu(content, ItemResourceType.MithrilArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MithrilArmor);

                    content.newParagraph();

                    // Mount Padded Armor
                    city.GetGroupedResource(CityResoureIndex.MountPaddedArmor).toMenu(content, ItemResourceType.MountPaddedArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountPaddedArmor);

                    // Mount Heavy Padded Armor
                    city.GetGroupedResource(CityResoureIndex.MountHeavyPaddedArmor).toMenu(content, ItemResourceType.MountHeavyPaddedArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountHeavyPaddedArmor);

                    // Mount Bronze Armor
                    city.GetGroupedResource(CityResoureIndex.MountBronzeArmor).toMenu(content, ItemResourceType.MountBronzeArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountBronzeArmor);

                    // Mount Iron Armor (mapped from Mail)
                    city.GetGroupedResource(CityResoureIndex.MountIronArmor).toMenu(content, ItemResourceType.MountIronArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountIronArmor);

                    // Mount Heavy Iron Armor (mapped from Heavy Mail)
                    city.GetGroupedResource(CityResoureIndex.MountHeavyIronArmor).toMenu(content, ItemResourceType.MountHeavyIronArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountHeavyIronArmor);

                    // Mount Light Plate Armor
                    city.GetGroupedResource(CityResoureIndex.MountLightPlateArmor).toMenu(content, ItemResourceType.MountLightPlateArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountLightPlateArmor);

                    // Mount Full Plate Armor
                    city.GetGroupedResource(CityResoureIndex.MountFullPlateArmor).toMenu(content, ItemResourceType.MountFullPlateArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountFullPlateArmor);

                    // Mount Mithril Armor
                    city.GetGroupedResource(CityResoureIndex.MountMithrilArmor).toMenu(content, ItemResourceType.MountMithrilArmor, false, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountMithrilArmor);

                    godPowerSetAllResources(content, City.MovableCityResource_Armor);
                    break;

                case ResourceGroup.Animals:

                    // --- Farm ---
                    city.GetGroupedResource(CityResoureIndex.Hen).toMenu(content, ItemResourceType.Hen, false, ref reachedBuffer, player, city);
                    city.GetGroupedResource(CityResoureIndex.Pig).toMenu(content, ItemResourceType.Pig, false, ref reachedBuffer, player, city, true);

                    // --- Dogs ---
                    city.GetGroupedResource(CityResoureIndex.Dog).toMenu(content, ItemResourceType.Dog, false, ref reachedBuffer, player, city);
                    city.GetGroupedResource(CityResoureIndex.Hound).toMenu(content, ItemResourceType.Hound, false, ref reachedBuffer, player, city, true);

                    // --- Oxen ---
                    city.GetGroupedResource(CityResoureIndex.Oxen).toMenu(content, ItemResourceType.Oxen, false, ref reachedBuffer, player, city);
                    city.GetGroupedResource(CityResoureIndex.KineOxen).toMenu(content, ItemResourceType.KineOxen, false, ref reachedBuffer, player, city, true);

                    // --- Horses ---
                    city.GetGroupedResource(CityResoureIndex.Pony).toMenu(content, ItemResourceType.Pony, false, ref reachedBuffer, player, city);
                    city.GetGroupedResource(CityResoureIndex.Horse).toMenu(content, ItemResourceType.Horse, false, ref reachedBuffer, player, city);
                    city.GetGroupedResource(CityResoureIndex.WarHorse).toMenu(content, ItemResourceType.WarHorse, false, ref reachedBuffer, player, city, true);
                    city.GetGroupedResource(CityResoureIndex.DraftHorse).toMenu(content, ItemResourceType.DraftHorse, false, ref reachedBuffer, player, city, true);

                    // --- Wild Pigs ---
                    city.GetGroupedResource(CityResoureIndex.WildPig).toMenu(content, ItemResourceType.WildPig, false, ref reachedBuffer, player, city, true);
                    city.GetGroupedResource(CityResoureIndex.WildHog).toMenu(content, ItemResourceType.WildHog, false, ref reachedBuffer, player, city, true);
                    city.GetGroupedResource(CityResoureIndex.WarHog).toMenu(content, ItemResourceType.WarHog, false, ref reachedBuffer, player, city, true);
                    city.GetGroupedResource(CityResoureIndex.StagHog).toMenu(content, ItemResourceType.StagHog, false, ref reachedBuffer, player, city, true);

                    // --- Wolves ---
                    city.GetGroupedResource(CityResoureIndex.Wolf).toMenu(content, ItemResourceType.Wolf, false, ref reachedBuffer, player, city, true);
                    city.GetGroupedResource(CityResoureIndex.Warg).toMenu(content, ItemResourceType.Warg, false, ref reachedBuffer, player, city, true);
                    city.GetGroupedResource(CityResoureIndex.AlphaWarg).toMenu(content, ItemResourceType.AlphaWarg, false, ref reachedBuffer, player, city, true);

                    // --- Cats ---
                    city.GetGroupedResource(CityResoureIndex.WildCat).toMenu(content, ItemResourceType.WildCat, false, ref reachedBuffer, player, city, true);
                    city.GetGroupedResource(CityResoureIndex.Lion).toMenu(content, ItemResourceType.Lion, false, ref reachedBuffer, player, city, true);
                    city.GetGroupedResource(CityResoureIndex.WarLion).toMenu(content, ItemResourceType.WarLion, false, ref reachedBuffer, player, city, true);

                    // --- Elephants ---
                    city.GetGroupedResource(CityResoureIndex.Elephant).toMenu(content, ItemResourceType.Elephant, false, ref reachedBuffer, player, city, true);
                    city.GetGroupedResource(CityResoureIndex.WarElephant).toMenu(content, ItemResourceType.WarElephant, false, ref reachedBuffer, player, city, true);
                    city.GetGroupedResource(CityResoureIndex.Oliphant).toMenu(content, ItemResourceType.Oliphant, false, ref reachedBuffer, player, city, true);
                    break;
            }
        }

        private void godPowerSetAllResources(RichBoxContent content, ItemResourceType[] Resources)
        {
            if (DssRef.difficulty.GodPowers())
            {
                content.newParagraph();
                //ItemResourceType[] Resources = City.MovableCityResource_Misc;

                HudLib.Label(content, DssRef.lang.GeneralSetting_SetAll);
                content.space();
                content.Add(new ArtButton(RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbText("= 0", HudLib.GodPower_Color) },
                   new RbAction(() =>
                   {
                       foreach (var res in Resources)
                       {
                           city.AddGroupedResource(res, -city.GetGroupedResource(res).amount);
                       }
                   }),
                   null, true));

                content.Add(new ArtButton(RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbText("+100", HudLib.GodPower_Color) },
                   new RbAction(() =>
                   {
                       foreach (var res in Resources)
                       {
                           city.AddGroupedResource(res, 100);
                       }
                   }),
                null, true));
            }
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
        //                new RbAction1Arg<bool>(buyRepairAction, true, RbSoundType.Buy),
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
        //        //        new RbAction(city.burnItDown, RbSoundType.Default),
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
        //                new RbAction1Arg<int>(buyCityGuardsAction, count, RbSoundType.Buy),
        //                new RbTooltip(buyGuardSizeToolTip, count),
        //                city.buyCityGuards(false, count)));
        //        }
        //        //content.Add(new RichBoxSpace());
        //        {
        //            int count = 5;
        //            content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { 
        //                    new RbText(string.Format(DssRef.lang.Hud_XTimes, count)) 
        //                },
        //                new RbAction1Arg<int>(buyCityGuardsAction, count, RbSoundType.Buy),
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
        //                new RbAction1Arg<int>(city.releaseGuardSize, count * DssConst.ExpandGuardSize, RbSoundType.Buy),
        //                new RbTooltip(releaseGuardSizeToolTip, count),
        //                city.canReleaseGuardSize(count)));
        //        }
        //        //if (!city.nobelHouse && city.canEverGetNobelHouse())
        //        //{
        //        //    content.Button(DssRef.lang.Building_NobleHouse,
        //        //            new RbAction(city.buyNobelHouseAction, RbSoundType.Buy),
        //        //            new RbAction(buyNobelhouseTooltip),
        //        //            city.canBuyNobelHouse());
        //        //}
        //    }
        //}

        

       

        

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
        //                new RbAction3Arg<UnitType, int, LocalPlayer>(city.buySoldiersAction, opt.unitType, 1, player, RbSoundType.Buy),
        //                new RbAction2Arg<CityPurchaseOption, int>(buySoldiersTip, opt, 1),
        //                canBuySoldiers(opt.unitType, 1)));

        //            content.space();
        //            multiBuy(5);

        //            content.space();

        //            multiBuy(25);

        //            void multiBuy(int multiCount)
        //            {
        //                content.Button(string.Format(DssRef.lang.Hud_XTimes, multiCount),
        //                    new RbAction3Arg<UnitType, int, LocalPlayer>(city.buySoldiersAction, opt.unitType, multiCount, player, RbSoundType.Buy),
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
        //            new RbAction1Arg<int>(buyMercenaryAction, 1, RbSoundType.Buy),
        //            new RbAction1Arg<int>(buyMercenaryToolTip, 1),
        //            city.buyMercenary(false, 1)));

        //        content.Add(new RichBoxSpace());

        //        content.Button((DssLib.MercenaryPurchaseCount * 5).ToString(),
        //            new RbAction1Arg<int>(buyMercenaryAction, 5, RbSoundType.Buy),
        //            new RbAction1Arg<int>(buyMercenaryToolTip, 5),
        //            city.buyMercenary(false, 5));

        //        content.Add(new RichBoxNewLine(true));


        //        if (city.damages.HasValue())
        //        {
        //            content.Add(new RichboxButton(new List<AbsRichBoxMember>{
        //                            new RichBoxImage(SpriteName.unitEmoteLove),
        //                            new RichBoxText(DssRef.lang.CityOption_Repair),
        //                        },
        //                new RbAction1Arg<bool>(buyRepairAction, true, RbSoundType.Buy),
        //                new RbAction1Arg<bool>(buyRepairToolTip, true),
        //                city.buyRepair(false, true)));
        //        }
        //        //else
        //        //{
        //        //    content.Add(new RichboxButton(new List<AbsRichBoxMember>{
        //        //            new RichBoxImage(SpriteName.WarsWorkerAdd),
        //        //            new RichBoxText(DssRef.lang.CityOption_ExpandWorkForce),
        //        //        },
        //        //        new RbAction1Arg<int>(buyWorkforceAction, 1, RbSoundType.Buy),
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
        //        //        new RbAction(city.burnItDown, RbSoundType.Default),
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
        //                new RbAction1Arg<int>(buyCityGuardsAction, count, RbSoundType.Buy),
        //                new RbAction1Arg<int>(buyGuardSizeToolTip, count),
        //                city.buyCityGuards(false, count)));
        //        }
        //        content.Add(new RichBoxSpace());
        //        {
        //            int count = 5;
        //            content.Button(string.Format(DssRef.lang.Hud_XTimes, count),
        //            new RbAction1Arg<int>(buyCityGuardsAction, count, RbSoundType.Buy),
        //            new RbAction1Arg<int>(buyGuardSizeToolTip, count),
        //            city.buyCityGuards(false, count));
        //        }

        //        content.newLine();

        //        if (!city.nobelHouse && city.canEverGetNobelHouse())
        //        {
        //            content.Button(DssRef.lang.Building_NobleHouse,
        //                    new RbAction(city.buyNobelHouseAction, RbSoundType.Buy),
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
        StockPile,
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

        God_Recruit,

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

    //enum ResourcesSubTab
    //{ 
    //    Overview_Resources,
    //    Overview_Metals,
    //    Overview_Weapons,
    //    Overview_Projectile,
    //    Overview_Armor,

    //    Work_Resources,
    //    Work_Metals,
    //    Work_Weapons,
    //    Work_Projectile,
    //    Work_Armor,
    //    Work_Mint,

    //    Stockpile_Resources,
    //    Stockpile_Metals,
    //    Stockpile_Weapons,
    //    Stockpile_Projectile,
    //    Stockpile_Armor,

    //    Auto,

    //}

    struct ResourcesSubTab
    {
        public ResourceGroup resourceGroup;
        public ResourceManagementType managementType;

        public ResourcesSubTab(ResourceManagementType managementType, ResourceGroup resourceGroup)
        { 
            this.managementType = managementType;
            this.resourceGroup = resourceGroup;
        }

        public bool EqualTab(ResourcesSubTab other)
        { 
            return resourceGroup == other.resourceGroup && managementType == other.managementType;
        }
    }

    enum ResourceGroup
    {
        Resources,
        Metals,
        Weapons,
        Projectile,
        Armor,
        Animals,
        Mint,
        NUM
    }

    enum ResourceManagementType
    { 
        Overview,
        Work,
        Stockpile,
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

    enum TagSubTab
    { 
        Tag,
        HudPin,
        NUM
    }
}
