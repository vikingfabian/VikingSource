using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.DSSWars.Display
{
    class PlayerHud_Faction
    {
        RichMenu menu;
        RichBoxContent content;
        void createMenu(LocalPlayer player)
        {
            if (menu == null)
            {
                var objectMenuArea = new VectorRect(player.hud.head.factionMenuStart,
                    new Vector2(HudLib.HeadDisplayWidth, 500));
                //objectMenuArea.Width = HudLib.HeadDisplayWidth;
                //objectMenuArea.Position.Y = player.hud.head.Bottom + Engine.Screen.IconSize * 0.5f;
                objectMenuArea.SetBottom(player.playerData.view.safeScreenArea.Bottom, true);
                menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
                var bgTex = menu.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);

                bgTex.SetColor(ColorExt.GrayScale(0.9f));
                bgTex.SetOpacity(0.95f);
            }
        }

        void deleteMenu()
        {
            menu?.DeleteMe();
            menu = null;
        }

        public void refreshUpdate(LocalPlayer player)
        {
            if (player.factionTab == MenuTab.NUM_NONE)
            {
                deleteMenu();
            }
            else
            {
                createMenu(player);
                content = new RichBoxContent();
                switch (player.factionTab)
                {
                    case MenuTab.Info:
                        infoTab(player);
                        break;

                    case MenuTab.Economy:
                        economyTab(player);
                        break;

                    case MenuTab.Automation:
                        player.automation.toMenu(content, true);
                        break;

                    case MenuTab.Resources:
                        player.faction.resourceTab(player, content);
                        break;

                    case MenuTab.Work:
                        player.faction.workTab(content);
                        break;

                    case MenuTab.Trade:
                        player.faction.tradeTab(content);
                        break;
                    case MenuTab.Progress:
                        progressTab(player);
                        break;
                }

                menu.Refresh(content);

            }

        }

        void infoTab(LocalPlayer player)
        {
            //FactionSize(faction, content, fullDisplay);


            //if (fullDisplay)
            //{
            //content.newParagraph();
            //content.Add(new RichBoxNewLine());
            //content.Add(new RichBoxImage(SpriteName.rtsIncomeTime));
            //content.space();
            //content.Add(new RichBoxText(string.Format(DssRef.lang.Hud_TotalIncome, TextLib.LargeNumber(Convert.ToInt32( faction.citiesEconomy.tax())))));

            //content.Add(new RichBoxNewLine());
            //content.Add(new RichBoxImage(SpriteName.rtsUpkeepTime));
            //content.space();
            ////content.Add(new RichBoxText(string.Format(DssRef.lang.Hud_ArmyUpkeep, TextLib.LargeNumber(faction.armyUpkeep))));

            //content.newLine();
            //var automationButton = new HUD.RichBox.RichboxButton(
            //    new List<AbsRichBoxMember>
            //    {
            //            new RichBoxImage(player.input.AutomationSetting.Icon),
            //            new RichBoxImage(SpriteName.MenuPixelIconSettings),
            //            new HUD.RichBox.RichBoxText(DssRef.lang.Automation_Title),
            //    },
            //    new RbAction1Arg<string>(player.hud.displays.SetMenuState, AutomationMenuState, SoundLib.menu),
            //    null);
            //content.Add(automationButton);
            //content.Button(SpriteName.MenuPixelIconSettings, "Automation", new RbAction(DssRef.state.exit), null, true);

            //string diplomacy = "Diplomatic points: {0}/{1}({2})";
            content.icontext(SpriteName.WarsDiplomaticPoint, string.Format(DssRef.lang.ResourceType_DiplomacyPoints_WithSoftAndHardLimit, player.diplomaticPoints.Int(), player.diplomaticPoints_softMax, player.diplomaticPoints.max));
            content.space();
            HudLib.InfoButton(content, new RbAction(() =>
            {
                RichBoxContent content = new RichBoxContent();
                content.h2(TextLib.LargeFirstLetter(DssRef.lang.ResourceType_DiplomacyPoints)).overrideColor = HudLib.TitleColor_Label;
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

                player.hud.tooltip.create(player, content, true);
            }));
            //content.icontext(SpriteName.WarsGroupIcon, string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_MercenaryMarket, player.mercenaryMarket.Int()));
            //string command = "Command points: {0}";
            //content.icontext(SpriteName.WarsCommandPoint, string.Format(command, player.commandPoints.ToString()));

            content.Add(new RbNewLine(true));

            //if (player.hud.detailLevel == HudDetailLevel.Extended)
            //{
            content.text(string.Format(DssRef.lang.Hud_CityCount, TextLib.LargeNumber(player.faction.cities.Count)));
            content.text(string.Format(DssRef.lang.Hud_ArmyCount, TextLib.LargeNumber(player.faction.armies.Count)));

            content.ButtonDescription(player.input.NextCity, DssRef.lang.Input_NextCity);
            content.ButtonDescription(player.input.NextArmy, DssRef.lang.Input_NextArmy);
            content.ButtonDescription(player.input.NextBattle, DssRef.lang.Input_NextBattle);

            content.ButtonDescription(player.input.Build, DssRef.lang.Input_Build);
            content.ButtonDescription(player.input.Copy, DssRef.lang.Hud_CopySetup);
            content.ButtonDescription(player.input.Paste, DssRef.lang.Hud_Paste);

            content.newParagraph();
            //}

            //if (Ref.isPaused && player.IsLocalHost())
            //{
            //    content.Button("Exit game", new RbAction(DssRef.state.exit), null, true);
            //}
            content.newLine();
            toggleMenu(player);
            //}
        }
        void economyTab(LocalPlayer player)
        {

            content.h2(DssRef.lang.UnitType_Cities).overrideColor = HudLib.TitleColor_Label;

            content.icontext(SpriteName.WarsResource_FoodAdd, string.Format(DssRef.lang.Economy_ResourceProduction, TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Food), player.faction.CityFoodProduction));
            content.space();
            HudLib.PerSecondInfo(player, content, true);

            content.icontext(SpriteName.WarsResource_FoodSub, string.Format(DssRef.lang.Economy_ResourceSpending, TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Food), player.faction.CityFoodSpending));
            content.space();
            HudLib.PerSecondInfo(player, content, true);

            content.icontext(SpriteName.rtsIncomeTime, string.Format(DssRef.lang.Economy_TaxIncome, Convert.ToInt32(player.faction.citiesEconomy.tax(null))));
            content.space();
            HudLib.InfoButton(content, new RbTooltip(taxInfo));

            content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Economy_BlackMarketCostsForResource, DssRef.lang.Resource_TypeName_Food, Convert.ToInt32(player.faction.citiesEconomy.blackMarketCosts_Food)));
            content.space();
            HudLib.PerSecondInfo(player, content, true);

            content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Economy_GuardUpkeep, Convert.ToInt32(player.faction.citiesEconomy.cityGuardUpkeep)));
            content.space();
            HudLib.PerSecondInfo(player, content, false);

            if (DssLib.UseLocalTrading)
            {
                content.icontext(SpriteName.rtsIncomeTime, string.Format(DssRef.lang.Economy_LocalCityTrade_Export, player.faction.CityTradeExport));
                content.space();
                HudLib.PerSecondInfo(player, content, false);

                content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Economy_LocalCityTrade_Import, player.faction.CityTradeImport));
                content.space();
                HudLib.PerSecondInfo(player, content, false);
            }
            content.icontext(SpriteName.rtsIncomeTime, string.Format(DssRef.lang.Economy_SoldResources, player.faction.CitySoldResources));
            content.space();
            HudLib.PerSecondInfo(player, content, false);

            content.icontext(SpriteName.WarsBuild_Nobelhouse, string.Format(DssRef.lang.Language_XCountIsY, DssRef.lang.Building_NobleHouse, player.faction.nobelHouseCount));

            content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Language_XUpkeepIsY, DssRef.lang.Building_NobleHouse, DssLib.NobleHouseUpkeep * player.faction.nobelHouseCount));
            content.space();
            HudLib.PerSecondInfo(player, content, false);
            //

            content.newParagraph();
            content.h2(DssRef.lang.UnitType_Armies).overrideColor = HudLib.TitleColor_Label;

            content.text(string.Format(DssRef.lang.Economy_ResourceSpending, TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Food), player.faction.armyFoodUpkeep));
            content.space();
            HudLib.PerSecondInfo(player, content, false);

            content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Economy_ImportCostsForResource, DssRef.lang.Resource_TypeName_Food, Convert.ToInt32(player.faction.armyFoodImportCost)));
            content.space();
            HudLib.PerSecondInfo(player, content, true);

            content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Economy_BlackMarketCostsForResource, DssRef.lang.Resource_TypeName_Food, Convert.ToInt32(player.faction.armyFoodBlackMarketCost)));
            content.space();
            HudLib.PerSecondInfo(player, content, true);
        }
        void toggleMenu(LocalPlayer player)
        {
            content.Add(new RbImage(player.input.ToggleHudDetail.Icon));
            content.Add(new RbImage(SpriteName.pjMenuIcon));
        }
        void taxInfo(RichBoxContent content, object tag)
        {
            //RichBoxContent content = new RichBoxContent();
            content.text(string.Format(DssRef.lang.Economy_TaxDescription, DssConst.TaxPerWorker));
            content.newParagraph();
            content.text(DssRef.lang.Info_PerSecond);
            //player.hud.tooltip.create(player, content, true);
        }


        void debugCultureFont()
        {
            string[] cultures = new string[]
            {
                    //"en-US",  // English (United States)
                    //"de-DE",  // German (Germany)
                    //"fr-FR",  // French (France)
                    //"de-CH",  // German (Switzerland)
                    //"pt-BR",  // Portuguese (Brazil)
                    //"it-IT",  // Italian (Italy)
                    //"es-ES",  // Spanish (Spain)
                    //"nl-NL",  // Dutch (Netherlands)
                    //"sv-SE",  // Swedish (Sweden)
                    //"da-DK",  // Danish (Denmark)
                    //"fi-FI",  // Finnish (Finland)
                    //"ru-RU",  // Russian (Russia)
                    //"zh-CN",  // Chinese (Simplified, China)
                    //"ja-JP",  // Japanese (Japan)
                    //"ko-KR",  // Korean (Korea)
                    "ar-SA",  // Arabic (Saudi Arabia)
                              //"hi-IN",  // Hindi (India)
                              //"th-TH",  // Thai (Thailand)
                              //"he-IL",  // Hebrew (Israel)
            };


            double number = 1234567.89;

            foreach (string s in cultures)
            {
                CultureInfo culture = new CultureInfo(s);
                string formatted = (0.1).ToString("ar-SA");

                content.text(s + formatted);
            }

        }


        void progressTab(Players.LocalPlayer player)
        {
            new XP.TechnologyHud().technologyHud(content, player, null, player.faction);
        }
        public bool updateMouseInput(ref bool mouseOver)
        {
            if (menu != null)
            {
                menu.updateMouseInput(ref mouseOver);
                return menu.needRefresh;
            }
            return false;
        }
    }
}
