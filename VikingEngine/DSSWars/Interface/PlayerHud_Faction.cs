using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.DSSWars.Interface
{
    class PlayerHud_Faction: IPlayerHud_Menu
    {
        public RichMenu menu;
        RichBoxContent content;
        public void createMenu(LocalPlayer player)
        {
            if (menu == null)
            {
                var objectMenuArea = new VectorRect(player.hud.head.factionMenuStart,
                    new Vector2(HudLib.HeadDisplayWidth, 500));
                objectMenuArea.SetBottom(player.playerData.view.safeScreenArea.Bottom, true);
                menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
                var bgTex = menu.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);

                bgTex.SetColor(ColorExt.GrayScale(0.9f));
                bgTex.SetOpacity(0.95f);
            }
        }

        public RichMenu Menu => menu;
        public bool IsFactionMenu { get { return true; } }

        void deleteMenu()
        {
            menu?.DeleteMe();
            menu = null;
        }

        public bool IsOpen()
        {
            return menu != null;
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

                    case MenuTab.StockPile:

                        player.faction.stockPileTab(player, content);
                        break;

                    case MenuTab.Work:
                        player.faction.workTab(content);
                        break;

                    //case MenuTab.Trade:
                    //    player.faction.tradeTab(content);
                    //    break;
                    case MenuTab.Progress:
                        progressTab(player);
                        break;
                }

                menu.Refresh(content, player.gameControls.controllerPointer);

            }

        }

        void infoTab(LocalPlayer player)
        {
           
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
            
            content.Add(new RbNewLine(true));

            content.text(string.Format(DssRef.lang.Hud_CityCount, TextLib.LargeNumber(player.faction.cities.Count)));
            content.text(string.Format(DssRef.lang.Hud_ArmyCount, TextLib.LargeNumber(player.faction.armies.Count)));

            content.ButtonDescription(player.gameControls.input.Build, DssRef.lang.Input_Build);
            content.ButtonDescription(player.gameControls.input.Copy, DssRef.lang.Hud_CopySetup);
            content.ButtonDescription(player.gameControls.input.Paste, DssRef.lang.Hud_Paste);

            content.newParagraph();
            
            content.newLine();
            toggleMenu(player);
        }
        void economyTab(LocalPlayer player)
        {
            bool advanced = !player.profile.casualControls;
            content.h2(DssRef.lang.UnitType_Cities).overrideColor = HudLib.TitleColor_Label;

            if (advanced)
            {
                content.newLine();
                content.Add(new RbImage(SpriteName.WarsResource_FoodAdd));
                content.space();
                content.Add(new RbText(string.Format(DssRef.lang.Economy_ResourceProduction, TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Food), Convert.ToInt32( player.faction.foodProduction.displayValue_gold_sec)), HudLib.AvailableColor));
                content.space();
                HudLib.PerSecondInfo(player, content, true);
            }
            if (advanced)
            {
                content.newLine();
                content.Add(new RbImage(SpriteName.WarsResource_FoodSub));
                content.space();
                content.Add(new RbText(string.Format(DssRef.lang.Economy_ResourceSpending, TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Food), Convert.ToInt32(player.faction.foodSpending.displayValue_gold_sec)), HudLib.NotAvailableColor));
                content.space();
                HudLib.PerSecondInfo(player, content, true);
            }
            
            {

                if (advanced)
                {
                    content.newParagraph();
                }
                else
                {
                    content.newLine();
                }

                content.Add(new RbImage(SpriteName.rtsIncomeTime));
                content.space();
                content.Add(new RbImage(SpriteName.WarsWorker));
                content.space();
                var textCont = new RbText(string.Format(DssRef.lang.Economy_TaxIncome, Money.CopperToGoldString_Large(player.faction.citiesEconomy.taxIncome_copp)),
                    HudLib.AvailableColor);
                content.Add(textCont);

                content.space();
                HudLib.InfoButton(content, new RbTooltip(HudLib.taxInfo));

            }

            {
                content.newLine();
                content.Add(new RbImage(SpriteName.rtsIncomeTime));
                content.space();
                content.Add(new RbImage(SpriteName.WarsResource_GoldOre));
                content.space();
                var textCont = new RbText(string.Format(DssRef.lang.Economy_SoldResources, player.faction.CitySoldResources),
                    HudLib.AvailableColor);
                content.Add(textCont);

                content.space();
                HudLib.PerSecondInfo(player, content, false);

            }

            if (advanced)
            {
                content.newLine();
                content.Add(new RbImage(SpriteName.rtsUpkeepTime));
                content.space();
                content.Add(new RbImage(SpriteName.WarsServiceMen));
                content.space();
                var textCont = new RbText(string.Format(DssRef.lang.Economy_ServicemenUpkeep, Money.CopperToGoldString_Dynamic(player.faction.citiesEconomy.servicemenUpkeep_copp)),
                    HudLib.NotAvailableColor);
                content.Add(textCont);

                content.space();
                HudLib.InfoButton(content, new RbTooltip(HudLib.servicemenUpkeepInfo));

            }
            {
                content.newLine();
                content.Add(new RbImage(SpriteName.rtsUpkeepTime));
                content.space();
                content.Add(new RbImage(SpriteName.WarsGuard));
                content.space();
                var textCont = new RbText(string.Format(DssRef.lang.Economy_GuardUpkeep, Money.CopperToGoldString_Dynamic(player.faction.citiesEconomy.cityGuardUpkeep_copp)),
                    HudLib.NotAvailableColor);
                content.Add(textCont);

                content.space();
                HudLib.InfoButton(content, new RbTooltip(HudLib.guardUpkeepInfo));

            }

            if (advanced)
            {
                content.newLine();
                content.Add(new RbImage(SpriteName.rtsUpkeepTime));
                content.space();
                content.Add(new RbText(string.Format(DssRef.lang.Economy_BlackMarketCostsForResource, DssRef.lang.Resource_TypeName_Food, Convert.ToInt32(player.faction.citiesEconomy.blackMarketCosts_Food_gold)), HudLib.NotAvailableColor));
                //content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Economy_BlackMarketCostsForResource, DssRef.lang.Resource_TypeName_Food, Convert.ToInt32(player.faction.citiesEconomy.blackMarketCosts_Food_gold)));
                content.space();
                HudLib.PerSecondInfo(player, content, true);
            }
            
            if (DssLib.UseLocalTrading)
            {
                content.icontext(SpriteName.rtsIncomeTime, string.Format(DssRef.lang.Economy_LocalCityTrade_Export, player.faction.CityTradeExport));
                content.space();
                HudLib.PerSecondInfo(player, content, false);

                content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Economy_LocalCityTrade_Import, player.faction.CityTradeImport));
                content.space();
                HudLib.PerSecondInfo(player, content, false);
            }


            content.newParagraph();
            content.h2(DssRef.lang.UnitType_Armies).overrideColor = HudLib.TitleColor_Label;

            if (advanced)
            {
                {
                    content.newLine();
                    content.Add(new RbImage(SpriteName.rtsUpkeepTime));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.lang.Economy_ResourceSpending, TextLib.LargeFirstLetter(DssRef.lang.ResourceType_Gold), /*player.ConvertUpkeep(*/Money.ToGold(Convert.ToInt32( player.faction.totalArmiesUpkeep.copper))/*, out _))*/), HudLib.NotAvailableColor));
                    content.space();
                    HudLib.PerSecondInfo(player, content, false);
                }

                {
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsResource_FoodSub));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.lang.Economy_ResourceSpending, TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Food), (int)player.faction.totalArmiesUpkeep.food/* player.ConvertUpkeep(player.faction.armyFoodUpkeep, out _))*/), HudLib.NotAvailableColor));
                    content.space();
                    HudLib.PerSecondInfo(player, content, false);
                }

                {
                    content.newLine();
                    content.Add(new RbImage(SpriteName.rtsUpkeepTime));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.lang.Economy_ImportCostsForResource, DssRef.lang.Resource_TypeName_Food, Convert.ToInt32(player.faction.armyFoodImportCost)), HudLib.NotAvailableColor));
                    content.space();
                    HudLib.PerSecondInfo(player, content, true);
                }
                {
                    content.newLine();
                    content.Add(new RbImage(SpriteName.rtsUpkeepTime));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.lang.Economy_BlackMarketCostsForResource, DssRef.lang.Resource_TypeName_Food, Convert.ToInt32(player.faction.armyFoodBlackMarketCost)), HudLib.NotAvailableColor));
                    content.space();
                    HudLib.PerSecondInfo(player, content, true);
                }
            }
            else
            {
                content.newLine();
                content.Add(new RbImage(SpriteName.rtsUpkeepTime));
                content.space();
                content.Add(new RbText(string.Format(DssRef.lang.Economy_ResourceSpending, TextLib.LargeFirstLetter(DssRef.lang.ResourceType_Gold), TextLib.OneDecimal(Money.ToGoldF(player.faction.totalArmiesUpkeep.copper))/* TextLib.OneDecimal( player.ConvertUpkeep(player.faction.armyFoodUpkeep, out _)))*/), HudLib.NotAvailableColor));
                content.space();
                HudLib.PerSecondInfo(player, content, false);
            }
        }
        void toggleMenu(LocalPlayer player)
        {
            content.Add(new RbImage(player.gameControls.input.ToggleHudDetail.Icon));
            content.Add(new RbImage(SpriteName.pjMenuIcon));
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
            new XP.TechnologyHud(player, null).technologyHud(content, player.faction);
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
