using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.GameState.BattleLab;
using VikingEngine.DSSWars.Interface.HudPinUi;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.PlayerControls.Casual;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Stockpile;
using VikingEngine.DSSWars.Work;
using VikingEngine.DSSWars.XP;
using VikingEngine.Graphics;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.ToGG;


namespace VikingEngine.DSSWars.Interface.MapObjMenu
{
    partial class MapObjMenu //CITY MENU
    {
        public static List<MenuTab> CityTabs;
        public static List<MenuTab> CasualTabs = new List<MenuTab> { MenuTab.Info, MenuTab.Casual_Recruit, MenuTab.Casual_Build, MenuTab.Tag };
        //protected LocalPlayer player;
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
            CityTabs = new List<MenuTab>() {
                MenuTab.Info, MenuTab.Resources, MenuTab.BlackMarket,
                MenuTab.Build, MenuTab.Delivery, MenuTab.Conscript, MenuTab.Reassign, 
                MenuTab.Defence, 
                MenuTab.Progress,
                MenuTab.Tag};

            if (DssRef.difficulty.setting_gameMode == GameModeMainType.Spectator)
            {
                CityTabs.Insert(1, MenuTab.God_Recruit);
            }
            //else
            //{
            //    CityTabs.Add(MenuTab.Help);
            //}
        }

