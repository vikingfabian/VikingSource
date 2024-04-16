using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Display
{
    class ArmyMenu
    {
        const string DisbandMenuState = "dis";
        const string DisbandAllMenuState = "disall";
        const string TradeMenuState = "trade";
        Players.LocalPlayer player;
        Army army;        

        public ArmyMenu(Players.LocalPlayer player, Army army, RichBoxContent content)
        {
            this.player = player;
            this.army = army;
             
            content.newLine();
            switch (player.hud.displays.CurrentMenuState)
            {

                default:
                    if (player.input.inputSource.IsController)
                    {
                        content.Add(new HUD.RichBox.RichBoxImage(player.input.ControllerFocus.Icon));
                        content.Add(new HUD.RichBox.RichBoxText(":"));
                        content.newLine();
                    }

                    var button = new HUD.RichBox.RichboxButton(
                    new List<AbsRichBoxMember>
                    {
                        new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_Halt),
                    },
                    new RbAction(halt), null);
                    button.addShortCutButton(player.input.Stop, false);
                    content.Add(button);

                    content.newLine();
                    var disbandButton = new HUD.RichBox.RichboxButton(
                        new List<AbsRichBoxMember>
                        {
                        new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_Disband),
                        },
                        new RbAction1Arg<string>(player.hud.displays.SetMenuState, DisbandMenuState, SoundLib.menu), 
                        null);
                    content.Add(disbandButton);

                    List<GameObject.Army> tradeAbleArmies = new List<GameObject.Army>();
                    DssRef.world.unitCollAreaGrid.collectArmies(player.faction, army.tilePos, 1,
                        tradeAbleArmies);
                    foreach (var ta in tradeAbleArmies)
                    {
                        if (ta != army && WP.birdDistance(army, ta) <= Army.MaxTradeDistance)
                        {
                            content.newLine();
                            var tradeButton = new HUD.RichBox.RichboxButton(
                                new List<AbsRichBoxMember>
                                {
                                new HUD.RichBox.RichBoxText(string.Format(DssRef.lang.ArmyOption_SendToX, ta.TypeName())),
                                },
                                new RbAction1Arg<Army>(startArmyTrade, ta, SoundLib.menu), null);
                            content.Add(tradeButton);
                        }
                    }

                    content.newLine();
                    var splitButton = new HUD.RichBox.RichboxButton(
                        new List<AbsRichBoxMember>
                        {
                                new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_Divide),
                        },
                        new RbAction1Arg<Army>(startArmyTrade, null, SoundLib.menu), null);
                    content.Add(splitButton);

                    break;

                case DisbandMenuState:
                    {
                        content.h2(DssRef.lang.ArmyOption_Disband);
                        var status = army.Status().getTypeCounts();

                        foreach (var kv in status)
                        {
                            content.newLine();
                            content.text(string.Format(DssRef.lang.ArmyOption_XGroupsOfType, kv.Value, DssRef.unitsdata.Name(kv.Key)));//kv.Key.ToString() + " groups: " + kv.Value);
                            content.newLine();
                            content.Button(string.Format(DssRef.lang.ArmyOption_RemoveX, 1),//"Remove 1",
                                new RbAction2Arg<UnitType, int>(army.disbandSoldiersAction, kv.Key, 1, SoundLib.menu),
                                null, true);

                            content.space();

                            content.Button(string.Format(DssRef.lang.ArmyOption_RemoveX, 5),//"Remove 5",
                                new RbAction2Arg<UnitType, int>(army.disbandSoldiersAction, kv.Key, 5, SoundLib.menu),
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
                    break;

                case DisbandAllMenuState:
                    content.h1(DssRef.lang.ArmyOption_DisbandAll);
                    content.h2(DssRef.lang.Hud_AreYouSure);
                    content.newLine();
                    var allbuttonyes = new HUD.RichBox.RichboxButton(
                        new List<AbsRichBoxMember>
                        {
                        new HUD.RichBox.RichBoxText(DssRef.lang.Hud_Yes),
                        },
                        new RbAction(disbandAllYes, SoundLib.menu), 
                        null);
                    content.Add(allbuttonyes);
                    break;

                case TradeMenuState:
                    {
                        if (player.hud.displays.otherArmy == null)
                        {
                            content.h2(DssRef.lang.ArmyOption_SendToNewArmy);
                        }
                        else
                        {
                            content.h2(string.Format(DssRef.lang.ArmyOption_SendToX, player.hud.displays.otherArmy.TypeName()));//"Send units to " + player.hud.displays.otherArmy.TypeName());
                        }
                        
                        var status = army.Status().getTypeCounts();
                        bool splitable = false;
                        foreach (var kv in status)
                        {
                            splitable |= kv.Value > 1;
                            content.newLine();
                            content.text(string.Format(DssRef.lang.ArmyOption_XGroupsOfType, kv.Value, DssRef.unitsdata.Name(kv.Key)));//kv.Key.ToString() + " groups: " + kv.Value);
                            content.newLine();
                            content.Button(string.Format(DssRef.lang.ArmyOption_SendX, 1),//"Send 1",
                                new RbAction2Arg<UnitType, int>(tradeSoldiersAction, kv.Key, 1, SoundLib.menu),
                                null, true);

                            content.space();

                            content.Button(string.Format(DssRef.lang.ArmyOption_SendX, 5),//"Send 5",
                                new RbAction2Arg<UnitType, int>(tradeSoldiersAction, kv.Key, 5, SoundLib.menu),
                                null,
                                kv.Value >= 5);

                            content.space();

                            content.Button(DssRef.lang.ArmyOption_SendAll,//"Send All",
                               new RbAction2Arg<UnitType, int>(tradeSoldiersAction, kv.Key, kv.Value, SoundLib.menu),
                               null, true);

                        }
                        content.newParagraph();

                        if (player.hud.displays.otherArmy == null)
                        {
                            var halfAndHalfbutton = new HUD.RichBox.RichboxButton(
                            new List<AbsRichBoxMember>
                            {
                                new HUD.RichBox.RichBoxText(DssRef.lang.ArmyOption_DivideHalf),
                            },
                            new RbAction(splitArmyInHalf, SoundLib.menu), null);
                            halfAndHalfbutton.enabled = splitable;
                            content.Add(halfAndHalfbutton);
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
                    break;
            }
            
        }

        void splitArmyInHalf()
        {
            player.hud.displays.otherArmy = null;

            var status = army.Status().getTypeCounts();
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
            //if (player.selmenu().otherArmy != null)
            //{
            //    var status = army.Status().getTypeCounts();
            //    foreach (var kv in status)
            //    {
            //        tradeSoldiersAction(otherArmy, kv.Key, kv.Value);                    
            //    }
            //}
        }

        void tradeSoldiersAction(UnitType type, int count)
        {
            army.tradeSoldiersAction(ref player.hud.displays.otherArmy, type, count);            
        }

        void startArmyTrade(Army toarmy)
        {
            player.hud.displays.otherArmy = toarmy;
            player.hud.displays.SetMenuState(TradeMenuState);
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
