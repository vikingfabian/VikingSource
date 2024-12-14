using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Display
{
    class ArmyMenu
    {
        //const string DisbandMenuState = "dis";
        const string DisbandAllMenuState = "disall";
        const string TradeMenuState = "trade";
        Players.LocalPlayer player;
        Army army;

        public static readonly List<MenuTab> Tabs = new List<MenuTab>() {
            MenuTab.Info, MenuTab.Divide, MenuTab.Disband, MenuTab.Tag };

        public ArmyMenu(Players.LocalPlayer player, Army army, RichBoxContent content)
        {
            this.player = player;
            this.army = army;

            content.newLine();
            content.Add(new RichBoxImage(SpriteName.rtsMoney));
            content.space();
            content.Add(new RichBoxText(DssRef.lang.ResourceType_Gold + ": " + TextLib.LargeNumber(army.gold), HudLib.NegativeRed(army.gold)));
            content.Add(new RichBoxNewLine());

            content.newLine();
            switch (player.hud.displays.CurrentMenuState)
            {

                default:
                    int tabSel = 0;

                    var tabs = new List<RichboxTabMember>((int)MenuTab.NUM);

                    List<MenuTab> availableTabs = player.AvailableArmyTabs();
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

                        if (availableTabs[i] == player.armyTab)
                        {
                            tabSel = i;
                        }
                    }

                    content.Add(new RichboxTabgroup(tabs, tabSel, player.armyTabClick, null, SoundLib.menutab, null, null));
                    content.newParagraph();
                    //content.newLine();
                    switch (player.armyTab)
                    {
                        case MenuTab.Info:
                            infoTab(content);
                            break;
                        case MenuTab.Divide:
                            divideTab(content);
                            break;
                        case MenuTab.Disband:
                            disbandTab(content);
                            break;
                        case MenuTab.Tag:
                            tagsToMenu(content);
                            break;


                    }


                    //var haltButton = new HUD.RichBox.RichboxButton(
                    //new List<AbsRichBoxMember>
                    //{
                    //    new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_Halt),
                    //},
                    //new RbAction(halt), null);
                    //haltButton.addShortCutButton(player.input.Stop, false);
                    //content.Add(haltButton);

                    content.newLine();

                    //if (player.tutorial == null)
                    //{
                    //var disbandButton = new HUD.RichBox.RichboxButton(
                    //new List<AbsRichBoxMember>
                    //{
                    //new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_Disband),
                    //},
                    //new RbAction1Arg<string>(player.hud.displays.SetMenuState, DisbandMenuState, SoundLib.menu),
                    //null);
                    //content.Add(disbandButton);

                    //List<GameObject.Army> tradeAbleArmies = new List<GameObject.Army>();
                    //DssRef.world.unitCollAreaGrid.collectArmies(player.faction, army.tilePos, 1,
                    //    tradeAbleArmies);

                    //for (int i = tradeAbleArmies.Count - 1; i >= 0; --i)
                    //{
                    //    if (WP.birdDistance(army, tradeAbleArmies[i]) > Army.MaxTradeDistance)
                    //    {
                    //        tradeAbleArmies.RemoveAt(i);
                    //    }
                    //}


                    //if (tradeAbleArmies.Count > 1)
                    //{
                    //    content.newLine();
                    //    var mergeAllButton = new HUD.RichBox.RichboxButton(
                    //        new List<AbsRichBoxMember>
                    //        {
                    //        new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_MergeAllArmies),
                    //        },
                    //        new RbAction1Arg<List<GameObject.Army>>(mergeAllArmies, tradeAbleArmies, SoundLib.menu), null);
                    //    content.Add(mergeAllButton);

                    //    foreach (var ta in tradeAbleArmies)
                    //    {
                    //        if (ta != army)
                    //        {
                    //            content.newLine();
                    //            var tradeButton = new HUD.RichBox.RichboxButton(
                    //                new List<AbsRichBoxMember>
                    //                {
                    //                new HUD.RichBox.RichBoxText(string.Format(DssRef.lang.ArmyOption_SendToX, ta.TypeName())),
                    //                },
                    //                new RbAction1Arg<Army>(startArmyTrade, ta, SoundLib.menu), null);
                    //            content.Add(tradeButton);
                    //        }
                    //    }
                    //}

                    //content.newLine();
                    //var splitButton = new HUD.RichBox.RichboxButton(
                    //    new List<AbsRichBoxMember>
                    //    {
                    //        new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_Divide),
                    //    },
                    //    new RbAction1Arg<Army>(startArmyTrade, null, SoundLib.menu), null);
                    //content.Add(splitButton);
                    //}
                    break;

                //case DisbandMenuState:
                //    {
                //        content.h2(DssRef.lang.ArmyOption_Disband).overrideColor = HudLib.TitleColor_Label;
                //        var status = army.Status().getTypeCounts(army.faction);

                //        foreach (var kv in status)
                //        {
                //            content.newLine();
                //            content.Add(new RichBoxImage(AllUnits.UnitFilterIcon(kv.Key)));
                //            content.Add(new RichBoxText(string.Format(DssRef.lang.ArmyOption_XGroupsOfType, kv.Value, LangLib.UnitFilterName(kv.Key))));//kv.Key.ToString() + " groups: " + kv.Value);
                //            content.newLine();
                //            content.Button(string.Format(DssRef.lang.ArmyOption_RemoveX, 1),//"Remove 1",
                //                new RbAction2Arg<UnitFilterType, int>(army.disbandSoldiersAction, kv.Key, 1, SoundLib.menu),
                //                null, true);

                //            content.space();

                //            content.Button(string.Format(DssRef.lang.ArmyOption_RemoveX, 5),//"Remove 5",
                //                new RbAction2Arg<UnitFilterType, int>(army.disbandSoldiersAction, kv.Key, 5, SoundLib.menu),
                //                null,
                //                kv.Value >= 5);

                //        }
                //        content.newParagraph();
                //        var allbutton = new HUD.RichBox.RichboxButton(
                //            new List<AbsRichBoxMember>
                //            {
                //        new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_DisbandAll),
                //            },
                //            new RbAction1Arg<string>(player.hud.displays.SetMenuState, DisbandAllMenuState, SoundLib.menu), 
                //            null);
                //        content.Add(allbutton);

                //        content.newParagraph();
                //    }
                //    break;

                case DisbandAllMenuState:
                    content.h1(DssRef.lang.ArmyOption_DisbandAll);
                    content.h2(Ref.langOpt.Hud_AreYouSure);
                    content.newLine();
                    var allbuttonyes = new HUD.RichBox.RichboxButton(
                        new List<AbsRichBoxMember>
                        {
                        new HUD.RichBox.RichBoxText(Ref.langOpt.Hud_Yes),
                        },
                        new RbAction(disbandAllYes, SoundLib.menu), 
                        null);
                    content.Add(allbuttonyes);
                    break;

                case TradeMenuState:
                    tradeArmyMenu(content);
                    break;
                    //    {
                    //        if (player.hud.displays.otherArmy == null)
                    //        {
                    //            content.h2(DssRef.lang.ArmyOption_SendToNewArmy).overrideColor = HudLib.TitleColor_Label;
                    //        }
                    //        else
                    //        {
                    //            content.h2(string.Format(DssRef.lang.ArmyOption_SendToX, player.hud.displays.otherArmy.TypeName())).overrideColor = HudLib.TitleColor_Label;
                    //        }

                    //        var status = army.Status().getTypeCounts(army.faction);
                    //        bool splitable = false;

                    //        foreach (var kv in status)
                    //        {
                    //            if (kv.Value > 1)
                    //            { 
                    //                splitable = true;
                    //                break;
                    //            }
                    //        }

                    //        content.newLine();

                    //        if (player.hud.displays.otherArmy == null)
                    //        {
                    //            var halfAndHalfbutton = new HUD.RichBox.RichboxButton(
                    //            new List<AbsRichBoxMember>
                    //            {
                    //                new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_DivideHalf),
                    //            },
                    //            new RbAction(splitArmyInHalf, SoundLib.menu), null);
                    //            halfAndHalfbutton.enabled = splitable;
                    //            content.Add(halfAndHalfbutton);
                    //        }
                    //        else
                    //        {
                    //            var allbutton = new HUD.RichBox.RichboxButton(
                    //            new List<AbsRichBoxMember>
                    //            {
                    //                new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_MergeArmies),
                    //            },
                    //            new RbAction(mergeArmies, SoundLib.menu), null);
                    //            content.Add(allbutton);
                    //        }

                    //        content.newParagraph();

                    //        foreach (var kv in status)
                    //        {
                    //            content.newLine();
                    //            content.Add(new RichBoxImage(AllUnits.UnitFilterIcon(kv.Key)));
                    //            content.Add(new RichBoxText(string.Format(DssRef.lang.ArmyOption_XGroupsOfType, kv.Value, LangLib.UnitFilterName(kv.Key))));//kv.Key.ToString() + " groups: " + kv.Value);
                    //            content.newLine();
                    //            content.Button(string.Format(DssRef.lang.ArmyOption_SendX, 1),//"Send 1",
                    //                new RbAction2Arg<UnitFilterType, int>(tradeSoldiersAction, kv.Key, 1, SoundLib.menu),
                    //                null, true);

                    //            content.space();

                    //            content.Button(string.Format(DssRef.lang.ArmyOption_SendX, 5),//"Send 5",
                    //                new RbAction2Arg<UnitFilterType, int>(tradeSoldiersAction, kv.Key, 5, SoundLib.menu),
                    //                null,
                    //                kv.Value >= 5);

                    //            content.space();

                    //            content.Button(DssRef.lang.ArmyOption_SendAll,//"Send All",
                    //               new RbAction2Arg<UnitFilterType, int>(tradeSoldiersAction, kv.Key, kv.Value, SoundLib.menu),
                    //               null, true);

                    //        }

                    //        content.newParagraph();

                    //    }
                    //    break;
            }
            
        }

        void infoTab(RichBoxContent content)
        {
            army.basicInfoHud(new ObjectHudArgs(null, content, player, true));

            var haltButton = new HUD.RichBox.RichboxButton(
                        new List<AbsRichBoxMember>
                        {
                        new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_Halt),
                        },
                        new RbAction(halt), null);
            haltButton.addShortCutButton(player.input.Stop, false);
            content.Add(haltButton);
        }

        void divideTab(RichBoxContent content)
        {
            List<GameObject.Army> tradeAbleArmies = new List<GameObject.Army>();
            DssRef.world.unitCollAreaGrid.collectArmies(player.faction, army.tilePos, 1,
                tradeAbleArmies);

            for (int i = tradeAbleArmies.Count - 1; i >= 0; --i)
            {
                if (tradeAbleArmies[i] == army ||
                    WP.birdDistance(army, tradeAbleArmies[i]) > Army.MaxTradeDistance)
                {
                    tradeAbleArmies.RemoveAt(i);
                }
            }

            if (!tradeAbleArmies.Contains(player.hud.displays.otherArmy))
            {
                player.hud.displays.otherArmy = null;
            }

            var status = army.Status().getTypeCounts(army.faction);
            bool splitable = false;

            foreach (var kv in status)
            {
                if (kv.Value > 1)
                {
                    splitable = true;
                    break;
                }
            }
            
            var mergeAllButton = new HUD.RichBox.RichboxButton(
                    new List<AbsRichBoxMember>
                    {
                        new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_MergeAllArmies),
                    },
                    new RbAction1Arg<List<GameObject.Army>>(mergeAllArmies, tradeAbleArmies, SoundLib.menu), null);
            mergeAllButton.enabled = tradeAbleArmies.Count > 0;
            content.Add(mergeAllButton);

            content.newLine();

            var halfAndHalfbutton = new HUD.RichBox.RichboxButton(
                new List<AbsRichBoxMember>
                {
                                new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_DivideHalf),
                },
                new RbAction(splitArmyInHalf, SoundLib.menu), null);
            halfAndHalfbutton.enabled = splitable;
            content.Add(halfAndHalfbutton);

            content.newParagraph();

            ////TRADE
            //{
                

            //    if (tradeAbleArmies.Count > 0)
            //    {
            //        content.h2(string.Format(DssRef.lang.ArmyOption_SendToX, player.hud.displays.otherArmy.TypeName())).overrideColor = HudLib.TitleColor_Label;
            //        content.newLine();
                    

            //        foreach (var ta in tradeAbleArmies)
            //        {                        
            //            content.newLine();
            //            var tradeButton = new HUD.RichBox.RichboxButton(
            //                new List<AbsRichBoxMember>
            //                {
            //                    new HUD.RichBox.RichBoxText(string.Format(DssRef.lang.ArmyOption_SendToX, ta.TypeName())),
            //                },
            //                new RbAction1Arg<Army>(startArmyTrade, ta, SoundLib.menu), null);
            //            content.Add(tradeButton);                        
            //        }

            //        content.newLine();
            //        content.Add(new RichBoxSeperationLine());
            //    }

            //}

            //LIST SEND OPTIONS
            HudLib.Label(content, string.Format( DssRef.lang.ArmyOption_SendToX, string.Empty) );
            content.newLine();
            var newArmyButton = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText("New army")},
                new RbAction1Arg<Army>(selectArmyTrade, null, SoundLib.menutab));
            newArmyButton.setGroupSelectionColor(HudLib.RbSettings, player.hud.displays.otherArmy == null);
            content.Add(newArmyButton);
            

            foreach (var otherArmy in tradeAbleArmies)
            {
                content.space();

                var buttonContent = new RichBoxContent();
                otherArmy.tagToHud(buttonContent);
                if (buttonContent.Count > 0)
                {
                    buttonContent.space();
                }
                buttonContent.Add(new RichBoxText(otherArmy.TypeName()));

                var button = new RichboxButton(buttonContent,
                new RbAction1Arg<Army>(selectArmyTrade, otherArmy, SoundLib.menutab));
                button.setGroupSelectionColor(HudLib.RbSettings, player.hud.displays.otherArmy == otherArmy);
                content.Add(button);
            }

            //SPLIT


                //var splitButton = new HUD.RichBox.RichboxButton(
                //    new List<AbsRichBoxMember>
                //    {
                //            new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_Divide),
                //    },
                //    new RbAction1Arg<Army>(startArmyTrade, null, SoundLib.menu), null);
                //content.Add(splitButton);
                //content.h2(DssRef.lang.ArmyOption_SendToNewArmy).overrideColor = HudLib.TitleColor_Label;





            content.newLine();

            foreach (var kv in status)
            {
                content.newLine();
                content.Add(new RichBoxImage(AllUnits.UnitFilterIcon(kv.Key)));
                content.Add(new RichBoxText(string.Format(DssRef.lang.ArmyOption_XGroupsOfType, kv.Value, LangLib.UnitFilterName(kv.Key))));//kv.Key.ToString() + " groups: " + kv.Value);
                content.newLine();
                content.Button(string.Format(DssRef.lang.ArmyOption_SendX, 1),//"Send 1",
                    new RbAction2Arg<UnitFilterType, int>(tradeSoldiersAction, kv.Key, 1, SoundLib.menu),
                    null, true);

                content.space();

                content.Button(string.Format(DssRef.lang.ArmyOption_SendX, 5),//"Send 5",
                    new RbAction2Arg<UnitFilterType, int>(tradeSoldiersAction, kv.Key, 5, SoundLib.menu),
                    null,
                    kv.Value >= 5);

                content.space();

                content.Button(DssRef.lang.ArmyOption_SendAll,//"Send All",
                    new RbAction2Arg<UnitFilterType, int>(tradeSoldiersAction, kv.Key, kv.Value, SoundLib.menu),
                    null, true);

            }
        }
        

        

        //void divideArmyMenu(RichBoxContent content)
        //{
            
        //}

        void tradeArmyMenu(RichBoxContent content)
        {
            if (player.hud.displays.otherArmy == null)
            {
                
            }
            else
            {
                
            }

            

            if (player.hud.displays.otherArmy == null)
            {
                
            }
            else
            {
                var allbutton = new HUD.RichBox.RichboxButton(
                new List<AbsRichBoxMember>
                {
                                new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_MergeArmies),
                },
                new RbAction(mergeArmies, SoundLib.menu), null);
                content.Add(allbutton);
            }

            
        }


        void disbandTab(RichBoxContent content)
        {
            content.h2(DssRef.lang.ArmyOption_Disband).overrideColor = HudLib.TitleColor_Label;
            var status = army.Status().getTypeCounts(army.faction);

            foreach (var kv in status)
            {
                content.newLine();
                content.Add(new RichBoxImage(AllUnits.UnitFilterIcon(kv.Key)));
                content.Add(new RichBoxText(string.Format(DssRef.lang.ArmyOption_XGroupsOfType, kv.Value, LangLib.UnitFilterName(kv.Key))));//kv.Key.ToString() + " groups: " + kv.Value);
                content.newLine();
                content.Button(string.Format(DssRef.lang.ArmyOption_RemoveX, 1),//"Remove 1",
                    new RbAction2Arg<UnitFilterType, int>(army.disbandSoldiersAction, kv.Key, 1, SoundLib.menu),
                    null, true);

                content.space();

                content.Button(string.Format(DssRef.lang.ArmyOption_RemoveX, 5),//"Remove 5",
                    new RbAction2Arg<UnitFilterType, int>(army.disbandSoldiersAction, kv.Key, 5, SoundLib.menu),
                    null,
                    kv.Value >= 5);

            }
            content.newParagraph();
            var allbutton = new HUD.RichBox.RichboxButton(
                new List<AbsRichBoxMember>
                {
                        new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_DisbandAll),
                },
                new RbAction1Arg<string>(player.hud.displays.SetMenuState, DisbandAllMenuState, SoundLib.menu),
                null);
            content.Add(allbutton);
        }
        public void tagsToMenu(RichBoxContent content)
        {
            content.newLine();
            content.Add(new RichboxCheckbox(new List<AbsRichBoxMember> { new RichBoxText(DssRef.lang.Tag_ViewOnMap) }, player.ArmyTagsOnMapProperty));
            content.newParagraph();

            for (CityTagBack back = CityTagBack.NONE; back < CityTagBack.NUM; back++)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember> {
                    new RichBoxImage(Data.CityTag.BackSprite(back))
                }, new RbAction1Arg<CityTagBack>((CityTagBack back) => { army.tagBack = back; }, back));
                button.setGroupSelectionColor(HudLib.RbSettings, back == army.tagBack);
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

            if (army.tagBack != CityTagBack.NONE)
            {
                content.newParagraph();
                for (ArmyTagArt art = ArmyTagArt.None; art < ArmyTagArt.NUM; art++)
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember> {
                    new RichBoxImage(Data.CityTag.ArtSprite(art))
                    }, new RbAction1Arg<ArmyTagArt>((ArmyTagArt art) => { army.tagArt = art; }, art));
                    button.setGroupSelectionColor(HudLib.RbSettings, art == army.tagArt);
                    content.Add(button);
                    content.space();
                }
            }
        }
        void splitArmyInHalf()
        {
            player.hud.displays.otherArmy = null;

            var status = army.Status().getTypeCounts(army.faction);
            foreach (var kv in status)
            {
                if (kv.Value > 1)
                {
                    tradeSoldiersAction(kv.Key, kv.Value/2);
                }
            }

            player.hud.displays.menuBack();
        }

        void mergeArmies()
        {
            army.mergeArmies(player.hud.displays.otherArmy);
        }

        void mergeAllArmies(List<GameObject.Army> tradeAbleArmies)
        {
            

            if (tradeAbleArmies.Count >= 1)
            {
                List<GameObject.Army> all = new List<Army>(tradeAbleArmies.Count + 1);
                all.Add(army);
                all.AddRange(tradeAbleArmies);

                GameObject.Army largest = null;

                foreach (var m in all)
                {
                    if (largest == null || m.strengthValue > largest.strengthValue)
                    { 
                        largest = m;
                    }
                }

                foreach (var m in all)
                {
                    if (m != largest)
                    {
                        m.mergeArmies(largest);
                    }
                }
            }
        }

        void tradeSoldiersAction(UnitFilterType type, int count)
        {
            army.tradeSoldiersAction(ref player.hud.displays.otherArmy, type, count);            
        }

        void startArmyTrade(Army toarmy)
        {
            player.hud.displays.otherArmy = toarmy;
            player.hud.displays.SetMenuState(TradeMenuState);
        }

        void selectArmyTrade(Army toarmy)
        {
            player.hud.displays.otherArmy = toarmy;
        }

        void halt()
        {
            SoundLib.orderstop.Play();
            army.haltMovement();
        }

        void disbandAllYes()
        {
            army.disbandArmyAction();
        }
    }
}