        public MapObjMenu(LocalPlayer player, City city, RichBoxContent content, out RichBoxContent secondMenuContent)
        {
            this.player = player;
            this.city = city;
            mapObj = city;
            secondMenuContent = null;

            if (!DssRef.storage.ruleset_instance.centralGold)
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
                if (viewControllerTabs && player.gameControls.input.Controller_TabLeft.IsActive)
                {
                    content.Add(new RbImage(player.gameControls.input.Controller_TabLeft.Icon) { color = focusColor });
                    content.space(0.5f);
                }
               
                var tabGroup = new ArtTabgroup(tabs, tabSel, player.cityTabClick);
                if (viewControllerTabs && player.gameControls.input.Controller_TabRight.IsActive)
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

                    case MenuTab.Reassign:
                        ResassignTab(content, out secondMenuContent);
                        break;

                    case MenuTab.Delivery:
                        deliveryTab(content);
                        break;
                                           
                    //case MenuTab.Trade:
                    //    tradeTab(content);
                    //    break;

                    case MenuTab.Build:
                        player.gameControls.build.toHud(player, content, city);
                        break;

                    case MenuTab.Progress:
                        progressTab(content);
                        break;

                    case MenuTab.CessPit:
                        city.cesspitToHud(player, content);
                        break;

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
                if (city != null && city.pfaction.TryGetFaction(out _))
                {
                    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                    {
                        conscript = BattleLabStorage.Singleton.setup.conscript,
                      
                    };

                    var army = city.recruitToClosestArmy();

                    if (army == null)
                    {
                        army = city.pfaction.GetFaction().NewArmy(city.recruitToTile);
                    }

                    for (int i = 0; i < count; ++i)
                    {
                        new SoldierGroup(army, SoldierProfile, army.position);
                    }

                    army.setAsStartArmy();
                }
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
                content.newParagraph();
                buySoldierOption(city.casualCityProfile.settler, CasualSoldierType.Settler);

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
                        new RbText(option.FullPrice.ToString(), player.pfaction.GetFaction().hasGold(option.FullPrice, city)? HudLib.AvailableColor_Dark : HudLib.NotAvailableColor_Dark),
                    }, new RbAction1Arg<CasualRecruitQueueItem>(casualRecruitGroup, recruitOption), new RbTooltip(casualRecruitTooltip, recruitOption)));

                    content.Add(new RbTab(0.4f));
                    foreach (var counts in RecruitTabCounts)
                    {
                        recruitOption.count = counts;

                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {                       
                            new RbText(string.Format(DssRef.lang.Hud_XTimes, counts), player.pfaction.GetFaction().hasGold(option.FullPrice * counts, city)? HudLib.AvailableColor_Dark : HudLib.NotAvailableColor_Dark),
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
                var data = SoldierProfile.createSoldierData();


                content.h1(string.Format(DssRef.lang.Language_XCountIsY, DssRef.lang.UnitType_SoldierGroup, recruitOption.count), HudLib.TitleColor_Head);
                content.h2(DssRef.lang.Hud_PurchaseTitle_Cost, HudLib.TitleColor_Label);

                content.newLine();
                HudLib.BulletPoint(content);
                HudLib.ResourceCost(content, ResourceType.Gold, recruitOption.purchaseOption.FullPrice * recruitOption.count, (int)player.pfaction.GetFaction().GetGold(city));

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

            content.h2(TextLib.LargeFirstLetter( DssRef.lang.Resource_TypeName_Food), HudLib.TitleColor_Head);

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
            content.Add(new RbImage(SpriteName.WarsBuild_TreeApple));
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
                    new RbAction1Arg<ProgressSubTab>((resourcesSubTab) =>
                    {
                        player.progressSubTab = resourcesSubTab;
                    }, workSubTab, RbSoundType.Tab), new RbTooltip_Text(description));
                
                content.Add(subTab);
                //content.space();
            }
            content.newParagraph();

            switch (player.progressSubTab)
            {
                default:
                    new TechnologyHud(player, city).technologyHud(content, city.pfaction.GetFaction());
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
                    new RbAction1Arg<ExperienceOrDistancePrio>((val) =>
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

                if (StartupSettings.UnlockAllProgress)
                {
                    content.Add(new ArtButton(RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbText("+1") }, 
                        new RbAction1Arg<WorkExperienceType>(city.debug_addOneMaster, experienceType)));
                }

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
            if (player.profile.casualControls)
            {
                player.tagSubTab = TagSubTab.Tag;
                HudLib.Label(content, DssRef.lang.ObjectUi_ViewOnMap + string.Format(" ({0})", DssRef.lang.Hud_AllCities));
                content.newLine();
                player.cityHudSettings.toHud(content, true, true);
                content.newParagraph();
            }
            else
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

                        case TagSubTab.TagSettings:
                            tabContent.Add(new RbImage(SpriteName.WarsHudIconSettings, 0.7f));
                            tabContent.space(0.6f);
                            tabContent.Add(new RbText(Ref.langOpt.Options_title));
                            break;
                    }

                    var subTab = new ArtButton(player.tagSubTab == subTabType ? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected, tabContent,
                        new RbAction1Arg<TagSubTab>((subTabType) =>
                        {
                            player.tagSubTab = subTabType;
                        }, subTabType, RbSoundType.Tab), description == null ? null : new RbTooltip_Text(description));
                    content.Add(subTab);
                }
                content.newParagraph();
            }

            switch (player.tagSubTab)
            {
                case TagSubTab.TagSettings:

                    HudLib.Label(content, DssRef.lang.ObjectUi_ViewOnMap + string.Format(" ({0})", DssRef.lang.Hud_AllCities));
                    content.newLine();
                    player.cityHudSettings.toHud(content, true, false);
                    break;

                default:
                    TagLib.TagsToMenu(content, player, city);
                    
                    break;

                case TagSubTab.HudPin:
                    for (ResourceGroupType managementType = 0; managementType < ResourceGroupType.Mint; managementType++)
                    {
                        IconName.Tab(managementType, out SpriteName managementIcon, out string managementName);

                        var subTab = new ArtButton(player.resourcesSubTab.resourceGroup == managementType ? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected,
                            new List<AbsRichBoxMember> { new RbImage(managementIcon) },
                            new RbAction1Arg<ResourceGroupType>((tab) =>
                            {
                                player.resourcesSubTab.resourceGroup = tab;
                            }, managementType, RbSoundType.Tab), new RbTooltip_Text(DssRef.lang.Work_SelectCategory));

                        content.Add(subTab);
                       
                    }

                    var itemList = ResourceLib.ResourceGroupList(player.resourcesSubTab.resourceGroup);

                    foreach (var item in itemList)
                    {
                        content.newLine();
                        IconName.Item(item, out var itemIcon, out var itemName);

                        content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                            new RbImage(itemIcon), new RbSpace(), new RbText(TextLib.LargeFirstLetter(itemName)) },
                            player.hud.pins.isPinnedProperty)
                        { propertyTag = new CityHudPinId(city.myIndex, new HudPin(item)) });
                    }

                    content.newParagraph();

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Editor_Canvas_Clear) },
                        new RbAction(() => { player.hud.pins.clear(city); })));


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

        void resourceTabsInfo(RichBoxContent content, object tag)
        {
            ResourceManagementType tab = (ResourceManagementType)tag;
            IconName.Tab(tab, out SpriteName categoryIcon, out string category);

            content.Add(new RbBeginTitle());
            content.Add(new RbImage(categoryIcon));
            content.hspace();
            content.Add(new RbText(category, HudLib.TitleColor_Head));

            content.newLine();

            switch (tab)
            {
                case ResourceManagementType.Overview:
                    HudLib.BulletPoint(content);
                    GroupedResource.BufferIconInfo(content, false);
                    
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(1.ToString()));
                    content.Add(new RbImage( SpriteName.WarsResource_Food));
                    content.Add(new RbText(DssRef.lang.Resource_TypeName_Food));
                    var arrow = new RbImage(SpriteName.pjNumArrowR);
                    arrow.color = Color.CornflowerBlue;
                    content.Add(arrow);
                    content.Add(new RbText(string.Format(DssRef.lang.Hud_EnergyAmount, DssRef.storage.ruleset_instance.FoodEnergySett)));
                    
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.Work_BadValueDescription));

                    break;

                case ResourceManagementType.Work:
                    content.text( string.Format(DssRef.lang.Work_OrderPrioDescription, WorkTemplate.MaxPrio));
                    break;

                case ResourceManagementType.Stockpile:

                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.Resource_StockPile_Info));

                    content.newLine();
                    HudLib.BulletPoint(content);
                    GroupedResource.BufferIconInfo(content, false);
                    
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.Work_BadValueDescription));
                    content.text(DssRef.lang.StockPile_ItemsAreNotLost, HudLib.InfoYellow_Light);
                    break;
            }
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

                    for (ResourceGroupType group = 0; group < ResourceGroupType.NUM; group++)
                    {
                        if (group == ResourceGroupType.Mint)
                        {
                            bool includeMint = managementType == ResourceManagementType.Work && city.buildingStructure.CoinMinter_count > 0;
                            if (!includeMint)
                            {
                                continue;
                            }
                        }

                        IconName.Tab(group, out SpriteName groupIcon, out string groupName);
                        var tab = new ResourcesSubTab(managementType, group);
                        content.Add(new ArtButton(player.resourcesSubTab.EqualTab(tab)? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected,
                            new List<AbsRichBoxMember> { new RbImage(groupIcon) },
                            new RbAction1Arg<ResourcesSubTab>((resourcesSubTab) =>
                            {
                                if (player.resourcesSubTab.managementType != resourcesSubTab.managementType)
                                {   
                                    SoundLib.SubTab(resourcesSubTab.managementType);
                                }
                                player.resourcesSubTab = resourcesSubTab;

                            }, tab, RbSoundType.Option),
                            new RbTooltip(resourceTabToolTip, tab)));
                        
                    }

                    HudLib.InfoButton(content, new RbTooltip(resourceTabsInfo, managementType));

                }
            }
        

            switch (player.resourcesSubTab.managementType)
            {
                case ResourceManagementType.Overview:
                    resourceOverview(content, player.resourcesSubTab.resourceGroup); 
                    break;                

                case ResourceManagementType.Work:
                    content.h2(DssRef.lang.Work_OrderPrioTitle, HudLib.TitleColor_Head);
                    city.workTemplate.toHud(player, content, player.resourcesSubTab.resourceGroup, city.pfaction.GetFaction(), city);
                    break;

                case ResourceManagementType.Stockpile:
                    content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);
                    new StockPileMenu(content, city, city.pfaction.GetFaction()).toHud(player, player.resourcesSubTab.resourceGroup);
                    break;
            }

        }

        void resourceOverview(RichBoxContent content, ResourceGroupType resourceGroup)
        {
            bool reachedBuffer = false;

            switch (resourceGroup)
            {
                case ResourceGroupType.Resources:
                    content.newLine();
                    city.waterToHud(content, true);

                    //bool foodSafeGuard = city.foodSafeGuardIsActive(out bool fuelSafeGuard, out bool rawFoodSafeGuard, out bool woodSafeGuard);

                    city.GetGroupedResource(CityResourceIndex.wood).toMenu(content, ItemResourceType.Wood_Group, ref reachedBuffer, player, city);//New solution
                    
                    city.GetGroupedResource(CityResourceIndex.fuel).toMenu(content, ItemResourceType.Fuel_G, ref reachedBuffer, player, city);
                    int totalmines = 0;
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_coal, ItemResourceType.Coal, Map.SubTile.Empty, ref totalmines);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Fuel1, null, true);
                    content.space();
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Charcoal);

                    city.GetGroupedResource(CityResourceIndex.stone).toMenu(content, ItemResourceType.Stone_G, ref reachedBuffer, player, city);// Replace "res_stone", and continue with the rest
                    city.GetGroupedResource(CityResourceIndex.Clay).toMenu(content, ItemResourceType.Clay, ref reachedBuffer, player, city);// Replace "res_stone", and continue with the rest
                    city.GetGroupedResource(CityResourceIndex.Brick).toMenu(content, ItemResourceType.Brick, ref reachedBuffer, player, city);// Replace "res_stone", and continue with the rest
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.ClayBrick);

                    city.GetGroupedResource(CityResourceIndex.skinLinnen).toMenu(content, ItemResourceType.SkinLinen_Group, ref reachedBuffer, player, city);

                    content.newParagraph();

                    city.GetGroupedResource(CityResourceIndex.rawFood).toMenu(content, ItemResourceType.RawFood_Group, ref reachedBuffer, player, city);
                    city.GetGroupedResource(CityResourceIndex.Salt).toMenu(content, ItemResourceType.Salt, ref reachedBuffer, player, city);
                    

                    city.GetGroupedResource(CityResourceIndex.food).toMenu(content, ItemResourceType.Food_G, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Food1, CraftResourceLib.Food2);
                    content.space();

                    //content.Add(new ArtToggle(city.res_food_safeguard, new List<AbsRichBoxMember> {
                    //        new RbImage(city.res_food_safeguard? SpriteName.WarsProtectedStockpileOn : SpriteName.WarsProtectedStockpileOff, 0.7f),
                    //    },
                    //new RbAction(() =>
                    //{
                    //    city.res_food_safeguard = !city.res_food_safeguard;
                    //}),
                    //new RbTooltip((RichBoxContent content, object tag) =>
                    //{
                    //    content.text(string.Format(DssRef.lang.Resource_FoodSafeGuard_Description, DssConst.WorkSafeGuardAmount)).overrideColor = HudLib.InfoYellow_Light;
                    //    content.text(city.res_food_safeguard ? DssRef.lang.Hud_On : DssRef.lang.Hud_Off);
                    //})));

                    city.GetGroupedResource(CityResourceIndex.ConservedFood).toMenu(content, ItemResourceType.ConservedFood, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.ConservedFood_Barrel, CraftResourceLib.ConservedFood_Smoked);

                    city.GetGroupedResource(CityResourceIndex.beer).toMenu(content, ItemResourceType.Beer, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Beer);

                    city.GetGroupedResource(CityResourceIndex.coolingfluid).toMenu(content, ItemResourceType.CoolingFluid, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.CoolingFluid);
                    

                    


                    city.GetGroupedResource(CityResourceIndex.Container).toMenu(content, ItemResourceType.Container, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Container_wood, CraftResourceLib.Container_clay);

                    city.GetGroupedResource(CityResourceIndex.Palisade).toMenu(content, ItemResourceType.Palisade, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Palisade);

                    city.GetGroupedResource(CityResourceIndex.Toolkit).toMenu(content, ItemResourceType.Toolkit, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Toolkit);

                    city.GetGroupedResource(CityResourceIndex.Wagon2Wheel).toMenu(content, ItemResourceType.Wagon2Wheel, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Wagon2Wheel);

                    city.GetGroupedResource(CityResourceIndex.Wagon4Wheel).toMenu(content, ItemResourceType.Wagon4Wheel, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Wagon4Wheel);

                    city.GetGroupedResource(CityResourceIndex.WagonClosed).toMenu(content, ItemResourceType.WagonClosed, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.WagonClosed);

                    city.GetGroupedResource(CityResourceIndex.WagonIron).toMenu(content, ItemResourceType.WagonIron, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.WagonIron);

                    city.GetGroupedResource(CityResourceIndex.WagonSteel).toMenu(content, ItemResourceType.WagonSteel, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.WagonSteel);

                    city.GetGroupedResource(CityResourceIndex.BlackPowder).toMenu(content, ItemResourceType.BlackPowder, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.BlackPowder);

                    city.GetGroupedResource(CityResourceIndex.GunPowder).toMenu(content, ItemResourceType.GunPowder, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.GunPowder);

                    city.GetGroupedResource(CityResourceIndex.LedBullet).toMenu(content, ItemResourceType.LedBullet, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.LedBullets);

                    godPowerSetAllResources(content, ResourceLib.MovableCityResource_Misc);

                    break;

                case ResourceGroupType.Metals:

                    int totalMines = 0;

                    city.GetGroupedResource(CityResourceIndex.ironore).toMenu(content, ItemResourceType.IronOre_G, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_bogIron, ItemResourceType.BogIron, Map.SubTile.Empty, ref totalMines);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_iron, ItemResourceType.Iron_G, Map.SubTile.Empty, ref totalMines);

                    city.GetGroupedResource(CityResourceIndex.TinOre).toMenu(content, ItemResourceType.TinOre, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_tin, ItemResourceType.Tin, Map.SubTile.Empty, ref totalMines);

                    city.GetGroupedResource(CityResourceIndex.CopperOre).toMenu(content, ItemResourceType.CopperOre, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_copper, ItemResourceType.Copper, Map.SubTile.Empty, ref totalMines);

                    city.GetGroupedResource(CityResourceIndex.LeadOre).toMenu(content, ItemResourceType.LeadOre, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_lead, ItemResourceType.Lead, Map.SubTile.Empty, ref totalMines);

                    city.GetGroupedResource(CityResourceIndex.SilverOre).toMenu(content, ItemResourceType.SilverOre, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_silver, ItemResourceType.Silver, Map.SubTile.Empty, ref totalMines);

                    city.GetGroupedResource(CityResourceIndex.GoldOre).toMenu(content, ItemResourceType.GoldOre, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_gold, ItemResourceType.Gold, Map.SubTile.Empty, ref totalMines);
                    HudLib.blueprintButton(city, player, content, Minting.ConvertGoldOre);

                    content.newParagraph();


                    city.GetGroupedResource(CityResourceIndex.iron).toMenu(content, ItemResourceType.Iron_G, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Iron, CraftResourceLib.Iron_AndCooling);

                    city.GetGroupedResource(CityResourceIndex.Tin).toMenu(content, ItemResourceType.Tin, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Tin);

                    city.GetGroupedResource(CityResourceIndex.Copper).toMenu(content, ItemResourceType.Copper, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Copper, CraftResourceLib.Cupper_AndCooling);

                    city.GetGroupedResource(CityResourceIndex.Lead).toMenu(content, ItemResourceType.Lead, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Lead);

                    city.GetGroupedResource(CityResourceIndex.Silver).toMenu(content, ItemResourceType.Silver, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Silver, CraftResourceLib.Silver_AndCooling);

                    city.GetGroupedResource(CityResourceIndex.RawMithril).toMenu(content, ItemResourceType.RawMithril, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_mithril, ItemResourceType.Mithril, Map.SubTile.Empty, ref totalMines);

                    city.GetGroupedResource(CityResourceIndex.Sulfur).toMenu(content, ItemResourceType.Sulfur, ref reachedBuffer, player, city);
                    city.terrainStructure.mine(player, content, city.terrainStructure.mineCount_sulfur, ItemResourceType.Sulfur, Map.SubTile.Empty, ref totalMines);
                    content.newParagraph();


                    city.GetGroupedResource(CityResourceIndex.Bronze).toMenu(content, ItemResourceType.Bronze, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Bronze);

                    city.GetGroupedResource(CityResourceIndex.CastIron).toMenu(content, ItemResourceType.CastIron, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.CastIron);

                    city.GetGroupedResource(CityResourceIndex.BloomeryIron).toMenu(content, ItemResourceType.BloomeryIron, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.BloomeryIron);

                    city.GetGroupedResource(CityResourceIndex.Steel).toMenu(content, ItemResourceType.Steel, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Steel, CraftResourceLib.Steel_AndCooling);

                    city.GetGroupedResource(CityResourceIndex.Mithril).toMenu(content, ItemResourceType.Mithril, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Mithril);


                    godPowerSetAllResources(content, ResourceLib.MovableCityResource_Metals);
                    break;

                case ResourceGroupType.Weapons:

                    city.GetGroupedResource(CityResourceIndex.sharpstick).toMenu(content, ItemResourceType.SharpStick, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.SharpStick);

                    city.GetGroupedResource(CityResourceIndex.BronzeSword).toMenu(content, ItemResourceType.BronzeSword, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.BronzeSword);

                    city.GetGroupedResource(CityResourceIndex.HandSpear).toMenu(content, ItemResourceType.HandSpear, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.HandSpearIron, CraftResourceLib.HandSpearBronze);

                    city.GetGroupedResource(CityResourceIndex.shortsword).toMenu(content, ItemResourceType.ShortSword, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.ShortSword);

                    city.GetGroupedResource(CityResourceIndex.Sword).toMenu(content, ItemResourceType.Sword, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Sword);

                    city.GetGroupedResource(CityResourceIndex.LongSword).toMenu(content, ItemResourceType.LongSword, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.LongSword);

                   
                    city.GetGroupedResource(CityResourceIndex.Warhammer).toMenu(content, ItemResourceType.Warhammer, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.WarhammerIron, CraftResourceLib.WarhammerBronze);

                    city.GetGroupedResource(CityResourceIndex.twohandsword).toMenu(content, ItemResourceType.TwoHandSword, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.TwoHandSword);

                    city.GetGroupedResource(CityResourceIndex.MithrilSword).toMenu(content, ItemResourceType.MithrilSword, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MithrilSword);

                    content.newParagraph();

                    city.GetGroupedResource(CityResourceIndex.BucklerShield).toMenu(content, ItemResourceType.BucklerShield, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.BucklerShield);

                    city.GetGroupedResource(CityResourceIndex.RoundShield).toMenu(content, ItemResourceType.RoundShield, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.RoundShield);

                    city.GetGroupedResource(CityResourceIndex.HeaterShield).toMenu(content, ItemResourceType.HeaterShield, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.HeaterShield);

                    city.GetGroupedResource(CityResourceIndex.TowerShield).toMenu(content, ItemResourceType.TowerShield, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.TowerShield);

                    godPowerSetAllResources(content, ResourceLib.MovableCityResource_WeaponMelee);

                    break;

                case ResourceGroupType.Projectile:

                    city.GetGroupedResource(CityResourceIndex.SlingShot).toMenu(content, ItemResourceType.SlingShot, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Slingshot);

                    city.GetGroupedResource(CityResourceIndex.ThrowingSpear).toMenu(content, ItemResourceType.ThrowingSpear, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.ThrowingSpear1, CraftResourceLib.ThrowingSpear2);

                    city.GetGroupedResource(CityResourceIndex.bow).toMenu(content, ItemResourceType.Bow, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Bow);

                    city.GetGroupedResource(CityResourceIndex.longbow).toMenu(content, ItemResourceType.LongBow, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.LongBow);

                    city.GetGroupedResource(CityResourceIndex.crossbow).toMenu(content, ItemResourceType.Crossbow, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.CrossBow);

                    city.GetGroupedResource(CityResourceIndex.MithrilBow).toMenu(content, ItemResourceType.MithrilBow, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MithrilBow);


                    city.GetGroupedResource(CityResourceIndex.HandCannon).toMenu(content, ItemResourceType.HandCannon, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.BronzeHandCannon);

                    city.GetGroupedResource(CityResourceIndex.HandCulvertin).toMenu(content, ItemResourceType.HandCulverin, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.BronzeHandCulverin);

                    city.GetGroupedResource(CityResourceIndex.Rifle).toMenu(content, ItemResourceType.Rifle, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Rifle);

                    city.GetGroupedResource(CityResourceIndex.Blunderbuss).toMenu(content, ItemResourceType.Blunderbuss, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Blunderbuss);
                    content.newParagraph();

                    city.GetGroupedResource(CityResourceIndex.ballista).toMenu(content, ItemResourceType.Ballista, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Ballista_Iron, CraftResourceLib.Ballista_Bronze);

                    city.GetGroupedResource(CityResourceIndex.Manuballista).toMenu(content, ItemResourceType.Manuballista, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.ManuBallista);

                    city.GetGroupedResource(CityResourceIndex.Catapult).toMenu(content, ItemResourceType.Catapult, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.Catapult);

                    city.GetGroupedResource(CityResourceIndex.SiegeCannonBronze).toMenu(content, ItemResourceType.SiegeCannonBronze, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.SiegeCannonBronze);

                    city.GetGroupedResource(CityResourceIndex.ManCannonBronze).toMenu(content, ItemResourceType.ManCannonBronze, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.ManCannonBronze);

                    city.GetGroupedResource(CityResourceIndex.SiegeCannonIron).toMenu(content, ItemResourceType.SiegeCannonIron, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.SiegeCannonIron);

                    city.GetGroupedResource(CityResourceIndex.ManCannonIron).toMenu(content, ItemResourceType.ManCannonIron, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.ManCannonIron);


                    godPowerSetAllResources(content, ResourceLib.MovableCityResource_WeaponRanged);

                    break;

                case ResourceGroupType.Armor:

                    city.GetGroupedResource(CityResourceIndex.paddedArmor).toMenu(content, ItemResourceType.PaddedArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.PaddedArmor);

                    city.GetGroupedResource(CityResourceIndex.HeavyPaddedArmor).toMenu(content, ItemResourceType.HeavyPaddedArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.HeavyPaddedArmor);

                    city.GetGroupedResource(CityResourceIndex.BronzeArmor).toMenu(content, ItemResourceType.BronzeArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.BronzeArmor);

                    city.GetGroupedResource(CityResourceIndex.mailArmor).toMenu(content, ItemResourceType.IronArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MailArmor);

                    city.GetGroupedResource(CityResourceIndex.heavyMailArmor).toMenu(content, ItemResourceType.HeavyIronArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.HeavyMailArmor);

                    city.GetGroupedResource(CityResourceIndex.LightPlateArmor).toMenu(content, ItemResourceType.LightPlateArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.PlateArmor);

                    city.GetGroupedResource(CityResourceIndex.FullPlateArmor).toMenu(content, ItemResourceType.FullPlateArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.FullPlateArmor);

                    city.GetGroupedResource(CityResourceIndex.MithrilArmor).toMenu(content, ItemResourceType.MithrilArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MithrilArmor);

                    content.newParagraph();

                    // Mount Padded Armor
                    city.GetGroupedResource(CityResourceIndex.MountPaddedArmor).toMenu(content, ItemResourceType.MountPaddedArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountPaddedArmor);

                    // Mount Heavy Padded Armor
                    city.GetGroupedResource(CityResourceIndex.MountHeavyPaddedArmor).toMenu(content, ItemResourceType.MountHeavyPaddedArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountHeavyPaddedArmor);

                    // Mount Bronze Armor
                    city.GetGroupedResource(CityResourceIndex.MountBronzeArmor).toMenu(content, ItemResourceType.MountBronzeArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountBronzeArmor);

                    // Mount Iron Armor (mapped from Mail)
                    city.GetGroupedResource(CityResourceIndex.MountIronArmor).toMenu(content, ItemResourceType.MountIronArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountIronArmor);

                    // Mount Heavy Iron Armor (mapped from Heavy Mail)
                    city.GetGroupedResource(CityResourceIndex.MountHeavyIronArmor).toMenu(content, ItemResourceType.MountHeavyIronArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountHeavyIronArmor);

                    // Mount Light Plate Armor
                    city.GetGroupedResource(CityResourceIndex.MountLightPlateArmor).toMenu(content, ItemResourceType.MountLightPlateArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountLightPlateArmor);

                    // Mount Full Plate Armor
                    city.GetGroupedResource(CityResourceIndex.MountFullPlateArmor).toMenu(content, ItemResourceType.MountFullPlateArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountFullPlateArmor);

                    // Mount Mithril Armor
                    city.GetGroupedResource(CityResourceIndex.MountMithrilArmor).toMenu(content, ItemResourceType.MountMithrilArmor, ref reachedBuffer, player, city);
                    HudLib.blueprintButton(city, player, content, CraftResourceLib.MountMithrilArmor);

                    godPowerSetAllResources(content, ResourceLib.MovableCityResource_Armor);
                    break;

                case ResourceGroupType.Animals:

                    bool hideZeroAnimals = false;//DssRef.difficulty.setting_gameMode != GameModeMainType.Spectator && !StartupSettings.UnlockAllProgress;

                    // --- Farm ---
                    city.GetGroupedResource(CityResourceIndex.Fowl).toMenu(content, ItemResourceType.Fowl, ref reachedBuffer, player, city);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterFowl);

                    city.GetGroupedResource(CityResourceIndex.Hen).toMenu(content, ItemResourceType.Hen, ref reachedBuffer, player, city);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterHen);

                    city.GetGroupedResource(CityResourceIndex.Boar).toMenu(content, ItemResourceType.Boar, ref reachedBuffer, player, city);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterBoar);

                    city.GetGroupedResource(CityResourceIndex.Pig).toMenu(content, ItemResourceType.Pig, ref reachedBuffer, player, city);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterPig);


                    // --- Dogs ---
                    city.GetGroupedResource(CityResourceIndex.Dog).toMenu(content, ItemResourceType.Dog, ref reachedBuffer, player, city);
                    city.GetGroupedResource(CityResourceIndex.Hound).toMenu(content, ItemResourceType.Hound, ref reachedBuffer, player, city, hideZeroAnimals);

                    // --- Oxen ---
                    city.GetGroupedResource(CityResourceIndex.Oxen).toMenu(content, ItemResourceType.Oxen, ref reachedBuffer, player, city);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterOxen);

                    city.GetGroupedResource(CityResourceIndex.KineOxen).toMenu(content, ItemResourceType.KineOxen, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterKineOxen);


                    // --- Horses ---
                    city.GetGroupedResource(CityResourceIndex.Pony).toMenu(content, ItemResourceType.Pony, ref reachedBuffer, player, city);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterPony);

                    city.GetGroupedResource(CityResourceIndex.Horse).toMenu(content, ItemResourceType.Horse, ref reachedBuffer, player, city);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterHorse);

                    city.GetGroupedResource(CityResourceIndex.WarHorse).toMenu(content, ItemResourceType.WarHorse, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterWarHorse);

                    city.GetGroupedResource(CityResourceIndex.DraftHorse).toMenu(content, ItemResourceType.DraftHorse, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterDraftHorse);


                    // --- Wild Pigs ---
                    city.GetGroupedResource(CityResourceIndex.WildPig).toMenu(content, ItemResourceType.WildPig, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterWildPig);

                    city.GetGroupedResource(CityResourceIndex.WildHog).toMenu(content, ItemResourceType.WildHog, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterWildHog);

                    city.GetGroupedResource(CityResourceIndex.WarHog).toMenu(content, ItemResourceType.WarHog, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterWarHog);

                    city.GetGroupedResource(CityResourceIndex.StagHog).toMenu(content, ItemResourceType.StagHog, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterStagHog);


                    // --- Wolves ---
                    city.GetGroupedResource(CityResourceIndex.Wolf).toMenu(content, ItemResourceType.Wolf, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterWolf);

                    city.GetGroupedResource(CityResourceIndex.Warg).toMenu(content, ItemResourceType.Warg, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterWarg);

                    city.GetGroupedResource(CityResourceIndex.AlphaWarg).toMenu(content, ItemResourceType.AlphaWarg, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterAlphaWarg);


                    // --- Cats ---
                    city.GetGroupedResource(CityResourceIndex.WildCat).toMenu(content, ItemResourceType.WildCat, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterWildCat);

                    city.GetGroupedResource(CityResourceIndex.Lion).toMenu(content, ItemResourceType.Lion, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterLion);

                    city.GetGroupedResource(CityResourceIndex.WarLion).toMenu(content, ItemResourceType.WarLion, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterWarLion);


                    // --- Elephants ---
                    city.GetGroupedResource(CityResourceIndex.Elephant).toMenu(content, ItemResourceType.Elephant, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterElephant);

                    city.GetGroupedResource(CityResourceIndex.WarElephant).toMenu(content, ItemResourceType.WarElephant, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterWarElephant);

                    city.GetGroupedResource(CityResourceIndex.Oliphant).toMenu(content, ItemResourceType.Oliphant, ref reachedBuffer, player, city, hideZeroAnimals);
                    HudLib.butcherBlueprintButton(city, player, content, CraftResourceLib.SlaughterOliphant);

                    break;
            }
        }

        private void godPowerSetAllResources(RichBoxContent content, ItemResourceType[] Resources)
        {
            if (DssRef.difficulty.GodPowers())
            {
                content.newParagraph();

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

        public void burnToolTip()
        {
            RichBoxContent content = new RichBoxContent();

            content.text(DssRef.lang.CityOption_BurnItDown_Description);
        }
        
    }

    enum MenuTab
    {         
        Info,
        Statistics,
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
        Reassign,
        Progress,
        Mix,
        Help,
        Defence,
        CessPit,
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

    struct ResourcesSubTab
    {
        public ResourceGroupType resourceGroup;
        public ResourceManagementType managementType;

        public ResourcesSubTab(ResourceManagementType managementType, ResourceGroupType resourceGroup)
        { 
            this.managementType = managementType;
            this.resourceGroup = resourceGroup;
        }

        public bool EqualTab(ResourcesSubTab other)
        { 
            return resourceGroup == other.resourceGroup && managementType == other.managementType;
        }
    }

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
        TagSettings,
        NUM
    }
}
