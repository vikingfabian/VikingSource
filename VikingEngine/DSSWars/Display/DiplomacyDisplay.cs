using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Valve.Steamworks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.GO.PickUp;
using VikingEngine.ToGG.MoonFall;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Display
{
    class DiplomacyDisplay : RichboxGuiPart
    {
        Players.LocalPlayer player;
        DiplomaticRelation selectedRelation;
        Faction otherfaction;
        bool againstDark;

        public DiplomacyDisplay(RichboxGui gui, Players.LocalPlayer player)
           : base(gui)
        { 
            this.player = player;
        }

        public void refresh(Vector2 pos)
        {
            interaction?.DeleteMe();
            interaction = null;

            bool selection;
            var faction = player.diplomacyMap.mainSelection(out selection);

            setVisible(faction != null);

            if (faction != null)
            {
                beginRefresh();
                toHud(faction, selection);
                endRefresh(pos, true);
            }
        }

        void toHud(Faction faction, bool selection)
        {
            otherfaction = faction;
            selectedRelation = player.faction.diplomaticRelations[faction.parentArrayIndex];
            againstDark = faction.WantToAllyAgainstDark() && player.faction.diplomaticSide == DiplomaticSide.Light;
            if (selectedRelation == null)
            {
               selectedRelation = DssRef.diplomacy.SetRelationType(player.faction, faction, RelationType.RelationType0_Neutral, true);
            }

            if ( selectedRelation!= null)
            {

                content.Add(new RichBoxBeginTitle(2));
                content.Add(faction.FlagTextureToHud());
                content.Add(new RichBoxText(faction.PlayerName));
                content.Add(new RichBoxSeperationLine());

               
                //content.text(string.Format(relation, Diplomacy.RelationString(selectedRelation.Relation)));

                content.Add(new RichBoxText(DssRef.lang.Diplomacy_RelationType + " :"));
                content.Add(new RichBoxImage(Diplomacy.RelationSprite(selectedRelation.Relation)));
                content.Add(new RichBoxText(Diplomacy.RelationString(selectedRelation.Relation)));

                content.newLine();

                if (otherfaction.player.IsAi())
                {
                    playerToAi();
                }
                else
                {
                    playerToPlayer();
                }
            }

            void playerToAi()
            {
                if (selectedRelation.Relation == RelationType.RelationTypeN2_Truce)
                {
                    //string truceLength = "Ends in {0} seconds";
                    int sec = Convert.ToInt32(selectedRelation.RelationEnd_GameTimeSec - Ref.TotalGameTimeSec);
                    content.text(string.Format(DssRef.lang.Diplomacy_TruceTimeLength, sec));
                }

               // string speakTerms = "Speaking terms: {0}";
                content.text(string.Format(DssRef.lang.Diplomacy_SpeakTermIs, Diplomacy.SpeakTermsString(selectedRelation.SpeakTerms)));

                if (selectedRelation.SpeakTerms > SpeakTerms.SpeakTermsN2_None &&
                    faction.player.IsAi())
                {
                    content.newParagraph();
                    if (selectedRelation.Relation <= RelationType.RelationTypeN3_War)
                    {
                        content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                        {
                            new RichBoxImage(SpriteName.WarsRelationTruce),
                            new RichBoxText(string.Format( DssRef.lang.Diplomacy_ForgeNewRelationTo, DssRef.lang.Diplomacy_RelationType_Truce)),//"Forge truce"),
                        },
                            new RbAction1Arg<bool>(peaceAction, false, SoundLib.menuBuy),
                            new RbAction1Arg<bool>(peaceTooltip, false),
                            canForgePeace(false)));

                    }
                    else if (selectedRelation.Relation == RelationType.RelationTypeN2_Truce)
                    {
                        content.newLine();

                        content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                        {

                            new RichBoxImage(SpriteName.WarsRelationTruce),
                            new RichBoxText(DssRef.lang.Diplomacy_ExtendTruceAction),//"Extend truce"),
                        },
                            new RbAction(extendTruceAction, SoundLib.menuBuy),
                            new RbAction(extendTruceTooltip),
                            canExtendTruce()));
                    }

                    if (selectedRelation.Relation <= RelationType.RelationTypeN2_Truce)
                    {
                        content.newLine();

                        content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                        {
                            new RichBoxImage(SpriteName.WarsRelationPeace),
                            new RichBoxText(string.Format( DssRef.lang.Diplomacy_ForgeNewRelationTo, DssRef.lang.Diplomacy_RelationType_Peace)),//"Forge peace"),
                        },
                            new RbAction1Arg<bool>(peaceAction, true, SoundLib.menuBuy),
                            new RbAction1Arg<bool>(peaceTooltip, true),
                            canForgePeace(true)));
                    }

                    if (selectedRelation.Relation == RelationType.RelationType0_Neutral ||
                        selectedRelation.Relation == RelationType.RelationType1_Peace)
                    {
                        content.newLine();

                        content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                        {
                            new RichBoxImage(SpriteName.WarsRelationGood),
                            new RichBoxText(string.Format( DssRef.lang.Diplomacy_ForgeNewRelationTo, DssRef.lang.Diplomacy_RelationType_Good)),//"Forge good relations"),
                        },
                            new RbAction1Arg<bool>(allianceAction, false, SoundLib.menuBuy),
                            new RbAction1Arg<bool>(allianceTooltip, false),
                            canForgeAlliance(false)));
                    }

                    if (selectedRelation.Relation == RelationType.RelationType2_Good)
                    {
                        content.newLine();

                        content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                            {
                                new RichBoxImage(SpriteName.WarsRelationAlly),
                                new RichBoxText(string.Format( DssRef.lang.Diplomacy_ForgeNewRelationTo, DssRef.lang.Diplomacy_RelationType_Ally)),//"Forge alliance"),
                            },
                            new RbAction1Arg<bool>(allianceAction, true, SoundLib.menuBuy),
                            new RbAction1Arg<bool>(allianceTooltip, true),
                            canForgeAlliance(true)));
                    }

                    if (selectedRelation.Relation == RelationType.RelationType3_Ally)
                    {
                        content.newLine();

                        content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                            {
                                new RichBoxText(DssRef.lang.Diplomacy_AbsorbServant),//"Absorb as servant"),
                            },
                            new RbAction(servantAction, SoundLib.menuBuy),
                            new RbAction(servantTooltip),
                            canMakeServant()));
                    }
                }

                if (againstDark)
                {
                    content.newLine();
                    content.ListDot();
                    content.Add(new RichBoxText(DssRef.lang.Diplomacy_LightSide));//"Is light side ally"));
                }
            }
        }

        void playerToPlayer()
        {
            var otherPlayer = otherfaction.player.GetLocalPlayer();

            var PtoP = player.toPlayerDiplomacies[otherPlayer.playerData.localPlayerIndex];

            if (PtoP.suggestingNewRelation)
            {
                //content.Add(new RichBoxText("New relation offered: "));
                content.Add(new RichBoxImage(Diplomacy.RelationSprite(PtoP.suggestedRelation)));
                content.Add(new RichBoxText(string.Format(DssRef.lang.Diplomacy_NewRelationOffered, Diplomacy.RelationString(PtoP.suggestedRelation))));
                content.newLine();

                if (PtoP.suggestedBy == player.playerData.localPlayerIndex)
                {
                    content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                        {
                            //new RichBoxImage(SpriteName.WarsRelationPeace),
                            new RichBoxText(DssRef.lang.Hud_Cancel),
                        },
                        new RbAction(cancelToPlayerRelation, SoundLib.menuBuy)));
                }
                else
                {
                    content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                        {
                            //new RichBoxImage(SpriteName.WarsRelationPeace),
                            new RichBoxText(DssRef.lang.Diplomacy_AcceptRelationOffer),
                        },
                       new RbAction(acceptToPlayerRelation, SoundLib.menuBuy)));
                }
            }
            else
            {
                if (selectedRelation.Relation <= RelationType.RelationTypeN2_Truce)
                {
                    content.newLine();

                    content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                        {
                            new RichBoxImage(SpriteName.WarsRelationPeace),
                            new RichBoxText(DssRef.lang.Diplomacy_OfferPeace),
                        },
                        new RbAction(offerToPlayerRelation, SoundLib.menuBuy)));
                }
                else if (selectedRelation.Relation < RelationType.RelationType3_Ally)
                {
                    content.newLine();

                    content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                        {
                            new RichBoxImage(SpriteName.WarsRelationAlly),
                            new RichBoxText(DssRef.lang.Diplomacy_OfferAlliance),
                        },
                        new RbAction(offerToPlayerRelation, SoundLib.menuBuy)));
                }
            }
        }

        void offerToPlayerRelation()
        {
            var otherPlayer = otherfaction.player.GetLocalPlayer();
            var PtoP = player.toPlayerDiplomacies[otherPlayer.playerData.localPlayerIndex];

            PtoP.suggestingNewRelation = true;

            if (selectedRelation.Relation <= RelationType.RelationTypeN2_Truce)
            {
                PtoP.suggestedRelation = RelationType.RelationType1_Peace;
            }
            else
            {
                PtoP.suggestedRelation = RelationType.RelationType3_Ally;
            }

            PtoP.suggestedBy = player.playerData.localPlayerIndex;


            var message = new RichBoxContent();
            message.h1(string.Format(DssRef.lang.Diplomacy_PlayerOfferAlliance, player.Name));
            message.newLine();
            message.Add(new RichBoxImage(Diplomacy.RelationSprite(PtoP.suggestedRelation)));
            message.Add(new RichBoxText(Diplomacy.RelationString(PtoP.suggestedRelation)));
            message.newLine();
            message.Add(new RichboxButton(new List<AbsRichBoxMember>
                {
                    new RichBoxText(DssRef.lang.Diplomacy_AcceptRelationOffer)
                },
                new RbAction(acceptToPlayerRelation)));
            otherPlayer.hud.messages.Add(message);
        }

        void acceptToPlayerRelation()
        {
            var otherPlayer = otherfaction.player.GetLocalPlayer();
            var PtoP = player.toPlayerDiplomacies[otherPlayer.playerData.localPlayerIndex];

            if (PtoP.suggestingNewRelation)
            { 
                DssRef.diplomacy.SetRelationType(player.faction, otherfaction, PtoP.suggestedRelation);
            }

            PtoP.suggestingNewRelation = false;
        }

        void cancelToPlayerRelation()
        {
            var otherPlayer = otherfaction.player.GetLocalPlayer();
            var PtoP = player.toPlayerDiplomacies[otherPlayer.playerData.localPlayerIndex];

            PtoP.suggestingNewRelation = false;
        }

        void extendTruceAction()
        {
            int cost = Diplomacy.ExtendTruceCost();
            if (player.diplomaticPoints.pay(cost, false))
            {
                selectedRelation.RelationEnd_GameTimeSec += DssLib.TruceTimeSec;
                player.hud.needRefresh = true;
            }
        }
        void extendTruceTooltip() 
        {
            int cost = Diplomacy.ExtendTruceCost();

            RichBoxContent content = new RichBoxContent();

            diplomacyCostToHud(cost, content);
            //string truceDesc = "Extends truce by {0} seconds";
            content.text(string.Format(DssRef.lang.Diplomacy_TruceExtendTimeLength, DssLib.TruceTimeSec));

            player.hud.tooltip.create(player, content, true);
        }

        bool canExtendTruce()
        {
            return player.diplomaticPoints.Int() >= Diplomacy.ExtendTruceCost();
        }

        void peaceAction(bool peace_notTruce)
        {
            //stoppa armeer med attack command
            //Faction otherfaction = selectedRelation.opponent(player.faction);
            int cost = Diplomacy.EndWarCost(selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, peace_notTruce);

            if (player.diplomaticPoints.pay(cost, false))
            {               
                if (peace_notTruce)
                {
                    DssRef.diplomacy.SetRelationType(player.faction, otherfaction, RelationType.RelationType1_Peace);
                }
                else
                {
                    DssRef.diplomacy.SetRelationType(player.faction, otherfaction, RelationType.RelationTypeN2_Truce);
                    
                    selectedRelation.RelationEnd_GameTimeSec = Ref.TotalGameTimeSec + DssLib.TruceTimeSec;
                }
            }
        }

        bool canForgePeace(bool peace_notTruce)
        { 
            
            return player.diplomaticPoints.Int() >= Diplomacy.EndWarCost(selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, peace_notTruce);
        }

        void peaceTooltip(bool peace_notTruce)
        {
            int cost = Diplomacy.EndWarCost(selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, peace_notTruce);
            RelationType toRelation = peace_notTruce ? RelationType.RelationType1_Peace : RelationType.RelationTypeN2_Truce;
            RichBoxContent content = new RichBoxContent();

            diplomacyCostToHud(cost, content);

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
            content.newLine();
            //string newRelationString = "New relation: ";

            //content.Add(new RichBoxText(newRelationString));
            content.Add(new RichBoxImage(Diplomacy.RelationSprite(toRelation)));
            content.Add(new RichBoxText(string.Format(DssRef.lang.Diplomacy_ForgeNewRelationTo, Diplomacy.RelationString(toRelation))));

            if (peace_notTruce == false)
            {
               // string truceTimeString = "For {0} seconds";
                content.text(string.Format(DssRef.lang.Diplomacy_TruceTimeLength, DssLib.TruceTimeSec));
            }

            player.hud.tooltip.create(player, content, true);
        }

        void allianceAction(bool ally_notFriend)
        {
            //Faction otherfaction = selectedRelation.opponent(player.faction);
            int cost = Diplomacy.AllianceCost(selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, ally_notFriend);

            if (player.diplomaticPoints.pay(cost, false))
            {
                if (ally_notFriend)
                {
                    ++player.statistics.AlliedFactions;
                    DssRef.diplomacy.SetRelationType(player.faction, otherfaction, RelationType.RelationType3_Ally);
                }
                else
                {
                    DssRef.diplomacy.SetRelationType(player.faction, otherfaction, RelationType.RelationType2_Good);
                }

                player.hud.needRefresh = true;
            }
        }

        bool canForgeAlliance(bool ally_notFriend)
        {
            return player.diplomaticPoints.Int() >= Diplomacy.AllianceCost(selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, ally_notFriend);
        }

        void allianceTooltip(bool ally_notFriend)
        {
            int cost = Diplomacy.AllianceCost(selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, ally_notFriend);
            RelationType toRelation = ally_notFriend ? RelationType.RelationType3_Ally : RelationType.RelationType2_Good;

            RichBoxContent content = new RichBoxContent();

            diplomacyCostToHud(cost, content);

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
            content.newLine();
            //string newRelationString = "New relation: ";

            //content.Add(new RichBoxText(newRelationString));
            content.Add(new RichBoxImage(Diplomacy.RelationSprite(toRelation)));
            content.Add(new RichBoxText(string.Format(DssRef.lang.Diplomacy_ForgeNewRelationTo, Diplomacy.RelationString(toRelation))));
            //content.text(string.Format(newRelationString, Diplomacy.RelationString(toRelation)));

            if (ally_notFriend)
            {
                content.text(DssRef.lang.Diplomacy_AllyDescription);//"Allies share war declarations.");
            }
            else
            {
                content.text(DssRef.lang.Diplomacy_GoodRelationDescription);//"Limits the ability to declare war.");
            }
            content.text(string.Format(DssRef.lang.Diplomacy_BreakingRelationCost, Diplomacy.DeclareWarCost(toRelation)));

            player.hud.tooltip.create(player, content, true);
        }

        void servantAction()
        {
            //Faction otherfaction = selectedRelation.opponent(player.faction);
            int cost = Diplomacy.MakeServantCost(player, againstDark);

            if (canMakeServant() &&
                player.diplomaticPoints.pay(cost, false))
            {
                ++player.statistics.ServantFactions;
                otherfaction.mergeTo(player.faction);

                player.hud.needRefresh = true;
            }
        }

        bool canMakeServant()
        {
            return selectedRelation.Relation == RelationType.RelationType3_Ally &&
                player.faction.militaryStrength >= Diplomacy.MiltitaryStrengthXServant * otherfaction.militaryStrength && 
                player.diplomaticPoints.Int() >= Diplomacy.MakeServantCost(player, againstDark) &&
                otherfaction.cities.Count <= DssRef.diplomacy.ServantMaxCities &&
                hasStrongerFoe();
        }

        bool hasStrongerFoe()
        {
            var wars = DssRef.diplomacy.collectWars(otherfaction);

            foreach (var w in wars)
            {
                if (DssRef.world.factions[w].militaryStrength > otherfaction.militaryStrength * 1.2f)
                { 
                    return true;
                }
            }
            return false;
        }

        void servantTooltip()
        {
            int cost = Diplomacy.MakeServantCost(player, againstDark);
            
            RichBoxContent content = new RichBoxContent();

            content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement);
            content.newLine();
            
            content.ListDot();
            {
               //string militaryStrength = "{0}x stronger military power";
                content.Add(new RichBoxText(string.Format(DssRef.lang.Diplomacy_ServantRequirement_XStrongerMilitary, Diplomacy.MiltitaryStrengthXServant)));
                content.newLine();
                //string militaryStrengthCompare = "Strength: Your {0} - Their {1}";
                //content.title("Military strength");
                //string 
                //content.newLine();
                content.Add(new RichBoxText(string.Format(DssRef.lang.Hud_CompareMilitaryStrength_YourToOther, Convert.ToInt32(player.faction.militaryStrength), Convert.ToInt32(otherfaction.militaryStrength)), 
                    HudLib.ResourceCostColor(player.faction.militaryStrength >= otherfaction.militaryStrength * Diplomacy.MiltitaryStrengthXServant)));
                //content.text(faction.player.Name + ": " + Convert.ToInt32(faction.militaryStrength));
                content.newLine();
            }
            content.ListDot();
            {
                string militaryStrength = DssRef.lang.Diplomacy_ServantRequirement_HopelessWar;
                content.Add(new RichBoxText(militaryStrength, HudLib.ResourceCostColor(hasStrongerFoe())));
                content.newLine();
            }
            content.ListDot();
            {
                string militaryStrength = DssRef.lang.Diplomacy_ServantRequirement_MaxCities;
                content.Add(new RichBoxText(string.Format(militaryStrength, DssRef.diplomacy.ServantMaxCities), HudLib.ResourceCostColor(otherfaction.cities.Count <= DssRef.diplomacy.ServantMaxCities)));
                content.newLine();
            }

            //content.Add(HudLib.ResourceCost("Military strength", Convert.ToInt32(faction.militaryStrength * Diplomacy.MiltitaryStrengthXServant), Convert.ToInt32(player.faction.militaryStrength)));
            content.newLine();

            diplomacyCostToHud(cost, content);
            content.text(DssRef.lang.Diplomacy_ServantPriceWillRaise);

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
            
            content.text(DssRef.lang.Diplomacy_ServantGainAbsorbFaction);

            player.hud.tooltip.create(player, content, true);
        }

        void diplomacyCostToHud(int cost, RichBoxContent content)
        {
            content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
            content.newLine();
            HudLib.ResourceCost(content, GameObject.Resource.ResourceType.DiplomaticPoint, cost, player.diplomaticPoints.Int());
            content.newLine();
        }
    }
}
