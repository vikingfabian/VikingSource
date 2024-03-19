using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Display
{
    class HeadDisplay : RichboxGuiPart
    {        
        public bool fullDisplay = true;
        public const string AutomationMenuState = "auto";

        public HeadDisplay(RichboxGui gui)
            :base(gui)
        {
        }

        public void refreshUpdate(Players.LocalPlayer player, bool fullDisplay, bool refresh, Faction faction)
        {
            if (bg.Visible && refresh)
            {
                beginRefresh();

                switch (player.hud.displays.CurrentMenuState)
                {

                    default:
                        defaultMenu(player, fullDisplay, faction);
                        break;
                    case AutomationMenuState:
                        player.automation.toMenu(content, fullDisplay);
                        break;
                }
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
                content.Add(new RichBoxText("Gold: " + TextLib.LargeNumber(faction.gold), negativeRed(faction.gold)));
                content.Add(new RichBoxNewLine());
            }

            void compressedGoldAndIncome()
            {
                content.Add(new RichBoxImage(SpriteName.rtsMoney));
                content.Add(new RichBoxText(TextLib.LargeNumber(faction.gold), negativeRed(faction.gold)));
                content.space();
                content.Add(new RichBoxImage(SpriteName.rtsIncome));
                content.Add(new RichBoxText(TextLib.LargeNumber(faction.NetIncome()), negativeRed(faction.NetIncome())));
                
            }

            void pauseButton()
            {
                if (DssRef.storage.allowPauseCommand)
                {
                    content.Add(new RichboxButton(new List<AbsRichBoxMember>
                    {
                        new RichBoxImage(player.input.PauseGame.Icon),
                        new RichBoxText(Ref.isPaused? "Resume" : "Pause"),
                    },
                        new RbAction(DssRef.state.pauseAction),
                        null));

                    content.Add(new RichBoxNewLine());
                }
            }

            

            void defaultMenu(Players.LocalPlayer player, bool fullDisplay, Faction faction)
            {
                if (player.hud.detailLevel == HudDetailLevel.Minimal)
                {
                    flagTexture();
                    content.space();
                    toggleMenu();
                    content.space();
                    compressedGoldAndIncome();
                }
                else
                {
                    this.fullDisplay = fullDisplay;


                    content.Add(new RichBoxBeginTitle(2));
                    flagTexture();
                    content.Add(new RichBoxText(faction.PlayerName));
                    content.Add(new RichBoxNewLine());

                    if (fullDisplay)
                    {
                        gold();

                        const string TotalIncomeText = "Total income/second: ";
                        content.Add(new RichBoxImage(SpriteName.rtsIncomeTime));
                        content.space();
                        content.Add(new RichBoxText(TotalIncomeText + TextLib.LargeNumber(faction.NetIncome()),
                                negativeRed(faction.NetIncome())));
                        content.newLine();
                    }
                    else
                    {
                        compressedGoldAndIncome();
                        content.newLine();
                    }

                    if (player.IsLocalHost() && Ref.isPaused)
                    {
                        pauseButton();
                    }
                    else
                    {
                        string gameSpeed = "Game speed: " + Ref.GameTimeSpeed.ToString() + "x";

                        if (DssRef.state.IsSinglePlayer())
                        {
                            input(player.input.GameSpeed, gameSpeed);
                            if (fullDisplay)
                            {
                                for (int i = 0; i < DssLib.GameSpeedOptions.Length; i++)
                                {
                                    content.Add(new RichboxButton(
                                        new List<AbsRichBoxMember> { new RichBoxText("x" + DssLib.GameSpeedOptions[i].ToString()) },
                                        new RbAction1Arg<int>(gameSpeedClick, DssLib.GameSpeedOptions[i]), null, true));
                                    content.space();
                                }
                                //content.newLine();
                            }
                        }

                        if (fullDisplay && player.IsLocalHost())
                        {
                            pauseButton();
                        }
                    }


                    if (fullDisplay)
                    {
                        content.newParagraph();
                        content.Add(new RichBoxNewLine());
                        content.Add(new RichBoxImage(SpriteName.rtsIncomeTime));
                        content.space();
                        content.Add(new RichBoxText("Workforce Income: " + TextLib.LargeNumber(faction.cityIncome)));

                        content.Add(new RichBoxNewLine());
                        content.Add(new RichBoxImage(SpriteName.rtsUpkeepTime));
                        content.space();
                        content.Add(new RichBoxText("Army upkeep: " + TextLib.LargeNumber(faction.armyUpkeep)));

                        content.newLine();
                        var automationButton = new HUD.RichBox.RichboxButton(
                            new List<AbsRichBoxMember>
                            {
                                new RichBoxImage(player.input.AutomationSetting.Icon),
                                new RichBoxImage(SpriteName.MenuPixelIconSettings),
                                new HUD.RichBox.RichBoxText("Automation"),
                            },
                            new RbAction1Arg<string>(player.hud.displays.SetMenuState, AutomationMenuState, SoundLib.menu),
                            null);
                        content.Add(automationButton);
                        //content.Button(SpriteName.MenuPixelIconSettings, "Automation", new RbAction(DssRef.state.exit), null, true);

                        string diplomacy = "Diplomatic points: {0}";
                        content.icontext(SpriteName.WarsDiplomaticPoint, string.Format(diplomacy, player.diplomaticPoints.ToString()));
                        string command = "Command points: {0}";
                        content.icontext(SpriteName.WarsCommandPoint, string.Format(command, player.commandPoints.ToString()));

                        content.Add(new RichBoxNewLine(true));

                        if (player.hud.detailLevel == HudDetailLevel.Extended)
                        {
                            content.text("City count: " + TextLib.LargeNumber(faction.cities.Count));
                            content.text("Army count: " + TextLib.LargeNumber(faction.armies.Count));
                            content.newParagraph();
                        }

                        if (Ref.isPaused && player.IsLocalHost())
                        {
                            content.Button("Exit game", new RbAction(DssRef.state.exit), null, true);
                        }
                        content.newLine();
                        toggleMenu();
                    }

                }
            }
        }

        void gameSpeedClick(int toSpeed)
        { 
            Ref.GameTimeSpeed= toSpeed;
        }
    }
}
