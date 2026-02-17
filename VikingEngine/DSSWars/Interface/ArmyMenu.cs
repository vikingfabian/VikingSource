using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Command;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.ToGG.HeroQuest.Players.Ai;
using VikingEngine.ToGG.MoonFall;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.DSSWars.Interface
{
    class ArmyMenu
    {
        const string DisbandAllMenuState = "disall";
        const string TradeMenuState = "trade";
        Players.LocalPlayer player;
        Army army;
        ArmyCollection objectCollection;

        public static readonly List<MenuTab> Tabs = new List<MenuTab>() {
            MenuTab.Info, MenuTab.Divide, MenuTab.Disband, MenuTab.Tag };

        public ArmyMenu(Players.LocalPlayer player, ArmyCollection objectCollection, RichBoxContent content)
        {
            this.player = player;
            this.objectCollection = objectCollection;

            if (objectCollection.objects.Count >= 1)
            {
                switch (player.hud.objMenu.menu.CurrentMenuState)
                {
                    default:
                        army = objectCollection.objects[0].army;
                        List<AbsArmy> tradeAbleArmies = new List<AbsArmy>(objectCollection.objects.Count - 1);
                        for (int i = 1; i < objectCollection.objects.Count; i++)
                        {
                            tradeAbleArmies.Add(objectCollection.objects[i].army);
                        }
                        FilterTradeAbleArmies(army, tradeAbleArmies);

                        
                        mergeAllButton(content, tradeAbleArmies);

                        content.newParagraph();
                        disbandAllButton(content);
                        break;

                    case DisbandAllMenuState:
                        disbandAllDialogue(content);
                        break;
                }
            }
        }

        public ArmyMenu(Players.LocalPlayer player, Army army, RichBoxContent content)
        {
            this.player = player;
            this.army = army;

            if (!DssRef.storage.gameRuleset.centralGold)
            {
                content.newLine();
                content.Add(new RbImage(SpriteName.rtsMoney));
                content.space();
                content.Add(new RbText(DssRef.lang.ResourceType_Gold + ": " + TextLib.LargeNumber(army.money.GetGold()), HudLib.NegativeRed(army.money.GetGold())));
                content.Add(new RbNewLine());
            }

            content.newLine();
            switch (player.hud.objMenu.menu.CurrentMenuState)
            {

                default:
                    int tabSel = 0;

                    var tabs = new List<ArtTabMember>((int)MenuTab.NUM_NONE);

                    List<MenuTab> availableTabs = player.AvailableArmyTabs();
                    for (int i = 0; i < availableTabs.Count; ++i)
                    {
                        var text = new RbText(LangLib.Tab(availableTabs[i], out string description, out _));
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

                        if (availableTabs[i] == player.armyTab)
                        {
                            tabSel = i;
                        }
                    }

                    bool viewControllerTabs = player.gameControls.tabFocusColor(Players.PlayerControls.ControllerTabFocus.ArmyMenu, out Color focusColor);
                    if (viewControllerTabs && player.gameControls.input.Controller_TabLeft.IsActive)
                    {
                        content.Add(new RbImage(player.gameControls.input.Controller_TabLeft.Icon) { color = focusColor });
                        content.space(0.5f);
                    }
                    var tabGroup = new ArtTabgroup(tabs, tabSel, player.armyTabClick);
                    if (viewControllerTabs && player.gameControls.input.Controller_TabRight.IsActive)
                    {
                        tabGroup.endAttach = new List<AbsRichBoxMember> { new RbSpace(0.5f), new RbImage(player.gameControls.input.Controller_TabRight.Icon) { color = focusColor } };
                    }

                    content.Add(tabGroup);
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

                    content.newLine();

                    break;

               

                case DisbandAllMenuState:
                    disbandAllDialogue(content);
                    break;

                case TradeMenuState:
                    tradeArmyMenu(content);
                    break;
                  
            }
            
        }

        void disbandAllDialogue(RichBoxContent content)
        { 
            content.h1(DssRef.lang.ArmyOption_DisbandAll, HudLib.TitleColor_Head);
                    content.h2(Ref.langOpt.Hud_AreYouSure);
                    content.newLine();
            
            var buttonyes = new ArtButton( RbButtonStyle.Primary,
                        new List<AbsRichBoxMember>
                        {
                        new HUD.RichBox.RbText(Ref.langOpt.Hud_Yes),
                        },
                        new RbAction(disbandAllYes, RbSoundType.Default), 
                        null);
                    content.Add(buttonyes);

            var buttonno = new ArtButton(RbButtonStyle.Secondary,
                       new List<AbsRichBoxMember>
                       {
                        new HUD.RichBox.RbText(Ref.langOpt.Hud_Cancel),
                       },
                       new RbAction(player.hud.objMenu.menu.menuBack, RbSoundType.Default),
                       null);
            content.Add(buttonno);
        }


        void infoTab(RichBoxContent content)
        {
            army.basicInfoHud(new ObjectHudArgs( content, player, true));

            content.newLine();
            ColumnWidth(content, army);

            content.newLine();
            if (army.HasSettler(out var unit))
            {
                settlerButton(player, content, unit);
            }

            var haltButton = new ArtButton( RbButtonStyle.Primary,
                        new List<AbsRichBoxMember>
                        {
                        new HUD.RichBox.RbText(DssRef.lang.ArmyOption_Halt),
                        },
                        new RbAction(halt), null);
            
            content.Add(haltButton);

            
        }

        public static void ColumnWidth(RichBoxContent content, AbsArmy army)
        {
            HudLib.Label(content, DssRef.lang.ArmyStructure_ColumnWidth);
            content.newLine();
            for (int w = Army.MinColumnWidth; w <= Army.MaxColumnWidth; w += 2)
            {
                var button = new ArtOption(w == army.armyColumnWidth,
                    new List<AbsRichBoxMember> { new RbText(w.ToString()) },
                    new RbAction1Arg<int>(army.armyColumnWidthClick, w, RbSoundType.Option));

                content.Add(button);
            }
        }

        public static void settlerButton(LocalPlayer player, RichBoxContent content, SoldierGroup unit)
        {
            if (DssRef.world.tileGrid.TryGet(unit.tilePos, out var tile))
            {
                bool unclaimedLand = tile.City().cityType == CityType.UnClaimed;

                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>{
                        new RbText(DssRef.lang.Action_PlaceSettlement) }, new RbAction(() =>
                        {
                            if (!unit.isDeleted)
                            {
                                new SettlerCommandTarget(player, unit);
                            }
                        }), null, unclaimedLand));

                content.newLine();
            }
        }

        void divideTab(RichBoxContent content)
        {
            List<GameObject.AbsArmy> tradeAbleArmies = new List<GameObject.AbsArmy>();
            DssRef.world.unitCollAreaGrid.collectArmies(player.faction.myIndex, army.tilePos, 1,
                tradeAbleArmies);

            //for (int i = tradeAbleArmies.Count - 1; i >= 0; --i)
            //{
            //    if (tradeAbleArmies[i] == army ||
            //        WP.birdDistance(army, tradeAbleArmies[i]) > Army.MaxTradeDistance)
            //    {
            //        tradeAbleArmies.RemoveAt(i);
            //    }
            //}
            FilterTradeAbleArmies(army, tradeAbleArmies);

            if (!tradeAbleArmies.Contains(player.hud.objMenu.otherArmy))
            {
                player.hud.objMenu.otherArmy = null;
            }

            var status = army.Status().getTypeCounts(army.GetFaction());
            bool splitable = false;

            foreach (var kv in status)
            {
                if (kv.Value > 1)
                {
                    splitable = true;
                    break;
                }
            }

            mergeAllButton(content, tradeAbleArmies);

            content.newLine();

            var halfAndHalfbutton = new ArtButton(RbButtonStyle.Primary,
                new List<AbsRichBoxMember>
                {
                                new HUD.RichBox.RbText(DssRef.lang.ArmyOption_DivideHalf),
                },
                new RbAction(splitArmyInHalf, RbSoundType.Default), null);
            halfAndHalfbutton.enabled = splitable;
            content.Add(halfAndHalfbutton);

            content.newParagraph();

            //LIST SEND OPTIONS
            HudLib.Label(content, string.Format(DssRef.lang.ArmyOption_SendToX, string.Empty));
            content.newLine();
            var newArmyButton = new ArtOption(player.hud.objMenu.otherArmy == null, new List<AbsRichBoxMember> { new RbText(DssRef.lang.ArmyOption_NewArmy) },
                new RbAction1Arg<Army>(selectArmyTrade, null, RbSoundType.Option));
            //newArmyButton.setGroupSelectionColor(HudLib.RbSettings, player.hud.objMenu.otherArmy == null);
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
                buttonContent.Add(new RbText(otherArmy.TypeName()));

                var button = new ArtOption(player.hud.objMenu.otherArmy == otherArmy, buttonContent,
                new RbAction1Arg<AbsArmy>(selectArmyTrade, otherArmy, RbSoundType.Option));
                //button.setGroupSelectionColor(HudLib.RbSettings, player.hud.objMenu.otherArmy == otherArmy);
                content.Add(button);
            }



            content.newLine();

            foreach (var kv in status)
            {
                content.newLine();
                content.Add(new RbImage(AllUnits.UnitFilterIcon(kv.Key)));
                content.Add(new RbText(string.Format(DssRef.lang.ArmyOption_XGroupsOfType, kv.Value, LangLib.UnitFilterName(kv.Key))));//kv.Key.ToString() + " groups: " + kv.Value);
                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.lang.ArmyOption_SendX, 1)) },//"Send 1",
                    new RbAction2Arg<UnitFilterType, int>(tradeSoldiersAction, kv.Key, 1, RbSoundType.Default),
                    null, true));

                content.space();

                content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.lang.ArmyOption_SendX, 5)) },//"Send 5",
                    new RbAction2Arg<UnitFilterType, int>(tradeSoldiersAction, kv.Key, 5, RbSoundType.Default),
                    null,
                    kv.Value >= 5));

                content.space();

                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.ArmyOption_SendAll) },//"Send All",
                    new RbAction2Arg<UnitFilterType, int>(tradeSoldiersAction, kv.Key, kv.Value, RbSoundType.Default),
                    null, true));

            }
        }

        private void mergeAllButton(RichBoxContent content, List<AbsArmy> tradeAbleArmies)
        {
            var mergeAllButton = new ArtButton(RbButtonStyle.Primary,
                    new List<AbsRichBoxMember>
                    {
                        new HUD.RichBox.RbText(DssRef.lang.ArmyOption_MergeAllArmies),
                    },
                    new RbAction1Arg<List<GameObject.AbsArmy>>(mergeAllArmies, tradeAbleArmies, RbSoundType.Default), null);
            mergeAllButton.enabled = tradeAbleArmies.Count > 0;
            content.Add(mergeAllButton);
        }

        public static void FilterTradeAbleArmies(Army army,List<AbsArmy> tradeAbleArmies)
        {
            for (int i = tradeAbleArmies.Count - 1; i >= 0; --i)
            {
                if (tradeAbleArmies[i] == army ||
                    WP.birdDistance(army, tradeAbleArmies[i]) > Army.MaxTradeDistance)
                {
                    tradeAbleArmies.RemoveAt(i);
                }
            }
        }
        

        //void divideArmyMenu(RichBoxContent content)
        //{
            
        //}

        void tradeArmyMenu(RichBoxContent content)
        {
            if (player.hud.objMenu.otherArmy == null)
            {
                
            }
            else
            {
                
            }

            if (player.hud.objMenu.otherArmy == null)
            {
                
            }
            else
            {
                var allbutton = new ArtButton( RbButtonStyle.Primary,
                new List<AbsRichBoxMember>
                {
                                new HUD.RichBox.RbText(DssRef.lang.ArmyOption_MergeArmies),
                },
                new RbAction(mergeArmies, RbSoundType.Default), null);
                content.Add(allbutton);
            }

            
        }


        void disbandTab(RichBoxContent content)
        {
            content.h2(DssRef.lang.ArmyOption_Disband).overrideColor = HudLib.TitleColor_Label;
            var status = army.Status().getTypeCounts(army.GetFaction());

            foreach (var kv in status)
            {
                content.newLine();
                content.Add(new RbImage(AllUnits.UnitFilterIcon(kv.Key)));
                content.Add(new RbText(string.Format(DssRef.lang.ArmyOption_XGroupsOfType, kv.Value, LangLib.UnitFilterName(kv.Key))));//kv.Key.ToString() + " groups: " + kv.Value);
                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.lang.ArmyOption_RemoveX, 1)) },//"Remove 1",
                    new RbAction2Arg<UnitFilterType, int>(army.disbandSoldiersAction, kv.Key, 1, RbSoundType.Default),
                    null, true));

                content.space();

                content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.lang.ArmyOption_RemoveX, 5)) },//"Remove 5",
                    new RbAction2Arg<UnitFilterType, int>(army.disbandSoldiersAction, kv.Key, 5, RbSoundType.Default),
                    null,
                    kv.Value >= 5));

            }
            content.newParagraph();
            disbandAllButton(content);
        }

        private void disbandAllButton(RichBoxContent content)
        {
            var allbutton = new ArtButton(RbButtonStyle.Primary,
                            new List<AbsRichBoxMember>
                            {
                        new HUD.RichBox.RbText(DssRef.lang.ArmyOption_DisbandAll),
                            },
                            new RbAction2Arg<string, StackOption>(player.hud.objMenu.menu.OpenMenu, DisbandAllMenuState, StackOption.Stack, RbSoundType.Default),
                            null);
            content.Add(allbutton);
        }

        public void tagsToMenu(RichBoxContent content)
        {
            HudLib.Label(content, DssRef.lang.ObjectUi_ViewOnMap + string.Format(" ({0})", DssRef.lang.Hud_AllArmies));
            content.newLine();
            player.armyHudSettings.toHud(content, false, player.profile.casualControls);
            //content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Tag_ViewOnMap) }, player.ArmyTagsOnMapProperty));
            content.newParagraph();

            for (CityTagBack back = CityTagBack.NONE; back < CityTagBack.NUM; back++)
            {
                var button = new ArtToggle(back == army.tagBack, new List<AbsRichBoxMember> {
                    new RbImage(Data.CityTag.BackSprite(back))
                }, new RbAction1Arg<CityTagBack>((CityTagBack back) => { army.tagBack = back; }, back, back == CityTagBack.NONE? RbSoundType.Deselect : RbSoundType.Option));
                //button.setGroupSelectionColor(HudLib.RbSettings, back == army.tagBack);
                content.Add(button);

                if (back == CityTagBack.NONE)
                {
                    content.newLine();
                }
                //else
                //{
                //    content.space();
                //}
            }

            if (army.tagBack != CityTagBack.NONE)
            {
                content.newParagraph();
                for (ArmyTagArt art = ArmyTagArt.None; art < ArmyTagArt.NUM; art++)
                {
                    var button = new ArtToggle(art == army.tagArt, new List<AbsRichBoxMember> {
                    new RbImage(Data.CityTag.ArtSprite(art))
                    }, new RbAction1Arg<ArmyTagArt>((ArmyTagArt art) => { army.tagArt = art; }, art, art == ArmyTagArt.None ? RbSoundType.Deselect : RbSoundType.Option));
                    //button.setGroupSelectionColor(HudLib.RbSettings, art == army.tagArt);
                    content.Add(button);
                    //content.space();
                }
            }
        }
        void splitArmyInHalf()
        {
            player.hud.objMenu.otherArmy = null;

            var status = army.Status().getTypeCounts(army.GetFaction());
            foreach (var kv in status)
            {
                if (kv.Value > 1)
                {
                    tradeSoldiersAction(kv.Key, kv.Value/2);
                }
            }

            player.hud.objMenu.menu.menuBack();
        }

        void mergeArmies()
        {
            army.mergeArmies(player.hud.objMenu.otherArmy);
        }

        void mergeAllArmies(List<GameObject.AbsArmy> tradeAbleArmies)
        {
            

            if (tradeAbleArmies.Count >= 1)
            {
                List<GameObject.AbsArmy> all = new List<AbsArmy>(tradeAbleArmies.Count + 1);
                all.Add(army);
                all.AddRange(tradeAbleArmies);

                GameObject.AbsArmy largest = null;

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
                        m.GetArmy().mergeArmies(largest);
                    }
                }
            }
        }

        void tradeSoldiersAction(UnitFilterType type, int count)
        {
            army.tradeSoldiersAction(ref player.hud.objMenu.otherArmy, type, count);            
        }

        void startArmyTrade(Army toarmy)
        {
            player.hud.objMenu.otherArmy = toarmy;
            player.hud.objMenu.menu.OpenMenu(TradeMenuState, StackOption.Stack);
        }

        void selectArmyTrade(AbsArmy toarmy)
        {
            player.hud.objMenu.otherArmy = toarmy;
        }

        void halt()
        {
            SoundLib.orderstop.Play();
            army.haltMovement();
        }

        void disbandAllYes()
        {
            objectCollection?.disbandArmyAction();
            army?.disbandArmyAction();
        }
    }
}
