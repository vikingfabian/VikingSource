using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Display
{
    class HeadDisplay : RichboxGuiPart
    {        
        public bool fullDisplay = true;
        public const string AutomationMenuState = "auto";
        public static readonly MenuTab[] Tabs = { MenuTab.Info, MenuTab.Economy, MenuTab.Automation, MenuTab.Work };
        public HeadDisplay(RichboxGui gui)
            :base(gui)
        {
        }

        public void refreshUpdate(Players.LocalPlayer player, bool fullDisplay, bool refresh, Faction faction)
        {
            if (bg.Visible && refresh)
            {
                beginRefresh();

                //if (player.tutorial != null)
                //{
                //    player.tutorial.tutorial_ToHud(content);
                //    content.Add(new RichBoxSeperationLine());
                //    content.newParagraph();
                //}

                defaultMenu(player, fullDisplay, faction);

                if (fullDisplay && player.tutorial == null)
                {
                    if (player.input.inputSource.IsController)
                    {
                        content.Add(new HUD.RichBox.RichBoxImage(player.input.ControllerFocus.Icon));
                        content.Add(new HUD.RichBox.RichBoxText(":"));
                        content.newLine();
                    }

                    content.newLine();
                    int tabSel = 0;

                    var tabs = new List<RichboxTabMember>((int)MenuTab.NUM);
                    for (int i = 0; i < Tabs.Length; ++i)
                    {
                        var text = new RichBoxText(LangLib.Tab(Tabs[i], out string description));
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

                        if (Tabs[i] == player.factionTab)
                        {
                            tabSel = i;
                        }
                    }

                    content.Add(new RichboxTabgroup(tabs, tabSel, player.factionTabClick, null, null));

                    switch (player.factionTab)
                    {
                        case MenuTab.Info:
                            infoTab();
                            break;

                        case MenuTab.Economy:
                            economyTab();
                            break;

                        case MenuTab.Automation:
                            player.automation.toMenu(content, fullDisplay);
                            break;

                        case MenuTab.Work:
                            faction.workTab(content);
                            break;

                        case MenuTab.Trade:
                            faction.tradeTab(content);
                            break;
                    }
                }
                //switch (player.hud.displays.CurrentMenuState)
                //{

                //    default:
                //        defaultMenu(player, fullDisplay, faction);
                //        break;
                //    case AutomationMenuState:
                //        player.automation.toMenu(content, fullDisplay);
                //        break;

                //}
                endRefresh(player.playerData.view.safeScreenArea.Position, true);
            }

            void toggleMenu()
            {
                content.Add(new RichBoxImage(player.input.ToggleHudDetail.Icon));
                content.Add(new RichBoxImage(SpriteName.pjMenuIcon));
            }

            void input(Input.IButtonMap button, string text)
            {
                content.Add(new RichBoxImage(button.Icon));
                content.Add(new RichBoxSpace());
                content.Add(new RichBoxText(text));

                content.Add(new RichBoxNewLine());
            }

            void flagTexture()
            {
                content.Add(new RichBoxTexture(faction.flagTexture, 1f, 0, 0.2f));
            }

            void gold()
            {
                content.Add(new RichBoxImage(SpriteName.rtsMoney));
                content.space();
                content.Add(new RichBoxText(DssRef.lang.ResourceType_Gold + ": " + TextLib.LargeNumber(faction.gold), negativeRed(faction.gold)));
                content.Add(new RichBoxNewLine());
            }

            void compressedGoldAndIncome()
            {
                content.Add(new RichBoxImage(SpriteName.rtsMoney));
                content.Add(new RichBoxText(TextLib.LargeNumber(faction.gold), negativeRed(faction.gold)));
                content.space();
                content.Add(new RichBoxImage(SpriteName.rtsIncome));
                content.Add(new RichBoxText(TextLib.LargeNumber(faction.MoneySecDiff()), negativeRed(faction.MoneySecDiff())));
                
            }

            void pauseButton()
            {
                if (DssRef.difficulty.allowPauseCommand)
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember>
                    {
                        new RichBoxImage(player.input.PauseGame.Icon),
                        new RichBoxText(Ref.isPaused? DssRef.lang.Input_ResumePaused : DssRef.lang.Input_Pause),
                    },
                    new RbAction(DssRef.state.pauseAction),
                    null);
                    button.setGroupSelectionColor(HudLib.RbSettings, Ref.isPaused);
                    content.Add(button);

                    content.Add(new RichBoxNewLine());
                }
            }

            void gameMenuButton()
            {
                content.Add(new RichboxButton(new List<AbsRichBoxMember>
                    {
                        new RichBoxImage(player.input.Menu.Icon),
                        new RichBoxText(DssRef.lang.GameMenu_Title),
                    },
                    new RbAction(DssRef.state.menuSystem.pauseMenu),
                    null));

                content.Add(new RichBoxNewLine());
            }



            void defaultMenu(Players.LocalPlayer player, bool fullDisplay, Faction faction)
            {
#if DEBUG
                //debugCultureFont();
#endif


                //if (player.hud.detailLevel == HudDetailLevel.Minimal)
                //{
                //    flagTexture();
                //    content.space();
                //    toggleMenu();
                //    content.space();
                //    compressedGoldAndIncome();
                //}
                //else
                //{
                    this.fullDisplay = fullDisplay;


                    content.Add(new RichBoxBeginTitle(2));
                    flagTexture();
                    content.Add(new RichBoxText(faction.PlayerName));
                    content.Add(new RichBoxNewLine());

                    if (fullDisplay)
                    {
                        gold();

                        content.Add(new RichBoxImage(SpriteName.rtsIncomeTime));
                        content.space();
                        content.Add(new RichBoxText(string.Format(DssRef.lang.Hud_TotalIncome, TextLib.LargeNumber(faction.MoneySecDiff())),
                                negativeRed(faction.MoneySecDiff())));
                        content.newLine();
                    }
                    else
                    {
                        compressedGoldAndIncome();
                        content.newLine();
                    }

                if (DssRef.state.IsSinglePlayer() && Ref.isPaused)
                {
                    pauseButton();
                }
                else
                {
                    string gameSpeed = string.Format(DssRef.lang.Hud_GameSpeedLabel, Ref.TargetGameTimeSpeed);//"Game speed: " + Ref.GameTimeSpeed.ToString() + "x";

                    if (DssRef.state.IsSinglePlayer())
                    {
                        input(player.input.GameSpeed, gameSpeed);
                        if (fullDisplay)
                        {
                            for (int i = 0; i < DssLib.GameSpeedOptions.Length; i++)
                            {
                                var button = new RichboxButton(
                                     new List<AbsRichBoxMember> { new RichBoxText(string.Format(DssRef.lang.Hud_XTimes, DssLib.GameSpeedOptions[i])) },
                                     new RbAction1Arg<int>(gameSpeedClick, DssLib.GameSpeedOptions[i]), null, true);
                                button.setGroupSelectionColor(HudLib.RbSettings, Ref.TargetGameTimeSpeed == DssLib.GameSpeedOptions[i]);
                                content.Add(button);
                                content.space(); 
                                
                            }
                            //content.newLine();
                        }
                    }
                    else
                    {
                        content.text(gameSpeed);
                        content.newLine();
                    }

                    if (fullDisplay && DssRef.state.IsSinglePlayer())
                    {
                        pauseButton();
                    }
                }
                if (fullDisplay && player.IsLocalHost())
                {
                    gameMenuButton();
                }

                if (fullDisplay)
                {
                    content.newParagraph();
                }
            }

            void infoTab()
            {
                FactionSize(faction, content, fullDisplay);


                if (fullDisplay)
                {
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

                    //content.icontext(SpriteName.WarsGroupIcon, string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_MercenaryMarket, player.mercenaryMarket.Int()));
                    //string command = "Command points: {0}";
                    //content.icontext(SpriteName.WarsCommandPoint, string.Format(command, player.commandPoints.ToString()));

                    content.Add(new RichBoxNewLine(true));

                    if (player.hud.detailLevel == HudDetailLevel.Extended)
                    {
                        content.text(string.Format(DssRef.lang.Hud_CityCount, TextLib.LargeNumber(faction.cities.Count)));
                        content.text(string.Format(DssRef.lang.Hud_ArmyCount, TextLib.LargeNumber(faction.armies.Count)));

                        content.ButtonDescription(player.input.NextCity, DssRef.lang.Input_NextCity);
                        content.ButtonDescription(player.input.NextArmy, DssRef.lang.Input_NextArmy);
                        content.ButtonDescription(player.input.NextBattle, DssRef.lang.Input_NextBattle);


                        content.newParagraph();
                    }

                    //if (Ref.isPaused && player.IsLocalHost())
                    //{
                    //    content.Button("Exit game", new RbAction(DssRef.state.exit), null, true);
                    //}
                    content.newLine();
                    toggleMenu();
                }
            }

            void economyTab()
            {                

                content.h2(DssRef.todoLang.UnitType_Cities);

                content.text(string.Format(DssRef.todoLang.Economy_ResourceProduction, DssRef.todoLang.Resource_TypeName_Food, faction.CityFoodProduction));
                content.space();
                HudLib.PerSecondInfo(player, content, true);

                content.text(string.Format(DssRef.todoLang.Economy_ResourceSpending, DssRef.todoLang.Resource_TypeName_Food, faction.CityFoodSpending));
                content.space();
                HudLib.PerSecondInfo(player, content, true);

                content.icontext(SpriteName.rtsIncomeTime, string.Format(DssRef.todoLang.Economy_TaxIncome, Convert.ToInt32(faction.citiesEconomy.tax())));
                content.space();
                HudLib.InfoButton(content, new RbAction(taxInfo));

                content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.todoLang.Economy_BlackMarketCostsForResource, DssRef.todoLang.Resource_TypeName_Food, Convert.ToInt32(faction.citiesEconomy.blackMarketCosts_Food)));
                content.space();
                HudLib.PerSecondInfo(player, content, true);

                content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.todoLang.Economy_GuardUpkeep, Convert.ToInt32(faction.citiesEconomy.cityGuardUpkeep)));
                content.space();
                HudLib.PerSecondInfo(player, content, false);

                if (DssLib.UseLocalTrading)
                {
                    content.icontext(SpriteName.rtsIncomeTime, string.Format(DssRef.todoLang.Economy_LocalCityTrade_Export, faction.CityTradeExport));
                    content.space();
                    HudLib.PerSecondInfo(player, content, false);

                    content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.todoLang.Economy_LocalCityTrade_Import, faction.CityTradeImport));
                    content.space();
                    HudLib.PerSecondInfo(player, content, false);
                }
                content.icontext(SpriteName.rtsIncomeTime, string.Format(DssRef.todoLang.Economy_SoldResources, faction.CitySoldResources));
                content.space();
                HudLib.PerSecondInfo(player, content, false);

                content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Building_NobleHouse, DssLib.NobleHouseUpkeep * faction.nobelHouseCount));
                content.space();
                HudLib.PerSecondInfo(player, content, false);
                //

                content.newParagraph();
                content.h2(DssRef.todoLang.UnitType_Armies);

                content.text(string.Format(DssRef.todoLang.Economy_ResourceSpending, DssRef.todoLang.Resource_TypeName_Food, faction.armyFoodUpkeep));
                content.space();
                HudLib.PerSecondInfo(player, content, false);

                content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.todoLang.Economy_ImportCostsForResource, DssRef.todoLang.Resource_TypeName_Food, Convert.ToInt32(faction.armyFoodImportCost)));
                content.space();
                HudLib.PerSecondInfo(player, content, true);

                content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.todoLang.Economy_BlackMarketCostsForResource, DssRef.todoLang.Resource_TypeName_Food, Convert.ToInt32(faction.armyFoodBlackMarketCost)));
                content.space();
                HudLib.PerSecondInfo(player, content, true);
            }

            void taxInfo()
            {
                RichBoxContent content = new RichBoxContent();
                content.text(string.Format(DssRef.todoLang.Economy_TaxDescription, DssConst.TaxPerWorker));
                content.newParagraph();
                content.text(DssRef.todoLang.Info_PerSecond);
                player.hud.tooltip.create(player, content, true);
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
        }



        public static void FactionSize(Faction faction, RichBoxContent content, bool fullDisplay)
        {
            if (fullDisplay)
            {
                 content.icontext(SpriteName.WarsWorker, DssRef.lang.ResourceType_Workers + ": " + TextLib.LargeNumber(faction.totalWorkForce));
                content.icontext(SpriteName.WarsStrengthIcon, string.Format(DssRef.lang.Hud_TotalStrengthRating, TextLib.LargeNumber(Convert.ToInt32(faction.militaryStrength))));
            }
            else
            {
                content.newLine();
                content.Add(new RichBoxImage(SpriteName.WarsWorker));
                content.space();
                content.Add(new RichBoxText(TextLib.LargeNumber(faction.totalWorkForce)));

                content.space(2);

                content.Add(new RichBoxImage(SpriteName.WarsStrengthIcon));
                content.space();
                content.Add(new RichBoxText(TextLib.LargeNumber(Convert.ToInt32(faction.militaryStrength))));
            }
            content.newLine();
        }

        void gameSpeedClick(int toSpeed)
        { 
            Ref.SetGameSpeed(toSpeed);
        }

        
    }
}
