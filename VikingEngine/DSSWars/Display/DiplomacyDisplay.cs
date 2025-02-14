using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Valve.Steamworks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.GO.PickUp;
using VikingEngine.ToGG.MoonFall;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Display
{
    class DiplomacyDisplay
    {
        Players.LocalPlayer player;
        DiplomaticRelation selectedRelation;
        Faction otherfaction;
        bool againstDark;

        public DiplomacyDisplay(Players.LocalPlayer player)
        { 
            this.player = player;
        }

        //public void refresh(Vector2 pos)
        //{
        //    interaction?.DeleteMe();
        //    interaction = null;

        //    bool selection;
        //    var faction = player.diplomacyMap.mainSelection(out selection);

        //    setVisible(faction != null);

        //    if (faction != null)
        //    {
        //        beginRefresh();
        //        toHud(faction, selection);
        //        endRefresh(pos, true);
        //    }
        //}

        public void toHud(RichBoxContent content, Faction faction, bool selection)
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
                FactionRelationDisplay(faction, selectedRelation.Relation, content);

                content.newLine();

                if (otherfaction.player.IsAi())
                {
                    playerToAi();
                }
                else
                {
                    playerToPlayer(content);
                }
            }

            void playerToAi()
            {
                if (selectedRelation.Relation == RelationType.RelationTypeN2_Truce)
                {
                    int sec = Convert.ToInt32(selectedRelation.RelationEnd_GameTimeSec - Ref.TotalGameTimeSec);
                    content.text(string.Format(DssRef.lang.Diplomacy_TruceTimeLength, sec));
                }

                content.text(string.Format(DssRef.lang.Diplomacy_SpeakTermIs, Diplomacy.SpeakTermsString(selectedRelation.SpeakTerms)));

                if (selectedRelation.SpeakTerms > SpeakTerms.SpeakTermsN2_None &&
                    faction.player.IsAi())
                {
                    content.newParagraph();
                    if (selectedRelation.Relation <= RelationType.RelationTypeN3_War)
                    {
                        content.Add(new ArtButton( RbButtonStyle.Primary,new List<AbsRichBoxMember>()
                        {
                            new RbImage(SpriteName.WarsRelationTruce),
                            new RbText(string.Format( DssRef.lang.Diplomacy_ForgeNewRelationTo, DssRef.lang.Diplomacy_RelationType_Truce)),//"Forge truce"),
                        },
                        new RbAction1Arg<bool>(peaceAction, false, SoundLib.menuBuy),
                        new RbTooltip(peaceTooltip, false),
                        canForgePeace(false)));

                    }
                    else if (selectedRelation.Relation == RelationType.RelationTypeN2_Truce)
                    {
                        content.newLine();

                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>()
                        {

                            new RbImage(SpriteName.WarsRelationTruce),
                            new RbText(DssRef.lang.Diplomacy_ExtendTruceAction),//"Extend truce"),
                        },
                            new RbAction(extendTruceAction, SoundLib.menuBuy),
                            new RbTooltip(extendTruceTooltip),
                            canExtendTruce()));
                    }

                    if (selectedRelation.Relation <= RelationType.RelationTypeN2_Truce)
                    {
                        content.newLine();

                        content.Add(new ArtButton(RbButtonStyle.Primary,new List<AbsRichBoxMember>()
                        {
                            new RbImage(SpriteName.WarsRelationPeace),
                            new RbText(string.Format( DssRef.lang.Diplomacy_ForgeNewRelationTo, DssRef.lang.Diplomacy_RelationType_Peace)),//"Forge peace"),
                        },
                            new RbAction1Arg<bool>(peaceAction, true, SoundLib.menuBuy),
                            new RbTooltip(peaceTooltip, true),
                            canForgePeace(true)));
                    }

                    if (selectedRelation.Relation == RelationType.RelationType0_Neutral ||
                        selectedRelation.Relation == RelationType.RelationType1_Peace)
                    {
                        content.newLine();

                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>()
                        {
                            new RbImage(SpriteName.WarsRelationGood),
                            new RbText(string.Format( DssRef.lang.Diplomacy_ForgeNewRelationTo, DssRef.lang.Diplomacy_RelationType_Good)),//"Forge good relations"),
                        },
                            new RbAction1Arg<bool>(allianceAction, false, SoundLib.menuBuy),
                            new RbTooltip(allianceTooltip, false),
                            canForgeAlliance(false)));
                    }

                    if (selectedRelation.Relation == RelationType.RelationType2_Good)
                    {
                        content.newLine();

                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>()
                            {
                                new RbImage(SpriteName.WarsRelationAlly),
                                new RbText(string.Format( DssRef.lang.Diplomacy_ForgeNewRelationTo, DssRef.lang.Diplomacy_RelationType_Ally)),//"Forge alliance"),
                            },
                            new RbAction1Arg<bool>(allianceAction, true, SoundLib.menuBuy),
                            new RbTooltip(allianceTooltip, true),
                            canForgeAlliance(true)));
                    }

                    if (selectedRelation.Relation == RelationType.RelationType3_Ally)
                    {
                        content.newLine();

                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>()
                            {
                                new RbText(DssRef.lang.Diplomacy_AbsorbServant),//"Absorb as servant"),
                            },
                            new RbAction(servantAction, SoundLib.menuBuy),
                            new RbTooltip(servantTooltip),
                            canMakeServant()));
                    }
                }

                if (againstDark)
                {
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.Diplomacy_LightSide));//"Is light side ally"));
                }

                if (player.diplomacyMap.previousFactionsLookedAt.Count > 1)
                {
                    content.newParagraph();
                    content.h2(DssRef.lang.Diplomacy_RelationWithOthers).overrideColor = HudLib.TitleColor_Label;

                    for (int i = 1; i < player.diplomacyMap.previousFactionsLookedAt.Count; i++)
                    {
                        content.newLine();
                        var thirdPartFaction = player.diplomacyMap.previousFactionsLookedAt[i];
                        var relation = DssRef.diplomacy.GetRelationType(otherfaction, thirdPartFaction);

                        content.Add(thirdPartFaction.FlagTextureToHud());
                        content.Add(new RbText(thirdPartFaction.PlayerName));

                        content.Add(new RbText(": "));
                        content.Add(new RbImage(Diplomacy.RelationSprite(relation)));
                        content.Add(new RbText(Diplomacy.RelationString(relation)));
                    }
                
                }
            }
        }

        public static void FactionRelationDisplay(Faction faction, RelationType relation, RichBoxContent content)
        {
            content.Add(new RbBeginTitle(2));
            content.Add(faction.FlagTextureToHud());
            content.Add(new RbText(faction.PlayerName));
            content.Add(new RbSeperationLine());

            HeadDisplay.FactionSize(faction, content, false);

            content.newParagraph();

            var relType = new RbText(DssRef.lang.Diplomacy_RelationType + ": ");
            relType.overrideColor = HudLib.TitleColor_TypeName;
            content.Add(relType);
            content.Add(new RbImage(Diplomacy.RelationSprite(relation)));
            content.Add(new RbText(Diplomacy.RelationString(relation)));
        }

        void playerToPlayer(RichBoxContent content)
        {
            var otherPlayer = otherfaction.player.GetLocalPlayer();

            var PtoP = player.toPlayerDiplomacies[otherPlayer.playerData.localPlayerIndex];

            if (PtoP.suggestingNewRelation)
            {
                //content.Add(new RichBoxText("New relation offered: "));
                content.Add(new RbImage(Diplomacy.RelationSprite(PtoP.suggestedRelation)));
                content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_NewRelationOffered, Diplomacy.RelationString(PtoP.suggestedRelation))));
                content.newLine();

                if (PtoP.suggestedBy == player.playerData.localPlayerIndex)
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary,new List<AbsRichBoxMember>()
                        {
                            //new RichBoxImage(SpriteName.WarsRelationPeace),
                            new RbText(Ref.langOpt.Hud_Cancel),
                        },
                        new RbAction(cancelToPlayerRelation, SoundLib.menuBuy)));
                }
                else
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary,new List<AbsRichBoxMember>()
                        {
                            //new RichBoxImage(SpriteName.WarsRelationPeace),
                            new RbText(DssRef.lang.Diplomacy_AcceptRelationOffer),
                        },
                       new RbAction(acceptToPlayerRelation, SoundLib.menuBuy)));
                }
            }
            else
            {
                if (selectedRelation.Relation <= RelationType.RelationTypeN2_Truce)
                {
                    content.newLine();

                    content.Add(new ArtButton(RbButtonStyle.Primary,new List<AbsRichBoxMember>()
                        {
                            new RbImage(SpriteName.WarsRelationPeace),
                            new RbText(DssRef.lang.Diplomacy_OfferPeace),
                        },
                        new RbAction(offerToPlayerRelation, SoundLib.menuBuy)));
                }
                else if (selectedRelation.Relation < RelationType.RelationType3_Ally)
                {
                    content.newLine();

                    content.Add(new ArtButton(RbButtonStyle.Primary,new List<AbsRichBoxMember>()
                        {
                            new RbImage(SpriteName.WarsRelationAlly),
                            new RbText(DssRef.lang.Diplomacy_OfferAlliance),
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
            message.Add(new RbImage(Diplomacy.RelationSprite(PtoP.suggestedRelation)));
            message.Add(new RbText(Diplomacy.RelationString(PtoP.suggestedRelation)));
            message.newLine();

            var acceptButtonContent = new List<AbsRichBoxMember>(7);
            otherPlayer.hud.messages.ControllerInputIcons(acceptButtonContent);
            acceptButtonContent.Add(new RbText(DssRef.lang.Diplomacy_AcceptRelationOffer));
            message.Add(new ArtButton(RbButtonStyle.Primary,
                acceptButtonContent,
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
        void extendTruceTooltip(RichBoxContent content, object tag)
        {
            int cost = Diplomacy.ExtendTruceCost();

            //RichBoxContent content = new RichBoxContent();

            diplomacyCostToHud(cost, content);
            //string truceDesc = "Extends truce by {0} seconds";
            content.text(string.Format(DssRef.lang.Diplomacy_TruceExtendTimeLength, DssLib.TruceTimeSec));

            //player.hud.tooltip.create(player, content, true);
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

        void peaceTooltip(RichBoxContent content, object tag)
        {
            bool peace_notTruce = (bool)tag;
            int cost = Diplomacy.EndWarCost(selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, peace_notTruce);
            RelationType toRelation = peace_notTruce ? RelationType.RelationType1_Peace : RelationType.RelationTypeN2_Truce;
            //RichBoxContent content = new RichBoxContent();

            diplomacyCostToHud(cost, content);

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            //string newRelationString = "New relation: ";

            //content.Add(new RichBoxText(newRelationString));
            content.Add(new RbImage(Diplomacy.RelationSprite(toRelation)));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_ForgeNewRelationTo, Diplomacy.RelationString(toRelation))));

            if (peace_notTruce == false)
            {
               // string truceTimeString = "For {0} seconds";
                content.text(string.Format(DssRef.lang.Diplomacy_TruceTimeLength, DssLib.TruceTimeSec));
            }

            //player.hud.tooltip.create(player, content, true);
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

        void allianceTooltip(RichBoxContent content, object tag)//bool ally_notFriend)
        {
            bool ally_notFriend = (bool)tag;
            int cost = Diplomacy.AllianceCost(selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, ally_notFriend);
            RelationType toRelation = ally_notFriend ? RelationType.RelationType3_Ally : RelationType.RelationType2_Good;

            //RichBoxContent content = new RichBoxContent();

            diplomacyCostToHud(cost, content);

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(Diplomacy.RelationSprite(toRelation)));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_ForgeNewRelationTo, Diplomacy.RelationString(toRelation))));
            
            if (ally_notFriend)
            {                
                content.text(DssRef.lang.Diplomacy_AllyDescription).overrideColor = HudLib.InfoYellow_Light;
                var opponents = otherfaction.CollectWars();
                foreach (var m in opponents)
                {
                    content.newLine();
                    HudLib.BulletPoint(content);
                   
                    var relation = DssRef.diplomacy.GetRelationType(otherfaction, m);
                    content.Add(new RbImage(Diplomacy.RelationSprite(relation)));
                    content.space();
                    content.Add(m.FlagTextureToHud());
                    content.Add(new RbText(m.PlayerName));
                }
            }
            else
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(DssRef.lang.Diplomacy_GoodRelationDescription));//"Limits the ability to declare war.");
            }

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbText( string.Format(DssRef.lang.Diplomacy_BreakingRelationCost, Diplomacy.DeclareWarCost(toRelation))));

            //player.hud.tooltip.create(player, content, true);
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
                player.diplomacyMap.cancel();
                //player.hud.needRefresh = true;
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

        void servantTooltip(RichBoxContent content, object tag)
        {
            int cost = Diplomacy.MakeServantCost(player, againstDark);
            
            //RichBoxContent content = new RichBoxContent();

            content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            
            HudLib.BulletPoint(content);
            {
               //string militaryStrength = "{0}x stronger military power";
                content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_ServantRequirement_XStrongerMilitary, Diplomacy.MiltitaryStrengthXServant)));
                content.newLine();
                //string militaryStrengthCompare = "Strength: Your {0} - Their {1}";
                //content.title("Military strength");
                //string 
                //content.newLine();
                content.Add(new RbText(string.Format(DssRef.lang.Hud_CompareMilitaryStrength_YourToOther, Convert.ToInt32(player.faction.militaryStrength), Convert.ToInt32(otherfaction.militaryStrength)), 
                    HudLib.ResourceCostColor(player.faction.militaryStrength >= otherfaction.militaryStrength * Diplomacy.MiltitaryStrengthXServant)));
                //content.text(faction.player.Name + ": " + Convert.ToInt32(faction.militaryStrength));
                content.newLine();
            }
            HudLib.BulletPoint(content);
            {
                string militaryStrength = DssRef.lang.Diplomacy_ServantRequirement_HopelessWar;
                content.Add(new RbText(militaryStrength, HudLib.ResourceCostColor(hasStrongerFoe())));
                content.newLine();
            }
            HudLib.BulletPoint(content);
            {
                string militaryStrength = DssRef.lang.Diplomacy_ServantRequirement_MaxCities;
                content.Add(new RbText(string.Format(militaryStrength, DssRef.diplomacy.ServantMaxCities), HudLib.ResourceCostColor(otherfaction.cities.Count <= DssRef.diplomacy.ServantMaxCities)));
                content.newLine();
            }

            //content.Add(HudLib.ResourceCost("Military strength", Convert.ToInt32(faction.militaryStrength * Diplomacy.MiltitaryStrengthXServant), Convert.ToInt32(player.faction.militaryStrength)));
            content.newLine();

            diplomacyCostToHud(cost, content);
            content.text(DssRef.lang.Diplomacy_ServantPriceWillRise);

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;
            
            content.text(DssRef.lang.Diplomacy_ServantGainAbsorbFaction);

            //player.hud.tooltip.create(player, content, true);
        }

        void diplomacyCostToHud(int cost, RichBoxContent content)
        {
            content.h2(DssRef.lang.Hud_PurchaseTitle_Cost).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            HudLib.ResourceCost(content, ResourceType.DiplomaticPoint, cost, player.diplomaticPoints.Int());
            content.newLine();
        }
    }
}
