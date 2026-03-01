using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using VikingEngine.DSSWars.Communication;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.GO.PickUp;
using VikingEngine.ToGG.MoonFall;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Interface
{
    

    class DiplomacyDisplay
    {
        static readonly RelationType[] RelationOptionsAsGod = { RelationType.RelationTypeN3_War, RelationType.RelationType1_Peace, RelationType.RelationType3_Ally };

        Players.LocalPlayer player;
        DiplomaticRelation selectedRelation;
        Faction otherfaction;
        bool againstDark;

        public DiplomacyDisplay(Players.LocalPlayer player)
        { 
            this.player = player;
        }

        public void quickSelect()
        {
            DiplomacyActionManager diplomacyActionManager = new DiplomacyActionManager();
            var options = diplomacyActionManager.diplomacyOptionsToBot(player, otherfaction);
            if (options.Count > 0)
            {
                commitDiplomacyAction(options.First().toRelation);
            }
        }


        public void toHud(RichBoxContent content, Faction botFaction, bool selection)
        {
            otherfaction = botFaction;

            selectedRelation = DssRef.diplomacy.GetRelation(player.faction, botFaction);//player.faction.diplomaticRelations[botFaction.myIndex];
            againstDark = botFaction.WantToAllyAgainstDark() && player.faction.diplomaticSide == DiplomaticSide.Light;
            //if (selectedRelation == null)
            //{
            //    selectedRelation = DssRef.diplomacy.SetRelationType(player.faction, botFaction, RelationType.RelationType0_Neutral, true);
            //}

            //if (selectedRelation != null)
            //{
                FactionRelationDisplay(botFaction, selectedRelation.Relation, content);

                content.newLine();
                if (DssRef.difficulty.setting_gameMode != Data.GameModeMainType.Spectator)
                {
                    if (otherfaction.player.IsBot())
                    {
                        playerToAi();
                    }
                    else
                    {
                        playerToPlayer(content);
                    }
                }
            //}
            
            if (player.gameControls.diplomacy.previousFactionsLookedAt.Count > 1)
            {
                content.newParagraph();
                content.h2(DssRef.lang.Diplomacy_RelationWithOthers, HudLib.TitleColor_Label);

                for (int i = 1; i < player.gameControls.diplomacy.previousFactionsLookedAt.Count; i++)
                {
                    content.newLine();
                    var thirdPartFaction = player.gameControls.diplomacy.previousFactionsLookedAt[i];
                    var relation = DssRef.diplomacy.GetRelation(otherfaction, thirdPartFaction).Relation;

                    content.Add(thirdPartFaction.FlagTextureToHud());
                    content.hspace();
                    content.Add(new RbText(thirdPartFaction.PlayerName));

                    content.Add(new RbText(": "));
                    content.Add(new RbImage(Diplomacy.RelationSprite(relation)));
                    content.Add(new RbText(Diplomacy.RelationString(relation)));

                    if (DssRef.difficulty.setting_gameMode == Data.GameModeMainType.Spectator)
                    {
                        content.space();
                        foreach (var forceRelation in RelationOptionsAsGod)
                        {
                            content.Add(new ArtButton( RbButtonStyle.GodPower,
                                new List<AbsRichBoxMember> { new RbImage(Diplomacy.RelationSprite(forceRelation)) },
                                new RbAction3Arg<RelationType, Faction, Faction>(setRelation_AsGod, forceRelation, otherfaction, thirdPartFaction),
                                new RbTooltip_Text(Diplomacy.RelationString(forceRelation)), true));
                    } }
                }

            }

            void CostDisplay(RichBoxContent content, int cost)
            {
                
                content.Add(new RbImage(SpriteName.WarsDiplomaticSub));
                content.hspace();
                content.Add(new RbText(cost.ToString()));
                content.space(2);
            }

            void playerToAi()
            {
                DiplomacyActionManager diplomacyActionManager = new DiplomacyActionManager();
                var options = diplomacyActionManager.diplomacyOptionsToBot(player, botFaction);

                if (selectedRelation.Relation == RelationType.RelationTypeN2_Truce)
                {
                    int sec = Convert.ToInt32(selectedRelation.RelationEnd_GameTimeSec.Seconds);
                    content.text(string.Format(DssRef.lang.Diplomacy_TruceTimeLength, sec));
                }

                content.text(string.Format(DssRef.lang.Diplomacy_SpeakTermIs, Diplomacy.SpeakTermsString(selectedRelation.SpeakTerms)));
               
                for (int i = 0; i < options.Count; ++i)
                {
                    var opt = options[i];
                   
                    content.newLine();
                    if (i == 0)
                    {
                        player.gameControls.input.QuickSelect.ToRichContent(content);
                        
                    }
                    content.Add(new RbTab(0.075f));
                    CostDisplay(content, opt.cost);

                    string forgeRelationString;

                    switch (opt.toRelation)
                    {
                        case RelationType.RelationTypeN3_War:
                            forgeRelationString = DssRef.lang.Hud_WardeclarationTitle;
                            break;
                        case RelationType.RelationType0_Neutral:
                            forgeRelationString = DssRef.lang.Diplomacy_EndRelations;
                            break;
                        default:
                            forgeRelationString = string.Format(DssRef.lang.Diplomacy_ForgeNewRelationTo, Diplomacy.RelationString(opt.toRelation));
                            break;
                    }

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>()
                        {
                            new RbImage( Diplomacy.RelationSprite(opt.toRelation)),
                            new RbSpace(), 
                            new RbText(forgeRelationString),
                        },
                       new RbAction1Arg<RelationType>(commitDiplomacyAction, opt.toRelation, RbSoundType.Buy),
                       new RbTooltip(relationTooltip, opt.toRelation), opt.available));                    
                }

#if DEBUG
                if (StartupSettings.EndlessDiplomacy)
                {
                    content.newParagraph();
                    content.Add(new RbButton(new List<AbsRichBoxMember>()
                        {
                            new RbText("*Merge"),
                        },
                      new RbAction(makeServant)));
                }
#endif
               
                if (againstDark)
                {
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.Diplomacy_LightSide));
                }
            }
        }

        void setRelation_AsGod(RelationType relation, Faction faction1, Faction faction2)
        {
            DssRef.diplomacy.SetRelationType(faction1, faction2, relation);
        }

        public static void FactionRelationDisplay(Faction faction, RelationType relation, RichBoxContent content)
        {
            content.Add(new RbBeginTitle(1));
            content.Add(faction.FlagTextureToHud());
            content.space(0.5f);
            content.Add(new RbImage(SpriteName.WarsGovernmentIcon));
            content.space(0.5f);
            content.Add(new RbText(faction.PlayerName, HudLib.TitleColor_Name));

            content.space(1);
            content.Add(new RbText(string.Format(DssRef.lang.UnitId, faction.myIndex), HudLib.SecondaryTextColor));

            content.Add(new RbSeperationLine());

            content.newLine();

            content.Add(new RbImage(SpriteName.rtsMoney));
            content.space();
            content.Add(new RbText(TextLib.LargeNumber(faction.money.GetGold())));

            content.space(2);


            content.Add(new RbImage(SpriteName.WarsWorker));
            content.space();
            content.Add(new RbText(TextLib.LargeNumber(faction.totalWorkForce)));

            content.space(2);

            content.Add(new RbImage(SpriteName.WarsStrengthIcon));
            content.space();
            content.Add(new RbText(TextLib.LargeNumber(Convert.ToInt32(faction.militaryStrength))));

            content.newParagraph();

            if (DssRef.difficulty.setting_gameMode != Data.GameModeMainType.Spectator)
            {
                var relType = new RbText(DssRef.lang.Diplomacy_RelationType + ": ");
                relType.overrideColor = HudLib.TitleColor_TypeName;
                content.Add(relType);
                content.Add(new RbImage(Diplomacy.RelationSprite(relation)));
                content.Add(new RbText(Diplomacy.RelationString(relation)));
            }
        }

        //public static void FactionSize(Faction faction, RichBoxContent content, bool fullDisplay)
        //{
        //    if (fullDisplay)
        //    {
        //        content.icontext(SpriteName.WarsWorker, DssRef.lang.ResourceType_Workers + ": " + TextLib.LargeNumber(faction.totalWorkForce));
        //        content.icontext(SpriteName.WarsStrengthIcon, string.Format(DssRef.lang.Hud_TotalStrengthRating, TextLib.LargeNumber(Convert.ToInt32(faction.militaryStrength))));
        //    }
        //    else
        //    {
        //        content.newLine();

        //        content.Add(new RbImage(SpriteName.rtsMoney));
        //        content.space();
        //        content.Add(new RbText(TextLib.LargeNumber(faction.money.GetGold())));

        //        content.space(2);

                
        //        content.Add(new RbImage(SpriteName.WarsWorker));
        //        content.space();
        //        content.Add(new RbText(TextLib.LargeNumber(faction.totalWorkForce)));

        //        content.space(2);

        //        content.Add(new RbImage(SpriteName.WarsStrengthIcon));
        //        content.space();
        //        content.Add(new RbText(TextLib.LargeNumber(Convert.ToInt32(faction.militaryStrength))));

                

                
        //    }
        //    content.newLine();
        //}

        void playerToPlayer(RichBoxContent content)
        {
            var otherPlayer = otherfaction.player.GetLocalPlayer();

            var PtoP = player.toPlayerDiplomacies[otherPlayer.playerData.localPlayerIndex];

            if (PtoP.suggestingNewRelation)
            {
                content.Add(new RbImage(Diplomacy.RelationSprite(PtoP.suggestedRelation)));
                content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_NewRelationOffered, Diplomacy.RelationString(PtoP.suggestedRelation))));
                content.newLine();

                if (PtoP.suggestedBy == player.playerData.localPlayerIndex)
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary,new List<AbsRichBoxMember>()
                        {
                            new RbText(Ref.langOpt.Hud_Cancel),
                        },
                        new RbAction(cancelToPlayerRelation, RbSoundType.Buy)));
                }
                else
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary,new List<AbsRichBoxMember>()
                        {
                            new RbText(DssRef.lang.Diplomacy_AcceptRelationOffer),
                        },
                       new RbAction(acceptToPlayerRelation, RbSoundType.Buy)));
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
                        new RbAction(offerToPlayerRelation, RbSoundType.Buy)));
                }
                else if (selectedRelation.Relation < RelationType.RelationType3_Ally)
                {
                    content.newLine();

                    content.Add(new ArtButton(RbButtonStyle.Primary,new List<AbsRichBoxMember>()
                        {
                            new RbImage(SpriteName.WarsRelationAlly),
                            new RbText(DssRef.lang.Diplomacy_OfferAlliance),
                        },
                        new RbAction(offerToPlayerRelation, RbSoundType.Buy)));
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
            MessageGroup_Ingame.ControllerInputIcons(player, acceptButtonContent);
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
                selectedRelation.RelationEnd_GameTimeSec.addTime(DssConst.TruceTimeSec);
                player.hud.needRefresh = true;
            }
        }
        void extendTruceTooltip(RichBoxContent content, object tag)
        {
            int cost = Diplomacy.ExtendTruceCost();

            diplomacyCostToHud(cost, content);
            content.text(string.Format(DssRef.lang.Diplomacy_TruceExtendTimeLength, DssConst.TruceTimeSec));
        }

        bool canExtendTruce(out int cost)
        {
            cost = Diplomacy.ExtendTruceCost();
            return player.diplomaticPoints.Int() >= cost;
        }

        void commitDiplomacyAction(RelationType toRelation)
        {
            switch (toRelation)
            {
                case RelationType.RelationTypeN2_Truce:
                    peaceAction(false);
                    break;
                case RelationType.ExtendTruce:
                    extendTruceAction();
                    break;
                case RelationType.RelationType1_Peace:
                    peaceAction(true);
                    break;
                case RelationType.RelationType2_Good:
                    allianceAction(false);
                    break;
                case RelationType.RelationType3_Ally:
                    allianceAction(true);
                    break;
                case RelationType.RelationType4_Servant:
                    servantAction();
                    break;
                case RelationType.RelationType0_Neutral:
                    DssRef.diplomacy.endRelations(player.faction, otherfaction);
                    break;
                case RelationType.RelationTypeN3_War:
                    DssRef.diplomacy.declareWar(player.faction, otherfaction);
                    break;
            }
        }



        void peaceAction(bool peace_notTruce)
        {
            int cost = Diplomacy.EndWarCost(otherfaction,selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, peace_notTruce);

            if (player.diplomaticPoints.pay(cost, false))
            {
                if (peace_notTruce)
                {
                    DssRef.diplomacy.SetRelationType(player.faction, otherfaction, RelationType.RelationType1_Peace);

                    selectedRelation.RelationEnd_GameTimeSec.setTimeFromNow(DssConst.PeaceSafeTimeSec.GetRandom());
                }
                else
                {
                    bool success = true;
                    if (DssRef.difficulty.UseTruceFailure(out float fail))
                    {
                        if (Ref.rnd.Chance(fail))
                        {
                            success = false;
                        }
                    }

                    if (success)
                    {
                        DssRef.diplomacy.SetRelationType(player.faction, otherfaction, RelationType.RelationTypeN2_Truce);

                        selectedRelation.RelationEnd_GameTimeSec.setTimeFromNow(DssConst.TruceTimeSec);
                    }
                    else
                    {
                        ref var relation = ref DssRef.diplomacy.GetRefRelation(player.faction.myIndex, otherfaction.myIndex);
                        relation.SpeakTerms--;
                        if (relation.SpeakTerms < SpeakTerms.SpeakTermsN2_None)
                        {
                            relation.SpeakTerms = SpeakTerms.SpeakTermsN2_None;
                        }

                        SoundLib.wrong.Play();
                    }
                }
            }
        }

        void relationTooltip( RichBoxContent content, object tag)
        {
            RelationType relationType = (RelationType)tag;
            switch (relationType)
            {
                case RelationType.RelationTypeN2_Truce:
                    peaceTooltip(content, false);
                    break;
                case RelationType.ExtendTruce:
                    extendTruceTooltip(content, null);
                    break;
                case RelationType.RelationType1_Peace:
                    peaceTooltip(content, true);
                    break;
                case RelationType.RelationType2_Good:
                    allianceTooltip(content, false);
                    break;
                case RelationType.RelationType3_Ally:
                    allianceTooltip(content, true);
                    break;
                case RelationType.RelationType4_Servant:
                    servantTooltip(content, null);
                    break;

                case RelationType.RelationType0_Neutral:
                    endRelationsTooltip(content, null);
                    break;
                case RelationType.RelationTypeN3_War:
                    declareWarTooltip(content, null);
                    break;
            }
        }

        void endRelationsTooltip(RichBoxContent content, object tag)
        {
            int cost = Diplomacy.EndRelationCost(selectedRelation.Relation);

            diplomacyCostToHud(cost, content, true);

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            content.Add(new RbImage(Diplomacy.RelationSprite(RelationType.RelationType0_Neutral)));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_ForgeNewRelationTo, Diplomacy.RelationString(RelationType.RelationType0_Neutral))));
        }

        void declareWarTooltip(RichBoxContent content, object tag)
        {
            int cost = Diplomacy.DeclareWarCost(selectedRelation.Relation);
            
            diplomacyCostToHud(cost, content, true);

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            content.Add(new RbImage(Diplomacy.RelationSprite( RelationType.RelationTypeN3_War)));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_ForgeNewRelationTo, Diplomacy.RelationString( RelationType.RelationTypeN3_War))));
        }

        void peaceTooltip(RichBoxContent content, object tag)
        {
            bool peace_notTruce = (bool)tag;
            int cost = Diplomacy.EndWarCost(otherfaction, selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, peace_notTruce);
            RelationType toRelation = peace_notTruce ? RelationType.RelationType1_Peace : RelationType.RelationTypeN2_Truce;
            
            diplomacyCostToHud(cost, content);

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            content.Add(new RbImage(Diplomacy.RelationSprite(toRelation)));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_ForgeNewRelationTo, Diplomacy.RelationString(toRelation))));

            if (peace_notTruce == false)
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_TruceTimeLength, DssConst.TruceTimeSec)));

                if (DssRef.difficulty.UseTruceFailure(out float failChance))
                {
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.cmdWarningTriangle));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.lang.Event_ChanceOfFailure, TextLib.PercentText(failChance))));
                }
            }
        }

        void allianceAction(bool ally_notFriend)
        {
            int cost = Diplomacy.AllianceCost(player, otherfaction, selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, ally_notFriend, out _);

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

                    selectedRelation.RelationEnd_GameTimeSec.setTimeFromNow(DssConst.PeaceSafeTimeSec.GetRandom());
                }

                player.hud.needRefresh = true;
            }
        }

        void allianceTooltip(RichBoxContent content, object tag)
        {
            bool ally_notFriend = (bool)tag;
            int cost = Diplomacy.AllianceCost(player, otherfaction, selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, ally_notFriend, out int allyCountCost);
            RelationType toRelation = ally_notFriend ? RelationType.RelationType3_Ally : RelationType.RelationType2_Good;

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
                if (opponents.Count > 0)
                {
                    content.newLine();
                    content.h2(DssRef.lang.Diplomacy_DeclareWarAgainst, HudLib.TitleColor_Label);
                }
                foreach (var m in opponents)
                {
                    content.newLine();
                    HudLib.BulletPoint(content);
                   
                    var relation = DssRef.diplomacy.GetRelation(otherfaction, m).Relation;
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
                content.Add(new RbText(DssRef.lang.Diplomacy_GoodRelationDescription));
            }

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_BreakingRelationCost, Diplomacy.DeclareWarCost(toRelation))));

            if (ally_notFriend && DssRef.difficulty.AllyCountCost())
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_CostPerAlly, DssConst.DiplomacyExtraCostPerAlly)));
                
                content.newLine();
                content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Diplomacy_AllyCount, player.allyCount), HudLib.InfoYellow_Light));
            }
        }

        void servantAction()
        {
            if (canMakeServant(out int cost) &&
                player.diplomaticPoints.pay(cost, false))
            {
                makeServant();
            }
        }

        public void makeServant()
        { 
            ++player.statistics.ServantFactions;
            otherfaction.mergeTo(player.faction);
            player.gameControls.diplomacy?.cancel();
        }

        bool canMakeServant(out int cost)
        {
            cost = Diplomacy.MakeServantCost(player, againstDark);

            return selectedRelation.Relation == RelationType.RelationType3_Ally &&
                player.faction.militaryStrength >= Diplomacy.MiltitaryStrengthXServant * otherfaction.militaryStrength && 
                player.diplomaticPoints.Int() >= cost &&
                otherfaction.cities.Count <= DssRef.diplomacy.ServantMaxCities &&
                hasStrongerFoe();
        }

        bool hasStrongerFoe()
        {
            List<int> wars = new List<int>(8);
            DssRef.diplomacy.collectWars(otherfaction, wars);

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

            content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            
            HudLib.BulletPoint(content);
            {
                content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_ServantRequirement_XStrongerMilitary, Diplomacy.MiltitaryStrengthXServant)));
                content.newLine();
                content.Add(new RbText(string.Format(DssRef.lang.Hud_CompareMilitaryStrength_YourToOther, Convert.ToInt32(player.faction.militaryStrength), Convert.ToInt32(otherfaction.militaryStrength)), 
                    HudLib.ResourceCostColor(player.faction.militaryStrength >= otherfaction.militaryStrength * Diplomacy.MiltitaryStrengthXServant)));
                
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

            content.newLine();

            diplomacyCostToHud(cost, content);
            content.text(DssRef.lang.Diplomacy_ServantPriceWillRise);

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;
            
            content.text(DssRef.lang.Diplomacy_ServantGainAbsorbFaction);
        }

        void diplomacyCostToHud(int cost, RichBoxContent content, bool allowNegative = false)
        {
            content.h2(DssRef.lang.Hud_PurchaseTitle_Cost).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            if (allowNegative)
            {
                content.Add(new RbImage(SpriteName.WarsDiplomaticSub));
                content.space(0.5f);
                content.Add(new RbText(string.Format(DssRef.lang.Hud_Purchase_ResourceCost, DssRef.lang.ResourceType_DiplomacyPoints, cost)));
         
            }
            else
            {
                HudLib.ResourceCost(content, ResourceType.DiplomaticPoint, cost, player.diplomaticPoints.Int());
            }
            content.newLine();
        }
    }
}
