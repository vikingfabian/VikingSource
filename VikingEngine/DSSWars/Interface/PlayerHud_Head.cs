using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.Graphics;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Interface
{
    class PlayerHud_Head
    {
        ImageAdvanced flag;
        NineSplitAreaTexture flagBg;
        RichMenu menu;
        public float Bottom;
        public float Right;
        public Vector2 factionMenuStart;

        LocalPlayer player;

        public static readonly MenuTab[] CasualTabs = { MenuTab.Economy };
        public static readonly MenuTab[] Tabs = { MenuTab.Economy, MenuTab.Resources, MenuTab.StockPile, MenuTab.Work, /*MenuTab.Automation*/ MenuTab.Progress };
        public static readonly MenuTab[] TutorialTabs = { MenuTab.Economy };

        public PlayerHud_Head(LocalPlayer player)
        {
            this.player = player;
            float headWidth = HudLib.HeadDisplayWidth * 1.75f;
            var headMenuArea = player.playerData.view.safeScreenArea;
            headMenuArea.Width = headWidth;
            menu = new RichMenu(HudLib.RbSettings_Head, headMenuArea, new Vector2(HudLib.MenuEdgeSize), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
            refreshFaction(player, true);
            menu.updateHeightFromContent();

            if (DssRef.difficulty.setting_gameMode != Data.GameModeMainType.Spectator)
            {
                VectorRect flagBgArea = new VectorRect(headMenuArea.Position, new Vector2(menu.backgroundArea.Height * 1.1f));
                var flagBgTexSett = new NineSplitSettings(SpriteName.WarsHudFlagBorder, 1, 8, 1f, true, true);
                flagBg = new NineSplitAreaTexture(flagBgTexSett, flagBgArea, HudLib.GUILayer + 2);
                menu.move(VectorExt.V2FromX(flagBgArea.Size.X - 4));
                flagBgArea.AddRadius(-(flagBgTexSett.BorderWidth() + 6));
                flag = new ImageAdvanced(SpriteName.NO_IMAGE, flagBgArea.Position, flagBgArea.Size, HudLib.GUILayer, false);
                flag.Texture = player.flagTexture;
                flag.SetFullTextureSource();

                var headBgTex = menu.addBackground(new NineSplitSettings(SpriteName.WarsHudHeadBarBg, 1, 16, 1f, true, true), HudLib.GUILayer + 4);
                headBgTex.SetOpacity(0.95f);
            }
                       

            Bottom = menu.backgroundArea.Bottom;
            Right = menu.backgroundArea.Right;

            factionMenuStart = new Vector2(menu.backgroundArea.X, Bottom);
        }
        public void refreshFaction(Players.LocalPlayer player, bool prepareLayout)
        {
            var content = new RichBoxContent();
            headMenu(content, prepareLayout);
            menu.Refresh(content, player.gameControls.controllerPointer);
        }
        public void refreshUpdate(LocalPlayer player)
        {
            refreshFaction(player, false);
        }

        /// <returns>need refresh</returns>
        public bool updateMouseInput(ref bool mouseOver)
        { 
            menu.updateMouseInput(ref mouseOver);
            return menu.needRefresh;
        }

        public MenuTab[] factionTabOptions()
        {
            if (player.profile.casualControls)
            {
                return CasualTabs;
            }
            else
            {
                return DssRef.storage.runTutorial_1short_2normal == 0 ? Tabs : TutorialTabs;
            }
        }

        public void headMenu(RichBoxContent content, bool prepareLayout)
        {
            //LocalPlayer localPlayer = player.GetLocalPlayer();

            if (DssRef.difficulty.setting_gameMode == Data.GameModeMainType.Spectator)
            {
                return;
            }

            long gold;
            long income;

            int workForce;
            int totalStrength;

            int foodAdd, foodSub;

            int diplomancyPoints;
            int diplomacySoftMax;
            int diplomacyMax;

            int armyCount;
            int cityCount;

            var faction = player.faction;
            gold = faction.money.GetGold();
            income = faction.GoldSecDiff();
            workForce = faction.totalWorkForce;
            totalStrength = Convert.ToInt32(faction.militaryStrength);
            foodAdd = faction.CityFoodProduction;
            foodSub = faction.CityFoodSpending;
            workForce = faction.totalWorkForce;
            diplomancyPoints = player.diplomaticPoints.Int();
            diplomacySoftMax = player.diplomaticPoints_softMax;
            diplomacyMax = player.diplomaticPoints.max;
            armyCount = faction.armies.Count;
            cityCount = faction.cities.Count;

            {
                RichBoxContent buttonContent = new RichBoxContent();
                buttonContent.Add(new RbImage(SpriteName.rtsMoney));
                buttonContent.Add(new RbText(TextLib.LargeNumber(gold), HudLib.NegativeRed(gold)));
                buttonContent.space();
                buttonContent.Add(new RbImage(SpriteName.rtsIncomeTime));
                buttonContent.Add(new RbText(TextLib.LargeNumber(income), HudLib.NegativeRed(income)));

                content.Add(new ArtButton(RbButtonStyle.HoverArea, buttonContent, null, 
                    new RbTooltip(factionGoldTip)));
            }

            content.Add(new RbTab(0.3f));
            {
                RichBoxContent buttonContent = new RichBoxContent();
                buttonContent.Add(new RbImage(SpriteName.WarsWorker));
                buttonContent.space(0.5f);
                buttonContent.Add(new RbText(TextLib.LargeNumber(workForce)));
                content.Add(new ArtButton(RbButtonStyle.HoverArea, buttonContent, null,
                    new RbTooltip_Text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.ResourceType_Workers, workForce))));
            }

            content.Add(new RbTab(0.45f));
            {
                RichBoxContent buttonContent = new RichBoxContent();
                buttonContent.Add(new RbImage(SpriteName.WarsStrengthIcon));
                buttonContent.space(0.5f);
                buttonContent.Add(new RbText(TextLib.LargeNumber(totalStrength)));
                content.Add(new ArtButton(RbButtonStyle.HoverArea, buttonContent, null,
                    new RbTooltip_Text(string.Format(DssRef.lang.Hud_TotalStrengthRating, TextLib.LargeNumber(Convert.ToInt32(totalStrength))))));
            }

            content.Add(new RbTab(0.6f));
            {
                RichBoxContent buttonContent = new RichBoxContent();
                buttonContent.Add(new RbImage(SpriteName.WarsDiplomaticPoint));
                buttonContent.space(0.5f);
                buttonContent.Add(new RbText($"{diplomancyPoints}/{diplomacySoftMax}({diplomacyMax})"));
                content.Add(new ArtButton(RbButtonStyle.HoverArea, buttonContent, null, 
                    new RbTooltip(diplomacyTip)));
            }

            
            
            content.Add(new RbTab(0.8f));
            if (!player.profile.casualControls)
            {
                int foodSum = foodAdd - foodSub;
                RichBoxContent buttonContent = new RichBoxContent();
                buttonContent.Add(new RbImage(SpriteName.WarsResource_FoodAdd));
                buttonContent.space(0.5f);
                buttonContent.Add(new RbText(TextLib.LargeNumber(foodSum), HudLib.NegativeRed(foodSum)));
                content.Add(new ArtButton(RbButtonStyle.HoverArea, buttonContent, null,
                    new RbTooltip(foodTip)));
                    
            }

            content.newLine();

            if (player.gameControls.input.inputSource.IsController)
            {
                content.Add(new RbImage(player.gameControls.input.ControllerFaction.Icon) { color = player.gameControls.controller_mayUseHeadDisplay()? Color.White : Color.Black });
                content.space();                
            }
            bool viewControllerTabs = player.gameControls.tabFocusColor(Players.PlayerControls.ControllerTabFocus.Headmenu, out Color focusColor);
            if (viewControllerTabs)
            {
                content.Add(new RbImage(player.gameControls.input.Controller_TabLeft.Icon) { color = focusColor });
                content.space(0.5f);
            }

            if (player.mapLayer() >= Map.MapDetailLayerType.FactionColors3 && !prepareLayout)
            {
                mapFilterTabs(content);
            }
            else
            {
                factionTabs(content);
            }

            if (viewControllerTabs)
            {
                content.space(0.5f);
                content.Add(new RbImage(player.gameControls.input.Controller_TabRight.Icon) { color = focusColor });
            }

            content.space(2);
            {
                RichBoxContent buttonContent = new RichBoxContent();
                if (player.gameControls.input.inputSource.IsController)
                {
                    buttonContent.Add(new RbImage(player.gameControls.input.NextCity.Icon));
                    buttonContent.space(0.5f);
                }
                buttonContent.Add(new RbImage(SpriteName.WarsCityHall));
                buttonContent.space(0.5f);
                buttonContent.Add(new RbText(cityCount.ToString()));
                content.Add(new ArtButton(RbButtonStyle.Outline, buttonContent, new RbAction1Arg<bool>(player.gameControls.nextCity, true),
                    new RbTooltip(nextCityTip)));
            }
            {
                RichBoxContent buttonContent = new RichBoxContent();
                if (player.gameControls.input.inputSource.IsController)
                {
                    buttonContent.Add(new RbImage(player.gameControls.input.NextArmy.Icon));
                    buttonContent.space(0.5f);
                }
                buttonContent.Add(new RbImage(SpriteName.WarsFlagType_Banner));
                buttonContent.space(0.5f);
                buttonContent.Add(new RbText(armyCount.ToString()));
                content.Add(new ArtButton(RbButtonStyle.Outline, buttonContent, new RbAction1Arg<bool>(player.gameControls.nextArmy, true),
                    new RbTooltip(nextArmyTip)));
            }
            {

                RichBoxContent buttonContent = new RichBoxContent();
                string toolTip;
                if (player.warCount == 0 && !prepareLayout)
                {
                    buttonContent.Add(new RbImage(SpriteName.WarsRelationPeace));
                    toolTip = DssRef.lang.WorkForce_Peace;
                }
                else
                {
                    if (player.gameControls.input.inputSource.IsController)
                    {
                        player.gameControls.input.NextWar.ToRichContent(buttonContent);
                        buttonContent.space(0.5f);
                    }
                    buttonContent.Add(new RbImage(SpriteName.WarsRelationWar));
                    toolTip = DssRef.lang.InputActionName_NextWar;
                }
                buttonContent.space(0.5f);
                buttonContent.Add(new RbText(player.warCount.ToString()));

                content.Add(new ArtButton(RbButtonStyle.Outline, buttonContent, new RbAction1Arg<bool>(player.gameControls.nextWar, true),
                    new RbTooltip_Text(toolTip)));
            }

        }

        void factionTabs(RichBoxContent content)
        {
            MenuTab[] tabOptions = factionTabOptions();
            for (int i = 0; i < tabOptions.Length; ++i)
            {
                var tab = tabOptions[i];
                SpriteName icon = SpriteName.NO_IMAGE;
                switch (tab)
                {
                    case MenuTab.Info:
                        icon = SpriteName.WarsHudInfoIcon; break;
                    case MenuTab.Economy:
                        icon = SpriteName.rtsMoney; break;
                    case MenuTab.Resources:
                        icon = SpriteName.WarsResource_Wood; break;
                    case MenuTab.StockPile:
                        icon = SpriteName.WarsStockpileAdd; break;
                    case MenuTab.Work:
                        icon = SpriteName.WarsHammer; break;
                    case MenuTab.Automation:
                        icon = SpriteName.AutomationGearIcon; break;
                    case MenuTab.Progress:
                        icon = SpriteName.WarsTechnology_Unlocked; break;

                }

                content.Add(new ArtOption(tab == player.factionTab,
                    new List<AbsRichBoxMember>
                    {
                        new RbImage(icon)
                    }, new RbAction1Arg<MenuTab>(TabClick, tab, RbSoundType.Option), new RbTooltip(TabTip, tab)));
            }
        }

        void mapFilterTabs(RichBoxContent content)
        {
            for (FactionMapFilter filter = 0; filter < FactionMapFilter.NUM; filter++)
            {
                SpriteName icon;

                switch (filter)
                {
                    default: icon = SpriteName.MissingImage; break;

                    case FactionMapFilter.FactionCols:
                        icon = SpriteName.WarsMapFilterFactions;
                        break;
                    case FactionMapFilter.Terrain:
                        icon = SpriteName.WarsMapFilterTerrain;
                        break;
                    case FactionMapFilter.Minimap:
                        icon = SpriteName.WarsMapFilterMinimap;
                        break;

                    case FactionMapFilter.PopulationHeatmap:
                        icon = SpriteName.WarsMapFilterWorkers;
                        break;
                    case FactionMapFilter.StrengthHeatmap:
                        icon = SpriteName.WarsMapFilterStrength;
                        break;

                }

                content.Add(new ArtOption(filter == player.factionPixelTexture.filter,
                   new List<AbsRichBoxMember>
                   {
                        new RbImage(icon)
                   }, new RbAction1Arg<FactionMapFilter>((FactionMapFilter filter)=>
                       { 
                            player.factionPixelTexture.filter = filter;
                            DssRef.world.BordersUpdated = true;
                       }
                   , filter, RbSoundType.Option), new RbTooltip_Text(DssRef.todoLang.MapFilter)));
            }
            
        }

        public void TabClick(MenuTab tab)
        {
            var player = this.player.GetLocalPlayer();
            player.gameControls.map.clearSelection();
            if (player.factionTab == tab)
            {
                player.factionTab = MenuTab.NUM_NONE;
            }
            else
            {
                player.factionTab = tab;
            }
            player.hud.needRefresh = true;
        }

        void TabTip(RichBoxContent content, object tag)
        {
            var tab = (MenuTab)tag;
            string name = LangLib.Tab(tab, out string description, out _);
            content.h1(name, HudLib.TitleColor_Head);
            content.h2(DssRef.lang.FactionSettings_Titel);
            content.newLine();
            content.Add(new RbText(DssRef.lang.FactionSettings_Description, HudLib.InfoYellow_Light));
            
            //content.newLine();
            //content.Add(new RbText(description, HudLib.InfoYellow_Light));
        }

        void nextCityTip(RichBoxContent content, object tag)
        {
            //var player = this.player.GetLocalPlayer();
            content.Add(new RbText(string.Format(DssRef.lang.Hud_CityCount, player.faction.cities.Count), HudLib.InfoYellow_Light));
            content.newParagraph();
            content.ButtonDescription(player.gameControls.input.NextCity, DssRef.lang.InputActionName_NextCity);
        }
        void nextArmyTip(RichBoxContent content, object tag)
        {
            //var player = this.player.GetLocalPlayer();
            content.Add(new RbText(string.Format(DssRef.lang.Hud_ArmyCount, player.faction.armies.Count), HudLib.InfoYellow_Light));
            content.newParagraph();
            content.ButtonDescription(player.gameControls.input.NextArmy, DssRef.lang.InputActionName_NextArmy);
        }

        void factionGoldTip(RichBoxContent content, object tag)
        {
            content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.ResourceType_Gold, TextLib.LargeNumber(player.faction.money.GetGold()))));
            content.newLine();
            content.Add(new RbText(string.Format(DssRef.lang.Hud_TotalIncome, TextLib.LargeNumber(player.faction.GoldSecDiff()))));
        }

        void diplomacyTip(RichBoxContent content, object tag)
        {

            content.h2(TextLib.LargeFirstLetter(DssRef.lang.ResourceType_DiplomacyPoints), HudLib.TitleColor_Head);
            content.newLine();
            content.Add(new RbImage(SpriteName.WarsDiplomaticPoint));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Resource_CurrentAmount, player.diplomaticPoints.Int())));

            content.text(string.Format(DssRef.lang.Resource_MaxAmount_Soft, player.diplomaticPoints_softMax));
            content.text(string.Format(DssRef.lang.Resource_MaxAmount, player.diplomaticPoints.max));

            
            content.newLine();
            HudLib.Label(content, "Below soft cap");
            content.newLine();
            content.Add(new RbImage(SpriteName.WarsDiplomaticAddTime));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Resource_AddPerSec, TextLib.ThreeDecimal(player.diplomacyAddPerSec()))));

            content.newLine();
            HudLib.Label(content, "Above soft cap");
            content.newLine();
            content.Add(new RbImage(SpriteName.WarsDiplomaticAddTime));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Resource_AddPerSec, TextLib.ThreeDecimal(player.diplomacyAddPerSec_CapIncluded()))));

            content.newLine();
            content.Add(new RbSeperationLine());
            content.Add(new RbBeginTitle());
            content.Add(new RbImage(SpriteName.WarsBuild_Embassy));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Language_XCountIsY, DssRef.lang.BuildingType_Embassy, player.faction.embassyCount), HudLib.TitleColor_Head));

            content.newLine();
            int diplomacydSec = Convert.ToInt32(DssRef.diplomacy.EmbassyAddDiplomacy * 3600);

            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsDiplomaticAddTime));
            content.Add(new RbText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsAdd, diplomacydSec)));
            content.newLine();

            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsDiplomaticPoint));
            content.Add(new RbText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsLimit, DssRef.diplomacy.EmbassyAddMaxDiplomacy)));
            content.newLine();
        }
        void foodTip(RichBoxContent content, object tag)
        {

            //foodAdd = faction.CityFoodProduction;
            //foodSub = faction.CityFoodSpending;
            content.Add(new RbImage(SpriteName.WarsResource_FoodAdd));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Info_TotalFoodProduction, player.faction.CityFoodProduction),
                HudLib.AvailableColor));

            content.newLine();

            content.Add(new RbImage(SpriteName.WarsResource_FoodSub));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Info_TotalFoodSpending, player.faction.CityFoodSpending),
                HudLib.NotAvailableColor));
        }
    }
    

    
    
}
