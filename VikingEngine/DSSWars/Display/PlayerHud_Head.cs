using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichMenu;
using VikingEngine.HUD;
using Microsoft.Xna.Framework;
using VikingEngine.HUD.RichBox;
using VikingEngine.DSSWars.Players;
using VikingEngine.LootFest.Players;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.ToGG.MoonFall;
using System.Globalization;
using System.Reflection.Metadata;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.Display
{
    class PlayerHud_Head
    {
        ImageAdvanced flag;
        NineSplitAreaTexture flagBg;
        RichMenu menu;
        public float Bottom;
        public Vector2 factionMenuStart;

        LocalPlayer player;
        public PlayerHud_Head(LocalPlayer player)
        {
            this.player = player;
            float headWidth = HudLib.HeadDisplayWidth * 1.6f;
            var headMenuArea = player.playerData.view.safeScreenArea;
            headMenuArea.Width = headWidth;
            menu = new RichMenu(HudLib.RbSettings_Head, headMenuArea, new Vector2(HudLib.MenuEdgeSize), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
            refreshFaction(player);
            menu.updateHeightFromContent();

            VectorRect flagBgArea = new VectorRect(headMenuArea.Position, new Vector2(menu.backgroundArea.Height * 1.1f));
            var flagBgTexSett = new NineSplitSettings(SpriteName.WarsHudFlagBorder, 1, 8, 1f, true, true);
            flagBg = new NineSplitAreaTexture(flagBgTexSett, flagBgArea, HudLib.GUILayer + 2);
            menu.move(VectorExt.V2FromX(flagBgArea.Size.X - 4));
            flagBgArea.AddRadius(-(flagBgTexSett.BorderWidth() + 6));
            flag = new ImageAdvanced(SpriteName.NO_IMAGE, flagBgArea.Position, flagBgArea.Size, HudLib.GUILayer, false);
            flag.Texture = player.faction.flagTexture;
            flag.SetFullTextureSource();

            var headBgTex = menu.addBackground(new NineSplitSettings(SpriteName.WarsHudHeadBarBg, 1, 16, 1f, true, true), HudLib.GUILayer + 4);
            headBgTex.SetOpacity(0.95f);

            Bottom = menu.backgroundArea.Bottom;

            factionMenuStart = new Vector2(menu.backgroundArea.X, Bottom);
        }
        public void refreshFaction(Players.LocalPlayer player)
        {
            var content = new RichBoxContent();
            headMenu(content, false);
            menu.Refresh(content);
        }
        public void refreshUpdate(LocalPlayer player)
        {
            refreshFaction(player);
        }

        /// <returns>need refresh</returns>
        public bool updateMouseInput(ref bool mouseOver)
        { 
            menu.updateMouseInput(ref mouseOver);
            return menu.needRefresh;
        }

        public void headMenu(RichBoxContent content, bool prepareLayout)
        {
            //LocalPlayer localPlayer = player.GetLocalPlayer();

            int gold;
            int income;

            int workForce;
            int totalStrength;

            int foodAdd, foodSub;

            int diplomancyPoints;
            int diplomacySoftMax;
            int diplomacyMax;

            int armyCount;
            int cityCount;

            var faction = player.faction;
            gold = faction.gold;
            income = faction.MoneySecDiff();
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

            //int tabSel = 0;
            //var tabs = new List<ArtTabMember>((int)MenuTab.NUM);
            for (int i = 0; i < HeadDisplay.Tabs.Length; ++i)
            {
                //var text = new RbText(LangLib.Tab( HeadDisplay.Tabs[i], out string description));
                //text.overrideColor = HudLib.RbSettings.tabSelected.Color;

                //AbsRbAction enter = null;
                //if (description != null)
                //{
                //    enter = new RbAction(() =>
                //    {
                //        RichBoxContent content = new RichBoxContent();
                //        content.text(description).overrideColor = HudLib.InfoYellow_Light;

                //        player.hud.tooltip.create(player, content, true);
                //    });
                //}
                var tab = HeadDisplay.Tabs[i];
                SpriteName icon = SpriteName.NO_IMAGE;
                switch (tab)
                {
                    case MenuTab.Info:
                        icon = SpriteName.WarsHudInfoIcon; break;
                    case MenuTab.Economy:
                        icon = SpriteName.rtsMoney; break;
                    case MenuTab.Resources:
                        icon = SpriteName.WarsResource_Wood; break;
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
                    }, new RbAction1Arg<MenuTab>(TabClick, tab), new RbTooltip(TabTip, tab)));
            }

            content.space(2);
            {
                RichBoxContent buttonContent = new RichBoxContent();
                buttonContent.Add(new RbImage(SpriteName.WarsCityHall));
                buttonContent.space(0.5f);
                buttonContent.Add(new RbText(cityCount.ToString()));
                content.Add(new ArtButton(RbButtonStyle.Outline, buttonContent, new RbAction1Arg<bool>(player.nextCity, true),
                    new RbTooltip(nextCityTip)));
            }
            {
                RichBoxContent buttonContent = new RichBoxContent();
                buttonContent.Add(new RbImage(SpriteName.WarsFlagType_Banner));
                buttonContent.space(0.5f);
                buttonContent.Add(new RbText(armyCount.ToString()));
                content.Add(new ArtButton(RbButtonStyle.Outline, buttonContent, new RbAction1Arg<bool>(player.nextArmy, true),
                    new RbTooltip(nextArmyTip)));
            }
        }

        void TabClick(MenuTab tab)
        {
            var player = this.player.GetLocalPlayer();
            player.mapControls.clearSelection();
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
            string name = LangLib.Tab(tab, out string description);
            content.Add(new RbText(name, HudLib.TitleColor_Label));
            //content.newLine();
            //content.Add(new RbText(description, HudLib.InfoYellow_Light));
        }

        void nextCityTip(RichBoxContent content, object tag)
        {
            //var player = this.player.GetLocalPlayer();
            content.Add(new RbText(string.Format(DssRef.lang.Hud_CityCount, player.faction.cities.Count), HudLib.InfoYellow_Light));
            content.newParagraph();
            content.ButtonDescription(player.input.NextCity, DssRef.lang.Input_NextCity);
        }
        void nextArmyTip(RichBoxContent content, object tag)
        {
            //var player = this.player.GetLocalPlayer();
            content.Add(new RbText(string.Format(DssRef.lang.Hud_ArmyCount, player.faction.armies.Count), HudLib.InfoYellow_Light));
            content.newParagraph();
            content.ButtonDescription(player.input.NextArmy, DssRef.lang.Input_NextArmy);
        }

        void factionGoldTip(RichBoxContent content, object tag)
        {
            content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.ResourceType_Gold, TextLib.LargeNumber(player.faction.gold))));
            content.newLine();
            content.Add(new RbText(string.Format(DssRef.lang.Hud_TotalIncome, TextLib.LargeNumber(player.faction.MoneySecDiff()))));
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
            content.Add(new RbImage(SpriteName.WarsDiplomaticAddTime));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Resource_AddPerSec, TextLib.ThreeDecimal(player.diplomacyAddPerSec_CapIncluded()))));

        }
        void foodTip(RichBoxContent content, object tag)
        {

            //foodAdd = faction.CityFoodProduction;
            //foodSub = faction.CityFoodSpending;
            content.Add(new RbImage(SpriteName.WarsResource_FoodAdd));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.todoLang.Info_TotalFoodProduction, player.faction.CityFoodProduction),
                HudLib.AvailableColor));

            content.newLine();

            content.Add(new RbImage(SpriteName.WarsResource_FoodSub));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.todoLang.Info_TotalFoodSpending, player.faction.CityFoodSpending),
                HudLib.NotAvailableColor));
        }
    }
    

    
    
}
